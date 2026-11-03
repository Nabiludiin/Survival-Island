# Blueprint: CompassSystem.cs
**Lokasi:** `docs/blueprint/Core_Logic/CompassSystem.md`
**Deskripsi Utama:** Menampilkan arah mata angin (N, E, S, W) dan penanda lokasi di layar, serta memiliki `MapMarkerRegistry` yang dipakai bersama peta.
**Issue:** #30, #69, #71, #81, #105
**Rujukan GDD:** Usulan, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

```csharp
public enum MarkerIcon { Base, Supply, Part, Helipad, Objective, PlayerPin, PlayerDanger, PlayerResource }
```

| Variabel | Tipe | Fungsi |
|---|---|---|
| `playerTransform` | `Transform` | Sumber arah |
| `compassBarUI` | `RectTransform` | Bar kompas |
| `Markers` | `MapMarkerRegistry` | Data penanda. Tetap aktif walaupun `WorldMapSystem` nonaktif |

`MapMarker` dan `MapMarkerRegistry` dijelaskan di `WorldMapSystem.md`.

## 2. Detail Logika & Fungsi

**`void Update()`**: ambil rotasi Y pemain, lalu geser `compassBarUI` berlawanan arah. Kompas hanya menggambar penanda dengan `showOnCompass = true`.

| Fungsi | Dipakai oleh | Isi |
|---|---|---|
| `string AddMarker(Vector3 position, MarkerIcon icon)` | `BeachSupplyManager` (#71) | Meneruskan ke `Markers.Add`, dan mengembalikan ID |
| `void SetMarker(string markerId, Vector3 position)` | `HeavyItemSystem` (#69), format `part_<partId>` | Meneruskan ke `Markers.Set` |
| `void RemoveMarker(string markerId)` | #69, #71 | Meneruskan ke `Markers.Remove` |

**Penanda tujuan (#81):** mendengar `ObjectiveMarkerChangedEvent { hasMarker, position, label }`. Event ini sticky, jadi `Last` dibaca di `OnEnable`.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `MapMarkerChangedEvent`, lewat `MapMarkerRegistry`.

**Event yang didengar:** `ObjectiveMarkerChangedEvent`.

| Sistem | Hubungan |
|---|---|
| `WorldMapSystem` (#105) | Membaca `Markers` |
| `BeachSupplyManager` (#71), `HeavyItemSystem` (#69) | Memasang penanda |
| `ObjectiveManager` (#81) | Mengirim penanda tujuan |

## 4. Kasus Khusus

- **Objek pool:** tidak memakai pool.
- **Game dijeda:** gerakan mengikuti rotasi pemain, yang berhenti saat jeda.
- **Permadeath:** penanda hilang bersama scene.
- **Save dan load:** penanda sistem dipasang ulang oleh pemiliknya. Penanda pemain disimpan `world_map` (#105).
- **Peta nonaktif:** registry dan kompas tetap berjalan.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** `MapMarkerRegistry` milik `CompassSystem` (`WorldMapSystem.md`).

**Keputusan teknis:** ID penanda memakai `string`, mengikuti `MapMarkerRegistry` (#105). Rancangan #71 yang memakai `int AddMarker` dan `RemoveMarker(int)` tidak dipakai.

**Menunggu developer:** tidak ada yang menghalangi.

## 6. Skenario Tes

1. Putar pemain. **Hasil:** bar bergeser berlawanan arah.
2. Tiba kargo pantai. **Hasil:** penanda `Supply` muncul, lalu hilang setelah `RemoveMarker`.
3. Temukan suku cadang. **Hasil:** penanda `part_<partId>` muncul.
4. Ganti tujuan. **Hasil:** penanda tujuan berpindah.
5. Nonaktifkan peta. **Hasil:** kompas tetap menampilkan penanda.
6. Buat penanda dengan `showOnCompass = false`. **Hasil:** tidak tampil di kompas.
