using UnityEngine;
using UnityEngine.AI;

namespace SurvivalIsland
{
    /// <summary>State zombie. Nilai mengikuti GLOSSARY.md (#95 menambah Dying).</summary>
    public enum AIState { Idle, Chase, AttackPlayer, AttackBuilding, BurnToAshes, Stagger, Knockback, SeekHiding, Submerged, Emerging, Dormant, Dying }

    /// <summary>Sumber pengali kecepatan, satu nilai per sumber (EnemyAI.md).</summary>
    public enum SpeedSource { LegHit, Trap, EmergeBoost, MilitarySlow, Pounce }

    /// <summary>
    /// Otak NavMeshAgent zombie. Blueprint: left-4-dead/EnemyAI.md (#25, #49, #59, #62, #63, #78, #93, #95, #108, #109).
    /// Claude.md Pilar 3: SEMUA musuh memakai NavMeshAgent. GDD bagian 3: zombie memburu MC dan menyerang base,
    /// terbakar di luar ruangan saat siang (SunlightExposure).
    /// EnemyAI tidak menghitung damage (EnemyAttack), tidak memilih target (ThreatSystem), dan tidak mengembalikan dirinya
    /// ke pool saat mati (EnemyHitReaction lewat SpawnManager.Despawn).
    ///
    /// Catatan Langkah 5 (penyimpangan sementara, dicatat di EnemyAI.md "Catatan Implementasi"):
    /// - BaseNavigation (#78) belum ada. Pemeriksaan terhalang memakai NavMeshAgent.pathStatus milik agen sendiri, dan sasaran
    ///   bangunan dicari dengan OverlapSphere di layer Structure bertag BaseWall/BaseDoor (Claude.md Pilar 3).
    /// - AITraversal (#94) dan ZombieLair (#112) belum ada: respons Surround/Retreat/RockThrow dan lair dilewati (TODO).
    /// - Tanpa HordeCrowdManager di scene, EnemyAI men-tick dirinya sendiri tiap 0,2 detik (cadangan, misalnya ShipPhase).
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent), typeof(EnemyHealth))]
    public class EnemyAI : MonoBehaviour, IPoolable, ICrowdAgent, IThreatAgent, IHitReactive
    {
        private const float FallbackTickSeconds = 0.2f;

        public AIState CurrentState { get; private set; } = AIState.Idle;
        public ITargetable CurrentTarget { get; private set; }
        public NavMeshAgent Agent => agent;
        public EnemyData Data => health != null ? health.Data : null;
        public WaveModifiers Wave => waveModifiers;
        public bool IsBurning { get; private set; }

        /// <summary>false saat Dormant, Submerged, dan BurnToAshes (BaseDefense.md, SleepSystem #67).</summary>
        public bool IsActiveThreat => CurrentState != AIState.Dormant && CurrentState != AIState.Submerged
                                   && CurrentState != AIState.BurnToAshes && CurrentState != AIState.Dying;

        // Dipakai HordeCrowdManager.TrySetDestination
        internal float lastRepathTime;
        internal Vector3 lastDestination = new Vector3(float.PositiveInfinity, 0f, 0f);

        private NavMeshAgent agent;
        private EnemyHealth health;
        private EnemyAttack attack;
        private AISensory sensory;
        private SpecialZombieAI special;
        private EnemyInstance instance;
        private Animator animator;
        private readonly ThreatState threat = new ThreatState();
        private WaveModifiers waveModifiers = WaveModifiers.One;
        private readonly float[] speedMult = new float[5];
        private readonly float[] speedUntil = new float[5];
        private int defaultAreaMask;
        private ObstacleAvoidanceType defaultAvoidance;

        private IDamageable currentBreach;
        private float blockedTimer;
        private float pathCheckTimer;
        private float spawnGraceTimer;
        private float huntTimer;
        private Vector3 huntPoint;
        private float tokenRetryTimer;
        private float fallbackTickTimer;
        private bool registeredCrowd;

        public int CrowdIndex { get; set; } = -1;
        public Vector3 Position => transform.position;
        public bool IsEngaged => CurrentState == AIState.Chase || CurrentState == AIState.AttackPlayer || CurrentState == AIState.AttackBuilding;

        // ---------------------------------------------------------------- IThreatAgent
        public ThreatState Threat => threat;
        public ThreatProfile Profile => Data != null ? Data.threatProfile : null;
        public float SenseRange => Profile != null && Profile.senseRange > 0f ? Profile.senseRange : 60f;
        public bool CanBeEvaluated => CurrentState == AIState.Idle || CurrentState == AIState.Chase
                                   || CurrentState == AIState.AttackPlayer || CurrentState == AIState.AttackBuilding;
        public AISensory Sensory => sensory;
        public bool IsWithinTerritory(Vector3 position) => true;

        private static GameBalanceConfig Bal => GameManager.CurrentBalance;
        private static bool IsShipPhase => GameManager.Instance != null && GameManager.Instance.currentPhase == GamePhase.SinkingShip;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            health = GetComponent<EnemyHealth>();
            attack = GetComponent<EnemyAttack>();
            sensory = GetComponent<AISensory>();
            special = GetComponent<SpecialZombieAI>();
            instance = GetComponent<EnemyInstance>();
            animator = GetComponentInChildren<Animator>();
            defaultAreaMask = agent.areaMask;
            defaultAvoidance = agent.obstacleAvoidanceType;
            for (int i = 0; i < speedMult.Length; i++) speedMult[i] = 1f;
        }

        private void OnEnable()
        {
            health.Damaged += HandleDamaged;
            health.Died += HandleDied;
            if (sensory != null) sensory.LevelChanged += HandleLevelChanged;
        }

        private void OnDisable()
        {
            health.Damaged -= HandleDamaged;
            health.Died -= HandleDied;
            if (sensory != null) sensory.LevelChanged -= HandleLevelChanged;
        }

        private void Update()
        {
            // Cadangan tanpa HordeCrowdManager (ShipPhase atau scene tes).
            if (registeredCrowd || Time.deltaTime <= 0f) return;
            fallbackTickTimer -= Time.deltaTime;
            if (fallbackTickTimer > 0f) return;
            fallbackTickTimer = FallbackTickSeconds;
            CrowdTick(FallbackTickSeconds);
        }

        // ---------------------------------------------------------------- Tick

        public void CrowdTick(float dt)
        {
            // 1.
            if (CurrentState == AIState.Dying || CurrentState == AIState.BurnToAshes || CurrentState == AIState.Submerged
                || CurrentState == AIState.Stagger || CurrentState == AIState.Knockback || CurrentState == AIState.SeekHiding
                || CurrentState == AIState.Emerging)
                return;
            if (!agent.enabled || !agent.isOnNavMesh) return;

            TickSpeedModifiers();
            if (spawnGraceTimer > 0f) spawnGraceTimer -= dt;
            // 2. TODO #94: TraversalAgent.Tick(dt).
            // 3. Kemampuan varian (#25).
            if (special != null && special.TickSpecial(dt)) { UpdateEngaged(); return; }
            if (CurrentState == AIState.Dormant) return;

            // Serangan yang sedang berjalan dibiarkan selesai.
            if (attack != null && attack.Phase != AttackPhase.None) { UpdateEngaged(); return; }

            switch (CurrentState)
            {
                case AIState.AttackBuilding: TickAttackBuilding(dt); break;
                default: TickSensesAndChase(dt); break;
            }
            UpdateEngaged();
        }

        private void UpdateEngaged()
        {
            if (instance != null) instance.IsEngaged = IsEngaged;
        }

        private void TickSensesAndChase(float dt)
        {
            // Indra dan target (hanya Fase 2). Fase 1: target diisi ShipHazardManager lewat SetTarget/WakeUp.
            AwarenessLevel level = sensory != null && !IsShipPhase ? sensory.Level : AwarenessLevel.Aware;
            if (level == AwarenessLevel.Aware && CurrentTarget != null && CurrentTarget.IsTargetable)
            {
                if (CurrentState != AIState.Chase && CurrentState != AIState.AttackPlayer) CurrentState = AIState.Chase;
                TickChase(dt);
                return;
            }

            if (CurrentState == AIState.Chase || CurrentState == AIState.AttackPlayer) CurrentState = AIState.Idle;
            if (level == AwarenessLevel.Suspicious && sensory != null) MoveTo(sensory.LastStimulusPosition);
            else if (DayPhaseTracker.IsNight && AISensoryHub.Instance != null) MoveTo(AISensoryHub.Instance.HuntHint);   // Unaware saat malam
        }

        private void TickChase(float dt)
        {
            var b = Bal;
            var t = CurrentTarget;
            var atk = Data != null ? Data.primaryAttack : null;
            float range = atk != null ? atk.range : 2f;
            float sqr = (t.Position - transform.position).sqrMagnitude;

            // 3. Dalam jangkauan: serang (EnemyAttack memegang izin AttackCoordinator).
            if (sqr <= range * range && attack != null)
            {
                tokenRetryTimer -= dt;
                if (tokenRetryTimer <= 0f)
                {
                    if (attack.TryStartAttack(t)) { CurrentState = AIState.AttackPlayer; return; }
                    tokenRetryTimer = b != null ? b.enemyAttack.tokenRetrySeconds : 0.3f;
                }
                // Tanpa izin: tunggu di lingkaran waitRingRadius dengan sudut dari ID (zombie terlihat mengepung).
                float ring = b != null ? b.enemyAttack.waitRingRadius : 3.5f;
                float ang = (GetInstanceID() & 1023) / 1023f * Mathf.PI * 2f;
                MoveTo(t.Position + new Vector3(Mathf.Cos(ang), 0f, Mathf.Sin(ang)) * ring);
                CurrentState = AIState.Chase;
                return;
            }
            CurrentState = AIState.Chase;

            // 2. Tujuan: posisi target kalau terlihat, posisi terakhir selama loseSightSeconds, lalu memburu tidak tepat.
            Vector3 dest = t.Position;
            bool seen = sensory == null || sensory.CanSee(t) || IsShipPhase;
            if (!seen && b != null && threat.lastSeenTime > 0f)
            {
                if (Time.time - threat.lastSeenTime <= b.threat.loseSightSeconds) dest = threat.lastSeenPosition;
                else
                {
                    huntTimer -= dt;
                    if (huntTimer <= 0f)
                    {
                        huntTimer = b.threat.huntUpdateSeconds;
                        Vector2 off = Random.insideUnitCircle * b.threat.huntOffsetRadius;
                        huntPoint = t.Position + new Vector3(off.x, 0f, off.y);
                    }
                    dest = huntPoint;
                }
            }
            MoveTo(dest);

            // 4. Pemeriksaan terhalang (jalur sementara, lihat catatan kelas).
            CheckBlocked(dt);
        }

        private void CheckBlocked(float dt)
        {
            var b = Bal;
            if (b == null || Data == null || !Data.canAttackBuildings || spawnGraceTimer > 0f || IsShipPhase) return;
            pathCheckTimer -= dt;
            if (pathCheckTimer > 0f) return;
            float interval = b.crowd.fallbackPathCheckIntervalSeconds;
            pathCheckTimer = interval;
            if (agent.pathPending) return;
            if (agent.pathStatus == NavMeshPathStatus.PathComplete) { blockedTimer = 0f; return; }
            blockedTimer += interval;
            if (blockedTimer < b.crowd.fallbackBlockedConfirmSeconds) return;
            if (TryFindBreachTarget(b.crowd.fallbackBreachSearchRadius, out currentBreach))
            {
                blockedTimer = 0f;
                CurrentState = AIState.AttackBuilding;
                return;
            }
            // Tanpa struktur: target dicoret sementara setelah unreachableSeconds (#108). TODO #94: AITraversal.GetResponse.
            if (blockedTimer >= b.threat.unreachableSeconds && ThreatSystem.Instance != null)
            {
                blockedTimer = 0f;
                ThreatSystem.Instance.ReportUnreachable(this);
            }
        }

        private static readonly Collider[] breachBuffer = new Collider[16];

        /// <summary>Cari dinding/pintu terdekat (tag BaseWall atau BaseDoor, Claude.md Pilar 3) yang punya IDamageable.</summary>
        private bool TryFindBreachTarget(float radius, out IDamageable target)
        {
            target = null;
            int structureMask = LayerMask.GetMask(PhysicsMasks.StructureLayer);
            Vector3 center = transform.position + transform.forward * (radius * 0.5f);
            int count = PhysicsQuery.OverlapSphere(center, radius, structureMask, breachBuffer);
            float best = float.MaxValue;
            for (int i = 0; i < count; i++)
            {
                var c = breachBuffer[i];
                if (c == null || !(c.CompareTag("BaseWall") || c.CompareTag("BaseDoor"))) continue;
                var d = c.GetComponentInParent<IDamageable>();
                if (d == null || !d.IsAlive) continue;
                float sqr = (c.ClosestPoint(transform.position) - transform.position).sqrMagnitude;
                if (sqr < best) { best = sqr; target = d; }
            }
            return target != null;
        }

        private void TickAttackBuilding(float dt)
        {
            if (currentBreach == null || !currentBreach.IsAlive)
            {
                currentBreach = null;
                CurrentState = AIState.Chase;
                return;
            }
            var c = currentBreach as Component;
            Vector3 p = c != null ? c.transform.position : transform.position;
            var atk = Data != null ? Data.primaryAttack : null;
            float range = atk != null ? atk.range : 2f;
            Vector3 closest = c != null && c.TryGetComponent(out Collider col) ? col.ClosestPoint(transform.position) : p;
            if ((closest - transform.position).sqrMagnitude <= range * range)
            {
                if (attack != null) attack.TryStartStructureAttack(currentBreach);
            }
            else MoveTo(closest);
        }

        private void MoveTo(Vector3 target)
        {
            if (!agent.enabled || !agent.isOnNavMesh) return;
            if (HordeCrowdManager.Instance != null) HordeCrowdManager.Instance.TrySetDestination(this, target);
            else if ((target - lastDestination).sqrMagnitude > 1f || Time.time - lastRepathTime > 1f)
            {
                agent.SetDestination(target);
                lastDestination = target;
                lastRepathTime = Time.time;
            }
        }

        // ---------------------------------------------------------------- Target dan state

        /// <summary>Hanya ThreatSystem yang memanggil ini, kecuali Fase 1 (ShipHazardManager).</summary>
        public void SetTarget(ITargetable target)
        {
            CurrentTarget = target;
            if (target == null && CurrentState == AIState.AttackPlayer && attack != null) attack.CancelAttack();
        }

        private static int Priority(AIState s)
        {
            switch (s)
            {
                case AIState.BurnToAshes: return 4;
                case AIState.Dying: return 3;
                case AIState.Knockback: return 2;
                case AIState.Stagger: return 1;
                default: return 0;
            }
        }

        /// <summary>Urutan BurnToAshes, Dying, Knockback, Stagger, lalu lainnya (#95). Permintaan lebih rendah ditolak.</summary>
        public bool RequestState(AIState next)
        {
            if (Priority(next) < Priority(CurrentState)) return false;
            if (CurrentState == AIState.Dying || CurrentState == AIState.BurnToAshes) return next == CurrentState || Priority(next) > Priority(CurrentState);
            CurrentState = next;
            UpdateEngaged();
            return true;
        }

        // ---------------------------------------------------------------- IHitReactive (#95)

        public bool RequestReaction(HitReactionKind kind)
        {
            switch (kind)
            {
                case HitReactionKind.Stagger: return RequestState(AIState.Stagger);
                case HitReactionKind.Knockback: return RequestState(AIState.Knockback);
                default: return RequestState(AIState.Dying);
            }
        }

        public void EndReaction()
        {
            if (CurrentState == AIState.Stagger || CurrentState == AIState.Knockback) CurrentState = AIState.Chase;
        }

        // ---------------------------------------------------------------- Kecepatan

        /// <summary>Nilai 1 menghapus sumber itu (EnemyHitReaction memakai LegHit).</summary>
        public void SetSpeedMultiplier(SpeedSource source, float multiplier)
        {
            int i = (int)source;
            speedMult[i] = multiplier;
            speedUntil[i] = float.PositiveInfinity;
            if (Mathf.Approximately(multiplier, 1f)) speedUntil[i] = 0f;
            ApplySpeed();
        }

        /// <summary>Pengali sementara. Tiap sumber hanya menyimpan satu nilai, dan waktunya diperbarui.</summary>
        public void ApplySpeedModifier(SpeedSource id, float multiplier, float seconds)
        {
            if (id == SpeedSource.Trap && Data != null && Data.immuneToTrapSlow) return;
            int i = (int)id;
            speedMult[i] = multiplier;
            speedUntil[i] = Time.time + Mathf.Max(0f, seconds);
            ApplySpeed();
        }

        public void ApplyWaveModifiers(in WaveModifiers m)
        {
            waveModifiers = m;
            ApplySpeed();
        }

        private void TickSpeedModifiers()
        {
            bool changed = false;
            for (int i = 0; i < speedMult.Length; i++)
            {
                if (!Mathf.Approximately(speedMult[i], 1f) && Time.time >= speedUntil[i]) { speedMult[i] = 1f; changed = true; }
            }
            if (changed) ApplySpeed();
        }

        /// <summary>EnemyAI satu-satunya penulis agent.speed di Fase 2 (EnemyAI.md "Rumus agent.speed").</summary>
        private void ApplySpeed()
        {
            if (IsShipPhase || Data == null) return;   // Fase 1: ShipHazardManager mengatur kecepatan zombie kapal
            float baseSpeed = Data.moveSpeed;
            float pounce = speedMult[(int)SpeedSource.Pounce];
            if (!Mathf.Approximately(pounce, 1f))
            {
                // Terkaman Zombie Desa: tepat villagePounceSpeed, tanpa pengali lain dan tanpa batas maxZombieSpeed.
                agent.speed = baseSpeed * pounce;
                return;
            }
            float maxSpeed = Bal != null ? Bal.aiDirector.maxZombieSpeed : 7.8f;
            float v = baseSpeed * waveModifiers.speed * speedMult[(int)SpeedSource.LegHit] * speedMult[(int)SpeedSource.Trap]
                    * speedMult[(int)SpeedSource.MilitarySlow];
            v = Mathf.Min(v, maxSpeed);
            // EmergeBoost dikalikan sesudah batas (#63).
            agent.speed = v * speedMult[(int)SpeedSource.EmergeBoost];
        }

        // ---------------------------------------------------------------- Event per objek

        private void HandleDamaged(DamageInfo info)
        {
            if (!info.byPlayer) return;
            // EnemyAI.md: damage dari pemain memanggil ThreatSystem.ReportAttacker. TODO #76: penyerang NPC.
            if (sensory != null) sensory.ForceAware(info.hitPoint - info.hitDirection * 2f);
            if (ThreatSystem.Instance != null && PlayerTargetable.Instance != null)
                ThreatSystem.Instance.ReportAttacker(this, PlayerTargetable.Instance);
        }

        private void HandleDied(DamageInfo info)
        {
            if (IsBurning) return;   // Urutan terbakar milik SunlightExposure
            if (attack != null) attack.CancelAttack();
            CurrentState = AIState.Dying;
            UpdateEngaged();
            // Mayat dan Despawn(Killed) dipegang EnemyHitReaction (#95).
        }

        private void HandleLevelChanged(AwarenessLevel level)
        {
            if (level == AwarenessLevel.Aware && ThreatSystem.Instance != null) ThreatSystem.Instance.RequestReevaluate(this);
        }

        // ---------------------------------------------------------------- Terbakar, Dormant, bangun

        /// <summary>Dipanggil SunlightExposure sebelum damage Fire (#63).</summary>
        public void EnterBurn()
        {
            IsBurning = true;
            CurrentState = AIState.BurnToAshes;
            if (attack != null) attack.CancelAttack();
            if (agent.enabled) agent.enabled = false;
            var pin = PlayerPinnedState.Instance;
            var self = GetComponent<IPinAttacker>();
            if (pin != null && self != null) pin.ReleaseByAttacker(self, PinReleaseReason.AttackerGone);
            UpdateEngaged();
        }

        /// <summary>Zombie diam (Lumut #25, sarang #112, kapal #75).</summary>
        public void EnterDormant()
        {
            CurrentState = AIState.Dormant;
            if (agent.enabled && agent.isOnNavMesh) { agent.isStopped = true; agent.ResetPath(); }
            UpdateEngaged();
        }

        /// <summary>Bangun dari Dormant ke Chase, dipakai Lumut dan zombie kapal (#75).</summary>
        public void WakeUp(Transform target)
        {
            if (CurrentState != AIState.Dormant && CurrentState != AIState.Idle) return;
            if (agent.enabled && agent.isOnNavMesh) agent.isStopped = false;
            if (target != null)
            {
                var t = target.GetComponentInParent<ITargetable>();
                if (t != null) SetTarget(t);
            }
            if (sensory != null) sensory.ForceAware(target != null ? target.position : transform.position);
            CurrentState = AIState.Chase;
            UpdateEngaged();
        }

        /// <summary>Status Submerged/SeekHiding/Emerging milik VillageZombieHiding (#63).</summary>
        public void SetHidingState(AIState state)
        {
            if (state == AIState.SeekHiding || state == AIState.Submerged || state == AIState.Emerging || state == AIState.Chase)
                CurrentState = state;
            if (instance != null) instance.IsDormantCounted = state == AIState.Submerged;
            UpdateEngaged();
        }

        public void SetTier(CrowdTier tier)
        {
            agent.obstacleAvoidanceType = tier == CrowdTier.Far || tier == CrowdTier.Culled
                ? ObstacleAvoidanceType.NoObstacleAvoidance
                : tier == CrowdTier.Close ? ObstacleAvoidanceType.MedQualityObstacleAvoidance : ObstacleAvoidanceType.LowQualityObstacleAvoidance;
            agent.avoidancePriority = Data != null && Data.isLarge ? 20 : CurrentState == AIState.AttackPlayer ? 30 : 50;
            if (animator != null)
                animator.cullingMode = tier == CrowdTier.Close || tier == CrowdTier.Mid
                    ? AnimatorCullingMode.CullUpdateTransforms : AnimatorCullingMode.CullCompletely;
            // TODO #93: update Animator bergiliran (animStaggerMid/animStaggerFar).
        }

        // ---------------------------------------------------------------- Pool

        public void OnSpawned()
        {
            var b = Bal;
            // 1. Langsung tanpa RequestState, karena state lama bisa Dying atau BurnToAshes.
            CurrentState = AIState.Idle;
            CurrentTarget = null;
            IsBurning = false;
            spawnGraceTimer = b != null ? b.crowd.fallbackSpawnGraceSeconds : 2f;
            for (int i = 0; i < speedMult.Length; i++) { speedMult[i] = 1f; speedUntil[i] = 0f; }
            waveModifiers = WaveModifiers.One;
            currentBreach = null;
            blockedTimer = 0f;
            pathCheckTimer = (GetInstanceID() & 7) * 0.125f;
            huntTimer = 0f;
            tokenRetryTimer = 0f;
            lastRepathTime = -10f;
            lastDestination = new Vector3(float.PositiveInfinity, 0f, 0f);
            threat.Reset(Time.time + (GetInstanceID() & 15) / 16f);

            // 2. Taruh di NavMesh (#88). Gagal = kembali ke pool.
            float sample = b != null ? b.spawn.sampleRadius : 6f;
            if (!NavMesh.SamplePosition(transform.position, out var hit, sample, NavMesh.AllAreas))
            {
                Debug.LogWarning($"[EnemyAI] '{name}' tidak menemukan NavMesh di sekitar titik spawn. Kembali ke pool.", this);
                ReturnSelf(DespawnReason.OffNavMesh);
                return;
            }
            agent.enabled = true;
            agent.Warp(hit.position);
            if (!agent.isOnNavMesh) { ReturnSelf(DespawnReason.OffNavMesh); return; }
            agent.isStopped = false;
            agent.updateRotation = true;
            // 3.
            agent.autoTraverseOffMeshLink = false;
            agent.areaMask = Data != null ? Data.traversalMask : defaultAreaMask;
            agent.obstacleAvoidanceType = defaultAvoidance;
            ApplySpeed();

            // 4. ThreatSystem tidak ada di ShipPhase. HordeCrowdManager boleh tidak ada (cadangan Update).
            if (ThreatSystem.Instance != null) ThreatSystem.Instance.Register(this);
            registeredCrowd = false;
            if (HordeCrowdManager.Instance != null)
            {
                registeredCrowd = HordeCrowdManager.Instance.Register(this);
                if (!registeredCrowd) { ReturnSelf(DespawnReason.Debug); return; }
            }
            fallbackTickTimer = (GetInstanceID() & 7) * 0.025f;
        }

        private void ReturnSelf(DespawnReason reason)
        {
            if (SpawnManager.Instance != null && !IsShipPhase) SpawnManager.Instance.Despawn(gameObject, reason);
            else if (ObjectPooler.Instance != null) ObjectPooler.Instance.ReturnToPool(gameObject);
        }

        public void OnReturned()
        {
            if (agent.enabled) agent.enabled = false;
            if (ThreatSystem.Instance != null) ThreatSystem.Instance.Unregister(this);
            if (HordeCrowdManager.Instance != null) HordeCrowdManager.Instance.Unregister(this);
            registeredCrowd = false;
            // TODO #78: BaseNavigation.ReleaseSlot.
            currentBreach = null;
            blockedTimer = 0f;
            agent.areaMask = defaultAreaMask;
            IsBurning = false;
            CurrentTarget = null;
        }
    }
}
