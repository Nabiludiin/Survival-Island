# Blueprint: TechTreeManager.cs
**Lokasi:** `docs/blueprint/Core_Logic/TechTreeManager.md`
**Deskripsi Utama:** Sistem progresi yang mengunci dan membuka resep, bangunan, dan upgrade tingkat lanjut lewat node. Sistem ini hanya menyimpan status node. Pemilik resep dan bangunan membaca statusnya sendiri.
**Issue:** #34, #77, #80
**Rujukan GDD:** Usulan, belum ada di GDD. Blueprint Jembatan dari Diary Bab 2 (GDD bagian 4) tidak dikelola sistem ini.

## 1. Arsitektur & Variabel Utama

### Pola

`TechTreeManager` adalah singleton per scene di `IslandPhase`. Tidak persisten, dan tidak menjadi `ISaveable` sendiri. Statusnya disimpan `CraftingManager` di entri `world_recipes` (`CraftingManager.md`).

Data node dipisah dari status. Aset `TechNodeData` hanya berisi aturan, dan status terbuka berada di `HashSet<string>` milik manager. Dengan begitu aset tidak pernah berubah saat bermain, dan reset run tidak perlu mengembalikan aset.

**Diganti:** kelas `TechNode { nodeID, isUnlocked, requiredNodeID }` dan `List<TechNode> techTree`. `isUnlocked` di dalam data membuat status sulit direset dan disimpan.

### Data

```csharp
public enum TechUnlockTrigger { Manual, ItemCrafted, StructureBuilt }

[CreateAssetMenu(menuName = "Survival/Tech Node")]
public class TechNodeData : ScriptableObject
{
    public string nodeId;               // Contoh: node_stone_upgrade
    public string displayNameKey;       // Kunci teks untuk #104
    public string requiredNodeId;       // Kosong kalau tanpa prasyarat
    public TechUnlockTrigger trigger;   // Cara node terbuka
    public string triggerKey;           // recipeId untuk ItemCrafted, buildableId untuk StructureBuilt
}
```

`TechUnlockTrigger` dan `TechNodeData` adalah nama baru dan perlu dicatat di `GLOSSARY.md`.

