using UnityEngine;

namespace SurvivalIsland.EditorTools
{
    /// <summary>
    /// Koordinat greybox pulau untuk menu Setup, mengikuti peta usulan RegionStreaming.md:
    /// tiap region 1000 x 1000 m; Region 1 Volcano X 1000-2000 Z 0-1000, Region 2 Amazon X 1000-2000 Z 1000-2000,
    /// Region 4 Desa X 0-1000 Z 1000-2000, Region 3 Salju X 0-1000 Z 2000-3000. Seam S1 (1500, 1000) jurang dan jembatan,
    /// S2 (1000, 1500), S3 (500, 2000), helipad (500, 2150). Tinggi tidak tertulis di dokumen: semua lantai datar di Y = 0.
    /// </summary>
    internal static class IslandLayout
    {
        public const float RegionSize = 1000f;

        // Titik acuan isi Region 1 dari menu Langkah 3 dan 4 (pantai selatan, tempat mendarat).
        // Isi lama yang dulu di sekitar (0, 0, 0) digeser ke sini; pantai pasir berada di Z < 35.
        public static readonly Vector3 R1Origin = new Vector3(1500f, 0f, 60f);

        public static readonly Vector3 R1Min = new Vector3(1000f, 0f, 0f);
        public static readonly Vector3 R2Min = new Vector3(1000f, 0f, 1000f);
        public static readonly Vector3 R4Min = new Vector3(0f, 0f, 1000f);
        public static readonly Vector3 R3Min = new Vector3(0f, 0f, 2000f);

        public static readonly Vector3 S1 = new Vector3(1500f, 0f, 1000f);
        public static readonly Vector3 S2 = new Vector3(1000f, 0f, 1500f);
        public static readonly Vector3 S3 = new Vector3(500f, 0f, 2000f);
        public static readonly Vector3 Helipad = new Vector3(500f, 0f, 2150f);

        // RegionGateSystem.md: lebar jurang minimal 30 m (region.ravineMinWidth).
        public const float RavineWidth = 30f;
    }
}
