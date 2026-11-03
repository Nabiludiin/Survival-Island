# Blueprint: BuildMenu.cs & BuildableData.cs
**Lokasi:** `docs/blueprint/raft/BuildMenu.md`
**Deskripsi Utama:** Alur membangun lengkap saat memegang palu: memilih struktur, memutar, mengatur tinggi, membongkar, dan meruntuhkan bangunan yang kehilangan penopang.
**Issue:** #79
**Rujukan GDD:** Bagian 4 (base dan restriksi pantai) dan bagian 6 (cadangan material untuk perbaikan). Menu radial, angka biaya, dan aturan runtuh adalah usulan, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

### Pembagian kelas

| Kelas | Jenis | Tugas |
|---|---|---|
| `BuildableData` | ScriptableObject | Data satu jenis struktur |
| `BuildMenu` | `UIPanel`, id `BuildMenu` | Menu radial dan pilihan mode palu |
| `HammerEquippable` | `EquippableBehaviour` | Meneruskan klik kiri dan klik kanan palu sesuai mode |
| `BuildManager` | Manager di `IslandPhase` | Hologram, validasi, dan pembuatan struktur |
| `StructureNode` | Komponen di prefab struktur | Posisi grid dan tingkat satu struktur |
| `StructureRegistry` | Manager di `IslandPhase`, `ISaveable` | Daftar struktur, grid, grafik dukungan, runtuhan, dan bongkar |

### `BuildableData`

```csharp
public enum BuildCategory { Structure, Furniture, Defense }
public enum BuildSlotKind { Cell, Edge }        // Cell: isi sel. Edge: sisi sel, seperti dinding
public enum SupportRule { GroundOrWall, FloorOrGround, WallBelow }
public enum NavObstacleMode { None, Carve, Avoid }   // Rincian di raft/BaseNavigation.md (#78)
public enum HammerMode { Repair, Build, Upgrade, Demolish }

[Serializable] public struct ItemCost { public string itemId; public int amount; }

[CreateAssetMenu(menuName = "Survival/Buildable Data")]
public class BuildableData : ScriptableObject
{
    public string id;
    public string nameKey;                  // Kunci teks untuk #104
    public Sprite icon;
    public BuildCategory category;
    public GameObject prefab;
    public ItemCost[] cost;
    public float maxHP;                     // Dibaca BaseHealth (#59)
    public DamageResistance[] resistances;  // Mengikuti DamagePipeline.md
    public Vector3Int gridSize;             // Ukuran dalam sel. Y dalam tingkat
    public BuildSlotKind slotKind;
    public SupportRule support;
    public NavObstacleMode navMode;
    public bool isDemolishable = true;
    public string unlockNodeId;             // Kosong berarti terbuka sejak awal (TechTreeManager)
    // Field perbaikan dan upgrade ada di raft/BuildingIntegrity.md (#47)
}
```

`woodCost`, `stoneCost`, dan `structurePrefabs` di `BuildManager` dihapus. `BuildManager.buildables` berisi daftar `BuildableData`.

### Daftar struktur awal

Biaya mengikuti target base awal di `GameBalanceConfig.md`: 4 dinding, 1 pintu, 1 lantai, dan 1 atap, sekitar 80 kayu.

