# Blueprint: DisasterManager.cs & DisasterData (ScriptableObject)
**Lokasi:** `docs/blueprint/Island_Logic/DisasterManager.md`
**Deskripsi Utama:** Menjalankan bencana terjadwal, yaitu Erupsi, Gempa Bumi, dan Badai Tropis, melalui tahap peringatan, aktif, dan pemulihan.
**Issue:** #70
**Rujukan GDD:** Bagian 5 (Erupsi di Region 1), bagian 6 (bencana acak, gempa merusak base yang dindingnya belum di-upgrade), dan bagian 7 (mesin menyala saat bencana besar). Angka adalah usulan.

## 1. Arsitektur & Variabel Utama

`DisasterManager` adalah manager di scene `IslandPhase` yang memakai `ISaveable`. Hanya satu bencana aktif pada satu waktu. Jadwalnya sudah dihitung dari seed oleh `RunSeedManager.BuildLayout` (#89).

```csharp
public enum DisasterPhase { Idle, Warning, Active, Aftermath }

[Serializable]
public struct DisasterScheduleEntry { public int day; public DisasterType type; public int scale; public float startHour; }

[CreateAssetMenu(menuName = "Survival/Disaster Data")]
public class DisasterData : ScriptableObject
{
    public DisasterType type;
    public float dailyChance;
    public int cooldownDays, minDay, majorMinScale;
    public float scale2Chance;
    public float warningSeconds, activeSeconds, activeSecondsScale2, aftermathSeconds;
    public float startHourMin, startHourMax;
    public AudioClip warningClip, activeLoop;
}
```

### Nilai awal `DisasterData`

| Field | `Eruption` | `Earthquake` | `TropicalStorm` |
|---|---|---|---|
| `dailyChance` | 0,05 | 0,06 | 0,07 |
| `cooldownDays` | 10 | 6 | 6 |
| `minDay` | 5 | 3 | 2 |
| `scale2Chance` | 0,3 | 0,3 | 0,3 |
| `majorMinScale` | 1, selalu besar | 2 | 2 |
| `warningSeconds` | 30 | 5 | 20 |
| `activeSeconds` / `activeSecondsScale2` | 120 / 180 | 25 / 40 | 240 / 300 |
| `aftermathSeconds` | 45 | 15 | 30 |
| `startHourMin` / `startHourMax` | 8 / 14 | 8 / 14 | 8 / 14 |

### Nilai config bencana

| Nilai | Awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.disaster.fireballIntervalSeconds` / `Scale2` | 4 / 2,5 | Jeda Fireball |
| `GameBalanceConfig.disaster.fireballRingMin` / `Max` | 8 m / 35 m | Jarak dari pemain |
| `GameBalanceConfig.disaster.fireballMarkerSeconds` | 2,5 | Waktu penanda sebelum jatuh |
| `GameBalanceConfig.disaster.fireballRadius` | 6 m | Radius ledakan |
| `GameBalanceConfig.disaster.fireballDamageCenter` / `Edge` | 35 / 10 | Damage makhluk |
| `GameBalanceConfig.disaster.fireballStructureDamage` | 60 | Damage bangunan |
| `GameBalanceConfig.disaster.fireballTreeChance` | 0,25 | Peluang pohon terdekat habis |
| `GameBalanceConfig.disaster.eruptionOffscreenHits` | 3 per skala | Hantaman ke base saat pemain di luar Region 1 |
| `GameBalanceConfig.disaster.quakeDamageRatio` / `Scale2` | 0,25 / 0,5 | Bagian HP maksimum yang hilang |
| `GameBalanceConfig.disaster.quakePulses` | 3 | Jumlah guncangan |
| `GameBalanceConfig.disaster.maxStructuresPerFrame` | 16 | Batas proses bangunan |
| `GameBalanceConfig.disaster.stormPulseSeconds` | 30 | Jeda damage angin |
| `GameBalanceConfig.disaster.stormStructureDamage` | 30 | Damage per struktur kayu |
| `GameBalanceConfig.disaster.treeFallSeconds` | 45 | Jeda pohon tumbang |
| `GameBalanceConfig.disaster.treeFallRingMin` / `Max` | 12 m / 30 m | Jarak pohon dari pemain |
| `GameBalanceConfig.disaster.treeFallRadius` | 3 m | Radius kena |
| `GameBalanceConfig.disaster.treeFallDamage` | 25 | Damage makhluk |
| `GameBalanceConfig.disaster.treeFallStructureDamage` | 60 | Damage bangunan |

Dari bagian `runSeed` dipakai `firstMajorDisasterDay` (3) dan `minDaysBetweenMajorDisasters` (4). Dari #50 dipakai interval Blood Moon (10 hari).

### Pool

| Tag | Isi |
|---|---|
| `Hazard_Fireball` | Batu pijar di layer `Projectile`, dengan jalur kinematik |
| `Hazard_ImpactMarker` | Lingkaran penanda titik jatuh |
| `Fx_FireballImpact`, `Fx_TreeFall` | Efek benturan |

## 2. Detail Logika & Fungsi

### Membuat jadwal

**`DisasterScheduler.Build(ref RandomStream s, DisasterData[] data, out DisasterScheduleEntry[] result)`** dipanggil `BuildLayout` lewat `Fork(Disaster, 0)`:

1. Untuk hari 1 sampai 60, lewati hari kelipatan interval Blood Moon.
2. Untuk tiap data, urut `Eruption`, `Earthquake`, lalu `TropicalStorm`: lewati kalau `day < minDay`, atau cooldown belum habis.
3. Ambil `r = s.NextFloat01()`. Kalau `r >= dailyChance`, lanjut.
4. `scale = s.NextFloat01() < scale2Chance ? 2 : 1`. Hitung apakah bencana besar (`scale >= majorMinScale`).
5. Kalau besar, tetapi sebelum `firstMajorDisasterDay` atau terlalu dekat dengan bencana besar terakhir: `Eruption` dibatalkan, sedangkan yang lain turun ke skala 1.
6. Isi `startHour` secara acak di antara `startHourMin` dan `startHourMax`, catat, lalu berhenti untuk hari itu. Maksimal satu bencana per hari.

Semua `s.Next*` selalu dipanggil dalam urutan yang sama, sehingga jadwal tetap satu untuk satu seed.

### Tahap bencana

**`HandleHour(HourChangedEvent e)`**, kalau `phase == Idle`: lewati entri yang harinya sudah terlewat, lalu kalau `day == currentDay` dan `jam >= startHour`, panggil `Begin(entry)`.

| Tahap | Yang terjadi |
|---|---|
| **`Warning`** | Terbitkan `DisasterStartedEvent { type, scale }`, kirim notifikasi (#22), mainkan `warningClip` lewat `AudioManager`. Erupsi juga memulai langit merah |
| **`Active`** | Kalau besar, terbitkan `MajorDisasterStartedEvent { type, durationSeconds }`. Jalankan efek |
| **`Aftermath`** | Efek berhenti, langit kembali, dan ada notifikasi `Periksa kerusakan base`. Terbitkan `DisasterEndedEvent` di akhir tahap |
| **`Idle`** | `SaveRequestedEvent` dengan alasan `DisasterEnded`, lalu `nextIndex++` |

Timer memakai `Time.deltaTime`.

| Bencana | Notifikasi peringatan |
|---|---|
| `Eruption` | `Gunung berapi bergemuruh. Cari perlindungan` |
| `Earthquake` | `Tanah bergetar` |
| `TropicalStorm` | `Angin menguat. Badai mendekat` |

`DisasterStartedEvent` dikirim sejak `Warning`, supaya `SleepSystem` membangunkan pemain sebelum efek dimulai.

### Erupsi

**Mulai:** `EnvironmentDirector.PushEventLayer(Eruption, bobot)`, dan `SpawnManager.PushOverride({ suspendAnimals = true })` karena hewan buas lari dari erupsi.

**Cakupan:**

| Pemain berada di | Fireball | Langit merah | Suara |
|---|---|---|---|
| Region 1 | Ya | Bobot 1 | Penuh |
| Region lain | Tidak | Bobot 0,35 | Gemuruh jauh |

Saat `RegionChangedEvent`, bobot dan Fireball ikut berubah, bahkan di tengah bencana.

**`TickFireballs()`**, setiap `fireballIntervalSeconds`, kalau pemain di Region 1:

1. Ambil titik acak di cincin `fireballRingMin` sampai `fireballRingMax`. Aliran yang dipakai adalah aliran tampilan, karena titik ini tidak perlu sama antar-load.
2. `PhysicsQuery.Raycast` ke bawah dari ketinggian 60 m dengan `PhysicsMasks.Ground`. Karena mask ini memuat `Structure`, titiknya bisa jatuh di atap.
3. Ambil `Hazard_ImpactMarker` dari pool, lalu `Hazard_Fireball` di langit. Fireball bergerak ke titik selama `fireballMarkerSeconds`, tanpa fisika, sehingga tidak bisa menembus sesuatu tanpa terdeteksi.

**`Fireball.Impact()`**:

1. `PhysicsQuery.OverlapSphere(titik, fireballRadius, PhysicsMasks.Explosion, buffer)`, lalu `CollectUniqueDamageables`.
2. Untuk target yang bukan bangunan, periksa `PhysicsQuery.Raycast` ke target dengan `PhysicsMasks.AIVision`. Kalau terhalang, target terlindung. Inilah yang memaksa MC berlindung, sesuai GDD bagian 5.
3. Target yang kena menerima `DamageInfo { type = Explosion, sourceId = "Fireball", amount = Lerp(fireballDamageCenter, fireballDamageEdge, jarak / radius) }`. Bangunan menerima `fireballStructureDamage`.
4. Dengan peluang `fireballTreeChance`, panggil `ForceDeplete(withDrops: false)` pada `ResourceNode` terdekat.
5. Mainkan `Fx_FireballImpact`, `CameraShake` (#24), dan `FireSystem.SpawnFireArea(titik, fire.fireballAreaRadius, fire.fireballAreaSeconds, fire.fireballDamagePerSec, false, true)` (#97).
6. Kembalikan Fireball dan penanda ke pool.

**Di luar Region 1:** saat `Active` dimulai, panggil `RemoteBaseSimulation.QueueDisasterDamage(Volcano, DamageType.Explosion, fireballStructureDamage, hits, onlyWood: false)`, dengan `hits = eruptionOffscreenHits * scale`.

### Gempa

Berlaku di seluruh pulau.

**`QuakePulse()`** dijalankan `quakePulses` kali, pada 20, 50, dan 80 persen durasi `Active`:

1. `CameraShake.Play(Earthquake, scale)` (#24), dan suara gemuruh.
2. `StructureRegistry.Instance.CopyLoadedStructures(buffer)`.
3. Untuk tiap struktur yang `data.survivesEarthquake == false`, antrekan `DamageInfo { type = Structural, sourceId = "Earthquake", amount = maxHP * ratio / quakePulses }`. Ratio memakai `quakeDamageRatio`.
4. Proses antrean paling banyak `maxStructuresPerFrame` per frame.
5. Pada denyut pertama, panggil `RemoteBaseSimulation.QueueDisasterDamage` untuk region yang tidak dimuat.

| Struktur (#47) | Akibat skala 1 | Akibat skala 2 |
|---|---|---|
| `Wall_Wood`, 300 HP | -75 HP | -150 HP |
| `Door_Wood`, 200 HP | -50 HP | -100 HP |
| `Wall_Stone`, `Door_Stone` | Tanpa damage | Tanpa damage |
| Lantai, atap, dan tangga kayu | 25 persen HP maksimum | 50 persen HP maksimum |

Dengan aturan ini, gempa tidak langsung menghancurkan dinding kayu yang penuh. Base yang tidak pernah diperbaiki akan runtuh pada gempa kedua atau ketiga, sehingga pemain perlu cadangan bahan, sesuai GDD bagian 6.

### Badai Tropis

**Mulai:** `RegionalWeather.PushOverride({ regionMask = semua, weather = Storm, snowWeather = Blizzard })`. Ini juga menaikkan air (#91) dan memadamkan api sesuai #44. Dilepas saat `Aftermath`.

**`StormPulse()`**, setiap `stormPulseSeconds`: pilih 1 struktur kayu per skala secara acak, lalu beri `stormStructureDamage` dengan `Structural`. Struktur `Stone` tidak terkena.

**`TreeFall()`**, setiap `treeFallSeconds`, kalau pemain tidak di Region 3:

1. `PhysicsQuery.OverlapSphere(pemain, treeFallRingMax, PhysicsMasks.Ground, buffer)`, lalu cari `ResourceNode` pohon yang masih hidup di luar `treeFallRingMin`.
2. Pasang `Hazard_ImpactMarker` selama 2 detik di depan pohon.
3. `node.ForceDeplete(withDrops: true)`, sehingga kayunya bisa dipungut.
4. Overlap `treeFallRadius` dengan `PhysicsMasks.Explosion`: makhluk menerima `treeFallDamage`, dan bangunan menerima `treeFallStructureDamage`.

### Prioritas

| Kondisi | Aturan |
|---|---|
| Blood Moon | Hari kelipatan interval dilewati saat jadwal dibuat |
| Blood Moon mulai saat bencana masih aktif (mis. lewat tidur atau `DevConsole`) | `HandleBloodMoonStarted`: langsung `EndNow()` tanpa `Aftermath`. Blood Moon menang |
| Malam biasa | Tidak ada larangan. Bencana dimulai sebelum pukul 14:00 dan biasanya selesai sebelum larut. Badai yang masuk malam tetap jalan bersama zombie |
| Dua bencana | Tidak mungkin dari jadwal. `ForceStart` ditolak kalau `phase != Idle` |
| Pemain ingin tidur | `SleepSystem` menolak kalau `IsActive` |

### API

- `bool IsActive`, `DisasterType ActiveType`, dan `DisasterPhase Phase`.
- `bool ForceStart(DisasterType type, int scale)`, dipakai `DevConsole`.

`LuckyMachine` (#72) hanya mendengar `MajorDisasterStartedEvent`, dan mengatur jendela 30 detiknya sendiri. Durasi `Active` bencana besar selalu minimal 30 detik.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `DisasterStartedEvent`, `MajorDisasterStartedEvent`, `DisasterEndedEvent`, dan `SaveRequestedEvent`.

**Event yang didengar:** `HourChangedEvent`, `DayChangedEvent`, `RegionChangedEvent`, dan `BloodMoonStartedEvent`.

| Sistem | Hubungan |
|---|---|
| `RunSeedManager` (#89) | `CurrentLayout.disasterSchedule`, `DisasterScheduler.Build` |
| `RegionalWeather` (#115) | Override badai |
| `EnvironmentDirector` (#92) | Lapisan langit merah |
| `TideManager` (#91) | Naik lewat cuaca `Storm` |
| `LuckyMachine` (#72) | Spin gratis |
| `BuildingIntegrity` (#47), `StructureRegistry` (#79) | `survivesEarthquake`, daftar struktur |
| `RemoteBaseSimulation` (#90) | Damage pada region yang tidak dimuat |
| `ResourceRespawn` (#73) | `ForceDeplete` |
| `SpawnManager` (#62) | `PushOverride`, `PopOverride` |
| `SleepSystem` (#67) | Bangun lewat `DisasterStartedEvent` |
| `NotificationManager` (#22), `CameraShake` (#24), `AudioManager`, `FireSystem` (#97) | Peringatan, getaran, suara, dan api |
| `DevConsole` (#56) | `disaster <tipe> [skala]` |

## 4. Kasus Khusus

- **Pool:** `Hazard_Fireball.OnSpawned` mengosongkan timer, jejak partikel, dan `hasImpacted`. `OnReturned` mengembalikan penandanya. Fireball yang masih terbang saat bencana berakhir tetap jatuh, lalu kembali ke pool.
- **Game dijeda:** semua timer berhenti karena `Time.deltaTime` 0, dan audio dijeda oleh `AudioManager`.
- **Permadeath:** `world_disasters` dihapus bersama save run. Run baru memakai seed dan jadwal baru.
- **Save saat bencana:** disimpan `nextIndex`, `phase`, `phaseTimeLeft`, dan `pulsesDone`. Fireball yang sedang terbang tidak disimpan.
- **Load saat bencana:** `RestoreState` memasang lagi override cuaca, spawn, dan lapisan langit. Damage yang sudah terjadi tidak diulang karena `pulsesDone`. `MajorDisasterStartedEvent` tidak dikirim ulang, karena `LuckyMachine` menyimpan jendelanya sendiri (#72).
- **`setday`:** entri yang terlewat tidak dijalankan.
- **Region 1 dilepas saat erupsi:** Fireball berhenti karena pemain di luar Region 1, dan penanda yang tersisa kembali ke pool lewat `RegionLoadStateChangedEvent`.
- **Pemain mati saat bencana:** `RunEndedEvent` membuat scene dimuat ulang, jadi semua efek hilang bersama scene.
- **Layout cadangan (#89):** kalau `usedFallbackLayout`, jadwal tetap dibuat dari fungsi yang sama.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** `survivesEarthquake` di #47, `DamageType` di #59, dan nama event di #107.

**Usulan yang menunggu developer:**

1. **Cakupan erupsi:** Fireball hanya di Region 1, sedangkan langit merah dan gemuruh terasa di semua region. Rekomendasi: pakai, karena GDD menempatkan erupsi di Region 1 dan pemain di mana pun tetap tahu ada peluang spin gratis.
2. **Prioritas:** Blood Moon menang, dan tidak ada bencana pada hari Blood Moon. Bencana mulai pukul 08:00 sampai 14:00. Rekomendasi: pakai, supaya pemain tidak menerima horde dan bencana sekaligus.
3. **Bencana besar:** erupsi selalu besar. Gempa dan badai hanya pada skala 2.
4. **Save:** bencana yang sedang berjalan ikut disimpan dan dilanjutkan. Rekomendasi: pakai, supaya bencana tidak bisa dihindari dengan keluar game.
5. **Peluang, durasi, dan damage di tabel.** Rekomendasi: pakai sebagai nilai awal, lalu setel lewat playtest.

## 6. Skenario Tes

1. `disaster Eruption 1` di Region 1. **Hasil:** peringatan, langit merah, Fireball dengan penanda, lalu `MajorDisasterStartedEvent` tepat satu kali.
2. Berdiri di bawah atap saat Fireball jatuh dekat. **Hasil:** pemain tidak menerima damage, atap menerima damage.
3. Berdiri di ruang terbuka pada titik penanda. **Hasil:** HP berkurang sekitar 35.
4. `disaster Eruption 1` di Region 3, lalu masuk Region 1. **Hasil:** awalnya tanpa Fireball dan langit merah samar, lalu keduanya penuh.
5. `disaster Earthquake 1` dengan dinding kayu dan batu. **Hasil:** kayu -75 HP, dan batu tetap.
6. `disaster Earthquake 2`. **Hasil:** `MajorDisasterStartedEvent` terkirim, dan layar bergetar tiga kali.
7. `disaster TropicalStorm 1` di Region 1. **Hasil:** `Storm`, air naik, kayu rusak bertahap, dan pohon tumbang dengan penanda.
8. Paksa Blood Moon saat erupsi. **Hasil:** erupsi berakhir, dan override dilepas.
9. Simpan saat `Active`, lalu load. **Hasil:** sisa waktu berlanjut, dan damage gempa tidak terulang.
10. Bandingkan jadwal dua kali dengan seed sama. **Hasil:** sama. Tidak ada bencana pada hari ke-10, 20, 30, dan seterusnya.
11. Tidur saat `Warning` mulai. **Hasil:** pemain bangun dengan `Disaster`.
12. Buka Profiler saat erupsi skala 2. **Hasil:** tidak ada `Instantiate`, dan GC mendekati nol.
