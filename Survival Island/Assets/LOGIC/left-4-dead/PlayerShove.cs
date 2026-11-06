using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Dorongan pemain di IslandPhase (tombol Shove, nilai awal V). Blueprint: left-4-dead/EnemyHitReaction.md bagian
    /// "Shove pemain" (#95). Usulan disetujui developer. Tanpa damage, tidak membatalkan reload.
    /// Di kapal, ShipShove (#75) memakai angkanya sendiri.
    /// </summary>
    public class PlayerShove : MonoBehaviour
    {
        private float nextShoveTime;
        private static readonly Collider[] buffer = new Collider[16];
        private readonly List<IDamageable> unique = new List<IDamageable>(8);
        private readonly List<Collider> uniqueColliders = new List<Collider>(8);

        private void Update()
        {
            var input = InputManager.Instance;
            if (input == null || Time.deltaTime <= 0f || !input.ShovePressed) return;
            TryShove();
        }

        private void TryShove()
        {
            var bal = GameManager.CurrentBalance;
            if (bal == null) return;
            var b = bal.hitReaction;
            // 1.
            if (Time.time < nextShoveTime) return;
            if (EquipmentManager.Instance != null && EquipmentManager.Instance.Locks != EquipLock.None) return;
            // 2. Stamina tidak cukup: notifikasi lelah (#22).
            var stats = SurvivalStats.Instance;
            if (stats != null && !stats.SpendStamina(b.shoveStaminaCost, StaminaUse.Melee))
            {
                if (NotificationManager.Instance != null) NotificationManager.Instance.Show("notif.no_stamina", NotificationKind.Warning);   // "Terlalu lelah"
                return;
            }
            nextShoveTime = Time.time + b.shoveCooldownSeconds;
            // 3. Animasi dorong tangan (#12, trigger Shove disetujui developer).
            var anim = PlayerAnimDriver.Instance;
            if (anim != null && anim.Arms != null) anim.Arms.Play(AnimIds.Shove);
            // 4.
            var cam = FirstPersonCamera.Instance != null ? FirstPersonCamera.Instance.WorldCamera : null;
            Vector3 forward = cam != null ? cam.transform.forward : transform.forward;
            forward.y = 0f;
            forward.Normalize();
            int count = PhysicsQuery.OverlapSphere(transform.position + Vector3.up + forward, b.shoveRadius, PhysicsMasks.Melee, buffer);
            PhysicsQuery.CollectUniqueDamageables(buffer, count, unique, uniqueColliders);
            // 5.
            for (int i = 0; i < unique.Count; i++)
            {
                var c = uniqueColliders[i];
                var reaction = c.GetComponentInParent<EnemyHitReaction>();
                if (reaction == null) continue;
                Vector3 to = reaction.transform.position - transform.position;
                to.y = 0f;
                if (to.sqrMagnitude < 0.0001f || Vector3.Dot(forward, to.normalized) < b.shoveConeDot) continue;
                var health = reaction.GetComponent<EnemyHealth>();
                if (health != null && health.Data != null && health.Data.canBeKnockedBack)
                    reaction.ApplyKnockback(to.normalized, b.shoveKnockback, b.shoveStaggerSeconds);
                // Musuh besar: bunyi dorongan gagal tanpa reaksi (TODO #114 suara).
            }
            unique.Clear();
            uniqueColliders.Clear();
        }
    }
}
