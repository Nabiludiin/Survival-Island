# Blueprint: RegionStreaming.cs
**Lokasi:** `docs/blueprint/Core_Logic/RegionStreaming.md`
**Deskripsi Utama:** Struktur scene pulau dan pemuatan 4 region secara additive berdasarkan jarak pemain, supaya perjalanan antar-region tidak terputus dan memori tetap terjaga.
**Issue:** #88
**Rujukan GDD:** Bagian 5 (4 Region, jembatan, dan perjalanan Region 4 ke Region 3). Struktur scene dan angka memori adalah usulan teknis, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

### Perbandingan pilihan

| Hal | Satu scene besar | Additive scene per region |
|---|---|---|
| Perjalanan tanpa loading | Ya | Ya, kalau region tetangga dimuat lebih dulu |
| Memori | Semua region selalu dimuat | Hanya region aktif dan tetangganya |
| Bake NavMesh | Satu bake panjang. Ubah satu pohon, bake semua | Per region. Sambungan memakai `NavMeshLink` |
| Kerja tim dan Git | Satu file scene yang mudah bentrok | Tiap region punya file sendiri |
| Risiko utama | RAM dan waktu muat awal | Hitch saat aktivasi, dan status objek harus disimpan saat dilepas |
| Status objek | Selalu ada di memori | Perlu `RegionStateStore` |
| Kapan cocok | Peta kecil | Peta besar dengan batas jelas |

**Pilihan: additive scene per region.** GDD membagi pulau menjadi 4 lingkungan berat yang berbeda dan pemain tidak butuh semuanya sekaligus. Perjalanan Region 4 ke Region 3 tetap tanpa putus, karena region tujuan dimuat sebelum pemain tiba.

### Struktur scene

| Scene | Isi | Selalu dimuat |
|---|---|---|
| `IslandPhase` | Manager Fase 2, pemain, kamera, cahaya matahari, laut, `FarMesh` tiap region, trigger `ZoneManager`, `SeamBarrier`, `NavMeshLink` antar-region, dan akar bangunan | Ya |
| `Region1_Volcano` | Terrain, pohon, batu, node panen, loot, dan NavMesh Region 1 | Tidak |
| `Region2_Amazon` | Sama, untuk Region 2 | Tidak |
| `Region3_Snow` | Sama, ditambah Pangkalan Militer dan model kerangka helikopter | Tidak |
| `Region4_Village` | Sama, ditambah bangunan desa | Tidak |

**Aturan region scene:**

