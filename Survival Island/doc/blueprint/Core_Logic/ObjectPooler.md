# Blueprint: ObjectPooler.cs
**Lokasi:** `Survival Island/doc/blueprint/Core_Logic/ObjectPooler.md`
**Deskripsi Utama:** Penyelamat FPS dan RAM. Zombie dan peluru tidak boleh di-`Instantiate` berulang. Skrip ini membuat gudang objek pasif yang bisa dipanggil dan dimatikan secara instan.
**Issue:** #82
**Rujukan GDD:** Usulan teknis, belum ada di GDD. Mengikuti aturan Object Pooling di `Claude.md` (Tahap 6 poin 1 dan 5).

## 1. Arsitektur & Variabel Utama

### Pola

`ObjectPooler` adalah manager persisten (`IBootstrapService`, urutan ke-12 di #82). `Awake` hanya mengisi `Instance`. Semua pekerjaan dilakukan di `Initialize()`, supaya `GameManager` dan `Balance` sudah siap.

Semua objek pool berada di bawah `PersistentRoot`, jadi tidak ikut hilang saat scene berganti. Susunannya:

```text
PersistentRoot
└── Managers
    └── ObjectPooler
        ├── PoolStaging   (selalu nonaktif, hanya dipakai saat membuat objek)
        └── PoolStorage   (aktif; anak-anaknya nonaktif)
            ├── zombie_standar (00) ... (49)
            └── VFX_Muzzle (00) ...
```

### Kontrak `IPoolable`

Interface ini dipakai banyak blueprint (`EnemyAI`, `SpawnManager`, `VFXManager`, `ShipInteractable`, `WorldItem`, dan lainnya). Bentuknya tetap:

```csharp
public interface IPoolable
{
    void OnSpawned();   // Dipanggil setelah SetActive(true) dan setelah posisi diatur
    void OnReturned();  // Dipanggil sebelum SetActive(false)
}

public enum PoolOverflow
{
    Fixed,  // Pool habis: kembalikan null. Dipakai musuh, efek, dan UI
    Grow    // Pool habis: buat objek baru sampai maxSize, lalu kembalikan null
}

[Serializable]
public class Pool
{
    public string tag;              // Kunci unik. Musuh memakai EnemyData.enemyId
    public GameObject prefab;
    public int size;                // Jumlah objek yang dibuat saat Initialize (pre-warm)
    public PoolOverflow overflow;   // Nilai awal Fixed
    public int maxSize;             // Hanya untuk Grow. 0 berarti sama dengan size
}
```

**Urutan panggilan pada satu objek, dijamin:**

| Saat | Urutan |
|---|---|
| Keluar dari pool | Atur posisi dan rotasi, `SetActive(true)` (Unity menjalankan `Awake` pertama kali dan `OnEnable`), lalu `OnSpawned()` |
| Kembali ke pool | `OnReturned()`, `SetActive(false)` (Unity menjalankan `OnDisable`), lalu pindah ke `PoolStorage` |

Dengan urutan ini, langganan event di `OnEnable` dan `OnDisable` (`GameEventBus.md`) sudah aktif saat `OnSpawned` berjalan, dan sudah lepas setelah objek masuk pool.

**Aturan untuk setiap `IPoolable`:**

1. `OnSpawned` mengembalikan HP, state, skala, dan komponen ke nilai awal (`Claude.md` Tahap 6 poin 5). Nilai awal diambil dari data atau prefab, bukan dari sisa pemakaian sebelumnya.
2. `OnReturned` melepas semua rujukan ke objek lain, pendaftaran ke manager, dan timer. Fungsi ini harus aman dipanggil dua kali, dan aman dipanggil walaupun `OnSpawned` belum selesai.
3. Data yang bergantung pada pemanggil diisi lewat `Setup(...)` setelah `SpawnFromPool`, bukan di `OnSpawned` (contoh `ShipInteractable.Setup`).
4. Objek tidak boleh memanggil `Destroy` pada dirinya. Jalur keluar satu-satunya adalah `ReturnToPool`, atau jalur pemiliknya, misalnya `SpawnManager.Despawn` di Fase 2.
5. Objek tidak boleh menjadi anak objek di scene region atau scene fase. Kalau perlu menempel, ikuti posisinya (`follow`), seperti `PooledEffect` (`VFXManager.md`). Pengecualian: objek UI boleh dipasang di canvas sementara, karena `ReturnAllActive` selalu berjalan sebelum scene diganti.

### `PooledObject` (komponen pembantu)

Ditambahkan otomatis oleh `ObjectPooler` ke setiap objek yang dibuat. Komponen ini bukan `IPoolable`, dan tidak boleh ditambahkan manual ke prefab.

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `PoolTag` | `string` | Dari `Pool.tag` | Tag asal. Dipakai `ReturnToPool` |
| `IsInPool` | `bool` | `true` | Penanda cepat untuk menolak pengembalian ganda |
| `activeIndex` | `int` | -1 | Indeks di daftar aktif, untuk penghapusan swap-remove |
| `poolables` | `IPoolable[]` | Diisi sekali saat dibuat | Hasil `GetComponentsInChildren<IPoolable>(true)`. Tidak dicari ulang setiap spawn |
| `SpawnCount` | `int` | 0 | Bertambah setiap keluar. Pemilik boleh memakainya sebagai generasi |

### Variabel `ObjectPooler`

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Instance` | `ObjectPooler` | `null` | Singleton |
| `pools` | `List<Pool>` | Diisi di prefab `PersistentRoot` | Daftar pool. Tabel nilai awal ada di bawah |
| `poolDictionary` | `Dictionary<string, Stack<PooledObject>>` | Kosong | Objek nonaktif per tag. `Stack` dipakai supaya objek yang baru kembali dipakai lebih dulu, sehingga cache CPU lebih hangat. Perilakunya sama dengan antrean |
| `activeByTag` | `Dictionary<string, List<PooledObject>>` | Kosong | Objek aktif per tag |
| `poolByTag` | `Dictionary<string, Pool>` | Kosong | Definisi per tag |
| `createdCount` | `Dictionary<string, int>` | 0 | Jumlah objek yang pernah dibuat dan masih ada |
| `warnedTags` | `HashSet<string>` | Kosong | Peringatan pool habis hanya dicatat sekali per tag per run |
| `returnBuffer` | `List<PooledObject>` | Kapasitas 256 | Salinan daftar aktif untuk `ReturnAllActive` |
| `IsInitialized` | `bool` | `false` | `Initialize` sudah selesai |
| `isQuitting` | `bool` | `false` | Diisi `OnApplicationQuit`. Semua panggilan diabaikan setelahnya |

### Nilai awal daftar `pools`

Ukuran pool adalah data, dan berada di prefab `PersistentRoot`, bukan di logika. Kalau blueprint pemilik menyebut angka, angka itu yang dipakai. Angka bertanda usulan belum ditulis di blueprint pemiliknya.

| Tag | `size` | `overflow` | Sumber angka |
|---|---|---|---|
| `zombie_standar` | 50 | `Fixed` | `SpawnManager.md`. Zombie kapal (#75) memakai pool yang sama |
| `zombie_desa` | 30 | `Fixed` | `SpawnManager.md`, `SunlightExposure.md` |
| `zombie_tribal`, `zombie_lumut`, `zombie_militer`, `zombie_beku` | 12 per tag | `Fixed` | `SpawnManager.md` |
| `hewan_serigala`, `hewan_serigala_salju`, `hewan_anjing` | 8 per tag | `Fixed` | `SpawnManager.md` |
| `hewan_gagak` | 16 | `Fixed` | `SpawnManager.md` |
| `hewan_babi`, `hewan_jaguar`, `hewan_ular`, `hewan_beruang` | 4 per tag | `Fixed` | `SpawnManager.md` |
| Semua tag `VFX_*` | Kolom "Isi pool" | `Fixed` | `VFXManager.md`. Tidak boleh lebih kecil dari `maxActive` |
| `FX_FireBurn`, `FX_FireArea` | 24, 8 | `Fixed` | `FireSystem.md` |
| `UI_DamageIndicator` | 6 | `Fixed` | `GameBalanceConfig.combatFeedback.maxIndicators` (#51) |
| `ShipLoot_Generic` | Jumlah semua titik `LootSpawnerSystem` di `ShipPhase` | `Fixed` | `LootSpawnerSystem.md`. Satu titik paling banyak satu objek |
| `ShipNpc_Generic` | 3 | `Fixed` | `ShipInteractable.md` (usulan di sana) |
| `Fx_Snow`, `Fx_Blizzard` | 2 per tag | `Fixed` | **Usulan** (`WeatherManager.md`, #115). Satu untuk cuaca aktif dan satu untuk crossfade |
| `WorldItem_Generic` | 128, `maxSize` 512 | `Grow` | **Usulan.** Item tanah jumlahnya sulit ditebak, dan dibatasi `maxLooseWorldItems` (#66) |
| `Audio_OneShot3D` | 16 | `Fixed` | **Usulan** (#52) |
| `Debris_Generic` | 8 | `Fixed` | **Usulan** (#106) |
| `Supply_BeachCrate` | `beachSupply.crateCountMax` | `Fixed` | **Usulan** (#71) |
| `Hazard_Fireball`, `Hazard_ImpactMarker` | 8, 8 | `Fixed` | **Usulan** (#70) |
| `FX_CollapseDebris`, `FX_RepairHit`, `FX_UpgradeDust` | 8, 4, 4 | `Fixed` | **Usulan** (#79, #47) |
| `Tribal_Snare`, `Thrown_Rock` | 6, 4 | `Fixed` | **Usulan** (#25, #94) |
| Tag proyektil lempar (#96) | 4 per tag | `Fixed` | **Usulan** |

**Perkiraan total:** sekitar 250 musuh dan 400 objek kecil. Angka ini masuk batas memori `streaming.baseMemoryLimitMB` (1.000 MB) milik `RegionStreaming.md`, yang sudah menghitung pool.

**Tidak ada angka di `GameBalanceConfig`.** Ukuran pool adalah kapasitas teknis, bukan angka keseimbangan permainan, dan tidak berubah saat bermain. Batas jumlah hidup yang memengaruhi permainan tetap ada di config pemiliknya, misalnya `spawn.maxZombiesAlive` dan `vfx.catalog.maxActive`.

## 2. Detail Logika & Fungsi

### `public void Initialize()`

Menggantikan `Start()`. Dipanggil sekali oleh `RunLifecycle.Boot()`.

1. Buat `PoolStaging` (nonaktif) dan `PoolStorage` (aktif) sebagai anak `ObjectPooler`, kalau belum ada.
2. Untuk tiap `Pool` di `pools`, jalankan `ValidatePool(pool)`. Pool yang gagal dilewati dengan kesalahan di log. Boot tetap berlanjut.
3. Isi `poolByTag`, buat `Stack` dan `List` kosong dengan kapasitas `size`.
4. Buat objek sebanyak `size` lewat `CreateObject(pool)`.
5. `IsInitialized = true`. Catat ringkasan di build pengembangan: jumlah tag dan jumlah objek.

**`private bool ValidatePool(Pool p)`** menolak:

- `tag` kosong, atau tag yang sudah dipakai pool lain.
- `prefab` kosong.
- `size < 1`.
- `overflow == Grow` dengan `maxSize` di antara 1 dan `size`. Nilai 0 diubah menjadi `size`.

Prefab tanpa `IPoolable` tetap diterima, tetapi dicatat sebagai peringatan, karena objek itu tidak bisa mereset dirinya.

### `private PooledObject CreateObject(Pool p)`

1. `var go = Instantiate(p.prefab, PoolStaging)`. Karena induknya nonaktif, `Awake` dan `OnEnable` milik prefab **belum** berjalan. Ini mencegah `NavMeshAgent` mencari NavMesh di scene `Bootstrap`, dan mencegah langganan event saat boot.
2. `go.SetActive(false)`, lalu `go.name = tag + " (" + nomor + ")"`.
3. Tambahkan `PooledObject`, isi `PoolTag`, `IsInPool = true`, dan `poolables`.
4. `go.transform.SetParent(PoolStorage, false)`. Objek tetap nonaktif, karena `activeSelf` bernilai `false`.
5. Masukkan ke `poolDictionary[tag]`, lalu `createdCount[tag]++`.

`Awake` milik prefab baru berjalan saat objek pertama kali keluar dari pool. Karena itu, `Awake` pada prefab pool hanya boleh mengambil komponen sendiri.

### `public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)`

1. Kalau `isQuitting`, kembalikan `null`.
2. Kalau `!IsInitialized`, catat kesalahan dan kembalikan `null`.
3. Kalau tag tidak ada di `poolByTag`, catat peringatan dan kembalikan `null`.
4. Ambil objek dari `poolDictionary[tag]`. Kalau kosong:
   - `Fixed`: kembalikan `null`. Di build pengembangan, catat sekali per tag lewat `warnedTags`. Tidak ada log di build rilis, karena `SpawnManager` dan `VFXManager` memang mengandalkan `null` sebagai batas.
   - `Grow`: kalau `createdCount[tag] < maxSize`, panggil `CreateObject` lalu ambil objek itu, dan catat peringatan sekali per tag. Kalau sudah `maxSize`, kembalikan `null`.
   - **Objek yang masih aktif tidak pernah diambil paksa.** Pemakaian ulang objek aktif hanya boleh dilakukan pemiliknya, misalnya `CombatFeedbackUI`.
5. Kalau objek yang diambil sudah `null` (dihancurkan dari luar), kurangi `createdCount[tag]`, catat peringatan, lalu ulangi langkah 4.
6. `transform.SetPositionAndRotation(position, rotation)`. Skala tidak diubah di sini, karena `OnSpawned` yang mengembalikannya.
7. `IsInPool = false`, `SpawnCount++`, lalu tambahkan ke `activeByTag[tag]` dan isi `activeIndex`.
8. `SetActive(true)`.
9. Panggil `OnSpawned()` pada tiap `poolables`. Kesalahan di satu komponen ditangkap dengan `try/catch`, dicatat, dan komponen lain tetap dipanggil.
10. **Pengecekan kembali di tengah spawn:** kalau `IsInPool` sudah `true` setelah langkah 9, berarti `OnSpawned` sendiri memanggil `ReturnToPool`. Contohnya `EnemyAI.OnSpawned`, yang mengembalikan zombie kalau tidak menemukan NavMesh. Kembalikan `null` ke pemanggil.
11. Kembalikan `go`.

Fungsi pembantu `public T SpawnFromPool<T>(string tag, Vector3 position, Quaternion rotation) where T : Component` memanggil fungsi di atas, lalu `GetComponent<T>()`. Kalau komponen tidak ada, objek dikembalikan ke pool dan hasilnya `null`.

### `public void ReturnToPool(string tag, GameObject obj)`

1. Kalau `isQuitting` atau `obj == null`, abaikan.
2. Ambil `PooledObject`. Kalau tidak ada, objek ini bukan dari pool. Catat kesalahan, lalu `obj.SetActive(false)`. Objek tidak dihancurkan, supaya kesalahan mudah dilihat di Hierarchy.
3. Kalau `IsInPool` sudah `true`, abaikan. Objek tidak boleh masuk dua kali.
4. Kalau `tag` berbeda dengan `PoolTag`, catat peringatan dan pakai `PoolTag`. Tag dari pemanggil hanya dipakai untuk pengecekan.
5. `IsInPool = true`. Tanda ini diisi lebih dulu, supaya panggilan ulang dari dalam `OnReturned` diabaikan.
6. Panggil `OnReturned()` pada tiap `poolables`, dengan `try/catch` per komponen.
7. `SetActive(false)`.
8. Lepas dari `activeByTag` dengan swap-remove lewat `activeIndex`. Perbarui `activeIndex` objek yang dipindah, lalu atur `activeIndex = -1`.
9. `transform.SetParent(PoolStorage, false)`, lalu `localScale` kembali ke skala prefab. Skala disimpan saat `CreateObject`.
10. `Push` ke `poolDictionary[PoolTag]`.

Overload `public void ReturnToPool(GameObject obj)` memakai `PoolTag` dari `PooledObject`. Pemanggil baru disarankan memakai overload ini.

### `public void ReturnAllActive()`

Dipanggil `RunLifecycle.ResetRunState()` langkah 4, sebelum scene diganti. `RunLifecycle.EnterIslandPhase()` langkah 1 juga memanggilnya selagi `ShipPhase` masih ada, supaya zombie, loot, dan NPC kapal tidak terbawa ke pulau. Juga dipakai `DevConsole` lewat jalur `SpawnManager`.

1. Salin semua objek aktif dari semua tag ke `returnBuffer`. Salinan dipakai karena `OnReturned` satu objek bisa mengembalikan objek lain.
2. Untuk tiap objek di `returnBuffer`:
   - Kalau `null`, catat peringatan "objek pool dihancurkan dari luar", kurangi `createdCount`, dan hapus dari daftar aktif.
   - Kalau masih aktif (`IsInPool == false`), panggil `ReturnToPool(obj)`.
3. Kosongkan `returnBuffer`.

### `public void ResetForNewRun()`

Dipanggil `RunLifecycle` langkah 10, setelah `ReturnAllActive` dan pergantian scene.

1. Kalau masih ada objek aktif, catat peringatan berisi tag dan jumlahnya, lalu jalankan `ReturnAllActive()`.
2. Untuk pool `Grow`, kalau `createdCount > size`, hancurkan kelebihan objek nonaktif sampai kembali ke `size`. Ini aman karena layar sedang `Loading`, dan menjaga jumlah objek tetap sama antar-run.
3. Untuk pool yang kekurangan objek, misalnya karena ada yang dihancurkan dari luar, buat ulang sampai `size`.
4. Kosongkan `warnedTags`.

### Fungsi baca

| Fungsi | Pemakai | Isi |
|---|---|---|
| `bool HasPool(string tag)` | `SpawnManager.ValidatePools`, `VFXManager` | Tag terdaftar |
| `int GetPoolSize(string tag)` | `SpawnManager.ValidatePools` (#62), `VFXManager` (#15) | `Pool.size`, atau 0 kalau tidak ada |
| `int CountInactive(string tag)` | `DevConsole` (#56) | Jumlah di `poolDictionary` |
| `int CountActive(string tag)` | `DevConsole` (#56), tes | Jumlah di `activeByTag` |
| `int TotalActive { get; }` | `RunLifecycle` langkah 7, tes | Jumlah semua objek aktif |

**Usulan:** perintah konsol `poolstat` menampilkan `tag aktif/nonaktif/dibuat` per pool, memakai fungsi di atas.

### Pemeriksaan Editor

**`OnValidate`** di prefab `PersistentRoot` mencatat peringatan untuk tag ganda, prefab kosong, `size < 1`, dan prefab yang punya `PooledObject` manual. Pemeriksaan ukuran terhadap batas permainan tetap milik pemilik pool: `SpawnManager.ValidatePools()` dan `VFXManager`.

## 3. Hubungan dengan Sistem Lain

**Event:** tidak mengirim dan tidak mendengar event. Pemanggilan langsung `SpawnFromPool` diizinkan `GameEventBus.md`, karena pemanggil butuh hasilnya saat itu juga.

| Sistem | Hubungan |
|---|---|
| `RunLifecycle` (#82) | Memanggil `Initialize`, `ReturnAllActive`, dan `ResetForNewRun`. Membaca `TotalActive` untuk pemeriksaan kebocoran |
| `VFXManager` (#15) | Berjalan tepat setelah `ObjectPooler` pada urutan bootstrap. Memakai pool `VFX_*` |
| `SpawnManager` (#62) | Memanggil `SpawnFromPool(enemyId, ...)` dan `ReturnToPool` lewat `Despawn`. Membaca `GetPoolSize` di `ValidatePools` |
| `ShipHazardManager` (#75), `EnemyAI` | Zombie kapal memakai pool `zombie_standar`, dan kembali lewat `ReturnToPool` karena `SpawnManager` tidak ada di `ShipPhase` |
| `WorldItemManager` (#66), `LootSpawnerSystem` | Pool `WorldItem_Generic` dan `ShipLoot_Generic` |
| `CombatFeedbackUI` (#51), `AmbientAudio` (#52), `FireSystem` (#97), `DisasterManager` (#70), `BeachSupplyManager` (#71), `ThrowableSystem` (#96), `SpecialZombieAI` (#25), `AITraversal` (#94), `DebrisRetrieval` (#106), `BuildMenu` (#79), `BuildingIntegrity` (#47) | Memanggil `SpawnFromPool` dan `ReturnToPool` untuk pool masing-masing |
| `DevConsole` (#56) | Membaca jumlah objek. `killall` memakai jalur `SpawnManager` |

## 4. Kasus Khusus

- **Reset objek pool:** setiap objek wajib mengembalikan HP, state, skala, dan komponen lewat `OnSpawned` dan `OnReturned`. `ObjectPooler` sendiri hanya mengembalikan induk, posisi, dan skala.
- **Pool habis:** `Fixed` mengembalikan `null`, dan pemanggil wajib menerima `null` tanpa error. `SpawnManager` mencoba lagi pada tick berikutnya. `VFXManager` membuang efek tanpa log. `Grow` membuat objek baru sampai `maxSize`.
- **`OnSpawned` mengembalikan objek ke pool:** `SpawnFromPool` mengembalikan `null` (langkah 10). Pemanggil tidak menerima objek nonaktif.
- **Kesalahan di `OnSpawned` atau `OnReturned`:** ditangkap per komponen. Objek tetap masuk pool, jadi jumlah objek tidak bocor.
- **Game dijeda:** `SpawnFromPool` dan `ReturnToPool` tetap bisa dipanggil, karena tidak memakai waktu. Pemanggilnya yang berhenti saat `Time.timeScale = 0`. `ReturnAllActive` saat reset berjalan ketika panel `Loading` menjeda waktu, dan tetap bekerja.
- **Permadeath:** pool tidak dibuat ulang. Objek hanya dikembalikan, sehingga jumlah objek tetap. Pool `Grow` dipangkas kembali ke `size`.
- **Save dan load:** isi pool tidak disimpan (`WorldStateSave.md`). Pemilik objek memunculkan ulang dari datanya sendiri, misalnya `WorldItemManager` dan `LootSpawnerSystem`.
- **Objek ikut scene region:** pengembalian dilakukan pemilik objek saat `Unloading` (#88), sebelum scene lepas. Karena objek pool tidak pernah menjadi anak objek region, scene yang lepas tidak menghancurkannya.
- **Objek dihancurkan dari luar:** daftar aktif dibersihkan dari referensi `null` saat `ReturnAllActive`, disertai peringatan. `ResetForNewRun` membuat ulang objek yang hilang.
- **Objek UI di canvas scene:** saat scene diganti, `ReturnAllActive` sudah memindahkannya ke `PoolStorage`, jadi tidak ikut hancur.
- **`managersRoot.SetActive(false)` saat reset (#82):** objek di `PoolStorage` sudah nonaktif, jadi tidak ada `OnEnable` atau `OnDisable` tambahan.
- **Menutup game:** `OnApplicationQuit` mengisi `isQuitting`. `OnDisable` atau `OnDestroy` objek lain yang memanggil `ReturnToPool` saat game ditutup diabaikan, tanpa error.
- **Tes dari scene lain di Editor:** `EditorQuickStart` (#82) menjalankan `Initialize`, jadi pool sudah siap sebelum objek scene memanggil `SpawnFromPool`.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** pool di `PersistentRoot`, dan `ReturnAllActive` sebelum scene diganti (`RunLifecycle.md`). Ukuran pool musuh dan `null` saat pool habis (`SpawnManager.md`). Ukuran pool efek dan pembuangan efek tanpa log (`VFXManager.md`). Objek aktif tidak pernah dipakai ulang oleh pooler (`VFXManager.md`, `CombatFeedbackUI.md`).

**Keputusan teknis di blueprint ini:**

- Objek dibuat di bawah induk nonaktif, supaya `Awake` dan `OnEnable` tidak berjalan di scene `Bootstrap`.
- Komponen `PooledObject` menyimpan tag, status, dan daftar `IPoolable`, supaya pengembalian ganda ditolak tanpa mencari di antrean.
- Pool `Grow` dipangkas ke `size` saat run baru.
- Ukuran pool berada di prefab `PersistentRoot`, bukan di `GameBalanceConfig`.

**Usulan yang menunggu developer (tidak menghalangi kode):** ukuran pool bertanda usulan di tabel nilai awal, dan perintah `poolstat`.

## 6. Skenario Tes

1. Boot game. **Hasil:** jumlah objek tiap pool sama dengan `size`, semuanya nonaktif di bawah `PoolStorage`, dan tidak ada peringatan `NavMeshAgent` di log.
2. Munculkan 10 zombie, lalu biarkan mati. **Hasil:** semua zombie kembali, dan `CountActive("zombie_standar")` bernilai 0.
3. Ulangi mati dan run baru lima kali. **Hasil:** jumlah objek pool tetap, dan `TotalActive` bernilai 0 setelah tiap reset.
4. Kembalikan objek yang sama dua kali. **Hasil:** `poolDictionary` hanya berisi satu, dan `OnReturned` hanya terpanggil sekali.
5. Pakai ulang zombie yang tadi mati. **Hasil:** HP, state, dan skala kembali ke nilai awal.
6. `spawn zombie_standar 100` lewat konsol. **Hasil:** berhenti di 50, fungsi mengembalikan `null` tanpa error, dan zombie yang hidup tidak berpindah tempat.
7. Naikkan `maxLooseWorldItems` lewat konsol, lalu jatuhkan 200 item di tanah. **Hasil:** pool `WorldItem_Generic` tumbuh dengan satu peringatan, lalu kembali ke 128 setelah run baru.
8. Munculkan zombie di titik tanpa NavMesh. **Hasil:** `EnemyAI.OnSpawned` mengembalikannya ke pool, dan `SpawnFromPool` mengembalikan `null`.
9. Panggil `SpawnFromPool("tag_salah", ...)`. **Hasil:** `null` dan satu peringatan, tanpa exception.
10. Hapus satu zombie aktif lewat Hierarchy, lalu mati. **Hasil:** peringatan "dihancurkan dari luar", dan run baru kembali punya 50 objek.
11. Panggil `ReturnToPool` pada objek yang bukan dari pool. **Hasil:** kesalahan di log, objek dimatikan, dan tidak masuk antrean mana pun.
12. Tempelkan efek pada zombie, lalu kembalikan zombie. **Hasil:** efek tidak ikut masuk pool zombie (`VFXManager.md`).
13. Buka `Pause`, lalu munculkan efek lewat konsol. **Hasil:** objek keluar dari pool, dan bergerak lagi setelah `Pause` ditutup.
14. Tutup game saat banyak objek aktif. **Hasil:** tidak ada error dari `ReturnToPool` di log.
15. Tembak 100 kali dengan Profiler terbuka. **Hasil:** tidak ada `Instantiate`, dan tidak ada alokasi GC dari `SpawnFromPool`.
