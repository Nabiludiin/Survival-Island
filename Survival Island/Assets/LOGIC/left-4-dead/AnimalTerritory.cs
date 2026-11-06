using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Teritori hewan di dekat kumpulan ResourceNode (WildlifeAI.md "Teritori di dekat resource", disetujui developer).
    /// GDD bagian 3: hewan menjaga teritori dan resource. SpawnManager memakai titik ini sebagai homePoint dengan peluang
    /// wildlife.territorySpawnChance.
    /// </summary>
    public class AnimalTerritory : MonoBehaviour
    {
        public string[] enemyIds = new string[0];
        public float radius = 15f;

        private static readonly List<AnimalTerritory> all = new List<AnimalTerritory>(16);
        public static IReadOnlyList<AnimalTerritory> All => all;

        private void OnEnable() { if (!all.Contains(this)) all.Add(this); }
        private void OnDisable() { all.Remove(this); }

        public bool Accepts(string enemyId)
        {
            for (int i = 0; i < enemyIds.Length; i++) if (enemyIds[i] == enemyId) return true;
            return false;
        }
    }
}
