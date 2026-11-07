using System;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Jenis hadiah bab Diary (ObjectiveManager.md bagian 1, #81).</summary>
    public enum DiaryRewardKind { None, GiveItem, RevealObjectives }

    /// <summary>Satu hadiah bab Diary. Diberikan ObjectiveManager (#81).</summary>
    [Serializable]
    public struct DiaryReward
    {
        public DiaryRewardKind kind;
        public ItemData item;
        public int amount;
    }

    /// <summary>
    /// Satu bab Diary. Blueprint: Island_Logic/ObjectiveManager.md bagian 1 (#81). GDD bagian 4: satu buku Diary,
    /// Bab 1 sampai 3 terbuka bertahap. Ditulis Langkah 7 karena DiaryManager.OpenDiary(DiaryEntry) memakainya.
    /// ObjectiveManager (belum ditulis) memakai tipe ini dan tidak mendeklarasikannya ulang.
    ///
    /// Tambahan sementara: fallbackTitle dan fallbackBody dipakai selama LocalizationManager (#104) belum ada.
    /// Dihapus saat #104 ditulis.
    /// </summary>
    [CreateAssetMenu(menuName = "Survival/Diary Entry")]
    public class DiaryEntry : ScriptableObject
    {
        [Range(1, 3)] public int chapter = 1;
        public string titleKey;     // diary.ch<chapter>.title (#104)
        public string bodyKey;      // diary.ch<chapter>.body (#104)
        public DiaryReward[] rewards = Array.Empty<DiaryReward>();

        [Header("Cadangan kalau kunci belum ada di tabel LocalizationManager (#104)")]
        public string fallbackTitle;
        [TextArea(6, 20)] public string fallbackBody;

        // Teks dari LocalizationManager (#104). Kunci yang belum ada memakai teks cadangan, lalu kunci mentah.
        public string ResolveTitle() => Resolve(titleKey, fallbackTitle);
        public string ResolveBody() => Resolve(bodyKey, fallbackBody);

        private static string Resolve(string key, string fallback)
        {
            var loc = LocalizationManager.Instance;
            if (loc != null && loc.Has(key)) return loc.Get(key);
            return !string.IsNullOrEmpty(fallback) ? fallback : key;
        }
    }
}
