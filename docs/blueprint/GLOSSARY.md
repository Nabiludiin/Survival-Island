# Glosarium Nama Resmi

**Lokasi:** `docs/blueprint/GLOSSARY.md`
**Issue:** #85
**Fungsi:** Satu sumber nama scene, manager, enum, tag, interface, dan file blueprint. Kalau blueprint lain memakai nama berbeda, ikuti glosarium ini.

Aturan game tetap mengikuti `GDD.md`. Glosarium hanya mengatur nama dan pemilik data.

---

## 1. Keputusan dari issue #85

Developer sudah menyetujui keputusan berikut.

| Topik | Keputusan |
|---|---|
| Item awal | `Base_Kail`, `Base_Pistol`, dan `Base_Pisau` |
| Status haus | Ada. `Thirst` menjadi status inti bersama `Hunger` dan `Infection` |
| Frostbite | `Stamina` terkuras dulu. Setelah habis, `HP` yang terkuras |
| Syarat jembatan | Nilai awal 500 kayu, dan bisa disetel lewat `GameBalanceConfig` |
| Syarat menang | Helikopter lepas landas. Hari ke-60 adalah target, dan melewatinya membuat zombie semakin kuat |
| Headshot | 2x damage |
| Tombol interaksi | `E` di Fase 1 dan Fase 2. Klik kiri dipakai item aktif |
| Referensi combat | Left 4 Dead (L4D) saja |
| Zombie setelah hari ke-60 | Per hari: jumlah +2 persen, HP +2 persen, dan damage +1 persen. Batasnya 1,5x, 2x, dan 1,5x |
| Infeksi mencapai 100 | HP turun bertahap, tidak mati seketika |
| Diary | Satu buku di shelter tua, dan bab terbuka bertahap |
| Lucky Machine | Satu mesin di setiap region. Kuota per region dan hadiah bersama menunggu konfirmasi developer |
| Fitur tambahan yang dipakai | #96, #106, dan #112 |
| Fitur yang tidak dipakai | #48, #105, dan #117. Blueprint-nya disimpan sebagai arsip |
| Angka inti dan usulan kecil | Disetujui sebagai nilai awal, dan boleh berubah saat playtest |

---

## 2. Nama yang diganti

| Nama lama | Nama resmi | Catatan |
|---|---|---|
| `IslandSurvivalPhase` | `IslandPhase` | Nama scene pulau |
| `TimeManager` | `DayNightDirector` | Jam dunia dan siklus siang malam |
| `PlayerStats` | `SurvivalStats` | Pemilik semua status pemain |
| `PlayerInventory`, `InventoryController` | `InventoryManager` | Satu manager inventori untuk dua fase |
| `CrossPhaseInventory` | `GameManager.savedShipLoot` | Data barang dari Fase 1 |
| `DayNightDirector.currentDay` | `GameManager.currentDay` | `DayNightDirector` hanya memanggil `GameManager.AdvanceDay()` |
| `AIState.AttackBase` | `AIState.AttackBuilding` | Mengikuti `Claude.md` |
| `DayNightSpawner` | `SpawnManager` | Issue #62 |
| Left 4 Dead 2, L4D2 | Left 4 Dead, L4D | Referensi combat |

---

## 3. Scene

