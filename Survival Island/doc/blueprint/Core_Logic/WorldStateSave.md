# Blueprint: WorldStateSave.cs & ISaveable
**Lokasi:** `Survival Island/doc/blueprint/Core_Logic/WorldStateSave.md`
**Deskripsi Utama:** Penyimpanan seluruh kondisi run ke satu file, dengan autosave yang aman, ID stabil untuk objek dunia, urutan pemuatan yang pasti, dan aturan permadeath.
**Issue:** #80
**Rujukan GDD:** Bagian 1 dan 3 (Permadeath: game mengulang dari awal saat kalah). Format file, pemicu autosave, dan aturan saat pertempuran adalah usulan teknis, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

### Pola

`WorldStateSave` adalah manager persisten (`IBootstrapService`, urutan ke-16 di #82). Blueprint ini melengkapi dan menggantikan isi `SaveSystem.md`: kelas `GameData`, pemanggilan langsung ke manager, dan `File.WriteAllText` tidak dipakai lagi.

Tiap sistem menyimpan datanya sendiri lewat `ISaveable`, jadi `WorldStateSave` tidak perlu mengenal isi tiap sistem.

```csharp
public interface ISaveable
{
    string SaveId { get; }          // Stabil dan unik dalam satu run
    int LoadOrder { get; }          // Hanya untuk manager di tabel urutan. Objek region memakai 0
    string CaptureState();          // JSON milik sistem itu
    void RestoreState(string json); // Mengisi data, tanpa mengirim event
    void OnRestoreFinished();       // Mengirim ulang event sticky dan menyegarkan tampilan
}

public enum SaveReason { ArrivedOnIsland, WakeUp, Daybreak, QuitToMenu, AppQuit, Manual, BridgeComplete, DisasterEnded, PartInstalled }
public enum SaveProblem { None, Corrupt, NewerVersion, OldVersionNoMigration }
```

### Bentuk file

```csharp
[Serializable]
public class RunSaveEnvelope
{
    public int saveVersion;         // Nilai awal 1
    public string gameVersion;      // Application.version. Hanya catatan
    public uint payloadCrc32;
    public string payloadJson;      // RunSavePayload dalam bentuk JSON
}

[Serializable]
public class RunSavePayload
{
    public long savedAtUtcTicks;
    public string reason;
    public bool wasInCombat;
    public int engagedEnemyCount;
    public RunSeedSaveData seed;       // Dari RunSeedManager (#89)
    public GameRunSaveData game;       // currentDay, hasEscaped, shipOutcome, dan fase
    public SaveEntry[] entries;        // Semua ISaveable di scene dasar
    public string regionStoreJson;     // Dari RegionStateStore.ToSaveJson() (#88)
}

[Serializable]
public struct SaveEntry
{
    public string id;
    public string json;
}
```

`JsonUtility` tidak bisa menyimpan `Dictionary`. Karena itu, `entries` berupa array, `RegionStateStore` menyediakan bentuk daftarnya sendiri, dan sistem yang memakai `Dictionary` wajib mengubahnya ke array di `CaptureState`.

### Lokasi file

| File | Isi | Saat permadeath |
|---|---|---|
| `survival_save.json` | Save run aktif (#82) | **Dihapus** |
| `survival_save.json.tmp` | Penulisan yang sedang berjalan | **Dihapus** |
| `survival_save.json.bak` | Save sebelumnya, hanya untuk pemulihan dari kerusakan | **Dihapus** |
| `survival_save.corrupt.json` | Salinan save rusak, untuk laporan bug | **Dihapus** |
| `run_end.marker` | Penanda akhir run (#82) | Dihapus setelah alur selesai |
| `stats.json` | Statistik (#57) | **Tetap** |
| `lore_archive.json` | ID lore yang pernah ditemukan (#100) | **Tetap**. `DeleteRunSave` tidak menyentuh file ini |
| `PlayerPrefs` | Setting dan tombol (#18, #19) | **Tetap** |

Semua berada di `Application.persistentDataPath`.

### Variabel

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Instance` | `WorldStateSave` | `null` | Singleton persisten |
| `registered` | `List<ISaveable>` | Kosong | Manager terdaftar di scene dasar |
| `LastProblem` | `SaveProblem` | `None` | Hasil pemeriksaan terakhir. Dibaca `MainMenuController` |
| `autosaveEnabled` | `bool` | `true` | Dimatikan saat `RunLifecycle.State = Ending` |
| `pendingReason` | `SaveReason?` | `null` | Permintaan yang tertunda |
| `isWriting` | `bool` | `false` | Sedang menulis |
| `writeTask` | `Task<bool>` | `null` | Penulisan autosave yang berjalan di thread lain. Dibaca langsung oleh `WaitForPendingWrite` dan `TrySaveNow`, tanpa menunggu `Update` |
| `abortWrites` | `volatile bool` | `false` | Diisi `true` oleh `MarkRunEnded`. Penulisan yang masih berjalan dibatalkan sebelum `File.Replace` |
| `loadedPayload` | `RunSavePayload` | `null` | Hasil `ReadRunFile` selama proses load |

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.save.combatEnemyRadius` | 30 | Musuh aktif dalam radius ini menandakan pertempuran |
| `GameBalanceConfig.save.combatQuietSeconds` | 10 | Jeda setelah pemain terkena damage |
| `GameBalanceConfig.save.autosaveRetrySeconds` | 10 | Jeda mencoba lagi saat ditunda |
| `GameBalanceConfig.save.autosaveMaxDeferSeconds` | 120 | Batas menunggu. Lewat dari ini, permintaan dibuang |
| `GameBalanceConfig.save.autosaveMinHpRatio` | 0,1 | HP minimum untuk autosave |
| `GameBalanceConfig.save.combatRestoreMaxEnemies` | 10 | Batas musuh yang dimunculkan lagi setelah keluar saat pertempuran |
| `GameBalanceConfig.save.combatRestoreRadius` | 20 | Jarak munculnya |

## 2. Detail Logika & Fungsi

### Pendaftaran

| Jenis objek | Cara masuk save |
|---|---|
| Manager di `IslandPhase` | `OnEnable` memanggil `WorldStateSave.Instance.Register(this)`, dan `OnDisable` memanggil `Unregister(this)` |
| Objek region (pohon, peti loot, Diary, Lucky Machine, `LootSpawnerSystem`) | Tidak mendaftar. `RegionStreaming.CaptureRegion` dan `RestoreRegion` memanggil `ISaveable` lewat `RegionSceneRoot`, lalu menulis ke `RegionStateStore` (#88) |
| Struktur pemain | Tidak mendaftar. `StructureRegistry` menulis semuanya (#79) |
| `WorldItem` lepas | Lewat `WorldItemManager` ke `RegionStateStore` (#66) |
| `RunSeedManager`, `GameManager` | Persisten, dan dipanggil langsung lewat `CaptureSave` dan `RestoreRunData`, sesuai #82 dan #89 |
| Musuh | Tidak disimpan (#88) |

`Register` mencatat kesalahan kalau `SaveId` sudah terpakai.

### ID stabil

**`SaveIdTag : MonoBehaviour`**, dengan `[SerializeField] string saveId`, dipakai objek yang diletakkan tangan di scene region.

- **Format:** `<region>_<jenis>_<8 karakter>`, contoh `r1_tree_a3f9c21b`.
- **Pembuatan:** `OnValidate` di Editor mengisi `saveId` dari `GUID.Generate()` kalau kosong, atau kalau objek baru diduplikasi dan ID-nya bentrok.
- **Tidak berubah:** nama objek dan posisinya boleh berubah. ID tetap sama.
- **`IslandInteractable.saveId` dan `ResourceNode`:** membaca dari `SaveIdTag`.

| Objek | Sumber `SaveId` |
|---|---|
| Pohon, batu, peti loot, dan objek scene lain | `SaveIdTag` |
| `LootSpawnerSystem` | `spawnerId` (#89) |
| Struktur pemain | `structureId`, contoh `s_000123`, dari `StructureRegistry.NewStructureId()` (#79) |
| Suku cadang | `part_<indeks>`, dari `HeavyItemSystem` (#69) |
| Manager | Konstanta di tabel urutan, contoh `player_vitals` |

**`SaveIdAuditWindow`** memindai semua scene region dan melaporkan ID kosong, ID ganda, dan objek bertanda `ISaveable` tanpa `SaveIdTag`. Pemeriksaan ini wajib lulus sebelum build.

**ID yang hilang di versi baru:** data dengan `SaveId` yang tidak punya objek diabaikan dengan peringatan. Objek tanpa data memakai status awalnya.

### Isi data

| Data | `SaveId` | Pemilik | Isi |
|---|---|---|---|
| Seed dan susunan run | Dipanggil langsung | `RunSeedManager` (#89) | `RunSeedSaveData` |
| Hari dan fase | Dipanggil langsung | `GameManager` | `currentDay`, `hasEscaped`, `hasArrivedOnIsland`, `shipOutcome` dengan `lostNpcIds`, dan `wasTainted` |
| Jam dunia | `world_clock` | `DayNightDirector` | `currentTimeOfDay`, `TotalGameHours`, dan `isNight` |
| Akses region dan jembatan | `world_gates` | `RegionGateSystem` (#68) | Kayu tersetor, jembatan selesai, region terbuka, dan `hasBridgeBlueprint` |
| Status region | `regionStoreJson` | `RegionStateStore` (#88) | Node panen, peti loot, `WorldItem`, dan `lastUnloadedGameHour` |
| Tas | `player_carry` | `CarryCapacity` (#118) | `equippedBag` |
| Inventori | `player_inventory` | `InventoryManager` | Slot berisi, `ItemInstance`, dan `nextInstanceId` |
| Hotbar | `player_equipment` | `EquipmentManager` (#60) | `EquipmentSaveData` |
| Barang lempar | `world_throwables` | `ThrowableSystem` (#96) | `selectedIndex` |
| Status tubuh | `player_vitals` | `SurvivalStats` (#111) | `HP`, `Stamina`, `Hunger`, `Thirst`, `Infection`, `IsExhausted`, dan sisa pemulihan |
| Penyakit dan efek | `player_disease`, `player_effects` | #45, #20 | Penyakit aktif dan sisa waktu efek. Infeksi zombie tidak punya entri sendiri: nilai `Infection` ada di `player_vitals`, dan tahap dihitung ulang `InfectionSystem` (#64) saat dimuat |
| Kehangatan | `player_warmth` | `WarmthSystem` (#65) | Nilai kehangatan dan pakaian |
| Posisi pemain | `player_transform` | `IslandPlayerController` | `GetSafeSavePosition()` (#101), `yaw`, dan `pitch` (#102) |
| Bangunan | `world_structures` | `StructureRegistry` (#79) | `StructureRecord[]`, HP, status pintu (#78), dan isi peti (#46) |
| Suku cadang | `world_parts` | `HeavyItemSystem` (#69) | Posisi dan status tiap barang |
| Helikopter | `world_helicopter` | `RescueEventManager` (#14) | Bagian terpasang |
| Diary dan objektif | `world_objectives` | `ObjectiveManager` (#81) | Status buku, bab terbuka, dan objektif |
| Resep terbuka | `world_recipes` | `CraftingManager`, `TechTreeManager` (#77) | Node dan resep |
| NPC | `world_npcs` | `NPCRoster` (#76) | Posisi, state, HP, dan yang sudah mati |
| Bencana | `world_disasters` | `DisasterManager` (#70) | Jadwal terpakai dan cooldown |
| Blood Moon | `world_bloodmoon` | `HordeManager` (#50) | `lastBloodMoonDay`, `IsActive`, `activeDay`, `wavesSpawned`, dan `waveTimer` |
| AI Director | `ai_director` | `AIDirector` (#27) | `spawnedTonight`, `CurrentPhase`, `Intensity`, `mobTimer`, dan `phaseTimer` |
| Api | `world_fire` | `FireSystem` (#97) | `{ structureId, burnRemaining, generation }` |
| Lore | `world_lore` | `LoreCollectibles` (#100) | Temuan lore dalam run ini |
| Barang hanyut | `world_debris` | `DebrisRetrieval` (#106) | `nextSpawnGameHour` |
| Kargo pantai | `world_supply_beach` | `BeachSupplyManager` (#71) | Kargo yang belum diambil |
| Hasil simulasi base | `world_remotebase` | `RemoteBaseSimulation` (#90) | Hasil yang belum diterapkan |
| Catatan hadiah Lucky Machine | `world_lucky` | `LuckyRewardLedger` (#72) | `grantCounts` sebagai array, dan `pendingSkins`, bersama untuk keempat mesin |
| Lucky Machine | Lewat `RegionStateStore`, dengan `saveId` dari `SaveIdTag` | `LuckyMachine` (#72) | `quotaProgress`, `storedSpins`, `freeWindowEndGameHour`, `freeSpinsUsed`, dan `gateState`, per mesin. `grantCounts` dan `pendingSkins` ada di `world_lucky`. Buff tersimpan di `player_effects`, sedangkan modifier dan skin di `ItemInstance` |

**`LightFixture` (#98):** `ISaveable` dengan `SaveId = StructureId`, berisi `fuelSeconds`, `IsLit`, dan `lastTickGameHour`. Dipulihkan setelah `world_structures`.

**Tidak disimpan:** musuh, state AI, panel UI, `Time.timeScale`, status tidur, hologram, isi pool, audio, dan efek. Semuanya dihitung ulang atau dimulai dari awal.

**Sudah di luar issue ini:** isi peti diatur #46, statistik di #57, dan setting di #19.

### Menulis save

**`private void OnSaveRequested(SaveRequestedEvent e)`** mencatat alasan, lalu memanggil `TrySaveNow(reason)`.

**`private bool TrySaveNow(SaveReason reason)`**:

1. Kalau `!autosaveEnabled`, atau `RunLifecycle.State != OnIsland`, terbitkan `SaveCompletedEvent { reason, succeeded = false }`, lalu kembalikan `false`. Dengan begitu `RunLifecycle` yang menunggu `QuitToMenu` tidak menunggu sampai batas waktu.
2. Kalau alasan termasuk autosave dan `!IsSafeToAutosave()`, simpan `pendingReason`, lalu mulai `DeferLoop()`, dan kembalikan `false`.
3. Kalau `isWriting`:
   - **Autosave:** simpan `pendingReason`, lalu selesai. Permintaan setelah penulisan selesai hanya berjalan sekali.
   - **`QuitToMenu`:** jalankan coroutine `QuitAfterPendingWrite()`, yang memanggil `yield return WaitForPendingWrite(run.saveWaitTimeoutSeconds)`, lalu melanjutkan langkah 4 sampai 8 dengan alasan `QuitToMenu`. Thread utama tidak diblokir, dan `RunLifecycle` tetap menghitung batas waktunya sendiri. Kembalikan `false` untuk pemanggilan ini, karena hasilnya dikirim lewat `SaveCompletedEvent`.
   - **`AppQuit`:** pengecualian dari janji "thread utama tidak diblokir" di bagian Permadeath. `writeTask.Wait(run.saveWaitTimeoutSeconds)` dijalankan di tempat, paling lama 5 detik, lalu lanjut ke langkah 4. Coroutine tidak dipakai, karena proses bisa berakhir atau dijeda (`OnApplicationPause`) sebelum frame berikutnya.
   - Permintaan keluar tidak pernah ditunda ke `pendingReason`, karena scene akan segera dibuang.
4. `RegionStreaming.Instance.CaptureLoadedRegions()`.
5. Bangun `RunSavePayload`. Panggil `CaptureState()` pada tiap `registered`, urut `LoadOrder`. Exception dari satu sistem menggagalkan save itu, bukan menulis file setengah jadi.
6. Ubah ke JSON, hitung `payloadCrc32`, lalu bangun `RunSaveEnvelope`.
7. Tulis lewat `WriteAtomic(mainPath, json, abortable: true)`.
8. Terbitkan `SaveCompletedEvent { reason, succeeded }`. `reason` adalah alasan yang benar-benar ditulis, sehingga selesainya autosave tidak terbaca sebagai selesainya save keluar.

**Kapan tiap alasan dipakai:**

| Alasan | Pengirim | Lolos pemeriksaan pertempuran |
|---|---|---|
| `ArrivedOnIsland` | `RunLifecycle` | Ya, selalu tulis |
| `WakeUp` | `SleepSystem` (#67) | Diperiksa |
| `Daybreak` | `WorldStateSave`, dari `DaybreakEvent` | Diperiksa |
| `QuitToMenu` | `RunLifecycle` | Ya, selalu tulis |
| `AppQuit` | `Application.wantsToQuit`, dan `OnApplicationPause(true)` di Android | Ya, selalu tulis |
| `Manual` | `DevConsole` (#56) | Ya, selalu tulis |
| `BridgeComplete` | `RegionGateSystem` (#68) | Diperiksa |
| `DisasterEnded` | `DisasterManager` (#70) | Diperiksa |
| `PartInstalled` | `RescueEventManager` (#14) | Diperiksa |

### Menulis tanpa merusak file lama

**`private bool WriteAtomic(string path, string json, bool abortable)`**:

1. Tulis ke `path + ".tmp"` lewat `FileStream`.
2. `stream.Flush(true)`, supaya data benar-benar sampai ke disk.
3. Kalau `abortable` dan `abortWrites` bernilai `true`, hapus `tmp` dan kembalikan `false`. Pemeriksaan ini hanya berlaku untuk save run (`survival_save.json`, `abortable: true`). `run_end.marker` ditulis dengan `abortable: false`, jadi tidak ikut dibatalkan. Selain itu, kalau file `path` ada, panggil `File.Replace(tmp, path, bak)` (`.bak` hanya untuk save run; marker memakai `null`). Kalau tidak ada, `File.Move(tmp, path)`.
4. Kalau `IOException`, hapus `tmp` kalau perlu, catat kesalahan, dan kembalikan `false`. File lama tetap utuh.

Kalau game mati sebelum langkah 3, file utama masih versi lama. Kalau mati sesudahnya, file baru sudah lengkap.

**Thread:** langkah 4 sampai 6 di `TrySaveNow` berjalan di thread utama, supaya datanya konsisten. `WriteAtomic` untuk autosave berjalan di `Task.Run`, disimpan di `writeTask`, dengan `isWriting = true`. Hasilnya dibaca di `Update` lewat `writeTask.IsCompleted`, dan `SaveCompletedEvent` terkirim dari thread utama. Untuk `QuitToMenu` dan `AppQuit`, `WriteAtomic` berjalan langsung, karena proses mungkin segera berakhir.

Waktu tangkap pada base 400 struktur harus diukur. Target: di bawah 50 ms. Waktu ini dilakukan pada pagi hari, saat ancaman sudah hilang.

### Pemicu autosave

| Pemicu | Waktu | Alasan |
|---|---|---|
| Tiba di pulau | Sekali, setelah `EnterIslandPhase` | Save pertama (#82) |
| **Pagi hari** | `DaybreakEvent`, jam 06:00 | Zombie baru saja terbakar. Lebih aman daripada 24:00, yang berada di tengah malam |
| Bangun tidur | `SleepSystem`, kalau bangun normal | Titik simpan alami, dan pemain merasa aman |
| Keluar game | `QuitToMenu`, `AppQuit` | Tidak kehilangan progres |

Bangun saat fajar mengirim `WakeUp` dan `Daybreak` hampir bersamaan. `pendingReason` menggabungkannya menjadi satu penulisan.

### Save saat pertempuran

**`private bool IsSafeToAutosave()`** bernilai `false` kalau salah satu berlaku:

| Kondisi | Sumber | Alasan |
|---|---|---|
| Ada musuh aktif dalam `combatEnemyRadius` | `PhysicsQuery.OverlapSphere` dengan `PhysicsMasks.EnemyBody` | Hindari menyimpan keadaan dikejar |
| Belum lewat `combatQuietSeconds` sejak `PlayerDamagedEvent` | Event | Sama |
| Bencana aktif | `DisasterStartedEvent` dan `DisasterEndedEvent` | Lingkungan berbahaya |
| Diterkam | `PlayerPinnedEvent` | Tidak bisa bergerak |
| Sedang tenggelam atau membeku | `VitalWarningChangedEvent` | Keadaan buruk akan langsung berlanjut saat dimuat |
| Lapar atau haus nol | `VitalWarningChangedEvent` | Sama |
| `HP / MaxHP < autosaveMinHpRatio` | `VitalsChangedEvent` | Sama |
| `SleepSystem.IsSleeping` | Dibaca langsung | Pemicu bangun menyusul |
| `PlayerTraversal` sedang jatuh atau memanjat | Dibaca langsung | Tetap memakai posisi aman (#101), tetapi tunda supaya tepat |

**`private IEnumerator DeferLoop()`**, dengan `WaitForSecondsRealtime(autosaveRetrySeconds)`:

1. Lewati pemeriksaan saat `UIFocusStack.Instance.IsPaused`.
2. Kalau `IsSafeToAutosave()`, jalankan `TrySaveNow(pendingReason)`, lalu selesai.
3. Kalau sudah lewat `autosaveMaxDeferSeconds`, buang permintaan. File terakhir tetap dipakai. Autosave berikutnya mencoba lagi.

Dengan aturan ini, autosave tidak pernah mengurung pemain dalam keadaan hampir pasti mati.

**Keluar saat pertempuran:** `QuitToMenu` dan `AppQuit` tetap menulis, karena pemain tidak boleh kehilangan run. Supaya bukan jalan pintas keluar dari bahaya, payload mencatat `engagedEnemyCount` dari `SpawnManager.Instance.EngagedEnemyCount`, dan `wasInCombat`, yang bernilai `true` kalau jumlahnya lebih dari 0 (#62). Saat `FinishRestore`, kalau `wasInCombat` dan saat itu malam, panggil `SpawnManager.Instance.RequestAmbush(posisiPemain, Mathf.Min(engagedEnemyCount, combatRestoreMaxEnemies), combatRestoreRadius)`.

### Mencegah save-scumming

| Aturan | Cara |
|---|---|
| Satu slot | Hanya satu file run. Tidak ada pilihan slot |
| Tanpa load manual | Tidak ada menu muat. Save hanya dibaca oleh tombol Lanjutkan |
| Tanpa simpan manual | Tidak ada tombol simpan. Hanya `QuitToMenu` dan autosave |
| Kematian menghapus save | `RunLifecycle` memakai `MarkRunEnded`, lalu `DeleteRunSave` |
| Menutup paksa saat sinematik | `run_end.marker` tetap menghapus save saat boot berikutnya (#82) |
| Hasil acak tidak bisa diulang | Status aliran acak disimpan (#89) |
| `.bak` tidak bisa dipilih pemain | Hanya dibaca kalau file utama rusak |
| Perintah `save` dan `loadlast` | Hanya di build pengembangan (#56) |

**Batas yang masih ada:** kalau pemain mematikan proses sebelum mati, dan `run_end.marker` belum dibuat, game kembali ke autosave terakhir. Hal ini tidak bisa dibedakan dari game yang crash. Pilihan menghapus save saat dibuka akan menghukum pemain yang benar-benar terkena crash. Risiko ini dibatasi karena autosave berjalan setiap pagi. Keputusannya dicatat di bagian 5.

### Membaca dan memulihkan

Urutan ini mengikuti `RunLifecycle.ContinueRun()`.

**`public bool HasContinuableSave()`**:

1. Kalau `run_end.marker` ada, kembalikan `false`.
2. Kalau file utama tidak ada, kembalikan `false`.
3. Baca dan periksa amplop lewat `ValidateEnvelope`. Isi `LastProblem`.
4. Kembalikan `LastProblem == None`. Hasilnya dicache sampai file berubah.

**`public bool ReadRunFile()`** mengurai `payloadJson` ke `loadedPayload`, lalu mengembalikan seed dan data game untuk #89 dan `GameManager`.

**`public void RestoreBaseScene()`**, setelah `IslandPhase` dimuat:

| Urutan | `LoadOrder` | Yang dipulihkan | Alasan posisinya |
|---|---|---|---|
| 1 | 10 | `world_clock` | Sistem lain membaca `TotalGameHours` |
| 2 | 20 | `regionStoreJson` | Harus ada sebelum region dimuat (#88) |
| 3 | 30 | `world_gates` | `RegionStreaming` membaca `IsUnlocked` |
| 4 | 40 | `player_carry` | Slot tas harus benar sebelum isi inventori (#118) |
| 5 | 50 | `player_inventory` | Sumber `ItemInstance` |
| 6 | 60 | `player_equipment`, `world_throwables` | Merujuk `instanceId` (#60), dan barang lempar di inventori (#96) |
| 7 | 70 | `player_vitals`, `player_disease`, `player_effects`, `player_warmth` | Mengisi status |
| 8 | 80 | `player_transform` | Menentukan region awal |
| 9 | 90 | `world_structures` | Memakai `GetStructureRoot`, dan item untuk peti |
| 10 | 100 | `world_fire`, `LightFixture` per `StructureId`, `world_parts`, `world_helicopter` | Data pulau. `world_fire` dan `LightFixture` merujuk struktur, jadi setelah `world_structures` |
| 11 | 110 | `world_objectives`, `world_recipes`, `world_lore` | Data progres |
| 12 | 120 | `world_npcs` | Butuh posisi pemain dan bangunan |
| 13 | 130 | `world_disasters`, `world_bloodmoon`, `world_supply_beach`, `world_remotebase`, `ai_director`, dan `world_debris` | Merujuk data di atas |

`RestoreState` hanya mengisi data dan tidak boleh mengirim event. Exception ditangkap per sistem, dicatat, lalu sistem itu memakai nilai awal.

**Tahap berikutnya:**

1. `RegionStreaming.LoadInitialRegions(posisiPemain)`. Objek region mengambil status dari `RegionStateStore` (#88).
2. **`public void FinishRestore()`**: panggil `OnRestoreFinished()` pada tiap `registered`, urut `LoadOrder`, lalu kosongkan `loadedPayload`. Pada tahap ini event sticky dikirim ulang, sesuai `GameEventBus.md`. Terakhir, jalankan aturan keluar saat pertempuran.

### Nomor versi

`SaveVersions.Current` bernilai 1 dan naik setiap format berubah. `gameVersion` hanya catatan, jadi perbaikan kecil tidak membatalkan save.

**`private SaveProblem ValidateEnvelope(RunSaveEnvelope e)`**:

| Kondisi | Hasil | Tindakan |
|---|---|---|
| JSON gagal diurai, atau CRC tidak cocok | `Corrupt` | Alur file rusak |
| `saveVersion > Current` | `NewerVersion` | Tombol Lanjutkan tidak tampil. Pesan bahwa save berasal dari versi lebih baru. File tidak diubah |
| `saveVersion < Current`, dan ada migrasi | `None` | `SaveMigrator` menjalankan rantai `Migrate_1_to_2`, dan seterusnya, pada `payloadJson` |
| `saveVersion < Current`, tanpa migrasi | `OldVersionNoMigration` | Tombol Lanjutkan tidak tampil. Pesan bahwa run lama tidak didukung |
| Sama | `None` | Lanjut |

### Save rusak

Alur saat `Corrupt`:

1. Salin file utama ke `survival_save.corrupt.json`, menimpa salinan lama.
2. Coba `survival_save.json.bak`. Kalau sah, pakai isinya sebagai file utama, lalu tampilkan notifikasi `Save terakhir rusak. Memakai save sebelumnya`.
3. Kalau `.bak` juga gagal, `LastProblem = Corrupt`. `MainMenuController` menampilkan pesan bahwa run tidak bisa dilanjutkan, dan Lanjutkan tidak tampil. Run Baru tetap tersedia.

**Data per sistem rusak:** kalau `RestoreState` satu sistem gagal setelah CRC lolos, sistem itu memakai nilai awal. Rujukan seperti `partPointByPart` yang keluar dari rentang (#89) dianggap `OldVersionNoMigration`.

**File `.tmp` tertinggal:** dihapus saat `Initialize()`. Kalau file utama hilang tetapi `.tmp` sah, `.tmp` tidak dipakai, karena penulisannya mungkin belum selesai. `.bak` yang dipakai.

### Permadeath

**`public IEnumerator WaitForPendingWrite(float timeoutSeconds)`**, dijalankan `RunLifecycle` sebelum `MarkRunEnded`: tunggu sampai `writeTask == null || writeTask.IsCompleted`, dengan `yield return null` dan waktu tak terskala, paling lama `timeoutSeconds`. Fungsi ini membaca `writeTask` langsung, bukan `isWriting`, karena `isWriting` baru dikosongkan di `Update`. Thread utama tidak diblokir, kecuali jalur `AppQuit` di `TrySaveNow` langkah 3.

**`public void MarkRunEnded(RunResult result, DeathReason reason)`**: isi `abortWrites = true`, lalu tulis `run_end.marker` lewat `WriteAtomic(markerPath, json, abortable: false)`. Marker tidak terkena `abortWrites`, jadi selalu tertulis. Kalau penulisan autosave masih berjalan setelah batas waktu, penulisan itu berhenti di langkah 3 `WriteAtomic` dan tidak bisa memunculkan save lagi setelah `DeleteRunSave`.

**`public void DeleteRunSave()`** menghapus `survival_save.json`, `.tmp`, `.bak`, dan `.corrupt.json`. Fungsi ini juga mengosongkan cache `HasContinuableSave`. File `stats.json` dan `PlayerPrefs` tidak disentuh. `PlayerPrefs.DeleteAll()` dilarang (#82).

**`public void ClearRunEndMarker()`** menghapus penanda.

**`public void SetAutosaveEnabled(bool enabled)`** mengisi `autosaveEnabled`. `RunLifecycle` mematikannya pada langkah 1 alur akhir run.

**`public bool TryReadRunEndMarker(out RunEndMarker m)`** dipakai `RecoverInterruptedEnd()` (#82).

**`public void ResetForNewRun()`**: `pendingReason = null`, `autosaveEnabled = true`, `abortWrites = false`, `writeTask = null`, `loadedPayload = null`, dan `registered` dikosongkan.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `SaveCompletedEvent` (`reason`, `succeeded`).

**Event yang didengar:** `SaveRequestedEvent`, `DaybreakEvent`, `PlayerDamagedEvent`, `VitalsChangedEvent`, `VitalWarningChangedEvent`, `PlayerPinnedEvent`, `DisasterStartedEvent`, `DisasterEndedEvent`, dan `RunEndedEvent` (mematikan autosave).

| Sistem | Hubungan |
|---|---|
| `RunLifecycle` (#82) | Memanggil `HasContinuableSave`, `ReadRunFile`, `RestoreBaseScene`, `FinishRestore`, `SetAutosaveEnabled`, `WaitForPendingWrite`, `MarkRunEnded`, `DeleteRunSave`, `ClearRunEndMarker`, dan `TryReadRunEndMarker` |
| `RegionStreaming` (#88) | `CaptureLoadedRegions`, `RegionStateStore`, dan pemulihan objek region |
| `RunSeedManager` (#89), `GameManager` | `CaptureSave`, `RestoreFromSave`, dan `RestoreRunData` |
| `StructureRegistry` (#79) | `world_structures`, termasuk #46 dan #78 |
| `SleepSystem` (#67) | Mengirim `SaveRequestedEvent` dengan alasan `WakeUp` |
| `SpawnManager` (#62) | `RequestAmbush` setelah keluar saat pertempuran |
| Semua pemilik data di tabel isi | Memakai `ISaveable` |
| `DevConsole` (#56) | Perintah `save`, `loadlast`, dan `savecorrupt` |
| `NotificationManager` (#22) | Pesan kegagalan |
| `LocalStatsManager` (#57), `SettingsManager` (#19) | File terpisah, tidak disentuh |

**Batas pekerjaan:** isi peti di #46, statistik di #57, dan pengaturan di #19.

## 4. Kasus Khusus

- **Objek pool:** tidak boleh mendaftar lewat `Register`. `WorldItem` lewat `WorldItemManager`. Objek pool yang kembali pada `Unloading` sudah menulis statusnya lebih dulu (#88).
- **Game dijeda:** save `QuitToMenu` berjalan saat `Pause` terbuka, karena `Update` dan `File` tidak memakai `Time.timeScale`. Autosave tidak dimulai dan `DeferLoop` melewati pemeriksaan saat dijeda.
- **Permadeath:** lihat bagian Permadeath. Autosave mati sejak langkah 1 pada alur akhir run, jadi save baru tidak bisa muncul setelah pemain mati.
- **Save dan load di Fase 1:** tidak ada (#82). `TrySaveNow` menolak saat `State` bukan `OnIsland`.
- **Disk penuh:** `WriteAtomic` mengembalikan `false`, dan file lama tetap utuh. Notifikasi `Gagal menyimpan` tampil. `RunLifecycle` menampilkan `ConfirmDialog` saat keluar.
- **Dua permintaan sekaligus:** digabung lewat `pendingReason`.
- **Menang atau kalah saat menulis:** `RunLifecycle` menjalankan `WaitForPendingWrite(saveWaitTimeoutSeconds)` (#82) sebagai coroutine, lalu `MarkRunEnded`. Kalau batas waktu habis, `abortWrites` membatalkan penulisan yang tersisa sebelum `File.Replace`. `DeleteRunSave` tetap dijalankan sesudahnya, dan save tidak bisa muncul kembali.
- **Keluar saat autosave sedang menulis:** `QuitToMenu` menunggu `writeTask` lalu menulis sendiri, dan `SaveCompletedEvent` membawa `reason = QuitToMenu`. `RunLifecycle` tidak salah membaca selesainya autosave sebagai selesainya save keluar.
- **Save saat region sedang dimuat:** `CaptureLoadedRegions` hanya membaca region `Loaded`. Region yang `Loading` memakai data di `Store`.
- **Save saat runtuhan berjalan:** dibahas di #79.
- **`RestoreState` di objek nonaktif:** struktur region yang tidak dimuat dibuat di bawah akar nonaktif, sehingga `Awake` belum berjalan. Sistem seperti itu tidak boleh mengandalkan `Awake` (#79).
- **Sistem belum ada pada versi awal:** `SaveEntry` tanpa pemilik diabaikan, dan manager tanpa data memakai nilai awal. Dengan begitu, sistem usulan yang nonaktif tidak merusak save.
- **Pemain dimuat di dalam bangunan baru:** tidak terjadi, karena posisi dan bangunan disimpan pada waktu yang sama.
- **Item dipindah ke `savedShipLoot`:** daftar ini kosong setelah diterapkan (#74), jadi save tidak menggandakan barang.
- **Autosave pertama gagal:** `ArrivedOnIsland` gagal berarti belum ada file. Lanjutkan tidak tampil, dan pemain diberi tahu lewat notifikasi.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** nama file save dan fungsi publik (`RunLifecycle.md`). Fase 1 tanpa save (`RunLifecycle.md`). `RegionStateStore` dan urutan pemuatan region (`RegionStreaming.md`). Musuh tidak disimpan (`RegionStreaming.md`). Event dikirim ulang setelah load (`GameEventBus.md`).

**Keputusan teknis di blueprint ini:**

- `ISaveable` bekerja dengan JSON per sistem, dibungkus satu amplop.
- `RestoreState` tidak boleh mengirim event. Event dikirim di `OnRestoreFinished`.
- `saveVersion` terpisah dari versi game.
- Penulisan memakai file sementara dan `File.Replace`.
- `CaptureState` berjalan di thread utama, dan penulisan autosave berjalan di thread lain.

**Usulan yang disetujui developer sebagai nilai awal (dicatat di #85):**

1. **Pemicu autosave:** tiba di pulau, pagi hari, bangun tidur, dan keluar game. "Pergantian hari" diartikan pagi pukul 06:00, karena pukul 24:00 berada di tengah serangan.
2. **Aturan pertempuran:** autosave ditunda saat ada musuh aktif dalam 30 meter, 10 detik setelah diserang, saat bencana, diterkam, tenggelam, membeku, lapar atau haus nol, dan HP di bawah 10 persen. Permintaan dibuang setelah 120 detik.
3. **Keluar saat pertempuran:** tetap disimpan, dan saat malam musuh muncul lagi di sekitar pemain, paling banyak 10. Rekomendasi: pakai, supaya keluar game tidak menjadi cara kabur.
4. **Mematikan proses sebelum mati:** kembali ke autosave terakhir. Tidak dicegah lebih jauh, supaya pemain yang terkena crash tidak kehilangan run. Pilihan lain adalah menghapus save setiap kali dibuka, seperti sebagian game roguelike. Rekomendasi: tidak memakai pilihan itu.
5. **Cadangan `.bak`:** dipakai hanya saat save rusak. Pemain bisa kehilangan progres satu autosave, tetapi tidak kehilangan seluruh run.
6. **Save rusak total atau versi tidak cocok:** run tidak bisa dilanjutkan, dengan pesan jelas, dan file tidak dihapus sampai Run Baru dimulai.

## 6. Skenario Tes

1. Selesaikan Fase 1. **Hasil:** `survival_save.json` muncul dengan `saveVersion` 1.
2. Tebang pohon, bangun dinding, setor kayu, simpan, keluar, lalu lanjutkan. **Hasil:** pohon tetap tumbang, dinding sama, dan kayu tersetor sama.
3. Catat `Hunger`, jam dunia, dan posisi, keluar, lalu lanjutkan. **Hasil:** nilainya sama, dan jam berjalan dari titik itu.
4. Ambil satu suku cadang, pasang ke helikopter, buka Diary Bab 2, simpan, lalu lanjutkan. **Hasil:** semua progres tetap ada.
5. Kunjungi Region 2, kembali sampai region dilepas, keluar, lalu lanjutkan. **Hasil:** status Region 2 tetap benar saat dikunjungi lagi.
6. Jalankan `SaveIdAuditWindow` pada scene dengan objek diduplikasi. **Hasil:** ID ganda diperbaiki atau dilaporkan.
7. Lihat `eventlog 1` saat lanjutkan. **Hasil:** event sticky terkirim sesudah semua `RestoreState` selesai.
8. Hentikan proses lewat Task Manager saat penulisan, dengan penundaan buatan dari `savedelay 5` di `DevConsole`. **Hasil:** file utama masih utuh, dan `.tmp` terhapus saat boot.
9. Jalankan `savecorrupt` untuk mengubah satu byte file utama, lalu buka menu. **Hasil:** `.bak` dipakai, dan notifikasi tampil.
10. Rusakkan file utama dan `.bak`. **Hasil:** Lanjutkan tidak tampil, pesan muncul, dan `.corrupt.json` tersedia. Run Baru berjalan.
11. Ubah `saveVersion` menjadi 99. **Hasil:** ditolak dengan pesan versi lebih baru, dan file tidak berubah.
12. Ubah `saveVersion` menjadi 0 tanpa migrasi. **Hasil:** ditolak dengan pesan versi lama.
13. Tunggu pagi saat zombie masih dalam 30 meter. **Hasil:** autosave ditunda. Setelah aman, satu autosave berjalan.
14. Bertarung terus lebih dari 120 detik dari pemicu. **Hasil:** permintaan dibuang, dan file tidak berubah.
15. Bangun saat fajar. **Hasil:** hanya satu penulisan.
16. Keluar ke menu saat malam dengan 5 zombie mengejar, lalu lanjutkan. **Hasil:** sekitar 5 zombie muncul 20 meter dari pemain.
17. Mati. **Hasil:** semua file save hilang, `stats.json` tetap ada, dan setting tidak berubah.
18. Matikan proses saat sinematik kematian, lalu buka lagi. **Hasil:** Lanjutkan tidak tampil, sesuai #82.
19. Tekan Esc saat `GameOver`, lalu cari tombol muat. **Hasil:** tidak ada pilihan muat di mana pun.
20. Buat base 400 struktur, lalu jalankan autosave sambil membuka Profiler. **Hasil:** waktu tangkap tercatat. Di atas 50 ms harus diperbaiki.
21. Isi disk uji sampai penuh, lalu simpan. **Hasil:** notifikasi gagal, dan file lama tetap bisa dimuat.
22. Lanjutkan lalu ambil loot, keluar, dan ulangi lima kali. **Hasil:** barang tidak berlipat, dan ukuran file stabil.
23. Dapatkan `lucky_laser` dan satu `lucky_hp`, simpan, keluar, lalu lanjutkan. **Hasil:** `grantCounts` sama, `lucky_laser` tidak keluar lagi, dan buff tetap aktif.
24. Jalankan `savedelay 3`, picu autosave pagi, lalu segera pilih Simpan dan Keluar ke Menu. **Hasil:** `SaveCompletedEvent` dengan `reason = Daybreak` tidak menutup penantian, lalu save `QuitToMenu` ditulis dan `SaveCompletedEvent { reason = QuitToMenu }` terkirim. Saat dilanjutkan, progres terakhir ada.
25. Jalankan `savedelay 10`, picu autosave, lalu turunkan HP ke 0. **Hasil:** alur mati menunggu paling lama 5 detik tanpa membekukan frame, penulisan autosave dibatalkan, dan setelah alur selesai tidak ada `survival_save.json`.
26. Ulangi tes 25, lalu matikan proses tepat setelah `MarkRunEnded`. **Hasil:** `run_end.marker` tetap tertulis walaupun `abortWrites = true`, dan boot berikutnya menyelesaikan permadeath.
27. Jalankan `savedelay 3`, picu autosave, lalu segera pilih Simpan dan Keluar ke Menu. **Hasil:** frame tidak membeku selama menunggu `writeTask`, lalu save `QuitToMenu` tertulis.

## 7. Catatan Implementasi (Langkah 1)

Kode: `WorldStateSave.cs`, `WorldStateSaveData.cs`, `ISaveable.cs`, dan `SaveIdTag.cs` di `Survival Island/Assets/LOGIC/Core_Logic/`. Tidak ada kelas `SaveSystem`.

- **Hasil `ReadRunFile`:** dibaca lewat properti `LoadedSeed` dan `LoadedGame`, karena fungsi itu mengembalikan `bool`.
- **Penggabungan autosave:** permintaan autosave (`WakeUp`, `Daybreak`, `BridgeComplete`, `DisasterEnded`, `PartInstalled`) ditampung dan diproses di `LateUpdate`, jadi permintaan pada frame yang sama hanya ditulis sekali (tes 15). Saat dijeda, permintaan menunggu.
- **`QuitToMenu` saat autosave lama belum selesai setelah batas waktu:** dianggap gagal (`SaveCompletedEvent { QuitToMenu, false }`), bukan menulis bersamaan. Dua penulisan bersamaan bisa saling menimpa lewat `File.Replace`. `RunLifecycle` lalu menampilkan `ConfirmDialog`.
- **`AppQuit` saat menulis:** kalau `writeTask.Wait` habis waktu, save `AppQuit` dibatalkan dengan log kesalahan.
- Penulisan langsung (tanpa thread lain) hanya untuk `QuitToMenu` dan `AppQuit`. Alasan lain, termasuk `ArrivedOnIsland` dan `Manual`, memakai `Task.Run`.
- Belum ada: `RegionStateStore`/`CaptureLoadedRegions` (#88), cek musuh di sekitar pemain, `PlayerDamagedEvent`, `VitalWarningChangedEvent`, rasio HP, `SleepSystem`, `PlayerTraversal`, `SpawnManager.RequestAmbush`, notifikasi #22, dan `SaveIdAuditWindow`. Semuanya ditandai TODO di kode.
