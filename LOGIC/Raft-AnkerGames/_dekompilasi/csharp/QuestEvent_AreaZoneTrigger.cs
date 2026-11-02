public class QuestEvent_AreaZoneTrigger : QuestEventBase
{
	public AreaZone_PlayerCounter areaZone;

	public void Update()
	{
		if (Raft_Network.IsHost && CanStillBeInteractedWith && areaZone.PlayersIsInArea)
		{
			SendOnInteractNetwork(areaZone.playersInArea[0]);
		}
	}

	public override bool CanPlayerInteract(Network_Player player, bool overwriteInventoryValidation = false)
	{
		return CanStillBeInteractedWith;
	}
}
