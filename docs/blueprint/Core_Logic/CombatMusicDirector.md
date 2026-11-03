# Blueprint: CombatMusicDirector.cs
**Lokasi:** `docs/blueprint/Core_Logic/CombatMusicDirector.md`
**Deskripsi Utama:** Mengatur musik sesuai ketegangan pertempuran dan suara penanda musuh 3D, supaya pemain bisa mendengar jenis ancaman dan arahnya.
**Issue:** #114
**Rujukan GDD:** Bagian 3 (ancaman siang dan malam), bagian 5 (varian per region), dan bagian 6 (bencana). Lapisan musik dan suara penanda adalah usulan.

## 1. Arsitektur & Variabel Utama

`CombatMusicDirector` adalah manager persisten di `PersistentRoot` dan memakai `IBootstrapService`. `ThreatAudioEmitter` adalah komponen di prefab musuh dan memakai `IPoolable`. Sistem ini aktif saat `GamePhase.IslandSurvival`. Musik Fase 1 milik `ShipTimerManager`.

```csharp
public enum MusicTier { Calm, Alert, Combat, Peak }
public enum ThreatCueKind { Idle, Alert }

[CreateAssetMenu(menuName = "Survival/Music Set")]
public class MusicSet : ScriptableObject
{
    public AudioClip calmDay, calmNight;           // Pengganti dayBGM dan nightBGM
    public AudioClip alertStem, combatStem, peakStem;  // combatStem menggantikan dangerBGM
}

[CreateAssetMenu(menuName = "Survival/Enemy Cue Set")]
public class EnemyCueSet : ScriptableObject
{
    public AudioClip[] idleClips, alertClips;
    public float idleMinInterval = 5f, idleMaxInterval = 12f;
    public float audibleRadius = 25f;
    public int priority = 1;                       // Nilai besar lebih sulit digantikan
    public bool isLarge;                           // Bobot 3 saat menghitung ancaman dekat
}
```

### Variabel `CombatMusicDirector`

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Instance` | `CombatMusicDirector` | `null` | Singleton |
| `CurrentTier` | `MusicTier` | `Calm` | Lapisan aktif |
| `score` | `float` | 0 | Ketegangan gabungan, 0 sampai 1 |
| `damageScore` | `float` | 0 | Dari damage yang diterima |
| `tierHoldTimer`, `riseTimer` | `float` | 0 | Pencegah perubahan terlalu sering |
| `tierFloor` | `MusicTier` | `Calm` | Batas bawah dari Blood Moon atau bencana |
| `stems` | `AudioSource[5]` | Satu per klip | Berjalan serentak dengan `PlayScheduled` |
| `crowdBedSource` | `AudioSource` | 3D | Suara kerumunan horde |

Semua sumber musik memakai `ignoreListenerPause = true` dan keluar ke grup mixer `Music`.

### Variabel `ThreatAudioEmitter`

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Active` (static) | `List<ThreatAudioEmitter>` | Kosong | Daftar musuh aktif, tanpa query fisika |
| `cues` | `EnemyCueSet` | Dari prefab | Data suara |
| `source` | `AudioSource` | Satu per prefab | `spatialBlend = 1`, grup `EnemyCue` |
| `lowpass` | `AudioLowPassFilter` | 22000 Hz | Peredam suara di balik dinding |
| `nextIdleTime` | `float` | Acak | Jadwal suara |
| `lastEngaged` | `bool` | `false` | Pendeteksi masuk `Chase` |

