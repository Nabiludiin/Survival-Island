# Blueprint: WarmthSystem.cs & TorchController.cs
**Lokasi:** `docs/blueprint/Island_Logic/WarmthSystem.md`
**Deskripsi Utama:** Pemilik nilai `Warmth` 0 sampai 100. Sistem ini menghitung panas dan dingin secara berkala, lalu memberi tahu `SurvivalStats` kapan pemain membeku.
**Issue:** #65
**Rujukan GDD:** Bagian 5 Region 3 (pakaian hangat atau obor, dan frostbite menguras stamina lalu HP). Nilai `Warmth`, hujan, air, dan durasi obor adalah usulan.

## 1. Arsitektur & Variabel Utama

| Kelas | Bentuk | Letak | Fungsi |
|---|---|---|---|
| `WarmthSystem` | Singleton, `ISaveable` (`player_warmth`) | Komponen pemain Fase 2 | Menghitung `Warmth` dan menyimpan pakaian yang dipakai |
| `TorchController` | `IEquippable`, `HandUse.OffHand` | Prefab `Tool_Torch` | Durasi bakar, cahaya, dan padam |

`Warmth` berada di `WarmthSystem`, bukan `SurvivalStats`. `SurvivalStats` hanya membaca `isFreezing`, sesuai `VitalsRules.md`.

**Variabel `WarmthSystem`:**

