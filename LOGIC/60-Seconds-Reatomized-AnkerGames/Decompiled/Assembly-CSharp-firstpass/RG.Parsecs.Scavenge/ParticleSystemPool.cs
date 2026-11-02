using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class ParticleSystemPool : MonoBehaviour
{
	public List<ParticleSystemEntry> Entries;

	private bool _initialized;

	private Transform Transform;

	private Dictionary<string, ParticleSystemEntry> _dict = new Dictionary<string, ParticleSystemEntry>();

	public static ParticleSystemPool I { get; private set; }

	public bool Initialized => _initialized;

	private void Awake()
	{
		if (I != null)
		{
			gameObject.SetActive(value: false);
			return;
		}
		I = this;
		Transform = transform;
		Transform.parent = null;
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		StartCoroutine(Initialize(Entries));
	}

	private IEnumerator Initialize(IList<ParticleSystemEntry> entries, Action callback = null)
	{
		int entryI = 0;
		if (entries != null)
		{
			while (entryI < entries.Count)
			{
				for (; entryI < entries.Count; entryI++)
				{
					ParticleSystemEntry entry = entries[entryI];
					Initialize(entry);
				}
				yield return null;
			}
		}
		yield return null;
		_initialized = true;
		callback?.Invoke();
	}

	private void Initialize(ParticleSystemEntry entry, Action callback = null)
	{
		if (_dict.ContainsKey(entry.Name))
		{
			Debug.LogErrorFormat("An element with the same key already exists in the pool: {0}", entry.Name);
			return;
		}
		entry.ResetRuntimeCount();
		_dict.Add(entry.Name, entry);
		entry.pool = new Stack<PoolableParticleSystem>(entry.RuntimeCount);
		PoolableParticleSystem prefab = entry.Prefab;
		if (prefab == null)
		{
			Debug.LogErrorFormat("GameObjectPool.Initialize Prefab is null for: {0}", entry.Name);
			return;
		}
		entry.Name = prefab.name;
		string prefabName = prefab.name;
		for (int i = 0; i < entry.RuntimeCount; i++)
		{
			AddInstanceToEntry(prefabName, prefab, entry);
		}
	}

	private void AddInstanceToEntry(string prefabName, PoolableParticleSystem prefab, ParticleSystemEntry entry)
	{
		PoolableParticleSystem poolableParticleSystem = UnityEngine.Object.Instantiate(prefab);
		poolableParticleSystem.gameObject.name = prefabName;
		PoolObject(prefabName, poolableParticleSystem, entry);
	}

	private void PoolObject(string name, PoolableParticleSystem obj, ParticleSystemEntry poolEntry)
	{
		obj.transform.parent = Transform;
		if (!poolEntry.pool.Contains(obj))
		{
			if (poolEntry.RuntimeCount < 9 && poolEntry.pool.Count >= poolEntry.RuntimeCount)
			{
				poolEntry.RuntimeCount++;
				poolEntry.pool.Push(obj);
			}
			else if (poolEntry.pool.Count > 4 * poolEntry.RuntimeCount)
			{
				UnityEngine.Object.Destroy(obj);
			}
			else
			{
				poolEntry.pool.Push(obj);
			}
		}
	}

	public void PoolObject(string name, PoolableParticleSystem obj)
	{
		if (!string.IsNullOrEmpty(name))
		{
			ParticleSystemEntry entry = GetEntry(name);
			PoolObject(name, obj, entry);
		}
	}

	private ParticleSystemEntry GetEntry(string name)
	{
		ParticleSystemEntry value = null;
		if (!_dict.TryGetValue(name, out value))
		{
			Debug.LogErrorFormat("ParticleSystemPool entry not found for key: {0}", name);
			return null;
		}
		return value;
	}

	public PoolableParticleSystem GetObjectForType(string name, bool onlyPooled = false)
	{
		if (!_initialized)
		{
			Debug.LogErrorFormat("GameObjectPool.GetObjectForType before objectPool initialization. Object: {0}", name);
			return null;
		}
		if (string.IsNullOrEmpty(name))
		{
			Debug.LogError("GetObjectForType name is null of empty");
			return null;
		}
		ParticleSystemEntry entry = GetEntry(name);
		if (entry == null)
		{
			Debug.LogErrorFormat("GameObjectPool.GetObjectForType no pool entry found. Name: {0}", name);
			return null;
		}
		if (entry.Prefab == null)
		{
			Debug.LogErrorFormat("GameObjectPool.GetObjectForType no Pool Entry found, but Prefab is null. Name: {0}", name);
			return null;
		}
		if (entry.pool.Count > 0)
		{
			PoolableParticleSystem poolableParticleSystem = entry.pool.Pop();
			while (poolableParticleSystem == null || poolableParticleSystem.transform == null)
			{
				if (entry.pool.Count > 0)
				{
					Debug.LogErrorFormat("GameObjectPool.GetObjectForType() for {0}, popped a null or destroyed element from the pool. Trying Next.", name);
					poolableParticleSystem = entry.pool.Pop();
					continue;
				}
				Debug.LogErrorFormat("GameObjectPool.GetObjectForType() for {0}, popped a null or destroyed element from the pool. ", name);
				break;
			}
			if (poolableParticleSystem != null)
			{
				poolableParticleSystem.transform.parent = null;
				return poolableParticleSystem;
			}
		}
		if (!onlyPooled)
		{
			PoolableParticleSystem poolableParticleSystem2 = UnityEngine.Object.Instantiate(entry.Prefab);
			poolableParticleSystem2.name += "_not_pooled";
			return poolableParticleSystem2;
		}
		return null;
	}
}