| ID | Nama | Slot | Biaya | HP | Dukungan | Nav | Tag |
|---|---|---|---|---|---|---|---|
| `Floor_Wood` | Lantai | `Cell` | 10 `Mat_Wood` | 200 | `GroundOrWall` | `None` | Tidak ada |
| `Wall_Wood` | Dinding | `Edge` | 12 `Mat_Wood` | 300 | `FloorOrGround` | `Carve` | `BaseWall` |
| `Door_Wood` | Pintu | `Edge` | 10 `Mat_Wood` | 200 | `FloorOrGround` | `Carve` (#78) | `BaseDoor` |
| `Roof_Wood` | Atap | `Cell` | 12 `Mat_Wood` | 150 | `WallBelow` | `None` | Tidak ada |
| `Stairs_Wood` | Tangga | `Cell` | 16 `Mat_Wood` | 200 | `FloorOrGround` | `None` | Tidak ada |
| `Bed_Basic` | Kasur (#67) | `Cell` | 20 `Mat_Wood` | 100 | `FloorOrGround` | `Avoid` | Tidak ada |
| `Chest_Wood` | Peti (#46) | `Cell` | 20 `Mat_Wood` | 150 | `FloorOrGround` | `Avoid` | Tidak ada |

Data dinding batu ada di #47. Meja crafting (#77), api unggun (#44), dan jebakan (#26) menambah `BuildableData` sendiri lewat aturan yang sama.

### Layer pada prefab struktur

Setiap prefab punya collider utama di layer `Structure`, sesuai `PhysicsLayers.md`. Objek yang bisa diinteraksi, seperti pintu, peti, dan kasur, juga punya anak dengan collider non-trigger di layer `Interactable` yang sedikit menonjol keluar. Dengan begitu, raycast `InteractBlock` tidak terhalang oleh collider `Structure` milik objek itu sendiri.

### Variabel `BuildManager`

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `buildables` | `BuildableData[]` | Dari Inspector | Semua struktur |
| `Mode` | `HammerMode` | `Repair` | Mode palu. `Repair` tidak menampilkan hologram |
| `Selected` | `BuildableData` | `null` | Struktur terpilih |
| `ghostCache` | `Dictionary<string, GameObject>` | Kosong | Hologram per ID, dibuat sekali lalu dinyalakan dan dimatikan |
| `ghostYaw` | `int` | 0 | Rotasi hologram: 0, 90, 180, atau 270 |
| `LastError` | `PlacementError` | `NoSelection` | Dibaca HUD untuk teks alasan |
| `ghostValidMat`, `ghostInvalidMat` | `Material` | Dari Inspector | Sudah ada di blueprint lama |

```csharp
public enum PlacementError
{
    None, NoSelection, OutOfRange, NoBuildZone, UnknownSurface, Beach, TooSteep,
    MaxLevel, MaxStructures, Overlap, Occupied, NoSupport, NotEnoughMaterial
}
```

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.build.gridSize` | 2,5 | Ukuran sel, dari `BuildManager.md` |
| `GameBalanceConfig.build.maxBuildDistance` | 6 | Jarak bangun, dari `BuildManager.md` |
| `GameBalanceConfig.build.levelHeight` | 2,5 | Tinggi satu tingkat, sama dengan tinggi dinding |
| `GameBalanceConfig.build.heightSnap` | 0,25 | Pembulatan tinggi lantai pertama |
| `GameBalanceConfig.build.maxFoundationHeightDelta` | 1,25 | Selisih tinggi tanah terbesar di bawah tapak |
| `GameBalanceConfig.build.groundProbeHeight` | 5 | Awal raycast pengukur tanah di atas titik bidik |
| `GameBalanceConfig.build.maxLevels` | 3 | Tingkat bangunan terbanyak |
| `GameBalanceConfig.build.maxHorizontalSpan` | 2 | Sel menjorok terjauh dari penopang |
| `GameBalanceConfig.build.maxStructures` | 400 | Batas struktur satu run |
| `GameBalanceConfig.build.overlapShrink` | 0,1 | Pengecilan kotak overlap, supaya struktur bersebelahan tetap sah |
| `GameBalanceConfig.build.maxCollapsePerFrame` | 4 | Struktur runtuh per frame |
| `GameBalanceConfig.build.maxCollapseVfxPerSecond` | 8 | Efek debu runtuhan per detik |
| `GameBalanceConfig.build.demolishHoldSeconds` | 1,2 | Lama menahan klik kiri untuk bongkar |
| `GameBalanceConfig.build.demolishRefundRatio` | 0,5 | Bagian biaya yang kembali pada HP penuh |
| `GameBalanceConfig.build.demolishBlockedAfterDamageSeconds` | 5 | Jeda bongkar setelah struktur diserang |
| `GameBalanceConfig.build.hammerDecayPerBuild` | 1 | Durabilitas palu per struktur |
| `GameBalanceConfig.build.hammerDecayPerDemolish` | 1 | Durabilitas palu per bongkar |

## 2. Detail Logika & Fungsi

### Menu radial saat memegang palu

Palu adalah `Tool_Hammer` di tabel `EquipmentManager.md`. Komponennya `HammerEquippable`.

| Input | Syarat | Hasil |
|---|---|---|
| Klik kanan | Palu aktif | Membuka `BuildMenu` |
| `B` (`OpenBuildMenu`) | Palu ada di hotbar | Berganti ke palu lewat `RequestSlot`, lalu membuka `BuildMenu` |
| `B` | Palu tidak ada di hotbar | Notifikasi `Butuh palu di hotbar` (#22) |
| Klik kiri pada sektor | Menu terbuka | Memilih, lalu menutup menu |
| Klik kanan, `B`, atau `Esc` | Menu terbuka | Menutup menu tanpa mengubah pilihan |

Menu dibuka dengan sekali klik, bukan ditahan. `UIFocusStack` mematikan Action Map `Gameplay` saat panel terbuka, jadi pelepasan klik kanan tidak bisa dibaca di map itu.

**Isi menu:**

| Cincin | Sektor |
|---|---|
| Utama | Perbaiki, Bangun Struktur, Bangun Perabot, Upgrade, Bongkar |
| Struktur | Daftar `BuildableData` dengan `category == Structure` |
| Perabot | Daftar `BuildableData` dengan `category == Furniture` atau `Defense` |

- Tengah cincin menampilkan nama, biaya, dan jumlah bahan yang dimiliki.
- Sektor dengan bahan kurang tetap bisa dipilih, tetapi berwarna redup. Hologram memberi alasan yang sama.
- Sektor dengan `unlockNodeId` yang belum terbuka disembunyikan.
- `OnFocusGained` memindahkan kursor ke tengah layar lewat `Mouse.WarpCursorPosition`.
- Waktu tetap berjalan saat menu terbuka, sesuai tabel panel di `UIFocusStack.md`.

**`public void SelectMode(HammerMode mode, BuildableData data)`** mengatur `Mode` dan `Selected`, menyalakan atau mematikan hologram, lalu menerbitkan `BuildModeChangedEvent { mode, buildableId }`. `buildableId` kosong kalau `data` bernilai `null`.

### Klik kiri palu

**`HammerEquippable.PrimaryAction(ActionPhase phase)`**:

| `Mode` | `Pressed` | `Held` | `Released` |
|---|---|---|---|
| `Build` | `BuildManager.TryExecuteBuild()` | Tidak ada | Tidak ada |
| `Repair` | `BuildingIntegrity.TryRepairSwing(target)` (#47) | Mengulang sesuai jeda ayunan | Tidak ada |
| `Upgrade` | Mulai hitung tahan (#47) | Melanjutkan hitung | Membatalkan |
| `Demolish` | Mulai hitung tahan | Melanjutkan hitung | Membatalkan |

**`HammerEquippable.OnUnequip()`** menyembunyikan hologram dan membatalkan hitung tahan, sesuai tabel pembatalan di `EquipmentManager.md`. Pilihan terakhir tetap diingat.

**Bidikan:** `StructureRegistry.TryGetAimedStructure(out StructureNode node)` memakai `PhysicsQuery.Raycast` dengan `PhysicsMasks.BuildSurface`. Hasil yang mengenai `Terrain` dianggap tidak ada target.

### Hologram

**`BuildManager.Update()`**, saat `Mode == Build`, `Selected != null`, dan `!UIFocusStack.Instance.IsGameplayInputBlocked`:

1. Baca `BuildRotate`. Kalau ditekan, `ghostYaw = (ghostYaw + 90) % 360`.
2. Panggil `UpdateGhostPosition()`.
3. `LastError = ValidatePlacement(...)`.
4. Pasang `ghostValidMat` kalau `None`, atau `ghostInvalidMat` kalau bukan. Material hanya diganti saat hasilnya berubah.
5. Kirim teks alasan ke HUD saat `LastError` berubah.

### Rotasi dan snap

**Bidang XZ:**

```csharp
float g = balance.build.gridSize;
Vector3 cell = new Vector3(Mathf.Round(hit.point.x / g) * g, 0f, Mathf.Round(hit.point.z / g) * g);
```

Grid berlaku untuk seluruh dunia, jadi semua struktur otomatis sejajar.

| `slotKind` | Posisi | Rotasi |
|---|---|---|
| `Cell` | Tengah sel | `ghostYaw` |
| `Edge`, dengan `ghostYaw` 0 atau 180 | Tengah sisi utara atau selatan sel, mana yang lebih dekat ke titik bidik | Sejajar sumbu X |
| `Edge`, dengan `ghostYaw` 90 atau 270 | Tengah sisi barat atau timur sel, mana yang lebih dekat ke titik bidik | Sejajar sumbu Z |

Kunci sisi memakai nilai kanonik, sehingga sisi utara sel A dan sisi selatan sel tetangganya adalah slot yang sama. Satu slot hanya bisa berisi satu struktur.

### Aturan tinggi

`snapPos.y = hit.point.y` di blueprint lama dihapus, karena dua lantai bersebelahan bisa punya tinggi berbeda dan bangunan bertingkat tidak punya pijakan yang pasti.

**`float ResolveHeight(Vector3 cell, in RaycastHit hit, out int level)`** memeriksa berurutan:

1. **Membidik struktur:** tinggi dasar struktur itu, ditambah `levelHeight` kalau titik kena berada di bagian atasnya. Dinding di atas lantai berada pada tingkat yang sama dengan lantai. Lantai dan atap di atas dinding berada satu tingkat di atasnya.
2. **Ada lantai tetangga:** pakai tinggi lantai itu. Baris lantai tetap rata walaupun tanahnya miring.
3. **Di tanah:** ukur tanah pada lima titik tapak dengan `PhysicsQuery.Raycast` ke `PhysicsMasks.Ground` mulai `groundProbeHeight` di atas titik bidik. Tinggi hasilnya adalah `Mathf.Round(maxY / heightSnap) * heightSnap`, dengan `level = 0`.

**Tanah miring:**

- Selisih tertinggi dan terendah melebihi `maxFoundationHeightDelta`: `PlacementError.TooSteep`.
- Selisih yang masih boleh ditutup rok kayu pada prefab lantai, yang panjangnya ke bawah 1,5 meter. Dengan begitu bangunan tidak terlihat melayang.
- Lantai tetangga yang tingginya mengambang lebih dari `maxFoundationHeightDelta` di atas tanah tetap boleh dibangun, tetapi tidak dihitung menapak tanah, dan harus memenuhi `maxHorizontalSpan`.

**Tingkat:** `level >= maxLevels` menghasilkan `PlacementError.MaxLevel`. Tangga `Stairs_Wood` menghubungkan satu tingkat ke tingkat di atasnya.

### Validasi penempatan

**`PlacementError ValidatePlacement(BuildableData data, Vector3 pos, Quaternion rot, int level)`** berhenti pada kesalahan pertama:

| No | Pemeriksaan | Kesalahan |
|---|---|---|
| 1 | Ada `Selected` | `NoSelection` |
| 2 | Jarak ke kamera `<= maxBuildDistance` | `OutOfRange` |
| 3 | Di luar `NoBuildVolume`, yaitu seam dan link (#88), `BridgeSite` (#68), serta helipad dan titik pasang (#14) | `NoBuildZone` |
| 4 | Permukaan di bawah tapak terbaca lewat `TryGetSurfaceAt`, atau tapak bertumpu pada struktur | `UnknownSurface` |
| 5 | `SurfaceDetection.Instance.IsBeachFootprint(...)` bernilai `false` | `Beach` |
| 6 | Selisih tinggi tanah masih boleh | `TooSteep` |
| 7 | `level < maxLevels` | `MaxLevel` |
| 8 | `StructureRegistry.Count < maxStructures` | `MaxStructures` |
| 9 | `PhysicsQuery.OverlapBox` dengan `PhysicsMasks.BuildOverlap` kosong | `Occupied` kalau mengenai `Player`, `Enemy`, atau `NPC`. `Overlap` kalau mengenai yang lain |
| 10 | Slot grid kosong | `Overlap` |
| 11 | `StructureRegistry.HasSupport(data, cell, edge, level)` | `NoSupport` |
| 12 | Semua `cost` tersedia lewat `InventoryManager.Instance.HasItem` | `NotEnoughMaterial` |

Bahan diperiksa paling akhir, supaya alasan yang tampil adalah hal yang perlu pemain ubah lebih dulu.

**Cegah menimpa pemain, NPC, dan musuh:** baris 9 memakai mask yang sudah memuat `Player`, `Enemy`, dan `NPC`. `TryExecuteBuild` menjalankan seluruh validasi sekali lagi tepat sebelum struktur dibuat, karena musuh bisa berjalan masuk setelah hologram terakhir diperiksa. Kotak overlap dikecilkan `overlapShrink`, supaya struktur yang bersentuhan tetap sah.

### Membuat struktur

**`public bool TryExecuteBuild()`**:

1. Jalankan `ValidatePlacement`. Kalau gagal, mainkan suara tolak dan selesai.
2. Kurangi tiap `cost` lewat `InventoryManager.Instance.RemoveItem`.
3. Kurangi durabilitas palu sebesar `hammerDecayPerBuild`.
4. `structureId = StructureRegistry.NewStructureId()`.
5. `Instantiate(prefab, pos, rot, RegionStreaming.Instance.GetStructureRoot(RegionStreaming.Instance.GetRegionAt(pos)))`.
6. `BaseHealth.Initialize(data, structureId, data.maxHP)`.
7. `StructureRegistry.Register(node)`, yang mengisi grid dan menghitung `isGrounded`.
8. Terbitkan `StructureBuiltEvent { structureId, buildableId }`.

Struktur memakai `Instantiate`, bukan `ObjectPooler`, karena jarang dibuat dan hidup lama (`DamagePipeline.md`). `BaseHealth` sudah ada di prefab.

### Bongkar bangunan

**`StructureRegistry.TryDemolish(StructureNode node)`** dipanggil setelah klik kiri ditahan `demolishHoldSeconds`:

1. Tolak kalau `!data.isDemolishable`, atau `Time.time - health.LastDamageTime < demolishBlockedAfterDamageSeconds`. Pemain tidak bisa mengambil kembali dinding yang sedang dipukul.
2. Hitung pengembalian tiap bahan: `Mathf.FloorToInt(amount * demolishRefundRatio * health.HpRatio)`.
3. Panggil `InventoryManager.AddItem`. Sisa yang tidak muat menjadi `WorldItem` lewat `WorldItemManager.Spawn(..., despawns: true)` di depan pemain.
4. Isi objek ikut keluar, misalnya peti (#46).
5. Panggil `BaseHealth.Remove()`, yang menerbitkan `StructureDestroyedEvent` dan menghapus objek. Grafik dukungan memeriksa tetangganya.

**Contoh `Wall_Wood`, biaya 12 kayu:**

| HP | Kayu kembali |
|---|---|
| 100 persen | 6 |
| 50 persen | 3 |
| 10 persen | 0 |

Runtuhan akibat bongkar tidak mengembalikan bahan. Sebelum bongkar, HUD menampilkan jumlah struktur yang akan ikut runtuh, dihitung dengan `PreviewCollapse(node)`.

### Grafik dukungan

Atap yang melayang setelah dindingnya hancur terjadi karena `ValidatePlacement` lama hanya memeriksa saat dibangun. Sekarang dukungan diperiksa ulang setiap ada struktur yang hilang.

**Tetangga dihitung dari grid, bukan disimpan.** Setelah load, grafik langsung ada begitu grid terisi.

| Hubungan | Biaya | Contoh |
|---|---|---|
| Vertikal | 0 | Dinding di atas lantai, atap di atas dinding, lantai atas di atas dinding, dinding di atas dinding |
| Horizontal | 1 | Lantai dengan lantai, atap dengan atap |

- **`isGrounded`:** lantai, dinding, dan perabot pada `level == 0` yang menapak tanah. Struktur ini selalu tegak.
- **`supportDistance`:** biaya terkecil menuju struktur `isGrounded`.
- **Syarat tegak:** ada jalur, dan `supportDistance <= maxHorizontalSpan`.
- **Aturan `SupportRule`:** menentukan syarat saat dibangun. `WallBelow` berarti atap menempel pada dinding di bawah sisinya, atau bersambung ke atap yang tegak.

**`private void OnStructureRemoved(StructureDestroyedEvent e)`**: lepaskan node dari grid, lalu masukkan tetangganya ke `dirtySeeds`. Lewat event ini, runtuhan akibat serangan, bencana, dan bongkar memakai satu jalur.

### Membatasi runtuhan beruntun

**`StructureRegistry.LateUpdate()`**:

1. Kalau `dirtySeeds` berisi, ambil satu benih, lalu kumpulkan satu komponen lewat BFS dengan `Queue` dan `List` yang dipakai ulang.
2. Hitung `supportDistance` dengan BFS 0-1 mulai dari semua node `isGrounded`.
3. Node yang tidak tercapai atau melewati batas masuk `collapseQueue`, tanpa duplikat, dengan tingkat tertinggi lebih dulu.
4. Hapus benih lain yang sudah tercakup komponen itu.
5. Proses paling banyak `maxCollapsePerFrame` dari `collapseQueue`. Tiap node memanggil `BaseHealth.TakeDamage` dengan `DamageType.Structural` sebesar sisa HP.

Runtuhan tidak memanggil dirinya sendiri, jadi tidak ada rekursi. Setiap penghapusan hanya menambah benih untuk frame berikutnya.

**Efek:** `CollapseDebris` diambil dari `ObjectPooler` dengan pool `FX_CollapseDebris`, dibatasi `maxCollapseVfxPerSecond`. Kalau melewati batas, struktur tetap hilang tanpa efek. Debu kembali ke pool setelah partikel selesai.

**Batas `maxStructures`:** menjaga ukuran BFS terburuk. Waktu pemeriksaan pada 400 struktur harus dicatat saat playtest, dan batas ini diturunkan kalau BFS memakan lebih dari 2 ms.

### Pantai dan alasan air pasang

GDD bagian 4 memberi dua alasan: bangunan "akan hancur oleh air pasang atau dilarang oleh sistem". Blueprint ini mengikuti **dilarang oleh sistem**, dengan alasan air pasang yang dijelaskan di dalam game:

- Hologram merah dengan pesan `Pasir tidak stabil. Air pasang akan menghanyutkan bangunan ini`.
- Diary Bab 1 berisi peringatan yang sama (#81).
- `TideManager` (#91) tidak perlu merusak bangunan, karena bangunan tidak pernah berdiri di pasir.

Perubahan teks GDD tidak dilakukan di sini. Usulan teksnya ada di bagian 5.

### Simpan dan muat

```csharp
[Serializable]
public class StructureRecord
{
    public string structureId;
    public string buildableId;
    public Vector3 position;
    public float yaw;
    public float currentHP;
    public string extraJson;   // Dari ISaveable di prefab: isi peti (#46), status pintu (#78)
}
```

`StructureRegistry` adalah `ISaveable`, dengan `SaveId = world_structures`. Isinya `StructureRecord[]` dan `nextStructureNumber`. `structureId` berbentuk `s_000123`, dan tidak pernah dipakai ulang dalam satu run.

**`RestoreState`:**

1. Urutkan rekaman dari tingkat rendah ke tinggi.
2. Buat objek langsung di bawah `GetStructureRoot(region)`, tanpa validasi dan tanpa mengurangi bahan.
3. `BaseHealth.Initialize(data, id, currentHP)`. Fungsi ini tidak bergantung pada `Awake`, karena `structureRoot` region yang tidak dimuat sedang nonaktif (#88).
4. Isi grid. Kalau `buildableId` tidak ditemukan, lewati dan catat peringatan (#80).
5. Panggil `RestoreState` pada `ISaveable` di prefab dengan `extraJson`.
6. Masukkan semua node ke `dirtySeeds`. Bangunan yang kehilangan penopang karena data berubah akan runtuh setelah load.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `StructureBuiltEvent` (`BuildManager`), `BuildModeChangedEvent` (`BuildManager`, baru).

**Event yang didengar:** `StructureDestroyedEvent` (`StructureRegistry`), `UIFocusChangedEvent` (menyembunyikan hologram), `HeavyItemChangedEvent` (palu ikut disimpan lewat kunci di `EquipmentManager.md`).

| Sistem | Hubungan |
|---|---|
| `EquipmentManager` (#60) | Palu adalah `HammerEquippable`, dan `RequestSlot` dipakai tombol `B` |
| `InputManager` (#18) | `OpenBuildMenu`, `ToggleBuildMenu`, dan `BuildRotate` |
| `UIFocusStack` (#83) | `BuildMenu` mewarisi `UIPanel` |
| `SurfaceDetection` (#86) | `IsBeachFootprint` dan `TryGetSurfaceAt` |
| `PhysicsLayers` (#87) | `BuildSurface`, `BuildOverlap`, `Ground`, dan `PhysicsQuery.OverlapBox` |
| `DamagePipeline` (#59) | `BaseHealth` memakai `maxHP` dan `resistances` dari `BuildableData` |
| `RegionStreaming` (#88) | `GetStructureRoot`, `GetRegionAt`, dan lokasi seam untuk `NoBuildVolume` |
| `BaseNavigation` (#78) | Membaca `navMode` |
| `BuildingIntegrity` (#47) | Mode `Repair` dan `Upgrade`, serta `StructureRegistry.ReplaceInstance` saat upgrade |
| `StorageChest` (#46), `SleepSystem` (#67) | Menjadi `BuildableData` |
| `WorldStateSave` (#80) | Mendaftarkan `StructureRegistry` |
| `InventoryManager`, `WorldItemManager` (#66) | Biaya dan pengembalian bahan |
| `TechTreeManager` | `unlockNodeId` |
| `RemoteBaseSimulation` (#90) | Membaca daftar struktur lewat `StructureRegistry` |

**Batas pekerjaan:** perbaikan dan upgrade di #47, jalur musuh di #78, dan restriksi pantai di #86.

## 4. Kasus Khusus

- **Objek pool:** struktur tidak memakai pool. Hanya `FX_CollapseDebris` yang memakai pool. Saat dipakai ulang, `OnSpawned` memutar partikel dari awal, dan `OnReturned` menghentikannya.
- **Game dijeda:** hitung tahan bongkar memakai `Time.deltaTime`, jadi berhenti. Runtuhan yang masih antre berhenti di `LateUpdate` karena dibungkus pemeriksaan `Time.timeScale > 0`, dan berlanjut setelah jeda.
- **Permadeath:** semua struktur hilang bersama `IslandPhase`, dan `world_structures` terhapus bersama save run (#82). Pilihan terakhir palu tidak disimpan.
- **Save dan load:** disimpan posisi, rotasi, HP, ID, dan data tambahan. Tidak disimpan: `Mode`, `Selected`, hologram, `dirtySeeds`, dan `collapseQueue`. Runtuhan yang sedang antre dihitung ulang setelah load.
- **Save saat runtuhan berjalan:** `WorldStateSave` menulis struktur yang masih ada. Setelah load, benih menyelesaikan runtuhan itu.
- **Pemain berdiri di bangunan yang runtuh:** collider hilang dan pemain jatuh. Damage jatuh mengikuti #101. Runtuhan tidak memberi damage langsung ke pemain.
- **Region dilepas saat runtuhan antre:** `BaseHealth` tetap bisa dipanggil pada objek nonaktif, jadi antrean tetap selesai, tanpa efek dan suara.
- **Kerusakan #90 saat region tidak dimuat:** memakai jalur `StructureDestroyedEvent` yang sama.
- **Inventori penuh saat bongkar:** bahan jatuh sebagai `WorldItem`, dan tidak hilang.
- **Palu rusak saat hologram aktif:** alur item hilang di `EquipmentManager.md` memanggil `OnUnequip`, dan hologram hilang.
- **Dua lantai pada tinggi berbeda bertemu:** tetangga hanya dihitung kalau selisih tingginya di bawah `heightSnap`, jadi tidak ada dukungan palsu.
- **Bahan habis setelah hologram hijau:** `TryExecuteBuild` memeriksa ulang.
- **Bangunan menghalangi jalur wajib:** `NoBuildVolume` mencegahnya. Pemeriksaan jalur menuju tujuan wajib ada di #116.
- **Tombol `R`:** `BuildRotate` hanya terbaca saat `Mode == Build`, dan `Reload` hanya saat `GunController` aktif (#60).

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** pantai dilarang (GDD bagian 4), klik kiri dipakai item aktif (`GLOSSARY.md`), dan klik kanan palu membuka menu radial (`EquipmentManager.md`).

**Keputusan teknis di blueprint ini:**

- Menu dibuka dengan sekali klik, karena Action Map berganti saat panel terbuka.
- Tetangga dihitung dari grid, sehingga grafik tidak perlu disimpan.
- `BaseHealth` tidak lagi menyimpan dirinya sendiri lewat `ISaveable`. Datanya ditulis `StructureRegistry`, karena struktur dibuat saat bermain dan harus dibuat ulang saat load.

**Usulan yang menunggu developer (dicatat di #85):**

1. **Bentuk menu:** menu radial dua cincin, dibuka dengan klik kanan atau `B`.
2. **Mode palu:** Perbaiki, Bangun, Upgrade, dan Bongkar. Mode awal Perbaiki, supaya klik kiri tidak membuang bahan tanpa sengaja. Tabel `EquipmentManager.md` yang menyebut klik kiri "Bangun" perlu mengikuti ini.
3. **Biaya, HP, dan daftar struktur:** mengikuti tabel "Daftar struktur awal". `Stairs_Wood` adalah tambahan untuk bangunan bertingkat.
4. **Batas bangunan:** 3 tingkat, 2 sel menjorok, dan 400 struktur.
5. **Pengembalian bongkar:** 50 persen dikali rasio HP, dibulatkan ke bawah, dan ditolak selama 5 detik setelah diserang.
6. **Area larangan bangun:** seam, lokasi jembatan, dan helipad.
7. **Teks GDD bagian 4, untuk disetujui:** "Pemain dilarang membangun di atas pasir pantai. Sistem menolak penempatan, dan Diary menjelaskan bahwa air pasang akan menghanyutkan bangunan di sana." Rekomendasi: pakai teks ini, karena satu aturan lebih jelas daripada dua kemungkinan.

## 6. Skenario Tes

1. Pegang palu, lalu klik kanan. **Hasil:** menu radial terbuka, kursor di tengah, dan waktu tetap berjalan.
2. Tekan `B` tanpa palu di hotbar. **Hasil:** notifikasi muncul, dan menu tidak terbuka.
3. Pilih Dinding, bidik tanah, lalu tekan `R` empat kali. **Hasil:** hologram berputar 90 derajat tiap tekan, dan kembali ke posisi awal.
4. Bangun dinding pada sisi sel A, lalu coba dinding di sisi yang sama dari sel tetangga. **Hasil:** ditolak dengan `Overlap`.
5. Bangun dua lantai bersebelahan di tanah miring. **Hasil:** tingginya sama, dan rok lantai menutup celah.
6. Bidik lereng dengan selisih di atas 1,25 meter. **Hasil:** merah dengan `TooSteep`.
7. Bangun dua tingkat, lalu coba tingkat ke-4. **Hasil:** ditolak dengan `MaxLevel`.
8. Bangun menjorok tiga sel dari dinding pada lantai atas. **Hasil:** sel ketiga ditolak dengan `NoSupport`.
9. Bidik pasir. **Hasil:** merah, dengan pesan air pasang.
10. Berdiri di dalam hologram, lalu klik. **Hasil:** ditolak dengan `Occupied`. Ulangi dengan zombie dan NPC. **Hasil:** sama.
11. Bangun tanpa kayu. **Hasil:** ditolak dengan `NotEnoughMaterial`, dan bahan tidak berkurang.
12. Bongkar dinding penuh dan dinding ber-HP 50 persen. **Hasil:** kembali 6 dan 3 kayu.
13. Bongkar dinding yang baru dipukul zombie. **Hasil:** ditolak selama 5 detik.
14. Hancurkan satu-satunya dinding penopang atap lewat `DevConsole`. **Hasil:** atap runtuh pada frame berikutnya.
15. Bangun 200 struktur bertingkat, lalu hancurkan penopang utamanya. **Hasil:** runtuh bertahap paling banyak 4 per frame, tingkat atas lebih dulu, tanpa frame di atas 33 ms. Catat waktunya.
16. Isi inventori penuh, lalu bongkar. **Hasil:** bahan jatuh di depan pemain.
17. Bangun base, rusak satu dinding, simpan, keluar, lalu lanjutkan. **Hasil:** posisi, rotasi, dan HP sama, dan atap tidak runtuh.
18. Bangun di Region 1, pergi sampai region dilepas, lalu simpan dan lanjutkan dari Region 3. **Hasil:** tidak ada error, dan bangunan muncul saat Region 1 dimuat lagi.
19. Coba bangun di seam `S1`. **Hasil:** ditolak dengan `NoBuildZone`.
20. Mati, lalu mulai run baru. **Hasil:** tidak ada struktur lama.
