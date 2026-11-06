using System.Collections;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Zombie Beku: meledak dingin saat mati. SpecialZombieAI.md (#25). GDD bagian 5, Region 3.
    /// Tidak meledak kalau mati karena api (es sudah mencair) atau BurnToAshes (tidak lewat Died).
    /// Catatan Langkah 5: WarmthSystem.ApplyColdBurst (#65, Langkah 6) belum ada, jadi pengurangan Warmth ditandai TODO.
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
            // 4. TODO #65: WarmthSystem.Instance.ApplyColdBurst(frozenBurstWarmthLoss) kalau pemain kena dan garis pandang bebas.
            // 5. NoiseSourceKind.Structure dipakai untuk ledakan (usulan blueprint).
            NoiseEmitter.Emit(center, b.frozenBurstRadius, NoiseSourceKind.Structure);
        }

        public override void OnReturned()
        {
            StopAllCoroutines();
            if (flashRenderer != null) flashRenderer.enabled = false;
        }
    }
}
