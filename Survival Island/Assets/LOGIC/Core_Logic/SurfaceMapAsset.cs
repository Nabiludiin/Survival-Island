using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Cache alphamap satu Terrain hasil SurfaceBakeTool (#86). Satu byte jenis dominan dan satu byte bobot pasir per sel,
    /// sehingga pembacaan saat bermain tanpa garbage. Blueprint: Core_Logic/SurfaceDetection.md "Data bake".
    /// Aset ini dibuat alat Editor, jadi tidak punya menu Create.
    /// </summary>
    public class SurfaceMapAsset : ScriptableObject
    {
        public int width, height;
        public byte[] dominant;     // Nilai SurfaceType per sel
        public byte[] sand;         // Bobot pasir per sel, 0 sampai 255
        public Hash128 sourceHash;  // Dari alphamap, untuk mendeteksi data lama

        /// <summary>Ukuran cocok dengan alphamap Terrain dan array terisi.</summary>
        public bool IsValidFor(TerrainData data)
        {
            if (data == null || dominant == null || sand == null) return false;
            int n = width * height;
            return width == data.alphamapWidth && height == data.alphamapHeight && dominant.Length == n && sand.Length == n;
        }
    }
}
