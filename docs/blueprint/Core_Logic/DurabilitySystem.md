# Blueprint: DurabilitySystem.cs
**Lokasi:** `docs/blueprint/Core_Logic/DurabilitySystem.md`
**Deskripsi Utama:** Kelas statis yang mengurangi durabilitas alat dan senjata, serta membaca statusnya. Nilainya disimpan di `ItemInstance.durability`.
**Issue:** #113, #66, #60, #72
**Rujukan GDD:** Usulan, belum ada di GDD. Aturan perbaikan ada di `ToolRepairSystem.md`.

## 1. Arsitektur & Variabel Utama

`DurabilitySystem` adalah kelas statis, tanpa `MonoBehaviour` dan tanpa status sendiri.

```csharp
public enum DurabilityState { NoDurability, Normal, Low, Broken, Destroyed }
```

**Dihapus:** `maxDurability`, `currentDurability`, dan `decayPerUse` di MonoBehaviour, serta `UseItem` dan `BreakItem`. Decay dibaca dari `ItemData` atau `GameBalanceConfig`, misalnya `resource.axeDecayPerUse`.

`maxDurability == -1` atau `inst.durability == -1` berarti tanpa durabilitas, sesuai `ItemUseSystem.md`.

## 2. Detail Logika & Fungsi

**`public static DurabilityState ApplyWear(ItemInstance inst, float amount)`**, sesuai `ToolRepairSystem.md`:

1. Kalau `inst.durability < 0`, kembalikan `NoDurability`. `Reward_FireAxe` dan senjata ber-`mod_indestructible` termasuk di sini (#72).
2. Simpan status lama, lalu `inst.durability = Max(0, durability - amount)`.
3. Hitung status baru lewat `GetState`.
4. Kalau `Destroyed`, panggil `InventoryManager.RemoveInstance(instanceId)`. `EquipmentManager` lalu menjalankan alur item hancur.
5. Kalau status berubah, terbitkan `DurabilityStateChangedEvent { instanceId, itemId, state }`.

**`public static DurabilityState GetState(ItemInstance inst)`** mengikuti tabel di `ToolRepairSystem.md`. Durabilitas 0 berarti `Broken`, bukan langsung hilang.

**`public static float GetEffectiveMax(ItemInstance inst, ItemData data)`** menghitung batas setelah pengurangan dari jumlah perbaikan.

**Item hancur:** hanya status `Destroyed` yang memicu `InventoryManager.RemoveInstance(instanceId)`, di dalam `ApplyWear` langkah 4. Lalu `EquipmentManager` menangani item aktif yang hilang.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `DurabilityStateChangedEvent`.

**Event yang didengar:** tidak ada.

| Sistem | Hubungan |
|---|---|
| `ToolRepairSystem` (#113) | Pemilik aturan perbaikan dan status |
| `EquipmentManager` (#60) | Menolak aksi item `Broken`, dan menangani item `Destroyed` |
| `GunController` (#58), `MeleeCombatSystem`, `ResourceNode` (#73), `PlayerPinnedState` (#110) | Memanggil `ApplyWear` dan `GetState` |
| `InventoryManager` (#66) | `RemoveInstance` |

## 4. Kasus Khusus

- **Objek pool:** tidak ada status, jadi tidak perlu reset.
- **Game dijeda:** hanya berjalan saat dipanggil.
- **Permadeath:** data hilang bersama `ItemInstance`.
- **Save dan load:** `durability` disimpan bersama `ItemInstance`.
- **Item tanpa durabilitas:** `GetState` mengembalikan `NoDurability`.
- **Item inti:** tidak pernah `Destroyed`, sesuai `ToolRepairSystem.md`.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** kelas statis, `ApplyWear`, `GetState`, dan `GetEffectiveMax` (`ToolRepairSystem.md`). -1 berarti tanpa durabilitas (`ItemUseSystem.md`).

**Menunggu developer:** tidak ada yang menghalangi.

## 6. Skenario Tes

1. Pakai kapak sampai durabilitas 0. **Hasil:** status `Broken`, item tetap di inventori, dan aksi ditolak.
2. Pakai `Reward_FireAxe`. **Hasil:** durabilitas tidak berubah.
3. Pasang `mod_indestructible`, lalu serang. **Hasil:** durabilitas tidak berubah.
4. Habiskan batas perbaikan pada item non-inti. **Hasil:** status `Destroyed`, lalu item hilang.
5. Simpan, keluar, lalu lanjutkan. **Hasil:** durabilitas per item sama.
