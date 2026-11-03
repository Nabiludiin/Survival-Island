# Blueprint: EnemyAI.cs
**Lokasi:** `docs/blueprint/left-4-dead/EnemyAI.md`
**Deskripsi Utama:** Otak `NavMeshAgent` untuk zombie. Mengatur state, tujuan gerak, dan perpindahan ke `AttackBuilding` saat jalur menuju target terhalang base. Damage, pemilihan target, dan indra berada di sistem masing-masing.
**Issue:** #25, #26, #49, #59, #62, #63, #67, #75, #78, #88, #93, #94, #95, #97, #108, #109, #112
**Rujukan GDD:** Bagian 3 (zombie terbakar di luar ruangan saat siang, sebagian diam di ruang gelap lalu bangun kalau terganggu, serta horde memburu MC dan menyerang base) dan bagian 5 (jenis zombie per region). Bentuk FSM dan prioritas state adalah usulan teknis, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

`EnemyAI : MonoBehaviour, IPoolable, ICrowdAgent` berada di prefab zombie, bersama `EnemyHealth`, `EnemyAttack`, `EnemyHitReaction`, `AISensory`, `EnemyInstance`, dan `TraversalAgent`. Hewan memakai `WildlifeAI`, bukan kelas ini.

```csharp
public enum AIState { Idle, Chase, AttackPlayer, AttackBuilding, BurnToAshes, Stagger, Knockback, SeekHiding, Submerged, Emerging, Dormant, Dying }
```

