using System;

namespace SurvivalIsland
{
    /// <summary>
    /// Penyakit yang bisa disembuhkan item (raft/DiseaseManager.md). Pemilik resmi: DiseaseManager (#45).
    /// Ditulis di Langkah 3 karena ItemData.curesDiseases sudah memakainya. Pemiliknya TIDAK mendeklarasikan ulang.
    /// </summary>
    [Flags]
    public enum DiseaseCure { None = 0, FoodPoisoning = 1, Venom = 2, Bleeding = 4, WoundInfection = 8 }
}
