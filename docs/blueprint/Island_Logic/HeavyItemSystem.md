# Blueprint: HeavyItemSystem.cs
**Lokasi:** `docs/blueprint/Island_Logic/HeavyItemSystem.md`
**Deskripsi Utama:** Mengatur suku cadang helikopter sejak tersebar di Region 4, diangkut dan dijatuhkan, sampai tiba di titik pemasangan.
**Issue:** #69
**Rujukan GDD:** Bagian 1 dan bagian 5 Region 4 (suku cadang tersebar, tidak bisa dibawa sekaligus, dan bolak-balik ke Region 3). Jumlah dan nama dua suku cadang terakhir adalah usulan, karena GDD menulis "dll".

## 1. Arsitektur & Variabel Utama

| Kelas | Bentuk | Letak | Fungsi |
|---|---|---|---|
| `HeavyItemSystem` | Singleton, `ISaveable` (`world_parts`) | Scene `IslandPhase` | Pemilik status semua suku cadang |
| `HeavyItemData` | ScriptableObject | Aset | Data tiap suku cadang |
| `HeavyItem` | `IInteractable` | Prefab, layer dan tag `HeavyItem` | Benda di dunia |
| `PartRecord` | Kelas serializable | Di dalam `HeavyItemSystem` | Status satu barang, tetap ada walaupun region dilepas |

```csharp
[CreateAssetMenu(menuName = "Survival/Heavy Item")]
public class HeavyItemData : ScriptableObject
{
    public string partId;
    public string displayNameKey;
    public float weightKg;                 // Untuk UI dan suara jatuh pada versi awal
    public GameObject prefab;
    public float speedMultiplierOverride;  // 0 berarti memakai region.heavyItemSpeedMultiplier
}

public enum PartState { Lying, Carried, Installed }

[Serializable]
public class PartRecord
{
    public string partId;
    public PartState state;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 lastValidPosition;
    public RegionType region;
    public bool isDiscovered;
}
```

**Daftar suku cadang.** `GameBalanceConfig.region.helicopterPartCount` bernilai 5.

| `partId` | Nama | Sumber | `weightKg` |
|---|---|---|---|
| `Part_Rotor` | Baling-baling | GDD | 80 |
| `Part_Engine` | Mesin | GDD | 150 |
| `Part_FuelTank` | Tangki Bahan Bakar | GDD | 100 |
| `Part_TailRotor` | Baling-baling Ekor | Usulan | 40 |
| `Part_Avionics` | Panel Kendali | Usulan | 30 |

**Variabel `HeavyItemSystem`:** `HeavyItemData[] parts`, `PartRecord[] records`, `HeavyItem[] instances`, `CarriedIndex` (`int`, -1), dan `validSampleTimer`, `recoveryTimer` (`float`, 0).

**Properti:** `bool IsCarrying => CarriedIndex >= 0`, dan `int InstalledCount`.

