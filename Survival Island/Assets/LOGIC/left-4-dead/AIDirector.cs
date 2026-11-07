using System;
using UnityEngine;

namespace SurvivalIsland
{
    public enum DirectorPhase { Inactive, BuildUp, SustainPeak, PeakFade, Relax }

    /// <summary>Satu baris kurva ancaman (AIDirector.md "Kurva ancaman").</summary>
    [Serializable]
    public struct ThreatKey
    {
        public int day;
        public int mobSize, maxAlive, nightBudget, maxLarge;
        public float mobIntervalMin, mobIntervalMax, specialShare;
        public float hpMultiplier, speedMultiplier, damageMultiplier, recoveryMultiplier;
    }

    /// <summary>Pengali gelombang untuk satu zombie. Semua 1 berarti tanpa perubahan.</summary>
    public struct WaveModifiers
    {
        public float hp, speed, damage, recovery;
        public static WaveModifiers One => new WaveModifiers { hp = 1f, speed = 1f, damage = 1f, recovery = 1f };
    }

    /// <summary>Permintaan gelombang ke SpawnManager (#62).</summary>
    public struct WaveRequest
    {
        public int count, maxLarge;
        public float specialShare;
        public WaveModifiers modifiers;
        public string reason;           // Mob, Panic, CampCounter, RemoteTakeover (#90)
        public string forcedEnemyId;    // Kosong kecuali CampCounter
        public Vector3? origin;         // null berarti sekitar pemain
    }

    [Serializable]
    public class AIDirectorSaveData
    {
        public int spawnedTonight;
        public int phase;
        public float intensity, mobTimer, phaseTimer;
    }

    /// <summary>
    /// Tempo serangan zombie tiap malam dan kurva kesulitan menuju hari ke-60 dan sesudahnya. Blueprint: left-4-dead/AIDirector.md (#27).
    /// GDD bagian 3: Horde malam, dan zombie semakin kuat setelah hari ke-60 (pengali: keputusan developer).
    /// Siklus empat tahap dan angka kurva adalah usulan disetujui developer (#85). Director menentukan berapa banyak dan
    /// seberapa beringas; SpawnManager menentukan jenis dan tempat.
    /// Catatan Langkah 5: malam dibaca dari HourChangedEvent (DayPhaseTracker), bukan NightfallEvent (Langkah 4).
    /// isHeld mengikuti SleepStateChangedEvent (#67). isOverridden diatur HordeManager lewat SetOverridden saat Blood Moon (#50).
    /// RequestCampCounter (#94) dipanggil AITraversal. Belum diimplementasikan (TODO): pengali jumlah dari DifficultyProfile
    /// (field belum diberi nama).
    /// </summary>
    public class AIDirector : MonoBehaviour, ISaveable
    {
        public const string SaveIdConst = "ai_director";

        public static AIDirector Instance { get; private set; }

        public DirectorPhase CurrentPhase { get; private set; } = DirectorPhase.Inactive;
        public float Intensity { get; private set; }
        public ThreatKey Threat { get; private set; }

        private int spawnedTonight;
        private float mobTimer;
        private float heldSeconds;
        private float phaseTimer;
        private float phaseDuration;
        private bool isHeld;
        private bool isOverridden;
        private bool wasNight;
        private Vector3 relaxStartPos;

        private static AIDirectorBalance Bal => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.aiDirector : null;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Debug.LogError("[AIDirector] Lebih dari satu instance."); enabled = false; return; }
            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void OnEnable()
        {
            EventChannel<HourChangedEvent>.Raised += HandleHour;
            EventChannel<PlayerDamagedEvent>.Raised += HandlePlayerDamaged;
            EventChannel<PlayerPinnedEvent>.Raised += HandlePinned;
            EventChannel<EnemyKilledEvent>.Raised += HandleEnemyKilled;
            EventChannel<SleepStateChangedEvent>.Raised += HandleSleep;
            if (WorldStateSave.Instance != null) WorldStateSave.Instance.Register(this);
            wasNight = false;
            if (DayPhaseTracker.IsNight) HandleNightfall();
        }

