# Blueprint: WeatherManager.cs
**Lokasi:** `docs/blueprint/Island_Logic/WeatherManager.md`
**Deskripsi Utama:** Sistem pergantian cuaca dinamis secara acak atau berbasis hari. Mengatur perubahan visual (partikel hujan/kabut), audio (suara badai), dan dampaknya pada *gameplay* (jarak pandang dan suhu pemain).

## 1. Arsitektur & Variabel Utama
*   `public static WeatherManager Instance;`
*   `public enum WeatherType { Clear, Rain, Fog, Storm }`
*   `public WeatherType currentWeather;`
*   `[Header("Visual & Audio References")]`
    *   `public ParticleSystem rainParticles;`
    *   `public ParticleSystem stormLeavesParticles;`
    *   `public Light directionalLight;` (Untuk digelapkan saat badai).
    *   `public AudioClip rainSFX, stormSFX;`
*   `[Header("Weather Settings")]`
    *   `public float weatherChangeInterval = 300f;` (Berubah setiap 5 menit waktu dunia nyata).
    *   `private float weatherTimer;`

## 2. Detail Logika & Fungsi
*   **`void Awake()`**
    *   Implementasi Singleton.
*   **`void Update()`**
    *   Hitung mundur `weatherTimer -= Time.deltaTime;`.
    *   Jika `weatherTimer <= 0`, panggil `RandomizeWeather()`.
*   **`private void RandomizeWeather()`**
    *   Pilih cuaca secara acak menggunakan probabilitas. Contoh: 50% Clear, 25% Rain, 15% Fog, 10% Storm.
    *   Panggil `SetWeather(newWeather);`
    *   Reset `weatherTimer` kembali ke `weatherChangeInterval`.
*   **`public void SetWeather(WeatherType newWeather)`**
    *   Ubah `currentWeather = newWeather`.
    *   Jika **Clear**: Matikan partikel hujan, kembalikan intensitas cahaya normal, matikan SFX cuaca.
    *   Jika **Rain**: Nyalakan `rainParticles`, turunkan intensitas cahaya 30%, mainkan `rainSFX` via `AudioManager`, turunkan status `Warmth` pemain secara perlahan.
    *   Jika **Fog**: Aktifkan `RenderSettings.fog = true` dan tingkatkan `fogDensity` agar jarak pandang sangat pendek.
    *   Jika **Storm**: Nyalakan `rainParticles` & `stormLeavesParticles`, intensitas cahaya sangat gelap, mainkan `stormSFX`, dan buat kecepatan berjalan pemain sedikit melambat karena angin badai.