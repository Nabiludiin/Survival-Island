# 🧩 Survival Island - Kode Gabungan (LOGIC)

Folder ini adalah tempat **kode gabungan C#** Survival Island. Semua *script* yang ditulis dari blueprint di `Survival Island/doc/blueprint/` disimpan di sini, sesuai `README.md` di root (TAHAP 6).

**Status:** Langkah 1 sampai 7 sudah ditulis dan digabung ke `main`. Proyek Unity 6 LTS di `Survival Island/` sudah dibuka di Unity; kode gabungan Langkah 1-3 terkompilasi tanpa error di Unity 6000.3.10f1 (6 November 2026), begitu juga gabungan Langkah 4-5 dan Langkah 6-7 (7 November 2026), tetapi belum ada yang dijalankan di Play Mode.

> Jangan tertukar dengan `LOGIC/` di root repo. Folder itu hanya bahan analisis dari game referensi (60 Seconds!, Raft, dan Zombie). Kode dari sana tidak boleh disalin ke folder ini.

## 📍 Struktur Folder

Usulan struktur: sub-folder di sini mengikuti folder blueprint, supaya setiap *script* mudah dicari blueprint-nya.

```text
Survival Island/Assets/LOGIC/
├── README.md          # File ini
├── Core_Logic/        # Dari Survival Island/doc/blueprint/Core_Logic/ (GameManager, ObjectPooler, UIManager, dll.)
├── 60-seconds/        # Dari Survival Island/doc/blueprint/60-seconds/ (Fase 1: kapal tenggelam)
├── raft/              # Dari Survival Island/doc/blueprint/raft/ (status, gathering, base building)
├── left-4-dead/       # Dari Survival Island/doc/blueprint/left-4-dead/ (siang-malam, AI musuh, spawn, gunplay)
└── Island_Logic/      # Dari Survival Island/doc/blueprint/Island_Logic/ (region, cuaca, Diary, Lucky Machine)
```

Sub-folder baru dibuat saat Langkah pertama yang membutuhkannya dijalankan. Folder kosong tidak perlu dibuat lebih dulu.

### Aturan penamaan

- Satu kelas utama per file. Nama file sama dengan nama kelas dan nama blueprint, misalnya `Survival Island/doc/blueprint/Core_Logic/GameManager.md` menjadi `Core_Logic/GameManager.cs`.
- Kelas kecil yang hanya dipakai satu sistem boleh satu file dengan pemiliknya, kalau blueprint-nya menulis begitu.
- Nama kelas, enum, tag, layer, dan scene mengikuti `Survival Island/doc/blueprint/GLOSSARY.md`.

## 🧱 Assembly dan Namespace

- **Assembly definition:** `SurvivalIsland.asmdef` di folder ini. Semua *script* di `LOGIC/` masuk satu assembly `SurvivalIsland`.
- **Referensi:** `Unity.InputSystem` (`UIPanel.toggleAction`), `Unity.TextMeshPro` (teks panel), dan `UnityEngine.UI` (`Button`, `Slider`). `Unity.AI.Navigation` dan `Unity.RenderPipelines.Universal.Runtime` belum ditambahkan, karena belum dipakai. Langkah yang pertama memakainya menambahkan referensi itu.
- **Namespace (usulan):** semua kelas memakai namespace `SurvivalIsland`, tanpa sub-namespace.
- **Kode khusus Editor** (`EditorQuickStart`, `SaveIdTag.OnValidate`) diapit `#if UNITY_EDITOR`, jadi tetap berada di assembly yang sama.

## 🧩 Isi Langkah 1

| File | Blueprint |
|---|---|
| `Core_Logic/GameEventBus.cs` (`EventChannel<T>`, `StickyEventAttribute`), `GameEventRegistry.cs`, `GameEvents.cs` | `GameEventBus.md` |
| `Core_Logic/PhysicsMasks.cs`, `PhysicsQuery.cs` | `PhysicsLayers.md` |
| `Core_Logic/GameBalanceConfig.cs`, `GameBalanceSections.cs` | `GameBalanceConfig.md` |
| `Core_Logic/GameManager.cs` (`GamePhase`, `RunResult`, `GameRunSaveData`) | `GameManager.md` |
| `Core_Logic/RunLifecycle.cs` (`RunState`), `IBootstrapService.cs`, `BootstrapEntry.cs`, `EditorQuickStart.cs`, `MainMenuController.cs` | `RunLifecycle.md` |
| `Core_Logic/AsyncSceneLoader.cs` | `AsyncSceneLoader.md` |
| `Core_Logic/RunSeedManager.cs`, `RandomStream.cs`, `RunLayout.cs` | `RunSeedManager.md` |
| `Core_Logic/UIFocusStack.cs`, `UIPanel.cs` | `UIFocusStack.md` |
| `Core_Logic/UIManager.cs`, `FadingUIPanel.cs`, `PausePanel.cs`, `LoadingPanel.cs`, `CinematicPanel.cs`, `EndScreenPanel.cs`, `ConfirmDialogPanel.cs` | `UIManager.md` |
| `Core_Logic/ObjectPooler.cs`, `PooledObject.cs`, `IPoolable.cs` | `ObjectPooler.md` |
| `Core_Logic/WorldStateSave.cs`, `WorldStateSaveData.cs`, `ISaveable.cs`, `SaveIdTag.cs` | `WorldStateSave.md` (pengganti `SaveSystem.md`, tidak ada kelas `SaveSystem`) |
| `Core_Logic/DeathReason.cs` | `GLOSSARY.md` (pemilik `DamagePipeline`, Langkah 5) |
| `Island_Logic/DisasterScheduleEntry.cs` (`DisasterType`) | `DisasterManager.md` (Langkah 7) |
| `60-seconds/ShipOutcomeData.cs` (`SavedItemData`) | `LifeboatExtraction.md` (Langkah 2) |

Tiga file terakhir adalah kontrak milik Langkah lain yang sudah dibutuhkan Langkah 1. Langkah pemiliknya memakai tipe ini dan tidak mendeklarasikannya ulang. Event baru ditambahkan ke `Core_Logic/GameEvents.cs`, dan bagian config baru diisi di `Core_Logic/GameBalanceSections.cs`.

Penyimpangan kecil dari blueprint dicatat di bagian "Catatan Implementasi (Langkah 1)" pada blueprint terkait.

### Penyiapan di Unity untuk Langkah 1 (sudah dilakukan)

