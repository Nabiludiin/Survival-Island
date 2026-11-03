# Blueprint: InventoryManager.cs
**Lokasi:** `docs/blueprint/Core_Logic/InventoryManager.md`
**Deskripsi Utama:** Sistem manajemen slot dinamis. Mengatur UI Grid, penyimpanan *resource*, dan transfer data awal saat pemain baru mendarat di pulau.

## 1. Arsitektur & Variabel Utama
*   `[Header("Inventory Data")]`
    *   `public List<InventorySlot> slots = new List<InventorySlot>();`
    *   `public int maxSlots = 15;` (Bisa diperluas dengan item *Backpack*).
*   `[Header("UI References")]`
    *   `public GameObject inventoryUI;`
    *   `public Transform gridLayoutGroup;`
    *   `public GameObject slotPrefab;`

## 2. Detail Logika & Fungsi
*   **`void Start()`**
    *   Jika `GameManager.Instance.currentPhase == IslandSurvival`:
        *   Panggil `LoadLootFromGameManager()` untuk mengisi inventory dengan barang yang diselamatkan dari kapal (Penting: Tambahkan item permanen `Base_Kail` dan `Base_Pistol`).
*   **`public bool AddItem(ItemData item, int amount)`**
    *   Cari slot yang memiliki item sama dan belum penuh (*Stacking*). Jika ada, tambahkan *amount*.
    *   Jika tidak ada *stack*, cari slot kosong pertama. Jika tidak ada slot, return `false` (Inventory Full).
    *   Return `true` dan panggil `UpdateUI()`.
*   **`public bool HasItem(string itemID, int requiredAmount)`**
    *   Fungsi pembantu krusial untuk *Crafting* dan *Building*. Lakukan iterasi (loop) ke semua slot. Hitung total item dengan ID tersebut. Return `true` jika total >= `requiredAmount`.
*   **`public void RemoveItem(string itemID, int amount)`**
    *   Kurangi item dari slot. Jika amount di slot <= 0, kosongkan slot tersebut.