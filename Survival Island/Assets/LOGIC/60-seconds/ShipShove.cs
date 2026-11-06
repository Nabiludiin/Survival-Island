using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Aksi dorong pemain Fase 1 (klik kiri). Blueprint: 60-seconds/ShipHazardManager.md bagian "Cara menghadapi zombie" (#75).
    /// Usulan disetujui (#85): Pistol dan Pisau tidak dipakai di Fase 1. Dorongan sejauh shoveKnockback, terhuyung
    /// shoveStaggerSeconds, tanpa damage, tanpa stamina, jeda shoveCooldownSeconds.
    ///
    /// Belum diimplementasikan: PhysicsQuery.CollectUniqueDamageables dan EnemyHitReaction.ApplyKnockback milik
    /// Langkah 5 (#59, #95). Sampai itu ada, dorongan hanya memeriksa jeda dan mencari collider musuh.
    /// </summary>
    public class ShipShove : MonoBehaviour
    {
        private static readonly ShipHazardBalance FallbackHazard = new ShipHazardBalance();

        [Tooltip("Animator tangan. Boleh kosong.")]
        [SerializeField] private Animator handAnimator;
        [SerializeField] private string shoveTrigger = "Shove";

        private readonly Collider[] buffer = new Collider[16];
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

            // 4-5. TODO Langkah 5 (#59, #95):
            //   PhysicsQuery.CollectUniqueDamageables(buffer, LastHitCount, results);
            //   untuk tiap IDamageable yang punya EnemyHitReaction:
            //     ApplyKnockback(arah dari pemain, hz.shoveKnockback, hz.shoveStaggerSeconds).
            for (int i = 0; i < LastHitCount; i++) buffer[i] = null;
        }
    }
}
