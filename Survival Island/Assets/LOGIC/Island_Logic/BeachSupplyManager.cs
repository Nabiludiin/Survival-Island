using System;
using UnityEngine;
using UnityEngine.AI;

namespace SurvivalIsland
{
    /// <summary>Tabel isi kargo per tier hari (BeachSupplyManager.md "Tabel isi").</summary>
    [Serializable]
    public struct BeachCrateTier
    {
        public int minDay;
        public LootTable table;
    }

    /// <summary>Status satu kiriman. Disimpan (world_supply_beach).</summary>
    [Serializable]
    public class BeachDeliveryState
    {
        public int day;                     // 0 berarti tidak ada kiriman
        public float expireTotalHours;
        public bool[] isOpened = new bool[0];
        public bool animalAmbushDone, zombieAmbushDone;
    }

    /// <summary>
    /// Kargo dari sisa kapal di pantai selatan Region 1 setiap 7 hari: isi, batas waktu, dan risiko.
    /// Manager di scene IslandPhase, ISaveable (world_supply_beach, LoadOrder 130). Satu kiriman dalam satu waktu.
    /// Blueprint: Island_Logic/BeachSupplyManager.md (#71). GDD bagian 6 (Weekly Beach Supply: Air Minum Bersih, Makanan
    /// Kaleng, Peluru, Resource langka). Jumlah, isi per hari, batas waktu, dan risiko adalah usulan disetujui developer (#85).
    ///
    /// Catatan implementasi (sistem pendukung):
    /// - Penanda kompas memakai ID tetap "supply" lewat CompassSystem.SetMarker (#30), jadi tidak perlu disimpan di sini.
    /// - Isi kargo dihitung ulang dari seed dan hari (BuildDelivery murni), jadi save cukup menyimpan kargo yang dibuka.
    /// </summary>
    public class BeachSupplyManager : MonoBehaviour, ISaveable
    {
        public const string SaveIdConst = "world_supply_beach";
        public const string CratePoolTag = "Supply_BeachCrate";
        private const int MaxCrates = 8;

        public static BeachSupplyManager Instance { get; private set; }

        [SerializeField] private BeachSupplyPoint[] points = new BeachSupplyPoint[0];
        [Tooltip("Satu titik yang selalu sah.")]
        [SerializeField] private Transform fallbackPoint;
        [Tooltip("Urut minDay naik: hari 7, 21, 42 (tabel isi blueprint).")]
        [SerializeField] private BeachCrateTier[] tiers = new BeachCrateTier[0];
        [SerializeField] private RegionSpawnTable beachDayTable;
        [SerializeField] private RegionSpawnTable beachNightTable;
        [SerializeField] private string splashVfxTag = "VFX_WaterSplash";

        private BeachDeliveryState state = new BeachDeliveryState();
        private readonly BeachCrate[] liveCrates = new BeachCrate[MaxCrates];
        private readonly Vector3[] crateSpots = new Vector3[MaxCrates];
        private static readonly Collider[] overlapBuffer = new Collider[4];
        private bool registeredForSave;
        private int pendingDay;
        private float ambushTimer;

        // Hasil BuildDelivery untuk kiriman aktif.
        private int[] pointOrder = new int[0];
        private ItemStack[] contents = new ItemStack[0];   // crate * rollsPerCrate
        private int contentDay = -1;

        public BeachDeliveryState State => state;
        public int PendingDay => pendingDay;
        public bool HasDelivery => state != null && state.day > 0;

        private static BeachSupplyBalance B => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.beachSupply : null;
        private static int CurrentDay => GameManager.Instance != null ? GameManager.Instance.currentDay : 1;
        private static float NowHours => DayNightDirector.Instance != null ? (float)DayNightDirector.Instance.TotalGameHours : 0f;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake() { Instance = this; }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void OnEnable()
        {
            EventChannel<DayChangedEvent>.Raised += HandleDayChanged;
            EventChannel<DaybreakEvent>.Raised += HandleDaybreak;
            EventChannel<HourChangedEvent>.Raised += HandleHour;
            EventChannel<RegionLoadStateChangedEvent>.Raised += HandleRegionLoadState;
            if (WorldStateSave.Instance != null)
            {
                WorldStateSave.Instance.Register(this);
                registeredForSave = true;
            }
        }

