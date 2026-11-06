using UnityEngine;
using UnityEngine.UI;

namespace SurvivalIsland
{
    /// <summary>
    /// Pengatur tombol menu utama di scene MainMenu. Mendorong panel MainMenu ke UIFocusStack saat scene aktif.
    /// Blueprint: Core_Logic/RunLifecycle.md bagian "Menu utama" (#82).
    ///
    /// Belum di Langkah 1: tombol Pengaturan (SettingsManager #19) dan Statistik (LocalStatsManager #57) hanya disambung
    /// kalau panelnya diisi di Inspector. Pesan save rusak/versi (WorldStateSave.LastProblem) masih lewat log.
    /// </summary>
    public sealed class MainMenuController : MonoBehaviour
    {
        [Tooltip("Panel MainMenu (FadingUIPanel dengan id MainMenu).")]
        [SerializeField] private UIPanel menuPanel;

        [SerializeField] private Button continueButton;
        [SerializeField] private Button newRunButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button statsButton;
        [SerializeField] private Button quitButton;

        [Tooltip("SettingsPanel milik #19. Boleh kosong sampai #19 ditulis.")]
        [SerializeField] private UIPanel settingsPanel;

        [Tooltip("StatsPanel milik #57. Boleh kosong sampai #57 ditulis.")]
        [SerializeField] private UIPanel statsPanel;

        private bool menuPushed;

        private void Awake()
        {
            if (continueButton != null) continueButton.onClick.AddListener(HandleContinueClicked);
            if (newRunButton != null) newRunButton.onClick.AddListener(HandleNewRunClicked);
            if (settingsButton != null) settingsButton.onClick.AddListener(HandleSettingsClicked);
            if (statsButton != null) statsButton.onClick.AddListener(HandleStatsClicked);
            if (quitButton != null) quitButton.onClick.AddListener(HandleQuitClicked);
        }

        private void Start()
        {
            var save = WorldStateSave.Instance;
            bool canContinue = save != null && save.HasContinuableSave();
            if (continueButton != null) continueButton.gameObject.SetActive(canContinue);
            if (save != null && save.LastProblem != SaveProblem.None)
                // TODO (#104/#22): tampilkan pesan save rusak atau versi tidak cocok di layar.
                Debug.LogWarning($"[MainMenuController] Save tidak bisa dilanjutkan: {save.LastProblem}.");

            if (settingsButton != null) settingsButton.interactable = settingsPanel != null;
            if (statsButton != null) statsButton.interactable = statsPanel != null;

        }

        private void Update()
        {
            // Panel MainMenu didorong setelah RunLifecycle menutup Loading (State = InMenu). Selama Loading di puncak,
            // Push ditolak karena rejectsNewPanels, jadi menunggu state lebih aman daripada mendorong di Start.
            if (menuPushed || menuPanel == null) return;
            var run = RunLifecycle.Instance;
            var stack = UIFocusStack.Instance;
            if (run == null || stack == null || run.State != RunState.InMenu) return;
            menuPushed = stack.Push(menuPanel) || stack.Contains(menuPanel);
        }

        private void HandleContinueClicked()
        {
            if (RunLifecycle.Instance != null) RunLifecycle.Instance.ContinueRun();
        }

        private void HandleNewRunClicked()
        {
            var save = WorldStateSave.Instance;
            bool hasSave = save != null && save.HasContinuableSave();
            if (hasSave && UIManager.Instance != null)
            {
                // Ada save: konfirmasi bahwa run lama akan hilang.
                var request = new ConfirmRequest
                {
                    titleKey = "ui.menu.new_run.title",
                    bodyKey = "ui.menu.new_run.overwrite",
                    onResult = HandleNewRunConfirmed
                };
                if (UIManager.Instance.ShowConfirm(request)) return;
            }
            StartNewRun();
        }

        private void HandleNewRunConfirmed(bool confirmed)
        {
            if (confirmed) StartNewRun();
        }

        private static void StartNewRun()
        {
            if (RunLifecycle.Instance != null) RunLifecycle.Instance.StartNewRun();
        }

        private void HandleSettingsClicked()
        {
            if (settingsPanel != null && UIFocusStack.Instance != null) UIFocusStack.Instance.Push(settingsPanel);
        }

        private void HandleStatsClicked()
        {
            if (statsPanel != null && UIFocusStack.Instance != null) UIFocusStack.Instance.Push(statsPanel);
        }

        private void HandleQuitClicked()
        {
            Application.Quit();
        }
    }
}
