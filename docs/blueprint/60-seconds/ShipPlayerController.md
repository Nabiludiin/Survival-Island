# Blueprint: ShipPlayerController.cs (First-Person Edition)
**Lokasi:** `docs/blueprint/60-seconds/ShipPlayerController.md`
**Deskripsi Utama:** Skrip pergerakan dan kamera murni **First-Person**. Skrip ini menangani rotasi leher (Mouse Look), pergerakan badan (WASD), mekanik goyangan layar akibat kapal tenggelam (*Ship Sway*), dan tembakan *Raycast* langsung dari titik tengah mata pemain.

## 1. Arsitektur & Variabel Utama
*   `[Header("First Person Camera Settings")]`
    *   `[SerializeField] private Camera firstPersonCamera;`
    *   `public float mouseSensitivity = 2f;`
    *   `public float maxLookAngle = 80f;` (Batas melihat ke atas/bawah agar leher tidak patah).
    *   `private float xRotation = 0f;` (Rotasi sumbu X kamera).
*   `[Header("Movement Settings")]`
    *   `[SerializeField] private CharacterController controller;`
    *   `public float moveSpeed = 5f;`
*   `[Header("Ship Sway (Simulasi Tenggelam)")]`
    *   `public float swayAmount = 5f;` (Derajat kemiringan kapal maksimal).
    *   `public float swaySpeed = 1f;`
*   `[Header("Interaction System (Crosshair)")]`
    *   `public float interactRange = 3f;`
    *   `private IInteractable currentHoveredObject;`

## 2. Detail Logika & Fungsi
*   **`void Start()`**
    *   Kunci dan sembunyikan kursor mouse agar pemain bisa melihat sekeliling dengan bebas:
        `Cursor.lockState = CursorLockMode.Locked;`
        `Cursor.visible = false;`

*   **`void Update()`**
    *   **1. First-Person Mouse Look (Menoleh):**
        *   Ambil input: `float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;`
        *   Ambil input: `float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;`
        *   Hitung rotasi vertikal (melihat ke atas/bawah): 
            `xRotation -= mouseY;`
            `xRotation = Mathf.Clamp(xRotation, -maxLookAngle, maxLookAngle);`
        *   Putar badan pemain (Kiri/Kanan): `transform.Rotate(Vector3.up * mouseX);`
    *   **2. Ship Sway Effect (Kamera Miring):**
        *   Gunakan matematika gelombang (*Sine Wave*) untuk memiringkan layar ke kiri dan kanan (Sumbu Z) seakan kapal terombang-ambing.
        *   `float zSway = Mathf.Sin(Time.time * swaySpeed) * swayAmount;`
        *   Terapkan rotasi akhir ke kamera: 
            `firstPersonCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, zSway);`
    *   **3. First-Person Movement (Berjalan):**
        *   `float x = Input.GetAxis("Horizontal");`
        *   `float z = Input.GetAxis("Vertical");`
        *   `Vector3 move = transform.right * x + transform.forward * z;`
        *   `controller.Move(move * moveSpeed * Time.deltaTime);`
    *   **4. Crosshair Interaction (Raycast dari Mata):**
        *   Selalu tembakkan `Raycast` dari posisi dan arah kamera:
            `PhysicsQuery.Raycast(ray, out RaycastHit hit, interactRange, PhysicsMasks.Interact);`
        *   Jika mengenai objek `IInteractable`, ubah UI Crosshair di tengah layar menjadi ikon tangan terbuka, dan tunggu tombol `E` untuk mengambil.

## 3. Catatan Khusus untuk AI (Claude)
*   **Raycast Origin:** Pastikan raycast interaksi *selalu* ditembakkan dari `firstPersonCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0))` yang merupakan titik tengah layar persis, bukan dari posisi dada pemain.
*   **Kamera Terpisah:** Komponen Camera HARUS dijadikan *child* dari GameObject Pemain dan diletakkan di koordinat `Y = 1.6f` (ketinggian mata orang dewasa).