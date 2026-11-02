using System;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

[Serializable]
public class Range2
{
	public Vector2 Min;

	public Vector2 Max;

	public Vector2 RandomValue => new Vector2(UnityEngine.Random.Range(Min.x, Max.x), UnityEngine.Random.Range(Min.y, Max.y));

	public static Vector2 operator +(Vector2 left, Range2 right)
	{
		return left + right.RandomValue;
	}
}
