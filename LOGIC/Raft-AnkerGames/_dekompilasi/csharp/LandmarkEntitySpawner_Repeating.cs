using UnityEngine;

public class LandmarkEntitySpawner_Repeating : LandmarkEntitySpawner
{
	[SerializeField]
	[Min(1f)]
	private float trySpawnInterval = 10f;

	[SerializeField]
	private GizmoBox spawnZone;

	private float spawnTimer;

	private static Raft_Network network;

	public override void AwakeCall()
	{
		base.AwakeCall();
		if (Raft_Network.IsHost)
		{
			spawnTimer = Random.Range(1f, trySpawnInterval);
		}
	}

	protected override void Start()
	{
		base.Start();
		if (Raft_Network.IsHost && network == null)
		{
			network = ComponentManager<Raft_Network>.Value;
		}
	}

	protected virtual void Update()
	{
		if (Raft_Network.IsHost && !HasSpawnedEntity())
		{
			spawnTimer -= Time.deltaTime;
			if (spawnTimer <= 0f)
			{
				spawnTimer = trySpawnInterval;
				TryToSpawnEntity();
			}
		}
	}

	protected override void TryToSpawnEntity()
	{
		if (!HasSpawnedEntity() && (!(spawnZone != null) || !spawnZone.IsColliding(includeInactiveObjects: false, LayerMasks.MASK_Players, 1f, QueryTriggerInteraction.Ignore)))
		{
			base.TryToSpawnEntity();
		}
	}
}
