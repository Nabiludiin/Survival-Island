# Blueprint: DurabilitySystem.cs
**Lokasi:** `docs/blueprint/Core_Logic/DurabilitySystem.md`
**Deskripsi Utama:** Ditempelkan pada senjata atau alat (Kapak/Pistol) untuk mengurangi umur pakainya setiap kali digunakan.

## 1. Arsitektur & Variabel
*   `public float maxDurability = 100f;`
*   `public float currentDurability;`
*   `public float decayPerUse = 5f;`

## 2. Detail Logika & Fungsi
*   **`public void UseItem()`**
    *   Kurangi `currentDurability -= decayPerUse;`
    *   Perbarui UI bar *durability* pada ikon *Inventory*.
    *   Jika `currentDurability <= 0`, panggil `BreakItem()`.
*   **`private void BreakItem()`**
    *   Mainkan SFX barang patah. Hapus item dari tangan pemain dan dari *Inventory*.