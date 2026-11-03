# Blueprint: HUDManager.cs
**Lokasi:** `docs/blueprint/Core_Logic/HUDManager.md`
**Deskripsi Utama:** Menampilkan informasi *real-time* kepada pemain (Darah, Stamina, Infeksi, Peluru) di layar (Heads-Up Display).

## 1. Arsitektur & Variabel
*   `public static HUDManager Instance;`
*   `[Header("UI Bars")]`
    *   `public Slider healthBar, staminaBar, infectionBar;`
*   `[Header("UI Text")]`
    *   `public TextMeshProUGUI ammoText;`
    *   `public TextMeshProUGUI interactionPromptText;` (Teks pop-up "Press E to Pick Up").

## 2. Detail Logika & Fungsi
*   **`void Awake()`** -> Singleton pattern.
*   **`public void UpdateVitals(float hp, float maxHp, float stamina, float maxStamina, float infection)`**
    *   Ubah nilai UI bar secara mulus: `healthBar.value = hp / maxHp;`
    *   Ubah warna `infectionBar` menjadi lebih merah pekat jika infection > 80%.
*   **`public void UpdateAmmo(int currentAmmo, int maxAmmo)`**
    *   Format teks: `ammoText.text = currentAmmo + " / " + maxAmmo;`
*   **`public void ShowInteraction(string message)` / `HideInteraction()`**
    *   Aktifkan/nonaktifkan `interactionPromptText` saat mendeteksi objek.