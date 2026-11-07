using System.Collections.Generic;

namespace SurvivalIsland
{
    /// <summary>
    /// Batas mayat dan ragdoll sekaligus. Blueprint: left-4-dead/EnemyHitReaction.md bagian "Batas mayat" (#95).
    /// Catatan Langkah 5: ditulis sebagai kelas statis (bukan manager scene), karena isinya hanya antrean; dikosongkan
    /// lewat Clear() saat scene IslandPhase dimulai (SpawnManager.Awake) dan saat fajar memudarkan mayat zombie.
    /// </summary>
    public static class CorpseBudget
    {
        private static readonly List<EnemyHitReaction> corpses = new List<EnemyHitReaction>(16);
        private static int activeRagdolls;

        public static int Count => corpses.Count;

        public static void Register(EnemyHitReaction c)
        {
            if (c == null || corpses.Contains(c)) return;
            corpses.Add(c);
            var bal = GameManager.CurrentBalance;
            int max = bal != null ? bal.hitReaction.maxCorpses : 12;
            // Mayat tertua melewati sisa waktu tunggu dan langsung memudar.
            for (int i = 0; i < corpses.Count - max; i++) if (corpses[i] != null) corpses[i].FadeNow();
        }

        public static void Remove(EnemyHitReaction c)
        {
            corpses.Remove(c);
        }

        public static bool TryAcquireRagdoll()
        {
            var bal = GameManager.CurrentBalance;
            int max = bal != null ? bal.hitReaction.maxRagdolls : 4;
            if (activeRagdolls >= max) return false;
            activeRagdolls++;
            return true;
        }

        public static void ReleaseRagdoll()
        {
            if (activeRagdolls > 0) activeRagdolls--;
        }

        /// <summary>DaybreakEvent: semua mayat zombie langsung memudar (dipanggil SpawnManager).</summary>
        public static void FadeAllZombies()
        {
            // Malam selesai: batas drop zombie (#103) mulai dari 0 lagi.
            EnemyHitReaction.dropsThisNight = 0;
            for (int i = 0; i < corpses.Count; i++)
            {
                var c = corpses[i];
                if (c != null && c.GetComponent<EnemyAI>() != null) c.FadeNow();
            }
        }

        public static void Clear()
        {
            corpses.Clear();
            activeRagdolls = 0;
            EnemyHitReaction.dropsThisNight = 0;
        }
    }
}
