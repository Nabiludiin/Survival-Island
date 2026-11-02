# Blueprint: CraftingManager.cs
**Lokasi:** `docs/blueprint/Core_Logic/CraftingManager.md`
**Deskripsi Utama:** Membaca data *Recipe* (ScriptableObject) dan berinteraksi dengan `InventoryManager` untuk menukar bahan mentah dengan barang jadi.

## 1. Arsitektur & Variabel Utama
*   `public List<CraftingRecipe> unlockedRecipes;` 
    *(Catatan: CraftingRecipe adalah ScriptableObject yang berisi array `RequiredItems` dan `ResultItem`).*
*   `public GameObject craftingUIPanel;`

## 2. Detail Logika & Fungsi
*   **`public void TryCraft(CraftingRecipe recipe)`**
    *   Lakukan iterasi ke bahan yang dibutuhkan `recipe.RequiredItems`.
    *   Panggil `InventoryManager.Instance.HasItem(bahan.ID, bahan.Amount)`.
    *   Jika ada HANYA SATU bahan yang tidak cukup, batalkan. Munculkan teks UI *"Missing Materials"*.
    *   Jika semua bahan cukup:
        *   Jalankan iterasi lagi, kali ini panggil `InventoryManager.Instance.RemoveItem(bahan.ID, bahan.Amount)` untuk setiap bahan.
        *   Panggil `InventoryManager.Instance.AddItem(recipe.ResultItem.ID, 1)`.
        *   Mainkan SFX *Crafting/Hammering*.