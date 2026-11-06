namespace SurvivalIsland
{
    // =====================================================================================
    // Event sistem pendukung Grup E (EnvironmentalHazard, FireSystem, LightSourceSystem, FlashlightController,
    // TideManager, SwimmingController, BeachSupplyManager, SleepSystem, RegionStreaming).
    // Sumber: tabel "Daftar event global" di Core_Logic/GameEventBus.md. Nama dan field mengikuti tabel itu.
    // =====================================================================================

    // ---------------------------------------------------------------- Pemain

    /// <summary>Tidur mulai atau selesai. Pengirim: SleepSystem (#67). Sticky.</summary>
    [StickyEvent]
    public struct SleepStateChangedEvent
    {
        public bool isSleeping;
        public SleepWakeReason reason;
        public SleepStateChangedEvent(bool isSleeping, SleepWakeReason reason) { this.isSleeping = isSleeping; this.reason = reason; }
    }

    /// <summary>Status air pemain. Pengirim: SwimmingController (#13). Sticky.</summary>
    [StickyEvent]
    public struct SwimStateChangedEvent
    {
        public bool isSwimming;
        public bool isUnderwater;
        public float breathRatio;
        public SwimStateChangedEvent(bool isSwimming, bool isUnderwater, float breathRatio)
        {
            this.isSwimming = isSwimming;
            this.isUnderwater = isUnderwater;
            this.breathRatio = breathRatio;
        }
    }

    /// <summary>Tingkat cahaya dan keterlihatan pemain. Pengirim: LightSourceSystem (#98). Sticky.</summary>
    [StickyEvent]
    public struct PlayerLightLevelChangedEvent
    {
        public float lightLevel01;
        public float visibility;
        public PlayerLightLevelChangedEvent(float lightLevel01, float visibility) { this.lightLevel01 = lightLevel01; this.visibility = visibility; }
    }

    // ---------------------------------------------------------------- Region

    /// <summary>Tingkat air laut. Pengirim: TideManager (#91). Sticky.</summary>
    [StickyEvent]
    public struct TideChangedEvent
    {
        public float level;
        public bool isHigh;
        public TideChangedEvent(float level, bool isHigh) { this.level = level; this.isHigh = isHigh; }
    }

    /// <summary>Status muat region scene. Pengirim: RegionStreaming (#88). Tidak sticky.</summary>
    public struct RegionLoadStateChangedEvent
    {
        public RegionType region;
        public RegionLoadState state;
        public RegionLoadStateChangedEvent(RegionType region, RegionLoadState state) { this.region = region; this.state = state; }
    }

    // ---------------------------------------------------------------- Suplai (#71)

    /// <summary>Kiriman kargo pantai datang. Pengirim: BeachSupplyManager (#71). Tidak sticky.</summary>
    public struct BeachSupplyArrivedEvent
    {
        public int day;
        public int crateCount;
        public BeachSupplyArrivedEvent(int day, int crateCount) { this.day = day; this.crateCount = crateCount; }
    }

    /// <summary>Satu kargo pantai dibuka. Pengirim: BeachSupplyManager (#71). Tidak sticky.</summary>
    public struct BeachCrateOpenedEvent
    {
        public int day;
        public BeachCrateOpenedEvent(int day) { this.day = day; }
    }

    // ---------------------------------------------------------------- Base

    /// <summary>Struktur mulai atau berhenti terbakar. Pengirim: Flammable (#97). Tidak sticky.</summary>
    public struct StructureBurningEvent
    {
        public string structureId;
        public bool isBurning;
        public StructureBurningEvent(string structureId, bool isBurning) { this.structureId = structureId; this.isBurning = isBurning; }
    }

    /// <summary>Obor Dinding atau Lampu menyala/padam. Pengirim: LightFixture (#98). Tidak sticky.</summary>
    public struct LightSourceToggledEvent
    {
        public string structureId;
        public bool isLit;
        public LightSourceToggledEvent(string structureId, bool isLit) { this.structureId = structureId; this.isLit = isLit; }
    }
}
