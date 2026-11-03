# Blueprint: BuildManager.cs
**Lokasi:** `docs/blueprint/raft/BuildManager.md`
**Deskripsi Utama:** Sistem penempatan struktur: hologram, grid, validasi, dan pembuatan objek. Alur palu, data struktur, dan tabel validasi lengkap ada di `BuildMenu.md`.
**Issue:** #59, #79, #86, #88
**Rujukan GDD:** Bagian 4 (base building dan larangan membangun di pantai). Grid, tingkat, dan angka adalah usulan teknis, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

`BuildManager` adalah manager di scene `IslandPhase`.

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `buildables` | `BuildableData[]` | Dari Inspector | Semua struktur (#79) |
| `Mode` | `HammerMode` | `Repair` | Mode palu |
| `Selected` | `BuildableData` | `null` | Struktur terpilih |
| `ghostObject` | `GameObject` | `null` | Hologram aktif |
| `ghostYaw` | `int` | 0 | Rotasi hologram per 90 derajat |
| `LastError` | `PlacementError` | `NoSelection` | Hasil validasi terakhir |
| `ghostValidMat`, `ghostInvalidMat` | `Material` | Dari Inspector | Material hologram |

`structurePrefabs`, `woodCost`, `stoneCost`, `buildableSurfaceMask`, dan `obstacleMask` dihapus. Query memakai `PhysicsMasks.BuildSurface` dan `PhysicsMasks.BuildOverlap`.

**Pantai:** tidak memakai LayerMask. Deteksi lewat `SurfaceDetection.Instance.IsBeachFootprint` (#86).

**Angka penyetelan:** `GameBalanceConfig.build.gridSize` (2,5) dan `maxBuildDistance` (6). Sisanya ada di `BuildMenu.md`.

## 2. Detail Logika & Fungsi

### Hologram

**`void UpdateGhostPosition()`**:

1. Sembunyikan hologram saat `UIFocusStack.IsGameplayInputBlocked` bernilai `true` (#88), lalu selesai.
2. `PhysicsQuery.Raycast(ray, out RaycastHit hit, maxBuildDistance, PhysicsMasks.BuildSurface)`, supaya raycast tidak mengenai pemain, trigger, atau musuh.
3. Bulatkan XZ ke grid:

```csharp
snapPos.x = Mathf.Round(hit.point.x / gridSize) * gridSize;
snapPos.z = Mathf.Round(hit.point.z / gridSize) * gridSize;
```

4. `snapPos.y` mengikuti aturan tinggi di `BuildMenu.md`, memakai `heightSnap`, `levelHeight`, dan `maxFoundationHeightDelta`. `snapPos.y = hit.point.y` dihapus.
5. Rotasi hologram mengikuti `ghostYaw`, per 90 derajat.

### Validasi

**`PlacementError ValidatePlacement(...)`** berhenti pada kesalahan pertama, dengan urutan di tabel validasi `BuildMenu.md`. Overlap memakai `PhysicsQuery.OverlapBox`.

Aturan pantai:

```csharp
if (SurfaceDetection.Instance.IsBeachFootprint(pos, ghostHalfExtents, ghostRotation))
    return PlacementError.Beach;
```

Titik yang belum bisa dibaca menghasilkan `PlacementError.UnknownSurface`.

Aturan atap lama pindah ke `SupportRule.WallBelow` di `BuildMenu.md`.

### Membuat struktur

**`ExecuteBuild()`**, atau `TryExecuteBuild()` di `BuildMenu.md`:

1. Jalankan `ValidatePlacement` sekali lagi. Kalau gagal, selesai.
2. Kurangi tiap biaya lewat `InventoryManager`.
3. `Instantiate(prefab, ghostPos, ghostRot, RegionStreaming.Instance.GetStructureRoot(RegionStreaming.Instance.GetRegionAt(ghostPos)))` (#88).
4. Isi `StructureId` dari `StructureRegistry.NewStructureId()` (#59).
5. Daftarkan ke `StructureRegistry`, lalu terbitkan `StructureBuiltEvent`.

`BaseHealth` sudah ada di prefab struktur, jadi langkah menempelkannya dihapus (#59, #79).

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `BuildModeChangedEvent` dan `StructureBuiltEvent`.

**Event yang didengar:** tidak ada dari blueprint ini.

| Sistem | Hubungan |
|---|---|
| `BuildMenu`, `StructureRegistry` (#79) | Data, alur palu, dan grafik dukungan |
| `SurfaceDetection` (#86) | `IsBeachFootprint` dan `TryGetSurfaceAt` |
| `RegionStreaming` (#88) | `GetStructureRoot` dan `GetRegionAt` |
| `UIFocusStack` (#83) | `IsGameplayInputBlocked` |
| `DamagePipeline` (#59) | `BaseHealth` dan `StructureId` |
| `PhysicsLayers` (#87) | `PhysicsQuery` dan `PhysicsMasks` |

## 4. Kasus Khusus

- **Objek pool:** struktur memakai `Instantiate`, karena jarang dibuat dan hidup lama. Pengecualian ini mengikuti `DamagePipeline.md`.
- **Game dijeda:** `IsGameplayInputBlocked` bernilai `true`, jadi hologram tersembunyi dan klik tidak diproses.
- **Permadeath:** struktur hilang bersama save run (#82).
- **Save dan load:** struktur disimpan `StructureRegistry` (#79). Hologram, `Mode`, dan `Selected` tidak disimpan.
- **Region dilepas:** struktur menjadi anak `GetStructureRoot`, jadi nonaktif bersama regionnya (#88).
- **Musuh masuk setelah hologram diperiksa:** validasi diulang tepat sebelum struktur dibuat.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** larangan pantai (GDD bagian 4), `PlacementError` dan urutan validasi (`BuildMenu.md`), serta `BaseHealth` di prefab (`DamagePipeline.md`).

**Keputusan teknis di blueprint ini:** `BuildManager.md` hanya merangkum kontrak. Kalau ada perbedaan rincian, `BuildMenu.md` yang dipakai.

**Usulan yang disetujui developer sebagai nilai awal:** grid 2,5 meter dan jarak bangun 6 meter perlu playtest.

## 6. Skenario Tes

1. Pilih dinding, lalu bidik tanah keras. **Hasil:** hologram biru dan menempel grid.
2. Bidik pasir pantai. **Hasil:** hologram merah, `LastError = Beach`, dan klik tidak membuat struktur.
3. Bidik titik yang belum bisa dibaca. **Hasil:** `LastError = UnknownSurface`.
4. Buka panel saat mode bangun. **Hasil:** hologram hilang, dan muncul lagi saat panel ditutup.
5. Bangun dinding di tiap region. **Hasil:** dinding menjadi anak `GetStructureRoot` milik region itu, dan punya `StructureId`.
6. Pindah region sampai region lama dilepas. **Hasil:** dinding nonaktif, lalu kembali saat region dimuat.
7. Putar hologram dengan `BuildRotate`. **Hasil:** berputar per 90 derajat.
8. Serang dinding baru dengan zombie. **Hasil:** `BaseHealth` dari prefab menerima damage.

Belum diuji di Unity, karena blueprint ini baru rancangan.
