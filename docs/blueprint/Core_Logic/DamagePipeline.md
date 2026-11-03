# Blueprint: DamagePipeline (IDamageable, DamageInfo, EnemyHealth, BaseHealth)
**Lokasi:** `docs/blueprint/Core_Logic/DamagePipeline.md`
**Deskripsi Utama:** Satu jalur damage untuk pemain, musuh, NPC, dan bangunan. Semua sumber damage mengisi `DamageInfo`, lalu memanggil `IDamageable.TakeDamage`.
**Issue:** #59
**Rujukan GDD:** Bagian 3 (HP habis karena serangan, lapar, haus, atau infeksi, dan infeksi dari serangan zombie), bagian 4 (base sebagai tempat berlindung), dan bagian 5 (Zombie Militer berarmor anti-peluru). Aturan friendly fire dan angka armor adalah usulan, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

### Bentuk kode

```csharp
public interface IDamageable
{
    bool IsAlive { get; }
    void TakeDamage(in DamageInfo info);
}

public struct DamageInfo
{
    public float amount;          // Damage dasar, sudah termasuk pengali bagian tubuh setelah melewati router
    public DamageType type;
    public GameObject source;     // Boleh null untuk lingkungan. Hanya sah pada frame yang sama
    public string sourceId;       // EnemyData.enemyId atau itemId. Dipakai statistik dan layar akhir
    public Vector3 hitPoint;
    public Vector3 hitDirection;  // Arah gerak serangan, dari sumber ke target. #51 memakai kebalikannya, dan #95 memakai nilai ini langsung
    public BodyPart bodyPart;     // Nilai awal Torso untuk damage tanpa hitbox
    public float infection;       // Tambahan infeksi. 0 kalau bukan serangan zombie
    public bool byPlayer;         // Berasal dari pemain, untuk friendly fire dan statistik
}
```

`DamageInfo` adalah struct dan dikirim lewat `in`, jadi tidak menghasilkan garbage. `BodyPart` di `GLOSSARY.md` tidak punya nilai kosong, sehingga damage tanpa hitbox memakai `Torso`.

### Enum

`DamageType` mengikuti `GLOSSARY.md`: `Bullet`, `Melee`, `Fire`, `Explosion`, `Frost`, `Starvation`, `Dehydration`, `Infection`, `Structural`, `Fall`, `Drown`, dan `Poison`. Checklist issue menyebut delapan jenis. `Dehydration`, `Fall`, `Drown`, dan `Poison` berasal dari keputusan haus di #85 serta kebutuhan #13, #45, dan #101.

