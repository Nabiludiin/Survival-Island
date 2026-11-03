# Blueprint: NPCCompanion.cs
**Lokasi:** `docs/blueprint/Island_Logic/NPCCompanion.md`
**Deskripsi Utama:** Logika AI NPC teman yang membantu pemain menyerang zombie.

## 1. Arsitektur & Variabel
*   `public Transform playerTarget;`
*   `public NavMeshAgent agent;`
*   `public enum NPCState { Follow, Attack, Idle }`

## 2. Detail Logika & Fungsi
*   **`void Update()`**
    *   **Follow:** Jika jarak dengan pemain > 5 meter, set `agent.SetDestination(playerTarget.position)`.
    *   **Attack:** Jika mendeteksi zombie di dekatnya, set target tembak ke zombie, berhenti berjalan, dan panggil skrip tembakan (Gun Controller NPC).