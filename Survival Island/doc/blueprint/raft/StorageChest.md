# Blueprint: StorageChest.cs
**Lokasi:** `Survival Island/doc/blueprint/raft/StorageChest.md`
**Deskripsi Utama:** Peti penyimpanan buatan pemain, dengan isi terpisah dari inventori, panel dua sisi, dan pemindahan cepat lewat klik kanan.
**Issue:** #46
**Rujukan GDD:** Bagian 4 (fungsi base). GDD tidak menyebut peti. Sistem ini mendukung pengelolaan sumber daya di bagian 1, dan jumlah slot serta aturannya adalah usulan.

## 1. Arsitektur & Variabel Utama

### Pola

`StorageChest : IslandInteractable` berada di prefab `Chest_Wood`, dan dibangun lewat `BuildableData` (#79). Tiap peti menyimpan array `InventorySlot` sendiri, memakai bentuk data yang sama dengan `InventoryManager` (#66).

| Kelas | Jenis | Tugas |
|---|---|---|
| `StorageChest` | Komponen di prefab | Isi satu peti |
| `StoragePanel` | `UIPanel`, id `Storage` | Panel kiri (inventori) dan kanan (peti). Satu instance di `IslandPhase` |
| `SlotArrayOps` | Kelas statis | Menambah, menggabung, dan menghitung isi array slot. Dipakai peti dan `InventoryManager` |
| `SlotGridView` | Komponen UI | Menggambar satu array slot. Dipakai panel kiri dan kanan |

### Variabel `StorageChest`

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `slots` | `InventorySlot[]` | Panjang `chestSlotCount`, semua kosong | Isi peti, terpisah dari inventori |
| `ContentsChanged` | `event Action` | Kosong | Event per objek untuk panel yang sedang terbuka |
| `IsOpen` | `bool` | `false` | Sedang dibuka panel |
| `lidAnimator` | `Animator` | Dari prefab | Tutup peti |

### Fungsi `SlotArrayOps`

| Fungsi | Kegunaan |
|---|---|
| `int Add(InventorySlot[] slots, ItemInstance source, int amount)` | Mengisi tumpukan sejenis, lalu slot kosong. Mengembalikan sisa |
| `int CountItem(InventorySlot[] slots, string itemId)` | Menghitung jumlah |
| `bool CanMerge(InventorySlot slot, ItemInstance source)` | Sama `itemId`, belum penuh, dan bukan item unik |

Isi `Add` sama dengan `InventoryManager.AddItem(ItemInstance, int)`, termasuk rata-rata berbobot `createdGameHour` saat menggabung tumpukan (#66). `InventoryManager` memanggil kelas ini, supaya logikanya tidak ditulis dua kali.

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.storage.chestSlotCount` | 20 | Slot per peti |
| `GameBalanceConfig.storage.dropScatterRadius` | 1,2 | Sebaran isi saat peti hancur |

Biaya dan HP `Chest_Wood` ada di `raft/BuildMenu.md`: 20 `Mat_Wood`, 150 HP.

## 2. Detail Logika & Fungsi

### Membuka peti

Tombol `E` lewat `PlayerInteractor`, dengan `HoldSeconds = 0`. Collider interaksi berada di anak prefab pada layer `Interactable`, dan collider utama di `Structure` (#79).

**`protected override bool OnInteract(in InteractContext ctx)`**:

1. `StoragePanel.Instance.Bind(this)`.
2. `UIFocusStack.Instance.Push(StoragePanel.Instance)`. Kalau ditolak, batalkan `Bind`, lalu kembalikan `false`.
3. `IsOpen = true`, lalu putar animasi tutup terbuka.

`blockedWhileCarrying` tetap `true`, jadi peti tidak bisa dibuka saat membawa Heavy Item (`IslandInteractable.md`).

Panel tidak menjeda waktu, sesuai `UIFocusStack.md`. Pemain berhenti bergerak dan tetap bisa diserang.

### Panel dua sisi

```text
+------------------+------------------+
|  INVENTORI       |  PETI            |
|  SlotGridView    |  SlotGridView    |
|  InventoryManager|  StorageChest    |
|  .slots          |  .slots          |
+------------------+------------------+
```

**`StoragePanel`** tidak mendorong panel `Inventory`. Panel ini memakai `SlotGridView` yang sama untuk kedua sisi.

**Sinkronisasi:** UI tidak mengubah data secara langsung, dan hanya menggambar ulang setelah data berubah.

| Sumber perubahan | Sinyal | Tindakan |
|---|---|---|
| `InventoryManager` | `InventoryChangedEvent` | `leftDirty = true` |
| `StorageChest` | `ContentsChanged` | `rightDirty = true` |
| Makanan berubah kesegaran | `InventoryChangedEvent`, dan `ContentsChanged` dari pemeriksaan `HourChangedEvent` | Sama |

`LateUpdate` menggambar ulang sisi yang kotor sekali per frame. Pemindahan banyak tumpukan tetap hanya satu kali gambar.

**`Show()`** berlangganan `InventoryChangedEvent` dan `ContentsChanged`, lalu menggambar kedua sisi. **`Hide()`** melepas keduanya.

### Memindahkan item

**Klik kanan** dibaca lewat `UI/RightClick` (#18). Saat `StoragePanel` terbuka, klik kanan menjadi pemindahan cepat. Menu kecil `InventoryPanel` (#66) tidak muncul di panel ini.

**`private void QuickTransfer(SlotSide side, int index)`**:

| `side` | Sumber | Tujuan |
|---|---|---|
| `Left` | `InventoryManager` | `StorageChest` |
| `Right` | `StorageChest` | `InventoryManager` |

1. Ambil slot sumber. Kalau kosong, selesai.
2. Kalau `side == Left`, panggil `CanStore(item)`. Kalau gagal, tampilkan notifikasi, lalu selesai.
3. Hitung `sisa = tujuan.Add(slot.instance, slot.amount)`.
4. `pindah = slot.amount - sisa`. Kalau `pindah == 0`, tampilkan `Peti penuh` atau `Inventori penuh`, lalu selesai.
5. Kurangi sumber sebesar `pindah`. Untuk inventori, lewat `InventoryManager.RemoveFromSlot`.
6. Kirim sinyal perubahan.

**Item yang tidak boleh masuk peti**, `bool CanStore(ItemData item)`:

| Item | Hasil | Alasan |
|---|---|---|
| `isCoreItem == true` | Ditolak | Item inti selalu kembali ke slot (`GLOSSARY.md`) |
| `isDroppable == false` | Ditolak | Mengikuti aturan #66 dan #116. Item wajib tetap terpantau |
| Item lain | Boleh | Tidak ada |

**Seret dan lepas** memakai satu `DragState` di `StoragePanel`:

| Aksi | Hasil |
|---|---|
| Seret antar-slot pada sisi yang sama | Menukar isi slot. Inventori lewat `MoveSlot`, peti lewat `SwapSlots` |
| Seret ke sisi lain, slot kosong atau sejenis | Pindah atau gabung |
| Seret ke sisi lain, slot berisi item berbeda | Menukar kedua isi, kalau `CanStore` lolos |
| Lepas di luar panel | Kembali ke slot asal. Membuang item hanya lewat panel `Inventory` |

**`OnClosed(UICloseReason reason)`**: kembalikan barang yang masih diseret ke slot asal, `chest.IsOpen = false`, dan tutup animasi. Berlaku juga saat pemain mati atau diterkam (#83).

Item unik seperti senjata memakai `ItemInstance` yang sama saat pindah, jadi durabilitas dan isi magazine tidak berubah. Rujukan hotbar lepas lewat `InventoryChangedEvent` (#60).

### Pembusukan

Isi peti tetap membusuk, karena `createdGameHour` tidak berubah dan peti tidak memberi perlambatan (`ItemUseSystem.md`). `StorageChest` mendengar `HourChangedEvent` hanya saat `IsOpen`, lalu mengirim `ContentsChanged`. Saat peti tertutup, kesegaran dihitung ketika item dipakai.

### Peti hancur atau dibongkar

**Dua jalur memakai fungsi yang sama, `private void SpillContents()`**, dipanggil dari `BaseHealth.Died` (peti hancur) dan `StructureRegistry.TryDemolish` (dibongkar):

1. Kalau `IsOpen`, panggil `UIFocusStack.Instance.Close(StoragePanel.Instance, UICloseReason.Forced)`.
2. Untuk tiap slot berisi, panggil `WorldItemManager.Spawn(instance, amount, pos, despawns: false)`, dengan `pos` acak dalam `dropScatterRadius` dari aliran tampilan, lalu dinaikkan ke tanah lewat `PhysicsQuery.Raycast` dan `PhysicsMasks.Ground`.
3. Kosongkan `slots`.

`despawns: false` membuat barang tidak hilang karena batas waktu dan tidak terkena `maxLooseWorldItems` (#66). Dengan 20 slot per peti, jumlah objek masih aman. Pemain tetap bisa memungutnya lewat `E`.

### Simpan dan muat

`JsonUtility` tidak mempertahankan `null` pada kelas `[Serializable]`, jadi slot kosong tidak boleh disimpan apa adanya. Yang ditulis hanya slot berisi:

```csharp
[Serializable]
public struct ChestSlotRecord
{
    public int slotIndex;
    public int amount;
    public ItemInstance instance;
}

[Serializable]
public class StorageChestSaveData
{
    public int slotCount;
    public ChestSlotRecord[] items;
}
```

`StorageChest` juga mengganti `CaptureState` dan `RestoreState` bawaan `IslandInteractable`:

| Jenis peti | Jalur penyimpanan |
|---|---|
| Peti buatan pemain | `StructureRegistry` menulis JSON ini ke `StructureRecord.extraJson` (#79), dengan `SaveId` dari `structureId` |
| Peti yang ditempatkan di region (kalau ada) | `RegionStateStore` (#88), dengan `saveId` stabil dari #80 |

**`RestoreState(string json)`**:

1. Buat `slots` sepanjang `chestSlotCount`.
2. Untuk tiap rekaman, cari `ItemDatabase.Get(itemId)`. Kalau tidak ada, lewati dan catat peringatan (#66).
3. Kalau `slotIndex >= slots.Length`, masukkan lewat `SlotArrayOps.Add`. Kalau tetap tidak muat, keluarkan sebagai `WorldItem`.
4. Tumpukan di atas `maxStack` dipecah (#118).

`instanceId` tetap memakai nomor lama. `InventoryManager.nextInstanceId` dipulihkan lebih dulu (#80), jadi nomor baru tidak bentrok.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** tidak ada event baru. `InventoryChangedEvent` tetap dikirim `InventoryManager` saat barang keluar atau masuk inventori.

**Event yang didengar:** `InventoryChangedEvent` (`StoragePanel`), `HourChangedEvent` (`StorageChest`, saat terbuka).

| Sistem | Hubungan |
|---|---|
| `IslandInteractable` (#17) | Kelas dasar, dengan `PlayerInteractor` |
| `UIFocusStack` (#83) | `StoragePanel` mewarisi `UIPanel`, dengan id `Storage` |
| `InputManager` (#18) | `UI/RightClick` |
| `InventoryManager`, `ItemUseSystem` (#66) | `InventorySlot`, `ItemInstance`, `AddItem`, `RemoveFromSlot`, dan `MoveSlot` |
| `WorldItemManager` (#66) | Mengeluarkan isi |
| `EquipmentManager` (#60) | Rujukan hotbar lepas saat barang pindah |
| `BuildMenu`, `StructureRegistry` (#79) | Membangun, membongkar, dan menyimpan |
| `BaseHealth` (#59) | `Died` memicu `SpillContents` |
| `WorldStateSave` (#80), `RegionStreaming` (#88) | Jalur penyimpanan |
| `CarryCapacity` (#118) | Tas tidak mengubah slot peti |
| `NotificationManager` (#22) | Pesan penolakan |

**Batas pekerjaan:** inventori pemain di #66 dan #118, bangunan di #79, dan peti loot sekali buka di `IslandInteractable.md`.

## 4. Kasus Khusus

- **Objek pool:** peti tidak memakai pool. `WorldItem` hasil tumpahan memakai pool milik #66, dan status `OnSpawned` diatur di sana.
- **Game dijeda:** panel `Storage` tidak menjeda, tetapi panel lain seperti `Pause` bisa menumpuk di atasnya. `Time.deltaTime` bernilai 0, tetapi klik UI tetap diproses oleh `StoragePanel` yang tidak lagi menjadi puncak. Karena itu, `QuickTransfer` memeriksa `UIFocusStack.Instance.TopPanel == this`.
- **Permadeath:** isi peti berada di save run (#80), dan terhapus bersamanya (#82). Isi peti tidak dibawa ke run baru.
- **Save dan load:** disimpan isi slot lewat jalur di atas. Tidak disimpan: `IsOpen`, `DragState`, dan posisi tutup. Run selalu dimuat dengan panel tertutup.
- **Autosave saat panel terbuka:** `CaptureState` membaca `slots` apa adanya. Barang yang sedang diseret belum dipindahkan datanya sampai dilepas, jadi tidak berlipat atau hilang.
- **Serangan saat panel terbuka:** panel tetap terbuka (`UIFocusStack.md`). Saat diterkam, panel tertutup lewat `CloseAllUnlocked`.
- **Peti hancur saat dibuka:** panel tertutup dulu, lalu isi tumpah.
- **Region dilepas dengan peti di dalamnya:** peti buatan pemain berada di `structureRoot` yang nonaktif (#88). Data tetap di memori, dan tetap tersimpan.
- **Base rusak saat ditinggal (#90):** peti hancur lewat `TakeDamage` pada objek nonaktif. `SpillContents` memakai data item ke `RegionStateStore`, karena `WorldItem` tidak bisa muncul di region yang tidak dimuat. Saat region kembali, `WorldItemManager.RestoreRegion` memunculkannya.
- **Dua sumber mengubah inventori saat panel terbuka:** misalnya makanan habis lewat #66. Panel tetap benar karena `InventoryChangedEvent`.
- **Klik kanan pada slot saat barang diseret:** diabaikan.
- **Slot peti berubah di versi baru:** langkah 3 pada `RestoreState` menjaga barang.
- **Peti kosong dibongkar:** hanya pengembalian bahan (#79).

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** panel `Storage` tidak menjeda game (`UIFocusStack.md`). Klik kanan pada `UI/RightClick` untuk pemindahan cepat (`InputManager.md`). Peti memakai `InventorySlot` dan `ItemInstance` (`ItemUseSystem.md`). Peti tidak terpengaruh tas (`CarryCapacity.md`).

**Keputusan teknis di blueprint ini:**

- `SlotArrayOps` dipakai bersama, supaya aturan tumpukan hanya ada di satu tempat.
- Panel menggambar ulang lewat tanda kotor, sekali per frame.
- Hanya slot berisi yang disimpan, karena batas `JsonUtility`.

**Usulan yang disetujui developer sebagai nilai awal (dicatat di #85):**

1. **Jumlah slot:** 20 per peti, sedikit lebih besar dari 15 slot inventori.
2. **Item inti dan item wajib:** tidak boleh masuk peti.
3. **Peti hancur:** isi tumpah sebagai `WorldItem` yang tidak hilang otomatis. Pilihan lain adalah isinya ikut hancur, tetapi itu terlalu menghukum saat base diserang.
4. **Makanan di peti:** tetap membusuk dengan kecepatan normal.
5. **Klik kanan:** memindahkan seluruh tumpukan. Memindahkan sebagian memakai seret dan pisah di panel `Inventory`.
6. **Zombie mengabaikan peti:** peti tidak bertag `BaseWall`, jadi hanya rusak karena ledakan, api, bencana, atau runtuhan.

## 6. Skenario Tes

1. Bangun peti di lantai, lalu tekan `E`. **Hasil:** panel `Storage` terbuka dengan inventori di kiri dan peti di kanan, dan waktu tetap berjalan.
2. Klik kanan tumpukan 30 kayu di kiri. **Hasil:** seluruh tumpukan pindah ke kanan.
3. Klik kanan tumpukan itu di kanan. **Hasil:** kembali ke kiri, dan menggabung dengan tumpukan yang ada.
4. Pindahkan kayu ke peti yang hampir penuh. **Hasil:** sebagian pindah, dan sisanya tetap di inventori.
5. Klik kanan `Base_Pistol`. **Hasil:** ditolak dengan notifikasi. Hal yang sama berlaku untuk item `KeyItem`.
6. Pindahkan Kapak yang sedang aktif ke peti. **Hasil:** Kapak hilang dari tangan, rujukan hotbar kosong, dan durabilitasnya tetap sama saat diambil lagi.
7. Seret item ke slot berisi item lain di sisi seberang. **Hasil:** kedua item bertukar.
8. Seret item, lalu turunkan HP ke 0. **Hasil:** barang kembali ke slot, panel tertutup, dan `Cinematic` aktif.
9. Isi peti, simpan, keluar, lalu lanjutkan. **Hasil:** isi, jumlah, durabilitas, dan isi magazine sama.
10. Tutup paksa game sesudah autosave, lalu lanjutkan. **Hasil:** tidak ada barang berlipat.
11. Buat dua peti dengan isi berbeda. **Hasil:** isi tidak tercampur.
12. Masukkan daging segar, tunggu sampai busuk lewat `settime`, lalu buka peti. **Hasil:** warna kesegaran berubah.
13. Hancurkan peti berisi 10 tumpukan lewat `DevConsole`. **Hasil:** 10 `WorldItem` muncul, dan tidak hilang setelah 2 hari game.
14. Bongkar peti berisi barang. **Hasil:** barang keluar, dan bahan peti kembali sesuai #79.
15. Hancurkan peti saat panel terbuka. **Hasil:** panel tertutup tanpa error.
16. Buka peti saat membawa Heavy Item. **Hasil:** ditolak dengan `Tangan penuh`.
17. Mati, lalu mulai run baru. **Hasil:** peti dan isinya tidak ada.

## 7. Catatan Implementasi (Langkah 3)

`SlotArrayOps` ditulis Langkah 3 (`Survival Island/Assets/LOGIC/raft/SlotArrayOps.cs`), karena `InventoryManager` memakainya. Parameter memakai `IList<InventorySlot>`, jadi array peti dan `List` inventori sama-sama diterima. Parameter opsional `Func<int> allocateId` memberi `instanceId` baru saat tumpukan terbagi ke beberapa slot. `IsUnique(ItemInstance, ItemData)` publik.

## 8. Catatan Implementasi (Sistem pendukung)

- File: `Assets/LOGIC/raft/StorageChest.cs` (`ChestSlotRecord`, `StorageChestSaveData`, `StorageChest`), `StoragePanel.cs` (dengan `SlotSide`), `SlotGridView.cs`, dan `SlotGridViewCell.cs` (komponen sel, file sendiri karena aturan nama file komponen Unity).
- `SpillContents` dipanggil dari `StructureDestroyedEvent` milik peti ini. Jalur hancur (`TakeDamage`) dan bongkar (`BaseHealth.Remove`) sama-sama menerbitkan event itu, jadi TODO #46 di `StructureRegistry.TryDemolish` sudah tercakup.
- Peti nonaktif di region yang dilepas tidak mendengar event, jadi tumpahan ke `RegionStateStore` (#88, #90) masih TODO.
- `InventoryManager` tidak punya fungsi mengisi slot tertentu. Pindah ke slot inventori tertentu dan tukar antar-sisi memakai `AddItem` lalu `MoveSlot`.
- `IslandInteractable` (#17) belum ada, jadi menerapkan `IInteractable` langsung.
