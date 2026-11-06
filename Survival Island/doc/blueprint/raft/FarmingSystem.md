# Blueprint: FarmingSystem.cs & PlanterBox.cs
**Lokasi:** `Survival Island/doc/blueprint/raft/FarmingSystem.md`
**Deskripsi Utama:** Bercocok tanam di kotak tanah sebagai sumber makanan pasif di base. Tanaman melewati 3 fase berbasis jam `DayNightDirector`, dan setiap fase butuh air.
**Issue:** #48
**Rujukan GDD:** Tidak dipakai, diputuskan developer. Disimpan sebagai arsip. Sistem lain tidak boleh bergantung pada blueprint ini.

## 1. Arsitektur & Variabel Utama

- **`FarmingSystem`:** singleton di `IslandPhase`. Mendengar `HourChangedEvent`, lalu menjalankan `Simulate()` pada semua kotak. Tanpa state yang perlu disimpan.
- **`PlanterBox : IslandInteractable`:** komponen prefab `Build_PlanterBox`. Satu kotak berisi satu tanaman, dan datanya ikut `extraJson` struktur (`BuildMenu.md`).
- **`CropData`:** ScriptableObject per jenis tanaman.

```csharp
public enum CropPhase { Empty, Sprout, Growing, Harvestable }

[CreateAssetMenu(menuName = "Survival/Crop Data")]
public class CropData : ScriptableObject
{
    public string cropId;              // crop_potato
    public string nameKey;             // Kunci teks #104
    public ItemData seedItem;          // Seed_Potato
    public ItemData harvestItem;       // Food_Potato
    public int harvestAmount;          // 2
    public float seedReturnChance;     // 0,75
    public GameObject sproutVisual, growingVisual, harvestableVisual;
}
```

### Variabel `FarmingSystem`

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Instance` | `FarmingSystem` | `null` | Singleton per scene |
| `planters` | `List<PlanterBox>` | Kosong | Semua kotak, termasuk di region yang dilepas |
| `crops` | `CropData[]` | Dari Inspector | Urutan pemilihan bibit |

### Variabel `PlanterBox`

| Variabel | Tipe | Nilai awal | Disimpan | Fungsi |
|---|---|---|---|---|
| `crop` | `CropData` | `null` | `cropId` | Jenis tanaman |
| `phase` | `CropPhase` | `Empty` | Ya | Fase |
| `isWatered` | `bool` | `false` | Ya | Fase ini sudah disiram |
| `phaseHours` | `float` | 0 | Ya | Jam game yang sudah dijalani di fase ini |
| `lastTickGameHour` | `float` | Saat dibuat | Ya | Dasar selisih jam |
| `pendingHarvest` | `int` | 0 | Ya | Hasil yang belum masuk karena inventori penuh |
| `homeRegion` | `RegionType` | `RegionStreaming.GetRegionAt` | Ya | Region cuaca |
| `isExposedToSky` | `bool` | Dihitung | Tidak | Terkena hujan atau tidak |

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.farming.sproutHours` | 12 | Jam game dari `Sprout` ke `Growing` |
| `GameBalanceConfig.farming.growingHours` | 24 | Jam game dari `Growing` ke `Harvestable` |
| `GameBalanceConfig.farming.waterPerPhase` | 1 | Porsi air per fase |
| `GameBalanceConfig.farming.skyCheckDistance` | 30 | Jarak cek atap, sama dengan `RainwaterCollector` |
| `GameBalanceConfig.farming.structureRecheckRadius` | 8 | Radius cek ulang atap saat struktur berubah |
| `GameBalanceConfig.farming.snowRegionGrowthAllowed` | `false` | Tumbuh atau tidak di `RegionType.Snow` |

### Data item dan struktur baru

| Data | ID | Isi |
|---|---|---|
| `BuildableData` | `Build_PlanterBox` | Slot `Cell`, biaya 12 `Mat_Wood`, HP 100, dukungan `FloorOrGround`, Nav `Avoid`, tanpa tag |
| `ItemData` | `Seed_Potato` | `ItemType.Material`, tumpukan 10, tidak busuk |
| `ItemData` | `Food_Potato` | Hunger +10, busuk 72 jam game, tanpa `poisonChance` |
| `ItemData` | `Food_CookedPotato` | Hunger +25, busuk 48 jam game. Dari `cook_potato` di `CampfireManager.md` |

