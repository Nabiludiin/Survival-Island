using System;
using UnityEngine;

[Serializable]
public class Interval_Float : Interval<float>
{
	public float Median => (maxValue + minValue) / 2f;

	public Interval_Float(float maxValue, float minValue)
	{
		base.maxValue = maxValue;
		base.minValue = minValue;
	}

	public override float GetRandomValue()
	{
		return UnityEngine.Random.Range(minValue, maxValue);
	}

	public override float GetValueFromNormalized(float normalizedValue)
	{
		float num = maxValue - minValue;
		return minValue + num * normalizedValue;
	}

	public override bool IsInsideOfIntervalInclusive(float value)
	{
		if (value >= minValue)
		{
			return value <= maxValue;
		}
		return false;
	}

	public override bool IsInsideOfIntervalExclusive(float value)
	{
		if (value > minValue)
		{
			return value < maxValue;
		}
		return false;
	}
}
