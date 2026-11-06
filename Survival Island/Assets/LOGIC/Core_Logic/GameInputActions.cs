using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SurvivalIsland
{
    /// <summary>
    /// Semua Action Map game. Blueprint: Core_Logic/InputManager.md bagian 1 (#18).
    ///
    /// Catatan implementasi (Langkah 3): blueprint menyebut kelas hasil generate Unity dari aset GameInput.inputactions.
    /// Aset itu belum bisa dibuat tanpa membuka Unity, jadi kelas ini dibuat tangan dengan bentuk yang sama
    /// (actions.Gameplay.Move, actions.UI.Cancel, actions.Debug.ToggleConsole) dan membangun InputActionAsset lewat kode.
    /// Kalau nanti aset .inputactions dibuat dan kelasnya di-generate dengan nama yang sama, hapus file ini.
    /// Tombol awal mengikuti tabel Action Map (usulan disetujui #85). Skema hanya KeyboardMouse.
    /// </summary>
    public sealed class GameInputActions : IDisposable
    {
        public const string KeyboardMouseScheme = "KeyboardMouse";

        public InputActionAsset asset { get; }
        public GameplayActions Gameplay { get; }
        public UIActions UI { get; }
        public DebugActions Debug { get; }

        public GameInputActions()
        {
            asset = ScriptableObject.CreateInstance<InputActionAsset>();
            asset.name = "GameInput";
            asset.AddControlScheme(KeyboardMouseScheme).WithRequiredDevice("<Keyboard>").WithRequiredDevice("<Mouse>");

            Gameplay = new GameplayActions(asset.AddActionMap("Gameplay"));
            UI = new UIActions(asset.AddActionMap("UI"));
            Debug = new DebugActions(asset.AddActionMap("Debug"));
        }

        public void Dispose()
        {
            if (asset != null) UnityEngine.Object.Destroy(asset);
        }

        public void RemoveAllBindingOverrides() => asset.RemoveAllBindingOverrides();
        public string SaveBindingOverridesAsJson() => asset.SaveBindingOverridesAsJson();
        public void LoadBindingOverridesFromJson(string json) => asset.LoadBindingOverridesFromJson(json);

        private static InputAction Button(InputActionMap map, string name, params string[] paths)
        {
            var a = map.AddAction(name, InputActionType.Button);
            for (int i = 0; i < paths.Length; i++) a.AddBinding(paths[i], groups: KeyboardMouseScheme);
            return a;
        }

        private static InputAction Value(InputActionMap map, string name, string controlType, string path)
        {
            var a = map.AddAction(name, InputActionType.Value, expectedControlLayout: controlType);
            if (!string.IsNullOrEmpty(path)) a.AddBinding(path, groups: KeyboardMouseScheme);
            return a;
        }

        private static InputAction PassThrough(InputActionMap map, string name, string controlType, string path)
        {
            var a = map.AddAction(name, InputActionType.PassThrough, expectedControlLayout: controlType);
            a.AddBinding(path, groups: KeyboardMouseScheme);
            return a;
        }

        // ---------------------------------------------------------------- Gameplay

        public sealed class GameplayActions
        {
            public readonly InputActionMap map;
            public readonly InputAction Move, Look, Jump, Sprint, Crouch, Interact, PrimaryAction, SecondaryAction,
                Reload, BuildRotate, HotbarSelect, HotbarScroll, Flashlight, Throw, DropHeavyItem, Struggle, Shove,
                OpenInventory, OpenCrafting, OpenBuildMenu, OpenDiary, Pause;

            public GameplayActions(InputActionMap m)
            {
                map = m;
                Move = m.AddAction("Move", InputActionType.Value, expectedControlLayout: "Vector2");
                Move.AddCompositeBinding("2DVector")
                    .With("Up", "<Keyboard>/w", KeyboardMouseScheme)
                    .With("Down", "<Keyboard>/s", KeyboardMouseScheme)
                    .With("Left", "<Keyboard>/a", KeyboardMouseScheme)
                    .With("Right", "<Keyboard>/d", KeyboardMouseScheme);
                Look = PassThrough(m, "Look", "Vector2", "<Mouse>/delta");
                Jump = Button(m, "Jump", "<Keyboard>/space");
                Sprint = Button(m, "Sprint", "<Keyboard>/leftShift");
                Crouch = Button(m, "Crouch", "<Keyboard>/leftCtrl");
                Interact = Button(m, "Interact", "<Keyboard>/e");          // Keputusan #85: E untuk interaksi
                PrimaryAction = Button(m, "PrimaryAction", "<Mouse>/leftButton");   // Keputusan #85: klik kiri item aktif
                SecondaryAction = Button(m, "SecondaryAction", "<Mouse>/rightButton");
                Reload = Button(m, "Reload", "<Keyboard>/r");
                BuildRotate = Button(m, "BuildRotate", "<Keyboard>/r");

                // HotbarSelect: tombol 1..8 memberi nilai 1..8 lewat processor scale (Value, dibaca sebagai angka).
                HotbarSelect = m.AddAction("HotbarSelect", InputActionType.Value, expectedControlLayout: "Axis");
                for (int i = 1; i <= 8; i++)
                    HotbarSelect.AddBinding("<Keyboard>/" + i, processors: "scale(factor=" + i + ")", groups: KeyboardMouseScheme);

                HotbarScroll = PassThrough(m, "HotbarScroll", "Axis", "<Mouse>/scroll/y");
                Flashlight = Button(m, "Flashlight", "<Keyboard>/f");
                Throw = Button(m, "Throw", "<Keyboard>/g");
                DropHeavyItem = Button(m, "DropHeavyItem", "<Keyboard>/q");
                Struggle = Button(m, "Struggle", "<Keyboard>/space");
                Shove = Button(m, "Shove", "<Keyboard>/v");
                OpenInventory = Button(m, "OpenInventory", "<Keyboard>/tab");
                OpenCrafting = Button(m, "OpenCrafting", "<Keyboard>/c");
                OpenBuildMenu = Button(m, "OpenBuildMenu", "<Keyboard>/b");
                OpenDiary = Button(m, "OpenDiary", "<Keyboard>/j");
                Pause = Button(m, "Pause", "<Keyboard>/escape");
            }

            public void Enable() => map.Enable();
            public void Disable() => map.Disable();
            public bool enabled => map.enabled;
        }

        // ---------------------------------------------------------------- UI

        public sealed class UIActions
        {
            public readonly InputActionMap map;
            public readonly InputAction Point, Click, RightClick, MiddleClick, ScrollWheel, Navigate, Submit, Cancel,
                ToggleInventory, ToggleCrafting, ToggleBuildMenu, ToggleDiary, Skip;

            public UIActions(InputActionMap m)
            {
                map = m;
                Point = PassThrough(m, "Point", "Vector2", "<Mouse>/position");
                Click = PassThrough(m, "Click", "Button", "<Mouse>/leftButton");
                RightClick = PassThrough(m, "RightClick", "Button", "<Mouse>/rightButton");
                MiddleClick = PassThrough(m, "MiddleClick", "Button", "<Mouse>/middleButton");
                ScrollWheel = PassThrough(m, "ScrollWheel", "Vector2", "<Mouse>/scroll");
                Navigate = m.AddAction("Navigate", InputActionType.PassThrough, expectedControlLayout: "Vector2");
                Navigate.AddCompositeBinding("2DVector")
                    .With("Up", "<Keyboard>/upArrow", KeyboardMouseScheme)
                    .With("Down", "<Keyboard>/downArrow", KeyboardMouseScheme)
                    .With("Left", "<Keyboard>/leftArrow", KeyboardMouseScheme)
                    .With("Right", "<Keyboard>/rightArrow", KeyboardMouseScheme);
                Submit = Button(m, "Submit", "<Keyboard>/enter");
                Cancel = Button(m, "Cancel", "<Keyboard>/escape");
                ToggleInventory = Button(m, "ToggleInventory", "<Keyboard>/tab");
                ToggleCrafting = Button(m, "ToggleCrafting", "<Keyboard>/c");
                ToggleBuildMenu = Button(m, "ToggleBuildMenu", "<Keyboard>/b");
                ToggleDiary = Button(m, "ToggleDiary", "<Keyboard>/j");
                Skip = Button(m, "Skip", "<Keyboard>/space");
            }

            public void Enable() => map.Enable();
            public void Disable() => map.Disable();
            public bool enabled => map.enabled;
        }

        // ---------------------------------------------------------------- Debug

        public sealed class DebugActions
        {
            public readonly InputActionMap map;
            public readonly InputAction ToggleConsole;

            public DebugActions(InputActionMap m)
            {
                map = m;
                ToggleConsole = Button(m, "ToggleConsole", "<Keyboard>/backquote");   // ` atau ~ (#56)
            }

            public void Enable() => map.Enable();
            public void Disable() => map.Disable();
        }
    }
}
