using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SurvivalIsland
{
    public enum AttackPhase { None, Windup, Active, Recovery }

    /// <summary>
    /// Satu ayunan serangan musuh: Windup, Active, Recovery. Blueprint: left-4-dead/EnemyAttack.md (#109).
    /// GDD bagian 3: serangan zombie menambah infeksi (DamageInfo.infection dari AttackData) dan HP bisa habis karena diserang.
    /// EnemyAI/WildlifeAI memutuskan kapan menyerang; komponen ini hanya aktif (enabled) selama ayunan berjalan.
    /// Pengali kesulitan TIDAK dipakai di sini, karena sudah diterapkan SurvivalStats (#59).
    /// </summary>
    public class EnemyAttack : MonoBehaviour, IPoolable
    {
        [SerializeField] private Animator animator;
        [SerializeField] private AudioSource telegraphSource;   // TODO #114: ganti ThreatAudioEmitter.PlayTell

        public AttackPhase Phase { get; private set; } = AttackPhase.None;
        public AttackData Current => current;

        private AttackData current;
        private float phaseTimer;
        private IDamageable targetDamageable;
        private ITargetable target;
        private bool isStructureAttack;
        private bool structureHitDone;
        private readonly List<IDamageable> swingHits = new List<IDamageable>(8);
        private readonly List<IDamageable> results = new List<IDamageable>(16);
        private readonly List<Collider> resultColliders = new List<Collider>(16);
        private static readonly Collider[] fallbackBuffer = new Collider[16];
        private static float lastTelegraphTime = -10f;
        private bool hitAny;
        private bool animEventFired;
        private float nextAttackTime;
        private float trackUntil;
        private float waveDamage = 1f, waveRecovery = 1f;
        private bool holdsToken;

        private NavMeshAgent agent;
        private EnemyHealth health;
        private EnemyAI ai;
        private int EnemyId => GetInstanceID();

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            health = GetComponent<EnemyHealth>();
            ai = GetComponent<EnemyAI>();
            if (animator == null) animator = GetComponentInChildren<Animator>();
            enabled = false;
        }

        private string SourceId => health != null && health.Data != null ? health.Data.enemyId : name;

        /// <summary>Serangan ke pemain atau NPC. attack null = primaryAttack dari data.</summary>
        public bool TryStartAttack(ITargetable t, AttackData attack = null)
        {
            if (Phase != AttackPhase.None || Time.time < nextAttackTime || t == null || !t.IsTargetable) return false;
            var a = attack != null ? attack : (health != null && health.Data != null ? health.Data.primaryAttack : null);
            if (a == null) return false;
            // 2. Jarak.
            if ((t.Position - transform.position).sqrMagnitude > a.range * a.range) return false;
            // 3. Izin AttackCoordinator.
            if (a.usesToken)
            {
                if (!AttackCoordinator.TryAcquireStatic(t, EnemyId)) return false;
                holdsToken = true;
            }
            // 4.
            targetDamageable = t.Damageable;
            if (targetDamageable == null) { ReleaseToken(); return false; }
            target = t;
            current = a;
            isStructureAttack = false;
            BeginWindup();
            return true;
        }

        /// <summary>Serangan ke dinding atau pintu, tanpa izin (jumlah penyerang dinding diatur BreachSlots #78).</summary>
        public bool TryStartStructureAttack(IDamageable wall)
        {
            if (Phase != AttackPhase.None || Time.time < nextAttackTime || wall == null || !wall.IsAlive) return false;
            var a = health != null && health.Data != null ? health.Data.primaryAttack : null;
            if (a == null) return false;
            targetDamageable = wall;
            target = null;
            current = a;
            isStructureAttack = true;
            structureHitDone = false;
            BeginWindup();
            return true;
        }

        private void BeginWindup()
        {
            // Pengali gelombang dibaca saat ayunan dimulai (AIDirector.md).
            waveDamage = ai != null ? ai.Wave.damage : 1f;
            waveRecovery = ai != null ? ai.Wave.recovery : 1f;
            Phase = AttackPhase.Windup;
            phaseTimer = current.windupSeconds;
            trackUntil = Time.time + current.windupSeconds * current.trackingRatio;
            swingHits.Clear();
            hitAny = false;
            animEventFired = false;
            enabled = true;
            if (agent != null && agent.enabled && agent.isOnNavMesh) agent.isStopped = true;
            if (animator != null && animator.isActiveAndEnabled && !string.IsNullOrEmpty(current.animTrigger)) animator.SetTrigger(current.animTrigger);
            Telegraph();
        }

        private void Telegraph()
        {
            if (telegraphSource != null && current.telegraphClip != null) telegraphSource.PlayOneShot(current.telegraphClip);
            if (target == null || target.Kind != TargetKind.Player) return;
            var bal = GameManager.CurrentBalance;
            float interval = bal != null ? bal.enemyAttack.telegraphMinInterval : 0.5f;
            if (Time.time - lastTelegraphTime < interval) return;
            lastTelegraphTime = Time.time;
            EventChannel<EnemyAttackTelegraphEvent>.Publish(new EnemyAttackTelegraphEvent(transform.position, current.attackId));
        }

        /// <summary>Animation Event dari klip serangan, diteruskan AnimationController (#12).</summary>
        public void AE_AttackHit()
        {
            if (Phase != AttackPhase.Windup || animEventFired) return;
            animEventFired = true;
            BeginActive();
        }

        private void Update()
        {
            if (Phase == AttackPhase.None) { enabled = false; return; }
            float dt = Time.deltaTime;
            if (dt <= 0f) return;
            phaseTimer -= dt;
            switch (Phase)
            {
                case AttackPhase.Windup:
                    // Arah mengikuti target sampai trackingRatio, lalu terkunci supaya pemain bisa menghindar ke samping.
                    if (Time.time < trackUntil) FaceTarget(dt);
                    if (target != null && !target.IsTargetable) { CancelAttack(); return; }
                    var bal = GameManager.CurrentBalance;
                    float grace = bal != null ? bal.enemyAttack.animEventGraceSeconds : 0.1f;
                    bool animActive = animator != null && animator.isActiveAndEnabled;
                    // Animator mati (OptimizationManager/#93): timer cadangan. Animator aktif: tunggu event + toleransi.
                    if (phaseTimer <= (animActive ? -grace : 0f) && !animEventFired) { animEventFired = true; BeginActive(); }
                    break;
                case AttackPhase.Active:
                    DoHitCheck();
                    if (phaseTimer <= 0f) BeginRecovery();
                    break;
                case AttackPhase.Recovery:
                    if (phaseTimer <= 0f) Finish(0f);
                    break;
            }
        }

        private void FaceTarget(float dt)
        {
            Vector3 p = target != null ? target.Position : (targetDamageable is Component c ? c.transform.position : transform.position);
            Vector3 dir = p - transform.position;
            dir.y = 0f;
            if (dir.sqrMagnitude < 0.0001f) return;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir), 540f * dt);
        }

        private void BeginActive()
        {
            Phase = AttackPhase.Active;
            phaseTimer = current.activeSeconds;
            DoHitCheck();
        }

        private void DoHitCheck()
        {
            if (current == null) return;
            if (isStructureAttack)
            {
                // Satu kali per ayunan, langsung ke sasaran bangunan. Structural, tanpa infeksi (BaseNavigation.md).
                if (structureHitDone || targetDamageable == null || !targetDamageable.IsAlive) return;
                structureHitDone = true;
                hitAny = true;
                var c = targetDamageable as Component;
                var info = new DamageInfo
                {
                    amount = current.structureDamage, type = DamageType.Structural, source = gameObject, sourceId = SourceId,
                    hitPoint = c != null ? c.transform.position : transform.position, hitDirection = transform.forward,
                    bodyPart = BodyPart.Torso, infection = 0f, byPlayer = false
                };
                targetDamageable.TakeDamage(info);
                return;
            }

            Vector3 center = transform.position + transform.forward * current.hitOffset + Vector3.up;
            var buffer = HordeCrowdManager.Instance != null ? HordeCrowdManager.Instance.AttackBuffer : fallbackBuffer;
            int count = PhysicsQuery.OverlapSphere(center, current.hitRadius, PhysicsMasks.AIAttack, buffer);
            PhysicsQuery.CollectUniqueDamageables(buffer, count, results, resultColliders);
            Vector3 chest = transform.position + Vector3.up * 1.2f;
            int structureLayer = LayerMask.NameToLayer(PhysicsMasks.StructureLayer);
            for (int i = 0; i < results.Count; i++)
            {
                var d = results[i];
                var col = resultColliders[i];
                if (d == null || !d.IsAlive || swingHits.Contains(d)) continue;
                if (col.gameObject.layer == structureLayer) continue;           // Bangunan lewat jalur struktur saja
                Vector3 to = col.bounds.center - transform.position;
                to.y = 0f;
                if (to.sqrMagnitude > 0.0001f && Vector3.Angle(transform.forward, to) > current.arcDegrees * 0.5f) continue;
                // Garis pandang: pukulan tidak menembus dinding.
                Vector3 aim = col.bounds.center;
                Vector3 los = aim - chest;
                float len = los.magnitude;
                if (len > 0.01f && PhysicsQuery.Raycast(new Ray(chest, los / len), out _, len, PhysicsMasks.AIVision)) continue;
                swingHits.Add(d);
                hitAny = true;
                var info = new DamageInfo
                {
                    amount = current.damage * waveDamage, type = current.damageType, source = gameObject, sourceId = SourceId,
                    hitPoint = col.ClosestPoint(center), hitDirection = to.sqrMagnitude > 0.0001f ? to.normalized : transform.forward,
                    bodyPart = BodyPart.Torso, infection = current.infection, byPlayer = false
                };
                d.TakeDamage(info);
            }
            results.Clear();
            resultColliders.Clear();
        }

        private void BeginRecovery()
        {
            Phase = AttackPhase.Recovery;
            phaseTimer = (hitAny ? current.recoveryAfterHit : current.recoveryAfterMiss) * waveRecovery;
        }

        private void Finish(float extraCooldown)
        {
            ReleaseToken();
            nextAttackTime = Time.time + extraCooldown;
            Phase = AttackPhase.None;
            current = null;
            target = null;
            targetDamageable = null;
            enabled = false;
            if (agent != null && agent.enabled && agent.isOnNavMesh) agent.isStopped = false;
        }

        private void ReleaseToken()
        {
            if (!holdsToken) return;
            holdsToken = false;
            AttackCoordinator.ReleaseStatic(EnemyId);
        }

        /// <summary>Dibatalkan stagger (#95). force dipakai ledakan.</summary>
        public void Interrupt(bool force)
        {
            if (Phase == AttackPhase.None || current == null) return;
            if (Phase != AttackPhase.Recovery && !current.canBeInterrupted && !force) return;
            var bal = GameManager.CurrentBalance;
            Finish(bal != null ? bal.enemyAttack.interruptCooldownSeconds : 0.6f);
        }

        /// <summary>Hentikan ayunan apa pun tanpa jeda tambahan (mati, BurnToAshes, target mati).</summary>
        public void CancelAttack()
        {
            if (Phase == AttackPhase.None) { ReleaseToken(); return; }
            Finish(0f);
        }

        public void OnSpawned()
        {
            Phase = AttackPhase.None;
            enabled = false;
            nextAttackTime = 0f;
            swingHits.Clear();
            holdsToken = false;
            current = null;
        }

        public void OnReturned()
        {
            AttackCoordinator.ReleaseStatic(EnemyId);
            holdsToken = false;
            Phase = AttackPhase.None;
            current = null;
            target = null;
            targetDamageable = null;
        }
    }
}
