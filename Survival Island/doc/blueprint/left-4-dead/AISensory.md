# Blueprint: AISensory.cs & NoiseEmitter.cs
**Lokasi:** `Survival Island/doc/blueprint/left-4-dead/AISensory.md`
**Deskripsi Utama:** Cara zombie melihat dan mendengar pemain, supaya pemain bisa mengendap-endap: kerucut pandang, Noise Sphere, dan ikon mata Detection Meter.
**Issue:** #49
**Rujukan GDD:** Bagian 3 (Mengendap: zombie mendeteksi pemain lewat pandangan dan suara, sehingga pemain bisa menghindar dengan mengendap, kecuali saat Blood Moon). Jarak, sudut pandang, laju deteksi, dan radius suara adalah usulan.
**Status:** Aturan GDD bagian 3. Sistem aktif. `HordeManager` (#50) dan `ZombieLair` (#112) mengandalkannya, dan `NoiseEmitter` sudah dipanggil #17, #47, #73, serta #101.

Saklar `GameBalanceConfig.stealth.enabled` hanya untuk tes. Saat `false`, setiap zombie langsung berstatus `Aware`, sesuai "Aggro penuh" di `Claude.md`. `NoiseEmitter` tetap mengirim event tanpa akibat.

## 1. Arsitektur & Variabel Utama

| Kelas | Jenis | Tempat | Fungsi |
|---|---|---|---|
| `AISensory` | `MonoBehaviour`, `IPoolable` | Prefab zombie | Menghitung `Detection01` tiap zombie |
| `AISensoryHub` | Singleton di `IslandPhase` | Satu objek | Membagi tick ke beberapa frame, mengumpulkan nilai tertinggi untuk UI, dan titik perburuan |
| `NoiseEmitter` | `MonoBehaviour` | Pemain, peti, palu, barang lempar | Mengirim `NoiseEmittedEvent` |
| `DetectionMeterUI` | `MonoBehaviour` | HUD | Ikon mata |

```csharp
public enum AwarenessLevel : byte { Unaware, Suspicious, Aware }
public enum NoiseSourceKind : byte { Movement, Landing, Gunshot, Melee, Tool, Container, Decoy, Structure, Throwable }
```

**Noise Sphere bukan collider.** Bentuknya hanya angka radius, lalu pendengar menghitung jarak sendiri. Dengan begitu tidak ada trigger tambahan yang mengganggu query fisika (#87).

**`AISensory`:**

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Detection01` | `float` | 0 | 0 sampai 1 |
| `Level` | `AwarenessLevel` | `Unaware` | Dibaca `EnemyAI` dan `ThreatSystem` (#108) |
| `LastStimulusPosition` | `Vector3` | Posisi spawn | Titik terakhir pemain terlihat atau terdengar |
| `CanSeePlayer` | `bool` | `false` | Hasil tick terakhir |
| `canSeeCurrentTarget` | `bool` | `false` | Garis pandang ke `EnemyAI.CurrentTarget` yang bukan pemain |
| `sinceLastStimulus` | `float` | 0 | Waktu sejak rangsangan terakhir |
| `LevelChanged` | `event Action<AwarenessLevel>` | Kosong | Event per objek untuk `EnemyAI`, `SpecialZombieAI` (#25), dan #112 |

**Angka (`GameBalanceConfig.stealth`):**

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `enabled` | `true` | Lihat Status di header |
| `sightRange` | 22 m | Jarak pandang |
| `sightAngleDeg` | 120 | Lebar kerucut penuh, jadi 60 derajat ke tiap sisi |
| `closeSenseRange` | 2,5 m | Di bawah jarak ini pemain selalu terdeteksi, tanpa kerucut |
| `sightGainMin` / `sightGainMax` | 0,4 / 2 per detik | Laju isi di ujung jarak dan di jarak dekat |
| `decayPerSec` | 0,15 | Laju turun saat tidak ada rangsangan |
| `suspiciousThreshold` | 0,35 | Batas `Suspicious` |
| `loseAwareSeconds` | 6 | Lama tanpa rangsangan sebelum `Aware` turun |
| `crouchVisibility` | 0,6 | Pengali jarak dan laju saat jongkok |
| `sprintVisibility` | 1,2 | Pengali saat berlari |
| `hearGain` | 0,5 | Isi maksimum per satu suara |
| `wallMuffle` | 0,6 | Pengali radius suara saat terhalang |
| `sensorTickSeconds` | 0,25 | Jeda tick per zombie |
| `sensorsPerFrame` | 6 | Batas `AISensory` diproses per frame |
| `huntHintSeconds` / `huntHintRadius` | 8 / 12 m | Titik perburuan malam hari |
| `noiseIdle` / `noiseCrouchMove` / `noiseWalk` / `noiseSprint` | 0,5 / 2 / 8 / 20 m | Radius gerak pemain |
| `noiseSwim` | 10 m | Radius saat berenang |
| `noiseGunshot` / `noiseMelee` | 40 / 8 m | Radius serangan |
| `noiseLandingMax` | 15 m | Radius mendarat pada `impact01 = 1` |
| `noiseTickSeconds` | 0,5 | Jeda kirim suara gerak |

`ZombieData` (#25) menambah `sightMultiplier` dan `hearingMultiplier`, keduanya bernilai awal 1.

`Throwable` (#96) dipakai suara barang lempar. Musuh tanpa target memakai suara ini sesuai `ThreatSystem.md`.

## 2. Detail Logika & Fungsi

### Kerucut pandang

**`public void Tick(float dt)`**, dipanggil `AISensoryHub`:

1. Kalau `hub.IgnoreStealth` atau `stealth.enabled == false`: `Detection01 = 1`, `Level = Aware`, `LastStimulusPosition = posisi pemain`, lalu selesai.
2. `range = sightRange * data.sightMultiplier * hub.PlayerVisibility * hub.WeatherSight` (#115).
3. Hitung `sqrDist` ke pemain. Kalau lebih dari `range²`, `CanSeePlayer = false` tanpa raycast.
4. Kalau jarak lebih dari `closeSenseRange`, cek sudut: `Vector3.Angle(transform.forward, arah) <= sightAngleDeg / 2`. Di luar kerucut, `CanSeePlayer = false`.
5. Garis pandang: `PhysicsQuery.Raycast` dari mata zombie ke kamera pemain dengan `PhysicsMasks.AIVision`. Kalau terhalang, ulangi ke dada pemain. Tidak mengenai apa pun berarti terlihat.
6. Kalau terlihat: `rate = Lerp(sightGainMin, sightGainMax, 1 - dist / range) * hub.PlayerVisibility`, `Detection01 += rate * dt`, `LastStimulusPosition = posisi pemain`, dan `sinceLastStimulus = 0`.
7. Kalau tidak: `sinceLastStimulus += dt`. Turunkan `Detection01` dengan `decayPerSec`, kecuali `Aware` yang masih dalam `loseAwareSeconds`.
8. Kalau `CurrentTarget` bukan pemain, satu raycast `AIVision` mengisi `canSeeCurrentTarget`.
9. Panggil `UpdateLevel()`.

**`public bool CanSee(ITargetable t)`** tidak menjalankan raycast. Hasilnya `CanSeePlayer` untuk pemain, `canSeeCurrentTarget` untuk target aktif, dan `false` untuk lainnya. Fungsi ini dibaca `ThreatSystem.Evaluate` (#108).

**`UpdateLevel()`:**

| Dari | Syarat | Ke |
|---|---|---|
| Apa pun | `Detection01 >= 1` | `Aware` |
| `Aware` | `sinceLastStimulus > loseAwareSeconds` | `Suspicious`, dengan `Detection01 = 0,6` |
| `Unaware` | `Detection01 >= suspiciousThreshold` | `Suspicious` |
| `Suspicious` | `Detection01 < suspiciousThreshold` | `Unaware` |

Kalau berubah, panggil `LevelChanged`.

### Noise Sphere

**`NoiseEmitter`:**

- `public void Emit(float radius, NoiseSourceKind kind)`: kirim `NoiseEmittedEvent { position, radius, sourceKind }`.
- `public void EmitLanding(float impact01)`: `Emit(noiseLandingMax * impact01, Landing)`, dipanggil #101.
- `public float CurrentRadius`: radius gerak terakhir, untuk `DevConsole` dan hewan (#61).

**Suara gerak pemain.** `NoiseEmitter` pada pemain punya `isPlayerMovement = true` dan menjalankan coroutine setiap `noiseTickSeconds`:

| Kondisi | Radius |
|---|---|
| Diam | `noiseIdle` |
| `PlayerTraversal.IsCrouching` dan bergerak | `noiseCrouchMove` |
| Berjalan | `noiseWalk` |
| `IslandPlayerController.IsSprinting` | `noiseSprint` |
| Mode `Swimming` | `noiseSwim` |

Saat diam, event tidak dikirim, supaya tidak ada beban tanpa guna. Saat tidur, coroutine berhenti lewat `SleepStateChangedEvent`.

**Sekali waktu:** `GunController` (#58) memanggil `Emit(noiseGunshot, Gunshot)`. `MeleeCombatSystem` memanggil `Emit(noiseMelee, Melee)`. Sistem lain memakai radius dari bagiannya sendiri.

**`AISensory.HandleNoise(NoiseEmittedEvent e)`:**

1. `d` = jarak ke `e.position`. `r = e.radius * data.hearingMultiplier * hub.WeatherHearing` (#115). Kalau `d > r`, selesai.
2. Kalau `d > r * wallMuffle`, jalankan satu raycast `AIVision` ke sumber suara. Kalau terhalang, suara tidak terdengar. Raycast hanya dipakai di lapisan luar ini, supaya 50 zombie tidak menembak ray setiap langkah pemain.
3. `LastStimulusPosition = e.position` dan `sinceLastStimulus = 0`.
4. Kalau `Gunshot`: `Detection01 = 1`.
5. Kalau `Decoy`: `Detection01 = max(Detection01, suspiciousThreshold)`, tanpa pernah menjadi `Aware`. Tujuannya mengikuti #96 dan #108.
6. Selain itu: `Detection01 += hearGain * (1 - d / r)`.
7. Panggil `UpdateLevel()`.

### Perilaku menurut status

| Status | Perilaku di `EnemyAI` | Kecepatan |
|---|---|---|
| `Unaware` saat malam | `AIState.Idle`, berjalan ke `hub.HuntHint` | Jalan |
| `Suspicious` | `AIState.Idle`, berjalan ke `LastStimulusPosition`, lalu menengok | Jalan |
| `Aware` | Target dari #108, `AIState.Chase` | Lari |

**`AISensoryHub.HuntHint`** adalah posisi pemain dengan geseran acak sejauh `huntHintRadius`, yang diperbarui setiap `huntHintSeconds`. Saat memperbarui `HuntHint`, kalau `SetLightAttraction` aktif, lempar `chance` dengan aliran `Gameplay` (#89). Kalau kena, `HuntHint` memakai titik tarikan, bukan posisi pemain (#98). Zombie tetap datang ke sekitar pemain sesuai GDD, tetapi baru mengunci pemain kalau melihat atau mendengarnya. Inilah ruang untuk mengendap-endap.

### `AISensoryHub`

- `Register(AISensory)` dan `Unregister(AISensory)`: dipanggil `OnEnable` dan `OnDisable`, memakai swap-remove pada `List` berkapasitas tetap.
- **`Update()`:** kalau `Time.deltaTime == 0`, selesai. Proses paling banyak `sensorsPerFrame` sensor. `dt` tiap sensor adalah `Time.time - lastTick`, dan sensor dilewati kalau belum `sensorTickSeconds`.
- **`PlayerVisibility`:** `crouchVisibility` atau `sprintVisibility` atau 1, dikali `externalVisibility`.
- **`public void SetVisibilityModifier(float value)`:** titik sambung untuk #98 dan `FlashlightController`. Nilai awal 1. Aturan cahayanya bukan bagian issue ini.
- **`public void SetLightAttraction(Vector3 point, float chance)`** (#98): mengisi titik tarikan cahaya dan peluangnya untuk `HuntHint`.
- **`public void SetWeatherModifiers(float sight, float hearing)`** (#115): mengisi properti `WeatherSight` dan `WeatherHearing`, keduanya bernilai awal 1.
- **Nilai UI:** ambil `Detection01` tertinggi setiap 0,25 detik. Kirim `PlayerDetectionChangedEvent { level01, isAware }` kalau selisihnya lebih dari 0,02.
- **`BloodMoonStartedEvent`:** `IgnoreStealth = true`, sampai `BloodMoonEndedEvent`. Semua sensor, termasuk yang baru muncul, menjadi `Aware` pada tick berikutnya. Ini memenuhi aturan "mengabaikan Stealth" di #50. Target markas tetap diatur #50 dan #108.

**`public void ForceAware(Vector3 stimulus)`** mengisi `Detection01 = 1`. Dipakai `RequestAmbush` dan kebangkitan Zombie Desa.

### UI Detection Meter

`DetectionMeterUI` mendengar `PlayerDetectionChangedEvent`, dan hanya menampilkan data:

| `level01` | Tampilan |
|---|---|
| Di bawah 0,02 | Tersembunyi, memudar dalam 1 detik |
| 0,02 sampai 0,99 | Mata terbuka perlahan, lewat `Image.fillAmount` pada kelopak, warna putih ke kuning |
| 1 (`isAware`) | Mata terbuka penuh, warna merah, dan berdenyut sekali |

Saat `stealth.enabled = false`, objek ini mematikan dirinya.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `NoiseEmittedEvent`, `PlayerDetectionChangedEvent` (baru).

**Event yang didengar:** `NoiseEmittedEvent` (tiap `AISensory`), `BloodMoonStartedEvent`, `BloodMoonEndedEvent`, `SleepStateChangedEvent` (#67).

| Sistem | Hubungan |
|---|---|
| `EnemyAI` | Membaca `Level` dan `LastStimulusPosition`, serta mendengar `LevelChanged` |
| `ThreatSystem` (#108) | Membaca `Level`, `CanSee(t)`, dan `LastStimulusPosition` sebagai suara terakhir |
| `PlayerTraversal` (#101) | Sumber `IsCrouching`, `Mode`, dan pemanggil `EmitLanding` |
| `WildlifeAI` (#61) | Mendengar `NoiseEmittedEvent`, tetapi tidak memakai `AISensory` dan tidak mengisi ikon mata |
| `HordeCrowdManager` (#93) | Mengatur `EnemyAI.CrowdTick`. `Tick` indra tetap dijadwalkan `AISensoryHub`, dengan batas sendiri |
| `ZombieLair` (#112), `SpecialZombieAI` (#25) | Memakai `LevelChanged` untuk membangunkan `Dormant` dan Zombie Lumut |
| `PhysicsQuery`, `PhysicsMasks` (#87) | Semua raycast memakai `PhysicsMasks.AIVision` |

## 4. Kasus Khusus

- **Objek pool:** `OnSpawned()` mengatur `Detection01 = 0`, `Level = Unaware`, `CanSeePlayer = false`, `sinceLastStimulus = 0`, dan `LastStimulusPosition` ke posisi spawn. `OnDisable` melepas event dan `Unregister`, sehingga objek nonaktif tidak menerima suara.
- **Game dijeda:** `Time.deltaTime` nol, jadi tick berhenti, dan coroutine suara ikut berhenti. Tidak ada suara baru karena input terkunci (#83). Ikon mata tetap menampilkan nilai terakhir.
- **Permadeath dan save:** tidak ada data yang disimpan, karena musuh tidak disimpan (#88). `GameEventRegistry.ClearAll()` menghapus nilai sticky. Setelah load, semua zombie mulai `Unaware`, kecuali yang muncul lewat `RequestAmbush`.
- **Zombie di-culling:** `Tick` tetap aman, karena jaraknya di atas `sightRange`. Suara keras tetap didengar, dan `OptimizationManager` menyalakan AI saat zombie masuk jarak.
- **Pemain di dalam base:** dinding menghalangi garis pandang, tetapi tembakan tetap terdengar sampai `noiseGunshot * wallMuffle`. Serangan ke dinding mengikuti #78.
- **`Submerged`, `Dormant`, dan `BurnToAshes`:** `Tick` dilewati. `Dormant` tetap mendengar suara (#112).
- **Banyak suara dalam satu frame:** setiap event ditangani terpisah, dengan batas `Detection01 = 1`.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** kelas `AISensory` dan `NoiseEmitter` (`GLOSSARY.md`). Mask `AIVision` (#87). `NoiseEmittedEvent` (#107). Blood Moon mengabaikan Stealth (#50). Tombol jongkok ditahan (#18).

**Keputusan teknis di blueprint ini:**

- Noise Sphere hanya angka, bukan collider.
- `Tick` dibagi ke beberapa frame, dan raycast suara hanya untuk lapisan luar radius.
- Zombie memakai `AISensory`, sedangkan hewan memakai radius aggro di #61.

**Usulan yang disetujui developer sebagai nilai awal:**

1. **Sistem stealth:** kini aturan GDD bagian 3, jadi dipakai dan tidak lagi menjadi usulan.
2. **Aturan "Aggro penuh" di `Claude.md` terhadap stealth.** Rekomendasi: zombie malam selalu bergerak ke `HuntHint`, tetapi baru mengejar langsung setelah `Aware`.
3. **Semua angka di bagian 1**, yaitu 22 m, 120 derajat, dan radius suara. Rekomendasi: pakai nilai awal, lalu setel lewat playtest.

## 6. Skenario Tes

1. Berdiri di belakang zombie pada jarak 10 m, lalu diam. **Hasil:** `Detection01` tidak naik dan ikon mata tersembunyi.
2. Berdiri di depan zombie pada jarak 20 m. **Hasil:** mata terbuka perlahan dan menjadi merah setelah sekitar 2 detik.
3. Ulangi sambil jongkok. **Hasil:** jarak 20 m tidak terdeteksi, karena jangkauan turun menjadi 13,2 m.
4. Berdiri di balik dinding. **Hasil:** raycast terhalang dan nilai tidak naik.
5. Berlari di belakang zombie pada jarak 15 m. **Hasil:** zombie menjadi `Suspicious` dan berjalan ke titik suara.
6. Berjalan jongkok di belakang zombie pada jarak 5 m. **Hasil:** tidak terdengar.
7. Tembak pistol pada jarak 35 m. **Hasil:** zombie langsung `Aware`.
8. Lari dari zombie sampai hilang dari pandangan selama lebih dari 6 detik. **Hasil:** turun ke `Suspicious`, lalu menuju titik terakhir.
9. Picu Blood Moon lewat `DevConsole`. **Hasil:** semua zombie `Aware` dan ikon mata merah.
10. Kembalikan zombie `Aware` ke pool, lalu munculkan lagi. **Hasil:** `Detection01 = 0`.
11. Buka 50 zombie di Profiler. **Hasil:** paling banyak 6 sensor per frame, dan `Publish` tidak menghasilkan GC.
12. Setel `stealth.enabled = false`. **Hasil:** zombie langsung mengejar, dan ikon mata mati.

## 7. Catatan Implementasi (Langkah 5)

Kode: `left-4-dead/AISensory.cs` (`AwarenessLevel`, `NoiseSourceKind`), `AISensoryHub.cs`, `NoiseEmitter.cs`.

- `HuntHint` memakai `UnityEngine.Random`, karena tidak memengaruhi isi dunia yang disimpan. Tarikan cahaya (#98) yang memakai aliran `Gameplay` masih TODO.
- `IgnoreStealth` berupa properti yang diisi `HordeManager` (#50) nanti. `DetectionMeterUI` belum ditulis, tetapi `PlayerDetectionChangedEvent` sudah dikirim.
- `NoiseEmitter.Player` (statis) dan `NoiseEmitter.Emit(Vector3, float, NoiseSourceKind)` ditambahkan untuk sumber suara tanpa komponen sendiri.
