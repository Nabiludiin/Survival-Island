namespace SurvivalIsland
{
    // =====================================================================================
    // Event milik sistem pendukung grup FondasiB (StatusEffectManager #20, DurabilitySystem #113).
    // Sumber: tabel "Daftar event global" di Core_Logic/GameEventBus.md. Nama field mengikuti kolom "Isi".
    // =====================================================================================

    /// <summary>
    /// Efek status dipasang, bertumpuk, atau dilepas. Pengirim: StatusEffectManager (#20). Didengar HUDManager (StatusEffectBar).
    /// Tidak sticky: HUD membaca StatusEffectManager.Active saat aktif.
    /// </summary>
    public struct StatusEffectChangedEvent
    {
        public string effectId;
        public bool isActive;
        public int stacks;
        public StatusEffectChangedEvent(string effectId, bool isActive, int stacks)
        { this.effectId = effectId; this.isActive = isActive; this.stacks = stacks; }
    }

    /// <summary>
    /// Status durabilitas satu item berubah (Normal, Low, Broken, Destroyed). Pengirim: DurabilitySystem dan ToolRepairSystem (#113).
    /// Didengar HUDManager (bar hotbar) dan NotificationManager (#22). Tidak sticky.
    /// </summary>
    public struct DurabilityStateChangedEvent
    {
        public int instanceId;
        public string itemId;
        public DurabilityState state;
        public DurabilityStateChangedEvent(int instanceId, string itemId, DurabilityState state)
        { this.instanceId = instanceId; this.itemId = itemId; this.state = state; }
    }
}
