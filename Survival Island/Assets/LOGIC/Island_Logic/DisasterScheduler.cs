using System;
using System.Collections.Generic;

namespace SurvivalIsland
{
    /// <summary>
    /// Fungsi murni pembuat jadwal bencana dari seed. Blueprint: Island_Logic/DisasterManager.md bagian 2 "Membuat jadwal" (#70)
    /// dan Core_Logic/RunSeedManager.md (#89). Dipanggil RunSeedManager.BuildLayout (blok pertama) dan
    /// RunSeedManager.EnsureDisasterScheduleThrough (blok berikutnya).
    ///
    /// GDD bagian 6: bencana acak. Aturan: maksimal satu bencana per hari, tidak ada bencana pada hari Blood Moon,
    /// bencana besar tidak sebelum firstMajorDisasterDay dan berjarak minimal minDaysBetweenMajorDisasters (usulan disetujui, #85).
    /// </summary>
    public static class DisasterScheduler
    {
        private static readonly List<DisasterScheduleEntry> buffer = new List<DisasterScheduleEntry>(32);
        private static readonly List<DisasterData> ordered = new List<DisasterData>(3);
        private static readonly int TypeCount = Enum.GetValues(typeof(DisasterType)).Length;

        public static void Build(ref RandomStream s, DisasterData[] data, int fromDay, int toDay,
                                 DisasterScheduleEntry[] previous, out DisasterScheduleEntry[] result)
        {
            buffer.Clear();
            result = Array.Empty<DisasterScheduleEntry>();
            if (data == null || data.Length == 0 || toDay < fromDay) return;

            var bal = GameManager.CurrentBalance;
            int bloodMoonInterval = bal != null ? bal.disaster.bloodMoonIntervalDays : 10;
            int firstMajorDay = bal != null ? bal.runSeed.firstMajorDisasterDay : 3;
            int minMajorGap = bal != null ? bal.runSeed.minDaysBetweenMajorDisasters : 4;

            // Urutan tetap Eruption, Earthquake, TropicalStorm (DisasterManager.md langkah 2), apa pun urutan di Inspector.
            ordered.Clear();
            for (int t = 0; t < TypeCount; t++)
                for (int i = 0; i < data.Length; i++)
                    if (data[i] != null && (int)data[i].type == t) { ordered.Add(data[i]); break; }

            // Cooldown dan jarak bencana besar ikut dihitung dari blok sebelumnya.
            var lastDayByType = new int[TypeCount];
            for (int t = 0; t < TypeCount; t++) lastDayByType[t] = int.MinValue / 2;
            int lastMajorDay = int.MinValue / 2;
            if (previous != null)
            {
                for (int i = 0; i < previous.Length; i++)
                {
                    var e = previous[i];
                    int t = (int)e.type;
                    if (t >= 0 && t < TypeCount && e.day > lastDayByType[t]) lastDayByType[t] = e.day;
                    var d = Find(e.type);
                    if (d != null && d.IsMajor(e.scale) && e.day > lastMajorDay) lastMajorDay = e.day;
                }
            }

            for (int day = fromDay; day <= toDay; day++)
            {
                // Hari Blood Moon dilewati: Blood Moon menang (keputusan 2).
                if (bloodMoonInterval > 0 && day % bloodMoonInterval == 0) continue;

                for (int k = 0; k < ordered.Count; k++)
                {
                    var d = ordered[k];
                    int t = (int)d.type;
                    if (day < d.minDay) continue;
                    if (day - lastDayByType[t] <= d.cooldownDays) continue;

                    float r = s.NextFloat01();
                    if (r >= d.dailyChance) continue;

                    int scale = s.NextFloat01() < d.scale2Chance ? 2 : 1;
                    float startHour = s.Range(d.startHourMin, d.startHourMax);

                    bool major = d.IsMajor(scale);
                    bool majorBlocked = day < firstMajorDay || day - lastMajorDay < minMajorGap;
                    if (major && majorBlocked)
                    {
                        // Keputusan 3: Erupsi selalu besar, jadi dibatalkan. Gempa dan badai turun ke skala 1.
                        if (d.majorMinScale <= 1) continue;
                        scale = 1;
                        major = d.IsMajor(scale);
                    }

                    buffer.Add(new DisasterScheduleEntry { day = day, type = d.type, scale = scale, startHour = startHour });
                    lastDayByType[t] = day;
                    if (major) lastMajorDay = day;
                    break;   // Maksimal satu bencana per hari
                }
            }

            result = buffer.ToArray();
            buffer.Clear();
        }

        private static DisasterData Find(DisasterType type)
        {
            for (int i = 0; i < ordered.Count; i++) if (ordered[i].type == type) return ordered[i];
            return null;
        }
    }
}
