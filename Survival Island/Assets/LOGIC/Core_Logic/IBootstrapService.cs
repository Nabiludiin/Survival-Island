namespace SurvivalIsland
{
    /// <summary>
    /// Kontrak manager persisten di bawah PersistentRoot. RunLifecycle memanggil keduanya sesuai urutan bootstrap
    /// (tabel "Urutan bootstrap" di Core_Logic/RunLifecycle.md, #82), bukan urutan Awake Unity.
    ///
    /// Aturan: Awake hanya mengisi Instance. Pekerjaan yang bergantung pada manager lain dilakukan di Initialize().
    /// </summary>
    public interface IBootstrapService
    {
        /// <summary>Dipanggil sekali, sesuai urutan bootstrap.</summary>
        void Initialize();

        /// <summary>Dipanggil saat run lama dibersihkan, sesuai urutan yang sama.</summary>
        void ResetForNewRun();
    }
}
