using UnityEngine;
using UnityEngine.AI;

namespace SurvivalIsland
{
    /// <summary>
    /// Pengisi Speed musuh dari NavMeshAgent dan penerjemah state AI ke Animator.
    /// Blueprint: Core_Logic/AnimationController.md bagian "EnemyAnimDriver" dan "State machine musuh" (#12).
    ///
    /// Dipasang di prefab zombie dan hewan (objek pool). Trigger serangan, Stagger, Knockback, Burn, Dormant, dan Emerge
    /// dikirim sistem pemiliknya (EnemyAttack #109, EnemyHitReaction #95, SunlightExposure #63), bukan komponen ini.
    /// Komponen ini hanya mengisi Speed dan bool Alert hewan.
    /// </summary>
    public class EnemyAnimDriver : MonoBehaviour, IPoolable
    {
        [Tooltip("Kosong: dicari di anak.")]
        [SerializeField] private AnimationController anim;

        private NavMeshAgent agent;
        private EnemyHealth health;
        private WildlifeAI wildlife;
        private float sampleTimer;
        private float sinceLastSample;
        private bool referenceDirty = true;
        private bool lastAlert;

        public AnimationController Anim => anim;

        private void Awake()
        {
            if (anim == null) anim = GetComponentInChildren<AnimationController>();
            agent = GetComponentInParent<NavMeshAgent>();
            health = GetComponentInParent<EnemyHealth>();
            wildlife = GetComponentInParent<WildlifeAI>();
        }

        public void OnSpawned()
        {
            // referenceSpeed diisi ulang dari data pada sampel pertama, karena urutan OnSpawned antar komponen tidak dijamin.
            referenceDirty = true;
            sampleTimer = 0f;
            sinceLastSample = 0f;
            lastAlert = false;
        }

        public void OnReturned() { }

        private void Update()
        {
            if (anim == null) return;
            var b = GameManager.CurrentBalance != null ? GameManager.CurrentBalance.animation : null;
            float dt = Time.deltaTime;
            sinceLastSample += dt;
            sampleTimer -= dt;
            if (sampleTimer > 0f) return;
            // Jeda enemySpeedSampleSeconds hemat untuk sekitar 50 zombie.
            sampleTimer = b != null ? b.enemySpeedSampleSeconds : 0.1f;

            if (referenceDirty) RefreshReferenceSpeed();
            float speed = agent != null && agent.enabled ? agent.velocity.magnitude / Mathf.Max(0.01f, anim.referenceSpeed) : 0f;
            anim.SetLocomotion(speed, sinceLastSample);
            sinceLastSample = 0f;

            if (wildlife != null)
            {
                // Hewan: bool Alert saat Alert atau TargetUnreachable.
                var s = wildlife.State;
                bool alert = s == AnimalState.Alert || s == AnimalState.TargetUnreachable;
                if (alert != lastAlert)
                {
                    lastAlert = alert;
                    anim.SetBool(AnimIds.Alert, alert);
                }
            }
        }

        private void RefreshReferenceSpeed()
        {
            var data = health != null ? health.Data : null;
            if (data == null) return;
            referenceDirty = false;
            // ZombieData.moveSpeed atau AnimalData.chaseSpeed.
            anim.referenceSpeed = data is AnimalData a ? a.chaseSpeed : data.moveSpeed;
        }
    }
}
