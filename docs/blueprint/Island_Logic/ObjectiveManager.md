# Blueprint: ObjectiveManager.cs (DiaryEntry)
**Lokasi:** `docs/blueprint/Island_Logic/ObjectiveManager.md`
**Deskripsi Utama:** Satu buku Diary dengan tiga bab yang terbuka bertahap, hadiah tiap bab, serta daftar tujuan bertahap yang tampil di HUD dan tetap maju walaupun pemain mengerjakannya tidak berurutan.
**Issue:** #81
**Rujukan GDD:** Bagian 4 (satu buku Diary di shelter tua, Bab 1 sampai 3 yang terbuka bertahap, dan Blueprint Jembatan), bagian 1 dan 3 (syarat menang), serta bagian 5 (urutan region dan logistik). Pemicu Bab 2 dan 3 adalah keputusan developer, dengan nilai awal yang bisa disetel. Hadiah Bab 1 dan tampilan HUD adalah usulan.

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
| `bookFound` | `bool` | `false` | Buku Diary sudah ditemukan di shelter tua |
| `chapter3RegionReached` | `bool` | `false` | Pemain pernah masuk `chapter3TriggerRegion` |
| `unlockedChapters` | `List<int>` | Kosong | Bab yang sudah terbuka |
| `grantedRewards` | `List<int>` | Kosong | Bab yang hadiahnya sudah diberikan |
| `bookMarkerPosition`, `bridgeMarkerPosition` | `Vector3` | Dari Inspector | Penanda shelter tua dan `BridgeSite`. Berupa data, karena region bisa tidak dimuat |
| `completed` | `HashSet<string>` | Kosong | ID tujuan selesai |
| `ActiveObjective` | `ObjectiveData` | `null` | Tujuan yang tampil di HUD |

### Bab Diary

Semua bab berada dalam satu buku `DiaryBook` di shelter tua yang hancur, sesuai GDD bagian 4. Penempatan shelter di Region 1 adalah usulan, karena GDD tidak menyebut regionnya.

| Bab | Terbuka saat | Hadiah | Isi sesuai GDD |
|---|---|---|---|
| 1 | Buku ditemukan | `Kit_Campfire` x1 | Jangan tidur di pantai, zombie terbakar matahari, cara membuat api unggun, memancing dengan kail di tebing karang |
| 2 | Buku sudah ditemukan, lalu `obj_base` selesai atau hari mencapai `chapter2TriggerDay`, mana yang lebih dulu | Blueprint Jembatan (`RegionGateSystem.HasBridgeBlueprint`), dan tujuan kelompok 2 | Penulis terinfeksi, vaksin dari getah dan jamur Region 2, Blueprint Jembatan |
| 3 | Buku sudah ditemukan, lalu pemain masuk `chapter3TriggerRegion` | Petunjuk helipad, dan tujuan kelompok 3 | Helipad dan landasan di puncak es, suku cadang tertinggal di desa |

**Kenapa pemicu ini tidak bisa membuat softlock:**

