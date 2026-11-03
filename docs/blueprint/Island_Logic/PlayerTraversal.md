# Blueprint: PlayerTraversal.cs
**Lokasi:** `docs/blueprint/Island_Logic/PlayerTraversal.md`
**Deskripsi Utama:** Gerakan pemain yang belum ada di `IslandPlayerController`: jongkok, tangga dan tali, damage jatuh, lereng curam, dan pengaruh permukaan.
**Issue:** #101
**Rujukan GDD:** Bagian 3 (HP habis menyebabkan kematian) dan bagian 5 (jurang di Region 2, puncak es di Region 3, dan pengangkutan barang berat). Semua mekanik gerak di sini adalah usulan, karena GDD tidak merincinya.

## 1. Arsitektur & Variabel Utama

### Pola

`PlayerTraversal` adalah komponen di pemain Fase 2, bersama `IslandPlayerController` dan `CharacterController`. `IslandPlayerController` tetap memegang jalan, lari, lompat, dan gravitasi. `PlayerTraversal` menentukan mode gerak dan pengali kecepatan.

```csharp
public enum TraversalMode { Ground, Crouching, Climbing, Sliding, Swimming, Locked }

public enum SpeedModifierId { Surface, Crouch, Water, HeavyItem, Weather, ItemUse, StatusEffect }
```

