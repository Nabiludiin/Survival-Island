# Blueprint: ObjectPooler.cs
**Lokasi:** `docs/blueprint/Core_Logic/ObjectPooler.md`
**Deskripsi Utama:** Penyelamat FPS/RAM. Zombie dan Peluru tidak boleh di-Instantiate berulang. Skrip ini membuat "Gudang" objek pasif yang bisa dipanggil dan dimatikan secara instan.

## 1. Arsitektur & Variabel Utama
*   `[System.Serializable] public class Pool { public string tag; public GameObject prefab; public int size; }`
*   `public List<Pool> pools;`
*   `public Dictionary<string, Queue<GameObject>> poolDictionary;`
*   `public static ObjectPooler Instance;`

## 2. Detail Logika & Fungsi
*   **`void Start()`**
    *   Inisialisasi *Dictionary*.
    *   Lakukan iterasi ke semua `Pool`. `Instantiate` prefab sebanyak `size`, set `SetActive(false)`, dan `Enqueue` ke dalam antrean (Queue).
*   **`public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)`**
    *   Validasi: `if (!poolDictionary.ContainsKey(tag)) return null;`
    *   Ambil objek: `GameObject objectToSpawn = poolDictionary[tag].Dequeue();`
    *   Aktifkan: Set posisi, rotasi, lalu `objectToSpawn.SetActive(true);`
    *   Kembalikan ke antrean agar bisa didaur ulang nanti: `poolDictionary[tag].Enqueue(objectToSpawn);`
    *   Return `objectToSpawn`.