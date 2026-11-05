# Blueprint: PlayerPinnedState.cs
**Lokasi:** `docs/blueprint/Island_Logic/PlayerPinnedState.md`
**Deskripsi Utama:** Keadaan pemain tertahan musuh dan cara melepaskan diri sendirian. Sistem ini mengatur kondisi pemain, sedangkan penyerang dan damage-nya diatur #25 dan #61.
**Issue:** #110
**Rujukan GDD:** Bagian 3 (Diterkam: Jaguar dan Zombie Tribal bisa menahan pemain, dan pemain harus melepaskan diri sendiri, misalnya dengan Pisau) dan bagian 5 (Jaguar menyergap dari pohon). Angka dan cara lepas adalah usulan. Tidak ada bantuan NPC, sesuai keputusan developer. Polanya berasal dari `docs/REFERENSI_MUSUH_L4D.md` bagian 8.

## 1. Arsitektur & Variabel Utama

`PlayerPinnedState` adalah komponen di objek pemain pada scene `IslandPhase`, dengan satu instance. Fase 1 tidak memakainya.

```csharp
public enum PinReleaseReason { Escaped, EscapedWithKnife, AttackerDamaged, AttackerGone, TimedOut, PlayerDied }

public interface IPinAttacker
{
    string AttackerId { get; }        // EnemyData.enemyId
    Transform LookTarget { get; }     // Titik kepala penyerang
    bool IsPinValid { get; }          // false kalau mati, terbakar, atau kembali ke pool
    void OnPinReleased(PinReleaseReason reason);   // Penyerang masuk Stagger atau jeda sendiri
    IDamageable DamageTarget { get; } // Target tusukan Pisau
}
```

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `IsPinned` | `bool` | `false` | Pemain sedang tertahan |
| `Attacker` | `IPinAttacker` | `null` | Satu penyerang pada satu waktu |
| `EscapeProgress` | `float` | 0 | Kemajuan lepas, 0 sampai 1. Dibaca HUD |
| `pinTimer` | `float` | 0 | Lama tertahan |
| `knifeCooldownTimer` | `float` | 0 | Jeda tusukan Pisau |
| `graceUntil` | `float` | 0 | Akhir kebal damage musuh setelah lepas |
| `repinImmuneUntil` | `float` | 0 | Akhir kebal terhadap penahanan baru |