Daftar resminya ada di `GLOSSARY.md`.

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `currentState` | `AIState` | `Idle` | State saat ini |
| `agent` | `NavMeshAgent` | Dari prefab | Semua gerak lewat agen. `autoTraverseOffMeshLink = false` (#94) |
| `data` | `EnemyData` | Dari prefab | `moveSpeed`, `canAttackBuildings`, `traversalMask`, dan lainnya (`SpecialZombieAI.md`) |
| `CurrentTarget` | `ITargetable` | `null` | Target gerak, diisi `SetTarget` (#108). Pengganti `playerTransform` |
| `threat` | `ThreatState` | Kosong | Data `ThreatSystem` per musuh (#108) |
| `currentBreach` | `BreachTarget` | `default` | Dinding atau pintu yang diserang (#78) |
| `blockedTimer` | `float` | 0 | Lama jalur terhalang (#78) |
| `spawnGraceTimer` | `float` | `spawnGraceSeconds` | Pemeriksaan terhalang ditahan setelah muncul (#78) |
| `speedModifiers` | Satu nilai per `SpeedSource` | Kosong | Pengali kecepatan dan waktunya. Termasuk `LegHit` dan `Trap` |
| `waveModifiers` | `WaveModifiers` | Semua 1 | Dari `ApplyWaveModifiers` (#27) |
| `lair` | `ZombieLair` | `null` | Sarang pemilik, untuk zombie `Dormant` (#112) |
| `IsActiveThreat` | `bool` | - | Properti, `false` saat `Dormant`, `Submerged`, dan `BurnToAshes` |
| `IsBurning` | `bool` | `false` | Diisi `EnterBurn`. `Died` diabaikan saat `true` |

Field lama `attackRange`, `damage`, `playerLayer`, `baseStructureLayer`, dan `pathUpdateDelay` dihapus. `burnEffect` pindah ke `SunlightExposure`.

Angka `spawnGraceSeconds`, `pathCheckIntervalSeconds`, dan `blockedConfirmSeconds` berada di `BaseNavigation.md`. Interval tick berada di `HordeCrowdManager.md`.

## 2. Detail Logika & Fungsi

### Tick

**`public void CrowdTick(float dt)`** dipanggil `HordeCrowdManager` (#93), menggantikan coroutine `AIBehaviorLoop`:

1. Keluar kalau state `Dying`, `BurnToAshes`, `Submerged`, `Stagger`, atau `Knockback`.
2. `TraversalAgent.Tick(dt)` (#94).
3. `SpecialZombieAI.TickSpecial(dt)` (#25). Kalau hasilnya `true`, logika `Chase` dilewati pada tick ini.
4. Jalankan logika state.

Pembaruan tujuan selalu lewat `HordeCrowdManager.TrySetDestination`, jadi tidak ada `SetDestination` per tick.

### Indra dan target

Hanya berlaku di Fase 2. `EnemyAI` membaca `AISensory.Level` (#49):

| `Level` | State | Tujuan |
|---|---|---|
| `Unaware` pada malam hari | `Idle` | `AISensoryHub.HuntHint` |
| `Suspicious` | `Idle` | `AISensory.LastStimulusPosition` |
| `Aware` | `Chase` | `CurrentTarget` dari `ThreatSystem` |

`EnemyInstance.IsEngaged` bernilai `true` saat `Chase` dan saat menyerang.

**`public void SetTarget(ITargetable target)`** mengisi `CurrentTarget`. Pemanggilnya hanya `ThreatSystem`, kecuali di Fase 1 (`ShipHazardManager`).

`EnemyHealth.Damaged` yang berasal dari pemain atau NPC memanggil `ThreatSystem.ReportAttacker`.

### Chase

1. Baca `AITraversal.Instance.GetResponse(this)` untuk `Surround`, `Retreat`, `RockThrow`, dan `AttackStructure` (#94).
2. Kalau tidak ada respons khusus, tuju `CurrentTarget.Position`.
3. Kalau target dalam jangkauan `data.primaryAttack.range`, masuk `AttackPlayer`.
4. Jalankan pemeriksaan terhalang.

### Pemeriksaan terhalang

Mengikuti `BaseNavigation.md` bagian "Syarat baru pindah ke `AttackBuilding`". Syarat lama (`velocity < 0.1` dan `Physics.Raycast`) dihapus.

1. Berlaku saat `Chase`, `data.canAttackBuildings`, dan `spawnGraceTimer` habis.
2. Masuk `pathCheckQueue` milik `BaseNavigation`.
3. `PathPartial` atau `PathInvalid` menambah `blockedTimer`. `PathComplete` menolkannya.
4. Saat `blockedTimer >= blockedConfirmSeconds`, panggil `BaseNavigation.TryAssignBreachTarget(this, out currentBreach)`.
5. Kalau berhasil, masuk `AttackBuilding`. Kalau gagal, ikuti `AITraversal.GetResponse`.

### Serangan

`EnemyAI` tidak menghitung damage, dan tidak menjalankan overlap (#59, #109).

| State | Panggilan |
|---|---|
| `AttackPlayer` | `EnemyAttack.TryStartAttack(CurrentTarget)` |
| `AttackBuilding` | `EnemyAttack.TryStartStructureAttack(currentBreach.target)` |

`OverlapSphere` dengan `HordeCrowdManager.Instance.AttackBuffer` dijalankan di `EnemyAttack.DoHitCheck`. `CancelAttack` dipanggil saat mati dan saat `BurnToAshes`.

### Prioritas state

**`public bool RequestState(AIState next)`** memakai urutan `BurnToAshes`, `Dying`, `Knockback`, `Stagger`, lalu lainnya (#95). Permintaan yang lebih rendah ditolak.

### Kecepatan

- **`public void SetSpeedMultiplier(SpeedSource source, float multiplier)`** dipakai `EnemyHitReaction` dengan `SpeedSource.LegHit`.
- **`public void ApplySpeedModifier(SpeedSource id, float multiplier, float seconds)`** dipakai `BaseDefense` dengan `SpeedSource.Trap`. Tiap sumber hanya menyimpan satu nilai, dan waktunya diperbarui.
- **`public void ApplyWaveModifiers(in WaveModifiers m)`** dipanggil `SpawnManager` setelah keluar dari pool.
- `agent.speed` = `data.moveSpeed` x pengali gelombang x semua pengali sumber.

### Terbakar saat fajar

Langganan `DaybreakEvent` dan `TriggerDaybreakDeath` dihapus. Keputusan terbakar milik `SunlightExposure` (#63). Zombie dengan `lair != null` di dalam `InteriorZone` tidak terbakar, baik `Dormant` maupun sudah bangun (#112).

**`public void EnterBurn()`**, dipanggil `SunlightExposure` sebelum damage `Fire`:

1. `IsBurning = true`, `currentState = AIState.BurnToAshes`.
2. Hentikan FSM dan `EnemyAttack.CancelAttack()`.
3. `agent.enabled = false`. Aman dipanggil saat agen sudah mati.
4. Lepaskan `PlayerPinnedState` kalau sedang menahan pemain.

`EnemyAI` mengabaikan `EnemyHealth.Died` saat `IsBurning`.

### Mati dan kembali ke pool

| Penyebab | Jalur |
|---|---|
| Mati biasa | `EnemyHealth.Died`, lalu `EnemyHitReaction.corpseRoutine` (#95), lalu `SpawnManager.Instance.Despawn(gameObject, DespawnReason.Killed)` |
| Terbakar | `SunlightExposure` menyelesaikan urutan, lalu `Despawn` dengan `DespawnReason.Burned` |
| Fase 2 lainnya | `SpawnManager.Instance.Despawn(gameObject, reason)` |
| Fase 1 | `ObjectPooler.Instance.ReturnToPool`, karena `SpawnManager` tidak ada di `ShipPhase` |

### Pool

**`OnSpawned()`:**

1. Mengisi `spawnGraceTimer`, dan mengembalikan `speedModifiers` serta `waveModifiers` ke 1.
2. Cari posisi lewat `NavMesh.SamplePosition`, aktifkan agen, panggil `agent.Warp`, lalu periksa `agent.isOnNavMesh` (#88). Agen yang terkurung carving memakai jalur yang sama (#78). Kalau gagal, kembali ke pool.
3. `agent.autoTraverseOffMeshLink = false`, dan `agent.areaMask = data.traversalMask`.
4. `ThreatSystem.Register` dan `HordeCrowdManager.Register`.

**`OnReturned()`:**

1. `agent.enabled = false` (#88).
2. `ThreatSystem.Unregister`, `HordeCrowdManager.Unregister`, dan `BaseNavigation.ReleaseSlot`.
3. `currentBreach = default`, `blockedTimer = 0`, `lair = null`, dan `areaMask` kembali ke nilai bawaan.
4. `IsBurning = false`.

### Dormant

**`public void EnterDormant(ZombieLair lair)`** (#112): `currentState = Dormant`, agen berhenti, `areaMask` hanya `Interior` saat siang, dan rujukan sarang disimpan.

**`public void WakeUp(Transform target)`** (#75): untuk zombie `Dormant` di kapal. Masuk `Chase`, dan `SetTarget` memakai target itu. Kecepatan agen boleh diatur `ShipHazardManager`, dan direset lewat `IPoolable`.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** tidak ada. Event mati dan kena dikirim `EnemyHealth`.

**Event yang didengar:** `EnemyHealth.Damaged` dan `Died` (event per objek), dan `AISensory.LevelChanged`.

| Sistem | Hubungan |
|---|---|
| `HordeCrowdManager` (#93) | `Register`, `CrowdTick`, `TrySetDestination`, dan `AttackBuffer` |
| `ThreatSystem` (#108) | `Register`, `Unregister`, `ReportAttacker`, dan `SetTarget` |
| `AISensory`, `AISensoryHub` (#49) | `Level`, `HuntHint`, dan `LastStimulusPosition` |
| `EnemyAttack` (#109) | `TryStartAttack`, `TryStartStructureAttack`, dan `CancelAttack` |
| `EnemyHitReaction` (#95) | `RequestState`, `SetSpeedMultiplier`, dan mayat |
| `SpecialZombieAI` (#25) | `TickSpecial` |
| `AITraversal` (#94) | `TraversalAgent.Tick` dan `GetResponse` |
| `BaseNavigation` (#78) | `pathCheckQueue`, `TryAssignBreachTarget`, dan `ReleaseSlot` |
| `SunlightExposure` (#63) | `EnterBurn`, serta state `SeekHiding`, `Submerged`, dan `Emerging` |
| `SpawnManager` (#62) | `Despawn`, `ApplyWaveModifiers`, dan `EnemyInstance.IsEngaged` |
| `BaseDefense` (#26) | `IsActiveThreat` dan `ApplySpeedModifier` |
| `SleepSystem` (#67) | `IsActiveThreat` |
| `ZombieLair` (#112) | `EnterDormant` |
| `ShipHazardManager` (#75) | `WakeUp`, `SetTarget`, dan kecepatan agen |
| `FireSystem`, `Flammable` (#97) | Lihat "Kasus Khusus" |

## 4. Kasus Khusus

- **Objek pool:** reset ada di `OnSpawned` dan `OnReturned`. HP direset `EnemyHealth` (#59).
- **Game dijeda:** `CrowdTick` berhenti lewat `HordeCrowdManager`, dan timer memakai `dt`.
- **Permadeath:** musuh tidak disimpan, jadi tidak ada data yang tersisa. `ObjectPooler` menampung semuanya saat scene berganti (#82).
- **Save dan load:** musuh tidak disimpan (#88). Setelah load, musuh baru memilih target sendiri.
- **Terbakar api biasa (#97):** prefab zombie memasang `Flammable` dengan profil `Flame_Zombie`. Status terbakar tidak mengubah `AIState`. Saat `AIState` menjadi `Submerged` atau `BurnToAshes`, `Flammable` memanggil `Extinguish(true)`.
- **Region dilepas (#88):** `SpawnManager` mengembalikan zombie, dan `ReleaseSlot` berjalan lewat `OnReturned`.
- **Vakum Lucky Machine (#72):** `Unregister` aman dipanggil dua kali.
- **Tidak ada target:** `CurrentTarget` bernilai `null`, jadi zombie memakai `HuntHint` atau suara terakhir.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** nilai `AIState` dan pemilik state (`GLOSSARY.md`), aturan terhalang (`BaseNavigation.md`), serta zombie terbakar di luar ruangan saat siang dan boleh diam di ruang gelap (GDD bagian 3).

**Keputusan teknis di blueprint ini:**

- `IsActiveThreat` bernilai `false` untuk `Dormant`, `Submerged`, dan `BurnToAshes`, sesuai `BaseDefense.md`.
- Pengembalian ke pool saat mati dipegang `EnemyHitReaction`, lewat `SpawnManager.Despawn` (#95, #62).

**Usulan yang disetujui developer sebagai nilai awal:** tidak ada keputusan baru dari blueprint ini.

## 6. Skenario Tes

1. Munculkan zombie, lalu berdiri di luar jarak pandang saat malam. **Hasil:** zombie berjalan ke `HuntHint`, dengan state `Idle`.
2. Tembak di dekat zombie. **Hasil:** `Level` menjadi `Aware`, dan state `Chase`.
3. Berdiri di dalam base tertutup. **Hasil:** setelah `blockedConfirmSeconds`, zombie masuk `AttackBuilding`, dan pukulan datang dari `EnemyAttack`.
4. Berdiri di batu tanpa jalur dan tanpa struktur. **Hasil:** zombie mengikuti `AITraversal.GetResponse`, tanpa `AttackBuilding`.
5. Tunggu fajar di tempat terbuka. **Hasil:** `EnterBurn` terpanggil, agen mati, dan `Died` tidak memicu mayat.
6. Pasang jebakan kawat dan tembak kaki zombie yang sama. **Hasil:** dua pengali aktif, masing-masing satu nilai, dan kecepatan pulih setelah waktunya habis.
7. Pakai ulang zombie yang sama 20 kali lewat `DevConsole`. **Hasil:** kecepatan, `areaMask`, `currentBreach`, dan `IsBurning` selalu kembali ke awal.
8. Lepas region saat zombie menyerang dinding. **Hasil:** zombie kembali ke pool, dan posisi serangannya kosong.
9. Tidur di dekat zombie `Dormant`. **Hasil:** `IsActiveThreat` bernilai `false`, dan tidur tidak ditolak karena zombie itu.
10. Di kapal, panggil `WakeUp` pada zombie `Dormant`. **Hasil:** zombie mengejar, dan kecepatan kembali normal saat dipakai ulang.
11. Tunggu fajar dengan zombie sarang di dalam `InteriorZone`. **Hasil:** `EnterBurn` tidak terpanggil, dan state tidak berubah.

Belum diuji di Unity, karena blueprint ini baru rancangan.
