# Blueprint: RegionGateSystem.cs & BridgeSite.cs
**Lokasi:** `docs/blueprint/Island_Logic/RegionGateSystem.md`
**Deskripsi Utama:** Pemilik status akses tiap region, penyetoran kayu ke jembatan, dan pengaman supaya pemain tidak melewati seam yang masih tertutup.
**Issue:** #68
**Rujukan GDD:** Bagian 4 (Diary Bab 2 memberi Blueprint Jembatan) dan bagian 5 (akses bertahap, jembatan gantung, nilai awal 500 kayu). Syarat Region 3 dan Region 4 adalah usulan, karena GDD tidak merincinya.

## 1. Arsitektur & Variabel Utama

| Kelas | Bentuk | Letak | Fungsi |
|---|---|---|---|
| `RegionGateSystem` | Singleton, `ISaveable` (`SaveId = world_gates`, `LoadOrder = 30`) | Scene `IslandPhase` | Menyimpan status gerbang, dan selalu ada walaupun region dilepas |
| `BridgeSite` | `IslandInteractable` | `Region1_Volcano`, di tepi jurang `S1` | Menerima setoran dan menampilkan progres |
| `GateBlocker` | `IslandInteractable` | Scene `IslandPhase`, di `S2` | Gerbang tua yang memberi alasan terkunci |
| `GateBarrier` | `BoxCollider` layer `Obstacle` | Scene `IslandPhase` | Dinding tak terlihat di seam dan perairan sekitarnya |
| `RavineRecoveryVolume` | Trigger layer `ZoneTrigger` | Dasar jurang `S1` | Mengembalikan pemain yang terjatuh |

```csharp
public enum GateKind { Bridge, DiaryChapter, FollowsPrevious }

[Serializable]
public class GateRule
{
    public string seamId;          // S1, S2, S3 (RegionStreaming.md)
    public RegionType unlocks;     // Amazon, Logistic, Snow
    public GateKind kind;
    public int diaryChapter;       // Dipakai GateKind.DiaryChapter
    public GameObject[] barriers;  // GateBarrier dan GateBlocker milik seam ini
}
```

Aturan gerbang adalah data, jadi mengubah usulan di bagian 5 tidak mengubah kode.