Sudah dibuat lewat menu **Survival Island > Setup > TMP Essential Resources** lalu **Survival Island > Setup > Langkah 1 - Fondasi** (`Assets/Editor/Langkah1Setup.cs`, boleh dijalankan ulang). Hasilnya `Assets/Data/GameBalanceConfig.asset`, `Assets/Prefabs/PersistentRoot.prefab` (9 layanan yang sudah ada kodenya, urutan sesuai `RunLifecycle.md`, ditambah `SurfaceDetection` setelah `ObjectPooler`), scene `Bootstrap`, dan scene `MainMenu`. Panel memakai UI polos sementara. Diuji di Play Mode (7 November 2026): Bootstrap → MainMenu → **Run Baru** → `ShipPhase` dengan state `InShip` dan seed terbentuk, tanpa error. Pause, layar akhir, dan dialog konfirmasi belum diuji.

1. Buat layer 8 sampai 23 sesuai `PhysicsLayers.md` di **Project Settings > Tags and Layers**. `PhysicsMasks` mencatat kesalahan untuk layer yang belum ada.
2. Buat aset **Create > Survival > Game Balance Config**, lalu isi ke `GameManager.balanceAsset`.
3. Buat prefab `PersistentRoot`: objek akar dengan `RunLifecycle`, anak `Managers` (`managersRoot`) berisi `GameManager`, `UIFocusStack`, `UIManager`, `ObjectPooler`, `AsyncSceneLoader`, `RunSeedManager`, `WorldStateSave`, canvas persisten, dan `EventSystem`. Isi `RunLifecycle.orderedServices` sesuai urutan bootstrap di `RunLifecycle.md` (GameManager pertama).
4. Buat scene `Bootstrap` (kamera hitam dan `BootstrapEntry`), `MainMenu` (`MainMenuController`), `ShipPhase`, dan `IslandPhase`, lalu masukkan ke **Build Settings** dengan `Bootstrap` di indeks 0.

## 🧩 Isi Langkah 2

| File | Blueprint |
|---|---|
| `60-seconds/ShipTimerManager.cs` | `ShipTimerManager.md` |
| `60-seconds/ShipInteractable.cs` (`ShipInteractableKind`) | `ShipInteractable.md` |
| `60-seconds/ShipPlayerController.cs` (`ShipControlLevel`) | `ShipPlayerController.md` |
| `60-seconds/ShipHazardManager.cs`, `ShipHazardSchedule.cs` (`ShipHazardKind`, `ShipHazardEntry`, `WaterKeyframe`), `ShipCorridor.cs`, `ShipSwaySource.cs`, `ShipShove.cs`, `ShipZombiePoint.cs` | `ShipHazardManager.md` |
| `60-seconds/LifeboatExtraction.cs` (`ShipFlowState`), `RunSummaryPanel.cs`, `IslandArrival.cs` | `LifeboatExtraction.md` |
| `Island_Logic/IInteractable.cs` (`InteractContext`, `InteractBlockReason`), `PlayerInteractor.cs`, `InteractHighlight.cs` (`HighlightState`) | `IslandInteractable.md` (bagian interaksi yang dipakai Fase 1) |

Tambahan kecil di file Langkah 1: event `InteractedEvent`, `ShipHandChangedEvent`, `ShipHazardStageChangedEvent`, `NPCRescuedEvent` di `GameEvents.cs`; bagian `ship`, `shipHazard`, `interact`, dan `infection.startingVaccineDoses` di `GameBalanceSections.cs`; dan pemanggilan `IslandArrival.Apply()` di `RunLifecycle.EnterIslandPhaseRoutine`.

Langkah 2 memanggil kelas Langkah 3 (`InventoryManager`, `ItemData`, `ItemDatabase`, `InputManager`, `FirstPersonCamera`, `ICameraOffsetSource`, `SurvivalStats`, `CarryCapacity`) dengan nama dan signature dari blueprint. Kode Langkah 2 baru bisa dikompilasi setelah kode Langkah 3 digabung. Bagian yang belum diimplementasikan dicatat di "Catatan Implementasi (Langkah 2)" pada blueprint terkait dan ditandai TODO di kode.

### Penyiapan di Unity untuk Langkah 2 (belum dilakukan)

1. Scene `ShipPhase`: `ShipTimerManager` (teks timer TMP, `Image` vignette, `AudioSource` detak), `ShipHazardManager` (aset **Create > Survival > Ship Hazard Schedule**, isi lewat menu konteks "Isi jadwal awal"; koridor, titik zombie, bidang air, `lowerDeckBarrier`, `ShipSwaySource`), dan objek Sekoci (layer `Interactable`, collider bukan trigger) dengan `LifeboatExtraction`, `InteractHighlight`, dan `RunSummaryPanel` (id `RunSummary`, `pausesTime`, `releasesCursor`, `blocksGameplayInput`, `isLocked`).
2. Prefab `Player_Ship` (layer dan tag `Player`): `CharacterController` (height 1,8, radius 0,35, stepOffset 0,3, slopeLimit 45, skinWidth 0,08), `ShipPlayerController`, `PlayerInteractor` (isi `cameraTransform` dengan `WorldCamera`), `ShipShove`, `SurvivalStats`, dan rig `FirstPersonCamera`.
3. Pool di `ObjectPooler`: `ShipLoot_Generic` (prefab barang dengan `ShipInteractable`, `InteractHighlight`, `BoxCollider` bukan trigger, layer `Interactable`), `ShipNpc_Generic` (ukuran 3), dan `zombie_standar`.
4. Scene `IslandPhase`: objek `IslandArrival` di pantai selatan Region 1 dengan `landingPoint`, `lookTarget`, `npcOrigin`, dan `overflowOrigin`.

## 🧩 Isi Langkah 3

| File | Blueprint |
|---|---|
| `Core_Logic/InputManager.cs` (`GameInputAction`, `RebindResult`), `GameInputActions.cs` | `InputManager.md` |
| `Core_Logic/FirstPersonCamera.cs` (`ICameraOffsetSource`, `ViewmodelMode`, `ViewmodelPose`, `PlayerViewSettings`) | `FirstPersonCamera.md` |
| `Core_Logic/ItemData.cs` (`ItemType`, `HandUse`, `ToolClass`), `ItemDatabase.cs` | `ItemData.md` |
| `Core_Logic/ItemInstance.cs` (`ItemInstance`, `InventorySlot`, `Freshness`), `ItemUseSystem.cs`, `WorldItem.cs`, `WorldItemManager.cs` | `ItemUseSystem.md` |
| `Core_Logic/InventoryManager.cs` (`ShipHandEntry`) | `InventoryManager.md` |
| `Core_Logic/CarryCapacity.cs` | `CarryCapacity.md` |
| `Core_Logic/EquipmentManager.cs` (`EquipLock`, `EquipState`, `EquipmentSaveData`), `EquippableBehaviour.cs` (`IEquippable`, `ActionPhase`), `ConsumableEquippable.cs`, `FistBehaviour.cs` | `EquipmentManager.md` |
| `raft/SurvivalStats.cs` (`StaminaUse`, `VitalPenaltySource`, `VitalWarningFlags`) | `SurvivalStats.md`, `VitalsRules.md` |
| `Island_Logic/IslandPlayerController.cs` | `IslandPlayerController.md` |
| `Island_Logic/PlayerTraversal.cs` (`TraversalMode`, `SpeedModifierId`), `ClimbableSurface.cs` | `PlayerTraversal.md` |
| `Core_Logic/DamagePipelineContracts.cs` (`IDamageable`, `DamageInfo`, `DamageType`, `BodyPart`, `DeathReasonMap`) | `DamagePipeline.md` (pemilik Langkah 5) |
| `Core_Logic/SurfaceType.cs` | `SurfaceDetection.md` (pemilik Langkah 4) |
| `Core_Logic/RecipeIngredient.cs` (`RecipeIngredient`, `CraftingStationType`) | `CraftingStation.md` (pemilik Langkah 4) |
| `raft/DiseaseCure.cs` | `DiseaseManager.md` |
| `raft/SlotArrayOps.cs` | `StorageChest.md` |

