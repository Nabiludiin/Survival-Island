namespace SurvivalIsland
{
    /// <summary>
    /// Empat region pulau. Nilai mengikuti GLOSSARY.md bagian 2. Pemilik resmi: ZoneManager (Langkah 6).
    /// Ditulis di Langkah 5 karena RegionSpawnTable, SpawnManager, dan AIDirector sudah memakainya.
    /// Langkah 6 TIDAK mendeklarasikan ulang enum ini.
    /// GDD bagian 5: Region 1 Gunung Berapi, Region 2 Amazon, Region 3 Es, Region 4 Desa Logistik.
    /// </summary>
    public enum RegionType { Volcano, Amazon, Snow, Logistic }
}
