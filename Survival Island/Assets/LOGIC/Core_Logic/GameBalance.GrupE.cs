namespace SurvivalIsland
{
    // =====================================================================================
    // Angka penyetelan Grup E. Nilai awal sesuai tabel "Angka penyetelan" tiap blueprint dan tabel
    // "Bagian config per sistem" di Core_Logic/GameBalanceConfig.md. Semua bagian sudah ada di GameBalanceConfig.
    // Semua angka di sini usulan yang disetujui developer sebagai nilai awal (#85), kecuali disebut lain.
    // =====================================================================================

    /// <summary>Tong Merah (EnvironmentalHazard.md, #59). Usulan.</summary>
    public partial class HazardBalance
    {
        public float barrelRadius = 5f;
        public float barrelDamage = 100f;
        public float chainDelaySeconds = 0.15f;
        public float fireFuseSeconds = 2f;
        public float noiseRadius = 40f;
    }

    /// <summary>Api menjalar (FireSystem.md, #97).</summary>
    public partial class FireBalance
    {
        public int maxBurning = 24;
        public int maxAreas = 8;
        public int maxCarvedAreas = 4;
        public float burnTickSeconds = 0.5f;
        public float spreadTickSeconds = 1f;
        public int spreadChecksPerTick = 6;
        public float areaTickSeconds = 0.5f;
        public float areaIgniteChancePerTick = 0.25f;
        public float reigniteCooldownSeconds = 20f;
        public int maxVegetationGeneration = 3;
        public float rainSpreadMultiplier = 0.25f;
        public float rainBurnMultiplier = 0.5f;          // Dipakai sebagai: burnRemaining berkurang 1/0,5 = 2 kali lebih cepat, dan durasi area dikali 0,5
        public float snowSpreadMultiplier = 0.5f;
        public float stormAreaMultiplier = 0.25f;
        public float roofRecheckSeconds = 5f;
        public float roofCheckDistance = 20f;            // Tambahan implementasi: sama dengan campfire.skyCheckDistance
        public float extinguishHoldSeconds = 1f;
        public float fireballAreaRadius = 4f;
        public float fireballAreaSeconds = 12f;
        public float fireballDamagePerSec = 8f;
        public float fireAxeIgniteChance = 0.25f;
        public float campfireTrapIgniteChancePerTick = 0.1f;
    }

    /// <summary>Sumber cahaya, keterlihatan, dan batas lampu (LightSourceSystem.md, #98).</summary>
    public partial class LightBalance
    {
        public float visibilityTickSeconds = 0.25f;
        public float budgetTickSeconds = 0.5f;
        public float fixtureTickSeconds = 1f;
        public float ambientDay = 1f;
        public float ambientNight = 0.1f;
        public float ambientAmazonDay = 0.45f;
        public float ambientInterior = 0.1f;
        public int interiorMaxRealtimeLights = 3;
        public float stormAmbientMultiplier = 0.7f;
        public float visibilityInDark = 0.6f;
        public float visibilityInLight = 1f;
        public float flashlightVisibilityBonus = 0.3f;
        public int maxRegisteredSources = 64;
        public int maxRealtimeLights = 6;
        public int maxShadowLights = 1;
        public float budgetSwapMargin = 1.2f;
        public int maxLightStructures = 24;
        public float rainFuelMultiplier = 1.5f;
        public bool stormExtinguishes = true;
        public float flickerSeconds = 2f;
        public float attractionChancePerLight = 0.05f;
        public float attractionChanceMax = 0.35f;
        public float mossStaticLightLevel = 0.5f;
        public float lightFadeSeconds = 0.25f;           // Peralihan lampu real-time (blueprint: 0,25 detik)
        public float addFuelHoldSeconds = 0.6f;          // Tahan E untuk menambah bahan bakar (blueprint: 0,6 detik)
        public float visibilityPublishDelta = 0.02f;
    }

    /// <summary>Senter (FlashlightController.md, #98).</summary>
    public partial class FlashlightBalance
    {
        public float maxBattery = 100f;
        public float drainPerSec = 0.2f;
        public float batteryRecharge = 100f;
        public float range = 25f;
        public float spotAngle = 50f;
        public float flickerHz = 6f;
    }

    /// <summary>Air pasang (TideManager.md, #91).</summary>
    public partial class TideBalance
    {
        public float lowWaterY = 0f;
        public float highWaterY = 1.6f;
        public float dayAmplitude = 0.6f;
        public float nightAmplitude = 1f;
        public float highThreshold = 0.7f;
        public float lowThreshold = 0.2f;
        public float warnLevel = 0.55f;
        public float stormSurgeLevel = 0.25f;
        public float surgeRatePerGameHour = 0.125f;
        public float colliderStepMeters = 0.15f;
        public float wadeDepth = 0.35f;
        public float wadeStaminaPerSec = 2f;
        public float highAreaCost = 20f;
        public float tickSeconds = 0.25f;                // Blueprint: Update setiap 0,25 detik
        public float visualLerpSpeed = 0.5f;             // Meter per detik untuk gerak halus mesh laut
        public float publishLevelDelta = 0.05f;
    }

    /// <summary>Berenang (WaterSwimmingController.md, #13). swimStaminaPerSec dan drownDamagePerSec ada di vitals.</summary>
    public partial class SwimmingBalance
    {
        public float idleStaminaPerSec = 2f;
        public float swimSpeedMultiplier = 0.5f;
        public float wadeSpeedMultiplier = 0.7f;
        public float wadeEnterDepth = 0.5f;
        public float swimEnterDepth = 1.3f;
        public float swimExitDepth = 1.0f;
        public float maxBreathSeconds = 30f;
        public float breathRecoverPerSec = 10f;
        public float sinkingBreathMultiplier = 2f;
        public float sinkSpeed = 0.5f;
        public float verticalSwimSpeed = 1.5f;
        public float surfaceEyeOffset = 0.3f;
        public float waterCheckSeconds = 0.1f;
        public float landingProbeDistance = 30f;
        public float breathPublishHz = 4f;               // Blueprint: 4 kali per detik selama breathRatio < 1
    }

    /// <summary>Kargo pantai mingguan (BeachSupplyManager.md, #71). GDD bagian 6: Weekly Beach Supply setiap 7 hari.</summary>
    public partial class BeachSupplyBalance
    {
        public int intervalDays = 7;                     // GDD bagian 6
        public int crateCountBase = 2;
        public int crateCountPerTwoWeeks = 1;
        public int crateCountMax = 4;
        public int rollsPerCrate = 3;
        public float crateLifetimeGameHours = 18f;
        public float crateMinLevel = 0.85f;
        public float openHoldSeconds = 1.5f;
        public float openNoiseRadius = 10f;
        public float ambushTriggerDistance = 50f;
        public int ambushAnimalsBase = 2;
        public int ambushAnimalsMax = 4;
        public int ambushZombiesBase = 4;
        public int ambushZombiesMax = 10;
        public float ambushSpawnRadius = 25f;            // Blueprint: RequestBurst(..., tengah, 25)
        public float pointNavSampleDistance = 2f;
        public float pointClearRadius = 1.5f;
        public float fallbackSpacing = 1.5f;
    }

    /// <summary>Tidur (SleepSystem.md, #67). maxGameHoursPerStep sudah ada di GameBalanceSections.</summary>
    public partial class SleepBalance
    {
        public float timeMultiplierAsleep = 2160f;
        public float drainRatioAsleep = 0.5f;
        public float enemyBlockRadius = 25f;
        public float enemyWakeRadius = 8f;
        public float enemyCheckSeconds = 0.5f;
        public float structureAlarmRadius = 15f;
        public float minVitalToSleep = 10f;
        public float retrySeconds = 5f;
        public float hpPerGameHourInBase = 2f;
        public float hpPerGameHourOutside = 0f;
        public float minHoursForStamina = 4f;
        public float interruptChanceInBase = 0.25f;
        public float interruptChanceOutside = 0.6f;
        public float interruptChanceBeach = 1f;
        public float interruptMinProgress = 0.25f;
        public float interruptMaxProgress = 0.9f;
        public int ambushBaseCount = 4;
        public int ambushCountPerTenDays = 1;
        public float ambushRadiusInBase = 20f;
        public float ambushRadiusExposed = 10f;
        public float beachRadius = 8f;
        public float beachTideStaminaLoss = 30f;
    }

    /// <summary>Streaming region (RegionStreaming.md, #88). Usulan teknis.</summary>
    public partial class StreamingBalance
    {
        public float loadDistance = 150f;
        public float unloadDistance = 250f;
        public float urgentDistance = 40f;
        public float checkIntervalSeconds = 0.5f;
        public int maxLoadedRegions = 2;
        public float stallNoticeSeconds = 2f;
        public float stallPanelSeconds = 10f;
        public float safePositionIntervalSeconds = 1f;
        public float unusedAssetsMinIntervalSeconds = 120f;
        public int baseMemoryLimitMB = 1000;
        public int regionMemoryLimitMB = 1200;
        public int regionBatchLimit = 600;
        public int baseBatchLimit = 300;
        public float regionLoadTimeLimitSeconds = 8f;
        public float loadRetrySeconds = 5f;              // Blueprint "Scene gagal dimuat": coba lagi setelah 5 detik
        public float safeGroundProbeDistance = 3f;
        public float stallSpeedThreshold = 0.3f;         // Pemain dianggap tertahan kalau hampir diam di dekat SeamBarrier
    }
}
