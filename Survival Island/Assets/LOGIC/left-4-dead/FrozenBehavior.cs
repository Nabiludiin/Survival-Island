using System.Collections;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Zombie Beku: meledak dingin saat mati. SpecialZombieAI.md (#25). GDD bagian 5, Region 3.
    /// Tidak meledak kalau mati karena api (es sudah mencair) atau BurnToAshes (tidak lewat Died).
    /// Catatan Langkah 6: pengurangan Warmth lewat WarmthSystem.ApplyColdBurst (#65) disambungkan di BurstRoutine langkah 4.
    /// </summary>
    public class FrozenBehavior : SpecialZombieAI
    {
        [SerializeField] private Renderer flashRenderer;

        private void OnEnable() { health.Died += HandleDied; }
        private void OnDisable() { health.Died -= HandleDied; }

        public override bool TickSpecial(float dt) => false;

        private void HandleDied(DamageInfo info)
        {
            if (info.type == DamageType.Fire || ai.IsBurning) return;
            StartCoroutine(BurstRoutine());
        }

        private IEnumerator BurstRoutine()
        {
            var b = Bal;
            if (b == null) yield break;
            // 2. Jeda dengan tubuh berkilat sebagai tanda. Rantai ledakan Beku tidak terjadi dalam satu frame.
            if (flashRenderer != null) flashRenderer.enabled = true;
            yield return new WaitForSeconds(b.frozenBurstDelaySeconds);
            Vector3 center = transform.position + Vector3.up;
            // 3. Explosion, byPlayer = false (tabel friendly fire #59).
            var info = new DamageInfo
            {
                amount = b.frozenBurstDamage, type = DamageType.Explosion, source = gameObject,
                sourceId = zombie != null ? zombie.enemyId : "zombie_beku", byPlayer = false
            };
            ExplosionDamage.Apply(center, b.frozenBurstRadius, info);
            // 4. Langkah 6 (#65): Warmth berkurang kalau pemain dalam radius dan garis pandang bebas.
            ApplyColdBurstToPlayer(center, b.frozenBurstRadius, b.frozenBurstWarmthLoss);
            // 5. NoiseSourceKind.Structure dipakai untuk ledakan (usulan blueprint).
            NoiseEmitter.Emit(center, b.frozenBurstRadius, NoiseSourceKind.Structure);
        }

        /// <summary>Langkah 6: pemain dalam radius dengan garis pandang bebas kehilangan Warmth (frozenBurstWarmthLoss).</summary>
        private static void ApplyColdBurstToPlayer(Vector3 center, float radius, float warmthLoss)
        {
            var warmth = WarmthSystem.Instance;
            if (warmth == null) return;
            Vector3 target = warmth.transform.position + Vector3.up;
            Vector3 toPlayer = target - center;
            float dist = toPlayer.magnitude;
            if (dist > radius) return;
            // Garis pandang: dinding atau medan di antara ledakan dan pemain menghalangi.
            if (dist > 0.01f && PhysicsQuery.Raycast(new Ray(center, toPlayer / dist), out var hit, dist, PhysicsMasks.Explosion)
                && hit.collider.GetComponentInParent<WarmthSystem>() == null) return;
            warmth.ApplyColdBurst(warmthLoss);
        }

        public override void OnReturned()
        {
            StopAllCoroutines();
            if (flashRenderer != null) flashRenderer.enabled = false;
        }
    }
}
