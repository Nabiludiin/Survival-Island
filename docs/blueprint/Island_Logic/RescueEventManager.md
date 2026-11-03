# Blueprint: RescueEventManager.cs
**Lokasi:** `docs/blueprint/Island_Logic/RescueEventManager.md`
**Deskripsi Utama:** Pemilik syarat kemenangan: perakitan helikopter dari suku cadang, urutan lepas landas, dan tekanan pada hari target.
**Issue:** #14
**Rujukan GDD:** Bagian 1, bagian 3 (Syarat Menang), dan bagian 5 Region 3 dan 4. Pemanasan mesin dan gelombang hari ke-60 adalah usulan, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

| Kelas | Bentuk | Letak | Fungsi |
|---|---|---|---|
| `RescueEventManager` | Singleton, `ISaveable` (`SaveId = world_helicopter`, `LoadOrder = 100`) | Scene `IslandPhase`, selalu aktif | Status perakitan dan lepas landas |
| `HelicopterFrame` | `MonoBehaviour` di `RegionSceneRoot` | `Region3_Snow` | Model kerangka. Menampilkan bagian sesuai progres |
| `PartInstallZone` | Trigger layer `ZoneTrigger` | Anak `HelicopterFrame` | Menerima suku cadang lewat tag `HeavyItem`, tanpa tombol `E` |
| `CockpitInteractable` | `IslandInteractable` | Anak `HelicopterFrame` | Menyalakan mesin dan lepas landas dengan `E` |

```csharp
public enum RescueState { Assembling, Ready, WarmingUp, ReadyToLift, Departed }
```

| Variabel | Tipe | Nilai awal | Disimpan | Fungsi |
|---|---|---|---|---|
| `State` | `RescueState` | `Assembling` | Ya, lihat bagian 4 | Status saat ini |
| `installedPartIds` | `List<string>` | Kosong | Ya | Salinan yang disamakan dengan `HeavyItemSystem.records` |
| `warmupLeft` | `float` | 0 | Tidak | Sisa pemanasan |
| `targetDayAnnounced` | `bool` | `false` | Ya | Pesan hari ke-60 hanya sekali |
| `targetWaveSent` | `bool` | `false` | Ya | Gelombang malam hari ke-60 hanya sekali |

