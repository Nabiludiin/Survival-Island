using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SurvivalIsland
{
    /// <summary>
    /// Satu slot ikon penanda di bilah kompas. Dibuat sekali di prefab (12 slot) dan dipakai ulang, tanpa Instantiate.
    /// Blueprint: Core_Logic/CompassSystem.md (#30), bagian "Variabel CompassSystem".
    /// </summary>
    public class CompassIcon : MonoBehaviour
    {
        public Image icon;
        public TextMeshProUGUI distanceText;
        public CanvasGroup group;
        [Tooltip("Panah kiri/kanan untuk penanda Objective yang dijepit di ujung bilah. Boleh kosong.")]
        public GameObject arrowLeft;
        public GameObject arrowRight;

        /// <summary>Angka meter terakhir yang ditulis. -1 = belum pernah, supaya teks pertama selalu ditulis.</summary>
        [System.NonSerialized] public int lastMeters = -1;

        public RectTransform Rect => (RectTransform)transform;

        public void SetArrows(bool outside, float delta)
        {
            if (arrowLeft != null) arrowLeft.SetActive(outside && delta < 0f);
            if (arrowRight != null) arrowRight.SetActive(outside && delta > 0f);
        }
    }
}
