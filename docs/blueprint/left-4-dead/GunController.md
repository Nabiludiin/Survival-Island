# Blueprint: GunController.cs (WeaponData & GunCore)
**Lokasi:** `docs/blueprint/left-4-dead/GunController.md`
**Deskripsi Utama:** Sistem senjata api untuk `Base_Pistol`, senjata hadiah Lucky Machine, dan NPC. Mengatur tembakan raycast, isi magazine, cadangan peluru, reload, recoil, spread, serta suara tembakan.
**Issue:** #58
**Rujukan GDD:** Bagian 3 (Pistol sebagai item awal, peluru sebagai prioritas loot) dan bagian 7 (Laser Pistol). Headshot 2x mengikuti `GLOSSARY.md`. Semua angka senjata, aturan gangguan reload, dan aturan rusak adalah usulan, karena gunplay L4D belum terekstrak (`docs/CATATAN_MEKANIK.md` bagian 5).

## 1. Arsitektur & Variabel Utama

### Pola

Logika tembakan dipisah menjadi dua lapis, supaya pemain dan NPC memakai sistem yang sama:

- **`GunCore`:** kelas biasa tanpa `MonoBehaviour`. Memegang isi magazine, jeda tembak, spread, dan satu fungsi `Fire`.
- **`GunController : EquippableBehaviour`:** pembungkus untuk pemain. Menerima input lewat `IEquippable` (#60), lalu mengisi `GunCore` dengan kamera, inventori, dan `ItemInstance`.
- **`NPCGunUser`:** pembungkus untuk NPC di prefab `NPCCompanion` (#76).

### `WeaponData` (ScriptableObject)

`ItemData` mendapat field `weaponData` yang merujuk aset ini. Angka per senjata berada di aset, jadi bisa disetel tanpa mengubah kode.

| Field | `Base_Pistol` | `Reward_LaserPistol` | `NPC_Pistol` | Fungsi |
|---|---|---|---|---|
| `fireMode` | `SemiAuto` | `SemiAuto` | `SemiAuto` | `SemiAuto` atau `Auto` |
| `damage` | 30 | 60 | 15 | Masuk ke `DamageInfo.amount` |
| `secondsBetweenShots` | 0,25 | 0,35 | 0,8 | Fire rate |
| `magazineSize` | 12 | 8 | 12 | Kapasitas |
| `startingMagazine` | 12 | 8 | 12 | Isi saat `magazineAmmo` masih -1 |
| `ammoItemId` | `Ammo_9mm` | Kosong | Kosong | Kosong berarti cadangan tak terbatas |
| `reloadSeconds` | 1,6 | 3 | 2,5 | Waktu reload |
| `maxRange` | 60 | 80 | 30 | Jarak raycast, dalam meter |
| `spreadBase` | 0,4 | 0,2 | 2 | Spread saat diam, dalam derajat |
| `spreadMoving` | 1,5 | 1 | 0 | Tambahan saat bergerak |
| `spreadPerShot` | 0,6 | 0,3 | 0 | Tambahan per tembakan |
| `spreadMax` | 4 | 3 | 2 | Batas atas |
| `spreadRecoverPerSec` | 6 | 6 | 0 | Laju kembali ke normal |
| `recoilPitch` / `recoilYaw` | 1,2 / 0,4 | 0,6 / 0,2 | 0 / 0 | Dikirim ke `AddRecoil` |
| `noiseRadius` | 35 | 20 | 25 | Radius Noise Sphere, dalam meter |
| `wearPerShot` | 0,25 | 0 | 0 | Keausan per tembakan |
| `muzzleFlashTag` / `impactTag` | `VFX_Muzzle` / `VFX_Impact` | `VFX_LaserMuzzle` / `VFX_LaserImpact` | `VFX_Muzzle` / `VFX_Impact` | Tag di `GameBalanceConfig.vfx.catalog` (#15) |
| `cameraShakeScale` | 0,6 | 1 | 0 | Skala getaran kamera. Dibaca `CameraShake` saat menerima `WeaponFiredEvent` (#24). Nilai selain `Base_Pistol` adalah usulan |

### Variabel `GunCore`

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `stats` | `WeaponStats` | Dari `WeaponStatCalculator.Get` | Hasil gabungan data, upgrade (#113), dan modifier (#72) |
| `nextShotTime` | `float` | 0 | Memakai `Time.time`, jadi berhenti saat dijeda |
| `currentSpread` | `float` | `spreadBase` | Dibaca crosshair lewat `CurrentSpread` |

### Variabel `GunController`

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `instance` | `ItemInstance` | `null` | Sumber `magazineAmmo` dan `durability` |
| `isReloading` | `bool` | `false` | Status reload |
| `reloadEndTime` | `float` | 0 | Waktu reload selesai |
| `autoReloadTime` | `float` | -1 | Waktu reload otomatis dimulai |
| `nextDryClickTime` | `float` | 0 | Jeda suara klik kosong |

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.gun.autoReloadDelay` | 0,3 | Jeda sebelum reload otomatis |
| `GameBalanceConfig.gun.dryClickCooldown` | 0,25 | Jeda suara klik kosong |
| `GameBalanceConfig.gun.npcFireMaxAngle` | 10 | Sudut maksimum NPC boleh menembak ke target |
| `GameBalanceConfig.gun.movingSpeedThreshold` | 0,5 | Kecepatan pemain yang dianggap bergerak, m/s |

## 2. Detail Logika & Fungsi

### Tembakan

**`public bool GunCore.Fire(in Ray ray, GameObject shooter, string itemId, bool byPlayer)`**:

1. Tolak kalau `Time.time < nextShotTime`.
2. `nextShotTime = Time.time + stats.secondsBetweenShots`.
3. Geser arah ray secara acak di dalam kerucut `currentSpread`. Memakai `UnityEngine.Random`, karena hasilnya tidak memengaruhi isi dunia.
4. `PhysicsQuery.Raycast(ray, out hit, stats.maxRange, PhysicsMasks.Shoot)`. Mask ini tidak memuat `Player` dan kapsul `Enemy`, dan `PhysicsQuery` selalu memakai `QueryTriggerInteraction.Ignore`, jadi collider pemain dan trigger zona tidak menghentikan peluru.
5. Kalau kena, isi `DamageInfo { amount = stats.damage, type = Bullet, source = shooter, sourceId = itemId, hitDirection = ray.direction, bodyPart = Torso, infection = 0, byPlayer }`, lalu panggil `DamageRouter.ApplyHit(hit, info)`. Pengali kepala 2x, armor, dan friendly fire dihitung jalur #59.
6. Panggil `VFXManager.Instance.PlayImpact(hit, overrideTag)` (#15).
7. `currentSpread = Min(currentSpread + spreadPerShot, spreadMax)`.
8. Setelah raycast selesai, terbitkan `ShotResolvedEvent { itemId, hitEnemy, isHeadshot, byPlayer }` (#57) satu kali per tembakan, termasuk senjata dengan banyak peluru. `WeaponFiredEvent` tetap ada.

Peluru tidak menembus objek dan tidak punya objek fisik, sesuai `PhysicsLayers.md`. Air tidak ada di mask `Shoot`.

### Alur tembak pemain

**`PrimaryAction(ActionPhase phase)`**, dipanggil `EquipmentManager`:

1. `SemiAuto` hanya menerima `Pressed`, sedangkan `Auto` menerima `Pressed` dan `Held`.
2. Kalau `DurabilitySystem.GetState(instance) == Broken`, mainkan klik kosong dan tampilkan `Pistol rusak` (#22), lalu selesai.
3. Kalau `isReloading`, abaikan.
4. Kalau `magazineAmmo == 0`, jalankan klik kosong, lalu selesai.
5. Buat ray dari `WorldCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f))`. Kamera dunia dipakai sesuai `FirstPersonCamera.md`, jadi recoil benar-benar menggeser bidikan.
6. Panggil `GunCore.Fire`. Kalau ditolak jeda, selesai.
7. `magazineAmmo--`, lalu `DurabilitySystem.ApplyWear(instance, wearPerShot)`.
8. `VFXManager.Instance.Play(muzzleFlashTag, pos, rot, muzzlePoint, viewmodel: true)`, `armsAnim.Play(AnimIds.Fire)` (#12), lalu SFX.
9. `FirstPersonCamera.AddRecoil(recoilPitch, Random.Range(-recoilYaw, recoilYaw))`.
10. `NoiseEmitter.Emit(GameBalanceConfig.stealth.noiseGunshot, NoiseSourceKind.Gunshot)` (#49), yang menerbitkan `NoiseEmittedEvent`.
11. Terbitkan `AmmoChangedEvent` dan `WeaponFiredEvent`.
12. Kalau `magazineAmmo == 0` dan cadangan ada, atur `autoReloadTime = Time.time + autoReloadDelay`.

### Alur peluru

**Isi magazine** berada di `ItemInstance.magazineAmmo`, jadi senjata yang dilepas tetap menyimpan isinya. **Cadangan** adalah jumlah `ammoItemId` di `InventoryManager`, yang dibaca lewat `GetReserve()`.

| Kondisi saat klik | Hasil |
|---|---|
| Magazine berisi | Menembak |
| Magazine kosong, cadangan ada | Klik kosong, dan reload otomatis dimulai |
| Magazine kosong, cadangan kosong | Klik kosong per `dryClickCooldown`, dan teks peluru di HUD berkedip |

Klik kosong tidak mengirim Noise Sphere.

**`public void StartReload()`**, dipicu `InputManager.ReloadPressed` di `Update` atau reload otomatis:

1. Tolak kalau sudah reload, magazine penuh, cadangan 0, atau item `Broken`.
2. `isReloading = true`, `reloadEndTime = Time.time + reloadSeconds`, lalu `armsAnim.PlayReload(reloadSeconds, clipSeconds)` (#12) dan SFX.

**`FinishReload()`**, saat `Time.time >= reloadEndTime`:

1. `take = Min(magazineSize - magazineAmmo, GetReserve())`.
2. Kurangi cadangan sebanyak `take` lewat `InventoryManager`. Kalau `ammoItemId` kosong, langkah ini dilewati.
3. `magazineAmmo += take`. Sisa di magazine lama tidak hilang.
4. `isReloading = false`, lalu terbitkan `AmmoChangedEvent`.

Peluru baru berkurang di akhir reload, jadi reload yang batal tidak membuang apa pun.

### Gangguan reload

**`CancelReload()`** mengatur `isReloading = false` dan `autoReloadTime = -1`, lalu memanggil `armsAnim.CancelAction()` (#12).

| Gangguan | Hasil | Alasan |
|---|---|---|
| Ganti senjata | Batal lewat `OnUnequip` | Sesuai `EquipmentManager.md` |
| Kena serangan | Tetap lanjut | Saat dikepung, pemain tidak boleh terkunci tanpa bisa mengisi peluru |
| Mulai berlari | Batal, saat `Update` melihat `IslandPlayerController.IsSprinting` berubah menjadi `true` | Memberi pilihan: kabur atau mengisi peluru |
| Kunci `HeavyItem`, `Climbing`, atau `Swimming` | Batal lewat `OnUnequip` | Tangan terpakai |
| Kunci `Pinned` | Batal, karena `GunController` mendengar `PlayerPinnedEvent` | Item tetap di tangan, tetapi input terkunci (#60) |
| Panel tanpa jeda terbuka | Tetap lanjut | Waktu masih berjalan |

Reload yang batal harus diulang dari awal.

### Spread dan recoil

**`GunController.Update()`** memanggil `GunCore.TickSpread(isMoving, deltaTime)`:

1. `floor = spreadBase + (isMoving ? spreadMoving : 0)`, dengan `isMoving` bernilai benar kalau kecepatan datar `IslandPlayerController` di atas `movingSpeedThreshold`.
2. `currentSpread = MoveTowards(currentSpread, floor, spreadRecoverPerSec * deltaTime)`.

Berlari tetap boleh menembak, tanpa aturan tambahan selain spread gerak. Pemulihan recoil milik `FirstPersonCamera` lewat `recoilRecoverPerSec`. Tembakan beruntun memang lebih meleset, dan pemain yang diam serta menembak pelan tetap akurat.

### Muzzle flash dan impact

- Muzzle flash: `VFXManager.Instance.Play(tag, pos, rot, muzzlePoint, viewmodel: true)`. Impact: `VFXManager.Instance.PlayImpact(hit, overrideTag)`, dengan `overrideTag` berisi `impactTag` senjata, atau `null` kalau tag itu `VFX_Impact` (#15).
- `PooledEffect` dipindah ke `VFXManager.md`, dan umur efek dibaca dari `GameBalanceConfig.vfx.catalog`.
- Efek pemain memakai layer `PlayerViewmodel`. Efek NPC tetap di layer dunia.

### Senjata lain

- **Laser Pistol:** `GunController` yang sama dengan aset berbeda. `ammoItemId` kosong membuat `GetReserve()` mengembalikan `int.MaxValue`, dan `reloadSeconds` 3 berfungsi sebagai waktu isi daya. Modifier `indestructible` (#72) memberi `durability = -1`.
- **NPC:** `NPCGunUser.TryShootAt(ITargetable target)` dipanggil state `Attack` di `NPCCompanion`:
  1. Tolak kalau sudut ke target di atas `npcFireMaxAngle`.
  2. Buat ray dari `muzzlePoint` menuju `target.AimPoint`. Titik awal berada di dalam kapsul NPC, sehingga raycast tidak mengenai kapsulnya sendiri.
  3. Panggil `GunCore.Fire` dengan `byPlayer = false`.
  4. Kirim suara lewat `NoiseEmitter`, tanpa `AmmoChangedEvent`.
  5. Saat magazine habis, tunggu `reloadSeconds`, lalu isi ulang tanpa memakai inventori.
- **Usulan #103, berlaku kalau usulan senjata disetujui:** `Wpn_SMG` (`Auto`, damage 18, magazine 30, jeda 0,09 detik, `Ammo_9mm`) dan `Wpn_Rifle` (`SemiAuto`, damage 45, magazine 20, jeda 0,3 detik, jarak 90 m, suara 45 m, `Ammo_556`).

### Item inti dan rusak

Mengikuti usulan yang sudah tertulis di `ToolRepairSystem.md`: `Base_Pistol` bisa mencapai `Broken`, tetapi tidak pernah `Destroyed`. Saat `Broken`, Pistol tidak bisa menembak dan reload, dan perbaikannya memakai resep `Hand`. Dengan `wearPerShot` 0,25, Pistol tahan 400 tembakan.

### Upgrade

Statistik dibaca lewat `WeaponStatCalculator.Get` saat senjata dipasang (`OnEquip`) (#113). **`public void RecalculateStats()`** memanggil `WeaponStatCalculator.Get`, lalu menyimpan hasilnya di `GunCore.stats`. `speed` membagi `secondsBetweenShots` dan `reloadSeconds`. Kalau `magazineAmmo` lebih besar dari kapasitas baru, isinya dibiarkan sampai ditembakkan.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `AmmoChangedEvent { inMagazine, reserve }` (sticky), `WeaponFiredEvent { itemId, position, byPlayer }` (baru), dan `ShotResolvedEvent { itemId, hitEnemy, isHeadshot, byPlayer }` (#57).

**Event yang didengar:** `InventoryChangedEvent` (kalau `itemId == ammoItemId`, terbitkan `AmmoChangedEvent` lagi), dan `PlayerPinnedEvent` (membatalkan reload).

| Sistem | Hubungan |
|---|---|
| `EquipmentManager` (#60) | Memanggil `OnEquip`, `OnUnequip`, dan `PrimaryAction`. `CanSwitchNow` selalu `true`, karena tembakan terjadi seketika |
| `DamagePipeline` (#59) | `DamageRouter.ApplyHit` |
| `FirstPersonCamera` (#102) | `WorldCamera` dan `AddRecoil` |
| `InputManager` (#18) | `ReloadPressed`, dibaca di `Update` hanya saat komponen aktif |
| `InventoryManager`, `ItemUseSystem` (#66) | Cadangan dan `magazineAmmo` |
| `DurabilitySystem`, `ToolRepairSystem` (#113) | `ApplyWear`, `GetState`, dan `WeaponStatCalculator` |
| `NoiseEmitter`, `AISensory` (#49) | Tembakan memancing zombie |
| `HUDManager` | Mendengar `AmmoChangedEvent`, lalu memanggil `UpdateAmmo` |
| `NPCCompanion`, `NPCRoster` (#76) | `NPCGunUser` dan `NPC_Pistol` |
| `LuckyMachine` (#72) | Sumber `Reward_LaserPistol` dan modifier |
| `CameraShake` (#24) | Mendengar `WeaponFiredEvent`, dan membaca `cameraShakeScale` |
| `LocalStatsManager` (#57) | Mendengar `WeaponFiredEvent` dan `ShotResolvedEvent` |
| `VFXManager` (#15), `AnimationController` (#12) | Efek tembakan, dan animasi tangan lewat `armsAnim` |

**Batas pekerjaan:** getaran kamera di #24, hit marker di #51, dan pusat efek partikel di #15.

## 4. Kasus Khusus

- **Objek pool:** reset `PooledEffect` diatur `VFXManager.md`. `GunController` sendiri bukan objek pool (#60). `NPCGunUser` mengisi ulang magazine saat NPC keluar dari pool.
- **Game dijeda:** semua waktu memakai `Time.time` dan `Time.deltaTime`, jadi reload, jeda tembak, dan spread berhenti. Input `Gameplay` mati lewat #83.
- **Permadeath:** isi magazine dan cadangan terhapus bersama save run (#82). `Base_Pistol` di run baru kembali ke `startingMagazine`.
- **Save:** `magazineAmmo` dan `durability` tersimpan di `ItemInstance`, dan cadangan tersimpan sebagai tumpukan `Ammo_9mm` (#66). `isReloading` tidak disimpan. Reload yang berjalan saat save batal tanpa mengurangi peluru.
- **`magazineAmmo == -1` saat `OnEquip`:** isi dengan `startingMagazine`.
- **Peluru dipindah ke peti saat reload:** `FinishReload` menghitung cadangan saat selesai, jadi `take` bisa 0.
- **Mati saat reload:** kunci `Dead` menghentikan semua. Peluru tidak berubah.
- **Target mati pada frame yang sama:** `DamageRouter` mengecek `IsAlive`, jadi tidak ada `EnemyKilledEvent` kedua.
- **Tembak `HeavyItem` atau `Interactable`:** peluru berhenti dengan efek impact, tanpa damage, karena tidak ada `IDamageable`.
- **Tembak di Fase 1:** tidak terjadi. `EquipmentManager` tidak ada di `ShipPhase`.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** headshot 2x, tombol `R`, klik kiri untuk item aktif, `PhysicsMasks.Shoot`, dan `AmmoChangedEvent`.

**Keputusan teknis di blueprint ini:**

- Logika tembakan berada di `GunCore`, dipakai bersama oleh pemain dan NPC.
- Peluru cadangan baru berkurang di akhir reload.
- Spread memakai `UnityEngine.Random`, bukan `RunSeedManager`.

**Usulan yang disetujui developer sebagai nilai awal (dicatat di #85):**

1. **Pistol sebagai item inti:** bisa rusak, tetapi tidak pernah hancur. Selaras dengan `ToolRepairSystem.md`, sehingga pemain tidak kehilangan item awal.
2. **Gangguan reload:** batal saat ganti senjata dan mulai berlari, serta lanjut saat kena serangan.
3. **Angka `Base_Pistol`:** damage 30, 12 peluru, reload 1,6 detik, jarak 60 meter, dan suara 35 meter. Perlu playtest.
4. **Isi awal Pistol:** 12 peluru penuh dan cadangan 0, supaya pemain yang gagal mengambil peluru di kapal masih bisa bertahan.
5. **Laser Pistol:** tanpa cadangan, 8 tembakan, isi daya 3 detik, damage 60, dan tak bisa rusak lewat modifier. GDD hanya menyebut damage tinggi.
6. **Peluru NPC:** tak terbatas dan tidak memakai inventori, dengan damage rendah, karena peluru pemain sangat terbatas.

## 6. Skenario Tes

1. Tembak zombie yang berdiri di dalam `ZoneTrigger`. **Hasil:** zombie terkena, dan trigger tidak menghentikan raycast.
2. Tembak kepala, badan, dan kaki. **Hasil:** damage 60, 30, dan 15.
3. Tekan klik kiri secepat mungkin selama 2 detik. **Hasil:** paling banyak 8 tembakan, dan Pistol tidak menembak beruntun saat tombol ditahan.
4. Habiskan magazine dengan 12 cadangan. **Hasil:** reload otomatis dimulai 0,3 detik kemudian. Magazine menjadi 12 dan cadangan 0.
5. Sisakan 5 di magazine, cadangan 20, lalu tekan `R`. **Hasil:** magazine 12, cadangan 13.
6. Habiskan semua peluru, lalu klik. **Hasil:** hanya bunyi klik, teks peluru berkedip, dan `eventlog 1` tidak menampilkan `NoiseEmittedEvent`.
7. Reload, lalu tekan tombol slot lain. **Hasil:** reload batal, cadangan tetap. Kembali ke Pistol. **Hasil:** isi magazine sama.
8. Reload, lalu mulai berlari. **Hasil:** reload batal. Reload, lalu biarkan zombie memukul. **Hasil:** reload selesai.
9. Tembak 10 kali cepat sambil berjalan, lalu berhenti. **Hasil:** spread melebar sampai batas 4 derajat, lalu kembali ke 0,4 derajat dalam sekitar 0,6 detik.
10. Tembak di dekat zombie yang belum melihat pemain. **Hasil:** zombie dalam radius 35 meter bereaksi.
11. Rusak Pistol lewat `DevConsole`, lalu klik. **Hasil:** tidak menembak, item tetap ada, dan bisa diperbaiki dengan 1 `Mat_Cloth`.
12. Pegang Laser Pistol, lalu tembak 8 kali. **Hasil:** isi daya 3 detik tanpa mengurangi item apa pun.
13. Biarkan NPC menembak zombie. **Hasil:** zombie terkena dengan `byPlayer = false`, dan cadangan pemain tetap.
14. Simpan dengan magazine 7, keluar, lalu lanjutkan. **Hasil:** magazine 7. Mati, lalu mulai run baru. **Hasil:** magazine kembali 12.
15. Jeda game saat reload selama 10 detik. **Hasil:** sisa waktu reload tetap sama.
16. Tembak 100 kali dengan Profiler terbuka. **Hasil:** ukuran pool efek tidak bertambah setelah awal, dan tidak ada `Instantiate`.
