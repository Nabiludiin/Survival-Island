# Blueprint: EnemyAI.cs
**Lokasi:** `docs/blueprint/left-4-dead/EnemyAI.md`
**Deskripsi Utama:** Sistem otak NavMesh yang sangat optimal. Mengatur State Machine (FSM), kalkulasi rute (Pathfinding), dan mekanik merusak struktur markas jika jalan zombie ke pemain terhalang (Obstacle Checking).

## 1. Arsitektur & Variabel Utama
*   `[Header("NavMesh & State")]`
    *   `public enum AIState { Idle, Chase, AttackPlayer, AttackBuilding, BurnToAshes }` (Daftar lengkap ada di `GLOSSARY.md`).
    *   `public AIState currentState;`
    *   `private NavMeshAgent agent;`
    *   `private Transform playerTransform;`
*   `[Header("Combat & Raycast")]`
    *   `public float attackRange = 2.0f;`
    *   `public float damage = 15f;`
    *   `public LayerMask playerLayer, baseStructureLayer;`
    *   `private float pathUpdateDelay = 0.5f;` (Jangan hitung rute tiap frame, berat!).
*   `[Header("Effects")]`
    *   `public ParticleSystem burnEffect;` (Partikel api saat siang).

## 2. Detail Logika & Fungsi
*   **`void OnEnable()` / `void OnDisable()`**
    *   Wajib berlangganan ke *Event Director*: `EventChannel<DaybreakEvent>.Raised += TriggerDaybreakDeath;`
    *   Pastikan *unsubscribe* di `OnDisable` agar tidak bocor memori (*memory leak*).
*   **`IEnumerator AIBehaviorLoop()`**
    *   Gunakan Coroutine (bukan `Update`) dengan jeda `WaitForSeconds(0.2f)` untuk optimasi FSM.
    *   **State.Chase:**
        *   Hitung rute: `agent.SetDestination(playerTransform.position);`
        *   Jika `Vector3.Distance(transform, playerTransform) <= attackRange`, ubah ke `State.AttackPlayer`.
    *   **State.AttackPlayer / AttackBuilding:**
        *   Hentikan langkah: `agent.isStopped = true;`
        *   Mainkan animasi *Attack*. Lakukan `Physics.OverlapSphere` di depan *transform* untuk memberikan *damage* (dan Infeksi) ke pemain atau Dinding.
    *   **Obstruction Check (PENTING):**
        *   Jika `agent.pathStatus == NavMeshPathStatus.PathPartial` atau kecepatan `agent.velocity.magnitude < 0.1f`, berarti zombie menyangkut (kemungkinan terhalang *base* pemain).
        *   Tembakkan `Physics.Raycast` ke depan. Jika menabrak `baseStructureLayer`, pindah *state* ke `State.AttackBuilding` untuk menghancurkan tembok tersebut.
*   **`void TriggerDaybreakDeath(DaybreakEvent e)`**
    *   Jika objek ini adalah Zombie:
    *   Set *State* ke `BurnToAshes`. Hentikan NavMesh (`agent.enabled = false`).
    *   Aktifkan `burnEffect.Play()`.
    *   Jalankan Coroutine pelelehan (kurangi skala Y ke 0 perlahan) lalu kembalikan lewat `ObjectPooler.Instance.ReturnToPool(...)` (JANGAN `Destroy()`).