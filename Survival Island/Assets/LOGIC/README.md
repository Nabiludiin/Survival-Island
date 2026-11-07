# 🧩 Survival Island - Kode Gabungan (LOGIC)

Folder ini adalah tempat **kode gabungan C#** Survival Island. Semua *script* yang ditulis dari blueprint di `Survival Island/doc/blueprint/` disimpan di sini, sesuai `README.md` di root (TAHAP 6).

**Status:** Langkah 1 sampai 7 sudah ditulis dan digabung ke `main`. Sistem pendukung (semua blueprint lain yang dipakai) ditulis di branch `claude/elegant-liskov-301aa5`, lihat bagian "Ditambahkan: sistem pendukung"; belum dikompilasi di Unity. Proyek Unity 6 LTS di `Survival Island/` sudah dibuka di Unity; kode gabungan Langkah 1-3 terkompilasi tanpa error di Unity 6000.3.10f1 (6 November 2026), begitu juga gabungan Langkah 4-5 dan Langkah 6-7 (7 November 2026), tetapi belum ada yang dijalankan di Play Mode.

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

Sudah dibuat lewat menu **Survival Island > Setup > TMP Essential Resources** lalu **Survival Island > Setup > Langkah 1 - Fondasi** (`Assets/Editor/Langkah1Setup.cs`, boleh dijalankan ulang). Hasilnya `Assets/Data/GameBalanceConfig.asset`, `Assets/Prefabs/PersistentRoot.prefab` (sejak sistem pendukung digabung: 19 service sesuai `RunLifecycle.md`, ditambah `SurfaceDetection` setelah `ObjectPooler`; lihat "Tambahan untuk `PersistentRoot`"), scene `Bootstrap`, dan scene `MainMenu`. Panel memakai UI polos sementara. Diuji di Play Mode (7 November 2026): Bootstrap → MainMenu → **Run Baru** → `ShipPhase` dengan state `InShip` dan seed terbentuk, tanpa error. Pause, layar akhir, dan dialog konfirmasi belum diuji.

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

### Penyiapan di Unity untuk Langkah 2 (sudah dilakukan untuk ShipPhase)

Dibuat lewat menu **Survival Island > Setup > Langkah 2 - ShipPhase** (`Assets/Editor/Langkah2Setup.cs`, boleh dijalankan ulang, jalankan setelah menu Langkah 1). Nomor 1 sampai 3 di bawah sudah dibuat. Nomor 4 (`IslandArrival` di `IslandPhase`) dibuat lewat menu Langkah 3. Isinya:

- **Data:** 7 `ItemData` isi `Loot_Ship` di `Assets/Resources/Items/` (`Food_Can`, `Ammo_9mm`, `Drink_CleanWater`, `Med_Bandage`, `Med_Medkit`, `Med_Vaccine`, `Item_Battery`). Angka diambil dari `ItemUseSystem.md`, `ShipInteractable.md`, `CarryCapacity.md`, dan `RegionLootTable.md`. Ada juga `Assets/Data/Ship/Loot_Ship.asset`, `ShipHazardSchedule.asset` (diisi lewat "Isi jadwal awal"), `Assets/Data/Enemies/zombie_standar.asset` dan `Zombie_Standard_Hit.asset`.
- **Prefab:** `Assets/Prefabs/Ship/Player_Ship` (rig `FirstPersonCamera`, `ViewmodelCamera` Overlay di Camera Stack, `CameraShake` di rig, `InventoryManager`, `CarryCapacity`), `ShipLoot_Generic`, `ShipNpc_Generic`, dan `Assets/Prefabs/Enemies/zombie_standar` (`NavMeshAgent`, `EnemyAI`, `EnemyHealth`, `EnemyAttack`, `AISensory`, 3 hitbox `EnemyHitbox`).
- **Pool di `PersistentRoot`:** `ShipLoot_Generic` (12, jumlah titik loot), `ShipNpc_Generic` (3), `zombie_standar` (50). Menu Langkah 1 sekarang mempertahankan pool yang sudah terdaftar.
- **Scene `ShipPhase`:** geladak greybox tiga dek (Y 0, 4, 8) yang miring tetap `shipHazard.staticTiltDegrees`, `NavMeshSurface` yang sudah dibake (`Assets/Scenes/ShipPhase_NavMesh.asset`), koridor `Corridor_MidPort` dan `Stair_LowerAft`, tangga `Stair_LowerFore`, `lowerDeckBarrier`, 10 `ShipZombiePoint` (`mid_45` memakai `wakeOnAlarm`), 12 titik `LootSpawnerSystem`, 3 titik NPC, sekoci di dek atas (`LifeboatExtraction`, `InteractHighlight`), `RunSummaryPanel`, HUD polos (`HUDManager`), dan `ShipTimerManager`. Bentuk geladak dan letak titik hanya greybox, bukan desain level final.

**Hasil uji Play Mode (7 November 2026):** Bootstrap → MainMenu → **Run Baru** → `ShipPhase`, tanpa error selain `AudioMixer` kosong.
- Timer berjalan dari 60 setelah jeda pra-game.
- 6 sampai 8 loot muncul dari 12 titik.
- Pengambilan lewat `ShipInteractable.Interact` dan `PlayerInteractor` mengisi tangan sesuai `shipPickupAmount`.
- Setor ke sekoci tercatat di HUD.
- Koridor runtuh pada T=30 dan T=15, air naik, `lowerDeckBarrier` aktif sekitar T=13, dan zombie kapal muncul.
- Berangkat lebih awal maupun timer habis di area aman membuka `RunSummary`. **Lanjut** memuat `IslandPhase` (`OnIsland`) dengan `savedShipLoot` terisi.

Pilihan data yang tidak tertulis di blueprint: `ItemType` `Ammo_9mm` = `Material`, `Item_Battery` = `Consumable` (karena `isConsumable`), dan `Med_Bandage` memakai `maxStack` 1 (nilai awal `ItemData`, karena tidak ada di tabel). Mohon dikonfirmasi.

Belum diuji: NPC kapal (butuh aset `NPCData` dan `RunLayoutSources`), zombie menyerang pemain, dan kematian di kapal. Error `IslandArrival tidak ada` hilang setelah menu Langkah 3.

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

### Penyiapan di Unity untuk Langkah 3 (sudah dilakukan untuk IslandPhase greybox)

Dibuat lewat menu **Survival Island > Setup > Langkah 3 - IslandPhase** (`Assets/Editor/Langkah3Setup.cs`, boleh dijalankan ulang, jalankan setelah menu Langkah 1 dan 2). Isinya:

- **Item inti:** `Base_Kail` (`Tool`, `TwoHand`), `Base_Pistol` (`Weapon`, `OneHand`, `WD_Base_Pistol` dengan angka dari tabel `GunController.md`), dan `Base_Pisau` (`Weapon`, `OneHand`, `toolClass` `Knife`). Ketiganya `isCoreItem` dan tidak bisa dibuang. `maxDurability` dibiarkan -1 karena blueprint belum memberi angka. `Base_Kail` belum punya viewmodel, karena `FishingSystem` butuh data ikan dari Langkah 4.
- **Prefab:** viewmodel greybox `Viewmodel_Pistol` (`GunController`) dan `Viewmodel_Pisau` (`MeleeCombatSystem`), dengan pose dari `FirstPersonCamera.md`. Ada juga `Assets/Prefabs/Items/WorldItem_Generic`, yang dipasang sebagai pool `WorldItem_Generic` (128, `Grow`, maksimum 512), dan `Assets/Prefabs/Player/Player_Island` (nomor 4 di bawah, ditambah `PlayerInteractor`, `FistBehaviour`, dan `ConsumableEquippable` di `ViewmodelRoot`).
- **Scene `IslandPhase`:** pantai datar greybox (layer `Terrain`, dua batu `Obstacle`), `IslandArrival` di pantai selatan, `WorldItemManager`, pemain, dan HUD polos dengan 5 bar vital, prompt, peluru, dan `HotbarView` 8 slot ditambah tangan kiri. Ini lantai sementara, bukan peta pulau.

