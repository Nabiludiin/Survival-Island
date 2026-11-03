# Blueprint: HitboxModifier.cs
**Lokasi:** `docs/blueprint/Island_Logic/HitboxModifier.md`
**Deskripsi Utama:** Dipasang di collider bagian tubuh musuh (Kepala, Badan, Kaki) untuk mengisi bagian tubuh dan pengali damage pada `DamageInfo`.
**Issue:** #59
**Rujukan GDD:** Usulan, belum ada di GDD. Headshot 2x mengikuti `Claude.md` Pilar 3 dan keputusan #85.

## 1. Arsitektur & Variabel Utama

Komponen di collider layer `EnemyHitbox`. Komponen ini tidak punya referensi HP. HP dicari `DamageRouter` lewat parent.

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `bodyPart` | `BodyPart` | `Torso` | `Head`, `Torso`, atau `Leg`, sesuai `GLOSSARY.md` |

| Angka | Nilai awal | Bagian tubuh |
|---|---|---|
| `GameBalanceConfig.damage.headMultiplier` | 2 | `Head` |
| `GameBalanceConfig.damage.torsoMultiplier` | 1 | `Torso` |
| `GameBalanceConfig.damage.legMultiplier` | 0,5 | `Leg` |

**Dihapus:** `mainHealthScript`, `damageMultiplier`, dan `OnHit`.

## 2. Detail Logika & Fungsi

**`public void Apply(ref DamageInfo info)`**, dipanggil `DamageRouter.ApplyHit`:

1. `info.bodyPart = bodyPart`.
2. Kalau `info.type` adalah `Bullet` atau `Melee`, kalikan `info.amount` dengan pengali bagian tubuh dari `GameBalanceConfig.damage`.
3. Jenis lain tidak dikalikan.

Fungsi ini tidak memanggil `TakeDamage`, supaya pengali headshot tidak ditulis dua kali.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim dan didengar:** tidak ada. `EnemyHitEvent` membawa `bodyPart` yang diisi di sini.

| Sistem | Hubungan |
|---|---|
| `DamageRouter` (#59) | Memanggil `Apply` |
| `GunController` (#58), `MeleeCombatSystem` | Mengirim damage lewat `DamageRouter` |
| `EnemyHitReaction` (#95) | Membaca `info.bodyPart` |
| `VFXManager` (#15) | Darah berat untuk `BodyPart.Head`, lewat `EnemyHitEvent` |
| `PhysicsLayers` (#87) | Collider berada di layer `EnemyHitbox` |

## 4. Kasus Khusus

- **Pool dipakai ulang:** komponen tidak punya status yang berubah, jadi tidak perlu direset.
- **Game dijeda:** tidak ada `Update`.
- **Ledakan dan api:** `ExplosionDamage` memakai kapsul `Enemy` dan selalu `Torso`, tanpa melewati hitbox.
- **Collider tanpa `HitboxModifier`:** `DamageRouter` tetap memberi damage dengan `Torso`.
- **Save dan permadeath:** tidak ada data tersimpan.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** headshot 2x (#85), urutan hitung damage, dan `BodyPart` di `GLOSSARY.md`.

**Usulan yang disetujui developer sebagai nilai awal (#85):**

1. **Kaki 0,5x:** berasal dari blueprint lama. GDD tidak menyebutnya. Rekomendasi: pakai sebagai nilai awal.

## 6. Skenario Tes

1. Tembak kepala, badan, dan kaki zombie. **Hasil:** damage 2x, 1x, dan 0,5x.
2. Pukul kepala dengan Pisau. **Hasil:** pengali 2x juga berlaku.
3. Ledakkan tong di dekat zombie. **Hasil:** tidak ada pengali kepala.
4. Ubah `headMultiplier` lewat `GameBalanceConfig`. **Hasil:** damage mengikuti nilai baru.
5. Cari `TakeDamage` di `HitboxModifier.cs`. **Hasil:** tidak ada.