| Mode | Dimulai oleh | Siapa menggerakkan |
|---|---|---|
| `Ground` | Nilai awal | `IslandPlayerController` |
| `Crouching` | Tombol `Crouch` | `IslandPlayerController`, dengan pengali `Crouch` |
| `Climbing` | Tekan `E` pada `ClimbableSurface` | `PlayerTraversal` |
| `Sliding` | Berdiri di lereng curam | `PlayerTraversal` |
| `Swimming` | `SwimmingController` (#13) | #13 |
| `Locked` | `PlayerPinnedState` (#110), #53 | Tidak ada |

**Prioritas mode:** `Locked`, `Swimming`, `Climbing`, `Sliding`, `Crouching`, lalu `Ground`.

**Fungsi publik untuk `SwimmingController` (#13):**

- `public void SetMode(TraversalMode mode)`: memasang mode, misalnya `SetMode(TraversalMode.Swimming)`.
- `public void ClearMode(TraversalMode mode)`: kalau `Mode == mode`, kembali ke `Ground`. Kalau tidak, tidak berubah.
- `public void SetOverWater(bool value)`: mengisi `isOverWater`, yang dibaca `OnLanded`.

### Pengali kecepatan

Pengali menggantikan `speed /= 2` di `Claude.md`, sehingga tidak berkurang berulang dan bisa ditumpuk dengan badai.

```csharp
// Di IslandPlayerController
public void SetSpeedModifier(SpeedModifierId id, float multiplier);
public void ClearSpeedModifier(SpeedModifierId id);
public float SpeedMultiplier { get; }   // Hasil kali semua pengali aktif, dengan batas bawah 0,1
```

Pengali disimpan di array berukuran tetap menurut `SpeedModifierId`, jadi tidak menghasilkan garbage. Kecepatan akhir: `walkSpeed` atau `sprintSpeed`, dikali `SpeedMultiplier`.

| Id | Pemilik | Nilai |
|---|---|---|
| `Surface` | `PlayerTraversal` | Tabel permukaan |
| `Crouch` | `PlayerTraversal` | `traversal.crouchSpeedMultiplier` |
| `Water` | `SwimmingController` (#13) | Menurut #13 |
| `HeavyItem` | `HeavyItemSystem` (#69) | `region.heavyItemSpeedMultiplier` 0,5 |
| `Weather` | `WeatherManager`, #115 | Menurut #115 |
| `ItemUse` | `ItemUseSystem` (#66) | `items.useMoveSpeedMultiplier` 0,5 |
| `StatusEffect` | `StatusEffectManager` (#20) | Menurut #20 |

### Variabel

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Mode` | `TraversalMode` | `Ground` | Mode saat ini |
| `IsCrouching` | `bool` | `false` | Dibaca #49, untuk Noise Sphere |
| `activeClimb` | `ClimbableSurface` | `null` | Tangga atau tali yang dipakai |
| `climbT` | `float` | 0 | Posisi di tangga, 0 bawah dan 1 atas |
| `fallPeakDownSpeed` | `float` | 0 | Kecepatan turun terbesar sebelum mendarat |
| `wasGrounded` | `bool` | `true` | Status tanah frame sebelumnya |
| `currentSurface` | `SurfaceType` | `Tanah` | Permukaan terakhir |
| `surfaceTimer` | `float` | 0 | Jeda pembacaan permukaan |
| `isOverWater` | `bool` | `false` | Pendaratan di air |

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.traversal.standHeight` | 1,8 | Tinggi `CharacterController` berdiri |
| `GameBalanceConfig.traversal.crouchHeight` | 1,1 | Tinggi saat jongkok |
| `GameBalanceConfig.traversal.heightChangeSpeed` | 6 | Kecepatan perubahan tinggi, dalam m/s |
| `GameBalanceConfig.traversal.eyeHeightCrouch` | 0,95 | Tinggi mata saat jongkok |
| `GameBalanceConfig.traversal.crouchSpeedMultiplier` | 0,5 | Kecepatan jongkok, sekitar 2,5 m/s |
| `GameBalanceConfig.traversal.ladderClimbSpeed` | 2,5 | Naik tangga, dalam m/s |
| `GameBalanceConfig.traversal.ropeClimbSpeed` | 1,8 | Naik tali, dalam m/s |
| `GameBalanceConfig.traversal.climbExhaustedMultiplier` | 0,4 | Kecepatan memanjat saat stamina habis |
| `GameBalanceConfig.traversal.climbEnterSeconds` | 0,25 | Waktu masuk ke tangga |
| `GameBalanceConfig.traversal.climbExitSeconds` | 0,35 | Waktu keluar ke atas atau bawah |
| `GameBalanceConfig.traversal.climbJumpOffForce` | 3 | Dorongan saat melepas dengan `Space` |
| `GameBalanceConfig.traversal.fallSafeSpeed` | 9 | Tanpa damage di bawah kecepatan ini, sekitar jatuh 4 meter |
| `GameBalanceConfig.traversal.fallLethalSpeed` | 20 | Damage 100 pada kecepatan ini, sekitar jatuh 20 meter |
| `GameBalanceConfig.traversal.slideMinAngle` | 45 | Sudut awal meluncur. Sama dengan `slopeLimit` |
| `GameBalanceConfig.traversal.slideSpeed` | 6 | Kecepatan meluncur |
| `GameBalanceConfig.traversal.slideSteerSpeed` | 1,5 | Geser ke samping saat meluncur |
| `GameBalanceConfig.traversal.surfaceCheckSeconds` | 0,2 | Jeda pembacaan permukaan |
| `GameBalanceConfig.traversal.jumpHeight` | 1,1 | Tinggi lompat. Menggantikan nilai 2 di `IslandPlayerController` |

### Tabel permukaan

Tabel berada di `GameBalanceConfig.traversal.surfaceProfiles`, satu baris per `SurfaceType`.

| `SurfaceType` | Pengali kecepatan | Pengali damage jatuh |
|---|---|---|
| `Pasir` | 0,9 | 0,9 |
| `Rumput` | 1 | 1 |
| `Tanah` | 1 | 1 |
| `Batu` | 1 | 1 |
| `Salju` | 0,8 | 0,75 |
| `Lumpur` | 0,7 | 0,75 |
| `Kayu` | 1 | 1 |
| `Logam` | 1 | 1 |

## 2. Detail Logika & Fungsi

### Urutan per frame

**`void Update()`**:

1. Baca `IsGrounded` dari `IslandPlayerController`.
2. Panggil `UpdateCrouch()`, kecuali mode `Climbing`, `Swimming`, atau `Locked`.
3. Panggil `UpdateSurface()`.
4. Panggil `UpdateSlide()`, kecuali mode `Climbing`, `Swimming`, atau `Locked`.
5. Kalau mode `Climbing`, panggil `UpdateClimb()`.
6. Panggil `UpdateFall()`.

`IslandPlayerController.Update()` memeriksa `Traversal.AllowsGroundMove`, yaitu mode `Ground` atau `Crouching`. Kalau `false`, ia tidak menggerakkan pemain.

Pemeriksaan tanah di `IslandPlayerController` memakai `CharacterController.isGrounded` ditambah `PhysicsQuery.Raycast` pendek ke `PhysicsMasks.Ground`, menggantikan `Physics.CheckSphere` tanpa aturan trigger.

### Jongkok

**`private void UpdateCrouch()`**:

1. Tentukan `wantCrouch = InputManager.Instance.CrouchHeld`. Kalau membawa Heavy Item, `wantCrouch = false`.
2. Kalau `wantCrouch` dan belum jongkok: `IsCrouching = true`, `SetSpeedModifier(Crouch, crouchSpeedMultiplier)`, dan `FirstPersonCamera.SetEyeHeight(eyeHeightCrouch)`.
3. Kalau `!wantCrouch` dan sedang jongkok: panggil `CanStandUp()`. Kalau `false`, tetap jongkok. Kalau `true`, kembali berdiri.
4. Ubah `controller.height` menuju target dengan `heightChangeSpeed`, dan atur `controller.center.y = controller.height / 2`, supaya kaki tetap di tanah.

**`private bool CanStandUp()`**, pemeriksaan ruang kepala:

```csharp
Vector3 bottom = transform.position + Vector3.up * (controller.radius + 0.05f);
Vector3 top = transform.position + Vector3.up * (standHeight - controller.radius);
return !PhysicsQuery.CheckCapsule(bottom, top, controller.radius * 0.95f, PhysicsMasks.Ground);
```

Kapsul memakai `controller.radius * 0.95` supaya tidak mengenai dinding di sisi pemain.

**Aturan lain:**

- `Sprint` diabaikan selama jongkok. Pemain perlu melepas `Crouch` lebih dulu.
- Melompat saat jongkok berdiri dulu, kalau `CanStandUp()`. Kalau tidak, lompatan ditolak.
- Senjata tetap bisa dipakai saat jongkok.
- `NoiseEmitter` (#49) membaca `IsCrouching`, dan mengecilkan radius langkah.

### Tangga dan tali

**`ClimbableSurface : IslandInteractable`** berada di layer `Interactable`, dengan collider biasa (bukan trigger).

| Field | Fungsi |
|---|---|
| `kind` | `Ladder` atau `Rope` |
| `bottomPoint`, `topPoint` | Dua ujung jalur panjat |
| `exitBottomPoint`, `exitTopPoint` | Titik pemain setelah keluar |
| `facing` | Arah pemain saat memanjat |

`CanInteract` mengembalikan `CarryingHeavyItem` saat pemain membawa Heavy Item.

**`public void BeginClimb(ClimbableSurface surface)`**, dipanggil `ClimbableSurface.OnInteract`:

1. `Mode = Climbing`, `activeClimb = surface`.
2. Hitung `climbT` dari titik terdekat di jalur panjat.
3. `EquipmentManager.PushLock(EquipLock.Climbing)`. Senjata tersimpan, dan #60 mengembalikannya setelah selesai.
4. `FirstPersonCamera.EnterClimbView(surface.facing)`.
5. Lerp posisi menuju jalur selama `climbEnterSeconds`.

**`private void UpdateClimb()`**:

```csharp
float input = InputManager.Instance.MoveInput.y;     // W naik, S turun
float speed = activeClimb.kind == ClimbKind.Ladder ? ladderClimbSpeed : ropeClimbSpeed;

if (Mathf.Abs(input) > 0.1f) stats.SpendStamina(climbStaminaPerSec * Time.deltaTime, StaminaUse.Climb);
if (stats.Stamina <= 0f) speed *= climbExhaustedMultiplier;

climbT += input * speed * Time.deltaTime / activeClimb.Length;
```

- Posisi pemain mengikuti `Vector3.Lerp(bottomPoint, topPoint, climbT)`, lalu `controller.Move` menuju titik itu.
- `climbT >= 1`: keluar ke `exitTopPoint` selama `climbExitSeconds`.
- `climbT <= 0`: keluar ke `exitBottomPoint`.
- Tombol `Jump`: lepas dari tangga dengan dorongan ke belakang `climbJumpOffForce`, lalu jatuh biasa.
- Stamina habis berarti lambat, tanpa jatuh.

**`public void EndClimb()`**: `Mode = Ground`, lepaskan kunci, dan panggil `FirstPersonCamera.ExitClimbView()`. Saat keluar di atas, `fallPeakDownSpeed` dinolkan.

**Akses senjata:** tidak bisa menembak atau memukul saat memanjat. Pemain yang dikejar bisa melepas lompatan.

**Terkena serangan saat memanjat:** tetap di tangga, kecuali #110 atau serangan menahan yang memanggil `ForceDetach()`.

**Tali di dalam jurang:** kalau ada, posisi `Rope` hanya titik buatan tangan. Jalur Region 4 ke Region 3 tidak boleh memakai tangga atau tali, karena Heavy Item tidak bisa memanjat.

### Damage jatuh

**`private void UpdateFall()`**:

```csharp
if (!IsGrounded && Mode != Climbing && Mode != Swimming)
    fallPeakDownSpeed = Mathf.Max(fallPeakDownSpeed, -controller.velocity.y);

if (IsGrounded && !wasGrounded) OnLanded();
wasGrounded = IsGrounded;
```

**`private void OnLanded()`**:

1. Kalau pemain mendarat di air (`isOverWater` atau #13 aktif), tidak ada damage.
2. Hitung `t = Mathf.InverseLerp(fallSafeSpeed, fallLethalSpeed, fallPeakDownSpeed)`.
3. Kalau `t > 0`, hitung `damage = t * 100 * surfaceProfile.fallDamageMultiplier`.
4. Kirim lewat jalur #59:

```csharp
stats.TakeDamage(new DamageInfo {
    amount = damage, type = DamageType.Fall,
    hitPoint = transform.position, bodyPart = BodyPart.Leg
});
```

5. `FirstPersonCamera.PlayLandingDip(impact01)`, dengan `impact01 = InverseLerp(3, fallLethalSpeed, fallPeakDownSpeed)`. Dip kecil juga tampil pada pendaratan biasa.
6. Kirim `NoiseEmitter.EmitLanding(impact01)` (#49).
7. `fallPeakDownSpeed = 0`.

**Jatuh ke `DeathReason`:** `DamageType.Fall` dipetakan ke `DeathReason.Fall` oleh `DeathReasonMap` (#59). Tidak ada jalur kematian terpisah.

**Teleport:** `public void ResetFallTracking()` mengatur `fallPeakDownSpeed = 0`. Dipanggil #56, #88, #116, dan setelah `CharacterController` dinyalakan kembali.

**Contoh:**

| Tinggi jatuh | Kecepatan | Damage di tanah | Damage di salju |
|---|---|---|---|
| 1,1 m (lompat) | 4,6 m/s | 0 | 0 |
| 4 m | 8,9 m/s | 0 | 0 |
| 8 m | 12,5 m/s | 32 | 24 |
| 15 m | 17,2 m/s | 74 | 56 |
| 20 m | 19,8 m/s | 98 | 74 |

### Lereng curam

**`private void UpdateSlide()`**:

1. Kalau tidak di tanah, dan mode `Sliding`, kembali ke `Ground`.
2. Lakukan `PhysicsQuery.Raycast` pendek ke bawah, memakai `PhysicsMasks.Ground`, untuk mengambil `normal`.
3. Hitung `angle = Vector3.Angle(normal, Vector3.up)`.
4. `angle >= slideMinAngle`: `Mode = Sliding`.
5. Saat meluncur, hitung `dir = Vector3.ProjectOnPlane(Vector3.down, normal).normalized`, lalu `controller.Move((dir * slideSpeed + sideInput * slideSteerSpeed) * Time.deltaTime)`.
6. Lompat dan lari ditolak selama `Sliding`. Berjalan melawan arah luncur tidak mungkin.
7. Damage jatuh tetap dihitung dari kecepatan turun saat keluar lereng.

`CharacterController.slopeLimit` bernilai sama dengan `slideMinAngle`, yaitu 45. Pada Terrain, sudut di atas 45 sekaligus menjadi batas area yang tidak bisa didaki.

### Pengaruh permukaan

**`private void UpdateSurface()`**: setiap `surfaceCheckSeconds`, baca `SurfaceDetection.Instance.GetSurfaceAt(transform.position)` (#86). Kalau berubah, panggil `SetSpeedModifier(Surface, profile.speedMultiplier)`. Pembacaan tidak dilakukan tiap frame, karena alphamap sudah di-cache di #86, tetapi hasilnya juga tidak berubah cepat.

Suara langkah dibaca #21 lewat fungsi #86 sendiri.

### Batas saat membawa Heavy Item

| Aksi | Saat membawa Heavy Item |
|---|---|
| Jongkok | Tidak bisa. Kalau sedang jongkok saat mengangkat, mengangkat ditolak (#69) |
| Tangga dan tali | Tidak bisa |
| Lompat dan lari | Tidak bisa (#69) |
| Lereng curam | Tetap meluncur. Barang jatuh diatur #69 |
| Pengali kecepatan | `HeavyItem` 0,5, dikali `Surface` dan `Weather` |
| Damage jatuh | Tetap berlaku |

**Hitungan:** membawa barang di salju: 5 x 0,5 x 0,8 = 2 m/s. Perhitungan `GameBalanceConfig.md` memakai 2,5 m/s, jadi 150 meter di area dingin menjadi 75 detik, bukan 60 detik. HP berkurang 55, bukan 40. Angka ini perlu dicatat di playtest.

### Vault

Tidak ada vault atau memanjat tepian di versi awal. Lihat bagian 5.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** tidak ada. Status dibaca lewat properti `Mode` dan `IsCrouching`.

**Event yang didengar:** `HeavyItemChangedEvent`, `PlayerDiedEvent`, `UIFocusChangedEvent`.

| Sistem | Hubungan |
|---|---|
| `IslandPlayerController` | Menyediakan `IsGrounded`, gravitasi, dan pengali kecepatan |
| `SurvivalStats` (#111) | `SpendStamina(StaminaUse.Climb)` |
| `DamagePipeline` (#59) | `DamageType.Fall`, dan `DeathReason.Fall` |
| `FirstPersonCamera` (#102) | `SetEyeHeight`, `EnterClimbView`, `ExitClimbView`, dan `PlayLandingDip` |
| `EquipmentManager` (#60) | `PushLock` dan `PopLock` |
| `IslandInteractable` (#17) | `ClimbableSurface` mewarisinya |
| `SurfaceDetection` (#86) | `GetSurfaceAt` |
| `AISensory`, `NoiseEmitter` (#49) | Membaca `IsCrouching`, dan `EmitLanding` |
| `SwimmingController` (#13) | Memegang mode `Swimming` lewat `SetMode` dan `ClearMode`, serta mengisi `isOverWater` lewat `SetOverWater` |
| `PlayerPinnedState` (#110) | Memakai `SetLocked` dan `ForceDetach` |
| `HeavyItemSystem` (#69) | Memegang pengali `HeavyItem` |
| `AITraversal` (#94) | Musuh di tangga dan tepi, di luar issue ini |
| `RegionGateSystem` (#68) | Jurang Region 2 mencegah lompat. `jumpHeight` baru mengecilkan risiko ini |

**Batas pekerjaan:** berenang di #13, kamera di #102, dan efek beban di #69.

## 4. Kasus Khusus

- **Objek pool:** tidak ada. Pemain tidak memakai pool, dan `ClimbableSurface` adalah objek statis.
- **Game dijeda:** `Time.deltaTime` bernilai 0, dan `Gameplay` mati, jadi semua mode berhenti di tempat. Pemain di tangga tidak jatuh.
- **Permadeath:** tidak ada data lintas run. Scene pemain dibuang (#82).
- **Save dan load:** mode tidak disimpan. `WorldStateSave` (#80) memakai `PlayerTraversal.GetSafeSavePosition()`, yaitu posisi `exitBottomPoint` saat memanjat, dan posisi terakhir di tanah saat melayang atau meluncur. Pemain selalu dimuat dalam keadaan berdiri.
- **Autosave saat jatuh:** posisi tanah terakhir mencegah pemain dimuat di udara, dan mencegah damage jatuh ganda.
- **`CharacterController` dimatikan:** `ResetFallTracking()` dipanggil saat dinyalakan kembali.
- **Mati di tangga:** `PlayerDiedEvent` memanggil `EndClimb()`, lalu #53 mengambil alih.
- **Tangga hancur atau objek dilepas saat dipakai:** kalau `activeClimb` menjadi `null`, mode kembali ke `Ground`, dan pemain jatuh biasa.
- **Region dilepas saat di tangga:** tidak terjadi, karena region pemain tidak dilepas (#88).
- **Jongkok di bawah atap rendah:** pemain tetap jongkok setelah melepas tombol, sampai `CanStandUp()` bernilai `true`.
- **Panel terbuka saat jongkok:** `CrouchHeld` menjadi `false` karena `Gameplay` mati, jadi pemain berdiri kalau ada ruang. Kalau tidak ada, pemain tetap jongkok.
- **Mendarat di Structure:** damage sama dengan permukaan. `GetSurfaceAt` mengembalikan `Kayu`, dan pengalinya 1.
- **Knockback atau Fireball:** semua kecepatan turun terbaca dari `controller.velocity`, jadi damage tetap adil.
- **Sliding di atas `WorldItem`:** tidak terpengaruh, karena `WorldItem` tidak bertabrakan dengan pemain (#87).
- **Berada di kapal:** komponen ini tidak dipasang di `ShipPlayerController`.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** tombol `Crouch` ditahan (#18). `DamageType.Fall` dan `DeathReason.Fall` (`GLOSSARY.md`). Pengali barang berat 0,5 (`GameBalanceConfig.md`).

**Keputusan teknis di blueprint ini:**

- `PlayerTraversal` mengatur mode, dan `IslandPlayerController` tetap menggerakkan pemain saat `Ground` dan `Crouching`.
- Pengali kecepatan memakai array per `SpeedModifierId`.
- Damage jatuh memakai kecepatan, bukan tinggi.

**Usulan yang disetujui developer sebagai nilai awal (dicatat di #85):**

1. **Vault dan memanjat tepian:** **tidak ada di versi awal.** Fitur itu menambah animasi, membuka celah masuk ke base, dan mengacaukan rute jurang. Bisa ditambah nanti lewat `TraversalMode`.
2. **Tangga dan tali:** naik dengan `E`, senjata tersimpan, dan stamina habis berarti lambat, bukan jatuh.
3. **Damage jatuh:** aman sampai sekitar 4 meter, dan mematikan sekitar 20 meter. Salju dan lumpur mengurangi 25 persen, dan air meniadakan damage.
4. **Lereng curam:** di atas 45 derajat pemain meluncur tanpa damage.
5. **Permukaan:** salju 0,8, lumpur 0,7, pasir 0,9, dan sisanya 1.
6. **`jumpHeight` turun dari 2 ke 1,1:** nilai 2 memungkinkan pemain melompat ke atas dinding base dan rintangan, sehingga menjadi celah.
7. **Membawa Heavy Item:** tidak bisa jongkok atau memanjat. Jalur Region 4 ke Region 3 harus bebas tangga. Salju membuat frostbite lebih lama dari asumsi `GameBalanceConfig.md`, seperti tertulis di bagian batas Heavy Item.

## 6. Skenario Tes

1. Tahan `Left Ctrl`. **Hasil:** tinggi menjadi 1,1, mata turun, dan kecepatan sekitar 2,5 m/s.
2. Masuk ke kolong 1,3 meter sambil jongkok, lalu lepas tombol. **Hasil:** pemain tetap jongkok. Keluar dari kolong. **Hasil:** pemain berdiri otomatis.
3. Lari, lalu tahan `Ctrl`. **Hasil:** pemain jongkok, dan `Sprint` diabaikan selama itu.
4. Tekan `E` pada tangga. **Hasil:** pemain menempel, senjata tersimpan, `W` naik, dan `S` turun.
5. Naik sampai atas. **Hasil:** pemain keluar ke `exitTopPoint`, dan senjata kembali.
6. Tekan `Space` di tengah tangga. **Hasil:** pemain lepas dan jatuh. Damage dihitung dari tinggi saat itu.
7. Habiskan stamina saat memanjat. **Hasil:** gerak melambat ke 40 persen, tanpa jatuh.
8. Coba pakai tangga saat membawa Heavy Item. **Hasil:** ditolak dengan `Tangan penuh`.
9. Jatuh dari 3 meter, 8 meter, dan 25 meter. **Hasil:** damage 0, sekitar 32, dan mati dengan alasan `Fall`.
10. Jatuh 8 meter ke salju, lalu ke air. **Hasil:** damage sekitar 24, lalu 0.
11. Injak lereng 50 derajat. **Hasil:** pemain meluncur 6 m/s, dan tidak bisa mendaki.
12. Berjalan di rumput, salju, dan lumpur. **Hasil:** kecepatan berubah, dan tetap berlaku bersama badai dan barang berat tanpa berkurang berulang.
13. Teleport tinggi lewat `DevConsole`. **Hasil:** `ResetFallTracking` mencegah damage palsu.
14. Simpan saat di tangga, keluar, lalu lanjutkan. **Hasil:** pemain berdiri di titik bawah tangga, tanpa damage.
15. Buka Pause di tangga selama 10 detik. **Hasil:** pemain tidak bergerak dan tidak kehilangan stamina.
16. Lihat `IsCrouching` bersama #49. **Hasil:** radius suara langkah lebih kecil saat jongkok.
