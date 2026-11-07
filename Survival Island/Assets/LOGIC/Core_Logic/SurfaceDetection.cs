using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Hasil baca permukaan satu titik (SurfaceDetection.md).</summary>
    public struct SurfaceSample
    {
        public SurfaceType type;     // Jenis dominan
        public float sandWeight;     // 0 sampai 1. Selalu 0 di mesh
        public float groundHeight;   // Tinggi Y titik kena
        public bool isTerrain;       // true kalau berasal dari Terrain
    }

    /// <summary>
    /// Satu jawaban untuk "permukaan apa di titik ini", di Terrain maupun mesh. Manager persisten (IBootstrapService),
    /// karena ShipPhase dan IslandPhase sama-sama memakainya. Blueprint: Core_Logic/SurfaceDetection.md (#86).
    ///
    /// GDD bagian 4: pemain dilarang membangun di pasir pantai. Batas pantai dibaca dari tekstur Terrain yang sama
    /// dengan yang dilihat pemain (alphamap yang di-bake), bukan dari layer Sand/Beach.
    ///
    /// SurfaceDebugOverlay dan perintah surfacedebug (#56) ada di Editor/Development Build. Belum ada (TODO):
    /// SurfaceAuditWindow (alat Editor).
    /// </summary>
    [DisallowMultipleComponent]
    public class SurfaceDetection : MonoBehaviour, IBootstrapService
    {
        public static SurfaceDetection Instance { get; private set; }

        private readonly List<TerrainSurfaceSource> sources = new List<TerrainSurfaceSource>(4);
        private readonly Dictionary<Collider, TerrainSurfaceSource> byCollider = new Dictionary<Collider, TerrainSurfaceSource>(4);

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        public void Initialize()
        {
            // Terrain yang sudah aktif sebelum manager siap (misalnya EditorQuickStart) didaftarkan di sini.
            var existing = FindObjectsByType<TerrainSurfaceSource>(FindObjectsSortMode.None);
            for (int i = 0; i < existing.Length; i++)
                if (existing[i].isActiveAndEnabled) Register(existing[i]);
        }

        public void ResetForNewRun()
        {
            // Tidak ada data run. Terrain baru mendaftar saat scene dimuat.
            sources.Clear();
            byCollider.Clear();
        }

        // ---------------------------------------------------------------- Pendaftaran Terrain

        public void Register(TerrainSurfaceSource source)
        {
            if (source == null || sources.Contains(source)) return;
            sources.Add(source);
            if (source.TerrainCollider != null) byCollider[source.TerrainCollider] = source;
            source.PrepareData(Balance != null ? Balance.surface.runtimeBakeRowsPerFrame : 64);
        }

        public void Unregister(TerrainSurfaceSource source)
        {
            if (source == null) return;
            sources.Remove(source);
            if (source.TerrainCollider != null) byCollider.Remove(source.TerrainCollider);
        }

        // ---------------------------------------------------------------- Fungsi utama

        /// <summary>Jenis permukaan di titik. Tanpa tanah: Tanah.</summary>
        public SurfaceType GetSurfaceAt(Vector3 worldPos)
        {
            return TryGetSurfaceAt(worldPos, out var sample) ? sample.type : SurfaceType.Tanah;
        }

        /// <summary>Raycast pendek ke bawah ke PhysicsMasks.Ground, lalu baca hasilnya.</summary>
        public bool TryGetSurfaceAt(Vector3 worldPos, out SurfaceSample sample)
        {
            float up = Balance != null ? Balance.surface.probeUp : 1f;
            float down = Balance != null ? Balance.surface.probeDown : 3f;
            var ray = new Ray(worldPos + Vector3.up * up, Vector3.down);
            if (!PhysicsQuery.Raycast(ray, out RaycastHit hit, up + down, PhysicsMasks.Ground))
            {
                sample = default;
                return false;
            }
            return ReadFromHit(in hit, out sample);
        }

        /// <summary>Untuk pemanggil yang sudah punya hasil raycast (efek peluru #15). Tanpa raycast baru.</summary>
        public SurfaceType GetSurfaceFromHit(in RaycastHit hit)
        {
            return ReadFromHit(in hit, out var sample) ? sample.type : SurfaceType.Tanah;
        }

        private bool ReadFromHit(in RaycastHit hit, out SurfaceSample sample)
        {
            if (hit.collider != null && byCollider.TryGetValue(hit.collider, out var source))
            {
                ReadTerrain(source, hit.point, out sample);
                return true;
            }

            var tag = hit.collider != null ? hit.collider.GetComponentInParent<SurfaceTag>() : null;
            sample = new SurfaceSample
            {
                // Tanpa SurfaceTag: Tanah. SurfaceAuditWindow melaporkan collider ini.
                type = tag != null ? tag.type : SurfaceType.Tanah,
                sandWeight = 0f,
                groundHeight = hit.point.y,
                isTerrain = false,
            };
            return true;
        }

        private static void ReadTerrain(TerrainSurfaceSource s, Vector3 p, out SurfaceSample sample)
        {
            sample.isTerrain = true;
            sample.groundHeight = s.Terrain != null ? s.Origin.y + s.Terrain.SampleHeight(p) : p.y;
            if (!s.IsReady || s.Width <= 0 || s.Height <= 0)
            {
                // Bake cadangan belum selesai: sumber ini memakai Tanah.
                sample.type = SurfaceType.Tanah;
                sample.sandWeight = 0f;
                return;
            }
            int i = s.CellIndex(p);
            sample.type = (SurfaceType)s.Dominant[i];
            sample.sandWeight = s.Sand[i] / 255f;
        }

        /// <summary>
        /// Baca Terrain langsung lewat batas XZ, tanpa raycast. Benda di atas tanah diabaikan, jadi batu atau lantai
        /// di atas pasir tetap terbaca sebagai pasir.
        /// </summary>
        public bool TryGetTerrainSurfaceAtXZ(Vector3 worldPos, out SurfaceSample sample)
        {
            for (int i = 0; i < sources.Count; i++)
            {
                var s = sources[i];
                if (s == null || !s.ContainsXZ(worldPos)) continue;
                // Seam: berhenti pada Terrain pertama yang memuat titik.
                ReadTerrain(s, worldPos, out sample);
                return true;
            }
            sample = default;
            return false;
        }

        /// <summary>
        /// GDD bagian 4: larangan bangun di pantai. Lima titik tapak (tengah dan empat sudut) dibaca lewat XZ.
        /// Titik dilarang kalau dominan Pasir atau bobot pasirnya mencapai beachBuildSandWeight (usulan disetujui #85).
        /// Titik di luar semua Terrain dilewati; pemeriksaan permukaan di BuildManager yang menolaknya.
        /// </summary>
        public bool IsBeachFootprint(Vector3 center, Vector3 halfExtents, Quaternion rotation)
        {
            float limit = Balance != null ? Balance.surface.beachBuildSandWeight : 0.35f;
            for (int i = 0; i < 5; i++)
            {
                Vector3 local;
                switch (i)
                {
                    case 0: local = Vector3.zero; break;
                    case 1: local = new Vector3(halfExtents.x, 0f, halfExtents.z); break;
                    case 2: local = new Vector3(-halfExtents.x, 0f, halfExtents.z); break;
                    case 3: local = new Vector3(halfExtents.x, 0f, -halfExtents.z); break;
                    default: local = new Vector3(-halfExtents.x, 0f, -halfExtents.z); break;
                }
                Vector3 p = center + rotation * local;
                if (!TryGetTerrainSurfaceAtXZ(p, out var sample)) continue;
                if (sample.type == SurfaceType.Pasir || sample.sandWeight >= limit) return true;
            }
            return false;
        }

        /// <summary>Titik tengah dan nearSandSampleCount titik pada lingkaran. Dipakai SleepSystem (#67) dan #91.</summary>
        public bool IsNearSand(Vector3 worldPos, float radius)
        {
            if (IsSand(worldPos)) return true;
            int count = Balance != null ? Balance.surface.nearSandSampleCount : 8;
            for (int i = 0; i < count; i++)
            {
                float a = (i / (float)count) * Mathf.PI * 2f;
                if (IsSand(worldPos + new Vector3(Mathf.Cos(a), 0f, Mathf.Sin(a)) * radius)) return true;
            }
            return false;
        }

        private bool IsSand(Vector3 p)
        {
            if (!TryGetTerrainSurfaceAtXZ(p, out var s)) return false;
            float limit = Balance != null ? Balance.surface.beachBuildSandWeight : 0.35f;
            return s.type == SurfaceType.Pasir || s.sandWeight >= limit;
        }

        /// <summary>Bobot pasir di XZ, atau 0 di luar Terrain.</summary>
        public float GetSandWeight(Vector3 worldPos)
        {
            return TryGetTerrainSurfaceAtXZ(worldPos, out var s) ? s.sandWeight : 0f;
        }
    }
}
