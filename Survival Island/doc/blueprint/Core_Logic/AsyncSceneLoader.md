# Blueprint: AsyncSceneLoader.cs
**Lokasi:** `Survival Island/doc/blueprint/Core_Logic/AsyncSceneLoader.md`
**Deskripsi Utama:** Satu-satunya pemanggil `SceneManager.LoadSceneAsync` dan `UnloadSceneAsync`. Memuat scene di latar belakang tanpa membuat game *freeze*.
**Issue:** #82, #88, #99
**Rujukan GDD:** Usulan teknis, belum ada di GDD. Mendukung transisi Fase 1 ke Fase 2 (GDD bagian 1 dan `CLAUDE.md` Pilar 1) dan kembali ke kapal saat permadeath (GDD bagian 3).

## 1. Arsitektur & Variabel Utama

`AsyncSceneLoader` adalah layanan tunggal persisten (`IBootstrapService`, urutan ke-14 di #82), di bawah `PersistentRoot`. Kelas ini tidak memegang `Slider` atau teks. Panel `Loading` milik `UIManager` yang membaca `Progress`. Kelas ini juga tidak mengatur `Time.timeScale`, karena satu-satunya penulisnya adalah `UIFocusStack`.

Nama scene yang dipakai: `Bootstrap`, `MainMenu`, `ShipPhase`, `IslandPhase`, dan scene region additive (`GLOSSARY.md` bagian 3, `RegionStreaming.md`).

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Instance` | `AsyncSceneLoader` | `null` | Singleton |
| `Progress` | `float` | 0 | Kemajuan 0 sampai 1 untuk pemuatan scene tunggal |
| `IsBusy` | `bool` | `false` | Ada pemuatan scene tunggal yang berjalan |
| `LastLoadSucceeded` | `bool` | `true` | Hasil `LoadSingle` terakhir. Dibaca `RunLifecycle` setelah `yield return` |
| `activationAllowed` | `bool` | `false` | Sudah diizinkan lewat `AllowActivation` |
| `singleOperation` | `AsyncOperation` | `null` | Operasi scene tunggal yang berjalan |
| `additiveInFlight` | `int` | 0 | Jumlah operasi additive yang belum selesai |

`LastLoadSucceeded` adalah usulan tambahan. `LoadSingle` berupa coroutine, sehingga tidak bisa mengembalikan nilai. Tanpa properti ini, `RunLifecycle` tidak bisa menjalankan aturan "Scene gagal dimuat" di `RunLifecycle.md`.

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.run.sceneLoadMinShowSeconds` | 0,5 | Waktu tampil minimum layar loading, dalam detik tak terskala |

Tidak ada angka lain. Prioritas pemuatan latar belakang (`Application.backgroundLoadingPriority`) diatur `RegionStreaming` untuk region (#88).

## 2. Detail Logika & Fungsi

Sumber kontrak: `Core_Logic/RunLifecycle.md` bagian "Layanan scene tunggal". Bagian ini melengkapi langkah yang belum tertulis di sana.

| Fungsi | Dipakai oleh |
|---|---|
| `IEnumerator LoadSingle(string sceneName, bool holdActivation = false)` | `RunLifecycle`, `NarrativeSequencer.PlayDuringLoad` (#99) |
| `AsyncOperation LoadAdditive(string sceneName)` | `RegionStreaming` (#88) |
| `AsyncOperation UnloadAdditive(string sceneName)` | `RegionStreaming` (#88) |
| `void AllowActivation()` | `NarrativeSequencer` (#99) |
| `float Progress`, `bool IsBusy` | Panel `Loading` (`UIManager`), `NarrativeSequencer` |
| `bool LastLoadSucceeded` | `RunLifecycle` |

**`Initialize()`**: isi `Instance`. Tidak ada pekerjaan lain.

**`ResetForNewRun()`**: `Progress = 0` dan `activationAllowed = false`. Tidak menyentuh `IsBusy`, karena reset sendiri sedang memakai `LoadSingle("Bootstrap")`.

### `LoadSingle`

1. Kalau `IsBusy`, catat kesalahan, set `LastLoadSucceeded = false`, lalu selesai.
2. Kalau `!Application.CanStreamedLevelBeLoaded(sceneName)`, catat kesalahan bahwa scene tidak ada di Build Settings, set `LastLoadSucceeded = false`, lalu selesai.
3. `IsBusy = true`, `Progress = 0`, `activationAllowed = false`, dan catat waktu mulai dengan `Time.realtimeSinceStartup`.
4. Tunggu sampai `additiveInFlight == 0`. Operasi additive yang sudah berjalan diselesaikan dulu, supaya scene region tidak aktif setelah scene tunggal baru.
5. `singleOperation = LoadSceneAsync(sceneName, Single)`. Kalau `null`, catat kesalahan, `IsBusy = false`, `LastLoadSucceeded = false`, lalu selesai. `RunLifecycle` menangani alurnya.
6. `singleOperation.allowSceneActivation = false`.
7. Selama `allowSceneActivation = false`, isi `Progress = Mathf.Clamp01(singleOperation.progress / 0.9f)` tiap frame. Skala ini diperlukan karena `progress` berhenti di 0,9 selama scene belum diaktifkan.
8. Tunggu sampai `progress >= 0.9f`, sudah lewat `sceneLoadMinShowSeconds` waktu tak terskala, dan `activationAllowed` kalau `holdActivation` aktif.
9. `allowSceneActivation = true`, `Progress = 1`, lalu tunggu `singleOperation.isDone`.
10. `singleOperation = null`, `IsBusy = false`, dan `LastLoadSucceeded = true`.

Langkah 1 sampai 6 berjalan sebelum `yield` pertama. Karena itu `NarrativeSequencer` yang memanggil `AllowActivation` setelah `StartCoroutine(LoadSingle(...))` tidak pernah tertimpa oleh langkah 3.

Semua penantian memakai `yield return null` dan `Time.realtimeSinceStartup`, bukan `WaitForSeconds`, karena panel `Loading` menjeda waktu.

### `LoadAdditive`

1. Kalau `IsBusy`, catat peringatan dan kembalikan `null`. `RegionStreaming` sudah punya aturan mencoba lagi setelah 5 detik untuk hasil `null`.
2. Kalau scene sudah dimuat, catat peringatan dan kembalikan `null`.
3. `op = LoadSceneAsync(sceneName, Additive)`. Kalau `null`, kembalikan `null`.
4. `additiveInFlight++`, lalu daftarkan `op.completed += HandleAdditiveDone`, yang menjalankan `additiveInFlight--`.
5. Kembalikan `op`. `Progress` tidak berubah, karena pemuatan region tidak memakai layar loading.

`HandleAdditiveDone` adalah method milik kelas, bukan lambda, sesuai aturan `GameEventBus.md`.

### `UnloadAdditive`

1. Kalau scene tidak sedang dimuat, catat peringatan dan kembalikan `null`.
2. `op = UnloadSceneAsync(sceneName)`, dengan penghitung `additiveInFlight` yang sama seperti `LoadAdditive`.
3. Kembalikan `op`. Fungsi ini boleh dipanggil saat `IsBusy`, karena scene tunggal baru tetap melepas semua scene lama.

### `AllowActivation`

Kalau `!IsBusy`, catat peringatan dan abaikan. Selain itu, set `activationAllowed = true`. Panggilan ganda tidak berpengaruh.

## 3. Hubungan dengan Sistem Lain

**Event:** tidak mengirim dan tidak mendengar event. Pemberitahuan scene aktif dilakukan pemanggil, misalnya `RunLifecycle` lewat `PhaseChangedEvent` dan `RegionStreaming` lewat `RegionLoadStateChangedEvent`.

| Sistem | Hubungan |
|---|---|
| `RunLifecycle` (#82) | Memanggil `LoadSingle`, membaca `LastLoadSucceeded`, dan memanggil `Initialize` serta `ResetForNewRun` |
| `RegionStreaming` (#88) | Memanggil `LoadAdditive` dan `UnloadAdditive` |
| `NarrativeSequencer` (#99) | Memanggil `AllowActivation`, dan membaca `Progress` |
| `UIManager` | Panel `Loading` membaca `Progress` |
| `UIFocusStack` (#83) | Menjeda waktu saat panel `Loading` terbuka. Kelas ini tidak menulis `Time.timeScale` |

**Dihapus dari sistem lain:** `UIManager.ShowLoadingScreen` dan pemanggilan `SceneManager` langsung di `ShipTimerManager` (#82).

## 4. Kasus Khusus

- **Objek pool:** tidak memakai pool. `RunLifecycle` mengembalikan semua objek pool sebelum memanggil `LoadSingle` (#82).
- **Game dijeda:** penantian memakai waktu tak terskala, karena panel `Loading` menjeda waktu. Pemuatan tetap berjalan.
- **Permadeath:** `ResetForNewRun` mengosongkan `Progress` dan `activationAllowed`. Tidak ada data save.
- **Save dan load:** tidak ada data yang disimpan.
- **Scene gagal dimuat:** kalau scene tidak ada di Build Settings, atau `LoadSceneAsync` mengembalikan `null`, catat kesalahan, `IsBusy = false`, dan `LastLoadSucceeded = false`. `RunLifecycle` kembali ke `MainMenu`, atau tetap di layar loading kalau yang gagal adalah `MainMenu`.
- **Region sedang dimuat saat reset:** langkah 4 `LoadSingle` menunggu operasi additive selesai. Tanpa ini, scene region bisa aktif di atas `Bootstrap` atau `MainMenu`.
- **Permintaan region saat `LoadSingle` berjalan:** ditolak dengan `null`. Saat `EnterIslandPhase` dan `ContinueRun`, `LoadInitialRegions` baru dipanggil setelah `LoadSingle` selesai, jadi alur normal tidak terkena aturan ini.
- **`AllowActivation` terlambat:** kalau adegan tidak pernah memanggilnya, pemuatan menunggu. `NarrativeSequencer` memanggilnya di langkah 4 `PlayDuringLoad`, setelah adegan selesai atau dilewati. Usulan: panggilan itu ditaruh di blok `finally`, supaya adegan yang gagal tetap melepas pemuatan.
- **`holdActivation` tanpa `NarrativeSequencer`:** pemanggil wajib memanggil `AllowActivation`. Di build pengembangan, catat peringatan kalau penantian lebih dari 60 detik waktu tak terskala.
- **Klik ganda pada tombol menu:** `RunLifecycle` menolak lewat state. Kalau tetap terpanggil, langkah 1 `LoadSingle` menolaknya.
- **`Progress` mundur:** tidak terjadi, karena `Progress` di-reset hanya di awal `LoadSingle`. Panel `Loading` juga memakai `Mathf.MoveTowards` sehingga bar tidak melompat.
- **Scene dimuat dari Editor langsung:** `EditorQuickStart` (#82) menyiapkan layanan ini sebelum scene lain memintanya.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** fungsi publik dan logika dasar `LoadSingle` (`RunLifecycle.md`). Panel `Loading` membaca `Progress` (`UIManager.md`). Pemuatan region dan prioritasnya (`RegionStreaming.md`). Pemuatan saat adegan (`NarrativeSequencer.md`).

**Keputusan teknis di blueprint ini:**

- `LoadSingle` menunggu operasi additive selesai sebelum mulai.
- `LoadAdditive` ditolak saat `IsBusy`. `UnloadAdditive` tetap diizinkan.
- Penghitung additive memakai `AsyncOperation.completed` dengan method kelas, tanpa lambda.

**Menunggu developer:**

1. **`LastLoadSucceeded`:** properti baru, supaya `RunLifecycle` tahu kegagalan pemuatan. Kalau disetujui, `RunLifecycle.md` membacanya setelah setiap `yield return LoadSingle(...)`.
2. **Penulisan pemanggilan:** `RunLifecycle.md` dan `NarrativeSequencer.md` menulis `AsyncSceneLoader.LoadSingle`, sedangkan `RegionStreaming.md` menulis `AsyncSceneLoader.Instance.LoadAdditive`. Kelas ini singleton, bukan statis, jadi bentuk script yang benar memakai `Instance`.

## 6. Skenario Tes

1. Selesaikan Fase 1. **Hasil:** bar `Loading` penuh tanpa berhenti di 90 persen.
2. Panggil `LoadSingle` dua kali berurutan. **Hasil:** panggilan kedua ditolak dan dicatat, dan pemuatan pertama tetap selesai.
3. Muat scene dengan `holdActivation = true`. **Hasil:** scene baru aktif setelah `AllowActivation()`.
4. Muat scene sangat kecil. **Hasil:** layar `Loading` tampil minimal 0,5 detik.
5. Panggil `LoadSingle("SceneTidakAda")`. **Hasil:** kesalahan dicatat, `IsBusy` kembali `false`, `LastLoadSucceeded` bernilai `false`, dan `RunLifecycle` kembali ke `MainMenu`.
6. Lewati adegan intro secepat mungkin. **Hasil:** panel `Loading` tampil lagi kalau `Progress < 1`, dan scene tetap aktif setelah selesai.
7. Panggil `AllowActivation()` saat tidak ada pemuatan. **Hasil:** peringatan, tanpa efek.
8. Tambahkan penundaan buatan lewat `streamdelay 15` (#56), dekati seam, lalu pilih Simpan dan Keluar ke Menu saat region masih dimuat. **Hasil:** `MainMenu` tampil tanpa scene region tersisa di Hierarchy.
9. Panggil `LoadAdditive` region saat `LoadSingle` berjalan. **Hasil:** mengembalikan `null` dengan peringatan, lalu `RegionStreaming` mencoba lagi.
10. Mati di Fase 2, lalu ulangi dari kapal. **Hasil:** `Progress` mulai dari 0, bukan 1.
11. Profiler saat pemuatan `IslandPhase`. **Hasil:** bar terus bergerak selama pemuatan, dan lonjakan besar hanya terjadi pada frame aktivasi scene.
