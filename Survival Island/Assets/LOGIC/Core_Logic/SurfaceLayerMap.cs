using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Pasangan TerrainLayer ke SurfaceType, per indeks (#86). Dua tekstur rumput boleh dipetakan ke satu Rumput.
    /// Blueprint: Core_Logic/SurfaceDetection.md "Data bake".
    /// </summary>
    [CreateAssetMenu(menuName = "Survival/Surface Layer Map")]
    public class SurfaceLayerMap : ScriptableObject
    {
        public TerrainLayer[] layers;
        public SurfaceType[] types;     // Pasangan per indeks

        /// <summary>Jenis untuk satu TerrainLayer. Layer tanpa pasangan dianggap Tanah (audit melaporkannya).</summary>
        public SurfaceType GetType(TerrainLayer layer)
        {
            if (layers != null && types != null)
            {
                int n = Mathf.Min(layers.Length, types.Length);
                for (int i = 0; i < n; i++)
                    if (layers[i] == layer) return types[i];
            }
            return SurfaceType.Tanah;
        }

        /// <summary>true kalau layer punya pasangan.</summary>
        public bool Contains(TerrainLayer layer)
        {
            if (layers == null) return false;
            for (int i = 0; i < layers.Length; i++)
                if (layers[i] == layer) return true;
            return false;
        }
    }
}
