using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArrayRandomizer : MonoBehaviour
{
	public static List<int> GenerateRandom(int count, int min, int max)
	{
		if (max <= min || count < 0 || (count > max - min && max - min > 0))
		{
			Debug.Log("Range " + min + " to " + max + " (" + (max - min) + " values), or count " + count + " is illegal");
		}
		HashSet<int> hashSet = new HashSet<int>();
		for (int i = max - count; i < max; i++)
		{
			if (!hashSet.Add(Random.Range(min, i + 1)))
			{
				hashSet.Add(i);
			}
		}
		List<int> list = hashSet.ToList();
		for (int num = list.Count - 1; num > 0; num--)
		{
			int index = Random.Range(0, num + 1);
			int value = list[index];
			list[index] = list[num];
			list[num] = value;
		}
		return list;
	}

	public static List<int> GenerateRandom(int count)
	{
		return GenerateRandom(count, 0, int.MaxValue);
	}
}
