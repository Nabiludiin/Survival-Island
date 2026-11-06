namespace SurvivalIsland
{
    // =====================================================================================
    // Event sistem pendukung Grup F (left-4-dead: HordeManager, AITraversal, ZombieLair, ThrowableSystem;
    // raft: BaseNavigation, BuildingIntegrity, BaseDefense).
    // Sumber: tabel "Daftar event global" di Core_Logic/GameEventBus.md. Nama dan field mengikuti tabel itu.
    // BloodMoonStartedEvent sudah ada di GameEvents.cs dan TIDAK dideklarasikan ulang di sini.
    // SleepStateChangedEvent milik SleepSystem (#67), bukan grup ini.
    // =====================================================================================

    // ---------------------------------------------------------------- Bencana (HordeManager, #50)

    /// <summary>Peringatan Blood Moon jam warningHour pada hari Blood Moon. Pengirim: HordeManager (#50, #114). Tidak sticky.</summary>
    public struct BloodMoonWarningEvent
    {
        public int day;
        public BloodMoonWarningEvent(int day) { this.day = day; }
    }

    /// <summary>Blood Moon selesai saat fajar. Pengirim: HordeManager (#50). Tidak sticky.</summary>
    public struct BloodMoonEndedEvent
    {
        public int day;
        public BloodMoonEndedEvent(int day) { this.day = day; }
    }

    // ---------------------------------------------------------------- Base (BaseNavigation, #78)

    /// <summary>Status pintu berubah. Pengirim: BaseDoor (#78). Tidak sticky.</summary>
    public struct DoorStateChangedEvent
    {
        public string structureId;
        public DoorState state;
        public DoorStateChangedEvent(string structureId, DoorState state) { this.structureId = structureId; this.state = state; }
    }

    /// <summary>Pemain masuk atau keluar base. Pengirim: BaseNavigation (#78). Sticky.</summary>
    [StickyEvent]
    public struct PlayerBaseStateChangedEvent
    {
        public bool isInside;
        public PlayerBaseStateChangedEvent(bool isInside) { this.isInside = isInside; }
    }

    // ---------------------------------------------------------------- Base (BuildingIntegrity, #47)

    /// <summary>Satu ayunan perbaikan berhasil. Pengirim: BuildingIntegrity (#47). Tidak sticky.</summary>
    public struct StructureRepairedEvent
    {
        public string structureId;
        public float hpRatio;
        public StructureRepairedEvent(string structureId, float hpRatio) { this.structureId = structureId; this.hpRatio = hpRatio; }
    }

    /// <summary>Struktur di-upgrade (structureId tetap). Pengirim: BuildingIntegrity (#47). Tidak sticky.</summary>
    public struct StructureUpgradedEvent
    {
        public string structureId;
        public string buildableId;
        public StructureUpgradedEvent(string structureId, string buildableId) { this.structureId = structureId; this.buildableId = buildableId; }
    }

    // ---------------------------------------------------------------- Region dan musuh (ZombieLair, #112)

    /// <summary>Pemain masuk atau keluar area tertutup. Pengirim: InteriorZone (#112). Sticky.</summary>
    [StickyEvent]
    public struct InteriorZoneChangedEvent
    {
        public string zoneId;
        public bool isInside;
        public bool isDark;
        public InteriorZoneChangedEvent(string zoneId, bool isInside, bool isDark) { this.zoneId = zoneId; this.isInside = isInside; this.isDark = isDark; }
    }

    /// <summary>Sarang zombie dibersihkan (sarangnya hancur). Pengirim: ZombieLair (#112). Tidak sticky.</summary>
    public struct LairClearedEvent
    {
        public string lairId;
        public RegionType region;
        public LairClearedEvent(string lairId, RegionType region) { this.lairId = lairId; this.region = region; }
    }

    // ---------------------------------------------------------------- Inventori (ThrowableSystem, #96)

    /// <summary>Barang lempar terpilih atau jumlahnya berubah. itemId kosong kalau tidak ada. Pengirim: ThrowableSystem. Sticky.</summary>
    [StickyEvent]
    public struct ThrowableChangedEvent
    {
        public string itemId;
        public int count;
        public ThrowableChangedEvent(string itemId, int count) { this.itemId = itemId; this.count = count; }
    }
}
