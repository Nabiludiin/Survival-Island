# Blueprint: ItemData.cs (ScriptableObject)
**Lokasi:** `Survival Island/doc/blueprint/Core_Logic/ItemData.md`
**Deskripsi Utama:** Cetakan data untuk variasi item, tanpa skrip terpisah per item.
**Issue:** #66, #58, #60, #65, #73, #104, #118
**Rujukan GDD:** Bagian 3 (item inti, makanan, dan obat). Field tambahan adalah usulan teknis.

## 1. Arsitektur & Variabel Utama

```csharp
[CreateAssetMenu(fileName = "New Item", menuName = "Survival/Item Data")]
public class ItemData : ScriptableObject
```

`ItemType` tetap mengikuti `GLOSSARY.md`: `Material`, `Consumable`, `Weapon`, `Tool`, dan `KeyItem`.

| Field | Tipe | Nilai awal | Sumber | Fungsi |
|---|---|---|---|---|
| `itemID` | `string` | Kosong | Lama | ID item |
| `displayNameKey` | `string` | `item.<itemId>.name` | #104 | Menggantikan `itemName`, selaras dengan `NPCData.displayNameKey` |
| `icon`, `prefab` | Aset | `null` | Lama | Tampilan |
| `type` | `ItemType` | `Material` | Lama | Jenis item |
| `healAmount` | `float` | 0 | Lama | Tambahan HP |
| `weaponDamage` | `float` | 0 | Lama | Hanya senjata melee. Senjata api memakai `weaponData` |
| `maxStack` | `int` | 1 | #66 | Batas tumpukan |
| `hungerDelta`, `thirstDelta`, `infectionDelta` | `float` | 0 | #66 | Perubahan status |
| `useSeconds`, `restoreSeconds` | `float` | 0 | #66 | Waktu pakai dan pemulihan |
| `isConsumable`, `isDroppable`, `isCoreItem` | `bool` | `false`, `true`, `false` | #66 | Aturan aksi |
| `maxDurability` | `float` | -1 | #66 | -1 berarti tidak memakai durabilitas |
| `spoilHours`, `poisonChance` | `float` | 0 | #66 | Pembusukan dan keracunan |
| `worldMesh`, `worldMaterial` | Aset | `null` | #66 | Tampilan `WorldItem` |
| `equipPrefab` | `EquippableBehaviour` | `null` | #60 | Prefab viewmodel |
| `handUse` | `HandUse` | `OneHand` | #60 | `OneHand`, `TwoHand`, `OffHand` |
| `weaponData` | `WeaponData` | `null` | #58 | `null` kalau bukan senjata api |
| `bagBonusSlots` | `int` | 0 | #118 | Tambahan slot dari tas |
| `shipWeight`, `shipPickupAmount` | `int` | 1, 1 | #118 | Berat dan jumlah di kapal |
| `toolClass` | `ToolClass` | `None` | #73 | Alat panen |
| `isWarmClothing` | `bool` | `false` | #65 | Dipakai `Gear_WarmClothes` |

Field `curesDiseases`, `fuelSeconds`, `appliesEffectId`, `isRepairable`, `repairCost`, dan `repairStation` tercatat di `ItemUseSystem.md`.

## 2. Detail Logika & Fungsi

`ItemData` hanya berisi data, dan tidak punya logika. `ItemDatabase.Get(itemId)` mencari aset lewat `Dictionary`.

| Item | `toolClass` |
|---|---|
| `Tool_Axe`, `Reward_FireAxe` | `Axe` |
| `Tool_Pickaxe` | `Pickaxe` |
| `Base_Pisau` | `Knife` |

## 3. Hubungan dengan Sistem Lain

**Event:** tidak mengirim dan tidak mendengar event.

| Sistem | Hubungan |
|---|---|
| `ItemUseSystem` (#66) | Membaca field pakai dan buang |
| `EquipmentManager` (#60) | `equipPrefab`, `handUse` |
| `GunController` (#58) | `weaponData` |
| `CarryCapacity` (#118), `ShipInteractable` | `bagBonusSlots`, `shipWeight`, `shipPickupAmount` |
| `ResourceRespawn`, `ResourceNode` (#73) | `toolClass` |
| `WarmthSystem` (#65) | `isWarmClothing` |
| `LocalizationManager` (#104) | `displayNameKey` |

## 4. Kasus Khusus

- **Objek pool:** `WorldItem` memasang mesh dari data saat `OnSpawned`.
- **Game dijeda:** data tidak berubah.
- **Permadeath:** aset tidak berubah.
- **Save dan load:** yang disimpan hanya `itemId`. `itemId` yang tidak ditemukan membuat slot kosong dengan peringatan (#80).
- **Teks hilang:** `displayNameKey` yang kosong mengikuti aturan `LocalizationManager.Get`.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** field #66 (`ItemUseSystem.md`), field #60 (`EquipmentManager.md`), field #118 (`CarryCapacity.md`), dan `ItemType` (`GLOSSARY.md`).

**Menunggu developer:** tidak ada yang menghalangi.

## 6. Skenario Tes

1. Buat aset `Food_Can`, lalu makan. **Hasil:** lapar naik sesuai `hungerDelta`.
2. Pegang `Base_Pistol`. **Hasil:** statistik dari `weaponData`, dan `weaponDamage` diabaikan.
3. Pegang `Tool_Axe` pada pohon. **Hasil:** `toolClass` `Axe` diterima node.
4. Ganti bahasa. **Hasil:** nama item ikut berubah.
5. Ambil barang di kapal. **Hasil:** berat sesuai `shipWeight`, dan jumlah sesuai `shipPickupAmount`.
