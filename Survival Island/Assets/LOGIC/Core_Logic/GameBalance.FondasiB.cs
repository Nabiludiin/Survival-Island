namespace SurvivalIsland
{
    // =====================================================================================
    // Angka penyetelan grup FondasiB. Nilai awal sesuai tabel "Angka penyetelan" tiap blueprint
    // dan tabel "Bagian config per sistem" di Core_Logic/GameBalanceConfig.md. Semua angka adalah usulan
    // yang disetujui developer sebagai nilai awal (#85, #113, #20, #103).
    // =====================================================================================

    /// <summary>Loot per lokasi (RegionLootTable.md, #103). Peluang titik House kosong ada di runSeed.lootPointEmptyChance.</summary>
    public partial class LootBalance
    {
        public float warehouseEmptyChance = 0.25f;   // Titik Warehouse kosong
        public int chestRollsMin = 1;                // Tumpukan per peti
        public int chestRollsMax = 3;
        public int bodyRollsMin = 1;                 // Tumpukan per mayat tentara
        public int bodyRollsMax = 2;
        public float amountMulVolcano = 1f;          // Pengali jumlah Region 1
        public float amountMulAmazon = 1.1f;         // Region 2
        public float amountMulLogistic = 1.25f;      // Region 4
        public float amountMulSnow = 1.4f;           // Region 3, area terjauh
        public float rareWeightPerDay = 0.02f;       // Tambahan bobot item langka per hari
        public float rareWeightMaxMul = 2f;          // Batas pengali bobot item langka
        public float amountBonusPer10Days = 0.1f;    // Tambahan jumlah tiap 10 hari
    }

    /// <summary>Efek status (StatusEffectManager.md, #20).</summary>
    public partial class StatusEffectsBalance
    {
        public int maxActive = 8;                        // Batas efek aktif
        public float supplementSeconds = 60f;            // Durasi Buff_Supplement
        public float supplementSpeedMultiplier = 1.2f;   // Kecepatan Buff_Supplement
        public float expiringWarningSeconds = 5f;        // Ikon berkedip menjelang habis
    }

    /// <summary>Durabilitas dan perbaikan (ToolRepairSystem.md, DurabilitySystem.md, #113).</summary>
    public partial class RepairBalance
    {
        public float lowRatio = 0.2f;            // Batas status Low
        public float maxLossPerRepair = 0.1f;    // Penurunan maksimum per perbaikan
        public int maxRepairs = 6;               // Batas perbaikan item non-inti
        public float repairSeconds = 3f;         // Lama memperbaiki
        public float upgradeSeconds = 6f;        // Lama memasang upgrade
    }

    /// <summary>Upgrade kecil per item (ToolRepairSystem.md, #113).</summary>
    public partial class UpgradeBalance
    {
        public float damageBonus = 0.1f;
        public int magazineBonus = 2;
        public float speedBonus = 0.1f;
        public float durabilityBonus = 0.25f;
        public int maxPerItem = 2;
        public float maxDamageMultiplier = 4f;   // Batas akhir pengali damage
    }
}
