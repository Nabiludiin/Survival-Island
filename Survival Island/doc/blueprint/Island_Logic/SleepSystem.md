# Blueprint: SleepSystem.cs
**Lokasi:** `Survival Island/doc/blueprint/Island_Logic/SleepSystem.md`
**Deskripsi Utama:** Tidur di Kasur untuk melewati malam dengan waktu dipercepat, lengkap dengan syarat, risiko, pemulihan, hukuman di pantai, dan pemicu autosave.
**Issue:** #67
**Rujukan GDD:** Bagian 3 (siklus siang dan malam, target 60 hari) dan bagian 4 (fungsi base, dan peringatan Diary Bab 1 agar tidak tidur di pantai). Mekanik tidur, angka, dan rincian hukumannya adalah usulan, karena GDD hanya menyebut peringatan itu.

## 1. Arsitektur & Variabel Utama

### Pola

`SleepSystem` adalah manager di scene `IslandPhase`. Satu state: terjaga atau tidur. Semua efek dipasang saat mulai, dan dilepas oleh satu fungsi, `EndSleep`, supaya tidak ada jalur bangun yang lupa mengembalikan waktu.

| Kelas | Jenis | Tugas |
|---|---|---|
| `SleepSystem` | Manager | Syarat, percepatan waktu, bangun, dan pemulihan |
| `BedInteractable` | `IslandInteractable`, di prefab `Bed_Basic` | Menerima `E`. Tabel `IslandInteractable.md` mencantumkan Kasur di bawah `SleepSystem` |
| `SleepPanel` | `UIPanel`, id `Sleep` | Layar gelap, jam, dan tulisan tombol bangun |

```csharp
public enum SleepBlockReason
{
    None, NotNight, EnemyNearby, DisasterActive, BloodMoon, Freezing, VitalsTooLow, RecentlyWoken
}

public enum SleepWakeReason { None, Daybreak, Manual, Attacked, EnemyNear, Interrupted, Disaster, TideRising, VitalEmpty, BedDestroyed, RunEnded, Freezing }
```

### Kasur sebagai struktur

Mengikuti `raft/BuildMenu.md`:

| Hal | Nilai |
|---|---|
| ID | `Bed_Basic` |
| Slot | `Cell`, `FloorOrGround` |
| Biaya dan HP | 20 `Mat_Wood`, 100 HP |
| NavMesh | `Avoid` |
| Syarat tempat | Semua aturan `ValidatePlacement`, termasuk larangan pasir |
| Interaksi | Tahan 0 detik. `blockedWhileCarrying = true` |

Kasur boleh dibangun di luar base. Syarat base dihitung saat tidur, bukan saat dibangun.

