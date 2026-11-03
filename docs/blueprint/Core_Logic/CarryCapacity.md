# Blueprint: CarryCapacity.cs
**Lokasi:** `docs/blueprint/Core_Logic/CarryCapacity.md`
**Deskripsi Utama:** Perpindahan barang dari slot tangan kapal ke inventori pulau, jumlah slot inventori, tumpukan bahan, dan tingkatan Backpack.
**Issue:** #118
**Rujukan GDD:** Bagian 3 (barang dari Fase 1 dibawa ke Fase 2, dan item awal). Konversi barang wajib ada untuk mendukung GDD. **Tingkatan Backpack adalah usulan, belum ada di GDD,** dan hanya dipakai kalau developer menyetujuinya. Tanpa tas, inventori tetap 15 slot.

## 1. Arsitektur & Variabel Utama

### Pola

`CarryCapacity` adalah komponen di pemain Fase 2, bersama `InventoryManager`. `InventoryManager` tetap memegang slot, dan `CarryCapacity` menentukan jumlah slot serta konversi barang dari kapal.

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `baseSlots` | `int` | `carry.baseSlots` | Slot tanpa tas |
| `equippedBag` | `ItemInstance` | `null` | Tas yang dipakai. Tidak memakai slot inventori |
| `CurrentCapacity` | `int` | 15 | `baseSlots` ditambah bonus tas |

Tambahan pada `ItemData`:

