# Blueprint: IslandPlayerController.cs
**Lokasi:** `docs/blueprint/Island_Logic/IslandPlayerController.md`
**Deskripsi Utama:** Skrip pergerakan Fase 2 untuk jalan, lari, lompat, gravitasi, dan pengali kecepatan. Mode gerak lain milik `PlayerTraversal`.
**Issue:** #101 (melengkapi #18, #49, #69, #102, dan #111)
**Rujukan GDD:** Bagian 5 Region 4 (suku cadang berat). Kecepatan setengah serta Sprint dan Jump mati mengikuti `Claude.md` Tahap 4. Angka gerak adalah usulan.

## 1. Arsitektur & Variabel Utama

Komponen di pemain Fase 2, bersama `CharacterController` dan `PlayerTraversal`. Mouse look dan kamera ditangani `FirstPersonCamera` (#102). Stamina dibaca dan dikurangi lewat `SurvivalStats` (#111).

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `walkSpeed`, `sprintSpeed` | `float` | 5, 8 | Kecepatan dasar |
| `gravity` | `float` | -9,81 | Gravitasi |
| `velocity` | `Vector3` | Nol | Kecepatan vertikal |
| `speedModifiers` | `float[]` | Panjang `SpeedModifierId`, isi 1 | Satu pengali per id, tanpa garbage |

| Angka | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.traversal.jumpHeight` | 1,1 | Tinggi lompat. Menggantikan nilai 2 |
| `GameBalanceConfig.vitals.sprintStaminaPerSec` | 10 | Biaya lari |
| `GameBalanceConfig.vitals.jumpStaminaCost` | 8 | Biaya lompat |

**Properti publik:**

| Nama | Dipakai oleh |
|---|---|
| `float SpeedMultiplier { get; }` | Hasil kali semua pengali, dengan batas bawah 0,1 |
| `bool IsSprinting { get; }` | `NoiseEmitter` dan `AISensoryHub` (#49), `FirstPersonCamera` (#102) |
| `bool IsGrounded { get; }` | `PlayerTraversal` (#101), `HeavyItemSystem` (#69) |

**Dihapus:** `isCarryingHeavyItem`, paksaan `currentSpeed = walkSpeed * 0.5f`, regenerasi stamina, `Physics.CheckSphere`, dan tombol `Left Shift` serta `Space` yang tertulis langsung.

## 2. Detail Logika & Fungsi

**`void Update()`**:

1. Kalau `!PlayerTraversal.AllowsGroundMove`, yaitu bukan mode `Ground` atau `Crouching`, jangan menggerakkan pemain.
2. Baca `InputManager.Instance.MoveInput`, `SprintHeld`, dan `JumpPressed`.
3. **Tanah:** `IsGrounded = CharacterController.isGrounded`, ditambah `PhysicsQuery.Raycast` pendek ke `PhysicsMasks.Ground`.
4. **Lari:** kalau `SprintHeld`, ada input maju, `!HeavyItemSystem.Instance.IsCarrying`, dan `SurvivalStats.SpendStamina(sprintStaminaPerSec * Time.deltaTime, StaminaUse.Sprint)` berhasil, `IsSprinting = true`. Kalau tidak, `IsSprinting = false`.
5. **Kecepatan akhir:** `walkSpeed` atau `sprintSpeed`, dikali `SpeedMultiplier`.
6. **Lompat:** kalau `IsGrounded`, `JumpPressed`, `!IsCarrying`, dan `SpendStamina(jumpStaminaCost, StaminaUse.Jump)` berhasil, `velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity)`.
7. Terapkan gravitasi, lalu `CharacterController.Move`.

**`public void SetSpeedModifier(SpeedModifierId id, float multiplier)`** menyimpan nilai ke `speedModifiers[id]`, lalu menghitung ulang `SpeedMultiplier`.

**`public void ClearSpeedModifier(SpeedModifierId id)`** mengembalikan nilai ke 1.

**`public void Teleport(Vector3 position)`:** matikan `CharacterController`, pindahkan posisi, nyalakan lagi, nolkan `velocity`. Pemanggil seperti `ProgressionSafety` (#116) lalu memanggil `PlayerTraversal.ResetFallTracking()`.

**Membawa Heavy Item:** `HeavyItemSystem` memanggil `SetSpeedModifier(SpeedModifierId.HeavyItem, ...)`. Sprint dan Jump membaca `HeavyItemSystem.IsCarrying`.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim dan didengar:** tidak ada. Status dibaca lewat properti.

| Sistem | Hubungan |
|---|---|
| `InputManager` (#18) | `MoveInput`, `SprintHeld`, dan `JumpPressed` |
| `PlayerTraversal` (#101) | `AllowsGroundMove`, pengali `Crouch` dan `Surface`, dan `IsGrounded` |
| `SurvivalStats` (#111) | `SpendStamina`. Regenerasi milik `SurvivalStats` |
| `HeavyItemSystem` (#69) | `IsCarrying` dan pengali `HeavyItem` |
| `FirstPersonCamera` (#102) | Mouse look dan kamera, serta membaca `IsSprinting` |
| `NoiseEmitter`, `AISensoryHub` (#49) | Membaca `IsSprinting` |
| `RegionalWeather` (#115) | Pengali `Weather` |
| `SwimmingController` (#13) | Pengali `Water` |
| `ItemUseSystem` (#66), `StatusEffectManager` (#20) | Pengali `ItemUse` dan `StatusEffect` |
| `ProgressionSafety` (#116), `DevConsole` (#56) | `Teleport` |

## 4. Kasus Khusus

- **Pool dipakai ulang:** pemain tidak memakai pool.
- **Game dijeda:** `Time.deltaTime` 0, jadi gerak dan biaya stamina berhenti. Input gameplay terkunci oleh `UIFocusStack`.
- **Pengali bertumpuk:** nilai per id tidak berkurang berulang. Contoh: barang 0,5 x badai 0,75 = 0,375.
- **Stamina habis saat lari:** `SpendStamina` menolak, dan pemain kembali berjalan.
- **Mode `Locked`:** `AllowsGroundMove` bernilai `false`, jadi pemain diam saat diterkam (#110).
- **Save dan load:** posisi pemain tersimpan oleh `WorldStateSave`. Pengali dipasang ulang oleh pemiliknya, jadi tidak disimpan.
- **Permadeath:** pemain dibuat ulang bersama scene `IslandPhase`.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** pengali kecepatan dan `jumpHeight` 1,1 (`PlayerTraversal.md`), biaya stamina (`VitalsRules.md`), kecepatan setengah saat membawa barang (`Claude.md` Tahap 4, #69), serta `FirstPersonCamera` (#102).

**Usulan yang disetujui developer sebagai nilai awal (#85):**

1. **`walkSpeed` 5 dan `sprintSpeed` 8:** nilai lama. Rekomendasi: pindahkan ke `GameBalanceConfig.traversal` saat script ditulis (#84).

## 6. Skenario Tes

1. Jalan, lari, dan lompat. **Hasil:** 5 m/s, 8 m/s, dan tinggi lompat 1,1 m. Stamina berkurang 10 per detik dan 8 per lompat.
2. Ganti tombol lari di menu. **Hasil:** tombol baru langsung berlaku.
3. Angkat suku cadang. **Hasil:** kecepatan 2,5 m/s, dan Sprint serta Jump mati.
4. Angkat suku cadang saat `Blizzard`. **Hasil:** pengali bertumpuk, tanpa berkurang berulang.
5. Lepaskan barang. **Hasil:** pengali `HeavyItem` kembali 1.
6. Berdiri diam. **Hasil:** stamina pulih lewat `SurvivalStats`, dan skrip ini tidak menulis stamina.
7. Panjat tangga. **Hasil:** skrip ini tidak menggerakkan pemain.
8. Panggil `Teleport`. **Hasil:** pemain pindah tanpa damage jatuh.
9. Baca `IsSprinting` saat lari dan jalan. **Hasil:** `true`, lalu `false`.
