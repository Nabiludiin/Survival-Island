# Blueprint: DurabilitySystem.cs
**Lokasi:** `Survival Island/doc/blueprint/Core_Logic/DurabilitySystem.md`
**Deskripsi Utama:** Kelas statis yang mengurangi durabilitas alat dan senjata, serta membaca statusnya. Nilainya disimpan di `ItemInstance.durability`. Aturan status dan perbaikan dimiliki `ToolRepairSystem.md`.
**Issue:** #113, #66, #60, #72
**Rujukan GDD:** Bagian 7 (Fire Axe tidak bisa rusak). Durabilitas alat dan senjata biasa adalah usulan, belum ada di GDD, dan sudah disetujui sebagai nilai awal di #113. Aturan perbaikan ada di `ToolRepairSystem.md`.

## 1. Arsitektur & Variabel Utama

`DurabilitySystem` adalah kelas statis, tanpa `MonoBehaviour` dan tanpa status sendiri. Semua data berada di `ItemInstance` (#66), jadi durabilitas ikut ke mana pun item pergi: slot inventori, hotbar, peti, atau `WorldItem`.

```csharp
public enum DurabilityState { NoDurability, Normal, Low, Broken, Destroyed }
```

**Dihapus:** `maxDurability`, `currentDurability`, dan `decayPerUse` di MonoBehaviour, serta `UseItem` dan `BreakItem`. Decay dibaca dari `ItemData` atau `GameBalanceConfig` oleh pemanggil, lalu dikirim sebagai `amount`.

`maxDurability == -1` atau `inst.durability == -1` berarti tanpa durabilitas, sesuai `ItemUseSystem.md`.

### Data yang dibaca

| Data | Sumber | Fungsi |
|---|---|---|
| `inst.durability` | `ItemInstance` | Nilai saat ini. -1 berarti tanpa durabilitas |
| `inst.repairCount` | `ItemInstance` (#113) | Jumlah perbaikan, untuk `Destroyed` dan batas maksimum |
| `inst.upgradeMask`, `inst.luckyModifierId` | `ItemInstance` (#113, #72) | Pengali batas maksimum |
| `data.maxDurability` | `ItemData`, lewat `ItemDatabase.Get(inst.itemId)` | Batas dasar |
| `data.isCoreItem` | `ItemData` | Item inti tidak pernah `Destroyed` |

### Angka penyetelan

Sistem ini tidak punya angka sendiri. Angka yang dibaca:

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.repair.lowRatio` | 0,2 | Batas status `Low` |
| `GameBalanceConfig.repair.maxLossPerRepair` | 0,1 | Penurunan maksimum per perbaikan |
| `GameBalanceConfig.repair.maxRepairs` | 6 | Batas perbaikan sebelum `Destroyed` |
| `GameBalanceConfig.upgrade.durabilityBonus` | 0,25 | Tambahan maksimum dari `Upg_Durability` |

Jumlah keausan per aksi milik pemanggil:

| Pemanggil | Aksi | `amount` |
|---|---|---|
| `MeleeCombatSystem`, `ResourceNode` (#73) | Ayunan Kapak yang kena | `resource.axeDecayPerUse` (1) |
| `MeleeCombatSystem`, `ResourceNode` (#73) | Ayunan Beliung yang kena | `resource.pickaxeDecayPerUse` (1) |
| `ResourceRespawn` (#73) | Memanen dengan pisau | `resource.knifeDecayPerHarvest` (0,5) |
| `BuildMenu` (#79) | Membangun dan membongkar | `build.hammerDecayPerBuild` (1), `build.hammerDecayPerDemolish` (1) |
| `BuildingIntegrity` (#47) | Ayunan perbaikan dan upgrade | `integrity.hammerDecayPerRepairSwing` (0,5), `integrity.hammerDecayPerUpgrade` (2) |
| `GunController` (#58) | Satu tembakan | `WeaponData.wearPerShot` (Pistol 0,25) |
| `PlayerPinnedState` (#110) | Satu tusukan pisau saat diterkam | `pinned.knifeDurabilityCost` (5) |

Tabel ini hanya daftar baca. Kalau angka berubah, ubah di blueprint pemiliknya. Kail tidak aus saat memancing (`FishingSystem.md`, `ToolRepairSystem.md`).

## 2. Detail Logika & Fungsi

**`public static DurabilityState ApplyWear(ItemInstance inst, float amount)`**, sesuai `ToolRepairSystem.md`:

1. Kalau `inst == null` atau `amount <= 0`, kembalikan `GetState(inst)` tanpa perubahan.
2. Kalau `inst.durability < 0`, kembalikan `NoDurability`. `Reward_FireAxe` dan senjata ber-`mod_indestructible` termasuk di sini (#72).
3. Simpan status lama lewat `GetState`.
4. `inst.durability = Max(0, durability - amount)`.
5. Hitung status baru lewat `GetState`.
6. Kalau status berubah, terbitkan `DurabilityStateChangedEvent { instanceId, itemId, state }`.
7. Kalau `Destroyed`, panggil `InventoryManager.RemoveInstance(instanceId)`. `EquipmentManager` lalu menjalankan alur item hancur.

Event dikirim sebelum `RemoveInstance`, supaya HUD dan notifikasi masih bisa membaca nama item. Fungsi ini tidak membaca `Time`, jadi aman dipanggil dari mana pun.

**`public static DurabilityState GetState(ItemInstance inst)`**, mengikuti tabel di `ToolRepairSystem.md`:

1. Kalau `inst == null` atau `inst.durability < 0`, kembalikan `NoDurability`.
2. Ambil `data = ItemDatabase.Get(inst.itemId)`. Kalau `null` atau `data.maxDurability < 0`, kembalikan `NoDurability`, dengan peringatan untuk `null`.
3. Kalau `inst.durability <= 0`:
   - Kalau `!data.isCoreItem` dan `inst.repairCount >= repair.maxRepairs`, kembalikan `Destroyed`.
   - Selain itu, kembalikan `Broken`.
4. Kalau `inst.durability <= GetEffectiveMax(inst, data) * repair.lowRatio`, kembalikan `Low`.
5. Selain itu, kembalikan `Normal`.

Durabilitas 0 berarti `Broken`, bukan langsung hilang. Status selalu dihitung dari data, jadi tidak disimpan.

**`public static float GetEffectiveMax(ItemInstance inst, ItemData data)`** menghitung batas setelah pengurangan dari jumlah perbaikan, upgrade, dan modifier Lucky Machine. Rumusnya ada di `ToolRepairSystem.md` bagian "Pengurangan durabilitas". Item inti tidak terkena pengurangan.

**`public static float GetRatio(ItemInstance inst)`** (usulan nama, untuk bar HUD): `durability / GetEffectiveMax`, dibatasi 0 sampai 1. Kembalikan 1 untuk `NoDurability`. Dipakai `EquipmentManager` untuk bar durabilitas hotbar, supaya rumusnya hanya ada di satu tempat.

**Item hancur:** hanya status `Destroyed` yang memicu `InventoryManager.RemoveInstance(instanceId)`, di dalam `ApplyWear` langkah 7. Lalu `EquipmentManager` menangani item aktif yang hilang.

**Nilai awal item baru:** pembuat `ItemInstance` (#66) mengisi `durability = data.maxDurability`. Untuk item tanpa durabilitas nilainya -1. `DurabilitySystem` tidak membuat item.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `DurabilityStateChangedEvent { instanceId, itemId, state }`, tidak sticky (`GameEventBus.md`).

**Event yang didengar:** tidak ada.

| Sistem | Hubungan |
|---|---|
| `ToolRepairSystem` (#113) | Pemilik aturan status, perbaikan, dan `GetEffectiveMax`. Mengisi ulang `durability` setelah perbaikan, lalu mengirim event yang sama |
| `EquipmentManager` (#60) | Menolak aksi item `Broken`, menangani item `Destroyed`, dan menggambar bar |
| `GunController` (#58), `MeleeCombatSystem`, `ResourceNode` (#73), `ResourceRespawn` (#73), `BuildMenu` (#79), `BuildingIntegrity` (#47), `PlayerPinnedState` (#110) | Memanggil `ApplyWear` dan `GetState` |
| `InventoryManager` (#66) | `RemoveInstance` |
| `ItemDatabase` (#66) | `Get(itemId)` |
| `LuckyMachine` (#72) | Mengisi `durability = -1` untuk `mod_indestructible` |
| `NotificationManager` (#22) | Peringatan `Low` dan `Broken`, dari `DurabilityStateChangedEvent` lewat `ToolRepairSystem` |

## 4. Kasus Khusus

- **Objek pool:** tidak ada status, jadi tidak perlu reset. `WorldItem` dari pool membawa `ItemInstance`, dan resetnya diatur #66.
- **Game dijeda:** hanya berjalan saat dipanggil. Semua pemanggil adalah aksi pemain, yang berhenti saat dijeda.
- **Permadeath:** data hilang bersama `ItemInstance` dan save run (#82). Tidak ada yang terbawa.
- **Save dan load:** `durability` dan `repairCount` disimpan bersama `ItemInstance` di `player_inventory`, isi peti (#46), dan `WorldItem` di `RegionStateStore`. Status tidak disimpan. Event tidak dikirim ulang saat load, karena HUD membaca `GetState` saat menggambar.
- **Item tanpa durabilitas:** `GetState` mengembalikan `NoDurability`.
- **Item inti:** tidak pernah `Destroyed`, sesuai `ToolRepairSystem.md`. Pistol, Kail, dan Pisau berhenti di `Broken`.
- **Keausan pada item `Broken`:** pemanggil seharusnya sudah menolak aksi. Kalau tetap terpanggil, `durability` tetap 0 dan status tidak berubah, jadi tidak ada event ganda.
- **Pecahan desimal:** `durability` memakai `float`. Pemeriksaan `<= 0` di `GetState` mencegah item tertahan di 0,0001.
- **`ItemData.maxDurability` diubah di versi baru:** `durability` lama bisa melebihi maksimum baru. `GetRatio` membatasi ke 1, dan perbaikan berikutnya menyamakan nilainya.
- **Durabilitas sebagai bahan bakar:** `Tool_Torch` (`WarmthSystem.md`) dan `Tool_Flashlight` (`FlashlightController.md`) menulis `durability` langsung sebagai sisa bakar atau baterai, tanpa `ApplyWear`. Keduanya tidak bisa diperbaiki, dan tidak mengirim `DurabilityStateChangedEvent`. Obor dihapus sendiri saat habis. Senter yang baterainya 0 terbaca `Broken` sampai diisi ulang.
- **Item non-inti yang tidak bisa diperbaiki:** kalau mencapai 0 lewat `ApplyWear`, status `Broken` tidak pernah berakhir, karena `repairCount` tidak naik. Lihat bagian 5.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** kelas statis, `ApplyWear`, `GetState`, dan `GetEffectiveMax` (`ToolRepairSystem.md`). -1 berarti tanpa durabilitas (`ItemUseSystem.md`). Item inti bisa rusak tetapi tidak hancur (keputusan #113 dan #58). Rusak berbeda dari hancur (keputusan #113).

**Keputusan teknis di blueprint ini:**

- Event dikirim sebelum `RemoveInstance`.
- `GetState` mengambil `ItemData` sendiri lewat `ItemDatabase`, supaya pemanggil cukup mengirim `ItemInstance`.

**Menunggu developer:**

1. **Item non-inti yang tidak bisa diperbaiki dan memakai `ApplyWear`:** saat ini tidak ada item seperti itu di daftar biaya `ToolRepairSystem.md`. Usulan: validasi Editor menolak item dengan `maxDurability > 0`, `isRepairable = false`, dan `isCoreItem = false`, kecuali obor dan senter yang memakai durabilitas sebagai bahan bakar.
2. **`GetRatio`:** nama fungsi baru. Kalau disetujui, `EquipmentManager.md` memakainya untuk bar hotbar.

## 6. Skenario Tes

1. Pakai kapak sampai durabilitas 0. **Hasil:** status `Broken`, item tetap di inventori, aksi ditolak, dan event terkirim sekali saat masuk `Low` dan sekali saat `Broken`.
2. Pakai `Reward_FireAxe`. **Hasil:** durabilitas tidak berubah, dan tidak ada event.
3. Pasang `mod_indestructible`, lalu serang. **Hasil:** durabilitas tidak berubah.
4. Habiskan batas perbaikan pada item non-inti. **Hasil:** status `Destroyed`, event terkirim, lalu item hilang dan `EquipmentManager` memilih slot lain.
5. Tembak `Base_Pistol` sampai durabilitas 0, lalu perbaiki 6 kali dan ulangi. **Hasil:** Pistol tetap `Broken`, tidak pernah `Destroyed`, dan maksimumnya tidak turun.
6. Panggil `ApplyWear` dengan `amount = 0` atau negatif. **Hasil:** tidak ada perubahan dan tidak ada event.
7. Pukul batu dengan Kapak, yang ditolak node. **Hasil:** durabilitas tetap.
8. Simpan kapak dengan durabilitas 37 di peti, keluar, lalu lanjutkan. **Hasil:** durabilitas tetap 37, dan bar sama.
9. Jatuhkan kapak `Broken`, lalu ambil lagi. **Hasil:** tetap `Broken`.
10. Kurangi `maxDurability` kapak di aset, lalu lanjutkan save lama. **Hasil:** bar penuh, tanpa error.
11. Buka Pause saat memegang senter menyala. **Hasil:** baterai tidak berkurang selama dijeda.
12. Mati, lalu mulai run baru. **Hasil:** alat baru punya durabilitas penuh.

## Catatan Implementasi (Sistem pendukung)

- Kode: `Assets/LOGIC/Core_Logic/DurabilitySystem.cs` (enum `DurabilityState` dan kelas statis). Event di `GameEvents.FondasiB.cs`.
- `GetEffectiveMax` membaca upgrade dan modifier lewat `WeaponStatCalculator.HasUpgrade` dan `WeaponStatCalculator.FindModifier` (lihat catatan `ToolRepairSystem.md`).
- `GetRatio` ditulis dengan nama usulan (masih "Menunggu developer" nomor 2). `HotbarView` memakainya.
- Validasi Editor untuk item non-inti yang tidak bisa diperbaiki (Menunggu developer nomor 1) belum ditulis.
