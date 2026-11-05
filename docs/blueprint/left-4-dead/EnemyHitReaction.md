# Blueprint: EnemyHitReaction.cs
**Lokasi:** `docs/blueprint/left-4-dead/EnemyHitReaction.md`
**Deskripsi Utama:** Respons musuh saat terkena serangan dan setelah mati: terhuyung, terdorong, melambat, Shove pemain, mayat, dan drop.
**Issue:** #95
**Rujukan GDD:** Bagian 5 (Zombie Militer berarmor, Beruang Kutub) dan bagian 3 (`Peluru` sebagai sumber daya terbatas). State terdorong dan terhuyung berasal dari `docs/CATATAN_MEKANIK.md` bagian 3. Shove, batas mayat, dan drop zombie adalah usulan, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

`EnemyHitReaction : MonoBehaviour, IPoolable` berada di objek utama musuh, termasuk hewan. Komponen ini mendengar `EnemyHealth.Damaged` dan `Died`, yang berada pada objek yang sama, lalu meminta reaksi lewat `IHitReactive`, bukan lewat `EnemyAI` langsung. Zombie memakai `EnemyAI`, sedangkan hewan memakai `WildlifeAI`, yang tidak punya `AIState` (#61).

```csharp
public enum HitReactionKind { Stagger, Knockback, Dying }

public interface IHitReactive
{
    bool RequestReaction(HitReactionKind kind);   // false kalau ditolak prioritas
    void EndReaction();                           // Reaksi selesai, AI lanjut
    void SetSpeedMultiplier(SpeedSource source, float multiplier);
}
```

`EnemyAI` meneruskan `RequestReaction` ke `RequestState` dengan `AIState` yang sama namanya (`EnemyAI.md`). `WildlifeAI` menghentikan tick FSM selama reaksi, lalu melanjutkan `AnimalState` sebelumnya saat `EndReaction`. Untuk `Dying`, FSM hewan berhenti tetap (`WildlifeAI.md`). Rujukan `IHitReactive` diambil sekali di `Awake`. Di bagian bawah, "`EnemyAI.RequestState(AIState.X)`" dan "`RequestState(AIState.X)`" berarti `reactive.RequestReaction(HitReactionKind.X)`, dan "`EnemyAI` kembali ke `Chase`" berarti `reactive.EndReaction()`.

**Urutan prioritas state:** `BurnToAshes` lalu `Dying`, `Knockback`, `Stagger`, kemudian state lainnya. Permintaan yang lebih rendah ditolak. `Dying` adalah nilai baru pada `AIState`, karena `GLOSSARY.md` belum punya state sekarat.

### Tambahan pada `EnemyData`

| Field | Zombie standar | Zombie Desa | Zombie Militer | Beruang Kutub | Fungsi |
|---|---|---|---|---|---|
| `poise` | 25 | 20 | 90 | 120 | Damage terkumpul sampai terhuyung |
| `canBeStaggered` | `true` | `true` | `true` | `true` | Boleh `Stagger` |
| `canBeKnockedBack` | `true` | `true` | `false` | `false` | Boleh `Knockback` |
| `knockbackMultiplier` | 1 | 1,2 | 0 | 0 | Pengali jarak dorong |
| `leavesCarcass` | `false` | `false` | `false` | `true` | Bangkai untuk #61 |
| `dropTable` | `Drop_Zombie_Standard` | `Drop_Zombie_Village` | `Drop_Zombie_Military` | `null` | `LootTable` (#103) |
| `dropChance` | 0,08 | 0,1 | 0,4 | 0 | Peluang drop |

Nilai varian lain ditetapkan #25 dan #61.

### Variabel

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `accumulatedPoise` | `float` | 0 | Damage sejak reaksi terakhir |
| `staggerImmuneUntil` | `float` | 0 | Mencegah terkunci stagger terus-menerus |
| `reactionEndTime` | `float` | 0 | Akhir `Stagger` |
| `knockbackVelocity` | `Vector3` | `zero` | Sisa dorongan |
| `legSlowUntil` | `float` | 0 | Akhir efek kaki |
| `corpseRoutine` | `Coroutine` | `null` | Urutan mati |
| `bones` | `Rigidbody[]` | Dari prefab | Tulang ragdoll, disimpan sekali di `Awake` |

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.hitReaction.staggerSeconds` | 0,35 | Lama `Stagger` |
| `GameBalanceConfig.hitReaction.staggerImmunitySeconds` | 1 | Kebal stagger setelah reaksi |
| `GameBalanceConfig.hitReaction.headStaggerMultiplier` | 1,5 | Pengali lama stagger dari kepala |
| `GameBalanceConfig.hitReaction.legSlowMultiplier` | 0,6 | Kecepatan saat kaki kena |
| `GameBalanceConfig.hitReaction.legSlowSeconds` | 2 | Lama melambat |
| `GameBalanceConfig.hitReaction.armorNoFlinchBelow` | 0,5 | Pengali armor di bawah ini tidak menambah `poise` |
| `GameBalanceConfig.hitReaction.meleeKnockback` | 0,8 | Jarak dorong melee |
| `GameBalanceConfig.hitReaction.explosionKnockback` | 2,5 | Jarak dorong ledakan |
| `GameBalanceConfig.hitReaction.knockbackSeconds` | 0,25 | Lama gerakan terdorong |
| `GameBalanceConfig.hitReaction.shoveRadius` | 1,6 | Radius Shove |
| `GameBalanceConfig.hitReaction.shoveConeDot` | 0,5 | Batas arah Shove, sekitar 120 derajat |
| `GameBalanceConfig.hitReaction.shoveKnockback` | 2,5 | Jarak dorong Shove |
| `GameBalanceConfig.hitReaction.shoveStaggerSeconds` | 1 | Lama terhuyung setelah Shove |
| `GameBalanceConfig.hitReaction.shoveCooldownSeconds` | 0,8 | Jeda Shove |
| `GameBalanceConfig.hitReaction.shoveStaminaCost` | 10 | Biaya stamina |
| `GameBalanceConfig.hitReaction.corpseHoldSeconds` | 6 | Lama mayat diam |
| `GameBalanceConfig.hitReaction.corpseFadeSeconds` | 1,5 | Lama memudar |
| `GameBalanceConfig.hitReaction.ragdollSettleSeconds` | 2 | Waktu sebelum tulang dibekukan |
| `GameBalanceConfig.hitReaction.maxCorpses` | 12 | Batas mayat sekaligus |
| `GameBalanceConfig.hitReaction.maxRagdolls` | 4 | Batas ragdoll fisika aktif |
| `GameBalanceConfig.hitReaction.ragdollForce` | 6 | Gaya pada tulang terdekat |
| `GameBalanceConfig.hitReaction.maxDropsPerNight` | 10 | Batas drop per malam |

## 2. Detail Logika & Fungsi

### Reaksi per serangan

**`private void HandleDamaged(DamageInfo info)`**:

1. Kalau state `Dying` atau `BurnToAshes`, abaikan.
2. Terapkan efek bagian tubuh.
3. Terapkan tabel jenis serangan.

| `DamageType` | Reaksi |
|---|---|
| `Bullet` | Menambah `accumulatedPoise`. Saat mencapai `poise`, `Stagger` |
| `Melee` | Sama dengan `Bullet`. Saat `poise` tercapai, panggil `ApplyKnockback(info.hitDirection, meleeKnockback, staggerSeconds)` |
| `Explosion` | Langsung `Knockback` sejauh `explosionKnockback`, lalu `Stagger`, dengan `force` ke `EnemyAttack.Interrupt` |
| `Fire`, jenis lain | Tidak ada reaksi. Efek api milik #97 dan #63 |

### Efek per bagian tubuh

Dibaca dari `info.bodyPart`, yang diisi `HitboxModifier.Apply`.

| Bagian | Efek |
|---|---|
| `Head` | Lama stagger dikali `headStaggerMultiplier` |
| `Torso` | Normal |
| `Leg` | `reactive.SetSpeedMultiplier(SpeedSource.LegHit, legSlowMultiplier)` selama `legSlowSeconds`. Diperpanjang kalau kena lagi, tanpa bertumpuk |
| Armor | Kalau `DamageResistanceTable.GetMultiplier(type, bodyPart) < armorNoFlinchBelow`, `poise` tidak bertambah, dan keluar percikan logam lewat `VFXManager.Instance.Play("VFX_MetalSpark", pos, rot)` (#15) |

Zombie Militer yang ditembak di badan tidak terhuyung, tetapi ditembak di kepala (pengali 1) masih menambah `poise`. Celah kepala di `DamagePipeline.md` jadi terasa.

### Stagger

**`public void ApplyStagger(float seconds)`**:

1. Tolak kalau `!data.canBeStaggered` atau `Time.time < staggerImmuneUntil`.
2. `EnemyAttack.Interrupt(false)`.
3. `reactive.RequestReaction(HitReactionKind.Stagger)`. Kalau ditolak, selesai. Lalu `agent.isStopped = true`, dan `anim.Play(AnimIds.Stagger)` (#12).
4. `reactionEndTime = Time.time + seconds`, `staggerImmuneUntil = reactionEndTime + staggerImmunitySeconds`, dan `accumulatedPoise = 0`.
5. Saat waktunya habis, `agent.isStopped = false`, lalu `reactive.EndReaction()`. Zombie kembali ke `Chase`, dan hewan melanjutkan `AnimalState` sebelumnya.

### Knockback: `NavMeshAgent` dan fisika

**Keputusan:** musuh hidup yang terdorong **tetap memakai `NavMeshAgent`**, lewat `agent.Move`, bukan `Rigidbody`. Cara ini murah untuk 50 zombie, tidak mungkin keluar dari NavMesh, dan tidak melawan `CharacterController` pemain. Fisika penuh hanya dipakai ragdoll mati.

**`public void ApplyKnockback(Vector3 dir, float distance, float staggerSeconds)`**, nama yang sama dengan pemanggilan di `ShipHazardManager.md`:

1. Kalau `!data.canBeKnockedBack`, lanjut ke `ApplyStagger(staggerSeconds)`, lalu selesai.
2. Kalau `!agent.enabled` atau `agent.isOnOffMeshLink`, lanjut ke `ApplyStagger`, lalu selesai. Link #94 tidak boleh terganggu.
3. `EnemyAttack.Interrupt(false)`, `reactive.RequestReaction(HitReactionKind.Knockback)`, `agent.isStopped = true`, `agent.ResetPath()`, `agent.updateRotation = false`, dan `anim.Play(AnimIds.Knockback)` (#12).
4. `knockbackVelocity = dir.flattened * distance * knockbackMultiplier / knockbackSeconds`. Kalau `dir` hampir nol (pengirim lama yang belum mengisi `hitDirection`), pakai arah dari pemain ke musuh, supaya dorongan tidak hilang.
5. Selama `knockbackSeconds`, panggil `agent.Move(knockbackVelocity * Time.deltaTime)`, dengan kecepatan yang berkurang linier.
6. Saat selesai, `agent.updateRotation = true`. Kalau `!agent.isOnNavMesh`, cari lewat `NavMesh.SamplePosition`, lalu `agent.Warp`. Kalau gagal, kembali ke pool.
7. Lanjut ke `ApplyStagger(staggerSeconds)`. Pemeriksaan kebal stagger dilewati pada langkah ini.

### Shove pemain

**`PlayerShove`**, komponen di pemain pada `IslandPhase`. Aksi `Shove` di `InputManager` bernilai awal `V`, dan tersedia dengan item apa pun di tangan, supaya tidak membebani klik kiri.

**`private void TryShove()`**:

1. Tolak kalau masih dalam `shoveCooldownSeconds`, atau `EquipmentManager.locks != None`.
2. Tolak kalau `!SurvivalStats.SpendStamina(shoveStaminaCost, StaminaUse.Melee)`, dengan notifikasi lelah.
3. Mainkan animasi dorong di viewmodel.
4. `count = PhysicsQuery.OverlapSphere(player.position + forward, shoveRadius, PhysicsMasks.Melee, buffer)`, lalu `CollectUniqueDamageables`.
5. Untuk tiap target yang punya `EnemyHitReaction` dan berada dalam `shoveConeDot`: kalau `data.canBeKnockedBack`, panggil `ApplyKnockback(arah menjauh, shoveKnockback, shoveStaggerSeconds)`. Kalau tidak, mainkan bunyi dorongan gagal tanpa reaksi apa pun.

Shove tidak memberi damage dan tidak membatalkan reload. Di kapal, `ShipShove` (#75) tetap memakai angkanya sendiri.

### Musuh besar

| Kelompok | Bisa `Knockback` | Bisa `Stagger` | Hasil Shove |
|---|---|---|---|
| Zombie biasa dan hewan kecil | Ya | Ya | Terdorong lalu terhuyung |
| Zombie Militer, Beruang Kutub | Tidak | Ya, dengan `poise` tinggi | Bunyi gagal, tanpa stagger |

### Urutan mati

**`private void HandleDied(DamageInfo info)`**:

1. `EnemyAttack.CancelAttack()`, `reactive.RequestReaction(HitReactionKind.Dying)`, dan matikan `NavMeshAgent` serta kapsul `Enemy`. Untuk hewan, isi `EnemyInstance.IsCarcass = true`, supaya pengaman macet dan `SweepWrongPhase` tidak mengembalikan mayat atau bangkai yang agennya mati (`SpawnManager.md`).
2. Panggil `SpawnDrop(info)`.
3. Pilih bentuk mayat:
   - `Explosion` atau `Melee` dengan Knockback, dan `CorpseBudget.TryAcquireRagdoll()` berhasil: ragdoll.
   - Selain itu: `anim.Play(AnimIds.Die)` (#12).
4. Daftarkan ke `CorpseBudget`.
5. Jalankan `corpseRoutine`.

**Ragdoll:** `animator.enabled = false`, tulang pindah ke layer `Ragdoll`, `isKinematic = false`, lalu `AddForceAtPosition(info.hitDirection * ragdollForce, info.hitPoint)` pada tulang terdekat. Ledakan mengisi `hitDirection` dari pusat ledakan ke target (`DamagePipeline.md`), dan melee dari kamera ke depan (`MeleeCombatSystem.md`). Setelah `ragdollSettleSeconds`, semua tulang kembali kinematik dan izin ragdoll dilepas.

**`corpseRoutine`:**

1. Tunggu `corpseHoldSeconds`. Kalau `data.leavesCarcass`, aktifkan `AnimalCarcass` pada prefab hewan, lalu berhenti di sini. Pudar dan pengembalian diatur `AnimalCarcass` (#61).
2. Pudarkan renderer lewat `MaterialPropertyBlock` selama `corpseFadeSeconds`.
3. `SpawnManager.Instance.Despawn(gameObject, DespawnReason.Killed)` (#62), supaya daftar musuh hidup tetap benar. `EnemyHitReaction` satu-satunya pemilik pengembalian musuh yang mati, kecuali bangkai hewan (`AnimalCarcass`, #61) dan zombie terbakar (`SunlightExposure`, #63). Hanya di Fase 1 musuh langsung kembali lewat `ObjectPooler.Instance.ReturnToPool(poolTag, gameObject)`.

### Batas mayat

**`CorpseBudget`** adalah manager per scene yang memakai antrean mayat.

- Saat jumlah mayat melebihi `maxCorpses`, mayat tertua melewati sisa waktu tunggu dan langsung memudar.
- Ragdoll baru melebihi `maxRagdolls`, sehingga mati dengan animasi.
- Saat `DaybreakEvent`, semua mayat zombie langsung memudar.

### Drop

**`private void SpawnDrop(DamageInfo info)`**:

1. Lewati kalau `dropTable == null` atau `dropsThisNight >= maxDropsPerNight`.
2. Lewati kalau bukan `info.byPlayer` dan `info.source` tidak bertag `NPC`. Zombie yang mati karena matahari (#63), tumbal Lucky Machine (#72), dan bencana jadi tidak menghasilkan drop.
3. Lempar peluang `dropChance` memakai `RunSeedManager.GetStream(Gameplay)`.
4. `RegionLootTable.TryRoll(dropTable, LootPointKind.Any, GameManager.currentDay, ref RunSeedManager.GetStream(Gameplay), out item, out amount)` (#103), lalu `WorldItem.Spawn(inst, amount, position, despawns: true)` (#66).

| Varian | `dropTable` | Peluang | Isi usulan |
|---|---|---|---|
| Zombie standar | `Drop_Zombie_Standard` | 8 persen | `Ammo_9mm` 1 sampai 3, atau `Mat_Cloth` 1 |
| Zombie Tribal | `Drop_Zombie_Tribal` | 15 persen | Bahan medis Region 2. ID final di #103 |
| Zombie Lumut | `Drop_Zombie_Moss` | 15 persen | Bahan medis Region 2. ID final di #103 |
| Zombie Militer | `Drop_Zombie_Military` | 40 persen | `Ammo_9mm` 3 sampai 6 |
| Zombie Beku | `Drop_Zombie_Frozen` | 8 persen | `Mat_Cloth` 1 sampai 2 |
| Zombie Desa | `Drop_Zombie_Village` | 10 persen | `Ammo_9mm` 1 sampai 3 |

Hasil paling banyak pada zombie standar: 0,08 × 2 = 0,16 peluru per mayat, kalau semua drop berupa peluru. Peluru tetap langka, karena hasil ini jauh di bawah satu peluru per zombie. HP final zombie ada di #25.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** tidak ada. Reaksi tetap berada di dalam objek.

**Event yang didengar:** `EnemyHealth.Damaged` dan `Died` (event per objek), `DaybreakEvent` (`CorpseBudget`), serta `NightfallEvent` (mengosongkan `dropsThisNight`).

| Sistem | Hubungan |
|---|---|
| `EnemyAI`, `WildlifeAI` (#61) | `IHitReactive`: `RequestReaction`, `EndReaction`, dan `SetSpeedMultiplier` |
| `EnemyAttack` (#109) | `Interrupt` dan `CancelAttack` |
| `DamagePipeline` (#59) | `info`, `DamageResistanceTable.GetMultiplier` |
| `HitboxModifier` | Mengisi `bodyPart` |
| `ShipHazardManager` (#75) | Memakai `ApplyKnockback` |
| `RegionLootTable` (#103), `ItemUseSystem` (#66), `RunSeedManager` (#89) | Drop |
| `WildlifeAI`, `AnimalCarcass` (#61) | Mengambil bangkai `leavesCarcass` |
| `SpawnManager` (#62) | `Despawn(gameObject, DespawnReason.Killed)` |
| `VFXManager` (#15), `AnimationController` (#12) | Percikan armor, dan `anim.Play` |
| `PhysicsLayers` (#87) | Layer `Ragdoll` dan `EnemyHitbox` |
| `InputManager` (#18), `SurvivalStats` (#111) | Tombol Shove dan stamina |

**Batas pekerjaan:** menguliti hewan di #61, jalur damage di #59, dan efek partikel di #15.

## 4. Kasus Khusus

- **Objek pool (`OnSpawned`):** hentikan `corpseRoutine`, `accumulatedPoise = 0`, `staggerImmuneUntil = 0`, `legSlowUntil = 0`, `knockbackVelocity = zero`, tulang kinematik dan kembali ke layer `EnemyHitbox`, `animator.enabled = true`, kapsul dan agen menyala, `MaterialPropertyBlock` dibersihkan, `agent.updateRotation = true`, dan pengali kecepatan `LegHit` dihapus.
- **Objek pool (`OnReturned`):** `CorpseBudget.Remove(this)`, dan izin ragdoll dilepas kalau masih dipegang.
- **Game dijeda:** waktu reaksi dan coroutine memakai waktu berskala, jadi berhenti. Fisika ragdoll juga berhenti.
- **Permadeath:** tidak ada data reaksi. `dropsThisNight` hilang bersama scene.
- **Save:** musuh dan mayat tidak disimpan (#88). Mayat hilang setelah load. Drop tersimpan sebagai `WorldItem` lewat `RegionStateStore`.
- **Fajar saat terhuyung:** `BurnToAshes` menimpa `Stagger`, dan `EnemyAI` memanggil reset reaksi.
- **Ditembak lagi saat `Knockback`:** `poise` tetap bertambah, tetapi dorongan tidak ditambah.
- **`OptimizationManager` mematikan agen:** `ApplyKnockback` turun menjadi `ApplyStagger`.
- **Ragdoll jatuh dari region yang dilepas:** `RegionLoadStateChangedEvent` lewat `SpawnManager` mengembalikan objek ke pool (#62).
- **Dua kematian pada frame sama:** `IsAlive` di `EnemyHealth` hanya mengirim `Died` sekali.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** `Stagger` dan `Knockback` di `GLOSSARY.md`. Pengali kaki 0,5 dan kepala 2x. Layer `Ragdoll` (#87). Pemakaian `RandomStreamId.Gameplay` untuk drop (#89).

**Keputusan teknis di blueprint ini:**

- Terdorong memakai `agent.Move`, bukan `Rigidbody`.
- Mayat memakai `CorpseBudget` per scene.
- `AIState.Dying` ditambahkan untuk mayat. Perlu dicatat di `GLOSSARY.md`.

**Usulan yang disetujui developer sebagai nilai awal (dicatat di #85):**

1. **Drop zombie:** ada, dengan peluang kecil per varian, batas 10 per malam, dan tanpa drop dari matahari atau tumbal. Zombie Militer menjadi sumber peluru utama, karena berisiko tinggi.
2. **Shove:** tombol `V`, tanpa damage, dengan biaya 10 stamina dan jeda 0,8 detik.
3. **Efek kaki:** kecepatan 60 persen selama 2 detik.
4. **Mayat:** 6 detik diam, 1,5 detik memudar, maksimal 12 mayat dan 4 ragdoll aktif.
5. **Musuh besar:** Zombie Militer dan Beruang Kutub kebal dorongan.

## 6. Skenario Tes

1. Tembak badan zombie standar dengan `Base_Pistol`. **Hasil:** zombie terhuyung 0,35 detik, dan peluru berikutnya dalam 1 detik tidak menguncinya lagi.
2. Tembak kaki. **Hasil:** kecepatan turun ke 60 persen selama 2 detik, dan tembakan kedua hanya memperpanjang waktu.
3. Tembak badan Zombie Militer berulang. **Hasil:** percikan logam, tanpa terhuyung. Tembak kepala. **Hasil:** terhuyung setelah `poise` 90 tercapai.
4. Pukul zombie dengan Pisau di dekat dinding. **Hasil:** terdorong sampai batas NavMesh, tidak menembus, lalu kembali mengejar.
5. Ledakkan tong di dekat zombie. **Hasil:** terdorong atau menjadi ragdoll kalau mati, dan serangan batal.
6. Tekan `V` di tengah 5 zombie. **Hasil:** zombie dalam 120 derajat terdorong, stamina turun 10, HP zombie tetap. Tekan lagi dalam 0,8 detik. **Hasil:** ditolak.
7. Tekan `V` pada Zombie Militer. **Hasil:** tidak bergerak dan tidak terhuyung.
8. Tekan `V` saat stamina 0. **Hasil:** ditolak, dengan notifikasi.
9. Bunuh 20 zombie dengan ledakan. **Hasil:** paling banyak 4 ragdoll aktif dan 12 mayat, dan yang tertua memudar lebih dulu.
10. Bunuh zombie tanpa ledakan. **Hasil:** animasi mati, diam 6 detik, memudar 1,5 detik, lalu kembali ke pool.
11. Pakai ulang zombie itu dari pool. **Hasil:** tulang di `EnemyHitbox`, Animator aktif, mesh terlihat penuh, dan tidak ada efek kaki lama.
12. Tunggu fajar dengan mayat tergeletak. **Hasil:** mayat langsung memudar, tanpa drop dari zombie yang terbakar.
13. Bunuh Zombie Militer 20 kali lewat `DevConsole`. **Hasil:** sekitar 8 drop peluru, dan batas per malam berlaku setelah 10 drop.
14. Bunuh dengan seed yang sama, urutan sama. **Hasil:** urutan drop sama.
15. Jeda saat mayat diam. **Hasil:** hitungan berhenti.
16. Tembak kaki Serigala, lalu tembak badannya sampai `poise` tercapai. **Hasil:** Serigala melambat dan terhuyung lewat `WildlifeAI` sebagai `IHitReactive`, tanpa error `EnemyAI`, lalu melanjutkan `Chase`.
17. Bunuh Beruang Kutub dan menjauh di luar pandangan. **Hasil:** `IsCarcass = true`, bangkai tidak dikembalikan sebagai `OffNavMesh`.
18. Pukul zombie dengan Pisau sampai `poise` tercapai. **Hasil:** zombie terdorong menjauhi pemain, bukan diam di tempat.
