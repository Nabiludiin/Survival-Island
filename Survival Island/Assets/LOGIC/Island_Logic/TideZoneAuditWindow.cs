#if UNITY_EDITOR
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace SurvivalIsland
{
    /// <summary>
    /// Alat Editor: memeriksa area pantai yang bisa terendam. Blueprint: Island_Logic/TideManager.md bagian
    /// "Area terendam dan #86" (#91). Sampel grid 2 meter di TideManager.tideZone, melaporkan:
    /// tanah di bawah MaxWaterY yang belum dicat pasir (GetSandWeight &lt; 0,5), titik itu di luar Area NavMesh Tidal,
    /// dan TideSafePoint di bawah MaxWaterY.
    ///
    /// Catatan implementasi (sistem pendukung): dijalankan saat Play Mode, karena SurfaceDetection dan TideManager
    /// diisi lewat bootstrap dan scene IslandPhase.
    /// </summary>
    public class TideZoneAuditWindow : EditorWindow
    {
        private const float GridStep = 2f;
        private const float SandMin = 0.5f;
        private const int MaxListed = 50;

        private string report = "Masuk Play Mode di IslandPhase, lalu tekan Periksa.";
        private Vector2 scroll;

        [MenuItem("Survival/Audit Area Pasang (TideZone)")]
        private static void Open() => GetWindow<TideZoneAuditWindow>("Audit Pasang");

        private void OnGUI()
        {
            if (GUILayout.Button("Periksa")) report = RunAudit();
            scroll = EditorGUILayout.BeginScrollView(scroll);
            EditorGUILayout.TextArea(report, GUILayout.ExpandHeight(true));
            EditorGUILayout.EndScrollView();
        }

        private static string RunAudit()
        {
            var tide = TideManager.Instance;
            var sd = SurfaceDetection.Instance;
            if (!Application.isPlaying || tide == null || sd == null) return "Butuh Play Mode dengan TideManager dan SurfaceDetection aktif.";

            float maxY = tide.MaxWaterY;
            int tidalArea = NavMesh.GetAreaFromName(TideManager.TidalAreaName);
            var notSand = new List<Vector3>();
            var notTidal = new List<Vector3>();
            var z = tide.TideZone;
            for (float x = z.min.x; x <= z.max.x; x += GridStep)
            {
                for (float zz = z.min.z; zz <= z.max.z; zz += GridStep)
                {
                    var p = new Vector3(x, z.center.y, zz);
                    if (!sd.TryGetTerrainSurfaceAtXZ(p, out var s) || s.groundHeight >= maxY) continue;
                    var ground = new Vector3(x, s.groundHeight, zz);
                    if (sd.GetSandWeight(ground) < SandMin) notSand.Add(ground);
                    if (tidalArea >= 0 && NavMesh.SamplePosition(ground, out var hit, 1f, NavMesh.AllAreas) && (hit.mask & (1 << tidalArea)) == 0)
                        notTidal.Add(ground);
                }
            }

            var sb = new StringBuilder();
            sb.AppendLine($"MaxWaterY = {maxY:0.00}");
            sb.AppendLine($"Tanah terendam yang belum pasir: {notSand.Count}");
            for (int i = 0; i < notSand.Count && i < MaxListed; i++) sb.AppendLine("  " + notSand[i]);
            if (tidalArea < 0) sb.AppendLine("Area NavMesh 'Tidal' belum dibuat.");
            else
            {
                sb.AppendLine($"Tanah terendam di luar Area Tidal: {notTidal.Count}");
                for (int i = 0; i < notTidal.Count && i < MaxListed; i++) sb.AppendLine("  " + notTidal[i]);
            }
            var safe = tide.SafePoints;
            int badSafe = 0;
            if (safe != null)
                for (int i = 0; i < safe.Length; i++)
                    if (safe[i] != null && safe[i].Position.y < maxY) { badSafe++; sb.AppendLine("  TideSafePoint rendah: " + safe[i].name); }
            sb.AppendLine($"TideSafePoint di bawah MaxWaterY: {badSafe}");
            return sb.ToString();
        }
    }
}
#endif
