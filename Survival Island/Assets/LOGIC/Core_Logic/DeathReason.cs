namespace SurvivalIsland
{
    /// <summary>
    /// Penyebab kematian. Nilai mengikuti GLOSSARY.md bagian 4. Pemilik resmi: DamagePipeline (#59, Langkah 5).
    /// Ditulis di Langkah 1 karena GameManager, RunLifecycle, dan WorldStateSave sudah memakainya.
    /// Langkah 5 TIDAK boleh mendeklarasikan ulang enum ini.
    /// </summary>
    public enum DeathReason
    {
        None,          // Nilai pertama. Dipakai RunEndedEvent saat menang (usulan #82, disetujui developer)
        Starvation,
        Dehydration,
        Zombified,
        KilledByEnemy,
        Frostbite,
        Drowned,
        Fall,
        Fire,
        Explosion,
        Poison,
        LeftOnShip
    }
}
