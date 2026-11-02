using System.Collections.Generic;
using UnityEngine;

namespace Knife.DeferredDecals.Spawn;

public class OnePrefabDecalPool : IOnePrefabPool<Decal>, ISpawner<Decal>
{
	private Queue<Decal> pool;

	private readonly int count;

	private readonly bool autoPopulateOnZero;

	private Decal cachedPrefab;

	public OnePrefabDecalPool(int prewarmCount, bool autoPopulateOnZero)
	{
		count = prewarmCount;
		this.autoPopulateOnZero = autoPopulateOnZero;
		pool = new Queue<Decal>();
	}

	public void Populate(Decal prefab)
	{
		cachedPrefab = prefab;
		for (int i = 0; i < count; i++)
		{
			Decal decal = Object.Instantiate(prefab);
			decal.gameObject.SetActive(value: false);
			pool.Enqueue(decal);
		}
	}

	public Decal Spawn()
	{
		if (pool.Count == 0 && autoPopulateOnZero)
		{
			Populate(cachedPrefab);
		}
		return pool.Dequeue();
	}

	public void Destroy(Decal instance)
	{
		pool.Enqueue(instance);
	}
}
