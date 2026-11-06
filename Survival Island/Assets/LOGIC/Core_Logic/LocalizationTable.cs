using System;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Satu baris kunci dan teks. Blueprint: Core_Logic/LocalizationManager.md (#104).</summary>
    [Serializable]
    public struct LocalizedRow
    {
        public string key;
        [TextArea] public string text;
    }

    /// <summary>
    /// Tabel teks satu bahasa, hasil impor CSV. Blueprint: Core_Logic/LocalizationManager.md (#104).
    /// Usulan disetujui developer: tabel kunci sendiri (bukan paket Unity Localization), bahasa awal dan cadangan "id".
    /// </summary>
    [CreateAssetMenu(menuName = "Survival/Localization Table")]
    public class LocalizationTable : ScriptableObject
    {
        public string localeId;       // "id" atau "en"
        public string cultureName;    // "id-ID" atau "en-US"
        public LocalizedRow[] rows = new LocalizedRow[0];

#if UNITY_EDITOR
        /// <summary>
        /// Isi rows dari teks CSV dua kolom (key,text). Kolom boleh diapit tanda kutip ganda; "" di dalam kutip = satu kutip.
        /// Baris pertama yang berisi "key" dianggap judul. Dipakai menu konteks Editor.
        /// </summary>
        public void ImportCsv(string csv)
        {
            var list = new System.Collections.Generic.List<LocalizedRow>();
            int i = 0;
            var field = new System.Text.StringBuilder();
            var fields = new System.Collections.Generic.List<string>(2);
            bool inQuotes = false;
            while (i <= csv.Length)
            {
                char c = i < csv.Length ? csv[i] : '\n';
                if (inQuotes)
                {
                    if (c == '"' && i + 1 < csv.Length && csv[i + 1] == '"') { field.Append('"'); i += 2; continue; }
                    if (c == '"') { inQuotes = false; i++; continue; }
                    field.Append(c); i++; continue;
                }
                if (c == '"') { inQuotes = true; i++; continue; }
                if (c == ',') { fields.Add(field.ToString()); field.Clear(); i++; continue; }
                if (c == '\r') { i++; continue; }
                if (c == '\n')
                {
                    fields.Add(field.ToString()); field.Clear();
                    if (fields.Count >= 2 && !string.IsNullOrWhiteSpace(fields[0])
                        && !(list.Count == 0 && fields[0].Trim() == "key"))
                        list.Add(new LocalizedRow { key = fields[0].Trim(), text = fields[1] });
                    fields.Clear();
                    i++; continue;
                }
                field.Append(c); i++;
            }
            rows = list.ToArray();
            UnityEditor.EditorUtility.SetDirty(this);
        }

        [ContextMenu("Impor CSV...")]
        private void ImportCsvFromFile()
        {
            string path = UnityEditor.EditorUtility.OpenFilePanel("Impor CSV lokalisasi", "", "csv");
            if (string.IsNullOrEmpty(path)) return;
            ImportCsv(System.IO.File.ReadAllText(path));
        }
#endif
    }
}
