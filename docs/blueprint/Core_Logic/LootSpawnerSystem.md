# Blueprint: LootSpawnerSystem.cs
**Lokasi:** `docs/blueprint/Core_Logic/LootSpawnerSystem.md`
**Deskripsi Utama:** Sistem RNG untuk menyebar item (Loot) secara acak di titik-titik tertentu, baik di Kapal maupun di Pulau.

## 1. Arsitektur & Variabel
*   `[System.Serializable] public class LootTable { public ItemData item; [Range(0,100)] public float dropChance; }`
*   `public List<Transform> spawnPoints;`
*   `public List<LootTable> availableLoot;`

## 2. Detail Logika & Fungsi
*   **`void Start()`**
    *   Panggil `SpawnLootAcrossMap()`.
*   **`public void SpawnLootAcrossMap()`**
    *   Lakukan perulangan (`foreach`) untuk setiap titik di `spawnPoints`.
    *   Tentukan apakah titik ini akan memunculkan item: `if (Random.value > 0.5f) continue;` (50% titik dibiarkan kosong agar realistis, dan titik berikutnya tetap diproses).
    *   Jika berhasil lolos, acak item dari `availableLoot` berdasarkan `dropChance`.
    *   `Instantiate(selectedItem.prefab, point.position, Quaternion.identity);`