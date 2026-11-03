# Blueprint: RunLifecycle.cs
**Lokasi:** `docs/blueprint/Core_Logic/RunLifecycle.md`
**Deskripsi Utama:** Pengatur alur satu run: bootstrap, menu utama, mulai atau lanjut run, perpindahan fase, akhir run, dan reset. Hanya sistem ini yang menghapus data run dan memerintahkan pemuatan scene antar-fase.
**Issue:** #82
**Rujukan GDD:** Bagian 1 dan bagian 3 (Permadeath, Syarat Menang, Game Over). Menu utama dan urutan bootstrap adalah usulan arsitektur, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

### Pola

`RunLifecycle` adalah singleton persisten dengan satu state machine. Fungsi publiknya menolak panggilan yang tidak sesuai state, sehingga klik ganda atau dua penyebab kematian tidak menjalankan alur dua kali.

```csharp
public enum RunState { Booting, InMenu, StartingRun, InShip, OnIsland, Ending, AwaitingChoice, Resetting }

public interface IBootstrapService
{
    void Initialize();      // Dipanggil sekali, sesuai urutan bootstrap
    void ResetForNewRun();  // Dipanggil saat run lama dibersihkan, sesuai urutan yang sama
}
```

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Instance` | `RunLifecycle` | `null` | Singleton |
| `State` | `RunState` | `Booting` | State saat ini, hanya bisa dibaca dari luar |
| `orderedServices` | `MonoBehaviour[]` | Tabel urutan di bawah | Daftar `IBootstrapService`, diisi di prefab |
| `managersRoot` | `GameObject` | Anak `PersistentRoot` | Induk semua manager persisten selain `RunLifecycle` |
| `cinematicPanel`, `gameOverPanel`, `winPanel`, `loadingPanel` | `UIPanel` | Dari `UIManager` | Panel alur run (#83) |

### Susunan scene `Bootstrap`

`Bootstrap` hanya berisi kamera hitam dan `BootstrapEntry`. Prefab `PersistentRoot` disusun seperti ini:

```text
PersistentRoot (DontDestroyOnLoad, memuat RunLifecycle)
└── Managers (managersRoot)
    ├── GameManager, InputManager, SettingsManager, AudioManager, ...
    └── Canvas persisten dan EventSystem
