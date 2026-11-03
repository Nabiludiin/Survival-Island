# Blueprint: ShipHazardManager.cs
**Lokasi:** `docs/blueprint/60-seconds/ShipHazardManager.md`
**Deskripsi Utama:** Bahaya nyata di kapal selama 60 detik: air yang naik, koridor runtuh, zombie, dan kemiringan yang memengaruhi gerakan.
**Issue:** #75
**Rujukan GDD:** Bagian 3 (kapal miring dan dipenuhi air serta zombie, dan permadeath). Jadwal, jumlah zombie, dan cara menghadapinya adalah usulan, karena GDD tidak merincinya.

## 1. Arsitektur & Variabel Utama

### Pola

`ShipHazardManager` adalah singleton per scene di `ShipPhase` (#82). Jadwal berupa data, dan pemeriksaan memakai waktu sisa dari `ShipTimerManager`.

```csharp
public enum ShipHazardKind { AlarmUp, CameraShake, SpawnZombies, CollapseWarning, Collapse, SetSwayStage, FinalShudder }

[Serializable]
public struct ShipHazardEntry
{
    public float atSecondsLeft;
    public ShipHazardKind kind;
    public string targetId;     // ID koridor atau grup zombie
    public int value;           // Tahap sway atau jumlah zombie
}

[CreateAssetMenu(menuName = "Survival/Ship Hazard Schedule")]
public class ShipHazardSchedule : ScriptableObject
{
    public ShipHazardEntry[] entries;           // Urut dari waktu sisa terbesar
    public WaterKeyframe[] waterKeyframes;
}

[Serializable]
public struct WaterKeyframe { public float atSecondsLeft; public float level; }
```

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Instance` | `ShipHazardManager` | `null` | Singleton per scene |
| `IsRunning` | `bool` | `false` | Aktif setelah `Begin()` |
| `nextEntryIndex` | `int` | 0 | Entri jadwal berikutnya |
| `WaterLevel` | `float` | Keyframe pertama | Tinggi air dunia |
| `activeZombies` | `List<EnemyAI>` | Kapasitas 8 | Zombie aktif di kapal |
| `corridors` | `ShipCorridor[]` | Dari scene | Koridor yang bisa runtuh |
| `waterVisual` | `Transform` | Dari scene | Bidang air |
| `lowerDeckBarrier` | `Collider` | Nonaktif | Penutup tangga dek bawah |

### Susunan kapal

Ukuran ini menjadi syarat level design (usulan).

| Dek | Tinggi lantai (`Y`) | Isi | Risiko |
|---|---|---|---|
| Dek bawah | 0 | Gudang, loot terbanyak, peluru | Banjir paling awal |
| Dek tengah | 4 | Kabin, titik awal pemain, NPC | Banjir di akhir |
| Dek atas | 8 | Sekoci (#74) | Tidak kena air sebelum `T=0` |

Jarak berjalan dengan `moveSpeed` 5 m/s: titik awal ke sekoci sekitar 12 detik, dan dek bawah ke sekoci sekitar 20 detik.

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.shipHazard.staticTiltDegrees` | 4 | Kemiringan tetap kapal |
| `GameBalanceConfig.shipHazard.tiltSpeedInfluence` | 0,12 | Pengaruh kemiringan ke kecepatan |
| `GameBalanceConfig.shipHazard.driftSpeedMax` | 0,4 | Geser ke sisi rendah pada tahap terakhir, dalam m/s |
| `GameBalanceConfig.shipHazard.swaySpeed` | 1 | Dari `ShipPlayerController` |
| `GameBalanceConfig.shipHazard.swayDegreesByStage` | 2, 3,5, 5, 7 | Roll kamera per tahap |
| `GameBalanceConfig.shipHazard.waterRiseMaxPerSec` | 0,5 | Batas perubahan tinggi air per detik |
| `GameBalanceConfig.shipHazard.waterAnkleDepth` | 0,2 | Air mulai memperlambat |
| `GameBalanceConfig.shipHazard.waterKneeDepth` | 0,6 | Tahap kedua |
| `GameBalanceConfig.shipHazard.waterWaistDepth` | 1,2 | Tahap ketiga |
| `GameBalanceConfig.shipHazard.waterHeadDepth` | 1,6 | Kepala terendam, sama dengan tinggi mata |
| `GameBalanceConfig.shipHazard.waterAnkleMultiplier` | 0,85 | Pengali kecepatan |
| `GameBalanceConfig.shipHazard.waterKneeMultiplier` | 0,6 | Pengali kecepatan |
| `GameBalanceConfig.shipHazard.waterWaistMultiplier` | 0,35 | Pengali kecepatan |
| `GameBalanceConfig.shipHazard.collapseWarningSeconds` | 3 | Tanda sebelum koridor runtuh |
| `GameBalanceConfig.shipHazard.zombieMaxAlive` | 8 | Batas zombie sekaligus |
| `GameBalanceConfig.shipHazard.zombieMoveSpeed` | 3,5 | Lebih lambat dari pemain 5 m/s |
| `GameBalanceConfig.shipHazard.zombieAggroRadius` | 12 | Radius bangun |
| `GameBalanceConfig.shipHazard.zombieMinSpawnDistance` | 10 | Jarak minimum kemunculan dari pemain dan sekoci |
| `GameBalanceConfig.shipHazard.zombieTickSeconds` | 0,25 | Jeda pembaruan kecepatan zombie |
| `GameBalanceConfig.shipHazard.shoveRadius` | 1,2 | Radius dorongan |
| `GameBalanceConfig.shipHazard.shoveForwardOffset` | 1 | Jarak titik dorongan dari pemain |
| `GameBalanceConfig.shipHazard.shoveKnockback` | 3 | Jarak terdorong |
| `GameBalanceConfig.shipHazard.shoveStaggerSeconds` | 1,5 | Lama terhuyung |
| `GameBalanceConfig.shipHazard.shoveCooldownSeconds` | 1,5 | Jeda dorongan |

## 2. Detail Logika & Fungsi

### Jadwal

**`void Update()`**, saat `IsRunning`:

```csharp
float t = ShipTimerManager.Instance.SecondsLeft;
while (nextEntryIndex < entries.Length && t <= entries[nextEntryIndex].atSecondsLeft)
    Fire(entries[nextEntryIndex++]);
UpdateWater(t);
UpdatePlayerEffects();
```

`while` memastikan tidak ada entri terlewat saat frame panjang. `Fire` hanya dipanggil sekali per entri, karena `nextEntryIndex` maju.

**Isi jadwal awal:**

| Waktu sisa | Kejadian | Tinggi air (`Y`) saat itu | Rute |
|---|---|---|---|
| `T=60` | Sway tahap 0, 4 zombie tertidur (3 dek bawah, 1 dek tengah) | -1 | Semua terbuka |
| `T=45` | Alarm lebih keras, sway tahap 1, 2 zombie bangun di dek tengah | -0,5, masih di bawah lantai dek bawah | Semua terbuka |
| `T=33` | Tanda runtuh `Corridor_MidPort`: debu, suara besi, dan garis merah | -0,1 | Masih terbuka |
| `T=30` | `CameraShake.Play(CameraShakeKind.ShipRumble)`, sway tahap 2, koridor runtuh, dan 2 zombie di dek bawah | 0, mulai membasahi dek bawah | `Corridor_MidPort` tertutup |
| `T=18` | Tanda runtuh `Stair_LowerAft` | 0,9 | Masih terbuka |
| `T=15` | Sway tahap 3, tangga runtuh, 2 zombie di dek tengah, dan garis area aman tampil | 1,1 | `Stair_LowerAft` tertutup |
| Sekitar `T=13` | Kepala terendam di dek bawah, dan `lowerDeckBarrier` aktif | 1,6 | Dek bawah tidak bisa dimasuki. Yang masih di bawah keluar lewat `Stair_LowerFore` |
| `T=5` | Air mencapai dek tengah | 4,0 | Dek tengah mulai melambat |
| `T=0` | `FinalShudder` dengan `CameraShake.Play(CameraShakeKind.ShipFinalShudder)`, lalu #74 menentukan hasil | 4,6 | Input terkunci |

`docs/blueprint/60-seconds/README.md` menulis air mulai masuk pada `T=15`. Blueprint ini mengikuti itu untuk dek tengah dan lantai utama yang dilihat pemain, sementara dek bawah mulai basah sejak `T=30`, supaya ada tahapan.

`ShipTimerManager` tetap mengatur tensi visual, seperti vignette dan pitch tick, di bawah 15 detik. Isi getaran ada di #24.

### Air naik

Satu bidang air berlaku untuk seluruh kapal, dan dek yang lebih rendah otomatis banjir lebih dulu. Lantai yang miring membuat sisi rendah lebih cepat basah.

**`private void UpdateWater(float t)`**:

1. Hitung `target` dengan interpolasi linier `waterKeyframes` pada waktu sisa `t`.
2. `WaterLevel = Mathf.MoveTowards(WaterLevel, target, waterRiseMaxPerSec * Time.deltaTime)`. Air naik bertahap dan tidak melompat.
3. Pindahkan `waterVisual`.
4. Kalau `WaterLevel >= waterHeadDepth`, dengan lantai dek bawah di `Y = 0`, aktifkan `lowerDeckBarrier` setelah pemain tidak lagi berada di bawahnya. Penghalang ini berbasis tinggi air, bukan entri jadwal, jadi tetap sinkron kalau keyframe disetel.

**Keyframe (waktu sisa, tinggi):** (60, -1), (30, 0), (15, 1,1), (5, 4), dan (0, 4,6). Perubahan tercepat 0,29 m/s, jadi `waterRiseMaxPerSec` hanya menjadi pengaman.

**Akibat ke pemain, `private void UpdatePlayerEffects()`:**

```csharp
float depth = WaterLevel - player.position.y;
float waterMul = depth < ankle ? 1f : depth < knee ? ankleMul : depth < waist ? kneeMul : waistMul;
playerController.SetShipMoveModifiers(waterMul, ComputeTiltMul(), ComputeDrift());
stats.SetDrowning(depth >= headDepth);
```

| Kedalaman di atas kaki | Kecepatan | Akibat |
|---|---|---|
| Di bawah 0,2 m | 100 persen | Tidak ada |
| 0,2 sampai 0,6 m | 85 persen | Percikan langkah |
| 0,6 sampai 1,2 m | 60 persen | Suara langkah air |
| 1,2 sampai 1,6 m | 35 persen | Efek basah di layar (#23) |
| 1,6 m atau lebih | 35 persen | `SetDrowning(true)`, HP turun `vitals.drownDamagePerSec` 10 per detik (#111) |

**Area bawah yang tidak bisa dilewati:** sekitar `T=13`, `lowerDeckBarrier` menutup jalan masuk tangga dek bawah. Pemain yang sudah di bawah tetap bisa naik, karena penghalang baru aktif saat pemain berada di atasnya. Kalau pemain masih di bawah, air menghabisi HP lewat `Drowned`.

### Koridor runtuh

`ShipCorridor` adalah komponen koridor.

| Field | Fungsi |
|---|---|
| `corridorId` | ID di jadwal |
| `debrisRoot` | Puing, nonaktif sebelum runtuh |
| `navObstacle` | `NavMeshObstacle` dengan `carving = true` |
| `warningVfx` | Debu dan percikan |
| `exitPoint` | Titik pindah pemain |
| `blockVolume` | `Bounds` area koridor |

**`Warn()`**: nyalakan `warningVfx`, mainkan suara besi melengkung, dan tampilkan garis merah di lantai.

**`Collapse()`**:

1. Kalau pemain berada di `blockVolume`, pindahkan lewat `controller.Move` ke `exitPoint`, tanpa damage. Tanda sudah diberikan selama 3 detik.
2. Nyalakan `debrisRoot` dan `navObstacle`.
3. Zombie di dalam `blockVolume` langsung kembali ke pool.
4. Panggil `CameraShake.Play(CameraShakeKind.ShipCollapse)` (#24) dan mainkan suara.

**Aturan rute:** selalu ada satu rute dari dek tengah ke sekoci. `ShipRouteAudit` adalah alat Editor yang memeriksa jalur NavMesh dari titik awal ke sekoci, dengan semua obstacle jadwal aktif, dan melaporkan kalau jalurnya putus. Titik NPC di dek bawah harus punya waktu jalan ke sekoci paling lama 20 detik, untuk syarat 5 di `RunLayoutValidator` (#89).

### Zombie di kapal

| Hal | Aturan |
|---|---|
| Jumlah | 4 tertidur di awal, tambah 2, 2, dan 2 sesuai jadwal. Paling banyak `zombieMaxAlive` 8 sekaligus |
| Jenis | Zombie standar, `ZombieData` dengan `enemyId = zombie_standar` (#25), memakai pool yang sama dengan pulau, dengan tag `Zombie_Standard`. Target memakai `EnemyAI.SetTarget` langsung, karena `ThreatSystem` tidak ada di `ShipPhase` (#109) |
| Posisi | Titik `ShipZombiePoint` per grup, minimal `zombieMinSpawnDistance` dari pemain dan sekoci |
| Tidur | `AIState.Dormant`. Bangun kalau pemain masuk `zombieAggroRadius` dan terlihat, atau saat suara `T=45` untuk grup yang dijadwalkan |
| Kecepatan | `zombieMoveSpeed`, dikali pengali air. Pemain selalu bisa berlari menjauh di lantai kering |
| Serangan | `AttackData` `Zombie_Ship_Bite` (#109): damage 15, infeksi 8 |
| Terendam | Zombie dengan kedalaman `>= waterHeadDepth` kembali ke pool. Dek bawah tidak penuh mayat |
| Macet | Lewat 5 detik tanpa jalur lengkap, zombie kembali ke `Dormant` |

**`private void SpawnZombies(string groupId, int count)`**: untuk tiap titik di grup, lewati kalau `activeZombies.Count >= zombieMaxAlive`, atau jaraknya kurang. Panggil `ObjectPooler.Instance.SpawnFromPool("Zombie_Standard", pos, rot)`, lalu tambahkan ke `activeZombies`. Saat `EnemyHealth.Died` atau objek kembali ke pool, hapus dari daftar.

`zombieTickSeconds` memperbarui `agent.speed` dan memeriksa kedalaman, jadi pekerjaan ini tidak berjalan per frame. Zombie kapal tidak terbakar, karena `DayNightDirector` tidak ada di `ShipPhase`, dan nilai `DaybreakEvent` sudah dihapus `GameEventRegistry.ClearAll()` (#82).

### Cara menghadapi zombie

**Pistol dan Pisau tidak dipakai di Fase 1.** Pemain membawanya, sesuai GDD, tetapi `EquipmentManager` tidak ada di `ShipPhase`, sehingga tangan kosong. Ini mengikuti `60-seconds/README.md` ("tidak perlu sistem crafting atau menembak").

**Pemain punya tiga pilihan:** menghindar, memakai rute lain, atau mendorong.

**`ShipShove`**, komponen di pemain Fase 1. `ShipPlayerController` membaca `InputManager.PrimaryPressed`, yang menjadi aksi dorong di kapal:

1. Tolak kalau masih dalam `shoveCooldownSeconds`.
2. Mainkan animasi dorong tangan.
3. `count = PhysicsQuery.OverlapSphere(player.position + forward * shoveForwardOffset, shoveRadius, PhysicsMasks.Melee, buffer)`.
4. `PhysicsQuery.CollectUniqueDamageables(buffer, count, results)`.
5. Untuk tiap `IDamageable` yang punya `EnemyHitReaction`, panggil `ApplyKnockback(dir, shoveKnockback, shoveStaggerSeconds)`, yang memakai `AIState.Knockback` dan `Stagger` (#95).

Dorongan tidak memberi damage dan tidak memakai stamina.

### Akibat kena serangan

- **HP:** berkurang lewat `IDamageable` (#59).
- **Infeksi:** naik dari `AttackData.infection`, dan **terbawa ke pulau** lewat `ShipOutcomeData.infectionOnExit` (#74).
- **HP juga terbawa ke pulau.** Lapar dan haus tidak berjalan di kapal.
- **Efek layar:** dari `VitalWarningChangedEvent` (#111) dan #23.

### Akibat HP habis di kapal

`SurvivalStats.TriggerDeath` menerbitkan `PlayerDiedEvent` dengan `KilledByEnemy` atau `Drowned`. `GameManager.TriggerGameOver` meneruskannya ke `RunLifecycle.RunEndSequence` (#82), jalur yang sama dengan pulau. Pemain kembali ke `ShipPhase` setelah memilih Ulangi. Tidak ada save yang dihapus, karena Fase 1 belum punya save.

Saat `RunEndedEvent` terkirim, `ShipHazardManager` memanggil `StopAll()`: `IsRunning = false`, zombie berhenti menyerang, dan air tidak naik lagi.

### Kemiringan dan gerakan

**Kemiringan memengaruhi gerakan, tetapi kecil.** Ship sway tetap efek kamera, dan kapal tidak benar-benar berputar.

| Lapisan | Nilai | Cara |
|---|---|---|
| Kemiringan tetap | `staticTiltDegrees` 4 | Geometri scene dibuat miring dan dibake begitu saja |
| Pengaruh ke kecepatan | `tiltSpeedInfluence` 0,12 | `speed *= 1 + influence * Dot(moveDir, downhill)`. Menanjak 12 persen lebih lambat, menurun 12 persen lebih cepat |
| Geser ke sisi rendah | `driftSpeedMax` 0,4 m/s pada tahap 3 | `SetShipMoveModifiers(..., drift)`, tahap sebelumnya 0 |
| Ship sway | `swayDegreesByStage` | Roll kamera lewat `ICameraOffsetSource` |

**`ShipSwaySource : ICameraOffsetSource`** menggantikan rumus `zSway` di `ShipPlayerController`:

```csharp
public Vector3 EulerOffset => new Vector3(
    Mathf.Sin(Time.time * swaySpeed * 0.7f) * currentSway * 0.2f,
    0f,
    Mathf.Sin(Time.time * swaySpeed) * currentSway);
```

`currentSway` mengejar nilai tahap dengan `Mathf.MoveTowards`, jadi tidak melompat.

### NavMesh pada scene miring

NavMesh tidak bisa berputar saat bermain, jadi **geometri tidak boleh diputar saat bermain**.

| Hal | Aturan |
|---|---|
| Bake | Satu `NavMeshSurface` di `ShipPhase`, setelah geometri diberi kemiringan tetap `staticTiltDegrees` |
| Setting agen | `Max Slope` 30, dan `Step Height` 0,4. Kemiringan 4 derajat dan tangga berada dalam batas |
| Antar-dek | Tangga yang bisa dilewati hasil bake, tanpa `NavMeshLink` |
| Koridor runtuh | `NavMeshObstacle` carving yang nonaktif sampai `Collapse()` |
| Air | Tidak mengubah NavMesh. Kedalaman diatur `zombieTickSeconds` dan pengembalian ke pool |
| Kemunculan | `SpawnFromPool` lalu `NavMesh.SamplePosition`, kemudian `agent.Warp`, dan `agent.isOnNavMesh` diperiksa (#88) |
| Ship sway | Hanya kamera, jadi tidak pernah menyentuh NavMesh |

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `ShipHazardStageChangedEvent { stage, secondsLeft }`, untuk #23, #52, dan #114.

**Event yang didengar:** `RunEndedEvent`.

| Sistem | Hubungan |
|---|---|
| `ShipTimerManager` | Menyediakan `SecondsLeft` |
| `LifeboatExtraction` (#74) | Memanggil `Begin` dan `StopAll` |
| `ShipPlayerController` | Menerima `SetShipMoveModifiers`, dan membaca aksi dorong |
| `FirstPersonCamera` (#102) | Mendaftarkan `ShipSwaySource` |
| `SurvivalStats` (#111) | `SetDrowning`, dan membawa HP serta infeksi |
| `DamagePipeline` (#59) | Damage dari serangan |
| `EnemyAI`, `EnemyAttack` (#109), `EnemyHitReaction` (#95) | Perilaku zombie, gigitan, dan dorongan |
| `ObjectPooler` | Pool `Zombie_Standard` dan efek air |
| `CameraShake` (#24), `PostProcessController` (#23) | Isi efek. Di luar issue ini |
| `RunLifecycle` (#82) | Alur mati |
| `RunSeedManager` (#89) | Validasi titik NPC |
| `AudioManager` | Alarm dan suara kapal |

## 4. Kasus Khusus

- **Objek pool:** zombie mengembalikan HP, state, skala, dan `agent.speed` lewat `IPoolable`. `ShipHazardManager` mengatur kecepatan pada tick berikutnya, jadi pengali air lama tidak terbawa. Saat keluar scene, `ObjectPooler.ReturnAllActive()` (#82) menampung semuanya, dan `activeZombies` hilang bersama scene.
- **Game dijeda:** `SecondsLeft` tidak maju, dan `Time.deltaTime` bernilai 0, jadi air, jadwal, serta zombie berhenti. `Time.time` di `ShipSwaySource` juga berhenti.
- **Permadeath:** tidak ada data tersimpan. `ShipPhase` dimuat ulang, dan `nextEntryIndex` kembali ke 0.
- **Save dan load:** Fase 1 tidak punya save (#82).
- **Berangkat lebih awal:** `StopAll()` menghentikan semua sebelum ringkasan (#74).
- **Frame panjang melompati beberapa entri:** semua entri diproses berurutan.
- **Pemain sedang di tangga dek bawah saat runtuh:** pemain dipindah ke `exitPoint` di atas tangga.
- **Zombie di balik koridor runtuh:** tetap ada, dan bisa mengambil rute lain. Kalau tidak ada jalur, ia kembali `Dormant`.
- **Pool zombie kosong:** `SpawnFromPool` mengikuti aturan #62 untuk pool terbatas. Grup dilewati tanpa error.
- **Kena air saat menahan `E`:** interaksi tetap berjalan, kecuali pemain terluka (#17).
- **Menahan klik kiri:** dorongan hanya pada `PrimaryPressed`, jadi tidak beruntun.
- **`KeyItem` hilang di kapal:** tidak ada, karena item inti tidak berada di tangan fisik. Barang kapal yang tertinggal memang hilang.
- **NPC di area yang banjir:** tidak diserang air. Kalau sudah terendam dalam, NPC tidak lagi bisa diambil (`IsAvailable = false` saat kedalaman `>= waterHeadDepth`).

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** jadwal alarm `T=45`, getaran `T=30`, dan air `T=15` (`60-seconds/README.md`). `DeathReason.Drowned` dan alur mati tunggal (`GLOSSARY.md`, #82). Tanpa sistem menembak di Fase 1 (`60-seconds/README.md`).

**Keputusan teknis di blueprint ini:**

- Jadwal berupa data, dan satu bidang air berlaku untuk seluruh kapal.
- Geometri kapal memakai kemiringan tetap, supaya NavMesh tetap sah.
- Ship sway menjadi sumber offset kamera, dan keluar dari `ShipPlayerController`.

**Usulan yang menunggu developer (dicatat di #85):**

1. **Pistol dan Pisau:** **tidak bisa dipakai di Fase 1.** Pemain hanya menghindar dan mendorong. Pilihan lain adalah mengizinkannya, tetapi peluru akan terbuang dan fase ini berubah menjadi pertarungan.
2. **Dorongan:** klik kiri mendorong zombie sejauh 3 meter, terhuyung 1,5 detik, tanpa damage, dengan jeda 1,5 detik.
3. **Zombie:** maksimal 8 sekaligus, total sekitar 10 dalam 60 detik, kecepatan 3,5 m/s, gigitan damage 15 dan infeksi 8.
4. **Terbawa ke pulau:** infeksi dan HP dari kapal terbawa.
5. **HP habis di kapal:** permadeath penuh, kembali ke `ShipPhase` lewat alur #82.
6. **Air:** mulai dari dek bawah pada `T=30`, dan mencapai dek tengah pada `T=5`. Dek atas tetap kering sampai `T=0`.
7. **Kemiringan:** memengaruhi kecepatan 12 persen, dengan geser kecil di 15 detik terakhir.
8. **Koridor runtuh:** dua titik, dengan tanda 3 detik, tanpa damage langsung.

## 6. Skenario Tes

1. Jalankan Fase 1 sampai selesai tanpa bergerak dari dek atas. **Hasil:** semua kejadian terjadi pada waktunya, dan sekali.
2. Jalankan `eventlog 1`. **Hasil:** `ShipHazardStageChangedEvent` terkirim pada `T=45`, `T=30`, dan `T=15`.
3. Berdiri di dek bawah. **Hasil:** pada `T=30` lantai basah, kecepatan turun bertahap, dan sekitar `T=13` kepala terendam.
4. Tetap di dek bawah. **Hasil:** HP turun 10 per detik, lalu mati dengan alasan `Drowned`.
5. Coba turun ke dek bawah setelah `T=13`. **Hasil:** tertahan `lowerDeckBarrier`.
6. Keluar dari dek bawah setelah `T=13` lewat tangga depan. **Hasil:** tetap bisa naik.
7. Berdiri di `Corridor_MidPort` saat `T=30`. **Hasil:** tanda muncul 3 detik sebelumnya, lalu pemain dipindah ke `exitPoint`, tanpa damage.
8. Jalankan `ShipRouteAudit`. **Hasil:** jalur dari titik awal ke sekoci ada pada semua kondisi. Hapus tangga cadangan secara sengaja. **Hasil:** terlapor.
9. Dekati zombie tidur. **Hasil:** zombie bangun dalam radius 12 meter kalau melihat pemain.
10. Klik kiri saat zombie di depan. **Hasil:** zombie terdorong dan terhuyung, tanpa HP berkurang. Klik lagi sebelum 1,5 detik. **Hasil:** ditolak.
11. Pastikan Pistol tidak bisa dikeluarkan. **Hasil:** tombol hotbar tidak berpengaruh, dan tidak ada error dari `EquipmentManager` yang tidak ada.
12. Biarkan zombie menggigit, lalu selesaikan Fase 1. **Hasil:** HP turun 15, infeksi naik 8, dan keduanya sama di pulau.
13. Mati oleh zombie. **Hasil:** alasan `KilledByEnemy`, alur #82 berjalan, dan Ulangi membuka `ShipPhase` dari awal.
14. Berjalan menanjak dan menurun di lantai miring. **Hasil:** kecepatan berbeda sekitar 12 persen. Saat `T<15`, pemain perlahan bergeser ke sisi rendah.
15. Biarkan zombie mengejar melewati tangga, lalu lihat NavMesh. **Hasil:** tidak ada peringatan agen di luar NavMesh, dan zombie tidak menembus puing.
16. Tekan Pause pada `T=20` selama 30 detik. **Hasil:** air, timer, dan zombie berhenti.
17. Mati dan mulai lagi lima kali. **Hasil:** jumlah objek pool tetap, dan tidak ada zombie tertinggal dari run sebelumnya.
18. Buka Profiler selama Fase 1. **Hasil:** tidak ada lonjakan GC dari `Update` setelah kemunculan zombie.
