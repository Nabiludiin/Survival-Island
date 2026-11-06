using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;

namespace SurvivalIsland
{
    public enum CrowdTier { Close, Mid, Far, Culled }

    /// <summary>Agen yang di-tick HordeCrowdManager (HordeCrowdManager.md, #93).</summary>
    public interface ICrowdAgent
    {
        int CrowdIndex { get; set; }
        Vector3 Position { get; }
        bool IsEngaged { get; }
        void CrowdTick(float dt);
        void SetTier(CrowdTier tier);
    }

    /// <summary>
    /// Pembagi update AI musuh ke beberapa frame, supaya 30 sampai 50 zombie tetap ringan.
    /// Blueprint: left-4-dead/HordeCrowdManager.md (#93). Usulan teknis; mendukung Horde malam GDD bagian 3.
    /// Singleton per scene (IslandPhase dan ShipPhase).
    /// Belum diimplementasikan (TODO #93): titik tujuan bersama per kelompok (cluster), animator stagger per tingkatan,
    /// perintah DevConsole stress/crowdstat/crowdai (#56).
    /// </summary>
    public class HordeCrowdManager : MonoBehaviour
    {
        public static HordeCrowdManager Instance { get; private set; }

        private ICrowdAgent[] agents;
        private float[] lastTickTime;
        private CrowdTier[] tiers;
        private bool[] visible;
        private int count;
        private int cursor;
        private bool iterating;
        private readonly System.Collections.Generic.List<ICrowdAgent> removeQueue = new System.Collections.Generic.List<ICrowdAgent>(16);
        private float tierTimer;
        private int destinationsThisFrame;
        private int destinationFrame = -1;
        private readonly Plane[] frustum = new Plane[6];
        private readonly Stopwatch watch = new Stopwatch();

        /// <summary>Buffer OverlapSphere bersama untuk serangan musuh. Hasilnya tidak boleh disimpan setelah tick.</summary>
        public Collider[] AttackBuffer { get; private set; }

        private static CrowdBalance Bal => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.crowd : null;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Debug.LogError("[HordeCrowdManager] Lebih dari satu instance."); enabled = false; return; }
            Instance = this;
            var b = Bal;
            int cap = b != null ? Mathf.Max(8, b.capacity) : 64;
            agents = new ICrowdAgent[cap];
            lastTickTime = new float[cap];
            tiers = new CrowdTier[cap];
            visible = new bool[cap];
            AttackBuffer = new Collider[b != null ? Mathf.Max(4, b.overlapBufferSize) : 16];
            if (b != null) NavMesh.pathfindingIterationsPerFrame = b.pathfindingIterationsPerFrame;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        /// <summary>false kalau kapasitas penuh (tanda maxAlive melewati kapasitas). EnemyAI lalu kembali ke pool.</summary>
        public bool Register(ICrowdAgent a)
        {
            if (a == null) return false;
            if (a.CrowdIndex >= 0 && a.CrowdIndex < count && agents[a.CrowdIndex] == a) return true;
            if (count >= agents.Length) { Debug.LogError("[HordeCrowdManager] Kapasitas penuh (crowd.capacity)."); return false; }
            int i = count++;
            agents[i] = a;
            a.CrowdIndex = i;
            // Jeda awal acak dari indeks, supaya tick tidak serempak.
            lastTickTime[i] = Time.time - (i % 8) * 0.0125f;
            tiers[i] = CrowdTier.Mid;
            a.SetTier(CrowdTier.Mid);
            return true;
        }

        public void Unregister(ICrowdAgent a)
        {
            if (a == null) return;
            if (iterating) { if (!removeQueue.Contains(a)) removeQueue.Add(a); return; }
            RemoveNow(a);
        }

        private void RemoveNow(ICrowdAgent a)
        {
            int i = a.CrowdIndex;
            if (i < 0 || i >= count || agents[i] != a) { a.CrowdIndex = -1; return; }
            int last = count - 1;
            agents[i] = agents[last];
            lastTickTime[i] = lastTickTime[last];
            tiers[i] = tiers[last];
            visible[i] = visible[last];
            if (agents[i] != null) agents[i].CrowdIndex = i;
            agents[last] = null;
            count--;
            a.CrowdIndex = -1;
            if (cursor > count) cursor = 0;
        }

