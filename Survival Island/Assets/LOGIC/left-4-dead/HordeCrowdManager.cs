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
    /// Animator bergiliran per tingkatan dijalankan EnemyAI.SetTier/LateUpdate (animStaggerMid/animStaggerFar).
    /// Kelompok zombie jauh (#93, keputusan developer 7 November 2026): agen di atas clusterMinDistance dipetakan ke sel
    /// clusterCellSize; pemimpin (terdekat ke pemain) menghitung jalur ke pemain dengan sharedPath, dan titik pada
    /// clusterWaypointLookahead menjadi tujuan semua anggota sel itu lewat TrySetDestination.
    /// Perintah DevConsole stress/crowdstat/crowdai (#56) hanya ada di Editor dan Development Build.
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

        // Kelompok (#93). Semua buffer dibuat sekali di Awake, jadi hitung ulang kelompok tidak membuat garbage.
        private int[] agentCluster;          // Indeks kelompok per agen, -1 kalau memakai tujuan sendiri
        private Vector3[] clusterWaypoint;
        private bool[] clusterHasWaypoint;
        private int[] clusterLeader;         // Indeks agen pemimpin
        private float[] clusterLeaderSqr;
        private int[] clusterAreaMask;
        private int clusterCount;
        private float clusterTimer;
        private readonly System.Collections.Generic.Dictionary<(int, int, int), int> cellToCluster =
            new System.Collections.Generic.Dictionary<(int, int, int), int>(64);
        private NavMeshPath sharedPath;
        private readonly Vector3[] cornerBuffer = new Vector3[32];

        // crowdai 0 mematikan pembagian update dan kelompok, untuk dibandingkan dengan perilaku lama (#56).
        private bool staggerEnabled = true;

        // Statistik crowdstat, dihitung per jendela satu detik.
        private const float StatWindowSeconds = 1f;
        private float statTimer;
        private int statTicks, statDestinations;
        private double statTickMs, statTickMsMax;
        private int statFrames;
        private string lastStatLine = string.Empty;

        /// <summary>Ringkasan statistik jendela terakhir (crowdstat).</summary>
        public string LastStatLine => lastStatLine;

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
            agentCluster = new int[cap];
            clusterWaypoint = new Vector3[cap];
            clusterHasWaypoint = new bool[cap];
            clusterLeader = new int[cap];
            clusterLeaderSqr = new float[cap];
            clusterAreaMask = new int[cap];
            for (int i = 0; i < cap; i++) agentCluster[i] = -1;
            sharedPath = new NavMeshPath();
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
            agentCluster[i] = -1;
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
            agentCluster[i] = agentCluster[last];
            agentCluster[last] = -1;
            if (agents[i] != null) agents[i].CrowdIndex = i;
            agents[last] = null;
            count--;
            a.CrowdIndex = -1;
            if (cursor > count) cursor = 0;
        }

        private void Update()
        {
            if (Time.deltaTime <= 0f) return;
            var b = Bal;
            if (b == null) return;
            UpdateStats();
            if (count == 0) return;

            tierTimer -= Time.deltaTime;
            if (tierTimer <= 0f) { tierTimer = b.tierUpdateSeconds; UpdateTiers(b); }

            clusterTimer -= Time.deltaTime;
            if (clusterTimer <= 0f) { clusterTimer = Mathf.Max(0.1f, b.clusterUpdateSeconds); UpdateClusters(b); }

            float now = Time.time;
            int ticks = 0;
            int checkedAgents = 0;
            watch.Reset();
            watch.Start();
            iterating = true;
            int total = count;
            if (!staggerEnabled)
            {
                // crowdai 0: semua agen di-tick setiap frame tanpa batas (perilaku lama, untuk perbandingan #56).
                for (int i = 0; i < count; i++)
                {
                    var a = agents[i];
                    if (a == null || tiers[i] == CrowdTier.Culled) continue;
                    float elapsed = now - lastTickTime[i];
                    lastTickTime[i] = now;
                    a.CrowdTick(Mathf.Min(elapsed, 1f));
                    ticks++;
                }
                checkedAgents = total;
            }
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
            statTicks += ticks;
            double ms = watch.Elapsed.TotalMilliseconds;
            statTickMs += ms;
            if (ms > statTickMsMax) statTickMsMax = ms;
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
            target = ClusterTarget(e, target, b);   // Kelompok jauh memakai titik bersama (#93)
            float threshold = Mathf.Clamp(Vector3.Distance(e.Position, target) * b.repathDistanceFactor, b.repathMinDistance, b.repathMaxDistance);
            if ((target - e.lastDestination).sqrMagnitude < threshold * threshold) return false;
            if (destinationFrame != Time.frameCount) { destinationFrame = Time.frameCount; destinationsThisFrame = 0; }
            if (destinationsThisFrame >= b.maxDestinationsPerFrame) return false;
            destinationsThisFrame++;
            statDestinations++;
            agent.SetDestination(target);
            e.lastDestination = target;
            e.lastRepathTime = Time.time;
            return true;
        }

        // ---------------------------------------------------------------- Kelompok jauh (#93)

        /// <summary>
        /// Langkah 1-2 "Jalur bersama untuk kelompok": petakan agen jauh ke sel, pilih pemimpin terdekat ke pemain,
        /// lalu hitung satu jalur per kelompok dengan sharedPath dan simpan titik pada jarak clusterWaypointLookahead.
        /// Agen dengan areaMask berbeda (#94) masuk kelompok berbeda karena areaMask bagian dari kunci sel.
        /// </summary>
        private void UpdateClusters(CrowdBalance b)
        {
            for (int i = 0; i < count; i++) agentCluster[i] = -1;
            clusterCount = 0;
            cellToCluster.Clear();
            var player = PlayerTargetable.Instance;
            if (!staggerEnabled || player == null) return;

            Vector3 p = player.Position;
            float min2 = b.clusterMinDistance * b.clusterMinDistance;
            float cell = Mathf.Max(0.5f, b.clusterCellSize);
            for (int i = 0; i < count; i++)
            {
                var e = agents[i] as EnemyAI;
                if (e == null || tiers[i] == CrowdTier.Culled) continue;
                var agent = e.Agent;
                if (agent == null || !agent.enabled || !agent.isOnNavMesh) continue;
                Vector3 pos = e.Position;
                float sqr = (pos - p).sqrMagnitude;
                if (sqr <= min2) continue;   // Langkah 4: dekat pemain memakai tujuan sendiri supaya bisa mengepung
                var key = (Mathf.FloorToInt(pos.x / cell), Mathf.FloorToInt(pos.z / cell), agent.areaMask);
                if (!cellToCluster.TryGetValue(key, out int c))
                {
                    c = clusterCount++;
                    cellToCluster[key] = c;
                    clusterLeader[c] = i;
                    clusterLeaderSqr[c] = sqr;
                    clusterAreaMask[c] = agent.areaMask;
                }
                else if (sqr < clusterLeaderSqr[c])
                {
                    clusterLeader[c] = i;
                    clusterLeaderSqr[c] = sqr;
                }
                agentCluster[i] = c;
            }
            if (clusterCount == 0) return;

            Vector3 goal = NavMesh.SamplePosition(p, out var hit, cell, NavMesh.AllAreas) ? hit.position : p;
            for (int c = 0; c < clusterCount; c++)
            {
                clusterHasWaypoint[c] = false;
                Vector3 from = agents[clusterLeader[c]].Position;
                if (!NavMesh.CalculatePath(from, goal, clusterAreaMask[c], sharedPath)) continue;
                if (sharedPath.status == NavMeshPathStatus.PathInvalid) continue;
                int n = sharedPath.GetCornersNonAlloc(cornerBuffer);
                if (n == 0) continue;
                clusterWaypoint[c] = PointAlongCorners(n, b.clusterWaypointLookahead);
                clusterHasWaypoint[c] = true;
            }
        }

        /// <summary>Titik pada jarak lookahead sepanjang sudut jalur. Jalur lebih pendek: sudut terakhir.</summary>
        private Vector3 PointAlongCorners(int n, float lookahead)
        {
            float left = lookahead;
            for (int i = 1; i < n; i++)
            {
                Vector3 a = cornerBuffer[i - 1], d = cornerBuffer[i] - a;
                float len = d.magnitude;
                if (len >= left) return len > 0.0001f ? a + d * (left / len) : cornerBuffer[i];
                left -= len;
            }
            return cornerBuffer[n - 1];
        }

        /// <summary>
        /// Langkah 3: anggota kelompok yang mengejar pemain memakai titik bersama. Tujuan yang bukan pemain (posisi serang
        /// dinding, titik traversal) tetap dipakai apa adanya. Tafsiran: tujuan dianggap "mengejar pemain" kalau berada
        /// dalam satu clusterCellSize dari pemain.
        /// </summary>
        private Vector3 ClusterTarget(EnemyAI e, Vector3 target, CrowdBalance b)
        {
            if (!staggerEnabled) return target;
            int i = e.CrowdIndex;
            if (i < 0 || i >= count || !ReferenceEquals(agents[i], e)) return target;
            int c = agentCluster[i];
            if (c < 0 || c >= clusterCount || !clusterHasWaypoint[c]) return target;
            var player = PlayerTargetable.Instance;
            if (player == null) return target;
            Vector3 p = player.Position;
            if ((e.Position - p).sqrMagnitude <= b.clusterMinDistance * b.clusterMinDistance) return target;
            if ((target - p).sqrMagnitude > b.clusterCellSize * b.clusterCellSize) return target;
            return clusterWaypoint[c];
        }

        // ---------------------------------------------------------------- Statistik dan DevConsole (#56)

        private void UpdateStats()
        {
            statFrames++;
            statTimer += Time.unscaledDeltaTime;
            if (statTimer < StatWindowSeconds) return;
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            // String hanya dibuat saat crowdstat menyala, supaya tidak ada garbage per detik di permainan biasa.
            if (showStats)
            {
                int close = 0, mid = 0, far = 0, culled = 0;
                for (int i = 0; i < count; i++)
                {
                    switch (tiers[i])
                    {
                        case CrowdTier.Close: close++; break;
                        case CrowdTier.Mid: mid++; break;
                        case CrowdTier.Far: far++; break;
                        default: culled++; break;
                    }
                }
                float window = statTimer;
                lastStatLine = $"crowd: Close {close}, Mid {mid}, Far {far}, Culled {culled}, kelompok {clusterCount} | " +
                               $"tick/s {statTicks / window:0} | SetDestination/s {statDestinations / window:0} | " +
                               $"tick ms rata-rata {statTickMs / Mathf.Max(1, statFrames):0.00}, maks {statTickMsMax:0.00}" +
                               (staggerEnabled ? string.Empty : " | crowdai 0");
                if (DevConsole.Instance != null) DevConsole.Instance.Print(lastStatLine);
            }
#endif
            statTimer = 0f;
            statTicks = 0;
            statDestinations = 0;
            statTickMs = 0;
            statTickMsMax = 0;
            statFrames = 0;
        }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        private const string StressEnemyId = "zombie_standar";   // Zombie standar untuk uji stres (HordeCrowdManager.md)
        private const float StressDefaultRadius = 20f;            // "default radius 20 meter" (HordeCrowdManager.md)
        private static bool consoleRegistered;
        private bool showStats;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics() => consoleRegistered = false;

        private void Start() => RegisterConsoleCommands();

        private static void RegisterConsoleCommands()
        {
            var console = DevConsole.Instance;
            if (console == null || consoleRegistered) return;
            consoleRegistered = true;
            // Handler tidak menyimpan rujukan ke objek scene: selalu membaca Instance (DevConsole.md).
            console.Register(new ConsoleCommand("stress", "stress <jumlah> [radius]",
                "Memunculkan zombie standar melingkari pemain untuk uji kerumunan", 1, true, args =>
            {
                if (!int.TryParse(args[0], System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out int n) || n <= 0)
                    return "Cara pakai: stress <jumlah> [radius]";
                float radius = StressDefaultRadius;
                if (args.Length > 1 && (!float.TryParse(args[1], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out radius) || radius <= 0f))
                    return "Cara pakai: stress <jumlah> [radius]";
                var sm = SpawnManager.Instance;
                if (sm == null) return "SpawnManager tidak ada di scene ini";
                int spawned = sm.DebugSpawnRing(StressEnemyId, n, radius);
                return $"stress: {spawned}/{n} {StressEnemyId} pada radius {radius:0.#} m";
            }));
            console.Register(new ConsoleCommand("crowdstat", "crowdstat <0/1>",
                "Menampilkan jumlah per tingkatan, tick/s, SetDestination/s, dan waktu tick tiap detik", 1, false, args =>
            {
                var hc = Instance;
                if (hc == null) return "HordeCrowdManager tidak ada di scene ini";
                if (!ParseToggle(args[0], out bool on)) return "Cara pakai: crowdstat <0/1>";
                hc.showStats = on;
                return on ? "crowdstat menyala" : "crowdstat mati";
            }));
            console.Register(new ConsoleCommand("crowdai", "crowdai <0/1>",
                "0 mematikan pembagian update dan kelompok (perilaku lama), 1 menyalakan lagi", 1, false, args =>
            {
                var hc = Instance;
                if (hc == null) return "HordeCrowdManager tidak ada di scene ini";
                if (!ParseToggle(args[0], out bool on)) return "Cara pakai: crowdai <0/1>";
                hc.staggerEnabled = on;
                if (!on) hc.clusterTimer = 0f;   // Kelompok dikosongkan pada frame berikutnya
                return on ? "crowdai 1: pembagian update menyala" : "crowdai 0: semua agen di-tick tiap frame";
            }));
        }

        private static bool ParseToggle(string s, out bool v)
        {
            switch ((s ?? string.Empty).ToLowerInvariant())
            {
                case "1": case "on": case "true": v = true; return true;
                case "0": case "off": case "false": v = false; return true;
                default: v = false; return false;
            }
        }
#endif

        /// <summary>Kualitas avoidance per tingkatan (HordeCrowdManager.md "Avoidance").</summary>
        public static ObstacleAvoidanceType AvoidanceFor(CrowdTier tier, bool isNearest)
        {
            if (tier == CrowdTier.Far || tier == CrowdTier.Culled) return ObstacleAvoidanceType.NoObstacleAvoidance;
            return isNearest ? ObstacleAvoidanceType.MedQualityObstacleAvoidance : ObstacleAvoidanceType.LowQualityObstacleAvoidance;
        }
    }
}
