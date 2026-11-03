# Blueprint: AITraversal.cs
**Lokasi:** `docs/blueprint/left-4-dead/AITraversal.md`
**Deskripsi Utama:** Aturan musuh melewati `NavMeshLink`, mengenali pemain yang tak terjangkau, dan mencegah pemain aman selamanya di atas batu atau atap.
**Issue:** #94
**Rujukan GDD:** Usulan, belum ada di GDD. Mendukung ancaman malam di bagian 3 dan aturan fungsi base di bagian 4. `EnemyAI` wajib memakai `NavMeshAgent`, sesuai `Claude.md`.

## 1. Arsitektur & Variabel Utama

| Kelas | Letak | Fungsi |
|---|---|---|
| `AITraversal` | Singleton di `IslandPhase` | Memeriksa keterjangkauan, memilih respons, dan menghitung berkemah |
| `TraversalAgent` | Prefab musuh, `IPoolable` | Melewati link, dan menjaga musuh tetap di NavMesh |
| `TraversalLinkGate` | Di objek `NavMeshLink` | Membatasi pengguna link |

```csharp
public enum ReachResponse { None, Surround, Retreat, RockThrow, AttackStructure }
```

### Area NavMesh

| Area | Nomor | Biaya | Arti |
|---|---|---|---|
| `Walkable` | 0 | 1 | Bawaan |
| `JumpLink` | 3 | 2 | Melompati celah atau naik tepi sampai 1,2 m |
| `ClimbLink` | 4 | 4 | Memanjat sampai 3 m, tangga, dan tali |
| `HighClimbLink` | 5 | 5 | Memanjat 3 sampai 4 m |
| `DropLow` | 6 | 1,5 | Turun satu arah sampai 2 m |
| `DropMid` | 7 | 1,5 | Turun satu arah sampai 4 m |
| `DropHigh` | 8 | 1,5 | Turun satu arah sampai 8 m |

`NavMeshLink` diletakkan tangan di scene region, dan area link menyatakan tingginya. Link antar-region tetap milik #88. Setiap `NavMeshAgent` mendapat `areaMask = EnemyData.traversalMask` dan `autoTraverseOffMeshLink = false`.

### Kemampuan per jenis

Mengikuti usulan #25, Runner berarti Zombie Desa, dan Tank berarti Zombie Militer.