```

`BootstrapEntry.Awake()` membuat `PersistentRoot` hanya kalau `RunLifecycle.Instance == null`. Dengan begitu, `Bootstrap` aman dimuat ulang sebagai scene transit kosong saat reset.

### Urutan bootstrap

Manager hanya mengisi `Instance` di `Awake`. Pekerjaan yang bergantung pada manager lain dilakukan di `Initialize()`, supaya urutannya pasti dan tidak bergantung pada urutan `Awake` Unity.

| Urutan | Manager | Alasan posisinya |
|---|---|---|
| 1 | `GameManager` | Menyediakan `Balance`, yang dibaca semua manager berikutnya |
| 2 | `InputManager` (#18) | Membuat Action Map dan memuat tombol tersimpan |
| 3 | `SettingsManager` (#19) | Membaca setting, lalu menerapkannya ke input dan audio |
| 4 | `AudioManager` | Memakai volume dari setting |
| 5 | `LocalizationManager` (#104) | Menyiapkan teks sebelum UI tampil |
| 6 | `UIFocusStack` (#83) | Pengatur waktu, kursor, dan fokus |
| 7 | `UIManager` | Menyiapkan panel persisten |
| 8 | `NotificationManager` (#22) | Memakai canvas persisten |
| 9 | `ObjectPooler` | Membuat isi pool |
| 10 | `AsyncSceneLoader` | Layanan scene tunggal |
| 11 | `RunSeedManager` (#89) | Belum membuat seed, hanya menyiapkan data |
| 12 | `WorldStateSave` (#80) | Menentukan lokasi save dan memeriksa penanda akhir run |
| 13 | `LocalStatsManager` (#57) | Membaca `stats.json` |
| 14 | `DevConsole` (#56) | Hanya di build pengembangan |

`RunLifecycle` berjalan terakhir, setelah semua layanan siap. Selama #80 belum selesai, nama `WorldStateSave` di blueprint ini merujuk ke `SaveSystem`.

### Masa hidup manager

| Masa hidup | Sistem | Alasan |
|---|---|---|
| **Persisten (`DontDestroyOnLoad`)** | Semua layanan pada tabel urutan bootstrap, `RunLifecycle`, `NarrativeSequencer` (#99), `VFXManager` (#15), dan `EventSystem` | Dipakai lintas scene, atau menyimpan data yang harus bertahan saat scene berganti |
| **Scene `MainMenu`** | `MainMenuController` | Hanya dipakai di menu |
| **Scene `ShipPhase`** | `ShipTimerManager`, `ShipHazardManager`, `LifeboatExtraction`, `LootSpawnerSystem`, `InventoryManager`, `HUDManager`, dan pemain Fase 1 | Fase 1 dibuang utuh setelah selesai |
| **Scene `IslandPhase`** | `RegionStreaming`, `DayNightDirector`, `ZoneManager`, `RegionalWeather`, `EnvironmentDirector`, `DisasterManager`, `SpawnManager`, `AIDirector`, `HordeManager`, `BuildManager`, `CraftingManager`, `InventoryManager`, `EquipmentManager`, `HeavyItemSystem`, `RescueEventManager`, `ObjectiveManager`, `DiaryManager`, `NPCRoster`, `HUDManager`, dan pemain Fase 2 | Terikat pada dunia pulau, dan hilang bersama scene saat reset |
| **Region scene** | Tidak boleh memuat manager. Hanya objek dunia (#88) | Region bisa dilepas kapan saja |

`InventoryManager` memakai satu kelas untuk dua fase, dengan instance di tiap scene. Barang berpindah fase lewat `GameManager.savedShipLoot`, sesuai `GLOSSARY.md`.

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.run.endSequenceTimeoutSeconds` | 15 | Batas tunggu sinematik sebelum alur akhir tetap berlanjut |
| `GameBalanceConfig.run.timeExpiredFadeSeconds` | 3 | Fade hitam saat hari ke-60 berakhir |
| `GameBalanceConfig.run.gameOverInputDelaySeconds` | 1 | Tombol layar akhir aktif setelah jeda ini, supaya tidak tertekan tanpa sengaja |
| `GameBalanceConfig.run.saveWaitTimeoutSeconds` | 5 | Batas tunggu `SaveCompletedEvent` saat keluar ke menu |
| `GameBalanceConfig.run.sceneLoadMinShowSeconds` | 0,5 | Waktu tampil minimum layar loading |

## 2. Detail Logika & Fungsi

### Boot

**`IEnumerator Boot()`**, dimulai dari `Start` milik `RunLifecycle`:

1. Panggil `Initialize()` pada tiap `orderedServices` sesuai urutan. Exception ditangkap per layanan dan dicatat, kecuali `GameManager`, yang menghentikan boot dengan pesan kesalahan.
2. Panggil `RecoverInterruptedEnd()` (lihat "Game ditutup paksa saat akhir run").
3. Panggil `GoToMainMenu()`.

**`IEnumerator GoToMainMenu()`** mendorong panel `Loading`, menjalankan `AsyncSceneLoader.LoadSingle("MainMenu")`, menutup panel `Loading`, lalu mengatur `State = InMenu`. `MainMenuController` mendorong panel `MainMenu` saat scene aktif.

**`public void ReturnToMainMenu()`** menjalankan `ResetRunState()`, lalu `GoToMainMenu()`. Panel `Loading` dari langkah reset dipakai ulang, karena `Push` mengabaikan panel yang sudah ada di stack.

**Tes dari scene lain di Editor:** `EditorQuickStart` memakai `RuntimeInitializeOnLoadMethod(BeforeSceneLoad)`. Kalau scene aktif bukan `Bootstrap`, ia membuat `PersistentRoot`, menjalankan `Initialize()`, lalu memulai run tes tanpa menu. Kode ini tidak ikut di build rilis.

### Menu utama

`MainMenuController` mendorong panel `MainMenu` ke `UIFocusStack`, lalu menampilkan:

