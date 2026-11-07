using System.Collections.Generic;

namespace SurvivalIsland
{
    /// <summary>
    /// Pembagi token menukik kawanan Gagak, daftar statis per groupId. Blueprint: left-4-dead/WildlifeAI.md
    /// "Kawanan Gagak (FlockDive)" (#61). Paling banyak wildlife.maxDiversAtOnce Gagak satu kelompok menukik sekaligus.
    /// Token dilepas WildlifeAI saat naik kembali, mati, lari ke sarang, atau kembali ke pool.
    /// </summary>
    public static class CrowFlock
    {
        private static readonly Dictionary<int, List<int>> holders = new Dictionary<int, List<int>>();

        public static bool TryAcquire(int groupId, int memberId, int maxDivers)
        {
            if (!holders.TryGetValue(groupId, out var list))
            {
                list = new List<int>(4);
                holders[groupId] = list;
            }
            if (list.Contains(memberId)) return true;
            if (list.Count >= maxDivers) return false;
            list.Add(memberId);
            return true;
        }

        public static void Release(int groupId, int memberId)
        {
            if (!holders.TryGetValue(groupId, out var list)) return;
            list.Remove(memberId);
            if (list.Count == 0) holders.Remove(groupId);
        }

        public static int DiversIn(int groupId) => holders.TryGetValue(groupId, out var list) ? list.Count : 0;
    }
}
