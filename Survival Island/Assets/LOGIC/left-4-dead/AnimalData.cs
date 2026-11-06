using System;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Bentuk serangan hewan (WildlifeAI.md, #61).</summary>
    public enum AnimalAttackKind : byte { Bite, Pounce, VenomBite, Charge, FlockDive }

    /// <summary>Satu hasil buruan saat dikuliti (#61).</summary>
    [Serializable]
    public struct HarvestEntry
    {
        public ItemData item;
        public int min, max;
    }

    /// <summary>
    /// Data hewan buas siang. Blueprint: left-4-dead/WildlifeAI.md (#61). GDD bagian 3: hewan menjaga teritori,
    /// bagian 5: hewan per region. Angka per spesies diisi di aset (tabel "Data per spesies").
    /// enemyId, maxHP, dan resistances berasal dari EnemyData. Serangan memakai primaryAttack (AttackData, #109).
    /// </summary>
    [CreateAssetMenu(fileName = "New Animal", menuName = "Survival/Animal Data")]
    public class AnimalData : EnemyData
    {
        public float wanderSpeed = 2f;
        public float chaseSpeed = 7f;
        public float aggroRadius = 18f;
        public float leashDistance = 35f;
        public float wanderRadius = 15f;
        public AnimalAttackKind attackKind = AnimalAttackKind.Bite;
        public HarvestEntry[] harvest = new HarvestEntry[0];

        private void OnEnable()
        {
            // WildlifeAI.md: hewan tidak pernah menyerang bangunan (#78).
            canAttackBuildings = false;
        }
    }
}
