using System;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Penampung air hujan (Build_RainCollector). Bertambah per jam game saat Rain atau Storm di homeRegion, lalu diambil dengan E
    /// sebagai Drink_CleanWater. Blueprint: raft/HydrationSystem.md bagian "RainwaterCollector" (#43).
    /// GDD bagian 3: haus bisa diisi air hujan yang ditampung.
    ///
    /// Catatan implementasi (sistem pendukung):
    /// - IslandInteractable (#17) belum ada, jadi kelas ini menerapkan IInteractable langsung (pola CraftingStation).
    /// - Data disimpan lewat ISaveable di prefab: StructureRegistry menulisnya ke StructureRecord.extraJson (#79).
    /// - Pendaftaran ke HydrationSystem di Start dan OnRestoreFinished (bukan OnEnable), dilepas di OnDestroy.
    /// - homeRegion: RegionStreaming.GetRegionAt (#88) belum ada, jadi diisi ZoneTracker.Active saat dibangun, lalu disimpan.
    /// </summary>
    public class RainwaterCollector : MonoBehaviour, IInteractable, ISaveable
    {
        [SerializeField] private ItemData cleanWaterItem;          // Drink_CleanWater
        [SerializeField] private Transform fillVisual;              // Tinggi air di dalam wadah
        [SerializeField] private float fillEmptyLocalY;
        [SerializeField] private float fillFullLocalY = 0.5f;
        [SerializeField] private Transform skyProbe;                // Titik awal raycast atap, di atas collider sendiri
        [SerializeField] private InteractHighlight highlight;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip collectClip;

        private float storedPortions;
        private RegionType homeRegion;
        private bool homeRegionSet;
        private bool isExposedToSky = true;
        private bool registered;

        public float StoredPortions => storedPortions;
        public bool IsExposedToSky => isExposedToSky;
        public RegionType HomeRegion => homeRegion;

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;
        private static float Capacity => Balance != null ? Balance.hydration.collectorCapacityPortions : 4f;

        // ---------------------------------------------------------------- Siklus hidup

        private void Start()
        {
            EnsureHomeRegion();
            RegisterSelf();
            RefreshSkyExposure();
            UpdateVisual();
        }

        private void OnEnable()
        {
            // Region kembali dimuat: tampilan air disegarkan (isi tetap bertambah selama nonaktif).
            UpdateVisual();
        }

        private void OnDestroy()
        {
            // Penampung hancur: isi hilang, air bukan barang di dunia.
            if (registered && HydrationSystem.Instance != null) HydrationSystem.Instance.Unregister(this);
            registered = false;
        }

        private void RegisterSelf()
        {
            if (registered || HydrationSystem.Instance == null) return;
            HydrationSystem.Instance.Register(this);
            registered = true;
        }

        private void EnsureHomeRegion()
        {
            if (homeRegionSet) return;
            // Region rumah dari RegionStreaming (#88), cadangan ZoneTracker.Active.
            homeRegion = RegionStreaming.RegionOf(transform.position);
            homeRegionSet = true;
        }

        // ---------------------------------------------------------------- Tick (dipanggil HydrationSystem)

        /// <summary>Satu tick per jam game, juga saat objek nonaktif (region dilepas).</summary>
        public void TickHour()
        {
            float cap = Capacity;
            if (!isExposedToSky || storedPortions >= cap) return;
            var b = Balance != null ? Balance.hydration : null;
            WeatherType w = RegionalWeather.Instance != null ? RegionalWeather.Instance.GetWeather(homeRegion) : WeatherType.Clear;
            // Usulan disetujui #43: Clear, Fog, Snow, dan Blizzard tidak mengisi (salju tidak mencair di wadah ini).
            float rate = w == WeatherType.Rain ? (b != null ? b.rainFillPerGameHour : 0.5f)
                       : w == WeatherType.Storm ? (b != null ? b.stormFillPerGameHour : 0.75f) : 0f;
            if (rate <= 0f) return;
            storedPortions = Mathf.Min(cap, storedPortions + rate);
            UpdateVisual();
        }

        /// <summary>Raycast ke atas dengan PhysicsMasks.Sunlight. Terbuka kalau tidak mengenai apa pun.</summary>
        public void RefreshSkyExposure()
        {
            float dist = Balance != null ? Balance.hydration.skyCheckDistance : 20f;
            Vector3 origin = skyProbe != null ? skyProbe.position : transform.position + Vector3.up * 2f;
            isExposedToSky = !PhysicsQuery.Raycast(new Ray(origin, Vector3.up), out _, dist, PhysicsMasks.Sunlight);
        }

        private void UpdateVisual()
        {
            if (fillVisual == null) return;
            float t = Capacity > 0f ? Mathf.Clamp01(storedPortions / Capacity) : 0f;
            var p = fillVisual.localPosition;
            p.y = Mathf.Lerp(fillEmptyLocalY, fillFullLocalY, t);
            fillVisual.localPosition = p;
            fillVisual.gameObject.SetActive(storedPortions > 0.001f);
        }

        // ---------------------------------------------------------------- IInteractable

        public bool IsAvailable => isActiveAndEnabled;
        public float HoldSeconds => 0f;

        public string GetPrompt()
        {
            int n = Mathf.FloorToInt(storedPortions);
            // Teks dari LocalizationManager (#104).
            if (n <= 0 && !isExposedToSky) return Text("prompt.rain_collector_covered", "Tertutup atap");
            var loc = LocalizationManager.Instance;
            return loc != null && loc.Has("prompt.rain_collector") ? loc.Format("prompt.rain_collector", "count", loc.FormatNumber(n)) : "Ambil Air (" + n + ")";
        }

        public InteractBlockReason CanInteract(in InteractContext ctx)
        {
            // blockedWhileCarrying = true (IslandInteractable.md).
            if (ctx.IsCarryingHeavyItem) return InteractBlockReason.CarryingHeavyItem;
            // Tertutup atap dan kosong: alasan Locked. Isi yang sudah ada tetap bisa diambil.
            if (Mathf.FloorToInt(storedPortions) <= 0 && !isExposedToSky) return InteractBlockReason.Locked;
            return InteractBlockReason.None;
        }

        public void OnHoverEnter() { if (highlight != null) highlight.SetState(HighlightState.Available); }
        public void OnHoverExit() { if (highlight != null) highlight.SetState(HighlightState.Off); }

        public bool Interact(in InteractContext ctx)
        {
            // 1.
            int portions = Mathf.FloorToInt(storedPortions);
            if (portions <= 0)
            {
                Notify("notif.collector_empty", NotificationKind.Info);   // "Penampung kosong"
                return false;
            }
            if (ctx.Inventory == null || cleanWaterItem == null) return false;
            // 2-3.
            int left = ctx.Inventory.AddItem(cleanWaterItem, portions);
            storedPortions -= portions - left;
            UpdateVisual();
            // 4.
            if (left == portions)
            {
                Notify("notif.inventory_full", NotificationKind.Warning);
                return false;
            }
            if (audioSource != null && collectClip != null) audioSource.PlayOneShot(collectClip);
            return true;
        }

        private static void Notify(string key, NotificationKind kind)
        {
            if (NotificationManager.Instance != null) NotificationManager.Instance.Show(key, kind);
        }

        // ---------------------------------------------------------------- ISaveable (lewat StructureRecord.extraJson)

        [Serializable]
        private class SaveData
        {
            public float storedPortions;
            public int homeRegion;
        }

        public string SaveId
        {
            get
            {
                // SaveId = StructureId (#43).
                var h = GetComponent<IStructureHealth>();
                return h != null ? h.StructureId : string.Empty;
            }
        }

        public int LoadOrder => 0;

        public string CaptureState()
        {
            return JsonUtility.ToJson(new SaveData { storedPortions = storedPortions, homeRegion = (int)homeRegion });
        }

        public void RestoreState(string json)
        {
            if (string.IsNullOrEmpty(json)) return;
            SaveData d = null;
            try { d = JsonUtility.FromJson<SaveData>(json); }
            catch (Exception e) { Debug.LogWarning($"[RainwaterCollector] Save tidak terbaca: {e.Message}"); }
            if (d == null) return;
            storedPortions = Mathf.Clamp(d.storedPortions, 0f, Capacity);
            homeRegion = (RegionType)d.homeRegion;
            homeRegionSet = true;
        }

        public void OnRestoreFinished()
        {
            // isExposedToSky dihitung ulang, tidak disimpan.
            EnsureHomeRegion();
            RegisterSelf();
            RefreshSkyExposure();
            UpdateVisual();
        }

        /// <summary>Teks dari LocalizationManager (#104); teks bawaan kalau kunci belum ada di tabel.</summary>
        private static string Text(string key, string fallback)
        {
            var loc = LocalizationManager.Instance;
            return loc != null && loc.Has(key) ? loc.Get(key) : fallback;
        }
    }
}
