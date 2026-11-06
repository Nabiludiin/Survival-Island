using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Cara node tech tree terbuka (TechTreeManager.md).</summary>
    public enum TechUnlockTrigger { Manual, ItemCrafted, StructureBuilt }

    /// <summary>
    /// Aturan satu node tech tree. Status terbuka TIDAK disimpan di aset, tetapi di TechTreeManager,
    /// supaya aset tidak berubah saat bermain. Blueprint: Core_Logic/TechTreeManager.md (#34).
    /// Isi pohon masih usulan menunggu developer, jadi belum ada aset node yang wajib dibuat.
    /// </summary>
    [CreateAssetMenu(menuName = "Survival/Tech Node")]
    public class TechNodeData : ScriptableObject
    {
        public string nodeId;               // Contoh: node_stone_upgrade
        public string displayNameKey;       // Kunci teks untuk #104
        public string requiredNodeId;       // Kosong kalau tanpa prasyarat
        public TechUnlockTrigger trigger;
        public string triggerKey;           // recipeId untuk ItemCrafted, buildableId untuk StructureBuilt
    }
}
