namespace SurvivalIsland
{
    // =====================================================================================
    // Field config grup C1 (sistem pendukung audio, animasi, dan optimasi): ambientAudio (#52), combatMusic (#114),
    // footstep (#21), animation (#12), optimization (#62, #93). Kelas bagian sudah dideklarasikan partial di
    // GameBalanceSections.cs dan sudah punya field di GameBalanceConfig.cs.
    // Nilai awal mengikuti tabel "Angka penyetelan" di blueprint masing-masing. Field yang ditandai
    // "tambahan implementasi" adalah angka yang ditulis langsung di teks blueprint (bukan di tabel), dipindah ke config
    // supaya tidak tertulis di logika (#84). Lihat "Catatan Implementasi (Sistem pendukung)" di blueprint terkait.
    // =====================================================================================

    /// <summary>Suara latar per area. Blueprint: Core_Logic/AmbientAudio.md (#52).</summary>
    public partial class AmbientAudioBalance
    {
        public float crossfadeSeconds = 3f;
        public float zoneExitGraceSeconds = 1.5f;
        public float retargetDuckSeconds = 0.2f;
        public float dayNightBlendSeconds = 20f;
        public float underwaterLowpassHz = 800f;
        public float sleepDuckDb = -6f;
        public float runEndFadeSeconds = 3f;

        // Tambahan implementasi (angka dari teks AmbientAudio.md bagian 2 dan 4).
        public float startCrossfadeSeconds = 0.5f;    // "BeginCrossfade(target, 0,5)" saat mulai run dan load
        public float tideMinVolume = 0.6f;            // "Lerp(0,6, 1, level)" untuk layer followsTide
        public float oneShotHeightMin = 5f;           // One-shot naik 5 sampai 15 meter dari pemain
        public float oneShotHeightMax = 15f;
    }

    /// <summary>Musik pertempuran dan suara penanda musuh. Blueprint: Core_Logic/CombatMusicDirector.md (#114).</summary>
    public partial class CombatMusicBalance
    {
        public float alertThreshold = 0.2f;
        public float combatThreshold = 0.45f;
        public float peakThreshold = 0.85f;
        public float hysteresis = 0.1f;
        public float riseConfirmSeconds = 0.5f;
        public float minTierHoldSeconds = 12f;
        public float riseFadeSeconds = 1.5f;
        public float fallFadeSeconds = 6f;
        public float nearRadius = 25f;
        public float nearSaturation = 6f;
        public float damageScorePerPoint = 0.02f;
        public float damageScoreDecayPerSec = 0.05f;
        public float pinnedScore = 0.7f;
        public int maxEnemyVoices = 8;
        public int maxSameVariantVoices = 3;
        public float occlusionCheckSeconds = 0.25f;
        public float occlusionLowpassHz = 1500f;
        public int crowdBedMinEngaged = 12;
        public float disasterMusicDuckDb = -6f;
        public float stingerMinInterval = 3f;

        // Tambahan implementasi (angka dari teks CombatMusicDirector.md bagian 2 dan 4).
        public float evaluateIntervalSeconds = 0.25f;  // EvaluateScore "4 kali per detik"
        public float phaseFloorRelax = 0.1f;           // Tabel PhaseFloor
        public float phaseFloorBuildUp = 0.25f;
        public float phaseFloorPeakFade = 0.5f;
        public float phaseFloorSustainPeak = 0.85f;
        public float largeThreatWeight = 3f;           // Bobot musuh cues.isLarge
        public float alertCalmVolume = 0.5f;           // "Calm lebih pelan" pada tingkat Alert (usulan nilai)
        public float calmDayNightFadeSeconds = 10f;    // calmDay dan calmNight saling crossfade 10 detik
        public float scheduleLeadSeconds = 0.1f;       // PlayScheduled(dspTime + 0,1)
        public int emitterTicksPerFrame = 16;          // Maksimal 16 emitter per frame
        public int tellPriorityBonus = 2;              // PlayTell memakai priority + 2
        public float voiceStealFadeSeconds = 0.05f;
        public float sameClipMinSeconds = 0.15f;
        public int crowdBedFullEngaged = 24;           // Jumlah musuh untuk volume kerumunan penuh (usulan nilai)
        public float cinematicFadeOutSeconds = 1f;
        public float cinematicFadeInSeconds = 2f;
        public float runEndFadeSeconds = 2f;
        public float loadStingerSuppressSeconds = 2f;  // bloodMoonStartClip dilewati 2 detik pertama setelah load
        public float cueMinDistance = 2f;              // Pengaturan sumber 3D
        public float cueSpread = 60f;
    }

    /// <summary>Suara langkah pemain. Blueprint: Core_Logic/FootstepAudio.md (#21).</summary>
    public partial class FootstepBalance
    {
        public float stepDistance = 1.75f;
        public float minSpeed = 0.5f;
        public float baseVolume = 0.6f;
        public float crouchVolumeMultiplier = 0.4f;
        public float sprintSpeedThreshold = 6f;
        public float sprintVolumeMultiplier = 1.3f;
        public float heavyVolumeMultiplier = 1.2f;
        public float pitchMin = 0.93f;
        public float pitchMax = 1.07f;
        public float volumeJitter = 0.1f;
        public float minLandingAirSeconds = 0.25f;
        public float landingFullFallSpeed = 12f;

        // Tambahan implementasi: "Mathf.Lerp(0.5f, 1.3f, impact01)" di PlayLanding.
        public float landingVolumeMin = 0.5f;
        public float landingVolumeMax = 1.3f;
    }

    /// <summary>Pembungkus Animator. Blueprint: Core_Logic/AnimationController.md (#12).</summary>
    public partial class AnimationBalance
    {
        public float locomotionDampSeconds = 0.1f;
        public float enemySpeedSampleSeconds = 0.1f;
        public float minReloadAnimSpeed = 0.5f;
        public float maxReloadAnimSpeed = 2f;
    }

    /// <summary>Culling jarak jauh. Blueprint: Core_Logic/OptimizationManager.md (#62, #93). Usulan.</summary>
    public partial class OptimizationBalance
    {
        public float cullingDistance = 50f;
        public float cullCheckSeconds = 1f;

        // Tambahan implementasi: radius NavMesh.SamplePosition saat agen dinyalakan kembali (Uncull langkah 1).
        public float uncullSampleRadius = 2f;
    }
}
