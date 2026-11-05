# Blueprint: BeachSupplyManager.cs
**Lokasi:** `docs/blueprint/Island_Logic/BeachSupplyManager.md`
**Deskripsi Utama:** Memunculkan kargo dari sisa kapal di pantai selatan Region 1 setiap 7 hari, lengkap dengan isi, batas waktu, dan risikonya.
**Issue:** #71
**Rujukan GDD:** Bagian 6 (Weekly Beach Supply). Jumlah, isi per hari, batas waktu, dan risiko adalah usulan.

## 1. Arsitektur & Variabel Utama

`BeachSupplyManager` adalah manager di scene `IslandPhase` yang memakai `ISaveable`. Dalam satu waktu hanya ada satu kiriman.

```csharp
[Serializable]
public struct BeachCrateTier { public int minDay; public LootTable table; }

[Serializable]
public class BeachDeliveryState
{
    public int day;                     // 0 berarti tidak ada kiriman
    public float expireTotalHours;
    public bool[] isOpened;             // Per kargo
    public bool animalAmbushDone, zombieAmbushDone;
}
```

| Objek | Fungsi |
|---|---|
| `BeachSupplyPoint` | Titik buatan tangan di `IslandPhase`: `position`, `isReachable` |
| `BeachCrate : IslandInteractable` | Kargo di layer `Interactable`, dari pool `Supply_BeachCrate` |
| `fallbackPoint` | Satu titik yang selalu sah |
| `compassMarkerId` | Penanda di kompas |

