using System;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

[Serializable]
public class Range3
{
	public Vector3 Min;

	public Vector3 Max;

	public Vector3 RandomValue => new Vector3(UnityEngine.Random.Range(Min.x, Max.x), UnityEngine.Random.Range(Min.y, Max.y), UnityEngine.Random.Range(Min.z, Max.z));

	public static Vector3 operator +(Vector3 left, Range3 right)
	{
		return left + right.RandomValue;
	}
}