        private void OnDisable()
        {
            EventChannel<DayChangedEvent>.Raised -= HandleDayChanged;
            EventChannel<DaybreakEvent>.Raised -= HandleDaybreak;
            EventChannel<HourChangedEvent>.Raised -= HandleHour;
            EventChannel<RegionLoadStateChangedEvent>.Raised -= HandleRegionLoadState;
            if (registeredForSave && WorldStateSave.Instance != null) WorldStateSave.Instance.Unregister(this);
            registeredForSave = false;
        }

        // ---------------------------------------------------------------- Pemicu

        private void HandleDayChanged(DayChangedEvent e)
        {
            // Kiriman ulang setelah load diabaikan.
            if (e.isReplay) return;
            var b = B;
            if (b != null && b.intervalDays > 0 && e.newDay % b.intervalDays == 0) pendingDay = e.newDay;
        }

        private void HandleDaybreak(DaybreakEvent e)
        {
            var b = B;
            int day = CurrentDay;
            // GDD bagian 6: hari ke-7, 14, 21, dan seterusnya, tanpa hari terakhir (usulan 8). Syarat dari currentDay,
            // dan state.day mencegah kiriman ganda saat DaybreakEvent dikirim ulang setelah load.
            if (b == null || b.intervalDays <= 0 || day % b.intervalDays != 0 || state.day == day) return;
            Deliver(day);
        }

        private void HandleHour(HourChangedEvent e)
        {
            if (!HasDelivery) return;
            if (NowHours >= state.expireTotalHours)
            {
                ExpireDelivery();
                return;
            }
            TickAmbush();
        }

        private void HandleRegionLoadState(RegionLoadStateChangedEvent e)
        {
            if (e.region != RegionType.Volcano) return;
            if (e.state == RegionLoadState.Loaded && HasDelivery) SpawnCrates();
            else if (e.state == RegionLoadState.Unloading) ReturnCrates(false);   // state tetap ada
        }

        private void Update()
        {
            // Pemeriksaan tambahan saat pemain memasuki tideZone, karena HourChangedEvent hanya sekali per 37,5 detik.
            if (!HasDelivery || Time.deltaTime <= 0f) return;
            ambushTimer += Time.deltaTime;
            if (ambushTimer < 1f) return;
            ambushTimer = 0f;
            var player = IslandPlayerController.Instance;
            var tide = TideManager.Instance;
            if (player == null || tide == null) return;
            var z = tide.TideZone;
            Vector3 p = player.transform.position;
            if (p.x >= z.min.x && p.x <= z.max.x && p.z >= z.min.z && p.z <= z.max.z) TickAmbush();
        }

        // ---------------------------------------------------------------- Kiriman

        /// <summary>Fungsi murni: urutan titik dan isi tiap kargo hanya dari seed dan hari.</summary>
        private void BuildDelivery(int day, out int[] pointByCrate, out ItemStack[] result)
        {
            var b = B;
            int rolls = b != null ? Mathf.Max(1, b.rollsPerCrate) : 3;
            int count = CrateCount(day);
            pointByCrate = new int[Mathf.Max(points.Length, 0)];
            result = new ItemStack[count * rolls];
            if (RunSeedManager.Instance == null) return;
            // 1.
            var s = RunSeedManager.Instance.Fork(RandomStreamId.BeachSupply, day);
            // 3. Urutan titik diacak; SpawnCrates mengambil titik isReachable yang lolos syarat.
            for (int i = 0; i < pointByCrate.Length; i++) pointByCrate[i] = i;
            if (pointByCrate.Length > 1) s.Shuffle(pointByCrate);
            // 4.
            LootTable table = TableFor(day);
            for (int c = 0; c < count; c++)
            {
                for (int r = 0; r < rolls; r++)
                {
                    ItemData item = null;
                    int amount = 0;
                    if (table != null) RegionLootTable.TryRoll(table, LootPointKind.Any, day, ref s, out item, out amount);
                    // 5. Isi pertama kargo pertama selalu air bersih atau makanan kaleng (GDD bagian 6).
                    if (c == 0 && r == 0 && !IsStaple(item))
                    {
                        string id = s.NextFloat01() < 0.5f ? "Drink_CleanWater" : "Food_Can";
                        item = ItemDatabase.Get(id);
                        amount = 2;
                    }
                    result[c * rolls + r] = new ItemStack(item, Mathf.Max(1, amount));
                }
            }
        }

