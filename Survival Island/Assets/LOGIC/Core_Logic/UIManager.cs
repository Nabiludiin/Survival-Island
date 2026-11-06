using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Pemilik panel persisten: Pause, Loading, Cinematic, GameOver, WinScreen, dan ConfirmDialog.
    /// Kelas ini mengisi panel. Waktu, kursor, dan Esc diatur UIFocusStack.
    /// Blueprint: Core_Logic/UIManager.md (#82, #83, #22, #104, #116). GDD bagian 3 (Game Over dan Menang).
    ///
    /// Urutan bootstrap ke-9. Tidak lagi menulis Time.timeScale atau kursor, dan tidak membaca Esc.
    /// </summary>
    public class UIManager : MonoBehaviour, IBootstrapService
    {
        public static UIManager Instance { get; private set; }

        [Header("Panel persisten (diisi di prefab PersistentRoot)")]
        public PausePanel pausePanel;
        public LoadingPanel loadingPanel;
        public CinematicPanel cinematicPanel;
        public EndScreenPanel gameOverPanel;
        public EndScreenPanel winPanel;
        public ConfirmDialogPanel confirmDialog;

        [Tooltip("Rujukan ke SettingsPanel milik SettingsManager (#19). Boleh kosong sampai #19 ditulis.")]
        public UIPanel settingsPanel;

        [Tooltip("Riwayat pesan di dalam Pause (#22). Bukan UIPanel sendiri. Boleh kosong sampai #22 ditulis.")]
        public GameObject notificationLogView;

        private ConfirmRequest pendingConfirm;
        private bool hasPendingConfirm;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("[UIManager] Instance ganda. Periksa prefab PersistentRoot.");
                return;
            }
            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void OnEnable()
        {
            EventChannel<LocaleChangedEvent>.Raised += HandleLocaleChanged;
        }

        private void OnDisable()
        {
            EventChannel<LocaleChangedEvent>.Raised -= HandleLocaleChanged;
        }

        // ---------------------------------------------------------------- Layanan

        public void Initialize()
        {
            // 1. Rujukan kosong dicatat sebagai kesalahan, boot tetap berlanjut.
            CheckReference(pausePanel, nameof(pausePanel));
            CheckReference(loadingPanel, nameof(loadingPanel));
            CheckReference(cinematicPanel, nameof(cinematicPanel));
            CheckReference(gameOverPanel, nameof(gameOverPanel));
            CheckReference(winPanel, nameof(winPanel));
            CheckReference(confirmDialog, nameof(confirmDialog));

            // 2-3. Semua panel tersembunyi dan Animator memakai waktu tak terskala.
            PreparePanel(pausePanel);
            PreparePanel(loadingPanel);
            PreparePanel(cinematicPanel);
            PreparePanel(gameOverPanel);
            PreparePanel(winPanel);
            PreparePanel(confirmDialog);
            if (notificationLogView != null) notificationLogView.SetActive(false);

            // 4. Tangkap prefab yang salah sambung.
            var stack = UIFocusStack.Instance;
            if (stack != null && stack.pausePanel != pausePanel)
                Debug.LogError("[UIManager] UIFocusStack.pausePanel berbeda dengan UIManager.pausePanel. Periksa prefab PersistentRoot.");
        }

        /// <summary>Dipanggil RunLifecycle langkah 10 reset. Panel sudah ditutup UIFocusStack.ClearForNewRun.</summary>
        public void ResetForNewRun()
        {
            if (gameOverPanel != null) gameOverPanel.ClearTexts();
            if (winPanel != null) winPanel.ClearTexts();
            if (pausePanel != null) pausePanel.ClearTexts();
            if (cinematicPanel != null)
            {
                cinematicPanel.SetBlack(0f);
                cinematicPanel.ClearCaption();
            }
            if (loadingPanel != null) loadingPanel.ResetDisplay();

            // Kosongkan request tanpa memanggil onResult.
            pendingConfirm = default;
            hasPendingConfirm = false;
            if (confirmDialog != null) confirmDialog.ClearContent();

            if (notificationLogView != null) notificationLogView.SetActive(false);
        }

        private static void CheckReference(Object reference, string fieldName)
        {
            if (reference == null) Debug.LogError($"[UIManager] Rujukan '{fieldName}' kosong di prefab PersistentRoot.");
        }

        private static void PreparePanel(FadingUIPanel panel)
        {
            if (panel == null) return;
            var animators = panel.GetComponentsInChildren<Animator>(true);
            for (int i = 0; i < animators.Length; i++) animators[i].updateMode = AnimatorUpdateMode.UnscaledTime;
            panel.ResetHidden();
        }

        // ---------------------------------------------------------------- Layar akhir

        /// <summary>Isi gameOverPanel. Tidak mendorong panel; RunLifecycle yang mendorongnya.</summary>
        public void ShowGameOver(DeathReason reason)
        {
            if (gameOverPanel != null) gameOverPanel.Fill(reason, true);
        }

        /// <summary>Isi winPanel tanpa penyebab.</summary>
        public void ShowWin()
        {
            if (winPanel != null) winPanel.Fill(DeathReason.None, false);
        }

        // ---------------------------------------------------------------- ConfirmDialog (usulan, disetujui developer)

        /// <summary>
        /// Buka dialog konfirmasi bersama. false kalau dialog lain masih terbuka, atau Push ditolak
        /// (mis. Loading atau Cinematic di puncak). Pemanggil wajib menangani false.
        /// </summary>
        public bool ShowConfirm(in ConfirmRequest request)
        {
            var stack = UIFocusStack.Instance;
            if (confirmDialog == null || stack == null) return false;

            if (hasPendingConfirm || stack.Contains(confirmDialog))
            {
                Debug.LogWarning("[UIManager] ShowConfirm ditolak: satu dialog berlaku pada satu waktu.");
                return false;
            }

            pendingConfirm = request;
            hasPendingConfirm = true;
            confirmDialog.SetContent(request);

            if (!stack.Push(confirmDialog))
            {
                pendingConfirm = default;
                hasPendingConfirm = false;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Dipanggil tombol Ya atau Tidak. Request dikosongkan lebih dulu (klik ganda tidak memanggil apa pun),
        /// lalu panel ditutup, lalu onResult dipanggil. onResult boleh membuka dialog baru.
        /// Catatan: blueprint menulis urutan "tutup, kosongkan, panggil". Di sini pengosongan dipindah sebelum Close,
        /// supaya OnClosed(User) dari Close tidak terbaca sebagai Esc. Hasil yang terlihat sama.
        /// </summary>
        public void ResolveConfirm(bool result)
        {
            if (!hasPendingConfirm) return;
            var request = pendingConfirm;
            pendingConfirm = default;
            hasPendingConfirm = false;

            if (UIFocusStack.Instance != null) UIFocusStack.Instance.Close(confirmDialog, UICloseReason.User);
            request.onResult?.Invoke(result);
        }

        /// <summary>Dipanggil ConfirmDialogPanel.OnClosed. Esc (User) berarti Tidak; tutup paksa tidak memanggil onResult.</summary>
        public void HandleConfirmClosed(UICloseReason reason)
        {
            if (!hasPendingConfirm) return;
            var request = pendingConfirm;
            pendingConfirm = default;
            hasPendingConfirm = false;

            if (reason == UICloseReason.User) request.onResult?.Invoke(false);
        }

        // ---------------------------------------------------------------- Teks

        /// <summary>
        /// Ambil teks dari kunci lokalisasi.
        /// TODO (LocalizationManager #104 belum ditulis): ganti dengan LocalizationManager.Get(key).
        /// Sampai saat itu kunci mentah dikembalikan, sesuai aturan #104 untuk kunci yang tidak ada.
        /// </summary>
        internal static string Localize(string key)
        {
            return key ?? string.Empty;
        }

        private void HandleLocaleChanged(LocaleChangedEvent e)
        {
            // Panel yang sedang terbuka mengisi ulang teksnya. Panel tertutup diisi ulang saat dibuka.
            if (pausePanel != null && pausePanel.IsShown) pausePanel.RefreshTexts();
            if (confirmDialog != null && confirmDialog.IsShown) confirmDialog.RefreshTexts();
        }
    }
}
