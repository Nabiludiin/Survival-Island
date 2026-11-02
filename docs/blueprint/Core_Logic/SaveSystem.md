# Blueprint: SaveSystem.cs
**Lokasi:** `docs/blueprint/Core_Logic/SaveSystem.md`
**Deskripsi Utama:** Menangani serialisasi dan deserialisasi data permainan menggunakan `JsonUtility`. Menyimpan progress seperti hari, inventori pemain, dan lokasi fisik pemain saat keluar dari game (Save & Quit).

## 1. Arsitektur & Struktur Data
*   `public static SaveSystem Instance;` (Singleton Pattern).
*   `private string saveFilePath;`
*   **Struktur Kelas Data JSON (`GameData`):**
    *   Buat *class* serializable di dalam skrip:
        `[System.Serializable]`
        `public class GameData {`
            `public int currentDay;`
            `public Vector3 playerPosition;`
            `public float playerHealth;`
            `public float playerInfection;`
            `public List<string> inventoryItemIDs;`
        `}`

## 2. Detail Logika & Fungsi
*   **`void Awake()`**
    *   Terapkan Singleton.
    *   Tentukan lokasi file (Aman untuk PC/Android): 
        `saveFilePath = Application.persistentDataPath + "/survival_save.json";`
*   **`public void SaveGame()`**
    *   Buat *instance* baru dari `GameData`.
    *   Ambil data dari *Manager* lain: `data.currentDay = GameManager.Instance.currentDay`, ambil posisi pemain, HP, dan isi inventori.
    *   Ubah ke format JSON: `string json = JsonUtility.ToJson(data, true);` (Gunakan parameter `true` agar JSON diformat rapi/Pretty Print untuk debugging).
    *   Tulis ke file: `File.WriteAllText(saveFilePath, json);`
    *   Panggil `UIManager.Instance.ShowNotification("Game Saved");`
*   **`public void LoadGame()`**
    *   Cek eksistensi file: `if (!File.Exists(saveFilePath)) return;`
    *   Baca file JSON: `string json = File.ReadAllText(saveFilePath);`
    *   Ubah kembali menjadi objek C#: `GameData data = JsonUtility.FromJson<GameData>(json);`
    *   Terapkan nilai-nilai tersebut kembali ke `GameManager`, `PlayerController`, dan `InventoryManager`.
*   **`public void DeleteSaveFile()` (Permadeath Trigger)**
    *   Jika pemain mati, hapus file agar tidak bisa diload ulang:
        `if (File.Exists(saveFilePath)) File.Delete(saveFilePath);`