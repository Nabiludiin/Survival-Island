using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Aksi dorong pemain Fase 1 (klik kiri). Blueprint: 60-seconds/ShipHazardManager.md bagian "Cara menghadapi zombie" (#75).
    /// Usulan disetujui (#85): Pistol dan Pisau tidak dipakai di Fase 1. Dorongan sejauh shoveKnockback, terhuyung
    /// shoveStaggerSeconds, tanpa damage, tanpa stamina, jeda shoveCooldownSeconds.
    ///
    /// Target dikumpulkan lewat PhysicsQuery.CollectUniqueDamageables (#59), lalu EnemyHitReaction.ApplyKnockback (#95)
    /// dengan angka shipHazard. Pola sama dengan PlayerShove di pulau, tanpa stamina dan tanpa kerucut arah.
    /// </summary>
    public class ShipShove : MonoBehaviour
    {
        private static readonly ShipHazardBalance FallbackHazard = new ShipHazardBalance();

        [Tooltip("Animator tangan. Boleh kosong.")]
        [SerializeField] private Animator handAnimator;
        [SerializeField] private string shoveTrigger = "Shove";

        private readonly Collider[] buffer = new Collider[16];
        private readonly List<IDamageable> unique = new List<IDamageable>(8);
        private readonly List<Collider> uniqueColliders = new List<Collider>(8);
        private float nextShoveTime;

        /// <summary>Jumlah collider terakhir yang terkena. Untuk uji di Inspector.</summary>
        public int LastHitCount { get; private set; }

        private static ShipHazardBalance Hazard
        {
            get
            {
                var b = GameManager.CurrentBalance;
                return b != null ? b.shipHazard : FallbackHazard;
            }
        }

        private void OnEnable()
        {
            nextShoveTime = 0f;
        }

        /// <summary>Dipanggil ShipPlayerController saat PrimaryPressed pada level Full.</summary>
        public void TryShove()
        {
            // 1. Tolak kalau masih jeda. Memakai Time.time, jadi jeda ikut berhenti saat Pause.
            if (Time.time < nextShoveTime) return;
            var hz = Hazard;
            nextShoveTime = Time.time + hz.shoveCooldownSeconds;

            // 2. Animasi tangan.
            if (handAnimator != null && !string.IsNullOrEmpty(shoveTrigger)) handAnimator.SetTrigger(shoveTrigger);

            // 3. Cari target di depan pemain.
            Vector3 center = transform.position + transform.forward * hz.shoveForwardOffset;
            LastHitCount = PhysicsQuery.OverlapSphere(center, hz.shoveRadius, PhysicsMasks.Melee, buffer);

            // 4. Satu target per musuh walau beberapa collider-nya kena (#59).
            PhysicsQuery.CollectUniqueDamageables(buffer, LastHitCount, unique, uniqueColliders);
            // 5. Dorong menjauh dari pemain dan terhuyung, tanpa damage (#95).
            for (int i = 0; i < uniqueColliders.Count; i++)
            {
                var reaction = uniqueColliders[i] != null ? uniqueColliders[i].GetComponentInParent<EnemyHitReaction>() : null;
                if (reaction == null) continue;
                Vector3 away = reaction.transform.position - transform.position;
                away.y = 0f;
                if (away.sqrMagnitude < 0.0001f) away = transform.forward;
                reaction.ApplyKnockback(away.normalized, hz.shoveKnockback, hz.shoveStaggerSeconds);
            }
            unique.Clear();
            uniqueColliders.Clear();
            for (int i = 0; i < LastHitCount; i++) buffer[i] = null;
        }
    }
}
