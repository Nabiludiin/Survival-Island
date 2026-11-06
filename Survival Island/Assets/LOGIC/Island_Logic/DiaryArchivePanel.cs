using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SurvivalIsland
{
    /// <summary>
    /// Daftar bab Diary yang sudah terbuka (UIPanelId.DiaryArchive). Blueprint: Island_Logic/DiaryManager.md bagian 1
    /// dan ObjectiveManager.md (#81). Menjeda game seperti Diary (pengaturan panel di Inspector, UIFocusStack.md).
    /// Keputusan developer: bab yang terbuka bisa dibaca ulang dari menu setelah buku ditemukan.
    ///
    /// Yang belum: daftar tujuan selesai dan tab Arsip (LoreCollectibles #100) milik ObjectiveManager, yang belum ditulis.
    /// Status bab terbuka dibaca lewat DiaryManager.IsChapterUnlocked, yang diisi ObjectiveManager.
    /// </summary>
    public class DiaryArchivePanel : FadingUIPanel
    {
        [Tooltip("Semua bab, urut Bab 1 sampai 3. Sama dengan ObjectiveManager.chapters.")]
        [SerializeField] private DiaryEntry[] chapters = new DiaryEntry[0];
        [Tooltip("Satu tombol per bab, sejajar dengan chapters.")]
        [SerializeField] private Button[] chapterButtons = new Button[0];
        [SerializeField] private TMP_Text emptyText;
        [SerializeField] private Button closeButton;

        private bool listenersAdded;

        private void Awake()
        {
            AddListeners();
        }

        private void AddListeners()
        {
            if (listenersAdded) return;
            listenersAdded = true;
            for (int i = 0; i < chapterButtons.Length; i++)
            {
                if (chapterButtons[i] == null) continue;
                int index = i;
                chapterButtons[i].onClick.AddListener(() => HandleChapterClicked(index));
            }
            if (closeButton != null) closeButton.onClick.AddListener(HandleCloseClicked);
        }

        public override void OnFocusGained()
        {
            AddListeners();
            Refresh();
        }

        private void Refresh()
        {
            var dm = DiaryManager.Instance;
            int shown = 0;
            for (int i = 0; i < chapterButtons.Length; i++)
            {
                var b = chapterButtons[i];
                if (b == null) continue;
                var entry = i < chapters.Length ? chapters[i] : null;
                bool unlocked = entry != null && dm != null && dm.IsChapterUnlocked(entry.chapter);
                b.gameObject.SetActive(unlocked);
                if (!unlocked) continue;
                shown++;
                var label = b.GetComponentInChildren<TMP_Text>();
                if (label != null) label.text = entry.ResolveTitle();
            }
            if (emptyText != null) emptyText.gameObject.SetActive(shown == 0);
        }

        private void HandleChapterClicked(int index)
        {
            if (index < 0 || index >= chapters.Length || chapters[index] == null) return;
            // Panel baca berada di atas arsip, sehingga menutupnya kembali ke arsip.
            if (DiaryManager.Instance != null) DiaryManager.Instance.OpenDiary(chapters[index]);
        }

        private void HandleCloseClicked()
        {
            if (UIFocusStack.Instance != null) UIFocusStack.Instance.Close(this, UICloseReason.User);
        }
    }
}
