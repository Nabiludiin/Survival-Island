using System;
using UnityEngine;

[Serializable]
public class SpawnableFloatingObject
{
	public uint uniqueIndex;

	public bool hasNetworkBehaviour;

	public bool recieveOnSpawnMessage;

	public ObjectSpawnType typeToSpawn = ObjectSpawnType.Prefab;

	public string poolName = "DefaultPool";

	public string assetBundleKey;

	public GameObject prefab;

	public Vector3 minRotation = new Vector3(-10f, 0f, -10f);

	public Vector3 maxRotation = new Vector3(10f, 360f, 10f);

	public float spawnChance = 100f;

	public int poolDuplicateCount;

	private bool IsPrefab()
	{
		return typeToSpawn == ObjectSpawnType.Prefab;
	}

	private bool IsPool()
	{
		return typeToSpawn == ObjectSpawnType.Pool;
	}

	private bool UseAssetBundleKey()
	{
		if (typeToSpawn != ObjectSpawnType.AssetBundleScene)
		{
			return typeToSpawn == ObjectSpawnType.Addressable;
		}
		return true;
	}
}
