namespace SurvivalIsland
{
    /// <summary>
    /// Pembaca siang/malam untuk sistem Langkah 5, tanpa status sendiri.
    /// Catatan Langkah 5: DayNightDirector (NightfallEvent, IsDay) milik Langkah 4 dan belum digabung. Sesuai pembagian
    /// kepemilikan, Langkah 5 membaca HourChangedEvent (sticky) dan membandingkannya dengan jam di DayNightDirector.md:
    /// malam mulai 18:00, siang mulai 06:00 (spawn.nightStartHour / spawn.dayStartHour).
    /// Setelah DayNightDirector digabung, pendengar boleh pindah ke NightfallEvent/DaybreakEvent; kelas ini tetap benar.
    /// GDD bagian 3: siang hewan buas, malam horde zombie.
    /// </summary>
    public static class DayPhaseTracker
    {
        /// <summary>Jam terakhir dari HourChangedEvent. Tanpa event (run baru jam 6), dianggap jam 6.</summary>
        public static int CurrentHour => EventChannel<HourChangedEvent>.HasLast ? EventChannel<HourChangedEvent>.Last.hour : 6;

        public static bool IsNight => IsNightHour(CurrentHour);

        public static bool IsNightHour(int hour)
        {
            var bal = GameManager.CurrentBalance;
            int night = bal != null ? bal.spawn.nightStartHour : 18;
            int day = bal != null ? bal.spawn.dayStartHour : 6;
            return night > day ? (hour >= night || hour < day) : (hour >= night && hour < day);
        }
    }
}
