using System;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Pencarian CraftingRecipe berdasarkan recipeId (CraftingStation.md: "RecipeDatabase memuat semua resep").
    ///
    /// Catatan implementasi (Langkah 4, usulan teknis): sama dengan ItemDatabase Langkah 3, aset dimuat sekali dari folder
    /// Resources/Recipes saat pertama dipakai. CookingRecipe (#44) ikut termuat karena mewarisi CraftingRecipe.
    /// </summary>
    public static class RecipeDatabase
    {
        public const string ResourcesFolder = "Recipes";

        private static Dictionary<string, CraftingRecipe> byId;
        private static List<CraftingRecipe> all;

        public static CraftingRecipe Get(string recipeId)
        {
            if (string.IsNullOrEmpty(recipeId)) return null;
            EnsureLoaded();
            return byId.TryGetValue(recipeId, out var r) ? r : null;
        }

        /// <summary>Semua resep, urut sesuai pemuatan. Jangan diubah oleh pemanggil.</summary>
        public static IReadOnlyList<CraftingRecipe> All
        {
            get { EnsureLoaded(); return all; }
        }

        /// <summary>Mengganti isi database secara manual (tes).</summary>
        public static void Register(IEnumerable<CraftingRecipe> recipes)
        {
            byId = new Dictionary<string, CraftingRecipe>(StringComparer.Ordinal);
            all = new List<CraftingRecipe>();
            if (recipes == null) return;
            foreach (var r in recipes)
            {
                if (r == null) continue;
                if (string.IsNullOrEmpty(r.recipeId)) { Debug.LogError($"[RecipeDatabase] Resep '{r.name}' tanpa recipeId."); continue; }
                if (byId.ContainsKey(r.recipeId)) { Debug.LogError($"[RecipeDatabase] recipeId ganda: {r.recipeId}"); continue; }
                byId.Add(r.recipeId, r);
                all.Add(r);
            }
        }

        private static void EnsureLoaded()
        {
            if (byId != null) return;
            Register(Resources.LoadAll<CraftingRecipe>(ResourcesFolder));
        }
    }
}
