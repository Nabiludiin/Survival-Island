namespace SurvivalIsland
{
    // =====================================================================================
    // Event grup C2 (sistem pendukung). Sumber: tabel "Daftar event global" di Core_Logic/GameEventBus.md (#107).
    // Hanya event yang DIKIRIM sistem grup ini: MapMarkerRegistry (milik CompassSystem, #30) dan NarrativeSequencer (#99).
    // CinematicStateChangedEvent dan SubtitleRequestedEvent sudah ada (GameEvents.cs, GameEvents.FondasiA.cs).
    // =====================================================================================

    /// <summary>Isi registry penanda berubah. Pengirim: MapMarkerRegistry (milik CompassSystem, #30). Tidak sticky.</summary>
    public struct MapMarkerChangedEvent
    {
        public string markerId;
        public bool isRemoved;
        public MapMarkerChangedEvent(string markerId, bool isRemoved) { this.markerId = markerId; this.isRemoved = isRemoved; }
    }

    /// <summary>Adegan cerita mulai atau selesai. Pengirim: NarrativeSequencer (#99). Didengar CombatMusicDirector (#114).</summary>
    [StickyEvent]
    public struct NarrativeStateChangedEvent
    {
        public string sceneId;
        public bool isPlaying;
        public NarrativeStateChangedEvent(string sceneId, bool isPlaying) { this.sceneId = sceneId; this.isPlaying = isPlaying; }
    }
}
