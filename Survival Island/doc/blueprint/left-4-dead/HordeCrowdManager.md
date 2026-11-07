# Blueprint: HordeCrowdManager.cs
**Lokasi:** `Survival Island/doc/blueprint/left-4-dead/HordeCrowdManager.md`
**Deskripsi Utama:** Manager terpusat yang membagi update AI musuh ke beberapa frame, supaya 30 sampai 50 zombie aktif tetap ringan, termasuk saat semuanya berada dekat pemain.
**Issue:** #93
**Rujukan GDD:** Usulan teknis, belum ada di GDD. Mendukung Horde malam di bagian 3 dan "secara masif" di bagian 5.

## 1. Arsitektur & Variabel Utama

`HordeCrowdManager` adalah Singleton di scene `IslandPhase` dan `ShipPhase`. Semua `EnemyAI` berhenti memakai Coroutine sendiri, lalu mendaftar ke manager.

```csharp
public enum CrowdTier { Close, Mid, Far, Culled }

public interface ICrowdAgent
{
    int CrowdIndex { get; set; }
    Vector3 Position { get; }
    bool IsEngaged { get; }             // AttackPlayer, AttackBuilding, atau sedang traversal
    void CrowdTick(float dt);
    void SetTier(CrowdTier tier);
}
```

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `agents` | `ICrowdAgent[]` | Panjang `crowd.capacity` | Daftar agen, dihapus dengan swap-remove |
| `lastTickTime` | `float[]` | 0 | Waktu tick terakhir per agen |
| `tiers` | `CrowdTier[]` | `Mid` | Tingkatan per agen |
| `count` | `int` | 0 | Jumlah agen |
| `cursor` | `int` | 0 | Posisi lanjut round-robin |
| `removeQueue` | `int[]` | Kosong | Agen yang kembali ke pool saat iterasi |
| `AttackBuffer` | `Collider[]` | Panjang `crowd.overlapBufferSize` | Buffer `OverlapSphere` bersama |
| `sharedPath` | `NavMeshPath` | Satu objek | Dipakai ulang untuk jalur kelompok |

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.crowd.capacity` | 64 | Kapasitas array. Mega mob 50 masih muat |
| `GameBalanceConfig.crowd.maxTicksPerFrame` | 12 | Batas `CrowdTick` per frame |
| `GameBalanceConfig.crowd.maxTickMilliseconds` | 2 | Batas waktu AI per frame |
| `GameBalanceConfig.crowd.intervalCloseVisible` | 0,1 | Dekat dan terlihat |
| `GameBalanceConfig.crowd.intervalCloseHidden` | 0,2 | Dekat dan tak terlihat |
| `GameBalanceConfig.crowd.intervalMidVisible` | 0,25 | Sedang dan terlihat |
| `GameBalanceConfig.crowd.intervalMidHidden` | 0,5 | Sedang dan tak terlihat |
| `GameBalanceConfig.crowd.intervalFar` | 1 | Jauh |
| `GameBalanceConfig.crowd.closeDistance` | 15 | Batas tingkat `Close` |
| `GameBalanceConfig.crowd.midDistance` | 30 | Batas tingkat `Mid` |
| `GameBalanceConfig.crowd.tierUpdateSeconds` | 0,25 | Jeda hitung ulang tingkatan |
| `GameBalanceConfig.crowd.repathMinSeconds` | 0,5 | Jeda minimum `SetDestination`, mengganti `pathUpdateDelay` |
| `GameBalanceConfig.crowd.repathMinDistance` / `repathMaxDistance` | 1 / 6 | Batas ambang geser target |
| `GameBalanceConfig.crowd.repathDistanceFactor` | 0,1 | Ambang = jarak ke target x faktor ini |
| `GameBalanceConfig.crowd.maxDestinationsPerFrame` | 4 | Batas `SetDestination` per frame |
| `GameBalanceConfig.crowd.animStaggerMid` / `animStaggerFar` | 2 / 4 | Update Animator setiap sekian frame |
| `GameBalanceConfig.crowd.clusterUpdateSeconds` | 1 | Jeda hitung ulang kelompok |
| `GameBalanceConfig.crowd.clusterCellSize` | 8 | Ukuran sel kelompok, dalam meter |
| `GameBalanceConfig.crowd.clusterMinDistance` | 15 | Kelompok hanya dipakai di atas jarak ini |
| `GameBalanceConfig.crowd.clusterWaypointLookahead` | 12 | Jarak titik tujuan bersama dari kelompok |
| `GameBalanceConfig.crowd.goodAvoidanceCount` | 8 | Agen dengan avoidance lebih baik |
| `GameBalanceConfig.crowd.overlapBufferSize` | 16 | Isi buffer `OverlapSphere` |
| `GameBalanceConfig.crowd.pathfindingIterationsPerFrame` | 150 | Nilai `NavMesh.pathfindingIterationsPerFrame` |

## 2. Detail Logika & Fungsi

### Pendaftaran

**`Register(ICrowdAgent a)`** dipanggil `EnemyAI.OnSpawned`. Agen masuk ke akhir array, dan `lastTickTime` diisi waktu sekarang dikurangi nilai acak dari ID musuh, supaya tick tidak serempak. **`Unregister(a)`** dipanggil `EnemyAI.OnReturned`. Saat iterasi, agen masuk `removeQueue`, dan diproses di akhir `Update`, karena `CrowdTick` bisa mengembalikan musuh ke pool.

### Pembagian update

**`void Update()`**:

1. Kalau `Time.deltaTime == 0`, keluar.
2. Setiap `tierUpdateSeconds`, panggil `UpdateTiers()`.
3. Mulai dari `cursor`, periksa agen satu per satu, paling banyak `count` agen.
4. Kalau `Time.time - lastTickTime[i] >= IntervalOf(i)`, panggil `agents[i].CrowdTick(dt)`, dengan `dt` adalah selisih waktu tersebut, paling besar 1 detik.
5. Berhenti kalau sudah `maxTicksPerFrame`, atau `Stopwatch` melewati `maxTickMilliseconds`. Simpan `cursor`, jadi agen yang tertinggal mendapat giliran pertama pada frame berikutnya.
6. Proses `removeQueue`.

Dengan 50 agen pada 60 FPS, batas 12 tick per frame memberi 720 tick per detik. Kebutuhan terburuknya, 50 agen dengan interval 0,1 detik, hanya 500 tick per detik.

### Frekuensi menurut jarak dan terlihat

**`UpdateTiers()`** menghitung `GeometryUtility.CalculateFrustumPlanes` sekali, lalu untuk tiap agen:

| Kondisi | Tingkatan | Interval |
|---|---|---|
| Sedang menyerang atau `IsEngaged` | `Close` | `intervalCloseVisible` |
| Jarak `<= closeDistance`, di dalam frustum | `Close` | `intervalCloseVisible` |
| Jarak `<= closeDistance`, di luar frustum | `Close` | `intervalCloseHidden` |
| Jarak `<= midDistance`, di dalam frustum | `Mid` | `intervalMidVisible` |
| Jarak `<= midDistance`, di luar frustum | `Mid` | `intervalMidHidden` |
| Jarak lebih dari `midDistance` | `Far` | `intervalFar` |
| Dimatikan `OptimizationManager` | `Culled` | Tidak ada tick |

"Terlihat" berarti berada di dalam frustum, tanpa raycast. Ini cukup murah, dan hasilnya aman kalau salah, karena hanya mengubah frekuensi.

### Update tujuan

**`public bool TrySetDestination(EnemyAI e, Vector3 target)`**:

1. Kalau `Time.time - e.lastRepathTime < repathMinSeconds`, kembalikan `false`.
2. Hitung `threshold = Clamp(jarak ke target * repathDistanceFactor, repathMinDistance, repathMaxDistance)`.
3. Kalau jarak antara `target` dan `e.lastDestination` kurang dari `threshold`, kembalikan `false`.
4. Kalau `destinationsThisFrame >= maxDestinationsPerFrame`, kembalikan `false`. Agen mencoba lagi pada tick berikutnya.
5. Panggil `agent.SetDestination(target)`, lalu simpan `lastDestination` dan `lastRepathTime`.

### Jalur bersama untuk kelompok

Unity tidak mengizinkan satu `NavMeshPath` dipakai agen dari posisi berbeda dengan aman. Jadi kelompok berbagi titik tujuan, bukan jalurnya.

1. Setiap `clusterUpdateSeconds`, agen di atas `clusterMinDistance` dipetakan ke sel ukuran `clusterCellSize`.
2. Agen terdekat ke pemain di sel itu menjadi pemimpin. `NavMesh.CalculatePath` memakai `sharedPath`, lalu titik pada jarak `clusterWaypointLookahead` disimpan.
3. Anggota memakai titik itu lewat `TrySetDestination`. Jalurnya pendek, sehingga pencarian lebih cepat.
4. Di bawah `clusterMinDistance`, agen memakai tujuan sendiri, supaya bisa mengepung.

Agen dengan `areaMask` berbeda (#94) tidak disatukan dalam satu kelompok.

### Avoidance

**`obstacleAvoidanceType`**, diatur saat `UpdateTiers`:

| Agen | Nilai |
|---|---|
| `goodAvoidanceCount` agen terdekat | `MedQualityObstacleAvoidance` |
| Lainnya di `Close` dan `Mid` | `LowQualityObstacleAvoidance` |
| `Far` | `NoObstacleAvoidance` |

`HighQuality` dan `GoodQuality` tidak dipakai untuk musuh.

**`avoidancePriority`** (0 paling diutamakan):

| Agen | Nilai |
|---|---|
| Varian besar | 20 |
| Sedang menyerang | 30 |
| Mengejar | 50 |
| Menunggu di `waitRingRadius` (#78) | 70 |
| NPC (#76) | 80 |

### Query fisika

- Serangan `EnemyAI` dan `EnemyAttack` (#109) memakai `PhysicsQuery.OverlapSphere(position, radius, mask, HordeCrowdManager.Instance.AttackBuffer)`, yang di dalamnya memanggil `OverlapSphereNonAlloc`, jadi tidak menghasilkan garbage. Buffer dipakai bergantian di thread utama, jadi aman dibagi. Hasilnya tidak boleh disimpan setelah tick selesai.
- Kalau jumlah hasil sama dengan panjang buffer, `DevConsole` mencatat peringatan.
- Raycast obstruction lama dihapus. Pemeriksaan jalur diatur `BaseNavigation.maxPathChecksPerFrame` (#78).

### Animasi

**`EnemyAI.SetTier(tier)`**:

| Tingkatan | `cullingMode` | Update |
|---|---|---|
| `Close` | `CullUpdateTransforms` | Setiap frame |
| `Mid` | `CullUpdateTransforms` | `animator.enabled = false`, lalu `animator.Update(akumulasi)` setiap `animStaggerMid` frame, bergantian menurut ID |
| `Far` | `CullCompletely` | Sama, setiap `animStaggerFar` frame |
| `Culled` | `CullCompletely` | `animator.enabled = false` |

- `SkinnedMeshRenderer.updateWhenOffscreen = false`, dan `quality` 2 tulang pada `Far`.
- Prefab memakai `LODGroup` dengan tiga tingkat. Material memakai GPU Instancing kalau tersedia.
- Saat Animator mati, damage memakai timer cadangan di #109.

### Target performa (usulan)

Target hardware: PC menengah, 1080p, pengaturan Medium.

| Skenario | FPS rata-rata | FPS minimum (1 persen terendah) | Waktu AI per frame | GC dari AI |
|---|---|---|---|---|
| 30 zombie | 60 | 45 | 3 ms | 0 byte |
| 50 zombie | 45 | 30 | 5 ms | 0 byte |

"Waktu AI" adalah penanda Profiler `HordeCrowd.Tick`, ditambah `NavMeshManager`.

### Uji stres lewat #56

Manager mendaftarkan perintah berikut lewat registry `DevConsole`:

| Perintah | Isi |
|---|---|
| `stress <jumlah> [radius]` | Memanggil `SpawnManager.DebugSpawn` melingkari pemain, default radius 20 meter |
| `crowdstat <0/1>` | Menampilkan jumlah per tingkatan, tick per detik, `SetDestination` per detik, dan waktu tick |
| `crowdai <0/1>` | Mematikan pembagian update, untuk membandingkan dengan perilaku lama |

**Skenario uji:**

| No | Lokasi | Perintah | Yang diukur |
|---|---|---|---|
| A | Region 1, tanah terbuka | `stress 30` | FPS dan waktu AI selama 60 detik |
| B | Region 1, tanah terbuka | `stress 50` | Sama |
| C | Base 4 dinding, pemain di dalam | `stress 30` | Carving, `BreachSlots`, dan posisi tunggu |
| D | Region 2, hutan rapat | `stress 50` | Avoidance di antara pohon |
| E | Sambil berlari memutar | `stress 50` | Jumlah `SetDestination` per detik |

Hasil dicatat di `Survival Island/doc/PLAYTEST_LOG.md`.

## 3. Hubungan dengan Sistem Lain

**Tidak mengirim atau mendengar event.** Manager dipanggil langsung oleh dependensinya.

| Sistem | Hubungan |
|---|---|
| `EnemyAI` | Memakai `ICrowdAgent`. Coroutine `AIBehaviorLoop` diganti `CrowdTick` |
| `OptimizationManager` | Tetap pemilik culling jarak jauh, dan membaca daftar agen dari manager. Memanggil `SetTier(Culled)` |
| `SpawnManager` (#62), `AIDirector` (#27) | Menentukan jumlah. Manager hanya mengikuti |
| `BaseNavigation` (#78) | Tidak mengubah `BreachSlots`, dan memakai state untuk tingkatan |
| `EnemyAttack` (#109) | Memakai `AttackBuffer`, dan timer cadangan saat Animator mati |
| `ThreatSystem` (#108) | Dijalankan di dalam `CrowdTick`, jadi mengikuti tingkatan |
| `SpecialZombieAI` (#25) | `TickSpecial` dipanggil di dalam `CrowdTick` |
| `AITraversal` (#94) | `IsEngaged = true` selama melewati `NavMeshLink` |

## 4. Kasus Khusus

- **Dipakai ulang dari pool:** `Register` di `OnSpawned` mengatur `lastDestination` ke nilai tak terhingga, tingkatan `Mid`, avoidance bawaan prefab, dan `animator.enabled = true`. `OnReturned` selalu memanggil `Unregister`.
- **Game dijeda:** `Time.deltaTime == 0`, jadi tidak ada tick. Selisih waktu dibatasi 1 detik setelah jeda selesai.
- **Permadeath dan save:** tidak ada data yang disimpan. Manager hilang bersama scene. `EnemyAI` harus aman kalau `Instance` kosong saat scene berakhir.
- **Kapasitas penuh:** `Register` mencatat kesalahan dan mengembalikan `false`, lalu `EnemyAI` kembali ke pool. Ini tanda `maxAlive` melewati kapasitas.
- **Frame berat:** `maxTickMilliseconds` menghentikan tick lebih cepat. Agen yang tertinggal hanya bereaksi sedikit lambat.
- **`BurnToAshes` serempak saat fajar:** efek terbakar dibagi #63, dan tick tetap dibatasi.
- **`timescale` dari konsol:** interval memakai `Time.time`, jadi ikut melambat atau cepat.
- **Tingkatan berubah saat menyerang:** `IsEngaged` memaksa `Close`, supaya serangan tidak tersendat.

## 5. Keputusan Desain

**Keputusan teknis di blueprint ini (usulan, dicatat di #85):** pembagian update memakai round-robin dengan batas per frame, dan "terlihat" memakai frustum.

**Usulan yang disetujui developer sebagai nilai awal (dicatat di #85):**

1. **Jalur bersama:** rekomendasi dipakai dalam bentuk titik tujuan bersama di atas 15 meter, bukan satu `NavMeshPath`. Hidupkan hanya kalau skenario B atau E belum memenuhi target.
2. **Target performa:** rekomendasi tabel di bagian 2, dengan PC menengah sebagai patokan. Developer perlu menentukan spesifikasi minimum sebenarnya.

## 6. Skenario Tes

1. Jalankan skenario A. **Hasil:** memenuhi target 30 zombie.
2. Jalankan skenario B. **Hasil:** memenuhi target 50 zombie.
3. Buka Profiler pada skenario B, Deep Profile mati. **Hasil:** alokasi GC dari `HordeCrowd.Tick` bernilai 0.
4. `crowdstat 1` pada skenario E. **Hasil:** `SetDestination` di bawah 100 per detik. Perilaku lama dengan 50 zombie mencapai 250 per detik.
5. Berdiri diam dikelilingi 30 zombie. **Hasil:** hampir tidak ada `SetDestination` setelah semua sampai, dan semua tetap menyerang.
6. Putar kamera membelakangi kerumunan. **Hasil:** `crowdstat` menunjukkan interval lebih panjang, dan zombie tetap maju.
7. Bunuh 30 zombie dengan cepat, ulangi `stress 30` tiga kali. **Hasil:** jumlah agen kembali 0 setiap kali, tanpa error dan tanpa agen ganda.
8. Jeda game di tengah skenario B selama 10 detik. **Hasil:** tidak ada tick, dan tidak ada lonjakan saat lanjut.
9. `crowdai 0`, ulangi skenario B. **Hasil:** selisih waktu AI terlihat dan tercatat.
10. Pada skenario C, amati zombie di depan dinding. **Hasil:** tidak bertumpuk pada satu titik, dan yang menunggu memberi jalan.

## 7. Catatan Implementasi (Langkah 5)

Kode: `left-4-dead/HordeCrowdManager.cs` (`CrowdTier`, `ICrowdAgent`).

- Belum ada: titik tujuan bersama per kelompok, update Animator bergiliran per tingkatan, dan perintah `stress`/`crowdstat`/`crowdai` (#56). Avoidance per tingkatan diatur `EnemyAI.SetTier` dengan aturan sederhana (Close = Med, Mid = Low, Far = None).

## Catatan Implementasi (Sistem pendukung)

- Isi TODO (sistem pendukung): Animasi bergiliran ditulis di `EnemyAI.SetTier` dan `LateUpdate`: `Close` Animator jalan sendiri; `Mid`/`Far` Animator dimatikan dan `animator.Update(akumulasi)` dipanggil setiap `animStaggerMid`/`animStaggerFar` frame, bergantian menurut `GetInstanceID`; `Culled` mati. Saat `Dying`, Animator diserahkan ke ragdoll `EnemyHitReaction`.
- Isi TODO (sistem pendukung): Masih TODO: titik tujuan bersama per kelompok (angka `cluster*` belum masuk config) dan perintah `stress`, `crowdstat`, `crowdai`. (Diganti keputusan developer 7 November 2026, lihat di bawah.)
- **Keputusan developer (7 November 2026):** Kelompok zombie jauh (#93) diimplementasikan. Field config: `crowd.clusterUpdateSeconds` 1, `clusterCellSize` 8, `clusterMinDistance` 15, `clusterWaypointLookahead` 12. Setiap `clusterUpdateSeconds`, agen `EnemyAI` (bukan `Culled`, di NavMesh) yang lebih jauh dari `clusterMinDistance` dipetakan ke sel `(x, z, areaMask)`; pemimpin = agen terdekat ke pemain di sel itu. Satu `NavMesh.CalculatePath` per kelompok dari pemimpin ke posisi pemain (di-`SamplePosition` dengan radius `clusterCellSize`) memakai `sharedPath`, sudutnya dibaca dengan `GetCornersNonAlloc` ke buffer tetap, lalu titik pada jarak `clusterWaypointLookahead` disimpan. Semua buffer dibuat di `Awake`, jadi tidak ada alokasi per frame. `TrySetDestination` mengganti tujuan anggota dengan titik itu. Tafsiran: penggantian hanya untuk tujuan yang "mengejar pemain" (tujuan dalam satu `clusterCellSize` dari pemain), sehingga posisi serang dinding dan titik traversal tidak terganggu; pemimpin juga memakai titik itu. Di bawah `clusterMinDistance` agen memakai tujuan sendiri.
- **Keputusan developer (7 November 2026):** Perintah DevConsole (di dalam `#if UNITY_EDITOR || DEVELOPMENT_BUILD`, didaftarkan di `Start`, handler membaca `Instance`): `stress <jumlah> [radius]` memanggil `SpawnManager.DebugSpawnRing("zombie_standar", jumlah, radius)` (fungsi baru, melingkari pemain, default radius 20 m, menandai run `[CHEAT]`); `crowdstat <0/1>` mencetak tiap detik ke DevConsole jumlah per tingkatan, jumlah kelompok, tick/s, `SetDestination`/s, serta waktu tick rata-rata dan maksimum; `crowdai <0/1>` dengan 0 men-tick semua agen setiap frame tanpa batas `maxTicksPerFrame`/`maxTickMilliseconds` dan mematikan kelompok (tafsiran "perilaku lama": batas `TrySetDestination` tetap berlaku). TODO lama #93 di bagian ini selesai.
