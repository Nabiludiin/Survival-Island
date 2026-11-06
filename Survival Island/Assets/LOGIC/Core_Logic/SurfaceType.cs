namespace SurvivalIsland
{
    /// <summary>
    /// Jenis permukaan tanah. Nilai mengikuti GLOSSARY.md. Pemilik resmi: SurfaceDetection (#86, Langkah 4).
    /// Ditulis di Langkah 3 karena GameBalanceConfig.traversal.surfaceProfiles dan PlayerTraversal sudah memakainya.
    /// Langkah 4 TIDAK mendeklarasikan ulang enum ini.
    /// </summary>
    public enum SurfaceType { Pasir, Rumput, Tanah, Batu, Salju, Lumpur, Kayu, Logam }
}
