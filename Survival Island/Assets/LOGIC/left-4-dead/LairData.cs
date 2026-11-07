using UnityEngine;

namespace SurvivalIsland
{
    public enum LairKind { Cave, MilitaryBunker, VillageWarehouse, RuinedHouse }

    /// <summary>
    /// Data satu sarang zombie. Blueprint: left-4-dead/ZombieLair.md (#112).
    /// GDD bagian 3: sebagian zombie bersembunyi diam di ruang gelap (gua, bunker, gudang) saat siang. Pembagian lokasi
    /// per region dan isi tiap lokasi adalah usulan disetujui developer (usulan 1 dan 2).
    /// </summary>
    [CreateAssetMenu(menuName = "Survival/Lair Data")]
    public class LairData : ScriptableObject
    {
        public string lairId;                // Unik per lokasi, juga SaveId
        public RegionType region;
        public LairKind kind;
        public EnemyData[] occupants = new EnemyData[0];   // Varian yang boleh muncul (enemyId = tag pool)
        public int dormantCount = 4;
        public LootTable lootTable;          // #103, dipakai LootSpawnerSystem di dalam zona
        public bool hasNest;
        [Tooltip("0 = GameBalanceConfig.lair.nestHP (nilai awal 300).")]
        public float nestHP;
    }
}
