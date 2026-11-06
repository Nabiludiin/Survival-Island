using UnityEngine;
using UnityEngine.UI;

namespace SurvivalIsland
{
    /// <summary>
    /// Kedipan hijau lembut layar saat FoodPoisoning atau Venom. Image penuh layar di canvas HUD, tanpa UIPanel.
    /// Blueprint: raft/DiseaseManager.md bagian "Efek layar dan UI" (#45). Bisa dipindah ke PostProcessController (#23) nanti.
    /// Usulan disetujui #45: alpha sampai overlayMaxAlpha per overlayPulseSeconds, dengan pilihan tanpa kedip (reduceFlashing #104).
    /// Time.time berhenti saat dijeda, jadi kedipan juga berhenti.
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class DiseaseOverlay : MonoBehaviour
    {
        [SerializeField] private Image overlay;
        [SerializeField] private Color baseColor = new Color(0.25f, 0.85f, 0.2f, 1f);

        private void Awake()
        {
            if (overlay == null) overlay = GetComponent<Image>();
            overlay.raycastTarget = false;
            SetAlpha(0f);
        }

        private void Update()
        {
            var dm = DiseaseManager.Instance;
            bool poisoned = dm != null && dm.IsPoisoned;
            if (!poisoned) { SetAlpha(0f); return; }

            var b = GameManager.CurrentBalance;
            float maxAlpha = b != null ? b.disease.overlayMaxAlpha : 0.18f;
            float pulse = b != null ? Mathf.Max(0.01f, b.disease.overlayPulseSeconds) : 2.5f;

            // Aksesibilitas: warna tetap dengan alpha setengah, tanpa kedipan.
            bool reduceFlashing = LocalizationManager.Instance != null && LocalizationManager.Instance.Accessibility.reduceFlashing;
            if (reduceFlashing) { SetAlpha(maxAlpha * 0.5f); return; }

            float t = Mathf.PingPong(Time.time / pulse, 1f);
            SetAlpha(Mathf.Lerp(0f, maxAlpha, t));
        }

        private void SetAlpha(float a)
        {
            var c = baseColor;
            c.a = a;
            overlay.color = c;
            overlay.enabled = a > 0.0001f;
        }
    }
}
