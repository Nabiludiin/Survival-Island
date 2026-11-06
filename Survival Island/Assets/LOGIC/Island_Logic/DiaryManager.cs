using System;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Pintu masuk Diary: menampilkan isi satu bab dan membuka arsip baca ulang.
    /// Blueprint: Island_Logic/DiaryManager.md (#81, melengkapi #83, #18, #104). GDD bagian 4: satu buku Diary,
    /// Bab 1 sampai 3 terbuka bertahap.
    ///
    /// Claude.md Tahap 5: saat membaca Diary, game dijeda (Time.timeScale = 0). Sesuai blueprint, kelas ini TIDAK
    /// menulis Time.timeScale atau Cursor sendiri; panel Diary dan DiaryArchive memakai pausesTime di UIFocusStack,
    /// yang menjadi satu-satunya pemilik jeda, kursor, dan Esc.
    ///
    /// DiaryManager tidak menyimpan apa pun. Status bab milik ObjectiveManager (#81, belum ditulis).
    /// </summary>
    public class DiaryManager : MonoBehaviour
    {
        public static DiaryManager Instance { get; private set; }

        [SerializeField] private DiaryPanel diaryPanel;
        [SerializeField] private DiaryArchivePanel archivePanel;

        /// <summary>
        /// Diisi ObjectiveManager (#81) dengan pemeriksaan unlockedChapters. null berarti belum ada bab terbuka.
        /// </summary>
        public Func<int, bool> ChapterUnlockedQuery { get; set; }

        public bool IsChapterUnlocked(int chapter) => ChapterUnlockedQuery != null && ChapterUnlockedQuery(chapter);

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("[DiaryManager] Instance ganda di scene IslandPhase.");
                return;
            }
            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        /// <summary>Menampilkan satu bab. Dipanggil ObjectiveManager (#81) dan DiaryArchivePanel.</summary>
        public void OpenDiary(DiaryEntry entry)
        {
            if (entry == null || diaryPanel == null || UIFocusStack.Instance == null) return;
            diaryPanel.SetEntry(entry);
            // Panel Diary menjeda game dan melepas kursor sesuai tabel UIFocusStack.md.
            UIFocusStack.Instance.Push(diaryPanel);
        }

        /// <summary>Membuka daftar bab terbuka. Dipakai tombol J dan ObjectiveManager.InteractBook untuk buku yang sudah ditemukan.</summary>
        public void OpenArchive()
        {
            if (archivePanel == null || UIFocusStack.Instance == null) return;
            UIFocusStack.Instance.Push(archivePanel);
        }

        private void Update()
        {
            // Usulan disetujui (#85): tombol J membuka arsip, ditolak saat panel lain menahan input gameplay.
            var input = InputManager.Instance;
            var focus = UIFocusStack.Instance;
            if (input == null || focus == null) return;
            if (input.OpenDiaryPressed && !focus.IsGameplayInputBlocked) OpenArchive();
        }
    }
}
