using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Penghitung satu run. Disimpan di stats.json, jadi bertahan saat Lanjutkan tanpa ISaveable (#57).</summary>
    [Serializable]
    public class RunCounters
    {
        public uint seed;
        public bool isCommitted;     // Sudah masuk total
        public bool isTainted;       // Memakai cheat (#56)
        public int zombiesKilled;
        public int animalsKilled;
        public int shotsFired;
        public int shotsMissed;
        public int headshots;
        public int npcsRescued;
        public int cratesOpened;     // Metrik "Kargo dibuka" (#71). Lihat Catatan Implementasi di blueprint
        public int dayReached;
        public float playSeconds;
    }

    /// <summary>Jumlah kematian per penyebab. reason = nama DeathReason (string), supaya enum baru tidak merusak file lama.</summary>
    [Serializable] public class DeathCount { public string reason; public int count; }

    /// <summary>Isi stats.json. JsonUtility tidak bisa menyimpan Dictionary, jadi deaths berupa daftar.</summary>
    [Serializable]
    public class LocalStatsData
    {
        public const int CurrentSchema = 1;

        public int schemaVersion = CurrentSchema;
        public int totalRuns, totalWins;
        public int totalZombiesKilled, totalAnimalsKilled;
        public int totalShotsFired, totalShotsMissed, totalHeadshots;
        public int totalNpcsRescued, totalCratesOpened;
        public int longestDaySurvived;
        public float totalPlaySeconds;
        public List<DeathCount> deaths = new List<DeathCount>();
        public string lastDeathReason;
        public uint lastSeed;
        public RunCounters currentRun = new RunCounters();
    }

    /// <summary>
    /// Statistik gaya bermain di file lokal stats.json, untuk menu utama dan layar akhir.
    /// Blueprint: Core_Logic/LocalStatsManager.md (#57). Usulan, belum ada di GDD; mendukung siklus Permadeath GDD bagian 1
    /// dan 3. Keputusan developer (#85): lokal saja, tidak ada data yang dikirim ke mana pun.
    ///
    /// Manager persisten (IBootstrapService, urutan bootstrap 17). Hanya mendengar event, tidak memengaruhi gameplay.
    /// File terpisah dari survival_save.json dan TIDAK terhapus saat permadeath.
    /// </summary>
    public class LocalStatsManager : MonoBehaviour, IBootstrapService
    {
        public static LocalStatsManager Instance { get; private set; }

        public const string FileName = "stats.json";
        private const string ZombiePrefix = "zombie_";

        public LocalStatsData Data { get; private set; } = new LocalStatsData();

        /// <summary>Angka run yang sedang berjalan atau baru selesai (layar GameOver dan WinScreen).</summary>
        public RunCounters CurrentRun => Data.currentRun;

        /// <summary>Seed terakhir untuk laporan bug (#89).</summary>
        public uint LastSeed => Data.lastSeed;

        private bool isDirty;
        private float flushTimer;
        private string mainPath, tmpPath, bakPath, corruptPath;

        private static StatsBalance B
        {
            get
            {
                var cfg = GameManager.CurrentBalance;
                return cfg != null ? cfg.stats : null;
            }
        }

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("[LocalStatsManager] Instance ganda. Yang baru dimatikan.");
                enabled = false;
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
            EventChannel<EnemyKilledEvent>.Raised += HandleEnemyKilled;
            EventChannel<WeaponFiredEvent>.Raised += HandleWeaponFired;
            EventChannel<ShotResolvedEvent>.Raised += HandleShotResolved;
            EventChannel<NPCRescuedEvent>.Raised += HandleNpcRescued;
            EventChannel<BeachCrateOpenedEvent>.Raised += HandleCrateOpened;
            EventChannel<RunStartedEvent>.Raised += HandleRunStarted;
            EventChannel<SaveCompletedEvent>.Raised += HandleSaveCompleted;
            Application.wantsToQuit += HandleWantsToQuit;
        }

        private void OnDisable()
        {
            EventChannel<EnemyKilledEvent>.Raised -= HandleEnemyKilled;
            EventChannel<WeaponFiredEvent>.Raised -= HandleWeaponFired;
            EventChannel<ShotResolvedEvent>.Raised -= HandleShotResolved;
            EventChannel<NPCRescuedEvent>.Raised -= HandleNpcRescued;
            EventChannel<BeachCrateOpenedEvent>.Raised -= HandleCrateOpened;
            EventChannel<RunStartedEvent>.Raised -= HandleRunStarted;
            EventChannel<SaveCompletedEvent>.Raised -= HandleSaveCompleted;
            Application.wantsToQuit -= HandleWantsToQuit;
        }

        /// <summary>Baca stats.json. File rusak dipindah ke stats.corrupt.json, lalu dicoba dari .bak.</summary>
        public void Initialize()
        {
            string dir = Application.persistentDataPath;
            mainPath = Path.Combine(dir, FileName);
            tmpPath = mainPath + ".tmp";
            bakPath = mainPath + ".bak";
            corruptPath = Path.Combine(dir, "stats.corrupt.json");

            isDirty = false;
            flushTimer = 0f;

            // 1. File tidak ada: mulai dari nol tanpa error.
            if (!File.Exists(mainPath))
            {
                Data = TryRead(bakPath, out var fromBak) ? fromBak : new LocalStatsData();
                return;
            }

            if (TryRead(mainPath, out var loaded))
            {
                Data = loaded;
                return;
            }

            // 2. Rusak.
            try
            {
                if (File.Exists(corruptPath)) File.Delete(corruptPath);
                File.Move(mainPath, corruptPath);
                Debug.LogWarning("[LocalStatsManager] stats.json rusak, dipindah ke stats.corrupt.json.");
            }
            catch (Exception ex) { Debug.LogWarning($"[LocalStatsManager] Gagal memindahkan file rusak: {ex.Message}"); }

            Data = TryRead(bakPath, out var restored) ? restored : new LocalStatsData();
        }

        /// <summary>Tidak menghapus apa pun. RunStartedEvent yang mengatur penghitung run.</summary>
        public void ResetForNewRun() { }

        private bool TryRead(string path, out LocalStatsData data)
        {
            data = null;
            if (string.IsNullOrEmpty(path) || !File.Exists(path)) return false;
            try
            {
                string json = File.ReadAllText(path);
                data = JsonUtility.FromJson<LocalStatsData>(json);
                if (data == null) return false;

                // 3. Versi lebih baru dari game: baca sebagai data baru, dan jangan menimpa file sampai ada perubahan.
                if (data.schemaVersion > LocalStatsData.CurrentSchema)
                {
                    Debug.LogWarning($"[LocalStatsManager] {Path.GetFileName(path)} memakai schema {data.schemaVersion}, lebih baru dari game. Dibaca sebagai data baru.");
                    data = new LocalStatsData();
                    return true;
                }

                if (data.deaths == null) data.deaths = new List<DeathCount>();
                if (data.currentRun == null) data.currentRun = new RunCounters();
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[LocalStatsManager] Gagal membaca {Path.GetFileName(path)}: {ex.Message}");
                data = null;
                return false;
            }
        }

        // ---------------------------------------------------------------- Penghitung

        private RunCounters Run => Data.currentRun;

        /// <summary>Penghitung hanya bertambah saat run sedang berjalan dan belum masuk total.</summary>
        private bool CanCount => EventChannel<RunStartedEvent>.HasLast && !Run.isCommitted;

        private void HandleEnemyKilled(EnemyKilledEvent e)
        {
            // Hanya EnemyKilledEvent: musuh yang kembali ke pool tanpa dibunuh (termasuk killall konsol) tidak dihitung.
            if (!e.byPlayer || !CanCount) return;
            if (!string.IsNullOrEmpty(e.enemyId) && e.enemyId.StartsWith(ZombiePrefix, StringComparison.Ordinal)) Run.zombiesKilled++;
            else Run.animalsKilled++;
            isDirty = true;
        }

        private void HandleWeaponFired(WeaponFiredEvent e)
        {
            if (!e.byPlayer || !CanCount) return;
            Run.shotsFired++;
            isDirty = true;
        }

        private void HandleShotResolved(ShotResolvedEvent e)
        {
            // ShotResolvedEvent dipakai karena EnemyHitEvent juga dikirim melee dan tidak membawa jenis senjata.
            if (!e.byPlayer || !CanCount) return;
            if (!e.hitEnemy) Run.shotsMissed++;
            if (e.isHeadshot) Run.headshots++;
            isDirty = true;
        }

        private void HandleNpcRescued(NPCRescuedEvent e)
        {
            if (!CanCount) return;
            Run.npcsRescued++;
            isDirty = true;
        }

        private void HandleCrateOpened(BeachCrateOpenedEvent e)
        {
            if (!CanCount) return;
            Run.cratesOpened++;
            isDirty = true;
        }

        private void HandleRunStarted(RunStartedEvent e)
        {
            // 1. Lanjutkan run: penghitung lama dipakai terus.
            if (!e.isNewRun) return;

            // 2. Usulan 3: run yang ditinggalkan lewat Run Baru masuk total, tanpa menambah totalRuns atau deaths.
            if (!Run.isCommitted && !Run.isTainted) CommitCounters();

            // 3-4.
            Data.currentRun = new RunCounters { seed = e.seed };
            isDirty = true;
        }

        private void Update()
        {
            // Waktu bermain memakai Time.deltaTime, jadi berhenti saat jeda.
            if (CanCount)
            {
                Run.playSeconds += Time.deltaTime;
                isDirty = true;
                // Tanda cheat disalin tiap frame (bool murah), karena DevConsole.ResetForNewRun mengembalikannya ke false
                // sebelum RunStartedEvent run berikutnya.
                SyncTaint();
            }

            // Tulis berkala memakai waktu nyata.
            flushTimer += Time.unscaledDeltaTime;
            var b = B;
            float interval = b != null ? b.flushIntervalSeconds : 60f;
            if (flushTimer >= interval)
            {
                flushTimer = 0f;
                if (isDirty) Flush();
            }
        }

        private void HandleSaveCompleted(SaveCompletedEvent e)
        {
            if (isDirty) Flush();
        }

        private bool HandleWantsToQuit()
        {
            if (isDirty) Flush();
            return true;
        }

        // ---------------------------------------------------------------- Akhir run

        /// <summary>
        /// Dipanggil RunLifecycle.RunEndSequence (langkah 6) dan RecoverInterruptedEnd (#82).
        /// </summary>
        public void RecordRunEnd(RunResult result, DeathReason reason, int day, uint seed)
        {
            // 1. Mencegah catatan ganda saat game mati di antara langkah 6 dan 7 (#82).
            if (Run.seed == seed && Run.isCommitted) return;

            // 2.
            Run.dayReached = day;

            // 3. Run dengan cheat tidak masuk statistik (#56).
            SyncTaint();
            if (Run.isTainted)
            {
                Run.isCommitted = true;
                isDirty = true;
                Flush();
                return;
            }

            // 4.
            CommitCounters();
            Data.totalRuns++;

            // 5. DeathReason.None tidak pernah masuk deaths, karena hanya dipakai saat Won.
            if (result == RunResult.Won) Data.totalWins++;
            else if (reason != DeathReason.None) AddDeath(reason);

            // 6.
            Data.longestDaySurvived = Mathf.Max(Data.longestDaySurvived, day);
            Data.lastSeed = seed;

            // 7.
            isDirty = true;
            Flush();
        }

        private void AddDeath(DeathReason reason)
        {
            string name = reason.ToString();
            var list = Data.deaths;
            DeathCount entry = null;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] != null && list[i].reason == name)
                {
                    entry = list[i];
                    list.RemoveAt(i);
                    break;
                }
            }
            if (entry == null) entry = new DeathCount { reason = name, count = 0 };
            entry.count++;
            // Entri terbaru di akhir daftar, supaya seri dimenangkan penyebab yang terakhir terjadi.
            list.Add(entry);
            Data.lastDeathReason = name;
        }

        private void CommitCounters()
        {
            var r = Run;
            Data.totalZombiesKilled += r.zombiesKilled;
            Data.totalAnimalsKilled += r.animalsKilled;
            Data.totalShotsFired += r.shotsFired;
            Data.totalShotsMissed += r.shotsMissed;
            Data.totalHeadshots += r.headshots;
            Data.totalNpcsRescued += r.npcsRescued;
            Data.totalCratesOpened += r.cratesOpened;
            Data.totalPlaySeconds += r.playSeconds;
            r.isCommitted = true;
            isDirty = true;
        }

        /// <summary>
        /// Salin tanda cheat dari DevConsole ke run ini. Perlu sebelum ResetForNewRun DevConsole mengembalikannya ke false.
        /// </summary>
        private void SyncTaint()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (DevConsole.Instance != null && DevConsole.Instance.IsRunTainted) Run.isTainted = true;
