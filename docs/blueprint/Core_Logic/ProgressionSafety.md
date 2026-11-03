# Blueprint: ProgressionSafety.cs
**Lokasi:** `docs/blueprint/Core_Logic/ProgressionSafety.md`
**Deskripsi Utama:** Merangkum aturan yang menjaga jalur menang tetap terbuka, memeriksa kondisi run secara berkala, dan memberi jalan keluar saat pemain tersangkut.
**Issue:** #116
**Rujukan GDD:** Bagian 1, 3, dan 4 (jalur menang dan Diary). Pengaman ini adalah usulan arsitektur, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

`ProgressionSafety` adalah singleton di scene `IslandPhase`. Sistem ini membaca status milik sistem lain dan tidak menjadi pemilik data baru.

```csharp
public enum SafetySeverity { Ok, Fixed, Warning, Blocked }
public enum UnstuckResult { Started, Busy, Cooldown, NotAllowed, NoSafePoint }

public struct SafetyFinding { public string checkId; public SafetySeverity severity; public string message; }
```

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Instance` | `ProgressionSafety` | `null` | Singleton |
| `lastReport` | `List<SafetyFinding>` | Kosong | Hasil pemeriksaan terakhir |
| `safePoints` | `Vector3[]` | Ukuran `unstuckRingSize` | Riwayat posisi aman |
| `regionFallbacks` | `RegionSafePoint[]` | Satu per region | Titik buatan tangan, berupa data |
| `lastTimeWarningDay` | `int` | 0 | Peringatan zombie menguat paling banyak sekali per hari |
| `unstuckCooldownLeft` | `float` | 0 | Sisa jeda |
| `unstuckRoutine` | `Coroutine` | `null` | Hitung mundur yang berjalan |

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.safety.unstuckRingSize` | 12 | Jumlah riwayat posisi |
| `GameBalanceConfig.safety.unstuckSampleSeconds` | 5 | Jeda mencatat posisi |
| `GameBalanceConfig.safety.unstuckNavSampleRadius` | 2 | Posisi harus dekat NavMesh |
| `GameBalanceConfig.safety.unstuckMinRewindDistance` | 3 | Jarak minimum dari posisi tersangkut |
| `GameBalanceConfig.safety.unstuckChannelSeconds` | 5 | Hitung mundur, batal kalau kena damage |
| `GameBalanceConfig.safety.unstuckCooldownSeconds` | 180 | Jeda antar-pemakaian |
| `GameBalanceConfig.safety.stuckHintSeconds` | 20 | Petunjuk kalau input maju tetapi posisi tidak berubah |
| `GameBalanceConfig.safety.minStonePickupsRegion1` | 4 | Batu kecil minimum di Region 1 |
| `GameBalanceConfig.safety.woodPerSecondEstimate` | 0,4 | Dari `GameBalanceConfig.md`: 10 kayu per 25 detik |
| `GameBalanceConfig.safety.regionTravelSeconds` | 300 | Perkiraan membuka satu region berikutnya |
| `GameBalanceConfig.safety.partRoundTripSeconds` | 240 | Dari `GameBalanceConfig.md` |
| `GameBalanceConfig.safety.estimateMargin` | 1,5 | Pengali waktu makan, tidur, dan bertahan |

### Benda dan kondisi wajib untuk menang

| No | Benda atau kondisi | Sumber | Pengaman | Pemilik |
|---|---|---|---|---|
| 1 | `Tool_Axe` (tidak wajib, tetapi mempercepat) | `rec_axe` di `Hand` | Bisa selalu dibuat ulang, lihat di bawah | #77, #113 |
| 2 | `Mat_Wood` 500 | Pohon Region 1, tumbuh lagi | Pohon bisa ditebang tangan (damage 1, `maxHits` 5) | #73 |
| 3 | Diary Bab 2 | Pos kemah Region 1 | Buku tanpa `BaseHealth`, dan `NoBuildVolume` | #81 |
| 4 | Blueprint Jembatan | Hadiah Bab 2 | Bukan item, jadi tidak memakai slot | #68 |
| 5 | Jembatan `S1` | `BridgeSite` | Tidak bisa dirusak. Setoran tidak bisa hilang | #68 |
| 6 | Diary Bab 3 | Pos terakhir Region 2 | Sama dengan Bab 2. Membuka `S2` | #81, #68 |
| 7 | Akses `Logistic` dan `Snow` | `S2`, `S3` | Status gerbang di `world_gates` | #68 |
| 8 | 5 suku cadang | Region 4 | Tidak bisa hancur, dan kembali ke posisi valid | #69 |
| 9 | Helipad, kerangka, dan kokpit | Region 3 | Tanpa `IDamageable`, dan `NoBuildVolume` | #14 |
| 10 | Vaksin (untuk bertahan hidup) | `rec_vaccine`, `MedicalBench` | Node Region 2 tumbuh lagi, dan `Base_Pisau` tidak bisa hancur | #64, #73 |

