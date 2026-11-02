using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PickupItem_Networked : MonoBehaviour_ID_Network
{
	public Action OnPickedUp;

	public ObjectSpawnType spawnType = ObjectSpawnType.Prefab;

	[HideInInspector]
	public ObjectSpawner spawner;

	[HideInInspector]
	public Scene originalScene;

	[HideInInspector]
	public uint uniqueSpawnableIndex;

	[HideInInspector]
	public int poolDuplicateIndex;

	public bool stopTrackingOnPickup;

	[Header("Send pickup message to everyone")]
	public bool stopTrackUseRPC = true;

	[Header("Optional old system, untrack behaviour automatically")]
	public MonoBehaviour_Network networkBehaviour;

	private PickupItem pickupItem;

	public PickupItem PickupItem
	{
		get
		{
			if (pickupItem != null)
			{
				return pickupItem;
			}
			pickupItem = GetComponent<PickupItem>();
			return pickupItem;
		}
	}

	public virtual bool CanBePickedUp()
	{
		switch (spawnType)
		{
		case ObjectSpawnType.AssetBundleScene:
		case ObjectSpawnType.Addressable:
			return true;
		case ObjectSpawnType.NetworkBehaviour:
			return true;
		case ObjectSpawnType.Prefab:
			return true;
		case ObjectSpawnType.Pool:
			return gameObject.activeSelf;
		case ObjectSpawnType.GameObject:
			return gameObject.activeSelf;
		default:
			return true;
		}
	}

	public virtual void HandleOnPickedUp()
	{
		switch (spawnType)
		{
		case ObjectSpawnType.Pool:
			PoolManager.ReturnObject(gameObject);
			break;
		case ObjectSpawnType.GameObject:
			gameObject.SetActiveSafe(value: false);
			break;
		case ObjectSpawnType.AssetBundleScene:
			gameObject.SetActiveSafe(value: false);
			_ = originalScene;
			transform.SetParent(null);
			SceneManager.MoveGameObjectToScene(gameObject, originalScene);
			break;
		case ObjectSpawnType.Addressable:
			gameObject.SetActive(value: false);
			transform.SetParent(null);
			break;
		case ObjectSpawnType.Prefab:
			UnityEngine.Object.Destroy(gameObject);
			break;
		case ObjectSpawnType.NetworkBehaviour:
			break;
		}
	}

	public virtual void OnRestored()
	{
	}
}
