# Blueprint: WildlifeAI.cs & AnimalData.cs
**Lokasi:** `Survival Island/doc/blueprint/left-4-dead/WildlifeAI.md`
**Deskripsi Utama:** AI hewan buas siang hari: teritorial, punya serangan khusus per spesies, kembali ke sarang saat malam, dan bangkainya bisa dikuliti.
**Issue:** #61
**Rujukan GDD:** Bagian 3 (ancaman siang, hewan menjaga teritori dan *resource*, hewan kembali ke sarang, dan berburu) dan bagian 5 (hewan per region). Angka dan bentuk serangan adalah usulan.

## 1. Arsitektur & Variabel Utama

`AIState` milik zombie tidak dipakai. Hewan punya `AnimalState` (`GLOSSARY.md`) dan FSM sendiri, yang berjalan lewat coroutine dengan jeda `GameBalanceConfig.wildlife.tickSeconds` (0,25).

**`IHitReactive` (#95):** `WildlifeAI` memakai interface yang sama dengan `EnemyAI`, supaya `EnemyHitReaction` tidak memanggil `EnemyAI` pada hewan.

| Fungsi | Isi pada hewan |
|---|---|
| `RequestReaction(Stagger)` / `RequestReaction(Knockback)` | Tolak kalau sedang `Dying` atau terkaman aktif. Kalau diterima, simpan `AnimalState` saat ini dan hentikan tick FSM. `State` tidak diganti, karena `AnimalState` tidak punya state reaksi |
| `RequestReaction(Dying)` | Selalu diterima. FSM berhenti tetap, sama dengan langkah `EnemyHealth.Died` di bawah |
| `EndReaction()` | Lanjutkan tick FSM dengan `AnimalState` yang disimpan |
| `SetSpeedMultiplier(source, m)` | Menyimpan pengali per `SpeedSource`, lalu memanggil ulang `ApplyStateSpeed(currentBaseSpeed)`. `agent.speed` hewan = kecepatan state (`wanderSpeed`, `chaseSpeed`, dan lainnya) x semua pengali sumber |

**`ApplyStateSpeed(float baseSpeed)`:** satu-satunya tempat `agent.speed` hewan ditulis. Fungsi ini menyimpan `currentBaseSpeed = baseSpeed`, lalu mengatur `agent.speed = baseSpeed x hasil kali semua pengali SpeedSource`. Semua `EnterX` di bawah memanggil fungsi ini, bukan menulis `agent.speed` langsung, supaya `LegHit` dan `Trap` tetap berlaku setelah state berganti.

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
| `State` | `AnimalState` | `Wander` | State saat ini. Bisa dibaca publik (#114) |
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
| `Alert` | Pemain terdeteksi | Berhenti, menghadap pemain, dan bersuara lewat `ThreatAudioEmitter.PlayTell` (#114) selama `alertSeconds` | `Chase`, atau `Wander` kalau pemain hilang |
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

**Menjaga resource (GDD bagian 3, disetujui developer):** kalau `e.sourceKind == NoiseSourceKind.Tool` (menebang, menambang, memetik, atau menguliti) dan sumbernya berada dalam `wanderRadius` dari `homePoint`, hewan langsung masuk `Chase`, bukan `Alert`. Pemain yang mengambil resource di dalam teritori dianggap mengganggu. Batas `leashDistance` tetap berlaku, jadi hewan tidak mengejar keluar teritori.

**Teritori di dekat resource (disetujui developer):** komponen scene `AnimalTerritory` diletakkan di Editor dekat kumpulan `ResourceNode`, misalnya jamur dan pohon getah Region 2 untuk Jaguar, atau area tambang Region 3 untuk Beruang Kutub. Isinya `enemyIds` (spesies yang boleh menempati) dan `radius`. `SpawnManager` memakai titik ini sebagai `homePoint` untuk hewan siang dengan peluang `wildlife.territorySpawnChance` (0,7). Kalau tidak ada `AnimalTerritory` yang cocok dalam jarak spawn, `FindSpawnPoint` biasa dipakai.

**`EnemyHealth.Damaged`:** kalau `info.byPlayer` dan bukan `FleeToDen`, langsung `Chase` kalau masih di dalam teritori. Hewan yang dilukai dari luar batas masuk `Alert`, lalu kembali ke teritorinya, sesuai #108.

Target dipilih lewat `ThreatSystem.Evaluate` dengan profil `Threat_Animal` (#108), yang mengisi `WildlifeAI.CurrentTarget` lewat `SetTarget`. `senseRange` berasal dari `aggroRadius`, dan `ThreatSystem.IsWithinTerritory` dipanggil lebih dulu.

### Teritori

**`EnterChase()`** menyimpan `chaseStartPoint = transform.position`, dan memanggil `ApplyStateSpeed(chaseSpeed)`.

**`TickChase()`:**

1. Kalau `Distance(transform, chaseStartPoint) > leashDistance`, masuk `ReturnToTerritory`.
2. Petakan target: `NavMesh.SamplePosition(target, out hit, targetSampleRadius, agent.areaMask)`, lalu `SetDestination(hit.position)`.
3. Setiap `pathCheckSeconds`, jalankan `NavMesh.CalculatePath` dengan `NavMeshPath` milik objek. Kalau bukan `PathComplete` selama `unreachableConfirmSeconds`, masuk `TargetUnreachable`.
4. Kalau masuk jarak `attack.range`, masuk `Attack`.

**`EnterReturn()`:** `ApplyStateSpeed(wanderSpeed * 1,5)`, `reaggroUntil = Time.time + reaggroCooldownSeconds`. Selama state ini, HP naik `returnHealPerSec` per detik, supaya pemain tidak bisa memancing lalu mencicil HP dari batas teritori.

### Target tak terjangkau

Misalnya pemain di dalam base yang tertutup. Hewan tidak menyerang bangunan (`canAttackBuildings = false`, #78).

1. Bergerak ke ujung jalur parsial, lalu mondar-mandir sambil bersuara, supaya pemain tahu hewan masih ada.
2. Terus jalankan `CalculatePath`. Kalau menjadi `PathComplete`, misalnya pintu terbuka, kembali ke `Chase`.
3. Setelah `unreachableGiveUpSeconds`, masuk `ReturnToTerritory`.
4. **Anti-eksploit:** kalau kena damage di state ini, langsung `ReturnToTerritory` dengan `ApplyStateSpeed(chaseSpeed)`, dan `reaggroCooldownSeconds` dimulai. Pemain tidak bisa menembak hewan yang tidak berdaya sampai mati dengan aman.

### Malam hari

**`HandleNightfall(NightfallEvent e)`** berlaku untuk semua hewan, kecuali yang sudah mati:

1. Batalkan serangan khusus, panggil `PlayerPinnedState.ReleaseByAttacker` kalau sedang menahan, dan lepaskan `AmbushPerch`.
2. `State = FleeToDen`, `ApplyStateSpeed(chaseSpeed * fleeSpeedMultiplier)`.
3. Tujuan: `DenPoint.FindNearest(posisi)`, yaitu komponen di scene region. Kalau tidak ada, cari titik acak menjauhi pemain.
4. Saat tiba dalam 2 m, panggil `SpawnManager.Instance.Despawn(gameObject, DespawnReason.ReturnedToDen)`.
5. Setelah `fleeTimeoutSeconds`, panggil hal yang sama saat hewan di luar pandangan kamera.

Dalam `OnEnable`, kalau `DayNightDirector.IsDay == false`, hewan langsung `FleeToDen`.

### Serangan khusus

Damage selalu lewat `IDamageable.TakeDamage(in DamageInfo)`, dengan `infection = 0` (#64). Waktu ayunan dan tanda serangan mengikuti `EnemyAttack` (#109).

**Terkaman Jaguar (`Pounce`):**

- `AmbushPerch` adalah penanda di pohon Region 2, dengan satu penghuni.
- Saat `Wander`, Jaguar menuju perch terdekat, `agent.enabled = false`, lalu naik ke cabang lewat animasi.
- Saat pemain berada dalam `pounceTriggerRadius` (9 m) dan tidak terhalang, beri tanda selama `pounceTelegraphSeconds` (0,8), berupa geraman lewat `ThreatAudioEmitter.PlayTell` (#114) dan daun bergoyang.
- Lompat lewat kurva parabola selama `pounceSeconds` (0,6) menuju posisi pemain saat tanda selesai. Mendarat dengan `NavMesh.SamplePosition` dan `agent.Warp`. Kalau gagal, panggil `Despawn` dengan `OffNavMesh`.
- Kena pemain dalam radius 1,5 m: damage 30, lalu `PlayerPinnedState.TryPin(this)` (#110). `WildlifeAI` memakai `IPinAttacker`. Kalau `TryPin` ditolak, misalnya `pinned.enabled = false`, Jaguar hanya mundur dan jeda ulang berjalan. Selama menahan, damage 5 setiap 0,5 detik.
- Terkaman batal saat Jaguar menerima `pounceBreakDamage` (40) dari pemain saja (`info.byPlayer == true`). Damage dari NPC, hewan lain, api, atau bencana tidak dihitung, karena pemain harus melepaskan diri sendiri (GDD bagian 3, keputusan developer). Polanya dari Hunter di `Survival Island/doc/REFERENSI_MUSUH_L4D.md`.
- Meleset: diam selama `pounceMissStunSeconds` (1,5), sebagai celah untuk pemain. Jeda ulang `pounceCooldownSeconds` (12).
- Saat `Chase`, Jaguar juga boleh menerkam dari tanah pada jarak 6 m, memakai jalur yang sama.

**Racun Ular (`VenomBite`):** gigitan dengan `DamageType.Poison`, jarak 1,5 m, jeda 3 detik. `DiseaseManager` (#45) membaca jenis damage ini dan memberi `Venom`, jadi `WildlifeAI` tidak mengenal `DiseaseManager`.

**Serudukan Babi (`Charge`):**

1. Dalam jarak 12 m, beri tanda menggaruk tanah selama `chargeTelegraphSeconds` (1).
2. Kunci titik tujuan pada garis lurus sejauh `chargeDistance` (12 m), lalu `ApplyStateSpeed(chargeSpeed)` (10). Tujuan tidak berubah lagi, jadi pemain bisa menghindar ke samping.
3. Setiap tick, `PhysicsQuery.OverlapSphere` dengan `PhysicsMasks.AIAttack` dan buffer tetap. Kena pemain: damage 18 berjenis `Melee`, satu kali per serudukan. Tidak ada pemanggilan getaran terpisah. Getaran datang otomatis dari `PlayerDamagedEvent`, sebagai `PlayerHit` (#24).
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

Bangkai yang tidak dikuliti hilang setelah `carcassLifetimeSeconds`, tetap ada saat malam, dan tidak menghitung batas hewan hidup. `EnemyInstance.IsCarcass` bernilai `true` sejak hewan mati (`EnemyHitReaction.md`), sehingga pengaman macet dan `SweepWrongPhase` di `SpawnManager` melewatinya. Saat umurnya habis, `AnimalCarcass` memudarkan bangkai lalu memanggil `SpawnManager.Instance.Despawn(gameObject, DespawnReason.Killed)`. Bangkai tetap ikut dikembalikan saat region dilepas.

### Hewan dan zombie

Saat fajar dan senja keduanya bisa berpapasan. Mereka saling mengabaikan: tidak saling menargetkan, tidak saling memberi damage, dan hanya saling menghindar lewat agen. Lihat bagian 5.

### Api dan suara

- **Api (#97):** tick AI hewan memanggil `FireSystem.Instance.IsNearFire(pos, margin)`, supaya hewan tidak berjalan ke `FireArea`. Prefab hewan memasang `Flammable` dengan profil `Flame_Animal`.
- **Suara (#114):** prefab semua hewan memasang komponen `ThreatAudioEmitter` dan aset `EnemyCueSet`.

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
| `FireSystem`, `Flammable` (#97) | `IsNearFire` dan profil `Flame_Animal` |
| `ThreatAudioEmitter` (#114) | Suara tanda, memakai `EnemyCueSet` |
| `AITraversal` (#94) | `TraversalAgent` untuk link. Respons target tak terjangkau tetap milik blueprint ini |

## 4. Kasus Khusus

- **Objek pool:** `OnSpawned()` mengatur `State = Wander`, `CurrentHP` lewat `EnemyHealth`, `agent.enabled = true` lewat `Warp`, semua pengali `SpeedSource` kembali ke 1, lalu `ApplyStateSpeed(wanderSpeed)`, tick FSM tidak dijeda reaksi, dan `agent.baseOffset` ke nilai prefab. `IsCarcass` dikosongkan oleh `EnemyInstance.OnSpawned`. `AnimalCarcass` dimatikan. Status jeda, `groupId`, perch, serta token Gagak dikosongkan di `OnReturned()`.
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

**Usulan yang disetujui developer sebagai nilai awal:**

1. **Cara Gagak terbang.** Rekomendasi: `NavMeshAgent` di tanah dengan `baseOffset` tinggi, supaya aturan `Claude.md` tetap terpenuhi.
2. **Hubungan hewan dan zombie saat fajar dan senja.** Rekomendasi: saling mengabaikan, supaya GDD tetap sederhana dan performa aman.
3. **Syarat menguliti.** Rekomendasi: `Base_Pisau` harus sedang dipegang, dan durabilitasnya tidak berkurang, karena Pisau adalah item inti.
4. **Semua angka spesies di bagian 1.** Rekomendasi: pakai nilai awal, lalu setel lewat playtest bersama `GameBalanceConfig`.
5. **Menjaga resource:** suara alat di dalam teritori langsung memicu `Chase`, dan teritori hewan diletakkan dekat resource lewat `AnimalTerritory` dengan peluang 0,7. Disetujui developer untuk memenuhi GDD bagian 3.

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
14. Saat diterkam Jaguar, beri 50 damage ke Jaguar lewat `DevConsole` dengan `byPlayer = false`. **Hasil:** terkaman tidak batal. Ulangi dengan tembakan Pistol pemain sebesar 40. **Hasil:** terkaman batal.
15. Tebang pohon di dalam `wanderRadius` serigala yang sedang `Wander`. **Hasil:** serigala langsung `Chase`. Ulangi di luar teritori. **Hasil:** hanya `Alert`.
16. Pasang `AnimalTerritory` Jaguar di dekat jamur Region 2, lalu tunggu spawn siang berulang. **Hasil:** sekitar 70 persen Jaguar muncul dengan `homePoint` di titik itu.
17. Tembak kaki Babi Hutan yang sedang `Chase`. **Hasil:** kecepatan `chaseSpeed` x `legSlowMultiplier`. Terhuyung menjeda FSM, lalu Babi kembali `Chase`. Picu serudukan sebelum efek kaki habis. **Hasil:** kecepatan serudukan `chargeSpeed` x `legSlowMultiplier`, karena `ApplyStateSpeed` tetap memakai pengali `LegHit`.
18. Bunuh Serigala siang hari, lalu majukan jam ke 18:00 tanpa melihat bangkainya. **Hasil:** bangkai tetap ada sampai `carcassLifetimeSeconds`, lalu kembali ke pool dengan `Killed`.

## 7. Catatan Implementasi (Langkah 5)

Kode: `left-4-dead/WildlifeAI.cs` (`AnimalState`), `AnimalData.cs` (`AnimalAttackKind`, `HarvestEntry`), `AnimalCarcass.cs`.

- **Belum ada:** `AmbushPerch` (Jaguar hanya menerkam dari tanah), Gagak terbang dan `CrowFlock` (`FlockDive` memakai gigitan biasa), `FireSystem.IsNearFire` (#97), dan `ThreatAudioEmitter` (#114).
- Malam dibaca dari `HourChangedEvent`. Gigitan dan racun memakai `EnemyAttack` dengan `AttackData.damageType`.
- `AnimalCarcass` memakai prompt teks langsung (`Kuliti`) sampai `LocalizationManager` (#104) ada.

## Catatan Implementasi (Sistem pendukung)

- Isi TODO (sistem pendukung): Suara tanda (#114): masuk `Alert` dan awal serangan khusus (terkam/seruduk) memanggil `ThreatAudioEmitter.PlayAlertTell()`, yaitu `PlayTell` dengan klip acak `EnemyCueSet.alertClips`. Tafsiran: klip diputar sekali saat masuk state, bukan diulang selama `alertSeconds`. (Diganti keputusan developer 7 November 2026, lihat di bawah.)
- **Keputusan developer (7 November 2026):** Suara tanda hewan diulang selama state `Alert` berlangsung, dengan jeda `wildlife.alertCueRepeatSeconds` (2 detik), lewat `ThreatAudioEmitter.PlayAlertTell()`. Ini menggantikan tafsiran "diputar sekali saat masuk state". Suara pertama tetap saat masuk `Alert`. Dengan `alertSeconds` 1,5 detik, nilai awal ini hanya memutar satu suara kecuali `Alert` diperpanjang.
- **Keputusan developer (7 November 2026):** `AmbushPerch` (#61) dan Gagak terbang (`CrowFlock`, #61) ditulis sekarang. Kode: `left-4-dead/AmbushPerch.cs`, `left-4-dead/CrowFlock.cs`, dan perubahan di `WildlifeAI.cs`. Angka memakai `GameBalanceConfig.wildlife` yang sudah ada (`pounceTriggerRadius`, `pounceTelegraphSeconds`, `pounceSeconds`, `pounceHitRadius`, `pounceMissStunSeconds`, `pounceCooldownSeconds`, `crowCruiseHeight`, `crowDiveHeight`, `crowDiveSeconds`, `maxDiversAtOnce`). Tidak ada field baru.
- **Perch:** `AmbushPerch` punya `basePoint` (kaki pohon di NavMesh), `perchPoint` (posisi di cabang), dan `leafShake` (daun bergoyang, boleh kosong). **Tafsiran:** saat `Wander` (ketika memilih tujuan keliling baru dan jeda terkaman sudah habis), Jaguar memilih perch kosong terdekat **di dalam `wanderRadius` dari `homePoint`**, supaya tidak meninggalkan teritori. Batal menuju perch (misalnya masuk `Alert`) melepas tempatnya.
- **Naik:** tiba di kaki pohon, `agent.enabled = false`, posisi langsung dipindah ke `perchPoint`, dan trigger animator `Climb` dipanggil. **Tafsiran:** gerak naik diserahkan ke klip animasi; `Climb` belum ada di `AnimIds` (`AnimationController.md`), jadi di-hash di `WildlifeAI`.
- **Di perch:** tick FSM khusus (agen mati). Pemain dalam `pounceTriggerRadius` dan garis pandang bebas: tanda `pounceTelegraphSeconds` (`ThreatAudioEmitter.PlayAlertTell` dan `AmbushPerch.PlayTell`), lalu perch dilepas dan Jaguar melompat ke posisi pemain saat tanda selesai, lewat kurva Bezier kuadrat (titik kontrol di tengah mendatar setinggi cabang) selama `pounceSeconds`. Mendarat: `NavMesh.SamplePosition` (`targetSampleRadius`) dan `agent.Warp`; gagal = `Despawn(OffNavMesh)`. Kena dalam `pounceHitRadius`: jalur yang sama dengan terkaman tanah (`TryPounceHit`, dipisah dari `TickPounce`). Meleset: diam `pounceMissStunSeconds`, lalu jeda `pounceCooldownSeconds`.
- **Tafsiran turun tanpa menerkam:** saat malam (`FleeToDen`), saat dilukai pemain di perch, atau saat perch dimatikan, Jaguar langsung dipindah ke kaki pohon di NavMesh (`DropFromPerch`), lalu melanjutkan perilaku biasa.
- `WildlifeAI.IsOnPerch` (publik) dibaca pengaman macet `SpawnManager`.
- **Gagak:** `OnSpawned` mengembalikan `agent.baseOffset` ke nilai prefab, lalu Gagak memakai `crowCruiseHeight`. Saat `Chase`, Gagak hanya menyerang lewat tukikan: kalau jarak mendatar ke target `<= primaryAttack.range` dan `CrowFlock.TryAcquire(groupId, ...)` berhasil (paling banyak `maxDiversAtOnce` per `groupId`), `baseOffset` turun ke `crowDiveHeight` dalam `crowDiveSeconds`, mematuk lewat `EnemyAttack.TryStartAttack`, lalu naik kembali. **Tafsiran:** naik kembali memakai durasi `crowDiveSeconds` yang sama. Token dilepas saat naik selesai, mati, `FleeToDen`, dan `OnReturned`.
- "Melee hanya kena saat Gagak menukik" tidak punya kode khusus: hitbox ikut `baseOffset`, jadi jangkauan melee pemain hanya sampai saat Gagak rendah. Peluru tetap mengenai.
- **Keputusan developer (7 November 2026, putaran ketiga):** Jaguar hanya memilih perch di dalam `wanderRadius` dari `homePoint`. Naik ke cabang langsung dipindah posisinya, lalu gerak naiknya diserahkan ke animasi `Climb` (`AnimIds.Climb`, resmi). Saat malam, dilukai pemain di perch, atau perch hilang, Jaguar langsung dipindah ke kaki pohon.
- **Keputusan developer (7 November 2026, putaran ketiga):** Gagak naik kembali memakai durasi yang sama dengan menukik (`crowDiveSeconds`).
