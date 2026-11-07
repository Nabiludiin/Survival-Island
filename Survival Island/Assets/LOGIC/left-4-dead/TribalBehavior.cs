using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Zombie Tribal: melempar jerat yang menarik dan menahan pemain. SpecialZombieAI.md (#25), pola Spitter.
    /// GDD bagian 3 (Diterkam): Zombie Tribal bisa menahan pemain, dan pemain harus lepas sendiri.
    /// Cara lepas: damage dari pemain >= tribalReleaseDamage (40, keputusan developer), aksi #110 (termasuk Pisau),
    /// Tribal mati, jarak > tribalBreakDistance, atau garis pandang terputus lebih dari 1 detik.
    /// Jerat diambil dari ObjectPooler dengan tag "Tribal_Snare".
    /// Tarikan pemain memakai IslandPlayerController.AddExternalVelocity (#101, keputusan developer 7 November 2026),
    /// menggantikan Teleport bertahap dari Langkah 5. Suara windup diulang tiap tribalWindupCueRepeatSeconds.
    /// </summary>
    public class TribalBehavior : SpecialZombieAI, IPinAttacker
    {
        public const string SnarePoolTag = "Tribal_Snare";

        [SerializeField] private Transform throwPoint;
        [SerializeField] private Transform headPoint;

        private enum SnareStep { Ready, Windup, Flying, Holding }

        private SnareStep step;
        private float stepTimer;
        private float cooldownUntil;
        private float damageSincePin;
        private float blockedTimer;
        private float holdDamageTimer;
        private float windupCueTimer;
        private TribalSnare activeSnare;
        private ThreatAudioEmitter emitter;   // Suara peringatan windup (#114). Boleh tidak ada.

        protected override void Awake()
        {
            base.Awake();
            emitter = GetComponent<ThreatAudioEmitter>();
        }

        public string AttackerId => zombie != null ? zombie.enemyId : "zombie_tribal";
        public Transform LookTarget => headPoint != null ? headPoint : transform;
        public bool IsPinValid => isActiveAndEnabled && health.IsAlive && !ai.IsBurning;
        public IDamageable DamageTarget => health;

        private void OnEnable() { health.Damaged += HandleDamaged; health.Died += HandleDied; }
        private void OnDisable() { health.Damaged -= HandleDamaged; health.Died -= HandleDied; }

        private void HandleDamaged(DamageInfo info)
        {
            if (step != SnareStep.Holding || !info.byPlayer) return;   // Hanya damage pemain (keputusan developer)
            damageSincePin += info.amount;
            var b = Bal;
            if (b != null && damageSincePin >= b.tribalReleaseDamage) ReleasePin(PinReleaseReason.AttackerDamaged);
        }

        private void HandleDied(DamageInfo info)
        {
            if (step == SnareStep.Holding) ReleasePin(PinReleaseReason.AttackerGone);
            ReturnSnare();
        }

        public override bool TickSpecial(float dt)
        {
            var b = Bal;
            var player = PlayerTargetable.Instance;
            if (b == null || player == null) return false;
            var agent = ai.Agent;
            switch (step)
            {
                case SnareStep.Ready:
                    if (ai.CurrentState != AIState.Chase || Time.time < cooldownUntil) return false;
                    float d = DistanceToPlayer;
                    bool pinned = EventChannel<PlayerPinnedEvent>.HasLast && EventChannel<PlayerPinnedEvent>.Last.isPinned;
                    if (d < b.tribalRangeMin || d > b.tribalRangeMax || pinned || !CanSeePlayer()) return false;
                    // 2. Windup: berhenti, mengayun, dan suara peringatan lewat ThreatAudioEmitter (#114).
                    step = SnareStep.Windup;
                    if (emitter != null) emitter.PlayAlertTell();
                    windupCueTimer = b.tribalWindupCueRepeatSeconds;
                    stepTimer = b.tribalWindupSeconds;
                    if (agent.isOnNavMesh) agent.isStopped = true;
                    return true;
                case SnareStep.Windup:
                    stepTimer -= dt;
                    // Keputusan developer (7 November 2026): suara windup diulang selama windup berlangsung.
                    windupCueTimer -= dt;
                    if (windupCueTimer <= 0f && b.tribalWindupCueRepeatSeconds > 0f)
                    {
                        if (emitter != null) emitter.PlayAlertTell();
                        windupCueTimer = b.tribalWindupCueRepeatSeconds;
                    }
                    if (stepTimer > 0f) return true;
                    // 3. Jerat mengarah ke posisi pemain saat windup berakhir, jadi bisa dihindari ke samping.
                    Vector3 from = throwPoint != null ? throwPoint.position : transform.position + Vector3.up * 1.5f;
                    Vector3 aim = player.AimPoint;
                    var pooler = ObjectPooler.Instance;
                    activeSnare = pooler != null ? pooler.SpawnFromPool<TribalSnare>(SnarePoolTag, from, Quaternion.LookRotation(aim - from)) : null;
                    if (activeSnare == null) { EndCooldown(b); return false; }
                    activeSnare.Launch(this, (aim - from).normalized, b.tribalProjectileSpeed, b.tribalRangeMax * 1.5f);
                    step = SnareStep.Flying;
                    return true;
                case SnareStep.Flying:
                    if (activeSnare == null) { EndCooldown(b); return false; }
                    return true;
                case SnareStep.Holding:
                    TickHolding(b, dt);
                    return true;
            }
            return false;
        }

        /// <summary>Dipanggil TribalSnare saat mengenai pemain atau meleset.</summary>
        internal void OnSnareResult(bool hitPlayer)
        {
            activeSnare = null;
            var b = Bal;
            if (b == null) return;
            if (!hitPlayer || PlayerPinnedState.Instance == null || !PlayerPinnedState.Instance.TryPin(this))
            {
                // Ditolak: jeda tanpa efek.
                EndCooldown(b);
                return;
            }
            step = SnareStep.Holding;
            damageSincePin = 0f;
            blockedTimer = 0f;
            holdDamageTimer = 1f;
        }

        private void TickHolding(SpecialZombieBalance b, float dt)
        {
            var player = PlayerTargetable.Instance;
            if (player == null) { ReleasePin(PinReleaseReason.AttackerGone); return; }
            float d = DistanceToPlayer;
            if (d > b.tribalBreakDistance) { ReleasePin(PinReleaseReason.AttackerGone); return; }
            blockedTimer = CanSeePlayer() ? 0f : blockedTimer + dt;
            if (blockedTimer > 1f) { ReleasePin(PinReleaseReason.AttackerGone); return; }

            // 5. Tarik pemain dan beri damage + infeksi dari secondaryAttack.
            // #101 (keputusan developer 7 November 2026): tarikan lewat AddExternalVelocity. Dipanggil tiap tick, jadi
            // dorongan baru menggantikan yang lama; kecepatan dibatasi supaya tidak melewati jarak 1,5 m.
            var pc = IslandPlayerController.Instance;
            if (pc != null)
            {
                Vector3 dir = transform.position - player.Position; dir.y = 0f;
                if (d > 1.5f && dir.sqrMagnitude > 0.0001f)
                {
                    float speed = Mathf.Min(b.tribalPullSpeed, (d - 1.5f) / Mathf.Max(dt, 0.0001f));
                    // Durasi minimal dua tick, supaya tarikan tidak putus kalau CrowdTick jarang (agen jauh).
                    pc.AddExternalVelocity(dir.normalized * speed, Mathf.Max(b.tribalPullFadeSeconds, dt * 2f));
                }
                else pc.ClearExternalVelocity();
            }
            holdDamageTimer -= dt;
            if (holdDamageTimer <= 0f && player.Damageable != null)
            {
                holdDamageTimer = 1f;
                var atk = zombie != null ? zombie.secondaryAttack : null;
                player.Damageable.TakeDamage(new DamageInfo
                {
                    amount = b.tribalDamagePerSec, type = DamageType.Melee, source = gameObject, sourceId = AttackerId,
                    hitPoint = player.AimPoint, hitDirection = (player.Position - transform.position).normalized,
                    bodyPart = BodyPart.Torso, infection = atk != null ? atk.infection : 0f, byPlayer = false
                });
            }
        }

        private void ReleasePin(PinReleaseReason reason)
        {
            if (PlayerPinnedState.Instance != null) PlayerPinnedState.Instance.ReleaseByAttacker(this, reason);
            if (step == SnareStep.Holding) EndCooldown(Bal);   // OnPinReleased juga bisa memanggil ini
        }

        /// <summary>Lepas lewat aksi pemain (#110): Tribal masuk jeda.</summary>
        public void OnPinReleased(PinReleaseReason reason)
        {
            EndCooldown(Bal);
        }

        private void EndCooldown(SpecialZombieBalance b)
        {
            if (step == SnareStep.Holding) StopPull();
            step = SnareStep.Ready;
            cooldownUntil = Time.time + (b != null ? b.tribalCooldownSeconds : 10f);
            if (ai.Agent != null && ai.Agent.enabled && ai.Agent.isOnNavMesh) ai.Agent.isStopped = false;
        }

        /// <summary>Tarikan berhenti saat pemain lepas, supaya sisa dorongan tidak terus menyeret pemain.</summary>
        private static void StopPull()
        {
            if (IslandPlayerController.Instance != null) IslandPlayerController.Instance.ClearExternalVelocity();
        }

        private void ReturnSnare()
        {
            if (activeSnare != null) { activeSnare.Detach(); activeSnare = null; }
        }

        public override void OnSpawned()
        {
            base.OnSpawned();
            step = SnareStep.Ready;
            cooldownUntil = 0f;
            damageSincePin = 0f;
            activeSnare = null;
        }

        public override void OnReturned()
        {
            if (PlayerPinnedState.Instance != null) PlayerPinnedState.Instance.ReleaseByAttacker(this, PinReleaseReason.AttackerGone);
            ReturnSnare();
            if (step == SnareStep.Holding) StopPull();
            step = SnareStep.Ready;
        }
    }
}
