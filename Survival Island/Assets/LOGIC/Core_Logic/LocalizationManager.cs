using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Pilihan aksesibilitas (#104). Disimpan di PlayerPrefs dengan kunci a11y.*.
    /// SettingsManager.headBobEnabled tetap milik #19.
    /// </summary>
    [Serializable]
    public struct AccessibilityOptions
    {
        public float textScale;          // Nilai awal 1
        public float cameraShakeScale;   // Nilai awal 1. 0 berarti mati
        public bool reduceFlashing;      // Nilai awal false
        public bool subtitlesEnabled;    // Nilai awal true
        public bool showSpeakerName;     // Nilai awal true

        public static AccessibilityOptions Default => new AccessibilityOptions
        {
            textScale = 1f, cameraShakeScale = 1f, reduceFlashing = false, subtitlesEnabled = true, showSpeakerName = true
        };
    }

    /// <summary>
    /// Sumber semua teks pemain lewat kunci, format angka dan jamak, serta pilihan aksesibilitas.
    /// Blueprint: Core_Logic/LocalizationManager.md (#104). Usulan teknis, belum ada di GDD; mendukung Diary
    /// (GDD bagian 4) dan peringatan bencana (GDD bagian 6).
    ///
    /// Urutan bootstrap ke-7 (RunLifecycle.md), sebelum UI tampil. Bahasa dan aksesibilitas ada di PlayerPrefs,
    /// jadi permadeath tidak mengubahnya.
    /// </summary>
    public class LocalizationManager : MonoBehaviour, IBootstrapService
    {
        public const string LocalePrefsKey = "settings.locale";
        public const string A11yTextScaleKey = "a11y.textScale";
        public const string A11yShakeKey = "a11y.cameraShakeScale";
        public const string A11yReduceFlashingKey = "a11y.reduceFlashing";
        public const string A11ySubtitlesKey = "a11y.subtitlesEnabled";
        public const string A11ySpeakerKey = "a11y.showSpeakerName";
        public const string A11yFlashWarningSeenKey = "a11y.flashWarningSeen";

        private const string InputTokenPrefix = "{input:";

        public static LocalizationManager Instance { get; private set; }

        [Tooltip("Satu aset per bahasa.")]
        [SerializeField] private LocalizationTable[] tables = new LocalizationTable[0];

        // Usulan disetujui developer (#104 poin 1): Bahasa Indonesia sebagai bahasa awal dan cadangan.
        [SerializeField] private string fallbackLocale = "id";

        public string CurrentLocale { get; private set; } = "id";
        public AccessibilityOptions Accessibility { get; private set; } = AccessibilityOptions.Default;
        public CultureInfo Culture { get; private set; } = CultureInfo.InvariantCulture;

        private readonly Dictionary<string, string> current = new Dictionary<string, string>(1024);
        private readonly Dictionary<string, string> fallback = new Dictionary<string, string>(1024);
        private readonly Dictionary<string, string> resolvedCache = new Dictionary<string, string>(64);
        private readonly HashSet<string> missingLogged = new HashSet<string>();
        private readonly StringBuilder builder = new StringBuilder(128);

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("[LocalizationManager] Instance ganda. Periksa prefab PersistentRoot.");
                return;
            }
            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void OnEnable()
        {
            EventChannel<InputBindingsChangedEvent>.Raised += HandleBindingsChanged;
        }

        private void OnDisable()
        {
            EventChannel<InputBindingsChangedEvent>.Raised -= HandleBindingsChanged;
        }

        // ---------------------------------------------------------------- Layanan

        public void Initialize()
        {
            BuildTable(fallback, FindTable(fallbackLocale));
            if (fallback.Count == 0) Debug.LogWarning($"[LocalizationManager] Tabel cadangan '{fallbackLocale}' kosong atau tidak ada.");

            // Bahasa di PlayerPrefs yang tidak dikenal kembali ke bahasa cadangan.
            string saved = PlayerPrefs.GetString(LocalePrefsKey, fallbackLocale);
            if (FindTable(saved) == null) saved = fallbackLocale;
            ApplyLocale(saved);

            var o = AccessibilityOptions.Default;
            o.textScale = PlayerPrefs.GetFloat(A11yTextScaleKey, o.textScale);
            o.cameraShakeScale = PlayerPrefs.GetFloat(A11yShakeKey, o.cameraShakeScale);
            o.reduceFlashing = PlayerPrefs.GetInt(A11yReduceFlashingKey, o.reduceFlashing ? 1 : 0) != 0;
            o.subtitlesEnabled = PlayerPrefs.GetInt(A11ySubtitlesKey, o.subtitlesEnabled ? 1 : 0) != 0;
            o.showSpeakerName = PlayerPrefs.GetInt(A11ySpeakerKey, o.showSpeakerName ? 1 : 0) != 0;
            Accessibility = Clamp(o);

            EventChannel<LocaleChangedEvent>.Publish(new LocaleChangedEvent(CurrentLocale));
            EventChannel<AccessibilityChangedEvent>.Publish(new AccessibilityChangedEvent(Accessibility));
        }

        /// <summary>Bahasa dan aksesibilitas tetap. Event sticky diterbitkan ulang karena GameEventRegistry.ClearAll menghapusnya.</summary>
        public void ResetForNewRun()
        {
            resolvedCache.Clear();
            EventChannel<LocaleChangedEvent>.Publish(new LocaleChangedEvent(CurrentLocale));
            EventChannel<AccessibilityChangedEvent>.Publish(new AccessibilityChangedEvent(Accessibility));
        }

        // ---------------------------------------------------------------- Bahasa

        public int LocaleCount => tables != null ? tables.Length : 0;

        public string GetLocaleId(int index)
        {
            return tables != null && index >= 0 && index < tables.Length && tables[index] != null ? tables[index].localeId : string.Empty;
        }

        public void SetLocale(string localeId)
        {
            if (FindTable(localeId) == null)
            {
                Debug.LogWarning($"[LocalizationManager] Bahasa '{localeId}' tidak dikenal.");
                return;
            }
            ApplyLocale(localeId);
            PlayerPrefs.SetString(LocalePrefsKey, localeId);
            PlayerPrefs.Save();
            EventChannel<LocaleChangedEvent>.Publish(new LocaleChangedEvent(CurrentLocale));
        }

        private void ApplyLocale(string localeId)
        {
            var table = FindTable(localeId);
            BuildTable(current, table);
            CurrentLocale = localeId;
            Culture = CultureInfo.InvariantCulture;
            if (table != null && !string.IsNullOrEmpty(table.cultureName))
            {
                try { Culture = CultureInfo.GetCultureInfo(table.cultureName); }
                catch (CultureNotFoundException) { Debug.LogWarning($"[LocalizationManager] Culture '{table.cultureName}' tidak dikenal."); }
            }
            resolvedCache.Clear();
        }

        private LocalizationTable FindTable(string localeId)
        {
            if (tables == null || string.IsNullOrEmpty(localeId)) return null;
            for (int i = 0; i < tables.Length; i++)
                if (tables[i] != null && tables[i].localeId == localeId) return tables[i];
            return null;
        }

        private static void BuildTable(Dictionary<string, string> target, LocalizationTable table)
        {
            target.Clear();
            if (table == null || table.rows == null) return;
            for (int i = 0; i < table.rows.Length; i++)
            {
                var row = table.rows[i];
                if (string.IsNullOrEmpty(row.key)) continue;
                target[row.key] = row.text ?? string.Empty;
            }
        }

        // ---------------------------------------------------------------- Teks

        /// <summary>True kalau kunci ada di bahasa aktif atau cadangan.</summary>
        public bool Has(string key)
        {
            return !string.IsNullOrEmpty(key) && (current.ContainsKey(key) || fallback.ContainsKey(key));
        }

        /// <summary>
        /// Teks untuk kunci. Cari di bahasa aktif, lalu cadangan. Kunci hilang dicatat sekali; Editor menampilkan #key#,
        /// build lain mengembalikan kunci mentah (#99).
        /// </summary>
        public string Get(string key)
        {
            if (string.IsNullOrEmpty(key)) return string.Empty;
            if (resolvedCache.TryGetValue(key, out var cached)) return cached;

            if (!current.TryGetValue(key, out var text) && !fallback.TryGetValue(key, out text))
            {
                if (missingLogged.Add(key)) Debug.LogWarning($"[LocalizationManager] Kunci '{key}' tidak ada di '{CurrentLocale}' maupun '{fallbackLocale}'.");
#if UNITY_EDITOR
                return "#" + key + "#";
#else
                return key;
#endif
            }

            if (text.IndexOf(InputTokenPrefix, StringComparison.Ordinal) < 0) return text;

            // Hasil token di-cache per kunci, dibuang saat InputBindingsChangedEvent atau LocaleChangedEvent.
            string resolved = ResolveInputTokens(text);
            resolvedCache[key] = resolved;
            return resolved;
        }

        public string Format(string key, string name, string value)
        {
            return Get(key).Replace("{" + name + "}", value ?? string.Empty);
        }

        public string Format(string key, string name1, string value1, string name2, string value2)
        {
            return Format(key, name1, value1).Replace("{" + name2 + "}", value2 ?? string.Empty);
        }

        public string Format(string key, string name1, string value1, string name2, string value2, string name3, string value3)
        {
            return Format(key, name1, value1, name2, value2).Replace("{" + name3 + "}", value3 ?? string.Empty);
        }

        /// <summary>Ganti {input:Action} dengan InputManager.GetBindingDisplay. Token tak dikenal dibiarkan dan dicatat sekali.</summary>
        private string ResolveInputTokens(string text)
        {
            builder.Clear();
            int i = 0;
            while (i < text.Length)
            {
                int start = text.IndexOf(InputTokenPrefix, i, StringComparison.Ordinal);
                if (start < 0) { builder.Append(text, i, text.Length - i); break; }
                int end = text.IndexOf('}', start);
                if (end < 0) { builder.Append(text, i, text.Length - i); break; }

                builder.Append(text, i, start - i);
                int nameStart = start + InputTokenPrefix.Length;
                string actionName = text.Substring(nameStart, end - nameStart);
                if (Enum.TryParse(actionName, out GameInputAction action) && InputManager.Instance != null)
                {
                    builder.Append(InputManager.Instance.GetBindingDisplay(action));
                }
                else
                {
                    if (missingLogged.Add("{input:" + actionName + "}"))
                        Debug.LogWarning($"[LocalizationManager] Token tombol '{actionName}' tidak dikenal.");
                    builder.Append(text, start, end - start + 1);
                }
                i = end + 1;
            }
            return builder.ToString();
        }

        private void HandleBindingsChanged(InputBindingsChangedEvent e)
        {
            resolvedCache.Clear();
        }

        // ---------------------------------------------------------------- Angka, hari, dan jamak

        /// <summary>1250 menjadi 1.250 (id) atau 1,250 (en).</summary>
        public string FormatNumber(int n) => n.ToString("N0", Culture);

        /// <summary>Memakai kunci hud.day ({day}): "Hari ke-12" (id) dan "Day 12" (en).</summary>
        public string FormatDay(int day) => Format("hud.day", "day", FormatNumber(day));

        /// <summary>Format 24 jam di kedua bahasa, misalnya 06:30.</summary>
        public string FormatClock(float hour)
        {
            hour = Mathf.Repeat(hour, 24f);
            int h = Mathf.FloorToInt(hour);
            int m = Mathf.Clamp(Mathf.FloorToInt((hour - h) * 60f), 0, 59);
            return h.ToString("00", CultureInfo.InvariantCulture) + ":" + m.ToString("00", CultureInfo.InvariantCulture);
        }

        /// <summary>key.one kalau count == 1, selain itu key.other (cadangan untuk Bahasa Indonesia). {count} diganti.</summary>
        public string GetPlural(string key, int count)
        {
            string chosen = count == 1 && Has(key + ".one") ? key + ".one" : key + ".other";
            return Format(chosen, "count", FormatNumber(count));
        }

        // ---------------------------------------------------------------- Aksesibilitas

        /// <summary>Dipanggil menu #19. Langsung disimpan ke PlayerPrefs.</summary>
        public void SetAccessibility(in AccessibilityOptions o)
        {
            Accessibility = Clamp(o);
            var a = Accessibility;
            PlayerPrefs.SetFloat(A11yTextScaleKey, a.textScale);
            PlayerPrefs.SetFloat(A11yShakeKey, a.cameraShakeScale);
            PlayerPrefs.SetInt(A11yReduceFlashingKey, a.reduceFlashing ? 1 : 0);
            PlayerPrefs.SetInt(A11ySubtitlesKey, a.subtitlesEnabled ? 1 : 0);
            PlayerPrefs.SetInt(A11ySpeakerKey, a.showSpeakerName ? 1 : 0);
            PlayerPrefs.Save();
            EventChannel<AccessibilityChangedEvent>.Publish(new AccessibilityChangedEvent(a));
        }

        private static AccessibilityOptions Clamp(AccessibilityOptions o)
        {
            // Usulan disetujui developer (#104 poin 3): textScale 0,85 sampai 1,5, cameraShakeScale 0 sampai 1.
            var b = GameManager.CurrentBalance;
            if (b != null) o.textScale = Mathf.Clamp(o.textScale, b.localization.textScaleMin, b.localization.textScaleMax);
            o.cameraShakeScale = Mathf.Clamp01(o.cameraShakeScale);
            return o;
        }

        // ---------------------------------------------------------------- Peringatan kilatan

        public bool FlashWarningSeen => PlayerPrefs.GetInt(A11yFlashWarningSeenKey, 0) != 0;

        /// <summary>
        /// Usulan disetujui developer (#104 poin 4): peringatan kilatan tampil sekali saat peluncuran pertama.
        /// Dipanggil MainMenuController. Ya = Lanjut, Tidak (atau Esc) = Kurangi kilatan. false kalau tidak ditampilkan.
        /// </summary>
        public bool TryShowFlashWarning()
        {
            if (FlashWarningSeen || UIManager.Instance == null) return false;
            var request = new ConfirmRequest
            {
                titleKey = "ui.flash_warning.title",
                bodyKey = "ui.flash_warning",
                yesKey = "ui.flash_warning.continue",
                noKey = "ui.flash_warning.reduce",
                onResult = HandleFlashWarningResult
            };
            if (!UIManager.Instance.ShowConfirm(request)) return false;
            PlayerPrefs.SetInt(A11yFlashWarningSeenKey, 1);
            PlayerPrefs.Save();
            return true;
        }

        private void HandleFlashWarningResult(bool proceed)
        {
            if (proceed) return;
            var o = Accessibility;
            o.reduceFlashing = true;
            SetAccessibility(o);
        }
    }

    /// <summary>
    /// Pembatas kilatan layar penuh sampai GameBalanceConfig.localization.maxFlashesPerSecond (#104).
    /// Pemanggil (DisasterManager, DiseaseManager, PostProcessController, VFXManager) menanyakan Allow() sebelum
    /// membuat kilatan layar penuh. Memakai Time.unscaledTime.
    /// </summary>
    public static class FlashGuard
    {
        private static readonly float[] times = new float[16];
        private static int head, count;

        /// <summary>true kalau satu kilatan lagi boleh dibuat dalam satu detik terakhir. Kalau boleh, kilatan dicatat.</summary>
        public static bool Allow()
        {
            var b = GameManager.CurrentBalance;
            int max = b != null ? Mathf.Clamp(b.localization.maxFlashesPerSecond, 0, times.Length) : times.Length;
            float now = Time.unscaledTime;

            // Buang catatan yang lebih tua dari satu detik.
            while (count > 0 && now - times[(head - count + times.Length) % times.Length] >= 1f) count--;
            if (count >= max) return false;

            times[head] = now;
            head = (head + 1) % times.Length;
            if (count < times.Length) count++;
            return true;
        }

        /// <summary>Kilatan sedang diminta dikurangi (Accessibility.reduceFlashing).</summary>
        public static bool ReduceFlashing => LocalizationManager.Instance != null && LocalizationManager.Instance.Accessibility.reduceFlashing;
    }
}
