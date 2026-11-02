# Blueprint: IslandPlayerController.cs
**Lokasi:** `docs/blueprint/Island_Logic/IslandPlayerController.md`
**Deskripsi Utama:** Skrip pergerakan khusus Fase 2. Berbeda dengan di kapal, di sini pemain bisa berlari, melompat, dan terkena mekanik *Encumbrance* (beban berat) dari Region 4.

## 1. Arsitektur & Variabel Utama
*   `[Header("Movement & Stamina")]`
    *   `public float walkSpeed = 5f, sprintSpeed = 8f;`
    *   `public float jumpHeight = 2f, gravity = -9.81f;`
    *   `public float maxStamina = 100f, currentStamina;`
*   `[Header("Encumbrance (Region 4 Mechanic)")]`
    *   `public bool isCarryingHeavyItem = false;`

## 2. Detail Logika & Fungsi
*   **`void Update()`**
    *   **Encumbrance Check:** 
        *   Jika `isCarryingHeavyItem == true`: Paksa `currentSpeed = walkSpeed * 0.5f`. Disable fungsi *Jump* dan *Sprint*.
    *   **Sprint & Stamina:**
        *   Jika input `Left Shift` ditekan dan `currentStamina > 0` dan `!isCarryingHeavyItem`: Set `currentSpeed = sprintSpeed`, kurangi `currentStamina -= 10f * Time.deltaTime`.
        *   Jika tidak lari, *Regenerate* stamina.
    *   **Gravity & Jump:**
        *   Cek tanah menggunakan `Physics.CheckSphere(groundCheck.position, groundDistance, groundMask)`.
        *   Jika *Grounded* dan input *Space*: `velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity)`.