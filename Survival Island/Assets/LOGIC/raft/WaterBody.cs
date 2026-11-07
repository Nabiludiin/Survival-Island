using UnityEngine;

namespace SurvivalIsland
{
    public enum WaterKind { Sea, River, Lake }

    /// <summary>
    /// Penanda badan air pada setiap collider di layer Water (trigger). Dibaca FishingSystem saat menentukan titik jatuh kail.
    /// Blueprint: raft/FishingSystem.md bagian 1 (#55).
    /// Usulan disetujui #55: memancing boleh di semua WaterBody; tebing karang memakai collider Water sendiri dengan pengali 0,75
    /// (petunjuk Diary Bab 1, GDD bagian 4).
    /// </summary>
    public class WaterBody : MonoBehaviour
    {
        public WaterKind kind = WaterKind.Sea;
        [Tooltip("Pengali waktu tunggu gigitan. Tebing karang 0,75")]
        public float biteDelayMultiplier = 1f;
    }
}
