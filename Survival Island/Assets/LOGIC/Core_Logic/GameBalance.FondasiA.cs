using System;

namespace SurvivalIsland
{
    // =====================================================================================
    // Field config grup FondasiA (sistem pendukung): settings (#19), localization (#104), shake (#24),
    // vfx (#15), notification (#22), postProcess (#23). Kelas bagian sudah dideklarasikan partial di
    // GameBalanceSections.cs dan sudah punya field di GameBalanceConfig.cs.
    // Nilai awal mengikuti tabel "Angka penyetelan" di blueprint masing-masing dan tabel
    // "Bagian config per sistem" di GameBalanceConfig.md. Nilai yang berlaku dibaca dari aset.
    // =====================================================================================

    /// <summary>Batas menu pengaturan. Blueprint: Core_Logic/SettingsManager.md (#19).</summary>
    public partial class SettingsBalance
    {
        public float sensitivityMin = 0.02f;
        public float sensitivityMax = 0.5f;
        public float fovMin = 60f;
        public float fovMax = 100f;
        // Usulan disetujui developer (#19): batas sempit, supaya gelapnya Region 2 (GDD bagian 5) tetap terasa.
        public float brightnessMin = 0.8f;
        public float brightnessMax = 1.2f;
    }

    /// <summary>Teks, subtitle, dan aksesibilitas. Blueprint: Core_Logic/LocalizationManager.md (#104).</summary>
    public partial class LocalizationBalance
    {
        public float textScaleMin = 0.85f;
        public float textScaleMax = 1.5f;
        public float subtitleMinSeconds = 1.5f;
        public float subtitleSecondsPerChar = 0.06f;
        public int subtitleMaxRows = 3;
        public int maxFlashesPerSecond = 3;
    }

    /// <summary>Konversi sinyal Impulse ke offset kamera. Blueprint: Core_Logic/CameraShake.md (#24).</summary>
    public partial class ShakeBalance
    {
        public float degreesPerUnit = 2f;
        public float metersPerUnit = 0.02f;
        public float rollPerUnit = 1f;
        public float maxDegrees = 8f;          // Lebih kecil dari camera.maxOffsetDegrees (15)
        public float maxMeters = 0.12f;
        public float hitReferenceDamage = 25f; // Damage yang menghasilkan skala 1
        public float hitScaleMin = 0.5f;
        public float hitScaleMax = 1.5f;
    }

    /// <summary>Katalog efek sekali muncul. Blueprint: Core_Logic/VFXManager.md (#15).</summary>
    public partial class VfxBalance
    {
        // Tabel "Katalog efek" di VFXManager.md: poolTag, umur, maxActive, cullDistance, minInterval.
        // Isi pool di ObjectPooler.pools tidak boleh lebih kecil dari maxActive.
        public VFXEntry[] catalog =
        {
            new VFXEntry("VFX_Muzzle", 0.5f, 8, 0f, 0f),
            new VFXEntry("VFX_LaserMuzzle", 0.5f, 8, 0f, 0f),
            new VFXEntry("VFX_Impact", 0.5f, 24, 60f, 0f),
            new VFXEntry("VFX_ImpactDust", 0.5f, 24, 60f, 0f),
            new VFXEntry("VFX_ImpactWood", 0.5f, 16, 60f, 0f),
            new VFXEntry("VFX_LaserImpact", 0.5f, 8, 60f, 0f),
            new VFXEntry("VFX_Blood", 0.8f, 32, 40f, 0.02f),
            new VFXEntry("VFX_BloodHeavy", 1.2f, 12, 40f, 0f),
            new VFXEntry("VFX_MetalSpark", 0.4f, 12, 40f, 0f),
            new VFXEntry("VFX_WoodDust", 0.8f, 8, 40f, 0.05f),
            new VFXEntry("VFX_StoneDust", 0.8f, 8, 40f, 0.05f),
            new VFXEntry("VFX_LeafBurst", 0.8f, 8, 40f, 0.05f),
            new VFXEntry("VFX_GachaSpark", 2f, 4, 0f, 0f),
            new VFXEntry("VFX_Splash", 1f, 8, 40f, 0.1f),
            new VFXEntry("VFX_Explosion", 2f, 4, 0f, 0f),     // Usulan EnvironmentalHazard.md
            new VFXEntry("VFX_ShipPickup", 0.5f, 4, 0f, 0f),  // Usulan ShipInteractable.md
        };

        public float lowQualityDensity = 0.5f;
        public float mediumQualityDensity = 0.75f;
    }

    /// <summary>Notifikasi pojok layar. Blueprint: Core_Logic/NotificationManager.md (#22).</summary>
    public partial class NotificationBalance
    {
        public int maxVisible = 4;
        public int queueCapacity = 12;
        public float infoSeconds = 3f;
        public float warningSeconds = 5f;
        public float fadeSeconds = 0.3f;
        public float mergeWindowSeconds = 2f;
        public int logCapacity = 100;
    }

    /// <summary>Efek layar URP. Blueprint: Core_Logic/PostProcessController.md (#23).</summary>
    public partial class PostProcessBalance
    {
        public float lowHpStartRatio = 0.4f;
        public float lowHpMaxVignette = 0.5f;
        public float lowHpPulseHz = 1.2f;
        public float lowHpPulseAmount = 0.08f;
        public float woundVignette = 0.25f;
        public float hitPulseAmount = 0.2f;
        public float hitPulseDecayPerSec = 1f;
        public float freezeVignette = 0.4f;
        public float breathVignette = 0.55f;
        public float breathStartRatio = 0.4f;
        public float sleepVignette = 0.6f;
        public float infectionSaturationMin = -35f;
        public float underwaterBlurStart = 0f;
        public float underwaterBlurEnd = 8f;
        public float changePerSec = 1.5f;
        public float underwaterChangePerSec = 6f;
        public float deathDesaturateSeconds = 2f;
    }
}
