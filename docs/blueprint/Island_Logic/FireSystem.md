# Blueprint: FireSystem.cs & Flammable.cs
**Lokasi:** `docs/blueprint/Island_Logic/FireSystem.md`
**Deskripsi Utama:** Aturan bersama untuk api yang membakar bangunan kayu, musuh, dan vegetasi: area api sementara, penjalaran berkala, pengaruh cuaca, dan cara memadamkan.
**Issue:** #97
**Rujukan GDD:** Bagian 5 (Erupsi, batu pijar menghancurkan area sekitar) dan bagian 6 (bencana memaksa perbaikan). Aturan api menjalar dan semua angkanya adalah usulan, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

| Kelas | Jenis | Tugas |
|---|---|---|
| `FireSystem` | Manager di `IslandPhase`, `ISaveable` | Daftar api aktif, tick damage dan penjalaran, batas jumlah, dan API untuk sistem lain |
| `Flammable` | Komponen, `IPoolable` | Status bakar satu objek. Ada di prefab struktur, `ResourceNode` pohon, zombie, dan hewan |
| `FlammableProfile` | ScriptableObject | Ketahanan, durasi, dan peluang menjalar per material |
| `FireArea` | Objek pool, tag `FX_FireArea` | Area api di tanah dengan damage per detik |
| `BurnEffect` | Objek pool, tag `FX_FireBurn` | Partikel, cahaya, dan collider padam untuk satu `Flammable` |
| `FireExtinguishTarget` | Komponen di kedua objek pool, memakai `IInteractable` | Memadamkan dengan tombol `E` |

