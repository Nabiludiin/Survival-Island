# Blueprint: UIManager.cs
**Lokasi:** `docs/blueprint/Core_Logic/UIManager.md`
**Deskripsi Utama:** Pusat kendali seluruh antarmuka Canvas Unity. Mengelola transisi layar (Loading Screen), Menu Pause, dan Game Over Screen secara independen dari logika gameplay.

## 1. Arsitektur & Variabel Utama
*   `public static UIManager Instance;` (Singleton Pattern).
*   `[Header("UI Panels")]`
    *   `public GameObject pauseMenuPanel;`
    *   `public GameObject gameOverPanel;`
    *   `public GameObject loadingScreenPanel;`
*   `[Header("UI Elements")]`
    *   `public TextMeshProUGUI gameOverReasonText;`
    *   `public Slider loadingProgressBar;`
*   `private bool isPaused = false;`

## 2. Detail Logika & Fungsi
*   **`void Awake()`**
    *   Terapkan Singleton: `if (Instance == null) Instance = this; else Destroy(gameObject);`
    *   Pastikan semua panel dinonaktifkan di awal: `pauseMenuPanel.SetActive(false); gameOverPanel.SetActive(false);`
*   **`void Update()`**
    *   Dengarkan input `Escape`: `if (Input.GetKeyDown(KeyCode.Escape)) TogglePause();`
*   **`public void TogglePause()`**
    *   Balikkan status: `isPaused = !isPaused;`
    *   Jika `isPaused`:
        *   Hentikan waktu: `Time.timeScale = 0f;`
        *   Tampilkan panel: `pauseMenuPanel.SetActive(true);`
        *   Munculkan kursor: `Cursor.lockState = CursorLockMode.None; Cursor.visible = true;`
    *   Jika `!isPaused`:
        *   Jalankan waktu: `Time.timeScale = 1f;`
        *   Sembunyikan panel: `pauseMenuPanel.SetActive(false);`
        *   Kunci kursor: `Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false;`
*   **`public void ShowGameOver(string reason)`**
    *   Set `gameOverReasonText.text = reason;`
    *   Aktifkan `gameOverPanel.SetActive(true);`
    *   Hentikan waktu dan munculkan kursor mouse.
*   **`public IEnumerator ShowLoadingScreen(AsyncOperation operation)`**
    *   Aktifkan `loadingScreenPanel.SetActive(true);`
    *   Gunakan perulangan `while (!operation.isDone)` untuk memperbarui nilai `loadingProgressBar.value` sesuai progress *loading*.