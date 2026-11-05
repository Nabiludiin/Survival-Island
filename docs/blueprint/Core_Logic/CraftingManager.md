# Blueprint: CraftingManager.cs
**Lokasi:** `docs/blueprint/Core_Logic/CraftingManager.md`
**Deskripsi Utama:** Catatan peralihan. Isi lama digantikan `Core_Logic/CraftingStation.md`. `CraftingManager` tetap menjadi manager antrean crafting, dengan isi baru. File ini merangkum kontrak `CraftingManager` untuk Langkah 4 di `CLAUDE.md`, lalu menunjuk ke bagian rinci di `CraftingStation.md`.
**Issue:** #77, #80
**Rujukan GDD:** Bagian 3 (formula vaksin dan NPC yang mempercepat crafting) dan bagian 4 (meja crafting di base). Jenis stasiun, antrean, dan angka resep adalah usulan yang sudah disetujui sebagai nilai awal di `CraftingStation.md`.

## 1. Arsitektur & Variabel Utama

**Aturan baca:** kalau file ini berbeda dengan `CraftingStation.md`, ikuti `CraftingStation.md`. File ini tidak menambah aturan baru.

### Isi lama dan penggantinya

| Isi lama | Pengganti | Bagian di `CraftingStation.md` |
|---|---|---|
| `unlockedRecipes` | `IsRecipeUnlocked(r)`, dihitung dari `startsUnlocked` dan `TechTreeManager` | "Tech tree" |
| `craftingUIPanel` | `CraftingPanel : UIPanel` (`UIPanelId.Crafting`) | "Panel resep" |
| `TryCraft()` | `RequestCraft(r)` yang mengembalikan `CraftResult` | "Memulai crafting" |
| Crafting langsung jadi | Antrean `CraftJob` sampai `maxQueue` | "Menjalankan antrean" |
| Bahan dihapus sebelum ruang dicek | `CanFitAfterRemoval` lebih dulu, baru `RemoveItem` | "Memulai crafting", langkah 5 dan 6 |

### Letak dan pola

