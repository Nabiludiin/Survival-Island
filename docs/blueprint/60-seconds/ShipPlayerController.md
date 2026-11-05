# Blueprint: ShipPlayerController.cs (First-Person Edition)
**Lokasi:** `docs/blueprint/60-seconds/ShipPlayerController.md`
**Deskripsi Utama:** Pergerakan pemain Fase 1. Kamera memakai prefab rig `FirstPersonCamera`, interaksi memakai `PlayerInteractor`, dan input memakai `InputManager`.
**Issue:** #17, #18, #74, #75, #83, #102
**Rujukan GDD:** Bagian 1 (genre first-person) dan bagian 3 (Fase 1 di kapal). Level kontrol dan aksi dorong adalah usulan.

## 1. Arsitektur & Variabel Utama

```csharp
public enum ShipControlLevel { None, LookOnly, Full }
```

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `controller` | `CharacterController` | Dari prefab | Pergerakan |
| `moveSpeed` | `float` | 5 | Kecepatan dasar |
| `controlLevel` | `ShipControlLevel` | `None` | Level kontrol saat ini |
| `waterMul`, `tiltMul` | `float` | 1 | Pengali dari `ShipHazardManager` (#75) |
| `drift` | `Vector3` | Nol | Geser kapal dari `ShipHazardManager` |

**Dihapus:** mouse look, `firstPersonCamera`, `mouseSensitivity`, `maxLookAngle`, `xRotation`, rumus `zSway`, raycast interaksi, `currentHoveredObject`, `Input.GetAxis`, serta penulisan `Cursor.lockState` dan `Cursor.visible`. Kursor diatur `UIFocusStack`.

## 2. Detail Logika & Fungsi

**`public void SetControlLevel(ShipControlLevel level)`**:

| Level | Gerak | Menengok | Interaksi |
|---|---|---|---|
| `None` | Tidak | Tidak | Tidak |
| `LookOnly` | Tidak | Ya | Tidak |
| `Full` | Ya | Ya | Ya |

Fungsi ini mengatur `FirstPersonCamera` dan `PlayerInteractor`.

**`public void SetShipMoveModifiers(float waterMul, float tiltMul, Vector3 drift)`**: menerima pengali dari `ShipHazardManager`.

**`void Update()`**:

1. Keluar dari gerak kalau `controlLevel != Full`.
2. Baca `InputManager.Instance.MoveInput`.
3. `Vector3 move = transform.right * x + transform.forward * z`.
4. `controller.Move((move * moveSpeed * waterMul * tiltMul + drift) * Time.deltaTime)`.
5. Kalau `InputManager.Instance.PrimaryPressed`, jalankan `ShipShove`.

**Interaksi:** tombol `E` dibaca `PlayerInteractor` (#17), bukan kelas ini. Ship sway memakai `ShipSwaySource` sebagai `ICameraOffsetSource` (#75, #102).

## 3. Hubungan dengan Sistem Lain

**Event:** tidak mengirim dan tidak mendengar event.

| Sistem | Hubungan |
|---|---|
| `InputManager` (#18) | Sumber `MoveInput` dan `PrimaryPressed` |
| `FirstPersonCamera` (#102) | Membaca `LookDelta`, dan mengatur kamera |
| `PlayerInteractor` (#17) | Membaca `InteractPressed` |
| `LifeboatExtraction` (#74) | Memanggil `SetControlLevel` |
| `ShipHazardManager` (#75) | Memanggil `SetShipMoveModifiers`, dan memiliki `ShipShove` |
| `UIFocusStack` (#83) | Mengatur kursor, dan mematikan input lewat Action Map |

## 4. Kasus Khusus

- **Objek pool:** pemain tidak memakai pool.
- **Game dijeda:** `Gameplay` mati, dan `Time.deltaTime` bernilai 0.
- **Permadeath:** kelas ini hilang bersama `ShipPhase`.
- **Save dan load:** Fase 1 tidak punya save (#82).
- **Mati:** `DeathCinematic.DisableController` mematikan kelas ini.
- **Waktu habis:** `LifeboatExtraction` memanggil `SetControlLevel(None)`, jadi pemain tidak bisa mengambil barang lagi.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** `SetControlLevel` (`LifeboatExtraction.md`), `SetShipMoveModifiers` dan `ShipShove` (`ShipHazardManager.md`), kamera (`FirstPersonCamera.md`), interaksi (`IslandInteractable.md`).

**Menunggu developer:** tidak ada yang menghalangi.

## 6. Skenario Tes

1. Pada `LookOnly`. **Hasil:** pemain menengok, tetapi tidak bisa bergerak dan berinteraksi.
2. Pada `Full`, tekan `W`. **Hasil:** bergerak dengan kecepatan 5 tanpa air.
3. Berdiri di air dengan `waterMul` 0,5. **Hasil:** kecepatan setengah.
4. Klik kiri di dekat zombie. **Hasil:** `ShipShove` berjalan.
5. Arahkan ke barang, lalu tekan `E`. **Hasil:** `PlayerInteractor` yang memprosesnya.
6. Ganti tombol `Move`. **Hasil:** tombol baru berlaku.
7. Tekan `Esc`. **Hasil:** kursor muncul, pemain berhenti, dan kelas ini tidak menulis kursor.
