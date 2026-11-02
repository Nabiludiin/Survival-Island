# Blueprint: DiaryManager.cs
**Lokasi:** `docs/blueprint/Island_Logic/DiaryManager.md`
**Deskripsi Utama:** Sistem penceritaan Lore. Menjeda permainan sepenuhnya agar pemain bisa membaca dengan aman.

## 1. Arsitektur & Variabel Utama
*   `public GameObject diaryUIPanel;`
*   `public TextMeshProUGUI titleText, contentText;`
*   `private bool isReading = false;`

## 2. Detail Logika & Fungsi
*   **`public void OpenDiary(string title, string content)`**
    *   Set `titleText.text = title;` dan `contentText.text = content;`
    *   Aktifkan `diaryUIPanel.SetActive(true);`
    *   **Time Freeze:** Set `Time.timeScale = 0f;` (Menghentikan fisika, AI, dan sistem lapar).
    *   Lepas kursor mouse: `Cursor.lockState = CursorLockMode.None; Cursor.visible = true;`
    *   Set `isReading = true;`
*   **`void Update()`**
    *   Jika `isReading` dan pemain menekan `Escape` atau klik tombol Close:
        *   Tutup UI panel.
        *   Kembalikan kursor dan waktu: `Time.timeScale = 1f; Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false;`