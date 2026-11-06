namespace SurvivalIsland
{
    // =====================================================================================
    // Field config grup C2 (sistem pendukung): combatFeedback (#51), compass (#30), tutorial (#16), stats (#57),
    // console (#56), narrative (#99). Kelas bagian sudah dideklarasikan partial di GameBalanceSections.cs dan sudah
    // punya field di GameBalanceConfig.cs. Nilai awal mengikuti tabel "Angka penyetelan" di blueprint masing-masing.
    // run.endingTimeoutSeconds (NarrativeSequencer.md) sudah ada di RunBalance, jadi tidak ditulis ulang.
    // =====================================================================================

    /// <summary>Indikator damage, Hit Marker, dan Kill Marker. Blueprint: Core_Logic/CombatFeedbackUI.md (#51).</summary>
    public partial class CombatFeedbackBalance
    {
        public int maxIndicators = 6;               // Sekaligus ukuran pool UI_DamageIndicator
        public float indicatorSeconds = 1.2f;
        public float indicatorMergeAngle = 30f;
        public float minAlpha = 0.35f;
        public float fullAlphaDamage = 30f;
        public float sourceOffsetMeters = 10f;
        public float telegraphSeconds = 0.6f;
        public float hitMarkerSeconds = 0.12f;
        public float headshotMarkerScale = 1.3f;
        public int killBlinkCount = 2;
        public float killBlinkSeconds = 0.3f;
        public float tinkMinInterval = 0.08f;
    }

    /// <summary>Bilah kompas. Blueprint: Core_Logic/CompassSystem.md (#30). Usulan disetujui developer sebagai nilai awal.</summary>
    public partial class CompassBalance
    {
        public float visibleArcDegrees = 180f;
        public float edgeFadeDegrees = 15f;
        public bool showDistance = true;
        public bool clampObjectiveToEdge = true;
    }

    /// <summary>Pop-up panduan. Blueprint: Core_Logic/TutorialManager.md (#16).</summary>
    public partial class TutorialBalance
    {
        public float displaySeconds = 6f;
        public float minGapSeconds = 3f;
        public float dangerHoldSeconds = 4f;
        public float minSeenSeconds = 2f;
        public int queueLimit = 4;
        public float lowVitalThreshold = 50f;
    }

    /// <summary>Statistik lokal stats.json. Blueprint: Core_Logic/LocalStatsManager.md (#57).</summary>
    public partial class StatsBalance
    {
        public float flushIntervalSeconds = 60f;
    }

    /// <summary>Konsol pengembang. Blueprint: Core_Logic/DevConsole.md (#56).</summary>
    public partial class ConsoleBalance
    {
        public int maxOutputLines = 200;
        public int maxHistory = 50;
    }

    /// <summary>Pemutar adegan. Blueprint: Core_Logic/NarrativeSequencer.md (#99).</summary>
    public partial class NarrativeBalance
    {
        public float beatFadeSeconds = 0.5f;
        public float musicFadeSeconds = 1f;
        public float skipHintSeconds = 3f;
        public float watchdogExtraSeconds = 10f;
    }
}
