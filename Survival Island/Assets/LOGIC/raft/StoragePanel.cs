using UnityEngine;
using UnityEngine.EventSystems;

namespace SurvivalIsland
{
    public enum SlotSide { Left, Right }

    /// <summary>
    /// Panel peti dua sisi (UIPanelId.Storage): kiri inventori pemain, kanan isi peti. Satu instance di IslandPhase.
    /// Tidak menjeda game (UIFocusStack.md). Blueprint: raft/StorageChest.md bagian "Panel dua sisi" (#46).
    /// Pengaturan Inspector: pausesTime = false, releasesCursor = true, blocksGameplayInput = true, closableByEsc = true.
    ///
    /// UI tidak mengubah tampilan langsung: data diubah lewat InventoryManager dan StorageChest, lalu sisi yang kotor
    /// digambar ulang sekali per frame di LateUpdate.
    /// Usulan disetujui #46: klik kanan memindahkan seluruh tumpukan; memindah sebagian lewat panel Inventory.
    /// </summary>
    public class StoragePanel : FadingUIPanel
    {
        public static StoragePanel Instance { get; private set; }

        [SerializeField] private SlotGridView leftGrid;    // Inventori
        [SerializeField] private SlotGridView rightGrid;   // Peti

        private StorageChest chest;
        private bool listening;
        private bool leftDirty, rightDirty;

        // DragState: satu barang yang sedang diseret. Data belum berpindah sampai dilepas.
        private bool dragging;
        private SlotSide dragSide;
        private int dragIndex = -1;
        private bool dropHandled;

        public StorageChest Bound => chest;

        private void Awake()
        {
            Instance = this;
        }

        protected override void OnDestroy()
        {
            StopListening();
            if (Instance == this) Instance = null;
            base.OnDestroy();
        }

        public void Bind(StorageChest c)
        {
            if (chest == c) return;
            if (chest != null && listening) chest.ContentsChanged -= HandleChestChanged;
            chest = c;
            if (chest != null && listening) chest.ContentsChanged += HandleChestChanged;
            leftDirty = rightDirty = true;
        }

        protected override void Show()
        {
            StartListening();
            leftDirty = rightDirty = true;
            Redraw();
            base.Show();
        }

        protected override void Hide()
        {
            StopListening();
            base.Hide();
        }

        public override void OnClosed(UICloseReason reason)
        {
            // Barang yang diseret kembali ke slot asal (data belum dipindah). Berlaku juga saat mati atau diterkam (#83).
            CancelDrag();
            if (chest != null) chest.SetOpen(false);
            Bind(null);
            base.OnClosed(reason);
        }

        private void StartListening()
        {
            if (listening) return;
            listening = true;
            EventChannel<InventoryChangedEvent>.Raised += HandleInventoryChanged;
            if (chest != null) chest.ContentsChanged += HandleChestChanged;
            if (leftGrid != null)
            {
                leftGrid.Clicked += OnLeftClicked;
                leftGrid.DragBegan += OnLeftDragBegan;
                leftGrid.DroppedOn += OnLeftDropped;
                leftGrid.DragEnded += OnDragEnded;
            }
            if (rightGrid != null)
            {
                rightGrid.Clicked += OnRightClicked;
                rightGrid.DragBegan += OnRightDragBegan;
                rightGrid.DroppedOn += OnRightDropped;
                rightGrid.DragEnded += OnDragEnded;
            }
        }

        private void StopListening()
        {
            if (!listening) return;
            listening = false;
            EventChannel<InventoryChangedEvent>.Raised -= HandleInventoryChanged;
            if (chest != null) chest.ContentsChanged -= HandleChestChanged;
            if (leftGrid != null)
            {
                leftGrid.Clicked -= OnLeftClicked;
                leftGrid.DragBegan -= OnLeftDragBegan;
                leftGrid.DroppedOn -= OnLeftDropped;
                leftGrid.DragEnded -= OnDragEnded;
            }
            if (rightGrid != null)
            {
                rightGrid.Clicked -= OnRightClicked;
                rightGrid.DragBegan -= OnRightDragBegan;
                rightGrid.DroppedOn -= OnRightDropped;
                rightGrid.DragEnded -= OnDragEnded;
            }
        }

        private void HandleInventoryChanged(InventoryChangedEvent e) => leftDirty = true;
        private void HandleChestChanged() => rightDirty = true;

        private void LateUpdate()
        {
            if (!listening) return;
            Redraw();
        }

