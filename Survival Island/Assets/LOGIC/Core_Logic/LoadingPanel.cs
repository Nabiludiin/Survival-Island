using UnityEngine;
using UnityEngine.UI;

namespace SurvivalIsland
{
    /// <summary>
    /// Panel Loading: bar kemajuan dan ikon berputar. Membaca AsyncSceneLoader.Progress.
    /// Blueprint: Core_Logic/UIManager.md bagian "Panel Loading" (#82, #83).
    /// Pengaturan UIFocusStack: menjeda waktu, terkunci, menolak panel baru.
    /// </summary>
    public class LoadingPanel : FadingUIPanel
    {
        [SerializeField] private Slider loadingProgressBar;
        [SerializeField] private RectTransform spinner;

        [Tooltip("Nilai tampilan di prefab, bukan angka permainan (UIManager.md).")]
        [SerializeField] private float spinnerDegreesPerSecond = 180f;

        private float displayed;
        private bool wasBusy;

        // Layar langsung tertutup tanpa fade masuk, supaya pergantian scene tidak terlihat.
        protected override bool FadeInOnShow => false;

        private void Update()
        {
            // Memakai Time.unscaledDeltaTime karena Loading menjeda waktu.
            var loader = AsyncSceneLoader.Instance;
            if (loader != null)
            {
                float target = loader.Progress;
                // Pemuatan baru dimulai: bar mulai dari 0 lagi (panel dipakai ulang antara reset dan pemuatan fase).
                if (loader.IsBusy && !wasBusy) displayed = 0f;
                wasBusy = loader.IsBusy;

                var balance = GameManager.CurrentBalance;
                if (balance != null)
                    // Mathf.Max: dalam satu pemuatan bar tidak pernah mundur.
                    displayed = Mathf.MoveTowards(displayed, Mathf.Max(displayed, target), balance.ui.loadingBarSpeed * Time.unscaledDeltaTime);
                else
                    displayed = Mathf.Max(displayed, target);

                if (loadingProgressBar != null) loadingProgressBar.value = displayed;
            }

            // Ikon tetap berputar setelah bar penuh, menutupi LoadInitialRegions (#88) yang tidak melaporkan kemajuan.
            if (spinner != null) spinner.Rotate(0f, 0f, -spinnerDegreesPerSecond * Time.unscaledDeltaTime);
        }

        /// <summary>Dipanggil UIManager.ResetForNewRun.</summary>
        public void ResetDisplay()
        {
            displayed = 0f;
            wasBusy = false;
            if (loadingProgressBar != null) loadingProgressBar.value = 0f;
        }
    }
}
