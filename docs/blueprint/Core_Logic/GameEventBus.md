# Blueprint: GameEventBus.cs
**Lokasi:** `docs/blueprint/Core_Logic/GameEventBus.md`
**Deskripsi Utama:** Pola komunikasi resmi antar-sistem. Sistem yang tidak perlu saling mengenal berkomunikasi lewat event bertipe, tanpa alokasi memori saat event dikirim.
**Issue:** #107
**Rujukan GDD:** Usulan arsitektur, belum ada di GDD. Mendukung semua sistem di GDD.

## 1. Arsitektur & Variabel Utama

### Perbandingan pilihan

| Hal | Event statis generik | ScriptableObject Event Channel |
|---|---|---|
| Pemasangan | Lewat kode, tanpa Inspector | Aset ditarik di Inspector |
| Salah sambung | Terdeteksi saat compile | Baru terlihat saat bermain |
| Data event | Struct bertipe, tanpa garbage | Perlu kelas channel per tipe |
| Jumlah event (sekitar 50) | Hanya sekitar 50 struct | Sekitar 50 struct, kelas, dan aset |
| Manager persisten | Tidak bergantung pada scene | Perlu referensi aset |
| Risiko | Pendengar lupa dilepas | Pendengar lupa dilepas |

**Pilihan: event statis generik.** Blueprint lama juga sudah memakai event statis, dan proyek ini ditulis lewat kode, sehingga pemeriksaan saat compile lebih berguna daripada pemasangan di Inspector.

### Bentuk kode

```csharp
public static class EventChannel<T> where T : struct
{
    public static event Action<T> Raised;
    public static bool HasLast { get; private set; }
    public static T Last { get; private set; }

    public static void Publish(in T data) { /* simpan Last kalau sticky, lalu panggil Raised */ }
    public static void Clear() { /* Raised = null, HasLast = false */ }
}
```