| Nama | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Warmth` | `float` | 100 | Nilai suhu tubuh |
| `IsFreezing` | `bool` | `false` | Dibaca `SleepSystem`, dan dikirim ke `SurvivalStats` |
| `currentRegion` | `RegionType` | `Volcano` | Dari `RegionChangedEvent` |
| `weatherByRegion` | `WeatherType[4]` | `Clear` | Dari `WeatherChangedEvent` |
| `wornClothingInstanceId` | `int` | -1 | Pakaian yang dipakai |
| `isTorchHeld` | `bool` | `false` | Obor menyala dan sedang dipegang |
| `isInWater` | `bool` | `false` | Dari `SwimStateChangedEvent` |
| `isIndoors` | `bool` | `false` | Hasil `RefreshShelter()` |
| `isInInterior` | `bool` | `false` | Dari `InteriorZoneChangedEvent` (#112) |

**Angka penyetelan (`GameBalanceConfig.warmth`, per detik kecuali disebut lain):**

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `tickSeconds` | 0,5 | Jeda pemeriksaan |
| `freezeEnterThreshold` | 20 | `IsFreezing` menjadi `true` |
| `freezeExitThreshold` | 30 | `IsFreezing` menjadi `false` |
| `ambientHeat` | 5 | Panas alami di luar Region 3 |
| `regionSnowCold` | 20 | Dingin Region 3, sekitar 4 detik sampai membeku |
| `blizzardCold` | 2 | Tambahan saat `Blizzard` |
| `rainCold` / `rainFloor` | 8 / 40 | Hujan, tidak bisa menurunkan `Warmth` di bawah 40 |
| `stormCold` / `stormFloor` | 12 / 30 | `Storm`, batas bawah 30 |
| `waterCold` / `waterFloor` | 15 / 30 | Air di luar Region 3 |
| `snowWaterCold` | 30 | Air di Region 3, tanpa batas bawah |
| `clothesHeat` | 20 | `Gear_WarmClothes` |
| `torchHeat` | 20 | Obor yang dipegang |
| `protectedFloor` | 30 | Usulan. Batas bawah `Warmth` selama `IsProtected()`. Sama dengan `freezeExitThreshold`, jadi pemain tidak membeku |
| `campfireHeat` | 40 | Dalam `campfire.warmthRadius` 5 meter |
| `indoorHeat` | 25 | Bangunan tertutup atau `InteriorZone` |
| `shelterCheckSeconds` | 2 | Jeda pemeriksaan bangunan |
| `shelterProbeDistance` | 6 | Jarak raycast dinding dan atap |
| `torchBurnSeconds` | 300 | Sekitar 8 jam game. Satu obor cukup untuk sekali jalan |
| `torchRainExtinguishSeconds` | 20 | Lama hujan sebelum obor padam |

**Sumber panas dan dingin:**

| Sumber | Jenis | Nilai | Syarat |
|---|---|---|---|
| Pakaian hangat | Panas | +20 | `wornClothingInstanceId != -1` |
| Obor | Panas | +20 | Menyala, dan berada di tangan kiri |
| Api unggun | Panas | +40 | Dalam radius salah satu `CampfireManager.ActiveLit` |
| Bangunan tertutup | Panas | +25 | `isIndoors` atau `isInInterior` |
| Region 3 | Dingin | -20 | `currentRegion == Snow` |
| Hujan | Dingin | -8 | Terkena langit, dengan batas bawah 40 |
| Badai | Dingin | -12 | Terkena langit, dengan batas bawah 30 |
| `Blizzard` | Dingin | -2 | Ditambah pada Region 3 (#115) |
| Air | Dingin | -15 atau -30 | `isInWater` (#13) |

Pakaian atau obor saja mencegah frostbite, sesuai GDD bagian 5. Selisih panas dan dingin bisa negatif, misalnya saat `Blizzard`, tetapi `protectedFloor` menahan `Warmth` di atas ambang beku, jadi pemain hanya kehilangan cadangan panas (usulan 8).

## 2. Detail Logika & Fungsi

### Pemeriksaan berkala

Ini menggantikan cek saat masuk zona di `ZoneManager`, sehingga memakai atau melepas perlindungan di dalam zona langsung berpengaruh.

**`private void Tick(float dt)`**, dipanggil setiap `tickSeconds` dari akumulator `Time.deltaTime`:

1. `heat = (currentRegion != Snow ? ambientHeat : 0)`, ditambah semua sumber panas aktif.
2. `cold = 0`, `floor = 0`. Mulai dengan `floor = 100` kalau ada sumber dingin berbatas.
3. Kalau `currentRegion == Snow`: `cold += regionSnowCold`, `floor = 0`. `Blizzard` menambah `blizzardCold`.
4. Di luar `Snow`: tambahkan hujan dan badai kalau `IsExposedToSky()`, dan pakai batas bawah terkecil.
5. Kalau `isInWater`: tambahkan `waterCold`, atau `snowWaterCold` dengan `floor = 0` di `Snow`.
6. `delta = (heat - cold) * dt`.
7. Kalau `delta < 0`: `Warmth = Max(Warmth + delta, Min(floor, Warmth))`. Kalau tidak, `Warmth = Min(100, Warmth + delta)`.
8. Kalau `IsProtected()`: `Warmth = Max(Warmth, protectedFloor)`.
9. Jalankan `UpdateFreezing()`.

**`private bool IsProtected()`:** `!isInWater`, dan `wornClothingInstanceId != -1` atau `isTorchHeld`.

**`UpdateFreezing()`:** `true` kalau `Warmth <= freezeEnterThreshold`, `false` kalau `Warmth >= freezeExitThreshold`, dan tetap di antaranya. Terbitkan `WarmthChangedEvent { warmth, isFreezing }` kalau `IsFreezing` berubah, atau nilai bulat `Warmth` berubah.

**Efek frostbite** mengikuti keputusan #85: `Stamina` terkuras dulu, lalu `HP`. `SurvivalStats` yang menghitungnya lewat `frostbiteStaminaDrainPerSec` 5 dan `frostbiteDamagePerSec` 1. `WarmthSystem` tidak mengurangi HP.

**Batas bawah menjaga GDD:** hujan dan air di region lain menurunkan `Warmth` tanpa pernah menyentuh 20, jadi frostbite hanya terjadi di Region 3.

**`public void ApplyColdBurst(float amount)`:** `Warmth = Max(IsProtected() ? protectedFloor : 0, Warmth - amount)`, lalu `UpdateFreezing()`. Dipakai Zombie Beku (#25) dengan `frozenBurstWarmthLoss` 25.

### Bangunan tertutup

**`private void RefreshShelter()`**, setiap `shelterCheckSeconds`:

1. `PhysicsQuery.Raycast` ke atas sejauh `shelterProbeDistance` dengan `PhysicsMasks.Ground`. Hasil harus berada di layer `Structure`.
2. `PhysicsQuery.Raycast` ke empat arah mendatar. Minimal tiga harus mengenai objek bertag `BaseWall` atau `BaseDoor`, sama dengan aturan tiga dinding di `BaseNavigation.md`.
3. `isIndoors` adalah gabungan keduanya.

**`private bool IsExposedToSky()`:** raycast ke atas dengan `PhysicsMasks.Sunlight` tidak mengenai apa pun, sama seperti `RainwaterCollector` (`HydrationSystem.md`).

### Pakaian hangat

- **Item:** `Gear_WarmClothes`, `ItemType.Tool`, dengan `isWarmClothing = true`.
- **Resep:** `rec_warm_clothes`, yaitu 4 `Mat_Hide` (#61) dan 2 `Mat_Rope`, selama 15 detik di `WoodBench` (`CraftingStation.md`).
- **Slot:** satu slot pakaian di luar hotbar dan di luar 15 slot inventori.

**`public bool WearClothing(int instanceId)`** dipanggil aksi `Pakai` di `ItemUseSystem` (#66). Keluarkan item dari slot inventori, simpan id-nya, dan lepas pakaian lama kalau ada.

**`public void RemoveClothing()`** mengembalikan item ke inventori lewat `AddItem`. Kalau penuh, item menjadi `WorldItem` (#66).

### Obor

- **Resep:** `rec_torch`, yaitu 1 `Mat_Wood` dan 1 `Mat_Cloth`, dibuat dengan tangan (`CraftingStation.md`).
- **Slot:** tangan kiri, lewat `EquipmentManager.EquipOffHand`. Tombol `F` memanggil `ToggleOffHand`.
- **Sisa bakar:** `ItemInstance.durability` menyimpan rasio sisa bakar. Obor tidak bisa diperbaiki (`ToolRepairSystem.md`).

**`TorchController.OnEquip()`:**

1. Kalau `CanLight()` gagal, tampilkan notifikasi `Terlalu basah` (#22), dan tetap padam.
2. Nyalakan `Light` dan partikel, pasang `LightSource` dengan `isHeld = true` (#98), lalu `IsLit = true`. Sumber ini tidak dihitung dalam tarikan cahaya.
3. `WarmthSystem.SetTorchHeld(true)`.

**`TorchController.OnUnequip()`:**

- Disimpan otomatis karena kapak atau Heavy Item: `SetTorchHeld(false)`, tetapi `IsLit` tetap `true`, dan durasi terus berjalan (`EquipmentManager.md`).
- Dimatikan lewat `F`: `IsLit = false`, dan durasi berhenti.

**`Update()`**, kalau `IsLit`:

1. `burnRemaining -= Time.deltaTime`. Kalau habis, `InventoryManager.RemoveInstance(instanceId)` dan kirim notifikasi.
2. `Storm` dan `Blizzard` tidak memadamkan obor. GDD bagian 5 menyebut membawa obor cukup untuk mencegah frostbite, dan developer memutuskan mengikutinya.
3. Kalau `Rain` dan terkena langit, tambah `rainTimer`. Kalau mencapai `torchRainExtinguishSeconds`, panggil `Extinguish()`. Di bawah atap, `rainTimer = 0`.

**`CanLight()`:** ditolak kalau `isInWater`, atau kalau `Rain` dan terkena langit. `Storm` dan `Blizzard` tidak menghalangi. Menyalakan ulang gratis selama durasi masih ada.

**Saat Heavy Item:** nilai `equipment.offHandAllowedWhileCarrying` tetap `false`. Obor disimpan otomatis saat barang diangkat, karena barang dipegang dua tangan (#69).

### Indikator HUD

`HUDManager` mendengar `WarmthChangedEvent`, lalu:

- Bar suhu tampil kalau `Warmth < 100` atau pemain di Region 3.
- Warna biru muda kalau `Warmth <= 50`, dan biru tua berkedip kalau `isFreezing`.
- `VitalWarningFlags.Freezing` dari `SurvivalStats` tetap memicu efek layar (#23).

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `WarmthChangedEvent`.

**Event yang didengar:** `RegionChangedEvent`, `WeatherChangedEvent`, `SwimStateChangedEvent` (baru, #13), `InteriorZoneChangedEvent` (#112), `RunEndedEvent`.

| Sistem | Hubungan |
|---|---|
| `SurvivalStats` (#111) | Mendengar `WarmthChangedEvent`, dan menghitung frostbite |
| `CampfireManager` (#44) | Daftar `ActiveLit` dan `warmthRadius` |
| `EquipmentManager` (#60) | Memegang obor di tangan kiri |
| `ItemUseSystem` (#66) | Aksi `Pakai` memanggil `WearClothing` |
| `CraftingStation` (#77) | Resep pakaian hangat dan obor |
| `RegionalWeather` (#115) | Menerbitkan cuaca tiap region |
| `SleepSystem` (#67) | Membaca `IsFreezing` |
| `SpecialZombieAI` (#25) | Memanggil `ApplyColdBurst` |
| `AISensory` (#49) | Boleh membaca `TorchController.IsLit`. Aturan deteksinya milik #49 |
| `LightSourceSystem` (#98) | Obor pegang memasang `LightSource` dengan `isHeld = true` |

## 4. Kasus Khusus

- **Objek pool:** tidak ada. Partikel api memakai `VFXManager` (#15), dan dikembalikan saat `OnUnequip` atau `OnDisable`.
- **Game dijeda:** `Time.deltaTime` berhenti, jadi `Warmth` dan durasi obor ikut berhenti.
- **Tidur:** `SleepSystem` menolak tidur saat `IsFreezing`. `Warmth` tetap memakai waktu nyata saat tidur. Pemain yang beratap dan berapi tidak turun.
- **Save:** `player_warmth` menyimpan `Warmth`, dan `wornClothingInstanceId` beserta `ItemInstance` pakaiannya. `IsFreezing` tidak disimpan. Setelah dimuat, `Tick` langsung berjalan dan mengirim `WarmthChangedEvent`. Sisa bakar obor mengikuti `ItemInstance` (#66), dan status menyala mengikuti `EquipmentSaveData.offHandActive`.
- **Permadeath:** `Warmth`, pakaian, dan obor terhapus bersama save run. Run baru mulai dari 100, tanpa pakaian.
- **Melepas pakaian di Region 3:** `Warmth` mulai turun pada tick berikutnya, paling lama 0,5 detik.
- **Region baru belum punya data cuaca:** `weatherByRegion` bernilai `Clear`, sampai `RegionalWeather` menerbitkan event.
- **`currentRegion` berubah saat di seam:** `RegionChangedEvent` sticky dibaca saat `OnEnable`, jadi pemain yang dimuat di Region 3 langsung terkena dingin.
- **Fase 1:** komponen ini tidak ada di `ShipPhase`.
- **Api unggun padam atau dilepas:** `CampfireManager` melepasnya dari `ActiveLit`, jadi tick berikutnya menghentikan panas.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:**

- Frostbite: `Stamina` dulu, lalu `HP` (`GLOSSARY.md`, GDD bagian 5). Poin "efek frostbite final" di checklist sudah terjawab.
- Tangan kiri untuk senter dan obor (`EquipmentManager.md`).
- Resep obor dan pakaian hangat (`CraftingStation.md`), yang menunggu konfirmasi di sini.

**Usulan yang disetujui developer sebagai nilai awal (dicatat di #85):**

1. **Bentuk `Warmth`:** rekomendasi nilai 0 sampai 100, dengan pemilik `WarmthSystem`. Ini mendukung `ApplyColdBurst` dan indikator HUD.
2. **Hujan dan air di luar Region 3:** rekomendasi hanya menurunkan `Warmth` sampai batas aman, jadi frostbite tetap khusus Region 3. Efek nyatanya adalah cadangan panas berkurang.
3. **Durasi obor dan padam:** bakar 300 detik, dan `Rain` memadamkan setelah 20 detik. **Keputusan developer:** `Storm` dan `Blizzard` tidak memadamkan obor, sesuai GDD bagian 5 ("membawa obor"). Region 3 tidak punya `Rain` (`RegionalWeather.md`), jadi obor selalu bisa melindungi pemain di sana selama durasinya masih ada. `CampfireManager` memakai aturan yang berbeda untuk hujan, karena api unggun lebih besar.
4. **Slot pakaian:** rekomendasi satu slot di luar inventori, tanpa durabilitas pada versi awal. Bahan kulit hewan sudah menjadi biayanya.
5. **Obor saat membawa Heavy Item:** rekomendasi tidak boleh. GDD menyebut barang berat, dan pemain jadi perlu pakaian hangat untuk perjalanan bolak-balik.
6. **Ambang `Warmth`:** rekomendasi membeku pada 20, sehingga stamina habis pada sekitar detik ke-24. Ini sedikit lebih lambat dari perhitungan `GameBalanceConfig.md`, yaitu 20 detik.
7. **Sumber angka dingin cuaca (#115, menunggu persetujuan):** saat ini `blizzardCold` (2) dan `GameBalanceConfig.weather.effects.warmthLossPerSec` memegang nilai yang sama. Usulan: hapus `blizzardCold`, lalu baca `RegionalWeather.Instance.GetEffects(currentRegion).warmthLossPerSec`. Akibatnya `Snow` ikut menambah dingin 0,5 per detik. Belum diterapkan, karena batasan #115 menyebut efek suhu milik #65.
8. **Batas bawah saat terlindung:** rekomendasi `protectedFloor` 30 untuk pakaian hangat atau obor menyala, sehingga `Blizzard` dan Zombie Beku tidak membekukan pemain yang terlindung. Pengecualian: air es Region 3 tetap bisa membekukan pemakai pakaian. Ini aturan tambahan yang menyimpang dari GDD bagian 5, dan perlu keputusan di #85.

## 6. Skenario Tes

1. Masuk Region 3 tanpa perlindungan. **Hasil:** `Warmth` mencapai 20 dalam sekitar 4 detik, stamina habis dalam sekitar 20 detik berikutnya, lalu HP turun 1 per detik.
2. Dalam kondisi itu, pakai pakaian hangat. **Hasil:** pada tick berikutnya `Warmth` naik ke 30, dan `IsFreezing` menjadi `false`. Pakaian tidak memulihkan lebih dari itu.
3. Nyalakan obor di Region 3 dari `Warmth` 100. **Hasil:** `Warmth` tidak turun selama obor menyala.
4. Lepas obor. **Hasil:** `Warmth` turun dalam 0,5 detik.
5. Berdiri dalam 5 meter dari api unggun di Region 3. **Hasil:** `Warmth` naik 20 per detik. Berjalan keluar radius. **Hasil:** turun lagi.
6. Masuk bangunan dengan atap dan empat dinding. **Hasil:** `isIndoors` aktif dalam 2 detik. Hancurkan atap. **Hasil:** tidak aktif.
7. Berdiri di hujan di Region 1 selama 60 detik. **Hasil:** `Warmth` berhenti di 40, tanpa frostbite.
8. Masuk air di Region 1, lalu di Region 3. **Hasil:** Region 1 berhenti di 30. Region 3 terus turun sampai membeku.
9. Paksa `Rain` saat obor menyala. **Hasil:** padam setelah 20 detik. Paksa `Storm`, lalu `Blizzard` di Region 3. **Hasil:** obor tetap menyala, dan tidak ada frostbite.
10. Biarkan obor menyala 300 detik. **Hasil:** item hilang, dan notifikasi muncul.
11. Angkat suku cadang saat memegang obor. **Hasil:** obor disimpan, panas obor hilang, dan durasi tetap berkurang.
12. Terkena ledakan Zombie Beku tanpa perlindungan. **Hasil:** `Warmth` turun 25 sekali. Ulangi dengan pakaian dan `Warmth` 40. **Hasil:** `Warmth` berhenti di 30, dan pemain tidak membeku.
13. Simpan dengan pakaian dan `Warmth` 60, lalu muat. **Hasil:** nilai sama. Mati dan mulai run baru. **Hasil:** `Warmth` 100, tanpa pakaian.
14. Pegang obor menyala. **Hasil:** `LightSource` terdaftar dengan `isHeld = true`, dan tidak menambah peluang tarikan zombie.
15. Paksa `Blizzard` di Region 3 dengan pakaian dari `Warmth` 100. **Hasil:** `Warmth` turun 2 per detik, berhenti di 30, dan tidak ada frostbite.
