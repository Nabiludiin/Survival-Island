# Blueprint: HydrationSystem.cs & RainwaterCollector.cs
**Lokasi:** `docs/blueprint/raft/HydrationSystem.md`
**Deskripsi Utama:** Sumber air dan aturan haus: pengurangan berdasar aktivitas, item minum, titik air kotor, penampung hujan, dan penalti dehidrasi.
**Issue:** #43
**Rujukan GDD:** Bagian 3 (haus diisi Air Minum Bersih, kelapa, atau air hujan yang ditampung, dan air kotor berisiko sakit) dan bagian 6 (Air Minum Bersih dari kargo). `Thirst` sebagai status inti adalah keputusan #85. Angka dan cara kerja penampung adalah usulan.

## 1. Arsitektur & Variabel Utama

### Pola

`SurvivalStats` tetap pemilik `Thirst` dan menghitung penurunannya (`VitalsRules.md`). Blueprint ini mengatur asal air.

| Kelas | Tempat | Fungsi |
|---|---|---|
| `HydrationSystem` | Pemain Fase 2, scene `IslandPhase` | Daftar penampung, tick per jam game, peringatan, dan pengambilan air kotor |
| `RainwaterCollector` | Prefab struktur, turunan `IslandInteractable` | Menampung air hujan, lalu diambil dengan `E` |
| `WaterSourcePoint` | Titik di tepi sungai dan danau, turunan `IslandInteractable` | Memberi `Drink_DirtyWater` |

**Variabel `HydrationSystem`:**

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Instance` | `HydrationSystem` | `null` | Singleton per scene |
| `collectors` | `List<RainwaterCollector>` | Kosong | Semua penampung, termasuk yang region-nya sedang tidak dimuat |
| `lastThirstLowNotified` | `bool` | `false` | Peringatan haus rendah sudah tampil |

**Variabel `RainwaterCollector`:**

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `storedPortions` | `float` | 0 | Isi saat ini, dalam porsi |
| `homeRegion` | `RegionType` | Dari `RegionStreaming.GetRegionAt` | Region cuaca yang dibaca |
| `isExposedToSky` | `bool` | Dihitung saat dibangun | Tidak tertutup atap |
| `fillVisual` | `Transform` | Dari prefab | Tinggi air di dalam wadah |

### Item minum

Baris lama di `ItemUseSystem.md` tidak berubah. `ItemUseSystem` menjalankan pemakaiannya, dan `DiseaseManager` (#45) menghitung risiko air kotor.

| Item | ID | Haus | Lapar | Pakai | Risiko | Cara mendapat |
|---|---|---|---|---|---|---|
| Botol Air Bersih | `Drink_CleanWater` | +25 | 0 | 2 detik | Tidak ada | Kapal, kargo (#71), loot, penampung hujan, atau merebus air kotor (#44) |
| Kelapa | `Drink_Coconut` | +25 | +5 | 3 detik | Tidak ada | Pohon kelapa di Region 1 dan Region 2, lewat `PickupInteractable` |
| Air kotor | `Drink_DirtyWater` | +15 | 0 | 2 detik | `poisonChance` 0,35 | `WaterSourcePoint` di sungai dan danau |

**Satu porsi air tanpa botol kosong.** Air memakai satu item habis pakai, dan tidak meninggalkan botol kosong, supaya slot inventori tidak terisi wadah.

**Air laut tidak bisa diminum.** Tidak ada `WaterSourcePoint` di laut. Rekomendasi ini mengikuti GDD, yang tidak menyebut laut sebagai sumber air.

### Pengurangan haus berdasar aktivitas

Dihitung di `SurvivalStats.TickDrains`, dengan `thirstDrainPerSec` 0,15 sebagai dasar.

| Aktivitas | Pengali | Nama config | Haus penuh habis dalam |
|---|---|---|---|
| Diam atau berjalan | 1 | Tidak ada | 667 detik, sekitar 0,74 hari game |
| Berlari | 1,5 | `GameBalanceConfig.vitals.sprintThirstMultiplier` | 444 detik |
| Berenang atau memanjat | 1,25 | `GameBalanceConfig.hydration.exertionThirstMultiplier` | 533 detik |

`isSprinting` diisi saat `SpendStamina` dipakai dengan `StaminaUse.Sprint`, dan `isExerting` diisi dengan cara yang sama untuk `Swim` atau `Climb`. Kalau lebih dari satu aktif, pengali terbesar yang dipakai.

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.hydration.exertionThirstMultiplier` | 1,25 | Berenang dan memanjat |
| `GameBalanceConfig.hydration.dehydratedStaminaRegenMultiplier` | 0 | Regenerasi stamina saat `Thirst` nol |
| `GameBalanceConfig.hydration.dirtyWaterHoldSeconds` | 1 | `holdSeconds` di `WaterSourcePoint` |
| `GameBalanceConfig.hydration.collectorCapacityPortions` | 4 | Isi penuh penampung |
| `GameBalanceConfig.hydration.rainFillPerGameHour` | 0,5 | Isi per jam game saat `Rain` |
| `GameBalanceConfig.hydration.stormFillPerGameHour` | 0,75 | Isi per jam game saat `Storm` |
| `GameBalanceConfig.hydration.skyCheckDistance` | 20 | Jarak raycast ke atas |
| `GameBalanceConfig.hydration.structureRecheckRadius` | 6 | Bangunan baru dalam jarak ini memicu cek ulang |

