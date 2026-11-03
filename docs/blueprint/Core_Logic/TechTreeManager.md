# Blueprint: TechTreeManager.cs
**Lokasi:** `docs/blueprint/Core_Logic/TechTreeManager.md`
**Deskripsi Utama:** Sistem progresi yang mengunci dan membuka resep tingkat lanjut.
**Issue:** #34, #77, #80
**Rujukan GDD:** Usulan, belum ada di GDD. Blueprint Jembatan dari Diary Bab 2 (GDD bagian 4) tidak dikelola sistem ini.

## 1. Arsitektur & Variabel Utama

```csharp
[Serializable]
public class TechNode { public string nodeID; public bool isUnlocked; public string requiredNodeID; }
```

| Variabel | Tipe | Fungsi |
|---|---|---|
| `techTree` | `List<TechNode>` | Semua node |

**Bukan isi sistem ini:** Blueprint Jembatan. Statusnya hanya ada di `RegionGateSystem.hasBridgeBlueprint`, tersimpan di `world_gates`, dan diisi saat `DiaryChapterFoundEvent` Bab 2 (#68). Bentuk ini masih usulan di `RegionGateSystem.md` dan `ObjectiveManager.md`, dan menunggu konfirmasi developer.

## 2. Detail Logika & Fungsi

**`public bool IsUnlocked(string nodeId)`**: mengembalikan status node. Dipakai `CraftingStation.md` dan `BuildMenu.md`.

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
| `BuildMenu` (#79), `BuildingIntegrity` (#47), `BaseDefense` (#26) | Membaca `IsUnlocked` lewat `unlockNodeId` |

## 4. Kasus Khusus

- **Objek pool:** tidak memakai pool.
- **Game dijeda:** tidak ada logika per frame.
- **Permadeath:** semua node kembali terkunci.
- **Save dan load:** masuk `world_recipes` (`LoadOrder = 110`) bersama `CraftingManager`, sesuai `WorldStateSave.md`. Event dikirim ulang saat `OnRestoreFinished`.
- **ID tidak dikenal:** `IsUnlocked` mengembalikan `false`, dengan peringatan.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** `IsUnlocked` (`CraftingStation.md`), dan penyimpanan (`WorldStateSave.md`).

**Menunggu developer:** isi pohon teknologi masih usulan. Bentuk Blueprint Jembatan sebagai penanda di `RegionGateSystem` juga menunggu konfirmasi.

## 6. Skenario Tes

1. Buka node tanpa prasyarat. **Hasil:** `IsUnlocked` bernilai `true`, dan event terkirim sekali.
2. Buka node dengan prasyarat terkunci. **Hasil:** ditolak.
3. Temukan Diary Bab 2. **Hasil:** tidak ada node yang berubah, `RegionGateSystem.HasBridgeBlueprint` bernilai `true`, dan jembatan bisa dibangun.
4. Simpan, keluar, lalu lanjutkan. **Hasil:** status node sama.
