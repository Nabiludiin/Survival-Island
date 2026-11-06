using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Titik buatan tangan untuk kargo pantai selatan Region 1 (di IslandPhase).
    /// Blueprint: Island_Logic/BeachSupplyManager.md bagian 1 (#71). Syarat tinggi di atas WaterYAtLevel(crateMinLevel)
    /// juga diperiksa di Editor (OnValidate tidak bisa membaca TideManager, jadi gizmo merah menandai titik tidak sah saat Play).
    /// </summary>
    public sealed class BeachSupplyPoint : MonoBehaviour
    {
        public bool isReachable = true;

        public Vector3 Position => transform.position;

        private void OnDrawGizmos()
        {
            Gizmos.color = isReachable ? new Color(0.2f, 0.8f, 1f) : Color.red;
            Gizmos.DrawWireCube(transform.position + Vector3.up * 0.5f, Vector3.one);
        }
    }
}
