# Blueprint: LightSourceSystem.cs
**Lokasi:** `docs/blueprint/Island_Logic/LightSourceSystem.md`
**Deskripsi Utama:** Sumber cahaya tetap di base (Obor Dinding dan Lampu), pengisian baterai senter, penghitungan keterlihatan pemain yang ringan, pengaruh cahaya pada zombie, dan batas lampu real-time.
**Issue:** #98
**Rujukan GDD:** Bagian 3 (malam memicu horde dan serangan base), bagian 4 (fungsi base), dan bagian 5 (Region 2 gelap meski siang, Zombie Lumut). Struktur lampu, baterai, dan tarikan cahaya adalah usulan, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

| Kelas | Jenis | Tugas |
|---|---|---|
| `LightSourceSystem` | Manager di `IslandPhase` | Daftar sumber cahaya, keterlihatan pemain, batas lampu, dan tarikan zombie |
| `LightSource` | Komponen | Satu pemancar cahaya. Dipakai struktur, api unggun, obor, senter, dan api |
| `LightFixture` | Turunan `IslandInteractable`, `ISaveable` | Bahan bakar dan interaksi Obor Dinding serta Lampu |
| `LightFixtureData` | ScriptableObject | Data bahan bakar tiap struktur |

**Bukan milik sistem ini:** logika senter (`FlashlightController`), api unggun (#44), dan obor pegang (#65). Ketiganya hanya memasang `LightSource`.

### Struktur baru

| ID | Nama | Slot | Biaya | HP | Bahan bakar | Isi per item | Kapasitas | Jangkauan | `intensity01` |
|---|---|---|---|---|---|---|---|---|---|
| `Build_WallTorch` | Obor Dinding | `WallMount` | 2 `Mat_Wood`, 1 `Mat_Cloth` | 50 | `Mat_Wood` | 300 detik setara | 900 | 8 m | 0,7 |
| `Build_Lamp` | Lampu | `Cell`, `FloorOrGround` | 4 `Mat_Wood`, 2 `Mat_Scrap` | 80 | `Item_Battery` | 1.200 detik setara | 1.200 | 14 m | 1 |

Keduanya berkategori `Furniture`, `navMode = None` untuk Obor Dinding dan `Avoid` untuk Lampu. Keduanya memasang `Flammable` dengan profil `Flame_WoodStructure` (#97). Satu malam berlangsung sekitar 450 detik, jadi Obor Dinding butuh 2 kayu per malam, dan Lampu butuh 1 baterai untuk sekitar 2,5 malam.

### Item baru

`Item_Battery`: `ItemType.Consumable`, `isConsumable = true`, `maxStack` 5. Sumber: loot (#103), kargo (#71), dan loot kapal. Baterai tidak bisa dirakit.

### Variabel `LightSource`

`intensity01`, `range`, `IsLit`, `isHeld` (ikut pemain), `realLight` (`Light`, boleh null), `emissive` (`Renderer` untuk tampilan tanpa lampu), dan `lastScore`.

### Variabel `LightSourceSystem`

| Variabel | Nilai awal | Fungsi |
|---|---|---|
| `sources` | List kapasitas 64 | Semua sumber aktif |
| `PlayerLightLevel` | 0 | Tingkat cahaya di posisi pemain, 0 sampai 1 |
| `PlayerVisibility` | 1 | Nilai yang dikirim ke `AISensoryHub` |

### Angka penyetelan (bagian config baru `GameBalanceConfig.light`)

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `light.visibilityTickSeconds` | 0,25 | Jeda hitung keterlihatan |
| `light.budgetTickSeconds` | 0,5 | Jeda pilih lampu real-time |
| `light.fixtureTickSeconds` | 1 | Jeda kurangi bahan bakar |
| `light.ambientDay` / `ambientNight` | 1 / 0,1 | Cahaya alam |
| `light.ambientAmazonDay` | 0,45 | Siang di Region 2 |
| `light.ambientInterior` | 0,1 | Dalam `InteriorZone` (#112) |
| `light.interiorMaxRealtimeLights` | 3 | Usulan. Batas lampu real-time dalam `InteriorZone` (#112) |
| `light.stormAmbientMultiplier` | 0,7 | Pengali saat `Storm` dan `Blizzard` |
| `light.visibilityInDark` / `visibilityInLight` | 0,6 / 1 | Pengali `externalVisibility` |
| `light.flashlightVisibilityBonus` | 0,3 | Tambahan saat senter menyala |
| `light.maxRegisteredSources` | 64 | Batas sumber terdaftar |
| `light.maxRealtimeLights` | 6 | Batas `Light` aktif |
| `light.maxShadowLights` | 1 | Batas `Light` berbayang |
| `light.budgetSwapMargin` | 1,2 | Lampu baru harus 20 persen lebih penting untuk menggantikan |
| `light.maxLightStructures` | 24 | Batas Obor Dinding dan Lampu |
| `light.rainFuelMultiplier` | 1,5 | Konsumsi Obor Dinding tak beratap saat `Rain` |
| `light.stormExtinguishes` | `true` | `Storm` dan `Blizzard` mematikan lampu tak beratap |
| `light.flickerSeconds` | 2 | Kedip sebelum padam |
| `light.attractionChancePerLight` / `attractionChanceMax` | 0,05 / 0,35 | Tarikan zombie |
| `light.mossStaticLightLevel` | 0,5 | Cahaya tetap yang membangunkan Zombie Lumut |
| `flashlight.maxBattery` / `drainPerSec` | 100 / 0,2 | Senter menyala 500 detik, sekitar satu malam |
| `flashlight.batteryRecharge` | 100 | Isi per `Item_Battery` |

## 2. Detail Logika & Fungsi

### Interaksi `LightFixture`

`OnInteract` dengan tombol `E`:

| Kondisi | Hasil |
|---|---|
| Ada bahan bakar, mati | Nyalakan. Ditolak dengan `Terlalu berangin` saat `Storm` atau `Blizzard` tak beratap |
| Menyala | Matikan. Bahan bakar tetap tersimpan |
| Tanpa bahan bakar, pemain punya bahan | Kurangi 1 item, isi `fuelSeconds`, lalu nyalakan |
| Tanpa bahan bakar, pemain tidak punya | Prompt `Butuh Kayu` atau `Butuh Baterai` |

Tahan `E` selama 0,6 detik menambah 1 bahan bakar kalau belum melewati kapasitas. Baterai yang masih terisi tidak dikembalikan.

### Waktu dan bahan bakar

Setiap `fixtureTickSeconds`, tiap `LightFixture` menyala menghitung `sim` dengan rumus `CampfireManager`, yaitu `(jamGame - lastTickGameHour) * 3600 / timeMultiplier`, supaya tidur ikut menghabiskan bahan bakar.

1. Baca `RegionalWeather.Instance.GetWeather(homeRegion)`.
2. Kalau tak beratap dan `Storm` atau `Blizzard`: `SetLit(false)`. Bahan bakar tetap. Lampu nyala lagi otomatis saat cuaca membaik, sedangkan Obor Dinding harus dinyalakan pemain dengan `E`.
3. Kurangi `fuelSeconds` dengan `sim`. Obor Dinding tak beratap saat `Rain` dikali `rainFuelMultiplier`. Lampu tidak terpengaruh `Rain`.
4. Kalau sisa bahan bakar di bawah `flickerSeconds`, kedipkan cahaya. Saat habis, `SetLit(false)` dan kirim `LightSourceToggledEvent`.

Atap dibaca lewat `PhysicsQuery.Raycast` ke atas dengan `PhysicsMasks.Sunlight`, dengan aturan cek ulang yang sama seperti `CampfireManager`.

### Baterai senter

`Item_Battery` dipakai lewat aksi `Pakai` di inventori (#66):

1. Cari `Tool_Flashlight` yang sedang dipegang. Kalau tidak ada, cari di inventori dengan isi terendah. Kalau tidak ada, tolak dengan `Tidak ada senter`.
2. Tolak kalau isinya sudah penuh, supaya baterai tidak terbuang.
3. `FlashlightController.Recharge(batteryRecharge)`, lalu kurangi 1 item.

Isi baterai disimpan di `ItemInstance.durability`, jadi tiap senter punya isi sendiri, dan ikut tersimpan bersama inventori. Senter tidak bisa diperbaiki lewat #113.

### Keterlihatan pemain

**`private void TickVisibility()`**, setiap `visibilityTickSeconds`, tanpa raycast:

```
ambient = ambientDay atau ambientNight (dari DaybreakEvent dan NightfallEvent)
kalau Region 2 dan siang: ambient = ambientAmazonDay
kalau InteriorZone.IsPlayerInside: ambient = ambientInterior
kalau Storm atau Blizzard: ambient *= stormAmbientMultiplier
tambahan = jumlah GetContribution(sumber, posisiPemain), untuk sumber yang menyala
kalau InteriorZone.IsPlayerInside: tambahan hanya dari sumber isHeld, yaitu senter dan obor
PlayerLightLevel = Clamp01(ambient + tambahan)
visibility = Lerp(visibilityInDark, visibilityInLight, PlayerLightLevel)
kalau senter menyala: visibility += flashlightVisibilityBonus
```

`GetContribution = intensity01 * (1 - jarak / range)`, atau 0 di luar `range`. Jarak memakai `sqrMagnitude` lebih dulu, supaya akar hanya dihitung untuk sumber yang terjangkau. Tembok tidak menghalangi hitungan ini. Dengan 64 sumber pada 4 kali per detik, biayanya bisa diabaikan.

Kalau `visibility` berubah lebih dari 0,02, panggil `AISensoryHub.Instance.SetVisibilityModifier(visibility)` dan terbitkan `PlayerLightLevelChangedEvent { lightLevel01, visibility }`.

**Contoh hasil:**

| Kondisi | `PlayerVisibility` |
|---|---|
| Siang | 1 |
| Malam, gelap total | 0,64 |
| Malam, gelap, senter menyala | 0,94 |
| Malam, 2 meter dari Lampu | Sekitar 0,98 |

**`public float GetLightLevelAt(Vector3 pos)`** menjumlahkan kontribusi sumber di posisi tersebut, tanpa ambient dan tanpa query fisika. Senter tidak dihitung di fungsi ini.

### Hubungan ke `AISensory` dan Zombie Lumut

- `AISensory` tetap memakai `hub.PlayerVisibility`. Nilai dari sistem ini mengisi `externalVisibility`, dan mengecilkan jarak serta laju deteksi saat gelap.
- `BloodMoonStartedEvent` tetap mengabaikan stealth lewat `IgnoreStealth`, tanpa perubahan.
- **Zombie Lumut:** `MossBehavior` menambah satu baris pada tabel `rage`: kalau `LightSourceSystem.Instance.GetLightLevelAt(pos) >= mossStaticLightLevel`, `rage += mossRagePerSec * dt`. Sorotan senter tetap memakai aturan `mossLightDistance` di #25, yang membaca `FlashlightController.IsPointInBeam(pos)`.
- Lampu menyala pada jarak 7 meter sudah mencapai 0,5, sedangkan obor pegang baru mencapainya pada jarak dekat. Memasang lampu di hutan bisa membangunkan Lumut dengan sengaja.

### Tarikan cahaya base

Setiap `NightfallEvent` dan saat `LightSourceToggledEvent` terkirim, hitung ulang:

1. `count` = jumlah `LightFixture` menyala. Kalau 0, `AISensoryHub.Instance.SetLightAttraction(Vector3.zero, 0)`.
2. Titik tarikan = rata-rata posisi `LightFixture` menyala.
3. `chance = Min(count * attractionChancePerLight, attractionChanceMax)`.
4. `AISensoryHub.Instance.SetLightAttraction(titik, chance)`.

Saat memperbarui `HuntHint`, hub melempar `chance` dengan aliran `Gameplay` (#89). Kalau kena, `HuntHint` memakai titik tarikan, bukan posisi pemain. Zombie `Unaware` berjalan ke `HuntHint`, jadi hanya zombie yang belum mengunci pemain yang terpengaruh. Pemain yang memasang banyak lampu memperoleh rasa aman, tetapi base lebih sering didatangi saat ia pergi. Api unggun dan obor pegang tidak dihitung.

### Batas lampu real-time

**`private void TickBudget()`**, setiap `budgetTickSeconds`:

1. Hitung skor tiap sumber menyala: `intensity01 * range / (1 + jarak ke kamera)`. Senter dan obor pegang selalu menang.
2. Pilih paling banyak `maxRealtimeLights` sumber dengan skor tertinggi. Sumber aktif hanya diganti kalau pesaing lebih tinggi sebesar `budgetSwapMargin`, supaya tidak berkedip.
3. Sumber terpilih: `realLight.enabled = true`. Lainnya: `realLight.enabled = false`, dengan `emissive` tetap menyala. Peralihan memudar selama 0,25 detik.
4. Hanya sumber terdekat, di luar senter, yang memakai bayangan, sesuai `maxShadowLights`. Sisanya `LightShadows.None`.
5. Sumber pada `BurnEffect` dan `FireArea` (#97) ikut bersaing dengan skor yang sama.
6. Kalau `InteriorZone.IsPlayerInside`, batas lampu real-time memakai `interiorMaxRealtimeLights`, dengan senter dan obor pegang tetap menang (#112).

Sumber di atas `maxRegisteredSources` tetap tampil lewat `emissive`, tetapi tidak dihitung. `BuildManager` menolak struktur cahaya baru di atas `maxLightStructures`, dengan pesan `Terlalu banyak lampu`. `ApplyQuality(int lights, int shadows)` dipanggil #19 untuk mengubah batas sesuai setelan grafis.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `PlayerLightLevelChangedEvent` (`lightLevel01`, `visibility`, baru, sticky) dan `LightSourceToggledEvent` (`structureId`, `isLit`, baru, tidak sticky).

**Event yang didengar:** `NightfallEvent`, `DaybreakEvent`, `RegionChangedEvent`, `WeatherChangedEvent`, `RunEndedEvent`, dan `StructureDestroyedEvent`.

| Sistem | Hubungan |
|---|---|
| `AISensory` (#49) | `SetVisibilityModifier` dan `SetLightAttraction` |
| `SpecialZombieAI` (#25) | `GetLightLevelAt` dan `IsPointInBeam` |
| `FlashlightController` | `Recharge`, `IsPointInBeam`, dan `LightSource` |
| `EquipmentManager` (#60), `ItemUseSystem` (#66) | Senter di tangan kiri dan aksi `Pakai` baterai |
| `BuildMenu` (#79) | `BuildableData`, slot `WallMount` |
| `CampfireManager` (#44), `WarmthSystem` (#65), `ThrowableSystem` (#96), `FireSystem` (#97) | Memasang `LightSource` |
| `RegionalWeather` (#115) | `GetWeather(region)` |
| `ZombieLair` (#112) | `InteriorZone.IsPlayerInside` |
| `WorldStateSave` (#80) | `LightFixture` menyimpan data lewat `ISaveable` |

## 4. Kasus Khusus

- **Dipakai ulang dari pool:** `LightSource` pada objek pool mendaftar di `OnSpawned`, dan keluar di `OnReturned`. `IsLit`, `intensity01`, dan `realLight.enabled` kembali ke nilai prefab. `LightFixture` tidak memakai pool.
- **Game dijeda:** tick dilewati saat `Time.deltaTime == 0`. Jam game tidak maju, jadi bahan bakar tidak berkurang.
- **Tidur:** bahan bakar ikut berkurang lewat `sim`. Pemain bisa bangun dengan lampu sudah padam.
- **Struktur hancur:** `StructureDestroyedEvent` melepas `LightSource`. Bahan bakar hilang. Kalau dinding hancur, Obor Dinding ikut hancur lewat aturan penopang #79.
- **Terbakar (#97):** Obor Dinding tidak menyalakan dindingnya, tetapi bisa ikut terbakar dari api lain.
- **Region dilepas (#88):** `OnDisable` melepas `LightSource`, dan `RemoteBaseSimulation` (#90) tidak menghitung lampu. Saat kembali, `sim` mengejar waktu yang lewat lewat `lastTickGameHour`.
- **Data lama senter:** `currentBattery` pada `FlashlightController` diganti `ItemInstance.durability`.
- **Save:** `LightFixture` memakai `SaveId = StructureId`, dan menyimpan `fuelSeconds`, `IsLit`, serta `lastTickGameHour`. Nilai keterlihatan dan pilihan lampu tidak disimpan, dan dihitung ulang setelah `OnRestoreFinished`.
- **Permadeath:** semua struktur cahaya hilang bersama `world_structures`, dan isi senter hilang bersama inventori. `SetVisibilityModifier(1)` dan `SetLightAttraction(Vector3.zero, 0)` dipanggil saat `RunEndedEvent`.

## 5. Keputusan Desain

**Keputusan yang mengikuti blueprint lain:** titik sambung `SetVisibilityModifier` (#49), tombol `E` (#85), serta aturan cuaca dan atap mengikuti `CampfireManager`.

**Usulan yang menunggu developer:**

1. **Cahaya base menarik zombie: ya, secara ringan.** Hanya zombie `Unaware` yang terpengaruh, dengan peluang maksimum 0,35. GDD sudah menyebut zombie memburu MC dan menyerang base, jadi aturan ini memberi pilihan terang atau aman tanpa mengubah aturan intinya.
2. **Cara mengisi senter: baterai temuan, tanpa generator.** Generator menambah struktur, bahan bakar, dan suara baru, sedangkan issue ini berprioritas rendah. Generator bisa ditambah kemudian.
3. **Bahan bakar:** Obor Dinding memakai `Mat_Wood`, dan Lampu memakai `Item_Battery`, supaya baterai punya dua kegunaan yang saling bersaing.
4. **Badai:** lampu tak beratap padam. Lampu menyala kembali otomatis, dan Obor Dinding harus dinyalakan lagi.
5. **Kuras senter 0,2 per detik:** menggantikan `drainRate = 1` yang habis dalam 100 detik, atau kurang dari seperempat malam.
6. **Slot baru `WallMount`:** Obor Dinding menempel di sisi dinding yang sudah ada.
7. **Semua angka di tabel config:** perlu disetel lewat playtest.

## 6. Skenario Tes

1. Bangun Obor Dinding pada dinding kayu, lalu isi 1 kayu dan nyalakan dengan `E`. **Hasil:** menyala, dan padam setelah 300 detik dengan kedip 2 detik.
2. Bangun Obor Dinding tanpa dinding. **Hasil:** ditolak.
3. Hancurkan dindingnya. **Hasil:** obor ikut hancur, dan sumber keluar dari daftar.
4. Bangun Lampu tanpa baterai, lalu tekan `E`. **Hasil:** prompt `Butuh Baterai`.
5. Pakai `Item_Battery` saat senter berisi 20. **Hasil:** isi menjadi 100, dan item berkurang. Saat senter penuh, ditolak.
6. Berdiri di malam gelap dan baca `PlayerVisibility` lewat `DevConsole`. **Hasil:** sekitar 0,64. Nyalakan senter. **Hasil:** sekitar 0,94.
7. Dekati Lampu. **Hasil:** nilai naik, dan zombie mendeteksi pemain dari jarak lebih jauh.
8. Jalan jongkok tanpa senter melewati zombie `Unaware` di malam gelap, lalu ulangi di dekat Lampu. **Hasil:** jarak deteksi kedua lebih jauh.
9. Taruh Lampu 5 meter dari Zombie Lumut. **Hasil:** `rage` naik dan zombie bangun dalam sekitar 2,5 detik. Pada 12 meter, tidak bangun.
10. Nyalakan 7 lampu dan pergi dari base saat malam. Catat `HuntHint`. **Hasil:** sekitar 35 persen pembaruan menunjuk ke base. Matikan semua lampu. **Hasil:** 0 persen.
11. Pasang 24 lampu dalam pandangan. **Hasil:** `Light` aktif paling banyak 6, dan berbayang 1. Sisanya tampil lewat `emissive`, tanpa berkedip saat kamera berputar.
12. Paksa `Storm` pada lampu tak beratap. **Hasil:** padam. Saat `Clear`, Lampu menyala lagi dan Obor Dinding tetap mati. Lampu beratap tidak berubah.
13. Paksa `Rain`. **Hasil:** Obor Dinding tak beratap boros 1,5 kali, dan Lampu normal.
14. Tidur dengan obor menyala. **Hasil:** bahan bakar berkurang sesuai jam yang dilewati.
15. Save, load, lalu mati dan mulai run baru. **Hasil:** setelah load, sisa bahan bakar sama. Pada run baru, tidak ada lampu tersisa, dan `PlayerVisibility` kembali mengikuti kondisi awal.
16. Tekan jeda. **Hasil:** bahan bakar dan isi senter tidak berkurang.
17. Masuk `InteriorZone` dengan lampu base dekat dinding zona. **Hasil:** keterlihatan hanya naik dari senter atau obor, dan lampu real-time tidak melewati `interiorMaxRealtimeLights`.