        private void Redraw()
        {
            if (leftDirty)
            {
                leftDirty = false;
                if (leftGrid != null && InventoryManager.Instance != null) leftGrid.Draw(InventoryManager.Instance.Slots);
            }
            if (rightDirty)
            {
                rightDirty = false;
                if (rightGrid != null && chest != null) rightGrid.Draw(chest.Slots);
            }
        }

        // Panel lain (misalnya Pause) bisa menumpuk di atas; klik UI hanya diproses saat panel ini di puncak.
        private bool IsTop => UIFocusStack.Instance != null && UIFocusStack.Instance.TopPanel == this;

        // ---------------------------------------------------------------- Klik kanan: pemindahan cepat

        private void OnLeftClicked(int index, PointerEventData.InputButton button)
        {
            if (button == PointerEventData.InputButton.Right) QuickTransfer(SlotSide.Left, index);
        }

        private void OnRightClicked(int index, PointerEventData.InputButton button)
        {
            if (button == PointerEventData.InputButton.Right) QuickTransfer(SlotSide.Right, index);
        }

        private void QuickTransfer(SlotSide side, int index)
        {
            if (dragging || !IsTop || chest == null) return;   // Klik kanan saat menyeret diabaikan
            var inv = InventoryManager.Instance;
            if (inv == null) return;
            // 1.
            var src = side == SlotSide.Left ? inv.GetSlot(index) : chest.GetSlot(index);
            if (src == null || src.IsEmpty) return;
            var inst = src.instance;
            int amount = src.amount;
            // 2.
            if (side == SlotSide.Left && !StorageChest.CanStore(ItemDatabase.Get(inst.itemId)))
            {
                Notify("notif.cannot_store");   // "Tidak bisa disimpan di peti"
                return;
            }
            // 3-4.
            int left = side == SlotSide.Left ? chest.Add(inst, amount) : inv.AddItem(inst, amount);
            int moved = amount - left;
            if (moved == 0)
            {
                Notify(side == SlotSide.Left ? "notif.chest_full" : "notif.inventory_full");   // "Peti penuh" / "Inventori penuh"
                return;
            }
            // 5. Rujukan hotbar lepas lewat InventoryChangedEvent (#60).
            if (side == SlotSide.Left) inv.RemoveFromSlot(index, moved);
            else chest.RemoveFromSlot(index, moved);
            // 6. Sinyal dikirim InventoryManager dan StorageChest.
        }

        // ---------------------------------------------------------------- Seret dan lepas

        private void OnLeftDragBegan(int index) => BeginDrag(SlotSide.Left, index);
        private void OnRightDragBegan(int index) => BeginDrag(SlotSide.Right, index);
        private void OnLeftDropped(int index) => Drop(SlotSide.Left, index);
        private void OnRightDropped(int index) => Drop(SlotSide.Right, index);

        private void BeginDrag(SlotSide side, int index)
        {
            if (!IsTop || chest == null) return;
            var s = GetSlot(side, index);
            if (s == null || s.IsEmpty) return;
            dragging = true;
            dragSide = side;
            dragIndex = index;
            dropHandled = false;
            Grid(side)?.SetHighlight(index, true);
        }

        private void OnDragEnded()
        {
            // Lepas di luar panel: kembali ke slot asal (data tidak pernah berpindah). Membuang item hanya lewat panel Inventory.
            CancelDrag();
        }

        private void CancelDrag()
        {
            if (dragging) Grid(dragSide)?.SetHighlight(dragIndex, false);
            dragging = false;
            dragIndex = -1;
            dropHandled = false;
        }

        private void Drop(SlotSide side, int index)
        {
            if (!dragging || dropHandled || !IsTop || chest == null) return;
            dropHandled = true;
            var inv = InventoryManager.Instance;
            if (inv == null) return;
            var from = GetSlot(dragSide, dragIndex);
            if (from == null || from.IsEmpty) return;

            // Sisi yang sama: tukar isi slot.
            if (side == dragSide)
            {
                if (side == SlotSide.Left) inv.MoveSlot(dragIndex, index);
                else chest.SwapSlots(dragIndex, index);
                return;
            }

            // Menuju peti: item inti dan item wajib ditolak.
            if (side == SlotSide.Right && !StorageChest.CanStore(ItemDatabase.Get(from.instance.itemId)))
            {
                Notify("notif.cannot_store");
                return;
            }

            var to = GetSlot(side, index);
            if (to == null) return;
            if (to.IsEmpty || SlotArrayOps.CanMerge(to, from.instance))
            {
                MoveIntoSlot(dragSide, dragIndex, side, index);
                return;
            }
            // Slot berisi item berbeda: tukar kedua isi, kalau isi seberang juga boleh masuk peti.
            var toData = ItemDatabase.Get(to.instance.itemId);
            if (dragSide == SlotSide.Right && !StorageChest.CanStore(toData))
            {
                Notify("notif.cannot_store");
                return;
            }
            SwapAcross(dragSide, dragIndex, index);
        }

