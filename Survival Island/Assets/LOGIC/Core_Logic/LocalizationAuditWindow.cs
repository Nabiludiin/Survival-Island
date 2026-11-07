#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Alat Editor pemeriksa kunci teks. Blueprint: Core_Logic/LocalizationManager.md bagian "LocalizationAuditWindow" (#104).
    /// Melaporkan: kunci yang hilang di salah satu bahasa, kunci yang tidak ditemukan di kode atau aset,
    /// dan kata "Press [" di kode atau aset. Pemeriksaan ini wajib lulus sebelum build.
    /// Menu: Survival > Localization > Audit.
    ///
    /// "Tidak terpakai" dicari sebagai teks kunci utuh di file .cs, .asset, .prefab, dan .unity. Kunci yang dirakit
    /// lewat kode (mis. "build.error." + error) bisa terlapor tidak terpakai; laporan ini hanya petunjuk.
    /// </summary>
    public class LocalizationAuditWindow : EditorWindow
    {
        private Vector2 scroll;
        private string report = "Tekan Periksa.";
        private bool passed;

        [MenuItem("Survival/Localization/Audit")]
        private static void Open()
        {
            GetWindow<LocalizationAuditWindow>("Localization Audit");
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Periksa")) Run();
            EditorGUILayout.HelpBox(passed ? "Lulus." : "Belum lulus atau belum diperiksa.", passed ? MessageType.Info : MessageType.Warning);
            scroll = EditorGUILayout.BeginScrollView(scroll);
            EditorGUILayout.TextArea(report, GUILayout.ExpandHeight(true));
            EditorGUILayout.EndScrollView();
        }

        private void Run()
        {
            var sb = new StringBuilder();
            int problems = 0;

            // 1. Kumpulkan semua tabel.
            var tables = new List<LocalizationTable>();
            foreach (var guid in AssetDatabase.FindAssets("t:LocalizationTable"))
            {
                var t = AssetDatabase.LoadAssetAtPath<LocalizationTable>(AssetDatabase.GUIDToAssetPath(guid));
                if (t != null) tables.Add(t);
            }
            if (tables.Count == 0) { report = "Tidak ada aset LocalizationTable."; passed = false; return; }

            var keysPerTable = new List<HashSet<string>>();
            var allKeys = new HashSet<string>();
            foreach (var t in tables)
            {
                var set = new HashSet<string>();
                if (t.rows != null)
                    foreach (var r in t.rows)
                        if (!string.IsNullOrEmpty(r.key)) { set.Add(r.key); allKeys.Add(r.key); }
                keysPerTable.Add(set);
            }

            // 2. Kunci yang hilang di salah satu bahasa.
            sb.AppendLine("== Kunci hilang ==");
            foreach (var key in allKeys)
            {
                for (int i = 0; i < tables.Count; i++)
                {
                    if (keysPerTable[i].Contains(key)) continue;
                    sb.AppendLine($"[{tables[i].localeId}] {key}");
                    problems++;
                }
            }

            // 3. Baca file teks proyek sekali.
            var files = new List<string>();
            foreach (var ext in new[] { "*.cs", "*.asset", "*.prefab", "*.unity" })
                files.AddRange(Directory.GetFiles(Application.dataPath, ext, SearchOption.AllDirectories));
            var contents = new List<string>(files.Count);
            foreach (var f in files)
            {
                try { contents.Add(File.ReadAllText(f)); }
                catch (IOException) { contents.Add(string.Empty); }
            }

            // 4. Kata "Press [" di kode atau aset (teks tombol wajib lewat GetBindingDisplay).
            sb.AppendLine("== \"Press [\" ==");
            string selfPath = Path.GetFullPath(Path.Combine(Application.dataPath, "LOGIC/Core_Logic/LocalizationAuditWindow.cs"));
            for (int i = 0; i < files.Count; i++)
            {
                if (Path.GetFullPath(files[i]) == selfPath) continue;
                if (contents[i].Contains("Press ["))
                {
                    sb.AppendLine(files[i].Substring(Application.dataPath.Length));
                    problems++;
                }
            }

            // 5. Kunci yang tidak ditemukan di kode atau aset (petunjuk, tidak menggagalkan).
            sb.AppendLine("== Mungkin tidak terpakai (petunjuk) ==");
            var tablePaths = new HashSet<string>();
            foreach (var t in tables) tablePaths.Add(Path.GetFullPath(AssetDatabase.GetAssetPath(t)));
            foreach (var key in allKeys)
            {
                bool found = false;
                for (int i = 0; i < files.Count && !found; i++)
                {
                    if (tablePaths.Contains(Path.GetFullPath(files[i]))) continue;
                    if (contents[i].Contains(key)) found = true;
                }
                if (!found) sb.AppendLine(key);
            }

            passed = problems == 0;
            report = sb.ToString();
        }
    }
}
#endif
