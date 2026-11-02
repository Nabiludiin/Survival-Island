using System;
using UnityEngine;

namespace UltimateWater.Utils;

[Serializable]
public class MinMaxRange
{
	public float MinValue;

	public float MaxValue;

	public float Random()
	{
		return UnityEngine.Random.Range(MinValue, MaxValue);
	}
}
