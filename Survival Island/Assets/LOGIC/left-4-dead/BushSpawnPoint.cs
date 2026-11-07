using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Titik semak tempat Zombie Lumut muncul menyamar di Region 2. Penanda scene Region2_Amazon, tanpa logika sendiri.
    /// Blueprint: left-4-dead/SpawnManager.md ("Zombie Lumut", SpawnEntry.useBushPoints) dan SpecialZombieAI.md (#25, #62).
    /// GDD bagian 5, Region 2: Zombie Lumut menyamar di antara semak.
    /// SpawnManager memilih titik yang memenuhi syarat jarak dan pandangan, lalu MossBehavior memulai zombie dari Dormant.
    /// Daftar statis diisi OnEnable dan dilepas OnDisable, jadi titik di region yang dilepas (#88) tidak dipilih.
    /// Letakkan Transform ini di NavMesh, di samping atau di dalam semak.
    /// </summary>
    public class BushSpawnPoint : MonoBehaviour
    {
        private static readonly List<BushSpawnPoint> all = new List<BushSpawnPoint>(32);

        public static IReadOnlyList<BushSpawnPoint> All => all;

        public Vector3 Position => transform.position;

        private void OnEnable() { if (!all.Contains(this)) all.Add(this); }
        private void OnDisable() { all.Remove(this); }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.2f, 0.6f, 0.2f, 0.8f);
            Gizmos.DrawWireSphere(transform.position + Vector3.up, 1f);
        }
#endif
    }
}