- **Buku:** tidak bisa rusak, tidak bisa tertimbun, dan selalu punya penanda arah sampai ditemukan (#116).
- **Bab 2:** hari selalu berjalan, jadi Blueprint Jembatan pasti didapat walau pemain tidak membangun apa pun. Jembatan butuh Blueprint, jadi Bab 2 selalu terbuka sebelum pemain bisa ke Region 2.
- **Bab 3:** terbuka otomatis saat pemain masuk Region 2, tanpa benda lain yang harus dicari. Gerbang `S2` lalu terbuka (#68).
- **Hadiah:** diberikan saat bab terbuka, bukan saat dibaca. Pemain yang belum membuka menu tetap mendapat Blueprint.

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

Kalau tidak ada tujuan terlihat yang belum selesai, HUD menampilkan tujuan cadangan: `Cari buku Diary di shelter tua` kalau `!bookFound`, atau `Seberangi jembatan ke Region 2` kalau Bab 3 belum terbuka. Bab 2 tidak butuh tujuan cadangan, karena `obj_base` yang selesai langsung membukanya.

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.objective.baseWallCount` | 4 | Dinding untuk `obj_base` |
| `GameBalanceConfig.objective.hudFlashSeconds` | 3 | Lama sorotan tujuan baru di HUD |
| `GameBalanceConfig.objective.chapter2TriggerDay` | 2 | Hari paling lambat Bab 2 terbuka, setelah buku ditemukan |
| `GameBalanceConfig.objective.chapter3TriggerRegion` | `Amazon` | Region yang membuka Bab 3. Jangan diisi region di balik `S2`, karena gerbang itu membutuhkan Bab 3 |

## 2. Detail Logika & Fungsi

**`void OnEnable()`** berlangganan event di bagian 3, lalu memanggil `RefreshAll()`.

**`public void InteractBook()`**, dipanggil `DiaryBook.OnInteract`:

1. Kalau `!bookFound`: `bookFound = true`, `RefreshAll()`, lalu `DiaryManager.Instance.OpenDiary(chapters[0])`, yang memanggil `UIFocusStack.Push(diaryPanel)`, sehingga game dijeda.
2. Kalau tidak: `DiaryManager.Instance.OpenArchive()`.

**`private void EvaluateChapterTriggers()`**, dipanggil dari `RefreshAll`. Keluar kalau `!bookFound`, lalu periksa berurutan:

| Bab | Syarat |
|---|---|
| 1 | Selalu, karena buku sudah ditemukan |
| 2 | `GameManager.currentDay >= chapter2TriggerDay`, `completed` memuat `obj_base`, atau `chapter3RegionReached` |
| 3 | `chapter3RegionReached` |

**`private void UnlockChapter(int chapter)`**, untuk tiap bab yang syaratnya terpenuhi. Keluar kalau bab sudah tercatat:

1. Tambah ke `unlockedChapters`.
2. `GrantRewards(chapters[chapter - 1])`.
3. Terbitkan `DiaryChapterUnlockedEvent { chapter }`. `NotificationManager` menampilkan `Halaman baru di Diary`.

**`HandleRegion(RegionChangedEvent e)`**: kalau `e.to == chapter3TriggerRegion`, `chapter3RegionReached = true`. Lalu `RefreshAll()`. `DayChangedEvent` juga memanggil `RefreshAll()`.

**`private void GrantRewards(DiaryEntry entry)`**: keluar kalau `grantedRewards` sudah memuat bab ini. Untuk tiap hadiah:

- `GiveItem`: `InventoryManager.AddItem(item, amount)`. Kalau inventori penuh, sisanya menjadi `WorldItem` di dekat pemain (#66).
- **Blueprint Jembatan:** bukan entri `rewards`. `RegionGateSystem` mendengar `DiaryChapterUnlockedEvent` dengan `chapter == 2`, lalu mengisi `hasBridgeBlueprint = true` dan menyimpannya di `world_gates` (#68). Jadi Blueprint tidak memakai slot, dan tidak bisa hilang, dibuang, atau hancur. Semua pembaca memakai properti `RegionGateSystem.HasBridgeBlueprint` (#116).
- `RevealObjectives`: cukup dengan `unlockedChapters`, karena `IsVisible` membacanya.

**`private bool IsVisible(ObjectiveData o)`** = `o.revealedByChapter == 0 || unlockedChapters.Contains(o.revealedByChapter)`.

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
2. `EvaluateChapterTriggers()`.
3. `ActiveObjective` = tujuan pertama menurut `order` yang terlihat dan belum selesai.
4. Kalau berubah, terbitkan `ActiveObjectiveChangedEvent { objectiveId, current, target }`, lalu `RefreshMarker()`.

**Contoh langkah tidak berurutan:**

- Pemain memasuki Region 4 sebelum Bab 3 terbuka, misalnya lewat `DevConsole`: `obj_region4` selesai tanpa notifikasi. Saat Bab 3 terbuka, tujuan itu langsung terlewati.
- Pemain membuat vaksin sebelum base: `obj_vaccine` selesai, dan `obj_base` tetap aktif.
- Dinding base hancur setelah `obj_base` selesai: tujuan tetap selesai. Tujuan tidak pernah kembali terbuka.

### Penanda arah

**`private void RefreshMarker()`** menerbitkan `ObjectiveMarkerChangedEvent { hasMarker, position, label }`, yang didengar `CompassSystem` (#30):

| Keadaan | Penanda |
|---|---|
| Tujuan cadangan aktif | `bookMarkerPosition` kalau `!bookFound`, atau `bridgeMarkerPosition` kalau Bab 3 belum terbuka |
| `obj_parts`, tidak membawa barang | Suku cadang terdekat lewat `HeavyItemSystem.TryGetNearestPart(pos, out part)` (#69) |
| `obj_parts`, membawa barang (`HeavyItemChangedEvent.isCarried`) | Helipad, dari `markerPosition` |
| `obj_base`, `obj_wood` | Tanpa penanda, kecuali buku belum ditemukan, maka ke shelter tua |
| Lainnya | `markerPosition` tujuan |

### HUD

`HUDManager` hanya mendengar event. Ia tidak mengubah status tujuan.

- Mendengar `ActiveObjectiveChangedEvent`: tampilkan `title` dan `current/target` di pojok kanan atas. Sorot selama `hudFlashSeconds` ketika berubah.
- Mendengar `DayChangedEvent`: sampai hari ke-60, tampilkan `Sisa hari: GameManager.TargetDay - newDay + 1`. Hari ke-60 tampil sebagai `Sisa hari: 1`. Setelah itu (`GameManager.IsPastTargetDay`), tampilkan `Target hari ke-60 terlewat`, tanpa angka negatif.
- Progres kayu diperbarui dari `InventoryChangedEvent` (`itemId == Mat_Wood`) lewat `RefreshProgress()`, dan paling sering 2 kali per detik.

### Baca ulang

Tombol `OpenDiary` (`J`) membuka `DiaryArchivePanel` lewat `UIFocusStack.Push`. Isinya daftar `unlockedChapters` dan daftar tujuan selesai, jadi pemain tidak perlu kembali ke shelter untuk membaca bab baru. Memilih bab memanggil `DiaryManager.OpenDiary(entry)`. Bab yang belum terbuka tidak tampil, dan sebelum buku ditemukan daftar bab kosong. Membaca tidak memberi hadiah, karena hadiah diberikan saat bab terbuka.

`DiaryArchivePanel` juga mendapat tab `Arsip` berisi `LoreEntry` dari `LoreCollectibles` (#100), dikelompokkan per `RegionType`. Memilih baris membuka panel `LoreReader` dan tidak memberi hadiah.

`DiaryManager.OpenDiary` membaca teks bab lewat `LocalizationManager.Get(titleKey)` dan `LocalizationManager.Get(bodyKey)` (#104).

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `DiaryChapterUnlockedEvent`, `ObjectiveChangedEvent`, `ActiveObjectiveChangedEvent`, dan `ObjectiveMarkerChangedEvent`.

**Event yang didengar:** `DayChangedEvent`, `StructureBuiltEvent`, `InventoryChangedEvent`, `RegionUnlockedEvent`, `ItemCraftedEvent`, `RegionChangedEvent`, `HelicopterPartInstalledEvent`, `HelicopterReadyEvent`, `HeavyItemChangedEvent`, dan `RunEndedEvent`.

| Sistem | Hubungan |
|---|---|
| `DiaryManager` | `OpenDiary(DiaryEntry)` menggantikan versi teks, `OpenArchive()` untuk buku yang sudah ditemukan, dan keduanya memakai `UIFocusStack` |
| `IslandInteractable` (#17) | `DiaryBook` memanggil `InteractBook` |
| `RegionGateSystem` (#68) | Mendengar `DiaryChapterUnlockedEvent` untuk Blueprint Jembatan dan gerbang `S2`. Sistem ini membaca `WoodRemaining` dan `IsUnlocked` |
| `CraftingManager` (#77) | `rec_vaccine` lewat `ItemCraftedEvent` |
| `HeavyItemSystem` (#69), `RescueEventManager` (#14) | Suku cadang, helikopter, dan lepas landas |
| `CompassSystem` (#30), `NotificationManager` (#22) | Penanda arah dan pesan |
| `ProgressionSafety` (#116) | Membaca `unlockedChapters`, dan melindungi buku Diary |
| `SleepSystem` (#67) | Teks peringatan pantai Bab 1 |
| `WorldStateSave` (#80) | `world_objectives`, `LoadOrder = 110`. Menyimpan status buku, bab Diary yang terbuka, dan objektif |
| `LoreCollectibles` (#100) | Tab `Arsip` pada `DiaryArchivePanel` |
| `LocalizationManager` (#104) | Teks `DiaryEntry` |

**Batas pekerjaan:** pop-up panduan kontrol di #16, catatan lore tambahan di #100, dan syarat akses region di #68.

## 4. Kasus Khusus

- **Pool:** sistem ini tidak memakai objek pool. Hadiah `WorldItem` diatur #66.
- **Game dijeda:** panel Diary dan arsip menjeda game lewat `UIFocusStack`. Event yang masuk tetap diproses, tetapi selama jeda hampir tidak ada event gameplay.
- **Save:** `CaptureState` menulis `bookFound`, `chapter3RegionReached`, `unlockedChapters`, `grantedRewards`, dan `completed`, dengan daftar sebagai array. `ActiveObjective` tidak disimpan, karena dihitung ulang. `RestoreState` tidak mengirim event. `OnRestoreFinished` memanggil `RefreshAll()`, lalu mengirim ulang event sticky.
- **Bangunan dimuat setelah manager:** `RefreshAll` di `OnRestoreFinished` berjalan setelah `StructureRegistry` selesai (#80).
- **Permadeath:** semua status terhapus bersama save run (#82). Scene baru memuat manager kosong. Hadiah dan bab mulai dari nol, sesuai GDD.
- **Buku Diary:** tidak punya `BaseHealth`, jadi tidak bisa rusak oleh bencana. Pengaman lain ada di #116. Buku tetap bisa dibaca di tempatnya.
- **Region tidak dimuat:** posisi penanda adalah data, jadi penanda tetap bekerja.
- **Syarat bab terpenuhi sebelum buku ditemukan:** misalnya base selesai pada hari ke-5. Bab 1 dan 2 terbuka bersamaan saat buku ditemukan, panel menampilkan Bab 1, dan notifikasi Bab 2 menunggu panel ditutup (#22).
- **Tertangkap pada frame kemenangan:** `RunEndedEvent` hanya mencatat `obj_takeoff`, dan tidak menampilkan notifikasi, karena alur akhir run sudah berjalan.
- **Kalah:** `RunEndedEvent` selain `Won` diabaikan.
- **`GetCount` belum ada di `InventoryManager.md`:** blueprint itu baru punya `HasItem`. `ThrowableSystem.md` juga memakai `GetCount`, jadi fungsi ini perlu ditambahkan di sana.

## 5. Keputusan Desain

**Sudah ditetapkan GDD:** isi tiga bab, satu buku Diary di shelter tua, bab yang terbuka bertahap mengikuti progres, dan Blueprint Jembatan dari Bab 2.

**Keputusan developer:** Bab tidak tersebar. Bab 1 terbuka saat buku ditemukan. Bab 2 dan 3 terbuka bertahap, bisa dibaca dari menu setelah buku ditemukan, dan ada notifikasi saat ada halaman baru. Pemicunya tidak boleh membuat softlock. Blueprint Jembatan harus bisa didapat sebelum pemain ke Region 2, dan Bab 3 terbuka setelah pemain masuk Region 2. Nilai awalnya `chapter2TriggerDay` 2 dan `chapter3TriggerRegion` `Amazon`, dan keduanya bisa disetel.

**Usulan yang disetujui developer sebagai nilai awal (dicatat di #85):**

1. **Hadiah api unggun Bab 1:** rekomendasi memberi satu `Kit_Campfire` dan tetap membiarkan `rec_campfire` terbuka sejak awal, sesuai `CraftingStation.md`. Pilihan lain adalah mengunci resep sampai Bab 1, tetapi pemain bisa kesulitan memasak sebelum menemukan shelter.
2. **Bentuk Blueprint Jembatan:** rekomendasi berupa penanda `hasBridgeBlueprint` di `RegionGateSystem` (#68), bukan item, agar tidak bisa hilang dan tidak memakai slot.
3. **Tujuan tersembunyi sebelum babnya terbuka:** rekomendasi disembunyikan, supaya Region 3 dan suku cadang tetap menjadi kejutan Bab 3.
4. **Syarat `obj_base`:** rekomendasi 4 dinding dan 1 pintu, sesuai contoh base awal di `GameBalanceConfig.md`.

## 6. Skenario Tes

1. Mulai run. **Hasil:** HUD menampilkan `Bangun base awal`, `Sisa hari: 60`, dan penanda ke shelter tua.
2. Tekan `E` pada buku Diary pada hari ke-1, tanpa base. **Hasil:** panel Bab 1 terbuka, game terjeda, `Kit_Campfire` masuk inventori, `DiaryChapterUnlockedEvent` terkirim sekali dengan `chapter = 1`, dan Bab 2 belum terbuka.
3. Baca lagi lewat `J`. **Hasil:** isi sama, dan `Kit_Campfire` tidak bertambah.
4. Bangun 4 dinding dan 1 pintu pada hari ke-1. **Hasil:** `obj_base` selesai, Bab 2 terbuka, notifikasi `Halaman baru di Diary` tampil, `RegionGateSystem.HasBridgeBlueprint` bernilai `true` tanpa membuka menu, dan HUD menampilkan progres kayu.
5. Pada run baru, temukan buku, jangan membangun, lalu tunggu hari ke-2. **Hasil:** Bab 2 terbuka, dan `HasBridgeBlueprint` bernilai `true`.
6. Setor 200 kayu, dan pegang 300 kayu. **Hasil:** `obj_wood` selesai, dan `obj_bridge` aktif.
7. Lewat `DevConsole`, pakai `unlockregion Logistic` dan `teleport` ke Region 4 sebelum masuk Region 2. **Hasil:** tidak ada notifikasi. Saat Bab 3 terbuka, `obj_region4` sudah terlewati.
8. Hancurkan satu dinding base. **Hasil:** `obj_base` tetap selesai.
9. Saat `obj_parts` aktif, angkat suku cadang. **Hasil:** penanda pindah ke helipad. Jatuhkan barang, dan penanda kembali ke barang terdekat.
10. Pasang semua suku cadang. **Hasil:** tujuan menjadi `Lepas landas`.
11. Isi inventori sampai penuh, lalu temukan buku Diary. **Hasil:** `Kit_Campfire` muncul sebagai `WorldItem` di dekat pemain.
12. Simpan setelah Bab 2, keluar, lalu lanjutkan. **Hasil:** bab, tujuan, dan penanda sama, tanpa notifikasi ulang.
13. Mati, lalu mulai run baru. **Hasil:** arsip kosong, dan semua tujuan mulai dari awal.
14. `setday 60`, lalu `setday 61`. **Hasil:** HUD berubah dari `Sisa hari: 1` menjadi `Target hari ke-60 terlewat`, dan tujuan aktif tidak berubah.
15. Temukan satu lore, lalu buka tab `Arsip`. **Hasil:** barisnya tampil di region yang benar, membuka `LoreReader`, dan tidak memberi hadiah.
16. Ganti bahasa, lalu baca Bab 1. **Hasil:** judul dan isi mengikuti bahasa baru.
17. Selesaikan jembatan, lalu masuk Region 2. **Hasil:** Bab 3 terbuka sekali, notifikasi tampil, `S2` terbuka, dan tujuan kelompok 3 terlihat.
18. Bangun base dan tunggu hari ke-5 tanpa menemukan buku. **Hasil:** tidak ada bab, dan HUD menampilkan `Cari buku Diary di shelter tua`. Temukan buku. **Hasil:** Bab 1 dan 2 terbuka bersamaan, dan Blueprint didapat.
19. `balance objective.chapter2TriggerDay 5`, temukan buku tanpa membangun, lalu tunggu hari ke-2. **Hasil:** Bab 2 belum terbuka. Tunggu hari ke-5. **Hasil:** Bab 2 terbuka.
20. Selesaikan jembatan tanpa masuk Region 2. **Hasil:** HUD menampilkan `Seberangi jembatan ke Region 2`, kalau tujuan lain sudah selesai.