        private static bool IsStaple(ItemData item) => item != null && (item.itemID == "Drink_CleanWater" || item.itemID == "Food_Can");

        private LootTable TableFor(int day)
        {
            LootTable best = null;
            int bestMin = int.MinValue;
            for (int i = 0; i < tiers.Length; i++)
                if (tiers[i].table != null && tiers[i].minDay <= day && tiers[i].minDay >= bestMin) { bestMin = tiers[i].minDay; best = tiers[i].table; }
            if (best == null && tiers.Length > 0) best = tiers[0].table;
            return best;
        }

        /// <summary>2 + hari/14 x 1, maksimal 4: 2, 3, 3, 4, lalu tetap 4 (termasuk setelah hari ke-60).</summary>
        private static int CrateCount(int day)
        {
            var b = B;
            if (b == null) return 2;
            return Mathf.Clamp(b.crateCountBase + day / 14 * b.crateCountPerTwoWeeks, 1, Mathf.Min(b.crateCountMax, MaxCrates));
        }

        private void EnsureContents()
        {
            if (contentDay == state.day) return;
            BuildDelivery(state.day, out pointOrder, out contents);
            contentDay = state.day;
        }

        private void Deliver(int day)
        {
            var b = B;
            // 1.
            int count = CrateCount(day);
            state = new BeachDeliveryState
            {
                day = day,
                expireTotalHours = NowHours + (b != null ? b.crateLifetimeGameHours : 18f),
                isOpened = new bool[count]
            };
            contentDay = -1;
            EnsureContents();
            // 2.
            if (RegionStreaming.IsRegionLoaded(RegionType.Volcano)) SpawnCrates();
            // 3-4.
            EventChannel<BeachSupplyArrivedEvent>.Publish(new BeachSupplyArrivedEvent(day, count));
            if (NotificationManager.Instance != null) NotificationManager.Instance.Show("notif.beach_supply_arrived", NotificationKind.Objective);   // "Kargo kapal terdampar di pantai selatan"
            // 5. Penanda kompas di titik tengah kargo (#30).
            UpdateCompassMarker();
        }

        private void SpawnCrates()
        {
            var pooler = ObjectPooler.Instance;
            if (pooler == null || !HasDelivery) return;
            EnsureContents();
            var b = B;
            var tide = TideManager.Instance;
            float minWaterY = tide != null ? tide.WaterYAtLevel(b != null ? b.crateMinLevel : 0.85f) : float.MinValue;
            int count = state.isOpened.Length;
            int cursor = 0;
            int fallbackUsed = 0;
            for (int c = 0; c < count; c++)
            {
                if (liveCrates[c] != null) continue;
                Vector3 spot;
                if (!NextValidPoint(ref cursor, minWaterY, out spot))
                {
                    Vector3 basePos = fallbackPoint != null ? fallbackPoint.position : transform.position;
                    float spacing = b != null ? b.fallbackSpacing : 1.5f;
                    var rng = RunSeedManager.Instance != null ? RunSeedManager.Instance.Fork(RandomStreamId.BeachSupply, state.day * 31 + c) : new RandomStream(1);
                    float ang = rng.Range(0f, Mathf.PI * 2f);
                    spot = basePos + new Vector3(Mathf.Cos(ang), 0f, Mathf.Sin(ang)) * spacing * fallbackUsed;
                    fallbackUsed++;
                }
                crateSpots[c] = spot;
                var crate = pooler.SpawnFromPool<BeachCrate>(CratePoolTag, spot, Quaternion.Euler(0f, c * 47f, 0f));
                if (crate == null) continue;
                crate.Bind(this, c, state.isOpened[c]);
                liveCrates[c] = crate;
            }
        }

