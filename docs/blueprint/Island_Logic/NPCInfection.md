# Blueprint: NPCInfection.cs
**Lokasi:** `docs/blueprint/Island_Logic/NPCInfection.md`
**Deskripsi Utama:** NPC bisa terinfeksi, diobati memakai stok vaksin pemain, dikarantina, diusir, atau berubah menjadi zombie.
**Status:** Usulan, belum ada di GDD. Sistem ini nonaktif sampai developer menyetujui. **Rekomendasi:** sebaiknya dipakai setelah #76 stabil, karena menambah pilihan sulit soal vaksin yang langka tanpa menyentuh infeksi pemain.
**Issue:** #117
**Rujukan GDD:** Usulan, belum ada di GDD. Terkait bagian 2 (wabah menyebar lewat gigitan), bagian 3 (stok vaksin sangat sedikit), dan bagian 4 (base melindungi NPC).

## 1. Arsitektur & Variabel Utama

`NPCInfection` adalah satu manager di `IslandPhase`, bukan komponen per NPC. Sebab objek NPC bisa mati saat regionnya dilepas (#88), sedangkan infeksinya harus tetap berjalan. Ia menjadi `ISaveable` dengan `SaveId = world_npc_infection`. Komponen `NPCInfectionVisual` di prefab NPC hanya menampilkan tanda.

```csharp
[Serializable]
public class NPCInfectionRecord
{
    public string npcId;
    public float value;           // 0 sampai 100
    public int stage;             // 1 sampai 4
    public bool isProgressing;    // Sudah pernah digigit, dan belum sembuh
    public bool isQuarantined;
    public Vector3 quarantinePoint;
    public bool pendingTurn;      // Mencapai 100 saat siang, berubah saat malam
}
```

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Instance` | `NPCInfection` | `null` | Singleton |
| `records` | `List<NPCInfectionRecord>` | Kosong | Satu data per NPC yang pernah digigit |
| `IsEnabled` | `bool` | Dari config | Semua fungsi publik keluar lebih awal kalau `false` |

### Angka penyetelan

Tahap, besar gigitan, dan vaksin memakai angka #64, supaya aturannya sama dengan pemain.

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.infection.stage2At`, `stage3At`, `stage4At` | 25, 50, 75 | Batas tahap, sama dengan pemain |
| `GameBalanceConfig.infection.vaccineReduction` | 50 | Infeksi turun per dosis |
| `GameBalanceConfig.infection.vaccineInjectSeconds` | 4 | Lama suntik |
| `GameBalanceConfig.infection.medicVaccineMultiplier` | 1,25 | Lewat `GetSkillMultiplier(Medic)` |
| `GameBalanceConfig.npcInfection.enabled` | `false` | Saklar utama |
| `GameBalanceConfig.npcInfection.gainMultiplier` | 1 | Pengali `DamageInfo.infection` |
| `GameBalanceConfig.npcInfection.selfProgressPerGameHour` | 1,5 | Kenaikan sendiri setelah digigit |
| `GameBalanceConfig.npcInfection.cureBelow` | 10 | Di bawah ini, `isProgressing` berhenti |
| `GameBalanceConfig.npcInfection.stage3SkillFactor` | 0,5 | Kekuatan skill di tahap 3 |
| `GameBalanceConfig.npcInfection.stage3SpeedMultiplier` | 0,85 | Kecepatan di tahap 3 |
| `GameBalanceConfig.npcInfection.stage4SpeedMultiplier` | 0,6 | Kecepatan di tahap 4 |
| `GameBalanceConfig.npcInfection.quarantineMinDistance` | 15 | Jarak minimum titik karantina dari struktur |
| `GameBalanceConfig.npcInfection.turnSeconds` | 3 | Lama animasi berubah |
| `GameBalanceConfig.npcInfection.turnNoiseRadius` | 25 | Radius suara saat berubah |
| `GameBalanceConfig.npcInfection.remoteTurnDamage` | 120 | Damage base kalau berubah saat ditinggal |
| `GameBalanceConfig.npcInfection.remoteBiteInfection` | 15 | Infeksi saat NPC cedera di #90 |

Dengan gigitan zombie standar 5 dan kenaikan 1,5 per jam, NPC yang digigit sekali mencapai 100 dalam sekitar 63 jam game, atau sekitar 2,6 hari. Satu dosis memberi tambahan sekitar 33 jam, jadi NPC perlu vaksin berulang atau butuh keputusan lain.

**Berbeda dari pemain:** infeksi pemain tidak naik sendiri (`selfProgressPerGameHour` 0 di #64). NPC jarang kena serangan, sehingga tanpa kenaikan sendiri sistem ini hampir tidak pernah terasa.

## 2. Detail Logika & Fungsi

### Kenaikan

**`public void AddFromDamage(string npcId, in DamageInfo info)`**, dipanggil `NPCCompanion` setelah menerima damage:

1. Keluar kalau `!IsEnabled` atau `info.infection <= 0`. Hewan bernilai 0, jadi tidak menginfeksi, sama dengan #64.
2. Ambil atau buat record. `value += info.infection × gainMultiplier`, `isProgressing = true`.
3. `UpdateStage(record)`.

**`public void AddRemote(string npcId, float amount)`**, dipanggil `RemoteBaseSimulation` saat NPC cedera, dengan `remoteBiteInfection`.

**`private void HandleHour(HourChangedEvent e)`**, satu kali per jam game, juga saat NPC nonaktif atau pemain tidur. Untuk tiap record yang `isProgressing`: tambah `selfProgressPerGameHour`, `UpdateStage`, lalu kalau `value >= 100`, panggil `TryTurn(record)`.

**`private void UpdateStage(NPCInfectionRecord r)`** memakai batas #64. Kalau tahap berubah, terbitkan `NPCInfectionChangedEvent { npcId, value, stage }`.

### Tanda per tahap

| Tahap | Ucapan (`NPCBarks`, #100) | Gerakan | Indikator | Skill |
|---|---|---|---|---|
| 1 | Normal | Normal | Tidak ada | Penuh |
| 2 | `Cuma luka kecil...`, batuk sesekali | Sesekali memegang luka | Ikon kecil di menu perintah NPC | Penuh |
| 3 | Mengeluh demam, meminta obat | Layer `Sick` di `Animator`, kecepatan x0,85 | Ikon infeksi di atas kepala saat dekat | x0,5 |
| 4 | Mengigau dan meminta pemain menjauh | Terhuyung, kecepatan x0,6, menolak `Work` | Ikon merah berkedip, dan notifikasi | Mati |

Kalau NPC medis hidup dan tidak sakit, menu perintah menampilkan angka infeksi, bukan hanya ikon (#76).

### Vaksin

Pilihan `Beri vaksin` muncul di menu NPC (#17, #76) mulai tahap 2.

**`public bool TryVaccinate(string npcId)`**:

1. Biaya: 1 `Med_Vaccine` dari stok pemain. Kalau `!InventoryManager.HasItem("Med_Vaccine", 1)`, tampilkan `Tidak ada vaksin`, lalu kembalikan `false`. Satu dosis yang sama juga dipakai pemain, sehingga pilihan sulitnya nyata.
2. Mulai aksi tahan `vaccineInjectSeconds`, memakai UI aksi #66. Batal kalau pemain bergerak, diserang, atau NPC menjauh, dan vaksin tidak berkurang.
3. Saat selesai: `RemoveItem("Med_Vaccine", 1)`. `value -= vaccineReduction × GetSkillMultiplier(Medic)`, minimal 0.
4. Kalau `value < cureBelow`, `isProgressing = false`, dan `pendingTurn = false`. Kalau `value` 0, record dihapus.
5. `UpdateStage`, lalu tampilkan `Kondisi <nama> membaik`.

NPC medis yang terinfeksi tahap 4 tidak memberi pengali, karena skill-nya mati.

### Berubah menjadi zombie

**`private void TryTurn(NPCInfectionRecord r)`**:

1. Kalau siang (`DayNightDirector.Instance.IsNight == false`): `pendingTurn = true`, NPC berbaring, dan menunggu. Zombie akan langsung terbakar sinar matahari kalau berubah di siang hari, sehingga pemain mendapat kesempatan terakhir untuk memberi vaksin.
2. Kalau malam dan objek NPC aktif: `TurnInWorld(r)`.
3. Kalau malam dan objek NPC nonaktif: `TurnRemote(r)`.

`HandleNightfall` memanggil langkah 2 atau 3 untuk semua record `pendingTurn`.

**`private void TurnInWorld(r)`**:

1. NPC berhenti, `NPCState.Idle`, dan memainkan animasi `turnSeconds`. Selama itu, vaksin masih bisa menyelamatkannya kalau suntikan sudah berjalan.
2. Catat `position` dan `insideBase = BaseNavigation.IsInsideBase(position)`.
3. `NPCRoster.KillNpc(npcId, DeathReason.Zombified)`, yang menerbitkan `NPCDiedEvent` dan mencabut skill.
4. `ObjectPooler.Instance.SpawnFromPool(enemyId, position, rotasi)` lewat `SpawnManager`, dengan `DespawnReason` biasa. `enemyId` bernilai `zombie_desa` kalau NPC berada di Region 4 (`Logistic`), karena GDD bagian 5 hanya menyebut Zombie Desa di sana, dan `zombie_standar` di Region 1, 2, dan 3. Model pakaian NPC dipasang sebagai varian tampilan.
5. Terbitkan `NPCTurnedEvent { npcId, position, insideBase }`, kirim suara `turnNoiseRadius` lewat `NoiseEmitter` (#49), lalu tampilkan notifikasi.

**Akibat di base:** kalau `insideBase`, zombie muncul di dalam dinding. Dinding tidak melindungi, dan NPC lain dekatnya menjadi target `ThreatSystem` biasa, jadi bisa digigit dan ikut terinfeksi. Tidak ada aturan penularan tambahan.

**`private void TurnRemote(r)`**: tidak memunculkan zombie, karena region tidak aktif. `NPCRoster.KillNpc(...)`, lalu `RemoteBaseSimulation.ApplyStructureDamage(cluster, remoteTurnDamage)` pada cluster tempat NPC tercatat, dan tiap NPC lain di cluster mendapat `AddRemote(remoteBiteInfection)`. Pagi hari pemain membaca notifikasi. Kalau base tidak ada, NPC hanya dicatat hilang.

### Karantina dan mengusir

| Perintah | Syarat | Akibat |
|---|---|---|
| **Karantina** | Tahap 2 atau lebih, titik berjarak `quarantineMinDistance` dari struktur | NPC memakai `StayAtBase` di `quarantinePoint`. `Work` dan skill berhenti sementara. Kalau berubah di sana, zombie muncul di luar base. Bisa dibatalkan |
| **Usir** | Tahap apa saja, dengan konfirmasi | `NPCRoster.RemoveNpc(npcId)`. NPC berjalan pergi lalu hilang. Skill hilang permanen selama run, dan NPC tidak akan berubah di dekat base. `NPCDiedEvent` tidak dikirim, tetapi `NPCExpelledEvent { npcId }` dikirim |
| **Biarkan** | Tanpa perintah | NPC tetap bekerja sampai tahap 3 |

Tembakan pemain ke NPC mengikuti aturan friendly fire di #59, dan tidak ada aturan khusus NPC terinfeksi.

### Dampak ke skill

**`public float GetSkillFactor(string npcId)`** mengembalikan 1 (tahap 1 dan 2), `stage3SkillFactor` (tahap 3), dan 0 (tahap 4 atau karantina). `NPCRoster.GetSkillMultiplier` menghitung `1 + (nilaiSkill - 1) × GetSkillFactor`, untuk NPC terbaik yang hidup. Hasilnya kembali normal setelah vaksin menurunkan tahap. Skill hilang permanen kalau NPC berubah, mati, atau diusir, sesuai #76.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `NPCInfectionChangedEvent`, `NPCTurnedEvent`, dan `NPCExpelledEvent`. `NPCDiedEvent` dikirim lewat `NPCRoster`.

**Event yang didengar:** `HourChangedEvent`, `NightfallEvent`, dan `NPCDiedEvent` (menghapus record NPC yang mati karena sebab lain).

| Sistem | Hubungan |
|---|---|
| `InfectionSystem` (#64) | Sumber tahap, angka gigitan, dan vaksin. Infeksi pemain tidak berubah |
| `NPCRoster`, `NPCCompanion` (#76) | Menu perintah, `KillNpc`, `RemoveNpc`, dan `GetSkillMultiplier` |
| `DamagePipeline` (#59), `EnemyAttack` (#109) | `DamageInfo.infection` |
| `RemoteBaseSimulation` (#90) | `AddRemote` dan `ApplyStructureDamage` |
| `SpawnManager` (#62), `ObjectPooler` | Zombie hasil perubahan |
| `SunlightExposure` (#63) | Zombie baru mengikuti aturan matahari |
| `BaseNavigation` (#78), `ThreatSystem` (#108) | `IsInsideBase`, dan target zombie |
| `NPCBarks` (#100), `NotificationManager` (#22) | Ucapan dan pesan |
| `ItemUseSystem` (#66), `InventoryManager` | UI aksi tahan dan stok vaksin |
| `WorldStateSave` (#80) | Menyimpan record |

## 4. Kasus Khusus

- **Sistem nonaktif:** `enabled = false` membuat semua fungsi keluar, menu tidak menampilkan pilihan, `GetSkillFactor` mengembalikan 1, dan `CaptureState` menulis daftar kosong. NPC tetap berjalan sesuai #76.
- **Pool:** zombie hasil perubahan adalah objek pool biasa, dan `OnSpawned` mengatur ulang HP, state, skala, serta komponen. Variasi tampilan pakaian NPC dimatikan di `OnReturned`, supaya zombie berikutnya memakai model biasa. NPC sendiri bukan objek pool.
- **Game dijeda:** `HourChangedEvent` berhenti, jadi infeksi tidak naik. UI aksi tahan memakai waktu terskala, jadi ikut berhenti.
- **Tidur:** jam cepat tetap menaikkan infeksi. Kalau NPC mencapai 100 saat pemain tidur di malam hari, `TurnInWorld` membangunkan pemain lewat `NoiseEmitter` dan aturan #67.
- **Saat disimpan:** `records` masuk `world_npc_infection` sebagai array. `OnRestoreFinished` mengirim ulang tanda ke `NPCInfectionVisual`. Animasi berubah tidak disimpan. Kalau save terjadi di tengah `turnSeconds`, saat load NPC berubah langsung pada malam itu, karena `value` masih 100.
- **Permadeath:** semua record terhapus bersama save run (#82). Di run baru, semua NPC mulai sehat.
- **Region dimuat ulang:** `NPCInfectionVisual` membaca record di `OnEnable`.
- **NPC mati karena damage biasa saat terinfeksi:** dianggap mati biasa, sehingga tidak menjadi zombie. Ini usulan untuk menjaga aturan tetap sederhana.
- **Pemain `Follow`:** NPC berubah di dekat pemain walaupun jauh dari base, memakai `TurnInWorld`.
- **Vaksin habis saat `pendingTurn`:** NPC tetap berbaring sampai malam. Pemain masih bisa memakai karantina atau usir.

## 5. Keputusan Desain

**Usulan yang menunggu developer (dicatat di #85). Seluruh sistem menunggu persetujuan.**

1. **Pakai sistem ini?** Rekomendasi: ya, setelah #76 selesai, dan dimatikan selama belum diuji.
2. **Kenaikan:** rekomendasi lewat gigitan dengan nilai sama seperti pemain, ditambah 1,5 per jam setelah digigit.
3. **Biaya vaksin:** rekomendasi satu dosis biasa dari stok pemain, tanpa barang tambahan.
4. **Waktu berubah:** rekomendasi ditunda sampai malam, karena GDD membuat zombie terbakar saat siang.
5. **Jenis zombie:** rekomendasi `zombie_standar` di Region 1, 2, dan 3, serta `zombie_desa` di Region 4, sesuai `SpawnManager.md`.
6. **Berubah saat ditinggal:** rekomendasi dihitung data lewat #90, tanpa zombie sungguhan.
7. **Aksi lain:** rekomendasi karantina dan usir, tanpa struktur baru.
8. **Skill:** rekomendasi melemah di tahap 3, mati di tahap 4, kembali setelah sembuh, dan hilang permanen kalau berubah.
9. **NPC mati biasa saat terinfeksi:** rekomendasi tidak menjadi zombie.

## 6. Skenario Tes

Nyalakan `enabled` lewat `DevConsole` sebelum tes 2 sampai 13 dan tes 15.

1. Biarkan `enabled = false`, lalu pukul NPC dengan zombie. **Hasil:** tidak ada infeksi, dan menu tanpa `Beri vaksin`.
2. Pukul NPC dengan zombie standar. **Hasil:** nilai 5, `isProgressing = true`.
3. Pukul NPC dengan hewan. **Hasil:** nilai tetap.
4. Majukan 20 jam game. **Hasil:** nilai 35, tahap 2, ucapan batuk muncul.
5. Majukan sampai tahap 3 dengan NPC medis. **Hasil:** ikon tampil, kecepatan x0,85, dan pengali medis turun separuh.
6. Beri vaksin pada nilai 60. **Hasil:** stok berkurang 1, nilai 10 atau di bawahnya kalau ada medis lain, dan tahap turun.
7. Batalkan suntikan dengan bergerak. **Hasil:** vaksin tidak berkurang.
8. Capai 100 saat siang. **Hasil:** NPC berbaring, baru berubah saat `NightfallEvent`.
9. Capai 100 di dalam base saat malam. **Hasil:** zombie muncul di dalam, `NPCDiedEvent` dengan `Zombified`, dan skill hilang.
10. Karantina di luar base, lalu biarkan berubah. **Hasil:** zombie muncul di titik karantina.
11. Usir NPC. **Hasil:** NPC pergi, skill hilang, dan tidak ada zombie.
12. Tinggalkan NPC ke region jauh, majukan waktu sampai berubah. **Hasil:** base terkena 120 damage, tanpa zombie baru saat kembali.
13. Simpan pada tahap 3, keluar, lalu lanjutkan. **Hasil:** nilai, tahap, dan ikon sama.
14. Mati, lalu mulai run baru. **Hasil:** semua NPC sehat.
15. Capai 100 di Region 4 saat malam. **Hasil:** yang muncul `zombie_desa`, bukan `zombie_standar`.
