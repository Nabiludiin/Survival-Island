# Blueprint: ShipInteractable.cs
**Lokasi:** `Survival Island/doc/blueprint/60-seconds/ShipInteractable.md`
**Deskripsi Utama:** Barang dan NPC yang bisa diambil di kapal. Mengikuti `IInteractable` (#17), menghitung berat dari data, dan kembali ke pool setelah diambil.
**Issue:** #17, #18, #74, #104, #118
**Rujukan GDD:** Bagian 3 (Fase 1: mengambil peluru, makanan kaleng, vaksin, dan menyelamatkan NPC). Berat, kapasitas tangan, dan wujud NPC di kapal adalah usulan yang sudah disetujui sebagai nilai awal (#85).

## 1. Arsitektur & Variabel Utama

### Pola

`public class ShipInteractable : MonoBehaviour, IInteractable, IPoolable`.

Satu kelas dipakai untuk barang dan NPC. Perbedaannya hanya pada `kind` dan sumber data. Objek tidak pernah dibuat dengan `Instantiate`, dan tidak pernah dihancurkan dengan `Destroy`.

| Wujud | Pool | Pemanggil `SpawnFromPool` | Isi prefab |
|---|---|---|---|
| Barang | `ShipLoot_Generic` | `LootSpawnerSystem` di `ShipPhase` | `MeshFilter`, `MeshRenderer`, `BoxCollider`, `InteractHighlight`, dan komponen ini |
| NPC | `ShipNpc_Generic` (usulan) | `LifeboatExtraction.SpawnShipNpcs()` (#74) | Tubuh NPC sederhana dengan `Animator` diam panik, `CapsuleCollider`, `InteractHighlight`, dan komponen ini. **Tanpa** `NavMeshAgent` dan tanpa `NPCCompanion` |

**Alasan pool NPC terpisah (usulan):** `NPCData.prefab` berisi `NPCCompanion` dan FSM pulau, jadi tidak cocok untuk kapal. NPC di kapal hanya target ambil yang diam, sesuai `LifeboatExtraction.md` ("tidak ada agen tambahan"). Pilihan lain adalah meletakkan tiga NPC langsung di scene lalu menyalakan yang terpilih, tetapi posisinya berasal dari seed (#89), jadi pool lebih rapi.

### Layer dan collider

| Hal | Aturan | Alasan |
|---|---|---|
| Layer | `Interactable` untuk barang dan NPC | Masuk mask `Interact` (`PhysicsLayers.md`). NPC kapal **tidak** memakai layer `NPC`, supaya tidak masuk mask `AIAttack`, `Shoot`, dan `Melee` |
| `isTrigger` | `false` | `PhysicsQuery` mengabaikan trigger (#87). Collider trigger tidak akan pernah terbidik |
| Tabrakan | Layer `Interactable` hanya bertabrakan dengan `Terrain`, `Structure`, dan `Obstacle` | Pemain dan zombie bisa menembus barang, jadi barang tidak menghalangi rute kabur |
| `Rigidbody` | Tidak ada | Barang diam di tempat. Tidak ada biaya fisika |
| Ukuran `BoxCollider` | Mengikuti `worldMesh.bounds`, ditambah `ship.pickupColliderPadding` | Barang kecil tetap mudah dibidik |

### Variabel

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `ItemID` | `string` | Kosong | `ItemData.itemID` atau `NPCData.npcId`. Dipakai `AddShipHandItem` dan `InteractedEvent` |
| `kind` | `ShipInteractableKind` | `Item` | `Item` atau `NPC` (`GLOSSARY.md`) |
| `weightCost` | `int` | 0 | `ItemData.shipWeight` untuk barang, dan `NPCData.weight` untuk NPC |
| `pickupAmount` | `int` | 0 | `ItemData.shipPickupAmount` untuk barang. Selalu 1 untuk NPC |
| `poolTag` | `string` | Dari prefab | `ShipLoot_Generic` atau `ShipNpc_Generic`, untuk `ReturnToPool` |
| `itemData` | `ItemData` | `null` | Data barang |
| `npcData` | `NPCData` | `null` | Data NPC |
| `isTaken` | `bool` | `false` | Sudah diambil. Menolak `Interact` kedua pada frame yang sama |
| `isHovered` | `bool` | `false` | Sedang dibidik `PlayerInteractor` |
| `promptNormal`, `promptFull` | `string` | `null` | Teks prompt yang sudah jadi, supaya tidak membuat string tiap frame |
| `highlight` | `InteractHighlight` | Dari prefab | Outline `Off`, `Available`, `Blocked` (#17) |
| `meshFilter`, `meshRenderer`, `boxCollider` | Komponen | Dari prefab | Tampilan barang. Diisi ulang tiap `Setup` |
| `placeholderMesh` | `Mesh` | Kubus | Dipakai kalau `ItemData.worldMesh` kosong |

**Dihapus:** `outlineMaterial`, `originalMaterials`, `UIManager.ShowTooltip`, `UIManager.HideTooltip`, `Interact(InventoryManager inv)`, dan `Destroy(gameObject)`.

### Berat dan jumlah per barang kapal

Sumber angka: `ItemData.shipWeight` dan `ItemData.shipPickupAmount` (#118). Daftar ini mengikuti isi `Loot_Ship` di `RegionLootTable.md` dan aturan konversi di `CarryCapacity.md`. Daftar barang final tetap di #103.

| Barang | `itemID` | `shipWeight` | `shipPickupAmount` | Catatan |
|---|---|---|---|---|
| Makanan kaleng | `Food_Can` | 1 | 2 | GDD bagian 3 |
| Peluru pistol | `Ammo_9mm` | 1 | 12 | GDD bagian 3 |
| Air Minum Bersih | `Drink_CleanWater` | 1 | 2 | |
| Perban | `Med_Bandage` | 1 | 1 | |
| Kotak obat | `Med_Medkit` | 1 | 1 | Langka |
| Vaksin | `Med_Vaccine` | 1 | 1 | Langka. **Keputusan developer:** tetap ada di kapal, dan dihitung sebagai stok awal |
| Baterai | `Item_Battery` | 1 | 1 | |
| Bahan, seperti kain dan tali | `Mat_*` | 1 | 4 | Kalau ditambahkan ke `Loot_Ship` nanti |
| Senjata atau alat besar | Sesuai item | 2 | 1 | Belum ada di `Loot_Ship`. Aturan berat tetap disiapkan |
| NPC | `npc_*` | Dari `NPCData.weight` | 1 | Semua NPC 4, jadi membawa satu NPC memenuhi tangan (`NPCRoster.md`) |

Item inti (`Base_Kail`, `Base_Pistol`, `Base_Pisau`) **tidak pernah** muncul sebagai `ShipInteractable`. Ketiganya selalu ditambahkan saat tiba di pulau (`IslandArrival.Apply`, #74), sesuai GDD bagian 3.

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.ship.handCapacity` | 4 | Sudah ada di #74. Dibaca lewat `InventoryManager.ShipHandCapacity` |
| `GameBalanceConfig.ship.pickupColliderPadding` | 0,05 | Tambahan ukuran collider barang, dalam meter (usulan) |
| `GameBalanceConfig.shipHazard.waterHeadDepth` | 1,6 | Sudah ada di #75. NPC sedalam ini tidak bisa diambil |

## 2. Detail Logika & Fungsi

### Pengisian data

`ObjectPooler.SpawnFromPool` memanggil `OnSpawned()` **sebelum** pemanggil tahu barang apa yang dimunculkan. Jadi data diisi lewat `Setup`, yang dipanggil tepat setelah `SpawnFromPool`.

**`public void Setup(ItemData item)`**, dipanggil `LootSpawnerSystem` tepat setelah `SpawnFromPool` (tabel wujud barang per scene di `LootSpawnerSystem.md`):

1. Kalau `item == null`, catat peringatan, lalu `ReturnToPool(poolTag, gameObject)` dan selesai.
2. `kind = Item`, `itemData = item`, `ItemID = item.itemID`.
3. `weightCost = Mathf.Max(1, item.shipWeight)`, dan `pickupAmount = Mathf.Max(1, item.shipPickupAmount)`.
4. `meshFilter.sharedMesh = item.worldMesh != null ? item.worldMesh : placeholderMesh`, dan `meshRenderer.sharedMaterial = item.worldMaterial`.
5. Atur `boxCollider.center` dan `boxCollider.size` dari `sharedMesh.bounds`, ditambah `pickupColliderPadding` di tiap sisi.
6. Panggil `BuildPromptCache()`.

**`public void Setup(NPCData npc)`**, dipanggil `LifeboatExtraction.SpawnShipNpcs()`:

1. Kalau `npc == null`, catat peringatan, kembalikan ke pool, lalu selesai.
2. `kind = NPC`, `npcData = npc`, `ItemID = npc.npcId`.
3. `weightCost = Mathf.Max(1, npc.weight)`, dan `pickupAmount = 1`.
4. Panggil `BuildPromptCache()`.

**`private void BuildPromptCache()`**:

```csharp
var loc = LocalizationManager.Instance;
string name = loc.Get(kind == ShipInteractableKind.NPC ? npcData.displayNameKey : itemData.displayNameKey);
promptNormal = loc.Format(kind == ShipInteractableKind.NPC ? "prompt.rescue" : "prompt.grab", "name", name);
promptFull = loc.Get("prompt.handsFull");
```

`GetPrompt` hanya mengembalikan teks aksi. Nama tombol ditambahkan `PlayerInteractor.RefreshPrompt` lewat `prompt.format` (`[{input:Interact}] {action}`), sesuai `IslandInteractable.md`. Jadi `ShipInteractable` **tidak** memanggil `GetBindingDisplay` sendiri.

### Anggota `IInteractable`

| Anggota | Isi |
|---|---|
| `IsAvailable` | `!isTaken && gameObject.activeInHierarchy && !IsSubmergedNpc()` |
| `HoldSeconds` | 0. Barang dan NPC diambil langsung |
| `GetPrompt()` | `WouldFit() ? promptNormal : promptFull` |
| `CanInteract(in InteractContext ctx)` | Lihat di bawah |
| `OnHoverEnter()` | `isHovered = true`, lalu `RefreshHighlight()` |
| `OnHoverExit()` | `isHovered = false`, lalu `highlight` ke `Off` |
| `Interact(in InteractContext ctx)` | Lihat di bawah |

**`public InteractBlockReason CanInteract(in InteractContext ctx)`**, urutan pemeriksaan:

1. Kalau `!IsAvailable`, kembalikan `Locked`.
2. Kalau `LifeboatExtraction.Instance == null` atau `LifeboatExtraction.Instance.State != ShipFlowState.Scavenge`, kembalikan `Locked`. Ini menutup celah pada frame waktu habis atau saat berangkat.
3. Kalau `ctx.Inventory.ShipHandWeight + weightCost > ctx.Inventory.ShipHandCapacity`, kembalikan `HandsFull`.
4. Selain itu, kembalikan `None`.

**`public bool Interact(in InteractContext ctx)`**:

1. Kalau `CanInteract(ctx) != None`, kembalikan `false`.
2. Kalau `!ctx.Inventory.AddShipHandItem(ItemID, weightCost, pickupAmount)`, kembalikan `false`. `InventoryManager` menolak sendiri kalau tangan penuh, jadi tidak ada barang yang hilang.
3. `isTaken = true`.
4. Efek:
   - Barang: `VFXManager.Instance.Play("VFX_ShipPickup", transform.position, Quaternion.identity)` dan `AudioManager.Instance.PlaySFX(pickupClip)`.
   - NPC: `AudioManager.Instance.PlaySFX(npcGrabClip)`. Tidak ada partikel.
5. `ObjectPooler.Instance.ReturnToPool(poolTag, gameObject)`. NPC yang dibawa hilang dari dunia, lalu HUD menampilkan ikonnya lewat `ShipHandChangedEvent` (#74).
6. Kembalikan `true`. `PlayerInteractor` lalu menerbitkan `InteractedEvent { targetId = ItemID }`.

`NPCRescuedEvent` **tidak** dikirim di sini. Event itu terkirim saat NPC disetor ke sekoci (`LifeboatExtraction.DepositHand`).

### Highlight

**`private bool WouldFit()`**:

```csharp
var inv = InventoryManager.Instance;
return inv != null && inv.ShipHandWeight + weightCost <= inv.ShipHandCapacity;
```

**`private void RefreshHighlight()`**: kalau `!isHovered`, selesai. Kalau `WouldFit()`, pakai `Available`. Kalau tidak, pakai `Blocked`. Warna `Blocked` berarti barang tidak muat, sesuai `LifeboatExtraction.md`.

**`private void HandleShipHandChanged(ShipHandChangedEvent e)`**: panggil `RefreshHighlight()`. Jadi saat pemain sedang membidik barang lalu menyetor dari jauh, warnanya ikut berubah. Event ini sticky, tetapi `OnEnable` tidak perlu membaca `Last`, karena highlight baru dihitung saat `OnHoverEnter`.

### NPC terendam

**`private bool IsSubmergedNpc()`**:

```csharp
if (kind != ShipInteractableKind.NPC) return false;
var hazard = ShipHazardManager.Instance;
return hazard != null && hazard.WaterLevel - transform.position.y >= balance.shipHazard.waterHeadDepth;
```

Aturan ini milik `ShipHazardManager.md` (kasus khusus "NPC di area yang banjir"). Barang yang terendam tetap bisa diambil, karena pemain yang turun ke sana sudah menanggung risiko tenggelam. Pemeriksaan ini hanya berjalan saat `PlayerInteractor` memindai, 20 kali per detik, jadi murah.

### Berlangganan event

```csharp
void OnEnable()  { EventChannel<ShipHandChangedEvent>.Raised += HandleShipHandChanged; EventChannel<LocaleChangedEvent>.Raised += HandleLocaleChanged; }
void OnDisable() { EventChannel<ShipHandChangedEvent>.Raised -= HandleShipHandChanged; EventChannel<LocaleChangedEvent>.Raised -= HandleLocaleChanged; }
```

**`HandleLocaleChanged`**: kalau sudah di-`Setup`, panggil `BuildPromptCache()`. Pergantian tombol tidak perlu didengar di sini, karena nama tombol ditambahkan `PlayerInteractor`.

### Reset pool

**`public void OnSpawned()`**:

1. `isTaken = false`, dan `isHovered = false`.
2. `highlight` ke `Off`.
3. `boxCollider.enabled = true`, dan `meshRenderer.enabled = true`, atau collider dan renderer NPC.
4. `transform.localScale = Vector3.one`.

**`public void OnReturned()`**:

1. `itemData = null`, `npcData = null`, `ItemID = string.Empty`, `weightCost = 0`, dan `pickupAmount = 0`.
2. `promptNormal = null`, dan `promptFull = null`.
3. `isHovered = false`, dan `highlight` ke `Off`.
4. `meshFilter.sharedMesh = null`. Collider kembali ke ukuran prefab.
5. NPC: `Animator.Rebind()`, supaya pose panik mulai dari awal.

Objek pool berada di bawah `PersistentRoot`, jadi kelas ini **tidak** menyimpan referensi ke objek scene. `LifeboatExtraction`, `ShipHazardManager`, dan `InventoryManager` selalu dibaca lewat `Instance` saat dibutuhkan.

### Pemeriksaan data di Editor (usulan)

`ShipLootAudit`, menu Editor, memeriksa semua `ItemData` di `Loot_Ship` dan semua `NPCData` di daftar #76:

- `shipWeight` dan `NPCData.weight` berada di antara 1 dan `ship.handCapacity`. Kalau lebih besar, barang itu tidak akan pernah bisa diambil.
- `shipPickupAmount` minimal 1.
- `worldMesh` tidak kosong. Kalau kosong, kubus pengganti tampil di game.
- `displayNameKey` ada di tabel teks (#104).

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** tidak ada secara langsung. `ShipHandChangedEvent` dikirim `InventoryManager`, dan `InteractedEvent` dikirim `PlayerInteractor`.

**Event yang didengar:** `ShipHandChangedEvent`, `LocaleChangedEvent`.

| Sistem | Hubungan |
|---|---|
| `PlayerInteractor` (#17) | Memanggil `IsAvailable`, `GetPrompt`, `CanInteract`, `OnHoverEnter`, `OnHoverExit`, dan `Interact` |
| `InteractHighlight` (#17) | Status `Off`, `Available`, dan `Blocked` |
| `InventoryManager` (#74) | `AddShipHandItem`, `ShipHandWeight`, dan `ShipHandCapacity` |
| `LifeboatExtraction` (#74) | Membaca `State`. `SpawnShipNpcs` memanggil `Setup(NPCData)` |
| `LootSpawnerSystem` | Memunculkan barang dari pool, lalu memanggil `Setup(ItemData)` |
| `ShipHazardManager` (#75) | `WaterLevel` untuk NPC terendam |
| `ObjectPooler` | Pool `ShipLoot_Generic` dan `ShipNpc_Generic` |
| `VFXManager` (#15), `AudioManager` | Efek dan suara ambil |
| `LocalizationManager` (#104) | `prompt.grab`, `prompt.rescue`, `prompt.handsFull`, dan nama item |
| `ItemData`, `NPCData` (#118, #76) | Berat, jumlah, dan tampilan |
| `PhysicsLayers` (#87) | Layer `Interactable` dan collider bukan trigger |

## 4. Kasus Khusus

- **Objek pool:** `OnSpawned` hanya mengembalikan status. Data diisi `Setup`. `OnReturned` membersihkan data, highlight, collider, renderer, dan teks, supaya barang berikutnya tidak membawa sisa barang sebelumnya.
- **Game dijeda:** `Gameplay` mati dan `PlayerInteractor` membersihkan target, jadi `OnHoverExit` terpanggil dan highlight mati. Kelas ini tidak punya logika per frame.
- **Permadeath:** `RunLifecycle` memanggil `ObjectPooler.ReturnAllActive()` sebelum scene berganti (#82). Semua objek kembali lewat `OnReturned`. Run baru memunculkan ulang barang dari seed baru.
- **Save dan load:** Fase 1 tidak punya save (#82). `LootSpawnerSystem` di kapal selalu memakai `RollAndRecord()`, jadi `MarkTaken` tidak perlu dipanggil.
- **Tangan penuh:** highlight `Blocked`, prompt `Tangan penuh. Setor ke Sekoci`, dan barang tetap ada.
- **Dua tekanan pada frame yang sama:** `isTaken` menolak yang kedua.
- **Waktu habis pada frame yang sama dengan menekan `E`:** kalau `HandleTimeUp` berjalan lebih dulu, `State` bukan lagi `Scavenge` dan `CanInteract` menolak. Kalau `Interact` lebih dulu, barang masuk tangan, lalu ikut disetor otomatis bila pemain berada di area aman (#74).
- **Data kosong:** `Setup(null)` mengembalikan objek ke pool dengan peringatan, tanpa error.
- **`worldMesh` kosong:** kubus pengganti tampil, dan `ShipLootAudit` melaporkannya.
- **Berat lebih besar dari kapasitas:** barang selalu `Blocked`. `ShipLootAudit` menangkapnya sebelum build.
- **NPC terendam:** `IsAvailable` menjadi `false`, prompt hilang, dan NPC terhitung korban di ringkasan (#74).
- **NPC dibawa lalu pemain mati:** NPC sudah kembali ke pool. Tidak ada tubuh tertinggal.
- **Tombol diganti:** prompt mengikuti `InputBindingsChangedEvent` lewat `PlayerInteractor`. Bahasa diganti: `HandleLocaleChanged` membangun ulang teks.
- **Scene `ShipPhase` dibuang saat objek masih dibidik:** `ReturnAllActive` memanggil `OnReturned`, dan `PlayerInteractor` ikut hilang bersama pemain.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** `Interact(in InteractContext)` dan `HandsFull` (`IslandInteractable.md`, `LifeboatExtraction.md`). Pool `ShipLoot_Generic` (`LifeboatExtraction.md`, `LootSpawnerSystem.md`). Sumber berat (`CarryCapacity.md`). Kunci `prompt.format` (`LocalizationManager.md`). Tombol `E` (#85). Vaksin tetap ada di kapal sebagai loot langka (keputusan developer, #85). NPC terendam tidak bisa diambil (`ShipHazardManager.md`).

**Keputusan teknis di blueprint ini:**

- Data diisi lewat `Setup` setelah `SpawnFromPool`, bukan di `OnSpawned`.
- Barang dan NPC kapal memakai layer `Interactable`, collider bukan trigger.
- Prompt disimpan sebagai dua string jadi, dan dibangun ulang hanya saat bahasa berubah.

**Usulan baru (disetujui developer sebagai nilai awal):**

1. **Pool NPC terpisah `ShipNpc_Generic`,** berisi tubuh NPC tanpa AI, ukuran 3 sesuai jumlah NPC di #76.
2. **Kunci teks baru:** `prompt.rescue` (`Selamatkan {name}`) dan `prompt.handsFull` (`Tangan penuh. Setor ke Sekoci`).
3. **Efek ambil:** tag `VFX_ShipPickup` di katalog `VFXManager`.
4. **Alat Editor `ShipLootAudit`** untuk memeriksa berat dan tampilan barang kapal.

**Ketidaksesuaian yang perlu diputuskan developer (tidak menghalangi kode):**

- **Berat NPC (keputusan developer):** semua NPC berberat 4, sama dengan `ship.handCapacity`. Menyelamatkan satu NPC selalu memenuhi tangan, jadi pemain harus memilih antara barang dan orang. Kode tetap membaca `NPCData.weight`.
- **Jumlah barang kapal (keputusan developer):** satu kotak peluru di kapal berisi 12 butir, satu magazine penuh `Base_Pistol`. `Loot_Ship` hanya memilih jenis barang, dan jumlahnya dibaca dari `shipPickupAmount`. Karena itu jumlah di `Loot_Ship` ditulis 1.

## 6. Skenario Tes

1. Masuk `ShipPhase`, lalu arahkan ke makanan kaleng dari 2 meter. **Hasil:** prompt `[E] Ambil Makanan Kaleng`, dan highlight `Available`.
2. Tekan `E`. **Hasil:** berat tangan bertambah 1, `ShipHandItems` berisi `Food_Can` sebanyak 2, dan objek kembali ke pool.
3. Ambil 4 barang berat 1, lalu bidik barang kelima. **Hasil:** highlight `Blocked`, prompt `Tangan penuh. Setor ke Sekoci`, dan barang tetap ada setelah `E` ditekan.
4. Sambil membidik barang kelima, setor ke sekoci lewat `DevConsole` atau dari dekat. **Hasil:** warna berubah menjadi `Available` tanpa menggeser bidikan.
5. Ambil `npc_medic`. **Hasil:** tangan penuh, NPC hilang dari dunia, dan HUD menampilkan ikon NPC. `NPCRescuedEvent` belum terkirim.
6. Ambil `npc_cook`, lalu bidik satu makanan. **Hasil:** makanan `Blocked` dan tidak terambil, karena berat Koki 4 sudah memenuhi tangan (`NPCRoster.md`).
7. Biarkan air menenggelamkan NPC di dek bawah. **Hasil:** prompt tidak muncul, dan NPC tercatat di bagian Korban.
8. Bidik barang dengan collider yang diubah menjadi trigger. **Hasil:** tidak terbidik. `LayerAuditWindow` (#87) melaporkannya.
9. Bidik barang di balik dinding. **Hasil:** tidak ada prompt.
10. Ganti `Interact` ke `T`. **Hasil:** prompt menampilkan `[T]`.
11. Ganti bahasa ke `en`. **Hasil:** prompt `[E] Grab Canned Food`, tanpa keluar dari scene.
12. Tekan `E` dua kali sangat cepat pada barang yang sama. **Hasil:** hanya satu yang masuk tangan.
13. Tekan `E` tepat saat `T=0`. **Hasil:** tidak ada error, dan barang masuk tangan atau ditolak, tidak keduanya.
14. Mati di kapal lalu ulangi lima kali. **Hasil:** jumlah objek di pool tetap, dan tidak ada barang yang membawa mesh atau ID barang sebelumnya.
15. Hapus `worldMesh` sebuah barang. **Hasil:** kubus tampil, dan `ShipLootAudit` melapor.
16. Jalankan `ShipLootAudit` dengan `shipWeight` 5. **Hasil:** terlapor sebagai barang yang tidak bisa diambil.
17. Buka Profiler sambil membidik barang bergantian selama 10 detik. **Hasil:** tidak ada alokasi GC dari `GetPrompt` atau `RefreshHighlight`.
18. Tekan `Esc` saat membidik barang. **Hasil:** prompt dan highlight hilang. Tutup panel. **Hasil:** keduanya kembali.
