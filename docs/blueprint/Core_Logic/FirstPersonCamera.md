# Blueprint: FirstPersonCamera.cs (Kamera & Viewmodel)
**Lokasi:** `docs/blueprint/Core_Logic/FirstPersonCamera.md`
**Deskripsi Utama:** Satu komponen mouse look untuk Fase 1 dan Fase 2, kamera dunia dan kamera tangan yang terpisah, penggabungan rotasi tambahan, serta posisi tiap item di layar.
**Issue:** #102
**Rujukan GDD:** Bagian 1 (genre First-Person) dan bagian 3 (kapal miring). Susunan kamera, angka, dan aim down sights adalah usulan teknis, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

### Susunan objek

```text
Player (CharacterController, yaw diputar di sini)
└── CameraRig (posisi lokal Y mengikuti tinggi mata)
    └── PitchPivot (pitch dari mouse)
        └── OffsetPivot (rotasi dan posisi tambahan)
            ├── WorldCamera (tag MainCamera, AudioListener)
            └── ViewmodelCamera
                └── ViewmodelRoot (sway dan pose item)
                    ├── Item tangan kanan (layer PlayerViewmodel)
                    └── Item tangan kiri (layer PlayerViewmodel)
```

Prefab rig ini sama di `ShipPhase` dan `IslandPhase`. `ShipPlayerController` dan `IslandPlayerController` hanya mengatur gerak, dan tidak membaca mouse.

### Dua kamera

| Kamera | Render | Culling Mask | FOV | Near clip | Tugas |
|---|---|---|---|---|---|
| `WorldCamera` | Base | Semua, kecuali `PlayerViewmodel` | `camera.baseFov`, ditambah kick lari | 0,1 | Dunia |
| `ViewmodelCamera` | Overlay di URP Camera Stack, dengan `Clear Depth` | Hanya `PlayerViewmodel` | `camera.viewmodelFov`, tetap | 0,01 | Tangan dan item |

Dengan `Clear Depth`, tangan selalu tergambar di atas dunia, sehingga senjata tidak menembus dinding. Item viewmodel tidak punya collider (`PhysicsLayers.md`). Pemilihan URP mengikuti `SurvivalStats.md`, yang memakai `Volume`. Kalau proyek memakai pipeline lain, `ViewmodelCamera` memakai `Depth only` dengan depth lebih tinggi.

### Variabel utama

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Instance` | `FirstPersonCamera` | `null` | Singleton per scene |
| `yaw`, `pitch` | `float` | 0 | Rotasi dasar dari mouse |
| `offsetSources` | `List<ICameraOffsetSource>` | Kapasitas 8 | Sumber rotasi dan posisi tambahan |
| `forcedLookTarget` | `Transform` | `null` | Target paksa, untuk #110 |
| `yawLimitCenter`, `yawLimitRange` | `float` | 0, tanpa batas | Batas yaw saat di tangga |
| `eyeHeightCurrent` | `float` | `camera.eyeHeightStand` | Tinggi mata saat ini |
| `viewmodelMode` | `ViewmodelMode` | `Visible` | `Visible`, `Lowered`, atau `Hidden` |
| `skipLookFrames` | `int` | 0 | Mengabaikan lonjakan mouse |

```csharp
public interface ICameraOffsetSource
{
    Vector3 EulerOffset { get; }      // Derajat
    Vector3 PositionOffset { get; }   // Meter, lokal
}

public enum ViewmodelMode { Visible, Lowered, Hidden }

