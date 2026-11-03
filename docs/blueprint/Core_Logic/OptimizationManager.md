# Blueprint: OptimizationManager.cs
**Lokasi:** `docs/blueprint/Core_Logic/OptimizationManager.md`
**Deskripsi Utama:** Mengatur performa game (FPS) dengan mematikan AI atau animasi zombie yang berada jauh dari pandangan pemain (Culling).

## 1. Arsitektur & Variabel
*   `public Transform player;`
*   `public float cullingDistance = 50f;`
*   `private List<GameObject> activeEnemies;`

## 2. Detail Logika & Fungsi
*   **`void Update()`**
    *   Gunakan `InvokeRepeating` (setiap 1 detik, bukan setiap frame) untuk mengecek jarak setiap musuh di `activeEnemies` terhadap `player.position`.
    *   Jika jarak > `cullingDistance`, nonaktifkan `Animator` dan `NavMeshAgent` musuh untuk menghemat CPU.