- Tidak boleh memuat manager, kamera, `AudioListener`, `EventSystem`, atau cahaya matahari. `LayerAuditWindow` (#87) menambah pemeriksaan ini.
- Semua objek berada di bawah satu akar dengan komponen `RegionSceneRoot`.
- `IslandPhase` selalu menjadi scene aktif. `Instantiate` jatuh ke scene ini, dan `EnvironmentDirector` (#92) menjadi satu-satunya penulis fog dan cahaya.

### Peta konsep

Tiap region memakai satu Terrain 1000 x 1000 meter. Ukuran dan koordinat ini adalah usulan, dan level design boleh menyetelnya selama aturan jarak seam tetap terpenuhi.

```text
 Z (utara)
 3000 +-------------------+
      | Region 3: Salju   |
      | Region3_Snow      |
      | [Pangkalan]       |
      | [H] Helipad       |
 2000 +--------S3---------+-------------------+
      | Region 4: Desa    | Region 2: Amazon  |
      | Region4_Village   S2 Region2_Amazon   |
      | [P] Suku cadang   | Jamur dan getah   |
 1000 +-------------------+--------S1---------+
      Tebing tak terlewati| Jurang + jembatan |
                          | Region 1: Berapi  |
                          | Region1_Volcano   |
                          | [Base] [Gunung]   |
    0 +-------------------+-------------------+
      0                 1000                2000  X
                             Pantai selatan
```

| Seam | Menghubungkan | Posisi | Bentuk |
|---|---|---|---|
| `S1` | Region 1 dan Region 2 | (1500, 1000) | Jembatan gantung di atas jurang (#68) |
| `S2` | Region 2 dan Region 4 | (1000, 1500) | Celah lembah di antara tebing |
| `S3` | Region 4 dan Region 3 | (500, 2000) | Jalur Angkut menanjak |

**Jalur pengangkutan suku cadang:** titik tengah desa (500, 1750), lalu `S3` (500, 2000), lalu Helipad (500, 2150). Total 400 meter, dengan 150 meter terakhir di area dingin. Angka ini mengikuti asumsi di `GameBalanceConfig`.

**Urutan akses:** pulau bersifat rantai, yaitu Region 1, Region 2, Region 4, lalu Region 3. Syarat akses di tiap seam milik #68.

**Sisi peta lainnya:** batas tiap region ditutup tebing, gunung, atau laut. Pemain hanya berpindah region lewat seam. Sudut (1000, 1000) tertutup tebing, jadi Region 1 dan Region 4 tidak bersentuhan.

### Data dan variabel

```csharp
public enum RegionLoadState { Unloaded, Loading, Loaded, Unloading }

[Serializable]
public class RegionRecord
{
    public RegionType region;
    public string sceneName;
    public Bounds bounds;
    public GameObject farMesh;
    public Transform structureRoot;     // Akar bangunan pemain di region ini
    [NonSerialized] public RegionLoadState state;
    public float lastUnloadedGameHour;  // Untuk mengejar waktu saat region kembali
}

[Serializable]
public class RegionSeam
{
    public RegionType a, b;
    public Vector3 position;
    public SeamBarrier barrier;
    public NavMeshLink[] links;         // Satu per agent type
}
```

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `regions` | `RegionRecord[4]` | Diisi di Inspector | Data tiap region |
| `seams` | `RegionSeam[3]` | `S1`, `S2`, `S3` | Sambungan antar-region |
| `Store` | `RegionStateStore` | Kosong | Status objek region, termasuk yang tidak dimuat |
| `IsReady` | `bool` | `false` | Region awal sudah siap |
| `IsStalled` | `bool` | `false` | Pemain tertahan karena region belum siap |
| `lastSafePosition` | `Vector3` | Posisi awal | Posisi aman terakhir |

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.streaming.loadDistance` | 150 | Jarak ke seam untuk mulai memuat region seberang |
| `GameBalanceConfig.streaming.unloadDistance` | 250 | Jarak ke seam untuk melepas region seberang |
| `GameBalanceConfig.streaming.urgentDistance` | 40 | Jarak untuk menaikkan prioritas pemuatan |
| `GameBalanceConfig.streaming.checkIntervalSeconds` | 0,5 | Jeda pemeriksaan jarak |
| `GameBalanceConfig.streaming.maxLoadedRegions` | 2 | Batas region dimuat bersamaan |
| `GameBalanceConfig.streaming.stallNoticeSeconds` | 2 | Jeda sebelum HUD menampilkan pesan memuat |
| `GameBalanceConfig.streaming.stallPanelSeconds` | 10 | Jeda sebelum panel `Loading` dan jeda waktu dipakai |
| `GameBalanceConfig.streaming.safePositionIntervalSeconds` | 1 | Jeda pencatatan posisi aman |
| `GameBalanceConfig.streaming.unusedAssetsMinIntervalSeconds` | 120 | Jeda minimum `Resources.UnloadUnusedAssets` |
| `GameBalanceConfig.streaming.baseMemoryLimitMB` | 1000 | Batas memori manager, `IslandPhase`, dan pool |
| `GameBalanceConfig.streaming.regionMemoryLimitMB` | 1200 | Batas memori tiap region |
| `GameBalanceConfig.streaming.regionBatchLimit` | 600 | Batas batch tiap region |
| `GameBalanceConfig.streaming.baseBatchLimit` | 300 | Batas batch `IslandPhase` |
| `GameBalanceConfig.streaming.regionLoadTimeLimitSeconds` | 8 | Batas waktu memuat region |

### Batas memori dan draw call

Target perangkat (usulan): PC dengan RAM 8 GB, VRAM 4 GB, resolusi 1080p, dan 60 FPS.

| Bagian | Memori | Batch per frame | Segitiga terlihat |
|---|---|---|---|
| Manager, `IslandPhase`, dan pool | Paling besar 1.000 MB | Paling banyak 300 | Paling banyak 500 ribu |
| Tiap region | Paling besar 1.200 MB | Paling banyak 600 | Paling banyak 1 juta |
| **Terburuk: dua region aktif** | **Paling besar 3.400 MB** | **Paling banyak 1.500** | **Paling banyak 2,5 juta** |

Saat `maxLoadedRegions` bernilai 2, jumlah terburuk tetap di bawah batas. Bayangan dan efek cuaca termasuk dalam batas batch ini.

## 2. Detail Logika & Fungsi

### Pemuatan awal

**`public IEnumerator LoadInitialRegions(Vector3 playerPosition)`** dipanggil `RunLifecycle` (#82) saat panel `Loading` terbuka:

1. Tentukan region lewat `GetRegionAt(playerPosition)`.
2. Set `Application.backgroundLoadingPriority = ThreadPriority.High`.
3. Muat region itu, dan region seberang kalau pemain berada dalam `loadDistance` dari seam.
4. Pulihkan status lewat `RestoreRegion(region)` untuk tiap region.
5. Kembalikan prioritas ke `BelowNormal`, set `IsReady = true`, dan mulai `EvaluationLoop()`.

### Pemuatan berdasarkan jarak

**`IEnumerator EvaluationLoop()`** berjalan setiap `checkIntervalSeconds` dengan `WaitForSecondsRealtime`, dan dilewati saat `UIFocusStack.Instance.IsPaused`:

1. Region tempat pemain berdiri harus `Loaded`. Kalau belum, langsung masukkan ke proses pemuatan darurat.
2. Untuk tiap seam yang menyentuh region pemain:
   - Lewati kalau region seberang belum dibuka, menurut `RegionGateSystem.IsUnlocked(region)` (#68).
   - `Vector3.Distance(pemain, seam.position) <= loadDistance`: minta `RequestLoad(seberang)`.
3. Untuk tiap region `Loaded` yang bukan region pemain, cari jaraknya ke seam terdekat. Kalau jaraknya `>= unloadDistance`, minta `RequestUnload(region)`.
4. Kalau pemain berada dalam `urgentDistance` dari seam dan region seberang masih `Loading`, set `backgroundLoadingPriority = High`. Kembalikan ke `BelowNormal` setelah selesai.

Selisih 150 dan 250 meter mencegah region dimuat dan dilepas berulang saat pemain mondar-mandir di dekat batas.

**Syarat peta:** jarak antar-seam harus lebih besar dari `loadDistance + unloadDistance`, yaitu 400 meter. Dengan koordinat usulan, jarak `S1` ke `S2` dan `S2` ke `S3` sekitar 707 meter. Jadi region ketiga tidak diminta dalam kondisi normal. `maxLoadedRegions` tetap menjadi pengaman: permintaan yang melewatinya menunggu sampai region lama dilepas.

**Satu operasi sekali jalan:** permintaan masuk antrean, dan region yang pemain tuju lebih didahulukan. Kalau permintaan muat datang untuk region yang sedang `Unloading`, permintaan disimpan lalu dijalankan setelah selesai.

### Memuat satu region

**`IEnumerator LoadRegion(RegionRecord r)`**:

1. `r.state = Loading`, lalu terbitkan `RegionLoadStateChangedEvent { region, Loading }`.
2. `yield return AsyncSceneLoader.Instance.LoadAdditive(r.sceneName)`.
3. Ambil `RegionSceneRoot`, lalu panggil `RestoreRegion(r.region)`. Objek mengambil statusnya dari `Store`.
4. Aktifkan `r.structureRoot`, matikan `r.farMesh`, dan panggil `RefreshLinks()`.
5. `LightProbes.TetrahedralizeAsync()`.
6. `r.state = Loaded`, matikan `SeamBarrier` yang kedua sisinya sudah dimuat, lalu terbitkan event `Loaded`.

**Terrain:** `Terrain.allowAutoConnect = true` dengan `groupingID` yang sama, jadi batasnya tersambung otomatis.

**Hitch aktivasi:** `Awake` dan `OnEnable` objek region tidak boleh melakukan pencarian berat. Pekerjaan besar dibagi lewat coroutine, setelah event `Loaded`.

### Melepas satu region

**`IEnumerator UnloadRegion(RegionRecord r)`**:

1. `r.state = Unloading`, lalu terbitkan event `Unloading`. `SpawnManager`, `NPCRoster`, `HeavyItemSystem`, dan `WorldItemManager` mengurus objeknya pada frame ini.
2. Tunggu satu frame.
3. Panggil `CaptureRegion(r.region)`, lalu catat `r.lastUnloadedGameHour`.
4. Aktifkan `r.farMesh`, matikan `r.structureRoot`, dan nyalakan `SeamBarrier` terkait.
5. `yield return AsyncSceneLoader.Instance.UnloadAdditive(r.sceneName)`.
6. Panggil `RefreshLinks()`, lalu `r.state = Unloaded`, dan terbitkan event `Unloaded`.
7. Panggil `Resources.UnloadUnusedAssets()` kalau sudah lewat `unusedAssetsMinIntervalSeconds` sejak pemanggilan terakhir.

### Menyimpan status objek di region yang tidak dimuat

`RegionStateStore` memegang status tiap objek per region, dan tetap hidup di `IslandPhase`:

```csharp
public class RegionStateStore
{
    // region -> (SaveId objek -> status JSON)
    Dictionary<RegionType, Dictionary<string, string>> states;

    public void Write(RegionType region, string saveId, string json);
    public bool TryRead(RegionType region, string saveId, out string json);
    public string ToSaveJson();              // Diubah ke daftar, karena JsonUtility tidak bisa Dictionary
    public void LoadFromSaveJson(string json);
}
```

| Objek | Cara disimpan |
|---|---|
| Objek statis dengan status (pohon, batu, peti, Diary, Lucky Machine) | `ISaveable` dengan `SaveId` stabil dari #80. Saat `CaptureRegion`, statusnya masuk ke `Store`. Saat `RestoreRegion`, statusnya dibaca kembali |
| `LootSpawnerSystem` | `ISaveable`. Daftar titik dan barang yang sudah diambil masuk ke `Store` (#89) |
| `WorldItem` yang jatuh | `WorldItemManager` (#66) memakai `CaptureRegion` dan `RestoreRegion`: mencatat `itemId`, jumlah, dan posisi ke `Store`, lalu mengembalikan objek ke pool |
| Node yang tumbuh kembali | `Store` menyimpan status dan jam absolut `RegrowAtGameHour`. Saat region kembali, #73 membandingkannya dengan `TotalGameHours` |

**Pengisian:** `RestoreRegion` memanggil `root.GetComponentsInChildren<ISaveable>(true)` sekali saat region masuk, lalu menyimpan daftarnya untuk dipakai `CaptureRegion`. Pencarian ini tidak dipanggil setiap frame.

**Save:** `WorldStateSave` (#80) memanggil `CaptureLoadedRegions()` lalu `Store.ToSaveJson()`. Region yang tidak dimuat sudah ada di `Store`, jadi datanya tetap tersimpan.

### NavMesh per region

| Hal | Aturan |
|---|---|
| Bake | Tiap region scene memiliki `NavMeshSurface` sendiri, dengan `collectObjects = Children`. Datanya ikut dimuat bersama scene |
| Layer sumber | `Terrain` dan `Obstacle`. Bangunan pemain memakai `NavMeshObstacle` (#78), bukan bake |
| Agent type | `Humanoid` untuk zombie, NPC, dan hewan kecil. `Large` untuk Beruang Kutub. Tiap agent type punya satu `NavMeshSurface` per region |
| Sambungan | `NavMeshLink` di `IslandPhase`, satu per seam dan agent type, dengan lebar 6 meter, `bidirectional = true`, dan `autoUpdate = false` |
| Batas | NavMesh kedua sisi berhenti sekitar 2 meter dari seam, dan titik awal serta ujung link berada di dalamnya |
| Saat bermain | Tidak ada bake ulang. Yang berubah hanya status link |

**`private void RefreshLinks()`** memanggil `link.UpdateLink()` dan mengatur `link.enabled` hanya kalau kedua region seam `Loaded`, serta seam itu sudah terbuka. Link `S1` juga mengikuti status jembatan (`BridgeSite`, #68).

### Musuh dan NPC saat region dilepas

| Objek | Perilaku saat region `Unloading` |
|---|---|
| **Zombie dan hewan** | `SpawnManager` (#62) mengembalikan semua musuh dengan `GetRegionAt(posisi)` sama dengan region itu ke `ObjectPooler`. HP dan state tidak disimpan. Musuh baru muncul lewat `SpawnManager` saat region kembali |
| **Musuh yang mengejar pemain** | Mereka berada dekat pemain, jadi berada di region yang tidak dilepas. Musuh tidak menyeberang ke region yang belum dimuat, karena link hanya aktif kalau dua sisinya `Loaded` |
| **NPC `Follow`** | Tidak terdampak dalam kondisi normal. Kalau tertinggal di region yang dilepas, NPC dipindah ke titik NavMesh valid terdekat di belakang pemain |
| **NPC `StayAtBase` atau `Work`** | `NPCRoster` (#76) mencatat posisi, state, dan HP, lalu mematikan objeknya. Objek NPC tinggal di `IslandPhase`, jadi tidak hilang. Saat region kembali, objek dinyalakan dan `agent.Warp` ke posisi tercatat |
| **Nasib base saat ditinggal** | `RemoteBaseSimulation` (#90) bekerja dari data, tanpa memuat region |

Musuh selalu mematikan `NavMeshAgent` di `OnReturned` dan memeriksa `agent.isOnNavMesh` di `OnSpawned`, supaya tidak ada agen yang kehilangan NavMesh.

### Yang tetap aktif

| Benda | Data | Objek di dunia |
|---|---|---|
| **Helikopter** | `RescueEventManager` (#14) di `IslandPhase`. Selalu aktif | Model di `Region3_Snow`, mengikuti progres saat region masuk. `FarMesh` tidak menampilkan progres |
| **Suku cadang** | `HeavyItemSystem` (#69) di `IslandPhase`. Posisi dan status selalu ada, jadi kompas (#30) tetap bekerja | Objek hanya aktif kalau regionnya `Loaded` |
| **Suku cadang di tangan** | Selalu aktif | Menjadi anak pemain, jadi tidak ikut dilepas |
| **Base** | `BaseHealth` dan daftar bangunan tetap ada | Berada di `RegionRecord.structureRoot` di `IslandPhase`. Dimatikan saat region dilepas, supaya tidak memakai draw call dan tidak melayang di atas area kosong |
| **Pemain, waktu, cuaca, dan laut** | Selalu aktif | Berada di `IslandPhase` |
| **`FarMesh`** | Tidak ada | Aktif kalau region aslinya tidak dimuat, supaya gunung berapi dan puncak salju tetap terlihat dari jauh |

Kerusakan dari #90 ditulis langsung ke `BaseHealth`, karena fungsi pada komponen di objek nonaktif tetap bisa dipanggil.

### Pengaman kalau pemain tiba lebih cepat

Dalam kondisi normal, pemain berlari 8 m/s butuh sekitar 18 detik dari `loadDistance` sampai seam, lebih lama dari batas muat 8 detik. Pemain membawa suku cadang butuh 60 detik. Kalau terlambat, pengaman berikut berlaku berurutan:

1. **Prioritas naik:** pada `urgentDistance`, prioritas muat menjadi `High`.
2. **Dinding seam:** `SeamBarrier` adalah `BoxCollider` di layer `Obstacle`, dan aktif selama salah satu sisi belum `Loaded`. Pemain tertahan di batas, dan tidak jatuh ke ruang kosong.
3. **Pesan:** kalau pemain tertahan selama `stallNoticeSeconds`, `IsStalled = true`, dan HUD menampilkan "Memuat area".
4. **Jeda:** kalau masih tertahan selama `stallPanelSeconds`, `RunLifecycle` mendorong panel `Loading`. Waktu berhenti, jadi pemain tidak mati saat menunggu. Panel tertutup setelah region `Loaded`.
5. **Jatuh ke ruang kosong:** `RegionBoundaryGuard` membuka `public Vector3 LastSafePosition { get; }` lewat `Instance`, yang dipakai `RavineRecoveryVolume` dan `VerifyPlayerRegion` (#68). Nilainya dicatat setiap `safePositionIntervalSeconds`, saat `PhysicsQuery.Raycast` ke `PhysicsMasks.Ground` mengenai tanah. Kalau pemain berada di region yang belum `Loaded` dan tidak ada tanah di bawahnya, pemain dipindah ke `lastSafePosition`, dan gravitasi `IslandPlayerController` ditahan sampai region siap.

### Fungsi publik

| Fungsi | Kegunaan |
|---|---|
| `RegionType GetRegionAt(Vector3 position)` | Region dari `Bounds`, tanpa memakai trigger |
| `bool IsLoaded(RegionType region)` | Dipakai `SpawnManager`, #70, dan #71 sebelum memunculkan objek |
| `Transform GetStructureRoot(RegionType region)` | Mengembalikan `RegionRecord.structureRoot`, sebagai induk bangunan baru untuk `BuildManager` dan `BuildMenu` (#79) |
| `float GetElapsedGameHours(RegionType region)` | Waktu sejak region dilepas. #73 tidak lagi memakainya |
| `void CaptureLoadedRegions()` | Mengambil status region aktif sebelum save (#80) |

`GetRegionAt` berbeda dengan `RegionChangedEvent`. Event itu milik `ZoneManager` dan menunjukkan region gameplay pemain. `RegionStreaming` tidak memakai trigger untuk memutuskan pemuatan.

### Pengukuran batas

Perintah `streamstat` di `DevConsole` (#56) menampilkan state tiap region, memori dari `Profiler.GetTotalAllocatedMemoryLong()`, `Batches Count` dari `ProfilerRecorder`, dan waktu muat terakhir. Nilai di atas batas ditampilkan sebagai peringatan. Memori satu region dihitung dari selisih sebelum dan sesudah region dimuat. Perintah `streamdelay <detik>` menambah penundaan buatan sebelum aktivasi, untuk menguji pengaman.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `RegionLoadStateChangedEvent` (`region`, `state`).

**Event yang didengar:** `RegionUnlockedEvent` (#68, memeriksa ulang jarak dan link), `RunEndedEvent` (menghentikan `EvaluationLoop`).

| Sistem | Hubungan |
|---|---|
| `RunLifecycle` (#82) | Memanggil `LoadInitialRegions`. Loading antar-fase ada di #82 |
| `AsyncSceneLoader` | Melakukan `LoadAdditive` dan `UnloadAdditive` |
| `RegionGateSystem` (#68) | Menyediakan `IsUnlocked`. `BridgeSite` berada di `Region1_Volcano`, dan membuka link `S1` |
| `WorldStateSave` (#80) | Menyimpan `Store`, dan menyediakan `ISaveable` |
| `SpawnManager` (#62) | Mengembalikan musuh saat `Unloading`, dan tidak memunculkan musuh di region yang tidak `Loaded` |
| `NPCRoster` (#76) | Mengurus NPC saat region dilepas dan kembali |
| `HeavyItemSystem` (#69) | Mengurus suku cadang yang tergeletak |
| `BuildManager`, #47, #78 | Bangunan memakai `GetStructureRoot`, dan menjadi nonaktif bersama region |
| `ResourceRespawn` (#73) | Mengejar waktu saat event `Loaded` |
| `RemoteBaseSimulation` (#90) | Bekerja dari data saat region base tidak dimuat |
| `ZoneManager` | Trigger berada di `IslandPhase`, dan tetap menjadi pengirim `RegionChangedEvent` |
| `EnvironmentDirector` (#92) | Satu-satunya penulis fog dan cahaya |
| `SurfaceDetection` (#86) | Membaca Terrain region yang dimuat |
| `OptimizationManager` | Tetap mengatur LOD dan culling musuh. Memori dunia ada di blueprint ini |

## 4. Kasus Khusus

- **Objek pool:** pool hidup di `PersistentRoot`, jadi tidak ikut terlepas bersama region. Musuh dan `WorldItem` harus dikembalikan saat event `Unloading`, sebelum NavMesh hilang. Saat dipakai ulang, musuh mereset HP, state, dan `NavMeshAgent` lewat `OnSpawned`.
- **Game dijeda:** `EvaluationLoop` tidak memulai operasi baru. Operasi yang sedang berjalan tetap selesai, karena `AsyncOperation` tidak mengikuti `Time.timeScale`.
- **Permadeath:** `RunLifecycle` memuat `Bootstrap` dengan mode `Single`, yang melepas `IslandPhase` dan semua region. `Store` hilang bersama scene, dan isi save dihapus oleh #82. Tidak ada status lintas run.
- **Save dan load:** daftar region yang sedang dimuat tidak disimpan, karena dihitung dari posisi pemain. Yang disimpan adalah `Store` dan `lastUnloadedGameHour`. Saat dimuat, `Store` diisi lebih dulu, baru region dimuat.
- **Tidur:** `SleepSystem` (#67) tidak memindahkan pemain, jadi tidak memicu pemuatan.
- **Region seberang terkunci:** tidak dimuat. Dari kejauhan, pemain melihat `FarMesh`.
- **Pemain melewati jurang lewat bug:** `GetRegionAt` menunjuk region yang belum dimuat. Pemuatan darurat berjalan, dan `RegionBoundaryGuard` menahan pemain. Pencegahan melompat atau berenang ada di #68.
- **Pemain membawa suku cadang lewat seam:** barang menjadi anak pemain, jadi tidak terpengaruh. Kalau jatuh di sisi yang dilepas, statusnya tersimpan di `HeavyItemSystem`.
- **Bencana di region yang tidak dimuat:** kerusakan bangunan memakai data lewat #70 dan #90. Fireball hanya muncul di sekitar pemain.
- **Scene gagal dimuat:** kalau `LoadAdditive` mengembalikan `null`, catat kesalahan, biarkan `SeamBarrier` aktif, dan coba lagi setelah 5 detik.
- **Run berakhir saat operasi berjalan:** `RunEndedEvent` menghentikan antrean. Operasi yang berjalan dibiarkan selesai, karena scene akan diganti saat reset.
- **Dua sistem meminta pemuatan yang sama:** `RequestLoad` diabaikan kalau state bukan `Unloaded`.
- **Occlusion Culling:** bake dilakukan dengan semua scene terbuka, supaya data tiap scene konsisten saat dimuat additive.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** nama scene `IslandPhase`, `Region1_Volcano`, `Region2_Amazon`, `Region3_Snow`, dan `Region4_Village` di `GLOSSARY.md`. Jarak 400 meter dengan 150 meter area dingin di `GameBalanceConfig`.

**Keputusan teknis di blueprint ini:**

- `IslandPhase` selalu menjadi scene aktif.
- Pemuatan memakai jarak ke seam, bukan trigger, supaya pemain tidak perlu menyentuh collider tertentu.
- Status objek disimpan sebagai JSON per `SaveId` di satu `RegionStateStore`.
- Tidak ada bake NavMesh saat bermain.

**Usulan yang disetujui developer sebagai nilai awal (dicatat di #85):**

1. **Additive scene per region:** dipilih daripada satu scene besar.
2. **Peta konsep:** rantai Region 1, Region 2, Region 4, Region 3, dengan tiga seam. Syarat akses tiap seam tetap milik #68.
3. **Musuh saat region dilepas:** dikembalikan ke pool tanpa disimpan.
4. **NPC di base:** objek dimatikan saat region dilepas, dan nasibnya dihitung #90.
5. **Bangunan:** tetap ada di `IslandPhase` tetapi nonaktif saat regionnya tidak dimuat.
6. **Batas perangkat:** RAM 8 GB, VRAM 4 GB, 1080p, 60 FPS, dengan batas memori dan batch pada tabel.
7. **Jeda saat pemain tertahan lama:** panel `Loading` menjeda waktu setelah 10 detik.

## 6. Skenario Tes

1. Mulai di Region 1, lalu jalankan `streamstat`. **Hasil:** hanya `IslandPhase` dan `Region1_Volcano` dimuat, dan `FarMesh` region lain terlihat.
2. Selesaikan jembatan lewat #68, lalu berjalan ke `S1`. **Hasil:** `Region2_Amazon` mulai dimuat pada jarak 150 meter dan selesai sebelum pemain tiba, tanpa layar loading.
3. Mondar-mandir di jarak 150 sampai 200 meter dari seam. **Hasil:** region tidak dimuat dan dilepas berulang.
4. Bawa suku cadang dari Region 4 ke Helipad. **Hasil:** perjalanan tanpa putus, FPS tidak turun di bawah 30 lebih dari 0,1 detik saat aktivasi, dan barang tetap di tangan.
5. Tebang pohon di Region 1, ambil loot, pindah sampai Region 1 dilepas, lalu kembali. **Hasil:** pohon dan loot tetap sesuai kondisi terakhir.
6. Simpan saat berada di Region 3, keluar, lalu lanjutkan. **Hasil:** Region 1 yang tidak dimuat tetap memiliki status tadi saat dikunjungi lagi.
7. Dekati `S1` saat malam dengan zombie yang mengejar. **Hasil:** zombie menyeberang lewat `NavMeshLink`, setelah kedua region dimuat.
8. Biarkan region dengan zombie dilepas. **Hasil:** zombie kembali ke pool, tidak ada error agen tanpa NavMesh, dan jumlah objek pool tetap.
9. Tinggalkan NPC di base di Region 1, lalu pergi jauh dan kembali. **Hasil:** NPC berada di posisi tercatat dengan state yang sama, kecuali diubah #90.
10. Bangun dinding, pergi sampai region dilepas, lalu kembali. **Hasil:** dinding tetap ada, dengan HP yang sama.
11. Tambahkan penundaan buatan lewat perintah `streamdelay 15` di `DevConsole`, lalu sprint ke seam. **Hasil:** pemain tertahan `SeamBarrier`, pesan muncul setelah 2 detik, dan panel `Loading` setelah 10 detik.
12. Pindahkan pemain ke region yang belum dimuat lewat `DevConsole`. **Hasil:** pemain tidak jatuh terus, region dimuat darurat, atau pemain kembali ke posisi aman.
13. Ukur semua region dengan `streamstat`. **Hasil:** catat angkanya. Nilai di atas batas harus diperbaiki atau batasnya diperbarui di blueprint ini.
14. Mati saat dua region dimuat. **Hasil:** semua region terlepas, dan run baru tidak membawa status lama.