#endif
            // Run yang dilanjutkan dengan penanda wasTainted di save GameManager tetap ditandai.
            var gm = GameManager.Instance;
            if (gm != null && gm.wasTainted && !Run.isCommitted) Run.isTainted = true;
        }

        // ---------------------------------------------------------------- Tulis file

        private void Flush()
        {
            if (string.IsNullOrEmpty(mainPath)) return;
            SyncTaintIfRunning();
            try
            {
                // 1-2. Tulis atomik: tmp, lalu ganti file utama dengan cadangan .bak.
                File.WriteAllText(tmpPath, JsonUtility.ToJson(Data, true));
                if (File.Exists(mainPath))
                {
                    try { File.Replace(tmpPath, mainPath, bakPath); }
                    catch (PlatformNotSupportedException)
                    {
                        File.Copy(mainPath, bakPath, true);
                        File.Delete(mainPath);
                        File.Move(tmpPath, mainPath);
                    }
                }
                else File.Move(tmpPath, mainPath);
                isDirty = false;
            }
            catch (Exception ex)
            {
                // 3. Kegagalan statistik tidak boleh menghentikan permadeath.
                Debug.LogWarning($"[LocalStatsManager] Gagal menulis stats.json: {ex.Message}");
                isDirty = true;
            }
        }

        private void SyncTaintIfRunning()
        {
            if (!Run.isCommitted) SyncTaint();
        }

        // ---------------------------------------------------------------- Fungsi baca

        /// <summary>Penyebab kematian terbanyak. Seri dimenangkan yang terakhir terjadi. null kalau belum pernah mati.</summary>
        public DeathReason? GetTopDeathReason()
        {
            DeathCount best = null;
            var list = Data.deaths;
            for (int i = 0; i < list.Count; i++)
            {
                var d = list[i];
                if (d == null || d.count <= 0) continue;
                if (best == null || d.count >= best.count) best = d;
            }
            if (best == null) return null;
            return Enum.TryParse(best.reason, out DeathReason reason) ? reason : (DeathReason?)null;
        }

        /// <summary>(fired - missed) / fired, atau 0 kalau belum pernah menembak.</summary>
        public float GetAccuracy()
        {
            int fired = Data.totalShotsFired;
            if (fired <= 0) return 0f;
            return Mathf.Clamp01((fired - Data.totalShotsMissed) / (float)fired);
        }

        /// <summary>Usulan 4: tombol hapus statistik di StatsPanel lewat ConfirmDialog. Tidak pernah dipanggil RunLifecycle.</summary>
        public void ResetAllStats()
        {
            Data = new LocalStatsData();
            isDirty = true;
            Flush();
        }
    }
}
