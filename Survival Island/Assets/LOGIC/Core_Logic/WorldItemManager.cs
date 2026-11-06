using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Pemilik semua WorldItem di IslandPhase. Blueprint: Core_Logic/ItemUseSystem.md bagian "WorldItem" (#66, #88).
    /// Aturan penghapusan: item yang dibuang hilang setelah worldItemDespawnGameHours, dicek tiap HourChangedEvent,
    /// dengan batas maxLooseWorldItems (usulan disetujui #85). Loot dan barang sisa kapal tidak pernah hilang otomatis.
    ///
    /// Belum diimplementasikan di Langkah 3 (TODO #88 / #91): CaptureRegion, RestoreRegion (butuh RegionType dan
    /// RegionStateStore milik RegionStreaming), dan SweepBelow (butuh TideSafePoint milik TideManager).
    /// </summary>
    public class WorldItemManager : MonoBehaviour
    {
        public static WorldItemManager Instance { get; private set; }

        private readonly List<WorldItem> active = new List<WorldItem>(128);
        private readonly List<WorldItem> loose = new List<WorldItem>(64);   // Urut dari yang tertua

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;

        public IReadOnlyList<WorldItem> Active => active;

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            EventChannel<HourChangedEvent>.Raised += HandleHourChanged;
        }

        private void OnDisable()
        {
            EventChannel<HourChangedEvent>.Raised -= HandleHourChanged;
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
            active.Add(item);
            if (canDespawn)
            {
                loose.Add(item);
                EnforceLooseLimit();
            }
            return item;
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
    }
}
