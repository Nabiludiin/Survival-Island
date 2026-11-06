# Blueprint: CampfireManager.cs
**Lokasi:** `Survival Island/doc/blueprint/raft/CampfireManager.md`
**Deskripsi Utama:** Satu komponen per api unggun. Mengatur panel masak, waktu matang, bahan bakar, makanan gosong, dan merebus air.
**Issue:** #44
**Rujukan GDD:** Bagian 4 (Diary Bab 1: cara membuat api unggun) dan bagian 3 (lapar dan risiko sakit). Waktu masak 30 detik berasal dari checklist issue. Bahan bakar, gosong, dan rebus air adalah usulan, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

### Pola

`CampfireManager : IslandInteractable, ISaveable` berada di prefab `Build_Campfire`. Tidak ada manager global, sehingga pemain boleh punya beberapa api unggun. Panel `CampfirePanel : UIPanel` memakai `UIPanelId.Campfire` yang sudah ada di #83, dan tidak menjeda game.

Prefab `Build_Campfire` juga memasang komponen `BaseTrap` dan anak `TriggerZone`, dengan `TrapKind.Campfire` yang aktif hanya saat `IsLit` (#26), serta `LightSource` (#98).

### Data resep

Resep memasak adalah `CookingRecipe`, turunan `CraftingRecipe` (#77), dengan `station = CraftingStationType.Campfire`. Jadi semua resep berada di satu `RecipeDatabase`, dan `CraftingManager` tidak menampilkan resep api unggun di panelnya.

| Field tambahan | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `canBurn` | `bool` | `true` | Bisa gosong kalau dibiarkan |
| `burnAfterSeconds` | `float` | `campfire.burnAfterSeconds` | Waktu sampai gosong setelah matang |
| `burntResult` | `ItemData` | `Food_Burnt` | Hasil gosong |

`craftSeconds` di `CraftingRecipe` menjadi lama memasak.

| Resep | Bahan | Hasil | Waktu | Gosong |
|---|---|---|---|---|
| `cook_meat` | `Food_RawMeat` | `Food_CookedMeat` | 30 detik | Ya |
| `cook_fish` | `Food_RawFish` | `Food_CookedFish` | 30 detik | Ya |
| `boil_water` | `Drink_DirtyWater` | `Drink_CleanWater` | 20 detik | Tidak |
| `burn_charcoal` | `Mat_Wood` | `Mat_Charcoal` | 30 detik | Tidak |

Resep ini terbuka sejak awal. `Mat_Charcoal` adalah bahan `Med_Norit` (#45).

**Data `Food_Burnt`:** hunger 5, pakai 2 detik, tidak membusuk, dan `poisonChance` 0. Item ini memakai `ItemData` biasa dan mengikuti tabel konsumsi di `ItemUseSystem.md`.

### Baris masak

```csharp
public enum CookRowState { Empty, Cooking, Done, Burnt }

[Serializable]
public class CookRow
{
    public ItemInstance input;     // Bahan mentah, satu item
    public ItemInstance output;    // Hasil, satu item
    public string recipeId;
    public float progressSeconds;  // Waktu masak yang sudah berjalan
    public float overcookSeconds;  // Waktu sejak matang
    public CookRowState state;
}
```

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `rows` | `CookRow[]` | Panjang `campfire.cookRowCount` | Baris masak |
| `fuelSeconds` | `float` | 0 | Sisa bahan bakar |
| `IsLit` | `bool` | `false` | Api menyala |
| `lastTickGameHour` | `float` | Jam game saat dibuat | Titik waktu tick terakhir |
| `tickTimer` | `float` | 0 | Jeda tick |
| `homeRegion` | `RegionType` | Dari `RegionStreaming.GetRegionAt` | Region cuaca yang dibaca |

### Bahan bakar

`ItemData` mendapat field `fuelSeconds` dengan nilai awal 0. Nilai lebih dari 0 berarti item bisa dibakar.

| Item | `fuelSeconds` |
|---|---|
| `Mat_Wood` | 60 |
| `Mat_Charcoal` | 120 |

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.campfire.cookSeconds` | 30 | Lama masak daging dan ikan, dari issue |
| `GameBalanceConfig.campfire.boilSeconds` | 20 | Lama merebus air |
| `GameBalanceConfig.campfire.burnAfterSeconds` | 30 | Waktu sampai gosong setelah matang |
| `GameBalanceConfig.campfire.cookRowCount` | 3 | Jumlah baris masak |
| `GameBalanceConfig.campfire.maxFuelSeconds` | 300 | Batas bahan bakar |
| `GameBalanceConfig.campfire.tickIntervalSeconds` | 0,25 | Jeda perhitungan |
| `GameBalanceConfig.campfire.rainFuelMultiplier` | 1,5 | Pengali konsumsi saat `Rain` kalau tidak beratap |
| `GameBalanceConfig.campfire.stormExtinguishes` | `true` | `Storm` dan `Blizzard` memadamkan api tak beratap |
| `GameBalanceConfig.campfire.warmthRadius` | 5 | Radius panas, dibaca #65 |
| `GameBalanceConfig.campfire.recipeWood` | 5 | Bahan `Kit_Campfire` |
| `GameBalanceConfig.campfire.recipeStone` | 3 | Bahan `Kit_Campfire` |

## 2. Detail Logika & Fungsi

### Membuat dan menaruh api unggun

Resep `rec_campfire` (#77) dibuat dengan tangan dan menghasilkan `Kit_Campfire`. Pakai `Kit_Campfire` lewat menu item: `ItemUseSystem` memanggil `BuildManager.BeginPlacementFromItem(instanceId, Build_Campfire)` (#79), dan item berkurang setelah penempatan berhasil. Palu tidak diperlukan, supaya api unggun tersedia sejak awal, sesuai Diary Bab 1. Larangan pantai tetap berlaku.

### Membuka panel

`protected override bool OnInteract(in InteractContext ctx)` memanggil `UIFocusStack.Instance.Push(CampfirePanel.Instance)`, setelah `CampfirePanel.Bind(this)`. Panel tertutup oleh Esc (`closableByEsc`).

### Panel `CampfirePanel`

```text
[Bahan bakar: ikon, bar, sisa detik, Tambah Kayu]
[Input 1] --progres--> [Output 1]
[Input 2] --progres--> [Output 2]
[Input 3] --progres--> [Output 3]
[Grid inventori pemain]
```

| Aksi pemain | Syarat | Hasil |
|---|---|---|
| Seret item ke kotak input, atau klik kanan item (`UI/RightClick`) | Ada resep untuk item itu, ada baris `Empty`, dan item tidak busuk | `InsertToRow(slot)` |
| Seret bahan bakar ke kotak bahan bakar, atau klik `Tambah Kayu` | `fuelSeconds < maxFuelSeconds` | `AddFuel(slot, 1)` |
| Klik kotak output, atau klik kanan | Output terisi, dan ada ruang | `CollectRow(index)` |
| Klik kotak input yang masih memasak | Kotak terisi | `RemoveInput(index)`, progres hilang, dan bahan kembali |
| Tombol `Ambil Semua` | Ada output terisi | `CollectRow` untuk tiap baris |

- **Warna:** progres kuning saat memasak, hijau saat matang, merah saat hampir gosong, dan hitam saat `Burnt`.
- **Item yang ditolak:** kotak berkedip dan menampilkan alasan, misalnya `Tidak bisa dimasak` atau `Sudah busuk`.
- **Panel hanya memanggil fungsi `CampfireManager`,** sesuai aturan UI di `GameEventBus.md`.

### Tick waktu

Semua perhitungan memakai jam game, supaya satu rumus berlaku saat game berjalan, dijeda, saat tidur, dan saat region kembali dimuat.

**`void Update()`**: tambah `tickTimer` dengan `Time.deltaTime`. Kalau melewati `tickIntervalSeconds`, panggil `Simulate()`, lalu nol-kan timer.

**`private void Simulate()`**:

```csharp
float now = DayNightDirector.Instance.TotalGameHours;
float sim = (now - lastTickGameHour) * 3600f / b.time.timeMultiplier;   // Detik setara
lastTickGameHour = now;
if (sim <= 0f) return;
TickFire(sim);
TickRows(sim);
```

`sim` bernilai sama dengan detik nyata saat bermain normal, karena 1 jam game sama dengan 37,5 detik. Saat `SleepSystem` mempercepat jam (#67), `sim` ikut membesar.

**`private float TickFire(float sim)`**:

1. Kalau `!IsLit`, selesai.
2. Baca `WeatherType w = RegionalWeather.Instance.GetWeather(homeRegion)`.
3. Kalau tidak beratap dan `stormExtinguishes` dan `w` adalah `Storm` atau `Blizzard`: `Extinguish("Api padam karena badai")`. Bahan bakar tersisa tetap ada.
4. Kurangi `fuelSeconds` dengan `sim`, dikali `rainFuelMultiplier` kalau tidak beratap dan `w == Rain`.
5. Kalau `fuelSeconds <= 0`: `fuelSeconds = 0`, panggil `Extinguish("Api padam")`.

Atap dibaca lewat `PhysicsQuery.Raycast` ke atas dengan `PhysicsMasks.Sunlight`, dengan aturan cek ulang yang sama seperti `RainwaterCollector` (#43).

**`private void TickRows(float sim)`**, untuk tiap baris:

| Status | `IsLit` | Tindakan |
|---|---|---|
| `Cooking` | `true` | `progressSeconds += sim * NPCRoster.GetSkillMultiplier(NPCSkill.Cook)` (#76). Bahan bakar tidak ikut dipercepat. Saat mencapai `craftSeconds`: `output` baru dari `CreateResult()`, `input = null`, `state = Done` |
| `Cooking` | `false` | Tidak berubah. Progres ditahan, tidak hilang |
| `Done` | `true`, dan `canBurn` | `overcookSeconds += sim`. Saat mencapai `burnAfterSeconds`: `output` diganti `Food_Burnt`, `state = Burnt` |
| `Done` | `false` | Tidak berubah. Api mati tidak membuat gosong |
| `Burnt`, `Empty` | Apa pun | Tidak berubah |

**`private ItemInstance CreateResult(CookRow row)`** membuat `ItemInstance` baru dengan `createdGameHour = now`, sesuai `ItemUseSystem.md`. Saat matang, `ItemCookedEvent { recipeId, isBurnt = false }` terbit. Saat gosong, event yang sama terbit dengan `isBurnt = true`. Notifikasi `Makanan matang` hanya muncul kalau panel tertutup dan pemain berada dalam 15 meter.

### Fungsi utama

**`public bool InsertToRow(int inventorySlot)`**:

1. Ambil item. Cari `CookingRecipe` lewat `RecipeDatabase.FindCookingByInput(itemId)`. Kalau tidak ada, tolak.
2. Kalau `ItemUseSystem.GetFreshness` menghasilkan `Rotten`, tolak. Kalau tidak dilarang, pemain bisa mengubah makanan busuk menjadi makanan segar.
3. Cari baris `Empty`. Kalau tidak ada, tolak.
4. `InventoryManager.RemoveFromSlot(slot, 1)`, isi `row.input`, `recipeId`, `state = Cooking`, `progressSeconds = 0`, dan `overcookSeconds = 0`.

Baris memuat satu item. Klik kanan pada tumpukan mengisi baris kosong satu per satu sampai habis.

**`public bool AddFuel(int inventorySlot, int amount)`**:

1. Tolak kalau `fuelSeconds >= maxFuelSeconds`, atau item tidak punya `fuelSeconds`.
2. `RemoveFromSlot(slot, amount)`, lalu `fuelSeconds = Min(maxFuelSeconds, fuelSeconds + itemFuel * amount)`.
3. Kalau `!IsLit` dan cuaca mengizinkan, panggil `Ignite()`. Penyalaan gratis, karena GDD tidak mengatur alat api.

**`public bool CollectRow(int index)`**:

1. `int sisa = InventoryManager.Instance.AddItem(row.output, 1)`.
2. Kalau `sisa > 0`, tampilkan `Inventori penuh`, kembalikan `false`.
3. Kosongkan baris, lalu `state = Empty`.

**`private void Ignite()`** dan **`private void Extinguish(string message)`** mengatur `IsLit`, partikel api lewat `VFXManager` (#15), suara, dan cahaya. Keduanya memanggil `LightSource.SetLit`, dan api unggun tidak dihitung dalam tarikan cahaya (#98). Kalau `Ignite` dipanggil saat `Storm` tak beratap, kembalikan penolakan `Terlalu berangin`.

### Sumber panas

`WarmthSystem` (#65) membaca `CampfireManager.ActiveLit`, daftar statis yang diisi `Ignite` dan dilepas `Extinguish` serta `OnDisable`, lalu membandingkan jarak dengan `warmthRadius`. Isi nilai panasnya milik #65.

### Hancur

Saat `StructureDestroyedEvent` untuk `StructureId` milik api ini: panggil `Extinguish`, tutup panel lewat `UIFocusStack.Close(panel, Forced)` kalau terikat, lalu keluarkan `input` dan `output` sebagai `WorldItem` lewat `WorldItemManager.Spawn(..., despawns: true)`.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `ItemCookedEvent`.

**Event yang didengar:** `StructureBuiltEvent`, `StructureDestroyedEvent`.

| Sistem | Hubungan |
|---|---|
| `IslandInteractable` (#17) | Kelas dasar, dan `E` membuka panel |
| `UIFocusStack` (#83) | Panel `Campfire`, tanpa jeda |
| `CraftingStation` (#77) | `CookingRecipe`, `RecipeDatabase`, dan resep `rec_campfire` |
| `BuildMenu` (#79), `BuildManager` | `Build_Campfire`, dan `BeginPlacementFromItem` |
| `InventoryManager`, `ItemUseSystem` (#66) | Slot, `ItemInstance`, dan `GetFreshness` |
| `DiseaseManager` (#45) | Daging matang tidak meracuni, dan `Mat_Charcoal` menjadi Norit |
| `HydrationSystem` (#43) | Merebus air kotor |
| `FishingSystem` (#55), `WildlifeAI` (#61) | Sumber `Food_RawFish` dan `Food_RawMeat` |
| `WarmthSystem` (#65), `LightSourceSystem` (#98), `FireSystem` (#97) | Membaca api menyala |
| `BaseDefense` (#26) | `BaseTrap` dan `TriggerZone` di prefab |
| `NPCRoster` (#76) | `GetSkillMultiplier(NPCSkill.Cook)` |
| `RegionalWeather` (#115) | `GetWeather` |
| `DayNightDirector` | `TotalGameHours` |
| `BaseHealth` (#59) | HP api unggun |
| `WorldStateSave` (#80) | Menyimpan status |

**Batas pekerjaan:** resep umum di #77, nilai panas di #65, dan api yang menyebar di #97.

## 4. Kasus Khusus

- **Objek pool:** api unggun tidak memakai pool, karena jarang dibuat. Item yang keluar saat hancur memakai pool `WorldItem_Generic` (`ObjectPooler.md`). Efek asap dan api mengikuti `VFXManager` (#15).
- **Game dijeda:** `TotalGameHours` berhenti, jadi `sim` bernilai 0. Panel `Campfire` tidak menjeda, jadi membuka panel tetap membakar bahan bakar.
- **Permadeath:** semua api unggun dan isinya terhapus bersama save run (#82).
- **Save dan load:** disimpan `fuelSeconds`, `IsLit`, `lastTickGameHour`, dan tiap `CookRow` dengan `ItemInstance`, `recipeId`, `progressSeconds`, `overcookSeconds`, serta `state`. `SaveId = StructureId`. Setelah dimuat, `Simulate()` tidak mengejar waktu, karena jam game disimpan bersama.
- **Region dilepas:** `structureRoot` dimatikan, dan `Update` berhenti. Saat `OnEnable`, `Simulate()` berjalan sekali dengan selisih jam game. Makanan yang ditinggal bisa matang, gosong, atau api bisa padam.
- **Tidur:** jam game lebih cepat, sehingga makanan yang ditinggal saat tidur gosong kalau bahan bakar cukup. Ini sengaja.
- **Bahan bakar habis saat memasak:** progres ditahan, tidak hilang. Menambah kayu melanjutkannya.
- **Panel terbuka saat pemain diterkam:** `UIFocusStack` menutup panel lewat `CloseAllUnlocked`, dan api tetap berjalan.
- **Memasak saat panel tertutup:** tetap berjalan. Panel hanya tampilan.
- **Inventori penuh saat mengambil:** hasil tetap di baris, dan tetap bisa gosong.
- **Makanan `Stale` dimasak:** boleh. Hasil baru memakai `createdGameHour` saat matang.
- **Dua pemakai satu `CampfirePanel`:** hanya satu panel. `Bind` mengganti api yang terikat saat panel dibuka.
- **Hujan pada `IsLit`:** api hanya lebih boros. `Storm` dan `Blizzard` memadamkan api tak beratap.
- **`Kit_Campfire` dibatalkan:** item tidak berkurang kalau penempatan batal.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** panel `Campfire` tidak menjeda (`UIFocusStack.md`). `createdGameHour` baru pada hasil matang (`ItemUseSystem.md`). Waktu matang 30 detik dan api mati kalau kayu habis (checklist issue ini).

**Keputusan teknis di blueprint ini:**

- `CookingRecipe` mewarisi `CraftingRecipe`, sehingga semua resep di satu database.
- Waktu dihitung dari selisih jam game, tanpa timer terpisah per baris.
- Baris memuat satu item, supaya tidak ada tumpukan dengan tingkat matang berbeda.

**Usulan yang disetujui developer sebagai nilai awal:**

1. **Waktu gosong:** 30 detik setelah matang, dan hasilnya `Food_Burnt` dengan lapar 5.
2. **Bahan bakar:** 1 kayu 60 detik, batas 300 detik, dan menambah kayu otomatis menyalakan api tanpa alat.
3. **Tiga baris masak** per api unggun.
4. **Makanan busuk tidak bisa dimasak.**
5. **Rebus air:** air kotor menjadi bersih dalam 20 detik.
6. **Cuaca:** `Rain` menghabiskan bahan bakar 1,5 kali, sedangkan `Storm` dan `Blizzard` memadamkan api tak beratap.
7. **Api unggun:** dibuat dari kit dengan tangan, 5 kayu dan 3 batu, tanpa palu.
8. **Arang:** kayu dibakar menjadi `Mat_Charcoal`, sebagai bahan Norit (#45).

## 6. Skenario Tes

1. Buat `Kit_Campfire` dengan 5 kayu dan 3 batu, lalu tempatkan di tanah. **Hasil:** api unggun muncul tanpa palu, dan kit berkurang.
2. Coba tempatkan di pasir. **Hasil:** hologram merah.
3. Tekan `E`. **Hasil:** panel terbuka, waktu tetap berjalan, dan zombie tetap bisa mendekat.
4. Masukkan 1 kayu. **Hasil:** api menyala, dan bar menunjukkan 60 detik.
5. Masukkan `Food_RawMeat`. **Hasil:** setelah 30 detik, muncul `Food_CookedMeat`, dan `ItemCookedEvent` terkirim.
6. Biarkan 30 detik lagi. **Hasil:** berubah menjadi `Food_Burnt`, dengan event `isBurnt = true`.
7. Makan `Food_Burnt`. **Hasil:** lapar naik 5, tanpa keracunan.
8. Masak dengan 10 detik bahan bakar tersisa. **Hasil:** api padam, progres berhenti di 10 detik. Tambah kayu. **Hasil:** progres lanjut.
9. Masukkan `Food_RawMeat` berstatus `Rotten`. **Hasil:** ditolak dengan pesan.
10. Rebus `Drink_DirtyWater`. **Hasil:** menjadi `Drink_CleanWater` dalam 20 detik, dan tidak gosong.
11. Isi tiga baris, lalu coba baris keempat. **Hasil:** ditolak.
12. Keluarkan input yang memasak. **Hasil:** bahan kembali, dan progres hilang.
13. Isi inventori, lalu ambil output. **Hasil:** ditolak, dan output tetap di baris.
14. Paksa `Rain` dengan api tak beratap. **Hasil:** bahan bakar berkurang 1,5 kali. Paksa `Storm`. **Hasil:** api padam.
15. Bangun atap di atasnya, lalu paksa `Storm`. **Hasil:** api tetap menyala.
16. Buka Pause selama 30 detik saat memasak. **Hasil:** progres tidak bertambah.
17. Taruh daging, lalu tidur semalam dengan bahan bakar cukup. **Hasil:** daging menjadi `Food_Burnt` saat bangun.
18. Tinggalkan region sampai dilepas selama 3 jam game, lalu kembali. **Hasil:** progres dan bahan bakar berubah sesuai waktu yang lewat.
19. Simpan saat masak, keluar, lalu lanjutkan. **Hasil:** progres, bahan bakar, dan isi baris sama.
20. Hancurkan api unggun saat panel terbuka. **Hasil:** panel tertutup, dan isi jatuh sebagai `WorldItem`.
21. Mati, lalu mulai run baru. **Hasil:** tidak ada api unggun dari run lama.

## 7. Catatan Implementasi (Sistem pendukung)

- File: `Assets/LOGIC/raft/CampfireManager.cs` (`CookRow`, `CookRowState`, `CampfireReject`, `CampfireManager`), `CookingRecipe.cs`, `CampfirePanel.cs`. Event `ItemCookedEvent` di `GameEvents.GrupG.cs`.
- `RecipeDatabase.FindCookingByInput` belum ada dan `RecipeDatabase` adalah kode lama, jadi pencarian ditulis sebagai `CookingRecipe.FindByInput(itemId)`. Bahan pertama resep dianggap input.
- `CookingRecipe.timeSource` memilih lama masak: `campfire.cookSeconds` (nilai awal), `campfire.boilSeconds` (untuk `boil_water`), atau `craftSeconds` aset.
- Config tambahan (usulan teknis): `campfire.skyCheckDistance`, `structureRecheckRadius` (sama dengan #43), dan `cookedNotifyDistance` (15 meter dari teks blueprint).
- `CampfireManager.IsNearLit(pos)` ditambahkan untuk `WarmthSystem` (#65).
- Belum ada (TODO di kode): `LightSource.SetLit` (#98), `BaseTrap` (#26), `NPCRoster.GetSkillMultiplier` (#76, pengali 1), `FireSystem` (#97), `RegionStreaming` (#88, `homeRegion` dari `ZoneTracker.Active`). `IslandInteractable` (#17) belum ada, jadi menerapkan `IInteractable` langsung.
- Panel: seret-lepas ke kotak input belum ada. Klik kiri atau kanan item inventori memasukkannya ke baris kosong (klik kanan mengisi beberapa baris dari satu tumpukan). Bahan bakar lewat tombol `Tambah Kayu`, atau klik item yang hanya bahan bakar.
- Pemakaian `Kit_Campfire` lewat `ItemUseSystem` (memanggil `BuildManager.BeginPlacementFromItem`) belum ada di kode lama.
