public class WorldEvent_Stingray : WorldEvent
{
	public WorldEvent_Stingray(SO_WorldEvent data, WorldEventManager handler)
		: base(data, handler)
	{
	}

	public override void StartEvent()
	{
		if (eventData.handledByClient && !Raft_Network.IsHost)
		{
			SpawnEntity(AI_NetworkBehaviourType.Stingray, Restriction_LineCollideWithChunkpoint.latestValidSpawnPoint);
		}
		else if (eventData.handledByServer && Raft_Network.IsHost)
		{
			SpawnEntity(AI_NetworkBehaviourType.Stingray, Restriction_LineCollideWithChunkpoint.latestValidSpawnPoint);
		}
		base.StartEvent();
	}
}
