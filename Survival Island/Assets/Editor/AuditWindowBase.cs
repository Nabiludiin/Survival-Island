using System.Text;
using UnityEditor;
using UnityEngine;

namespace SurvivalIsland.EditorTools
{
    /// <summary>
    /// Dasar jendela audit Editor: tombol Periksa, status lulus, dan laporan teks.
    /// Dipakai SaveIdAuditWindow, ShipLootAudit, ResourceAuditWindow, dan TechTreeAuditWindow.
    /// Hasil juga dicatat ke Console, supaya bisa dibaca tanpa membuka jendela (mis. lewat Unity MCP).
    /// </summary>
    public abstract class AuditWindowBase : EditorWindow
    {
        private Vector2 scroll;
        private string report = "Tekan Periksa.";
        private bool passed;
        private bool hasRun;

        /// <summary>Judul pendek untuk log Console.</summary>
        protected abstract string AuditName { get; }

        /// <summary>Isi laporan ke sb, lalu kembalikan jumlah masalah. 0 berarti lulus.</summary>
        protected abstract int Audit(StringBuilder sb);

        /// <summary>Tombol tambahan di bawah tombol Periksa, mis. perbaikan otomatis.</summary>
        protected virtual void DrawExtraButtons() { }

        protected void Run()
        {
            var sb = new StringBuilder();
            int problems = Audit(sb);
            passed = problems == 0;
            hasRun = true;
            report = sb.ToString();
            string summary = $"[{AuditName}] {(passed ? "Lulus" : problems + " masalah")}.";
            if (passed) Debug.Log(summary + "\n" + report);
            else Debug.LogWarning(summary + "\n" + report);
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Periksa")) Run();
            DrawExtraButtons();
            var type = !hasRun ? MessageType.None : passed ? MessageType.Info : MessageType.Warning;
            EditorGUILayout.HelpBox(!hasRun ? "Belum diperiksa." : passed ? "Lulus." : "Belum lulus.", type);
            scroll = EditorGUILayout.BeginScrollView(scroll);
            EditorGUILayout.TextArea(report, GUILayout.ExpandHeight(true));
            EditorGUILayout.EndScrollView();
        }

        /// <summary>Buka jendela dan langsung periksa.</summary>
        protected static void OpenAndRun<T>(string title) where T : AuditWindowBase
        {
            var w = GetWindow<T>(title);
            w.Run();
        }
    }
}
