# Blueprint: RegionLootTable.cs (LootTable)
**Lokasi:** `docs/blueprint/Core_Logic/RegionLootTable.md`
**Deskripsi Utama:** Isi loot per lokasi dan jenis titik, daftar senjata dan peluru, skala terhadap hari, serta rumus bobot yang menggantikan `dropChance` di `LootSpawnerSystem`.
**Issue:** #103
**Rujukan GDD:** Bagian 3 (peluru dan makanan di kapal, looting), bagian 2 (jejak militer), bagian 5 (Pangkalan Militer di Region 3, desa di Region 4), dan bagian 7 (senjata Lucky Machine). Isi tabel, senjata baru, dan semua angka adalah usulan.

## 1. Arsitektur & Variabel Utama

`RegionLootTable` adalah kelas statis tanpa status. Ia memilih isi dari aset `LootTable`. Status titik tetap dipegang `LootSpawnerSystem` dan `LootContainerInteractable`.

```csharp
public enum LootLocation { None, Ship, Volcano, Amazon, Snow, Logistic }

[Flags] public enum LootPointKind { House = 1, Chest = 2, SoldierBody = 4, Warehouse = 8, Any = 15 }

[Serializable]
public struct LootEntry           // Menggantikan kelas LootTable lama yang berisi satu item
{
    public ItemData item;
    public float weight;          // Bobot relatif, bukan persen
    public int minAmount, maxAmount;
    public LootPointKind kinds;   // Jenis titik yang boleh memuat entri ini
    public int minDay;            // Bobot dianggap 0 sebelum hari ini
    public bool isRare;           // Bobotnya naik mengikuti hari
}

[CreateAssetMenu(menuName = "Survival/Loot Table")]
public class LootTable : ScriptableObject
{
    public string tableId;
    public LootLocation location;   // None untuk Drop_* dan Debris_Default
    public LootEntry[] entries;
}
```

### Jenis titik

