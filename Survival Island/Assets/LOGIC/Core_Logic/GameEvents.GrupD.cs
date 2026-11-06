namespace SurvivalIsland
{
    // =====================================================================================
    // Event sistem pendukung Grup D (Island_Logic: NPCRoster, NPCCompanion, ObjectiveManager, LoreCollectibles,
    // RemoteBaseSimulation). Sumber: tabel "Daftar event global" di Core_Logic/GameEventBus.md.
    // Nama, field, dan status sticky mengikuti tabel itu. NPCRescuedEvent sudah ada di GameEvents.cs.
    // =====================================================================================

    // ---------------------------------------------------------------- NPC (#76)

    /// <summary>NPC mati, hilang permanen selama run. Pengirim: NPCCompanion, NPCRoster.KillNpc (#76). Tidak sticky.</summary>
    public struct NPCDiedEvent
    {
        public string npcId;
        public DeathReason reason;
        public NPCDiedEvent(string npcId, DeathReason reason) { this.npcId = npcId; this.reason = reason; }
    }

    /// <summary>State FSM satu NPC berubah. Pengirim: NPCCompanion (#76). Tidak sticky.</summary>
    public struct NPCStateChangedEvent
    {
        public string npcId;
        public NPCState state;
        public NPCStateChangedEvent(string npcId, NPCState state) { this.npcId = npcId; this.state = state; }
    }

    // ---------------------------------------------------------------- Objektif (#81, #100)

    /// <summary>Bab Diary terbuka (GDD bagian 4). Pengirim: ObjectiveManager (#81). Tidak sticky.</summary>
    public struct DiaryChapterUnlockedEvent
    {
        public int chapter;
        public DiaryChapterUnlockedEvent(int chapter) { this.chapter = chapter; }
    }

    /// <summary>Satu tujuan berubah status. Pengirim: ObjectiveManager (#81). Sticky.</summary>
    [StickyEvent]
    public struct ObjectiveChangedEvent
    {
        public string objectiveId;
        public bool isComplete;
        public ObjectiveChangedEvent(string objectiveId, bool isComplete) { this.objectiveId = objectiveId; this.isComplete = isComplete; }
    }

    /// <summary>Tujuan aktif di HUD. objectiveId kosong berarti tujuan cadangan. Pengirim: ObjectiveManager (#81). Sticky.</summary>
    [StickyEvent]
    public struct ActiveObjectiveChangedEvent
    {
        public string objectiveId;
        public int current;
        public int target;
        public ActiveObjectiveChangedEvent(string objectiveId, int current, int target)
        {
            this.objectiveId = objectiveId; this.current = current; this.target = target;
        }
    }

    /// <summary>Penanda arah tujuan aktif. Didengar CompassSystem (#30). Pengirim: ObjectiveManager (#81). Sticky.</summary>
    [StickyEvent]
    public struct ObjectiveMarkerChangedEvent
    {
        public bool hasMarker;
        public UnityEngine.Vector3 position;
        public string label;
        public ObjectiveMarkerChangedEvent(bool hasMarker, UnityEngine.Vector3 position, string label)
        {
            this.hasMarker = hasMarker; this.position = position; this.label = label;
        }
    }

    /// <summary>Lore baru ditemukan di run ini. Pengirim: LoreCollectibles (#100). Tidak sticky.</summary>
    public struct LoreFoundEvent
    {
        public string loreId;
        public RegionType region;
        public bool isFirstEver;
        public LoreFoundEvent(string loreId, RegionType region, bool isFirstEver)
        {
            this.loreId = loreId; this.region = region; this.isFirstEver = isFirstEver;
        }
    }

    /// <summary>Petunjuk gameplay dari lore. Didengar HeavyItemSystem (#69). Pengirim: LoreCollectibles (#100). Tidak sticky.</summary>
    public struct LoreHintGrantedEvent
    {
        public string loreId;
        public LoreHintKind hintKind;
        public RegionType region;
        public LoreHintGrantedEvent(string loreId, LoreHintKind hintKind, RegionType region)
        {
            this.loreId = loreId; this.hintKind = hintKind; this.region = region;
        }
    }

    // ---------------------------------------------------------------- Base (#90)

    /// <summary>Serangan simulasi pada base yang ditinggal dimulai. Pengirim: RemoteBaseSimulation (#90). Tidak sticky.</summary>
    public struct BaseRaidStartedEvent
    {
        public string clusterId;
        public RegionType region;
        public BaseRaidStartedEvent(string clusterId, RegionType region) { this.clusterId = clusterId; this.region = region; }
    }

    /// <summary>Hasil serangan simulasi saat fajar. Pengirim: RemoteBaseSimulation (#90). Tidak sticky.</summary>
    public struct BaseRaidResolvedEvent
    {
        public float damageTotal;
        public int npcInjured;
        public BaseRaidResolvedEvent(float damageTotal, int npcInjured) { this.damageTotal = damageTotal; this.npcInjured = npcInjured; }
    }
}
