using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Satu pengatur fokus UI, tombol Esc, kursor, audio jeda, dan Time.timeScale.
    /// Blueprint: Core_Logic/UIFocusStack.md (#83). Jeda saat Diary mengikuti Claude.md Tahap 5.
    ///
    /// Urutan bootstrap ke-8. ApplyState adalah SATU-SATUNYA penulis Time.timeScale, Cursor.lockState,
    /// Cursor.visible, dan AudioListener.pause di seluruh proyek.
    /// </summary>
    public class UIFocusStack : MonoBehaviour, IBootstrapService
    {
        public static UIFocusStack Instance { get; private set; }

        [Tooltip("Panel yang dibuka Esc saat stack kosong. Objek yang sama dengan UIManager.pausePanel.")]
        public UIPanel pausePanel;

        private readonly List<UIPanel> stack = new List<UIPanel>(8);
        private float gameplayTimeScale = 1f;
        private int openedFrame = -1;
        private int lastEscapeFrame = -1;

        /// <summary>Ada panel yang menjeda.</summary>
        public bool IsPaused { get; private set; }

        /// <summary>Ada panel yang memblokir input gameplay.</summary>
        public bool IsGameplayInputBlocked { get; private set; }

        /// <summary>Puncak stack, atau null.</summary>
        public UIPanel TopPanel => stack.Count > 0 ? stack[stack.Count - 1] : null;

        public int Count => stack.Count;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("[UIFocusStack] Instance ganda. Periksa prefab PersistentRoot.");
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
            EventChannel<PlayerPinnedEvent>.Raised += HandlePlayerPinned;
            // GameEventRegistry.ClearAll() (#82) menghapus nilai sticky, jadi status saat ini diterbitkan ulang.
            EventChannel<UIFocusChangedEvent>.Publish(new UIFocusChangedEvent(IsGameplayInputBlocked, IsPaused));
        }

        private void OnDisable()
        {
            EventChannel<PlayerPinnedEvent>.Raised -= HandlePlayerPinned;
        }

        public void Initialize()
        {
            stack.Clear();
            gameplayTimeScale = 1f;
            ApplyState();
        }

        public void ResetForNewRun()
        {
            // Stack sudah dibersihkan ClearForNewRun di awal reset (#82). Tidak ada pekerjaan lain.
        }

        // ---------------------------------------------------------------- Fungsi utama

        public bool Contains(UIPanel panel) => panel != null && stack.Contains(panel);

        /// <summary>Buka panel di puncak stack. false kalau sudah terbuka atau ditolak puncak saat ini.</summary>
        public bool Push(UIPanel panel)
        {
            if (panel == null) return false;

            // 1. Panel yang sama tidak dibuka dua kali.
            if (stack.Contains(panel)) return false;

            // 2. Puncak yang menolak panel baru (Loading, Cinematic, GameOver, WinScreen).
            var top = TopPanel;
            if (top != null && top.settings.rejectsNewPanels && !IsDevConsoleException(panel))
                return false;

            // 3-4.
            if (top != null) top.OnFocusLost();
            stack.Add(panel);
            openedFrame = Time.frameCount;
            panel.InvokeShow();
            panel.OnFocusGained();

            // 5. TutorialManager (#16) mendengar panel yang dibuka.
            EventChannel<UIPanelOpenedEvent>.Publish(new UIPanelOpenedEvent(panel.id));

            // 6.
            ApplyState();
            return true;
        }

        private static bool IsDevConsoleException(UIPanel panel)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            // Satu-satunya pengecualian rejectsNewPanels, hanya di build pengembangan.
            return panel.id == UIPanelId.DevConsole;
#else
            return false;
