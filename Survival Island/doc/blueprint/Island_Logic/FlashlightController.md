# Blueprint: FlashlightController.cs
**Lokasi:** `Survival Island/doc/blueprint/Island_Logic/FlashlightController.md`
**Deskripsi Utama:** Mengontrol senter di tangan kiri, konsumsi baterai, pengisian daya, dan sorotannya.
**Issue:** #98 (melengkapi #18 dan #60)
**Rujukan GDD:** Usulan, belum ada di GDD. Mendukung Region 2 yang gelap di bagian 5. GDD bagian 5 hanya menyebut obor untuk Region 3, jadi senter tidak memberi panas.

## 1. Arsitektur & Variabel Utama

`FlashlightController : EquippableBehaviour`, dengan `HandUse.OffHand`, pada viewmodel `Tool_Flashlight`. Isi baterai tiap senter berada di `ItemInstance.durability`, jadi tersimpan bersama inventori. `ItemData.maxDurability` untuk `Tool_Flashlight` bernilai sama dengan `flashlight.maxBattery` (100).

**Batas tanggung jawab:**

| Pekerjaan | Pemilik |
|---|---|
| Tombol `F`, slot tangan kiri, dan kapan senter disimpan | `EquipmentManager` (#60) |
| Lampu menyala, kedip, mati, kuras baterai, dan sorotan | `FlashlightController` |
| Aksi `Pakai` pada `Item_Battery` dan memilih senter yang diisi | `ItemUseSystem` (#66), sesuai `LightSourceSystem.md` |
| Keterlihatan pemain, batas lampu real-time, dan bayangan | `LightSourceSystem` (#98) |
| Fog dan ambient gelap Region 2 | `EnvironmentDirector` (#92). Senter tidak menulis fog |
| Panas dan perlindungan frostbite | `WarmthSystem` (#65). Senter tidak dihitung |

### Variabel

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `flashlight` | `Light` | Dari prefab, tipe `Spot` | Lampu sorot |
| `lightSource` | `LightSource` | Dari prefab, `isHeld = true` | Pendaftaran ke `LightSourceSystem` (#98) |
| `beamOrigin` | `Transform` | Transform `flashlight` | Titik dan arah sorotan |
| `baseIntensity` | `float` | `flashlight.intensity` prefab | Dipakai saat kedip |
| `instance` | `ItemInstance` | `null` | Item yang sedang dipegang |
| `isOn` | `bool` | `false` | Status lampu |
| `static Active` | `FlashlightController` | `null` | Senter yang sedang dipegang. Dibaca `Recharge` dan `IsPointInBeam` |

### Angka penyetelan

| Angka | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.flashlight.maxBattery` | 100 | Isi penuh |
| `GameBalanceConfig.flashlight.drainPerSec` | 0,2 | Senter menyala 500 detik, sekitar satu malam |
| `GameBalanceConfig.flashlight.batteryRecharge` | 100 | Isi per `Item_Battery`, dipakai `ItemUseSystem` |
| `GameBalanceConfig.light.flickerSeconds` | 2 | Sudah ada (#98). Kedip sebelum baterai habis |
| `GameBalanceConfig.flashlight.range` | 25 | Jangkauan sorot dalam meter (usulan baru, disetujui developer) |
| `GameBalanceConfig.flashlight.spotAngle` | 50 | Sudut kerucut sorot (usulan baru, disetujui developer) |
| `GameBalanceConfig.flashlight.flickerHz` | 6 | Kecepatan kedip (usulan baru, disetujui developer) |

Nilai `range` dan `spotAngle` ditulis ke `flashlight` di `OnEquip`, sehingga lampu yang terlihat dan `IsPointInBeam` selalu memakai angka yang sama.

**Dihapus:** `currentBattery`, `maxBattery`, `drainRate`, pembacaan tombol `F`, dan pembacaan `flashlight.drainPerSec` dari komponen `Light`. Angka kuras selalu dari `GameBalanceConfig`.

## 2. Detail Logika & Fungsi

### Dipegang dan disimpan

**Tombol `F`:** `InputManager.Instance.FlashlightPressed` dibaca `EquipmentManager.ToggleOffHand` (#60), bukan komponen ini. Senter yang habis tetap bisa dipegang. Menekan `F` lagi menyimpannya.

**`OnEquip(ItemInstance inst)`**:

1. `instance = inst`, dan `Active = this`.
2. Terapkan `flashlight.range` dan `flashlight.spotAngle` dari config.
3. Kalau `inst.durability > 0`, panggil `SetLit(true)`. Kalau tidak, `SetLit(false)` dan tampilkan notifikasi `Baterai habis` (#22).

**`OnUnequip()`**: `SetLit(false)`, `instance = null`, dan kalau `Active == this`, `Active = null`. Baterai berhenti berkurang.

**`private void SetLit(bool on)`**: `isOn = on`, `flashlight.enabled = on`, `flashlight.intensity = baseIntensity`, `lightSource.IsLit = on`, dan `lightSource.intensity01 = on ? 1 : 0`. `LightSourceSystem` membaca perubahan ini pada tick berikutnya, termasuk tambahan `flashlightVisibilityBonus`.

### Kuras baterai

**`void Update()`**, hanya kalau `isOn`:

1. `instance.durability -= drainPerSec * Time.deltaTime`.
2. `remainingSeconds = instance.durability / drainPerSec`.
3. Kalau `remainingSeconds < flickerSeconds`, kedipkan: `flashlight.intensity = baseIntensity * (PingPong(Time.time * flickerHz, 1) > 0,5 ? 1 : 0,2)`. Tanpa `Random`, jadi kedipnya sama di tiap uji.
4. Kalau `instance.durability <= 0`: `durability = 0`, `SetLit(false)`, lalu tampilkan notifikasi `Baterai habis`. Senter tetap di tangan.

`Update` tidak berjalan saat lampu mati, karena langkah pertamanya keluar lebih dulu.

### Isi ulang

**`public static bool Recharge(ItemInstance target, float amount)`**:

1. Kalau `target == null`, kembalikan `false`.
2. Kalau `target.durability >= maxBattery`, kembalikan `false`. Baterai tidak terbuang.
3. `target.durability = Min(target.durability + amount, maxBattery)`.
4. Kalau `Active != null` dan `Active.instance == target` dan `!Active.isOn`, panggil `Active.SetLit(true)`. Senter yang sedang dipegang langsung menyala lagi (usulan).
5. Kembalikan `true`.

Fungsi ini statis, karena senter yang diisi bisa sedang tersimpan di inventori. Komponen viewmodel hanya hidup saat senter dipegang. Pemanggilnya `ItemUseSystem`, yang memilih senter dipegang lebih dulu, lalu senter dengan isi terendah, dan baru mengurangi `Item_Battery` kalau hasilnya `true`.

### Sorotan

**`public static bool IsPointInBeam(Vector3 pos)`**:

1. Kalau `Active == null` atau `!Active.isOn`, kembalikan `false`.
2. `d = pos - beamOrigin.position`. Kalau `d.sqrMagnitude > range * range`, kembalikan `false`.
3. Kembalikan `Vector3.Angle(beamOrigin.forward, d) <= spotAngle / 2`.

Tanpa query fisika. Garis pandang diperiksa pemanggil, misalnya Zombie Lumut (#25) dengan batas `specialZombie.mossLightDistance` 12. Fungsi ini statis supaya pemanggil tidak perlu mencari komponen.

**Keterlihatan:** penulisan `SetVisibilityModifier` milik `LightSourceSystem`. Komponen ini hanya memasang `LightSource`.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim dan didengar:** tidak ada. Notifikasi dikirim lewat `NotificationManager` (#22).

| Sistem | Hubungan |
|---|---|
| `EquipmentManager` (#60) | `ToggleOffHand`, `OnEquip`, `OnUnequip`, dan aturan tangan kiri |
| `InputManager` (#18) | `FlashlightPressed`, dibaca `EquipmentManager` |
| `ItemUseSystem` (#66) | Memanggil `Recharge(target, batteryRecharge)` saat `Item_Battery` dipakai |
| `LightSourceSystem` (#98) | Membaca `LightSource` untuk keterlihatan dan batas lampu |
| `SpecialZombieAI` (#25) | Zombie Lumut membaca `IsPointInBeam` |
| `InventoryManager` (#66) | `ItemInstance.durability` |
| `NotificationManager` (#22) | Pesan `Baterai habis` |
| `ToolRepairSystem` (#113) | Senter tidak bisa diperbaiki |
| `RegionLootTable` (#103) | Sumber `Tool_Flashlight` dan `Item_Battery` |

## 4. Kasus Khusus

- **Pool dipakai ulang:** viewmodel memakai cache `EquipmentManager`, bukan pool. `OnEquip` mengisi ulang status, dan `OnUnequip` mengosongkan `Active`.
- **Game dijeda:** `Time.deltaTime` 0, jadi baterai dan kedip berhenti.
- **Tidur (#67):** baterai dihitung dengan `Time.deltaTime`, jadi tidur tidak mempercepat kuras. Usulan: senter tidak perlu dimatikan otomatis, karena waktu nyata saat tidur pendek.
- **Kapak atau Kail dipegang:** senter disimpan otomatis oleh `EquipmentManager`, sehingga `OnUnequip` mematikan lampu dan baterai berhenti.
- **Membawa Heavy Item, memanjat, atau berenang:** mengikuti kunci `EquipmentManager`. Kalau `OnUnequip` dipanggil, lampu mati. Senter tidak padam karena air sendiri, berbeda dari obor (usulan).
- **Pemain diterkam (#110):** kunci `Pinned` menahan input item, tetapi senter tetap menyala dan tetap menguras baterai.
- **`Storm` dan `Blizzard`:** senter tidak padam. Aturan padam saat badai di `LightSourceSystem` hanya untuk lampu tak beratap.
- **Baterai habis:** `OnEquip` tidak menyalakan lampu sampai `Recharge` dipanggil.
- **Dua senter di inventori:** hanya yang dipegang yang menguras. `Recharge` memilih lewat `ItemUseSystem`.
- **Senter dibuang saat menyala:** `EquipmentManager` memanggil `OnUnequip` lebih dulu, karena item keluar dari slot.
- **Save dan load:** isi baterai mengikuti `ItemInstance`. Status tangan kiri mengikuti `EquipmentSaveData.offHandActive`, lalu `OnEquip` menyalakan lampu kalau masih terisi.
- **Permadeath:** senter dan isinya hilang bersama inventori run.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** angka baterai dan `Item_Battery` (#98, `LightSourceSystem.md`), slot tangan kiri (#60), dan obor tetap menyala saat `Storm` serta `Blizzard` (keputusan developer, untuk obor, bukan senter).

**Usulan yang disetujui developer sebagai nilai awal (#85):**

1. **Senter dan baterai:** GDD tidak menyebutnya. Rekomendasi: pakai, supaya Region 2 tetap gelap tanpa membuat pemain buta.

**Usulan baru (disetujui developer sebagai nilai awal):**

2. **`Recharge` statis dengan parameter `ItemInstance`:** supaya senter di inventori bisa diisi. Pemanggil di `ItemUseSystem.md` dan `LightSourceSystem.md` perlu menyebut senter targetnya.
3. **Senter yang dipegang menyala otomatis setelah diisi.**
4. **`range` 25, `spotAngle` 50, dan `flickerHz` 6 masuk `GameBalanceConfig.flashlight`.**
5. **Senter tidak padam di air, hujan, atau badai.**
6. **Senter tidak memberi panas di Region 3.** GDD hanya menyebut pakaian hangat dan obor.

## 6. Skenario Tes

1. Tekan `F` dengan senter terisi. **Hasil:** lampu menyala, dan `LightSource` terdaftar dengan `isHeld = true`.
2. Biarkan menyala 500 detik. **Hasil:** lampu berkedip selama 2 detik terakhir, lalu mati, dan senter tetap di tangan.
3. Pakai `Item_Battery` saat senter habis di tangan. **Hasil:** isi menjadi 100, dan lampu langsung menyala.
4. Pakai `Item_Battery` saat senter penuh. **Hasil:** ditolak, dan jumlah baterai tidak berkurang.
5. Simpan senter berisi 10 di inventori, pegang Pistol, lalu pakai `Item_Battery`. **Hasil:** senter di inventori menjadi 100 tanpa harus dipegang.
6. Ganti ke Kapak saat senter menyala. **Hasil:** senter disimpan, dan baterai berhenti berkurang.
7. Arahkan sorotan ke Zombie Lumut. **Hasil:** `IsPointInBeam` bernilai `true`. Matikan senter. **Hasil:** `false`.
8. Arahkan sorotan ke titik 30 meter di depan, lalu ke titik 5 meter di samping (90 derajat). **Hasil:** keduanya `false`.
9. Cari `currentBattery`, pembacaan tombol, dan `Random` di skrip. **Hasil:** tidak ada.
10. Simpan dengan isi 40, lalu muat. **Hasil:** isi tetap 40, dan lampu menyala kalau senter dipegang saat disimpan.
11. Jeda game 60 detik dengan senter menyala. **Hasil:** isi tidak berubah.
12. Berdiri dalam `Storm`, lalu `Blizzard`, dengan senter menyala. **Hasil:** senter tetap menyala.
13. Diterkam Jaguar dengan senter menyala. **Hasil:** lampu tetap menyala dan baterai tetap berkurang.
14. Ubah `flashlight.drainPerSec` ke 1 lewat `DevConsole`. **Hasil:** isi berkurang 1 per detik.
15. Bandingkan nilai keterlihatan di malam Region 2 dengan senter mati dan menyala. **Hasil:** nilai naik sesuai rumus `LightSourceSystem.md`, yang memakai `flashlightVisibilityBonus` (0,3).

## 7. Catatan Implementasi (Sistem pendukung)

- Kode: `Island_Logic/FlashlightController.cs`. Config `GameBalanceConfig.flashlight` di `GameBalance.GrupE.cs`.
- Tambahan static `IsActiveLit` dan `ActiveInstance`, dibaca `LightSourceSystem` (bonus keterlihatan dan pilihan senter untuk baterai).
- `Awake` menandai `LightSource` prefab sebagai `isHeld = true` dan `isFlashlight = true` (field baru di `LightSource`, supaya `GetLightLevelAt` dan bayangan tidak menghitung senter).
- Belum diuji di Unity.
