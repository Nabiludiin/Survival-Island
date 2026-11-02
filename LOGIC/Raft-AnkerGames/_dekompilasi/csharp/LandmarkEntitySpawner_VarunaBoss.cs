using UnityEngine;

public class LandmarkEntitySpawner_VarunaBoss : LandmarkEntitySpawner
{
	public QuestEventBase questEventGarageDoor;

	public QuestEventBase questEventLastPillar;

	private float timer;

	protected override void OnEnable()
	{
		base.OnEnable();
		timer = 15f;
	}

	private void Update()
	{
		if (!Raft_Network.IsHost || SaveAndLoad.IsGameLoading)
		{
			return;
		}
		if (timer > 0f)
		{
			timer -= Time.deltaTime;
			return;
		}
		timer = 15f;
		if (spawnedEntityBehaviour == null && questEventGarageDoor.IsAtMaxStateIndex && !questEventLastPillar.IsAtMaxStateIndex)
		{
			CreateEntity();
			Debug.Log("Spawned Varuna Boss Shark as FIX");
		}
	}

	protected override void TryToSpawnEntity()
	{
		if (!questEventLastPillar.IsAtMaxStateIndex && !(spawnedEntityBehaviour != null))
		{
			base.TryToSpawnEntity();
		}
	}
}
