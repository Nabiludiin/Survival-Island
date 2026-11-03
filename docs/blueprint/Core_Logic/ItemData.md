# Blueprint: ItemData.cs (ScriptableObject)
**Lokasi:** `docs/blueprint/Core_Logic/ItemData.md`
**Deskripsi Utama:** Cetakan database (Scriptable Object) untuk membuat variasi item tanpa membebani memori dengan skrip terpisah-pisah.

## 1. Arsitektur & Variabel
*   `[CreateAssetMenu(fileName = "New Item", menuName = "Survival/Item Data")]`
*   `public class ItemData : ScriptableObject`
*   `[Header("Basic Info")]`
    *   `public string itemID;`
    *   `public string itemName;`
    *   `public Sprite icon;`
    *   `public GameObject prefab;`
*   `[Header("Item Type")]`
    *   `public enum ItemType { Material, Consumable, Weapon, Tool, KeyItem }`
    *   `public ItemType type;`
*   `[Header("Specific Attributes")]`
    *   `public float healAmount;` (Jika Consumable).
    *   `public float weaponDamage;` (Jika Weapon).