`IsEngaged` membaca `EnemyAI.currentState` (`Chase`, `AttackPlayer`, `AttackBuilding`) atau `WildlifeAI.State` (`Chase`, `Attack`). `TargetUnreachable` tidak dihitung, agar hewan di seberang jurang tidak mengubah musik.

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.combatMusic.alertThreshold` | 0,2 | Masuk `Alert` |
| `GameBalanceConfig.combatMusic.combatThreshold` | 0,45 | Masuk `Combat` |
| `GameBalanceConfig.combatMusic.peakThreshold` | 0,85 | Masuk `Peak` |
| `GameBalanceConfig.combatMusic.hysteresis` | 0,1 | Selisih ambang saat turun |
| `GameBalanceConfig.combatMusic.riseConfirmSeconds` | 0,5 | Syarat naik tingkat |
| `GameBalanceConfig.combatMusic.minTierHoldSeconds` | 12 | Waktu minimum sebelum turun |
| `GameBalanceConfig.combatMusic.riseFadeSeconds` | 1,5 | Crossfade saat naik |
| `GameBalanceConfig.combatMusic.fallFadeSeconds` | 6 | Crossfade saat turun |
| `GameBalanceConfig.combatMusic.nearRadius` | 25 | Radius musuh dekat |
| `GameBalanceConfig.combatMusic.nearSaturation` | 6 | Bobot yang membuat nilai dekat menjadi 1 |
| `GameBalanceConfig.combatMusic.damageScorePerPoint` | 0,02 | Tambahan per 1 damage |
| `GameBalanceConfig.combatMusic.damageScoreDecayPerSec` | 0,05 | Penurunan `damageScore` |
| `GameBalanceConfig.combatMusic.pinnedScore` | 0,7 | Nilai minimum saat tertahan |
| `GameBalanceConfig.combatMusic.maxEnemyVoices` | 8 | Batas suara musuh serentak |
| `GameBalanceConfig.combatMusic.maxSameVariantVoices` | 3 | Batas per `enemyId` |
| `GameBalanceConfig.combatMusic.occlusionCheckSeconds` | 0,25 | Jeda pemeriksaan dinding |
| `GameBalanceConfig.combatMusic.occlusionLowpassHz` | 1500 | Suara di balik dinding |
| `GameBalanceConfig.combatMusic.crowdBedMinEngaged` | 12 | Jumlah musuh untuk suara kerumunan |
| `GameBalanceConfig.combatMusic.disasterMusicDuckDb` | -6 | Musik saat bencana |
| `GameBalanceConfig.combatMusic.stingerMinInterval` | 3 | Jeda antar-peringatan |

## 2. Detail Logika & Fungsi

### Lapisan musik

| Tingkat | Yang terdengar | Situasi |
|---|---|---|
| `Calm` | `calmDay` atau `calmNight` | Tidak ada ancaman |
| `Alert` | `Calm` lebih pelan, ditambah `alertStem` | Musuh mendekat, atau Director di `BuildUp` |
| `Combat` | `alertStem` dan `combatStem` | Sedang bertempur |
| `Peak` | `combatStem` dan `peakStem` | Tekanan tertinggi, misalnya `SustainPeak` |

Semua stem mempunyai tempo dan panjang sama, serta dimulai dengan `PlayScheduled(AudioSettings.dspTime + 0.1)` yang sama. Pergantian tingkat hanya mengubah volume, sehingga ketukan tetap selaras. `calmDay` dan `calmNight` saling crossfade selama 10 detik saat `DaybreakEvent` dan `NightfallEvent`.

### Sumber intensitas

**`EvaluateScore()`** berjalan 4 kali per detik:

```text
proximity = Min(1, jumlah bobot emitter IsEngaged dalam nearRadius / nearSaturation)
director  = AIDirector.Instance ? AIDirector.Instance.Intensity : 0
phase     = PhaseFloor(tahap dari DirectorPhaseChangedEvent)
pinned    = sedang tertahan ? pinnedScore : 0
score     = Max(proximity, damageScore, director, phase, pinned)
```

| `DirectorPhase` | `PhaseFloor` |
|---|---|
| `Inactive` | 0 |
| `Relax` | 0,1 |
| `BuildUp` | 0,25 |
| `PeakFade` | 0,5 |
| `SustainPeak` | 0,85 |

- **Musuh dekat:** bobot 1, atau 3 kalau `cues.isLarge`, sehingga Beruang Kutub dan Zombie Militer terasa lebih berbahaya.
- **Damage:** `PlayerDamagedEvent` menambah `damageScore` sebesar `amount * damageScorePerPoint`, lalu nilainya turun tiap detik.
- **Siang hari:** Director berada di `Inactive`, sehingga musik bergantung pada hewan dekat dan damage.

### Crossfade supaya tidak terlalu sering berubah

**`UpdateTier(dt)`**

1. `target` adalah tingkat dari `score`, dengan `tierFloor` sebagai batas bawah.
2. **Naik:** `riseTimer += dt`. Kalau mencapai `riseConfirmSeconds`, pindah langsung ke `target`, boleh melompat beberapa tingkat. Kena damage melewati penantian ini.
3. **Turun:** `score` harus di bawah `ambang - hysteresis`, dan `tierHoldTimer` harus habis. Turun hanya satu tingkat setiap kali, dan `tierHoldTimer = minTierHoldSeconds` diatur ulang.
4. Saat tingkat berubah, `FadeStems()` mengubah volume dengan `riseFadeSeconds` atau `fallFadeSeconds`.

`FadeStems` memakai `Time.unscaledDeltaTime`, mengikuti aturan `UIFocusStack.md`, supaya fade tidak tersangkut saat jeda.

### Suara penanda per varian dan hewan

| `enemyId` | Karakter suara | `audibleRadius` | `priority` |
|---|---|---|---|
| `zombie_standar` | Erangan serak | 20 | 1 |
| `zombie_tribal` | Gumam dan siulan tinggi | 25 | 2 |
| `zombie_lumut` | Tanpa suara saat `Dormant`, kecuali napas dalam 6 m dari prefab #25. Jeritan saat marah | 30 | 3 |
| `zombie_militer` | Napas berat dan bunyi armor | 35 | 4 |
| `zombie_beku` | Es retak dan napas tertahan | 25 | 2 |
| `zombie_desa` | Jeritan pendek. Cipratan air saat `Emerging` | 30 | 3 |
| `hewan_serigala`, `hewan_serigala_salju` | Lolongan, lalu geraman saat `Alert` | 40 | 2 |
| `hewan_babi` | Dengus berat | 20 | 1 |
| `hewan_jaguar` | Geraman rendah saat berada di perch | 15 | 4 |
| `hewan_ular` | Desis | 8 | 3 |
| `hewan_beruang` | Raungan rendah | 45 | 4 |
| `hewan_anjing` | Gonggongan | 30 | 1 |
| `hewan_gagak` | Koakan kawanan | 30 | 1 |

**`ThreatAudioEmitter.Tick(now)`** dipanggil `CombatMusicDirector`, maksimal 16 emitter per frame secara bergiliran. Dengan begitu, musuh tidak perlu `Update` masing-masing:

1. Keluar kalau state musuh `Submerged`, `Dormant`, atau `BurnToAshes`.
2. Kalau `IsEngaged` baru menjadi `true`, minta suara `Alert`.
3. Kalau `now >= nextIdleTime` dan jarak di dalam `audibleRadius`, minta suara `Idle`, lalu jadwalkan ulang secara acak.

Suara serangan tetap milik `EnemyAttack.telegraphClip` (#109), `MossBehavior`, dan `WildlifeAI`. Ketiganya memanggil `emitter.PlayTell(clip)`, supaya memakai sumber 3D dan batas suara yang sama. `PlayTell` memakai `priority + 2`.

### Arah suara 3D

Pengaturan sumber: `spatialBlend = 1`, `rolloffMode = Logarithmic`, `minDistance = 2`, `maxDistance = audibleRadius`, `dopplerLevel = 0`, dan `spread = 60`.

**`UpdateOcclusion()`** berjalan setiap `occlusionCheckSeconds`, hanya untuk suara yang sedang aktif:

```csharp
bool blocked = PhysicsQuery.Raycast(new Ray(listenerPos, toEnemy.normalized), out _, toEnemy.magnitude, PhysicsMasks.AIVision);
lowpass.cutoffFrequency = blocked ? occlusionLowpassHz : 22000f;
```

Musuh tetap terdengar dari balik dinding, tetapi teredam. Pemain tetap tahu arah ancaman saat berada di dalam base.

### Batas suara zombie serentak

**`bool TryAcquireVoice(ThreatAudioEmitter e, int priority, out ThreatAudioEmitter victim)`**

1. Tolak kalau klip yang sama baru dimulai kurang dari 0,15 detik lalu.
2. Kalau `enemyId` sama sudah mencapai `maxSameVariantVoices`, tolak, kecuali `PlayTell`.
3. Kalau jumlah suara belum mencapai `maxEnemyVoices`, terima.
4. Kalau penuh, hitung `nilai = priority * 100 - jarak`. Kalau suara baru lebih tinggi dari suara terendah, hentikan suara terendah dengan fade 0,05 detik, lalu terima. Kalau tidak, tolak.

**Suara kerumunan:** kalau musuh `IsEngaged` di sekitar pemain mencapai `crowdBedMinEngaged`, `crowdBedSource` memutar loop kerumunan di titik tengah musuh tersebut, dengan volume mengikuti jumlahnya. Suara ini mengisi horde besar tanpa menambah puluhan sumber suara, dan tidak dihitung dalam batas di atas.

### Peringatan Blood Moon dan bencana

| Event | Tindakan |
|---|---|
| `BloodMoonWarningEvent` (jam 12:00) | Putar `bloodMoonWarningClip` lewat `PlayStinger`, bersama notifikasi dari `HordeManager` |
| `BloodMoonStartedEvent` | Putar `bloodMoonStartClip`, dan `tierFloor = Alert` |
| `BloodMoonEndedEvent` | `tierFloor = Calm` |
| `DisasterStartedEvent` | Turunkan grup `Music` sebesar `disasterMusicDuckDb`, dan `tierFloor = Alert`. `warningClip` dan `activeLoop` tetap diputar `DisasterManager` lewat grup `Warning` |
| `MajorDisasterStartedEvent` | Tidak menambah suara. `tierFloor` tetap sama |
| `DisasterEndedEvent` | Kembalikan volume, dan lepas `tierFloor` kalau Blood Moon tidak aktif |

**`PlayStinger(clip)`** memutar lewat grup `Warning`. Kalau peringatan lain baru dimulai kurang dari `stingerMinInterval` lalu, klip menunggu dalam antrean berisi 1 klip.

### Saat jeda dan adegan

- **Jeda (#83):** `UIFocusStack` mengatur `AudioListener.pause` dan `SetPausedMix(true)`. Musik memakai `ignoreListenerPause`, sehingga tetap berjalan pada `pausedMusicVolumeDb` (-12). Suara musuh, stinger, dan kerumunan berhenti. `EvaluateScore` memakai `Time.deltaTime`, jadi `score` dan timer ikut berhenti.
- **Panel tanpa jeda:** musik dan suara tetap berjalan seperti biasa.
- **Adegan (#99):** `CinematicStateChangedEvent { isPlaying = true, keepMusic }`. Kalau `keepMusic == false`, stem turun ke 0 selama 1 detik. `isMuted = true` membuat emitter berhenti meminta suara, dan tingkat dikunci. Saat selesai, `score` dihitung ulang dan musik masuk kembali selama 2 detik.
- **Adegan naratif (#99):** mendengar `NarrativeStateChangedEvent`, dan berhenti memilih musik selama `isPlaying` bernilai `true`.
- **Tidur (#67):** `score` dikunci ke 0 selama `isSleeping`. Penyergapan membangunkan pemain, lalu perhitungan kembali normal.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** tidak ada. `DevConsole` (#56) membaca `CurrentTier` dan `score` lewat perintah `music`.

**Event yang didengar:** `DirectorPhaseChangedEvent`, `PlayerDamagedEvent`, `PlayerPinnedEvent`, `BloodMoonWarningEvent`, `BloodMoonStartedEvent`, `BloodMoonEndedEvent`, `DisasterStartedEvent`, `DisasterEndedEvent`, `DaybreakEvent`, `NightfallEvent`, `CinematicStateChangedEvent`, `NarrativeStateChangedEvent`, `SleepStateChangedEvent`, `PhaseChangedEvent`, dan `RunEndedEvent`.

| Sistem | Hubungan |
|---|---|
| `AudioManager` | Pemilik mixer, `SetPausedMix`, dan `StopWorldAudio`. BGM siang dan malam pindah ke sini |
| `AIDirector` (#27) | Sumber `Intensity` dan tahap, hanya dibaca |
| `SpecialZombieAI` (#25), `WildlifeAI` (#61), `EnemyAttack` (#109) | Pemanggil `PlayTell`, dan sumber state |
| `HordeManager` (#50), `DisasterManager` (#70) | Sumber peringatan |
| `UIFocusStack` (#83), `NarrativeSequencer` (#99), `DeathCinematic` (#53) | Jeda dan adegan |
| `AmbientAudio` (#52), `FootstepAudio` (#21), `AIDirector` (#27) | Di luar issue ini: suara latar, langkah, dan tempo spawn |

## 4. Kasus Khusus

- **Pakai ulang dari pool:** `OnSpawned` mengatur `lastEngaged = false`, `nextIdleTime` acak, `cutoffFrequency = 22000`, dan mendaftar ke `Active`. `OnReturned` memanggil `source.Stop()`, melepas slot suara, dan keluar dari `Active` dengan swap-remove.
- **Musuh mati saat bersuara:** suara berhenti dengan fade 0,05 detik saat kembali ke pool. Suara mati dan efeknya milik #95.
- **Region dilepas (#88):** musuh kembali ke pool lewat jalur biasa, sehingga daftar dan slot suara tetap benar.
- **Run berakhir:** `RunEndedEvent` memudarkan stem selama 2 detik dan mengunci tingkat. `AudioManager.StopWorldAudio()` memanggil `StopImmediate()`.
- **Permadeath:** `ResetForNewRun()` mengembalikan `CurrentTier = Calm`, `score = 0`, `tierFloor = Calm`, dan mengosongkan `Active` serta slot suara.
- **Save dan load:** tidak ada data yang disimpan. Setelah load, tingkat mulai dari `Calm`, lalu naik dari event sticky dan kondisi saat itu. `HordeManager` mengirim ulang `BloodMoonStartedEvent` saat restore (#50), sehingga `tierFloor` pulih. Suara `bloodMoonStartClip` dilewati selama 2 detik pertama setelah load, supaya peringatan tidak terdengar sebagai kejadian baru.
- **Penyesuaian volume:** volume pemain dari `SettingsManager` (#19) berlaku lewat grup mixer, bukan lewat `AudioSource.volume`.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** tahap Director (#27), `AudioMixer.updateMode = UnscaledTime` dan volume musik saat jeda (#83), serta referensi combat L4D (#85).

**Usulan yang disetujui developer sebagai nilai awal:**

1. **Empat stem selaras, bukan empat lagu terpisah.** Rekomendasi: stem selaras, karena pergantian lebih halus. Syaratnya, aset musik harus dibuat dengan tempo dan panjang sama.
2. **Sumber ketegangan.** Rekomendasi: nilai terbesar dari musuh dekat, damage, `Intensity`, dan tahap Director, supaya siang hari juga punya musik pertempuran dari hewan.
3. **Batas 8 suara musuh dan 3 per varian, ditambah suara kerumunan.** Rekomendasi: dipakai sebagai nilai awal, lalu disetel lewat playtest.
4. **`BloodMoonWarningEvent` baru.** Rekomendasi: `HordeManager` mengirim event ini, sehingga `CombatMusicDirector` tidak menghitung ulang aturan hari Blood Moon.
5. **Tanda suara Zombie Lumut.** Rekomendasi: tetap hanya napas dalam 6 meter saat `Dormant`, sesuai #25, supaya penyamarannya tidak terbongkar.
6. **Musik Fase 1.** Rekomendasi: tetap milik `ShipTimerManager`, karena tekanan 60 detik sudah tetap.

## 6. Skenario Tes

1. Berdiri diam saat siang tanpa musuh. **Hasil:** `CurrentTier = Calm`, dan `calmDay` berbunyi.
2. Pancing serigala sampai `Chase` dalam 25 m. **Hasil:** musik naik ke `Alert` dalam sekitar 0,5 detik.
3. Biarkan serigala menggigit beberapa kali. **Hasil:** musik naik ke `Combat`, dan ketukannya tidak bergeser.
4. Lari keluar dari jangkauan. **Hasil:** tingkat turun satu per satu, dengan jeda minimal 12 detik.
5. Paksa skor berubah di sekitar ambang setiap 2 detik lewat `DevConsole`. **Hasil:** musik turun paling cepat setiap 12 detik.
6. Malam dengan Director di `SustainPeak`. **Hasil:** `CurrentTier = Peak`, lalu menurun setelah `Relax`.
7. Tutup mata, lalu dengarkan zombie di kiri, kanan, dan belakang. **Hasil:** arahnya bisa ditebak dengan headphone.
8. Berdiri di balik dinding base. **Hasil:** suara zombie teredam, lalu jelas kembali setelah dinding hilang.
9. Spawn 50 zombie standar. **Hasil:** paling banyak 3 suara per varian dan suara kerumunan aktif, tanpa alokasi GC per frame.
10. Spawn Zombie Militer di tengah horde. **Hasil:** suaranya mendapat slot dan menggantikan suara prioritas rendah.
11. Hari Blood Moon, jam 12:00. **Hasil:** peringatan dan notifikasi muncul sekali. Saat malam tiba, musik minimal `Alert`.
12. `disaster Eruption 1`. **Hasil:** musik turun 6 dB, peringatan dari `DisasterManager` terdengar, dan tidak ada peringatan ganda.
13. Buka Pause saat `Combat`. **Hasil:** musik tetap berjalan pada -12 dB, sedangkan suara zombie berhenti. Tutup Pause. **Hasil:** semuanya berlanjut.
14. Mulai adegan dengan `keepMusic = false`. **Hasil:** musik hilang dalam 1 detik, lalu kembali setelah adegan selesai.
15. Zombie kembali ke pool saat fajar sambil bersuara, lalu dipakai ulang. **Hasil:** tidak ada suara lama, dan filter kembali 22000 Hz.
16. Mati saat `Peak`, lalu mulai run baru. **Hasil:** musik memudar, dan run baru mulai dari `Calm` dengan `Active` kosong.
