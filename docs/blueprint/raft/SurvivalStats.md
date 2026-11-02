# Blueprint: SurvivalStats.cs
**Lokasi:** `docs/blueprint/raft/SurvivalStats.md`
**Deskripsi Utama:** Sistem biologi karakter. Sangat detail dalam menghitung *Decay Rates* (tingkat kebusukan/lapar) dan menghadirkan efek *Post-Processing* saat kondisi karakter kritis (mata buram karena infeksi).

## 1. Arsitektur & Variabel Utama
*   `[Header("Core Stats (0 to 100)")]`
    *   `public float HP, MaxHP = 100f;`
    *   `public float Hunger, MaxHunger = 100f;`
    *   `public float Infection, MaxInfection = 100f;`
*   `[Header("Decay & Damage Modifiers")]`
    *   `public float hungerDrainPerSec = 0.3f;`
    *   `public float starvationDamagePerSec = 1.5f;`
    *   `public float frostbiteDamagePerSec = 5.0f;` (Berlaku di Region 3).
*   `[Header("Events & Visuals")]`
    *   `public Action<DeathReason> OnPlayerDeath;` (C# Event untuk men-trigger Game Over).
    *   `[SerializeField] private Volume postProcessVolume;` (Untuk efek *Vignette/Chromatic Aberration* saat infeksi tinggi).

## 2. Detail Logika & Fungsi
*   **`void Update()`**
    *   **Hunger Update:** `Hunger = Mathf.Clamp(Hunger - (hungerDrainPerSec * Time.deltaTime), 0, MaxHunger);`
    *   **Starvation:** `if (Hunger == 0) HP -= starvationDamagePerSec * Time.deltaTime;`
    *   **Infection Effects:** 
        *   Jika `Infection > 50f`, tingkatkan *Chromatic Aberration* pada post-processing untuk membuat penglihatan pemain berbayang hijau/buram. Hitung intensitas menggunakan: `(Infection - 50f) / 50f`.
    *   **Death Check:**
        *   Jika `HP <= 0`, panggil `TriggerDeath(DeathReason.Starvation)`.
        *   Jika `Infection >= 100`, panggil `TriggerDeath(DeathReason.Zombified)`.
*   **`public void Heal(float amount, string type)`**
    *   Fungsi modular. Jika `type == "Vaccine"`, kurangi `Infection`. Jika `type == "Food"`, tambah `Hunger`.
*   **`private void TriggerDeath(DeathReason reason)`**
    *   Disable pergerakan, mainkan animasi kamera jatuh ke tanah.
    *   **Permadeath Logic:** Hapus file Save Data menggunakan `PlayerPrefs.DeleteAll()` atau hapus file JSON lokal. Panggil `SceneManager.LoadScene("ShipPhase")` untuk melempar pemain ke awal.