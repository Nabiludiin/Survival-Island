# Blueprint: EnvironmentalHazard.cs
**Lokasi:** `docs/blueprint/Island_Logic/EnvironmentalHazard.md`
**Deskripsi Utama:** Logika untuk objek interaktif yang berbahaya, seperti Tong Merah (Explosive Barrel).
**Issue:** #59 (bagian ledakan)
**Rujukan GDD:** Usulan, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

Komponen di prefab tong yang diletakkan di peta. Damage ledakan melewati `ExplosionDamage` (`DamagePipeline.md`).

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `explosionRadius` | `float` | 5 | Radius ledakan |
| `explosionDamage` | `float` | 100 | Damage di pusat |
| `explosionForce` | `float` | 500 | Dorongan `Rigidbody` |
| `explosionVFXPoolTag` | `string` | Dari Inspector | Efek ledakan dari `ObjectPooler`, karena efek ini sering muncul |
| `hasExploded` | `bool` | `false` | Mencegah ledakan ganda |

## 2. Detail Logika & Fungsi

**`public void Explode()`**:

1. Kalau `hasExploded`, keluar. Set `hasExploded = true`.
2. `ObjectPooler.Instance.SpawnFromPool(explosionVFXPoolTag, transform.position, transform.rotation)`.
3. `ExplosionDamage.Apply(transform.position, explosionRadius, info)`, dengan `info = DamageInfo { amount = explosionDamage, type = Explosion, source = gameObject, sourceId = "Barrel" }`. Fungsi ini memilih target unik lewat `PhysicsMasks.Explosion` dan menerapkan falloff, sehingga satu musuh hanya kena satu kali.
4. Musuh yang punya `Rigidbody` menerima `AddExplosionForce` dengan `explosionForce`, lewat query terpisah `PhysicsQuery.OverlapSphere` dan `PhysicsMasks.Explosion`.
5. Tong diletakkan di peta dan tidak sering muncul, jadi `Destroy(gameObject)` tetap boleh, dengan statusnya dicatat ke save (#80). Kalau tong dibuat berulang saat bermain, pindahkan ke `ObjectPooler`.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim dan didengar:** tidak ada yang baru. `EnemyHitEvent` dikirim `EnemyHealth` setelah menerima damage.

| Sistem | Hubungan |
|---|---|
| `DamagePipeline` (#59) | `ExplosionDamage.Apply` dan `DamageInfo`. Tong memanggil `Explode` saat `TakeDamage`, kalau tong memakai `IDamageable` |
| `PhysicsLayers` (#87) | `PhysicsMasks.Explosion` |
| `ObjectPooler` | Efek ledakan |
| `WorldStateSave` (#80), `RegionStateStore` (#88) | Status tong yang sudah meledak, lewat `SaveIdTag` |

## 4. Kasus Khusus

- **Pool dipakai ulang:** tong tidak memakai pool. Kalau kelak dipindah ke pool, `hasExploded` harus dikembalikan ke `false`.
- **Game dijeda:** `Explode` hanya dipicu damage, jadi tidak berjalan saat jeda. Efek partikel ikut berhenti.
- **Ledakan berantai:** `ExplosionDamage` dapat mengenai tong lain. `hasExploded` mencegah tong yang sama meledak dua kali.
- **Dinding:** ledakan tidak mengecek garis pandang, dan dinding juga menerima damage (`DamagePipeline.md`).
- **Save dan load:** tong yang sudah meledak tidak dibuat ulang saat region dimuat.
- **Permadeath:** status tong terhapus bersama save run.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** falloff dan aturan bagian tubuh `Torso` (`DamagePipeline.md`).

**Usulan yang menunggu developer (#85):**

1. **Tong Merah:** GDD tidak menyebutnya. Radius 5, damage 100, dan dorongan 500 berasal dari blueprint lama dan sebaiknya pindah ke `GameBalanceConfig` (#84).

## 6. Skenario Tes

1. Tembak tong dengan zombie di pusat. **Hasil:** zombie menerima damage penuh satu kali, dan efek muncul dari pool.
2. Letakkan zombie di tepi radius. **Hasil:** damage menurun sesuai `explosionMinFalloff`.
3. Letakkan dua tong berdekatan, lalu ledakkan satu. **Hasil:** keduanya meledak, masing-masing satu kali.
4. Simpan setelah ledakan, lalu muat. **Hasil:** tong tetap hilang.
5. Mati, lalu mulai run baru. **Hasil:** tong kembali utuh.