`DeathReason` mengikuti `GLOSSARY.md`, ditambah `None` sebagai nilai pertama (usulan #82). `None` hanya dipakai `RunEndedEvent` saat menang.

| Jenis | Pengirim umum | Melewati `HitboxModifier` | Mengenai bangunan |
|---|---|---|---|
| `Bullet` | `GunController` | Ya | Tidak |
| `Melee` | `MeleeCombatSystem`, `EnemyAttack` (#109) ke pemain | Ya, kalau mengenai `EnemyHitbox` | Tidak |
| `Fire` | Fireball, sinar matahari (#63), #97 | Tidak | Ya |
| `Explosion` | `EnvironmentalHazard`, Fireball, #96 | Tidak | Ya |
| `Frost` | `SurvivalStats` (#111) | Tidak | Tidak |
| `Starvation`, `Dehydration`, `Infection`, `Drown` | `SurvivalStats` (#111) | Tidak | Tidak |
| `Structural` | Serangan `AttackBuilding` (#109), gempa (#70), #90 | Tidak | Ya |
| `Fall` | `PlayerTraversal` (#101) | Tidak | Tidak |
| `Poison` | Ular (#61), #45 | Tidak | Tidak |

### Pemilik HP

| Target | Kelas | Sumber HP awal |
|---|---|---|
| Pemain | `SurvivalStats` | `MaxHP` 100 |
| Musuh | `EnemyHealth` | `EnemyData.maxHP` (#25, #61) |
| NPC | `NPCCompanion` (#76) | `NPCData.maxHP` |
| Bangunan | `BaseHealth` | `BuildableData.maxHP` (#79) |

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.damage.headMultiplier` | 2 | Headshot, keputusan #85 |
| `GameBalanceConfig.damage.torsoMultiplier` | 1 | Badan |
| `GameBalanceConfig.damage.legMultiplier` | 0,5 | Kaki, dari `HitboxModifier.md` |
| `GameBalanceConfig.damage.explosionMinFalloff` | 0,25 | Damage di tepi radius ledakan |
| `GameBalanceConfig.damage.playerExplosionSelfMultiplier` | 0,5 | Ledakan buatan pemain ke pemain dan NPC |
| `GameBalanceConfig.damage.minDamageAfterArmor` | 1 | Damage minimum setelah armor, supaya tembakan tidak pernah 0 |
| `GameBalanceConfig.damage.maxExplosionTargets` | 64 | Ukuran buffer `Collider` ledakan |

## 2. Detail Logika & Fungsi

### Mencari target lewat parent

Collider yang kena sering berupa kepala atau kaki, jadi `IDamageable` tidak boleh dicari di collider itu saja. Semua senjata memakai satu fungsi:

```csharp
public static class DamageRouter
{
    public static bool ApplyHit(in RaycastHit hit, DamageInfo info)
    {
        var target = hit.collider.GetComponentInParent<IDamageable>();
        if (target == null || !target.IsAlive) return false;
        info.hitPoint = hit.point;
        if (hit.collider.TryGetComponent(out HitboxModifier hitbox)) hitbox.Apply(ref info);
        target.TakeDamage(info);
        return true;
    }
}
```

**`HitboxModifier.Apply(ref DamageInfo info)`**: mengisi `info.bodyPart`, lalu mengalikan `info.amount` dengan pengali bagian tubuh, hanya kalau `info.type` adalah `Bullet` atau `Melee`. Fungsi lama `OnHit` dan referensi `mainHealthScript` dihapus.

Hasil yang dijamin: raycast dan melee selalu melewati `HitboxModifier`, dan pengali headshot tidak ditulis dua kali.

### Urutan hitung damage

1. Pengirim mengisi `amount` dari data senjata atau `AttackData`.
2. `DamageRouter` menerapkan pengali bagian tubuh, kalau ada hitbox.
3. `EnemyHealth` atau `BaseHealth` menerapkan armor dari data target.
4. `SurvivalStats` menerapkan `DifficultyProfile.enemyDamageMultiplier` untuk serangan musuh ke pemain.
5. HP berkurang, event terkirim, lalu kematian diperiksa.

Modifier senjata dari #113 dan #72 masuk di langkah 1, jadi urutan modifier tidak bercampur dengan armor.

### Armor

```csharp
[Serializable]
public struct DamageResistance
{
    public DamageType type;
    public BodyPart bodyPart;
    public bool anyBodyPart;
    public float multiplier;      // 1 berarti tanpa pengurangan
}
```

`EnemyData.resistances` dan `BuildableData.resistances` memakai struct ini. Fungsi bersama `DamageResistanceTable.Apply(resistances, in info)` mengembalikan damage setelah armor, dengan batas bawah `minDamageAfterArmor` untuk musuh. Daftar pendek ini dicari berurutan, tanpa `Dictionary`.

`DamageResistanceTable.GetMultiplier(resistances, DamageType, BodyPart)` mengembalikan pengali armor tanpa menghitung damage (#95).

**Contoh Zombie Militer (angka final di #25):**

| Jenis | Bagian tubuh | Pengali | Hasil dari 30 damage |
|---|---|---|---|
| `Bullet` | `Torso` | 0,35 | 10,5 |
| `Bullet` | `Leg` | 0,5 | 7,5, setelah pengali kaki 0,5 |
| `Bullet` | `Head` | 1 | 60, setelah pengali kepala 2 |
| `Melee` | Semua | 0,8 | 24 |
| `Explosion`, `Fire` | Semua | 1 | Tidak berubah |

Dengan begitu, GDD tetap terpenuhi karena badan tahan peluru, dan pemain punya celah yang bisa dipelajari.

### `EnemyHealth`

`EnemyHealth : MonoBehaviour, IDamageable, IPoolable` berada di objek utama musuh.

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `data` | `EnemyData` | Dari prefab | Sumber `maxHP` dan `resistances` |
| `hpMultiplier` | `float` | 1 | Diisi `SpawnManager` (#62) dan #27 setelah objek keluar dari pool |
| `CurrentHP` | `float` | `data.maxHP` | HP saat ini |
| `IsAlive` | `bool` | `true` | Penjaga kematian ganda |
| `Damaged`, `Died` | `event Action<DamageInfo>` | Kosong | Event per objek untuk komponen di prefab yang sama: `EnemyAI`, `EnemyHitReaction` (#95) |

`Damaged` dan `Died` bukan event statis dan tidak melintasi sistem, jadi tidak melanggar `GameEventBus`. Pendengarnya mengikuti aturan `OnEnable` dan `OnDisable`.

**`public void TakeDamage(in DamageInfo info)`**:

1. Kalau `!IsAlive`, abaikan.
2. Hitung `final = DamageResistanceTable.Apply(data.resistances, info)`.
3. `CurrentHP -= final`.
4. Hitung `isKill = CurrentHP <= 0`. Kalau `true`, `IsAlive = false` lebih dulu, supaya damage pada frame yang sama tidak membunuh dua kali.
5. Terbitkan `EnemyHitEvent { position = info.hitPoint, bodyPart, isKill, byPlayer }`.
6. Panggil `Damaged?.Invoke(info)`.
7. Kalau `isKill`, panggil `Died?.Invoke(info)`, lalu terbitkan `EnemyKilledEvent { enemyId = data.enemyId, position, damageType = info.type, byPlayer }`.

`EnemyHealth` tidak mengembalikan dirinya ke pool. Mayat dan ragdoll diatur #95, dan pengembalian dilakukan pemilik state di `EnemyAI`.

**`public void SetInvulnerable(bool value)`**: saat aktif, `TakeDamage` diabaikan (#72).

**`public void SetHPMultiplier(float value)`** mengatur `hpMultiplier`, lalu mengisi ulang `CurrentHP = data.maxHP * value`.

**`OnSpawned()`**: `IsAlive = true`, `CurrentHP = data.maxHP * hpMultiplier`, dan `SetInvulnerable(false)`. **`OnReturned()`**: `hpMultiplier = 1`, dan daftar `Damaged` serta `Died` tidak dihapus paksa, karena pendengarnya sudah lepas lewat `OnDisable`.

### `BaseHealth`

`BaseHealth : MonoBehaviour, IDamageable` berada di tiap prefab struktur, tanpa `ISaveable`. `BuildManager` tidak lagi menempelkannya saat bangun.

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `data` | `BuildableData` | Dari prefab | `maxHP` dan `resistances` |
| `CurrentHP` | `float` | `data.maxHP` | HP saat ini |
| `StructureId` | `string` | Dibuat `StructureRegistry.NewStructureId()` | ID stabil untuk #80 |
| `HpRatio` | `float` | 1 | `CurrentHP / data.maxHP` (#47) |
| `HpRatioChanged` | `event Action<float>` | Kosong | Event per objek untuk `StructureVisual` (#47) |
| `LastDamageTime` | `float` | 0 | Waktu damage terakhir (#47) |

**`public void Initialize(BuildableData data, string id, float hp)`** mengisi data, ID, dan HP saat dibangun atau dipulihkan (#47).

**Pengali bawaan bangunan:**

| Jenis | Pengali | Alasan |
|---|---|---|
| `Bullet` dan `Melee` | 0 | Pemain tidak bisa merusak dinding sendiri. Bongkar bangunan ada di #79 |
| `Explosion`, `Structural` | 1 | Serangan zombie, bencana, dan ledakan |
| `Fire` | Dari `BuildableData.resistances`: 1 untuk kayu, 0 untuk batu | Mengikuti material (#47) |

**`TakeDamage`**: kalikan damage dengan pengali bawaan dan pengali dari `SetDamageMultiplier`, kurangi HP, terbitkan `StructureDamagedEvent { structureId, hpRatio }`. Saat HP habis, terbitkan `StructureDestroyedEvent { structureId, position }`, lalu nonaktifkan dan hapus objek. Bangunan jarang dibuat dan dihancurkan, jadi tidak memakai `ObjectPooler`. Runtuhan beruntun diatur #79, dan **perbaikan diatur #47**, lewat `public void Repair(float amount)`.

**`public void SetDamageMultiplier(DamageType type, float multiplier)`** menyimpan pengali tambahan per jenis damage, dengan nilai awal 1. Dipakai `BaseDoor` saat `Locked` (#78).

**`public void Remove()`**, untuk bongkar bangunan (#79): terbitkan `StructureDestroyedEvent { structureId, position }`, lalu nonaktifkan dan hapus objek tanpa melewati `TakeDamage`.

Serangan zombie ke dinding memakai `DamageType.Structural`, sehingga pengali `Melee` tidak menghalanginya. Kerusakan dari #90 memanggil `TakeDamage` yang sama, karena fungsi pada objek nonaktif tetap bisa dipanggil (#88).

### Menerima damage di pemain

`SurvivalStats` menjadi `IDamageable`. Aturan pemulihan dan prioritas kondisi ada di `raft/VitalsRules.md` (#111).

**`public void TakeDamage(in DamageInfo info)`**, untuk damage dari luar:

1. Kalau `isDead`, abaikan.
2. Terapkan aturan friendly fire. Kalau hasilnya 0, abaikan.
3. Kalau serangan dari musuh dan `Time.time < PlayerPinnedState.graceUntil`, buang damage. Kalau serangan dari musuh selain `Attacker` saat `IsPinned`, kalikan dengan `pinned.otherAttackerDamageMultiplier` (#110). Damage lingkungan dan status tidak terpengaruh.
4. Kalau serangan dari musuh, kalikan dengan `DifficultyProfile.enemyDamageMultiplier`.
5. Panggil `ReduceHP(amount, info.type, info)`.
6. Kalau `info.infection > 0`, panggil `AddInfection(info.infection)`. Nilainya berasal dari `AttackData.infection` (#109), dan tabel per jenis musuh ada di #64.
7. Catat `lastDamageTime = Time.time`, untuk jeda pemulihan #111.
8. Terbitkan `PlayerDamagedEvent { info, remainingHP }`.

**`private void ReduceHP(float amount, DamageType type, in DamageInfo info)`** menyimpan `lastDamageType = type` setiap kali HP berkurang, lalu mengurangi HP. Kalau HP habis, panggil `TriggerDeath(DeathReasonMap.From(type))`.

Damage kondisi dari #111 memanggil `ReduceHP` lewat `ApplyVitalDamage`, tanpa `PlayerDamagedEvent`, supaya indikator arah (#51) tidak muncul setiap frame.

### Pencatatan `DeathReason`

`DeathReasonMap.From(DamageType)` memakai sumber damage terakhir:

| `DamageType` terakhir | `DeathReason` |
|---|---|
| `Bullet`, `Melee`, `Structural` | `KilledByEnemy` |
| `Fire` | `Fire` |
| `Explosion` | `Explosion` |
| `Frost` | `Frostbite` |
| `Starvation` | `Starvation` |
| `Dehydration` | `Dehydration` |
| `Infection` | `Zombified` |
| `Fall` | `Fall` |
| `Drown` | `Drowned` |
| `Poison` | `Poison` |

`Infection >= 100` tidak membunuh langsung. Kematian `Zombified` hanya terjadi saat HP habis dengan `DamageType.Infection` (GDD bagian 3, baris HP 5 di `VitalsRules.md`). `LeftOnShip` (#74) tidak melewati damage, tetapi langsung lewat `GameManager.TriggerGameOver`.

### Ledakan dan target unik

```csharp
public static class ExplosionDamage
{
    static readonly Collider[] buffer = new Collider[64];   // Mengikuti maxExplosionTargets
    static readonly List<IDamageable> unique = new List<IDamageable>(32);

    public static void Apply(Vector3 center, float radius, DamageInfo info)
    {
        int count = PhysicsQuery.OverlapSphere(center, radius, PhysicsMasks.Explosion, buffer);
        PhysicsQuery.CollectUniqueDamageables(buffer, count, unique);
        // Untuk tiap target: hitung falloff dari jarak ke titik collider terdekat, lalu TakeDamage
    }
}
```

- **Tanpa damage ganda:** `PhysicsMasks.Explosion` memuat kapsul `Enemy`, bukan `EnemyHitbox`, dan `CollectUniqueDamageables` tetap menyaring lewat parent.
- **Falloff:** `Mathf.Lerp(1, explosionMinFalloff, jarak / radius)`.
- **Bagian tubuh:** selalu `Torso`, tanpa pengali headshot.
- **Garis pandang:** ledakan tidak mengecek dinding. Dinding juga menerima damage.
- `EnvironmentalHazard`, Fireball (#70), dan #96 memakai fungsi ini.

### Friendly fire

| Sumber | Pemain | NPC | Bangunan | Musuh |
|---|---|---|---|---|
| Peluru dan melee pemain | Tidak mungkin | Tertahan, damage 0 | 0 | Penuh |
| Peluru NPC | Tertahan, damage 0 | Tertahan, damage 0 | 0 | Penuh, dengan `byPlayer = false` |
| Ledakan dan api buatan pemain (`byPlayer = true`) | Dikali `playerExplosionSelfMultiplier` | Dikali `playerExplosionSelfMultiplier` | Penuh | Penuh |
| Ledakan lingkungan, Fireball, dan Zombie Beku | Penuh | Penuh | Penuh | Penuh |
| Serangan musuh | Penuh | Penuh | `Structural` penuh | Tidak mengenai, karena mask `AIAttack` tidak memuat `Enemy` |

Target memeriksa aturan ini di `TakeDamage`, jadi senjata tidak perlu tahu jenis targetnya.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:**

| Event | Pengirim |
|---|---|
| `EnemyHitEvent`, `EnemyKilledEvent` | `EnemyHealth` |
| `StructureDamagedEvent`, `StructureDestroyedEvent` | `BaseHealth` |
| `PlayerDamagedEvent`, `PlayerDiedEvent` | `SurvivalStats` |

`EnemyKilledEvent` adalah bentuk resmi `OnEnemyKilled` dari checklist. Pendengarnya statistik (#57) dan Director (#27). Issue #72 tetap bisa membacanya, tetapi `GameEventBus.md` menetapkan bahwa kuota tumbal dihitung saat musuh masuk kandang.

**Event yang didengar:** tidak ada.

| Sistem | Hubungan |
|---|---|
| `HitboxModifier` | Dipanggil `DamageRouter`. Tidak lagi punya referensi HP |
| `GunController` (#58), `MeleeCombatSystem` | Memanggil `DamageRouter.ApplyHit`. `MeleeCombatSystem` memakai `PhysicsQuery.Raycast` dengan `PhysicsMasks.Melee` |
| `EnemyAttack` (#109) | Mengisi `DamageInfo` dari `AttackData`, termasuk `infection` |
| `EnemyHitReaction` (#95) | Mendengar `Damaged` dan `Died` |
| `InfectionSystem` (#64) | Mengisi nilai infeksi dan tahap |
| `VitalsRules` (#111) | Mengatur damage kondisi lewat `ApplyVitalDamage` |
| `PhysicsLayers` (#87) | Menyediakan `CollectUniqueDamageables` |
| `CombatFeedbackUI` (#51) | Membaca `EnemyHitEvent` dan `PlayerDamagedEvent` |

**Batas pekerjaan:** pengali per bagian tubuh tetap milik `HitboxModifier`, perbaikan dinding milik #47, dan indikator arah milik #51.

## 4. Kasus Khusus

- **Objek pool:** `OnSpawned` selalu mengisi ulang HP dan `IsAlive`. Musuh mati yang dipakai ulang tidak membawa `hpMultiplier` lama, karena `OnReturned` mengembalikannya ke 1.
- **Target kembali ke pool pada frame yang sama:** `DamageRouter` mengecek `IsAlive`. Peluru kedua pada musuh yang sudah mati tidak menghasilkan event kedua.
- **`source` sudah kembali ke pool:** `source` hanya boleh dipakai pada frame damage. Sistem yang perlu data lebih lama memakai `sourceId`.
- **Game dijeda:** jalur ini hanya berjalan saat dipanggil. Tidak ada damage baru saat waktu berhenti, karena sumbernya memakai `Time.deltaTime` atau input gameplay.
- **Permadeath:** HP musuh tidak disimpan. `CurrentHP` bangunan dan pemain ikut save run, sehingga terhapus bersama `WorldStateSave.DeleteRunSave()` (#82).
- **Save:** data struktur ditulis `StructureRegistry` ke `world_structures` lewat `StructureRecord`, bukan oleh `BaseHealth` (#79, `WorldStateSave.md`). Musuh tidak disimpan, sesuai #88.
- **Mati dan menang pada frame yang sama:** `GameManager.IsRunEnding` menahan panggilan kedua (#82).
- **Pemain mati saat damage beruntun:** `isDead` menahan `TriggerDeath` kedua, jadi `DeathReason` memakai damage yang pertama membunuh.
- **Damage 0 atau negatif:** diabaikan, dan pemulihan selalu lewat fungsi pemulihan.
- **Ledakan di region yang tidak dimuat:** tidak terjadi. Kerusakan bangunan di region itu lewat data (#70, #90).
- **Godmode (#56):** `SurvivalStats.SetInvulnerable(true)` menahan HP di 1. Event tetap terkirim.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** headshot 2x, serta nama `DamageType`, `DeathReason`, dan `BodyPart` di `GLOSSARY.md`. NPC menahan peluru di `PhysicsLayers.md`.

**Keputusan teknis di blueprint ini:**

- Semua senjata mencari target lewat `DamageRouter.ApplyHit`.
- Pengali armor dihitung di target, sesudah pengali bagian tubuh.
- Damage kondisi tidak mengirim `PlayerDamagedEvent`.
- Serangan ke dinding memakai `Structural`.

**Usulan yang menunggu developer (dicatat di #85):**

1. **Friendly fire:** peluru dan melee pemain tidak melukai NPC. Ledakan buatan pemain melukai pemain dan NPC sebesar 50 persen. Ledakan lingkungan melukai semua pihak.
2. **Armor Zombie Militer:** badan 0,35, kaki 0,5, kepala tanpa pengurangan, sehingga headshot menjadi cara utama melawannya.
3. **Pemain merusak bangunan:** peluru dan melee pemain tidak merusak bangunan sendiri.
4. **Falloff ledakan:** berkurang linier sampai 25 persen di tepi radius.
5. **`DeathReason.None`:** nilai baru dari #82 untuk hasil menang. Perlu ditambahkan ke `GLOSSARY.md`.

## 6. Skenario Tes

1. Tembak kepala, badan, dan kaki zombie standar dengan `Base_Pistol`. **Hasil:** HP berkurang 2x, 1x, dan 0,5x damage senjata.
2. Pukul kepala zombie dengan `Base_Pisau`. **Hasil:** pengali kepala berlaku, dan tidak ada error null.
3. Tembak badan dan kepala Zombie Militer. **Hasil:** badan menerima 35 persen damage, dan kepala menerima 2x damage.
4. Ledakkan tong di dekat zombie dengan tiga hitbox. **Hasil:** satu `EnemyHitEvent` dan satu pengurangan HP.
5. Bunuh zombie, lalu lihat log `eventlog 1`. **Hasil:** satu `EnemyKilledEvent` dengan `byPlayer = true`.
6. Tembak NPC. **Hasil:** peluru tertahan, tanpa damage ke NPC.
7. Ledakkan tong sejauh separuh radius dari pemain dengan `byPlayer = true`. **Hasil:** pemain menerima sekitar 31 persen damage dasar (falloff 0,625 dikali 0,5).
8. Biarkan zombie memukul pemain. **Hasil:** HP turun, `Infection` naik sebesar `AttackData.infection`, dan `PlayerDamagedEvent` terkirim sekali.
9. Mati karena lapar, lalu karena frostbite, jatuh, dan zombie, masing-masing di run berbeda. **Hasil:** `DeathReason` di layar akhir berturut-turut `Starvation`, `Frostbite`, `Fall`, dan `KilledByEnemy`.
10. Biarkan zombie memukul dinding sampai hancur. **Hasil:** `StructureDamagedEvent` terkirim tiap pukulan, lalu `StructureDestroyedEvent` sekali.
11. Tembak dinding sendiri. **Hasil:** HP dinding tidak berubah.
12. Spawn zombie yang sudah pernah mati dari pool. **Hasil:** HP penuh, `IsAlive` bernilai `true`, dan bisa dibunuh lagi.
13. Simpan setelah dinding rusak, keluar, lalu lanjutkan. **Hasil:** HP dinding sama. Setelah mati, run baru tidak memuat dinding itu.
14. Buka Profiler dan tembak 100 kali. **Hasil:** jalur `DamageRouter` tanpa alokasi GC selain `GetComponentInParent` yang tidak mengalokasi di build.
