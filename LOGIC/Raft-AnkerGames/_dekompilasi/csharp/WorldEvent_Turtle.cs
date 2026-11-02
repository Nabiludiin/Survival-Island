public class WorldEvent_Turtle : WorldEvent
{
	public WorldEvent_Turtle(SO_WorldEvent data, WorldEventManager handler)
		: base(data, handler)
	{
	}

	public override void StartEvent()
	{
		if (eventData.handledByClient && !Raft_Network.IsHost)
		{
			SpawnEntity(AI_NetworkBehaviourType.Turtle, Restriction_LineCollideWithChunkpoint.latestValidSpawnPoint);
		}
		else if (eventData.handledByServer && Raft_Network.IsHost)
		{
			SpawnEntity(AI_NetworkBehaviourType.Turtle, Restriction_LineCollideWithChunkpoint.latestValidSpawnPoint);
		}
		base.StartEvent();
	}
}
