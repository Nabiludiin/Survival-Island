using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Satu-satunya titik Langkah 7 yang memanggil HeavyItemSystem dan HeavyItem (milik Langkah 6, #69).
    /// Nama mengikuti Island_Logic/HeavyItemSystem.md: HeavyItemSystem.Instance, MarkInstalled(partId),
    /// records (PartRecord: partId, state, position), dan HeavyItem dengan partId.
    /// Kalau kode Langkah 6 memakai nama lain, cukup file ini yang disesuaikan saat digabung.
    /// </summary>
    public static class HeavyItemBridge
    {
        public static bool IsAvailable => HeavyItemSystem.Instance != null;

        /// <summary>Ambil partId dari collider bertag HeavyItem. null kalau bukan suku cadang.</summary>
        public static string GetPartId(Collider other)
        {
            if (other == null) return null;
            var item = other.GetComponentInParent<HeavyItem>();
            return item != null ? item.PartId : null;
        }

        /// <summary>HeavyItemSystem melepas barang yang dibawa lebih dulu, lalu menandainya terpasang.</summary>
        public static void MarkInstalled(string partId)
        {
            if (HeavyItemSystem.Instance != null) HeavyItemSystem.Instance.MarkInstalled(partId);
        }

        /// <summary>partId yang berstatus Installed di HeavyItemSystem.records (sumber kebenaran setelah load).</summary>
        public static void CollectInstalled(List<string> result)
        {
            result.Clear();
            var sys = HeavyItemSystem.Instance;
            if (sys == null || sys.records == null) return;
            for (int i = 0; i < sys.records.Length; i++)
            {
                var r = sys.records[i];
                if (r != null && r.state == PartState.Installed) result.Add(r.partId);
            }
        }

        /// <summary>partId barang Lying dalam radius dari center (ReconcileLyingParts, tanpa query fisika).</summary>
        public static void CollectLyingNear(Vector3 center, float radius, List<string> result)
        {
            result.Clear();
            var sys = HeavyItemSystem.Instance;
            if (sys == null || sys.records == null) return;
            float sq = radius * radius;
            for (int i = 0; i < sys.records.Length; i++)
            {
                var r = sys.records[i];
                if (r != null && r.state == PartState.Lying && (r.position - center).sqrMagnitude <= sq) result.Add(r.partId);
            }
        }
    }
}
