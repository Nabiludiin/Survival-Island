# Blueprint: ThreatSystem.cs (ITargetable)
**Lokasi:** `docs/blueprint/left-4-dead/ThreatSystem.md`
**Deskripsi Utama:** Satu aturan untuk memilih target musuh dari pemain, NPC, markas, umpan, dan suara, supaya sistem lain tidak saling menimpa target.
**Issue:** #108
**Rujukan GDD:** Bagian 3 (hewan menjaga teritori, zombie memburu MC dan menyerang base) dan bagian 4 (base sebagai tempat NPC berlindung). Rumus, bobot, dan urutan override adalah usulan, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

### Pola

`ThreatSystem` adalah manager per scene di `IslandPhase`. Hanya manager ini yang boleh mengubah `EnemyAI.CurrentTarget` dan `WildlifeAI.CurrentTarget`. Sistem lain memberi masukan lewat fungsi di bawah, bukan menulis target langsung.

```csharp
public enum TargetKind { Player, NPC, BaseAnchor, Lure }

public interface ITargetable
{
    int TargetId { get; }
    TargetKind Kind { get; }
    Vector3 Position { get; }
    Vector3 AimPoint { get; }       // Titik dada, untuk garis pandang dan tembakan NPC
    float ThreatWeight { get; }     // 0 sampai 1, bisa berubah saat bermain
    bool IsTargetable { get; }      // false saat mati, nonaktif, atau disembunyikan sistem
    IDamageable Damageable { get; } // null untuk BaseAnchor dan Lure
}
```

