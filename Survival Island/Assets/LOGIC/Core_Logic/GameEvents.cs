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
}
