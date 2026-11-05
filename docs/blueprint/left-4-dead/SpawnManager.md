# Blueprint: SpawnManager.cs & RegionSpawnTable.cs
**Lokasi:** `docs/blueprint/left-4-dead/SpawnManager.md`
**Deskripsi Utama:** Menentukan jenis musuh per region dan tempat kemunculannya, siang dan malam, lewat `ObjectPooler`.
**Issue:** #62
**Rujukan GDD:** Bagian 3 (siang hewan di luar ruangan, sebagian zombie diam di ruang gelap, dan malam zombie) dan bagian 5 (ancaman tiap region). Bobot dan angka adalah usulan.

## 1. Arsitektur & Variabel Utama

`SpawnManager` adalah singleton di scene `IslandPhase`. Fase 1 tetap memakai `ShipHazardManager`. Jumlah dan tempo zombie malam ditentukan `AIDirector` (#27) lewat `RequestWave`, sedangkan blueprint ini menentukan jenis dan tempat. Hewan siang tidak melewati Director.

```csharp
[CreateAssetMenu(menuName = "Survival/Region Spawn Table")]
public class RegionSpawnTable : ScriptableObject
{
    public RegionType region;
    public SpawnEntry[] dayEntries;
    public SpawnEntry[] nightEntries;
    public string campCounterEnemyId; // Pembalas untuk #94
}

[Serializable]
public struct SpawnEntry
{
    public EnemyData enemy;   // enemyId sekaligus tag pool
    public float weight;
    public int minDay;        // Bobot dianggap 0 sebelum hari ini
    public int maxAlive;      // Batas per jenis
    public int groupMin, groupMax;
    public bool isSpecial;    // Dibatasi WaveRequest.specialShare
    public bool useBushPoints; // Zombie Lumut, muncul di titik semak (#25)
}

public enum DespawnReason : byte { Killed, Burned, ReturnedToDen, Harvested, Stuck, OffNavMesh, TooFar, RegionUnloaded, Sacrificed, Debug }
```

### Isi tabel (sesuai GDD bagian 5)

| Region | Siang (bobot, kelompok) | Malam (bobot) |
|---|---|---|
| `Volcano` | `hewan_serigala` (60, 2-3), `hewan_babi` (40, 1-2) | `zombie_standar` (100) |
| `Amazon` | `hewan_jaguar` (40, 1), `hewan_ular` (60, 1) | `zombie_standar` (50), `zombie_tribal` (30), `zombie_lumut` (20) |
| `Snow` | `hewan_serigala_salju` (70, 2-3), `hewan_beruang` (30, 1) | `zombie_standar` (40), `zombie_militer` (30), `zombie_beku` (30) |
| `Logistic` | `hewan_anjing` (60, 2-4), `hewan_gagak` (40, 5-8) | `zombie_desa` (100) |

Runner, Tank, dan Spitter tidak menjadi `enemyId` baru. Sesuai `SpecialZombieAI.md` (#25), ketiganya hanya pola perilaku pada zombie yang ada di GDD bagian 5, jadi tabel ini tidak berubah.

### Pencatatan per musuh

`EnemyInstance : MonoBehaviour, IPoolable` ada di tiap prefab musuh Fase 2:

| Variabel | Fungsi |
|---|---|
| `PoolTag` | Tag saat kembali ke pool |
| `GroupId` | Kelompok, dipakai Gagak (#61) |
| `IsEngaged` | Diisi `EnemyAI` dan `WildlifeAI` saat mengejar atau menyerang |
| `IsDormantCounted` | `true` saat `Submerged`, sehingga tidak dihitung sebagai musuh aktif |
| `IsSacrificing` | `public bool`. Diisi `LuckyMachine` (#72) saat vakum, dan dikosongkan di `OnSpawned` |
| `lastPosition`, `stuckTimer` | Pengaman macet |
| `IsCulled`, `culledAgentWasEnabled`, `culledAnimatorWasEnabled` | Status culling milik `OptimizationManager`, beserta status `NavMeshAgent.enabled` dan `Animator.enabled` sebelum culling (usulan, `OptimizationManager.md`) |

### Variabel `SpawnManager`

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `tables` | `RegionSpawnTable[4]` | Inspector | Satu per region |
| `activeEnemies` | `List<EnemyInstance>` | Kapasitas 64 | Dibaca `OptimizationManager` lewat `ActiveEnemies` |
| `activeRegion` | `RegionType` | `Volcano` | Dari `RegionChangedEvent` |
| `isNight` | `bool` | `false` | Dari event siang dan malam |
| `overrides` | `List<SpawnOverride>` | Kosong | Tumpukan override |

### Angka (`GameBalanceConfig.spawn`)

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `tickSeconds` | 2 | Jeda perhitungan spawn |
| `maxSpawnsPerTick` | 4 | Batas per tick |
| `maxSpawnsPerFrame` | 2 | Batas per frame saat burst |
| `maxZombiesAlive` | 30 | Batas zombie aktif, dari batas mob di `docs/CATATAN_MEKANIK.md` |
| `maxZombiesOverride` | 50 | Batas saat override, sama dengan mega mob |
| `maxAnimalsAlive` | 8 | Batas hewan hidup |
| `minDistance` / `maxDistance` | 20 / 45 m | Jarak spawn dari pemain |
| `sampleRadius` | 6 m | Radius `NavMesh.SamplePosition` |
| `attemptsPerSpawn` | 8 | Percobaan mencari titik |
| `behindChance` | 0,75 | Peluang dari belakang pemain |
| `behindHalfAngleDeg` | 70 | Lebar arah belakang |
| `despawnDistance` | 70 m | Musuh tidak terlibat di atas jarak ini dikembalikan |
| `stuckCheckSeconds` | 5 | Jeda pengaman macet |
| `stuckMoveEpsilon` | 0,5 m | Di bawah ini dianggap tidak bergerak |
| `stuckSeconds` | 20 | Dikembalikan kalau di luar pandangan |
| `stuckForceSeconds` | 45 | Dikembalikan walaupun terlihat |
| `cleanupSeconds` | 30 | Jeda sebelum sapu bersih setelah transisi |
| `dayAnimalsAlive` | 4 | Target hewan hidup saat siang |

## 2. Detail Logika & Fungsi

### Siklus

| Event | Tindakan |
|---|---|
| `DaybreakEvent` | `isNight = false`. Zombie di luar ruangan dibakar `SunlightExposure` (#63). Zombie sarang di dalam `InteriorZone` tidak disentuh (#112). Spawn hewan dimulai. `PopulateHidingSpots()` kalau Region 4 dimuat. Jadwalkan `SweepWrongPhase()` setelah `cleanupSeconds` |
| `NightfallEvent` | `isNight = true`. Hewan pergi sendiri (#61). Spawn zombie dimulai. Zombie Desa bangkit (#63). Jadwalkan `SweepWrongPhase()` |
| `RegionChangedEvent` | `activeRegion = to`. Lihat "Pindah region" |
| `RegionLoadStateChangedEvent` `Unloading` | `DespawnRegion(region)` pada frame yang sama |
| `RegionLoadStateChangedEvent` `Loaded` | Kalau `Logistic` dan siang, `PopulateHidingSpots()` |
| `SleepStateChangedEvent` | Selama tidur, tick tidak memunculkan musuh baru. `RequestAmbush` tetap jalan |
| `LairClearedEvent` (#112) | Tambah `regionBudgetReduction[region]` sebesar `lair.clearedBudgetReduction`, paling besar `lair.maxBudgetReduction` (`ZombieLair.md`). Nilai ini mengurangi `count` di `RequestWave` sebagai rasio |
| `RunEndedEvent` | Hentikan coroutine |

**`SweepWrongPhase()`** mengembalikan musuh yang salah waktu dan berada di luar pandangan, misalnya hewan yang tersesat saat malam, kecuali `IsDormantCounted`, zombie sarang di dalam `InteriorZone` (`EnemyAI.lair != null`, #112), dan musuh hasil override aktif.

### Tick spawn

`IEnumerator SpawnLoop()` berisi `WaitForSeconds(tickSeconds)` dan memanggil `TickSpawn()`. Logikanya ada di fungsi biasa, karena `ref` dari `RunSeedManager.GetStream` tidak boleh dipakai di dalam iterator.

**`private void TickSpawn()`:**

Tick ini hanya mengisi hewan pada siang hari.

1. Kalau tidur, malam, atau ada override dengan `suspendNormalSpawn` atau `suspendAnimals`, selesai.
2. `deficit = min(dayAnimalsAlive, maxAnimalsAlive) - CountAwake()`, dan batasi `maxSpawnsPerTick`. Kalau `deficit <= 0`, selesai.
3. Ambil `ref var rng = ref RunSeedManager.Instance.GetStream(RandomStreamId.Enemy)`.
4. Untuk tiap slot: `PickEntry()`, `FindSpawnPoint()`, lalu `SpawnGroup()`. Kalau gagal, hentikan tick ini.

### Gelombang dari Director

**`public int RequestWave(in WaveRequest r)`**, dipanggil `AIDirector` (#27):

1. Kalau tidur, kembalikan 0. Saat tidur, hanya `RequestAmbush` yang boleh memunculkan musuh.
2. `count = r.count * (1 - regionBudgetReduction[activeRegion])`, lalu batasi dengan `CurrentZombieCap - AliveZombies` dan sisa pool.
3. Masukkan `count` slot ke antrean, bersama `r.modifiers`, `r.maxLarge`, `r.specialShare`, `r.forcedEnemyId`, dan `r.origin`. Kembalikan `count`, yang dipakai Director sebagai jumlah yang muncul.
4. `Update` mengerjakan paling banyak `maxSpawnsPerFrame` slot, memakai `PickEntry()` dan `FindSpawnPoint()`. Kalau `forcedEnemyId` terisi, jenisnya tidak diacak.
5. `PickEntry()` memberi bobot 0 pada entri `isSpecial` kalau porsinya sudah `specialShare`, dan pada `ZombieData.isLarge` kalau jumlahnya sudah `maxLarge`.
6. Setelah keluar dari pool: `EnemyHealth.SetHPMultiplier(r.modifiers.hp)`, lalu `EnemyAI.ApplyWaveModifiers(in r.modifiers)`.
7. Slot yang tetap gagal setelah 3 percobaan tick dibuang, lalu panggil `AIDirector.RefundBudget(1)`.

**`PickEntry()`:** isi `float[] weightBuffer` milik manager, dengan bobot 0 untuk entri yang belum `minDay` atau sudah `maxAlive`. Panggil `rng.PickWeighted(weightBuffer)`.

**`SpawnGroup(entry, point)`:**

1. `count = rng.Range(groupMin, groupMax + 1)`.
2. Untuk tiap anggota: `go = ObjectPooler.Instance.SpawnFromPool(entry.enemy.enemyId, posisi, rotasi)`. Anggota lain digeser dalam 2 m lewat `SamplePosition`.
3. Kalau `go == null`, hentikan kelompok ini. Pool memang punya batas.
4. Isi `EnemyInstance`. Pengali gelombang hanya berlaku untuk zombie, lewat `RequestWave`, `RequestBurst`, dan `RequestAmbush`.
5. `WildlifeAI` menerima `homePoint` dan `groupId`.
6. Tambah ke `activeEnemies`, lalu kirim `EnemySpawnedEvent { enemyId, position }`.

**Zombie Desa:** entri dengan `ZombieData.hidesFromSun` tidak memakai `FindSpawnPoint`. Gelombang malam memanggil `HidingSpot.FindEmergeSpot(playerPos)` (#63), sehingga musuh selalu naik dari air atau lumpur.

**Zombie Lumut:** entri `useBushPoints` memilih komponen `BushSpawnPoint` di `Region2_Amazon` yang memenuhi syarat jarak dan pandangan, lalu `SpecialZombieAI` memulainya dari `Dormant` (#25).

### Titik spawn

**`private bool FindSpawnPoint(ref RandomStream rng, out Vector3 point)`**, diulang sampai `attemptsPerSpawn`:

Kalau `WaveRequest.origin` terisi (#90, gelombang `RemoteTakeover`), langkah 1 sampai 2 memakai titik itu sebagai pusat dengan sudut 360 derajat, bukan posisi pemain.

1. **Arah:** kalau `rng.NextFloat01() < behindChance`, sudut acak dalam `behindHalfAngleDeg` di sekitar arah belakang kamera. Kalau tidak, sudut 360 derajat penuh.
2. **Jarak:** `rng.Range(minDistance, maxDistance)`.
3. **NavMesh:** `NavMesh.SamplePosition(kandidat, out hit, sampleRadius, areaMask)`.
4. **Jarak ulang:** jarak `hit.position` harus tetap dalam rentang.
5. **Region:** `RegionStreaming.GetRegionAt(hit.position) == activeRegion`, dan `IsLoaded(activeRegion)`. Ini mencegah musuh muncul di balik jurang atau region terkunci.
6. **Pandangan:** `GeometryUtility.TestPlanesAABB` terhadap frustum, memakai `Plane[6]` yang dihitung sekali per tick. Kalau masuk frustum, titik hanya diterima kalau `PhysicsQuery.Raycast` dengan `PhysicsMasks.AIVision` dari kamera terhalang.
7. **Base dan sarang:** tolak kalau `BaseNavigation.IsInsideBase(hit.position)`, atau kalau titik berada di dalam `InteriorZone` (#112).
8. **Sesak:** tolak kalau `PhysicsQuery.OverlapSphere(titik, 1, PhysicsMasks.BuildOverlap, buffer) > 0`.

Kalau semua gagal, kembalikan `false` dan coba lagi pada tick berikutnya. Syarat pandangan tidak pernah dilonggarkan. `DevConsole` menampilkan jumlah kegagalan lewat `spawnstat`.

**Arah belakang.** `behindChance` 0,75 mengikuti catatan mob di `docs/CATATAN_MEKANIK.md`, yang masih perkiraan. Arah tetap acak, sehingga pemain tetap perlu waspada ke semua sisi.

### Batas dan pool

- **Batas zombie:** `CurrentZombieCap` bernilai `maxZombiesAlive`, atau `capOverride` dari override, paling banyak `maxZombiesOverride`.
- **Batas hewan:** `maxAnimalsAlive`.
- **Ukuran pool:** sama dengan jumlah hidup terbanyak per jenis. Nilai awal: `zombie_standar` 50, `zombie_desa` 30, varian lain 12, Serigala dan Anjing 8, Gagak 16, hewan lain 4.
- **`ValidatePools()`** berjalan saat `Awake` di Editor. Ia mencatat peringatan kalau `pool.size < maxAlive` pada tabel mana pun.
- **`OptimizationManager`** tidak menyimpan daftar sendiri. Ia membaca `SpawnManager.Instance.ActiveEnemies`. `MaxAlive` dipakai untuk kapasitas daftarnya. Musuh yang di-culling tetap dihitung hidup, sehingga batas tidak terlewati. `maxDistance` (45 m) sengaja di bawah `cullingDistance` (50 m), supaya musuh baru tidak langsung mati AI-nya.

### Pengaman macet

**`IEnumerator WatchdogLoop()`**, setiap `stuckCheckSeconds`:

1. Lewati `IsDormantCounted`, state `AttackPlayer`, `AttackBuilding`, `Attack`, `BurnToAshes`, `Dying`, `SeekHiding`, `Emerging`, `Dormant`, dan hewan yang menunggu di perch.
2. Kalau `!agent.isOnNavMesh`, panggil `Despawn(OffNavMesh)` saat di luar pandangan. Kalau tetap terlihat, setelah `stuckForceSeconds`.
3. Kalau bergerak kurang dari `stuckMoveEpsilon`, tambah `stuckTimer`. Kalau tidak, nolkan.
4. Kalau `stuckTimer >= stuckSeconds` dan di luar pandangan, atau `>= stuckForceSeconds`, panggil `Despawn(Stuck)`.
5. Kalau musuh tidak `IsEngaged` dan lebih jauh dari `despawnDistance`, panggil `Despawn(TooFar)`.

Musuh `IsCulled` (`OptimizationManager.md`) melewati langkah 2 sampai 4, karena agennya sengaja dimatikan sehingga `isOnNavMesh` bernilai `false` dan posisinya diam. Langkah 5 tetap berlaku.

Musuh yang dikembalikan tidak mengirim `EnemyKilledEvent`, jadi tidak menghitung statistik dan Lucky Machine. Kalau zombie, `AIDirector.RefundBudget(1)` mengembalikan anggaran malam itu.

### Pindah region

Rekomendasi (lihat bagian 5):

- **Musuh yang `IsEngaged`:** tetap mengejar selama regionnya dimuat.
- **Musuh lain:** tetap ada sampai `TooFar`, yaitu 70 m.
- **Musuh baru:** memakai tabel `activeRegion` baru.
- **Region dilepas (#88):** semua musuh dengan `GetRegionAt(posisi) == region` dikembalikan, walaupun sedang mengejar. HP dan state tidak disimpan.

### Fungsi publik

| Fungsi | Pemanggil | Isi |
|---|---|---|
| `void Despawn(GameObject go, DespawnReason r)` | `EnemyAI`, `WildlifeAI`, #95, #72 | Hapus dari daftar lalu `ReturnToPool`. Satu-satunya jalur kembali di Fase 2. Alasan `Sacrificed` tidak memanggil `RefundBudget` |
| `int RequestWave(in WaveRequest r)` | #27 | Lihat "Gelombang dari Director" |
| `void ReturnStuck(EnemyInstance e)` | #25, #94 | `Despawn(Stuck)` lalu `RefundBudget(1)` |
| `int ReturnRaidersNear(Vector3 center, float radius)` | #90 | `Despawn(go, DespawnReason.TooFar)` untuk zombie di sekitar base. Mengembalikan jumlahnya |
| `int AliveZombies { get; }`, `int AliveLarge { get; }` | #27 | Zombie hidup, tanpa `IsDormantCounted` |
| `void RequestAmbush(Vector3 center, int count, float radius)` | #67, #80 | Memakai tabel malam, cincin `radius`, dan `AISensory.ForceAware`. Berlaku saat tidur. HP dan damage memakai `AIDirector.GetPostTargetModifiers(currentDay)`. `count` tidak dikali di sini, supaya pemulihan #80 tetap memunculkan jumlah yang sama |
| `SpawnOverrideHandle PushOverride(in SpawnOverride o)` | #50, #70 | Lihat di bawah |
| `void PopOverride(SpawnOverrideHandle h)` | #50, #70 | Melepas satu override |
| `void RequestBurst(RegionSpawnTable table, int count, Vector3 center, float radius, in WaveModifiers modifiers)` | #50, #71 | Memunculkan bertahap dengan `maxSpawnsPerFrame`, abaikan `behindChance`, tetap di luar pandangan. `modifiers` diisi seperti `RequestWave`. Tanpa parameter itu, semua pengali bernilai 1 |
| `void DebugSpawn(string enemyId, int count)` | #56 | Di depan pemain, tanpa syarat pandangan |
| `void DebugReturnAll()` | #56 | Semua kembali ke pool |
| `int EngagedEnemyCount { get; }` | #80 | Jumlah `IsEngaged` |

```csharp
public struct SpawnOverride
{
    public RegionSpawnTable table;   // null berarti tabel region
    public bool forceNightTable;     // Pakai malam walaupun siang
    public bool suspendNormalSpawn;  // Hentikan tick biasa
    public bool suspendAnimals;      // Hentikan spawn hewan, misalnya saat erupsi
    public int capOverride;          // 0 berarti batas biasa
    public int desiredOverride;      // 0 berarti ikut #27
}
```

Beberapa override boleh aktif. Yang terakhir didorong menang untuk `table`, nilai terbesar menang untuk `capOverride`, dan tanda `suspend*` digabung dengan OR.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `EnemySpawnedEvent`.

**Event yang didengar:** `DaybreakEvent`, `NightfallEvent`, `RegionChangedEvent`, `RegionLoadStateChangedEvent` (#88), `SleepStateChangedEvent` (#67), `LairClearedEvent` (#112), `RunEndedEvent`.

| Sistem | Hubungan |
|---|---|
| `ZoneManager` | Sumber region aktif lewat `RegionChangedEvent` |
| `RegionStreaming` (#88) | `GetRegionAt`, `IsLoaded`, dan event `Unloading` |
| `AIDirector` (#27) | Memanggil `RequestWave`, membaca `AliveZombies` dan `AliveLarge`, serta menerima `RefundBudget` |
| `HordeManager` (#50), `DisasterManager` (#70) | `PushOverride`, `PopOverride`, dan `RequestBurst` |
| `SunlightExposure`, `HidingSpot` (#63) | Pembakaran dan titik sembunyi |
| `WildlifeAI` (#61), `EnemyAI` | Memanggil `Despawn` |
| `OptimizationManager` | Membaca `ActiveEnemies` dan `MaxAlive` |
| `RunSeedManager` (#89) | Aliran `RandomStreamId.Enemy` |
| `SleepSystem` (#67), `WorldStateSave` (#80) | `RequestAmbush`, `EngagedEnemyCount` |
| `DevConsole` (#56) | `DebugSpawn`, `DebugReturnAll`, `spawnstat` |
| `AITraversal` (#94) | Memanggil `ReturnStuck`, dan membaca `campCounterEnemyId` |
| `LuckyMachine` (#72) | Mengisi `EnemyInstance.IsSacrificing`, dan memanggil `Despawn` dengan alasan `Sacrificed` |
| `RemoteBaseSimulation` (#90) | Memanggil `ReturnRaidersNear`, dan memakai `WaveRequest.origin` |
| `ThreatSystem` (#108), `HordeCrowdManager` (#93) | Memakai musuh yang muncul, tanpa mengatur spawn |

## 4. Kasus Khusus

- **Objek pool:** `EnemyInstance.OnSpawned()` mengosongkan `stuckTimer`, `IsEngaged`, `IsDormantCounted`, `IsSacrificing`, `IsCulled`, `culledAgentWasEnabled`, dan `culledAnimatorWasEnabled`, lalu menyalakan `Animator` (`OptimizationManager.md`). Reset HP mengikuti #59, dan reset agen serta state mengikuti pemilik masing-masing. `SetHPMultiplier` dipanggil setelah objek keluar dari pool.
- **Pool kosong:** `SpawnFromPool` mengembalikan `null`, dan tick berikutnya mencoba lagi.
- **Game dijeda:** semua loop memakai `WaitForSeconds`, jadi berhenti saat `timeScale = 0`. Event transisi tetap diterima, tetapi tidak ada spawn.
- **Tidur:** tick berhenti. Pengaman macet tetap jalan, karena waktu dipercepat.
- **Permadeath:** `ObjectPooler.ReturnAllActive()` (#82) mengembalikan semua, lalu `SpawnManager` hilang bersama scene. Aliran `Enemy` mengikuti seed run baru.
- **Save dan load:** musuh dan daftar tidak disimpan (#88). Saat load, `SpawnManager` membaca event sticky `RegionChangedEvent`, `DaybreakEvent`, dan `NightfallEvent`, lalu mulai dari kosong. Status aliran `Enemy` disimpan #89. Pemulihan pertempuran memakai `RequestAmbush`, dengan `wasInCombat` dan `engagedEnemyCount` (#80).
- **Pemain di dekat batas region terkunci:** syarat `GetRegionAt` menolak titik di seberang jurang.
- **Area NavMesh sempit:** kalau tidak ada titik, spawn ditunda, bukan dipaksa di depan mata.
- **`Despawn` dipanggil dua kali:** objek yang sudah keluar daftar diabaikan.
- **Mayat:** #95 memanggil `Despawn(Killed)` setelah memudar, jadi mayat tetap menghitung batas sampai saat itu. Kalau mayat membuat spawn terhambat, `CountAwake` mengabaikan objek dengan `EnemyHealth.IsAlive == false`.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** nama `SpawnManager` (`GLOSSARY.md`). Zombie tidak berkeliaran di luar ruangan saat siang, tetapi boleh diam di ruang gelap (GDD bagian 3). Jenis musuh per region (GDD bagian 5). Pemilihan jumlah oleh #27. `RandomStreamId.Enemy` (#89). `RequestAmbush` (#67, #80). Musuh dikembalikan saat `Unloading` (#88).

**Keputusan teknis di blueprint ini:**

- `Despawn` menjadi satu-satunya jalur kembali ke pool di Fase 2, sehingga daftar selalu benar.
- `EnemyInstance` memisahkan data spawn dari AI.

**Usulan yang disetujui developer sebagai nilai awal:**

1. **Bobot tabel per region.** Rekomendasi: nilai di bagian 1.
2. **Batas musuh hidup dan target hewan.** Rekomendasi: 30 zombie, 50 saat override, batas 8 hewan, dan target 4 hewan.
3. **Arah kemunculan.** Rekomendasi: `behindChance` 0,75.
4. **Pindah region.** Rekomendasi: musuh yang sedang mengejar tetap mengejar, sisanya dikembalikan setelah 70 m.
5. **Jarak spawn dan pengaman macet.** Rekomendasi: 20 sampai 45 m, 20 detik di luar pandangan, dan 45 detik kalau terlihat.

## 6. Skenario Tes

1. Masuk Region 1 saat siang dengan log aktif, jauh dari sarang. **Hasil:** tick spawn hanya memunculkan Serigala dan Babi Hutan, dengan rasio mendekati 60:40.
2. Majukan jam ke 18:00. **Hasil:** spawn beralih ke `zombie_standar`, dan hewan pergi.
3. Pindah ke Region 3 saat malam. **Hasil:** musuh baru memakai tabel `Snow`.
4. Catat 100 titik spawn. **Hasil:** semuanya 20 sampai 45 m, di luar pandangan, dan sekitar 75 persen dari belakang.
5. Berdiri di dekat tebing. **Hasil:** tidak ada musuh muncul di seberang jurang, dan `spawnstat` mencatat percobaan gagal.
6. Berdiri di dalam base. **Hasil:** tidak ada musuh muncul di dalam.
7. `spawn zombie_standar 100`. **Hasil:** berhenti di ukuran pool, tanpa error.
8. Tahan satu zombie dalam kotak tertutup selama 20 detik, di luar pandangan. **Hasil:** kembali ke pool, tanpa `EnemyKilledEvent`.
9. Mundur dari region sampai `Unloading`. **Hasil:** semua musuh region itu kembali pada frame yang sama.
10. Panggil `RequestWave` dengan `count = 10` dan `maxLarge = 1` saat malam. **Hasil:** 10 zombie muncul bertahap, paling banyak 1 varian besar, dan pengali HP terpasang.
11. `PushOverride` dengan `capOverride = 50`, lalu `PopOverride`. **Hasil:** batas naik, lalu kembali 30. Musuh lama tetap hidup.
12. Tidur sampai gangguan. **Hasil:** tidak ada spawn biasa, dan `RequestAmbush` tetap memunculkan musuh `Aware`. Ulangi pada `setday 70`. **Hasil:** zombie memakai HP 1,2 kali dan damage 1,1 kali.
13. Jalankan dua run dengan seed sama tanpa aksi. **Hasil:** urutan jenis musuh sama.
14. Mati lalu mulai run baru. **Hasil:** `activeEnemies` kosong, dan tidak ada event ganda.
15. Tunggu fajar di dekat gua berisi zombie sarang, lalu tunggu `cleanupSeconds`, di luar pandangan. **Hasil:** `SweepWrongPhase` tidak mengembalikan zombie sarang di dalam `InteriorZone`, dan zombie horde di luar ruangan sudah habis.
