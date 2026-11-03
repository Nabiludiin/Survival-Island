# Blueprint: BaseNavigation.cs & BaseDoor.cs
**Lokasi:** `docs/blueprint/raft/BaseNavigation.md`
**Deskripsi Utama:** Pengubah jalur musuh saat base dibangun, pintu, deteksi pemain di dalam base, dan aturan zombie memilih dinding atau pintu yang diserang.
**Issue:** #78
**Rujukan GDD:** Bagian 3 (Horde Zombie menyerang base saat malam) dan bagian 4 (base sebagai tempat berlindung dan tempat NPC berlindung). Rincian pintu, kunci, dan pembagian posisi serangan adalah usulan, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

### Perbandingan pilihan NavMesh

| Hal | `NavMeshObstacle` dengan carving | Bake ulang per area |
|---|---|---|
| Kapan diperbarui | Saat obstacle muncul, hilang, atau berpindah | Saat `NavMeshSurface.UpdateNavMesh` dipanggil |
| Biaya | Hanya tile yang tersentuh | Membaca ulang geometri di area itu |
| Jeda setelah bangun | Sekitar satu frame | Bisa beberapa frame, walaupun async |
| Dinding hancur beruntun | Banyak obstacle hilang, dan tile yang sama hanya dibangun sekali per frame | Permintaan bake harus diantre |
| Bangunan bertingkat | Tidak menambah jalur ke lantai atas | Bisa menambah, tetapi perlu sumber geometri `Structure` |
| Kecocokan dengan #88 | Cocok. `RegionStreaming.md` melarang bake saat bermain | Bertentangan dengan aturan itu |

**Pilihan: carving.** Jalur menjadi benar-benar terhalang, sehingga `PathPartial` bisa dipercaya sebagai tanda pemain tidak terjangkau. Dampak performanya dibatasi lewat aturan di bawah dan diukur pada skenario tes.

### Aturan obstacle per struktur