Meja `Build_MedicalBench` membutuhkan 15 kayu dan 4 batu, yang tersedia dari jalur alat di bawah.

## 2. Detail Logika & Fungsi

### Perlindungan `KeyItem`

`ItemType.KeyItem` dan `isCoreItem` tidak boleh keluar dari kepemilikan pemain, pada semua jalur:

| Jalur hilang | Aturan | Dijaga oleh |
|---|---|---|
| Dibuang | `isDroppable = false` | #66 |
| Dimasukkan peti | Ditolak | #46 |
| Hancur oleh durabilitas | Tidak pernah `Destroyed`. Boleh `Broken`, dan bisa diperbaiki di `Hand` | #113 |
| `WorldItem` kedaluwarsa | `despawns == false` | #66 |
| Air pasang | Sama dengan `despawns == false` | #91 |
| Inventori penuh saat menerima | Menjadi `WorldItem` tanpa kedaluwarsa | #66 |
| Bahan resep | Resep tidak boleh memakai `KeyItem` | #77 |
| Tumbal atau hadiah Lucky Machine | Tidak boleh dikonsumsi mesin | #72 |

**`public static bool IsProtected(ItemData item)`** = `item.isCoreItem || item.itemType == ItemType.KeyItem`. Peti, drop, resep, dan mesin memakai fungsi ini, supaya aturannya hanya satu.

**`private void AuditCoreItems()`:** kalau `Base_Kail`, `Base_Pistol`, atau `Base_Pisau` tidak ada di inventori dan tidak ada sebagai `WorldItem`, tambahkan kembali lewat `InventoryManager.AddItem`, lalu laporkan `Fixed`. Ini pengaman bug, bukan jalur normal.

### Alat dasar

Rantai minimum dari tangan kosong:

1. Pohon bisa ditebang tangan, jadi `Mat_Wood` selalu tersedia.
2. Batu pertama berasal dari `PickupInteractable` batu kecil, karena batu besar membutuhkan Kapak atau Beliung (`ResourceRespawn.md`).
3. `rec_axe` (3 kayu, 2 batu) dan `rec_pickaxe` dibuat di `Hand`, dan `startsUnlocked`.

**Celah yang harus ditutup:** `ResourceRespawn.md` belum punya jadwal tumbuh untuk batu kecil, padahal `IslandInteractable.md` menyerahkannya ke #73. Kalau pemain memakai semua batu kecil dan kapaknya hancur, pemain tidak punya batu untuk membuat kapak baru.

**`private void EnsureToolPath()`**, tiap pemeriksaan:

1. Kalau pemain punya `Tool_Axe` atau `Tool_Pickaxe` yang tidak `Destroyed`, di inventori atau peti: `Ok`.
2. Kalau `Mat_Stone >= 2` di inventori dan peti: `Ok`.
3. Kalau jumlah batu kecil aktif di Region 1 lebih dari 0 (`ResourceRespawn`): `Ok`.
4. Selain itu: `ResourceRespawn.Instance.ForceRegrowStones(Region1, minStonePickupsRegion1)`, lalu laporkan `Fixed`.

### Diary dan Blueprint Jembatan

- Buku tidak punya `BaseHealth`, jadi erupsi dan gempa tidak merusaknya (`ObjectiveManager.md`).
- Buku tetap ada setelah dibaca, dan bisa dibaca ulang di arsip `J`.
- Bangunan tidak bisa menimbun buku, lewat `NoBuildVolume` di tiap `DiaryBook`.
- Blueprint Jembatan tersimpan sebagai status gerbang, sehingga tidak bisa hilang, dibuang, atau hancur.
- `Region` yang dilepas tidak berpengaruh, karena `markerPosition` adalah data.