| Pemilik | Pemasang | `Kind` | `ThreatWeight` |
|---|---|---|---|
| Pemain | `PlayerTargetable` | `Player` | 1 |
| NPC | `NPCCompanion` (#76) | `NPC` | 0,6, atau 0,3 saat `TakeCover` |
| Markas | `BaseAnchor` di pusat base (#78) | `BaseAnchor` | Hanya saat Blood Moon |
| Umpan | Proyektil #96 | `Lure` | Nonaktif sampai #96 disetujui |

Dinding dan pintu **bukan** `ITargetable`. Target tetap tujuan utama, dan saat jalur terhalang, `BaseNavigation` mengisi `currentBreach` sebagai sasaran sementara.

### `ThreatProfile` (ScriptableObject)

`EnemyData.threatProfile` merujuk aset ini. Zombie dan hewan memakai aset berbeda.

| Field | `Threat_Zombie` | `Threat_Animal` | Fungsi |
|---|---|---|---|
| `allowedKinds` | Semua | `Player`, `NPC` | Jenis yang boleh dipilih |
| `kindWeightPlayer` | 40 | 40 | Bobot jenis pemain |
| `kindWeightNpc` | 30 | 40 | Bobot jenis NPC |
| `distanceWeight` | 30 | 30 | Bobot jarak |
| `visibleBonus` | 25 | 25 | Bonus saat terlihat |
| `recentDamageBonus` | 30 | 40 | Bonus setelah melukai musuh ini |
| `senseRange` | 60 | Dari `AnimalData.aggroRadius` | Jarak terjauh kandidat |
| `huntsUnseen` | `true` | `false` | Tetap memburu pemain tanpa melihatnya |
| `reactsToLure` | `true` | `false` | Bereaksi pada umpan |

### Variabel per musuh (`ThreatState`, struct di `EnemyAI` dan `WildlifeAI`)

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `current` | `ITargetable` | `null` | Target saat ini |
| `heldUntil` | `float` | 0 | Waktu minimum sebelum boleh berganti |
| `lastSeenTime` / `lastSeenPosition` | `float` / `Vector3` | 0 / `zero` | Posisi terakhir terlihat |
| `aggroTarget` / `aggroUntil` | `ITargetable` / `float` | `null` / 0 | Penyerang terakhir |
| `unreachableId` / `unreachableUntil` | `int` / `float` | -1 / 0 | Target yang sementara dicoret |
| `nextEvalTime` | `float` | Acak dari ID | Jadwal penilaian berikutnya |

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.threat.reselectSeconds` | 1 | Jeda penilaian musuh dekat |
| `GameBalanceConfig.threat.farReselectSeconds` | 2,5 | Jeda penilaian musuh jauh |
| `GameBalanceConfig.threat.farDistance` | 30 | Batas dekat dan jauh, dalam meter |
| `GameBalanceConfig.threat.minTargetHoldSeconds` | 3 | Waktu minimum sebelum berganti |
| `GameBalanceConfig.threat.switchMargin` | 15 | Selisih skor minimum untuk berganti |
| `GameBalanceConfig.threat.aggroOverrideSeconds` | 4 | Lama mengincar penyerang |
| `GameBalanceConfig.threat.crowdPenalty` | 8 | Pengurang skor per pengincar lain |
| `GameBalanceConfig.threat.maxChasersPerNpc` | 3 | Batas pengincar satu NPC |
| `GameBalanceConfig.threat.bloodMoonPlayerRange` | 8 | Jarak pemain yang terlihat tetap diserang saat Blood Moon |
| `GameBalanceConfig.threat.loseSightSeconds` | 8 | Lama target tidak terlihat dianggap hilang |
| `GameBalanceConfig.threat.searchSeconds` | 3 | Lama mencari di posisi terakhir |
| `GameBalanceConfig.threat.huntOffsetRadius` | 8 | Ketidaktepatan saat memburu tanpa melihat |
| `GameBalanceConfig.threat.huntUpdateSeconds` | 5 | Jeda pembaruan tujuan saat memburu |
| `GameBalanceConfig.threat.unreachableSeconds` | 6 | Lama tanpa jalur sebelum target dicoret |
| `GameBalanceConfig.threat.unreachableBanSeconds` | 10 | Lama target dicoret |
| `GameBalanceConfig.threat.maxSelectionsPerFrame` | 8 | Batas penilaian per frame |
| `GameBalanceConfig.threat.maxCandidates` | 16 | Batas kandidat terdaftar |

## 2. Detail Logika & Fungsi

### Rumus nilai ancaman

**`private float Score(in ThreatState s, ThreatProfile p, ITargetable t, float sqrDistance)`**:

```text
skor = kindWeight * t.ThreatWeight
     + distanceWeight * (1 - jarak / senseRange)
     + visibleBonus jika terlihat
     + recentDamageBonus jika t baru melukai musuh ini
     - crowdPenalty * jumlah pengincar lain pada t
```

Target dilewati kalau `IsTargetable` salah, `Kind` tidak ada di `allowedKinds`, jarak di luar `senseRange`, atau `TargetId == unreachableId`. `terlihat` dibaca dari hasil tersimpan `AISensory.CanSee(t)` (#49), jadi tidak ada raycast tambahan. `CanSee` hanya mengembalikan hasil untuk pemain dan `CurrentTarget`, dan kandidat lain bernilai `false`.

**Contoh zombie, pemain 20 meter dan tidak terlihat, NPC 5 meter dan terlihat:**

| Target | Jenis | Jarak | Terlihat | Skor |
|---|---|---|---|---|
| Pemain | 40 × 1 = 40 | 30 × 0,67 = 20 | 0 | 60 |
| NPC | 30 × 0,6 = 18 | 30 × 0,92 = 27,5 | 25 | 70,5 |

Zombie memilih NPC. Kalau pemain mendekat ke 10 meter dan terlihat, skornya 40 + 25 + 25 = 90, sehingga pemain bisa menarik zombie dari NPC.

### Memilih ulang

**`Update`** mengambil musuh dari antrean bergilir, paling banyak `maxSelectionsPerFrame`. Tiap musuh dinilai kalau `Time.time >= nextEvalTime`, dengan jeda `reselectSeconds` atau `farReselectSeconds`. Jadwal awalnya acak dari ID, supaya tidak serempak. Pada 50 zombie, semua terlayani dalam kurang dari 7 frame.

**`private void Evaluate(EnemyAI e)`**:

1. Lewati kalau state `Stagger`, `Knockback`, `Dying`, `BurnToAshes`, `SeekHiding`, `Submerged`, `Emerging`, atau `Dormant`.
2. Kalau `current` tidak lagi sah, panggil `Release`, lalu lewati waktu minimum.
3. Periksa override berurutan.
4. Hitung skor semua kandidat, lalu ambil yang tertinggi.
5. Terapkan **pencegah berganti:** ganti hanya kalau `current` kosong, atau `Time.time >= heldUntil` dan skor baru melewati skor `current` sebesar `switchMargin`.
6. Saat berganti, perbarui hitungan pengincar, `heldUntil = Time.time + minTargetHoldSeconds`, lalu panggil `EnemyAI.SetTarget(t)`.

### Override

Diperiksa dari atas ke bawah. Override mengabaikan `heldUntil` dan `switchMargin`.

| Urutan | Pemicu | Target | Berakhir |
|---|---|---|---|
| 1 | Baru dilukai pemain atau NPC | `aggroTarget`, lewat `ReportAttacker` | `aggroOverrideSeconds` habis, atau target tidak sah |
| 2 | `Lure` aktif dalam radius, dan `reactsToLure` | Umpan terdekat | Umpan selesai (#96) |
| 3 | Blood Moon, untuk zombie | `BaseAnchor`. Pemain atau NPC yang terlihat dalam `bloodMoonPlayerRange` tetap diserang | `BloodMoonEndedEvent` |
| 4 | Tidak ada | Skor biasa | Tidak ada |

Saat Blood Moon tanpa base, `BaseAnchor` tidak sah, dan zombie memburu pemain.

**`public void ReportAttacker(EnemyAI e, ITargetable attacker)`** dipanggil `EnemyAI` saat `EnemyHealth.Damaged` mencatat serangan pemain atau NPC. `DamageInfo.source` hanya sah pada frame itu, jadi target diambil saat itu juga.

### Zombie dan hewan

| Hal | Zombie | Hewan |
|---|---|---|
| Kandidat | Semua dalam `senseRange` | Hanya yang berada dalam `AnimalData.aggroRadius` |
| Batas gerak | Tidak ada | Jarak batas pengejaran dari titik awal (#61) |
| Tanpa target | Memburu pemain atau mencari | `ReturnToTerritory`. Target dikosongkan, dan pindah state menjadi tugas `WildlifeAI` |
| Umpan dan Blood Moon | Bereaksi | Tidak bereaksi |
| Base | Dinding diserang lewat #78 | `TargetUnreachable` (#61). Tidak menyerang dinding |
| Bertemu musuh lain | Tidak saling menyerang, karena mask `AIAttack` tidak memuat `Enemy` | Sama |

`WildlifeAI` memakai `Evaluate` yang sama, tetapi memanggil `ThreatSystem.IsWithinTerritory` lebih dulu. Hewan yang dilukai dari luar batas masuk `Alert`, lalu kembali ke teritorinya.

### Membagi target

- Tiap target memiliki hitungan pengincar di array `chaserCounts`.
- `crowdPenalty` membuat zombie berikutnya cenderung memilih target lain, sehingga kelompok tidak semuanya mengejar NPC yang sama.
- NPC yang sudah mencapai `maxChasersPerNpc` dilewati, kecuali lewat override 1.
- Pemain tidak punya batas pengincar. Jumlah yang boleh memukul dibatasi `AttackCoordinator` (#109), dan posisi di dinding dibagi `BreachSlots` (#78).

### Target mati, hilang, atau tidak terjangkau

| Keadaan | Tindakan |
|---|---|
| **Mati atau nonaktif** | `IsTargetable` salah. `current` langsung dilepas dan dinilai ulang pada giliran berikutnya, tanpa menunggu `heldUntil`. `PlayerDiedEvent` melepas semua target |
| **Tidak terlihat** | Selama `loseSightSeconds`, tujuan adalah `lastSeenPosition`. Sampai di sana, zombie mencari selama `searchSeconds` |
| **Tetap hilang atau belum pernah terlihat, `huntsUnseen` benar** | Tujuan menjadi posisi pemain ditambah acak dalam `huntOffsetRadius`, diperbarui tiap `huntUpdateSeconds`. Sesuai `Claude.md` (aggro penuh), tetapi tidak tepat, sehingga bersembunyi tetap berguna |
| **Tetap hilang, `huntsUnseen` salah** | Target dilepas. Musuh kembali ke `Idle` atau `ReturnToTerritory` |
| **Jalur terhalang base** | Target tetap. `BaseNavigation` memberi `currentBreach`, dan `EnemyAI` masuk `AttackBuilding` |
| **Tidak terjangkau, tanpa struktur** | Setelah `unreachableSeconds` di `PathPartial` atau `PathInvalid`, `unreachableId = TargetId` selama `unreachableBanSeconds`, lalu dinilai ulang. Kalau tinggal satu target, `EnemyAI` mengikuti `AITraversal.GetResponse` (#94), bukan hanya berjalan ke titik terdekat. Aturan anti-exploit milik #94, dan pengaman macet milik #62 |
| **Suara tanpa target** | Musuh tanpa target menuju posisi `NoiseEmittedEvent` terbaru dari `AISensory`, dalam mode mencari |

### Performa (#93)

- Kandidat terdaftar sedikit: pemain, NPC, satu `BaseAnchor`, dan umpan. Satu penilaian adalah `O(kandidat)`, memakai jarak kuadrat.
- Tidak ada `GetComponent`, `Find`, raycast, atau alokasi di `Evaluate`. Garis pandang dibaca dari `AISensory`.
- `HordeCrowdManager` (#93) boleh mengubah kelompok jauh dan dekat. `ThreatSystem` hanya menghormati jatah `maxSelectionsPerFrame`.
- `SetDestination` tetap dibatasi #93, yaitu hanya saat target bergeser cukup jauh. `SetTarget` tidak memanggilnya langsung.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** tidak ada. Target adalah data per musuh, dan `EnemyAI` membacanya.

**Event yang didengar:** `BloodMoonStartedEvent`, `BloodMoonEndedEvent`, `PlayerDiedEvent`, `NPCDiedEvent`, dan `NoiseEmittedEvent` lewat `AISensory`.

| Sistem | Hubungan |
|---|---|
| `EnemyAI` | Mengganti `playerTransform` dengan `CurrentTarget`, dan memanggil `Register`, `Unregister`, serta `ReportAttacker` |
| `WildlifeAI` (#61) | Memakai `Threat_Animal` |
| `AISensory` (#49) | Menyediakan `CanSee`, dan suara terakhir lewat `LastStimulusPosition` |
| `BaseNavigation` (#78) | `currentBreach`, dan pemasangan `BaseAnchor` |
| `HordeManager` (#50) | Mengirim event Blood Moon |
| `NPCCompanion`, `NPCRoster` (#76) | Memasang `ITargetable` |
| `ThrowableSystem` (#96) | Memasang `Lure` kalau disetujui |
| `EnemyAttack` (#109) | Menerima `ITargetable` dari `CurrentTarget` |
| `HordeCrowdManager` (#93) | Jatah per frame dan pembaruan tujuan |
| `AITraversal` (#94), `SpawnManager` (#62) | Anti-exploit dan pengaman macet |

**Batas pekerjaan:** cara melihat dan mendengar di #49, dan cara menyerang struktur di #78.

## 4. Kasus Khusus

- **Objek pool:** `EnemyAI.OnSpawned` mendaftar dan mengosongkan `ThreatState`, dengan `nextEvalTime` baru. `OnReturned` memanggil `Unregister`, yang mengurangi `chaserCounts` dan mengosongkan rujukan target. `Unregister` aman dipanggil dua kali, karena `LuckyMachine` (#72) memanggilnya saat vakum mulai, dan `EnemyAI.OnReturned` memanggilnya lagi.
- **Game dijeda:** `Update` tetap berjalan, tetapi `Time.time` tidak maju, jadi tidak ada penilaian baru.
- **Permadeath dan save:** tidak ada data yang disimpan. Musuh tidak disimpan (#88). Setelah load, musuh baru memilih target sendiri. `BaseAnchor` dibangun ulang dari struktur tersimpan.
- **Pemain pindah region:** musuh yang dikembalikan ke pool (#62) memanggil `Unregister`. Musuh yang tetap mengejar mengikuti aturan biasa.
- **Pemain di dalam base:** target tetap pemain. `PathPartial` memicu #78.
- **Pemain tertahan (#110):** target tidak berubah. #110 memakai `SetAttackerLimit`.
- **NPC ikut masuk base:** `TakeCover` menurunkan `ThreatWeight`, tetapi NPC tetap bisa dipilih kalau dekat.
- **Musuh dilukai ledakan lingkungan:** `source` kosong, jadi `ReportAttacker` tidak terpanggil.
- **Zombie Lumut di `Dormant`:** dilewati sampai #25 membangunkannya.
- **Sampai di `BaseAnchor`:** `Damageable` bernilai `null`, jadi tidak ada pukulan. Zombie memanggil `BaseNavigation.TryAssignBreachTarget`, dan kalau tidak ada struktur, target pindah ke pemain.
- **Fase 1:** `ThreatSystem` tidak ada di `ShipPhase`. Zombie kapal mengincar pemain lewat `EnemyAI.SetTarget` langsung, sebagai satu-satunya pengecualian dari aturan penulis tunggal.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** serangan dinding milik #78, deteksi milik #49, dan Blood Moon yang mengabaikan Stealth menurut #50.

**Keputusan teknis di blueprint ini:**

- `ThreatSystem` satu-satunya penulis target di Fase 2.
- Dinding bukan `ITargetable`.
- Penilaian dibagi per frame, dengan hasil sensor yang sudah tersimpan.

**Usulan yang menunggu developer (dicatat di #85):**

1. **Urutan override:** baru ditembak, lalu umpan, lalu Blood Moon. Pemain yang menembak selalu mendapat balasan, dan umpan tetap berguna saat Blood Moon.
2. **NPC sebagai target:** zombie dan hewan boleh mengincar NPC, dengan bobot lebih rendah dari pemain. Selaras dengan keputusan #76 kalau disetujui.
3. **Zombie yang kehilangan jejak:** tetap memburu pemain tanpa posisi tepat, dalam radius 8 meter. Ini menengahi `Claude.md` (aggro penuh) dan #49 (Stealth).
4. **Waktu:** penilaian tiap 1 detik, dan target tetap minimal 3 detik.
5. **Batas pengincar NPC:** 3 per NPC.

## 6. Skenario Tes

1. Spawn 1 zombie jauh dari NPC dengan pemain terlihat. **Hasil:** zombie mengincar pemain, dan `CurrentTarget` tampil di `DevConsole`.
2. Tempatkan NPC 5 meter dari zombie dan pemain 20 meter tanpa terlihat. **Hasil:** zombie memilih NPC, sesuai contoh skor.
3. Tembak zombie yang mengincar NPC. **Hasil:** zombie berbalik ke pemain selama 4 detik, walau masih dalam waktu minimum.
4. Gerak bolak-balik di antara dua target yang skornya berdekatan. **Hasil:** zombie tidak berganti lebih cepat dari 3 detik.
5. Mulai Blood Moon lewat `DevConsole` dengan base. **Hasil:** zombie menuju base. Pemain yang berdiri dalam 8 meter dan terlihat diserang.
6. Mulai Blood Moon tanpa base. **Hasil:** zombie memburu pemain.
7. Spawn 10 zombie dekat satu NPC. **Hasil:** paling banyak 3 mengincar NPC, sisanya pemain.
8. Bunuh NPC yang diincar. **Hasil:** zombie langsung memilih target lain, tanpa error null.
9. Bersembunyi di balik batu setelah terlihat. **Hasil:** zombie ke posisi terakhir, mencari 3 detik, lalu bergerak mendekat tanpa langsung tepat.
10. Masuk ke base tertutup. **Hasil:** target tetap pemain, lalu zombie menyerang dinding.
11. Berdiri di batu yang tidak terjangkau, tanpa struktur. **Hasil:** setelah 6 detik target dicoret kalau ada target lain. Tanpa target lain, zombie mengikuti `AITraversal.GetResponse`.
12. Dekati serigala, lalu lari melewati batas pengejaran. **Hasil:** target dilepas, dan serigala `ReturnToTerritory`.
13. Bunyikan tembakan di dekat serigala yang jauh. **Hasil:** serigala tidak memilih suara sebagai target.
14. Bunyikan tembakan di dekat zombie tanpa target. **Hasil:** zombie menuju sumber suara.
15. Jalankan uji 50 zombie (#56) dengan Profiler. **Hasil:** `ThreatSystem` tanpa alokasi GC, paling banyak 8 penilaian per frame.
16. Pakai ulang zombie dari pool. **Hasil:** target lama tidak terbawa, dan `chaserCounts` tetap benar.
