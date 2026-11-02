using System;
using UnityEngine;

public class ObjectSpawner_Landmark : ObjectSpawner_Chunk
{
	protected override void Start()
	{
		base.Start();
		OnRemoveSpawn = (Action<GameObject>)Delegate.Combine(OnRemoveSpawn, new Action<GameObject>(OnRemoveSpawned));
	}

	private void OnRemoveSpawned(GameObject removedObj)
	{
		if (removedObj != null)
		{
			Landmark component = removedObj.GetComponent<Landmark>();
			if (component != null)
			{
				component.OnRemove();
			}
			else
			{
				Debug.Log("ObjectSpawner_Landmark: Could not call OnRemove on landmark, can't find landmarkscript");
			}
		}
	}

	public override PickupItem_Networked SpawnItem(Vector3 position, Vector3 eulerRotation, SpawnableFloatingObject spawnableObject, uint spawnObjectIndex, uint spawnBehaviourIndex, int poolDuplicateIndex)
	{
		PickupItem_Networked pickupItem_Networked = base.SpawnItem(position, eulerRotation, spawnableObject, spawnObjectIndex, spawnBehaviourIndex, poolDuplicateIndex);
		if (pickupItem_Networked != null)
		{
			Landmark component = pickupItem_Networked.GetComponent<Landmark>();
			if (component != null)
			{
				component.OnSpawn();
			}
			else
			{
				Debug.Log("ObjectSpawner_Landmark: Could not reset pickups on landmark, can't find landmarkscript");
			}
		}
		return pickupItem_Networked;
	}
}
