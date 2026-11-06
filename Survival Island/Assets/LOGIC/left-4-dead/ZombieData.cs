using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Perilaku varian zombie (GLOSSARY.md, SpecialZombieAI.md #25).</summary>
    public enum SpecialZombieKind { None, TribalSnare, MossDisguise, MilitaryArmor, FrozenBurst, VillagePounce }

    /// <summary>
    /// Data varian zombie GDD bagian 5. Blueprint: left-4-dead/SpecialZombieAI.md (#25).
    /// Statistik awal (usulan disetujui developer): standar 50 HP 4 m/s, tribal 80/4, lumut 120/5,5, militer 300/3,
    /// beku 60/3,5, desa 60/7,5. Diisi di aset, bukan di kode.
    /// </summary>
    [CreateAssetMenu(fileName = "New Zombie", menuName = "Survival/Zombie Data")]
    public class ZombieData : EnemyData
    {
        public SpecialZombieKind kind = SpecialZombieKind.None;
        public AttackData secondaryAttack;              // Boleh kosong
        public bool hidesFromSun;                       // #63, true hanya zombie_desa
        public float sightMultiplier = 1f;              // Dibaca AISensory (#49)
        public float hearingMultiplier = 1f;

        private void OnEnable()
        {
            // SpecialZombieAI.md: zombie boleh menyerang bangunan (#78).
            canAttackBuildings = true;
        }
    }
}
