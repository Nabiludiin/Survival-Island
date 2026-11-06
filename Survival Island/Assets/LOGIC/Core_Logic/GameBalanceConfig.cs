using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Satu sumber angka penyetelan (ScriptableObject).
    /// Blueprint: Core_Logic/GameBalanceConfig.md (#84). Claude.md Tahap 6 poin 8.
    ///
    /// - Sistem membaca angka lewat GameManager.Instance.Balance.&lt;bagian&gt;.&lt;nama&gt;, bukan menulis angka sendiri.
    /// - GameManager memakai SALINAN aset (Instantiate) per run, jadi DevConsole bisa mengubah angka tanpa mengubah aset asli.
    /// - Kelas tiap bagian ada di GameBalanceSections.cs. Bagian yang belum dipakai Langkah 1 masih kosong (TODO);
    ///   Langkah pemiliknya mengisi field-nya sesuai tabel "Bagian config per sistem" di blueprint.
    /// </summary>
    [CreateAssetMenu(menuName = "Survival/Game Balance Config", fileName = "GameBalanceConfig")]
    public partial class GameBalanceConfig : ScriptableObject
    {
        // ---------------------------------------------------------------- Angka inti (GameBalanceConfig.md bagian 2)
        public TimeBalance time = new TimeBalance();
        public VitalsBalance vitals = new VitalsBalance();
        public ResourceBalance resource = new ResourceBalance();
        public RegionBalance region = new RegionBalance();
        public WeatherBalance weather = new WeatherBalance();
        public DifficultyProfile[] difficulties = { new DifficultyProfile() };

        // ---------------------------------------------------------------- Bagian milik sistem lain
        public RunBalance run = new RunBalance();
        public UIBalance ui = new UIBalance();
        public InputBalance input = new InputBalance();
        public StreamingBalance streaming = new StreamingBalance();
        public RunSeedBalance runSeed = new RunSeedBalance();
        public DamageBalance damage = new DamageBalance();
        public EquipmentBalance equipment = new EquipmentBalance();
        public ItemsBalance items = new ItemsBalance();
        public InteractBalance interact = new InteractBalance();
        public CameraBalance camera = new CameraBalance();
        public TraversalBalance traversal = new TraversalBalance();
        public CarryBalance carry = new CarryBalance();
        public ConsoleBalance console = new ConsoleBalance();
        public ShipBalance ship = new ShipBalance();
        public ShipHazardBalance shipHazard = new ShipHazardBalance();
        public InfectionBalance infection = new InfectionBalance();
        public HydrationBalance hydration = new HydrationBalance();
        public CampfireBalance campfire = new CampfireBalance();
        public DiseaseBalance disease = new DiseaseBalance();
        public StatusEffectsBalance statusEffects = new StatusEffectsBalance();
        public CraftingBalance crafting = new CraftingBalance();
        public RepairBalance repair = new RepairBalance();
        public UpgradeBalance upgrade = new UpgradeBalance();
        public FishingBalance fishing = new FishingBalance();
        public DebrisBalance debris = new DebrisBalance();
        public SurfaceBalance surface = new SurfaceBalance();
        public BuildBalance build = new BuildBalance();
        public BaseNavBalance baseNav = new BaseNavBalance();
        public IntegrityBalance integrity = new IntegrityBalance();
        public StorageBalance storage = new StorageBalance();
        public SleepBalance sleep = new SleepBalance();
        public SaveBalance save = new SaveBalance();
        public GunBalance gun = new GunBalance();
        public EnemyAttackBalance enemyAttack = new EnemyAttackBalance();
        public HitReactionBalance hitReaction = new HitReactionBalance();
        public ThreatBalance threat = new ThreatBalance();
        public StealthBalance stealth = new StealthBalance();
        public WildlifeBalance wildlife = new WildlifeBalance();
        public SpawnBalance spawn = new SpawnBalance();
        public SunlightBalance sunlight = new SunlightBalance();
        public SpecialZombieBalance specialZombie = new SpecialZombieBalance();
        public AIDirectorBalance aiDirector = new AIDirectorBalance();
        public CrowdBalance crowd = new CrowdBalance();
        public PinnedBalance pinned = new PinnedBalance();
        public TrapBalance trap = new TrapBalance();
        public ThrowableBalance throwable = new ThrowableBalance();
        public LairBalance lair = new LairBalance();
        public WarmthBalance warmth = new WarmthBalance();
        public HeavyItemBalance heavyItem = new HeavyItemBalance();
        public SwimmingBalance swimming = new SwimmingBalance();
        public TideBalance tide = new TideBalance();
        public EnvironmentBalance environment = new EnvironmentBalance();
        public DisasterBalance disaster = new DisasterBalance();
        public BeachSupplyBalance beachSupply = new BeachSupplyBalance();
        public BloodMoonBalance bloodMoon = new BloodMoonBalance();
        public LuckyMachineBalance luckyMachine = new LuckyMachineBalance();
        public NpcBalance npc = new NpcBalance();
        public ObjectiveBalance objective = new ObjectiveBalance();
        public LootBalance loot = new LootBalance();
        public RemoteBaseBalance remoteBase = new RemoteBaseBalance();
        public FireBalance fire = new FireBalance();
        public LightBalance light = new LightBalance();
        public FlashlightBalance flashlight = new FlashlightBalance();
        public RescueBalance rescue = new RescueBalance();
        public DeathCinematicBalance deathCinematic = new DeathCinematicBalance();
        public StatsBalance stats = new StatsBalance();
        public NarrativeBalance narrative = new NarrativeBalance();
        public SafetyBalance safety = new SafetyBalance();
        public AnimationBalance animation = new AnimationBalance();
        public VfxBalance vfx = new VfxBalance();
        public TutorialBalance tutorial = new TutorialBalance();
        public SettingsBalance settings = new SettingsBalance();
        public FootstepBalance footstep = new FootstepBalance();
        public NotificationBalance notification = new NotificationBalance();
        public PostProcessBalance postProcess = new PostProcessBalance();
        public ShakeBalance shake = new ShakeBalance();
        public CombatFeedbackBalance combatFeedback = new CombatFeedbackBalance();
        public AmbientAudioBalance ambientAudio = new AmbientAudioBalance();
        public CombatMusicBalance combatMusic = new CombatMusicBalance();
        public LoreBalance lore = new LoreBalance();
        public BarksBalance barks = new BarksBalance();
        public LocalizationBalance localization = new LocalizationBalance();
        public OptimizationBalance optimization = new OptimizationBalance();
        public CompassBalance compass = new CompassBalance();
        public HazardBalance hazard = new HazardBalance();
    }
}
