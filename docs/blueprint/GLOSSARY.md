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
| Syarat menang | Helikopter lepas landas sebelum hari ke-60 berakhir |
| Headshot | 2x damage |
| Tombol interaksi | `E` di Fase 1 dan Fase 2. Klik kiri dipakai item aktif |
| Referensi combat | Left 4 Dead (L4D) saja |

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
| `RunResult` | `Won`, `Died`, `TimeExpired` | `GameManager` |
| `RegionType` | `Volcano`, `Amazon`, `Snow`, `Logistic` | `ZoneManager` |
| `ItemType` | `Material`, `Consumable`, `Weapon`, `Tool`, `KeyItem` | `ItemData` |
| `ShipInteractableKind` | `Item`, `NPC` | `ShipInteractable` |
| `DamageType` | `Bullet`, `Melee`, `Fire`, `Explosion`, `Frost`, `Starvation`, `Dehydration`, `Infection`, `Structural`, `Fall`, `Drown`, `Poison` | `DamagePipeline` (#59) |
| `DeathReason` | `Starvation`, `Dehydration`, `Zombified`, `KilledByEnemy`, `Frostbite`, `Drowned`, `Fall`, `Fire`, `Explosion`, `Poison`, `LeftOnShip`, `TimeExpired` | `DamagePipeline` (#59) |
| `BodyPart` | `Head`, `Torso`, `Leg` | `HitboxModifier` |
| `SurfaceType` | `Pasir`, `Rumput`, `Tanah`, `Batu`, `Salju`, `Lumpur`, `Kayu`, `Logam` | `SurfaceDetection` (#86) |
| `WeatherType` | `Clear`, `Rain`, `Fog`, `Storm`, `Snow`, `Blizzard` | `WeatherManager`, ditambah #115 |
| `DisasterType` | `Eruption`, `Earthquake`, `TropicalStorm` | `DisasterManager` (#70) |
| `AIState` | `Idle`, `Chase`, `AttackPlayer`, `AttackBuilding`, `BurnToAshes`, `Stagger`, `Knockback`, `SeekHiding`, `Submerged`, `Emerging`, `Dormant` | `EnemyAI` |
| `AnimalState` | `Wander`, `Alert`, `Chase`, `Attack`, `ReturnToTerritory`, `FleeToDen`, `TargetUnreachable` | `WildlifeAI` (#61) |
| `NPCState` | `Follow`, `Attack`, `Idle`, `StayAtBase`, `Work`, `TakeCover` | `NPCCompanion`, ditambah #76 |

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
| #48 | `raft/FarmingSystem.md` | `FarmingSystem`, `PlanterBox` |
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
| #105 | `Core_Logic/WorldMapSystem.md` | `WorldMapSystem` |
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
| #117 | `Island_Logic/NPCInfection.md` | `NPCInfection` |
| #118 | `Core_Logic/CarryCapacity.md` | `CarryCapacity` |
