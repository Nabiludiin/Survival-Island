using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Jenis permukaan untuk mesh, bangunan, batu besar, dan lantai kapal (#86). Bobot pasir selalu 0.
    /// Blueprint: Core_Logic/SurfaceDetection.md tabel "Mesh dan bangunan".
    /// </summary>
    [DisallowMultipleComponent]
    public class SurfaceTag : MonoBehaviour
    {
        public SurfaceType type = SurfaceType.Tanah;
    }
}
