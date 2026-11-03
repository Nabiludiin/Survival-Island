# Blueprint: HitboxModifier.cs
**Lokasi:** `docs/blueprint/Island_Logic/HitboxModifier.md`
**Deskripsi Utama:** Dipasang di bagian *collider* spesifik musuh (Kepala, Badan, Kaki) untuk membedakan jumlah *damage* (Headshot).

## 1. Arsitektur & Variabel
*   `public EnemyHealth mainHealthScript;`
*   `public float damageMultiplier = 1f;` (Kepala = 2.0f, Kaki = 0.5f).

## 2. Detail Logika & Fungsi
*   **`public void OnHit(float baseDamage)`**
    *   Dipanggil oleh senjata pemain (Raycast/Melee) yang mengenai *collider* ini.
    *   Hitung total: `float finalDamage = baseDamage * damageMultiplier;`
    *   Kirim ke skrip utama: `mainHealthScript.TakeDamage(finalDamage);`