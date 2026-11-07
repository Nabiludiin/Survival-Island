using UnityEngine;
using UnityEngine.AI;

namespace SurvivalIsland
{
    /// <summary>
    /// Pelintas NavMeshLink dan penjaga agar musuh tetap di NavMesh. Komponen prefab musuh, IPoolable.
    /// Blueprint: left-4-dead/AITraversal.md bagian "Melewati NavMeshLink", "Jatuh dari NavMesh", dan respons RockThrow (#94).
    /// Claude.md Pilar 3: semua musuh memakai NavMeshAgent, dengan autoTraverseOffMeshLink = false.
    ///
    /// Catatan implementasi (sistem pendukung): EnemyAI.CrowdTick (kode Langkah 5) belum memanggil Tick. Sampai baris itu
    /// ditambah, Update menjalankan Tick sendiri kalau CrowdTick tidak memanggilnya pada frame ini.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class TraversalAgent : MonoBehaviour, IPoolable
    {
        public enum LinkKind : byte { Jump, Climb, Drop }

        [Tooltip("AttackData Zombie_Rock_Throw (#109). Kosong = traversal.rockDamage dan rockWindupSeconds.")]
        [SerializeField] private AttackData rockAttack;
        [Tooltip("Titik lepas batu. Kosong = posisi + 1,6 m.")]
        [SerializeField] private Transform throwPoint;
        [SerializeField] private Animator animator;
        [SerializeField] private string climbBool = "Climbing";
        [SerializeField] private string throwTrigger = "Throw";

        public bool IsTraversing { get; private set; }
        public EnemyAI AI { get; private set; }
        public bool HoldsRockSlot { get; internal set; }

        private NavMeshAgent agent;
        private EnemyInstance instance;
        private Collider bodyCollider;
        private int enemyId;

        // Melintas
        private TraversalLinkGate gate;
        private float linkWaitTimer;
        private LinkKind kind;
        private Vector3 start, end;
        private float traverseT, traverseDuration;
        private int avoidArea = -1;
        private float avoidUntil;
        private float avoidOriginalCost;
        private float offNavTimer;
        private int lastTickFrame = -1;

        // Batu
        private float rockCooldownUntil;
        private float rockWindupTimer = -1f;
        private ITargetable rockTarget;

        private static int jumpArea = -2, climbArea, highClimbArea, dropLowArea, dropMidArea, dropHighArea;

        private static TraversalBalance Bal => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.traversal : null;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            AI = GetComponent<EnemyAI>();
            instance = GetComponent<EnemyInstance>();
            bodyCollider = GetComponent<Collider>();
            if (animator == null) animator = GetComponentInChildren<Animator>();
            enemyId = GetInstanceID();
            if (jumpArea == -2) CacheAreas();
        }

        private static void CacheAreas()
        {
            jumpArea = NavMesh.GetAreaFromName("JumpLink");
            climbArea = NavMesh.GetAreaFromName("ClimbLink");
            highClimbArea = NavMesh.GetAreaFromName("HighClimbLink");
            dropLowArea = NavMesh.GetAreaFromName("DropLow");
            dropMidArea = NavMesh.GetAreaFromName("DropMid");
            dropHighArea = NavMesh.GetAreaFromName("DropHigh");
        }

        // ---------------------------------------------------------------- Pool

        public void OnSpawned()
        {
            // Kasus Khusus "Dipakai ulang dari pool".
            IsTraversing = false;
            if (agent.enabled) agent.updatePosition = true;
            offNavTimer = 0f;
            linkWaitTimer = 0f;
            gate = null;
            rockWindupTimer = -1f;
            rockTarget = null;
            rockCooldownUntil = 0f;
            if (instance != null) instance.IsEngaged = false;
            if (AITraversal.Instance != null) AITraversal.Instance.Register(this);
        }

