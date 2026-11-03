# Blueprint: AsyncSceneLoader.cs
**Lokasi:** `docs/blueprint/Core_Logic/AsyncSceneLoader.md`
**Deskripsi Utama:** Satu-satunya pemanggil `SceneManager.LoadSceneAsync` dan `UnloadSceneAsync`. Memuat scene di latar belakang tanpa membuat game *freeze*.
**Issue:** #82
**Rujukan GDD:** Usulan teknis, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

`AsyncSceneLoader` adalah layanan tunggal persisten (`IBootstrapService`, #82). Kelas ini tidak memegang `Slider` atau teks. Panel `Loading` milik `UIManager` yang membaca `Progress`.

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Instance` | `AsyncSceneLoader` | `null` | Singleton |
| `Progress` | `float` | 0 | Kemajuan 0 sampai 1 |
| `IsBusy` | `bool` | `false` | Ada pemuatan scene tunggal yang berjalan |
| `activationAllowed` | `bool` | `false` | Sudah diizinkan lewat `AllowActivation` |

Waktu tampil minimum memakai `GameBalanceConfig.run.sceneLoadMinShowSeconds` (0,5).

## 2. Detail Logika & Fungsi

Rincian ada di `Core_Logic/RunLifecycle.md` bagian "Layanan scene tunggal".

| Fungsi | Dipakai oleh |
|---|---|
| `IEnumerator LoadSingle(string sceneName, bool holdActivation = false)` | `RunLifecycle` |
| `AsyncOperation LoadAdditive(string sceneName)` | `RegionStreaming` (#88) |
| `AsyncOperation UnloadAdditive(string sceneName)` | `RegionStreaming` (#88) |
| `void AllowActivation()` | `NarrativeSequencer` (#99) |
| `float Progress`, `bool IsBusy` | Panel `Loading` |

**`LoadSingle`**:

1. Tolak dan catat kalau `IsBusy`.
2. Set `operation = LoadSceneAsync(sceneName, Single)` dan `operation.allowSceneActivation = false`.
3. Selama `allowSceneActivation = false`, isi `Progress = Mathf.Clamp01(operation.progress / 0.9f)`. Skala ini diperlukan karena `progress` berhenti di 0,9 selama scene belum diaktifkan.
4. Tunggu `sceneLoadMinShowSeconds` waktu tak terskala, dan tunggu `AllowActivation()` kalau `holdActivation` aktif.
5. Set `allowSceneActivation = true`, `Progress = 1`, lalu tunggu `operation.isDone`.

## 3. Hubungan dengan Sistem Lain

**Event:** tidak mengirim dan tidak mendengar event.

| Sistem | Hubungan |
|---|---|
| `RunLifecycle` (#82) | Memanggil `LoadSingle` |
| `RegionStreaming` (#88) | Memanggil `LoadAdditive` dan `UnloadAdditive` |
| `NarrativeSequencer` (#99) | Memanggil `AllowActivation` |
| `UIManager` | Panel `Loading` membaca `Progress` |

## 4. Kasus Khusus

- **Objek pool:** tidak memakai pool.
- **Game dijeda:** penantian memakai waktu tak terskala, karena panel `Loading` menjeda waktu.
- **Permadeath:** `ResetForNewRun` mengosongkan `Progress` dan `activationAllowed`. Tidak ada data save.
- **Save dan load:** tidak ada data yang disimpan.
- **Scene gagal dimuat:** kalau `LoadSceneAsync` mengembalikan `null`, catat kesalahan, `IsBusy = false`, lalu `RunLifecycle` menangani alurnya.
- **Operasi additive selama `allowSceneActivation = false`:** mengantre di Unity, jadi region baru dimuat setelah scene aktif.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** fungsi publik dan logika `LoadSingle` (`RunLifecycle.md`).

**Menunggu developer:** tidak ada yang menghalangi.

## 6. Skenario Tes

1. Selesaikan Fase 1. **Hasil:** bar `Loading` penuh tanpa berhenti di 90 persen.
2. Panggil `LoadSingle` dua kali berurutan. **Hasil:** panggilan kedua ditolak dan dicatat.
3. Muat scene dengan `holdActivation = true`. **Hasil:** scene baru aktif setelah `AllowActivation()`.
4. Muat scene sangat kecil. **Hasil:** layar `Loading` tampil minimal 0,5 detik.
