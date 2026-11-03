# Blueprint: DiaryManager.cs
**Lokasi:** `docs/blueprint/Island_Logic/DiaryManager.md`
**Deskripsi Utama:** Menampilkan isi bab Diary dan membuka arsip baca ulang. Jeda, kursor, dan penutupan panel diatur `UIFocusStack`.
**Issue:** #81 (melengkapi #83, #18, dan #104)
**Rujukan GDD:** Bagian 4 (Sistem Diary, Bab 1 sampai 3). Jeda saat membaca mengikuti `Claude.md` Tahap 5. Tombol `J` dan arsip baca ulang adalah usulan, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

`DiaryManager` adalah manager di scene `IslandPhase`. Status bab dan hadiah milik `ObjectiveManager` (#81), jadi `DiaryManager` tidak menyimpan apa pun.

| Kelas | Jenis | Fungsi |
|---|---|---|
| `DiaryManager` | Manager di `IslandPhase` | Pintu masuk `OpenDiary`, dan membaca tombol arsip |
| `DiaryPanel` | Turunan `UIPanel`, `UIPanelId.Diary` | Panel teks satu bab |
| `DiaryArchivePanel` | Turunan `UIPanel` | Daftar bab ditemukan dan tujuan selesai. Isi dan tab `Arsip` ada di `ObjectiveManager.md` |

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Instance` | `DiaryManager` | `null` | Singleton per scene |
| `diaryPanel` | `DiaryPanel` | Dari scene | Panel baca |
| `archivePanel` | `DiaryArchivePanel` | Dari scene | Panel arsip |
| `titleText`, `contentText` | `TextMeshProUGUI` | Dari prefab | Milik `DiaryPanel` |

**Dihapus:** `isReading`, `Update()` yang membaca `Escape`, serta semua penulisan `Time.timeScale` dan `Cursor`.

## 2. Detail Logika & Fungsi

**`public void OpenDiary(DiaryEntry entry)`**, menggantikan `OpenDiary(string title, string content)`:

1. `titleText.text = LocalizationManager.Get(entry.titleKey)`, dan `contentText.text = LocalizationManager.Get(entry.bodyKey)` (#104).
2. `UIFocusStack.Instance.Push(diaryPanel)`. Panel `Diary` menjeda game dan melepas kursor sesuai tabel `UIFocusStack.md`.

**Tombol Close** pada `DiaryPanel` memanggil `UIFocusStack.Instance.Close(diaryPanel, UICloseReason.User)`. Esc ditangani `UIFocusStack`, karena hanya sistem itu yang membaca Esc.

**Aksi input `OpenDiary` (`J`):** kalau `InputManager.Instance.OpenDiaryPressed` dan `!UIFocusStack.Instance.IsGameplayInputBlocked`, panggil `UIFocusStack.Instance.Push(archivePanel)`. Memilih bab di arsip memanggil `OpenDiary(entry)`. Panel baca berada di atas arsip, sehingga menutupnya kembali ke arsip.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim dan didengar:** tidak ada. Penemuan bab menerbitkan `DiaryChapterFoundEvent` dari `ObjectiveManager`.

| Sistem | Hubungan |
|---|---|
| `ObjectiveManager` (#81) | Memanggil `OpenDiary(entry)`, dan menyediakan `DiaryEntry`, `foundChapters`, serta isi arsip |
| `UIFocusStack` (#83) | `Push` dan `Close`. Pemilik jeda dan kursor |
| `InputManager` (#18) | Aksi `OpenDiary` |
| `LocalizationManager` (#104) | Teks `diary.ch<chapter>.title` dan `diary.ch<chapter>.body` |
| `LoreCollectibles` (#100) | Tab `Arsip` pada `DiaryArchivePanel` |

## 4. Kasus Khusus

- **Pool dipakai ulang:** panel tidak memakai pool.
- **Game dijeda:** `Diary` sudah menjeda. Membuka Pause di atasnya, lalu menutup Diary, tidak menjalankan waktu kembali, karena jeda dihitung dari seluruh isi stack.
- **Tombol `J` saat panel lain terbuka:** ditolak lewat `IsGameplayInputBlocked`.
- **Save dan load:** tidak ada data milik `DiaryManager`. Bab ditemukan tersimpan di `world_objectives`.
- **Permadeath:** `DiaryManager` hilang bersama scene `IslandPhase`, dan isi arsip mulai dari nol.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** Diary menjeda game (`Claude.md` Tahap 5), panel dan Esc lewat `UIFocusStack` (#83), serta `OpenDiary(DiaryEntry)` dan `DiaryArchivePanel` (#81).

**Usulan yang menunggu developer (#85):**

1. **Tombol `J` dan arsip baca ulang:** rekomendasi pakai, karena Diary berfungsi sebagai *guidebook* sepanjang run.

## 6. Skenario Tes

1. Tekan `E` pada buku Bab 1. **Hasil:** panel terbuka, `Time.timeScale` 0, dan kursor lepas.
2. Klik Close. **Hasil:** panel tertutup, waktu berjalan, dan kursor terkunci.
3. Buka lagi, lalu tekan Esc. **Hasil:** panel tertutup lewat `UIFocusStack`.
4. Cari `Time.timeScale` dan `Cursor.` di `DiaryManager.cs`. **Hasil:** tidak ada.
5. Tekan `J`, lalu pilih bab. **Hasil:** panel baca terbuka di atas arsip. Tutup panel. **Hasil:** arsip masih terbuka.
6. Tekan `J` saat `CraftingPanel` terbuka. **Hasil:** arsip tidak terbuka.
7. Ganti bahasa, lalu buka bab. **Hasil:** teks mengikuti bahasa baru.
8. Mati, lalu mulai run baru dan tekan `J`. **Hasil:** arsip kosong.
