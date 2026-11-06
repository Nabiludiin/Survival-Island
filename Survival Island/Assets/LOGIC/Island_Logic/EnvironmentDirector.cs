using System;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Lapisan event suasana (EnvironmentProfile.md "Lapisan event").</summary>
    public enum EnvEventId { BloodMoon, Eruption }

    /// <summary>Lapisan fog dan cahaya satu WeatherType. Baris di GameBalanceConfig.environment.weatherLayers.</summary>
    [Serializable]
    public struct WeatherEnvironmentLayer
    {
        public WeatherType weather;          // Label. Baris dibaca menurut urutan WeatherType
        public float addFogDensity;
        public float sunMultiplier;
        public float ambientMultiplier;
        public Color fogColor;
        public float fogColorWeight;

        public WeatherEnvironmentLayer(WeatherType weather, float addFog, float sun, float ambient, Color fogColor, float fogColorWeight)
        {
            this.weather = weather;
            addFogDensity = addFog;
            sunMultiplier = sun;
            ambientMultiplier = ambient;
            this.fogColor = fogColor;
            this.fogColorWeight = fogColorWeight;
        }
    }

    /// <summary>Lapisan satu event (Blood Moon, erupsi). Baris di GameBalanceConfig.environment.eventLayers.</summary>
    [Serializable]
    public struct EnvironmentEventLayer
    {
        public EnvEventId id;
        public int priority;
        public Color fogColor;
        public float addFogDensity;
        public float sunMultiplier;
        public Color sunColor;
        public float blendInSeconds;   // -1 = environment.eventBlendSeconds
        public float blendOutSeconds;  // -1 = environment.eventBlendSeconds

        public EnvironmentEventLayer(EnvEventId id, int priority, Color fogColor, float addFog, float sunMultiplier, Color sunColor,
            float blendIn, float blendOut)
        {
            this.id = id;
            this.priority = priority;
            this.fogColor = fogColor;
            addFogDensity = addFog;
            this.sunMultiplier = sunMultiplier;
            this.sunColor = sunColor;
            blendInSeconds = blendIn;
            blendOutSeconds = blendOut;
        }
    }

    /// <summary>
    /// Menggabungkan suasana region, jam, cuaca, dan event menjadi satu hasil, lalu menjadi SATU-SATUNYA penulis
    /// RenderSettings.fog*, RenderSettings.ambientLight, RenderSettings.skybox, dan Light matahari.
    /// Blueprint: Island_Logic/EnvironmentProfile.md (#92). GDD bagian 5 (suasana tiap region, Region 2 gelap meski siang)
    /// dan bagian 3 (siang dan malam, langit merah Blood Moon). Urutan gabungan usulan disetujui: region, jam, cuaca, event, interior.
    ///
    /// Belum ada (TODO): InteriorZoneChangedEvent dan lightProfile (#112), SettingsManager.brightness (#19, sementara 1),
    /// PostProcessController.SetRegionProfile (#23).
    /// </summary>
    public class EnvironmentDirector : MonoBehaviour
    {
        private const int RegionCount = 4;
        private const int EventCount = 2;

        public static EnvironmentDirector Instance { get; private set; }

        private struct EnvState
        {
            public Color fogColor;
            public float fogDensity;
            public Color ambient;
            public Color sunColor;
            public float sunIntensity;
        }

        [Tooltip("Satu profil per RegionType, urutan Volcano, Amazon, Snow, Logistic.")]
        [SerializeField] private RegionEnvironmentProfile[] profiles = new RegionEnvironmentProfile[RegionCount];
        [Tooltip("Directional Light matahari dan bulan. DayNightDirector tidak menulis Light.")]
        [SerializeField] private Light sun;
        [Tooltip("Material SkyboxBlended (_TexA, _TexB, _Blend). Boleh kosong.")]
        [SerializeField] private Material skyboxBlended;

        // Region
        private EnvState regionFrom;
        private RegionType activeRegion = RegionType.Volcano;
        private float regionT = 1f;
        private Texture skyFrom;
        // Cuaca
        private WeatherType weatherFrom = WeatherType.Clear, weatherTo = WeatherType.Clear;
        private float weatherT = 1f;
        // Event
        private readonly float[] eventTarget = new float[EventCount];
        private readonly float[] eventWeight = new float[EventCount];
        // Partikel region
        private GameObject particleNow, particleOld;
        private string particleNowTag, particleOldTag;
        private float particleNowBase, particleOldBase;

        private EnvState lastApplied;
        private bool hasApplied;
        private bool snapNext = true;
        private float applyTimer;

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            Instance = this;
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            // Flat, supaya DynamicGI.UpdateEnvironment tidak perlu dipanggil.
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            if (skyboxBlended != null) RenderSettings.skybox = skyboxBlended;
        }

        private void OnEnable()
        {
            EventChannel<RegionChangedEvent>.Raised += HandleRegionChanged;
            EventChannel<WeatherChangedEvent>.Raised += HandleWeatherChanged;
            // TODO #112: EventChannel<InteriorZoneChangedEvent>.Raised += HandleInterior.

            // Ketiganya sticky. Load: semua T langsung 1 supaya tidak ada transisi saat layar baru terbuka.
            if (EventChannel<RegionChangedEvent>.HasLast) activeRegion = EventChannel<RegionChangedEvent>.Last.to;
            weatherFrom = weatherTo = RegionalWeather.Instance != null ? RegionalWeather.Instance.GetWeather(activeRegion) : WeatherType.Clear;
            regionT = weatherT = 1f;
            snapNext = true;
            SwapParticles(Profile(activeRegion), instant: true);
        }

        private void OnDisable()
        {
            EventChannel<RegionChangedEvent>.Raised -= HandleRegionChanged;
            EventChannel<WeatherChangedEvent>.Raised -= HandleWeatherChanged;
            ReturnParticle(ref particleNow, particleNowTag);
            ReturnParticle(ref particleOld, particleOldTag);
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        // ---------------------------------------------------------------- Event dari sistem lain

        private void HandleRegionChanged(RegionChangedEvent e)
        {
            // Event pertama (from == to) = keadaan awal tanpa transisi.
            bool first = e.from == e.to;
            // 1. prev = nilai region saat ini, jadi bolak-balik di tengah transisi tidak melompat.
            regionFrom = RegionLayer();
            skyFrom = CurrentSky();
            activeRegion = e.to;
            regionT = first ? 1f : 0f;
            // Cuaca region baru langsung dibaca. Transisinya ikut weatherBlendSeconds.
            var w = RegionalWeather.Instance != null ? RegionalWeather.Instance.GetWeather(activeRegion) : weatherTo;
            if (w != weatherTo) BeginWeather(w);
            if (first) snapNext = true;
            // 4. Partikel. 5. TODO #23: PostProcessController.Instance.SetRegionProfile(profile.postProfile, regionBlendSeconds).
            SwapParticles(Profile(activeRegion), first);
        }

        private void HandleWeatherChanged(WeatherChangedEvent e)
        {
            // Hanya region aktif.
            if (e.region != activeRegion || e.weather == weatherTo) return;
            BeginWeather(e.weather);
        }

        private void BeginWeather(WeatherType w)
        {
            // Mulai dari campuran saat ini supaya tidak melompat.
            weatherFrom = weatherT >= 0.5f ? weatherTo : weatherFrom;
            weatherTo = w;
            weatherT = 0f;
        }

        /// <summary>Dipanggil HordeManager (#50) dan DisasterManager (#70). Bobot 0 sampai 1.</summary>
        public void PushEventLayer(EnvEventId id, float weight)
        {
            eventTarget[(int)id] = Mathf.Clamp01(weight);
        }

        public void PopEventLayer(EnvEventId id)
        {
            eventTarget[(int)id] = 0f;
        }

        // ---------------------------------------------------------------- Transisi dan penulisan

        private void Update()
        {
            var bal = Balance;
            if (bal == null) return;
            var env = bal.environment;
            float dt = Time.deltaTime; // Game dijeda: transisi berhenti, nilai terakhir tetap tertulis

            regionT = Mathf.MoveTowards(regionT, 1f, dt / Mathf.Max(0.01f, env.regionBlendSeconds));
            weatherT = Mathf.MoveTowards(weatherT, 1f, dt / Mathf.Max(0.01f, env.weatherBlendSeconds));
            for (int i = 0; i < EventCount; i++)
            {
                var layer = EventLayer((EnvEventId)i);
                bool rising = eventTarget[i] > eventWeight[i];
                float secs = rising ? layer.blendInSeconds : layer.blendOutSeconds;
                if (secs < 0f) secs = env.eventBlendSeconds;
                eventWeight[i] = Mathf.MoveTowards(eventWeight[i], eventTarget[i], dt / Mathf.Max(0.01f, secs));
            }
            UpdateParticleFade(dt / Mathf.Max(0.01f, env.regionBlendSeconds));

            applyTimer -= dt;
            if (applyTimer > 0f && !snapNext) return;
            applyTimer = env.applyIntervalSeconds;

            if (snapNext)
            {
                regionT = weatherT = 1f;
                for (int i = 0; i < EventCount; i++) eventWeight[i] = eventTarget[i];
                snapNext = false;
            }
            Compute(out var s);
            Apply(in s);
        }

        private EnvState RegionLayer()
        {
            var to = FromProfile(Profile(activeRegion));
            if (regionT >= 1f) return to;
            return Lerp(regionFrom, to, regionT);
        }

        private void Compute(out EnvState s)
        {
            var env = Balance.environment;
            var clock = DayNightDirector.Instance;
            var profile = Profile(activeRegion);

            // 1. Region.
            s = RegionLayer();

            // 2. Jam. t = jam / 24.
            float t = clock != null ? Mathf.Repeat(clock.CurrentTimeOfDay / 24f, 1f) : 0.5f;
            Color hourAmbient = profile != null ? profile.ambientByHour.Evaluate(t) : Color.white;
            s.ambient *= hourAmbient;
            s.fogColor *= hourAmbient;   // Warna fog ikut pengali ambient (senter di area gelap)
            if (profile != null)
            {
                s.sunColor = profile.sunColorByHour.Evaluate(t);
                s.sunIntensity *= profile.sunIntensityByHour.Evaluate(t);
                if (clock != null && clock.IsNight) s.fogDensity *= profile.nightFogMultiplier;
            }

            // 3. Cuaca: fog ditambah, cahaya dikalikan, warna fog di-lerp.
            var wa = WeatherLayer(weatherFrom);
            var wb = WeatherLayer(weatherTo);
            float addFog = Mathf.Lerp(wa.addFogDensity, wb.addFogDensity, weatherT);
            float sunMul = Mathf.Lerp(wa.sunMultiplier, wb.sunMultiplier, weatherT);
            float ambMul = Mathf.Lerp(wa.ambientMultiplier, wb.ambientMultiplier, weatherT);
            Color fogA = Color.Lerp(s.fogColor, wa.fogColor, wa.fogColorWeight);
            Color fogB = Color.Lerp(s.fogColor, wb.fogColor, wb.fogColorWeight);
            s.fogDensity += addFog;
            s.sunIntensity *= sunMul;
            s.ambient *= ambMul;
            s.fogColor = Color.Lerp(fogA, fogB, weatherT);

            // 4. Event: prioritas tertinggi dengan bobot di atas 0. Blood Moon (30) menang atas erupsi (20).
            int best = -1, bestPriority = int.MinValue;
            for (int i = 0; i < EventCount; i++)
            {
                if (eventWeight[i] <= 0f) continue;
                var layer = EventLayer((EnvEventId)i);
                if (layer.priority > bestPriority)
                {
                    bestPriority = layer.priority;
                    best = i;
                }
            }
            if (best >= 0)
            {
                var layer = EventLayer((EnvEventId)best);
                float k = eventWeight[best];
                s.fogColor = Color.Lerp(s.fogColor, layer.fogColor, k);
                s.fogDensity += layer.addFogDensity * k;
                s.sunIntensity *= Mathf.Lerp(1f, layer.sunMultiplier, k);
                s.sunColor = Color.Lerp(s.sunColor, layer.sunColor, k);
            }

            // 5. TODO #112: interior (lerp ke lightProfile, matahari 0 kalau isDark).

            // 6. Batas. TODO #19: ambient dikali SettingsManager.Instance.brightness sebelum dibatasi.
            s.fogDensity = Mathf.Min(s.fogDensity, env.maxFogDensity);
            float minLum = profile != null && profile.minAmbientLuminance > 0f ? profile.minAmbientLuminance : env.defaultMinAmbientLuminance;
            s.ambient = RaiseToLuminance(s.ambient, minLum);
        }

        private void Apply(in EnvState s)
        {
            const float eps = 0.0005f;
            if (!hasApplied || Differs(lastApplied.fogColor, s.fogColor, eps)) RenderSettings.fogColor = s.fogColor;
            if (!hasApplied || Mathf.Abs(lastApplied.fogDensity - s.fogDensity) > eps * 0.01f) RenderSettings.fogDensity = s.fogDensity;
            if (!hasApplied || Differs(lastApplied.ambient, s.ambient, eps)) RenderSettings.ambientLight = s.ambient;
            if (sun != null)
            {
                if (!hasApplied || Differs(lastApplied.sunColor, s.sunColor, eps)) sun.color = s.sunColor;
                if (!hasApplied || Mathf.Abs(lastApplied.sunIntensity - s.sunIntensity) > eps) sun.intensity = s.sunIntensity;
                // Rotasi dari DayNightDirector, jadi arah bayangan sama dengan raycast SunlightExposure (#63).
                if (DayNightDirector.Instance != null) sun.transform.rotation = DayNightDirector.Instance.SunRotation;
            }
            ApplySkybox();
            lastApplied = s;
            hasApplied = true;
        }

        // ---------------------------------------------------------------- Skybox dan partikel

        private Texture CurrentSky()
        {
            var p = Profile(activeRegion);
            return p != null ? p.skybox : null;
        }

        private void ApplySkybox()
        {
            if (skyboxBlended == null) return;
            // Material dibuat sekali, hanya isinya yang berubah.
            var to = CurrentSky();
            skyboxBlended.SetTexture("_TexA", regionT >= 1f || skyFrom == null ? to : skyFrom);
            skyboxBlended.SetTexture("_TexB", to);
            skyboxBlended.SetFloat("_Blend", regionT);
        }

        private void SwapParticles(RegionEnvironmentProfile p, bool instant)
        {
            string tag = p != null ? p.ambientParticleTag : null;
            if (tag == particleNowTag && particleNow != null) return;
            // Partikel lama diturunkan, lalu kembali ke pool setelah transisi.
            ReturnParticle(ref particleOld, particleOldTag);
            particleOld = particleNow;
            particleOldTag = particleNowTag;
            particleOldBase = particleNowBase;
            particleNow = null;
            particleNowTag = tag;
            if (instant) ReturnParticle(ref particleOld, particleOldTag);

            var pool = ObjectPooler.Instance;
            var cam = Camera.main;
            if (string.IsNullOrEmpty(tag) || pool == null || cam == null) return;
            particleNow = pool.SpawnFromPool(tag, cam.transform.position, Quaternion.identity);
            if (particleNow == null) return;
            particleNow.transform.SetParent(cam.transform, true);
            var ps = particleNow.GetComponent<ParticleSystem>();
            particleNowBase = ps != null ? ps.emission.rateOverTimeMultiplier : 0f;
            SetEmission(particleNow, instant ? particleNowBase : 0f);
        }

        private void UpdateParticleFade(float step)
        {
            if (particleNow != null) SetEmission(particleNow, particleNowBase * regionT);
            if (particleOld != null)
            {
                SetEmission(particleOld, particleOldBase * (1f - regionT));
                if (regionT >= 1f) ReturnParticle(ref particleOld, particleOldTag);
            }
        }

        private static void SetEmission(GameObject go, float rate)
        {
            var ps = go != null ? go.GetComponent<ParticleSystem>() : null;
            if (ps == null) return;
            var em = ps.emission;
            em.rateOverTimeMultiplier = rate;
        }

        private static void ReturnParticle(ref GameObject go, string tag)
        {
            if (go == null) return;
            var obj = go;
            go = null;
            obj.transform.SetParent(null, true);
            if (ObjectPooler.Instance != null) ObjectPooler.Instance.ReturnToPool(tag, obj);
        }

        // ---------------------------------------------------------------- Bantuan

        private RegionEnvironmentProfile Profile(RegionType r)
        {
            int i = (int)r;
            if (profiles != null && i < profiles.Length && profiles[i] != null) return profiles[i];
            // Profil hilang: pakai profil Region 1 dan catat error.
            if (profiles != null && profiles.Length > 0 && profiles[0] != null)
            {
                Debug.LogError($"[EnvironmentDirector] Profil {r} kosong. Memakai profil Volcano.");
                return profiles[0];
            }
            return null;
        }

        private static EnvState FromProfile(RegionEnvironmentProfile p)
        {
            if (p == null)
                return new EnvState { fogColor = Color.gray, fogDensity = 0.004f, ambient = Color.gray, sunColor = Color.white, sunIntensity = 1f };
            return new EnvState
            {
                fogColor = p.fogColor,
                fogDensity = p.fogDensity,
                ambient = p.ambientColor,
                sunColor = Color.white,
                sunIntensity = p.sunIntensity,
            };
        }

        private static EnvState Lerp(in EnvState a, in EnvState b, float t)
        {
            return new EnvState
            {
                fogColor = Color.Lerp(a.fogColor, b.fogColor, t),
                fogDensity = Mathf.Lerp(a.fogDensity, b.fogDensity, t),
                ambient = Color.Lerp(a.ambient, b.ambient, t),
                sunColor = Color.Lerp(a.sunColor, b.sunColor, t),
                sunIntensity = Mathf.Lerp(a.sunIntensity, b.sunIntensity, t),
            };
        }

        private static WeatherEnvironmentLayer WeatherLayer(WeatherType w)
        {
            var rows = Balance != null ? Balance.environment.weatherLayers : null;
            int i = (int)w;
            if (rows == null || i >= rows.Length) return new WeatherEnvironmentLayer(w, 0f, 1f, 1f, Color.white, 0f);
            return rows[i];
        }

        private static EnvironmentEventLayer EventLayer(EnvEventId id)
        {
            var rows = Balance != null ? Balance.environment.eventLayers : null;
            if (rows != null)
                for (int i = 0; i < rows.Length; i++)
                    if (rows[i].id == id) return rows[i];
            return new EnvironmentEventLayer(id, 0, Color.white, 0f, 1f, Color.white, -1f, -1f);
        }

        private static float Luminance(Color c) => 0.2126f * c.r + 0.7152f * c.g + 0.0722f * c.b;

        /// <summary>Layar tidak boleh hitam total, termasuk saat Storm di malam hari.</summary>
        private static Color RaiseToLuminance(Color c, float min)
        {
            float lum = Luminance(c);
            if (lum >= min) return c;
            if (lum <= 0.0001f) return new Color(min, min, min, 1f);
            float k = min / lum;
            return new Color(c.r * k, c.g * k, c.b * k, 1f);
        }

        private static bool Differs(Color a, Color b, float eps)
        {
            return Mathf.Abs(a.r - b.r) > eps || Mathf.Abs(a.g - b.g) > eps || Mathf.Abs(a.b - b.b) > eps;
        }
    }
}
