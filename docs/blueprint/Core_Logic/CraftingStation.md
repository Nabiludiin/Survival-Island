# Blueprint: CraftingStation.cs (CraftingRecipe & CraftingManager)
**Lokasi:** `docs/blueprint/Core_Logic/CraftingStation.md`
**Deskripsi Utama:** Data resep, jenis stasiun, antrean crafting, dan panel resep. Blueprint ini menggantikan isi `CraftingManager.md` yang lama.
**Issue:** #77
**Rujukan GDD:** Bagian 3 (formula vaksin dan NPC yang mempercepat crafting), bagian 4 (base sebagai lokasi meja crafting), bagian 5 (vaksin dari bahan Region 2, pakaian hangat dan obor). Jenis stasiun, antrean, dan angka resep adalah usulan.

## 1. Arsitektur & Variabel Utama

### Data

```csharp
public enum CraftingStationType { Hand, WoodBench, MedicalBench, Campfire }

[Serializable]
public struct RecipeIngredient { public ItemData item; public int amount; }

[CreateAssetMenu(menuName = "Survival/Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public string recipeId;              // Contoh: rec_vaccine
    public string displayNameKey;        // Kunci teks untuk #104
    public RecipeIngredient[] ingredients;
    public ItemData result;
    public int resultAmount = 1;
    public float craftSeconds = 3f;
    public CraftingStationType station;
    public string techNodeId;            // Kosong kalau tidak terikat tech tree
    public bool startsUnlocked;
}
```

