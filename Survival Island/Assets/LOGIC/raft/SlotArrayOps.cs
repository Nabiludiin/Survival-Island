using System;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Aturan tumpukan bersama untuk inventori pemain dan peti. Blueprint: raft/StorageChest.md bagian "Fungsi SlotArrayOps" (#46),
    /// dipakai InventoryManager.AddItem (ItemUseSystem.md, #66), supaya aturan tumpukan hanya ditulis sekali.
    ///
    /// Catatan implementasi (Langkah 3): parameter memakai IList&lt;InventorySlot&gt;, sehingga array peti (InventorySlot[])
    /// dan List milik InventoryManager sama-sama diterima. Parameter allocateId opsional dipakai saat tumpukan terbagi ke
    /// lebih dari satu slot dan butuh instanceId baru.
    /// </summary>
    public static class SlotArrayOps
    {
        /// <summary>Item dengan data unik per instance tidak pernah digabung.</summary>
        public static bool IsUnique(ItemInstance inst, ItemData data)
        {
            if (data != null && data.EffectiveMaxStack <= 1) return true;
            return inst != null && (inst.durability >= 0f || inst.magazineAmmo >= 0);
        }

        /// <summary>Sama itemId, belum penuh, dan bukan item unik (StorageChest.md).</summary>
        public static bool CanMerge(InventorySlot slot, ItemInstance source)
        {
            if (slot == null || slot.IsEmpty || source == null) return false;
            if (!string.Equals(slot.instance.itemId, source.itemId, StringComparison.Ordinal)) return false;
            var data = ItemDatabase.Get(source.itemId);
            if (data == null || IsUnique(source, data) || IsUnique(slot.instance, data)) return false;
            return slot.amount < data.EffectiveMaxStack;
        }

        /// <summary>
        /// Mengisi tumpukan sejenis dulu, lalu slot kosong. Mengembalikan sisa yang tidak muat.
        /// source tidak disimpan langsung; slot baru memakai salinannya.
        /// </summary>
        public static int Add(IList<InventorySlot> slots, ItemInstance source, int amount, Func<int> allocateId = null)
        {
            if (slots == null || source == null || amount <= 0) return Mathf.Max(0, amount);
            var data = ItemDatabase.Get(source.itemId);
            if (data == null)
            {
                Debug.LogWarning($"[SlotArrayOps] itemId tidak dikenal: {source.itemId}");
                return amount;
            }

            int max = data.EffectiveMaxStack;
            int remaining = amount;

            // 1. Gabung ke tumpukan sejenis.
            if (!IsUnique(source, data))
            {
                for (int i = 0; i < slots.Count && remaining > 0; i++)
                {
                    var slot = slots[i];
                    if (!CanMerge(slot, source)) continue;
                    int take = Mathf.Min(max - slot.amount, remaining);
                    MergeFreshness(slot.instance, slot.amount, source, take);
                    slot.amount += take;
                    remaining -= take;
                }
            }

            // 2. Isi slot kosong.
            bool firstNewSlot = true;
            for (int i = 0; i < slots.Count && remaining > 0; i++)
            {
                var slot = slots[i];
                if (slot == null || !slot.IsEmpty) continue;
                int take = Mathf.Min(max, remaining);
                var inst = source.Clone();
                if (!firstNewSlot && allocateId != null) inst.instanceId = allocateId();
                firstNewSlot = false;
                slot.instance = inst;
                slot.amount = take;
                remaining -= take;
            }
            return remaining;
        }

        /// <summary>Menghitung jumlah satu itemId (StorageChest.md).</summary>
        public static int CountItem(IList<InventorySlot> slots, string itemId)
        {
            if (slots == null || string.IsNullOrEmpty(itemId)) return 0;
            int total = 0;
            for (int i = 0; i < slots.Count; i++)
            {
                var slot = slots[i];
                if (slot == null || slot.IsEmpty) continue;
                if (string.Equals(slot.instance.itemId, itemId, StringComparison.Ordinal)) total += slot.amount;
            }
            return total;
        }

        /// <summary>
        /// ItemUseSystem.md "Pembusukan": dua tumpukan digabung, createdGameHour memakai rata-rata berbobot jumlahnya.
        /// </summary>
        private static void MergeFreshness(ItemInstance target, int targetAmount, ItemInstance source, int sourceAmount)
        {
            if (target.createdGameHour < 0f || source.createdGameHour < 0f) return;
            int total = targetAmount + sourceAmount;
            if (total <= 0) return;
            target.createdGameHour = (target.createdGameHour * targetAmount + source.createdGameHour * sourceAmount) / total;
        }
    }
}
