using System.Collections;
using TMPro;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Panel Cinematic: lapisan hitam penuh layar dan satu teks tengah. Layar untuk DeathCinematic (#53)
    /// dan adegan NarrativeSequencer (#99). Blueprint: Core_Logic/UIManager.md bagian "Panel Cinematic".
    /// </summary>
    public class CinematicPanel : FadingUIPanel
    {
        [SerializeField] private CanvasGroup blackOverlay;
        [SerializeField] private TMP_Text captionText;

        // Fade baru menghentikan fade lama: setiap fade menaikkan token, fade lama berhenti saat token berubah.
        private int fadeToken;

        protected override bool FadeInOnShow => false;
        protected override bool FadeOutOnHide => false;

        protected override void Show()
        {
            base.Show();
            // Dunia tetap terlihat sampai lapisan hitam dinaikkan.
            SetBlack(0f);
            ClearCaption();
        }

        /// <summary>Naikkan lapisan hitam ke 1 dengan waktu tak terskala. Dipakai DeathCinematic langkah 8 dan #99.</summary>
        public IEnumerator FadeToBlack(float seconds) => FadeOverlay(1f, seconds);

        /// <summary>Turunkan lapisan hitam ke 0 (#99).</summary>
        public IEnumerator FadeFromBlack(float seconds) => FadeOverlay(0f, seconds);

        /// <summary>Atur lapisan hitam langsung tanpa fade.</summary>
        public void SetBlack(float alpha)
        {
            fadeToken++;
            if (blackOverlay != null) blackOverlay.alpha = Mathf.Clamp01(alpha);
        }

        /// <summary>Teks tengah dari kunci lokalisasi (#104).</summary>
        public void ShowCaption(string key)
        {
            if (captionText != null) captionText.text = UIManager.Localize(key);
        }

        public void ClearCaption()
        {
            if (captionText != null) captionText.text = string.Empty;
        }

        private IEnumerator FadeOverlay(float target, float seconds)
        {
            int token = ++fadeToken;
            if (blackOverlay == null) yield break;

            if (seconds <= 0f)
            {
                blackOverlay.alpha = target;
                yield break;
            }

            float speed = 1f / seconds;
            while (token == fadeToken && !Mathf.Approximately(blackOverlay.alpha, target))
            {
                blackOverlay.alpha = Mathf.MoveTowards(blackOverlay.alpha, target, speed * Time.unscaledDeltaTime);
                yield return null;
            }
            if (token == fadeToken) blackOverlay.alpha = target;
        }
    }
}