| Hal | Nilai |
|---|---|
| Letak | Manager di scene `IslandPhase`. Tidak persisten, dan hilang bersama scene saat reset (#82) |
| Pola | Singleton per scene (`Instance`) |
| Save | `ISaveable`, `SaveId = world_recipes`, `LoadOrder = 110` (`WorldStateSave.md`) |
| Data | `CraftingRecipe` (ScriptableObject), dicari lewat `RecipeDatabase` |
| Enum | `CraftingStationType`, `CraftResult` (`GLOSSARY.md`) |

Variabel (`queue`, `stations`, `availableMask`, `npcSpeedMultiplier`, `hitPauseTimer`) dan nilai awalnya ada di `CraftingStation.md` bagian "Variabel `CraftingManager`".

### Angka penyetelan

Semua angka sudah berada di `GameBalanceConfig.crafting` (`maxQueue` 5, `stationRangeMeters` 4, `leaveGraceMeters` 2, `stationCheckSeconds` 0,5, `hitPauseSeconds` 1,5, `npcCrafterSpeedMultiplier` 1,5). Waktu dan bahan tiap resep berada di aset `CraftingRecipe`. Tidak ada angka di file ini yang perlu dipindah.

## 2. Detail Logika & Fungsi

### Kontrak publik

| Fungsi atau properti | Kegunaan | Dipanggil oleh |
|---|---|---|
| `CraftResult RequestCraft(CraftingRecipe r)` | Memasukkan job ke antrean | `CraftingPanel` |
| `void CancelJob(int index)` | Membatalkan job dan mengembalikan bahan | `CraftingPanel` |
| `bool IsRecipeUnlocked(CraftingRecipe r)` | Status resep | `CraftingPanel`, `CampfireManager` (#44) |
| `int availableMask` (baca saja) | Jenis stasiun dalam jangkauan | `ToolRepairSystem` (#113) |
| `void SetNpcSpeedMultiplier(float m)` | Pengali kecepatan dari NPC tukang | `NPCRoster` (#76) |
| `void RegisterStation(CraftingStation s)` / `UnregisterStation(...)` | Daftar stasiun aktif | `CraftingStation.OnEnable` / `OnDisable` |
| `IReadOnlyList<CraftJob> Queue` | Isi antrean untuk tampilan | `CraftingPanel`. `HUDManager` cukup mendengar `CraftQueueChangedEvent` |

Nama `RegisterStation`, `UnregisterStation`, dan `Queue` adalah usulan nama untuk perilaku yang sudah tertulis di `CraftingStation.md` ("didaftarkan di `OnEnable`" dan daftar antrean di panel).

Langkah tiap fungsi ada di `CraftingStation.md` bagian 2. Bagian di bawah hanya merangkum yang menyangkut save dan tech tree, karena dua hal itu disebut di Tahap 7.

### Data save `world_recipes`

```csharp
[Serializable]
public class CraftingSaveData
{
    public string[] unlockedNodeIds;   // Dari TechTreeManager.GetUnlockedIds()
    public CraftJobSave[] queue;       // Urutan sama dengan antrean
}

[Serializable]
public struct CraftJobSave { public string recipeId; public float remainingSeconds; }
```

**`CaptureState()`**:

1. Isi `unlockedNodeIds` dari `TechTreeManager.Instance.GetUnlockedIds()`.
2. Salin `queue` menjadi array `CraftJobSave`.
3. Kembalikan `JsonUtility.ToJson`. Tidak ada `Dictionary`, sesuai aturan `WorldStateSave.md`.

**`RestoreState(string json)`**, tanpa mengirim event:

1. Urai JSON. Kalau gagal, catat kesalahan dan pakai nilai awal (antrean kosong, semua node terkunci).
2. `TechTreeManager.Instance.RestoreUnlocked(unlockedNodeIds)`.
3. Untuk tiap job, cari `RecipeDatabase.Get(recipeId)`. Job dengan resep yang tidak ditemukan dibuang dengan peringatan. Bahan tidak dikembalikan, karena sudah dipotong saat job dibuat.
4. Potong antrean ke `maxQueue` kalau nilai config turun setelah pembaruan. Job yang terpotong dibuang dengan peringatan.

**`OnRestoreFinished()`**:

1. Hitung ulang `availableMask` lewat `RefreshAvailability()`.
2. Terbitkan `CraftQueueChangedEvent` sekali, supaya ikon antrean di HUD benar.
3. Jangan menerbitkan `ItemCraftedEvent` atau `TechNodeUnlockedEvent`. Keduanya event sesaat, dan pengiriman ulang akan memunculkan notifikasi palsu.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `ItemCraftedEvent { recipeId }`, `CraftQueueChangedEvent { queueCount, activeRecipeId }` (`GameEventBus.md`).

**Event yang didengar:** `TechNodeUnlockedEvent`, `PlayerDamagedEvent`, dan `PlayerDiedEvent`. `CraftingPanel` mendengar `InventoryChangedEvent` hanya saat terbuka.

| Sistem | Hubungan | Rincian |
|---|---|---|
| `CraftingStation.md` (#77) | Sumber semua aturan | Bagian 1 sampai 6 |
| `TechTreeManager` (#34) | `IsUnlocked`, `GetUnlockedIds`, `RestoreUnlocked` | `TechTreeManager.md` |
| `InventoryManager`, `ItemUseSystem` (#66) | `HasItem`, `RemoveItem`, `AddItem`, `CanFitAfterRemoval` | `ItemUseSystem.md` |
| `WorldItemManager` (#66) | Hasil dan bahan yang tidak muat | `ItemUseSystem.md` |
| `WorldStateSave` (#80) | Menyimpan `world_recipes` | Tabel "Isi data" dan urutan 11 |
| `ToolRepairSystem` (#113) | Membaca `availableMask`, dan tab Perbaiki di `CraftingPanel` | `ToolRepairSystem.md` |
| `CampfireManager` (#44) | `CookingRecipe` dan `RecipeDatabase` | `raft/CampfireManager.md` |
| `NPCRoster` (#76) | `SetNpcSpeedMultiplier` | `CraftingStation.md` bagian 3 |
| `UIFocusStack` (#83) | Panel `Crafting` tidak menjeda game | `UIFocusStack.md` |

## 4. Kasus Khusus

Rincian ada di `CraftingStation.md` bagian 4. Ringkasan untuk pemeriksaan cepat:

- **Objek pool:** manager tidak memakai pool. Hasil yang jatuh memakai `WorldItem_Generic`, dengan reset dari #66.
- **Game dijeda:** antrean memakai `Time.deltaTime`, jadi berhenti saat `Pause`. Panel `Crafting` sendiri tidak menjeda.
- **Permadeath:** antrean dan node hilang bersama scene dan save (#82). Run baru mulai dengan antrean kosong dan hanya resep `startsUnlocked`.
- **Save dan load:** bentuk data di bagian 2. Bahan tidak disimpan, jadi tidak bisa terpotong dua kali.
- **`world_recipes` tidak ada di save lama:** manager memakai nilai awal, sesuai aturan "Sistem belum ada pada versi awal" di `WorldStateSave.md`.
- **`TechTreeManager` belum aktif saat `RestoreState`:** tidak terjadi, karena keduanya berada di `IslandPhase`, dan `RestoreBaseScene` berjalan setelah scene aktif. Kalau `TechTreeManager.Instance` tetap `null`, catat kesalahan dan lewati langkah 2.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** penggantian isi dan semua aturan crafting (`CraftingStation.md`). Tabel dan urutan save (`WorldStateSave.md`). Panel tanpa jeda (`UIFocusStack.md`).

**Keputusan teknis di file ini:**

- `CraftingManager` memegang entri save untuk node tech tree, karena `world_recipes` adalah satu entri untuk keduanya. `TechTreeManager` tidak menjadi `ISaveable` sendiri.
- `OnRestoreFinished` hanya mengirim `CraftQueueChangedEvent`, bukan event sesaat lain.

**Menunggu developer:** tidak ada yang menghalangi. Nama file ini dipertahankan karena `CLAUDE.md` Tahap 7 memakainya sebagai blueprint utama Langkah 4.

## 6. Skenario Tes

Ikuti 16 skenario di `CraftingStation.md` bagian 6. Tambahan khusus save dan tech tree:

1. Buka satu node lewat `TechTreeManager.UnlockNode`, antre 2 job, simpan, keluar, lalu lanjutkan. **Hasil:** node tetap terbuka, antrean dan sisa waktu sama, dan tidak ada notifikasi `Resep baru` saat dimuat.
2. Ubah `recipeId` satu job di file save menjadi ID yang tidak ada, lalu lanjutkan. **Hasil:** job itu dibuang dengan peringatan, job lain tetap.
3. Hapus entri `world_recipes` dari file save, lalu lanjutkan. **Hasil:** antrean kosong, resep awal tetap terbuka, tanpa error.
4. Lanjutkan run dengan 1 job di antrean. **Hasil:** ikon antrean di HUD langsung tampil.
5. Mati, lalu mulai run baru. **Hasil:** `GetUnlockedIds()` kosong, dan antrean kosong.
