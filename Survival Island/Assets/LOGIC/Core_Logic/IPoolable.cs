namespace SurvivalIsland
{
    /// <summary>
    /// Kontrak objek pool. Blueprint: Core_Logic/ObjectPooler.md bagian 1 "Kontrak IPoolable". Claude.md Tahap 6 poin 1 dan 5.
    ///
    /// Urutan dijamin:
    /// - Keluar: posisi dan rotasi diatur, SetActive(true) (Awake pertama dan OnEnable), lalu OnSpawned().
    /// - Kembali: OnReturned(), SetActive(false) (OnDisable), lalu pindah ke PoolStorage.
    ///
    /// Aturan: OnSpawned mengembalikan HP, state, skala, dan komponen ke nilai awal dari data atau prefab.
    /// OnReturned melepas semua rujukan, aman dipanggil dua kali. Data dari pemanggil diisi lewat Setup(...) setelah spawn.
    /// Objek tidak boleh Destroy dirinya sendiri, dan tidak boleh menjadi anak objek scene region atau scene fase.
    /// </summary>
    public interface IPoolable
    {
        void OnSpawned();
        void OnReturned();
    }
}
