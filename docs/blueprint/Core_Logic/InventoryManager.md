# Blueprint: InventoryManager.cs
**Lokasi:** `docs/blueprint/Core_Logic/InventoryManager.md`
**Deskripsi Utama:** Pengelola slot inventori dan isi tangan di kapal. Satu kelas dipakai di dua fase, dengan instance di tiap scene.
**Issue:** #66, #74, #118, #81, #96, #110
**Rujukan GDD:** Bagian 1 dan 3 (item inti dan barang dari kapal). Aturan tumpukan, kapasitas tangan, dan pembusukan adalah usulan.

## 1. Arsitektur & Variabel Utama

`InventoryManager` berada di scene `ShipPhase` dan `IslandPhase` (#82). Rincian `ItemInstance`, `InventorySlot`, kesegaran, dan save ada di `ItemUseSystem.md`.

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `slots` | `List<InventorySlot>` | Sebanyak `carry.baseSlots` (15) | Slot berisi `ItemInstance` dan `amount` |
| `nextInstanceId` | `int` | 1 | ID unik item dalam satu run |
| `ShipHandWeight`, `ShipHandCapacity` | `int` | 0, `ship.handCapacity` | Berat tangan di kapal, dan batasnya |
| `ShipHandItems` | `List<ShipHandEntry>` | Kosong | Isi tangan: `itemId`, `amount`, `isNpc` |

**Diganti:** `maxSlots` publik diganti `SetMaxSlots(int)`, dan UI grid dipindah ke `InventoryPanel` (#66).

## 2. Detail Logika & Fungsi

| Fungsi | Kegunaan |
|---|---|
| `int AddItem(ItemInstance source, int amount)` | Mengembalikan sisa yang tidak muat. Memanggil `SlotArrayOps.Add` |
| `int AddItem(ItemData item, int amount)` | Membuat `ItemInstance` baru, lalu memakai fungsi di atas |
| `bool HasItem(string itemId, int amount)` | Memeriksa total |
| `int GetCount(string itemId)` | Total item di semua slot, memakai `SlotArrayOps.CountItem`. Dipakai #81 dan #96 |
| `void RemoveItem(string itemId, int amount)` | Mengurangi dari beberapa slot |
| `void RemoveFromSlot(int slotIndex, int amount)` | Mengurangi dari slot tertentu |
| `void RemoveInstance(int instanceId)` | Menghapus item unik |
| `int SplitStack(int slotIndex, int amount)` | Memisahkan tumpukan. Mengembalikan indeks baru, atau -1 |
| `void MoveSlot(int from, int to)` | Menukar dua slot |
| `ItemInstance FindInstance(int instanceId)` | Mencari item unik |
| `ItemInstance FindFirstInstance(string itemId)` | Mencari item pertama sesuai ID. Dipakai `PlayerPinnedState` untuk `Base_Pisau` saat Pisau tidak aktif (usulan #110) |
| `bool CanFitAfterRemoval(RecipeIngredient[] removed, ItemData result, int amount)` | Simulasi ruang tanpa mengubah slot (#77) |
| `void SetMaxSlots(int value)` | Mengikuti aturan naik dan turun di `CarryCapacity.md` |

**Pembusukan:** mendengar `HourChangedEvent`, lalu memeriksa slot berisi `spoilHours > 0` (#66).

**Isi saat tiba di pulau:** `Start` tidak lagi memanggil `LoadLootFromGameManager`. Pengisian dilakukan `IslandArrival.Apply` (#74).

**Tangan di kapal (#74):**

| Fungsi | Kegunaan |
|---|---|
| `bool AddShipHandItem(string id, int weight, int amount)` | Menambah barang ke tangan |
| `void ClearShipHand()` | Mengosongkan tangan |

Setiap perubahan isi tangan menerbitkan `ShipHandChangedEvent { weight, capacity }`.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `InventoryChangedEvent`, `ShipHandChangedEvent`.

**Event yang didengar:** `HourChangedEvent`.

| Sistem | Hubungan |
|---|---|
| `ItemUseSystem` (#66) | Pemilik aturan pakai dan kontrak save |
| `CarryCapacity` (#118) | Memanggil `SetMaxSlots` dan `ApplyShipLoot` |
| `IslandArrival`, `LifeboatExtraction` (#74) | Mengisi inventori, dan membaca isi tangan |
| `ShipInteractable` | Memanggil `AddShipHandItem` |
| `DurabilitySystem`, `ToolRepairSystem` (#113) | Mengubah data `ItemInstance` |
| `CraftingManager` (#77) | `HasItem`, `RemoveItem`, `AddItem`, dan `CanFitAfterRemoval` |
| `ThrowableSystem` (#96), `ObjectiveManager` (#81) | Memakai `GetCount` |
| `PlayerPinnedState` (#110) | Membaca `Base_Pisau` lewat `FindFirstInstance` |

## 4. Kasus Khusus

- **Objek pool:** kelas ini tidak dipakai ulang. Item berlebih menjadi `WorldItem` lewat `WorldItemManager`.
- **Game dijeda:** jam game berhenti, jadi pembusukan berhenti.
- **Permadeath:** isi inventori dan `nextInstanceId` hilang bersama save run.
- **Save dan load:** `ISaveable`, `SaveId = player_inventory`, dan `LoadOrder = 50`. Isinya slot berisi, `ItemInstance`, dan `nextInstanceId`. Tangan di kapal tidak disimpan.
- **Inventori penuh:** `AddItem` mengembalikan sisa. Pemanggil tetap memegang barang yang tidak muat.
- **Item inti:** `Base_Kail`, `Base_Pistol`, dan `Base_Pisau` tidak bisa dibuang, sesuai `GLOSSARY.md`.
- **Tangan penuh:** `AddShipHandItem` mengembalikan `false`, dan `ShipInteractable.CanInteract` sudah menolak lebih dulu.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** `AddItem` mengembalikan sisa (`ItemUseSystem.md`). Tangan di kapal (`LifeboatExtraction.md`). `SetMaxSlots` (`CarryCapacity.md`).

**Usulan yang menunggu developer (dicatat di #85):** nama `FindFirstInstance` untuk kebutuhan #110.

## 6. Skenario Tes

1. Tambah 30 kayu dengan tumpukan 20, dan satu slot kosong. **Hasil:** sisa 10 dikembalikan.
2. Panggil `GetCount("Mat_Wood")` dengan 3 tumpukan. **Hasil:** total benar.
3. Tiba di pulau. **Hasil:** item inti ada, dan barang kapal masuk sekali.
4. Turunkan kapasitas saat slot penuh. **Hasil:** sisa jatuh sebagai `WorldItem`, dan notifikasi tampil.
5. Ambil barang kapal melebihi kapasitas tangan. **Hasil:** ditolak, dan `ShipHandChangedEvent` tidak terkirim.
6. Setor ke sekoci. **Hasil:** `ClearShipHand` mengosongkan semuanya.
7. Simpan, keluar, lalu lanjutkan. **Hasil:** isi slot dan `nextInstanceId` sama.
8. Diterkam tanpa Pisau di tangan. **Hasil:** `FindFirstInstance("Base_Pisau")` tetap mengembalikan Pisau.
