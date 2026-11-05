# Blueprint: ShipTimerManager.cs
**Lokasi:** `docs/blueprint/60-seconds/ShipTimerManager.md`
**Deskripsi Utama:** Menghitung mundur Fase 1, menampilkan timer, dan mengatur tensi lewat musik, detak jam, vignette UI, dan getar teks timer. Hasil akhir Fase 1 dan perpindahan scene milik `LifeboatExtraction` (#74).
**Issue:** #74, #82
**Rujukan GDD:** Bagian 3 (Fase 1 selama 60 detik). **Keputusan developer:** 60 detik adalah batas paling lama, dan pemain boleh berangkat lebih awal dengan sekoci. Efek kepanikan, ambang 15 detik, dan tampilan timer adalah usulan yang sudah disetujui sebagai nilai awal (#85).

## 1. Arsitektur & Variabel Utama

### Pola

`ShipTimerManager : MonoBehaviour` adalah singleton per scene di `ShipPhase` (#82). Kelas ini **hanya** menghitung waktu dan mengatur tensi. Kelas ini tidak menyimpan inventori, tidak memuat scene, dan tidak memulai dirinya sendiri.

| Tugas | Pemilik |
|---|---|
| Hitung mundur, teks timer, vignette UI, getar teks, detak jam, musik Fase 1 | Kelas ini |
| Kapan timer mulai dan berhenti, hasil akhir, ringkasan | `LifeboatExtraction` (#74) |
| Alarm, suara besi kapal, air, zombie, getaran kamera | `ShipHazardManager` (#75), `CameraShake` (#24) |
| Efek layar status pemain, seperti tenggelam | `PostProcessController` (#23) |

Pembagian ini mengikuti `CombatMusicDirector.md` (musik Fase 1 milik kelas ini), `CameraShake.md` (getar panel UI milik kelas ini), dan `AmbientAudio.md` (suara kapal milik `ShipHazardManager`).

### Variabel

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Instance` | `ShipTimerManager` | `null` | Singleton per scene. Diisi di `Awake`, dikosongkan di `OnDestroy` |
| `SecondsLeft` | `float` | `ship.durationSeconds` (60) | Sisa waktu. `public get`, `private set`. Dibaca `ShipHazardManager` dan `LifeboatExtraction` |
| `IsRunning` | `bool` | `false` | Timer berjalan. `public get` |
| `IsTimeUp` | `bool` | `false` | Waktu sudah habis. `public get` |
| `lastShownSecond` | `int` | -1 | Angka terakhir di layar. Teks hanya ditulis ulang saat angka berubah |
| `panicFactor` | `float` | 0 | 0 sampai 1, naik di bawah `panicStartSeconds` |
| `timerText` | `TextMeshProUGUI` | Dari scene | Teks timer di HUD |
| `timerRect` | `RectTransform` | Dari scene | Posisi teks, untuk getar |
| `timerBasePos` | `Vector2` | Dari `timerRect` saat `Awake` | Posisi asal teks |
| `vignetteOverlay` | `Image` | Dari scene, alfa 0 | Bingkai gelap UI. Bukan Post Processing (`PostProcessController.md`) |
| `panicCurve` | `AnimationCurve` | Linear 0 ke 1 | Bentuk naiknya kepanikan. Disimpan di prefab, bukan di logika |
| `tickSource` | `AudioSource` | Dari scene, `loop = true`, mati | Detak jam |
| `musicClip` | `AudioClip` | Dari Inspector | Musik Fase 1 |
| `timeUpClip` | `AudioClip` | Dari Inspector | Sirene akhir saat waktu habis |
| `normalColor`, `panicColor` | `Color` | Putih, merah | Warna teks timer |

**Dihapus:** `shipGroanAudio` (suara kapal pindah ke `ShipHazardManager`, sesuai `AmbientAudio.md`), suara air saat waktu habis (milik `FinalShudder` di #75), `savedShipLoot`, `SceneManager.LoadScene`, dan `basePitch` serta `maxPitch` yang tertulis langsung.

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.ship.durationSeconds` | 60 | Sudah ada. Batas waktu paling lama (GDD bagian 3) |
| `GameBalanceConfig.ship.panicStartSeconds` | 15 | Efek kepanikan mulai. Sama dengan tahap air dek tengah di #75 |
| `GameBalanceConfig.ship.tickPitchMin` | 1 | Pitch detak saat tenang |
| `GameBalanceConfig.ship.tickPitchMax` | 2 | Pitch detak pada `T=0` |
| `GameBalanceConfig.ship.tickVolume` | 0,6 | Volume detak |
| `GameBalanceConfig.ship.vignetteMaxAlpha` | 0,55 | Alfa vignette pada `T=0` |
| `GameBalanceConfig.ship.timerShakePixels` | 6 | Jarak getar teks terbesar, dalam piksel referensi Canvas |
| `GameBalanceConfig.ship.timerShakeFrequency` | 25 | Kecepatan getar teks |
| `GameBalanceConfig.ship.musicFadeInSeconds` | 1 | Musik masuk saat timer mulai |
| `GameBalanceConfig.ship.musicFadeOutSeconds` | 0,5 | Musik keluar saat timer berhenti |

Semua angka baru di atas adalah usulan, dan perlu ditambahkan ke `ShipBalance` di `GameBalanceConfig.md`.

## 2. Detail Logika & Fungsi

### Persiapan

**`void Awake()`**:

1. Kalau `Instance` sudah terisi, catat peringatan lalu `Destroy(this)`. Hanya satu per scene.
2. `Instance = this`, dan simpan `timerBasePos`.
3. `SecondsLeft = ship.durationSeconds`, `IsRunning = false`, `IsTimeUp = false`.
4. Panggil `ShowIdle()`.

**`private void ShowIdle()`**: tampilkan angka penuh tanpa berjalan. Ini memenuhi langkah 4 jeda pra-game di `LifeboatExtraction.md` ("timer menampilkan 60, tanpa berjalan").

1. `WriteSeconds(Mathf.CeilToInt(SecondsLeft))`.
2. Warna teks `normalColor`, `vignetteOverlay` alfa 0, dan `timerRect.anchoredPosition = timerBasePos`.
3. `tickSource` mati.

### Mulai dan berhenti

**`public void StartCountdown()`**, dipanggil `LifeboatExtraction.PreGameRoutine` langkah 7:

1. Kalau `IsRunning` atau `IsTimeUp`, abaikan. Timer tidak bisa dimulai dua kali dalam satu scene.
2. Kalau `GameManager.Instance.IsRunEnding`, abaikan.
3. `SecondsLeft = ship.durationSeconds`, lalu `IsRunning = true`.
4. `tickSource.pitch = tickPitchMin`, `tickSource.volume = tickVolume`, lalu `tickSource.Play()`.
5. `AudioManager.Instance.PlayMusicClip(musicClip, musicFadeInSeconds)`.

**`public void StopCountdown()`**, dipanggil `LifeboatExtraction.RequestDepart` dan `HandleRunEnded`:

1. Kalau `!IsRunning`, abaikan. Fungsi ini aman dipanggil berulang.
2. `IsRunning = false`.
3. `tickSource.Stop()`.
4. `AudioManager.Instance.PlayMusicClip(null, musicFadeOutSeconds)`. Musik memudar.
5. Teks timer berhenti pada angka terakhir. Vignette dan getar dibiarkan pada nilai terakhir, karena panel `RunSummary` atau layar mati segera menutupinya.

`SecondsLeft` **tidak** direset saat berhenti. `LifeboatExtraction.ResolveOutcome` membacanya untuk `secondsLeftOnExit`.

### Per frame

**`void Update()`**:

```csharp
if (!IsRunning || IsTimeUp) return;
if (GameManager.Instance.IsRunEnding) { StopCountdown(); return; }   // pengaman kalau RunEndedEvent terlambat

SecondsLeft = Mathf.Max(0f, SecondsLeft - Time.deltaTime);
UpdateDisplay();
UpdatePanic();
if (SecondsLeft <= 0f) ExecuteTimeUp();
```

`Time.deltaTime` dibatasi `Time.maximumDeltaTime` Unity (0,333 detik), jadi satu frame panjang tidak menghabiskan banyak waktu sekaligus. `ShipHazardManager` tetap memproses semua entri jadwal yang terlewat dengan `while`.

**`private void UpdateDisplay()`**:

1. `int shown = Mathf.CeilToInt(SecondsLeft)`. Pemain melihat `60` sampai `1`, lalu `0` hanya saat waktu habis.
2. Kalau `shown != lastShownSecond`, panggil `WriteSeconds(shown)`.

**`private void WriteSeconds(int value)`**: `timerText.SetText("{0}", value)`, lalu `lastShownSecond = value`. `SetText` dengan argumen angka tidak membuat string baru, jadi tidak ada garbage per detik.

**`private void UpdatePanic()`**:

```csharp
float start = balance.ship.panicStartSeconds;
float raw = SecondsLeft >= start ? 0f : 1f - SecondsLeft / start;
panicFactor = panicCurve.Evaluate(raw);

tickSource.pitch = Mathf.Lerp(tickPitchMin, tickPitchMax, panicFactor);
SetVignetteAlpha(panicFactor * vignetteMaxAlpha);
timerText.color = Color.Lerp(normalColor, panicColor, panicFactor);

float shakeScale = LocalizationManager.Instance.Accessibility.cameraShakeScale;
float amp = panicFactor * timerShakePixels * shakeScale;
float t = Time.time * timerShakeFrequency;
timerRect.anchoredPosition = timerBasePos + new Vector2(
    (Mathf.PerlinNoise(t, 0f) - 0.5f) * 2f * amp,
    (Mathf.PerlinNoise(0f, t) - 0.5f) * 2f * amp);
```

- **Di atas 15 detik:** `panicFactor` 0. Detak tetap ada dengan pitch 1, supaya tekanan waktu terasa sejak awal.
- **Pada 10 detik:** `raw` sekitar 0,33. Pitch sekitar 1,33, dan vignette mulai terlihat.
- **Pada 0 detik:** pitch 2, vignette 0,55, dan teks merah penuh.
- **Getar teks** memakai `Time.time`, jadi berhenti saat game dijeda. Getar ikut pengaturan `cameraShakeScale` (#104), sehingga pemain yang mematikan getaran kamera juga tidak melihat teks bergetar.
- **`SetVignetteAlpha`** mengubah `vignetteOverlay.color` hanya kalau selisihnya lebih dari 0,005, supaya Canvas tidak dibangun ulang tiap frame.

### Waktu habis

**`private void ExecuteTimeUp()`**:

1. `IsTimeUp = true`, `IsRunning = false`, dan `SecondsLeft = 0`.
2. `WriteSeconds(0)`.
3. `tickSource.Stop()`, lalu `AudioManager.Instance.PlaySFX(timeUpClip)`.
4. `AudioManager.Instance.PlayMusicClip(null, musicFadeOutSeconds)`.
5. `LifeboatExtraction.Instance.HandleTimeUp()`.

`LifeboatExtraction` menentukan hasil: aman berarti ringkasan, tidak aman berarti `DeathReason.LeftOnShip` (#74). Getaran terakhir `FinalShudder` dijalankan `ShipHazardManager` dari jadwalnya sendiri pada `T=0`, bukan dari fungsi ini.

### Berhenti saat run berakhir

```csharp
void OnEnable()  { EventChannel<RunEndedEvent>.Raised += HandleRunEnded; }
void OnDisable() { EventChannel<RunEndedEvent>.Raised -= HandleRunEnded; }
void HandleRunEnded(RunEndedEvent e) { StopCountdown(); }
```

### Alat uji (usulan)

**`public void DebugSetSecondsLeft(float value)`**, hanya di `#if UNITY_EDITOR || DEVELOPMENT_BUILD`: `SecondsLeft = Mathf.Clamp(value, 0, ship.durationSeconds)`. Perintah konsol `shiptime <detik>` (#56) memanggilnya, supaya tes tahap 15 detik dan waktu habis tidak perlu menunggu satu menit penuh.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** tidak ada. Sistem lain membaca `SecondsLeft` langsung, karena nilainya berubah tiap frame dan pembacanya sudah bergantung pada kelas ini.

**Event yang didengar:** `RunEndedEvent`.

| Sistem | Hubungan |
|---|---|
| `LifeboatExtraction` (#74) | Memanggil `StartCountdown` dan `StopCountdown`, membaca `SecondsLeft`, dan menerima `HandleTimeUp` |
| `ShipHazardManager` (#75) | Membaca `SecondsLeft` untuk jadwal dan tinggi air |
| `GameManager` | `IsRunEnding`, dan `RunEndedEvent` |
| `AudioManager` | `PlayMusicClip` untuk musik Fase 1, dan `PlaySFX` untuk sirene akhir |
| `CombatMusicDirector` (#114) | Tidak aktif di Fase 1. Musik Fase 1 milik kelas ini |
| `CameraShake` (#24) | Getaran kamera. Kelas ini hanya menggetarkan teks UI |
| `PostProcessController` (#23) | Efek layar status. `vignetteOverlay` tetap UI milik kelas ini |
| `LocalizationManager` (#104) | `Accessibility.cameraShakeScale` |
| `RunLifecycle` (#82) | Dipanggil `LifeboatExtraction`, bukan kelas ini |
| `DevConsole` (#56) | Perintah `shiptime` (usulan) |

## 4. Kasus Khusus

- **Objek pool:** tidak memakai pool. Satu objek per scene.
- **Game dijeda:** `Time.deltaTime` dan `Time.time` bernilai 0, jadi timer, pitch, vignette, dan getar berhenti. `tickSource` ikut berhenti karena `AudioListener.pause` mengikuti `UIFocusStack`, dan sumber ini tidak memakai `ignoreListenerPause` (`AudioManager.md`). Musik memakai `bgmSource` milik `AudioManager`, yang tetap terdengar pelan lewat snapshot `Paused`.
- **Panel `Cinematic` di awal Fase 1:** timer belum mulai, karena `StartCountdown` baru dipanggil setelah jeda pra-game.
- **Permadeath:** kelas ini hilang bersama `ShipPhase`. `Instance` dikosongkan di `OnDestroy`, dan run baru mulai dari `Awake` dengan 60 detik penuh.
- **Save dan load:** Fase 1 tidak punya save (#82). Keluar ke menu dari kapal membuang run (#74).
- **Mati sebelum waktu habis:** `RunEndedEvent` memanggil `StopCountdown`, dan `HandleTimeUp` tidak terpanggil. Kalau event terlambat satu frame, pemeriksaan `IsRunEnding` di `Update` menahan timer.
- **Mati pada frame yang sama dengan waktu habis:** `HandleTimeUp` mengabaikan karena `IsRunEnding` (#74).
- **Berangkat lebih awal:** `LifeboatExtraction` memanggil `StopCountdown`. `SecondsLeft` tetap, dan ringkasan menampilkannya. Keputusan developer: 60 detik adalah batas paling lama.
- **`StartCountdown` dipanggil dua kali:** yang kedua diabaikan.
- **`StopCountdown` sebelum `StartCountdown`:** diabaikan.
- **Frame sangat panjang di akhir:** `SecondsLeft` dibatasi di 0, dan `ExecuteTimeUp` hanya berjalan sekali karena `IsTimeUp`.
- **`LifeboatExtraction.Instance` kosong:** catat error di `ExecuteTimeUp`. Ini kesalahan susunan scene, dan `ShipPhase` wajib punya sekoci.
- **Teks berubah ukuran karena `textScale` (#104):** getar memakai `anchoredPosition`, jadi tetap berpusat di posisi asal.
- **`DeathCinematic` memperlambat waktu:** timer sudah berhenti karena `RunEndedEvent`.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** alur Fase 1, jeda pra-game, dan pemanggilan `EnterIslandPhase` (`LifeboatExtraction.md`). Durasi 60 detik sebagai batas paling lama, dan berangkat lebih awal (GDD bagian 3, keputusan developer #85). Musik Fase 1 milik kelas ini (`CombatMusicDirector.md`). Getar UI milik kelas ini (`CameraShake.md`). Suara kapal milik `ShipHazardManager` (`AmbientAudio.md`).

**Keputusan teknis di blueprint ini:**

- Timer tidak mengirim event per detik. Pembaca memakai `SecondsLeft` langsung.
- Angka di layar memakai pembulatan ke atas, jadi `0` hanya tampil saat waktu benar-benar habis.
- Teks ditulis ulang hanya saat angka berubah, tanpa garbage.

**Usulan baru (menunggu persetujuan developer):**

1. Detak jam terdengar sejak timer mulai, lalu makin cepat di bawah 15 detik.
2. Getar teks timer ikut pengaturan `cameraShakeScale`.
3. Angka penyetelan baru di `GameBalanceConfig.ship`: `panicStartSeconds`, `tickPitchMin`, `tickPitchMax`, `tickVolume`, `vignetteMaxAlpha`, `timerShakePixels`, `timerShakeFrequency`, `musicFadeInSeconds`, dan `musicFadeOutSeconds`.
4. Perintah konsol `shiptime <detik>` untuk pengujian.

**Menunggu developer:** tidak ada yang menghalangi.

## 6. Skenario Tes

1. Masuk `ShipPhase`, lalu tunggu selama jeda pra-game. **Hasil:** teks `60`, tidak berkurang, dan detak belum terdengar.
2. Tunggu kontrol dibuka. **Hasil:** detak dan musik mulai, lalu teks turun satu per detik.
3. Panggil `StartCountdown` sekali lagi lewat Inspector. **Hasil:** diabaikan, dan waktu tidak kembali ke 60.
4. Biarkan timer habis di area aman. **Hasil:** teks `0`, sirene akhir berbunyi, `HandleTimeUp` terpanggil sekali, dan kelas ini tidak memuat scene.
5. Jalankan `shiptime 16`, lalu amati. **Hasil:** di atas 15 tidak ada vignette. Setelah 15, pitch naik, vignette menebal, teks memerah, dan teks bergetar.
6. Pada 10 detik tersisa. **Hasil:** pitch sekitar 1,33, dan vignette terlihat jelas.
7. Atur `cameraShakeScale` ke 0, lalu ulangi tes 5. **Hasil:** teks tidak bergetar, tetapi warna dan vignette tetap berubah.
8. Tekan `Esc` pada 20 detik selama 10 detik. **Hasil:** teks, detak, getar, dan vignette berhenti. Setelah ditutup, lanjut dari 20.
9. Mati pada detik ke-30. **Hasil:** timer berhenti, musik memudar, dan `HandleTimeUp` tidak terpanggil.
10. Setor barang, lalu tahan `E` di sekoci pada 40 detik. **Hasil:** timer berhenti tanpa `HandleTimeUp`, dan ringkasan menampilkan sisa 40 detik.
11. Turunkan HP ke 0 pada `T=0,1`. **Hasil:** alur mati berjalan sekali, tanpa ringkasan.
12. Buka Profiler selama 60 detik. **Hasil:** tidak ada alokasi GC dari `Update` kelas ini.
13. Paksa frame lambat 0,5 detik di akhir lewat `Debug.Break`. **Hasil:** `ExecuteTimeUp` tetap sekali, dan `SecondsLeft` tidak negatif.
14. Ganti `ship.durationSeconds` ke 30 di config. **Hasil:** timer mulai dari 30 tanpa mengubah kode.
15. Mati lalu ulangi dari kapal tiga kali. **Hasil:** tiap run dimulai dari 60, tanpa sisa vignette atau warna merah.