        public void OnReturned()
        {
            // Gate, jatah batu, dan SetAreaCost dikembalikan.
            if (gate != null) gate.Exit(enemyId);
            gate = null;
            IsTraversing = false;
            RestoreAreaCost();
            if (agent.enabled) agent.updatePosition = true;
            rockWindupTimer = -1f;
            rockTarget = null;
            if (AITraversal.Instance != null) AITraversal.Instance.Unregister(this);
            HoldsRockSlot = false;
            if (animator != null && !string.IsNullOrEmpty(climbBool)) animator.SetBool(climbBool, false);
        }

        // ---------------------------------------------------------------- Tick

        private void Update()
        {
            // Cadangan selama EnemyAI.CrowdTick belum memanggil Tick (lihat catatan kelas). Hewan juga lewat sini.
            if (lastTickFrame == Time.frameCount || Time.deltaTime <= 0f) return;
            TickInternal(Time.deltaTime);
        }

        /// <summary>
        /// Dipanggil EnemyAI.CrowdTick. Tiap frame diproses tepat satu kali (CrowdTick atau Update), dengan Time.deltaTime,
        /// supaya lintasan tetap halus walau CrowdTick berjalan bergiliran. Game dijeda: musuh yang melintas berhenti di udara.
        /// </summary>
        public void Tick(float dt)
        {
            if (lastTickFrame == Time.frameCount) return;
            TickInternal(Time.deltaTime);
        }

        private void TickInternal(float dt)
        {
            lastTickFrame = Time.frameCount;
            var b = Bal;
            if (b == null || dt <= 0f) return;
            if (avoidArea >= 0 && Time.time >= avoidUntil) RestoreAreaCost();

            // Mati saat melintas: ragdoll atau mayat diambil alih #95, gate dilepas.
            if (AI != null && (AI.CurrentState == AIState.Dying || AI.CurrentState == AIState.BurnToAshes))
            {
                if (IsTraversing) AbortTraverse();
                return;
            }

            if (IsTraversing) { StepTraverse(b, dt); return; }
            if (!agent.enabled) return;

            if (rockWindupTimer >= 0f) TickRockWindup(dt);

            // Jatuh dari NavMesh.
            if (!agent.isOnNavMesh)
            {
                offNavTimer += dt;
                if (offNavTimer < b.offNavGraceSeconds) return;
                offNavTimer = 0f;
                bool underTerrain = Terrain.activeTerrain != null
                    && transform.position.y < Terrain.activeTerrain.SampleHeight(transform.position) + Terrain.activeTerrain.transform.position.y - 0.5f;
                if (!underTerrain && NavMesh.SamplePosition(transform.position, out var hit, b.offNavSampleRadius, agent.areaMask))
                    agent.Warp(hit.position);
                // Kembali ke pool tanpa EnemyKilledEvent, drop, atau kuota Lucky Machine.
                else if (SpawnManager.Instance != null && instance != null) SpawnManager.Instance.ReturnStuck(instance);
                return;
            }
            offNavTimer = 0f;

            if (agent.isOnOffMeshLink) TryBeginTraverse(b, dt);
        }

        // ---------------------------------------------------------------- Melewati NavMeshLink

        private void TryBeginTraverse(TraversalBalance b, float dt)
        {
            var link = agent.currentOffMeshLinkData;
            // 1.
            var owner = link.owner as Component;
            var linkGate = owner != null ? owner.GetComponent<TraversalLinkGate>() : null;
            // 2. Link penuh: tunggu sampai linkWaitSeconds, lalu hindari area itu selama linkAvoidSeconds.
            if (linkGate != null && !linkGate.TryEnter(enemyId))
            {
                linkWaitTimer += dt;
                if (linkWaitTimer < b.linkWaitSeconds) return;
                linkWaitTimer = 0f;
                int area = AreaOf(owner);
                agent.ResetPath();
                if (area >= 0)
                {
                    RestoreAreaCost();
                    avoidArea = area;
                    avoidOriginalCost = agent.GetAreaCost(area);
                    agent.SetAreaCost(area, b.linkAvoidCost);
                    avoidUntil = Time.time + b.linkAvoidSeconds;
                }
                return;
            }
            linkWaitTimer = 0f;
            gate = linkGate;
            // 3.
            start = transform.position;
            end = link.endPos + Vector3.up * agent.baseOffset;
            kind = KindOf(AreaOf(owner), start, end);
            agent.updatePosition = false;
            IsTraversing = true;
            if (instance != null) instance.IsEngaged = true;
            traverseT = 0f;
            float dist = Vector3.Distance(start, end);
            switch (kind)
            {
                case LinkKind.Climb:
                    traverseDuration = dist / Mathf.Max(0.1f, b.climbSpeed);
                    if (animator != null && !string.IsNullOrEmpty(climbBool)) animator.SetBool(climbBool, true);
                    break;
                case LinkKind.Drop:
                    float h = Mathf.Max(0.1f, start.y - end.y);
                    traverseDuration = Mathf.Sqrt(2f * h / Mathf.Max(0.1f, -Physics.gravity.y));
                    break;
                default:
                    traverseDuration = b.jumpSeconds;
                    break;
            }
            traverseDuration = Mathf.Max(0.05f, traverseDuration);
        }

