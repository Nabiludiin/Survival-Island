# Blueprint: TechTreeManager.cs
**Lokasi:** `docs/blueprint/Core_Logic/TechTreeManager.md`
**Deskripsi Utama:** Sistem progresi yang mengunci dan membuka resep tingkat lanjut, serta syarat cerita seperti blueprint jembatan.
**Issue:** #34, #68, #77, #80
**Rujukan GDD:** Bagian 4 dan 5 (Diary Bab 2 memberi Blueprint Jembatan). Pohon lainnya adalah usulan.

## 1. Arsitektur & Variabel Utama

```csharp
[Serializable]
public class TechNode { public string nodeID; public bool isUnlocked; public string requiredNodeID; }
```

| Variabel | Tipe | Fungsi |
|---|---|---|
| `techTree` | `List<TechNode>` | Semua node |

**Node wajib:** `node_bridge_blueprint`, tanpa `requiredNodeID`. `RegionGateSystem.HasBridgeBlueprint` membaca node ini (#68).

## 2. Detail Logika & Fungsi

**`public bool IsUnlocked(string nodeId)`**: mengembalikan status node. Dipakai `CraftingStation.md`, `ObjectiveManager.md`, dan #68.

**`public void UnlockNode(string id)`**:

1. Cari node. Keluar kalau tidak ada atau sudah terbuka.
2. Periksa `requiredNodeID`.
3. `isUnlocked = true`, mainkan suara, lalu terbitkan `TechNodeUnlockedEvent { nodeId }`.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `TechNodeUnlockedEvent`.

**Event yang didengar:** tidak ada.

| Sistem | Hubungan |
|---|---|
| `CraftingManager` (#77) | Membaca `IsUnlocked`, dan menyimpan status lewat `world_recipes` |
| `RegionGateSystem` (#68) | Membaca `node_bridge_blueprint` |
| `ObjectiveManager` (#81) | Membuka node dari hadiah Diary |

## 4. Kasus Khusus

- **Objek pool:** tidak memakai pool.
- **Game dijeda:** tidak ada logika per frame.
- **Permadeath:** semua node kembali terkunci.
- **Save dan load:** masuk `world_recipes` (`LoadOrder = 110`) bersama `CraftingManager`, sesuai `WorldStateSave.md`. Event dikirim ulang saat `OnRestoreFinished`.
- **ID tidak dikenal:** `IsUnlocked` mengembalikan `false`, dengan peringatan.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** `IsUnlocked` (`CraftingStation.md`), dan penyimpanan (`WorldStateSave.md`).

**Menunggu developer:** isi pohon teknologi di luar `node_bridge_blueprint` masih usulan.

## 6. Skenario Tes

1. Buka node tanpa prasyarat. **Hasil:** `IsUnlocked` bernilai `true`, dan event terkirim sekali.
2. Buka node dengan prasyarat terkunci. **Hasil:** ditolak.
3. Temukan Diary Bab 2. **Hasil:** `node_bridge_blueprint` terbuka, dan jembatan bisa dibangun.
4. Simpan, keluar, lalu lanjutkan. **Hasil:** status node sama.
