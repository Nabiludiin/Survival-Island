using TMPro;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Penampil subtitle persisten pada Canvas di bawah PersistentRoot.
    /// Blueprint: Core_Logic/LocalizationManager.md bagian "Subtitle" (#104).
    ///
    /// Baris dibuat sekali sebanyak GameBalanceConfig.localization.subtitleMaxRows dari rowTemplate, lalu dipakai ulang.
    /// Timer memakai Time.deltaTime, jadi subtitle membeku saat game dijeda.
    /// </summary>
    public class SubtitlePresenter : MonoBehaviour
    {
        [Tooltip("Satu baris contoh (TMP_Text, nonaktif). Disalin saat pertama dipakai sampai subtitleMaxRows.")]
        [SerializeField] private TMP_Text rowTemplate;

        private struct Row
        {
            public TMP_Text label;
            public float remaining;
            public int priority;
            public bool active;
            public int order;     // Urutan masuk, untuk memilih baris yang diganti
        }

        // Nilai awal config, hanya dipakai kalau GameManager belum siap (mis. scene uji).
        private static readonly LocalizationBalance DefaultBalance = new LocalizationBalance();

        private Row[] rows;
        private float baseFontSize;
        private int orderCounter;

        private void OnEnable()
        {
            // ClearAll (#82) mematikan dan menyalakan managersRoot, jadi langganan dipasang ulang di sini.
            EventChannel<SubtitleRequestedEvent>.Raised += HandleRequest;
            EventChannel<RunEndedEvent>.Raised += HandleRunEnded;
            EventChannel<AccessibilityChangedEvent>.Raised += HandleAccessibility;
        }

        private void OnDisable()
        {
            EventChannel<SubtitleRequestedEvent>.Raised -= HandleRequest;
            EventChannel<RunEndedEvent>.Raised -= HandleRunEnded;
            EventChannel<AccessibilityChangedEvent>.Raised -= HandleAccessibility;
        }

        private bool EnsureRows()
        {
            if (rows != null) return true;
            if (rowTemplate == null)
            {
                Debug.LogWarning("[SubtitlePresenter] rowTemplate kosong.");
                return false;
            }
            var b = GameManager.CurrentBalance;
            int count = Mathf.Max(1, (b != null ? b.localization : DefaultBalance).subtitleMaxRows);
            baseFontSize = rowTemplate.fontSize;
            rows = new Row[count];
            rows[0].label = rowTemplate;
            // Dibuat sekali saja, bukan per subtitle (#104: tidak memakai Instantiate berulang).
            for (int i = 1; i < count; i++)
                rows[i].label = Instantiate(rowTemplate, rowTemplate.transform.parent);
            for (int i = 0; i < count; i++)
            {
                // Satu entri dibatasi dua baris teks (#104, "Teks panjang pada skala 1,5").
                rows[i].label.maxVisibleLines = 2;
                rows[i].label.overflowMode = TextOverflowModes.Ellipsis;
                rows[i].label.gameObject.SetActive(false);
            }
            ApplyScale();
            return true;
        }

        private void HandleRequest(SubtitleRequestedEvent e)
        {
            var loc = LocalizationManager.Instance;
            if (loc == null || string.IsNullOrEmpty(e.textKey) || !EnsureRows()) return;

            var a11y = loc.Accessibility;
            // 1. Keputusan developer (#104 poin 5): peringatan bencana selalu tampil walau subtitle dimatikan,
            //    karena menyangkut keselamatan pemain (GDD bagian 6).
            if (!a11y.subtitlesEnabled && e.sourceKind != SubtitleSourceKind.Warning) return;

            string body = loc.Get(e.textKey);
            var b = GameManager.CurrentBalance;
            var lb = b != null ? b.localization : DefaultBalance;
            // 2. Lama tampil: Max(subtitleMinSeconds, jumlah huruf * subtitleSecondsPerChar).
            float seconds = Mathf.Max(lb.subtitleMinSeconds, body.Length * lb.subtitleSecondsPerChar);

            // 3. Baris kosong, atau baris prioritas terendah (yang paling lama kalau seri). Permintaan yang lebih
            //    rendah dari semua baris dibuang.
            int slot = -1;
            for (int i = 0; i < rows.Length; i++)
                if (!rows[i].active) { slot = i; break; }
            if (slot < 0)
            {
                int lowest = -1;
                for (int i = 0; i < rows.Length; i++)
                {
                    if (lowest < 0 || rows[i].priority < rows[lowest].priority
                        || (rows[i].priority == rows[lowest].priority && rows[i].order < rows[lowest].order))
                        lowest = i;
                }
                if (e.priority < rows[lowest].priority) return;
                slot = lowest;
            }

            // 4. [Nama] teks kalau showSpeakerName aktif dan speakerKey tidak kosong.
            string text = a11y.showSpeakerName && !string.IsNullOrEmpty(e.speakerKey)
                ? "[" + loc.Get(e.speakerKey) + "] " + body
                : body;

            ref var row = ref rows[slot];
            row.label.text = text;
            row.label.transform.SetAsLastSibling();
            row.label.gameObject.SetActive(true);
            row.remaining = seconds;
            row.priority = e.priority;
            row.active = true;
            row.order = ++orderCounter;
        }

        private void Update()
        {
            if (rows == null) return;
            float dt = Time.deltaTime;   // 5. Berhenti saat Time.timeScale = 0
            for (int i = 0; i < rows.Length; i++)
            {
                if (!rows[i].active) continue;
                rows[i].remaining -= dt;
                if (rows[i].remaining <= 0f) Hide(i);
            }
        }

        private void Hide(int i)
        {
            rows[i].active = false;
            rows[i].label.gameObject.SetActive(false);
        }

        /// <summary>Hapus semua subtitle. Dipanggil saat RunEndedEvent.</summary>
        public void ClearAll()
        {
            if (rows == null) return;
            for (int i = 0; i < rows.Length; i++) Hide(i);
        }

        private void HandleRunEnded(RunEndedEvent e) => ClearAll();

        private void HandleAccessibility(AccessibilityChangedEvent e) => ApplyScale();

        private void ApplyScale()
        {
            if (rows == null) return;
            float scale = LocalizationManager.Instance != null ? LocalizationManager.Instance.Accessibility.textScale : 1f;
            for (int i = 0; i < rows.Length; i++) rows[i].label.fontSize = baseFontSize * scale;
        }
    }
}
