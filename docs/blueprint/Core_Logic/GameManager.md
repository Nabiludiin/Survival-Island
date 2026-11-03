# Blueprint: GameManager.cs
**Lokasi:** `docs/blueprint/Core_Logic/GameManager.md`
**Deskripsi Utama:** Manager persisten yang memegang status run: fase, hari, dan data dari Kapal (Fase 1) ke Pulau (Fase 2). Sistem ini menentukan hasil akhir run, tetapi tidak memuat scene dan tidak menghapus save.
**Issue:** #82, #74, #56, #14
**Rujukan GDD:** Bagian 1 dan 3 (Permadeath, Syarat Menang, target hari ke-60). `IBootstrapService`, `IsRunEnding`, dan penanda `wasTainted` adalah usulan teknis, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

`GameManager` adalah manager persisten pertama pada urutan bootstrap (`IBootstrapService`, #82). Objeknya berada di bawah `PersistentRoot`, jadi kelas ini tidak memanggil `DontDestroyOnLoad` sendiri. `Awake` hanya mengisi `Instance`.

```csharp
public enum GamePhase { SinkingShip, IslandSurvival }
public enum RunResult { Won, Died }
```

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Instance` | `GameManager` | `null` | Singleton |
| `currentPhase` | `GamePhase` | `SinkingShip` | Fase aktif. Diubah lewat `SetPhase` |
| `currentDay` | `int` | 1 | Pemilik hari, sesuai `GLOSSARY.md` |
| `hasEscaped` | `bool` | `false` | Helikopter sudah lepas landas |
| `IsRunEnding` | `bool` | `false` | Hasil run sudah ditentukan. Panggilan akhir berikutnya diabaikan |
| `TargetDay` | `const int` | 60 | Target hari, sesuai GDD. Bukan batas, karena `currentDay` tidak punya batas atas |
| `IsPastTargetDay` | `bool` | `false` | Properti `currentDay > TargetDay` |
| `dayLimitPassedSent` | `bool` | `false` | `DayLimitPassedEvent` sudah terkirim pada run ini. Tidak disimpan, karena dihitung ulang dari `currentDay` |
| `savedShipLoot` | `List<SavedItemData>` | Kosong | Barang dari Fase 1. Dikosongkan setelah `IslandArrival.Apply` |
| `shipOutcome` | `ShipOutcomeData` | Kosong | Hasil Fase 1: NPC selamat, korban, HP, infeksi, dan sisa waktu (#74) |
| `hasArrivedOnIsland` | `bool` | `false` | `IslandArrival.Apply` sudah berjalan (#74) |
| `wasTainted` | `bool` | `false` | Run memakai perintah curang `DevConsole` (#56) |
| `Balance` | `GameBalanceConfig` | Salinan aset | Salinan config untuk run ini (#84) |

## 2. Detail Logika & Fungsi

### Layanan dan reset

**`public void Initialize()`** membuat salinan `GameBalanceConfig`.

**`public void ResetForNewRun()`** hanya mengubah data, dan tidak memuat scene. Pemuatan scene milik `RunLifecycle`:

1. Kosongkan `savedShipLoot` dan `shipOutcome`.
2. `currentDay = 1`, `hasEscaped = false`, `hasArrivedOnIsland = false`, `IsRunEnding = false`, `wasTainted = false`, dan `dayLimitPassedSent = false`.
3. `currentPhase = SinkingShip`.
4. Buat salinan `GameBalanceConfig` yang baru.

Fungsi ini menggantikan `ResetGameAndReturnToShip()`.

**`public void SetPhase(GamePhase phase)`**: isi `currentPhase`, lalu terbitkan `PhaseChangedEvent { phase }`.

**`public void RestoreRunData(GameRunSaveData data)`** dipanggil `RunLifecycle.ContinueRun()`. Mengisi `currentDay`, `hasEscaped`, `hasArrivedOnIsland`, `shipOutcome` (termasuk `lostNpcIds`), dan `wasTainted`, tanpa mengirim event. `dayLimitPassedSent` diisi dari `IsPastTargetDay`, jadi event tidak terkirim dua kali setelah load. Sistem yang perlu status ini setelah load membaca `IsPastTargetDay`. Pasangannya, `CaptureRunData()`, dipanggil `WorldStateSave` (#80).

### Data dari Fase 1

**`public void SaveShipOutcome(List<SavedItemData> items, ShipOutcomeData outcome)`** dipanggil `LifeboatExtraction` (#74). Mengisi `savedShipLoot` dan `shipOutcome`. Fungsi ini menggantikan `SaveFase1Loot(List<Item>)`.

`IslandArrival.Apply` mengosongkan `savedShipLoot` setelah barang diterapkan, lalu mengatur `hasArrivedOnIsland = true`.

### Hari

**`public void AdvanceDay()`** dipanggil `DayNightDirector` saat jam melewati 24:00:

1. Keluar kalau `IsRunEnding`.
2. Naikkan `currentDay`, lalu terbitkan `DayChangedEvent { newDay }`.
3. Panggil `CheckDay60Milestone()`.

**`public void CheckDay60Milestone()`**: kalau `IsPastTargetDay` dan `dayLimitPassedSent == false`, isi `dayLimitPassedSent = true`, lalu terbitkan `DayLimitPassedEvent { day = currentDay }`. Pada alur normal, ini terjadi saat `currentDay` pertama kali menjadi `TargetDay + 1`. Fungsi ini tidak mengakhiri run, sesuai GDD bagian 3. Kekuatan zombie setelah hari ke-60 dihitung `AIDirector` (#27).

**`DebugSetDay(int day)`**, hanya di dalam `#if UNITY_EDITOR || DEVELOPMENT_BUILD`:

1. Tolak kalau `day < 1`. Tidak ada batas atas.
2. `currentDay = day`.
3. Terbitkan `DayChangedEvent` sekali, tanpa `AdvanceDay()` berulang.
4. Panggil `CheckDay60Milestone()`.

### Akhir run

**`public void TriggerGameOver(DeathReason reason)`**:

1. Keluar kalau `IsRunEnding`.
2. `IsRunEnding = true`.
3. Terbitkan `RunEndedEvent { RunResult.Died, reason }`.

**`public void TriggerWinSequence()`**, dipanggil `RescueEventManager` (#14) saat helikopter lepas landas:

1. Keluar kalau `IsRunEnding`.
2. `hasEscaped = true` dan `IsRunEnding = true`.
3. Terbitkan `RunEndedEvent { RunResult.Won, DeathReason.None }`.

`GameManager` mendengar `PlayerDiedEvent` di `OnEnable`, melepasnya di `OnDisable`, lalu memanggil `TriggerGameOver(e.reason)`.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `PhaseChangedEvent`, `DayChangedEvent`, `DayLimitPassedEvent`, `RunEndedEvent`.

**Event yang didengar:** `PlayerDiedEvent`.

| Sistem | Hubungan |
|---|---|
| `RunLifecycle` (#82) | Memanggil `Initialize`, `ResetForNewRun`, `SetPhase`, dan `RestoreRunData`. Mendengar `RunEndedEvent` |
| `DayNightDirector` | Memanggil `AdvanceDay` |
| `LifeboatExtraction`, `IslandArrival` (#74) | Mengisi dan membaca data Fase 1 |
| `RescueEventManager` (#14) | Memanggil `TriggerWinSequence` |
| `WorldStateSave` (#80) | Menyimpan dan memulihkan data run |
| `DevConsole` (#56) | Memanggil `DebugSetDay` dan mengisi `wasTainted` |

## 4. Kasus Khusus

- **Objek pool:** tidak memakai pool.
- **Game dijeda:** tidak ada logika per frame. `TriggerGameOver` tetap berlaku kalau dipanggil saat panel terbuka.
- **Permadeath:** semua data run kembali ke nilai awal lewat `ResetForNewRun`. Save dihapus oleh `RunLifecycle`.
- **Save dan load:** yang disimpan adalah `currentDay`, `hasEscaped`, `hasArrivedOnIsland`, `shipOutcome` dengan `lostNpcIds`, dan `wasTainted`. `savedShipLoot` sudah kosong saat save pertama, jadi barang tidak berlipat. `IsRunEnding` tidak disimpan.
- **Mati dan menang pada frame yang sama:** panggilan pertama menang, dan panggilan berikutnya diabaikan `IsRunEnding`.
- **Hari ke-60 terlewat saat helikopter terbang:** `AdvanceDay` keluar lebih awal karena `IsRunEnding` sudah `true`, jadi `DayLimitPassedEvent` tidak terkirim.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** syarat menang dan target hari (`GLOSSARY.md`). `DeathReason.None` saat menang (usulan di `RunLifecycle.md`). Alur akhir run tunggal (`RunLifecycle.md`). `SaveShipOutcome` (`LifeboatExtraction.md`).

**Keputusan teknis di blueprint ini:** `GameManager` tidak memuat scene. Penanda `wasTainted` berada di data run (usulan #56).

**Menunggu developer:** tidak ada yang menghalangi. Usulan terkait dicatat di #85 melalui blueprint pemiliknya.

## 6. Skenario Tes

1. Mulai run baru. **Hasil:** `currentDay` 1, `currentPhase = SinkingShip`, dan `PhaseChangedEvent` terkirim sekali.
2. Jalankan `setday 60`, lalu tunggu jam melewati 24:00. **Hasil:** `currentDay` 61, `IsPastTargetDay` bernilai `true`, `DayLimitPassedEvent` terkirim sekali, dan `RunEndedEvent` tidak terkirim.
3. Panggil `TriggerWinSequence`, lalu turunkan HP ke 0. **Hasil:** hasil tetap `Won`, dengan `DeathReason.None`, dan event terkirim sekali.
4. Mati, lalu periksa data setelah `ResetForNewRun`. **Hasil:** semua nilai kembali ke nilai awal, dan scene tidak berganti dari fungsi ini.
5. Tiba di pulau, keluar, lalu lanjutkan. **Hasil:** `IslandArrival.Apply` tidak berjalan lagi, dan barang tidak berlipat.
6. Jalankan `setday 61` pada run baru. **Hasil:** diterima, dan `DayChangedEvent` serta `DayLimitPassedEvent` masing-masing terkirim sekali. Jalankan `setday 0`. **Hasil:** ditolak.
7. Tunggu sampai hari ke-62, simpan, lalu lanjutkan. **Hasil:** `IsPastTargetDay` tetap `true`, dan `DayLimitPassedEvent` tidak terkirim lagi.
8. Pada hari ke-75, panggil `TriggerWinSequence`. **Hasil:** `RunEndedEvent` bernilai `Won`.
