# Blueprint: AIDirector.cs
**Lokasi:** `Survival Island/doc/blueprint/left-4-dead/AIDirector.md`
**Deskripsi Utama:** Mengatur tempo serangan zombie setiap malam dan kurva kesulitan menuju hari ke-60 serta sesudahnya. Director menentukan berapa banyak dan seberapa beringas, sedangkan `SpawnManager` (#62) menentukan jenis dan tempatnya.
**Issue:** #27
**Rujukan GDD:** Bagian 3 (Horde malam, dan zombie semakin kuat setelah hari ke-60) dan bagian 5 (ancaman malam per region). Aturan zombie semakin kuat ada di GDD, dan pengali setelah hari ke-60 adalah keputusan developer sebagai nilai awal. Siklus empat tahap dan angka lainnya adalah usulan dari `Survival Island/doc/CATATAN_MEKANIK.md`, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

`AIDirector` adalah Singleton di scene `IslandPhase`, sesuai `RunLifecycle.md`, dan memakai `ISaveable`. Director hanya berjalan saat malam. Hewan siang diatur #61 dan #62.

```csharp
public enum DirectorPhase { Inactive, BuildUp, SustainPeak, PeakFade, Relax }

[Serializable]
public struct ThreatKey
{
    public int day;
    public int mobSize, maxAlive, nightBudget, maxLarge;
    public float mobIntervalMin, mobIntervalMax, specialShare;
    public float hpMultiplier, speedMultiplier, damageMultiplier, recoveryMultiplier;
}

public struct WaveModifiers { public float hp, speed, damage, recovery; }

public struct WaveRequest
{
    public int count, maxLarge;
    public float specialShare;
    public WaveModifiers modifiers;
    public string reason;           // Mob, Panic, CampCounter, RemoteTakeover (#90)
    public string forcedEnemyId;    // Kosong kecuali CampCounter
    public Vector3? origin;         // #90. null berarti sekitar pemain
}
```

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `CurrentPhase` | `DirectorPhase` | `Inactive` | Tahap saat ini |
| `Intensity` | `float` | 0 | Ketegangan pemain, 0 sampai 1 |
| `Threat` | `ThreatKey` | Hari 1 | Hasil kurva untuk malam ini |
| `spawnedTonight` | `int` | 0 | Zombie yang sudah diminta malam ini |
| `mobTimer` | `float` | `firstWaveDelaySeconds` | Sisa waktu ke mob berikutnya |
| `heldSeconds` | `float` | 0 | Lama mob tertahan setelah timer habis |
| `phaseTimer` | `float` | 0 | Waktu di tahap ini |
| `isHeld` | `bool` | `false` | Sedang tidur |
| `isOverridden` | `bool` | `false` | Blood Moon sedang berjalan |
| `relaxStartPos` | `Vector3` | Nol | Posisi pemain saat Relax mulai |

### Kurva ancaman (usulan)

`GameBalanceConfig.aiDirector.threatCurve` berisi `ThreatKey[]`. Nilai di antara dua baris dihitung secara linear. Satu malam berlangsung sekitar 450 detik.

| Hari | `mobSize` | Jeda mob (dtk) | `maxAlive` | `nightBudget` | `specialShare` | `maxLarge` | HP | Kecepatan | Damage | Jeda serang |
|---|---|---|---|---|---|---|---|---|---|---|
| 1 | 6 | 100-140 | 12 | 24 | 0,10 | 1 | 1,00 | 1,00 | 1,00 | 1,00 |
| 10 | 9 | 90-125 | 15 | 40 | 0,15 | 1 | 1,05 | 1,00 | 1,00 | 0,97 |
| 20 | 13 | 80-110 | 18 | 60 | 0,20 | 2 | 1,10 | 1,03 | 1,05 | 0,94 |
| 30 | 17 | 70-100 | 22 | 85 | 0,30 | 2 | 1,20 | 1,06 | 1,10 | 0,90 |
| 40 | 21 | 60-90 | 25 | 110 | 0,35 | 3 | 1,30 | 1,09 | 1,15 | 0,85 |
| 50 | 26 | 55-80 | 28 | 140 | 0,40 | 3 | 1,40 | 1,12 | 1,20 | 0,80 |
| 60 | 30 | 45-70 | 30 | 180 | 0,50 | 4 | 1,50 | 1,15 | 1,25 | 0,75 |

- **Jumlah:** `mobSize` dan `maxAlive` berhenti di 30 sampai hari ke-60, mengikuti batas di `CATATAN_MEKANIK.md`. Mega mob 50 milik #50.
- **Jenis:** `specialShare` adalah porsi varian khusus dalam satu gelombang. Jenisnya diambil dari `RegionSpawnTable` (#62), jadi Region 1 tetap hanya berisi zombie standar, sesuai GDD.
- **Beringas:** selain darah dan damage, jeda serangan makin pendek, mengikuti saran "kesulitan mengubah perilaku" di `REFERENSI_MUSUH_L4D.md`.

### Setelah hari ke-60 (keputusan developer)

GDD bagian 3 menetapkan zombie semakin kuat setelah hari ke-60, tetapi tidak memberi angka. Karena itu kurva tidak berhenti pada baris hari 60. Baris itu menjadi nilai dasar, lalu `EvaluateCurve(day)` mengalikan hasilnya:

```
hariLewat     = Max(0, day - GameManager.TargetDay)
pengaliJumlah = Min(postTargetCountMax,  1 + postTargetCountPerDay  * hariLewat)
pengaliHP     = Min(postTargetHpMax,     1 + postTargetHpPerDay     * hariLewat)
pengaliDamage = Min(postTargetDamageMax, 1 + postTargetDamagePerDay * hariLewat)
```

| Nilai `ThreatKey` | Setelah hari ke-60 |
|---|---|
| `mobSize`, `nightBudget` | Dikali `pengaliJumlah`, lalu dibulatkan |
| `hpMultiplier` | Dikali `pengaliHP` |
| `damageMultiplier` | Dikali `pengaliDamage` |
| `maxAlive`, `maxLarge`, kecepatan, jeda serang, `specialShare` | Tetap seperti baris hari 60 |

| Hari | `mobSize` diminta | `nightBudget` | HP | Damage |
|---|---|---|---|---|
| 60 | 30 | 180 | 1,50 | 1,25 |
| 70 | 36 | 216 | 1,80 | 1,38 |
| 85 | 45 | 270 | 2,25 | 1,56 |
| 110 ke atas | 45 | 270 | 3,00 | 1,88 |

- **Batas performa:** zombie aktif tetap dibatasi `maxAlive` 30 lewat langkah 4 di `TrySpawnMob`, sama dengan `SpawnManager.maxZombiesAlive`, dan di bawah target 30 sampai 50 zombie di `HordeCrowdManager`. Jadi `nightBudget` menambah total zombie per malam tanpa menambah beban per frame.
- **Batas adil:** semua pengali berhenti pada batasnya, dan kecepatan tetap di bawah `maxZombieSpeed`, supaya run yang sangat panjang tetap bisa dimenangkan.
- **Berlaku sama untuk semua spawn zombie:** Blood Moon (#50) dan penyergapan (#67, #71) memakai pengali yang sama, lewat `GetPostTargetCountMultiplier` dan `GetPostTargetModifiers`. Hewan tidak memakainya.

### Angka penyetelan lain

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.aiDirector.regionMultiplierVolcano` | 0,8 | Pengali jumlah Region 1 |
| `GameBalanceConfig.aiDirector.regionMultiplierAmazon` | 1 | Pengali jumlah Region 2 |
| `GameBalanceConfig.aiDirector.regionMultiplierSnow` | 1 | Pengali jumlah Region 3 |
| `GameBalanceConfig.aiDirector.regionMultiplierLogistic` | 1,2 | Pengali jumlah Region 4, sesuai "secara masif" di GDD |
| `GameBalanceConfig.aiDirector.firstWaveDelaySeconds` | 20 | Jeda mob pertama setelah malam tiba |
| `GameBalanceConfig.aiDirector.minBuildUpSeconds` | 15 | Waktu minimum Build Up |
| `GameBalanceConfig.aiDirector.peakThreshold` | 0,9 | Intensitas untuk masuk puncak |
| `GameBalanceConfig.aiDirector.sustainMinSeconds` / `sustainMaxSeconds` | 3 / 5 | Lama puncak |
| `GameBalanceConfig.aiDirector.fadeMaxSeconds` | 30 | Batas Peak Fade |
| `GameBalanceConfig.aiDirector.relaxMinSeconds` / `relaxMaxSeconds` | 30 / 45 | Lama istirahat |
| `GameBalanceConfig.aiDirector.relaxBreakDistance` | 60 | Relax berhenti kalau pemain menjauh sejauh ini, dalam meter |
| `GameBalanceConfig.aiDirector.intensityDecayPerSec` | 0,033 | Dari 1 ke 0 dalam sekitar 30 detik |
| `GameBalanceConfig.aiDirector.intensityPerPlayerDamage` | 0,01 | Per 1 damage yang diterima pemain |
| `GameBalanceConfig.aiDirector.intensityPerPin` | 0,3 | Saat pemain tertahan |
| `GameBalanceConfig.aiDirector.intensityPerNearKill` | 0,2 | Musuh mati pada jarak paling dekat |
| `GameBalanceConfig.aiDirector.nearKillMin` / `nearKillMax` | 3 / 10 | Nilai menurun sampai nol pada jarak ini |
| `GameBalanceConfig.aiDirector.minWaveSize` | 3 | Di bawah ini mob ditunda |
| `GameBalanceConfig.aiDirector.waveRetrySeconds` | 5 | Jeda mencoba lagi |
| `GameBalanceConfig.aiDirector.heldGrowthSeconds` | 60 | Waktu mob tertahan untuk mencapai ukuran maksimum |
| `GameBalanceConfig.aiDirector.heldGrowthMax` | 0,5 | Tambahan ukuran paling besar |
| `GameBalanceConfig.aiDirector.holdMobLargeCount` | 3 | Mob ditahan kalau varian besar sebanyak ini masih hidup |
| `GameBalanceConfig.aiDirector.maxZombieSpeed` | 7,8 | Batas kecepatan, di bawah lari pemain 8 m/s |
| `GameBalanceConfig.aiDirector.postTargetCountPerDay` / `postTargetCountMax` | 0,02 / 1,5 | Pengali jumlah per hari setelah hari ke-60, dan batasnya (keputusan developer) |
| `GameBalanceConfig.aiDirector.postTargetHpPerDay` / `postTargetHpMax` | 0,02 / 2 | Pengali HP per hari setelah hari ke-60, dan batasnya (keputusan developer) |
| `GameBalanceConfig.aiDirector.postTargetDamagePerDay` / `postTargetDamageMax` | 0,01 / 1,5 | Pengali damage per hari setelah hari ke-60, dan batasnya (keputusan developer) |

## 2. Detail Logika & Fungsi

### `HandleNightfall(NightfallEvent e)`

1. `Threat = EvaluateCurve(GameManager.Instance.currentDay)`.
2. `spawnedTonight = 0`, `Intensity = 0`, dan `mobTimer = firstWaveDelaySeconds`.
3. `SetPhase(BuildUp)`.

### `HandleDaybreak(DaybreakEvent e)`

`SetPhase(Inactive)`. Pembakaran zombie milik #63, bukan Director.

### `Update()`

Keluar lebih awal kalau `CurrentPhase == Inactive`, `isHeld`, `isOverridden`, atau `Time.deltaTime == 0`.

1. `Intensity = Max(0, Intensity - intensityDecayPerSec * dt)`.
2. `phaseTimer += dt`.
3. Jalankan tahap:

| Tahap | Tindakan | Keluar ke | Syarat keluar |
|---|---|---|---|
| `BuildUp` | `mobTimer -= dt`. Kalau `<= 0`, panggil `TrySpawnMob()` | `SustainPeak` | `Intensity >= peakThreshold` dan `phaseTimer >= minBuildUpSeconds` |
| `SustainPeak` | Tidak ada mob baru | `PeakFade` | Timer acak `sustainMinSeconds` sampai `sustainMaxSeconds` habis |
| `PeakFade` | Tidak ada mob baru. Menunggu situasi reda | `Relax` | `Intensity < peakThreshold` dan `AliveLarge < holdMobLargeCount`, atau `phaseTimer >= fadeMaxSeconds` |
| `Relax` | Tidak ada mob baru. Pemain memperbaiki base | `BuildUp` | Timer acak `relaxMinSeconds` sampai `relaxMaxSeconds` habis, atau pemain menjauh `relaxBreakDistance` |

`SetPhase` mengosongkan `phaseTimer`, dan menerbitkan `DirectorPhaseChangedEvent { phase }`.

### `private void TrySpawnMob()`

1. Kalau `SpawnManager.Instance.AliveLarge >= holdMobLargeCount`, `heldSeconds += dt`, lalu keluar.
2. `grow = 1 + Min(heldSeconds / heldGrowthSeconds, 1) * heldGrowthMax`.
3. `want = RoundToInt(Threat.mobSize * regionMult * difficulty.enemyCountMultiplier * grow)`. `regionMult` dari `RegionChangedEvent.Last.to`.
4. `count = Min(want, Threat.maxAlive - AliveZombies, Threat.nightBudget - spawnedTonight)`.
5. Kalau `count < minWaveSize`, `mobTimer = waveRetrySeconds`, `heldSeconds += waveRetrySeconds`, lalu keluar. Kalau `nightBudget` sudah habis, Director tidak meminta mob lagi malam itu.
6. Panggil `int SpawnManager.Instance.RequestWave(in WaveRequest r)`, dengan alasan `Mob`. Nilai kembalinya adalah jumlah yang diterima (#62). Slot yang gagal dibuang `SpawnManager`, lalu memanggil `RefundBudget(1)`.
7. `spawnedTonight += jumlah yang diterima`, `heldSeconds = 0`, dan `mobTimer` diisi nilai acak dari jeda mob lewat `RunSeedManager.Instance.GetStream(RandomStreamId.Enemy)`.

**`BuildModifiers()`** mengisi `WaveModifiers` dari `Threat`. `damage` **tidak** dikali `difficulty.enemyDamageMultiplier`, karena pengali kesulitan hanya diterapkan sekali di `SurvivalStats.TakeDamage` (`DamagePipeline.md`, langkah 4). `EnemyAttack` membaca `damage` dan `recovery` dari `EnemyAI.waveModifiers` (#109). `SpawnManager` mengisi `EnemyHealth.hpMultiplier`, lalu `EnemyAI.ApplyWaveModifiers(in modifiers)`. Kecepatan akhir dibatasi `maxZombieSpeed`, kecuali terkaman Zombie Desa (`SpeedSource.Pounce`) dan `SpeedSource.EmergeBoost` yang sementara (`EnemyAI.md` bagian Kecepatan).

### Intensitas

| Event | Tambahan |
|---|---|
| `PlayerDamagedEvent` | `damage * intensityPerPlayerDamage` |
| `PlayerPinnedEvent` dengan `isPinned` | `intensityPerPin` |
| `EnemyKilledEvent` dalam `nearKillMax` | `intensityPerNearKill * InverseLerp(nearKillMax, nearKillMin, jarak)` |

### Fungsi publik

| Fungsi | Pemanggil | Isi |
|---|---|---|
| `RequestPanicWave(int size, string reason)` | #14, #70 | Melewati tahap dan `heldSeconds`, tetapi tetap mematuhi `maxAlive`. Menambah `spawnedTonight` |
| `RequestCampCounter(string enemyId, int count)` | #94 | Gelombang dengan `forcedEnemyId`. Boleh saat Relax, dan tidak memakai `nightBudget` |
| `RefundBudget(int count)` | `SpawnManager.Despawn` (alasan `Stuck`, `OffNavMesh`, `TooFar`, `RegionUnloaded`) dan slot `RequestWave` yang gagal | Mengurangi `spawnedTonight`, paling kecil 0, supaya zombie yang hilang tanpa dibunuh tidak memotong malam itu. Zombie yang dibunuh, terbakar, atau ditumbalkan tidak dikembalikan |
| `GetPostTargetModifiers(int day)` | #50, #62, #71 | Mengembalikan `WaveModifiers` berisi `pengaliHP` dan `pengaliDamage`, sedangkan kecepatan dan jeda serang bernilai 1. Sampai hari ke-60, semua nilainya 1 |
| `GetPostTargetCountMultiplier(int day)` | #50, #67, #71 | Mengembalikan `pengaliJumlah`. Sampai hari ke-60, nilainya 1. Pemanggil membulatkan hasilnya, dan batas zombie aktif tetap berlaku |
| `AddExternalSpawn(int count)` | `SunlightExposure.ReportEmergence()` saat `NightfallEvent` (#63, #62) | Menambah `spawnedTonight` untuk zombie yang bangkit dari tempat sembunyi. Kalau `count >= mobSize`, mob pertama dilewati |

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `DirectorPhaseChangedEvent`.

**Event yang didengar:** `NightfallEvent`, `DaybreakEvent`, `PlayerDamagedEvent`, `PlayerPinnedEvent`, `EnemyKilledEvent`, `RegionChangedEvent`, `SleepStateChangedEvent`, `BloodMoonStartedEvent`, dan `BloodMoonEndedEvent`.

| Sistem | Hubungan |
|---|---|
| `SpawnManager` (#62) | Dipanggil langsung lewat `RequestWave`. Menyediakan `AliveZombies` dan `AliveLarge` |
| `HordeManager` (#50) | Saat Blood Moon, `isOverridden = true`. Spawn massal milik #50, dan memakai `GetPostTargetCountMultiplier` serta `GetPostTargetModifiers` setelah hari ke-60 |
| `SleepSystem` (#67) | Saat tidur, `isHeld = true`. Anggaran dan tahap ditahan. Penyergapan memakai `RequestAmbush`, dengan pengali jumlah dari `GetPostTargetCountMultiplier` |
| `BeachSupplyManager` (#71) | Penyergapan zombie memakai `GetPostTargetCountMultiplier` dan `GetPostTargetModifiers` |
| `SunlightExposure` (#63) | Zombie Desa yang bangkit saat malam dihitung lewat `AddExternalSpawn(count)`, dan mengurangi mob pertama |
| `CombatMusicDirector` (#114) | Membaca `DirectorPhaseChangedEvent` dan `Intensity` |
| `HordeCrowdManager` (#93) | Batas `maxAlive` menentukan bebannya, termasuk setelah hari ke-60. Manager itu tidak mengatur jumlah |
| `DevConsole` (#56) | Mendaftarkan `director`, `directorphase <nama>`, dan memakai `setday`, `settime`, serta `balance` |

## 4. Kasus Khusus

- **Objek pool:** Director bukan objek pool. Pengali diisi sesudah `OnSpawned`, dan `EnemyAI.OnSpawned` selalu mengembalikannya ke 1 lebih dulu.
- **Game dijeda:** `Time.deltaTime` nol, jadi timer dan intensitas berhenti.
- **Permadeath:** `currentDay` kembali ke 1 lewat `RunLifecycle`, dan seluruh status hilang bersama scene.
- **Save dan load:** `SaveId = ai_director`, `LoadOrder = 130` (sesudah `world_npcs`), menyimpan `spawnedTonight`, `CurrentPhase`, `Intensity`, `mobTimer`, dan `phaseTimer`. Keluar lalu masuk lagi tidak mengisi ulang anggaran. Musuh sendiri tidak disimpan (#88). `OnRestoreFinished` menghitung ulang `Threat`.
- **Tidur melewati fajar:** `DaybreakEvent` tetap datang, dan Director menjadi `Inactive`.
- **Pemain berpindah region:** `Threat` tetap, tetapi `regionMult` dan isi `RegionSpawnTable` mengikuti region baru pada mob berikutnya.
- **`setday` di tengah malam:** Director memakai hari baru mulai `NightfallEvent` berikutnya. Perintah `director refresh` menghitung ulang seketika.
- **Hari di luar kurva:** nilai di bawah hari 1 memakai baris hari 1. Setelah hari ke-60, nilainya memakai baris hari 60 ditambah pengali di bagian 1. Tidak ada batas hari, hanya batas pengali.
- **Pemain di dalam base:** Director tetap berjalan. Base memang tempat berlindung, dan zombie menyerang dindingnya (#78).

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** Director mendengar `EnemyKilledEvent` (#107), `hpMultiplier` diisi lewat `SpawnManager` (#59), dan Director menahan anggaran saat tidur (#67).

**Keputusan developer:** setelah hari ke-60, nilai awal pengalinya jumlah +2 persen, HP +2 persen, dan damage +1 persen per hari, dengan batas 1,5 kali, 2 kali, dan 1,5 kali. Blood Moon dan penyergapan memakai pengali yang sama. Angkanya tetap perlu diuji bersama jumlah peluru.

**Usulan yang disetujui developer sebagai nilai awal (dicatat di #85):**

1. **Siklus empat tahap:** rekomendasi dipakai saat malam saja, supaya serangan datang bergelombang dan pemain sempat memperbaiki base.
2. **Kurva ancaman:** rekomendasi tabel di bagian 1, dari 24 zombie per malam pada hari 1 sampai 180 pada hari 60. Perlu diuji bersama jumlah peluru.
3. **Pengali Region 4:** rekomendasi 1,2 kali, sebagai arti "secara masif" di GDD.
4. **Batas kecepatan:** rekomendasi 7,8 m/s, supaya pemain tanpa beban tetap bisa kabur.
5. **Hewan siang:** rekomendasi tidak diskalakan Director. Issue ini hanya menyebut zombie, dan GDD menyebut hewan sebagai penjaga teritori.
6. **Zombie aktif setelah hari ke-60:** rekomendasi `maxAlive` tetap 30, supaya performa tidak berubah. Alternatifnya, naik sampai 40 lewat `capOverride`, tetapi perlu diuji dengan `HordeCrowdManager`.

## 6. Skenario Tes

1. `setday 1`, `settime 17.9`, tunggu satu malam. **Hasil:** mob pertama sekitar 20 detik setelah malam tiba, dan total zombie tidak lebih dari 24 di Region 2.
2. `setday 60`, ulangi. **Hasil:** mob berukuran sekitar 30, `maxAlive` 30, dan total sekitar 180.
3. Biarkan zombie memukul pemain. **Hasil:** intensitas naik, tahap berganti ke `SustainPeak`, `PeakFade`, lalu `Relax`. Perintah `director` menampilkan nilainya.
4. Saat Relax, berlari 60 meter. **Hasil:** kembali ke `BuildUp` sebelum timer habis.
5. `setday 20` di Region 3, dan jaga 3 Zombie Militer tetap hidup. **Hasil:** mob ditahan, lalu ukurannya lebih besar setelah Militer mati.
6. Tidur saat malam. **Hasil:** tidak ada gelombang baru, dan `phaseTimer` berhenti.
7. Mulai Blood Moon lewat #50. **Hasil:** Director berhenti, lalu lanjut setelah `BloodMoonEndedEvent`.
8. Di Region 1, `setday 60`, dan `setday 85`. **Hasil:** hanya zombie standar yang muncul pada keduanya.
9. Keluar ke menu saat malam, lalu lanjutkan. **Hasil:** `spawnedTonight` tetap, dan anggaran tidak terisi ulang.
10. Jalankan dengan seed yang sama, tanpa perbedaan tindakan. **Hasil:** jeda mob sama.
11. `setday 70`, tunggu malam, lalu `director`. **Hasil:** `nightBudget` 216, HP 1,8 kali, damage sekitar 1,38 kali, dan `maxAlive` tetap 30.
12. `setday 200`, tunggu malam. **Hasil:** `nightBudget` 270, HP 3 kali, damage sekitar 1,88 kali, zombie aktif tidak lebih dari 30, dan FPS dicatat ke lembar playtest.
13. Panggil `GetPostTargetModifiers` dan `GetPostTargetCountMultiplier` pada hari 60, 70, dan 200. **Hasil:** hari 60 semua 1, hari 70 jumlah 1,2, HP 1,2, dan damage 1,1, serta hari 200 berhenti pada 1,5, 2, dan 1,5.
14. Pakai profil kesulitan uji dengan `enemyDamageMultiplier` 2, lalu biarkan zombie standar hari 1 memukul pemain. **Hasil:** damage 30, bukan 60, karena pengali kesulitan hanya dipakai `SurvivalStats`.
15. Bunuh 5 zombie, lalu `director`. **Hasil:** `spawnedTonight` tidak berkurang. Kembalikan 1 zombie lewat pengaman macet. **Hasil:** `spawnedTonight` berkurang 1, sekali saja.
