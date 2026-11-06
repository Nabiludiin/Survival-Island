using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Menampilkan cuaca di tempat pemain lewat partikel dan audio yang menempel pada kamera.
    /// Blueprint: Island_Logic/WeatherManager.md (#115, melengkapi #65, #89, #92, #101).
    /// GDD bagian 5 (salju Region 3) dan bagian 6 (Badai Tropis). Tampilan per cuaca adalah usulan disetujui developer.
    ///
    /// Tidak mengacak cuaca, tidak menulis RenderSettings atau Light, dan tidak memadamkan sumber cahaya.
    /// Satu-satunya pemanggil Present adalah RegionalWeather.
    ///
    /// isUnderwater dari SwimStateChangedEvent (#13), dan kepadatan partikel dikali VFXManager.densityMultiplier (#15).
    /// </summary>
    public class WeatherManager : MonoBehaviour
    {
        private const int ChRain = 0, ChLeaves = 1, ChSnow = 2, ChBlizzard = 3, ChannelCount = 4;

        public static WeatherManager Instance { get; private set; }

        /// <summary>Satu saluran partikel. Array tetap, jadi crossfade tanpa alokasi.</summary>
        private struct WeatherChannel
        {
            public ParticleSystem system;   // null untuk saluran pool yang belum diambil
            public GameObject pooled;       // Objek pool, untuk ReturnToPool
            public string poolTag;          // Kosong untuk partikel scene
            public float baseRate;          // rateOverTime asli dari prefab
            public float weight, target;    // 0 sampai 1
        }

        [Tooltip("Tempat partikel menempel. Kosong = Camera.main.")]
        [SerializeField] private Transform followTarget;
        [SerializeField] private ParticleSystem rainParticles;
        [SerializeField] private ParticleSystem stormLeavesParticles;
        [SerializeField] private string snowPoolTag = "Fx_Snow";            // Usulan nama tag, beda dengan Fx_SnowDust milik suasana
        [SerializeField] private string blizzardPoolTag = "Fx_Blizzard";

        [Header("Audio (grup mixer Ambient, loop, spatialBlend 0)")]
        [SerializeField] private AudioClip rainLoop;
        [SerializeField] private AudioClip stormLoop;
        [SerializeField] private AudioClip windLightLoop;
        [SerializeField] private AudioClip windStrongLoop;
        [SerializeField] private AudioSource deckA;
        [SerializeField] private AudioSource deckB;

        private readonly WeatherChannel[] channels = new WeatherChannel[ChannelCount];
        private WeatherType shown = WeatherType.Clear;
        private bool isInside;
        private bool isUnderwater;
        private float blendSeconds = 6f;
        private float targetA, targetB;
        private bool idle = true;

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;

        // Tabel tampilan per cuaca: bobot hujan, daun, salju, badai salju (relatif terhadap baseRate prefab).
        private static readonly float[,] displayTable =
        {
            // Rain, Leaves, Snow, Blizzard
            { 0f,   0f, 0f,   0f },   // Clear
            { 1f,   0f, 0f,   0f },   // Rain
            { 0f,   0f, 0f,   0f },   // Fog
            { 1.5f, 1f, 0f,   0f },   // Storm
            { 0f,   0f, 1f,   0f },   // Snow
            { 0f,   0f, 0.5f, 1f },   // Blizzard
        };

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            Instance = this;
            if (followTarget == null && Camera.main != null) followTarget = Camera.main.transform;
            channels[ChRain] = SceneChannel(rainParticles);
            channels[ChLeaves] = SceneChannel(stormLeavesParticles);
            channels[ChSnow] = new WeatherChannel { poolTag = snowPoolTag };
            channels[ChBlizzard] = new WeatherChannel { poolTag = blizzardPoolTag };
            PrepareDeck(deckA);
            PrepareDeck(deckB);
        }

        private void OnEnable()
        {
            EventChannel<SwimStateChangedEvent>.Raised += HandleSwim;   // #13, sticky
            if (EventChannel<SwimStateChangedEvent>.HasLast) SetUnderwater(EventChannel<SwimStateChangedEvent>.Last.isUnderwater);
        }

        private void OnDisable()
        {
            EventChannel<SwimStateChangedEvent>.Raised -= HandleSwim;
            // Partikel pool yang masih menjadi anak kamera ikut hancur saat IslandPhase dilepas. Kembalikan dulu ke pool.
            for (int i = 0; i < ChannelCount; i++) ReleasePooled(i);
            if (deckA != null) deckA.Stop();
            if (deckB != null) deckB.Stop();
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private static WeatherChannel SceneChannel(ParticleSystem ps)
        {
            var c = new WeatherChannel { system = ps, poolTag = string.Empty };
            if (ps != null)
            {
                var em = ps.emission;
                c.baseRate = em.rateOverTimeMultiplier;
                em.rateOverTimeMultiplier = 0f;
            }
            return c;
        }

        private static void PrepareDeck(AudioSource s)
        {
            if (s == null) return;
            s.loop = true;
            s.spatialBlend = 0f;
            s.volume = 0f;
            s.playOnAwake = false;
        }

        // ---------------------------------------------------------------- Menerima perintah tampil

        /// <summary>Dipanggil RegionalWeather.ApplyActive. Nilai sama tidak memulai crossfade baru.</summary>
        public void Present(WeatherType w, float blendSeconds, bool isPlayerInside)
        {
            var bal = Balance;
            // 1.
            if (w == shown && isPlayerInside == isInside) return;

            // 2-3.
            bool weatherChanged = w != shown;
            shown = w;
            isInside = isPlayerInside;
            this.blendSeconds = !weatherChanged
                ? (bal != null ? bal.weather.insideBlendSeconds : 1.5f)
                : Mathf.Max(blendSeconds, 0.01f);

            // 4-5.
            RefreshTargets();

            // 6.
            SwitchAudio(ClipFor(w), VolumeFor(w));
            idle = false;
        }

        private void RefreshTargets()
        {
            // Usulan disetujui: di dalam ruangan dan di bawah air semua partikel mati.
            bool hide = isInside || isUnderwater;
            for (int i = 0; i < ChannelCount; i++)
            {
                channels[i].target = hide ? 0f : displayTable[(int)shown, i];
                if (channels[i].target > 0f && channels[i].system == null && !string.IsNullOrEmpty(channels[i].poolTag))
                    AcquirePooled(i);
            }
            idle = false;
        }

        private void AcquirePooled(int i)
        {
            var pool = ObjectPooler.Instance;
            if (pool == null || followTarget == null) return;
            // 1. Pool habis = null. Cuaca tetap jalan tanpa partikel itu.
            var go = pool.SpawnFromPool(channels[i].poolTag, followTarget.position, followTarget.rotation);
            if (go == null) return;
            // 2.
            go.transform.SetParent(followTarget, false);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            // 3. Bobot lama tidak terbawa.
            var ps = go.GetComponent<ParticleSystem>();
            if (ps == null)
            {
                pool.ReturnToPool(channels[i].poolTag, go);
                return;
            }
            channels[i].pooled = go;
            channels[i].system = ps;
            channels[i].baseRate = ps.emission.rateOverTimeMultiplier;
            channels[i].weight = 0f;
            var em = ps.emission;
            em.rateOverTimeMultiplier = 0f;
        }

        private void ReleasePooled(int i)
        {
            if (channels[i].pooled == null) return;
            var go = channels[i].pooled;
            channels[i].pooled = null;
            channels[i].system = null;
            channels[i].weight = 0f;
            if (go == null) return;
            go.transform.SetParent(null, true);
            if (ObjectPooler.Instance != null) ObjectPooler.Instance.ReturnToPool(channels[i].poolTag, go);
        }

        // ---------------------------------------------------------------- Crossfade

        private void Update()
        {
            if (idle) return;
            // Time.deltaTime: crossfade berhenti saat game dijeda.
            float step = Time.deltaTime / Mathf.Max(0.01f, blendSeconds);
            float density = VFXManager.Instance != null ? VFXManager.Instance.densityMultiplier : 1f;   // #15
            bool settled = true;

            // 1-4. Partikel.
            for (int i = 0; i < ChannelCount; i++)
            {
                ref var c = ref channels[i];
                c.weight = Mathf.MoveTowards(c.weight, c.target, step);
                if (c.system != null)
                {
                    var em = c.system.emission;
                    em.rateOverTimeMultiplier = c.baseRate * c.weight * density;
                }
                if (c.pooled != null && c.weight <= 0f && c.target <= 0f) ReleasePooled(i);
                if (!Mathf.Approximately(c.weight, c.target)) settled = false;
            }

            // 5. Audio digeser dengan langkah yang sama.
            settled &= StepDeck(deckA, targetA, step);
            settled &= StepDeck(deckB, targetB, step);

            // Tidak bekerja setiap frame saat cuaca tetap. Bukan enabled = false, karena OnDisable mengembalikan pool.
            if (settled) idle = true;
        }

        private static bool StepDeck(AudioSource s, float target, float step)
        {
            if (s == null) return true;
            s.volume = Mathf.MoveTowards(s.volume, target, step);
            if (s.volume <= 0f && target <= 0f && s.isPlaying)
            {
                s.Stop();
                s.clip = null;
            }
            return Mathf.Approximately(s.volume, target);
        }

        // ---------------------------------------------------------------- Audio

        private void SwitchAudio(AudioClip clip, float volume)
        {
            var bal = Balance;
            float baseVol = bal != null ? bal.weather.audioVolume : 0.8f;
            float insideMul = bal != null ? bal.weather.insideVolumeMultiplier : 0.3f;
            // 4.
            float target = volume * baseVol * (isInside ? insideMul : 1f);

            // 1. Deck lama = deck yang sedang naik atau paling keras. Deck baru = yang diam atau sedang memudar.
            bool aIsOld = targetA > 0f || (targetB <= 0f && deckA != null && deckA.volume >= (deckB != null ? deckB.volume : 0f));
            AudioSource oldDeck = aIsOld ? deckA : deckB;
            AudioSource newDeck = aIsOld ? deckB : deckA;

            // 2. Klip sama: cukup ubah volume, tanpa memulai ulang loop.
            if (oldDeck != null && clip != null && oldDeck.clip == clip)
            {
                SetTarget(oldDeck, target);
                SetTarget(newDeck, 0f);
                return;
            }

            // 3. Klip berbeda: deck lama menuju 0, deck baru mulai dari 0. Paling banyak dua loop berbunyi.
            SetTarget(oldDeck, 0f);
            if (newDeck == null) return;
            if (clip == null)
            {
                SetTarget(newDeck, 0f);
                return;
            }
            if (newDeck.clip != clip)
            {
                newDeck.Stop();
                newDeck.clip = clip;
                newDeck.volume = 0f;
            }
            if (!newDeck.isPlaying) newDeck.Play();
            SetTarget(newDeck, target);
        }

        private void SetTarget(AudioSource deck, float value)
        {
            if (deck == null) return;
            if (deck == deckA) targetA = value;
            else if (deck == deckB) targetB = value;
        }

        private AudioClip ClipFor(WeatherType w)
        {
            switch (w)
            {
                case WeatherType.Rain: return rainLoop;
                case WeatherType.Fog: return windLightLoop;
                case WeatherType.Storm: return stormLoop;
                case WeatherType.Snow: return windLightLoop;
                case WeatherType.Blizzard: return windStrongLoop;
                default: return null;
            }
        }

        private static float VolumeFor(WeatherType w)
        {
            switch (w)
            {
                case WeatherType.Clear: return 0f;
                case WeatherType.Fog: return 0.4f;
                default: return 1f;
            }
        }

        // ---------------------------------------------------------------- Di bawah air

        // Di bawah air (#13): partikel cuaca disembunyikan. Audio tidak diubah, low-pass milik AmbientAudio.
        private void HandleSwim(SwimStateChangedEvent e) => SetUnderwater(e.isUnderwater);

        /// <summary>Juga bisa dipanggil langsung (DevConsole, scene uji).</summary>
        public void SetUnderwater(bool value)
        {
            if (isUnderwater == value) return;
            isUnderwater = value;
            RefreshTargets();
        }
    }
}