        private bool NextValidPoint(ref int cursor, float minWaterY, out Vector3 spot)
        {
            var b = B;
            spot = Vector3.zero;
            while (cursor < pointOrder.Length)
            {
                int idx = pointOrder[cursor++];
                if (idx < 0 || idx >= points.Length) continue;
                var p = points[idx];
                if (p == null || !p.isReachable) continue;
                Vector3 pos = p.Position;
                if (!NavMesh.SamplePosition(pos, out var hit, b != null ? b.pointNavSampleDistance : 2f, NavMesh.AllAreas)) continue;
                if (PhysicsQuery.OverlapSphere(pos, b != null ? b.pointClearRadius : 1.5f, PhysicsMasks.BuildOverlap, overlapBuffer) > 0)
                {
                    for (int i = 0; i < overlapBuffer.Length; i++) overlapBuffer[i] = null;
                    continue;
                }
                float ground = hit.position.y;
                if (SurfaceDetection.Instance != null && SurfaceDetection.Instance.TryGetTerrainSurfaceAtXZ(pos, out var s)) ground = s.groundHeight;
                if (ground <= minWaterY) continue;
                spot = hit.position;
                return true;
            }
            return false;
        }

        private void ReturnCrates(bool playSplash)
        {
            var pooler = ObjectPooler.Instance;
            for (int i = 0; i < liveCrates.Length; i++)
            {
                var c = liveCrates[i];
                if (c == null) continue;
                liveCrates[i] = null;
                if (playSplash && VFXManager.Instance != null && !string.IsNullOrEmpty(splashVfxTag))
                    VFXManager.Instance.Play(splashVfxTag, c.transform.position, Quaternion.identity);
                if (pooler != null) pooler.ReturnToPool(c.gameObject);
                else c.gameObject.SetActive(false);
            }
        }

        // ---------------------------------------------------------------- Membuka kargo

        public bool IsCrateOpened(int index) => state.isOpened != null && index >= 0 && index < state.isOpened.Length && state.isOpened[index];

        /// <summary>Dipanggil BeachCrate. Isi keluar sebagai WorldItem (despawns false), jadi inventori penuh tidak masalah.</summary>
        internal bool OpenCrate(int index, Vector3 position)
        {
            if (!HasDelivery || IsCrateOpened(index) || index < 0 || index >= state.isOpened.Length) return false;
            EnsureContents();
            // 1.
            state.isOpened[index] = true;
            // 2.
            var b = B;
            int rolls = b != null ? Mathf.Max(1, b.rollsPerCrate) : 3;
            var wim = WorldItemManager.Instance;
            var inv = InventoryManager.Instance;
            for (int r = 0; r < rolls; r++)
            {
                int k = index * rolls + r;
                if (k >= contents.Length) break;
                var st = contents[k];
                if (st.item == null || wim == null) continue;
                var inst = inv != null ? inv.CreateInstance(st.item) : new ItemInstance { itemId = st.item.itemID, durability = st.item.maxDurability };
                Vector3 p = position + Vector3.up * 0.6f + new Vector3((r - 1) * 0.35f, 0f, 0.4f);
                wim.Spawn(inst, st.amount, p, false);
            }
            // 4. LocalStatsManager (#118) mendengar event ini.
            EventChannel<BeachCrateOpenedEvent>.Publish(new BeachCrateOpenedEvent(state.day));
            // 5.
            if (AllOpened())
            {
                // Semua kargo terbuka: penanda kompas dihapus (#30).
                if (CompassSystem.Instance != null) CompassSystem.Instance.RemoveMarker(CompassMarkerId);
            }
            return true;
        }

        private bool AllOpened()
        {
            if (state.isOpened == null) return true;
            for (int i = 0; i < state.isOpened.Length; i++) if (!state.isOpened[i]) return false;
            return true;
        }

        // ---------------------------------------------------------------- Batas waktu

        private void ExpireDelivery()
        {
            bool anyClosed = !AllOpened();
            // 1-2.
            ReturnCrates(true);
            // 3.
            bool inRegion1 = IslandPlayerController.Instance != null && RegionStreaming.RegionOf(IslandPlayerController.Instance.transform.position) == RegionType.Volcano;
            if (anyClosed && inRegion1 && NotificationManager.Instance != null)
                NotificationManager.Instance.Show("notif.beach_supply_expired", NotificationKind.Info);   // "Kargo terseret air pasang"
            // 4. Hapus penanda kompas (#30).
            if (CompassSystem.Instance != null) CompassSystem.Instance.RemoveMarker(CompassMarkerId);
            state = new BeachDeliveryState();
            contentDay = -1;
        }