| Kind | Objek | Cara kerja | Isi |
|---|---|---|---|
| `House` | `LootSpawnerSystem` di rumah | Tersebar saat region pertama dimuat | 1 tumpukan, bisa kosong |
| `Chest` | `LootContainerInteractable` | Diisi saat dibuka | `loot.chestRollsMin` sampai `loot.chestRollsMax` tumpukan |
| `SoldierBody` | `LootContainerInteractable` dengan model mayat, `HoldSeconds` 1 | Diisi saat digeledah | `loot.bodyRollsMin` sampai `loot.bodyRollsMax` tumpukan |
| `Warehouse` | `LootSpawnerSystem` di gudang | Sama dengan `House`, dengan peluang kosong lebih kecil | 1 tumpukan per titik |

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.runSeed.lootPointEmptyChance` | 0,5 | Titik `House` kosong. Sudah ada di #89 |
| `GameBalanceConfig.loot.warehouseEmptyChance` | 0,25 | Titik `Warehouse` kosong |
| `GameBalanceConfig.loot.chestRollsMin` / `chestRollsMax` | 1 / 3 | Tumpukan per peti |
| `GameBalanceConfig.loot.bodyRollsMin` / `bodyRollsMax` | 1 / 2 | Tumpukan per mayat tentara |
| `GameBalanceConfig.loot.amountMulVolcano` | 1 | Pengali jumlah Region 1 |
| `GameBalanceConfig.loot.amountMulAmazon` | 1,1 | Pengali jumlah Region 2 |
| `GameBalanceConfig.loot.amountMulLogistic` | 1,25 | Pengali jumlah Region 4 |
| `GameBalanceConfig.loot.amountMulSnow` | 1,4 | Pengali jumlah Region 3, area terjauh |
| `GameBalanceConfig.loot.rareWeightPerDay` | 0,02 | Tambahan bobot item langka per hari |
| `GameBalanceConfig.loot.rareWeightMaxMul` | 2 | Batas pengali bobot item langka |
| `GameBalanceConfig.loot.amountBonusPer10Days` | 0,1 | Tambahan jumlah tiap 10 hari |

### Isi tabel lokasi (usulan)

Persen dihitung dari entri yang lolos saringan. Jadi total bobot tidak harus 100. H = `House`, C = `Chest`, B = `SoldierBody`, W = `Warehouse`. Tanda * berarti `isRare`.

| Tabel | Item, jumlah, bobot, titik |
|---|---|
| `Loot_Ship` | `Food_Can` 1, 30. `Ammo_9mm` 3-6, 25. `Drink_CleanWater` 1, 20. `Med_Bandage` 1, 12. `Med_Medkit` 1, 5*. `Med_Vaccine` 1, 4*. `Item_Battery` 1, 5. Semua `Any` |
| `Loot_Region1` | `Mat_Cloth` 1-3, 25, H. `Food_Can` 1-2, 20, HC. `Drink_CleanWater` 1, 20, HC. `Mat_Rope` 1-2, 15, HC. `Med_Bandage` 1, 10, HC. `Ammo_9mm` 3-6, 8, C. `Seed_Potato` 1-2, 6, H (usulan, kalau #48 disetujui). `Item_Battery` 1, 5, HC |
| `Loot_Region2` | `Food_Can` 1-2, 15, HC. `Drink_CleanWater` 1, 15, HC. `Mat_TreeSap` 1-2, 14, HC. `Mat_Mushroom` 1-2, 14, HC. `Med_Norit` 1, 12, HC. `Mat_Rope` 1-2, 10, H. `Ammo_9mm` 3-6, 10, C. `Med_Antibiotic` 1, 8, C*. `Item_Battery` 1, 5, HC |
| `Loot_Region3` | `Ammo_9mm` 6-12, 25, BW. `Mat_Scrap` 2-4, 15, W. `Ammo_556` 5-15, 15, BWC*, `minDay` 10. `Food_Can` 1-2, 12, CW. `Drink_CleanWater` 1, 10, CW. `Med_Medkit` 1, 8, BC*. `Gear_WarmClothes` 1, 6, CW. `Tool_Flashlight` 1, 4, W. `Med_Supplement` 1, 3, C*. `Item_Battery` 1, 5, CW |
| `Loot_Region4` | `Ammo_9mm` 3-8, 20, HW. `Mat_Scrap` 2-5, 20, HW. `Food_Can` 1-2, 18, HC. `Drink_CleanWater` 1, 15, HC. `Mat_Rope` 1-2, 10, H. `Med_Bandage` 1, 10, HC. `Med_Antibiotic` 1, 5, C*. `Item_Battery` 1, 5, HC |

Suku cadang helikopter tidak ada di tabel ini (#69). Bobot `Item_Battery` masih sementara, dan disetel saat tabel difinalkan (#98).

### Isi tabel lain

| Tabel | Isi | Pemakai |
|---|---|---|
| `Debris_Default` | Mengikuti `raft/DebrisRetrieval.md`. Usulan, kalau #106 disetujui | #106 |
| `supply_drop` | `Ammo_9mm` 3-6, 30. `Food_Can` 1-2, 30. `Drink_CleanWater` 1-2, 25. `Med_Bandage` 1, 15. Tanpa `Med_Vaccine`, sesuai GDD bagian 3 | #54 |
| `Loot_Lair_Cave` | Usulan: `Ammo_9mm` 6-12, 30. `Mat_Scrap` 3-5, 25. `Med_Medkit` 1, 20*. `Food_Can` 2-3, 25 | #112 |
| `Loot_Lair_MilitaryBunker` | Usulan: `Ammo_9mm` 8-15, 30. `Ammo_556` 10-20, 25*. `Med_Medkit` 1, 20*. `Mat_Scrap` 3-5, 25 | #112 |
| `Loot_Lair_VillageWarehouse` | Usulan: `Mat_Scrap` 4-6, 30. `Ammo_9mm` 6-10, 25. `Food_Can` 2-4, 25. `Med_Antibiotic` 1, 20* | #112 |
| `Loot_Lair_RuinedHouse` | Usulan: `Food_Can` 2-3, 30. `Drink_CleanWater` 2, 25. `Med_Bandage` 2, 25. `Ammo_9mm` 4-8, 20 | #112 |
| `Drop_Zombie_Standard` | `Ammo_9mm` 1-3, 70. `Mat_Cloth` 1, 30 | #95 |
| `Drop_Zombie_Tribal`, `Drop_Zombie_Moss` | `Mat_TreeSap` 1, 50. `Mat_Mushroom` 1, 50. Ini mengisi ID yang ditunda #95 | #95 |
| `Drop_Zombie_Military` | `Ammo_9mm` 3-6, 100 | #95 |
| `Drop_Zombie_Frozen` | `Mat_Cloth` 1-2, 100 | #95 |
| `Drop_Zombie_Village` | `Ammo_9mm` 1-3, 100 | #95 |

### Daftar senjata dan peluru

| ID | Jenis | Peluru | Sumber | Status |
|---|---|---|---|---|
| `Base_Pistol` | Pistol, damage 30 | `Ammo_9mm` | Item awal | GDD |
| `Base_Pisau` | Melee | Tidak ada | Item awal | GDD |
| `Tool_Axe`, `Tool_Pickaxe` | Melee dan alat | Tidak ada | Resep (#77) | Sudah ada |
| `Reward_LaserPistol`, `Reward_FireAxe` | Hadiah | Tanpa peluru | Lucky Machine (#72) | GDD |
| `Wpn_Machete` | Melee, damage 35 | Tidak ada | Pos penulis Diary, Region 2 | Usulan |
| `Wpn_SMG` | `Auto`, damage 18, magazine 30, jeda 0,09 detik | `Ammo_9mm` | Gudang balai desa, Region 4 | Usulan |
| `Wpn_Rifle` | `SemiAuto`, damage 45, magazine 20, jeda 0,3 detik, jarak 90 m, suara 45 m | `Ammo_556` | Gudang senjata, Pangkalan Militer, Region 3 | Usulan |

**Jenis peluru hanya dua:** `Ammo_9mm` dan `Ammo_556`. Senapan tetap harus mengarah ke kepala Zombie Militer, karena armor badan 0,35 (#59) berlaku juga untuknya.

### Isi tetap

Barang unik tidak diacak. Titik yang dibuat tangan memakai field baru `guaranteedItems` (`ItemData` dan jumlah), yang keluar sebelum isi acak. Dengan begitu, seed yang sama selalu memberi hadiah yang sama, dan penjelajahan jauh selalu berbuah.

| Titik | Isi tetap |
|---|---|
| `Chest_AuthorCamp`, Region 2 | `Wpn_Machete` |
| `Warehouse_VillageHall`, Region 4 | `Wpn_SMG`, 30 `Ammo_9mm` |
| `Chest_Armory`, Region 3 | `Wpn_Rifle`, 20 `Ammo_556`, `Bag_Military` (#118) |

## 2. Detail Logika & Fungsi

### Rumus peluang baru

Rumus lama memakai `dropChance` 0 sampai 100 tanpa aturan total, sehingga arti angkanya berubah saat isi daftar berubah. Rumus baru:

```
bobot_efektif(e) = 0                        kalau kind tidak cocok, atau hari < e.minDay
                 = e.weight × pengaliLangka kalau e.isRare
                 = e.weight                 selain itu
