using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SurvivalIsland
{
    /// <summary>
    /// Ringkasan hasil Fase 1 (panel RunSummary, #83). Blueprint: 60-seconds/LifeboatExtraction.md bagian "Ringkasan hasil Fase 1".
    /// Pengaturan panel di Inspector: pausesTime, releasesCursor, blocksGameplayInput, isLocked = true; closableByEsc = false.
    /// Animasi memakai waktu tak terskala (FadingUIPanel).
    ///
    /// Belum diimplementasikan: nama dan skill NPC dari NPCData (#76), nama item lewat LocalizationManager (#104).
    /// Sementara menampilkan ID apa adanya.
    /// </summary>
    public class RunSummaryPanel : FadingUIPanel
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text itemsText;
        [SerializeField] private TMP_Text rescuedText;
        [SerializeField] private GameObject lostSection;
        [SerializeField] private TMP_Text lostText;
        [SerializeField] private TMP_Text conditionText;
        [SerializeField] private GameObject timeSection;
        [SerializeField] private TMP_Text timeText;
        [SerializeField] private Button continueButton;

        private readonly StringBuilder sb = new StringBuilder(256);

        private void Awake()
        {
            if (continueButton != null) continueButton.onClick.AddListener(HandleContinue);
        }

        protected override void Show()
        {
            Fill();
            base.Show();
        }

        private void Fill()
        {
            var flow = LifeboatExtraction.Instance;
            if (flow == null) return;
            var outcome = flow.LastOutcome;

            if (titleText != null) titleText.text = "Sekoci Diturunkan";   // TODO #104: kunci teks

            if (itemsText != null)
            {
                sb.Clear();
                var items = flow.DepositedItems;
                for (int i = 0; i < items.Count; i++) sb.Append(items[i].itemId).Append(" x").Append(items[i].amount).Append('\n');
                itemsText.text = sb.Length > 0 ? sb.ToString() : "-";
            }

            if (rescuedText != null)
            {
                sb.Clear();
                var rescued = flow.RescuedNpcIds;
                for (int i = 0; i < rescued.Count; i++) sb.Append(rescued[i]).Append('\n');
                rescuedText.text = sb.Length > 0 ? sb.ToString() : "-";
            }

            // Bagian Korban disembunyikan kalau tidak ada NPC yang tertinggal.
            bool hasLost = outcome != null && outcome.lostNpcIds.Count > 0;
            if (lostSection != null) lostSection.SetActive(hasLost);
            if (lostText != null && hasLost)
            {
                sb.Clear();
                for (int i = 0; i < outcome.lostNpcIds.Count; i++) sb.Append(outcome.lostNpcIds[i]).Append('\n');
                lostText.text = sb.ToString();
            }

            if (conditionText != null && outcome != null)
            {
                sb.Clear();
                sb.Append("HP ").Append(Mathf.CeilToInt(outcome.hpOnExit));
                if (outcome.infectionOnExit > 0f) sb.Append("  Infeksi ").Append(Mathf.CeilToInt(outcome.infectionOnExit));
                conditionText.text = sb.ToString();
            }

            // Sisa detik hanya kalau berangkat lebih awal.
            bool showTime = outcome != null && outcome.departedEarly;
            if (timeSection != null) timeSection.SetActive(showTime);
            if (timeText != null && showTime) timeText.text = Mathf.CeilToInt(outcome.secondsLeftOnExit).ToString();
        }

        private void HandleContinue()
        {
            if (LifeboatExtraction.Instance != null) LifeboatExtraction.Instance.ContinueToIsland();
        }
    }
}
