using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Tinggi permukaan dan jenis air pada trigger layer Water (laut, sungai, danau).
    /// Blueprint: Island_Logic/WaterSwimmingController.md bagian 1 (#13). Collider laut digerakkan TideManager (#91).
    ///
    /// Catatan implementasi (sistem pendukung): WaterKind sudah ada di WaterBody (FishingSystem #55). Supaya jenis air
    /// tidak dicatat dua kali, WaterVolume membaca kind dari WaterBody di objek yang sama.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(WaterBody))]
    public sealed class WaterVolume : MonoBehaviour
    {
        [SerializeField] private Collider waterCollider;
        [SerializeField] private WaterBody body;

        public WaterKind Kind => body != null ? body.kind : WaterKind.Sea;

        /// <summary>Permukaan air: puncak bounds trigger. Ikut bergerak saat TideManager memindah collider laut.</summary>
        public float SurfaceY => waterCollider != null ? waterCollider.bounds.max.y : transform.position.y;

        public bool IsActive => isActiveAndEnabled && waterCollider != null && waterCollider.enabled;

        private void Awake()
        {
            if (waterCollider == null) waterCollider = GetComponent<Collider>();
            if (body == null) body = GetComponent<WaterBody>();
            waterCollider.isTrigger = true;
        }
    }
}
