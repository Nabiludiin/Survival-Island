using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Pengali senjata atau alat (enchant). Blueprint: Island_Logic/LuckyMachine.md bagian 1 (#72), dibaca
    /// ToolRepairSystem/WeaponStatCalculator (#113, belum ditulis). Disimpan sebagai ItemInstance.luckyModifierId.
    /// Keputusan developer 13: modifier boleh dipasang ke alat; magazineBonus tidak berlaku untuk alat.
    /// </summary>
    [CreateAssetMenu(menuName = "Survival/Weapon Modifier")]
    public class WeaponModifierData : ScriptableObject
    {
        public string modifierId;
        public float damageMultiplier = 1f;
        public float speedMultiplier = 1f;
        public float durabilityMultiplier = 1f;
        public int magazineBonus;
        [Tooltip("inst.durability = -1 (#113).")]
        public bool indestructible;

        /// <summary>Nilai pembanding untuk aturan "modifier baru menggantikan yang lama kalau lebih besar".</summary>
        public float Strength => damageMultiplier * speedMultiplier;
    }
}
