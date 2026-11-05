# Blueprint: ItemUseSystem.cs (ItemInstance & WorldItem)
**Lokasi:** `docs/blueprint/Core_Logic/ItemUseSystem.md`
**Deskripsi Utama:** Alur memakai, membuang, dan membusuknya item, data tiap item di slot (`ItemInstance`), dan item yang tergeletak di dunia (`WorldItem`).
**Issue:** #66
**Rujukan GDD:** Bagian 3 (lapar, haus, vaksin, air kotor berisiko, dan item inti). Pemulihan bertahap, pembusukan, dan aturan penghapusan item adalah usulan, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

### Tambahan pada `ItemData`

`ItemData` tetap sumber data per jenis item. Nilainya bersifat data, sehingga item baru tidak butuh kode baru.

| Field | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `maxStack` | `int` | 1 | Batas tumpukan per slot. Item dengan durabilitas atau magazine selalu 1 |
| `hungerDelta` | `float` | 0 | Tambahan lapar |
| `thirstDelta` | `float` | 0 | Tambahan haus |
| `infectionDelta` | `float` | 0 | Perubahan infeksi. Negatif mengobati (#64) |
| `healAmount` | `float` | 0 | Sudah ada. Tambahan HP |
| `useSeconds` | `float` | 0 | Waktu memakai. Bisa dibatalkan |
| `restoreSeconds` | `float` | 0 | Lama pemulihan bertahap. 0 berarti langsung |
| `isConsumable` | `bool` | `false` | Punya aksi Pakai |
| `isDroppable` | `bool` | `true` | Bisa dibuang |
| `isCoreItem` | `bool` | `false` | Item inti. Tidak bisa dibuang, dan selalu kembali ke slot |
| `maxDurability` | `float` | -1 | -1 berarti tidak memakai durabilitas |
| `spoilHours` | `float` | 0 | Jam game sampai busuk. 0 berarti tidak busuk |
| `poisonChance` | `float` | 0 | Peluang keracunan saat segar. Dipakai #45 |
| `worldMesh`, `worldMaterial` | Aset | `null` | Tampilan `WorldItem` |
| `curesDiseases` | `DiseaseCure` | `None` | Penyakit yang disembuhkan (#45) |
| `fuelSeconds` | `float` | 0 | Bahan bakar api unggun. `Mat_Wood` 60, `Mat_Charcoal` 120 (#44) |
| `appliesEffectId` | `string` | Kosong | Efek yang dipasang saat dipakai (#20) |
| `isRepairable` | `bool` | `false` | Bisa diperbaiki (#113) |
| `repairCost` | `RecipeIngredient[]` | Kosong | Biaya perbaikan penuh (#113) |
| `repairStation` | `CraftingStationType` | `WoodBench` | Stasiun perbaikan (#113) |

`Base_Kail`, `Base_Pistol`, dan `Base_Pisau` memakai `isCoreItem = true` dan `isDroppable = false`, sesuai `GLOSSARY.md`. `ItemType.KeyItem` juga selalu `isDroppable = false` (#116).

### `ItemInstance`

```csharp
[Serializable]
public class ItemInstance
{
    public int instanceId;          // Unik dalam satu run, dari InventoryManager.nextInstanceId
    public string itemId;
    public float durability;        // -1 kalau tidak dipakai
    public int magazineAmmo;        // -1 kalau bukan senjata api (#58)
    public float createdGameHour;   // Jam game total saat dibuat. -1 kalau tidak busuk
    public int upgradeMask;         // Dicadangkan untuk #113
    public int repairCount;         // Jumlah perbaikan (#113)
    public string luckyModifierId;  // Modifier Lucky Machine. Kosong kalau tidak ada (#113, #72)
    public string skinId;           // Tampilan senjata dari Lucky Machine. Kosong kalau tidak ada (#72)
}

[Serializable]
public class InventorySlot
{
    public ItemInstance instance;   // null berarti slot kosong
    public int amount;
}
```

`InventorySlot` tidak lagi menyimpan `ItemData` langsung. `ItemDatabase.Get(itemId)` mengambil datanya, dengan pencarian `Dictionary` yang dibangun sekali.

**Fungsi tambahan pada `InventoryManager`:**

| Fungsi | Kegunaan |
|---|---|
| `int AddItem(ItemInstance source, int amount)` | Memanggil `SlotArrayOps.Add`, lalu mengembalikan sisa yang tidak muat. `AddItem(ItemData, int)` tetap ada dan memakai fungsi ini. Aturan tumpukan hanya ada di `SlotArrayOps`, dipakai bersama `StorageChest` (#46) |
| `void RemoveFromSlot(int slotIndex, int amount)` | Mengurangi dari slot tertentu |
| `void RemoveInstance(int instanceId)` | Menghapus item unik |
| `int SplitStack(int slotIndex, int amount)` | Memisahkan tumpukan ke slot kosong. Mengembalikan indeks baru, atau -1 |
| `void MoveSlot(int from, int to)` | Menukar isi dua slot |
| `ItemInstance FindInstance(int instanceId)` | Mencari item unik |
| `int GetCount(string itemId)` | Menghitung total lewat `SlotArrayOps.CountItem` (#46) |
| `bool CanFitAfterRemoval(RecipeIngredient[] removed, ItemData result, int amount)` | Simulasi ruang tanpa mengubah slot (#77) |

`AddItem` lama yang mengembalikan `bool` diganti, supaya tumpukan yang hanya muat sebagian tidak hilang.

**Kontrak save `InventoryManager` (#80):** `ISaveable`, dengan `SaveId = player_inventory` dan `LoadOrder = 50`. Yang disimpan: slot berisi, `ItemInstance`, dan `nextInstanceId`.

### Status pakai

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `IsUsing` | `bool` | `false` | Sedang memakai item |
| `useSlotIndex` | `int` | -1 | Slot yang dipakai |
| `useTimer` | `float` | 0 | Sisa waktu pakai |

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.items.useMoveSpeedMultiplier` | 0,5 | Kecepatan saat memakai item |
| `GameBalanceConfig.items.defaultRestoreSeconds` | 20 | Pemulihan bertahap untuk makanan dan minuman |
| `GameBalanceConfig.items.dropForwardDistance` | 1 | Jarak jatuhnya item dari kamera |
| `GameBalanceConfig.items.worldItemDespawnGameHours` | 48 | Item yang dibuang hilang setelah 2 hari game |
| `GameBalanceConfig.items.maxLooseWorldItems` | 64 | Batas `WorldItem` lepas sekaligus |
| `GameBalanceConfig.items.staleRatio` | 0,5 | Bagian umur saat makanan mulai basi |
| `GameBalanceConfig.items.staleHungerMultiplier` | 0,75 | Hasil makanan basi |
| `GameBalanceConfig.items.rottenHungerMultiplier` | 0,25 | Hasil makanan busuk |
| `GameBalanceConfig.items.rottenPoisonChance` | 0,8 | Peluang keracunan makanan busuk |

### Contoh isi `ItemData` konsumsi

Nilai mengikuti target makan dan minum di `GameBalanceConfig.md`.

| Item | ID | Lapar | Haus | Pakai | Busuk | `poisonChance` |
|---|---|---|---|---|---|---|
| Daging mentah | `Food_RawMeat` | 15 | 0 | 2 detik | 12 jam game | 0,5 (#45) |
| Ikan mentah | `Food_RawFish` | 12 | 0 | 2 detik | 12 jam game | 0,5 (#45, #55) |
| Daging matang | `Food_CookedMeat` | 33 | 0 | 2 detik | 48 jam game | 0 |
| Ikan matang | `Food_CookedFish` | 33 | 5 | 2 detik | 36 jam game | 0 |
| Makanan gosong | `Food_Burnt` | 5 | 0 | 2 detik | Tidak busuk | 0 (#44) |
| Makanan kaleng | `Food_Can` | 33 | 5 | 3 detik | Tidak busuk | 0 |
| Air Minum Bersih | `Drink_CleanWater` | 0 | 25 | 2 detik | Tidak busuk | 0 |
| Kelapa | `Drink_Coconut` | 5 | 25 | 3 detik | 96 jam game | 0 |
| Air kotor | `Drink_DirtyWater` | 0 | 15 | 2 detik | Tidak busuk | 0,35 (#45) |
| Vaksin | `Med_Vaccine` | 0 | 0 | 4 detik (`GameBalanceConfig.infection.vaccineInjectSeconds`) | Tidak busuk | 0 |

**Isi tambahan obat dan lainnya:**

| Item | ID | Isi | Pakai |
|---|---|---|---|
| Vaksin | `Med_Vaccine` | `restoreSeconds = 0`, `infectionDelta = -50` (`GameBalanceConfig.infection.vaccineReduction`) | 4 detik |
| Norit | `Med_Norit` | Menyembuhkan lewat `curesDiseases` (#45) | 2 detik |
| Perban | `Med_Bandage` | HP +5 (#45) | 3 detik |
| Medkit | `Med_Medkit` | HP +40 (#45) | 5 detik |
| Antibiotik | `Med_Antibiotic` | Menyembuhkan lewat `curesDiseases` (#45) | 2 detik |
| Baterai | `Item_Battery` | `isConsumable`, `maxStack` 5 (#98) | Aksi khusus |
| Kit api unggun | `Kit_Campfire` | Aksi khusus (#44) | Tanpa waktu pakai |

## 2. Detail Logika & Fungsi

### Aksi item di UI

`InventoryPanel : UIPanel` memakai panel `Inventory` (#83). Klik kanan pada slot membuka menu kecil. Isi menu mengikuti data item.

| Aksi | Syarat tampil | Fungsi | Hasil |
|---|---|---|---|
| **Pakai** | `isConsumable` | `ItemUseSystem.TryUse(slot)` | Mulai memakai, lalu panel tetap terbuka |
| **Pakai** | `isWarmClothing` | `WarmthSystem.WearClothing(instanceId)` (#65), bukan `TryUse` | Pakaian terpasang |
| **Pakai** | `Kit_Campfire` | `BuildManager.BeginPlacementFromItem(instanceId, Build_Campfire)` (#44) | Mulai penempatan |
| **Pakai** | `Item_Battery` | Cari `Tool_Flashlight` yang dipegang, lalu yang isinya terendah di inventori. Panggil `FlashlightController.Recharge(senter, flashlight.batteryRecharge)` dengan `ItemInstance` senter yang dipilih, lalu kurangi 1 item hanya kalau hasilnya `true` (#98, `FlashlightController.md`) | Ditolak kalau senter tidak ada atau sudah penuh |
| **Buang** | `isDroppable` | `ItemUseSystem.DropFromSlot(slot, amount)` | Item jatuh sebagai `WorldItem` |
| **Pisahkan** | `amount > 1` dan ada slot kosong | `InventoryManager.SplitStack(slot, amount / 2)` | Tumpukan terbagi dua |
| **Ke Hotbar** | Punya `equipPrefab` atau `isConsumable` | `EquipmentManager.AssignToHotbar(index, instanceId)` | Rujukan masuk ke hotbar (#60) |
| **Pegang di Tangan Kiri** | `handUse == OffHand` | `EquipmentManager.EquipOffHand(instanceId)` | Senter atau Obor di tangan kiri (#60) |

- Seret dan lepas antar-slot memanggil `MoveSlot`.
- Menyeret keluar panel sama dengan Buang.
- Saat `StoragePanel` terbuka, menu kecil `InventoryPanel` tidak muncul pada klik kanan, karena klik kanan menjadi transfer cepat (#46).
- `OnClosed` mengembalikan barang yang masih diseret ke slot asalnya, termasuk saat pemain mati (#83).
- Panel tidak mengubah status langsung, sesuai aturan UI di `GameEventBus.md`.

### Memakai item

**`public bool TryUse(int slotIndex)`**:

1. Tolak kalau `IsUsing`, slot kosong, atau item bukan `isConsumable`.
2. Tolak kalau semua status yang dipulihkan sudah penuh, dengan notifikasi. Kalau `item.infectionDelta < 0`, panggil `InfectionSystem.CanUseMedicine(item)` (#64). Kalau `item.curesDiseases != None`, atau item obat dengan `healAmount > 0`, panggil `DiseaseManager.CanUseMedicine(item)` (#45).
3. Catat `useSlotIndex`, atur `useTimer = useSeconds`, dan `IsUsing = true`.
4. Pasang `IslandPlayerController.SetSpeedModifier(ItemUse, useMoveSpeedMultiplier)`.
5. Kalau `useSeconds == 0`, langsung panggil `CompleteUse()`.

**`Update()`**, saat `IsUsing`: kurangi `useTimer` dengan `Time.deltaTime`. Kalau `useTimer <= 0`, panggil `CompleteUse()`.

**`private void CompleteUse()`**:

1. Ambil `ItemData` dan `ItemInstance` dari slot. Kalau slot sudah berubah, batalkan.
2. Hitung `freshness = GetFreshness(instance)`.
3. Hitung lapar, haus, dan HP setelah pengali kesegaran.
4. Kalau `restoreSeconds > 0`, panggil `SurvivalStats.QueueRestore(hunger, thirst, restoreSeconds)`. Kalau 0, panggil `RestoreHunger` dan `RestoreThirst`.
5. Kalau `healAmount > 0`, panggil `SurvivalStats.RestoreHP(healAmount)` langsung.
6. Kalau `infectionDelta != 0`, panggil `InfectionSystem.ApplyMedicine(item)`, atau `SurvivalStats.ReduceInfection` selama #64 belum tersedia.
7. Panggil `DiseaseManager.OnConsumed(item, freshness)` (#45) untuk peluang keracunan.
8. `InventoryManager.RemoveFromSlot(slot, 1)`, yang menerbitkan `InventoryChangedEvent`.
9. Kalau `appliesEffectId` terisi, panggil `StatusEffectManager.ApplyById(appliesEffectId, itemId)` (#20).
10. Terbitkan `ItemConsumedEvent { itemId, freshness }`.
11. Hapus pengali kecepatan, dan `IsUsing = false`.

**`public void CancelUse(string reason)`**: `IsUsing = false`, hapus pengali kecepatan, dan item tidak berkurang.

**Pemicu batal:**

| Pemicu | Sumber |
|---|---|
| Terkena serangan | `PlayerDamagedEvent` |
| Berganti item | `EquippedItemChangedEvent`, kalau item yang dipakai adalah item di tangan |
| Mulai lari | `InputManager.SprintHeld` |
| Heavy Item, diterkam, memanjat, atau mati | `HeavyItemChangedEvent`, `PlayerPinnedEvent`, #101, dan `PlayerDiedEvent` |

### Pemulihan bertahap

`CATATAN_MEKANIK.md` bagian 2 mencatat makanan di Raft menaikkan nilai target, lalu nilai yang tampil mengejarnya selama sekitar 20 detik.

**Yang diterapkan:** makanan dan minuman memakai `restoreSeconds = defaultRestoreSeconds`. `SurvivalStats.QueueRestore` menyalurkan sisa pemulihan per detik. Pemulihan tidak batal saat pemain diserang, karena makanan sudah dimakan. Kurva mentah dan matang dari Raft tidak ditiru, karena perbedaan itu sudah ada di `hungerDelta`.

**Yang tetap langsung:** HP dari obat, dan infeksi dari vaksin.

### Pembusukan

Pembusukan memakai jam game, sehingga ikut berhenti saat game dijeda dan ikut cepat saat tidur.

```csharp
public enum Freshness { Fresh, Stale, Rotten }

Freshness GetFreshness(ItemInstance inst, ItemData data)
{
    if (data.spoilHours <= 0f) return Freshness.Fresh;
    float age = DayNightDirector.Instance.TotalGameHours - inst.createdGameHour;
    if (age >= data.spoilHours) return Freshness.Rotten;
    return age >= data.spoilHours * balance.staleRatio ? Freshness.Stale : Freshness.Fresh;
}
```

- **Tanpa pemeriksaan per frame:** `InventoryManager` mendengar `HourChangedEvent`, lalu memeriksa slot berisi `spoilHours > 0`. Jarak tiap jam game sekitar 37,5 detik.
- **Tampilan:** ikon diberi warna `Segar`, `Basi`, atau `Busuk`. Teks diperbarui hanya kalau kesegaran berubah.
- **Menggabung tumpukan:** kedua tumpukan digabung, dan `createdGameHour` memakai rata-rata berbobot jumlahnya.
- **Peti (#46):** isi peti memakai fungsi yang sama, tanpa perlambatan.
- **`WorldItem`:** umur tetap berjalan, karena `ItemInstance` ikut bersamanya.

### Akibat makanan busuk

| Kesegaran | Lapar dan haus | Peluang keracunan |
|---|---|---|
| `Fresh` | 100 persen | `poisonChance` item |
| `Stale` | x `staleHungerMultiplier` | `poisonChance` item |
| `Rotten` | x `rottenHungerMultiplier` | `rottenPoisonChance`, memakai yang lebih besar dari `poisonChance` |

Isi keracunan, efek, dan obatnya milik #45. Blueprint ini mengirim `OnConsumed(item, freshness)`. Peluang dihitung di #45 dengan aliran `Gameplay` (#89). Kalau #45 belum selesai, item busuk tetap memberi hasil rendah tanpa keracunan.

### `WorldItem`

`WorldItem : MonoBehaviour, IInteractable, IPoolable` adalah item di tanah. Berada di layer `WorldItem`, memakai satu pool `WorldItem_Generic`, dan mesh diganti dari `ItemData.worldMesh`.

| Variabel | Tipe | Fungsi |
|---|---|---|
| `Instance` | `ItemInstance` | Data item |
| `Amount` | `int` | Jumlah |
| `despawnAtGameHour` | `float` | Waktu hilang. -1 berarti tidak hilang |
| `sourceSpawner`, `sourcePointIndex` | `LootSpawnerSystem`, `int` | Sumber loot (#89). Kosong kalau item dijatuhkan |
| `homeRegion` | `RegionType` | Region tempat item berada |

**`WorldItemManager`** berada di `IslandPhase`, dan menjadi "pemilik `WorldItem`" yang disebut #88.

| Fungsi | Kegunaan |
|---|---|
| `WorldItem Spawn(ItemInstance inst, int amount, Vector3 pos, bool despawns)` | Mengambil dari pool, memasang mesh, lalu mendaftarkan |
| `void Despawn(WorldItem item)` | Melepas daftar dan mengembalikan ke pool |
| `void CaptureRegion(RegionType region)` | Dipanggil saat region `Unloading`. Menulis data ke `RegionStateStore`, lalu mengembalikan objeknya ke pool |
| `void RestoreRegion(RegionType region)` | Dipanggil saat region `Loaded`. Memunculkan kembali item |
| `void SweepBelow(Bounds zone, float waterY, TideSafePoint[] safe)` | Dipanggil `TideManager` (#91). Item biasa di bawah `waterY` kembali ke pool. Item penting pindah ke `TideSafePoint` terdekat, disertai notifikasi |

`WorldItemManager` mendengar `RegionLoadStateChangedEvent`. Pada `Unloading`, ia mencatat `itemId`, jumlah, dan posisi ke `RegionStateStore`, lalu mengembalikan objek ke pool. Pada `Loaded`, ia memunculkannya kembali (#88).

**Membuang item, `public void DropFromSlot(int slotIndex, int amount)`**:

1. Tolak kalau `!isDroppable`, dengan notifikasi.
2. Hitung posisi dari kamera sejauh `dropForwardDistance`. Kalau `PhysicsQuery.Raycast` ke `PhysicsMasks.InteractBlock` mengenai dinding, pakai titik itu.
3. Panggil `WorldItemManager.Spawn(salinan, amount, pos, despawns: true)`, lalu `RemoveFromSlot`.
4. Beri dorongan kecil ke depan lewat `Rigidbody`.

**Mengambil item, `public bool Interact(in InteractContext ctx)`**, lewat `E` (#17):

1. `int sisa = ctx.Inventory.AddItem(Instance, Amount)`.
2. Kalau `sisa == Amount`, inventori penuh. **Item tetap di tanah,** dan tampil notifikasi `Inventori penuh` (#22). Kembalikan `false`.
3. Kalau `sisa > 0`, `Amount = sisa`, panggil `sourceSpawner?.UpdateRemaining(sourcePointIndex, sisa)` (`LootSpawnerSystem.md`), dan item tetap di tanah dengan notifikasi `Sebagian terambil`. Tanpa panggilan ini, load memulihkan jumlah awal dan barang berlipat.
4. Kalau `sisa == 0`, panggil `sourceSpawner?.MarkTaken(sourcePointIndex)`, lalu `Despawn`.

**Aturan penghapusan:**

| Kategori | Hilang otomatis |
|---|---|
| Dibuang pemain atau jatuh dari musuh | Ya, setelah `worldItemDespawnGameHours`, diperiksa saat `HourChangedEvent` |
| Loot dari `LootSpawnerSystem` | Tidak. Statusnya dicatat spawner |
| Barang sisa kedatangan di pantai (#74) | Tidak |
| Item dengan `isDroppable = false` | Tidak pernah menjadi `WorldItem`, kecuali oleh #116 |
| Melebihi `maxLooseWorldItems` | `WorldItem` lepas tertua kembali ke pool, dengan pengecualian barang di atas |

**Air dan jatuh keluar peta:** item yang masuk trigger `Water` kehilangan kecepatan dan tenggelam, lalu dihapus setelah 1 jam game. Item yang berada di bawah `RegionRecord.bounds` dikembalikan ke posisi jatuhnya.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `ItemConsumedEvent`, dan `InventoryChangedEvent` lewat `InventoryManager`.

**Event yang didengar:** `HourChangedEvent`, `PlayerDamagedEvent`, `EquippedItemChangedEvent`, `HeavyItemChangedEvent`, `PlayerPinnedEvent`, `PlayerDiedEvent`, `RegionLoadStateChangedEvent`.

| Sistem | Hubungan |
|---|---|
| `SurvivalStats` (#111) | Menerima `RestoreHunger`, `RestoreThirst`, `RestoreHP`, dan `QueueRestore` |
| `InfectionSystem` (#64) | Mengatur efek vaksin dan durasinya |
| `DiseaseManager` (#45) | Mengatur keracunan lewat `OnConsumed` |
| `HydrationSystem` (#43) | Mengisi data air, termasuk air kotor |
| `CampfireManager` (#44) | Menghasilkan item matang dengan `createdGameHour` baru |
| `StorageChest` (#46) | Memakai `InventorySlot` dan `ItemInstance` |
| `EquipmentManager` (#60) | Memakai `instanceId` |
| `GunController` (#58) | Menulis `magazineAmmo` |
| `DurabilitySystem`, `ToolRepairSystem` (#113) | Menulis `durability` dan `upgradeMask` |
| `IslandInteractable` (#17) | Menyediakan `PlayerInteractor` dan `InteractContext` |
| `NotificationManager` (#22) | Menampilkan notifikasi |
| `RegionStreaming` (#88) | Memanggil simpan dan pulih per region |
| `LootSpawnerSystem`, #89 | Memakai `WorldItem` dan `MarkTaken` |
| `CarryCapacity` (#118) | Mengatur jumlah slot dan konversi dari kapal |

**Batas pekerjaan:** memasak di #44, keracunan di #45, peti di #46, dan interaksi objek dunia di #17.

## 4. Kasus Khusus

- **Objek pool:** `OnSpawned` memasang mesh dan collider sesuai data. `OnReturned` mengosongkan `Instance`, `Amount`, `sourceSpawner`, dan kecepatan `Rigidbody`, supaya `WorldItem` berikutnya tidak membawa data lama.
- **Target hover kembali ke pool:** `PlayerInteractor` melepas target yang tidak aktif (#17).
- **Game dijeda:** `Time.deltaTime` bernilai 0, jadi waktu pakai berhenti. Jam game juga berhenti, jadi pembusukan berhenti. Panel `Inventory` tidak menjeda, sesuai #83, jadi memakai item di menu tetap memakan waktu.
- **Permadeath:** isi inventori, `WorldItem`, dan `nextInstanceId` terhapus bersama save run (#82).
- **Save dan load:** setiap `InventorySlot` disimpan dengan `ItemInstance` lengkap: `instanceId`, `itemId`, `durability`, `magazineAmmo`, `createdGameHour`, `upgradeMask`, `repairCount`, `luckyModifierId`, dan `skinId`, lalu `amount` dan `nextInstanceId`. `WorldItem` disimpan per region di `RegionStateStore`. `IsUsing` tidak disimpan, jadi pemakaian yang sedang berjalan batal.
- **Inventori penuh saat mengambil:** item tetap di tanah, tanpa pengurangan apa pun.
- **Slot berubah saat dipakai:** `CompleteUse` membandingkan `instanceId` dengan yang tercatat. Kalau berbeda, pemakaian batal.
- **Tumpukan makanan dengan umur berbeda:** digabung lewat rata-rata berbobot. Ini sedikit menguntungkan pemain, tetapi jauh lebih sederhana daripada satu slot per umur.
- **Item inti di `DropFromSlot`:** selalu ditolak, juga lewat seret dan `DevConsole`.
- **Item jatuh di region lain:** `homeRegion` dihitung dari `RegionStreaming.GetRegionAt`.
- **`ItemData` diganti di versi baru:** `itemId` tidak ditemukan saat load. Slot dikosongkan, dan peringatan dicatat (#80).

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** item inti tidak bisa dibuang (`GLOSSARY.md`). Isi `ItemInstance` (`GLOSSARY.md`). Target makan dan minum (`GameBalanceConfig.md`). Peluang 50 persen daging mentah (#45).

**Keputusan teknis di blueprint ini:**

- Slot menyimpan `ItemInstance`, bukan `ItemData`.
- `WorldItem` memakai satu pool generik dan mesh dari data.
- Pembusukan dihitung dari jam game total saat dibutuhkan, tanpa penghitung per item.

**Usulan yang disetujui developer sebagai nilai awal (dicatat di #85):**

1. **Pemulihan bertahap:** makanan dan minuman naik selama 20 detik, mengikuti Raft. Tanpa kurva mentah dan matang.
2. **Pemakaian bisa batal:** terkena serangan, berganti item, atau mulai lari membatalkan pemakaian, dan item tidak hilang.
3. **Umur makanan:** daging mentah 12 jam game, ikan matang 36, daging matang 48, kelapa 96, dan makanan kaleng serta air tidak busuk.
4. **Tiga tingkat kesegaran:** Segar, Basi (75 persen hasil), dan Busuk (25 persen hasil, dengan peluang keracunan 80 persen).
5. **Penghapusan item:** item yang dibuang hilang setelah 48 jam game, dengan batas 64 item lepas. Loot dan barang dari kapal tidak hilang.
6. **Isi item konsumsi:** mengikuti tabel contoh.

## 6. Skenario Tes

1. Makan `Food_Can` saat lapar 50. **Hasil:** lapar naik 33 selama 20 detik, dan tumpukan berkurang 1.
2. Makan saat lapar 100. **Hasil:** ditolak dengan notifikasi, dan item tetap.
3. Mulai makan, lalu biarkan zombie memukul. **Hasil:** pemakaian batal, dan item tidak berkurang.
4. Mulai makan, lalu tekan lari. **Hasil:** pemakaian batal. Selama memakai, kecepatan setengah.
5. Buang 5 kayu dari tumpukan 20. **Hasil:** `WorldItem` berisi 5 muncul di depan pemain, dan slot tersisa 15.
6. Coba buang `Base_Pistol` lewat menu dan seret. **Hasil:** ditolak dua-duanya.
7. Isi semua slot, lalu tekan `E` pada `WorldItem`. **Hasil:** item tetap di tanah, dan notifikasi `Inventori penuh` tampil.
8. Sisakan ruang 3 untuk tumpukan 10, lalu ambil. **Hasil:** 3 masuk, dan 7 tetap di tanah.
9. Pisahkan tumpukan 20 di slot dengan ruang kosong. **Hasil:** dua tumpukan 10. Tanpa ruang, aksi tidak tampil.
10. Tembak beberapa kali, simpan, keluar, lalu lanjutkan. **Hasil:** isi magazine dan durabilitas sama per item.
11. Biarkan `Food_RawMeat` selama 6 jam game, lalu lewat 12 jam. **Hasil:** ikon berubah ke Basi, lalu Busuk.
12. Makan daging busuk. **Hasil:** lapar naik 25 persen, dan `DiseaseManager.OnConsumed` menerima `Rotten`.
13. Biarkan `Food_Can` selama 10 hari lewat `setday`. **Hasil:** tetap Segar.
14. Buka Pause selama 2 menit. **Hasil:** umur makanan dan waktu pakai tidak bertambah.
15. Jatuhkan item, pindah sampai region dilepas, lalu kembali. **Hasil:** item ada di posisi yang sama.
16. Jatuhkan 70 item. **Hasil:** tertua hilang, jumlah tetap 64, dan loot tidak ikut hilang.
17. Pakai item dari pool berkali-kali lalu ambil. **Hasil:** tidak ada mesh atau data dari item sebelumnya.
18. Mati saat inventori terbuka sambil menyeret. **Hasil:** barang kembali ke slot, dan run baru mulai dengan isi awal.