#endif
        }

        /// <summary>Tutup panel di mana pun posisinya di stack.</summary>
        public bool Close(UIPanel panel, UICloseReason reason)
        {
            int index = panel == null ? -1 : stack.IndexOf(panel);
            if (index < 0) return false;

            bool wasTop = index == stack.Count - 1;
            stack.RemoveAt(index);

            panel.InvokeHide();
            panel.OnClosed(reason);

            // Puncak baru hanya menerima OnFocusGained kalau yang ditutup memang puncak.
            if (wasTop && stack.Count > 0) TopPanel.OnFocusGained();

            ApplyState();
            return true;
        }

        /// <summary>Tutup panel teratas. false kalau stack kosong.</summary>
        public bool Pop(UICloseReason reason = UICloseReason.User)
        {
            return stack.Count > 0 && Close(TopPanel, reason);
        }

        /// <summary>Tutup semua panel yang tidak terkunci, dari puncak ke bawah.</summary>
        public void CloseAllUnlocked(UICloseReason reason)
        {
            for (int i = stack.Count - 1; i >= 0; i--)
            {
                if (i >= stack.Count) continue;   // OnClosed panel lain bisa menutup panel lagi
                var panel = stack[i];
                if (!panel.settings.isLocked) Close(panel, reason);
            }
        }

        /// <summary>Tutup semua panel termasuk yang terkunci. Hanya dipanggil RunLifecycle (#82).</summary>
        public void ClearForNewRun()
        {
            for (int i = stack.Count - 1; i >= 0; i--)
            {
                if (i >= stack.Count) continue;
                Close(stack[i], UICloseReason.Forced);
            }
            gameplayTimeScale = 1f;
            ApplyState();
        }

        /// <summary>Jalur satu-satunya untuk gerak lambat (#53) dan perintah konsol (#56). Hanya berlaku saat tidak dijeda.</summary>
        public void SetGameplayTimeScale(float value)
        {
            gameplayTimeScale = Mathf.Max(0f, value);
            ApplyState();
        }

        // ---------------------------------------------------------------- Esc

        /// <summary>
        /// Dipanggil InputManager (#18) dari Gameplay/Pause dan UI/Cancel. Esc hanya menutup panel teratas.
        /// </summary>
        public void HandleEscape()
        {
            if (Time.frameCount == lastEscapeFrame) return;
            lastEscapeFrame = Time.frameCount;

            if (stack.Count == 0)
            {
                var gm = GameManager.Instance;
                bool runEnding = gm != null && gm.IsRunEnding;
                if (!runEnding && pausePanel != null) Push(pausePanel);
                return;
            }

            var top = TopPanel;
            if (top.settings.closableByEsc) Close(top, UICloseReason.User);
        }

        private void Update()
        {
            // Tombol buka dan tutup yang sama (mis. Tab): UI/Toggle... dibaca di sini.
            var top = TopPanel;
            if (top == null || top.toggleAction == null || top.toggleAction.action == null) return;
            if (Time.frameCount != openedFrame && top.toggleAction.action.WasPressedThisFrame())
                Close(top, UICloseReason.User);
        }

        // ---------------------------------------------------------------- Status

        private void ApplyState()
        {
            bool paused = false, blocked = false, cursorFree = false;
            for (int i = 0; i < stack.Count; i++)
            {
                var s = stack[i].settings;
                paused |= s.pausesTime;
                blocked |= s.blocksGameplayInput;
                cursorFree |= s.releasesCursor;
            }

            // Dihitung dari seluruh isi stack, jadi menutup Diary saat Pause masih terbuka tidak menjalankan waktu.
            Time.timeScale = paused ? 0f : gameplayTimeScale;
            Cursor.lockState = cursorFree ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = cursorFree;
            AudioListener.pause = paused;
            // TODO (AudioManager belum ditulis, bukan Langkah 1): AudioManager.Instance.SetPausedMix(paused);

            bool changed = paused != IsPaused || blocked != IsGameplayInputBlocked;
            IsPaused = paused;
            IsGameplayInputBlocked = blocked;
            if (changed) EventChannel<UIFocusChangedEvent>.Publish(new UIFocusChangedEvent(blocked, paused));
        }

        private void HandlePlayerPinned(PlayerPinnedEvent e)
        {
            // Pemain diterkam: panel non-locked ditutup supaya input gameplay kembali untuk meronta (#110).
            if (e.isPinned) CloseAllUnlocked(UICloseReason.Forced);
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            // Keputusan developer (#85, usulan 4 UIFocusStack.md): game membuka Pause saat jendela kehilangan fokus.
            if (!hasFocus)
            {
                var run = RunLifecycle.Instance;
                bool playing = run != null && (run.State == RunState.InShip || run.State == RunState.OnIsland);
                var gm = GameManager.Instance;
                bool runEnding = gm != null && gm.IsRunEnding;
                if (stack.Count == 0 && playing && !runEnding && pausePanel != null) Push(pausePanel);
            }
            else
            {
                ApplyState();
            }
        }
    }
}
