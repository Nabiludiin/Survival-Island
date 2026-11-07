using UnityEngine;
using UnityEngine.AI;

namespace SurvivalIsland
{
    /// <summary>
    /// Naik turunnya air laut menurut jam game. Manager di scene IslandPhase, tanpa status tersimpan: nilai air dihitung
    /// dari jam, jadi selalu benar setelah tidur, jeda, dan load. Hanya laut yang bergerak; sungai dan danau diam.
    /// Blueprint: Island_Logic/TideManager.md (#91). GDD bagian 4: air pasang di pasir pantai dan peringatan Diary Bab 1.
    /// Siklus, tinggi air, dan akibatnya adalah usulan yang disetujui developer (#85).
    ///
    /// Catatan implementasi (sistem pendukung):
    /// - WorldItemManager.SweepBelow (#66) dipanggil di StepCollider. HeavyItemSystem.SweepBelow (#69) juga dipanggil di sana.
    /// - Area NavMesh "Tidal" dibaca lewat NavMesh.GetAreaFromName. Kalau area belum dibuat, SetAreaCost dilewati.
    /// </summary>
    public class TideManager : MonoBehaviour
    {
        public const string TidalAreaName = "Tidal";
        private static readonly int TideWaterYId = Shader.PropertyToID("_TideWaterY");

        public static TideManager Instance { get; private set; }

        [Tooltip("Mesh laut, digerakkan halus setiap frame.")]
        [SerializeField] private Transform seaVisual;
        [Tooltip("Collider layer Water milik laut (WaterVolume), digerakkan per langkah colliderStepMeters.")]
        [SerializeField] private Transform seaTrigger;
        [Tooltip("Selisih tinggi permukaan trigger terhadap posisi transform-nya (misalnya setengah tinggi BoxCollider).")]
        [SerializeField] private float seaTriggerSurfaceOffset;
        [Tooltip("Area pantai yang diperiksa (pemain, barang).")]
        [SerializeField] private Bounds tideZone = new Bounds(Vector3.zero, new Vector3(2000f, 50f, 200f));
        [SerializeField] private TideSafePoint[] safePoints = new TideSafePoint[0];

        private float surge;
        private float surgeTarget;
        private float colliderY = float.NaN;
        private float visualY = float.NaN;
        private float tickTimer;
        private double lastTotalHours = -1;
        private bool lastPublishedHigh;
        private float lastPublishedLevel = float.NaN;
        private bool firstTick = true;
        private int warnedDay = -1;
        private float lastLevel;
        private int tidalArea = -2;

        public float Level { get; private set; }
        public float WaterY { get; private set; }
        public bool IsHigh => Level >= (T != null ? T.highThreshold : 0.7f);
        public bool IsLow => Level <= (T != null ? T.lowThreshold : 0.2f);
        public Bounds TideZone => tideZone;
        public TideSafePoint[] SafePoints => safePoints;

        /// <summary>Tinggi pada level 1 + stormSurgeLevel.</summary>
        public float MaxWaterY => WaterYAtLevel(1f + (T != null ? T.stormSurgeLevel : 0.25f));

        private static TideBalance T => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.tide : null;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake() { Instance = this; }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void OnEnable()
        {
            EventChannel<WeatherChangedEvent>.Raised += HandleWeatherChanged;
            firstTick = true;
        }

        private void OnDisable()
        {
            EventChannel<WeatherChangedEvent>.Raised -= HandleWeatherChanged;
        }

        private void HandleWeatherChanged(WeatherChangedEvent e)
        {
            // Pantai ada di selatan Region 1, jadi hanya cuaca Volcano yang dihitung.
            if (e.region != RegionType.Volcano) return;
            surgeTarget = e.weather == WeatherType.Storm ? (T != null ? T.stormSurgeLevel : 0.25f) : 0f;
        }

        // ---------------------------------------------------------------- Rumus

        /// <summary>
        /// Pasang pukul 00:00 dan 12:00, surut pukul 06:00 dan 18:00. Pasang malam lebih tinggi (usulan 1 disetujui),
        /// supaya tidur di pantai memang berbahaya (GDD bagian 4, Diary Bab 1).
        /// </summary>
        public static float BaseLevel(float h, float dayAmplitude, float nightAmplitude)
        {
            float amp = Mathf.Lerp(dayAmplitude, nightAmplitude, 0.5f + 0.5f * Mathf.Cos(2f * Mathf.PI * h / 24f));
            return amp * (0.5f + 0.5f * Mathf.Cos(2f * Mathf.PI * h / 12f));
        }

        public float WaterYAtLevel(float level)
        {
            var t = T;
            float low = t != null ? t.lowWaterY : 0f;
            float high = t != null ? t.highWaterY : 1.6f;
            return low + level * (high - low);
        }

        /// <summary>Titik terendam: tanah Terrain di bawah WaterY. Tanpa raycast.</summary>
        public bool IsPointSubmerged(Vector3 p)
        {
            var sd = SurfaceDetection.Instance;
            if (sd == null || !sd.TryGetTerrainSurfaceAtXZ(p, out var s)) return false;
            return s.groundHeight < WaterY;
        }

        // ---------------------------------------------------------------- Update