Dengan interval cuaca 8 jam game (`GameBalanceConfig.weather.weatherIntervalGameHours`), satu periode `Rain` menghasilkan 4 porsi. Ini setara kebutuhan satu hari, yaitu 4 kali minum.

`Drink_DirtyWater.poisonChance` bernilai 0,35. Efeknya ada di #45.

## 2. Detail Logika & Fungsi

### Penalti dehidrasi

`SurvivalStats` menjalankan tabel berikut setiap kali `VitalsChangedEvent` diterbitkan.

| Syarat | Akibat | Sumber aturan |
|---|---|---|
| `Thirst < lowVitalThreshold` (25) | `SetPenalty(Thirst, 1, lowThirstRegenMultiplier)`, regenerasi stamina setengah | `VitalsRules.md` |
| `Thirst <= 0` | `SetPenalty(Thirst, 1, dehydratedStaminaRegenMultiplier)`, **stamina tidak bisa beregenerasi** | Checklist issue ini |
| `Thirst <= 0` | Baris HP 4: -0,5 HP per detik, `DamageType.Dehydration` | `VitalsRules.md` |
| `Thirst >= lowVitalThreshold` | `SetPenalty(Thirst, 1, 1)` | Kembali normal |

Stamina yang tersisa tetap bisa dipakai sampai habis. Setelah minum, regenerasi kembali pada saat `Thirst` lebih dari 0. Pemulihan bertahap 20 detik (#66) cukup menaikkan nilai ini pada detik pertama.

### Peringatan

**`void HandleWarnings(VitalWarningChangedEvent e)`**:

- Tanda `ThirstLow` baru menyala dan `lastThirstLowNotified == false`: notifikasi `Kamu haus` (#22).
- Tanda `Dehydrated` baru menyala: notifikasi `Dehidrasi: stamina tidak pulih`.
- Tanda `ThirstLow` mati: `lastThirstLowNotified = false`.

### Mengambil air kotor

`WaterSourcePoint` diletakkan tangan di tepi sungai dan danau, di layer `Interactable`, dan memakai `holdSeconds = dirtyWaterHoldSeconds`. Layer `Water` tetap trigger dan tidak masuk mask `Interact`.

**`protected override bool OnInteract(in InteractContext ctx)`**:

1. `int sisa = ctx.Inventory.AddItem(dirtyWaterItem, 1)`.
2. Kalau `sisa > 0`, tampilkan `Inventori penuh`, kembalikan `false`.
3. Mainkan suara air. Kembalikan `true`.

Sumber ini tidak habis dan tidak perlu disimpan. `IsAvailable` selalu `true`, dan `blockedWhileCarrying = true`.

### `RainwaterCollector`

Dibangun lewat menu bangun (#79), dengan `buildableId = Build_RainCollector`, biaya usulan 6 `Mat_Wood` dan 2 `Mat_Cloth`. Aturan pantai dari `BuildManager` tetap berlaku. Objek berada di layer `Structure` untuk tubuhnya, dan layer `Interactable` untuk collider interaksinya.

**Pendaftaran:** saat objek dibuat atau dipulihkan, panggil `HydrationSystem.Instance.Register(this)`, dan saat dihancurkan panggil `Unregister(this)`. Pendaftaran ini tidak memakai `OnEnable`, karena `structureRoot` dimatikan saat region dilepas (#88), sedangkan air harus tetap bertambah. Fungsi pada objek nonaktif tetap bisa dipanggil.

**`HydrationSystem.HandleHour(HourChangedEvent e)`** memanggil `collector.TickHour()` untuk semua `collectors`.

**`public void TickHour()`**:

```csharp
if (!isExposedToSky || storedPortions >= capacity) return;
WeatherType w = RegionalWeather.Instance.GetWeather(homeRegion);
float rate = w == WeatherType.Rain ? b.rainFillPerGameHour
           : w == WeatherType.Storm ? b.stormFillPerGameHour : 0f;
storedPortions = Mathf.Min(capacity, storedPortions + rate);
UpdateVisual();
```

- `Clear`, `Fog`, `Snow`, dan `Blizzard` tidak mengisi. Salju tidak mencair di wadah ini.
- Cuaca dibaca per `homeRegion`, bukan region pemain (#115). Penampung di Region 1 tetap terisi saat pemain di Region 3.
- Tidur otomatis mengisi lebih cepat, karena jam game berjalan lebih cepat.
- Satu tick per jam game cukup, karena air hujan tidak butuh ketepatan per detik.

**Terlindung atap: `private void RefreshSkyExposure()`**: `PhysicsQuery.Raycast` ke atas sejauh `skyCheckDistance` dengan `PhysicsMasks.Sunlight`. `isExposedToSky` bernilai `true` kalau tidak mengenai apa pun. Dipanggil saat dibangun, dipulihkan, dan saat `StructureBuiltEvent` atau `StructureDestroyedEvent` terjadi dalam `structureRecheckRadius`. Hologram bangunan juga mengecek ini, dan tetap boleh dibangun dengan peringatan `Tertutup atap`.

**Mengambil air, `protected override bool OnInteract(in InteractContext ctx)`**:

1. `int portions = Mathf.FloorToInt(storedPortions)`. Kalau 0, tampilkan `Penampung kosong`, kembalikan `false`.
2. `int sisa = ctx.Inventory.AddItem(cleanWaterItem, portions)`.
3. `storedPortions -= portions - sisa`.
4. Kalau `sisa == portions`, tampilkan `Inventori penuh`, kembalikan `false`.
5. Kembalikan `true`.

`GetPrompt()` mengembalikan `Ambil Air (n)`, atau `Tertutup atap`, yang menjadi alasan `Locked`.

### Kelapa

Pohon kelapa berada di Region 1 dan Region 2. Buahnya memakai `PickupInteractable` (#17), dengan tumbuh kembali mengikuti `resource.coconutRespawnDays` di #73.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** tidak ada. Pengambilan item menerbitkan `InventoryChangedEvent` lewat `InventoryManager`.

**Event yang didengar:** `HourChangedEvent`, `VitalWarningChangedEvent`, `StructureBuiltEvent`, dan `StructureDestroyedEvent`.

| Sistem | Hubungan |
|---|---|
| `SurvivalStats`, `VitalsRules` (#111) | Pemilik `Thirst`, pengurangan, dan penalti |
| `ItemUseSystem` (#66) | Menjalankan pemakaian item minum |
| `DiseaseManager` (#45) | Menghitung `poisonChance` air kotor |
| `CampfireManager` (#44) | Merebus `Drink_DirtyWater` menjadi `Drink_CleanWater` |
| `RegionalWeather` (#115) | `GetWeather(RegionType)` |
| `RegionStreaming` (#88) | `GetRegionAt`, dan `structureRoot` nonaktif saat region dilepas |
| `BuildMenu` (#79), `BaseHealth` (#59) | Membangun penampung dan HP-nya |
| `IslandInteractable` (#17) | Kelas dasar dan `PickupInteractable` |
| `FarmingSystem` (#48) | Satu penyiraman memakai 1 `Drink_DirtyWater`, atau 1 `Drink_CleanWater` kalau air kotor habis |
| `ResourceRespawn` (#73) | Buah kelapa tumbuh kembali |
| `WorldStateSave` (#80) | Menyimpan isi penampung |
| `NotificationManager` (#22) | Peringatan |

**Batas pekerjaan:** isi efek sakit di #45, cuaca di #115, dan damage dehidrasi di #111.

## 4. Kasus Khusus

- **Objek pool:** tidak ada objek pool baru. `WorldItem` air yang terbuang memakai pool bersama (#66). Penampung adalah bangunan, dan bangunan tidak memakai pool (#59).
- **Game dijeda:** `HourChangedEvent` tidak terkirim, karena jam game berhenti. Penampung tidak bertambah.
- **Permadeath:** semua penampung terhapus bersama struktur dan save run (#82).
- **Save dan load:** `RainwaterCollector` menyimpan `storedPortions` lewat `ISaveable`, dengan `SaveId = StructureId`. `isExposedToSky` dihitung ulang. `WaterSourcePoint` tidak punya data.
- **Region dilepas saat hujan:** `TickHour` tetap berjalan untuk penampung nonaktif, karena `collectors` memegang referensinya. Tampilan air diperbarui saat region kembali.
- **Penampung hancur saat berisi:** isinya hilang. Air bukan barang di dunia.
- **Bangunan baru menutup penampung:** pengisian berhenti saat `StructureBuiltEvent`, dan isi yang sudah ada tetap bisa diambil.
- **Air kotor di tangan saat dehidrasi:** tetap boleh diminum, dengan risiko keracunan.
- **Minum saat haus penuh:** ditolak oleh `ItemUseSystem` langkah 2.
- **Fase 1:** `drainEnabled = false`. Air yang diambil di kapal baru terpakai di pulau.
- **Godmode (#56):** tidak memengaruhi penalti stamina.
- **Kelapa busuk:** `spoilHours` 96 dari #66 tetap berlaku.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** `Thirst` status inti (`GLOSSARY.md`). Angka haus dan lari (`GameBalanceConfig.md`). Nilai item minum (`ItemUseSystem.md`). Dehidrasi menghentikan regenerasi stamina (checklist issue ini, dicatat di `VitalsRules.md`).

**Keputusan teknis di blueprint ini:**

- Penampung ditick per jam game, dari `HydrationSystem`, termasuk yang nonaktif.
- Air tidak meninggalkan botol kosong dalam data.
- `WaterSourcePoint` memakai layer `Interactable`, sehingga layer `Water` tetap trigger.

**Usulan yang menunggu developer:**

1. **Tanpa botol kosong:** satu item air habis sekali minum. Pilihan lain adalah botol yang diisi ulang, yang menambah aturan slot dan wadah.
2. **Air laut:** tidak bisa diminum.
3. **Air kotor:** tersedia tanpa batas di sungai dan danau, dengan risiko keracunan 35 persen.
4. **Merebus air kotor** menjadi air bersih di api unggun (#44).
5. **Penampung:** 4 porsi penuh, 0,5 porsi per jam game saat hujan, 0,75 saat badai, tidak mengisi saat salju.
6. **Aktivitas lain:** berenang dan memanjat 1,25 kali, selain berlari yang sudah 1,5 kali.

## 6. Skenario Tes

1. Diam 100 detik, catat `Thirst`. **Hasil:** turun sekitar 15.
2. Lari 100 detik (stamina dibuat kebal lewat `setvital stamina 100` berkala). **Hasil:** turun sekitar 22,5.
3. Minum `Drink_CleanWater` saat `Thirst` 50. **Hasil:** naik 25 selama 20 detik, dan item berkurang 1.
4. `setvital thirst 10`. **Hasil:** regenerasi stamina setengah, dan notifikasi `Kamu haus`.
5. `setvital thirst 0`. **Hasil:** stamina tidak pulih, HP turun 0,5 per detik, dan notifikasi dehidrasi.
6. Minum saat `Thirst` 0. **Hasil:** stamina langsung mulai pulih, dengan regenerasi setengah sampai `Thirst` 25.
7. Tahan `E` 1 detik di `WaterSourcePoint`. **Hasil:** 1 `Drink_DirtyWater` masuk inventori.
8. Minum `Drink_DirtyWater` 20 kali pada aliran `Gameplay`. **Hasil:** sekitar 7 kali keracunan, dan `Thirst` naik 15 tiap kali.
9. Bangun penampung di ruang terbuka, lalu paksa `Rain` lewat `DevConsole`. **Hasil:** bertambah 0,5 porsi tiap jam game, dan tinggi air berubah.
10. Paksa `Storm`. **Hasil:** bertambah 0,75 porsi tiap jam game. Paksa `Snow` di Region 3. **Hasil:** tidak bertambah.
11. Bangun atap di atas penampung. **Hasil:** pengisian berhenti, dan prompt `Tertutup atap`.
12. Tekan `E` saat isi 2,5. **Hasil:** 2 `Drink_CleanWater` masuk, dan 0,5 tersisa.
13. Isi inventori sampai sisa ruang 1, lalu ambil 3 porsi. **Hasil:** 1 masuk, dan 2 porsi tetap di penampung.
14. Pergi ke region lain sampai region penampung dilepas selama hujan. Kembali. **Hasil:** isi tetap bertambah.
15. Simpan saat isi 3, keluar, lalu lanjutkan. **Hasil:** isi 3.
16. Hancurkan penampung lewat `DevConsole`. **Hasil:** terlepas dari daftar, dan tick berikutnya tanpa error.
17. Mati, lalu mulai run baru. **Hasil:** tidak ada penampung.
