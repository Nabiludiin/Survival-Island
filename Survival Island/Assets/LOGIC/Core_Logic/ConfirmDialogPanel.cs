using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SurvivalIsland
{
    /// <summary>
    /// Isi dialog konfirmasi. Blueprint: Core_Logic/UIManager.md bagian "ConfirmDialog" (usulan, disetujui developer).
    /// onResult: true untuk Ya, false untuk Tidak atau Esc. Tidak dipanggil kalau dialog ditutup paksa.
    /// </summary>
    public struct ConfirmRequest
    {
        public string titleKey;          // Kunci #104
        public string bodyKey;           // Kunci #104
        public string yesKey, noKey;     // Kosong berarti ui.yes dan ui.no
        public Action<bool> onResult;
    }

    /// <summary>
    /// Panel ConfirmDialog bersama. Dibuka lewat UIManager.ShowConfirm, bukan langsung.
    /// Blueprint: Core_Logic/UIManager.md bagian "ConfirmDialog".
    /// </summary>
    public class ConfirmDialogPanel : FadingUIPanel
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text bodyText;
        [SerializeField] private Button yesButton;
        [SerializeField] private Button noButton;
        [SerializeField] private TMP_Text yesLabel;
        [SerializeField] private TMP_Text noLabel;

        private Coroutine delayRoutine;
        private bool listenersAdded;
        private string titleKey, bodyKey, yesKey, noKey;

        private void Awake()
        {
            AddButtonListeners();
        }

        private void AddButtonListeners()
        {
            if (listenersAdded) return;
            listenersAdded = true;
            if (yesButton != null) yesButton.onClick.AddListener(HandleYesClicked);
            if (noButton != null) noButton.onClick.AddListener(HandleNoClicked);
        }

        /// <summary>Isi teks dari request dan matikan tombol sampai jeda input selesai. Dipanggil UIManager.ShowConfirm.</summary>
        public void SetContent(in ConfirmRequest request)
        {
            titleKey = request.titleKey;
            bodyKey = request.bodyKey;
            yesKey = string.IsNullOrEmpty(request.yesKey) ? "ui.yes" : request.yesKey;
            noKey = string.IsNullOrEmpty(request.noKey) ? "ui.no" : request.noKey;
            RefreshTexts();
            SetButtonsInteractable(false);
        }

        /// <summary>Isi ulang teks dari kunci yang tersimpan (LocaleChangedEvent).</summary>
        public void RefreshTexts()
        {
            if (titleText != null) titleText.text = UIManager.Localize(titleKey);
            if (bodyText != null) bodyText.text = UIManager.Localize(bodyKey);
            if (yesLabel != null) yesLabel.text = UIManager.Localize(yesKey);
            if (noLabel != null) noLabel.text = UIManager.Localize(noKey);
        }

        public void ClearContent()
        {
            titleKey = bodyKey = yesKey = noKey = null;
            if (titleText != null) titleText.text = string.Empty;
            if (bodyText != null) bodyText.text = string.Empty;
        }

        protected override void Show()
        {
            AddButtonListeners();
            base.Show();
            SetButtonsInteractable(false);
        }

        protected override void Hide()
        {
            StopDelay();
            SetButtonsInteractable(false);
            base.Hide();
        }

        public override void OnFocusGained()
        {
            StopDelay();
            if (isActiveAndEnabled) delayRoutine = StartCoroutine(EnableButtonsAfterDelay());
        }

        public override void OnClosed(UICloseReason reason)
        {
            // Esc (User) berarti Tidak. Forced, RunEnded, dan SceneUnloaded tidak memanggil onResult.
            if (UIManager.Instance != null) UIManager.Instance.HandleConfirmClosed(reason);
        }

        private IEnumerator EnableButtonsAfterDelay()
        {
            // Jeda supaya klik ganda tidak langsung menyetujui.
            var balance = GameManager.CurrentBalance;
            float delay = balance != null ? balance.ui.confirmInputDelaySeconds : 0f;
            if (delay > 0f) yield return new WaitForSecondsRealtime(delay);

            SetButtonsInteractable(true);
            // Fokus awal di tombol Tidak, supaya Enter tidak menyetujui tanpa sengaja.
            if (noButton != null && EventSystem.current != null) EventSystem.current.SetSelectedGameObject(noButton.gameObject);
            delayRoutine = null;
        }

        private void StopDelay()
        {
            if (delayRoutine != null)
            {
                StopCoroutine(delayRoutine);
                delayRoutine = null;
            }
        }

        private void SetButtonsInteractable(bool value)
        {
            if (yesButton != null) yesButton.interactable = value;
            if (noButton != null) noButton.interactable = value;
        }

        private void HandleYesClicked()
        {
            SetButtonsInteractable(false);
            if (UIManager.Instance != null) UIManager.Instance.ResolveConfirm(true);
        }

        private void HandleNoClicked()
        {
            SetButtonsInteractable(false);
            if (UIManager.Instance != null) UIManager.Instance.ResolveConfirm(false);
        }
    }
}
