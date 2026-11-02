using System;
using UnityEngine;

[Serializable]
public class Interval_Int : Interval<int>
{
	public override int GetRandomValue()
	{
		return UnityEngine.Random.Range(minValue, maxValue + 1);
	}

	public override int GetValueFromNormalized(float normalizedValue)
	{
		int num = maxValue - minValue;
		return minValue + Mathf.RoundToInt((float)num * normalizedValue);
	}

	public override bool IsInsideOfIntervalInclusive(int value)
	{
		if (value >= minValue)
		{
			return value <= maxValue;
		}
		return false;
	}

	public override bool IsInsideOfIntervalExclusive(int value)
	{
		if (value > minValue)
		{
			return value < maxValue;
		}
		return false;
	}
}
