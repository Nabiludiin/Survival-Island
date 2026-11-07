using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace SurvivalIsland
{
    /// <summary>Nama action yang dipakai di luar InputManager (menu #19, teks tombol), supaya tidak memakai string.</summary>
    public enum GameInputAction
    {
        Move, Look, Jump, Sprint, Crouch, Interact, PrimaryAction, SecondaryAction, Reload, BuildRotate,
        HotbarSelect, HotbarScroll, Flashlight, Throw, DropHeavyItem, Struggle, Shove,
        OpenInventory, OpenCrafting, OpenBuildMenu, OpenDiary, Pause,
        Cancel, Submit, Point, Click, Skip, ToggleConsole
    }

    public enum RebindStatus { Done, Canceled, Conflict, Rejected }

    /// <summary>Hasil ganti tombol (InputManager.md).</summary>
    public struct RebindResult
    {
        public RebindStatus status;
        public GameInputAction conflictingAction;

        public RebindResult(RebindStatus status, GameInputAction conflictingAction = default)
        {
            this.status = status;
            this.conflictingAction = conflictingAction;
        }
    }

    /// <summary>
    /// Satu pintu input (Unity New Input System). Manager persisten, IBootstrapService urutan ke-2 (#82).
    /// Blueprint: Core_Logic/InputManager.md (#18). Usulan teknis; E untuk interaksi dan klik kiri untuk item aktif
    /// mengikuti keputusan #85 di GLOSSARY.md.
    ///
    /// Sistem gameplay membaca properti (tanpa garbage dan tanpa langganan). Hanya Esc yang memakai callback,
    /// diteruskan ke UIFocusStack.HandleEscape(). Kursor dan waktu milik UIFocusStack.
    ///
    /// Pengaturan proyek yang wajib di Unity (belum dilakukan): Active Input Handling = Input System Package (New),
    /// InputSettings.updateMode = ProcessEventsInDynamicUpdate, backgroundBehavior = ResetAndDisableNonBackgroundDevices.
    /// </summary>
    public class InputManager : MonoBehaviour, IBootstrapService
    {
        public const string BindingsPrefsKey = "settings.input.bindings";

        public static InputManager Instance { get; private set; }

        private GameInputActions actions;
        private InputActionRebindingExtensions.RebindingOperation rebindOperation;
        private readonly Dictionary<GameInputAction, string> displayCache = new Dictionary<GameInputAction, string>();
        private bool escapeBound;

        public GameInputActions Actions => actions;

        // ---------------------------------------------------------------- Properti baca (InputManager.md bagian 2)
        // Action yang dimatikan mengembalikan nilai kosong dan false, jadi pembaca tidak perlu mengecek menu terbuka.

        public Vector2 MoveInput => actions != null ? actions.Gameplay.Move.ReadValue<Vector2>() : Vector2.zero;
        /// <summary>Jarak mouse per frame. TIDAK dikali Time.deltaTime.</summary>
        public Vector2 LookDelta => actions != null ? actions.Gameplay.Look.ReadValue<Vector2>() : Vector2.zero;
        public bool JumpPressed => actions != null && actions.Gameplay.Jump.WasPressedThisFrame();
        public bool SprintHeld => actions != null && actions.Gameplay.Sprint.IsPressed();
        public bool CrouchHeld => actions != null && actions.Gameplay.Crouch.IsPressed();
        public bool InteractPressed => actions != null && actions.Gameplay.Interact.WasPressedThisFrame();
        public bool InteractHeld => actions != null && actions.Gameplay.Interact.IsPressed();
        public bool PrimaryPressed => actions != null && actions.Gameplay.PrimaryAction.WasPressedThisFrame();
        public bool PrimaryHeld => actions != null && actions.Gameplay.PrimaryAction.IsPressed();
        public bool PrimaryReleased => actions != null && actions.Gameplay.PrimaryAction.WasReleasedThisFrame();
        public bool SecondaryPressed => actions != null && actions.Gameplay.SecondaryAction.WasPressedThisFrame();
        public bool SecondaryHeld => actions != null && actions.Gameplay.SecondaryAction.IsPressed();
        public bool SecondaryReleased => actions != null && actions.Gameplay.SecondaryAction.WasReleasedThisFrame();
        public bool ReloadPressed => actions != null && actions.Gameplay.Reload.WasPressedThisFrame();
        public bool BuildRotatePressed => actions != null && actions.Gameplay.BuildRotate.WasPressedThisFrame();
        public bool FlashlightPressed => actions != null && actions.Gameplay.Flashlight.WasPressedThisFrame();
        public bool ThrowPressed => actions != null && actions.Gameplay.Throw.WasPressedThisFrame();
        public bool DropHeavyItemPressed => actions != null && actions.Gameplay.DropHeavyItem.WasPressedThisFrame();
        public bool StrugglePressed => actions != null && actions.Gameplay.Struggle.WasPressedThisFrame();
        /// <summary>Tombol Struggle sedang ditahan. Mode aksesibilitas holdToStruggle (#19, PlayerPinnedState.md).</summary>
        public bool StruggleHeld => actions != null && actions.Gameplay.Struggle.IsPressed();
        public bool ShovePressed => actions != null && actions.Gameplay.Shove.WasPressedThisFrame();
        public bool OpenInventoryPressed => actions != null && actions.Gameplay.OpenInventory.WasPressedThisFrame();
        public bool OpenCraftingPressed => actions != null && actions.Gameplay.OpenCrafting.WasPressedThisFrame();
        public bool OpenBuildMenuPressed => actions != null && actions.Gameplay.OpenBuildMenu.WasPressedThisFrame();
        public bool OpenDiaryPressed => actions != null && actions.Gameplay.OpenDiary.WasPressedThisFrame();
        public bool SkipPressed => actions != null && actions.UI.Skip.WasPressedThisFrame();

        /// <summary>Slot hotbar yang ditekan frame ini, 1 sampai 8. 0 kalau tidak ada.</summary>
        public int HotbarSelectPressed
        {
            get
            {
                if (actions == null || !actions.Gameplay.HotbarSelect.WasPressedThisFrame()) return 0;
                return Mathf.RoundToInt(actions.Gameplay.HotbarSelect.ReadValue<float>());
            }
        }

        /// <summary>Nilai scroll frame ini. Tidak dikali waktu (EquipmentManager.md).</summary>
        public float HotbarScroll => actions != null ? actions.Gameplay.HotbarScroll.ReadValue<float>() : 0f;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        public bool ConsoleTogglePressed => actions != null && actions.Debug.ToggleConsole.WasPressedThisFrame();
#endif

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            // IBootstrapService: Awake hanya mengisi Instance.
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning("[InputManager] Instance ganda.");
                enabled = false;
                return;
            }
            Instance = this;
        }

        private void OnEnable()
        {
            EventChannel<UIFocusChangedEvent>.Raised += HandleFocus;
            EventChannel<InputBindingsChangedEvent>.Raised += HandleBindingsChanged;
            if (actions != null)
            {
                ApplyMaps(EventChannel<UIFocusChangedEvent>.HasLast && EventChannel<UIFocusChangedEvent>.Last.isGameplayBlocked);
                BindEscape();
            }
        }

        private void OnDisable()
        {
            EventChannel<UIFocusChangedEvent>.Raised -= HandleFocus;
            EventChannel<InputBindingsChangedEvent>.Raised -= HandleBindingsChanged;
            if (actions != null)
            {
                UnbindEscape();
                actions.Gameplay.Disable();
                actions.UI.Disable();
            }
        }

        private void OnDestroy()
        {
            CancelRebind();
            actions?.Dispose();
            if (Instance == this) Instance = null;
        }

        // ---------------------------------------------------------------- IBootstrapService

        public void Initialize()
        {
            if (actions != null) return;
            actions = new GameInputActions();
            LoadBindingOverrides();
            ConnectUIModule();
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            actions.Debug.Enable();
#endif
            // OnEnable pertama terjadi sebelum actions dibuat, jadi isinya dijalankan di sini.
            ApplyMaps(EventChannel<UIFocusChangedEvent>.HasLast && EventChannel<UIFocusChangedEvent>.Last.isGameplayBlocked);
            BindEscape();
        }

        /// <summary>Membatalkan ganti tombol yang masih berjalan. Tombol pilihan pemain TIDAK direset (PlayerPrefs).</summary>
        public void ResetForNewRun()
        {
            CancelRebind();
        }

        // ---------------------------------------------------------------- Action Map

        private void HandleFocus(UIFocusChangedEvent e)
        {
            if (actions != null) ApplyMaps(e.isGameplayBlocked);
        }

        /// <summary>Hanya satu dari Gameplay dan UI yang aktif, jadi Esc tidak dibaca dua kali.</summary>
        private void ApplyMaps(bool gameplayBlocked)
        {
            if (gameplayBlocked)
            {
                actions.Gameplay.Disable();
                actions.UI.Enable();
            }
            else
            {
                actions.UI.Disable();
                actions.Gameplay.Enable();
            }
        }

        private void BindEscape()
        {
            if (escapeBound) return;
            actions.Gameplay.Pause.performed += HandleEscapePerformed;
            actions.UI.Cancel.performed += HandleEscapePerformed;
            escapeBound = true;
        }

        private void UnbindEscape()
        {
            if (!escapeBound) return;
            actions.Gameplay.Pause.performed -= HandleEscapePerformed;
            actions.UI.Cancel.performed -= HandleEscapePerformed;
            escapeBound = false;
        }

        /// <summary>Satu-satunya tempat yang meneruskan Esc.</summary>
        private void HandleEscapePerformed(InputAction.CallbackContext ctx)
        {
            if (UIFocusStack.Instance != null) UIFocusStack.Instance.HandleEscape();
        }

        /// <summary>Menghubungkan Action Map UI ke InputSystemUIInputModule milik EventSystem persisten.</summary>
        private void ConnectUIModule()
        {
            var es = EventSystem.current != null ? EventSystem.current : FindAnyObjectByType<EventSystem>();
            var module = es != null ? es.GetComponent<InputSystemUIInputModule>() : null;
            if (module == null)
            {
                Debug.LogWarning("[InputManager] InputSystemUIInputModule tidak ditemukan di EventSystem persisten.");
                return;
            }
            module.actionsAsset = actions.asset;
            module.point = InputActionReference.Create(actions.UI.Point);
            module.leftClick = InputActionReference.Create(actions.UI.Click);
            module.rightClick = InputActionReference.Create(actions.UI.RightClick);
            module.middleClick = InputActionReference.Create(actions.UI.MiddleClick);
            module.scrollWheel = InputActionReference.Create(actions.UI.ScrollWheel);
            module.move = InputActionReference.Create(actions.UI.Navigate);
            module.submit = InputActionReference.Create(actions.UI.Submit);
            module.cancel = InputActionReference.Create(actions.UI.Cancel);
        }

        // ---------------------------------------------------------------- Ganti tombol (#19)

        private static bool IsFixed(GameInputAction a)
        {
            return a == GameInputAction.Pause || a == GameInputAction.Cancel || a == GameInputAction.Submit
                || a == GameInputAction.Point || a == GameInputAction.Click || a == GameInputAction.ToggleConsole;
        }

        /// <summary>Mencari InputAction dari enum. null kalau tidak ada.</summary>
        public InputAction GetAction(GameInputAction a)
        {
            if (actions == null) return null;
            var g = actions.Gameplay;
            var u = actions.UI;
            switch (a)
            {
                case GameInputAction.Move: return g.Move;
                case GameInputAction.Look: return g.Look;
                case GameInputAction.Jump: return g.Jump;
                case GameInputAction.Sprint: return g.Sprint;
                case GameInputAction.Crouch: return g.Crouch;
                case GameInputAction.Interact: return g.Interact;
                case GameInputAction.PrimaryAction: return g.PrimaryAction;
                case GameInputAction.SecondaryAction: return g.SecondaryAction;
                case GameInputAction.Reload: return g.Reload;
                case GameInputAction.BuildRotate: return g.BuildRotate;
                case GameInputAction.HotbarSelect: return g.HotbarSelect;
                case GameInputAction.HotbarScroll: return g.HotbarScroll;
                case GameInputAction.Flashlight: return g.Flashlight;
                case GameInputAction.Throw: return g.Throw;
                case GameInputAction.DropHeavyItem: return g.DropHeavyItem;
                case GameInputAction.Struggle: return g.Struggle;
                case GameInputAction.Shove: return g.Shove;
                case GameInputAction.OpenInventory: return g.OpenInventory;
                case GameInputAction.OpenCrafting: return g.OpenCrafting;
                case GameInputAction.OpenBuildMenu: return g.OpenBuildMenu;
                case GameInputAction.OpenDiary: return g.OpenDiary;
                case GameInputAction.Pause: return g.Pause;
                case GameInputAction.Cancel: return u.Cancel;
                case GameInputAction.Submit: return u.Submit;
                case GameInputAction.Point: return u.Point;
                case GameInputAction.Click: return u.Click;
                case GameInputAction.Skip: return u.Skip;
                case GameInputAction.ToggleConsole: return actions.Debug.ToggleConsole;
                default: return null;
            }
        }

        /// <summary>Pasangan Open* di Gameplay dan Toggle* di UI diganti bersamaan.</summary>
        private InputAction GetMirrorAction(GameInputAction a)
        {
            switch (a)
            {
                case GameInputAction.OpenInventory: return actions.UI.ToggleInventory;
                case GameInputAction.OpenCrafting: return actions.UI.ToggleCrafting;
                case GameInputAction.OpenBuildMenu: return actions.UI.ToggleBuildMenu;
                case GameInputAction.OpenDiary: return actions.UI.ToggleDiary;
                default: return null;
            }
        }

        public void StartRebind(GameInputAction action, int bindingIndex, Action<RebindResult> onDone)
        {
            var target = GetAction(action);
            if (target == null || IsFixed(action) || rebindOperation != null
                || bindingIndex < 0 || bindingIndex >= target.bindings.Count)
            {
                onDone?.Invoke(new RebindResult(RebindStatus.Rejected));
                return;
            }

            bool wasEnabled = target.enabled;
            target.Disable();
            string oldOverride = target.bindings[bindingIndex].overridePath;
            float timeout = GameManager.CurrentBalance != null ? GameManager.CurrentBalance.input.rebindTimeoutSeconds : 5f;

            rebindOperation = target.PerformInteractiveRebinding(bindingIndex)
                .WithCancelingThrough("<Keyboard>/escape")
                .WithControlsExcluding("<Mouse>/position")
                .WithControlsExcluding("<Mouse>/delta")
                .WithTimeout(timeout)
                .OnComplete(op =>
                {
                    string newPath = target.bindings[bindingIndex].effectivePath;
                    if (FindConflict(action, newPath, out var other))
                    {
                        // Batalkan penggantian, menu #19 yang menanyakan pilihan pemain.
                        if (string.IsNullOrEmpty(oldOverride)) target.RemoveBindingOverride(bindingIndex);
                        else target.ApplyBindingOverride(bindingIndex, oldOverride);
                        FinishRebind(target, wasEnabled);
                        onDone?.Invoke(new RebindResult(RebindStatus.Conflict, other));
                        return;
                    }
                    var mirror = GetMirrorAction(action);
                    if (mirror != null && bindingIndex < mirror.bindings.Count) mirror.ApplyBindingOverride(bindingIndex, newPath);
                    SaveBindingOverrides();
                    FinishRebind(target, wasEnabled);
                    EventChannel<InputBindingsChangedEvent>.Publish(new InputBindingsChangedEvent());
                    onDone?.Invoke(new RebindResult(RebindStatus.Done));
                })
                .OnCancel(op =>
                {
                    FinishRebind(target, wasEnabled);
                    onDone?.Invoke(new RebindResult(RebindStatus.Canceled));
                });
            rebindOperation.Start();
        }

        private void FinishRebind(InputAction target, bool wasEnabled)
        {
            rebindOperation?.Dispose();
            rebindOperation = null;
            if (wasEnabled) target.Enable();
        }

        private void CancelRebind()
        {
            if (rebindOperation == null) return;
            rebindOperation.Cancel();
            rebindOperation?.Dispose();
            rebindOperation = null;
        }

        /// <summary>
        /// Bentrok hanya diperiksa di map yang sama. Jump-Struggle dan Reload-BuildRotate boleh berbagi tombol.
        /// </summary>
        private bool FindConflict(GameInputAction action, string path, out GameInputAction other)
        {
            other = default;
            var target = GetAction(action);
            if (target == null || string.IsNullOrEmpty(path)) return false;
            foreach (GameInputAction candidate in Enum.GetValues(typeof(GameInputAction)))
            {
                if (candidate == action || IsSharedPair(action, candidate)) continue;
                var a = GetAction(candidate);
                if (a == null || a == target || a.actionMap != target.actionMap) continue;
                var bindings = a.bindings;
                for (int i = 0; i < bindings.Count; i++)
                {
                    if (bindings[i].isComposite) continue;
                    if (string.Equals(bindings[i].effectivePath, path, StringComparison.OrdinalIgnoreCase))
                    {
                        other = candidate;
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool IsSharedPair(GameInputAction a, GameInputAction b)
        {
            return (a == GameInputAction.Jump && b == GameInputAction.Struggle) || (a == GameInputAction.Struggle && b == GameInputAction.Jump)
                || (a == GameInputAction.Reload && b == GameInputAction.BuildRotate) || (a == GameInputAction.BuildRotate && b == GameInputAction.Reload);
        }

        public void ResetBindings()
        {
            if (actions == null) return;
            actions.RemoveAllBindingOverrides();
            SaveBindingOverrides();
            EventChannel<InputBindingsChangedEvent>.Publish(new InputBindingsChangedEvent());
        }

        private void SaveBindingOverrides()
        {
            PlayerPrefs.SetString(BindingsPrefsKey, actions.SaveBindingOverridesAsJson());
            PlayerPrefs.Save();
        }

        /// <summary>JSON rusak diabaikan, tombol kembali ke nilai awal.</summary>
        private void LoadBindingOverrides()
        {
            string json = PlayerPrefs.GetString(BindingsPrefsKey, string.Empty);
            if (string.IsNullOrEmpty(json)) return;
            try
            {
                actions.LoadBindingOverridesFromJson(json);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[InputManager] Data tombol rusak, memakai tombol awal. {ex.Message}");
                actions.RemoveAllBindingOverrides();
            }
        }

        // ---------------------------------------------------------------- Teks tombol

        /// <summary>Nama tombol aktif, misalnya "E". Penulisan "Press [E]" langsung dilarang (InputManager.md).</summary>
        public string GetBindingDisplay(GameInputAction action)
        {
            if (displayCache.TryGetValue(action, out var cached)) return cached;
            var a = GetAction(action);
            string text = a != null ? a.GetBindingDisplayString() : string.Empty;
            displayCache[action] = text;
            return text;
        }

        private void HandleBindingsChanged(InputBindingsChangedEvent e)
        {
            displayCache.Clear();
        }
    }
}
