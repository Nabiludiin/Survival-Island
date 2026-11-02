using System;
using UnityEngine;

namespace RG.Parsecs.Common;

[Serializable]
public struct SRange(float min, float max)
{
	[Range(0f, 1f)]
	[SerializeField]
	private float _min = min;

	[Range(0f, 1f)]
	[SerializeField]
	private float _max = max;

	public float Min => _min;

	public float Max => _max;

	public bool Contains(float value, bool inclusive = true)
	{
		if (inclusive)
		{
			if (value >= Min)
			{
				return value <= Max;
			}
			return false;
		}
		if (value > Min)
		{
			return value <= Max;
		}
		return false;
	}
}
