using UnityEngine;
using UnityEngine.AI;

namespace SurvivalIsland
{
    /// <summary>
    /// Data dasar musuh. ZombieData (#25) dan AnimalData (#61) mewarisinya.
    /// Blueprint: left-4-dead/SpecialZombieAI.md bagian "Data", ditambah field #59, #95, #108, #109.
    /// enemyId sekaligus tag pool (SpawnManager.md, ObjectPooler.md).
    /// </summary>
    public abstract class EnemyData : ScriptableObject
    {
        [Header("Identitas")]
        public string enemyId;                          // zombie_standar, hewan_serigala, ...
        public float maxHP = 50f;
        public float moveSpeed = 4f;

        [Header("Damage (#59, #109)")]
        public DamageResistance[] resistances = new DamageResistance[0];
        public AttackData primaryAttack;
        public ThreatProfile threatProfile;             // Threat_Zombie atau Threat_Animal (#108)

        [Header("Perilaku")]
        public bool canAttackBuildings;                 // true untuk ZombieData, false untuk AnimalData (#78)
        public bool isLarge;                            // Pengaman macet dan batas AIDirector
        public bool immuneToTrapSlow;                   // #26
        [Tooltip("Area NavMesh yang boleh dilewati (#94). -1 = semua area.")]
        public int traversalMask = NavMesh.AllAreas;

        [Header("Reaksi kena (#95)")]
        public float poise = 25f;
        public bool canBeStaggered = true;
        public bool canBeKnockedBack = true;
        public float knockbackMultiplier = 1f;
        public bool leavesCarcass;
        [Tooltip("Tabel drop saat mati (#103). Kosong = tanpa drop.")]
        public LootTable dropTable;
        [Range(0f, 1f)] public float dropChance;

        /// <summary>Profil ancaman, atau null kalau belum diisi (ThreatSystem memakai nilai bawaan).</summary>
        public ThreatProfile Threat => threatProfile;
    }
}
