using System;
using UnityEngine;

namespace SurvivalIsland
{
    [Serializable]
    public class HordeManagerSaveData
    {
        public int lastBloodMoonDay = -1;
        public bool isActive;
        public int activeDay = -1;
        public int wavesSpawned;
        public float waveTimer;
        public int lastWarningDay = -1;
    }

    /// <summary>
    /// Blood Moon terjadwal. Blueprint: left-4-dead/HordeManager.md (#50).
    /// GDD bagian 3: setiap 10 hari malam menjadi merah, zombie mengabaikan Stealth, dan horde besar menyerbu base sampai fajar.
    /// Isi gelombang, jumlah zombie, dan peringatan adalah usulan disetujui developer (HordeManager.md bagian 5).
    /// Manager per scene IslandPhase, ISaveable world_bloodmoon. HordeManager hanya mengatur jadwal, suasana, dan gelombang;
    /// target milik ThreatSystem (#108), deteksi milik AISensory (#49), kemunculan milik SpawnManager (#62).
    ///
    /// Catatan implementasi (sistem pendukung):
    /// - AIDirector dan AISensoryHub belum mendengar BloodMoonStartedEvent, jadi HordeManager memanggil SetOverridden dan
    ///   IgnoreStealth langsung (API publik yang memang disiapkan untuk #50). Kalau pendengarnya ditambah, nilainya tetap sama.
    /// - Region pusat dibaca dari RegionStreaming.RegionOf (#88; cadangan ZoneTracker.Active).
    /// - Tidur dibaca dari SleepStateChangedEvent (#67).
    /// </summary>
    public class HordeManager : MonoBehaviour, ISaveable
    {
        public const string SaveIdConst = "world_bloodmoon";

        public static HordeManager Instance { get; private set; }

        [Tooltip("Tabel malam per RegionType (Volcano, Amazon, Snow, Logistic). Aset yang sama dengan SpawnManager (#62).")]
        [SerializeField] private RegionSpawnTable[] tablesByRegion = new RegionSpawnTable[4];

        /// <summary>Blood Moon sedang berjalan. Dibaca DisasterManager (#70).</summary>
        public bool IsActive { get; private set; }
        /// <summary>Dipakai SleepSystem.GetBlockReason (#67).</summary>
        public bool IsBloodMoonActive => IsActive;

        private int activeDay = -1;
        private int lastBloodMoonDay = -1;
        private int lastWarningDay = -1;
        private int wavesSpawned;
        private float waveTimer;
        private SpawnOverrideHandle overrideHandle;
        private bool hasOverride;
        private bool isSleeping;

        private static BloodMoonBalance Bal => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.bloodMoon : null;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            if (Instance != null && Instance != this) { Debug.LogError("[HordeManager] Lebih dari satu instance."); enabled = false; return; }
            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void OnEnable()
        {
            EventChannel<NightfallEvent>.Raised += HandleNightfall;
            EventChannel<DaybreakEvent>.Raised += HandleDaybreak;
            EventChannel<HourChangedEvent>.Raised += HandleHour;
            EventChannel<RunEndedEvent>.Raised += HandleRunEnded;
            EventChannel<SleepStateChangedEvent>.Raised += HandleSleep;   // #67
            if (WorldStateSave.Instance != null) WorldStateSave.Instance.Register(this);
        }

        private void OnDisable()
        {
            EventChannel<NightfallEvent>.Raised -= HandleNightfall;
            EventChannel<DaybreakEvent>.Raised -= HandleDaybreak;
            EventChannel<HourChangedEvent>.Raised -= HandleHour;
            EventChannel<RunEndedEvent>.Raised -= HandleRunEnded;
            EventChannel<SleepStateChangedEvent>.Raised -= HandleSleep;
            if (WorldStateSave.Instance != null) WorldStateSave.Instance.Unregister(this);
        }

        // ---------------------------------------------------------------- Kalkulator jadwal

        /// <summary>GDD bagian 3: Blood Moon setiap kelipatan intervalDays (10). Jadwal tetap, tanpa RunSeedManager.</summary>
        public bool IsBloodMoonDay(int day)
        {
            var b = Bal;
            return b != null && b.enabled && b.intervalDays > 0 && day > 0 && day % b.intervalDays == 0;
        }

        /// <summary>Kelipatan berikutnya setelah fromDay. Dipakai DevConsole dan petunjuk Diary (#81).</summary>
        public int NextBloodMoonDay(int fromDay)
        {
            var b = Bal;
            int interval = b != null && b.intervalDays > 0 ? b.intervalDays : 10;
            return (Mathf.Max(0, fromDay) / interval + 1) * interval;
        }

