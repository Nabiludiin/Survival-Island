namespace SurvivalIsland
{
    // =====================================================================================
    // Event sistem pendukung Grup G (raft: Hydration, Disease, Campfire, Fishing, Debris, Storage).
    // Sumber: tabel "Daftar event global" di Core_Logic/GameEventBus.md. Nama dan field mengikuti tabel itu.
    // StatusEffectChangedEvent (#20) dan TideChangedEvent (#91) milik sistem lain dan TIDAK dideklarasikan di sini.
    // =====================================================================================

    /// <summary>Penyakit sekunder mulai atau berakhir. Pengirim: DiseaseManager (#45). Tidak sticky.</summary>
    public struct DiseaseChangedEvent
    {
        public DiseaseType type;
        public bool isActive;
        public DiseaseChangedEvent(DiseaseType type, bool isActive) { this.type = type; this.isActive = isActive; }
    }

    /// <summary>State Kail berganti. endReason hanya bermakna saat state Recovering. Pengirim: FishingSystem (#55).</summary>
    public struct FishingStateChangedEvent
    {
        public FishingState state;
        public FishingEndReason endReason;
        public FishingStateChangedEvent(FishingState state, FishingEndReason endReason) { this.state = state; this.endReason = endReason; }
    }

    /// <summary>Satu baris masak matang (isBurnt false) atau gosong (isBurnt true). Pengirim: CampfireManager (#44).</summary>
    public struct ItemCookedEvent
    {
        public string recipeId;
        public bool isBurnt;
        public ItemCookedEvent(string recipeId, bool isBurnt) { this.recipeId = recipeId; this.isBurnt = isBurnt; }
    }

    /// <summary>Ikan tertangkap. Pengirim: FishingSystem (#55).</summary>
    public struct FishCaughtEvent
    {
        public string itemId;
        public FishCaughtEvent(string itemId) { this.itemId = itemId; }
    }

    /// <summary>Barang hanyut masuk inventori. Pengirim: DebrisRetrieval (#106).</summary>
    public struct DebrisCollectedEvent
    {
        public string itemId;
        public int amount;
        public DebrisCollectedEvent(string itemId, int amount) { this.itemId = itemId; this.amount = amount; }
    }
}
