using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Penyimpanan seluruh kondisi run ke satu file, dengan autosave aman, urutan pemuatan pasti, dan aturan permadeath.
    /// Blueprint: Core_Logic/WorldStateSave.md (#80). Menggantikan SaveSystem.md (catatan peralihan; tidak ada kelas SaveSystem).
    /// GDD bagian 1 dan 3: permadeath, kalah mengulang dari awal.
    ///
    /// Urutan bootstrap ke-16. Fase 1 tidak punya save (#82): semua save ditolak kecuali RunLifecycle.State == OnIsland.
    ///
    /// Aturan pertempuran (musuh dalam radius, PlayerDamagedEvent, VitalWarningChangedEvent, rasio HP, SleepSystem,
    /// PlayerTraversal), SpawnManager.EngagedEnemyCount dan RequestAmbush (#62), dan notifikasi (#22) sudah tersambung.
    /// Fungsi debug DevConsole (#56) savecorrupt (DebugCorruptMainFile) dan savedelay (DebugSetWriteDelay) hanya ada di
    /// Editor dan Development Build.
    /// RegionStreaming.CaptureLoadedRegions dan RegionStateStore (regionStoreJson, #88) sudah tersambung. Tanpa RegionStreaming
    /// (scene uji), regionStoreJson kosong dan objek region memakai jalur cadangan Register (RegionSaveRouting).
    /// Pemeriksaan ID save ada di alat Editor SaveIdAuditWindow (Assets/Editor, menu Survival > Audit > Save ID).
    /// </summary>
    public class WorldStateSave : MonoBehaviour, IBootstrapService
    {
        public static WorldStateSave Instance { get; private set; }

        private const string MainFileName = "survival_save.json";
        private const string CorruptFileName = "survival_save.corrupt.json";
        private const string MarkerFileName = "run_end.marker";

        private readonly List<ISaveable> registered = new List<ISaveable>();
        private readonly List<ISaveable> sortBuffer = new List<ISaveable>();

        /// <summary>Hasil pemeriksaan terakhir. Dibaca MainMenuController.</summary>
        public SaveProblem LastProblem { get; private set; } = SaveProblem.None;

        private bool autosaveEnabled = true;
        private SaveReason? pendingReason;
        private SaveReason? queuedAutosave;
        private bool isWriting;
        private SaveReason writingReason;
        private Task<bool> writeTask;
        private volatile bool abortWrites;
        private RunSavePayload loadedPayload;
        private bool? continuableCache;

        private string mainPath, tmpPath, bakPath, corruptPath, markerPath;

        // Status untuk IsSafeToAutosave.
        private Coroutine deferRoutine;
        private int activeDisasterCount;
        private bool isPinned;
        private float lastPlayerDamagedTime = float.NegativeInfinity;   // Diisi dari PlayerDamagedEvent
        private VitalWarningFlags vitalWarnings;
        private float hpRatio = 1f;
        private readonly Collider[] enemyBuffer = new Collider[16];
        private PlayerTraversal playerTraversal;

        /// <summary>Seed dari file yang terakhir dibaca ReadRunFile. Untuk RunSeedManager.RestoreFromSave.</summary>
        public RunSeedSaveData LoadedSeed => loadedPayload != null ? loadedPayload.seed : default;

        /// <summary>Data game dari file yang terakhir dibaca ReadRunFile. Untuk GameManager.RestoreRunData.</summary>
        public GameRunSaveData LoadedGame => loadedPayload != null ? loadedPayload.game : null;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("[WorldStateSave] Instance ganda. Periksa prefab PersistentRoot.");
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
            EventChannel<SaveRequestedEvent>.Raised += HandleSaveRequested;
            EventChannel<DaybreakEvent>.Raised += HandleDaybreak;
            EventChannel<PlayerPinnedEvent>.Raised += HandlePlayerPinned;
            EventChannel<DisasterStartedEvent>.Raised += HandleDisasterStarted;
            EventChannel<DisasterEndedEvent>.Raised += HandleDisasterEnded;
            EventChannel<RunEndedEvent>.Raised += HandleRunEnded;
            EventChannel<PlayerDamagedEvent>.Raised += HandlePlayerDamaged;
            EventChannel<VitalsChangedEvent>.Raised += HandleVitals;
            EventChannel<VitalWarningChangedEvent>.Raised += HandleVitalWarning;
        }

        private void OnDisable()
        {
            EventChannel<SaveRequestedEvent>.Raised -= HandleSaveRequested;
            EventChannel<DaybreakEvent>.Raised -= HandleDaybreak;
            EventChannel<PlayerPinnedEvent>.Raised -= HandlePlayerPinned;
            EventChannel<DisasterStartedEvent>.Raised -= HandleDisasterStarted;
            EventChannel<DisasterEndedEvent>.Raised -= HandleDisasterEnded;
            EventChannel<RunEndedEvent>.Raised -= HandleRunEnded;
            EventChannel<PlayerDamagedEvent>.Raised -= HandlePlayerDamaged;
            EventChannel<VitalsChangedEvent>.Raised -= HandleVitals;
            EventChannel<VitalWarningChangedEvent>.Raised -= HandleVitalWarning;
            deferRoutine = null;   // Coroutine berhenti saat objek nonaktif
        }

        // ---------------------------------------------------------------- Layanan

        public void Initialize()
        {
            // Semua file di Application.persistentDataPath. Path dibaca di thread utama.
            string dir = Application.persistentDataPath;
            mainPath = Path.Combine(dir, MainFileName);
            tmpPath = mainPath + ".tmp";
            bakPath = mainPath + ".bak";
            corruptPath = Path.Combine(dir, CorruptFileName);
            markerPath = Path.Combine(dir, MarkerFileName);

            // File .tmp tertinggal dihapus; .tmp tidak pernah dipakai, karena penulisannya mungkin belum selesai.
            TryDelete(tmpPath);
            TryDelete(markerPath + ".tmp");
        }

        public void ResetForNewRun()
        {
            pendingReason = null;
            queuedAutosave = null;
            autosaveEnabled = true;
            abortWrites = false;
            writeTask = null;
            isWriting = false;
            loadedPayload = null;
            registered.Clear();
            activeDisasterCount = 0;
            isPinned = false;
            lastPlayerDamagedTime = float.NegativeInfinity;
            vitalWarnings = VitalWarningFlags.None;
            hpRatio = 1f;
            playerTraversal = null;
            if (deferRoutine != null)
            {
                StopCoroutine(deferRoutine);
                deferRoutine = null;
            }
        }

        // ---------------------------------------------------------------- Pendaftaran

        /// <summary>Dipanggil manager di IslandPhase dari OnEnable.</summary>
        public void Register(ISaveable saveable)
        {
            if (saveable == null) return;
            for (int i = 0; i < registered.Count; i++)
            {
                if (registered[i] == saveable) return;
                if (registered[i].SaveId == saveable.SaveId)
                {
                    Debug.LogError($"[WorldStateSave] SaveId '{saveable.SaveId}' sudah terpakai.");
                    return;
                }
            }
            registered.Add(saveable);
        }

        /// <summary>Dipanggil manager di IslandPhase dari OnDisable.</summary>
        public void Unregister(ISaveable saveable)
        {
            registered.Remove(saveable);
        }

        public void SetAutosaveEnabled(bool enabled)
        {
            autosaveEnabled = enabled;
        }

        // ---------------------------------------------------------------- Menulis save

        private void HandleSaveRequested(SaveRequestedEvent e)
        {
            // Permintaan autosave pada frame yang sama digabung menjadi satu penulisan (mis. WakeUp dan Daybreak saat fajar).
            if (IsAutosaveReason(e.reason)) queuedAutosave = e.reason;
            else TrySaveNow(e.reason);
        }

        private void HandleDaybreak(DaybreakEvent e)
        {
            // Keputusan developer (#85): autosave pagi pukul 06:00, karena zombie baru terbakar.
            queuedAutosave = SaveReason.Daybreak;
        }

        private void LateUpdate()
        {
            // Game dijeda: autosave tidak dimulai.
            if (!queuedAutosave.HasValue || IsPaused()) return;
            var reason = queuedAutosave.Value;
            queuedAutosave = null;
            TrySaveNow(reason);
        }

        private void Update()
        {
            // Hasil penulisan di thread lain dibaca di sini, lalu SaveCompletedEvent dikirim dari thread utama.
            if (isWriting && writeTask != null && writeTask.IsCompleted) CompleteBackgroundWrite();
        }

        private static bool IsAutosaveReason(SaveReason reason)
        {
            // Tabel "Kapan tiap alasan dipakai": alasan yang "Diperiksa" pemeriksaan pertempuran.
            switch (reason)
            {
                case SaveReason.WakeUp:
                case SaveReason.Daybreak:
                case SaveReason.BridgeComplete:
                case SaveReason.DisasterEnded:
                case SaveReason.PartInstalled:
                    return true;
                default:
                    return false;
            }
        }

        private static bool IsPaused()
        {
            return UIFocusStack.Instance != null && UIFocusStack.Instance.IsPaused;
        }

        private bool TrySaveNow(SaveReason reason)
        {
            // 1. Ditolak saat autosave mati atau bukan di pulau. Tetap kirim hasil, supaya penunggu QuitToMenu tidak menunggu sampai batas waktu.
            var run = RunLifecycle.Instance;
            if (!autosaveEnabled || run == null || run.State != RunState.OnIsland)
            {
                EventChannel<SaveCompletedEvent>.Publish(new SaveCompletedEvent(reason, false));
                return false;
            }

            // 2. Autosave ditunda saat tidak aman.
            if (IsAutosaveReason(reason) && !IsSafeToAutosave())
            {
                pendingReason = reason;
                StartDefer();
                return false;
            }

            // 3. Sedang menulis.
            if (isWriting)
            {
                switch (reason)
                {
                    case SaveReason.QuitToMenu:
                        StartCoroutine(QuitAfterPendingWrite());
                        return false;
                    case SaveReason.AppQuit:
                        // Pengecualian: thread utama diblokir paling lama saveWaitTimeoutSeconds, karena proses akan berakhir.
                        if (!writeTask.Wait(TimeSpan.FromSeconds(SaveWaitTimeout())))
                        {
                            Debug.LogError("[WorldStateSave] Save AppQuit dibatalkan: penulisan sebelumnya belum selesai.");
                            return false;
                        }
                        CompleteBackgroundWrite();
                        break;
                    default:
                        // Autosave: catat, lalu berjalan sekali setelah penulisan selesai.
                        pendingReason = reason;
                        return false;
                }
            }

            return WriteNow(reason);
        }

        /// <summary>Langkah 4 sampai 8 TrySaveNow.</summary>
        private bool WriteNow(SaveReason reason)
        {
            string json;
            try
            {
                // 4. Region yang dimuat menulis statusnya ke RegionStateStore (#88). Region lain sudah ada di Store.
                if (RegionStreaming.Instance != null) RegionStreaming.Instance.CaptureLoadedRegions();
                // 5-6. Exception dari satu sistem menggagalkan save, bukan menulis file setengah jadi.
                json = BuildEnvelopeJson(reason);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                EventChannel<SaveCompletedEvent>.Publish(new SaveCompletedEvent(reason, false));
                return false;
            }

            // 7. Keluar game menulis langsung (proses bisa segera berakhir). Autosave di thread lain.
            if (reason == SaveReason.QuitToMenu || reason == SaveReason.AppQuit)
            {
                bool ok = WriteAtomic(mainPath, json, true, bakPath);
                continuableCache = null;
                // 8.
                EventChannel<SaveCompletedEvent>.Publish(new SaveCompletedEvent(reason, ok));
                return ok;
            }

            isWriting = true;
            writingReason = reason;
            string path = mainPath, backup = bakPath;
            int delayMs = 0;
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            delayMs = debugWriteDelayMs;   // Perintah savedelay (#56), hanya untuk penulisan di thread lain
#endif
            writeTask = Task.Run(() => WriteAtomic(path, json, true, backup, delayMs));
            return true;
        }

        private void CompleteBackgroundWrite()
        {
            var task = writeTask;
            bool ok = task != null && task.Status == TaskStatus.RanToCompletion && task.Result;
            isWriting = false;
            writeTask = null;
            continuableCache = null;
            // reason adalah alasan yang benar-benar ditulis, jadi selesainya autosave tidak terbaca sebagai selesainya save keluar.
            EventChannel<SaveCompletedEvent>.Publish(new SaveCompletedEvent(writingReason, ok));

            if (pendingReason.HasValue && deferRoutine == null)
            {
                var next = pendingReason.Value;
                pendingReason = null;
                TrySaveNow(next);
            }
        }

        private IEnumerator QuitAfterPendingWrite()
        {
            yield return WaitForPendingWrite(SaveWaitTimeout());
            if (writeTask != null && writeTask.IsCompleted) CompleteBackgroundWrite();

            if (isWriting)
            {
                // Penulisan lama belum selesai. Menulis bersamaan bisa saling menimpa, jadi save keluar dianggap gagal
                // dan RunLifecycle menampilkan ConfirmDialog bahwa progres terakhir bisa hilang.
                EventChannel<SaveCompletedEvent>.Publish(new SaveCompletedEvent(SaveReason.QuitToMenu, false));
                yield break;
            }

            var run = RunLifecycle.Instance;
            if (!autosaveEnabled || run == null || run.State != RunState.OnIsland)
            {
                EventChannel<SaveCompletedEvent>.Publish(new SaveCompletedEvent(SaveReason.QuitToMenu, false));
                yield break;
            }
            WriteNow(SaveReason.QuitToMenu);
        }

        private string BuildEnvelopeJson(SaveReason reason)
        {
            var gm = GameManager.Instance;
            var seeds = RunSeedManager.Instance;
            if (gm == null || seeds == null) throw new InvalidOperationException("[WorldStateSave] GameManager atau RunSeedManager belum siap.");

            int engaged = SpawnManager.Instance != null ? SpawnManager.Instance.EngagedEnemyCount : 0;
            var payload = new RunSavePayload
            {
                savedAtUtcTicks = DateTime.UtcNow.Ticks,
                reason = reason.ToString(),
                // Keluar saat pertempuran dicatat (#62), supaya FinishRestore bisa memanggil RequestAmbush.
                wasInCombat = engaged > 0,
                engagedEnemyCount = engaged,
                seed = seeds.CaptureSave(),
                game = gm.CaptureRunData(),
                // #88: status semua region, termasuk yang tidak dimuat.
                regionStoreJson = RegionStreaming.Instance != null ? RegionStreaming.Instance.Store.ToSaveJson() : string.Empty
            };

            var ordered = GetSortedRegistered();
            payload.entries = new SaveEntry[ordered.Count];
            for (int i = 0; i < ordered.Count; i++)
                payload.entries[i] = new SaveEntry { id = ordered[i].SaveId, json = ordered[i].CaptureState() };

            string payloadJson = JsonUtility.ToJson(payload);
            var envelope = new RunSaveEnvelope
            {
                saveVersion = SaveVersions.Current,
                gameVersion = Application.version,
                payloadCrc32 = SaveCrc32.Compute(payloadJson),
                payloadJson = payloadJson
            };
            return JsonUtility.ToJson(envelope);
        }

        private List<ISaveable> GetSortedRegistered()
        {
            sortBuffer.Clear();
            sortBuffer.AddRange(registered);
            // Insertion sort: stabil, dan jumlah manager kecil.
            for (int i = 1; i < sortBuffer.Count; i++)
            {
                var item = sortBuffer[i];
                int j = i - 1;
                while (j >= 0 && sortBuffer[j].LoadOrder > item.LoadOrder)
                {
                    sortBuffer[j + 1] = sortBuffer[j];
                    j--;
                }
                sortBuffer[j + 1] = item;
            }
            return sortBuffer;
        }

        /// <summary>
        /// Tulis tanpa merusak file lama: file .tmp, Flush(true), lalu File.Replace (atau File.Move kalau belum ada).
        /// Kalau game mati sebelum File.Replace, file utama masih versi lama. Bisa berjalan di thread lain.
        /// </summary>
        private bool WriteAtomic(string path, string json, bool abortable, string backupPath, int debugDelayMs = 0)
        {
            string tmp = path + ".tmp";
            try
            {
                var bytes = new UTF8Encoding(false).GetBytes(json);
                using (var stream = new FileStream(tmp, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Flush(true);   // Pastikan data sampai ke disk
                }

                // Penundaan buatan untuk uji #80 (savedelay): .tmp sudah ada, file utama belum diganti.
                if (debugDelayMs > 0) System.Threading.Thread.Sleep(debugDelayMs);

                // Hanya save run yang bisa dibatalkan (abortable). run_end.marker selalu tertulis.
                if (abortable && abortWrites)
                {
                    TryDelete(tmp);
                    return false;
                }

                if (File.Exists(path)) File.Replace(tmp, path, backupPath);
                else File.Move(tmp, path);
                return true;
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                TryDelete(tmp);
                Debug.LogError($"[WorldStateSave] Gagal menulis '{path}': {ex.Message}");
                return false;
            }
        }

        // ---------------------------------------------------------------- Save saat pertempuran

        private bool IsSafeToAutosave()
        {
            var balance = GameManager.CurrentBalance;
            if (balance == null) return false;

            // Musuh aktif dalam combatEnemyRadius. Musuh Dormant, Submerged, atau BurnToAshes tidak dihitung.
            var player = IslandPlayerController.Instance;
            if (player != null && HasActiveEnemyNear(player.transform.position, balance.save.combatEnemyRadius)) return false;

            // Belum lewat combatQuietSeconds sejak PlayerDamagedEvent.
            if (Time.time - lastPlayerDamagedTime < balance.save.combatQuietSeconds) return false;

            // Bencana aktif.
            if (activeDisasterCount > 0) return false;

            // Diterkam.
            if (isPinned) return false;

            // Tenggelam, membeku, lapar atau haus nol (VitalWarningChangedEvent).
            const VitalWarningFlags unsafeFlags =
                VitalWarningFlags.Drowning | VitalWarningFlags.Freezing | VitalWarningFlags.Starving | VitalWarningFlags.Dehydrated;
            if ((vitalWarnings & unsafeFlags) != 0) return false;

            // HP / MaxHP < autosaveMinHpRatio.
            if (hpRatio < balance.save.autosaveMinHpRatio) return false;

            // Sedang tidur: pemicu bangun menyusul.
            if (SleepSystem.Instance != null && SleepSystem.Instance.IsSleeping) return false;

            // Sedang jatuh atau memanjat.
            if (player != null)
            {
                if (playerTraversal == null) playerTraversal = player.GetComponent<PlayerTraversal>();
                if (!player.IsGrounded) return false;
                if (playerTraversal != null && playerTraversal.Mode == TraversalMode.Climbing) return false;
            }
            return true;
        }

        private bool HasActiveEnemyNear(Vector3 center, float radius)
        {
            int count = PhysicsQuery.OverlapSphere(center, radius, PhysicsMasks.EnemyBody, enemyBuffer);
            bool found = false;
            for (int i = 0; i < count; i++)
            {
                var c = enemyBuffer[i];
                enemyBuffer[i] = null;
                if (found || c == null) continue;
                var ai = c.GetComponentInParent<EnemyAI>();
                if (ai == null || ai.IsActiveThreat) found = true;
            }
            return found;
        }

        private void StartDefer()
        {
            if (deferRoutine == null && isActiveAndEnabled) deferRoutine = StartCoroutine(DeferLoop());
        }

        /// <summary>Coba lagi tiap autosaveRetrySeconds (waktu nyata). Permintaan dibuang setelah autosaveMaxDeferSeconds.</summary>
        private IEnumerator DeferLoop()
        {
            float start = Time.realtimeSinceStartup;
            while (true)
            {
                var balance = GameManager.CurrentBalance;
                float retry = balance != null ? balance.save.autosaveRetrySeconds : 0f;
                float maxDefer = balance != null ? balance.save.autosaveMaxDeferSeconds : 0f;
                yield return new WaitForSecondsRealtime(retry);

                if (!pendingReason.HasValue) break;
                if (Time.realtimeSinceStartup - start > maxDefer)
                {
                    // File terakhir tetap dipakai. Autosave berikutnya mencoba lagi.
                    pendingReason = null;
                    break;
                }
                // Lewati pemeriksaan saat dijeda.
                if (IsPaused()) continue;

                if (IsSafeToAutosave())
                {
                    var reason = pendingReason.Value;
                    pendingReason = null;
                    deferRoutine = null;
                    TrySaveNow(reason);
                    yield break;
                }
            }
            deferRoutine = null;
        }

        private void HandlePlayerPinned(PlayerPinnedEvent e) { isPinned = e.isPinned; }

        private void HandlePlayerDamaged(PlayerDamagedEvent e) { lastPlayerDamagedTime = Time.time; }

        private void HandleVitalWarning(VitalWarningChangedEvent e) { vitalWarnings = e.flags; }

        private void HandleVitals(VitalsChangedEvent e)
        {
            var stats = SurvivalStats.Instance;
            float max = stats != null ? stats.MaxHP : 100f;
            hpRatio = max > 0f ? e.hp / max : 1f;
        }

        private void HandleDisasterStarted(DisasterStartedEvent e) { activeDisasterCount++; }

        private void HandleDisasterEnded(DisasterEndedEvent e) { activeDisasterCount = Mathf.Max(0, activeDisasterCount - 1); }

        private void HandleRunEnded(RunEndedEvent e)
        {
            // Autosave mati sejak hasil run ditentukan, jadi save baru tidak bisa muncul setelah pemain mati.
            autosaveEnabled = false;
        }

        // ---------------------------------------------------------------- Membaca dan memulihkan

        /// <summary>Tombol Lanjutkan boleh tampil. Hasil dicache sampai file berubah.</summary>
        public bool HasContinuableSave()
        {
            if (continuableCache.HasValue) return continuableCache.Value;

            bool result;
            if (File.Exists(markerPath) || !File.Exists(mainPath))
            {
                LastProblem = SaveProblem.None;
                result = false;
            }
            else
            {
                LastProblem = ReadAndValidate(mainPath, out _);
                if (LastProblem == SaveProblem.Corrupt) LastProblem = RecoverFromCorrupt();
                result = LastProblem == SaveProblem.None;
            }

            continuableCache = result;
            return result;
        }

        /// <summary>Alur file rusak: salin ke .corrupt.json, lalu coba .bak.</summary>
        private SaveProblem RecoverFromCorrupt()
        {
            try { File.Copy(mainPath, corruptPath, true); }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                Debug.LogError($"[WorldStateSave] Gagal menyalin save rusak: {ex.Message}");
            }

            if (File.Exists(bakPath) && ReadAndValidate(bakPath, out _) == SaveProblem.None)
            {
                try
                {
                    File.Copy(bakPath, mainPath, true);
                    Debug.LogWarning("[WorldStateSave] Save terakhir rusak. Memakai save sebelumnya (.bak).");
                    // Notifikasi #22: "Save terakhir rusak. Memakai save sebelumnya".
                    if (NotificationManager.Instance != null) NotificationManager.Instance.Show("notif.save_backup_used", NotificationKind.Warning);
                    return SaveProblem.None;
                }
                catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
                {
                    Debug.LogError($"[WorldStateSave] Gagal memulihkan .bak: {ex.Message}");
                }
            }
            return SaveProblem.Corrupt;
        }

        private SaveProblem ReadAndValidate(string path, out RunSaveEnvelope envelope)
        {
            envelope = null;
            try
            {
                envelope = JsonUtility.FromJson<RunSaveEnvelope>(File.ReadAllText(path, Encoding.UTF8));
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[WorldStateSave] Gagal membaca '{path}': {ex.Message}");
                return SaveProblem.Corrupt;
            }
            return ValidateEnvelope(envelope);
        }

        private static SaveProblem ValidateEnvelope(RunSaveEnvelope e)
        {
            if (e == null || string.IsNullOrEmpty(e.payloadJson)) return SaveProblem.Corrupt;
            if (SaveCrc32.Compute(e.payloadJson) != e.payloadCrc32) return SaveProblem.Corrupt;
            if (e.saveVersion > SaveVersions.Current) return SaveProblem.NewerVersion;
            if (e.saveVersion < SaveVersions.Current)
            {
                string probe = e.payloadJson;
                return SaveMigrator.TryMigrate(e.saveVersion, ref probe) ? SaveProblem.None : SaveProblem.OldVersionNoMigration;
            }
            return SaveProblem.None;
        }

        /// <summary>Baca file save ke memori. Hasil dibaca lewat LoadedSeed dan LoadedGame.</summary>
        public bool ReadRunFile()
        {
            loadedPayload = null;
            if (!HasContinuableSave()) return false;

            if (ReadAndValidate(mainPath, out var envelope) != SaveProblem.None) return false;

            string json = envelope.payloadJson;
            if (envelope.saveVersion < SaveVersions.Current && !SaveMigrator.TryMigrate(envelope.saveVersion, ref json))
                return false;

            try
            {
                loadedPayload = JsonUtility.FromJson<RunSavePayload>(json);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                loadedPayload = null;
            }
            return loadedPayload != null && loadedPayload.game != null;
        }

        /// <summary>Pulihkan data scene dasar setelah IslandPhase dimuat, urut LoadOrder. RestoreState tidak mengirim event.</summary>
        public void RestoreBaseScene()
        {
            if (loadedPayload == null)
            {
                Debug.LogError("[WorldStateSave] RestoreBaseScene dipanggil tanpa ReadRunFile.");
                return;
            }

            var entries = loadedPayload.entries ?? Array.Empty<SaveEntry>();
            var ordered = GetSortedRegistered();
            bool storeLoaded = false;
            for (int i = 0; i < ordered.Count; i++)
            {
                var saveable = ordered[i];
                // Urutan 2 (LoadOrder 20): regionStoreJson diisi sebelum sistem LoadOrder di atas 20 dan sebelum region dimuat.
                if (!storeLoaded && saveable.LoadOrder > 20)
                {
                    LoadRegionStore();
                    storeLoaded = true;
                }
                int index = FindEntry(entries, saveable.SaveId);
                if (index < 0) continue;   // Sistem tanpa data memakai nilai awal
                try
                {
                    saveable.RestoreState(entries[index].json);
                }
                catch (Exception ex)
                {
                    // Sistem itu memakai nilai awal.
                    Debug.LogException(ex);
                }
            }

            if (!storeLoaded) LoadRegionStore();

            for (int i = 0; i < entries.Length; i++)
            {
                if (FindRegistered(entries[i].id) == null)
                    Debug.LogWarning($"[WorldStateSave] Data '{entries[i].id}' tidak punya pemilik. Diabaikan.");
            }
        }

        /// <summary>RegionStateStore memuat regionStoreJson (#88). Tanpa RegionStreaming (scene uji), data itu diabaikan.</summary>
        private void LoadRegionStore()
        {
            var rs = RegionStreaming.Instance;
            if (rs == null)
            {
                if (!string.IsNullOrEmpty(loadedPayload.regionStoreJson))
                    Debug.LogWarning("[WorldStateSave] regionStoreJson ada, tetapi RegionStreaming tidak ada di scene ini. Diabaikan.");
                return;
            }
            try { rs.Store.LoadFromSaveJson(loadedPayload.regionStoreJson); }
            catch (Exception ex) { Debug.LogException(ex); }
        }

        /// <summary>Panggil OnRestoreFinished (event sticky dikirim ulang), lalu aturan keluar saat pertempuran.</summary>
        public void FinishRestore()
        {
            var ordered = GetSortedRegistered();
            for (int i = 0; i < ordered.Count; i++)
            {
                try { ordered[i].OnRestoreFinished(); }
                catch (Exception ex) { Debug.LogException(ex); }
            }

            // Keluar saat pertempuran di malam hari bukan jalan pintas: musuh muncul lagi di sekitar pemain (#62).
            var bal = GameManager.CurrentBalance;
            var spawner = SpawnManager.Instance;
            var player = IslandPlayerController.Instance;
            if (loadedPayload != null && loadedPayload.wasInCombat && bal != null && spawner != null && player != null
                && DayNightDirector.Instance != null && DayNightDirector.Instance.IsNight)
            {
                spawner.RequestAmbush(player.transform.position,
                    Mathf.Min(loadedPayload.engagedEnemyCount, bal.save.combatRestoreMaxEnemies), bal.save.combatRestoreRadius);
            }

            loadedPayload = null;
        }

        private static int FindEntry(SaveEntry[] entries, string id)
        {
            for (int i = 0; i < entries.Length; i++) if (entries[i].id == id) return i;
            return -1;
        }

        private ISaveable FindRegistered(string id)
        {
            for (int i = 0; i < registered.Count; i++) if (registered[i].SaveId == id) return registered[i];
            return null;
        }

        // ---------------------------------------------------------------- Permadeath

        /// <summary>Tunggu penulisan autosave selesai tanpa membekukan frame, paling lama timeoutSeconds (waktu nyata).</summary>
        public IEnumerator WaitForPendingWrite(float timeoutSeconds)
        {
            float start = Time.realtimeSinceStartup;
            // Membaca writeTask langsung, bukan isWriting, karena isWriting baru dikosongkan di Update.
            while (writeTask != null && !writeTask.IsCompleted && Time.realtimeSinceStartup - start < timeoutSeconds)
                yield return null;
        }

        /// <summary>
        /// Batalkan penulisan yang tersisa, lalu tulis run_end.marker (tidak bisa dibatalkan).
        /// Pemain tidak bisa lolos dari permadeath dengan mematikan game saat sinematik.
        /// </summary>
        public void MarkRunEnded(RunResult result, DeathReason reason)
        {
            abortWrites = true;
            var gm = GameManager.Instance;
            var seeds = RunSeedManager.Instance;
            var marker = new RunEndMarker
            {
                result = result,
                reason = reason,
                currentDay = gm != null ? gm.currentDay : 0,
                seed = seeds != null ? seeds.CurrentSeed : 0
            };
            WriteAtomic(markerPath, JsonUtility.ToJson(marker), false, null);
            continuableCache = null;
        }

        /// <summary>Hapus save run (utama, .tmp, .bak, .corrupt.json). stats.json dan PlayerPrefs tidak disentuh.</summary>
        public void DeleteRunSave()
        {
            TryDelete(mainPath);
            TryDelete(tmpPath);
            TryDelete(bakPath);
            TryDelete(corruptPath);
            continuableCache = null;
            LastProblem = SaveProblem.None;
        }

        public void ClearRunEndMarker()
        {
            TryDelete(markerPath);
            continuableCache = null;
        }

        /// <summary>Baca penanda akhir run saat boot (RunLifecycle.RecoverInterruptedEnd).</summary>
        public bool TryReadRunEndMarker(out RunEndMarker marker)
        {
            marker = default;
            if (string.IsNullOrEmpty(markerPath) || !File.Exists(markerPath)) return false;
            try
            {
                marker = JsonUtility.FromJson<RunEndMarker>(File.ReadAllText(markerPath, Encoding.UTF8));
                return true;
            }
            catch (Exception ex)
            {
                // Penanda tetap berarti run sudah berakhir, walaupun isinya tidak terbaca.
                Debug.LogWarning($"[WorldStateSave] run_end.marker tidak terbaca: {ex.Message}");
                return true;
            }
        }

        // ---------------------------------------------------------------- Pembantu

        private static float SaveWaitTimeout()
        {
            var balance = GameManager.CurrentBalance;
            return balance != null ? balance.run.saveWaitTimeoutSeconds : 0f;
        }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        // ---------------------------------------------------------------- Debug (DevConsole #56, uji #80)

        private volatile int debugWriteDelayMs;

        /// <summary>Penundaan buatan sebelum File.Replace, untuk autosave di thread lain (savedelay). 0 = mati.</summary>
        public float DebugWriteDelaySeconds => debugWriteDelayMs / 1000f;

        /// <summary>Perintah savedelay &lt;detik&gt;: tunda penulisan save di thread lain. 0 mematikan.</summary>
        public void DebugSetWriteDelay(float seconds)
        {
            debugWriteDelayMs = Mathf.Max(0, Mathf.RoundToInt(seconds * 1000f));
        }

        /// <summary>
        /// Perintah savecorrupt: ubah satu byte di tengah file save utama, supaya CRC atau JSON gagal dan .bak dipakai
        /// saat menu dibuka (WorldStateSave.md skenario 9). false kalau file tidak ada atau gagal ditulis.
        /// </summary>
        public bool DebugCorruptMainFile(out string message)
        {
            if (string.IsNullOrEmpty(mainPath) || !File.Exists(mainPath))
            {
                message = "File save utama tidak ada.";
                return false;
            }
            try
            {
                var bytes = File.ReadAllBytes(mainPath);
                if (bytes.Length == 0)
                {
                    message = "File save utama kosong.";
                    return false;
                }
                int index = bytes.Length / 2;
                bytes[index] ^= 0x01;
                File.WriteAllBytes(mainPath, bytes);
                continuableCache = null;
                message = $"Byte {index} dari {bytes.Length} diubah: {mainPath}";
                return true;
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                message = "Gagal: " + ex.Message;
                return false;
            }
        }
#endif

        private static void TryDelete(string path)
        {
            if (string.IsNullOrEmpty(path)) return;
            try
            {
                if (File.Exists(path)) File.Delete(path);
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                Debug.LogError($"[WorldStateSave] Gagal menghapus '{path}': {ex.Message}");
            }
        }
    }
}
