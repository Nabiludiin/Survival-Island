namespace SurvivalIsland
{
    /// <summary>
    /// Batu kecil dan ranting yang tumbuh lagi (#116). Blueprint: raft/ResourceRespawn.md bagian "Batu kecil dan ranting".
    /// Jadwal dari GameBalanceConfig.resource.stoneRespawnDays. Sumber batu pertama tanpa alat (ProgressionSafety.md).
    /// ResourceRespawn.CountActiveStones dan ForceRegrowStones menghitung semua PickupInteractable Mat_Stone, termasuk ini.
    /// </summary>
    public class StonePickup : RegrowablePickup
    {
        protected override float RespawnDays { get { var r = Resource; return r != null ? r.stoneRespawnDays : 0f; } }
    }
}
