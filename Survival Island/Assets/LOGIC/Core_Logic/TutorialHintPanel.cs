using TMPro;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Kotak teks pop-up panduan di canvas persisten, di atas HUD dan di bawah panel menjeda.
    /// Bukan UIPanel: tidak masuk UIFocusStack, tidak mengambil kursor atau input, dan tidak menangkap klik.
    /// Blueprint: Core_Logic/TutorialManager.md (#16), bagian "Tampilan".
    /// </summary>
    public class TutorialHintPanel : MonoBehaviour
    {
        [SerializeField] private CanvasGroup group;
        [SerializeField] private TMP_Text text;

        private bool isShown;
        private bool isSuspended;

        private void Awake()
        {
            if (group != null)
            {
                group.blocksRaycasts = false;   // Inventori di bawahnya tetap bisa dipakai
                group.interactable = false;
            }
            if (text != null) text.raycastTarget = false;
            Apply();
        }

        public void Show(string message)
        {
            if (text != null) text.text = message;
            isShown = true;
            Apply();
        }

        /// <summary>Ganti teks yang sedang tampil (InputBindingsChangedEvent).</summary>
        public void SetText(string message)
        {
            if (text != null) text.text = message;
        }

        public void Hide()
        {
            isShown = false;
            Apply();
        }

        /// <summary>Sembunyi sementara saat game dijeda. Sisa waktu disimpan TutorialManager.</summary>
        public void SetSuspended(bool suspended)
        {
            isSuspended = suspended;
            Apply();
        }

        private void Apply()
        {
            bool visible = isShown && !isSuspended;
            if (group != null) group.alpha = visible ? 1f : 0f;
            else gameObject.SetActive(visible);
        }
    }
}
