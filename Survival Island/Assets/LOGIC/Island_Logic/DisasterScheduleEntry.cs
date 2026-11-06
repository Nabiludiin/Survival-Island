using System;

namespace SurvivalIsland
{
    /// <summary>
    /// Jenis bencana. Nilai mengikuti GLOSSARY.md bagian 4. Pemilik resmi: DisasterManager (#70, Langkah 7).
    /// Ditulis di Langkah 1 karena RunLayout (RunSeedManager) dan event bencana sudah memakainya.
    /// Langkah 7 TIDAK boleh mendeklarasikan ulang enum ini.
    /// </summary>
    public enum DisasterType
    {
        Eruption,
        Earthquake,
        TropicalStorm
    }

    /// <summary>
    /// Satu baris jadwal bencana, disimpan di RunSeedManager.CurrentLayout.disasterSchedule.
    /// Bentuk dari Island_Logic/DisasterManager.md bagian 1. Isi jadwal dibuat Langkah 7.
    /// </summary>
    [Serializable]
    public struct DisasterScheduleEntry
    {
        public int day;
        public DisasterType type;
        public int scale;
        public float startHour;
    }
}
