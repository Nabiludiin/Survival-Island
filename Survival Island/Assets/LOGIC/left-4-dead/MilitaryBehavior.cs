using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Zombie Militer: armor (di ZombieData.resistances) dan melambat saat ditembak dari dekat. SpecialZombieAI.md (#25).
    /// GDD bagian 5, Region 3: Zombie Militer berarmor anti-peluru. Pola Tank (usulan disetujui developer).
    /// agent.speed tidak ditulis langsung; lewat EnemyAI.ApplySpeedModifier(MilitarySlow).
    /// </summary>
    public class MilitaryBehavior : SpecialZombieAI
    {
        private void OnEnable() { health.Damaged += HandleDamaged; }
        private void OnDisable() { health.Damaged -= HandleDamaged; }

        private void HandleDamaged(DamageInfo info)
        {
            var b = Bal;
            if (b == null || info.type != DamageType.Bullet || !health.IsAlive) return;
            // Makin dekat penembak, makin lambat. Menembak dari dekat lebih efektif, tetapi lebih berbahaya.
            float t = Mathf.InverseLerp(b.militarySlowEndDistance, b.militarySlowFullDistance, DistanceToPlayer);
            ai.ApplySpeedModifier(SpeedSource.MilitarySlow, Mathf.Lerp(1f, b.militarySlowMultiplier, t), b.militarySlowSeconds);
        }

        public override bool TickSpecial(float dt)
        {
            TickLargeStuck(dt);
            return false;
        }
    }
}
