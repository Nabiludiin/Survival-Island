public class WorldEvent_Whale : WorldEvent
{
	public WorldEvent_Whale(SO_WorldEvent data, WorldEventManager handler)
		: base(data, handler)
	{
	}

	public override void StartEvent()
	{
		if (eventData.handledByClient && !Raft_Network.IsHost)
		{
			SpawnEntity(AI_NetworkBehaviourType.Whale, Restriction_LineCollideWithChunkpoint.latestValidSpawnPoint);
		}
		else if (eventData.handledByServer && Raft_Network.IsHost)
		{
			SpawnEntity(AI_NetworkBehaviourType.Whale, Restriction_LineCollideWithChunkpoint.latestValidSpawnPoint);
		}
		base.StartEvent();
	}
}
