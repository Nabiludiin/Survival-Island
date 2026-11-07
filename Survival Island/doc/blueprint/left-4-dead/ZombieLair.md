# Blueprint: ZombieLair.cs & InteriorZone.cs
**Lokasi:** `Survival Island/doc/blueprint/left-4-dead/ZombieLair.md`
**Deskripsi Utama:** Tempat persembunyian zombie dan area tertutup yang gelap, sehingga menjelajah saat siang menjadi lebih berisiko, walaupun zombie tetap terlindung dari sinar matahari.
**Issue:** #112
**Rujukan GDD:** Bagian 3 (tidak ada zombie yang berkeliaran di luar ruangan saat siang, sebagian zombie bersembunyi diam di ruang gelap seperti gua, bunker, dan gudang lalu bangun kalau terganggu, dan horde keluar dari persembunyiannya saat malam).

## 1. Arsitektur & Variabel Utama

| Kelas | Letak | Fungsi |
|---|---|---|
| `InteriorZone` | Objek di scene region, layer `ZoneTrigger`, `BoxCollider` trigger | Batas area tertutup, status gelap, dan penanda bagi sistem lain |
| `ZombieLair` | Komponen di objek yang sama, `ISaveable` | Mengatur zombie, titik tidur, loot, dan status bersih |
| `LairNest` | Anak `ZombieLair`, `IDamageable` | Sarang yang bisa dihancurkan |
| `LairRegistry` | Manager di `IslandPhase` | Daftar sarang dan zona aktif, untuk pencarian tanpa alokasi |

```csharp
[CreateAssetMenu(menuName = "Survival/Lair Data")]
public class LairData : ScriptableObject
{
    public string lairId;                // Unik per lokasi
    public RegionType region;
    public LairKind kind;                // Cave, MilitaryBunker, VillageWarehouse, RuinedHouse
    public EnemyData[] occupants;        // Varian yang boleh muncul
    public int dormantCount;
    public LootTable lootTable;          // #103
    public bool hasNest;
    public float nestHP;
}
```

**Variabel `InteriorZone`:** `zoneId`, `isDark` (`true`), `exitPoints` (`Transform[]`), `lightProfile` (`RegionEnvironmentProfile`), dan `IsPlayerInside`.

**Variabel `ZombieLair`:** `data`, `spawnPoints` (`Transform[]`), `activeZombies` (`List<EnemyAI>`, kapasitas tetap), `remainingCount`, `isCleared`, `isNestDestroyed`, `isSpawned`, dan `lootCollected`.

