using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SurvivalIsland
{
    /// <summary>
    /// Layar akhir GameOver atau WinScreen. Satu kelas untuk dua panel; id diatur di Inspector.
    /// Blueprint: Core_Logic/UIManager.md bagian "Layar akhir GameOver dan WinScreen". GDD bagian 3.
    /// </summary>
    public class EndScreenPanel : FadingUIPanel
    {
        [Tooltip("Isi panel yang fade masuk. Latar hitam (CanvasGroup panel ini) langsung tampil dengan alpha 1.")]
        [SerializeField] private CanvasGroup contentGroup;

        [SerializeField] private TMP_Text reasonText;
        [SerializeField] private TMP_Text dayText;
        [SerializeField] private TMP_Text seedText;
        [SerializeField] private TMP_Text statsText;

        [Tooltip("Ulangi dari Kapal. Kosongkan untuk WinScreen.")]
        [SerializeField] private Button retryButton;
        [SerializeField] private Button menuButton;

        private Coroutine delayRoutine;
        private Coroutine contentFadeRoutine;
        private bool listenersAdded;

        protected override bool FadeInOnShow => false;

        private void Awake()
        {
            AddButtonListeners();
        }

        private void AddButtonListeners()
        {
            if (listenersAdded) return;
            listenersAdded = true;
            if (retryButton != null) retryButton.onClick.AddListener(HandleRetryClicked);
            if (menuButton != null) menuButton.onClick.AddListener(HandleMenuClicked);
        }

        /// <summary>Isi panel. showReason = false untuk WinScreen. Dipanggil UIManager.ShowGameOver dan ShowWin.</summary>
        public void Fill(DeathReason reason, bool showReason)
        {
            if (reasonText != null)
            {
                reasonText.gameObject.SetActive(showReason);
                // Kunci gameover.<DeathReason> lewat LocalizationManager (#104). Kunci mentah kalau belum ada.
                reasonText.text = showReason ? UIManager.Localize("gameover." + reason) : string.Empty;
            }

            var gm = GameManager.Instance;
            if (dayText != null) dayText.text = gm != null ? gm.currentDay.ToString() : string.Empty;

            // Seed dan versi (#89). Baris dikosongkan kalau seed belum tersedia.
            if (seedText != null)
            {
                var seeds = RunSeedManager.Instance;
                bool hasSeed = seeds != null && !string.IsNullOrEmpty(seeds.CurrentSeedText);
                seedText.text = hasSeed ? $"Seed {seeds.CurrentSeedText} · v{Application.version}" : string.Empty;
            }

            // TODO (LocalStatsManager #57 belum ditulis): zombie dibunuh, tembakan meleset, tanda [CHEAT].
            // Sampai saat itu baris disembunyikan, sesuai aturan "kalau tidak tersedia" di UIManager.md.
            if (statsText != null)
            {
                statsText.text = string.Empty;
                statsText.gameObject.SetActive(false);
            }
        }

        /// <summary>Kosongkan teks. Dipanggil UIManager.ResetForNewRun.</summary>
        public void ClearTexts()
        {
            if (reasonText != null) reasonText.text = string.Empty;
            if (dayText != null) dayText.text = string.Empty;
            if (seedText != null) seedText.text = string.Empty;
            if (statsText != null) statsText.text = string.Empty;
        }

        protected override void Show()
        {
            AddButtonListeners();
            base.Show();
            SetButtonsInteractable(false);

            // Latar hitam langsung tampil, lalu isi panel fade masuk.
            if (contentGroup != null)
            {
                contentGroup.alpha = 0f;
                if (contentFadeRoutine != null) StopCoroutine(contentFadeRoutine);
                contentFadeRoutine = StartCoroutine(FadeContentIn());
            }
        }

        protected override void Hide()
        {
            StopDelay();
            SetButtonsInteractable(false);
            base.Hide();
        }

        public override void OnFocusGained()
        {
            // Tombol aktif setelah gameOverInputDelaySeconds waktu tak terskala, supaya tidak tertekan tanpa sengaja.
            StopDelay();
            if (isActiveAndEnabled) delayRoutine = StartCoroutine(EnableButtonsAfterDelay());
        }

        private IEnumerator FadeContentIn()
        {
            float seconds = FadeSeconds();
            if (seconds > 0f)
            {
                float speed = 1f / seconds;
                while (contentGroup.alpha < 1f)
                {
                    contentGroup.alpha = Mathf.MoveTowards(contentGroup.alpha, 1f, speed * Time.unscaledDeltaTime);
                    yield return null;
                }
            }
            contentGroup.alpha = 1f;
            contentFadeRoutine = null;
        }

        private IEnumerator EnableButtonsAfterDelay()
        {
            var balance = GameManager.CurrentBalance;
            float delay = balance != null ? balance.run.gameOverInputDelaySeconds : 0f;
            if (delay > 0f) yield return new WaitForSecondsRealtime(delay);

            SetButtonsInteractable(true);
            var first = retryButton != null ? retryButton : menuButton;
            if (first != null && EventSystem.current != null) EventSystem.current.SetSelectedGameObject(first.gameObject);
            delayRoutine = null;
        }

        private void StopDelay()
        {
            if (delayRoutine != null)
            {
                StopCoroutine(delayRoutine);
                delayRoutine = null;
            }
        }

        private void SetButtonsInteractable(bool value)
        {
            if (retryButton != null) retryButton.interactable = value;
            if (menuButton != null) menuButton.interactable = value;
        }

        private void HandleRetryClicked()
        {
            // Semua tombol langsung mati, supaya klik ganda tidak memanggil dua alur.
            SetButtonsInteractable(false);
            // GDD bagian 3: kalah mengulang dari Fase 1.
            if (RunLifecycle.Instance != null) RunLifecycle.Instance.StartNewRun();
        }

        private void HandleMenuClicked()
        {
            SetButtonsInteractable(false);
            if (RunLifecycle.Instance != null) RunLifecycle.Instance.ReturnToMainMenu();
        }
    }
}
