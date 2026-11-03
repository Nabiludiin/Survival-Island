# Blueprint: SpecialZombieAI.cs & ZombieData.cs
**Lokasi:** `docs/blueprint/left-4-dead/SpecialZombieAI.md`
**Deskripsi Utama:** Data dan perilaku tiap varian zombie di GDD. `EnemyAI` tetap memegang kejar dan state dasar, sedangkan `SpecialZombieAI` menambah kemampuan khusus varian.
**Issue:** #25
**Rujukan GDD:** Bagian 3 (infeksi dan zombie terbakar) dan bagian 5 (jenis zombie per region). Pola L4D dan semua angka adalah usulan, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

### Data

```csharp
public class EnemyData : ScriptableObject
{
    public string enemyId;              // zombie_militer
    public string poolTag;              // Zombie_Militer
    public float maxHP;
    public float moveSpeed;
    public DamageResistance[] resistances;   // #59
    public AttackData primaryAttack;         // #109
    public ThreatProfile threatProfile;      // #108. Zombie: Threat_Zombie, hewan: Threat_Animal
    public bool canAttackBuildings;          // #78. true untuk ZombieData, false untuk AnimalData
    public bool isLarge;                     // Pengaman macet dan batas AIDirector
    public float poise;                      // #95
    public bool canBeStaggered;              // #95
    public bool canBeKnockedBack;            // #95
    public float knockbackMultiplier;        // #95
    public bool leavesCarcass;               // #95, dipakai #61
    public LootTable dropTable;              // #95, #103
    public float dropChance;                 // #95
    public bool immuneToTrapSlow;            // #26. Nilai awal false
    public int traversalMask;                // NavMesh area, #94
}

public class ZombieData : EnemyData
{
    public SpecialZombieKind kind;
    public AttackData secondaryAttack;       // Boleh kosong
    public bool hidesFromSun;                // #63. true hanya zombie_desa
    public float sightMultiplier = 1f;       // #63, dibaca AISensory
    public float hearingMultiplier = 1f;     // #63, dibaca AISensory
}

public enum SpecialZombieKind { None, TribalSnare, MossDisguise, MilitaryArmor, FrozenBurst, VillagePounce }
```