| Jenis | Isi `traversalMask` selain `Walkable` | Respons jarak jauh |
|---|---|---|
| Standar, Beku, Lumut | `JumpLink`, `DropLow`, `DropMid` | Batu, saat jumlah pelempar belum penuh |
| Tribal | `JumpLink`, `ClimbLink`, `DropLow`, `DropMid` | Jerat (#25) |
| Desa (Runner) | Semua area link | Batu |
| Militer (Tank) | `DropLow` | Batu, saat jumlah pelempar belum penuh |
| Hewan | Diatur per `AnimalData` (#61) | `TargetUnreachable` (#61) |

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.traversal.probeIntervalSeconds` | 1,5 | Jeda pemeriksaan keterjangkauan |
| `GameBalanceConfig.traversal.maxProbesPerFrame` | 2 | Batas `NavMesh.CalculatePath` milik sistem ini per frame |
| `GameBalanceConfig.traversal.campShotWindowSeconds` | 5 | Lama timer berkemah dipercepat setelah menembak |
| `GameBalanceConfig.traversal.linkAvoidSeconds` | 5 | Lama link yang penuh dihindari |
| `GameBalanceConfig.traversal.retreatMaxTries` | 3 | Titik mundur yang dicoba per tick |
| `GameBalanceConfig.traversal.reachTolerance` | 2,5 | Jarak ujung jalur ke pemain yang dianggap terjangkau |
| `GameBalanceConfig.traversal.sampleRadius` | 1,5 | Radius `NavMesh.SamplePosition` di sekitar pemain |
| `GameBalanceConfig.traversal.surroundAfterSeconds` | 3 | Mulai mengepung |
| `GameBalanceConfig.traversal.rockAfterSeconds` | 10 | Mulai melempar batu |
| `GameBalanceConfig.traversal.retreatAfterSeconds` | 20 | Zombie tanpa serangan jarak jauh mundur |
| `GameBalanceConfig.traversal.surroundRadius` | 6 | Radius kepungan |
| `GameBalanceConfig.traversal.maxRockThrowers` | 3 | Pelempar batu sekaligus |
| `GameBalanceConfig.traversal.rockRangeMin` / `rockRangeMax` | 5 / 25 | Jarak lemparan |
| `GameBalanceConfig.traversal.rockCooldownSeconds` | 8 | Jeda tiap pelempar |
| `GameBalanceConfig.traversal.campMoveDistance` | 5 | Pemain dianggap pindah di atas jarak ini |
| `GameBalanceConfig.traversal.campShotMultiplier` | 2 | Timer berkemah lebih cepat setelah menembak |
| `GameBalanceConfig.traversal.campTier1Seconds` | 45 | Hukuman pertama |
| `GameBalanceConfig.traversal.campTier2Seconds` | 90 | Hukuman kedua |
| `GameBalanceConfig.traversal.campRepeatSeconds` | 60 | Jeda pengulangan setelah hukuman kedua |
| `GameBalanceConfig.traversal.campCounterCount` | 2 | Jumlah varian pembalas |
| `GameBalanceConfig.traversal.offNavGraceSeconds` | 1 | Toleransi di luar NavMesh |
| `GameBalanceConfig.traversal.offNavSampleRadius` | 3 | Radius pencarian kembali |
| `GameBalanceConfig.traversal.linkUsersDefault` | 1 | Pengguna link sekaligus |
| `GameBalanceConfig.traversal.linkWaitSeconds` | 3 | Lama menunggu link sebelum mencari jalur lain |
| `GameBalanceConfig.traversal.jumpSeconds` | 0,6 | Lama melompat |
| `GameBalanceConfig.traversal.climbSpeed` | 1,5 | Kecepatan memanjat, m/s |

## 2. Detail Logika & Fungsi

### Melewati `NavMeshLink`

**`TraversalAgent.Tick(float dt)`**, dipanggil dari `EnemyAI.CrowdTick`, karena `autoTraverseOffMeshLink` dimatikan:

1. Kalau `agent.isOnOffMeshLink` dan belum melintas, ambil `currentOffMeshLinkData`, lalu baca `TraversalLinkGate` dari `owner`.
2. `gate.TryEnter(enemyId)`. Kalau penuh, tunggu sampai `linkWaitSeconds`. Lewat dari itu, panggil `agent.ResetPath()`, lalu naikkan biaya area itu selama `linkAvoidSeconds` dengan `agent.SetAreaCost`, supaya jalur lain dipilih.
3. Mulai `Traverse(start, end, kind)`. `agent.updatePosition = false`, dan `IsEngaged = true`.
4. Gerakkan `transform.position` tiap frame:
   - **Jump:** busur parabola selama `jumpSeconds`.
   - **Climb** (`ClimbLink`, `HighClimbLink`): garis lurus dengan `climbSpeed`, animasi memanjat.
   - **Drop** (`DropLow`, `DropMid`, `DropHigh`): busur dengan gravitasi. Musuh tidak menerima damage jatuh.
5. Selesai: `agent.CompleteOffMeshLink()`, `agent.Warp(end)`, `agent.updatePosition = true`, lalu `gate.Exit(enemyId)`.

**Terkena damage saat melintas:** musuh tetap melintas sampai selesai, karena stagger (#95) hanya berlaku di tanah. Kalau mati, ragdoll atau mayat langsung diambil alih #95, dan gate dilepas.

### Mengenali pemain yang tak terjangkau

Satu pemeriksaan untuk tiap nilai `traversalMask`, bukan untuk tiap musuh, supaya murah saat ada 50 zombie.

**`private void ProbeReachability()`**, setiap `probeIntervalSeconds`:

1. Kumpulkan `traversalMask` unik dari musuh yang `Chase` (#93 sudah punya daftarnya). Untuk tiap mask, ambil musuh terdekat sebagai titik asal.
2. Kalau `NavMesh.SamplePosition(pemain, out hit, sampleRadius, mask)` gagal, mask itu tak terjangkau. Selesai.
3. `NavMesh.CalculatePath(asal, hit.position, mask, sharedPath)`, paling banyak `maxProbesPerFrame`.
4. Terjangkau kalau `status == PathComplete` dan jarak titik terakhir ke pemain `<= reachTolerance`. Syarat kedua penting, karena pemain di atas batu bisa menghasilkan jalur lengkap yang berakhir di kaki batu.
5. `unreachableTimer[mask]` bertambah kalau tak terjangkau, dan menjadi 0 kalau terjangkau.

**`IsPlayerInsideBase == true` (#78) tidak dihitung tak terjangkau.** Base adalah tempat berlindung di GDD, dan zombie sudah punya `AttackBuilding`.

### Respons bertahap

**`public ReachResponse GetResponse(EnemyAI e)`** dibaca `EnemyAI` saat `Chase`:

| Waktu tak terjangkau | Syarat | Respons |
|---|---|---|
| Kapan saja | `TryAssignBreachTarget` berhasil (#78) | `AttackStructure`, memakai `AttackBuilding` |
| `>= surroundAfterSeconds` | Tidak ada struktur | `Surround` |
| `>= rockAfterSeconds` | Punya jatah, garis pandang bebas, dan jarak sesuai | `RockThrow` |
| `>= retreatAfterSeconds` | Bukan pelempar batu dan bukan Tribal | `Retreat` |

**Mencari jalur lain** sudah berjalan sendiri, karena `CalculatePath` dan `SetDestination` memakai `areaMask` yang berisi link.

- **`Surround`:** tujuan adalah titik pada lingkaran `surroundRadius`, dengan sudut dari ID musuh, lewat `NavMesh.SamplePosition` dan `HordeCrowdManager.TrySetDestination`.
- **`RockThrow`:** `AttackData` `Zombie_Rock_Throw` (#109), damage 10, infeksi 0, ancang-ancang 1 detik, dan arah memperkirakan gerak pemain. `SpawnFromPool("Thrown_Rock", ...)` di layer `Projectile`. Kena pemain atau struktur, lalu kembali ke pool. Jatah `maxRockThrowers` dikembalikan saat musuh mati atau pool.
- **`Retreat`:** tujuan adalah titik di luar garis pandang pemain, dicek dengan `PhysicsMasks.AIVision`, paling banyak `retreatMaxTries` titik per tick. Zombie tidak lagi menjadi sasaran gratis. Saat pemain turun, timer menjadi 0, dan semua kembali `Chase`.

### Atap bangunan pemain

Zombie **tidak** bisa naik ke atap. `BuildManager` tidak membuat `NavMeshLink`, dan NavMesh hanya ada di tanah, sesuai `BaseNavigation.md`.

Pemain di atap tidak aman, karena `TryAssignBreachTarget` menemukan dinding di bawahnya. Saat dinding hancur, atap runtuh lewat grafik dukungan (#79), dan pemain jatuh dengan damage jatuh (#101). Tangga buatan pemain juga tidak membuat link.

### Hukuman berkemah

**`campTimer`** berjalan kalau ada `unreachableTimer` milik mask `zombie_standar` di atas 0, pemain tidak di dalam base, dan saat itu malam. Timer bertambah `dt`, atau `dt * campShotMultiplier` selama `campShotWindowSeconds` setelah `AmmoChangedEvent` berkurang. Timer menjadi 0 kalau pemain terjangkau, atau berpindah lebih dari `campMoveDistance` ke titik yang terjangkau.

| Waktu | Hukuman |
|---|---|
| `campTier1Seconds` | `maxRockThrowers` dikali 2 sampai timer kembali 0 |
| `campTier2Seconds` | `AIDirector.Instance.RequestCampCounter(enemyId, campCounterCount)` |
| Setiap `campRepeatSeconds` sesudahnya | Mengulang tier 2 |

`enemyId` pembalas diambil dari `RegionSpawnTable.campCounterEnemyId` (#62):

| Region | Pembalas | Alasan |
|---|---|---|
| 1 | `zombie_standar` | GDD hanya menyebut zombie standar |
| 2 | `zombie_tribal` | Jerat menarik pemain turun |
| 3 | `zombie_militer` | Darah tebal dan lemparan batu |
| 4 | `zombie_desa` | Bisa memanjat |

### Jatuh dari NavMesh

**`TraversalAgent.Tick`**, kalau `agent.enabled`, tidak sedang melintas, dan `!agent.isOnNavMesh`:

1. `offNavTimer += dt`. Kalau kurang dari `offNavGraceSeconds`, keluar.
2. `NavMesh.SamplePosition(pos, out hit, offNavSampleRadius, mask)`. Kalau berhasil, `agent.Warp(hit.position)`, lalu `offNavTimer = 0`.
3. Kalau gagal, atau posisi di bawah `Terrain`, panggil `SpawnManager.Instance.ReturnStuck(enemy)`. Tidak ada `EnemyKilledEvent`, drop, atau kuota Lucky Machine.

### Daftar titik playtest

Lokasi berikut berasal dari GDD, dan koordinatnya diisi saat level selesai. Tiap titik dicoba dengan perintah `reachcheck`, yang menampilkan hasil pemeriksaan tiap mask.

| Region | Titik | Yang dicoba | Hasil benar |
|---|---|---|---|
| 1 | Batu besar di hutan pinus | Berdiri dan menembak | Kepungan, batu, lalu pembalas |
| 1 | Lereng dan tepi kawah gunung berapi | Melompat ke tepi | Terjangkau, atau dilindungi lereng curam (#101) |
| 1 | Pohon tumbang dan tumpukan batu | Berdiri di atasnya | Sama seperti batu besar |
| 2 | Tepi jurang dan kedua ujung jembatan | Berdiri di ujung | Zombie melewati jembatan |
| 2 | Akar besar dan tebing | Berdiri di atasnya | Tribal memanjat atau menjerat |
| 3 | Atap bangunan pangkalan militer | Naik lewat tangga | Tangga punya `ClimbLink`, atau ada pembalas |
| 3 | Helipad dan kerangka helikopter | Berdiri di kerangka | Zombie menjangkau pemain, atau melempar batu |
| 3 | Menara dan tepian es | Berdiri dan menembak | Sama seperti batu besar |
| 4 | Atap rumah reruntuhan | Melompat dari reruntuhan | Desa memanjat, kalau ada link |
| 4 | Tepi sungai dan danau | Berdiri di pulau kecil | Pembalas muncul, karena air bukan zona aman |
| Base | Atap dan lantai atas | Menembak dari atap | Dinding diserang, lalu atap runtuh |
| Base | Tangga bangunan | Berdiri di ujung tangga | Tidak ada link, dan dinding diserang |

## 3. Hubungan dengan Sistem Lain

**Event yang didengar:** `NightfallEvent`, `DaybreakEvent`, dan `AmmoChangedEvent`. **Event yang dikirim:** tidak ada.

| Sistem | Hubungan |
|---|---|
| `EnemyAI` | Memanggil `TraversalAgent.Tick`, dan membaca `GetResponse` |
| `BaseNavigation` (#78) | `IsPlayerInsideBase` dan `TryAssignBreachTarget`. Serangan dinding tetap miliknya |
| `HordeCrowdManager` (#93) | `TrySetDestination`, daftar agen, dan `IsEngaged` |
| `ThreatSystem` (#108) | Membaca `GetResponse` saat target tak terjangkau |
| `SpawnManager` (#62) | `ReturnStuck` dan `RegionSpawnTable.campCounterEnemyId` |
| `AIDirector` (#27) | `RequestCampCounter` |
| `EnemyAttack` (#109) | `Zombie_Rock_Throw` |
| `WildlifeAI` (#61) | Hewan tidak memakai respons ini, tetapi memakai `TraversalAgent` untuk link |
| `RegionStreaming` (#88) | Link antar-region, dan `Warp` setelah region dimuat |
| `DevConsole` (#56) | Mendaftarkan `reachcheck` dan `navlinks <0/1>` |

## 4. Kasus Khusus

- **Dipakai ulang dari pool:** `OnSpawned` memeriksa `agent.isOnNavMesh`, mengatur `updatePosition = true`, `offNavTimer = 0`, dan `IsEngaged = false`. `OnReturned` melepas gate dan jatah batu, serta mengembalikan `SetAreaCost`.
- **Game dijeda:** semua timer memakai `Time.deltaTime` atau `dt` dari `CrowdTick`, jadi berhenti. Musuh yang sedang melintas juga berhenti.
- **Permadeath dan save:** tidak ada yang disimpan. `campTimer` dan timer tak terjangkau kembali 0 saat load, dan musuh tidak disimpan (#88).
- **Siang hari:** `campTimer = 0`. Zombie sudah terbakar, dan hewan memakai #61.
- **Tidur:** `campTimer` berhenti selama `SleepStateChangedEvent.isSleeping`.
- **Pemain tertahan (#110) atau berenang (#13):** tidak dihitung berkemah.
- **Pemain membawa Heavy Item:** aturan sama. Membawa barang tidak mengubah keterjangkauan.
- **Struktur baru memutus jalur:** carving (#78) membuat `PathPartial`, lalu `AttackStructure` menang.
- **Link pada region yang dilepas:** musuh di region itu sudah dikembalikan (#88), jadi gate dikosongkan saat `OnDisable`.
- **Pemain berdiri di atas musuh:** kapsul `Enemy` bertabrakan dengan pemain. Pemeriksaan tetap memakai titik NavMesh terdekat, jadi tetap terjangkau.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** NavMesh hanya di tanah (#88), zombie tidak memanjat dinding (#78), dan musuh macet kembali ke pool (`docs/REFERENSI_MUSUH_L4D.md` bagian 8).

**Usulan yang disetujui developer sebagai nilai awal (dicatat di #85):**

1. **Kemampuan per jenis:** rekomendasi tabel di bagian 1. Hanya Desa dan Tribal yang memanjat.
2. **Respons:** rekomendasi bertahap, mulai dari mengepung, melempar batu, lalu mundur. Struktur diserang kalau ada.
3. **Atap:** rekomendasi tidak bisa dinaiki, sesuai `BaseNavigation.md`. Atap diatasi lewat menghancurkan dinding.
4. **Melempar batu:** rekomendasi dipakai semua zombie dengan batas 3 pelempar, meski GDD tidak menyebutnya, karena tanpa ini pemain di atas batu tetap aman.
5. **Hukuman berkemah:** rekomendasi 45 dan 90 detik, dengan varian pembalas per region, dan tidak berlaku di dalam base.

## 6. Skenario Tes

1. Letakkan `JumpLink` dan `ClimbLink` di tebing uji. `spawn zombie_standar 1`. **Hasil:** melompati celah, tidak memanjat.
2. `spawn zombie_desa 1` di tempat yang sama. **Hasil:** memanjat dan sampai ke pemain.
3. `spawn zombie_militer 1`. **Hasil:** memutar lewat jalur lain, atau mengepung.
4. Berdiri di atas batu tanpa link, `spawn zombie_standar 5`. **Hasil:** `reachcheck` menyatakan tak terjangkau, zombie mengepung setelah 3 detik, batu terlempar setelah 10 detik, dan sisanya mundur setelah 20 detik.
5. Terus menembak dari batu. **Hasil:** tier 2 tercapai sekitar 45 detik kalau terus menembak, dan varian pembalas muncul.
6. Turun dari batu. **Hasil:** semua kembali `Chase`, dan `campTimer = 0`.
7. Berdiri di atap base. **Hasil:** zombie menyerang dinding, dinding hancur, atap runtuh, dan pemain menerima damage jatuh.
8. Masuk ke base tertutup. **Hasil:** tidak ada hukuman berkemah, dan zombie memakai `AttackBuilding`.
9. Dorong musuh keluar NavMesh lewat `tp`, atau letakkan di tempat tanpa NavMesh. **Hasil:** kembali lewat `Warp`, atau kembali ke pool tanpa `EnemyKilledEvent`.
10. Sepuluh zombie menuju satu link. **Hasil:** bergantian, dan sebagian memilih jalur lain setelah 3 detik.
11. Jeda game saat zombie melompat. **Hasil:** berhenti di udara, lalu lanjut.
12. Bunuh zombie yang sedang memanjat, lalu pakai ulang dari pool. **Hasil:** gate kosong, dan `updatePosition = true`.
13. Ulangi daftar titik playtest pada tiap region. **Hasil:** tidak ada titik yang aman selamanya.
