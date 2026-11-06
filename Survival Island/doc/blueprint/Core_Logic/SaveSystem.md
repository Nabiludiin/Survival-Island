# Blueprint: SaveSystem.cs
**Lokasi:** `Survival Island/doc/blueprint/Core_Logic/SaveSystem.md`
**Deskripsi Utama:** Catatan peralihan. Isi file ini digantikan `Core_Logic/WorldStateSave.md`. `GameData`, pemanggilan langsung ke manager, dan `File.WriteAllText` tidak dipakai lagi. File ini merangkum kontrak save untuk Langkah 1 di `CLAUDE.md`, lalu menunjuk ke bagian rinci di `WorldStateSave.md`.
**Issue:** #82, #80
**Rujukan GDD:** Bagian 1 dan 3 (Permadeath: kalah mengulang dari Fase 1). Rincian format dan autosave adalah usulan teknis di `WorldStateSave.md`, yang sudah disetujui sebagai nilai awal.

## 1. Arsitektur & Variabel Utama

Tidak ada kelas `SaveSystem` baru. Semua nama di file ini diteruskan ke `WorldStateSave` (#80). Selama `WorldStateSave` belum ditulis sebagai script, nama fungsi di bawah adalah kontrak yang harus disediakan.

**Aturan baca:** kalau file ini berbeda dengan `WorldStateSave.md` atau `RunLifecycle.md`, ikuti kedua file itu. File ini tidak menambah aturan baru.

### Isi lama dan penggantinya

| Isi lama | Pengganti | Bagian di `WorldStateSave.md` |
|---|---|---|
| Kelas `GameData` berisi semua data | `RunSaveEnvelope` dan `RunSavePayload`, dengan satu `SaveEntry` per sistem | "Bentuk file" |
| `SaveGame()` yang membaca tiap manager | `SaveRequestedEvent`, lalu `TrySaveNow`, yang memanggil `CaptureState()` tiap `ISaveable` | "Menulis save" |
| `LoadGame()` yang mengisi tiap manager | `ReadRunFile`, `RestoreBaseScene`, dan `FinishRestore`, dipanggil `RunLifecycle.ContinueRun` | "Membaca dan memulihkan" |
| `DeleteSaveFile()` | `DeleteRunSave()` | "Permadeath" |
| `saveFilePath` sendiri | `Application.persistentDataPath` + `survival_save.json` | "Lokasi file" |
| `File.WriteAllText` | `WriteAtomic`: file `.tmp`, `Flush(true)`, lalu `File.Replace` | "Menulis tanpa merusak file lama" |
| `UIManager.ShowNotification` | Pendengar `SaveCompletedEvent`, misalnya `NotificationManager` (#22) | Bagian 3 |
| `PlayerPrefs.DeleteAll()` saat mati | Dilarang. Hanya file run yang dihapus | "Permadeath", dan `RunLifecycle.md` |

### Kontrak `ISaveable`

Setiap pemilik data menyimpan datanya sendiri. Bentuk lengkap ada di `WorldStateSave.md` bagian 1.

| Anggota | Aturan singkat |
|---|---|
| `string SaveId` | Stabil dan unik. Manager memakai konstanta di tabel "Isi data", contoh `player_vitals` |
| `int LoadOrder` | Urutan pulih untuk manager di scene dasar. Objek region memakai 0 |
| `string CaptureState()` | JSON milik sistem itu. `Dictionary` diubah ke array |
| `void RestoreState(string json)` | Hanya mengisi data. Tidak boleh mengirim event |
| `void OnRestoreFinished()` | Mengirim ulang event sticky dan menyegarkan tampilan |

Manager di `IslandPhase` mendaftar lewat `WorldStateSave.Instance.Register(this)` di `OnEnable`, dan `Unregister(this)` di `OnDisable`. Objek region, struktur pemain, dan `WorldItem` tidak mendaftar sendiri (`WorldStateSave.md` bagian "Pendaftaran").

### Angka penyetelan

Semua angka sudah berada di `GameBalanceConfig.save` (`combatEnemyRadius` 30, `combatQuietSeconds` 10, `autosaveRetrySeconds` 10, `autosaveMaxDeferSeconds` 120, `autosaveMinHpRatio` 0,1, `combatRestoreMaxEnemies` 10, `combatRestoreRadius` 20) dan `GameBalanceConfig.run.saveWaitTimeoutSeconds` (5). Tidak ada angka di file ini yang perlu dipindah.

## 2. Detail Logika & Fungsi

### Fungsi publik

| Fungsi | Kegunaan | Dipanggil oleh |
|---|---|---|
| `void Register(ISaveable s)` / `Unregister(ISaveable s)` | Daftar manager scene dasar | Manager di `IslandPhase` |
| `bool HasContinuableSave()` | Tombol Lanjutkan boleh tampil | `MainMenuController` |
| `SaveProblem LastProblem` | Alasan save tidak bisa dipakai | `MainMenuController` |
| `bool ReadRunFile()` | Membaca file save ke memori | `RunLifecycle.ContinueRun` |
| `void RestoreBaseScene()` | Memulihkan data scene dasar, urut `LoadOrder` | `RunLifecycle.ContinueRun` |
| `void FinishRestore()` | Memanggil `OnRestoreFinished`, lalu aturan keluar saat pertempuran | `RunLifecycle.ContinueRun` |
| `void SetAutosaveEnabled(bool)` | Mematikan autosave saat `Ending` | `RunLifecycle` |
| `void MarkRunEnded(RunResult, DeathReason)` | Menulis `run_end.marker` berisi `result`, `reason`, `currentDay`, dan `seed` | `RunLifecycle.RunEndSequence` |
| `void ClearRunEndMarker()` | Menghapus penanda | `RunLifecycle` |
| `bool TryReadRunEndMarker(out RunEndMarker)` | Membaca penanda saat boot | `RunLifecycle.RecoverInterruptedEnd` |
| `void DeleteRunSave()` | Menggantikan `DeleteSaveFile()` | `RunLifecycle` |
| `void ResetForNewRun()` | Mengosongkan `pendingReason`, `loadedPayload`, dan `registered`, lalu menyalakan autosave | `RunLifecycle.ResetRunState`, langkah 10 |

Pesan berhasil atau gagal tidak lagi memanggil `UIManager`. Sistem yang perlu memberi tahu pemain mendengar `SaveCompletedEvent`.

### Kapan save terjadi

Tidak ada tombol simpan manual. Save hanya terjadi lewat `SaveRequestedEvent { reason }`:

| `SaveReason` | Pengirim | Ringkasan |
|---|---|---|
| `ArrivedOnIsland` | `RunLifecycle` | Save pertama, setelah tiba di pulau |
| `Daybreak` | `WorldStateSave`, dari `DaybreakEvent` | Pagi pukul 06:00 |
| `WakeUp` | `SleepSystem` (#67) | Bangun tidur normal |
| `QuitToMenu`, `AppQuit` | `RunLifecycle`, `Application.wantsToQuit` | Selalu ditulis |
| `BridgeComplete`, `DisasterEnded`, `PartInstalled` | #68, #70, #14 | Titik progres penting |
| `Manual` | `DevConsole` (#56) | Hanya build pengembangan |

Alasan autosave ditunda saat pertempuran. Aturannya ada di `WorldStateSave.md` bagian "Save saat pertempuran".

### Simpan dan Keluar (Save & Quit)

`RunLifecycle.md` dan keputusan #82 menyebut "Save & Quit di `SaveSystem.md`". Bentuk resminya:

1. Pemain menekan **Simpan dan Keluar ke Menu** di panel `Pause`.
2. `RunLifecycle.SaveAndQuitToMenu()` menerbitkan `SaveRequestedEvent { QuitToMenu }`, lalu menunggu `SaveCompletedEvent` paling lama `run.saveWaitTimeoutSeconds`.
3. `WorldStateSave` menulis langsung tanpa thread lain, walaupun game sedang dijeda dan pemain sedang bertempur.
4. Kalau gagal atau waktu habis, `RunLifecycle` menampilkan `ConfirmDialog` bahwa progres terakhir bisa hilang.
5. Di Fase 1 tidak ada save. `ConfirmDialog` memberi tahu bahwa run hilang.

### Urutan pulih

Tabel urutan `LoadOrder` 10 sampai 130 ada di `WorldStateSave.md` bagian "Membaca dan memulihkan". Ringkasnya: jam dunia lebih dulu, lalu region, akses region, tas, inventori, hotbar, status tubuh, posisi, bangunan, data pulau, progres, NPC, dan terakhir sistem ancaman.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `SaveCompletedEvent { reason, succeeded }`, oleh `WorldStateSave`.

**Event yang didengar:** `SaveRequestedEvent`, `DaybreakEvent`, `PlayerDamagedEvent`, `VitalsChangedEvent`, `VitalWarningChangedEvent`, `PlayerPinnedEvent`, `DisasterStartedEvent`, `DisasterEndedEvent`, dan `RunEndedEvent`, semuanya oleh `WorldStateSave`.

| Sistem | Hubungan |
|---|---|
| `WorldStateSave` (#80) | Pemilik semua logika save |
| `RunLifecycle` (#82) | Satu-satunya pemanggil alur muat, hapus, dan penanda akhir run |
| Pemilik data di tabel "Isi data" `WorldStateSave.md` | Memakai `ISaveable` |
| `RegionStreaming` (#88) | `RegionStateStore` untuk objek region |
| `RunSeedManager` (#89), `GameManager` | Dipanggil langsung, bukan lewat `ISaveable` |
| `MainMenuController` | `HasContinuableSave` dan `LastProblem` |
| `NotificationManager` (#22) | Mendengar `SaveCompletedEvent` untuk pesan gagal |

## 4. Kasus Khusus

Rincian ada di `WorldStateSave.md` bagian 4. Ringkasan untuk pemeriksaan cepat:

- **Objek pool:** objek pool tidak boleh memanggil `Register`. Isi pool tidak disimpan.
- **Game dijeda:** `QuitToMenu` tetap menulis saat `Pause` terbuka. Autosave tidak dimulai saat dijeda.
- **Permadeath:** `MarkRunEnded`, lalu `DeleteRunSave` menghapus `survival_save.json`, `.tmp`, `.bak`, dan `.corrupt.json`. `stats.json`, `lore_archive.json`, dan `PlayerPrefs` tetap.
- **Save di Fase 1:** tidak ada. `TrySaveNow` menolak kalau `RunLifecycle.State` bukan `OnIsland`.
- **Save rusak:** salin ke `.corrupt.json`, coba `.bak`, lalu sembunyikan Lanjutkan kalau keduanya gagal.
- **Versi file:** `saveVersion` terpisah dari versi game. Versi lebih baru atau lama tanpa migrasi tidak bisa dilanjutkan.
- **Script lama masih memanggil nama lama:** tidak boleh ada. Pemeriksaan di skenario tes 2 dan 3.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** penggantian `SaveSystem` (`WorldStateSave.md`, `RunLifecycle.md`). Pemicu autosave, aturan pertempuran, cadangan `.bak`, dan perilaku save rusak (`WorldStateSave.md` bagian 5, disetujui developer). Save & Quit lewat panel `Pause` (`RunLifecycle.md`, keputusan #82).

**Menunggu developer:** file ini bisa dihapus setelah semua rujukan pindah ke `WorldStateSave`. Rujukan yang masih memakai nama `SaveSystem`: `CLAUDE.md` Tahap 7 Langkah 1, `README.md` (tabel folder dan tahap 3), `RunLifecycle.md` (urutan bootstrap dan keputusan 5), `Island_Logic/WaterSwimmingController.md` bagian 4, dan `Survival Island/doc/USULAN_TERBUKA.md` (#82).

## 6. Skenario Tes

Ikuti skenario di `WorldStateSave.md` bagian 6. Tambahan untuk peralihan ini:

1. Selesaikan Fase 1. **Hasil:** `survival_save.json` muncul, tanpa file save lain dari nama lama.
2. Cari `File.WriteAllText` dan `PlayerPrefs.DeleteAll` di folder `LOGIC`. **Hasil:** tidak ada pemakaian untuk file save.
3. Cari `SaveSystem.` di folder `LOGIC`. **Hasil:** tidak ada pemanggilan kelas lama.
4. Buka `Pause` di Fase 2, lalu pilih Simpan dan Keluar ke Menu. **Hasil:** save tertulis, `MainMenu` tampil, dan Lanjutkan tersedia.
5. Ulangi langkah 4 di Fase 1. **Hasil:** `ConfirmDialog` tampil, dan tidak ada file save.
6. Buat folder save hanya-baca, lalu Simpan dan Keluar. **Hasil:** `ConfirmDialog` gagal simpan tampil, dan file lama tetap utuh.
7. Mati di Fase 2. **Hasil:** semua file run terhapus, `stats.json` dan setting tetap.
