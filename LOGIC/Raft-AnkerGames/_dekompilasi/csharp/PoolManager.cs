using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
	public static Transform Transform;

	public List<ObjectPool> objectPools = new List<ObjectPool>();

	private void Awake()
	{
		ComponentManager<PoolManager>.Value = this;
		Transform = transform;
		foreach (ObjectPool objectPool in objectPools)
		{
			objectPool.Initialize();
		}
	}

	public GameObject GetObject(string poolName, bool returnObjectActive = true, int poolDuplicateIndex = 0)
	{
		for (int i = 0; i < objectPools.Count; i++)
		{
			ObjectPool objectPool = objectPools[i];
			if (objectPool.name == poolName && objectPool.poolDuplicateIndex == poolDuplicateIndex)
			{
				return objectPool.GetObject(returnObjectActive);
			}
		}
		Debug.Log("Tried to access pool with name [" + poolName + "] from pool manager but there was no pool with that name");
		return null;
	}

	public List<ObjectPool> GetPoolsByName(string poolName)
	{
		List<ObjectPool> list = null;
		foreach (ObjectPool objectPool in objectPools)
		{
			if (objectPool.name == poolName)
			{
				if (list == null)
				{
					list = new List<ObjectPool>();
				}
				list.Add(objectPool);
			}
		}
		return list;
	}

	public ObjectPool GetPoolByName(string poolName)
	{
		foreach (ObjectPool objectPool in objectPools)
		{
			if (objectPool.name == poolName)
			{
				return objectPool;
			}
		}
		return null;
	}

	public static void ReturnObject(GameObject obj)
	{
		if (GameManager.IsQuittingGame)
		{
			return;
		}
		if (Transform == null || Transform.gameObject == null)
		{
			Debug.Log("Can't return object, Poolmanager is null");
			return;
		}
		if (obj.transform.parent != Transform)
		{
			obj.transform.SetParent(Transform);
		}
		obj.SetActive(value: false);
	}

	public int GetDuplicatePoolCount(string poolName)
	{
		int num = 0;
		for (int i = 0; i < objectPools.Count; i++)
		{
			if (objectPools[i].name == poolName)
			{
				num++;
			}
		}
		return num;
	}
}