        private static int AreaOf(Component owner)
        {
            var navLink = owner as Unity.AI.Navigation.NavMeshLink;
            return navLink != null ? navLink.area : -1;
        }

        private static LinkKind KindOf(int area, Vector3 s, Vector3 e)
        {
            if (area >= 0 && (area == climbArea || area == highClimbArea)) return LinkKind.Climb;
            if (area >= 0 && (area == dropLowArea || area == dropMidArea || area == dropHighArea)) return LinkKind.Drop;
            if (area >= 0 && area == jumpArea) return LinkKind.Jump;
            // Area tidak dikenal: lompat.
            return LinkKind.Jump;
        }

        private void StepTraverse(TraversalBalance b, float dt)
        {
            // 4. Musuh tidak menerima damage jatuh; stagger (#95) hanya di tanah, jadi lintasan tetap selesai.
            traverseT += dt;
            float u = Mathf.Clamp01(traverseT / traverseDuration);
            Vector3 p;
            switch (kind)
            {
                case LinkKind.Climb:
                    p = Vector3.Lerp(start, end, u);
                    break;
                case LinkKind.Drop:
                    // Busur gravitasi: mendatar linier, tinggi mengikuti jatuh bebas.
                    p = Vector3.Lerp(start, end, u);
                    float fall = start.y - end.y;
                    p.y = start.y - fall * u * u;
                    break;
                default:
                    // Parabola dengan puncak setengah meter di atas titik tertinggi.
                    p = Vector3.Lerp(start, end, u);
                    float apex = Mathf.Max(start.y, end.y) + 0.5f - Mathf.Lerp(start.y, end.y, 0.5f);
                    p.y += 4f * apex * u * (1f - u);
                    break;
            }
            transform.position = p;
            Vector3 flat = end - start; flat.y = 0f;
            if (flat.sqrMagnitude > 0.001f) transform.rotation = Quaternion.LookRotation(flat);
            if (u < 1f) return;
            // 5.
            FinishTraverse();
        }

        private void FinishTraverse()
        {
            IsTraversing = false;
            if (agent.enabled && agent.isOnOffMeshLink) agent.CompleteOffMeshLink();
            if (agent.enabled) { agent.Warp(end); agent.updatePosition = true; }
            if (gate != null) gate.Exit(enemyId);
            gate = null;
            if (animator != null && !string.IsNullOrEmpty(climbBool)) animator.SetBool(climbBool, false);
        }

        private void AbortTraverse()
        {
            IsTraversing = false;
            if (gate != null) gate.Exit(enemyId);
            gate = null;
            if (agent.enabled) agent.updatePosition = true;
        }

        private void RestoreAreaCost()
        {
            if (avoidArea >= 0 && agent.enabled) agent.SetAreaCost(avoidArea, avoidOriginalCost);
            avoidArea = -1;
        }

        // ---------------------------------------------------------------- Respons tak terjangkau

