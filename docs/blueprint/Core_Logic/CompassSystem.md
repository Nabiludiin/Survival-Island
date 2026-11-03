# Blueprint: CompassSystem.cs
**Lokasi:** `docs/blueprint/Core_Logic/CompassSystem.md`
**Deskripsi Utama:** Menampilkan arah mata angin (N, E, S, W) dan penanda lokasi di layar, serta memiliki data penanda `MapMarkerRegistry`.
**Issue:** #30, #69, #71, #81
**Rujukan GDD:** Usulan, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

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
    public IReadOnlyList<MapMarker> All { get; }
}
```

| Variabel | Tipe | Fungsi |
|---|---|---|
| `playerTransform` | `Transform` | Sumber arah |
| `compassBarUI` | `RectTransform` | Bar kompas |
| `Markers` | `MapMarkerRegistry` | Data penanda |

`MapMarkerRegistry` adalah kelas biasa milik sistem ini. Setiap perubahan menerbitkan `MapMarkerChangedEvent { markerId, isRemoved }`.

## 2. Detail Logika & Fungsi

**`void Update()`**: ambil rotasi Y pemain, lalu geser `compassBarUI` berlawanan arah. Kompas menggambar semua penanda di `Markers`.

| Fungsi | Dipakai oleh | Isi |
|---|---|---|
| `string AddMarker(Vector3 position, MarkerIcon icon)` | `BeachSupplyManager` (#71) | Meneruskan ke `Markers.Add`, dan mengembalikan ID |
| `void SetMarker(string markerId, Vector3 position)` | `HeavyItemSystem` (#69), format `part_<partId>` | Meneruskan ke `Markers.Set`. `icon` diambil dari ID: `part_` menjadi `Part`, dan `helipad` menjadi `Helipad` |
| `void RemoveMarker(string markerId)` | #69, #71 | Meneruskan ke `Markers.Remove` |

**Penanda tujuan (#81):** mendengar `ObjectiveMarkerChangedEvent { hasMarker, position, label }`. Event ini sticky, jadi `Last` dibaca di `OnEnable`. Penandanya memakai ID `objective`.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `MapMarkerChangedEvent`, lewat `MapMarkerRegistry`.

**Event yang didengar:** `ObjectiveMarkerChangedEvent`.

| Sistem | Hubungan |
|---|---|
| `BeachSupplyManager` (#71), `HeavyItemSystem` (#69) | Memasang penanda |
| `ObjectiveManager` (#81) | Mengirim penanda tujuan |

## 4. Kasus Khusus

- **Objek pool:** tidak memakai pool.
- **Game dijeda:** gerakan mengikuti rotasi pemain, yang berhenti saat jeda.
- **Permadeath:** penanda hilang bersama scene.
- **Save dan load:** kompas tidak menyimpan data. Penanda dipasang ulang oleh pemiliknya.

## 5. Keputusan Desain

**Keputusan developer:** #105 tidak dipakai. `MapMarker` dan `MapMarkerRegistry` berpindah ke blueprint ini, dan penanda buatan pemain ikut dihapus.

**Keputusan teknis:** ID penanda memakai `string`, mengikuti `MapMarkerRegistry`. Rancangan #71 yang memakai `int AddMarker` dan `RemoveMarker(int)` tidak dipakai.

**Menunggu developer:** tidak ada yang menghalangi.

## 6. Skenario Tes

1. Putar pemain. **Hasil:** bar bergeser berlawanan arah.
2. Tiba kargo pantai. **Hasil:** penanda `Supply` muncul, lalu hilang setelah `RemoveMarker`.
3. Temukan suku cadang. **Hasil:** penanda `part_<partId>` muncul.
4. Ganti tujuan. **Hasil:** penanda tujuan berpindah.
5. Save, keluar, lalu lanjutkan. **Hasil:** penanda kargo, suku cadang, dan tujuan kembali lewat pemiliknya.
6. Panggil `RemoveMarker` dengan ID yang tidak ada. **Hasil:** tidak terjadi apa-apa, dan penanda lain tetap tampil.