        private void OnDisable()
        {
            EventChannel<HourChangedEvent>.Raised -= HandleHour;
            EventChannel<PlayerDamagedEvent>.Raised -= HandlePlayerDamaged;
            EventChannel<PlayerPinnedEvent>.Raised -= HandlePinned;
            EventChannel<EnemyKilledEvent>.Raised -= HandleEnemyKilled;
            EventChannel<SleepStateChangedEvent>.Raised -= HandleSleep;
            if (WorldStateSave.Instance != null) WorldStateSave.Instance.Unregister(this);
        }

        // AIDirector.md: selama tidur anggaran dan tahap ditahan (#67).
        private void HandleSleep(SleepStateChangedEvent e) => isHeld = e.isSleeping;

        private void HandleHour(HourChangedEvent e)
        {
            bool night = DayPhaseTracker.IsNightHour(e.hour);
            if (night && !wasNight) HandleNightfall();
            else if (!night && wasNight) HandleDaybreak();
        }

        // GDD bagian 3: malam adalah waktu Horde.
        private void HandleNightfall()
        {
            wasNight = true;
            var b = Bal;
            if (b == null) return;
            int day = GameManager.Instance != null ? GameManager.Instance.currentDay : 1;
            Threat = EvaluateCurve(day);
            spawnedTonight = 0;
            Intensity = 0f;
            mobTimer = b.firstWaveDelaySeconds;
            heldSeconds = 0f;
            SetPhase(DirectorPhase.BuildUp);
        }

        private void HandleDaybreak()
        {
            wasNight = false;
            SetPhase(DirectorPhase.Inactive);   // Pembakaran zombie milik SunlightExposure (#63)
        }

        private void Update()
        {
            var b = Bal;
            float dt = Time.deltaTime;
            if (b == null || CurrentPhase == DirectorPhase.Inactive || isHeld || isOverridden || dt <= 0f) return;
            // 1-2.
            Intensity = Mathf.Max(0f, Intensity - b.intensityDecayPerSec * dt);
            phaseTimer += dt;
            var spawn = SpawnManager.Instance;
            // 3.
            switch (CurrentPhase)
            {
                case DirectorPhase.BuildUp:
                    mobTimer -= dt;
                    if (mobTimer <= 0f) TrySpawnMob(b, dt);
                    if (Intensity >= b.peakThreshold && phaseTimer >= b.minBuildUpSeconds) SetPhase(DirectorPhase.SustainPeak);
                    break;
                case DirectorPhase.SustainPeak:
                    if (phaseTimer >= phaseDuration) SetPhase(DirectorPhase.PeakFade);
                    break;
                case DirectorPhase.PeakFade:
                    int large = spawn != null ? spawn.AliveLarge : 0;
                    if ((Intensity < b.peakThreshold && large < b.holdMobLargeCount) || phaseTimer >= b.fadeMaxSeconds) SetPhase(DirectorPhase.Relax);
                    break;
                case DirectorPhase.Relax:
                    bool moved = PlayerTargetable.Instance != null
                                 && Vector3.Distance(PlayerTargetable.Instance.Position, relaxStartPos) >= b.relaxBreakDistance;
                    if (phaseTimer >= phaseDuration || moved) SetPhase(DirectorPhase.BuildUp);
                    break;
            }
        }

