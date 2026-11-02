# Blueprint: ResourceNode.cs
**Lokasi:** `docs/blueprint/raft/ResourceNode.md`
**Deskripsi Utama:** Sistem panen (*harvesting*) interaktif. Mengatur berapa kali sebuah objek harus dipukul, validasi alat (kapak vs beliung), dan fisika penyebaran *loot* (*drop scatter physics*) saat hancur.

## 1. Arsitektur & Variabel Utama
*   `[Header("Node Settings")]`
    *   `public enum NodeType { Tree, Rock, MedicalHerb }`
    *   `public NodeType nodeType;`
    *   `public int maxHits = 5;`
    *   `private int currentHits;`
*   `[Header("Loot & Drops")]`
    *   `public GameObject[] dropPrefabs;` (Array item yang akan jatuh, misal: Kayu, Daun).
    *   `public Vector2 dropAmountRange = new Vector2(2, 5);` (Min dan Max jumlah item yang jatuh).
*   `[Header("Feedback Visual & Audio")]`
    *   `public ParticleSystem hitParticles;`
    *   `public AudioClip hitSound, breakSound;`
    *   `private AudioSource audioSource;`

## 2. Detail Logika & Fungsi
*   **`void Awake()`**
    *   Set `currentHits = maxHits`.
    *   Ambil komponen `audioSource = GetComponent<AudioSource>();`
*   **`public void ReceiveHit(int damage, string toolType)`**
    *   **Validasi Alat (Penting):** 
        *   Jika `nodeType == Tree` dan `toolType != "Axe"`, *damage* = 1 (Pemain memukul pohon pakai tangan/kail).
        *   Jika `toolType == "Axe"`, *damage* = 3.
    *   Kurangi HP: `currentHits -= damage;`
    *   Mainkan partikel: `hitParticles.Play();`
    *   Mainkan suara: `audioSource.PlayOneShot(hitSound);`
    *   **Goyangan Objek (Juice):** Gunakan *Tweening* atau manipulasi `transform.localScale` sejenak agar pohon/batu terlihat "terpukul".
    *   Jika `currentHits <= 0`, panggil `ShatterNode()`.
*   **`private void ShatterNode()`**
    *   Hitung jumlah item: `int dropCount = Random.Range((int)dropAmountRange.x, (int)dropAmountRange.y);`
    *   **Scatter Physics (Loot Tersebar):**
        *   Gunakan `for loop` sebanyak `dropCount`.
        *   `GameObject loot = Instantiate(dropPrefabs[Random.Range(0, dropPrefabs.Length)], transform.position + Vector3.up, Quaternion.identity);`
        *   Ambil `Rigidbody rb = loot.GetComponent<Rigidbody>();`
        *   Terapkan daya ledak acak agar item terpental ke segala arah: `rb.AddExplosionForce(500f, transform.position, 3f);`
    *   Mainkan `breakSound` (Gunakan `AudioSource.PlayClipAtPoint` agar suara tidak hilang saat objek hancur).
    *   `Destroy(gameObject);`