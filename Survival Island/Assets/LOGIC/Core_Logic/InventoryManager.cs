using System;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Isi satu slot tangan di kapal (LifeboatExtraction.md, #74).</summary>
    [Serializable]
    public struct ShipHandEntry
    {
        public string itemId;
        public int amount;
        public bool isNpc;

        public ShipHandEntry(string itemId, int amount, bool isNpc)
        {
            this.itemId = itemId;
            this.amount = amount;
            this.isNpc = isNpc;
        }
    }

    /// <summary>
    /// Pengelola slot inventori dan isi tangan di kapal. Satu kelas untuk dua fase, satu instance per scene (#82).
    /// Blueprint: Core_Logic/InventoryManager.md (#66, #74, #118, #81, #96, #110), rincian slot di ItemUseSystem.md.
    /// GDD bagian 1 dan 3: item inti dan barang dari kapal. Aturan tumpukan, kapasitas tangan, dan pembusukan adalah usulan.
    ///
    /// Save: ISaveable "player_inventory", LoadOrder 50, hanya di IslandPhase. Tangan di kapal tidak disimpan.
    /// </summary>
    public class InventoryManager : MonoBehaviour, ISaveable
    {
        public const string SaveIdConst = "player_inventory";

        public static InventoryManager Instance { get; private set; }

        [SerializeField] private List<InventorySlot> slots = new List<InventorySlot>();
        [SerializeField] private int nextInstanceId = 1;

        private readonly List<ShipHandEntry> shipHandItems = new List<ShipHandEntry>();
        private readonly List<Freshness> freshnessCache = new List<Freshness>();
        private bool registeredForSave;

        /// <summary>Slot untuk dibaca UI. Ubah isi hanya lewat fungsi di kelas ini.</summary>
        public IReadOnlyList<InventorySlot> Slots => slots;
        public int SlotCount => slots.Count;
        public int NextInstanceId => nextInstanceId;

        // ---------------------------------------------------------------- Tangan di kapal (#74)

        public int ShipHandWeight { get; private set; }
        public int ShipHandCapacity => Balance != null ? Balance.ship.handCapacity : 4;
        public List<ShipHandEntry> ShipHandItems => shipHandItems;

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning("[InventoryManager] Instance ganda di scene. Yang baru dimatikan.");
                enabled = false;
                return;
            }
            Instance = this;
            int baseSlots = Balance != null ? Balance.carry.baseSlots : 15;
            if (slots.Count == 0) SetSlotCountRaw(baseSlots);
        }

        private void OnEnable()
        {
            EventChannel<HourChangedEvent>.Raised += HandleHourChanged;
            // Fase 1 tidak punya save (#82). Hanya instance IslandPhase yang mendaftar.
            if (GameManager.Instance != null && GameManager.Instance.currentPhase == GamePhase.IslandSurvival
                && WorldStateSave.Instance != null)
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

        // ---------------------------------------------------------------- Menambah

        /// <summary>Menambah salinan source. Mengembalikan sisa yang tidak muat; pemanggil tetap memegang sisa itu (#66).</summary>
        public int AddItem(ItemInstance source, int amount)
        {
            if (source == null || amount <= 0) return Mathf.Max(0, amount);
            if (source.instanceId <= 0) source.instanceId = AllocateInstanceId();
            int remaining = SlotArrayOps.Add(slots, source, amount, AllocateInstanceId);
            int added = amount - remaining;
            if (added > 0) PublishChanged(source.itemId, added);
            return remaining;
        }

        /// <summary>Membuat ItemInstance baru dari data, lalu memakai AddItem(ItemInstance, int).</summary>
        public int AddItem(ItemData item, int amount)
        {
            if (item == null) return Mathf.Max(0, amount);
            return AddItem(CreateInstance(item), amount);
        }

        /// <summary>ItemInstance baru dengan nilai awal dari data (durabilitas, magazine, jam buat).</summary>
        public ItemInstance CreateInstance(ItemData item)
        {
            var inst = new ItemInstance
            {
                instanceId = AllocateInstanceId(),
                itemId = item.itemID,
                durability = item.maxDurability,
                magazineAmmo = -1,   // GunController (#58) mengisi saat pertama dipegang
                createdGameHour = item.spoilHours > 0f ? ItemUseSystem.CurrentTotalGameHours() : -1f,
            };
            return inst;
        }

        public int AllocateInstanceId() => nextInstanceId++;

        // ---------------------------------------------------------------- Membaca

        public bool HasItem(string itemId, int amount) => GetCount(itemId) >= amount;

        public int GetCount(string itemId) => SlotArrayOps.CountItem(slots, itemId);

        public ItemInstance FindInstance(int instanceId)
        {
            int i = IndexOfInstance(instanceId);
            return i >= 0 ? slots[i].instance : null;
        }

        /// <summary>Item pertama sesuai ID. Dipakai PlayerPinnedState untuk Base_Pisau (usulan #110, disetujui).</summary>
        public ItemInstance FindFirstInstance(string itemId)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                var s = slots[i];
                if (!s.IsEmpty && string.Equals(s.instance.itemId, itemId, StringComparison.Ordinal)) return s.instance;
            }
            return null;
        }

        public int IndexOfInstance(int instanceId)
        {
            for (int i = 0; i < slots.Count; i++)
                if (!slots[i].IsEmpty && slots[i].instance.instanceId == instanceId) return i;
            return -1;
        }

        public InventorySlot GetSlot(int index) => index >= 0 && index < slots.Count ? slots[index] : null;

        public int CountFilledSlots()
        {
            int n = 0;
            for (int i = 0; i < slots.Count; i++) if (!slots[i].IsEmpty) n++;
            return n;
        }

        // ---------------------------------------------------------------- Mengurangi

        /// <summary>Mengurangi dari beberapa slot, mulai dari slot terakhir supaya slot depan tetap rapi.</summary>
        public void RemoveItem(string itemId, int amount)
        {
            if (amount <= 0) return;
            int left = amount;
            for (int i = slots.Count - 1; i >= 0 && left > 0; i--)
            {
                var s = slots[i];
                if (s.IsEmpty || !string.Equals(s.instance.itemId, itemId, StringComparison.Ordinal)) continue;
                int take = Mathf.Min(s.amount, left);
                s.amount -= take;
                left -= take;
                if (s.amount <= 0) s.Clear();
            }
            int removed = amount - left;
            if (removed > 0) PublishChanged(itemId, -removed);
        }

        public void RemoveFromSlot(int slotIndex, int amount)
        {
            var s = GetSlot(slotIndex);
            if (s == null || s.IsEmpty || amount <= 0) return;
            string id = s.instance.itemId;
            int take = Mathf.Min(s.amount, amount);
            s.amount -= take;
            if (s.amount <= 0) s.Clear();
            PublishChanged(id, -take);
        }

        public void RemoveInstance(int instanceId)
        {
            int i = IndexOfInstance(instanceId);
            if (i >= 0) RemoveFromSlot(i, slots[i].amount);
        }

        // ---------------------------------------------------------------- Menata

        /// <summary>Memisahkan tumpukan ke slot kosong. Mengembalikan indeks baru, atau -1.</summary>
        public int SplitStack(int slotIndex, int amount)
        {
            var s = GetSlot(slotIndex);
            if (s == null || s.IsEmpty || amount <= 0 || amount >= s.amount) return -1;
            int empty = FindEmptySlot();
            if (empty < 0) return -1;
            var copy = s.instance.Clone();
            copy.instanceId = AllocateInstanceId();
            slots[empty].instance = copy;
            slots[empty].amount = amount;
            s.amount -= amount;
            PublishChanged(copy.itemId, 0);
            return empty;
        }

        /// <summary>Menukar isi dua slot. Seret dan lepas di InventoryPanel (#66).</summary>
        public void MoveSlot(int from, int to)
        {
            if (from == to || GetSlot(from) == null || GetSlot(to) == null) return;
            var a = slots[from];
            var b = slots[to];
            var inst = a.instance; int amt = a.amount;
            a.instance = b.instance; a.amount = b.amount;
            b.instance = inst; b.amount = amt;
            string id = !b.IsEmpty ? b.instance.itemId : (!a.IsEmpty ? a.instance.itemId : string.Empty);
            PublishChanged(id, 0);
        }

        /// <summary>
        /// Simulasi ruang setelah bahan diambil, tanpa mengubah slot (#77, CraftingManager).
        /// </summary>
        public bool CanFitAfterRemoval(RecipeIngredient[] removed, ItemData result, int amount)
        {
            if (result == null || amount <= 0) return true;
            var sim = new List<InventorySlot>(slots.Count);
            for (int i = 0; i < slots.Count; i++)
            {
                var s = slots[i];
                sim.Add(new InventorySlot { instance = s.IsEmpty ? null : s.instance.Clone(), amount = s.IsEmpty ? 0 : s.amount });
            }
            if (removed != null)
            {
                for (int r = 0; r < removed.Length; r++)
                {
                    if (removed[r].item == null) continue;
                    int left = removed[r].amount;
                    for (int i = sim.Count - 1; i >= 0 && left > 0; i--)
                    {
                        var s = sim[i];
                        if (s.IsEmpty || s.instance.itemId != removed[r].item.itemID) continue;
                        int take = Mathf.Min(s.amount, left);
                        s.amount -= take;
                        left -= take;
                        if (s.amount <= 0) s.Clear();
                    }
                }
            }
            var probe = new ItemInstance { itemId = result.itemID, durability = result.maxDurability };
            return SlotArrayOps.Add(sim, probe, amount) == 0;
        }

        /// <summary>
        /// Jumlah slot. Naik: slot kosong ditambah di akhir. Turun: isi slot di atas batas dipindah ke slot kosong,
        /// sisanya jatuh sebagai WorldItem di depan pemain (CarryCapacity.md).
        /// </summary>
        public void SetMaxSlots(int value) => SetMaxSlots(value, true);

        /// <summary>publish = false dipakai CarryCapacity.RestoreState, karena RestoreState tidak boleh mengirim event (#80).</summary>
        public void SetMaxSlots(int value, bool publish)
        {
            value = Mathf.Max(1, value);
            if (value == slots.Count) return;
            if (value > slots.Count)
            {
                SetSlotCountRaw(value);
                if (publish) PublishChanged(string.Empty, 0);
                return;
            }

            bool dropped = false;
            for (int i = slots.Count - 1; i >= value; i--)
            {
                var s = slots[i];
                if (s.IsEmpty) continue;
                int target = -1;
                for (int j = 0; j < value; j++) if (slots[j].IsEmpty) { target = j; break; }
                if (target >= 0)
                {
                    slots[target].instance = s.instance;
                    slots[target].amount = s.amount;
                }
                else
                {
                    DropAsWorldItem(s.instance, s.amount);
                    dropped = true;
                }
                s.Clear();
            }
            slots.RemoveRange(value, slots.Count - value);
            freshnessCache.Clear();
            if (dropped)
            {
                // TODO #22 (NotificationManager): notifikasi "Beberapa barang terjatuh".
                Debug.Log("[InventoryManager] Beberapa barang terjatuh karena kapasitas turun.");
            }
            if (publish) PublishChanged(string.Empty, 0);
        }

        // ---------------------------------------------------------------- Tangan di kapal (#74)

        /// <summary>
        /// Menambah barang ke tangan kapal. Ditolak kalau melebihi ShipHandCapacity (ShipInteractable.CanInteract sudah
        /// menolak lebih dulu). Usulan InventoryManager.md: ID berawalan "npc_" adalah NPC, tidak digabung, amount 1.
        /// </summary>
        public bool AddShipHandItem(string id, int weight, int amount)
        {
            if (string.IsNullOrEmpty(id) || weight < 0 || amount <= 0) return false;
            if (ShipHandWeight + weight > ShipHandCapacity) return false;

            bool isNpc = id.StartsWith("npc_", StringComparison.Ordinal);
            bool merged = false;
            if (!isNpc)
            {
                for (int i = 0; i < shipHandItems.Count; i++)
                {
                    var e = shipHandItems[i];
                    if (e.isNpc || !string.Equals(e.itemId, id, StringComparison.Ordinal)) continue;
                    e.amount += amount;
                    shipHandItems[i] = e;
                    merged = true;
                    break;
                }
            }
            if (!merged) shipHandItems.Add(new ShipHandEntry(id, isNpc ? 1 : amount, isNpc));

            ShipHandWeight += weight;
            PublishShipHand();
            return true;
        }

        /// <summary>Mengosongkan seluruh tangan sekaligus (LifeboatExtraction.DepositHand).</summary>
        public void ClearShipHand()
        {
            shipHandItems.Clear();
            ShipHandWeight = 0;
            PublishShipHand();
        }

        private void PublishShipHand()
        {
            EventChannel<ShipHandChangedEvent>.Publish(new ShipHandChangedEvent(ShipHandWeight, ShipHandCapacity));
        }

        // ---------------------------------------------------------------- Pembusukan (#66)

        /// <summary>
        /// ItemUseSystem.md "Pembusukan": dicek tiap jam game, bukan tiap frame. Event dikirim hanya kalau kesegaran slot berubah,
        /// supaya ikon di panel ikut berubah.
        /// </summary>
        private void HandleHourChanged(HourChangedEvent e)
        {
            while (freshnessCache.Count < slots.Count) freshnessCache.Add(Freshness.Fresh);
            for (int i = 0; i < slots.Count; i++)
            {
                var s = slots[i];
                if (s.IsEmpty) { freshnessCache[i] = Freshness.Fresh; continue; }
                var data = ItemDatabase.Get(s.instance.itemId);
                if (data == null || data.spoilHours <= 0f) continue;
                var f = ItemUseSystem.GetFreshness(s.instance, data);
                if (f == freshnessCache[i]) continue;
                freshnessCache[i] = f;
                PublishChanged(s.instance.itemId, 0);
            }
        }

        // ---------------------------------------------------------------- Bantuan

        private int FindEmptySlot()
        {
            for (int i = 0; i < slots.Count; i++) if (slots[i].IsEmpty) return i;
            return -1;
        }

        private void SetSlotCountRaw(int count)
        {
            while (slots.Count < count) slots.Add(new InventorySlot());
            if (slots.Count > count) slots.RemoveRange(count, slots.Count - count);
        }

        private void DropAsWorldItem(ItemInstance inst, int amount)
        {
            if (inst == null || amount <= 0) return;
            var manager = WorldItemManager.Instance;
            Vector3 pos = transform.position + transform.forward * (Balance != null ? Balance.items.dropForwardDistance : 1f) + Vector3.up;
            if (manager != null) manager.Spawn(inst, amount, pos, true);
            else Debug.LogWarning($"[InventoryManager] WorldItemManager tidak ada. {amount}x {inst.itemId} hilang.");
        }

        private void PublishChanged(string itemId, int delta)
        {
            int total = string.IsNullOrEmpty(itemId) ? 0 : GetCount(itemId);
            EventChannel<InventoryChangedEvent>.Publish(new InventoryChangedEvent(itemId ?? string.Empty, delta, total));
        }

        // ---------------------------------------------------------------- Save (#80)

        [Serializable]
        private struct SlotRecord
        {
            public int index;
            public ItemInstance instance;
            public int amount;
        }

        [Serializable]
        private class InventorySaveData
        {
            public SlotRecord[] slots;
            public int nextInstanceId = 1;
        }

        public string SaveId => SaveIdConst;
        public int LoadOrder => 50;

        public string CaptureState()
        {
            var records = new List<SlotRecord>();
            for (int i = 0; i < slots.Count; i++)
                if (!slots[i].IsEmpty) records.Add(new SlotRecord { index = i, instance = slots[i].instance, amount = slots[i].amount });
            return JsonUtility.ToJson(new InventorySaveData { slots = records.ToArray(), nextInstanceId = nextInstanceId });
        }

        /// <summary>
        /// CarryCapacity (LoadOrder 40) sudah memasang jumlah slot. itemId yang tidak ada membuat slot kosong dengan
        /// peringatan (#80). Tumpukan di atas maxStack dipecah (#118). Tidak mengirim event.
        /// </summary>
        public void RestoreState(string json)
        {
            var data = JsonUtility.FromJson<InventorySaveData>(json);
            if (data == null) return;
            for (int i = 0; i < slots.Count; i++) slots[i].Clear();
            nextInstanceId = Mathf.Max(1, data.nextInstanceId);
            if (data.slots == null) return;

            var leftovers = new List<SlotRecord>();
            foreach (var r in data.slots)
            {
                if (r.instance == null || r.amount <= 0) continue;
                var item = ItemDatabase.Get(r.instance.itemId);
                if (item == null)
                {
                    Debug.LogWarning($"[InventoryManager] itemId '{r.instance.itemId}' tidak ditemukan saat load. Slot dikosongkan.");
                    continue;
                }
                if (r.index >= 0 && r.index < slots.Count && slots[r.index].IsEmpty && r.amount <= item.EffectiveMaxStack)
                {
                    slots[r.index].instance = r.instance;
                    slots[r.index].amount = r.amount;
                }
                else leftovers.Add(r);
            }
            foreach (var r in leftovers)
            {
                int rest = SlotArrayOps.Add(slots, r.instance, r.amount, AllocateInstanceId);
                if (rest > 0) DropAsWorldItem(r.instance, rest);
            }
        }

        public void OnRestoreFinished()
        {
            freshnessCache.Clear();
            PublishChanged(string.Empty, 0);
        }
    }
}
