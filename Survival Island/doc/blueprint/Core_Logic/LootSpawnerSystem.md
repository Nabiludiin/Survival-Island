# Blueprint: LootSpawnerSystem.cs
**Lokasi:** `Survival Island/doc/blueprint/Core_Logic/LootSpawnerSystem.md`
**Deskripsi Utama:** Menyebar loot di titik tertentu, di kapal maupun di pulau, dari seed run, lalu mengingat hasilnya supaya tidak berlipat setelah load.
**Issue:** #89, #74, #103
**Rujukan GDD:** Bagian 1 (*scavenging*) dan bagian 3 (looting di kapal pada Fase 1, dan looting di pulau sebagai sumber makanan). Peluang dan aturan titik adalah usulan.

## 1. Arsitektur & Variabel Utama

### Pola

`LootSpawnerSystem : MonoBehaviour, ISaveable`, dengan `SaveId = spawnerId`. Satu komponen mewakili satu kelompok titik, misalnya satu rumah, satu gudang, atau satu dek kapal.

| Lokasi | Scene | Masuk save lewat |
|---|---|---|
| Kapal | `ShipPhase` | Tidak disimpan (#82) |
| Pulau | Region scene (#88), di bawah `RegionSceneRoot` | `RegionStreaming.CaptureRegion` dan `RestoreRegion`, lalu `RegionStateStore` (`WorldStateSave.md`). Tidak memanggil `WorldStateSave.Register` |

Spawner tidak pernah berada di scene dasar `IslandPhase`, karena titik loot adalah objek dunia milik region.

```csharp
[Serializable]
public class LootPointDef
{
    public Transform point;
    public LootPointKind kind;
    public ItemStack[] guaranteedItems;   // Hanya elemen pertama yang dipakai
}

[Serializable]
public struct LootPointState { public int pointIndex; public string itemId; public int amount; public bool isTaken; }

[Serializable]
class LootSpawnerSaveData
{
    public int version = 1;
    public List<LootPointState> points;
}
```

### Variabel

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `spawnerId` | `string` | Dibuat di Editor | ID stabil, juga bagian kunci acak. Format `<region>_loot_<8 karakter>`, contoh `r4_loot_9c01d2ef`. Di kapal `ship_loot_<8 karakter>` |
| `table` | `LootTable` | Inspector | Menggantikan `availableLoot` (#103). Kapal memakai `Loot_Ship`. Spawner di sarang memakai `data.lootTable` (#112) |
| `spawnPoints` | `LootPointDef[]` | Inspector | Menggantikan `List<Transform>`. Indeks array adalah `pointIndex` |
| `states` | `List<LootPointState>` | Kosong | Hasil yang sudah dicatat. Satu baris per titik, termasuk titik kosong |
| `liveItems` | `WorldItem[]` | Panjang `spawnPoints` | Objek pulau yang sedang ada per titik. `null` kalau kosong atau sudah diambil |
| `liveShipItems` | `ShipInteractable[]` | Panjang `spawnPoints` | Objek kapal per titik |
| `homeRegion` | `RegionType` | Dari `RegionSceneRoot` induk | Region pemilik spawner |
| `hasSpawned` | `bool` | `false` | Penyebaran atau pemulihan sudah berjalan |
| `restoredJson` | `string` | `null` | Data dari `RestoreState`, menunggu dipakai |

**Dihapus:** kelas `LootTable` lama, `dropChance`, `availableLoot`, `SpawnLootAcrossMap`, dan baris `Random.value > 0.5f`.

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.runSeed.lootPointEmptyChance` | 0,5 | Peluang titik biasa kosong (#89) |
| `GameBalanceConfig.loot.warehouseEmptyChance` | 0,25 | Peluang titik `Warehouse` kosong (#103) |

Jumlah tumpukan dan skala hari diatur `RegionLootTable` (#103), bukan di sini.

### Aturan titik di Editor

**`OnValidate`** mencatat peringatan untuk:

- `spawnerId` kosong atau bentrok. ID kosong diisi otomatis dari `GUID.Generate()`, sama seperti `SaveIdTag` (`WorldStateSave.md`).
- `point` kosong.
- `guaranteedItems` lebih dari satu elemen. Titik hanya memegang satu tumpukan. Isi lebih banyak memakai `LootContainerInteractable` (#17).
- `table` kosong pada titik tanpa `guaranteedItems`.

**Urutan titik tidak boleh diubah** setelah build rilis. Titik baru ditambahkan di akhir array, karena `pointIndex` menjadi bagian kunci acak dan save.

**Tinggi titik:** titik pulau tidak boleh berada di bawah batas air pasang (#91). `SaveIdAuditWindow` (#80) ikut memeriksa `spawnerId`.

## 2. Detail Logika & Fungsi

### Kapan menyebar

| Lokasi | Pemicu |
|---|---|
| `ShipPhase` | `Start()` memanggil `SpawnOrRestore()`. Kapal tidak punya save, jadi selalu lewat `RollAndRecord()` |
| Pulau | `RegionLoadStateChangedEvent { region == homeRegion, state == Loaded }` memanggil `SpawnOrRestore()` |
| Scene uji tanpa `RegionStreaming` | `Start()` memanggil `SpawnOrRestore()` |

**Alasan pemicu pulau memakai event `Loaded`:** `RegionStreaming.LoadRegion` memanggil `RestoreRegion` di langkah 3, lalu menerbitkan `Loaded` di langkah 6 (`RegionStreaming.md`). Dengan begitu, data save sudah masuk lewat `RestoreState` sebelum spawner memutuskan untuk mengacak atau memulihkan. Kalau memakai `Start()`, urutannya tidak pasti, dan loot bisa berlipat.

### `public void RestoreState(string json)`

Hanya menyimpan `restoredJson = json`. Tidak memunculkan objek dan tidak mengirim event, sesuai kontrak `ISaveable`.

### `private void SpawnOrRestore()`

1. Kalau `hasSpawned`, selesai. Lalu `hasSpawned = true`.
2. Kalau `restoredJson` ada, jalankan `RestoreFromData()`.
3. Kalau tidak ada, jalankan `RollAndRecord()`.
4. Untuk tiap baris `states` yang `!isTaken` dan punya `itemId`, panggil `SpawnPoint(state)`.

### `private void RestoreFromData()`

1. Baca `LootSpawnerSaveData`. Kalau rusak, catat peringatan dan jalankan `RollAndRecord()`.
2. Salin baris dengan `pointIndex` yang masih ada di `spawnPoints`. Baris di luar jangkauan dibuang dengan peringatan.
3. Titik yang tidak punya baris adalah titik baru dari versi game yang lebih baru. Acak titik itu saja lewat `RollPoint`.
4. Tidak ada pengacakan untuk titik yang sudah punya baris, termasuk titik kosong.

### `private void RollAndRecord()`

`day` bernilai `GameManager.Instance.currentDay` di pulau, dan selalu 1 di kapal (`RegionLootTable.md`). `streamId` bernilai `ShipLoot` di kapal dan `IslandLoot` di pulau.

Untuk tiap titik, panggil `RollPoint(...)`, lalu tambahkan hasilnya ke `states`. Titik kosong dicatat dengan `itemId = ""`, `amount = 0`, dan `isTaken = true`, supaya load tidak mengacak ulang titik itu.

### `public static bool RollPoint(...)`

```csharp
public static bool RollPoint(LootPointDef def, LootTable table, string spawnerId, int pointIndex,
                             RandomStreamId streamId, int day, out string itemId, out int amount)
```

Fungsi murni tanpa status. `RunSeedManager` memakai fungsi yang sama untuk syarat 4 (`minShipFoodItems`, `minShipAmmoItems`, dan `minShipDrinkItems`) lewat `RunLayoutSources`, tanpa membuka `ShipPhase` (`RunSeedManager.md`). Hasil keduanya harus sama persis.

1. `var s = RunSeedManager.Instance.Fork(streamId, spawnerId + "/" + pointIndex)`.
2. Kalau titik punya `guaranteedItems`, pakai elemen pertama. Titik ini tidak bisa kosong. Kembalikan `true`.
3. Peluang kosong: `warehouseEmptyChance` kalau `kind` berisi flag `Warehouse`, dan `lootPointEmptyChance` untuk lainnya. Kalau `s.NextFloat01() < peluang`, kembalikan `false`.
4. Panggil `RegionLootTable.TryRoll(table, kind, day, ref s, out item, out amount)`. Kalau gagal, kembalikan `false` dengan peringatan, karena tabel tidak punya isi untuk `kind` itu.
5. `itemId = item.itemID`, lalu kembalikan `true`.

Urutan pemakaian aliran acak harus tetap: lempar kosong lebih dulu, lalu `TryRoll`. Mengubah urutan ini mengubah semua susunan loot untuk seed yang sama.

### `private void SpawnPoint(in LootPointState st)`

1. Ambil `ItemData` lewat `ItemDatabase.Get(st.itemId)`. Kalau tidak ada, misalnya item dihapus di versi baru, tandai `isTaken = true` dengan peringatan.
2. Munculkan wujudnya:

| Lokasi | Wujud | Langkah |
|---|---|---|
| `ShipPhase` | `ShipInteractable` dari pool `ShipLoot_Generic` (#74) | `SpawnFromPool<ShipInteractable>("ShipLoot_Generic", point.position, point.rotation)`, lalu `Setup(itemData)`. Jumlah ambil memakai `ItemData.shipPickupAmount`, bukan `st.amount` (`ShipInteractable.md`) |
| Pulau | `WorldItem` lewat `WorldItemManager.Spawn` (#66) | `Spawn(new ItemInstance(itemData), st.amount, point.position, despawns: false)`, lalu isi `sourceSpawner = this` dan `sourcePointIndex` |

3. Simpan objeknya ke `liveItems` atau `liveShipItems`.
4. Kalau hasilnya `null` karena pool penuh, biarkan baris tetap `!isTaken`. Objek dicoba lagi saat region dimuat berikutnya, dan loot tidak hilang.

### Saat barang diambil

**`public void MarkTaken(int pointIndex)`**: dipanggil `WorldItem` saat isinya habis diambil (`ItemUseSystem.md`).

1. Abaikan kalau `pointIndex` di luar jangkauan.
2. Isi `isTaken = true` dan `amount = 0` pada baris itu.
3. `liveItems[pointIndex] = null`.

**`public void UpdateRemaining(int pointIndex, int amount)`**: dipanggil `WorldItem` saat sebagian terambil, karena inventori penuh. Isi `amount` baris itu. Tanpa fungsi ini, load akan memulihkan jumlah awal, dan barang berlipat. Lihat "Kasus Khusus".

Di kapal, `ShipInteractable` kembali ke pool sendiri. Spawner tidak perlu tahu, karena kapal tidak disimpan.

### Region dilepas

Mendengar `RegionLoadStateChangedEvent { region == homeRegion, state == Unloading }`:

1. Untuk tiap `liveItems[i]` yang masih milik spawner ini (`sourceSpawner == this` dan `sourcePointIndex == i`), salin `Amount` ke baris `i`. Ini pengaman kalau `UpdateRemaining` terlewat.
2. Kembalikan objeknya lewat `WorldItemManager.Despawn`, lalu kosongkan `liveItems`.
3. `hasSpawned = false`. `restoredJson` tidak dipakai lagi, karena `states` sudah ada di memori. Saat region dimuat kembali, `RestoreState` mengisi data terbaru dari `RegionStateStore`.

Objek yang sudah dikembalikan `WorldItemManager` lebih dulu dilewati, karena `OnReturned` mengosongkan `sourceSpawner`.

### `public string CaptureState()`

Dipanggil `RegionStreaming.CaptureRegion` satu frame setelah `Unloading`, dan oleh `CaptureLoadedRegions` saat save. Tulis `LootSpawnerSaveData { version = 1, points = states }` sebagai JSON. Kalau `hasSpawned == false` dan `states` kosong, kembalikan `restoredJson` apa adanya, supaya data lama tidak terhapus.

**`public void OnRestoreFinished()`**: tidak ada event sticky yang perlu dikirim. Fungsi ini kosong.

**`public int LoadOrder => 0`**, sesuai aturan objek region di `WorldStateSave.md`.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** tidak ada.

**Event yang didengar:** `RegionLoadStateChangedEvent` (#88), hanya untuk `homeRegion`. Berlangganan di `OnEnable`, lepas di `OnDisable`.

| Sistem | Hubungan |
|---|---|
| `RunSeedManager` (#89) | `Fork`. Memakai `RollPoint` untuk syarat 4 lewat `RunLayoutSources` |
| `RegionLootTable` (#103) | `TryRoll`, isi `LootTable`, dan `LootPointKind` |
| `ObjectPooler` | Pool `ShipLoot_Generic` di kapal |
| `WorldItemManager`, `WorldItem` (#66) | Wujud item pulau. `WorldItem` memanggil `MarkTaken` dan `UpdateRemaining` |
| `ShipInteractable`, `LifeboatExtraction` (#74) | Wujud barang kapal, `Setup(ItemData)` |
| `RegionStreaming` (#88), `RegionStateStore`, `WorldStateSave` (#80) | Menyimpan `states` lewat `ISaveable` |
| `ItemDatabase` | Mengubah `itemId` menjadi `ItemData` |
| `GameManager` | `currentDay` untuk skala hari di pulau |
| `ZombieLair` (#112) | Memberi `lootTable` untuk spawner di dalam sarang |

**Batas pekerjaan:** peti yang dibuka pemain milik `LootContainerInteractable` (#17). Node yang tumbuh kembali milik `ResourceRespawn` (#73). Loot di titik tetap tidak tumbuh kembali.

## 4. Kasus Khusus

- **Objek pool:** objek yang diambil kembali ke pool. Spawner hanya memegang status dan rujukan sementara. `WorldItem.OnReturned` mengosongkan `sourceSpawner`, jadi rujukan lama tidak bisa memanggil `MarkTaken` pada spawner yang salah.
- **Game dijeda:** tidak ada logika per frame. Pemuatan region tidak berjalan saat jeda (`RegionStreaming.md`), jadi penyebaran juga menunggu.
- **Permadeath:** status hilang bersama save. Run baru memakai seed baru, jadi susunan berbeda.
- **Save dan load:** `CaptureState` menulis `states` ke `RegionStateStore`. Region yang belum dikunjungi tetap menyebar dengan hasil sama, karena kuncinya tetap. Hanya `day` yang bisa berbeda, karena memakai hari saat region pertama dimuat.
- **`ShipPhase`:** tidak punya save (#82), jadi selalu lewat `RollAndRecord()`. Seed yang sama memberi susunan kapal yang sama. Objek kembali ke pool lewat `ObjectPooler.ReturnAllActive()` saat Fase 1 selesai.
- **Sebagian terambil:** `WorldItem` tetap ada, `UpdateRemaining` mencatat sisanya, dan `MarkTaken` baru dipanggil saat habis.
- **Pool `WorldItem_Generic` habis:** baris tetap belum diambil, dan objek muncul saat region dimuat berikutnya.
- **Item dihapus di versi baru:** baris ditandai diambil dengan peringatan, tanpa error.
- **Titik ditambah di versi baru:** hanya titik baru yang diacak saat load. Titik lama tidak berubah.
- **Air pasang (#91):** titik pulau dilarang di bawah batas pasang. Kalau tetap tersapu `SweepBelow`, barang dianggap hilang, dan `MarkTaken` tidak dipanggil. Saat load, barang muncul lagi di titik asal.
- **Region dimuat ulang cepat:** `hasSpawned` dan pengecekan pemilik di `liveItems` mencegah objek ganda.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** `SpawnOrRestore`, `LootPointState`, `MarkTaken`, `Fork(ShipLoot/IslandLoot, spawnerId + "/" + pointIndex)`, dan kapal tanpa save (`RunSeedManager.md`). `LootPointDef` dan urutan lempar kosong lalu `TryRoll` (`RegionLootTable.md`). Pool kapal (`LifeboatExtraction.md`). `SaveId = spawnerId` dan jalur `RegionStateStore` (`WorldStateSave.md`).

**Usulan yang disetujui developer sebagai nilai awal (dicatat di #85):** peluang kosong 50 persen dan 25 persen untuk gudang.

**Keputusan teknis di blueprint ini:**

- Penyebaran di pulau dipicu event `Loaded`, bukan `Start()`, supaya data save selalu masuk lebih dulu.
- Titik kosong ikut dicatat, supaya load tidak mengacak ulang.
- `RollPoint` menjadi fungsi statis bersama untuk spawner dan pemeriksaan seed.
- Satu titik memegang satu tumpukan. `guaranteedItems` hanya memakai elemen pertama.

**Menunggu file lain:** `ItemUseSystem.md` perlu memanggil `UpdateRemaining` saat item sebagian terambil. Sampai itu ada, pengaman di langkah `Unloading` menutup sebagian besar kasus, tetapi save di tengah region yang masih dimuat tetap membawa jumlah lama.

## 6. Skenario Tes

1. Pakai seed yang sama dua kali. **Hasil:** susunan loot sama, di kapal dan di pulau.
2. Ambil barang, simpan, keluar, lalu lanjutkan. **Hasil:** barang tidak muncul lagi, dan loot lain tetap.
3. Muat ulang region dengan berjalan menjauh lalu kembali. **Hasil:** loot tidak berlipat.
4. Periksa kapal. **Hasil:** barang berupa `ShipInteractable` dari `ShipLoot_Generic`.
5. Periksa pulau. **Hasil:** barang berupa `WorldItem` dengan `sourceSpawner` terisi.
6. Titik dengan `guaranteedItems`. **Hasil:** tidak pernah kosong.
7. Pakai 100 seed lewat `RunSeedAudit`. **Hasil:** titik biasa kosong sekitar 50 persen, dan titik `Warehouse` sekitar 25 persen.
8. Bandingkan hasil `RunSeedManager` syarat 4 dengan isi kapal yang sebenarnya untuk 20 seed. **Hasil:** jumlah makanan, peluru, dan minuman sama persis.
9. Isi inventori sampai penuh, ambil tumpukan peluru, simpan, lalu lanjutkan. **Hasil:** sisa peluru di tanah sama dengan sebelum save.
10. Simpan dengan titik kosong, lalu lanjutkan. **Hasil:** titik itu tetap kosong, dan tidak ada pengacakan ulang.
11. Tambahkan satu titik baru di akhir array, lalu muat save lama. **Hasil:** hanya titik baru yang diacak, dan titik lama sama.
12. Hapus satu item dari `ItemDatabase`, lalu muat save yang memakainya. **Hasil:** peringatan di log, tanpa error, dan titik itu kosong.
13. Kecilkan pool `WorldItem_Generic` ke 1, lalu masuk region dengan banyak loot. **Hasil:** sebagian barang tidak muncul tanpa error, dan muncul setelah region dimuat ulang dengan pool normal.
14. Masuk region pada hari 1 dan hari 20 dengan seed sama, di dua run berbeda. **Hasil:** titik kosong sama, tetapi isi bisa berbeda karena skala hari (#103).
15. Mati di kapal lalu ulangi dengan seed baru. **Hasil:** objek kapal lama kembali ke pool, dan susunan baru muncul tanpa sisa.

## Catatan Implementasi (Sistem pendukung)

- Kode: `Assets/LOGIC/Core_Logic/LootSpawnerSystem.cs` (`LootPointDef`, `LootPointState`, `LootSpawnerSaveData`, `LootSpawnerSystem`).
- **Dengan #88:** `homeRegion` dibaca dari `RegionSceneRoot` induk di `Awake`, `HandleRegionLoadState` tersambung (`Loaded` memanggil `SpawnOrRestore`, `Unloading` memanggil `ReleaseForUnload`), dan spawner tidak mendaftar ke `WorldStateSave`. Kalau spawner baru aktif setelah regionnya `Loaded` (event terlewat), `Start()` menyebar langsung.
- **Jalur cadangan scene uji tetap ada** (keputusan sesi induk, lewat `RegionSaveRouting`): tanpa `RegionStreaming` atau di luar `RegionSceneRoot`, `homeRegion` dari Inspector, spawner mendaftar ke `WorldStateSave` (`LoadOrder` 0), dan penyebaran dipicu `Start()`. Saat melanjutkan save, `Start()` menunggu dan penyebaran dijalankan di `OnRestoreFinished`.
- **`WorldItem.sourceSpawner`/`sourcePointIndex` (#66) sudah ada:** `SpawnPoint` mengisinya, dan `WorldItem` memanggil `MarkTaken`/`UpdateRemaining` sendiri. `SyncLiveItems()` hanya menyalin `Amount` dari objek yang masih milik spawner; objek yang kembali ke pool lewat jalur lain dilepas dari `liveItems` tanpa mengubah baris, sesuai "Kasus Khusus". Array `liveInstances` dihapus.
- Lokasi kapal atau pulau dibaca dari `GameManager.currentPhase` (`SinkingShip` = kapal).
- `UpdateRemaining(i, 0)` diperlakukan sebagai `MarkTaken(i)`.
- `spawnerId` otomatis memakai awalan `ship` kalau `table.location == Ship`, selain itu `r<1-4>` dari `homeRegion`.
- **Keputusan developer (7 November 2026):** Isi `RollPoint` dipindah ke `RollPointCore`. Fungsi baru `RollPointForSeed(kind, guaranteedItems, table, spawnerId, pointIndex, streamId, day, seed, ...)` dipakai `RunLayoutValidator` syarat 4 untuk seed calon, dengan hasil sama persis seperti `RollPoint` (#89). Getter baru `SpawnPointDefs` dibaca menu Editor `RunLayoutSources`.
