using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Data satu suku cadang helikopter. Blueprint: Island_Logic/HeavyItemSystem.md (#69).
    /// GDD bagian 1 dan 5: Baling-baling, Mesin, Tangki Bahan Bakar. Baling-baling Ekor dan Panel Kendali adalah usulan disetujui.
    /// </summary>
    [CreateAssetMenu(menuName = "Survival/Heavy Item")]
    public class HeavyItemData : ScriptableObject
    {
        public string partId = "Part_Rotor";
        public string displayNameKey;
        [Tooltip("Untuk UI dan suara jatuh pada versi awal. Tidak memengaruhi angka permainan.")]
        public float weightKg = 80f;
        [Tooltip("Prefab di layer dan tag HeavyItem, dengan HeavyItem, Rigidbody, dan Collider.")]
        public GameObject prefab;
        [Tooltip("0 berarti memakai region.heavyItemSpeedMultiplier (0,5).")]
        public float speedMultiplierOverride;
    }
}
