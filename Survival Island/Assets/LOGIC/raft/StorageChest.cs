using System;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Satu slot berisi di save peti. Slot kosong tidak ditulis karena JsonUtility tidak mempertahankan null.</summary>
    [Serializable]
    public struct ChestSlotRecord
    {
        public int slotIndex;
        public int amount;
        public ItemInstance instance;
    }

    [Serializable]
    public class StorageChestSaveData
    {
        public int slotCount;
        public ChestSlotRecord[] items;
    }

    /// <summary>
    /// Peti penyimpanan buatan pemain (prefab Chest_Wood). Isi terpisah dari inventori, memakai InventorySlot dan ItemInstance
    /// yang sama. Blueprint: raft/StorageChest.md (#46). GDD bagian 4 (fungsi base); jumlah slot dan aturannya adalah usulan
    /// yang disetujui developer (dicatat di #85).
    ///
    /// Catatan implementasi (sistem pendukung):
    /// - IslandInteractable (#17) belum ada, jadi kelas ini menerapkan IInteractable langsung.
    /// - SpillContents dipanggil dari StructureDestroyedEvent milik peti ini. BaseHealth.Remove (bongkar) dan kematian
    ///   (TakeDamage) sama-sama menerbitkan event itu, jadi satu jalur menutup keduanya.
    /// - Isi yang tumpah di region yang tidak dimuat ditulis ke RegionStateStore lewat WorldItemManager.SpawnOrStore (#88).
    ///   Peti nonaktif di region yang dilepas (#90) tetap mendengar StructureDestroyedEvent, karena pendaftaran di Awake/OnDestroy.
    ///   Peti yang dipulihkan langsung ke structureRoot nonaktif (Awake belum berjalan) belum mendengar sampai region dimuat.
    /// </summary>
    public class StorageChest : MonoBehaviour, IInteractable, ISaveable
    {
        private static readonly int OpenHash = Animator.StringToHash("Open");

        [SerializeField] private Animator lidAnimator;
        [SerializeField] private InteractHighlight highlight;

        private InventorySlot[] slots;
        private bool spilled;

        public IReadOnlyList<InventorySlot> Slots => slots;
        public bool IsOpen { get; private set; }

        /// <summary>Event per objek untuk panel yang sedang terbuka.</summary>
        public event Action ContentsChanged;

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;
        private static int SlotCountConfig => Balance != null ? Mathf.Max(1, Balance.storage.chestSlotCount) : 20;

        private string StructureId
        {
            get
            {
                var h = GetComponent<IStructureHealth>();
                return h != null ? h.StructureId : string.Empty;
            }
        }

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            if (slots == null) slots = NewSlots(SlotCountConfig);
            // Didaftarkan di Awake dan dilepas di OnDestroy (bukan OnEnable/OnDisable), supaya peti di region yang dilepas
            // (structureRoot nonaktif) tetap menumpahkan isinya saat hancur oleh RemoteBaseSimulation (#90, StorageChest.md).
            EventChannel<StructureDestroyedEvent>.Raised += HandleStructureDestroyed;
        }

        private void OnDestroy()
        {
            EventChannel<StructureDestroyedEvent>.Raised -= HandleStructureDestroyed;
        }

        private void OnDisable()
        {
            SetHourListening(false);
        }

        private static InventorySlot[] NewSlots(int count)
        {
            var a = new InventorySlot[count];
            for (int i = 0; i < count; i++) a[i] = new InventorySlot();
            return a;
        }

        // ---------------------------------------------------------------- Isi (dipanggil StoragePanel)

        public InventorySlot GetSlot(int index) => slots != null && index >= 0 && index < slots.Length ? slots[index] : null;

        /// <summary>Menambah salinan source lewat aturan tumpukan bersama. Mengembalikan sisa.</summary>
        public int Add(ItemInstance source, int amount)
        {
            var inv = InventoryManager.Instance;
            Func<int> alloc = inv != null ? (Func<int>)inv.AllocateInstanceId : null;
            int left = SlotArrayOps.Add(slots, source, amount, alloc);
            if (left != amount) RaiseChanged();
            return left;
        }

        public void RemoveFromSlot(int index, int amount)
        {
            var s = GetSlot(index);
            if (s == null || s.IsEmpty || amount <= 0) return;
            s.amount -= amount;
            if (s.amount <= 0) s.Clear();
            RaiseChanged();
        }

        /// <summary>Menukar isi dua slot peti.</summary>
        public void SwapSlots(int a, int b)
        {
            var sa = GetSlot(a);
            var sb = GetSlot(b);
            if (sa == null || sb == null || a == b) return;
            var inst = sa.instance; int amt = sa.amount;
            sa.instance = sb.instance; sa.amount = sb.amount;
            sb.instance = inst; sb.amount = amt;
            RaiseChanged();
        }

        /// <summary>Mengganti isi satu slot (dipakai tukar antar-sisi). instance null mengosongkan.</summary>
        public void SetSlot(int index, ItemInstance instance, int amount)
        {
            var s = GetSlot(index);
            if (s == null) return;
            if (instance == null || amount <= 0) s.Clear();
            else { s.instance = instance; s.amount = amount; }
            RaiseChanged();
        }

        public int CountItem(string itemId) => SlotArrayOps.CountItem(slots, itemId);

        /// <summary>Usulan disetujui #46: item inti dan item wajib (isDroppable false) tidak boleh masuk peti.</summary>
        public static bool CanStore(ItemData item)
        {
            if (item == null) return false;
            if (item.isCoreItem) return false;      // GLOSSARY: item inti selalu kembali ke slot
            if (!item.isDroppable) return false;    // #66, #116: item wajib tetap terpantau
            return true;
        }

        private void RaiseChanged() => ContentsChanged?.Invoke();

        // ---------------------------------------------------------------- Buka dan tutup

        public bool IsAvailable => isActiveAndEnabled && !spilled;
        public float HoldSeconds => 0f;
        public string GetPrompt() { var loc = LocalizationManager.Instance; return loc != null && loc.Has("prompt.storage_open") ? loc.Get("prompt.storage_open") : "Buka peti"; }   // #104

        public InteractBlockReason CanInteract(in InteractContext ctx)
        {
            // blockedWhileCarrying = true: tidak bisa dibuka saat membawa Heavy Item ("Tangan penuh").
            return ctx.IsCarryingHeavyItem ? InteractBlockReason.CarryingHeavyItem : InteractBlockReason.None;
        }

        public void OnHoverEnter() { if (highlight != null) highlight.SetState(HighlightState.Available); }
        public void OnHoverExit() { if (highlight != null) highlight.SetState(HighlightState.Off); }

        public bool Interact(in InteractContext ctx)
        {
            var panel = StoragePanel.Instance;
            var stack = UIFocusStack.Instance;
            if (panel == null || stack == null) return false;
            // 1-2.
            panel.Bind(this);
            if (!stack.Push(panel))
            {
                panel.Bind(null);
                return false;
            }
            // 3.
            SetOpen(true);
            return true;
        }

        /// <summary>Dipanggil StoragePanel saat panel tertutup.</summary>
        public void SetOpen(bool open)
        {
            IsOpen = open;
            if (lidAnimator != null) lidAnimator.SetBool(OpenHash, open);
            // Pembusukan tampak di panel: HourChangedEvent didengar hanya saat terbuka.
            SetHourListening(open);
        }

        private bool hourListening;

        private void SetHourListening(bool on)
        {
            if (on == hourListening) return;
            hourListening = on;
            if (on) EventChannel<HourChangedEvent>.Raised += HandleHour;
            else EventChannel<HourChangedEvent>.Raised -= HandleHour;
        }

        // Usulan disetujui #46: isi peti tetap membusuk dengan kecepatan normal (createdGameHour tidak berubah).
        private void HandleHour(HourChangedEvent e) => RaiseChanged();

        // ---------------------------------------------------------------- Hancur atau dibongkar

        private void HandleStructureDestroyed(StructureDestroyedEvent e)
        {
            string id = StructureId;
            if (string.IsNullOrEmpty(id) || !string.Equals(e.structureId, id, StringComparison.Ordinal)) return;
            SpillContents();
        }

        /// <summary>
        /// Usulan disetujui #46: isi tumpah sebagai WorldItem yang tidak hilang otomatis (despawns: false), sehingga tidak
        /// terkena batas waktu maupun maxLooseWorldItems.
        /// </summary>
        private void SpillContents()
        {
            if (spilled) return;
            spilled = true;
            // 1. Panel tertutup dulu.
            var panel = StoragePanel.Instance;
            if (IsOpen && panel != null && UIFocusStack.Instance != null) UIFocusStack.Instance.Close(panel, UICloseReason.Forced);
            SetOpen(false);
            // 2.
            var wim = WorldItemManager.Instance;
            float radius = Balance != null ? Balance.storage.dropScatterRadius : 1.2f;
            for (int i = 0; i < slots.Length; i++)
            {
                var s = slots[i];
                if (s == null || s.IsEmpty) continue;
                if (wim != null)
                {
                    // Posisi acak dari aliran tampilan (UnityEngine.Random, bukan aliran Gameplay).
                    Vector2 r = UnityEngine.Random.insideUnitCircle * radius;
                    Vector3 pos = transform.position + new Vector3(r.x, 1.5f, r.y);
                    if (PhysicsQuery.Raycast(new Ray(pos, Vector3.down), out var hit, 4f, PhysicsMasks.Ground)) pos = hit.point + Vector3.up * 0.15f;
                    // Peti di region yang tidak dimuat (#90): data item masuk RegionStateStore dan muncul saat region kembali (#88).
                    wim.SpawnOrStore(s.instance, s.amount, pos, false);
                }
            }
            // 3.
            for (int i = 0; i < slots.Length; i++) slots[i].Clear();
            RaiseChanged();
        }

        // ---------------------------------------------------------------- ISaveable (lewat StructureRecord.extraJson)

        public string SaveId => StructureId;
        public int LoadOrder => 0;

        public string CaptureState()
        {
            // Barang yang sedang diseret belum dipindahkan datanya, jadi tidak berlipat atau hilang.
            var list = new List<ChestSlotRecord>(slots.Length);
            for (int i = 0; i < slots.Length; i++)
            {
                var s = slots[i];
                if (s == null || s.IsEmpty) continue;
                list.Add(new ChestSlotRecord { slotIndex = i, amount = s.amount, instance = s.instance });
            }
            return JsonUtility.ToJson(new StorageChestSaveData { slotCount = slots.Length, items = list.ToArray() });
        }

        public void RestoreState(string json)
        {
            // 1.
            slots = NewSlots(SlotCountConfig);
            if (string.IsNullOrEmpty(json)) return;
            StorageChestSaveData d = null;
            try { d = JsonUtility.FromJson<StorageChestSaveData>(json); }
            catch (Exception e) { Debug.LogWarning($"[StorageChest] Save tidak terbaca: {e.Message}"); }
            if (d == null || d.items == null) return;

            var overflow = new List<ChestSlotRecord>();
            for (int i = 0; i < d.items.Length; i++)
            {
                var r = d.items[i];
                if (r.instance == null || r.amount <= 0) continue;
                // 2. itemId tidak dikenal: dilewati dengan peringatan (#66).
                var data = ItemDatabase.Get(r.instance.itemId);
                if (data == null)
                {
                    Debug.LogWarning($"[StorageChest] itemId '{r.instance.itemId}' tidak ditemukan, dilewati.");
                    continue;
                }
                // 3-4. Slot di luar panjang baru, atau tumpukan di atas maxStack: lewat Add.
                int max = data.EffectiveMaxStack;
                if (r.slotIndex >= 0 && r.slotIndex < slots.Length && slots[r.slotIndex].IsEmpty && r.amount <= max)
                {
                    // instanceId tetap nomor lama (nextInstanceId dipulihkan lebih dulu, #80).
                    slots[r.slotIndex].instance = r.instance;
                    slots[r.slotIndex].amount = r.amount;
                }
                else overflow.Add(r);
            }
            for (int i = 0; i < overflow.Count; i++)
            {
                var r = overflow[i];
                var inv = InventoryManager.Instance;
                Func<int> alloc = inv != null ? (Func<int>)inv.AllocateInstanceId : null;
                int left = SlotArrayOps.Add(slots, r.instance, r.amount, alloc);
                if (left > 0 && WorldItemManager.Instance != null)
                    WorldItemManager.Instance.Spawn(r.instance, left, transform.position + Vector3.up, false);
            }
        }

        public void OnRestoreFinished()
        {
            RaiseChanged();
        }
    }
}