**Hasil uji Play Mode (7 November 2026):** Run Baru → kapal → setor 3 barang → berangkat → `IslandPhase`.
- `IslandArrival.Apply` memasukkan item inti, 2 `Med_Vaccine` awal, dan barang kapal ke inventori.
- `hasArrivedOnIsland` bernilai true.
- Pistol bisa dipegang (HUD peluru "10 / 12" setelah dua tembakan).
- Menu Jeda menyimpan senjata.
- `WorldItem` bisa dijatuhkan, jatuh ke tanah, lalu diambil lagi.
- Error yang tersisa hanya `AudioMixer` kosong.

Belum diuji: Pisau dan tinju mengenai musuh, makan dan minum, `SurfaceDetection` di lantai non-Terrain, dan save lalu lanjutkan di pulau. Save lama yang dibuat sebelum `IslandArrival` ada akan dilanjutkan tanpa item, karena `Apply` tidak dijalankan di jalur Lanjutkan. Pemain perlu memulai Run Baru.

Daftar penyiapan dari blueprint (sudah dicakup menu di atas, kecuali pengaturan Input System di nomor 1 yang dibuat sebelumnya):

1. **Project Settings > Player > Active Input Handling** = `Input System Package (New)`. Di **Project Settings > Input System Package**: `Update Mode` = `Process Events In Dynamic Update`, `Background Behavior` = `Reset And Disable Non Background Devices`.
2. Tambahkan `InputManager` ke `Managers` di `PersistentRoot`, di urutan ke-2 `RunLifecycle.orderedServices` (setelah `GameManager`). `EventSystem` persisten memakai `InputSystemUIInputModule`.
3. Simpan semua aset `ItemData` di folder `Assets/Resources/Items/` (dibaca `ItemDatabase`). Item inti `Base_Kail`, `Base_Pistol`, dan `Base_Pisau` memakai `isCoreItem = true`.
4. Prefab pemain Fase 2: `CharacterController`, `IslandPlayerController`, `PlayerTraversal`, `SurvivalStats`, `InventoryManager`, `CarryCapacity`, `ItemUseSystem`, `EquipmentManager` (isi `fistBehaviour` dan `consumableBehaviour`), dan rig `FirstPersonCamera` (CameraRig > PitchPivot > OffsetPivot > WorldCamera dan ViewmodelCamera > ViewmodelRoot). `ViewmodelCamera` menjadi Overlay di Camera Stack URP.
5. Prefab `WorldItem` (layer `WorldItem`, `Rigidbody`, `MeshFilter`, `MeshRenderer`, `MeshCollider` convex) didaftarkan ke `ObjectPooler` dengan tag `WorldItem_Generic`. `WorldItemManager` diletakkan di scene `IslandPhase`.

## 🚀 Status Langkah

Langkah mengikuti tabel di `Claude.md` root (TAHAP 7). Perintahnya "Jalankan Langkah [X]".

| Langkah | Isi | Folder utama | Status |
|---|---|---|---|
| **1** | Kerangka `GameManager`, `UIManager`, `ObjectPooler`, `SaveSystem`, dan pola *State Management* inti | `Core_Logic/` | Diuji di Unity: alur Bootstrap → MainMenu → Run Baru (pause, layar akhir, dan save/lanjutkan belum diuji) |
| **2** | Fase 1: `ShipTimerManager`, `ShipInteractable`, `ShipPlayerController` | `60-seconds/` | Diuji di Unity: ShipPhase greybox sampai IslandPhase (NPC kapal dan mati di kapal belum diuji) |
| **3** | `IslandPlayerController`, `SurvivalStats`, `InventoryManager`, `ItemData` | `Island_Logic/`, `raft/`, `Core_Logic/` | Diuji di Unity: tiba di pantai, inventori, hotbar, WorldItem (makan/minum dan pertarungan belum diuji) |
| **4** | `DayNightDirector`, lapar dan infeksi, `BuildManager`, `ResourceNode`, `CraftingManager` | `left-4-dead/`, `raft/`, `Core_Logic/` | Diuji di Unity: jam, panen, crafting, bangun, dan restriksi pantai (menu B, demolish, dan tumbuh ulang belum diuji) |
| **5** | `EnemyAI`, spawn siang dan malam, gunplay, `MeleeCombatSystem`, `HitboxModifier` | `left-4-dead/`, `Island_Logic/`, `Core_Logic/` | Diuji di Unity: zombie standar (terbakar, mengendap, mengejar, menyerang), hitbox, serigala dan babi (varian khusus, terkam, Blood Moon belum diuji) |
| **6** | `ZoneManager`, frostbite, jembatan kayu, barang berat, `WeatherManager` | `Island_Logic/` | Diuji di Unity: zona, cuaca, jembatan S1, gerbang S2/S3, angkat barang berat, frostbite (obor, pakaian, jatuh jurang belum diuji) |
| **7** | *Sudden Event*, `LuckyMachine`, `DiaryManager`, alur menang dan kalah | `Island_Logic/`, `Core_Logic/` | Diuji di Unity: Diary bab 1, erupsi, Lucky Machine (korban, mode gratis, hadiah), pasang suku cadang, lepas landas dan layar menang (gempa, badai, sinematik mati belum diuji) |

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

### Penyiapan di Unity untuk Langkah 4 (sudah dilakukan untuk IslandPhase greybox)

Dibuat lewat menu **Survival Island > Setup > Langkah 4 - Survival dan Bangun** (`Assets/Editor/Langkah4Setup.cs`, boleh dijalankan ulang, jalankan setelah menu Langkah 3). Semua angka diambil dari blueprint. Isinya:

- **Item (`Resources/Items`):**
  - Bahan: `Mat_Wood` (100, bahan bakar 60 detik), `Mat_Stone` (100), dan `Mat_Cloth`, `Mat_Rope`, `Mat_Hide`, `Mat_TreeSap`, `Mat_Mushroom`, `Mat_Charcoal` (20, arang 120 detik).
  - Alat: `Tool_Axe` (`TwoHand`, `Axe`, durabilitas 100, diperbaiki di `WoodBench`), `Tool_Pickaxe`, `Tool_Hammer` (`HammerEquippable`), dan `Tool_Torch` (`OffHand`, tanpa viewmodel).
  - Lainnya: `Kit_Campfire`, `Gear_WarmClothes`, `Bag_Cloth` (+5 slot), `Bag_Leather` (+10 slot), `Med_Norit`, dan `Med_Antibiotic`. `Med_Bandage` mendapat `curesDiseases = Bleeding`.
