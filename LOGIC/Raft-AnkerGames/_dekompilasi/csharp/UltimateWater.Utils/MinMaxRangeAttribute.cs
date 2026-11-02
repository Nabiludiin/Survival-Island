using UnityEngine;

namespace UltimateWater.Utils;

public class MinMaxRangeAttribute : PropertyAttribute
{
	public float MinValue;

	public float MaxValue;

	public MinMaxRangeAttribute(float minValue, float maxValue)
	{
		MinValue = minValue;
		MaxValue = maxValue;
	}
}