`AnimalData` (#61) mewarisi `EnemyData`. Angka statistik berada di aset `ZombieData`. Angka perilaku berada di `GameBalanceConfig.specialZombie`.

**Bangunan (#78):** hewan yang terhalang tidak menyerang bangunan, dan memakai `AnimalState.TargetUnreachable` (#61).

**Kebal lambat jebakan (#26):** `immuneToTrapSlow = true` diisi untuk varian besar, yaitu Zombie Militer. Tank bukan jenis musuh terpisah, melainkan pola pada Zombie Militer.

**Zombie Lumut (#63):** memakai `BushSpawnPoint` lewat `SpawnEntry.useBushPoints` di `SpawnManager.md`.

**Suara (#114):** prefab semua varian memasang komponen `ThreatAudioEmitter` dan aset `EnemyCueSet`.

### Statistik awal

| Varian | `enemyId` | Region | HP | Kecepatan | Damage serangan utama | Infeksi | `isLarge` |
|---|---|---|---|---|---|---|---|
| Standar | `zombie_standar` | 1, 2, 3, dan Fase 1 | 50 | 4 | 15 | `infection.gainStandard` 5 | Tidak |
| Tribal | `zombie_tribal` | 2 | 80 | 4 | 10 | `infection.gainTribal` 6 | Tidak |
| Lumut | `zombie_lumut` | 2 | 120 | 5,5 | 30 | `infection.gainMoss` 8 | Tidak |
| Militer | `zombie_militer` | 3 | 300 | 3 | 30 | `infection.gainMilitary` 10 | Ya |
| Beku | `zombie_beku` | 3 | 60 | 3,5 | 10 | `infection.gainFrozen` 5 | Tidak |
| Desa | `zombie_desa` | 4 | 60 | 7,5 | 12 | `infection.gainVillage` 6 | Tidak |

**Patokan:** pemain berjalan 5 m/s dan berlari 8 m/s. Hanya Zombie Desa yang mendekati kecepatan lari, sesuai "sangat cepat" di GDD. Dengan peluru 30 damage, zombie standar mati dengan 1 headshot atau 2 tembakan badan.

### Armor Zombie Militer

| Jenis | Bagian tubuh | Pengali armor | Catatan |
|---|---|---|---|
| `Bullet` | `Torso` | 0,35 | Rompi anti-peluru |
| `Bullet` | `Leg` | 0,5 | Sesudah pengali kaki 0,5 |
| `Bullet` | `Head` | 1 | Celah utama. 30 x 2 = 60, jadi 5 headshot |
| `Melee` | Semua | 0,8 | |
| `Explosion`, `Fire` | Semua | 1 | Cara lain melawan selain headshot |

Nilai ini sama dengan contoh di `DamagePipeline.md` dan masuk `ZombieData.resistances`.

### Serangan dan reaksi per varian

`EnemyHitReaction.md` dan `EnemyAttack.md` meminta #25 mengisi nilai varian lain.

| Varian | `primaryAttack` | `secondaryAttack` | `poise` | `canBeKnockedBack` | `knockbackMultiplier` |
|---|---|---|---|---|---|
| Standar | `Zombie_Standard_Hit` | Tidak ada | 25 | Ya | 1 |
| Tribal | `Zombie_Tribal_Hit` | `Zombie_Tribal_Snare` | 30 | Ya | 1 |
| Lumut | `Zombie_Lumut_Lunge` | Tidak ada | 45 | Ya | 0,8 |
| Militer | `Zombie_Heavy_Hit` | `Zombie_Rock_Throw` (#94) | 90 | Tidak | 0 |
| Beku | `Zombie_Beku_Hit` | Tidak ada | 25 | Ya | 1 |
| Desa | `Zombie_Desa_Pounce` | Tidak ada | 20 | Ya | 1,2 |

Aset `AttackData` baru mengikuti bentuk #109, dan memakai damage serta infeksi dari tabel statistik.

### Komponen

`SpecialZombieAI` adalah kelas dasar abstrak yang berada di prefab bersama `EnemyAI`, dan memakai `IPoolable`. Tiap varian punya satu turunan: `TribalBehavior`, `MossBehavior`, `MilitaryBehavior`, `FrozenBehavior`, dan `VillageBehavior`. Zombie standar tidak memakainya.

```csharp
public abstract class SpecialZombieAI : MonoBehaviour, IPoolable
{
    public abstract bool TickSpecial(float dt); // true berarti gerak pada tick ini diambil alih
    public virtual void OnSpawned() { }
    public virtual void OnReturned() { }
}
```

`EnemyAI.CrowdTick` (#93) memanggil `TickSpecial` lebih dulu. Kalau hasilnya `true`, logika `Chase` dilewati pada tick itu.

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.specialZombie.tribalRangeMin` / `tribalRangeMax` | 5 / 14 | Jarak melempar jerat, dalam meter |
| `GameBalanceConfig.specialZombie.tribalWindupSeconds` | 1,2 | Ancang-ancang |
| `GameBalanceConfig.specialZombie.tribalProjectileSpeed` | 18 | Jerat bisa dihindari |
| `GameBalanceConfig.specialZombie.tribalCooldownSeconds` | 10 | Jeda lemparan |
| `GameBalanceConfig.specialZombie.tribalPullSpeed` | 2 | Kecepatan menarik pemain |
| `GameBalanceConfig.specialZombie.tribalDamagePerSec` | 5 | Damage selama menahan |
| `GameBalanceConfig.specialZombie.tribalReleaseDamage` | 25 | Damage yang membuat jerat lepas |
| `GameBalanceConfig.specialZombie.tribalBreakDistance` | 18 | Jerat putus di atas jarak ini |
| `GameBalanceConfig.specialZombie.mossWakeDistance` | 4 | Bangun seketika |
| `GameBalanceConfig.specialZombie.mossAlertDistance` | 8 | Amarah naik di dalam jarak ini |
| `GameBalanceConfig.specialZombie.mossLightDistance` | 12 | Amarah naik kalau disorot senter |
| `GameBalanceConfig.specialZombie.mossRagePerSec` | 0,4 | Laju amarah, 2,5 detik sampai penuh |
| `GameBalanceConfig.specialZombie.mossRageDecayPerSec` | 0,2 | Amarah turun tanpa ancaman |
| `GameBalanceConfig.specialZombie.mossTellMinSeconds` / `mossTellMaxSeconds` | 6 / 10 | Jeda tanda samar |
| `GameBalanceConfig.specialZombie.militarySlowMultiplier` | 0,5 | Kecepatan saat ditembak dari dekat |
| `GameBalanceConfig.specialZombie.militarySlowFullDistance` / `militarySlowEndDistance` | 4 / 8 | Efek penuh, lalu melemah sampai hilang |
| `GameBalanceConfig.specialZombie.militarySlowSeconds` | 1 | Lama melambat tiap kena |
| `GameBalanceConfig.specialZombie.frozenBurstRadius` | 4 | Radius ledakan |
| `GameBalanceConfig.specialZombie.frozenBurstDamage` | 20 | Damage `Explosion` di tengah |
| `GameBalanceConfig.specialZombie.frozenBurstDelaySeconds` | 0,3 | Jeda sebelum meledak |
| `GameBalanceConfig.specialZombie.frozenBurstWarmthLoss` | 25 | `Warmth` yang hilang kalau kena |
| `GameBalanceConfig.specialZombie.villagePounceMin` / `villagePounceMax` | 3 / 10 | Jarak menerkam |
| `GameBalanceConfig.specialZombie.villagePounceWindupSeconds` | 0,5 | Pekikan sebelum menerkam |
| `GameBalanceConfig.specialZombie.villagePounceSpeed` | 16 | Kecepatan selama terkaman |
| `GameBalanceConfig.specialZombie.villagePounceRecoverySeconds` | 1,5 | Terhuyung, celah untuk membalas |
| `GameBalanceConfig.specialZombie.villagePounceCooldownSeconds` | 6 | Jeda terkaman |

## 2. Detail Logika & Fungsi

### Runner, Tank, dan Spitter

Mengikuti rekomendasi di bagian 5, ketiganya menjadi pola perilaku, bukan `enemyId` baru:

| Pola | Dipakai oleh | Alasan |
|---|---|---|
| Runner | Zombie Desa | GDD menyebut bergerak sangat cepat |
| Tank | Zombie Militer | Darah tebal dan armor |
| Spitter (jarak jauh) | Zombie Tribal | Satu-satunya varian dengan serangan jarak jauh |

### Zombie Tribal: jerat

1. **`TickSpecial`:** kalau jarak ke pemain ada di antara `tribalRangeMin` dan `tribalRangeMax`, jeda selesai, pemain terlihat (`PhysicsQuery.Raycast` dengan `PhysicsMasks.AIVision` tidak mengenai apa pun), dan `PlayerPinnedEvent.Last.isPinned == false`, mulai ancang-ancang.
2. **Windup:** agen berhenti, animasi mengayun, dan suara peringatan berbunyi lewat `ThreatAudioEmitter.PlayTell` (#114). Jerat mengarah ke posisi pemain saat windup berakhir, jadi pemain bisa menghindar ke samping.
3. **Lempar:** `SpawnFromPool("Tribal_Snare", ...)` di layer `Projectile`, bergerak lurus dengan `tribalProjectileSpeed`.
4. **Kena pemain:** `TribalBehavior` memakai `IPinAttacker`, lalu memanggil `PlayerPinnedState.Instance.TryPin(this)` (#110). Kalau ditolak, jerat kembali ke pool tanpa efek.
5. **Menahan:** pemain ditarik `tribalPullSpeed`, dan menerima `tribalDamagePerSec` serta infeksi dari `secondaryAttack`.

**Cara pemain lepas:**

- Memberi `tribalReleaseDamage` kepada Tribal, dihitung dari `EnemyHealth.Damaged` sejak jerat mengenai.
- Memakai aksi lepas di #110, termasuk Pisau.
- Membunuh Tribal, atau jaraknya melewati `tribalBreakDistance`.
- Garis antara Tribal dan pemain terhalang `PhysicsMasks.AIVision` lebih dari 1 detik.

Kalau Tribal yang melepas, panggil `PlayerPinnedState.Instance.ReleaseByAttacker(this, PinReleaseReason.AttackerDamaged)`. Kalau mati atau kembali ke pool, alasannya `AttackerGone`. Lepas lewat aksi pemain sampai ke `OnPinReleased`, lalu Tribal masuk jeda.

### Zombie Lumut: menyamar

**Muncul:** `SpawnManager` (#62) menaruhnya pada titik semak. State awal `AIState.Dormant`, `agent.isStopped = true`, dan tidak ikut mengejar.

**Pemicu bangun:** `rage` bernilai 0 sampai 1, dan zombie bangun saat `rage >= 1`.

| Kondisi | Akibat |
|---|---|
| Pemain dalam `mossWakeDistance` | `rage = 1` |
| Terkena damage | `rage = 1` |
| `NoiseEmittedEvent` mencakup posisinya | `rage = 1` |
| Pemain dalam `mossAlertDistance` dan berdiri | `rage += mossRagePerSec * dt` |
| Sorotan senter dalam `mossLightDistance`, `FlashlightController.IsPointInBeam(pos)` bernilai `true`, dan garis pandang bebas | `rage += mossRagePerSec * dt` |
| `LightSourceSystem.Instance.GetLightLevelAt(pos) >= light.mossStaticLightLevel` (0,5) (#98) | `rage += mossRagePerSec * dt` |
| Tidak ada ancaman | `rage -= mossRageDecayPerSec * dt` |

Pemain jongkok (#101) yang tidak menyorotkan senter tidak menaikkan `rage`, kecuali masuk `mossWakeDistance`. Pemain jadi bisa memilih menghindar.

**Tanda samar:** semak di tubuhnya tidak bergoyang saat angin, napas pelan terdengar dalam 6 meter, dan mata berkilat singkat setiap `mossTellMinSeconds` sampai `mossTellMaxSeconds`. Tandanya ada di aset prefab, tanpa ikon HUD. Napas tetap berupa aset prefab, bukan `ThreatAudioEmitter.PlayTell` (#114).

**Saat bangun:** animasi bangkit 0,6 detik, lalu `Chase` dengan `primaryAttack`. Zombie tidak bisa kembali menyamar.

### Zombie Militer: armor dan melambat

`MilitaryBehavior` mendengar `EnemyHealth.Damaged`. Untuk `DamageType.Bullet`, hitung `t = InverseLerp(militarySlowEndDistance, militarySlowFullDistance, jarak ke pemain)`, lalu kecepatan menjadi `moveSpeed * Lerp(1, militarySlowMultiplier, t)` selama `militarySlowSeconds`. Menembak dari dekat lebih efektif, tetapi lebih berbahaya. `canBeKnockedBack = false` (#95).

### Pengaman macet varian besar

Berlaku pada `isLarge == true`. Tidak ada bunuh diri seperti Tank di L4D. Musuh kembali ke pool tanpa `EnemyKilledEvent` dan tanpa drop.

| Kondisi | Akibat |
|---|---|
| Kecepatan di bawah 0,2 m/s saat `Chase` selama 2 detik, dan pemain tak terlihat 5 detik | Minta target baru ke `ThreatSystem` (#108) |
| Kecepatan di bawah 0,2 m/s selama 10 detik, dan pemain tak terlihat 15 detik | `SpawnManager.Instance.ReturnStuck(enemy)` |
| Pemain tak terlihat 60 detik | `SpawnManager.Instance.ReturnStuck(enemy)` |

Pemeriksaan tidak berjalan saat `AttackBuilding` atau di dalam kandang Lucky Machine, karena musuh memang boleh diam di sana. Pengembalian ditunda selama musuh berada di dalam frustum kamera, supaya tidak hilang di depan pemain. Angkanya `GameBalanceConfig.specialZombie.largeStuckRetargetSeconds` 2, `largeStuckUnseenRetargetSeconds` 5, `largeStuckReturnSeconds` 10, `largeStuckUnseenReturnSeconds` 15, dan `largeUnseenReturnSeconds` 60.

### Zombie Beku: ledakan

1. `FrozenBehavior` mendengar `EnemyHealth.Died`. Kalau `damageType == Fire`, tidak meledak, karena es sudah mencair.
2. Tunggu `frozenBurstDelaySeconds`, dengan tubuh berkilat sebagai tanda.
3. `PhysicsQuery.OverlapSphere(pos, frozenBurstRadius, PhysicsMasks.Explosion, buffer)`, lalu `CollectUniqueDamageables`. Kirim `DamageType.Explosion` dengan `byPlayer = false`, sesuai tabel friendly fire di #59.
4. Kalau pemain termasuk korban dan garis pandang bebas, panggil `WarmthSystem.Instance.ApplyColdBurst(frozenBurstWarmthLoss)`.
5. `NoiseEmitter.Emit(frozenBurstRadius, NoiseSourceKind.Structure)` dari komponen di prefab Zombie Beku (#49). Jenis `Structure` adalah usulan, karena `NoiseSourceKind` belum punya jenis ledakan. Mayat diteruskan ke #95.

`BurnToAshes` saat fajar dan masuk kandang tidak melewati `Died`, jadi tidak meledak.

### Zombie Desa: setelah naik ke darat

`SeekHiding`, `Submerged`, dan `Emerging` milik #63. Setelah `Emerging` selesai, `VillageBehavior` bekerja:

1. Kalau jarak ke pemain ada di antara `villagePounceMin` dan `villagePounceMax`, jeda selesai, dan `NavMesh.Raycast` ke pemain tidak terhalang, minta izin lewat `AttackCoordinator.TryAcquire` (#109).
2. **Windup:** berhenti, memekik, dan menghadap pemain.
3. **Terkaman:** `agent.speed = villagePounceSpeed` menuju titik pemain saat itu, paling lama 0,7 detik. Musuh tetap di NavMesh, tanpa fisika.
4. **Kena:** `primaryAttack` sekali, tanpa menahan pemain. Karena zombie ini datang massal, pinned akan membuat pemain mati tanpa peluang.
5. **Pemulihan:** diam selama `villagePounceRecoverySeconds`, lalu kecepatan kembali ke `moveSpeed`.

## 3. Hubungan dengan Sistem Lain

| Sistem | Hubungan |
|---|---|
| `EnemyAI` | Memanggil `TickSpecial`, dan tetap pemilik state |
| `EnemyHealth` (#59) | `Damaged` dan `Died`, serta `resistances` |
| `EnemyAttack`, `AttackCoordinator`, `AttackData` (#109) | Izin serangan dan data serangan |
| `PlayerPinnedState` (#110) | Menerima `TryPin` dan `ReleaseByAttacker` dari Tribal |
| `WarmthSystem` (#65) | Menerima `ApplyColdBurst` dari Beku |
| `SunlightExposure` (#63) | Pemilik aturan terbakar dan sembunyi |
| `SpawnManager` (#62) | Memilih varian per region dan menyediakan `ReturnStuck` |
| `AIDirector` (#27) | Mengatur jumlah varian khusus dan pengali |
| `AITraversal` (#94) | Membaca `traversalMask` |
| `LuckyMachine` (#72) | Kuota dihitung saat masuk kandang, dari `enemyId` |

**Event yang dikirim:** `NoiseEmittedEvent` dari ledakan Beku, lewat `NoiseEmitter`. **Event yang didengar:** `NoiseEmittedEvent` oleh Lumut, dan `PlayerPinnedEvent` oleh Tribal.

## 4. Kasus Khusus

- **Dipakai ulang dari pool:** `OnSpawned` mengatur `agent.speed = moveSpeed`, mengosongkan timer, `rage = 0`, status jerat, dan efek lambat. Lumut kembali ke `Dormant`. `OnReturned` memanggil `ReleaseByAttacker(this, AttackerGone)` kalau masih menahan pemain, dan mengembalikan jerat yang masih terbang.
- **Game dijeda:** semua timer memakai `dt` dari `CrowdTick`, jadi berhenti saat `Time.timeScale = 0`. Pemain tidak bisa bergerak, dan jerat juga berhenti.
- **Permadeath dan save:** tidak ada yang disimpan, sesuai `WorldStateSave.md`. Aset `ZombieData` tidak berubah. Reset memakai jalur pool biasa.
- **Region dilepas (#88):** semua varian kembali ke pool. Jerat aktif dilepas lewat `OnReturned`.
- **Fajar saat menahan pemain:** `BurnToAshes` menang, dan `OnReturned` melepas pemain.
- **Pemain membawa Heavy Item saat kena jerat:** diatur #110.
- **Beku meledak di dekat Beku lain:** rantai ledakan diizinkan. Tiap ledakan menunggu `frozenBurstDelaySeconds`, jadi tidak terjadi dalam satu frame.
- **Lumut di luar Region 2:** `SpawnManager` tidak memilihnya, sesuai GDD.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** headshot 2x (#85), armor Militer yang bisa ditembus di kepala (#59), infeksi per varian (#64), dan zombie tidak bisa mengenai zombie lewat `EnemyAttack` (#59).

**Usulan yang disetujui developer sebagai nilai awal (dicatat di #85):**

1. **Runner, Tank, dan Spitter:** rekomendasi digabung sebagai pola pada varian GDD, tanpa `enemyId` baru, karena GDD tidak menyebut ketiganya.
2. **Zombie Tribal:** rekomendasi jerat jarak 5 sampai 14 meter yang menarik dan menahan. Lepas lewat 25 damage, aksi #110, jarak, atau garis pandang terputus.
3. **Zombie Lumut:** rekomendasi `Dormant` dengan amarah bergaya Witch, dan tidak bisa kembali menyamar.
4. **Zombie Beku:** rekomendasi menurunkan `Warmth`, bukan melambatkan, karena pemain yang membawa barang sudah berkecepatan setengah, dan `Warmth` terhubung ke frostbite di GDD.
5. **Zombie Desa:** rekomendasi kecepatan 7,5 m/s dan terkaman tanpa pinned.
6. **Pengaman macet:** rekomendasi memakai ambang L4D dengan kembali ke pool, tanpa drop.
7. **Semua statistik:** nilai awal di bagian 1 menunggu playtest.

## 6. Skenario Tes

1. `spawn zombie_militer 1`. Tembak badan, kaki, dan kepala dengan peluru 30 damage. **Hasil:** damage sekitar 10,5, 7,5, dan 60.
2. Tembak Militer dari 3 meter, lalu dari 9 meter. **Hasil:** melambat 50 persen dari dekat, dan tanpa lambat dari jauh.
3. `spawn zombie_tribal 1` dari 10 meter. Geser ke samping saat windup. **Hasil:** jerat meleset.
4. Biarkan jerat kena, lalu tembak Tribal sampai 25 damage. **Hasil:** pemain lepas dan bisa bergerak.
5. Berdiri di belakang pohon saat ditarik. **Hasil:** jerat putus setelah sekitar 1 detik.
6. Jongkok melewati Lumut pada jarak 6 meter tanpa senter. **Hasil:** tidak bangun.
7. Sorot Lumut dari 10 meter. **Hasil:** bangun setelah sekitar 2,5 detik.
8. Bunuh Beku dengan pistol pada jarak 3 meter. **Hasil:** ledakan setelah 0,3 detik, HP turun, `Warmth` turun 25, dan pemain tidak melambat.
9. Bunuh Beku dengan api. **Hasil:** tidak meledak.
10. `spawn zombie_desa 1` dari 10 meter. **Hasil:** memekik, menerkam, lalu terhuyung 1,5 detik. Pemain tidak tertahan.
11. Kurung Militer di balik `Obstacle`, lalu menjauh tanpa melihatnya selama 15 detik. **Hasil:** ganti target, lalu kembali ke pool, tanpa `EnemyKilledEvent`.
12. Pakai ulang Lumut yang sudah bangun dari pool. **Hasil:** kembali `Dormant`, `rage = 0`.
13. Jeda game saat jerat terbang. **Hasil:** jerat berhenti, dan lanjut setelah jeda selesai.
