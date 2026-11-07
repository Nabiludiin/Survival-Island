using System.Collections.Generic;
using System.Text;
using UnityEditor;

namespace SurvivalIsland.EditorTools
{
    /// <summary>
    /// Validasi tech tree. Blueprint: Core_Logic/TechTreeManager.md bagian "Validasi Editor" (#34).
    /// Memeriksa semua aset TechNodeData:
    /// - nodeId unik dan tidak kosong.
    /// - requiredNodeId kosong atau merujuk node yang ada.
    /// - Tidak ada rantai prasyarat yang berputar.
    /// - triggerKey ada di resep (ItemCrafted) atau BuildableData (StructureBuilt).
    /// - techNodeId di CraftingRecipe dan unlockNodeId di BuildableData merujuk node yang ada.
    /// Juga memeriksa array nodes di tiap TechTreeManager: elemen kosong, dan node anak yang ditaruh sebelum prasyaratnya
    /// (petunjuk, karena satu pemicu baru membuka anaknya pada pemicu berikutnya).
    /// Menu: Survival > Audit > Tech Tree.
    /// </summary>
    public class TechTreeAuditWindow : AuditWindowBase
    {
        protected override string AuditName => "Tech Tree Audit";

        [MenuItem("Survival/Audit/Tech Tree")]
        private static void Open() => OpenAndRun<TechTreeAuditWindow>("Tech Tree Audit");

        protected override int Audit(StringBuilder sb)
        {
            int problems = 0;
            var nodes = AuditUtil.FindAssets<TechNodeData>();
            var recipes = AuditUtil.FindAssets<CraftingRecipe>();
            var buildables = AuditUtil.FindAssets<BuildableData>();

            var recipeIds = new HashSet<string>();
            foreach (var r in recipes) if (!string.IsNullOrEmpty(r.recipeId)) recipeIds.Add(r.recipeId);
            var buildableIds = new HashSet<string>();
            foreach (var b in buildables) if (!string.IsNullOrEmpty(b.id)) buildableIds.Add(b.id);

            problems += CheckNodes(nodes, recipeIds, buildableIds, sb, out var byId);

            // 4. Rujukan dari resep dan bangunan.
            sb.AppendLine("== Rujukan resep dan bangunan ==");
            foreach (var r in recipes)
                if (!string.IsNullOrEmpty(r.techNodeId) && !byId.ContainsKey(r.techNodeId))
                { sb.AppendLine($"Resep {r.recipeId}: techNodeId {r.techNodeId} tidak ada."); problems++; }
            foreach (var b in buildables)
                if (!string.IsNullOrEmpty(b.unlockNodeId) && !byId.ContainsKey(b.unlockNodeId))
                { sb.AppendLine($"Bangunan {b.id}: unlockNodeId {b.unlockNodeId} tidak ada."); problems++; }

            // 5. Array nodes di TechTreeManager pada scene.
            sb.AppendLine("== TechTreeManager di scene ==");
            int managers = 0;
            bool ok = AuditUtil.ForEachScene((scene, path) =>
            {
                foreach (var m in AuditUtil.ComponentsInScene<TechTreeManager>(scene))
                {
                    managers++;
                    var arr = new SerializedObject(m).FindProperty("nodes");
                    var order = new List<string>();
                    for (int i = 0; arr != null && i < arr.arraySize; i++)
                    {
                        var n = arr.GetArrayElementAtIndex(i).objectReferenceValue as TechNodeData;
                        if (n == null) { sb.AppendLine($"{AuditUtil.PathOf(m)}: nodes[{i}] kosong."); problems++; continue; }
                        order.Add(n.nodeId);
                    }
                    for (int i = 0; i < order.Count; i++)
                    {
                        if (!byId.TryGetValue(order[i] ?? string.Empty, out var n) || string.IsNullOrEmpty(n.requiredNodeId)) continue;
                        int req = order.IndexOf(n.requiredNodeId);
                        if (req > i) sb.AppendLine($"Petunjuk: {AuditUtil.PathOf(m)}: {n.nodeId} ditaruh sebelum prasyaratnya {n.requiredNodeId}.");
                    }
                }
            });
            if (!ok) { sb.AppendLine("Dibatalkan: perubahan scene belum disimpan."); problems++; }

            sb.AppendLine($"Diperiksa: {nodes.Count} node, {recipes.Count} resep, {buildables.Count} bangunan, {managers} TechTreeManager.");
            if (nodes.Count == 0) sb.AppendLine("Catatan: belum ada aset TechNodeData. Isi pohon masih usulan (#34).");
            return problems;
        }

        /// <summary>
        /// Pemeriksaan node tanpa membaca aset atau scene: nodeId, prasyarat, pemicu, dan rantai berputar.
        /// Dipisah supaya bisa diuji dengan TechNodeData di memori.
        /// </summary>
        public static int CheckNodes(IList<TechNodeData> nodes, HashSet<string> recipeIds, HashSet<string> buildableIds,
                                     StringBuilder sb, out Dictionary<string, TechNodeData> byId)
        {
            int problems = 0;

            // 1. nodeId unik dan tidak kosong.
            sb.AppendLine("== nodeId ==");
            byId = new Dictionary<string, TechNodeData>();
            foreach (var n in nodes)
            {
                if (string.IsNullOrEmpty(n.nodeId)) { sb.AppendLine($"{n.name}: nodeId kosong."); problems++; continue; }
                if (byId.TryGetValue(n.nodeId, out var other)) { sb.AppendLine($"{n.nodeId}: dipakai {other.name} dan {n.name}."); problems++; continue; }
                byId[n.nodeId] = n;
            }

            // 2. Prasyarat dan pemicu.
            sb.AppendLine("== Prasyarat dan pemicu ==");
            foreach (var n in byId.Values)
            {
                if (!string.IsNullOrEmpty(n.requiredNodeId) && !byId.ContainsKey(n.requiredNodeId))
                { sb.AppendLine($"{n.nodeId}: requiredNodeId {n.requiredNodeId} tidak ada."); problems++; }

                switch (n.trigger)
                {
                    case TechUnlockTrigger.ItemCrafted:
                        if (!recipeIds.Contains(n.triggerKey ?? string.Empty))
                        { sb.AppendLine($"{n.nodeId}: triggerKey '{n.triggerKey}' bukan recipeId yang ada."); problems++; }
                        break;
                    case TechUnlockTrigger.StructureBuilt:
                        if (!buildableIds.Contains(n.triggerKey ?? string.Empty))
                        { sb.AppendLine($"{n.nodeId}: triggerKey '{n.triggerKey}' bukan id BuildableData yang ada."); problems++; }
                        break;
                }
            }

            // 3. Rantai berputar: ikuti requiredNodeId sampai habis atau kembali ke node yang sudah dilewati.
            sb.AppendLine("== Rantai berputar ==");
            var reported = new HashSet<string>();
            foreach (var start in byId.Keys)
            {
                var path = new List<string>();
                var onPath = new HashSet<string>();
                string cur = start;
                while (!string.IsNullOrEmpty(cur) && byId.TryGetValue(cur, out var node))
                {
                    if (!onPath.Add(cur))
                    {
                        int from = path.IndexOf(cur);
                        var loop = path.GetRange(from, path.Count - from);
                        loop.Sort(System.StringComparer.Ordinal);
                        if (reported.Add(string.Join(",", loop)))
                        { sb.AppendLine(string.Join(" -> ", path.GetRange(from, path.Count - from)) + " -> " + cur); problems++; }
                        break;
                    }
                    path.Add(cur);
                    cur = node.requiredNodeId;
                }
            }

            return problems;
        }
    }
}
