namespace SurvivalIsland
{
    /// <summary>
    /// Kelapa yang tumbuh lagi (#43). Blueprint: raft/ResourceRespawn.md bagian "Kelapa", dipakai HydrationSystem.md.
    /// Jadwal dari GameBalanceConfig.resource.coconutRespawnDays.
    /// </summary>
    public class CoconutPickup : RegrowablePickup
    {
        protected override float RespawnDays { get { var r = Resource; return r != null ? r.coconutRespawnDays : 0f; } }
    }
}
