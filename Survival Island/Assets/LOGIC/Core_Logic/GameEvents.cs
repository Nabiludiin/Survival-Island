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
}
