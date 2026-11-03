# Blueprint: RemoteBaseSimulation.cs
**Lokasi:** `docs/blueprint/Island_Logic/RemoteBaseSimulation.md`
**Deskripsi Utama:** Menghitung nasib base yang ditinggal jauh saat malam, dari data dan tanpa zombie sungguhan, lalu menerapkan kerusakan, jebakan terpakai, dan cedera NPC.
**Issue:** #90
**Rujukan GDD:** Bagian 3 (horde menyerang base saat malam), bagian 4 (base melindungi NPC), bagian 5 (bolak-balik Region 4 ke Region 3), dan bagian 6 (bencana). Aturan simulasi, rumus, dan angka adalah usulan.

## 1. Arsitektur & Variabel Utama

`RemoteBaseSimulation` adalah singleton di `IslandPhase`, dan `ISaveable` dengan `SaveId = world_remotebase`. Ia hanya memakai data. Musuh tidak dibuat, dan NavMesh tidak dipakai (#78).

```csharp
public class BaseCluster            // Dihitung ulang, tidak disimpan
{
    public string clusterId;        // structureId terkecil di dalamnya
    public Vector3 center;
    public RegionType region;
    public List<StructureNode> nodes;
    public bool isRemote;
}

[Serializable]
public class RaidPlan               // Disimpan
{
    public string clusterId;
    public int day;
    public float netDamage;         // Hasil rumus
    public float appliedFraction;   // 0 sampai 1, bagian yang sudah diterapkan
    public float startHour, endHour;
    public bool notified;
    public bool isTakenOver;        // Sudah beralih ke serangan nyata
    public int structuresLost, trapsWorn, npcInjured, npcDead;
}
```

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Instance` | `RemoteBaseSimulation` | `null` | Singleton |
| `clusters` | `List<BaseCluster>` | Kosong | Dibangun ulang tiap `NightfallEvent` dan saat struktur berubah |
| `plans` | `List<RaidPlan>` | Kosong | Serangan malam ini |
| `pendingReports` | `List<RaidPlan>` | Kosong | Laporan yang belum tampil |

### Base dan pos sementara

Pemain boleh punya lebih dari satu base. Struktur yang berjarak kurang dari `clusterRadius` bergabung dalam satu cluster. Cluster dengan struktur kurang dari `minStructuresForRaid` dianggap pos sementara, jadi tidak diserang simulasi. Paling banyak `maxRaidedClusters` cluster terbesar diserang per malam.

### Angka penyetelan

Semua bernama `GameBalanceConfig.remoteBase.<nama>`.

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `remoteDistance` | 80 | Jarak pemain ke `center`. Di atas ini, base dianggap ditinggal |
| `returnDistance` | 60 | Di bawah ini, base kembali nyata. Selisihnya mencegah peralihan bolak-balik |
| `distanceCheckSeconds` | 2 | Jeda pemeriksaan jarak |
| `clusterRadius` | 30 | Jarak penggabungan struktur |
| `minStructuresForRaid` | 4 | Batas pos sementara |
| `maxRaidedClusters` | 2 | Batas cluster per malam |
| `firstRaidDay` | 3 | Hari pertama simulasi |
| `raidChanceBase` | 0,5 | Peluang serangan per malam |
| `raidChancePerDay` | 0,01 | Tambahan peluang per hari |
| `raidChanceMax` | 0,9 | Batas peluang |
| `basePower` | 150 | Kekuatan hari ke-1 |
| `powerPerDay` | 20 | Tambahan per hari |
| `maxPower` | 2550 | Batas kekuatan sebelum pengali ukuran dan Blood Moon, setara hari ke-121 (usulan) |
| `bloodMoonMultiplier` | 2 | Pengali saat Blood Moon (#50) |
| `referenceStructures` | 12 | Ukuran base acuan |
| `sizeFactorMin` / `sizeFactorMax` | 0,75 / 1,5 | Batas pengali ukuran |
| `varianceFraction` | 0,15 | Keacakan hasil, plus minus |
| `scoreWallWood` / `scoreWallStone` | 10 / 25 | Nilai pertahanan per dinding atau pintu |
| `scoreTrap` | 15 | Nilai per jebakan yang hidup |
| `scoreNpcGuard` | 20 | Nilai per NPC penjaga |
| `mitigationK` | 200 | Tetapan kurva pengurangan |
| `maxMitigation` | 0,8 | Batas pengurangan |
| `hitChunk` | 60 | Damage per pukulan simulasi |
| `doorWeight` | 2 | Bobot pintu sebagai sasaran |
| `trapWearFraction` | 0,3 | Bagian `maxHP` jebakan yang hilang per serangan |
| `raidStartHourMin` / `raidStartHourMax` | 20 / 23 | Jam mulai serangan |
| `raidEndHour` | 5 | Jam selesai |
| `npcInjuryChanceMax` | 0,6 | Batas peluang NPC cedera |
| `npcInjuryDamage` | 40 | Damage cedera |
| `npcDeathChanceOnBreach` | 0,35 | Peluang cedera berubah mematikan saat dinding jebol |
| `npcGuardDeathMultiplier` | 0,5 | Penjaga bersenjata lebih selamat |
| `npcGuardMinHpRatio` | 0,25 | NPC di bawah batas ini tidak dihitung penjaga |
| `takeoverZombiesPerPower` | 0,02 | Zombie nyata per sisa kekuatan saat pemain kembali |
| `takeoverMaxZombies` | 12 | Batas zombie nyata |

## 2. Detail Logika & Fungsi

### Aturan: tiga keadaan

| Keadaan | Syarat | Yang terjadi |
|---|---|---|
| **Nyata** | Pemain dalam `returnDistance`, dan region `IsLoaded` | Horde biasa menyerang lewat #78. Sistem ini diam |
| **Simulasi** | Malam, dan pemain di atas `remoteDistance`, atau region tidak dimuat | Sistem ini menghitung |
| **Aman** | Siang, atau pos sementara | Tidak ada serangan, sesuai GDD bagian 3 |

### Rumus serangan

Dihitung sekali saat `NightfallEvent`, sehingga save dan load tidak mengubah hasil.

```
peluang   = min(raidChanceMax, raidChanceBase + raidChancePerDay × hari)
kekuatan  = min(maxPower, basePower + powerPerDay × (hari - 1))
            × clamp(jumlahStruktur / referenceStructures, sizeFactorMin, sizeFactorMax)
            × (Blood Moon ? bloodMoonMultiplier : 1)
pertahanan = Σ dinding atau pintu (score tier × rasio HP)
           + scoreTrap × jebakan hidup
           + scoreNpcGuard × NPC penjaga
mitigasi  = min(maxMitigation, pertahanan / (pertahanan + mitigationK))
netDamage = kekuatan × (1 - mitigasi) × acak(1 - varianceFraction, 1 + varianceFraction)
```

- Dinding dan pintu adalah `BuildableData` dengan `tag` `BaseWall` atau `BaseDoor`. `tier` berasal dari #47.
- Jebakan yang dihitung: `TrapKind.Spike` dan `BarbedWire`. Api unggun tidak dihitung, karena bahan bakarnya berubah terus.
- Semua lemparan memakai `RunSeedManager.GetStream(Enemy)`.

| Contoh | Kekuatan | Pertahanan | Mitigasi | `netDamage` |
|---|---|---|---|---|
| Hari 10, 7 struktur, 4 dinding kayu dan 1 pintu | 330 × 0,75 = 248 | 50 | 0,2 | Sekitar 198, satu dinding rusak tanpa hancur |
| Hari 40, 7 struktur sama | 930 × 0,75 = 698 | 50 | 0,2 | Sekitar 558, 1 sampai 2 dinding hancur |
| Hari 40, 18 struktur, 12 dinding batu, 5 jebakan, 1 NPC | 930 × 1,5 = 1.395 | 395 | 0,66 | Sekitar 474, terbagi ke banyak dinding |

### Alur satu malam

**`private void HandleNightfall(NightfallEvent e)`**:

1. `RebuildClusters()`, memakai daftar `StructureRegistry`.
2. Kalau `e.day < firstRaidDay`, selesai.
3. Untuk cluster terbesar sampai `maxRaidedClusters`, lewati pos sementara, lalu lempar `peluang`.
4. Kalau lolos, buat `RaidPlan` dengan `startHour` acak. Semua cluster mendapat rencana, termasuk yang saat ini nyata. Rencana cluster nyata tidak berjalan selama pemain tetap dekat.

**`private void HandleHour(HourChangedEvent e)`**, untuk tiap rencana cluster yang `isRemote`:

1. Kalau jam melewati `startHour` dan `!notified`: `notified = true`, terbitkan `BaseRaidStartedEvent { clusterId, region }`, lalu tampilkan notifikasi `Base di <region> sedang diserang!`.
2. `ApplyProgress(plan, Fraction(e.hour))`, dengan `Fraction` naik dari 0 di `startHour` sampai 1 di `raidEndHour`. Perhitungan jam melewati tengah malam.

**`private void ApplyProgress(RaidPlan plan, float target)`**: `amount = netDamage × (target - appliedFraction)`, lalu `ApplyStructureDamage(cluster, amount)`, dan `appliedFraction = target`. Karena hanya selisih yang diterapkan, damage tidak pernah ganda.

**`private void ApplyStructureDamage(BaseCluster c, float amount)`**:

1. Bagi menjadi pukulan sebesar `hitChunk`, dan sisa pecahan dibawa ke panggilan berikutnya.
2. Pilih sasaran berbobot: pintu `doorWeight`, dinding 1. Jebakan dan struktur dalam baru menjadi sasaran setelah semua dinding dan pintu hancur.
3. `BaseHealth.TakeDamage(in DamageInfo { amount = hitChunk, type = Structural, byPlayer = false })`. Fungsi ini bisa dipanggil pada objek nonaktif (#59, #88). `StructureDestroyedEvent` dan runtuhan mengikuti jalur #79.

**`private void HandleDaybreak(DaybreakEvent e)`**, untuk tiap rencana yang `!isTakenOver`:

1. `ApplyProgress(plan, 1)` kalau cluster masih `isRemote`. Rencana yang belum sempat berjalan pada cluster nyata dibuang.
2. **Jebakan:** tiap jebakan hidup kehilangan `trapWearFraction × maxHP`, lewat `TakeDamage`. Jumlahnya masuk `trapsWorn`.
3. **NPC:** `ResolveNpcs(plan)`.
4. Terbitkan `BaseRaidResolvedEvent { damageTotal, npcInjured }`, lalu tampilkan ringkasan, misalnya `Base diserang: 2 dinding hancur, 1 NPC terluka`.
5. Hapus rencana. Kalau pemain sedang tidur, ringkasan masuk `pendingReports`, dan tampil saat `SleepStateChangedEvent` selesai.

### NPC penjaga

NPC penjaga adalah NPC `StayAtBase` atau `Work` yang posisi tercatatnya berada di cluster, dengan HP di atas `npcGuardMinHpRatio`. NPC `Follow` ikut pemain, sehingga tidak terdampak.

**`private void ResolveNpcs(RaidPlan p)`**, per NPC:

1. `tekanan = netDamage / total maxHP dinding dan pintu awal`. `isBreached` bernilai benar kalau ada dinding atau pintu yang hancur malam ini.
2. Peluang cedera = `min(npcInjuryChanceMax, tekanan)`. Kalau tidak ada yang jebol, peluang ini dibagi 2.
3. Kalau cedera: `NPCRoster.ApplyRemoteDamage(npcId, npcInjuryDamage)`.
4. Kalau `isBreached`, lempar `npcDeathChanceOnBreach`, dikali `npcGuardDeathMultiplier` untuk NPC bersenjata. Kalau kena, atau HP habis, `NPCRoster.KillNpc(npcId, DeathReason.KilledByEnemy)`, yang menerbitkan `NPCDiedEvent`.

Peluang selamat tanpa cedera pada hari ke-40 dengan base kayu 5 struktur: 40 persen. NPC di balik base batu terawat hampir selalu aman, sehingga upgrade dinding punya nilai jelas.

### Peralihan saat pemain kembali

**`IEnumerator DistanceLoop()`**, tiap `distanceCheckSeconds` saat malam:

- **Simulasi ke nyata:** pemain di bawah `returnDistance` dan `RegionStreaming.IsLoaded(region)`.
  1. `ApplyProgress(plan, Fraction(jam sekarang))`.
  2. Kalau `startHour` sudah lewat dan `appliedFraction < 1`, `isTakenOver = true`, lalu `SpawnManager.RequestWave` dengan `count = min(takeoverMaxZombies, sisa × takeoverZombiesPerPower)`, `reason = "RemoteTakeover"`, dan `origin = center`. Pemain menemukan zombie sedang membobol dinding.
  3. Kalau belum mulai, rencana dibatalkan. Horde biasa sudah mengikuti pemain.
- **Nyata ke simulasi:** pemain di atas `remoteDistance`.
  1. `SpawnManager.ReturnRaidersNear(center, clusterRadius)` mengembalikan zombie di sekitar base ke pool, dengan alasan `TooFar`.
  2. Kalau ada rencana yang belum dibuang, `appliedFraction` diisi `Fraction(jam sekarang)` tanpa damage, lalu sisa malam disimulasikan.

Dengan cara ini, zombie tidak membeku di dekat base akibat `OptimizationManager`, sebab sebelum jarak `remoteDistance` zombie `AttackBuilding` tetap berjalan.

### Bencana saat ditinggal

Base yang ditinggal tetap terkena bencana. `DisasterManager` (#70) memutuskan besarnya, dan sistem ini hanya menerapkannya, supaya tidak ganda.

**`public void QueueDisasterDamage(RegionType region, DamageType type, float damagePerHit, int hits, bool onlyWood)`**, dipanggil #70 untuk region yang tidak dimuat:

1. Ambil semua cluster di `region`, termasuk pos sementara. Keluar kalau tidak ada.
2. Untuk tiap cluster, kumpulkan sasaran dari `StructureRegistry`. Kalau `onlyWood`, buang struktur dengan `data.survivesEarthquake == true` (`raft/BuildingIntegrity.md`).
3. Pilih `hits` sasaran acak memakai `RunSeedManager.GetStream(Disaster)`, lalu panggil `BaseHealth.TakeDamage(in DamageInfo { amount = damagePerHit, type, byPlayer = false })`.

Hantaman langsung diterapkan pada panggilan itu, dan tidak masuk `RaidPlan`, sehingga tidak perlu disimpan. Karena `DisasterManager` memanggilnya hanya saat bencana dimulai atau pada denyut pertama, jumlahnya kecil dan tidak perlu dibagi per frame.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `BaseRaidStartedEvent` dan `BaseRaidResolvedEvent`.

**Event yang didengar:** `NightfallEvent`, `DaybreakEvent`, `HourChangedEvent`, `StructureBuiltEvent`, `StructureDestroyedEvent`, `BloodMoonStartedEvent`, `BloodMoonEndedEvent`, `SleepStateChangedEvent`, dan `RunEndedEvent`.

| Sistem | Hubungan |
|---|---|
| `StructureRegistry`, `BuildableData` (#79) | Daftar struktur dan data |
| `BaseHealth`, `DamagePipeline` (#59) | Jalur damage |
| `BuildingIntegrity` (#47), `BaseDefense` (#26) | `tier` dan jebakan |
| `NPCRoster` (#76) | Cedera NPC |
| `SpawnManager` (#62), `AIDirector` (#27), `OptimizationManager` | Peralihan dan pengecualian culling |
| `RegionStreaming` (#88) | `IsLoaded` dan `GetRegionAt` |
| `DisasterManager` (#70) | Memanggil `QueueDisasterDamage` untuk region yang tidak dimuat |
| `NotificationManager` (#22) | Pesan |
| `RunSeedManager` (#89), `WorldStateSave` (#80) | Keacakan dan simpanan |

**Batas pekerjaan:** serangan saat pemain di base di #78, perbaikan dinding di #47, dan jebakan di #26.

## 4. Kasus Khusus

- **Pool:** sistem ini tidak memakai objek pool sendiri. Zombie peralihan dari `SpawnManager` diatur ulang di `OnSpawned`, dan kembali lewat `ObjectPooler`.
- **Game dijeda:** `HourChangedEvent` berhenti, dan `DistanceLoop` memakai waktu terskala. Simulasi ikut berhenti.
- **Tidur:** waktu cepat hanya mempercepat jam, dan selisih tetap dihitung dari jam. Kalau pemain tidur di base kedua, base pertama tetap disimulasikan.
- **Saat disimpan:** `plans` dan `pendingReports` masuk `world_remotebase` sebagai array. Kerusakan yang sudah diterapkan ada di `StructureRecord.currentHP`, jadi tidak diulang.
- **Load:** `OnRestoreFinished` membangun cluster dari `clusterId`, lalu menerapkan sisa yang tertinggal. Kalau load terjadi saat siang dan rencana belum selesai, `HandleDaybreak` dijalankan sekali untuk rencana itu. Cluster yang hilang membuang rencananya.
- **Permadeath:** autosave mati pada `RunEndedEvent`, rencana dikosongkan, dan semua data terhapus bersama save run (#82).
- **Semua dinding hancur sebelum pagi:** sisa damage mengenai struktur dalam. Kalau semua habis, sisa dibuang, dan cluster hilang.
- **Pemain membangun saat simulasi berjalan di cluster jauh:** tidak mungkin, karena pembangunan butuh pemain dekat, sehingga cluster sudah nyata.
- **Jebakan di region tidak dimuat:** `BaseTrap` tidak berjalan. Hanya `TakeDamage` yang dipanggil.
- **Isi peti hancur:** tumpah ke `RegionStateStore` sesuai `raft/StorageChest.md`.
- **Hari ke-60:** bukan akhir run, jadi simulasi tetap berjalan dan kekuatan terus naik sampai `maxPower`. Kalau run berakhir, `RunEndedEvent` membatalkan semua rencana.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** bekerja dari data tanpa memuat region (`RegionStreaming.md`), dan damage `Structural` ke objek nonaktif (`DamagePipeline.md`).

**Usulan yang disetujui developer sebagai nilai awal (dicatat di #85):**

1. **Aturan dasar:** rekomendasi diserang secara simulasi. Base aman membuat pertahanan sia-sia saat ditinggal, dan serangan penuh terlalu berat untuk performa.
2. **Rumus dan angka:** rekomendasi seperti bagian 2, dengan serangan pertama hari ke-3.
3. **Peluang NPC mati:** rekomendasi hanya saat dinding jebol.
4. **Banyak base:** rekomendasi boleh, dengan batas `maxRaidedClusters` 2, dan pos sementara di bawah 4 struktur tidak diserang.
5. **Bencana saat ditinggal:** rekomendasi tetap kena, dihitung #70.
6. **Notifikasi tanpa radio:** rekomendasi pemain selalu tahu, walaupun tidak ada alasan dalam cerita. Ini lebih adil untuk game permadeath.
7. **Kekuatan setelah hari ke-60:** rekomendasi tetap naik 20 per hari, sampai `maxPower` 2550, supaya base masih bisa bertahan pada run yang sangat panjang.

## 6. Skenario Tes

Gunakan `DevConsole` untuk mengatur hari dan teleport.

1. Malam hari ke-2, pemain jauh. **Hasil:** tidak ada rencana.
2. Hari ke-10, atur peluang 1, pemain di Region 3, base di Region 1. **Hasil:** notifikasi serangan muncul setelah `startHour`, dan HP dinding turun bertahap.
3. Tunggu sampai pagi. **Hasil:** total damage sesuai `netDamage`, `BaseRaidResolvedEvent` terkirim, ringkasan tampil, dan jebakan kehilangan 30 persen HP.
4. Bandingkan base kayu dan batu pada hari dan jarak yang sama. **Hasil:** base batu menerima damage lebih kecil.
5. Biarkan dinding jebol dengan 2 NPC `StayAtBase`. **Hasil:** NPC bisa cedera atau mati, dan skill-nya hilang lewat `NPCDiedEvent`.
6. Kembali ke base tengah malam. **Hasil:** HP sesuai kerusakan sampai jam itu, zombie muncul sekitar base, dan pagi tidak menambah damage dobel.
7. Saat diserang nyata, lari di atas 80 m. **Hasil:** zombie sekitar base kembali ke pool, lalu simulasi berlanjut.
8. Bolak-balik di jarak 60 sampai 80 m. **Hasil:** tidak ada peralihan berulang.
9. Bangun pos 3 struktur. **Hasil:** tidak diserang simulasi.
10. Simpan saat rencana berjalan, keluar, lalu lanjutkan. **Hasil:** `netDamage` sama, dan tidak ada damage berulang.
11. Tidur di base kedua. **Hasil:** base pertama mendapat hasil, dan ringkasan tampil saat bangun.
12. Picu gempa saat base ditinggal. **Hasil:** dinding `survivesEarthquake` tidak rusak.
13. Jeda game saat serangan. **Hasil:** HP tidak berubah.
14. Mati saat rencana berjalan, lalu mulai run baru. **Hasil:** tidak ada rencana tersisa.
15. `setday 200`, lalu tinggalkan base saat malam. **Hasil:** serangan tetap dihitung, dan kekuatan dasar berhenti di 2550.
