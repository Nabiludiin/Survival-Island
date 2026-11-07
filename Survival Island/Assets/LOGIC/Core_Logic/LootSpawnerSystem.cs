using System;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Definisi satu titik loot (GLOSSARY.md, RegionLootTable.md #103).</summary>
    [Serializable]
    public class LootPointDef
    {
        public Transform point;
        public LootPointKind kind = LootPointKind.House;
        [Tooltip("Isi tetap. Hanya elemen pertama yang dipakai (satu titik satu tumpukan).")]
        public ItemStack[] guaranteedItems = new ItemStack[0];
    }

    /// <summary>Status satu titik loot yang disimpan (GLOSSARY.md, #89). itemId kosong berarti titik kosong.</summary>
    [Serializable]
    public struct LootPointState
    {
        public int pointIndex;
        public string itemId;
        public int amount;
        public bool isTaken;
    }

    [Serializable]
    internal class LootSpawnerSaveData
    {
        public int version = 1;
        public List<LootPointState> points = new List<LootPointState>();
    }

    /// <summary>
    /// Menyebar loot di titik tertentu, di kapal maupun di pulau, dari seed run, lalu mengingat hasilnya supaya tidak berlipat
    /// setelah load. Blueprint: Core_Logic/LootSpawnerSystem.md (#89, #74, #103). GDD bagian 1 (scavenging) dan bagian 3
    /// (looting di kapal pada Fase 1, looting di pulau). Peluang kosong 50/25 persen adalah usulan disetujui developer (#85).
    ///
    /// Satu komponen = satu kelompok titik (satu rumah, gudang, atau dek kapal). Kapal tidak disimpan (#82).
    ///
    /// Catatan implementasi (sistem pendukung), lihat juga bagian akhir blueprint:
    /// - Pulau dengan RegionStreaming (#88): homeRegion dari RegionSceneRoot induk, status lewat RegionStateStore, dan
    ///   penyebaran dipicu RegionLoadStateChangedEvent Loaded. Tidak mendaftar ke WorldStateSave.
    /// - Scene uji tanpa RegionStreaming (keputusan sesi induk): jalur cadangan tetap ada lewat RegionSaveRouting.
    ///   homeRegion dari Inspector, spawner mendaftar ke WorldStateSave, penyebaran dipicu Start() (atau OnRestoreFinished
    ///   saat melanjutkan save).
    /// - WorldItem membawa sourceSpawner/sourcePointIndex (#66) dan memanggil MarkTaken/UpdateRemaining sendiri.
    /// </summary>
    public class LootSpawnerSystem : MonoBehaviour, ISaveable
    {
        public const int SaveDataVersion = 1;
        public const string ShipPoolTag = "ShipLoot_Generic";

        [Tooltip("ID stabil, juga bagian kunci acak. Format <region>_loot_<8>, contoh r4_loot_9c01d2ef; kapal ship_loot_<8>.")]
        [SerializeField] private string spawnerId;
        [Tooltip("Kapal memakai Loot_Ship. Spawner di sarang memakai data.lootTable (#112).")]
        [SerializeField] private LootTable table;
        [Tooltip("Urutan TIDAK boleh diubah setelah rilis. Titik baru ditambahkan di akhir.")]
        [SerializeField] private LootPointDef[] spawnPoints = new LootPointDef[0];
        [Tooltip("Dibaca dari RegionSceneRoot induk kalau ada (#88). Isian ini hanya untuk scene uji tanpa RegionSceneRoot.")]
        [SerializeField] private RegionType homeRegion = RegionType.Volcano;

        private readonly List<LootPointState> states = new List<LootPointState>();   // Indeks = pointIndex
        private WorldItem[] liveItems = new WorldItem[0];
        private ShipInteractable[] liveShipItems = new ShipInteractable[0];
        private bool hasSpawned;
        private string restoredJson;
        private bool registeredForSave;
        private bool waitingForRestore;

        public string SpawnerId => spawnerId;
        public RegionType HomeRegion => homeRegion;
        public LootTable Table => table;
        public IReadOnlyList<LootPointState> States => states;
        /// <summary>Definisi titik, dibaca menu Editor RunLayoutSources untuk menyalin spawner kapal (#89).</summary>
        public IReadOnlyList<LootPointDef> SpawnPointDefs => spawnPoints;

        private static bool IsShipPhase =>
            GameManager.Instance != null && GameManager.Instance.currentPhase == GamePhase.SinkingShip;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            EnsureArrays();
            // homeRegion dari RegionSceneRoot induk (LootSpawnerSystem.md "Variabel").
            if (RegionSceneRoot.TryGetRegionOf(this, out var r)) homeRegion = r;
        }

        private void OnEnable()
        {
            EventChannel<RegionLoadStateChangedEvent>.Raised += HandleRegionLoadState;
            // Objek region tidak memanggil WorldStateSave.Register (WorldStateSave.md); RegionSaveRouting memilih jalurnya.
            // Kapal tidak disimpan (#82).
            if (!IsShipPhase) registeredForSave = RegionSaveRouting.Register(this, this);
        }

        private void OnDisable()
        {
            EventChannel<RegionLoadStateChangedEvent>.Raised -= HandleRegionLoadState;
            RegionSaveRouting.Unregister(this, ref registeredForSave);
        }

        private void Start()
        {
            // Kapal: selalu RollAndRecord (tidak punya save, #82).
            if (IsShipPhase)
            {
                SpawnOrRestore();
                return;
            }

            // Pulau dengan RegionStreaming: dipicu event Loaded (HandleRegionLoadState). Kalau region sudah Loaded sebelum
            // spawner aktif, event itu terlewat, jadi sebar di sini. RestoreState sudah masuk lewat RestoreRegion.
            if (RegionSaveRouting.IsStoredByRegion(this))
            {
                if (RegionStreaming.Instance.IsLoaded(homeRegion)) SpawnOrRestore();
                return;
            }

            // Pulau tanpa RegionStreaming. Saat melanjutkan save, data masuk lewat RestoreState sesudah scene dimuat,
            // jadi tunggu OnRestoreFinished supaya loot tidak diacak ulang (alasan sama dengan pemicu event Loaded di blueprint).
            if (WorldStateSave.Instance != null && WorldStateSave.Instance.LoadedGame != null && registeredForSave)
            {
                waitingForRestore = true;
                return;
            }
            SpawnOrRestore();
        }

        private void EnsureArrays()
        {
            int n = spawnPoints != null ? spawnPoints.Length : 0;
            if (liveItems.Length != n) liveItems = new WorldItem[n];
            if (liveShipItems.Length != n) liveShipItems = new ShipInteractable[n];
        }

        // ---------------------------------------------------------------- Menyebar atau memulihkan

        private void SpawnOrRestore()
        {
            // 1.
            if (hasSpawned) return;
            hasSpawned = true;
            EnsureArrays();

            // 2-3.
            if (!string.IsNullOrEmpty(restoredJson)) RestoreFromData();
            else RollAndRecord();

            // 4.
            for (int i = 0; i < states.Count; i++)
            {
                var st = states[i];
                if (!st.isTaken && !string.IsNullOrEmpty(st.itemId)) SpawnPoint(in st);
            }
        }

        private void RestoreFromData()
        {
            LootSpawnerSaveData data = null;
            try { data = JsonUtility.FromJson<LootSpawnerSaveData>(restoredJson); }
            catch (Exception ex) { Debug.LogWarning($"[LootSpawnerSystem] {spawnerId}: data save rusak ({ex.Message}). Diacak ulang.", this); }

            // 1. Rusak: acak ulang.
            if (data == null || data.points == null)
            {
                RollAndRecord();
                return;
            }
            if (data.version != SaveDataVersion)
                Debug.LogWarning($"[LootSpawnerSystem] {spawnerId}: versi data {data.version} berbeda dari {SaveDataVersion}.", this);

            int n = spawnPoints.Length;
            var rows = new LootPointState[n];
            var hasRow = new bool[n];

            // 2. Salin baris yang titiknya masih ada.
            for (int i = 0; i < data.points.Count; i++)
            {
                var row = data.points[i];
                if (row.pointIndex < 0 || row.pointIndex >= n)
                {
                    Debug.LogWarning($"[LootSpawnerSystem] {spawnerId}: titik {row.pointIndex} tidak ada lagi. Baris dibuang.", this);
                    continue;
                }
                rows[row.pointIndex] = row;
                hasRow[row.pointIndex] = true;
            }

            // 3. Titik baru dari versi game yang lebih baru: acak titik itu saja. 4. Titik lama tidak diacak.
            int day = CurrentDay();
            var streamId = IsShipPhase ? RandomStreamId.ShipLoot : RandomStreamId.IslandLoot;
            states.Clear();
            for (int i = 0; i < n; i++)
            {
                if (hasRow[i]) states.Add(rows[i]);
                else states.Add(MakeState(i, streamId, day));
            }
        }

        private void RollAndRecord()
        {
            // RegionLootTable.md: hari selalu 1 di kapal.
            int day = CurrentDay();
            var streamId = IsShipPhase ? RandomStreamId.ShipLoot : RandomStreamId.IslandLoot;
            states.Clear();
            for (int i = 0; i < spawnPoints.Length; i++) states.Add(MakeState(i, streamId, day));
        }

        private LootPointState MakeState(int index, RandomStreamId streamId, int day)
        {
            bool ok = RollPoint(spawnPoints[index], table, spawnerId, index, streamId, day, out string itemId, out int amount);
            // Titik kosong dicatat dengan isTaken = true supaya load tidak mengacak ulang.
            return ok
                ? new LootPointState { pointIndex = index, itemId = itemId, amount = amount, isTaken = false }
                : new LootPointState { pointIndex = index, itemId = string.Empty, amount = 0, isTaken = true };
        }

        private static int CurrentDay()
        {
            if (IsShipPhase) return 1;
            return GameManager.Instance != null ? Mathf.Max(1, GameManager.Instance.currentDay) : 1;
        }

        /// <summary>
        /// Fungsi murni: hasil hanya bergantung pada seed run, spawnerId, pointIndex, dan hari. Dipakai juga RunSeedManager
        /// untuk syarat 4 (minShipFoodItems dan lainnya) lewat RunLayoutSources, jadi hasilnya harus sama persis.
        /// Urutan aliran acak tetap: lempar kosong lalu TryRoll.
        /// </summary>
        public static bool RollPoint(LootPointDef def, LootTable table, string spawnerId, int pointIndex,
                                     RandomStreamId streamId, int day, out string itemId, out int amount)
        {
            itemId = string.Empty;
            amount = 0;
            if (def == null) return false;
            return RollPointCore(def.kind, def.guaranteedItems, table, spawnerId, pointIndex, streamId, day, false, 0u, out itemId, out amount);
        }

        /// <summary>
        /// Sama dengan RollPoint, tetapi untuk seed tertentu dan tanpa LootPointDef (tanpa Transform). Dipakai RunLayoutValidator
        /// syarat 4 lewat RunLayoutSources.shipLootSpawners, sebelum seed calon ditetapkan (#89). Hasil sama persis dengan
        /// RollPoint saat CurrentSeed = seed.
        /// </summary>
        public static bool RollPointForSeed(LootPointKind kind, ItemStack[] guaranteedItems, LootTable table, string spawnerId,
                                            int pointIndex, RandomStreamId streamId, int day, uint seed,
                                            out string itemId, out int amount)
        {
            return RollPointCore(kind, guaranteedItems, table, spawnerId, pointIndex, streamId, day, true, seed, out itemId, out amount);
        }

        private static bool RollPointCore(LootPointKind kind, ItemStack[] guaranteedItems, LootTable table, string spawnerId,
                                          int pointIndex, RandomStreamId streamId, int day, bool useSeed, uint seed,
                                          out string itemId, out int amount)
        {
            itemId = string.Empty;
            amount = 0;

            // 2. Isi tetap: titik ini tidak bisa kosong (RegionLootTable.md, "Isi tetap").
            if (guaranteedItems != null && guaranteedItems.Length > 0 && guaranteedItems[0].item != null)
            {
                itemId = guaranteedItems[0].item.itemID;
                amount = Mathf.Max(1, guaranteedItems[0].amount);
                return true;
            }

            // 1. Aliran berkunci per titik.
            RandomStream s;
            if (useSeed) s = RunSeedManager.ForkForSeed(seed, streamId, spawnerId + "/" + pointIndex);
            else
            {
                if (RunSeedManager.Instance == null)
                {
                    Debug.LogWarning("[LootSpawnerSystem] RunSeedManager belum ada. Titik dibiarkan kosong.");
                    return false;
                }
                s = RunSeedManager.Instance.Fork(streamId, spawnerId + "/" + pointIndex);
            }

            // 3. Peluang kosong. Usulan disetujui developer: 50 persen biasa, 25 persen gudang (#85).
            var bal = GameManager.CurrentBalance;
            float emptyChance = 0f;
            if (bal != null)
                emptyChance = (kind & LootPointKind.Warehouse) != 0 ? bal.loot.warehouseEmptyChance : bal.runSeed.lootPointEmptyChance;
            if (s.NextFloat01() < emptyChance) return false;

            // 4.
            if (!RegionLootTable.TryRoll(table, kind, day, ref s, out ItemData item, out amount))
            {
                Debug.LogWarning($"[LootSpawnerSystem] {spawnerId}/{pointIndex}: tabel {(table != null ? table.name : "kosong")} tidak punya isi untuk {kind}.");
                return false;
            }

            // 5.
            itemId = item.itemID;
            return true;
        }

        // ---------------------------------------------------------------- Memunculkan wujud

        private void SpawnPoint(in LootPointState st)
        {
            int i = st.pointIndex;
            var def = spawnPoints[i];
            if (def == null || def.point == null) return;

            // 1.
            var data = ItemDatabase.Get(st.itemId);
            if (data == null)
            {
                Debug.LogWarning($"[LootSpawnerSystem] {spawnerId}: item {st.itemId} tidak ada di ItemDatabase. Titik {i} ditandai diambil.", this);
                var row = states[i];
                row.isTaken = true;
                row.amount = 0;
                states[i] = row;
                return;
            }

            if (IsShipPhase)
            {
                // Kapal: ShipInteractable dari pool, jumlah ambil memakai ItemData.shipPickupAmount (ShipInteractable.md).
                if (ObjectPooler.Instance == null) return;
                var ship = ObjectPooler.Instance.SpawnFromPool<ShipInteractable>(ShipPoolTag, def.point.position, def.point.rotation);
                if (ship == null) return;   // 4. Pool penuh: baris tetap belum diambil
                ship.Setup(data);
                liveShipItems[i] = ship;
                return;
            }

            // Pulau: WorldItem lewat WorldItemManager (#66), tidak hilang otomatis.
            if (WorldItemManager.Instance == null) return;
            var inst = InventoryManager.Instance != null
                ? InventoryManager.Instance.CreateInstance(data)
                : new ItemInstance { itemId = data.itemID, durability = data.maxDurability };
            var w = WorldItemManager.Instance.Spawn(inst, st.amount, def.point.position, false);
            if (w == null) return;   // 4. Pool WorldItem_Generic habis: dicoba lagi saat region dimuat berikutnya
            w.transform.rotation = def.point.rotation;
            w.sourceSpawner = this;
            w.sourcePointIndex = i;
            liveItems[i] = w;
        }

        // ---------------------------------------------------------------- Saat barang diambil

        /// <summary>Dipanggil WorldItem saat isinya habis diambil (ItemUseSystem.md).</summary>
        public void MarkTaken(int pointIndex)
        {
            if (pointIndex < 0 || pointIndex >= states.Count) return;
            var row = states[pointIndex];
            row.isTaken = true;
            row.amount = 0;
            states[pointIndex] = row;
            if (pointIndex < liveItems.Length) liveItems[pointIndex] = null;
        }

        /// <summary>Dipanggil WorldItem saat sebagian terambil karena inventori penuh. Mencegah barang berlipat setelah load.</summary>
        public void UpdateRemaining(int pointIndex, int amount)
        {
            if (pointIndex < 0 || pointIndex >= states.Count) return;
            if (amount <= 0)
            {
                MarkTaken(pointIndex);
                return;
            }
            var row = states[pointIndex];
            row.amount = amount;
            states[pointIndex] = row;
        }

        private bool IsOurs(WorldItem w, int i) =>
            w != null && w.gameObject.activeInHierarchy && w.sourceSpawner == this && w.sourcePointIndex == i;

        /// <summary>
        /// Pengaman kalau UpdateRemaining terlewat: salin Amount dari objek yang masih milik spawner ini
        /// (sourceSpawner == this dan sourcePointIndex == i). Objek yang sudah kembali ke pool lewat jalur lain dilewati,
        /// karena OnReturned mengosongkan sourceSpawner; barisnya tidak diubah (LootSpawnerSystem.md "Region dilepas").
        /// </summary>
        private void SyncLiveItems()
        {
            for (int i = 0; i < liveItems.Length && i < states.Count; i++)
            {
                var w = liveItems[i];
                if (w == null) continue;
                if (!IsOurs(w, i)) liveItems[i] = null;
                else if (w.Amount != states[i].amount) UpdateRemaining(i, w.Amount);
            }
        }

        // ---------------------------------------------------------------- Region dilepas

        /// <summary>
        /// Dipanggil saat RegionLoadStateChangedEvent { region == homeRegion, state == Unloading } (#88).
        /// </summary>
        public void ReleaseForUnload()
        {
            // 1.
            SyncLiveItems();
            // 2.
            for (int i = 0; i < liveItems.Length; i++)
            {
                var w = liveItems[i];
                if (IsOurs(w, i) && WorldItemManager.Instance != null) WorldItemManager.Instance.Despawn(w);
                liveItems[i] = null;
            }
            // 3. states tetap di memori; RestoreState mengisi data terbaru saat region dimuat kembali.
            hasSpawned = false;
            restoredJson = null;
        }

        /// <summary>Pemicu pulau (#88): Loaded menyebar atau memulihkan, Unloading melepas objek. Hanya untuk homeRegion.</summary>
        private void HandleRegionLoadState(RegionLoadStateChangedEvent e)
        {
            if (IsShipPhase || e.region != homeRegion || !RegionSaveRouting.IsStoredByRegion(this)) return;
            if (e.state == RegionLoadState.Loaded) SpawnOrRestore();
            else if (e.state == RegionLoadState.Unloading) ReleaseForUnload();
        }

        // ---------------------------------------------------------------- ISaveable

        public string SaveId => spawnerId;

        /// <summary>Objek region memakai 0 (WorldStateSave.md).</summary>
        public int LoadOrder => 0;

        public string CaptureState()
        {
            if (!hasSpawned && states.Count == 0)
                return restoredJson ?? JsonUtility.ToJson(new LootSpawnerSaveData { version = SaveDataVersion });
            SyncLiveItems();
            return JsonUtility.ToJson(new LootSpawnerSaveData { version = SaveDataVersion, points = new List<LootPointState>(states) });
        }

        /// <summary>Hanya menyimpan JSON. Tidak memunculkan objek dan tidak mengirim event (kontrak ISaveable).</summary>
        public void RestoreState(string json)
        {
            restoredJson = json;
        }

        /// <summary>
        /// Blueprint: kosong (tidak ada event sticky). Jalur cadangan tanpa RegionStreaming: kalau Start menunggu restore,
        /// penyebaran dijalankan di sini, setelah RestoreState pasti sudah dipanggil.
        /// </summary>
        public void OnRestoreFinished()
        {
            if (!waitingForRestore) return;
            waitingForRestore = false;
            SpawnOrRestore();
        }

        // ---------------------------------------------------------------- Editor

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(spawnerId))
            {
                bool ship = table != null && table.location == LootLocation.Ship;
                string prefix = ship ? "ship" : "r" + ((int)homeRegion + 1);
                spawnerId = prefix + "_loot_" + UnityEditor.GUID.Generate().ToString().Substring(0, 8);
            }

            if (spawnPoints == null) return;
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                var def = spawnPoints[i];
                if (def == null) continue;
                if (def.point == null) Debug.LogWarning($"[LootSpawnerSystem] {spawnerId}: titik {i} tanpa Transform.", this);
                bool hasGuaranteed = def.guaranteedItems != null && def.guaranteedItems.Length > 0;
                if (def.guaranteedItems != null && def.guaranteedItems.Length > 1)
                    Debug.LogWarning($"[LootSpawnerSystem] {spawnerId}: titik {i} punya lebih dari satu guaranteedItems. Hanya elemen pertama dipakai; isi lebih banyak memakai LootContainerInteractable (#17).", this);
                if (!hasGuaranteed && table == null)
                    Debug.LogWarning($"[LootSpawnerSystem] {spawnerId}: titik {i} tanpa table dan tanpa guaranteedItems.", this);
                else if (!hasGuaranteed && !table.HasEntryFor(def.kind))
                    Debug.LogWarning($"[LootSpawnerSystem] {spawnerId}: tabel {table.name} tidak punya entri untuk {def.kind} (titik {i}).", this);
            }
            // spawnerId bentrok antar-objek dan titik di bawah batas pasang (#91) diperiksa SaveIdAuditWindow (#80).
        }
#endif
    }
}
