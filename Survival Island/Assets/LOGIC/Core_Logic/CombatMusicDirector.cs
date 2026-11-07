using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace SurvivalIsland
{
    public enum MusicTier { Calm, Alert, Combat, Peak }

    /// <summary>
    /// Musik Fase 2 sesuai ketegangan pertempuran, dan pengatur suara penanda musuh 3D (ThreatAudioEmitter).
    /// Blueprint: Core_Logic/CombatMusicDirector.md (#114). GDD bagian 3 (ancaman siang dan malam), bagian 5 (varian per
    /// region), bagian 6 (bencana). Lapisan musik dan suara penanda adalah usulan yang disetujui developer.
    ///
    /// Manager persisten di PersistentRoot, urutan bootstrap ke-6 (memakai mixer AudioManager).
    /// Aktif hanya saat GamePhase.IslandSurvival; musik Fase 1 milik ShipTimerManager. Tidak menyimpan data ke save.
    /// </summary>
    public class CombatMusicDirector : MonoBehaviour, IBootstrapService
    {
        public static CombatMusicDirector Instance { get; private set; }

        [Header("Musik")]
        [SerializeField] private MusicSet musicSet;
        [SerializeField] private AudioMixerGroup musicGroup;

        [Header("Suara musuh")]
        [Tooltip("Grup EnemyCue. Dipakai ThreatAudioEmitter dan suara kerumunan.")]
        [SerializeField] private AudioMixerGroup enemyCueGroup;
        [Tooltip("Loop suara kerumunan horde.")]
        [SerializeField] private AudioClip crowdLoopClip;

        [Header("Peringatan (lewat AudioManager.PlayWarning, grup Warning)")]
        [SerializeField] private AudioClip bloodMoonWarningClip;
        [SerializeField] private AudioClip bloodMoonStartClip;

        public MusicTier CurrentTier { get; private set; } = MusicTier.Calm;
        /// <summary>Ketegangan gabungan 0..1. Dibaca DevConsole (#56) lewat perintah music.</summary>
        public float Score => score;
        public AudioMixerGroup EnemyCueGroup => enemyCueGroup;

        // Urutan stem.
        private const int CalmDay = 0, CalmNight = 1, AlertStem = 2, CombatStem = 3, PeakStem = 4, StemCount = 5;
        // Urutan level fade: Calm (dibagi siang dan malam saat ditulis), Alert, Combat, Peak.
        private const int LevelCount = 4;

        private readonly AudioSource[] stems = new AudioSource[StemCount];
        private AudioSource crowdBedSource;
        private readonly float[] levels = new float[LevelCount];
        private readonly float[] levelTargets = new float[LevelCount];
        private float stemFadeSpeed = 1f;
        private float nightBlend, nightTarget;
        private float musicFade = 1f, musicFadeTarget = 1f, musicFadeSpeed = 1f;
        private float duckLinear = 1f;
        private bool stemsPlaying;

        private float score;
        private float damageScore;
        private bool damageBypass;
        private float tierHoldTimer, riseTimer;
        private MusicTier tierFloor = MusicTier.Calm;
        private float evalTimer;

        private DirectorPhase directorPhase = DirectorPhase.Inactive;
        private bool isPinned, isSleeping;
        private bool cinematicPlaying, cinematicMuted, narrativePlaying;
        private bool bloodMoonActive, disasterActive;
        private bool phaseActive, runOver;
        private float suppressStartStingerUntil;

        // Peringatan.
        private float lastStingerTime = -100f;
        private AudioClip queuedStinger;

        // Suara musuh.
        private readonly List<ThreatAudioEmitter> voices = new List<ThreatAudioEmitter>(16);
        private int emitterCursor;
        private Vector3 listenerPos;
        private int engagedCount;
        private Vector3 engagedCentroid;

        private static CombatMusicBalance Bal => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.combatMusic : null;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("[CombatMusicDirector] Instance ganda. Periksa prefab PersistentRoot.");
                return;
            }
            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        // ---------------------------------------------------------------- Layanan

        public void Initialize()
        {
            if (AudioManager.Instance == null || AudioManager.Instance.Mixer == null)
                Debug.LogWarning("[CombatMusicDirector] Mixer AudioManager kosong. Musik keluar tanpa grup Music.");

            // Sumber dibuat sekali saat bootstrap, bukan saat bermain.
            if (stems[0] == null)
            {
                var go = new GameObject("MusicStems");
                go.transform.SetParent(transform, false);
                for (int i = 0; i < StemCount; i++)
                {
                    var s = go.AddComponent<AudioSource>();
                    s.playOnAwake = false;
                    s.loop = true;
                    s.spatialBlend = 0f;
                    // Musik tetap berjalan saat jeda pada pausedMusicVolumeDb lewat snapshot mixer (#83).
                    s.ignoreListenerPause = true;
                    s.outputAudioMixerGroup = musicGroup;
                    s.volume = 0f;
                    stems[i] = s;
                }
            }
            if (crowdBedSource == null)
            {
                var go = new GameObject("CrowdBed");
                go.transform.SetParent(transform, false);
                crowdBedSource = go.AddComponent<AudioSource>();
                crowdBedSource.playOnAwake = false;
                crowdBedSource.loop = true;
                crowdBedSource.spatialBlend = 1f;
                crowdBedSource.rolloffMode = AudioRolloffMode.Logarithmic;
                crowdBedSource.dopplerLevel = 0f;
                crowdBedSource.outputAudioMixerGroup = enemyCueGroup;
                crowdBedSource.volume = 0f;
            }
            if (EventChannel<PhaseChangedEvent>.HasLast) ApplyPhase(EventChannel<PhaseChangedEvent>.Last.phase);
        }

        public void ResetForNewRun()
        {
            // Bagian 4 "Permadeath".
            StopImmediate();
            CurrentTier = MusicTier.Calm;
            score = 0f;
            damageScore = 0f;
            damageBypass = false;
            tierFloor = MusicTier.Calm;
            tierHoldTimer = 0f;
            riseTimer = 0f;
            bloodMoonActive = false;
            disasterActive = false;
            runOver = false;
            phaseActive = false;
            queuedStinger = null;
            for (int i = 0; i < ThreatAudioEmitter.Active.Count; i++)
                if (ThreatAudioEmitter.Active[i] != null) ThreatAudioEmitter.Active[i].activeIndex = -1;
            ThreatAudioEmitter.Active.Clear();
            emitterCursor = 0;
        }

        /// <summary>Hentikan semua musik dan suara musuh seketika. Dipanggil AudioManager.StopWorldAudio.</summary>
        public void StopImmediate()
        {
            for (int i = 0; i < StemCount; i++)
                if (stems[i] != null) { stems[i].Stop(); stems[i].volume = 0f; }
            stemsPlaying = false;
            for (int i = 0; i < LevelCount; i++) { levels[i] = 0f; levelTargets[i] = 0f; }
            if (crowdBedSource != null) { crowdBedSource.Stop(); crowdBedSource.volume = 0f; }
            for (int i = voices.Count - 1; i >= 0; i--) StopVoiceNow(voices[i]);
            voices.Clear();
            musicFade = 1f;
            musicFadeTarget = 1f;
        }

        // ---------------------------------------------------------------- Event

        private void OnEnable()
        {
            EventChannel<DirectorPhaseChangedEvent>.Raised += OnDirectorPhase;
            EventChannel<PlayerDamagedEvent>.Raised += OnPlayerDamaged;
            EventChannel<PlayerPinnedEvent>.Raised += OnPlayerPinned;
            EventChannel<BloodMoonWarningEvent>.Raised += OnBloodMoonWarning;
            EventChannel<BloodMoonStartedEvent>.Raised += OnBloodMoonStarted;
            EventChannel<BloodMoonEndedEvent>.Raised += OnBloodMoonEnded;
            EventChannel<DisasterStartedEvent>.Raised += OnDisasterStarted;
            EventChannel<DisasterEndedEvent>.Raised += OnDisasterEnded;
            EventChannel<DaybreakEvent>.Raised += OnDaybreak;
            EventChannel<NightfallEvent>.Raised += OnNightfall;
            EventChannel<CinematicStateChangedEvent>.Raised += OnCinematic;
            EventChannel<NarrativeStateChangedEvent>.Raised += OnNarrative;
            EventChannel<SleepStateChangedEvent>.Raised += OnSleep;
            EventChannel<PhaseChangedEvent>.Raised += OnPhaseChanged;
            EventChannel<RunEndedEvent>.Raised += OnRunEnded;

            // Kondisi saat ini dari event sticky.
            if (EventChannel<DirectorPhaseChangedEvent>.HasLast) directorPhase = EventChannel<DirectorPhaseChangedEvent>.Last.phase;
            if (EventChannel<PlayerPinnedEvent>.HasLast) isPinned = EventChannel<PlayerPinnedEvent>.Last.isPinned;
            if (EventChannel<SleepStateChangedEvent>.HasLast) isSleeping = EventChannel<SleepStateChangedEvent>.Last.isSleeping;
            if (EventChannel<CinematicStateChangedEvent>.HasLast) ApplyCinematic(EventChannel<CinematicStateChangedEvent>.Last);
            if (EventChannel<NarrativeStateChangedEvent>.HasLast) narrativePlaying = EventChannel<NarrativeStateChangedEvent>.Last.isPlaying;
            nightTarget = ReadIsNight() ? 1f : 0f;
            nightBlend = nightTarget;
            if (EventChannel<PhaseChangedEvent>.HasLast) ApplyPhase(EventChannel<PhaseChangedEvent>.Last.phase);
        }

        private void OnDisable()
        {
            EventChannel<DirectorPhaseChangedEvent>.Raised -= OnDirectorPhase;
            EventChannel<PlayerDamagedEvent>.Raised -= OnPlayerDamaged;
            EventChannel<PlayerPinnedEvent>.Raised -= OnPlayerPinned;
            EventChannel<BloodMoonWarningEvent>.Raised -= OnBloodMoonWarning;
            EventChannel<BloodMoonStartedEvent>.Raised -= OnBloodMoonStarted;
            EventChannel<BloodMoonEndedEvent>.Raised -= OnBloodMoonEnded;
            EventChannel<DisasterStartedEvent>.Raised -= OnDisasterStarted;
            EventChannel<DisasterEndedEvent>.Raised -= OnDisasterEnded;
            EventChannel<DaybreakEvent>.Raised -= OnDaybreak;
            EventChannel<NightfallEvent>.Raised -= OnNightfall;
            EventChannel<CinematicStateChangedEvent>.Raised -= OnCinematic;
            EventChannel<NarrativeStateChangedEvent>.Raised -= OnNarrative;
            EventChannel<SleepStateChangedEvent>.Raised -= OnSleep;
            EventChannel<PhaseChangedEvent>.Raised -= OnPhaseChanged;
            EventChannel<RunEndedEvent>.Raised -= OnRunEnded;
        }

        private static bool ReadIsNight()
        {
            bool hasNight = EventChannel<NightfallEvent>.HasLast;
            if (!hasNight) return false;
            if (!EventChannel<DaybreakEvent>.HasLast) return true;
            return EventChannel<NightfallEvent>.Last.day >= EventChannel<DaybreakEvent>.Last.day;
        }

        private void OnPhaseChanged(PhaseChangedEvent e) => ApplyPhase(e.phase);

        private void ApplyPhase(GamePhase phase)
        {
            if (stems[0] == null) return;   // Belum Initialize; Initialize membaca ulang fase dari event sticky
            bool active = phase == GamePhase.IslandSurvival;
            if (active == phaseActive) return;
            phaseActive = active;
            if (active)
            {
                // Setelah load, tingkat mulai dari Calm lalu naik dari kondisi saat itu (bagian 4 "Save dan load").
                runOver = false;
                CurrentTier = MusicTier.Calm;
                SetLevelTargets(MusicTier.Calm);
                for (int i = 0; i < LevelCount; i++) levels[i] = levelTargets[i];
                musicFade = cinematicMuted ? 0f : 1f;
                musicFadeTarget = musicFade;
                suppressStartStingerUntil = Time.unscaledTime + (Bal != null ? Bal.loadStingerSuppressSeconds : 2f);
                StartStems();
            }
            else StopImmediate();
        }

        private void OnRunEnded(RunEndedEvent e)
        {
            // Bagian 4 "Run berakhir": stem memudar dan tingkat dikunci.
            runOver = true;
            var b = Bal;
            FadeMusicTo(0f, b != null ? b.runEndFadeSeconds : 2f);
            for (int i = voices.Count - 1; i >= 0; i--) StopVoiceNow(voices[i]);
            voices.Clear();
        }

        private void OnDirectorPhase(DirectorPhaseChangedEvent e) => directorPhase = e.phase;

        private void OnPlayerDamaged(PlayerDamagedEvent e)
        {
            if (!phaseActive || runOver) return;
            var b = Bal;
            if (b == null) return;
            damageScore = Mathf.Min(1f, damageScore + e.info.amount * b.damageScorePerPoint);
            // Kena damage melewati penantian riseConfirmSeconds (UpdateTier langkah 2).
            damageBypass = true;
            EvaluateScore(b);
        }

        private void OnPlayerPinned(PlayerPinnedEvent e) => isPinned = e.isPinned;
        private void OnSleep(SleepStateChangedEvent e) => isSleeping = e.isSleeping;
        private void OnNarrative(NarrativeStateChangedEvent e) => narrativePlaying = e.isPlaying;

        private void OnDaybreak(DaybreakEvent e) => nightTarget = 0f;
        private void OnNightfall(NightfallEvent e) => nightTarget = 1f;

        private void OnBloodMoonWarning(BloodMoonWarningEvent e)
        {
            // Notifikasi milik HordeManager; di sini hanya suara peringatan.
            PlayStinger(bloodMoonWarningClip);
        }

        private void OnBloodMoonStarted(BloodMoonStartedEvent e)
        {
            // GDD bagian 3: Blood Moon setiap 10 hari. Saat restore (#50) event dikirim ulang; suara dilewati sesaat setelah load.
            if (Time.unscaledTime >= suppressStartStingerUntil) PlayStinger(bloodMoonStartClip);
            bloodMoonActive = true;
            UpdateFloor();
        }

        private void OnBloodMoonEnded(BloodMoonEndedEvent e)
        {
            bloodMoonActive = false;
            UpdateFloor();
        }

        private void OnDisasterStarted(DisasterStartedEvent e)
        {
            // GDD bagian 6: bencana. warningClip dan activeLoop tetap diputar DisasterManager.
            disasterActive = true;
            UpdateFloor();
        }

        private void OnDisasterEnded(DisasterEndedEvent e)
        {
            disasterActive = false;
            UpdateFloor();
        }

        private void UpdateFloor() => tierFloor = bloodMoonActive || disasterActive ? MusicTier.Alert : MusicTier.Calm;

        private void OnCinematic(CinematicStateChangedEvent e)
        {
            bool wasPlaying = cinematicPlaying;
            ApplyCinematic(e);
            if (wasPlaying && !e.isPlaying && phaseActive && !runOver)
            {
                // Selesai: score dihitung ulang, musik masuk kembali.
                var b = Bal;
                if (b != null) EvaluateScore(b);
                FadeMusicTo(1f, b != null ? b.cinematicFadeInSeconds : 2f);
            }
        }

        private void ApplyCinematic(CinematicStateChangedEvent e)
        {
            cinematicPlaying = e.isPlaying;
            cinematicMuted = e.isPlaying && !e.keepMusic;
            if (cinematicMuted)
            {
                var b = Bal;
                FadeMusicTo(0f, b != null ? b.cinematicFadeOutSeconds : 1f);
            }
        }

        private void FadeMusicTo(float target, float seconds)
        {
            musicFadeTarget = target;
            musicFadeSpeed = seconds > 0f ? 1f / seconds : float.MaxValue;
        }

        // ---------------------------------------------------------------- Update

        private void Update()
        {
            var b = Bal;
            if (b == null || !phaseActive) return;

            ListenerPosition();
            float dt = Time.deltaTime;            // Score dan timer ikut berhenti saat jeda
            float udt = Time.unscaledDeltaTime;   // Fade tidak tersangkut saat jeda (UIFocusStack.md)

            if (!runOver)
            {
                damageScore = Mathf.Max(0f, damageScore - b.damageScoreDecayPerSec * dt);
                evalTimer += dt;
                if (evalTimer >= b.evaluateIntervalSeconds)
                {
                    evalTimer = 0f;
                    EvaluateScore(b);
                }
                UpdateTier(dt, b);
            }

            FadeStems(udt, b);
            UpdateStinger(b);

            // Suara musuh berhenti saat jeda (AudioListener.pause), jadi slot tidak diperiksa.
            if (!AudioListener.pause)
            {
                UpdateVoices(udt, b);
                if (!runOver && !cinematicPlaying) TickEmitters(b);
                UpdateCrowdBed(udt, b);
            }
        }

        private void ListenerPosition()
        {
            var fp = FirstPersonCamera.Instance;
            if (fp != null && fp.WorldCamera != null) { listenerPos = fp.WorldCamera.transform.position; return; }
            var cam = Camera.main;
            if (cam != null) listenerPos = cam.transform.position;
        }

        // ---------------------------------------------------------------- Score

        private float PhaseFloor(CombatMusicBalance b)
        {
            switch (directorPhase)
            {
                case DirectorPhase.Relax: return b.phaseFloorRelax;
                case DirectorPhase.BuildUp: return b.phaseFloorBuildUp;
                case DirectorPhase.PeakFade: return b.phaseFloorPeakFade;
                case DirectorPhase.SustainPeak: return b.phaseFloorSustainPeak;
                default: return 0f;
            }
        }

        private void EvaluateScore(CombatMusicBalance b)
        {
            // Proximity: bobot emitter IsEngaged dalam nearRadius. Sekaligus menghitung musuh untuk suara kerumunan.
            float near2 = b.nearRadius * b.nearRadius;
            float weight = 0f;
            int engaged = 0;
            Vector3 sum = Vector3.zero;
            var list = ThreatAudioEmitter.Active;
            for (int i = 0; i < list.Count; i++)
            {
                var e = list[i];
                if (e == null || !e.IsEngaged) continue;
                Vector3 p = e.transform.position;
                engaged++;
                sum += p;
                if ((p - listenerPos).sqrMagnitude <= near2)
                    weight += e.Cues != null && e.Cues.isLarge ? b.largeThreatWeight : 1f;
            }
            engagedCount = engaged;
            engagedCentroid = engaged > 0 ? sum / engaged : listenerPos;

            // Tidur (#67): score dikunci ke 0 selama isSleeping.
            if (isSleeping) { score = 0f; return; }

            // Usulan disetujui developer: nilai terbesar dari musuh dekat, damage, Intensity, dan tahap Director,
            // supaya siang hari (Director Inactive) tetap punya musik pertempuran dari hewan.
            float proximity = b.nearSaturation > 0f ? Mathf.Min(1f, weight / b.nearSaturation) : 0f;
            float director = AIDirector.Instance != null ? AIDirector.Instance.Intensity : 0f;
            float phase = PhaseFloor(b);
            float pinned = isPinned ? b.pinnedScore : 0f;
            score = Mathf.Max(Mathf.Max(proximity, damageScore), Mathf.Max(Mathf.Max(director, phase), pinned));
        }

        // ---------------------------------------------------------------- Tingkat

        private MusicTier RawTier(float s, CombatMusicBalance b)
        {
            if (s >= b.peakThreshold) return MusicTier.Peak;
            if (s >= b.combatThreshold) return MusicTier.Combat;
            if (s >= b.alertThreshold) return MusicTier.Alert;
            return MusicTier.Calm;
        }

        private static float Threshold(MusicTier t, CombatMusicBalance b)
        {
            switch (t)
            {
                case MusicTier.Alert: return b.alertThreshold;
                case MusicTier.Combat: return b.combatThreshold;
                case MusicTier.Peak: return b.peakThreshold;
                default: return 0f;
            }
        }

        private void UpdateTier(float dt, CombatMusicBalance b)
        {
            // Adegan (#99) dan adegan naratif: tingkat dikunci.
            if (cinematicPlaying || narrativePlaying) { damageBypass = false; return; }

            MusicTier raw = RawTier(score, b);
            MusicTier target = raw > tierFloor ? raw : tierFloor;
            if (tierHoldTimer > 0f) tierHoldTimer -= dt;

            if (target > CurrentTier)
            {
                // 2. Naik: harus bertahan riseConfirmSeconds, boleh melompat beberapa tingkat. Damage melewati penantian.
                riseTimer += dt;
                if (riseTimer >= b.riseConfirmSeconds || damageBypass) SetTier(target, true, b);
            }
            else
            {
                riseTimer = 0f;
                // 3. Turun: di bawah ambang - hysteresis, setelah minTierHoldSeconds, satu tingkat setiap kali.
                if (target < CurrentTier && tierHoldTimer <= 0f && score < Threshold(CurrentTier, b) - b.hysteresis)
                    SetTier(CurrentTier - 1, false, b);
            }
            damageBypass = false;
        }

        private void SetTier(MusicTier t, bool rising, CombatMusicBalance b)
        {
            CurrentTier = t;
            tierHoldTimer = b.minTierHoldSeconds;
            riseTimer = 0f;
            // 4. FadeStems memakai riseFadeSeconds atau fallFadeSeconds.
            float seconds = rising ? b.riseFadeSeconds : b.fallFadeSeconds;
            stemFadeSpeed = seconds > 0f ? 1f / seconds : float.MaxValue;
            SetLevelTargets(t);
        }

        private void SetLevelTargets(MusicTier t)
        {
            var b = Bal;
            float calmUnderAlert = b != null ? b.alertCalmVolume : 0.5f;
            // Tabel "Lapisan musik".
            levelTargets[0] = t == MusicTier.Calm ? 1f : t == MusicTier.Alert ? calmUnderAlert : 0f;
            levelTargets[1] = t == MusicTier.Alert || t == MusicTier.Combat ? 1f : 0f;
            levelTargets[2] = t == MusicTier.Combat || t == MusicTier.Peak ? 1f : 0f;
            levelTargets[3] = t == MusicTier.Peak ? 1f : 0f;
        }

        // ---------------------------------------------------------------- Stem

        private void StartStems()
        {
            if (musicSet == null || stems[0] == null) return;
            var b = Bal;
            // Semua stem dimulai pada dspTime yang sama, sehingga ketukan tetap selaras. Pergantian tingkat hanya mengubah volume.
            double dsp = AudioSettings.dspTime + (b != null ? b.scheduleLeadSeconds : 0.1f);
            Schedule(CalmDay, musicSet.calmDay, dsp);
            Schedule(CalmNight, musicSet.calmNight, dsp);
            Schedule(AlertStem, musicSet.alertStem, dsp);
            Schedule(CombatStem, musicSet.combatStem, dsp);
            Schedule(PeakStem, musicSet.peakStem, dsp);
            stemsPlaying = true;
            WriteStemVolumes();
        }

        private void Schedule(int index, AudioClip clip, double dsp)
        {
            var s = stems[index];
            s.Stop();
            s.clip = clip;
            if (clip != null) s.PlayScheduled(dsp);
        }

        private void FadeStems(float udt, CombatMusicBalance b)
        {
            if (!stemsPlaying) return;
            for (int i = 0; i < LevelCount; i++) levels[i] = Mathf.MoveTowards(levels[i], levelTargets[i], stemFadeSpeed * udt);
            nightBlend = b.calmDayNightFadeSeconds > 0f ? Mathf.MoveTowards(nightBlend, nightTarget, udt / b.calmDayNightFadeSeconds) : nightTarget;
            musicFade = Mathf.MoveTowards(musicFade, musicFadeTarget, musicFadeSpeed * udt);
            float duckTarget = disasterActive ? Mathf.Pow(10f, b.disasterMusicDuckDb / 20f) : 1f;
            duckLinear = Mathf.MoveTowards(duckLinear, duckTarget, (b.riseFadeSeconds > 0f ? 1f / b.riseFadeSeconds : 1f) * udt);
            WriteStemVolumes();
        }

        // Volume pemain berlaku lewat grup Music (SettingsManager #19); di sini hanya posisi fade dan peredaman bencana.
        private void WriteStemVolumes()
        {
            float m = musicFade * duckLinear;
            stems[CalmDay].volume = levels[0] * (1f - nightBlend) * m;
            stems[CalmNight].volume = levels[0] * nightBlend * m;
            stems[AlertStem].volume = levels[1] * m;
            stems[CombatStem].volume = levels[2] * m;
            stems[PeakStem].volume = levels[3] * m;
        }

        // ---------------------------------------------------------------- Peringatan

        /// <summary>Putar lewat grup Warning. Peringatan yang terlalu rapat menunggu di antrean berisi satu klip.</summary>
        public void PlayStinger(AudioClip clip)
        {
            if (clip == null) return;
            var b = Bal;
            float interval = b != null ? b.stingerMinInterval : 3f;
            if (Time.time - lastStingerTime < interval) { queuedStinger = clip; return; }
            lastStingerTime = Time.time;
            if (AudioManager.Instance != null) AudioManager.Instance.PlayWarning(clip);
        }

        private void UpdateStinger(CombatMusicBalance b)
        {
            if (queuedStinger == null || Time.time - lastStingerTime < b.stingerMinInterval) return;
            var clip = queuedStinger;
            queuedStinger = null;
            PlayStinger(clip);
        }

        // ---------------------------------------------------------------- Suara musuh

        private void TickEmitters(CombatMusicBalance b)
        {
            var list = ThreatAudioEmitter.Active;
            int n = list.Count;
            if (n == 0) return;
            int budget = Mathf.Min(n, Mathf.Max(1, b.emitterTicksPerFrame));
            float now = Time.time;
            for (int k = 0; k < budget; k++)
            {
                if (emitterCursor >= list.Count) emitterCursor = 0;
                if (list.Count == 0) break;
                var e = list[emitterCursor++];
                if (e != null) e.Tick(now, listenerPos, this);
            }
        }

        /// <summary>Permintaan suara Idle atau Alert dari ThreatAudioEmitter.Tick.</summary>
        internal void RequestCue(ThreatAudioEmitter e, ThreatCueKind kind, Vector3 listener)
        {
            var cues = e.Cues;
            if (cues == null) return;
            var clips = kind == ThreatCueKind.Alert ? cues.alertClips : cues.idleClips;
            if (clips == null || clips.Length == 0) return;
            var clip = clips[Random.Range(0, clips.Length)];
            if (clip == null) return;
            TryPlayVoice(e, clip, cues.priority, false);
        }

        /// <summary>Suara serangan (PlayTell): priority + tellPriorityBonus, tanpa batas per varian.</summary>
        internal void RequestTell(ThreatAudioEmitter e, AudioClip clip)
        {
            if (!phaseActive || runOver || AudioListener.pause) return;
            var b = Bal;
            int bonus = b != null ? b.tellPriorityBonus : 2;
            int prio = (e.Cues != null ? e.Cues.priority : 1) + bonus;
            TryPlayVoice(e, clip, prio, true);
        }

        private void TryPlayVoice(ThreatAudioEmitter e, AudioClip clip, int priority, bool isTell)
        {
            var b = Bal;
            if (b == null || e.Source == null || !e.isActiveAndEnabled) return;
            if (!TryAcquireVoice(e, priority, clip, isTell, b, out var victim)) return;
            if (victim != null) BeginVoiceFade(victim, b.voiceStealFadeSeconds);

            var s = e.Source;
            s.Stop();
            s.clip = clip;
            s.volume = 1f;
            s.Play();
            e.hasVoice = true;
            e.voiceFading = false;
            e.voicePriority = priority;
            e.voiceIsTell = isTell;
            e.voiceClip = clip;
            e.voiceStartTime = Time.time;
            e.nextOcclusionTime = 0f;   // Diperiksa pada UpdateVoices berikutnya
            if (!voices.Contains(e)) voices.Add(e);
        }

        /// <summary>Batas suara musuh serentak (bagian "Batas suara zombie serentak").</summary>
        private bool TryAcquireVoice(ThreatAudioEmitter e, int priority, AudioClip clip, bool isTell, CombatMusicBalance b, out ThreatAudioEmitter victim)
        {
            victim = null;
            float now = Time.time;
            string id = e.EnemyId;
            int sameVariant = 0, live = 0;
            ThreatAudioEmitter lowest = null;
            float lowestValue = float.MaxValue;

            for (int i = 0; i < voices.Count; i++)
            {
                var v = voices[i];
                if (v == null || v.voiceFading) continue;
                // 1. Klip yang sama baru dimulai kurang dari sameClipMinSeconds lalu.
                if (v.voiceClip == clip && now - v.voiceStartTime < b.sameClipMinSeconds) return false;
                if (v == e) continue;   // Suara emitter ini sendiri akan diganti
                live++;
                if (v.EnemyId == id) sameVariant++;
                float value = v.voicePriority * 100f - Vector3.Distance(v.transform.position, listenerPos);
                if (value < lowestValue) { lowestValue = value; lowest = v; }
            }

            // Emitter yang sedang bersuara hanya diganti oleh suara dengan prioritas sama atau lebih tinggi.
            if (e.hasVoice && !e.voiceFading && e.Source.isPlaying && priority < e.voicePriority) return false;

            // 2. Batas per enemyId, kecuali PlayTell.
            if (!isTell && sameVariant >= b.maxSameVariantVoices) return false;
            // 3. Masih ada slot.
            if (live < b.maxEnemyVoices) return true;
            // 4. Penuh: gantikan suara terendah kalau nilai suara baru lebih tinggi.
            float newValue = priority * 100f - Vector3.Distance(e.transform.position, listenerPos);
            if (lowest != null && newValue > lowestValue) { victim = lowest; return true; }
            return false;
        }

        private static void BeginVoiceFade(ThreatAudioEmitter v, float seconds)
        {
            v.voiceFading = true;
            v.voiceFadeSpeed = seconds > 0f ? 1f / seconds : float.MaxValue;
        }

        private static void StopVoiceNow(ThreatAudioEmitter v)
        {
            if (v == null) return;
            if (v.Source != null) { v.Source.Stop(); v.Source.clip = null; }
            v.hasVoice = false;
            v.voiceFading = false;
            v.voiceClip = null;
        }

        /// <summary>Dipanggil ThreatAudioEmitter.OnReturned: slot suara dilepas.</summary>
        internal void ReleaseVoice(ThreatAudioEmitter e)
        {
            int i = voices.IndexOf(e);
            if (i >= 0) voices.RemoveAt(i);
        }

        private void UpdateVoices(float udt, CombatMusicBalance b)
        {
            float now = Time.time;
            for (int i = voices.Count - 1; i >= 0; i--)
            {
                var v = voices[i];
                if (v == null || v.Source == null) { voices.RemoveAt(i); continue; }
                var s = v.Source;
                if (v.voiceFading)
                {
                    s.volume = Mathf.MoveTowards(s.volume, 0f, v.voiceFadeSpeed * udt);
                    if (s.volume <= 0f) s.Stop();
                }
                if (!s.isPlaying)
                {
                    v.hasVoice = false;
                    v.voiceFading = false;
                    v.voiceClip = null;
                    voices.RemoveAt(i);
                    continue;
                }
                // Oklusi: musuh di balik dinding tetap terdengar, tetapi teredam.
                if (now >= v.nextOcclusionTime)
                {
                    v.nextOcclusionTime = now + b.occlusionCheckSeconds;
                    Vector3 to = v.transform.position - listenerPos;
                    float dist = to.magnitude;
                    bool blocked = dist > 0.01f && PhysicsQuery.Raycast(new Ray(listenerPos, to / dist), out _, dist, PhysicsMasks.AIVision);
                    if (v.Lowpass != null) v.Lowpass.cutoffFrequency = blocked ? b.occlusionLowpassHz : ThreatAudioEmitter.OpenCutoffHz;
                }
            }
        }

        // Suara kerumunan: mengisi horde besar tanpa menambah puluhan sumber, tidak dihitung dalam batas suara.
        private void UpdateCrowdBed(float udt, CombatMusicBalance b)
        {
            if (crowdBedSource == null || crowdLoopClip == null) return;
            bool on = !runOver && engagedCount >= b.crowdBedMinEngaged;
            float target = on ? Mathf.Clamp01((float)engagedCount / Mathf.Max(1, b.crowdBedFullEngaged)) : 0f;
            if (on && !crowdBedSource.isPlaying)
            {
                crowdBedSource.clip = crowdLoopClip;
                crowdBedSource.volume = 0f;
                crowdBedSource.Play();
            }
            crowdBedSource.transform.position = engagedCentroid;
            crowdBedSource.volume = Mathf.MoveTowards(crowdBedSource.volume, target, (b.riseFadeSeconds > 0f ? 1f / b.riseFadeSeconds : 1f) * udt);
            if (!on && crowdBedSource.isPlaying && crowdBedSource.volume <= 0f) crowdBedSource.Stop();
        }
    }
}