| Angka | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.pinned.enabled` | `true` | Aturan GDD bagian 3. Saklar hanya untuk tes. Saat `false`, `TryPin` selalu menolak |
| `GameBalanceConfig.pinned.mashProgressPerPress` | 0,1 | Kemajuan tiap tekanan `Struggle` |
| `GameBalanceConfig.pinned.mashDecayPerSec` | 0,05 | Kemajuan turun saat berhenti menekan |
| `GameBalanceConfig.pinned.mashMinPressInterval` | 0,08 | Jeda minimum antar-tekanan, untuk membatasi makro |
| `GameBalanceConfig.pinned.holdProgressPerSec` | 0,35 | Kemajuan saat menahan tombol |
| `GameBalanceConfig.pinned.struggleStaminaPerPress` | 3 | Biaya stamina tiap tekanan |
| `GameBalanceConfig.pinned.struggleStaminaHoldPerSec` | 10 | Biaya stamina saat menahan |
| `GameBalanceConfig.pinned.staminaEmptyMultiplier` | 0,5 | Pengali saat stamina 0 |
| `GameBalanceConfig.pinned.hpEmptyMultiplier` | 0,75 | Pengali saat HP mendekati 0 |
| `GameBalanceConfig.pinned.infectionFullMultiplier` | 0,75 | Pengali saat infeksi penuh |
| `GameBalanceConfig.pinned.minEscapeMultiplier` | 0,35 | Batas bawah pengali gabungan |
| `GameBalanceConfig.pinned.knifeProgress` | 0,6 | Kemajuan tiap tusukan Pisau |
| `GameBalanceConfig.pinned.knifeDamage` | 25 | Damage tusukan, sama dengan `MeleeCombatSystem` lama |
| `GameBalanceConfig.pinned.knifeCooldownSeconds` | 1,5 | Jeda antar-tusukan |
| `GameBalanceConfig.pinned.knifeDurabilityCost` | 5 | Durabilitas Pisau tiap tusukan |
| `GameBalanceConfig.pinned.maxPinSeconds` | 8 | Penyerang melepas paksa setelah waktu ini |
| `GameBalanceConfig.pinned.maxOtherAttackers` | 1 | Musuh lain yang boleh memukul saat pemain tertahan |
| `GameBalanceConfig.pinned.otherAttackerDamageMultiplier` | 0,5 | Pengali damage musuh lain saat pemain tertahan |
| `GameBalanceConfig.pinned.postEscapeGraceSeconds` | 1,5 | Kebal damage musuh setelah lepas |
| `GameBalanceConfig.pinned.repinImmuneSeconds` | 4 | Kebal penahanan baru setelah lepas |
| `GameBalanceConfig.pinned.releasePushDistance` | 1 | Jarak penyerang terdorong saat lepas |

## 2. Detail Logika & Fungsi

### Masuk ke status Pinned

**`public bool TryPin(IPinAttacker attacker)`** dipanggil `SpecialZombieAI` (Zombie Tribal) atau `WildlifeAI` (Jaguar), setelah serangannya mengenai pemain.

1. Tolak kalau `pinned.enabled` bernilai `false`, `IsPinned`, `Time.time < repinImmuneUntil`, pemain mati, panel `Cinematic` aktif, atau mode `PlayerTraversal` adalah `Swimming`.
2. Simpan `Attacker`, lalu nolkan `EscapeProgress`, `pinTimer`, dan `knifeCooldownTimer`.
3. Panggil `PlayerTraversal.ForceDetach()` kalau memanjat, lalu `PlayerTraversal.SetLocked(true)`.
4. Panggil `PlayerInteractor.SetEnabled(false)`, dan `FirstPersonCamera.SetForcedLook(attacker.LookTarget)`.
5. Terbitkan `PlayerPinnedEvent { attackerId, isPinned = true }`. `EquipmentManager` memasang `EquipLock.Pinned`, `UIFocusStack` menutup panel, dan `HeavyItemSystem` menjatuhkan barang.
6. `CameraShake.Play(CameraShakeKind.Pinned)` (#24), lalu kembalikan `true`.

Kalau `false`, penyerang menjalankan jeda sendiri tanpa menahan pemain.

### Update saat tertahan

Semua memakai `Time.deltaTime`, sehingga berhenti saat game dijeda.

1. Kalau `Attacker.IsPinValid == false`, panggil `Release(AttackerGone)`.
2. Tambah `pinTimer`. Kalau lewat `maxPinSeconds`, panggil `Release(TimedOut)`.
3. Hitung `mult = EscapeMultiplier()`.
4. Baca input cara lepas sesuai pilihan pengaturan.
5. Kalau `EscapeProgress >= 1`, panggil `Release(Escaped)`. Kemajuan hanya berasal dari pemain sendiri.

### Cara lepas

| Cara | Input | Logika |
|---|---|---|
| Menekan berulang (bawaan) | `Struggle` (`Space`) | Tekanan yang lebih cepat dari `mashMinPressInterval` diabaikan. Tiap tekanan sah menambah `mashProgressPerPress * mult`, lalu `SpendStamina(struggleStaminaPerPress, StaminaUse.Struggle)`. Tanpa tekanan, kemajuan turun `mashDecayPerSec` |
| Menahan tombol (aksesibilitas) | Tahan `Struggle` | Pilihan `holdToStruggle` di #19. Menambah `holdProgressPerSec * mult * deltaTime`, dengan biaya `struggleStaminaHoldPerSec`. Kemajuan turun kalau tombol dilepas |
| Pisau | `PrimaryPressed` | Berlaku tanpa memegang Pisau di tangan, selama `Base_Pisau` tidak `Broken`. Lihat di bawah |

Stamina yang habis tidak menghentikan cara lepas. `SpendStamina` boleh mengembalikan `false`, dan kemajuan tetap dihitung memakai `staminaEmptyMultiplier`.

**Tusukan Pisau:**

1. Tolak kalau `knifeCooldownTimer > 0` atau `DurabilitySystem.GetState` Pisau adalah `Broken`.
2. Mainkan animasi tusuk, lalu kirim `DamageInfo { amount = knifeDamage, type = Melee, sourceId = "Base_Pisau", byPlayer = true }` ke `Attacker.DamageTarget`.
3. Tambah `EscapeProgress` sebesar `knifeProgress * mult`.
4. Panggil `DurabilitySystem.ApplyWear(pisau, knifeDurabilityCost)`, sesuai aturan item inti di `ToolRepairSystem.md`.
5. Isi `knifeCooldownTimer`. Kalau kemajuan mencapai 1, panggil `Release(EscapedWithKnife)`.

### Pengaruh status pada kesulitan

```text
s = Stamina / MaxStamina, h = HP / MaxHP, i = Infection / MaxInfection
mult = Lerp(staminaEmptyMultiplier, 1, s) * Lerp(hpEmptyMultiplier, 1, h) * Lerp(1, infectionFullMultiplier, i)
mult = Max(mult, minEscapeMultiplier)
```

Dengan 6 tekanan per detik:

| Kondisi | `mult` | Kemajuan bersih per detik | Waktu lepas |
|---|---|---|---|
| Semua status baik | 1 | 0,55 | Sekitar 1,8 detik |
| Stamina 0 saja | 0,5 | 0,25 | Sekitar 4 detik |
| Stamina 0, HP rendah, infeksi penuh | 0,35 | 0,16 | Sekitar 6,3 detik, masih di bawah `maxPinSeconds` |

### Lepas

**`public void Release(PinReleaseReason reason)`**:

1. Kalau `!IsPinned`, selesai.
2. Simpan `Attacker` ke variabel lokal, lalu kosongkan `Attacker` dan `IsPinned`.
3. Panggil `PlayerTraversal.SetLocked(false)`, `PlayerInteractor.SetEnabled(true)`, dan `FirstPersonCamera.SetForcedLook(null)`.
4. Kalau `reason != PlayerDied`, isi `graceUntil` dan `repinImmuneUntil`, lalu panggil `attacker.OnPinReleased(reason)`. Penyerang terdorong `releasePushDistance` lewat #95.
5. Terbitkan `PlayerPinnedEvent { attackerId, isPinned = false }`.

**`public void ReleaseByAttacker(IPinAttacker attacker, PinReleaseReason reason)`** dipanggil penyerang yang mati, kembali ke pool, atau menerima damage melebihi batasnya. Fungsi ini mengabaikan panggilan dari penyerang selain `Attacker`.

### Musuh lain selama tertahan

- `TryPin` memanggil `EnemyAttack.SetAttackerLimit(playerTargetable, maxOtherAttackers)` (#109). `Release` mengembalikannya dengan nilai -1.
- Damage musuh lain dikali `otherAttackerDamageMultiplier` di `SurvivalStats.TakeDamage`. Infeksinya tidak diubah.
- Damage lingkungan, lapar, haus, frostbite, dan ledakan tidak dikurangi, karena bukan serangan musuh.
- Selama `graceUntil`, `SurvivalStats.TakeDamage` membuang damage dengan `info.source` musuh. Sumber lain tetap berlaku.

### Efek dan HUD

- **Kamera:** `SetForcedLook` mengarah ke penyerang, dan getaran `Pinned` berasal dari #24. Getaran kecil tiap damage mengikuti `PlayerDamagedEvent`.
- **Penunjuk tombol:** `PinnedPromptUI` mendengar `PlayerPinnedEvent`, lalu menampilkan nama tombol dari `InputManager`. Isinya `Struggle` dan ikon Pisau kalau tersedia, dengan cincin kemajuan yang membaca `EscapeProgress`.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `PlayerPinnedEvent`.

**Event yang didengar:** `PlayerDiedEvent`, untuk memanggil `Release(PlayerDied)`.

| Sistem | Hubungan |
|---|---|
| `SpecialZombieAI` (#25), `WildlifeAI` (#61) | Memanggil `TryPin` dan `ReleaseByAttacker`, serta mengisi damage selama menahan |
| `SurvivalStats`, `VitalsRules` (#111) | Stamina, HP, dan infeksi. Tambahan `StaminaUse.Struggle` dan pemeriksaan damage |
| `EquipmentManager` (#60) | `EquipLock.Pinned` mengunci semua input item |
| `InputManager` (#18) | `Struggle` dan `PrimaryPressed` |
| `PlayerTraversal` (#101), `PlayerInteractor` (#17) | Mengunci gerakan dan interaksi |
| `FirstPersonCamera` (#102), `CameraShake` (#24) | Arah pandang dan getaran |
| `NPCCompanion`, `NPCRoster` (#76) | NPC tidak menolong, dan tidak menyerang `Attacker` selama pemain tertahan. Lihat bagian 5 |
| `HeavyItemSystem` (#69) | Menjatuhkan barang saat `isPinned = true` |
| `EnemyAttack` (#109) | `SetAttackerLimit` membatasi penyerang lain |
| `WorldStateSave` (#80) | Menunda autosave saat tertahan |

## 4. Kasus Khusus

- **Objek pool:** penyerang memanggil `ReleaseByAttacker(this, AttackerGone)` di `OnReturned`. `IsPinValid` menjadi pengaman kalau langkah itu terlewat. Sistem ini sendiri tidak di-pool.
- **Game dijeda:** timer dan fungsi `Update` berhenti. Tekanan saat jeda tidak dihitung karena input gameplay terkunci.
- **Permadeath dan save:** tidak ada data yang disimpan. Autosave ditunda saat tertahan, jadi save tidak pernah dimuat dalam keadaan tertahan. Saat mati, `Release(PlayerDied)` berjalan sebelum #53 mengambil kamera.
- **Fajar saat ditahan Zombie Tribal:** zombie masuk `BurnToAshes` (#63), `IsPinValid` menjadi `false`, dan pemain lepas.
- **Membawa Heavy Item:** barang jatuh di dekat kaki pemain. Bit `HeavyItem` dilepas lewat `HeavyItemChangedEvent`, sehingga hanya `Pinned` yang tersisa dan Pisau tetap bisa dipakai.
- **Pisau `Broken`:** ikon Pisau berwarna redup, dan pemain tetap bisa memakai `Struggle`.
- **Sedang tidur:** panel `SleepPanel` ditutup #83, sesuai `SleepSystem.md`.
- **Penahanan kedua saat tertahan:** `TryPin` menolak. Musuh kedua menjadi penyerang biasa dengan batas `maxOtherAttackers`.
- **Region dilepas saat tertahan:** tidak boleh terjadi karena pemain terkunci di tempat. Kalau penyerang hilang, `IsPinValid` melepas pemain.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** `PlayerPinnedEvent` (`GameEventBus.md`), tombol `Struggle` (`InputManager.md`), `EquipLock.Pinned` (`EquipmentManager.md`), dan `SetForcedLook` (`FirstPersonCamera.md`).

**Usulan yang disetujui developer sebagai nilai awal (dicatat di #85):**

1. **Sifat sistem:** kini aturan GDD bagian 3, untuk Jaguar dan Zombie Tribal. `pinned.enabled = true`.
2. **Cara lepas:** menekan berulang menjadi bawaan, menahan tombol menjadi pilihan aksesibilitas, dan Pisau menjadi cara cepat. Rekomendasi: pakai ketiganya.
3. **Pengaruh status:** stamina paling kuat, HP dan infeksi lebih ringan, dengan batas bawah 0,35. Rekomendasi: pakai, supaya pemain yang lemah masih punya peluang.
4. **Peran NPC:** diputuskan developer, NPC tidak menolong. Pemain hanya bisa melepaskan diri sendiri, sesuai GDD bagian 3. NPC juga tidak menyerang `Attacker` selama pemain tertahan (`NPCRoster.md`).
5. **Musuh lain:** paling banyak 1 penyerang lain dengan damage 50 persen, serta batas tertahan 8 detik. Rekomendasi: pakai.
6. **Durabilitas Pisau:** berkurang 5 tiap tusukan. Pisau rusak tidak bisa dipakai, sampai diperbaiki lewat `ToolRepairSystem`. Rekomendasi: pakai.
7. **Heavy Item:** barang langsung jatuh. Rekomendasi: pakai, karena dua tangan terpakai dan pemain butuh tangan untuk melawan.
8. **Waktu kebal:** 1,5 detik dari damage musuh, dan 4 detik dari penahanan baru. Rekomendasi: pakai.

## 6. Skenario Tes

1. Atur `pinned.enabled = false` lewat `DevConsole`, lalu panggil `TryPin`. **Hasil:** ditolak, dan penyerang menjalankan jedanya. Lalu kembalikan `pinned.enabled = true`, dan ulangi dengan zombie uji. **Hasil:** gerakan dan senjata terkunci, kamera menghadap penyerang, dan penunjuk tombol muncul.
2. Tekan `Space` 6 kali per detik dengan status penuh. **Hasil:** lepas dalam sekitar 2 detik, penyerang terdorong, dan kontrol kembali.
3. Atur stamina 0, HP 10, dan infeksi 100. **Hasil:** lepas dalam sekitar 6,3 detik, dan tidak lewat 8 detik.
4. Berhenti menekan selama 8 detik. **Hasil:** `Release(TimedOut)` berjalan, dan pemain kebal penahanan selama 4 detik.
5. Tekan klik kiri dua kali selama tertahan, dengan senjata aktif Pistol. **Hasil:** tusukan pertama dan kedua terpisah 1,5 detik, tanpa peluru keluar, dan durabilitas Pisau turun 10.
6. Atur Pisau `Broken`, lalu tekan klik kiri. **Hasil:** tusukan ditolak, dan `Space` tetap berfungsi.
7. Tertahan sambil membawa Heavy Item. **Hasil:** barang jatuh, `HeavyItemChangedEvent.isCarried = false`, dan barang bisa diambil lagi setelah lepas.
8. Tertahan bersama 5 zombie di sekitar. **Hasil:** hanya 1 zombie lain yang memukul, dengan damage 50 persen.
9. Tertahan dengan NPC `Follow` di dekat pemain. **Hasil:** NPC tidak menyerang penyerang, dan kemajuan tidak naik tanpa tekanan tombol.
10. Bunuh penyerang lewat `DevConsole` saat menahan. **Hasil:** pemain langsung lepas. Ulangi dengan mengirim penyerang kembali ke pool, dan pastikan hasilnya sama.
11. Jeda game saat tertahan selama 10 detik. **Hasil:** `pinTimer` dan `EscapeProgress` tidak berubah.
12. Mati saat tertahan. **Hasil:** `PlayerDiedEvent` satu kali, `Attacker` kosong, dan run berikutnya mulai tanpa kunci.
