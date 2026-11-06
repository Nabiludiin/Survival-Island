namespace SurvivalIsland
{
    // =====================================================================================
    // Angka penyetelan Grup F. Nilai awal sesuai tabel "Angka penyetelan" tiap blueprint dan tabel
    // "Bagian config per sistem" di Core_Logic/GameBalanceConfig.md. Semua bagian sudah ada di GameBalanceConfig.
    // =====================================================================================

    /// <summary>Blood Moon (HordeManager.md, #50). GDD bagian 3: setiap 10 hari. Isi gelombang usulan disetujui developer.</summary>
    public partial class BloodMoonBalance
    {
        public bool enabled = true;                 // Aturan GDD bagian 3. Saklar hanya untuk tes
        public int intervalDays = 10;               // GDD bagian 3: hari ke-10, 20, 30, ...
        public int warningHour = 12;
        public int waveCount = 4;
        public float firstWaveDelaySeconds = 20f;
        public float waveIntervalSeconds = 90f;
        public int baseWaveSize = 12;
        public int waveSizePerEvent = 4;
        public int maxAlive = 50;                   // SpawnOverride.capOverride
        public int sustainAlive = 20;
        public float sustainCheckSeconds = 30f;
        public float burstRadius = 45f;
        public float baseEngageDistance = 120f;
    }

    /// <summary>Jalur musuh, pintu, dan deteksi dalam base (BaseNavigation.md, #78). Usulan disetujui developer (#85).</summary>
    public partial class BaseNavBalance
    {
        public float pathCheckIntervalSeconds = 1f;
        public int maxPathChecksPerFrame = 4;
        public float spawnGraceSeconds = 2f;
        public float blockedConfirmSeconds = 1.5f;
        public float retargetCooldownSeconds = 4f;
        public float targetSearchRadius = 6f;
        public int maxAttackersPerWall = 4;
        public int maxAttackersPerDoor = 3;
        public float slotSpacing = 0.8f;
        public float slotStandoff = 0.9f;
        public float waitRingRadius = 4f;
        public float insideCheckSeconds = 0.5f;
        public float insideRayDistance = 6f;
        public int insideMinWallHits = 3;
        public float doorSwingSeconds = 0.4f;
        public float doorNpcAutoCloseSeconds = 1.5f;
        public float lockedDamageMultiplier = 0.75f;
    }

    /// <summary>Perbaikan dan upgrade bangunan (BuildingIntegrity.md, #47). Usulan disetujui developer (#85).</summary>
    public partial class IntegrityBalance
    {
        public float repairRange = 4f;
        public float repairSwingSeconds = 0.6f;
        public float repairHpRatioPerSwing = 0.15f;
        public float repairBlockedAfterDamageSeconds = 2f;
        public float upgradeHoldSeconds = 1f;
        public float upgradeBlockedAfterDamageSeconds = 5f;
        public float hammerDecayPerRepairSwing = 0.5f;
        public float hammerDecayPerUpgrade = 2f;
        public float noiseRadiusPerSwing = 15f;
    }

    /// <summary>Jebakan base (BaseDefense.md, #26). Usulan disetujui developer (#85).</summary>
    public partial class TrapBalance
    {
        public float spikeDamagePerHit = 25f;
        public float spikeHitIntervalSeconds = 1.5f;
        public float spikeSelfDamagePerHit = 5f;     // Paku rusak setelah 12 kali kena (HP 60)
        public float spikeNoiseRadius = 6f;
        public float wireDamagePerHit = 4f;
        public float wireHitIntervalSeconds = 1f;
        public float wireSlowMultiplier = 0.5f;
        public float wireSlowSeconds = 1.5f;
        public float wireSelfDamagePerHit = 2f;      // Kawat rusak setelah 40 kali kena (HP 80)
        public float campfireDamagePerHit = 8f;
        public float campfireHitIntervalSeconds = 1f;
        public float tickIntervalSeconds = 0.25f;
        public int maxVictimsPerTrap = 8;
        public int maxTrapsPerRegion = 30;
    }

    /// <summary>Barang lempar (ThrowableSystem.md, #96). GDD bagian 3: Molotov, Bom Rakitan, Batu, Suar.</summary>
    public partial class ThrowableBalance
    {
        public bool enabled = true;                  // Sistem dipakai. false hanya sakelar uji
        public float molotovForce = 15f;
        public float molotovRadius = 3.5f;
        public float molotovDamagePerSec = 15f;
        public float molotovDuration = 8f;
        public float molotovNoise = 15f;
        public float bombForce = 13f;
        public float bombFuse = 3f;
        public float bombRadius = 6f;
        public float bombDamage = 120f;
        public float bombNoise = 40f;
        public float rockForce = 18f;
        public float rockNoise = 12f;
        public float flareForce = 15f;
        public float flareFuse = 0.5f;
        public float flareDuration = 15f;
        public float flareNoise = 25f;
        public float flareNoiseInterval = 1f;
        public float holdToAimSeconds = 0.2f;
        public float throwCooldownSeconds = 0.8f;
        public int arcPointCount = 30;
        public float arcTimeStep = 0.06f;
        public float maxLifetimeSeconds = 25f;
        public int maxActiveLures = 2;
        public float fireTickSeconds = 1f;           // Cadangan Molotov sampai FireSystem (#97) ada (Kasus Khusus)
    }

    /// <summary>Sarang zombie (ZombieLair.md, #112). GDD bagian 3: zombie diam di ruang gelap saat siang.</summary>
    public partial class LairBalance
    {
        public bool enabled = true;                  // Sistem dipakai. false hanya sakelar uji
        public float activationRadius = 40f;
        public float deactivationRadius = 60f;
        public float senseIntervalSeconds = 0.5f;
        public float proximityWakeRadius = 3f;
        public float crouchProximityWakeRadius = 1.5f;
        public float lightWakeRange = 8f;
        public float lightWakeSeconds = 2f;
        public float noiseWakeMultiplier = 1f;
        public float groupWakeRadius = 6f;
        public float doorWaitSeconds = 6f;
        public float returnToDormantSeconds = 20f;
        public float clearedBudgetReduction = 0.1f;
        public float maxBudgetReduction = 0.3f;
        public float nightReleaseRatio = 0.5f;
        public float nestHP = 300f;
        public float registryTickSeconds = 1f;       // LairRegistry.Tick "tiap 1 detik"
    }

    /// <summary>
    /// Bagian AI (#94, AITraversal.md) dari TraversalBalance. Bagian pemain (#101) ada di GameBalanceSections.cs.
    /// Usulan disetujui developer (#85).
    /// </summary>
    public partial class TraversalBalance
    {
        public float probeIntervalSeconds = 1.5f;
        public int maxProbesPerFrame = 2;
        public float campShotWindowSeconds = 5f;
        public float linkAvoidSeconds = 5f;
        public int retreatMaxTries = 3;
        public float reachTolerance = 2.5f;
        public float sampleRadius = 1.5f;
        public float surroundAfterSeconds = 3f;
        public float rockAfterSeconds = 10f;
        public float retreatAfterSeconds = 20f;
        public float surroundRadius = 6f;
        public int maxRockThrowers = 3;
        public float rockRangeMin = 5f;
        public float rockRangeMax = 25f;
        public float rockCooldownSeconds = 8f;
        public float campMoveDistance = 5f;
        public float campShotMultiplier = 2f;
        public float campTier1Seconds = 45f;
        public float campTier2Seconds = 90f;
        public float campRepeatSeconds = 60f;
        public int campCounterCount = 2;
        public float offNavGraceSeconds = 1f;
        public float offNavSampleRadius = 3f;
        public int linkUsersDefault = 1;
        public float linkWaitSeconds = 3f;
        public float jumpSeconds = 0.6f;
        public float climbSpeed = 1.5f;
        public float rockDamage = 10f;               // AttackData Zombie_Rock_Throw (#109): damage 10, infeksi 0
        public float rockWindupSeconds = 1f;         // Ancang-ancang 1 detik
        public float rockSpeed = 16f;                // Usulan teknis: kecepatan lempar batu zombie
        public float retreatDistance = 15f;          // Usulan teknis: jarak titik mundur dari pemain
        public float rockLifetimeSeconds = 6f;       // Usulan teknis: batu yang tidak mengenai apa pun kembali ke pool
        public float linkAvoidCost = 1000f;          // Usulan teknis: biaya area link yang penuh selama linkAvoidSeconds
    }
}
