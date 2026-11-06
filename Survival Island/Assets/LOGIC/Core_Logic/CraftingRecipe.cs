using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Data satu resep. Blueprint: Core_Logic/CraftingStation.md "Data" (#77). Waktu dan bahan tiap resep berada di aset ini,
    /// bukan di GameBalanceConfig. Resep inti memakai startsUnlocked = true (alat dasar selalu bisa dibuat dengan tangan).
    /// CookingRecipe (#44) mewarisi kelas ini dengan station = Campfire; CraftingPanel dan RequestCraft melewatinya.
    /// </summary>
    [CreateAssetMenu(menuName = "Survival/Crafting Recipe")]
    public class CraftingRecipe : ScriptableObject
    {
        public string recipeId;              // Contoh: rec_vaccine
        public string displayNameKey;        // Kunci teks untuk #104
        public RecipeIngredient[] ingredients;
        public ItemData result;
        public int resultAmount = 1;
        public float craftSeconds = 3f;
        public CraftingStationType station;
        public string techNodeId;            // Kosong kalau tidak terikat tech tree
        public bool startsUnlocked;
    }
}
