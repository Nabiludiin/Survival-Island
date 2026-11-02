using System;
using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public static class RandomCollectionOperations
{
	public static void Shuffle<T>(this IList<T> list)
	{
		int num = list.Count;
		while (num > 1)
		{
			int index = UnityEngine.Random.Range(0, num) % num;
			num--;
			T value = list[index];
			list[index] = list[num];
			list[num] = value;
		}
	}

	public static T RandomElement<T>(this T[] array)
	{
		return array[UnityEngine.Random.Range(0, array.Length)];
	}

	public static T RandomElement<T>(this T[] array, int seed, out int index)
	{
		if (seed >= 0)
		{
			index = seed;
			return array[seed];
		}
		index = UnityEngine.Random.Range(0, array.Length);
		return array[index];
	}

	public static T RandomElement<T>(this IList<T> array)
	{
		return array[UnityEngine.Random.Range(0, array.Count)];
	}

	public static T LastElement<T>(this IList<T> array)
	{
		return array[array.Count - 1];
	}

	public static T MaxElement<T>(this T[] array, Func<T, float?> getValue) where T : class
	{
		float num = float.MinValue;
		int num2 = -1;
		for (int i = 0; i < array.Length; i++)
		{
			T arg = array[i];
			float? num3 = getValue(arg);
			if (num3.HasValue && num3.Value > num)
			{
				num = num3.Value;
				num2 = i;
			}
		}
		if (num2 < 0)
		{
			return null;
		}
		return array[num2];
	}

	public static T MaxElement<T>(this IList<T> array, Func<T, float?> getValue) where T : class
	{
		float num = float.MinValue;
		int num2 = -1;
		for (int i = 0; i < array.Count; i++)
		{
			T arg = array[i];
			float? num3 = getValue(arg);
			if (num3.HasValue && num3.Value > num)
			{
				num = num3.Value;
				num2 = i;
			}
		}
		if (num2 < 0)
		{
			return null;
		}
		return array[num2];
	}

	public static T MinElement<T>(this IList<T> array, Func<T, float?> getValue) where T : class
	{
		float num = float.MaxValue;
		int num2 = -1;
		for (int i = 0; i < array.Count; i++)
		{
			T arg = array[i];
			float? num3 = getValue(arg);
			if (num3.HasValue && num3.Value < num)
			{
				num = num3.Value;
				num2 = i;
			}
		}
		if (num2 < 0)
		{
			return null;
		}
		return array[num2];
	}
}
