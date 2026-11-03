# Blueprint: DayNightDirector.cs
**Lokasi:** `docs/blueprint/left-4-dead/DayNightDirector.md`
**Deskripsi Utama:** *Brain* dari sistem dunia. Menggunakan pola desain *Observer* (`C# Events`) untuk mengirimkan sinyal "Pagi" atau "Malam" ke seluruh zombie dan hewan tanpa perlu *FindObjectsOfType* (Sangat optimal).

## 1. Arsitektur & Variabel Utama
*   `[Header("Time Configurations")]`
    *   `public float timeMultiplier = 50f;` (Kecepatan waktu berjalan).
    *   `public float currentTimeOfDay = 6.0f;` (Mulai jam 6 pagi).
    *   `private bool isNight = false;`
*   `[Header("Sun & Moon Light")]`
    *   `public Light directionalLight;`
    *   `public Gradient lightColor;` (Gradasi warna cahaya dari pagi ke malam).
*   `[Header("Director Events")]`
    *   `EventChannel<DaybreakEvent>` (Jam 06:00).
    *   `EventChannel<NightfallEvent>` (Jam 18:00).
    *   Bentuk event mengikuti `Core_Logic/GameEventBus.md` (#107).

## 2. Detail Logika & Fungsi
*   **`void Update()`**
    *   **Time Tick:** `currentTimeOfDay += (Time.deltaTime * timeMultiplier) / 3600f;` (Kalkulasi detik ke jam).
    *   Jika `currentTimeOfDay >= 24.0f`:
        *   `currentTimeOfDay %= 24.0f;`
        *   **Hari dan Win/Loss Check:** Panggil `GameManager.Instance.AdvanceDay();`. `GameManager` pemilik `currentDay` dan yang memeriksa hari ke-60.
    *   **Lighting Math (Rotasi Langit):**
        *   Ubah jam (0-24) menjadi derajat rotasi (0-360): `float sunRotation = (currentTimeOfDay / 24f) * 360f - 90f;`
        *   Rotasikan `directionalLight`: `directionalLight.transform.localRotation = Quaternion.Euler(sunRotation, -30f, 0f);`
        *   Ubah warna cahaya menyesuaikan jam (Senja/Pagi): `directionalLight.color = lightColor.Evaluate(currentTimeOfDay / 24f);`
    *   **Trigger Events (Observer Pattern):**
        *   Jika `currentTimeOfDay >= 18.0f` dan `!isNight`: 
            `isNight = true;` lalu kirim `EventChannel<NightfallEvent>.Publish(...)`.
        *   Jika `currentTimeOfDay >= 6.0f` dan `currentTimeOfDay < 18.0f` dan `isNight`: 
            `isNight = false;` lalu kirim `EventChannel<DaybreakEvent>.Publish(...)`.