| Tombol | Syarat tampil | Aksi |
|---|---|---|
| **Lanjutkan** | `WorldStateSave.HasContinuableSave()` bernilai `true` | `RunLifecycle.ContinueRun()` |
| **Run Baru** | Selalu | Kalau ada save, tampilkan `ConfirmDialog` bahwa run lama akan hilang. Lalu `RunLifecycle.StartNewRun()` |
| **Pengaturan** | Selalu | Dorong panel `Settings` milik #19 |
| **Keluar** | Selalu | `Application.Quit()` |

`HasContinuableSave()` bernilai `false` kalau file tidak ada, penanda akhir run ada, atau save rusak dan tidak bisa dipakai (aturan rinci di #80).

### Mulai run baru

**`public void StartNewRun()`**, hanya berlaku saat state `InMenu` atau `AwaitingChoice`. Coroutine-nya:

1. `State = StartingRun`, lalu jalankan `ResetRunState()`.
2. `WorldStateSave.DeleteRunSave()`.
3. `RunSeedManager.BeginNewRun()` (#89).
4. Terbitkan `RunStartedEvent { seed, isNewRun = true }`. Event ini sticky, jadi objek di scene baru bisa membacanya saat aktif.
5. `GameManager.SetPhase(GamePhase.SinkingShip)`, yang menerbitkan `PhaseChangedEvent`.
6. `yield return AsyncSceneLoader.LoadSingle("ShipPhase")`.
7. Tutup panel `Loading`, lalu `State = InShip`.

### Lanjutkan run

**`public void ContinueRun()`**, hanya berlaku saat state `InMenu`:

1. `State = StartingRun`, lalu jalankan `ResetRunState()`.
2. `WorldStateSave.ReadRunFile()`. Kalau gagal, kembali ke `MainMenu` dengan notifikasi dari #80.
3. `RunSeedManager.RestoreFromSave(...)` dan `GameManager.RestoreRunData(...)`.
4. Terbitkan `RunStartedEvent { seed, isNewRun = false }`, lalu `GameManager.SetPhase(GamePhase.IslandSurvival)`.
5. `yield return AsyncSceneLoader.LoadSingle("IslandPhase")`.
6. `WorldStateSave.RestoreBaseScene()`. Ini mengisi data di scene dasar, termasuk `RegionStateStore` dan posisi pemain.
7. `yield return RegionStreaming.Instance.LoadInitialRegions(posisiPemain)` (#88). Objek region mengambil statusnya dari `RegionStateStore` saat region masuk.
8. `WorldStateSave.FinishRestore()`, yang mengirim ulang event sticky lewat `ISaveable`.
9. Tutup panel `Loading`, lalu `State = OnIsland`.

### Pindah dari kapal ke pulau

**`public void EnterIslandPhase()`** dipanggil pemilik akhir Fase 1 (#74, `ShipTimerManager`). Hanya berlaku saat state `InShip`:

1. Dorong panel `Loading`, atau biarkan #99 memutar adegan sekoci dengan `holdActivation = true`.
2. `GameManager.SetPhase(GamePhase.IslandSurvival)`.
3. `yield return AsyncSceneLoader.LoadSingle("IslandPhase", holdActivation)`.
4. `yield return RegionStreaming.Instance.LoadInitialRegions(titikPendaratan)`.
5. `State = OnIsland`, lalu terbitkan `SaveRequestedEvent` dengan alasan `ArrivedOnIsland`. Ini save pertama dalam satu run.
6. Tutup panel `Loading`.

### Alur akhir run tunggal

Tiga penyebab akhir run memakai satu jalur:

```text
SurvivalStats  --PlayerDiedEvent-->  GameManager.TriggerGameOver(reason)  ─┐
GameManager.CheckDay60Milestone  -->  TriggerGameOver(TimeExpired)         ├─> RunEndedEvent ─> RunLifecycle
RescueEventManager  ------------->  GameManager.TriggerWinSequence()      ─┘
```

- `GameManager` memegang `IsRunEnding`. Panggilan pertama menang, dan panggilan berikutnya diabaikan. Dengan begitu, pemain yang mati saat helikopter sudah terbang tidak mengubah hasil.
- `DeathReason` dari kematian di kapal (#74 dan #75) melewati jalur yang sama.
- `RunEndedEvent.reason` bernilai `DeathReason.None` saat menang.

**`IEnumerator RunEndSequence(RunResult result, DeathReason reason)`**:

| Langkah | Tindakan | `Died` | `TimeExpired` | `Won` |
|---|---|---|---|---|
| 1 | `State = Ending`, matikan autosave | Sama | Sama | Sama |
| 2 | `WorldStateSave.MarkRunEnded(result, reason)` | Sama | Sama | Sama |
| 3 | `UIFocusStack.CloseAllUnlocked(RunEnded)`, lalu dorong panel `Cinematic` | Sama | Sama | Sama |
| 4 | Putar penutup, dengan batas `endSequenceTimeoutSeconds` | `DeathCinematic.Play(reason)` (#53) | Fade hitam `timeExpiredFadeSeconds`, lalu teks gagal kabur | `NarrativeSequencer.PlayEnding(...)` (#99) |
| 5 | `WorldStateSave.DeleteRunSave()` | Sama | Sama | Sama |
| 6 | `LocalStatsManager.RecordRunEnd(result, reason, currentDay, seed)` (#57) | Sama | Sama | Sama |
| 7 | `WorldStateSave.ClearRunEndMarker()` | Sama | Sama | Sama |
| 8 | Tutup `Cinematic`, dorong layar akhir | `GameOver` | `GameOver` | `WinScreen` |
| 9 | `State = AwaitingChoice`, tunggu pilihan pemain | Ulangi atau Menu | Ulangi atau Menu | Menu |

Langkah 4 memakai waktu tak terskala untuk batas tunggunya. Kalau `DeathCinematic` atau `NarrativeSequencer` belum tersedia, langkah ini dilewati. Kegagalan sinematik tidak boleh menghentikan permadeath.

**Layar akhir:** menampilkan penyebab, hari tercapai, dan seed dari `RunSeedManager.CurrentSeedText` (#89). Tombol aktif setelah `gameOverInputDelaySeconds` waktu tak terskala.

| Tombol | Aksi |
|---|---|
| **Ulangi dari Kapal** | `StartNewRun()`. Pemain kembali ke `ShipPhase`, sesuai GDD bagian 3 |
| **Menu Utama** | `ReturnToMainMenu()` |

### Keluar ke menu saat bermain

**`public void SaveAndQuitToMenu()`**, dipanggil dari panel `Pause`:

1. **Di Fase 2:** terbitkan `SaveRequestedEvent` dengan alasan `QuitToMenu`, lalu tunggu `SaveCompletedEvent` selama `saveWaitTimeoutSeconds`. Kalau gagal, tampilkan `ConfirmDialog` bahwa progres terakhir bisa hilang.
2. **Di Fase 1:** tampilkan `ConfirmDialog` bahwa run ini hilang, karena Fase 1 tidak punya save.
3. Jalankan `ReturnToMainMenu()`.

Menutup jendela memakai alur yang sama lewat `Application.wantsToQuit`. Saat state `Ending` atau `AwaitingChoice`, game langsung ditutup tanpa save.

### Reset satu pintu

**`IEnumerator ResetRunState()`** adalah satu-satunya tempat pembersihan run:

1. `State = Resetting`.
2. `UIFocusStack.ClearForNewRun()`, lalu dorong panel `Loading`. `Time.timeScale` dan kursor diatur ulang oleh stack, bukan oleh sistem lain.
3. `AudioManager.StopWorldAudio()` dan `AudioListener.pause` mengikuti stack.
4. `ObjectPooler.ReturnAllActive()`. Tiap objek menjalankan `OnReturned()` selagi scene lama masih ada.
5. `yield return AsyncSceneLoader.LoadSingle("Bootstrap")`. Scene lama dan semua region additive terlepas, sehingga pendengar di scene otomatis lepas lewat `OnDisable`.
6. `RunLifecycle` melepas langganannya, lalu `managersRoot.SetActive(false)`. Pendengar persisten lepas lewat `OnDisable`.
7. Di build pengembangan, catat tiap channel yang masih punya pendengar lewat `GameEventRegistry.ListenerCount`, sebagai peringatan kebocoran. Pada titik ini jumlahnya seharusnya 0.
8. `GameEventRegistry.ClearAll()`. Nilai sticky lama dan pendengar yang bocor hilang.
9. `managersRoot.SetActive(true)`. Pendengar persisten berlangganan kembali lewat `OnEnable`, `UIFocusStack` menerbitkan ulang statusnya, dan `RunLifecycle` memasang ulang langganannya.
10. Panggil `ResetForNewRun()` pada tiap `orderedServices` sesuai urutan bootstrap. `GameManager` membersihkan `savedShipLoot`, mengatur `currentDay = 1`, `hasEscaped = false`, `IsRunEnding = false`, dan membuat salinan `GameBalanceConfig` yang baru.
11. `yield return Resources.UnloadUnusedAssets()`, lalu `GC.Collect()`. Keduanya aman karena layar sedang loading.

**Yang dijamin setelah reset:**

| Hal | Hasil | Dijaga oleh |
|---|---|---|
| `Time.timeScale` | 0 selama `Loading`, lalu 1 saat panel tertutup | `UIFocusStack` |
| Kursor | Terkunci saat stack kosong, bebas di menu | `UIFocusStack` |
| Event | Tanpa nilai sticky lama, dan jumlah pendengar kembali ke nilai awal | `GameEventRegistry` |
| Pool | Semua objek tidak aktif dan berstatus awal | `ObjectPooler`, `IPoolable` |
| Status run | Kembali ke nilai awal | `ResetForNewRun()` |

### Data yang dihapus dan dipertahankan

`PlayerPrefs.DeleteAll()` dilarang, karena juga menghapus setting dan tombol. Penghapusan data run hanya lewat `WorldStateSave.DeleteRunSave()`.

| Data | Lokasi | Saat permadeath |
|---|---|---|
| Save run dan seed | `survival_save.json` (#80) | **Dihapus** |
| Isi `GameManager`, status di memori, dan isi pool | Memori | **Direset** |
| Setting dan tombol | `PlayerPrefs` (#18, #19) | Tetap |
| Statistik akumulatif | `stats.json` (#57) | Tetap |
| Adegan yang sudah ditonton | Di luar data run (#99) | Tetap |

### Layanan scene tunggal

`AsyncSceneLoader` menjadi satu-satunya pemanggil `SceneManager.LoadSceneAsync` dan `UnloadSceneAsync`. `UIManager.ShowLoadingScreen` dan pemanggilan langsung di `ShipTimerManager` dihapus.

| Fungsi | Dipakai oleh |
|---|---|
| `IEnumerator LoadSingle(string sceneName, bool holdActivation = false)` | `RunLifecycle` |
| `AsyncOperation LoadAdditive(string sceneName)` | `RegionStreaming` (#88) |
| `AsyncOperation UnloadAdditive(string sceneName)` | `RegionStreaming` (#88) |
| `void AllowActivation()` | `NarrativeSequencer` (#99), setelah adegan selesai |
| `float Progress`, `bool IsBusy` | Panel `Loading` |

**Logika `LoadSingle`:**

1. Set `operation = LoadSceneAsync(sceneName, Single)` dan `operation.allowSceneActivation = false`.
2. Selama `operation.progress < 0.9f`, isi `Progress = Mathf.Clamp01(operation.progress / 0.9f)`. Skala ini diperlukan karena `progress` berhenti di 0,9 selama scene belum diaktifkan.
3. Tunggu minimal `sceneLoadMinShowSeconds` waktu tak terskala, dan tunggu `AllowActivation()` kalau `holdActivation` aktif.
4. Set `allowSceneActivation = true`, `Progress = 1`, lalu tunggu `operation.isDone`.

Panel `Loading` mengubah bar dengan `Mathf.MoveTowards` dan `Time.unscaledDeltaTime`, jadi bar tidak mundur dan tetap bergerak saat waktu berhenti. Permintaan saat `IsBusy` ditolak dan dicatat.

### Aturan melepas langganan event untuk semua manager

- Manager persisten dan per scene berlangganan di `OnEnable` dan melepas di `OnDisable`, mengikuti `GameEventBus`. Berlangganan di `Start` atau `Awake` dilarang.
- Event statis di luar `EventChannel<T>` dilarang, termasuk `DayNightDirector.OnDaybreak` lama.
- `AudioManager` memindahkan langganannya dari `Start` ke `OnEnable` dan `OnDisable`, dengan `EventChannel<DaybreakEvent>` dan `EventChannel<NightfallEvent>`.
- Lambda yang menangkap variabel dilarang sebagai pendengar.
- Langkah 7 pada reset menjadi pemeriksaan otomatisnya.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `RunStartedEvent`, `SaveRequestedEvent`.

**Event yang didengar:** `RunEndedEvent`, `SaveCompletedEvent`.

**Sistem yang dipanggil langsung:**

| Sistem | Fungsi |
|---|---|
| `GameManager` | `SetPhase`, `RestoreRunData`, dan `ResetForNewRun` |
| `UIFocusStack` (#83) | `Push`, `Close`, `CloseAllUnlocked`, dan `ClearForNewRun` |
| `AsyncSceneLoader` | Pemuatan scene |
| `RunSeedManager` (#89) | `BeginNewRun`, `RestoreFromSave`, dan `CurrentSeedText` |
| `RegionStreaming` (#88) | `LoadInitialRegions` |
| `WorldStateSave` (#80) | `HasContinuableSave`, `ReadRunFile`, `RestoreBaseScene`, `FinishRestore`, `MarkRunEnded`, `DeleteRunSave`, dan `ClearRunEndMarker` |
| `LocalStatsManager` (#57) | `RecordRunEnd` |
| `DeathCinematic` (#53), `NarrativeSequencer` (#99) | Penutup run |
| `ObjectPooler`, `GameEventRegistry`, `AudioManager` | Reset |

**Batas pekerjaan:** isi menu Pengaturan ada di #19, isi sinematik kematian di #53, dan event helikopter di #14.

## 4. Kasus Khusus

- **Objek pool:** `ReturnAllActive()` berjalan sebelum scene diganti. Pool hidup di `PersistentRoot`, jadi prefab tidak dibuat ulang setiap permadeath. Tiap objek kembali lewat `OnReturned()` dan siap dipakai ulang.
- **Game dijeda:** semua penantian di blueprint ini memakai `WaitForSecondsRealtime` dan `Time.unscaledDeltaTime`. Alur tetap berjalan walaupun `Time.timeScale` bernilai 0.
- **Mati saat menu terbuka:** panel non-locked ditutup lewat `CloseAllUnlocked`, dengan alasan `RunEnded`. Lihat #83.
- **Mati dan menang pada frame yang sama:** panggilan pertama ke `GameManager` menang. Sisanya diabaikan oleh `IsRunEnding`.
- **Hari ke-60 berakhir saat helikopter terbang:** `TriggerWinSequence` sudah mengatur `hasEscaped = true` dan `IsRunEnding = true`, jadi `CheckDay60Milestone` tidak berpengaruh.
- **Save dan load:** posisi di menu tidak disimpan. Save pertama dibuat setelah tiba di pulau, jadi "Lanjutkan" tidak pernah membuka Fase 1.
- **Permadeath:** save dihapus setelah sinematik, dan penanda akhir run dibuat sebelum sinematik.
- **Game ditutup paksa saat akhir run:** penanda `run_end.marker` berisi `result`, `reason`, `currentDay`, dan `seed`. Saat boot berikutnya, `RecoverInterruptedEnd()` menghapus save, mencatat statistik, lalu menghapus penandanya. Pemain tidak bisa lolos dari permadeath dengan mematikan game saat sinematik.
- **Statistik tercatat dua kali:** kalau game mati di antara langkah 6 dan 7, `LocalStatsManager` mengabaikan seed yang sama dengan catatan terakhirnya.
- **Save gagal saat keluar:** pemain mendapat pilihan lewat `ConfirmDialog`, dan tidak keluar diam-diam.
- **Scene gagal dimuat:** kalau `LoadSceneAsync` mengembalikan `null`, kembali ke `MainMenu` dan tampilkan notifikasi. Kalau gagalnya pada `MainMenu`, catat kesalahan dan tetap di layar loading.
- **`Time.timeScale` saat scene baru:** scene baru tidak boleh mengatur nilai ini. Satu-satunya penulisnya adalah `UIFocusStack`.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:**

- Kalah mengembalikan pemain ke Fase 1 (GDD bagian 3).
- Penghapusan save dan pemuatan scene hanya dilakukan `RunLifecycle` (`SurvivalStats.md`).
- `GameEventRegistry.ClearAll()` dipanggil `RunLifecycle` (`GameEventBus.md`).

**Keputusan teknis di blueprint ini:**

- `Bootstrap` dipakai ulang sebagai scene transit, supaya ada jeda bersih antara scene lama dan run baru.
- Bootstrap memakai `IBootstrapService.Initialize()` dengan urutan tertulis, bukan urutan `Awake`.
- `ClearAll()` diapit `managersRoot.SetActive(false)` dan `SetActive(true)`, karena `ClearAll()` juga menghapus pendengar persisten.

**Usulan yang menunggu developer (dicatat di #85):**

1. **Pembagian manager persisten dan per scene:** mengikuti tabel "Masa hidup manager".
2. **Layar kalah:** dua tombol, Ulangi dari Kapal dan Menu Utama. GDD hanya meminta kembali ke Fase 1, jadi tombol Menu Utama adalah tambahan.
3. **Setelah menang:** save dihapus, tampil `WinScreen`, lalu kembali ke `MainMenu`. Run yang sudah menang tidak bisa dilanjutkan.
4. **Save di Fase 1:** tidak ada. Menutup game saat di kapal membuang run tersebut, karena Fase 1 hanya 60 detik.
5. **Keluar saat bermain:** panel `Pause` memiliki Simpan dan Keluar ke Menu. Ini mengikuti Save & Quit di `SaveSystem.md`.
6. **Penutup gagal kabur:** fade hitam sederhana dan teks, sampai #99 menyediakan adegan.
7. **`DeathReason.None`:** nilai baru untuk `RunEndedEvent` saat menang, supaya nilai bawaan enum tidak terbaca sebagai `Starvation`.

## 6. Skenario Tes

1. Buka `Bootstrap` lalu tekan Play. **Hasil:** semua layanan berjalan sesuai urutan di log, lalu `MainMenu` tampil dengan kursor bebas.
2. Buka menu tanpa file save. **Hasil:** Lanjutkan tidak tampil.
3. Klik Run Baru dua kali dengan cepat. **Hasil:** hanya satu run dimulai, dan `ShipPhase` dimuat sekali.
4. Selesaikan Fase 1. **Hasil:** bar loading bergerak sampai penuh tanpa berhenti di 90 persen, lalu file save pertama muncul setelah tiba di pulau.
5. Keluar ke menu dari Pause, lalu klik Lanjutkan. **Hasil:** hari, status, dan posisi kembali, dan barang tidak berlipat.
6. Turunkan HP ke 0 lewat `DevConsole`. **Hasil:** urutan log adalah sinematik, hapus save, catat statistik, lalu layar `GameOver`. Seed tampil di layar.
7. Tekan Esc berkali-kali di layar `GameOver`. **Hasil:** tidak terjadi apa-apa, dan `Time.timeScale` tetap 0.
8. Klik Ulangi dari Kapal. **Hasil:** `ShipPhase` dimuat, `Time.timeScale` bernilai 1, kursor terkunci, dan `currentDay` bernilai 1.
9. Ubah volume, mati, lalu mulai run baru. **Hasil:** volume dan tombol tidak berubah, dan statistik tetap ada.
10. Jalankan `setday 60` lalu tunggu jam melewati 24:00. **Hasil:** alur sama seperti mati, dengan `RunResult.TimeExpired`.
11. Panggil `TriggerWinSequence` lalu turunkan HP ke 0 saat adegan berjalan. **Hasil:** hasil tetap `Won`, dan `RunEndedEvent` terkirim sekali.
12. Matikan proses game saat sinematik kematian, lalu buka lagi. **Hasil:** Lanjutkan tidak tampil, save sudah hilang, dan statistik mencatat run tersebut.
13. Mati lalu mulai run baru sebanyak lima kali sambil memantau log. **Hasil:** tidak ada peringatan kebocoran pendengar, event tidak ganda, dan jumlah objek pool tetap.
14. Tekan Play langsung dari `IslandPhase` di Editor. **Hasil:** `EditorQuickStart` menyiapkan manager dan game berjalan.
