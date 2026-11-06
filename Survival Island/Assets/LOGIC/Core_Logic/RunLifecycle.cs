using System;
using System.Collections;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>State alur run. Blueprint: Core_Logic/RunLifecycle.md bagian 1 (#82).</summary>
    public enum RunState { Booting, InMenu, StartingRun, InShip, OnIsland, Ending, AwaitingChoice, Resetting }

    /// <summary>
    /// Pengatur alur satu run: bootstrap, menu utama, mulai atau lanjut run, perpindahan fase, akhir run, dan reset.
    /// Hanya sistem ini yang menghapus data run dan memerintahkan pemuatan scene antar-fase.
    /// Blueprint: Core_Logic/RunLifecycle.md (#82). GDD bagian 1 dan 3 (Permadeath, Syarat Menang, Game Over).
    ///
    /// Berada di objek akar PersistentRoot (DontDestroyOnLoad), di luar managersRoot.
    /// Fungsi publik menolak panggilan yang tidak sesuai state, jadi klik ganda tidak menjalankan alur dua kali.
    ///
    /// Sistem pendukung tersambung: NarrativeSequencer (#99) untuk adegan intro, sekoci, dan ending (tanpa NarrativeSequencer
    /// scene langsung aktif), DeathCinematic (#53), LocalStatsManager.RecordRunEnd (#57), AudioManager.StopWorldAudio,
    /// dan NotificationManager (#22). IslandArrival.Apply (#74) disambungkan Langkah 2.
    /// RegionStreaming.LoadInitialRegions (#88) dipanggil saat panel Loading terbuka. Tanpa RegionStreaming (scene uji
    /// satu scene), langkah itu dilewati.
    /// </summary>
    public class RunLifecycle : MonoBehaviour
    {
        public static RunLifecycle Instance { get; private set; }

        public const string BootstrapScene = "Bootstrap";
        public const string MainMenuScene = "MainMenu";
        public const string ShipScene = "ShipPhase";
        public const string IslandScene = "IslandPhase";

        /// <summary>State saat ini, hanya bisa dibaca dari luar.</summary>
        public RunState State { get; private set; } = RunState.Booting;

        [Tooltip("Daftar IBootstrapService sesuai urutan bootstrap di RunLifecycle.md (GameManager pertama).")]
        [SerializeField] private MonoBehaviour[] orderedServices = Array.Empty<MonoBehaviour>();

        [Tooltip("Anak PersistentRoot: induk semua manager persisten selain RunLifecycle.")]
        [SerializeField] private GameObject managersRoot;

        [Header("Panel alur run (objek yang sama dengan UIManager)")]
        [SerializeField] private UIPanel cinematicPanel;
        [SerializeField] private UIPanel gameOverPanel;
        [SerializeField] private UIPanel winPanel;
        [SerializeField] private UIPanel loadingPanel;

        private bool servicesInitialized;
        private bool subscribed;
        private bool flowBusy;
        private bool quitInProgress;
        private bool appQuitConfirmed;
        private bool hasQuitSaveResult;
        private bool quitSaveSucceeded;
        private bool editorQuickStartActive;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                // BootstrapEntry hanya membuat PersistentRoot kalau Instance null, jadi ini kesalahan susunan.
                Debug.LogError("[RunLifecycle] Instance ganda. PersistentRoot kedua dihancurkan.");
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void OnEnable()
        {
            Subscribe();
            Application.wantsToQuit += HandleWantsToQuit;
        }

        private void OnDisable()
        {
            Unsubscribe();
            Application.wantsToQuit -= HandleWantsToQuit;
        }

        private void Subscribe()
        {
            if (subscribed) return;
            subscribed = true;
            EventChannel<RunEndedEvent>.Raised += HandleRunEnded;
            EventChannel<SaveCompletedEvent>.Raised += HandleSaveCompleted;
        }

        private void Unsubscribe()
        {
            if (!subscribed) return;
            subscribed = false;
            EventChannel<RunEndedEvent>.Raised -= HandleRunEnded;
            EventChannel<SaveCompletedEvent>.Raised -= HandleSaveCompleted;
        }

        private void Start()
        {
            if (editorQuickStartActive) return;
            StartCoroutine(Boot());
        }

        // ---------------------------------------------------------------- Boot

        private IEnumerator Boot()
        {
            State = RunState.Booting;

            // 1. Initialize tiap layanan sesuai urutan.
            if (!InitializeServices()) yield break;

            // 2. Run yang terputus saat akhir run diselesaikan dulu.
            RecoverInterruptedEnd();

            // 3.
            yield return GoToMainMenu();
        }

        /// <summary>
        /// Panggil Initialize pada tiap layanan. Exception dicatat per layanan, kecuali GameManager yang menghentikan boot.
        /// </summary>
        private bool InitializeServices()
        {
            if (servicesInitialized) return true;

            for (int i = 0; i < orderedServices.Length; i++)
            {
                var behaviour = orderedServices[i];
                if (!(behaviour is IBootstrapService service))
                {
                    Debug.LogError($"[RunLifecycle] orderedServices[{i}] kosong atau bukan IBootstrapService.");
                    continue;
                }

                try
                {
                    service.Initialize();
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                    Debug.Log($"[RunLifecycle] Initialize {i + 1}: {behaviour.GetType().Name}");
#endif
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex, behaviour);
                    if (behaviour is GameManager)
                    {
                        Debug.LogError("[RunLifecycle] GameManager gagal Initialize. Boot dihentikan.");
                        return false;
                    }
                }
            }

            servicesInitialized = true;
            return true;
        }

        private IEnumerator GoToMainMenu()
        {
            PushPanel(loadingPanel);
            yield return Loader.LoadSingle(MainMenuScene);

            if (!Loader.LastLoadSucceeded)
            {
                // Gagal memuat MainMenu: catat kesalahan dan tetap di layar loading.
                Debug.LogError("[RunLifecycle] MainMenu gagal dimuat. Tetap di layar loading.");
                yield break;
            }

            ClosePanel(loadingPanel);
            State = RunState.InMenu;
            // MainMenuController mendorong panel MainMenu saat scene aktif.
        }

        /// <summary>Reset, lalu kembali ke MainMenu. Dipakai layar akhir, Pause, dan dialog keluar.</summary>
        public void ReturnToMainMenu()
        {
            if (flowBusy) return;
            bool allowed = State == RunState.AwaitingChoice || State == RunState.InShip || State == RunState.OnIsland;
            if (!allowed) return;
            StartCoroutine(ReturnToMainMenuRoutine());
        }

        private IEnumerator ReturnToMainMenuRoutine()
        {
            flowBusy = true;
            yield return ResetRunState();
            // Panel Loading dari reset dipakai ulang, karena Push mengabaikan panel yang sudah ada di stack.
            yield return GoToMainMenu();
            flowBusy = false;
        }

        // ---------------------------------------------------------------- Mulai run baru

        /// <summary>Hanya berlaku saat InMenu atau AwaitingChoice. GDD bagian 3: Ulangi dari Kapal.</summary>
        public void StartNewRun()
        {
            if (flowBusy) return;
            if (State != RunState.InMenu && State != RunState.AwaitingChoice) return;
            StartCoroutine(StartNewRunRoutine());
        }

        private IEnumerator StartNewRunRoutine()
        {
            flowBusy = true;

            // 1.
            State = RunState.StartingRun;
            yield return ResetRunState();
            State = RunState.StartingRun;

            // 2-3.
            Save.DeleteRunSave();
            Seeds.BeginNewRun();

            // 4. Sticky, jadi objek di scene baru bisa membacanya saat aktif.
            EventChannel<RunStartedEvent>.Publish(new RunStartedEvent(Seeds.CurrentSeed, true));

            // 5.
            Game.SetPhase(GamePhase.SinkingShip);

            // 6. Adegan intro diputar selama ShipPhase dimuat dengan holdActivation (#99).
            yield return LoadWithNarrative(ShipScene, NarrativeSequencer.IntroShip);
            if (!Loader.LastLoadSucceeded)
            {
                yield return FailBackToMenu();
                yield break;
            }

            // 7.
            ClosePanel(loadingPanel);
            State = RunState.InShip;
            flowBusy = false;
        }

        // ---------------------------------------------------------------- Lanjutkan run

        /// <summary>Hanya berlaku saat InMenu.</summary>
        public void ContinueRun()
        {
            if (flowBusy || State != RunState.InMenu) return;
            StartCoroutine(ContinueRunRoutine());
        }

        private IEnumerator ContinueRunRoutine()
        {
            flowBusy = true;

            // 1.
            State = RunState.StartingRun;
            yield return ResetRunState();
            State = RunState.StartingRun;

            // 2.
            if (!Save.ReadRunFile())
            {
                // Notifikasi #22 bahwa save tidak bisa dipakai (#80).
                Debug.LogError("[RunLifecycle] File save tidak bisa dibaca. Kembali ke MainMenu.");
                yield return FailBackToMenu("notif.save_unusable");
                yield break;
            }

            // 3.
            Seeds.RestoreFromSave(Save.LoadedSeed);
            Game.RestoreRunData(Save.LoadedGame);

            // 4.
            EventChannel<RunStartedEvent>.Publish(new RunStartedEvent(Seeds.CurrentSeed, false));
            Game.SetPhase(GamePhase.IslandSurvival);

            // 5.
            yield return Loader.LoadSingle(IslandScene);
            if (!Loader.LastLoadSucceeded)
            {
                yield return FailBackToMenu();
                yield break;
            }

            // 6. Mengisi data scene dasar, termasuk RegionStateStore dan posisi pemain.
            Save.RestoreBaseScene();

            // 7. Region awal dari posisi pemain yang baru dipulihkan (player_transform). Objek region mengambil status dari
            //    RegionStateStore, yang sudah diisi RestoreBaseScene (WorldStateSave.md "Tahap berikutnya").
            if (RegionStreaming.Instance != null)
            {
                var player = IslandPlayerController.Instance;
                Vector3 pos = player != null ? player.transform.position : Vector3.zero;
                yield return RegionStreaming.Instance.LoadInitialRegions(pos);
            }

            // 8. Event sticky dikirim ulang lewat ISaveable.
            Save.FinishRestore();

            // 9. GameManager bukan ISaveable, jadi DayChangedEvent dikirim ulang di sini.
            Game.PublishDayState(true);

            // 10.
            ClosePanel(loadingPanel);
            State = RunState.OnIsland;
            flowBusy = false;
        }

        // ---------------------------------------------------------------- Kapal ke pulau

        /// <summary>
        /// Dipanggil LifeboatExtraction (#74) di akhir Fase 1. Hanya berlaku saat InShip.
        /// Selama transisi State = StartingRun, supaya panggilan ganda dan save ditolak.
        /// </summary>
        public void EnterIslandPhase()
        {
            if (flowBusy || State != RunState.InShip) return;
            StartCoroutine(EnterIslandPhaseRoutine());
        }

        private IEnumerator EnterIslandPhaseRoutine()
        {
            flowBusy = true;
            State = RunState.StartingRun;

            // 1. Zombie, loot, dan NPC kapal hidup di bawah PersistentRoot. Dikembalikan selagi ShipPhase masih ada.
            Pooler.ReturnAllActive();

            // Tanpa NarrativeSequencer (#99), layar ditutup panel Loading selama pemuatan.
            PushPanel(loadingPanel);

            // 2-4. Adegan sekoci diputar selama IslandPhase dimuat dengan holdActivation (#99).
            Game.SetPhase(GamePhase.IslandSurvival);
            yield return LoadWithNarrative(IslandScene, NarrativeSequencer.LifeboatToBeach);
            if (!Loader.LastLoadSucceeded)
            {
                yield return FailBackToMenu();
                yield break;
            }

            // 5. Region pantai dimuat sebelum pemain ditempatkan (#88).
            if (RegionStreaming.Instance != null)
            {
                Vector3 landing = IslandArrival.Instance != null ? IslandArrival.Instance.LandingPosition : Vector3.zero;
                yield return RegionStreaming.Instance.LoadInitialRegions(landing);
            }
            // 6. IslandArrival (#74, Langkah 2) memanggil GameManager.MarkArrivedOnIsland sendiri.
            if (IslandArrival.Instance != null) IslandArrival.Instance.Apply();
            else Debug.LogError("[RunLifecycle] IslandArrival tidak ada di IslandPhase.");

            // 7. DayChangedEvent { newDay = 1 } terkirim pertama kali.
            Game.PublishDayState(false);

            // 8. Save pertama dalam satu run.
            State = RunState.OnIsland;
            EventChannel<SaveRequestedEvent>.Publish(new SaveRequestedEvent(SaveReason.ArrivedOnIsland));

            // 9.
            ClosePanel(loadingPanel);
            flowBusy = false;
        }

        // ---------------------------------------------------------------- Akhir run

        private void HandleRunEnded(RunEndedEvent e)
        {
            // Melewati hari ke-60 bukan akhir run (GDD bagian 3); hanya mati dan menang yang sampai ke sini.
            bool inRun = State == RunState.InShip || State == RunState.OnIsland || State == RunState.StartingRun;
            if (!inRun) return;
            StartCoroutine(RunEndSequence(e.result, e.reason));
        }

        private IEnumerator RunEndSequence(RunResult result, DeathReason reason)
        {
            flowBusy = true;

            // 1.
            State = RunState.Ending;
            Save.SetAutosaveEnabled(false);

            // 2. Penantian tidak membekukan frame; penulisan yang tersisa dibatalkan MarkRunEnded.
            yield return Save.WaitForPendingWrite(Balance != null ? Balance.run.saveWaitTimeoutSeconds : 0f);
            Save.MarkRunEnded(result, reason);

            // 3.
            Focus.CloseAllUnlocked(UICloseReason.RunEnded);
            PushPanel(cinematicPanel);

            // 4. Kegagalan sinematik tidak boleh menghentikan permadeath; waktu tunggu tak terskala.
            float timeout = Balance == null ? 0f
                : result == RunResult.Won ? Balance.run.endingTimeoutSeconds : Balance.run.endSequenceTimeoutSeconds;
            // Langkah 7 (#53): Died -> DeathCinematic.Instance.Play(reason); kalau Instance null, langkah ini dilewati.
            // Won -> NarrativeSequencer.PlayEnding() (#99); kalau Instance null, langkah ini dilewati.
            IEnumerator closing = null;
            if (result == RunResult.Died && DeathCinematic.Instance != null) closing = DeathCinematic.Instance.Play(reason);
            else if (result == RunResult.Won && NarrativeSequencer.Instance != null) closing = NarrativeSequencer.Instance.PlayEnding();
            if (closing != null) yield return RunWithTimeout(closing, timeout);

            // 5.
            Save.DeleteRunSave();

            // 6. Statistik lokal (#57).
            if (LocalStatsManager.Instance != null && Game != null && Seeds != null)
                LocalStatsManager.Instance.RecordRunEnd(result, reason, Game.currentDay, Seeds.CurrentSeed);

            // 7.
            Save.ClearRunEndMarker();

            // 8. Cinematic ditutup dan layar akhir didorong pada frame yang sama, jadi tidak ada frame dunia yang terlihat.
            ClosePanel(cinematicPanel);
            var ui = UIManager.Instance;
            if (result == RunResult.Died)
            {
                if (ui != null) ui.ShowGameOver(reason);
                PushPanel(gameOverPanel);
            }
            else
            {
                if (ui != null) ui.ShowWin();
                PushPanel(winPanel);
            }

            // 9. Tunggu pilihan pemain (tombol layar akhir memanggil StartNewRun atau ReturnToMainMenu).
            State = RunState.AwaitingChoice;
            flowBusy = false;
        }

        /// <summary>Jalankan routine sampai selesai atau sampai batas waktu (waktu nyata).</summary>
        private IEnumerator RunWithTimeout(IEnumerator routine, float timeoutSeconds)
        {
            bool done = false;
            var handle = StartCoroutine(TrackCompletion(routine, () => done = true));
            float start = Time.realtimeSinceStartup;
            while (!done && Time.realtimeSinceStartup - start < timeoutSeconds)
                yield return null;
            if (!done && handle != null)
            {
                StopCoroutine(handle);
                Debug.LogWarning("[RunLifecycle] Penutup run melewati batas waktu. Alur akhir tetap berlanjut.");
            }
        }

        private static IEnumerator TrackCompletion(IEnumerator routine, Action onDone)
        {
            yield return routine;
            onDone();
        }

        /// <summary>
        /// Game ditutup paksa saat akhir run: penanda run_end.marker masih ada. Hapus save, catat statistik, hapus penanda.
        /// </summary>
        private void RecoverInterruptedEnd()
        {
            if (!Save.TryReadRunEndMarker(out var marker)) return;
            Save.DeleteRunSave();
            LocalStatsManager.Instance?.RecordRunEnd(marker.result, marker.reason, marker.currentDay, marker.seed);   // #57
            Debug.Log($"[RunLifecycle] Run yang terputus saat berakhir diselesaikan ({marker.result}, hari {marker.currentDay}).");
            Save.ClearRunEndMarker();
        }

        // ---------------------------------------------------------------- Keluar ke menu saat bermain

        /// <summary>Dipanggil dari panel Pause.</summary>
        public void SaveAndQuitToMenu()
        {
            if (flowBusy || quitInProgress) return;

            if (State == RunState.OnIsland)
            {
                StartCoroutine(SaveAndQuitRoutine());
            }
            else if (State == RunState.InShip)
            {
                // Fase 1 tidak punya save (#82): beri tahu bahwa run ini hilang.
                ShowQuitConfirm("ui.quit.title", "ui.quit.lose_run");
            }
        }

        private IEnumerator SaveAndQuitRoutine()
        {
            quitInProgress = true;
            hasQuitSaveResult = false;
            quitSaveSucceeded = false;

            EventChannel<SaveRequestedEvent>.Publish(new SaveRequestedEvent(SaveReason.QuitToMenu));

            // Tunggu SaveCompletedEvent dengan reason == QuitToMenu. Hasil alasan lain (mis. autosave) diabaikan.
            float timeout = Balance != null ? Balance.run.saveWaitTimeoutSeconds : 0f;
            float start = Time.realtimeSinceStartup;
            while (!hasQuitSaveResult && Time.realtimeSinceStartup - start < timeout)
                yield return null;

            quitInProgress = false;
            if (hasQuitSaveResult && quitSaveSucceeded)
            {
                ReturnToMainMenu();
            }
            else
            {
                // Gagal atau batas waktu habis: pemain mendapat pilihan, tidak keluar diam-diam.
                ShowQuitConfirm("ui.quit.title", "ui.quit.save_failed");
            }
        }

        private void HandleSaveCompleted(SaveCompletedEvent e)
        {
            if (!quitInProgress || e.reason != SaveReason.QuitToMenu) return;
            hasQuitSaveResult = true;
            quitSaveSucceeded = e.succeeded;
        }

        private void ShowQuitConfirm(string titleKey, string bodyKey)
        {
            var ui = UIManager.Instance;
            var request = new ConfirmRequest { titleKey = titleKey, bodyKey = bodyKey, onResult = HandleQuitConfirmResult };
            if (ui == null || !ui.ShowConfirm(request))
                Debug.LogWarning("[RunLifecycle] ConfirmDialog keluar tidak bisa dibuka.");
        }

        private void HandleQuitConfirmResult(bool confirmed)
        {
            if (confirmed) ReturnToMainMenu();
        }

        /// <summary>Menutup jendela memakai alur yang sama dengan keluar ke menu.</summary>
        private bool HandleWantsToQuit()
        {
            switch (State)
            {
                case RunState.OnIsland:
                    // Selalu tulis (AppQuit). WorldStateSave menulis langsung di thread utama.
                    if (!flowBusy) EventChannel<SaveRequestedEvent>.Publish(new SaveRequestedEvent(SaveReason.AppQuit));
                    return true;

                case RunState.InShip:
                    if (appQuitConfirmed) return true;
                    // Fase 1 tidak punya save: tanya dulu, karena run ini akan hilang.
                    appQuitConfirmed = true;
                    var ui = UIManager.Instance;
                    var request = new ConfirmRequest { titleKey = "ui.quit.title", bodyKey = "ui.quit.lose_run", onResult = HandleAppQuitConfirmResult };
                    if (ui != null && ui.ShowConfirm(request)) return false;
                    return true;

                default:
                    // Ending, AwaitingChoice, menu, dan transisi: langsung ditutup tanpa save.
                    return true;
            }
        }

        private void HandleAppQuitConfirmResult(bool confirmed)
        {
            if (confirmed) Application.Quit();
            else appQuitConfirmed = false;
        }

        // ---------------------------------------------------------------- Reset satu pintu

        /// <summary>Satu-satunya tempat pembersihan run (RunLifecycle.md "Reset satu pintu").</summary>
        private IEnumerator ResetRunState()
        {
            // 1.
            State = RunState.Resetting;

            // 2. Time.timeScale dan kursor diatur ulang oleh stack, bukan oleh sistem lain.
            Focus.ClearForNewRun();
            PushPanel(loadingPanel);

            // 3. AudioListener.pause mengikuti stack.
            AudioManager.Instance?.StopWorldAudio();

            // 4. Tiap objek menjalankan OnReturned selagi scene lama masih ada.
            Pooler.ReturnAllActive();

            // 5. Scene lama dan region additive terlepas; pendengar di scene lepas lewat OnDisable.
            yield return Loader.LoadSingle(BootstrapScene);
            if (!Loader.LastLoadSucceeded)
                Debug.LogError("[RunLifecycle] Scene Bootstrap gagal dimuat saat reset. Reset dilanjutkan.");

            // 6. Pendengar persisten lepas lewat OnDisable.
            Unsubscribe();
            if (managersRoot != null) managersRoot.SetActive(false);

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            // 7. Pemeriksaan kebocoran: pada titik ini jumlah pendengar seharusnya 0.
            if (GameEventRegistry.ListenerCount > 0) GameEventRegistry.LogRemainingListeners();
#endif

            // 8. Nilai sticky lama dan pendengar yang bocor hilang.
            GameEventRegistry.ClearAll();

            // 9. Pendengar persisten berlangganan kembali lewat OnEnable; UIFocusStack menerbitkan ulang statusnya.
            if (managersRoot != null) managersRoot.SetActive(true);
            Subscribe();

            // 10. ResetForNewRun sesuai urutan bootstrap.
            for (int i = 0; i < orderedServices.Length; i++)
            {
                if (!(orderedServices[i] is IBootstrapService service)) continue;
                try { service.ResetForNewRun(); }
                catch (Exception ex) { Debug.LogException(ex, orderedServices[i]); }
            }

            // 11. Aman karena layar sedang loading.
            yield return Resources.UnloadUnusedAssets();
            GC.Collect();
        }

        /// <summary>
        /// Muat scene tunggal sambil memutar adegan (NarrativeSequencer.PlayDuringLoad, #99). Tanpa NarrativeSequencer,
        /// scene langsung dimuat seperti biasa. Hasil dibaca dari Loader.LastLoadSucceeded.
        /// </summary>
        private IEnumerator LoadWithNarrative(string sceneName, string narrativeId)
        {
            var narrative = NarrativeSequencer.Instance;
            if (narrative == null)
            {
                yield return Loader.LoadSingle(sceneName);
                yield break;
            }
            // Langkah awal LoadSingle berjalan sebelum yield pertamanya, jadi AllowActivation dari PlayDuringLoad tidak tertimpa.
            var load = StartCoroutine(Loader.LoadSingle(sceneName, holdActivation: true));
            yield return narrative.PlayDuringLoad(narrativeId);
            yield return load;
        }

        /// <summary>Scene gagal dimuat: kembali ke MainMenu (layar Loading sudah terbuka).</summary>
        private IEnumerator FailBackToMenu(string notifKey = "notif.load_failed")
        {
            Debug.LogError("[RunLifecycle] Pemuatan gagal. Kembali ke MainMenu.");
            yield return ResetRunState();
            // Notifikasi kegagalan (#22). Dikirim setelah reset, karena ResetForNewRun mengosongkan antrean notifikasi.
            if (NotificationManager.Instance != null) NotificationManager.Instance.Show(notifKey, NotificationKind.Danger);
            yield return GoToMainMenu();
            flowBusy = false;
        }

        // ---------------------------------------------------------------- Tes dari scene lain di Editor

#if UNITY_EDITOR
        /// <summary>
        /// Dipanggil EditorQuickStart setelah PersistentRoot dibuat di scene selain Bootstrap.
        /// Initialize berjalan langsung (sebelum Start objek scene), lalu run tes dimulai tanpa menu.
        /// </summary>
        internal void BeginEditorQuickStart(string activeSceneName)
        {
            editorQuickStartActive = true;
            if (!InitializeServices()) return;

            Seeds.BeginNewRun();
            EventChannel<RunStartedEvent>.Publish(new RunStartedEvent(Seeds.CurrentSeed, true));

            if (activeSceneName == ShipScene)
            {
                Game.SetPhase(GamePhase.SinkingShip);
                State = RunState.InShip;
            }
            else
            {
                if (activeSceneName != IslandScene)
                    Debug.LogWarning($"[RunLifecycle] EditorQuickStart di scene '{activeSceneName}': diperlakukan sebagai IslandPhase.");
                Game.SetPhase(GamePhase.IslandSurvival);
                Game.PublishDayState(false);
                State = RunState.OnIsland;
            }
        }
#endif

        // ---------------------------------------------------------------- Pembantu

        private static GameManager Game => GameManager.Instance;
        private static GameBalanceConfig Balance => GameManager.CurrentBalance;
        private static AsyncSceneLoader Loader => AsyncSceneLoader.Instance;
        private static UIFocusStack Focus => UIFocusStack.Instance;
        private static ObjectPooler Pooler => ObjectPooler.Instance;
        private static WorldStateSave Save => WorldStateSave.Instance;
        private static RunSeedManager Seeds => RunSeedManager.Instance;

        private static void PushPanel(UIPanel panel)
        {
            if (panel != null && Focus != null) Focus.Push(panel);
        }

        private static void ClosePanel(UIPanel panel)
        {
            if (panel != null && Focus != null) Focus.Close(panel, UICloseReason.Forced);
        }
    }
}