**Properti:** `int InstalledCount`, `int TotalParts => GameBalanceConfig.region.helicopterPartCount` (5), dan `bool IsDeparted`.

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.region.helicopterPartCount` | 5 | Sudah ada |
| `GameBalanceConfig.rescue.engineWarmupSeconds` | 120 | Lama pemanasan. Setara sekitar 3,2 jam game |
| `GameBalanceConfig.rescue.engineNoiseRadius` | 60 | Radius suara mesin, dikirim tiap 5 detik |
| `GameBalanceConfig.rescue.engineNoiseIntervalSeconds` | 5 | Jeda suara mesin |
| `GameBalanceConfig.rescue.warmupPanicWaveSize` | 15 | Ukuran gelombang kalau mesin dinyalakan saat malam |
| `GameBalanceConfig.rescue.targetNightWaveSize` | 25 | Gelombang malam hari ke-60 |
| `GameBalanceConfig.rescue.abortDistance` | 40 | Jarak dari kokpit yang membatalkan pemanasan |

## 2. Detail Logika & Fungsi

### Perakitan

**`PartInstallZone.OnTriggerEnter(Collider other)`**: kalau `other.CompareTag("HeavyItem")`, ambil `HeavyItem`, lalu panggil `RescueEventManager.Instance.InstallPart(partId)`. Trigger ini tidak memakai raycast, jadi aturan `QueryTriggerInteraction.Ignore` tidak terlibat.

**`public void InstallPart(string partId)`**:

1. Keluar kalau `partId` sudah ada di `installedPartIds`, atau `State != Assembling`.
2. `HeavyItemSystem.Instance.MarkInstalled(partId)`. Barang yang dibawa dilepas lebih dulu oleh fungsi itu.
3. Tambah ke `installedPartIds`.
4. Terbitkan `HelicopterPartInstalledEvent { partId, installed, total }`. `HelicopterFrame` menampilkan bagiannya, dan HUD menulis `Suku cadang n / total`.
5. Terbitkan `SaveRequestedEvent` dengan alasan `PartInstalled`, supaya perjalanan panjang tidak hilang.
6. Kalau `installed >= total`: `State = Ready`, lalu terbitkan `HelicopterReadyEvent`.

Urutan pasang bebas. Pemasangan langsung selesai saat barang tiba, tanpa bahan tambahan. Alasannya, tantangan GDD ada pada perjalanan dari Region 4 ke Region 3.

**Barang tergeletak di dalam trigger.** Trigger tidak terpicu kalau `Region3_Snow` baru dimuat dan barang sudah di dalamnya. `HelicopterFrame.OnEnable` memanggil `ReconcileLyingParts()`: baca `HeavyItemSystem.records`, lalu pasang tiap barang `Lying` yang jaraknya dari `PartInstallZone.center` kurang dari radius zona. Fungsi ini membaca data, tanpa query fisika.

**`HelicopterFrame.OnEnable`** juga membaca `EventChannel<HelicopterPartInstalledEvent>.Last` dan `installedPartIds`, karena region bisa dilepas (#88). Data progres selalu berada di `IslandPhase`, dan model kerangka membacanya saat `Region3_Snow` berstatus `Loaded`.

### Urutan lepas landas

**`CockpitInteractable`** menampilkan prompt sesuai state:

| State | Prompt | Saat `E` |
|---|---|---|
| `Assembling` | `Suku cadang kurang (n / total)` | Ditolak |
| `Ready` | `Nyalakan mesin` | `StartEngine()` |
| `WarmingUp` | `Mesin memanas: m:ss` | Ditolak |
| `ReadyToLift` | `Lepas landas` | `LiftOff()` |

**`StartEngine()`**:

1. `State = WarmingUp`, dan `warmupLeft = engineWarmupSeconds`.
2. Terbitkan `HelicopterEngineChangedEvent { state, secondsLeft }`. `CombatMusicDirector` (#114), HUD, dan `CameraShake` (#24) mendengarnya.
3. Kalau `DayNightDirector.Instance.IsNight`: `AIDirector.Instance.RequestPanicWave(warmupPanicWaveSize, "helicopter_engine")`. Saat siang tidak ada zombie di luar ruangan, sesuai GDD, jadi gelombang tidak dikirim. Ancaman siang di luar ruangan hanya hewan buas yang tertarik suara mesin.

**`Update()` saat `WarmingUp`** memakai `Time.deltaTime`:

1. `warmupLeft -= Time.deltaTime`.
2. Tiap `engineNoiseIntervalSeconds`: panggil `NoiseEmitter.Emit(engineNoiseRadius, NoiseSourceKind.Structure)` dari komponen di `HelicopterFrame` (#49).
3. Terbitkan `HelicopterEngineChangedEvent` hanya saat detik bulatnya berubah.
4. Kalau pemain lebih jauh dari `abortDistance`: `State = Ready`, kirim notifikasi `Mesin mati`, lalu terbitkan event. Ini menjaga tensi ala finale L4D, yaitu pemain bertahan di dekat helikopter.
5. Kalau `warmupLeft <= 0`: `State = ReadyToLift`, lalu terbitkan event.

**`LiftOff()`**, langkahnya harus berurutan pada satu frame:

1. Keluar kalau `GameManager.IsRunEnding`.
2. `State = Departed`.
3. `NarrativeSequencer.Instance.PrepareEnding(BuildEndingContext())` (#99).
4. `GameManager.Instance.TriggerWinSequence()`. Fungsi ini mengisi `hasEscaped = true` dan menerbitkan `RunEndedEvent { Won, DeathReason.None }`. `RunLifecycle` melanjutkan alur akhir (#82).

**`EndingContext BuildEndingContext()`** berisi `aliveNpcCount` dari `NPCRoster.GetAliveNpcIds()` (#76), `lostOnShipCount` dari `GameManager.shipOutcome.lostNpcIds` (#74), `rescuedCount`, dan `day`. Semua NPC yang masih hidup dianggap ikut terbang, dari mana pun posisinya.

### Hari target

GDD tidak meminta event paksa di hari ke-60. Hari ke-60 adalah target, dan melewatinya membuat zombie semakin kuat (`AIDirector.md`). Tekanan di bagian ini adalah usulan yang tidak mengubah syarat menang.

**`HandleDayChanged(DayChangedEvent e)`**: kalau `e.newDay == GameManager.TargetDay` dan `targetDayAnnounced == false`:

1. Kirim notifikasi kritis `Hari ke-60. Setelah malam ini, zombie akan semakin kuat` (#22).
2. `targetDayAnnounced = true`.

**`HandleNightfall(NightfallEvent e)`**: kalau `e.day == GameManager.TargetDay`, `State != Departed`, dan `targetWaveSent == false`: `AIDirector.RequestPanicWave(targetNightWaveSize, "target_night")`, lalu `targetWaveSent = true`.

**`HandleDayLimitPassed(DayLimitPassedEvent e)`**: kirim notifikasi kritis `Hari ke-60 terlewat. Zombie semakin kuat, tetapi helikopter masih bisa kabur` (#22).

**Tidak ada batas waktu kabur.** Perakitan, pemanasan, dan lepas landas tetap berjalan setelah hari ke-60, sesuai GDD bagian 3.

### Perintah `DevConsole` (#56)

| Perintah | Hasil |
|---|---|
| `heli status` | State, bagian terpasang, dan sisa pemanasan |
| `heli install <partId/all>` | Memanggil `InstallPart` |
| `heli warmup <detik>` | Mengisi `warmupLeft` |
| `heli liftoff` | Memanggil `LiftOff`, tanpa syarat state |

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `HelicopterPartInstalledEvent`, `HelicopterReadyEvent`, `HelicopterEngineChangedEvent` (baru), `NoiseEmittedEvent` (lewat `NoiseEmitter`), `SaveRequestedEvent`.

**Event yang didengar:** `DayChangedEvent`, `DayLimitPassedEvent`, `NightfallEvent`, `RunEndedEvent`.

| Sistem | Hubungan |
|---|---|
| `HeavyItemSystem` (#69) | `MarkInstalled`, dan `records` sebagai sumber status barang |
| `GameManager` | `TriggerWinSequence`, `IsRunEnding`, dan `shipOutcome` |
| `RunLifecycle` (#82) | Melanjutkan setelah `RunEndedEvent` |
| `NarrativeSequencer` (#99) | `PrepareEnding`, adegan lepas landas, dan ending |
| `ObjectiveManager` (#81) | Mendengar event helikopter untuk `obj_parts` dan `obj_takeoff` |
| `AIDirector` (#27) | `RequestPanicWave` |
| `NPCRoster` (#76) | `GetAliveNpcIds` |
| `ProgressionSafety` (#116) | Membaca `InstalledCount` dan `State` |
| `BuildMenu` (#79) | `NoBuildVolume` di helipad dan titik pasang |
| `RegionStreaming` (#88) | Model mengikuti progres saat region masuk |

**Batas pekerjaan:** barang dan perjalanannya di #69, isi adegan di #99, dan alur akhir run di #82.

## 4. Kasus Khusus

- **Objek pool:** sistem ini tidak memakai pool. VFX pemasangan lewat `VFXManager` (#15).
- **Game dijeda:** `warmupLeft` memakai `Time.deltaTime`, jadi berhenti saat jeda. Event yang sudah dikirim tetap berlaku.
- **Save:** `CaptureState` menulis `installedPartIds`, `targetDayAnnounced`, `targetWaveSent`, dan state. `WarmingUp` serta `ReadyToLift` disimpan sebagai `Ready`, jadi pemanasan mengulang setelah load dan tidak bisa dipakai untuk menghindari gelombang. `RestoreState` tidak mengirim event.
- **Setelah load:** `OnRestoreFinished` menyamakan `installedPartIds` dengan `HeavyItemSystem.records` (`records` yang menang), lalu mengirim ulang `HelicopterPartInstalledEvent` dan `HelicopterReadyEvent`. `LoadOrder` 100 sama dengan `world_parts`. Ini aman, karena `FinishRestore` memanggil `OnRestoreFinished` setelah semua `RestoreState` selesai (#80). Yang disimpan adalah bagian yang sudah terpasang.
- **Permadeath:** `world_helicopter` terhapus bersama save run. Manager hidup di scene `IslandPhase`, jadi kembali kosong saat run baru.
- **Mati saat pemanasan:** alur mati biasa. Mesin berhenti karena `RunEndedEvent` menonaktifkan `Update`.
- **Mati dan lepas landas pada frame yang sama:** panggilan pertama ke `GameManager` menang (#82).
- **Tengah malam hari ke-60 saat `WarmingUp` atau `ReadyToLift`:** state tidak berubah, dan pemain tetap bisa lepas landas.
- **Heavy Item dibawa saat lepas landas:** tidak mungkin, karena semua barang sudah terpasang saat `Ready`.
- **Region 3 dilepas saat pemanasan:** jarak pemain sudah melewati `abortDistance` sebelum region lepas, jadi pemanasan sudah batal.
- **Kerangka dan kokpit:** tidak memakai `IDamageable`, sehingga zombie, hewan, dan bencana tidak bisa merusaknya.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:**

- Menang kalau helikopter lepas landas, tanpa batas waktu (GDD bagian 3, `GLOSSARY.md`).
- Pemasangan lewat trigger tanpa `E` (`IslandInteractable.md`, `PhysicsLayers.md`).
- Lima suku cadang (`GameBalanceConfig.md`, `HeavyItemSystem.md`).

**Usulan yang disetujui developer sebagai nilai awal (dicatat di #85):**

1. **Pemanasan mesin:** rekomendasi 120 detik bertahan di dekat kokpit sebelum bisa terbang. GDD hanya menyebut merakit dan lepas landas, jadi ini tambahan untuk memberi klimaks. Alternatifnya, langsung terbang saat `E` ditekan.
2. **Gelombang saat pemanasan:** rekomendasi `RequestPanicWave` hanya saat malam, karena zombie terbakar di siang hari.
3. **Event hari ke-60:** rekomendasi pesan kritis dan satu gelombang besar pada malam hari ke-60, sebagai tekanan sebelum zombie menguat, tanpa mengubah syarat menang.
4. **Bahan tambahan:** rekomendasi tidak ada. Pemasangan langsung selesai saat barang tiba.
5. **NPC ikut terbang:** rekomendasi semua NPC hidup dianggap ikut. Alternatifnya, hanya NPC di dekat helipad, tetapi itu menambah pekerjaan pengawalan.
6. **Save setelah tiap pemasangan:** rekomendasi ya, dengan `SaveReason.PartInstalled`, yang tetap melewati pemeriksaan pertempuran, supaya perjalanan panjang tidak hilang saat game tertutup.

## 6. Skenario Tes

1. `setday 59`, `tpregion Snow`, lalu bawa `Part_Rotor` ke `PartInstallZone`. **Hasil:** barang terpasang, HUD menulis `Suku cadang 1 / 5`, dan model bertambah.
2. Jatuhkan barang di dalam zona dengan `Q`. **Hasil:** tetap terpasang.
3. Jatuhkan barang di zona lewat data save, lalu muat region. **Hasil:** `ReconcileLyingParts` memasangnya.
4. Tekan `E` di kokpit dengan 4 bagian. **Hasil:** ditolak dengan pesan `Suku cadang kurang`.
5. `heli install all`. **Hasil:** `HelicopterReadyEvent` terkirim sekali, dan `obj_parts` selesai.
6. `settime 22`, tekan `E`. **Hasil:** gelombang panik datang, dan hitung mundur 120 detik berjalan.
7. `settime 12`, tekan `E`. **Hasil:** tidak ada gelombang, dan suara mesin tetap terkirim (`eventlog 1`).
8. Berjalan lebih dari 40 meter saat pemanasan. **Hasil:** mesin mati, dan state kembali `Ready`.
9. Buka panel `Pause` saat pemanasan. **Hasil:** hitung mundur berhenti.
10. Simpan saat pemanasan, lalu lanjutkan run. **Hasil:** state `Ready`, dan bagian terpasang tetap 5.
11. Selesaikan pemanasan, lalu tekan `E`. **Hasil:** `RunEndedEvent` bernilai `Won`, adegan berjalan, dan `WinScreen` tampil.
12. `setday 60`, `settime 23.9`, biarkan jam lewat 24:00 tanpa lepas landas. **Hasil:** run tetap berjalan, dan notifikasi hari ke-60 terlewat tampil sekali. Pada hari ke-75, `heli install all`, panaskan mesin, lalu tekan `E`. **Hasil:** `RunEndedEvent` bernilai `Won`.
13. `setday 60`, `settime 18`, tunggu malam. **Hasil:** satu gelombang `target_night`, dan pesan hari ke-60 hanya sekali.
14. Mati, lalu mulai run baru. **Hasil:** state `Assembling`, dan `installedPartIds` kosong.
