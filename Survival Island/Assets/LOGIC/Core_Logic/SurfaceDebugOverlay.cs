#if UNITY_EDITOR || DEVELOPMENT_BUILD
using UnityEngine;
using UnityEngine.Rendering;

namespace SurvivalIsland
{
    /// <summary>
    /// Tampilan debug permukaan saat bermain: kisi debugGridCells x debugGridCells di sekitar pemain, satu warna per
    /// SurfaceType, ditambah teks seperti "Pasir (0,62)" untuk titik di bawah pemain.
    /// Blueprint: Core_Logic/SurfaceDetection.md bagian "Tampilan debug" (#86). Dinyalakan perintah surfacedebug (#56).
    /// Hanya ada di Editor dan Development Build.
    ///
    /// Kisi dibaca lewat TryGetTerrainSurfaceAtXZ (tanpa raycast), dan hanya diambil ulang setelah pemain berpindah satu sel,
    /// supaya tidak memanggil ribuan sampel per frame. Garis digambar dengan GL.LINES setelah kamera URP selesai menggambar.
    /// </summary>
    public class SurfaceDebugOverlay : MonoBehaviour
    {
        private static readonly Color[] TypeColors =
        {
            new Color(1f, 0.85f, 0.4f),   // Pasir
            new Color(0.3f, 0.9f, 0.3f),  // Rumput
            new Color(0.6f, 0.4f, 0.2f),  // Tanah
            new Color(0.6f, 0.6f, 0.6f),  // Batu
            new Color(0.9f, 0.95f, 1f),   // Salju
            new Color(0.35f, 0.25f, 0.15f), // Lumpur
            new Color(0.8f, 0.5f, 0.25f), // Kayu
            new Color(0.5f, 0.7f, 0.9f),  // Logam
        };

        private Vector3[] points = new Vector3[0];
        private Color[] colors = new Color[0];
        private bool[] valid = new bool[0];
        private Vector3 sampledCenter = new Vector3(float.MaxValue, 0f, 0f);
        private string underPlayer = string.Empty;
        private Material lineMaterial;
        private GUIStyle style;

        private static SurfaceBalance B => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.surface : null;

        private void OnEnable()
        {
            RenderPipelineManager.endCameraRendering += HandleEndCamera;
            sampledCenter = new Vector3(float.MaxValue, 0f, 0f);
        }

        private void OnDisable()
        {
            RenderPipelineManager.endCameraRendering -= HandleEndCamera;
        }

        private void OnDestroy()
        {
            if (lineMaterial != null) Destroy(lineMaterial);
        }

        private Transform Player
        {
            get
            {
                var p = PlayerTargetable.Instance;
                return p != null ? p.transform : null;
            }
        }

        private void Update()
        {
            var b = B;
            var sd = SurfaceDetection.Instance;
            var player = Player;
            if (b == null || sd == null || player == null) return;

            // Teks titik di bawah pemain (raycast biasa, sama seperti langkah kaki).
            if (sd.TryGetSurfaceAt(player.position, out var s))
                underPlayer = s.type + " (" + s.sandWeight.ToString("0.00") + ")";
            else underPlayer = "-";

            float spacing = Mathf.Max(0.1f, b.debugGridSpacing);
            Vector3 center = player.position;
            if ((center - sampledCenter).sqrMagnitude < spacing * spacing) return;
            sampledCenter = center;
            Resample(sd, b, center, spacing);
        }

        private void Resample(SurfaceDetection sd, SurfaceBalance b, Vector3 center, float spacing)
        {
            int n = Mathf.Clamp(b.debugGridCells, 2, 256);
            int total = n * n;
            if (points.Length != total)
            {
                points = new Vector3[total];
                colors = new Color[total];
                valid = new bool[total];
            }
            float half = (n - 1) * spacing * 0.5f;
            for (int z = 0; z < n; z++)
            {
                for (int x = 0; x < n; x++)
                {
                    int i = z * n + x;
                    var p = new Vector3(center.x - half + x * spacing, center.y, center.z - half + z * spacing);
                    if (sd.TryGetTerrainSurfaceAtXZ(p, out var sample))
                    {
                        p.y = sample.groundHeight + 0.05f;
                        int t = (int)sample.type;
                        var c = t >= 0 && t < TypeColors.Length ? TypeColors[t] : Color.magenta;
                        // Garis merah pada titik yang dilarang untuk bangunan (sand >= beachBuildSandWeight).
                        colors[i] = sample.sandWeight >= b.beachBuildSandWeight ? Color.red : c;
                        valid[i] = true;
                    }
                    else valid[i] = false;
                    points[i] = p;
                }
            }
        }

        private void HandleEndCamera(ScriptableRenderContext context, Camera cam)
        {
            if (points.Length == 0 || cam == null || cam.cameraType != CameraType.Game) return;
            if (lineMaterial == null)
            {
                var shader = Shader.Find("Hidden/Internal-Colored");
                if (shader == null) return;
                lineMaterial = new Material(shader) { hideFlags = HideFlags.HideAndDontSave };
                lineMaterial.SetInt("_ZTest", (int)CompareFunction.Always);
            }
            var b = B;
            int n = b != null ? Mathf.Clamp(b.debugGridCells, 2, 256) : 0;
            if (n * n != points.Length) return;

            lineMaterial.SetPass(0);
            GL.PushMatrix();
            GL.LoadProjectionMatrix(cam.projectionMatrix);
            GL.modelview = cam.worldToCameraMatrix;
            GL.Begin(GL.LINES);
            for (int z = 0; z < n; z++)
            {
                for (int x = 0; x < n; x++)
                {
                    int i = z * n + x;
                    if (!valid[i]) continue;
                    if (x + 1 < n && valid[i + 1]) { GL.Color(colors[i]); GL.Vertex(points[i]); GL.Vertex(points[i + 1]); }
                    if (z + 1 < n && valid[i + n]) { GL.Color(colors[i]); GL.Vertex(points[i]); GL.Vertex(points[i + n]); }
                }
            }
            GL.End();
            GL.PopMatrix();
        }

        private void OnGUI()
        {
            if (style == null) style = new GUIStyle(GUI.skin.label) { fontSize = 16 };
            GUI.Label(new Rect(10f, 10f, 400f, 24f), "Permukaan: " + underPlayer, style);
        }
    }
}
#endif
