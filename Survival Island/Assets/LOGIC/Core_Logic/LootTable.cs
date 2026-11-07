using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Aset tabel loot per lokasi atau per sumber drop. Blueprint: Core_Logic/RegionLootTable.md bagian 1 (#103).
    /// Nama resmi aset menurut GLOSSARY.md. Isi tabel (Loot_Ship, Loot_Region1-4, Loot_Lair_*, Drop_Zombie_*) adalah usulan
    /// dan diisi di Inspector, bukan di kode. Pemilihan isi dilakukan RegionLootTable.TryRoll.
    /// </summary>
    [CreateAssetMenu(menuName = "Survival/Loot Table")]
    public class LootTable : ScriptableObject
    {
        public string tableId;
        [Tooltip("None untuk Drop_* dan Debris_Default.")]
        public LootLocation location;
        public LootEntry[] entries = new LootEntry[0];

        /// <summary>true kalau ada entri dengan bobot positif yang boleh muncul di jenis titik ini (abaikan minDay).</summary>
        public bool HasEntryFor(LootPointKind kind)
        {
            if (entries == null) return false;
            for (int i = 0; i < entries.Length; i++)
                if (entries[i].item != null && entries[i].weight > 0f && (entries[i].kinds & kind) != 0) return true;
            return false;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (entries == null) return;
            for (int i = 0; i < entries.Length; i++)
            {
                if (entries[i].minAmount < 1) entries[i].minAmount = 1;
                if (entries[i].maxAmount < entries[i].minAmount) entries[i].maxAmount = entries[i].minAmount;
                if (entries[i].item == null) Debug.LogWarning($"[LootTable] {name}: entri {i} tanpa item.", this);
            }
        }
#endif
    }
}
