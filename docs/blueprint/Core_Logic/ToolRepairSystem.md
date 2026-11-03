# Blueprint: ToolRepairSystem.cs
**Lokasi:** `docs/blueprint/Core_Logic/ToolRepairSystem.md`
**Deskripsi Utama:** Perbaikan alat dan senjata, status rusak dan hancur, upgrade kecil per item, serta satu urutan hitung untuk modifier upgrade dan Lucky Machine.
**Issue:** #113
**Rujukan GDD:** Bagian 7 (Fire Axe tak bisa rusak dan senjata hadiah). Perbaikan dan upgrade adalah usulan, belum ada di GDD. `Base_Pistol`, `Base_Kail`, dan `Base_Pisau` mengikuti aturan item inti di `GLOSSARY.md`.

## 1. Arsitektur & Variabel Utama

### Pola

Durabilitas tetap berada di `ItemInstance.durability`. `DurabilitySystem` menjadi kelas statis tanpa `MonoBehaviour`, dan hanya mengurangi nilai itu. `ToolRepairSystem` adalah manager di `IslandPhase` yang menjalankan satu pekerjaan perbaikan atau upgrade. Panelnya adalah tab Perbaiki di `CraftingPanel` (#77).

### Tambahan pada `ItemInstance`

| Field | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `repairCount` | `int` | 0 | Jumlah perbaikan |
| `upgradeMask` | `int` | 0 | Sudah dicadangkan. Tiap bit satu `ItemUpgradeData` |
| `luckyModifierId` | `string` | Kosong | Modifier dari Lucky Machine (#72) |

### Tambahan pada `ItemData`

| Field | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `isRepairable` | `bool` | `false` | Bisa diperbaiki |
| `repairCost` | `RecipeIngredient[]` | Kosong | Biaya perbaikan penuh |
| `repairStation` | `CraftingStationType` | `WoodBench` | Stasiun perbaikan |

`maxDurability = -1` berarti tidak punya durabilitas, seperti `Reward_FireAxe`. Item ini tidak pernah rusak dan tidak tampil di tab Perbaiki.

### Status durabilitas

```csharp
public enum DurabilityState { NoDurability, Normal, Low, Broken, Destroyed }
```

| Status | Syarat | Akibat |
|---|---|---|
| `Normal` | Di atas `lowRatio` dari maksimum efektif | Normal |
| `Low` | Sampai `lowRatio` | Bar kuning dan peringatan sekali |
| `Broken` | `durability == 0` | Tetap di slot, tidak bisa dipakai, dan bisa diperbaiki |
| `Destroyed` | Non-inti, `durability == 0`, dan `repairCount >= maxRepairs` | Dihapus dari inventori |

### Biaya perbaikan awal

| Item | Biaya penuh | Stasiun | Batas maksimum turun |
|---|---|---|---|
| `Tool_Axe`, `Tool_Pickaxe` | 2 `Mat_Wood`, 1 `Mat_Stone` | `WoodBench` | Ya |
| `Tool_Hammer` | 1 `Mat_Wood`, 1 `Mat_Stone` | `Hand` | Ya |
| Senjata api non-inti (#58) | 2 `Mat_Cloth`, 2 `Mat_Stone` | `WoodBench` | Ya |
| `Base_Pistol` | 1 `Mat_Cloth` | `Hand` | Tidak, item inti |
| `Base_Kail` | 2 `Mat_Wood` | `Hand` | Tidak, item inti |
| `Base_Pisau` | 1 `Mat_Stone` | `Hand` | Tidak, item inti |
| `Tool_Torch`, `Tool_Flashlight` | Tidak bisa | Tidak ada | Mengikuti #65 dan `FlashlightController` |
| `Reward_FireAxe` | Tidak bisa, dan tidak perlu | Tidak ada | Tidak punya durabilitas |

### Upgrade

```csharp
public enum UpgradeKind { Damage, MagazineSize, Speed, Durability }

[CreateAssetMenu(menuName = "Survival/Item Upgrade")]
public class ItemUpgradeData : ScriptableObject
{
    public string upgradeId;
    public int maskBit;                  // 0 sampai 30, tetap setelah rilis
    public UpgradeKind kind;
    public ItemType[] appliesTo;
    public RecipeIngredient[] cost;
    public CraftingStationType station;  // WoodBench
    public string techNodeId;            // Kosong berarti terbuka sejak awal
}
```

| `upgradeId` | Berlaku untuk | Efek | Biaya |
|---|---|---|---|
| `Upg_Damage` | Weapon dan Tool | Damage naik `upgrade.damageBonus` | 4 `Mat_Stone`, 2 `Mat_Wood` |
| `Upg_Magazine` | Senjata api | Magazine naik `upgrade.magazineBonus` | 4 `Mat_Stone`, 2 `Mat_Cloth` |
| `Upg_Speed` | Weapon dan Tool | Fire rate atau ayunan naik `upgrade.speedBonus` | 2 `Mat_Rope`, 2 `Mat_Wood` |
| `Upg_Durability` | Item dengan durabilitas | Maksimum naik `upgrade.durabilityBonus` | 2 `Mat_Rope`, 4 `Mat_Wood` |

Tiap jenis hanya bisa dipasang sekali per item, dan maksimum `upgrade.maxPerItem`. `Upg_Magazine` juga mensyaratkan item punya `WeaponData` (#58). Bonusnya kecil, supaya senjata hadiah tetap istimewa.

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.repair.lowRatio` | 0,2 | Batas status `Low` |
| `GameBalanceConfig.repair.maxLossPerRepair` | 0,1 | Penurunan maksimum per perbaikan |
| `GameBalanceConfig.repair.maxRepairs` | 6 | Batas perbaikan item non-inti |
| `GameBalanceConfig.repair.repairSeconds` | 3 | Lama memperbaiki |
| `GameBalanceConfig.repair.upgradeSeconds` | 6 | Lama memasang upgrade |
| `GameBalanceConfig.upgrade.damageBonus` | 0,1 | Tambahan damage |
| `GameBalanceConfig.upgrade.magazineBonus` | 2 | Tambahan isi magazine |
| `GameBalanceConfig.upgrade.speedBonus` | 0,1 | Tambahan kecepatan |
| `GameBalanceConfig.upgrade.durabilityBonus` | 0,25 | Tambahan durabilitas |
| `GameBalanceConfig.upgrade.maxPerItem` | 2 | Batas upgrade per item |
| `GameBalanceConfig.upgrade.maxDamageMultiplier` | 4 | Batas akhir pengali damage |

## 2. Detail Logika & Fungsi

### Pengurangan durabilitas

**`public static DurabilityState ApplyWear(ItemInstance inst, float amount)`**, pengganti `UseItem`:

1. Kalau `inst.durability < 0`, kembalikan `NoDurability`.
2. Simpan status lama, lalu `inst.durability = Max(0, durability - amount)`.
3. Hitung status baru lewat `GetState`.
4. Kalau `Destroyed`, panggil `InventoryManager.RemoveInstance(instanceId)`. `EquipmentManager` lalu menjalankan alur item hancur.
5. Kalau status berubah, terbitkan `DurabilityStateChangedEvent { instanceId, itemId, state }`.

**`public static float GetEffectiveMax(ItemInstance inst, ItemData data)`**:

```csharp
float lossRatio = data.isCoreItem ? 0f : inst.repairCount * maxLossPerRepair;
float upgrade = HasUpgrade(inst, UpgradeKind.Durability) ? 1f + durabilityBonus : 1f;
float lucky = LuckyModifier(inst)?.durabilityMultiplier ?? 1f;
return data.maxDurability * (1f - lossRatio) * upgrade * lucky;
```

Item inti tidak pernah mencapai `Destroyed`. Peringatan tampil saat masuk `Low`, sekali per penurunan: `Kapak hampir rusak` (#22), disertai SFX dan bar kuning. Saat `Broken`, tampil `Kapak rusak`. Status dihitung dari data, jadi tidak perlu disimpan.

### Item yang rusak

`IEquippable.PrimaryAction` tidak dipanggil untuk item `Broken`. `EquipmentManager` memeriksa `DurabilitySystem.GetState`, memainkan suara patah, dan menampilkan notifikasi. Item tetap di hotbar. Item rusak tidak hilang saat disimpan di peti (#46) atau dijatuhkan (#66), karena `ItemInstance` ikut bersamanya.

### Memperbaiki

**`public RepairCheck CanRepair(ItemInstance inst)`** memeriksa `isRepairable`, `durability` belum penuh, stasiun di `CraftingManager.availableMask`, batas perbaikan belum tercapai, dan bahan tersedia.

Biaya mengikuti kerusakan: `Ceil(cost * (1 - durability / effectiveMax))`, minimum 1 tiap bahan. Item inti selalu punya resep `Hand`, supaya pemain tidak terkunci di tengah run.

**`public bool StartRepair(int instanceId)`**:

1. Pastikan `CanRepair` lolos dan tidak ada pekerjaan berjalan.
2. Isi `currentJob` dengan `repairSeconds`. Bahan belum dipotong.
3. `Update` mengurangi timer dengan `Time.deltaTime`. Kalau stasiun di luar jangkauan, panel ditutup, atau `PlayerDamagedEvent` terbit, pekerjaan batal tanpa biaya.
4. Saat selesai, periksa ulang bahan, lalu panggil `RemoveItem`.
5. Kalau bukan item inti, `repairCount++`.
6. `durability = GetEffectiveMax(...)`. Perbaikan selalu penuh terhadap maksimum yang baru.
7. Terbitkan `DurabilityStateChangedEvent`, lalu mainkan SFX.

**Contoh:** Kapak dengan `maxDurability` 100. Perbaikan pertama mengisi sampai 90, kedua 80. Setelah 6 kali, maksimum 40. Saat 40 itu habis, kapak hancur. Dengan `axeDecayPerUse` 1, satu kapak bertahan total 490 ayunan (100 + 90 + 80 + 70 + 60 + 50 + 40), sebelum perlu dibuat baru.

### Upgrade

**`public bool StartUpgrade(int instanceId, ItemUpgradeData upgrade)`** memakai alur pekerjaan yang sama, dengan `upgradeSeconds`. Saat selesai:

1. Potong bahan, lalu atur bit `upgradeMask`.
2. Kalau `Durability`, tambahkan selisih maksimum ke `durability`.
3. Kalau `MagazineSize`, kapasitas naik, dan `magazineAmmo` tidak berubah.
4. Panggil `GunController.RecalculateStats` (#58) atau `MeleeCombatSystem.RecalculateStats` kalau item sedang dipegang.

### Menghitung statistik

**`public static WeaponStats WeaponStatCalculator.Get(in WeaponStats baseStats, ItemInstance inst)`** adalah satu-satunya tempat modifier digabung:

| Urutan | Sumber | Rumus |
|---|---|---|
| 1 | `WeaponData` (#58) atau `ItemData.weaponDamage` | Nilai dasar |
| 2 | Upgrade (#113) | `damage * (1 + damageBonus)`, `speed * (1 + speedBonus)`, dan `magazine + magazineBonus` |
| 3 | Lucky Machine (#72), `WeaponModifierData` | `damage * damageMultiplier`, `speed * speedMultiplier`, dan `magazine + magazineBonus` |
| 4 | Batas | Damage maksimum `base * maxDamageMultiplier`, dan magazine minimum 1 |

Hasilnya dihitung saat item dipasang atau berubah, lalu disimpan oleh pengendali senjata. Jadi tembakan tidak mengulang perhitungan dan tidak membuat garbage. Hasil ini menjadi `DamageInfo.amount`, sesuai langkah 1 di `DamagePipeline.md`. Armor dan pengali headshot 2x dihitung sesudahnya.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `DurabilityStateChangedEvent`.

**Event yang didengar:** `PlayerDamagedEvent` (membatalkan pekerjaan), `UIFocusChangedEvent` (panel tertutup), dan `PlayerDiedEvent` (berhenti).

| Sistem | Hubungan |
|---|---|
| `DurabilitySystem` | Ditulis ulang menjadi `ApplyWear`, `GetState`, dan `GetEffectiveMax` |
| `ItemUseSystem` (#66), `InventoryManager` | `ItemInstance`, `RemoveInstance`, dan `RemoveItem` |
| `EquipmentManager` (#60) | Menolak aksi item `Broken`, dan menjalankan alur item hancur |
| `CraftingManager`, `CraftingPanel` (#77) | `availableMask`, `RecipeIngredient`, dan tab Perbaiki |
| `GunController` (#58), `MeleeCombatSystem` | Membaca `WeaponStatCalculator`, dan memanggil `ApplyWear` |
| `LuckyMachine` (#72) | Mengisi `luckyModifierId`, dan menyediakan `WeaponModifierData` |
| `ResourceRespawn` (#73), `FishingSystem` (#55), `BuildingIntegrity` (#47) | Memanggil `ApplyWear` |
| `HUDManager`, `NotificationManager` (#22) | Bar durabilitas dan peringatan |

**Batas pekerjaan:** perbaikan bangunan di #47, isi modifier gacha di #72, dan meja di #77.

## 4. Kasus Khusus

- **Objek pool:** sistem ini tidak punya objek pool. Efek percikan memakai `VFXManager` (#15), yang mengatur reset sendiri.
- **Game dijeda:** `currentJob` memakai `Time.deltaTime`, jadi berhenti. Panel `Crafting` sendiri tidak menjeda game.
- **Permadeath:** semua `ItemInstance`, upgrade, dan modifier hilang bersama run (#82). Tidak ada yang terbawa.
- **Save dan load:** tidak ada `ISaveable` sendiri. `durability`, `repairCount`, `upgradeMask`, dan `luckyModifierId` ikut `InventorySlot` (#66), isi peti (#46), dan `WorldItem` di `RegionStateStore`. `currentJob` tidak disimpan, jadi pekerjaan berjalan batal tanpa biaya.
- **`upgradeMask` setelah pembaruan game:** `maskBit` tidak boleh dipakai ulang, supaya save lama tetap benar.
- **Modifier tidak ditemukan:** `luckyModifierId` yang tidak dikenal diabaikan, dengan peringatan.
- **Item dipindah saat diperbaiki:** `StartRepair` menyimpan `instanceId`. Kalau `FindInstance` mengembalikan `null` saat selesai, pekerjaan batal.
- **Bahan hilang saat pekerjaan berjalan:** pemeriksaan ulang di langkah 4 membatalkan pekerjaan tanpa mengubah item.
- **Fire Axe:** `maxDurability = -1`, sehingga `ApplyWear` berhenti di langkah 1. Tab Perbaiki dan `Upg_Durability` tidak menampilkannya. Upgrade lain tetap boleh.
- **Laser Pistol:** statusnya mengikuti #58 dan #72. Kalau memakai durabilitas, biayanya ditulis di datanya.
- **Modifier `indestructible` dari #72:** `LuckyMachine` mengatur `inst.durability = -1`, dan status `Broken` langsung berakhir.
- **Item inti rusak saat tidak ada bahan:** Pisau dan Kail hanya butuh bahan Region 1. Pistol butuh `Mat_Cloth`, dengan sumber dari kapal dan Region 1, sesuai `CarryCapacity.md`. Sampai diperbaiki, pemain tetap punya pisau atau tangan.
- **Heavy Item:** membawa Heavy Item tidak menghalangi panel, tetapi perbaikan butuh jarak stasiun yang sama.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** item inti tidak bisa dibuang (`GLOSSARY.md`). Modifier masuk di langkah 1 urutan damage (`DamagePipeline.md`). Headshot 2x (`GLOSSARY.md`). Fire Axe tanpa `DurabilitySystem` (`EquipmentManager.md`).

**Keputusan teknis di blueprint ini:**

- `WeaponStatCalculator` menjadi satu titik gabungan modifier.
- Status durabilitas dihitung dari data.
- `DurabilitySystem` tanpa `MonoBehaviour`, sesuai `EquipmentManager.md`.

**Usulan yang menunggu developer:**

1. **Sistem perbaikan dan upgrade dipakai:** rekomendasi ya. GDD tidak menyebutnya, tetapi alat dan senjata perlu bertahan selama 60 hari, dan bisa lebih lama.
2. **Biaya dan stasiun:** tabel di bagian 1, dengan biaya sesuai tingkat kerusakan.
3. **Batas maksimum turun:** ya untuk item non-inti, 10 persen per perbaikan sampai 6 kali. Dengan begitu, Fire Axe tetap bernilai sebagai hadiah.
4. **Rusak dan hancur:** durabilitas 0 berarti rusak dan bisa diperbaiki. Item hancur hanya kalau sudah melewati batas perbaikan.
5. **Item inti (selaras dengan keputusan #58):** Pistol, Kail, dan Pisau bisa rusak tetapi tidak pernah hancur, maksimumnya tidak turun, dan biaya perbaikannya murah di `Hand`.
6. **Upgrade kecil:** 4 jenis, sekali per jenis, dan maksimum 2 per item. Damage dan kecepatan naik 10 persen, magazine naik 2, dan durabilitas naik 25 persen.
7. **Urutan modifier:** upgrade dulu, lalu Lucky Machine dengan perkalian, dibatasi 4 kali damage dasar.

## 6. Skenario Tes

1. Pakai kapak sampai durabilitas 20 persen. **Hasil:** bar kuning, dan peringatan muncul sekali.
2. Pakai kapak sampai 0. **Hasil:** kapak tetap di slot, klik kiri menolak, dan notifikasi `Kapak rusak` tampil.
3. Perbaiki kapak rusak di `WoodBench` dengan bahan cukup. **Hasil:** setelah 3 detik durabilitas 90, dan bahan berkurang.
4. Perbaiki saat durabilitas 50 persen dari maksimum. **Hasil:** biaya sesuai rumus, dengan minimum 1 tiap bahan.
5. Jalan menjauh saat memperbaiki. **Hasil:** pekerjaan batal, dan bahan utuh.
6. Terkena serangan saat memperbaiki. **Hasil:** pekerjaan batal, dan bahan utuh.
7. Ulangi perbaikan sampai `repairCount` 6, lalu habiskan. **Hasil:** kapak hancur, dan `EquipmentManager` memilih slot lain.
8. Habiskan `Base_Pistol`. **Hasil:** pistol tidak bisa menembak, tetap di slot, dan tidak hilang.
9. Perbaiki `Base_Pistol` berkali-kali. **Hasil:** maksimum tetap penuh.
10. Pakai `Reward_FireAxe`. **Hasil:** durabilitas tidak berkurang, dan tidak muncul di tab Perbaiki.
11. Pasang `Upg_Damage` pada pistol. **Hasil:** damage naik 10 persen. Headshot tetap 2x dari nilai baru.
12. Pasang `Upg_Magazine` saat isi 5/12. **Hasil:** menjadi 5/14.
13. Pasang upgrade dan modifier Lucky Machine bersamaan. **Hasil:** damage = dasar x 1,1 x pengali, dalam batas 4 kali.
14. Pasang upgrade ketiga. **Hasil:** ditolak.
15. Simpan item dengan upgrade ke peti, simpan game, lalu lanjutkan. **Hasil:** durabilitas, jumlah perbaikan, upgrade, dan modifier tetap sama.
16. Mati dan mulai run baru. **Hasil:** item inti kembali penuh, tanpa upgrade.
17. Buka Pause saat memperbaiki selama 10 detik. **Hasil:** timer tidak berkurang.
18. Tembak 100 kali dengan Profiler terbuka. **Hasil:** `WeaponStatCalculator` tidak dipanggil per tembakan, dan tidak ada alokasi GC.
