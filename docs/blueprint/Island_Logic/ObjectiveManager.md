# Blueprint: ObjectiveManager.cs (DiaryEntry)
**Lokasi:** `docs/blueprint/Island_Logic/ObjectiveManager.md`
**Deskripsi Utama:** Tiga bab Diary dengan lokasi dan hadiahnya, serta daftar tujuan bertahap yang tampil di HUD dan tetap maju walaupun pemain mengerjakannya tidak berurutan.
**Issue:** #81
**Rujukan GDD:** Bagian 4 (Diary Bab 1 sampai 3 dan Blueprint Jembatan), bagian 1 dan 3 (syarat menang), serta bagian 5 (urutan region dan logistik). Tempat Bab 2 dan 3 adalah usulan yang menyimpang dari GDD bagian 4, yang menempatkan buku Diary di shelter tua. Hadiah Bab 1 dan tampilan HUD adalah usulan.

## 1. Arsitektur & Variabel Utama

`ObjectiveManager` adalah singleton di scene `IslandPhase` (#82) dan `ISaveable` dengan `SaveId = world_objectives` dan `LoadOrder = 110`. Tujuan didorong event, tanpa `Update`. `DiaryManager` hanya menampilkan isi bab.

```csharp
public enum DiaryRewardKind { None, GiveItem, RevealObjectives }
public enum ObjectiveCondition { BuildCount, ItemTotal, BridgeBuilt, ItemCrafted, RegionEntered, PartsInstalled, Escaped }

[Serializable]
public struct DiaryReward
{
    public DiaryRewardKind kind;
    public ItemData item; public int amount;
}

[CreateAssetMenu(menuName = "Survival/Diary Entry")]
public class DiaryEntry : ScriptableObject
{
    public int chapter;                // 1 sampai 3
    public string titleKey;            // diary.ch<chapter>.title (#104)
    public string bodyKey;             // diary.ch<chapter>.body (#104)
    public RegionType region;          // Lokasi buku
    public Vector3 markerPosition;     // Data, karena region bisa tidak dimuat
    public int requiresChapter;        // 0 berarti tanpa syarat. Hanya mengatur penanda arah
    public DiaryReward[] rewards;
}

[CreateAssetMenu(menuName = "Survival/Objective Data")]
public class ObjectiveData : ScriptableObject
{
    public string objectiveId;
    public string title;
    public int order;
    public int revealedByChapter;      // 0 berarti terlihat sejak awal
    public ObjectiveCondition condition;
    public string targetId;            // Item, resep, atau awalan buildableId
    public RegionType targetRegion;
    public int targetAmount;
    public Vector3 markerPosition;
    public bool hasMarker;
}
```

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Instance` | `ObjectiveManager` | `null` | Singleton |
| `chapters` | `DiaryEntry[]` | 3 aset | Semua bab |
| `objectives` | `ObjectiveData[]` | 8 aset, urut `order` | Semua tujuan |
| `foundChapters` | `List<int>` | Kosong | Bab yang sudah ditemukan |
| `grantedRewards` | `List<int>` | Kosong | Bab yang hadiahnya sudah diberikan |
| `completed` | `HashSet<string>` | Kosong | ID tujuan selesai |
| `ActiveObjective` | `ObjectiveData` | `null` | Tujuan yang tampil di HUD |

### Bab Diary

| Bab | Tempat (Bab 2 dan 3 usulan) | Hadiah | Isi sesuai GDD |
|---|---|---|---|
| 1 | Shelter tua hancur, Region 1 | `Kit_Campfire` x1 | Jangan tidur di pantai, zombie terbakar matahari, cara membuat api unggun, memancing dengan kail di tebing karang |
| 2 | Usulan: pos kemah penulis di tepi jurang, Region 1 | Blueprint Jembatan (`RegionGateSystem.HasBridgeBlueprint`), dan tujuan kelompok 2 | Penulis terinfeksi, vaksin dari getah dan jamur Region 2, Blueprint Jembatan |
| 3 | Usulan: pos terakhir penulis di hutan, Region 2 | Petunjuk helipad, dan tujuan kelompok 3 | Helipad dan landasan di puncak es, suku cadang tertinggal di desa |

**Usulan yang menyimpang dari GDD:** GDD bagian 4 menempatkan buku Diary di shelter tua, dan tidak menyebut bab yang tersebar. Usulan ini menaruh Bab 2 di Region 1, karena jembatan menuju Region 2 membutuhkan isi bab ini, dan Bab 3 di Region 2, supaya pemain menemukannya setelah menyeberang. Keputusannya ada di bagian 5.

### Daftar tujuan

| `order` | `objectiveId` | Judul HUD | Terlihat sejak | Syarat selesai |
|---|---|---|---|---|
| 1 | `obj_base` | Bangun base awal | Awal | `BuildCount`: `objective.baseWallCount` struktur `Wall_` dan 1 `Door_` |
| 2 | `obj_wood` | `Kumpulkan kayu n/<region.bridgeWoodCost>`, dengan angka dibaca dari `GameBalanceConfig` saat HUD diperbarui | Bab 2 | `ItemTotal`: kayu di inventori + setoran `BridgeSite` >= `region.bridgeWoodCost` |
| 3 | `obj_bridge` | Bangun jembatan | Bab 2 | `BridgeBuilt`: `RegionUnlockedEvent` untuk `Amazon` |
| 4 | `obj_vaccine` | Buat vaksin | Bab 2 | `ItemCrafted`: `rec_vaccine` |
| 5 | `obj_region3` | Capai Region 3 | Bab 3 | `RegionEntered`: `Snow` |
| 6 | `obj_region4` | Capai Region 4 | Bab 3 | `RegionEntered`: `Logistic` |
| 7 | `obj_parts` | Angkut suku cadang `n`/`total` | Bab 3 | `PartsInstalled`: `HelicopterPartInstalledEvent.installed` >= `total` |
| 8 | `obj_takeoff` | Lepas landas | Bab 3 | `Escaped`: `RunEndedEvent` dengan `RunResult.Won` |

Kalau tidak ada tujuan terlihat yang belum selesai, HUD menampilkan tujuan cadangan `Cari bab Diary berikutnya`, dengan penanda ke bab terkecil yang belum ditemukan.

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.objective.baseWallCount` | 4 | Dinding untuk `obj_base` |
| `GameBalanceConfig.objective.hudFlashSeconds` | 3 | Lama sorotan tujuan baru di HUD |

## 2. Detail Logika & Fungsi

**`void OnEnable()`** berlangganan event di bagian 3, lalu memanggil `RefreshAll()`.

**`public void FoundChapter(DiaryEntry entry)`**, dipanggil `DiaryBook.OnInteract`:

1. Kalau bab belum tercatat: tambah ke `foundChapters`, terbitkan `DiaryChapterFoundEvent { chapter }`, lalu tampilkan notifikasi `Bab baru ditemukan`.
2. `GrantRewards(entry)`.
3. `DiaryManager.Instance.OpenDiary(entry)`, yang memanggil `UIFocusStack.Push(diaryPanel)`, sehingga game dijeda.
4. `RefreshAll()`.

**`private void GrantRewards(DiaryEntry entry)`**: keluar kalau `grantedRewards` sudah memuat bab ini. Untuk tiap hadiah:

- `GiveItem`: `InventoryManager.AddItem(item, amount)`. Kalau inventori penuh, sisanya menjadi `WorldItem` di dekat buku (#66).
- **Blueprint Jembatan:** bukan entri `rewards`. `RegionGateSystem` mendengar `DiaryChapterFoundEvent` dengan `chapter == 2`, lalu mengisi `hasBridgeBlueprint = true` dan menyimpannya di `world_gates` (#68). Jadi Blueprint tidak memakai slot, dan tidak bisa hilang, dibuang, atau hancur. Semua pembaca memakai properti `RegionGateSystem.HasBridgeBlueprint` (#116).
- `RevealObjectives`: cukup dengan `foundChapters`, karena `IsVisible` membacanya.

**`private bool IsVisible(ObjectiveData o)`** = `o.revealedByChapter == 0 || foundChapters.Contains(o.revealedByChapter)`.

**`private bool Evaluate(ObjectiveData o)`** memeriksa kondisi saat ini:

| Kondisi | Pemeriksaan |
|---|---|
| `BuildCount` | Hitung `StructureRegistry` yang `buildableId`-nya berawalan `Wall_` dan `Door_` |
| `ItemTotal` | `InventoryManager.GetCount(targetId) + (region.bridgeWoodCost - RegionGateSystem.WoodRemaining) >= targetAmount`, atau jembatan sudah selesai |
| `BridgeBuilt` | `RegionGateSystem.IsUnlocked(Amazon)` |
| `ItemCrafted`, `RegionEntered`, `Escaped` | Hanya berubah lewat event. Tercatat di `completed` saat event tiba |
| `PartsInstalled` | Membaca `HelicopterPartInstalledEvent.Last` |

**`private void Complete(ObjectiveData o)`**: keluar kalau sudah ada di `completed`. Tambahkan, terbitkan `ObjectiveChangedEvent { objectiveId, isComplete = true }`, lalu tampilkan notifikasi `Tujuan selesai` hanya kalau `IsVisible(o)`.

**`private void RefreshAll()`**:

1. **Semua** tujuan yang belum selesai diperiksa `Evaluate`, baik terlihat, aktif, maupun belum. Inilah aturan langkah tidak berurutan.
2. `ActiveObjective` = tujuan pertama menurut `order` yang terlihat dan belum selesai.
3. Kalau berubah, terbitkan `ActiveObjectiveChangedEvent { objectiveId, current, target }`, lalu `RefreshMarker()`.

**Contoh langkah tidak berurutan:**

- Pemain memasuki Region 4 sebelum Bab 3 ditemukan: `obj_region4` selesai tanpa notifikasi. Saat Bab 3 terbaca, tujuan itu langsung terlewati.
- Pemain membuat vaksin sebelum base: `obj_vaccine` selesai, dan `obj_base` tetap aktif.
- Dinding base hancur setelah `obj_base` selesai: tujuan tetap selesai. Tujuan tidak pernah kembali terbuka.

### Penanda arah

**`private void RefreshMarker()`** menerbitkan `ObjectiveMarkerChangedEvent { hasMarker, position, label }`, yang didengar `CompassSystem` (#30):

| Keadaan | Penanda |
|---|---|
| Tujuan cadangan aktif | `markerPosition` bab terkecil yang belum ditemukan |
| `obj_parts`, tidak membawa barang | Suku cadang terdekat lewat `HeavyItemSystem.TryGetNearestPart(pos, out part)` (#69) |
| `obj_parts`, membawa barang (`HeavyItemChangedEvent.isCarried`) | Helipad, dari `markerPosition` |
| `obj_base`, `obj_wood` | Tanpa penanda, kecuali Bab 1 belum ditemukan, maka ke shelter tua |
| Lainnya | `markerPosition` tujuan |

### HUD

`HUDManager` hanya mendengar event. Ia tidak mengubah status tujuan.

- Mendengar `ActiveObjectiveChangedEvent`: tampilkan `title` dan `current/target` di pojok kanan atas. Sorot selama `hudFlashSeconds` ketika berubah.
- Mendengar `DayChangedEvent`: sampai hari ke-60, tampilkan `Sisa hari: GameManager.TargetDay - newDay + 1`. Hari ke-60 tampil sebagai `Sisa hari: 1`. Setelah itu (`GameManager.IsPastTargetDay`), tampilkan `Target hari ke-60 terlewat`, tanpa angka negatif.
- Progres kayu diperbarui dari `InventoryChangedEvent` (`itemId == Mat_Wood`) lewat `RefreshProgress()`, dan paling sering 2 kali per detik.

### Baca ulang

Tombol `OpenDiary` (`J`) membuka `DiaryArchivePanel` lewat `UIFocusStack.Push`. Isinya daftar `foundChapters` dan daftar tujuan selesai. Memilih bab memanggil `DiaryManager.OpenDiary(entry)`. Bab yang belum ditemukan tidak tampil. Membaca ulang tidak memberi hadiah lagi.

`DiaryArchivePanel` juga mendapat tab `Arsip` berisi `LoreEntry` dari `LoreCollectibles` (#100), dikelompokkan per `RegionType`. Memilih baris membuka panel `LoreReader` dan tidak memberi hadiah.

`DiaryManager.OpenDiary` membaca teks bab lewat `LocalizationManager.Get(titleKey)` dan `LocalizationManager.Get(bodyKey)` (#104).

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `DiaryChapterFoundEvent`, `ObjectiveChangedEvent`, `ActiveObjectiveChangedEvent`, dan `ObjectiveMarkerChangedEvent`.

**Event yang didengar:** `StructureBuiltEvent`, `InventoryChangedEvent`, `RegionUnlockedEvent`, `ItemCraftedEvent`, `RegionChangedEvent`, `HelicopterPartInstalledEvent`, `HelicopterReadyEvent`, `HeavyItemChangedEvent`, dan `RunEndedEvent`.

| Sistem | Hubungan |
|---|---|
| `DiaryManager` | `OpenDiary(DiaryEntry)` menggantikan versi teks, dan memakai `UIFocusStack` |
| `IslandInteractable` (#17) | `DiaryBook` memanggil `FoundChapter` |
| `RegionGateSystem` (#68) | Mendengar `DiaryChapterFoundEvent` untuk Blueprint Jembatan dan gerbang `S2`. Sistem ini membaca `WoodRemaining` dan `IsUnlocked` |
| `CraftingManager` (#77) | `rec_vaccine` lewat `ItemCraftedEvent` |
| `HeavyItemSystem` (#69), `RescueEventManager` (#14) | Suku cadang, helikopter, dan lepas landas |
| `CompassSystem` (#30), `NotificationManager` (#22) | Penanda arah dan pesan |
| `ProgressionSafety` (#116) | Membaca `foundChapters`, dan melindungi buku Diary |
| `SleepSystem` (#67) | Teks peringatan pantai Bab 1 |
| `WorldStateSave` (#80) | `world_objectives`, `LoadOrder = 110`. Menyimpan bab Diary yang ditemukan dan objektif |
| `LoreCollectibles` (#100) | Tab `Arsip` pada `DiaryArchivePanel` |
| `LocalizationManager` (#104) | Teks `DiaryEntry` |

**Batas pekerjaan:** pop-up panduan kontrol di #16, catatan lore tambahan di #100, dan syarat akses region di #68.

## 4. Kasus Khusus

- **Pool:** sistem ini tidak memakai objek pool. Hadiah `WorldItem` diatur #66.
- **Game dijeda:** panel Diary dan arsip menjeda game lewat `UIFocusStack`. Event yang masuk tetap diproses, tetapi selama jeda hampir tidak ada event gameplay.
- **Save:** `CaptureState` menulis `foundChapters`, `grantedRewards`, dan `completed` sebagai array. `ActiveObjective` tidak disimpan, karena dihitung ulang. `RestoreState` tidak mengirim event. `OnRestoreFinished` memanggil `RefreshAll()`, lalu mengirim ulang event sticky.
- **Bangunan dimuat setelah manager:** `RefreshAll` di `OnRestoreFinished` berjalan setelah `StructureRegistry` selesai (#80).
- **Permadeath:** semua status terhapus bersama save run (#82). Scene baru memuat manager kosong. Hadiah dan bab mulai dari nol, sesuai GDD.
- **Buku Diary:** tidak punya `BaseHealth`, jadi tidak bisa rusak oleh bencana. Pengaman lain ada di #116. Buku tetap bisa dibaca di tempatnya.
- **Region tidak dimuat:** `markerPosition` adalah data, jadi penanda tetap bekerja.
- **Tertangkap pada frame kemenangan:** `RunEndedEvent` hanya mencatat `obj_takeoff`, dan tidak menampilkan notifikasi, karena alur akhir run sudah berjalan.
- **Kalah:** `RunEndedEvent` selain `Won` diabaikan.
- **`GetCount` belum ada di `InventoryManager.md`:** blueprint itu baru punya `HasItem`. `ThrowableSystem.md` juga memakai `GetCount`, jadi fungsi ini perlu ditambahkan di sana.

## 5. Keputusan Desain

**Sudah ditetapkan GDD:** isi tiga bab, buku Diary di shelter tua, dan Blueprint Jembatan dari Bab 2.

**Usulan yang menunggu developer (dicatat di #85):**

1. **Tempat Bab 2 dan 3 (menyimpang dari GDD bagian 4):** rekomendasi Bab 2 di pos kemah penulis di Region 1, dan Bab 3 di Region 2. Developer perlu memilih di #85: (a) GDD diubah agar bab tersebar, atau (b) satu buku di shelter, dengan bab terbuka bertahap oleh pemicu lain. Kalau memilih (b), tabel bab dan gerbang `S2` di `RegionGateSystem.md` perlu disesuaikan.
2. **Hadiah api unggun Bab 1:** rekomendasi memberi satu `Kit_Campfire` dan tetap membiarkan `rec_campfire` terbuka sejak awal, sesuai `CraftingStation.md`. Pilihan lain adalah mengunci resep sampai Bab 1, tetapi pemain bisa kesulitan memasak sebelum menemukan shelter.
3. **Bentuk Blueprint Jembatan:** rekomendasi berupa penanda `hasBridgeBlueprint` di `RegionGateSystem` (#68), bukan item, agar tidak bisa hilang dan tidak memakai slot.
4. **Tujuan tersembunyi sebelum babnya ditemukan:** rekomendasi disembunyikan, supaya Region 3 dan suku cadang tetap menjadi kejutan Bab 3.
5. **Syarat `obj_base`:** rekomendasi 4 dinding dan 1 pintu, sesuai contoh base awal di `GameBalanceConfig.md`.

## 6. Skenario Tes

1. Mulai run. **Hasil:** HUD menampilkan `Bangun base awal`, `Sisa hari: 60`, dan penanda ke shelter tua.
2. Tekan `E` pada buku Bab 1. **Hasil:** panel terbuka, game terjeda, `Kit_Campfire` masuk inventori, dan `DiaryChapterFoundEvent` terkirim.
3. Baca lagi lewat `J`. **Hasil:** isi sama, dan `Kit_Campfire` tidak bertambah.
4. Bangun 4 dinding dan 1 pintu. **Hasil:** `obj_base` selesai, lalu HUD menampilkan `Cari bab Diary berikutnya`.
5. Temukan Bab 2. **Hasil:** `RegionGateSystem.HasBridgeBlueprint` bernilai `true`, dan HUD menampilkan progres kayu.
6. Setor 200 kayu, dan pegang 300 kayu. **Hasil:** `obj_wood` selesai, dan `obj_bridge` aktif.
7. Lewat `DevConsole`, pindah ke Region 4 sebelum Bab 3. **Hasil:** tidak ada notifikasi. Saat Bab 3 ditemukan, `obj_region4` sudah terlewati.
8. Hancurkan satu dinding base. **Hasil:** `obj_base` tetap selesai.
9. Saat `obj_parts` aktif, angkat suku cadang. **Hasil:** penanda pindah ke helipad. Jatuhkan barang, dan penanda kembali ke barang terdekat.
10. Pasang semua suku cadang. **Hasil:** tujuan menjadi `Lepas landas`.
11. Isi inventori sampai penuh, lalu temukan Bab 1. **Hasil:** `Kit_Campfire` muncul sebagai `WorldItem` di dekat buku.
12. Simpan setelah Bab 2, keluar, lalu lanjutkan. **Hasil:** bab, tujuan, dan penanda sama, tanpa notifikasi ulang.
13. Mati, lalu mulai run baru. **Hasil:** arsip kosong, dan semua tujuan mulai dari awal.
14. `setday 60`, lalu `setday 61`. **Hasil:** HUD berubah dari `Sisa hari: 1` menjadi `Target hari ke-60 terlewat`, dan tujuan aktif tidak berubah.
15. Temukan satu lore, lalu buka tab `Arsip`. **Hasil:** barisnya tampil di region yang benar, membuka `LoreReader`, dan tidak memberi hadiah.
16. Ganti bahasa, lalu baca Bab 1. **Hasil:** judul dan isi mengikuti bahasa baru.
