# Blueprint: FlashlightController.cs
**Lokasi:** `docs/blueprint/Island_Logic/FlashlightController.md`
**Deskripsi Utama:** Mengontrol senter di tangan kiri, konsumsi baterai, pengisian daya, dan sorotannya.
**Issue:** #98 (melengkapi #18 dan #60)
**Rujukan GDD:** Usulan, belum ada di GDD. Mendukung Region 2 yang gelap di bagian 5.

## 1. Arsitektur & Variabel Utama

`FlashlightController : EquippableBehaviour`, dengan `HandUse.OffHand`, pada viewmodel `Tool_Flashlight`. Isi baterai tiap senter berada di `ItemInstance.durability`, jadi tersimpan bersama inventori.

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `flashlight` | `Light` | Dari prefab | Lampu sorot |
| `lightSource` | `LightSource` | Dari prefab, `isHeld = true` | Pendaftaran ke `LightSourceSystem` (#98) |
| `instance` | `ItemInstance` | `null` | Item yang sedang dipegang |
| `isOn` | `bool` | `false` | Status lampu |

| Angka | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.flashlight.maxBattery` | 100 | Isi penuh |
| `GameBalanceConfig.flashlight.drainPerSec` | 0,2 | Senter menyala 500 detik, sekitar satu malam |
| `GameBalanceConfig.flashlight.batteryRecharge` | 100 | Isi per `Item_Battery`, dipakai `LightSourceSystem` |

**Dihapus:** `currentBattery`, `maxBattery`, `drainRate`, dan pembacaan tombol `F`.

## 2. Detail Logika & Fungsi

**Tombol `F`:** `InputManager.Instance.FlashlightPressed` dibaca `EquipmentManager.ToggleOffHand` (#60), bukan komponen ini.

**`OnEquip(ItemInstance inst)`:** simpan `instance = inst`. Kalau `inst.durability > 0`, `isOn = true`, nyalakan `flashlight`, dan `lightSource.IsLit = true`.

**`OnUnequip()`:** `isOn = false`, matikan `flashlight`, dan `lightSource.IsLit = false`.

**`void Update()`**, kalau `isOn`:

1. `instance.durability -= flashlight.drainPerSec * Time.deltaTime`.
2. Kalau habis, set `durability = 0`, kedipkan cahaya, lalu matikan lampu. Senter tetap di tangan.

**`public void Recharge(float amount)`:** `durability = Min(durability + amount, maxBattery)`, dipanggil `LightSourceSystem` saat `Item_Battery` dipakai.

**`public bool IsPointInBeam(Vector3 pos)`:** `true` kalau `isOn`, jarak ke `pos` di bawah `flashlight.range`, dan sudut ke arah kamera di bawah `flashlight.spotAngle / 2`. Tanpa query fisika. Garis pandang diperiksa pemanggil.

**Keterlihatan:** penulisan `SetVisibilityModifier` milik `LightSourceSystem`. Komponen ini hanya memasang `LightSource`.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim dan didengar:** tidak ada.

| Sistem | Hubungan |
|---|---|
| `EquipmentManager` (#60) | `ToggleOffHand`, `OnEquip`, dan `OnUnequip` |
| `InputManager` (#18) | `FlashlightPressed`, dibaca `EquipmentManager` |
| `LightSourceSystem` (#98) | `LightSource`, `Recharge`, dan keterlihatan |
| `SpecialZombieAI` (#25) | Zombie Lumut membaca `IsPointInBeam` |
| `InventoryManager` (#66) | `ItemInstance.durability` |
| `ToolRepairSystem` (#113) | Senter tidak bisa diperbaiki |

## 4. Kasus Khusus

- **Pool dipakai ulang:** viewmodel memakai cache `EquipmentManager`, bukan pool. `OnEquip` mengisi ulang status.
- **Game dijeda:** `Time.deltaTime` 0, jadi baterai berhenti.
- **Kapak atau Kail dipegang:** senter disimpan otomatis oleh `EquipmentManager`, sehingga `OnUnequip` mematikan lampu dan baterai berhenti.
- **Baterai habis:** `OnEquip` tidak menyalakan lampu sampai `Recharge` dipanggil.
- **Save dan load:** isi baterai mengikuti `ItemInstance`. Status tangan kiri mengikuti `EquipmentSaveData.offHandActive`.
- **Permadeath:** senter dan isinya hilang bersama inventori run.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** angka baterai dan `Item_Battery` (#98, `LightSourceSystem.md`), serta slot tangan kiri (#60).

**Usulan yang menunggu developer (#85):**

1. **Senter dan baterai:** GDD tidak menyebutnya. Rekomendasi: pakai, supaya Region 2 tetap gelap tanpa membuat pemain buta.

## 6. Skenario Tes

1. Tekan `F` dengan senter terisi. **Hasil:** lampu menyala, dan `LightSource` terdaftar dengan `isHeld = true`.
2. Biarkan menyala 500 detik. **Hasil:** lampu berkedip lalu mati, dan senter tetap di tangan.
3. Pakai `Item_Battery`. **Hasil:** `Recharge` mengisi sampai 100.
4. Ganti ke Kapak saat senter menyala. **Hasil:** senter disimpan, dan baterai berhenti berkurang.
5. Arahkan sorotan ke Zombie Lumut. **Hasil:** `IsPointInBeam` bernilai `true`. Matikan senter. **Hasil:** `false`.
6. Cari `currentBattery` dan pembacaan tombol di skrip. **Hasil:** tidak ada.
7. Simpan dengan isi 40, lalu muat. **Hasil:** isi tetap 40.
