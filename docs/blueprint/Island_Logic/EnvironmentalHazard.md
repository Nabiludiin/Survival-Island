# Blueprint: EnvironmentalHazard.cs
**Lokasi:** `docs/blueprint/Island_Logic/EnvironmentalHazard.md`
**Deskripsi Utama:** Logika untuk objek interaktif yang berbahaya, seperti Tong Merah (Explosive Barrel).

## 1. Arsitektur & Variabel
*   `public float explosionRadius = 5f;`
*   `public float explosionDamage = 100f;`
*   `public float explosionForce = 500f;`
*   `public GameObject explosionVFX;`

## 2. Detail Logika & Fungsi
*   **`public void Explode()`**
    *   Munculkan efek ledakan: `Instantiate(explosionVFX, transform.position, transform.rotation);`
    *   Gunakan `Physics.OverlapSphere` untuk mendeteksi semua *collider* (pemain atau musuh) di sekitar radius.
    *   Kurangi HP target, dan jika musuh punya `Rigidbody`, tambahkan `AddExplosionForce`.
    *   `Destroy(gameObject);`