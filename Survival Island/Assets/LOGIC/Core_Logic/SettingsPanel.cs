using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SurvivalIsland
{
    /// <summary>
    /// Panel UIPanelId.Settings, dibuka dari MainMenu dan Pause. Blueprint: Core_Logic/SettingsManager.md bagian
    /// "SettingsPanel" (#19), ditambah tab Bahasa dan Aksesibilitas dari LocalizationManager.md (#104).
    ///
    /// Panel hanya memanggil fungsi SettingsManager, LocalizationManager, dan InputManager. Panel tidak menulis
    /// QualitySettings atau PlayerPrefs sendiri. Rujukan UI boleh kosong; kontrol yang kosong dilewati.
    /// </summary>
    public class SettingsPanel : FadingUIPanel
    {
        /// <summary>Satu baris daftar tombol pada tab Kontrol.</summary>
        [Serializable]
        public struct BindingRow
        {
            public GameInputAction action;
            public int bindingIndex;
            public Button button;
            public TMP_Text keyLabel;
        }

        [Header("Tab (Kontrol, Audio, Grafis, Tampilan, Lainnya, Aksesibilitas)")]
        [SerializeField] private Button[] tabButtons = new Button[0];
        [SerializeField] private GameObject[] tabPages = new GameObject[0];

        [Header("Kontrol")]
        [SerializeField] private Slider sensitivitySlider;
        [SerializeField] private Toggle invertYToggle;
        [SerializeField] private BindingRow[] bindingRows = new BindingRow[0];
        [SerializeField] private Button resetBindingsButton;
        [SerializeField] private TMP_Text rebindStatusText;

        [Header("Audio")]
        [SerializeField] private Slider masterSlider;
        [SerializeField] private Slider bgmSlider;
        [SerializeField] private Slider sfxSlider;

        [Header("Grafis")]
        [SerializeField] private TMP_Dropdown presetDropdown;     // Urutan QualityPreset
        [SerializeField] private TMP_Dropdown shadowDropdown;     // Urutan ShadowQuality
        [SerializeField] private TMP_Dropdown textureDropdown;    // Urutan TextureQuality
        [SerializeField] private Slider brightnessSlider;
        [SerializeField] private Button applyGraphicsButton;

        [Header("Tampilan")]
        [SerializeField] private Slider fovSlider;
        [SerializeField] private Toggle headBobToggle;
        [SerializeField] private Toggle weaponSwayToggle;
        [SerializeField] private Toggle fovKickToggle;
        [SerializeField] private Toggle chromaticToggle;          // Di samping headBob (#23)

        [Header("Lainnya")]
        [SerializeField] private Toggle tutorialToggle;
        [SerializeField] private Button replayTutorialButton;
        [SerializeField] private Button resetDefaultsButton;

        [Header("Bahasa dan aksesibilitas (#104)")]
        [SerializeField] private TMP_Dropdown localeDropdown;     // Urutan LocalizationManager tables
        [SerializeField] private Slider textScaleSlider;
        [SerializeField] private Slider cameraShakeSlider;
        [SerializeField] private Toggle reduceFlashingToggle;
        [SerializeField] private Toggle subtitlesToggle;
        [SerializeField] private Toggle speakerNameToggle;
        [SerializeField] private Toggle holdToStruggleToggle;     // Pilihan aksesibilitas #110

        private bool refreshing;
        private bool wired;

        protected override void OnEnable()
        {
            base.OnEnable();
            EventChannel<InputBindingsChangedEvent>.Raised += HandleBindingsChanged;
            EventChannel<LocaleChangedEvent>.Raised += HandleLocaleChanged;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            EventChannel<InputBindingsChangedEvent>.Raised -= HandleBindingsChanged;
            EventChannel<LocaleChangedEvent>.Raised -= HandleLocaleChanged;
        }

        protected override void Show()
        {
            // Kabel dipasang saat pertama dibuka, karena batas slider dibaca dari GameManager.Balance.
            Wire();
            base.Show();
            RefreshAll();
            SelectTab(0);
        }

        // ---------------------------------------------------------------- Tutup

        public override void OnClosed(UICloseReason reason)
        {
            var s = SettingsManager.Instance;
            if (s == null || !s.IsDirty) return;

            if (reason == UICloseReason.User)
            {
                var request = new ConfirmRequest
                {
                    titleKey = "ui.settings.save_title",
                    bodyKey = "ui.settings.save_body",
                    onResult = HandleSaveConfirm
                };
                // Dialog ditolak (dialog lain terbuka): simpan, supaya pilihan pemain tidak hilang.
                if (UIManager.Instance == null || !UIManager.Instance.ShowConfirm(request)) s.Commit();
            }
            else
            {
                // Forced, RunEnded, SceneUnloaded: Commit tanpa dialog.
                s.Commit();
            }
        }

        private static void HandleSaveConfirm(bool save)
        {
            var s = SettingsManager.Instance;
            if (s == null) return;
            if (save) s.Commit();
            else s.Revert();
        }

        // ---------------------------------------------------------------- Kabel UI

        private void Wire()
        {
            if (wired) return;
            wired = true;

            for (int i = 0; i < tabButtons.Length; i++)
            {
                int index = i;
                if (tabButtons[i] != null) tabButtons[i].onClick.AddListener(() => SelectTab(index));
            }

            var limits = GameManager.CurrentBalance != null ? GameManager.CurrentBalance.settings : null;
            if (limits != null)
            {
                SetRange(sensitivitySlider, limits.sensitivityMin, limits.sensitivityMax);
                SetRange(fovSlider, limits.fovMin, limits.fovMax);
                SetRange(brightnessSlider, limits.brightnessMin, limits.brightnessMax);
            }
            var loc = GameManager.CurrentBalance != null ? GameManager.CurrentBalance.localization : null;
            if (loc != null) SetRange(textScaleSlider, loc.textScaleMin, loc.textScaleMax);
            SetRange(cameraShakeSlider, 0f, 1f);
            SetRange(masterSlider, 0f, 1f);
            SetRange(bgmSlider, 0f, 1f);
            SetRange(sfxSlider, 0f, 1f);

            AddSlider(sensitivitySlider, v => Settings?.SetMouseSensitivity(v));
            AddToggle(invertYToggle, v => Settings?.SetInvertY(v));
            AddSlider(masterSlider, v => Settings?.SetVolume(AudioChannel.Master, v));
            AddSlider(bgmSlider, v => Settings?.SetVolume(AudioChannel.BGM, v));
            AddSlider(sfxSlider, v => Settings?.SetVolume(AudioChannel.SFX, v));
            AddPointerUp(sfxSlider, () => Settings?.PlaySfxPreview());

            AddDropdown(presetDropdown, v => { Settings?.SetQualityPreset((QualityPreset)v); RefreshGraphics(); });
            AddDropdown(shadowDropdown, v => { Settings?.SetShadowQuality((ShadowQuality)v); RefreshGraphics(); });
            AddDropdown(textureDropdown, v => { Settings?.SetTextureQuality((TextureQuality)v); RefreshGraphics(); });
            AddSlider(brightnessSlider, v => Settings?.SetBrightness(v));
            if (applyGraphicsButton != null) applyGraphicsButton.onClick.AddListener(() => { Settings?.ApplyGraphics(); RefreshGraphics(); });

            AddSlider(fovSlider, v => Settings?.SetFieldOfView(v));
            AddToggle(headBobToggle, v => Settings?.SetHeadBob(v));
            AddToggle(weaponSwayToggle, v => Settings?.SetWeaponSway(v));
            AddToggle(fovKickToggle, v => Settings?.SetFovKick(v));
            AddToggle(chromaticToggle, v => Settings?.SetChromaticAberration(v));

            AddToggle(tutorialToggle, v => Settings?.SetTutorialEnabled(v));
            // TODO(TutorialManager): replayTutorialButton memanggil TutorialManager.Instance.ResetSeen() (#16).
            if (replayTutorialButton != null) replayTutorialButton.interactable = false;
            if (resetDefaultsButton != null) resetDefaultsButton.onClick.AddListener(() => { Settings?.ResetToDefaults(); RefreshAll(); });

            for (int i = 0; i < bindingRows.Length; i++)
            {
                int index = i;
                if (bindingRows[i].button != null) bindingRows[i].button.onClick.AddListener(() => StartRebind(index));
            }
            if (resetBindingsButton != null) resetBindingsButton.onClick.AddListener(() => InputManager.Instance?.ResetBindings());

            AddDropdown(localeDropdown, v =>
            {
                var l = LocalizationManager.Instance;
                if (l != null) l.SetLocale(l.GetLocaleId(v));
            });
            AddSlider(textScaleSlider, v => EditAccessibility((ref AccessibilityOptions o) => o.textScale = v));
            AddSlider(cameraShakeSlider, v => EditAccessibility((ref AccessibilityOptions o) => o.cameraShakeScale = v));
            AddToggle(reduceFlashingToggle, v => EditAccessibility((ref AccessibilityOptions o) => o.reduceFlashing = v));
            AddToggle(subtitlesToggle, v => EditAccessibility((ref AccessibilityOptions o) => o.subtitlesEnabled = v));
            AddToggle(speakerNameToggle, v => EditAccessibility((ref AccessibilityOptions o) => o.showSpeakerName = v));
            AddToggle(holdToStruggleToggle, v => Settings?.SetHoldToStruggle(v));
        }

        private static SettingsManager Settings => SettingsManager.Instance;

        private delegate void AccessibilityEdit(ref AccessibilityOptions o);

        private void EditAccessibility(AccessibilityEdit edit)
        {
            var l = LocalizationManager.Instance;
            if (l == null) return;
            var o = l.Accessibility;
            edit(ref o);
            // Aksesibilitas disimpan LocalizationManager langsung ke PlayerPrefs (#104), tidak lewat Commit.
            l.SetAccessibility(o);
        }

        private void AddSlider(Slider s, Action<float> onChange)
        {
            if (s != null) s.onValueChanged.AddListener(v => { if (!refreshing) onChange(v); });
        }

        private void AddToggle(Toggle t, Action<bool> onChange)
        {
            if (t != null) t.onValueChanged.AddListener(v => { if (!refreshing) onChange(v); });
        }

        private void AddDropdown(TMP_Dropdown d, Action<int> onChange)
        {
            if (d != null) d.onValueChanged.AddListener(v => { if (!refreshing) onChange(v); });
        }

        private static void AddPointerUp(Slider s, Action onUp)
        {
            if (s == null) return;
            var trigger = s.GetComponent<EventTrigger>();
            if (trigger == null) trigger = s.gameObject.AddComponent<EventTrigger>();
            var entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
            entry.callback.AddListener(_ => onUp());
            trigger.triggers.Add(entry);
        }

        private static void SetRange(Slider s, float min, float max)
        {
            if (s == null) return;
            s.minValue = min;
            s.maxValue = max;
        }

        // ---------------------------------------------------------------- Isi ulang tampilan

        private void SelectTab(int index)
        {
            for (int i = 0; i < tabPages.Length; i++)
                if (tabPages[i] != null) tabPages[i].SetActive(i == index);
        }

        private void RefreshAll()
        {
            var s = Settings;
            refreshing = true;
            if (s != null)
            {
                SetSlider(sensitivitySlider, s.mouseSensitivity);
                SetToggle(invertYToggle, s.invertY);
                SetSlider(masterSlider, s.masterVolume);
                SetSlider(bgmSlider, s.bgmVolume);
                SetSlider(sfxSlider, s.sfxVolume);
                SetSlider(brightnessSlider, s.brightness);
                SetSlider(fovSlider, s.fieldOfView);
                SetToggle(headBobToggle, s.headBobEnabled);
                SetToggle(weaponSwayToggle, s.weaponSwayEnabled);
                SetToggle(fovKickToggle, s.fovKickEnabled);
                SetToggle(chromaticToggle, s.chromaticAberrationEnabled);
                SetToggle(tutorialToggle, s.tutorialEnabled);
                SetToggle(holdToStruggleToggle, s.holdToStruggle);
            }
            var l = LocalizationManager.Instance;
            if (l != null)
            {
                var a = l.Accessibility;
                SetSlider(textScaleSlider, a.textScale);
                SetSlider(cameraShakeSlider, a.cameraShakeScale);
                SetToggle(reduceFlashingToggle, a.reduceFlashing);
                SetToggle(subtitlesToggle, a.subtitlesEnabled);
                SetToggle(speakerNameToggle, a.showSpeakerName);
                RefreshLocaleDropdown(l);
            }
            refreshing = false;
            RefreshGraphics();
            RefreshBindings();
        }

        private void RefreshGraphics()
        {
            var s = Settings;
            if (s == null) return;
            refreshing = true;
            if (presetDropdown != null) presetDropdown.SetValueWithoutNotify((int)s.qualityPreset);
            if (shadowDropdown != null) shadowDropdown.SetValueWithoutNotify((int)s.shadowQuality);
            if (textureDropdown != null) textureDropdown.SetValueWithoutNotify((int)s.textureQuality);
            refreshing = false;
            if (applyGraphicsButton != null) applyGraphicsButton.interactable = s.HasPendingGraphics;
        }

        private void RefreshLocaleDropdown(LocalizationManager l)
        {
            if (localeDropdown == null) return;
            var options = new List<string>(l.LocaleCount);
            int selected = 0;
            for (int i = 0; i < l.LocaleCount; i++)
            {
                string id = l.GetLocaleId(i);
                options.Add(l.Get("ui.language." + id));
                if (id == l.CurrentLocale) selected = i;
            }
            localeDropdown.ClearOptions();
            localeDropdown.AddOptions(options);
            localeDropdown.SetValueWithoutNotify(selected);
        }

        private void RefreshBindings()
        {
            var input = InputManager.Instance;
            if (input == null) return;
            for (int i = 0; i < bindingRows.Length; i++)
                if (bindingRows[i].keyLabel != null) bindingRows[i].keyLabel.text = input.GetBindingDisplay(bindingRows[i].action);
        }

        private static void SetSlider(Slider s, float v) { if (s != null) s.SetValueWithoutNotify(v); }
        private static void SetToggle(Toggle t, bool v) { if (t != null) t.SetIsOnWithoutNotify(v); }

        // ---------------------------------------------------------------- Ganti tombol

        private void StartRebind(int index)
        {
            var input = InputManager.Instance;
            if (input == null || index < 0 || index >= bindingRows.Length) return;
            var row = bindingRows[index];
            if (rebindStatusText != null) rebindStatusText.text = LocalizationManager.Instance != null ? LocalizationManager.Instance.Get("ui.settings.rebind_wait") : string.Empty;
            input.StartRebind(row.action, row.bindingIndex, HandleRebindDone);
        }

        private void HandleRebindDone(RebindResult result)
        {
            if (rebindStatusText != null)
            {
                var l = LocalizationManager.Instance;
                rebindStatusText.text = l == null || result.status == RebindStatus.Done ? string.Empty
                    : result.status == RebindStatus.Conflict
                        ? l.Format("ui.settings.rebind_conflict", "action", result.conflictingAction.ToString())
                        : l.Get("ui.settings.rebind_" + result.status.ToString().ToLowerInvariant());
            }
            RefreshBindings();
        }

        private void HandleBindingsChanged(InputBindingsChangedEvent e) => RefreshBindings();

        private void HandleLocaleChanged(LocaleChangedEvent e)
        {
            if (!IsShown) return;
            var l = LocalizationManager.Instance;
            if (l == null) return;
            refreshing = true;
            RefreshLocaleDropdown(l);
            refreshing = false;
            RefreshBindings();
        }
    }
}