- **Resep:** 12 resep inti `CraftingStation.md` di `Resources/Recipes`, semuanya terbuka sejak awal.
- **Bangunan:** 12 `BuildableData` (`Assets/Data/Buildables`) dengan prefab greybox di `Assets/Prefabs/Structures`. Tiap prefab punya `StructureNode`, `BaseHealth`, layer `Structure`, dan `SurfaceTag` `Kayu`. Meja memakai `CraftingStation`. `Build_BarbedWire` dan `Build_Lamp` dilewati karena `Mat_Scrap` belum punya data.
- **Node sumber daya:** `Node_Tree`, `Node_Rock`, `Node_Mushroom`, dan `Node_SapTree` (`Assets/Data/ResourceNodes`, tabel `ResourceRespawn.md`).
- **`IslandPhase`:**
  - Lantai kotak diganti Terrain Region 1 (1000 x 1000 m sejak menu Langkah 6, posisi di `IslandLayout`), dengan pasir di pantai selatan dan rumput di utara. Terrain memakai `TerrainSurfaceSource`, `SurfaceLayerMap` (pasir = `Pasir`, rumput = `Rumput`), dan peta permukaan yang sudah dibake.
  - 7 pohon (termasuk 1 pohon getah), 3 batu, dan 2 jamur.
  - `DayNightDirector`, `ResourceRespawn`, `CraftingManager`, `TechTreeManager` (`nodes` kosong, sesuai `TechTreeManager.md`), `StructureRegistry`, dan `BuildManager`.
  - `CraftingPanel` dan `BuildMenu` polos. Kedua panel tetap aktif dengan alpha 0, karena `Instance` diisi di `Awake`.
- **Prefab lama:** `InfectionSystem` di `Player_Island`, dan `ResourceDropGuard` di `WorldItem_Generic`.

**Hasil uji Play Mode (7 November 2026, mulai langsung dari `IslandPhase`):**
- Jam game berjalan dari 06:00.
- Pohon tumbang setelah 5 pukulan tangan dan menjatuhkan 10 `Mat_Wood`.
- `rec_hammer` menghasilkan palu, sedangkan `rec_vaccine` ditolak `NoStation` tanpa Meja Medis.
- Dengan palu, `Floor_Wood` terbangun di rumput dan memakai 10 kayu.
- Membangun di pasir ditolak dengan alasan `Beach` dan ghost berwarna merah (restriksi pantai, Pilar 2).
- `CraftingPanel` menampilkan 12 resep.
- Error yang tersisa hanya `AudioMixer` kosong.

Belum diuji: `BuildMenu` lewat tombol B, demolish, upgrade, dan perbaikan, meja crafting yang dibangun, tumbuh ulang node, infeksi berjalan, dan save lalu lanjutkan.

Data yang belum ada di blueprint dan perlu dikonfirmasi developer:
- `maxDurability` `Tool_Pickaxe` dan `Tool_Hammer` (sementara -1).
- `ItemType` bahan, kit, dan tas (sementara `Material`) serta alat (sementara `Tool`).
- `maxStack` `Med_Norit` dan `Med_Antibiotic` (sementara 1).
- ID node pohon dan batu (sementara `Node_Tree` dan `Node_Rock`).
- Kategori bangunan dasar (sementara `Structure` atau `Furniture`).

Daftar penyiapan dari blueprint:

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

### Penyiapan di Unity untuk Langkah 5 (sudah dilakukan untuk IslandPhase greybox)

Dibuat lewat menu **Survival Island > Setup > Langkah 5 - Musuh dan Senjata** (`Assets/Editor/Langkah5Setup.cs`, boleh dijalankan ulang). Urutan menu: 1, 2, 3, 4, lalu 5. Menu 2 dan 3 membangun ulang scene, jadi menu 4 dan 5 perlu dijalankan lagi sesudahnya. Isinya:

- **Data:**
  - 17 `AttackData` (`Assets/Data/Enemies/Attacks`). Tiga aset lengkap dan varian zombie mengikuti `EnemyAttack.md`. Serangan hewan memakai nama `Animal_<enemyId>` dan hanya mengisi damage (ditambah Poison dan jangkauan 1,5 untuk ular).
  - 6 `ZombieData`, termasuk resistansi militer.
  - 8 `AnimalData` beserta hasil kulitnya (daging dan kulit).
  - `Threat_Zombie` dan `Threat_Animal`.
  - 6 `LootTable` `Drop_Zombie_*`.
  - 4 `RegionSpawnTable` (`Assets/Data/Spawn`).
  - Item baru `Food_RawMeat` (`ItemUseSystem.md`).
- **Prefab (`Assets/Prefabs/Enemies`):** 6 zombie, 8 hewan, dan `Tribal_Snare`.
  - Zombie memakai `EnemyAI`, `EnemyHealth`, `EnemyAttack`, `EnemyHitReaction`, `AISensory`, `SunlightExposure`, `TraversalAgent`, `EnemyInstance`, dan perilaku khusus varian.
  - Hewan memakai `WildlifeAI` dan `AnimalCarcass`.
  - Semuanya punya 3 hitbox `EnemyHitbox`.
  - `zombie_standar` sekarang dimiliki menu ini. Menu Langkah 2 tidak lagi menimpanya.
- **Pool:** 15 tag di `PersistentRoot`, dengan ukuran dari `ObjectPooler.md`.
- **Pemain:**
  - `Player_Island` mendapat `PlayerTargetable`, `PlayerPinnedState`, `PlayerShove`, dan `NoiseEmitter` (`isPlayerMovement`).
  - `Player_Ship` mendapat `PlayerTargetable`.
- **Scene:**
  - `IslandPhase` mendapat `SpawnManager`, `AITraversal`, `AIDirector`, `ThreatSystem`, `AISensoryHub`, `HordeCrowdManager`, `AttackCoordinator`, dan NavMesh pulau yang dibake (`IslandPhase_NavMesh.asset`).
  - `ShipPhase` mendapat `HordeCrowdManager` dan `AttackCoordinator`.

**Hasil uji Play Mode (7 November 2026, `IslandPhase`):**
- Zombie yang muncul siang hari langsung terbakar (`BurnToAshes`).
- Malam hari (`DebugSetTime(22)`), zombie yang membelakangi pemain diam tetap `Unaware` (mengendap). Setelah melihat pemain, zombie menjadi `Suspicious`, lalu `Aware`, lalu mengejar dan menyerang. HP pemain turun 15 per pukulan dan infeksi naik 5.
- Tembakan 30 ke kepala membunuh zombie (50 HP). Tembakan ke badan memberi 30 damage dan `Stagger`.
- Saat HP pemain habis, run masuk `Ending`, lalu `AwaitingChoice`, lalu kembali ke `ShipPhase` setelah Coba Lagi.
- Siang hari, `SpawnManager` memunculkan babi sendiri. Serigala menyerang dan babi mengejar.
- Error yang tersisa hanya `AudioMixer` kosong.

