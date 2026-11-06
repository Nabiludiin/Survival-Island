using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Titik di atas garis air tertinggi (TideManager.MaxWaterY), tujuan barang yang tidak boleh hilang saat pasang.
    /// Blueprint: Island_Logic/TideManager.md bagian "Barang di pantai" (#91). Diletakkan di IslandPhase.
    /// </summary>
    public sealed class TideSafePoint : MonoBehaviour
    {
        public Vector3 Position => transform.position;

        /// <summary>Titik terdekat dari pos, atau null kalau daftar kosong.</summary>
        public static TideSafePoint Nearest(TideSafePoint[] points, Vector3 pos)
        {
            if (points == null) return null;
            TideSafePoint best = null;
            float bestD = float.MaxValue;
            for (int i = 0; i < points.Length; i++)
            {
                var p = points[i];
                if (p == null) continue;
                float d = (p.transform.position - pos).sqrMagnitude;
                if (d < bestD) { bestD = d; best = p; }
            }
            return best;
        }
    }
}
