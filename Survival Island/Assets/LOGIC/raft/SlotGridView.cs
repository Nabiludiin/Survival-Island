using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SurvivalIsland
{
    /// <summary>
    /// Menggambar satu array slot (inventori pemain atau peti) ke sel UI. Hanya tampilan: tidak mengubah data, dan
    /// meneruskan klik serta seret ke pemiliknya (StoragePanel, CampfirePanel). Blueprint: raft/StorageChest.md tabel "Pola" (#46).
    /// Sel dibuat sekali dari cellPrefab lalu dipakai ulang, supaya gambar ulang tanpa alokasi berulang.
    /// </summary>
    public class SlotGridView : MonoBehaviour
    {
        [SerializeField] private Transform cellRoot;
        [SerializeField] private SlotGridViewCell cellPrefab;

        private readonly List<SlotGridViewCell> cells = new List<SlotGridViewCell>(24);

        /// <summary>(index, tombol) saat sel diklik.</summary>
        public event Action<int, PointerEventData.InputButton> Clicked;
        public event Action<int> DragBegan;
        public event Action<int> DroppedOn;
        public event Action DragEnded;

        public int CellCount => cells.Count;

        /// <summary>Menggambar ulang semua sel. Slot null atau kosong digambar kosong.</summary>
        public void Draw(IReadOnlyList<InventorySlot> slots)
        {
            int count = slots != null ? slots.Count : 0;
            EnsureCells(count);
            for (int i = 0; i < cells.Count; i++)
            {
                bool used = i < count;
                cells[i].gameObject.SetActive(used);
                if (!used) continue;
                var s = slots[i];
                if (s == null || s.IsEmpty) { cells[i].SetEmpty(); continue; }
                var data = ItemDatabase.Get(s.instance.itemId);
                cells[i].SetItem(data, s.instance, s.amount);
            }
        }

        /// <summary>Menandai satu sel (misalnya sel yang sedang diseret, atau berkedip saat ditolak).</summary>
        public void SetHighlight(int index, bool on)
        {
            if (index >= 0 && index < cells.Count) cells[index].SetHighlight(on);
        }

        private void EnsureCells(int count)
        {
            if (cellPrefab == null) return;
            var root = cellRoot != null ? cellRoot : transform;
            while (cells.Count < count)
            {
                var c = Instantiate(cellPrefab, root);
                c.Bind(this, cells.Count);
                cells.Add(c);
            }
        }

        internal void RaiseClicked(int index, PointerEventData.InputButton button) => Clicked?.Invoke(index, button);
        internal void RaiseDragBegan(int index) => DragBegan?.Invoke(index);
        internal void RaiseDroppedOn(int index) => DroppedOn?.Invoke(index);
        internal void RaiseDragEnded() => DragEnded?.Invoke();
    }
}