Catatan uji: `UIFocusStack` membuka Menu Jeda setiap kali jendela Unity kehilangan fokus (keputusan #85). Saat Unity dikendalikan dari luar, game sering terjeda.

Belum diuji:
- Varian zombie khusus (tribal, lumut, militer, beku, desa).
- Jaguar menerkam (`PlayerPinnedState`).
- Menguliti hewan.
- Horde Blood Moon.
- `GunController` lewat klik kiri.
- Panel Game Over sendiri (sempat terlewat karena klik dari jendela Game).

Data yang belum ada di blueprint dan perlu dikonfirmasi developer:
- Nama aset serangan hewan, serta semua field selain damage.
- `traversalMask` (sementara `AllAreas`, karena area NavMesh traversal #94 belum dibuat).
- `minDay`, `maxAlive`, dan `isSpecial` di tabel spawn.
- Poise dan knockback hewan selain beruang.
- `leavesCarcass` hewan yang tidak tertulis (sementara true kalau punya hasil kulit).

Daftar penyiapan dari blueprint:

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

### Penyiapan di Unity untuk Langkah 6 (sudah dilakukan untuk IslandPhase greybox)

Dibuat lewat menu **Survival Island > Setup > Langkah 6 - Region dan Cuaca** (`Assets/Editor/Langkah6Setup.cs`). Koordinat peta ada di `Assets/Editor/IslandLayout.cs` dan mengikuti peta usulan `RegionStreaming.md`:

| Region | Letak |
|---|---|
| R1 Volcano | X 1000-2000, Z 0-1000 |
| R2 Amazon | X 1000-2000, Z 1000-2000 |
| R4 Desa | X 0-1000, Z 1000-2000 |
| R3 Salju | X 0-1000, Z 2000-3000 |

Seam S1 berada di (1500, 1000), S2 di (1000, 1500), S3 di (500, 2000), dan helipad di (500, 2150). Isi Region 1 dari menu Langkah 3 dan 4 sudah digeser ke `IslandLayout.R1Origin` (1500, 0, 60). Pantai pendaratan ada di Z < 35. Tinggi permukaan tidak tertulis di dokumen, jadi semua lantai datar. Urutan menu: 3, 4, 5, lalu 6. Isinya:

- **Peta:**
  - Terrain R2, R3, dan R4. Masing-masing punya satu lapisan permukaan yang dibake (Tanah, Salju, Tanah).
  - Tebing di semua tepi selain seam.
  - Jurang S1 selebar 30 m dengan dasar di Y -20 dan `RavineRecoveryVolume`.
  - `BridgeSite` di tepi R1 dengan 20 ruas, `deckCollider`, papan TMP, dan `NoBuildVolume`.
  - `GateBarrier` di S1, S2, dan S3, ditambah `GateBlocker` di S2.
  - Helipad greybox dengan `NoBuildVolume`. Komponen helikopter dipasang menu Langkah 7.
  - 4 trigger `ZoneManager` yang saling tumpang tindih 10 m.
- **Sistem:**
  - `RegionalWeather` dengan 4 tabel (menu konteks "Isi nilai awal region").
  - `WeatherManager` dengan partikel hujan dan daun, serta 2 `AudioSource`.
  - `EnvironmentDirector` dengan 4 profil (menu konteks yang sama) dan Directional Light.
  - `RegionGateSystem` dengan aturan S1 Bridge, S2 DiaryChapter 3, dan S3 FollowsPrevious.
  - `HeavyItemSystem` dengan 5 `HeavyItemData` dan prefab, `PartSpawnPoints_Village` (10 titik di desa, jarak 75 m, letaknya greybox), dan `carryAnchor`.
- **Pool:** 6 pool `Fx_*`, masing-masing 2. Ukuran selain Snow dan Blizzard tidak tertulis. Isinya `ParticleSystem` di akar, tanpa `PooledEffect`.
- **Pemain dan item:**
  - `Player_Island` mendapat `WarmthSystem` dan `CarryAnchor`.
  - `Tool_Torch` memakai `Viewmodel_Torch` (`TorchController`, `Light`, `ParticleSystem`).
- **NavMesh:** dibake ulang untuk seluruh peta.

**Hasil uji Play Mode (7 November 2026):**
- **Zona dan cuaca:** di pantai R1 zona `Volcano` dan cuaca cerah. Masuk R2 membuat zona berganti ke `Amazon`, cuaca `Rain`, dan kabut berubah.
- **Jembatan S1:** bab 2 ditambah 250 kayu membuat 10 ruas, dan 500 kayu membuat 20 ruas, deck aktif, barrier hilang, dan Amazon terbuka.
- **Gerbang S2 dan S3:** bab 3 membuka Desa dan Salju, dan barrier S2 hilang.
- **Barang berat:** 5 suku cadang tergeletak di desa. Mengangkat Mesin membuat `SpeedMultiplier` 0,5 dan mengunci senjata.
- **Frostbite:** di R3 tanpa pakaian hangat, kehangatan habis dalam sekitar 4 detik (`regionSnowCold` 20). Setelah itu stamina habis, lalu HP turun.
- **Error:** yang tersisa hanya `AudioMixer` kosong.

Belum diuji:
- Menyetor kayu lewat interaksi E di `BridgeSite`. Uji memakai `DebugDeposit`.
- Jatuh ke jurang dan dipulihkan.
- Obor dan pakaian hangat. `WearClothing` belum punya pemanggil di UI.
- Peralihan suasana dan partikel `Fx_*` secara visual.
- Badai.
- Barang berat jatuh karena serangan.

Data yang belum ada di dokumen:
- Tinggi permukaan dan kedalaman jurang.
- Letak titik suku cadang dan ukuran pool `Fx_*` suasana.
- Kurva cahaya per jam dan skybox per region.
- Jangkauan cahaya obor.

Daftar penyiapan dari blueprint:

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

### Penyiapan di Unity untuk Langkah 7 (sudah dilakukan untuk IslandPhase greybox)

Dibuat lewat menu **Survival Island > Setup > Langkah 7 - Event dan Akhir** (`Assets/Editor/Langkah7Setup.cs`, jalankan setelah menu 6). Isinya:

- **Bencana:** `Disaster_Eruption`, `Disaster_Earthquake`, dan `Disaster_TropicalStorm` (`Assets/Data/Disasters`, angka `DisasterManager.md`). Ketiganya diisi ke `RunSeedManager.disasterData` di `PersistentRoot` dan ke `DisasterManager`. Pool `Hazard_Fireball` dan `Hazard_ImpactMarker` masing-masing 8.
- **Lucky Machine:**
  - 4 `LuckyMachineData` dengan kuota per region, dan 9 `LuckyRewardData` yang dipakai bersama oleh semua mesin. Bobot per hadiah memakai nilai awal 1, karena bobot tier ada di config.
  - 3 `WeaponModifierData` di `Resources/WeaponModifiers`, dan buff `Buff_LuckyHP` serta `Buff_LuckyStamina`.
  - Item `Reward_LaserPistol` (`WD_Reward_LaserPistol`, memakai viewmodel Pistol) dan `Reward_FireAxe` (memakai viewmodel Kapak).
  - Mesin greybox per region beserta kandang (trigger `ZoneTrigger` 6 x 4 x 4 m), gerbang `NavMeshObstacle`, dan `vacuumPoint`. Letak R3 (350, 2050) dari blueprint. Letak R1, R2, dan R4 greybox, karena dokumen menunggu #88.
- **Diary dan tujuan:**
  - `Diary_Ch1` sampai `Ch3`. Isinya hanya kunci teks, karena isi teks belum ditulis di dokumen mana pun.
  - 8 `ObjectiveData`. Penandanya memakai seam, jembatan, dan helipad dari `IslandLayout`.
  - `DiaryBook` greybox dekat pantai pendaratan. Letak shelter tidak tertulis.
  - `ObjectiveManager`, `DiaryManager`, dan panel `Diary`, `DiaryArchive`, `LuckyMachine`. Ketiga panel aktif dengan alpha 0.
- **Penyelamatan:**
  - `HelicopterFrame` di helipad dengan 5 visual suku cadang, `PartInstallZone` (radius 4), dan `CockpitInteractable`.
  - `RescueEventManager` di `IslandPhase`.
  - `DeathCinematic` di `Player_Island` dan `Player_Ship`. Tanpa `PlayerBodyRig`, gayanya menjadi Blackout.
- **Urutan eksekusi `RescueEventManager` = -50.** Menurut blueprint, `HelicopterFrame` ada di scene `Region3_Snow` yang dimuat belakangan. Di scene greybox tunggal, `OnEnable` rangka bisa berjalan sebelum `Awake` manager, sehingga pendaftaran rangka hilang dan mesin langsung mati.

**Hasil uji Play Mode (7 November 2026, `IslandPhase`):**
- **Diary:** `ObjectiveManager.InteractBook` membuka bab 1. `DiaryPanel` tampil dan menjeda game (`timeScale` 0), dan `Kit_Campfire` x1 diberikan.
- **Erupsi:** `ForceStart(Eruption)` masuk Warning 30 detik, lalu Active, dan Fireball jatuh.
- **Lucky Machine:** 3 serigala dikorbankan ke kandang Region 1, sehingga `StoredSpins` menjadi 1. Bencana besar membuka mode gratis 30 detik. Putaran gratis menghasilkan `lucky_fireaxe` (S), yang muncul sebagai WorldItem.
- **Helikopter:** 5 suku cadang yang diletakkan di helipad terpasang lewat `PartInstallZone`, visualnya muncul, dan status menjadi `Ready`. Setelah `StartEngine` dan pemanasan, statusnya `ReadyToLift`, lalu `LiftOff` membuat `hasEscaped` true dan run masuk `Ending`. Layar menang tampil, lalu tombol Coba Lagi tertekan dan run kembali ke `ShipPhase`.
- **Error:** yang tersisa hanya `AudioMixer` kosong.

**Perlu dicek developer secara manual:** selama uji yang dikendalikan dari luar, tombol yang dipilih otomatis (Run Baru, Lanjutkan, Statistik, Coba Lagi) beberapa kali tertekan sendiri sesaat setelah panel tampil. Dugaannya, ini event Submit dari Input System saat fokus jendela Unity berpindah. Belum terbukti, dan perlu dicoba dengan bermain langsung.

Belum diuji:
- Gempa dan Badai Tropis.
- Hadiah lain (buff, modifier, skin).
- Arsip Diary dan bab 2 dan 3 lewat buku.
- Panel Lucky Machine lewat E.
- `DeathCinematic`.
- Narasi ending (`NarrativeScene` x7 belum dibuat karena isi beat tidak tertulis; alur menang tetap jalan tanpanya).

Data yang belum ada di dokumen:
- Isi teks Diary, tujuan, dan narasi.
- Letak Lucky Machine R1, R2, dan R4, serta shelter atau buku.
- Damage dan viewmodel Kapak Api.
- Bobot per hadiah.
- `DeathProfile` (pesan, warna, suara).

Daftar penyiapan dari blueprint:

1. Aset **Create > Survival > Disaster Data** untuk `Eruption`, `Earthquake`, dan `TropicalStorm` (nilai di `DisasterManager.md`), lalu isi ke `RunSeedManager.disasterData` di `PersistentRoot` dan ke `DisasterManager.disasterData`.
2. Scene `IslandPhase`: objek manager berisi `DisasterManager`, `LuckyRewardLedger`, `RescueEventManager`, dan `DiaryManager` (isi `diaryPanel` dan `archivePanel`). Canvas: `DiaryPanel` (id `Diary`) dan `DiaryArchivePanel` (id `DiaryArchive`) dengan `pausesTime`, `releasesCursor`, `blocksGameplayInput`, dan `closableByEsc`; `LuckyMachinePanel` (id `LuckyMachine`) sama tetapi tanpa `pausesTime`. Objek panel aktif saat scene dimuat, supaya `Awake` mengisi `Instance`.
3. Pool di `ObjectPooler`: `Hazard_Fireball` (layer `Projectile`, komponen `Fireball`) dan `Hazard_ImpactMarker` (komponen `HazardImpactMarker`).
4. Satu prefab mesin per region: `LuckyMachine` + `SaveIdTag` (aset **Create > Survival > Lucky Machine Data** dengan kuota di `LuckyMachine.md`, hadiah **Survival > Lucky Reward** dan **Survival > Weapon Modifier**), layer `Interactable`; anak `LuckyMachineCage` dengan `BoxCollider` trigger 6 x 4 x 4 m di layer `ZoneTrigger`, `Rigidbody` kinematic, objek gerbang dengan `NavMeshObstacle` carve, dan `vacuumPoint`. Isi `duplicateModifierFallback` dengan `lucky_ammo`.
5. `Region3_Snow`: `HelicopterFrame` dengan bagian per `partId`, anak `PartInstallZone` (trigger `ZoneTrigger`), dan `CockpitInteractable` (layer `Interactable`).
6. Prefab pemain Fase 1 dan Fase 2: `DeathCinematic` (isi `cameraRig` dan, kalau ada, `PlayerBodyRig` nonaktif dengan tulang `Rigidbody` kinematic dan `focusBone`).
7. Aset **Create > Survival > Diary Entry** untuk Bab 1 sampai 3, diisi ke `DiaryArchivePanel.chapters`.

## 🧩 Ditambahkan: sistem pendukung

Semua blueprint yang belum punya kode ditulis di branch `claude/elegant-liskov-301aa5` (7 November 2026). Status: **ditulis, belum dikompilasi di Unity.** Kode dicek dengan `dotnet build` (netstandard2.1, C# 9) terhadap DLL Unity 6000.3.10f1 dan DLL `Library/ScriptAssemblies` checkout utama (Input System, TextMeshPro, UGUI, Cinemachine, URP, RP Core, AI Navigation), tanpa stub: 0 error dan 0 warning di Editor, rilis, dan `DEVELOPMENT_BUILD`. Belum ada yang dijalankan di Play Mode. File `.meta` belum dibuat; Unity membuatnya saat proyek dibuka.

Penyimpangan dan tafsiran dicatat di bagian "Catatan Implementasi (Sistem pendukung)" pada tiap blueprint.

### Tidak ditulis

| Blueprint | Alasan |
|---|---|
| `raft/FarmingSystem.md` (#48), `Island_Logic/SupplyDrop.md` (#54), `Core_Logic/WorldMapSystem.md` (#105), `Island_Logic/NPCInfection.md` (#117) | Diputuskan developer tidak dipakai (arsip) |
| `GameEventBus.md`, `PhysicsLayers.md`, `SaveSystem.md`, `DamagePipeline.md`, `EnvironmentProfile.md`, `VitalsRules.md` | Sudah dicakup kode Langkah 1-6 dengan nama kelas lain (lihat tabel Langkah di atas) |

### File → blueprint

| File | Blueprint |
|---|---|
| `Core_Logic/SettingsManager.cs`, `SettingsPanel.cs` | `SettingsManager.md` (#19) |
| `Core_Logic/AudioManager.cs` | `AudioManager.md` (#114) |
| `Core_Logic/LocalizationManager.cs` (`AccessibilityOptions`, `FlashGuard`), `LocalizationTable.cs`, `LocalizedText.cs`, `SubtitlePresenter.cs`, `LocalizationAuditWindow.cs` (Editor) | `LocalizationManager.md` (#104) |
| `Core_Logic/CameraShake.cs`, `CameraShakeProfile.cs` | `CameraShake.md` (#24, Cinemachine 3.x) |
| `Core_Logic/VFXManager.cs` (`VFXEntry`, `VFXHandle`), `PooledEffect.cs` | `VFXManager.md` (#15) |
| `Core_Logic/NotificationManager.cs`, `NotificationRow.cs`, `NotificationLogView.cs` | `NotificationManager.md` (#22) |
| `Core_Logic/PostProcessController.cs` | `PostProcessController.md` (#23) |
| `Core_Logic/GhostStatusIcon.cs` | `LocalizationManager.md` (penanda selain warna di hologram bangunan) |
| `Core_Logic/RegionLootTable.cs`, `LootTable.cs` | `RegionLootTable.md` (#103) |
| `Core_Logic/LootSpawnerSystem.cs` | `LootSpawnerSystem.md` |
| `Core_Logic/StatusEffectManager.cs`, `StatusEffectData.cs`, `StatusEffectDatabase.cs`, `StatusEffectBar.cs` | `StatusEffectManager.md` (#20) |
| `Core_Logic/DurabilitySystem.cs` | `DurabilitySystem.md` |
| `Core_Logic/ToolRepairSystem.cs`, `ItemUpgradeData.cs`, `WeaponStatCalculator.cs` | `ToolRepairSystem.md` (#113) |
| `Core_Logic/HUDManager.cs`, `HotbarView.cs` | `HUDManager.md`, `EquipmentManager.md` (HotbarView) |
| `Core_Logic/AmbientAudio.cs`, `AmbientProfile.cs`, `AmbientZone.cs`, `PooledAudioSource.cs` | `AmbientAudio.md` (#52) |
| `Core_Logic/CombatMusicDirector.cs`, `MusicSet.cs`, `EnemyCueSet.cs`, `ThreatAudioEmitter.cs` | `CombatMusicDirector.md` (#114) |
| `Core_Logic/FootstepAudio.cs`, `SurfaceAudioSet.cs` | `FootstepAudio.md` (#21) |
| `Core_Logic/AnimationController.cs` (`AnimIds`), `PlayerAnimDriver.cs`, `EnemyAnimDriver.cs` | `AnimationController.md` (#12) |
| `Core_Logic/OptimizationManager.cs` | `OptimizationManager.md` |
| `Core_Logic/CombatFeedbackUI.cs`, `DamageIndicator.cs` | `CombatFeedbackUI.md` |
| `Core_Logic/CompassSystem.cs` (`MapMarkerRegistry`), `CompassIcon.cs` | `CompassSystem.md` (#30) |
| `Core_Logic/TutorialManager.cs`, `TutorialHint.cs`, `TutorialHintPanel.cs` | `TutorialManager.md` (#16) |
| `Core_Logic/LocalStatsManager.cs`, `StatsPanel.cs` | `LocalStatsManager.md` (#57) |
| `Core_Logic/DevConsole.cs` (hanya Editor dan `DEVELOPMENT_BUILD`) | `DevConsole.md` (#56) |
| `Core_Logic/NarrativeSequencer.cs` (`EndingContext`), `NarrativeScene.cs`, `HelipadFoundVolume.cs` | `NarrativeSequencer.md` (#99) |
| `Core_Logic/RegionStreaming.cs`, `RegionStateStore.cs`, `RegionSceneRoot.cs`, `SeamBarrier.cs`, `RegionBoundaryGuard.cs` | `RegionStreaming.md` (#88) |
| `Core_Logic/ProgressionSafety.cs` | `ProgressionSafety.md` (#116) |
| `Island_Logic/NPCData.cs`, `NPCRoster.cs`, `NPCCompanion.cs`, `left-4-dead/NPCGunUser.cs` | `NPCRoster.md`, `NPCCompanion.md` (#76), `GunController.md` (bagian NPC) |
| `Island_Logic/ObjectiveManager.cs`, `ObjectiveData.cs`, `DiaryBook.cs` | `ObjectiveManager.md` (#81) |
| `Island_Logic/LoreCollectibles.cs`, `LoreEntry.cs`, `LorePoint.cs`, `LoreReaderPanel.cs`, `BarkSet.cs`, `NPCBarks.cs` | `LoreCollectibles.md` (#100) |
| `Island_Logic/IslandInteractable.cs`, `PickupInteractable.cs`, `LootContainerInteractable.cs` | `IslandInteractable.md` (#17, sisa di luar kontrak Langkah 2) |
| `Island_Logic/RemoteBaseSimulation.cs` | `RemoteBaseSimulation.md` (#90) |
| `Island_Logic/EnvironmentalHazard.cs` | `EnvironmentalHazard.md` (#96) |
| `Island_Logic/FireSystem.cs`, `Flammable.cs`, `FlammableProfile.cs`, `FireArea.cs`, `BurnEffect.cs`, `FireExtinguishTarget.cs` | `FireSystem.md` (#97) |
| `Island_Logic/LightSourceSystem.cs`, `LightSource.cs`, `LightFixture.cs`, `LightFixtureData.cs` | `LightSourceSystem.md` (#98) |
| `Island_Logic/FlashlightController.cs` | `FlashlightController.md` |
| `Island_Logic/TideManager.cs`, `TideSafePoint.cs`, `LowTideSpot.cs`, `TideZoneAuditWindow.cs` (Editor) | `TideManager.md` (#91) |
| `Island_Logic/SwimmingController.cs`, `WaterVolume.cs` | `WaterSwimmingController.md` (#13) |
| `Island_Logic/BeachSupplyManager.cs`, `BeachSupplyPoint.cs`, `BeachCrate.cs` | `BeachSupplyManager.md` |
| `Island_Logic/SleepSystem.cs`, `BedInteractable.cs`, `SleepPanel.cs` | `SleepSystem.md` |
| `left-4-dead/HordeManager.cs` | `HordeManager.md` (#50) |
| `left-4-dead/AITraversal.cs`, `TraversalAgent.cs`, `TraversalLinkGate.cs`, `ThrownRock.cs` | `AITraversal.md` (#94) |
| `left-4-dead/ZombieLair.cs`, `InteriorZone.cs`, `LairNest.cs`, `LairRegistry.cs`, `LairData.cs` | `ZombieLair.md` (#112) |
| `left-4-dead/ThrowableSystem.cs`, `ThrowableData.cs`, `ThrowableProjectile.cs`, `LureTargetable.cs` | `ThrowableSystem.md` |
| `raft/BaseNavigation.cs`, `BaseDoor.cs`, `BaseDoorLatch.cs`, `BaseAnchor.cs` | `BaseNavigation.md` (#78) |
| `raft/BuildingIntegrity.cs`, `StructureVisual.cs`, `StructureIntegrityInfo.cs` | `BuildingIntegrity.md` (#47) |
| `raft/BaseTrap.cs`, `BaseTrapZone.cs`, `TrapData.cs` | `BaseDefense.md` |
| `raft/HydrationSystem.cs`, `RainwaterCollector.cs`, `WaterSourcePoint.cs` | `HydrationSystem.md` |
| `raft/DiseaseManager.cs` (`DiseaseType`, `DiseaseEffectTable`), `DiseaseOverlay.cs` | `DiseaseManager.md` |
| `raft/CampfireManager.cs`, `CookingRecipe.cs`, `CampfirePanel.cs` | `CampfireManager.md` (#44) |
| `raft/FishingSystem.cs`, `WaterBody.cs`, `FishingTensionHUD.cs` | `FishingSystem.md` |
| `raft/DebrisRetrieval.cs`, `DebrisItem.cs`, `DebrisLane.cs` | `DebrisRetrieval.md` |
| `raft/StorageChest.cs`, `StoragePanel.cs`, `SlotGridView.cs`, `SlotGridViewCell.cs` | `StorageChest.md` |

Event baru ada di akhir `Core_Logic/GameEvents.cs` (bagian "Ditambahkan: sistem pendukung"), dan field config baru di akhir `Core_Logic/GameBalanceSections.cs`. Semua kelas bagian config dibuat `partial`, begitu juga `GameBalanceConfig`. `SurvivalIsland.asmdef` menambah referensi `Unity.Cinemachine`, `Unity.RenderPipelines.Universal.Runtime`, `Unity.RenderPipelines.Core.Runtime`, dan `Unity.AI.Navigation`.

### Tambahan untuk `PersistentRoot` (`RunLifecycle.orderedServices`)

Urutan lengkap sesuai tabel "Urutan bootstrap" di `RunLifecycle.md`. Yang baru punya kode ditebalkan:

1. `GameManager`
2. `InputManager`
3. **`SettingsManager`**
4. **`AudioManager`**
5. **`AmbientAudio`**
6. **`CombatMusicDirector`**
7. **`LocalizationManager`**
8. `UIFocusStack`
9. `UIManager`
10. **`NotificationManager`**
11. **`TutorialManager`**
12. `ObjectPooler` (lalu `SurfaceDetection`, seperti sekarang)
13. **`VFXManager`**
14. `AsyncSceneLoader`
15. `RunSeedManager`
16. `WorldStateSave`
17. **`LocalStatsManager`**
18. **`DevConsole`** (hanya Editor dan `DEVELOPMENT_BUILD`; di build rilis kelasnya tidak ada)
19. **`NarrativeSequencer`** (usulan teknis: persisten menurut tabel masa hidup di `RunLifecycle.md`, dan punya `ResetForNewRun`)

Juga di bawah `PersistentRoot`, tetapi bukan service: `SubtitlePresenter`, canvas notifikasi, `SettingsPanel` (isi `UIManager.settingsPanel`), `TutorialHintPanel`, UI `DevConsole`, dan UI `NarrativeSequencer`.

**Sudah dipasang (7 November 2026)** lewat menu Langkah 1 (`Langkah1Setup.cs`):
- Ke-19 service di atas masuk `orderedServices` sesuai urutan.
- `AudioManager` mendapat 3 `AudioSource` (bgm, sfx, warning), dan `NarrativeSequencer` mendapat `voiceSource`.
- UI pendukung memakai tampilan polos sementara. `SettingsPanel` hanya berisi slider dan toggle utama, tanpa tab, dropdown grafis, maupun baris rebind.
- `MainMenu` mendapat `StatsPanel`.

Diuji di Play Mode: alur Bootstrap → MainMenu → peringatan kilatan (`ConfirmDialog`) → Run Baru → `ShipPhase` berjalan tanpa error selain `[AudioManager] Rujukan 'mixer' kosong`.

Belum dibuat: `AudioMixer` (tidak bisa dibuat lewat API publik, jadi perlu dibuat manual), `LocalizationTable` `id`/`en`, aset `MusicSet`/`AmbientProfile`, dan pool `VFX_*`. Semua bagian itu hanya mencatat peringatan. `MainMenuController.settingsPanel` masih kosong, karena `SettingsPanel` berada di prefab persisten, sehingga tombol Pengaturan di menu utama nonaktif.

### Komponen scene

| Scene | Komponen |
|---|---|
| `ShipPhase` dan `IslandPhase` | `HUDManager` + `HotbarView` + `StatusEffectBar` + `CombatFeedbackUI`, `CameraShake` (rig kamera), `PostProcessController` (`PostFxRig`), `LootSpawnerSystem` (dek kapal atau region), `FootstepAudio` (pemain) |
| `IslandPhase` | `ToolRepairSystem`, `OptimizationManager`, `CompassSystem` (anak HUD), `RegionStreaming`, `RegionBoundaryGuard`, `FireSystem`, `LightSourceSystem`, `TideManager`, `BeachSupplyManager`, `SleepSystem`, `HordeManager`, `AITraversal`, `LairRegistry`, `ThrowableSystem`, `BaseNavigation`, `BuildingIntegrity`, `DebrisRetrieval`, `NPCRoster`, `ObjectiveManager`, `LoreCollectibles`, `NPCBarks`, `RemoteBaseSimulation`, `ProgressionSafety`. Di pemain: `StatusEffectManager`, `HydrationSystem`, `DiseaseManager`, `SwimmingController`. Panel: `CampfirePanel`, `StoragePanel`, `SleepPanel`, `LoreReaderPanel`, `DiseaseOverlay`, `FishingTensionHUD` |
| Scene region | `RegionSceneRoot` (akar), `ZombieLair` + `InteriorZone` + `LairNest`, `EnvironmentalHazard`, `LowTideSpot`, `DebrisLane`, `WaterSourcePoint`, `WaterBody`, `AmbientZone`, `HelipadFoundVolume` (Region3_Snow), `TraversalLinkGate` |
| `MainMenu` | `StatsPanel` (isi `MainMenuController.statsPanel`) |

### Penyiapan di Unity (belum dilakukan)

1. **Audio:** `AudioMixer` dengan grup `Music`, `Ambient` (anak `AmbientA` dan `AmbientB`), `EnemyCue`, `Warning`, dan `SFX`; snapshot `Normal`, `Paused`, dan `Cinematic` (opsional); parameter terekspos `Master_Vol`, `Music_Vol`, `SFX_Vol`, `AmbientA_Vol`, `AmbientB_Vol`, dan `Ambient_Lowpass`. Tiga `AudioSource` di `AudioManager` (bgm, sfx, warning). Aset `AmbientProfile`, `MusicSet` (5 stem), `EnemyCueSet`, dan `SurfaceAudioSet` (8 baris).
2. **Teks:** aset `LocalizationTable` `id` dan `en` (bisa impor CSV). Kunci baru yang dipakai kode: `hud.*`, `notif.*`, `prompt.*`, `ui.settings.*`, `ui.flash_warning.*`, `ui.language.<id>`, `ui.compass.n/e/s/w`, `ui.stats.*`, `objective.*`, `lore.*`, dan `bark.*`. Kalau kunci belum ada, kode menampilkan kunci atau teks cadangan.
3. **Kamera dan layar:** `CameraShakeProfile` di rig kamera. `PostFxRig` per scene: tiga `Volume` global (A dan B priority 0, Status priority 10), dan Post Processing aktif di kamera. `SettingsManager` menulis `shadowDistance` dan `maxAdditionalLightsCount` ke aset URP aktif; di Editor perubahan ini ikut tersimpan ke aset.
4. **Pool `ObjectPooler`:** semua tag `VFX_*` di `vfx.catalog` (prefab dengan `PooledEffect`), `Audio_OneShot3D` (16), `UI_DamageIndicator` (6), `ShipLoot_Generic`, `WorldItem_Generic`, `Debris_Generic`, `FX_FireArea` (8), `FX_FireBurn` (24), `Supply_BeachCrate`, `Thrown_Rock`, proyektil barang lempar, `FX_RepairHit`, `FX_UpgradeDust`, dan `FX_StructureCrack`.
5. **NavMesh Area:** `JumpLink`, `ClimbLink`, `HighClimbLink`, `DropLow`, `DropMid`, `DropHigh`, `DoorLink`, `Interior`, dan `Tidal`.
6. **Scene region:** `Region1_Volcano`, `Region2_Amazon`, `Region3_Snow`, dan `Region4_Village`, masing-masing dengan satu akar `RegionSceneRoot` dan masuk Build Settings. Isi `RegionStreaming.regions[4]` dan `seams[3]`, lalu pasang `SeamBarrier` dan `NavMeshLink` per seam.
7. **Data:** `LootTable` (Loot_Ship, Loot_Region1-4, Loot_Lair_*, Drop_Zombie_*, Debris_Default, kargo pantai), `StatusEffectData` + `StatusEffectDatabase`, `ItemUpgradeData` (`Resources/ItemUpgrades`), `CookingRecipe` (`Resources/Recipes`), `DiseaseEffectTable`, `ThrowableData` x4, `TrapData` x3, `LairData`, `FlammableProfile` x6, `LightFixtureData` x2, `NPCData` x3, `BarkSet`, `ObjectiveData` x8, `DiaryEntry` x3, `LoreEntry`, `TutorialHint` (`Resources/TutorialHints`), dan `NarrativeScene` x7.
8. **Prefab:** NPC (`NavMeshAgent`, `NPCCompanion`, `NPCGunUser`, layer `NPC`), musuh (`TraversalAgent`, `ThreatAudioEmitter`, `AnimationController`, `EnemyAnimDriver`, `Flammable`, tag `Zombie` atau `Animal`), struktur (`StructureIntegrityInfo`, `StructureVisual`, `BaseDoor`, `BaseTrap`, `CampfireManager`, `StorageChest`, `RainwaterCollector`, `LightFixture`, `BedInteractable`), Tong Merah (`EnvironmentalHazard`), viewmodel `Base_Kail` (`FishingSystem`) dan senter (`FlashlightController`), serta rig tangan (`PlayerAnimDriver`).
9. **UI:** semua panel di atas memakai `UIPanel` dengan setelan dari blueprint masing-masing (jeda, kursor, blok input, Esc).

### Keputusan developer setelah sistem pendukung (7 November 2026)

Dicatat di `doc/USULAN_TERBUKA.md` (Keputusan utama) dan blueprint terkait.

- `EnemyAI` memakai `BaseNavigation.PollChase`/`PollAttacking`, `NotifySpawned`, dan `TraversalAgent.TickUnreachable`. Tanpa `BaseNavigation` di scene, pemeriksaan jalur Langkah 5 tetap dipakai.
- Pusat base: rata-rata posisi struktur (sudah begitu di kode).
- Obor Dinding: tekan `E` menambah 1 kayu (sudah begitu di kode).
- Trigger `Shove` (`AnimIds.Shove`) dipakai `PlayerShove`. Tambahkan parameter trigger `Shove` di `AC_PlayerArms`.
- `resource.knifeDecayPerCombatHit` = 1 (`MeleeCombatSystem`), dan `equipment.fistDamage` = 5 (`FistBehaviour` sekarang turunan `MeleeCombatSystem`).
- `LifeboatExtraction`: isi `npcDatabase` (aset `NPCData` yang sama dengan `NPCRoster`) dan `shipNpcPoints`, serta pool `ShipNpc_Generic`. `NarrativeSequencer`: isi `npcDatabase`; beat dengan `textKey` `narrative.lifeboat.lost` (teks tabel memakai `{names}`) menyebut korban kapal.
- `NarrativeSequencer` masuk `orderedServices` urutan 19.
- `ItemData.OnValidate` (Editor) mencatat error untuk item non-inti dengan durabilitas yang tidak bisa diperbaiki, kecuali `Tool_Torch` dan `Tool_Flashlight`.

### TODO sisa yang disambung (7 November 2026)

- **#88 penyimpanan region:** `RegionSaveRouting` (di `RegionStateStore.cs`). Objek di bawah `RegionSceneRoot` dipegang `RegionStateStore` saat `RegionStreaming` ada, dan tetap mendaftar ke `WorldStateSave` di scene uji tanpa streaming. `WorldItemManager.CaptureRegion`/`RestoreRegion`/`SweepBelow`, `WorldItem.sourceSpawner`, `LootSpawnerSystem.HandleRegionLoadState`, `HeavyItemSystem` per region, `RunLifecycle` memanggil `LoadInitialRegions`. Pastikan semua objek region berada di bawah `RegionSceneRoot` dan `RegionStreaming.regions[].bounds` terisi.
- **File baru:** `Core_Logic/SurfaceDebugOverlay.cs` (debug, perintah `surfacedebug`), `left-4-dead/DetectionMeterUI.cs` (meter deteksi stealth, `AISensory.md`), `raft/RegrowablePickup.cs`, `StonePickup.cs`, `CoconutPickup.cs` (`ResourceRespawn.md`).
- **Sambungan lain:** zombie kapal tidur dan bangun (`ShipHazardManager`), `ShipShove` mendorong, penanda dan isi sekoci di HUD, `SettingsManager.showHitMarkers`, `AudioManager.PlaySFXAt` (3D lewat pool `Audio_OneShot3D`), `PostProcessController.SetScreenTint`, `MeleeCombatSystem.RecalculateStats`, `EquipmentManager.LowerBriefly` saat melempar, `CraftingStation` dan `ClimbableSurface` mewarisi `IslandInteractable`, perintah DevConsole `savecorrupt`, `savedelay`, `postfx`, `heli install all`, `surfacedebug`.
- **Penyiapan Unity baru:** `HUDManager.lifeboatContentsText` dan `buildHintText`; `DetectionMeterUI` di HUD; `SettingsPanel.showHitMarkersToggle`; `GunController.muzzlePoint`; klip `PlayerShove.shoveFailClip`, `DisasterData.pulseClip`, `LuckyMachine.vacuumClip` dan `freeWindowWarnClip`; batu kecil dan kelapa memakai `StonePickup`/`CoconutPickup`; `ClimbableSurface` di layer `Interactable` dengan `promptKey`; kunci `notif.tide_items_moved`, `hud.lifeboat_contents`, `hud.demolish_preview`; shader `Hidden/Internal-Colored` ikut Development Build.
- **Masih TODO (29):** butuh keputusan desain (alarm kapal T=45, pengali `DifficultyProfile`, gaya dorong tarikan Tribal, titik tujuan kelompok #93), aset baru (partikel vakum dan api obor, animasi berbaring dan tusuk pisau, layar basah), titik uji `tpregion`, sistem berblueprint besar yang belum ditulis (`VillageZombieHiding`/`HidingSpot` #63, `BushSpawnPoint`, `AmbushPerch`, `RunLayoutSources`/`RunLayoutValidator`, jalur bersama kelompok), dan alat Editor yang butuh assembly Editor terpisah.

### Keputusan developer putaran kedua (7 November 2026)

- **Alarm kapal:** isi `ShipHazardManager.alarmSource` (AudioSource dengan klip alarm). Centang `ShipZombiePoint.wakeOnAlarm` di 2 titik dek tengah.
- **Kesulitan:** `DifficultyProfile.enemyCountMultiplier` (dipakai `AIDirector`) dan `harvestYieldMultiplier` (dipakai `ResourceNode`).
- **Dorongan luar:** `IslandPlayerController.AddExternalVelocity`, dipakai tarikan Zombie Tribal.
- **Kelompok zombie jauh (#93):** jalur bersama per sel di `HordeCrowdManager`, perintah `crowdstat`, `stress`, `crowdai`.
- **`RunLayoutSources` dan `RunLayoutValidator` (#89):** buat aset **Survival > Run Layout Sources**, isi ke `RunSeedManager.layoutSources`, lalu isi `partSpawnPoints`, `shipNpcs`, dan `disasterData`. Jalankan menu "Salin data kapal dari scene ShipPhase yang terbuka", "Periksa isReachable" pada `PartSpawnPointTable`, isi `safeLayout`, lalu "Periksa safeLayout" dan "RunSeedAudit". Sebelum itu, semua seed gagal pemeriksaan dan run memakai `safeLayout`. Tanpa aset ini, perilaku lama tetap jalan dengan peringatan.
- **Zombie Desa, Lumut, Jaguar, Gagak:** file baru `left-4-dead/HidingSpot.cs`, `VillageZombieHiding.cs`, `BushSpawnPoint.cs`, `AmbushPerch.cs`, `CrowFlock.cs`. Prefab `zombie_desa` mendapat `VillageZombieHiding` (Animator: bool `Dormant`, trigger `Emerge`). `Region4_Village` mendapat `HidingSpot` (kapasitas total minimal 30). `Region2_Amazon` mendapat `BushSpawnPoint` (entri `zombie_lumut` diberi `useBushPoints = true`) dan `AmbushPerch`. Animator Jaguar perlu trigger `Climb`.
