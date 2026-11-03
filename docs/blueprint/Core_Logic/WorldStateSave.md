# Blueprint: WorldStateSave.cs & ISaveable
**Lokasi:** `docs/blueprint/Core_Logic/WorldStateSave.md`
**Deskripsi Utama:** Penyimpanan seluruh kondisi run ke satu file, dengan autosave yang aman, ID stabil untuk objek dunia, urutan pemuatan yang pasti, dan aturan permadeath.
**Issue:** #80
**Rujukan GDD:** Bagian 1 dan 3 (Permadeath: game mengulang dari awal saat kalah). Format file, pemicu autosave, dan aturan saat pertempuran adalah usulan teknis, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

### Pola

`WorldStateSave` adalah manager persisten (`IBootstrapService`, urutan ke-12 di #82). Blueprint ini melengkapi dan menggantikan isi `SaveSystem.md`: kelas `GameData`, pemanggilan langsung ke manager, dan `File.WriteAllText` tidak dipakai lagi.

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

public enum SaveReason { ArrivedOnIsland, WakeUp, Daybreak, QuitToMenu, AppQuit, Manual }
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
| Hari dan fase | Dipanggil langsung | `GameManager` | `currentDay`, `hasEscaped`, dan `shipOutcome` |
| Jam dunia | `world_clock` | `DayNightDirector` | `currentTimeOfDay`, `TotalGameHours`, dan `isNight` |
| Akses region dan jembatan | `world_gates` | `RegionGateSystem` (#68) | Kayu tersetor, jembatan selesai, dan region terbuka |
| Status region | `regionStoreJson` | `RegionStateStore` (#88) | Node panen, peti loot, `WorldItem`, dan `lastUnloadedGameHour` |
| Tas | `player_carry` | `CarryCapacity` (#118) | `equippedBag` |
| Inventori | `player_inventory` | `InventoryManager` | Slot berisi, `ItemInstance`, dan `nextInstanceId` |
| Hotbar | `player_equipment` | `EquipmentManager` (#60) | `EquipmentSaveData` |
| Status tubuh | `player_vitals` | `SurvivalStats` (#111) | `HP`, `Stamina`, `Hunger`, `Thirst`, `Infection`, `IsExhausted`, dan sisa pemulihan |
| Infeksi, penyakit, dan efek | `player_infection`, `player_disease`, `player_effects` | #64, #45, #20 | Tahap dan sisa waktu |
| Posisi pemain | `player_transform` | `IslandPlayerController` | `GetSafeSavePosition()` (#101), `yaw`, dan `pitch` (#102) |
| Bangunan | `world_structures` | `StructureRegistry` (#79) | `StructureRecord[]`, HP, status pintu (#78), dan isi peti (#46) |
| Suku cadang | `world_parts` | `HeavyItemSystem` (#69) | Posisi dan status tiap barang |
| Helikopter | `world_helicopter` | `RescueEventManager` (#14) | Bagian terpasang |
| Diary dan objektif | `world_objectives` | `ObjectiveManager` (#81) | Bab ditemukan dan objektif |
| Resep terbuka | `world_recipes` | `CraftingManager`, `TechTreeManager` (#77) | Node dan resep |
| NPC | `world_npcs` | `NPCRoster` (#76) | Posisi, state, HP, dan yang sudah mati |
| Bencana | `world_disasters` | `DisasterManager` (#70) | Jadwal terpakai dan cooldown |
| Kargo dan suplai | `world_supply` | #71, #54 | Kargo yang belum diambil |
| Hasil simulasi base | `world_remotebase` | `RemoteBaseSimulation` (#90) | Hasil yang belum diterapkan |
| Lucky Machine | Lewat `RegionStateStore` | `LuckyMachine` (#72) | Kuota dan hadiah |

**Tidak disimpan:** musuh, state AI, panel UI, `Time.timeScale`, status tidur, hologram, isi pool, audio, dan efek. Semuanya dihitung ulang atau dimulai dari awal.

**Sudah di luar issue ini:** isi peti diatur #46, statistik di #57, dan setting di #19.

### Menulis save

**`private void OnSaveRequested(SaveRequestedEvent e)`** mencatat alasan, lalu memanggil `TrySaveNow(reason)`.

**`private bool TrySaveNow(SaveReason reason)`**:

1. Kalau `!autosaveEnabled`, atau `RunLifecycle.State != OnIsland`, kembalikan `false`.
2. Kalau alasan termasuk autosave dan `!IsSafeToAutosave()`, simpan `pendingReason`, lalu mulai `DeferLoop()`, dan kembalikan `false`.
3. Kalau `isWriting`, simpan `pendingReason`, lalu selesai. Permintaan setelah penulisan selesai hanya berjalan sekali.
4. `RegionStreaming.Instance.CaptureLoadedRegions()`.
5. Bangun `RunSavePayload`. Panggil `CaptureState()` pada tiap `registered`, urut `LoadOrder`. Exception dari satu sistem menggagalkan save itu, bukan menulis file setengah jadi.
6. Ubah ke JSON, hitung `payloadCrc32`, lalu bangun `RunSaveEnvelope`.
7. Tulis lewat `WriteAtomic(json)`.
8. Terbitkan `SaveCompletedEvent { succeeded }`.

**Kapan tiap alasan dipakai:**

| Alasan | Pengirim | Lolos pemeriksaan pertempuran |
|---|---|---|
| `ArrivedOnIsland` | `RunLifecycle` | Ya, selalu tulis |
| `WakeUp` | `SleepSystem` (#67) | Diperiksa |
| `Daybreak` | `WorldStateSave`, dari `DaybreakEvent` | Diperiksa |
| `QuitToMenu` | `RunLifecycle` | Ya, selalu tulis |
| `AppQuit` | `Application.wantsToQuit`, dan `OnApplicationPause(true)` di Android | Ya, selalu tulis |
| `Manual` | `DevConsole` (#56) | Ya, selalu tulis |

### Menulis tanpa merusak file lama

**`private bool WriteAtomic(string json)`**:

1. Tulis ke `survival_save.json.tmp` lewat `FileStream`.
2. `stream.Flush(true)`, supaya data benar-benar sampai ke disk.
3. Kalau `survival_save.json` ada, panggil `File.Replace(tmp, main, bak)`. Kalau tidak ada, `File.Move(tmp, main)`.
4. Kalau `IOException`, hapus `tmp` kalau perlu, catat kesalahan, dan kembalikan `false`. File lama tetap utuh.

Kalau game mati sebelum langkah 3, file utama masih versi lama. Kalau mati sesudahnya, file baru sudah lengkap.

**Thread:** langkah 4 sampai 6 di `TrySaveNow` berjalan di thread utama, supaya datanya konsisten. `WriteAtomic` untuk autosave berjalan di `Task.Run`, dengan `isWriting = true`. Hasilnya dibaca di `Update`, dan `SaveCompletedEvent` terkirim dari thread utama. Untuk `QuitToMenu` dan `AppQuit`, `WriteAtomic` berjalan langsung, karena proses mungkin segera berakhir.

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

**Keluar saat pertempuran:** `QuitToMenu` dan `AppQuit` tetap menulis, karena pemain tidak boleh kehilangan run. Supaya bukan jalan pintas keluar dari bahaya, payload mencatat `wasInCombat` dan `engagedEnemyCount`. Saat `FinishRestore`, kalau `wasInCombat` dan saat itu malam, panggil `SpawnManager.Instance.RequestAmbush(posisiPemain, Mathf.Min(engagedEnemyCount, combatRestoreMaxEnemies), combatRestoreRadius)`.

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
| 6 | 60 | `player_equipment` | Merujuk `instanceId` (#60) |
| 7 | 70 | `player_vitals`, `player_infection`, `player_disease`, `player_effects` | Mengisi status |
| 8 | 80 | `player_transform` | Menentukan region awal |
| 9 | 90 | `world_structures` | Memakai `GetStructureRoot`, dan item untuk peti |
| 10 | 100 | `world_parts`, `world_helicopter` | Data pulau |
| 11 | 110 | `world_objectives`, `world_recipes` | Data progres |
| 12 | 120 | `world_npcs` | Butuh posisi pemain dan bangunan |
| 13 | 130 | `world_disasters`, `world_supply`, `world_remotebase` | Terakhir, karena bisa merujuk data di atas |

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

**`public void MarkRunEnded(RunResult result, DeathReason reason)`** menulis `run_end.marker` lewat cara `WriteAtomic`.

**`public void DeleteRunSave()`** menghapus `survival_save.json`, `.tmp`, `.bak`, dan `.corrupt.json`. Fungsi ini juga mengosongkan cache `HasContinuableSave`. File `stats.json` dan `PlayerPrefs` tidak disentuh. `PlayerPrefs.DeleteAll()` dilarang (#82).

**`public void ClearRunEndMarker()`** menghapus penanda.

**`public bool TryReadRunEndMarker(out RunEndMarker m)`** dipakai `RecoverInterruptedEnd()` (#82).

**`public void ResetForNewRun()`**: `pendingReason = null`, `autosaveEnabled = true`, `loadedPayload = null`, dan `registered` dikosongkan.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `SaveCompletedEvent` (`succeeded`).

**Event yang didengar:** `SaveRequestedEvent`, `DaybreakEvent`, `PlayerDamagedEvent`, `VitalsChangedEvent`, `VitalWarningChangedEvent`, `PlayerPinnedEvent`, `DisasterStartedEvent`, `DisasterEndedEvent`, dan `RunEndedEvent` (mematikan autosave).

| Sistem | Hubungan |
|---|---|
| `RunLifecycle` (#82) | Memanggil `HasContinuableSave`, `ReadRunFile`, `RestoreBaseScene`, `FinishRestore`, `MarkRunEnded`, `DeleteRunSave`, `ClearRunEndMarker`, dan `TryReadRunEndMarker` |
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
- **Menang atau kalah saat menulis:** `MarkRunEnded` menunggu `isWriting` selesai, paling lama `saveWaitTimeoutSeconds` (#82), lalu lanjut. `DeleteRunSave` tetap dijalankan sesudahnya.
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

**Usulan yang menunggu developer (dicatat di #85):**

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
