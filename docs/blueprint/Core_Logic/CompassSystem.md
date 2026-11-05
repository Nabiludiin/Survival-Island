# Blueprint: CompassSystem.cs
**Lokasi:** `docs/blueprint/Core_Logic/CompassSystem.md`
**Deskripsi Utama:** Menampilkan arah mata angin (N, E, S, W) dan penanda lokasi di bilah atas layar, serta memiliki data penanda `MapMarkerRegistry`.
**Issue:** #30, #69, #71, #81
**Rujukan GDD:** Usulan, belum ada di GDD. Membantu tujuan GDD bagian 1 dan 5: memindahkan suku cadang dari Region 4 ke Region 3.

## 1. Arsitektur & Variabel Utama

### Pola

`CompassSystem` adalah singleton per scene di `IslandPhase`. Komponennya berada di canvas HUD, sebagai anak `HUDManager`. `Awake` mengisi `Instance` dan membuat `Markers`, supaya sistem lain bisa memasang penanda sejak `OnEnable` atau `OnRestoreFinished` mereka.

**Hanya di `IslandPhase` (usulan):** semua pemasang penanda (#69, #71, #81) hanya ada di pulau. Fase 1 sudah punya `lifeboatMarker` milik `HUDManager` (#74), jadi kompas tidak dipasang di `ShipPhase`.

**Arah utara (usulan):** sumbu +Z dunia adalah utara, dan +X adalah timur. Layout peta di #88 mengikuti aturan ini.

```csharp
public enum MarkerIcon { Supply, Part, Helipad, Objective }

[Serializable]
public struct MapMarker
{
    public string id;           // part_2, supply, objective, helipad
    public Vector3 position;
    public MarkerIcon icon;
    public string labelKey;     // Kunci #104, boleh kosong
}

public class MapMarkerRegistry
{
    public string Add(Vector3 position, MarkerIcon icon);    // Membuat id otomatis
    public void Set(in MapMarker marker);                    // Membuat atau memperbarui
    public bool Remove(string id);
    public bool TryGet(string id, out MapMarker marker);
    public IReadOnlyList<MapMarker> All { get; }
}
```

`MapMarkerRegistry` adalah kelas biasa milik sistem ini. Setiap perubahan menerbitkan `MapMarkerChangedEvent { markerId, isRemoved }` (`GameEventBus.md`, tidak sticky).

### Variabel `MapMarkerRegistry`

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `markers` | `List<MapMarker>` | Kosong, kapasitas 16 | Isi registry. `All` mengembalikannya sebagai `IReadOnlyList` |
| `nextAutoId` | `int` | 1 | Nomor untuk ID otomatis `marker_<n>` |

### Variabel `CompassSystem`

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Instance` | `CompassSystem` | `null` | Singleton per scene |
| `Markers` | `MapMarkerRegistry` | Dibuat di `Awake` | Data penanda |
| `playerTransform` | `Transform` | Inspector, objek `Player` di `IslandPhase` | Sumber yaw. Yaw pemain diputar pada objek ini (`FirstPersonCamera.md`) |
| `compassBarUI` | `RectTransform` | Inspector | Bilah kompas |
| `barTicks` | `RawImage` | Inspector | Tekstur garis derajat yang berulang setiap 360 derajat |
| `cardinalLabels` | `TextMeshProUGUI[4]` | Inspector | Huruf arah pada 0, 90, 180, dan 270 derajat |
| `iconSlots` | `CompassIcon[]` | 12 slot di prefab | Ikon penanda yang dibuat sekali. Tidak memakai `Instantiate` saat bermain |
| `iconSprites` | `Sprite[]` | Inspector, urut `MarkerIcon` | Gambar per jenis penanda |
| `visible` | `List<int>` | Kapasitas 12 | Indeks penanda yang mendapat slot. Dihitung ulang hanya saat registry berubah |
| `layoutDirty` | `bool` | `true` | Registry berubah, jadi pembagian slot perlu dihitung ulang |
| `isHidden` | `bool` | `false` | Kompas disembunyikan saat adegan atau setelah mati |

`CompassIcon` adalah komponen kecil di setiap slot: `Image icon`, `TextMeshProUGUI distanceText`, `CanvasGroup group`, dan `int lastMeters`.

### Angka penyetelan (usulan, `GameBalanceConfig.compass`)

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `compass.visibleArcDegrees` | 180 | Lebar sudut yang terlihat di bilah |
| `compass.edgeFadeDegrees` | 15 | Ikon memudar di ujung bilah |
| `compass.showDistance` | `true` | Jarak dalam meter di bawah ikon |
| `compass.clampObjectiveToEdge` | `true` | Penanda `Objective` di luar bilah tetap tampil di ujung, dengan panah |

Jumlah slot ikon (12) adalah ukuran prefab, bukan angka permainan. Paling banyak ada 8 penanda bersamaan: 5 suku cadang, 1 kargo, 1 tujuan, dan 1 helipad.

## 2. Detail Logika & Fungsi

### Registry

**`public string Add(Vector3 position, MarkerIcon icon)`**: buat ID `marker_<nextAutoId>`, naikkan `nextAutoId`, tambahkan penanda, terbitkan event, lalu kembalikan ID.

**`public void Set(in MapMarker marker)`**:

1. Kalau `marker.id` kosong, catat peringatan dan selesai.
2. Kalau ID sudah ada, ganti datanya di tempat. Kalau belum, tambahkan di akhir.
3. Terbitkan `MapMarkerChangedEvent { markerId, isRemoved = false }`.

**`public bool Remove(string id)`**: hapus kalau ada, terbitkan event dengan `isRemoved = true`, lalu kembalikan `true`. Kalau tidak ada, kembalikan `false` tanpa event.

Pencarian ID memakai loop biasa, karena isinya paling banyak belasan. Tidak ada alokasi per frame.

### Fungsi untuk sistem lain

| Fungsi | Dipakai oleh | Isi |
|---|---|---|
| `string AddMarker(Vector3 position, MarkerIcon icon)` | `BeachSupplyManager` (#71) | Meneruskan ke `Markers.Add`, dan mengembalikan ID |
| `void SetMarker(string markerId, Vector3 position)` | `HeavyItemSystem` (#69), format `part_<partId>` dan `helipad` | Meneruskan ke `Markers.Set`. `icon` diambil dari ID, lihat di bawah |
| `void SetMarker(string markerId, Vector3 position, MarkerIcon icon)` | Pemanggil baru | Meneruskan ke `Markers.Set` dengan ikon yang jelas |
| `void RemoveMarker(string markerId)` | #69, #71 | Meneruskan ke `Markers.Remove`. ID yang tidak ada diabaikan |

**Ikon dari ID pada `SetMarker` dua parameter:**

| ID | Ikon |
|---|---|
| Diawali `part_` | `Part` |
| `helipad` | `Helipad` |
| `objective` | `Objective` |
| Lainnya | Ikon lama kalau ID sudah ada. Kalau belum, `Objective` dengan peringatan sekali |

### Penanda tujuan (#81)

Mendengar `ObjectiveMarkerChangedEvent { hasMarker, position, label }`. Event ini sticky, jadi `Last` dibaca di `OnEnable`.

- `hasMarker == true`: `Markers.Set` dengan `id = "objective"`, `icon = Objective`, `labelKey = label`.
- `hasMarker == false`: `Markers.Remove("objective")`.

### Pembagian slot ikon

**`private void RebuildVisible()`**, dijalankan di `LateUpdate` kalau `layoutDirty`:

1. Kosongkan `visible`.
2. Urutkan penanda menurut prioritas: `Objective`, `Helipad`, `Part`, lalu `Supply`. Penanda dengan prioritas sama diurutkan menurut ID, supaya hasilnya tetap.
3. Ambil paling banyak `iconSlots.Length`. Isi `icon.sprite` dari `iconSprites`, dan matikan slot sisanya.
4. `layoutDirty = false`.

`MapMarkerChangedEvent` hanya mengatur `layoutDirty = true`. Perubahan posisi saja tetap memicu hitung ulang, karena biayanya kecil.

### Tampilan per frame

**`void LateUpdate()`**, setelah pemain dan kamera berputar:

1. Kalau `isHidden` atau `playerTransform == null`, matikan `compassBarUI` dan selesai.
2. `yaw = playerTransform.eulerAngles.y`.
3. **Bilah:** `barTicks.uvRect = new Rect(yaw / 360f - arc / 720f, 0, arc / 360f, 1)`, dengan `arc = visibleArcDegrees`. Tekstur diatur `Wrap Mode = Repeat`, jadi bilah bergeser berlawanan arah putaran tanpa batas.
4. **Huruf arah:** tiap `cardinalLabels[i]` memakai sudut `i * 90`, lalu diposisikan dengan `PlaceOnBar`.
5. **Penanda:** untuk tiap indeks di `visible`, hitung `bearing = Mathf.Atan2(dx, dz) * Mathf.Rad2Deg` dari posisi pemain ke penanda. Tinggi diabaikan. Lalu panggil `PlaceOnBar`.
6. **Jarak:** kalau `showDistance`, hitung meter horizontal yang dibulatkan. Teks hanya ditulis kalau angkanya berubah dari `lastMeters`, memakai `TMP_Text.SetText("{0} m", meter)` supaya tidak membuat string baru.

**`private void PlaceOnBar(RectTransform item, CanvasGroup group, float bearing, bool clampToEdge)`**:

```csharp
float half = visibleArcDegrees * 0.5f;
float delta = Mathf.DeltaAngle(yaw, bearing);                  // -180 sampai 180
bool outside = Mathf.Abs(delta) > half;
if (outside && !clampToEdge) { group.alpha = 0f; return; }
float clamped = Mathf.Clamp(delta, -half, half);
item.anchoredPosition = new Vector2(clamped / half * compassBarUI.rect.width * 0.5f, item.anchoredPosition.y);
float fade = Mathf.InverseLerp(half, half - edgeFadeDegrees, Mathf.Abs(delta));
group.alpha = outside ? 1f : fade;                              // Penanda yang dijepit tetap jelas
```

`clampToEdge` hanya `true` untuk `Objective` saat `clampObjectiveToEdge` aktif. Panah kiri atau kanan di ikon tampil saat `outside`.

### Tampil dan sembunyi

| Kejadian | Tindakan |
|---|---|
| `CinematicStateChangedEvent.isPlaying == true` (#53, #99) | `isHidden = true` |
| `CinematicStateChangedEvent.isPlaying == false` | `isHidden = false`, kecuali pemain sudah mati |
| `PlayerDiedEvent` | `isHidden = true`, sampai scene diganti |
| Panel menjeda terbuka | Tidak ada tindakan. Panel menutupi HUD (`UIManager.md`) |
| Panel tanpa jeda terbuka (Inventori) | Kompas tetap tampil, supaya pemain tetap tahu arah saat menyusun barang |

Sticky `CinematicStateChangedEvent.Last` dibaca di `OnEnable`.

### Bahasa

`cardinalLabels` memakai kunci `ui.compass.n`, `ui.compass.e`, `ui.compass.s`, dan `ui.compass.w` (#104). Isinya diisi ulang saat `LocaleChangedEvent`. Dalam Bahasa Indonesia, hurufnya U, T, S, dan B. Label penanda (`labelKey`) belum ditampilkan di bilah, dan disiapkan untuk tooltip di masa depan.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `MapMarkerChangedEvent`, lewat `MapMarkerRegistry`.

**Event yang didengar:** `ObjectiveMarkerChangedEvent` (sticky), `CinematicStateChangedEvent` (sticky), `PlayerDiedEvent`, dan `LocaleChangedEvent`. Semuanya berlangganan di `OnEnable` dan lepas di `OnDisable`.

| Sistem | Hubungan |
|---|---|
| `BeachSupplyManager` (#71) | `AddMarker` dan `RemoveMarker` untuk kargo pantai |
| `HeavyItemSystem` (#69) | `SetMarker("part_<partId>")`, `SetMarker("helipad")`, dan `RemoveMarker` |
| `ObjectiveManager` (#81) | Mengirim `ObjectiveMarkerChangedEvent` |
| `HUDManager` | Induk canvas kompas |
| `FirstPersonCamera` (#102) | Yaw berada di `playerTransform` |
| `LocalizationManager` (#104) | Huruf arah |
| `WorldMapSystem` (#105) | Arsip, tidak dipakai. Kalau dipakai nanti, peta membaca `Markers.All` |

## 4. Kasus Khusus

- **Objek pool:** tidak memakai `ObjectPooler`. Slot ikon dibuat sekali di prefab dan dipakai ulang.
- **Game dijeda:** `LateUpdate` tetap berjalan, tetapi yaw dan posisi tidak berubah karena pemain berhenti. Tidak ada timer, jadi tidak ada yang perlu dihentikan.
- **Permadeath:** registry dan `nextAutoId` hilang bersama scene `IslandPhase`. Run baru mulai dengan registry kosong.
- **Save dan load:** kompas tidak menyimpan data. Penanda dipasang ulang oleh pemiliknya: `BeachSupplyManager` dan `HeavyItemSystem` lewat `OnRestoreFinished`, dan tujuan lewat event sticky. `Markers` sudah dibuat di `Awake`, sebelum `OnRestoreFinished` berjalan.
- **Region dilepas (#88):** penanda adalah data posisi, jadi tetap tampil walaupun regionnya tidak dimuat (`RegionStreaming.md`).
- **Pemain tepat di posisi penanda:** jarak horizontal di bawah 0,5 meter. `Atan2(0, 0)` bernilai 0, jadi ikon tampil di utara dengan jarak `0 m`. Tidak ada error.
- **ID kosong atau ganda:** ID kosong ditolak dengan peringatan. ID ganda memperbarui penanda lama, tidak membuat dua.
- **Penanda lebih banyak dari slot:** penanda dengan prioritas rendah tidak tampil, tanpa error.
- **`playerTransform` kosong:** bilah disembunyikan, dan peringatan dicatat sekali.
- **Region 2 berkabut:** kompas tetap bekerja. Kabut hanya mengurangi pandangan, sesuai GDD bagian 5.

## 5. Keputusan Desain

**Keputusan developer:** #105 tidak dipakai. `MapMarker` dan `MapMarkerRegistry` berpindah ke blueprint ini, dan penanda buatan pemain ikut dihapus.

**Keputusan teknis:**

- ID penanda memakai `string`, mengikuti `MapMarkerRegistry`. Rancangan #71 yang memakai `int AddMarker` dan `RemoveMarker(int)` tidak dipakai.
- Bilah digeser lewat `RawImage.uvRect`, bukan dengan memindahkan `RectTransform`, supaya putaran 360 derajat tidak punya sambungan.
- Huruf arah diletakkan dengan perhitungan yang sama dengan penanda, supaya bisa diterjemahkan.

**Usulan baru (tidak menghalangi kode):**

1. Kompas hanya ada di `IslandPhase`.
2. Sumbu +Z dunia adalah utara.
3. Angka `GameBalanceConfig.compass` di atas.
4. Penanda tujuan tetap tampil di ujung bilah saat berada di belakang pemain.
5. Kompas disembunyikan saat adegan dan setelah mati.

## 6. Skenario Tes

1. Putar pemain 360 derajat. **Hasil:** bilah bergeser berlawanan arah tanpa loncatan di sambungan 0 dan 360 derajat.
2. Hadap ke +Z dunia. **Hasil:** huruf U (atau N) berada di tengah bilah.
3. Tiba kargo pantai. **Hasil:** penanda `Supply` muncul, lalu hilang setelah `RemoveMarker`.
4. Temukan suku cadang. **Hasil:** penanda `part_<partId>` muncul dengan ikon `Part` dan jarak dalam meter.
5. Angkat suku cadang. **Hasil:** penanda barang itu hilang, dan penanda `helipad` muncul.
6. Ganti tujuan. **Hasil:** penanda tujuan berpindah.
7. Balik badan dari penanda tujuan. **Hasil:** ikon tujuan tertahan di ujung bilah dengan panah, sedangkan penanda lain hilang.
8. Save, keluar, lalu lanjutkan. **Hasil:** penanda kargo, suku cadang, dan tujuan kembali lewat pemiliknya.
9. Panggil `RemoveMarker` dengan ID yang tidak ada. **Hasil:** tidak terjadi apa-apa, tidak ada event, dan penanda lain tetap tampil.
10. Panggil `SetMarker("part_1", ...)` dua kali dengan posisi berbeda. **Hasil:** hanya satu penanda, di posisi terakhir.
11. Berjalan menjauh dari penanda sampai regionnya dilepas. **Hasil:** penanda tetap tampil di arah yang benar.
12. Mati. **Hasil:** kompas hilang selama adegan kematian.
13. Ganti bahasa ke Inggris. **Hasil:** huruf arah berubah menjadi N, E, S, dan W.
14. Buka inventori. **Hasil:** kompas tetap tampil. Buka `Pause`. **Hasil:** kompas tertutup panel.
15. Putar kamera terus selama satu menit dengan Profiler terbuka. **Hasil:** tidak ada alokasi GC dari `CompassSystem`.