[Serializable]
public struct ViewmodelPose
{
    public Vector3 position;
    public Vector3 rotation;
    public float swayMultiplier;
    public float bobMultiplier;
}
```

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.camera.maxLookAngle` | 80 | Batas atas dan bawah, sama dengan `ShipPlayerController` |
| `GameBalanceConfig.camera.baseFov` | 70 | FOV dunia. Pemain bisa mengubahnya di #19 |
| `GameBalanceConfig.camera.viewmodelFov` | 55 | FOV tangan, tidak ikut berubah |
| `GameBalanceConfig.camera.eyeHeightStand` | 1,6 | Tinggi mata berdiri, dari `ShipPlayerController` |
| `GameBalanceConfig.camera.eyeHeightSpeed` | 6 | Kecepatan perubahan tinggi mata, dalam m/s |
| `GameBalanceConfig.camera.maxOffsetDegrees` | 15 | Batas total rotasi tambahan per sumbu |
| `GameBalanceConfig.camera.recoilRecoverPerSec` | 12 | Pemulihan recoil, dalam derajat per detik |
| `GameBalanceConfig.camera.swayAmount` | 0,02 | Jarak weapon sway per satuan mouse |
| `GameBalanceConfig.camera.swayMax` | 0,06 | Batas jarak sway |
| `GameBalanceConfig.camera.swaySmooth` | 10 | Kecepatan kembalinya sway |
| `GameBalanceConfig.camera.bobFrequency` | 9 | Frekuensi head bob saat jalan, dalam radian per detik |
| `GameBalanceConfig.camera.bobAmplitude` | 0,025 | Tinggi head bob, dalam meter |
| `GameBalanceConfig.camera.bobSprintMultiplier` | 1,5 | Pengali head bob saat lari |
| `GameBalanceConfig.camera.bobHeavyMultiplier` | 1,8 | Pengali head bob saat membawa Heavy Item |
| `GameBalanceConfig.camera.fovSprintBonus` | 8 | Tambahan FOV saat lari |
| `GameBalanceConfig.camera.fovChangeSpeed` | 40 | Kecepatan perubahan FOV, dalam derajat per detik |
| `GameBalanceConfig.camera.landingDipMax` | 0,25 | Dip kamera terbesar saat mendarat |
| `GameBalanceConfig.camera.ladderYawRange` | 70 | Batas yaw saat di tangga |
| `GameBalanceConfig.camera.lowerSpeed` | 8 | Kecepatan viewmodel turun dan naik |

### Pengaturan pemain dari #19

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `SettingsManager.mouseSensitivity` | 0,12 | Pengali `LookDelta` |
| `SettingsManager.invertY` | `false` | Membalik sumbu Y |
| `SettingsManager.fieldOfView` | 70 | Menggantikan `baseFov` |
| `SettingsManager.headBobEnabled` | `true` | Head bob |
| `SettingsManager.weaponSwayEnabled` | `true` | Weapon sway |
| `SettingsManager.fovKickEnabled` | `true` | Perubahan FOV saat lari |

