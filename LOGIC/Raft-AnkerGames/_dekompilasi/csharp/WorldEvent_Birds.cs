using UnityEngine;

public class WorldEvent_Birds : WorldEvent
{
	public WorldEvent_Birds(SO_WorldEvent data, WorldEventManager handler)
		: base(data, handler)
	{
	}

	public override void StartEvent()
	{
		if (eventData.handledByClient && !Raft_Network.IsHost)
		{
			SpawnEntity(AI_NetworkBehaviourType.BirdPack, GetBirdpackSpawnPosition());
		}
		else if (eventData.handledByServer && Raft_Network.IsHost)
		{
			SpawnEntity(AI_NetworkBehaviourType.BirdPack, GetBirdpackSpawnPosition());
		}
		base.StartEvent();
	}

	public Vector3 GetBirdpackSpawnPosition()
	{
		Vector3 result = ((SingletonGeneric<GameManager>.Singleton != null) ? SingletonGeneric<GameManager>.Singleton.lockedPivot.position : Vector3.zero) + Random.insideUnitSphere.XZOnly().normalized * 600f;
		result.y = 100f;
		return result;
	}
}
