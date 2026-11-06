using UnityEngine;
using UnityEngine.AI;

namespace SurvivalIsland
{
    /// <summary>
    /// Zombie Desa: terkaman cepat tanpa menahan pemain. SpecialZombieAI.md (#25). GDD bagian 5, Region 4: sangat cepat.
    /// Pola Runner (usulan disetujui developer). Kecepatan terkaman lewat SpeedSource.Pounce, tepat villagePounceSpeed.
    /// SeekHiding/Submerged/Emerging milik VillageZombieHiding (#63, belum ditulis).
    /// </summary>
    public class VillageBehavior : SpecialZombieAI
    {
        private enum PounceStep { None, Windup, Leap, Recover }

        private PounceStep step;
        private float stepTimer;
        private float cooldownUntil;
        private Vector3 leapTarget;
        private bool leapHit;

        public override bool TickSpecial(float dt)
        {
            var b = Bal;
            if (b == null || ai.CurrentState == AIState.Submerged || ai.CurrentState == AIState.SeekHiding || ai.CurrentState == AIState.Emerging) return false;
            var agent = ai.Agent;
            switch (step)
            {
                case PounceStep.None:
                    if (ai.CurrentState != AIState.Chase || Time.time < cooldownUntil || PlayerTargetable.Instance == null) return false;
                    float d = DistanceToPlayer;
                    if (d < b.villagePounceMin || d > b.villagePounceMax) return false;
                    // 1. Jalur lurus di NavMesh tidak terhalang, dan izin AttackCoordinator (#109).
                    if (NavMesh.Raycast(transform.position, PlayerPos, out _, agent.areaMask)) return false;
                    if (!AttackCoordinator.TryAcquireStatic(PlayerTargetable.Instance, GetInstanceID())) return false;
                    // 2. Windup: berhenti, memekik, menghadap pemain.
                    step = PounceStep.Windup;
                    stepTimer = b.villagePounceWindupSeconds;
                    if (agent.isOnNavMesh) agent.isStopped = true;
                    return true;
                case PounceStep.Windup:
                    stepTimer -= dt;
                    Vector3 to = PlayerPos - transform.position; to.y = 0f;
                    if (to.sqrMagnitude > 0.001f) transform.rotation = Quaternion.LookRotation(to);
                    if (stepTimer > 0f) return true;
                    // 3. Terkaman menuju titik pemain saat itu, paling lama 0,7 detik.
                    step = PounceStep.Leap;
                    stepTimer = 0.7f;
                    leapTarget = PlayerPos;
                    leapHit = false;
                    ai.ApplySpeedModifier(SpeedSource.Pounce, b.villagePounceSpeed / Mathf.Max(0.01f, ai.Data.moveSpeed), 0.7f);
                    if (agent.isOnNavMesh) { agent.isStopped = false; agent.SetDestination(leapTarget); }
                    return true;
                case PounceStep.Leap:
                    stepTimer -= dt;
                    // 4. Kena: primaryAttack sekali, tanpa menahan pemain (massal + pinned = mati tanpa peluang).
                    var atk = ai.Data.primaryAttack;
                    float range = atk != null ? atk.range : 2f;
                    if (!leapHit && PlayerTargetable.Instance != null && DistanceToPlayer <= range)
                    {
                        leapHit = true;
                        var target = PlayerTargetable.Instance.Damageable;
                        if (target != null && atk != null)
                            target.TakeDamage(new DamageInfo
                            {
                                amount = atk.damage * ai.Wave.damage, type = atk.damageType, source = gameObject, sourceId = ai.Data.enemyId,
                                hitPoint = PlayerPos, hitDirection = (PlayerPos - transform.position).normalized, bodyPart = BodyPart.Torso,
                                infection = atk.infection, byPlayer = false
                            });
                        stepTimer = 0f;
                    }
                    if (stepTimer > 0f) return true;
                    // 5. Pemulihan: terhuyung, celah untuk membalas.
                    ai.SetSpeedMultiplier(SpeedSource.Pounce, 1f);
                    AttackCoordinator.ReleaseStatic(GetInstanceID());
                    step = PounceStep.Recover;
                    stepTimer = b.villagePounceRecoverySeconds;
                    if (agent.isOnNavMesh) agent.isStopped = true;
                    return true;
                case PounceStep.Recover:
                    stepTimer -= dt;
                    if (stepTimer > 0f) return true;
                    step = PounceStep.None;
                    cooldownUntil = Time.time + b.villagePounceCooldownSeconds;
                    if (agent.isOnNavMesh) agent.isStopped = false;
                    return false;
            }
            return false;
        }

        public override void OnSpawned()
        {
            base.OnSpawned();
            step = PounceStep.None;
            cooldownUntil = 0f;
        }

        public override void OnReturned()
        {
            AttackCoordinator.ReleaseStatic(GetInstanceID());
            step = PounceStep.None;
        }
    }
}