Nilai sensitivitas lama `2` mengikuti `Input.GetAxis`. Nilainya berubah karena `LookDelta` berupa jarak piksel per frame (#18).

## 2. Detail Logika & Fungsi

### Mouse look

**`void Update()`**, membaca input lebih dulu:

```csharp
if (skipLookFrames > 0) { skipLookFrames--; return; }
Vector2 look = InputManager.Instance.LookDelta * settings.mouseSensitivity * lookScale;
yaw += look.x;
pitch += settings.invertY ? look.y : -look.y;
pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);
if (yawLimitRange > 0f) yaw = yawLimitCenter + Mathf.Clamp(Mathf.DeltaAngle(yawLimitCenter, yaw), -yawLimitRange, yawLimitRange);
playerBody.rotation = Quaternion.Euler(0f, yaw, 0f);
```

`LookDelta` tidak dikali `Time.deltaTime`. Fungsi ini mendengar `UIFocusChangedEvent`, dan mengatur `skipLookFrames = 1` saat `isGameplayBlocked` berubah menjadi `false`, supaya kamera tidak melompat setelah panel tertutup.

**`void LateUpdate()`**, dengan `[DefaultExecutionOrder(200)]`, berjalan setelah gerak pemain dan sumber lain:

1. Perbarui `CameraRig.localPosition.y` dari `eyeHeightCurrent`, memakai `Mathf.MoveTowards`.
2. Atur `PitchPivot.localRotation = Quaternion.Euler(pitch, 0, 0)`.
3. Gabungkan semua sumber offset, lalu terapkan ke `OffsetPivot`.
4. Perbarui FOV.
5. Perbarui weapon sway dan pose viewmodel.

### Menggabungkan rotasi

Semua rotasi tambahan masuk ke `OffsetPivot`, jadi `pitch` dan `yaw` pemain tidak pernah tercemar. Setelah recoil dan getaran hilang, bidikan kembali ke posisi yang pemain atur.

| Sumber | Pemilik | Sumbu | Masuk lewat | Keluar |
|---|---|---|---|---|
| Recoil | `GunController` (#58) | Pitch dan yaw | `AddRecoil(float pitchUp, float yawSide)` | Pulih `recoilRecoverPerSec` |
| Getaran | `CameraShake` (#24) | Semua | `ICameraOffsetSource` | Diatur #24 |
| Ship sway | `ShipHazardManager` (#75) | Roll dan sedikit pitch | `ICameraOffsetSource` | Berakhir bersama scene |
| Head bob | Komponen ini | Posisi Y dan sedikit roll | Internal | Kembali saat diam |
| Dip mendarat | `PlayerTraversal` (#101) | Posisi Y | `PlayLandingDip(float impact01)` | Pulih pegas |
| Pinned | #110 | Rotasi | `SetForcedLook(Transform)` | Lepas dengan `SetForcedLook(null)` |

```csharp
Vector3 euler = recoilEuler + bobEuler;
Vector3 pos = bobPosition + landingPosition;
for (int i = 0; i < offsetSources.Count; i++)
{
    euler += offsetSources[i].EulerOffset;
    pos += offsetSources[i].PositionOffset;
}
euler = ClampPerAxis(euler, maxOffsetDegrees);
offsetPivot.localRotation = Quaternion.Euler(euler);
offsetPivot.localPosition = pos;
```

**Alasan penjumlahan:** semua nilainya sudut kecil, jadi urutannya tidak berpengaruh besar, dan batas `maxOffsetDegrees` mencegah kamera terbalik saat beberapa efek terjadi bersamaan.

**`public void RegisterOffsetSource(ICameraOffsetSource s)` dan `UnregisterOffsetSource(...)`** dipanggil pemilik sumber pada `OnEnable` dan `OnDisable`. Daftar berupa `List` yang dikunjungi lewat indeks, jadi tidak ada garbage.

**Recoil dan bidikan:** raycast tembakan #58 memakai `WorldCamera`, sehingga recoil benar-benar menggeser bidikan selagi offset masih ada.

### Weapon sway, head bob, dan FOV lari

**Weapon sway:** `ViewmodelRoot` bergeser berlawanan arah mouse.

```csharp
Vector2 target = -Vector2.ClampMagnitude(lookDelta * swayAmount, swayMax) * pose.swayMultiplier;
swayOffset = Vector2.Lerp(swayOffset, target, swaySmooth * Time.deltaTime);
```

**Head bob:** memakai kecepatan horizontal dari `CharacterController.velocity`, bukan input, supaya kamera diam saat pemain menabrak dinding.

- Hanya aktif kalau `isGrounded` dan kecepatan di atas 0,5 m/s.
- `bobPhase += bobFrequency * (speed / walkSpeed) * Time.deltaTime`.
- Posisi: `Mathf.Sin(bobPhase) * bobAmplitude`, dikali `bobSprintMultiplier` saat lari.
- Viewmodel ikut bergerak, dikali `pose.bobMultiplier`.

**FOV lari:** nilai target `fieldOfView + fovSprintBonus` saat `IslandPlayerController.IsSprinting`, memakai `Mathf.MoveTowards`.

**Pilihan menonaktifkan:** tiap efek dari #19 diperiksa sebelum dihitung. Kalau mati, nilainya menjadi 0, dan efek itu tidak dijalankan. `ship sway` dan getaran tidak dimatikan di sini, karena pengaturannya ada di #24 dan #75.

### Posisi tiap item

Pose disimpan di `EquippableBehaviour.pose` (#60). Nilai awal, posisi dalam meter terhadap `ViewmodelCamera`:

| Item | Posisi | Rotasi | Sway | Bob |
|---|---|---|---|---|
| `Base_Pistol` | (0,18, -0,16, 0,35) | (0, -2, 0) | 1 | 1 |
| `Base_Pisau` | (0,22, -0,20, 0,38) | (15, -20, 0) | 1,2 | 1 |
| `Base_Kail` | (0,20, -0,22, 0,45) | (20, -10, 0) | 0,8 | 1 |
| Kapak dan Beliung | (0,25, -0,28, 0,50) | (10, -25, 0) | 0,7 | 1,2 |
| Palu | (0,22, -0,24, 0,42) | (10, -20, 0) | 0,9 | 1 |
| Obor (tangan kiri) | (-0,24, -0,22, 0,42) | (5, 10, 0) | 0,8 | 1 |
| Senter (tangan kiri) | (-0,20, -0,20, 0,40) | (0, 5, 0) | 0,8 | 1 |
| Tangan kosong | (0,22, -0,30, 0,40) | (0, -10, 0) | 1 | 1 |
| Heavy Item (dua tangan) | (0, -0,40, 0,70) | (0, 0, 0) | 0,4 | 1,8 |

Saat berganti item, `EquipmentManager` memanggil `SetMainPose(ViewmodelPose)`. Turun dan naik saat berganti memakai `SetViewmodelMode`, dengan kecepatan `lowerSpeed`.

### Aim down sights

**Rekomendasi: tidak ada ADS di versi awal.** Referensi combat adalah L4D (`GLOSSARY.md`), yang tidak memakai ADS. Klik kanan pada pistol dibiarkan kosong (#60). `SetMainPose` sudah cukup untuk menambah pose ADS nanti tanpa mengubah susunan kamera.

### Keadaan khusus

| Keadaan | Pemicu | Kamera | Viewmodel |
|---|---|---|---|
| **Jongkok** | `SetEyeHeight(eyeHeightCrouch)` dari #101 | Tinggi mata turun halus | Tetap |
| **Tangga** | `EnterClimbView(Vector3 ladderForward)` dari #101 | Yaw dibatasi `ladderYawRange`, dan pitch bebas | `Lowered`. Item turun keluar layar |
| **Membawa Heavy Item** | `HeavyItemChangedEvent` | `lookScale = 0,8`, bob dikali `bobHeavyMultiplier`, dan tidak ada FOV lari | `Hidden` untuk item, dan model barang tampil di pose Heavy Item |
| **Diterkam** | `SetForcedLook(attacker)` dari #110 | Rotasi menuju penyerang, dan input mouse dimatikan | `Lowered`, kecuali pisau |
| **Mati** | `DetachForDeath()` dari #53 | Input berhenti, offset dinolkan, dan rig dilepas dari pemain | `Hidden`. `ViewmodelCamera` dimatikan, dan #53 mengendalikan `WorldCamera` |
| **Adegan** | Panel `Cinematic` | `Gameplay` mati. #99 boleh memakai kamera lain | `Hidden` |
| **Fase 1** | Rig sama, tanpa item | Ship sway aktif, dan tangan kosong tampil | `Visible` |

**`public void DetachForDeath()`** memanggil `CameraRig.SetParent(null, true)`, sehingga kamera tidak ikut terjatuh bersama `CharacterController` yang dimatikan.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** tidak ada.

**Event yang didengar:** `UIFocusChangedEvent`, `HeavyItemChangedEvent`, `EquippedItemChangedEvent`.

| Sistem | Hubungan |
|---|---|
| `InputManager` (#18) | Menyediakan `LookDelta` |
| `SettingsManager` (#19) | Sensitivitas, FOV, dan pilihan efek. Nilainya dibaca langsung tiap frame, jadi perubahan menu langsung berlaku |
| `ShipPlayerController`, `IslandPlayerController` | Tidak lagi punya mouse look |
| `CameraShake` (#24) | Mengisi `ICameraOffsetSource`. Kalau memakai Cinemachine Impulse, hasil impulsnya diubah menjadi offset ini |
| `GunController` (#58) | `AddRecoil`, dan raycast dari `WorldCamera` |
| `EquipmentManager` (#60) | Mengatur pose dan mode viewmodel |
| `PlayerInteractor` (#17), `BuildManager` | Memakai `WorldCamera` |
| `PlayerTraversal` (#101) | `SetEyeHeight`, `EnterClimbView`, `ExitClimbView`, dan `PlayLandingDip` |
| `ShipHazardManager` (#75) | Sumber ship sway |
| `DeathCinematic` (#53) | `DetachForDeath` |
| `PlayerPinnedState` (#110) | `SetForcedLook` |
| `AnimationController` (#12) | Animator tangan di dalam viewmodel |

**Batas pekerjaan:** getaran di #24, animator di #12, dan tombol di #18.

## 4. Kasus Khusus

- **Objek pool:** rig dan viewmodel tidak memakai `ObjectPooler`, karena hanya ada satu. Muzzle flash dari pool (#58) yang tampil di viewmodel harus berada di layer `PlayerViewmodel`.
- **Game dijeda:** input `Gameplay` mati, dan `Time.deltaTime` bernilai 0. Efek berhenti di tempat, dan kembali berjalan setelah jeda.
- **Permadeath:** rig ikut hilang bersama scene. Rig yang dilepas `DetachForDeath` masih berada di scene aktif, jadi ikut terbuang. `SettingsManager` tidak direset.
- **Save dan load:** `yaw` dan `pitch` disimpan bersama posisi pemain lewat `WorldStateSave` (#80). Tinggi mata tidak disimpan, karena pemain selalu mulai berdiri. Efek sementara tidak disimpan.
- **Perpindahan fase:** rig baru dibuat bersama scene. Rotasi tidak dibawa dari kapal, dan #74 mengatur arah pandang awal di pantai.
- **Lonjakan mouse:** `skipLookFrames` menangani panel dan Alt-Tab, karena #83 membuka Pause saat kehilangan fokus.
- **Teleport (#56, #88):** rig mengikuti pemain karena berupa anak objek.
- **Head bob saat jeda input:** kecepatan pemain menjadi 0, jadi bob kembali ke tengah dengan `Mathf.MoveTowards`.
- **Dua sumber berhenti bersamaan:** `UnregisterOffsetSource` aman dipanggil dua kali.
- **Sumber offset dimatikan di tengah efek:** nilainya hilang pada frame berikutnya, tanpa sisa.
- **Pitch saat keluar dari `SetForcedLook`:** `yaw` dan `pitch` diisi dari rotasi akhir, supaya tidak melompat.
- **Viewmodel saat FOV diubah:** `viewmodelFov` tetap, jadi tangan tidak melebar.
- **Mesh item menembus near clip:** near 0,01 khusus kamera tangan mencegah kliping.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** layer `PlayerViewmodel` (`PhysicsLayers.md`). `LookDelta` tanpa `Time.deltaTime` (`InputManager.md`). Referensi combat L4D (`GLOSSARY.md`). Kamera pada `Y = 1,6` dan batas `80` derajat (`ShipPlayerController.md`).

**Keputusan teknis di blueprint ini:**

- Rotasi dasar dan rotasi tambahan dipisah lewat `PitchPivot` dan `OffsetPivot`.
- Rotasi tambahan dijumlahkan, dengan batas per sumbu.
- Satu prefab rig untuk dua fase.

**Usulan yang disetujui developer sebagai nilai awal (dicatat di #85):**

1. **Aim down sights:** tidak ada di versi awal, mengikuti L4D.
2. **Nilai awal kamera:** FOV 70, FOV tangan 55, tambahan FOV lari 8, dan head bob 2,5 cm.
3. **Pilihan pemain:** head bob, weapon sway, dan FOV lari bisa dimatikan di #19.
4. **Tangga:** yaw dibatasi 70 derajat, dan senjata turun.
5. **Heavy Item:** barang tampil di layar, menutupi sebagian pandangan bawah, dengan sensitivitas 80 persen.
6. **Pipeline render:** URP dengan Camera Stack. Proyek Unity belum dibuat, jadi ini perlu dipastikan saat proyek dibuat.

## 6. Skenario Tes

1. Gerakkan mouse di `ShipPhase` dan `IslandPhase`. **Hasil:** perilaku sama, dan tidak ada error `Input` lama.
2. Ubah sensitivitas dan membalik sumbu Y di #19. **Hasil:** langsung berlaku.
3. Hadapkan pistol ke dinding dari jarak 0,1 meter. **Hasil:** pistol tidak menembus dinding.
4. Tembak 5 kali tanpa menggerakkan mouse. **Hasil:** kamera naik, lalu kembali ke bidikan awal.
5. Tembak saat kena ledakan di kapal yang miring. **Hasil:** semua efek tergabung, dan kamera tidak terbalik.
6. Jalan, lari, dan diam. **Hasil:** head bob mengikuti kecepatan, dan FOV naik 8 hanya saat lari.
7. Berjalan menabrak dinding. **Hasil:** head bob berhenti.
8. Matikan head bob, sway, dan FOV lari di #19. **Hasil:** ketiganya berhenti, dan tembakan tetap normal.
9. Ganti Pistol, Pisau, Kail, Kapak, dan Obor. **Hasil:** tiap item tampil di pose-nya tanpa terpotong.
10. Naik tangga. **Hasil:** yaw terbatas, senjata turun, lalu kembali setelah turun dari tangga.
11. Ambil Heavy Item. **Hasil:** model barang tampil, senjata hilang, dan FOV tidak berubah saat mencoba lari.
12. Mati. **Hasil:** kamera lepas dari badan, dan viewmodel hilang.
13. Buka panel, gerakkan mouse, lalu tutup. **Hasil:** kamera tidak melompat.
14. Simpan saat menghadap utara, keluar, lalu lanjutkan. **Hasil:** arah pandang sama.
15. Biarkan pemain diterkam, lalu lepas. **Hasil:** kamera tidak melompat setelah lepas.
16. Buka Profiler selama 60 detik sambil bergerak. **Hasil:** tidak ada alokasi GC dari `LateUpdate`.
