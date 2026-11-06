#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Alat Editor: bake alphamap Terrain menjadi SurfaceMapAsset (#86). Blueprint: Core_Logic/SurfaceDetection.md "Cache alphamap".
    /// Menu: Survival > Surface > Bake Terrain Terpilih. Terrain harus punya TerrainSurfaceSource dengan layerMap terisi.
    /// Aset disimpan di samping scene region (atau di folder Assets kalau scene belum disimpan).
    ///
    /// Belum ada di Langkah 4: SurfaceAuditWindow (tampilan warna, garis merah batas pantai, laporan collider tanpa SurfaceTag).
    /// </summary>
    public static class SurfaceBakeTool
    {
        [MenuItem("Survival/Surface/Bake Terrain Terpilih")]
        private static void BakeSelected()
        {
            int baked = 0;
            foreach (var go in Selection.gameObjects)
            {
                var source = go.GetComponent<TerrainSurfaceSource>();
                if (source == null) continue;
                if (Bake(source)) baked++;
            }
            Debug.Log($"[SurfaceBakeTool] {baked} Terrain di-bake.");
        }

        public static bool Bake(TerrainSurfaceSource source)
        {
            var terrain = source.GetComponent<Terrain>();
            var data = terrain != null ? terrain.terrainData : null;
            if (data == null) { Debug.LogError("[SurfaceBakeTool] Terrain tanpa TerrainData."); return false; }
            if (source.layerMap == null) { Debug.LogError($"[SurfaceBakeTool] '{source.name}' belum punya SurfaceLayerMap."); return false; }

            int w = data.alphamapWidth, h = data.alphamapHeight;
            var layers = data.terrainLayers;
            int layerCount = layers.Length;
            var layerTypes = new SurfaceType[layerCount];
            for (int i = 0; i < layerCount; i++)
            {
                if (!source.layerMap.Contains(layers[i]))
                    Debug.LogWarning($"[SurfaceBakeTool] TerrainLayer '{(layers[i] != null ? layers[i].name : "null")}' tidak punya pasangan di SurfaceLayerMap.");
                layerTypes[i] = source.layerMap.GetType(layers[i]);
            }

            // 1. Baca seluruh alphamap satu kali.
            float[,,] alpha = data.GetAlphamaps(0, 0, w, h);
            var dominant = new byte[w * h];
            var sand = new byte[w * h];
            var weights = new float[8];
            var firstLayer = new int[8];
            var hash = new Hash128();

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    System.Array.Clear(weights, 0, weights.Length);
                    for (int t = 0; t < 8; t++) firstLayer[t] = int.MaxValue;
                    // 2. Jumlahkan bobot per SurfaceType.
                    for (int l = 0; l < layerCount; l++)
                    {
                        float a = alpha[y, x, l];
                        int t = (int)layerTypes[l];
                        weights[t] += a;
                        if (a > 0f && l < firstLayer[t]) firstLayer[t] = l;
                        hash.Append(a);
                    }
                    // 3. Jenis terbesar. Kalau seri, ambil jenis dengan urutan layer lebih awal.
                    int best = (int)SurfaceType.Tanah;
                    float bestW = -1f;
                    for (int t = 0; t < 8; t++)
                    {
                        if (weights[t] > bestW || (Mathf.Approximately(weights[t], bestW) && firstLayer[t] < firstLayer[best]))
                        {
                            bestW = weights[t];
                            best = t;
                        }
                    }
                    int idx = y * w + x;
                    dominant[idx] = (byte)best;
                    // 4. Bobot pasir dikali 255.
                    sand[idx] = (byte)Mathf.Clamp(Mathf.RoundToInt(weights[(int)SurfaceType.Pasir] * 255f), 0, 255);
                }
            }

            // 5. Simpan sourceHash, lalu aset di samping scene region.
            var asset = source.map;
            bool isNew = asset == null;
            if (isNew) asset = ScriptableObject.CreateInstance<SurfaceMapAsset>();
            asset.width = w;
            asset.height = h;
            asset.dominant = dominant;
            asset.sand = sand;
            asset.sourceHash = hash;

            if (isNew)
            {
                string scenePath = source.gameObject.scene.path;
                string folder = string.IsNullOrEmpty(scenePath) ? "Assets" : System.IO.Path.GetDirectoryName(scenePath).Replace('\\', '/');
                string path = AssetDatabase.GenerateUniqueAssetPath($"{folder}/{source.name}_SurfaceMap.asset");
                AssetDatabase.CreateAsset(asset, path);
                source.map = asset;
                EditorUtility.SetDirty(source);
            }
            else
            {
                EditorUtility.SetDirty(asset);
            }
            AssetDatabase.SaveAssets();
            return true;
        }
    }
}
#endif