### Jembatan

Mengikuti rekomendasi `RegionGateSystem.md`: jembatan **tidak bisa dirusak**, tanpa `BaseHealth` dan `IDamageable`, sehingga tidak perlu jalur perbaikan. Kehilangan 500 kayu terlalu berat dalam game permadeath. Kalau developer memilih jembatan bisa rusak, perbaikan memakai #47 dengan syarat jalur `S1` tidak pernah tertutup total.

### Keluar dari posisi tersangkut

**Mencatat posisi.** `Update` menambah timer. Tiap `unstuckSampleSeconds`, kalau `TraversalMode == Ground`, pemain menapak, tidak di air, dan `NavMesh.SamplePosition` berhasil dalam `unstuckNavSampleRadius`: simpan ke `safePoints`.

**`public UnstuckResult RequestUnstuck()`**, dari tombol `Keluar dari Tersangkut` pada panel `Pause`, lewat `ConfirmDialog`:

1. `NotAllowed` kalau `RunLifecycle.State != OnIsland`, pemain diterkam (`PlayerPinnedEvent.Last.isPinned`), atau `IsRunEnding`.
2. `Cooldown` kalau `unstuckCooldownLeft > 0`.
3. `Busy` kalau `unstuckRoutine != null`.
4. Mulai `UnstuckRoutine()`, tutup `Pause`, lalu tampilkan hitung mundur.

**`IEnumerator UnstuckRoutine()`** memakai `Time.deltaTime`:

