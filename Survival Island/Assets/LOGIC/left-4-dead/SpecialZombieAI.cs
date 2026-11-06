using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Kelas dasar kemampuan varian zombie. Blueprint: left-4-dead/SpecialZombieAI.md (#25).
    /// EnemyAI.CrowdTick memanggil TickSpecial lebih dulu; true berarti gerak pada tick ini diambil alih.
    /// Zombie standar tidak memakainya. Runner/Tank/Spitter adalah pola pada varian GDD (usulan disetujui developer).
    /// Pengaman macet varian besar ada di sini (berlaku untuk turunan dengan data.isLarge).
    /// </summary>
    [RequireComponent(typeof(EnemyAI))]
    public abstract class SpecialZombieAI : MonoBehaviour, IPoolable
    {
        protected EnemyAI ai;
        protected EnemyHealth health;
        protected EnemyAttack attack;
        protected ZombieData zombie;

        private float slowTimer, unseenTimer;

        protected static SpecialZombieBalance Bal => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.specialZombie : null;

        protected virtual void Awake()
        {
            ai = GetComponent<EnemyAI>();
            health = GetComponent<EnemyHealth>();
            attack = GetComponent<EnemyAttack>();
            zombie = health != null ? health.Data as ZombieData : null;
        }

        /// <summary>true kalau gerak pada tick ini diambil alih.</summary>
        public abstract bool TickSpecial(float dt);

        protected Vector3 PlayerPos => PlayerTargetable.Instance != null ? PlayerTargetable.Instance.Position : transform.position;
        protected float DistanceToPlayer => PlayerTargetable.Instance != null ? Vector3.Distance(transform.position, PlayerTargetable.Instance.Position) : float.MaxValue;

        /// <summary>Garis pandang ke pemain lewat AIVision (tidak mengenai apa pun berarti terlihat).</summary>
        protected bool CanSeePlayer()
        {
            var p = PlayerTargetable.Instance;
            if (p == null) return false;
            Vector3 from = transform.position + Vector3.up * 1.6f;
            Vector3 dir = p.AimPoint - from;
            float len = dir.magnitude;
            return len < 0.01f || !PhysicsQuery.Raycast(new Ray(from, dir / len), out _, len, PhysicsMasks.AIVision);
        }

        /// <summary>
        /// Pengaman macet varian besar (SpecialZombieAI.md): minta target baru, lalu kembali ke pool tanpa drop.
        /// Tidak berjalan saat AttackBuilding. TODO #72: lewati di dalam kandang Lucky Machine.
        /// </summary>
        protected void TickLargeStuck(float dt)
        {
            var b = Bal;
            if (b == null || zombie == null || !zombie.isLarge || ai.CurrentState == AIState.AttackBuilding) return;
            bool seen = ai.Sensory != null ? ai.Sensory.CanSeePlayer : CanSeePlayer();
            unseenTimer = seen ? 0f : unseenTimer + dt;
            bool slow = ai.CurrentState == AIState.Chase && ai.Agent != null && ai.Agent.velocity.sqrMagnitude < 0.04f;
            slowTimer = slow ? slowTimer + dt : 0f;
            if (slowTimer >= b.largeStuckRetargetSeconds && unseenTimer >= b.largeStuckUnseenRetargetSeconds && ThreatSystem.Instance != null)
                ThreatSystem.Instance.RequestReevaluate(ai);
            bool stuck = (slowTimer >= b.largeStuckReturnSeconds && unseenTimer >= b.largeStuckUnseenReturnSeconds) || unseenTimer >= b.largeUnseenReturnSeconds;
            if (stuck && SpawnManager.Instance != null)
            {
                var inst = GetComponent<EnemyInstance>();
                // Ditunda selama terlihat di frustum kamera, supaya tidak hilang di depan pemain.
                var cam = FirstPersonCamera.Instance != null ? FirstPersonCamera.Instance.WorldCamera : null;
                bool inView = false;
                if (cam != null)
                {
                    Vector3 vp = cam.WorldToViewportPoint(transform.position);
                    inView = vp.z > 0f && vp.x > 0f && vp.x < 1f && vp.y > 0f && vp.y < 1f;
                }
                if (!inView && inst != null) SpawnManager.Instance.ReturnStuck(inst);
            }
        }

        public virtual void OnSpawned()
        {
            slowTimer = 0f;
            unseenTimer = 0f;
        }

        public virtual void OnReturned() { }
    }
}
