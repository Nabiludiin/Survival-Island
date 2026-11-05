# Blueprint: ShipTimerManager.cs
**Lokasi:** `docs/blueprint/60-seconds/ShipTimerManager.md`
**Deskripsi Utama:** Menghitung mundur Fase 1 dan mengatur atmosfer kepanikan lewat audio dan efek layar. Hasil akhir Fase 1 dan perpindahan scene milik `LifeboatExtraction` (#74).
**Issue:** #74, #82
**Rujukan GDD:** Bagian 3 (Fase 1 selama persis 60 detik). Efek kepanikan adalah usulan.

## 1. Arsitektur & Variabel Utama

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `SecondsLeft` | `float` | `GameBalanceConfig.ship.durationSeconds` (60) | Sisa waktu |
| `isRunning` | `bool` | `false` | Timer tidak berjalan sebelum `StartCountdown` |
| `isTimeUp` | `bool` | `false` | Waktu sudah habis |
| `timerText` | `TextMeshProUGUI` | Dari scene | Tampilan timer |
| `vignetteOverlay` | `Image` | Dari scene | Efek gelap di pinggir layar |
| `panicCurve` | `AnimationCurve` | Dari Inspector | Kekuatan getar UI |
| `tickTockAudio`, `shipGroanAudio` | `AudioSource` | Dari scene | Suara detak dan kapal |
| `basePitch`, `maxPitch` | `float` | 1, 2 | Rentang pitch |

## 2. Detail Logika & Fungsi

**`public void StartCountdown()`**: isi `SecondsLeft`, lalu `isRunning = true`.

**`public void StopCountdown()`**: `isRunning = false`.

**`void Update()`**:

1. Keluar kalau `!isRunning` atau `isTimeUp`.
2. `SecondsLeft -= Time.deltaTime`.
3. Saat `SecondsLeft` di bawah 15: `panicFactor = 1 - SecondsLeft / 15`, lalu ubah pitch, warna vignette, dan getar teks.
4. Kalau `SecondsLeft <= 0`, panggil `ExecuteTimeUp()`.

**`void ExecuteTimeUp()`**:

1. `isTimeUp = true`.
2. Mainkan sirene dan suara air.
3. Panggil `LifeboatExtraction.Instance.HandleTimeUp()`.

`LifeboatExtraction` menentukan hasil, menyimpan barang lewat `GameManager.SaveShipOutcome`, lalu memanggil `RunLifecycle.Instance.EnterIslandPhase()`. Kelas ini tidak menyimpan `savedShipLoot` dan tidak memanggil `SceneManager`.

**Berhenti saat run berakhir:** mendengar `RunEndedEvent`, lalu memanggil `StopCountdown()`.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** tidak ada.

**Event yang didengar:** `RunEndedEvent`.

| Sistem | Hubungan |
|---|---|
| `LifeboatExtraction` (#74) | Memanggil `StartCountdown` dan `StopCountdown`, dan menerima `HandleTimeUp` |
| `ShipHazardManager` (#75) | Membaca `SecondsLeft` untuk jadwal bahaya |
| `RunLifecycle` (#82) | Dipanggil oleh `LifeboatExtraction`, bukan kelas ini |

## 4. Kasus Khusus

- **Objek pool:** tidak memakai pool.
- **Game dijeda:** `Time.deltaTime` bernilai 0, jadi timer berhenti.
- **Permadeath:** kelas ini hilang bersama `ShipPhase`, dan timer mulai ulang pada run baru.
- **Save dan load:** Fase 1 tidak punya save (#82).
- **Mati sebelum waktu habis:** `RunEndedEvent` menghentikan timer, dan `HandleTimeUp` tidak terpanggil.
- **Berangkat lebih awal:** `LifeboatExtraction` memanggil `StopCountdown`.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** alur Fase 1 dan pemanggilan `EnterIslandPhase` (`LifeboatExtraction.md`). Durasi 60 detik (GDD bagian 3).

**Menunggu developer:** tidak ada yang menghalangi.

## 6. Skenario Tes

1. Masuk `ShipPhase`, lalu tunggu sebelum `StartCountdown`. **Hasil:** timer tidak berjalan.
2. Biarkan timer habis. **Hasil:** `HandleTimeUp` terpanggil sekali, dan kelas ini tidak memuat scene.
3. Tekan `Esc` saat timer berjalan. **Hasil:** timer berhenti.
4. Mati pada detik ke-30. **Hasil:** timer berhenti.
5. Pada 10 detik tersisa. **Hasil:** pitch naik, dan vignette menebal.
6. Setor barang lalu berangkat lebih awal. **Hasil:** timer berhenti tanpa `HandleTimeUp`.