        private void SetPhase(DirectorPhase p)
        {
            var b = Bal;
            CurrentPhase = p;
            phaseTimer = 0f;
            phaseDuration = 0f;
            if (b != null && RunSeedManager.Instance != null)
            {
                ref var rng = ref RunSeedManager.Instance.GetStream(RandomStreamId.Enemy);
                if (p == DirectorPhase.SustainPeak) phaseDuration = rng.Range(b.sustainMinSeconds, b.sustainMaxSeconds);
                else if (p == DirectorPhase.Relax) phaseDuration = rng.Range(b.relaxMinSeconds, b.relaxMaxSeconds);
            }
            else if (b != null)
            {
                phaseDuration = p == DirectorPhase.SustainPeak ? b.sustainMaxSeconds : p == DirectorPhase.Relax ? b.relaxMaxSeconds : 0f;
            }
            if (p == DirectorPhase.Relax && PlayerTargetable.Instance != null) relaxStartPos = PlayerTargetable.Instance.Position;
            EventChannel<DirectorPhaseChangedEvent>.Publish(new DirectorPhaseChangedEvent(p));
        }

        private void TrySpawnMob(AIDirectorBalance b, float dt)
        {
            var spawn = SpawnManager.Instance;
            if (spawn == null) { mobTimer = b.waveRetrySeconds; return; }
            // 1. Mob ditahan kalau varian besar masih banyak.
            if (spawn.AliveLarge >= b.holdMobLargeCount) { heldSeconds += dt; return; }
            // 2-3.
            float grow = 1f + Mathf.Min(heldSeconds / Mathf.Max(0.01f, b.heldGrowthSeconds), 1f) * b.heldGrowthMax;
            float difficultyCount = 1f;   // TODO: DifficultyProfile.enemyCountMultiplier belum diberi nama di GameBalanceConfig
            int want = Mathf.RoundToInt(Threat.mobSize * RegionMultiplier(b) * difficultyCount * grow);
            // 4.
            int count = Mathf.Min(want, Mathf.Min(Threat.maxAlive - spawn.AliveZombies, Threat.nightBudget - spawnedTonight));
            // 5.
            if (count < b.minWaveSize)
            {
                mobTimer = b.waveRetrySeconds;
                heldSeconds += b.waveRetrySeconds;
                return;
            }
            // 6.
            var r = new WaveRequest
            {
                count = count, maxLarge = Threat.maxLarge, specialShare = Threat.specialShare,
                modifiers = BuildModifiers(), reason = "Mob", forcedEnemyId = null, origin = null
            };
            int accepted = spawn.RequestWave(in r);
            // 7.
            spawnedTonight += accepted;
            heldSeconds = 0f;
            if (RunSeedManager.Instance != null)
            {
                ref var rng = ref RunSeedManager.Instance.GetStream(RandomStreamId.Enemy);
                mobTimer = rng.Range(Threat.mobIntervalMin, Threat.mobIntervalMax);
            }
            else mobTimer = Threat.mobIntervalMax;
        }

        private static float RegionMultiplier(AIDirectorBalance b)
        {
            var region = EventChannel<RegionChangedEvent>.HasLast ? EventChannel<RegionChangedEvent>.Last.to : RegionType.Volcano;
            switch (region)
            {
                case RegionType.Amazon: return b.regionMultiplierAmazon;
                case RegionType.Snow: return b.regionMultiplierSnow;
                case RegionType.Logistic: return b.regionMultiplierLogistic;   // GDD bagian 5: "secara masif"
                default: return b.regionMultiplierVolcano;
            }
        }

        /// <summary>Damage tidak dikali pengali kesulitan; itu diterapkan sekali di SurvivalStats (DamagePipeline.md).</summary>
        private WaveModifiers BuildModifiers()
        {
            return new WaveModifiers
            {
                hp = Threat.hpMultiplier, speed = Threat.speedMultiplier,
                damage = Threat.damageMultiplier, recovery = Threat.recoveryMultiplier
            };
        }

