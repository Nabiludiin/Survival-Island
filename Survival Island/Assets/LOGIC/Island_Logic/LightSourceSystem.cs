using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Daftar sumber cahaya, keterlihatan pemain yang ringan, batas lampu real-time, dan tarikan zombie ke cahaya base.
    /// Manager di scene IslandPhase. Blueprint: Island_Logic/LightSourceSystem.md (#98).
    /// GDD bagian 3 (malam memicu horde dan serangan base) dan bagian 5 (Region 2 gelap meski siang, Zombie Lumut).
    /// Struktur lampu, baterai, dan tarikan cahaya adalah usulan yang disetujui developer (#85).
    ///
    /// Catatan implementasi (sistem pendukung):
    /// - AISensoryHub.SetLightAttraction belum ada (TODO di AISensoryHub.cs): titik dan peluang dihitung dan disimpan di
    ///   LightAttractionPoint / LightAttractionChance, pemanggilannya ditandai TODO.
    /// - Pengurangan Item_Battery dilakukan lewat TryUseBattery, untuk dipanggil ItemUseSystem (#66, kode lama).
    /// </summary>
    public class LightSourceSystem : MonoBehaviour
    {
        public const string BatteryItemId = "Item_Battery";
        public const string FlashlightItemId = "Tool_Flashlight";

        public static LightSourceSystem Instance { get; private set; }

        private readonly List<LightSource> sources = new List<LightSource>(64);
        private readonly List<LightSource> sortBuffer = new List<LightSource>(64);
        private static readonly System.Comparison<LightSource> ByScoreDesc = CompareScore;

        private float visibilityTimer;
        private float budgetTimer;
        private bool isNight;
        private RegionType currentRegion = RegionType.Volcano;
        private float lastPublishedVisibility = -1f;
        private int maxLightsOverride = -1;
        private int maxShadowsOverride = -1;
        private Camera cachedCamera;

        public float PlayerLightLevel { get; private set; }
        public float PlayerVisibility { get; private set; } = 1f;
        public Vector3 LightAttractionPoint { get; private set; }
        public float LightAttractionChance { get; private set; }
        public int RegisteredCount => sources.Count;

        private static LightBalance B => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.light : null;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake() { Instance = this; }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void OnEnable()
        {
            EventChannel<NightfallEvent>.Raised += HandleNightfall;
            EventChannel<DaybreakEvent>.Raised += HandleDaybreak;
            EventChannel<RegionChangedEvent>.Raised += HandleRegionChanged;
            EventChannel<WeatherChangedEvent>.Raised += HandleWeatherChanged;
            EventChannel<RunEndedEvent>.Raised += HandleRunEnded;
            EventChannel<LightSourceToggledEvent>.Raised += HandleToggled;
            if (EventChannel<RegionChangedEvent>.HasLast) currentRegion = EventChannel<RegionChangedEvent>.Last.to;
        }

        private void OnDisable()
        {
            EventChannel<NightfallEvent>.Raised -= HandleNightfall;
            EventChannel<DaybreakEvent>.Raised -= HandleDaybreak;
            EventChannel<RegionChangedEvent>.Raised -= HandleRegionChanged;
            EventChannel<WeatherChangedEvent>.Raised -= HandleWeatherChanged;
            EventChannel<RunEndedEvent>.Raised -= HandleRunEnded;
            EventChannel<LightSourceToggledEvent>.Raised -= HandleToggled;
        }

        private void Start()
        {
            isNight = DayNightDirector.Instance != null && DayNightDirector.Instance.IsNight;
            // Sumber yang aktif sebelum manager ini ada.
            var existing = FindObjectsByType<LightSource>(FindObjectsSortMode.None);
            for (int i = 0; i < existing.Length; i++)
                if (Register(existing[i])) existing[i].MarkRegistered();
            RecalculateAttraction();
        }

        private void HandleNightfall(NightfallEvent e) { isNight = true; RecalculateAttraction(); }
        private void HandleDaybreak(DaybreakEvent e) { isNight = false; }
        private void HandleRegionChanged(RegionChangedEvent e) => currentRegion = e.to;
        private void HandleWeatherChanged(WeatherChangedEvent e) { }   // Cuaca dibaca langsung tiap tick
        private void HandleToggled(LightSourceToggledEvent e) => RecalculateAttraction();

        private void HandleRunEnded(RunEndedEvent e)
        {
            // Permadeath: keterlihatan dan tarikan kembali netral.
            if (AISensoryHub.Instance != null) AISensoryHub.Instance.SetVisibilityModifier(1f);
            LightAttractionPoint = Vector3.zero;
            LightAttractionChance = 0f;
            // TODO(AISensoryHub #98): AISensoryHub.Instance.SetLightAttraction(Vector3.zero, 0f);
        }

        // ---------------------------------------------------------------- Daftar sumber

        /// <summary>false kalau batas maxRegisteredSources tercapai (sumber tetap tampil lewat emissive, tidak dihitung).</summary>
        public bool Register(LightSource s)
        {
            if (s == null) return false;
            if (sources.Contains(s)) return true;
            int max = B != null ? B.maxRegisteredSources : 64;
            if (sources.Count >= max && !s.isHeld) return false;
            sources.Add(s);
            return true;
        }

        public void Unregister(LightSource s) => sources.Remove(s);

        /// <summary>Dipanggil #19 (SettingsManager) untuk mengubah batas sesuai setelan grafis.</summary>
        public void ApplyQuality(int lights, int shadows)
        {
            maxLightsOverride = Mathf.Max(0, lights);
            maxShadowsOverride = Mathf.Max(0, shadows);
            budgetTimer = float.MaxValue;   // Terapkan pada frame berikutnya
        }

        /// <summary>BuildManager (#79) menolak struktur cahaya baru di atas maxLightStructures ("Terlalu banyak lampu").</summary>
        public static bool CanBuildLightStructure()
        {
            int max = B != null ? B.maxLightStructures : 24;
            return LightFixture.All.Count < max;
        }

        // ---------------------------------------------------------------- Tick

        private void Update()
        {
            float dt = Time.deltaTime;
            if (dt <= 0f) return;   // Jeda
            var b = B;
            if (b == null) return;

            visibilityTimer += dt;
            if (visibilityTimer >= b.visibilityTickSeconds)
            {
                visibilityTimer = 0f;
                TickVisibility(b);
            }
            budgetTimer += dt;
            if (budgetTimer >= b.budgetTickSeconds)
            {
                budgetTimer = 0f;
                TickBudget(b);
            }
        }

        private static bool PlayerInInterior()
        {
            var zones = InteriorZone.Active;
            for (int i = 0; i < zones.Count; i++) if (zones[i] != null && zones[i].IsPlayerInside) return true;
            return false;
        }

        private void TickVisibility(LightBalance b)
        {
            var player = IslandPlayerController.Instance;
            if (player == null) return;
            Vector3 p = player.transform.position;
            bool interior = PlayerInInterior();

            float ambient = isNight ? b.ambientNight : b.ambientDay;
            // GDD bagian 5: Region 2 gelap meski siang.
            if (currentRegion == RegionType.Amazon && !isNight) ambient = b.ambientAmazonDay;
            if (interior) ambient = b.ambientInterior;
            WeatherType w = RegionalWeather.Instance != null ? RegionalWeather.Instance.GetWeather(currentRegion) : WeatherType.Clear;
            if (w == WeatherType.Storm || w == WeatherType.Blizzard) ambient *= b.stormAmbientMultiplier;

            float extra = 0f;
            for (int i = 0; i < sources.Count; i++)
            {
                var s = sources[i];
                if (s == null || !s.IsLit) continue;
                if (interior && !s.isHeld) continue;   // Dalam InteriorZone hanya senter dan obor (#112)
                extra += GetContribution(s, p);
            }

            PlayerLightLevel = Mathf.Clamp01(ambient + extra);
            float visibility = Mathf.Lerp(b.visibilityInDark, b.visibilityInLight, PlayerLightLevel);
            if (FlashlightController.IsActiveLit) visibility += b.flashlightVisibilityBonus;
            PlayerVisibility = visibility;

            if (Mathf.Abs(visibility - lastPublishedVisibility) > b.visibilityPublishDelta)
            {
                lastPublishedVisibility = visibility;
                if (AISensoryHub.Instance != null) AISensoryHub.Instance.SetVisibilityModifier(visibility);
                EventChannel<PlayerLightLevelChangedEvent>.Publish(new PlayerLightLevelChangedEvent(PlayerLightLevel, visibility));
            }
        }

        /// <summary>intensity01 x (1 - jarak / range), 0 di luar range. Akar hanya untuk sumber yang terjangkau.</summary>
        private static float GetContribution(LightSource s, Vector3 pos)
        {
            if (s.range <= 0f) return 0f;
            float d2 = (s.transform.position - pos).sqrMagnitude;
            if (d2 >= s.range * s.range) return 0f;
            return s.intensity01 * (1f - Mathf.Sqrt(d2) / s.range);
        }

        /// <summary>Jumlah kontribusi sumber menyala di posisi ini, tanpa ambient dan tanpa senter (Zombie Lumut #25).</summary>
        public float GetLightLevelAt(Vector3 pos)
        {
            float sum = 0f;
            for (int i = 0; i < sources.Count; i++)
            {
                var s = sources[i];
                if (s == null || !s.IsLit || s.isFlashlight) continue;
                sum += GetContribution(s, pos);
            }
            return sum;
        }

        // ---------------------------------------------------------------- Batas lampu real-time

        private void TickBudget(LightBalance b)
        {
            if (cachedCamera == null) cachedCamera = Camera.main;
            Vector3 cam = cachedCamera != null ? cachedCamera.transform.position : Vector3.zero;
            int maxLights = maxLightsOverride >= 0 ? maxLightsOverride : b.maxRealtimeLights;
            int maxShadows = maxShadowsOverride >= 0 ? maxShadowsOverride : b.maxShadowLights;
            if (PlayerInInterior()) maxLights = Mathf.Min(maxLights, b.interiorMaxRealtimeLights);   // #112

            // 1. Skor. Senter dan obor pegang selalu menang. Sumber yang sedang aktif mendapat margin supaya tidak berkedip.
            sortBuffer.Clear();
            for (int i = 0; i < sources.Count; i++)
            {
                var s = sources[i];
                if (s == null || !s.IsLit || s.realLight == null) { if (s != null) s.lastScore = -1f; continue; }
                float score = s.isHeld ? float.MaxValue : s.intensity01 * s.range / (1f + Vector3.Distance(cam, s.transform.position));
                if (!s.isHeld && s.HasRealtimeBudget && s.realLight.enabled) score *= b.budgetSwapMargin;
                s.lastScore = score;
                sortBuffer.Add(s);
            }
            sortBuffer.Sort(ByScoreDesc);

            // 2-4. Pilih maxLights teratas; bayangan hanya untuk sumber terdekat di luar senter.
            int shadowsGiven = 0;
            LightSource nearestForShadow = null;
            float nearest = float.MaxValue;
            for (int i = 0; i < sortBuffer.Count && i < maxLights; i++)
            {
                var s = sortBuffer[i];
                if (s.isHeld || s.isFlashlight) continue;
                float d = (s.transform.position - cam).sqrMagnitude;
                if (d < nearest) { nearest = d; nearestForShadow = s; }
            }
            for (int i = 0; i < sortBuffer.Count; i++)
            {
                var s = sortBuffer[i];
                bool chosen = i < maxLights;
                bool shadows = chosen && s == nearestForShadow && shadowsGiven < maxShadows;
                if (shadows) shadowsGiven++;
                s.SetRealtimeBudget(chosen, shadows);
            }
            // Sumber mati atau tanpa Light: tidak memakai anggaran.
            for (int i = 0; i < sources.Count; i++)
            {
                var s = sources[i];
                if (s != null && !s.IsLit) s.SetRealtimeBudget(false, false);
            }
            sortBuffer.Clear();
        }

        private static int CompareScore(LightSource a, LightSource b) => b.lastScore.CompareTo(a.lastScore);

        // ---------------------------------------------------------------- Tarikan cahaya base

        private void RecalculateAttraction()
        {
            var b = B;
            var all = LightFixture.All;
            int count = 0;
            Vector3 sum = Vector3.zero;
            for (int i = 0; i < all.Count; i++)
            {
                var f = all[i];
                if (f == null || !f.IsLit) continue;
                count++;
                sum += f.transform.position;
            }
            if (count == 0)
            {
                LightAttractionPoint = Vector3.zero;
                LightAttractionChance = 0f;
            }
            else
            {
                LightAttractionPoint = sum / count;
                // Usulan 1 disetujui: tarikan ringan, maksimum 0,35, hanya zombie Unaware yang terpengaruh (diatur AISensoryHub).
                LightAttractionChance = Mathf.Min(count * (b != null ? b.attractionChancePerLight : 0.05f), b != null ? b.attractionChanceMax : 0.35f);
            }
            // TODO(AISensoryHub #98): AISensoryHub.Instance.SetLightAttraction(LightAttractionPoint, LightAttractionChance);
        }

        // ---------------------------------------------------------------- Baterai senter (dipanggil ItemUseSystem #66)

        /// <summary>
        /// Aksi Pakai Item_Battery: senter dipegang lebih dulu, lalu senter dengan isi terendah di inventori.
        /// Mengurangi 1 baterai hanya kalau Recharge berhasil. failKey berisi kunci notifikasi kalau ditolak.
        /// </summary>
        public static bool TryUseBattery(InventoryManager inv, out string failKey)
        {
            failKey = null;
            if (inv == null || !inv.HasItem(BatteryItemId, 1)) { failKey = "notif.no_battery"; return false; }
            // 1.
            ItemInstance target = FlashlightController.ActiveInstance;
            if (target == null)
            {
                var slots = inv.Slots;
                for (int i = 0; i < slots.Count; i++)
                {
                    var s = slots[i];
                    if (s == null || s.IsEmpty || s.instance.itemId != FlashlightItemId) continue;
                    if (target == null || s.instance.durability < target.durability) target = s.instance;
                }
            }
            if (target == null) { failKey = "notif.no_flashlight"; return false; }   // "Tidak ada senter"
            // 2-3.
            var b = GameManager.CurrentBalance;
            float amount = b != null ? b.flashlight.batteryRecharge : 100f;
            if (!FlashlightController.Recharge(target, amount)) { failKey = "notif.flashlight_full"; return false; }
            inv.RemoveItem(BatteryItemId, 1);
            return true;
        }
    }
}
