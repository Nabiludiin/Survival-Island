# Blueprint: SurfaceDetection.cs
**Lokasi:** `docs/blueprint/Core_Logic/SurfaceDetection.md`
**Deskripsi Utama:** Satu jawaban untuk pertanyaan "permukaan apa di titik ini", baik di Terrain maupun mesh biasa. Sistem ini menggantikan layer `Sand/Beach` sebagai dasar restriksi pantai.
**Issue:** #86
**Rujukan GDD:** Bagian 4 (restriksi pantai dan peringatan tidak tidur di pantai). Cara membaca permukaan dan aturan garis pantai adalah usulan teknis, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

### Perbandingan pilihan

| Hal | Membaca alphamap Terrain | Volume trigger khusus pantai |
|---|---|---|
| Sumber kebenaran | Tekstur yang dilihat pemain | Kotak yang diletakkan terpisah dari tekstur |
| Saat peta diubah | Bake ulang satu kali | Volume diatur ulang dengan tangan |
| Bentuk pantai berkelok | Mengikuti tekstur per sel | Butuh banyak kotak atau mesh khusus |
| Jenis permukaan lain | Rumput, salju, dan lumpur ikut terbaca | Tiap jenis butuh volume sendiri |
| Dipakai suara langkah dan efek peluru | Ya | Tidak cocok, karena hanya untuk pantai |
| Query fisika | Satu raycast, atau tanpa raycast kalau XZ sudah diketahui | Harus memakai pengecualian `QueryTriggerInteraction` (#87) |
| Risiko | Pembacaan alphamap berat kalau tidak di-cache | Batas volume dan tekstur bisa tidak cocok |

**Pilihan: alphamap yang di-bake menjadi cache.** Pemain melihat batas pantai dari tekstur, jadi hologram merah harus mengikuti tekstur yang sama. Issue #21, #15, dan #91 juga memakai sumber ini.

### Pola

`SurfaceDetection` adalah manager persisten (`IBootstrapService`), karena `ShipPhase` dan `IslandPhase` sama-sama memakainya. Sistem ini tidak menyimpan status run. Isinya hanya daftar Terrain yang sedang aktif.

```csharp
public enum SurfaceType : byte { Pasir, Rumput, Tanah, Batu, Salju, Lumpur, Kayu, Logam }

public struct SurfaceSample
{
    public SurfaceType type;     // Jenis dominan
    public float sandWeight;     // 0 sampai 1. Selalu 0 di mesh
    public float groundHeight;   // Tinggi Y titik kena
    public bool isTerrain;       // true kalau berasal dari Terrain
}

public class SurfaceTag : MonoBehaviour      // Dipasang pada mesh, bangunan, dan lantai kapal
{
    public SurfaceType type = SurfaceType.Tanah;
}

public class TerrainSurfaceSource : MonoBehaviour   // Dipasang pada tiap Terrain
{
    public SurfaceMapAsset map;
    // OnEnable: SurfaceDetection.Instance.Register(this)
    // OnDisable: SurfaceDetection.Instance.Unregister(this)
}
```

### Data bake

```csharp
[CreateAssetMenu(menuName = "Survival/Surface Layer Map")]
public class SurfaceLayerMap : ScriptableObject
{
    public TerrainLayer[] layers;
    public SurfaceType[] types;     // Pasangan per indeks
}

public class SurfaceMapAsset : ScriptableObject
{
    public int width, height;
    public byte[] dominant;         // Nilai SurfaceType per sel
    public byte[] sand;             // Bobot pasir per sel, 0 sampai 255
    public Hash128 sourceHash;      // Dari alphamap, untuk mendeteksi data lama
}
```

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Instance` | `SurfaceDetection` | `null` | Singleton persisten |
| `sources` | `List<TerrainSurfaceSource>` | Kosong, kapasitas 4 | Terrain aktif. Paling banyak dua region dimuat bersamaan (#88) |
| `byCollider` | `Dictionary<Collider, TerrainSurfaceSource>` | Kosong | Pencarian sumber dari hasil raycast |

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.surface.probeUp` | 1 | Jarak titik awal raycast di atas posisi yang ditanya |
| `GameBalanceConfig.surface.probeDown` | 3 | Jarak raycast ke bawah |
| `GameBalanceConfig.surface.beachBuildSandWeight` | 0,35 | Bobot pasir yang sudah melarang bangunan |
| `GameBalanceConfig.surface.nearSandSampleCount` | 8 | Jumlah titik pada lingkaran `IsNearSand` |
| `GameBalanceConfig.surface.runtimeBakeRowsPerFrame` | 64 | Baris alphamap per frame saat bake cadangan |
| `GameBalanceConfig.surface.debugGridCells` | 64 | Ukuran kisi tampilan debug |
| `GameBalanceConfig.surface.debugGridSpacing` | 1 | Jarak antar-titik tampilan debug, dalam meter |

## 2. Detail Logika & Fungsi

### Cache alphamap

`TerrainData.GetAlphamaps` mengembalikan `float[,,]`. Untuk peta 1024 x 1024 dengan 8 layer, ukurannya sekitar 32 MB, dan pemanggilannya menghasilkan garbage besar. Karena itu, game tidak membacanya saat bermain.

**`SurfaceBakeTool`** adalah alat Editor. Untuk tiap Terrain:

1. Baca seluruh alphamap satu kali.
2. Jumlahkan bobot per `SurfaceType` di tiap sel lewat `SurfaceLayerMap`. Dua tekstur rumput dihitung sebagai satu `Rumput`.
3. Simpan jenis terbesar ke `dominant`. Kalau seri, ambil urutan layer yang lebih awal.
4. Simpan jumlah bobot `Pasir` ke `sand`, dikali 255.
5. Simpan `sourceHash`, lalu simpan aset di samping scene region.

Hasilnya sekitar 2 MB per Terrain, dan ikut dilepas bersama region. Terrain tidak berubah saat bermain, jadi cache tidak perlu diperbarui.

**Cadangan saat bermain:** kalau `map` kosong atau ukurannya tidak cocok, `Register` mencatat peringatan dan menjalankan coroutine yang membaca `runtimeBakeRowsPerFrame` baris per frame. Selama belum selesai, sumber itu memakai `Tanah`. Build rilis harus lulus `SurfaceAuditWindow`, sehingga cadangan ini hanya untuk pengembangan.

### Fungsi utama

**`public SurfaceType GetSurfaceAt(Vector3 worldPos)`** memanggil `TryGetSurfaceAt`, lalu mengembalikan `sample.type`. Kalau tidak ada tanah, hasilnya `Tanah`.

**`public bool TryGetSurfaceAt(Vector3 worldPos, out SurfaceSample sample)`**:

1. Buat ray dari `worldPos + Vector3.up * probeUp` ke bawah sejauh `probeUp + probeDown`.
2. `PhysicsQuery.Raycast(ray, out hit, distance, PhysicsMasks.Ground)`. Kalau gagal, kembalikan `false`.
3. Kembalikan hasil `ReadFromHit(in hit, out sample)`.

**`public SurfaceType GetSurfaceFromHit(in RaycastHit hit)`** dipakai #15, karena peluru sudah punya hasil raycast. Fungsi ini tidak menembakkan raycast baru.

**`private bool ReadFromHit(in RaycastHit hit, out SurfaceSample sample)`**:

1. Kalau `byCollider` memuat `hit.collider`, baca peta Terrain pada `hit.point`.
2. Kalau tidak, cari `SurfaceTag` di collider atau parent-nya, lalu pakai `type`, dengan `sandWeight = 0`.
3. Kalau tidak ada keduanya, pakai `Tanah`. `SurfaceAuditWindow` melaporkan collider ini.

**Membaca peta Terrain:**

```csharp
int CellIndex(TerrainSurfaceSource s, Vector3 p)
{
    float u = Mathf.Clamp01((p.x - s.origin.x) / s.size.x);
    float v = Mathf.Clamp01((p.z - s.origin.z) / s.size.z);
    int x = Mathf.RoundToInt(u * (s.map.width - 1));
    int y = Mathf.RoundToInt(v * (s.map.height - 1));
    return y * s.map.width + x;
}
```

Fungsi ini tidak mengalokasi memori. `origin` dan `size` disimpan saat `Register`, bukan dibaca dari `Terrain` tiap kali.

**`public bool TryGetTerrainSurfaceAtXZ(Vector3 worldPos, out SurfaceSample sample)`** mencari di `sources` lewat batas XZ, tanpa raycast. Fungsi ini mengabaikan benda di atas tanah, sehingga batu atau lantai di atas pasir tetap terbaca sebagai pasir.

### Campuran tekstur di garis pantai

Satu titik memakai **nilai dominan**, supaya suara langkah dan kecepatan jalan tidak berkedip. Hanya saja, nilai dominan saja membuat pondasi bisa menempel di pasir. Bangunan memakai aturan lebih ketat:

**`public bool IsBeachFootprint(Vector3 center, Vector3 halfExtents, Quaternion rotation)`**:

1. Ambil lima titik: tengah dan empat sudut tapak di bidang XZ.
2. Untuk tiap titik, panggil `TryGetTerrainSurfaceAtXZ`.
3. Kalau `sample.type == Pasir`, atau `sample.sandWeight >= beachBuildSandWeight`, kembalikan `true`.
4. Titik di luar semua Terrain dilewati. Pemeriksaan permukaan di `BuildManager` yang menolaknya.

**`public bool IsNearSand(Vector3 worldPos, float radius)`** memeriksa titik tengah dan `nearSandSampleCount` titik pada lingkaran. Dipakai `SleepSystem` (#67) dan bisa dipakai #91.

**`public float GetSandWeight(Vector3 worldPos)`** mengembalikan bobot pasir di XZ, atau 0 di luar Terrain.

### Aturan anti-pantai di `BuildManager`

Aturan 1 di `ValidatePlacement` berubah menjadi:

```csharp
if (SurfaceDetection.Instance.IsBeachFootprint(pos, ghostHalfExtents, ghostRotation))
    return PlacementError.Beach;
```

Layer `Sand/Beach` tidak dipakai lagi (`PhysicsLayers.md`). Urutan lengkap validasi ada di `raft/BuildMenu.md` (#79).

### Data untuk sistem lain

| Sistem | Fungsi | Kebutuhan |
|---|---|---|
| `FootstepAudio` (#21) | `GetSurfaceAt(posisi kaki)` | Pilih SFX per `SurfaceType`, termasuk `Kayu` dan `Logam` di kapal |
| `VFXManager` (#15) | `GetSurfaceFromHit(hit)` | Efek impact peluru per `SurfaceType`. Tembakan ke `EnemyHitbox` memakai `EnemyHitEvent`, bukan fungsi ini |
| `TideManager` (#91) | `TryGetTerrainSurfaceAtXZ`, `GetSandWeight`, `IsNearSand` | Area pantai dan tinggi tanah untuk menentukan titik yang terendam |
| `PlayerTraversal` (#101) | `GetSurfaceAt` | Pengali kecepatan dan damage jatuh |
| `BuildManager`, `BuildMenu` (#79) | `IsBeachFootprint` | Restriksi pantai |
| `SleepSystem` (#67) | `IsNearSand` | Hukuman tidur di pantai |

### Mesh dan bangunan

| Objek | `SurfaceType` |
|---|---|
| Lantai dan dinding kapal | `Kayu` atau `Logam`, lewat `SurfaceTag` |
| Bangunan kayu pemain | `Kayu`, lewat `SurfaceTag` di prefab |
| Bangunan batu pemain (#47) | `Batu`, lewat `SurfaceTag` di prefab |
| Batu besar dan tebing | `Batu`, lewat `SurfaceTag` |
| Batang pohon | `Kayu`, lewat `SurfaceTag` |

### Tampilan debug

- **Di Editor:** `SurfaceAuditWindow` menampilkan `SurfaceMapAsset` sebagai tekstur warna, satu warna per `SurfaceType`, dengan garis merah pada sel yang `sand >= beachBuildSandWeight`. Garis merah ini adalah batas pantai yang benar-benar dipakai bangunan.
- **Saat bermain:** perintah `surfacedebug 1` di `DevConsole` (#56) menyalakan `SurfaceDebugOverlay`. Komponen ini menggambar kisi `debugGridCells` x `debugGridCells` di sekitar pemain dengan `GL.LINES`, ditambah teks seperti `Pasir (0,62)` untuk titik di bawah pemain.
- **Pemeriksaan audit:** `TerrainLayer` tanpa pasangan di `SurfaceLayerMap`, `sourceHash` yang sudah berbeda, Terrain tanpa `TerrainSurfaceSource`, dan collider di `PhysicsMasks.Ground` tanpa `SurfaceTag`.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** tidak ada.

**Event yang didengar:** tidak ada. `TerrainSurfaceSource` mendaftar sendiri lewat `OnEnable` dan `OnDisable`, jadi region yang dimuat atau dilepas (#88) langsung tercermin.

| Sistem | Hubungan |
|---|---|
| `PhysicsLayers` (#87) | `PhysicsQuery.Raycast` dan `PhysicsMasks.Ground` |
| `RegionStreaming` (#88) | Menentukan Terrain mana yang aktif |
| `RunLifecycle` (#82) | Memanggil `Initialize()` saat boot |
| `DevConsole` (#56) | Perintah `surfacedebug` |
| Pemakai di tabel "Data untuk sistem lain" | Memanggil fungsi publik secara langsung, sesuai aturan pemanggilan dengan jawaban langsung di `GameEventBus.md` |

**Batas pekerjaan:** suara langkah di #21, efek impact di #15, dan aturan air pasang di #91.

## 4. Kasus Khusus

- **Objek pool:** tidak memakai pool. Objek pool boleh memanggil fungsi ini saat `OnSpawned`, dan tidak boleh menyimpan hasilnya melewati `OnReturned`.
- **Game dijeda:** semua fungsi tetap menjawab, karena tidak memakai waktu. Tidak ada pemanggil yang berjalan saat input mati. Bake cadangan berjalan per frame, tanpa bergantung pada `Time.timeScale`.
- **Permadeath:** tidak ada data run. `ResetForNewRun()` hanya mengosongkan `sources` dan `byCollider`, dan Terrain baru mendaftar saat scene dimuat.
- **Save dan load:** tidak ada data yang disimpan. Cache berasal dari aset.
- **Region belum dimuat:** `TryGetSurfaceAt` dan `TryGetTerrainSurfaceAtXZ` mengembalikan `false`. Pemanggil yang butuh kepastian, seperti `BuildManager`, menolak dengan `PlacementError.UnknownSurface`.
- **Dua Terrain bertemu di seam:** pencarian berhenti pada Terrain pertama yang memuat titik itu. Pembuat peta harus memakai jenis permukaan yang sama pada dua sisi seam, dan audit memeriksanya.
- **Pemain berdiri di bangunan di atas Terrain:** raycast mengenai `Structure`, jadi hasilnya mengikuti `SurfaceTag` bangunan.
- **Kapal miring:** ray tetap lurus ke bawah dunia, dan lantai kapal memakai `SurfaceTag`, bukan Terrain.
- **Titik di dalam air:** fungsi ini hanya melaporkan dasar air. Air pasang dan kedalaman milik #91 dan #13.
- **Peta diubah setelah bake:** `sourceHash` berbeda dan audit gagal, sehingga batas pantai lama tidak diam-diam terbawa ke build.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** layer `Sand/Beach` dihapus, dan `SurfaceType` memakai delapan nilai (`GLOSSARY.md`).

**Keputusan teknis di blueprint ini:**

- Alphamap di-bake di Editor menjadi `byte[]`, supaya pembacaan di game tanpa garbage.
- Bobot dihitung per `SurfaceType`, bukan per tekstur.
- `IsBeachFootprint` membaca Terrain langsung lewat XZ, supaya benda di atas pasir tidak menutupi pasir.

**Usulan yang disetujui developer sebagai nilai awal (dicatat di #85):**

1. **Sumber deteksi:** alphamap yang di-bake, bukan volume trigger.
2. **Campuran tekstur:** nilai dominan untuk suara dan gerak. Untuk bangunan, titik dilarang kalau bobot pasir mencapai 35 persen pada salah satu dari lima titik tapak.
3. **Masa hidup manager:** persisten, karena dipakai dua fase.

## 6. Skenario Tes

1. Buat Terrain uji dengan pantai dan rumput pada satu Terrain, lalu jalankan `SurfaceBakeTool`. **Hasil:** tampilan audit menunjukkan garis merah mengikuti tekstur pasir.
2. Jalankan `surfacedebug 1`, lalu berjalan dari rumput ke pasir. **Hasil:** teks berubah dari `Rumput` ke `Pasir` di tempat tekstur berganti, dan bobot pasir naik bertahap.
3. Arahkan hologram lantai ke tengah pantai. **Hasil:** hologram merah dengan alasan `Beach`.
4. Arahkan hologram sehingga satu sudutnya menyentuh gradasi pasir di atas 35 persen. **Hasil:** tetap merah. Geser menjauh. **Hasil:** hijau.
5. Letakkan batu besar di pasir, lalu arahkan hologram ke atas batu. **Hasil:** tetap merah, karena pasir dibaca lewat XZ.
6. Berjalan di lantai kapal dengan `SurfaceTag` `Kayu`, lalu `Logam`. **Hasil:** `GetSurfaceAt` mengikuti tag.
7. Berjalan di atas lantai bangunan yang berdiri di rumput. **Hasil:** `GetSurfaceAt` menghasilkan `Kayu`.
8. Tembak pasir, batu, dan salju. **Hasil:** `GetSurfaceFromHit` menghasilkan jenis yang sesuai tanpa raycast tambahan.
9. Pindah sampai `Region1_Volcano` dilepas. **Hasil:** `sources` berkurang dan tidak ada referensi mati. Panggilan pada titik di region itu mengembalikan `false`.
10. Panggil `GetSurfaceAt` 1000 kali dalam satu frame sambil membuka Profiler. **Hasil:** tanpa alokasi GC, dan waktu tercatat di lembar playtest.
11. Hapus satu pasangan di `SurfaceLayerMap`, lalu buka audit. **Hasil:** `TerrainLayer` itu masuk laporan.
12. Hapus `map` dari satu Terrain, lalu tekan Play. **Hasil:** peringatan tercatat, bake cadangan selesai dalam beberapa frame, dan FPS tidak jatuh di atas 0,1 detik.