        /// <summary>Kurva linear antar baris. Setelah hari ke-60 (GDD bagian 3) dikali pengali keputusan developer.</summary>
        public ThreatKey EvaluateCurve(int day)
        {
            var b = Bal;
            var curve = b != null ? b.threatCurve : null;
            if (curve == null || curve.Length == 0) return new ThreatKey { day = day, mobSize = 6, maxAlive = 12, nightBudget = 24, mobIntervalMin = 100, mobIntervalMax = 140, hpMultiplier = 1, speedMultiplier = 1, damageMultiplier = 1, recoveryMultiplier = 1 };
            ThreatKey k;
            if (day <= curve[0].day) k = curve[0];
            else if (day >= curve[curve.Length - 1].day) k = curve[curve.Length - 1];
            else
            {
                int i = 0;
                while (i < curve.Length - 1 && curve[i + 1].day < day) i++;
                var a = curve[i];
                var c = curve[i + 1];
                float t = Mathf.InverseLerp(a.day, c.day, day);
                k = new ThreatKey
                {
                    day = day,
                    mobSize = Mathf.RoundToInt(Mathf.Lerp(a.mobSize, c.mobSize, t)),
                    maxAlive = Mathf.RoundToInt(Mathf.Lerp(a.maxAlive, c.maxAlive, t)),
                    nightBudget = Mathf.RoundToInt(Mathf.Lerp(a.nightBudget, c.nightBudget, t)),
                    maxLarge = Mathf.RoundToInt(Mathf.Lerp(a.maxLarge, c.maxLarge, t)),
                    mobIntervalMin = Mathf.Lerp(a.mobIntervalMin, c.mobIntervalMin, t),
                    mobIntervalMax = Mathf.Lerp(a.mobIntervalMax, c.mobIntervalMax, t),
                    specialShare = Mathf.Lerp(a.specialShare, c.specialShare, t),
                    hpMultiplier = Mathf.Lerp(a.hpMultiplier, c.hpMultiplier, t),
                    speedMultiplier = Mathf.Lerp(a.speedMultiplier, c.speedMultiplier, t),
                    damageMultiplier = Mathf.Lerp(a.damageMultiplier, c.damageMultiplier, t),
                    recoveryMultiplier = Mathf.Lerp(a.recoveryMultiplier, c.recoveryMultiplier, t),
                };
            }
            k.day = day;
            // Setelah hari ke-60: jumlah, HP, dan damage naik per hari dengan batas.
            float countMult = GetPostTargetCountMultiplier(day);
            var post = GetPostTargetModifiers(day);
            k.mobSize = Mathf.RoundToInt(k.mobSize * countMult);
            k.nightBudget = Mathf.RoundToInt(k.nightBudget * countMult);
            k.hpMultiplier *= post.hp;
            k.damageMultiplier *= post.damage;
            return k;
        }

        public float GetPostTargetCountMultiplier(int day)
        {
            var b = Bal;
            if (b == null) return 1f;
            int past = Mathf.Max(0, day - GameManager.TargetDay);
            return Mathf.Min(b.postTargetCountMax, 1f + b.postTargetCountPerDay * past);
        }

        public WaveModifiers GetPostTargetModifiers(int day)
        {
            var m = WaveModifiers.One;
            var b = Bal;
            if (b == null) return m;
            int past = Mathf.Max(0, day - GameManager.TargetDay);
            m.hp = Mathf.Min(b.postTargetHpMax, 1f + b.postTargetHpPerDay * past);
            m.damage = Mathf.Min(b.postTargetDamageMax, 1f + b.postTargetDamagePerDay * past);
            return m;
        }

        /// <summary>Gelombang panik (#14, #70). Melewati tahap, tetap mematuhi maxAlive.</summary>
        public int RequestPanicWave(int size, string reason)
        {
            var spawn = SpawnManager.Instance;
            if (spawn == null || size <= 0) return 0;
            int count = Mathf.Min(size, Threat.maxAlive - spawn.AliveZombies);
            if (count <= 0) return 0;
            var r = new WaveRequest { count = count, maxLarge = Threat.maxLarge, specialShare = Threat.specialShare, modifiers = BuildModifiers(), reason = reason };
            int accepted = spawn.RequestWave(in r);
            spawnedTonight += accepted;
            return accepted;
        }

