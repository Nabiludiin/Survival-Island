using System.IO;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SurvivalIsland.EditorTools
{
    /// <summary>
    /// Penyiapan Unity untuk Langkah 1 (LOGIC/README.md, "Penyiapan di Unity untuk Langkah 1"):
    /// aset GameBalanceConfig, prefab PersistentRoot (RunLifecycle.md, "Susunan scene Bootstrap"),
    /// scene Bootstrap, dan scene MainMenu. Panel memakai UI polos sebagai pengganti sementara.
    /// Tampilan akhir panel belum dirancang.
    /// Menu boleh dijalankan ulang: aset yang ada ditimpa, scene diisi ulang.
    /// </summary>
    public static class Langkah1Setup
    {
        private const string DataFolder = "Assets/Data";
        private const string PrefabFolder = "Assets/Prefabs";
        private const string ConfigPath = DataFolder + "/GameBalanceConfig.asset";
        private const string PersistentRootPath = PrefabFolder + "/PersistentRoot.prefab";
        private const string BootstrapScenePath = "Assets/Scenes/Bootstrap.unity";
        private const string MainMenuScenePath = "Assets/Scenes/MainMenu.unity";

        [MenuItem("Survival Island/Setup/TMP Essential Resources")]
        public static void ImportTmpEssentials()
        {
            TMP_PackageResourceImporter.ImportResources(true, false, false);
        }

        [MenuItem("Survival Island/Setup/Langkah 1 - Fondasi")]
        public static void Run()
        {
            if (TMP_Settings.defaultFontAsset == null)
            {
                Debug.LogError("[Langkah1Setup] TMP Essential Resources belum diimpor. Jalankan menu 'TMP Essential Resources' dulu.");
                return;
            }

            EnsureFolder(DataFolder);
            EnsureFolder(PrefabFolder);

            var config = CreateOrLoadConfig();
            var rootPrefab = BuildPersistentRoot(config);
            BuildBootstrapScene(rootPrefab);
            BuildMainMenuScene();

            AssetDatabase.SaveAssets();
            Debug.Log("[Langkah1Setup] Selesai: GameBalanceConfig, PersistentRoot, Bootstrap, dan MainMenu.");
        }

        // ---------------------------------------------------------------- Aset

        private static GameBalanceConfig CreateOrLoadConfig()
        {
            var config = AssetDatabase.LoadAssetAtPath<GameBalanceConfig>(ConfigPath);
            if (config != null) return config;
            config = ScriptableObject.CreateInstance<GameBalanceConfig>();
            AssetDatabase.CreateAsset(config, ConfigPath);
            return config;
        }

        // ---------------------------------------------------------------- PersistentRoot

        private static GameObject BuildPersistentRoot(GameBalanceConfig config)
        {
            var root = new GameObject("PersistentRoot");
            var lifecycle = root.AddComponent<RunLifecycle>();

            var managers = new GameObject("Managers");
            managers.transform.SetParent(root.transform, false);

            // Urutan bootstrap mengikuti tabel di RunLifecycle.md. Manager yang belum ditulis
            // (SettingsManager, AudioManager, LocalizationManager, dan lainnya) dilewati.
            var gameManager = AddManager<GameManager>(managers, "GameManager");
            var input = AddManager<InputManager>(managers, "InputManager");
            var focus = AddManager<UIFocusStack>(managers, "UIFocusStack");
            var ui = AddManager<UIManager>(managers, "UIManager");
            var pooler = AddManager<ObjectPooler>(managers, "ObjectPooler");
            var surface = AddManager<SurfaceDetection>(managers, "SurfaceDetection");
            var loader = AddManager<AsyncSceneLoader>(managers, "AsyncSceneLoader");
            var seed = AddManager<RunSeedManager>(managers, "RunSeedManager");
            var save = AddManager<WorldStateSave>(managers, "WorldStateSave");
            Set(gameManager, "balanceAsset", config);

            var canvas = CreateCanvas(managers.transform, "PersistentCanvas", 100);
            CreateEventSystem(managers.transform);

            var pause = BuildPausePanel(canvas.transform);
            var loading = BuildLoadingPanel(canvas.transform);
            var cinematic = BuildCinematicPanel(canvas.transform);
            var gameOver = BuildEndScreenPanel(canvas.transform, "GameOverPanel", UIPanelId.GameOver, "GAME OVER");
            var win = BuildEndScreenPanel(canvas.transform, "WinPanel", UIPanelId.WinScreen, "SELAMAT");
            var confirm = BuildConfirmDialog(canvas.transform);

            ui.pausePanel = pause;
            ui.loadingPanel = loading;
            ui.cinematicPanel = cinematic;
            ui.gameOverPanel = gameOver;
            ui.winPanel = win;
            ui.confirmDialog = confirm;
            focus.pausePanel = pause;

            var so = new SerializedObject(lifecycle);
            var services = so.FindProperty("orderedServices");
            MonoBehaviour[] ordered = { gameManager, input, focus, ui, pooler, surface, loader, seed, save };
            services.arraySize = ordered.Length;
            for (int i = 0; i < ordered.Length; i++) services.GetArrayElementAtIndex(i).objectReferenceValue = ordered[i];
            so.FindProperty("managersRoot").objectReferenceValue = managers;
            so.FindProperty("cinematicPanel").objectReferenceValue = cinematic;
            so.FindProperty("gameOverPanel").objectReferenceValue = gameOver;
            so.FindProperty("winPanel").objectReferenceValue = win;
            so.FindProperty("loadingPanel").objectReferenceValue = loading;
            so.ApplyModifiedPropertiesWithoutUndo();

            var prefab = PrefabUtility.SaveAsPrefabAsset(root, PersistentRootPath);
            Object.DestroyImmediate(root);
            return prefab;
        }

        private static T AddManager<T>(GameObject parent, string name) where T : Component
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent.transform, false);
            return go.AddComponent<T>();
        }

        // ---------------------------------------------------------------- Panel persisten

        private static PausePanel BuildPausePanel(Transform canvas)
        {
            var panel = CreatePanel<PausePanel>(canvas, "PausePanel", UIPanelId.Pause, Settings(true, true, true, true, false, false), 0.75f);
            var list = CreateVerticalList(panel.transform);
            CreateText(list, "Title", "JEDA", 64);
            Set(panel, "resumeButton", CreateButton(list, "ResumeButton", "Lanjutkan"));
            Set(panel, "settingsButton", CreateButton(list, "SettingsButton", "Pengaturan"));
            Set(panel, "logButton", CreateButton(list, "LogButton", "Catatan"));
            Set(panel, "unstuckButton", CreateButton(list, "UnstuckButton", "Saya Tersangkut"));
            var saveQuit = CreateButton(list, "SaveQuitButton", "Simpan & Keluar");
            Set(panel, "saveQuitButton", saveQuit);
            Set(panel, "saveQuitLabel", saveQuit.GetComponentInChildren<TMP_Text>());
            Set(panel, "unstuckResultText", CreateText(list, "UnstuckResult", "", 28));
            return panel;
        }

        private static LoadingPanel BuildLoadingPanel(Transform canvas)
        {
            var panel = CreatePanel<LoadingPanel>(canvas, "LoadingPanel", UIPanelId.Loading, Settings(true, false, true, false, true, true), 1f);
            var list = CreateVerticalList(panel.transform);
            CreateText(list, "Title", "Memuat...", 48);
            Set(panel, "loadingProgressBar", CreateSlider(list));
            return panel;
        }

        private static CinematicPanel BuildCinematicPanel(Transform canvas)
        {
            var panel = CreatePanel<CinematicPanel>(canvas, "CinematicPanel", UIPanelId.Cinematic, Settings(false, false, true, false, true, true), 0f);
            var black = CreateRect(panel.transform, "BlackOverlay");
            Stretch(black);
            black.gameObject.AddComponent<Image>().color = Color.black;
            Set(panel, "blackOverlay", black.gameObject.AddComponent<CanvasGroup>());
            var caption = CreateText(panel.transform, "Caption", "", 40);
            var rt = (RectTransform)caption.transform;
            rt.anchorMin = new Vector2(0.1f, 0.05f);
            rt.anchorMax = new Vector2(0.9f, 0.2f);
            rt.offsetMin = rt.offsetMax = Vector2.zero;
            Set(panel, "captionText", caption);
            return panel;
        }

        private static EndScreenPanel BuildEndScreenPanel(Transform canvas, string name, UIPanelId id, string title)
        {
            var panel = CreatePanel<EndScreenPanel>(canvas, name, id, Settings(true, true, true, false, true, true), 0.9f);
            var list = CreateVerticalList(panel.transform);
            Set(panel, "contentGroup", list.gameObject.AddComponent<CanvasGroup>());
            CreateText(list, "Title", title, 72);
            Set(panel, "reasonText", CreateText(list, "Reason", "", 36));
            Set(panel, "dayText", CreateText(list, "Day", "", 32));
            Set(panel, "seedText", CreateText(list, "Seed", "", 28));
            Set(panel, "statsText", CreateText(list, "Stats", "", 28));
            Set(panel, "retryButton", CreateButton(list, "RetryButton", "Coba Lagi"));
            Set(panel, "menuButton", CreateButton(list, "MenuButton", "Menu Utama"));
            return panel;
        }

        private static ConfirmDialogPanel BuildConfirmDialog(Transform canvas)
        {
            var panel = CreatePanel<ConfirmDialogPanel>(canvas, "ConfirmDialogPanel", UIPanelId.ConfirmDialog, Settings(true, true, true, true, false, false), 0.6f);
            var list = CreateVerticalList(panel.transform);
            Set(panel, "titleText", CreateText(list, "Title", "", 48));
            Set(panel, "bodyText", CreateText(list, "Body", "", 32));
            var yes = CreateButton(list, "YesButton", "Ya");
            var no = CreateButton(list, "NoButton", "Tidak");
            Set(panel, "yesButton", yes);
            Set(panel, "noButton", no);
            Set(panel, "yesLabel", yes.GetComponentInChildren<TMP_Text>());
            Set(panel, "noLabel", no.GetComponentInChildren<TMP_Text>());
            return panel;
        }

        // ---------------------------------------------------------------- Scene

        private static void BuildBootstrapScene(GameObject rootPrefab)
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            CreateBlackCamera();
            var entry = new GameObject("BootstrapEntry").AddComponent<BootstrapEntry>();
            Set(entry, "persistentRootPrefab", rootPrefab);
            EditorSceneManager.SaveScene(scene, BootstrapScenePath);
        }

        private static void BuildMainMenuScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            CreateBlackCamera();

            // Menu memakai canvas sendiri; EventSystem berasal dari PersistentRoot.
            var canvas = CreateCanvas(null, "MainMenuCanvas", 0);
            var menu = CreatePanel<FadingUIPanel>(canvas.transform, "MainMenuPanel", UIPanelId.MainMenu, Settings(false, true, true, false, true, false), 1f);
            var list = CreateVerticalList(menu.transform);
            CreateText(list, "Title", "SURVIVAL ISLAND", 80);

            var controller = new GameObject("MainMenuController").AddComponent<MainMenuController>();
            Set(controller, "menuPanel", menu);
            Set(controller, "continueButton", CreateButton(list, "ContinueButton", "Lanjutkan"));
            Set(controller, "newRunButton", CreateButton(list, "NewRunButton", "Run Baru"));
            Set(controller, "settingsButton", CreateButton(list, "SettingsButton", "Pengaturan"));
            Set(controller, "statsButton", CreateButton(list, "StatsButton", "Statistik"));
            Set(controller, "quitButton", CreateButton(list, "QuitButton", "Keluar"));

            EditorSceneManager.SaveScene(scene, MainMenuScenePath);
        }

        private static void CreateBlackCamera()
        {
            var cam = new GameObject("MainCamera");
            cam.tag = "MainCamera";
            var camera = cam.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = Color.black;
            cam.AddComponent<AudioListener>();
        }

        // ---------------------------------------------------------------- Bahan UI

        private static UIPanelSettings Settings(bool pause, bool cursor, bool block, bool esc, bool locked, bool reject)
        {
            return new UIPanelSettings
            {
                pausesTime = pause, releasesCursor = cursor, blocksGameplayInput = block,
                closableByEsc = esc, isLocked = locked, rejectsNewPanels = reject
            };
        }

        private static Canvas CreateCanvas(Transform parent, string name, int sortingOrder)
        {
            var go = new GameObject(name, typeof(RectTransform));
            if (parent != null) go.transform.SetParent(parent, false);
            go.layer = LayerMask.NameToLayer("UI");
            var canvas = go.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = sortingOrder;
            var scaler = go.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;
            go.AddComponent<GraphicRaycaster>();
            return canvas;
        }

        private static void CreateEventSystem(Transform parent)
        {
            var go = new GameObject("EventSystem");
            go.transform.SetParent(parent, false);
            go.AddComponent<EventSystem>();
            go.AddComponent<InputSystemUIInputModule>();
        }

        private static T CreatePanel<T>(Transform canvas, string name, UIPanelId id, UIPanelSettings settings, float backgroundAlpha) where T : UIPanel
        {
            var rt = CreateRect(canvas, name);
            Stretch(rt);
            var bg = rt.gameObject.AddComponent<Image>();
            bg.color = new Color(0f, 0f, 0f, backgroundAlpha);
            bg.raycastTarget = backgroundAlpha > 0f;
            var panel = rt.gameObject.AddComponent<T>();
            panel.id = id;
            panel.settings = settings;
            return panel;
        }

        private static Transform CreateVerticalList(Transform parent)
        {
            var rt = CreateRect(parent, "Content");
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = new Vector2(800, 900);
            var layout = rt.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.spacing = 16;
            layout.childControlWidth = true;
            layout.childControlHeight = false;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;
            return rt;
        }

        private static TMP_Text CreateText(Transform parent, string name, string text, float size)
        {
            var rt = CreateRect(parent, name);
            rt.sizeDelta = new Vector2(800, size * 1.4f);
            var tmp = rt.gameObject.AddComponent<TextMeshProUGUI>();
            tmp.text = text;
            tmp.fontSize = size;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.color = Color.white;
            tmp.raycastTarget = false;
            return tmp;
        }

        private static Button CreateButton(Transform parent, string name, string label)
        {
            var rt = CreateRect(parent, name);
            rt.sizeDelta = new Vector2(500, 72);
            var image = rt.gameObject.AddComponent<Image>();
            image.color = new Color(0.18f, 0.22f, 0.2f, 1f);
            var button = rt.gameObject.AddComponent<Button>();
            button.targetGraphic = image;
            var text = CreateText(rt, "Label", label, 34);
            Stretch((RectTransform)text.transform);
            return button;
        }

        private static Slider CreateSlider(Transform parent)
        {
            var rt = CreateRect(parent, "ProgressBar");
            rt.sizeDelta = new Vector2(700, 24);
            rt.gameObject.AddComponent<Image>().color = new Color(1f, 1f, 1f, 0.15f);
            var fillArea = CreateRect(rt, "Fill Area");
            Stretch(fillArea);
            var fill = CreateRect(fillArea, "Fill");
            Stretch(fill);
            fill.gameObject.AddComponent<Image>().color = Color.white;
            var slider = rt.gameObject.AddComponent<Slider>();
            slider.fillRect = fill;
            slider.interactable = false;
            slider.transition = Selectable.Transition.None;
            slider.minValue = 0f;
            slider.maxValue = 1f;
            return slider;
        }

        private static RectTransform CreateRect(Transform parent, string name)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            go.layer = LayerMask.NameToLayer("UI");
            return (RectTransform)go.transform;
        }

        private static void Stretch(RectTransform rt)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = rt.offsetMax = Vector2.zero;
        }

        // ---------------------------------------------------------------- Utilitas

        private static void Set(Object target, string field, Object value)
        {
            var so = new SerializedObject(target);
            var prop = so.FindProperty(field);
            if (prop == null)
            {
                Debug.LogError($"[Langkah1Setup] Field '{field}' tidak ada di {target.GetType().Name}.");
                return;
            }
            prop.objectReferenceValue = value;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path)) return;
            string parent = Path.GetDirectoryName(path).Replace('\\', '/');
            AssetDatabase.CreateFolder(parent, Path.GetFileName(path));
        }
    }
}