- `GameEventRegistry` mencatat fungsi `Clear` tiap channel saat pertama kali dipakai.
- Struct dengan atribut `[StickyEvent]` menyimpan nilai terakhir di `Last`.
- `GameEventRegistry.ClearAll()` dipanggil `RunLifecycle` (#82) saat run berakhir.
- Isi event adalah struct kecil, bukan kelas, jadi tidak menghasilkan garbage.

## 2. Detail Logika & Fungsi

### Kapan memakai event dan pemanggilan langsung

| Pola | Kapan dipakai | Contoh |
|---|---|---|
| **Event wajib** | Pengirim tidak perlu tahu siapa yang mendengar, atau pendengarnya lebih dari satu | Musuh mati, fajar tiba, bencana mulai |
| **Pemanggilan langsung boleh** | Pemanggil butuh jawaban saat itu juga dari sistem yang memang menjadi dependensinya | `InventoryManager.Instance.HasItem()`, `ObjectPooler.Instance.SpawnFromPool()` |
| **Dilarang** | Sistem gameplay memanggil sistem lain yang bukan dependensinya | `EnemyAI` memanggil `LuckyMachine.AddEnergy()` |

UI hanya mendengar event atau membaca data. UI tidak mengubah status gameplay secara langsung.

### Aturan berlangganan

```csharp
void OnEnable()  { EventChannel<NightfallEvent>.Raised += HandleNightfall; }
void OnDisable() { EventChannel<NightfallEvent>.Raised -= HandleNightfall; }
```

- Berlangganan di `OnEnable`, dan lepas di `OnDisable`, termasuk untuk objek dari pool.
- Jangan berlangganan di `Start` atau `Awake`, karena pasangan pelepasannya tidak jelas.
- Manager persisten mengikuti aturan yang sama. Ini menggantikan `AudioManager` lama yang tidak pernah melepas event.
- Jangan membuat objek baru atau memakai lambda yang menangkap variabel saat berlangganan, karena lambda itu tidak bisa dilepas.

### Pendengar yang terlambat aktif

Event yang menyatakan kondisi saat ini ditandai `[StickyEvent]`. Pendengar membaca nilai terakhirnya saat aktif:

```csharp
void OnEnable()
{
    EventChannel<RegionChangedEvent>.Raised += HandleRegion;
    if (EventChannel<RegionChangedEvent>.HasLast) HandleRegion(EventChannel<RegionChangedEvent>.Last);
}
```

Event sesaat, seperti `EnemyKilledEvent`, tidak disimpan. Pendengar yang belum aktif memang tidak perlu menerimanya.

### Daftar event global

Sistem baru menambahkan event ke tabel ini, bukan membuat jalur sendiri. Kolom Sticky bernilai "Ya" untuk event `[StickyEvent]`.

| Kelompok | Struct | Isi | Pengirim | Sticky |
|---|---|---|---|---|
| Waktu | `DaybreakEvent` | `day` | `DayNightDirector` | Ya |
| Waktu | `NightfallEvent` | `day` | `DayNightDirector` | Ya |
| Waktu | `DayChangedEvent` | `newDay` | `GameManager` | Ya |
| Waktu | `HourChangedEvent` | `hour` | `DayNightDirector` | Ya |
| Pemain | `VitalsChangedEvent` | `hp`, `stamina`, `hunger`, `thirst`, `infection` | `SurvivalStats`, 4 kali per detik | Ya |
| Pemain | `WarmthChangedEvent` | `warmth`, `isFreezing` | `WarmthSystem` | Ya |
| Pemain | `PlayerDamagedEvent` | `DamageInfo`, `remainingHP` | `SurvivalStats` | Tidak |
| Pemain | `PlayerDiedEvent` | `DeathReason` | `SurvivalStats` | Tidak |
| Pemain | `InfectionStageChangedEvent` | `stage` | `InfectionSystem` | Ya |
| Pemain | `PlayerPinnedEvent` | `attackerId`, `isPinned` | `PlayerPinnedState` | Ya |
| Pemain | `NoiseEmittedEvent` | `position`, `radius`, `sourceKind` | `NoiseEmitter` | Tidak |
| Musuh | `EnemySpawnedEvent` | `enemyId`, `position` | `SpawnManager` | Tidak |
| Musuh | `EnemyHitEvent` | `position`, `bodyPart`, `isKill`, `byPlayer` | `EnemyHealth` | Tidak |
| Musuh | `EnemyKilledEvent` | `enemyId`, `position`, `damageType`, `byPlayer` | `EnemyHealth` | Tidak |
| Musuh | `EnemySacrificedEvent` | `enemyId` | `LuckyMachine` | Tidak |
| Inventori | `InventoryChangedEvent` | `itemId`, `delta`, `total` | `InventoryManager` | Tidak |
| Inventori | `EquippedItemChangedEvent` | `itemId` | `EquipmentManager` | Ya |
| Inventori | `AmmoChangedEvent` | `inMagazine`, `reserve` | `GunController` | Ya |
| Inventori | `ItemCraftedEvent` | `recipeId` | `CraftingManager` | Tidak |
| Inventori | `HeavyItemChangedEvent` | `partId`, `isCarried` | `HeavyItemSystem` | Ya |
| Region | `RegionChangedEvent` | `from`, `to` | `ZoneManager` | Ya |
| Region | `RegionUnlockedEvent` | `region` | `RegionGateSystem` | Tidak |
| Region | `WeatherChangedEvent` | `region`, `weather` | `RegionalWeather` | Ya |
| Region | `TideChangedEvent` | `level`, `isHigh` | `TideManager` | Ya |
| Bencana | `DisasterStartedEvent` | `type`, `scale` | `DisasterManager` | Tidak |
| Bencana | `MajorDisasterStartedEvent` | `type`, `durationSeconds` | `DisasterManager` | Tidak |
| Bencana | `DisasterEndedEvent` | `type` | `DisasterManager` | Tidak |
| Bencana | `BloodMoonStartedEvent` | `day` | `HordeManager` | Tidak |
| Bencana | `BloodMoonEndedEvent` | `day` | `HordeManager` | Tidak |
| Suplai | `BeachSupplyArrivedEvent` | `day`, `crateCount` | `BeachSupplyManager` | Tidak |
| Suplai | `SupplyDropLandedEvent` | `position` | `SupplyDropManager` | Tidak |
| Base | `StructureBuiltEvent` | `structureId`, `buildableId` | `BuildManager` | Tidak |
| Base | `StructureDamagedEvent` | `structureId`, `hpRatio` | `BaseHealth` | Tidak |
| Base | `StructureDestroyedEvent` | `structureId`, `position` | `BaseHealth` | Tidak |
| Base | `BaseRaidResolvedEvent` | `damageTotal`, `npcInjured` | `RemoteBaseSimulation` | Tidak |
| NPC | `NPCRescuedEvent` | `npcId` | `LifeboatExtraction` | Tidak |
| NPC | `NPCDiedEvent` | `npcId`, `reason` | `NPCCompanion` | Tidak |
| Objektif | `DiaryChapterFoundEvent` | `chapter` | `ObjectiveManager` | Tidak |
| Objektif | `ObjectiveChangedEvent` | `objectiveId`, `isComplete` | `ObjectiveManager` | Ya |
| Objektif | `HelicopterPartInstalledEvent` | `partId`, `installed`, `total` | `RescueEventManager` | Ya |
| Objektif | `HelicopterReadyEvent` | Tanpa isi | `RescueEventManager` | Ya |
| Run | `RunStartedEvent` | `seed`, `isNewRun` | `RunLifecycle` | Ya |
| Run | `PhaseChangedEvent` | `phase` | `GameManager` | Ya |
| Run | `RunEndedEvent` | `RunResult`, `DeathReason` | `GameManager` | Tidak |
| Run | `SaveRequestedEvent` | `reason` | Pemicu autosave | Tidak |
| Run | `SaveCompletedEvent` | `succeeded` | `WorldStateSave` | Tidak |
| UI | `UIFocusChangedEvent` | `isGameplayBlocked`, `isPaused` | `UIFocusStack` | Ya |

### Contoh rujukan: musuh mati dan Lucky Machine

Dulu `EnemyAI` memanggil `LuckyMachine.AddEnergy()`, sehingga AI musuh harus mengenal mesin gacha. Sekarang:

1. `EnemyHealth` menerbitkan `EnemyKilledEvent`.
2. `LocalStatsManager` (#57) dan `AIDirector` (#27) mendengarnya.
3. `LuckyMachine` tidak bergantung pada event ini. Kuota tumbal dihitung saat musuh masuk kandang (#72).

`EnemyAI` tidak perlu tahu siapa yang mendengar.

## 3. Hubungan dengan Sistem Lain

- **#82 `RunLifecycle`:** memanggil `GameEventRegistry.ClearAll()` saat run berakhir, setelah semua pendengar sekali pakai selesai.
- **#56 `DevConsole`:** perintah `eventlog 1` menampilkan event yang terkirim.
- **Semua blueprint baru:** menulis event yang dikirim dan didengar di bagian 3.

## 4. Kasus Khusus

- **Objek pool:** `OnDisable` terpanggil saat objek kembali ke pool, jadi langganannya otomatis lepas. Objek tidak boleh menerima event saat nonaktif.
- **Game dijeda:** event tetap terkirim, tetapi sistem yang memakai `Time.deltaTime` tidak maju. Pendengar tidak perlu mengecek jeda.
- **Pendengar melempar exception:** `Publish` menangkap exception per pendengar di build Editor dan mencatatnya, supaya pendengar lain tetap jalan.
- **Publish di dalam publish:** diperbolehkan, tetapi rantainya tidak boleh kembali ke event yang sama. `eventlog` membantu menemukannya.
- **Permadeath dan save:** event tidak disimpan. Setelah memuat save, tiap manager mengirim ulang event sticky lewat `ISaveable` (#80).
- **Domain Reload dimatikan di Editor:** `GameEventRegistry` memanggil `ClearAll()` lewat `RuntimeInitializeOnLoadMethod`, supaya langganan lama tidak terbawa.

## 5. Keputusan Desain

**Keputusan yang dibuat di blueprint ini (usulan arsitektur, dicatat di #85):**

- Memakai event statis generik `EventChannel<T>`, bukan ScriptableObject.
- `DayNightDirector.OnDaybreak` dan `OnNightfall` diganti `DaybreakEvent` dan `NightfallEvent`.
- Nama event di issue lain, seperti `OnEnemyKilled` dan `OnMajorDisasterStarted`, berarti `EnemyKilledEvent` dan `MajorDisasterStartedEvent`.

**Menunggu developer:** tidak ada yang menghalangi.

## 6. Skenario Tes

1. Buat dua objek yang mendengar `NightfallEvent`, kirim event lewat `DevConsole`. **Hasil:** keduanya terpanggil sekali.
2. Matikan satu objek, kirim lagi. **Hasil:** hanya objek aktif yang terpanggil.
3. Pindah wilayah, lalu aktifkan objek baru yang membaca `RegionChangedEvent.Last`. **Hasil:** objek langsung tahu region aktif.
4. Kirim 1000 `EnemyHitEvent` dalam satu frame sambil membuka Profiler. **Hasil:** tidak ada alokasi GC dari `Publish`.
5. Mati lalu mulai run baru. **Hasil:** jumlah pendengar kembali ke nilai awal, dan tidak ada event ganda.
