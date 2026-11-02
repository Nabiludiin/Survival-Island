public class WorldEvent_Dolphins : WorldEvent
{
	public WorldEvent_Dolphins(SO_WorldEvent data, WorldEventManager handler)
		: base(data, handler)
	{
	}

	public override void StartEvent()
	{
		if (eventData.handledByClient && !Raft_Network.IsHost)
		{
			SpawnEntity(AI_NetworkBehaviourType.Dolphin, Restriction_LineCollideWithChunkpoint.latestValidSpawnPoint);
		}
		else if (eventData.handledByServer && Raft_Network.IsHost)
		{
			SpawnEntity(AI_NetworkBehaviourType.Dolphin, Restriction_LineCollideWithChunkpoint.latestValidSpawnPoint);
		}
		base.StartEvent();
	}
}