### Variabel

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Instance` | `SleepSystem` | `null` | Singleton per scene |
| `IsSleeping` | `bool` | `false` | Sedang tidur |
| `currentBed` | `BedInteractable` | `null` | Kasur yang dipakai |
| `startGameHour` | `float` | 0 | `TotalGameHours` saat mulai |
| `inBase` | `bool` | `false` | Hasil `IsInsideBase` saat mulai |
| `onBeach` | `bool` | `false` | Hasil `IsNearSand` saat mulai |
| `interruptAtGameHour` | `float` | -1 | Jam gangguan. -1 berarti malam ini aman |
| `lastWokenTime` | `float` | -999 | Dipakai `RecentlyWoken` |
| `enemyBuffer` | `Collider[]` | Ukuran 8 | Buffer pemeriksaan musuh |

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.sleep.timeMultiplierAsleep` | 2160 | Satu malam 12 jam game selesai dalam 20 detik nyata |
| `GameBalanceConfig.sleep.drainRatioAsleep` | 0,5 | Lapar dan haus selama tidur, dibanding terjaga pada jam game yang sama |
| `GameBalanceConfig.sleep.maxGameHoursPerStep` | 0,25 | Batas maju jam dalam satu langkah `DayNightDirector` |
| `GameBalanceConfig.sleep.enemyBlockRadius` | 25 | Musuh dalam radius ini menolak tidur |
| `GameBalanceConfig.sleep.enemyWakeRadius` | 8 | Musuh dalam radius ini membangunkan pemain |
| `GameBalanceConfig.sleep.enemyCheckSeconds` | 0,5 | Jeda pemeriksaan musuh |
| `GameBalanceConfig.sleep.structureAlarmRadius` | 15 | Dinding diserang dalam radius ini membangunkan pemain |
| `GameBalanceConfig.sleep.minVitalToSleep` | 10 | Lapar dan haus minimum untuk tidur |
| `GameBalanceConfig.sleep.retrySeconds` | 5 | Jeda sebelum boleh tidur lagi setelah terbangun paksa |
| `GameBalanceConfig.sleep.hpPerGameHourInBase` | 2 | Pemulihan HP per jam game di dalam base |
| `GameBalanceConfig.sleep.hpPerGameHourOutside` | 0 | Pemulihan HP di luar base |
| `GameBalanceConfig.sleep.minHoursForStamina` | 4 | Jam tidur minimum untuk stamina penuh |
| `GameBalanceConfig.sleep.interruptChanceInBase` | 0,25 | Peluang gangguan per malam di dalam base |
| `GameBalanceConfig.sleep.interruptChanceOutside` | 0,6 | Peluang gangguan di luar base |
| `GameBalanceConfig.sleep.interruptChanceBeach` | 1 | Peluang gangguan di pantai |
| `GameBalanceConfig.sleep.interruptMinProgress` | 0,25 | Gangguan paling cepat setelah bagian malam ini |
| `GameBalanceConfig.sleep.interruptMaxProgress` | 0,9 | Gangguan paling lambat |
| `GameBalanceConfig.sleep.ambushBaseCount` | 4 | Musuh penyergap dasar |
| `GameBalanceConfig.sleep.ambushCountPerTenDays` | 1 | Tambahan per 10 hari |
| `GameBalanceConfig.sleep.ambushRadiusInBase` | 20 | Jarak muncul penyergap saat di dalam base |
| `GameBalanceConfig.sleep.ambushRadiusExposed` | 10 | Jarak muncul di luar base dan di pantai |
| `GameBalanceConfig.sleep.beachRadius` | 8 | Radius pasir yang dihitung tidur di pantai |
| `GameBalanceConfig.sleep.beachTideStaminaLoss` | 30 | Stamina hilang saat tersapu air pasang |

**Hasil satu malam penuh dalam base, dengan `timeMultiplier` 96:**

| Hal | Perhitungan | Hasil |
|---|---|---|
| Kecepatan waktu | 2160 / 96 | 22,5 kali |
| Waktu nyata | 12 jam x 3600 / 2160 | 20 detik |
| Pengali lapar dan haus | 22,5 x 0,5 | 11,25 |
| Lapar berkurang | 0,11 x 11,25 x 20 | Sekitar 25 |
| Haus berkurang | 0,15 x 11,25 x 20 | Sekitar 34 |
| HP pulih | 12 x 2 | 24 |

## 2. Detail Logika & Fungsi

### Syarat tidur

**`public SleepBlockReason GetBlockReason()`** berhenti pada alasan pertama:

