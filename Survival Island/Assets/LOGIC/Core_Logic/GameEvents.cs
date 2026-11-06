namespace SurvivalIsland
{
    // =====================================================================================
    // Struct event global. Sumber: tabel "Daftar event global" di Core_Logic/GameEventBus.md (#107).
    //
    // Langkah 1 hanya menulis event yang dikirim atau didengar sistem Langkah 1.
    // Langkah berikutnya MENAMBAH event-nya di file ini (sesuai tabel di GameEventBus.md),
    // bukan membuat jalur event sendiri. Nama field mengikuti kolom "Isi" di tabel.
    // =====================================================================================

    // ---------------------------------------------------------------- Waktu

    /// <summary>Fajar tiba. Pengirim: DayNightDirector (Langkah 4). Didengar WorldStateSave untuk autosave pagi.</summary>
    [StickyEvent]
    public struct DaybreakEvent
    {
        public int day;
        public DaybreakEvent(int day) { this.day = day; }
    }

    /// <summary>Hari berganti. isReplay = true saat dikirim ulang setelah load (GameManager.PublishDayState).</summary>
    [StickyEvent]
    public struct DayChangedEvent
    {
        public int newDay;
        public bool isReplay;
        public DayChangedEvent(int newDay, bool isReplay) { this.newDay = newDay; this.isReplay = isReplay; }
    }

    /// <summary>Hari target (60) terlewat. Bukan akhir run (GDD bagian 3).</summary>
    [StickyEvent]
    public struct DayLimitPassedEvent
    {
        public int day;
        public DayLimitPassedEvent(int day) { this.day = day; }
    }

    // ---------------------------------------------------------------- Pemain

    /// <summary>
    /// Status tubuh pemain. Pengirim: SurvivalStats (Langkah 3), 4 kali per detik.
    /// Didengar WorldStateSave untuk aturan autosave saat HP rendah.
    /// </summary>
    [StickyEvent]
    public struct VitalsChangedEvent
    {
        public float hp;
        public float stamina;
        public float hunger;
        public float thirst;
        public float infection;
    }

    /// <summary>Pemain mati. Pengirim: SurvivalStats (Langkah 3). Didengar GameManager.</summary>
    public struct PlayerDiedEvent
    {
        public DeathReason reason;
        public PlayerDiedEvent(DeathReason reason) { this.reason = reason; }
    }

    /// <summary>
    /// Pemain diterkam atau lepas. Pengirim: PlayerPinnedState (Langkah 5).
    /// attackerId = EnemyData.enemyId (PlayerPinnedState.md). Didengar UIFocusStack dan WorldStateSave.
    /// </summary>
    [StickyEvent]
    public struct PlayerPinnedEvent
    {
        public string attackerId;
        public bool isPinned;
        public PlayerPinnedEvent(string attackerId, bool isPinned) { this.attackerId = attackerId; this.isPinned = isPinned; }
    }

    // ---------------------------------------------------------------- Bencana

    /// <summary>Bencana mulai. Pengirim: DisasterManager (Langkah 7). Didengar WorldStateSave (tunda autosave).</summary>
    public struct DisasterStartedEvent
    {
        public DisasterType type;
        public int scale;
        public DisasterStartedEvent(DisasterType type, int scale) { this.type = type; this.scale = scale; }
    }

    /// <summary>Bencana selesai. Pengirim: DisasterManager (Langkah 7).</summary>
    public struct DisasterEndedEvent
    {
        public DisasterType type;
        public DisasterEndedEvent(DisasterType type) { this.type = type; }
    }

    // ---------------------------------------------------------------- Run

    /// <summary>Run dimulai atau dilanjutkan. Pengirim: RunLifecycle.</summary>
    [StickyEvent]
    public struct RunStartedEvent
    {
        public uint seed;
        public bool isNewRun;
        public RunStartedEvent(uint seed, bool isNewRun) { this.seed = seed; this.isNewRun = isNewRun; }
    }

    /// <summary>Fase berganti. Pengirim: GameManager.SetPhase.</summary>
    [StickyEvent]
    public struct PhaseChangedEvent
    {
        public GamePhase phase;
        public PhaseChangedEvent(GamePhase phase) { this.phase = phase; }
    }

    /// <summary>Hasil run ditentukan. reason = DeathReason.None saat menang. Pengirim: GameManager.</summary>
    public struct RunEndedEvent
    {
        public RunResult result;
        public DeathReason reason;
        public RunEndedEvent(RunResult result, DeathReason reason) { this.result = result; this.reason = reason; }
    }

    /// <summary>Minta save. Didengar WorldStateSave.</summary>
    public struct SaveRequestedEvent
    {
        public SaveReason reason;
        public SaveRequestedEvent(SaveReason reason) { this.reason = reason; }
    }

    /// <summary>Save selesai. Pendengar mencocokkan reason dengan permintaannya sendiri. Pengirim: WorldStateSave.</summary>
    public struct SaveCompletedEvent
    {
        public SaveReason reason;
        public bool succeeded;
        public SaveCompletedEvent(SaveReason reason, bool succeeded) { this.reason = reason; this.succeeded = succeeded; }
    }

    // ---------------------------------------------------------------- UI

    /// <summary>Status fokus UI berubah. Pengirim: UIFocusStack.ApplyState.</summary>
    [StickyEvent]
    public struct UIFocusChangedEvent
    {
        public bool isGameplayBlocked;
        public bool isPaused;
        public UIFocusChangedEvent(bool isGameplayBlocked, bool isPaused) { this.isGameplayBlocked = isGameplayBlocked; this.isPaused = isPaused; }
    }

    /// <summary>Panel baru dibuka. Pengirim: UIFocusStack.Push. Didengar TutorialManager (#16).</summary>
    public struct UIPanelOpenedEvent
    {
        public UIPanelId panelId;
        public UIPanelOpenedEvent(UIPanelId panelId) { this.panelId = panelId; }
    }

    /// <summary>Bahasa diganti. Pengirim: LocalizationManager (#104). Didengar UIManager.</summary>
    [StickyEvent]
    public struct LocaleChangedEvent
    {
        public string localeId;
        public LocaleChangedEvent(string localeId) { this.localeId = localeId; }
    }

    // ---------------------------------------------------------------- Ditambahkan Langkah 2 (Fase 1 dan interaksi)

    /// <summary>Pemain berhasil berinteraksi dengan target. Pengirim: PlayerInteractor (#17). Didengar #16 dan #57.</summary>
    public struct InteractedEvent
    {
        public string targetId;
        public InteractedEvent(string targetId) { this.targetId = targetId; }
    }

    /// <summary>Isi tangan di kapal berubah. Pengirim: InventoryManager (Langkah 3). Didengar HUD dan ShipInteractable.</summary>
    [StickyEvent]
    public struct ShipHandChangedEvent
    {
        public int weight;
        public int capacity;
        public ShipHandChangedEvent(int weight, int capacity) { this.weight = weight; this.capacity = capacity; }
    }

    /// <summary>Tahap bahaya kapal berganti (sway). Pengirim: ShipHazardManager (#75). Didengar #23, #52, #114.</summary>
    public struct ShipHazardStageChangedEvent
    {
        public int stage;
        public float secondsLeft;
        public ShipHazardStageChangedEvent(int stage, float secondsLeft) { this.stage = stage; this.secondsLeft = secondsLeft; }
    }

    /// <summary>NPC disetor ke sekoci. Pengirim: LifeboatExtraction (#74).</summary>
    public struct NPCRescuedEvent
    {
        public string npcId;
        public NPCRescuedEvent(string npcId) { this.npcId = npcId; }
    }

    // =====================================================================================
    // Ditambah Langkah 3 (pemain, status tubuh, inventori, input, kamera, equipment).
    // Sumber: tabel "Daftar event global" di GameEventBus.md. Event yang dikirim sistem Langkah lain
    // (HourChangedEvent, WarmthChangedEvent, HeavyItemChangedEvent) ditulis di sini karena sudah didengar
    // sistem Langkah 3. Langkah pemiliknya memakai struct ini dan tidak mendeklarasikannya ulang.
    // =====================================================================================

    // ---------------------------------------------------------------- Waktu (dikirim Langkah 4)

    /// <summary>Jam game berganti. Pengirim: DayNightDirector (Langkah 4). hour = jam dalam hari (0-23).</summary>
    [StickyEvent]
    public struct HourChangedEvent
    {
        public int hour;
        public HourChangedEvent(int hour) { this.hour = hour; }
    }

    // ---------------------------------------------------------------- Pemain (Langkah 3)

    /// <summary>
    /// Pemain menerima damage dari luar. Pengirim: SurvivalStats.TakeDamage (Langkah 3).
    /// Damage kondisi (lapar, haus, frostbite, tenggelam, infeksi) TIDAK mengirim event ini (DamagePipeline.md).
    /// </summary>
    public struct PlayerDamagedEvent
    {
        public DamageInfo info;
        public float remainingHP;
        public PlayerDamagedEvent(in DamageInfo info, float remainingHP) { this.info = info; this.remainingHP = remainingHP; }
    }

    /// <summary>Kombinasi tanda peringatan status berubah. Pengirim: SurvivalStats (VitalsRules.md).</summary>
    [StickyEvent]
    public struct VitalWarningChangedEvent
    {
        public VitalWarningFlags flags;
        public VitalWarningChangedEvent(VitalWarningFlags flags) { this.flags = flags; }
    }

    /// <summary>Kehangatan berubah. Pengirim: WarmthSystem (Langkah 6). Didengar SurvivalStats (isFreezing).</summary>
    [StickyEvent]
    public struct WarmthChangedEvent
    {
        public float warmth;
        public bool isFreezing;
        public WarmthChangedEvent(float warmth, bool isFreezing) { this.warmth = warmth; this.isFreezing = isFreezing; }
    }

    // ---------------------------------------------------------------- Inventori (Langkah 3)

    /// <summary>Isi inventori berubah. Pengirim: InventoryManager. total = jumlah itemId itu setelah berubah.</summary>
    public struct InventoryChangedEvent
    {
        public string itemId;
        public int delta;
        public int total;
        public InventoryChangedEvent(string itemId, int delta, int total) { this.itemId = itemId; this.delta = delta; this.total = total; }
    }

    /// <summary>Item di tangan selesai berganti. itemId kosong = tangan kosong. Pengirim: EquipmentManager.</summary>
    [StickyEvent]
    public struct EquippedItemChangedEvent
    {
        public string itemId;
        public EquippedItemChangedEvent(string itemId) { this.itemId = itemId; }
    }

    /// <summary>Barang berat diangkat atau dilepas. Pengirim: HeavyItemSystem (Langkah 6).</summary>
    [StickyEvent]
    public struct HeavyItemChangedEvent
    {
        public string partId;
        public bool isCarried;
        public HeavyItemChangedEvent(string partId, bool isCarried) { this.partId = partId; this.isCarried = isCarried; }
    }

    /// <summary>Isi satu slot hotbar berubah. itemId kosong = slot kosong. Pengirim: EquipmentManager.</summary>
    public struct HotbarChangedEvent
    {
        public int hotbarIndex;
        public string itemId;
        public HotbarChangedEvent(int hotbarIndex, string itemId) { this.hotbarIndex = hotbarIndex; this.itemId = itemId; }
    }

    /// <summary>Item selesai dipakai. Pengirim: ItemUseSystem.</summary>
    public struct ItemConsumedEvent
    {
        public string itemId;
        public Freshness freshness;
        public ItemConsumedEvent(string itemId, Freshness freshness) { this.itemId = itemId; this.freshness = freshness; }
    }

    /// <summary>Jumlah slot inventori berubah. Pengirim: CarryCapacity.</summary>
    [StickyEvent]
    public struct InventoryCapacityChangedEvent
    {
        public int capacity;
        public InventoryCapacityChangedEvent(int capacity) { this.capacity = capacity; }
    }

    // ShipHandChangedEvent { weight, capacity } (dikirim InventoryManager) DIDEKLARASIKAN Langkah 2 di file ini,
    // supaya tidak ganda saat merge. Branch Langkah 3 saja tidak terkompilasi tanpa branch Langkah 2.

    // ---------------------------------------------------------------- UI (Langkah 3)

    /// <summary>Tombol diganti atau direset. Pengirim: InputManager. Teks tombol di layar membuang cache-nya.</summary>
    public struct InputBindingsChangedEvent { }

    // =====================================================================================
    // Ditambahkan Langkah 4 (siang-malam, infeksi, bangunan, resource, crafting).
    // Sumber: tabel "Daftar event global" di GameEventBus.md. StructureDamagedEvent dan StructureDestroyedEvent
    // dikirim BaseHealth (Langkah 5), tetapi ditulis di sini karena StructureRegistry (Langkah 4) sudah mendengarnya.
    // Langkah 5 memakai struct ini dan tidak mendeklarasikannya ulang.
    // =====================================================================================

    // ---------------------------------------------------------------- Waktu (Langkah 4)

    /// <summary>Malam tiba (18:00). Pengirim: DayNightDirector. GDD bagian 3: malam = horde zombie.</summary>
    [StickyEvent]
    public struct NightfallEvent
    {
        public int day;
        public NightfallEvent(int day) { this.day = day; }
    }

    // ---------------------------------------------------------------- Pemain (Langkah 4)

    /// <summary>Tahap infeksi (1-4) berubah. Pengirim: InfectionSystem (#64).</summary>
    [StickyEvent]
    public struct InfectionStageChangedEvent
    {
        public int stage;
        public InfectionStageChangedEvent(int stage) { this.stage = stage; }
    }

    /// <summary>Vaksin selesai dipakai. Pengirim: InfectionSystem (#64).</summary>
    public struct VaccineUsedEvent
    {
        public float reduction;
        public float infectionAfter;
        public VaccineUsedEvent(float reduction, float infectionAfter) { this.reduction = reduction; this.infectionAfter = infectionAfter; }
    }

    // ---------------------------------------------------------------- Inventori dan crafting (Langkah 4)

    /// <summary>Satu job crafting selesai. Pengirim: CraftingManager (#77). Didengar TechTreeManager.</summary>
    public struct ItemCraftedEvent
    {
        public string recipeId;
        public ItemCraftedEvent(string recipeId) { this.recipeId = recipeId; }
    }

    /// <summary>Isi antrean crafting berubah. activeRecipeId kosong kalau antrean kosong. Pengirim: CraftingManager.</summary>
    public struct CraftQueueChangedEvent
    {
        public int queueCount;
        public string activeRecipeId;
        public CraftQueueChangedEvent(int queueCount, string activeRecipeId) { this.queueCount = queueCount; this.activeRecipeId = activeRecipeId; }
    }

    /// <summary>Node tech tree terbuka. Pengirim: TechTreeManager (#34). Tidak dikirim ulang saat load.</summary>
    public struct TechNodeUnlockedEvent
    {
        public string nodeId;
        public TechNodeUnlockedEvent(string nodeId) { this.nodeId = nodeId; }
    }

    /// <summary>Node resource dipukul. Pengirim: ResourceNode (#73). Didengar AudioManager (#16).</summary>
    public struct ResourceNodeHitEvent
    {
        public NodeType nodeType;
        public bool isDepleted;
        public bool byPlayer;
        public ResourceNodeHitEvent(NodeType nodeType, bool isDepleted, bool byPlayer)
        {
            this.nodeType = nodeType;
            this.isDepleted = isDepleted;
            this.byPlayer = byPlayer;
        }
    }

    // ---------------------------------------------------------------- Base (Langkah 4)

    /// <summary>Struktur selesai dibangun. Pengirim: BuildManager (#79).</summary>
    public struct StructureBuiltEvent
    {
        public string structureId;
        public string buildableId;
        public UnityEngine.Vector3 position;
        public StructureBuiltEvent(string structureId, string buildableId, UnityEngine.Vector3 position)
        {
            this.structureId = structureId;
            this.buildableId = buildableId;
            this.position = position;
        }
    }

    /// <summary>Struktur menerima damage. Pengirim: BaseHealth (Langkah 5, DamagePipeline.md).</summary>
    public struct StructureDamagedEvent
    {
        public string structureId;
        public float hpRatio;
        public UnityEngine.Vector3 position;
        public StructureDamagedEvent(string structureId, float hpRatio, UnityEngine.Vector3 position)
        {
            this.structureId = structureId;
            this.hpRatio = hpRatio;
            this.position = position;
        }
    }

    /// <summary>Struktur hancur atau dibongkar. Pengirim: BaseHealth (Langkah 5). Didengar StructureRegistry (#79).</summary>
    public struct StructureDestroyedEvent
    {
        public string structureId;
        public UnityEngine.Vector3 position;
        public StructureDestroyedEvent(string structureId, UnityEngine.Vector3 position)
        {
            this.structureId = structureId;
            this.position = position;
        }
    }

    /// <summary>Mode palu atau struktur terpilih berubah. buildableId kosong kalau tidak ada pilihan. Pengirim: BuildManager.</summary>
    public struct BuildModeChangedEvent
    {
        public HammerMode mode;
        public string buildableId;
        public BuildModeChangedEvent(HammerMode mode, string buildableId) { this.mode = mode; this.buildableId = buildableId; }
    }

    // =====================================================================================
    // Ditambahkan Langkah 5 (musuh, spawn, damage, gunplay, deteksi).
    // Sumber: tabel "Daftar event global" di GameEventBus.md. RegionChangedEvent dikirim ZoneManager (Langkah 6),
    // tetapi ditulis di sini karena SpawnManager dan AIDirector sudah mendengarnya. Langkah 6 memakai struct ini
    // dan tidak mendeklarasikannya ulang. NightfallEvent milik DayNightDirector (Langkah 4) SENGAJA tidak ditulis di sini;
    // Langkah 5 membaca malam dari HourChangedEvent lewat DayPhaseTracker.
    // =====================================================================================

    // ---------------------------------------------------------------- Pemain (Langkah 5)

    /// <summary>Noise Sphere. Bukan collider, pendengar menghitung jarak sendiri (AISensory.md). Pengirim: NoiseEmitter.</summary>
    public struct NoiseEmittedEvent
    {
        public UnityEngine.Vector3 position;
        public float radius;
        public NoiseSourceKind sourceKind;
        public NoiseEmittedEvent(UnityEngine.Vector3 position, float radius, NoiseSourceKind sourceKind)
        { this.position = position; this.radius = radius; this.sourceKind = sourceKind; }
    }

    /// <summary>Nilai deteksi tertinggi untuk ikon mata. Pengirim: AISensoryHub.</summary>
    [StickyEvent]
    public struct PlayerDetectionChangedEvent
    {
        public float level01;
        public bool isAware;
        public PlayerDetectionChangedEvent(float level01, bool isAware) { this.level01 = level01; this.isAware = isAware; }
    }

    // ---------------------------------------------------------------- Musuh (Langkah 5)

    /// <summary>Musuh keluar dari pool di Fase 2. Pengirim: SpawnManager.</summary>
    public struct EnemySpawnedEvent
    {
        public string enemyId;
        public UnityEngine.Vector3 position;
        public EnemySpawnedEvent(string enemyId, UnityEngine.Vector3 position) { this.enemyId = enemyId; this.position = position; }
    }

    /// <summary>Musuh terkena damage. Pengirim: EnemyHealth. Didengar VFXManager (#15) dan CombatFeedbackUI (#51).</summary>
    public struct EnemyHitEvent
    {
        public UnityEngine.Vector3 position;
        public BodyPart bodyPart;
        public bool isKill;
        public bool byPlayer;
        public EnemyHitEvent(UnityEngine.Vector3 position, BodyPart bodyPart, bool isKill, bool byPlayer)
        { this.position = position; this.bodyPart = bodyPart; this.isKill = isKill; this.byPlayer = byPlayer; }
    }

    /// <summary>Musuh mati (bentuk resmi OnEnemyKilled). Pengirim: EnemyHealth. Didengar AIDirector, statistik (#57).</summary>
    public struct EnemyKilledEvent
    {
        public string enemyId;
        public UnityEngine.Vector3 position;
        public DamageType damageType;
        public bool byPlayer;
        public EnemyKilledEvent(string enemyId, UnityEngine.Vector3 position, DamageType damageType, bool byPlayer)
        { this.enemyId = enemyId; this.position = position; this.damageType = damageType; this.byPlayer = byPlayer; }
    }

    /// <summary>Tanda serangan ke pemain (Windup). Pengirim: EnemyAttack. Didengar CombatFeedbackUI (#51).</summary>
    public struct EnemyAttackTelegraphEvent
    {
        public UnityEngine.Vector3 position;
        public string attackId;
        public EnemyAttackTelegraphEvent(UnityEngine.Vector3 position, string attackId) { this.position = position; this.attackId = attackId; }
    }

    /// <summary>Tahap AIDirector berganti. Pengirim: AIDirector. Didengar CombatMusicDirector (#114).</summary>
    [StickyEvent]
    public struct DirectorPhaseChangedEvent
    {
        public DirectorPhase phase;
        public DirectorPhaseChangedEvent(DirectorPhase phase) { this.phase = phase; }
    }

    // ---------------------------------------------------------------- Senjata (Langkah 5)

    /// <summary>Isi magazine dan cadangan berubah. Pengirim: GunController. Didengar HUDManager.</summary>
    [StickyEvent]
    public struct AmmoChangedEvent
    {
        public int inMagazine;
        public int reserve;
        public AmmoChangedEvent(int inMagazine, int reserve) { this.inMagazine = inMagazine; this.reserve = reserve; }
    }

    /// <summary>Senjata api ditembakkan. Pengirim: GunController. Didengar CameraShake (#24) dan statistik (#57).</summary>
    public struct WeaponFiredEvent
    {
        public string itemId;
        public UnityEngine.Vector3 position;
        public bool byPlayer;
        public WeaponFiredEvent(string itemId, UnityEngine.Vector3 position, bool byPlayer) { this.itemId = itemId; this.position = position; this.byPlayer = byPlayer; }
    }

    /// <summary>Hasil satu tembakan, sekali per tembakan. Pengirim: GunCore. Didengar statistik (#57).</summary>
    public struct ShotResolvedEvent
    {
        public string itemId;
        public bool hitEnemy;
        public bool isHeadshot;
        public bool byPlayer;
        public ShotResolvedEvent(string itemId, bool hitEnemy, bool isHeadshot, bool byPlayer)
        { this.itemId = itemId; this.hitEnemy = hitEnemy; this.isHeadshot = isHeadshot; this.byPlayer = byPlayer; }
    }

    // ---------------------------------------------------------------- Region (dikirim Langkah 6)

    /// <summary>Pemain pindah region. Pengirim: ZoneManager (Langkah 6). Didengar SpawnManager dan AIDirector.</summary>
    [StickyEvent]
    public struct RegionChangedEvent
    {
        public RegionType from;
        public RegionType to;
        public RegionChangedEvent(RegionType from, RegionType to) { this.from = from; this.to = to; }
    }

    // =====================================================================================
    // Ditambahkan Langkah 6 (region, cuaca, gerbang, jembatan).
    // Sumber: tabel "Daftar event global" di GameEventBus.md. RegionChangedEvent (Langkah 5), WarmthChangedEvent dan
    // HeavyItemChangedEvent (Langkah 3) sudah ada di atas dan TIDAK dideklarasikan ulang.
    // =====================================================================================

    /// <summary>Cuaca satu region berubah. Pengirim: RegionalWeather (#115). Last berisi region aktif (diterbitkan paling akhir).</summary>
    [StickyEvent]
    public struct WeatherChangedEvent
    {
        public RegionType region;
        public WeatherType weather;
        public WeatherChangedEvent(RegionType region, WeatherType weather) { this.region = region; this.weather = weather; }
    }

    /// <summary>Region baru bisa dimasuki. Pengirim: RegionGateSystem.Unlock (#68), sekali per region.</summary>
    public struct RegionUnlockedEvent
    {
        public RegionType region;
        public RegionUnlockedEvent(RegionType region) { this.region = region; }
    }

    /// <summary>Progres kayu jembatan berubah. Pengirim: RegionGateSystem (#68). Didengar BridgeSite.</summary>
    [StickyEvent]
    public struct BridgeProgressChangedEvent
    {
        public int deposited;
        public int required;
        public BridgeProgressChangedEvent(int deposited, int required) { this.deposited = deposited; this.required = required; }
    }

    // =====================================================================================
    // Ditambahkan Langkah 7 (bencana, Lucky Machine, helikopter, sinematik).
    // Sumber: tabel "Daftar event global" di GameEventBus.md. BloodMoonStartedEvent dikirim HordeManager (#50, belum
    // ditulis), tetapi ditulis di sini karena DisasterManager sudah mendengarnya. Pemiliknya memakai struct ini
    // dan tidak mendeklarasikannya ulang.
    // =====================================================================================

    // ---------------------------------------------------------------- Bencana (Langkah 7)

    /// <summary>Bencana besar masuk tahap Active. Pengirim: DisasterManager. Didengar LuckyMachine (mode gratis 30 detik).</summary>
    public struct MajorDisasterStartedEvent
    {
        public DisasterType type;
        public float durationSeconds;
        public MajorDisasterStartedEvent(DisasterType type, float durationSeconds) { this.type = type; this.durationSeconds = durationSeconds; }
    }

    /// <summary>Malam Blood Moon dimulai. Pengirim: HordeManager (#50). Didengar DisasterManager (Blood Moon menang).</summary>
    public struct BloodMoonStartedEvent
    {
        public int day;
        public BloodMoonStartedEvent(int day) { this.day = day; }
    }

    // ---------------------------------------------------------------- Lucky Machine (Langkah 7)

    /// <summary>Musuh selesai tersedot kandang. Pengirim: LuckyMachine. Bukan EnemyKilledEvent.</summary>
    public struct EnemySacrificedEvent
    {
        public string enemyId;
        public EnemySacrificedEvent(string enemyId) { this.enemyId = enemyId; }
    }

    /// <summary>Mode gratis mesin mulai atau selesai. Pengirim: LuckyMachine. Didengar HUDManager (hitung mundur).</summary>
    [StickyEvent]
    public struct LuckyMachineFreeWindowChangedEvent
    {
        public RegionType region;
        public bool isActive;
        public float seconds;
        public LuckyMachineFreeWindowChangedEvent(RegionType region, bool isActive, float seconds)
        { this.region = region; this.isActive = isActive; this.seconds = seconds; }
    }

    /// <summary>Hasil satu putaran. Pengirim: LuckyMachine. Didengar statistik (#57).</summary>
    public struct LuckyMachineSpinResolvedEvent
    {
        public string rewardId;
        public LuckyMachineSpinResolvedEvent(string rewardId) { this.rewardId = rewardId; }
    }

    // ---------------------------------------------------------------- Helikopter (Langkah 7)

    /// <summary>Suku cadang terpasang. Pengirim: RescueEventManager. Didengar HUD, HelicopterFrame, ObjectiveManager.</summary>
    [StickyEvent]
    public struct HelicopterPartInstalledEvent
    {
        public string partId;
        public int installed;
        public int total;
        public HelicopterPartInstalledEvent(string partId, int installed, int total)
        { this.partId = partId; this.installed = installed; this.total = total; }
    }

    /// <summary>Semua suku cadang terpasang. Pengirim: RescueEventManager.</summary>
    [StickyEvent]
    public struct HelicopterReadyEvent { }

    /// <summary>Status mesin helikopter. Pengirim: RescueEventManager. Didengar HUD, CombatMusicDirector (#114), CameraShake (#24).</summary>
    [StickyEvent]
    public struct HelicopterEngineChangedEvent
    {
        public RescueState state;
        public float secondsLeft;
        public HelicopterEngineChangedEvent(RescueState state, float secondsLeft) { this.state = state; this.secondsLeft = secondsLeft; }
    }

    // ---------------------------------------------------------------- UI (Langkah 7)

    /// <summary>Sinematik mulai atau selesai. Pengirim: DeathCinematic, NarrativeSequencer (#99).</summary>
    [StickyEvent]
    public struct CinematicStateChangedEvent
    {
        public bool isPlaying;
        public bool keepMusic;
        public CinematicStateChangedEvent(bool isPlaying, bool keepMusic) { this.isPlaying = isPlaying; this.keepMusic = keepMusic; }
    }

    // =====================================================================================
    // Ditambahkan: sistem pendukung (FondasiA). Event dari tabel GameEventBus.md.
    // =====================================================================================

    // =====================================================================================
    // Event grup FondasiA (sistem pendukung). Sumber: tabel "Daftar event global" di Core_Logic/GameEventBus.md (#107).
    // Hanya event yang DIKIRIM sistem grup ini, ditambah SubtitleRequestedEvent yang pendengarnya milik #104
    // (pengirimnya, NarrativeSequencer, DisasterManager, dan NPCBarks, memakai struct ini dan tidak mendeklarasikannya ulang).
    // =====================================================================================

    /// <summary>Bagian setting yang berubah. Graphics = setelah ApplyGraphics, All = setelah Initialize atau Commit.</summary>
    public enum SettingsCategory { All, Graphics }

    /// <summary>Setting pemain berubah. Pengirim: SettingsManager (#19). Didengar VFXManager (#15).</summary>
    [StickyEvent]
    public struct SettingsChangedEvent
    {
        public SettingsCategory category;
        public SettingsChangedEvent(SettingsCategory category) { this.category = category; }
    }

    /// <summary>Pilihan aksesibilitas berubah. Pengirim: LocalizationManager (#104). Didengar LocalizedText dan subtitle.</summary>
    [StickyEvent]
    public struct AccessibilityChangedEvent
    {
        public AccessibilityOptions options;
        public AccessibilityChangedEvent(in AccessibilityOptions options) { this.options = options; }
    }

    /// <summary>Asal subtitle (LocalizationManager.md, tabel sumber subtitle).</summary>
    public enum SubtitleSourceKind { Scene, Warning, Bark }

    /// <summary>
    /// Minta satu baris subtitle. Pengirim: NarrativeSequencer (#99, prioritas 3), DisasterManager (#70, prioritas 2),
    /// NPCBarks (#100, prioritas 1). Didengar SubtitlePresenter.
    /// </summary>
    public struct SubtitleRequestedEvent
    {
        public string textKey;
        public string speakerKey;   // Kosong berarti tanpa nama
        public int priority;
        public SubtitleSourceKind sourceKind;
        public SubtitleRequestedEvent(string textKey, string speakerKey, int priority, SubtitleSourceKind sourceKind)
        { this.textKey = textKey; this.speakerKey = speakerKey; this.priority = priority; this.sourceKind = sourceKind; }
    }

    // =====================================================================================
    // Ditambahkan: sistem pendukung (FondasiB). Event dari tabel GameEventBus.md.
    // =====================================================================================

    // =====================================================================================
    // Event milik sistem pendukung grup FondasiB (StatusEffectManager #20, DurabilitySystem #113).
    // Sumber: tabel "Daftar event global" di Core_Logic/GameEventBus.md. Nama field mengikuti kolom "Isi".
    // =====================================================================================

    /// <summary>
    /// Efek status dipasang, bertumpuk, atau dilepas. Pengirim: StatusEffectManager (#20). Didengar HUDManager (StatusEffectBar).
    /// Tidak sticky: HUD membaca StatusEffectManager.Active saat aktif.
    /// </summary>
    public struct StatusEffectChangedEvent
    {
        public string effectId;
        public bool isActive;
        public int stacks;
        public StatusEffectChangedEvent(string effectId, bool isActive, int stacks)
        { this.effectId = effectId; this.isActive = isActive; this.stacks = stacks; }
    }

    /// <summary>
    /// Status durabilitas satu item berubah (Normal, Low, Broken, Destroyed). Pengirim: DurabilitySystem dan ToolRepairSystem (#113).
    /// Didengar HUDManager (bar hotbar) dan NotificationManager (#22). Tidak sticky.
    /// </summary>
    public struct DurabilityStateChangedEvent
    {
        public int instanceId;
        public string itemId;
        public DurabilityState state;
        public DurabilityStateChangedEvent(int instanceId, string itemId, DurabilityState state)
        { this.instanceId = instanceId; this.itemId = itemId; this.state = state; }
    }

    // =====================================================================================
    // Ditambahkan: sistem pendukung (GrupC2). Event dari tabel GameEventBus.md.
    // =====================================================================================

    // =====================================================================================
    // Event grup C2 (sistem pendukung). Sumber: tabel "Daftar event global" di Core_Logic/GameEventBus.md (#107).
    // Hanya event yang DIKIRIM sistem grup ini: MapMarkerRegistry (milik CompassSystem, #30) dan NarrativeSequencer (#99).
    // CinematicStateChangedEvent dan SubtitleRequestedEvent sudah ada (GameEvents.cs, GameEvents.FondasiA.cs).
    // =====================================================================================

    /// <summary>Isi registry penanda berubah. Pengirim: MapMarkerRegistry (milik CompassSystem, #30). Tidak sticky.</summary>
    public struct MapMarkerChangedEvent
    {
        public string markerId;
        public bool isRemoved;
        public MapMarkerChangedEvent(string markerId, bool isRemoved) { this.markerId = markerId; this.isRemoved = isRemoved; }
    }

    /// <summary>Adegan cerita mulai atau selesai. Pengirim: NarrativeSequencer (#99). Didengar CombatMusicDirector (#114).</summary>
    [StickyEvent]
    public struct NarrativeStateChangedEvent
    {
        public string sceneId;
        public bool isPlaying;
        public NarrativeStateChangedEvent(string sceneId, bool isPlaying) { this.sceneId = sceneId; this.isPlaying = isPlaying; }
    }

    // =====================================================================================
    // Ditambahkan: sistem pendukung (GrupD). Event dari tabel GameEventBus.md.
    // =====================================================================================

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

    // =====================================================================================
    // Ditambahkan: sistem pendukung (GrupE). Event dari tabel GameEventBus.md.
    // =====================================================================================

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

    // =====================================================================================
    // Ditambahkan: sistem pendukung (GrupF). Event dari tabel GameEventBus.md.
    // =====================================================================================

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

    // =====================================================================================
    // Ditambahkan: sistem pendukung (GrupG). Event dari tabel GameEventBus.md.
    // =====================================================================================

    // =====================================================================================
    // Event sistem pendukung Grup G (raft: Hydration, Disease, Campfire, Fishing, Debris, Storage).
    // Sumber: tabel "Daftar event global" di Core_Logic/GameEventBus.md. Nama dan field mengikuti tabel itu.
    // StatusEffectChangedEvent (#20) dan TideChangedEvent (#91) milik sistem lain dan TIDAK dideklarasikan di sini.
    // =====================================================================================

    /// <summary>Penyakit sekunder mulai atau berakhir. Pengirim: DiseaseManager (#45). Tidak sticky.</summary>
    public struct DiseaseChangedEvent
    {
        public DiseaseType type;
        public bool isActive;
        public DiseaseChangedEvent(DiseaseType type, bool isActive) { this.type = type; this.isActive = isActive; }
    }

    /// <summary>State Kail berganti. endReason hanya bermakna saat state Recovering. Pengirim: FishingSystem (#55).</summary>
    public struct FishingStateChangedEvent
    {
        public FishingState state;
        public FishingEndReason endReason;
        public FishingStateChangedEvent(FishingState state, FishingEndReason endReason) { this.state = state; this.endReason = endReason; }
    }

    /// <summary>Satu baris masak matang (isBurnt false) atau gosong (isBurnt true). Pengirim: CampfireManager (#44).</summary>
    public struct ItemCookedEvent
    {
        public string recipeId;
        public bool isBurnt;
        public ItemCookedEvent(string recipeId, bool isBurnt) { this.recipeId = recipeId; this.isBurnt = isBurnt; }
    }

    /// <summary>Ikan tertangkap. Pengirim: FishingSystem (#55).</summary>
    public struct FishCaughtEvent
    {
        public string itemId;
        public FishCaughtEvent(string itemId) { this.itemId = itemId; }
    }

    /// <summary>Barang hanyut masuk inventori. Pengirim: DebrisRetrieval (#106).</summary>
    public struct DebrisCollectedEvent
    {
        public string itemId;
        public int amount;
        public DebrisCollectedEvent(string itemId, int amount) { this.itemId = itemId; this.amount = amount; }
    }
}
