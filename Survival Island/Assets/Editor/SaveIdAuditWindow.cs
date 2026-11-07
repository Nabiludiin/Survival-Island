using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SurvivalIsland.EditorTools
{
    /// <summary>
    /// Pemeriksa ID save di semua scene. Blueprint: Core_Logic/WorldStateSave.md bagian "ID stabil" (#80),
    /// dan Core_Logic/LootSpawnerSystem.md bagian "Aturan titik di Editor" (#89, #91).
    /// Melaporkan:
    /// - SaveIdTag dengan saveId kosong.
    /// - ID ganda (SaveIdTag, LootSpawnerSystem.spawnerId, dan SaveId ISaveable lain) dalam satu kelompok scene.
    /// - Objek ISaveable yang membaca ID dari SaveIdTag tetapi tidak punya SaveIdTag.
    /// - LootSpawnerSystem dengan spawnerId kosong.
    /// - Titik loot pulau di bawah batas air pasang tertinggi (TideManager.MaxWaterY, #91).
    /// Pemeriksaan ini wajib lulus sebelum build. Menu: Survival > Audit > Save ID.
    ///
    /// Kelompok scene: ShipPhase, Bootstrap, dan MainMenu masing-masing berdiri sendiri. Scene lain (IslandPhase dan
    /// scene region #88) dimuat bersama, jadi ID-nya harus unik antar-scene.
    /// </summary>
    public class SaveIdAuditWindow : AuditWindowBase
    {
        private const string IslandGroup = "Pulau";

        protected override string AuditName => "Save ID Audit";

        [MenuItem("Survival/Audit/Save ID")]
        private static void Open() => OpenAndRun<SaveIdAuditWindow>("Save ID Audit");

        protected override void DrawExtraButtons()
        {
            if (GUILayout.Button("Perbaiki ID kosong dan ganda (simpan scene)"))
            {
                int fixedCount = FixIds();
                Debug.Log($"[Save ID Audit] {fixedCount} ID dibuat ulang.");
                Run();
            }
        }

        /// <summary>Satu ID yang ditemukan, beserta cara mengubahnya kalau perlu dibuat ulang.</summary>
        private struct IdEntry
        {
            public string group;
            public string id;
            public string label;
        }

        protected override int Audit(StringBuilder sb)
        {
            int problems = 0;
            var entries = new List<IdEntry>();
            var emptyTags = new List<string>();
            var emptySpawners = new List<string>();
            var missingTags = new List<string>();
            var emptyOther = new List<string>();
            var lowPoints = new List<string>();
            float maxWaterY = MaxWaterY(out string tideNote);

            bool ok = AuditUtil.ForEachScene((scene, path) =>
            {
                string group = AuditUtil.IsStandalone(path) ? scene.name : IslandGroup;
                bool islandScene = group == IslandGroup;

                foreach (var tag in AuditUtil.ComponentsInScene<SaveIdTag>(scene))
                {
                    if (string.IsNullOrEmpty(tag.SaveId)) emptyTags.Add(AuditUtil.PathOf(tag));
                    else entries.Add(new IdEntry { group = group, id = tag.SaveId, label = AuditUtil.PathOf(tag) });
                }

                foreach (var spawner in AuditUtil.ComponentsInScene<LootSpawnerSystem>(scene))
                {
                    if (string.IsNullOrEmpty(spawner.SpawnerId)) emptySpawners.Add(AuditUtil.PathOf(spawner));
                    else entries.Add(new IdEntry { group = group, id = spawner.SpawnerId, label = AuditUtil.PathOf(spawner) + " (spawnerId)" });

                    // #91: titik loot pulau tidak boleh di bawah batas air pasang.
                    if (!islandScene) continue;
                    var defs = spawner.SpawnPointDefs;
                    for (int i = 0; i < defs.Count; i++)
                    {
                        var point = defs[i] != null ? defs[i].point : null;
                        if (point == null) continue;
                        if (point.position.y < maxWaterY)
                            lowPoints.Add($"{AuditUtil.PathOf(spawner)} titik {i}: y {point.position.y:0.00} < {maxWaterY:0.00}");
                    }
                }

                foreach (var mb in AuditUtil.ComponentsInScene<MonoBehaviour>(scene))
                {
                    if (!(mb is ISaveable saveable) || mb is LootSpawnerSystem) continue;
                    if (ReadsIdFromTag(mb.GetType()))
                    {
                        // ID objek ini sudah dihitung lewat SaveIdTag-nya.
                        if (mb.GetComponent<SaveIdTag>() == null) missingTags.Add($"{AuditUtil.PathOf(mb)} ({mb.GetType().Name})");
                        continue;
                    }
                    string id = SafeSaveId(saveable);
                    if (string.IsNullOrEmpty(id)) emptyOther.Add($"{AuditUtil.PathOf(mb)} ({mb.GetType().Name})");
                    else entries.Add(new IdEntry { group = group, id = id, label = $"{AuditUtil.PathOf(mb)} ({mb.GetType().Name})" });
                }
            });
            if (!ok) { sb.AppendLine("Dibatalkan: perubahan scene belum disimpan."); return 1; }

            problems += Section(sb, "SaveIdTag tanpa saveId", emptyTags);
            problems += Section(sb, "LootSpawnerSystem tanpa spawnerId", emptySpawners);
            problems += Section(sb, "ISaveable yang butuh SaveIdTag tetapi tidak punya", missingTags);

            sb.AppendLine("== ID ganda ==");
            foreach (var group in GroupDuplicates(entries))
            {
                sb.AppendLine($"[{group.Key}]");
                foreach (var label in group.Value) sb.AppendLine("  " + label);
                problems++;
            }

            sb.AppendLine($"== Titik loot di bawah air pasang ({tideNote}) ==");
            foreach (var l in lowPoints) sb.AppendLine(l);
            problems += lowPoints.Count;

            // Petunjuk saja: sebagian manager baru mengisi SaveId saat Awake, jadi kosong di Editor belum tentu salah.
            sb.AppendLine("== SaveId kosong di Editor (petunjuk, tidak menggagalkan) ==");
            foreach (var l in emptyOther) sb.AppendLine(l);

            sb.AppendLine($"Diperiksa: {entries.Count} ID di {AuditUtil.SceneAssetPaths().Count} scene.");
            return problems;
        }

        /// <summary>Buat ulang ID kosong, dan ID ganda selain kemunculan pertama. Scene yang berubah disimpan.</summary>
        private int FixIds()
        {
            // Lintasan 1: ID mana yang ganda per kelompok.
            var counts = new Dictionary<string, int>();
            AuditUtil.ForEachScene((scene, path) =>
            {
                string group = AuditUtil.IsStandalone(path) ? scene.name : IslandGroup;
                foreach (var tag in AuditUtil.ComponentsInScene<SaveIdTag>(scene)) Count(counts, group, tag.SaveId);
                foreach (var sp in AuditUtil.ComponentsInScene<LootSpawnerSystem>(scene)) Count(counts, group, sp.SpawnerId);
            });

            // Lintasan 2: urutan sama, jadi kemunculan pertama tetap dan sisanya dibuat ulang.
            var seen = new HashSet<string>();
            int changed = 0;
            AuditUtil.ForEachScene((scene, path) =>
            {
                string group = AuditUtil.IsStandalone(path) ? scene.name : IslandGroup;
                foreach (var tag in AuditUtil.ComponentsInScene<SaveIdTag>(scene))
                    if (NeedsNewId(counts, seen, group, tag.SaveId))
                        changed += Regenerate(tag, "saveId", PrefixOf(tag.SaveId, new SerializedObject(tag).FindProperty("idPrefix")?.stringValue ?? "obj"));
                foreach (var sp in AuditUtil.ComponentsInScene<LootSpawnerSystem>(scene))
                    if (NeedsNewId(counts, seen, group, sp.SpawnerId))
                        changed += Regenerate(sp, "spawnerId", PrefixOf(sp.SpawnerId, path.Contains("ShipPhase") ? "ship_loot" : "loot"));
            }, save: true);
            return changed;
        }

        private static void Count(Dictionary<string, int> counts, string group, string id)
        {
            if (string.IsNullOrEmpty(id)) return;
            string key = group + "|" + id;
            counts.TryGetValue(key, out int n);
            counts[key] = n + 1;
        }

        private static bool NeedsNewId(Dictionary<string, int> counts, HashSet<string> seen, string group, string id)
        {
            if (string.IsNullOrEmpty(id)) return true;
            string key = group + "|" + id;
            if (!counts.TryGetValue(key, out int n) || n < 2) return false;
            return !seen.Add(key);   // kemunculan pertama dibiarkan
        }

        private static int Regenerate(Component c, string property, string prefix)
        {
            var so = new SerializedObject(c);
            var prop = so.FindProperty(property);
            if (prop == null) return 0;
            prop.stringValue = prefix + "_" + GUID.Generate().ToString().Substring(0, 8);
            so.ApplyModifiedPropertiesWithoutUndo();
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(c.gameObject.scene);
            return 1;
        }

        /// <summary>Awalan ID lama sebelum 8 karakter acak terakhir, mis. r1_tree_a3f9c21b menjadi r1_tree.</summary>
        private static string PrefixOf(string id, string fallback)
        {
            if (string.IsNullOrEmpty(id)) return fallback;
            int cut = id.LastIndexOf('_');
            return cut > 0 ? id.Substring(0, cut) : fallback;
        }

        private static Dictionary<string, List<string>> GroupDuplicates(List<IdEntry> entries)
        {
            var byKey = new Dictionary<string, List<string>>();
            foreach (var e in entries)
            {
                string key = e.group + " | " + e.id;
                if (!byKey.TryGetValue(key, out var list)) byKey[key] = list = new List<string>();
                list.Add(e.label);
            }
            var dup = new Dictionary<string, List<string>>();
            foreach (var kv in byKey) if (kv.Value.Count > 1) dup[kv.Key] = kv.Value;
            return dup;
        }

        private static int Section(StringBuilder sb, string title, List<string> items)
        {
            sb.AppendLine("== " + title + " ==");
            foreach (var i in items) sb.AppendLine(i);
            return items.Count;
        }

        /// <summary>True kalau tipe ini menyimpan SaveIdTag di field, artinya SaveId-nya dibaca dari SaveIdTag.</summary>
        private static bool ReadsIdFromTag(Type type)
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
            for (var t = type; t != null && t != typeof(MonoBehaviour); t = t.BaseType)
                foreach (var f in t.GetFields(flags))
                    if (f.FieldType == typeof(SaveIdTag)) return true;
            return false;
        }

        private static string SafeSaveId(ISaveable s)
        {
            try { return s.SaveId; }
            catch (Exception) { return null; }
        }

        /// <summary>
        /// Tinggi air pasang tertinggi: lowWaterY + (1 + stormSurgeLevel) * (highWaterY - lowWaterY),
        /// sama dengan TideManager.MaxWaterY. Tanpa config memakai nilai awal blueprint.
        /// </summary>
        private static float MaxWaterY(out string note)
        {
            var config = AuditUtil.LoadConfig();
            var t = config != null ? config.tide : new TideBalance();
            float y = t.lowWaterY + (1f + t.stormSurgeLevel) * (t.highWaterY - t.lowWaterY);
            note = $"batas y {y:0.00}{(config == null ? ", config tidak ditemukan" : "")}";
            return y;
        }
    }
}
