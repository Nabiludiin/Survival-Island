using TMPro;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Pengisi teks pada TMP_Text dari kunci lokalisasi, ditambah ukuran font dari Accessibility.textScale.
    /// Blueprint: Core_Logic/LocalizationManager.md bagian "LocalizedText" (#104).
    ///
    /// Membaca teks di OnEnable, jadi objek pool dan panel yang dinyalakan ulang selalu memakai bahasa terbaru.
    /// </summary>
    [RequireComponent(typeof(TMP_Text))]
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField] private string key;
        [Tooltip("Matikan untuk teks yang ukurannya diatur sistem lain.")]
        [SerializeField] private bool applyTextScale = true;

        private TMP_Text label;
        private float baseFontSize, baseFontSizeMax;
        private bool captured;

        public string Key => key;

        private void Awake()
        {
            CaptureBase();
        }

        private void OnEnable()
        {
            EventChannel<LocaleChangedEvent>.Raised += HandleLocale;
            EventChannel<InputBindingsChangedEvent>.Raised += HandleBindings;
            EventChannel<AccessibilityChangedEvent>.Raised += HandleAccessibility;
            // GameEventRegistry.ClearAll (#82) melepas langganan objek di luar managersRoot; RunStartedEvent menyegarkan teks.
            EventChannel<RunStartedEvent>.Raised += HandleRunStarted;
            Refresh();
        }

        private void OnDisable()
        {
            EventChannel<LocaleChangedEvent>.Raised -= HandleLocale;
            EventChannel<InputBindingsChangedEvent>.Raised -= HandleBindings;
            EventChannel<AccessibilityChangedEvent>.Raised -= HandleAccessibility;
            EventChannel<RunStartedEvent>.Raised -= HandleRunStarted;
        }

        /// <summary>Ganti kunci lewat kode, lalu isi ulang.</summary>
        public void SetKey(string newKey)
        {
            key = newKey;
            Refresh();
        }

        public void Refresh()
        {
            CaptureBase();
            var loc = LocalizationManager.Instance;
            if (loc == null || label == null) return;
            if (!string.IsNullOrEmpty(key)) label.text = loc.Get(key);
            ApplyScale(label, loc.Accessibility.textScale);
        }

        private void CaptureBase()
        {
            if (captured) return;
            label = GetComponent<TMP_Text>();
            if (label == null) return;
            baseFontSize = label.fontSize;
            baseFontSizeMax = label.fontSizeMax;
            captured = true;
        }

        private void ApplyScale(TMP_Text t, float scale)
        {
            if (!applyTextScale) return;
            // HUD memakai Auto Size dengan batas tetap, supaya skala 1,5 tidak keluar kotak (#104, "Layout").
            if (t.enableAutoSizing) t.fontSizeMax = baseFontSizeMax * scale;
            else t.fontSize = baseFontSize * scale;
        }

        private void HandleLocale(LocaleChangedEvent e) => Refresh();
        private void HandleBindings(InputBindingsChangedEvent e) => Refresh();
        private void HandleAccessibility(AccessibilityChangedEvent e) => Refresh();
        private void HandleRunStarted(RunStartedEvent e) => Refresh();
    }
}
