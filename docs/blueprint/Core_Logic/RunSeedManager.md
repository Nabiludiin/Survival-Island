# Blueprint: RunSeedManager.cs
**Lokasi:** `docs/blueprint/Core_Logic/RunSeedManager.md`
**Deskripsi Utama:** Pemilik seed utama tiap run, aliran acak per sistem, dan susunan run yang sudah dijamin bisa dimenangkan.
**Issue:** #89
**Rujukan GDD:** Bagian 1 (genre Roguelike dan Permadeath), bagian 3 (Fase 1), bagian 5 (suku cadang tersebar di desa), dan bagian 6 (bencana acak). Daftar yang berubah tiap run adalah usulan, karena GDD tidak merincinya.

## 1. Arsitektur & Variabel Utama

### Pola

`RunSeedManager` adalah manager persisten (`IBootstrapService`, urutan ke-11 di #82). Sistem gameplay tidak boleh memakai `UnityEngine.Random`, kecuali untuk efek yang tidak memengaruhi permainan.

| Jenis acak | Sumber | Contoh |
|---|---|---|
| Memengaruhi permainan | `RunSeedManager` | Loot, suku cadang, cuaca, bencana, drop, dan hasil mesin |
| Hanya tampilan | `UnityEngine.Random` | Getaran UI, partikel, dan pitch suara |

### Aliran acak

Satu aliran bersama membuat hasil saling bergantung: kalau satu sistem menambah satu panggilan acak, hasil semua sistem lain ikut berubah. Karena itu, tiap sistem punya aliran sendiri.

```csharp
public enum RandomStreamId
{
    ShipLoot, IslandLoot, PartLocation, Weather, Disaster, NPC,
    BeachSupply, Enemy, Gameplay, LuckyMachine
}

public struct RandomStream
{
    private ulong state;
    public RandomStream(ulong seed) { state = seed; }
    public ulong State => state;

    public ulong NextULong()            // SplitMix64
    {
        ulong z = state += 0x9E3779B97F4A7C15UL;
        z = (z ^ (z >> 30)) * 0xBF58476D1CE4E5B9UL;
        z = (z ^ (z >> 27)) * 0x94D049BB133111EBUL;
        return z ^ (z >> 31);
    }

    public float NextFloat01();                    // 0 sampai kurang dari 1
    public int Range(int minInclusive, int maxExclusive);
    public float Range(float min, float max);
    public int PickWeighted(float[] weights);      // Indeks terpilih, -1 kalau total bobot 0
    public void Shuffle(int[] indices);            // Fisher-Yates
}
```

Kelas bawaan `System.Random` tidak dipakai, karena statusnya tidak bisa disimpan ke file save.

**Ada dua cara memakai aliran:**

| Cara | Fungsi | Kapan dipakai | Yang disimpan |
|---|---|---|---|
| **Berkunci** | `Fork(id, key)` | Hasil harus sama, tidak bergantung pada urutan kunjungan atau jumlah panggilan sebelumnya | Tidak perlu status. Cukup seed |
| **Berurutan** | `ref GetStream(id)` | Hasil makin lama makin bergantung pada tindakan pemain | `state` tiap aliran |

| Aliran | Cara | Kunci | Pemakai |
|---|---|---|---|
| `ShipLoot` | Berkunci | `spawnerId` dan indeks titik | `LootSpawnerSystem` di `ShipPhase` |
| `IslandLoot` | Berkunci | `spawnerId` dan indeks titik | `LootSpawnerSystem`, #103 |
| `PartLocation` | Berkunci | Nomor percobaan | `BuildLayout`, untuk #69 |
| `NPC` | Berkunci | Nomor percobaan | `BuildLayout`, untuk #76 |
| `Disaster` | Berkunci | Nomor percobaan | `BuildLayout`, untuk #70 |
| `Weather` | Berkunci | Region dan nomor slot waktu | `RegionalWeather` (#115) |
| `BeachSupply` | Berkunci | Nomor hari | `BeachSupplyManager` (#71) |
| `Enemy` | Berurutan | Tidak ada | `SpawnManager` (#62), `AIDirector` (#27) |
| `Gameplay` | Berurutan | Tidak ada | `ResourceNode`, `FishingSystem` (#55), dan drop lain |
| `LuckyMachine` | Berurutan | Tidak ada | `LuckyMachine` (#72) |

### Data run

```csharp
[Serializable]
public class RunLayout
{
    public int[] partPointByPart;       // Indeks suku cadang -> indeks titik di PartSpawnPointTable
    public string[] shipNpcIds;         // NPC yang muncul di kapal pada run ini
    public int[] shipNpcPointByNpc;     // NPC -> titik di kapal
    public DisasterScheduleEntry[] disasterSchedule;   // Hari, jenis, dan skala
}

[Serializable]
public struct RunSeedSaveData
{
    public int version;                 // Nilai awal 1
    public uint seed;
    public ulong[] sequentialStates;    // Urutan mengikuti RandomStreamId
    public RunLayout layout;
    public bool usedFallbackLayout;
}
```

`RunLayoutSources` adalah ScriptableObject berisi sumber data: `PartSpawnPointTable`, daftar NPC, titik NPC di kapal, `DisasterData[]`, dan `RunLayout safeLayout`.

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `CurrentSeed` | `uint` | 0, artinya belum ada run | Seed utama |
| `CurrentSeedText` | `string` | Kosong | Format `XXXX-XXXX`, contoh `7F3A-91C2` |
| `CurrentLayout` | `RunLayout` | `null` | Susunan run yang sudah lolos pemeriksaan |
| `sequential` | `RandomStream[]` | Kosong | Aliran berurutan |
| `forcedNextSeed` | `uint` | 0 | Seed dari `DevConsole`. Hanya di build pengembangan |

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.runSeed.maxSeedRerolls` | 8 | Batas percobaan seed baru |
| `GameBalanceConfig.runSeed.lootPointEmptyChance` | 0,5 | Peluang titik loot kosong. Menggantikan angka langsung di `LootSpawnerSystem` |
| `GameBalanceConfig.runSeed.minShipFoodItems` | 3 | Makanan minimum di kapal |
| `GameBalanceConfig.runSeed.minShipAmmoItems` | 2 | Peluru minimum di kapal |
| `GameBalanceConfig.runSeed.minShipDrinkItems` | 1 | Minuman minimum di kapal |
| `GameBalanceConfig.runSeed.minPartPointSpacing` | 30 | Jarak minimum antar-suku cadang, dalam meter |
| `GameBalanceConfig.runSeed.firstMajorDisasterDay` | 3 | Hari paling awal bencana skala besar |
| `GameBalanceConfig.runSeed.minDaysBetweenMajorDisasters` | 4 | Jarak minimum antar-bencana skala besar |

## 2. Detail Logika & Fungsi

### Membuat seed

**`public void BeginNewRun()`**, dipanggil `RunLifecycle.StartNewRun()` (#82) sebelum scene dimuat:

1. `seed = forcedNextSeed != 0 ? forcedNextSeed : GenerateSeed()`.
2. Kalau seed berasal dari `forcedNextSeed`, simpan `isForced = true`, lalu kosongkan `forcedNextSeed`.
3. Bangun susunan run dengan `ResolveLayout(seed, isForced)`.
4. Isi `CurrentSeed`, `CurrentSeedText`, dan `CurrentLayout`.
5. Buat aliran berurutan: `sequential[id] = new RandomStream(DeriveSeed(seed, id, 0))`.

**`uint GenerateSeed()`** memakai `(uint)Guid.NewGuid().GetHashCode()`, dan mengulang kalau hasilnya 0. Waktu sistem tidak dipakai, karena dua run yang berdekatan bisa punya seed mirip. Seed hanya dibuat saat run baru.

`RunStartedEvent` dikirim `RunLifecycle`, sesudah fungsi ini selesai, dan seed sudah bisa dibaca semua pendengar.

### Fungsi aliran

```csharp
public RandomStream Fork(RandomStreamId id, int key);
public RandomStream Fork(RandomStreamId id, string key);
public ref RandomStream GetStream(RandomStreamId id);

ulong DeriveSeed(uint seed, RandomStreamId id, ulong keyHash)
{
    // Campur seed, id, dan kunci lewat finalizer SplitMix64
}
```

- Kunci string memakai hash FNV-1a 64-bit yang ditulis sendiri. `string.GetHashCode` tidak dipakai, karena hasilnya bisa berbeda antar-runtime.
- `Fork` mengembalikan salinan baru tanpa alokasi. Dua panggilan dengan kunci yang sama menghasilkan urutan yang sama.
- `GetStream` mengembalikan `ref`. Menyalin hasilnya ke variabel biasa membuat status tidak maju, jadi pemanggil harus memakai `ref var s = ref ...GetStream(id);`.

### Apa yang berubah dan tetap tiap run

| Yang berubah tiap run | Cara | Pemilik isi |
|---|---|---|
| Posisi dan isi loot kapal | `Fork(ShipLoot, ...)` | `LootSpawnerSystem`, #103 |
| NPC yang muncul di kapal dan posisinya | `BuildLayout`, dari daftar #76 | #76 |
| Lokasi suku cadang di Region 4 | `BuildLayout`, memilih titik buatan tangan | #69 |
| Jadwal bencana | `BuildLayout`, dari `DisasterData` | #70 |
| Cuaca per region | `Fork(Weather, ...)` | #115 |
| Isi kargo mingguan dan loot pulau | `Fork(BeachSupply, ...)`, `Fork(IslandLoot, ...)` | #71, #103 |

| Yang tetap tiap run | Alasan |
|---|---|
| Peta, posisi region, seam, dan jembatan | GDD bagian 5, dan #88 |
| Urutan dan isi Diary Bab 1, 2, dan 3 | GDD bagian 4 |
| Syarat menang: rakit helikopter dan lepas landas sebelum hari ke-60 berakhir | GDD bagian 3, `GLOSSARY.md` |
| Item awal: `Base_Kail`, `Base_Pistol`, dan `Base_Pisau` | `GLOSSARY.md` |
| Jumlah kayu jembatan dan jumlah suku cadang | `GameBalanceConfig.region` |
| Jadwal Blood Moon | Tetap di #50, di luar issue ini |
| Jadwal Weekly Beach Supply: kelipatan 7 hari | GDD bagian 6. Hanya isinya yang berubah |
| Pohon Region 1, jamur dan getah Region 2 | Syarat jembatan dan vaksin (GDD bagian 5). Posisi node tidak diacak |
| Jenis musuh tiap region | GDD bagian 5 |

### Membangun susunan run yang valid

**`RunLayout BuildLayout(uint seed)`** adalah fungsi murni. Hasilnya hanya ditentukan oleh `seed` dan `RunLayoutSources`, dan tidak membaca scene:

1. **Suku cadang:** `Fork(PartLocation, 0)`, `Shuffle` indeks titik di `PartSpawnPointTable`, lalu ambil sebanyak `GameBalanceConfig.region.helicopterPartCount` titik pertama yang memenuhi `minPartPointSpacing`.
2. **NPC:** `Fork(NPC, 0)`, pilih NPC dari daftar #76, dan pasangkan ke titik kapal yang sudah dibuat tangan.
3. **Bencana:** `Fork(Disaster, 0)`, lalu jalankan hari 1 sampai 60 satu per satu sambil menghitung cooldown dari `DisasterData` (#70). Karena jadwal dihitung sekali dan disimpan, `setday 59` tetap memberi jadwal yang sama.

**`bool RunLayoutValidator.Validate(RunLayout layout, uint seed, out string failure)`** memeriksa:

| No | Syarat | Mencegah |
|---|---|---|
| 1 | Jumlah titik suku cadang sama dengan `helicopterPartCount`, dan semua titik berbeda | Suku cadang kurang atau bertumpuk |
| 2 | Tiap titik punya `isReachable` bernilai `true`, dari pemeriksaan NavMesh di Editor, dan tidak terendam saat pasang (#91) | Barang tidak bisa diambil |
| 3 | Jarak antar-titik minimal `minPartPointSpacing` | Semua barang berkumpul di satu tempat |
| 4 | Hasil `ShipLoot` memuat `minShipFoodItems`, `minShipAmmoItems`, dan `minShipDrinkItems` | Mati kelaparan sejak awal tanpa pilihan |
| 5 | Titik NPC kapal berada di area yang bisa dicapai sebelum air naik (#75) | NPC mustahil diselamatkan |
| 6 | Bencana skala besar tidak muncul sebelum `firstMajorDisasterDay`, dan jaraknya minimal `minDaysBetweenMajorDisasters` | Base awal hancur sebelum sempat dibangun |

Syarat 4 menjalankan perhitungan `Fork(ShipLoot, ...)` lewat daftar `spawnerId` di `RunLayoutSources`, tanpa membuka `ShipPhase`.

Benda wajib lain tidak bergantung pada seed, sehingga tetap aman: node kayu, jamur, dan getah tidak diacak, dan aturan Diary, kapak, serta `KeyItem` ada di #116.

**`RunLayout ResolveLayout(ref uint seed, bool isForced)`**:

1. Bangun dan periksa susunan. Kalau lolos, selesai.
2. **Kalau seed dipaksa dari `DevConsole`:** pakai apa adanya dan catat peringatan, karena tujuannya mengulang bug.
3. Kalau tidak, `seed = DeriveNextSeed(seed)`, lalu coba lagi sampai `maxSeedRerolls`. Seed yang ditampilkan adalah seed yang lolos, jadi mengulangnya langsung menghasilkan susunan yang sama.
4. Kalau semua gagal, pakai `RunLayoutSources.safeLayout`, atur `usedFallbackLayout = true`, dan catat kesalahan. Pemain tetap bisa bermain.

**`RunSeedAudit`** adalah alat Editor yang menjalankan 1000 seed dan melaporkan alasan gagal terbanyak. Peluang gagal di atas 5 persen berarti data titik perlu diperbaiki, bukan menaikkan `maxSeedRerolls`.

### Perubahan `LootSpawnerSystem`

Masalah lamanya: `Start()` selalu menyebar loot, sehingga setelah load, item bisa tersebar ulang dan berlipat. Logika barunya:

1. `Start()` memanggil `SpawnOrRestore()`, bukan `SpawnLootAcrossMap()`.
2. `LootSpawnerSystem` menjadi `ISaveable`, dengan `SaveId = spawnerId`. Statusnya berupa daftar `LootPointState { pointIndex, itemId, amount, isTaken }`.
3. **`SpawnOrRestore()`:**
   - Kalau status dari `RegionStateStore` (#88) ada, pulihkan item dari daftar. Titik dengan `isTaken = true` dilewati. Tidak ada pengacakan.
   - Kalau belum ada, ini penyebaran pertama dalam run. Panggil `RollAndRecord()`.
4. **`RollAndRecord()`:** untuk tiap titik, buat `var s = Fork(streamId, spawnerId + "/" + pointIndex)`. `streamId` bernilai `ShipLoot` di `ShipPhase` dan `IslandLoot` di pulau. Titik kosong ditentukan `s.NextFloat01() < lootPointEmptyChance`. Item dipilih dengan `s.PickWeighted(bobot)` (#103), lalu dicatat ke daftar.
5. Item memakai `WorldItem` dari `ObjectPooler`, bukan `Instantiate`. Saat diambil, `WorldItem` memanggil `spawner.MarkTaken(pointIndex)`.
6. `CaptureState()` menulis daftar tersebut untuk `RegionStateStore` dan save.

**Hasilnya:** run baru menyebar loot sekali per spawner. Setelah load atau region kembali, hasilnya dibaca dari save. Region yang belum pernah dikunjungi saat game disimpan tetap menyebar saat pertama dimuat, dengan hasil yang sama seperti tanpa save, karena kuncinya tetap.

`LootSpawnerSystem` di `ShipPhase` tidak punya save (#82), jadi selalu menyebar lewat `RollAndRecord()`. Seed yang sama memberi susunan kapal yang sama.

### Menyimpan dan memulihkan

**`public RunSeedSaveData CaptureSave()`** menyalin seed, `state` aliran berurutan, dan `CurrentLayout`.

**`public void RestoreFromSave(RunSeedSaveData data)`**, dipanggil `RunLifecycle.ContinueRun()`:

1. Isi `CurrentSeed`, `CurrentSeedText`, dan `CurrentLayout`. **Susunan dibaca dari save dan tidak dibangun ulang,** supaya perubahan data tidak memindahkan suku cadang yang sudah tercatat.
2. Pulihkan `sequential` dari `sequentialStates`. Karena hasil Lucky Machine dan drop melanjutkan status tersimpan, keluar lalu masuk lagi tidak mengubah hasil berikutnya.
3. Kalau `version` berbeda, ikuti aturan versi di #80.

`RunSeedManager` mendaftar sebagai `ISaveable` lewat #80. Datanya dipulihkan sebelum scene dimuat.

### Menampilkan seed

| Tempat | Isi | Cara |
|---|---|---|
| Layar `GameOver` dan `WinScreen` | `Seed 7F3A-91C2 · v0.1.0` | `UIManager` membaca `CurrentSeedText` dan `Application.version` |
| `DevConsole`: `seed` | Seed, versi, dan `usedFallbackLayout` | #56 |
| `DevConsole`: `seed <XXXX-XXXX>` | Mengisi `forcedNextSeed` untuk run berikutnya | #56 |
| `DevConsole`: `layout` | Titik suku cadang, NPC, dan jadwal bencana | #56 |
| `LocalStatsManager` | Seed run terakhir, untuk laporan bug (#57) | #57 |

Layar akhir membaca seed sebelum reset, karena `ResetForNewRun()` baru berjalan setelah pemain memilih tombol.

**Cara mengulang bug hari ke-59:** jalankan `seed 7F3A-91C2`, klik Run Baru, lalu `setday 59`. Lokasi suku cadang, jadwal bencana, dan cuaca hari itu sama, karena semuanya berkunci atau dihitung dari awal. **Hasil aliran `Enemy`, `Gameplay`, dan `LuckyMachine` tidak dijamin sama,** karena bergantung pada tindakan pemain. Seed juga hanya sah pada versi game dan data yang sama.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** tidak ada. `RunStartedEvent` dikirim `RunLifecycle`, dengan isi `CurrentSeed`.

**Event yang didengar:** tidak ada. Sistem lain memanggil fungsi publiknya.

| Sistem | Hubungan |
|---|---|
| `RunLifecycle` (#82) | Memanggil `BeginNewRun`, `RestoreFromSave`, dan `ResetForNewRun` |
| `WorldStateSave` (#80) | Menyimpan `RunSeedSaveData` |
| `LootSpawnerSystem`, `RegionLootTable` (#103) | Memakai `Fork`. Tabel dan bobot milik #103 |
| `HeavyItemSystem` (#69) | Membaca `CurrentLayout.partPointByPart`, dan mengurus penyebarannya |
| `NPCRoster` (#76), `ShipInteractable` | Membaca `CurrentLayout.shipNpcIds` |
| `DisasterManager` (#70) | Membaca `CurrentLayout.disasterSchedule`. Isi `DisasterData` milik #70 |
| `RegionalWeather` (#115), `WeatherManager` | `Fork(Weather, (int)region * 10000 + slot)`, dengan `slot` berupa total jam game dibagi `GameBalanceConfig.weather.weatherIntervalGameHours` |
| `BeachSupplyManager` (#71) | `Fork(BeachSupply, currentDay)` |
| `SpawnManager` (#62), `AIDirector` (#27) | Memakai aliran `Enemy` |
| `ResourceNode`, `FishingSystem` (#55) | Memakai aliran `Gameplay` |
| `LuckyMachine` (#72) | Memakai aliran `LuckyMachine` |
| `ProgressionSafety` (#116) | Mengandalkan seed yang valid dari blueprint ini |
| `DevConsole` (#56) | Perintah `seed` dan `layout` |
| `UIManager` | Menampilkan seed di layar akhir |

**Batas pekerjaan:** tabel loot per region ada di #103, dan jadwal Blood Moon ada di #50.

## 4. Kasus Khusus

- **Objek pool:** objek pool tidak boleh menyimpan salinan aliran. Kalau butuh acak saat `OnSpawned`, objek mengambil `Fork` atau `GetStream` baru. Dengan begitu, objek yang dipakai ulang tidak mengulang urutan lama.
- **Game dijeda:** `RunSeedManager` tidak memakai waktu. Tidak ada aliran yang maju saat dijeda, kecuali ada panggilan sistem yang tetap berjalan.
- **Permadeath:** `ResetForNewRun()` mengosongkan `CurrentSeed`, `CurrentSeedText`, `CurrentLayout`, dan `sequential`. Save yang memuat seed dihapus oleh #82. `forcedNextSeed` tidak ikut dihapus, karena dipakai run berikutnya.
- **Yang disimpan:** seed, `sequentialStates`, `layout`, dan `usedFallbackLayout`. **Yang tidak disimpan:** hasil `Fork`, karena bisa dihitung ulang, kecuali hasil loot yang sudah dicatat `LootSpawnerSystem`.
- **Memuat save:** tidak memanggil `BuildLayout`, dan tidak membuat seed baru.
- **Fase 1 ditutup:** tidak ada save, jadi seed hilang. Run berikutnya memakai seed baru.
- **Data diubah di versi baru:** `layout` di save tetap berlaku. Kalau `partPointByPart` menunjuk indeks yang tidak ada, aturan versi #80 yang berlaku.
- **Panggilan sebelum run dimulai:** `Fork` saat `CurrentSeed == 0` hanya terjadi saat tes. Di Editor, sistem mencatat peringatan dan memakai seed sementara dari `EditorQuickStart` (#82).
- **Bobot loot kosong:** `PickWeighted` mengembalikan -1, dan titik itu dibiarkan kosong.
- **Seed yang gagal dipaksa:** berjalan dengan peringatan di Console, dan tidak diubah.
- **Pemakai `UnityEngine.Random`:** hasil tampilan tidak boleh memengaruhi keputusan gameplay. Hasil pencarian teks `Random.` dicek saat review.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** Blood Moon tetap (#50), Diary tiga bab (GDD bagian 4), kargo tiap kelipatan 7 hari (GDD bagian 6), serta syarat menang dan item awal (`GLOSSARY.md`).

**Keputusan teknis di blueprint ini:**

- Tiap sistem punya aliran sendiri, dan `System.Random` tidak dipakai.
- Aliran berkunci dipakai untuk hal yang harus sama pada seed yang sama.
- Jadwal bencana dihitung sekali dan disimpan, supaya `setday` tetap konsisten.

**Usulan yang menunggu developer (dicatat di #85):**

1. **Seed dari pemain:** **tidak diizinkan pada versi awal.** Mengetahui seed berarti lokasi suku cadang dan jadwal bencana bisa dibagikan, yang mengurangi tekanan permadeath. Seed tetap ditampilkan untuk laporan bug, dan hanya `DevConsole` yang bisa memasukkannya. Fitur ini bisa ditambah nanti tanpa mengubah sistem.
2. **Daftar yang berubah tiap run:** mengikuti tabel "Apa yang berubah dan tetap tiap run".
3. **Lokasi suku cadang:** acak dari titik buatan tangan di Region 4, bukan posisi bebas. Sekurangnya 10 titik untuk 5 suku cadang. Ini juga menjawab pilihan di #69.
4. **NPC kapal:** tidak selalu sama tiap run. Jumlah dan daftarnya mengikuti #76.
5. **Jaminan bekal kapal:** minimal 3 makanan, 2 peluru, dan 1 minuman di seluruh kapal.
6. **Batas bencana:** bencana besar tidak sebelum hari ke-3, dan berjarak minimal 4 hari.
7. **Isi layar akhir:** seed ditampilkan kecil bersama nomor versi.

## 6. Skenario Tes

1. Mulai dua Run Baru. **Hasil:** seed berbeda, dan posisi loot kapal berbeda.
2. Jalankan `seed 7F3A-91C2`, Run Baru, catat loot kapal. Ulangi dengan seed sama. **Hasil:** susunan kapal, NPC, titik suku cadang, dan jadwal bencana sama.
3. Tambahkan pemanggilan aliran `Gameplay` lewat `DevConsole`, lalu periksa loot. **Hasil:** hasil `IslandLoot` tidak berubah.
4. Sampai di Pulau, ambil beberapa loot, simpan, keluar, lalu lanjutkan. **Hasil:** loot yang diambil tidak muncul lagi, dan loot lain tidak berlipat.
5. Ulangi langkah 4 sebanyak tiga kali. **Hasil:** jumlah item di dunia tidak bertambah.
6. Kunjungi Region 2 setelah load, padahal belum pernah dikunjungi. **Hasil:** loot menyebar sekali, sama seperti pada seed yang sama tanpa save.
7. Putar Lucky Machine dan catat hasil. Tutup paksa game, lanjutkan dari autosave sebelum putaran itu, lalu putar lagi. **Hasil:** hasilnya sama, jadi pemain tidak bisa mengulang demi hadiah lain.
8. Jalankan `setday 59` pada seed yang sama, lalu `layout`. **Hasil:** isi `layout` dan jadwal bencana sama seperti saat bermain normal.
9. Mati. **Hasil:** seed tampil di layar `GameOver`, dan cocok dengan perintah `seed` sebelum mati.
10. Klik Ulangi dari Kapal. **Hasil:** seed baru dibuat.
11. Jalankan `RunSeedAudit` dengan 1000 seed. **Hasil:** semua lolos atau mendapat seed pengganti, dan peluang gagal di bawah 5 persen.
12. Hapus sebagian titik di `PartSpawnPointTable` hingga kurang dari 5, lalu mulai Run Baru. **Hasil:** `safeLayout` dipakai, kesalahan tercatat, dan game tidak berhenti.
13. Buka file save. **Hasil:** seed, status aliran, dan `layout` tersimpan.
14. Cari `Random.` di script gameplay. **Hasil:** hanya efek tampilan yang memakainya.
