using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Satu hadiah Lucky Machine. Blueprint: Island_Logic/LuckyMachine.md bagian 1 (#72).
    /// GDD bagian 7: hadiah berupa skin, buff permanen, enchant senjata atau alat, dan senjata ajaib.
    ///
    /// Penyimpangan: blueprint memakai StatusEffectData untuk buff, tetapi StatusEffectManager (#20) belum ditulis.
    /// Sementara field buff bertipe ScriptableObject; tipe diganti saat #20 ada.
    /// </summary>
    [CreateAssetMenu(menuName = "Survival/Lucky Reward")]
    public class LuckyRewardData : ScriptableObject
    {
        public string rewardId;
        public LuckyRewardKind kind;
        public LuckyTier tier;
        [Tooltip("Bobot dalam tier.")]
        public float weight = 1f;
        [Tooltip("Keluar dari undian setelah didapat dari mesin mana pun.")]
        public bool isUnique;
        [Tooltip("0 berarti tanpa batas.")]
        public int maxGrants;

        public WeaponModifierData modifier;
        public string skinId;
        public string skinTargetItemId;
        [Tooltip("TODO #20: StatusEffectData dengan durationUnit Permanent.")]
        public ScriptableObject buff;
        public ItemData item;
        public int amount = 1;
    }
}
