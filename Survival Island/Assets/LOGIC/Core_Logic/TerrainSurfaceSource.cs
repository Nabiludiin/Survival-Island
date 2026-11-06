using System.Collections;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Dipasang pada tiap Terrain. Mendaftar ke SurfaceDetection saat aktif, sehingga region yang dimuat atau dilepas (#88)
    /// langsung tercermin. Blueprint: Core_Logic/SurfaceDetection.md bagian 1 "Pola" dan "Cadangan saat bermain".
    ///
    /// Catatan implementasi (Langkah 4): blueprint menulis map dan pemanggilan Register saja. Field layerMap ditambahkan
    /// karena bake cadangan saat bermain perlu tahu jenis tiap TerrainLayer. origin dan size disimpan saat Register.
    /// </summary>
    [RequireComponent(typeof(Terrain))]
    [DisallowMultipleComponent]
    public class TerrainSurfaceSource : MonoBehaviour
    {
        public SurfaceMapAsset map;
        public SurfaceLayerMap layerMap;

        public Terrain Terrain { get; private set; }
        public Collider TerrainCollider { get; private set; }
        public Vector3 Origin { get; private set; }
        public Vector3 Size { get; private set; }

        // Data yang dibaca SurfaceDetection. Menunjuk ke array aset, atau ke hasil bake cadangan.
        public int Width { get; private set; }
        public int Height { get; private set; }
        public byte[] Dominant { get; private set; }
        public byte[] Sand { get; private set; }

        /// <summary>false selama bake cadangan berjalan. Selama itu sumber ini menjawab Tanah.</summary>
        public bool IsReady { get; private set; }

        private Coroutine bakeRoutine;

        private void Awake()
        {
            Terrain = GetComponent<Terrain>();
            TerrainCollider = GetComponent<TerrainCollider>();
        }

        private void OnEnable()
        {
            CacheBounds();
            if (SurfaceDetection.Instance != null) SurfaceDetection.Instance.Register(this);
        }

        private void OnDisable()
        {
            if (bakeRoutine != null) { StopCoroutine(bakeRoutine); bakeRoutine = null; }
            if (SurfaceDetection.Instance != null) SurfaceDetection.Instance.Unregister(this);
        }

        private void CacheBounds()
        {
            if (Terrain == null) Terrain = GetComponent<Terrain>();
            Origin = transform.position;
            Size = Terrain != null && Terrain.terrainData != null ? Terrain.terrainData.size : Vector3.one;
        }

        /// <summary>true kalau titik XZ berada di dalam Terrain ini.</summary>
        public bool ContainsXZ(Vector3 p)
        {
            return p.x >= Origin.x && p.x <= Origin.x + Size.x && p.z >= Origin.z && p.z <= Origin.z + Size.z;
        }

        /// <summary>Indeks sel untuk titik dunia, tanpa alokasi (SurfaceDetection.md "Membaca peta Terrain").</summary>
        public int CellIndex(Vector3 p)
        {
            float u = Mathf.Clamp01((p.x - Origin.x) / Size.x);
            float v = Mathf.Clamp01((p.z - Origin.z) / Size.z);
            int x = Mathf.RoundToInt(u * (Width - 1));
            int y = Mathf.RoundToInt(v * (Height - 1));
            return y * Width + x;
        }

        /// <summary>Dipanggil SurfaceDetection.Register. Memakai aset kalau cocok, kalau tidak mulai bake cadangan.</summary>
        internal void PrepareData(int rowsPerFrame)
        {
            CacheBounds();
            var data = Terrain != null ? Terrain.terrainData : null;
            if (map != null && map.IsValidFor(data))
            {
                Width = map.width;
                Height = map.height;
                Dominant = map.dominant;
                Sand = map.sand;
                IsReady = true;
                return;
            }

            // Build rilis harus lulus SurfaceAuditWindow, jadi cadangan ini hanya untuk pengembangan.
            Debug.LogWarning($"[SurfaceDetection] Terrain '{name}' tanpa SurfaceMapAsset yang cocok. Bake cadangan saat bermain dimulai.");
            IsReady = false;
            if (data == null || !isActiveAndEnabled) return;
            if (bakeRoutine != null) StopCoroutine(bakeRoutine);
            bakeRoutine = StartCoroutine(RuntimeBake(data, Mathf.Max(1, rowsPerFrame)));
        }

        private IEnumerator RuntimeBake(TerrainData data, int rowsPerFrame)
        {
            int w = data.alphamapWidth;
            int h = data.alphamapHeight;
            var dominant = new byte[w * h];
            var sand = new byte[w * h];
            TerrainLayer[] layers = data.terrainLayers;
            int layerCount = layers != null ? layers.Length : 0;
            var layerTypes = new SurfaceType[layerCount];
            for (int i = 0; i < layerCount; i++)
                layerTypes[i] = layerMap != null ? layerMap.GetType(layers[i]) : SurfaceType.Tanah;

            const int typeCount = 8;
            var weights = new float[typeCount];

            for (int y0 = 0; y0 < h; y0 += rowsPerFrame)
            {
                int rows = Mathf.Min(rowsPerFrame, h - y0);
                // Hanya bake cadangan yang memanggil GetAlphamaps saat bermain (SurfaceDetection.md "Cache alphamap").
                float[,,] chunk = data.GetAlphamaps(0, y0, w, rows);
                for (int y = 0; y < rows; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        System.Array.Clear(weights, 0, typeCount);
                        for (int l = 0; l < layerCount; l++)
                            weights[(int)layerTypes[l]] += chunk[y, x, l];
                        // Jenis terbesar. Kalau seri, nilai enum yang lebih awal (mendekati urutan layer di bake Editor).
                        int best = (int)SurfaceType.Tanah;
                        float bestW = -1f;
                        for (int t = 0; t < typeCount; t++)
                            if (weights[t] > bestW) { bestW = weights[t]; best = t; }
                        int idx = (y0 + y) * w + x;
                        dominant[idx] = (byte)best;
                        sand[idx] = (byte)Mathf.Clamp(Mathf.RoundToInt(weights[(int)SurfaceType.Pasir] * 255f), 0, 255);
                    }
                }
                yield return null;
            }

            Width = w;
            Height = h;
            Dominant = dominant;
            Sand = sand;
            IsReady = true;
            bakeRoutine = null;
        }
    }
}
