using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Volume larangan bangun: seam dan link (#88), BridgeSite (#68), helipad dan titik pasang (#14), dan DiaryBook (#81, #116).
    /// Blueprint: raft/BuildMenu.md tabel validasi baris 3. Usulan disetujui: seam, lokasi jembatan, dan helipad.
    ///
    /// Catatan implementasi (Langkah 4): PhysicsQuery selalu mengabaikan trigger (#87), jadi volume ini tidak memakai
    /// collider. Kotak lokal (center, size) didaftarkan di daftar statis dan diperiksa lewat matriks transform.
    /// </summary>
    public class NoBuildVolume : MonoBehaviour
    {
        private static readonly List<NoBuildVolume> active = new List<NoBuildVolume>();

        [SerializeField] private Vector3 center = Vector3.zero;
        [SerializeField] private Vector3 size = new Vector3(10f, 10f, 10f);

        private void OnEnable() { if (!active.Contains(this)) active.Add(this); }
        private void OnDisable() { active.Remove(this); }

        private bool Contains(Vector3 worldPos)
        {
            Vector3 local = transform.InverseTransformPoint(worldPos) - center;
            Vector3 h = size * 0.5f;
            return Mathf.Abs(local.x) <= h.x && Mathf.Abs(local.y) <= h.y && Mathf.Abs(local.z) <= h.z;
        }

        /// <summary>true kalau titik berada di salah satu volume aktif.</summary>
        public static bool IsBlocked(Vector3 worldPos)
        {
            for (int i = 0; i < active.Count; i++)
                if (active[i] != null && active[i].Contains(worldPos)) return true;
            return false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1f, 0.2f, 0.2f, 0.35f);
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(center, size);
        }
    }
}
