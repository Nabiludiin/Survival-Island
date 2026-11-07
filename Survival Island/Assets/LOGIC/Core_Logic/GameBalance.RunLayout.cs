namespace SurvivalIsland
{
    /// <summary>
    /// Tambahan config seed run untuk RunLayoutSources dan RunLayoutValidator (#89). Blueprint: Core_Logic/RunSeedManager.md.
    /// Ditulis di file terpisah karena GameBalanceSections.cs adalah file bersama.
    /// </summary>
    public partial class RunSeedBalance
    {
        /// <summary>
        /// Jumlah NPC kapal per run, dibatasi jumlah NPC di RunLayoutSources.shipNpcs. Tafsiran sementara: 3, yaitu semua
        /// NPC di daftar #76 (NPCRoster.md), sehingga yang berubah tiap run adalah posisinya. Menunggu keputusan developer.
        /// </summary>
        public int shipNpcCount = 3;

        /// <summary>Waktu jalan terlama dari titik NPC kapal ke sekoci, syarat 5 (ShipHazardManager.md "Aturan rute": 20 detik).</summary>
        public float maxShipNpcWalkSeconds = 20f;
    }
}
