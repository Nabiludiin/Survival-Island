using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace SurvivalIsland
{
    /// <summary>
    /// Zombie terbakar sinar matahari. Blueprint: left-4-dead/SunlightExposure.md (#63). Menggantikan EnemyAI.TriggerDaybreakDeath.
    /// GDD bagian 3: zombie terbakar dan mati di bawah matahari, tidak berkeliaran di luar ruangan saat siang.
    /// Claude.md Pilar 3: semua zombie memainkan animasi terbakar saat matahari terbit, lalu kembali ke ObjectPooler.
    ///
    /// Catatan Langkah 5:
    /// - DayNightDirector.SunDirection (Langkah 4) belum ada. Arah matahari dibaca dari RenderSettings.sun (Directional Light
    ///   yang diatur DayNightDirector), dengan elevasi minimal minSunElevationDeg. Tanpa sun, dipakai arah tegak ke atas.
    /// - Siang/malam dari HourChangedEvent (DayPhaseTracker), bukan DaybreakEvent/NightfallEvent.
    /// - Zombie sarang di InteriorZone dibaca dari LairRegistry (#112). Zombie Desa yang Submerged diberi IsProtected oleh
    ///   VillageZombieHiding (#63), dan ReportEmergence meneruskan jumlah penghuni HidingSpot ke AIDirector.
    /// </summary>
    public class SunlightExposure : MonoBehaviour, IPoolable
    {
        [SerializeField] private ParticleSystem burnEffect;
        [SerializeField] private Animator animator;
        [SerializeField] private float headHeight = 2f;

        public bool IsBurning { get; private set; }
        public bool IsProtected { get; set; }   // Submerged atau zombie sarang di InteriorZone

        private float shelteredTimer;
        private Vector3 baseScale;
        private bool burnPending;
        private bool wasDay;
        private Coroutine exposureLoop;
        private EnemyAI ai;
        private EnemyHealth health;
        private NavMeshAgent agent;

        private static SunlightBalance Bal => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.sunlight : null;

        private void Awake()
        {
            ai = GetComponent<EnemyAI>();
            health = GetComponent<EnemyHealth>();
            agent = GetComponent<NavMeshAgent>();
            if (animator == null) animator = GetComponentInChildren<Animator>();
            baseScale = transform.localScale;
        }

        private void OnEnable()
        {
            EventChannel<HourChangedEvent>.Raised += HandleHour;
        }

        private void OnDisable()
        {
            EventChannel<HourChangedEvent>.Raised -= HandleHour;
        }

        private static bool IsShipPhase => GameManager.Instance != null && GameManager.Instance.currentPhase == GamePhase.SinkingShip;

        private void HandleHour(HourChangedEvent e)
        {
            bool day = !DayPhaseTracker.IsNightHour(e.hour);
            if (day && !wasDay) StartExposure();
            else if (!day && wasDay) StopExposure();
            wasDay = day;
        }

        private void StartExposure()
        {
            if (IsShipPhase || exposureLoop != null || !isActiveAndEnabled) return;
            exposureLoop = StartCoroutine(ExposureLoop());
        }

        private void StopExposure()
        {
            if (exposureLoop != null) { StopCoroutine(exposureLoop); exposureLoop = null; }
            shelteredTimer = 0f;
        }

        private IEnumerator ExposureLoop()
        {
            var b = Bal;
            float interval = b != null ? b.checkIntervalSeconds : 2f;
            // 1. Jeda awal dari ID, supaya tidak mengecek pada frame yang sama. Tidak mengambil aliran acak.
            yield return new WaitForSeconds((GetInstanceID() & 7) / 8f * interval);
            // 2-3.
            while (!IsBurning && !burnPending)
            {
                bool submerged = ai != null && ai.CurrentState == AIState.Submerged;
                if (!submerged && health.IsAlive) CheckExposure(interval);
                yield return new WaitForSeconds(interval);
            }
            exposureLoop = null;
        }

        private void CheckExposure(float interval)
        {
            var b = Bal;
            if (b == null) return;
            // 1. Zombie sarang di dalam InteriorZone terlindungi (#112).
            if (ai != null && LairRegistry.IsShelteredLairZombie(ai)) return;
            if (IsProtected) return;
            // 2. Arah matahari, elevasi dinaikkan ke minimal minSunElevationDeg.
            Vector3 dir = SunDirection(b.minSunElevationDeg);
            // 3-4.
            Vector3 origin = transform.position + Vector3.up * headHeight;
            if (!PhysicsQuery.Raycast(new Ray(origin, dir), out _, b.rayLength, PhysicsMasks.Sunlight)) { StartBurn(); return; }
            // 5. Tempat tertutup hanya menunda sampai maxShelteredSeconds.
            shelteredTimer += interval;
            if (shelteredTimer >= b.maxShelteredSeconds) StartBurn();
        }

        private static Vector3 SunDirection(float minElevationDeg)
        {
            var sun = RenderSettings.sun;
            Vector3 toSun = sun != null ? -sun.transform.forward : Vector3.up;
            Vector3 flat = new Vector3(toSun.x, 0f, toSun.z);
            float elevation = Mathf.Asin(Mathf.Clamp(toSun.y, -1f, 1f)) * Mathf.Rad2Deg;
            if (elevation >= minElevationDeg || flat.sqrMagnitude < 0.0001f) return toSun.y > 0f ? toSun.normalized : Vector3.up;
            float rad = minElevationDeg * Mathf.Deg2Rad;
            return (flat.normalized * Mathf.Cos(rad) + Vector3.up * Mathf.Sin(rad)).normalized;
        }

        /// <summary>
        /// Laporan ke Director (SunlightExposure.md): dipanggil SpawnManager saat malam tiba. Zombie Desa yang akan naik
        /// dari HidingSpot dihitung sebagai spawn luar, supaya mob pertama tidak menumpuk di atasnya (#27).
        /// </summary>
        public static void ReportEmergence()
        {
            int n = HidingSpot.TotalOccupants;
            if (n > 0 && AIDirector.Instance != null) AIDirector.Instance.AddExternalSpawn(n);
        }

        /// <summary>Mulai urutan terbakar. Penjaga burnPending supaya hanya sekali.</summary>
        public void StartBurn()
        {
            if (burnPending || IsBurning || !isActiveAndEnabled) return;
            burnPending = true;
            var b = Bal;
            // 1. Jeda acak dihitung sebelum iterator (ref tidak boleh di dalam iterator).
            float delay = 0f;
            if (b != null && RunSeedManager.Instance != null)
            {
                ref var rng = ref RunSeedManager.Instance.GetStream(RandomStreamId.Enemy);
                delay = rng.Range(0f, b.burnStaggerMaxSeconds);
            }
            StartCoroutine(BurnRoutine(delay));
        }

        private IEnumerator BurnRoutine(float delay)
        {
            if (delay > 0f) yield return new WaitForSeconds(delay);
            // 2. Sudah dibunuh pemain: mayat mengikuti #95.
            if (!health.IsAlive) { burnPending = false; yield break; }
            // 3.
            IsBurning = true;
            if (ai != null) ai.EnterBurn();
            // 4. Damage mematikan, armor tidak bisa menahan. EnemyKilledEvent terkirim dengan byPlayer = false.
            var info = new DamageInfo
            {
                amount = Mathf.Max(1f, health.CurrentHP) * 1000f, type = DamageType.Fire, source = null, sourceId = "sunlight",
                hitPoint = transform.position, hitDirection = Vector3.down, bodyPart = BodyPart.Torso, byPlayer = false
            };
            health.TakeDamage(info);
            // 5.
            if (burnEffect != null) burnEffect.Play();
            if (animator != null && animator.isActiveAndEnabled) animator.SetTrigger("Burn");
            // 6. Skala Y turun ke 0 selama burnDurationSeconds.
            var b = Bal;
            float dur = b != null ? b.burnDurationSeconds : 3f;
            for (float t = 0f; t < dur; t += Time.deltaTime)
            {
                var s = baseScale;
                s.y = Mathf.Lerp(baseScale.y, 0.01f, t / dur);
                transform.localScale = s;
                yield return null;
            }
            // 7.
            if (SpawnManager.Instance != null) SpawnManager.Instance.Despawn(gameObject, DespawnReason.Burned);
            else if (ObjectPooler.Instance != null) ObjectPooler.Instance.ReturnToPool(gameObject);
        }

        public void OnSpawned()
        {
            transform.localScale = baseScale;
            IsBurning = false;
            burnPending = false;
            IsProtected = false;
            shelteredTimer = 0f;
            if (animator != null) animator.Rebind();
            wasDay = !DayPhaseTracker.IsNight;
            // Muncul saat siang (misalnya DevConsole): loop ikut mulai.
            if (wasDay) StartExposure();
        }

        public void OnReturned()
        {
            StopAllCoroutines();
            exposureLoop = null;
            if (burnEffect != null) burnEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            transform.localScale = baseScale;
            if (agent != null && agent.enabled) agent.enabled = false;
            IsBurning = false;
            burnPending = false;
            IsProtected = false;
            shelteredTimer = 0f;
        }
    }
}
