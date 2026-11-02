using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
	public Action<GameObject> OnSpawn;

	public Action<GameObject> OnRemoveSpawn;

	public uint UniqueIndex;

	public bool setIndexOnSpawn = true;

	public List<SpawnableFloatingObject> spawnableObjects;

	public List<PickupItem_Networked> spawnedObjects = new List<PickupItem_Networked>();

	protected ObjectSpawnerManager objectSpawnerManager;

	protected Raft_Network network;

	protected AssetBundleHooker assetBundleHooker;

	protected SceneLoader sceneLoader;

	private PoolManager poolManager;

	protected virtual void Start()
	{
		objectSpawnerManager = ComponentManager<ObjectSpawnerManager>.Value;
		poolManager = ComponentManager<PoolManager>.Value;
		network = ComponentManager<Raft_Network>.Value;
		assetBundleHooker = ComponentManager<AssetBundleHooker>.Value;
		sceneLoader = ComponentManager<SceneLoader>.Value;
	}

	private void OnDestroy()
	{
		objectSpawnerManager = null;
		poolManager = null;
		network = null;
		assetBundleHooker = null;
		sceneLoader = null;
	}

	protected virtual void Update()
	{
	}

	private void OnValidate()
	{
		if (spawnableObjects == null)
		{
			return;
		}
		PoolManager poolManager = null;
		for (int i = 0; i < spawnableObjects.Count; i++)
		{
			SpawnableFloatingObject spawnableFloatingObject = spawnableObjects[i];
			if (spawnableFloatingObject.typeToSpawn == ObjectSpawnType.Pool && !string.IsNullOrEmpty(spawnableFloatingObject.poolName))
			{
				if (poolManager == null)
				{
					poolManager = UnityEngine.Object.FindObjectOfType<PoolManager>();
				}
				if (poolManager != null)
				{
					spawnableFloatingObject.poolDuplicateCount = poolManager.GetDuplicatePoolCount(spawnableFloatingObject.poolName);
				}
				else
				{
					spawnableFloatingObject.poolDuplicateCount = -1;
				}
			}
			else
			{
				spawnableFloatingObject.poolDuplicateCount = -1;
			}
		}
	}

	public SpawnableFloatingObject GetSpawnableFromIndex(uint uniqueSpawnableIndex)
	{
		return spawnableObjects.Find((SpawnableFloatingObject s) => s.uniqueIndex == uniqueSpawnableIndex);
	}

	public PickupItem_Networked GetSpawnedObjectFromObjectIndex(int objectIndex)
	{
		foreach (PickupItem_Networked spawnedObject in spawnedObjects)
		{
			if (spawnedObject != null && spawnedObject.ObjectIndex == objectIndex)
			{
				return spawnedObject;
			}
		}
		return null;
	}

	public virtual PickupItem_Networked SpawnItem(Vector3 position, Vector3 eulerRotation, SpawnableFloatingObject spawnableObject, uint spawnObjectIndex, uint spawnBehaviourIndex, int poolDuplicateIndex)
	{
		GameObject gameObject = null;
		AsyncedLandmark asyncedLandmark = null;
		switch (spawnableObject.typeToSpawn)
		{
		case ObjectSpawnType.Pool:
			gameObject = poolManager.GetObject(spawnableObject.poolName, returnObjectActive: false, poolDuplicateIndex);
			break;
		case ObjectSpawnType.AssetBundleScene:
			asyncedLandmark = assetBundleHooker.GetLandmarkFromUniqueIndex(poolDuplicateIndex);
			if (asyncedLandmark != null)
			{
				gameObject = asyncedLandmark.landmark.gameObject;
			}
			else
			{
				Helper.LogBuild("The landmark with index " + poolDuplicateIndex + " was not found in dictionary" + spawnableObject.assetBundleKey);
			}
			break;
		case ObjectSpawnType.Addressable:
			gameObject = sceneLoader.SpawnLandmarkFromUniqueIndex(poolDuplicateIndex);
			break;
		default:
			gameObject = UnityEngine.Object.Instantiate(spawnableObject.prefab, position, Quaternion.identity);
			break;
		}
		if (gameObject != null)
		{
			PickupItem_Networked component = gameObject.GetComponent<PickupItem_Networked>();
			if (component != null && spawnedObjects != null && spawnedObjects.Contains(component))
			{
				return null;
			}
			gameObject.SetActiveSafe(value: true);
			if (component != null)
			{
				component.spawnType = spawnableObject.typeToSpawn;
				component.spawner = this;
				component.uniqueSpawnableIndex = spawnableObject.uniqueIndex;
				component.poolDuplicateIndex = poolDuplicateIndex;
				if (spawnableObject.typeToSpawn == ObjectSpawnType.AssetBundleScene)
				{
					component.originalScene = asyncedLandmark.scene;
					component.poolDuplicateIndex = (int)asyncedLandmark.landmark.uniqueLandmarkIndex;
				}
				if (!Raft_Network.IsHost || setIndexOnSpawn)
				{
					component.ObjectIndex = spawnObjectIndex;
				}
			}
			if ((!Raft_Network.IsHost || setIndexOnSpawn) && component.networkBehaviour != null)
			{
				component.networkBehaviour.ObjectIndex = spawnObjectIndex;
				component.networkBehaviour.BehaviourIndex = spawnBehaviourIndex;
				NetworkUpdateManager.AddBehaviour(component.networkBehaviour);
			}
			gameObject.transform.parent = objectSpawnerManager.floatingObjectParent;
			gameObject.transform.position = position;
			gameObject.transform.eulerAngles = eulerRotation;
			gameObject.SetActive(value: true);
			if (component != null)
			{
				spawnedObjects.Add(component);
				NetworkIDManager.AddNetworkID(component);
			}
			if (OnSpawn != null)
			{
				OnSpawn(gameObject);
			}
			if (spawnableObject.recieveOnSpawnMessage)
			{
				gameObject.SendMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);
			}
			return component;
		}
		return null;
	}

	public PickupItem_Networked SpawnItemNetwork(Vector3 spawnPosition, int uniqueSpawnableIndex = -1, int p_poolDuplicateIndex = -1)
	{
		if (objectSpawnerManager == null)
		{
			objectSpawnerManager = ComponentManager<ObjectSpawnerManager>.Value;
		}
		if (network == null)
		{
			network = ComponentManager<Raft_Network>.Value;
		}
		if (poolManager == null)
		{
			poolManager = ComponentManager<PoolManager>.Value;
		}
		if (!Raft_Network.IsHost)
		{
			return null;
		}
		SpawnableFloatingObject spawnableFloatingObject = ((uniqueSpawnableIndex < 0) ? GetObjectToSpawn() : GetSpawnableFromIndex((uint)uniqueSpawnableIndex));
		if (spawnableFloatingObject == null)
		{
			Debug.LogError("NULL, Can't find a spawnable object with index of " + uniqueSpawnableIndex, gameObject);
			return null;
		}
		return SpawnItemNetwork(spawnPosition, spawnableFloatingObject, p_poolDuplicateIndex);
	}

	public PickupItem_Networked SpawnItemNetwork(Vector3 spawnPosition, SpawnableFloatingObject spawnableObject, int p_poolDuplicateIndex = -1)
	{
		if (spawnableObject == null)
		{
			Debug.LogError("SpawnItemNetwork, Can't spawn because spawnableObject is null");
			return null;
		}
		Vector3 vector = new Vector3(UnityEngine.Random.Range(spawnableObject.minRotation.x, spawnableObject.maxRotation.x), UnityEngine.Random.Range(spawnableObject.minRotation.y, spawnableObject.maxRotation.y), UnityEngine.Random.Range(spawnableObject.minRotation.z, spawnableObject.maxRotation.z));
		uint num = (setIndexOnSpawn ? SaveAndLoad.GetUniqueObjectIndex() : 0u);
		uint num2 = ((spawnableObject.hasNetworkBehaviour && setIndexOnSpawn) ? NetworkUpdateManager.GetUniqueBehaviourIndex() : 0u);
		int poolDuplicateIndex = p_poolDuplicateIndex;
		if (p_poolDuplicateIndex == -1)
		{
			poolDuplicateIndex = ((spawnableObject.poolDuplicateCount > 1) ? UnityEngine.Random.Range(0, spawnableObject.poolDuplicateCount) : 0);
		}
		if (spawnableObject.typeToSpawn == ObjectSpawnType.AssetBundleScene)
		{
			AsyncedLandmark asyncedLandmark = null;
			if (p_poolDuplicateIndex == -1)
			{
				asyncedLandmark = assetBundleHooker.GetRandomLandmarkFromAssetBundleKey(spawnableObject.assetBundleKey);
			}
			else
			{
				poolDuplicateIndex = p_poolDuplicateIndex;
				asyncedLandmark = assetBundleHooker.GetLandmarkFromUniqueIndex(poolDuplicateIndex);
			}
			if (asyncedLandmark == null)
			{
				Debug.Log("Asynced Landmark is null :(");
				return null;
			}
			poolDuplicateIndex = ((p_poolDuplicateIndex != -1) ? p_poolDuplicateIndex : ((int)asyncedLandmark.landmark.uniqueLandmarkIndex));
		}
		PickupItem_Networked pickupItem_Networked = SpawnItem(spawnPosition, vector, spawnableObject, num, num2, poolDuplicateIndex);
		if (!setIndexOnSpawn)
		{
			num = pickupItem_Networked.ObjectIndex;
			if (spawnableObject.hasNetworkBehaviour)
			{
				num2 = pickupItem_Networked.networkBehaviour.BehaviourIndex;
			}
		}
		network.RPC(new Message_ObjectSpawner_SpawnItem(Messages.ObjectSpawner_SpawnItem, objectSpawnerManager, spawnPosition, vector, UniqueIndex, num, num2, spawnableObject.uniqueIndex, poolDuplicateIndex), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		return pickupItem_Networked;
	}

	public void ClearItems(bool clearUsingNetwork)
	{
		int num = 1000;
		while (spawnedObjects.Count > 0)
		{
			PickupItem_Networked pickupNetwork = spawnedObjects[0];
			if (clearUsingNetwork)
			{
				PickupObjectManager.RemovePickupItemNetwork(pickupNetwork);
			}
			else
			{
				PickupObjectManager.RemovePickupItem(pickupNetwork);
			}
			num--;
			if (num <= 0)
			{
				break;
			}
		}
	}

	public void RemoveItem(PickupItem_Networked itemToRemove, bool removeUsingNetwork)
	{
		if (spawnedObjects != null && spawnedObjects != null && spawnedObjects.Contains(itemToRemove))
		{
			if (removeUsingNetwork)
			{
				PickupObjectManager.RemovePickupItemNetwork(itemToRemove);
			}
			else
			{
				PickupObjectManager.RemovePickupItem(itemToRemove);
			}
		}
	}

	public int GetRandomPoolDuplicateIndexFromSpawnable(SpawnableFloatingObject spawnableObject)
	{
		if (spawnableObject.typeToSpawn == ObjectSpawnType.Pool && !string.IsNullOrEmpty(spawnableObject.poolName))
		{
			if (poolManager == null)
			{
				poolManager = UnityEngine.Object.FindObjectOfType<PoolManager>();
			}
			if (poolManager != null)
			{
				return UnityEngine.Random.Range(0, poolManager.GetDuplicatePoolCount(spawnableObject.poolName));
			}
			return -1;
		}
		if (spawnableObject.typeToSpawn == ObjectSpawnType.AssetBundleScene && !string.IsNullOrEmpty(spawnableObject.assetBundleKey))
		{
			return ((int?)assetBundleHooker.GetRandomLandmarkFromAssetBundleKey(spawnableObject.assetBundleKey)?.landmark.uniqueLandmarkIndex) ?? (-1);
		}
		return -1;
	}

	public SpawnableFloatingObject GetObjectToSpawn(bool randomizeIfSpawnCountOne = true)
	{
		if (spawnableObjects != null && spawnableObjects.Count == 1)
		{
			if (!randomizeIfSpawnCountOne)
			{
				return spawnableObjects[0];
			}
			if (UnityEngine.Random.Range(0f, 100f) < spawnableObjects[0].spawnChance)
			{
				return spawnableObjects[0];
			}
			return null;
		}
		float num = 0f;
		foreach (SpawnableFloatingObject spawnableObject in spawnableObjects)
		{
			num += spawnableObject.spawnChance;
		}
		float num2 = UnityEngine.Random.Range(0f, num);
		float num3 = 0f;
		foreach (SpawnableFloatingObject spawnableObject2 in spawnableObjects)
		{
			num3 += spawnableObject2.spawnChance;
			if (num2 < num3)
			{
				return spawnableObject2;
			}
		}
		return null;
	}
}
