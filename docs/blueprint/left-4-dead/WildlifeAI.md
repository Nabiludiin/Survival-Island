# Blueprint: WildlifeAI.cs & AnimalData.cs
**Lokasi:** `docs/blueprint/left-4-dead/WildlifeAI.md`
**Deskripsi Utama:** AI hewan buas siang hari: teritorial, punya serangan khusus per spesies, kembali ke sarang saat malam, dan bangkainya bisa dikuliti.
**Issue:** #61
**Rujukan GDD:** Bagian 3 (ancaman siang, hewan kembali ke sarang, dan berburu) dan bagian 5 (hewan per region). Angka dan bentuk serangan adalah usulan.

## 1. Arsitektur & Variabel Utama

`AIState` milik zombie tidak dipakai. Hewan punya `AnimalState` (`GLOSSARY.md`) dan FSM sendiri, yang berjalan lewat coroutine dengan jeda `GameBalanceConfig.wildlife.tickSeconds` (0,25).

```csharp
public enum AnimalAttackKind : byte { Bite, Pounce, VenomBite, Charge, FlockDive }

[CreateAssetMenu(menuName = "Survival/Animal Data")]
public class AnimalData : EnemyData   // enemyId, maxHP, resistances berasal dari EnemyData (#25)
{
    public float wanderSpeed, chaseSpeed;
    public float aggroRadius;      // Radius mendeteksi pemain
    public float leashDistance;    // Jarak batas dari titik awal pengejaran
    public float wanderRadius;     // Radius keliling dari homePoint
    public AnimalAttackKind attackKind;
    public AttackData attack;      // Damage, jarak, dan jeda (#109)
    public HarvestEntry[] harvest; // Hasil buruan
    public bool canAttackBuildings = false; // Selalu false, dibaca #78
}
[Serializable] public struct HarvestEntry { public ItemData item; public int min, max; }
```

### Data per spesies (nilai awal)

| Spesies | `enemyId` | HP | Jalan / kejar | Damage | Aggro | Batas | `attackKind` | Daging / kulit |
|---|---|---|---|---|---|---|---|---|
| Serigala | `hewan_serigala` | 60 | 2 / 7 | 12 | 18 | 35 | `Bite` | 2 / 1 |
| Babi Hutan | `hewan_babi` | 90 | 2 / 6,5 | 18 | 12 | 20 | `Charge` | 3 / 1 |
| Jaguar | `hewan_jaguar` | 110 | 2,5 / 8,5 | 30 | 14 | 15 | `Pounce` | 2 / 2 |
| Ular Berbisa | `hewan_ular` | 25 | 0,8 / 3 | 6 | 5 | 8 | `VenomBite` | 1 / 0 |
| Beruang Kutub | `hewan_beruang` | 400 | 2 / 6 | 35 | 20 | 25 | `Bite` | 6 / 4 |
| Serigala Salju | `hewan_serigala_salju` | 75 | 2 / 7,5 | 14 | 20 | 35 | `Bite` | 2 / 2 |
| Anjing Liar | `hewan_anjing` | 45 | 2,5 / 7,5 | 10 | 16 | 30 | `Bite` | 1 / 1 |
| Gagak | `hewan_gagak` | 15 | 3 / 6 | 5 | 15 | 25 | `FlockDive` | 0 / 0 |

