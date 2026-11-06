using System.Collections;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.AI;

namespace SurvivalIsland
{
    /// <summary>
    /// Culling jarak jauh: mematikan NavMeshAgent dan Animator musuh yang jauh dari pemain.
    /// Blueprint: Core_Logic/OptimizationManager.md (#62, #90, #93). Usulan teknis, mendukung horde malam GDD bagian 2 tanpa lag.
    /// Pembagian tick dan tingkatan jarak dekat milik HordeCrowdManager (#93).
    ///
    /// Singleton per scene di IslandPhase. Daftar musuh dibaca dari SpawnManager.ActiveEnemies; status culling disimpan
    /// di EnemyInstance (IsCulled, culledAgentWasEnabled, culledAnimatorWasEnabled), supaya ikut direset saat kembali ke pool.
    /// Tidak mengirim atau mendengar event, dan tidak menyimpan apa pun ke save.
    /// </summary>
    public class OptimizationManager : MonoBehaviour
    {
        public static OptimizationManager Instance { get; private set; }

        private static readonly ProfilerMarker CullMarker = new ProfilerMarker("Optimization.Cull");

        private Transform player;
        private Coroutine loop;
        private WaitForSeconds wait;
        private float waitSeconds = -1f;

        /// <summary>Jumlah musuh yang sedang di-culling. Untuk DevConsole (cullinfo) dan Profiler.</summary>
        public int CulledCount { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this) { Debug.LogError("[OptimizationManager] Lebih dari satu instance."); enabled = false; return; }
            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void OnEnable()
        {
            FindPlayer();
            loop = StartCoroutine(CullingLoop());
        }

        private void OnDisable()
        {
            if (loop != null) StopCoroutine(loop);
            loop = null;
            player = null;
        }

        // Pemain Fase 2 di IslandPhase. Dicari ulang kalau belum ada saat OnEnable.
        private void FindPlayer()
        {
            if (player != null) return;
            if (IslandPlayerController.Instance != null) player = IslandPlayerController.Instance.transform;
        }

        private IEnumerator CullingLoop()
        {
            while (true)
            {
                var opt = GameManager.CurrentBalance != null ? GameManager.CurrentBalance.optimization : null;
                float seconds = opt != null ? Mathf.Max(0.1f, opt.cullCheckSeconds) : 1f;
                if (wait == null || !Mathf.Approximately(seconds, waitSeconds))
                {
                    waitSeconds = seconds;
                    wait = new WaitForSeconds(seconds);   // Waktu berskala: berhenti saat jeda
                }
                yield return wait;

                FindPlayer();
                // 1. Lewati putaran ini kalau pemain atau SpawnManager belum ada.
                if (player == null || SpawnManager.Instance == null || opt == null) continue;
                RunPass(opt);
            }
        }

        private void RunPass(OptimizationBalance opt)
        {
            using (CullMarker.Auto())
            {
                var list = SpawnManager.Instance.ActiveEnemies;
                Vector3 p = player.position;
                float cull2 = opt.cullingDistance * opt.cullingDistance;
                float baseRange = BaseRaidKeepDistance();
                float base2 = baseRange * baseRange;
                int culled = 0;
                // 2. Indeks dari belakang, tanpa foreach dan tanpa alokasi.
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    var e = list[i];
                    if (e == null) continue;
                    Evaluate(e, p, cull2, base2, opt);
                    if (e.IsCulled) culled++;
                }
                // Dihitung ulang tiap putaran, jadi tidak bisa salah lama.
                CulledCount = culled;
            }
        }

        // remoteDistance + clusterRadius (110 m). Lihat Catatan Implementasi di OptimizationManager.md.
        private static float BaseRaidKeepDistance()
        {
            var b = GameManager.CurrentBalance;
            if (b == null || b.remoteBase == null) return 0f;
            return b.remoteBase.remoteDistance + b.remoteBase.clusterRadius;
        }

        private void Evaluate(EnemyInstance e, Vector3 playerPos, float cull2, float base2, OptimizationBalance opt)
        {
            // 1. Sistem dormant mengatur komponennya sendiri.
            if (e.IsDormantCounted)
            {
                if (e.IsCulled) Uncull(e, opt);
                return;
            }

            bool attackingBase = e.AI != null && e.AI.CurrentState == AIState.AttackBuilding;
            float sqr = (e.transform.position - playerPos).sqrMagnitude;

            // 2. Musuh yang mengejar pemain tidak boleh membeku di belakang pemain (selaras HordeCrowdManager: IsEngaged = Close).
            // 3. Zombie yang menyerang base tetap aktif selama base masih Nyata (#90).
            bool keep = (e.IsEngaged && !attackingBase) || (attackingBase && sqr <= base2);

            if (!keep && sqr > cull2)
            {
                // 4.
                if (!e.IsCulled) Cull(e);
            }
            else if (e.IsCulled)
            {
                // 5.
                Uncull(e, opt);
            }
        }

        private static void Cull(EnemyInstance e)
        {
            var agent = e.Agent;
            var animator = e.GetComponentInChildren<Animator>();
            // 1. Status lama disimpan, supaya komponen yang sengaja dimatikan sistem lain (Jaguar di perch) tidak dinyalakan.
            e.culledAgentWasEnabled = agent != null && agent.enabled;
            e.culledAnimatorWasEnabled = animator != null && animator.enabled;
            // 2.
            if (agent != null && agent.enabled)
            {
                if (agent.isOnNavMesh) agent.ResetPath();
                agent.enabled = false;
            }
            // 3.
            if (animator != null) animator.enabled = false;
            // 4. Zombie (ICrowdAgent): HordeCrowdManager berhenti memanggil CrowdTick. Hewan dilewati.
            if (e.AI != null && HordeCrowdManager.Instance != null) HordeCrowdManager.Instance.SetCulled(e.AI, true);
            // 5. Tetap dihitung hidup, sehingga MaxAlive tidak terlewati.
            e.IsCulled = true;
        }

        private static void Uncull(EnemyInstance e, OptimizationBalance opt)
        {
            var agent = e.Agent;
            // 1. Nyalakan agen; kalau tidak di NavMesh, coba Warp ke titik terdekat. Gagal: pengaman macet SpawnManager.
            if (e.culledAgentWasEnabled && agent != null)
            {
                agent.enabled = true;
                if (!agent.isOnNavMesh && NavMesh.SamplePosition(e.transform.position, out var hit, opt.uncullSampleRadius, NavMesh.AllAreas))
                    agent.Warp(hit.position);
            }
            // 2-3. Animator dikembalikan ke status sebelum culling (lihat Catatan Implementasi: EnemyAI.SetTier hanya mengatur
            // cullingMode, belum animator.enabled). Tingkatan zombie dikembalikan lewat SetCulled(false) -> Mid, lalu
            // putaran tingkatan HordeCrowdManager berikutnya memperbaikinya.
            var animator = e.GetComponentInChildren<Animator>();
            if (animator != null) animator.enabled = e.culledAnimatorWasEnabled;
            if (e.AI != null && HordeCrowdManager.Instance != null) HordeCrowdManager.Instance.SetCulled(e.AI, false);
            // 4. Tujuan baru: zombie lewat HordeCrowdManager.TrySetDestination pada tick berikutnya, hewan lewat state WildlifeAI.
            // 5.
            e.IsCulled = false;
        }
    }
}
