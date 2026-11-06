using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SurvivalIsland
{
    /// <summary>
    /// Panel Pause: Lanjut, Pengaturan, Log, Keluar dari Tersangkut, dan Simpan dan Keluar ke Menu.
    /// Blueprint: Core_Logic/UIManager.md bagian "Panel Pause" (#83, #22, #116).
    /// Dibuka UIFocusStack.HandleEscape saat stack kosong.
    /// </summary>
    public class PausePanel : FadingUIPanel
    {
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button logButton;
        [SerializeField] private Button unstuckButton;
        [SerializeField] private Button saveQuitButton;
        [SerializeField] private TMP_Text saveQuitLabel;
        [SerializeField] private TMP_Text unstuckResultText;

        private bool listenersAdded;

        private void Awake()
        {
            AddButtonListeners();
        }

        private void AddButtonListeners()
        {
            if (listenersAdded) return;
            listenersAdded = true;
            if (resumeButton != null) resumeButton.onClick.AddListener(HandleResumeClicked);
            if (settingsButton != null) settingsButton.onClick.AddListener(HandleSettingsClicked);
            if (logButton != null) logButton.onClick.AddListener(HandleLogClicked);
            if (unstuckButton != null) unstuckButton.onClick.AddListener(HandleUnstuckClicked);
            if (saveQuitButton != null) saveQuitButton.onClick.AddListener(HandleSaveQuitClicked);
        }

        protected override void Show()
        {
            AddButtonListeners();
            base.Show();
        }

        /// <summary>Segarkan isi setiap kali Pause menjadi puncak stack.</summary>
        public override void OnFocusGained()
        {
            RefreshTexts();
            if (unstuckResultText != null) unstuckResultText.text = string.Empty;
            if (resumeButton != null && EventSystem.current != null)
                EventSystem.current.SetSelectedGameObject(resumeButton.gameObject);
        }

        public override void OnClosed(UICloseReason reason)
        {
            // Pause berikutnya mulai dari menu utama jeda.
            SetLogVisible(false);
        }

        /// <summary>Isi ulang label (juga dipanggil saat LocaleChangedEvent).</summary>
        public void RefreshTexts()
        {
            var run = RunLifecycle.Instance;
            bool onIsland = run != null && run.State == RunState.OnIsland;

            // Tombol Keluar dari Tersangkut hanya saat OnIsland (#116).
            // TODO Langkah 7 (ProgressionSafety #116 belum ditulis): ubah unstuckAvailable menjadi true.
            const bool unstuckAvailable = false;
            if (unstuckButton != null) unstuckButton.gameObject.SetActive(onIsland && unstuckAvailable);

            // Fase 1 tidak punya save (#82), jadi labelnya "Keluar ke Menu" (usulan UIManager.md, disetujui).
            var gm = GameManager.Instance;
            bool island = gm != null && gm.currentPhase == GamePhase.IslandSurvival;
            if (saveQuitLabel != null) saveQuitLabel.text = UIManager.Localize(island ? "ui.pause.save_quit" : "ui.pause.quit");
        }

        public void ClearTexts()
        {
            if (unstuckResultText != null) unstuckResultText.text = string.Empty;
            SetLogVisible(false);
        }

        private void HandleResumeClicked()
        {
            if (UIFocusStack.Instance != null) UIFocusStack.Instance.Close(this, UICloseReason.User);
        }

        private void HandleSettingsClicked()
        {
            var ui = UIManager.Instance;
            if (ui != null && ui.settingsPanel != null && UIFocusStack.Instance != null)
                UIFocusStack.Instance.Push(ui.settingsPanel);
        }

        private void HandleLogClicked()
        {
            var ui = UIManager.Instance;
            if (ui == null || ui.notificationLogView == null) return;
            // TODO (NotificationManager #22 belum ditulis): isi daftar lewat NotificationManager.Instance.GetLog(list).
            SetLogVisible(!ui.notificationLogView.activeSelf);
        }

        private void HandleUnstuckClicked()
        {
            // TODO Langkah 7: ShowConfirm(ui.unstuck.confirm), lalu ProgressionSafety.Instance.RequestUnstuck()
            // dan tampilkan ui.unstuck.<UnstuckResult> di unstuckResultText.
        }

        private void HandleSaveQuitClicked()
        {
            if (RunLifecycle.Instance != null) RunLifecycle.Instance.SaveAndQuitToMenu();
        }

        private static void SetLogVisible(bool visible)
        {
            var ui = UIManager.Instance;
            if (ui != null && ui.notificationLogView != null) ui.notificationLogView.SetActive(visible);
        }
    }
}
