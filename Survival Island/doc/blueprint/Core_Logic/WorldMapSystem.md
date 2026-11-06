# Blueprint: WorldMapSystem.cs
**Lokasi:** `Survival Island/doc/blueprint/Core_Logic/WorldMapSystem.md`
**Deskripsi Utama:** Peta pulau yang terbuka saat dijelajahi, dengan penanda otomatis dan penanda buatan pemain.
**Issue:** #105
**Rujukan GDD:** Tidak dipakai, diputuskan developer. Disimpan sebagai arsip. Sistem lain tidak boleh bergantung pada blueprint ini.
**Status:** Tidak dipakai, diputuskan developer. Disimpan sebagai arsip.
**Rekomendasi pemakaian:** Sebaiknya dipakai dalam bentuk peta kertas di menu penuh, tanpa minimap. Rute bolak-balik Region 4 ke Region 3 sulit diingat, dan peta kertas tidak mengurangi suasana survival seperti minimap yang selalu tampil.

## 1. Arsitektur & Variabel Utama

- `WorldMapSystem` adalah manager di scene `IslandPhase`, dan hanya aktif kalau `GameBalanceConfig.map.enabled = true`.
- Kalau nonaktif, komponen mematikan diri di `OnEnable`, tidak mendaftar ke save, dan tombol `OpenMap` tidak melakukan apa pun. Kompas (#30) tetap berjalan.
- Gambar peta dibuat di Editor oleh `MapBakeWindow`, karena region additive (#88) tidak selalu dimuat. Tidak ada kamera peta saat bermain.

```csharp
public enum MarkerIcon { Base, Supply, Part, Helipad, Objective, PlayerPin, PlayerDanger, PlayerResource }
public enum MarkerSource { System, Player }

[Serializable]
public struct MapMarker
{
    public string id;           // part_2, supply, objective, player_3
    public Vector3 position;
    public MarkerIcon icon;
    public string labelKey;     // Kunci #104, boleh kosong
    public MarkerSource source;
    public bool showOnCompass;
    public bool showOnMap;
}

[Serializable]
public class WorldMapSaveData
{
    public string fogBits;          // Base64 dari fogGrid
    public MapMarker[] playerMarkers;
    public int[] revealedSheets;    // RegionType yang lembar petanya sudah ditemukan
    public int nextPlayerMarkerId;
}
```

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `mapBounds` | `Bounds` | Gabungan `RegionRecord.bounds` (#88) | Batas peta |
| `gridWidth`, `gridHeight` | `int` | Dihitung dari `mapBounds` | Ukuran fog |
| `fogGrid` | `ulong[]` | Semua 0 | 1 bit per sel. 1 berarti terbuka |
| `fogTexture` | `Texture2D` | R8, ukuran grid | Masker untuk shader panel |
| `fogDirty` | `bool` | `false` | Tekstur perlu diperbarui |
| `lastStampPosition` | `Vector3` | Posisi pemain | Mencegah stempel berulang |
| `revealedSheets` | `HashSet<RegionType>` | Kosong | Lembar peta yang ditemukan |
| `iconPool` | `MapMarkerIcon[]` | 32 ikon dibuat sekali | Ikon penanda di panel |

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.map.enabled` | `false` | Saklar sistem |
| `GameBalanceConfig.map.fogCellSize` | 8 | Ukuran sel, dalam meter |
| `GameBalanceConfig.map.fogRevealRadius` | 40 | Radius pandang saat berjalan |
| `GameBalanceConfig.map.fogRevealRadiusAmazon` | 25 | Radius di Region 2, karena hutannya rapat (GDD bagian 5) |
| `GameBalanceConfig.map.fogUpdateSeconds` | 1 | Jeda pemeriksaan stempel |
| `GameBalanceConfig.map.maxPlayerMarkers` | 8 | Batas penanda pemain |

## 2. Detail Logika & Fungsi

### Bentuk peta

**`WorldMapPanel : UIPanel`** memakai pengaturan `WorldMap` yang sudah ada di `UIFocusStack.md`: tidak menjeda, melepas kursor, dan memblokir input gameplay. Tombolnya `OpenMap` dan `ToggleMap` (`M`) dari #18.

**`public void OpenMap()`:**
1. Keluar kalau sistem nonaktif, atau `PlayerPinnedEvent.Last.isPinned` bernilai `true`.
2. Kalau `fogDirty`, tulis `fogGrid` ke `fogTexture`, lalu `Apply(false)`.
3. `UIFocusStack.Instance.Push(worldMapPanel)`. `Push` menolak dengan sendirinya kalau `Cinematic` atau `Loading` berada di puncak.
4. Isi ikon dari `CompassSystem.Instance.Markers`, ditambah panah pemain.

Tiga lapisan pada panel:
1. **Kertas kosong:** selalu tampil.
2. **Sketsa medan:** hasil bake, terlihat pada sel yang fog-nya terbuka.
3. **Garis dan nama tempat:** hasil bake per region, dan hanya tampil kalau lembar region itu sudah ada di `revealedSheets`.

### Area terbuka

**`Update`** menambah timer dengan `Time.deltaTime`. Setiap `fogUpdateSeconds`, kalau pemain sudah pindah lebih dari setengah `fogCellSize`, panggil `StampFog(playerPos)`.

**`private void StampFog(Vector3 pos)`:**
1. Pilih radius. Pakai `fogRevealRadiusAmazon` kalau `RegionChangedEvent.Last.to == Amazon`.
2. Ubah `pos` ke sel lewat `WorldToCell`.
3. Isi bit sel dalam lingkaran. Kalau ada bit yang berubah, `fogDirty = true`.
4. Tidak ada alokasi, karena hanya mengubah array `ulong`.

`fogTexture` hanya diperbarui saat panel terbuka, supaya biaya bermain tetap kecil. Panel yang sedang terbuka memperbaruinya setiap `fogUpdateSeconds`.

**`public Vector2 WorldToMap(Vector3 world)`** memakai `Mathf.InverseLerp` pada X dan Z `mapBounds`, lalu mengalikannya dengan ukuran `RectTransform` peta.

### Penanda

Data penanda berada di `MapMarkerRegistry`, kelas biasa milik `CompassSystem` (#30). Kompas harus tetap berjalan walaupun peta nonaktif, jadi registry tidak boleh berada di sistem ini.

```csharp
public class MapMarkerRegistry
{
    public string Add(Vector3 position, MarkerIcon icon);    // Membuat id otomatis
    public void Set(in MapMarker marker);                    // Membuat atau memperbarui
    public bool Remove(string id);
    public IReadOnlyList<MapMarker> All { get; }
}
```

`MarkerIcon`, `MapMarker`, dan `MapMarkerRegistry` ikut dikompilasi bersama `CompassSystem`, walaupun peta nonaktif. `CompassSystem.AddMarker`, `SetMarker`, dan `RemoveMarker` yang sudah dipakai #69 dan #71 meneruskan panggilan ke registry. Setiap perubahan menerbitkan `MapMarkerChangedEvent { markerId, isRemoved }`.

**Penanda otomatis:**

| Penanda | Sumber | Kapan |
|---|---|---|
| Kargo | `BeachSupplyManager` (#71), `MarkerIcon.Supply` | Selama kargo tertutup |
| Suku cadang | `HeavyItemSystem` (#69), `part_<partId>` | `isDiscovered` dan `Lying` |
| Tujuan | `CompassSystem` dari `ObjectiveMarkerChangedEvent` (#81), `objective` | Selama ada penanda tujuan |
| Base | Sistem ini, `base` | `PlayerBaseStateChangedEvent { isInside: true }` pertama kali, dan diperbarui kalau base lain dimasuki |
| Helipad | Sistem ini, `helipad`, dengan posisi dari data | `RegionChangedEvent.to == Snow` pertama kali. `HeavyItemSystem` tetap mengatur tanda kompasnya saat membawa barang |

Sistem ini hanya menulis penanda `base` dan `helipad`. Keduanya memakai `showOnCompass = false`, supaya kompas tetap mengikuti aturan #69 dan tujuan #81.

**Penanda pemain:**
- **Klik kiri pada area kosong:** `AddPlayerMarker(mapPos, icon)`. Kalau sudah `maxPlayerMarkers`, tampilkan `notif.map_markers_full`. Id-nya `player_<nextPlayerMarkerId>`.
- **Klik kiri pada penanda pemain:** menu kecil berisi `Hapus` dan `Tampilkan di kompas`.
- **Pilihan ikon:** `PlayerPin`, `PlayerDanger`, dan `PlayerResource`.
- **Klik di area yang belum terbuka:** boleh, supaya pemain bisa menandai tempat dari petunjuk.
- **Tinggi posisi:** Y memakai tinggi pemain, karena kompas hanya memakai arah horizontal.

Kompas (#30) membaca `showOnCompass`. Panel membaca `showOnMap`. Penanda pemain memakai `showOnCompass = false` secara awal.

### Lembar peta sebagai hadiah

**`MapSheetPickup : IslandInteractable`** berada di scene region, dan tidak mengisi slot tas. `OnInteract` memanggil `RevealSheet(region)`, lalu menampilkan `notif.map_sheet_found`.

| Lembar | Lokasi usulan | Alasan |
|---|---|---|
| `Volcano` | Shelter tua tempat buku Diary | Peta awal yang hampir pasti ditemukan |
| `Amazon` | Kemah peneliti di ujung jembatan | Hadiah setelah membuka jembatan |
| `Snow` | Pos jaga Pangkalan Militer | Jejak militer di GDD bagian 2 |
| `Logistic` | Balai desa, atau dari log militer `RevealMapRegion` (#100) | Membantu mencari suku cadang |

**`public void RevealSheet(RegionType r)`:** tambahkan ke `revealedSheets`. Lembar tidak membuka fog, dan hanya menambah garis serta nama tempat di area yang sudah dijelajahi. `LoreHintGrantedEvent` dengan `RevealMapRegion` memanggil fungsi ini juga.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `MapMarkerChangedEvent`, dari `MapMarkerRegistry`.

**Event yang didengar:** `RegionChangedEvent`, `PlayerBaseStateChangedEvent`, `LoreHintGrantedEvent`, dan `RunEndedEvent`.

| Sistem | Hubungan |
|---|---|
| `CompassSystem` (#30) | Pemilik `MapMarkerRegistry`, dan tetap satu-satunya tampilan di atas layar |
| `UIFocusStack` (#83) | Panel `WorldMap`, tanpa jeda |
| `InputManager` (#18) | `OpenMap` dan `ToggleMap` |
| `RegionStreaming` (#88) | `RegionRecord.bounds` untuk `mapBounds`. Data ini ada di scene dasar, jadi tidak perlu region dimuat |
| `WorldStateSave` (#80) | `world_map` dengan `LoadOrder` 135 |
| `LoreCollectibles` (#100) | Hadiah `RevealMapRegion` |
| `LocalizationManager` (#104) | Nama tempat dan label |

**Batas pekerjaan:** bilah kompas di atas layar tetap milik #30.

## 4. Kasus Khusus

- **Objek pool:** ikon penanda memakai array `iconPool` milik panel, dan ikon yang tidak terpakai dimatikan serta kembali kosong. `ObjectPooler` tidak dipakai, karena jumlahnya kecil dan tetap. Kalau penanda melebihi 32, penanda pemain diprioritaskan dan sisanya tidak digambar.
- **Game dijeda:** peta tidak menjeda game. Kalau `Pause` dibuka di atasnya, timer stempel berhenti, dan panel tetap berada di bawah `Pause`.
- **Musuh menyerang saat peta terbuka:** game tetap berjalan, jadi membuka peta di luar base berbahaya. `PlayerPinnedState` (#110) menutup panel lewat `Close(Forced)` saat pemain ditangkap.
- **Save:** `CaptureState` menyimpan `WorldMapSaveData`. `OnRestoreFinished` memasang lagi penanda pemain ke registry dan menandai `fogDirty = true`. Penanda sistem dipasang lagi oleh pemiliknya masing-masing, karena `LoadOrder` 135 berjalan paling akhir.
- **Permadeath:** semua data ada di `survival_save.json`, jadi area terbuka, lembar, dan penanda hilang. Peta tidak disimpan di `stats.json`.
- **Region belum dimuat:** gambar sudah di-bake, dan penanda hanya memakai posisi. Peta tetap lengkap.
- **Fase 1:** `WorldMapSystem` tidak ada di `ShipPhase`, jadi `OpenMap` tidak melakukan apa pun.
- **Membawa Heavy Item:** peta boleh dibuka. Barang tidak terjatuh.
- **Pemain tiba di pantai:** `StampFog` dipanggil sekali saat `OnEnable`, supaya posisi awal langsung terbuka.
- **Ukuran data:** pulau selebar 2 km dengan sel 8 m menghasilkan 62.500 bit, sekitar 10 KB dalam Base64.
- **Sistem disetujui di tengah pengembangan:** save lama tanpa `world_map` memakai nilai awal, sesuai aturan #80.

## 5. Keputusan Desain

**Keputusan yang dipakai di blueprint ini:**

- `WorldMap` tidak menjeda, sesuai tabel `UIFocusStack.md` yang sudah ada.
- Sistem nonaktif lewat `GameBalanceConfig.map.enabled = false`.

**Usulan yang menunggu developer:**

1. **Apakah sistem ini dipakai:** rekomendasi ya, karena pemain harus mengingat rute di 4 region.
2. **Bentuk peta:** menu penuh bergaya kertas, tanpa minimap. Alasan: tekanan survival tetap terasa, dan #30 sudah mengisi kebutuhan arah saat bermain.
3. **Peta menjeda atau tidak:** tidak menjeda, sehingga mengikuti aturan inventori di #83.
4. **Fog of war:** radius 40 m, dan 25 m di Region 2. Pemain tidak punya peta lengkap sejak awal.
5. **Lembar peta:** satu per region di lokasi pada tabel. Lembar hanya menambah keterangan, jadi tidak wajib untuk bermain.
6. **Penanda pemain:** maksimum 8, dan tidak muncul di kompas kecuali dipilih.
7. **Permadeath:** semua data peta terhapus, sesuai GDD bagian 1.

## 6. Skenario Tes

1. Setel `map.enabled = false`, lalu tekan `M`. **Hasil:** tidak ada panel, dan kompas serta save tetap normal.
2. Setel `map.enabled = true`, lalu tiba di pulau dan tekan `M`. **Hasil:** panel terbuka, game tetap berjalan, kursor lepas, dan hanya area sekitar pemain yang terbuka.
3. Berjalan 200 m, lalu buka lagi. **Hasil:** jalurnya terbuka selebar sekitar 80 m. Di Region 2 selebar sekitar 50 m.
4. Tekan `M` lagi. **Hasil:** panel tertutup, dan input gameplay kembali.
5. Tambahkan penanda pemain sampai 8, lalu satu lagi. **Hasil:** penanda ke-9 ditolak dengan notifikasi.
6. Pilih `Tampilkan di kompas`. **Hasil:** kompas menampilkan penanda itu. Hapus penanda. **Hasil:** hilang dari keduanya.
7. Tunggu kargo mingguan. **Hasil:** ikon `Supply` tampil di kompas dan peta, lalu hilang setelah semua kargo dibuka.
8. Ambil lembar `Snow`. **Hasil:** nama tempat tampil, hanya di area yang sudah terbuka.
9. Biarkan pemain ditangkap Special Zombie saat peta terbuka. **Hasil:** panel tertutup paksa.
10. Save, keluar, lalu lanjutkan. **Hasil:** fog, lembar, penanda pemain, dan penanda sistem kembali sama.
11. Mati, lalu mulai run baru. **Hasil:** peta kosong kembali.
12. Buka Profiler saat berjalan dengan peta tertutup. **Hasil:** tidak ada alokasi GC, dan `fogTexture.Apply` tidak terpanggil.