        /// <summary>
        /// Dipanggil EnemyAI saat Chase setelah BaseNavigation tidak menemukan struktur (#78). true berarti respons sudah
        /// mengatur gerak, jadi EnemyAI tidak perlu MoveTo sendiri.
        /// </summary>
        public bool TickUnreachable(ITargetable target)
        {
            var at = AITraversal.Instance;
            if (at == null || AI == null || target == null) return false;
            var response = at.GetResponse(AI);
            if (response != ReachResponse.RockThrow && HoldsRockSlot && rockWindupTimer < 0f) at.ReleaseRockSlot(this);
            switch (response)
            {
                case ReachResponse.Surround:
                    MoveTo(at.GetSurroundPoint(AI, target.Position));
                    return true;
                case ReachResponse.Retreat:
                    if (at.TryGetRetreatPoint(AI, target, out var p)) MoveTo(p);
                    return true;
                case ReachResponse.RockThrow:
                    TryStartRockThrow(target);
                    return true;
                default:
                    return false;
            }
        }

        private void MoveTo(Vector3 p)
        {
            if (!agent.enabled || !agent.isOnNavMesh) return;
            agent.isStopped = false;
            if (HordeCrowdManager.Instance != null && AI != null) HordeCrowdManager.Instance.TrySetDestination(AI, p);
            else agent.SetDestination(p);
        }

        public bool CanThrowRockNow => Time.time >= rockCooldownUntil && rockWindupTimer < 0f;

        private void TryStartRockThrow(ITargetable target)
        {
            if (rockWindupTimer >= 0f || Time.time < rockCooldownUntil) return;
            var b = Bal;
            rockTarget = target;
            rockWindupTimer = rockAttack != null ? rockAttack.windupSeconds : (b != null ? b.rockWindupSeconds : 1f);
            if (agent.enabled && agent.isOnNavMesh) agent.isStopped = true;
            if (animator != null && !string.IsNullOrEmpty(throwTrigger)) animator.SetTrigger(throwTrigger);
        }

        private void TickRockWindup(float dt)
        {
            if (rockTarget != null && rockTarget.IsTargetable)
            {
                Vector3 look = rockTarget.Position - transform.position; look.y = 0f;
                if (look.sqrMagnitude > 0.01f) transform.rotation = Quaternion.LookRotation(look);
            }
            rockWindupTimer -= dt;
            if (rockWindupTimer > 0f) return;
            rockWindupTimer = -1f;
            var b = Bal;
            rockCooldownUntil = Time.time + (b != null ? b.rockCooldownSeconds : 8f);
            if (agent.enabled && agent.isOnNavMesh) agent.isStopped = false;
            if (rockTarget == null || !rockTarget.IsTargetable || b == null) return;
            ThrowRock(b, rockTarget);
        }

        private void ThrowRock(TraversalBalance b, ITargetable target)
        {
            var pooler = ObjectPooler.Instance;
            if (pooler == null || !pooler.HasPool(ThrownRock.PoolTag)) return;
            Vector3 from = throwPoint != null ? throwPoint.position : transform.position + Vector3.up * 1.6f;
            // Arah memperkirakan gerak pemain.
            Vector3 aim = target.AimPoint;
            float dist = Vector3.Distance(from, aim);
            float flight = dist / Mathf.Max(1f, b.rockSpeed);
            if (target.Kind == TargetKind.Player && IslandPlayerController.Instance != null) aim += IslandPlayerController.Instance.Velocity * flight;
            Vector3 v = (aim - from - 0.5f * Physics.gravity * flight * flight) / Mathf.Max(0.05f, flight);
            var rock = pooler.SpawnFromPool<ThrownRock>(ThrownRock.PoolTag, from, Quaternion.LookRotation(v.sqrMagnitude > 0.01f ? v : transform.forward));
            if (rock == null) return;
            float dmg = rockAttack != null ? rockAttack.damage : b.rockDamage;
            float inf = rockAttack != null ? rockAttack.infection : 0f;
            string id = rockAttack != null ? rockAttack.attackId : "Zombie_Rock_Throw";
            rock.Launch(v, dmg, inf, id, bodyCollider);
        }
    }
}
