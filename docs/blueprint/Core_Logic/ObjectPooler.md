# Blueprint: ObjectPooler.cs
**Lokasi:** `docs/blueprint/Core_Logic/ObjectPooler.md`
**Deskripsi Utama:** Penyelamat FPS/RAM. Zombie dan Peluru tidak boleh di-Instantiate berulang. Skrip ini membuat "Gudang" objek pasif yang bisa dipanggil dan dimatikan secara instan.

## 1. Arsitektur & Variabel Utama
*   `[System.Serializable] public class Pool { public string tag; public GameObject prefab; public int size; }`
*   `public List<Pool> pools;`
*   `public Dictionary<string, Queue<GameObject>> poolDictionary;` (Hanya berisi objek yang sedang tidak aktif).
*   `IPoolable` (Interface pilihan): `OnSpawned()` dan `OnReturned()` untuk mengembalikan HP, state, skala, dan komponen ke nilai awal.
*   `public static ObjectPooler Instance;`

## 2. Detail Logika & Fungsi
*   **`void Start()`**
    *   Inisialisasi *Dictionary*.
    *   Lakukan iterasi ke semua `Pool`. `Instantiate` prefab sebanyak `size`, set `SetActive(false)`, dan `Enqueue` ke dalam antrean (Queue).
*   **`public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)`**
    *   Validasi: `if (!poolDictionary.ContainsKey(tag)) return null;`
    *   Jika antrean kosong, buat satu objek baru dan catat peringatan, atau return `null` untuk pool yang punya batas keras. Objek yang masih hidup tidak pernah dipakai ulang.
    *   Ambil objek: `GameObject objectToSpawn = poolDictionary[tag].Dequeue();`
    *   Aktifkan: Set posisi, rotasi, lalu `objectToSpawn.SetActive(true);`
    *   Panggil `OnSpawned()` pada semua `IPoolable` di objek tersebut.
    *   Return `objectToSpawn`.
*   **`public void ReturnToPool(string tag, GameObject obj)`**
    *   Panggil `OnReturned()` pada semua `IPoolable`, lalu `obj.SetActive(false);` dan `poolDictionary[tag].Enqueue(obj);`.
    *   Objek yang sudah ada di antrean tidak boleh masuk dua kali.