# Blueprint: HitboxModifier.cs
**Lokasi:** `Survival Island/doc/blueprint/Island_Logic/HitboxModifier.md`
**Deskripsi Utama:** Dipasang di collider bagian tubuh musuh (Kepala, Badan, Kaki) untuk mengisi bagian tubuh dan pengali damage pada `DamageInfo`.
**Issue:** #59
**Rujukan GDD:** Usulan, belum ada di GDD. Headshot 2x mengikuti `Claude.md` Pilar 3 dan keputusan #85. Armor Zombie Militer (GDD bagian 5) dihitung di `DamageResistanceTable`, bukan di sini.

## 1. Arsitektur & Variabel Utama

Komponen di collider layer `EnemyHitbox`. Komponen ini tidak punya referensi HP. HP dicari `DamageRouter` lewat parent.

**Batas tanggung jawab:**

| Pekerjaan | Pemilik |
|---|---|
| Mengisi `bodyPart` dan pengali bagian tubuh | `HitboxModifier` |
| Mencari `IDamageable` lewat parent dan memanggil `Apply` | `DamageRouter` (`DamagePipeline.md`) |
| Armor per jenis damage dan bagian tubuh | `DamageResistanceTable`, dari `EnemyData.resistances` (#59, #25) |
| Reaksi kena kaki, stagger, dan ragdoll | `EnemyHitReaction` (#95) |
| Pindah layer tulang saat ragdoll dan kembali saat pool | `EnemyHitReaction` (#95) |

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `bodyPart` | `BodyPart` | `Torso` | `Head`, `Torso`, atau `Leg`, sesuai `GLOSSARY.md` |

Komponen tidak menyimpan pengali sendiri. Angka dibaca dari `GameManager.Instance.Balance.damage` setiap kali `Apply` dipanggil, supaya perubahan lewat `DevConsole` langsung berlaku. Biayanya hanya membaca satu field.

| Angka | Nilai awal | Bagian tubuh |
|---|---|---|
| `GameBalanceConfig.damage.headMultiplier` | 2 | `Head` |
| `GameBalanceConfig.damage.torsoMultiplier` | 1 | `Torso` |
| `GameBalanceConfig.damage.legMultiplier` | 0,5 | `Leg` |

**Dihapus:** `mainHealthScript`, `damageMultiplier`, dan `OnHit`.

### Susunan hitbox di prefab

Collider dipasang pada tulang rig, bukan pada kapsul gerak `Enemy`. Collider tidak boleh trigger, karena `PhysicsQuery.Raycast` memakai `QueryTriggerInteraction.Ignore` (#87).

| Tulang | Bentuk collider | `bodyPart` |
|---|---|---|
| Kepala | `SphereCollider` | `Head` |
| Dada, perut, dan pinggul | `CapsuleCollider` atau `BoxCollider` | `Torso` |
| Lengan atas dan bawah | `CapsuleCollider` | `Torso` (usulan) |
| Paha dan betis | `CapsuleCollider` | `Leg` |

**Hewan (#61):** memakai komponen yang sama dengan `EnemyHealth`. Hewan besar (Serigala, Jaguar, Beruang Kutub, Babi Hutan, anjing liar) memakai susunan kepala, badan, dan kaki. Hewan kecil (Gagak, Ular) cukup satu hitbox `Torso` (usulan), karena kepalanya terlalu kecil untuk dibidik dengan adil.

## 2. Detail Logika & Fungsi

**`public void Apply(ref DamageInfo info)`**, dipanggil `DamageRouter.ApplyHit`:

1. `info.bodyPart = bodyPart`.
2. Kalau `info.type` adalah `Bullet` atau `Melee`, kalikan `info.amount` dengan `GetMultiplier(bodyPart)`.
3. Jenis lain tidak dikalikan, tetapi `bodyPart` tetap terisi, supaya armor dan reaksi tetap tahu bagian yang kena.
4. Field lain (`byPlayer`, `infection`, `sourceId`, `hitDirection`) tidak diubah.

Fungsi ini tidak memanggil `TakeDamage`, supaya pengali headshot tidak ditulis dua kali.

**`public static float GetMultiplier(BodyPart part)`**:

1. Ambil `d = GameManager.Instance.Balance.damage`.
2. Kembalikan `d.headMultiplier`, `d.torsoMultiplier`, atau `d.legMultiplier` sesuai `part`.
3. Hasilnya dibatasi `Max(0, nilai)`, supaya angka negatif dari konsol tidak menyembuhkan musuh.

Fungsi statis ini juga boleh dibaca UI atau `DevConsole` untuk menampilkan pengali, tanpa mencari komponen.

**Urutan dengan sistem lain** (dari `DamagePipeline.md`):

1. Senjata mengisi `amount`, termasuk modifier Lucky Machine (#72) dan upgrade (#113).
2. `HitboxModifier.Apply` mengalikan pengali bagian tubuh.
3. `EnemyHealth.TakeDamage` menerapkan armor dari `EnemyData.resistances`.

Contoh Zombie Militer dengan senjata 30 damage: kepala 30 x 2 x 1 = 60, badan 30 x 1 x 0,35 = 10,5, dan kaki 30 x 0,5 x 0,5 = 7,5.

**`void OnValidate()`** (hanya Editor):

1. Kalau `gameObject.layer` bukan `EnemyHitbox`, tulis peringatan.
2. Kalau collider di objek ini `isTrigger`, tulis peringatan.
3. Kalau `GetComponentInParent<IDamageable>()` bernilai `null`, tulis peringatan.

Pemeriksaan yang sama dijalankan alat audit layer di `PhysicsLayers.md` sebelum build.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim dan didengar:** tidak ada. `EnemyHitEvent` membawa `bodyPart` yang diisi di sini.

| Sistem | Hubungan |
|---|---|
| `DamageRouter` (#59) | Memanggil `Apply` |
| `GunController` (#58), `MeleeCombatSystem` | Mengirim damage lewat `DamageRouter` |
| `NPCGunUser` (#58, #76) | Peluru NPC juga lewat `DamageRouter`, jadi headshot NPC ikut 2x |
| `EnemyHealth`, `DamageResistanceTable` (#59) | Membaca `info.bodyPart` untuk armor |
| `EnemyHitReaction` (#95) | Membaca `info.bodyPart`, termasuk efek kena kaki |
| `VFXManager` (#15) | `VFX_BloodHeavy` untuk `BodyPart.Head`, lewat `EnemyHitEvent` |
| `CombatFeedbackUI` (#51) | Skala Hit Marker headshot, lewat `EnemyHitEvent` |
| `PhysicsLayers` (#87) | Collider berada di layer `EnemyHitbox`, masuk mask `Shoot` dan `Melee` |
| `SunlightExposure` (#63) | Mematikan hitbox saat zombie bersembunyi |
| `BaseDefense` (#26) | Jebakan mengisi `bodyPart = Leg` sendiri, tanpa melewati komponen ini |

## 4. Kasus Khusus

- **Pool dipakai ulang:** komponen tidak punya status yang berubah, jadi tidak perlu direset. Layer tulang dikembalikan ke `EnemyHitbox` oleh `EnemyHitReaction.OnSpawned` (#95).
- **Musuh mati dan menjadi ragdoll:** tulang pindah ke layer `Ragdoll`, yang tidak ada di mask `Shoot`. Peluru berikutnya menembus ke belakang dan tidak memanggil `Apply`. `DamageRouter` juga menolak target dengan `IsAlive = false`.
- **Zombie bersembunyi atau terbakar:** hitbox dimatikan `SunlightExposure` (#63), sehingga tidak bisa ditembak dari luar gua.
- **Game dijeda:** tidak ada `Update`. Tidak ada tembakan saat jeda.
- **Ledakan dan api:** `ExplosionDamage` memakai kapsul `Enemy` dan selalu `Torso`, tanpa melewati hitbox.
- **Collider tanpa `HitboxModifier`:** `DamageRouter` tetap memberi damage dengan `Torso` dan pengali 1.
- **Pemain diterkam:** semua input senjata terkunci (#110), kecuali Pisau sebagai cara lepas. Tusukan Pisau dikirim langsung ke `Attacker.DamageTarget`, tanpa raycast, jadi tidak memakai pengali bagian tubuh.
- **Satu tembakan mengenai dua hitbox:** tidak terjadi, karena raycast berhenti pada collider pertama.
- **Save dan permadeath:** tidak ada data tersimpan.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** headshot 2x (#85), urutan hitung damage, armor Zombie Militer, dan `BodyPart` di `GLOSSARY.md`.

**Usulan yang disetujui developer sebagai nilai awal (#85):**

1. **Kaki 0,5x:** berasal dari blueprint lama. GDD tidak menyebutnya. Rekomendasi: pakai sebagai nilai awal.

**Usulan baru (disetujui developer sebagai nilai awal):**

2. **Lengan dihitung `Torso`:** tidak perlu nilai `BodyPart` baru di `GLOSSARY.md`.
3. **Hewan kecil hanya punya hitbox `Torso`:** Gagak dan Ular sulit dibidik kepalanya.
4. **Angka dibaca setiap `Apply`:** perubahan lewat `DevConsole` langsung terasa saat playtest.

## 6. Skenario Tes

1. Tembak kepala, badan, dan kaki zombie standar. **Hasil:** damage 2x, 1x, dan 0,5x.
2. Pukul kepala dengan Pisau. **Hasil:** pengali 2x juga berlaku.
3. Ledakkan tong di dekat zombie. **Hasil:** tidak ada pengali kepala, dan `bodyPart` bernilai `Torso`.
4. Ubah `headMultiplier` lewat `DevConsole` (`balance damage.headMultiplier 3`). **Hasil:** tembakan berikutnya langsung memakai 3x.
5. Cari `TakeDamage` di `HitboxModifier.cs`. **Hasil:** tidak ada.
6. Tembak kepala dan badan Zombie Militer dengan senjata 30 damage. **Hasil:** 60 dan 10,5.
7. Biarkan NPC menembak kepala zombie. **Hasil:** pengali 2x berlaku, dan `EnemyHitEvent.byPlayer` bernilai `false`.
8. Bunuh zombie hingga menjadi ragdoll, lalu tembak mayatnya. **Hasil:** tidak ada `EnemyHitEvent` baru.
9. Kembalikan zombie itu ke pool lalu spawn ulang, kemudian tembak kepalanya. **Hasil:** pengali 2x berlaku lagi.
10. Tembak kepala Serigala, lalu tembak Gagak di bagian mana pun. **Hasil:** Serigala 2x, dan Gagak selalu 1x.
11. Pasang `HitboxModifier` di objek dengan layer `Default`. **Hasil:** peringatan muncul di Console Editor.
12. Atur `legMultiplier` ke -1 lewat konsol, lalu tembak kaki. **Hasil:** damage 0, dan HP musuh tidak naik.
