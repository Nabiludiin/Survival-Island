using UnityEngine;
using UnityEngine.UI;

namespace SurvivalIsland
{
    /// <summary>
    /// Ikon mata di HUD yang menunjukkan seberapa dekat pemain terdeteksi zombie. Blueprint: left-4-dead/AISensory.md
    /// bagian "UI Detection Meter" (#49). GDD bagian 3: Mengendap, pemain bisa menghindar dari zombie.
    ///
    /// Hanya menampilkan data dari PlayerDetectionChangedEvent (sticky, pengirim AISensoryHub):
    /// - level01 di bawah 0,02: tersembunyi, memudar dalam stealth.detectionMeterFadeSeconds (1 detik).
    /// - 0,02 sampai 0,99: kelopak terbuka lewat Image.fillAmount, warna putih ke kuning.
    /// - isAware: terbuka penuh, merah, dan berdenyut sekali.
    /// Saat stealth.enabled = false, objek ini mematikan dirinya. Komponen di canvas HUD, bukan singleton.
    /// </summary>
    public class DetectionMeterUI : MonoBehaviour
    {
        private const float HiddenBelow = 0.02f;   // Batas tampil dari tabel blueprint

        [SerializeField] private CanvasGroup group;
        [Tooltip("Kelopak mata. Image Filled; fillAmount = level deteksi.")]
        [SerializeField] private Image eyelid;
        [Tooltip("Gambar yang diwarnai. Kosong = eyelid.")]
        [SerializeField] private Image tintTarget;
        [SerializeField] private RectTransform pulseTarget;

        [Header("Tampilan (bukan angka gameplay)")]
        [SerializeField] private Color calmColor = Color.white;
        [SerializeField] private Color suspiciousColor = Color.yellow;
        [SerializeField] private Color awareColor = Color.red;
        [SerializeField] private float pulseScale = 1.3f;
        [SerializeField] private float pulseSeconds = 0.25f;

        private float level;
        private bool aware;
        private bool wasAware;
        private float pulseTimer;

        private static StealthBalance B => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.stealth : null;

        private void OnEnable()
        {
            EventChannel<PlayerDetectionChangedEvent>.Raised += HandleDetection;
            if (EventChannel<PlayerDetectionChangedEvent>.HasLast) HandleDetection(EventChannel<PlayerDetectionChangedEvent>.Last);
            else { level = 0f; aware = false; }
            wasAware = aware;
            pulseTimer = 0f;
            if (group != null) group.alpha = level < HiddenBelow ? 0f : 1f;
            Apply();
        }

        private void OnDisable()
        {
            EventChannel<PlayerDetectionChangedEvent>.Raised -= HandleDetection;
        }

        private void HandleDetection(PlayerDetectionChangedEvent e)
        {
            level = Mathf.Clamp01(e.level01);
            aware = e.isAware;
            // Berdenyut sekali saat baru menjadi Aware.
            if (aware && !wasAware) pulseTimer = pulseSeconds;
            wasAware = aware;
            Apply();
        }

        private void Apply()
        {
            if (eyelid != null) eyelid.fillAmount = aware ? 1f : level;
            var img = tintTarget != null ? tintTarget : eyelid;
            if (img != null) img.color = aware ? awareColor : Color.Lerp(calmColor, suspiciousColor, level);
        }

        private void Update()
        {
            var b = B;
            if (b != null && !b.enabled) { gameObject.SetActive(false); return; }
            float dt = Time.unscaledDeltaTime;

            // Tampil langsung, hilang memudar.
            if (group != null)
            {
                bool show = aware || level >= HiddenBelow;
                float fade = b != null && b.detectionMeterFadeSeconds > 0f ? dt / b.detectionMeterFadeSeconds : 1f;
                group.alpha = show ? 1f : Mathf.MoveTowards(group.alpha, 0f, fade);
            }

            if (pulseTarget != null)
            {
                float s = 1f;
                if (pulseTimer > 0f)
                {
                    pulseTimer -= dt;
                    float t = pulseSeconds > 0f ? 1f - Mathf.Clamp01(pulseTimer / pulseSeconds) : 1f;
                    s = Mathf.Lerp(pulseScale, 1f, t);
                }
                pulseTarget.localScale = new Vector3(s, s, 1f);
            }
        }
    }
}
