using System;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Satu jenis musuh di tabel spawn (SpawnManager.md, #62).</summary>
    [Serializable]
    public struct SpawnEntry
    {
        public EnemyData enemy;        // enemyId sekaligus tag pool
        public float weight;
        public int minDay;             // Bobot dianggap 0 sebelum hari ini
        public int maxAlive;           // Batas per jenis. 0 = tanpa batas jenis
        public int groupMin, groupMax;
        public bool isSpecial;         // Dibatasi WaveRequest.specialShare
        public bool useBushPoints;     // Zombie Lumut (#25)
    }

    /// <summary>
    /// Jenis musuh per region, siang dan malam. Blueprint: left-4-dead/SpawnManager.md (#62).
    /// GDD bagian 5 (isi aset, usulan bobot disetujui developer):
    /// Volcano siang serigala 60 (2-3), babi 40 (1-2), malam zombie_standar 100.
    /// Amazon siang jaguar 40 (1), ular 60 (1), malam standar 50, tribal 30, lumut 20.
    /// Snow siang serigala_salju 70 (2-3), beruang 30 (1), malam standar 40, militer 30, beku 30.
    /// Logistic siang anjing 60 (2-4), gagak 40 (5-8), malam zombie_desa 100.
    /// </summary>
    [CreateAssetMenu(fileName = "SpawnTable_", menuName = "Survival/Region Spawn Table")]
    public class RegionSpawnTable : ScriptableObject
    {
        public RegionType region;
        public SpawnEntry[] dayEntries = new SpawnEntry[0];
        public SpawnEntry[] nightEntries = new SpawnEntry[0];
        public string campCounterEnemyId;   // Pembalas untuk #94
    }
}
