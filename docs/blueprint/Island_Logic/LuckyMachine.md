# Blueprint: LuckyMachine.cs
**Lokasi:** `docs/blueprint/Island_Logic/LuckyMachine.md`
**Deskripsi Utama:** Mesin Gacha. Ditenagai oleh energi kiamat (kematian zombie atau energi erupsi). Memberikan RNG *Loot*.

## 1. Arsitektur & Variabel Utama
*   `[Header("Gacha Config")]`
    *   `public int energyCost = 100;`
    *   `public int currentEnergy = 0;`
    *   `public Transform spawnPoint;`
*   `[Header("Loot Tables")]`
    *   `public GameObject[] tierSLoot;` (5% Chance - Laser, Fire Axe).
    *   `public GameObject[] tierALoot;` (25% Chance - Medkit, Vaccine).
    *   `public GameObject[] tierBLoot;` (70% Chance - Wood, Ammo).

## 2. Detail Logika & Fungsi
*   **`public void AddEnergy(int amount)`**
    *   Dipanggil oleh *EnemyAI* saat mati (`AddEnergy(10)`). Jika `currentEnergy >= energyCost`, aktifkan lampu mesin (Bisa diputar).
*   **`public void RollGacha()`**
    *   Mainkan animasi mesin berputar (*Slot Machine UI* atau *Mesh Rotation*).
    *   Hitung probabilitas: `float roll = Random.Range(0f, 100f);`
    *   `if (roll <= 5f) SpawnLoot(tierSLoot);`
    *   `else if (roll <= 30f) SpawnLoot(tierALoot);`
    *   `else SpawnLoot(tierBLoot);`
    *   Kurangi `currentEnergy -= energyCost;`