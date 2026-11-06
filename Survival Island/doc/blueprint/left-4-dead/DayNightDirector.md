# Blueprint: DayNightDirector.cs
**Lokasi:** `Survival Island/doc/blueprint/left-4-dead/DayNightDirector.md`
**Deskripsi Utama:** Pemilik jam dunia. Memajukan waktu dan mengirim sinyal pagi serta malam lewat event, tanpa `FindObjectsOfType`. Cahaya ditulis `EnvironmentDirector`.
**Issue:** #56, #63, #66, #67, #80, #92
**Rujukan GDD:** Bagian 3 (siklus siang dan malam, target 60 hari). Kecepatan waktu dan langkah jam adalah usulan teknis, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

`DayNightDirector : MonoBehaviour, ISaveable` adalah singleton di `IslandPhase`, dengan `SaveId = world_clock` dan `LoadOrder = 10`.

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `currentTimeOfDay` | `float` | 6 | Jam 0 sampai 24. Mulai jam 6 pagi |
| `TotalGameHours` | `double` | 6 | Jam sejak run dimulai. Sama dengan `(GameManager.currentDay - 1) * 24 + currentTimeOfDay` |
| `isNight` | `bool` | `false` | Status terakhir |
| `lastHour` | `int` | 6 | Jam terakhir yang sudah dikirim |
| `overrideMultiplier` | `float` | -1 | Nilai di bawah 0 berarti tidak ada override |

**Properti publik:**

| Properti | Isi | Dipakai |
|---|---|---|
| `IsNight` | `isNight` | `SleepSystem`, `RescueEventManager` |
| `IsDay` | `!isNight` | `SunlightExposure` |
| `TotalGameHours` | Lihat tabel di atas | `CampfireManager`, `ItemUseSystem`, `StatusEffectManager`, `RegionalWeather` |
| `BaseTimeMultiplier` | `GameManager.Instance.Balance.time.timeMultiplier` | `Update` |
| `SunRotation` | `Quaternion`, dihitung dari `currentTimeOfDay` | `EnvironmentDirector` |
| `SunDirection` | `Vector3`, dihitung dari `SunRotation` | `SunlightExposure` |

