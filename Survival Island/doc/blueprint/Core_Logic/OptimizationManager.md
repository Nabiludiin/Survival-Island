# Blueprint: OptimizationManager.cs
**Lokasi:** `Survival Island/doc/blueprint/Core_Logic/OptimizationManager.md`
**Deskripsi Utama:** Mengatur performa game dengan mematikan AI dan animasi musuh yang berada jauh dari pemain (culling jarak jauh). Pembagian tick dan tingkatan jarak dekat dimiliki `HordeCrowdManager` (#93).
**Issue:** #62, #90, #93
**Rujukan GDD:** Usulan teknis, belum ada di GDD. Mendukung horde zombie malam di GDD bagian 2 tanpa lag.

## 1. Arsitektur & Variabel Utama

### Pola

`OptimizationManager` adalah manager per scene di `IslandPhase`. Fase 1 tidak memakainya, karena `ShipPhase` tidak punya `SpawnManager`. Manager ini tidak menyimpan daftar musuh sendiri, dan tidak menyimpan apa pun ke save.

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Instance` | `OptimizationManager` | `null` | Singleton per scene |
| `player` | `Transform` | `null`, diisi di `OnEnable` | Pusat jarak. Diambil dari pemain Fase 2 di `IslandPhase` |
| `loop` | `Coroutine` | `null` | `CullingLoop` yang berjalan |
| `culledCount` | `int` | 0 | Jumlah musuh yang sedang di-culling. Hanya untuk `DevConsole` dan Profiler |

**Dihapus:** `activeEnemies` milik sendiri. Daftar musuh dibaca dari `SpawnManager.Instance.ActiveEnemies`, dan `MaxAlive` dipakai untuk kapasitas (#62). Agen dan tingkatannya dibaca lewat `HordeCrowdManager` (#93).

### Status culling per musuh

Status culling disimpan di musuh, bukan di manager, supaya ikut direset saat musuh kembali ke pool:

| Field di `EnemyInstance` | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `IsCulled` | `bool` | `false` | Sedang di-culling |
| `culledAgentWasEnabled` | `bool` | `false` | Status `NavMeshAgent.enabled` sebelum culling |
| `culledAnimatorWasEnabled` | `bool` | `false` | Status `Animator.enabled` sebelum culling |

Ketiga field ini usulan tambahan untuk `EnemyInstance` di `SpawnManager.md`. Status lama disimpan, supaya culling tidak menyalakan komponen yang sengaja dimatikan sistem lain, misalnya Jaguar di perch (`WildlifeAI.md`).

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.optimization.cullingDistance` | 50 | Jarak culling, dalam meter |
| `GameBalanceConfig.optimization.cullCheckSeconds` | 1 | Jeda pemeriksaan |
| `GameBalanceConfig.remoteBase.remoteDistance` | 80 | Dibaca. Batas pengecualian zombie `AttackBuilding` (#90) |
| `GameBalanceConfig.remoteBase.clusterRadius` | 30 | Dibaca. Tambahan jarak pengecualian, lihat langkah 3 |

Bagian `optimization` belum ada di `GameBalanceConfig.md`, dan perlu ditambahkan. Jarak dibandingkan dalam bentuk kuadrat, jadi tidak ada `sqrt`.

## 2. Detail Logika & Fungsi

**`OnEnable`**: cari pemain Fase 2, isi `player`, lalu mulai `CullingLoop()`. **`OnDisable`**: hentikan coroutine dan kosongkan `player`. Manager tidak mendengar event.

**`IEnumerator CullingLoop()`**, setiap `cullCheckSeconds` dengan `WaitForSeconds` berskala:

1. Kalau `player == null` atau `SpawnManager.Instance == null`, lewati putaran ini.
2. Iterasi `ActiveEnemies` dengan indeks dari belakang, tanpa `foreach` dan tanpa alokasi. Untuk tiap musuh, jalankan `Evaluate(e)`.

**`private void Evaluate(EnemyInstance e)`**:

1. **Lewati** musuh yang `IsDormantCounted`. Kalau sedang `IsCulled`, nyalakan kembali lewat `Uncull(e)`, karena sistem dormant mengatur komponennya sendiri.
2. **Musuh yang sedang bertarung tidak di-culling:** kalau `e.IsEngaged` dan state-nya bukan `AttackBuilding`, perlakukan seperti dekat. Musuh yang mengejar pemain tidak boleh membeku di belakang pemain. Ini selaras dengan `HordeCrowdManager`, yang memberi `IsEngaged` tingkatan `Close`.
3. **Zombie yang menyerang base:** kalau `EnemyAI` berstate `AttackBuilding` dan jarak ke pemain masih dalam `remoteDistance + clusterRadius` (110 m), jangan di-culling (#90). Tambahan `clusterRadius` diperlukan karena `RemoteBaseSimulation` mengukur jarak pemain ke pusat base, bukan ke tiap zombie. Zombie di sisi jauh base bisa berjarak lebih dari 80 m dari pemain, padahal base masih **Nyata**. Di atas 110 m, pusat base pasti lebih dari 80 m, dan `RemoteBaseSimulation` mengembalikan zombie lewat `ReturnRaidersNear`.
4. Kalau jarak > `cullingDistance` dan belum `IsCulled`, jalankan `Cull(e)`.
5. Kalau jarak <= `cullingDistance` dan sedang `IsCulled`, jalankan `Uncull(e)`.

**`private void Cull(EnemyInstance e)`**:

1. Simpan `culledAgentWasEnabled = agent.enabled` dan `culledAnimatorWasEnabled = animator.enabled`.
2. Kalau agen aktif dan berada di NavMesh, panggil `agent.ResetPath()`, lalu `agent.enabled = false`.
3. `animator.enabled = false`.
4. Kalau musuh punya `ICrowdAgent`, panggil `SetTier(CrowdTier.Culled)`. `HordeCrowdManager` lalu berhenti memanggil `CrowdTick` untuknya. Hewan (`WildlifeAI`) tidak memakai `ICrowdAgent`, jadi langkah ini dilewati.
5. `IsCulled = true`, dan `culledCount++`.

Musuh ini tetap dihitung hidup, sehingga batas `MaxAlive` tidak terlewati.

**`private void Uncull(EnemyInstance e)`**:

1. Kalau `culledAgentWasEnabled`, nyalakan agen. Kalau `!agent.isOnNavMesh`, coba `NavMesh.SamplePosition` di sekitar posisi lalu `agent.Warp`. Kalau tetap gagal, biarkan pengaman macet `SpawnManager` yang mengembalikannya ke pool.
2. Untuk `ICrowdAgent`, panggil `HordeCrowdManager.Instance.UpdateTiers()` atau tunggu putaran tingkatan berikutnya. `SetTier` yang mengatur `animator.enabled`, karena tingkatan `Mid` dan `Far` sengaja mematikan Animator dan memperbaruinya manual.
3. Untuk musuh tanpa `ICrowdAgent`, kembalikan `animator.enabled = culledAnimatorWasEnabled`.
4. Perbarui tujuan. Zombie lewat `HordeCrowdManager.TrySetDestination` pada tick berikutnya. Hewan lewat state `WildlifeAI` yang sedang berjalan.
5. `IsCulled = false`, dan `culledCount--`.

**Profiler:** seluruh `CullingLoop` dibungkus penanda `Optimization.Cull`. Target: di bawah 0,1 ms untuk 64 musuh.

## 3. Hubungan dengan Sistem Lain

**Event:** tidak mengirim dan tidak mendengar event.

| Sistem | Hubungan |
|---|---|
| `SpawnManager` (#62) | Sumber `ActiveEnemies` dan `MaxAlive`. Pengaman macetnya perlu melewati musuh `IsCulled` (lihat bagian 4) |
| `HordeCrowdManager` (#93) | `ICrowdAgent.SetTier(Culled)`, `UpdateTiers`, dan `TrySetDestination` |
| `EnemyAI` (#25), `WildlifeAI` (#61) | Komponen `NavMeshAgent` dan `Animator` yang dimatikan |
| `RemoteBaseSimulation` (#90) | Sumber `remoteDistance` dan `clusterRadius`, dan menangani base di luar jarak itu |
| `EnemyAttack` (#109), `AnimationController` (#12) | Tetap berfungsi saat Animator dimatikan, lewat timer cadangan |
| `DevConsole` (#56) | Usulan: perintah `cullinfo` menampilkan `culledCount` |

## 4. Kasus Khusus

- **Objek pool:** musuh yang kembali ke pool hilang dari `ActiveEnemies`. `EnemyInstance.OnSpawned` harus mengosongkan `IsCulled`, `culledAgentWasEnabled`, dan `culledAnimatorWasEnabled`, lalu menyalakan `Animator`. Agen sudah dinyalakan `EnemyAI.OnSpawned` lewat `Warp`. Tanpa reset ini, musuh dari pool bisa muncul dengan Animator mati. `culledCount` dihitung ulang dari `ActiveEnemies` setiap putaran, jadi tidak bisa salah lama.
- **Pengaman macet `SpawnManager`:** agen yang dimatikan membuat `agent.isOnNavMesh` bernilai `false` dan posisinya tidak bergerak. Tanpa pengecualian, `WatchdogLoop` langkah 2 sampai 4 akan mengembalikan musuh yang di-culling dengan alasan `OffNavMesh` atau `Stuck`. Karena itu `WatchdogLoop` perlu melewati langkah 2 dan 3 untuk musuh `IsCulled`. Langkah 5 (`TooFar` pada 70 m) tetap berlaku.
- **Game dijeda:** coroutine memakai waktu berskala, jadi berhenti. Status culling tetap seperti sebelum jeda.
- **Tidur:** `SleepSystem` (#67) menolak tidur kalau ada musuh dekat, jadi culling tidak memengaruhinya. Musuh yang di-culling tetap terhitung oleh pemeriksaan jarak `SleepSystem`, karena pemeriksaannya memakai posisi atau collider musuh, bukan agen atau Animator.
- **Permadeath:** ikut hilang bersama scene. Pool dikembalikan lewat `ReturnAllActive` sebelum scene diganti (#82), jadi reset di `OnSpawned` berlaku untuk run baru.
- **Save dan load:** tidak ada data yang disimpan. Musuh juga tidak disimpan (#88).
- **Region dilepas:** `SpawnManager` mengembalikan musuh saat event `Unloading` (#88), termasuk yang sedang di-culling.
- **Musuh lebih dari kapasitas:** tidak terjadi, karena batasnya `MaxAlive`.
- **`maxDistance` spawn:** 45 m sengaja di bawah `cullingDistance` 50 m (#62). Musuh baru tidak langsung di-culling.
- **Bolak-balik di sekitar 50 m:** paling sering satu kali per `cullCheckSeconds`, jadi biayanya kecil. Kalau playtest menunjukkan kedipan animasi, lihat usulan di bagian 5.
- **Pemain teleport atau dimuat:** putaran berikutnya menghitung ulang semua jarak. Paling lama `cullCheckSeconds` musuh berada di status lama.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** sumber daftar dari `SpawnManager` (`SpawnManager.md`). Peran culling jarak jauh (`HordeCrowdManager.md`). Pengecualian base (`RemoteBaseSimulation.md`). `maxDistance` di bawah `cullingDistance` (`SpawnManager.md`).

**Keputusan teknis di blueprint ini:**

- Status culling disimpan di `EnemyInstance`, bukan di daftar manager.
- Status `enabled` sebelum culling disimpan, lalu dikembalikan.
- Animator zombie dikembalikan lewat `SetTier`, bukan dinyalakan langsung.

**Keputusan yang perlu dicek developer:**

1. #62 meminta daftar musuh dari `SpawnManager`, dan #93 meminta daftar agen dari `HordeCrowdManager`. Blueprint ini memakai `SpawnManager` sebagai daftar musuh, dan `HordeCrowdManager` hanya untuk tingkatan.
2. **Musuh `IsEngaged` tidak di-culling (usulan teknis):** tanpa aturan ini, zombie yang mengejar pemain lebih dari 50 m membeku dan tidak pernah dikembalikan, karena `SpawnManager` tidak mengembalikan musuh `IsEngaged`.
3. **Pengecualian base memakai 110 m (usulan teknis):** `remoteDistance + clusterRadius`, bukan 80 m, dengan alasan di bagian 2 langkah 3.
4. **Jarak pantul (usulan, belum dipakai):** culling pada 50 m dan kembali aktif pada 45 m kalau playtest menunjukkan bolak-balik. Nilai awal tetap satu jarak.

## 6. Skenario Tes

1. Jauhi zombie yang sedang berkeliaran lebih dari 50 m. **Hasil:** Animator dan agen mati, tier `Culled`, dan zombie tetap terhitung hidup.
2. Dekati kembali. **Hasil:** zombie aktif lagi dengan tujuan baru, dan Animator mengikuti tingkatan `Mid` atau `Far`.
3. Biarkan zombie menyerang base, lalu berjalan 70 m dari base. **Hasil:** zombie tetap menyerang.
4. Berdiri 75 m dari pusat base, dengan zombie menyerang dinding di sisi jauh sekitar 100 m dari pemain. **Hasil:** zombie tetap menyerang.
5. Jauhi base lebih dari 80 m dari pusatnya. **Hasil:** alur `RemoteBaseSimulation` mengambil alih, dan zombie dikembalikan ke pool.
6. Musuh `IsDormantCounted`. **Hasil:** dilewati.
7. Biarkan zombie mengejar, lalu sprint menjauh lebih dari 50 m. **Hasil:** zombie tetap mengejar dan tidak membeku.
8. Jauhi Jaguar yang sedang di perch, lalu kembali. **Hasil:** agen tetap mati selama di perch, dan Jaguar tidak jatuh.
9. Biarkan zombie di-culling selama 30 detik di luar pandangan. **Hasil:** tidak dikembalikan sebagai `Stuck` atau `OffNavMesh`, dan dikembalikan sebagai `TooFar` setelah lewat 70 m.
10. Kembalikan zombie yang di-culling ke pool lewat `DevConsole`, lalu munculkan zombie baru dari pool yang sama di dekat pemain. **Hasil:** Animator menyala, dan `IsCulled` bernilai `false`.
11. Buka Pause selama 5 detik dengan zombie di 60 m. **Hasil:** status tidak berubah selama jeda.
12. Jalankan uji stres 50 zombie dari #93, dengan separuh di atas 50 m. **Hasil:** penanda `Optimization.Cull` di bawah 0,1 ms, tanpa alokasi GC.
13. Mati, lalu mulai run baru. **Hasil:** tidak ada musuh yang muncul dengan Animator mati.

## 7. Catatan Implementasi (Sistem pendukung)

Kode: `Assets/LOGIC/Core_Logic/OptimizationManager.cs`. Belum diuji di Unity.

- **Beda dengan teks blueprint:** `EnemyAI.SetTier` saat ini hanya mengatur `cullingMode`, belum `animator.enabled`. Karena itu `Uncull` mengembalikan `animator.enabled = culledAnimatorWasEnabled` untuk semua musuh, lalu memanggil `HordeCrowdManager.SetCulled(agent, false)` (tingkatan `Mid`, diperbaiki putaran tingkatan berikutnya). `HordeCrowdManager.UpdateTiers` bersifat privat, jadi tidak dipanggil langsung.
- **Tafsiran:** `Cull` dan `Uncull` memanggil `HordeCrowdManager.SetCulled`, yang sudah ada dan memanggil `SetTier(Culled)`.
- **Tafsiran:** pemain dicari lewat `IslandPlayerController.Instance`, dan dicari ulang tiap putaran kalau belum ada saat `OnEnable`.
- **Angka tambahan di config:** `optimization.uncullSampleRadius` 2, radius `NavMesh.SamplePosition` saat agen dinyalakan kembali.
- Pengecualian `WatchdogLoop` untuk musuh `IsCulled` sudah ada di `SpawnManager.cs`.