Prefab mengikuti `BuildMenu.md`: tubuh di layer `Structure`, dan anak ber-collider di layer `Interactable` untuk interaksi.

**Sumber bibit:** `RegionLootTable` (#103) dan kargo mingguan (#71). Panen juga mengembalikan bibit dengan peluang `seedReturnChance`.

**Cek keseimbangan:** satu kotak menghasilkan 2 `Food_CookedPotato` (50 Hunger) per 36 jam game, atau 33 Hunger per hari. Angka itu setara satu porsi per hari, atau sepertiga kebutuhan harian di `GameBalanceConfig.md`. Tiga kotak bisa menutup kebutuhan makan, tetapi butuh 4 porsi air per hari, setara kebutuhan minum pemain sendiri. Angka ini perlu dicek saat playtest.

## 2. Detail Logika & Fungsi

### Pendaftaran

Saat dibangun atau dipulihkan, `PlanterBox` memanggil `FarmingSystem.Instance.Register(this)`, dan `Unregister(this)` saat dihancurkan. Pendaftaran tidak memakai `OnEnable`, karena `structureRoot` dimatikan saat region dilepas (#88) sedangkan tanaman harus tetap tumbuh. Pola ini sama dengan `RainwaterCollector`.

### Interaksi tombol `E`

**`GetPrompt()`** dan **`CanInteract(ctx)`** mengikuti fase:

| Kondisi | Teks | Syarat |
|---|---|---|
| `Empty` | `Tanam <nama>` | Ada bibit. Kalau tidak: teks `Butuh bibit`, ditolak |
| `Sprout` atau `Growing`, `!isWatered` | `Siram` | Ada air. Kalau tidak: teks `Butuh air`, ditolak |
| `Sprout` atau `Growing`, `isWatered` | `Sedang tumbuh` | Ditolak, tanpa suara |
| `Harvestable` | `Panen <nama>` | Selalu boleh |

**`OnInteract(ctx)`** menjalankan salah satu fungsi di bawah.

**`private bool Plant(in InteractContext ctx)`**:

1. Cari `CropData` pertama di `FarmingSystem.crops` yang bibitnya ada di inventori.
2. `ctx.Inventory.RemoveItem(crop.seedItem.itemID, 1)`.
3. `phase = Sprout`, `isWatered = false`, `phaseHours = 0`, dan `lastTickGameHour = TotalGameHours`.
4. Panggil `UpdateVisual()`.

Kotak yang baru ditanam belum disiram, sehingga pemain langsung belajar aturan air.

**`private bool Water(in InteractContext ctx)`**:

1. Urutan pemakaian: `Drink_DirtyWater`, lalu `Drink_CleanWater`. Air bersih yang berharga dipakai terakhir.
2. `RemoveItem(id, waterPerPhase)`.
3. `isWatered = true`, mainkan suara dan efek, lalu `UpdateVisual()`.

**Arti "botol air":** `HydrationSystem.md` menetapkan satu item air habis sekali pakai dan tidak meninggalkan botol kosong. Jadi menyiram memakai satu item air dari inventori, tanpa item botol atau slot wadah baru.

**`private bool Harvest(in InteractContext ctx)`**:

1. Kalau `pendingHarvest == 0`, isi `pendingHarvest = crop.harvestAmount`, lalu undi bibit: `ref var rng = ref RunSeedManager.Instance.GetStream(RandomStreamId.Gameplay)`. Kalau `rng.NextFloat01() < crop.seedReturnChance`, tambahkan 1 bibit.
2. `int sisa = ctx.Inventory.AddItem(crop.harvestItem, pendingHarvest)`.
3. Kalau `sisa > 0`: `pendingHarvest = sisa`, tampilkan `Inventori penuh`, fase tetap `Harvestable`, dan selesai. Pemain bisa kembali lagi.
4. Kalau habis: terbitkan `CropHarvestedEvent`, lalu `ClearCrop()`, yang mengembalikan semua variabel ke nilai awal dan `phase = Empty`.

Kalau bibit hasil panen tidak muat, bibit dijatuhkan `WorldItemManager` sebagai `WorldItem` di depan kotak.

### Pertumbuhan

**`FarmingSystem.HandleHour(HourChangedEvent e)`:** panggil `planter.Simulate()` untuk semua `planters`.

**`PlanterBox.Simulate()`**:

```csharp
float now = DayNightDirector.Instance.TotalGameHours;
float elapsed = now - lastTickGameHour;
lastTickGameHour = now;
if (elapsed <= 0f || phase == CropPhase.Empty || phase == CropPhase.Harvestable) return;
if (homeRegion == RegionType.Snow && !b.snowRegionGrowthAllowed) return;

if (!isWatered && isExposedToSky) {
    WeatherType w = RegionalWeather.Instance.GetWeather(homeRegion);
    if (w == WeatherType.Rain || w == WeatherType.Storm) isWatered = true;
}
if (!isWatered) return;           // Tanpa air, tanaman menunggu, tidak mati

phaseHours += elapsed;
float need = phase == CropPhase.Sprout ? b.sproutHours : b.growingHours;
if (phaseHours >= need) AdvancePhase();
```

**`private void AdvancePhase()`**:

1. `Sprout` menjadi `Growing`, atau `Growing` menjadi `Harvestable`.
2. `phaseHours = 0`, `isWatered = false`, lalu `UpdateVisual()`.

Sisa jam tidak dibawa ke fase berikutnya, supaya setiap fase benar-benar butuh air baru. Dengan begitu, loncatan waktu besar lewat tidur atau `setday` hanya memajukan satu fase.

**Dua kali siram per siklus:** satu di awal `Sprout`, satu di awal `Growing`. `Harvestable` tidak butuh air.

**`UpdateVisual()`:** nyalakan satu dari tiga visual, sesuai fase. Tanah tampak gelap kalau `isWatered`, dan kering kalau tidak.

**`RefreshSkyExposure()`:** `PhysicsQuery.Raycast` ke atas sejauh `skyCheckDistance` dengan `PhysicsMasks.Sunlight`. `isExposedToSky` bernilai `true` kalau tidak mengenai apa pun. Dipanggil saat dibuat, dipulihkan, dan saat `StructureBuiltEvent` atau `StructureDestroyedEvent` terjadi dalam `structureRecheckRadius`. Kotak beratap tetap bisa ditanam, tetapi harus disiram manual.

## 3. Hubungan dengan Sistem Lain

**Event dikirim:** `CropHarvestedEvent`.
**Event didengar:** `HourChangedEvent`, `StructureBuiltEvent`, dan `StructureDestroyedEvent`. `PlanterBox` tidak berlangganan di `OnEnable`, karena objeknya bisa nonaktif saat region dilepas. Pendengaran dilakukan `FarmingSystem`, lalu diteruskan ke tiap kotak.

| Sistem | Hubungan |
|---|---|
| `DayNightDirector` | `TotalGameHours` dan sumber `HourChangedEvent` |
| `SleepSystem` (#67) | Tidur mempercepat jam, jadi tanaman ikut cepat tumbuh |
| `IslandInteractable` (#17) | Kelas dasar, tombol `E` |
| `InventoryManager` | Bibit, air, dan hasil panen |
| `HydrationSystem` (#43) | Sumber item air |
| `RegionalWeather` (#115) | Cuaca per `homeRegion` |
| `BuildManager`, `BuildMenu` (#79) | Membangun, aturan pantai (#86), dan `extraJson` |
| `BaseHealth` (#59) | HP kotak |
| `CampfireManager` (#44) | Resep `cook_potato` |
| `RunSeedManager` (#89) | Peluang bibit kembali |
| `RegionLootTable` (#103), `BeachSupplyManager` (#71) | Sumber bibit |

## 4. Kasus Khusus

- **Objek pool:** kotak adalah struktur, dan struktur tidak memakai pool (`BuildMenu.md`). Visual fase berupa anak prefab yang dinyalakan dan dimatikan.
- **Game dijeda:** `TotalGameHours` berhenti, dan `HourChangedEvent` tidak terkirim. Tanaman tidak tumbuh.
- **Region dilepas:** tanaman tetap tumbuh, karena `planters` memuat objek nonaktif. Cuaca dibaca dari `homeRegion`.
- **Kotak hancur:** tanaman, bibit, dan `pendingHarvest` hilang tanpa pengembalian. `Unregister` dipanggil.
- **Zombie:** kotak tidak bertag `BaseWall`, jadi bukan target `AttackBuilding` utama. Kotak tetap menerima damage lewat `BaseHealth` dari ledakan, bencana, dan api.
- **Pantai:** dilarang oleh aturan `BuildManager`, tanpa kode tambahan.
- **Save dan load:** `CaptureState` menulis variabel bertanda "Disimpan" ke `extraJson`. `RestoreState` hanya mengisi data dan tidak mengirim event. Setelah load, `Register`, `RefreshSkyExposure`, dan `UpdateVisual` dipanggil. Selisih jam dibaca dari `lastTickGameHour`, jadi waktu tetap benar.
- **`cropId` tidak dikenal saat load:** kotak menjadi `Empty`, dan kesalahannya dicatat.
- **Permadeath:** semua struktur dan isi inventori dihapus lewat `WorldStateSave` dan `RunLifecycle` (#82). Tidak ada data pertanian yang bertahan.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** air tidak meninggalkan botol kosong (`HydrationSystem.md`). Base dilarang di pasir (GDD bagian 4). `cook_potato` memakai `Food_Potato` (`CampfireManager.md`).

**Usulan yang menunggu developer:**

1. **Sistem ini dipakai atau tidak:** GDD belum menyebut bercocok tanam. Rekomendasi dipakai dengan hasil kecil.
2. **Jenis tanaman:** satu jenis pada versi awal, yaitu kentang, karena `CampfireManager.md` sudah menyiapkannya.
3. **Air kotor untuk menyiram:** boleh, dan didahulukan sebelum air bersih. Tanaman tidak punya risiko sakit.
4. **Tanpa air:** tanaman berhenti tumbuh, tetapi tidak mati. Pemain yang sibuk tidak kehilangan bibit.
5. **Hujan menyiram otomatis:** `Rain` dan `Storm` menyiram kotak yang tidak beratap.
6. **Region salju:** tanaman tidak tumbuh di `RegionType.Snow`, lewat `snowRegionGrowthAllowed = false`.
7. **Angka hasil:** waktu total 36 jam game dan hasil 2 kentang per siklus.

## 6. Skenario Tes

1. Bangun `Build_PlanterBox` di tanah. **Hasil:** berhasil, prompt `Butuh bibit`.
2. Coba bangun di pasir. **Hasil:** hologram merah.
3. Tanam `Seed_Potato`. **Hasil:** bibit berkurang 1, visual `Sprout`, dan prompt `Siram`.
4. Biarkan 24 jam game tanpa air dalam cuaca `Clear`. **Hasil:** tetap `Sprout`.
5. Siram dengan `Drink_DirtyWater`. **Hasil:** item berkurang 1, dan tanah tampak gelap.
6. Lewati 12 jam game. **Hasil:** fase menjadi `Growing`, dan prompt `Siram` kembali.
7. Siram, lewati 24 jam game. **Hasil:** fase `Harvestable`.
8. Panen dengan inventori kosong. **Hasil:** 2 `Food_Potato` masuk, kotak kosong, dan `CropHarvestedEvent` terkirim.
9. Panen 20 kali dengan seed yang sama. **Hasil:** sekitar 15 bibit kembali, dan nilainya sama pada urutan aksi yang sama.
10. Panen dengan inventori penuh, kosongkan, lalu panen lagi. **Hasil:** hasil pertama tertahan di `pendingHarvest`, dan tidak berlipat ganda.
11. Paksa `Rain` pada kotak tanpa atap yang belum disiram. **Hasil:** `isWatered` menjadi `true` pada jam berikutnya.
12. Ulangi dengan atap di atas kotak. **Hasil:** `isWatered` tetap `false`.
13. Bongkar atap saat hujan. **Hasil:** `isExposedToSky` berubah, dan siram otomatis berjalan.
14. Tanam lalu tinggalkan region sejauh yang melepas scene-nya, dengan kotak sudah disiram. Kembali setelah 12 jam game. **Hasil:** fase sudah `Growing`.
15. Tidur 8 jam game. **Hasil:** `phaseHours` bertambah 8.
16. Panggil `setday` melompat 3 hari pada `Sprout` yang sudah disiram. **Hasil:** hanya maju ke `Growing`.
17. Jeda game 60 detik. **Hasil:** `phaseHours` tidak berubah.
18. Save saat `Growing` dengan `phaseHours` 5, lalu load. **Hasil:** fase, nilai, dan visual sama.
19. Hancurkan kotak dengan ledakan. **Hasil:** tanaman hilang, dan `planters` tidak lagi memuatnya.
20. Mati, lalu mulai run baru. **Hasil:** tidak ada kotak, dan `planters` kosong.
21. Tanam di Region 3. **Hasil:** walau disiram, `phaseHours` tetap 0.
