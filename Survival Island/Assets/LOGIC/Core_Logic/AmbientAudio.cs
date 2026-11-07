using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace SurvivalIsland
{
    /// <summary>
    /// Suara latar berdasarkan tempat pemain berdiri, dengan crossfade dua deck lewat Audio Mixer.
    /// Blueprint: Core_Logic/AmbientAudio.md (#52). GDD bagian 5 (suasana tiap region) dan bagian 6 (pantai), secara
    /// tidak langsung; pemetaan suaranya usulan yang disetujui developer.
    ///
    /// Manager persisten di PersistentRoot, urutan bootstrap ke-5 (memakai mixer AudioManager).
    /// Aktif hanya saat GamePhase.IslandSurvival. Tidak menyimpan data ke save.
    /// </summary>
    public class AmbientAudio : MonoBehaviour, IBootstrapService
    {
        public static AmbientAudio Instance { get; private set; }

        [Header("Mixer (grup Ambient > AmbientA, AmbientB)")]
        [SerializeField] private AudioMixerGroup groupA;
        [SerializeField] private AudioMixerGroup groupB;
        [SerializeField] private string paramA = "AmbientA_Vol";
        [SerializeField] private string paramB = "AmbientB_Vol";
        [SerializeField] private string paramLowpass = "Ambient_Lowpass";

        [Header("Profil cadangan per RegionType (Volcano, Amazon, Snow, Logistic)")]
        [Tooltip("Usulan disetujui developer (#52): empat profil cadangan, supaya tidak ada area yang sunyi.")]
        [SerializeField] private AmbientProfile[] regionProfiles = new AmbientProfile[4];

        private const float NoLowpassHz = 22000f;
        private const float MinWeight = 0.0001f;
        private const float RetargetWeightLimit = 0.05f;   // Bobot deck masuk yang perlu diturunkan dulu (bagian 2 langkah 1)
        private const int MaxOneShotTimers = 8;

        private AudioMixer mixer;
        private AmbientDeck deckA, deckB;
        private readonly Dictionary<AmbientProfile, int> zoneCounts = new Dictionary<AmbientProfile, int>();

        private AmbientProfile currentProfile;
        private AmbientProfile pendingProfile;
        private bool hasPending;
        private float pendingTimer;

        // Kondisi dari event sticky.
        private bool isNight;
        private RegionType currentRegion;
        private bool hasRegion;
        private WeatherType currentWeather;
        private float tideLevel = 1f;
        private bool isUnderwater, isSleeping;
        private bool disasterActive, bloodMoonActive;
        private bool phaseActive;
        private bool runOver;   // RunEndedEvent sudah datang; area tidak lagi memulai crossfade sampai run berikutnya

        // Crossfade.
        private bool crossfading;
        private AmbientDeck incoming, outgoing;
        private AmbientProfile crossfadeTarget;
        private float crossT, crossStartT, outStartWeight, crossSeconds;
        private float duckTimer, duckStartWeight;
        private bool ducking;

        // Fade saat run berakhir.
        private float masterFade = 1f;
        private bool runEnding;

        // One-shot.
        private readonly float[] oneShotTimers = new float[MaxOneShotTimers];
        private AmbientProfile oneShotProfile;

        private static AmbientAudioBalance Bal => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.ambientAudio : null;

        public AmbientProfile CurrentProfile => currentProfile;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("[AmbientAudio] Instance ganda. Periksa prefab PersistentRoot.");
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
            mixer = AudioManager.Instance != null ? AudioManager.Instance.Mixer : null;
            if (mixer == null) Debug.LogWarning("[AmbientAudio] Mixer AudioManager kosong. Crossfade hanya lewat volume deck tidak tersedia.");
            if (deckA == null) deckA = new AmbientDeck(this, "AmbientDeckA", groupA, paramA);
            if (deckB == null) deckB = new AmbientDeck(this, "AmbientDeckB", groupB, paramB);
            ResetMixer();
            if (EventChannel<PhaseChangedEvent>.HasLast) ApplyPhase(EventChannel<PhaseChangedEvent>.Last.phase);
        }

        public void ResetForNewRun()
        {
            zoneCounts.Clear();
            currentProfile = null;
            pendingProfile = null;
            hasPending = false;
            runOver = false;
            phaseActive = false;   // Fase berikutnya dari PhaseChangedEvent memulai ulang deck
            StopImmediate();
        }

        /// <summary>Hentikan kedua deck seketika. Dipanggil AudioManager.StopWorldAudio kalau fade belum selesai.</summary>
        public void StopImmediate()
        {
            crossfading = false;
            ducking = false;
            runEnding = false;
            masterFade = 1f;
            oneShotProfile = null;
            if (deckA != null) { deckA.Stop(); deckA.weight = 0f; }
            if (deckB != null) { deckB.Stop(); deckB.weight = 0f; }
            currentProfile = null;
            ResetMixer();
        }

        private void ResetMixer()
        {
            WriteDeckWeight(deckA);
            WriteDeckWeight(deckB);
            if (mixer != null && !string.IsNullOrEmpty(paramLowpass)) mixer.SetFloat(paramLowpass, NoLowpassHz);
        }

        // ---------------------------------------------------------------- Event

        private void OnEnable()
        {
            EventChannel<PhaseChangedEvent>.Raised += OnPhaseChanged;
            EventChannel<RunEndedEvent>.Raised += OnRunEnded;
            EventChannel<RegionChangedEvent>.Raised += OnRegionChanged;
            EventChannel<DaybreakEvent>.Raised += OnDaybreak;
            EventChannel<NightfallEvent>.Raised += OnNightfall;
            EventChannel<WeatherChangedEvent>.Raised += OnWeatherChanged;
            EventChannel<TideChangedEvent>.Raised += OnTideChanged;
            EventChannel<SwimStateChangedEvent>.Raised += OnSwimStateChanged;
            EventChannel<SleepStateChangedEvent>.Raised += OnSleepStateChanged;
            EventChannel<DisasterStartedEvent>.Raised += OnDisasterStarted;
            EventChannel<DisasterEndedEvent>.Raised += OnDisasterEnded;
            EventChannel<BloodMoonStartedEvent>.Raised += OnBloodMoonStarted;
            EventChannel<BloodMoonEndedEvent>.Raised += OnBloodMoonEnded;
            AudioSettings.OnAudioConfigurationChanged += OnAudioConfigurationChanged;

            // Kondisi saat ini dari event sticky (AmbientAudio.md bagian 4 "Mulai run dan load").
            ReadStickyState();
        }

        private void OnDisable()
        {
            EventChannel<PhaseChangedEvent>.Raised -= OnPhaseChanged;
            EventChannel<RunEndedEvent>.Raised -= OnRunEnded;
            EventChannel<RegionChangedEvent>.Raised -= OnRegionChanged;
            EventChannel<DaybreakEvent>.Raised -= OnDaybreak;
            EventChannel<NightfallEvent>.Raised -= OnNightfall;
            EventChannel<WeatherChangedEvent>.Raised -= OnWeatherChanged;
            EventChannel<TideChangedEvent>.Raised -= OnTideChanged;
            EventChannel<SwimStateChangedEvent>.Raised -= OnSwimStateChanged;
            EventChannel<SleepStateChangedEvent>.Raised -= OnSleepStateChanged;
            EventChannel<DisasterStartedEvent>.Raised -= OnDisasterStarted;
            EventChannel<DisasterEndedEvent>.Raised -= OnDisasterEnded;
            EventChannel<BloodMoonStartedEvent>.Raised -= OnBloodMoonStarted;
            EventChannel<BloodMoonEndedEvent>.Raised -= OnBloodMoonEnded;
            AudioSettings.OnAudioConfigurationChanged -= OnAudioConfigurationChanged;
        }

        private void ReadStickyState()
        {
            if (EventChannel<RegionChangedEvent>.HasLast) { currentRegion = EventChannel<RegionChangedEvent>.Last.to; hasRegion = true; }
            if (EventChannel<WeatherChangedEvent>.HasLast)
            {
                var w = EventChannel<WeatherChangedEvent>.Last;
                if (!hasRegion || w.region == currentRegion) currentWeather = w.weather;
            }
            if (EventChannel<TideChangedEvent>.HasLast) tideLevel = Mathf.Clamp01(EventChannel<TideChangedEvent>.Last.level);
            if (EventChannel<SwimStateChangedEvent>.HasLast) isUnderwater = EventChannel<SwimStateChangedEvent>.Last.isUnderwater;
            if (EventChannel<SleepStateChangedEvent>.HasLast) isSleeping = EventChannel<SleepStateChangedEvent>.Last.isSleeping;
            isNight = ReadIsNight();
            if (EventChannel<PhaseChangedEvent>.HasLast) ApplyPhase(EventChannel<PhaseChangedEvent>.Last.phase);
        }

        // Event sticky yang terakhir terkirim menentukan siang atau malam.
        private static bool ReadIsNight()
        {
            bool hasNight = EventChannel<NightfallEvent>.HasLast;
            bool hasDay = EventChannel<DaybreakEvent>.HasLast;
            if (hasNight && !hasDay) return true;
            if (!hasNight) return false;
            // Keduanya ada: malam kalau hari NightfallEvent >= hari DaybreakEvent (malam datang setelah fajar hari yang sama).
            return EventChannel<NightfallEvent>.Last.day >= EventChannel<DaybreakEvent>.Last.day;
        }

        private void OnPhaseChanged(PhaseChangedEvent e) => ApplyPhase(e.phase);

        private void ApplyPhase(GamePhase phase)
        {
            if (deckA == null) return;   // Belum Initialize; Initialize membaca ulang fase dari event sticky
            bool active = phase == GamePhase.IslandSurvival;
            if (active == phaseActive) return;
            phaseActive = active;
            if (active)
            {
                // Mulai run atau load: langsung ke target tanpa zoneExitGraceSeconds (bagian 4).
                runEnding = false;
                runOver = false;
                masterFade = 1f;
                hasPending = false;
                var b = Bal;
                var target = ComputeTarget();
                if (target != null) BeginCrossfade(target, b != null ? b.startCrossfadeSeconds : 0.5f);
            }
            else
            {
                // Fase 1: deck diam, suara kapal milik ShipHazardManager (#75).
                StopImmediate();
            }
        }

        private void OnRunEnded(RunEndedEvent e)
        {
            // Fade runEndFadeSeconds, one-shot berhenti (bagian 2 "Pengubah lain").
            runEnding = true;
            runOver = true;
            hasPending = false;
            oneShotProfile = null;
        }

        private void OnRegionChanged(RegionChangedEvent e)
        {
            currentRegion = e.to;
            hasRegion = true;
            if (EventChannel<WeatherChangedEvent>.HasLast && EventChannel<WeatherChangedEvent>.Last.region == currentRegion)
                currentWeather = EventChannel<WeatherChangedEvent>.Last.weather;
            ResolveTarget();
        }

        private void OnDaybreak(DaybreakEvent e) => isNight = false;
        private void OnNightfall(NightfallEvent e) => isNight = true;

        private void OnWeatherChanged(WeatherChangedEvent e)
        {
            // Hanya cuaca region tempat pemain berada.
            if (hasRegion && e.region != currentRegion) return;
            currentWeather = e.weather;
        }

        private void OnTideChanged(TideChangedEvent e) => tideLevel = Mathf.Clamp01(e.level);

        private void OnSwimStateChanged(SwimStateChangedEvent e)
        {
            isUnderwater = e.isUnderwater;
            if (mixer == null || string.IsNullOrEmpty(paramLowpass)) return;
            var b = Bal;
            mixer.SetFloat(paramLowpass, isUnderwater && b != null ? b.underwaterLowpassHz : NoLowpassHz);
        }

        private void OnSleepStateChanged(SleepStateChangedEvent e) => isSleeping = e.isSleeping;
        private void OnDisasterStarted(DisasterStartedEvent e) => disasterActive = true;
        private void OnDisasterEnded(DisasterEndedEvent e) => disasterActive = false;
        private void OnBloodMoonStarted(BloodMoonStartedEvent e) => bloodMoonActive = true;
        private void OnBloodMoonEnded(BloodMoonEndedEvent e) => bloodMoonActive = false;

        // Kartu suara berubah: Unity memuat ulang audio, jadi klip deck aktif dimuat ulang (bagian 4).
        private void OnAudioConfigurationChanged(bool deviceWasChanged)
        {
            if (deckA != null && deckA.profile != null) deckA.Load(deckA.profile);
            if (deckB != null && deckB.profile != null) deckB.Load(deckB.profile);
        }

        // ---------------------------------------------------------------- Area

        public void EnterZone(AmbientProfile profile)
        {
            if (profile == null) return;
            zoneCounts.TryGetValue(profile, out int c);
            zoneCounts[profile] = c + 1;
            ResolveTarget();
        }

        public void ExitZone(AmbientProfile profile)
        {
            if (profile == null || !zoneCounts.TryGetValue(profile, out int c)) return;
            if (c <= 1) zoneCounts.Remove(profile);
            else zoneCounts[profile] = c - 1;
            ResolveTarget();
        }

        private AmbientProfile ComputeTarget()
        {
            // 1. Area dengan priority terbesar. Usulan disetujui developer: pantai (20) mengalahkan hutan (10).
            AmbientProfile best = null;
            foreach (var kv in zoneCounts)
            {
                if (kv.Value <= 0 || kv.Key == null) continue;
                if (best == null || kv.Key.priority > best.priority) best = kv.Key;
            }
            if (best != null) return best;
            // 2. Profil cadangan region.
            int r = (int)currentRegion;
            if (hasRegion && regionProfiles != null && r >= 0 && r < regionProfiles.Length) return regionProfiles[r];
            return null;
        }

        private void ResolveTarget()
        {
            if (!phaseActive || runOver) return;
            var target = ComputeTarget();
            var reference = crossfading ? crossfadeTarget : currentProfile;
            // 3. Sama dengan profil aktif: batalkan penundaan.
            if (target == reference) { hasPending = false; pendingProfile = null; return; }
            // 4. Berbeda: tunggu jeda yang sama untuk masuk dan keluar, supaya garis batas tidak membuat suara bolak-balik.
            var b = Bal;
            pendingProfile = target;
            hasPending = true;
            pendingTimer = b != null ? b.zoneExitGraceSeconds : 0f;
        }

        // ---------------------------------------------------------------- Update

        private void Update()
        {
            if (deckA == null || deckB == null) return;
            var b = Bal;
            if (b == null) return;
            float dt = Time.deltaTime;   // Ikut berhenti saat jeda (bagian 4)

            if (phaseActive && !runOver && hasPending)
            {
                pendingTimer -= dt;
                if (pendingTimer <= 0f)
                {
                    hasPending = false;
                    BeginCrossfade(pendingProfile, b.crossfadeSeconds);
                    pendingProfile = null;
                }
            }

            UpdateCrossfade(dt, b);

            if (runEnding)
            {
                masterFade = b.runEndFadeSeconds > 0f ? Mathf.MoveTowards(masterFade, 0f, dt / b.runEndFadeSeconds) : 0f;
                if (masterFade <= 0f) StopImmediate();
            }

            UpdateLayerVolumes(deckA, dt, b);
            UpdateLayerVolumes(deckB, dt, b);
            WriteDeckWeight(deckA);
            WriteDeckWeight(deckB);

            if (phaseActive && !runOver) UpdateOneShots(dt, b);
        }

        // ---------------------------------------------------------------- Crossfade

        private void BeginCrossfade(AmbientProfile profile, float seconds)
        {
            if (deckA == null || deckB == null) return;
            crossfadeTarget = profile;
            crossSeconds = Mathf.Max(0.01f, seconds);

            // Deck yang sudah memutar profil ini dipakai lagi sebagai deck masuk, tanpa memuat ulang klip.
            if (deckA.profile == profile && profile != null) { incoming = deckA; outgoing = deckB; }
            else if (deckB.profile == profile && profile != null) { incoming = deckB; outgoing = deckA; }
            else
            {
                // 1. Deck dengan bobot lebih kecil menjadi deck masuk.
                incoming = deckA.weight <= deckB.weight ? deckA : deckB;
                outgoing = incoming == deckA ? deckB : deckA;
                var b = Bal;
                if (incoming.weight > RetargetWeightLimit && b != null && b.retargetDuckSeconds > 0f)
                {
                    // Turunkan dulu, supaya penggantian klip tidak terdengar terputus.
                    ducking = true;
                    duckTimer = b.retargetDuckSeconds;
                    duckStartWeight = incoming.weight;
                }
                else
                {
                    ducking = false;
                    incoming.weight = 0f;
                    incoming.Load(profile);
                }
            }

            // Mulai dari bobot saat ini, sehingga tidak ada lonjakan volume.
            crossStartT = ducking ? 0f : Mathf.Asin(Mathf.Clamp01(incoming.weight)) / (Mathf.PI * 0.5f);
            crossT = crossStartT;
            outStartWeight = outgoing.weight;
            crossfading = true;
        }

        private void UpdateCrossfade(float dt, AmbientAudioBalance b)
        {
            if (!crossfading) return;

            if (ducking)
            {
                duckTimer -= dt;
                incoming.weight = b.retargetDuckSeconds > 0f ? duckStartWeight * Mathf.Clamp01(duckTimer / b.retargetDuckSeconds) : 0f;
                if (duckTimer > 0f) return;
                ducking = false;
                incoming.weight = 0f;
                incoming.Load(crossfadeTarget);
                crossStartT = 0f;
                crossT = 0f;
                outStartWeight = outgoing.weight;
            }

            // 3-4. Kurva daya sama, supaya volume total tidak turun di tengah transisi.
            crossT = Mathf.MoveTowards(crossT, 1f, dt / crossSeconds);
            incoming.weight = crossfadeTarget != null ? Mathf.Sin(crossT * Mathf.PI * 0.5f) : 0f;
            float u = crossStartT < 1f ? Mathf.Clamp01((crossT - crossStartT) / (1f - crossStartT)) : 1f;
            outgoing.weight = outStartWeight * Mathf.Cos(u * Mathf.PI * 0.5f);

            if (crossT >= 1f)
            {
                // 6. Hentikan deck keluar.
                outgoing.weight = 0f;
                outgoing.Stop();
                if (crossfadeTarget == null) { incoming.Stop(); incoming.weight = 0f; }
                currentProfile = crossfadeTarget;
                crossfading = false;
            }
        }

        private void WriteDeckWeight(AmbientDeck deck)
        {
            if (deck == null || mixer == null || string.IsNullOrEmpty(deck.param)) return;
            // 5. Tulis dalam dB. Fade run berakhir dan peredaman tidur ikut ditambahkan di sini (lihat Catatan Implementasi).
            float w = Mathf.Max(deck.weight * masterFade, MinWeight);
            float db = 20f * Mathf.Log10(w);
            var b = Bal;
            if (isSleeping && b != null) db += b.sleepDuckDb;
            mixer.SetFloat(deck.param, Mathf.Max(-80f, db));
        }

        // ---------------------------------------------------------------- Volume layer

        private bool IsBadWeather => currentWeather == WeatherType.Rain || currentWeather == WeatherType.Storm || currentWeather == WeatherType.Blizzard;

        private void UpdateLayerVolumes(AmbientDeck deck, float dt, AmbientAudioBalance b)
        {
            var profile = deck.profile;
            if (profile == null || profile.layers == null) return;
            float speed = b.dayNightBlendSeconds > 0f ? dt / b.dayNightBlendSeconds : 1f;
            int n = Mathf.Min(profile.layers.Length, AmbientProfile.MaxLayers);
            for (int i = 0; i < n; i++)
            {
                var src = deck.sources[i];
                if (src.clip == null) continue;
                src.volume = Mathf.MoveTowards(src.volume, LayerTarget(profile.layers[i], b), speed);
            }
        }

        private float LayerTarget(AmbientLayer layer, AmbientAudioBalance b)
        {
            if (layer == null) return 0f;
            float target = isNight ? layer.nightVolume : layer.dayVolume;
            if (layer.followsTide) target *= Mathf.Lerp(b.tideMinVolume, 1f, tideLevel);
            if (layer.mutedByBadWeather && IsBadWeather) target = 0f;
            // Usulan disetujui developer (#52): jangkrik hilang saat Blood Moon dan bencana, malam terasa sunyi dan mengancam.
            if (layer.mutedByThreatEvent && (disasterActive || bloodMoonActive)) target = 0f;
            return target;
        }

        // ---------------------------------------------------------------- One-shot

        private void UpdateOneShots(float dt, AmbientAudioBalance b)
        {
            // One-shot mengikuti deck yang paling keras.
            var deck = deckA.weight >= deckB.weight ? deckA : deckB;
            var profile = deck.weight > RetargetWeightLimit ? deck.profile : null;
            if (profile != oneShotProfile)
            {
                oneShotProfile = profile;
                if (profile != null && profile.oneShots != null)
                    for (int i = 0; i < profile.oneShots.Length && i < MaxOneShotTimers; i++)
                        oneShotTimers[i] = NextInterval(profile.oneShots[i]);
            }
            if (profile == null || profile.oneShots == null) return;

            for (int i = 0; i < profile.oneShots.Length && i < MaxOneShotTimers; i++)
            {
                var shot = profile.oneShots[i];
                if (shot == null) continue;
                oneShotTimers[i] -= dt;
                if (oneShotTimers[i] > 0f) continue;
                oneShotTimers[i] = NextInterval(shot);
                if (shot.dayOnly && isNight) continue;
                if (IsBadWeather) continue;
                PlayOneShot(shot, deck, b);
            }
        }

        // Suara tidak memengaruhi gameplay, jadi memakai UnityEngine.Random biasa tanpa seed (#89).
        private static float NextInterval(AmbientOneShot shot) =>
            shot == null ? 1f : Random.Range(Mathf.Max(0.1f, shot.minInterval), Mathf.Max(shot.minInterval, shot.maxInterval));

        private void PlayOneShot(AmbientOneShot shot, AmbientDeck deck, AmbientAudioBalance b)
        {
            if (shot.clips == null || shot.clips.Length == 0) return;
            var clip = shot.clips[Random.Range(0, shot.clips.Length)];
            if (clip == null) return;
            Vector3 center = ListenerPosition();
            float angle = Random.value * Mathf.PI * 2f;
            Vector3 pos = center + new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * shot.radius
                        + Vector3.up * Random.Range(b.oneShotHeightMin, b.oneShotHeightMax);
            // Suara 3D, sehingga camar terdengar dari arah tertentu.
            PooledAudioSource.PlayAt(clip, pos, deck.group);
        }

        private static Vector3 ListenerPosition()
        {
            var fp = FirstPersonCamera.Instance;
            if (fp != null && fp.WorldCamera != null) return fp.WorldCamera.transform.position;
            var cam = Camera.main;
            return cam != null ? cam.transform.position : Vector3.zero;
        }

        // ---------------------------------------------------------------- Deck

        /// <summary>Satu jalur crossfade: tiga AudioSource 2D loop yang keluar ke grup AmbientA atau AmbientB.</summary>
        private sealed class AmbientDeck
        {
            public readonly AudioSource[] sources = new AudioSource[AmbientProfile.MaxLayers];
            public readonly string param;
            public readonly AudioMixerGroup group;
            public AmbientProfile profile;
            public float weight;
            private readonly AmbientAudio owner;

            public AmbientDeck(AmbientAudio owner, string name, AudioMixerGroup group, string param)
            {
                this.owner = owner;
                this.group = group;
                this.param = param;
                // Dibuat sekali saat Initialize, bukan saat bermain.
                var go = new GameObject(name);
                go.transform.SetParent(owner.transform, false);
                for (int i = 0; i < sources.Length; i++)
                {
                    var s = go.AddComponent<AudioSource>();
                    s.playOnAwake = false;
                    s.loop = true;
                    s.spatialBlend = 0f;
                    s.outputAudioMixerGroup = group;
                    sources[i] = s;
                }
            }

            /// <summary>2. Isi klip, acak AudioSource.time, dan langsung ke volume target layer.</summary>
            public void Load(AmbientProfile p)
            {
                profile = p;
                var b = Bal;
                for (int i = 0; i < sources.Length; i++)
                {
                    var s = sources[i];
                    var layer = p != null && p.layers != null && i < p.layers.Length ? p.layers[i] : null;
                    s.Stop();
                    if (layer == null || layer.clip == null) { s.clip = null; continue; }
                    s.clip = layer.clip;
                    s.volume = b != null ? owner.LayerTarget(layer, b) : layer.dayVolume;
                    s.time = Random.Range(0f, layer.clip.length * 0.99f);
                    s.Play();
                }
            }

            public void Stop()
            {
                profile = null;
                for (int i = 0; i < sources.Length; i++)
                {
                    sources[i].Stop();
                    sources[i].clip = null;
                }
            }
        }
    }
}