        private void Update()
        {
            var t = T;
            var clock = DayNightDirector.Instance;
            if (t == null || clock == null) return;
            float dt = Time.deltaTime;

            // 4. Mesh laut bergerak halus setiap frame.
            if (seaVisual != null && !float.IsNaN(visualY))
            {
                visualY = Mathf.MoveTowards(visualY, WaterY, t.visualLerpSpeed * Mathf.Max(dt, 0f));
                var vp = seaVisual.position;
                seaVisual.position = new Vector3(vp.x, visualY, vp.z);
            }

            if (dt <= 0f && !firstTick) return;   // Jeda: jam berhenti, air diam
            tickTimer += dt;
            if (tickTimer < t.tickSeconds && !firstTick) return;
            tickTimer = 0f;

            // 1-2.
            float h = clock.CurrentTimeOfDay;
            double total = clock.TotalGameHours;
            if (firstTick)
            {
                // Setelah load atau masuk scene: surge langsung mengikuti cuaca, tanpa lompatan bertahap.
                if (RegionalWeather.Instance != null)
                    surgeTarget = RegionalWeather.Instance.GetWeather(RegionType.Volcano) == WeatherType.Storm ? t.stormSurgeLevel : 0f;
                surge = surgeTarget;
            }
            else
            {
                float hoursPassed = lastTotalHours >= 0 ? Mathf.Max(0f, (float)(total - lastTotalHours)) : 0f;
                surge = Mathf.MoveTowards(surge, surgeTarget, t.surgeRatePerGameHour * hoursPassed);
            }
            lastTotalHours = total;

            // 3.
            lastLevel = Level;
            Level = BaseLevel(h, t.dayAmplitude, t.nightAmplitude) + surge;
            WaterY = WaterYAtLevel(Level);
            if (firstTick || float.IsNaN(visualY)) visualY = WaterY;

            // 5.
            if (firstTick || float.IsNaN(colliderY) || Mathf.Abs(WaterY - colliderY) >= t.colliderStepMeters) StepCollider();

            // 6.
            bool high = IsHigh;
            if (firstTick || float.IsNaN(lastPublishedLevel) || Mathf.Abs(Level - lastPublishedLevel) >= t.publishLevelDelta || high != lastPublishedHigh)
            {
                if (firstTick || high != lastPublishedHigh) ApplyNavCost(high);
                lastPublishedLevel = Level;
                lastPublishedHigh = high;
                EventChannel<TideChangedEvent>.Publish(new TideChangedEvent(Level, high));
            }
            firstTick = false;

            // 7.
            TickPlayer(t, t.tickSeconds);
        }

        private void StepCollider()
        {
            colliderY = WaterY;
            if (seaTrigger != null)
            {
                var p = seaTrigger.position;
                seaTrigger.position = new Vector3(p.x, WaterY - seaTriggerSurfaceOffset, p.z);
            }
            Shader.SetGlobalFloat(TideWaterYId, WaterY);
            // Barang despawns tenggelam lalu hilang 1 jam game; barang penting dan KeyItem pindah ke TideSafePoint.Nearest.
            if (WorldItemManager.Instance != null) WorldItemManager.Instance.SweepBelow(tideZone, WaterY, safePoints);
            // HeavyItem yang tertinggal di pantai dipindah ke titik aman yang sama (#69).
            if (HeavyItemSystem.Instance != null) HeavyItemSystem.Instance.SweepBelow(tideZone, WaterY, safePoints);
        }

        private void ApplyNavCost(bool high)
        {
            if (tidalArea == -2) tidalArea = NavMesh.GetAreaFromName(TidalAreaName);
            if (tidalArea < 0) return;
            // Agen menghindari pantai terendam tetapi tetap lewat kalau itu satu-satunya jalan (usulan 4). Tanpa bake ulang.
            NavMesh.SetAreaCost(tidalArea, high ? (T != null ? T.highAreaCost : 20f) : 1f);
        }

        private void TickPlayer(TideBalance t, float dt)
        {
            var player = IslandPlayerController.Instance;
            if (player == null) return;
            Vector3 pos = player.transform.position;
            if (pos.x < tideZone.min.x || pos.x > tideZone.max.x || pos.z < tideZone.min.z || pos.z > tideZone.max.z) return;

            // 1-2. Usulan 2: stamina berkurang saat mengarungi, tanpa dorongan. Pengali kecepatan milik SwimmingController.
            var sd = SurfaceDetection.Instance;
            if (sd != null && sd.TryGetTerrainSurfaceAtXZ(pos, out var s))
            {
                float depth = WaterY - s.groundHeight;
                bool swimming = SwimmingController.Instance != null && SwimmingController.Instance.Mode == WaterMode.Swimming;
                if (depth >= t.wadeDepth && !swimming && SurvivalStats.Instance != null)
                    SurvivalStats.Instance.SpendStamina(t.wadeStaminaPerSec * dt, StaminaUse.Swim);
            }

            // 3. Peringatan sekali per malam saat air naik melewati warnLevel.
            int day = GameManager.Instance != null ? GameManager.Instance.currentDay : 0;
            bool rising = Level > lastLevel;
            if (Level >= t.warnLevel && rising && warnedDay != day && NotificationManager.Instance != null)
            {
                warnedDay = day;
                NotificationManager.Instance.Show("notif.tide_rising", NotificationKind.Warning);   // "Air mulai pasang, jauhi pantai"
            }
        }

        /// <summary>Perintah settide di DevConsole (#56): paksa surge (auto = ikut cuaca).</summary>
        public void DebugSetSurge(float value, bool auto)
        {
            if (auto)
            {
                surgeTarget = RegionalWeather.Instance != null && RegionalWeather.Instance.GetWeather(RegionType.Volcano) == WeatherType.Storm
                    ? (T != null ? T.stormSurgeLevel : 0.25f) : 0f;
                return;
            }
            surgeTarget = value;
            surge = value;
        }
    }
}
