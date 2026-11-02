using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Randomizer
{
	[SerializeField]
	private UnityEngine.Object componentType;

	public RandomItem[] items;

	public uint lastRandomizedIndex;

	[SerializeField]
	private float totalWeight;

	public float TotalWeight
	{
		get
		{
			float num = 0f;
			if (items != null && items.Length != 0)
			{
				RandomItem[] array = items;
				foreach (RandomItem randomItem in array)
				{
					num += randomItem.weight;
				}
			}
			return num;
		}
	}

	public bool HasItems
	{
		get
		{
			if (items != null)
			{
				return items.Length != 0;
			}
			return false;
		}
	}

	private void InspectorGUI()
	{
		float num = TotalWeight;
		if (num != 0f)
		{
			RandomItem[] array = items;
			foreach (RandomItem randomItem in array)
			{
				randomItem.spawnChance = randomItem.weight / num * 100f + "%";
				if (!(componentType != null) || !(randomItem.obj != null))
				{
					continue;
				}
				Type type = Type.GetType(componentType.name);
				if (randomItem.obj is GameObject)
				{
					GameObject gameObject = (GameObject)randomItem.obj;
					if (gameObject != null)
					{
						randomItem.obj = gameObject.GetComponent(type);
					}
				}
			}
		}
		totalWeight = num;
	}

	public T[] GetAllItems<T>()
	{
		List<T> list = new List<T>();
		for (int i = 0; i < items.Length; i++)
		{
			RandomItem randomItem = items[i];
			if (randomItem == null || !(randomItem.obj != null))
			{
				continue;
			}
			if (randomItem.obj is SO_RandomizerCategory)
			{
				SO_RandomizerCategory sO_RandomizerCategory = (SO_RandomizerCategory)randomItem.obj;
				if (sO_RandomizerCategory != null)
				{
					list.AddRange(sO_RandomizerCategory.randomizer.GetAllItems<T>());
				}
			}
			else if (randomItem.obj is T)
			{
				list.Add((T)(object)randomItem.obj);
			}
		}
		return list.ToArray();
	}

	public T[] GetRandomItems<T>(int amountOfItems)
	{
		T[] array = new T[amountOfItems];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = GetRandomItem<T>();
		}
		return array;
	}

	public T GetRandomItem<T>()
	{
		float maxInclusive = TotalWeight;
		float randomValue = UnityEngine.Random.Range(0f, maxInclusive);
		return GetRandomItem<T>(randomValue);
	}

	public T GetRandomItem<T>(System.Random randomGenerator)
	{
		float num = TotalWeight;
		float randomValue = (float)(randomGenerator.NextDouble() * (double)num);
		return GetRandomItem<T>(randomValue);
	}

	public T GetRandomItem<T>(float randomValue)
	{
		float num = 0f;
		for (uint num2 = 0u; num2 < items.Length; num2++)
		{
			RandomItem randomItem = items[num2];
			num += randomItem.weight;
			if (!(randomValue < num))
			{
				continue;
			}
			if (randomItem.obj == null)
			{
				return default;
			}
			if (randomItem.obj is SO_RandomizerCategory)
			{
				SO_RandomizerCategory sO_RandomizerCategory = (SO_RandomizerCategory)randomItem.obj;
				if (!(sO_RandomizerCategory == null))
				{
					return sO_RandomizerCategory.randomizer.GetRandomItem<T>();
				}
				Debug.Log("Category was null");
			}
			if (randomItem.obj is T)
			{
				lastRandomizedIndex = num2;
				return (T)(object)randomItem.obj;
			}
		}
		return default;
	}

	public T GetItemByIndex<T>(uint index)
	{
		if (index < 0 || index >= items.Length)
		{
			return default;
		}
		return (T)(object)items[index].obj;
	}

	public int GetIndexFromItem<T>(T item)
	{
		if (item == null)
		{
			return -1;
		}
		for (int i = 0; i < items.Length; i++)
		{
			RandomItem randomItem = items[i];
			if (randomItem != null && randomItem.obj is T && item.Equals((T)(object)randomItem.obj))
			{
				return i;
			}
		}
		return -1;
	}

	public int GetItemCount()
	{
		if (items == null)
		{
			return 0;
		}
		return items.Length;
	}
}
