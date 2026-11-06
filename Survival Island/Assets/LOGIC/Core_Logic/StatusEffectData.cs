using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Data satu efek sementara atau permanen. Blueprint: Core_Logic/StatusEffectManager.md bagian 1 (#20).
    /// Efek baru tidak butuh kode baru. Contoh: Buff_Supplement (usulan disetujui developer), Debuff_* milik DiseaseManager (#45),
    /// dan buff Permanent hadiah Lucky Machine (GDD bagian 7, #72).
    /// </summary>
    [CreateAssetMenu(menuName = "Survival/Status Effect")]
    public class StatusEffectData : ScriptableObject
    {
        public string effectId;                  // Contoh: Buff_Supplement
        public string displayNameKey;            // Kunci teks untuk #104
        public Sprite icon;
        public bool isBuff;

        public EffectDurationUnit durationUnit = EffectDurationUnit.Seconds;
        [Tooltip("Mengikuti durationUnit: detik atau jam game. Diabaikan untuk Permanent.")]
        public float duration;
        public EffectStackRule stackRule = EffectStackRule.Refresh;
        public int maxStacks = 1;

        public float moveSpeedMultiplier = 1f;
        public float maxStaminaMultiplier = 1f;
        public float maxHpMultiplier = 1f;       // Dipakai Buff_LuckyHP (#72)
        public float staminaRegenMultiplier = 1f;
        public bool blocksHPRegen;

        [Tooltip("Damage kondisi per detik per tumpukan. 0 berarti tanpa damage.")]
        public float damagePerSecond;
        public DamageType damageType;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (maxStacks < 1) maxStacks = 1;
            if (string.IsNullOrEmpty(effectId)) Debug.LogWarning($"[StatusEffectData] {name}: effectId kosong.", this);
        }
#endif
    }
}
