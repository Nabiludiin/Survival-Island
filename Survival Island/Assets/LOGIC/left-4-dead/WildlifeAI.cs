using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace SurvivalIsland
{
    /// <summary>State hewan (GLOSSARY.md, WildlifeAI.md #61).</summary>
    public enum AnimalState { Wander, Alert, Chase, Attack, ReturnToTerritory, FleeToDen, TargetUnreachable }

    /// <summary>
    /// AI hewan buas siang hari. Blueprint: left-4-dead/WildlifeAI.md (#61).
    /// GDD bagian 3: ancaman siang, hewan menjaga teritori dan resource, kembali ke sarang saat malam, dan berburu.
    /// Claude.md Pilar 3: siang hari hewan buas dengan state Wander dan Chase (teritorial), memakai NavMeshAgent.
    /// Jaguar menahan pemain lewat PlayerPinnedState (GDD bagian 3, Diterkam).
    ///
    /// Catatan Langkah 5 (TODO): Jaguar menunggu di pohon (AmbushPerch) belum ada, jadi terkaman hanya dari tanah;
    /// Gagak terbang (baseOffset, CrowFlock) belum ada, jadi FlockDive memakai gigitan biasa; api (#97).
    /// Suara tanda Alert dan serangan khusus lewat ThreatAudioEmitter.PlayAlertTell (#114).
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent), typeof(EnemyHealth))]
    public class WildlifeAI : MonoBehaviour, IPoolable, IThreatAgent, IHitReactive, IPinAttacker
    {
        public AnimalState State { get; private set; } = AnimalState.Wander;
        public ITargetable CurrentTarget { get; private set; }

        [SerializeField] private Transform headPoint;

        private NavMeshAgent agent;
        private EnemyHealth health;
        private EnemyAttack attack;
        private ThreatAudioEmitter emitter;   // Suara tanda (#114). Boleh tidak ada.
        private AnimalData data;
        private NavMeshPath path;
        private readonly ThreatState threat = new ThreatState();
        private readonly float[] speedMult = { 1f, 1f, 1f, 1f, 1f };
        private float currentBaseSpeed;
        private bool reactionPaused;
        private bool dead;

        private Vector3 homePoint;
        private Vector3 chaseStartPoint;
        private int groupId;
        private float stateTimer;
        private float wanderWaitUntil;
        private float reaggroUntil;
        private float pathCheckTimer;
        private float unreachableTimer;
        private float specialCooldownUntil;
        private float pinDamageTimer;
        private float pounceDamageTaken;
        private bool isPinning;
        private Vector3 chargeTarget;
        private bool chargeHit;
        private int attackStep;   // 0 = tidak ada, 1 = tanda, 2 = gerak, 3 = pusing/meleset
        private Coroutine fsm;
        private static readonly Collider[] chargeBuffer = new Collider[8];

        private static WildlifeBalance Bal => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.wildlife : null;

        // ---------------------------------------------------------------- IThreatAgent
        public ThreatState Threat => threat;
        public Vector3 Position => transform.position;
        public ThreatProfile Profile => data != null ? data.threatProfile : null;
        public float SenseRange => Profile != null && Profile.senseRange > 0f ? Profile.senseRange : (data != null ? data.aggroRadius : 15f);
        public bool CanBeEvaluated => !dead && !reactionPaused && State != AnimalState.FleeToDen && State != AnimalState.ReturnToTerritory;
        public AISensory Sensory => null;
        public bool IsWithinTerritory(Vector3 position) => data == null || (position - homePoint).sqrMagnitude <= data.leashDistance * data.leashDistance;
        public void SetTarget(ITargetable target) => CurrentTarget = target;

        // ---------------------------------------------------------------- IPinAttacker (Jaguar)
        public string AttackerId => data != null ? data.enemyId : name;
        public Transform LookTarget => headPoint != null ? headPoint : transform;
        public bool IsPinValid => isPinning && !dead && isActiveAndEnabled;
        public IDamageable DamageTarget => health;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            health = GetComponent<EnemyHealth>();
            attack = GetComponent<EnemyAttack>();
            emitter = GetComponent<ThreatAudioEmitter>();
            data = health.Data as AnimalData;
            path = new NavMeshPath();
            if (data == null) Debug.LogError($"[WildlifeAI] '{name}' butuh AnimalData di EnemyHealth.", this);
        }

        private void OnEnable()
        {
            health.Damaged += HandleDamaged;
            health.Died += HandleDied;
            EventChannel<NoiseEmittedEvent>.Raised += HandleNoise;
            EventChannel<HourChangedEvent>.Raised += HandleHour;
        }

        private void OnDisable()
        {
            health.Damaged -= HandleDamaged;
            health.Died -= HandleDied;
            EventChannel<NoiseEmittedEvent>.Raised -= HandleNoise;
            EventChannel<HourChangedEvent>.Raised -= HandleHour;
        }

        /// <summary>Diisi SpawnManager setelah keluar dari pool.</summary>
        public void SetHome(Vector3 home, int group)
        {
            homePoint = home;
            groupId = group;
        }

        // ---------------------------------------------------------------- FSM

        private IEnumerator FsmLoop()
        {
            while (true)
            {
                var b = Bal;
                float tick = b != null ? b.tickSeconds : 0.25f;
                yield return new WaitForSeconds(tick);
                if (dead) yield break;
                if (reactionPaused || b == null || data == null || !agent.enabled || !agent.isOnNavMesh) continue;
                stateTimer += tick;
                switch (State)
                {
                    case AnimalState.Wander: TickWander(b); break;
                    case AnimalState.Alert: TickAlert(b); break;
                    case AnimalState.Chase: TickChase(b, tick); break;
                    case AnimalState.Attack: TickAttack(b, tick); break;
                    case AnimalState.ReturnToTerritory: TickReturn(b, tick); break;
                    case AnimalState.TargetUnreachable: TickUnreachable(b, tick); break;
                    case AnimalState.FleeToDen: TickFlee(b); break;
                }
                var inst = GetComponent<EnemyInstance>();
                if (inst != null) inst.IsEngaged = State == AnimalState.Chase || State == AnimalState.Attack;
            }
        }

        private void SetState(AnimalState s)
        {
            State = s;
            stateTimer = 0f;
            var b = Bal;
            if (b == null || data == null) return;
            switch (s)
            {
                case AnimalState.Wander: ApplyStateSpeed(data.wanderSpeed); break;
                case AnimalState.Alert:
                    ApplyStateSpeed(0f);
                    // Bersuara saat masuk Alert (WildlifeAI.md tabel state, #114). Klip diputar sekali, panjangnya dari aset.
                    if (emitter != null) emitter.PlayAlertTell();
                    break;
                case AnimalState.Chase: chaseStartPoint = transform.position; unreachableTimer = 0f; ApplyStateSpeed(data.chaseSpeed); break;
                case AnimalState.ReturnToTerritory:
                    ApplyStateSpeed(data.wanderSpeed * b.returnSpeedMultiplier);
                    reaggroUntil = Time.time + b.reaggroCooldownSeconds;
                    CurrentTarget = null;
                    if (agent.isOnNavMesh) agent.SetDestination(homePoint);
                    break;
                case AnimalState.FleeToDen: ApplyStateSpeed(data.chaseSpeed * b.fleeSpeedMultiplier); break;
            }
        }

        /// <summary>Satu-satunya tempat agent.speed hewan ditulis: kecepatan state x semua pengali SpeedSource.</summary>
        private void ApplyStateSpeed(float baseSpeed)
        {
            currentBaseSpeed = baseSpeed;
            float m = 1f;
            for (int i = 0; i < speedMult.Length; i++) m *= speedMult[i];
            agent.speed = baseSpeed * m;
        }

        private ITargetable Target => CurrentTarget != null && CurrentTarget.IsTargetable ? CurrentTarget
                                    : (PlayerTargetable.Instance != null && PlayerTargetable.Instance.IsTargetable ? PlayerTargetable.Instance : null);

        private void TickWander(WildlifeBalance b)
        {
            if (CheckAggro(b)) return;
            if (agent.pathPending) return;
            if (!agent.hasPath || agent.remainingDistance < 0.5f)
            {
                if (Time.time < wanderWaitUntil) return;
                // Keliling dalam wanderRadius dari homePoint, tunggu 2 sampai 6 detik. Acak ini tidak memengaruhi save.
                Vector2 r = Random.insideUnitCircle * data.wanderRadius;
                if (NavMesh.SamplePosition(homePoint + new Vector3(r.x, 0f, r.y), out var hit, 4f, agent.areaMask)) agent.SetDestination(hit.position);
                wanderWaitUntil = Time.time + Random.Range(b.wanderWaitMinSeconds, b.wanderWaitMaxSeconds);
            }
        }

        private bool CheckAggro(WildlifeBalance b)
        {
            if (Time.time < reaggroUntil) return false;
            var t = Target;
            if (t == null) return false;
            float r = data.aggroRadius;
            var trav = IslandPlayerController.Instance != null ? IslandPlayerController.Instance.GetComponent<PlayerTraversal>() : null;
            if (t.Kind == TargetKind.Player && trav != null && trav.IsCrouching) r *= b.crouchAggroMultiplier;
            if ((t.Position - transform.position).sqrMagnitude > r * r) return false;
            if (!HasLineOfSight(t.AimPoint)) return false;
            CurrentTarget = t;
            SetState(AnimalState.Alert);
            return true;
        }

        private bool HasLineOfSight(Vector3 point)
        {
            Vector3 from = transform.position + Vector3.up;
            Vector3 dir = point - from;
            float len = dir.magnitude;
            return len < 0.01f || !PhysicsQuery.Raycast(new Ray(from, dir / len), out _, len, PhysicsMasks.AIVision);
        }

        private void TickAlert(WildlifeBalance b)
        {
            var t = Target;
            if (t == null) { SetState(AnimalState.Wander); return; }
            Vector3 to = t.Position - transform.position; to.y = 0f;
            if (to.sqrMagnitude > 0.001f) transform.rotation = Quaternion.LookRotation(to);
            if (agent.isOnNavMesh) agent.ResetPath();
            if (stateTimer < b.alertSeconds) return;
            if ((t.Position - transform.position).sqrMagnitude > data.aggroRadius * data.aggroRadius * 1.5f) { SetState(AnimalState.Wander); return; }
            SetState(AnimalState.Chase);
        }

        private void TickChase(WildlifeBalance b, float tick)
        {
            var t = Target;
            if (t == null) { SetState(AnimalState.ReturnToTerritory); return; }
            // 1. Batas teritori dari titik mulai mengejar.
            if (Vector3.Distance(transform.position, chaseStartPoint) > data.leashDistance) { SetState(AnimalState.ReturnToTerritory); return; }
            // 2.
            if (NavMesh.SamplePosition(t.Position, out var hit, b.targetSampleRadius, agent.areaMask)) agent.SetDestination(hit.position);
            // 3. Target tidak terjangkau (misalnya pemain di dalam base). Hewan tidak menyerang bangunan (#78).
            pathCheckTimer -= tick;
            if (pathCheckTimer <= 0f)
            {
                pathCheckTimer = b.pathCheckSeconds;
                bool complete = NavMesh.CalculatePath(transform.position, hit.position, agent.areaMask, path) && path.status == NavMeshPathStatus.PathComplete;
                unreachableTimer = complete ? 0f : unreachableTimer + b.pathCheckSeconds;
                if (unreachableTimer >= b.unreachableConfirmSeconds) { SetState(AnimalState.TargetUnreachable); return; }
            }
            // 4. Serangan khusus atau gigitan.
            float d = Vector3.Distance(transform.position, t.Position);
            if (data.attackKind == AnimalAttackKind.Pounce && Time.time >= specialCooldownUntil && d <= b.groundPounceDistance && HasLineOfSight(t.AimPoint))
            { BeginSpecial(); return; }
            if (data.attackKind == AnimalAttackKind.Charge && Time.time >= specialCooldownUntil && d <= b.chargeTriggerDistance && d > 3f)
            { BeginSpecial(); return; }
            var atk = data.primaryAttack;
            if (atk != null && d <= atk.range && attack != null && attack.TryStartAttack(t)) SetState(AnimalState.Attack);
        }

        private void BeginSpecial()
        {
            SetState(AnimalState.Attack);
            attackStep = 1;
            // Tanda serangan khusus lewat ThreatAudioEmitter (#114), misalnya geraman Jaguar sebelum menerkam (WildlifeAI.md).
            if (emitter != null) emitter.PlayAlertTell();
            if (agent.isOnNavMesh) agent.ResetPath();
        }

        private void TickAttack(WildlifeBalance b, float tick)
        {
            var t = Target;
            if (attackStep == 0)
            {
                // Gigitan lewat EnemyAttack (#109). Selesai: kembali Chase.
                if (attack == null || attack.Phase == AttackPhase.None) SetState(AnimalState.Chase);
                return;
            }
            if (data.attackKind == AnimalAttackKind.Pounce) TickPounce(b, t);
            else if (data.attackKind == AnimalAttackKind.Charge) TickCharge(b, t);
            else { attackStep = 0; SetState(AnimalState.Chase); }
        }

        // Terkaman Jaguar dari tanah: tanda, lompat, tahan pemain (GDD bagian 3, Diterkam).
        private void TickPounce(WildlifeBalance b, ITargetable t)
        {
            switch (attackStep)
            {
                case 1:
                    if (t == null) { EndSpecial(b.pounceCooldownSeconds); return; }
                    Vector3 to = t.Position - transform.position; to.y = 0f;
                    if (to.sqrMagnitude > 0.001f) transform.rotation = Quaternion.LookRotation(to);
                    if (stateTimer < b.pounceTelegraphSeconds) return;
                    chargeTarget = t.Position;
                    attackStep = 2;
                    stateTimer = 0f;
                    ApplyStateSpeed(Vector3.Distance(transform.position, chargeTarget) / Mathf.Max(0.05f, b.pounceSeconds));
                    agent.SetDestination(chargeTarget);
                    break;
                case 2:
                    if (t != null && Vector3.Distance(transform.position, t.Position) <= b.pounceHitRadius)
                    {
                        var atk = data.primaryAttack;
                        if (t.Damageable != null && atk != null)
                            t.Damageable.TakeDamage(MakeHit(atk.damage, t));
                        if (t.Kind == TargetKind.Player && PlayerPinnedState.Instance != null)
                        {
                            isPinning = true;
                            pounceDamageTaken = 0f;
                            pinDamageTimer = b.pinDamageInterval;
                            if (PlayerPinnedState.Instance.TryPin(this)) { attackStep = 4; if (agent.isOnNavMesh) agent.ResetPath(); return; }
                            isPinning = false;
                        }
                        EndSpecial(b.pounceCooldownSeconds);   // Ditolak: mundur, jeda ulang berjalan
                        return;
                    }
                    if (stateTimer >= b.pounceSeconds) { attackStep = 3; stateTimer = 0f; ApplyStateSpeed(0f); }   // Meleset
                    break;
                case 3:
                    if (stateTimer >= b.pounceMissStunSeconds) EndSpecial(b.pounceCooldownSeconds);   // Celah untuk pemain
                    break;
                case 4:
                    // Selama menahan: damage 5 setiap 0,5 detik.
                    pinDamageTimer -= b.tickSeconds;
                    if (pinDamageTimer <= 0f && t != null && t.Damageable != null)
                    {
                        pinDamageTimer = b.pinDamageInterval;
                        t.Damageable.TakeDamage(MakeHit(b.pinDamage, t));
                    }
                    break;
            }
        }

        // Serudukan Babi: tujuan terkunci di garis lurus, pemain bisa menghindar ke samping.
        private void TickCharge(WildlifeBalance b, ITargetable t)
        {
            switch (attackStep)
            {
                case 1:
                    if (stateTimer < b.chargeTelegraphSeconds) return;
                    if (t == null) { EndSpecial(3f); return; }
                    Vector3 dir = t.Position - transform.position; dir.y = 0f;
                    chargeTarget = transform.position + dir.normalized * b.chargeDistance;
                    if (NavMesh.SamplePosition(chargeTarget, out var hit, 3f, agent.areaMask)) chargeTarget = hit.position;
                    chargeHit = false;
                    attackStep = 2;
                    stateTimer = 0f;
                    ApplyStateSpeed(b.chargeSpeed);
                    agent.SetDestination(chargeTarget);
                    break;
                case 2:
                    if (!chargeHit)
                    {
                        int count = PhysicsQuery.OverlapSphere(transform.position + transform.forward + Vector3.up * 0.5f, 1f, PhysicsMasks.AIAttack, chargeBuffer);
                        for (int i = 0; i < count; i++)
                        {
                            var d = chargeBuffer[i].GetComponentInParent<IDamageable>();
                            if (d == null || !d.IsAlive || chargeBuffer[i].GetComponentInParent<PlayerTargetable>() == null) continue;
                            chargeHit = true;
                            var atk = data.primaryAttack;
                            if (atk != null) d.TakeDamage(MakeHit(atk.damage, t));
                            break;
                        }
                    }
                    // Menabrak struktur atau penghalang: pusing. Dinding tidak menerima damage.
                    bool blocked = PhysicsQuery.Raycast(new Ray(transform.position + Vector3.up * 0.5f, transform.forward), out _, 1f, PhysicsMasks.AIVision);
                    if (blocked) { attackStep = 3; stateTimer = 0f; ApplyStateSpeed(0f); return; }
                    if (!agent.pathPending && agent.remainingDistance < 0.5f || stateTimer > 3f) EndSpecial(4f);
                    break;
                case 3:
                    if (stateTimer >= b.chargeStunSeconds) EndSpecial(4f);
                    break;
            }
        }

        private DamageInfo MakeHit(float amount, ITargetable t)
        {
            var atk = data.primaryAttack;
            Vector3 p = t != null ? t.AimPoint : transform.position + transform.forward;
            return new DamageInfo
            {
                amount = amount, type = atk != null ? atk.damageType : DamageType.Melee, source = gameObject, sourceId = data.enemyId,
                hitPoint = p, hitDirection = (p - transform.position).normalized, bodyPart = BodyPart.Torso,
                infection = 0f, byPlayer = false   // Hewan tidak menularkan infeksi (#64)
            };
        }

        private void EndSpecial(float cooldown)
        {
            attackStep = 0;
            specialCooldownUntil = Time.time + cooldown;
            SetState(AnimalState.Chase);
        }

        /// <summary>Lepas dari terkaman (aksi pemain, waktu habis, atau damage dari pemain).</summary>
        public void OnPinReleased(PinReleaseReason reason)
        {
            isPinning = false;
            var b = Bal;
            if (attackStep == 4) EndSpecial(b != null ? b.pounceCooldownSeconds : 12f);
        }

        private void TickReturn(WildlifeBalance b, float tick)
        {
            // HP pulih saat kembali, supaya pemain tidak bisa mencicil HP dari batas teritori.
            if (health.IsAlive && health.CurrentHP < health.MaxHP) health.Heal(health.MaxHP * b.returnHealPerSec * tick);
            if (!agent.pathPending && agent.remainingDistance < 1.5f) SetState(AnimalState.Wander);
        }

        private void TickUnreachable(WildlifeBalance b, float tick)
        {
            var t = Target;
            if (t == null || stateTimer >= b.unreachableGiveUpSeconds) { SetState(AnimalState.ReturnToTerritory); return; }
            pathCheckTimer -= tick;
            if (pathCheckTimer > 0f) return;
            pathCheckTimer = b.pathCheckSeconds;
            if (NavMesh.SamplePosition(t.Position, out var hit, b.targetSampleRadius, agent.areaMask)
                && NavMesh.CalculatePath(transform.position, hit.position, agent.areaMask, path))
            {
                if (path.status == NavMeshPathStatus.PathComplete) { SetState(AnimalState.Chase); return; }
                // Mondar-mandir di ujung jalur parsial, supaya pemain tahu hewan masih ada.
                if (path.corners.Length > 0) agent.SetDestination(path.corners[path.corners.Length - 1]);
            }
        }

        private void TickFlee(WildlifeBalance b)
        {
            var den = DenPoint.FindNearest(transform.position);
            if (den != null)
            {
                agent.SetDestination(den.transform.position);
                if ((den.transform.position - transform.position).sqrMagnitude <= 4f) { Despawn(DespawnReason.ReturnedToDen); return; }
            }
            else if (!agent.hasPath && PlayerTargetable.Instance != null)
            {
                Vector3 away = transform.position - PlayerTargetable.Instance.Position; away.y = 0f;
                if (NavMesh.SamplePosition(transform.position + away.normalized * 20f, out var hit, 6f, agent.areaMask)) agent.SetDestination(hit.position);
            }
            if (stateTimer >= b.fleeTimeoutSeconds && !InView()) Despawn(DespawnReason.ReturnedToDen);
        }

        private bool InView()
        {
            var cam = FirstPersonCamera.Instance != null ? FirstPersonCamera.Instance.WorldCamera : null;
            if (cam == null) return false;
            Vector3 vp = cam.WorldToViewportPoint(transform.position);
            return vp.z > 0f && vp.x > 0f && vp.x < 1f && vp.y > 0f && vp.y < 1f;
        }

        private void Despawn(DespawnReason reason)
        {
            if (SpawnManager.Instance != null) SpawnManager.Instance.Despawn(gameObject, reason);
            else if (ObjectPooler.Instance != null) ObjectPooler.Instance.ReturnToPool(gameObject);
        }

        // ---------------------------------------------------------------- Event

        private void HandleHour(HourChangedEvent e)
        {
            if (dead || !DayPhaseTracker.IsNightHour(e.hour) || State == AnimalState.FleeToDen) return;
            EnterFlee();
        }

        /// <summary>GDD bagian 3: hewan kembali ke sarang saat malam.</summary>
        private void EnterFlee()
        {
            attackStep = 0;
            if (attack != null) attack.CancelAttack();
            if (isPinning && PlayerPinnedState.Instance != null) PlayerPinnedState.Instance.ReleaseByAttacker(this, PinReleaseReason.AttackerGone);
            isPinning = false;
            CurrentTarget = null;
            SetState(AnimalState.FleeToDen);
        }

        private void HandleNoise(NoiseEmittedEvent e)
        {
            var b = Bal;
            if (b == null || dead || data == null || State != AnimalState.Wander || Time.time < reaggroUntil) return;
            float d = Vector3.Distance(transform.position, e.position);
            // Menjaga resource (GDD bagian 3, disetujui developer): suara alat di dalam teritori langsung memicu Chase.
            if (e.sourceKind == NoiseSourceKind.Tool && (e.position - homePoint).sqrMagnitude <= data.wanderRadius * data.wanderRadius
                && d <= e.radius)
            {
                CurrentTarget = Target;
                if (CurrentTarget != null) SetState(AnimalState.Chase);
                return;
            }
            if (d < e.radius * b.noiseAggroMultiplier) { CurrentTarget = Target; if (CurrentTarget != null) SetState(AnimalState.Alert); }
        }

        private void HandleDamaged(DamageInfo info)
        {
            var b = Bal;
            if (b == null || dead || !info.byPlayer || State == AnimalState.FleeToDen) return;
            // Terkaman batal saat Jaguar menerima pounceBreakDamage dari pemain saja (keputusan developer).
            if (isPinning)
            {
                pounceDamageTaken += info.amount;
                if (pounceDamageTaken >= b.pounceBreakDamage && PlayerPinnedState.Instance != null)
                    PlayerPinnedState.Instance.ReleaseByAttacker(this, PinReleaseReason.AttackerDamaged);
                return;
            }
            // Anti-eksploit: dilukai saat TargetUnreachable langsung kembali dengan kecepatan kejar.
            if (State == AnimalState.TargetUnreachable)
            {
                SetState(AnimalState.ReturnToTerritory);
                ApplyStateSpeed(data.chaseSpeed);
                return;
            }
            CurrentTarget = PlayerTargetable.Instance;
            if (ThreatSystem.Instance != null && CurrentTarget != null) ThreatSystem.Instance.ReportAttacker(this, CurrentTarget);
            if (IsWithinTerritory(transform.position)) { if (State != AnimalState.Attack) SetState(AnimalState.Chase); }
            else SetState(AnimalState.Alert);
        }

        private void HandleDied(DamageInfo info)
        {
            dead = true;
            if (isPinning && PlayerPinnedState.Instance != null) PlayerPinnedState.Instance.ReleaseByAttacker(this, PinReleaseReason.AttackerGone);
            isPinning = false;
            if (fsm != null) { StopCoroutine(fsm); fsm = null; }
            // Mayat, bangkai, dan pengembalian ke pool milik EnemyHitReaction dan AnimalCarcass (#95, #61).
        }

        // ---------------------------------------------------------------- IHitReactive (#95)

        public bool RequestReaction(HitReactionKind kind)
        {
            if (kind == HitReactionKind.Dying) { dead = true; if (fsm != null) { StopCoroutine(fsm); fsm = null; } return true; }
            if (dead || isPinning) return false;
            reactionPaused = true;
            return true;
        }

        public void EndReaction()
        {
            reactionPaused = false;
            ApplyStateSpeed(currentBaseSpeed);
        }

        public void SetSpeedMultiplier(SpeedSource source, float multiplier)
        {
            if (source == SpeedSource.Trap && data != null && data.immuneToTrapSlow) return;
            speedMult[(int)source] = multiplier;
            ApplyStateSpeed(currentBaseSpeed);
        }

        // ---------------------------------------------------------------- Pool

        public void OnSpawned()
        {
            var bal = GameManager.CurrentBalance;
            dead = false;
            reactionPaused = false;
            isPinning = false;
            attackStep = 0;
            CurrentTarget = null;
            for (int i = 0; i < speedMult.Length; i++) speedMult[i] = 1f;
            threat.Reset(Time.time + (GetInstanceID() & 15) / 16f);
            homePoint = transform.position;
            reaggroUntil = 0f;
            specialCooldownUntil = 0f;
            if (!NavMesh.SamplePosition(transform.position, out var hit, bal != null ? bal.spawn.sampleRadius : 6f, NavMesh.AllAreas))
            {
                Despawn(DespawnReason.OffNavMesh);
                return;
            }
            agent.enabled = true;
            agent.Warp(hit.position);
            if (!agent.isOnNavMesh) { Despawn(DespawnReason.OffNavMesh); return; }
            agent.isStopped = false;
            agent.areaMask = data != null ? data.traversalMask : NavMesh.AllAreas;
            SetState(AnimalState.Wander);
            if (ThreatSystem.Instance != null) ThreatSystem.Instance.Register(this);
            if (fsm != null) StopCoroutine(fsm);
            fsm = StartCoroutine(FsmLoop());
            // Muncul saat malam: langsung ke sarang.
            if (DayPhaseTracker.IsNight) EnterFlee();
        }

        public void OnReturned()
        {
            if (fsm != null) { StopCoroutine(fsm); fsm = null; }
            if (isPinning && PlayerPinnedState.Instance != null) PlayerPinnedState.Instance.ReleaseByAttacker(this, PinReleaseReason.AttackerGone);
            isPinning = false;
            if (ThreatSystem.Instance != null) ThreatSystem.Instance.Unregister(this);
            if (agent.enabled) agent.enabled = false;
            groupId = 0;
            CurrentTarget = null;
        }
    }
}
