using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Jenis upgrade kecil per item. Nilai mengikuti GLOSSARY.md (#113).</summary>
    public enum UpgradeKind { Damage, MagazineSize, Speed, Durability }

    /// <summary>
    /// Satu upgrade item (Upg_Damage, Upg_Magazine, Upg_Speed, Upg_Durability). Blueprint: Core_Logic/ToolRepairSystem.md
    /// bagian "Upgrade" (#113). Usulan disetujui developer: sekali per jenis, maksimum upgrade.maxPerItem per item.
    /// Tiap aset satu bit di ItemInstance.upgradeMask. maskBit TIDAK boleh dipakai ulang setelah rilis.
    /// Aset diletakkan di Resources/ItemUpgrades supaya terbaca WeaponStatCalculator (catatan implementasi).
    /// </summary>
    [CreateAssetMenu(menuName = "Survival/Item Upgrade")]
    public class ItemUpgradeData : ScriptableObject
    {
        public string upgradeId;
        [Range(0, 30)] public int maskBit;      // Tetap setelah rilis
        public UpgradeKind kind;
        public ItemType[] appliesTo = new ItemType[0];
        public RecipeIngredient[] cost = new RecipeIngredient[0];
        public CraftingStationType station = CraftingStationType.WoodBench;
        [Tooltip("Kosong berarti terbuka sejak awal.")]
        public string techNodeId;

        public int Mask => 1 << Mathf.Clamp(maskBit, 0, 30);

        public bool AppliesTo(ItemType type)
        {
            if (appliesTo == null) return false;
            for (int i = 0; i < appliesTo.Length; i++) if (appliesTo[i] == type) return true;
            return false;
        }
    }
}
