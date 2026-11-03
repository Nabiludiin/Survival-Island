# Blueprint: ShipInteractable.cs
**Lokasi:** `docs/blueprint/60-seconds/ShipInteractable.md`
**Deskripsi Utama:** Sistem modular berbasis *Interface* (`IInteractable`). Menangani bagaimana barang berinteraksi dengan tas pemain, menghitung bobot barang, dan memberikan *feedback* visual (Outline) tanpa memberatkan kinerja (*draw calls*).

## 1. Arsitektur & Variabel Utama
*   Implementasikan interface: `public class ShipInteractable : MonoBehaviour, IInteractable`
*   `[Header("Item Properties")]`
    *   `public string ItemID;` (Gunakan ID unik seperti "itm_ammo_01", "npc_medic" untuk database).
    *   `public ShipInteractableKind kind;` (Enum: `Item`, `NPC`). Jenis item dibaca dari `ItemData.ItemType`.
    *   `public int weightCost = 1;` (Berat item. Makanan = 1, Senjata = 2, NPC = 4).
*   `[Header("Visual Feedback")]`
    *   `[SerializeField] private Material outlineMaterial;` (Material khusus shader glow).
    *   `private MeshRenderer meshRenderer;`
    *   `private Material[] originalMaterials;` (Simpan material asli saat game dimulai).

## 2. Detail Logika & Fungsi
*   **`void Awake()`**
    *   Cache komponen: `meshRenderer = GetComponent<MeshRenderer>();`
    *   Simpan material asli ke dalam array agar tidak hilang saat outline aktif.
*   **`public void OnHoverEnter()`**
    *   Ubah array material mesh untuk menambahkan `outlineMaterial` di index terakhir.
    *   Panggil event ke UI Manager: `UIManager.Instance.ShowTooltip("Press [E] to grab: " + ItemID + " (Weight: " + weightCost + ")");`
*   **`public void OnHoverExit()`**
    *   Kembalikan array material ke `originalMaterials`.
    *   Panggil `UIManager.Instance.HideTooltip();`
*   **`public bool Interact(InventoryManager inv)`**
    *   **Validasi Kapasitas:** `if (inv.ShipHandWeight + weightCost > inv.ShipHandCapacity) return false;`
    *   **Eksekusi:** 
        *   Panggil fungsi *pickup* di inventory: `inv.AddShipHandItem(this.ItemID, this.weightCost);`
        *   Mainkan *Particle System* debu kecil dan *AudioSource* *loot*.
        *   Matikan collider dan mesh renderer, tunggu audio selesai, lalu `Destroy(gameObject);`.
        *   `return true;`