**Bukan milik sistem ini:** terbakar sinar matahari (#63) dan jalur damage dasar (#59). `FireSystem` hanya memanggil `IDamageable.TakeDamage` dengan `DamageType.Fire`.

### Data `FlammableProfile`

| Profil | `fireResistance` | `burnSeconds` | `burnDamagePerSec` | `spreadChancePerTick` | `spreadRadius` | Akhir bakar |
|---|---|---|---|---|---|---|
| `Flame_WoodStructure` | 0,3 | 30 | 6 | 0,05 | 3 | Padam, HP tetap berkurang |
| `Flame_Stone` | 1 | 0 | 0 | 0 | 0 | Tidak pernah menyala |
| `Flame_Vegetation` | 0,2 | 20 | 0 | 0,08 | 4 | `ResourceNode.ForceDeplete(false)` |
| `Flame_Zombie` | 0 | 8 | 10 | 0,10 | 2 | Padam |
| `Flame_Animal` | 0,3 | 6 | 8 | 0 | 0 | Padam |
| `Flame_Barrel` (usulan, `EnvironmentalHazard.md`) | 0 | 3 | 5 | 0 | 0 | Tong meledak. Damage `Fire` pertama memulai sumbu `hazard.fireFuseSeconds` (2), lalu `EnvironmentalHazard` mematikan `Flammable` saat meledak |

- `fireResistance = 1` berarti kebal. `Wall_Stone` dan `Door_Stone` memakai `Flame_Stone`, selaras dengan pengali `Fire` 0 di `BuildingIntegrity.md`. Upgrade dinding (#47) otomatis mengganti profil karena prefabnya berganti.
- Dinding kayu terbakar penuh kehilangan 180 dari 300 HP, jadi satu kali api merusak berat, tetapi belum meruntuhkan.
- Pemain dan NPC tidak punya `Flammable`. Mereka hanya menerima damage selama berada di `FireArea`.

### Variabel `Flammable`

`profile`, `IsBurning`, `burnRemaining`, `reigniteAt` (waktu paling cepat bisa menyala lagi), `generation` (jumlah lompatan dari sumber), `byPlayer`, `isRoofed`, `homeRegion`, `damageable` (`IDamageable`), dan `activeEffect` (`BurnEffect`).

### Angka penyetelan (bagian config baru `GameBalanceConfig.fire`)

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `fire.maxBurning` | 24 | Batas `Flammable` menyala bersamaan |
| `fire.maxAreas` | 8 | Batas `FireArea` aktif |
| `fire.maxCarvedAreas` | 4 | Batas `FireArea` yang memotong NavMesh |
| `fire.burnTickSeconds` | 0,5 | Jeda damage bakar |
| `fire.spreadTickSeconds` | 1 | Jeda satu giliran penjalaran |
| `fire.spreadChecksPerTick` | 6 | Jumlah api yang memeriksa sekitarnya per giliran |
| `fire.areaTickSeconds` | 0,5 | Jeda damage `FireArea` |
| `fire.areaIgniteChancePerTick` | 0,25 | Peluang `FireArea` menyalakan `Flammable` di dalamnya |
| `fire.reigniteCooldownSeconds` | 20 | Jeda sebelum objek yang sama bisa menyala lagi |
| `fire.maxVegetationGeneration` | 3 | Batas lompatan api antar-vegetasi |
| `fire.rainSpreadMultiplier` / `rainBurnMultiplier` | 0,25 / 0,5 | Pengaruh `Rain` pada api tak beratap |
| `fire.snowSpreadMultiplier` | 0,5 | Pengaruh `Snow` pada api tak beratap |
| `fire.stormAreaMultiplier` | 0,25 | Sisa durasi `FireArea` tak beratap saat `Storm` atau `Blizzard` |
| `fire.roofRecheckSeconds` | 5 | Jeda cek ulang atap |
| `fire.extinguishHoldSeconds` | 1 | Lama menahan `E` untuk memadamkan |
| `fire.fireballAreaRadius` / `fireballAreaSeconds` / `fireballDamagePerSec` | 4 / 12 / 8 | Area api dari Fireball (#70) |
| `fire.fireAxeIgniteChance` | 0,25 | Peluang Fire Axe menyalakan target per pukulan |
| `fire.campfireTrapIgniteChancePerTick` | 0,1 | Peluang jebakan api unggun (#26) menyalakan musuh |

## 2. Detail Logika & Fungsi

### API `FireSystem`

**`public FireArea SpawnFireArea(Vector3 pos, float radius, float durationSeconds, float damagePerSec, bool byPlayer, bool carveNavMesh = false)`**, sesuai pemanggilan di `ThrowableSystem.md`:

1. Kalau `FireArea` aktif sudah `maxAreas`, kembalikan area dengan sisa waktu terkecil ke pool. Api baru selalu menang, supaya Molotov tidak gagal.
2. Ambil `FX_FireArea` dari `ObjectPooler`, lalu isi data.
3. Cek atap lewat `PhysicsQuery.Raycast` ke atas dengan `PhysicsMasks.Sunlight`. Kalau tak beratap, kali durasi sesuai cuaca di tabel bawah.
4. Kalau `carveNavMesh` dan jumlah area carve di bawah `maxCarvedAreas`, nyalakan `NavMeshObstacle` (`carving = true`, `carveOnlyStationary = true`).

**`public bool IsNearFire(Vector3 pos, float margin)`** memeriksa semua `FireArea` aktif dengan jarak kuadrat, tanpa query fisika. Dipakai hewan dan NPC untuk menghindar.

### `Flammable`

**`public bool TryIgnite(float chance, bool byPlayer, int generation = 0)`**:

1. Tolak kalau `IsBurning`, `fireResistance >= 1`, `Time.time < reigniteAt`, atau `FireSystem` sudah `maxBurning`.
2. Kalau profil `Flame_Vegetation` dan `generation > maxVegetationGeneration`, tolak. Ini mencegah satu percikan menghabiskan satu hutan dan material pemain.
3. Lempar peluang `chance * (1 - fireResistance)` dengan aliran `Gameplay` (#89). Kalau gagal, selesai.
4. Isi `IsBurning`, `burnRemaining = burnSeconds`, `homeRegion = RegionStreaming.Instance.GetRegionAt(pos)`, cek atap, lalu ambil `FX_FireBurn`.
5. Daftar ke `FireSystem`. Kalau objek struktur, terbitkan `StructureBurningEvent { structureId, isBurning = true }`.

**`public void Extinguish(bool silent = false)`**: `IsBurning = false`, `reigniteAt = Time.time + reigniteCooldownSeconds`, kembalikan efek ke pool, keluar dari daftar. Kalau struktur dan `!silent`, terbitkan `StructureBurningEvent` dengan `isBurning = false`.

### Tick di `FireSystem.Update()`

Kalau `Time.deltaTime == 0`, selesai.

1. **Efek:** setiap frame, salin posisi `Flammable` musuh ke `BurnEffect`. Efek tidak dijadikan child, karena Unity melarang mengganti parent saat objek sedang dinonaktifkan.
2. **Damage bakar, setiap `burnTickSeconds`:** untuk tiap `Flammable` menyala, kirim `DamageInfo { amount = burnDamagePerSec * burnTickSeconds, type = Fire, sourceId = "fire", byPlayer, hitPoint = pos }`, lalu kurangi `burnRemaining`. Saat habis, jalankan aturan "Akhir bakar".
3. **Penjalaran, setiap `spreadTickSeconds`:** proses `spreadChecksPerTick` api secara bergilir:
   - Baca cuaca `RegionalWeather.Instance.GetWeather(homeRegion)`. Kalau peluangnya 0, lewati.
   - `PhysicsQuery.OverlapSphere(pos, spreadRadius, PhysicsMasks.FireReach, buffer)` dengan buffer 32.
   - Untuk tiap hasil, `GetComponentInParent<Flammable>()`, lalu `TryIgnite(spreadChance * pengaliCuaca, byPlayer, generation + 1)`.
4. **Status tubuh musuh:** kalau `EnemyHealth.IsAlive == false`, `AIState` menjadi `Submerged` atau `BurnToAshes`, panggil `Extinguish(true)`. Api padam di air, dan efek matahari (#63) tidak ditumpuk.

### `FireArea`

Setiap `areaTickSeconds`:

1. `PhysicsQuery.OverlapSphere(pos, radius, PhysicsMasks.FireReach, buffer)`.
2. Untuk tiap `Flammable` yang ditemukan: `TryIgnite(areaIgniteChancePerTick, byPlayer)`.
3. `PhysicsQuery.CollectUniqueDamageables`. Target selain `BaseHealth` menerima `damagePerSec * areaTickSeconds` dengan `type = Fire`. Struktur tidak menerima damage area, supaya tidak dihitung dua kali dengan damage bakar.

Saat durasi habis, kembali ke pool.

**Efek ke NavMesh:**

| Sumber | Carve | Akibat |
|---|---|---|
| Molotov (#96) | Tidak | Zombie menerobos dan terbakar, seperti L4D |
| Fireball (#70) | Ya | Agent mencari jalan memutar selama api menyala |
| Struktur dan pohon terbakar | Tidak perlu | Sudah menjadi obstacle (#78, #73) |

Hewan dan NPC memanggil `IsNearFire` pada tick mereka, supaya tidak berjalan ke area Molotov.

### Material

Kayu menyala lewat profil `Flame_WoodStructure`, dan batu kebal lewat `Flame_Stone`. Semua prefab bangunan kayu dari #79, #44, #46, #67, dan #98 wajib memasang `Flammable`. Vegetasi di sini berarti pohon dan tanaman `ResourceNode`. Rumput pada Terrain tidak bisa terbakar.

### Cuaca

Atap dicek saat menyala, lalu setiap `roofRecheckSeconds`, karena atapnya bisa ikut hancur. Raycast mulai 0,2 meter di atas bounds objek, supaya tidak mengenai dirinya sendiri.

| Cuaca (tak beratap) | `Flammable` | `FireArea` |
|---|---|---|
| `Clear`, `Fog` | Normal | Normal |
| `Rain` | Penjalaran dikali 0,25, `burnRemaining` berkurang 2 kali lebih cepat | Durasi dikali 0,5 |
| `Snow` | Penjalaran dikali 0,5 | Normal |
| `Storm`, `Blizzard` | Padam pada tick berikutnya | Durasi dikali 0,25, tanpa menyalakan objek |

Aturan `Storm` dan `Blizzard` sama dengan `CampfireManager`. Objek beratap tidak terpengaruh.

### Zombie terbakar

- Zombie yang menyala tetap mengejar seperti biasa, dan tidak memakai `BurnToAshes`.
- Selama 8 detik, zombie menerima damage dan bisa menyalakan kayu dalam 2 meter. Zombie yang terbakar saat menyerang dinding kayu berisiko membakar base, jadi Molotov di dekat base punya harga.
- Sumber yang bisa menyalakan zombie: `FireArea`, Fire Axe, dan jebakan api unggun (#26).
- `EnemyData.resistances` tetap berlaku pada damagenya.

### Sumber api lain

| Sumber | Aturan |
|---|---|
| Fireball (#70) | Saat mendarat, `SpawnFireArea(pos, fireballAreaRadius, fireballAreaSeconds, fireballDamagePerSec, false, true)` |
| Molotov (#96) | `SpawnFireArea` dengan data `ThrowableData`. Nilai `damage` berarti damage per detik |
| Fire Axe (#72) | `Flammable.TryIgnite(fireAxeIgniteChance, true)` saat mengenai target |
| Api unggun (#44) | Tidak menjalar. Hanya jebakan api unggun (#26) yang bisa menyalakan musuh |
| Obor pegang (#65), Obor Dinding (#98) | Tidak menyalakan apa pun |

### Memadamkan

`FireExtinguishTarget` berada pada collider kecil berlayer `Interactable` di `FX_FireBurn` dan `FX_FireArea`, sehingga terbaca `PhysicsMasks.Interact`. Collider ini dimatikan kalau pemiliknya musuh. Kelas ini memakai `IInteractable` langsung, bukan `IslandInteractable`, karena api tidak punya `SaveId` sendiri.

- `HoldSeconds = extinguishHoldSeconds`, prompt `Padamkan (1 air)`.
- Saat selesai, kurangi 1 `Drink_DirtyWater`. Kalau tidak ada, kurangi 1 `Drink_CleanWater`. Kalau keduanya kosong, tampilkan `Butuh air` dan api tetap menyala.
- Panggil `Flammable.Extinguish()` atau kembalikan `FireArea` ke pool.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `StructureBurningEvent` (`structureId`, `isBurning`, baru, tidak sticky). Damage tetap memicu `StructureDamagedEvent` dan `EnemyKilledEvent` dari pemiliknya.

**Event yang didengar:** `RunEndedEvent` (padamkan semua, kembalikan ke pool).

| Sistem | Hubungan |
|---|---|
| `DamagePipeline` (#59), `BuildingIntegrity` (#47) | `TakeDamage` tipe `Fire`. Pengali `Fire` 1 dan 0 tetap dipakai |
| `RegionalWeather` (#115) | `GetWeather(region)` |
| `ResourceRespawn` (#73) | `ForceDeplete(false)` |
| `ThrowableSystem` (#96), `DisasterManager` (#70), `MeleeCombatSystem` (#72), `BaseDefense` (#26) | Pemicu api |
| `LightSourceSystem` (#98) | `FireArea` dan `BurnEffect` memakai `LightSource`, sehingga ikut batas lampu |
| `ItemUseSystem` (#66), `InventoryManager`, `IInteractable` (#17) | Memakai air untuk memadamkan |
| `WorldStateSave` (#80), `StructureRegistry` (#79) | HP tersimpan di `StructureRecord`, status bakar di `world_fire` |
| `NotificationManager` (#22) | Mendengar `StructureBurningEvent`, menampilkan `Base terbakar` |

## 4. Kasus Khusus

- **Dipakai ulang dari pool:** `Flammable.OnSpawned` mengosongkan `IsBurning`, `burnRemaining`, `reigniteAt`, dan `generation`. `OnReturned` memanggil `Extinguish(true)`. `FireArea.OnReturned` mematikan `NavMeshObstacle` dan timer. Prewarm: `FX_FireBurn` 24 dan `FX_FireArea` 8.
- **Game dijeda:** semua timer memakai `Time.deltaTime`, dan tick dilewati saat 0. `FireExtinguishTarget` tidak aktif karena `PlayerInteractor` ikut berhenti.
- **Tidur:** `SleepSystem` mempercepat jam, tetapi api tidak ikut dipercepat. Damage ke base tidak membangunkan pemain lewat blueprint ini.
- **Struktur hancur saat menyala:** `Flammable.OnDisable` memanggil `Extinguish(true)` sebelum objek dihapus. Runtuhan beruntun tetap milik #79.
- **Region dilepas (#88):** `OnDisable` memadamkan api di region itu tanpa event. Pohon yang belum habis kembali normal. Efek pool tetap utuh, karena bukan child.
- **Batas tercapai:** `TryIgnite` gagal tanpa pesan, tetapi damage area tetap berjalan.
- **Save:** `FireSystem` memakai `SaveId = world_fire`, berisi daftar `{ structureId, burnRemaining, generation }` khusus struktur. `FireArea` dan api pada musuh tidak disimpan. HP dan tampilan kerusakan pulih dari `StructureRecord` dan `StructureVisual`. Node habis disimpan `ResourceNode`. Saat `OnRestoreFinished`, api struktur dinyalakan lagi tanpa lempar peluang.
- **Permadeath:** `RunEndedEvent` memadamkan semua, dan `world_fire` terhapus bersama save run (#82). Tidak ada data api yang bertahan ke run baru.

## 5. Keputusan Desain

**Keputusan yang mengikuti blueprint lain:** `Storm` dan `Blizzard` memadamkan api tak beratap (#44). Pengali `Fire` batu 0 (#47). Damage lewat `DamagePipeline` (#59).

**Usulan yang disetujui developer sebagai nilai awal:**

1. **Cara memadamkan:** tahan `E` 1 detik dan pakai 1 air, air kotor lebih dulu. Rekomendasi ini memberi air kotor kegunaan baru tanpa alat tambahan.
2. **Api unggun dan obor tidak menjalar:** supaya pemain tidak kehilangan base karena alat dasar yang diajarkan Diary Bab 1.
3. **Pemain dan NPC tidak ikut menyala:** hanya menerima damage selama di dalam area. Aturannya lebih mudah dipahami dan tidak butuh status baru.
4. **Zombie menerobos Molotov, dan hanya Fireball yang memotong NavMesh:** mengikuti rasa L4D, dan menghemat biaya carving.
5. **Batas lompatan vegetasi 3:** supaya kayu untuk jembatan (500) tidak habis oleh satu percikan.
6. **Semua angka di tabel config:** perlu disetel lewat playtest.

## 6. Skenario Tes

1. Lempar Molotov ke dinding kayu. **Hasil:** dinding menyala, HP turun 6 per detik, dan notifikasi muncul.
2. Biarkan terbakar sampai padam. **Hasil:** HP berkurang sekitar 180, dan api tidak menyala lagi selama 20 detik.
3. Ulangi pada `Wall_Stone`. **Hasil:** tidak menyala dan HP tetap.
4. Nyalakan satu pohon di hutan. **Hasil:** api melompat paling jauh 3 kali, lalu berhenti. Pohon yang habis berstatus `Depleted`.
5. Paksa 50 objek menyala lewat `DevConsole`. **Hasil:** paling banyak 24 aktif, dan tidak ada lonjakan GC di Profiler.
6. Molotov saat `Rain`, lalu saat `Storm`. **Hasil:** durasi dikali 0,5 dan 0,25. Api di bawah atap tidak berubah.
7. Nyalakan dinding tak beratap, lalu paksa `Storm`. **Hasil:** padam pada tick berikutnya.
8. Bakar zombie di samping dinding kayu. **Hasil:** zombie tetap mengejar, mati sesuai damage, dan dinding kadang ikut menyala.
9. Bakar Zombie Desa, lalu fajar. **Hasil:** api padam saat `Submerged` atau `BurnToAshes`, tanpa efek ganda.
10. Tahan `E` pada api dengan 1 `Drink_DirtyWater`. **Hasil:** padam dan item berkurang. Tanpa air, muncul `Butuh air`.
11. Fireball jatuh di jalur zombie. **Hasil:** agent memutar selama 12 detik. Molotov tidak mengubah jalur.
12. Save saat dinding menyala, lalu load. **Hasil:** HP sama, api menyala lagi dengan sisa waktu yang sama.
13. Mati saat base terbakar, lalu mulai run baru. **Hasil:** tidak ada efek api tersisa, dan pool kembali penuh.
14. Tekan jeda saat api menyala. **Hasil:** HP dan timer berhenti.
