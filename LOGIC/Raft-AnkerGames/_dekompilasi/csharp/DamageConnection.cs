using System;
using UnityEngine;

[Serializable]
public class DamageConnection
{
	[Range(0f, 1f)]
	public float damagePercent;

	public Mesh mesh;
}
