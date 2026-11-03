# Blueprint: SurvivalStats.cs
**Lokasi:** `docs/blueprint/raft/SurvivalStats.md`
**Deskripsi Utama:** Sistem biologi karakter. Sangat detail dalam menghitung *Decay Rates* (tingkat kebusukan/lapar) dan menghadirkan efek *Post-Processing* saat kondisi karakter kritis (mata buram karena infeksi).

## 1. Arsitektur & Variabel Utama
*   `[Header("Core Stats (0 to 100)")]`
    *   `public float HP, MaxHP = 100f;`
    *   `public float Stamina, MaxStamina = 100f;` (Pindah dari `IslandPlayerController`, lihat #111).
    *   `public float Hunger, MaxHunger = 100f;`
    *   `public float Thirst, MaxThirst = 100f;`
    *   `public float Infection, MaxInfection = 100f;`
*   `[Header("Decay & Damage Modifiers")]`
    *   Angka dibaca dari `GameBalanceConfig.vitals` (#84): `hungerDrainPerSec`, `thirstDrainPerSec`, `starvationDamagePerSec`, `dehydrationDamagePerSec`, `frostbiteStaminaDrainPerSec`, dan `frostbiteDamagePerSec`.
*   `[Header("Events & Visuals")]`
    *   `EventChannel<PlayerDiedEvent>` dan `EventChannel<VitalsChangedEvent>` (#107).
    *   `[SerializeField] private Volume postProcessVolume;` (Untuk efek *Vignette/Chromatic Aberration* saat infeksi tinggi).

## 2. Detail Logika & Fungsi
*   **`void Update()`**
    *   **Hunger Update:** `Hunger = Mathf.Clamp(Hunger - (hungerDrainPerSec * Time.deltaTime), 0, MaxHunger);`
    *   **Thirst Update:** `Thirst = Mathf.Clamp(Thirst - (thirstDrainPerSec * Time.deltaTime), 0, MaxThirst);`
    *   **Starvation, Dehydration, dan Frostbite:** Mengikuti tabel prioritas di `raft/VitalsRules.md` (#111).
    *   **Infection Effects:** 
        *   Jika `Infection > 50f`, tingkatkan *Chromatic Aberration* pada post-processing untuk membuat penglihatan pemain berbayang hijau/buram. Hitung intensitas menggunakan: `(Infection - 50f) / 50f`.
    *   **Death Check:**
        *   Jika `HP <= 0`, panggil `TriggerDeath()` dengan alasan dari sumber damage terakhir (#59).
        *   Jika `Infection >= 100`, panggil `TriggerDeath(DeathReason.Zombified)`.
*   **`public void Heal(float amount, string type)`**
    *   Fungsi modular. Jika `type == "Vaccine"`, kurangi `Infection`. Jika `type == "Food"`, tambah `Hunger`. Jika `type == "Water"`, tambah `Thirst`.
*   **`private void TriggerDeath(DeathReason reason)`**
    *   Disable pergerakan, mainkan animasi kamera jatuh ke tanah.
    *   **Permadeath Logic:** Kirim `PlayerDiedEvent`. Penghapusan save dan pemuatan scene hanya dilakukan `RunLifecycle` (#82). `PlayerPrefs.DeleteAll()` dilarang, karena juga menghapus pengaturan pemain.