using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SurvivalIsland
{
    /// <summary>
    /// Panel Statistik di menu utama (UIPanelId.Stats), dibuka MainMenuController lewat UIFocusStack.Push.
    /// Menampilkan jumlah run, menang, hari terlama, zombie dibunuh, akurasi, dan penyebab kematian terbanyak.
    /// Blueprint: Core_Logic/LocalStatsManager.md (#57), bagian "Fungsi baca dan tampilan".
    /// </summary>
    public class StatsPanel : FadingUIPanel
    {
        [SerializeField] private TMP_Text runsText;
        [SerializeField] private TMP_Text winsText;
        [SerializeField] private TMP_Text longestDayText;
        [SerializeField] private TMP_Text zombiesText;
        [SerializeField] private TMP_Text accuracyText;
        [SerializeField] private TMP_Text topDeathText;
        [SerializeField] private Button resetButton;
        [SerializeField] private Button closeButton;

        [Header("Kunci teks (#104)")]
        [SerializeField] private string resetTitleKey = "ui.stats.reset_title";
        [SerializeField] private string resetBodyKey = "ui.stats.reset_body";
        [SerializeField] private string noDeathKey = "ui.stats.no_death";
        [Tooltip("Awalan kunci penyebab kematian, sama dengan layar GameOver (gameover.Frostbite).")]
        [SerializeField] private string deathKeyPrefix = "gameover.";

        private void Awake()
        {
            if (resetButton != null) resetButton.onClick.AddListener(HandleResetClicked);
            if (closeButton != null) closeButton.onClick.AddListener(HandleCloseClicked);
        }

        protected override void Show()
        {
            base.Show();
            Refresh();
        }

        /// <summary>Isi ulang semua angka dari LocalStatsManager.Data.</summary>
        public void Refresh()
        {
            var stats = LocalStatsManager.Instance;
            if (stats == null) return;
            var d = stats.Data;
            SetNumber(runsText, d.totalRuns);
            SetNumber(winsText, d.totalWins);
            SetNumber(longestDayText, d.longestDaySurvived);
            SetNumber(zombiesText, d.totalZombiesKilled);
            if (accuracyText != null) accuracyText.SetText("{0:0}%", stats.GetAccuracy() * 100f);
            if (topDeathText != null)
            {
                var top = stats.GetTopDeathReason();
                topDeathText.text = UIManager.Localize(top.HasValue ? deathKeyPrefix + top.Value : noDeathKey);
            }
        }

        private static void SetNumber(TMP_Text t, int value)
        {
            if (t != null) t.SetText("{0}", value);
        }

        private void HandleResetClicked()
        {
            // Usulan 4 (#57): hapus statistik lewat ConfirmDialog.
            var ui = UIManager.Instance;
            if (ui == null) return;
            ui.ShowConfirm(new ConfirmRequest
            {
                titleKey = resetTitleKey,
                bodyKey = resetBodyKey,
                onResult = HandleResetConfirmed
            });
        }

        private void HandleResetConfirmed(bool yes)
        {
            if (!yes || LocalStatsManager.Instance == null) return;
            LocalStatsManager.Instance.ResetAllStats();
            Refresh();
        }

        private void HandleCloseClicked()
        {
            if (UIFocusStack.Instance != null) UIFocusStack.Instance.Close(this, UICloseReason.User);
        }
    }
}