        /// <summary>Pindah atau gabung ke satu slot tujuan di sisi seberang.</summary>
        private void MoveIntoSlot(SlotSide fromSide, int fromIndex, SlotSide toSide, int toIndex)
        {
            var inv = InventoryManager.Instance;
            var from = GetSlot(fromSide, fromIndex);
            var to = GetSlot(toSide, toIndex);
            var inst = from.instance;
            int amount = from.amount;
            int moved;
            if (to.IsEmpty)
            {
                var data = ItemDatabase.Get(inst.itemId);
                moved = Mathf.Min(amount, data != null ? data.EffectiveMaxStack : amount);
                if (toSide == SlotSide.Right) chest.SetSlot(toIndex, inst.Clone(), moved);
                else
                {
                    // InventoryManager tidak punya pengisian slot tertentu: tambah lalu pindahkan ke slot tujuan.
                    int before = inv.IndexOfInstance(inst.instanceId);
                    int left = inv.AddItem(inst, moved);
                    moved -= left;
                    int placed = inv.IndexOfInstance(inst.instanceId);
                    if (moved > 0 && placed >= 0 && placed != toIndex && before < 0) inv.MoveSlot(placed, toIndex);
                }
            }
            else
            {
                // Gabung: hanya sebanyak ruang di tumpukan tujuan.
                var data = ItemDatabase.Get(inst.itemId);
                int room = data != null ? data.EffectiveMaxStack - to.amount : 0;
                moved = Mathf.Min(amount, Mathf.Max(0, room));
                if (moved <= 0 || !SlotArrayOps.CanMerge(to, inst)) return;
                if (toSide == SlotSide.Right) moved -= chest.Add(inst, moved);
                else moved -= inv.AddItem(inst, moved);
            }
            if (moved <= 0) return;
            if (fromSide == SlotSide.Left) inv.RemoveFromSlot(fromIndex, moved);
            else chest.RemoveFromSlot(fromIndex, moved);
        }

        /// <summary>Menukar isi slot inventori dan slot peti yang berisi item berbeda.</summary>
        private void SwapAcross(SlotSide fromSide, int fromIndex, int toIndex)
        {
            var inv = InventoryManager.Instance;
            int invIndex = fromSide == SlotSide.Left ? fromIndex : toIndex;
            int chestIndex = fromSide == SlotSide.Left ? toIndex : fromIndex;
            var invSlot = inv.GetSlot(invIndex);
            var chestSlot = chest.GetSlot(chestIndex);
            if (invSlot == null || chestSlot == null || invSlot.IsEmpty || chestSlot.IsEmpty) return;

            var invInst = invSlot.instance; int invAmt = invSlot.amount;
            var chestInst = chestSlot.instance; int chestAmt = chestSlot.amount;
            // Instance yang sama dipakai saat pindah, jadi durabilitas dan isi magazine tidak berubah.
            inv.RemoveFromSlot(invIndex, invAmt);
            chest.SetSlot(chestIndex, invInst, invAmt);
            int left = inv.AddItem(chestInst, chestAmt);
            int placed = inv.IndexOfInstance(chestInst.instanceId);
            if (placed >= 0 && placed != invIndex) inv.MoveSlot(placed, invIndex);
            if (left > 0)
            {
                // Tidak muat (seharusnya tidak terjadi karena slot baru kosong): kembalikan seperti semula.
                inv.RemoveInstance(chestInst.instanceId);
                chest.SetSlot(chestIndex, chestInst, chestAmt);
                inv.AddItem(invInst, invAmt);
            }
        }

        private InventorySlot GetSlot(SlotSide side, int index)
        {
            if (side == SlotSide.Left) return InventoryManager.Instance != null ? InventoryManager.Instance.GetSlot(index) : null;
            return chest != null ? chest.GetSlot(index) : null;
        }

        private SlotGridView Grid(SlotSide side) => side == SlotSide.Left ? leftGrid : rightGrid;

        private static void Notify(string key)
        {
            if (NotificationManager.Instance != null) NotificationManager.Instance.Show(key, NotificationKind.Warning);
        }
    }
}
