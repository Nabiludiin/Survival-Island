using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Status durabilitas item. Nilai mengikuti GLOSSARY.md (#113). Dihitung dari data, tidak disimpan.</summary>
    public enum DurabilityState { NoDurability, Normal, Low, Broken, Destroyed }

    /// <summary>
    /// Mengurangi durabilitas alat dan senjata serta membaca statusnya. Kelas statis tanpa status; nilai ada di
    /// ItemInstance.durability (-1 = tanpa durabilitas). Blueprint: Core_Logic/DurabilitySystem.md (#113, #66, #60, #72).
    /// Aturan status dan rumus batas maksimum ditetapkan Core_Logic/ToolRepairSystem.md.
    ///
    /// GDD bagian 7: Fire Axe tidak bisa rusak (maxDurability -1). Durabilitas alat biasa adalah usulan disetujui developer (#113).
    /// Jumlah keausan per aksi dikirim pemanggil sebagai amount (tabel di blueprint).
    /// </summary>
    public static class DurabilitySystem
    {
        private static GameBalanceConfig Balance => GameManager.CurrentBalance;

        /// <summary>Kurangi durabilitas. Event dikirim sebelum RemoveInstance, supaya HUD dan notifikasi masih bisa membaca nama item.</summary>
        public static DurabilityState ApplyWear(ItemInstance inst, float amount)
        {
            // 1.
            if (inst == null || amount <= 0f) return GetState(inst);
            // 2. Reward_FireAxe dan mod_indestructible (#72).
            if (inst.durability < 0f) return DurabilityState.NoDurability;

            // 3-5.
            var before = GetState(inst);
            if (before == DurabilityState.NoDurability) return before;
            inst.durability = Mathf.Max(0f, inst.durability - amount);
            var after = GetState(inst);

            // 6. Item Broken yang tetap terpanggil: durability tetap 0, status sama, tidak ada event ganda.
            if (after != before)
                EventChannel<DurabilityStateChangedEvent>.Publish(new DurabilityStateChangedEvent(inst.instanceId, inst.itemId, after));

            // 7. Hanya Destroyed yang menghapus item. EquipmentManager lalu menjalankan alur item hancur.
            if (after == DurabilityState.Destroyed && InventoryManager.Instance != null)
                InventoryManager.Instance.RemoveInstance(inst.instanceId);
            return after;
        }

        public static DurabilityState GetState(ItemInstance inst)
        {
            // 1.
            if (inst == null || inst.durability < 0f) return DurabilityState.NoDurability;

            // 2.
            var data = ItemDatabase.Get(inst.itemId);
            if (data == null)
            {
                Debug.LogWarning($"[DurabilitySystem] ItemData {inst.itemId} tidak ditemukan.");
                return DurabilityState.NoDurability;
            }
            if (data.maxDurability < 0f) return DurabilityState.NoDurability;

            var repair = Balance != null ? Balance.repair : null;
            int maxRepairs = repair != null ? repair.maxRepairs : 6;
            float lowRatio = repair != null ? repair.lowRatio : 0.2f;

            // 3. Durabilitas 0 berarti Broken. Item inti (Pistol, Kail, Pisau) tidak pernah Destroyed (keputusan #113, #58).
            if (inst.durability <= 0f)
            {
                if (!data.isCoreItem && inst.repairCount >= maxRepairs) return DurabilityState.Destroyed;
                return DurabilityState.Broken;
            }

            // 4-5.
            if (inst.durability <= GetEffectiveMax(inst, data) * lowRatio) return DurabilityState.Low;
            return DurabilityState.Normal;
        }

        /// <summary>
        /// Batas maksimum setelah pengurangan per perbaikan, upgrade Durability, dan modifier Lucky Machine.
        /// Rumus: ToolRepairSystem.md bagian "Pengurangan durabilitas". Item inti tidak terkena pengurangan.
        /// </summary>
        public static float GetEffectiveMax(ItemInstance inst, ItemData data)
        {
            if (data == null || data.maxDurability < 0f) return -1f;
            if (inst == null) return data.maxDurability;

            float maxLoss = Balance != null ? Balance.repair.maxLossPerRepair : 0.1f;
            float durabilityBonus = Balance != null ? Balance.upgrade.durabilityBonus : 0.25f;

            float lossRatio = data.isCoreItem ? 0f : inst.repairCount * maxLoss;
            float upgrade = WeaponStatCalculator.HasUpgrade(inst, UpgradeKind.Durability) ? 1f + durabilityBonus : 1f;
            var mod = WeaponStatCalculator.FindModifier(inst.luckyModifierId);
            float lucky = mod != null ? mod.durabilityMultiplier : 1f;
            return data.maxDurability * Mathf.Max(0f, 1f - lossRatio) * upgrade * lucky;
        }

        /// <summary>
        /// Rasio bar HUD 0 sampai 1 (usulan nama di blueprint, menunggu developer). 1 untuk NoDurability.
        /// Nilai lama yang melebihi maksimum baru dibatasi ke 1.
        /// </summary>
        public static float GetRatio(ItemInstance inst)
        {
            if (inst == null || inst.durability < 0f) return 1f;
            var data = ItemDatabase.Get(inst.itemId);
            if (data == null || data.maxDurability < 0f) return 1f;
            float max = GetEffectiveMax(inst, data);
            if (max <= 0f) return 0f;
            return Mathf.Clamp01(inst.durability / max);
        }
    }
}