        /// <summary>Rumus ukuran gelombang (HordeManager.md bagian 1). Ikut pengali jumlah setelah hari ke-60 (keputusan developer).</summary>
        public int GetWaveSize(int day)
        {
            var b = Bal;
            if (b == null) return 0;
            int eventNumber = Mathf.Max(1, day / Mathf.Max(1, b.intervalDays));
            float mult = AIDirector.Instance != null ? AIDirector.Instance.GetPostTargetCountMultiplier(day) : 1f;
            return Mathf.Min(b.maxAlive, Mathf.RoundToInt((b.baseWaveSize + (eventNumber - 1) * b.waveSizePerEvent) * mult));
        }

        private static bool IsIslandPhase => GameManager.Instance != null && GameManager.Instance.currentPhase == GamePhase.IslandSurvival;
        private static int CurrentDay => GameManager.Instance != null ? GameManager.Instance.currentDay : 1;

        // ---------------------------------------------------------------- Peringatan

        private void HandleHour(HourChangedEvent e)
        {
            var b = Bal;
            if (b == null || e.hour != b.warningHour || !IsIslandPhase) return;
            int day = CurrentDay;
            if (!IsBloodMoonDay(day) || lastWarningDay == day) return;
            // Usulan 7: satu notifikasi jam 12:00 pada hari yang sama, tanpa hitung mundur HUD.
            lastWarningDay = day;
            if (NotificationManager.Instance != null) NotificationManager.Instance.Show("notif.bloodmoon.warning", NotificationKind.Danger);
            EventChannel<BloodMoonWarningEvent>.Publish(new BloodMoonWarningEvent(day));
        }

        // ---------------------------------------------------------------- Mulai

        private void HandleNightfall(NightfallEvent e)
        {
            // Event sticky bisa dikirim ulang; lastBloodMoonDay mencegah mulai ganda.
            if (!IsBloodMoonDay(e.day) || lastBloodMoonDay == e.day || !IsIslandPhase) return;
            StartBloodMoon(e.day);
        }

        private void StartBloodMoon(int day)
        {
            var b = Bal;
            if (b == null) return;
            IsActive = true;
            activeDay = day;
            lastBloodMoonDay = day;
            wavesSpawned = 0;
            waveTimer = b.firstWaveDelaySeconds;
            ApplyOverrides();
            EventChannel<BloodMoonStartedEvent>.Publish(new BloodMoonStartedEvent(day));
        }

        /// <summary>Override spawn, lapisan suasana, dan saklar sistem lain. Dipakai saat mulai dan setelah load.</summary>
        private void ApplyOverrides()
        {
            var b = Bal;
            if (b == null) return;
            if (SpawnManager.Instance != null && !hasOverride)
            {
                overrideHandle = SpawnManager.Instance.PushOverride(new SpawnOverride
                {
                    forceNightTable = true, suspendNormalSpawn = true, suspendAnimals = true, capOverride = b.maxAlive
                });
                hasOverride = true;
            }
            // Hanya EnvironmentDirector yang menulis fog dan cahaya (#92). Prioritas 30 ada di environment.eventLayers.
            if (EnvironmentDirector.Instance != null) EnvironmentDirector.Instance.PushEventLayer(EnvEventId.BloodMoon, 1f);
            // GDD bagian 3: zombie mengabaikan Stealth saat Blood Moon (#49). Director berhenti (#27).
            if (AISensoryHub.Instance != null) AISensoryHub.Instance.IgnoreStealth = true;
            if (AIDirector.Instance != null) AIDirector.Instance.SetOverridden(true);
        }

        private void ClearOverrides()
        {
            if (hasOverride && SpawnManager.Instance != null) SpawnManager.Instance.PopOverride(overrideHandle);
            hasOverride = false;
            if (EnvironmentDirector.Instance != null) EnvironmentDirector.Instance.PopEventLayer(EnvEventId.BloodMoon);
            if (AISensoryHub.Instance != null) AISensoryHub.Instance.IgnoreStealth = false;
            if (AIDirector.Instance != null) AIDirector.Instance.SetOverridden(false);
        }

        // ---------------------------------------------------------------- Spawn massal

