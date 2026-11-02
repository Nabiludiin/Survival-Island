# Blueprint: GameManager.cs
**Lokasi:** `LOGIC/Core_Logic/GameManager.md`
**Deskripsi Utama:** *Singleton* absolut yang hidup di semua *Scene* (`DontDestroyOnLoad`). Menjembatani data dari Kapal (Fase 1) ke Pulau (Fase 2), dan mengecek kondisi Menang/Kalah (Permadeath).

## 1. Arsitektur & Variabel Utama
*   `public static GameManager Instance { get; private set; }`
*   `[Header("Global Game State")]`
    *   `public enum GamePhase { SinkingShip, IslandSurvival }`
    *   `public GamePhase currentPhase;`
    *   `public int currentDay = 1;`
    *   `public bool isHelicopterBuilt = false;`
*   `[Header("Cross-Phase Data")]`
    *   `public List<SavedItemData> savedShipLoot = new List<SavedItemData>();` (Data item yang berhasil dibawa dari Fase 1).

## 2. Detail Logika & Fungsi
*   **`void Awake()`**
    *   Implementasi Singleton murni: `if (Instance != null && Instance != this) Destroy(this); else { Instance = this; DontDestroyOnLoad(gameObject); }`
*   **`public void SaveFase1Loot(List<Item> lootedItems)`**
    *   Ubah data item dari kapal menjadi format serialisasi (ID Item) dan simpan ke `savedShipLoot`.
*   **`public void CheckDay60Milestone()`**
    *   Dipanggil oleh `DayNightDirector` setiap pergantian hari.
    *   Jika `currentDay == 60`:
        *   Jika `isHelicopterBuilt == true` -> Panggil `TriggerWinSequence()`.
        *   Jika `isHelicopterBuilt == false` -> Panggil `TriggerGameOver("The Rescue Helicopter arrived, but you had no flare or ride. You are left behind.")`.
*   **`public void ResetGameAndReturnToShip()` (Permadeath)**
    *   Hapus data pulau. Bersihkan `savedShipLoot`. Set `currentDay = 1`.
    *   Muat ulang *Scene* Kapal: `SceneManager.LoadScene("ShipPhase");`