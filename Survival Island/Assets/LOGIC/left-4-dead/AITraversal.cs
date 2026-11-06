using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SurvivalIsland
{
    public enum ReachResponse { None, Surround, Retreat, RockThrow, AttackStructure }

    /// <summary>
    /// Pemeriksa keterjangkauan pemain, pemilih respons bertahap, dan hukuman berkemah. Blueprint: left-4-dead/AITraversal.md (#94).
    /// Usulan, belum ada di GDD; mendukung ancaman malam (GDD bagian 3) dan fungsi base (bagian 4). Usulan disetujui
    /// developer (#85): kemampuan per jenis, respons mengepung lalu batu lalu mundur, atap tidak bisa dinaiki, batu dengan
    /// batas 3 pelempar, hukuman berkemah 45 dan 90 detik yang tidak berlaku di dalam base.
    /// Singleton di IslandPhase. Satu pemeriksaan per nilai traversalMask, bukan per musuh.
    ///
    /// Catatan implementasi (sistem pendukung):
    /// - AIDirector.RequestCampCounter (#27) belum ada: pembalas diminta lewat SpawnManager.RequestWave dengan forcedEnemyId
    ///   dari RegionSpawnTable.campCounterEnemyId (reason "CampCounter").
    /// - SleepStateChangedEvent (#67) dan status berenang (#13) belum ada: campTimer tidak berhenti saat tidur (TODO).
    /// - EnemyAI (kode Langkah 5) belum membaca GetResponse; TraversalAgent.TickUnreachable sudah menjalankan responsnya.
    /// </summary>
    public class AITraversal : MonoBehaviour
    {
        public const string StandardZombieId = "zombie_standar";
        private const int MaxMasks = 16;

        public static AITraversal Instance { get; private set; }

        [Tooltip("Tabel per RegionType, untuk campCounterEnemyId (#62). Aset yang sama dengan SpawnManager.")]
        [SerializeField] private RegionSpawnTable[] tablesByRegion = new RegionSpawnTable[4];

        public float CampTimer => campTimer;

        private readonly List<TraversalAgent> agents = new List<TraversalAgent>(64);
        private readonly int[] masks = new int[MaxMasks];
        private readonly float[] unreachableTimer = new float[MaxMasks];
        private readonly float[] lastProbeTime = new float[MaxMasks];
        private readonly Vector3[] probeOrigin = new Vector3[MaxMasks];
        private readonly bool[] seenThisRound = new bool[MaxMasks];
        private int maskCount;
        private int standardMask = int.MinValue;
        private readonly Queue<int> probeQueue = new Queue<int>(MaxMasks);
        private readonly List<TraversalAgent> rockThrowers = new List<TraversalAgent>(8);
        private NavMeshPath sharedPath;
        private float probeTimer;

        private float campTimer;
        private float shotWindowUntil;
        private int lastAmmo = -1;
        private bool tier2Fired;
        private float nextRepeatAt;

        private static TraversalBalance Bal => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.traversal : null;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Debug.LogError("[AITraversal] Lebih dari satu instance."); enabled = false; return; }
            Instance = this;
            sharedPath = new NavMeshPath();
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void OnEnable()
        {
            EventChannel<NightfallEvent>.Raised += HandleNightfall;
            EventChannel<DaybreakEvent>.Raised += HandleDaybreak;
            EventChannel<AmmoChangedEvent>.Raised += HandleAmmo;
        }

        private void OnDisable()
        {
            EventChannel<NightfallEvent>.Raised -= HandleNightfall;
            EventChannel<DaybreakEvent>.Raised -= HandleDaybreak;
            EventChannel<AmmoChangedEvent>.Raised -= HandleAmmo;
        }

        // ---------------------------------------------------------------- Daftar agen

        public void Register(TraversalAgent a)
        {
            if (a == null || agents.Contains(a)) return;
            agents.Add(a);
            var d = a.AI != null ? a.AI.Data : null;
            if (d != null && d.enemyId == StandardZombieId) standardMask = d.traversalMask;
        }

        public void Unregister(TraversalAgent a)
        {
            agents.Remove(a);
            ReleaseRockSlot(a);
        }

        // ---------------------------------------------------------------- Keterjangkauan

        private int IndexOfMask(int mask)
        {
            for (int i = 0; i < maskCount; i++) if (masks[i] == mask) return i;
            return -1;
        }

        /// <summary>Waktu tak terjangkau untuk satu traversalMask. 0 kalau terjangkau atau belum diperiksa.</summary>
        public float GetUnreachableSeconds(int traversalMask)
        {
            int i = IndexOfMask(traversalMask);
            return i >= 0 ? unreachableTimer[i] : 0f;
        }

        private void Update()
        {
            var b = Bal;
            if (b == null || Time.deltaTime <= 0f) return;
            // Game dijeda: Time.deltaTime 0, semua timer berhenti.
            probeTimer -= Time.deltaTime;
            if (probeTimer <= 0f)
            {
                probeTimer = b.probeIntervalSeconds;
                CollectProbes();
            }
            int done = 0;
            while (done < b.maxProbesPerFrame && probeQueue.Count > 0)
            {
                int i = probeQueue.Dequeue();
                if (i < maskCount) { Probe(i, b); done++; }
            }
            TickCamp(b, Time.deltaTime);
        }

        /// <summary>Langkah 1: traversalMask unik dari musuh yang Chase, dengan musuh terdekat sebagai titik asal.</summary>
        private void CollectProbes()
        {
            var player = PlayerTargetable.Instance;
            for (int i = 0; i < maskCount; i++) seenThisRound[i] = false;
            if (player == null) return;
            Vector3 pp = player.Position;
            float[] best = distBuffer;
            for (int a = 0; a < agents.Count; a++)
            {
                var ag = agents[a];
                if (ag == null || ag.AI == null || ag.AI.Data == null || ag.AI.CurrentState != AIState.Chase || ag.IsTraversing) continue;
                var agent = ag.AI.Agent;
                if (agent == null || !agent.enabled || !agent.isOnNavMesh) continue;
                int mask = ag.AI.Data.traversalMask;
                int idx = IndexOfMask(mask);
                if (idx < 0)
                {
                    if (maskCount >= MaxMasks) continue;
                    idx = maskCount++;
                    masks[idx] = mask;
                    unreachableTimer[idx] = 0f;
                    lastProbeTime[idx] = Time.time;
                }
                float d = (ag.transform.position - pp).sqrMagnitude;
                if (!seenThisRound[idx] || d < best[idx]) { best[idx] = d; probeOrigin[idx] = agent.nextPosition; seenThisRound[idx] = true; }
            }
            for (int i = 0; i < maskCount; i++)
            {
                if (seenThisRound[i]) probeQueue.Enqueue(i);
                else unreachableTimer[i] = 0f;   // Tidak ada pengejar dengan mask ini
            }
        }

        private readonly float[] distBuffer = new float[MaxMasks];

        private void Probe(int i, TraversalBalance b)
        {
            var player = PlayerTargetable.Instance;
            float now = Time.time;
            float elapsed = now - lastProbeTime[i];
            lastProbeTime[i] = now;
            if (player == null) { unreachableTimer[i] = 0f; return; }
            // Base adalah tempat berlindung di GDD (bagian 4), dan zombie sudah punya AttackBuilding: tidak dihitung tak terjangkau.
            if (BaseNavigation.Instance != null && BaseNavigation.Instance.IsPlayerInsideBase) { unreachableTimer[i] = 0f; return; }
            bool reachable = false;
            int mask = masks[i];
            // 2.
            if (NavMesh.SamplePosition(player.Position, out var hit, b.sampleRadius, mask))
            {
                // 3-4. Jalur lengkap DAN ujungnya dekat pemain (pemain di atas batu bisa menghasilkan jalur ke kaki batu).
                if (NavMesh.CalculatePath(probeOrigin[i], hit.position, mask, sharedPath) && sharedPath.status == NavMeshPathStatus.PathComplete)
                {
                    int n = sharedPath.GetCornersNonAlloc(cornerBuffer);
                    Vector3 last = n > 0 ? cornerBuffer[Mathf.Min(n, cornerBuffer.Length) - 1] : probeOrigin[i];
                    reachable = (last - player.Position).sqrMagnitude <= b.reachTolerance * b.reachTolerance;
                }
            }
            // 5.
            unreachableTimer[i] = reachable ? 0f : unreachableTimer[i] + elapsed;
        }

        private readonly Vector3[] cornerBuffer = new Vector3[64];

        // ---------------------------------------------------------------- Respons

        /// <summary>Respons bertahap untuk musuh yang Chase (tabel "Respons bertahap"). Dibaca EnemyAI dan ThreatSystem.</summary>
        public ReachResponse GetResponse(EnemyAI e)
        {
            var b = Bal;
            if (e == null || e.Data == null || b == null) return ReachResponse.None;
            // Kapan saja: struktur yang sudah dipilih #78 menang.
            if (BaseNavigation.Instance != null && BaseNavigation.Instance.IsBreachValid(e)) return ReachResponse.AttackStructure;
            float t = GetUnreachableSeconds(e.Data.traversalMask);
            if (t <= 0f) return ReachResponse.None;
            var ag = e.GetComponent<TraversalAgent>();
            bool isTribal = e.GetComponent<TribalBehavior>() != null;   // Tribal memakai jerat (#25), bukan batu atau mundur
            if (!isTribal && t >= b.rockAfterSeconds && ag != null && CanRockThrow(ag, e)) return ReachResponse.RockThrow;
            bool isThrower = ag != null && ag.HoldsRockSlot;
            if (t >= b.retreatAfterSeconds && !isThrower && !isTribal) return ReachResponse.Retreat;
            if (t >= b.surroundAfterSeconds) return ReachResponse.Surround;
            return ReachResponse.None;
        }

        private int MaxThrowers
        {
            get
            {
                var b = Bal;
                int max = b != null ? b.maxRockThrowers : 3;
                // Hukuman berkemah tier 1: jumlah pelempar dikali 2.
                if (b != null && campTimer >= b.campTier1Seconds) max *= 2;
                return max;
            }
        }

        /// <summary>Punya jatah, garis pandang bebas, dan jarak sesuai.</summary>
        private bool CanRockThrow(TraversalAgent ag, EnemyAI e)
        {
            var b = Bal;
            var player = PlayerTargetable.Instance;
            if (player == null || b == null) return false;
            float d = Vector3.Distance(e.transform.position, player.Position);
            if (d < b.rockRangeMin || d > b.rockRangeMax) return false;
            if (!ag.HoldsRockSlot)
            {
                if (rockThrowers.Count >= MaxThrowers) return false;
                if (!HasLineOfSight(e.transform.position + Vector3.up * 1.6f, player.AimPoint)) return false;
                rockThrowers.Add(ag);
                ag.HoldsRockSlot = true;
                return true;
            }
            return ag.CanThrowRockNow || HasLineOfSight(e.transform.position + Vector3.up * 1.6f, player.AimPoint);
        }

        /// <summary>Jatah batu dikembalikan saat musuh mati, kembali ke pool, atau tidak lagi melempar.</summary>
        public void ReleaseRockSlot(TraversalAgent ag)
        {
            if (ag == null) return;
            rockThrowers.Remove(ag);
            ag.HoldsRockSlot = false;
        }

        private static bool HasLineOfSight(Vector3 from, Vector3 to)
        {
            Vector3 dir = to - from;
            float len = dir.magnitude;
            return len < 0.01f || !PhysicsQuery.Raycast(new Ray(from, dir / len), out _, len, PhysicsMasks.AIVision);
        }

        /// <summary>Surround: titik pada lingkaran surroundRadius, sudut dari ID musuh.</summary>
        public Vector3 GetSurroundPoint(EnemyAI e, Vector3 center)
        {
            var b = Bal;
            float r = b != null ? b.surroundRadius : 6f;
            float ang = (e.GetInstanceID() & 1023) / 1023f * Mathf.PI * 2f;
            Vector3 p = center + new Vector3(Mathf.Cos(ang), 0f, Mathf.Sin(ang)) * r;
            int mask = e.Agent != null ? e.Agent.areaMask : NavMesh.AllAreas;
            return NavMesh.SamplePosition(p, out var hit, r, mask) ? hit.position : p;
        }

        /// <summary>Retreat: titik di luar garis pandang pemain (AIVision), paling banyak retreatMaxTries per tick.</summary>
        public bool TryGetRetreatPoint(EnemyAI e, ITargetable from, out Vector3 point)
        {
            point = e.transform.position;
            var b = Bal;
            if (b == null || from == null) return false;
            Vector3 away = e.transform.position - from.Position; away.y = 0f;
            if (away.sqrMagnitude < 0.01f) away = e.transform.forward;
            away.Normalize();
            int mask = e.Agent != null ? e.Agent.areaMask : NavMesh.AllAreas;
            for (int i = 0; i < b.retreatMaxTries; i++)
            {
                float ang = Random.Range(-60f, 60f);
                Vector3 cand = from.Position + Quaternion.Euler(0f, ang, 0f) * away * b.retreatDistance;
                if (!NavMesh.SamplePosition(cand, out var hit, 3f, mask)) continue;
                if (HasLineOfSight(hit.position + Vector3.up * 1.6f, from.AimPoint)) continue;
                point = hit.position;
                return true;
            }
            return false;
        }

        // ---------------------------------------------------------------- Hukuman berkemah

        private void TickCamp(TraversalBalance b, float dt)
        {
            var player = PlayerTargetable.Instance;
            bool night = DayPhaseTracker.IsNight;
            bool inside = BaseNavigation.Instance != null && BaseNavigation.Instance.IsPlayerInsideBase;
            bool pinned = PlayerPinnedState.Instance != null && PlayerPinnedState.Instance.IsPinned;
            // TODO(SleepSystem): berhenti selama SleepStateChangedEvent.isSleeping (#67). TODO(SwimmingSystem): tidak saat berenang (#13).
            float unreach = standardMask != int.MinValue ? GetUnreachableSeconds(standardMask) : 0f;
            // Siang, terjangkau, di dalam base, atau tertahan: timer 0.
            // Berpindah ke titik terjangkau juga membuat unreach 0, jadi ikut di sini.
            if (player == null || !night || inside || pinned || unreach <= 0f) { ResetCamp(player); return; }
            float mult = Time.time < shotWindowUntil ? b.campShotMultiplier : 1f;
            campTimer += dt * mult;

            if (campTimer >= b.campTier2Seconds)
            {
                if (!tier2Fired) { tier2Fired = true; RequestCampCounter(b); nextRepeatAt = campTimer + b.campRepeatSeconds; }
                else if (campTimer >= nextRepeatAt) { RequestCampCounter(b); nextRepeatAt = campTimer + b.campRepeatSeconds; }
            }
        }

        private void ResetCamp(PlayerTargetable player)
        {
            campTimer = 0f;
            tier2Fired = false;
            nextRepeatAt = 0f;
        }

        /// <summary>Tier 2: varian pembalas per region (tabel "Hukuman berkemah").</summary>
        private void RequestCampCounter(TraversalBalance b)
        {
            int r = (int)ZoneTracker.Active;   // TODO(RegionStreaming): GetRegionAt(pemain) (#88)
            var table = tablesByRegion != null && r >= 0 && r < tablesByRegion.Length ? tablesByRegion[r] : null;
            string id = table != null && !string.IsNullOrEmpty(table.campCounterEnemyId) ? table.campCounterEnemyId : StandardZombieId;
            // TODO(AIDirector): AIDirector.Instance.RequestCampCounter(id, campCounterCount) (#27).
            var spawn = SpawnManager.Instance;
            if (spawn == null) return;
            int day = GameManager.Instance != null ? GameManager.Instance.currentDay : 1;
            var mods = AIDirector.Instance != null ? AIDirector.Instance.GetPostTargetModifiers(day) : WaveModifiers.One;
            var req = new WaveRequest
            {
                count = b.campCounterCount, maxLarge = int.MaxValue, specialShare = 1f, modifiers = mods,
                reason = "CampCounter", forcedEnemyId = id, origin = null
            };
            spawn.RequestWave(in req);
        }

        // ---------------------------------------------------------------- Event

        private void HandleAmmo(AmmoChangedEvent e)
        {
            var b = Bal;
            int total = e.inMagazine;
            // Peluru di magasin berkurang = menembak: timer berkemah dipercepat selama campShotWindowSeconds.
            if (lastAmmo >= 0 && total < lastAmmo && b != null) shotWindowUntil = Time.time + b.campShotWindowSeconds;
            lastAmmo = total;
        }

        private void HandleNightfall(NightfallEvent e) => ResetCamp(PlayerTargetable.Instance);

        private void HandleDaybreak(DaybreakEvent e)
        {
            // Siang: campTimer 0. Zombie sudah terbakar, dan hewan memakai #61.
            ResetCamp(PlayerTargetable.Instance);
            for (int i = 0; i < maskCount; i++) unreachableTimer[i] = 0f;
        }

        // ---------------------------------------------------------------- DevConsole (#56)

        /// <summary>reachcheck: hasil pemeriksaan tiap mask.</summary>
        public string DebugReachCheck()
        {
            var sb = new System.Text.StringBuilder();
            for (int i = 0; i < maskCount; i++) sb.Append("mask ").Append(masks[i]).Append(": takTerjangkau ").Append(unreachableTimer[i].ToString("0.0")).Append(" dtk\n");
            sb.Append("campTimer ").Append(campTimer.ToString("0.0")).Append(", pelemparBatu ").Append(rockThrowers.Count).Append('/').Append(MaxThrowers);
            return sb.ToString();
        }
    }
}