`public float timeMultiplier`, `public Light directionalLight`, dan `lightColor` dihapus. Warna cahaya pindah ke `EnvironmentDirector` (#92).

**Angka penyetelan:** `GameBalanceConfig.time.timeMultiplier`, dan `GameBalanceConfig.sleep.maxGameHoursPerStep` (0,25 jam).

**Event:** bentuknya mengikuti `Core_Logic/GameEventBus.md` (#107).

## 2. Detail Logika & Fungsi

### Memajukan jam

**`void Update()`**:

```csharp
float multiplier = overrideMultiplier > 0f ? overrideMultiplier : BaseTimeMultiplier;
float remaining = (Time.deltaTime * multiplier) / 3600f;
float step = GameManager.Instance.Balance.sleep.maxGameHoursPerStep;
while (remaining > 0f)
{
    AdvanceClock(Mathf.Min(remaining, step));
    remaining -= step;
}
```

**`private void AdvanceClock(float hours)`** memajukan `currentTimeOfDay` dan `TotalGameHours`, lalu memeriksa batas, sesuai tabel di `Island_Logic/SleepSystem.md`:

| Batas | Pemeriksaan | Tindakan |
|---|---|---|
| 24:00 | `currentTimeOfDay >= 24` | `currentTimeOfDay -= 24`, lalu `GameManager.Instance.AdvanceDay()` |
| 18:00 | `currentTimeOfDay >= 18` dan `!isNight` | `isNight = true`, lalu terbitkan `NightfallEvent` |
| 06:00 | `6 <= currentTimeOfDay < 18` dan `isNight` | `isNight = false`, lalu terbitkan `DaybreakEvent` |
| Tiap jam | `Mathf.FloorToInt(currentTimeOfDay) != lastHour` | `lastHour` diperbarui, lalu terbitkan `HourChangedEvent` |

`GameManager` memiliki `currentDay`, dan yang menandai saat hari ke-60 terlewat. Jam terus berjalan setelah hari itu.

### Override kecepatan

- **`public void SetTimeMultiplierOverride(float value)`**: `overrideMultiplier = value`. Dipakai `SleepSystem`.
- **`public void ClearTimeMultiplierOverride()`**: `overrideMultiplier = -1`.

### Arah matahari

`SunRotation = Quaternion.Euler((currentTimeOfDay / 24f) * 360f - 90f, -30f, 0f)`, dan `SunDirection = -(SunRotation * Vector3.forward)`. Keduanya hanya dibaca. `DayNightDirector` tidak menulis `Light`.

### Fungsi pengembangan

**`public void DebugSetTime(float jam)`**, di dalam `#if` pengembangan. Dipanggil `DevConsole` (#56) lewat perintah `settime`.

### Simpan dan muat

- **`CaptureState()`:** `currentTimeOfDay`, `TotalGameHours`, dan `isNight`.
- **`RestoreState(string json)`:** hanya mengisi data, tanpa mengirim event.
- **`OnRestoreFinished()`:** mengirim ulang event sticky.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `DaybreakEvent`, `NightfallEvent`, dan `HourChangedEvent`. Ketiganya sticky.

**Event yang didengar:** tidak ada.

| Sistem | Hubungan |
|---|---|
| `GameManager` | Menerima `AdvanceDay`, dan menyediakan `Balance` |
| `SleepSystem` (#67) | `IsNight`, `TotalGameHours`, `SetTimeMultiplierOverride`, dan `ClearTimeMultiplierOverride` |
| `EnvironmentDirector` (#92) | Membaca `SunRotation`, dan pemilik satu-satunya untuk `Light` |
| `SunlightExposure` (#63) | `IsDay` dan `SunDirection` |
| `WorldStateSave` (#80) | Mendaftarkan `world_clock` |
| `DevConsole` (#56) | `DebugSetTime` |
| `TideManager` (#91) | Membaca `currentTimeOfDay` |

## 4. Kasus Khusus

- **Objek pool:** tidak memakai pool.
- **Game dijeda:** `Time.deltaTime` bernilai 0, jadi jam berhenti.
- **Permadeath:** `world_clock` terhapus bersama save run (#82). Run baru mulai dari jam 6 pagi.
- **Save dan load:** event sticky dikirim ulang di `OnRestoreFinished`, bukan di `RestoreState`. Sistem lain membaca `TotalGameHours` setelah `LoadOrder` 10.
- **Frame sangat panjang:** `while` memecah langkah, jadi batas 18:00, 06:00, dan tiap jam tidak terlewat.
- **Tidur melewati 24:00:** `AdvanceDay` tetap terpanggil tepat sekali per hari, sesuai `SleepSystem.md`.
- **Lewat hari ke-60:** jam dan hari tetap berjalan. Tidak ada Game Over.
- **`AdvanceDay` keluar lebih awal saat `IsRunEnding`:** `TotalGameHours` disimpan sebagai nilai sendiri, jadi tidak mundur walaupun `currentDay` berhenti naik.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** pemilik `currentDay` (`GLOSSARY.md`), `SaveId` dan urutan muat (`WorldStateSave.md`), langkah jam (`SleepSystem.md`), dan cahaya milik `EnvironmentDirector` (`EnvironmentProfile.md`).

**Keputusan teknis di blueprint ini:**

- `TotalGameHours` disimpan dan dimajukan `AdvanceClock`, karena `WorldStateSave.md` menyimpannya dan `AdvanceDay` bisa keluar lebih awal.
- `SunDirection` dihitung dari jam, karena `directionalLight` tidak lagi ada di kelas ini.

**Usulan yang disetujui developer sebagai nilai awal:** tidak ada keputusan baru dari blueprint ini.

## 6. Skenario Tes

1. Mulai run baru. **Hasil:** jam 6, `IsDay` bernilai `true`, dan `TotalGameHours` 6.
2. `settime 17.9`, lalu tunggu. **Hasil:** `NightfallEvent` sekali saat 18:00.
3. `settime 5.9`, lalu tunggu. **Hasil:** `DaybreakEvent` sekali saat 06:00.
4. Tunggu melewati 24:00. **Hasil:** `AdvanceDay` sekali, `currentTimeOfDay` kembali dari 0, dan `TotalGameHours` terus naik.
5. Atur `SetTimeMultiplierOverride` sangat besar. **Hasil:** tiap batas jam tetap terkirim berurutan.
6. Jeda game. **Hasil:** jam berhenti.
7. Simpan saat malam, lalu lanjutkan. **Hasil:** jam dan `TotalGameHours` sama, serta event malam dikirim ulang setelah semua data pulih.
8. Lewati hari ke-60 dengan `setday 60`. **Hasil:** jam terus berjalan, dan `RunEndedEvent` tidak terkirim.
9. Periksa scene. **Hasil:** rotasi `Directional Light` hanya ditulis `EnvironmentDirector`.

Belum diuji di Unity, karena blueprint ini baru rancangan.

## 7. Catatan Implementasi (Langkah 4)

Kode: `Survival Island/Assets/LOGIC/left-4-dead/DayNightDirector.cs`.

- **Event awal run baru:** `Start()` mengirim `DaybreakEvent` (atau `NightfallEvent`) dan `HourChangedEvent` sekali, supaya pendengar sticky punya jam sejak awal. Save yang dimuat menimpanya lewat `OnRestoreFinished`.
- **`DebugSetTime`:** menyamakan `isNight` dengan jam baru dan mengirim ulang event sticky kalau statusnya berubah. Batas 18:00 dan 06:00 berikutnya tetap mengirim event saat dilewati.
- **`ItemUseSystem.CurrentTotalGameHours()`** sekarang memakai `DayNightDirector.Instance.TotalGameHours` kalau ada (hook kecil di file Langkah 3). Rumus Langkah 3 tetap sebagai cadangan.
- Nilai cadangan saat `GameManager.CurrentBalance` kosong: `timeMultiplier` 96 dan `maxGameHoursPerStep` 0,25, sama dengan config.
- Belum diuji di Unity.