1. Tunggu `unstuckChannelSeconds`. `PlayerDamagedEvent` membatalkan hitung mundur, tanpa memakai cooldown. Ini mencegah fitur dipakai untuk kabur dari pertempuran.
2. Pilih titik: `safePoints` terbaru yang berjarak minimal `unstuckMinRewindDistance`, lalu `regionFallbacks` region saat ini, lalu `IslandArrival.LandingPosition`.
3. Kalau membawa Heavy Item: `HeavyItemSystem.Instance.Drop(DropReason.Unstuck)`. Barang punya pengaman posisinya sendiri (#69).
4. `IslandPlayerController.Teleport(titik)`, lalu `PlayerTraversal.ResetFallTracking()`.
5. `unstuckCooldownLeft = unstuckCooldownSeconds`.

Titik riwayat selalu berupa tempat yang pernah diinjak, sehingga pemain tidak bisa melewati gerbang terkunci.

**Petunjuk.** Kalau input maju aktif tetapi posisi berubah kurang dari 0,5 meter selama `stuckHintSeconds`, tampilkan notifikasi `Tersangkut? Buka menu jeda` sekali per kejadian.

### Pemeriksaan kondisi run

**`public IReadOnlyList<SafetyFinding> RunCheck()`** dipanggil saat `DayChangedEvent`, sekali pada frame pertama `RunLifecycle.State == OnIsland` (termasuk setelah load, saat semua `ISaveable` sudah pulih), dan oleh `softlockcheck`.

| `checkId` | Pemeriksaan | Hasil kalau gagal |
|---|---|---|
| `core_items` | `AuditCoreItems` | `Fixed` |
| `tool_path` | `EnsureToolPath` | `Fixed` |
| `parts_exist` | `HeavyItemSystem.records` berjumlah `helicopterPartCount` | `Blocked` |
| `parts_reachable` | Barang `Lying`: `NavMesh.SamplePosition` berhasil. Hanya barang di region yang dimuat | `Warning`. #69 memulihkannya |
| `bridge` | Blueprint ada dan jembatan belum selesai, tetapi `BridgeSite` tidak aktif | `Blocked` |
| `gates` | Kalau `HasBridgeBlueprint` dan jembatan selesai, `IsUnlocked(Amazon)` | `Blocked` |
| `gates_diary` | Bab 3 ada di `foundChapters`, `IsUnlocked(Logistic)` | `Blocked` |
| `path_blocked` | Tidak ada struktur di `NoBuildVolume` jalur wajib (`BuildMenu.md`) | `Warning` |
| `time_budget` | Lihat di bawah | `Warning` |

`Blocked` berarti ada bug: catat `Debug.LogError` dengan seed dari `RunSeedManager.CurrentSeedText`, dan tampilkan pesan `Terjadi masalah` di build pengembangan. Sistem tidak pernah mengakhiri run.

### Peringatan zombie menguat

**`private float EstimateSecondsNeeded()`**:

1. Kayu: kalau `Amazon` belum terbuka, `WoodRemaining / woodPerSecondEstimate`.
2. Region: `regionTravelSeconds` x jumlah region yang belum terbuka.
3. Suku cadang: `partRoundTripSeconds` x (`total - InstalledCount`).
4. Lepas landas: `rescue.engineWarmupSeconds`, kalau belum `Departed`.
5. Kalikan `estimateMargin`.

**Sisa waktu sampai target:** `(GameManager.TargetDay - currentDay) * dayLengthSeconds + (24 - jam) / 24 * dayLengthSeconds`. Hari ke-60 bukan kekalahan, jadi pemeriksaan ini hanya berjalan kalau `GameManager.IsPastTargetDay == false`. Setelah itu `time_budget` selalu `Ok`.

Kalau sisa kurang dari perkiraan, dan `lastTimeWarningDay != currentDay`: kirim notifikasi `Zombie akan menguat sebelum helikopter siap. Perkiraan kurang n hari` (#22). Peringatan hanya berupa pesan. Game tetap berjalan, dan run tidak dipaksa berakhir. Akhir run tetap ditentukan `GameManager`.

### Perintah `DevConsole` (#56)

| Perintah | Hasil |
|---|---|
| `softlockcheck` | Menjalankan `RunCheck`, lalu mencetak semua `SafetyFinding` |
| `unstuck` | `RequestUnstuck`, tanpa cooldown |
| `breaktool <itemId>` | Mengisi durabilitas sampai `Destroyed` |
| `clearstones` | Menandai semua batu kecil Region 1 sebagai diambil |

Perintah pengubah kondisi membuat `IsRunTainted` bernilai `true`.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** tidak ada. Pesan lewat `NotificationManager` (#22).

**Event yang didengar:** `DayChangedEvent`, `PlayerDamagedEvent`, `PlayerPinnedEvent`, dan `RunEndedEvent`.

| Sistem | Hubungan |
|---|---|
| `RegionGateSystem` (#68) | `HasBridgeBlueprint`, `IsUnlocked`, `WoodRemaining` |
| `HeavyItemSystem` (#69) | `records`, dan `Drop(Unstuck)` |
| `ObjectiveManager` (#81) | `foundChapters` |
| `RescueEventManager` (#14) | `InstalledCount`, `State` |
| `ResourceRespawn` (#73) | `ForceRegrowStones`, dan jumlah batu aktif |
| `InventoryManager`, `ItemUseSystem` (#66), `StorageChest` (#46) | `IsProtected`, jumlah item |
| `ToolRepairSystem` (#113) | Item inti tidak pernah `Destroyed` |
| `RunSeedManager` (#89) | Layout valid, dan seed untuk log |
| `IslandPlayerController`, `PlayerTraversal` (#101) | `Teleport`, `ResetFallTracking`, `TraversalMode` |
| `DevConsole` (#56) | Perintah di atas |
| `UIFocusStack` (#83), `UIManager` | Tombol di panel `Pause`, dan `ConfirmDialog` |

**Batas pekerjaan:** pengaman suku cadang di #69, seed yang valid di #89, dan pemulihan resource di #73.

## 4. Kasus Khusus

- **Objek pool:** tidak memakai pool, dan tidak memunculkan objek. `ForceRegrowStones` memakai objek milik #73.
- **Game dijeda:** semua timer memakai `Time.deltaTime`. Hitung mundur dan cooldown berhenti saat jeda. `RunCheck` tidak berjalan saat jeda, karena `DayChangedEvent` tidak terbit.
- **Save:** tidak ada data yang disimpan. Setelah load, `safePoints` diisi posisi pemain dan `regionFallbacks`, cooldown kembali 0, dan `lastTimeWarningDay` kembali 0.
- **Permadeath:** manager hidup di scene `IslandPhase`, jadi kembali kosong saat run baru. Tidak ada data yang bertahan.
- **Region belum dimuat saat pemeriksaan:** pemeriksaan membaca data manager, bukan objek region. `parts_reachable` melewati barang di region yang dilepas.
- **Fase 1:** sistem ini tidak aktif, karena manager berada di `IslandPhase`, dan Fase 1 hanya 60 detik.
- **Teleport ke air pasang:** titik riwayat hanya tercatat di luar air. `regionFallbacks` harus berada di atas batas pasang (#91).
- **Tersangkut saat tenggelam:** `NotAllowed` tidak berlaku, karena mode `Swimming` boleh. Titik tujuan tetap di darat.
- **Pemain bisa menyalahgunakan untuk kembali cepat:** titik hanya riwayat 60 detik terakhir (12 x 5 detik), dan ada hitung mundur serta cooldown.
- **Peringatan zombie menguat saat hampir menang:** tetap muncul kalau memang kurang, dan tidak mengubah apa pun.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:**

- Suku cadang tidak bisa hilang (`HeavyItemSystem.md`).
- Item inti tidak pernah hancur (`ToolRepairSystem.md`).
- Node kayu, jamur, dan getah tidak diacak (`RunSeedManager.md`).

**Usulan yang menunggu developer (dicatat di #85):**

1. **Jembatan tidak bisa rusak:** rekomendasi mengikuti usulan `RegionGateSystem.md`. Alternatifnya, jembatan punya HP dan diperbaiki lewat #47.
2. **Pengaman Diary:** rekomendasi buku tidak bisa rusak dan Blueprint berupa status, bukan item.
3. **Batu kecil tumbuh lagi:** rekomendasi jadwal normal di #73, ditambah `ForceRegrowStones` sebagai pengaman.
4. **Keluar dari tersangkut:** rekomendasi tombol di `Pause`, hitung mundur 5 detik yang batal saat kena damage, cooldown 180 detik, dan barang berat dijatuhkan.
5. **Waktu pemeriksaan:** rekomendasi tiap pergantian hari, dan setelah load.
6. **Peringatan zombie menguat:** rekomendasi sekali per hari sebelum hari ke-60, dengan margin 1,5. Tidak pernah memaksa game berakhir.

## 6. Skenario Tes

Semua memakai `DevConsole` (#56).

1. `softlockcheck` pada run baru. **Hasil:** semua `Ok`.
2. Coba buang `Base_Pisau`, dan masukkan ke peti. **Hasil:** keduanya ditolak.
3. `removeitem Base_Pistol`, lalu `softlockcheck`. **Hasil:** `core_items` berstatus `Fixed`, dan pistol kembali.
4. Hapus semua kayu dan batu, `breaktool Tool_Axe`, lalu `clearstones`, dan `softlockcheck`. **Hasil:** `tool_path` berstatus `Fixed`, batu kecil muncul, dan `rec_axe` bisa dibuat.
5. Tanpa kapak, tebang pohon dengan tangan. **Hasil:** pohon tumbang setelah 5 pukulan.
6. `disaster Eruption 2` di dekat buku Bab 1. **Hasil:** buku tetap bisa dibaca.
7. Coba bangun dinding di atas buku. **Hasil:** ditolak `NoBuildZone`.
8. `disaster Earthquake 2` di jembatan. **Hasil:** jembatan utuh, dan `IsUnlocked(Amazon)` tetap.
9. Tp ke dalam geometri, lalu tombol `Keluar dari Tersangkut`. **Hasil:** setelah 5 detik pemain kembali ke posisi terakhir yang valid, tanpa damage jatuh.
10. Pukul pemain saat hitung mundur. **Hasil:** batal, dan cooldown tidak terpakai.
11. Pakai lagi dalam 180 detik. **Hasil:** ditolak `Cooldown`.
12. Pakai saat membawa `Part_Engine`. **Hasil:** barang jatuh di tempat valid.
13. `setday 58`, tanpa jembatan, lalu tunggu pergantian hari. **Hasil:** peringatan zombie menguat tampil sekali, dan game tetap berjalan.
14. `setday 60` dengan helikopter `Ready`, lalu `softlockcheck`. **Hasil:** `time_budget` `Ok` kalau jam masih cukup untuk pemanasan.
15. Mati, lalu run baru. **Hasil:** `softlockcheck` kembali bersih.
16. `setday 61`, tanpa jembatan, lalu `softlockcheck`. **Hasil:** `time_budget` `Ok`, peringatan tidak tampil, dan pemeriksaan jalur menang tetap berjalan.
