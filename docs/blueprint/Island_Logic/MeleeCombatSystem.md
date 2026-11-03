# Blueprint: MeleeCombatSystem.cs
**Lokasi:** `docs/blueprint/Island_Logic/MeleeCombatSystem.md`
**Deskripsi Utama:** Sistem pertarungan jarak dekat menggunakan senjata tumpul/tajam (Kapak, Pisau). Mengandalkan Raycast pendek atau OverlapSphere untuk akurasi pukulan.

## 1. Arsitektur & Variabel
*   `public float meleeDamage = 25f;`
*   `public float attackRange = 2f;`
*   `public float staminaCost = 15f;`
*   `public float attackCooldown = 1f;`
*   `private float nextAttackTime = 0f;`
*   `public LayerMask enemyLayer;`
*   `public Transform cameraTransform;`

## 2. Detail Logika & Fungsi
*   **`void Update()`**
    *   Jika klik kiri ditekan, dan `Time.time >= nextAttackTime`, dan Stamina > `staminaCost`, panggil `PerformMeleeAttack()`.
*   **`private void PerformMeleeAttack()`**
    *   Kurangi stamina pemain. Set `nextAttackTime = Time.time + attackCooldown`.
    *   Picu animasi "Swing" di Animator. Mainkan SFX "Whoosh".
    *   Gunakan `Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, attackRange, enemyLayer)`.
    *   Jika kena, cek `hit.collider.GetComponent<EnemyHealth>()`.
    *   Panggil fungsi `.TakeDamage(meleeDamage)` pada musuh.
    *   Mainkan partikel percikan darah dan SFX pukulan daging.