**Angka penyetelan:**

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.region.heavyItemSpeedMultiplier` | 0,5 | Sudah ada |
| `GameBalanceConfig.vitals.heavyStaminaRegenMultiplier` | 0,75 | Sudah ada |
| `GameBalanceConfig.heavyItem.pickupHoldSeconds` | 0,5 | Sesuai `IslandInteractable.md` |
| `GameBalanceConfig.heavyItem.dropDamageThreshold` | 25 | Damage yang menjatuhkan barang. Zombie biasa 15, serangan berat 30 |
| `GameBalanceConfig.heavyItem.dropForwardDistance` | 1,2 | Jarak barang dari pemain saat dilepas |
| `GameBalanceConfig.heavyItem.validSampleSeconds` | 1 | Jeda mencatat posisi valid saat dibawa |
| `GameBalanceConfig.heavyItem.navSampleRadius` | 2 | Jarak maksimum ke NavMesh |
| `GameBalanceConfig.heavyItem.recoveryCheckSeconds` | 5 | Jeda pemeriksaan barang tergeletak |
| `GameBalanceConfig.heavyItem.discoverRadius` | 40 | Jarak barang mulai ditandai |

## 2. Detail Logika & Fungsi

### Penyebaran di Region 4

**`private void BuildRecords()`** berjalan saat `RunStartedEvent { isNewRun = true }`:

1. Baca `RunSeedManager.CurrentLayout.partPointByPart`.
2. Untuk indeks `i`, ambil titik dari `PartSpawnPointTable`, lalu isi `records[i]` dengan `state = Lying`, `region = Logistic`, dan `lastValidPosition = titik`.
3. Buat `instances[i]` sekali lewat `Instantiate` sebagai objek `IslandPhase`, lalu matikan sampai `Region4_Village` berstatus `Loaded`.

Titik dipilih acak dari titik buatan tangan, minimal 10 titik untuk 5 barang, sesuai `RunSeedManager.md`. Barang bukan objek pool, karena jumlahnya tetap dan tidak pernah dibuang.

### Mengambil

`HeavyItem.CanInteract(in ctx)`:

1. Kalau `ctx.IsCarryingHeavyItem`, kembalikan `CarryingHeavyItem`.
2. Kalau `PlayerTraversal.Mode` bukan `Ground` atau `Crouching`, kembalikan `HandsFull`.
3. Selain itu, `None`.

**`public bool PickUp(int index)`:**

1. `CarriedIndex = index`, `records[index].state = Carried`.
2. Jadikan barang anak `carryAnchor` pemain, `Rigidbody.isKinematic = true`, dan `collider.isTrigger = true`. Dengan begitu titik pasang (#14) tetap mendeteksi tag `HeavyItem`, dan barang tidak mendorong apa pun.
3. `IslandPlayerController.SetSpeedModifier(SpeedModifierId.HeavyItem, ResolveMultiplier(parts[index]))`.
4. Terbitkan `HeavyItemChangedEvent { partId, isCarried = true }`.

Barang tidak masuk inventori. Hanya ada satu `CarriedIndex`, sehingga pemain hanya bisa membawa satu barang.

### Efek saat membawa

`speed /= 2` diganti pengali. `SetSpeedModifier` menyimpan satu nilai per id, jadi nilainya tidak berkurang berulang, dan dikalikan dengan cuaca. Contoh: 0,5 x badai 0,7 = 0,35.

| Efek | Pemilik | Cara |
|---|---|---|
| Kecepatan setengah | `IslandPlayerController` | `SpeedModifierId.HeavyItem` |
| Sprint dan Jump mati | `IslandPlayerController` | Membaca `HeavyItemSystem.IsCarrying` |
| Regenerasi stamina | `SurvivalStats` | `HeavyItemChangedEvent` |
| Senjata dan alat terkunci | `EquipmentManager` | `EquipLock.HeavyItem`. Pemain tidak bisa menembak sebelum melepas barang (#60) |
| Obor disimpan | `EquipmentManager` | `offHandAllowedWhileCarrying = false` (#65) |
| Kamera | `FirstPersonCamera` | `lookScale`, bob, dan pose barang |
| Pintu | `IslandInteractable` | Tetap bisa dibuka |

### Melepas dan menjatuhkan

**`public void Drop(DropReason reason)`** dengan `DropReason { Manual, HeavyHit, Pinned, Water, Death, Installed }`:

1. Kalau `!IsCarrying`, keluar.
2. Lepas barang dari `carryAnchor`, `collider.isTrigger = false`, dan `Rigidbody.isKinematic = false`.
3. Letakkan `dropForwardDistance` di depan pemain. Kalau `PhysicsQuery.Raycast` ke arah itu mengenai `PhysicsMasks.InteractBlock`, letakkan di kaki pemain.
4. `ClearSpeedModifier(SpeedModifierId.HeavyItem)`, `state = Lying`, dan `CarriedIndex = -1`.
5. Terbitkan `HeavyItemChangedEvent { partId, isCarried = false }`.

| Pemicu | Sumber | Alasan |
|---|---|---|
| Tombol `Q` | `InputManager.DropHeavyItemPressed` | `Manual`. Pemain bisa bertarung |
| Serangan kuat | `PlayerDamagedEvent`, `amount >= dropDamageThreshold`, atau `type == Explosion` | `HeavyHit` |
| Diterkam | `PlayerPinnedEvent.isPinned` | `Pinned` |
| Berenang | `SwimStateChangedEvent.isSwimming` | `Water`. Barang langsung kembali ke posisi valid |
| Mati | `PlayerDiedEvent` | `Death`, supaya tidak terlihat melayang di `DeathCinematic` (#53) |

Air dangkal yang masih bisa dilewati tidak menjatuhkan barang (#13).

### Pengaman barang hilang

**Mencatat posisi valid.** Setiap `validSampleSeconds` saat dibawa, `lastValidPosition` diganti posisi kaki pemain kalau:

1. Pemain `isGrounded`, dan `TraversalMode` adalah `Ground`.
2. `NavMesh.SamplePosition` menemukan titik dalam `navSampleRadius`.
3. Pemain tidak berada di air.

**`HeavyItem.OnTriggerEnter`:** kalau collider berada di layer `Water`, panggil `ReturnToValid(index)`.

**`RecoveryTick()`** setiap `recoveryCheckSeconds`, untuk barang `Lying` di region `Loaded`:

1. Lewati barang yang masih bergerak (`Rigidbody.IsSleeping()` bernilai `false`), kecuali sudah berada di bawah `RegionRecord.bounds.min.y`.
2. Kalau tidak ada NavMesh dalam `navSampleRadius`, atau posisi di bawah batas region, panggil `ReturnToValid(index)`.
3. Kalau tidak, salin posisi ke `records[i].position`, dan perbarui `lastValidPosition`.

**`ReturnToValid(int index)`:** matikan kecepatan, pindahkan ke `lastValidPosition + Vector3.up * 0.5f`, dan kirim notifikasi `Suku cadang dikembalikan` (#22). Kalau titik itu juga tidak valid, gunakan titik dari `PartSpawnPointTable`. Barang tidak pernah bisa hilang, jadi #116 hanya perlu membaca status.

### Region dilepas dan dipindahkan

Saat `RegionLoadStateChangedEvent` `Unloading`, barang `Lying` di region itu disalin ke `records`, `Rigidbody` dibuat kinematic, dan objek dimatikan. Saat `Loaded`, objek aktif di posisi tersimpan. Barang yang dibawa adalah anak pemain, sehingga tidak terpengaruh. `records[i].region` diperbarui lewat `RegionStreaming.GetRegionAt`.

### Penanda dan jumlah terpasang

- **Penemuan:** setiap `recoveryCheckSeconds`, barang dalam `discoverRadius` dari pemain menjadi `isDiscovered = true`.
- **Kompas (#30):** `CompassSystem.SetMarker("part_<partId>", position)` untuk barang `isDiscovered` yang `Lying`. Barang yang dibawa tidak ditandai, tetapi helipad ditandai selama `IsCarrying`.
- **Jumlah terpasang:** HUD mendengar `HelicopterPartInstalledEvent { partId, installed, total }`, lalu menulis `Suku cadang 2 / 5`.

### Tiba di titik pasang

**`public void MarkInstalled(string partId)`** dipanggil `RescueEventManager` (#14) saat trigger titik pasang menerima barang:

1. Kalau barang itu sedang dibawa, jalankan langkah `Drop` tanpa fisika (`DropReason.Installed`).
2. `state = Installed`, matikan `instances[i]`, dan `CompassSystem.RemoveMarker`.
3. Tandai `InstalledCount`. Perakitan dan kemenangan milik #14.

Barang yang tergeletak di dalam trigger juga terpasang, sehingga pemain tidak dihukum karena barang jatuh di dekat kerangka.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `HeavyItemChangedEvent`.

**Event yang didengar:** `RunStartedEvent`, `PlayerDamagedEvent`, `PlayerPinnedEvent`, `PlayerDiedEvent`, `SwimStateChangedEvent` (baru, #13), `RegionLoadStateChangedEvent` (#88), dan `RunEndedEvent`.

| Sistem | Hubungan |
|---|---|
| `IslandPlayerController`, `PlayerTraversal` (#101) | `SetSpeedModifier`, dan membaca `IsCarrying` |
| `EquipmentManager` (#60), `SurvivalStats` (#111), `FirstPersonCamera` (#102) | Mendengar `HeavyItemChangedEvent` |
| `RunSeedManager` (#89) | `CurrentLayout.partPointByPart` |
| `RegionStreaming` (#88) | Status muat dan `GetRegionAt` |
| `RescueEventManager` (#14) | Memanggil `MarkInstalled`, dan menerbitkan `HelicopterPartInstalledEvent` |
| `CompassSystem` (#30) | Penanda |
| `BuildManager` | `PhysicsMasks.BuildOverlap` sudah memuat `HeavyItem`, jadi barang tidak bisa tertimbun bangunan |
| `ProgressionSafety` (#116) | Membaca `records` |

## 4. Kasus Khusus

- **Objek pool:** barang tidak memakai pool. Debu saat jatuh memakai `VFXManager`.
- **Game dijeda:** `Rigidbody` dan timer berhenti, dan input `Q` terkunci oleh `IsGameplayInputBlocked`.
- **Save:** `world_parts` menyimpan semua `PartRecord`. Barang `Carried` tetap `Carried`, dan dipasang kembali ke `carryAnchor` setelah pemain dipulihkan. `HeavyItemChangedEvent` sticky dikirim ulang. `RestoreState` tidak mengirim event.
- **Permadeath:** `world_parts` terhapus bersama save run. Run baru memakai susunan baru dari seed baru.
- **Serangan saat menyeberang seam:** barang jatuh di sisi tempat pemain berada, dan statusnya tetap tercatat walaupun region dilepas.
- **Fireball, tembakan, dan ledakan:** barang tidak memakai `IDamageable`, jadi tidak bisa hancur. Peluru berhenti di barang, sesuai `PhysicsMasks.Shoot`.
- **Item Lain di tanah:** tangan penuh, jadi `WorldItem` dan node panen menolak dengan `Tangan penuh`.
- **Tepi tebing:** pemeriksaan NavMesh mengembalikan barang yang jatuh ke tempat yang tak terjangkau.
- **Barang jatuh di air dangkal:** tetap memicu `Water`, dan kembali ke posisi valid. Posisi itu tidak pernah berada di air.
- **Menang saat membawa barang:** `MarkInstalled` melepasnya lebih dulu, jadi `HelicopterReadyEvent` tidak bentrok dengan pengali.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:**

- Pengali kecepatan menggantikan `speed /= 2` (`PlayerTraversal.md`).
- Tombol `Q` (`InputManager.md`), pintu boleh dibuka, dan hold 0,5 detik (`IslandInteractable.md`).
- Penyebaran acak dari titik buatan tangan, 5 barang (`RunSeedManager.md`, `GameBalanceConfig.md`). Keduanya sudah tercatat sebagai usulan di dokumen itu.

**Usulan yang menunggu developer (dicatat di #85):**

1. **Dua suku cadang tambahan:** rekomendasi Baling-baling Ekor dan Panel Kendali, untuk mengisi "dll" di GDD.
2. **Pengali kecepatan per barang:** rekomendasi semua 0,5 pada versi awal, sesuai `Claude.md`. Medan `speedMultiplierOverride` tersedia kalau Mesin ingin dibuat lebih lambat. `weightKg` tidak memengaruhi angka permainan.
3. **Penjatuh barang:** rekomendasi damage 25 ke atas, ledakan, dan diterkam. Zombie biasa tidak menjatuhkan barang, supaya perjalanan tidak terasa acak.
4. **Masuk air dalam:** rekomendasi barang langsung kembali ke posisi valid terakhir, bukan tenggelam dan diselami.
5. **Penanda:** rekomendasi hanya barang yang sudah terlihat dalam 40 meter, supaya mencari di desa tetap menjadi tantangan. Alternatifnya, semua barang ditandai setelah Diary Bab 3.

## 6. Skenario Tes

1. Mulai run baru, lalu jalankan `layout` di `DevConsole`. **Hasil:** 5 titik berbeda di Region 4, dan barang ada di titik itu.
2. Tahan `E` 0,5 detik pada Mesin. **Hasil:** barang di tangan, kecepatan 2,5 m/s, Sprint dan Jump mati, hotbar redup, dan inventori tidak berubah.
3. Tekan `E` pada barang lain. **Hasil:** ditolak, dan notifikasi `Tangan penuh`.
4. Paksa `Storm` saat membawa, lalu cuaca kembali cerah. **Hasil:** kecepatan 0,5 x pengali badai, lalu kembali 2,5 m/s, tanpa menumpuk.
5. Tekan `Q`. **Hasil:** barang jatuh di depan, kecepatan normal, dan senjata terakhir kembali.
6. Terkena `Zombie_Standard_Hit`. **Hasil:** barang tetap dibawa. Terkena `Zombie_Heavy_Hit`. **Hasil:** barang jatuh.
7. Diterkam Hunter. **Hasil:** barang jatuh saat `PlayerPinnedEvent` terkirim.
8. Berjalan ke air dalam. **Hasil:** barang kembali ke posisi darat terakhir, dan notifikasi muncul.
9. Dorong barang ke jurang lewat `DevConsole`. **Hasil:** dalam 5 detik barang kembali.
10. Letakkan barang, pergi sampai Region 4 dilepas, lalu kembali. **Hasil:** barang berada di posisi yang sama.
11. Bawa barang ke helipad. **Hasil:** `MarkInstalled` berjalan, kecepatan normal, dan HUD menulis `1 / 5`.
12. Simpan saat membawa, lalu muat. **Hasil:** barang tetap di tangan, dan efeknya aktif.
13. Mati lalu mulai run baru. **Hasil:** barang kembali ke titik baru, dan jumlah terpasang 0.
