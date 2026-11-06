using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Jalur arus barang hanyut di scene region, dari laut lepas ke titik hilang. Mendaftar di OnEnable dan melepas diri di
    /// OnDisable, sehingga ikut aktif bersama region (#88). Blueprint: raft/DebrisRetrieval.md bagian 1 (#106).
    ///
    /// Aturan membuat jalur: titik pertama dan terakhir minimal 60 meter dari pantai; bagian tengah 10-20 meter dari garis
    /// pantai supaya masuk jangkauan fishing.castMaxDistance. Jalur harus dicek manual di scene (tidak diperiksa LayerAuditWindow).
    /// </summary>
    public class DebrisLane : MonoBehaviour
    {
        [Tooltip("Titik dunia, dari laut lepas ke titik hilang. Minimal 2.")]
        public Vector3[] points = new Vector3[0];
        public RegionType region = RegionType.Volcano;

        public bool IsValid => points != null && points.Length >= 2;

        private void OnEnable()
        {
            if (DebrisRetrieval.Instance != null) DebrisRetrieval.Instance.RegisterLane(this);
        }

        private void Start()
        {
            // Jalur yang aktif sebelum DebrisRetrieval Awake didaftarkan di sini.
            if (DebrisRetrieval.Instance != null) DebrisRetrieval.Instance.RegisterLane(this);
        }

        private void OnDisable()
        {
            // Region dilepas: semua barang di jalur ini kembali ke pool.
            if (DebrisRetrieval.Instance != null) DebrisRetrieval.Instance.UnregisterLane(this);
        }

        /// <summary>Jarak XZ terdekat dari pos ke garis jalur.</summary>
        public float DistanceXZ(Vector3 pos)
        {
            if (!IsValid) return float.MaxValue;
            float best = float.MaxValue;
            for (int i = 0; i < points.Length - 1; i++)
            {
                ClosestOnSegmentXZ(points[i], points[i + 1], pos, out float t);
                Vector3 p = Vector3.Lerp(points[i], points[i + 1], t);
                float dx = p.x - pos.x, dz = p.z - pos.z;
                float d = dx * dx + dz * dz;
                if (d < best) best = d;
            }
            return Mathf.Sqrt(best);
        }

        /// <summary>Segmen dan t terdekat dari pos (dipakai saat barang dilepas kail dan hanyut lagi).</summary>
        public void FindNearest(Vector3 pos, out int segment, out float segmentT)
        {
            segment = 0;
            segmentT = 0f;
            if (!IsValid) return;
            float best = float.MaxValue;
            for (int i = 0; i < points.Length - 1; i++)
            {
                ClosestOnSegmentXZ(points[i], points[i + 1], pos, out float t);
                Vector3 p = Vector3.Lerp(points[i], points[i + 1], t);
                float dx = p.x - pos.x, dz = p.z - pos.z;
                float d = dx * dx + dz * dz;
                if (d < best) { best = d; segment = i; segmentT = t; }
            }
        }

        private static void ClosestOnSegmentXZ(Vector3 a, Vector3 b, Vector3 p, out float t)
        {
            float abx = b.x - a.x, abz = b.z - a.z;
            float len2 = abx * abx + abz * abz;
            t = len2 > 0.0001f ? Mathf.Clamp01(((p.x - a.x) * abx + (p.z - a.z) * abz) / len2) : 0f;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (!IsValid) return;
            Gizmos.color = Color.cyan;
            for (int i = 0; i < points.Length - 1; i++) Gizmos.DrawLine(points[i], points[i + 1]);
        }
#endif
    }
}
