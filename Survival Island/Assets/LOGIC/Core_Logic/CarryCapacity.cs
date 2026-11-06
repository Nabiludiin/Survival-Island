using System;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Simpanan tas yang dipakai (CarryCapacity.md).</summary>
    [Serializable]
    public struct CarryCapacitySaveData
    {
        public ItemInstance equippedBag;   // null kalau tanpa tas
    }

    /// <summary>
    /// Jumlah slot inventori, tingkatan Backpack, dan konversi barang kapal ke inventori pulau.
    /// Blueprint: Core_Logic/CarryCapacity.md (#118). Komponen di pemain Fase 2, bersama InventoryManager.
    /// GDD bagian 3: barang dari Fase 1 dibawa ke Fase 2. Tingkatan Backpack adalah usulan yang disetujui developer.
    /// </summary>
    [RequireComponent(typeof(InventoryManager))]
    public class CarryCapacity : MonoBehaviour, ISaveable
    {
        public const string SaveIdConst = "player_carry";

        public static CarryCapacity Instance { get; private set; }

        private InventoryManager inventory;
        private ItemInstance equippedBag;
        private bool registeredForSave;

        public int CurrentCapacity { get; private set; } = 15;
        public ItemInstance EquippedBag => equippedBag;

        /// <summary>Sisa dari ApplyShipLoot terakhir (sama dengan nilai kembaliannya), untuk IslandArrival.</summary>
        public List<SavedItemData> LastOverflow { get; private set; } = new List<SavedItemData>();

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;
        private int BaseSlots => Balance != null ? Balance.carry.baseSlots : 15;

        private void Awake()
        {
            Instance = this;
            inventory = GetComponent<InventoryManager>();
        }

        private void Start()
        {
            RecalculateCapacity();
        }

        private void OnEnable()
        {
            if (WorldStateSave.Instance != null)
            {
                WorldStateSave.Instance.Register(this);
                registeredForSave = true;
            }
        }

        private void OnDisable()
        {
            if (registeredForSave && WorldStateSave.Instance != null) WorldStateSave.Instance.Unregister(this);
            registeredForSave = false;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        // ---------------------------------------------------------------- Kapal ke pulau

        /// <summary>
        /// Dipanggil IslandArrival.Apply (#74) SETELAH item inti dan vaksin awal masuk. Tidak menambah item inti.
        /// entry.amount sudah berisi shipPickupAmount, jadi tidak dikali lagi. Mengembalikan daftar sisa (overflow) yang
        /// dimunculkan IslandArrival di pantai sebagai WorldItem dengan despawns: false.
        /// </summary>
        public List<SavedItemData> ApplyShipLoot(List<SavedItemData> loot)
        {
            var overflow = new List<SavedItemData>();
            LastOverflow = overflow;
            if (loot == null) return overflow;
            for (int i = 0; i < loot.Count; i++)
            {
                var entry = loot[i];
                // NPC tidak masuk inventori (#76); dicatat lewat rescuedNpcIds oleh LifeboatExtraction.
                if (string.IsNullOrEmpty(entry.itemId) || entry.itemId.StartsWith("npc_", StringComparison.Ordinal)) continue;
                var data = ItemDatabase.Get(entry.itemId);
                if (data == null)
                {
                    Debug.LogWarning($"[CarryCapacity] Barang kapal '{entry.itemId}' tidak dikenal. Dilewati.");
                    continue;
                }
                int rest = inventory.AddItem(data, entry.amount);
                if (rest > 0) overflow.Add(new SavedItemData { itemId = entry.itemId, amount = rest });
            }
            return overflow;
        }

        // ---------------------------------------------------------------- Tas

        /// <summary>
        /// Memakai tas dari inventori, atau melepas dengan bag = null. Ditolak kalau isi tidak muat di kapasitas baru
        /// (usulan disetujui: melepas tas ditolak kalau slot tambahan masih terisi).
        /// </summary>
        public bool SetBag(ItemInstance bag)
        {
            int newBonus = bag != null ? BagBonus(bag) : 0;
            if (bag != null && newBonus <= 0) return false;

            int newCapacity = BaseSlots + newBonus;
            int filled = inventory.CountFilledSlots();
            // Tas baru keluar dari slot, tas lama masuk ke slot.
            if (bag != null && inventory.IndexOfInstance(bag.instanceId) >= 0) filled -= 1;
            if (equippedBag != null) filled += 1;
            if (filled > newCapacity)
            {
                // TODO #22: pesan "Kosongkan tas dulu".
                Debug.Log("[CarryCapacity] Kosongkan tas dulu.");
                return false;
            }

            var oldBag = equippedBag;
            if (bag != null) inventory.RemoveInstance(bag.instanceId);
            equippedBag = bag;
            // Kapasitas dinaikkan dulu supaya tas lama pasti muat.
            RecalculateCapacity();
            if (oldBag != null && inventory.AddItem(oldBag, 1) > 0)
            {
                // Seharusnya tidak terjadi karena hitungan di atas. Kembalikan keadaan.
                equippedBag = oldBag;
                if (bag != null) inventory.AddItem(bag, 1);
                RecalculateCapacity();
                return false;
            }
            return true;
        }

        /// <summary>Tas lepas karena sistem lain (DevConsole, load). Kapasitas turun, sisa jatuh sebagai WorldItem.</summary>
        public void ForceRemoveBag()
        {
            equippedBag = null;
            RecalculateCapacity();
        }

        private static int BagBonus(ItemInstance bag)
        {
            var data = bag != null ? ItemDatabase.Get(bag.itemId) : null;
            return data != null ? Mathf.Max(0, data.bagBonusSlots) : 0;
        }

        private void RecalculateCapacity()
        {
            int newCapacity = BaseSlots + BagBonus(equippedBag);
            inventory.SetMaxSlots(newCapacity);
            CurrentCapacity = newCapacity;
            EventChannel<InventoryCapacityChangedEvent>.Publish(new InventoryCapacityChangedEvent(newCapacity));
        }

        // ---------------------------------------------------------------- Save (#80), sebelum InventoryManager

        public string SaveId => SaveIdConst;
        public int LoadOrder => 40;

        public string CaptureState()
        {
            return JsonUtility.ToJson(new CarryCapacitySaveData { equippedBag = equippedBag });
        }

        public void RestoreState(string json)
        {
            var data = JsonUtility.FromJson<CarryCapacitySaveData>(json);
            var bag = data.equippedBag;
            // JsonUtility membuat objek kosong untuk null; itemId kosong berarti tanpa tas.
            if (bag != null && string.IsNullOrEmpty(bag.itemId)) bag = null;
            if (bag != null && ItemDatabase.Get(bag.itemId) == null)
            {
                Debug.LogWarning($"[CarryCapacity] Tas '{bag.itemId}' tidak ditemukan saat load. Kapasitas turun (#80).");
                bag = null;
            }
            equippedBag = bag;
            // Tanpa event di RestoreState; event dikirim di OnRestoreFinished.
            int newCapacity = BaseSlots + BagBonus(equippedBag);
            inventory.SetMaxSlots(newCapacity, false);
            CurrentCapacity = newCapacity;
        }

        public void OnRestoreFinished()
        {
            EventChannel<InventoryCapacityChangedEvent>.Publish(new InventoryCapacityChangedEvent(CurrentCapacity));
        }
    }
}
