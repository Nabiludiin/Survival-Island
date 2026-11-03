# Blueprint: TechTreeManager.cs
**Lokasi:** `docs/blueprint/Core_Logic/TechTreeManager.md`
**Deskripsi Utama:** Sistem progresi yang mengunci/membuka resep *crafting* tingkat lanjut (Tier 2/Tier 3) berdasarkan pencapaian pemain.

## 1. Arsitektur & Variabel
*   `[System.Serializable] public class TechNode { public string nodeID; public bool isUnlocked; public string requiredNodeID; }`
*   `public List<TechNode> techTree;`

## 2. Detail Logika & Fungsi
*   **`public void UnlockNode(string id)`**
    *   Cari *node* tersebut. Cek apakah `requiredNodeID` sudah terbuka.
    *   Jika sudah, set `isUnlocked = true`, mainkan SFX "Achievement/Unlock", dan perbarui UI Tech Tree.