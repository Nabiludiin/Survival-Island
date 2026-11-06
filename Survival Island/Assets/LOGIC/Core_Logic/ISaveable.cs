namespace SurvivalIsland
{
    /// <summary>
    /// Kontrak pemilik data yang disimpan. Tiap sistem menyimpan datanya sendiri, jadi WorldStateSave tidak perlu
    /// mengenal isi tiap sistem. Blueprint: Core_Logic/WorldStateSave.md bagian 1 (#80), ringkasan di SaveSystem.md.
    ///
    /// - Manager di IslandPhase: Register(this) di OnEnable, Unregister(this) di OnDisable.
    /// - Objek region, struktur pemain, dan WorldItem TIDAK mendaftar sendiri (lewat RegionStateStore #88, StructureRegistry #79).
    ///   Objek region memakai RegionSaveRouting.Register: tanpa RegionStreaming (scene uji) baru jalur cadangan Register dipakai.
    /// - Objek pool tidak boleh mendaftar.
    /// </summary>
    public interface ISaveable
    {
        /// <summary>Stabil dan unik dalam satu run. Manager memakai konstanta, contoh "player_vitals".</summary>
        string SaveId { get; }

        /// <summary>Urutan pulih untuk manager di scene dasar (tabel LoadOrder 10-130). Objek region memakai 0.</summary>
        int LoadOrder { get; }

        /// <summary>JSON milik sistem itu. Dictionary wajib diubah ke array (JsonUtility tidak mendukung Dictionary).</summary>
        string CaptureState();

        /// <summary>Hanya mengisi data. TIDAK boleh mengirim event.</summary>
        void RestoreState(string json);

        /// <summary>Mengirim ulang event sticky dan menyegarkan tampilan.</summary>
        void OnRestoreFinished();
    }
}