### Variabel

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Instance` | `TechTreeManager` | `null` | Singleton per scene |
| `nodes` | `TechNodeData[]` | Dari Inspector | Semua node |
| `nodesById` | `Dictionary<string, TechNodeData>` | Dibangun di `Awake` | Pencarian cepat |
| `unlocked` | `HashSet<string>` | Kosong | Node yang sudah terbuka |
| `unlockClip` | `AudioClip` | Dari Inspector | Suara saat node terbuka |

Tidak ada angka penyetelan. Tidak ada yang perlu masuk `GameBalanceConfig`.

### Isi pohon (usulan, menunggu developer)

Semua resep inti di `CraftingStation.md` memakai `startsUnlocked = true`, jadi pada versi awal tidak ada resep inti yang memakai node. Node hanya dipakai bangunan yang sudah menyebut `TechTreeManager`:

| `nodeId` | Prasyarat | Pemicu | Yang dibuka | Sumber |
|---|---|---|---|---|
| `node_stone_upgrade` | Tidak ada | `ItemCrafted`, `rec_hammer` | Upgrade dinding dan pintu batu (`BuildingIntegrity.md`) | Contoh di issue #34: dinding batu terbuka setelah membuat palu |
| `node_barbed_wire` | `node_stone_upgrade` | `StructureBuilt`, `Build_SpikeTrap` | `Build_BarbedWire` (`BaseDefense.md`) | Usulan |

Sampai tabel ini disetujui, `unlockNodeId` pada `BuildableData` tetap kosong, sesuai keputusan #85 di `BuildingIntegrity.md`. Dengan `unlockNodeId` kosong, bangunan terbuka sejak awal.

**Bukan isi sistem ini:** Blueprint Jembatan. Statusnya hanya ada di `RegionGateSystem.hasBridgeBlueprint`, tersimpan di `world_gates`, dan diisi saat `DiaryChapterUnlockedEvent` Bab 2 (#68). Bentuk ini ditulis di `RegionGateSystem.md` dan `ObjectiveManager.md`. Catatan: tabel #68 di `docs/USULAN_TERBUKA.md` masih menulis node `node_bridge_blueprint` di sistem ini. Kepala file itu menyatakan blueprint yang diikuti kalau berbeda, jadi blueprint ini mengikuti `RegionGateSystem.md`. Karena itu `TechUnlockTrigger` tidak punya pilihan Diary.

## 2. Detail Logika & Fungsi

**`void Awake()`**: isi `Instance`, lalu bangun `nodesById`. ID kosong atau ganda dicatat sebagai kesalahan, dan node kedua diabaikan.

**`void OnEnable()` dan `OnDisable()`**: berlangganan dan melepas `ItemCraftedEvent` dan `StructureBuiltEvent`, sesuai `GameEventBus.md`. Langganan hanya dipasang kalau ada node dengan pemicu itu.

**`public bool IsUnlocked(string nodeId)`**:

1. Kalau `nodeId` kosong atau `null`, kembalikan `true`. Kosong berarti tanpa syarat, sesuai `BuildMenu.md`.
2. Kalau `nodesById` tidak berisi `nodeId`, catat peringatan sekali per ID, lalu kembalikan `false`.
3. Kembalikan `unlocked.Contains(nodeId)`.

Dipakai `CraftingStation.md`, `BuildMenu.md`, `BuildingIntegrity.md`, dan `BaseDefense.md`.

**`public bool CanUnlock(string nodeId)`**: `true` kalau node dikenal, belum terbuka, dan `IsUnlocked(requiredNodeId)`. Dipakai panel untuk aturan "redup kalau prasyarat sudah terbuka" di `CraftingStation.md`.

**`public bool UnlockNode(string nodeId)`**:

1. Cari node. Kembalikan `false` kalau tidak ada atau sudah terbuka.
2. Kalau `!IsUnlocked(requiredNodeId)`, kembalikan `false`. Pemicu tidak disimpan, jadi pemain perlu mengulang pemicunya setelah prasyarat terbuka.
3. `unlocked.Add(nodeId)`.
4. `AudioManager.Instance.PlaySFX(unlockClip)`.
5. Terbitkan `TechNodeUnlockedEvent { nodeId }`, lalu kembalikan `true`.

Notifikasi `Resep baru` dibuat `CraftingManager` dari event ini, bukan oleh sistem ini.

**`private void HandleItemCrafted(ItemCraftedEvent e)`**: untuk tiap node dengan `trigger == ItemCrafted` dan `triggerKey == e.recipeId`, panggil `UnlockNode`.

**`private void HandleStructureBuilt(StructureBuiltEvent e)`**: sama, dengan `trigger == StructureBuilt` dan `triggerKey == e.buildableId`.

Daftar node kecil, jadi pencarian linear cukup dan tidak menghasilkan garbage.

**`public string[] GetUnlockedIds()`**: salin `unlocked` ke array yang diurutkan, supaya isi save stabil. Dipanggil `CraftingManager.CaptureState`.

**`public void RestoreUnlocked(string[] ids)`**, dipanggil `CraftingManager.RestoreState`:

1. Kosongkan `unlocked`.
2. Tambahkan tiap ID yang ada di `nodesById`. ID tidak dikenal dibuang dengan peringatan.
3. Prasyarat tidak diperiksa ulang, karena save hanya berisi node yang lolos `UnlockNode`.
4. Tidak mengirim event dan tidak memainkan suara.

### Validasi Editor

`OnValidate` atau menu Editor memeriksa:

- `nodeId` unik dan tidak kosong.
- `requiredNodeId` kosong atau merujuk node yang ada.
- Tidak ada rantai prasyarat yang berputar.
- `triggerKey` ada di `RecipeDatabase` untuk `ItemCrafted`, atau di daftar `BuildableData` untuk `StructureBuilt`.
- Tiap `techNodeId` di `CraftingRecipe` dan `unlockNodeId` di `BuildableData` merujuk node yang ada.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `TechNodeUnlockedEvent { nodeId }`, tidak sticky (`GameEventBus.md`).

**Event yang didengar:** `ItemCraftedEvent` (`CraftingManager`) dan `StructureBuiltEvent` (`BuildManager`), hanya untuk node berpemicu.

| Sistem | Hubungan |
|---|---|
| `CraftingManager` (#77) | Membaca `IsUnlocked`, mendengar `TechNodeUnlockedEvent`, dan menyimpan status lewat `GetUnlockedIds` dan `RestoreUnlocked` di `world_recipes` |
| `BuildMenu` (#79), `BuildingIntegrity` (#47), `BaseDefense` (#26) | Membaca `IsUnlocked` lewat `unlockNodeId` |
| `BuildManager` (#79) | Sumber `StructureBuiltEvent` |
| `AudioManager` | `PlaySFX` |
| `WorldStateSave` (#80) | Tidak langsung. Lewat `CraftingManager` |
| `RegionGateSystem` (#68) | Tidak ada hubungan. Blueprint Jembatan milik sistem itu |

## 4. Kasus Khusus

- **Objek pool:** tidak memakai pool.
- **Game dijeda:** tidak ada logika per frame. Pemicu hanya datang dari crafting dan bangunan, yang ikut berhenti saat dijeda.
- **Permadeath:** manager hilang bersama `IslandPhase` saat reset (#82). Run baru mulai dengan `unlocked` kosong. Aset `TechNodeData` tidak berubah, jadi tidak perlu direset.
- **Save dan load:** masuk `world_recipes` (`LoadOrder = 110`) lewat `CraftingManager`, sesuai `WorldStateSave.md`. `TechNodeUnlockedEvent` **tidak** dikirim ulang saat load, karena event ini tidak sticky dan setiap pengiriman memunculkan notifikasi `Resep baru`. Pembaca memanggil `IsUnlocked` saat panel atau menu dibuka.
- **ID tidak dikenal:** `IsUnlocked` mengembalikan `false`, dengan peringatan sekali.
- **Node dihapus di versi baru:** ID di save yang tidak lagi ada dibuang saat `RestoreUnlocked`.
- **Node baru di versi baru:** terkunci di save lama. Pemain membukanya lewat pemicu biasa.
- **Pemicu terjadi sebelum prasyarat terbuka:** diabaikan. Pemicu yang dipakai pada tabel usulan bisa diulang, jadi pemain tidak terkunci.
- **Satu pemicu membuka dua node berantai:** urutan `nodes` menentukan. Node anak yang diperiksa lebih dulu gagal, lalu terbuka pada pemicu berikutnya. Validasi Editor menyarankan prasyarat ditaruh lebih dulu di array.
- **Run tanpa node terbuka:** semua resep inti tetap bisa dibuat, karena memakai `startsUnlocked`.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** `IsUnlocked` (`CraftingStation.md`). Penyimpanan di `world_recipes` (`WorldStateSave.md`). `unlockNodeId` kosong berarti terbuka (`BuildMenu.md`). Upgrade batu lewat sistem ini, dengan `unlockNodeId` kosong sampai node disetujui (`BuildingIntegrity.md`, keputusan #85). Blueprint Jembatan sebagai penanda di `RegionGateSystem` (`RegionGateSystem.md`, `ObjectiveManager.md`).

**Keputusan teknis di blueprint ini:**

- Data node dan status dipisah.
- Event tidak dikirim ulang saat load.
- Pemicu dibaca dari event yang sudah ada, tanpa pemanggilan langsung dari sistem lain.

**Menunggu developer:**

1. **Isi pohon:** tabel node di bagian 1 masih usulan. Pemicu `node_stone_upgrade` mengikuti contoh issue #34. Pemicu `node_barbed_wire` adalah ide baru.
2. **Notifikasi untuk node bangunan:** node yang tidak membuka resep tidak punya notifikasi. Usulan: `BuildMenu` menampilkan `Bangunan baru` dari `TechNodeUnlockedEvent`.
3. **Ketidaksesuaian di `USULAN_TERBUKA.md` #68:** baris "Bentuk Blueprint Jembatan" perlu diperbarui agar sama dengan `RegionGateSystem.md`.

## 6. Skenario Tes

1. Buka node tanpa prasyarat lewat `UnlockNode`. **Hasil:** `IsUnlocked` bernilai `true`, event terkirim sekali, dan suara terdengar.
2. Panggil `UnlockNode` dua kali untuk node yang sama. **Hasil:** panggilan kedua mengembalikan `false`, tanpa event.
3. Buka node dengan prasyarat terkunci. **Hasil:** ditolak, tanpa event.
4. Panggil `IsUnlocked("")`. **Hasil:** `true`.
5. Panggil `IsUnlocked("node_tidak_ada")` dua kali. **Hasil:** `false`, dengan satu peringatan.
6. Isi tabel usulan, lalu buat `rec_hammer`. **Hasil:** `node_stone_upgrade` terbuka, dan upgrade batu muncul di menu.
7. Bangun `Build_SpikeTrap` sebelum `node_stone_upgrade` terbuka. **Hasil:** `node_barbed_wire` tetap terkunci. Buat palu, lalu bangun jebakan lagi. **Hasil:** `node_barbed_wire` terbuka.
8. Temukan buku Diary, dan tunggu Bab 2 terbuka. **Hasil:** tidak ada node yang berubah, `RegionGateSystem.HasBridgeBlueprint` bernilai `true`, dan jembatan bisa dibangun.
9. Buka satu node, simpan, keluar, lalu lanjutkan. **Hasil:** status node sama, dan tidak ada notifikasi atau suara saat dimuat.
10. Ubah satu ID di `world_recipes` menjadi ID yang tidak ada, lalu lanjutkan. **Hasil:** ID itu dibuang dengan peringatan, node lain tetap.
11. Buka Pause selama 10 detik setelah job `rec_hammer` dimulai. **Hasil:** node baru terbuka setelah job selesai, bukan saat dijeda.
12. Mati, lalu mulai run baru. **Hasil:** semua node terkunci.
13. Buat dua node yang saling menjadi prasyarat, lalu jalankan validasi Editor. **Hasil:** rantai berputar dilaporkan.
