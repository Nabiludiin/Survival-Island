using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Resep api unggun: cook_meat, cook_fish, boil_water, burn_charcoal. Turunan CraftingRecipe dengan station = Campfire,
    /// sehingga semua resep berada di satu RecipeDatabase dan CraftingManager melewatinya.
    /// Blueprint: raft/CampfireManager.md bagian "Data resep" (#44). craftSeconds menjadi lama memasak.
    /// Usulan disetujui #44: gosong 30 detik setelah matang menjadi Food_Burnt; air rebus tidak bisa gosong.
    ///
    /// Catatan implementasi (sistem pendukung): RecipeDatabase.FindCookingByInput belum ada dan RecipeDatabase adalah kode lama,
    /// jadi pencarian ditulis sebagai fungsi statis di sini (FindByInput). Bahan pertama resep dianggap input.
    /// </summary>
    [CreateAssetMenu(menuName = "Survival/Cooking Recipe")]
    public class CookingRecipe : CraftingRecipe
    {
        public bool canBurn = true;
        [Tooltip("Kurang dari 0 = memakai GameBalanceConfig.campfire.burnAfterSeconds")]
        public float burnAfterSeconds = -1f;
        public ItemData burntResult;     // Food_Burnt

        private void OnEnable()
        {
            station = CraftingStationType.Campfire;
        }

        /// <summary>Asal lama memasak. Config dipakai supaya angka 30/20 detik bisa disetel di satu tempat (#84).</summary>
        public enum CookTimeSource { RecipeCraftSeconds, CampfireCookSeconds, CampfireBoilSeconds }
        public CookTimeSource timeSource = CookTimeSource.CampfireCookSeconds;

        public float EffectiveCookSeconds
        {
            get
            {
                var b = GameManager.CurrentBalance;
                switch (timeSource)
                {
                    case CookTimeSource.CampfireCookSeconds: return b != null ? b.campfire.cookSeconds : 30f;
                    case CookTimeSource.CampfireBoilSeconds: return b != null ? b.campfire.boilSeconds : 20f;
                    default: return Mathf.Max(0.01f, craftSeconds);
                }
            }
        }

        public ItemData InputItem => ingredients != null && ingredients.Length > 0 ? ingredients[0].item : null;

        public float EffectiveBurnAfterSeconds
        {
            get
            {
                if (burnAfterSeconds >= 0f) return burnAfterSeconds;
                var b = GameManager.CurrentBalance;
                return b != null ? b.campfire.burnAfterSeconds : 30f;
            }
        }

        /// <summary>Resep api unggun yang inputnya itemId ini. null kalau tidak bisa dimasak.</summary>
        public static CookingRecipe FindByInput(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return null;
            var all = RecipeDatabase.All;
            for (int i = 0; i < all.Count; i++)
            {
                if (!(all[i] is CookingRecipe c)) continue;
                var input = c.InputItem;
                if (input != null && string.Equals(input.itemID, itemId, System.StringComparison.Ordinal)) return c;
            }
            return null;
        }
    }
}