| Field | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `bagBonusSlots` | `int` | 0 | Bonus slot. Lebih dari 0 berarti item adalah tas |
| `shipWeight` | `int` | 1 | Berat saat diambil di kapal. Sumber `ShipInteractable.weightCost` |
| `shipPickupAmount` | `int` | 1 | Jumlah item yang didapat dari satu objek di kapal |

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.carry.baseSlots` | 15 | Dari `InventoryManager.md` |
| `GameBalanceConfig.carry.woodStack` | 100 | Tumpukan kayu per slot |
| `GameBalanceConfig.carry.stoneStack` | 100 | Tumpukan batu per slot |
| `GameBalanceConfig.carry.materialStack` | 20 | Tumpukan bahan lain, seperti getah, jamur, dan kain |
| `GameBalanceConfig.carry.foodStack` | 5 | Tumpukan makanan dan minuman |
| `GameBalanceConfig.carry.ammoStack` | 60 | Tumpukan peluru |
| `GameBalanceConfig.carry.overflowSpacing` | 0,6 | Jarak `WorldItem` sisa di pantai |

Nilai tumpukan ini mengisi `ItemData.maxStack` (#66).

### Tingkatan Backpack (usulan)

| Tingkat | ID | Bonus slot | Total | Cara mendapat | Bahan | Lokasi |
|---|---|---|---|---|---|---|
| 0 | Tanpa tas | 0 | 15 | Sejak awal | Tidak ada | Tidak ada |
| 1 | `Bag_Cloth` | +5 | 20 | Crafting tangan (#77) | 6 kain atau serat, 2 tali | Bahan dari kapal dan Region 1 |
| 2 | `Bag_Leather` | +10 | 25 | Crafting di meja (#77) | 6 kulit hewan (#61), 4 tali, 1 `Bag_Cloth` | Kulit hewan Region 1 dan Region 2 |
| 3 | `Bag_Military` | +15 | 30 | Ditemukan, tidak bisa dibuat | Tidak ada | Peti loot Pangkalan Militer di Region 3 (#103) |

Bonus tidak bertambah dari beberapa tas sekaligus. Hanya tas yang dipakai yang berlaku.

## 2. Detail Logika & Fungsi

### Konversi dari kapal ke pulau

**Alur data:**

```text
ShipInteractable.Interact    ->  InventoryManager.AddShipHandItem(itemId, weight, amount)
LifeboatExtraction (#74)     ->  GameManager.savedShipLoot (daftar SavedItemData { itemId, amount })
IslandArrival (#74)          ->  CarryCapacity.ApplyShipLoot(savedShipLoot)
```

`SavedItemData` tidak memuat berat. Berat hanya berlaku di kapal, dan satu objek kapal berubah menjadi `shipPickupAmount` item.

**Aturan konversi:**

| Barang di kapal | Berat | Menjadi di pulau | Jumlah | Slot di pulau |
|---|---|---|---|---|
| Peluru (`Ammo_9mm`) | 1 | `Ammo_9mm` | 12 | Bertumpuk, 60 per slot |
| Makanan kaleng (`Food_Can`) | 1 | `Food_Can` | 2 | Bertumpuk, 5 per slot |
| Air Minum Bersih (`Drink_CleanWater`) | 1 | `Drink_CleanWater` | 2 | Bertumpuk, 5 per slot |
| Vaksin (`Med_Vaccine`) | 1 | `Med_Vaccine` | 1 | Bertumpuk, 5 per slot |
| Kotak obat (`Med_Medkit`) | 1 | `Med_Medkit` | 1 | Bertumpuk, 5 per slot |
| Peralatan, seperti kain dan tali (`Mat_*`) | 1 | Item yang sama | 4 | Bertumpuk, 20 per slot |
| Senjata atau alat besar | 2 | Item yang sama, dengan `ItemInstance` baru | 1 | Satu slot per item |
| NPC (`npc_*`) | 4 | **Tidak masuk inventori** | Tidak ada | Dicatat di `rescuedNpcIds`, lalu menjadi NPC di pantai (#76) |

Berat barang mengikuti `ShipInteractable.md`: makanan 1, senjata 2, dan NPC 4. Daftar barang kapal final ada di #103.

**`public void ApplyShipLoot(List<SavedItemData> loot)`**, dipanggil `IslandArrival` setelah item inti masuk:

1. Urutkan: item inti lebih dulu, lalu vaksin awal (#64), lalu sisa `loot` sesuai urutan disetor.
2. Tiap item masuk lewat `InventoryManager.AddItem(ItemData, amount)`. Item baru mendapat `ItemInstance` dengan `createdGameHour` saat ini.
3. Sisa yang tidak muat masuk daftar `overflow`.
4. `IslandArrival` memunculkan `overflow` di pantai sebagai `WorldItem` dengan `despawns: false`, berjarak `overflowSpacing`.

Item inti (`Base_Kail`, `Base_Pistol`, dan `Base_Pisau`) tidak pernah menjadi sisa, karena masuk paling awal, dan selalu ditambahkan terlepas dari isi `loot`, sesuai GDD bagian 3.

### Kapasitas

**`public void SetBag(ItemInstance bag)`**:

1. Hitung kapasitas dengan tas baru. Kalau `bag` bernilai `null` berarti melepas tas. Kalau kapasitas itu lebih kecil dari jumlah slot terisi, tampilkan pesan dan tolak.
2. Keluarkan tas lama ke slot inventori kalau ada. Kalau tidak muat, tolak.
3. `equippedBag = bag`, hapus tas baru dari slot inventori, dan panggil `RecalculateCapacity()`.

**`private void RecalculateCapacity()`**:

```csharp
int bonus = equippedBag != null ? ItemDatabase.Get(equippedBag.itemId).bagBonusSlots : 0;
int newCapacity = balance.carry.baseSlots + bonus;
inventory.SetMaxSlots(newCapacity);
CurrentCapacity = newCapacity;
```

**`InventoryManager.SetMaxSlots(int value)`**:

- **Naik:** tambah slot kosong di akhir, lalu `UpdateUI()`.
- **Turun:** isi slot di atas batas dipindah ke slot kosong di bawah batas. Sisanya yang tidak muat menjadi `WorldItem` di depan pemain, lalu tampil notifikasi `Beberapa barang terjatuh` (#22).

### Tas hilang atau kapasitas turun

| Kejadian | Akibat |
|---|---|
| Melepas tas lewat UI saat slot tambahan terisi | **Ditolak,** dengan pesan `Kosongkan tas dulu`. Pemain tidak kehilangan barang tanpa sengaja |
| Mengganti tas dengan tingkat lebih rendah | Ditolak dengan cara yang sama, kalau isi tidak muat |
| Tas lepas karena sistem lain, misalnya perintah `DevConsole` | `SetMaxSlots` turun, dan sisanya jatuh sebagai `WorldItem` |
| Memuat save dengan `itemId` tas yang tidak ada | `SetMaxSlots` turun, dan sisanya jatuh. Peringatan dicatat (#80) |
| Membuang tas | `Bag_*` di slot inventori boleh dibuang. Tas yang dipakai harus dilepas dulu |

Tas yang dipakai tidak bisa hancur, karena tidak punya durabilitas.

### Slot tas di UI dan hotbar

- **Panel `Inventory`:** satu kotak khusus berlabel Tas, di atas grid. Seret tas ke kotak itu, atau pakai aksi `Pakai Tas` di menu item (#66). Kotak menampilkan bonus slot.
- **Grid:** slot tambahan muncul di bawah, dengan bingkai berbeda per tingkat.
- **Hotbar (#60):** tas tidak bisa masuk ke hotbar, karena `equipPrefab` bernilai `null`.
- **HUD:** tidak ada tampilan khusus. Slot terisi tampil saat panel terbuka.

### Beban dan kecepatan

**Rekomendasi: inventori tidak memperlambat pemain.** Slot sudah menjadi batas. Satu-satunya perlambatan berat ada di Heavy Item (#69), yang berada di luar inventori. Dengan begitu, target GDD tetap satu tantangan yang jelas, dan pemain tidak menghitung berat kayu.

### Kayu dan batu

**Rekomendasi: gabungan keduanya.** Kayu dan batu punya tumpukan besar (100 per slot), dan `BridgeSite` (#68) menerima setoran bertahap, tanpa syarat 500 kayu sekaligus di tangan. `Claude.md` mencek `Kayu >= 500`, dan #68 mengubahnya menjadi jumlah kumulatif.

### Hitungan terhadap target #84

**Slot yang biasanya terpakai di tengah permainan, tanpa tas:**

| Isi | Slot |
|---|---|
| `Base_Kail`, `Base_Pistol`, dan `Base_Pisau` | 3 |
| Kapak, Beliung, Palu, dan Obor | 4 |
| Vaksin dan obat | 1 |
| Peluru | 1 |
| Makanan, 2 jenis | 2 |
| Minuman, 2 jenis | 2 |
| **Jumlah terpakai** | **13** |
| **Sisa untuk bahan** | **2** |

**Hasilnya terhadap target `GameBalanceConfig.md`:**

| Target | Kebutuhan | Tanpa tas (2 slot) | Dengan `Bag_Cloth` (7 slot) |
|---|---|---|---|
| Base awal, sekitar 80 kayu | 1 slot | Muat | Muat |
| Satu perjalanan menebang | 200 kayu, 20 pohon, sekitar 500 detik | Muat | Masih ada 5 slot |
| Jembatan, 500 kayu | 5 slot | 3 kali setor ke `BridgeSite` | 1 kali setor |
| Satu dosis vaksin, 2 getah dan 2 jamur | 2 slot | Pas, tanpa ruang kayu | Muat |
| Suku cadang (#69) | Di luar inventori | Tidak terpengaruh | Tidak terpengaruh |
| Fase 1 ke Fase 2 | Sekitar 16 berat, 4 kali bolak-balik | Sebagian besar muat, sisanya di pantai | Masih ada ruang |

**Kesimpulan:** inventori 15 slot cukup untuk target jembatan dan base dengan setoran bertahap. Peti (#46) dan tas memberi kenyamanan, bukan syarat menang. **Kemenangan tidak pernah bergantung pada tas,** sehingga tas aman ditolak developer.

### Simpan dan muat

```csharp
[Serializable]
public struct CarryCapacitySaveData
{
    public ItemInstance equippedBag;   // null kalau tanpa tas
}
```

`SaveId = player_carry`. `RestoreState` berjalan **sebelum** isi slot `InventoryManager` dipulihkan, supaya `SetMaxSlots` sudah benar dan slot tambahan tidak terbuang. `CurrentCapacity` tidak disimpan, karena dihitung ulang.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `InventoryCapacityChangedEvent { capacity }`, sticky, untuk panel inventori.

**Event yang didengar:** tidak ada.

| Sistem | Hubungan |
|---|---|
| `InventoryManager` | Menyediakan `SetMaxSlots` dan `AddItem` |
| `ItemData`, `ItemUseSystem` (#66) | Field tas, `maxStack`, dan aksi Pakai Tas |
| `EquipmentManager` (#60) | Tas tidak masuk hotbar |
| `LifeboatExtraction`, `IslandArrival` (#74) | Mengirim isi `savedShipLoot` |
| `ShipInteractable` | Membaca `shipWeight` dan `shipPickupAmount` |
| `NPCRoster` (#76) | Menerima NPC dari kapal |
| `StorageChest` (#46) | Peti tidak terpengaruh tas |
| `HeavyItemSystem` (#69) | Di luar inventori |
| `CraftingStation` (#77), `RegionLootTable` (#103) | Resep tas dan lokasi `Bag_Military` |
| `RegionGateSystem` (#68) | Setoran kayu bertahap |
| `WorldStateSave` (#80) | Menyimpan `equippedBag` |
| `NotificationManager` (#22) | Pesan |

**Batas pekerjaan:** sistem inventori resmi di #85, peti di #46, dan Heavy Item di #69.

## 4. Kasus Khusus

- **Objek pool:** barang yang jatuh karena kapasitas turun memakai `WorldItem` dari pool (#66). Tidak ada objek lain di pool.
- **Game dijeda:** `Inventory` tidak menjeda (#83), jadi mengganti tas tetap memakan waktu nyata. Saat dijeda, panel tidak terbuka.
- **Permadeath:** tas, slot, dan isi terhapus bersama save run (#82). Run baru kembali ke 15 slot tanpa tas.
- **Save dan load:** tas disimpan, dan kapasitas dihitung ulang. Lihat "Simpan dan muat".
- **Fase 1 ke Fase 2:** bonus tas tidak ada di kapal. Tas tidak bisa didapat dari kapal, supaya hitungan slot awal selalu 15.
- **Isi `savedShipLoot` melebihi slot:** sisanya berada di pantai, dan tidak hilang.
- **Tas diambil saat kapasitas penuh:** masuk ke slot inventori kalau ada ruang. Kalau penuh, tetap di tanah.
- **Item ber-`maxStack` berbeda dari versi lama:** tumpukan lebih besar dari `maxStack` dipecah saat load (#80).
- **`Ammo_9mm` di kapal melebihi tumpukan:** `AddItem` membagi ke beberapa slot.
- **NPC diangkut lalu tidak disetor:** menjadi korban (#74), dan tidak dikonversi.
- **Slot tambahan aktif saat panel tertutup:** `InventoryCapacityChangedEvent` tetap memperbarui data, dan panel membaca saat dibuka.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** satu `InventoryManager` untuk dua fase, dan `GameManager.savedShipLoot` (`GLOSSARY.md`). Berat makanan 1, senjata 2, dan NPC 4 (`ShipInteractable.md`). Item inti selalu dibawa (GDD bagian 3).

**Keputusan teknis di blueprint ini:**

- `CarryCapacity` menentukan jumlah slot, dan `InventoryManager` tetap pemilik isi slot.
- Kapasitas dipulihkan lebih dulu sebelum isi slot.
- Berat tidak dibawa ke data pulau.

**Usulan yang disetujui developer sebagai nilai awal (dicatat di #85):**

1. **Tiga tingkat Backpack:** +5, +10, dan +15 slot. Rekomendasi: **dipakai**, karena memberi hadiah eksplorasi tanpa memengaruhi syarat menang. Kalau ditolak, semua aturan tas dihapus, dan konversi tetap berlaku.
2. **Melepas tas:** ditolak kalau slot tambahan masih terisi.
3. **Slot tas:** kotak khusus di panel `Inventory`, bukan di hotbar.
4. **Berat inventori:** tidak memperlambat pemain.
5. **Kayu dan batu:** tumpukan 100 per slot, ditambah setoran bertahap di `BridgeSite`.
6. **Hasil barang kapal:** satu objek peluru menjadi 12, makanan kaleng dan air menjadi 2, dan kain atau tali menjadi 4.
7. **Barang sisa di pantai:** tidak hilang otomatis.

## 6. Skenario Tes

1. Selesaikan Fase 1 dengan 3 peluru, 1 makanan, dan 1 NPC. **Hasil:** inventori berisi item inti, 36 `Ammo_9mm`, dan 2 `Food_Can`. NPC muncul di pantai, bukan di slot.
2. Selesaikan Fase 1 tanpa barang. **Hasil:** inventori tetap berisi `Base_Kail`, `Base_Pistol`, dan `Base_Pisau`.
3. Paksa `savedShipLoot` berisi 30 jenis barang lewat `DevConsole`. **Hasil:** 15 slot terisi, dan sisanya berada di pantai sebagai `WorldItem`.
4. Ambil `WorldItem` sisa di pantai setelah 3 hari game. **Hasil:** barang masih ada.
5. Tebang kayu sampai 250. **Hasil:** tiga slot terpakai, yaitu 100, 100, dan 50.
6. Pakai `Bag_Cloth`. **Hasil:** 5 slot baru muncul.
7. Isi slot tambahan, lalu lepas tas. **Hasil:** ditolak dengan pesan, dan barang tetap.
8. Paksa lepas lewat `DevConsole`. **Hasil:** barang pindah ke slot kosong, dan sisanya jatuh dengan notifikasi.
9. Coba seret tas ke hotbar. **Hasil:** ditolak.
10. Kenakan `Bag_Leather` saat `Bag_Cloth` terpasang. **Hasil:** `Bag_Cloth` kembali ke slot inventori, atau ditolak kalau tidak muat.
11. Simpan dengan tas dan slot tambahan terisi, keluar, lalu lanjutkan. **Hasil:** kapasitas dan isi sama.
12. Setor kayu ke `BridgeSite` tiga kali. **Hasil:** progres 500 tercapai tanpa pernah membawa 500 kayu.
13. Berjalan dengan inventori penuh dan kosong. **Hasil:** kecepatan sama.
14. Mati, lalu mulai run baru. **Hasil:** 15 slot, tanpa tas.
