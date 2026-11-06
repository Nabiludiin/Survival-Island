using System;

namespace SurvivalIsland
{
    // Kontrak milik CraftingStation / CraftingManager (#77, Langkah 4), blueprint Core_Logic/CraftingStation.md.
    // Ditulis di Langkah 3 karena InventoryManager.CanFitAfterRemoval dan ItemData.repairStation sudah memakainya.
    // Langkah 4 TIDAK mendeklarasikan ulang tipe di file ini.

    /// <summary>Satu bahan resep atau biaya (CraftingStation.md).</summary>
    [Serializable]
    public struct RecipeIngredient
    {
        public ItemData item;
        public int amount;
    }

    /// <summary>Jenis stasiun crafting. Nilai mengikuti GLOSSARY.md.</summary>
    public enum CraftingStationType { Hand, WoodBench, MedicalBench, Campfire }
}
