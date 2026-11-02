# Blueprint: ZoneManager.cs
**Lokasi:** `docs/blueprint/Island_Logic/ZoneManager.md`
**Deskripsi Utama:** Ditempelkan pada *BoxCollider* (IsTrigger) berukuran raksasa di masing-masing area untuk mengatur mekanik spesifik 4 Region.

## 1. Arsitektur & Variabel Utama
*   `public enum RegionType { Volcano, Amazon, Snow, Logistic }`
*   `public RegionType myRegion;`
*   `private Coroutine frostbiteCoroutine;`

## 2. Detail Logika & Fungsi
*   **`void OnTriggerEnter(Collider other)`**
    *   Cek apakah `other.CompareTag("Player")`.
    *   Jika `Snow`: Cek atribut `Warmth` pemain. Jika kurang, mulai `frostbiteCoroutine` (mengurangi HP -5 per detik). Ubah *Post-Processing* menjadi nuansa kebiruan.
    *   Jika `Logistic`: Munculkan UI Tutorial *"Heavy Items Nearby. Carrying them will slow you down!"*
*   **`void OnTriggerExit(Collider other)`**
    *   Jika pemain keluar dari area, hentikan semua Coroutine *environment damage* dan normalkan *Post-Processing*.