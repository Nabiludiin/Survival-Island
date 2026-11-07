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
            // Pool yang didaftarkan menu lain (mis. Langkah 2: ShipLoot_Generic, zombie_standar) dipertahankan.
            var oldPools = new System.Collections.Generic.List<Pool>();
            var oldPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(PersistentRootPath);
            var oldPooler = oldPrefab != null ? oldPrefab.GetComponentInChildren<ObjectPooler>(true) : null;
            if (oldPooler != null) oldPools.AddRange(oldPooler.pools);

            var root = new GameObject("PersistentRoot");
            var lifecycle = root.AddComponent<RunLifecycle>();

            var managers = new GameObject("Managers");
            managers.transform.SetParent(root.transform, false);

            // Urutan bootstrap mengikuti tabel di RunLifecycle.md (19 baris; LOGIC/README.md,
            // "Tambahan untuk PersistentRoot"). SurfaceDetection tetap tepat setelah ObjectPooler.
            var gameManager = AddManager<GameManager>(managers, "GameManager");
            var input = AddManager<InputManager>(managers, "InputManager");
            var settings = AddManager<SettingsManager>(managers, "SettingsManager");
            var audio = AddManager<AudioManager>(managers, "AudioManager");
            var ambient = AddManager<AmbientAudio>(managers, "AmbientAudio");
            var music = AddManager<CombatMusicDirector>(managers, "CombatMusicDirector");
            var localization = AddManager<LocalizationManager>(managers, "LocalizationManager");
            var focus = AddManager<UIFocusStack>(managers, "UIFocusStack");
            var ui = AddManager<UIManager>(managers, "UIManager");
            var notification = AddManager<NotificationManager>(managers, "NotificationManager");
            var tutorial = AddManager<TutorialManager>(managers, "TutorialManager");
            var pooler = AddManager<ObjectPooler>(managers, "ObjectPooler");
            var surface = AddManager<SurfaceDetection>(managers, "SurfaceDetection");
            var vfx = AddManager<VFXManager>(managers, "VFXManager");
            var loader = AddManager<AsyncSceneLoader>(managers, "AsyncSceneLoader");
            var seed = AddManager<RunSeedManager>(managers, "RunSeedManager");
            var save = AddManager<WorldStateSave>(managers, "WorldStateSave");
            var stats = AddManager<LocalStatsManager>(managers, "LocalStatsManager");
            var narrative = AddManager<NarrativeSequencer>(managers, "NarrativeSequencer");
            Set(gameManager, "balanceAsset", config);
            pooler.pools = oldPools;

            // AudioManager: tiga AudioSource (bgm, sfx, warning). AudioMixer belum dibuat (lihat README),
            // jadi volume diatur lewat AudioSource saja sampai mixer dipasang.
            Set(audio, "bgmSource", AddAudioSource(audio.transform, "BGM", true));
            Set(audio, "sfxSource", AddAudioSource(audio.transform, "SFX", false));
            Set(audio, "warningSource", AddAudioSource(audio.transform, "Warning", false));
            Set(narrative, "voiceSource", AddAudioSource(narrative.transform, "Voice", false));

            var canvas = CreateCanvas(managers.transform, "PersistentCanvas", 100);
            CreateEventSystem(managers.transform);

            // UI pendukung di bawah PersistentRoot, bukan service.
            BuildSubtitles(canvas.transform);
            BuildNotifications(canvas.transform, notification);
            var hintPanel = BuildTutorialHint(canvas.transform);
            Set(tutorial, "hintPanel", hintPanel);
            var settingsPanel = BuildSettingsPanel(canvas.transform);
            ui.settingsPanel = settingsPanel;

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

            // UI NarrativeSequencer: gambar beat dan petunjuk lewati, di atas CinematicPanel.
            Set(narrative, "cinematicPanel", cinematic);
            BuildNarrativeUI(canvas.transform, narrative);

            // DevConsole hanya ada di Editor dan DEVELOPMENT_BUILD (urutan 18).
            var devConsole = BuildDevConsole(canvas.transform);

            var so = new SerializedObject(lifecycle);
            var services = so.FindProperty("orderedServices");
            MonoBehaviour[] ordered =
            {
                gameManager, input, settings, audio, ambient, music, localization, focus, ui,
                notification, tutorial, pooler, surface, vfx, loader, seed, save, stats, devConsole, narrative
            };
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

        // ---------------------------------------------------------------- UI sistem pendukung

        // Panel yang tidak disiapkan UIManager.Initialize mulai tersembunyi; Show() menyalakannya.
        private static void HideAtStart(FadingUIPanel panel)
        {
            var group = panel.GetComponent<CanvasGroup>();
            if (group == null) group = panel.gameObject.AddComponent<CanvasGroup>();
            group.alpha = 0f;
            group.blocksRaycasts = false;
            group.interactable = false;
            panel.gameObject.SetActive(false);
        }

        private static AudioSource AddAudioSource(Transform parent, string name, bool loop)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            var src = go.AddComponent<AudioSource>();
            src.playOnAwake = false;
            src.loop = loop;
            src.spatialBlend = 0f;
            return src;
        }

        private static void BuildSubtitles(Transform canvas)
        {
            var rt = CreateRect(canvas, "Subtitles");
            rt.anchorMin = new Vector2(0.15f, 0.04f);
            rt.anchorMax = new Vector2(0.85f, 0.22f);
            rt.offsetMin = rt.offsetMax = Vector2.zero;
            var layout = rt.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.childAlignment = TextAnchor.LowerCenter;
            layout.childControlWidth = true;
            layout.childControlHeight = false;
            layout.childForceExpandHeight = false;
            var presenter = rt.gameObject.AddComponent<SubtitlePresenter>();
            var template = CreateText(rt, "RowTemplate", "", 32);
            Set(presenter, "rowTemplate", template);
        }

        private static void BuildNotifications(Transform canvas, NotificationManager manager)
        {
            var rt = CreateRect(canvas, "Notifications");
            rt.anchorMin = rt.anchorMax = new Vector2(1f, 1f);
            rt.pivot = new Vector2(1f, 1f);
            rt.anchoredPosition = new Vector2(-24, -24);
            rt.sizeDelta = new Vector2(560, 400);
            var group = rt.gameObject.AddComponent<CanvasGroup>();
            group.blocksRaycasts = false;

            // Baris contoh; NotificationManager menyalinnya sebanyak notification.maxVisible saat Initialize.
            var row = CreateRect(rt, "NotificationRow");
            row.anchorMin = row.anchorMax = new Vector2(1f, 1f);
            row.pivot = new Vector2(1f, 1f);
            row.sizeDelta = new Vector2(560, 56);
            row.gameObject.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.6f);
            var rowComp = row.gameObject.AddComponent<NotificationRow>();
            var label = CreateText(row, "Label", "", 26);
            var lrt = (RectTransform)label.transform;
            Stretch(lrt);
            lrt.offsetMin = new Vector2(16, 0);
            label.alignment = TextAlignmentOptions.MidlineLeft;
            Set(rowComp, "label", label);

            Set(manager, "canvasGroup", group);
            Set(manager, "rowPrefab", rowComp);
            Set(manager, "rowContainer", rt);
        }

        private static TutorialHintPanel BuildTutorialHint(Transform canvas)
        {
            var rt = CreateRect(canvas, "TutorialHintPanel");
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 1f);
            rt.pivot = new Vector2(0.5f, 1f);
            rt.anchoredPosition = new Vector2(0, -120);
            rt.sizeDelta = new Vector2(900, 80);
            rt.gameObject.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.6f);
            var group = rt.gameObject.AddComponent<CanvasGroup>();
            group.alpha = 0f;
            var panel = rt.gameObject.AddComponent<TutorialHintPanel>();
            var text = CreateText(rt, "Text", "", 30);
            Stretch((RectTransform)text.transform);
            Set(panel, "group", group);
            Set(panel, "text", text);
            return panel;
        }

        // SettingsPanel polos: hanya slider dan toggle utama. Tab, dropdown grafis, dan baris rebind
        // belum dibuat (boleh kosong menurut SettingsPanel, yang memeriksa null).
        private static SettingsPanel BuildSettingsPanel(Transform canvas)
        {
            var panel = CreatePanel<SettingsPanel>(canvas, "SettingsPanel", UIPanelId.Settings, Settings(true, true, true, true, false, false), 0.85f);
            var list = CreateVerticalList(panel.transform);
            list.GetComponent<VerticalLayoutGroup>().spacing = 6;
            CreateText(list, "Title", "PENGATURAN", 56);
            Set(panel, "sensitivitySlider", CreateLabeledSlider(list, "Sensitivitas"));
            Set(panel, "invertYToggle", CreateToggle(list, "Balik sumbu Y"));
            Set(panel, "masterSlider", CreateLabeledSlider(list, "Volume utama"));
            Set(panel, "bgmSlider", CreateLabeledSlider(list, "Volume musik"));
            Set(panel, "sfxSlider", CreateLabeledSlider(list, "Volume efek"));
            Set(panel, "brightnessSlider", CreateLabeledSlider(list, "Kecerahan"));
            Set(panel, "fovSlider", CreateLabeledSlider(list, "Bidang pandang"));
            Set(panel, "headBobToggle", CreateToggle(list, "Goyangan kepala"));
            Set(panel, "tutorialToggle", CreateToggle(list, "Tutorial"));
            Set(panel, "subtitlesToggle", CreateToggle(list, "Subtitle"));
            Set(panel, "resetDefaultsButton", CreateButton(list, "ResetDefaultsButton", "Kembalikan Bawaan"));
            HideAtStart(panel);
            return panel;
        }

        private static void BuildNarrativeUI(Transform canvas, NarrativeSequencer narrative)
        {
            var root = CreateRect(canvas, "NarrativeUI");
            Stretch(root);
            var image = CreateRect(root, "BeatImage");
            Stretch(image);
            var img = image.gameObject.AddComponent<Image>();
            img.preserveAspect = true;
            img.raycastTarget = false;
            var group = image.gameObject.AddComponent<CanvasGroup>();
            group.alpha = 0f;
            group.blocksRaycasts = false;
            var hint = CreateText(root, "SkipHint", "Tahan Spasi untuk melewati", 24);
            var hrt = (RectTransform)hint.transform;
            hrt.anchorMin = hrt.anchorMax = new Vector2(1f, 0f);
            hrt.pivot = new Vector2(1f, 0f);
            hrt.anchoredPosition = new Vector2(-24, 24);
            hint.gameObject.SetActive(false);
            Set(narrative, "beatImage", img);
            Set(narrative, "beatImageGroup", group);
            Set(narrative, "skipHint", hint.gameObject);
        }

        private static DevConsole BuildDevConsole(Transform canvas)
        {
            // Komponen DevConsole di objek yang selalu aktif; panelRoot (anak) yang dinyalakan dan dimatikan.
            var rt = CreateRect(canvas, "DevConsole");
            Stretch(rt);
            var console = rt.gameObject.AddComponent<DevConsole>();
            console.id = UIPanelId.DevConsole;
            console.settings = Settings(true, true, true, true, false, false);

            var panelRoot = CreateRect(rt, "PanelRoot");
            panelRoot.anchorMin = new Vector2(0f, 0.5f);
            panelRoot.anchorMax = Vector2.one;
            panelRoot.offsetMin = panelRoot.offsetMax = Vector2.zero;
            panelRoot.gameObject.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.85f);

            var output = CreateText(panelRoot, "Output", "", 22);
            var ort = (RectTransform)output.transform;
            ort.anchorMin = new Vector2(0f, 0f);
            ort.anchorMax = Vector2.one;
            ort.offsetMin = new Vector2(16, 64);
            ort.offsetMax = new Vector2(-16, -16);
            output.alignment = TextAlignmentOptions.BottomLeft;

            var input = CreateInputField(panelRoot);
            var taint = CreateText(panelRoot, "TaintLabel", "[CHEAT]", 22);
            var trt = (RectTransform)taint.transform;
            trt.anchorMin = trt.anchorMax = Vector2.one;
            trt.pivot = Vector2.one;
            trt.anchoredPosition = new Vector2(-16, -8);
            trt.sizeDelta = new Vector2(200, 30);
            taint.color = Color.red;
            taint.gameObject.SetActive(false);

            Set(console, "panelRoot", panelRoot.gameObject);
            Set(console, "inputField", input);
            Set(console, "outputText", output);
            Set(console, "taintLabel", taint.gameObject);
            return console;
        }

        private static TMP_InputField CreateInputField(Transform parent)
        {
            var rt = CreateRect(parent, "Input");
            rt.anchorMin = new Vector2(0f, 0f);
            rt.anchorMax = new Vector2(1f, 0f);
            rt.pivot = new Vector2(0.5f, 0f);
            rt.offsetMin = new Vector2(8, 8);
            rt.offsetMax = new Vector2(-8, 56);
            var bg = rt.gameObject.AddComponent<Image>();
            bg.color = new Color(1f, 1f, 1f, 0.1f);
            var viewport = CreateRect(rt, "TextArea");
            Stretch(viewport);
            viewport.offsetMin = new Vector2(10, 4);
            viewport.offsetMax = new Vector2(-10, -4);
            viewport.gameObject.AddComponent<RectMask2D>();
            var text = CreateText(viewport, "Text", "", 24);
            Stretch((RectTransform)text.transform);
            text.alignment = TextAlignmentOptions.MidlineLeft;
            var field = rt.gameObject.AddComponent<TMP_InputField>();
            field.textViewport = viewport;
            field.textComponent = text;
            field.targetGraphic = bg;
            field.lineType = TMP_InputField.LineType.SingleLine;
            return field;
        }

        private static Slider CreateLabeledSlider(Transform parent, string label)
        {
            CreateText(parent, label + "Label", label, 26);
            var slider = CreateSlider(parent);
            slider.interactable = true;
            slider.name = label + "Slider";
            var handleArea = CreateRect(slider.transform, "Handle Slide Area");
            Stretch(handleArea);
            var handle = CreateRect(handleArea, "Handle");
            handle.sizeDelta = new Vector2(24, 0);
            var himg = handle.gameObject.AddComponent<Image>();
            himg.color = new Color(0.6f, 0.8f, 0.7f, 1f);
            slider.handleRect = handle;
            slider.targetGraphic = himg;
            slider.transition = Selectable.Transition.ColorTint;
            return slider;
        }

        private static Toggle CreateToggle(Transform parent, string label)
        {
            var rt = CreateRect(parent, label + "Toggle");
            rt.sizeDelta = new Vector2(500, 40);
            var box = CreateRect(rt, "Background");
            box.anchorMin = box.anchorMax = new Vector2(0f, 0.5f);
            box.pivot = new Vector2(0f, 0.5f);
            box.sizeDelta = new Vector2(32, 32);
            var bimg = box.gameObject.AddComponent<Image>();
            bimg.color = new Color(1f, 1f, 1f, 0.2f);
            var check = CreateRect(box, "Checkmark");
            Stretch(check);
            check.offsetMin = new Vector2(6, 6);
            check.offsetMax = new Vector2(-6, -6);
            var cimg = check.gameObject.AddComponent<Image>();
            cimg.color = Color.white;
            var text = CreateText(rt, "Label", label, 26);
            var trt = (RectTransform)text.transform;
            Stretch(trt);
            trt.offsetMin = new Vector2(48, 0);
            text.alignment = TextAlignmentOptions.MidlineLeft;
            var toggle = rt.gameObject.AddComponent<Toggle>();
            toggle.targetGraphic = bimg;
            toggle.graphic = cimg;
            return toggle;
        }

        private static StatsPanel BuildStatsPanel(Transform canvas)
        {
            var panel = CreatePanel<StatsPanel>(canvas, "StatsPanel", UIPanelId.Stats, Settings(true, true, true, true, false, false), 0.9f);
            var list = CreateVerticalList(panel.transform);
            CreateText(list, "Title", "STATISTIK", 56);
            Set(panel, "runsText", CreateText(list, "Runs", "", 30));
            Set(panel, "winsText", CreateText(list, "Wins", "", 30));
            Set(panel, "longestDayText", CreateText(list, "LongestDay", "", 30));
            Set(panel, "zombiesText", CreateText(list, "Zombies", "", 30));
            Set(panel, "accuracyText", CreateText(list, "Accuracy", "", 30));
            Set(panel, "topDeathText", CreateText(list, "TopDeath", "", 30));
            Set(panel, "resetButton", CreateButton(list, "ResetButton", "Hapus Statistik"));
            Set(panel, "closeButton", CreateButton(list, "CloseButton", "Tutup"));
            HideAtStart(panel);
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

            // StatsPanel di MainMenu (LOGIC/README.md, "Komponen scene"). SettingsPanel ada di PersistentRoot
            // (UIManager.settingsPanel); MainMenuController.settingsPanel adalah rujukan scene, jadi dibiarkan kosong.
            Set(controller, "statsPanel", BuildStatsPanel(canvas.transform));

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