| No | Syarat | Alasan | Pesan |
|---|---|---|---|
| 1 | `DayNightDirector.Instance.IsNight` | `NotNight` | `Hanya bisa tidur saat malam` |
| 2 | `Time.time - lastWokenTime >= retrySeconds` | `RecentlyWoken` | `Masih terlalu gelisah` |
| 3 | Tidak ada bencana aktif (#70) | `DisasterActive` | `Tidak aman saat bencana` |
| 4 | Tidak sedang Blood Moon (#50) | `BloodMoon` | `Malam ini terlalu berbahaya` |
| 5 | `!WarmthSystem.IsFreezing` (#65) | `Freezing` | `Terlalu dingin untuk tidur` |
| 6 | `Hunger` dan `Thirst >= minVitalToSleep` | `VitalsTooLow` | `Terlalu lapar atau haus` |
| 7 | Tidak ada musuh aktif dalam `enemyBlockRadius` | `EnemyNearby` | `Ada musuh di dekat sini` |

Pemeriksaan musuh memakai `PhysicsQuery.OverlapSphere` dengan `PhysicsMasks.EnemyBody`. Musuh yang menurut `EnemyAI.IsActiveThreat` sedang `Dormant` atau `Submerged` tidak dihitung, sesuai #63.

**Harus di dalam base?** Tidak wajib. Di luar base pemain tetap bisa tidur, tetapi tanpa pemulihan HP dan dengan peluang gangguan lebih tinggi. Dengan begitu, base tetap berguna tanpa membuat pemain yang belum punya base terkunci dari tidur.

`BedInteractable.CanInteract` mengembalikan `InteractBlockReason.Locked` kalau `GetBlockReason() != None`, dan `GetPrompt()` memakai pesan di tabel, supaya outline merah punya alasan yang jelas.

### Mulai tidur

**`public bool TrySleep(BedInteractable bed)`**:

1. Kalau `GetBlockReason() != None`, tampilkan notifikasi, lalu kembalikan `false`.
2. Panggil `BeginSleep(bed)`.

**`private void BeginSleep(BedInteractable bed)`**:

1. Isi `currentBed`, `startGameHour`, `inBase = BaseNavigation.Instance.IsInsideBase(bed.transform.position)`, dan `onBeach = SurfaceDetection.Instance.IsNearSand(bed.transform.position, beachRadius)`.
2. Panggil `RollInterrupt()`.
3. `DayNightDirector.Instance.SetTimeMultiplierOverride(timeMultiplierAsleep)`.
4. `SurvivalStats.SetDrainMultiplier((timeMultiplierAsleep / timeMultiplier) * drainRatioAsleep)`.
5. `PlayerInteractor.SetEnabled(false)`, lalu turunkan senjata lewat `EquipmentManager.PushLock(EquipLock.Menu)`, dan mainkan animasi kamera berbaring (#102). Pemain tidak dipindahkan, sesuai `RegionStreaming.md`.
6. `UIFocusStack.Instance.Push(sleepPanel)`.
7. `IsSleeping = true`, lalu terbitkan `SleepStateChangedEvent { isSleeping = true, reason = None }`.

### Percepatan waktu tanpa `Time.timeScale`

`Time.timeScale` tidak diubah. Hanya jam dunia yang lebih cepat, jadi fisika, animasi, dan AI tetap normal.

**Perubahan di `DayNightDirector`:**

```csharp
public float BaseTimeMultiplier => GameManager.Instance.Balance.time.timeMultiplier;
float overrideMultiplier = -1f;

public void SetTimeMultiplierOverride(float value) { overrideMultiplier = value; }
public void ClearTimeMultiplierOverride() { overrideMultiplier = -1f; }

void Update()
{
    float multiplier = overrideMultiplier > 0f ? overrideMultiplier : BaseTimeMultiplier;
    float remaining = Time.deltaTime * multiplier / 3600f;
    float step = balance.sleep.maxGameHoursPerStep;

    while (remaining > 0f)
    {
        AdvanceClock(Mathf.Min(remaining, step));   // Pemeriksaan batas ada di dalam fungsi ini
        remaining -= step;
    }
}
```

**`AdvanceClock(float hours)`** memajukan `currentTimeOfDay` dan `TotalGameHours`, lalu memeriksa batas:

| Batas | Pemeriksaan | Jaminan satu kali |
|---|---|---|
| 24:00 | `currentTimeOfDay >= 24` lalu `-= 24`, dan panggil `GameManager.AdvanceDay()` | Jam langsung berkurang 24 |
| 18:00 | `currentTimeOfDay >= 18` dan `!isNight` | `isNight` langsung `true` |
| 06:00 | `6 <= currentTimeOfDay < 18` dan `isNight` | `isNight` langsung `false` |
| Tiap jam | `Mathf.FloorToInt(currentTimeOfDay) != lastHour` | `lastHour` diperbarui |

Karena tiap langkah paling besar 0,25 jam, frame panjang tidak bisa melompati jam atau batas 18:00 dan 06:00. Rumus lama juga tidak punya pelindung untuk `Time.deltaTime` yang besar pada kecepatan tinggi.

**Pengecekan hari ke-60:** `AdvanceDay` berada di dalam `AdvanceClock`, jadi `CheckDay60Milestone` tetap dipanggil tepat satu kali. Tidur melewati 24:00 pada hari ke-60 tidak mengakhiri run dan tidak butuh konfirmasi. `DayLimitPassedEvent` terkirim sekali, dan tidur berlanjut.

### Selama tidur

**`void Update()`**, saat `IsSleeping`:

1. Hitung `hoursSlept = TotalGameHours - startGameHour`, lalu perbarui jam di `SleepPanel`.
2. Kalau `interruptAtGameHour >= 0` dan `TotalGameHours >= interruptAtGameHour`, jalankan `TriggerInterrupt()`.
3. Kalau `Hunger <= 0` atau `Thirst <= 0`, panggil `EndSleep(VitalEmpty)`.
4. Setiap `enemyCheckSeconds`, periksa `PhysicsMasks.EnemyBody` dalam `enemyWakeRadius`. Kalau ada musuh aktif, panggil `EndSleep(EnemyNear)`.

**Status lain selama tidur:**

| Hal | Perilaku |
|---|---|
| Lapar dan haus | Memakai `SetDrainMultiplier` di atas |
| Stamina dan HP | Tetap memakai waktu nyata di `SurvivalStats` (#111). Pemulihan besar diberikan saat bangun |
| Busuk makanan, cuaca, dan bencana | Memakai jam game, jadi ikut cepat |
| Infeksi | Mengikuti aturan waktu #64 |

### Bangun

| Pemicu | Sumber | `SleepWakeReason` |
|---|---|---|
| Fajar | `DaybreakEvent` | `Daybreak` |
| Esc | `SleepPanel.OnClosed(User)` | `Manual` |
| Pemain diserang | `PlayerDamagedEvent` | `Attacked` |
| Musuh mendekat | Pemeriksaan berkala | `EnemyNear` |
| Dinding diserang | `StructureDamagedEvent`, kalau jaraknya dalam `structureAlarmRadius` | `EnemyNear` |
| Gangguan malam | `TriggerInterrupt` | `Interrupted` |
| Bencana mulai | `DisasterStartedEvent` | `Disaster` |
| Air pasang | `TideChangedEvent.isHigh`, kalau `onBeach` | `TideRising` |
| Lapar atau haus nol | `Update` | `VitalEmpty` |
| Kasur hancur | `StructureDestroyedEvent` | `BedDestroyed` |
| Mulai membeku | `WarmthChangedEvent.isFreezing` berubah menjadi `true` (#65), misalnya api unggun padam karena bahan bakarnya ikut jam game yang dipercepat | `Freezing`, dengan notifikasi `Kedinginan, kamu terbangun` |
| Panel ditutup sistem lain | `SleepPanel.OnClosed(Forced)` atau `OnClosed(RunEnded)` | `RunEnded` |

**`private void EndSleep(SleepWakeReason reason)`**, satu-satunya jalur bangun:

1. Kalau `!IsSleeping`, selesai.
2. `IsSleeping = false`.
3. `DayNightDirector.Instance.ClearTimeMultiplierOverride()`, dan `SurvivalStats.SetDrainMultiplier(1)`.
4. `PlayerInteractor.SetEnabled(true)`, dan `EquipmentManager.PopLock(EquipLock.Menu)`.
5. Kalau panel masih terbuka, `UIFocusStack.Instance.Close(sleepPanel, UICloseReason.Forced)`.
6. Panggil `ApplyRest(reason)`, kecuali `RunEnded`.
7. Atur `lastWokenTime = Time.time`, kecuali `Daybreak` dan `Manual`.
8. Terbitkan `SleepStateChangedEvent { isSleeping = false, reason }`.
9. Kalau `reason` adalah `Daybreak` atau `Manual`, terbitkan `SaveRequestedEvent` dengan alasan `WakeUp`. Pemeriksaan pertempuran tetap dilakukan #80.

`EndSleep` dijaga dari panggilan ganda oleh langkah 1, jadi `Close` pada langkah 5 yang memanggil `OnClosed` tidak mengulang proses.

### Pemulihan saat bangun

**`private void ApplyRest(SleepWakeReason reason)`**, dengan `hours = TotalGameHours - startGameHour`:

| Hal | Syarat | Hasil |
|---|---|---|
| HP | `Hunger > 0`, `Thirst > 0`, dan `Infection < regenMaxInfection` | `RestoreHP(hours * (inBase ? hpPerGameHourInBase : hpPerGameHourOutside))` |
| Stamina | `hours >= minHoursForStamina`, `reason` bukan `Attacked`, `Interrupted`, `TideRising`, atau `Freezing` | `RestoreStamina(MaxStamina)` |
| Di pantai | `onBeach` | Tidak ada HP dan stamina |

Infeksi tidak berkurang karena tidur. Obatnya tetap Vaksin, sesuai GDD bagian 3.

### Hukuman tidur di pantai

Kasur tidak bisa dibangun di atas pasir (#79). Karena itu, **tidur di pantai** berarti kasur berada dalam `beachRadius` dari pasir, yang dibaca `SurfaceDetection.IsNearSand`. Prompt kasur menambahkan peringatan `Terlalu dekat pantai` sebelum pemain menekan `E`, sesuai Diary Bab 1.

**Akibatnya:**

1. `RollInterrupt` selalu menghasilkan gangguan, karena `interruptChanceBeach` bernilai 1.
2. Penyergap muncul pada `ambushRadiusExposed`, jauh lebih dekat daripada di dalam base.
3. Kalau air pasang datang (#91), pemain langsung bangun dengan `TideRising`, dan kehilangan `beachTideStaminaLoss` stamina lewat `SpendStamina(beachTideStaminaLoss, StaminaUse.Swim)`.
4. Tidak ada pemulihan HP dan stamina.

Kalau #91 belum tersedia atau ditolak, langkah 3 tidak berlaku, dan sisanya tetap jalan.

### Gangguan malam dan perilaku spawner

Spawner berhenti selama tidur. Tanpa risiko lain, tidur setiap malam akan menghapus semua ancaman zombie. Karena itu, risiko dihitung sekali saat mulai tidur.

**`private void RollInterrupt()`**:

```csharp
ref var rng = ref RunSeedManager.Instance.GetStream(RandomStreamId.Gameplay);
float chance = onBeach ? b.interruptChanceBeach : inBase ? b.interruptChanceInBase : b.interruptChanceOutside;

if (rng.NextFloat01() >= chance) { interruptAtGameHour = -1f; return; }

float progress = rng.Range(b.interruptMinProgress, b.interruptMaxProgress);
interruptAtGameHour = startGameHour + HoursUntilDaybreak() * progress;
```

**`private void TriggerInterrupt()`**:

1. `count = Round((ambushBaseCount + currentDay / 10 * ambushCountPerTenDays) * AIDirector.Instance.GetPostTargetCountMultiplier(currentDay))`, sesuai keputusan developer di `AIDirector.md`.
2. `SpawnManager.Instance.RequestAmbush(posisiPemain, count, inBase ? ambushRadiusInBase : ambushRadiusExposed)`.
3. Panggil `EndSleep(Interrupted)`, dengan notifikasi `Ada suara di luar`.

**Spawner selama tidur:**

| Sistem | Perilaku saat `SleepStateChangedEvent.isSleeping` |
|---|---|
| `SpawnManager` (#62) | Tidak memunculkan musuh baru, kecuali `RequestAmbush` |
| `AIDirector` (#27) | Menahan anggaran dan fase. Tidak menambah tekanan |
| `HordeManager` (#50) | Tidak memulai gelombang baru |
| Musuh yang sudah ada | Tetap berjalan. Mereka bisa membangunkan pemain atau menyerang base |
| Saat bangun | Spawner lanjut dari jam saat itu, dan tidak mengejar kuota yang terlewat |

`DaybreakEvent` tetap membakar semua zombie saat fajar (#63).

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `SleepStateChangedEvent` (`isSleeping`, `reason`, sticky, baru), `SaveRequestedEvent` (alasan `WakeUp`).

**Event yang didengar:** `DaybreakEvent`, `PlayerDamagedEvent`, `StructureDamagedEvent`, `StructureDestroyedEvent`, `DisasterStartedEvent`, `TideChangedEvent`, dan `WarmthChangedEvent`. Jarak `StructureDamagedEvent` dibaca dari field `position` di event itu.

| Sistem | Hubungan |
|---|---|
| `DayNightDirector` | `IsNight`, `TotalGameHours`, `SetTimeMultiplierOverride`, dan `ClearTimeMultiplierOverride` |
| `SurvivalStats` (#111) | `SetDrainMultiplier`, `RestoreHP`, `RestoreStamina`, dan `SpendStamina` |
| `GameManager` | `currentDay` |
| `BuildMenu`, `BuildManager` (#79) | Kasur adalah `BuildableData` |
| `BaseNavigation` (#78) | `IsInsideBase` |
| `SurfaceDetection` (#86) | `IsNearSand` |
| `UIFocusStack` (#83) | `SleepPanel` |
| `SpawnManager` (#62), `AIDirector` (#27), `HordeManager` (#50) | Mendengar `SleepStateChangedEvent`. `RequestAmbush` dipanggil langsung |
| `WarmthSystem` (#65), `DisasterManager` (#70), `TideManager` (#91) | Syarat dan pemicu bangun |
| `WorldStateSave` (#80) | Mendengar `SaveRequestedEvent`, dan menolak autosave selama `IsSleeping` |
| `RunSeedManager` (#89) | Aliran `Gameplay` |
| `FirstPersonCamera` (#102) | Animasi berbaring |
| `DiaryManager`, `ObjectiveManager` (#81) | Teks peringatan Diary Bab 1 |
| `PostProcessController` (#23), `AmbientAudio` (#52) | Boleh mendengar `SleepStateChangedEvent` |

**Batas pekerjaan:** struktur bangunan umum di #79, dan isi data save di #80.

## 4. Kasus Khusus

- **Objek pool:** `SleepSystem` tidak memakai pool. Penyergap berasal dari pool `SpawnManager`, dan reset mengikuti `OnSpawned` di #59 dan #78.
- **Game dijeda:** tidur tidak bisa dijeda lewat Esc, karena Esc membangunkan pemain. `DevConsole` tetap bisa menjeda. `Time.deltaTime` bernilai 0, jadi jam tidak maju. Kehilangan fokus jendela tidak membuka `Pause`, karena stack tidak kosong (#83), jadi tidur tetap berjalan.
- **Permadeath:** `RunEnded` membangunkan pemain lewat `CloseAllUnlocked`, tanpa pemulihan. Override waktu hilang bersama scene. Kasur berada di `StructureRecord`, dan terhapus bersama save run (#82).
- **Save dan load:** status tidur tidak disimpan. Menutup jendela saat tidur memakai alur `SaveAndQuitToMenu`, dengan jam saat itu, dan pemain dimuat dalam keadaan terjaga. `interruptAtGameHour` tidak disimpan. `lastWokenTime` dimulai ulang.
- **Tidur menjelang fajar:** kalau tinggal sedikit jam, `HoursUntilDaybreak` kecil, jadi waktu gangguan tetap berada sebelum fajar.
- **Tidur sebelum 00:00:** `DayChangedEvent` terkirim satu kali saat melewati tengah malam.
- **Frame sangat panjang saat tidur:** loop `AdvanceClock` memecah langkahnya, jadi jam dan event tetap berurutan.
- **`DevConsole` `settime` saat tidur:** jam berubah langsung. Kalau melewati fajar, `DaybreakEvent` membangunkan pemain.
- **Pemain diterkam saat tidur (#110):** `CloseAllUnlocked(Forced)` menutup `SleepPanel`, dan `OnClosed` memanggil `EndSleep`. Pemicu ini diberi alasan `Attacked` kalau `PlayerDamagedEvent` juga datang lebih dulu.
- **Kasur hancur oleh runtuhan:** pemain bangun, dan pemulihan dihitung sesuai jam yang sudah lewat.
- **Region dilepas saat tidur:** tidak terjadi, karena pemain tidak berpindah.
- **Kasur di base yang dindingnya baru hancur:** `inBase` dihitung saat mulai dan tidak berubah. Zombie di dekat dinding membangunkan pemain lebih dulu.
- **Beberapa kasur:** boleh. Tidak ada kasur utama, dan tidak ada titik kembali, karena permadeath mengulang run dari kapal.
- **Membawa Heavy Item:** `blockedWhileCarrying` menolak interaksi.
- **Melewatkan Weekly Beach Supply:** kargo tiba saat jam berjalan, dan pemain menemukannya setelah bangun (#71).

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** tidur mempercepat waktu lewat `timeMultiplier`, tanpa mengubah nilai dasar (`GameBalanceConfig.md`). `SetDrainMultiplier` tersedia (`VitalsRules.md`). Tidur tidak memindahkan pemain (`RegionStreaming.md`). Peringatan tidak tidur di pantai (GDD bagian 4).

**Keputusan teknis di blueprint ini:**

- `DayNightDirector` memakai override, dan nilai dasar di config tidak disentuh.
- Jam dimajukan per langkah kecil, supaya event batas tidak terlewat.
- `EndSleep` menjadi satu jalur bangun.

**Usulan yang disetujui developer sebagai nilai awal (dicatat di #85):**

1. **Syarat tidur:** hanya malam, tanpa musuh aktif dalam 25 meter, tanpa bencana, bukan Blood Moon, tidak membeku, dan lapar serta haus minimal 10.
2. **Harus di dalam base:** tidak wajib. Rekomendasi: pakai, supaya pemain baru tetap bisa tidur, tetapi base jelas lebih aman.
3. **Lama tidur:** satu malam penuh selesai dalam 20 detik nyata.
4. **Lapar dan haus:** berkurang setengah dari laju biasa pada jam game yang sama. Satu malam penuh mengurangi sekitar 25 lapar dan 34 haus.
5. **Pemulihan:** **tidur memulihkan HP dan stamina.** HP 2 per jam game hanya di dalam base, dengan syarat lapar dan haus tidak nol serta infeksi di bawah 50. Stamina penuh kalau tidur minimal 4 jam dan tidak terbangun paksa. Rekomendasi: pakai, karena memberi alasan membangun Kasur.
6. **Tidur di pantai:** berarti kasur dalam 8 meter dari pasir. Selalu ada penyergapan, air pasang membangunkan dan mengurangi 30 stamina, dan tidak ada pemulihan.
7. **Spawner saat tidur:** berhenti, diganti gangguan yang dihitung sekali saat tidur: 25 persen di dalam base, 60 persen di luar, dan 100 persen di pantai.

## 6. Skenario Tes

1. Bangun Kasur di lantai. **Hasil:** biaya berkurang, dan objek muncul. Coba bangun di pasir. **Hasil:** ditolak.
2. Tekan `E` pada Kasur saat siang. **Hasil:** ditolak dengan pesan `Hanya bisa tidur saat malam`.
3. Tidur pukul 18:00 di dalam base tanpa musuh, dengan gangguan dimatikan lewat `DevConsole`. **Hasil:** layar gelap, jam berjalan, dan pemain bangun pukul 06:00 setelah sekitar 20 detik.
4. Buka Profiler saat tidur. **Hasil:** `Time.timeScale` tetap 1.
5. Pantau `eventlog 1` selama tidur malam penuh. **Hasil:** `DayChangedEvent` satu kali, `DaybreakEvent` satu kali, dan `HourChangedEvent` 12 kali.
6. Tidur dengan `Hunger` 50 dan `Thirst` 50. **Hasil:** saat bangun, lapar sekitar 25 dan haus sekitar 16.
7. Tidur dengan `Thirst` 15. **Hasil:** bangun otomatis saat haus 0, dengan alasan `VitalEmpty`.
8. Pukul pemain lewat `DevConsole` saat tidur. **Hasil:** bangun langsung, waktu kembali normal pada frame yang sama, dan stamina tidak penuh.
9. Tekan Esc saat tidur. **Hasil:** bangun, Pause tidak terbuka, dan autosave `WakeUp` diminta.
10. Tidur dengan zombie dalam 25 meter. **Hasil:** ditolak dengan `Ada musuh di dekat sini`.
11. Biarkan zombie berjalan ke arah base saat pemain tidur. **Hasil:** pemain bangun saat zombie memukul dinding dalam 15 meter, atau saat zombie mencapai 8 meter.
12. Paksa gangguan lewat `DevConsole`. **Hasil:** pemain bangun di tengah malam, dan penyergap muncul sekitar 20 meter dari base.
13. Bangun Kasur 5 meter dari pasir. **Hasil:** prompt `Terlalu dekat pantai`, penyergap muncul sekitar 10 meter, dan tidak ada pemulihan.
14. Jalankan `settide high` saat tidur di dekat pantai. **Hasil:** bangun dengan `TideRising`, dan stamina berkurang 30.
15. Selama tidur, hitung spawn di `eventlog 1`. **Hasil:** tidak ada `EnemySpawnedEvent`, kecuali dari gangguan.
16. Jalankan `setday 60`, lalu tidur pukul 20:00. **Hasil:** tidak ada dialog konfirmasi. Saat melewati 24:00, `DayLimitPassedEvent` terkirim sekali, `RunEndedEvent` tidak terkirim, dan pemain bangun normal pada hari ke-61.
17. Turunkan frame lewat `DevConsole` sampai 5 FPS saat tidur. **Hasil:** tidak ada jam yang terlewat, dan event tetap berurutan.
18. Bangun di dalam base setelah tidur semalam dengan HP 50. **Hasil:** HP 74, dan stamina penuh.
19. Tidur saat `Infection` 60. **Hasil:** HP tidak bertambah dari tidur.
20. Mati saat tidur. **Hasil:** alur akhir run berjalan, `SleepPanel` tertutup, dan `timeMultiplier` run baru kembali ke nilai dasar.
21. Tutup jendela saat tidur, lalu lanjutkan. **Hasil:** jam mengikuti waktu penyimpanan, dan pemain dalam keadaan terjaga.
22. `setday 70`, lalu paksa gangguan tidur. **Hasil:** jumlah penyergap 13, yaitu (4 + 7) x 1,2 dibulatkan, dengan HP 1,2 kali dan damage 1,1 kali.
23. Di Region 3, tidur di luar ruangan dekat api unggun berisi 60 detik bahan bakar, tanpa pakaian hangat. **Hasil:** api padam karena jam dipercepat, `Warmth` turun, dan saat `isFreezing` menjadi `true` pemain langsung bangun dengan alasan `Freezing`. Waktu kembali normal, dan stamina tidak dipulihkan.
