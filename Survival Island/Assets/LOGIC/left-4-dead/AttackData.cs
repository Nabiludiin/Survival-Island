using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Satu jenis serangan jarak dekat musuh. Blueprint: left-4-dead/EnemyAttack.md bagian "AttackData" (#109).
    /// GDD bagian 3: serangan zombie menambah infeksi. Nilai infection diisi dari tabel #64
    /// (Standard 5, Tribal 6, Moss 8, Military 10, Frozen 5, Village 6, hewan 0) di aset.
    /// </summary>
    [CreateAssetMenu(fileName = "New Attack", menuName = "Survival/Attack Data")]
    public class AttackData : ScriptableObject
    {
        public string attackId = "Zombie_Standard_Hit";
        public float damage = 15f;
        public float structureDamage = 10f;
        public float infection = 5f;
        public DamageType damageType = DamageType.Melee;   // Ular memakai Poison (#61)
        public float range = 2f;
        public float hitRadius = 0.9f;
        public float hitOffset = 1f;
        public float arcDegrees = 120f;
        public float windupSeconds = 0.5f;
        [Range(0f, 1f)] public float trackingRatio = 0.6f;
        public float activeSeconds = 0.15f;
        public float recoveryAfterHit = 1.2f;
        public float recoveryAfterMiss = 0.8f;
        public bool canBeInterrupted = true;
        public bool usesToken = true;
        public string animTrigger = "Attack";
        public AudioClip telegraphClip;
    }
}
