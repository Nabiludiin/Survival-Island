# Blueprint: GameManager.cs
**Lokasi:** `docs/blueprint/Core_Logic/GameManager.md`
**Deskripsi Utama:** Manager persisten yang memegang status run: fase, hari, dan data dari Kapal (Fase 1) ke Pulau (Fase 2). Sistem ini menentukan hasil akhir run, tetapi tidak memuat scene dan tidak menghapus save.
**Issue:** #82, #74, #56, #14
**Rujukan GDD:** Bagian 1 dan 3 (Permadeath, Syarat Menang, batas hari ke-60). `IBootstrapService`, `IsRunEnding`, dan penanda `wasTainted` adalah usulan teknis, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

`GameManager` adalah manager persisten pertama pada urutan bootstrap (`IBootstrapService`, #82). Objeknya berada di bawah `PersistentRoot`, jadi kelas ini tidak memanggil `DontDestroyOnLoad` sendiri. `Awake` hanya mengisi `Instance`.

```csharp
public enum GamePhase { SinkingShip, IslandSurvival }
public enum RunResult { Won, Died, TimeExpired }
```

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Instance` | `GameManager` | `null` | Singleton |
| `currentPhase` | `GamePhase` | `SinkingShip` | Fase aktif. Diubah lewat `SetPhase` |
| `currentDay` | `int` | 1 | Pemilik hari, sesuai `GLOSSARY.md` |
| `hasEscaped` | `bool` | `false` | Helikopter sudah lepas landas |
| `IsRunEnding` | `bool` | `false` | Hasil run sudah ditentukan. Panggilan akhir berikutnya diabaikan |
| `MaxDay` | `const int` | 60 | Batas hari, sesuai GDD |
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
2. `currentDay = 1`, `hasEscaped = false`, `hasArrivedOnIsland = false`, `IsRunEnding = false`, dan `wasTainted = false`.
3. `currentPhase = SinkingShip`.
4. Buat salinan `GameBalanceConfig` yang baru.

Fungsi ini menggantikan `ResetGameAndReturnToShip()`.

**`public void SetPhase(GamePhase phase)`**: isi `currentPhase`, lalu terbitkan `PhaseChangedEvent { phase }`.

**`public void RestoreRunData(GameRunSaveData data)`** dipanggil `RunLifecycle.ContinueRun()`. Mengisi `currentDay`, `hasEscaped`, `hasArrivedOnIsland`, `shipOutcome` (termasuk `lostNpcIds`), dan `wasTainted`, tanpa mengirim event. Pasangannya, `CaptureRunData()`, dipanggil `WorldStateSave` (#80).

### Data dari Fase 1

**`public void SaveShipOutcome(List<SavedItemData> items, ShipOutcomeData outcome)`** dipanggil `LifeboatExtraction` (#74). Mengisi `savedShipLoot` dan `shipOutcome`. Fungsi ini menggantikan `SaveFase1Loot(List<Item>)`.

`IslandArrival.Apply` mengosongkan `savedShipLoot` setelah barang diterapkan, lalu mengatur `hasArrivedOnIsland = true`.

### Hari

**`public void AdvanceDay()`** dipanggil `DayNightDirector` saat jam melewati 24:00:

1. Keluar kalau `IsRunEnding`.
2. Naikkan `currentDay`, lalu terbitkan `DayChangedEvent { newDay }`.
3. Panggil `CheckDay60Milestone()`.

**`public void CheckDay60Milestone()`**: kalau `currentDay > MaxDay` dan `hasEscaped == false`, hari ke-60 sudah berakhir, jadi panggil `TriggerGameOver(DeathReason.TimeExpired)`.

**`DebugSetDay(int day)`**, hanya di dalam `#if UNITY_EDITOR || DEVELOPMENT_BUILD`:

1. Tolak kalau `day` di luar rentang 1 sampai `MaxDay`.
2. `currentDay = day`.
3. Terbitkan `DayChangedEvent` sekali, tanpa `AdvanceDay()` berulang.

### Akhir run

**`public void TriggerGameOver(DeathReason reason)`**:

1. Keluar kalau `IsRunEnding`.
2. `IsRunEnding = true`.
3. Terbitkan `RunEndedEvent`, dengan `RunResult.TimeExpired` untuk `DeathReason.TimeExpired`, dan `RunResult.Died` untuk alasan lain.

**`public void TriggerWinSequence()`**, dipanggil `RescueEventManager` (#14) saat helikopter lepas landas:

1. Keluar kalau `IsRunEnding`.
2. `hasEscaped = true` dan `IsRunEnding = true`.
3. Terbitkan `RunEndedEvent { RunResult.Won, DeathReason.None }`.

`GameManager` mendengar `PlayerDiedEvent` di `OnEnable`, melepasnya di `OnDisable`, lalu memanggil `TriggerGameOver(e.reason)`.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `PhaseChangedEvent`, `DayChangedEvent`, `RunEndedEvent`.

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
- **Hari ke-60 berakhir saat helikopter terbang:** `AdvanceDay` keluar lebih awal karena `IsRunEnding` sudah `true`.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** syarat menang dan batas hari (`GLOSSARY.md`). `DeathReason.None` saat menang (usulan di `RunLifecycle.md`). Alur akhir run tunggal (`RunLifecycle.md`). `SaveShipOutcome` (`LifeboatExtraction.md`).

**Keputusan teknis di blueprint ini:** `GameManager` tidak memuat scene. Penanda `wasTainted` berada di data run (usulan #56).

**Menunggu developer:** tidak ada yang menghalangi. Usulan terkait dicatat di #85 melalui blueprint pemiliknya.

## 6. Skenario Tes

1. Mulai run baru. **Hasil:** `currentDay` 1, `currentPhase = SinkingShip`, dan `PhaseChangedEvent` terkirim sekali.
2. Jalankan `setday 60`, lalu tunggu jam melewati 24:00. **Hasil:** `RunEndedEvent` dengan `TimeExpired`.
3. Panggil `TriggerWinSequence`, lalu turunkan HP ke 0. **Hasil:** hasil tetap `Won`, dengan `DeathReason.None`, dan event terkirim sekali.
4. Mati, lalu periksa data setelah `ResetForNewRun`. **Hasil:** semua nilai kembali ke nilai awal, dan scene tidak berganti dari fungsi ini.
5. Tiba di pulau, keluar, lalu lanjutkan. **Hasil:** `IslandArrival.Apply` tidak berjalan lagi, dan barang tidak berlipat.
6. Jalankan `setday 61`. **Hasil:** ditolak, dan `DayChangedEvent` tidak terkirim.
