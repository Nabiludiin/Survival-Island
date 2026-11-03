# Blueprint: CompassSystem.cs
**Lokasi:** `docs/blueprint/Core_Logic/CompassSystem.md`
**Deskripsi Utama:** Menampilkan arah mata angin (N, E, S, W) dan penanda lokasi objek (Quest Marker) di layar UI.

## 1. Arsitektur & Variabel
*   `public Transform playerTransform;`
*   `public RectTransform compassBarUI;`

## 2. Detail Logika & Fungsi
*   **`void Update()`**
    *   Ambil rotasi Y pemain: `playerTransform.eulerAngles.y`.
    *   Geser posisi X dari UI `compassBarUI` berlawanan dengan rotasi pemain agar gambar kompas seolah berputar.