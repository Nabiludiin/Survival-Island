using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Knife.DeferredDecals.Spawn;

public class MultiPrefabDecalPool : IMultiPrefabPool<Decal>, ISpawner<Decal>
{
	private List<Queue<Decal>> pools;

	private Dictionary<Decal, int> instancies;

	private readonly int count;

	private readonly bool autoPopulateOnZero;

	private Decal[] cachedPrefabs;

	public MultiPrefabDecalPool(int prewarmCount, bool autoPopulateOnZero)
	{
		count = prewarmCount;
		this.autoPopulateOnZero = autoPopulateOnZero;
		pools = new List<Queue<Decal>>();
		instancies = new Dictionary<Decal, int>();
	}

	public void Populate(IEnumerable<Decal> prefabs)
	{
		cachedPrefabs = prefabs.ToArray();
		IEnumerator<Decal> enumerator = prefabs.GetEnumerator();
		int num = 0;
		while (enumerator.MoveNext())
		{
			pools.Add(new Queue<Decal>());
			Decal current = enumerator.Current;
			for (int i = 0; i < count; i++)
			{
				Decal decal = Object.Instantiate(current);
				decal.gameObject.SetActive(value: false);
				instancies.Add(decal, num);
				pools[num].Enqueue(decal);
			}
			num++;
		}
	}

	private void Populate(int poolIndex)
	{
		for (int i = 0; i < count; i++)
		{
			Decal decal = Object.Instantiate(cachedPrefabs[i]);
			decal.gameObject.SetActive(value: false);
			instancies.Add(decal, poolIndex);
			pools[poolIndex].Enqueue(decal);
		}
	}

	public Decal Spawn()
	{
		int num = Random.Range(0, pools.Count);
		Queue<Decal> queue = pools[num];
		if (queue.Count == 0 && autoPopulateOnZero)
		{
			Populate(num);
		}
		return queue.Dequeue();
	}

	public void Destroy(Decal instance)
	{
		instance.gameObject.SetActive(value: false);
		pools[instancies[instance]].Enqueue(instance);
	}
}
