# Blueprint: AsyncSceneLoader.cs
**Lokasi:** `docs/blueprint/Core_Logic/AsyncSceneLoader.md`
**Deskripsi Utama:** Meload *scene* Pulau di latar belakang tanpa membuat game *freeze*.

## 1. Arsitektur & Variabel
*   `public Slider loadingSlider;`
*   `public TextMeshProUGUI progressText;`

## 2. Detail Logika & Fungsi
*   **`public void LoadScene(string sceneName)`**
    *   Mulai Coroutine `LoadSceneAsync(sceneName)`.
*   **`IEnumerator LoadSceneAsync(string sceneName)`**
    *   Jalankan `AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);`
    *   Gunakan loop `while (!operation.isDone)`:
    *   Ubah nilai `loadingSlider.value = operation.progress;`