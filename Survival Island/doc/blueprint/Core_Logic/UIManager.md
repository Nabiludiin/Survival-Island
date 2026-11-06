# Blueprint: UIManager.cs
**Lokasi:** `Survival Island/doc/blueprint/Core_Logic/UIManager.md`
**Deskripsi Utama:** Pemilik panel persisten: `Pause`, `Loading`, `Cinematic`, `GameOver`, `WinScreen`, dan `ConfirmDialog`. Kelas ini mengisi panel, sedangkan waktu, kursor, dan tombol Esc diatur `UIFocusStack`.
**Issue:** #82, #83, #22, #104, #116
**Rujukan GDD:** Bagian 3 (Game Over dan Menang). Isi panel, tombol tambahan, dan pemilik `ConfirmDialog` adalah usulan, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

### Pola

`UIManager` adalah manager persisten (`IBootstrapService`, urutan ke-9 di #82). `Awake` hanya mengisi `Instance`. Panel tinggal di canvas persisten di bawah `PersistentRoot`, jadi tidak ikut hilang saat scene berganti.

Semua panel adalah turunan `UIPanel` (#83). Pengaturan jeda, kursor, Esc, dan kunci tiap panel mengikuti tabel di `UIFocusStack.md`, dan tidak ditulis ulang di sini.

| Kelas panel | `UIPanelId` | Isi |
|---|---|---|
| `PausePanel` | `Pause` | Tombol jeda, `NotificationLogView`, dan teks hasil keluar dari tersangkut |
| `LoadingPanel` | `Loading` | Bar kemajuan dan ikon berputar |
| `CinematicPanel` | `Cinematic` | Lapisan hitam penuh layar dan satu teks tengah |
| `EndScreenPanel` | `GameOver` atau `WinScreen` | Satu kelas untuk dua panel. `id` diatur di Inspector |
| `ConfirmDialogPanel` | `ConfirmDialog` | Judul, isi, tombol Ya dan Tidak |

**Pemilik `ConfirmDialog` (usulan):** `RunLifecycle`, `ProgressionSafety`, `SettingsManager`, `LocalStatsManager`, dan `LocalizationManager` sudah memakai `ConfirmDialog`, tetapi belum ada blueprint yang memilikinya. Panel ini persisten dan dipakai lintas scene, jadi ditaruh di sini.

`MainMenu` milik `MainMenuController`, `Settings` milik `SettingsManager` (#19), dan `Stats` milik `LocalStatsManager` (#57). `UIManager` hanya memegang rujukan ke `SettingsPanel` untuk tombol Pengaturan.

### Susunan canvas persisten (usulan)

Satu `Canvas` persisten per lapisan, dengan `Canvas Scaler` 1920x1080 dan `Match = 0,5`. Urutan dari bawah ke atas:

| `sortingOrder` | Lapisan | Pemilik |
|---|---|---|
| 0 | HUD | `HUDManager`, di canvas scene |
| 10 | Notifikasi dan petunjuk tutorial | #22, #16 |
| 20 | Panel tanpa jeda (Inventori, Crafting, dan lainnya) | Pemilik panel, di canvas scene |
| 30 | `Cinematic` | Sistem ini. Kanvas adegan `NarrativeSequencer` (#99) berada tepat di atasnya |
| 40 | `Pause`, `GameOver`, `WinScreen`, `MainMenu` | Sistem ini, `MainMenuController` |
| 45 | `Settings`, `Stats` | #19, #57 |
| 50 | `ConfirmDialog` | Sistem ini |
| 60 | `Loading` | Sistem ini |
| 100 | `DevConsole` | #56 |

### Variabel

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Instance` | `UIManager` | `null` | Singleton |
| `pausePanel` | `PausePanel` | Prefab | Diserahkan ke `UIFocusStack.pausePanel` lewat rujukan di prefab `PersistentRoot` |
| `loadingPanel` | `LoadingPanel` | Prefab | Dipakai `RunLifecycle` |
| `cinematicPanel` | `CinematicPanel` | Prefab | Dipakai `RunLifecycle`, `DeathCinematic` (#53), dan `NarrativeSequencer` (#99) |
| `gameOverPanel`, `winPanel` | `EndScreenPanel` | Prefab | Layar akhir, didorong `RunLifecycle` |
| `confirmDialog` | `ConfirmDialogPanel` | Prefab | Dialog konfirmasi bersama |
| `settingsPanel` | `UIPanel` | Prefab | Rujukan ke `SettingsPanel` milik #19 |
| `notificationLogView` | `NotificationLogView` | Prefab, nonaktif | Riwayat pesan di dalam `Pause`. Bukan `UIPanel` sendiri (#22) |

Rujukan panel di `RunLifecycle` dan `UIFocusStack` diisi di prefab `PersistentRoot`, dan menunjuk objek yang sama dengan variabel di atas. `UIManager` tidak membuat panel saat bermain.

**Dihapus:** `Update()` yang membaca Escape, `TogglePause()`, `isPaused`, `ShowLoadingScreen(AsyncOperation)`, `ShowGameOver(string)`, `ShowTooltip`, `HideTooltip`, dan `ShowNotification`. Kelas ini tidak lagi menulis `Time.timeScale` atau kursor.

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.ui.panelFadeSeconds` | 0,15 | Lama fade panel (sudah ada, #83) |
| `GameBalanceConfig.run.gameOverInputDelaySeconds` | 1 | Jeda sebelum tombol layar akhir aktif (sudah ada, #82) |
| `GameBalanceConfig.ui.loadingBarSpeed` | 1,5 | **Usulan.** Kecepatan bar `Loading`, dalam bagian per detik |
| `GameBalanceConfig.ui.confirmInputDelaySeconds` | 0,25 | **Usulan.** Jeda sebelum tombol `ConfirmDialog` aktif, supaya klik ganda tidak langsung menyetujui |

## 2. Detail Logika & Fungsi

### `public void Initialize()`

1. Pastikan semua rujukan panel terisi. Panel yang kosong dicatat sebagai kesalahan, dan boot tetap berlanjut.
2. Untuk tiap panel: `CanvasGroup.alpha = 0`, `interactable = false`, `blocksRaycasts = false`, lalu `SetActive(false)`.
3. Atur `Animator.updateMode = UnscaledTime` pada semua `Animator` di panel (`UIFocusStack.md`).
4. Pastikan `UIFocusStack.Instance.pausePanel == pausePanel`. Kalau berbeda, catat kesalahan. Ini menangkap prefab yang salah sambung.

### Perilaku tampil dan sembunyi bersama

Kelima kelas panel memakai langkah yang sama di `Show()` dan `Hide()`:

**`Show()`:** `SetActive(true)`, `blocksRaycasts = true`, lalu fade `alpha` 0 ke 1 selama `panelFadeSeconds` dengan `Time.unscaledDeltaTime`. `interactable = true` diisi saat fade mulai, kecuali panel yang memakai jeda input sendiri.

**`Hide()`:** `interactable = false` dan `blocksRaycasts = false` langsung, supaya klik tidak tembus ke panel yang sudah ditutup. Lalu fade ke 0, dan `SetActive(false)` setelah selesai. Kalau `Show()` dipanggil di tengah fade keluar, fade dibalik dari nilai sekarang.

Waktu sudah berhenti saat `Push`, tanpa menunggu fade (`UIFocusStack.md`). `CinematicPanel` dan `LoadingPanel` adalah pengecualian, lihat bagian masing-masing.

### Panel `Pause`

| Tombol | Syarat tampil | Aksi |
|---|---|---|
| Lanjut | Selalu | `UIFocusStack.Close(pausePanel, User)` |
| Pengaturan | Selalu | `UIFocusStack.Push(settingsPanel)` (#19) |
| `Log` | Selalu | Buka atau tutup `NotificationLogView`, yang mengisi daftarnya lewat `NotificationManager.Instance.GetLog(list)` (#22) |
| Keluar dari Tersangkut | Hanya `RunLifecycle.State == OnIsland` | `ShowConfirm` dengan kunci `ui.unstuck.confirm`. Kalau Ya, panggil `ProgressionSafety.Instance.RequestUnstuck()` dan tampilkan hasilnya (#116) |
| Simpan dan Keluar ke Menu | Selalu | `RunLifecycle.Instance.SaveAndQuitToMenu()` |

**`OnFocusGained()`** menyegarkan isi setiap kali `Pause` menjadi puncak stack:

1. Tombol Keluar dari Tersangkut tampil hanya saat `OnIsland`.
2. Label tombol keluar memakai `ui.pause.save_quit` di Fase 2, dan `ui.pause.quit` di Fase 1, karena Fase 1 tidak punya save (#82). Peringatan kehilangan run tetap diatur `RunLifecycle`.
3. Teks hasil tersangkut dikosongkan.
4. Fokus tombol pertama diatur ke Lanjut, supaya navigasi keyboard dan gamepad punya awal.

**Hasil `UnstuckResult`:**

| Hasil | Tampilan |
|---|---|
| `Started` | Tidak ada. `ProgressionSafety` menutup `Pause` sendiri dan menampilkan hitung mundur |
| `Busy`, `Cooldown`, `NotAllowed`, `NoSafePoint` | Teks `ui.unstuck.<UnstuckResult>` di bawah tombol, lewat `LocalizationManager` (#104). Notifikasi tidak dipakai, karena `NotificationManager` menahan pesan selama `Pause` terbuka (#22) |

**`OnClosed(reason)`:** tutup `NotificationLogView` kalau terbuka, supaya `Pause` berikutnya mulai dari menu utama jeda.

### Panel `Loading`

`LoadingPanel.Update()` hanya berjalan saat panel aktif, dan memakai `Time.unscaledDeltaTime`, karena `Loading` menjeda waktu.

```csharp
float target = AsyncSceneLoader.Instance.Progress;
if (loader.IsBusy && !wasBusy) displayed = 0f;    // Pemuatan baru dimulai
wasBusy = loader.IsBusy;
displayed = Mathf.MoveTowards(displayed, Mathf.Max(displayed, target), loadingBarSpeed * Time.unscaledDeltaTime);
loadingProgressBar.value = displayed;
spinner.Rotate(0f, 0f, -spinnerDegreesPerSecond * Time.unscaledDeltaTime);
```

- Dalam satu pemuatan, bar tidak pernah mundur, karena target memakai `Mathf.Max`.
- Panel `Loading` dipakai ulang di antara reset dan pemuatan scene fase (#82). Saat `IsBusy` berubah dari `false` ke `true`, bar mulai dari 0 lagi.
- Setelah `Progress = 1`, bar penuh tetapi ikon tetap berputar. Ini menutupi `LoadInitialRegions` (#88), yang tidak melaporkan kemajuan.
- `Show()` panel ini tidak memakai fade masuk. Layar langsung tertutup, supaya pergantian scene tidak terlihat.
- `spinnerDegreesPerSecond` adalah nilai tampilan di prefab (180), bukan angka permainan.

### Panel `Cinematic`

Panel ini menjadi layar untuk `DeathCinematic` (#53) dan adegan `NarrativeSequencer` (#99). Saat terbuka, dunia tetap terlihat sampai lapisan hitam dinaikkan.

| Fungsi | Pemakai | Isi |
|---|---|---|
| `IEnumerator FadeToBlack(float seconds)` | `DeathCinematic` langkah 8, #99 | Naikkan `blackOverlay.alpha` ke 1 dengan `Time.unscaledDeltaTime` |
| `IEnumerator FadeFromBlack(float seconds)` | #99 | Turunkan ke 0 |
| `void SetBlack(float alpha)` | #53, #99 | Atur langsung tanpa fade |
| `void ShowCaption(string key)` | `DeathCinematic` (`messageKey`) | Teks tengah dari `LocalizationManager.Get(key)` (#104) |
| `void ClearCaption()` | #53, #99 | Kosongkan teks tengah |

- **`Show()`:** `blackOverlay.alpha = 0`, teks kosong, dan panel langsung tampil tanpa fade.
- **`Hide()`:** langsung mati tanpa fade. `RunLifecycle` menutup `Cinematic` dan mendorong layar akhir pada frame yang sama, jadi tidak ada frame dunia yang terlihat (`DeathCinematic.md`: layar tetap hitam sampai `GameOver` tampil).
- **Pemanggilan bertumpuk:** fade baru menghentikan fade lama, lalu mulai dari nilai `alpha` saat itu.

### Layar akhir `GameOver` dan `WinScreen`

**`public void ShowGameOver(DeathReason reason)`** mengisi `gameOverPanel`, dan tidak mendorong panel sendiri:

| Bagian | Sumber | Kalau tidak tersedia |
|---|---|---|
| Penyebab | Kunci `gameover.<DeathReason>` lewat `LocalizationManager` (#104) | Kunci mentah, sesuai aturan #104 |
| Hari tercapai | `GameManager.Instance.currentDay` | Selalu ada |
| Seed dan versi | `RunSeedManager.Instance.CurrentSeedText` dan `Application.version`, contoh `Seed 7F3A-91C2 · v0.1.0` (#89) | Baris dikosongkan |
| Statistik run | `LocalStatsManager.Instance.CurrentRun`: zombie dibunuh dan tembakan meleset (#57). Tanda `[CHEAT]` kalau `isTainted` | Baris disembunyikan |

**`public void ShowWin()`** mengisi `winPanel` dengan cara yang sama, tanpa penyebab. `RunLifecycle` memanggil salah satu fungsi ini di langkah 8, lalu mendorong panelnya.

**Tombol layar akhir (`RunLifecycle.md`):**

| Panel | Tombol | Aksi |
|---|---|---|
| `GameOver` | Ulangi dari Kapal | `RunLifecycle.Instance.StartNewRun()` (GDD bagian 3) |
| `GameOver`, `WinScreen` | Menu Utama | `RunLifecycle.Instance.ReturnToMainMenu()` |

- **`Show()`:** latar hitam penuh langsung tampil dengan `alpha` 1, lalu isi panel fade masuk. Tombol mulai dengan `interactable = false`.
- **`OnFocusGained()`:** mulai hitung `gameOverInputDelaySeconds` dengan waktu tak terskala. Setelah lewat, tombol aktif dan fokus diatur ke tombol pertama.
- **Klik tombol:** semua tombol langsung `interactable = false`, supaya klik ganda tidak memanggil dua alur. `RunLifecycle` juga menolak panggilan yang tidak sesuai state.

### `ConfirmDialog` (usulan)

```csharp
public struct ConfirmRequest
{
    public string titleKey;          // Kunci #104
    public string bodyKey;           // Kunci #104
    public string yesKey, noKey;     // Kosong berarti ui.yes dan ui.no
    public Action<bool> onResult;    // true untuk Ya, false untuk Tidak atau Esc
}
```

**`public bool ShowConfirm(in ConfirmRequest request)`**:

1. Kalau `confirmDialog` sudah terbuka, catat peringatan dan kembalikan `false`. Satu dialog berlaku pada satu waktu.
2. Simpan `request`, isi teks lewat `LocalizationManager.Get`, dan atur tombol ke `interactable = false`.
3. `return UIFocusStack.Instance.Push(confirmDialog)`. Kalau ditolak, kosongkan `request` dan kembalikan `false`.
4. Setelah `confirmInputDelaySeconds` waktu tak terskala, tombol aktif. Fokus awal ada di tombol Tidak, supaya `Enter` tidak menyetujui tanpa sengaja.

**Hasil:**

| Cara tertutup | `onResult` |
|---|---|
| Klik Ya | `true` |
| Klik Tidak, atau Esc (`UICloseReason.User`) | `false` |
| `Forced`, `RunEnded`, atau `SceneUnloaded` | Tidak dipanggil |

Urutannya: tutup panel lewat `UIFocusStack.Close`, kosongkan `request`, lalu panggil `onResult`. Dengan begitu, `onResult` boleh membuka dialog baru, dan pemanggil seperti `ProgressionSafety` boleh menutup `Pause` di bawahnya.

Pemanggil yang harus bertindak saat dialog tertutup paksa memakai `OnClosed` panelnya sendiri, seperti `SettingsPanel` yang menjalankan `Commit` (#19).

### `public void ResetForNewRun()`

Dipanggil `RunLifecycle` langkah 10. `UIFocusStack.ClearForNewRun()` sudah menutup semua panel lebih dulu.

1. Kosongkan teks layar akhir, teks hasil tersangkut, dan teks `Cinematic`.
2. `blackOverlay.alpha = 0`, `displayed = 0`, dan `wasBusy = false`.
3. Kosongkan `request` milik `ConfirmDialog` tanpa memanggil `onResult`.
4. Tutup `NotificationLogView`.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** tidak ada. Panel membaca data saat dibuka.

**Event yang didengar:** `LocaleChangedEvent` (#104). Panel yang sedang terbuka mengisi ulang teksnya. Panel tertutup diisi ulang saat dibuka.

| Sistem | Hubungan |
|---|---|
| `UIFocusStack` (#83) | Mengatur waktu, kursor, input, dan Esc. Menerima `pausePanel` lewat prefab |
| `RunLifecycle` (#82) | Mendorong dan menutup panel alur run, memanggil `ShowGameOver` dan `ShowWin`. Menerima klik tombol layar akhir dan `SaveAndQuitToMenu` |
| `AsyncSceneLoader` | Sumber `Progress` dan `IsBusy` |
| `DeathCinematic` (#53), `NarrativeSequencer` (#99) | Memakai fungsi `CinematicPanel` |
| `RunSeedManager` (#89), `LocalStatsManager` (#57), `GameManager` | Sumber data layar akhir |
| `LocalizationManager` (#104) | Semua teks panel |
| `NotificationManager` (#22) | Riwayat pesan di `NotificationLogView` |
| `ProgressionSafety` (#116) | Keluar dari tersangkut |
| `SettingsManager` (#19) | Panel `Settings` dibuka dari `Pause` |
| `RunLifecycle`, `ProgressionSafety`, `SettingsManager`, `LocalStatsManager`, `LocalizationManager`, `MainMenuController` | Memakai `ShowConfirm` |

## 4. Kasus Khusus

- **Objek pool:** tidak memakai pool. Panel dibuat sekali di prefab.
- **Game dijeda:** semua fade, hitung jeda tombol, dan bar memakai `Time.unscaledDeltaTime` atau `WaitForSecondsRealtime`. `Animator` panel memakai `UnscaledTime`.
- **Permadeath:** `ResetForNewRun` mengosongkan isi panel. `UIFocusStack.ClearForNewRun` menutup panelnya.
- **Save dan load:** panel tidak disimpan (`WorldStateSave.md`). Run yang dimuat mulai dengan stack kosong.
- **`LocalStatsManager` atau seed belum tersedia:** baris dikosongkan, dan layar akhir tetap tampil. Kegagalan data tidak boleh menahan permadeath.
- **Mati saat `Pause` atau `ConfirmDialog` terbuka:** `RunLifecycle` menutup panel lewat `CloseAllUnlocked(RunEnded)`. `onResult` dialog tidak dipanggil, lalu layar akhir diisi.
- **Esc di layar akhir:** tidak berpengaruh, karena `GameOver` dan `WinScreen` tidak bisa ditutup Esc (`UIFocusStack.md`).
- **Klik ganda di Ya pada `ConfirmDialog`:** `request` dikosongkan sebelum `onResult`, jadi klik kedua tidak memanggil apa pun.
- **`ShowConfirm` saat `Loading` atau `Cinematic` di puncak:** `Push` ditolak karena `rejectsNewPanels`, dan fungsi mengembalikan `false`. Pemanggil seperti `RunLifecycle` harus menangani hasil ini.
- **Bahasa diganti saat `Pause` terbuka:** `LocaleChangedEvent` mengisi ulang label tombol dan teks hasil.
- **Pemuatan scene gagal (`AsyncSceneLoader`):** bar berhenti di nilai terakhir, dan `RunLifecycle` menangani alurnya. Panel tidak menutup dirinya sendiri.
- **Jendela kehilangan fokus:** `UIFocusStack` mendorong `pausePanel`. `UIManager` hanya menyegarkan isinya lewat `OnFocusGained`.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** `UIManager` tidak mengatur waktu dan kursor (`UIFocusStack.md`). Isi layar akhir dan tombolnya (`RunLifecycle.md`). `ShowGameOver(DeathReason)` (`LocalizationManager.md`). Seed dan versi di layar akhir (`RunSeedManager.md`). Statistik layar akhir dan tanda `[CHEAT]` (`LocalStatsManager.md`). Layar tetap hitam sampai `GameOver` tampil (`DeathCinematic.md`).

**Usulan yang disetujui developer sebagai nilai awal (dicatat di #85):** tombol `Log` di `Pause` (#22), dan tombol Keluar dari Tersangkut di Fase 2 (#116).

**Usulan baru (disetujui developer sebagai nilai awal):**

1. `UIManager` menjadi pemilik `ConfirmDialog`, dengan `ShowConfirm(ConfirmRequest)`.
2. API `CinematicPanel` untuk fade hitam dan teks tengah, karena `DeathCinematic.md` sudah menyerahkan tugas ini ke `UIManager`.
3. Urutan `sortingOrder` canvas persisten.
4. Angka `ui.loadingBarSpeed` (1,5) dan `ui.confirmInputDelaySeconds` (0,25).
5. Label tombol keluar di Fase 1 menjadi "Keluar ke Menu".

## 6. Skenario Tes

1. Tekan `Esc` saat bermain. **Hasil:** `Pause` tampil lewat `UIFocusStack`, dan kelas ini tidak menyentuh `Time.timeScale`.
2. Mati karena `Frostbite`. **Hasil:** `GameOver` menampilkan teks `gameover.Frostbite`, hari, seed, versi, zombie dibunuh, dan tembakan meleset.
3. Ganti bahasa, lalu mati lagi. **Hasil:** teks penyebab ikut berubah.
4. Klik `Log` di `Pause`. **Hasil:** riwayat pesan tampil, tanpa panel baru di stack. Tutup lalu buka lagi `Pause`. **Hasil:** riwayat sudah tertutup.
5. Di Fase 1, buka `Pause`. **Hasil:** tombol Keluar dari Tersangkut tidak tampil, dan label tombol keluar adalah "Keluar ke Menu".
6. Di Fase 2, klik Keluar dari Tersangkut lalu setujui. **Hasil:** `Pause` tertutup dan hitung mundur mulai. Ulangi saat cooldown. **Hasil:** teks `ui.unstuck.Cooldown` tampil di `Pause`.
7. Klik Keluar dari Tersangkut, lalu tekan Esc di dialog. **Hasil:** hanya dialog yang tertutup, `RequestUnstuck` tidak dipanggil, dan `Pause` tetap terbuka.
8. Muat scene besar. **Hasil:** bar `Loading` tidak mundur, tetap bergerak saat waktu berhenti, dan ikon tetap berputar setelah bar penuh.
9. Mati, lalu pilih Ulangi dari Kapal. **Hasil:** bar mulai dari 0 untuk pemuatan `ShipPhase`, bukan tetap penuh dari pemuatan `Bootstrap`.
10. Mati, lalu klik tombol layar akhir sebelum 1 detik. **Hasil:** tidak ada aksi. Setelah 1 detik, tombol aktif.
11. Klik Menu Utama dua kali dengan cepat. **Hasil:** `ReturnToMainMenu` hanya berjalan sekali.
12. Mati karena `Drowned`, dan perhatikan saat `Cinematic` berganti ke `GameOver`. **Hasil:** tidak ada satu frame dunia yang terlihat.
13. Buka `ConfirmDialog` dari `Pause`, lalu turunkan HP ke 0 lewat konsol. **Hasil:** dialog dan `Pause` tertutup, `onResult` tidak dipanggil, lalu `Cinematic` tampil.
14. Panggil `ShowConfirm` dua kali berturut-turut. **Hasil:** panggilan kedua mengembalikan `false` dengan peringatan.
15. Menang lewat konsol. **Hasil:** `WinScreen` tampil tanpa baris penyebab, dengan satu tombol Menu Utama.
16. Pakai perintah cheat dari `DevConsole` yang menandai run (`isTainted`), lalu mati. **Hasil:** statistik tampil dengan tanda `[CHEAT]`.
17. Cari `Time.timeScale` dan `Cursor.lockState` di script UI. **Hasil:** tidak ada penulisan di kelas ini.