pengaliLangka    = min(rareWeightMaxMul, 1 + rareWeightPerDay × (hari - 1))
peluang(e)       = bobot_efektif(e) / jumlah semua bobot_efektif
```

Peluang titik kosong hanya ditentukan `lootPointEmptyChance` atau `warehouseEmptyChance`, dan tidak ikut dalam bobot. Di `LootSpawnerSystem`, baris `Random.value > 0.5f` dihapus.

**`public static bool TryRoll(LootTable table, LootPointKind kind, int day, ref TStream stream, out ItemData item, out int amount)`**, `TStream` adalah tipe aliran dari `RunSeedManager.Fork`:

1. Isi `weightBuffer` (array statis yang dipakai ulang, tanpa garbage) dengan `bobot_efektif` tiap entri.
2. `int i = stream.PickWeighted(weightBuffer)`. Kalau -1, kembalikan `false`, dan titik dibiarkan kosong (#89).
3. `amount = stream` acak dari `minAmount` sampai `maxAmount`, lalu kalikan `GetAmountMul(table.location, day)`, bulatkan ke bawah, minimal 1. Item yang tidak bisa bertumpuk selalu berjumlah 1.

**`public static float GetAmountMul(LootLocation loc, int day)`** = `amountMul<lokasi> × (1 + amountBonusPer10Days × floor(day / 10))`. Lokasi `Ship` dan `None` memakai pengali 1.

### Skala hari dan jarak

- **Jarak dari area awal** diwakili lokasi. Region 3 lebih jauh sehingga pengalinya paling besar. Tidak ada hitungan meter, karena titik sudah dibuat tangan per region.
- **Hari** mengubah bobot item langka dan jumlah. Saringan `minDay` menahan `Ammo_556` sebelum hari ke-10.
- Hari yang dipakai adalah `GameManager.currentDay` saat titik pertama kali diisi, lalu hasilnya disimpan.

### Pemakaian

**`LootSpawnerSystem.RollAndRecord()`** mengikuti #89. Tiap titik mendapat `pointKind` di Inspector. `spawnPoints` berubah menjadi `LootPointDef[] { Transform point; LootPointKind kind; ItemStack[] guaranteedItems; }`, dan `availableLoot` diganti `LootTable table`. Urutannya: isi tetap, lempar peluang kosong, lalu `TryRoll`. Titik dengan isi tetap tidak bisa kosong.

**`LootContainerInteractable.OnInteract`** mengikuti #17. Setelah `Fork(IslandLoot, saveId)`, keluarkan `guaranteedItems`, lalu lempar jumlah tumpukan sesuai kind, dan panggil `TryRoll` sebanyak itu. Hasil yang item-nya sama digabung. `dropPoints` dipakai bergiliran.

**`EnemyHitReaction.SpawnDrop`** memanggil `TryRoll(dropTable, Any, day, ref GetStream(Gameplay), ...)`. `DebrisRetrieval` memakai cara yang sama dengan kind `Any`.

### Pengisian ulang

Titik loot tidak terisi lagi sepanjang run, termasuk setelah hari ke-60 (usulan). Sumber yang berulang sudah ada dan tidak berhenti setelah hari ke-60: panen (#73), kargo mingguan (#71), dan kotak suplai (#54). Dengan begitu, area yang sudah dijarah tetap kosong, dan pemain terdorong ke region berikutnya.

### Armor pemain

Tidak ada armor pemain di versi awal (usulan). `Infection` tetap naik sesuai `AttackData.infection`, karena vaksin yang langka adalah tekanan utama GDD. Tidak ada slot baru di `EquipmentManager`, dan hadiah Region 3 berupa senapan, peluru, dan `Bag_Military`.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** tidak ada. **Event yang didengar:** tidak ada. Kelas ini dipanggil langsung oleh pemakainya.

| Sistem | Hubungan |
|---|---|
| `LootSpawnerSystem`, `LootContainerInteractable` (#17) | Memanggil `TryRoll` dan memegang status titik |
| `RunSeedManager` (#89) | `Fork(ShipLoot atau IslandLoot, kunci)` dan `PickWeighted` |
| `WorldStateSave` (#80), `RegionStateStore` (#88) | Menyimpan `LootPointState` dan `isOpened` |
| `EnemyHitReaction` (#95), `DebrisRetrieval` (#106), `ZombieLair` (#112) | Memakai aset `LootTable` |
| `WorldItem`, `ObjectPooler` (#66) | Wujud item di dunia |
| `GunController` (#58) | Aset `WeaponData` untuk `Wpn_SMG` dan `Wpn_Rifle` |
| `CarryCapacity` (#118), `FarmingSystem` (#48), `DiseaseManager` (#45), `BaseDefense` (#26), `StatusEffectManager` (#20) | Sumber `Bag_Military`, `Seed_Potato`, obat, `Mat_Scrap`, dan `Med_Supplement` |

**Batas pekerjaan:** kotak dari langit di #54, kargo pantai di #71, dan resep di #77.

## 4. Kasus Khusus

- **Pool:** item memakai `WorldItem` dari `ObjectPooler`, dan `OnSpawned` milik #66 mengatur ulang isinya. `TryRoll` tidak membuat objek.
- **Game dijeda:** tidak berpengaruh, karena pengisian terjadi saat region dimuat atau saat `E`, yang tidak jalan ketika dijeda.
- **Inventori penuh:** tidak menghalangi, karena isi keluar sebagai `WorldItem`.
- **Saat disimpan:** titik yang sudah diambil dicatat lewat `LootPointState.isTaken`, dan peti lewat `isOpened`. Isi tiap titik `LootSpawnerSystem` ikut tersimpan. Karena itu, load tidak pernah mengacak ulang.
- **Permadeath:** semua status hilang bersama save run (#82). Run baru mengacak lagi dari seed baru. Aset `LootTable` tidak berubah.
- **Hari pengisian berbeda:** seed yang sama bisa memberi isi peti berbeda kalau pemain membukanya pada hari lain. Ini wajar. Susunan titik dan isi tetap tidak berubah, jadi pengulangan bug tetap memungkinkan kalau hari juga dicatat.
- **Tabel kosong atau semua bobot 0:** `PickWeighted` mengembalikan -1, dan titik kosong. Validator Editor melaporkan tabel tanpa entri untuk kind yang memakainya.
- **Fase 1:** `Loot_Ship` tidak punya save (#82), dan hari selalu dianggap 1.
- **Barang wajib:** tidak ada barang wajib menang yang bergantung pada loot (#116).

## 5. Keputusan Desain

**Keputusan yang dibuat:**

- Bobot relatif menggantikan `dropChance`, karena `PickWeighted` di #89 sudah memakai bobot.
- `LootEntry` memakai nama baru, karena `LootTable` adalah nama resmi aset menurut `GLOSSARY.md`.

**Usulan yang menunggu developer (dicatat di #85):**

1. **Senjata baru:** tambah `Wpn_Machete`, `Wpn_SMG`, dan `Wpn_Rifle`, dengan `Ammo_556` sebagai peluru kedua. Rekomendasi: pakai. Pistol, Pisau, dan Kapak saja tidak memberi alasan menjelajah.
2. **Armor pemain:** rekomendasi tidak ada di versi awal, dan tidak mengubah infeksi.
3. **Isi ulang titik loot:** rekomendasi tidak terisi lagi sepanjang run. Run setelah hari ke-60 mengandalkan sumber berulang.
4. **Skala:** rekomendasi memakai pengali per lokasi dan hari seperti tabel angka di atas.
5. **Isi tetap:** rekomendasi tiga barang unik di titik tetap, bukan diacak.

## 6. Skenario Tes

1. Buat tabel dengan bobot 30 dan 10, panggil `TryRoll` 10.000 kali. **Hasil:** sekitar 75 dan 25 persen.
2. Tambah entri berbobot 40 ke tabel itu. **Hasil:** menjadi 37,5, 12,5, dan 50 persen, tanpa mengubah entri lain.
3. Panggil `TryRoll` dengan kind `SoldierBody` pada `Loot_Region1`. **Hasil:** kembali `false`, titik kosong, tanpa error.
4. Di Region 3, buat `currentDay` 5, lalu buka peti. Ulangi di hari 12. **Hasil:** `Ammo_556` hanya muncul di hari 12.
5. Bandingkan jumlah `Ammo_9mm` di Region 1 dan Region 3 pada 100 titik. **Hasil:** Region 3 lebih banyak sesuai pengali.
6. Buka `Chest_Armory`. **Hasil:** selalu keluar `Wpn_Rifle`, 20 `Ammo_556`, dan `Bag_Military`, ditambah isi acak.
7. Ambil 2 item, simpan, keluar, lalu lanjutkan. **Hasil:** item yang diambil tidak muncul lagi, dan sisanya sama.
8. Tinggalkan region, kembali 3 hari kemudian. **Hasil:** titik tetap kosong, peti tetap terbuka.
9. Mati, lalu mulai run baru. **Hasil:** semua titik terisi lagi dari seed baru.
10. Bunuh zombie Tribal dengan pistol sampai drop keluar. **Hasil:** isinya `Mat_TreeSap` atau `Mat_Mushroom`.
11. Jalankan 1.000 `TryRoll` sambil membuka Profiler. **Hasil:** tidak ada alokasi GC dari `TryRoll`.
