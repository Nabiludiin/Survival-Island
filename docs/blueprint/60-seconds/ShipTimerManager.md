# Blueprint: ShipTimerManager.cs
**Lokasi:** `docs/blueprint/60-seconds/ShipTimerManager.md`
**Deskripsi Utama:** Sistem ini tidak hanya menghitung mundur, tetapi juga mengatur atmosfer kepanikan (*tension escalation*) melalui manipulasi audio dinamis dan efek visual kamera. Ini adalah *Controller* utama untuk Fase 1.

## 1. Arsitektur & Variabel Utama
*   `[Header("Time Settings")]`
    *   `private float currentTime = 60.0f;`
    *   `private bool isTimeUp = false;`
*   `[Header("UI & Visual References")]`
    *   `[SerializeField] private TextMeshProUGUI timerText;`
    *   `[SerializeField] private Image vignetteOverlay;` (Efek gelap di pinggir layar yang menebal saat waktu mau habis).
    *   `[SerializeField] private Image fadePanel;` (Untuk transisi Fade to Black).
    *   `[SerializeField] private AnimationCurve panicCurve;` (Kurva untuk mengatur intensitas getaran UI secara *smooth*).
*   `[Header("Audio Settings")]`
    *   `[SerializeField] private AudioSource tickTockAudio;`
    *   `[SerializeField] private AudioSource shipGroanAudio;` (Suara besi kapal melengkung).
    *   `private float basePitch = 1.0f, maxPitch = 2.0f;`

## 2. Detail Logika & Fungsi
*   **`void Update()`**
    *   Jika `isTimeUp`, abaikan update.
    *   `currentTime -= Time.deltaTime;`
    *   **Dynamic Tension Logic (Waktu < 15 detik):**
        *   Hitung rasio kepanikan: `float panicFactor = 1.0f - (currentTime / 15.0f);`
        *   Ubah *pitch* audio secara dinamis: `tickTockAudio.pitch = Mathf.Lerp(basePitch, maxPitch, panicFactor);`
        *   Ubah transparansi Vignette merah di layar: `vignetteOverlay.color = new Color(1, 0, 0, panicFactor * 0.5f);`
        *   Getarkan UI Teks (*Shake Effect*): `timerText.rectTransform.anchoredPosition = originalPos + (Random.insideUnitCircle * panicCurve.Evaluate(panicFactor) * 10f);`
    *   **Time Up Trigger:** Jika `currentTime <= 0f`, panggil `ExecuteTimeUp()`.

## 3. Transisi & Penyimpanan Data (Edge Cases)
*   **`void ExecuteTimeUp()`**
    *   Set `isTimeUp = true;`
    *   **Kunci Input:** Nonaktifkan skrip `ShipPlayerController` agar pemain tidak bisa mengambil barang lagi setelah waktu habis.
    *   Mainkan SFX Sirine/Air masuk.
    *   Jalankan Coroutine `FadeToBlackAndLoad()`.
*   **`IEnumerator FadeToBlackAndLoad()`**
    *   Gunakan `Mathf.Lerp` di dalam blok `while (fadePanel.color.a < 1)` untuk transisi *fade-out* selama 2.5 detik.
    *   **Data Persistence:** Ambil data dari `InventoryManager`, lalu simpan ke `GameManager.Instance.savedShipLoot` (menggunakan *Singleton* dan `DontDestroyOnLoad`).
    *   Muat scene `IslandPhase` lewat `RunLifecycle` (#82), yang memakai *Async* agar tidak *freeze*.