        /// <summary>
        /// Pembalas berkemah (#94, AIDirector.md "Fungsi publik"): gelombang dengan forcedEnemyId. Boleh saat Relax dan tidak
        /// memakai nightBudget (spawnedTonight tidak bertambah). Pengali pasca hari ke-60 tetap berlaku.
        /// </summary>
        public int RequestCampCounter(string enemyId, int count)
        {
            var spawn = SpawnManager.Instance;
            if (spawn == null || count <= 0 || string.IsNullOrEmpty(enemyId)) return 0;
            int day = GameManager.Instance != null ? GameManager.Instance.currentDay : 1;
            var r = new WaveRequest
            {
                count = count, maxLarge = int.MaxValue, specialShare = 1f, modifiers = GetPostTargetModifiers(day),
                reason = "CampCounter", forcedEnemyId = enemyId, origin = null
            };
            return spawn.RequestWave(in r);
        }

        /// <summary>Hanya dari SpawnManager.Despawn (Stuck, OffNavMesh, TooFar, RegionUnloaded) dan slot gagal.</summary>
        public void RefundBudget(int count) => spawnedTonight = Mathf.Max(0, spawnedTonight - count);

        /// <summary>Zombie Desa yang bangkit dari tempat sembunyi (#63).</summary>
        public void AddExternalSpawn(int count)
        {
            spawnedTonight += Mathf.Max(0, count);
            if (count >= Threat.mobSize && CurrentPhase == DirectorPhase.BuildUp && Bal != null) mobTimer = Threat.mobIntervalMin;
        }

        public void SetHeld(bool held) => isHeld = held;              // SleepSystem (#67)
        public void SetOverridden(bool value) => isOverridden = value; // HordeManager (#50)

        // ---------------------------------------------------------------- Intensitas

        private void HandlePlayerDamaged(PlayerDamagedEvent e)
        {
            var b = Bal;
            if (b == null || CurrentPhase == DirectorPhase.Inactive) return;
            Intensity = Mathf.Min(1f, Intensity + e.info.amount * b.intensityPerPlayerDamage);
        }

        private void HandlePinned(PlayerPinnedEvent e)
        {
            var b = Bal;
            if (b == null || !e.isPinned || CurrentPhase == DirectorPhase.Inactive) return;
            Intensity = Mathf.Min(1f, Intensity + b.intensityPerPin);
        }

        private void HandleEnemyKilled(EnemyKilledEvent e)
        {
            var b = Bal;
            if (b == null || CurrentPhase == DirectorPhase.Inactive || PlayerTargetable.Instance == null) return;
            float d = Vector3.Distance(PlayerTargetable.Instance.Position, e.position);
            if (d > b.nearKillMax) return;
            Intensity = Mathf.Min(1f, Intensity + b.intensityPerNearKill * Mathf.InverseLerp(b.nearKillMax, b.nearKillMin, d));
        }

        // ---------------------------------------------------------------- Save (#80)

        public string SaveId => SaveIdConst;
        public int LoadOrder => 130;

        public string CaptureState()
        {
            return JsonUtility.ToJson(new AIDirectorSaveData
            {
                spawnedTonight = spawnedTonight, phase = (int)CurrentPhase, intensity = Intensity, mobTimer = mobTimer, phaseTimer = phaseTimer
            });
        }

        public void RestoreState(string json)
        {
            var d = JsonUtility.FromJson<AIDirectorSaveData>(json);
            if (d == null) return;
            spawnedTonight = d.spawnedTonight;
            CurrentPhase = (DirectorPhase)d.phase;
            Intensity = d.intensity;
            mobTimer = d.mobTimer;
            phaseTimer = d.phaseTimer;
        }

        public void OnRestoreFinished()
        {
            int day = GameManager.Instance != null ? GameManager.Instance.currentDay : 1;
            Threat = EvaluateCurve(day);
            wasNight = CurrentPhase != DirectorPhase.Inactive;
        }
    }
}
