# 🧩 Survival Island - Kode Gabungan (LOGIC)

Folder ini adalah tempat **kode gabungan C#** Survival Island. Semua *script* yang ditulis dari blueprint di `Survival Island/doc/blueprint/` disimpan di sini, sesuai `README.md` di root (TAHAP 6).

**Status:** Langkah 1 (fondasi inti), Langkah 2 (Fase 1 kapal), dan Langkah 3 (pemain, status tubuh, inventori) sudah ditulis dan digabung. Proyek Unity 6 LTS di `Survival Island/` sudah dibuka di Unity; kode gabungan Langkah 1-3 terkompilasi tanpa error di Unity 6000.3.10f1 (6 November 2026), tetapi belum ada yang dijalankan di Play Mode.

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

### Penyiapan di Unity untuk Langkah 1 (belum dilakukan)

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
| **4** | `DayNightDirector`, lapar dan infeksi, `BuildManager`, `ResourceNode`, `CraftingManager` | `left-4-dead/`, `raft/`, `Core_Logic/` | Belum dimulai |
| **5** | `EnemyAI`, spawn siang dan malam, gunplay, `MeleeCombatSystem`, `HitboxModifier` | `left-4-dead/`, `Island_Logic/` | Belum dimulai |
| **6** | `ZoneManager`, frostbite, jembatan kayu, barang berat, `WeatherManager` | `Island_Logic/` | Belum dimulai |
| **7** | *Sudden Event*, `LuckyMachine`, `DiaryManager`, alur menang dan kalah | `Island_Logic/`, `Core_Logic/` | Belum dimulai |

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