Daging adalah `Food_RawMeat` (#66), dan kulit adalah `Mat_Hide` (#77). Beruang memenuhi "berdarah tebal" lewat HP, dan tetap bisa dipancing ke kandang Lucky Machine (#72) lewat tag `Animal`.

### Variabel `WildlifeAI`

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `State` | `AnimalState` | `Wander` | State saat ini |
| `homePoint` | `Vector3` | Posisi spawn | Pusat teritori |
| `chaseStartPoint` | `Vector3` | - | Titik saat mulai mengejar |
| `groupId` | `int` | 0 | Dari `SpawnManager`, dipakai kawanan Gagak |
| `specialCooldownUntil` | `float` | 0 | Jeda serangan khusus |

### Angka umum (`GameBalanceConfig.wildlife`)

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `tickSeconds` | 0,25 | Jeda FSM |
| `alertSeconds` | 1,5 | Waktu tanda sebelum mengejar |
| `crouchAggroMultiplier` | 0,6 | Pengali radius aggro saat pemain jongkok |
| `noiseAggroMultiplier` | 0,5 | Pengali radius suara untuk hewan |
| `targetSampleRadius` | 2 m | Radius `NavMesh.SamplePosition` target |
| `returnHealPerSec` | 0,05 | Pemulihan HP (rasio per detik) saat kembali ke teritori |
| `reaggroCooldownSeconds` | 5 | Hewan yang kembali mengabaikan pemain selama ini |
| `pathCheckSeconds` | 1 | Jeda `NavMesh.CalculatePath` |
| `unreachableConfirmSeconds` | 2 | Syarat masuk `TargetUnreachable` |
| `unreachableGiveUpSeconds` | 15 | Batas menunggu target |
| `fleeSpeedMultiplier` | 1,2 | Pengali `chaseSpeed` saat ke sarang |
| `fleeTimeoutSeconds` | 60 | Batas waktu ke sarang |
| `skinHoldSeconds` | 3 | Lama menahan `E` saat menguliti |
| `carcassLifetimeSeconds` | 180 | Umur bangkai |

## 2. Detail Logika & Fungsi

### Diagram state

| State | Masuk saat | Perilaku | Keluar |
|---|---|---|---|
| `Wander` | Spawn, tiba di rumah | Jalan ke titik acak dalam `wanderRadius`, tunggu 2 sampai 6 detik | `Alert` |
| `Alert` | Pemain terdeteksi | Berhenti, menghadap pemain, dan bersuara selama `alertSeconds` | `Chase`, atau `Wander` kalau pemain hilang |
| `Chase` | Tanda selesai, atau kena damage di dalam teritori | Mengejar dengan `chaseSpeed` | `Attack`, `ReturnToTerritory`, `TargetUnreachable` |
| `Attack` | Masuk jarak serangan | Serangan sesuai `attackKind` | `Chase` |
| `ReturnToTerritory` | Melewati batas atau menyerah | Berjalan ke `homePoint`, dan memulihkan HP | `Wander` |
| `TargetUnreachable` | Jalur tidak lengkap | Menunggu di titik terdekat | `Chase`, `ReturnToTerritory` |
| `FleeToDen` | `NightfallEvent` | Lari ke `DenPoint`, abaikan pemain | Kembali ke pool |

### Deteksi

**`CheckAggro()`**, saat `Wander`:

1. Kalau `Time.time < reaggroUntil`, selesai.
2. `r = aggroRadius`, dikali `crouchAggroMultiplier` kalau `PlayerTraversal.IsCrouching`.
3. Kalau jarak ke target lebih dari `r`, selesai.
4. Garis pandang: `PhysicsQuery.Raycast` dengan `PhysicsMasks.AIVision`. Kalau tidak terhalang, masuk `Alert`.

**`HandleNoise(NoiseEmittedEvent e)`:** kalau `State == Wander` dan jarak ke sumber kurang dari `e.radius * noiseAggroMultiplier`, masuk `Alert` sambil menghadap sumber. Hewan tidak mengisi ikon mata #49.

**`EnemyHealth.Damaged`:** kalau `info.byPlayer` dan bukan `FleeToDen`, langsung `Chase` kalau masih di dalam teritori. Hewan yang dilukai dari luar batas masuk `Alert`, lalu kembali ke teritorinya, sesuai #108.

Target dipilih lewat `ThreatSystem.Evaluate` dengan profil `Threat_Animal` (#108), yang mengisi `WildlifeAI.CurrentTarget` lewat `SetTarget`. `senseRange` berasal dari `aggroRadius`, dan `ThreatSystem.IsWithinTerritory` dipanggil lebih dulu.

### Teritori

**`EnterChase()`** menyimpan `chaseStartPoint = transform.position`, dan mengatur `agent.speed = chaseSpeed`.

**`TickChase()`:**

1. Kalau `Distance(transform, chaseStartPoint) > leashDistance`, masuk `ReturnToTerritory`.
2. Petakan target: `NavMesh.SamplePosition(target, out hit, targetSampleRadius, agent.areaMask)`, lalu `SetDestination(hit.position)`.
3. Setiap `pathCheckSeconds`, jalankan `NavMesh.CalculatePath` dengan `NavMeshPath` milik objek. Kalau bukan `PathComplete` selama `unreachableConfirmSeconds`, masuk `TargetUnreachable`.
4. Kalau masuk jarak `attack.range`, masuk `Attack`.

**`EnterReturn()`:** `agent.speed = wanderSpeed * 1,5`, `reaggroUntil = Time.time + reaggroCooldownSeconds`. Selama state ini, HP naik `returnHealPerSec` per detik, supaya pemain tidak bisa memancing lalu mencicil HP dari batas teritori.

### Target tak terjangkau

Misalnya pemain di dalam base yang tertutup. Hewan tidak menyerang bangunan (`canAttackBuildings = false`, #78).

1. Bergerak ke ujung jalur parsial, lalu mondar-mandir sambil bersuara, supaya pemain tahu hewan masih ada.
2. Terus jalankan `CalculatePath`. Kalau menjadi `PathComplete`, misalnya pintu terbuka, kembali ke `Chase`.
3. Setelah `unreachableGiveUpSeconds`, masuk `ReturnToTerritory`.
4. **Anti-eksploit:** kalau kena damage di state ini, langsung `ReturnToTerritory` dengan `chaseSpeed`, dan `reaggroCooldownSeconds` dimulai. Pemain tidak bisa menembak hewan yang tidak berdaya sampai mati dengan aman.

### Malam hari

**`HandleNightfall(NightfallEvent e)`** berlaku untuk semua hewan, kecuali yang sudah mati:

1. Batalkan serangan khusus, panggil `PlayerPinnedState.ReleaseByAttacker` kalau sedang menahan, dan lepaskan `AmbushPerch`.
2. `State = FleeToDen`, `agent.speed = chaseSpeed * fleeSpeedMultiplier`.
3. Tujuan: `DenPoint.FindNearest(posisi)`, yaitu komponen di scene region. Kalau tidak ada, cari titik acak menjauhi pemain.
4. Saat tiba dalam 2 m, panggil `SpawnManager.Instance.Despawn(gameObject, DespawnReason.ReturnedToDen)`.
5. Setelah `fleeTimeoutSeconds`, panggil hal yang sama saat hewan di luar pandangan kamera.

Dalam `OnEnable`, kalau `DayNightDirector.IsDay == false`, hewan langsung `FleeToDen`.

### Serangan khusus

Damage selalu lewat `IDamageable.TakeDamage(in DamageInfo)`, dengan `infection = 0` (#64). Waktu ayunan dan tanda serangan mengikuti `EnemyAttack` (#109).

**Terkaman Jaguar (`Pounce`):**

- `AmbushPerch` adalah penanda di pohon Region 2, dengan satu penghuni.
- Saat `Wander`, Jaguar menuju perch terdekat, `agent.enabled = false`, lalu naik ke cabang lewat animasi.
- Saat pemain berada dalam `pounceTriggerRadius` (9 m) dan tidak terhalang, beri tanda selama `pounceTelegraphSeconds` (0,8), berupa geraman dan daun bergoyang.
- Lompat lewat kurva parabola selama `pounceSeconds` (0,6) menuju posisi pemain saat tanda selesai. Mendarat dengan `NavMesh.SamplePosition` dan `agent.Warp`. Kalau gagal, panggil `Despawn` dengan `OffNavMesh`.
- Kena pemain dalam radius 1,5 m: damage 30, lalu `PlayerPinnedState.TryPin(this)` (#110). `WildlifeAI` memakai `IPinAttacker`. Kalau `TryPin` ditolak, misalnya `pinned.enabled = false`, Jaguar hanya mundur dan jeda ulang berjalan. Selama menahan, damage 5 setiap 0,5 detik.
- Terkaman batal saat Jaguar menerima `pounceBreakDamage` (40), sesuai pola Hunter di `docs/REFERENSI_MUSUH_L4D.md`.
- Meleset: diam selama `pounceMissStunSeconds` (1,5), sebagai celah untuk pemain. Jeda ulang `pounceCooldownSeconds` (12).
- Saat `Chase`, Jaguar juga boleh menerkam dari tanah pada jarak 6 m, memakai jalur yang sama.

**Racun Ular (`VenomBite`):** gigitan dengan `DamageType.Poison`, jarak 1,5 m, jeda 3 detik. `DiseaseManager` (#45) membaca jenis damage ini dan memberi `Venom`, jadi `WildlifeAI` tidak mengenal `DiseaseManager`.

**Serudukan Babi (`Charge`):**

1. Dalam jarak 12 m, beri tanda menggaruk tanah selama `chargeTelegraphSeconds` (1).
2. Kunci titik tujuan pada garis lurus sejauh `chargeDistance` (12 m), lalu `agent.speed = chargeSpeed` (10). Tujuan tidak berubah lagi, jadi pemain bisa menghindar ke samping.
3. Setiap tick, `PhysicsQuery.OverlapSphere` dengan `PhysicsMasks.AIAttack` dan buffer tetap. Kena pemain: damage 18, satu kali per serudukan, plus getaran kamera (#24).
4. Kena `Structure` atau `Obstacle` lewat ray pendek `AIVision`: pusing selama `chargeStunSeconds` (2). Dinding tidak menerima damage.

**Kawanan Gagak (`FlockDive`):**

- `SpawnManager` memunculkan 5 sampai 8 Gagak dengan `groupId` yang sama.
- **Cara terbang:** tetap memakai `NavMeshAgent` di NavMesh tanah, sesuai `Claude.md`, dengan `agent.baseOffset = crowCruiseHeight` (4). Model dan hitbox ikut naik. Karena jalurnya tetap darat, Gagak tidak melewati dinding, dan tidak terbang ke atas laut.
- **Menukik:** `CrowFlock` adalah daftar statis per `groupId` yang membagi `maxDiversAtOnce` (2) token. Pemegang token menurunkan `baseOffset` ke 1,2 m dalam 0,5 detik, mematuk, lalu naik kembali dan melepas token.
- Melee hanya kena saat Gagak menukik. Peluru selalu bisa mengenai.
- Pemain di dalam base mengikuti aturan `TargetUnreachable`.

### Bangkai dan menguliti

`EnemyHealth.Died` menghentikan FSM. Animasi mati mengikuti #95. Kalau `data.leavesCarcass` (#95), `corpseRoutine` berhenti setelah `corpseHoldSeconds`, lalu mengaktifkan `AnimalCarcass`, yaitu komponen pada prefab hewan yang memakai `IInteractable`, dengan collider di layer `Interactable`. Gagak bernilai `false`, jadi memudar seperti mayat biasa.

| Fungsi | Isi |
|---|---|
| `IsAvailable` | `true` kalau `harvest` tidak kosong dan belum dikuliti |
| `HoldSeconds` | `skinHoldSeconds` |
| `GetPrompt()` | `Kuliti` |
| `CanInteract(ctx)` | `CarryingHeavyItem` kalau membawa suku cadang. `MissingTool` kalau `EventChannel<EquippedItemChangedEvent>.Last.itemId` bukan `Base_Pisau`. Selain itu `None` |
| `Interact(ctx)` | Lihat di bawah |

**`Interact`:**

1. Untuk tiap `HarvestEntry`, ambil jumlah dari aliran `RandomStreamId.Gameplay`.
2. `sisa = ctx.Inventory.AddItem(item, jumlah)`. Sisa dijatuhkan sebagai `WorldItem` dari pool (#66).
3. Panggil `NoiseEmitter.Emit(skinNoiseRadius, Tool)`, dengan nilai awal 6 m.
4. Panggil `SpawnManager.Instance.Despawn(gameObject, DespawnReason.Harvested)`.

Bangkai yang tidak dikuliti hilang setelah `carcassLifetimeSeconds`, tetap ada saat malam, dan tidak menghitung batas hewan hidup.

### Hewan dan zombie

Saat fajar dan senja keduanya bisa berpapasan. Mereka saling mengabaikan: tidak saling menargetkan, tidak saling memberi damage, dan hanya saling menghindar lewat agen. Lihat bagian 5.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** tidak ada dari `WildlifeAI`. `EnemyHealth` mengirim `EnemyHitEvent` dan `EnemyKilledEvent`, serta `NoiseEmitter` mengirim `NoiseEmittedEvent`.

**Event yang didengar:** `NightfallEvent`, `NoiseEmittedEvent`, dan `EquippedItemChangedEvent` (dibaca lewat `Last`). Event per objek: `EnemyHealth.Damaged` dan `Died`.

| Sistem | Hubungan |
|---|---|
| `SpawnManager` (#62) | Memunculkan hewan, mengisi `homePoint` dan `groupId`, serta menerima `Despawn` |
| `ThreatSystem` (#108) | Memilih target, dengan aturan teritorial |
| `EnemyAttack` (#109) | Waktu dan tanda serangan biasa |
| `EnemyHitReaction` (#95) | Animasi kena pukul dan mati, serta menyerahkan bangkai lewat `leavesCarcass` |
| `DamagePipeline` (#59) | `EnemyHealth` dan `IDamageable` |
| `PlayerPinnedState` (#110) | Terkaman Jaguar, lewat `IPinAttacker`, `TryPin`, dan `ReleaseByAttacker` |
| `DiseaseManager` (#45) | Membaca `DamageType.Poison` |
| `InventoryManager`, `ItemUseSystem` (#66) | Menerima hasil buruan |
| `CampfireManager` (#44), `CraftingStation` (#77) | Memakai `Food_RawMeat` dan `Mat_Hide` |
| `LuckyMachine` (#72) | Kandang mengenali tag `Animal` |
| `AITraversal` (#94) | `TraversalAgent` untuk link. Respons target tak terjangkau tetap milik blueprint ini |

## 4. Kasus Khusus

- **Objek pool:** `OnSpawned()` mengatur `State = Wander`, `CurrentHP` lewat `EnemyHealth`, `agent.enabled = true` lewat `Warp`, `agent.speed = wanderSpeed`, dan `agent.baseOffset` ke nilai prefab. `AnimalCarcass` dimatikan. Status jeda, `groupId`, perch, serta token Gagak dikosongkan di `OnReturned()`.
- **Game dijeda:** coroutine FSM dan `Time.deltaTime` berhenti. Tahan `E` juga berhenti, karena `Gameplay` mati (#83).
- **Tidur:** waktu dipercepat, jadi hewan yang tersisa ikut bergerak cepat, tetapi malam tidak punya hewan. `SpawnManager` tidak memunculkan hewan saat tidur.
- **Permadeath dan save:** hewan dan bangkai tidak disimpan (#88). Daging dan kulit di inventori mengikuti #66 dan #80, dan dihapus saat permadeath.
- **Region dilepas (#88):** `SpawnManager` mengembalikan semua hewan di region itu, termasuk bangkainya.
- **Pemain mati saat ditahan Jaguar:** `RunEndedEvent` membawa reset normal, dan `ReturnAllActive` menampung Jaguar.
- **Hewan mati di dalam `FleeToDen`:** bangkai tetap bisa dikuliti.
- **Perch dipakai Jaguar lain:** cari perch berikutnya, atau tetap keliling di tanah.
- **Tiga Serigala sekaligus:** setiap hewan menyimpan `chaseStartPoint` sendiri.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** hewan kembali ke sarang saat malam (GDD bagian 3). Hewan per region (GDD bagian 5). Daftar `AnimalState` (`GLOSSARY.md`). Infeksi dari hewan bernilai 0 (`InfectionSystem.md`). Hewan tidak menyerang bangunan (`BaseNavigation.md`).

**Keputusan teknis di blueprint ini:**

- Serangan khusus memakai `AnimalAttackKind`, bukan state baru, supaya daftar `AnimalState` tetap mengikuti `GLOSSARY.md`.
- Radius aggro hewan terpisah dari `AISensory`, karena hewan lebih sedikit dan perilakunya lebih sederhana.

**Usulan yang menunggu developer:**

1. **Cara Gagak terbang.** Rekomendasi: `NavMeshAgent` di tanah dengan `baseOffset` tinggi, supaya aturan `Claude.md` tetap terpenuhi.
2. **Hubungan hewan dan zombie saat fajar dan senja.** Rekomendasi: saling mengabaikan, supaya GDD tetap sederhana dan performa aman.
3. **Syarat menguliti.** Rekomendasi: `Base_Pisau` harus sedang dipegang, dan durabilitasnya tidak berkurang, karena Pisau adalah item inti.
4. **Semua angka spesies di bagian 1.** Rekomendasi: pakai nilai awal, lalu setel lewat playtest bersama `GameBalanceConfig`.

## 6. Skenario Tes

1. Dekati Serigala sampai 18 m, tanpa penghalang. **Hasil:** berhenti, menggeram selama 1,5 detik, lalu mengejar.
2. Ulangi sambil jongkok. **Hasil:** baru sadar di sekitar 10,8 m.
3. Lari sejauh lebih dari 35 m dari titik awal pengejaran. **Hasil:** Serigala kembali ke teritori, dan HP-nya naik.
4. Masuk base tertutup saat Serigala mengejar. **Hasil:** `TargetUnreachable`, mondar-mandir, lalu kembali setelah 15 detik. Dinding tidak diserang.
5. Tembak hewan dalam `TargetUnreachable`. **Hasil:** hewan langsung pergi.
6. Buka pintu saat hewan menunggu. **Hasil:** kembali ke `Chase`.
7. Majukan jam ke 18:00 saat hewan mengejar. **Hasil:** semua hewan lari ke `DenPoint` lalu kembali ke pool.
8. Berjalan di bawah Jaguar di Region 2. **Hasil:** ada tanda 0,8 detik, lalu terkaman. Beri 40 damage untuk membatalkan.
9. Digigit Ular. **Hasil:** `Venom` aktif, dan Infection tidak naik.
10. Hindari serudukan Babi ke arah samping, dengan batu di belakang. **Hasil:** babi menabrak batu dan pusing 2 detik.
11. Lawan 6 Gagak. **Hasil:** paling banyak 2 Gagak menukik sekaligus.
12. Tahan `E` pada bangkai sambil memegang Pistol. **Hasil:** ditolak dengan `MissingTool`. Ulangi dengan Pisau. **Hasil:** daging dan kulit masuk inventori, dan sisanya jatuh.
13. Pakai ulang hewan dari pool sebanyak 20 kali. **Hasil:** HP, `baseOffset`, dan state kembali ke nilai awal.