        private void Update()
        {
            if (!IsActive || isSleeping || Time.deltaTime <= 0f) return;
            var b = Bal;
            var spawn = SpawnManager.Instance;
            if (b == null || spawn == null) return;

            // Game dijeda: Time.deltaTime 0, jadi timer berhenti.
            waveTimer -= Time.deltaTime;
            if (waveTimer > 0f) return;

            GetBurstCenter(out Vector3 center);
            var table = TableAt(center);
            var mods = AIDirector.Instance != null ? AIDirector.Instance.GetPostTargetModifiers(activeDay) : WaveModifiers.One;

            if (wavesSpawned < b.waveCount)
            {
                // Gelombang utama (usulan 4).
                int count = Mathf.Min(GetWaveSize(activeDay), b.maxAlive - spawn.AliveZombies);
                if (count > 0) spawn.RequestBurst(table, count, center, b.burstRadius, in mods);
                wavesSpawned++;
                waveTimer = b.waveIntervalSeconds;
            }
            else
            {
                // Tahap jaga: sisa malam diisi sustainAlive.
                int count = b.sustainAlive - spawn.AliveZombies;
                if (count > 0) spawn.RequestBurst(table, count, center, b.burstRadius, in mods);
                waveTimer = b.sustainCheckSeconds;
            }
        }

        /// <summary>Usulan 5: horde berpusat di base kalau pemain dekat base, selain itu berpusat di pemain.</summary>
        private void GetBurstCenter(out Vector3 center)
        {
            var player = PlayerTargetable.Instance;
            center = player != null ? player.Position : transform.position;
            var nav = BaseNavigation.Instance;
            if (nav != null && nav.IsAnchorEngaged(out Vector3 anchor)) center = anchor;
        }

        private RegionSpawnTable TableAt(Vector3 center)
        {
            // Region di titik pusat dari RegionStreaming (#88), cadangan ZoneTracker.Active.
            int r = (int)RegionStreaming.RegionOf(center);
            return tablesByRegion != null && r >= 0 && r < tablesByRegion.Length ? tablesByRegion[r] : null;
        }

        // ---------------------------------------------------------------- Selesai

        private void HandleDaybreak(DaybreakEvent e)
        {
            if (IsActive) EndBloodMoon();
        }

        private void EndBloodMoon()
        {
            ClearOverrides();
            IsActive = false;
            // Zombie tersisa terbakar saat fajar lewat SunlightExposure (#63). HordeManager tidak mengembalikan zombie.
            EventChannel<BloodMoonEndedEvent>.Publish(new BloodMoonEndedEvent(activeDay));
        }

        private void HandleRunEnded(RunEndedEvent e)
        {
            // Kasus Khusus: bersihkan override tanpa mengirim event.
            if (IsActive) ClearOverrides();
            IsActive = false;
        }

        /// <summary>Dipanggil SleepSystem (#67) sampai SleepStateChangedEvent ada. Tidur tidak memicu gelombang baru.</summary>
        public void SetSleeping(bool value) => isSleeping = value;
        private void HandleSleep(SleepStateChangedEvent e) => isSleeping = e.isSleeping;

        // ---------------------------------------------------------------- DevConsole (#56)

        /// <summary>bloodmoon start.</summary>
        public void DebugStart()
        {
            if (IsActive) return;
            StartBloodMoon(CurrentDay);
        }

        /// <summary>bloodmoon stop.</summary>
        public void DebugStop()
        {
            if (IsActive) EndBloodMoon();
        }

        /// <summary>bloodmoon status.</summary>
        public string DebugStatus()
        {
            return $"aktif={IsActive} hari={activeDay} gelombang={wavesSpawned} timer={waveTimer:0.0} berikutnya={NextBloodMoonDay(CurrentDay)}";
        }

        // ---------------------------------------------------------------- Save (#80)

        public string SaveId => SaveIdConst;
        public int LoadOrder => 130;

        public string CaptureState()
        {
            return JsonUtility.ToJson(new HordeManagerSaveData
            {
                lastBloodMoonDay = lastBloodMoonDay, isActive = IsActive, activeDay = activeDay,
                wavesSpawned = wavesSpawned, waveTimer = waveTimer, lastWarningDay = lastWarningDay
            });
        }

        public void RestoreState(string json)
        {
            var d = JsonUtility.FromJson<HordeManagerSaveData>(json);
            if (d == null) return;
            lastBloodMoonDay = d.lastBloodMoonDay;
            IsActive = d.isActive;
            activeDay = d.activeDay;
            wavesSpawned = d.wavesSpawned;
            waveTimer = d.waveTimer;
            lastWarningDay = d.lastWarningDay;
        }

        public void OnRestoreFinished()
        {
            // Kasus Khusus "Save saat Blood Moon": pasang lagi override dan lapisan, lalu kirim ulang event (tidak sticky).
            if (!IsActive) return;
            ApplyOverrides();
            EventChannel<BloodMoonStartedEvent>.Publish(new BloodMoonStartedEvent(activeDay));
        }
    }
}
