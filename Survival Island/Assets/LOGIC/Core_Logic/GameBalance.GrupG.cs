namespace SurvivalIsland
{
    // =====================================================================================
    // Angka penyetelan sistem pendukung Grup G. Nilai awal dari tabel "Angka penyetelan" tiap blueprint:
    // raft/HydrationSystem.md (#43), DiseaseManager.md (#45), CampfireManager.md (#44), FishingSystem.md (#55),
    // DebrisRetrieval.md (#106), StorageChest.md (#46). Semua nilai adalah usulan yang disetujui developer sebagai nilai awal.
    // =====================================================================================

    public partial class HydrationBalance
    {
        // exertionThirstMultiplier sudah ada di GameBalanceSections.cs (Langkah 3).
        public float dehydratedStaminaRegenMultiplier = 0f;   // Regenerasi stamina saat Thirst nol
        public float dirtyWaterHoldSeconds = 1f;              // holdSeconds WaterSourcePoint
        public float collectorCapacityPortions = 4f;          // Isi penuh RainwaterCollector
        public float rainFillPerGameHour = 0.5f;
        public float stormFillPerGameHour = 0.75f;
        public float skyCheckDistance = 20f;                  // Raycast atap ke atas
        public float structureRecheckRadius = 6f;             // Bangunan baru dalam jarak ini memicu cek ulang atap
    }

    public partial class DiseaseBalance
    {
        public float foodPoisonGameHours = 3f;
        public float venomGameHours = 2f;
        public float bleedingGameHours = 6f;
        public float foodPoisonDamagePerSec = 0.25f;
        public float venomDamagePerSec = 0.5f;
        public float bleedingDamagePerSec = 0.3f;
        public float bleedChance = 0.25f;
        public float bleedMinDamage = 15f;
        public float overlayMaxAlpha = 0.18f;
        public float overlayPulseSeconds = 2.5f;
    }

    public partial class CampfireBalance
    {
        public float cookSeconds = 30f;          // Checklist issue #44
        public float boilSeconds = 20f;
        public float burnAfterSeconds = 30f;
        public int cookRowCount = 3;
        public float maxFuelSeconds = 300f;
        public float tickIntervalSeconds = 0.25f;
        public float rainFuelMultiplier = 1.5f;
        public bool stormExtinguishes = true;
        public float warmthRadius = 5f;          // Dibaca WarmthSystem (#65)
        public int recipeWood = 5;               // Bahan Kit_Campfire (diisi di aset resep rec_campfire)
        public int recipeStone = 3;
        public float skyCheckDistance = 20f;     // Sama dengan hydration.skyCheckDistance (aturan cek atap #43)
        public float structureRecheckRadius = 6f;
        public float cookedNotifyDistance = 15f; // Notifikasi "Makanan matang" hanya dalam jarak ini
    }

    public partial class FishingBalance
    {
        public float castMinDistance = 6f;
        public float castMaxDistance = 25f;
        public float castChargeSeconds = 1.2f;
        public float castProbeDepth = 40f;
        public float lureFlightSeconds = 0.8f;
        public float leashDistance = 32f;
        public float biteDelayMin = 12f;
        public float biteDelayMax = 30f;
        public float rainBiteMultiplier = 0.85f;
        public float biteWindowSeconds = 2f;
        public float tensionStart = 0.35f;
        public float tensionRisePerSec = 0.30f;
        public float tensionFallPerSec = 0.35f;
        public float surgeIntervalMin = 1.5f;
        public float surgeIntervalMax = 3.5f;
        public float surgeSeconds = 0.8f;
        public float surgeRisePerSec = 0.35f;
        public float progressPerSec = 0.10f;
        public float slackEscapeSeconds = 2f;
        public float recoverSeconds = 1f;
        public float lineBreakRecoverSeconds = 5f;
        public float waterRecheckSeconds = 1f;
    }

    public partial class DebrisBalance
    {
        public bool enabled = true;                  // Sakelar uji
        public int maxActive = 4;
        public float spawnIntervalGameHours = 3f;
        public float spawnCheckSeconds = 1f;
        public float stormIntervalMultiplier = 0.5f;
        public float rainIntervalMultiplier = 0.75f;
        public float highTideIntervalMultiplier = 0.75f;
        public float spawnPlayerMaxDistance = 150f;
        public float driftSpeed = 1.2f;
        public float hookRadius = 2.5f;
        public float pullSpeed = 4f;
        public float collectDistance = 3f;
        public float pullMaxDistance = 32f;
        public float bobAmplitude = 0.08f;           // Efek mengapung (Mathf.Sin kecil), usulan teknis
        public float bobFrequency = 1.2f;
    }

    public partial class StorageBalance
    {
        public int chestSlotCount = 20;
        public float dropScatterRadius = 1.2f;
    }
}
