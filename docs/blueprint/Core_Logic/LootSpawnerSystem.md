# Blueprint: LootSpawnerSystem.cs
**Lokasi:** `docs/blueprint/Core_Logic/LootSpawnerSystem.md`
**Deskripsi Utama:** Menyebar loot di titik tertentu, di kapal maupun di pulau, dari seed run, lalu mengingat hasilnya supaya tidak berlipat setelah load.
**Issue:** #89, #74, #103
**Rujukan GDD:** Bagian 1 dan 5 (mencari barang di kapal dan pulau). Peluang dan aturan titik adalah usulan.

## 1. Arsitektur & Variabel Utama

`LootSpawnerSystem : MonoBehaviour, ISaveable`, dengan `SaveId = spawnerId`.

```csharp
[Serializable]
public class LootPointDef
{
    public Transform point;
    public LootPointKind kind;
    public ItemStack[] guaranteedItems;
}

[Serializable]
public struct LootPointState { public int pointIndex; public string itemId; public int amount; public bool isTaken; }
```

| Variabel | Tipe | Fungsi |
|---|---|---|
| `spawnerId` | `string` | ID stabil, juga bagian kunci acak |
| `table` | `LootTable` | Menggantikan `availableLoot` (#103) |
| `spawnPoints` | `LootPointDef[]` | Menggantikan `List<Transform>` |
| `states` | `List<LootPointState>` | Hasil yang sudah dicatat |

**Dihapus:** kelas `LootTable` lama, `dropChance`, `availableLoot`, `SpawnLootAcrossMap`, dan baris `Random.value > 0.5f`.

**Angka penyetelan:** `GameBalanceConfig.runSeed.lootPointEmptyChance` (0,5) untuk titik biasa, dan `GameBalanceConfig.loot.warehouseEmptyChance` (0,25) untuk `Warehouse`.

## 2. Detail Logika & Fungsi

**`Start()`** memanggil `SpawnOrRestore()`.

**`SpawnOrRestore()`**:

- Kalau status dari `RegionStateStore` ada, pulihkan dari `states`. Titik `isTaken` dilewati, tanpa pengacakan.
- Kalau belum ada, panggil `RollAndRecord()`.

**`RollAndRecord()`**, untuk tiap titik:

1. `var s = RunSeedManager.Instance.Fork(ShipLoot atau IslandLoot, spawnerId + "/" + pointIndex)`.
2. Kalau titik punya `guaranteedItems`, pakai isi tetap. Titik ini tidak bisa kosong.
3. Kalau tidak, lempar peluang kosong: `s.NextFloat01() < lootPointEmptyChance`, atau `warehouseEmptyChance` untuk `Warehouse`. Kalau kosong, lanjut ke titik berikutnya.
4. `RegionLootTable.TryRoll(table, kind, currentDay, ref s, out item, out amount)`.
5. Catat ke `states`, lalu munculkan objeknya.

**Wujud objek:**

| Lokasi | Wujud |
|---|---|
| `ShipPhase` | `ShipInteractable` dari pool `ShipLoot_Generic` (#74) |
| Pulau | `WorldItem` lewat `WorldItemManager.Spawn` (#66) |

**`public void MarkTaken(int pointIndex)`**: menandai `isTaken = true`. Dipanggil saat `WorldItem` habis diambil.

## 3. Hubungan dengan Sistem Lain

**Event:** tidak mengirim dan tidak mendengar event.

| Sistem | Hubungan |
|---|---|
| `RunSeedManager` (#89) | `Fork` dan rincian alur |
| `RegionLootTable` (#103) | `TryRoll` |
| `ObjectPooler`, `WorldItemManager` (#66) | Wujud item |
| `ShipInteractable`, `LifeboatExtraction` (#74) | Wujud barang kapal |
| `RegionStateStore` (#88), `WorldStateSave` (#80) | Menyimpan `states` |

## 4. Kasus Khusus

- **Objek pool:** objek yang diambil kembali ke pool. Spawner hanya memegang status.
- **Game dijeda:** tidak ada logika per frame.
- **Permadeath:** status hilang bersama save. Run baru memakai seed baru.
- **Save dan load:** `CaptureState` menulis `states` ke `RegionStateStore`. Region yang belum dikunjungi tetap menyebar dengan hasil sama, karena kuncinya tetap.
- **`ShipPhase`:** tidak punya save (#82), jadi selalu lewat `RollAndRecord()`.
- **Sebagian terambil:** `WorldItem` tetap ada, dan `MarkTaken` baru dipanggil saat habis.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** `SpawnOrRestore`, `LootPointState`, dan `MarkTaken` (`RunSeedManager.md`). `LootPointDef` (`RegionLootTable.md`). Pool kapal (`LifeboatExtraction.md`).

**Usulan yang menunggu developer (dicatat di #85):** peluang kosong 50 persen dan 25 persen untuk gudang.

## 6. Skenario Tes

1. Pakai seed yang sama dua kali. **Hasil:** susunan loot sama.
2. Ambil barang, simpan, keluar, lalu lanjutkan. **Hasil:** barang tidak muncul lagi, dan loot lain tetap.
3. Muat ulang region. **Hasil:** loot tidak berlipat.
4. Periksa kapal. **Hasil:** barang berupa `ShipInteractable` dari `ShipLoot_Generic`.
5. Periksa pulau. **Hasil:** barang berupa `WorldItem`.
6. Titik dengan `guaranteedItems`. **Hasil:** tidak pernah kosong.
7. Pakai 100 seed lewat `RunSeedAudit`. **Hasil:** titik kosong sekitar 50 persen.