| Seam | Membuka | `kind` | Syarat |
|---|---|---|---|
| `S1` | `Amazon` | `Bridge` | Blueprint Jembatan, dan kayu tersetor mencapai `bridgeWoodCost` |
| `S2` | `Logistic` | `DiaryChapter` (3) | Usulan: Diary Bab 3 terbuka, yang terjadi saat pemain masuk Region 2 |
| `S3` | `Snow` | `FollowsPrevious` | Usulan: terbuka bersama `S2`. Rintangannya adalah dingin (#65) |

**Variabel `RegionGateSystem`:**

| Nama | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `woodDeposited` | `int` | 0 | Kayu yang sudah disetor |
| `hasBridgeBlueprint` | `bool` | `false` | Blueprint Jembatan dari Diary Bab 2. Dibaca lewat `HasBridgeBlueprint` |
| `bridgeComplete` | `bool` | `false` | Jembatan selesai |
| `unlockedMask` | `int` | Hanya `Volcano` | Bit per `RegionType` |
| `rules` | `GateRule[3]` | Tabel di atas | Aturan seam |
| `verifyTimer` | `float` | 0 | Pemeriksaan posisi pemain |

**Variabel `BridgeSite`:** `Transform[] segments` (20 ruas), `Collider deckCollider`, `TextMeshPro progressLabel`, `GameObject constructionProps`.

**Angka penyetelan:**

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.region.bridgeWoodCost` | 500 | Sudah ada. Keputusan #85 |
| `GameBalanceConfig.region.bridgeDepositHoldSeconds` | 1 | Lama tahan `E`, sesuai `IslandInteractable.md` |
| `GameBalanceConfig.region.ravineMinWidth` | 30 | Lebar jurang minimum dalam meter, diperiksa alat Editor |
| `GameBalanceConfig.region.gateVerifySeconds` | 2 | Jeda pemeriksaan posisi |
| `GameBalanceConfig.region.ravineFallDamage` | 20 | Damage `Fall` saat dikembalikan dari jurang |

## 2. Detail Logika & Fungsi

### Fungsi publik `RegionGateSystem`

| Fungsi | Dipakai oleh | Isi |
|---|---|---|
| `bool IsUnlocked(RegionType r)` | `RegionStreaming`, #116, #81 | Membaca `unlockedMask` |
| `int WoodRemaining { get; }` | `BridgeSite`, #81 | `Max(0, bridgeWoodCost - woodDeposited)` |
| `bool HasBridgeBlueprint { get; }` | `BridgeSite`, #116 | Membaca `hasBridgeBlueprint`, yang diisi `HandleDiaryChapter` saat Bab 2 terbuka |
| `int Deposit(int amount)` | `BridgeSite` | Mengubah progres, dan mengembalikan jumlah yang diterima |
| `void Unlock(RegionType r)` | Internal, `DevConsole` | Membuka region dan mematikan barrier |

### Menyetor kayu

`BridgeSite` memakai jumlah kumulatif. Ini menggantikan cek `Kayu >= 500` di `Claude.md`, karena 500 kayu memakan 5 slot dan tidak perlu dibawa sekaligus (`CarryCapacity.md`).

**`CanInteract(in InteractContext ctx)`:**

1. Kalau `ctx.IsCarryingHeavyItem`, kembalikan `CarryingHeavyItem`.
2. Kalau `!HasBridgeBlueprint`, kembalikan `Locked`. Prompt: `Butuh Blueprint Jembatan`.
3. Kalau `ctx.Inventory` tidak punya `Mat_Wood`, kembalikan `Locked`. Prompt: `Butuh kayu (312 / 500)`.
4. Selain itu, `None`.

Kayu boleh dikumpulkan sebelum Blueprint didapat. Hanya penyetorannya yang terkunci.

**`OnInteract(in InteractContext ctx)`**, setelah `E` ditahan `bridgeDepositHoldSeconds`:

1. `n = Min(jumlah Mat_Wood di inventori, WoodRemaining)`.
2. `ctx.Inventory.RemoveItem("Mat_Wood", n)`, lalu `RegionGateSystem.Deposit(n)`.
3. `Deposit` menambah `woodDeposited`, dan menerbitkan `BridgeProgressChangedEvent { deposited, required }`.
4. Kalau `WoodRemaining == 0` dan `!bridgeComplete`, jalankan `CompleteBridge()`.

**`CompleteBridge()`:**

1. `bridgeComplete = true`.
2. Panggil `BridgeSite.SetComplete()` kalau `Region1_Volcano` dimuat. Scene ini pasti dimuat, karena pemain sedang berdiri di sana.
3. `Unlock(Amazon)`, terbitkan `RegionUnlockedEvent { Amazon }`, dan kirim notifikasi (#22).
4. Terbitkan `SaveRequestedEvent` dengan `reason = BridgeComplete`.

### Visual jembatan mengikuti progres

**`BridgeSite.RefreshVisual()`** dipanggil saat `OnEnable` dan saat `BridgeProgressChangedEvent`:

1. `visible = FloorToInt(woodDeposited / (float)bridgeWoodCost * segments.Length)`.
2. Ruas `0..visible-1` aktif, dan ruas baru muncul dengan debu kayu lewat `VFXManager` (#15).
3. `progressLabel.text = "312 / 500"`. Papan ini berada di dunia game, bukan HUD.
4. `deckCollider.enabled = bridgeComplete`. Ruas setengah jadi tidak punya collider, sehingga pemain tidak bisa berjalan di jembatan yang belum selesai.
5. Kalau selesai, `constructionProps` dimatikan, dan `IsAvailable = false`.

`BridgeSite` tidak menyimpan data sendiri, karena scene region bisa dilepas. `CaptureState` mengembalikan string kosong, dan status dibaca dari `RegionGateSystem`.

### NavMesh

Tidak ada bake ulang saat bermain. `NavMeshLink` seam `S1` sudah ada di `IslandPhase`, satu per agent type (`RegionStreaming.md`). `RefreshLinks` mengaktifkannya kalau kedua region `Loaded` dan `IsUnlocked(Amazon)`, lalu `RegionUnlockedEvent` memicu pemeriksaan ulang. Dengan begitu zombie dan hewan bisa menyeberang. Hewan tetap dibatasi oleh wilayahnya di `WildlifeAI.md`, jadi yang menyeberang umumnya zombie.

### Jembatan tidak bisa dirusak (usulan)

Jembatan berada di layer `Structure` supaya bisa dipijak, tetapi tidak punya `BaseHealth`, tidak memakai `IDamageable`, dan tidak bertag `BaseWall`. Zombie, hewan, dan bencana (#70) tidak punya jalur damage ke jembatan, sehingga 500 kayu tidak bisa hilang dan #116 tidak perlu jalur perbaikan.

### Gerbang berikutnya

**`HandleDiaryChapter(DiaryChapterUnlockedEvent e)`:**

1. Kalau `e.chapter == 2`, set `hasBridgeBlueprint = true`.
2. Untuk tiap aturan `DiaryChapter` dengan bab yang sama, panggil `Unlock`, lalu buka aturan `FollowsPrevious` sesudahnya.

**`Unlock(RegionType r)`:** set bit, matikan `barriers` milik aturan itu, lalu terbitkan `RegionUnlockedEvent`. Kalau bit sudah terpasang, keluar tanpa event.

**`GateBlocker`:** `CanInteract` mengembalikan `Locked` dengan prompt `Terkunci. Cari petunjuk di Diary`. Objeknya mati saat `S2` terbuka.

### Mencegah melewati jurang

| Lapis | Cara | Mencegah |
|---|---|---|
| 1. Jarak | Lebar jurang minimal `ravineMinWidth` 30 meter. Lompatan lari terjauh sekitar 7,5 meter (`jumpHeight` 1,1, `sprintSpeed` 8) | Melompat |
| 2. Dinding | `GateBarrier` setinggi 30 meter di sisi seberang jurang, dan mati hanya saat seam terbuka | Lompat dari tempat tinggi |
| 3. Air | `GateBarrier` memanjang dari tebing sampai ke laut, melewati batas renang stamina (42 meter, #13) | Berenang memutar |
| 4. Bangunan | `NoBuildVolume` di sekitar `BridgeSite` dan tepi jurang (`BuildMenu.md`) | Tangga atau lantai buatan |
| 5. Pengembalian | `RavineRecoveryVolume` | Pemain yang jatuh |
| 6. Pemeriksaan | `VerifyPlayerRegion()` | Bug fisika |

**`RavineRecoveryVolume.OnTriggerEnter`:** kalau tag `Player`, pindahkan ke `RegionBoundaryGuard.LastSafePosition`, lalu kirim `DamageInfo` berjenis `Fall` sebesar `ravineFallDamage`. Volume diletakkan 5 meter di atas dasar, sehingga damage mendarat dari #101 tidak ikut terhitung.

**`VerifyPlayerRegion()`**, setiap `gateVerifySeconds` memakai `Time.deltaTime`: kalau `!IsUnlocked(RegionStreaming.GetRegionAt(pemain))`, pindahkan pemain ke `LastSafePosition`, dan catat peringatan lewat `DevConsole`.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `RegionUnlockedEvent`, `BridgeProgressChangedEvent` (baru, sticky), `SaveRequestedEvent`.

**Event yang didengar:** `DiaryChapterUnlockedEvent`, `RunEndedEvent` (menghentikan pemeriksaan).

| Sistem | Hubungan |
|---|---|
| `InventoryManager` | `RemoveItem("Mat_Wood", n)` |
| `RegionStreaming` (#88) | Membaca `IsUnlocked`, dan menyediakan `GetRegionAt` |
| `RegionBoundaryGuard` (#88) | Menyediakan `LastSafePosition` |
| `ObjectiveManager` (#81) | Menerbitkan bab Diary, dan membaca `WoodRemaining` serta `IsUnlocked` |
| `SwimmingController` (#13) | Dibatasi `GateBarrier` di air |
| `ProgressionSafety` (#116) | Membaca `HasBridgeBlueprint`, `bridgeComplete`, dan `IsUnlocked` |
| `DevConsole` (#56) | Perintah `unlockregion <region>` dan `depositwood <jumlah>` |

## 4. Kasus Khusus

- **Objek pool:** `RegionGateSystem` dan `BridgeSite` bukan objek pool. Debu kayu memakai `VFXManager`, dan tidak ada status yang perlu direset.
- **Game dijeda:** input terkunci (#83), sehingga setoran tidak bisa dimulai. `verifyTimer` memakai `Time.deltaTime`, jadi ikut berhenti.
- **Save:** `world_gates` (`LoadOrder = 30`) menyimpan `woodDeposited`, `bridgeComplete`, `unlockedMask`, dan `hasBridgeBlueprint`. `RestoreState` tidak mengirim event. `BridgeProgressChangedEvent` dikirim ulang setelah semua data dimuat. `barriers` diatur dari `unlockedMask`.
- **Permadeath:** semua data di atas ikut terhapus bersama save run. Kayu tersetor tidak kembali ke run baru.
- **Region dilepas saat progres berjalan:** `BridgeSite.OnEnable` memanggil `RefreshVisual`, sehingga ruas kembali sesuai data saat scene dimuat lagi.
- **`bridgeWoodCost` diubah lewat `DevConsole` lebih rendah dari `woodDeposited`:** setoran berikutnya tidak diperlukan. `RefreshVisual` membatasi rasio ke 1, dan `CompleteBridge` dipanggil saat `BalanceChanged`, atau saat `Interact` berikutnya.
- **Inventori hanya punya sebagian kayu:** setoran tetap diterima sebagian. Tidak ada kayu yang tersisa di lokasi.
- **Bab 3 sebelum jembatan selesai:** tidak terjadi dalam permainan normal, karena Bab 3 terbuka saat pemain masuk Region 2. Lewat `DevConsole`, `S2` bisa terbuka di data lebih dulu, tetapi `Amazon` belum terbuka, jadi `RegionStreaming` tetap tidak memuat Region 4.
- **Pemain di atas jembatan saat load:** `deckCollider` aktif dari data sebelum pemain dipulihkan, karena `RegionGateSystem` dimuat lebih dulu.
- **Zombie mengejar saat menyetor:** tahan `E` batal saat pemain menerima damage, mengikuti `PlayerInteractor`.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:**

- Syarat jembatan 500 kayu, dan bisa disetel (`GLOSSARY.md`, GDD bagian 5).
- Urutan rantai Region 1, Region 2, Region 4, Region 3, serta seam `S1`, `S2`, dan `S3` (`RegionStreaming.md`).
- Blueprint Jembatan berasal dari Diary Bab 2 (GDD bagian 4).

**Usulan yang disetujui developer sebagai nilai awal (dicatat di #85):**

1. **Syarat Region 4 (`S2`):** rekomendasi Diary Bab 3, karena bab itu mengungkap lokasi suku cadang. Bab 3 terbuka otomatis saat pemain masuk Region 2 (`ObjectiveManager.md`), jadi syarat ini tidak bisa membuat softlock.
2. **Syarat Region 3 (`S3`):** rekomendasi terbuka bersama `S2`, tanpa syarat item. Dingin sudah menjadi rintangan alami, dan pemain tidak boleh terkunci hanya karena gagal menemukan satu benda.
3. **Kerusakan jembatan:** rekomendasi tidak bisa dirusak zombie, hewan, atau bencana, karena kerugiannya terlalu besar dalam game permadeath. Alternatifnya, jembatan punya HP dan bisa diperbaiki lewat #47 dan #116.
4. **Bentuk Blueprint Jembatan:** rekomendasi berupa penanda `hasBridgeBlueprint` di sistem ini, tersimpan di `world_gates`, bukan item inventori, supaya tidak bisa terbuang atau hilang. Ini sama dengan usulan di `ObjectiveManager.md`. Semua pembaca memakai properti `HasBridgeBlueprint` (#116).
5. **Pencegahan renang:** rekomendasi memakai dinding tak terlihat di air. Alternatif arus laut lebih alami, tetapi lebih sulit dijamin.

## 6. Skenario Tes

1. Bawa 100 kayu ke `BridgeSite` tanpa Blueprint, lalu tekan `E`. **Hasil:** prompt `Butuh Blueprint Jembatan`, dan kayu tidak berkurang.
2. Temukan buku Diary, dan tunggu Bab 2 terbuka, sehingga `HasBridgeBlueprint` bernilai `true`, lalu tahan `E` selama 1 detik. **Hasil:** 100 kayu pindah, papan menulis `100 / 500`, dan 4 ruas tampil.
3. Coba berjalan di ruas setengah jadi. **Hasil:** pemain tidak tertahan oleh ruas, dan jatuh lalu dikembalikan oleh `RavineRecoveryVolume` dengan HP turun 20.
4. Setor tiga kali sampai 500. **Hasil:** semua ruas tampil, `deckCollider` aktif, `RegionUnlockedEvent(Amazon)` terkirim sekali, dan Region 2 mulai dimuat pada jarak 150 meter.
5. Malam hari setelah jembatan selesai, pancing zombie dari Region 2. **Hasil:** zombie menyeberang lewat `NavMeshLink`.
6. Sebelum jembatan selesai, lari dan lompat dari tepi jurang. **Hasil:** pemain jatuh sebelum mencapai sisi seberang.
7. Berenang menuju Region 2 dari laut. **Hasil:** tertahan `GateBarrier`, dan kehabisan stamina kalau tetap di air.
8. Tembak dan ledakkan jembatan, lalu paksa bencana. **Hasil:** jembatan tidak berubah.
9. Pakai `teleport` ke Region 2 sebelum terbuka. **Hasil:** dalam 2 detik pemain kembali ke `LastSafePosition`.
10. Simpan pada `312 / 500`, keluar, lalu muat. **Hasil:** papan dan ruas sama. Mati, lalu mulai run baru. **Hasil:** progres kembali `0 / 500`, dan Blueprint hilang.
11. Masuk Region 2, sehingga Bab 3 terbuka. **Hasil:** `GateBlocker` hilang, `S2` dan `S3` terbuka di data, dan Region 4 dimuat saat pemain mendekat.
