# Blueprint: SaveSystem.cs
**Lokasi:** `docs/blueprint/Core_Logic/SaveSystem.md`
**Deskripsi Utama:** Catatan peralihan. Isi file ini digantikan `Core_Logic/WorldStateSave.md`. `GameData`, pemanggilan langsung ke manager, dan `File.WriteAllText` tidak dipakai lagi.
**Issue:** #82, #80
**Rujukan GDD:** Bagian 1 dan 3 (Permadeath). Rincian format adalah usulan teknis di `WorldStateSave.md`.

## 1. Arsitektur & Variabel Utama

Tidak ada kelas `SaveSystem` baru. Semua nama di file ini diteruskan ke `WorldStateSave` (#80). Selama `WorldStateSave` belum ditulis sebagai script, nama fungsi di bawah adalah kontrak yang harus disediakan.

**Dihapus:** kelas `GameData`, `saveFilePath` sendiri, `SaveGame()`, `LoadGame()`, `DeleteSaveFile()`, dan panggilan `UIManager.ShowNotification`.

## 2. Detail Logika & Fungsi

| Fungsi | Kegunaan | Dipanggil oleh |
|---|---|---|
| `bool HasContinuableSave()` | Tombol Lanjutkan boleh tampil | `MainMenuController` |
| `bool ReadRunFile()` | Membaca file save ke memori | `RunLifecycle.ContinueRun` |
| `void RestoreBaseScene()` | Memulihkan data scene dasar | `RunLifecycle.ContinueRun` |
| `void FinishRestore()` | Mengirim ulang event sticky | `RunLifecycle.ContinueRun` |
| `void SetAutosaveEnabled(bool)` | Mematikan autosave saat `Ending` | `RunLifecycle` |
| `void MarkRunEnded(RunResult, DeathReason)` | Menulis `run_end.marker` berisi `result`, `reason`, `currentDay`, dan `seed` | `RunLifecycle.RunEndSequence` |
| `void ClearRunEndMarker()` | Menghapus penanda | `RunLifecycle` |
| `bool TryReadRunEndMarker(out RunEndMarker)` | Membaca penanda saat boot | `RunLifecycle.RecoverInterruptedEnd` |
| `void DeleteRunSave()` | Menggantikan `DeleteSaveFile()` | `RunLifecycle` |

Pesan berhasil atau gagal tidak lagi memanggil `UIManager`. Sistem yang perlu memberi tahu pemain mendengar `SaveCompletedEvent`.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `SaveCompletedEvent`, oleh `WorldStateSave`.

**Event yang didengar:** `SaveRequestedEvent`, oleh `WorldStateSave`.

Pemilik data menyimpan lewat `ISaveable`. Daftarnya ada di `WorldStateSave.md`.

## 4. Kasus Khusus

Semua kasus khusus ada di `WorldStateSave.md`: pool, game dijeda, permadeath, save rusak, dan versi file.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** penggantian `SaveSystem` (`WorldStateSave.md`, `RunLifecycle.md`).

**Menunggu developer:** file ini bisa dihapus setelah semua rujukan pindah ke `WorldStateSave`.

## 6. Skenario Tes

Ikuti bagian 6 di `WorldStateSave.md`. Pastikan juga tidak ada script yang memanggil `File.WriteAllText` untuk file save.
