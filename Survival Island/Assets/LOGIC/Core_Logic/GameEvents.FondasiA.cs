namespace SurvivalIsland
{
    // =====================================================================================
    // Event grup FondasiA (sistem pendukung). Sumber: tabel "Daftar event global" di Core_Logic/GameEventBus.md (#107).
    // Hanya event yang DIKIRIM sistem grup ini, ditambah SubtitleRequestedEvent yang pendengarnya milik #104
    // (pengirimnya, NarrativeSequencer, DisasterManager, dan NPCBarks, memakai struct ini dan tidak mendeklarasikannya ulang).
    // =====================================================================================

    /// <summary>Bagian setting yang berubah. Graphics = setelah ApplyGraphics, All = setelah Initialize atau Commit.</summary>
    public enum SettingsCategory { All, Graphics }

    /// <summary>Setting pemain berubah. Pengirim: SettingsManager (#19). Didengar VFXManager (#15).</summary>
    [StickyEvent]
    public struct SettingsChangedEvent
    {
        public SettingsCategory category;
        public SettingsChangedEvent(SettingsCategory category) { this.category = category; }
    }

    /// <summary>Pilihan aksesibilitas berubah. Pengirim: LocalizationManager (#104). Didengar LocalizedText dan subtitle.</summary>
    [StickyEvent]
    public struct AccessibilityChangedEvent
    {
        public AccessibilityOptions options;
        public AccessibilityChangedEvent(in AccessibilityOptions options) { this.options = options; }
    }

    /// <summary>Asal subtitle (LocalizationManager.md, tabel sumber subtitle).</summary>
    public enum SubtitleSourceKind { Scene, Warning, Bark }

    /// <summary>
    /// Minta satu baris subtitle. Pengirim: NarrativeSequencer (#99, prioritas 3), DisasterManager (#70, prioritas 2),
    /// NPCBarks (#100, prioritas 1). Didengar SubtitlePresenter.
    /// </summary>
    public struct SubtitleRequestedEvent
    {
        public string textKey;
        public string speakerKey;   // Kosong berarti tanpa nama
        public int priority;
        public SubtitleSourceKind sourceKind;
        public SubtitleRequestedEvent(string textKey, string speakerKey, int priority, SubtitleSourceKind sourceKind)
        { this.textKey = textKey; this.speakerKey = speakerKey; this.priority = priority; this.sourceKind = sourceKind; }
    }
}
