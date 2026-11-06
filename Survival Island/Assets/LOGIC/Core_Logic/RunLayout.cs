using System;

namespace SurvivalIsland
{
    /// <summary>
    /// Susunan run yang sudah lolos pemeriksaan. Blueprint: Core_Logic/RunSeedManager.md bagian 1 "Data run" (#89).
    /// Langkah 1 hanya menulis bentuk datanya. Isi dibangun BuildLayout, yang menunggu RunLayoutSources (lihat RunSeedManager).
    /// </summary>
    [Serializable]
    public class RunLayout
    {
        public int[] partPointByPart = Array.Empty<int>();       // Indeks suku cadang -> indeks titik di PartSpawnPointTable (#69)
        public string[] shipNpcIds = Array.Empty<string>();      // NPC yang muncul di kapal pada run ini (#76)
        public int[] shipNpcPointByNpc = Array.Empty<int>();     // NPC -> titik di kapal
        public DisasterScheduleEntry[] disasterSchedule = Array.Empty<DisasterScheduleEntry>();   // Hari, jenis, skala (#70)
        public int disasterScheduleThroughDay;                    // Hari terakhir yang sudah dijadwalkan
    }

    /// <summary>Data seed yang disimpan WorldStateSave (#80). Blueprint: RunSeedManager.md bagian 1.</summary>
    [Serializable]
    public struct RunSeedSaveData
    {
        public int version;                 // Nilai awal 1
        public uint seed;
        public ulong[] sequentialStates;    // Urutan mengikuti RandomStreamId
        public RunLayout layout;
        public bool usedFallbackLayout;
    }
}
