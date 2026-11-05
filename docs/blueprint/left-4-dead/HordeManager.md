# Blueprint: HordeManager.cs
**Lokasi:** `docs/blueprint/left-4-dead/HordeManager.md`
**Deskripsi Utama:** Event Blood Moon terjadwal. Setiap kelipatan 10 hari, langit menjadi merah, zombie mengabaikan Stealth, lalu horde besar menyerbu markas pemain sampai fajar.
**Issue:** #50
**Rujukan GDD:** Bagian 3 (Horde Zombie keluar malam hari untuk memburu MC dan menyerang base). Blood Moon setiap 10 hari: malam merah, serbuan besar ke base, dan zombie mengabaikan Stealth. Isi gelombang, jumlah zombie, dan peringatan adalah usulan.
**Status:** Aturan GDD bagian 3. Sistem aktif (`GameBalanceConfig.bloodMoon.enabled = true`), dan saklarnya hanya untuk tes. #27, #49, #67, dan #108 sudah menyiapkan kaitannya, dan event ini memberi ujian base yang bisa diantisipasi.

## 1. Arsitektur & Variabel Utama

`HordeManager` adalah manager per scene di `IslandPhase` (sesuai `RunLifecycle.md`), dan memakai `ISaveable` dengan `SaveId` `world_bloodmoon`. Tugasnya hanya mengatur jadwal, suasana, dan gelombang. Target milik `ThreatSystem` (#108), deteksi milik `AISensory` (#49), dan kemunculan milik `SpawnManager` (#62).

### Variabel

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `IsActive` | `bool` | `false` | Blood Moon sedang berjalan. Dibaca `DisasterManager` (#70) |
| `IsBloodMoonActive` | `bool` | `false` | Properti `=> IsActive`, dipakai `GetBlockReason` di `SleepSystem` (#67) |
| `activeDay` | `int` | -1 | Hari saat malam ini mulai |
| `lastBloodMoonDay` | `int` | -1 | Mencegah mulai dua kali |
| `wavesSpawned` | `int` | 0 | Gelombang yang sudah keluar |
| `waveTimer` | `float` | 0 | Sisa waktu ke gelombang berikutnya |
| `overrideHandle` | `SpawnOverrideHandle` | Kosong | Dari `SpawnManager.PushOverride` |
| `tablesByRegion` | `RegionSpawnTable[4]` | Dari Inspector | Tabel malam per `RegionType`. Memakai aset yang sama dengan #62 |

### Angka (`GameBalanceConfig.bloodMoon`, bagian baru)

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `enabled` | `true` | Aturan GDD bagian 3. Saklar hanya untuk tes |
| `intervalDays` | 10 | Hari ke-10, 20, 30, sesuai GDD bagian 3 |
| `warningHour` | 12 | Jam peringatan pada hari itu |
| `waveCount` | 4 | Jumlah gelombang utama |
| `firstWaveDelaySeconds` | 20 | Jeda setelah matahari terbenam |
| `waveIntervalSeconds` | 90 | Jeda antar-gelombang |
| `baseWaveSize` | 12 | Ukuran gelombang pada hari ke-10 |
| `waveSizePerEvent` | 4 | Tambahan tiap Blood Moon berikutnya |
| `maxAlive` | 50 | Nilai `SpawnOverride.capOverride` |
| `sustainAlive` | 20 | Jumlah zombie yang dijaga setelah gelombang utama |
| `sustainCheckSeconds` | 30 | Jeda pengecekan `sustainAlive` |
| `burstRadius` | 45 | Radius `RequestBurst`, di atas `AIDirector` |
| `baseEngageDistance` | 120 | Jarak pemain ke base agar serbuan berpusat di base |

**Patokan malam:** 1 malam sekitar 450 detik. Empat gelombang berakhir pada detik 290, sehingga sisa malam diisi `sustainAlive`.

**Ukuran gelombang:** `Min(maxAlive, Round((baseWaveSize + (nomorEvent - 1) * waveSizePerEvent) * AIDirector.Instance.GetPostTargetCountMultiplier(day)))`, dengan `nomorEvent = day / intervalDays`. Hari ke-10 berisi 12, hari ke-60 berisi 32, dan hari ke-70 berisi 43 zombie per gelombang. Ukurannya berhenti di 50, mulai hari ke-80. `sustainAlive` tetap 20, karena batas zombie aktif tidak ikut dikali, sama seperti `maxAlive` di `AIDirector.md`.

## 2. Detail Logika & Fungsi

### Kalkulator jadwal

**`public bool IsBloodMoonDay(int day)`** mengembalikan `enabled && day > 0 && day % intervalDays == 0`.

**`public int NextBloodMoonDay(int fromDay)`** mengembalikan kelipatan berikutnya. Dipakai `DevConsole` dan #81 kalau Diary ingin memberi petunjuk.

Jadwal tetap dan tidak memakai `RunSeedManager`, sesuai `RunSeedManager.md`.

**Peringatan:** saat `HourChangedEvent.hour == warningHour` dan `IsBloodMoonDay(GameManager.currentDay)`, kirim `Bulan merah akan terbit malam ini` lewat `NotificationManager` (#22), dan terbitkan `BloodMoonWarningEvent { day }` (#114), tepat satu kali per hari. Pemain punya sekitar 6 jam game untuk bersiap.

### Mulai

**`private void HandleNightfall(NightfallEvent e)`**:

1. Selesai kalau `!IsBloodMoonDay(e.day)`, `lastBloodMoonDay == e.day`, atau fase bukan `IslandSurvival`.
2. Panggil `StartBloodMoon(e.day)`.

**`private void StartBloodMoon(int day)`**:

1. `IsActive = true`, `activeDay = day`, `lastBloodMoonDay = day`, `wavesSpawned = 0`, `waveTimer = firstWaveDelaySeconds`.
2. `overrideHandle = SpawnManager.Instance.PushOverride(new SpawnOverride { forceNightTable = true, suspendNormalSpawn = true, suspendAnimals = true, capOverride = maxAlive })`.
3. `EnvironmentDirector.PushEventLayer(EnvEventId.BloodMoon, 1f)` (#92).
4. Terbitkan `BloodMoonStartedEvent { day }`.

Pendengar event melakukan bagian masing-masing:

| Pendengar | Perilaku |
|---|---|
| `AISensory` (#49) | `IgnoreStealth = true`. Semua zombie menjadi `Aware`, termasuk yang baru muncul |
| `ThreatSystem` (#108) | Override urutan 3: zombie mengunci `BaseAnchor` |
| `AIDirector` (#27) | `isOverridden = true`. Mob biasa berhenti |
| `RemoteBaseSimulation` (#90) | Menaikkan kekuatan serangan kalau base jauh |

### Override cuaca

Hanya `EnvironmentDirector` (#92) yang menulis fog dan cahaya. Blood Moon adalah lapisan event `EnvEventId.BloodMoon` dengan prioritas tertinggi (30), setelah dasar region, jam, dan cuaca. Nilainya milik tabel lapisan di `EnvironmentProfile.md`:

| Parameter lapisan | Nilai awal | Efek |
|---|---|---|
| Warna fog | `#6B0F12` | Kabut merah pekat |
| Tambahan fog | 0,01 | Jarak pandang menurun, dibatasi `maxFogDensity` |
| Pengali matahari | 0,8, warna merah | Cahaya menjadi merah |
| Warna langit (usulan tambahan) | `#801010` | Langit merah. Lihat perubahan kontrak di bagian 5 |
| Transisi (usulan tambahan) | Masuk 20 detik, keluar 10 detik | Masuk saat matahari terbenam, keluar saat fajar |

`RegionalWeather` (#115) tetap berjalan, supaya jadwal cuacanya tidak bergeser. Hujan dan Blizzard tetap memainkan partikel, sedangkan warna langit dan kabut mengikuti lapisan Blood Moon. Senter dan api tetap berguna, karena `minAmbientLuminance` di `EnvironmentProfile.md` menjaga batas gelap.

### Spawn massal

**`void Update()`**, kalau `IsActive` dan tidak sedang tidur:

1. `waveTimer -= Time.deltaTime`. Kalau `> 0`, selesai.
2. `GetBurstCenter(out center)`.
3. **Gelombang utama**, saat `wavesSpawned < waveCount`:
   - `count = Min(ukuran gelombang, maxAlive - SpawnManager.AliveZombies)`.
   - Kalau `count > 0`, panggil `SpawnManager.Instance.RequestBurst(tablesByRegion[region], count, center, burstRadius, AIDirector.Instance.GetPostTargetModifiers(activeDay))`.
   - `wavesSpawned++`, lalu `waveTimer = waveIntervalSeconds`.
4. **Tahap jaga**, setelah itu:
   - `count = sustainAlive - AliveZombies`. Kalau `> 0`, panggil `RequestBurst` dengan jumlah itu dan `modifiers` yang sama.
   - `waveTimer = sustainCheckSeconds`.

`RequestBurst` memunculkan zombie bertahap sesuai `maxSpawnsPerFrame`, dan tetap di luar pandangan. Budget `AIDirector` tidak terpakai, karena Director sedang berhenti.

**`private void GetBurstCenter(out Vector3 center)`**:

1. Kalau `BaseNavigation.Instance.TryGetBaseAnchor(out anchor)`, dan `RegionStreaming.IsLoaded(GetRegionAt(anchor))`, dan jarak pemain ke `anchor <= baseEngageDistance`: `center = anchor`.
2. Selain itu: `center = posisi pemain`.

### Mengunci markas

- Zombie tidak mencari jalur ke koordinat yang tidak dimuat. Karena itu, `BaseAnchor.IsTargetable` bernilai `true` hanya kalau syarat langkah 1 di atas terpenuhi. Ini menjaga aturan di `ThreatSystem.md`: "tanpa base, zombie memburu pemain".
- Pemain atau NPC yang terlihat dalam `threat.bloodMoonPlayerRange` (8 meter) tetap diserang, sesuai #108.
- Saat terhalang dinding, `EnemyAI` masuk `AttackBuilding` lewat `BaseNavigation.currentBreach` (#78).

### Selesai

**`private void HandleDaybreak(DaybreakEvent e)`**: kalau `IsActive`, panggil `EndBloodMoon()`.

**`private void EndBloodMoon()`**:

1. `SpawnManager.Instance.PopOverride(overrideHandle)`.
2. `EnvironmentDirector.PopEventLayer(EnvEventId.BloodMoon)`.
3. `IsActive = false`.
4. Terbitkan `BloodMoonEndedEvent { day = activeDay }`.

Zombie yang tersisa terbakar saat fajar lewat `SunlightExposure` (#63). `HordeManager` tidak mengembalikan zombie sendiri.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `BloodMoonStartedEvent { day }`, `BloodMoonEndedEvent { day }`.

**Event yang didengar:** `NightfallEvent`, `DaybreakEvent`, `HourChangedEvent`, `SleepStateChangedEvent` (#67), `RunEndedEvent`.

| Sistem | Hubungan |
|---|---|
| `SpawnManager` (#62) | `PushOverride`, `PopOverride`, `RequestBurst`, dan `AliveZombies` |
| `EnvironmentDirector` (#92) | Lapisan suasana. Satu-satunya penulis fog dan cahaya |
| `BaseNavigation` (#78) | `TryGetBaseAnchor` |
| `RegionStreaming` (#88) | `IsLoaded`, `GetRegionAt` |
| `HordeCrowdManager` (#93) | Menanggung beban zombie lewat aturan LOD-nya, tanpa perubahan |
| `SleepSystem` (#67) | Membaca `IsBloodMoonActive`, untuk alasan `BloodMoon` |
| `DisasterManager` (#70) | Membaca `IsActive`, untuk aturan bentrok |
| `CombatMusicDirector` (#114), `AmbientAudio` (#52) | Mendengar event untuk musik dan suara |
| `DevConsole` (#56) | `bloodmoon start`, `bloodmoon stop`, `bloodmoon status` |

**Batas pekerjaan:** perilaku zombie (#25), serangan dinding (#78), dan kerusakan saat pemain jauh (#90).

## 4. Kasus Khusus

- **Pool dipakai ulang:** `HordeManager` tidak menyimpan rujukan zombie. Zombie memakai reset dari `EnemyInstance`. `IgnoreStealth` dibaca dari `AISensory`, sehingga zombie baru langsung `Aware`, sedangkan zombie setelah Blood Moon kembali normal.
- **Game dijeda:** `waveTimer` memakai `Time.deltaTime`, jadi berhenti.
- **Tidur:** `SleepSystem` menolak tidur saat `IsBloodMoonActive`. Pemain yang entah bagaimana tidur (misalnya lewat `DevConsole`) tidak memicu gelombang baru, sesuai `SleepSystem.md`.
- **Base dihancurkan saat malam:** `TryGetBaseAnchor` gagal, sehingga zombie memburu pemain. Gelombang berikutnya berpusat di pemain.
- **Pemain jauh dari base:** horde berpusat pada pemain, dan `RemoteBaseSimulation` (#90) menghitung kerusakan base.
- **Pemain pindah region:** tabel gelombang berikutnya mengikuti `GetRegionAt(center)`. Region tujuan harus sudah dimuat, sesuai #88.
- **Zombie Desa di Region 4:** tabel tetap dipakai. `RequestBurst` mengikuti aturan `hidesFromSun` di `SpawnManager`, sehingga mereka naik dari air atau lumpur kalau titiknya ada.
- **Bentrok dengan bencana:** lapisan Blood Moon punya prioritas lebih tinggi, tetapi aturan mulai bencana milik #70. Kalau erupsi aktif saat Blood Moon, batu pijar tetap jatuh.
- **Hari ke-60:** malam hari ke-60 tetap Blood Moon, dan jadwal berlanjut ke hari ke-70, 80, dan seterusnya. Kalau `RunEndedEvent` terkirim, `HordeManager` langsung membersihkan override tanpa mengirim event.
- **Save saat Blood Moon:** yang disimpan `lastBloodMoonDay`, `IsActive`, `activeDay`, `wavesSpawned`, dan `waveTimer`. Setelah `OnRestoreFinished`, override dan lapisan dipasang lagi, lalu `BloodMoonStartedEvent` dikirim ulang, karena event itu tidak sticky. Zombie tidak disimpan. Mereka muncul lagi lewat gelombang berikutnya.
- **Lompat waktu (`setday`, `settime`):** kalau `settime` melewati fajar, `DaybreakEvent` menutup event. Kalau `NightfallEvent` dikirim berkali-kali, `lastBloodMoonDay` mencegah mulai ganda.
- **Permadeath:** `RunLifecycle` (#82) menghapus save, dan `HordeManager` hilang bersama scene. `OnDisable` melepas event. Override tidak perlu dilepas, karena `SpawnManager` juga dibuat ulang.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** Blood Moon mengabaikan Stealth (#49), target `BaseAnchor` dan urutan override (#108), Director berhenti (#27), tidak bisa tidur (#67), jadwal tidak diacak (#89), dan hanya `EnvironmentDirector` yang menulis fog (#92).

**Keputusan developer:** setelah hari ke-60, Blood Moon memakai pengali jumlah, HP, dan damage yang sama dengan `AIDirector.md`.

**Usulan yang disetujui developer sebagai nilai awal:**

1. **Pemakaian sistem:** kini aturan GDD bagian 3, jadi tidak lagi menjadi usulan.
2. **Satu malam penuh:** dari `NightfallEvent` sampai `DaybreakEvent`, dan zombie tetap terbakar saat fajar, sesuai GDD bagian 3.
3. **Hanya visual pada cuaca:** cuaca tetap berjalan, dan hanya warna langit serta kabut yang ditimpa.
4. **Empat gelombang, lalu jaga 20 zombie:** ukuran 12 pada hari ke-10, 32 pada hari ke-60, lalu naik bersama pengali jumlah sampai batas 50.
5. **Serbuan mengikuti pemain kalau base jauh:** kalau pemain lebih dari 120 meter dari base, horde berpusat pada pemain, dan base dihitung lewat #90. Ini sesuai batas region dimuat di #88.
6. **Hari ke-60 ikut Blood Moon:** menjadi puncak menuju target. Blood Moon berlanjut setelah hari ke-60.
7. **Peringatan jam 12:00:** satu notifikasi pada hari yang sama. Tanpa hitung mundur HUD.

**Perubahan kontrak yang diperlukan:** `BaseAnchor` dan `TryGetBaseAnchor` di `raft/BaseNavigation.md`, warna langit dan waktu transisi lapisan `BloodMoon` di `Island_Logic/EnvironmentProfile.md`, serta baris `world_bloodmoon` di `Core_Logic/WorldStateSave.md`.

## 6. Skenario Tes

`bloodMoon.enabled` sudah `true` sebagai nilai awal.

1. `setday 9`, lalu lewati ke hari berikutnya. **Hasil:** `IsBloodMoonDay(10)` benar, dan notifikasi muncul jam 12:00.
2. `setday 11` sampai malam. **Hasil:** tidak ada Blood Moon, dan `AIDirector` berjalan normal.
3. `setday 10`, lalu tunggu 18:00. **Hasil:** langit dan kabut menjadi merah dalam 20 detik, dan `BloodMoonStartedEvent` terkirim sekali.
4. Jongkok di balik semak saat event mulai. **Hasil:** semua zombie menjadi `Aware`, sesuai ikon `AISensory`.
5. Berada dalam base dengan `BaseAnchor` sah. **Hasil:** gelombang muncul di sekitar base, zombie menuju base, lalu `AttackBuilding` pada dinding.
6. Berdiri 8 meter dari zombie di luar base. **Hasil:** zombie menyerang pemain.
7. Tes tanpa base. **Hasil:** zombie memburu pemain, dan gelombang berpusat pada pemain.
8. Berada 300 meter dari base. **Hasil:** horde berpusat pada pemain, dan jalur zombie tidak menuju base.
9. Catat ukuran gelombang dan jumlah zombie saat puncak di hari ke-60, ke-70, dan ke-120. **Hasil:** gelombang 32, 43, dan 50, zombie aktif tidak melewati 50, zombie hari ke-120 memakai pengali HP dan damage, dan FPS dicatat ke lembar playtest.
10. Tunggu fajar. **Hasil:** warna kembali, `BloodMoonEndedEvent` terkirim, batas spawn kembali normal, dan zombie terbakar.
11. Coba tidur saat Blood Moon. **Hasil:** ditolak dengan pesan `Malam ini terlalu berbahaya`.
12. Simpan saat gelombang kedua, lalu muat. **Hasil:** suasana merah kembali, dan gelombang berlanjut dari `wavesSpawned` tersimpan.
13. Jeda game. **Hasil:** gelombang tidak muncul selama jeda.
14. Mati saat Blood Moon, lalu mulai run baru. **Hasil:** langit normal, dan event tidak aktif.
15. Atur `balance bloodMoon.enabled 0`, lalu `setday 10`. **Hasil:** malam berjalan biasa. Saklar ini hanya untuk tes.
