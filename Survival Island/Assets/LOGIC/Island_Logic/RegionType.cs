namespace SurvivalIsland
{
    /// <summary>
    /// Empat region pulau (GDD bagian 5). Nilai mengikuti GLOSSARY.md. Pemilik resmi: ZoneManager (Langkah 6).
    /// Ditulis di Langkah 4 karena ResourceNode (jadwal tumbuh per region, #73) dan GameBalanceConfig.resource
    /// sudah memakainya. Langkah 5 dan 6 TIDAK mendeklarasikan ulang enum ini.
    /// </summary>
    public enum RegionType { Volcano, Amazon, Snow, Logistic }
}
