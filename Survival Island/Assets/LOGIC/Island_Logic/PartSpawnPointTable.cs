using System;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Satu titik suku cadang buatan tangan di Region 4.</summary>
    [Serializable]
    public struct PartSpawnPoint
    {
        public Vector3 position;
        public Vector3 eulerRotation;
        [Tooltip("Diisi lewat pemeriksaan Editor. Titik false tidak boleh dipakai.")]
        public bool isReachable;
    }

    /// <summary>
    /// Daftar titik suku cadang di Region 4 (GLOSSARY.md, #89). Minimal 10 titik untuk 5 barang (RunSeedManager.md).
    /// Ditulis Langkah 6 sebagai ScriptableObject berisi posisi dunia, supaya bisa dibaca saat Region4_Village tidak dimuat.
    /// </summary>
    [CreateAssetMenu(menuName = "Survival/Part Spawn Point Table")]
    public class PartSpawnPointTable : ScriptableObject
    {
        public PartSpawnPoint[] points = Array.Empty<PartSpawnPoint>();

        public int Count => points != null ? points.Length : 0;

        public bool IsValidIndex(int i) => points != null && i >= 0 && i < points.Length && points[i].isReachable;

#if UNITY_EDITOR
        /// <summary>Pemeriksaan Editor: isReachable = ada NavMesh dalam 2 meter. Jalankan saat scene region terbuka.</summary>
        [ContextMenu("Periksa isReachable dengan NavMesh")]
        private void CheckReachable()
        {
            if (points == null) return;
            for (int i = 0; i < points.Length; i++)
                points[i].isReachable = UnityEngine.AI.NavMesh.SamplePosition(points[i].position, out _, 2f, UnityEngine.AI.NavMesh.AllAreas);
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}
