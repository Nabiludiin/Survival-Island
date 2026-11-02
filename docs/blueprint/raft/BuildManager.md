# Blueprint: BuildManager.cs
**Lokasi:** `docs/blueprint/raft/BuildManager.md`
**Deskripsi Utama:** Sistem kalkulasi matriks 3D untuk menempatkan struktur. Memuat validasi *Overlap* (menabrak pohon/bangunan lain), pengecekan tanah (anti-pantai), dan pengecekan struktural (atap butuh dinding).

## 1. Arsitektur & Variabel Utama
*   `[Header("Building Prefabs & Ghosting")]`
    *   `public GameObject[] structurePrefabs;` (Array: 0 = Lantai, 1 = Dinding, 2 = Atap).
    *   `private GameObject ghostObject;` (Hologram biru/merah).
    *   `public Material ghostValidMat, ghostInvalidMat;`
*   `[Header("Grid System")]`
    *   `public float gridSize = 2.5f;`
    *   `public float maxBuildDistance = 6f;`
*   `[Header("Layer Masks")]`
    *   `public LayerMask buildableSurfaceMask;` (Tanah biasa).
    *   `public LayerMask restrictedMask;` (Pasir Pantai, Region C).
    *   `public LayerMask obstacleMask;` (Pohon, Batu, Zombie).

## 2. Detail Logika & Fungsi
*   **`void UpdateGhostPosition()`**
    *   Gunakan `Physics.Raycast(camera.position, camera.forward, out RaycastHit hit, maxBuildDistance)`.
    *   **Grid Math:** 
        `Vector3 snapPos;`
        `snapPos.x = Mathf.Round(hit.point.x / gridSize) * gridSize;`
        `snapPos.z = Mathf.Round(hit.point.z / gridSize) * gridSize;`
        `snapPos.y = hit.point.y;` (Y disesuaikan dengan *terrain height*).
    *   Set posisi `ghostObject` ke `snapPos`.
*   **`bool ValidatePlacement(Vector3 pos)`**
    *   **Aturan 1 (Anti-Pantai):** Lakukan `Physics.SphereCast` ke bawah. Jika mengenai `restrictedMask`, return `false`.
    *   **Aturan 2 (Overlap):** Gunakan `Physics.OverlapBox(pos, ghostObject.bounds.extents, Quaternion.identity, obstacleMask)`. Jika array hasil > 0 (berarti ada objek menabrak struktur), return `false`.
    *   **Aturan 3 (Ketergantungan Struktur):** Jika sedang membangun "Atap", pastikan *Raycast* ke bawah mengenai objek dengan *Tag* `BaseWall`. Jika tidak ada dinding di bawahnya, atap melayang tidak diizinkan. return `false`.
*   **`void ExecuteBuild()`**
    *   Jika `ValidatePlacement` bernilai true dan `Inventory.HasItems(woodCost, stoneCost)`:
        *   Kurangi materi.
        *   `Instantiate(prefab Asli, ghostPos, ghostRot)`.
        *   Terapkan script `BaseHealth` ke objek yang baru dibuat (agar bisa diserang zombie).