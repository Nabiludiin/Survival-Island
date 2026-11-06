namespace SurvivalIsland
{
    /// <summary>
    /// Bagian BaseHealth yang dipakai BuildManager dan StructureRegistry (#79).
    ///
    /// Catatan implementasi (Langkah 4): BaseHealth milik DamagePipeline (Langkah 5). Supaya kode Langkah 4 tidak bergantung
    /// pada kelas yang belum ditulis, BuildManager dan StructureRegistry memanggil interface ini. BaseHealth ditulis sebagai
    /// "BaseHealth : MonoBehaviour, IDamageable, IStructureHealth" (sudah dikabarkan ke sesi Langkah 5).
    /// Anggota mengikuti tabel BaseHealth di Core_Logic/DamagePipeline.md.
    /// </summary>
    public interface IStructureHealth
    {
        string StructureId { get; }
        BuildableData Data { get; }
        float CurrentHP { get; }
        float HpRatio { get; }
        float LastDamageTime { get; }

        /// <summary>Mengisi data, ID, dan HP saat dibangun atau dipulihkan. Tidak bergantung pada Awake.</summary>
        void Initialize(BuildableData data, string id, float hp);

        /// <summary>Bongkar: terbitkan StructureDestroyedEvent, lalu nonaktifkan dan hapus objek tanpa lewat TakeDamage.</summary>
        void Remove();
    }
}
