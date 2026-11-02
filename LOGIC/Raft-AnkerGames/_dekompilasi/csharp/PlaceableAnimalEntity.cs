using System.Collections;
using UnityEngine;

public class PlaceableAnimalEntity : Block
{
	public AI_NetworkBehaviourType animalType;

	public int spawnCount = 1;

	private static Network_Host_Entities networkHostEntities;

	private bool isPlaced;

	protected override void Start()
	{
		if (isPlaced)
		{
			base.Start();
			networkHostEntities = ComponentManager<Network_Host_Entities>.Value;
			if (Raft_Network.IsHost)
			{
				SpawnEntityType();
			}
			gameObject.SetActiveSafe(value: false);
		}
	}

	public override void OnFinishedPlacement()
	{
		base.OnFinishedPlacement();
		isPlaced = true;
		if (Raft_Network.IsHost)
		{
			StartCoroutine("RemoveBlock", 1f);
		}
	}

	public void SpawnEntityType()
	{
		Vector3 position = transform.position;
		for (int i = 0; i < spawnCount; i++)
		{
			position.y += 0.25f;
			(networkHostEntities.CreateAINetworkBehaviour(animalType, position) as AI_NetworkBehaviour_Domestic).QuickTameLate();
		}
	}

	private IEnumerator RemoveBlock(float delay)
	{
		yield return new WaitForSeconds(delay);
		BlockCreator.RemoveBlockNetwork(this, null, updateRaftBounds: true);
	}
}
