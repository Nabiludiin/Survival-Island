using TMPro;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Layar gelap saat tidur: jam dan tulisan tombol bangun. Esc membangunkan pemain (OnClosed User = Manual).
    /// Blueprint: Island_Logic/SleepSystem.md bagian 1 (#67, #83). Panel tidak menjeda waktu (pausesTime = false), jadi jam
    /// dunia tetap dipercepat lewat DayNightDirector.
    ///
    /// Catatan implementasi (sistem pendukung): UIPanelId belum punya nilai Sleep (UIPanel.cs kode lama). Isi id di Inspector
    /// setelah nilai Sleep ditambahkan; sementara memakai nilai yang ada.
    /// </summary>
    public class SleepPanel : FadingUIPanel
    {
        [SerializeField] private TMP_Text clockText;
        [SerializeField] private TMP_Text hintText;

        protected override void Show()
        {
            base.Show();
            if (hintText != null) hintText.text = Text("ui.sleep.hint", "Esc: bangun");   // Kunci teks #104
            Refresh();
        }

        private void Update() => Refresh();

        private void Refresh()
        {
            if (clockText == null || DayNightDirector.Instance == null) return;
            float h = DayNightDirector.Instance.CurrentTimeOfDay;
            int hh = Mathf.FloorToInt(h) % 24;
            int mm = Mathf.FloorToInt((h - Mathf.Floor(h)) * 60f);
            clockText.text = hh.ToString("00") + ":" + mm.ToString("00");
        }

        public override void OnClosed(UICloseReason reason)
        {
            base.OnClosed(reason);
            if (SleepSystem.Instance != null) SleepSystem.Instance.HandlePanelClosed(reason);
        }

        /// <summary>Teks dari LocalizationManager (#104); teks bawaan kalau kunci belum ada di tabel.</summary>
        private static string Text(string key, string fallback)
        {
            var loc = LocalizationManager.Instance;
            return loc != null && loc.Has(key) ? loc.Get(key) : fallback;
        }
    }
}
