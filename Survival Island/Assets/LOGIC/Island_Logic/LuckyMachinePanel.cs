using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SurvivalIsland
{
    /// <summary>
    /// Panel Lucky Machine (UIPanelId.LuckyMachine). Blueprint: Island_Logic/LuckyMachine.md bagian 2 "Memutar" (#72)
    /// dan Core_Logic/UIFocusStack.md (#83). Menampilkan kuota, putaran tersimpan, sisa waktu gratis, dan tombol putar.
    /// Panel tidak menjeda game: pengaturan di Inspector releasesCursor, blocksGameplayInput, closableByEsc, tanpa pausesTime.
    /// </summary>
    public class LuckyMachinePanel : FadingUIPanel
    {
        public static LuckyMachinePanel Instance { get; private set; }

        [SerializeField] private TMP_Text quotaText;
        [SerializeField] private TMP_Text spinsText;
        [SerializeField] private TMP_Text freeWindowText;
        [SerializeField] private TMP_Text resultText;
        [SerializeField] private Button spinButton;
        [SerializeField] private Button closeButton;

        private LuckyMachine machine;
        private readonly StringBuilder sb = new StringBuilder(128);
        private bool listenersAdded;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("[LuckyMachinePanel] Instance ganda.");
                return;
            }
            Instance = this;
            AddListeners();
        }

        protected override void OnDestroy()
        {
            if (Instance == this) Instance = null;
            if (machine != null) machine.StateChanged -= HandleMachineChanged;
            base.OnDestroy();
        }

        private void AddListeners()
        {
            if (listenersAdded) return;
            listenersAdded = true;
            if (spinButton != null) spinButton.onClick.AddListener(HandleSpinClicked);
            if (closeButton != null) closeButton.onClick.AddListener(HandleCloseClicked);
        }

        /// <summary>Dipanggil LuckyMachine.Interact sebelum Push.</summary>
        public void Bind(LuckyMachine target)
        {
            if (machine != null) machine.StateChanged -= HandleMachineChanged;
            machine = target;
            if (machine != null) machine.StateChanged += HandleMachineChanged;
            if (resultText != null) resultText.text = string.Empty;
            Refresh();
        }

        public override void OnClosed(UICloseReason reason)
        {
            if (machine != null) machine.StateChanged -= HandleMachineChanged;
            machine = null;
        }

        private void HandleMachineChanged(LuckyMachine m)
        {
            Refresh();
            if (resultText != null && m != null && !m.IsSpinning && m.LastReward != null)
                resultText.text = m.LastReward.rewardId;   // TODO #104: nama hadiah lewat LocalizationManager
        }

        private void HandleSpinClicked()
        {
            if (machine == null) return;
            if (machine.TrySpin() && resultText != null) resultText.text = "...";
        }

        private void HandleCloseClicked()
        {
            if (UIFocusStack.Instance != null) UIFocusStack.Instance.Close(this, UICloseReason.User);
        }

        private void Update()
        {
            // Sisa waktu gratis berubah tiap frame. Waktu gameplay, karena panel tidak menjeda.
            if (machine != null && freeWindowText != null) RefreshFreeWindow();
        }

        private void Refresh()
        {
            if (machine == null) return;
            var data = machine.Data;
            if (quotaText != null)
            {
                sb.Clear();
                if (data != null && data.quotas != null)
                {
                    for (int i = 0; i < data.quotas.Length; i++)
                    {
                        var q = data.quotas[i];
                        if (q == null) continue;
                        sb.Append(q.enemyId).Append(": ").Append(machine.GetQuotaProgress(i)).Append(" / ").Append(q.required).Append('\n');
                    }
                }
                quotaText.text = sb.ToString();
            }
            if (spinsText != null) spinsText.text = "Putaran: " + machine.StoredSpins;
            if (spinButton != null) spinButton.interactable = machine.CanSpin;
            RefreshFreeWindow();
        }

        private void RefreshFreeWindow()
        {
            if (freeWindowText == null || machine == null) return;
            float left = machine.FreeWindowRemaining;
            freeWindowText.text = left > 0f ? "Gratis: " + Mathf.CeilToInt(left) + " detik" : string.Empty;
            if (spinButton != null) spinButton.interactable = machine.CanSpin;
        }
    }
}