| Scene | Isi | Pemilik alur |
|---|---|---|
| `Bootstrap` | Memuat manager persisten | `RunLifecycle` (#82) |
| `MainMenu` | Menu utama | `RunLifecycle` (#82) |
| `ShipPhase` | Fase 1 | `ShipTimerManager` |
| `IslandPhase` | Scene dasar Fase 2 | `RegionStreaming` (#88) |
| `Region1_Volcano`, `Region2_Amazon`, `Region3_Snow`, `Region4_Village` | Scene additive tiap region. Usulan, menunggu #88 | `RegionStreaming` (#88) |

---

## 4. Enum bersama

| Enum | Nilai | Pemilik |
|---|---|---|
| `GamePhase` | `SinkingShip`, `IslandSurvival` | `GameManager` |
| `RunResult` | `Won`, `Died` | `GameManager` |
| `RegionType` | `Volcano`, `Amazon`, `Snow`, `Logistic` | `ZoneManager` |
| `ItemType` | `Material`, `Consumable`, `Weapon`, `Tool`, `KeyItem` | `ItemData` |
| `ShipInteractableKind` | `Item`, `NPC` | `ShipInteractable` |
| `DamageType` | `Bullet`, `Melee`, `Fire`, `Explosion`, `Frost`, `Starvation`, `Dehydration`, `Infection`, `Structural`, `Fall`, `Drown`, `Poison` | `DamagePipeline` (#59) |
| `DeathReason` | `None` (nilai pertama, dipakai `RunEndedEvent` saat menang, usulan #82), `Starvation`, `Dehydration`, `Zombified`, `KilledByEnemy`, `Frostbite`, `Drowned`, `Fall`, `Fire`, `Explosion`, `Poison`, `LeftOnShip` | `DamagePipeline` (#59) |
| `BodyPart` | `Head`, `Torso`, `Leg` | `HitboxModifier` |
| `SurfaceType` | `Pasir`, `Rumput`, `Tanah`, `Batu`, `Salju`, `Lumpur`, `Kayu`, `Logam` | `SurfaceDetection` (#86) |
| `WeatherType` | `Clear`, `Rain`, `Fog`, `Storm`, `Snow`, `Blizzard` | `WeatherManager`, ditambah #115 |
| `DisasterType` | `Eruption`, `Earthquake`, `TropicalStorm` | `DisasterManager` (#70) |
| `AIState` | `Idle`, `Chase`, `AttackPlayer`, `AttackBuilding`, `BurnToAshes`, `Stagger`, `Knockback`, `SeekHiding`, `Submerged`, `Emerging`, `Dormant`, `Dying` (#95) | `EnemyAI` |
| `AnimalState` | `Wander`, `Alert`, `Chase`, `Attack`, `ReturnToTerritory`, `FleeToDen`, `TargetUnreachable` | `WildlifeAI` (#61) |
| `NPCState` | `Follow`, `Attack`, `Idle`, `StayAtBase`, `Work`, `TakeCover` | `NPCCompanion`, ditambah #76 |
| `RunState` | `Booting`, `InMenu`, `StartingRun`, `InShip`, `OnIsland`, `Ending`, `AwaitingChoice`, `Resetting` | `RunLifecycle` (#82) |
| `UIPanelId` | Jenis panel UI | `UIFocusStack` (#83) |
| `UICloseReason` | `User`, `Forced`, `RunEnded`, `SceneUnloaded` | `UIFocusStack` (#83) |
| `GameInputAction` | Nama action untuk ganti tombol dan teks tombol | `InputManager` (#18) |
| `RegionLoadState` | `Unloaded`, `Loading`, `Loaded`, `Unloading` | `RegionStreaming` (#88) |
| `RandomStreamId` | Aliran acak per sistem | `RunSeedManager` (#89) |
| `ActionPhase` | `Pressed`, `Held`, `Released` | `EquipmentManager` (#60) |
| `HandUse` | `OneHand`, `TwoHand`, `OffHand` | `EquipmentManager` (#60) |
| `EquipLock` | Flag kunci pergantian item | `EquipmentManager` (#60) |
| `EquipState` | `Idle`, `Lowering`, `Raising` | `EquipmentManager` (#60) |
| `Freshness` | `Fresh`, `Stale`, `Rotten` | `ItemUseSystem` (#66) |
| `InteractBlockReason` | Alasan interaksi ditolak | `IslandInteractable` (#17) |
| `TraversalMode` | Mode gerak pemain | `PlayerTraversal` (#101) |
| `SpeedModifierId` | Sumber pengali kecepatan pemain | `PlayerTraversal` (#101) |
| `StaminaUse` | `Sprint`, `Jump`, `Melee`, `Swim`, `Climb`, `Struggle` (#110) | `VitalsRules` (#111) |
| `VitalPenaltySource` | Sumber penalti stamina | `VitalsRules` (#111) |
| `VitalWarningFlags` | Flag peringatan status pemain | `VitalsRules` (#111) |
| `ViewmodelMode` | `Visible`, `Lowered`, `Hidden` | `FirstPersonCamera` (#102) |
| `ShipFlowState` | Alur Fase 1 | `LifeboatExtraction` (#74) |
| `ShipControlLevel` | `None`, `LookOnly`, `Full` | `ShipPlayerController` (#74) |
| `ShipHazardKind` | Jenis kejadian bahaya | `ShipHazardManager` (#75) |
| `DiseaseType` | `FoodPoisoning`, `Venom`, `Bleeding`, `WoundInfection` | `DiseaseManager` (#45) |
| `DiseaseCure` | Flags untuk `activeMask` dan `ItemData.curesDiseases`: `None`, `FoodPoisoning`, `Venom`, `Bleeding`, `WoundInfection` | `DiseaseManager` (#45) |
| `CookRowState` | `Empty`, `Cooking`, `Done`, `Burnt` | `CampfireManager` (#44) |
| `EffectDurationUnit` | `Seconds`, `GameHours`, `Permanent` | `StatusEffectManager` (#20) |
| `EffectStackRule` | `Refresh`, `Extend`, `Intensify`, `Reject` | `StatusEffectManager` (#20) |
| `CraftingStationType` | `Hand`, `WoodBench`, `MedicalBench` (#64), `Campfire` (#44) | `CraftingManager` (#77) |
| `CraftResult` | Hasil `RequestCraft` | `CraftingManager` (#77) |
| `ToolClass` | `None`, `Axe`, `Pickaxe`, `Knife` | `ResourceRespawn` (#73) |
| `NodeType` | `Tree`, `Rock`, `MedicalHerb` | `ResourceNode` |
| `NodeStatus` | `Standing`, `Depleted` | `ResourceRespawn` (#73) |
| `HitResult` | `Hit`, `Rejected` | `ResourceRespawn` (#73) |
| `DurabilityState` | `NoDurability`, `Normal`, `Low`, `Broken`, `Destroyed` | `DurabilitySystem` (#113) |
| `UpgradeKind` | `Damage`, `MagazineSize`, `Speed`, `Durability` | `ToolRepairSystem` (#113) |
| `FishingState` | `Idle`, `Charging`, `LureFlying`, `Waiting`, `BiteWindow`, `Reeling`, `Intercepted`, `Recovering` | `FishingSystem` (#55) |
| `FishingEndReason` | `Caught`, `FishEscaped`, `LineBroken`, `Cancelled`, `Interrupted`, `WaterGone`, `TooFar`, `InterceptedByTarget` | `FishingSystem` (#55) |
| `BuildCategory` | `Structure`, `Furniture`, `Defense` | `BuildMenu` (#79) |
| `BuildSlotKind` | `Cell`, `Edge`, `WallMount` (#98) | `BuildMenu` (#79) |
| `SupportRule` | `GroundOrWall`, `FloorOrGround`, `WallBelow` | `BuildMenu` (#79) |
| `NavObstacleMode` | `None`, `Carve`, `Avoid` | `BuildMenu` (#79) |
| `HammerMode` | `Repair`, `Build`, `Upgrade`, `Demolish` | `BuildMenu` (#79) |
| `PlacementError` | Alasan penempatan ditolak | `BuildMenu` (#79) |
| `DoorState` | `Closed`, `Open`, `Locked` | `BaseNavigation` (#78) |
| `StructureTier` | `Wood`, `Stone` | `BuildingIntegrity` (#47) |
| `SlotSide` | `Left`, `Right` | `StorageChest` (#46) |
| `SleepBlockReason` | Alasan tidur ditolak | `SleepSystem` (#67) |
| `SleepWakeReason` | Alasan bangun, termasuk `None` saat mulai tidur | `SleepSystem` (#67) |
| `SaveReason` | `ArrivedOnIsland`, `WakeUp`, `Daybreak`, `QuitToMenu`, `AppQuit`, `Manual`, `BridgeComplete` (#68), `DisasterEnded` (#70), `PartInstalled` (#14) | `WorldStateSave` (#80) |
| `SaveProblem` | `None`, `Corrupt`, `NewerVersion`, `OldVersionNoMigration` | `WorldStateSave` (#80) |
| `WeaponFireMode` | `SemiAuto`, `Auto` | `GunController` (#58) |
| `AttackPhase` | `None`, `Windup`, `Active`, `Recovery` | `EnemyAttack` (#109) |
| `SpeedSource` | Sumber pengali kecepatan `EnemyAI`, termasuk `LegHit` (#95) dan `Trap` (#26) | `EnemyAI` |
| `TargetKind` | `Player`, `NPC`, `BaseAnchor`, `Lure` | `ThreatSystem` (#108) |
| `AwarenessLevel` | `Unaware`, `Suspicious`, `Aware` | `AISensory` (#49) |
| `NoiseSourceKind` | `Movement`, `Landing`, `Gunshot`, `Melee`, `Tool`, `Container`, `Decoy`, `Structure`, `Throwable` (#96) | `AISensory` (#49) |
| `AnimalAttackKind` | `Bite`, `Pounce`, `VenomBite`, `Charge`, `FlockDive` | `WildlifeAI` (#61) |
| `DespawnReason` | Alasan kembali ke pool | `SpawnManager` (#62) |
| `HidingKind` | `Water`, `Mud` | `SunlightExposure` (#63) |
| `SpecialZombieKind` | Perilaku varian zombie di `ZombieData` | `SpecialZombieAI` (#25) |
| `DirectorPhase` | `Inactive`, `BuildUp`, `SustainPeak`, `PeakFade`, `Relax` | `AIDirector` (#27) |
| `CrowdTier` | `Close`, `Mid`, `Far`, `Culled` | `HordeCrowdManager` (#93) |
| `ReachResponse` | `None`, `Surround`, `Retreat`, `RockThrow`, `AttackStructure` | `AITraversal` (#94) |
| `PinReleaseReason` | `Escaped`, `EscapedWithKnife`, `AttackerDamaged`, `AttackerGone`, `TimedOut`, `PlayerDied` | `PlayerPinnedState` (#110) |
| `TrapKind` | `Spike`, `BarbedWire`, `Campfire` | `BaseDefense` (#26) |
| `ThrowEffect` | `Fire`, `Explosion`, `NoiseOnly`, `Lure` | `ThrowableSystem` (#96) |
| `LairKind` | `Cave`, `MilitaryBunker`, `VillageWarehouse`, `RuinedHouse` | `ZombieLair` (#112) |
| `GateKind` | `Bridge`, `DiaryChapter`, `FollowsPrevious` | `RegionGateSystem` (#68) |
| `PartState` | `Lying`, `Carried`, `Installed` | `HeavyItemSystem` (#69) |
| `DropReason` | `Manual`, `HeavyHit`, `Pinned`, `Water`, `Death`, `Installed`, `Unstuck` (#116) | `HeavyItemSystem` (#69) |
| `WaterKind` | `Sea`, `River`, `Lake` | `SwimmingController` (#13) |
| `WaterMode` | `None`, `Wading`, `Swimming` | `SwimmingController` (#13) |
| `EnvEventId` | Lapisan event `EnvironmentDirector`, misalnya `Eruption` dan `BloodMoon` | `EnvironmentDirector` (#92) |
| `DisasterPhase` | `Idle`, `Warning`, `Active`, `Aftermath` | `DisasterManager` (#70) |
| `MarkerIcon` | `Supply`, `Part`, `Helipad`, `Objective` | `CompassSystem` (#30, #71) |
| `SupplyDropState` | `Idle`, `Scheduled`, `PlaneInbound`, `Descending`, `Landed` | `SupplyDropManager` (#54) |
| `LuckyRewardKind` | `WeaponModifier`, `Skin`, `PermanentBuff`, `SpawnItem` | `LuckyMachine` (#72) |
| `LuckyTier` | `B`, `A`, `S` | `LuckyMachine` (#72) |
| `CageGateState` | `Open`, `Closed` | `LuckyMachine` (#72) |
| `NPCSkill` | `None`, `Medic`, `Crafter`, `Cook`. `Medic` dibaca lewat `NPCRoster.GetSkillMultiplier` | `NPCRoster` (#76) |
| `NPCCommand` | `Follow`, `Wait`, `ReturnToBase` | `NPCRoster` (#76) |
| `ObjectiveCondition` | `BuildCount`, `ItemTotal`, `BridgeBuilt`, `ItemCrafted`, `RegionEntered`, `PartsInstalled`, `Escaped` | `ObjectiveManager` (#81) |
| `DiaryRewardKind` | `None`, `GiveItem`, `RevealObjectives` | `ObjectiveManager` (#81) |
| `LootLocation` | `None`, `Ship`, `Volcano`, `Amazon`, `Snow`, `Logistic` | `RegionLootTable` (#103) |
| `LootPointKind` | Flags `House`, `Chest`, `SoldierBody`, `Warehouse`, `Any` | `RegionLootTable` (#103) |
| `RescueState` | `Assembling`, `Ready`, `WarmingUp`, `ReadyToLift`, `Departed` | `RescueEventManager` (#14) |
| `DeathStyle` | `Ragdoll`, `Collapse`, `Sink`, `Zombify`, `Blackout` | `DeathCinematic` (#53) |
| `SafetySeverity` | `Ok`, `Fixed`, `Warning`, `Blocked` | `ProgressionSafety` (#116) |
| `UnstuckResult` | Hasil permintaan keluar dari tersangkut | `ProgressionSafety` (#116) |
| `ShadowQuality` | `Off`, `Low`, `Medium`, `High` | `SettingsManager` (#19) |
| `TextureQuality` | `Quarter`, `Half`, `Full` | `SettingsManager` (#19) |
| `QualityPreset` | `Low`, `Medium`, `High`, `Custom` | `SettingsManager` (#19) |
| `AudioChannel` | `Master`, `BGM`, `SFX` | `SettingsManager` (#19) |
| `NotificationKind` | `Info`, `Gain`, `Warning`, `Danger`, `Objective` | `NotificationManager` (#22) |
| `CameraShakeKind` | Jenis getaran kamera | `CameraShake` (#24) |
| `ShakeChannel` | `Aim`, `Visual` | `CameraShake` (#24) |
| `MusicTier` | `Calm`, `Alert`, `Combat`, `Peak` | `CombatMusicDirector` (#114) |
| `ThreatCueKind` | `Idle`, `Alert` | `CombatMusicDirector` (#114) |
| `LoreKind` | `Note`, `Recording`, `Board` | `LoreCollectibles` (#100) |
| `LoreSource` | `Researcher`, `Military`, `Experiment` | `LoreCollectibles` (#100) |
| `LoreHintKind` | `None`, `PartLocation` | `LoreCollectibles` (#100) |
| `BarkCategory` | Kategori ucapan NPC | `NPCBarks` (#100) |

---

## 5. Interface dan data bersama

| Nama | Fungsi | Pemilik |
|---|---|---|
| `IDamageable`, `DamageInfo` | Jalur damage tunggal | #59 |
| `EnemyHealth`, `BaseHealth` | HP musuh dan struktur | #59 |
| `IInteractable` | Interaksi tombol `E`, dipakai `ShipInteractable` dan `IslandInteractable` | #17 |
| `IEquippable` | Item yang bisa dipegang | #60 |
| `ITargetable` | Target musuh | #108 |
| `ISaveable` | Pendaftaran data save | #80 |
| `ItemInstance` | Data per item di slot: durabilitas, isi magazine, umur busuk | #66 |
| `WorldItem` | Item yang tergeletak di dunia | #66 |
| `EnemyData` | Data dasar musuh. `ZombieData` (#25) dan `AnimalData` (#61) mewarisinya | #25 |
| `AttackData` | Data serangan musuh | #109 |
| `WeaponData` | Data senjata api | #58 |
| `ThrowableData` | Data barang lempar | #96 |
| `CraftingRecipe` | Data resep | #77 |
| `BuildableData` | Data struktur | #79 |
| `HeavyItemData` | Data suku cadang | #69 |
| `NPCData` | Data NPC | #76 |
| `DisasterData` | Data bencana | #70 |
| `DiaryEntry`, `LoreEntry` | Data cerita | #81, #100 |
| `LootTable` | Tabel loot per lokasi | #103 |
| `RegionSpawnTable` | Tabel musuh per region | #62 |
| `RegionEnvironmentProfile` | Suasana per region | #92 |
| `GameBalanceConfig` | Semua angka penyetelan | #84 |
| `EventChannel<T>` | Jalur event global | #107 |
| `PhysicsMasks`, `PhysicsQuery` | LayerMask dan query fisika bersama | #87 |
| `IBootstrapService` | Interface manager persisten dengan `Initialize()` dan `ResetForNewRun()` | #82 |
| `PersistentRoot` | Prefab `DontDestroyOnLoad` yang memuat `RunLifecycle` dan anak `Managers` | #82 |
| `BootstrapEntry` | Komponen scene `Bootstrap` yang membuat `PersistentRoot` kalau belum ada | #82 |
| `EditorQuickStart` | Alat Editor untuk Play langsung dari scene selain `Bootstrap` | #82 |
| `MainMenuController` | Pengatur tombol menu utama di scene `MainMenu` | #82 |
| `UIPanel` | Kelas dasar panel UI yang masuk `UIFocusStack` | #83 |
| `UIPanelSettings` | Struct pengaturan panel: `pausesTime`, `releasesCursor`, `blocksGameplayInput`, `closableByEsc`, `isLocked`, `rejectsNewPanels` | #83 |
| `GameInput.inputactions` | Aset pemetaan tombol New Input System | #18 |
| `GameInputActions` | Kelas hasil generate dari `GameInput.inputactions` | #18 |
| `RebindResult` | Struct hasil ganti tombol: `status` dan `conflictingAction` | #18 |
| `RegionRecord` | Data satu region: scene, bounds, `FarMesh`, akar bangunan, dan status | #88 |
| `RegionSeam` | Data sambungan dua region: posisi, `SeamBarrier`, dan `NavMeshLink` | #88 |
| `RegionStateStore` | Penyimpan status objek per region dan `SaveId`, termasuk region yang tidak dimuat | #88 |
| `RegionSceneRoot` | Komponen pada akar tunggal tiap region scene | #88 |
| `SeamBarrier` | Collider penahan di batas region selama salah satu sisi belum dimuat | #88 |
| `RegionBoundaryGuard` | Pengaman posisi aman terakhir dan pemain tanpa tanah | #88 |
| `FarMesh` | Model jauh tiap region di `IslandPhase`, aktif saat region aslinya tidak dimuat | #88 |
| `RandomStream` | Struct generator acak SplitMix64 yang statusnya bisa disimpan | #89 |
| `RunLayout` | Data susunan run: titik suku cadang, NPC kapal, dan jadwal bencana | #89 |
| `RunLayoutSources` | ScriptableObject sumber data susunan run dan `safeLayout` | #89 |
| `RunLayoutValidator` | Pemeriksa susunan run agar bisa dimenangkan | #89 |
| `RunSeedSaveData` | Data save seed, status aliran, dan susunan run | #89 |
| `RunSeedAudit` | Alat Editor yang menguji banyak seed | #89 |
| `PartSpawnPointTable` | Daftar titik suku cadang buatan tangan di Region 4, dengan `isReachable` | #89 |
| `DisasterScheduleEntry` | Struct jadwal bencana (hari, jenis, skala), disimpan di `CurrentLayout.disasterSchedule` | #89, #70 |
| `LootPointState` | Status satu titik loot: `pointIndex`, `itemId`, `amount`, `isTaken` | #89 |
| `DamageRouter` | Kelas statis pencari `IDamageable` lewat parent dan penerap `HitboxModifier` | #59 |
| `DamageResistance` | Struct armor per `DamageType` dan `BodyPart` | #59 |
| `DamageResistanceTable` | Fungsi bersama menghitung damage setelah armor | #59 |
| `ExplosionDamage` | Kelas statis damage ledakan dengan target unik dan falloff | #59 |
| `DeathReasonMap` | Pemetaan `DamageType` ke `DeathReason` | #59 |
| `EquippableBehaviour` | Kelas dasar MonoBehaviour yang mengimplementasikan `IEquippable` | #60 |
| `EquipmentSaveData` | Data save hotbar dan tangan kiri | #60 |
| `HotbarView` | Tampilan hotbar di HUD | #60 |
| `FistBehaviour` | `EquippableBehaviour` untuk tangan kosong | #60 |
| `ConsumableEquippable` | `EquippableBehaviour` untuk konsumsi di hotbar | #60 |
| `TorchController` | `EquippableBehaviour` (`IEquippable`) tangan kiri pada prefab `Tool_Torch`: durasi bakar, cahaya, dan padam | #60, #65 |
| `InventorySlot` | Slot berisi `ItemInstance` dan `amount` | #66 |
| `ItemDatabase` | Pencarian `ItemData` berdasarkan `itemId` dan alias | #66 |
| `InventoryPanel` | `UIPanel` inventori dengan aksi item | #66 |
| `WorldItemManager` | Pemilik semua `WorldItem`, termasuk simpan dan pulih per region | #66 |
| `InteractContext` | Struct data untuk `IInteractable` | #17 |
| `InteractHighlight` | Komponen outline `Off`, `Available`, `Blocked` | #17 |
| `PickupInteractable` | Objek kecil yang langsung diambil | #17 |
| `LootContainerInteractable` | Peti loot yang mengeluarkan `WorldItem` | #17 |
| `ClimbableSurface` | Tangga atau tali yang dipanjat dengan `E` | #101 |
| `ICameraOffsetSource` | Interface sumber rotasi dan posisi tambahan kamera | #102 |
| `ViewmodelPose` | Struct posisi dan rotasi item di layar | #102 |
| `CarryCapacitySaveData` | Data save tas | #118 |
| `ConsoleCommand` | Data satu perintah konsol | #56 |
| `ConsoleHandler` | Delegate perintah konsol | #56 |
| `SavedItemData` | Struct `itemId` dan `amount` di `GameManager.savedShipLoot` | #74 |
| `ShipOutcomeData` | Hasil Fase 1: NPC selamat, korban, HP, infeksi, dan sisa waktu | #74 |
| `ShipHandEntry` | Isi slot tangan di kapal | #74 |
| `RunSummaryPanel` | `UIPanel` ringkasan Fase 1 | #74 |
| `IslandArrival` | Penempatan pemain, barang, dan NPC saat tiba di pantai | #74 |
| `ShipHazardSchedule` | ScriptableObject jadwal bahaya dan keyframe air | #75 |
| `ShipHazardEntry` | Satu entri jadwal bahaya | #75 |
| `WaterKeyframe` | Waktu sisa dan tinggi air | #75 |
| `ShipCorridor` | Koridor yang bisa runtuh | #75 |
| `ShipZombiePoint` | Titik kemunculan zombie di kapal per grup | #75 |
| `ShipRouteAudit` | Alat Editor pemeriksa rute ke sekoci | #75 |
| `ShipSwaySource` | Sumber offset kamera untuk ship sway | #75 |
| `ShipShove` | Aksi dorong pemain di Fase 1 | #75 |
| `DiseaseEffectTable` | Pasangan `DiseaseType` dan `StatusEffectData` dari Inspector | #45 |
| `DiseaseOverlay` | Image hijau penuh layar di canvas HUD untuk keracunan dan racun ular | #45 |
| `WaterSourcePoint` | Titik air kotor di tepi sungai dan danau, turunan `IslandInteractable` | #43 |
| `CampfirePanel` | `UIPanel` masak yang memakai `UIPanelId.Campfire` | #44 |
| `CookingRecipe` | Turunan `CraftingRecipe` untuk resep api unggun | #44 |
| `CookRow` | Data satu baris masak di `CampfireManager` | #44 |
| `Node_Mushroom`, `Node_SapTree` | Prefab varian `NodeType.MedicalHerb` di Region 2 | #64 |
| `Mat_TreeSap`, `Mat_Mushroom` | Item bahan vaksin dan antibiotik | #64 |
| `Mat_Charcoal` | Item arang, bahan bakar dan bahan `Med_Norit` | #44 |
| `Food_Burnt` | Item makanan gosong | #44 |
| `Food_RawFish` | Item ikan mentah. Dipakai di #44 dan #45 | #44 |
| `Kit_Campfire`, `Build_Campfire` | Item kit dan `BuildableData` api unggun | #44 |
| `Med_Norit`, `Med_Bandage`, `Med_Antibiotic` | Item obat penyakit sekunder | #45 |
| `Mat_Cloth` | Item kain, bahan `Med_Bandage` | #45 |
| `StatusEffectData` | ScriptableObject data efek sementara | #20 |
| `ActiveEffect` | Struct efek yang sedang aktif | #20 |
| `StatusEffectDatabase` | Pencarian `StatusEffectData` lewat `effectId` | #20 |
| `StatusEffectBar` | Bagian HUD yang menampilkan ikon efek | #20 |
| `Med_Supplement` | Item suplemen pemberi `Buff_Supplement` | #20 |
| `CraftingManager` | Manager antrean crafting, tetap dipakai dengan isi baru | #77 |
| `RecipeIngredient` | Struct bahan resep dan biaya | #77 |
| `RecipeDatabase` | Pencarian `CraftingRecipe` lewat `recipeId` | #77 |
| `CraftJob` | Satu job di antrean crafting | #77 |
| `CraftingPanel` | `UIPanel` untuk `UIPanelId.Crafting` | #77 |
| `Build_WoodBench` | `BuildableData` meja kayu | #77 |
| `Build_MedicalBench` | `BuildableData` meja medis | #77 |
| `Gear_WarmClothes` | Item pakaian hangat, nama sementara sampai #65 selesai | #77 |
| `Mat_Hide` | Bahan kulit hewan | #77 |
| `Mat_Rope` | Bahan tali | #77 |
| `ToolRule` | Struct damage per alat pada node | #73 |
| `RegrowRule` | Struct hari tumbuh kembali per region | #73 |
| `ResourceNodeData` | ScriptableObject data node panen | #73 |
| `IRegrowable` | Interface objek yang tumbuh kembali | #73 |
| `ResourceDropGuard` | Komponen pool yang menjaga item jatuh dari air dan tertimbun | #73 |
| `CoconutPickup` | `PickupInteractable` kelapa yang tumbuh kembali | #73 |
| `ResourceAuditWindow` | Alat Editor untuk memeriksa kepadatan node | #73 |
| `ItemUpgradeData` | ScriptableObject data upgrade item | #113 |
| `WeaponStatCalculator` | Satu titik gabungan statistik senjata | #113 |
| `WeaponStats` | Struct statistik senjata hasil perhitungan | #113 |
| `WeaponModifierData` | ScriptableObject modifier senjata dari Lucky Machine, dibaca `ToolRepairSystem` | #113, #72 |
| `RepairCheck` | Hasil pemeriksaan `CanRepair` | #113 |
| `IKailCastInterceptor` | Interface sistem yang mengambil alih lemparan Kail, dipakai `DebrisRetrieval` | #55 |
| `WaterBody` | Komponen pada collider layer `Water`: `kind` (`Sea`, `River`, `Lake`) dan `biteDelayMultiplier`. Bisa dipakai #13 dan #91 | #55 |
| `FishingTensionHUD` | Panel HUD Tension Bar, hanya membaca `FishingSystem` | #55 |
| `DebrisItem` | Barang hanyut dari pool `Debris_Generic` | #106 |
| `DebrisLane` | Jalur arus di scene region | #106 |
| `PhysicsMasks.CastLanding` | Mask titik jatuh Kail: `Water`, `Terrain`, `Structure`, `Obstacle` | #55 |
| `PhysicsQuery.RaycastIncludingTriggers` | Raycast yang sengaja mengenai trigger, untuk air | #55 |
| `SurfaceSample` | Struct hasil pembacaan permukaan | #86 |
| `SurfaceTag` | Komponen penanda `SurfaceType` pada mesh, bangunan, dan lantai kapal | #86 |
| `TerrainSurfaceSource` | Komponen pendaftar Terrain ke `SurfaceDetection` | #86 |
| `SurfaceLayerMap` | ScriptableObject pemetaan `TerrainLayer` ke `SurfaceType` | #86 |
| `SurfaceMapAsset` | ScriptableObject berisi alphamap yang di-bake | #86 |
| `SurfaceBakeTool` | Alat Editor untuk bake alphamap | #86 |
| `SurfaceAuditWindow` | Jendela Editor untuk memeriksa batas pantai | #86 |
| `SurfaceDebugOverlay` | Tampilan debug saat bermain, lewat perintah `surfacedebug` | #86 |
| `ItemCost` | Struct biaya: `itemId` dan `amount` | #79 |
| `HammerEquippable` | `EquippableBehaviour` yang meneruskan input palu sesuai mode | #79 |
| `StructureRegistry` | Daftar struktur, grafik dukungan, bongkar, runtuhan, dan penulis `world_structures` | #79 |
| `StructureNode` | Komponen posisi grid dan tingkat satu struktur | #79 |
| `StructureRecord` | Data save satu struktur | #79 |
| `NoBuildVolume` | Volume larangan bangun di seam, jembatan, dan helipad | #79 |
| `CollapseDebris` | Efek debu runtuhan dari pool `FX_CollapseDebris` | #79 |
| `BreachTarget` | Struct target serangan zombie pada struktur | #78 |
| `BreachSlots` | Pembagian posisi serangan per struktur | #78 |
| `BaseDoorLatch` | Target interaksi gerendel untuk mengunci pintu | #78 |
| `DoorLink` | NavMesh Area untuk `npcLink`, hanya dimuat agen NPC | #78 |
| `DamageVisualStage` | Struct tahap tampilan rusak per rasio HP | #47 |
| `StructureVisual` | Komponen penukar material dan mesh sesuai rasio HP | #47 |
| `StoragePanel` | `UIPanel` dengan id `Storage`: panel inventori di kiri dan peti di kanan | #46 |
| `SlotArrayOps` | Kelas statis untuk menambah, menggabung, dan menghitung isi array slot. Dipakai peti dan `InventoryManager` | #46 |
| `SlotGridView` | Komponen UI yang menggambar satu array slot | #46 |
| `DragState` | Status seret dan lepas di `StoragePanel` | #46 |
| `ChestSlotRecord` | Struct save untuk satu slot peti yang berisi | #46 |
| `StorageChestSaveData` | Data save isi satu peti | #46 |
| `BedInteractable` | `IslandInteractable` di prefab `Bed_Basic`, menerima tombol `E` | #67 |
| `SleepPanel` | `UIPanel` dengan id `Sleep`: layar gelap, jam, dan tombol bangun | #67 |
| `RunSaveEnvelope` | Amplop file save: versi, CRC, dan payload JSON | #80 |
| `RunSavePayload` | Isi save satu run | #80 |
| `SaveEntry` | Pasangan id dan JSON milik satu `ISaveable` | #80 |
| `SaveIdTag` | MonoBehaviour penyimpan ID stabil untuk objek scene region | #80 |
| `SaveIdAuditWindow` | Alat Editor untuk memeriksa ID kosong dan ID ganda | #80 |
| `SaveMigrator` | Rantai migrasi format save antar-versi | #80 |
| `SaveVersions` | Konstanta versi format save, `Current` bernilai 1 | #80 |
| `GunCore` | Logika tembakan bersama pemain dan NPC, tanpa MonoBehaviour | #58 |
| `NPCGunUser` | Pembungkus `GunCore` untuk NPC | #58 |
| `PooledEffect` | `IPoolable` untuk efek sekali muncul, seperti muzzle flash dan impact. Berasal dari `GunController`, dipindah ke `VFXManager` | #58, #15 |
| `AttackCoordinator` | Manager per scene yang membagikan izin menyerang per target | #109 |
| `PlayerShove` | Komponen aksi Shove pemain di `IslandPhase` | #95 |
| `CorpseBudget` | Manager per scene untuk batas mayat dan ragdoll | #95 |
| `BaseAnchor` | `ITargetable` di pusat base untuk Blood Moon | #108 |
| `PlayerTargetable` | `ITargetable` milik pemain | #108 |
| `ThreatProfile` | ScriptableObject bobot pemilihan target, aset `Threat_Zombie` dan `Threat_Animal` | #108 |
| `ThreatState` | Struct status target per musuh | #108 |
| `AISensoryHub` | Pembagi tick indra, nilai UI, dan titik perburuan | #49 |
| `DetectionMeterUI` | Ikon mata di HUD | #49 |
| `HarvestEntry` | Struct hasil buruan di `AnimalData` | #61 |
| `AnimalCarcass` | `IInteractable` untuk menguliti bangkai | #61 |
| `DenPoint` | Penanda sarang hewan di scene region | #61 |
| `AmbushPerch` | Penanda pohon untuk sergapan Jaguar | #61 |
| `CrowFlock` | Pembagi token menukik kawanan Gagak | #61 |
| `hewan_serigala`, `hewan_babi`, `hewan_jaguar`, `hewan_ular`, `hewan_beruang`, `hewan_serigala_salju`, `hewan_anjing`, `hewan_gagak` | Nilai `EnemyData.enemyId` dan tag pool hewan | #61 |
| `SpawnEntry` | Baris `RegionSpawnTable` | #62 |
| `SpawnOverride`, `SpawnOverrideHandle` | Override spawn untuk #50 dan #70 | #62 |
| `EnemyInstance` | Data spawn per musuh Fase 2 dan pengaman macet | #62 |
| `BushSpawnPoint` | Titik semak Zombie Lumut di Region 2 | #62 |
| `VillageZombieHiding` | Pengatur state sembunyi Zombie Desa | #63 |
| `TribalBehavior`, `MossBehavior`, `MilitaryBehavior`, `FrozenBehavior`, `VillageBehavior` | Turunan `SpecialZombieAI` untuk tiap varian | #25 |
| `Tribal_Snare`, `Thrown_Rock` | Tag pool proyektil di layer `Projectile` (`Tribal_Snare` #25, `Thrown_Rock` #94) | #25 |
| `ThreatKey` | Baris kurva ancaman per hari di `GameBalanceConfig.aiDirector` | #27 |
| `WaveRequest`, `WaveModifiers` | Struct permintaan gelombang dari `AIDirector` ke `SpawnManager`, dan pengali musuh | #27 |
| `ICrowdAgent` | Interface agen yang di-tick `HordeCrowdManager` | #93 |
| `TraversalAgent` | Komponen musuh untuk melewati `NavMeshLink` dan menjaga posisi di NavMesh | #94 |
| `TraversalLinkGate` | Komponen di `NavMeshLink` yang membatasi pengguna | #94 |
| `JumpLink`, `ClimbLink`, `HighClimbLink`, `DropLow`, `DropMid`, `DropHigh` | Area NavMesh untuk `NavMeshLink`, nomor 3 sampai 8 | #94 |
| `IPinAttacker` | Interface penyerang yang menahan pemain | #110 |
| `PinnedPromptUI` | Penunjuk tombol dan cincin kemajuan saat tertahan | #110 |
| `TrapData` | ScriptableObject data efek jebakan | #26 |
| `TrapVictim` | Struct target di dalam zona jebakan | #26 |
| `Build_SpikeTrap` | `BuildableData` Spike Trap | #26 |
| `Build_BarbedWire` | `BuildableData` kawat berduri | #26 |
| `Mat_Scrap` | Item bahan dari reruntuhan Region 3 dan 4 | #26 |
| `ThrowableProjectile` | Komponen proyektil dari `ObjectPooler` | #96 |
| `LureTargetable` | `ITargetable` berjenis `Lure` milik Suar | #96 |
| `Throw_Molotov` | Item Molotov | #96 |
| `Throw_Bomb` | Item Bom Rakitan | #96 |
| `Throw_Rock` | Item Batu lempar | #96 |
| `Throw_Flare` | Item Suar | #96 |
| `LairData` | ScriptableObject data sarang | #112 |
| `LairNest` | Sarang yang bisa dihancurkan, `IDamageable` | #112 |
| `LairRegistry` | Manager daftar sarang dan zona aktif | #112 |
| `Interior` | NavMesh Area di dalam `InteriorZone` | #112 |
| `BridgeSite` | `IslandInteractable` di tepi jurang S1 untuk setoran dan progres jembatan | #68 |
| `GateBlocker` | `IslandInteractable` gerbang tua di S2 yang memberi alasan terkunci | #68 |
| `GateBarrier` | `BoxCollider` layer `Obstacle`, dinding tak terlihat di seam dan perairan sekitarnya | #68 |
| `RavineRecoveryVolume` | Trigger layer `ZoneTrigger` di dasar jurang S1 yang mengembalikan pemain | #68 |
| `GateRule` | Data aturan per seam | #68 |
| `HeavyItem` | Komponen `IInteractable` pada prefab suku cadang di dunia | #69 |
| `PartRecord` | Data serializable status satu suku cadang | #69 |
| `WaterVolume` | Trigger layer `Water` yang memberi tinggi permukaan dan jenis air | #13 |
| `TideSafePoint` | Titik di atas garis air tertinggi, tujuan barang penting yang terkena pasang | #91 |
| `LowTideSpot` | `IslandInteractable` berupa titik karang kecil yang hanya bisa dipakai saat surut | #91 |
| `TideZoneAuditWindow` | Alat Editor yang memeriksa tanah terendam yang belum dicat pasir atau di luar Area `Tidal` | #91 |
| `Tidal` | Area NavMesh pantai yang biayanya naik saat pasang lewat `NavMesh.SetAreaCost` | #91 |
| `ZoneTracker` | Kelas statis penghitung zona yang menentukan region aktif saat zona bertumpuk | #92 |
| `WeatherEnvironmentLayer` | Data lapisan fog dan cahaya per `WeatherType` | #92 |
| `EnvironmentEventLayer` | Data lapisan suasana event bencana atau Blood Moon, dengan prioritas | #92 |
| `RegionWeatherTable` | ScriptableObject tabel peluang cuaca per region | #115 |
| `WeatherEffects` | Struct efek cuaca: `speedMultiplier`, `aiSightMultiplier`, `aiHearingMultiplier`, `warmthLossPerSec` | #115 |
| `WeatherOverride` | Struct pemaksa cuaca per region dari `DisasterManager`: `regionMask`, `weather`, `snowWeather` | #115 |
| `DisasterScheduler` | Kelas pembuat jadwal bencana dari seed, dipanggil `BuildLayout` di `RunSeedManager` | #70 |
| `Hazard_Fireball` | Tag pool Fireball erupsi | #70 |
| `Hazard_ImpactMarker` | Tag pool penanda titik jatuh Fireball dan pohon tumbang | #70 |
| `BeachCrate` | `IslandInteractable` kargo pantai yang berasal dari pool | #71 |
| `Supply_BeachCrate` | Tag pool kargo pantai | #71 |
| `BeachCrateTier` | Struct tier isi kargo menurut hari minimum | #71 |
| `BeachDeliveryState` | Data simpanan kiriman kargo: kargo yang sudah dibuka dan status kiriman | #71 |
| `BeachSupplyPoint` | Titik buatan tangan di `IslandPhase` untuk posisi kargo: `position`, `isReachable` | #71 |
| `SupplyPlane` | Prefab pool visual dan suara pesawat | #54 |
| `SupplyCrate` | Prefab pool peti, turunan `IslandInteractable` | #54 |
| `LuckyMachineCage` | Komponen trigger kandang, gerbang, dan vakum | #72 |
| `LuckyMachineData` | ScriptableObject region, kuota, dan tabel hadiah. Satu aset per mesin | #72 |
| `LuckyRewardLedger` | Catatan hadiah bersama untuk keempat Lucky Machine, disimpan di `world_lucky` | #72 |
| `LuckyRewardData` | ScriptableObject satu hadiah | #72 |
| `SacrificeQuota` | Data kuota per `enemyId` | #72 |
| `NPCRecord` | Baris data per NPC di `NPCRoster`, termasuk isi yang disimpan | #76 |
| `ObjectiveData` | ScriptableObject satu tujuan cerita | #81 |
| `DiaryReward` | Struct hadiah satu bab Diary | #81 |
| `DiaryArchivePanel` | Panel baca ulang bab dan daftar tujuan selesai | #81 |
| `LootEntry` | Struct satu baris `LootTable`, menggantikan kelas `LootTable` lama | #103 |
| `LootPointDef` | Definisi satu titik di `LootSpawnerSystem`: `point`, `kind`, `guaranteedItems` | #103 |
| `ItemStack` | Pasangan `ItemData` dan jumlah untuk `guaranteedItems` | #103 |
| `BaseCluster` | Kelompok struktur yang dihitung ulang sebagai satu base, tidak disimpan | #90 |
| `RaidPlan` | Rencana serangan simulasi satu malam, disimpan di `world_remotebase` | #90 |
| `FireArea` | Objek pool (tag `FX_FireArea`), area api di tanah dengan damage per detik | #97 |
| `BurnEffect` | Objek pool (tag `FX_FireBurn`), partikel, cahaya, dan collider padam untuk satu `Flammable` | #97 |
| `FlammableProfile` | ScriptableObject berisi ketahanan, durasi, dan peluang menjalar per material. Aset: `Flame_WoodStructure`, `Flame_Stone`, `Flame_Vegetation`, `Flame_Zombie`, `Flame_Animal` | #97 |
| `FireExtinguishTarget` | Komponen `IInteractable` pada objek pool api untuk memadamkan dengan tombol `E` | #97 |
| `world_fire` | `SaveId` `FireSystem` untuk status bakar struktur | #97 |
| `LightSource` | Komponen satu pemancar cahaya, dipakai struktur, api unggun, obor, senter, dan api | #98 |
| `LightFixture` | Turunan `IslandInteractable` dan `ISaveable` untuk bahan bakar dan interaksi Obor Dinding serta Lampu | #98 |
| `LightFixtureData` | ScriptableObject berisi data bahan bakar tiap struktur cahaya | #98 |
| `Build_WallTorch` | ID `BuildableData` untuk Obor Dinding | #98 |
| `Build_Lamp` | ID `BuildableData` untuk Lampu | #98 |
| `Item_Battery` | Item `Consumable` untuk mengisi senter dan menjadi bahan bakar Lampu | #98 |
| `HelicopterFrame` | Model kerangka helikopter di `Region3_Snow` | #14 |
| `PartInstallZone` | Trigger titik pasang suku cadang | #14 |
| `CockpitInteractable` | Interaksi `E` untuk menyalakan mesin dan lepas landas | #14 |
| `DeathProfile` | Data gaya, teks, warna, dan suara per `DeathReason` | #53 |
| `PlayerBodyRig` | Tubuh ragdoll pada prefab pemain, nonaktif selama hidup | #53 |
| `LocalStatsData` | Isi file `stats.json` | #57 |
| `RunCounters` | Penghitung statistik run yang sedang berjalan | #57 |
| `DeathCount` | Pasangan penyebab kematian dan jumlahnya | #57 |
| `StatsPanel` | Panel statistik di menu utama | #57 |
| `NarrativeScene` | ScriptableObject satu adegan | #99 |
| `NarrativeBeat` | Satu langkah adegan: gambar, teks, subtitle, dan audio | #99 |
| `EndingContext` | Data NPC dan hari untuk memilih ending | #99 |
| `HelipadFoundVolume` | Trigger pemicu adegan `helipad_found` di `Region3_Snow` | #99 |
| `SafetyFinding` | Satu hasil pemeriksaan kondisi run | #116 |
| `RegionSafePoint` | Titik aman buatan tangan per region | #116 |
| `StonePickup` | `PickupInteractable` batu kecil dan ranting yang bisa tumbuh lagi | #116 |
| `AnimIds` | Kelas statis berisi hash parameter Animator | #12 |
| `PlayerAnimDriver` | Mengisi `Speed` tangan dan mengganti set animasi item | #12 |
| `EnemyAnimDriver` | Mengisi `Speed` dari `NavMeshAgent` dan menerjemahkan state AI | #12 |
| `ViewmodelAnimSet` | Pasangan `itemId` dan `AnimatorOverrideController` untuk tangan | #12 |
| `AC_PlayerArms`, `AC_ZombieBase`, `AC_AnimalBase`, `AC_NPC` | Aset Animator Controller per kelompok karakter | #12 |
| `VFXHandle` | Struct pegangan efek loop, dengan pemeriksaan `generation` | #15 |
| `VFXEntry` | Aturan satu efek di `GameBalanceConfig.vfx.catalog` | #15 |
| `TutorialHint` | ScriptableObject data satu pop-up tutorial | #16 |
| `TutorialHintPanel` | Panel teks pop-up tutorial, di luar `UIFocusStack` | #16 |
| `SettingsData` | Data setting pemain yang disimpan sebagai JSON di `PlayerPrefs` | #19 |
| `SettingsPanel` | `UIPanel` untuk `UIPanelId.Settings` | #19 |
| `SurfaceAudioSet` | ScriptableObject berisi klip langkah dan mendarat per `SurfaceType` | #21 |
| `SurfaceAudioEntry` | Satu baris `SurfaceAudioSet` untuk satu `SurfaceType` | #21 |
| `NotificationEntry` | Data satu pesan yang tampil atau menunggu | #22 |
| `NotificationLogEntry` | Data satu baris riwayat pesan | #22 |
| `NotificationRow` | Baris UI notifikasi yang dipakai ulang | #22 |
| `NotificationStyle` | Warna, ikon, dan klip per `NotificationKind` | #22 |
| `NotificationLogView` | Tampilan riwayat pesan di dalam panel `Pause` | #22 |
| `PostFxRig` | Prefab per scene berisi `PostProcessController` dan tiga Volume global | #23 |
| `StatusProfile` | Aset `VolumeProfile` untuk efek status, disalin saat `Awake` | #23 |
| `CameraShakePreset` | Data satu jenis getaran | #24 |
| `CameraShakeProfile` | ScriptableObject berisi semua `CameraShakePreset` | #24 |
| `DamageIndicator` | Objek pool UI berbentuk busur damage, dengan tag `UI_DamageIndicator` | #51 |
| `AmbientProfile` | ScriptableObject berisi loop dan one-shot satu area | #52 |
| `AmbientLayer` | Kelas serializable untuk satu loop di `AmbientProfile` | #52 |
| `AmbientOneShot` | Kelas serializable untuk suara acak di `AmbientProfile` | #52 |
| `AmbientZone` | Komponen `BoxCollider` trigger area suara di layer `ZoneTrigger` | #52 |
| `AmbientDeck` | Satu jalur crossfade dengan 3 `AudioSource` | #52 |
| `PooledAudioSource` | Komponen `IPoolable` untuk suara one-shot 3D dengan tag pool `Audio_OneShot3D`, dipakai bersama #114 | #52 |
| `MusicSet` | ScriptableObject berisi klip calm dan stem musik | #114 |
| `EnemyCueSet` | ScriptableObject berisi suara penanda per varian dan hewan | #114 |
| `ThreatAudioEmitter` | Komponen `IPoolable` di prefab musuh untuk suara 3D, daftar musuh aktif, dan status `IsEngaged` | #114 |
| `BarkSet` | ScriptableObject kumpulan baris ucapan per NPC | #100 |
| `BarkLine` | Struct satu baris ucapan: kunci teks dan klip opsional | #100 |
| `LorePoint` | `IslandInteractable` temuan lore di scene region | #100 |
| `LoreReaderPanel` | `UIPanel` untuk panel `LoreReader` | #100 |
| `LoreArchiveFile` | Pembaca dan penulis `lore_archive.json` | #100 |
| `LocalizationTable` | ScriptableObject tabel teks per bahasa | #104 |
| `LocalizedRow` | Struct satu baris kunci dan teks | #104 |
| `LocalizedText` | Komponen pengisi teks pada `TMP_Text` | #104 |
| `AccessibilityOptions` | Struct pilihan aksesibilitas | #104 |
| `SubtitlePresenter` | Komponen penampil subtitle persisten | #104 |
| `GhostStatusIcon` | Ikon status pada hologram bangunan | #104 |
| `FlashGuard` | Pembatas kilatan layar penuh | #104 |
| `LocalizationAuditWindow` | Alat Editor pemeriksa kunci teks | #104 |
| `MapMarker` | Struct data penanda kompas | #30 |
| `MapMarkerRegistry` | Kelas penyimpan penanda, dimiliki `CompassSystem` | #30 |

**Pemilik status pemain:** `SurvivalStats` memegang `HP`, `Stamina`, `Hunger`, `Thirst`, dan `Infection`. Sistem lain membaca atau mengubah nilainya lewat fungsi publik `SurvivalStats`.

**Pemilik hari:** `GameManager.currentDay`. `DayNightDirector` memanggil `GameManager.AdvanceDay()` saat jam melewati 24:00.

**Item inti:** `Base_Kail`, `Base_Pistol`, dan `Base_Pisau`. Ketiganya berjenis `KeyItem` atau memiliki tanda `isCoreItem`, dan tidak bisa dibuang.

---

## 6. Layer dan tag

Daftar lengkap ada di `Core_Logic/PhysicsLayers.md`.

- **Layer:** `Player`, `PlayerViewmodel`, `Enemy`, `EnemyHitbox`, `NPC`, `Interactable`, `WorldItem`, `Terrain`, `Structure`, `StructureGhost`, `Obstacle`, `Water`, `ZoneTrigger`, `Projectile`, `Ragdoll`, `HeavyItem`.
- **Tag:** `Player`, `MainCamera`, `Zombie`, `Animal`, `NPC`, `BaseWall`, `BaseDoor`, `HeavyItem`.
- **Dihapus:** layer `Sand/Beach`. Pantai dideteksi lewat `SurfaceDetection` (#86), karena satu Terrain hanya punya satu layer.

---

## 7. File blueprint per issue

Setiap issue menulis ke file berikut. Kalau issue tidak menyebut nama file, baris ini yang menentukannya.

| Issue | File | Kelas utama |
|---|---|---|
| #12 | `Core_Logic/AnimationController.md` | `AnimationController` |
| #13 | `Island_Logic/WaterSwimmingController.md` | `SwimmingController` |
| #14 | `Island_Logic/RescueEventManager.md` | `RescueEventManager` |
| #15 | `Core_Logic/VFXManager.md` | `VFXManager` |
| #16 | `Core_Logic/TutorialManager.md` | `TutorialManager` |
| #17 | `Island_Logic/IslandInteractable.md` | `IslandInteractable`, `PlayerInteractor` |
| #18 | `Core_Logic/InputManager.md` | `InputManager` |
| #19 | `Core_Logic/SettingsManager.md` | `SettingsManager` |
| #20 | `Core_Logic/StatusEffectManager.md` | `StatusEffectManager` |
| #21 | `Core_Logic/FootstepAudio.md` | `FootstepAudio` |
| #22 | `Core_Logic/NotificationManager.md` | `NotificationManager` |
| #23 | `Core_Logic/PostProcessController.md` | `PostProcessController` |
| #24 | `Core_Logic/CameraShake.md` | `CameraShake` |
| #25 | `left-4-dead/SpecialZombieAI.md` | `SpecialZombieAI`, `ZombieData` |
| #26 | `raft/BaseDefense.md` | `BaseTrap` |
| #27 | `left-4-dead/AIDirector.md` | `AIDirector` |
| #43 | `raft/HydrationSystem.md` | `HydrationSystem`, `RainwaterCollector` |
| #44 | `raft/CampfireManager.md` | `CampfireManager` |
| #45 | `raft/DiseaseManager.md` | `DiseaseManager` |
| #46 | `raft/StorageChest.md` | `StorageChest` |
| #47 | `raft/BuildingIntegrity.md` | `BuildingIntegrity` |
| #48 | `raft/FarmingSystem.md` (tidak dipakai) | `FarmingSystem`, `PlanterBox` |
| #49 | `left-4-dead/AISensory.md` | `AISensory`, `NoiseEmitter` |
| #50 | `left-4-dead/HordeManager.md` | `HordeManager` |
| #51 | `Core_Logic/CombatFeedbackUI.md` | `CombatFeedbackUI` |
| #52 | `Core_Logic/AmbientAudio.md` | `AmbientAudio` |
| #53 | `Core_Logic/DeathCinematic.md` | `DeathCinematic` |
| #54 | `Island_Logic/SupplyDrop.md` | `SupplyDropManager` |
| #55 | `raft/FishingSystem.md` | `FishingSystem` |
| #56 | `Core_Logic/DevConsole.md` | `DevConsole` |
| #57 | `Core_Logic/LocalStatsManager.md` | `LocalStatsManager` |
| #58 | `left-4-dead/GunController.md` | `GunController` |
| #59 | `Core_Logic/DamagePipeline.md` | `IDamageable`, `EnemyHealth`, `BaseHealth` |
| #60 | `Core_Logic/EquipmentManager.md` | `EquipmentManager` |
| #61 | `left-4-dead/WildlifeAI.md` | `WildlifeAI`, `AnimalData` |
| #62 | `left-4-dead/SpawnManager.md` | `SpawnManager` |
| #63 | `left-4-dead/SunlightExposure.md` | `SunlightExposure`, `HidingSpot` |
| #64 | `raft/InfectionSystem.md` | `InfectionSystem` |
| #65 | `Island_Logic/WarmthSystem.md` | `WarmthSystem` |
| #66 | `Core_Logic/ItemUseSystem.md` | `ItemUseSystem` |
| #67 | `Island_Logic/SleepSystem.md` | `SleepSystem` |
| #68 | `Island_Logic/RegionGateSystem.md` | `RegionGateSystem`, `BridgeSite` |
| #69 | `Island_Logic/HeavyItemSystem.md` | `HeavyItemSystem` |
| #70 | `Island_Logic/DisasterManager.md` | `DisasterManager` |
| #71 | `Island_Logic/BeachSupplyManager.md` | `BeachSupplyManager` |
| #72 | `Island_Logic/LuckyMachine.md` | `LuckyMachine` (memperbarui blueprint lama) |
| #73 | `raft/ResourceRespawn.md` | `ResourceRespawn` |
| #74 | `60-seconds/LifeboatExtraction.md` | `LifeboatExtraction` |
| #75 | `60-seconds/ShipHazardManager.md` | `ShipHazardManager` |
| #76 | `Island_Logic/NPCRoster.md` | `NPCRoster`, `NPCData` |
| #77 | `Core_Logic/CraftingStation.md` | `CraftingStation`, `CraftingRecipe` |
| #78 | `raft/BaseNavigation.md` | `BaseNavigation`, `BaseDoor` |
| #79 | `raft/BuildMenu.md` | `BuildMenu`, `BuildableData` |
| #80 | `Core_Logic/WorldStateSave.md` | `WorldStateSave`, `ISaveable` |
| #81 | `Island_Logic/ObjectiveManager.md` | `ObjectiveManager`, `DiaryEntry` |
| #82 | `Core_Logic/RunLifecycle.md` | `RunLifecycle` |
| #83 | `Core_Logic/UIFocusStack.md` | `UIFocusStack` |
| #84 | `Core_Logic/GameBalanceConfig.md` | `GameBalanceConfig` |
| #85 | `GLOSSARY.md` | Glosarium ini |
| #86 | `Core_Logic/SurfaceDetection.md` | `SurfaceDetection` |
| #87 | `Core_Logic/PhysicsLayers.md` | `PhysicsMasks`, `PhysicsQuery` |
| #88 | `Core_Logic/RegionStreaming.md` | `RegionStreaming` |
| #89 | `Core_Logic/RunSeedManager.md` | `RunSeedManager` |
| #90 | `Island_Logic/RemoteBaseSimulation.md` | `RemoteBaseSimulation` |
| #91 | `Island_Logic/TideManager.md` | `TideManager` |
| #92 | `Island_Logic/EnvironmentProfile.md` | `EnvironmentDirector`, `RegionEnvironmentProfile` |
| #93 | `left-4-dead/HordeCrowdManager.md` | `HordeCrowdManager` |
| #94 | `left-4-dead/AITraversal.md` | `AITraversal` |
| #95 | `left-4-dead/EnemyHitReaction.md` | `EnemyHitReaction` |
| #96 | `left-4-dead/ThrowableSystem.md` | `ThrowableSystem` |
| #97 | `Island_Logic/FireSystem.md` | `FireSystem`, `Flammable` |
| #98 | `Island_Logic/LightSourceSystem.md` | `LightSourceSystem` |
| #99 | `Core_Logic/NarrativeSequencer.md` | `NarrativeSequencer` |
| #100 | `Island_Logic/LoreCollectibles.md` | `LoreCollectibles`, `NPCBarks` |
| #101 | `Island_Logic/PlayerTraversal.md` | `PlayerTraversal` |
| #102 | `Core_Logic/FirstPersonCamera.md` | `FirstPersonCamera` |
| #103 | `Core_Logic/RegionLootTable.md` | `RegionLootTable` |
| #104 | `Core_Logic/LocalizationManager.md` | `LocalizationManager` |
| #105 | `Core_Logic/WorldMapSystem.md` (tidak dipakai) | `WorldMapSystem` |
| #106 | `raft/DebrisRetrieval.md` | `DebrisRetrieval` |
| #107 | `Core_Logic/GameEventBus.md` | `EventChannel<T>` |
| #108 | `left-4-dead/ThreatSystem.md` | `ThreatSystem`, `ITargetable` |
| #109 | `left-4-dead/EnemyAttack.md` | `EnemyAttack`, `AttackData` |
| #110 | `Island_Logic/PlayerPinnedState.md` | `PlayerPinnedState` |
| #111 | `raft/VitalsRules.md` | Aturan untuk `SurvivalStats` |
| #112 | `left-4-dead/ZombieLair.md` | `ZombieLair`, `InteriorZone` |
| #113 | `Core_Logic/ToolRepairSystem.md` | `ToolRepairSystem` |
| #114 | `Core_Logic/CombatMusicDirector.md` | `CombatMusicDirector` |
| #115 | `Island_Logic/RegionalWeather.md` | `RegionalWeather` |
| #116 | `Core_Logic/ProgressionSafety.md` | `ProgressionSafety` |
| #117 | `Island_Logic/NPCInfection.md` (tidak dipakai) | `NPCInfection` |
| #118 | `Core_Logic/CarryCapacity.md` | `CarryCapacity` |
