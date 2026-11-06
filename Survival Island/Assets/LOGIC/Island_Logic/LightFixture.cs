using System;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Bahan bakar dan interaksi Obor Dinding serta Lampu. Struktur base yang memasang LightSource dan Flammable.
    /// Blueprint: Island_Logic/LightSourceSystem.md bagian "Interaksi LightFixture" dan "Waktu dan bahan bakar" (#98).
    /// Usulan yang disetujui developer (#85): Obor Dinding memakai Mat_Wood, Lampu memakai Item_Battery; badai mematikan
    /// lampu tak beratap, Lampu menyala lagi otomatis dan Obor Dinding harus dinyalakan pemain.
    ///
    /// Catatan implementasi (sistem pendukung):
    /// - IslandInteractable (#17) belum ada: IInteractable langsung (diizinkan IslandInteractable.md bagian "Jenis objek").
    /// - ISaveable lewat StructureRecord.extraJson (pola CampfireManager), SaveId = StructureId, LoadOrder 100.
    /// - "Tahan E 0,6 detik menambah bahan bakar" belum ditulis: PlayerInteractor hanya mengenal satu HoldSeconds per target,
    ///   jadi tekan dan tahan tidak bisa dibedakan tanpa mengubah PlayerInteractor (kode lama). Sementara, menekan E saat mati
    ///   dan bahan bakar belum penuh juga menambah 1 item sebelum menyala. TODO(PlayerInteractor #17).
    /// </summary>
    public class LightFixture : MonoBehaviour, IInteractable, ISaveable
    {
        public static readonly List<LightFixture> All = new List<LightFixture>(24);

        [SerializeField] private LightFixtureData data;
        [SerializeField] private LightSource lightSource;
        [SerializeField] private Transform skyProbe;
        [SerializeField] private InteractHighlight highlight;

        private float fuelSeconds;
        private float lastTickGameHour = -1f;
        private float tickTimer;
        private bool isRoofed;
        private bool stormOff;      // Dimatikan badai; Lampu menyala lagi otomatis
        private bool destroyedHandled;
        private RegionType homeRegion;
        private bool homeRegionSet;

        public bool IsLit { get; private set; }
        public float FuelSeconds => fuelSeconds;
        public LightFixtureData Data => data;

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;

        private string StructureId
        {
            get
            {
                var h = GetComponentInParent<IStructureHealth>();
                return h != null ? h.StructureId : string.Empty;
            }
        }

        // ---------------------------------------------------------------- Siklus hidup

        private void Start()
        {
            if (lastTickGameHour < 0f) lastTickGameHour = NowGameHours();
            EnsureHomeRegion();
            RefreshRoof();
            ApplyLit();
        }

        private void OnEnable()
        {
            if (!All.Contains(this)) All.Add(this);
            EventChannel<StructureDestroyedEvent>.Raised += HandleStructureDestroyed;
            EventChannel<StructureBuiltEvent>.Raised += HandleStructureBuilt;
            // Region kembali dimuat: sim mengejar waktu yang lewat lewat lastTickGameHour.
            if (lastTickGameHour >= 0f) { RefreshRoof(); Simulate(); }
            ApplyLit();
        }

        private void OnDisable()
        {
            All.Remove(this);
            EventChannel<StructureDestroyedEvent>.Raised -= HandleStructureDestroyed;
            EventChannel<StructureBuiltEvent>.Raised -= HandleStructureBuilt;
        }

        private void EnsureHomeRegion()
        {
            if (homeRegionSet) return;
            homeRegion = RegionStreaming.RegionOf(transform.position);
            homeRegionSet = true;
        }

        private static float NowGameHours() => DayNightDirector.Instance != null ? (float)DayNightDirector.Instance.TotalGameHours : 0f;

        // ---------------------------------------------------------------- Waktu dan bahan bakar

        private void Update()
        {
            if (Time.deltaTime <= 0f) return;
            tickTimer += Time.deltaTime;
            float interval = Balance != null ? Balance.light.fixtureTickSeconds : 1f;
            if (tickTimer < interval) return;
            tickTimer = 0f;
            Simulate();
        }

        private void Simulate()
        {
            float now = NowGameHours();
            float multiplier = Balance != null ? Balance.time.timeMultiplier : 96f;
            // Rumus CampfireManager: detik setara, lebih besar saat tidur (#67).
            float sim = (now - lastTickGameHour) * 3600f / Mathf.Max(0.0001f, multiplier);
            lastTickGameHour = now;
            if (sim <= 0f || data == null) return;
            var b = Balance != null ? Balance.light : null;

            // 1.
            WeatherType w = RegionalWeather.Instance != null ? RegionalWeather.Instance.GetWeather(homeRegion) : WeatherType.Clear;
            bool storm = w == WeatherType.Storm || w == WeatherType.Blizzard;
            bool stormKills = b == null || b.stormExtinguishes;

            // Lampu menyala lagi otomatis saat cuaca membaik.
            if (!IsLit && stormOff && (!storm || isRoofed))
            {
                stormOff = false;
                if (data.isLamp && fuelSeconds > 0f) SetLit(true);
            }
            if (!IsLit) return;

            // 2. Tak beratap saat Storm atau Blizzard: padam, bahan bakar tetap.
            if (!isRoofed && storm && stormKills)
            {
                stormOff = true;
                SetLit(false);
                return;
            }

            // 3. Obor Dinding tak beratap saat Rain boros 1,5 kali. Lampu tidak terpengaruh.
            float mult = !data.isLamp && !isRoofed && w == WeatherType.Rain ? (b != null ? b.rainFuelMultiplier : 1.5f) : 1f;
            fuelSeconds -= sim * mult;

            // 4. Kedip, lalu padam.
            if (fuelSeconds <= 0f)
            {
                fuelSeconds = 0f;
                SetLit(false);
                return;
            }
            float flicker = b != null ? b.flickerSeconds : 2f;
            float hz = Balance != null ? Balance.flashlight.flickerHz : 6f;   // Kecepatan kedip yang sama dengan senter
            if (lightSource != null) lightSource.SetFlicker(fuelSeconds < flicker ? (Mathf.PingPong(Time.time * hz, 1f) > 0.5f ? 1f : 0.2f) : 1f);
        }

        private void SetLit(bool on)
        {
            if (IsLit == on) return;
            IsLit = on;
            ApplyLit();
            EventChannel<LightSourceToggledEvent>.Publish(new LightSourceToggledEvent(StructureId, on));
        }

        private void ApplyLit()
        {
            if (lightSource == null) return;
            lightSource.SetFlicker(1f);
            lightSource.IsLit = IsLit && isActiveAndEnabled;
        }

        // ---------------------------------------------------------------- Atap (aturan CampfireManager)

        private void RefreshRoof()
        {
            float dist = Balance != null ? Balance.campfire.skyCheckDistance : 20f;
            Vector3 origin = skyProbe != null ? skyProbe.position : transform.position + Vector3.up * 1f;
            isRoofed = PhysicsQuery.Raycast(new Ray(origin, Vector3.up), out _, dist, PhysicsMasks.Sunlight);
        }

        private void HandleStructureBuilt(StructureBuiltEvent e) => RecheckIfNear(e.position);

        private void RecheckIfNear(Vector3 pos)
        {
            float r = Balance != null ? Balance.campfire.structureRecheckRadius : 6f;
            if ((pos - transform.position).sqrMagnitude <= r * r) RefreshRoof();
        }

        private void HandleStructureDestroyed(StructureDestroyedEvent e)
        {
            string id = StructureId;
            if (string.IsNullOrEmpty(id) || !string.Equals(e.structureId, id, StringComparison.Ordinal))
            {
                RecheckIfNear(e.position);
                return;
            }
            if (destroyedHandled) return;
            destroyedHandled = true;
            // Struktur hancur: bahan bakar hilang, LightSource dilepas lewat OnDisable.
            fuelSeconds = 0f;
            SetLit(false);
        }

        // ---------------------------------------------------------------- IInteractable

        private bool HasFuelItem(in InteractContext ctx)
        {
            var inv = ctx.Inventory != null ? ctx.Inventory : InventoryManager.Instance;
            return inv != null && data != null && inv.HasItem(data.fuelItemId, 1);
        }

        private bool StormBlocks()
        {
            if (isRoofed || RegionalWeather.Instance == null) return false;
            var w = RegionalWeather.Instance.GetWeather(homeRegion);
            return w == WeatherType.Storm || w == WeatherType.Blizzard;
        }

        public bool IsAvailable => isActiveAndEnabled && data != null;
        public float HoldSeconds => 0f;

        public string GetPrompt()
        {
            if (data == null) return string.Empty;
            if (IsLit) return Text("prompt.light_off", "Matikan");
            if (fuelSeconds <= 0f && InventoryManager.Instance != null && !InventoryManager.Instance.HasItem(data.fuelItemId, 1))
                return Text(data.needFuelKey, data.isLamp ? "Butuh Baterai" : "Butuh Kayu");
            return Text("prompt.light_on", "Nyalakan");
        }

        public InteractBlockReason CanInteract(in InteractContext ctx)
        {
            if (data == null) return InteractBlockReason.Locked;
            if (!IsLit && fuelSeconds <= 0f && !HasFuelItem(ctx)) return InteractBlockReason.MissingTool;
            return InteractBlockReason.None;
        }

        public void OnHoverEnter() { if (highlight != null) highlight.SetState(HighlightState.Available); }
        public void OnHoverExit() { if (highlight != null) highlight.SetState(HighlightState.Off); }

        public bool Interact(in InteractContext ctx)
        {
            if (data == null) return false;
            // Menyala: matikan, bahan bakar tetap tersimpan.
            if (IsLit)
            {
                stormOff = false;
                SetLit(false);
                return true;
            }
            if (StormBlocks())
            {
                if (NotificationManager.Instance != null) NotificationManager.Instance.Show("notif.too_windy", NotificationKind.Warning);   // "Terlalu berangin"
                return false;
            }
            var inv = ctx.Inventory != null ? ctx.Inventory : InventoryManager.Instance;
            // Tanpa bahan bakar (atau belum penuh): kurangi 1 item dan isi. Baterai yang masih terisi tidak dikembalikan.
            if (inv != null && inv.HasItem(data.fuelItemId, 1) && fuelSeconds + data.secondsPerItem <= data.capacitySeconds + 0.01f
                && (fuelSeconds <= 0f || !data.isLamp))
            {
                inv.RemoveItem(data.fuelItemId, 1);
                fuelSeconds = Mathf.Min(data.capacitySeconds, fuelSeconds + data.secondsPerItem);
            }
            if (fuelSeconds <= 0f) return false;
            lastTickGameHour = NowGameHours();
            stormOff = false;
            SetLit(true);
            return true;
        }

        private static string Text(string key, string fallback)
        {
            var loc = LocalizationManager.Instance;
            if (loc == null) return fallback;
            string s = loc.Get(key);
            return string.IsNullOrEmpty(s) || s == key ? fallback : s;
        }

        // ---------------------------------------------------------------- ISaveable (StructureRecord.extraJson)

        [Serializable]
        private class SaveData
        {
            public float fuelSeconds;
            public bool isLit;
            public float lastTickGameHour;
            public bool stormOff;
            public int homeRegion;
        }

        public string SaveId => StructureId;
        public int LoadOrder => 100;

        public string CaptureState()
        {
            return JsonUtility.ToJson(new SaveData
            {
                fuelSeconds = fuelSeconds,
                isLit = IsLit,
                lastTickGameHour = lastTickGameHour,
                stormOff = stormOff,
                homeRegion = (int)homeRegion
            });
        }

        public void RestoreState(string json)
        {
            if (string.IsNullOrEmpty(json)) return;
            SaveData d = null;
            try { d = JsonUtility.FromJson<SaveData>(json); }
            catch (Exception e) { Debug.LogWarning($"[LightFixture] Save tidak terbaca: {e.Message}"); }
            if (d == null) return;
            float cap = data != null ? data.capacitySeconds : d.fuelSeconds;
            fuelSeconds = Mathf.Clamp(d.fuelSeconds, 0f, cap);
            IsLit = d.isLit && fuelSeconds > 0f;
            lastTickGameHour = d.lastTickGameHour;
            stormOff = d.stormOff;
            homeRegion = (RegionType)d.homeRegion;
            homeRegionSet = true;
        }

        public void OnRestoreFinished()
        {
            RefreshRoof();
            ApplyLit();
        }
    }
}