`BuildableData.navMode` (#79) menentukan perilakunya.

| `navMode` | Struktur | `NavMeshObstacle` |
|---|---|---|
| `Carve` | Dinding dan rangka pintu pada `level == 0` | `carving = true`, `carveOnlyStationary = false` |
| `Avoid` | Kasur, peti, dan perabot kecil | `carving = false`. Agen hanya menghindar secara lokal |
| `None` | Lantai, atap, dan tangga | Tidak ada |

- Struktur pada `level > 0` tidak memakai carving, karena tidak menyentuh NavMesh di tanah.
- Bentuk `Box`, dengan ukuran mengikuti collider utama. Dinding bersebelahan harus bertemu tanpa celah.
- Struktur tidak pernah bergerak, jadi `carveOnlyStationary` dimatikan. Upgrade (#47) langsung menutup jalur pada frame yang sama.

### `BaseDoor`

```csharp
public enum DoorState { Closed, Open, Locked }

public class BaseDoor : MonoBehaviour, IInteractable, ISaveable
{
    public Transform leafPivot;            // Daun pintu yang berputar
    public NavMeshObstacle leafObstacle;   // Menutup lubang pintu
    public NavMeshLink npcLink;            // Lewat pintu tertutup, hanya untuk NPC
    public DoorState State { get; private set; } = DoorState.Closed;
}
```

| Bagian prefab `Door_Wood` | Layer dan tag | Fungsi |
|---|---|---|
| Rangka kiri dan kanan | `Structure`, tag `BaseDoor` | Selalu carving |
| Ambang atas | `Structure`, tag `BaseDoor` | Tanpa obstacle |
| Daun pintu | `Structure`, tag `BaseDoor` | Berputar. `leafObstacle` menutup lubang saat tertutup |
| Zona interaksi depan dan belakang | `Interactable`, collider non-trigger | Target `PlayerInteractor`, sedikit menonjol dari daun pintu |
| Zona gerendel depan dan belakang | `Interactable`, collider non-trigger | Target `BaseDoorLatch` untuk mengunci |

Lebar lubang pintu 1,4 meter, supaya agen `Humanoid` dengan radius 0,4 meter bisa lewat. Agen `Large`, seperti Beruang Kutub, tidak muat.

### NavMesh Area baru

| Area | Dipakai oleh | `areaMask` |
|---|---|---|
| `DoorLink` | `npcLink` di tiap pintu | Dimuat agen NPC. Tidak dimuat agen musuh |

### Variabel `BaseNavigation`

`BaseNavigation` adalah manager di scene `IslandPhase`.

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Instance` | `BaseNavigation` | `null` | Singleton per scene |
| `IsPlayerInsideBase` | `bool` | `false` | Hasil pemeriksaan terakhir |
| `slotsByStructure` | `Dictionary<string, BreachSlots>` | Kosong | Posisi serangan per struktur |
| `pathCheckQueue` | `Queue<EnemyAI>` | Kosong | Antrean pemeriksaan jalur |
| `sharedPath` | `NavMeshPath` | Dibuat sekali | Dipakai ulang oleh semua pemeriksaan |
| `overlapBuffer` | `Collider[]` | Ukuran 32 | Buffer pencarian struktur |

```csharp
public struct BreachTarget
{
    public string structureId;
    public IDamageable target;
    public Vector3 slotPosition;
    public int slotIndex;
}
```

### Tambahan pada `EnemyData`

| Field | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `canAttackBuildings` | `bool` | `true` untuk zombie, `false` untuk hewan | Hewan memakai `TargetUnreachable` (#61) |

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.baseNav.pathCheckIntervalSeconds` | 1 | Jeda pemeriksaan jalur tiap zombie |
| `GameBalanceConfig.baseNav.maxPathChecksPerFrame` | 4 | Batas `NavMesh.CalculatePath` per frame |
| `GameBalanceConfig.baseNav.spawnGraceSeconds` | 2 | Zombie baru muncul belum boleh dianggap terhalang |
| `GameBalanceConfig.baseNav.blockedConfirmSeconds` | 1,5 | Lama jalur harus terhalang sebelum pindah state |
| `GameBalanceConfig.baseNav.retargetCooldownSeconds` | 4 | Jeda sebelum zombie boleh mengganti target |
| `GameBalanceConfig.baseNav.targetSearchRadius` | 6 | Radius pencarian dinding dan pintu dari ujung jalur |
| `GameBalanceConfig.baseNav.maxAttackersPerWall` | 4 | Penyerang per dinding |
| `GameBalanceConfig.baseNav.maxAttackersPerDoor` | 3 | Penyerang per pintu |
| `GameBalanceConfig.baseNav.slotSpacing` | 0,8 | Jarak antar-posisi serangan |
| `GameBalanceConfig.baseNav.slotStandoff` | 0,9 | Jarak posisi serangan dari permukaan struktur |
| `GameBalanceConfig.baseNav.waitRingRadius` | 4 | Jarak menunggu kalau semua posisi penuh |
| `GameBalanceConfig.baseNav.insideCheckSeconds` | 0,5 | Jeda pemeriksaan `IsPlayerInsideBase` |
| `GameBalanceConfig.baseNav.insideRayDistance` | 6 | Panjang ray pemeriksaan dinding |
| `GameBalanceConfig.baseNav.insideMinWallHits` | 3 | Dinding minimum dari empat arah |
| `GameBalanceConfig.baseNav.doorSwingSeconds` | 0,4 | Lama pintu berputar |
| `GameBalanceConfig.baseNav.doorNpcAutoCloseSeconds` | 1,5 | Pintu menutup kembali setelah NPC lewat |
| `GameBalanceConfig.baseNav.lockedDoorDamageMultiplier` | 0,75 | Damage ke pintu yang terkunci |

## 2. Detail Logika & Fungsi

### Dampak performa carving

| Aturan | Alasan |
|---|---|
| Hanya dinding dan rangka pintu di `level == 0` | Jumlah obstacle mengikuti keliling base, bukan seluruh struktur |
| Lantai dan atap tanpa obstacle | Tidak mengubah tile |
| `maxStructures` 400 (#79) | Batas atas jumlah obstacle |
| Runtuhan paling banyak 4 per frame (#79) | Perubahan tile tersebar |
| Region dilepas | `structureRoot` nonaktif, sehingga obstacle mati bersama region (#88) |
| Region kembali | Obstacle aktif sekaligus. Terjadi selagi `RegionStreaming` menyalakan akar bangunan, dan masuk batas hitch di #88 |

Perintah `navstat` di `DevConsole` (#56) menampilkan jumlah obstacle carving, waktu `NavMeshManager` dari Profiler, dan zombie yang sedang memeriksa jalur.

### Pintu

**`public bool Interact(in InteractContext ctx)`** menjalankan `Toggle()`. `HoldSeconds = 0`, dan interaksi tetap boleh saat membawa Heavy Item, sesuai `IslandInteractable.md`.

**`public void Toggle()`**:

| Status | Syarat | Hasil |
|---|---|---|
| `Closed` | Busur putar kosong | `Open`. Pintu berputar menjauhi pemain |
| `Open` | Lubang pintu kosong | `Closed` |
| `Locked` | Tidak ada | Ditolak dengan `InteractBlockReason.Locked` dan suara gerendel |

Busur dan lubang pintu diperiksa dengan `PhysicsQuery.OverlapBox` dan `PhysicsMasks.Occupant`. Kalau ada yang menghalangi, tampil notifikasi `Terhalang`, dan status tidak berubah. Dengan begitu, daun pintu tidak pernah menembus pemain, NPC, atau zombie.

**`BaseDoorLatch`** adalah `IInteractable` terpisah pada zona gerendel. Pada `Closed`, `E` mengubahnya ke `Locked`. Pada `Locked`, `E` mengubahnya ke `Closed`. Gerendel tidak tersedia saat `Open`.

**Saat status berubah**, `SetState(DoorState next)`:

1. Tulis `State`, lalu jalankan coroutine putar selama `doorSwingSeconds`.
2. Atur NavMesh sesuai tabel berikut.
3. Terbitkan `DoorStateChangedEvent { structureId, state }`.

### NavMesh pintu dan akses NPC

| Status | `leafObstacle` | `npcLink` | Musuh | NPC |
|---|---|---|---|---|
| `Open` | Mati | Mati | Lewat NavMesh biasa | Lewat NavMesh biasa |
| `Closed` | Nyala | Nyala | Terhalang, lalu menyerang | Lewat `npcLink` |
| `Locked` | Nyala | Mati | Terhalang, lalu menyerang | Terhalang. NPC tetap di sisi tempatnya berada |

**NPC melewati pintu tertutup:**

1. Agen NPC memakai `autoTraverseOffMeshLink = false`.
2. Saat `agent.isOnOffMeshLink` dan `currentOffMeshLinkData.owner` adalah `npcLink`, NPC memanggil `door.RequestNpcPass(this)`.
3. Pintu terbuka. Status tetap dicatat sebagai `Closed` untuk NavMesh sampai NPC selesai, jadi jalur zombie tidak ikut berubah.
4. Setelah daun terbuka, NPC berjalan di sepanjang link, lalu memanggil `agent.CompleteOffMeshLink()`.
5. Pintu menutup setelah `doorNpcAutoCloseSeconds`, kalau lubang pintu sudah kosong. Kalau tidak kosong, pintu menunggu.

Zombie yang masuk selama pintu terbuka memang terjadi, dan menjadi risiko membiarkan NPC keluar masuk saat malam. Perintah `StayAtBase` (#76) mengurangi risiko itu.

### HP dan tag pintu

- HP dan pengali mengikuti `Door_Wood` di `raft/BuildMenu.md`.
- Tag `BaseDoor` menjadi target `AttackBuilding`, sama seperti `BaseWall`.
- Saat `Locked`, damage `Structural` dikali `lockedDamageMultiplier`, lewat `BaseHealth.SetDamageMultiplier` dari `BaseDoor`.
- Saat pintu hancur, `StructureDestroyedEvent` melepas penyerangnya. Rangka, daun, dan `npcLink` hilang bersama objek, sehingga lubang terbuka untuk semua agen.

### Pemain di dalam base

**Perbandingan:**

| Hal | Volume trigger | Cek struktur di sekitar |
|---|---|---|
| Bangunan bentuk bebas | Volume harus dibuat dari susunan sel | Tidak perlu tahu bentuk |
| Saat dinding hancur | Volume harus dibangun ulang | Langsung ikut berubah |
| Biaya | Murah saat bermain, rumit saat bangunan berubah | Lima raycast tiap 0,5 detik |

**Pilihan: cek struktur di sekitar.**

**`public bool IsInsideBase(Vector3 position)`**:

1. Dari `position + Vector3.up`, tembakkan `PhysicsQuery.Raycast` ke empat arah dunia sejauh `insideRayDistance`, dengan `PhysicsMasks.BuildSurface`.
2. Hitung arah yang mengenai collider bertag `BaseWall` atau `BaseDoor`.
3. Kembalikan `true` kalau hitungannya `>= insideMinWallHits`.

Atap tidak disyaratkan, karena zombie tidak memanjat dinding, dan base awal bisa saja belum beratap. Pintu terbuka menghilangkan satu arah, sehingga tiga dinding lainnya tetap cukup.

**`IEnumerator InsideLoop()`** memanggil `IsInsideBase(posisi pemain)` setiap `insideCheckSeconds`. Kalau berubah, terbitkan `PlayerBaseStateChangedEvent { isInside }`. Fungsi yang sama dipakai `SleepSystem` (#67) untuk kasur.

### Syarat baru pindah ke `AttackBuilding`

`agent.velocity.magnitude < 0.1f` juga benar saat zombie baru muncul atau sedang menyerang, jadi syarat itu **dihapus**.

**`EnemyAI`**, saat state `Chase` dan `data.canAttackBuildings`:

1. Abaikan selama `spawnGraceSeconds` setelah `OnSpawned`.
2. Setiap `pathCheckIntervalSeconds`, masuk `pathCheckQueue`, dengan jeda awal acak dari ID musuh supaya tidak serempak. `Update` milik `BaseNavigation` memproses paling banyak `maxPathChecksPerFrame`.
3. Kalau `agent.pathPending`, lewati pemeriksaan ini.
4. Kalau `agent.pathStatus == PathComplete`, `blockedTimer = 0`.
5. Kalau `PathPartial` atau `PathInvalid`, tambah `blockedTimer` sebesar waktu sejak pemeriksaan terakhir.
6. Kalau `blockedTimer >= blockedConfirmSeconds`, panggil `BaseNavigation.TryAssignBreachTarget(this, out BreachTarget t)`.
7. Kalau berhasil, atur `currentBreach = t`, lalu `AIState.AttackBuilding`. Kalau gagal, tetap `Chase` menuju titik terdekat, dan `blockedTimer = 0`.

Pemain yang berdiri di atas batu tanpa jalur tidak memicu serangan apa pun, karena tidak ada struktur yang ditemukan pada langkah berikutnya.

### Memilih dinding atau pintu

**`public bool TryAssignBreachTarget(EnemyAI enemy, out BreachTarget target)`**:

1. Ambil `end`, yaitu titik terakhir `agent.path.corners`, atau posisi zombie kalau kosong.
2. **Garis ke pemain:** `PhysicsQuery.Raycast` dari `end + Vector3.up` menuju titik tengah pemain, dengan `PhysicsMasks.BuildSurface`. Kalau mengenai collider bertag `BaseWall` atau `BaseDoor`, struktur itu menjadi kandidat pertama.
3. **Sekitar ujung jalur:** `PhysicsQuery.OverlapSphere(end, targetSearchRadius, PhysicsMasks.AIAttack, overlapBuffer)`. Ambil collider bertag `BaseWall` atau `BaseDoor`, lalu urutkan dari yang terdekat ke `end`.
4. Untuk kandidat berurutan, panggil `TryReserveSlot(structureId, enemyId, end, out slot)`. Kandidat pertama yang punya posisi kosong menang.
5. Kalau kosong semua tetapi ada kandidat, zombie mendapat posisi tunggu pada `waitRingRadius`, dan mencoba lagi pada pemeriksaan berikutnya.
6. Kalau tidak ada kandidat, kembalikan `false`.

Target dipilih dari ujung jalur, bukan dari posisi zombie, supaya hasilnya tetap dinding yang benar-benar berada di antara zombie dan pemain.

**Pemain berada di lantai atas:** NavMesh hanya ada di tanah (#88), jadi jalur selalu `PathPartial`. Langkah 3 menemukan dinding di bawah pemain. Saat dinding hancur, lantai atas ikut runtuh lewat grafik dukungan (#79).

### Membagi posisi serangan

**`BreachSlots`** dibuat saat struktur pertama kali diserang, lalu dipakai ulang:

```csharp
public class BreachSlots
{
    public Vector3[] outerPositions;    // Di kedua sisi struktur
    public int[] occupantIds;           // -1 berarti kosong
    public int attackerCount;
}
```

**`private bool TryReserveSlot(string id, int enemyId, Vector3 from, out int slot)`**:

1. Kalau `attackerCount >= maxAttackersPerWall` atau `maxAttackersPerDoor`, kembalikan `false`.
2. Hitung sisi struktur yang menghadap `from`. Hanya posisi di sisi itu yang dipakai, supaya zombie di luar tidak berebut titik di dalam base.
3. Pilih posisi kosong terdekat yang berjarak `slotStandoff` dari permukaan. Jaraknya sudah diisi `slotSpacing`.
4. Pastikan titik itu sah lewat `NavMesh.SamplePosition`, dengan radius 1 meter. Kalau tidak sah, coba posisi lain.
5. Isi `occupantIds`, lalu tambah `attackerCount`.

**Saat zombie sudah punya target:**

- `agent.isStopped = false`, dan `SetDestination(slotPosition)`.
- Sesampainya di posisi, `agent.isStopped = true`, lalu putar menghadap struktur.
- Serangan dijalankan `EnemyAttack` (#109), dengan `DamageType.Structural` langsung ke `currentBreach.target`, sesuai `DamagePipeline.md`.
- `ReleaseSlot` dipanggil saat keluar dari state, mati, atau `OnReturned`.

### Kembali ke `Chase` dan retarget

| Pemicu | Tindakan |
|---|---|
| `StructureDestroyedEvent` untuk target itu | Semua penyerang dilepas, `blockedTimer = 0`, lalu `Chase` pada frame berikutnya, sesudah NavMesh berubah |
| `DoorStateChangedEvent` menjadi `Open` pada target itu | Sama, karena lubang pintu terbuka |
| `DaybreakEvent` | `BurnToAshes` menang atas semua state (#63) |
| Pemeriksaan jalur menjadi `PathComplete` | Kembali ke `Chase`. Saat `AttackBuilding`, pemeriksaan memakai `NavMesh.CalculatePath` dengan `sharedPath` dan `areaMask` musuh |
| Jalur menuju `slotPosition` gagal | Lepas posisi, lalu coba lagi setelah `retargetCooldownSeconds` |
| Target lain lebih dekat | Diabaikan selama `retargetCooldownSeconds`, supaya zombie tidak bolak-balik |

**`OnStructureDestroyed`** mengambil pemetaan struktur ke `occupantIds`, jadi tidak perlu mencari musuh satu per satu. Data `BreachSlots` dikembalikan ke daftar cadangan setelah struktur hilang.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `DoorStateChangedEvent` (`BaseDoor`, baru), `PlayerBaseStateChangedEvent` (`BaseNavigation`, baru, sticky).

**Event yang didengar:** `StructureDestroyedEvent`, `DoorStateChangedEvent`, dan `RegionLoadStateChangedEvent` (mengosongkan `BreachSlots` region yang dilepas).

| Sistem | Hubungan |
|---|---|
| `EnemyAI` | Memakai syarat dan fungsi di atas. Field `playerLayer`, `baseStructureLayer`, dan Obstruction Check lama dihapus |
| `BuildManager`, `BuildMenu` (#79) | `navMode`, `level`, dan daftar struktur |
| `BuildingIntegrity` (#47) | Upgrade mengganti objek, dan `structureId` tetap sama, sehingga `BreachSlots` tetap sah |
| `DamagePipeline` (#59) | `BaseHealth` dan `DamageType.Structural` |
| `EnemyAttack` (#109) | Menjalankan pukulan ke `currentBreach.target` |
| `ThreatSystem` (#108) | Menentukan target gerak. Pengecekan jalur berlaku pada target itu |
| `HordeCrowdManager` (#93) | Tidak mengubah `BreachSlots`, dan memakai state untuk LOD |
| `NPCCompanion`, `NPCRoster` (#76) | Memakai `npcLink`, `RequestNpcPass`, dan `IsInsideBase` |
| `IslandInteractable` (#17) | Pintu boleh dipakai saat membawa Heavy Item |
| `SleepSystem` (#67) | Memakai `IsInsideBase` |
| `RemoteBaseSimulation` (#90) | Tidak memakai sistem ini saat base ditinggal |
| `WildlifeAI` (#61) | Hewan memakai `TargetUnreachable` saat terhalang |
| `RegionStreaming` (#88) | Bangunan nonaktif bersama region |

**Batas pekerjaan:** HP dan perbaikan dinding di #47, jebakan di #26, dan menu bangun di #79.

## 4. Kasus Khusus

- **Objek pool:** `EnemyAI.OnReturned` wajib memanggil `ReleaseSlot`, dan mengatur `currentBreach = default`, `blockedTimer = 0`. `OnSpawned` memulai `spawnGraceSeconds` dari awal, dan memeriksa `agent.isOnNavMesh`. Zombie dari pool tidak membawa target lama.
- **Game dijeda:** coroutine pintu dan `InsideLoop` berhenti, karena memakai waktu berskala. Antrean jalur tidak berjalan, karena `EnemyAI` tidak menambah pemeriksaan saat waktu berhenti.
- **Permadeath:** tidak ada data di luar `StructureRecord`. `BreachSlots` dan status pintu hilang bersama scene dan save run (#82).
- **Save dan load:** `BaseDoor` menyimpan `State` lewat `StructureRecord.extraJson` (#79). Setelah load, obstacle dan link mengikuti status, tanpa animasi. `Open` tetap `Open`. `IsPlayerInsideBase`, `BreachSlots`, dan state zombie tidak disimpan.
- **Pintu terbuka saat NPC lewat lalu pemain menekan `E`:** `Toggle` ditolak selama `RequestNpcPass` berjalan.
- **Pintu dikunci saat NPC di luar:** NPC tidak bisa masuk. Jalurnya `PathPartial`, dan NPC menunggu di depan pintu sesuai #76.
- **Base hanya berlubang di atap:** zombie tidak memanjat, jadi tetap menyerang dinding.
- **Base tanpa dinding penuh:** ada jalur masuk, jadi `PathComplete`, dan zombie langsung `Chase` tanpa merusak bangunan.
- **Upgrade saat diserang:** ditolak oleh #47 selama jeda setelah kena damage.
- **Region dilepas saat zombie menyerang:** zombie kembali ke pool (#88), dan `ReleaseSlot` berjalan lewat `OnReturned`.
- **Zombie Desa keluar dari air dekat base:** aturan jalur tetap sama setelah `Emerging` (#63).
- **Bangunan dibuat di atas zombie:** ditolak `PlacementError.Occupied` (#79), sehingga agen tidak terkurung di dalam carving.
- **Agen terkurung karena carving:** kalau `agent.isOnNavMesh` bernilai `false` setelah perubahan, `EnemyAI` memakai `NavMesh.SamplePosition` lalu `agent.Warp`. Kalau gagal, zombie kembali ke pool.
- **Banyak zombie, semua posisi penuh:** zombie menunggu pada `waitRingRadius`, dan posisi itu dibagi #93.
- **Zombie meledak dekat dinding (#25):** damage `Explosion` lewat `DamagePipeline`, tanpa memakai posisi serangan.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** nama `AIState.AttackBuilding` dan tag `BaseWall` serta `BaseDoor` (`GLOSSARY.md`). Serangan ke dinding memakai `Structural` (`DamagePipeline.md`). Tidak ada bake saat bermain (`RegionStreaming.md`).

**Keputusan teknis di blueprint ini:**

- Tanda terhalang berasal dari status jalur, bukan kecepatan.
- Pemeriksaan jalur diantre, dengan batas per frame.
- Target dipilih dari ujung jalur.

**Usulan yang menunggu developer (dicatat di #85):**

1. **Carving** dipilih dibanding bake ulang per area.
2. **Pintu terkunci:** dikunci lewat gerendel dengan `E`, menahan NPC agar tidak lewat, dan menerima 75 persen damage. Zombie tidak bisa membuka pintu yang tidak terkunci, tetapi tetap menyerangnya. Rekomendasi: pakai, supaya kunci punya fungsi yang jelas.
3. **NPC lewat pintu tertutup:** boleh kalau tidak terkunci, dengan pintu menutup sendiri setelah 1,5 detik.
4. **Definisi di dalam base:** minimal 3 dari 4 arah terhalang dinding atau pintu dalam 6 meter. Atap tidak wajib.
5. **Jumlah penyerang:** 4 per dinding dan 3 per pintu. Sisanya menunggu.
6. **Zombie di lantai atas:** tidak bisa naik. Bangunan bertingkat aman dari gigitan langsung, tetapi bisa runtuh kalau penopangnya hancur.

## 6. Skenario Tes

1. Bangun empat dinding dan satu pintu tertutup, lalu berdiri di dalam saat malam. **Hasil:** Gizmo NavMesh terpotong, dan jalur zombie menjadi `PathPartial`.
2. Amati zombie baru muncul dari pool dalam keadaan diam. **Hasil:** tidak masuk `AttackBuilding` selama 2 detik pertama.
3. Biarkan zombie di luar base. **Hasil:** setelah sekitar 1,5 detik terhalang, zombie berjalan ke dinding atau pintu terdekat di arah pemain, lalu memukul.
4. Berdiri di atas batu tinggi tanpa base. **Hasil:** zombie berkumpul di bawah, dan tidak ada `AttackBuilding`.
5. Datangkan 20 zombie ke satu dinding. **Hasil:** paling banyak 4 memukul, tidak bertumpuk pada satu titik, dan sisanya memilih dinding sebelah atau menunggu.
6. Hancurkan dinding yang sedang diserang lewat `DevConsole`. **Hasil:** semua penyerang kembali ke `Chase` dan masuk lewat lubang.
7. Buka pintu saat zombie memukulnya. **Hasil:** zombie berhenti memukul dan masuk. Ini berlaku juga untuk daun pintu yang tidak bisa menutup kalau ada zombie di lubang.
8. Tutup pintu saat berdiri di lubang. **Hasil:** ditolak dengan `Terhalang`.
9. Kunci pintu, lalu perintahkan NPC keluar. **Hasil:** NPC tidak bisa lewat. Buka kunci. **Hasil:** NPC membuka pintu, lewat, dan pintu tertutup lagi.
10. Saat pintu tertutup tanpa kunci, biarkan zombie mendekat. **Hasil:** zombie tidak memakai `npcLink`, dan menyerang pintu.
11. Pukul pintu terkunci lewat `DevConsole`. **Hasil:** damage 75 persen. Buka kunci. **Hasil:** damage 100 persen.
12. Masuk dan keluar base sambil membuka `eventlog 1`. **Hasil:** `PlayerBaseStateChangedEvent` terkirim satu kali tiap perubahan.
13. Bawa Heavy Item, lalu tekan `E` pada pintu. **Hasil:** pintu terbuka.
14. Bangun base dengan 100 dinding carving, lalu jalankan `navstat` sambil menghancurkan 10 dinding. **Hasil:** waktu `NavMeshManager` tercatat, tanpa frame di atas 33 ms. Kalau melewati batas, turunkan `maxStructures` atau ubah pilihan di bagian 5.
15. Biarkan 60 zombie mengejar sambil membuka Profiler. **Hasil:** paling banyak 4 `CalculatePath` per frame, dan tanpa alokasi GC dari pemeriksaan.
16. Kunci pintu, simpan, keluar, lalu lanjutkan. **Hasil:** pintu tetap terkunci dan NavMesh sesuai.
17. Biarkan fajar tiba saat zombie memukul dinding. **Hasil:** zombie terbakar, dan posisi serangan kembali kosong.
