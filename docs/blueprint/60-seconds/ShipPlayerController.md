# Blueprint: ShipPlayerController.cs (First-Person Edition)
**Lokasi:** `docs/blueprint/60-seconds/ShipPlayerController.md`
**Deskripsi Utama:** Pergerakan pemain Fase 1 dan level kontrol. Kamera memakai prefab rig `FirstPersonCamera`, interaksi memakai `PlayerInteractor`, input memakai `InputManager`, dan bahaya kapal memberi pengali gerak.
**Issue:** #17, #18, #74, #75, #83, #102
**Rujukan GDD:** Bagian 1 (genre first-person) dan bagian 3 (Fase 1 di kapal yang miring dan dipenuhi air). Level kontrol, aksi dorong, dan gerak tanpa lari atau lompat adalah usulan yang sudah disetujui sebagai nilai awal (#85).

## 1. Arsitektur & Variabel Utama

### Pola

`ShipPlayerController : MonoBehaviour` berada di prefab pemain Fase 1. Kelas ini hanya mengatur gerak dan level kontrol. Kamera, interaksi, dorongan, dan status pemain berada di komponen lain pada prefab yang sama.

```text
Player_Ship (layer Player, tag Player)
├── CharacterController
├── ShipPlayerController      ← kelas ini
├── PlayerInteractor (#17)
├── ShipShove (#75)
├── SurvivalStats (#111)
├── FootstepAudio (#21)
├── DeathCinematic (#53)
└── CameraRig (FirstPersonCamera, #102)
```

`PlayerTraversal` dan `EquipmentManager` **tidak** dipasang di pemain Fase 1 (`PlayerTraversal.md`, `ShipHazardManager.md`).

```csharp
public enum ShipControlLevel { None, LookOnly, Full }
```

### Gerak di Fase 1

| Aksi | Ada di kapal | Alasan |
|---|---|---|
| Jalan `Move` | Ya | Kecepatan tetap 5 m/s |
| Lari `Sprint` | Tidak | Jarak rute di #75 dihitung dengan 5 m/s. Stamina tidak dipakai di kapal |
| Lompat `Jump` | Tidak | Antar-dek memakai tangga. Mencegah melompati puing koridor runtuh |
| Jongkok `Crouch` | Tidak | Tidak ada tempat sempit di kapal |
| Klik kiri `PrimaryAction` | Ya | Menjadi `ShipShove` (#75) |
| Interaksi `E` | Ya | Dibaca `PlayerInteractor`, bukan kelas ini |

`InputManager.md` sudah menyebut action yang tidak dipakai di kapal tidak punya pembaca, jadi tidak perlu Action Map baru.

### Variabel

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `controller` | `CharacterController` | Dari prefab | Pergerakan |
| `interactor` | `PlayerInteractor` | Dari prefab | Dimatikan dan dinyalakan sesuai level |
| `fpCamera` | `FirstPersonCamera` | Dari prefab | Mouse look dimatikan dan dinyalakan sesuai level |
| `shove` | `ShipShove` | Dari prefab | Aksi dorong |
| `ControlLevel` | `ShipControlLevel` | `None` | Level saat ini. `public get` |
| `waterMul`, `tiltMul` | `float` | 1, 1 | Pengali dari `ShipHazardManager` (#75) |
| `drift` | `Vector3` | Nol | Geser kapal dari `ShipHazardManager`, dalam m/s |
| `verticalVelocity` | `float` | 0 | Kecepatan jatuh |
| `MoveDirectionWorld` | `Vector3` | Nol | Arah gerak dunia, panjang 0 sampai 1. `public get`. Dibaca `ShipHazardManager.ComputeTiltMul` |

**Dihapus:** mouse look, `firstPersonCamera`, `mouseSensitivity`, `maxLookAngle`, `xRotation`, rumus `zSway`, raycast interaksi, `currentHoveredObject`, `Input.GetAxis`, `moveSpeed` yang tertulis langsung, serta penulisan `Cursor.lockState` dan `Cursor.visible`. Kursor diatur `UIFocusStack`.

### Pengaturan `CharacterController` (usulan)

| Field | Nilai | Alasan |
|---|---|---|
| `height` | 1,8 | Tinggi mata 1,6 dari `camera.eyeHeightStand` |
| `radius` | 0,35 | Muat di koridor kapal |
| `stepOffset` | 0,3 | Di bawah `Step Height` NavMesh 0,4, jadi pemain tidak naik ke tempat yang tidak bisa dicapai zombie |
| `slopeLimit` | 45 | Kemiringan tetap 4 derajat dan tangga berada jauh di bawahnya |
| `skinWidth` | 0,08 | Sekitar 10 persen radius, sesuai saran Unity |

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.ship.moveSpeed` | 5 | Kecepatan jalan di kapal. Sama dengan `walkSpeed` pulau. Dipakai #75 untuk menghitung jarak rute |
| `GameBalanceConfig.ship.gravity` | -9,81 | Gravitasi pemain kapal |
| `GameBalanceConfig.ship.groundedStickVelocity` | -2 | Kecepatan bawah saat menapak, supaya tetap menempel di tangga turun |
| `GameBalanceConfig.shipHazard.*` | Lihat #75 | `waterMul`, `tiltMul`, dan `drift` dihitung di sana |

Tiga angka pertama adalah usulan baru di `ShipBalance`. Kalau `IslandPlayerController` memindahkan `gravity` ke `GameBalanceConfig.traversal` (#84), kelas ini sebaiknya memakai nilai yang sama.

## 2. Detail Logika & Fungsi

### Persiapan

**`void Awake()`**: ambil `controller`, `interactor`, `fpCamera`, dan `shove` dari prefab. Kalau salah satu kosong, catat error.

**`void Start()`**: panggil `SetControlLevel(ShipControlLevel.None)`. Ini menyamakan kamera dan interaksi dengan nilai awal, sebelum `LifeboatExtraction.PreGameRoutine` membuka `LookOnly` (#74).

Kelas ini **tidak** memanggil `SurvivalStats.SetDrainEnabled(false)`. Pemanggilnya `LifeboatExtraction.PreGameRoutine` langkah 3.

### Level kontrol

**`public void SetControlLevel(ShipControlLevel level)`**, dipanggil `LifeboatExtraction`:

| Level | Gerak | Menengok | Interaksi | Dorong | Kapan |
|---|---|---|---|---|---|
| `None` | Tidak | Tidak | Tidak | Tidak | Sebelum pra-game, saat `Resolving`, dan saat ringkasan |
| `LookOnly` | Tidak | Ya | Tidak | Tidak | Jeda pra-game 3 detik |
| `Full` | Ya | Ya | Ya | Ya | Selama `Scavenge` |

Langkah:

1. `ControlLevel = level`.
2. `fpCamera.SetLookEnabled(level != ShipControlLevel.None)`.
3. `interactor.SetEnabled(level == ShipControlLevel.Full)`. `SetEnabled(false)` membersihkan target dan prompt (#17).
4. Kalau `level != Full`: `MoveDirectionWorld = Vector3.zero`.

Gravitasi tetap berjalan di semua level, supaya pemain tidak melayang. Geser `drift` hanya berlaku pada `Full`.

**Catatan kontrak:** `FirstPersonCamera.SetLookEnabled(bool)` sudah ada di `FirstPersonCamera.md` (`lookEnabled`). Saat mati, `LookDelta` diabaikan, tetapi `LateUpdate` tetap menerapkan ship sway dan getaran. Mematikan komponen kamera tidak boleh dipakai, karena getaran `FinalShudder` pada `T=0` ikut hilang.

### Pengali dari bahaya kapal

**`public void SetShipMoveModifiers(float waterMul, float tiltMul, Vector3 drift)`**, dipanggil `ShipHazardManager.UpdatePlayerEffects()` tiap frame:

1. `this.waterMul = Mathf.Clamp01(waterMul)`.
2. `this.tiltMul = Mathf.Max(0f, tiltMul)`.
3. `this.drift = new Vector3(drift.x, 0f, drift.z)`. Komponen vertikal dibuang.

Nilai berlaku pada `Update` berikutnya. Jeda satu frame tidak terasa, jadi urutan eksekusi tidak perlu diatur.

### Per frame

**`void Update()`**:

```csharp
// 1. Gravitasi, di semua level
if (controller.isGrounded && verticalVelocity < 0f) verticalVelocity = ship.groundedStickVelocity;
else verticalVelocity += ship.gravity * Time.deltaTime;

// 2. Gerak horizontal, hanya Full
Vector3 horizontal = Vector3.zero;
if (ControlLevel == ShipControlLevel.Full)
{
    Vector2 input = Vector2.ClampMagnitude(InputManager.Instance.MoveInput, 1f);
    MoveDirectionWorld = transform.right * input.x + transform.forward * input.y;
    horizontal = MoveDirectionWorld * (ship.moveSpeed * waterMul * tiltMul) + drift;
}

// 3. Satu kali Move per frame
controller.Move((horizontal + Vector3.up * verticalVelocity) * Time.deltaTime);

// 4. Dorong
if (ControlLevel == ShipControlLevel.Full && InputManager.Instance.PrimaryPressed) shove.TryShove();
```

- **`ClampMagnitude`:** gerak diagonal tidak lebih cepat dari 5 m/s.
- **Satu `Move` per frame:** gravitasi dan gerak digabung, supaya `isGrounded` akurat.
- **Arah kamera:** `transform` pemain sudah diputar `FirstPersonCamera` lewat yaw (#102), jadi `forward` mengikuti arah pandang.
- **`MoveDirectionWorld`** diisi sebelum dikali kecepatan. `ShipHazardManager` memakainya untuk `Dot(moveDir, downhill)` pada frame berikutnya.
- **Dorong** memakai `PrimaryPressed`, bukan `PrimaryHeld`, jadi tidak beruntun. Jeda dan isi dorongan diatur `ShipShove` (#75).

### Contoh kecepatan akhir

| Keadaan | `waterMul` | `tiltMul` | Kecepatan |
|---|---|---|---|
| Lantai kering, datar | 1 | 1 | 5 m/s |
| Kering, menanjak penuh | 1 | 0,88 | 4,4 m/s |
| Kering, menurun penuh | 1 | 1,12 | 5,6 m/s |
| Air selutut, datar | 0,6 | 1 | 3 m/s |
| Air sepinggang, menanjak | 0,35 | 0,88 | sekitar 1,5 m/s |
| Diam pada tahap 3 | Tidak berpengaruh | Tidak berpengaruh | Geser 0,4 m/s ke sisi rendah |

Zombie kapal berjalan 3,5 m/s dikali pengali air yang sama (#75). Di lantai kering pemain selalu lebih cepat.

### Saat dimatikan

**`void OnDisable()`**: `verticalVelocity = 0`, `MoveDirectionWorld = Vector3.zero`, dan `drift = Vector3.zero`. `DeathCinematic.DisableController` mematikan kelas ini sebelum mematikan `CharacterController` (#53).

## 3. Hubungan dengan Sistem Lain

**Event:** tidak mengirim dan tidak mendengar event. Kelas ini dikendalikan pemanggilan langsung dari `LifeboatExtraction` dan `ShipHazardManager`, yang memang menjadi pemilik alurnya (`GameEventBus.md`, pemanggilan langsung boleh).

| Sistem | Hubungan |
|---|---|
| `InputManager` (#18) | Sumber `MoveInput` dan `PrimaryPressed` |
| `FirstPersonCamera` (#102) | Membaca `LookDelta`, memutar yaw pemain, dan menerima `SetLookEnabled` |
| `PlayerInteractor` (#17) | Membaca `InteractPressed`. Menerima `SetEnabled` |
| `ShipShove` (#75) | Menerima `TryShove()` |
| `LifeboatExtraction` (#74) | Memanggil `SetControlLevel` |
| `ShipHazardManager` (#75) | Memanggil `SetShipMoveModifiers`, dan membaca `MoveDirectionWorld` serta `controller` |
| `SurvivalStats` (#111) | Status pemain. Drain dimatikan `LifeboatExtraction` |
| `DeathCinematic` (#53) | Mematikan kelas ini saat mati |
| `FootstepAudio` (#21) | Membaca `CharacterController`, tanpa bergantung pada kelas ini |
| `UIFocusStack` (#83) | Mengatur kursor, dan mematikan input lewat Action Map |
| `GameBalanceConfig` (#84) | `ship.moveSpeed`, `ship.gravity`, dan `ship.groundedStickVelocity` |

## 4. Kasus Khusus

- **Objek pool:** pemain tidak memakai pool.
- **Game dijeda:** `Gameplay` mati, jadi `MoveInput` bernilai nol. `Time.deltaTime` bernilai 0, jadi gravitasi dan `drift` juga berhenti. Level kontrol tidak berubah.
- **Permadeath:** kelas ini hilang bersama `ShipPhase`. Run baru mulai dari `Start` dengan level `None`.
- **Save dan load:** Fase 1 tidak punya save (#82).
- **Mati:** `DeathCinematic.DisableController` mematikan kelas ini. `OnDisable` menolkan kecepatan, dan kemiringan kapal tetap berlaku pada tubuh (#53).
- **Waktu habis atau berangkat:** `LifeboatExtraction` memanggil `SetControlLevel(None)`. Pemain berhenti, tidak bisa menengok, dan tidak bisa mengambil barang. Getaran terakhir tetap tampil.
- **Ringkasan terbuka:** panel `RunSummary` menjeda waktu. Level tetap `None`.
- **Koridor runtuh saat pemain di dalamnya:** `ShipCorridor.Collapse` memindah pemain lewat `controller.Move` ke `exitPoint` (#75). `verticalVelocity` tidak diubah, jadi pemain langsung jatuh ke lantai kalau titiknya sedikit di atas lantai.
- **Terjebak di dalam air dalam:** pemain tetap bisa berjalan 35 persen. HP turun lewat `SetDrowning` (#75).
- **Tertahan `lowerDeckBarrier`:** collider biasa menahan `CharacterController`.
- **Menabrak zombie:** layer `Player` bertabrakan dengan `Enemy`, jadi pemain tidak menembus zombie. Pemain harus menghindar atau mendorong.
- **Melewati barang dan NPC kapal:** layer `Interactable` tidak bertabrakan dengan `Player`, jadi tidak menghalangi.
- **Klik kiri saat `LookOnly`:** diabaikan.
- **Menekan `Sprint`, `Jump`, atau angka hotbar:** tidak ada pembaca, jadi tidak ada efek dan tidak ada error.
- **`MoveInput` dari gamepad dengan nilai kecil:** deadzone diatur Input System (#18).
- **Fokus jendela hilang:** `InputManager` mereset device, jadi pemain tidak terus berjalan (#18).

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** `SetControlLevel` dan tiga level (`LifeboatExtraction.md`). `SetShipMoveModifiers`, `ShipShove`, dan kecepatan 5 m/s untuk rute (`ShipHazardManager.md`). Kamera dan tinggi mata (`FirstPersonCamera.md`). Interaksi `E` lewat `PlayerInteractor` (`IslandInteractable.md`, #85). Pistol dan Pisau tidak dipakai di kapal (#75, disetujui developer).

**Keputusan teknis di blueprint ini:**

- Gravitasi dan gerak digabung dalam satu `controller.Move`.
- Gravitasi tetap berjalan di semua level kontrol.
- `MoveDirectionWorld` dibuka untuk perhitungan kemiringan #75.
- Pemain kapal tidak berlari, tidak melompat, dan tidak berjongkok.

**Usulan baru (menunggu persetujuan developer):**

1. Angka baru `ship.moveSpeed`, `ship.gravity`, dan `ship.groundedStickVelocity` di `GameBalanceConfig`.
2. Pengaturan `CharacterController` di tabel bagian 1.
3. Tanpa lari, lompat, dan jongkok di kapal. Rute #75 sudah dihitung dengan jalan 5 m/s, jadi lari akan membuat jadwal air terlalu mudah.

**Perlu perubahan di blueprint lain:** tidak ada. `FirstPersonCamera.SetLookEnabled(bool)` dan `ShipShove.TryShove()` sudah tertulis di `FirstPersonCamera.md` dan `ShipHazardManager.md`. Enum `ShipControlLevel` hanya didefinisikan di sini.

**Menunggu developer:** tidak ada yang menghalangi.

## 6. Skenario Tes

1. Masuk `ShipPhase` sebelum panel `Cinematic` tertutup. **Hasil:** level `None`, pemain tidak bisa menengok, dan tidak ada prompt.
2. Pada `LookOnly`. **Hasil:** pemain menengok, tetapi tidak bisa bergerak, berinteraksi, atau mendorong.
3. Pada `Full`, tekan `W` di lantai datar yang kering. **Hasil:** bergerak 5 m/s. Ukur dengan dua titik berjarak 10 meter, sekitar 2 detik.
4. Tekan `W` dan `D` bersamaan. **Hasil:** kecepatan tetap 5 m/s, tidak 7.
5. Berdiri di air selutut. **Hasil:** kecepatan 3 m/s.
6. Berjalan menanjak lalu menurun di lantai miring. **Hasil:** kecepatan berbeda sekitar 12 persen.
7. Diam pada `T<15`. **Hasil:** pemain bergeser perlahan ke sisi rendah. Pada `LookOnly` atau `None`, tidak bergeser.
8. Turun tangga dek. **Hasil:** pemain menempel di anak tangga, tanpa melayang atau memantul.
9. Klik kiri di dekat zombie. **Hasil:** `ShipShove.TryShove` berjalan sekali. Tahan klik kiri. **Hasil:** tidak beruntun.
10. Tekan `Space`, `Left Shift`, `Left Ctrl`, dan `1`. **Hasil:** tidak ada efek dan tidak ada error.
11. Arahkan ke barang, lalu tekan `E`. **Hasil:** `PlayerInteractor` yang memprosesnya.
12. Ganti tombol `Move` ke panah. **Hasil:** tombol baru berlaku.
13. Tekan `Esc` saat berjalan. **Hasil:** kursor muncul, pemain berhenti, tidak jatuh, dan kelas ini tidak menulis kursor.
14. Biarkan waktu habis. **Hasil:** pemain berhenti dan tidak bisa menengok, tetapi getaran `FinalShudder` tetap tampil.
15. Mati karena zombie. **Hasil:** kelas ini mati sebelum `CharacterController`, dan tidak ada error `Move` pada controller yang mati.
16. Berdiri di koridor yang runtuh pada `T=30`. **Hasil:** pemain pindah ke `exitPoint`, lalu jatuh pelan ke lantai.
17. Dorong pemain ke zombie. **Hasil:** pemain tidak menembus zombie. Berjalan melewati makanan kaleng. **Hasil:** pemain menembusnya.
18. Ubah `ship.moveSpeed` ke 4 di config. **Hasil:** kecepatan berubah tanpa mengubah kode.
19. Mati dan ulangi dari kapal tiga kali. **Hasil:** tiap run mulai dengan level `None`, dan tidak ada sisa `drift` atau kecepatan jatuh.
