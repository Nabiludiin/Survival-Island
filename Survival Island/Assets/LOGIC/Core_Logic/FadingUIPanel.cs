using System.Collections;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Perilaku tampil dan sembunyi bersama untuk panel UIManager.
    /// Blueprint: Core_Logic/UIManager.md bagian 2 "Perilaku tampil dan sembunyi bersama" (#83).
    ///
    /// - Show: SetActive(true), blocksRaycasts = true, lalu fade alpha 0 ke 1 dengan Time.unscaledDeltaTime.
    /// - Hide: interactable dan blocksRaycasts langsung false, lalu fade ke 0, lalu SetActive(false).
    /// - Show di tengah fade keluar membalik fade dari nilai sekarang.
    /// Panel ini juga dipakai langsung (tanpa turunan) untuk panel sederhana seperti MainMenu.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class FadingUIPanel : UIPanel
    {
        private CanvasGroup group;
        private Coroutine fadeRoutine;
        private bool isShown;
        private bool isDestroying;

        protected CanvasGroup Group
        {
            get
            {
                if (group == null) group = GetComponent<CanvasGroup>();
                return group;
            }
        }

        public bool IsShown => isShown;

        /// <summary>false untuk panel yang langsung tampil tanpa fade masuk (Loading, Cinematic).</summary>
        protected virtual bool FadeInOnShow => true;

        /// <summary>false untuk panel yang langsung hilang tanpa fade keluar (Cinematic).</summary>
        protected virtual bool FadeOutOnHide => true;

        /// <summary>true untuk panel yang mengatur interactable sendiri karena punya jeda input (layar akhir, ConfirmDialog).</summary>
        protected virtual bool ManagesOwnInteractable => false;

        protected override void Show()
        {
            isShown = true;
            gameObject.SetActive(true);
            var g = Group;
            g.blocksRaycasts = true;
            if (!ManagesOwnInteractable) g.interactable = true;
            StartFade(1f, FadeInOnShow ? FadeSeconds() : 0f);
        }

        protected override void Hide()
        {
            isShown = false;
            var g = Group;
            // Langsung, supaya klik tidak tembus ke panel yang sudah ditutup.
            g.interactable = false;
            g.blocksRaycasts = false;
            StartFade(0f, FadeOutOnHide ? FadeSeconds() : 0f);
        }

        /// <summary>Kondisi awal tersembunyi tanpa fade. Dipakai UIManager.Initialize.</summary>
        public void ResetHidden()
        {
            StopFade();
            isShown = false;
            var g = Group;
            g.alpha = 0f;
            g.interactable = false;
            g.blocksRaycasts = false;
            gameObject.SetActive(false);
        }

        protected virtual void OnEnable()
        {
            // Coroutine fade berhenti saat objek dimatikan (mis. managersRoot.SetActive(false) saat reset #82).
            // Saat aktif lagi, alpha langsung disesuaikan dengan status tampil.
            if (isShown) Group.alpha = 1f;
        }

        protected virtual void OnDisable()
        {
            fadeRoutine = null;
        }

        protected override void OnDestroy()
        {
            isDestroying = true;
            base.OnDestroy();
        }

        private void StartFade(float target, float duration)
        {
            StopFade();
            if (isDestroying) return;

            if (duration <= 0f || !isActiveAndEnabled)
            {
                Group.alpha = target;
                if (target <= 0f) gameObject.SetActive(false);
                return;
            }
            fadeRoutine = StartCoroutine(FadeTo(target, duration));
        }

        private void StopFade()
        {
            if (fadeRoutine != null)
            {
                StopCoroutine(fadeRoutine);
                fadeRoutine = null;
            }
        }

        private IEnumerator FadeTo(float target, float duration)
        {
            var g = Group;
            float speed = 1f / duration;
            while (!Mathf.Approximately(g.alpha, target))
            {
                // Waktu tak terskala: panel tetap beranimasi saat Time.timeScale = 0 (UIFocusStack.md).
                g.alpha = Mathf.MoveTowards(g.alpha, target, speed * Time.unscaledDeltaTime);
                yield return null;
            }
            g.alpha = target;
            fadeRoutine = null;
            if (target <= 0f) gameObject.SetActive(false);
        }

        protected static float FadeSeconds()
        {
            var balance = GameManager.CurrentBalance;
            return balance != null ? balance.ui.panelFadeSeconds : 0f;
        }
    }
}
