# Blueprint: SunlightExposure.cs & HidingSpot.cs
**Lokasi:** `Survival Island/doc/blueprint/left-4-dead/SunlightExposure.md`
**Deskripsi Utama:** Aturan zombie terbakar sinar matahari, beserta pengecualian Zombie Desa yang bersembunyi di air atau lumpur Region 4 dan zombie sarang di ruang gelap (#112) pada siang hari.
**Issue:** #63
**Rujukan GDD:** Bagian 3 (zombie terbakar dan mati di bawah matahari, tidak berkeliaran di luar ruangan saat siang, dan sebagian bersembunyi diam di ruang gelap) dan bagian 5 (Region 2 gelap, dan Zombie Desa berendam atau terkubur lalu naik secara massal saat malam).

## 1. Arsitektur & Variabel Utama

Blueprint ini menggantikan `EnemyAI.TriggerDaybreakDeath()`. Pencarian lewat tag tidak dipakai, karena `FindGameObjectsWithTag` dilarang saat bermain (#87). Tiap zombie mendengar event sendiri.

| Kelas | Jenis | Tempat | Fungsi |
|---|---|---|---|
| `SunlightExposure` | `MonoBehaviour`, `IPoolable` | Prefab semua zombie | Pengecekan sinar dan pembakaran |
| `VillageZombieHiding` | `MonoBehaviour`, `IPoolable` | Prefab `zombie_desa` | State `SeekHiding`, `Submerged`, dan `Emerging` |
| `HidingSpot` | `MonoBehaviour` | Scene `Region4_Village` | Titik air dan lumpur |

**Variabel `SunlightExposure`:**

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `IsBurning` | `bool` | `false` | Dibaca #95 dan `EnemyAI` |
| `IsProtected` | `bool` | `false` | `true` saat `Submerged`, atau untuk zombie sarang di dalam `InteriorZone` (#112) |
| `shelteredTimer` | `float` | 0 | Lama berada di tempat tertutup saat siang |
| `baseScale` | `Vector3` | Dari `Awake` | Skala asli untuk reset |
| `burnEffect` | `ParticleSystem` | Prefab | Api |

**Variabel `HidingSpot`:**

```csharp
public enum HidingKind : byte { Water, Mud }

public class HidingSpot : MonoBehaviour
{
    public HidingKind kind;
    public int capacity;              // Air 8, lumpur 4
    public Transform entryPoint;      // Di tepi, berada di NavMesh
    public Transform[] emergePoints;  // Titik naik, berada di NavMesh
    public float buryDepth = 2f;      // Jarak turun di bawah permukaan
    public ParticleSystem tellEffect; // Gelembung atau lumpur bergerak
}
```

Total kapasitas semua titik di Region 4 minimal sama dengan ukuran pool `zombie_desa`, yaitu 30 (#62).

**Angka (`GameBalanceConfig.sunlight`):**

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `checkIntervalSeconds` | 2 | Jeda pengecekan sinar per zombie |
| `rayLength` | 200 m | Panjang ray ke matahari |
| `minSunElevationDeg` | 35 | Sudut matahari terendah untuk ray |
| `burnStaggerMaxSeconds` | 4 | Jeda acak sebelum api, supaya terbakar bergelombang |
| `burnDurationSeconds` | 3 | Lama meleleh |
| `maxShelteredSeconds` | 60 | Batas berlindung siang hari |
| `seekHidingHour` | 5 | Jam Zombie Desa mulai bersembunyi |
| `spotSearchRadius` | 120 m | Jarak terjauh mencari `HidingSpot` |
| `submergeSeconds` / `emergeSeconds` | 2 / 2 | Lama turun dan naik |
| `emergeStaggerSeconds` | 4 | Sebaran waktu naik |
| `emergeBoostMultiplier` / `emergeBoostSeconds` | 1,2 / 8 | Tambahan kecepatan setelah naik |
| `daySubmergedCount` | 12 | Zombie Desa yang disiapkan saat Region 4 dimuat siang hari |

`ZombieData` (#25) menambah `hidesFromSun`, dengan nilai `true` hanya pada `zombie_desa`.

## 2. Detail Logika & Fungsi

### Aturan sinar

Lihat rekomendasi di bagian 5. Ringkasnya, tidak ada zombie yang tersisa di luar ruangan pada siang hari. Sinar langsung membakar, sedangkan tempat tertutup hanya menunda sampai `maxShelteredSeconds`. Pengecualiannya zombie yang bersembunyi: `Submerged`, dan zombie sarang di ruang gelap (GDD bagian 3).

| Keadaan | Akibat |
|---|---|
| Terkena sinar langsung | Terbakar |
| Kanopi Region 2, bangunan, atau bayangan gunung | Tertunda paling lama 60 detik |
| Badai dan hujan | Tidak melindungi, karena `RegionalWeather` tidak ikut dihitung |
| `Submerged`, atau zombie sarang (`EnemyAI.lair != null`) di `InteriorZone` (#112), baik `Dormant` maupun sudah bangun | Terlindungi |
| Zombie horde di dalam `InteriorZone` (`EnemyAI.lair` kosong) | Tidak terlindungi. Mengikuti baris sinar dan tempat tertutup di atas |

### Pengecekan berkala

**`HandleDaybreak(DaybreakEvent e)`** memulai `ExposureLoop()`. Kalau `OnEnable` menemukan `DayNightDirector.IsDay == true`, misalnya musuh dari `DevConsole`, loop ikut mulai.

**`IEnumerator ExposureLoop()`:**

1. Tunggu jeda awal `(GetInstanceID() & 7) / 8f * checkIntervalSeconds`, supaya zombie tidak mengecek pada frame yang sama. Jeda ini tidak mengambil aliran acak.
2. Setiap `checkIntervalSeconds`, kalau `!IsBurning` dan zombie tidak `Submerged`, panggil `CheckExposure()`.
3. Berhenti saat `IsBurning`, atau saat `NightfallEvent`. Tidak ada pengecekan pada malam hari.

**`private void CheckExposure()`:**

1. Kalau `EnemyAI.lair != null` dan `InteriorZone.Contains(posisi)` (#112) bernilai `true`, `IsProtected = true`, lalu selesai. Kalau tidak, `IsProtected = false`, lalu lanjut. Zombie yang sudah keluar dari zona, atau zombie horde yang masuk zona, tidak terlindungi.
2. `dir = DayNightDirector.Instance.SunDirection`, dengan elevasi dinaikkan ke minimal `minSunElevationDeg`. Tanpa batas ini, matahari yang masih rendah akan membuat bukit melindungi hampir semua zombie.
3. `PhysicsQuery.Raycast` dari atas kepala dengan `dir`, sepanjang `rayLength`, memakai `PhysicsMasks.Sunlight`.
4. Kalau tidak kena apa pun, panggil `StartBurn()`.
5. Kalau kena, `shelteredTimer += checkIntervalSeconds`. Kalau `>= maxShelteredSeconds`, panggil `StartBurn()`.

Biaya maksimal 50 zombie x 1 ray per 2 detik, hanya saat pagi. Karena tiap zombie cepat terbakar, jumlah ini langsung turun.

### Pembakaran

**`public void StartBurn()`** memulai coroutine `BurnRoutine()`, dengan penjaga `burnPending` supaya hanya berjalan sekali:

1. Hitung jeda acak sampai `burnStaggerMaxSeconds` lewat `RandomStreamId.Enemy`, sebelum masuk iterator, lalu tunggu.
2. Kalau `!EnemyHealth.IsAlive`, selesai. Zombie sudah dibunuh pemain, jadi mayat mengikuti #95.
3. `IsBurning = true`. `EnemyAI.EnterBurn()`: `AIState.BurnToAshes`, hentikan FSM dan serangan, `agent.enabled = false`, serta lepaskan `PlayerPinnedState` kalau sedang menahan.
4. Kirim damage mematikan: `EnemyHealth.TakeDamage` dengan `amount = CurrentHP * 1000`, `type = Fire`, `byPlayer = false`, dan `sourceId = "sunlight"`, sehingga armor tidak bisa menahannya. Karena `IsAlive` menjadi `false`, tembakan pemain berikutnya diabaikan, dan `EnemyKilledEvent` terkirim dengan `byPlayer = false`. `EnemyAI` dan #95 mengabaikan `Died` saat `IsBurning`.
5. `burnEffect.Play()`, dan `anim.Play(AnimIds.Burn)` (#12).
6. Selama `burnDurationSeconds`, turunkan skala Y dari `baseScale.y` ke 0.
7. Panggil `SpawnManager.Instance.Despawn(gameObject, DespawnReason.Burned)`.

Zombie yang terbakar tidak menjatuhkan loot, dan #95 melewati mayat serta ragdoll saat `IsBurning`.

### Zombie Desa

**`SeekHiding`:** dimulai saat `HourChangedEvent` mencapai `seekHidingHour`. Pada `timeMultiplier` 96, satu jam game sama dengan 37,5 detik, jadi itulah waktu lari sebelum fajar. Zombie yang muncul setelah jam itu membaca `EventChannel<HourChangedEvent>.Last` dalam `OnEnable`.

1. Lepas target, dan tinggalkan pemain. Hapus boost lewat `EnemyAI.SetSpeedMultiplier(SpeedSource.EmergeBoost, 1)`. `agent.speed` tidak ditulis langsung, supaya pengali gelombang dan sumber lain tetap berlaku (`EnemyAI.md`).
2. `spot = HidingSpot.FindAvailable(posisi, spotSearchRadius)`, yaitu titik terdekat dengan tempat kosong. Panggil `spot.Reserve(this)`.
3. Kalau tidak ada titik, zombie tetap mengejar pemain seperti biasa, lalu kena aturan terlambat.
4. Pergi ke `spot.entryPoint`. Kalau tiba, masuk `Submerged`.

**`Submerged`:**

1. `agent.enabled = false`, kapsul `Enemy` dan hitbox `EnemyHitbox` dimatikan, serta `EnemyInstance.IsDormantCounted = true`.
2. Set bool `AnimIds.Dormant = true` (#12). Turun sejauh `buryDepth` selama `submergeSeconds`, lalu `Renderer` dimatikan.
3. `SunlightExposure.IsProtected = true`.
4. FSM berhenti, sehingga tidak ada beban AI.

**Terlambat bersembunyi.** Kalau `DaybreakEvent` tiba saat belum `Submerged`, Zombie Desa diperlakukan seperti zombie lain: terbakar saat `CheckExposure` pertama, dengan tempat kosong dilepas. Ini menjadi celah untuk pemain yang menahan mereka sampai pagi.

**`Emerging`:** saat `NightfallEvent`:

1. Jeda acak sampai `emergeStaggerSeconds`, supaya naik bergelombang.
2. Pilih `emergePoints` acak, `agent.Warp` ke sana, dengan model mulai dari `buryDepth` di bawahnya.
3. Nyalakan `Renderer`, hitbox, dan kapsul. Set `AnimIds.Dormant = false`, panggil trigger `AnimIds.Emerge` (#12), lalu naik selama `emergeSeconds`, sambil memainkan efek dan suara.
4. `spot.Release(this)`, `IsDormantCounted = false`, dan `IsProtected = false`.
5. Panggil `AISensory.ForceAware(posisi pemain)` kalau pemain berada dalam jarak `huntHintRadius` x 2. Lainnya memakai perburuan biasa (#49).
6. `EnemyAI.ApplySpeedModifier(SpeedSource.EmergeBoost, emergeBoostMultiplier, emergeBoostSeconds)`, lalu kembali normal saat waktunya habis. `AIState.Chase`.

**"Sangat cepat"** berasal dari `moveSpeed` Zombie Desa, yaitu 7,5 m/s di #25. Dengan boost (7,5 x 1,2 = 9 m/s), zombie sempat melebihi `sprintSpeed` 8 selama 8 detik pertama, lalu pemain kembali bisa lolos. `EmergeBoost` dikalikan sesudah batas `maxZombieSpeed` (7,8), jadi tidak terpotong (`EnemyAI.md` bagian Kecepatan). Terkaman setelah naik milik `VillageBehavior` (#25).

**Laporan ke Director.** Saat `NightfallEvent`, `SpawnManager` memanggil `SunlightExposure.ReportEmergence()`, yang meneruskan `AIDirector.Instance.AddExternalSpawn(HidingSpot.TotalOccupants)`. Dengan begitu, mob pertama tidak menumpuk di atas zombie yang baru naik (#27).

### `HidingSpot`

| Fungsi | Isi |
|---|---|
| `static HidingSpot FindAvailable(Vector3 from, float radius)` | Titik terdekat yang punya tempat. Daftar statis diisi `OnEnable`, dan dilepas `OnDisable` |
| `static HidingSpot FindEmergeSpot(Vector3 player)` | Titik terdekat ke pemain dalam `spawn.maxDistance`, untuk spawn malam |
| `bool Reserve(VillageZombieHiding z)` / `void Release(...)` | Mengatur penghuni |
| `int Occupants` | Jumlah penghuni. `tellEffect` aktif kalau lebih dari 0 |
| `static int TotalOccupants` | Jumlah penghuni semua titik |

**Mengisi titik saat siang.** `SpawnManager.PopulateHidingSpots()` memunculkan Zombie Desa langsung dalam keadaan `Submerged`, sampai jumlah penghuni semua titik mencapai `daySubmergedCount`. Fungsi ini dipanggil saat Region 4 dimuat pada siang hari, dan saat fajar. Ini membuat desa terasa berbahaya pada malam pertama pemain di sana.

**Spawn malam.** Slot gelombang malam `SpawnManager` (`RequestWave`, `RequestBurst`, `RequestAmbush`, bukan `TickSpawn` yang hanya mengisi hewan) memilih titik lewat `HidingSpot.FindEmergeSpot(playerPos)`, lalu memanggil `VillageZombieHiding.SpawnEmerging(spot)` pada zombie yang baru keluar dari pool (`SpawnManager.md`). Dengan begitu, tambahan musuh malam juga muncul dari air dan lumpur.

### Terlihat dan diserang

Saat `Submerged`, zombie tidak terlihat dan tidak bisa diserang, karena `Renderer`, kapsul, dan hitbox mati. Pemain hanya melihat tanda samar: gelembung di air, lumpur bergerak, dan suara rendah saat mendekat. Ledakan tidak mengenainya, karena `PhysicsMasks.Explosion` memerlukan kapsul `Enemy`.

### Reset dari pool

**`SunlightExposure.OnReturned()`:**

- `StopAllCoroutines()`.
- `burnEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear)`.
- `transform.localScale = baseScale`.
- `agent.enabled = false`, supaya agen tidak menempel ke posisi lama.
- `IsBurning = false`, `burnPending = false`, `IsProtected = false`, dan `shelteredTimer = 0`.

**`SunlightExposure.OnSpawned()`:**

- `agent.enabled = true`, lalu `agent.Warp(transform.position)`, dan `agent.isStopped = false`.
- `animator.Rebind()`.
- Kalau siang, mulai `ExposureLoop()`.

**`VillageZombieHiding.OnReturned()`:** `spot?.Release(this)`, lalu kembalikan `Renderer`, kapsul, dan hitbox ke nilai awal. Pengali `EmergeBoost` dihapus `EnemyAI.OnSpawned`, yang mengembalikan semua `speedModifiers` ke 1. Layer hitbox mengikuti #87 dan #95.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** tidak ada event baru. `EnemyHealth` mengirim `EnemyKilledEvent` dengan `DamageType.Fire`.

**Event yang didengar:** `DaybreakEvent`, `NightfallEvent`, `HourChangedEvent` (hanya `zombie_desa`).

| Sistem | Hubungan |
|---|---|
| `DayNightDirector` | Menyediakan `IsDay` dan `SunDirection` |
| `EnemyAI` | `EnterBurn()`, serta state `BurnToAshes`, `SeekHiding`, `Submerged`, dan `Emerging` |
| `SpawnManager` (#62) | `Despawn`, `PopulateHidingSpots`, dan `FindEmergeSpot` |
| `AISensory` (#49) | `ForceAware` saat naik, dan `Tick` dilewati saat `Submerged` |
| `DamagePipeline` (#59) | Jalur damage `Fire` |
| `EnemyHitReaction` (#95) | Melewati mayat kalau `IsBurning` |
| `SpecialZombieAI` (#25) | `hidesFromSun` dan `moveSpeed` |
| `ZombieLair` (#112) | Menyediakan `InteriorZone.Contains` dan `EnemyAI.lair`, yang dibaca `CheckExposure` untuk mengisi `IsProtected` |
| `AIDirector` (#27) | Mendengar `EnemyKilledEvent`, membaca `byPlayer`, dan menerima `AddExternalSpawn` |
| `PhysicsQuery`, `PhysicsMasks` (#87) | Mask `Sunlight` |

## 4. Kasus Khusus

- **Game dijeda:** `WaitForSeconds` dan `Time.deltaTime` berhenti, jadi api dan penyusutan berhenti di tengah, lalu lanjut.
- **Tidur:** waktu dipercepat lewat `timeMultiplier`. Kalau jam melompati `seekHidingHour`, Zombie Desa yang terlambat terkena aturan terlambat. Pada malam tidur, pemain ada di base, jadi hal ini jarang terlihat.
- **Permadeath dan save:** tidak ada data yang disimpan. Zombie dan penghuni `HidingSpot` hilang bersama run, serta `ReturnAllActive()` menjalankan reset. Setelah load, `SpawnManager` mengisi ulang titik kalau siang.
- **Region 4 dilepas:** semua zombie, termasuk `Submerged`, dikembalikan oleh `DespawnRegion` (#88), dan penghuni dilepas.
- **Zombie Desa keluar dari Region 4:** tidak menemukan titik, lalu terbakar saat fajar.
- **Titik penuh:** cari titik lain. Kalau tidak ada, kena aturan terlambat.
- **Pemain berdiri di titik naik:** zombie tetap naik, dan kapsul mendorong lewat agen. Tidak ada damage khusus.
- **Pemain berenang di atas titik (#13):** tidak ada tabrakan, karena kapsul mati.
- **Blood Moon:** berakhir saat fajar (#50), lalu aturan biasa berlaku.
- **Zombie dimatikan culling:** `agent` nonaktif, tetapi `ExposureLoop` tetap jalan, karena hanya butuh transform. `EnterBurn` aman dipanggil saat agen sudah mati.
- **Zombie sedang menahan pemain (#110):** pemain dilepas di `EnterBurn`.
- **Keluar masuk tempat tertutup:** `shelteredTimer` tidak direset selama siang, jadi waktunya tidak bisa diperpanjang.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** zombie terbakar dan tidak berkeliaran di luar ruangan saat siang, sebagian zombie diam di ruang gelap, serta Zombie Desa berendam atau terkubur (GDD bagian 3 dan 5). State `SeekHiding`, `Submerged`, `Emerging`, dan `BurnToAshes` (`GLOSSARY.md`). Mask `Sunlight` (#87).

**Keputusan teknis di blueprint ini:**

- Pengecekan berkala per 2 detik dan hanya pada siang hari, tanpa `Update`.
- `EnemyAI.TriggerDaybreakDeath()` dihapus, dan digantikan `SunlightExposure`.
- Saat `Submerged`, FSM dan agen mati, sehingga zombie itu hampir tanpa beban.

**Usulan yang disetujui developer sebagai nilai awal:**

1. **Aturan fajar.** Rekomendasi: raycast ke arah matahari, dengan batas `maxShelteredSeconds`. Hasilnya sinar langsung membakar, dan tetap tidak ada zombie berkeliaran di luar ruangan pada siang hari, sesuai GDD.
2. **Kanopi Region 2, bangunan, dan badai.** Rekomendasi: kanopi dan bangunan hanya menunda paling lama 60 detik, sedangkan badai tidak melindungi. GDD menyebut Region 2 gelap sebagai suasana, bukan pengecualian. Pengecualian ruang gelap hanya untuk zombie sarang di dalam `InteriorZone` (#112), dan bukan usulan, karena sudah ada di GDD bagian 3.
3. **Akibat Zombie Desa terlambat.** Rekomendasi: terbakar seperti zombie lain.
4. **Zombie yang sedang sembunyi.** Rekomendasi: tidak terlihat dan tidak bisa diserang, tetapi ada tanda gelembung atau lumpur.
5. **Kapasitas `HidingSpot`.** Rekomendasi: air 8, lumpur 4, dan total minimal 30.
6. **Waktu mulai sembunyi.** Rekomendasi: 05:00.

## 6. Skenario Tes

1. Berdiri di dekat 10 zombie standar di lapangan terbuka, lalu majukan jam ke 06:00. **Hasil:** semua terbakar bergelombang dalam beberapa detik, lalu kembali ke pool.
2. Taruh zombie di dalam bangunan beratap. **Hasil:** tidak langsung terbakar, tetapi terbakar setelah sekitar 60 detik.
3. Taruh zombie di hutan Region 2. **Hasil:** paling lama 60 detik, tidak ada zombie tersisa.
4. Picu badai saat fajar. **Hasil:** aturan sama dengan cuaca cerah.
5. Tembak zombie yang sedang terbakar. **Hasil:** tidak ada damage kedua, dan statistik tidak berubah.
6. Di Region 4, biarkan jam 05:00 tiba. **Hasil:** Zombie Desa berhenti mengejar dan lari ke `HidingSpot`.
7. Isi titik sampai penuh. **Hasil:** zombie berikutnya mencari titik lain, dan yang tidak kebagian terbakar.
8. Tahan satu Zombie Desa jauh dari titik sampai fajar. **Hasil:** terbakar saat pengecekan pertama.
9. Pada siang hari, tembak dan lempar peledak ke titik yang berisi. **Hasil:** tidak ada damage, dan gelembung tetap ada.
10. Majukan jam ke 18:00 di Region 4. **Hasil:** Zombie Desa naik bergelombang dalam 4 detik, lalu lari lebih cepat dari jalan kaki pemain.
11. Pakai ulang zombie yang terbakar sebanyak 20 kali. **Hasil:** skala normal, agen di NavMesh, partikel bersih, dan `IsBurning = false`.
12. Pantau 50 zombie saat fajar di Profiler. **Hasil:** tidak ada lonjakan ray dalam satu frame, dan beban malam hari nol.
13. Lepas Region 4 saat siang, lalu muat kembali. **Hasil:** tidak ada kebocoran penghuni, dan titik terisi ulang.
14. Majukan jam ke 06:00 saat zombie sarang `Dormant` dan yang sudah bangun berada di dalam `InteriorZone`. **Hasil:** keduanya `IsProtected = true`, tidak terbakar, dan tetap ada setelah 60 detik.
15. Dorong zombie sarang keluar dari zona saat siang. **Hasil:** `IsProtected = false`, dan terbakar saat pengecekan berikutnya.
16. Taruh zombie horde di dalam `InteriorZone` saat fajar. **Hasil:** tidak terlindungi, dan terbakar paling lama setelah 60 detik.
17. `setday 60` di Region 4, lalu majukan jam ke 18:00 dan tembak kaki Zombie Desa yang baru naik. **Hasil:** kecepatan = `Min(moveSpeed x pengali gelombang x legSlowMultiplier, maxZombieSpeed)` x `emergeBoostMultiplier`. Boost tidak ikut dipotong `maxZombieSpeed`. Setelah `emergeBoostSeconds`, hanya boost yang hilang.

## 7. Catatan Implementasi (Langkah 5)

Kode: `left-4-dead/SunlightExposure.cs`.

- **Arah matahari** dibaca dari `RenderSettings.sun` (Directional Light), karena `DayNightDirector.SunDirection` (Langkah 4) belum ada. Elevasinya tetap dinaikkan ke `minSunElevationDeg`.
- Siang dibaca dari `HourChangedEvent` (`DayPhaseTracker`).
- `VillageZombieHiding`, `HidingSpot`, dan pengecualian zombie sarang di `InteriorZone` (#112) belum ditulis. `IsProtected` bisa diisi dari luar.
