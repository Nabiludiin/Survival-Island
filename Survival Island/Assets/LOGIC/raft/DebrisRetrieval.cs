using System;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Barang hanyut di laut yang ditarik dengan Base_Kail, memakai lemparan yang sama seperti memancing.
    /// Singleton di IslandPhase. Blueprint: raft/DebrisRetrieval.md (#106).
    /// GDD bagian 3: selain memancing, Kail bisa menarik barang hanyut dari laut, seperti di game Raft.
    /// Isi, laju, dan pengaruh cuaca/pasang adalah usulan yang disetujui developer sebagai nilai awal.
    ///
    /// Catatan implementasi (sistem pendukung):
    /// - TideManager (#91) belum ada: tideIsHigh tetap false (TODO di HandleTide).
    /// - Isi barang memakai RegionLootTable.TryRoll dengan LootPointKind.Any, sesuai RegionLootTable.md.
    /// </summary>
    [DisallowMultipleComponent]
    public class DebrisRetrieval : MonoBehaviour, IKailCastInterceptor, ISaveable
    {
        public const string SaveIdConst = "world_debris";

        public static DebrisRetrieval Instance { get; private set; }

        [Tooltip("LootTable Debris_Default (#103)")]
        [SerializeField] private LootTable debrisTable;

        private readonly List<DebrisLane> lanes = new List<DebrisLane>(8);
        private readonly List<DebrisItem> active = new List<DebrisItem>(8);
        private readonly List<DebrisLane> laneCandidates = new List<DebrisLane>(8);
        private float nextSpawnGameHour = -1f;
        private float checkTimer;
        private DebrisItem hooked;
        private FishingSystem kail;
        private bool tideIsHigh;
        private bool waitingForSpace;   // Barang tertahan karena inventori penuh
        private bool spaceFreed;
        private bool interceptorRegistered;

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;
        private static bool Enabled => Balance == null || Balance.debris.enabled;

        public IReadOnlyList<DebrisItem> Active => active;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            if (WorldStateSave.Instance != null) WorldStateSave.Instance.Register(this);
            // TODO(TideManager #91): EventChannel<TideChangedEvent>.Raised += HandleTide; lalu baca Last (sticky).
            // Sakelar uji: tidak mendaftar ke FishingSystem dan tidak memunculkan barang.
            if (!Enabled) return;
            FishingSystem.RegisterInterceptor(this);
            interceptorRegistered = true;
        }

        private void OnDisable()
        {
            if (WorldStateSave.Instance != null) WorldStateSave.Instance.Unregister(this);
            if (interceptorRegistered) FishingSystem.UnregisterInterceptor(this);
            interceptorRegistered = false;
            StopWaitingForSpace();
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private static float Now()
        {
            return DayNightDirector.Instance != null ? (float)DayNightDirector.Instance.TotalGameHours : ItemUseSystem.CurrentTotalGameHours();
        }

        // ---------------------------------------------------------------- Jalur

        public void RegisterLane(DebrisLane lane)
        {
            if (lane != null && !lanes.Contains(lane)) lanes.Add(lane);
        }

        /// <summary>Region dilepas: barang di jalur ini kembali ke pool. Yang tersangkut dilepas dari kail lebih dulu.</summary>
        public void UnregisterLane(DebrisLane lane)
        {
            lanes.Remove(lane);
            for (int i = active.Count - 1; i >= 0; i--)
            {
                var d = active[i];
                if (d == null || d.lane != lane) continue;
                if (d == hooked)
                {
                    var k = kail;
                    hooked = null;
                    kail = null;
                    StopWaitingForSpace();
                    if (k != null) k.EndIntercept();
                }
                Despawn(d);
            }
        }

        // ---------------------------------------------------------------- Kemunculan

        private void Update()
        {
            if (!Enabled) return;
            checkTimer += Time.deltaTime;
            float every = Balance != null ? Balance.debris.spawnCheckSeconds : 1f;
            if (checkTimer < every) return;
            checkTimer = 0f;
            TrySpawn();
        }

        private void TrySpawn()
        {
            float now = Now();
            if (nextSpawnGameHour < 0f) { nextSpawnGameHour = now + CurrentInterval(); return; }
            // 1.
            if (now < nextSpawnGameHour) return;
            // 2.
            nextSpawnGameHour = now + CurrentInterval();
            // 3.
            int max = Balance != null ? Balance.debris.maxActive : 4;
            if (active.Count >= max) return;
            var pooler = ObjectPooler.Instance;
            if (pooler == null || !pooler.HasPool(DebrisItem.PoolTag) || RunSeedManager.Instance == null) return;

            // 4. Jalur acak dalam spawnPlayerMaxDistance, aliran Gameplay.
            var player = IslandPlayerController.Instance;
            if (player == null) return;
            float maxDist = Balance != null ? Balance.debris.spawnPlayerMaxDistance : 150f;
            laneCandidates.Clear();
            for (int i = 0; i < lanes.Count; i++)
                if (lanes[i] != null && lanes[i].IsValid && lanes[i].DistanceXZ(player.transform.position) <= maxDist)
                    laneCandidates.Add(lanes[i]);
            if (laneCandidates.Count == 0) return;
            ref var rng = ref RunSeedManager.Instance.GetStream(RandomStreamId.Gameplay);
            var lane = laneCandidates[rng.Range(0, laneCandidates.Count)];

            // 5. Isi dari Debris_Default.
            if (debrisTable == null) return;
            int day = GameManager.Instance != null ? GameManager.Instance.currentDay : 1;
            if (!RegionLootTable.TryRoll(debrisTable, LootPointKind.Any, day, ref rng, out var item, out int amount) || item == null) return;

            // 6.
            var d = pooler.SpawnFromPool<DebrisItem>(DebrisItem.PoolTag, lane.points[0], Quaternion.identity);
            if (d == null) return;
            d.Setup(item, amount, lane, rng.NextFloat01() * Mathf.PI * 2f);
            active.Add(d);
        }

        private float CurrentInterval()
        {
            var b = Balance != null ? Balance.debris : null;
            float i = b != null ? b.spawnIntervalGameHours : 3f;
            WeatherType w = WeatherNearPlayer();
            // Usulan disetujui #106: badai membuat barang lebih banyak.
            if (w == WeatherType.Storm) i *= b != null ? b.stormIntervalMultiplier : 0.5f;
            else if (w == WeatherType.Rain) i *= b != null ? b.rainIntervalMultiplier : 0.75f;
            if (tideIsHigh) i *= b != null ? b.highTideIntervalMultiplier : 0.75f;
            return Mathf.Max(0.05f, i);
        }

        /// <summary>Cuaca jalur terdekat dari pemain. Tanpa jalur: region pemain.</summary>
        private WeatherType WeatherNearPlayer()
        {
            var rw = RegionalWeather.Instance;
            if (rw == null) return WeatherType.Clear;
            var player = IslandPlayerController.Instance;
            RegionType region = ZoneTracker.Active;
            if (player != null)
            {
                float best = float.MaxValue;
                for (int i = 0; i < lanes.Count; i++)
                {
                    if (lanes[i] == null) continue;
                    float d = lanes[i].DistanceXZ(player.transform.position);
                    if (d < best) { best = d; region = lanes[i].region; }
                }
            }
            return rw.GetWeather(region);
        }

        // TODO(TideManager #91): pasang dari TideChangedEvent.isHigh.
        // private void HandleTide(TideChangedEvent e) => tideIsHigh = e.isHigh;

        // ---------------------------------------------------------------- IKailCastInterceptor

        public bool IsTargetAt(Vector3 landing) => FindNearest(landing) != null;

        public bool TryIntercept(Vector3 landing, FishingSystem k)
        {
            // 1.
            var item = FindNearest(landing);
            if (item == null) return false;
            // 2.
            hooked = item;
            kail = k;
            item.isHooked = true;
            // 3. Benang tergambar ke barang.
            if (k != null) k.SetInterceptTarget(item.transform);
            return true;
        }

        private DebrisItem FindNearest(Vector3 landing)
        {
            // Daftar paling banyak maxActive barang: tanpa query fisika.
            float r = Balance != null ? Balance.debris.hookRadius : 2.5f;
            float best = r * r;
            DebrisItem found = null;
            for (int i = 0; i < active.Count; i++)
            {
                var d = active[i];
                if (d == null || d.isHooked) continue;
                Vector3 p = d.transform.position;
                float dx = p.x - landing.x, dz = p.z - landing.z;
                float d2 = dx * dx + dz * dz;
                if (d2 <= best) { best = d2; found = d; }
            }
            return found;
        }

        public void TickIntercept(bool isHeld, float dt)
        {
            // 1.
            if (hooked == null)
            {
                var k = kail;
                kail = null;
                StopWaitingForSpace();
                if (k != null) k.EndIntercept();
                return;
            }
            var player = IslandPlayerController.Instance;
            if (player == null) return;
            Vector3 pp = player.transform.position;
            Vector3 pos = hooked.transform.position;
            // 2. Tarik di bidang XZ, Y tetap. Tombol dilepas: barang diam. Tanpa Tension Bar (usulan disetujui #106).
            if (isHeld && !waitingForSpace)
            {
                float speed = Balance != null ? Balance.debris.pullSpeed : 4f;
                Vector3 target = new Vector3(pp.x, pos.y, pp.z);
                hooked.transform.position = Vector3.MoveTowards(pos, target, speed * dt);
                pos = hooked.transform.position;
            }
            float dx = pos.x - pp.x, dz = pos.z - pp.z;
            float distXZ = Mathf.Sqrt(dx * dx + dz * dz);
            // 3.
            float maxPull = Balance != null ? Balance.debris.pullMaxDistance : 32f;
            if (distXZ > maxPull)
            {
                var k = kail;
                ReleaseHooked();
                if (k != null) k.EndIntercept();
                return;
            }
            // 4.
            float collect = Balance != null ? Balance.debris.collectDistance : 3f;
            if (distXZ < collect && (!waitingForSpace || spaceFreed)) Collect();
        }

        public void CancelIntercept()
        {
            // FishingSystem sudah mengakhiri sesinya, jadi EndIntercept tidak dipanggil.
            ReleaseHooked();
        }

        private void ReleaseHooked()
        {
            StopWaitingForSpace();
            if (hooked != null) hooked.ResumeDrift();
            if (kail != null) kail.SetInterceptTarget(null);
            hooked = null;
            kail = null;
        }

        private void Collect()
        {
            spaceFreed = false;
            var inv = InventoryManager.Instance;
            if (inv == null || hooked == null) return;
            // 1.
            var item = hooked.item;
            int amount = hooked.amount;
            int left = inv.AddItem(item, amount);
            // 2.
            if (left == 0)
            {
                EventChannel<DebrisCollectedEvent>.Publish(new DebrisCollectedEvent(item != null ? item.itemID : string.Empty, amount));
                var k = kail;
                var d = hooked;
                hooked = null;
                kail = null;
                StopWaitingForSpace();
                Despawn(d);
                if (k != null) k.EndIntercept();
                return;
            }
            // 3. Inventori penuh: sisa tetap mengapung, bisa diambil dengan E selama kail belum dilepas.
            hooked.amount = left;
            if (left < amount)
                EventChannel<DebrisCollectedEvent>.Publish(new DebrisCollectedEvent(item != null ? item.itemID : string.Empty, amount - left));
            if (NotificationManager.Instance != null) NotificationManager.Instance.Show("notif.inventory_full", NotificationKind.Warning);
            StartWaitingForSpace();
        }

        /// <summary>Ambil dengan E (barang tertahan karena inventori penuh).</summary>
        public bool TryPickUp(DebrisItem d, InventoryManager inv)
        {
            if (d == null || inv == null || d.item == null) return false;
            int amount = d.amount;
            int left = inv.AddItem(d.item, amount);
            if (left == amount)
            {
                if (NotificationManager.Instance != null) NotificationManager.Instance.Show("notif.inventory_full", NotificationKind.Warning);
                return false;
            }
            EventChannel<DebrisCollectedEvent>.Publish(new DebrisCollectedEvent(d.item.itemID, amount - left));
            if (left > 0) { d.amount = left; return true; }
            if (d == hooked)
            {
                var k = kail;
                hooked = null;
                kail = null;
                StopWaitingForSpace();
                Despawn(d);
                if (k != null) k.EndIntercept();
            }
            else Despawn(d);
            return true;
        }

        // InventoryChangedEvent didengar hanya saat barang tertahan karena inventori penuh.
        private void StartWaitingForSpace()
        {
            if (waitingForSpace) return;
            waitingForSpace = true;
            spaceFreed = false;
            EventChannel<InventoryChangedEvent>.Raised += HandleInventoryChanged;
        }

        private void StopWaitingForSpace()
        {
            if (!waitingForSpace) return;
            waitingForSpace = false;
            spaceFreed = false;
            EventChannel<InventoryChangedEvent>.Raised -= HandleInventoryChanged;
        }

        private void HandleInventoryChanged(InventoryChangedEvent e)
        {
            // Ada item keluar: mungkin ada ruang, Collect dicoba lagi.
            if (e.delta < 0) spaceFreed = true;
        }

        // ---------------------------------------------------------------- Pool

        public void Despawn(DebrisItem d)
        {
            if (d == null) return;
            if (d == hooked) hooked = null;   // TickIntercept berikutnya mengakhiri sesi
            active.Remove(d);
            var pooler = ObjectPooler.Instance;
            if (pooler != null) pooler.ReturnToPool(DebrisItem.PoolTag, d.gameObject);
            else d.gameObject.SetActive(false);
        }

        // ---------------------------------------------------------------- ISaveable

        [Serializable]
        private class SaveData
        {
            public float nextSpawnGameHour = -1f;
        }

        public string SaveId => SaveIdConst;
        public int LoadOrder => 130;

        public string CaptureState()
        {
            // Barang yang sedang hanyut tidak disimpan (usulan disetujui #106).
            return JsonUtility.ToJson(new SaveData { nextSpawnGameHour = nextSpawnGameHour });
        }

        public void RestoreState(string json)
        {
            if (string.IsNullOrEmpty(json)) return;
            SaveData d = null;
            try { d = JsonUtility.FromJson<SaveData>(json); }
            catch (Exception e) { Debug.LogWarning($"[DebrisRetrieval] Save tidak terbaca: {e.Message}"); }
            if (d != null) nextSpawnGameHour = d.nextSpawnGameHour;
        }

        public void OnRestoreFinished() { }
    }
}
