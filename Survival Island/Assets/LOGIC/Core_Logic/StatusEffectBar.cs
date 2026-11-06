using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SurvivalIsland
{
    /// <summary>
    /// Ikon efek status di HUD, bagian HUDManager tanpa UIPanel. Blueprint: Core_Logic/StatusEffectManager.md bagian
    /// "Tampilan" (#20). Satu ikon per efek (buff hijau, debuff merah), lingkaran sisa waktu diperbarui 4 kali per detik,
    /// ikon berkedip di bawah statusEffects.expiringWarningSeconds, efek Permanent tanpa lingkaran, angka tumpukan kalau > 1.
    /// Ikon dipakai ulang dari daftar tetap (tanpa pool dan tanpa Instantiate), jumlahnya disiapkan sebanyak maxActive.
    /// </summary>
    public class StatusEffectBar : MonoBehaviour
    {
        [Serializable]
        public class EffectIcon
        {
            public GameObject root;
            public Image icon;
            public Image background;      // Diwarnai buff atau debuff
            public Image timerFill;       // Image.Type Filled, Radial360
            public TMP_Text stackText;
        }

        [SerializeField] private EffectIcon[] icons = new EffectIcon[0];
        [SerializeField] private Color buffColor = new Color(0.3f, 0.8f, 0.3f);
        [SerializeField] private Color debuffColor = new Color(0.85f, 0.25f, 0.25f);
        [SerializeField] private float blinkHz = 3f;

        private const float TickInterval = 0.25f;   // Blueprint: 4 kali per detik
        private float tickTimer;
        private float blinkTime;
        private int shown;

        private void OnEnable()
        {
            Refresh();
        }

        /// <summary>Susun ulang ikon dari StatusEffectManager.Active. Dipanggil HUDManager saat StatusEffectChangedEvent.</summary>
        public void Refresh()
        {
            var mgr = StatusEffectManager.Instance;
            var list = mgr != null ? mgr.Active : null;
            shown = 0;
            for (int i = 0; i < icons.Length; i++)
            {
                var w = icons[i];
                if (w == null) continue;
                bool has = list != null && i < list.Count && list[i].data != null;
                if (w.root != null) w.root.SetActive(has);
                if (!has) continue;
                shown = i + 1;

                var e = list[i];
                if (w.icon != null) w.icon.sprite = e.data.icon;
                if (w.background != null) w.background.color = e.data.isBuff ? buffColor : debuffColor;
                if (w.stackText != null) w.stackText.text = e.stacks > 1 ? e.stacks.ToString() : string.Empty;
                if (w.timerFill != null) w.timerFill.enabled = e.data.durationUnit != EffectDurationUnit.Permanent;
            }
            if (list != null && list.Count > icons.Length)
                Debug.LogWarning($"[StatusEffectBar] Ikon kurang: {list.Count} efek, {icons.Length} ikon. Siapkan sebanyak statusEffects.maxActive.", this);
            UpdateTimers();
        }

        private void Update()
        {
            if (shown == 0) return;
            // Waktu berskala: lingkaran dan kedip berhenti saat jeda.
            float dt = Time.deltaTime;
            blinkTime += dt;
            tickTimer += dt;
            if (tickTimer < TickInterval) return;
            tickTimer = 0f;
            UpdateTimers();
        }

        private void UpdateTimers()
        {
            var mgr = StatusEffectManager.Instance;
            if (mgr == null) return;
            var list = mgr.Active;
            var bal = GameManager.CurrentBalance;
            float warn = bal != null ? bal.statusEffects.expiringWarningSeconds : 5f;
            bool blinkOn = Mathf.Repeat(blinkTime * blinkHz, 1f) < 0.5f;

            for (int i = 0; i < icons.Length && i < list.Count; i++)
            {
                var w = icons[i];
                var e = list[i];
                if (w == null || e.data == null) continue;
                if (e.data.durationUnit == EffectDurationUnit.Permanent)
                {
                    if (w.icon != null) w.icon.enabled = true;
                    continue;
                }
                float left = mgr.GetRemainingSeconds(e);
                float full = mgr.GetFullSeconds(e.data);
                if (w.timerFill != null) w.timerFill.fillAmount = full > 0f ? Mathf.Clamp01(left / full) : 0f;
                if (w.icon != null) w.icon.enabled = left > warn || blinkOn;
            }
        }
    }
}
