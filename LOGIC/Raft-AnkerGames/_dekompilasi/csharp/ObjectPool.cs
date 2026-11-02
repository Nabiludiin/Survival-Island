using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObjectPool
{
	public string name;

	[SerializeField]
	public int poolDuplicateIndex;

	[SerializeField]
	private GameObject objectPrefab;

	[SerializeField]
	private int initialPoolSize = 50;

	[SerializeField]
	private bool canGrow = true;

	private List<GameObject> pool = new List<GameObject>();

	public List<GameObject> Objects => pool;

	public GameObject ObjectPrefab => objectPrefab;

	public void Initialize()
	{
		if (objectPrefab == null)
		{
			Debug.LogError("Could not instantiate pool prefab, prefab was null. Regarding: " + name);
			return;
		}
		for (int i = 0; i < initialPoolSize; i++)
		{
			AddObjectToPool();
		}
	}

	public GameObject GetObject(bool returnObjectActive)
	{
		for (int i = 0; i < pool.Count; i++)
		{
			GameObject gameObject = pool[i];
			if (gameObject == null)
			{
				pool.RemoveAt(i);
				i--;
			}
			else if (!gameObject.activeSelf)
			{
				if (returnObjectActive)
				{
					gameObject.SetActive(value: true);
				}
				if (gameObject.transform.localScale != objectPrefab.transform.localScale)
				{
					gameObject.transform.localScale = objectPrefab.transform.localScale;
				}
				return gameObject;
			}
		}
		if (canGrow)
		{
			GameObject gameObject2 = AddObjectToPool();
			if (returnObjectActive)
			{
				gameObject2.SetActive(value: true);
			}
			return gameObject2;
		}
		return null;
	}

	private GameObject AddObjectToPool()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(objectPrefab, Vector3.zero, Quaternion.identity);
		pool.Add(gameObject);
		gameObject.transform.SetParent(PoolManager.Transform);
		gameObject.SetActive(value: false);
		return gameObject;
	}
}