`CookingRecipe : CraftingRecipe` (#44) menambah `canBurn`, `burnAfterSeconds`, dan `burntResult`, dengan `station = Campfire`. Panel `CraftingManager` tidak menampilkan resep `Campfire`. `RecipeDatabase` memuat semua resep dan menyediakan pencarian `recipeId`.

### Kelas

| Kelas | Letak | Tugas |
|---|---|---|
| `CraftingManager` | Manager di `IslandPhase`, `ISaveable` dengan `SaveId = world_recipes` | Antrean, syarat resep, dan penyerahan hasil |
| `CraftingStation : IslandInteractable` | Prefab `Build_WoodBench` dan `Build_MedicalBench` | Mendaftar sebagai stasiun, dan membuka panel lewat `E` |
| `CraftingPanel : UIPanel` | HUD, `UIPanelId.Crafting` | Daftar resep dan antrean. Tidak menjeda game, sesuai #83 |

Api unggun tidak memakai `CraftingStation`. `CampfireManager` dan `CampfirePanel` menangani resep `Campfire`, sehingga `CraftingPanel` dan `RequestCraft` melewatinya.

**Bangunan stasiun:** `BuildableData` milik #79, dengan aturan yang sama seperti peti.

| `buildableId` | Biaya | HP | Dukungan | Navigasi |
|---|---|---|---|---|
| `Build_WoodBench` | 15 `Mat_Wood` | 150 | `FloorOrGround` | `Avoid` |
| `Build_MedicalBench` | 15 `Mat_Wood`, 4 `Mat_Stone` | 150 | `FloorOrGround` | `Avoid` |

### Variabel `CraftingManager`

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `queue` | `List<CraftJob>` | Kosong, kapasitas `maxQueue` | Antrean |
| `stations` | `List<CraftingStation>` | Kosong | Stasiun aktif, didaftarkan di `OnEnable` |
| `availableMask` | `int` | Hanya `Hand` | Jenis stasiun dalam jangkauan |
| `npcSpeedMultiplier` | `float` | 1 | Dari `NPCRoster` (#76) |
| `hitPauseTimer` | `float` | 0 | Jeda setelah diserang |

```csharp
[Serializable]
public class CraftJob { public string recipeId; public float remainingSeconds; }
```

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.crafting.maxQueue` | 5 | Batas antrean |
| `GameBalanceConfig.crafting.stationRangeMeters` | 4 | Jarak memakai stasiun |
| `GameBalanceConfig.crafting.leaveGraceMeters` | 2 | Tambahan jarak sebelum proses berhenti |
| `GameBalanceConfig.crafting.stationCheckSeconds` | 0,5 | Jeda pemeriksaan jarak |
| `GameBalanceConfig.crafting.hitPauseSeconds` | 1,5 | Jeda proses setelah kena serangan |
| `GameBalanceConfig.crafting.npcCrafterSpeedMultiplier` | 1,5 | Skill NPC tukang, dibaca #76 |

Waktu dan bahan tiap resep berada di aset `CraftingRecipe`.

### Resep inti

| `recipeId` | Bahan | Hasil | Detik | Stasiun | Sumber aturan |
|---|---|---|---|---|---|
| `rec_axe` | 3 `Mat_Wood`, 2 `Mat_Stone` | `Tool_Axe` | 4 | `Hand` | Usulan |
| `rec_pickaxe` | 3 `Mat_Wood`, 3 `Mat_Stone` | `Tool_Pickaxe` | 4 | `Hand` | Usulan |
| `rec_hammer` | 2 `Mat_Wood`, 1 `Mat_Stone` | `Tool_Hammer` | 3 | `Hand` | Usulan |
| `rec_torch` | 1 `Mat_Wood`, 1 `Mat_Cloth` | `Tool_Torch` | 2 | `Hand` | Usulan, bersama #65 |
| `rec_campfire` | 5 `Mat_Wood`, 3 `Mat_Stone` | `Kit_Campfire` | 3 | `Hand` | #44, `campfire.recipeWood` dan `recipeStone` |
| `rec_warm_clothes` | 4 `Mat_Hide`, 2 `Mat_Rope` | `Gear_WarmClothes` | 15 | `WoodBench` | Usulan, bersama #65 |
| `rec_vaccine` | 2 `Mat_TreeSap`, 2 `Mat_Mushroom` | `Med_Vaccine` | 10 | `MedicalBench` | `InfectionSystem.md` |
| `rec_antibiotic` | 1 `Mat_TreeSap`, 1 `Mat_Mushroom` | `Med_Antibiotic` | 8 | `MedicalBench` | `DiseaseManager.md` |
| `rec_norit` | 2 `Mat_Charcoal` | `Med_Norit` | 3 | `Hand` | `DiseaseManager.md` |
| `rec_bandage` | 2 `Mat_Cloth` | `Med_Bandage` | 3 | `Hand` | `DiseaseManager.md` |
| `rec_bag_cloth` | 6 `Mat_Cloth`, 2 `Mat_Rope` | `Bag_Cloth` | 5 | `Hand` | `CarryCapacity.md` |
| `rec_bag_leather` | 6 `Mat_Hide`, 4 `Mat_Rope`, 1 `Bag_Cloth` | `Bag_Leather` | 10 | `WoodBench` | `CarryCapacity.md` |

**Resep api unggun (`CookingRecipe`, #44):** `cook_meat`, `cook_fish`, `boil_water`, dan `burn_charcoal`, dengan isi di `raft/CampfireManager.md`.

**Resep barang lempar (#96):** `rec_throw_rock`, `rec_throw_flare`, `rec_throw_molotov`, dan `rec_throw_bomb`, dengan isi di `left-4-dead/ThrowableSystem.md`. Keempatnya didaftarkan hanya kalau `throwable.enabled` bernilai `true`.

**Validasi Editor (#116):** bahan resep tidak boleh memenuhi `ProgressionSafety.IsProtected(item)`.

Semua resep di atas memakai `startsUnlocked = true`. Alat dasar harus tersedia dengan tangan, karena membangun meja sudah butuh palu. Batu pertama berasal dari `PickupInteractable` (#17) dan kayu pertama dari pohon yang dipukul tangan (#73).

## 2. Detail Logika & Fungsi

### Deteksi stasiun

**`private void RefreshAvailability()`**, tiap `stationCheckSeconds` selama panel terbuka atau antrean berisi:

1. `availableMask` dimulai dari `Hand`.
2. Untuk tiap `CraftingStation`, hitung jarak kuadrat ke pemain. Batasnya `stationRangeMeters` kalau panel terbuka, ditambah `leaveGraceMeters` untuk proses yang sudah berjalan.
3. Dalam batas, jalankan `PhysicsQuery.Raycast` dari kamera ke titik tengah stasiun dengan `PhysicsMasks.InteractBlock`. Stasiun dihitung kalau tidak terhalang, atau yang terkena adalah stasiun itu sendiri.
4. Tambahkan jenisnya ke `availableMask`. Kalau mask berubah, panel menggambar ulang.

Karena daftar stasiun kecil dan tidak diperiksa tiap frame, tidak perlu overlap fisika.

### Memulai crafting

**`public CraftResult RequestCraft(CraftingRecipe r)`** mengembalikan `Ok`, `Locked`, `NoStation`, `MissingMaterial`, `NoSpace`, atau `QueueFull`:

1. Tolak `Campfire`, lalu periksa `IsRecipeUnlocked(r)`.
2. Periksa jenis `r.station` di `availableMask`.
3. Periksa `queue.Count < maxQueue`.
4. Periksa semua bahan dengan `InventoryManager.HasItem`.
5. **Periksa ruang lebih dulu:** `InventoryManager.CanFitAfterRemoval(r.ingredients, r.result, r.resultAmount)`. Fungsi ini menghitung slot secara simulasi, tanpa mengubah inventori.
6. Baru sekarang panggil `RemoveItem` untuk tiap bahan, lalu tambahkan `CraftJob` dengan `remainingSeconds = r.craftSeconds`.
7. Terbitkan `CraftQueueChangedEvent`.

Urutan ini memperbaiki bug lama: bahan tidak dihapus sebelum ruang inventori dipastikan.

### Menjalankan antrean

**`void Update()`**, kalau antrean berisi dan `hitPauseTimer` sudah habis:

1. Ambil job pertama yang stasiunnya tersedia. Job yang stasiunnya jauh dilewati, jadi resep tangan tetap berjalan.
2. `remainingSeconds -= Time.deltaTime * npcSpeedMultiplier`.
3. Kalau habis, panggil `CompleteJob`.

**`private void CompleteJob(CraftJob job)`**:

1. `int sisa = InventoryManager.AddItem(new ItemInstance(r.result), r.resultAmount)`. `createdGameHour` diisi saat ini.
2. Kalau inventori sudah penuh lagi karena pemain memungut barang, `sisa` dimunculkan lewat `WorldItemManager.Spawn(..., despawns: false)` di dekat pemain, dengan notifikasi `Inventori penuh, hasil jatuh` (#22). Hasil tidak pernah hilang.
3. Mainkan SFX, hapus job, lalu terbitkan `ItemCraftedEvent { recipeId }` dan `CraftQueueChangedEvent`.

**`public void CancelJob(int index)`** menghapus job dan mengembalikan seluruh bahan lewat `AddItem`. Bahan yang tidak muat menjadi `WorldItem`.

### Tech tree

Daftar `unlockedRecipes` dihapus, sehingga resep hanya punya satu sumber status.

```csharp
public bool IsRecipeUnlocked(CraftingRecipe r) =>
    r.startsUnlocked || (!string.IsNullOrEmpty(r.techNodeId) && TechTreeManager.Instance.IsUnlocked(r.techNodeId));
```

`TechTreeManager.UnlockNode` menerbitkan `TechNodeUnlockedEvent { nodeId }`. `CraftingManager` mendengarnya, lalu menampilkan notifikasi `Resep baru` untuk resep dengan node itu. Syarat membuka node berada di luar issue ini (#34). `CraftingManager` menyimpan status node, karena `world_recipes` adalah satu entri untuk keduanya.

### Panel resep

`CraftingPanel` dibuka dengan `C` (`toggleAction`, #18) atau `E` di stasiun, lalu masuk lewat `UIFocusStack.Push`.

| Kondisi resep | Tampilan | Tombol buat |
|---|---|---|
| Terbuka, bahan dan stasiun lengkap | Normal | Aktif |
| Bahan kurang | Jumlah bahan yang kurang berwarna merah | Nonaktif |
| Stasiun jauh | Redup, dengan teks `Butuh Meja Kayu` | Nonaktif |
| Terkunci, node prasyarat sudah terbuka | Redup, nama disembunyikan `???` | Nonaktif |
| Terkunci, node prasyarat belum terbuka | Disembunyikan | Tidak ada |

Panel mendengar `InventoryChangedEvent`, `CraftQueueChangedEvent`, dan `TechNodeUnlockedEvent` hanya saat terbuka. Saat ditutup, antrean tetap berjalan, dan `HUDManager` menampilkan ikon kecil dari `CraftQueueChangedEvent`.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `ItemCraftedEvent`, `CraftQueueChangedEvent`.

**Event yang didengar:** `InventoryChangedEvent` (panel), `TechNodeUnlockedEvent`, `PlayerDamagedEvent` (`hitPauseTimer = hitPauseSeconds`), dan `PlayerDiedEvent` (berhenti).

| Sistem | Hubungan |
|---|---|
| `InventoryManager`, `ItemUseSystem` (#66) | `HasItem`, `RemoveItem`, `AddItem`, dan `CanFitAfterRemoval` |
| `WorldItemManager` (#66) | Menampung hasil dan bahan yang tidak muat |
| `TechTreeManager`, #34 | `IsUnlocked`, `UnlockNode`, dan node resep |
| `NPCRoster` (#76) | `SetNpcSpeedMultiplier(float)`. Syarat NPC aktif ditentukan #76 |
| `CampfireManager` (#44) | Memakai `CookingRecipe`, `RecipeDatabase`, dan resep `rec_campfire` |
| `BuildMenu` (#79), `BaseHealth` (#59) | Membangun dan merusak meja |
| `ToolRepairSystem` (#113) | Memakai `availableMask`, dan menambah tab Perbaiki di panel |
| `WorldStateSave` (#80) | Menyimpan `world_recipes` |

**Batas pekerjaan:** syarat membuka resep di #34, memasak di #44, dan aturan bangunan di #79.

## 4. Kasus Khusus

- **Objek pool:** `CraftingManager` tidak memakai pool. Hasil yang jatuh memakai `WorldItem_Generic`, dengan reset dari #66.
- **Game dijeda:** `Time.deltaTime` bernilai 0, jadi antrean berhenti. Panel `Crafting` sendiri tidak menjeda game.
- **Permadeath:** antrean dan status node hilang bersama save run (#82). Tidak ada resep yang terbawa ke run baru.
- **Save dan load:** `CaptureState` menyimpan `{ unlockedNodeIds[], queue[] { recipeId, remainingSeconds } }`. Bahan tidak disimpan, karena sudah dipotong dari inventori. Saat dimuat, `recipeId` yang tidak ditemukan dibuang dengan peringatan. `availableMask` dihitung ulang.
- **Pemain menjauh:** job meja berhenti tanpa hilang, lalu lanjut saat pemain kembali ke stasiun sejenis. Job tangan tetap berjalan.
- **Pemain diserang:** proses berhenti sesaat selama `hitPauseSeconds`, tanpa membatalkan job dan tanpa menutup panel.
- **Meja hancur:** job terhenti sampai ada meja sejenis. Pemain bisa membatalkannya untuk mendapatkan bahan kembali.
- **Membawa Heavy Item:** `E` terhalang oleh `blockedWhileCarrying`. Tombol `C` tetap membuka panel, dan antrean tetap berjalan.
- **Bahan berupa item unik:** `Bag_Cloth` yang sedang dipakai tidak dihitung, karena berada di luar slot (#118).
- **Bahan dengan umur busuk:** pembatalan mengembalikan item baru dengan umur baru. Risiko penyalahgunaan kecil, karena bahan resep di daftar inti tidak busuk.
- **Run tanpa meja:** semua alat dasar dan obor tetap bisa dibuat dengan tangan, sehingga pemain tidak terkunci.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** resep `rec_vaccine` dan `startsUnlocked` (`InfectionSystem.md`). `rec_campfire` dibuat dengan tangan (`CampfireManager.md`). Resep obat (`DiseaseManager.md`). Resep tas (`CarryCapacity.md`). Panel `Crafting` tidak menjeda (`UIFocusStack.md`). Tombol `C` (`InputManager.md`).

**Keputusan teknis di blueprint ini:**

- Status resep dihitung dari data dan tech tree, tanpa daftar kedua.
- Ruang inventori diperiksa lewat simulasi sebelum bahan dipotong.
- Stasiun mendaftar sendiri, jadi pemeriksaan jarak tidak butuh overlap.

**Usulan yang disetujui developer sebagai nilai awal:**

1. **Jenis stasiun:** `Hand`, `WoodBench`, `MedicalBench`, dan `Campfire`, dengan biaya meja di tabel bagian 1.
2. **Proses:** antrean berurutan sampai 5 job. Bahan dipotong saat masuk antrean, dan kembali penuh saat dibatalkan.
3. **Pengaruh NPC:** NPC tukang mempercepat crafting 1,5 kali. GDD hanya menyebut "mempercepat".
4. **Menjauh dan diserang:** job meja berhenti saat pemain lebih dari 6 meter, dan semua job berhenti 1,5 detik saat kena serangan. Tidak ada job yang batal otomatis.
5. **Hasil inti:** bahan, waktu, dan stasiun tiap resep bertanda usulan, termasuk `Gear_WarmClothes` yang menunggu #65. Semua resep inti terbuka sejak awal.
6. **Resep terkunci:** redup kalau prasyaratnya sudah terbuka, dan disembunyikan kalau belum.

## 6. Skenario Tes

1. Isi bahan `rec_hammer`, tekan `C`, lalu buat. **Hasil:** bahan berkurang, dan setelah 3 detik `Tool_Hammer` masuk.
2. Penuhi inventori dengan barang yang tidak bertumpuk, lalu buat `rec_axe` yang bahannya tidak mengosongkan slot. **Hasil:** ditolak `NoSpace`, dan bahan tetap utuh.
3. Buat resep yang bahannya mengosongkan satu slot saat inventori penuh. **Hasil:** diterima, karena simulasi menghitung slot itu.
4. Masukkan job, lalu penuhi inventori sebelum selesai. **Hasil:** hasil jatuh sebagai `WorldItem` dengan notifikasi.
5. Antre 5 job, lalu tambah satu. **Hasil:** ditolak `QueueFull`.
6. Batalkan job. **Hasil:** semua bahan kembali.
7. Buka panel jauh dari meja. **Hasil:** `rec_warm_clothes` redup dengan teks `Butuh Meja Kayu`.
8. Mulai `rec_warm_clothes` di meja, lalu jalan 10 meter. **Hasil:** proses berhenti, job tangan tetap jalan, dan proses lanjut saat kembali.
9. Hancurkan meja lewat `DevConsole` saat job berjalan. **Hasil:** job terhenti tanpa error, dan bisa dibatalkan.
10. Aktifkan `SetNpcSpeedMultiplier(1,5)`. **Hasil:** resep 15 detik selesai dalam 10 detik.
11. Terkena serangan saat crafting. **Hasil:** sisa waktu berhenti 1,5 detik, dan panel tetap terbuka.
12. Buka Pause selama 10 detik. **Hasil:** sisa waktu tidak berkurang.
13. Panggil `UnlockNode` untuk resep terkunci. **Hasil:** resep muncul tanpa membuka ulang panel, dan notifikasi tampil.
14. Simpan dengan 2 job dan keluar, lalu lanjutkan. **Hasil:** antrean dan sisa waktu sama, dan bahan tidak terpotong dua kali.
15. Mati lalu mulai run baru. **Hasil:** antrean kosong, dan hanya resep awal yang terbuka.
16. Buat `rec_vaccine` tanpa `MedicalBench`. **Hasil:** ditolak `NoStation`.
