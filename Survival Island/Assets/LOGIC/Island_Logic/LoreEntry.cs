using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Bentuk temuan: catatan, rekaman, atau papan (LoreCollectibles.md, #100).</summary>
    public enum LoreKind { Note, Recording, Board }

    /// <summary>Asal temuan. GDD bagian 2: pulau bekas eksperimen dan jejak militer.</summary>
    public enum LoreSource { Researcher, Military, Experiment }

    /// <summary>Petunjuk gameplay. Usulan disetujui: hanya 2 log militer Region 4 memakai PartLocation.</summary>
    public enum LoreHintKind { None, PartLocation }

    /// <summary>
    /// Satu temuan cerita opsional. Blueprint: Island_Logic/LoreCollectibles.md bagian 1 (#100).
    /// GDD bagian 5: Region 4 adalah pusat lore, jadi paling banyak temuan di sana (usulan sebaran 20 temuan).
    /// </summary>
    [CreateAssetMenu(menuName = "Survival/Lore Entry")]
    public class LoreEntry : ScriptableObject
    {
        [Tooltip("Contoh lore_r4_archive_01. Stabil, karena disimpan di save dan lore_archive.json.")]
        public string loreId;
        public string titleKey;           // lore.<loreId>.title
        public string bodyKey;            // lore.<loreId>.body
        public RegionType region;
        public LoreKind kind;
        public LoreSource source;
        [Tooltip("Hanya untuk Recording.")]
        public AudioClip recordingClip;
        public LoreHintKind hint = LoreHintKind.None;
        public int sortOrder;

        public string TitleKey => string.IsNullOrEmpty(titleKey) ? "lore." + loreId + ".title" : titleKey;
        public string BodyKey => string.IsNullOrEmpty(bodyKey) ? "lore." + loreId + ".body" : bodyKey;
    }
}
