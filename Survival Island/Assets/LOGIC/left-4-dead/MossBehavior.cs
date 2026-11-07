using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Zombie Lumut: menyamar Dormant, bangun saat amarah penuh (gaya Witch). SpecialZombieAI.md (#25). GDD bagian 5, Region 2.
    /// Usulan disetujui developer: tidak bisa kembali menyamar. Pemain jongkok tanpa senter tidak menaikkan amarah.
    /// Sorotan senter (FlashlightController.IsPointInBeam) dan tingkat cahaya (LightSourceSystem) menaikkan amarah (#98).
    /// Tanda samar (napas, mata) ada di aset prefab.
    /// </summary>
    public class MossBehavior : SpecialZombieAI
    {
        private float rage;
        private bool awake;
        private float wakeTimer;

        private void OnEnable()
        {
            health.Damaged += HandleDamaged;
            EventChannel<NoiseEmittedEvent>.Raised += HandleNoise;
        }

        private void OnDisable()
        {
            health.Damaged -= HandleDamaged;
            EventChannel<NoiseEmittedEvent>.Raised -= HandleNoise;
        }

        private void HandleDamaged(DamageInfo info) { if (!awake) rage = 1f; }

        private void HandleNoise(NoiseEmittedEvent e)
        {
            if (awake) return;
            if ((e.position - transform.position).sqrMagnitude <= e.radius * e.radius) rage = 1f;
        }

        public override bool TickSpecial(float dt)
        {
            var b = Bal;
            if (b == null) return false;
            if (awake)
            {
                // Animasi bangkit 0,6 detik, lalu Chase.
                if (wakeTimer > 0f) { wakeTimer -= dt; if (wakeTimer <= 0f) ai.WakeUp(PlayerTargetable.Instance != null ? PlayerTargetable.Instance.transform : null); return wakeTimer > 0f; }
                return false;
            }
            if (ai.CurrentState != AIState.Dormant) ai.EnterDormant();

            float d = DistanceToPlayer;
            bool threat = false;
            if (d <= b.mossWakeDistance) rage = 1f;
            else if (d <= b.mossAlertDistance)
            {
                var trav = IslandPlayerController.Instance != null ? IslandPlayerController.Instance.GetComponent<PlayerTraversal>() : null;
                bool crouching = trav != null && trav.IsCrouching;
                if (!crouching) { rage += b.mossRagePerSec * dt; threat = true; }
            }
            // SpecialZombieAI.md (#98): disorot senter dalam mossLightDistance dengan garis pandang bebas.
            if (!threat && d <= b.mossLightDistance && FlashlightController.IsPointInBeam(transform.position + Vector3.up) && CanSeePlayer())
            {
                rage += b.mossRagePerSec * dt;
                threat = true;
            }
            // Tingkat cahaya statis di posisi Lumut minimal light.mossStaticLightLevel.
            var lightBal = GameManager.CurrentBalance != null ? GameManager.CurrentBalance.light : null;
            if (!threat && lightBal != null && LightSourceSystem.Instance != null
                && LightSourceSystem.Instance.GetLightLevelAt(transform.position) >= lightBal.mossStaticLightLevel)
            {
                rage += b.mossRagePerSec * dt;
                threat = true;
            }
            if (!threat && rage < 1f) rage = Mathf.Max(0f, rage - b.mossRageDecayPerSec * dt);

            if (rage >= 1f)
            {
                awake = true;
                wakeTimer = 0.6f;
                var anim = GetComponentInChildren<Animator>();
                if (anim != null && anim.isActiveAndEnabled) anim.SetTrigger("Wake");
            }
            return true;
        }

        public override void OnSpawned()
        {
            base.OnSpawned();
            rage = 0f;
            awake = false;
            wakeTimer = 0f;
            // Muncul dari Dormant (#62). EnemyAI.OnSpawned sudah mengisi Idle lebih dulu.
            ai.EnterDormant();
        }
    }
}