        // ---------------------------------------------------------------- Risiko pantai

        private void TickAmbush()
        {
            if (!HasDelivery || AllOpened()) return;
            var player = IslandPlayerController.Instance;
            var spawn = SpawnManager.Instance;
            var b = B;
            if (player == null || spawn == null || b == null) return;
            Vector3 center = CrateCenter();
            if ((player.transform.position - center).sqrMagnitude > b.ambushTriggerDistance * b.ambushTriggerDistance) return;
            int day = state.day;
            bool night = DayNightDirector.Instance != null && DayNightDirector.Instance.IsNight;
            // GDD bagian 3: hewan buas siang, zombie malam.
            if (!night && !state.animalAmbushDone)
            {
                state.animalAmbushDone = true;
                int animals = Mathf.Min(b.ambushAnimalsBase + day / 21, b.ambushAnimalsMax);
                if (beachDayTable != null) spawn.RequestBurst(beachDayTable, animals, center, b.ambushSpawnRadius);
            }
            else if (night && !state.zombieAmbushDone)
            {
                state.zombieAmbushDone = true;
                // Keputusan developer: setelah hari ke-60 memakai pengali AIDirector.md.
                var director = AIDirector.Instance;
                float mul = director != null ? director.GetPostTargetCountMultiplier(day) : 1f;
                int zombies = Mathf.RoundToInt(Mathf.Min(b.ambushZombiesBase + day / 7, b.ambushZombiesMax) * mul);
                if (beachNightTable != null)
                {
                    if (director != null) spawn.RequestBurst(beachNightTable, zombies, center, b.ambushSpawnRadius, director.GetPostTargetModifiers(day));
                    else spawn.RequestBurst(beachNightTable, zombies, center, b.ambushSpawnRadius);
                }
            }
        }

        private const string CompassMarkerId = "supply";

        /// <summary>Penanda kompas kargo (#30): ada selama masih ada kargo tertutup.</summary>
        private void UpdateCompassMarker()
        {
            var cs = CompassSystem.Instance;
            if (cs == null) return;
            if (!HasDelivery || AllOpened()) cs.RemoveMarker(CompassMarkerId);
            else cs.SetMarker(CompassMarkerId, CrateCenter(), MarkerIcon.Supply);
        }

        private Vector3 CrateCenter()
        {
            Vector3 sum = Vector3.zero;
            int n = 0;
            for (int i = 0; i < liveCrates.Length; i++)
                if (liveCrates[i] != null) { sum += crateSpots[i]; n++; }
            if (n > 0) return sum / n;
            return fallbackPoint != null ? fallbackPoint.position : transform.position;
        }

        // ---------------------------------------------------------------- DevConsole

        /// <summary>Paksa kiriman hari tertentu (setday + fajar di DevConsole #56).</summary>
        public void DebugDeliver(int day) => Deliver(day);

        // ---------------------------------------------------------------- Save (world_supply_beach)

        public string SaveId => SaveIdConst;
        public int LoadOrder => 130;

        public string CaptureState() => JsonUtility.ToJson(state ?? new BeachDeliveryState());

        public void RestoreState(string json)
        {
            ReturnCrates(false);
            state = new BeachDeliveryState();
            contentDay = -1;
            if (string.IsNullOrEmpty(json)) return;
            try { state = JsonUtility.FromJson<BeachDeliveryState>(json) ?? new BeachDeliveryState(); }
            catch (Exception e) { Debug.LogWarning($"[BeachSupplyManager] Save tidak terbaca: {e.Message}"); }
            if (state.isOpened == null) state.isOpened = new bool[0];
        }

        /// <summary>Kargo tertutup muncul lagi dengan isi yang sama. Notifikasi tidak diulang.</summary>
        public void OnRestoreFinished()
        {
            if (!HasDelivery) return;
            if (RegionStreaming.IsRegionLoaded(RegionType.Volcano)) SpawnCrates();
            // Pasang lagi penanda kompas kalau masih ada kargo tertutup (#30).
            UpdateCompassMarker();
        }
    }
}
