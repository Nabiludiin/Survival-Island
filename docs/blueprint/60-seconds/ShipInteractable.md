# Blueprint: ShipInteractable.cs
**Lokasi:** `docs/blueprint/60-seconds/ShipInteractable.md`
**Deskripsi Utama:** Barang dan NPC yang bisa diambil di kapal. Mengikuti `IInteractable` (#17), menghitung berat dari data, dan kembali ke pool setelah diambil.
**Issue:** #17, #18, #74, #104, #118
**Rujukan GDD:** Bagian 3 (Fase 1: mengambil peluru, makanan kaleng, dan menyelamatkan NPC). Berat dan kapasitas tangan adalah usulan.

## 1. Arsitektur & Variabel Utama

`public class ShipInteractable : MonoBehaviour, IInteractable, IPoolable`, memakai pool `ShipLoot_Generic`.

| Variabel | Tipe | Fungsi |
|---|---|---|
| `ItemID` | `string` | ID item atau NPC |
| `kind` | `ShipInteractableKind` | `Item` atau `NPC` |
| `weightCost` | `int` | Dibaca dari `ItemData.shipWeight` untuk barang, dan `NPCData` untuk NPC |
| `pickupAmount` | `int` | Dibaca dari `ItemData.shipPickupAmount` |
| `highlight` | `InteractHighlight` | Menggantikan penambahan material |

**Dihapus:** `outlineMaterial`, `originalMaterials`, `UIManager.ShowTooltip`, `UIManager.HideTooltip`, dan `Destroy(gameObject)`.

## 2. Detail Logika & Fungsi

| Anggota `IInteractable` | Isi |
|---|---|
| `IsAvailable` | Objek masih aktif dan belum diambil |
| `HoldSeconds` | 0 |
| `CanInteract(in InteractContext ctx)` | `HandsFull` kalau `ShipHandWeight + weightCost > ShipHandCapacity` |
| `GetPrompt()` | Dari kunci `prompt.format` dan `prompt.grab` lewat `LocalizationManager`. Token `{input:Interact}` memakai `InputManager.Instance.GetBindingDisplay(GameInputAction.Interact)` |

**`public bool Interact(in InteractContext ctx)`**:

1. Kalau `CanInteract` menolak, kembalikan `false`.
2. `ctx.Inventory.AddShipHandItem(ItemID, weightCost, pickupAmount)`.
3. Mainkan partikel dan suara lewat pool.
4. `ObjectPooler.Instance.ReturnToPool("ShipLoot_Generic", gameObject)`, lalu kembalikan `true`.

## 3. Hubungan dengan Sistem Lain

**Event:** `ShipHandChangedEvent` dikirim lewat `InventoryManager`.

| Sistem | Hubungan |
|---|---|
| `PlayerInteractor` (#17) | Memanggil `CanInteract` dan `Interact` |
| `InventoryManager` (#74) | `AddShipHandItem` |
| `LootSpawnerSystem` | Memunculkan objek dari pool |
| `LocalizationManager` (#104), `InputManager` (#18) | Teks prompt |
| `ItemData`, `NPCData` (#118, #76) | Berat dan jumlah |

## 4. Kasus Khusus

- **Objek pool:** `OnSpawned` mengisi data dari `ItemID`. `OnReturned` mengembalikan collider, renderer, dan highlight.
- **Game dijeda:** interaksi mati, karena `Gameplay` mati.
- **Permadeath:** objek kembali ke pool lewat `ReturnAllActive`.
- **Save dan load:** Fase 1 tidak punya save (#82).
- **Tangan penuh:** `InteractHighlight` memakai warna `Blocked`.
- **Tombol diganti:** teks prompt mengikuti `InputBindingsChangedEvent`.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** `Interact(in InteractContext)` dan `HandsFull` (`IslandInteractable.md`, `LifeboatExtraction.md`). Pool (`LifeboatExtraction.md`). Sumber berat (`CarryCapacity.md`). Kunci teks (`LocalizationManager.md`).

**Menunggu developer:** tidak ada yang menghalangi.

## 6. Skenario Tes

1. Arahkan ke makanan kaleng. **Hasil:** prompt menampilkan tombol aktif, dan highlight `Available`.
2. Tekan `E`. **Hasil:** berat bertambah, dan objek kembali ke pool.
3. Tangan penuh. **Hasil:** highlight `Blocked`, dan barang tetap ada.
4. Ganti `Interact` ke `T`. **Hasil:** prompt menampilkan `T`.
5. Ganti bahasa. **Hasil:** teks prompt berubah.
6. Pakai objek dari pool berulang kali. **Hasil:** tidak ada data dari barang sebelumnya.
