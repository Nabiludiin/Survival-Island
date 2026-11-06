using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Satu-satunya titik Langkah 7 yang memanggil HeavyItemSystem dan HeavyItem (milik Langkah 6, #69).
    /// API mengikuti kode Langkah 6 (commit df3176d06), yang sedikit berbeda dari HeavyItemSystem.md:
    /// records bersifat private dan dibaca lewat PartCount dan GetRecord(i), dan HeavyItem hanya punya Index,
    /// jadi partId = GetRecord(heavyItem.Index).partId.
    /// </summary>
    public static class HeavyItemBridge
    {
        public static bool IsAvailable => HeavyItemSystem.Instance != null;

        /// <summary>Ambil partId dari collider bertag HeavyItem. null kalau bukan suku cadang.</summary>
        public static string GetPartId(Collider other)
        {
            var sys = HeavyItemSystem.Instance;
            if (other == null || sys == null) return null;
            var item = other.GetComponentInParent<HeavyItem>();
            if (item == null || item.Index < 0 || item.Index >= sys.PartCount) return null;
            var r = sys.GetRecord(item.Index);
            return r != null ? r.partId : null;
        }

        /// <summary>HeavyItemSystem melepas barang yang dibawa lebih dulu, lalu menandainya terpasang (tanpa event).</summary>
        public static void MarkInstalled(string partId)
        {
            if (HeavyItemSystem.Instance != null) HeavyItemSystem.Instance.MarkInstalled(partId);
        }

        /// <summary>partId yang berstatus Installed di HeavyItemSystem (sumber kebenaran setelah load).</summary>
        public static void CollectInstalled(List<string> result)
        {
            result.Clear();
            var sys = HeavyItemSystem.Instance;
            if (sys == null) return;
            for (int i = 0; i < sys.PartCount; i++)
            {
                var r = sys.GetRecord(i);
                if (r != null && r.state == PartState.Installed) result.Add(r.partId);
            }
        }

        /// <summary>partId barang Lying dalam radius dari center (ReconcileLyingParts, tanpa query fisika).</summary>
        public static void CollectLyingNear(Vector3 center, float radius, List<string> result)
        {
            result.Clear();
            var sys = HeavyItemSystem.Instance;
            if (sys == null) return;
            float sq = radius * radius;
            for (int i = 0; i < sys.PartCount; i++)
            {
                var r = sys.GetRecord(i);
                if (r != null && r.state == PartState.Lying && (r.position - center).sqrMagnitude <= sq) result.Add(r.partId);
            }
        }
    }
}