Lima baris terakhir adalah kontrak milik Langkah lain yang sudah dibutuhkan Langkah 3. Pemiliknya tidak mendeklarasikannya ulang. Event baru Langkah 3 ada di bagian "Ditambah Langkah 3" pada `Core_Logic/GameEvents.cs`. Config yang diisi: `vitals` (tambahan #111), `input`, `equipment`, `items`, `camera`, `traversal`, `carry`, serta satu field di `hydration` (`exertionThirstMultiplier`). `ShipHandChangedEvent` dan `ship.handCapacity` yang dipakai `InventoryManager` ditulis Langkah 2, jadi branch Langkah 3 hanya terkompilasi setelah digabung dengan branch Langkah 2. Penyimpangan kecil dicatat di bagian "Catatan Implementasi (Langkah 3)" pada blueprint terkait.

### Penyiapan di Unity untuk Langkah 3 (belum dilakukan)

1. **Project Settings > Player > Active Input Handling** = `Input System Package (New)`. Di **Project Settings > Input System Package**: `Update Mode` = `Process Events In Dynamic Update`, `Background Behavior` = `Reset And Disable Non Background Devices`.
2. Tambahkan `InputManager` ke `Managers` di `PersistentRoot`, di urutan ke-2 `RunLifecycle.orderedServices` (setelah `GameManager`). `EventSystem` persisten memakai `InputSystemUIInputModule`.
3. Simpan semua aset `ItemData` di folder `Assets/Resources/Items/` (dibaca `ItemDatabase`). Item inti `Base_Kail`, `Base_Pistol`, dan `Base_Pisau` memakai `isCoreItem = true`.
4. Prefab pemain Fase 2: `CharacterController`, `IslandPlayerController`, `PlayerTraversal`, `SurvivalStats`, `InventoryManager`, `CarryCapacity`, `ItemUseSystem`, `EquipmentManager` (isi `fistBehaviour` dan `consumableBehaviour`), dan rig `FirstPersonCamera` (CameraRig > PitchPivot > OffsetPivot > WorldCamera dan ViewmodelCamera > ViewmodelRoot). `ViewmodelCamera` menjadi Overlay di Camera Stack URP.
5. Prefab `WorldItem` (layer `WorldItem`, `Rigidbody`, `MeshFilter`, `MeshRenderer`, `MeshCollider` convex) didaftarkan ke `ObjectPooler` dengan tag `WorldItem_Generic`. `WorldItemManager` diletakkan di scene `IslandPhase`.

## 🚀 Status Langkah

Langkah mengikuti tabel di `Claude.md` root (TAHAP 7). Perintahnya "Jalankan Langkah [X]".

| Langkah | Isi | Folder utama | Status |
|---|---|---|---|
| **1** | Kerangka `GameManager`, `UIManager`, `ObjectPooler`, `SaveSystem`, dan pola *State Management* inti | `Core_Logic/` | Terkompilasi, belum diuji di Play Mode |
| **2** | Fase 1: `ShipTimerManager`, `ShipInteractable`, `ShipPlayerController` | `60-seconds/` | Terkompilasi, belum diuji di Play Mode |
| **3** | `IslandPlayerController`, `SurvivalStats`, `InventoryManager`, `ItemData` | `Island_Logic/`, `raft/`, `Core_Logic/` | Terkompilasi, belum diuji di Play Mode |
| **4** | `DayNightDirector`, lapar dan infeksi, `BuildManager`, `ResourceNode`, `CraftingManager` | `left-4-dead/`, `raft/`, `Core_Logic/` | Terkompilasi, belum diuji di Play Mode |
| **5** | `EnemyAI`, spawn siang dan malam, gunplay, `MeleeCombatSystem`, `HitboxModifier` | `left-4-dead/`, `Island_Logic/`, `Core_Logic/` | Terkompilasi, belum diuji di Play Mode |
| **6** | `ZoneManager`, frostbite, jembatan kayu, barang berat, `WeatherManager` | `Island_Logic/` | Terkompilasi, belum diuji di Play Mode |
| **7** | *Sudden Event*, `LuckyMachine`, `DiaryManager`, alur menang dan kalah | `Island_Logic/`, `Core_Logic/` | Terkompilasi, belum diuji di Play Mode |

Isi kolom **Status** dengan salah satu: `Belum dimulai`, `Sedang ditulis`, `Ditulis, belum diuji di Unity`, atau `Diuji di Unity`. Jangan menulis `Diuji di Unity` sebelum *script* benar-benar dijalankan di Unity.

## 🔗 Hubungan dengan Proyek Unity

**Keputusan developer:** folder `Survival Island/` adalah root proyek Unity, dengan Unity 6 LTS. Folder ini (`Assets/LOGIC/`) adalah tempat semua *script*. `Survival Island/doc/` berada di luar `Assets/`, jadi tidak diimpor Unity.

### Membuka proyek pertama kali

1. Buka Unity Hub, pilih **Add project from disk**, lalu pilih folder `Survival Island/`.
2. Pilih editor Unity 6 LTS. Unity membuat `Packages/`, `ProjectSettings/`, dan `Library/` sendiri. `Library/` dan folder buatan lain diabaikan lewat `Survival Island/.gitignore`.
3. Lewat **Window > Package Manager**, pasang paket yang dipakai blueprint:

| Paket | Dipakai oleh |
|---|---|
| Input System | `InputManager` (#18) |
| AI Navigation | `NavMeshAgent`, `NavMeshSurface`, dan `NavMeshLink` di semua AI musuh, NPC, dan `BaseNavigation` |
| Universal RP | `PostProcessController`, `EnvironmentProfile` |
| Cinemachine | Impulse di `CameraShake`. Blueprint memakai nama kelas Cinemachine 2.x. Kalau yang terpasang 3.x, sesuaikan kodenya dan catat di `CameraShake.md` |
| TextMeshPro | Teks UI. Di Unity 6 sudah termasuk paket uGUI |

4. Commit `Packages/manifest.json`, `Packages/packages-lock.json`, dan `ProjectSettings/` setelah proyek pertama kali dibuka, supaya versi paket sama untuk semua orang.

**Catatan OneDrive:** proyek berada di folder OneDrive. Sinkronisasi OneDrive bisa mengunci file di `Library/` saat Unity menulis. Kalau muncul error akses file, jeda sinkronisasi OneDrive selama Unity terbuka.

## ✅ Sebelum Menambah Script

1. Baca `Survival Island/Claude.md` untuk aturan menulis kode.
2. Pastikan blueprint utama dan prasyarat Langkah itu sudah ada di `Survival Island/doc/blueprint/`.
3. Setelah menulis, perbarui tabel **Status Langkah** di atas.

## 🧩 Ditambahkan Langkah 4

Status: ditulis, belum dikompilasi di Unity. Sudah dicek dengan `dotnet build` (netstandard2.1, C# 9) terhadap DLL Unity 6000.3.10f1 dan DLL Input System, TextMeshPro, serta UGUI dari `Library/ScriptAssemblies` checkout utama: 0 error dan 0 warning pada konfigurasi Editor, rilis, dan `DEVELOPMENT_BUILD`. Pengecekan itu memakai stub `DamageResistance` (milik Langkah 5), jadi branch Langkah 4 sendirian baru terkompilasi di Unity setelah digabung dengan Langkah 5.

| File | Blueprint |
|---|---|
| `left-4-dead/DayNightDirector.cs` | `DayNightDirector.md` |
| `raft/InfectionSystem.cs` | `InfectionSystem.md` (#64) |
| `Core_Logic/SurfaceDetection.cs` (`SurfaceSample`), `SurfaceTag.cs`, `TerrainSurfaceSource.cs`, `SurfaceMapAsset.cs`, `SurfaceLayerMap.cs`, `SurfaceBakeTool.cs` (Editor) | `SurfaceDetection.md` (#86) |
| `raft/ResourceNode.cs`, `ResourceNodeData.cs` (`NodeType`, `NodeStatus`, `HitResult`, `ToolRule`, `RegrowRule`, `IRegrowable`), `ResourceDropGuard.cs`, `ResourceRespawn.cs` | `ResourceNode.md`, `ResourceRespawn.md` (#73) |
| `Core_Logic/CraftingRecipe.cs`, `RecipeDatabase.cs`, `CraftingManager.cs` (`CraftResult`, `CraftJob`, `CraftingSaveData`), `CraftingStation.cs`, `CraftingPanel.cs` | `CraftingManager.md`, `CraftingStation.md` (#77) |
| `Core_Logic/TechTreeManager.cs`, `TechNodeData.cs` (`TechUnlockTrigger`) | `TechTreeManager.md` |
| `raft/BuildableData.cs`, `BuildManager.cs` (`PlacementError`), `StructureNode.cs` (`GridKey`, `GridLayer`), `StructureRegistry.cs` (`StructureRecord`), `HammerEquippable.cs`, `BuildMenu.cs`, `NoBuildVolume.cs`, `IStructureHealth.cs` | `BuildManager.md`, `BuildMenu.md` (#79) |
| `Island_Logic/RegionType.cs` | `GLOSSARY.md` (pemilik `ZoneManager`, Langkah 6) |

Kontrak untuk Langkah lain: `RegionType` (Langkah 5 dan 6 tidak mendeklarasikan ulang), `IStructureHealth` (diterapkan `BaseHealth` Langkah 5), `StructureDamagedEvent` dan `StructureDestroyedEvent` (dikirim `BaseHealth`), serta `ResourceNode.ReceiveHit(ToolClass, Vector3, bool)` untuk `MeleeCombatSystem`.

Tambahan di file bersama: event di bagian "Ditambahkan Langkah 4" pada `Core_Logic/GameEvents.cs`; config `resource`, `infection`, `crafting`, `surface`, `build`, dan `sleep.maxGameHoursPerStep` di bagian "Ditambahkan Langkah 4" pada `Core_Logic/GameBalanceSections.cs` (deklarasi kelas di atas hanya ditambah kata `partial`). Hook kecil di file Langkah 3: `ItemUseSystem.CurrentTotalGameHours`, cek dan efek vaksin di `ItemUseSystem`, dan `PlayerTraversal.UpdateSurface`. `SurvivalStats.cs` tidak diubah: lapar dan haus sudah berjalan dari Langkah 3, dan infeksi memakai `SetPenalty`, `AddInfection`, serta `ReduceInfection`. Penyimpangan dicatat di bagian "Catatan Implementasi (Langkah 4)" pada blueprint terkait.

### Penyiapan di Unity untuk Langkah 4 (belum dilakukan)

1. `SurfaceDetection` di `Managers` pada `PersistentRoot`, lalu daftarkan di `RunLifecycle.orderedServices`.
2. Scene `IslandPhase`: objek manager berisi `DayNightDirector`, `ResourceRespawn`, `CraftingManager`, `TechTreeManager`, `BuildManager` (isi `buildables`, `ghostValidMat`, `ghostInvalidMat`, `structureRoot`), dan `StructureRegistry`. Canvas HUD: `CraftingPanel` (id `Crafting`) dan `BuildMenu` (id `BuildMenu`), keduanya `releasesCursor`, `blocksGameplayInput`, `closableByEsc`, tanpa `pausesTime`.
3. Pemain Fase 2: tambahkan `InfectionSystem` di samping `SurvivalStats`.
4. Tiap Terrain: `TerrainSurfaceSource` dengan aset **Create > Survival > Surface Layer Map**, lalu jalankan **Survival > Surface > Bake Terrain Terpilih**.
5. Aset `CraftingRecipe` di `Assets/Resources/Recipes/` (resep inti di `CraftingStation.md`), aset `BuildableData` (tabel `BuildMenu.md`), dan aset `ResourceNodeData` (tabel `ResourceRespawn.md`). Prefab struktur: collider di layer `Structure`, `StructureNode`, `BaseHealth` (Langkah 5), dan pivot lantai di permukaan atas. Item `Tool_Hammer` memakai `equipPrefab` dengan `HammerEquippable`.
6. Node resource: `ResourceNode` + `SaveIdTag`, layer `Obstacle`, dengan `standingVisual` dan `depletedVisual`. Prefab `WorldItem_Generic` mendapat `ResourceDropGuard`.

## 🧩 Ditambahkan Langkah 5

Langkah 5 (Pilar 3 "L4D"): AI musuh `NavMeshAgent`, spawn siang dan malam, gunplay raycast, melee, `HitboxModifier`, dan implementasi `DamagePipeline`. Status: **ditulis, belum dikompilasi di Unity**. Kode hanya dicek dengan `dotnet build` (netstandard2.1, C# 9) terhadap DLL UnityEngine 6000.3.10f1 dengan stub InputSystem/TMP/UGUI. Branch ini sendirian **tidak** terkompilasi, karena `BaseHealth` dan `MeleeCombatSystem` memakai `BuildableData`, `IStructureHealth`, `ResourceNode`, `RegionType`, `StructureDamagedEvent`, dan `StructureDestroyedEvent` milik Langkah 4 (branch `claude/eager-austin-b83a6f`). Gabungan uji kedua branch: 0 error dan 0 warning di Editor, rilis, dan `DEVELOPMENT_BUILD`. Belum ada yang dijalankan di Play Mode.

**Merge dengan Langkah 4:** konflik teks hanya di bagian akhir `Core_Logic/GameEvents.cs`, `Core_Logic/GameBalanceSections.cs`, `Assets/LOGIC/README.md`, dan `doc/blueprint/Core_Logic/DamagePipeline.md`. Ambil kedua sisi. Di dua file `.cs`, git menggabungkan `}` terakhir kedua sisi, jadi tambahkan satu `    }` penutup kelas terakhir Langkah 5 sebelum komentar "Ditambahkan Langkah 4".

| File | Blueprint |
|---|---|
| `Core_Logic/EnemyHealth.cs`, `BaseHealth.cs` (memakai `IStructureHealth` dan `BuildableData` dari Langkah 4), `DamageRouter.cs`, `DamageResistance.cs` (`DamageResistance`, `DamageResistanceTable`), `ExplosionDamage.cs` | `DamagePipeline.md` |
| `Island_Logic/HitboxModifier.cs` | `HitboxModifier.md` |
| `Island_Logic/MeleeCombatSystem.cs` | `MeleeCombatSystem.md` |
| `Island_Logic/PlayerPinnedState.cs` (`PinReleaseReason`, `IPinAttacker`) | `PlayerPinnedState.md` |
| `left-4-dead/EnemyAI.cs` (`AIState`, `SpeedSource`) | `EnemyAI.md` |
| `left-4-dead/EnemyData.cs`, `ZombieData.cs` (`SpecialZombieKind`), `SpecialZombieAI.cs`, `TribalBehavior.cs`, `TribalSnare.cs`, `MossBehavior.cs`, `MilitaryBehavior.cs`, `FrozenBehavior.cs`, `VillageBehavior.cs` | `SpecialZombieAI.md` |
| `left-4-dead/EnemyAttack.cs` (`AttackPhase`), `AttackData.cs`, `AttackCoordinator.cs` | `EnemyAttack.md` |
| `left-4-dead/EnemyHitReaction.cs` (`HitReactionKind`, `IHitReactive`), `CorpseBudget.cs`, `PlayerShove.cs` | `EnemyHitReaction.md` |
| `left-4-dead/AISensory.cs` (`AwarenessLevel`, `NoiseSourceKind`), `AISensoryHub.cs`, `NoiseEmitter.cs` | `AISensory.md` |
| `left-4-dead/ThreatSystem.cs` (`ThreatState`, `IThreatAgent`), `ITargetable.cs` (`TargetKind`), `ThreatProfile.cs`, `PlayerTargetable.cs` | `ThreatSystem.md` |
| `left-4-dead/HordeCrowdManager.cs` (`CrowdTier`, `ICrowdAgent`) | `HordeCrowdManager.md` |
| `left-4-dead/SpawnManager.cs` (`DespawnReason`, `SpawnOverride`), `RegionSpawnTable.cs` (`SpawnEntry`), `EnemyInstance.cs`, `AnimalTerritory.cs`, `DenPoint.cs` | `SpawnManager.md` |
| `left-4-dead/AIDirector.cs` (`DirectorPhase`, `ThreatKey`, `WaveModifiers`, `WaveRequest`) | `AIDirector.md` |
| `left-4-dead/SunlightExposure.cs` | `SunlightExposure.md` |
| `left-4-dead/WildlifeAI.cs` (`AnimalState`), `AnimalData.cs`, `AnimalCarcass.cs` | `WildlifeAI.md` |
| `left-4-dead/GunController.cs`, `GunCore.cs`, `WeaponData.cs` (`FireMode`, `WeaponStats`) | `GunController.md` |
| `left-4-dead/DayPhaseTracker.cs` | Pembaca siang/malam sementara dari `HourChangedEvent` (jam di `DayNightDirector.md`) |

### Perubahan kecil di file Langkah lain

- `Core_Logic/GameEvents.cs`: bagian baru "Ditambahkan Langkah 5" di akhir file (`NoiseEmittedEvent`, `PlayerDetectionChangedEvent`, `EnemySpawnedEvent`, `EnemyHitEvent`, `EnemyKilledEvent`, `EnemyAttackTelegraphEvent`, `DirectorPhaseChangedEvent`, `AmmoChangedEvent`, `WeaponFiredEvent`, `ShotResolvedEvent`, dan `RegionChangedEvent` milik `ZoneManager`). `NightfallEvent`, `StructureDamagedEvent`, dan `StructureDestroyedEvent` ditulis Langkah 4, dan `RegionType` juga ditulis Langkah 4.
- `Core_Logic/GameBalanceSections.cs`: bagian `damage`, `gun`, `enemyAttack`, `hitReaction`, `threat`, `stealth`, `wildlife`, `spawn`, `sunlight`, `specialZombie`, `aiDirector`, `crowd`, dan `pinned` diisi di bagian baru "Ditambahkan Langkah 5" di akhir file. Baris kelas kosong lamanya (`[Serializable] public class XBalance { }` beserta komentarnya) dihapus, karena C# tidak bisa menambah field ke kelas yang sudah dideklarasikan. Saat merge dengan Langkah 4, kedua bagian akhir file diambil.
- `Core_Logic/PhysicsQuery.cs`: `CollectUniqueDamageables` ditambahkan (dicadangkan Langkah 1 untuk Langkah 5).
- `Core_Logic/ItemData.cs`: field `weaponData` (dicadangkan Langkah 3 untuk Langkah 5).
- `raft/SurvivalStats.cs`: satu baris hook di `TakeDamage` langkah 3, memanggil `PlayerPinnedState.ModifyIncomingEnemyDamage`.
- Kolom **Status** Langkah 5 di tabel "Status Langkah" diubah menjadi `Ditulis, belum dikompilasi`.

### Belum diimplementasikan (sistem pendukung belum ada)

`BaseNavigation` (#78, diganti pemeriksaan jalur sementara), `AITraversal` (#94), `ZombieLair` (#112), `VillageZombieHiding`/`HidingSpot` (#63), `BushSpawnPoint` (#25), `RegionStreaming` (#88), `HordeManager` Blood Moon (#50), `DurabilitySystem`/`WeaponStatCalculator` (#113), `VFXManager` (#15), `AnimationController` (#12), `NPCGunUser` (#76), `DetectionMeterUI`, drop `LootTable` (#103), `AmbushPerch` dan Gagak terbang (#61). Semuanya ditandai TODO di kode dan dicatat di bagian "Catatan Implementasi (Langkah 5)" pada blueprint terkait. `ShipHazardManager` (Langkah 2) belum memanggil `EnemyAI.SetTarget`/`WakeUp`, jadi zombie kapal belum mengejar.

### Penyiapan di Unity untuk Langkah 5 (belum dilakukan)

1. Scene `IslandPhase`: satu objek manager berisi `SpawnManager` (isi 4 aset **Create > Survival > Region Spawn Table** sesuai tabel GDD bagian 5), `AIDirector`, `ThreatSystem`, `AISensoryHub`, `HordeCrowdManager`, dan `AttackCoordinator`. `NavMeshSurface` di-bake untuk pulau. `ShipPhase` cukup `HordeCrowdManager` dan `AttackCoordinator`.
2. Prefab pemain Fase 2: tambahkan `PlayerTargetable`, `PlayerPinnedState`, `PlayerShove`, dan `NoiseEmitter` (`isPlayerMovement = true`). Prefab pemain kapal: `PlayerTargetable`.
3. Prefab zombie (tag `Zombie`, kapsul di layer `Enemy`): `NavMeshAgent`, `EnemyHealth` (aset **Survival > Zombie Data**), `EnemyInstance`, `EnemyAI`, `EnemyAttack` (aset **Survival > Attack Data**), `EnemyHitReaction`, `AISensory`, `SunlightExposure`, dan turunan `SpecialZombieAI` sesuai varian. Collider tulang di layer `EnemyHitbox` (bukan trigger) dengan `HitboxModifier` Head/Torso/Leg, dan `Rigidbody` kinematik untuk ragdoll.
4. Prefab hewan (tag `Animal`): sama, tetapi `WildlifeAI` (aset **Survival > Animal Data**) menggantikan `EnemyAI`, tanpa `AISensory` dan `SunlightExposure`. Hewan yang dikuliti memakai `AnimalCarcass` dengan collider anak di layer `Interactable`.
5. Pool di `ObjectPooler`, tag = `enemyId`: `zombie_standar` 50, `zombie_desa` 30, varian zombie lain 12, `hewan_serigala` dan `hewan_anjing` 8, `hewan_gagak` 16, hewan lain 4, serta `Tribal_Snare` (layer `Projectile`, komponen `TribalSnare`).
6. Aset **Survival > Weapon Data** untuk `Base_Pistol` (nilai di `GunController.md`), diisi ke `ItemData.weaponData`. `equipPrefab` Pistol memakai `GunController` dengan `ParticleSystem` muzzle flash, dan `equipPrefab` Pisau/Kapak/Beliung memakai `MeleeCombatSystem`.
7. Aset **Survival > Threat Profile**: `Threat_Zombie` dan `Threat_Animal` (nilai di `ThreatSystem.md`). Letakkan `DenPoint` dan `AnimalTerritory` di scene region.

## 🧩 Ditambahkan Langkah 6

Langkah 6 (sistem region): `ZoneManager`, cuaca, suasana region, frostbite Region 3, jembatan kayu Region 2, dan barang berat Region 4. Status: **ditulis, belum dikompilasi di Unity**. Kode hanya dicek dengan `dotnet build` (netstandard2.1, C# 9) terhadap DLL Unity 6000.3.10f1 dan DLL Input System, TextMeshPro, serta UGUI dari `Library/ScriptAssemblies` checkout utama, tanpa stub: 0 error dan 0 warning pada konfigurasi Editor, rilis, dan `DEVELOPMENT_BUILD`. Belum ada yang dijalankan di Play Mode. File `.meta` dibuat manual dengan GUID acak.

| File | Blueprint |
|---|---|
| `Island_Logic/ZoneManager.cs`, `ZoneTracker.cs` | `ZoneManager.md`, `EnvironmentProfile.md` "Zona bertumpuk" (#92) |
| `Island_Logic/WeatherType.cs`, `RegionalWeather.cs` (`WeatherEffects`, `WeatherOverride`, `WeatherOverrideHandle`), `RegionWeatherTable.cs` (`WeatherWeight`) | `RegionalWeather.md` (#115) |
| `Island_Logic/WeatherManager.cs` | `WeatherManager.md` (#115) |
| `Island_Logic/EnvironmentDirector.cs` (`EnvEventId`, `WeatherEnvironmentLayer`, `EnvironmentEventLayer`), `RegionEnvironmentProfile.cs` | `EnvironmentProfile.md` (#92) |
| `Island_Logic/WarmthSystem.cs`, `TorchController.cs` | `WarmthSystem.md` (#65) |
| `Island_Logic/RegionGateSystem.cs` (`GateKind`, `GateRule`), `BridgeSite.cs`, `GateBlocker.cs`, `RavineRecoveryVolume.cs` | `RegionGateSystem.md` (#68) |
| `Island_Logic/HeavyItemSystem.cs` (`PartState`, `PartRecord`, `DropReason`), `HeavyItemData.cs`, `HeavyItem.cs`, `PartSpawnPointTable.cs` (`PartSpawnPoint`) | `HeavyItemSystem.md` (#69) |

Kontrak yang dipakai apa adanya: `RegionType` (Langkah 4), `RegionChangedEvent` (Langkah 5), `WarmthChangedEvent` dan `HeavyItemChangedEvent` (Langkah 3), serta `SpeedModifierId` (Langkah 3). Kontrak baru untuk Langkah 7: `WeatherType`, `RegionalWeather.PushOverride`/`PopOverride` (Badai Tropis), `EnvironmentDirector.PushEventLayer`/`PopEventLayer` (erupsi, Blood Moon), `HeavyItemSystem.MarkInstalled`/`InstalledCount`/`TryGetNearestPart`, `RegionGateSystem.IsUnlocked`/`WoodRemaining`/`HasBridgeBlueprint`, dan `RegionGateSystem.HandleDiaryChapter(int)` sampai `DiaryChapterUnlockedEvent` ada.

### Perubahan kecil di file Langkah lain

- `Core_Logic/GameEvents.cs`: bagian baru "Ditambahkan Langkah 6" di akhir file (`WeatherChangedEvent`, `RegionUnlockedEvent`, `BridgeProgressChangedEvent`).
- `Core_Logic/GameBalanceSections.cs`: bagian baru "Ditambahkan Langkah 6" di akhir file (tambahan `region` #68, tambahan `weather` #115, `warmth`, `heavyItem`, `environment`). Lima deklarasi kelas di atas (`RegionBalance`, `WeatherBalance`, `WarmthBalance`, `HeavyItemBalance`, `EnvironmentBalance`) hanya ditambah kata `partial`, sama seperti cara Langkah 4.
- `Island_Logic/IslandPlayerController.cs`: `Teleport` memanggil `ZoneManager.ResyncAll(position)` (satu baris, TODO Langkah 3).
- `Island_Logic/PlayerInteractor.cs`: `BuildContext` mengisi `IsCarryingHeavyItem` dari `HeavyItemSystem.IsCarrying` (TODO Langkah 2).
- `left-4-dead/FrozenBehavior.cs`: ledakan Zombie Beku memanggil `WarmthSystem.ApplyColdBurst` untuk pemain dalam radius dengan garis pandang bebas (TODO Langkah 5).
- Kolom **Status** Langkah 6 di tabel "Status Langkah" diubah menjadi `Ditulis, belum dikompilasi`.

Kecepatan setengah, Sprint dan Jump mati, regenerasi stamina, dan kunci senjata saat membawa barang berat sudah berjalan di `IslandPlayerController`, `SurvivalStats`, dan `EquipmentManager` lewat `HeavyItemChangedEvent`, jadi file itu tidak diubah. Frostbite (Stamina dulu, lalu HP) sudah dihitung `SurvivalStats` dari `WarmthChangedEvent.isFreezing`.

### Belum diimplementasikan (sistem pendukung belum ada)

`RegionStreaming` dan `RegionBoundaryGuard` (#88, diganti `ZoneTracker.Active` dan posisi aman di `RegionGateSystem`), `SwimStateChangedEvent`/`SwimmingController` (#13, diganti `PlayerTraversal.Mode == Swimming`), `InteriorZone` (#112), `CampfireManager` (#44), `PostProcessController` (#23), `SettingsManager.brightness` (#19), `VFXManager` (#15), `NotificationManager` (#22), `CompassSystem` (#30), `LoreHintGrantedEvent` (#100), `LightSource` (#98), `DevConsole` (#56), `DiaryChapterUnlockedEvent` (#81, Langkah 7), isi `RunSeedManager.BuildLayout` untuk suku cadang (#89, diganti pemilihan cadangan di `HeavyItemSystem`), aksi `Pakai` pakaian hangat di UI inventori, `NavMeshLink` seam, dan alat Editor `ZoneCoverageAudit`. Semuanya ditandai TODO di kode dan dicatat di bagian "Catatan Implementasi (Langkah 6)" pada blueprint terkait.

### Penyiapan di Unity untuk Langkah 6 (belum dilakukan)

1. Scene `IslandPhase`: satu objek manager berisi `RegionalWeather` (4 aset **Create > Survival > Regional Weather Table**, isi lewat menu konteks "Isi nilai awal region"), `WeatherManager` (anak kamera: `rainParticles`, `stormLeavesParticles`, dua `AudioSource` di grup mixer `Ambient`), `EnvironmentDirector` (4 aset **Create > Survival > Region Environment Profile** dengan menu konteks "Isi nilai awal region", Directional Light matahari, material `SkyboxBlended` opsional), `RegionGateSystem` (isi `rules[].barriers` dengan `GateBarrier` dan `GateBlocker`), dan `HeavyItemSystem` (5 aset **Create > Survival > Heavy Item**, aset **Create > Survival > Part Spawn Point Table** minimal 10 titik, `carryAnchor` di depan kamera pemain).
2. Trigger zona: `BoxCollider` raksasa (IsTrigger, layer `ZoneTrigger`) dengan `ZoneManager` per region, bertumpuk minimal 10 meter di seam, dari dasar laut sampai puncak.
3. Pemain Fase 2: tambahkan `WarmthSystem` di samping `SurvivalStats`. Item `Tool_Torch` memakai `equipPrefab` dengan `TorchController` (`Light`, `ParticleSystem`). Item `Gear_WarmClothes` memakai `isWarmClothing = true`.
4. Region 1: `BridgeSite` di tepi jurang `S1` (layer `Interactable`, 20 ruas, `deckCollider` di layer `Structure`, papan TMP), `NoBuildVolume` di sekitarnya, dan `RavineRecoveryVolume` (trigger layer `ZoneTrigger`) 5 meter di atas dasar jurang.
5. Prefab suku cadang: layer dan tag `HeavyItem`, `HeavyItem`, `Rigidbody`, dan collider bukan trigger. Pool `Fx_Snow`, `Fx_Blizzard`, `Fx_AshLight`, `Fx_Spores`, `Fx_SnowDust`, dan `Fx_Mist` di `ObjectPooler`.

## 🧩 Ditambahkan Langkah 7

Langkah 7: *Sudden Event* (`DisasterManager`, Fireball Region 1 dari `ObjectPooler`, `MajorDisasterStartedEvent`), `LuckyMachine` (kuota musuh per region lewat kandang, mode gratis 30 detik saat bencana besar, hadiah gacha), `DiaryManager` (panel teks yang menjeda lewat `UIFocusStack`), dan alur menang dan kalah (`RescueEventManager`, `DeathCinematic`). Status: **ditulis, belum dikompilasi di Unity.** Kode hanya dicek dengan `dotnet build` (netstandard2.1, C# 9) terhadap DLL UnityEngine 6000.3.10f1 serta `Unity.InputSystem`, `Unity.TextMeshPro`, dan `UnityEngine.UI` dari `Library/ScriptAssemblies` checkout utama: 0 error dan 0 warning di Editor, rilis, dan `DEVELOPMENT_BUILD`. Branch ini sendirian butuh `HeavyItemSystem`, `HeavyItem`, `PartRecord`, dan `PartState` milik Langkah 6, jadi baru terkompilasi setelah digabung. Uji gabung dengan branch Langkah 6 (`claude/sleepy-cerf-920dd6`, df3176d06) di worktree sementara: konflik teks hanya di akhir `GameEvents.cs`, `GameBalanceSections.cs`, dan file README ini. Setelah kedua sisi diambil, `dotnet build` tanpa stub memberi 0 error dan 0 warning di ketiga konfigurasi. Belum ada yang dijalankan di Play Mode.

| File | Blueprint |
|---|---|
| `Island_Logic/DisasterManager.cs` (`DisasterPhase`), `DisasterData.cs`, `DisasterScheduler.cs`, `Fireball.cs`, `HazardImpactMarker.cs` | `DisasterManager.md` (#70) |
| `Island_Logic/LuckyMachine.cs`, `LuckyMachineCage.cs`, `LuckyMachineData.cs` (`LuckyRewardKind`, `LuckyTier`, `CageGateState`, `SacrificeQuota`), `LuckyRewardData.cs`, `WeaponModifierData.cs`, `LuckyRewardLedger.cs`, `LuckyMachinePanel.cs` | `LuckyMachine.md` (#72) |
| `Island_Logic/DiaryManager.cs`, `DiaryPanel.cs`, `DiaryArchivePanel.cs` | `DiaryManager.md` (#81) |
| `Island_Logic/DiaryEntry.cs` (`DiaryRewardKind`, `DiaryReward`) | `ObjectiveManager.md` (pemilik `ObjectiveManager`, belum ditulis) |
| `Island_Logic/RescueEventManager.cs` (`RescueState`), `HelicopterFrame.cs`, `PartInstallZone.cs`, `CockpitInteractable.cs`, `HeavyItemBridge.cs` | `RescueEventManager.md` (#14) |
| `Core_Logic/DeathCinematic.cs` (`DeathStyle`, `DeathProfile`) | `DeathCinematic.md` (#53) |

### Perubahan kecil di file Langkah lain

- `Core_Logic/GameEvents.cs`: bagian baru "Ditambahkan Langkah 7" di akhir file (`MajorDisasterStartedEvent`, `BloodMoonStartedEvent` milik `HordeManager`, `EnemySacrificedEvent`, `LuckyMachineFreeWindowChangedEvent`, `LuckyMachineSpinResolvedEvent`, `HelicopterPartInstalledEvent`, `HelicopterReadyEvent`, `HelicopterEngineChangedEvent`, dan `CinematicStateChangedEvent`).
- `Core_Logic/GameBalanceSections.cs`: bagian `disaster`, `luckyMachine`, `rescue`, dan `deathCinematic` diisi di bagian baru "Ditambahkan Langkah 7" di akhir file. Empat deklarasi kelas kosongnya hanya ditambah kata `partial`, cara yang sama dengan Langkah 4.
- `Core_Logic/RunSeedManager.cs` (Langkah 1): field `disasterData`, isi `BuildLayout` untuk blok pertama jadwal bencana, dan `EnsureDisasterScheduleThrough`. Keduanya adalah TODO Langkah 7 yang ditinggalkan Langkah 1.
- `Core_Logic/RunLifecycle.cs` (Langkah 1): satu baris di `RunEndSequence` langkah 4 yang memutar `DeathCinematic.Instance.Play(reason)` saat mati. `GameManager.cs` tidak diubah, karena `TriggerWinSequence` dan `TriggerGameOver` sudah ada.

### Titik bentrok saat merge dengan Langkah 6

- `HeavyItemBridge.cs` adalah satu-satunya file yang memanggil `HeavyItemSystem` dan `HeavyItem`, dan sudah memakai API kode Langkah 6 (`PartCount`, `GetRecord`, `HeavyItem.Index`).
- `GameEvents.cs` dan `GameBalanceSections.cs`: ambil kedua bagian akhir. Git menggabungkan `}` terakhir kedua sisi, jadi tambahkan satu `    }` penutup struct atau kelas terakhir Langkah 7 sebelum komentar "Ditambahkan Langkah 6".
- Tabel **Status Langkah** di file README ini: pakai status baru untuk Langkah 6 dan Langkah 7, lalu ambil kedua bagian akhir.
- Langkah 6 tidak menulis event helikopter, `MajorDisasterStartedEvent`, atau `BloodMoonStartedEvent`, jadi tidak ada deklarasi ganda.

### Belum diimplementasikan (sistem pendukung belum ada)

`ObjectiveManager` (#81, sumber bab terbuka dan buku Diary), `NarrativeSequencer` (#99, ending menang), `StatusEffectManager` (#20, buff permanen), `RegionStateStore` (#88, sementara mesin langsung `Register` ke `WorldStateSave`), `IslandInteractable` (#17), `HordeManager` (#50), `NotificationManager` (#22), `LocalizationManager` (#104), `AudioManager` (#114), `CameraShake` (#24), `VFXManager` (#15), `EnvironmentDirector` (#92), `RegionalWeather` (#115), `RemoteBaseSimulation` (#90), `FireSystem` (#97), `BuildingIntegrity` (`survivesEarthquake`, #47), dan `NPCRoster` (#76). Semuanya ditandai TODO di kode dan dicatat di bagian "Catatan Implementasi (Langkah 7)" pada blueprint terkait.

### Penyiapan di Unity untuk Langkah 7 (belum dilakukan)

1. Aset **Create > Survival > Disaster Data** untuk `Eruption`, `Earthquake`, dan `TropicalStorm` (nilai di `DisasterManager.md`), lalu isi ke `RunSeedManager.disasterData` di `PersistentRoot` dan ke `DisasterManager.disasterData`.
2. Scene `IslandPhase`: objek manager berisi `DisasterManager`, `LuckyRewardLedger`, `RescueEventManager`, dan `DiaryManager` (isi `diaryPanel` dan `archivePanel`). Canvas: `DiaryPanel` (id `Diary`) dan `DiaryArchivePanel` (id `DiaryArchive`) dengan `pausesTime`, `releasesCursor`, `blocksGameplayInput`, dan `closableByEsc`; `LuckyMachinePanel` (id `LuckyMachine`) sama tetapi tanpa `pausesTime`. Objek panel aktif saat scene dimuat, supaya `Awake` mengisi `Instance`.
3. Pool di `ObjectPooler`: `Hazard_Fireball` (layer `Projectile`, komponen `Fireball`) dan `Hazard_ImpactMarker` (komponen `HazardImpactMarker`).
4. Satu prefab mesin per region: `LuckyMachine` + `SaveIdTag` (aset **Create > Survival > Lucky Machine Data** dengan kuota di `LuckyMachine.md`, hadiah **Survival > Lucky Reward** dan **Survival > Weapon Modifier**), layer `Interactable`; anak `LuckyMachineCage` dengan `BoxCollider` trigger 6 x 4 x 4 m di layer `ZoneTrigger`, `Rigidbody` kinematic, objek gerbang dengan `NavMeshObstacle` carve, dan `vacuumPoint`. Isi `duplicateModifierFallback` dengan `lucky_ammo`.
5. `Region3_Snow`: `HelicopterFrame` dengan bagian per `partId`, anak `PartInstallZone` (trigger `ZoneTrigger`), dan `CockpitInteractable` (layer `Interactable`).
6. Prefab pemain Fase 1 dan Fase 2: `DeathCinematic` (isi `cameraRig` dan, kalau ada, `PlayerBodyRig` nonaktif dengan tulang `Rigidbody` kinematic dan `focusBone`).
7. Aset **Create > Survival > Diary Entry** untuk Bab 1 sampai 3, diisi ke `DiaryArchivePanel.chapters`.
