using System.Collections.Generic;
using System.Text;
using UnityEditor;

namespace SurvivalIsland.EditorTools
{
    /// <summary>
    /// Penghitung node panen per region. Blueprint: raft/ResourceRespawn.md bagian "Kepadatan per region" (#73).
    /// Menghitung ResourceNode di semua scene, lalu membandingkannya dengan GameBalanceConfig.resource.minNodesPerRegion.
    /// Region node dibaca dari RegionSceneRoot induknya (scene region #88), atau dari field region di Inspector.
    /// Menu: Survival > Audit > Resource.
    /// </summary>
    public class ResourceAuditWindow : AuditWindowBase
    {
        private const int Trees = 0, Rocks = 1, Mushrooms = 2, SapTrees = 3, Unknown = 4;
        private static readonly string[] KindNames = { "pohon", "batu besar", "jamur", "pohon getah", "herba lain" };

        protected override string AuditName => "Resource Audit";

        [MenuItem("Survival/Audit/Resource")]
        private static void Open() => OpenAndRun<ResourceAuditWindow>("Resource Audit");

        protected override int Audit(StringBuilder sb)
        {
            var config = AuditUtil.LoadConfig();
            var minimums = config != null ? config.resource.minNodesPerRegion : new ResourceBalance().minNodesPerRegion;
            if (config == null) sb.AppendLine("Peringatan: GameBalanceConfig tidak ditemukan, memakai tabel awal.");

            var counts = new Dictionary<RegionType, int[]>();
            var noData = new List<string>();
            int total = 0;

            bool ok = AuditUtil.ForEachScene((scene, path) =>
            {
                foreach (var node in AuditUtil.ComponentsInScene<ResourceNode>(scene))
                {
                    if (node.Data == null) { noData.Add(AuditUtil.PathOf(node)); continue; }
                    var region = RegionSceneRoot.TryGetRegionOf(node, out var r) ? r : node.Region;
                    if (!counts.TryGetValue(region, out var arr)) counts[region] = arr = new int[KindNames.Length];
                    arr[KindOf(node.Data)]++;
                    total++;
                }
            });
            if (!ok) { sb.AppendLine("Dibatalkan: perubahan scene belum disimpan."); return 1; }

            int problems = 0;
            sb.AppendLine("== Kepadatan per region (ada / minimum) ==");
            foreach (var min in minimums)
            {
                counts.TryGetValue(min.region, out var arr);
                arr = arr ?? new int[KindNames.Length];
                int[] need = { min.trees, min.rocks, min.mushrooms, min.sapTrees };
                var line = new StringBuilder($"{min.region}:");
                for (int k = 0; k < need.Length; k++)
                {
                    bool low = arr[k] < need[k];
                    if (low) problems++;
                    line.Append($" {KindNames[k]} {arr[k]}/{need[k]}{(low ? " KURANG" : "")},");
                }
                if (arr[Unknown] > 0) line.Append($" {KindNames[Unknown]} {arr[Unknown]},");
                sb.AppendLine(line.ToString().TrimEnd(','));
            }

            sb.AppendLine("== ResourceNode tanpa data ==");
            foreach (var n in noData) sb.AppendLine(n);
            problems += noData.Count;

            sb.AppendLine($"Total node: {total}.");
            return problems;
        }

        /// <summary>
        /// Jenis untuk tabel kepadatan. MedicalHerb dibedakan lewat nodeTypeId (Node_Mushroom dan Node_SapTree),
        /// karena keduanya memakai NodeType yang sama.
        /// </summary>
        private static int KindOf(ResourceNodeData d)
        {
            switch (d.nodeType)
            {
                case NodeType.Tree: return Trees;
                case NodeType.Rock: return Rocks;
            }
            string id = (d.nodeTypeId ?? string.Empty).ToLowerInvariant();
            if (id.Contains("mushroom") || id.Contains("jamur")) return Mushrooms;
            if (id.Contains("sap") || id.Contains("getah")) return SapTrees;
            return Unknown;
        }
    }
}
