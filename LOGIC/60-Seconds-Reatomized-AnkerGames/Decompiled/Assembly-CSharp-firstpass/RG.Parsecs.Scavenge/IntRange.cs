using System;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

[Serializable]
public class IntRange
{
	public int Min;

	public int Max;

	public int RandomValue => UnityEngine.Random.Range(Min, Max);

	public IntRange()
	{
	}

	public IntRange(int min, int max)
	{
		Min = min;
		Max = max;
	}

	public static int operator +(int left, IntRange right)
	{
		return left + right.RandomValue;
	}

	public float GetNormalized(float value)
	{
		float num = Max - Min;
		return Mathf.Clamp01((value - (float)Min) / num);
	}

	public float Clamp(int value)
	{
		return Mathf.Clamp(value, Min, Max);
	}

	public float Lerp(float t)
	{
		return Mathf.Lerp(Min, Max, t);
	}

	public bool IsWithin(float value)
	{
		if (value >= (float)Min)
		{
			return value <= (float)Max;
		}
		return false;
	}
}
