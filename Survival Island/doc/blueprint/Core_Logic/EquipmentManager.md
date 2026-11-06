# Blueprint: EquipmentManager.cs
**Lokasi:** `Survival Island/doc/blueprint/Core_Logic/EquipmentManager.md`
**Deskripsi Utama:** Pengatur satu item aktif di tangan pemain dan hotbar. Hanya item aktif yang menerima input klik kiri dan klik kanan, jadi senjata, alat, dan palu bangun tidak saling bentrok.
**Issue:** #60
**Rujukan GDD:** Bagian 3 (`Base_Kail`, `Base_Pistol`, dan `Base_Pisau`), bagian 5 (menebang, menambang, dan obor di Region 3), dan bagian 7 (senjata hadiah Lucky Machine). Jumlah slot hotbar dan aturan tangan kiri adalah usulan, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

### Pola

`EquipmentManager` berada di scene `IslandPhase` (#82). Fase 1 tidak memakainya, karena Pistol dan Pisau tidak dipakai di kapal (#75).

Hotbar **berisi rujukan**, bukan salinan barang. Barang tetap berada di slot `InventoryManager`, dan hotbar menyimpan `ItemInstance.instanceId` (#66). Dengan begitu, durabilitas dan isi magazine hanya punya satu sumber.

```csharp
public interface IEquippable
{
    void OnEquip(ItemInstance instance);  // Item masuk ke tangan, setelah animasi naik mulai
    void OnUnequip();                     // Membatalkan reload, ayunan, dan cast
    void PrimaryAction(ActionPhase phase);
    void SecondaryAction(ActionPhase phase);
    bool CanSwitchNow { get; }            // false pada saat momen damage sedang berjalan
}

public enum ActionPhase { Pressed, Held, Released }
public enum HandUse { OneHand, TwoHand, OffHand }

[Flags]
public enum EquipLock { None = 0, Menu = 1, HeavyItem = 2, Climbing = 4, Pinned = 8, Swimming = 16, Dead = 32 }
```

`EquippableBehaviour : MonoBehaviour, IEquippable` adalah kelas dasar komponen item. Fungsi yang tidak dipakai item boleh dibiarkan kosong.

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Instance` | `EquipmentManager` | `null` | Singleton per scene |
| `hotbar` | `int[]` | Panjang `hotbarSlotCount`, isi -1 | `instanceId` per slot hotbar |
| `activeHotbarIndex` | `int` | 1, slot `Base_Pistol` | Slot aktif. -1 berarti tangan kosong |
| `offHandInstanceId` | `int` | -1 | Senter atau obor di tangan kiri |
| `Active` | `IEquippable` | `fistBehaviour` | Item yang menerima input |
| `state` | `EquipState` | `Idle` | `Idle`, `Lowering`, atau `Raising` |
| `pendingIndex` | `int` | -2 | Pilihan terakhir saat sedang berganti. -2 berarti kosong |
| `locks` | `EquipLock` | `None` | Kunci pergantian dan input |
| `behaviourCache` | `Dictionary<string, EquippableBehaviour>` | Kosong | Viewmodel per `itemId`, dibuat sekali lalu dinyalakan dan dimatikan |
| `fistBehaviour` | `EquippableBehaviour` | Dari prefab | Tangan kosong |

### Tambahan pada `ItemData`

| Field | Tipe | Fungsi |
|---|---|---|
| `equipPrefab` | `EquippableBehaviour` | Prefab viewmodel. `null` berarti tidak bisa dipegang |
| `handUse` | `HandUse` | `OneHand`, `TwoHand`, atau `OffHand` |

### Daftar item yang bisa dipegang

| Item | ID | Pemakaian tangan | Komponen | Klik kiri | Klik kanan |
|---|---|---|---|---|---|
| Kail | `Base_Kail` | `TwoHand` | `FishingSystem` (#55) | Lempar dan tarik | Batal |
| Pistol | `Base_Pistol` | `OneHand` | `GunController` (#58) | Tembak | Tidak dipakai |
| Pisau | `Base_Pisau` | `OneHand` | `MeleeCombatSystem` | Tusuk | Tidak dipakai |
| Kapak | `Tool_Axe` | `TwoHand` | `MeleeCombatSystem` | Tebang dan serang | Tidak dipakai |
| Beliung | `Tool_Pickaxe` | `TwoHand` | `MeleeCombatSystem` | Tambang dan serang | Tidak dipakai |
| Palu | `Tool_Hammer` | `OneHand` | `HammerEquippable`, diteruskan ke `BuildManager` dan `BuildingIntegrity` (#79) | Sesuai `HammerMode`: Perbaiki (awal), Bangun, Upgrade, atau Bongkar | Menu radial (#79) |
| Obor | `Tool_Torch` | `OffHand` | `TorchController` (#65) | Tidak dipakai | Tidak dipakai |
| Senter | `Tool_Flashlight` | `OffHand` | `FlashlightController` | Tidak dipakai | Tidak dipakai |
| Laser Pistol | `Reward_LaserPistol` | `OneHand` | `GunController` dengan `WeaponData` lain | Tembak | Tidak dipakai |
| Fire Axe | `Reward_FireAxe` | `TwoHand` | `MeleeCombatSystem`, tanpa `DurabilitySystem` | Serang | Tidak dipakai |
| Tangan kosong | Tidak ada | `OneHand` | `FistBehaviour` | Pukul | Tidak dipakai |
| Makanan, minuman, atau obat di hotbar | Sesuai data | `OneHand` | `ConsumableEquippable` | Pakai lewat #66 | Batal |

ID selain tiga item inti adalah usulan, dan dipakai sampai #77 serta #103 membuat daftar final. Tombol `R` dibaca langsung oleh `GunController`, dan `BuildRotate` oleh `BuildManager`, saat komponennya aktif.

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.equipment.hotbarSlotCount` | 8 | Sama dengan delapan item utama di tabel, dan tombol `1` sampai `8` (#18) |
| `GameBalanceConfig.equipment.lowerSeconds` | 0,15 | Waktu menurunkan item |
| `GameBalanceConfig.equipment.raiseSeconds` | 0,2 | Waktu mengangkat item, dengan input dimatikan |
| `GameBalanceConfig.equipment.offHandAllowedWhileCarrying` | `false` | Obor saat membawa Heavy Item. Final di #65 |

## 2. Detail Logika & Fungsi

### Membaca input

`EquipmentManager.Update()` adalah satu-satunya pembaca `PrimaryAction` dan `SecondaryAction`. `MeleeCombatSystem` tidak lagi membaca klik kiri di `Update()`.

```csharp
void Update()
{
    if (state != EquipState.Idle) TickSwitch();   // Tetap selesai walau panel tanpa jeda terbuka
    if (locks != EquipLock.None) return;          // Kunci Menu mengikuti IsGameplayInputBlocked
    ReadHotbarInput();
    if (state != EquipState.Idle) return;

    var input = InputManager.Instance;
    if (DurabilitySystem.GetState(ActiveInstance) == DurabilityState.Broken)
    {
        if (input.PrimaryPressed) RejectBroken();   // Suara dan notifikasi. Item tetap di hotbar (#113)
    }
    else if (input.PrimaryPressed) Active.PrimaryAction(ActionPhase.Pressed);
    else if (input.PrimaryHeld) Active.PrimaryAction(ActionPhase.Held);
    else if (input.PrimaryReleased) Active.PrimaryAction(ActionPhase.Released);
    // SecondaryAction mengikuti pola yang sama
}
```

Rincian tiap kunci ada di tabel kunci.

**`public void RefreshEquipped()`** membaca ulang modifier, dan mengganti material viewmodel dari `ItemInstance.skinId` (#72).

### Hotbar

**Isi awal saat tiba di pulau (`BuildDefaultHotbar`):** slot 1 `Base_Kail`, slot 2 `Base_Pistol`, slot 3 `Base_Pisau`. Item aktif pertama adalah `Base_Pistol`, supaya pemain langsung bisa bertahan.

**`private void ReadHotbarInput()`**:

- `HotbarSelect` bernilai `n`: panggil `RequestSlot(n - 1)`. Menekan tombol slot yang sedang aktif beralih ke tangan kosong.
- `HotbarScroll`: cari slot terisi berikutnya atau sebelumnya, dengan melewati slot kosong dan berputar. Nilai scroll tidak dikali waktu, dan dilewati kalau nilainya 0.

**`public void AssignToHotbar(int hotbarIndex, int instanceId)`**, dipanggil dari aksi item di #66:

1. Tolak kalau item tidak punya `equipPrefab`, atau item ber-`handUse` `OffHand`. Item `OffHand` memakai `EquipOffHand`.
2. Kalau item sudah berada di slot lain, kosongkan slot lain itu.
3. Isi slot, lalu terbitkan `HotbarChangedEvent`.
4. Kalau slot itu aktif, ganti item di tangan.

**Sinkronisasi dengan `InventoryManager`:** `EquipmentManager` mendengar `InventoryChangedEvent`. Untuk tiap slot hotbar, kalau `instanceId` tidak lagi ada di inventori, slot dikosongkan dan `HotbarChangedEvent` terkirim. Kalau slot itu aktif, jalankan alur item hilang.

### Berganti item

**`public void RequestSlot(int index)`**:

1. Kalau `locks != EquipLock.None`, tolak.
2. Kalau `state != Idle`, simpan `pendingIndex = index`, lalu selesai. Pemain yang menekan `1`, `2`, `3` dengan cepat hanya mendapat pilihan terakhir.
3. Kalau `!Active.CanSwitchNow`, simpan `pendingIndex = index`. `TickSwitch()` mencoba lagi pada frame berikutnya.
4. Panggil `Active.OnUnequip()`, sembunyikan item lewat `state = Lowering`, dan mulai hitung `lowerSeconds`.

**`TickSwitch()`**: setelah `lowerSeconds`, matikan viewmodel lama, nyalakan viewmodel baru, panggil `OnEquip(instance)`, terapkan aturan tangan kiri, lalu `state = Raising`. Setelah `raiseSeconds`, `state = Idle`. Kalau `pendingIndex` terisi, panggil `RequestSlot(pendingIndex)` dan kosongkan nilainya. Saat berganti selesai, terbitkan `EquippedItemChangedEvent { itemId }`.

**Pembatalan:**

| Aksi yang berjalan | Saat berganti |
|---|---|
| Reload | Batal di `OnUnequip`. Peluru cadangan tidak berkurang (#58) |
| Ayunan melee pada ancang-ancang | Batal di `OnUnequip` |
| Ayunan melee pada momen damage | `CanSwitchNow = false` sampai selesai, paling lama 0,2 detik |
| Cast kail | Batal, dan kail ditarik kembali (#55) |
| Hologram bangun | Disembunyikan (#79) |
| Pemakaian konsumsi | Batal tanpa mengurangi item (#66) |

### Tangan kiri

Satu slot `offHandInstanceId` untuk Senter atau Obor. Tombol `F` memanggil `ToggleOffHand()`, dengan item terakhir yang dipakai. Pemilihan item lewat aksi `Pegang di Tangan Kiri` di inventori (#66).

| Item utama | Tangan kiri | Akibat |
|---|---|---|
| `OneHand`, seperti Pistol, Pisau, dan Palu | Boleh | Senter atau Obor tetap menyala |
| `TwoHand`, seperti Kail, Kapak, dan Beliung | Disimpan otomatis | Efek cahaya dan panas mati. Kembali saat item `OneHand` dipegang lagi |
| Membawa Heavy Item | Mengikuti `offHandAllowedWhileCarrying` | Nilai awal: tidak boleh |

Durasi bakar Obor tetap berjalan saat disimpan otomatis, sesuai #65. `FlashlightController` tidak lagi membaca tombol `F`, dan menerima `OnEquip` serta `OnUnequip` dari `EquipmentManager`.

### Kunci

| Kunci | Pemicu | Yang terkunci |
|---|---|---|
| `Menu` | `UIFocusChangedEvent.isGameplayBlocked` | Semua input item. Item tetap di tangan |
| `HeavyItem` | `HeavyItemChangedEvent.isCarried = true` | Pergantian dan input. Item disimpan, dan tangan dipakai barang |
| `Climbing` | `PlayerTraversal` (#101) | Pergantian dan input. Item diturunkan |
| `Pinned` | `PlayerPinnedEvent.isPinned = true` | Semua input item, kecuali cara lepas di #110 |
| `Swimming` | `SwimmingController` (#13) | Pergantian dan input |
| `Dead` | `PlayerDiedEvent` | Semua |

**`public void PushLock(EquipLock l)` dan `PopLock(EquipLock l)`** mengubah bit `locks`. Saat kunci `HeavyItem`, `Climbing`, atau `Swimming` masuk, panggil `Active.OnUnequip()`, sembunyikan viewmodel, simpan `lastIndex`, dan atur `Active = fistBehaviour` yang disembunyikan. Saat semua kunci itu lepas, kembalikan item terakhir lewat `RequestSlot(lastIndex)`.

### Item aktif hancur atau hilang

**Pemicu:** status `DurabilityState.Destroyed` memanggil `InventoryManager.RemoveInstance(instanceId)`, atau item dijatuhkan dan habis dipakai. Status `Broken` tidak memicu alur ini (#113). `DurabilitySystem` hanya mengurangi `ItemInstance.durability`, dan tidak lagi berada di MonoBehaviour senjata.

**Alur:**

1. Panggil `Active.OnUnequip()`, tanpa animasi turun.
2. Cari slot hotbar terdekat ke kanan yang masih berisi item. Kalau ada, pilih slot itu.
3. Kalau tidak ada, `activeHotbarIndex = -1`, dan `Active = fistBehaviour`.
4. Tampilkan notifikasi `Item rusak` (#22).

Aturan senjata inti yang hancur ada di #58 dan #113. Hasil ini berlaku untuk item apa pun.

### Tampilan hotbar di HUD

`HotbarView` adalah bagian dari `HUDManager`, tanpa `UIPanel`, karena bukan panel fokus.

- Menampilkan delapan kotak bernomor, ikon, jumlah tumpukan, dan bar durabilitas.
- Kotak aktif diberi bingkai. Selama `state != Idle`, kotak tujuan berkedip.
- Kotak tangan kiri terpisah, di sisi kiri hotbar.
- Kunci `HeavyItem` meredupkan seluruh hotbar.
- Hanya memperbarui saat `HotbarChangedEvent`, `EquippedItemChangedEvent`, dan `InventoryChangedEvent` terkirim, bukan setiap frame.

### Simpan dan muat

```csharp
[Serializable]
public struct EquipmentSaveData
{
    public int[] hotbarInstanceIds;
    public int activeHotbarIndex;
    public int offHandInstanceId;
    public bool offHandActive;
}
```

`SaveId = player_equipment`. `RestoreState` berjalan setelah `InventoryManager`, supaya `instanceId` sudah bisa dicari. Rujukan yang tidak ditemukan dikosongkan. Item aktif dipasang tanpa animasi, lalu `EquippedItemChangedEvent` dikirim ulang.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `EquippedItemChangedEvent`, `HotbarChangedEvent`.

**Event yang didengar:** `UIFocusChangedEvent`, `InventoryChangedEvent`, `HeavyItemChangedEvent`, `PlayerPinnedEvent`, `PlayerDiedEvent`.

| Sistem | Hubungan |
|---|---|
| `InputManager` (#18) | Menyediakan `PrimaryPressed`, `PrimaryHeld`, `PrimaryReleased`, `SecondaryHeld`, dan hotbar |
| `InventoryManager`, `ItemUseSystem` (#66) | Pemilik `ItemInstance`, dan aksi memindahkan ke hotbar |
| `GunController` (#58), `MeleeCombatSystem`, `FishingSystem` (#55), #79 | Menjadi `EquippableBehaviour` |
| `HeavyItemSystem` (#69) | Mengirim `HeavyItemChangedEvent` |
| `WarmthSystem` (#65) | Mengatur Obor di tangan kiri |
| `PlayerTraversal` (#101), `SwimmingController` (#13) | Memakai `PushLock` dan `PopLock` |
| `FirstPersonCamera` (#102) | Memegang `ViewmodelRoot`, dan pose tiap item |
| `CarryCapacity` (#118) | Tas tidak masuk hotbar |
| `AnimationController` (#12) | Membaca `EquippedItemChangedEvent` untuk animasi |

**Batas pekerjaan:** pemetaan tombol di #18, animator di #12, dan logika tiap senjata di blueprint masing-masing.

## 4. Kasus Khusus

- **Objek pool:** viewmodel tidak memakai `ObjectPooler`, karena jumlahnya tetap dan hanya dinyalakan atau dimatikan. `behaviourCache` hilang bersama scene.
- **Game dijeda:** `Update` tetap berjalan, tetapi input `Gameplay` mati, dan hitungan pergantian memakai `Time.deltaTime` yang bernilai 0. Pergantian berlanjut setelah jeda.
- **Permadeath:** isi hotbar ikut terhapus bersama save run (#82). Run baru membangun `BuildDefaultHotbar` lagi.
- **Save dan load:** susunan hotbar, item aktif, dan tangan kiri disimpan. `state` dan `pendingIndex` tidak disimpan.
- **Panel tanpa jeda terbuka:** `Gameplay` mati, jadi pemain tidak bisa menembak. Pergantian yang sedang berjalan tetap selesai.
- **Item dipindah ke peti saat aktif:** `InventoryChangedEvent` mengosongkan rujukan dan memicu alur item hilang.
- **Tumpukan konsumsi habis di tangan:** alur item hilang berjalan, tanpa notifikasi `Item rusak`.
- **Pistol tanpa peluru:** tetap di tangan. Klik kosong diatur #58.
- **Tombol slot kosong:** ditolak dengan suara klik, dan item aktif tidak berubah.
- **Heavy Item jatuh saat diserang:** `PopLock(HeavyItem)` mengembalikan item terakhir.
- **Mati saat berganti:** kunci `Dead` menghentikan semua, dan #53 mengambil alih kamera.
- **Dua sistem memakai kunci yang sama:** `Pinned` saat `HeavyItem` tetap berlaku bersama. Setiap bit dilepas pemiliknya.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** klik kiri dipakai item aktif (`GLOSSARY.md`). Item inti `Base_Kail`, `Base_Pistol`, dan `Base_Pisau` tidak bisa dibuang. Tombol `1` sampai `8` sebagai nilai sementara (#18).

**Keputusan teknis di blueprint ini:**

- Hotbar menyimpan `instanceId`, bukan barang.
- `EquipmentManager` satu-satunya pembaca klik kiri dan klik kanan untuk item.
- Viewmodel dibuat sekali per item, lalu dinyalakan dan dimatikan.

**Usulan yang disetujui developer sebagai nilai awal (dicatat di #85):**

1. **Jumlah slot hotbar:** 8, supaya semua item utama muat.
2. **Isi hotbar:** merujuk ke slot inventori, sehingga 8 slot hotbar tidak mengurangi 15 slot inventori.
3. **Tangan kiri:** satu slot untuk Senter atau Obor. Keduanya bisa dipakai bersama Pistol, Pisau, dan Palu, tetapi tidak dengan Kail, Kapak, dan Beliung.
4. **Jeda ganti item:** 0,35 detik total.
5. **Item aktif hancur:** beralih ke slot terisi berikutnya, lalu tangan kosong kalau tidak ada.
6. **Konsumsi di hotbar:** boleh, dengan klik kiri untuk memakai.
7. **Tangan kosong:** menekan tombol slot aktif menyimpan item, dan klik kiri menjadi pukulan lemah.

## 6. Skenario Tes

1. Mulai di pulau. **Hasil:** hotbar berisi Kail, Pistol, dan Pisau, dengan Pistol aktif.
2. Tekan `1`, `2`, dan `3` secepat mungkin. **Hasil:** item berakhir di slot 3, tanpa ada aksi ganda.
3. Klik kiri saat memegang Pistol, lalu saat memegang Kail. **Hasil:** hanya item aktif yang bereaksi.
4. Reload Pistol, lalu tekan `3` di tengah reload. **Hasil:** reload batal, dan peluru cadangan tidak berkurang.
5. Scroll mouse ke atas dan ke bawah dengan dua slot kosong di antara. **Hasil:** slot kosong terlewati.
6. Buka inventori, lalu klik kiri pada slot. **Hasil:** Pistol tidak menembak.
7. Pindahkan Kapak ke slot 4 lewat menu item, lalu tekan `4`. **Hasil:** Kapak di tangan, dan masih terhitung satu slot inventori.
8. Pegang Pistol dengan Obor di tangan kiri. **Hasil:** keduanya tampil. Ganti ke Kapak. **Hasil:** Obor disimpan. Kembali ke Pistol. **Hasil:** Obor kembali.
9. Rusak Kapak aktif lewat `DevConsole`. **Hasil:** item beralih ke slot berikutnya, notifikasi muncul, dan slot Kapak kosong.
10. Ambil Heavy Item. **Hasil:** item disimpan, tombol hotbar diabaikan, dan hotbar redup. Lepas barang. **Hasil:** item terakhir kembali.
11. Simpan dengan susunan khusus dan Kapak aktif, keluar, lalu lanjutkan. **Hasil:** susunan dan item aktif sama, dan durabilitas tidak berubah.
12. Jatuhkan item yang ada di hotbar. **Hasil:** rujukan hilang, dan kotak kosong.
13. Mati, lalu mulai run baru. **Hasil:** hotbar kembali ke isi awal.
14. Diam memegang Pistol dengan Profiler terbuka. **Hasil:** `Update` tanpa alokasi GC.

## 7. Catatan Implementasi (Langkah 3)

Kode: `Survival Island/Assets/LOGIC/Core_Logic/EquipmentManager.cs`, `EquippableBehaviour.cs` (`IEquippable`, `ActionPhase`), `ConsumableEquippable.cs`, `FistBehaviour.cs`. `HandUse` berada di `ItemData.cs`.

- **`BuildDefaultHotbar`** dijalankan otomatis pada `InventoryChangedEvent` pertama yang membuat `Base_Pistol` ada (setelah `IslandArrival.Apply` menambah item inti). Penanda `defaultHotbarBuilt` ditambahkan ke `EquipmentSaveData`, supaya hotbar awal tidak dibangun ulang setelah load.
- **`LastUnequippedInstanceId`:** properti tambahan, dibaca `ItemUseSystem` untuk membatalkan pemakaian hanya kalau item yang dipakai adalah item di tangan.
- **Konsumsi di hotbar:** makanan tanpa `equipPrefab` memakai satu komponen `consumableBehaviour` bersama (field Inspector).
- `RequestSlot(-1)` berarti tangan kosong. Properti baca tambahan: `ActiveInstanceId`, `ActiveInstance`, `ActiveHotbarIndex`, `GetHotbarInstanceId`, `OffHandInstanceId`, `OffHandActive`, `State`, `Locks`.
- Belum ada: cek `DurabilitySystem.GetState == Broken` (#113), notifikasi `Item rusak` (#22), dan suara klik slot kosong. Pukulan `FistBehaviour` menunggu `MeleeCombatSystem` (Langkah 5).

## 8. Catatan Implementasi (Sistem pendukung)

- **Keputusan developer (7 November 2026):** tangan kosong (`FistBehaviour`) memukul lewat jalur `MeleeCombatSystem` dengan damage `equipment.fistDamage` (nilai awal 5).