| Angka | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.lair.enabled` | `true` | Sistem dipakai. `false` hanya sakelar uji |
| `GameBalanceConfig.lair.activationRadius` | 40 | Jarak pemain sebelum zombie dimunculkan |
| `GameBalanceConfig.lair.deactivationRadius` | 60 | Jarak pemain sebelum zombie kembali ke pool |
| `GameBalanceConfig.lair.senseIntervalSeconds` | 0,5 | Jeda pemeriksaan Dormant |
| `GameBalanceConfig.lair.proximityWakeRadius` | 3 | Jarak bangun saat berdiri |
| `GameBalanceConfig.lair.crouchProximityWakeRadius` | 1,5 | Jarak bangun saat jongkok |
| `GameBalanceConfig.lair.lightWakeRange` | 8 | Jarak senter yang bisa membangunkan |
| `GameBalanceConfig.lair.lightWakeSeconds` | 2 | Lama disorot sebelum bangun |
| `GameBalanceConfig.lair.noiseWakeMultiplier` | 1 | Pengali radius suara yang membangunkan |
| `GameBalanceConfig.lair.groupWakeRadius` | 6 | Zombie sekitar ikut bangun |
| `GameBalanceConfig.lair.doorWaitSeconds` | 6 | Lama menunggu di pintu keluar saat siang |
| `GameBalanceConfig.lair.returnToDormantSeconds` | 20 | Lama kehilangan target sebelum tidur lagi |
| `GameBalanceConfig.lair.clearedBudgetReduction` | 0,1 | Pengurangan horde region per sarang yang hancur |
| `GameBalanceConfig.lair.maxBudgetReduction` | 0,3 | Batas pengurangan horde per region |
| `GameBalanceConfig.lair.nightReleaseRatio` | 0,5 | Bagian zombie sarang yang ikut horde saat malam |

### Lokasi

| Lokasi | Region | Isi (`occupants`) | Sarang | Loot |
|---|---|---|---|---|
| Gua kaki gunung | 1, `Volcano` | Zombie standar, 4 sampai 6 | Ya | Batu dan bahan langka |
| Gua akar | 2, `Amazon` | Zombie Tribal, Zombie Lumut, dan standar, 6 sampai 8 | Ya | Getah dan jamur untuk vaksin |
| Bunker militer | 3, `Snow` | Zombie Militer, Zombie Beku, dan standar, 8 sampai 12 | Ya | Peluru dan perlengkapan militer |
| Gudang desa | 4, `Logistic` | Zombie standar, 4 sampai 6 | Tidak | Bahan, `Mat_Scrap`, dan makanan kaleng |
| Rumah runtuh | 4, `Logistic` | Zombie standar, 2 sampai 3 | Tidak | Loot kecil dan lore (#100) |

Zombie Desa tidak memakai sarang ini. GDD bagian 5 menetapkan mereka bersembunyi di air atau lumpur, dan rinciannya ada di #63.

## 2. Detail Logika & Fungsi

### Masuk dan keluar zona

**`OnTriggerEnter` dan `OnTriggerExit`** di `InteriorZone` memeriksa tag `Player`. Saat berubah:

1. Isi `IsPlayerInside`.
2. Terbitkan `InteriorZoneChangedEvent { zoneId, isInside, isDark }`.

`EnvironmentDirector` (#92) mengganti profil cahaya dan kabut, dan `RegionalWeather` (#115) meredam suara cuaca. Tiap `InteriorZone` juga membawa `NoBuildVolume`, sehingga `BuildManager` menolak bangunan di dalamnya.

**`public static bool Contains(Vector3 pos)`** memeriksa semua `InteriorZone` yang aktif, memakai `collider.bounds.Contains` pada tiap zona. Fungsi ini dipanggil `SunlightExposure.CheckExposure` dan penolakan titik di `SpawnManager.FindSpawnPoint` (#63).

### Pemunculan

`LairRegistry.Tick()` berjalan tiap 1 detik, dan memeriksa jarak pemain ke tiap sarang di region yang dimuat.

1. Kalau jarak di bawah `activationRadius`, `!isSpawned`, dan `remainingCount > 0`, panggil `SpawnDormant()`.
2. Kalau jarak di atas `deactivationRadius`, panggil `Despawn()`. Zombie `Dormant` kembali ke pool, dan `remainingCount` tetap.

**`SpawnDormant()`:** untuk tiap `spawnPoints` sampai `remainingCount`, pilih `occupants` lewat `RunSeedManager`, ambil dari `ObjectPooler`, lalu panggil `EnemyAI.EnterDormant(this)`. `HordeCrowdManager` (#93) menghitungnya sebagai musuh aktif.

### State Dormant

`AIState.Dormant` sudah ada di `GLOSSARY.md`. Agen berhenti, animasi tidur atau duduk berjalan, dan AI tidak mencari target. `EnemyAI.IsActiveThreat` bernilai `false`, sehingga #67 dan #26 mengabaikannya.

Zombie Dormant bangun oleh:

| Pemicu | Cara | Catatan |
|---|---|---|
| Suara | `NoiseEmittedEvent`, jarak kurang dari `radius * noiseWakeMultiplier` | Suara dari luar zona harus melewati `exitPoints` terdekat. Dihitung sebagai jarak ke `exitPoints` ditambah jarak ke zombie |
| Cahaya | Senter menyala, dalam `lightWakeRange`, dan garis pandang bebas selama `lightWakeSeconds` | Diperiksa `senseIntervalSeconds` lewat `PhysicsQuery.Raycast` dengan `PhysicsMasks.AIVision` |
| Jarak | Pemain dalam `proximityWakeRadius`, atau `crouchProximityWakeRadius` saat jongkok | `PlayerTraversal.IsCrouching` |
| Damage | Menerima damage apa pun | Lewat `EnemyHealth` |

Saat bangun, panggil `WakeGroup(position)`, yaitu membangunkan zombie lain dalam `groupWakeRadius`. Lalu pindah ke `Chase`, dan `ThreatSystem` (#108) memilih target. Zombie Lumut tetap memakai pemicu milik #25.

### Tidak keluar ke area terang

Saat siang, zombie sarang memakai `NavMeshAgent.areaMask` yang hanya memuat Area `Interior`. Area itu dipasang lewat `NavMeshModifierVolume` di dalam `InteriorZone`, saat bake NavMesh per region. Jadi jalur otomatis berhenti di ambang pintu.

**Pengejaran saat siang:**

1. Pemain keluar dari zona. `NavMesh.CalculatePath` ke pemain tidak lengkap.
2. Zombie berjalan ke `exitPoints` terdekat, lalu berhenti tepat sebelum cahaya.
3. Setelah `doorWaitSeconds`, zombie kembali ke titik tidurnya. Setelah `returnToDormantSeconds` tanpa target, panggil `EnterDormant`.
4. Kalau terdorong keluar oleh ledakan, zombie berada di luar zona. `SunlightExposure` (#63) membakarnya sesuai aturan, dan `EnemyHealth` menghitung kematian biasa.

**`DaybreakEvent`:** zombie sarang (`EnemyAI.lair != null`), baik tidur maupun sudah bangun, tidak terbakar selama `Contains` bernilai `true`, dan `areaMask` kembali hanya `Interior`. Zombie yang sedang di luar zona mengikuti #63.

**`NightfallEvent`:** kalau `LairData.region != Logistic`, `areaMask` kembali penuh, dan sebanyak `nightReleaseRatio` dari `remainingCount` keluar lewat `exitPoints` menjadi anggota horde, dengan menyerahkan kendali ke `SpawnManager` (#62). Rujukan `EnemyAI.lair` milik zombie itu dikosongkan, dan `Died` dilepas, sehingga saat fajar ia diperlakukan sebagai zombie horde. Sisanya tetap `Dormant`. Kalau `LairData.region == Logistic`, `nightReleaseRatio` tidak berlaku: `areaMask` tetap `Interior`, dan semua penghuni tetap di dalam ruangan, karena GDD bagian 5 hanya menyebut Zombie Desa sebagai ancaman malam Region 4 (usulan 2). Saat `DaybreakEvent` berikutnya, jumlah sarang diisi ulang sampai `dormantCount`, kecuali `isCleared`.

### Loot

`LootSpawnerSystem` di dalam zona memakai `data.lootTable` (#103). Peti diisi saat region dimuat, dan statusnya mengikuti aturan #103. Hadiah bernilai tinggi diletakkan di ruang terdalam, supaya pemain harus melewati zombie.

### Menghancurkan sarang

`LairNest` memakai `IDamageable` dengan `data.nestHP` (nilai awal 300, `GameBalanceConfig.lair.nestHP`). Saat HP habis:

1. Isi `isNestDestroyed = true` dan `isCleared = true`.
2. Bangunkan semua zombie yang tersisa.
3. Terbitkan `LairClearedEvent { lairId, region }`.
4. `SpawnManager` (#62) mengurangi anggaran horde region itu sebesar `GameBalanceConfig.lair.clearedBudgetReduction`, dengan batas `maxBudgetReduction`. Nilainya dibaca dari data sarang, jadi tidak perlu dibawa `LairClearedEvent`.

`isCleared` juga berlaku di lokasi tanpa sarang, saat `remainingCount == 0`, tetapi tanpa pengurangan horde. Lokasi itu hanya berhenti terisi ulang.

### Cahaya dan senter

- `isDark = true` membuat `EnvironmentDirector` mematikan pengaruh matahari dan memakai ambient rendah di zona ini.
- Senter (`FlashlightController`) dan obor menjadi sumber cahaya utama. Keterlihatan pemain dihitung #98.
- Sumber cahaya yang dipasang di dalam zona dibatasi lewat #98.
- Senter membantu pemain melihat, tetapi membangunkan zombie. Tanpa senter, pemain tetap aman tetapi sulit menemukan loot.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `InteriorZoneChangedEvent` (baru, sticky), `LairClearedEvent` (baru).

**Event yang didengar:** `DaybreakEvent`, `NightfallEvent`, `NoiseEmittedEvent`, `RegionChangedEvent`, `RunEndedEvent`.

| Sistem | Hubungan |
|---|---|
| `EnemyAI`, `SpecialZombieAI` (#25) | `EnterDormant`, `AIState.Dormant`, dan varian |
| `SunlightExposure`, `HidingSpot` (#63) | Pengecualian di dalam zona |
| `SpawnManager` (#62), `AIDirector` (#27) | Horde malam dan anggaran. `SpawnManager` tidak membuat spawn terbuka di dalam `InteriorZone` |
| `HordeCrowdManager` (#93) | Batas jumlah musuh aktif |
| `AISensory`, `NoiseEmitter` (#49) | Suara dan pandangan |
| `ThreatSystem` (#108) | Target setelah bangun |
| `RegionLootTable`, `LootSpawnerSystem` (#103) | Loot |
| `LightSourceSystem` (#98), `FlashlightController` | Cahaya |
| `EnvironmentDirector` (#92), `RegionalWeather` (#115) | Suasana dan suara |
| `BaseNavigation` (#78) | Tambahan Area `Interior` |
| `RegionStreaming` (#88), `WorldStateSave` (#80) | Menyimpan status tiap sarang |

## 4. Kasus Khusus

- **Objek pool:** `EnterDormant` mengisi state, `areaMask`, dan rujukan sarang. `OnReturned` mengosongkan rujukan sarang dan mengembalikan `areaMask` ke nilai bawaan. `EnemyKilledEvent` tidak dipakai, karena `ZombieLair` mendengar `EnemyHealth.Died` milik zombie itu saat `EnterDormant`, lalu melepasnya saat `OnReturned`.
- **Game dijeda:** `LairRegistry.Tick` dan timer memakai `Time.deltaTime`, sehingga berhenti.
- **Permadeath dan save:** `ZombieLair.CaptureState` menyimpan `lairId`, `remainingCount`, `isCleared`, `isNestDestroyed`, dan HP sarang lewat `RegionSceneRoot` ke `RegionStateStore`. Zombie aktif tidak disimpan. Saat dimuat, zombie muncul lagi `Dormant` sesuai `remainingCount`. Pengurangan horde dihitung ulang oleh `SpawnManager` dari jumlah sarang hancur. Semua terhapus saat permadeath.
- **Region dilepas:** `Despawn()` berjalan, zombie kembali ke pool, dan status masuk `RegionStateStore`.
- **Pemain tidur di dekat sarang:** `IsActiveThreat = false` membuat zombie Dormant tidak menghalangi tidur (#67). Suara dan jarak tetap bisa membangunkannya.
- **Pemain membangun di dalam zona:** ditolak dengan `NoBuildZone`, karena zona membawa `NoBuildVolume`.
- **Pemain bersembunyi dekat pintu saat siang:** zombie berhenti di pintu. Pemain boleh menembak dari luar, dan zombie kembali masuk setelah `doorWaitSeconds`.
- **Zombie horde yang berada di dalam zona saat fajar:** bukan zombie sarang (`EnemyAI.lair` kosong), jadi tidak terlindungi dan tetap mengikuti aturan #63. `SeekHiding` hanya milik Zombie Desa.
- **Pintu struktur pemain:** dinding pemain di ambang pintu tidak mengubah `Interior`. `AttackBuilding` tetap berjalan malam hari.
- **NavMesh rusak di pintu:** `NavMesh.SamplePosition` menempatkan zombie kembali ke titik tidurnya saat `areaMask` berganti.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** `AIState.Dormant` (`GLOSSARY.md`), Zombie Desa di air atau lumpur (GDD bagian 5), sinar matahari membunuh zombie, serta zombie boleh diam di ruang gelap saat siang dan bangun kalau terganggu (GDD bagian 3, keputusan developer).

**Usulan yang disetujui developer sebagai nilai awal (dicatat di #85):**

1. **Lokasi:** GDD bagian 3 menyebut gua, bunker, dan gudang. Pembagian gua Region 1 dan 2, bunker Region 3, gudang Region 4, serta tambahan rumah runtuh Region 4 adalah usulan. Rekomendasi: pakai.
2. **Zombie standar di Region 4:** GDD hanya menyebut Zombie Desa sebagai ancaman malam di region itu. Rekomendasi: pakai zombie standar di dalam ruangan, dalam jumlah kecil, karena Zombie Desa harus bersembunyi di air atau lumpur.
3. **Menghancurkan sarang mengurangi horde:** 10 persen per sarang, paling banyak 30 persen per region. Rekomendasi: pakai, sebagai alasan menyerang sarang. Horde tetap datang sesuai GDD.
4. **Sarang menyumbang horde malam:** separuh keluar saat malam, dan terisi lagi saat fajar, kecuali lokasi Region 4. Rekomendasi: pakai, sesuai kalimat GDD "keluar dari persembunyiannya".
5. **Batas pengejaran:** berhenti di pintu saat siang, lewat `areaMask`. Rekomendasi: pakai.
6. **Senter membangunkan zombie setelah 2 detik.** Rekomendasi: pakai, supaya pemain menyeimbangkan cahaya dan risiko.

## 6. Skenario Tes

1. Setel sakelar uji `lair.enabled = false` lewat `balance` di `DevConsole`, lalu datangi gua. **Hasil:** tidak ada zombie sarang.
2. Dengan nilai awal `enabled = true`, dekati gua Region 1 saat siang. **Hasil:** 4 sampai 6 zombie muncul `Dormant` saat jarak 40 meter, dan tidak ada yang terbakar.
3. Jalan jongkok dengan senter mati. **Hasil:** zombie tetap tidur, kecuali jarak di bawah 1,5 meter.
4. Sorot zombie dengan senter selama 2 detik. **Hasil:** zombie bangun bersama yang berjarak 6 meter darinya.
5. Tembak dari jauh di dalam gua. **Hasil:** semua dalam radius suara bangun.
6. Lari keluar dari gua saat siang. **Hasil:** zombie berhenti di ambang pintu, menunggu 6 detik, lalu kembali. Tidak ada yang berjalan di bawah matahari.
7. Dorong zombie keluar dengan ledakan saat siang. **Hasil:** zombie terbakar mengikuti #63.
8. Tunggu `NightfallEvent` di luar gua. **Hasil:** separuh zombie keluar, dan bergabung dengan horde.
9. Tunggu fajar. **Hasil:** jumlah sarang kembali penuh, kecuali `isCleared`.
10. Hancurkan sarang Region 1. **Hasil:** `LairClearedEvent` terkirim, dan horde malam berikutnya berkurang 10 persen. Hancurkan sarang kedua di region yang sama sampai batas. **Hasil:** berhenti di 30 persen.
11. Masuk zona. **Hasil:** `InteriorZoneChangedEvent` terkirim sekali, ruangan gelap, dan suara hujan teredam.
12. Coba bangun di dalam zona. **Hasil:** hologram merah dengan `NoBuildZone`.
13. Bunuh 3 zombie, simpan, lalu muat. **Hasil:** `remainingCount` sesuai, dan zombie muncul lagi `Dormant`.
14. Jauhi sarang sampai 60 meter. **Hasil:** zombie kembali ke pool. Dekati lagi. **Hasil:** zombie yang dipakai ulang punya HP penuh dan `areaMask` yang benar.
15. Mati, lalu mulai run baru. **Hasil:** semua sarang kembali utuh.
16. Tunggu `NightfallEvent` di luar gudang desa Region 4, lalu bangunkan penghuninya. **Hasil:** tidak ada zombie standar yang keluar dari ruangan, dan horde Region 4 hanya berisi Zombie Desa.
17. Bangunkan zombie sarang di dalam gua, lalu majukan jam melewati fajar saat ia masih mengejar di dalam zona. **Hasil:** tidak terbakar, dan `areaMask` kembali hanya `Interior`.
18. Biarkan zombie yang dilepas saat malam tetap di luar gua sampai fajar. **Hasil:** terbakar seperti zombie horde, dan jumlah sarang terisi ulang.

## Catatan Implementasi (Sistem pendukung)

- Kode: `ZombieLair.cs`, `InteriorZone.cs`, `LairNest.cs`, `LairRegistry.cs`, dan `LairData.cs` di `Assets/LOGIC/left-4-dead/`. Config `lair` ada di `Core_Logic/GameBalance.GrupF.cs`, dengan tambahan `nestHP` 300 dan `registryTickSeconds` 1 sesuai teks bagian 2.
- `RegionStateStore` (#88) belum ada. Karena itu, `ZombieLair` mendaftar langsung ke `WorldStateSave` dengan `SaveId` `lair_<lairId>` dan `LoadOrder` 140, sama seperti `LuckyMachine`. Ganti cara ini saat #88 sudah ada.
- `EnemyAI` belum punya field `lair` dan `EnterDormant(lair)`. Rujukan zombie ke sarang disimpan di `LairRegistry` (`GetLair`, `IsLairZombie`, dan `IsShelteredLairZombie`). `areaMask` diatur `ZombieLair` setelah `EnemyAI.EnterDormant()`.
- `Died` dan `Damaged` didengar per slot zombie. Delegate-nya dibuat sekali di `Awake`.
- Zombie yang kehilangan target, atau selesai menunggu di pintu, langsung kembali `Dormant` sambil berjalan ke titik tidurnya. Pemicu bangun tetap berlaku selama ia berjalan.
- Zombie yang dilepas saat malam belum masuk daftar `SpawnManager`, karena belum ada API pengambilalihan. Jatah yang belum muncul diminta lewat `SpawnManager.RequestBurst` di pintu keluar terdekat.
- `LairNest` memakai `LairData.nestHP` kalau nilainya lebih dari 0. Selain itu, `LairNest` memakai `lair.nestHP`.
- `FlashlightController` dan #98 belum ada, jadi cahaya dibaca dari `TorchController` yang menyala.
- `lootCollected` tidak disimpan di sini. Loot tetap milik `LootSpawnerSystem` (#103).
- Pendengar `InteriorZoneChangedEvent` (`EnvironmentDirector`, `RegionalWeather`, dan `WarmthSystem`) masih berupa TODO di kode lama.
