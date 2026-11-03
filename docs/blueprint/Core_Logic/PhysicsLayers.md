# Blueprint: PhysicsLayers (PhysicsMasks.cs & PhysicsQuery.cs)
**Lokasi:** `docs/blueprint/Core_Logic/PhysicsLayers.md`
**Deskripsi Utama:** Aturan layer, tag, tabrakan, dan query fisika yang dipakai semua sistem.
**Issue:** #87
**Rujukan GDD:** Usulan teknis, belum ada di GDD. Mendukung restriksi pantai di bagian 4 dan combat.

## 1. Arsitektur & Variabel Utama

### Daftar layer resmi

| No | Layer | Objek | Catatan |
|---|---|---|---|
| 8 | `Player` | Kapsul `CharacterController` pemain | Tidak masuk mask tembakan pemain |
| 9 | `PlayerViewmodel` | Tangan dan senjata di layar | Hanya dirender kamera tangan (#102), tanpa collider |
| 10 | `Enemy` | Kapsul gerak musuh | Untuk tabrakan dan `NavMeshAgent`, tidak bisa ditembak |
| 11 | `EnemyHitbox` | Collider kepala, badan, dan kaki | Hanya menerima raycast |
| 12 | `NPC` | Kapsul NPC | Menahan peluru, tanpa damage bawaan (#59) |
| 13 | `Interactable` | Peti, sekoci, Diary, Lucky Machine, kargo | Target interaksi `E` |
| 14 | `WorldItem` | Item jatuh | Hanya menyentuh `Terrain`, `Structure`, dan `Obstacle` |
| 15 | `Terrain` | Unity Terrain dan lantai kapal | Permukaan dibaca lewat #86 |
| 16 | `Structure` | Bangunan pemain dan jembatan | Tag `BaseWall` atau `BaseDoor` |
| 17 | `StructureGhost` | Hologram bangunan | Tidak bertabrakan dengan apa pun |
| 18 | `Obstacle` | Pohon, batu, dan benda statis | Node panen juga di sini |
| 19 | `Water` | Permukaan laut, sungai, dan danau | Trigger. Dibaca lewat #13 dan #55 |
| 20 | `ZoneTrigger` | Zona region, trigger audio, kandang Lucky Machine | Selalu trigger |
| 21 | `Projectile` | Fireball dan barang lempar | Peluru pistol memakai raycast |
| 22 | `Ragdoll` | Tulang ragdoll pemain dan musuh | Hanya menyentuh `Terrain`, `Structure`, dan `Obstacle` |
| 23 | `HeavyItem` | Suku cadang helikopter | Bisa diinteraksi, tidak mendorong pemain |

**Layer `Sand/Beach` dihapus.** Satu Terrain hanya punya satu layer, jadi pantai dibedakan lewat `SurfaceDetection` (#86).

### Daftar tag resmi

| Tag | Dipakai untuk |
|---|---|
| `Player` | Pengenal pemain di trigger |
| `MainCamera` | Kamera utama |
| `Zombie` | Trigger kandang (#72) dan sinar matahari (#63) |
| `Animal` | Trigger kandang (#72) |
| `NPC` | Trigger area base |
| `BaseWall` | Dinding, syarat atap, dan target `AttackBuilding` |
| `BaseDoor` | Pintu (#78) |
| `HeavyItem` | Trigger titik pasang helikopter (#14) |

**Batas pemakaian tag:** tag hanya untuk pengenalan cepat di trigger dan satu syarat struktur. Jenis musuh tertentu dibaca dari `EnemyData.enemyId`, bukan lewat tag baru. `FindGameObjectsWithTag` dilarang saat bermain.

### Kumpulan LayerMask bersama

`PhysicsMasks` adalah kelas statis yang menyimpan mask di bawah ini sekali saat game mulai.

| Mask | Layer | Dipakai oleh |
|---|---|---|
| `Shoot` | `EnemyHitbox`, `Terrain`, `Structure`, `Obstacle`, `NPC`, `Interactable`, `HeavyItem` | `GunController` |
| `Melee` | `EnemyHitbox`, `Obstacle`, `Structure`, `NPC` | `MeleeCombatSystem`, panen |
| `Interact` | `Interactable`, `WorldItem`, `HeavyItem`, `NPC` | `PlayerInteractor` (#17) |
| `InteractBlock` | `Terrain`, `Structure`, `Obstacle` | Cek garis pandang interaksi |
| `BuildSurface` | `Terrain`, `Structure` | `BuildManager` |
| `BuildOverlap` | `Obstacle`, `Structure`, `Player`, `Enemy`, `NPC`, `HeavyItem` | `BuildManager` |
| `AIVision` | `Terrain`, `Structure`, `Obstacle` | `AISensory` (#49) |
| `AIAttack` | `Player`, `NPC`, `Structure` | `EnemyAttack` (#109) |
| `Explosion` | `Player`, `Enemy`, `NPC`, `Structure` | Ledakan dan Fireball |
| `Ground` | `Terrain`, `Structure`, `Obstacle` | Cek pijakan, #21, #86 |
| `Sunlight` | `Terrain`, `Structure`, `Obstacle` | `SunlightExposure` (#63), `HydrationSystem` (`RainwaterCollector.RefreshSkyExposure`, #43) |
| `EnemyBody` | `Enemy` | `SleepSystem` (#67) dan `WorldStateSave` (#80), untuk memeriksa musuh di sekitar pemain |
| `CastLanding` | `Water`, `Terrain`, `Structure`, `Obstacle` | `FishingSystem` (#55) |
| `RegrowBlock` | `Structure`, `Player`, `NPC`, `HeavyItem` | `ResourceRespawn` (#73) |
| `Water` | `Water` | `SwimmingController` (#13) |

## 2. Detail Logika & Fungsi

### Aturan query fisika

Trigger raksasa di `ZoneManager` bisa menghentikan raycast, karena secara bawaan `Physics.Raycast` juga mengenai trigger. Oleh karena itu:

- **Aturan wajib:** semua raycast dan overlap memakai `LayerMask` serta `QueryTriggerInteraction.Ignore`.
- **Pengecualian:** query yang memang mencari trigger, misalnya air (#13), harus menulis alasannya di blueprint-nya.
- **Pengganti `Physics.Raycast`:** `PhysicsQuery.Raycast(ray, out hit, distance, mask)` selalu mengirim `QueryTriggerInteraction.Ignore`.
- **Overlap tanpa garbage:** `PhysicsQuery.OverlapSphere(position, radius, mask, buffer)` memakai `OverlapSphereNonAlloc` dan buffer milik pemanggil.
- **Target unik:** `PhysicsQuery.CollectUniqueDamageables(hits, count, results)` mengambil `IDamageable` lewat parent dan membuang duplikat (#59).
- **SphereCast:** `PhysicsQuery.SphereCast(ray, radius, out hit, distance, mask)`, selalu dengan `QueryTriggerInteraction.Ignore` (#17).
- **Kapsul:** `PhysicsQuery.CheckCapsule(bottom, top, radius, mask)`, selalu dengan `QueryTriggerInteraction.Ignore`. Dipakai `PlayerTraversal.CanStandUp` (#101).
- **Kotak:** `PhysicsQuery.OverlapBox(center, halfExtents, rotation, mask, buffer)`, memakai `OverlapBoxNonAlloc` dan `QueryTriggerInteraction.Ignore`. Dipakai validasi `BuildManager` (#79).

**Pengecualian tertulis untuk air:** tiga fungsi ini sengaja memakai `QueryTriggerInteraction.Collide`, karena collider `Water` adalah trigger.

- `PhysicsQuery.RaycastIncludingTriggers(ray, out hit, distance, mask)`, khusus query yang memang mencari collider `Water` (#55).
- `PhysicsQuery.RaycastWater(origin, dir, dist, out hit)`, memakai mask `Water` (#13).
- `PhysicsQuery.OverlapWater(pos, radius, buffer)`, memakai mask `Water` (#13).

### Layer Collision Matrix

`Ya` berarti kedua layer bertabrakan secara fisik. Layer yang tidak disebut pada satu baris tidak bertabrakan dengan layer tersebut.

| Layer | Bertabrakan dengan |
|---|---|
| `Player` | `Enemy`, `NPC`, `Terrain`, `Structure`, `Obstacle`, `Projectile`, `ZoneTrigger`, `Water` |
| `PlayerViewmodel` | Tidak ada |
| `Enemy` | `Player`, `Enemy`, `NPC`, `Terrain`, `Structure`, `Obstacle`, `Projectile`, `ZoneTrigger`, `Water` |
| `EnemyHitbox` | Tidak ada. Hanya raycast |
| `NPC` | `Player`, `Enemy`, `NPC`, `Terrain`, `Structure`, `Obstacle`, `Projectile`, `ZoneTrigger`, `Water` |
| `Interactable` | `Terrain`, `Structure`, `Obstacle` |
| `WorldItem` | `Terrain`, `Structure`, `Obstacle`, `Water` |
| `Terrain` | Semua, kecuali `PlayerViewmodel`, `EnemyHitbox`, `StructureGhost`, `ZoneTrigger`, dan `Water` |
| `Structure` | Sama dengan `Terrain` |
| `StructureGhost` | Tidak ada |
| `Obstacle` | Sama dengan `Terrain` |
| `Water` | `Player`, `Enemy`, `NPC`, `WorldItem`, `HeavyItem` |
| `ZoneTrigger` | `Player`, `Enemy`, `NPC`, `HeavyItem` |
| `Projectile` | `Player`, `Enemy`, `NPC`, `Terrain`, `Structure`, `Obstacle` |
| `Ragdoll` | `Terrain`, `Structure`, `Obstacle` |
| `HeavyItem` | `Terrain`, `Structure`, `Obstacle`, `Water`, `ZoneTrigger` |

### Bagian tubuh dan pergerakan

Musuh memakai dua lapis collider:

1. **Kapsul gerak di layer `Enemy`:** berada di objek utama, mengikuti `NavMeshAgent`, dan tidak termasuk mask `Shoot`.
2. **Collider tubuh di layer `EnemyHitbox`:** bukan trigger, mengikuti tulang, dan memiliki `HitboxModifier`. Matriks membuatnya tidak bertabrakan dengan apa pun, sehingga tidak mengganggu pergerakan.

Karena collider tubuh bukan trigger, raycast dengan `QueryTriggerInteraction.Ignore` tetap mengenainya.

### Ragdoll, item jatuh, dan NPC

- **Ragdoll:** tulang berpindah ke layer `Ragdoll` saat mati, supaya pemain dan musuh tidak tersandung mayat.
- **Item jatuh:** `WorldItem` tidak menyentuh pemain, jadi tidak mendorong atau menghalangi jalan.
- **NPC:** bertabrakan dengan pemain supaya terasa nyata. Agen NPC memakai `avoidancePriority` lebih rendah agar memberi jalan (#76).

### Pemeriksaan Editor

`LayerAuditWindow` adalah alat Editor yang memeriksa scene dan prefab, lalu melaporkan:

- Objek dengan `Terrain` Unity yang tidak berada di layer `Terrain`.
- Collider trigger di luar layer `ZoneTrigger` dan `Water`.
- `HitboxModifier` di luar layer `EnemyHitbox`.
- Objek `NavMeshAgent` di luar layer `Enemy` dan `NPC`.
- Script yang masih memanggil `Physics.Raycast` tanpa `PhysicsQuery`, lewat pencarian teks.
- Komponen `IInteractable` yang tidak punya collider di layer `Interactable`, `WorldItem`, `HeavyItem`, atau `NPC` (#17).

## 3. Hubungan dengan Sistem Lain

- **Tidak mengirim atau mendengar event.** `PhysicsMasks` dan `PhysicsQuery` adalah kelas statis.
- **#86 `SurfaceDetection`:** memakai `PhysicsMasks.Ground`, lalu membaca jenis permukaan.
- **#59 `DamagePipeline`:** memakai `CollectUniqueDamageables`.
- **Semua sistem raycast:** `GunController`, `MeleeCombatSystem`, `PlayerInteractor`, `BuildManager`, dan `AISensory`.

## 4. Kasus Khusus

- **Objek pool:** saat kembali dipakai, tulang ragdoll harus kembali ke layer `EnemyHitbox`. Lihat #95.
- **Game dijeda:** query fisika tetap berjalan, tetapi tidak ada pemanggilnya karena input terkunci (#83).
- **Permadeath dan save:** tidak ada data yang disimpan.
- **Pemain di dalam trigger:** `QueryTriggerInteraction.Ignore` juga berlaku, jadi raycast tidak terganggu saat pemain berada di dalam zona.
- **Kapsul musuh menghalangi tembakan:** kapsul `Enemy` tidak ada di mask `Shoot`, jadi peluru langsung mengenai `EnemyHitbox` di dalamnya.

## 5. Keputusan Desain

**Keputusan yang dibuat di blueprint ini (usulan teknis, dicatat di #85):**

- NPC menahan peluru tanpa menerima damage. Aturan friendly fire final ada di #59.
- Nomor layer 8 sampai 23 memakai slot layer buatan Unity.

**Menunggu developer:** tidak ada yang menghalangi.

## 6. Skenario Tes

1. Tembak musuh yang berdiri di dalam zona region. **Hasil:** peluru mengenai musuh, bukan trigger zona.
2. Tembak kepala, badan, dan kaki. **Hasil:** pengali 2x, 1x, dan 0,5x sesuai `HitboxModifier`.
3. Berjalan melewati ragdoll dan item jatuh. **Hasil:** pemain tidak tersandung.
4. Arahkan hologram ke pemain atau musuh. **Hasil:** hologram merah, tanpa menabrak apa pun.
5. Jalankan `LayerAuditWindow` pada prefab yang sengaja diberi layer salah. **Hasil:** prefab masuk laporan.
6. Ledakkan tong di dekat musuh dengan tiga hitbox. **Hasil:** musuh hanya menerima satu kali damage.
