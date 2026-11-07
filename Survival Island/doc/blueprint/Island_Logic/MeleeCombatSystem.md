# Blueprint: MeleeCombatSystem.cs
**Lokasi:** `Survival Island/doc/blueprint/Island_Logic/MeleeCombatSystem.md`
**Deskripsi Utama:** Sistem pertarungan jarak dekat dan panen dengan Pisau, Kapak, dan Beliung, memakai raycast pendek dari kamera.
**Issue:** #59 (melengkapi #12, #18, #49, #60, #73, #97, #103, dan #111)
**Rujukan GDD:** Bagian 3 (`Base_Pisau` sebagai item inti, dan Pisau untuk lepas saat diterkam). Angka damage dan stamina adalah usulan.

## 1. Arsitektur & Variabel Utama

`MeleeCombatSystem : EquippableBehaviour`, sesuai tabel item di `EquipmentManager.md`. Input datang dari `EquipmentManager`, sehingga komponen ini tidak membaca klik kiri.

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `meleeDamage` | `float` | 25 | Dari `WeaponStatCalculator` (#113) |
| `attackRange` | `float` | 2 | Jarak raycast |
| `attackCooldown` | `float` | 1 | Jeda ayunan |
| `nextAttackTime` | `float` | 0 | Waktu ayunan berikutnya |
| `cameraTransform` | `Transform` | Dari `FirstPersonCamera` | Asal raycast |
| `armsAnim` | `AnimationController` | Dari prefab | Animasi tangan (#12) |
| `instance` | `ItemInstance` | `null` | Item yang dipegang |
| `isInDamageMoment` | `bool` | `false` | Dibaca `CanSwitchNow` |

| Angka | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.vitals.meleeStaminaCost` | 15 | Biaya tiap ayunan |
| `GameBalanceConfig.vitals.exhaustedMeleeCooldownMultiplier` | 1,5 | Saat `IsExhausted` |
| `GameBalanceConfig.vitals.exhaustedMeleeDamageMultiplier` | 0,7 | Saat `IsExhausted` |
| `GameBalanceConfig.stealth.noiseMelee` | 8 m | Radius suara ayunan |
| `GameBalanceConfig.fire.fireAxeIgniteChance` | 0,25 | Peluang `Reward_FireAxe` menyalakan target |

**Dihapus:** `enemyLayer`, `staminaCost`, `Update()` yang membaca klik kiri, `Physics.Raycast`, `GetComponent<EnemyHealth>()`, serta pemutaran partikel darah langsung.

**Senjata tambahan (usulan #103):** `Wpn_Machete`, melee dengan damage 35. Berlaku kalau usulan senjata di `RegionLootTable.md` disetujui.

## 2. Detail Logika & Fungsi

**`public override void PrimaryAction(ActionPhase phase)`**, dipanggil `EquipmentManager` dari `InputManager.PrimaryPressed`:

- Kalau `phase == Pressed` dan `Time.time >= nextAttackTime`, panggil `PerformMeleeAttack()`.

**`public override bool CanSwitchNow => !isInDamageMoment`:** bernilai `false` pada momen damage, paling lama 0,2 detik.

**`private void PerformMeleeAttack()`**:

1. `SurvivalStats.SpendStamina(meleeStaminaCost, StaminaUse.Melee)`. Melee tetap berhasil saat lelah.
2. `cooldown = attackCooldown`, dan `damage = meleeDamage`. Kalau `SurvivalStats.IsExhausted`, kalikan dengan `exhaustedMeleeCooldownMultiplier` dan `exhaustedMeleeDamageMultiplier`.
3. `nextAttackTime = Time.time + cooldown`.
4. `armsAnim.Play(AnimIds.Swing)`, dan SFX ayunan.
5. `NoiseEmitter.Emit(GameBalanceConfig.stealth.noiseMelee, NoiseSourceKind.Melee)`.
6. `isInDamageMoment = true`, lalu `PhysicsQuery.Raycast(cameraTransform.position, cameraTransform.forward, out hit, attackRange, PhysicsMasks.Melee)`.
7. Tangani hasil, lalu `isInDamageMoment = false`.

**Hasil raycast:**

| Target | Langkah |
|---|---|
| `ResourceNode` | `result = node.ReceiveHit(item.toolClass, hit.point)`. Kurangi durabilitas lewat `ApplyWear` hanya kalau `result == Hit` |
| `IDamageable` | `DamageRouter.ApplyHit(hit, new DamageInfo { amount = damage, type = DamageType.Melee, source = gameObject pemain, sourceId = item.itemId, hitDirection = cameraTransform.forward, bodyPart = Torso, byPlayer = true })`. `hitDirection` wajib diisi, karena knockback dan ragdoll #95 memakainya. Kurangi durabilitas lewat `ApplyWear` |
| Lainnya | Suara pukulan permukaan |

**Fire Axe (#97):** kalau item adalah `Reward_FireAxe` dan target punya `Flammable`, panggil `Flammable.TryIgnite(fire.fireAxeIgniteChance, true)`.

**Darah:** `VFXManager` memunculkannya dari `EnemyHitEvent`, jadi tidak ada pemutaran partikel di sini.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** tidak ada secara langsung. `EnemyHitEvent` dikirim `EnemyHealth`, dan suara lewat `NoiseEmitter`.

| Sistem | Hubungan |
|---|---|
| `EquipmentManager` (#60), `InputManager` (#18) | `PrimaryAction`, `OnEquip`, dan `CanSwitchNow` |
| `DamagePipeline` (#59), `PhysicsLayers` (#87) | `PhysicsQuery.Raycast`, `PhysicsMasks.Melee`, dan `DamageRouter.ApplyHit` |
| `SurvivalStats` (#111) | `SpendStamina` dan `IsExhausted` |
| `ResourceRespawn` (#73) | `ReceiveHit(toolClass, hitPoint)` |
| `ToolRepairSystem` (#113) | `WeaponStatCalculator`, `RecalculateStats`, dan `ApplyWear` |
| `AnimationController` (#12), `VFXManager` (#15) | `Play(AnimIds.Swing)`, dan darah dari `EnemyHitEvent` |
| `AISensory` (#49) | `NoiseEmitter.Emit` |
| `FireSystem` (#97), `LuckyMachine` (#72) | Fire Axe |
| `RegionLootTable` (#103) | `Wpn_Machete`, kalau disetujui |

## 4. Kasus Khusus

- **Pool dipakai ulang:** viewmodel memakai cache `EquipmentManager`. `OnUnequip` membatalkan ayunan dan mengosongkan `isInDamageMoment`.
- **Game dijeda:** `EquipmentManager` tidak meneruskan input saat diblokir. `Time.time` tidak maju saat `timeScale` 0.
- **Stamina habis:** ayunan tetap berjalan, dengan pengali lelah (`VitalsRules.md`).
- **Alat salah pada node:** `ReceiveHit` mengembalikan `Rejected`, dan durabilitas tidak berkurang.
- **Fire Axe:** `maxDurability = -1`, jadi `ApplyWear` selesai lebih awal.
- **Save dan load:** tidak ada data sendiri. Durabilitas mengikuti `ItemInstance`.
- **Permadeath:** tidak ada data yang terbawa.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** biaya dan pengali stamina (`VitalsRules.md`), `DamageRouter` (`DamagePipeline.md`), serta aturan `Hit` (`ResourceRespawn.md`).

**Usulan yang disetujui developer sebagai nilai awal (#85):**

1. **`Wpn_Machete` damage 35:** usulan senjata di `RegionLootTable.md`. Tidak berlaku sampai disetujui.
2. **Damage 25, jarak 2, dan jeda 1 detik:** nilai lama. Rekomendasi: pindahkan ke data senjata saat script ditulis (#84).

## 6. Skenario Tes

1. Pegang Pisau, lalu klik kiri pada zombie. **Hasil:** damage 25, stamina -15, dan darah muncul dari `EnemyHitEvent`.
2. Pukul kepala. **Hasil:** damage 50 lewat `HitboxModifier`.
3. Pukul saat `IsExhausted`. **Hasil:** ayunan berhasil, damage x0,7, dan jeda x1,5.
4. Pukul pohon dengan Kapak. **Hasil:** node berkurang, dan durabilitas turun.
5. Pukul pohon dengan Beliung yang ditolak. **Hasil:** `Rejected`, dan durabilitas tetap.
6. Ganti senjata tepat saat momen damage. **Hasil:** pergantian menunggu sampai selesai.
7. Ayun dekat zombie `Unaware`. **Hasil:** `NoiseEmitter` memakai radius 8 m.
8. Pukul zombie dengan `Reward_FireAxe` berulang. **Hasil:** sekitar 25 persen pukulan menyalakan target.
9. Cari `Physics.Raycast` dan `GetComponent<EnemyHealth>` di skrip. **Hasil:** tidak ada.
10. Bunuh zombie dengan pukulan melee yang memicu knockback. **Hasil:** zombie terdorong atau ragdoll jatuh searah pandangan kamera, bukan diam di tempat.

## 7. Catatan Implementasi (Langkah 5)

Kode: `Island_Logic/MeleeCombatSystem.cs`.

- Damage dibaca dari `ItemData.weaponDamage`, dengan cadangan 25 di prefab kalau nilainya 0. Jarak (2) dan jeda (1) berada di prefab viewmodel, karena `ItemData` belum punya field untuk itu. Pindahkan ke data senjata kalau #113 menambahkannya.
- Panen memanggil `ResourceNode.ReceiveHit(toolClass, hitPoint, true)` milik Langkah 4, lalu berhenti tanpa damage. `DurabilitySystem.ApplyWear` (#113), `AnimationController` (#12), dan Fire Axe (#97) masih TODO.

## 8. Catatan Implementasi (Sistem pendukung)

- **Keputusan developer (7 November 2026):** tusukan Pisau yang kena musuh memakai keausan `resource.knifeDecayPerCombatHit` (nilai awal 1).
- **Keputusan developer (7 November 2026):** pukulan tangan kosong memakai `MeleeCombatSystem` lewat `FistBehaviour` (turunan kelas ini) dengan damage dasar `equipment.fistDamage` (nilai awal 5). Damage dasar dibaca dari `protected virtual GetBaseDamage(ItemData)`. Tanpa item, jadi tanpa keausan.

## Catatan Implementasi (Sistem pendukung)

- Isi TODO (sistem pendukung): `public void RecalculateStats()` menghitung ulang damage lewat `WeaponStatCalculator.GetMeleeDamage`; dipanggil `OnEquip` dan `ToolRepairSystem` (#113).
- Isi TODO (sistem pendukung): Fire Axe (#97): kalau `itemId == Reward_FireAxe`, damage berhasil, dan target punya `Flammable`, panggil `TryIgnite(fire.fireAxeIgniteChance, true)`.
