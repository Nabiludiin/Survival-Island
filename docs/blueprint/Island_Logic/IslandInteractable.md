# Blueprint: IslandInteractable.cs & PlayerInteractor.cs
**Lokasi:** `docs/blueprint/Island_Logic/IslandInteractable.md`
**Deskripsi Utama:** Interaksi tombol `E` dengan objek di pulau, seperti mengambil batu kecil, membuka peti loot, dan memakai Mesin Gacha. Sistem ini dirancang untuk pemain yang bergerak bebas dan bisa berlari.
**Issue:** #17
**Rujukan GDD:** Bagian 3 (looting), bagian 4 (Diary), bagian 6 (kargo Weekly Beach Supply), dan bagian 7 (Lucky Machine). Tombol `E` mengikuti keputusan #85 di `GLOSSARY.md`. Ketahanan target dan interaksi tahan adalah usulan, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

### Perbedaan dengan `ShipInteractable`

| Hal | `ShipInteractable` (Fase 1) | `IslandInteractable` (Fase 2) |
|---|---|---|
| Hasil interaksi | Barang masuk slot tangan, dan objek hilang | Beragam: ambil, buka, panel, atau aksi |
| Ruang simpan | Berat barang di tangan | Slot inventori (#66) |
| Gerak pemain | Jalan biasa | Bisa lari, jadi target mudah terlewat |
| Status objek | Hilang setelah diambil, tanpa save | Disimpan, termasuk saat region dilepas (#88) |

Keduanya memakai `IInteractable` dan `PlayerInteractor` yang sama, supaya raycast dan prompt tidak ditulis dua kali.

### Bentuk kode

```csharp
public interface IInteractable
{
    bool IsAvailable { get; }                       // false kalau objek nonaktif atau sudah dipakai
    float HoldSeconds { get; }                      // 0 berarti langsung
    string GetPrompt();                             // Teks aksi, tanpa nama tombol
    InteractBlockReason CanInteract(in InteractContext ctx);
    void OnHoverEnter();
    void OnHoverExit();
    bool Interact(in InteractContext ctx);          // true kalau berhasil
}

public readonly struct InteractContext
{
    public readonly InventoryManager Inventory;
    public readonly Transform Player;
    public readonly bool IsCarryingHeavyItem;
}

public enum InteractBlockReason { None, InventoryFull, HandsFull, MissingTool, CarryingHeavyItem, Locked }
```

`ShipInteractable` mengubah `Interact(InventoryManager inv)` menjadi `Interact(in InteractContext ctx)`, dan mengembalikan `HandsFull` saat tangan penuh.

```csharp
public abstract class IslandInteractable : MonoBehaviour, IInteractable, ISaveable
{
    [SerializeField] protected string promptKey;      // Kunci teks untuk #104
    [SerializeField] protected float holdSeconds;     // Nilai awal 0
    [SerializeField] protected bool blockedWhileCarrying = true;
    [SerializeField] protected string saveId;         // ID stabil dari #80
    protected abstract bool OnInteract(in InteractContext ctx);
}
```

### Jenis objek

Kelas di bawah ini mewarisi `IslandInteractable`, atau memakai `IInteractable` langsung.

| Objek | Kelas | Hasil | `HoldSeconds` | Pemilik |
|---|---|---|---|---|
| Batu kecil, ranting, dan buah | `PickupInteractable` | Item masuk inventori, lalu objek hilang | 0 | Issue ini |
| Peti loot | `LootContainerInteractable` | Membuka peti dan mengeluarkan `WorldItem` | 1 | Issue ini, dengan isi dari #103 |
| Item jatuh | `WorldItem` | Masuk inventori | 0 | #66 |
| Lucky Machine | `LuckyMachine` | Membuka panel | 0 | #72 |
| Diary | `DiaryBook` | Membuka panel | 0 | #81 |
| Kargo pantai | `CargoCrate` | Membuka kargo | 2 | #71 |
| Peti penyimpanan | `StorageChest` | Membuka panel | 0 | #46 |
| Meja crafting | `CraftingStation` | Membuka panel | 0 | #77 |
| Api unggun | `CampfireManager` | Membuka panel | 0 | #44 |
| Kasur | `SleepSystem` | Tidur | 0 | #67 |
| Lokasi jembatan | `BridgeSite` | Menyetor kayu | 1 | #68 |
| Suku cadang | `HeavyItem` | Mengangkat barang | 0,5 | #69 |
| NPC | `NPCCompanion` | Menu perintah | 0 | #76 |
| Tangga dan tali | `ClimbableSurface` | Mulai memanjat | 0 | #101 |

### Variabel `PlayerInteractor`

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Current` | `IInteractable` | `null` | Target saat ini |
| `holdTimer` | `float` | 0 | Waktu tombol ditahan |
| `lostTargetTimer` | `float` | 0 | Waktu sejak target hilang dari bidikan |
| `scanTimer` | `float` | 0 | Jeda pemindaian |
| `cameraTransform` | `Transform` | Dari `FirstPersonCamera` | Asal raycast |

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.interact.range` | 3 | Jarak interaksi, sama dengan `ShipPlayerController` |
| `GameBalanceConfig.interact.sphereRadius` | 0,15 | Lebar bidikan |
| `GameBalanceConfig.interact.scanIntervalSeconds` | 0,05 | Pemindaian 20 kali per detik |
| `GameBalanceConfig.interact.targetGraceSeconds` | 0,15 | Target bertahan sebentar setelah bidikan lepas |
| `GameBalanceConfig.interact.holdLoseRange` | 3,5 | Interaksi tahan batal di luar jarak ini |
| `GameBalanceConfig.interact.moveCancelSpeed` | 6 | Interaksi tahan batal kalau kecepatan pemain di atas nilai ini |

## 2. Detail Logika & Fungsi

### Memindai target

`PlayerInteractor` berada di pemain Fase 1 dan Fase 2. `ShipPlayerController` tidak lagi punya raycast sendiri.

**`private IInteractable ScanForTarget()`**:

```csharp
Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
if (!PhysicsQuery.SphereCast(ray, sphereRadius, out RaycastHit hit, range, PhysicsMasks.Interact)) return null;

// Mask Interact tidak memuat dinding, jadi garis pandang diperiksa terpisah
if (PhysicsQuery.Raycast(ray, out RaycastHit block, hit.distance, PhysicsMasks.InteractBlock)) return null;

return hit.collider.GetComponentInParent<IInteractable>();
```

- **Asal raycast:** kamera dunia, dari `FirstPersonCamera` (#102). Getaran dan recoil ikut menggeser bidikan, tetapi `targetGraceSeconds` meredam perubahan singkat.
- **Sphere cast:** memudahkan bidikan saat berlari. Dinding tetap memblokir, karena raycast kedua memakai garis lurus.
- **Trigger diabaikan:** semua query lewat `PhysicsQuery`, jadi zona region tidak menghalangi (#87).

### Alur per frame

**`void Update()`**:

1. Kalau `UIFocusStack.Instance.IsGameplayInputBlocked`, panggil `ClearTarget()` dan selesai. Prompt hilang saat panel terbuka.
2. Kalau `Current` tidak lagi tersedia, panggil `ClearTarget()`. Pengecekan: objek `activeInHierarchy` dan `IsAvailable`.
3. Setiap `scanIntervalSeconds`, panggil `ScanForTarget()`:
   - Target sama: `lostTargetTimer = 0`.
   - Target baru: `OnHoverExit` target lama, `OnHoverEnter` target baru, dan `holdTimer = 0`.
   - Tidak ada target: tambah `lostTargetTimer`. Kalau melewati `targetGraceSeconds` dan tidak sedang menahan tombol, panggil `ClearTarget()`.
4. Perbarui prompt lewat `RefreshPrompt()`.
5. Tangani tombol.

### Tombol langsung dan tahan

**`HandleInput()`**:

| Keadaan | Tindakan |
|---|---|
| `HoldSeconds == 0` dan `InteractPressed` | Panggil `TryInteract()` |
| `HoldSeconds > 0` dan `InteractHeld` | Tambah `holdTimer`. Saat mencapai `HoldSeconds`, panggil `TryInteract()`, lalu `holdTimer = 0` |
| Tombol dilepas | `holdTimer = 0` |

`InputManager` perlu `InteractHeld`, dengan pola yang sama seperti `PrimaryHeld`.

**Selama interaksi tahan, pemain tetap bisa bergerak.** Interaksi batal kalau:

- Target berganti.
- Jarak ke target melebihi `holdLoseRange`.
- Kecepatan pemain melebihi `moveCancelSpeed`, yang berarti pemain sedang berlari.
- `PlayerDamagedEvent` terkirim.

**`private void TryInteract()`**:

1. Kalau jarak ke target melebihi `holdLoseRange`, panggil `ClearTarget()` dan selesai. Target yang bertahan karena `targetGraceSeconds` bisa sudah tertinggal saat pemain berlari.
2. Hitung `reason = Current.CanInteract(ctx)`. Kalau bukan `None`, tampilkan notifikasi yang sesuai (#22), mainkan suara tolak, lalu selesai.
3. Panggil `Current.Interact(ctx)`.
4. Kalau berhasil, panggil `RefreshPrompt()` dan terbitkan `InteractedEvent { targetId }`.

`InteractContext` dibuat sekali per frame, tanpa alokasi, karena berupa struct.

### Prompt dan highlight

**`RefreshPrompt()`** membuat teks seperti `[E] Buka Peti`, dengan `InputManager.GetBindingDisplay(GameInputAction.Interact)` dan `GetPrompt()`. Kalau tombol diganti, teks ikut berubah (#18). Hasilnya dikirim ke `HUDManager.ShowInteraction(string)`, atau `HideInteraction()` kalau kosong.

Teks hanya dibuat ulang kalau target atau alasan penolakan berubah, supaya tidak menghasilkan garbage per frame. Interaksi tahan juga memanggil `HUDManager.SetInteractionProgress(float)`.

**`InteractHighlight`** adalah komponen bersama `IslandInteractable` dan `ShipInteractable`:

| Status | Warna | Kapan |
|---|---|---|
| `Off` | Tanpa outline | Tidak dibidik |
| `Available` | Kuning muda | `CanInteract` menghasilkan `None` |
| `Blocked` | Merah | `CanInteract` menghasilkan alasan lain, termasuk tangan penuh di #74 |

Warna dibaca dari `GameBalanceConfig.interact`. Outline memakai `MaterialPropertyBlock` pada renderer outline yang sudah ada, bukan menambah material ke array, supaya tidak mengalokasi saat hover.

### Objek milik issue ini

**`PickupInteractable`:**

| Field | Nilai awal | Fungsi |
|---|---|---|
| `item` | Dari Inspector | `ItemData` yang diambil |
| `amount` | 1 | Jumlah |

`OnInteract`: panggil `ctx.Inventory.AddItem(item, amount)`. Kalau ada sisa, `amount = sisa`, tampilkan `Inventori penuh`, dan objek tetap ada. Kalau habis, catat `isConsumed = true`, lalu matikan objek. Batu kecil dan ranting hasil respawn diatur #73.

**`LootContainerInteractable`:**

| Field | Nilai awal | Fungsi |
|---|---|---|
| `lootTable` | Dari Inspector | `LootTable` dari #103 |
| `dropPoints` | 3 `Transform` | Titik keluarnya item |
| `isOpened` | `false` | Sudah dibuka |

`OnInteract`:

1. `isOpened = true`, dan mainkan animasi tutup terbuka.
2. Buat `var rng = RunSeedManager.Instance.Fork(RandomStreamId.IslandLoot, saveId)`, supaya isinya tetap pada seed yang sama dan tidak berubah setelah load (#89).
3. Tiap isi peti menjadi `WorldItem` lewat `WorldItemManager.Spawn(..., despawns: false)`. Pemain tidak perlu ruang inventori untuk membuka peti.
4. Kirim `NoiseEmittedEvent` lewat `NoiseEmitter` komponen peti (#49), dengan radius kecil.

Setelah dibuka, `IsAvailable = false`, jadi prompt tidak muncul lagi.

### Saat membawa Heavy Item

Objek dengan `blockedWhileCarrying = true` mengembalikan `CarryingHeavyItem`. Nilai awal `true`, karena kedua tangan dipakai.

| Objek | Saat membawa Heavy Item |
|---|---|
| Pintu (#78) | Boleh, supaya rute Region 4 ke Region 3 tidak terhalang |
| Suku cadang lain | Tolak. Hanya satu barang |
| Titik pasang helikopter (#14) | Lewat trigger, tanpa `E` |
| Benda lain | Tolak dengan notifikasi `Tangan penuh` |

Barang dilepas dengan tombol `DropHeavyItem`, bukan `E` (#69).

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `InteractedEvent`, untuk #16 dan #57.

**Event yang didengar:** `PlayerDamagedEvent`, `UIFocusChangedEvent`.

| Sistem | Hubungan |
|---|---|
| `InputManager` (#18) | Menyediakan `InteractPressed`, `InteractHeld`, dan `GetBindingDisplay` |
| `FirstPersonCamera` (#102) | Asal raycast |
| `PhysicsLayers` (#87) | Mask `Interact` dan `InteractBlock`, serta `SphereCast` |
| `UIFocusStack` (#83) | Panel yang dibuka memakai `Push` |
| `ItemUseSystem` (#66) | `WorldItem` memakai interface ini, dan `AddItem` mengembalikan sisa |
| `NotificationManager` (#22) | Pesan penolakan |
| `HUDManager` | Prompt dan bar tahan |
| `WorldStateSave` (#80), `RegionStreaming` (#88) | Status objek |
| `ShipInteractable`, #74 | Memakai interface dan `PlayerInteractor` yang sama |
| `RegionLootTable` (#103) | Isi peti |
| Semua pemilik di tabel jenis objek | Mewarisi `IslandInteractable` |

## 4. Kasus Khusus

- **Objek pool:** `WorldItem` dan NPC bisa kembali ke pool saat dibidik. `Update` langkah 2 melepas target itu. Saat dipakai ulang, `OnSpawned` mengatur `IsAvailable = true` dan highlight `Off`.
- **Objek region dilepas saat dibidik:** Unity menganggap referensinya `null`, jadi langkah 2 melepas target tanpa error.
- **Game dijeda:** input `Gameplay` mati, dan `IsGameplayInputBlocked` membersihkan target. Interaksi tahan batal, dan `holdTimer` kembali ke 0.
- **Panel tanpa jeda terbuka:** sama dengan jeda, karena `Gameplay` mati.
- **Permadeath:** status `isConsumed` dan `isOpened` berada di save run, dan terhapus bersamanya (#82).
- **Save dan load:** `IslandInteractable` menyimpan `{ saveId, isConsumed, isOpened, amount }` lewat `ISaveable`. Objek yang sudah dipakai dimatikan saat `RestoreState`. `WorldItem` yang keluar dari peti disimpan di `RegionStateStore` (#66), jadi membuka peti tidak bisa diulang. `Current` dan `holdTimer` tidak disimpan.
- **Berlari melewati objek:** `targetGraceSeconds` menjaga target selama 0,15 detik, sehingga prompt tidak berkedip. Tombol `E` tidak mengenai objek yang sudah jauh, karena `TryInteract` memeriksa jarak ulang dengan batas `holdLoseRange`.
- **Dua objek berdekatan:** sphere cast mengambil yang pertama terkena. Kalau keduanya sama-sama terkena, yang lebih dekat dipilih.
- **Inventori penuh:** `PickupInteractable` memakai pesan yang sama dengan `WorldItem`.
- **Diterkam atau memanjat:** #110 dan #101 memanggil `PlayerInteractor.SetEnabled(false)`, yang membersihkan target.
- **Objek milik sistem lain punya collider di layer berbeda:** tiap collider target interaksi harus berada di layer `Interactable`, `WorldItem`, `HeavyItem`, atau `NPC`. `LayerAuditWindow` (#87) menambah pemeriksaan ini.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** tombol interaksi `E` di dua fase (`GLOSSARY.md`). Mask `Interact` dan `InteractBlock` (`PhysicsLayers.md`). Prompt memakai `GetBindingDisplay` (`InputManager.md`).

**Keputusan teknis di blueprint ini:**

- `IInteractable` dan `PlayerInteractor` dipakai dua fase.
- Target dipilih dengan sphere cast, lalu garis pandang diperiksa dengan raycast.
- Target bertahan 0,15 detik agar prompt stabil.

**Usulan yang menunggu developer (dicatat di #85):**

1. **Peti loot mengeluarkan isi ke tanah,** bukan membuka panel. Pemain tidak berhenti di tengah bahaya, dan panel hanya untuk peti simpanan (#46).
2. **Interaksi tahan:** membuka peti loot 1 detik dan kargo 2 detik, dan batal saat pemain berlari atau diserang. Benda kecil langsung terambil.
3. **Saat membawa Heavy Item:** hanya pintu yang boleh dipakai.
4. **Membuka peti menimbulkan suara kecil,** supaya looting malam punya risiko. Radius mengikuti #49.

## 6. Skenario Tes

1. Bidik batu kecil dari 2 meter. **Hasil:** prompt `[E] Ambil Batu` tampil dengan outline kuning. Tekan `E`. **Hasil:** batu masuk inventori, lalu hilang.
2. Bidik batu di balik dinding dari jarak 2 meter. **Hasil:** tidak ada prompt.
3. Lari lurus melewati batu sambil menekan `E` saat prompt muncul. **Hasil:** batu terambil, dan prompt tidak berkedip.
4. Berdiri di dalam zona region raksasa, lalu bidik peti. **Hasil:** trigger tidak menghalangi.
5. Tahan `E` pada peti loot selama 1 detik. **Hasil:** bar penuh, peti terbuka, dan item keluar ke tanah.
6. Tahan `E` lalu lepas pada 0,5 detik. **Hasil:** bar kembali kosong, dan peti tertutup.
7. Tahan `E` sambil lari, atau terima pukulan. **Hasil:** interaksi batal.
8. Isi inventori, lalu ambil batu. **Hasil:** outline merah dengan pesan `Inventori penuh`, dan batu tetap ada.
9. Buka peti, simpan, keluar, lalu lanjutkan. **Hasil:** peti tetap terbuka, dan isinya tidak berlipat.
10. Buka peti pada seed yang sama di dua run. **Hasil:** isinya sama.
11. Bawa Heavy Item, lalu bidik peti. **Hasil:** ditolak dengan `Tangan penuh`. Bidik pintu. **Hasil:** pintu terbuka.
12. Buka inventori saat membidik objek. **Hasil:** prompt hilang. Tutup inventori. **Hasil:** prompt kembali.
13. Ganti tombol `Interact` ke `T` lewat #19. **Hasil:** prompt menjadi `[T]`.
14. Bidik `WorldItem`, lalu biarkan item hilang karena batas waktu. **Hasil:** prompt hilang tanpa error.
15. Tinggalkan region sampai dilepas saat prompt tampil, lewat `DevConsole`. **Hasil:** tidak ada error referensi.
16. Bidik Lucky Machine, lalu tekan `E`. **Hasil:** panel `LuckyMachine` terbuka lewat `UIFocusStack`.
