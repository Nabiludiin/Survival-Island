using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Titik pasang suku cadang (trigger layer ZoneTrigger, anak HelicopterFrame). Blueprint:
    /// Island_Logic/RescueEventManager.md bagian 2 "Perakitan" (#14). Pemasangan lewat trigger tanpa tombol E,
    /// jadi aturan QueryTriggerInteraction.Ignore tidak terlibat. Urutan pasang bebas, tanpa bahan tambahan (keputusan 4).
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class PartInstallZone : MonoBehaviour
    {
        public Vector3 Center => transform.position;

        /// <summary>Radius untuk ReconcileLyingParts. Dari GameBalanceConfig.rescue.partInstallRadius.</summary>
        public float Radius => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.rescue.partInstallRadius : 4f;

        private void OnTriggerEnter(Collider other)
        {
            if (other == null || !other.CompareTag("HeavyItem")) return;
            string partId = HeavyItemBridge.GetPartId(other);
            if (!string.IsNullOrEmpty(partId) && RescueEventManager.Instance != null)
                RescueEventManager.Instance.InstallPart(partId);
        }
    }
}
