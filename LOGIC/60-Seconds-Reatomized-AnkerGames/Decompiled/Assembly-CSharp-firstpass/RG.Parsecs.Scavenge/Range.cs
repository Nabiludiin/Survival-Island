using System;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

[Serializable]
public class Range
{
	public float Min;

	public float Max;

	public float RandomValue => UnityEngine.Random.Range(Min, Max);

	public Range()
	{
	}

	public Range(float min, float max)
	{
		Min = min;
		Max = max;
	}

	public void Init(float min, float max)
	{
		Min = min;
		Max = max;
	}

	public static float operator +(float left, Range right)
	{
		return left + right.RandomValue;
	}

	public float GetNormalized(float value)
	{
		float num = Max - Min;
		return Mathf.Clamp01((value - Min) / num);
	}

	public float Clamp(float value)
	{
		return Mathf.Clamp(value, Min, Max);
	}

	public float Lerp(float t)
	{
		return Mathf.Lerp(Min, Max, t);
	}

	public bool IsWithin(float value)
	{
		if (value >= Min)
		{
			return value <= Max;
		}
		return false;
	}
}