| Nilai config | Awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.beachSupply.intervalDays` | 7 | Jadwal kiriman |
| `GameBalanceConfig.beachSupply.crateCountBase` | 2 | Jumlah kargo hari ke-7 |
| `GameBalanceConfig.beachSupply.crateCountPerTwoWeeks` | 1 | Tambahan per 14 hari |
| `GameBalanceConfig.beachSupply.crateCountMax` | 4 | Batas jumlah kargo |
| `GameBalanceConfig.beachSupply.rollsPerCrate` | 3 | Jumlah isi per kargo |
| `GameBalanceConfig.beachSupply.crateLifetimeGameHours` | 18 | Dari 06:00 sampai 00:00 |
| `GameBalanceConfig.beachSupply.crateMinLevel` | 0,85 | Titik harus di atas air pada level ini |
| `GameBalanceConfig.beachSupply.openHoldSeconds` | 1,5 | Waktu tahan `E` |
| `GameBalanceConfig.beachSupply.openNoiseRadius` | 10 m | Suara saat dibuka |
| `GameBalanceConfig.beachSupply.ambushTriggerDistance` | 50 m | Jarak pemain yang memicu penyergapan |
| `GameBalanceConfig.beachSupply.ambushAnimalsBase` / `Max` | 2 / 4 | Hewan siang, ditambah 1 per 21 hari |
| `GameBalanceConfig.beachSupply.ambushZombiesBase` / `Max` | 4 / 10 | Zombie malam, ditambah 1 per 7 hari |

### Tabel isi

Mengikuti GDD: Air Minum Bersih, Makanan Kaleng, Peluru, dan Resource langka. Tiap baris memakai `LootTable` (#103), dan pilihan tier mengikuti `minDay`.

| Item | Jumlah | Hari 7-14 | Hari 21-35 | Hari 42+ |
|---|---|---|---|---|
| `Drink_CleanWater` | 2 | 40 | 30 | 25 |
| `Food_Can` | 2 | 40 | 30 | 20 |
| `Ammo_9mm` | 6 sampai 12 | 15 | 30 | 30 |
| `Mat_Scrap` (langka) | 3 | 3 | 5 | 12 |
| `Med_Antibiotic` (langka) | 1 | 0 | 2 | 8 |
| `Item_Battery` (langka, #98) | 1 | 5 | 8 | 10 |

`Med_Vaccine` tidak masuk tabel, karena GDD menetapkan vaksin baru hanya dari bahan Region 2.

## 2. Detail Logika & Fungsi

### Pemicu

**`HandleDayChanged(DayChangedEvent e)`**: kalau `e.newDay % intervalDays == 0`, isi `pendingDay = e.newDay`.

`DayChangedEvent` terjadi pukul 00:00, saat puncak pasang. Karena itu kargo baru muncul saat fajar.

**`HandleDaybreak(DaybreakEvent e)`**: kalau `pendingDay == currentDay`, panggil `Deliver(pendingDay)`.

Jadwalnya hari ke-7, 14, 21, dan seterusnya, tanpa hari terakhir. Sampai hari ke-60 ada 8 kiriman, dan kiriman berikutnya datang pada hari ke-63, 70, dan seterusnya.

### Jumlah dan titik

**`private void BuildDelivery(int day, out int[] pointByCrate, out RollResult[] contents)`** adalah fungsi murni:

1. `var s = RunSeedManager.Instance.Fork(RandomStreamId.BeachSupply, day)`.
2. `count = Min(crateCountBase + day / 14 * crateCountPerTwoWeeks, crateCountMax)`. Hasilnya 2, 3, 3, 4, lalu tetap 4 kargo, termasuk setelah hari ke-60.
3. `s.Shuffle(indeks titik)`, lalu ambil titik `isReachable`.
4. Untuk tiap kargo, ambil `rollsPerCrate` isi dari `LootTable` tier yang sesuai.
5. Isi pertama pada kargo pertama selalu `Drink_CleanWater` atau `Food_Can`, supaya kiriman tidak pernah hanya berisi barang lain.

Karena hasilnya hanya bergantung pada seed dan hari, save cukup menyimpan kargo mana yang sudah dibuka.

**`private void Deliver(int day)`**:

1. Isi `state`, dengan `expireTotalHours = TotalGameHours + crateLifetimeGameHours`.
2. Panggil `SpawnCrates()`, kalau `RegionStreaming.Instance.IsLoaded(Volcano)`. Kalau belum dimuat, kargo muncul saat `RegionLoadStateChangedEvent` menyatakan `Loaded`.
3. Terbitkan `BeachSupplyArrivedEvent { day, crateCount }`.
4. Kirim notifikasi `Kargo kapal terdampar di pantai selatan` (#22).
5. `compassMarkerId = CompassSystem.Instance.AddMarker(titik tengah kargo, MarkerIcon.Supply)` (#30).

**`SpawnCrates()`** memeriksa tiap titik sebelum dipakai:

| Syarat | Kalau gagal |
|---|---|
| `NavMesh.SamplePosition` berhasil dalam 2 m | Pakai titik berikutnya |
| `PhysicsQuery.OverlapSphere` 1,5 m dengan `PhysicsMasks.BuildOverlap` kosong | Pakai titik berikutnya |
| `groundHeight` di atas `TideManager.WaterYAtLevel(crateMinLevel)` | Pakai titik berikutnya |
| Semua titik habis | Pakai `fallbackPoint`, dengan jarak acak 1,5 m antar-kargo |

Syarat tinggi juga diperiksa di Editor, sehingga titik di data sudah benar. Dengan `crateMinLevel` 0,85, kargo tidak terendam oleh pasang siang, termasuk saat badai.

### Membuka kargo (#17)

`BeachCrate` memakai `IInteractable`:

- `GetPrompt()` berisi `Buka Kargo`, dan `HoldSeconds = openHoldSeconds`.
- `IsAvailable` bernilai `!isOpened`.

**`OnInteract`**:

1. `state.isOpened[i] = true`, dan mainkan animasi.
2. Tiap isi menjadi `WorldItem` lewat `WorldItemManager.Spawn(..., despawns: false)`, sama seperti peti di #17. Pemain tidak perlu ruang inventori.
3. `NoiseEmitter.Emit(openNoiseRadius, NoiseSourceKind.Container)` dari komponen di prefab kargo (#49).
4. Terbitkan `LocalStatsManager` lewat `BeachCrateOpenedEvent { day }`.
5. Kalau semua kargo sudah dibuka, hapus penanda kompas.

Barang yang keluar tetap berada di pantai dan mengikuti aturan air di #91, yaitu pindah ke `TideSafePoint` kalau terkena pasang.

### Batas waktu

**`HandleHour(HourChangedEvent e)`**:

Kalau `TotalGameHours >= expireTotalHours`, panggil `ExpireDelivery()`:

1. Kembalikan semua kargo ke `ObjectPooler`, termasuk yang sudah terbuka.
2. Mainkan efek air di kargo yang terlihat.
3. Kalau ada kargo yang belum dibuka dan pemain berada di Region 1, kirim notifikasi `Kargo terseret air pasang`.
4. Hapus penanda kompas, dan `state.day = 0`.

Dengan satu hari 900 detik, pemain memiliki sekitar 11 menit nyata. Jarak sekitar 1.000 m dari ujung terjauh butuh sekitar 200 detik berjalan, jadi kargo tetap bisa dicapai. Yang sulit adalah pergi tanpa meninggalkan pekerjaan lain.

### Risiko pantai

GDD menyebut hewan buas pada siang hari dan zombie pada malam hari, jadi penyergapan mengikuti waktu tersebut.

**`TickAmbush()`**, dipanggil dari `HandleHour`, kalau kiriman masih ada dan ada kargo yang belum dibuka:

| Kondisi | Tindakan |
|---|---|
| Siang, pemain dalam `ambushTriggerDistance`, dan `!animalAmbushDone` | `SpawnManager.RequestBurst(beachDayTable, ambushAnimals, tengah, 25)` |
| Malam, pemain dalam `ambushTriggerDistance`, dan `!zombieAmbushDone` | `SpawnManager.RequestBurst(beachNightTable, ambushZombies, tengah, 25, AIDirector.Instance.GetPostTargetModifiers(day))` |

- `ambushAnimals = Min(ambushAnimalsBase + day / 21, ambushAnimalsMax)`, sehingga hari ke-7 berisi 2 hewan.
- `ambushZombies = Round(Min(ambushZombiesBase + day / 7, ambushZombiesMax) * AIDirector.Instance.GetPostTargetCountMultiplier(day))`, sehingga hari ke-7 berisi 5 zombie, dan hari ke-70 berisi 12 zombie. Hewan tidak memakai pengali ini.
- Kalau pemain terlalu jauh, tidak ada yang muncul. Jarak ini mengikuti `despawnDistance` 70 m di `SpawnManager`, supaya musuh tidak langsung dikembalikan. Pemeriksaan diulang pada jam berikutnya.
- `HandleHour` hanya sekali per 37,5 detik, jadi pemeriksaan tambahan berjalan saat pemain memasuki `tideZone`.
- `RequestBurst` sudah menjaga agar musuh muncul di luar pandangan.

### Isi yang berubah

Tier memberi air dan makanan lebih dulu pada pekan awal, karena pemain masih membangun sumber sendiri. Peluru naik pada pekan tengah, saat horde makin besar. Barang langka naik menjelang akhir, untuk mendukung perjalanan Region 3.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `BeachSupplyArrivedEvent`, `BeachCrateOpenedEvent` (baru), dan `NoiseEmittedEvent` (lewat `NoiseEmitter`).

**Event yang didengar:** `DayChangedEvent`, `DaybreakEvent`, `HourChangedEvent`, dan `RegionLoadStateChangedEvent`.

| Sistem | Hubungan |
|---|---|
| `GameManager` | `currentDay` |
| `RunSeedManager` (#89) | `Fork(BeachSupply, day)` |
| `LootTable`, `RegionLootTable` (#103) | Bobot isi |
| `IslandInteractable`, `PlayerInteractor` (#17) | Interaksi tahan `E` |
| `WorldItemManager` (#66) | Memunculkan isi |
| `TideManager` (#91) | `WaterYAtLevel`, aturan barang terkena air |
| `SpawnManager` (#62) | `RequestBurst` |
| `RegionStreaming` (#88) | `IsLoaded` |
| `NotificationManager` (#22), `CompassSystem` (#30) | Pemberitahuan dan arah |
| `WorldStateSave` (#80) | Menyimpan status |
| `SupplyDropManager` (#54), `DebrisRetrieval` (#106) | Sumber terpisah |

## 4. Kasus Khusus

- **Pool:** `BeachCrate.OnSpawned` mengisi indeks, memasang `isOpened` dari `state`, dan menutup tutup kargo. `OnReturned` mengosongkan indeks dan target hover.
- **Game dijeda:** jam berhenti, sehingga batas waktu dan penyergapan ikut berhenti. Interaksi terkunci oleh #83.
- **Permadeath:** status dihapus. Run baru memakai seed baru, dengan isi dan titik baru.
- **Save dan load:** disimpan `BeachDeliveryState`. Kargo yang belum dibuka muncul kembali dengan isi yang sama, karena `BuildDelivery` berfungsi murni. Yang sudah dibuka tampil terbuka. Isi yang tercecer disimpan oleh `WorldItemManager`. Notifikasi tidak diulang, dan penanda kompas dipasang lagi kalau masih ada kargo tertutup.
- **Lewat batas saat tidak dimuat:** `HandleHour` tetap jalan karena manager berada di `IslandPhase`. Kargo yang tidak muncul tetap kedaluwarsa.
- **Region 1 dilepas:** kargo kembali ke pool, dan `state` tetap ada. Dimunculkan lagi saat `Loaded`.
- **`setday` atau tidur melewati fajar:** `DaybreakEvent` tetap terkirim karena tidur berhenti saat fajar. `setday 7` memberi `pendingDay`, dan kargo muncul pada fajar berikutnya.
- **Setelah hari ke-60:** kiriman ke-8 pada hari ke-56, dan ke-9 pada hari ke-63. Isi tetap memakai tier `Hari 42+`, jadi makanan, air, dan peluru terus tersedia. Hewan tetap dibatasi `ambushAnimalsMax`. Zombie dibatasi `ambushZombiesMax`, lalu dikali pengali jumlah dari `AIDirector.md`, jadi paling banyak 15.
- **Bencana:** kiriman tidak dibatalkan. Saat badai, pasang naik, tetapi kargo masih di atas level 0,85 pada siang hari.

## 5. Keputusan Desain

**Sudah ditetapkan GDD:** jadwal hari ke-7, 14, dan seterusnya, dan empat kelompok isi.

**Keputusan developer:** setelah hari ke-60, penyergapan zombie memakai pengali jumlah, HP, dan damage yang sama dengan `AIDirector.md`.

**Usulan yang disetujui developer sebagai nilai awal:**

1. **Lokasi:** hanya pantai selatan Region 1 (#88). Rekomendasi: pakai, karena region ini selalu terbuka.
2. **Jumlah kargo:** 2 sampai 4, dengan 3 isi per kargo.
3. **Batas waktu:** muncul pukul 06:00, dan hilang pukul 00:00 pada puncak pasang. Rekomendasi: pakai, supaya GDD "turun sebentar" dan #91 saling menguatkan.
4. **Risiko:** hewan siang dan zombie malam, tanpa Zombie Desa. Rekomendasi: pakai, karena pantai berada di Region 1.
5. **Isi berubah menurut hari:** tiga tier di tabel. Rekomendasi: pakai.
6. **Barang langka:** `Mat_Scrap` dan `Med_Antibiotic`, tanpa vaksin. Daftar ini bisa diperpanjang setelah #103 selesai.
7. **`SaveId`:** `world_supply_beach`, terpisah dari #54, karena `Register` menolak `SaveId` ganda.
8. **Setelah hari ke-60:** kiriman terus berjalan tiap 7 hari dengan 4 kargo dan tier `Hari 42+`. Rekomendasi: pakai, supaya run panjang tetap mungkin dimenangkan. Jumlah peluru perlu diuji bersama pengali HP di `AIDirector.md`.

## 6. Skenario Tes

1. `setday 7`, lalu tunggu fajar. **Hasil:** 2 kargo di pantai, ada notifikasi, ada penanda kompas, dan `BeachSupplyArrivedEvent` terkirim sekali.
2. `setday 8`. **Hasil:** tidak ada kiriman baru.
3. Tahan `E` 1,5 detik pada kargo. **Hasil:** isi keluar sebagai `WorldItem`, walaupun inventori penuh.
4. Ulangi dengan seed sama. **Hasil:** titik dan isi sama.
5. Simpan setelah membuka satu kargo, lalu load. **Hasil:** satu terbuka, satu tertutup, dan isinya tetap.
6. Tunggu sampai 00:00 tanpa membuka. **Hasil:** kargo hilang, dan ada notifikasi.
7. `setday 56`, lalu cek isi beberapa seed. **Hasil:** barang langka lebih sering, dan `Med_Vaccine` tidak pernah muncul.
8. Mendekat pada siang hari. **Hasil:** 2 sampai 4 hewan muncul di luar pandangan, satu kali saja.
9. Berada di sana pada malam hari dengan kargo tertutup. **Hasil:** zombie muncul sesuai rumus, dan tidak ada hewan.
10. Berada di Region 3 saat kiriman datang. **Hasil:** notifikasi tetap muncul, dan kargo muncul saat Region 1 dimuat.
11. Bangun struktur di dekat titik kargo lewat `DevConsole`. **Hasil:** titik lain terpilih.
12. Cek titik di Editor. **Hasil:** semua `isReachable` bernilai `true` dan berada di atas level 0,85.
13. `setday 63`, lalu tunggu fajar. **Hasil:** 4 kargo muncul dengan isi tier `Hari 42+`.
14. `setday 70`, lalu berada di dekat kargo pada malam hari. **Hasil:** 12 zombie muncul, dengan HP 1,2 kali dan damage 1,1 kali. Ulangi pada siang hari. **Hasil:** jumlah hewan tidak berubah.
