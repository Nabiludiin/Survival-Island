# Blueprint: EnvironmentalHazard.cs
**Lokasi:** `docs/blueprint/Island_Logic/EnvironmentalHazard.md`
**Deskripsi Utama:** Logika untuk objek interaktif yang berbahaya, seperti Tong Merah (Explosive Barrel).

## 1. Arsitektur & Variabel
*   `public float explosionRadius = 5f;`
*   `public float explosionDamage = 100f;`
*   `public float explosionForce = 500f;`
*   `public string explosionVFXPoolTag;` (Efek ledakan sering muncul, jadi memakai `ObjectPooler`).

## 2. Detail Logika & Fungsi
*   **`public void Explode()`**
    *   Munculkan efek ledakan: `ObjectPooler.Instance.SpawnFromPool(explosionVFXPoolTag, transform.position, transform.rotation);`
    *   Gunakan `PhysicsQuery.OverlapSphere` dengan `PhysicsMasks.Explosion` untuk mendeteksi *collider* di sekitar radius.
    *   Saring target unik lewat `PhysicsQuery.CollectUniqueDamageables` (#59), supaya satu musuh hanya kena satu kali.
    *   Kurangi HP target lewat `IDamageable`, dan jika musuh punya `Rigidbody`, tambahkan `AddExplosionForce`.
    *   Tong diletakkan di peta dan tidak sering muncul, jadi `Destroy(gameObject);` tetap boleh, dengan statusnya dicatat ke save (#80). Kalau tong dibuat berulang saat bermain, pindahkan ke `ObjectPooler`.