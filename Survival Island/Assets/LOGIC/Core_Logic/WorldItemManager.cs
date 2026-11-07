using System;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Pemilik semua WorldItem di IslandPhase. Blueprint: Core_Logic/ItemUseSystem.md bagian "WorldItem" (#66, #88).
    /// Aturan penghapusan: item yang dibuang hilang setelah worldItemDespawnGameHours, dicek tiap HourChangedEvent,
    /// dengan batas maxLooseWorldItems (usulan disetujui #85). Loot dan barang sisa kapal tidak pernah hilang otomatis.
    ///
    /// Simpan per region (#88, RegionStreaming.md "Menyimpan status objek di region yang tidak dimuat"):
    /// - CaptureRegion/RestoreRegion dipanggil RegionStreaming. Data satu region ditulis sebagai satu entri RegionStateStore
    ///   dengan kunci StoreKey. Item milik LootSpawnerSystem (sourceSpawner terisi) dilewati: statusnya dicatat spawner.
    /// - Jalur cadangan tanpa RegionStreaming (scene uji): kelas ini ISaveable "world_items" ke WorldStateSave, berisi
    ///   semua item lepas. Dengan RegionStreaming, entri itu kosong karena datanya di RegionStateStore.
    /// - SweepBelow dipanggil TideManager (#91).
    /// </summary>
    public class WorldItemManager : MonoBehaviour, ISaveable
    {
        public static WorldItemManager Instance { get; private set; }

        /// <summary>Kunci entri WorldItem lepas di RegionStateStore, satu per region. Tidak bentrok dengan format SaveIdTag.</summary>
        public const string StoreKey = "world_items";
        public const string SaveIdConst = "world_items";

        private readonly List<WorldItem> active = new List<WorldItem>(128);
        private readonly List<WorldItem> loose = new List<WorldItem>(64);   // Urut dari yang tertua
        private readonly List<WorldItem> buffer = new List<WorldItem>(64);
        private bool registeredForSave;
        private string fallbackJson;

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;

        public IReadOnlyList<WorldItem> Active => active;

        [Serializable]
        private class WorldItemRecord
        {
            public ItemInstance instance;
            public int amount;
            public Vector3 position;
            public Quaternion rotation = Quaternion.identity;
            public bool despawns;
            public float despawnAtGameHour = -1f;
        }

        [Serializable]
        private class WorldItemList
        {
            public int version = 1;
            public List<WorldItemRecord> items = new List<WorldItemRecord>();
        }

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            EventChannel<HourChangedEvent>.Raised += HandleHourChanged;
            // Manager di IslandPhase: selalu mendaftar. CaptureState memutuskan isi saat save, karena RegionStreaming
            // di scene yang sama mungkin belum menjalankan Awake pada titik ini.
            if (WorldStateSave.Instance != null)
            {
                WorldStateSave.Instance.Register(this);
                registeredForSave = true;
            }
        }

        private void OnDisable()
        {
            EventChannel<HourChangedEvent>.Raised -= HandleHourChanged;
            if (registeredForSave && WorldStateSave.Instance != null) WorldStateSave.Instance.Unregister(this);
            registeredForSave = false;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        /// <summary>Mengambil dari pool, memasang mesh, lalu mendaftarkan. despawns = true untuk item dibuang atau jatuh dari musuh.</summary>
        public WorldItem Spawn(ItemInstance inst, int amount, Vector3 pos, bool despawns)
        {
            if (inst == null || amount <= 0 || ObjectPooler.Instance == null) return null;
            var item = ObjectPooler.Instance.SpawnFromPool<WorldItem>(WorldItem.PoolTag, pos, Quaternion.identity);
            if (item == null) return null;

            var data = ItemDatabase.Get(inst.itemId);
            // Item yang tidak bisa dibuang tidak pernah hilang otomatis (ItemUseSystem.md).
            bool canDespawn = despawns && (data == null || data.isDroppable);
            float hours = Balance != null ? Balance.items.worldItemDespawnGameHours : 48f;
            item.Setup(inst, amount, canDespawn, ItemUseSystem.CurrentTotalGameHours() + hours);
            // ItemUseSystem.md Kasus Khusus: homeRegion dari RegionStreaming.GetRegionAt.
            item.homeRegion = RegionStreaming.RegionOf(pos);
            active.Add(item);
            if (canDespawn)
            {
                loose.Add(item);
                EnforceLooseLimit();
            }
            return item;
        }

        /// <summary>
        /// Seperti Spawn, tetapi kalau region di posisi itu tidak dimuat (misalnya peti hancur lewat #90 di base yang ditinggal),
        /// data item ditulis ke RegionStateStore dan muncul saat region kembali (StorageChest.md Kasus Khusus).
        /// </summary>
        public void SpawnOrStore(ItemInstance inst, int amount, Vector3 pos, bool despawns)
        {
            if (inst == null || amount <= 0) return;
            var rs = RegionStreaming.Instance;
            if (rs != null)
            {
                RegionType region = rs.GetRegionAt(pos);
                if (!rs.IsLoaded(region))
                {
                    var data = ItemDatabase.Get(inst.itemId);
                    bool canDespawn = despawns && (data == null || data.isDroppable);
                    float hours = Balance != null ? Balance.items.worldItemDespawnGameHours : 48f;
                    var list = ReadList(rs.Store, region) ?? new WorldItemList();
                    list.items.Add(new WorldItemRecord
                    {
                        instance = inst,
                        amount = amount,
                        position = pos,
                        despawns = canDespawn,
                        despawnAtGameHour = canDespawn ? ItemUseSystem.CurrentTotalGameHours() + hours : -1f
                    });
                    rs.Store.Write(region, StoreKey, JsonUtility.ToJson(list));
                    return;
                }
            }
            Spawn(inst, amount, pos, despawns);
        }

        /// <summary>Melepas daftar dan mengembalikan ke pool.</summary>
        public void Despawn(WorldItem item)
        {
            if (item == null) return;
            active.Remove(item);
            loose.Remove(item);
            if (ObjectPooler.Instance != null) ObjectPooler.Instance.ReturnToPool(WorldItem.PoolTag, item.gameObject);
        }

        private void EnforceLooseLimit()
        {
            int max = Balance != null ? Balance.items.maxLooseWorldItems : 64;
            while (loose.Count > max) Despawn(loose[0]);
        }

        private void HandleHourChanged(HourChangedEvent e)
        {
            float now = ItemUseSystem.CurrentTotalGameHours();
            for (int i = active.Count - 1; i >= 0; i--)
            {
                var item = active[i];
                if (item == null || !item.gameObject.activeInHierarchy)
                {
                    active.RemoveAt(i);
                    continue;
                }
                if (item.Despawns && item.despawnAtGameHour >= 0f && now >= item.despawnAtGameHour) Despawn(item);
            }
        }

        // ---------------------------------------------------------------- Simpan per region (#88)

        /// <summary>
        /// Catat itemId, jumlah, dan posisi WorldItem lepas di region ini ke Store. releasing = true saat region Unloading:
        /// objeknya dikembalikan ke pool (ItemUseSystem.md). false saat save: objek tetap di dunia.
        /// Region item dihitung dari posisi saat ini, karena item bisa terdorong melewati batas.
        /// </summary>
        public void CaptureRegion(RegionType region, RegionStateStore store, bool releasing)
        {
            if (store == null) return;
            var rs = RegionStreaming.Instance;
            var list = new WorldItemList();
            buffer.Clear();
            for (int i = active.Count - 1; i >= 0; i--)
            {
                var item = active[i];
                if (item == null || !item.gameObject.activeInHierarchy || item.Instance == null || item.Amount <= 0)
                {
                    active.RemoveAt(i);
                    continue;
                }
                // Loot spawner dicatat spawner sendiri (LootSpawnerSystem.md "Region dilepas").
                if (item.sourceSpawner != null) continue;
                Vector3 pos = item.transform.position;
                item.homeRegion = rs != null ? rs.GetRegionAt(pos) : RegionStreaming.RegionOf(pos);
                if (item.homeRegion != region) continue;
                list.items.Add(ToRecord(item));
                if (releasing) buffer.Add(item);
            }
            // Selalu ditulis (juga kosong), supaya data lama region ini tidak muncul lagi.
            store.Write(region, StoreKey, JsonUtility.ToJson(list));
            for (int i = 0; i < buffer.Count; i++) Despawn(buffer[i]);
            buffer.Clear();
        }

        /// <summary>Munculkan kembali WorldItem region ini dari Store, lalu hapus entrinya supaya tidak muncul dua kali.</summary>
        public void RestoreRegion(RegionType region, RegionStateStore store)
        {
            if (store == null) return;
            var list = ReadList(store, region);
            store.Remove(region, StoreKey);
            if (list != null) SpawnRecords(list);
        }

        private WorldItemList ReadList(RegionStateStore store, RegionType region)
        {
            if (!store.TryRead(region, StoreKey, out var json)) return null;
            return ParseList(json);
        }

        private static WorldItemList ParseList(string json)
        {
            if (string.IsNullOrEmpty(json)) return null;
            try
            {
                var list = JsonUtility.FromJson<WorldItemList>(json);
                if (list != null && list.items == null) list.items = new List<WorldItemRecord>();
                return list;
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[WorldItemManager] Data WorldItem tidak terbaca: {ex.Message}");
                return null;
            }
        }

        private static WorldItemRecord ToRecord(WorldItem item)
        {
            return new WorldItemRecord
            {
                instance = item.Instance,
                amount = item.Amount,
                position = item.transform.position,
                rotation = item.transform.rotation,
                despawns = item.Despawns,
                despawnAtGameHour = item.despawnAtGameHour
            };
        }

        private void SpawnRecords(WorldItemList list)
        {
            for (int i = 0; i < list.items.Count; i++)
            {
                var rec = list.items[i];
                if (rec == null || rec.instance == null || rec.amount <= 0) continue;
                // ItemData dihapus di versi baru: item dibuang dengan peringatan (ItemUseSystem.md Kasus Khusus).
                if (ItemDatabase.Get(rec.instance.itemId) == null)
                {
                    Debug.LogWarning($"[WorldItemManager] Item {rec.instance.itemId} tidak ada di ItemDatabase. Dibuang.");
                    continue;
                }
                var item = Spawn(rec.instance, rec.amount, rec.position, rec.despawns);
                if (item == null) continue;   // Pool habis
                item.PlaceAt(rec.position, rec.rotation);
                // Waktu hilang memakai jam absolut tersimpan, bukan dihitung ulang dari sekarang.
                if (item.Despawns) item.despawnAtGameHour = rec.despawnAtGameHour;
            }
        }

        // ---------------------------------------------------------------- Air pasang (#91)

        /// <summary>
        /// TideManager.md "Barang di pantai": item biasa di bawah waterY mengikuti aturan air (tenggelam, hilang 1 jam game).
        /// Item yang tidak hilang otomatis (despawns false, KeyItem, item inti) pindah ke TideSafePoint terdekat, dengan
        /// notifikasi sekali per pemanggilan.
        /// </summary>
        public void SweepBelow(Bounds zone, float waterY, TideSafePoint[] safe)
        {
            bool moved = false;
            for (int i = active.Count - 1; i >= 0; i--)
            {
                var item = active[i];
                if (item == null || !item.gameObject.activeInHierarchy || item.Instance == null) continue;
                Vector3 p = item.transform.position;
                if (p.y >= waterY) continue;
                if (p.x < zone.min.x || p.x > zone.max.x || p.z < zone.min.z || p.z > zone.max.z) continue;

                if (item.Despawns)
                {
                    item.EnterWater();
                    continue;
                }
                var point = TideSafePoint.Nearest(safe, p);
                if (point == null) continue;   // Tanpa titik aman: dibiarkan (TideZoneAuditWindow melaporkan)
                item.PlaceAt(point.Position + Vector3.up * 0.3f, item.transform.rotation);
                moved = true;
            }
            // "Barang terbawa ke atas pantai" (#22).
            if (moved && NotificationManager.Instance != null) NotificationManager.Instance.Show("notif.tide_items_moved", NotificationKind.Info);
        }

        // ---------------------------------------------------------------- ISaveable (jalur cadangan tanpa RegionStreaming)

        public string SaveId => SaveIdConst;

        /// <summary>Tidak ada di tabel LoadOrder; dipakai 0 seperti objek region. Item dimunculkan di OnRestoreFinished.</summary>
        public int LoadOrder => 0;

        public string CaptureState()
        {
            var list = new WorldItemList();
            // Dengan RegionStreaming, data WorldItem ada di RegionStateStore (CaptureLoadedRegions), jadi entri ini kosong.
            if (RegionStreaming.Instance != null) return JsonUtility.ToJson(list);
            for (int i = 0; i < active.Count; i++)
            {
                var item = active[i];
                if (item == null || !item.gameObject.activeInHierarchy || item.Instance == null || item.Amount <= 0) continue;
                if (item.sourceSpawner != null) continue;
                list.items.Add(ToRecord(item));
            }
            return JsonUtility.ToJson(list);
        }

        /// <summary>Hanya menyimpan JSON (kontrak ISaveable).</summary>
        public void RestoreState(string json)
        {
            fallbackJson = json;
        }

        public void OnRestoreFinished()
        {
            string json = fallbackJson;
            fallbackJson = null;
            if (RegionStreaming.Instance != null) return;
            var list = ParseList(json);
            if (list != null) SpawnRecords(list);
        }
    }
}
