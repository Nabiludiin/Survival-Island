# Blueprint: UIManager.cs
**Lokasi:** `docs/blueprint/Core_Logic/UIManager.md`
**Deskripsi Utama:** Pemilik panel persisten: `Pause`, `Loading`, `Cinematic`, `GameOver`, dan `WinScreen`. Kelas ini mengisi panel, sedangkan waktu, kursor, dan tombol Esc diatur `UIFocusStack`.
**Issue:** #82, #83, #22, #104, #116
**Rujukan GDD:** Bagian 3 (Game Over). Isi panel dan tombol tambahan adalah usulan, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

`UIManager` adalah manager persisten (`IBootstrapService`, #82). `Awake` memakai pola bootstrap: hanya mengisi `Instance`. Semua panel adalah turunan `UIPanel` (#83).

| Variabel | Tipe | Fungsi |
|---|---|---|
| `Instance` | `UIManager` | Singleton |
| `pausePanel` | `UIPanel` | Panel `Pause`. Diserahkan ke `UIFocusStack` |
| `gameOverPanel`, `winPanel` | `UIPanel` | Layar akhir, didorong `RunLifecycle` lewat `UIFocusStack` |
| `loadingPanel`, `cinematicPanel` | `UIPanel` | Panel alur run |
| `loadingProgressBar` | `Slider` | Bar di panel `Loading` |
| `notificationLogView` | `NotificationLogView` | Riwayat pesan di dalam `Pause`. Bukan `UIPanel` sendiri (#22) |

**Dihapus:** `Update()` yang membaca Escape, `TogglePause()`, `isPaused`, `ShowLoadingScreen(AsyncOperation)`, dan `ShowGameOver(string)`. Kelas ini tidak lagi menulis `Time.timeScale` atau kursor.

## 2. Detail Logika & Fungsi

**`public void Initialize()`**: nonaktifkan semua panel, lalu serahkan `pausePanel` ke `UIFocusStack`.

**Panel `Loading`:** selama aktif, bar membaca `AsyncSceneLoader.Instance.Progress`, lalu bergerak dengan `Mathf.MoveTowards` dan `Time.unscaledDeltaTime`.

**`public void ShowGameOver(DeathReason reason)`** mengisi `gameOverPanel`, dan tidak mendorong panel sendiri:

- Penyebab dari kunci `gameover.<DeathReason>` lewat `LocalizationManager` (#104).
- Hari tercapai dari `GameManager.currentDay`.
- Seed dari `RunSeedManager.Instance.CurrentSeedText`.
- Statistik run dari `LocalStatsManager.CurrentRun` (#57).

`winPanel` diisi dengan cara yang sama, tanpa penyebab. `RunLifecycle` mendorong panelnya sesudah pengisian.

**Panel `Pause`** memuat:

| Tombol | Syarat | Aksi |
|---|---|---|
| Lanjut | Selalu | `UIFocusStack.Close(pausePanel, User)` |
| Pengaturan | Selalu | Dorong panel `Settings` (#19) |
| `Log` | Selalu | Buka `NotificationLogView`, yang mengisi daftarnya lewat `NotificationManager.Instance.GetLog(list)` (#22) |
| Keluar dari Tersangkut | Hanya Fase 2 | Buka `ConfirmDialog`, lalu panggil `ProgressionSafety.RequestUnstuck()` dan tampilkan hasil `UnstuckResult` (#116) |
| Simpan dan Keluar ke Menu | Selalu | `RunLifecycle.SaveAndQuitToMenu()` |

## 3. Hubungan dengan Sistem Lain

**Event:** tidak mengirim event gameplay. Panel membaca data saat dibuka.

| Sistem | Hubungan |
|---|---|
| `UIFocusStack` (#83) | Mengatur waktu, kursor, input, dan Esc |
| `RunLifecycle` (#82) | Mendorong dan menutup panel alur run |
| `AsyncSceneLoader` | Sumber `Progress` |
| `RunSeedManager` (#89), `LocalStatsManager` (#57) | Sumber data layar akhir |
| `LocalizationManager` (#104) | Teks layar akhir |
| `NotificationManager` (#22) | Riwayat pesan |
| `ProgressionSafety` (#116) | Keluar dari tersangkut |

## 4. Kasus Khusus

- **Objek pool:** tidak memakai pool.
- **Game dijeda:** semua animasi panel memakai `Time.unscaledDeltaTime`.
- **Permadeath:** `ResetForNewRun` mengosongkan isi panel. `UIFocusStack.ClearForNewRun` menutup panelnya.
- **Save dan load:** panel tidak disimpan.
- **`LocalStatsManager` atau seed belum tersedia:** kolom dikosongkan, dan layar akhir tetap tampil.
- **Mati saat `Pause` terbuka:** `RunLifecycle` menutup panel lewat `CloseAllUnlocked`, lalu mengisi layar akhir.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** `UIManager` tidak mengatur waktu dan kursor (`UIFocusStack.md`). Isi layar akhir (`RunLifecycle.md`). `ShowGameOver(DeathReason)` (`LocalizationManager.md`).

**Usulan yang disetujui developer sebagai nilai awal (dicatat di #85):** tombol `Log` di `Pause` (#22), dan tombol Keluar dari Tersangkut di Fase 2 (#116).

## 6. Skenario Tes

1. Tekan `Esc` saat bermain. **Hasil:** `Pause` tampil lewat `UIFocusStack`, dan kelas ini tidak menyentuh `Time.timeScale`.
2. Mati karena `Frostbite`. **Hasil:** `GameOver` menampilkan teks `gameover.Frostbite`, hari, dan seed.
3. Ganti bahasa, lalu mati lagi. **Hasil:** teks penyebab ikut berubah.
4. Klik `Log` di `Pause`. **Hasil:** riwayat pesan tampil, tanpa panel baru di stack.
5. Di Fase 1, buka `Pause`. **Hasil:** tombol Keluar dari Tersangkut tidak tampil.
6. Di Fase 2, klik Keluar dari Tersangkut lalu setujui. **Hasil:** hasil `UnstuckResult` tampil.
7. Muat scene besar. **Hasil:** bar `Loading` tidak mundur dan tetap bergerak saat waktu berhenti.