        private void Update()
        {
            if (Time.deltaTime <= 0f || count == 0) return;
            var b = Bal;
            if (b == null) return;

            tierTimer -= Time.deltaTime;
            if (tierTimer <= 0f) { tierTimer = b.tierUpdateSeconds; UpdateTiers(b); }

            float now = Time.time;
            int ticks = 0;
            int checkedAgents = 0;
            watch.Reset();
            watch.Start();
            iterating = true;
            int total = count;
            while (checkedAgents < total && ticks < b.maxTicksPerFrame)
            {
                if (cursor >= count) cursor = 0;
                if (count == 0) break;
                int i = cursor++;
                checkedAgents++;
                var a = agents[i];
                if (a == null || tiers[i] == CrowdTier.Culled) continue;
                float elapsed = now - lastTickTime[i];
                if (elapsed < IntervalOf(i, b)) continue;
                lastTickTime[i] = now;
                a.CrowdTick(Mathf.Min(elapsed, 1f));
                ticks++;
                if (watch.Elapsed.TotalMilliseconds > b.maxTickMilliseconds) break;
            }
            iterating = false;
            watch.Stop();
            for (int r = 0; r < removeQueue.Count; r++) RemoveNow(removeQueue[r]);
            removeQueue.Clear();
        }

        private float IntervalOf(int i, CrowdBalance b)
        {
            var a = agents[i];
            if (a.IsEngaged) return b.intervalCloseVisible;
            switch (tiers[i])
            {
                case CrowdTier.Close: return visible[i] ? b.intervalCloseVisible : b.intervalCloseHidden;
                case CrowdTier.Mid: return visible[i] ? b.intervalMidVisible : b.intervalMidHidden;
                default: return b.intervalFar;
            }
        }

        private void UpdateTiers(CrowdBalance b)
        {
            var cam = FirstPersonCamera.Instance != null ? FirstPersonCamera.Instance.WorldCamera : Camera.main;
            var player = PlayerTargetable.Instance;
            if (player == null) return;
            if (cam != null) GeometryUtility.CalculateFrustumPlanes(cam, frustum);
            Vector3 p = player.Position;
            float close2 = b.closeDistance * b.closeDistance;
            float mid2 = b.midDistance * b.midDistance;
            for (int i = 0; i < count; i++)
            {
                var a = agents[i];
                if (a == null || tiers[i] == CrowdTier.Culled) continue;
                Vector3 pos = a.Position;
                float sqr = (pos - p).sqrMagnitude;
                // "Terlihat" berarti di dalam frustum, tanpa raycast (HordeCrowdManager.md).
                visible[i] = cam != null && GeometryUtility.TestPlanesAABB(frustum, new Bounds(pos + Vector3.up, new Vector3(1f, 2f, 1f)));
                CrowdTier t = a.IsEngaged || sqr <= close2 ? CrowdTier.Close : sqr <= mid2 ? CrowdTier.Mid : CrowdTier.Far;
                if (t != tiers[i]) { tiers[i] = t; a.SetTier(t); }
            }
        }

        /// <summary>OptimizationManager mematikan atau menyalakan tick agen.</summary>
        public void SetCulled(ICrowdAgent a, bool culled)
        {
            int i = a != null ? a.CrowdIndex : -1;
            if (i < 0 || i >= count || agents[i] != a) return;
            tiers[i] = culled ? CrowdTier.Culled : CrowdTier.Mid;
            a.SetTier(tiers[i]);
        }

        /// <summary>
        /// Satu-satunya jalan SetDestination musuh. Dibatasi jeda, ambang geser, dan jumlah per frame (HordeCrowdManager.md).
        /// </summary>
        public bool TrySetDestination(EnemyAI e, Vector3 target)
        {
            var b = Bal;
            var agent = e != null ? e.Agent : null;
            if (b == null || agent == null || !agent.enabled || !agent.isOnNavMesh) return false;
            if (Time.time - e.lastRepathTime < b.repathMinSeconds) return false;
            float threshold = Mathf.Clamp(Vector3.Distance(e.Position, target) * b.repathDistanceFactor, b.repathMinDistance, b.repathMaxDistance);
            if ((target - e.lastDestination).sqrMagnitude < threshold * threshold) return false;
            if (destinationFrame != Time.frameCount) { destinationFrame = Time.frameCount; destinationsThisFrame = 0; }
            if (destinationsThisFrame >= b.maxDestinationsPerFrame) return false;
            destinationsThisFrame++;
            agent.SetDestination(target);
            e.lastDestination = target;
            e.lastRepathTime = Time.time;
            return true;
        }

        /// <summary>Kualitas avoidance per tingkatan (HordeCrowdManager.md "Avoidance").</summary>
        public static ObstacleAvoidanceType AvoidanceFor(CrowdTier tier, bool isNearest)
        {
            if (tier == CrowdTier.Far || tier == CrowdTier.Culled) return ObstacleAvoidanceType.NoObstacleAvoidance;
            return isNearest ? ObstacleAvoidanceType.MedQualityObstacleAvoidance : ObstacleAvoidanceType.LowQualityObstacleAvoidance;
        }
    }
}
