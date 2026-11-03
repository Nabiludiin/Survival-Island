# Blueprint: AudioManager.cs
**Lokasi:** `docs/blueprint/Core_Logic/AudioManager.md`
**Deskripsi Utama:** Pusat kendali sistem suara global. Mengelola musik latar (BGM) dinamis untuk siang/malam, efek suara (SFX) global, dan pengaturan volume master/spesifik.

## 1. Arsitektur & Variabel Utama
*   `public static AudioManager Instance;` (Singleton Pattern).
*   `[Header("Audio Sources")]`
    *   `public AudioSource bgmSource;` (Untuk musik latar).
    *   `public AudioSource sfxSource;` (Untuk efek suara UI atau global).
*   `[Header("Audio Clips")]`
    *   `public AudioClip dayBGM;`
    *   `public AudioClip nightBGM;`
    *   `public AudioClip dangerBGM;` (Saat dikejar horde zombie).
*   `[Header("Volume Settings")]`
    *   `[Range(0f, 1f)] public float masterVolume = 1f;`
    *   `[Range(0f, 1f)] public float bgmVolume = 1f;`
    *   `[Range(0f, 1f)] public float sfxVolume = 1f;`

## 2. Detail Logika & Fungsi
*   **`void Awake()`**
    *   Terapkan Singleton: `if (Instance == null) Instance = this; else Destroy(gameObject);`
*   **`void Start()`**
    *   *Subscribe* ke Event waktu dari `DayNightDirector`:
        `DayNightDirector.OnDaybreak += PlayDayMusic;`
        `DayNightDirector.OnNightfall += PlayNightMusic;`
*   **`public void PlaySFX(AudioClip clip, float volumeMultiplier = 1f)`**
    *   Fungsi global agar skrip lain mudah memutar suara (misal: tombol UI ditekan).
    *   Gunakan `sfxSource.PlayOneShot(clip, volumeMultiplier * sfxVolume * masterVolume);`
*   **`public void PlayDayMusic()` / `PlayNightMusic()`**
    *   Jangan langsung mengganti `bgmSource.clip`. Gunakan Coroutine `CrossfadeBGM(AudioClip newClip)` agar transisi lagunya halus.
*   **`private IEnumerator CrossfadeBGM(AudioClip newClip)`**
    *   Turunkan volume `bgmSource` secara bertahap (menggunakan `Mathf.Lerp`) selama 2 detik hingga 0.
    *   Ganti `bgmSource.clip = newClip;` lalu mainkan `bgmSource.Play();`
    *   Naikkan kembali volume `bgmSource` secara bertahap ke target `bgmVolume * masterVolume` selama 2 detik.
*   **`public void UpdateVolume(float master, float bgm, float sfx)`**
    *   Fungsi ini dipanggil oleh UI Settings Menu jika pemain menggeser slider volume.
    *   Simpan pengaturan ini ke `PlayerPrefs` agar tersimpan saat game ditutup.