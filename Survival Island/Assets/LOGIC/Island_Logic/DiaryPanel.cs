using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SurvivalIsland
{
    /// <summary>
    /// Panel teks satu bab Diary (UIPanelId.Diary). Blueprint: Island_Logic/DiaryManager.md bagian 1 (#81, #83).
    /// Claude.md Tahap 5: membaca Diary menjeda game (Time.timeScale = 0). Jeda dan kursor diatur UIFocusStack lewat
    /// pengaturan panel di Inspector (pausesTime, releasesCursor, blocksGameplayInput, closableByEsc), bukan di sini.
    /// </summary>
    public class DiaryPanel : FadingUIPanel
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI contentText;
        [SerializeField] private Button closeButton;
        [SerializeField] private ScrollRect scroll;

        private bool listenerAdded;

        private void Awake()
        {
            AddListener();
        }

        private void AddListener()
        {
            if (listenerAdded) return;
            listenerAdded = true;
            if (closeButton != null) closeButton.onClick.AddListener(HandleCloseClicked);
        }

        public void SetEntry(DiaryEntry entry)
        {
            AddListener();
            if (entry == null) return;
            if (titleText != null) titleText.text = entry.ResolveTitle();
            if (contentText != null) contentText.text = entry.ResolveBody();
            if (scroll != null) scroll.verticalNormalizedPosition = 1f;
        }

        private void HandleCloseClicked()
        {
            // Tombol Close memakai jalur UIFocusStack, sama dengan Esc.
            if (UIFocusStack.Instance != null) UIFocusStack.Instance.Close(this, UICloseReason.User);
        }
    }
}
