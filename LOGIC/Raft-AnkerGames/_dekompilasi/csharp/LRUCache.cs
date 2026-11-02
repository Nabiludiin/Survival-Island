using System;
using System.Collections.Generic;

public class LRUCache<TKey, TValue>
{
	private readonly int capacity;

	private readonly Dictionary<TKey, LinkedListNode<KeyValuePair<TKey, TValue>>> cacheMap;

	private readonly LinkedList<KeyValuePair<TKey, TValue>> lruList;

	public LRUCache(int capacity)
	{
		if (capacity <= 0)
		{
			throw new ArgumentException("Capacity must be greater than zero.");
		}
		this.capacity = capacity;
		cacheMap = new Dictionary<TKey, LinkedListNode<KeyValuePair<TKey, TValue>>>(capacity);
		lruList = new LinkedList<KeyValuePair<TKey, TValue>>();
	}

	public void Add(TKey key, TValue value)
	{
		if (cacheMap.Count >= capacity)
		{
			EvictLeastRecentlyUsed();
		}
		if (cacheMap.ContainsKey(key))
		{
			lruList.Remove(cacheMap[key]);
		}
		LinkedListNode<KeyValuePair<TKey, TValue>> linkedListNode = new LinkedListNode<KeyValuePair<TKey, TValue>>(new KeyValuePair<TKey, TValue>(key, value));
		lruList.AddLast(linkedListNode);
		cacheMap[key] = linkedListNode;
	}

	public bool Get(TKey key, out TValue value)
	{
		if (!cacheMap.ContainsKey(key))
		{
			value = default;
			return false;
		}
		LinkedListNode<KeyValuePair<TKey, TValue>> linkedListNode = cacheMap[key];
		lruList.Remove(linkedListNode);
		lruList.AddLast(linkedListNode);
		value = linkedListNode.Value.Value;
		return true;
	}

	private void EvictLeastRecentlyUsed()
	{
		LinkedListNode<KeyValuePair<TKey, TValue>> first = lruList.First;
		cacheMap.Remove(first.Value.Key);
		lruList.RemoveFirst();
	}
}
