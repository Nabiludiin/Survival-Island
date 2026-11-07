# Blueprint: EnvironmentalHazard.cs
**Lokasi:** `Survival Island/doc/blueprint/Island_Logic/EnvironmentalHazard.md`
**Deskripsi Utama:** Logika untuk objek interaktif yang berbahaya, seperti Tong Merah (Explosive Barrel). Tong meledak saat ditembak, terkena ledakan, atau terbakar.
**Issue:** #59 (bagian ledakan)
**Rujukan GDD:** Usulan, belum ada di GDD. GDD bagian 3 hanya menyebut barang lempar (Molotov, Bom Rakitan) untuk mengendalikan kerumunan zombie. Tong Merah adalah tambahan bergaya L4D.

## 1. Arsitektur & Variabel Utama

`EnvironmentalHazard : MonoBehaviour, IDamageable, ISaveable` di prefab tong yang diletakkan tangan di scene region. Damage ledakan melewati `ExplosionDamage` (`DamagePipeline.md`). Status tersimpan lewat `RegionStateStore` (#88), dengan `SaveId` dari `SaveIdTag` (#80), sama seperti peti loot.

**Batas tanggung jawab:**

| Pekerjaan | Pemilik |
|---|---|
| Kapan tong meledak, sumbu, dan status meledak | `EnvironmentalHazard` |
| Mencari target, falloff, dan friendly fire | `ExplosionDamage`, target masing-masing (#59) |
| Dorongan dan ragdoll musuh | `EnemyHitReaction` (#95), dari `DamageType.Explosion` |
| Dorongan ragdoll pemain saat mati | `DeathCinematic` (#53) |
| Tong ikut terbakar dari api | `FireSystem` (#97), lewat `Flammable` di prefab |

**Prefab:** collider tong berada di layer `Obstacle`, jadi bisa ditembak (`PhysicsMasks.Shoot`) dan menghalangi gerak. Layer ini tidak ada di `PhysicsMasks.Explosion`, sehingga ledakan berantai tidak memakai query fisika (lihat `NotifyExplosion`). Prefab juga memasang `SaveIdTag`, `NoiseEmitter` (#49), dan `Flammable` dengan profil `Flame_Barrel` (usulan, #97).

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `hazardId` | `string` | `"Barrel"` | Mengisi `DamageInfo.sourceId`, untuk statistik dan layar akhir |
| `explosionVFXPoolTag` | `string` | `VFX_Explosion` | Efek ledakan lewat `VFXManager.Play` (usulan tag baru) |
| `explosionSFX` | `AudioClip` | Dari Inspector | Suara ledakan |
| `visualRoot` | `GameObject` | Dari prefab | Model dan collider yang dimatikan setelah meledak |
| `hasExploded` | `bool` | `false` | Mencegah ledakan ganda. Disimpan |
| `isArmed` | `bool` | `false` | Sumbu sedang berjalan. Tidak disimpan |
| `armedByPlayer` | `bool` | `false` | `byPlayer` dari damage pemicu, diteruskan ke ledakan |
| `IsAlive` | `bool` | `!hasExploded` | Bagian `IDamageable` |

`static readonly List<EnvironmentalHazard> active` berisi tong yang belum meledak dan sedang dimuat. Tong mendaftar di `OnEnable` dan keluar di `OnDisable`.

**Angka penyetelan (usulan bagian baru `GameBalanceConfig.hazard`):**

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.hazard.barrelRadius` | 5 | Radius ledakan. Sedikit lebih kecil dari Bom Rakitan (6) |
| `GameBalanceConfig.hazard.barrelDamage` | 100 | Damage di pusat. Sedikit lebih kecil dari Bom Rakitan (120) |
| `GameBalanceConfig.hazard.chainDelaySeconds` | 0,15 | Jeda sebelum tong lain ikut meledak |
| `GameBalanceConfig.hazard.fireFuseSeconds` | 2 | Jeda setelah tong mulai terbakar |
| `GameBalanceConfig.hazard.noiseRadius` | 40 | Radius suara, sama dengan Bom Rakitan |

**Dihapus:** `explosionForce` dan `AddExplosionForce`. Musuh memakai `NavMeshAgent` tanpa `Rigidbody` dinamis, dan dorongan sudah diatur `EnemyHitReaction` (`explosionKnockback`). Angka lama `explosionRadius` dan `explosionDamage` di komponen pindah ke config. `Destroy(gameObject)` juga dihapus (lihat langkah 7 di `Detonate`).

## 2. Detail Logika & Fungsi

### Pemicu

**`public void TakeDamage(in DamageInfo info)`**:

1. Kalau `hasExploded` atau `isArmed`, abaikan.
2. Kalau `info.amount <= 0`, abaikan.
3. Pilih jalur menurut `info.type`:

| `info.type` | Hasil |
|---|---|
| `Bullet`, `Explosion` | `Arm(info.byPlayer, 0)`. Meledak pada frame berikutnya |
| `Fire` | `Arm(info.byPlayer, fireFuseSeconds)`. Tong mendesis lalu meledak |
| `Melee`, `Structural`, dan lainnya | Diabaikan. Tong tidak meledak karena dipukul Pisau atau zombie (usulan) |

**`private void Arm(bool byPlayer, float delay)`**:

1. `isArmed = true`, dan `armedByPlayer = byPlayer`.
2. Jalankan coroutine `FuseRoutine(delay)`. Untuk `delay = 0`, coroutine tetap menunggu satu frame dengan `yield return null`.

**Alasan menunggu satu frame:** `ExplosionDamage` memakai buffer statis. Kalau tong meledak langsung di dalam `TakeDamage` yang dipanggil `ExplosionDamage.Apply`, buffer akan ditimpa saat masih dipakai. Menunda ledakan menghindari rekursi ini.

**`private IEnumerator FuseRoutine(float delay)`**: tunggu `delay` dengan `WaitForSeconds` (ikut `Time.timeScale`), lalu panggil `Detonate()`.

### Ledakan

**`private void Detonate()`**:

1. Kalau `hasExploded`, keluar. Set `hasExploded = true`, `isArmed = false`, lalu keluar dari `active`.
2. `VFXManager.Instance.Play(explosionVFXPoolTag, transform.position, Quaternion.identity)` (#15).
3. `AudioManager.Instance.PlaySFX(explosionSFX)`, dan `CameraShake.PlayAt(CameraShakeKind.Explosion, transform.position)` (#24).
4. Isi `info = DamageInfo { amount = barrelDamage, type = Explosion, source = gameObject, sourceId = hazardId, hitPoint = transform.position, bodyPart = Torso, byPlayer = armedByPlayer }`.
5. `ExplosionDamage.Apply(transform.position, barrelRadius, info)`. Fungsi ini memilih target unik lewat `PhysicsMasks.Explosion`, menerapkan falloff, dan menjalankan aturan friendly fire, sehingga satu musuh hanya kena satu kali.
6. `NoiseEmitter.Emit(noiseRadius, NoiseSourceKind.Throwable)` (#49). Belum ada `NoiseSourceKind` khusus ledakan, jadi tong memakai jenis yang sama dengan Bom Rakitan.
7. Matikan `visualRoot` dan `Flammable`. Objek akar tetap ada tetapi nonaktif, karena `RegionStreaming.CaptureRegion` memakai daftar `ISaveable` yang dikumpulkan saat region dimuat. Objek yang di-`Destroy` tidak bisa melaporkan statusnya.
8. Ledakan berantai tidak dipanggil di sini, karena `ExplosionDamage.Apply` di langkah 5 sudah memanggil `NotifyExplosion` (`DamagePipeline.md`).

### Ledakan berantai

**`public static void NotifyExplosion(Vector3 center, float radius, bool byPlayer)`**:

1. Untuk tiap tong di `active`, hitung jarak kuadrat ke `center`.
2. Kalau di dalam `radius`, dan tong belum meledak atau bersumbu, panggil `Arm(byPlayer, chainDelaySeconds)`.
3. Daftar disalin ke buffer statis sebelum diulang, karena `Arm` bisa mengubah `active` lewat ledakan berikutnya.

Fungsi ini dipanggil tong sendiri, dan sebaiknya juga oleh `ExplosionDamage.Apply`, supaya Bom Rakitan (#96) dan Fireball (#70) ikut meledakkan tong. Perubahan itu perlu dicatat di `DamagePipeline.md`.

Daftar tong pendek (hanya region yang dimuat), jadi perulangan tanpa query fisika ini murah dan tanpa alokasi.

### `byPlayer` dan terkaman

`armedByPlayer` diteruskan dari damage pemicu, termasuk lewat ledakan berantai:

| Pemicu | `byPlayer` | Akibat |
|---|---|---|
| Peluru atau Bom Rakitan pemain | `true` | Pemain dan NPC menerima `playerExplosionSelfMultiplier` (0,5). Kematian musuh masuk statistik pemain |
| Peluru NPC | `false` | Damage penuh ke semua pihak |
| Fireball, Molotov lingkungan, atau api menjalar dari sumber bukan pemain | `false` | Damage penuh ke semua pihak |

Sesuai keputusan developer, hanya damage pemain yang membatalkan terkaman Jaguar atau jerat Zombie Tribal (#110). Tong yang diledakkan pemain bisa melepas pemain dari terkaman. Tong yang diledakkan Fireball atau NPC tidak melepasnya, walaupun penyerang terluka.

### Save

**`string CaptureState()`**: `{ hasExploded }`.

**`void RestoreState(string json)`**: isi `hasExploded`. Kalau `true`, matikan `visualRoot` dan `Flammable`, lalu keluar dari `active`.

**`void OnRestoreFinished()`**: tidak mengirim event.

`SaveId` dibaca dari `SaveIdTag`, dan `LoadOrder` bernilai 0, sesuai aturan objek region di `WorldStateSave.md`.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `NoiseEmittedEvent`, lewat `NoiseEmitter`. `EnemyHitEvent`, `EnemyKilledEvent`, dan `PlayerDamagedEvent` dikirim oleh target, bukan oleh tong.

**Event yang didengar:** tidak ada.

| Sistem | Hubungan |
|---|---|
| `DamagePipeline` (#59) | `IDamageable`, `ExplosionDamage.Apply`, `DamageInfo`, falloff, dan friendly fire |
| `GunController` (#58) | Peluru mengenai collider `Obstacle`, lalu `DamageRouter.ApplyHit` menemukan tong lewat parent |
| `ThrowableSystem` (#96), `DisasterManager` (#70) | Ledakan Bom Rakitan dan Fireball memicu tong lewat `NotifyExplosion` |
| `FireSystem` (#97) | `Flammable` mengirim damage `Fire`, dan memulai sumbu |
| `EnemyHitReaction` (#95) | Dorongan dan ragdoll musuh |
| `PlayerPinnedState` (#110) | Hanya ledakan dengan `byPlayer = true` yang dihitung sebagai damage pemain |
| `AISensory` (#49) | Suara ledakan memanggil zombie |
| `VFXManager` (#15), `AudioManager`, `CameraShake` (#24) | Efek, suara, dan getaran |
| `PhysicsLayers` (#87) | Layer `Obstacle`, `PhysicsMasks.Shoot`, dan `PhysicsMasks.Explosion` |
| `WorldStateSave` (#80), `RegionStateStore` (#88) | Status `hasExploded`, lewat `SaveIdTag` |

## 4. Kasus Khusus

- **Pool dipakai ulang:** tong tidak memakai pool, karena diletakkan di peta dan jumlahnya sedikit. Efek ledakan memakai pool lewat `VFXManager`. Kalau kelak tong dibuat saat bermain, `OnSpawned` wajib mengembalikan `hasExploded = false`, `isArmed = false`, `armedByPlayer = false`, menyalakan `visualRoot`, dan mendaftar ke `active`.
- **Game dijeda:** sumbu memakai `WaitForSeconds`, jadi berhenti saat jeda. Efek partikel ikut berhenti.
- **Ledakan berantai:** `hasExploded` dan `isArmed` mencegah tong yang sama meledak dua kali. Jeda `chainDelaySeconds` membuat rantai terlihat beruntun.
- **Rekursi buffer:** ledakan tidak pernah dipanggil di dalam `TakeDamage` pada frame yang sama.
- **Dinding:** ledakan tidak mengecek garis pandang, dan dinding juga menerima damage (`DamagePipeline.md`).
- **Region dilepas saat sumbu berjalan:** objek nonaktif menghentikan coroutine. `isArmed` tidak disimpan, jadi tong utuh lagi saat region dimuat kembali (usulan, supaya tidak ada ledakan tanpa saksi).
- **Tong di dalam base pemain:** tong hanya diletakkan level designer di peta. Pemain tidak bisa membawanya.
- **Pemain diterkam di dekat tong:** lihat bagian `byPlayer` di atas.
- **Mati dan menang pada frame yang sama:** ditangani `GameManager.IsRunEnding` (#82).
- **Save dan load:** tong yang sudah meledak tetap nonaktif saat region dimuat.
- **Permadeath:** status tong terhapus bersama save run, sehingga run baru memuat tong utuh.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** falloff, friendly fire, dan aturan bagian tubuh `Torso` (`DamagePipeline.md`). Hanya damage pemain yang melepas terkaman atau jerat (keputusan developer, `PlayerPinnedState.md`).

**Usulan yang disetujui developer sebagai nilai awal (#85):**

1. **Tong Merah:** GDD tidak menyebutnya. Radius 5 dan damage 100 berasal dari blueprint lama, dan dipindah ke `GameBalanceConfig.hazard` (#84).

**Usulan baru (disetujui developer sebagai nilai awal):**

2. **Pemicu:** peluru, ledakan, dan api memicu tong. Pukulan melee dan serangan zombie tidak.
3. **Sumbu api 2 detik:** memberi waktu pemain menjauh saat Molotov mengenai tong.
4. **Dorongan `Rigidbody` dihapus:** dorongan musuh sudah dari #95.
5. **Ledakan berantai lewat daftar, bukan layer:** tidak perlu menambah `Obstacle` ke `PhysicsMasks.Explosion`.
6. **`NoiseSourceKind.Throwable` untuk tong:** atau tambah nilai baru `Explosion` di `GLOSSARY.md`.
7. **Sumbu yang terputus karena region dilepas tidak disimpan.**

## 6. Skenario Tes

1. Tembak tong dengan zombie di pusat. **Hasil:** zombie menerima damage penuh satu kali, dan efek muncul dari pool.
2. Letakkan zombie di tepi radius. **Hasil:** damage menurun sesuai `explosionMinFalloff`.
3. Letakkan dua tong berdekatan, lalu ledakkan satu. **Hasil:** keduanya meledak dengan jeda sekitar 0,15 detik, masing-masing satu kali.
4. Simpan setelah ledakan, lalu muat. **Hasil:** tong tetap hilang.
5. Mati, lalu mulai run baru. **Hasil:** tong kembali utuh.
6. Pukul tong dengan Pisau. **Hasil:** tidak meledak.
7. Lempar Molotov ke tong. **Hasil:** tong mendesis, lalu meledak sekitar 2 detik kemudian.
8. Lempar Bom Rakitan di dekat tong. **Hasil:** tong ikut meledak lewat `NotifyExplosion`.
9. Tembak tong sejauh separuh radius dari pemain. **Hasil:** pemain menerima sekitar 31 damage (100 x 0,625 x 0,5).
10. Biarkan NPC menembak tong di dekat pemain. **Hasil:** pemain menerima damage penuh setelah falloff, dan `byPlayer` bernilai `false`.
11. Lempar Bom Rakitan ke dekat tong, lalu biarkan Jaguar menerkam pemain di dekatnya sebelum bom meledak. Ulangi dengan Fireball (`disaster Eruption`) yang meledakkan tong. **Hasil:** ledakan berantai dari bom pemain melepas terkaman, sedangkan ledakan dari Fireball tidak.
12. Ledakkan tong saat zombie berada 30 meter jauhnya tanpa target. **Hasil:** zombie menuju titik ledakan.
13. Jeda game saat sumbu api berjalan selama 10 detik nyata. **Hasil:** tong tidak meledak sampai jeda selesai.
14. Ledakkan tong, lalu jalan menjauh sampai region dilepas dan kembali lagi. **Hasil:** tong tetap hilang.
15. Ledakkan lima tong berantai sambil membuka Profiler. **Hasil:** tidak ada alokasi GC dari `NotifyExplosion`, dan tidak ada error buffer.

## 7. Catatan Implementasi (Sistem pendukung)

- Kode: `Assets/LOGIC/Island_Logic/EnvironmentalHazard.cs`, config `GameBalanceConfig.hazard` di `GameBalance.GrupE.cs`.
- Langkah 8 `Detonate`: selama `ExplosionDamage.Apply` belum memanggil `NotifyExplosion` (TODO lama di `ExplosionDamage.cs`), tong memanggil `NotifyExplosion` sendiri setelah `Apply`. Tong yang sudah bersumbu diabaikan, jadi pemanggilan ganda aman setelah `ExplosionDamage` diperbarui.
- `Arm` diabaikan kalau objek tidak aktif, karena coroutine tidak bisa berjalan di objek nonaktif.
- Kalau prefab tidak memasang `NoiseEmitter`, suara memakai `NoiseEmitter.Emit(posisi, radius, kind)` statis.
- Belum diuji di Unity.
