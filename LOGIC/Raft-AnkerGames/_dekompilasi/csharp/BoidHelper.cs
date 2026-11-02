using System;
using UnityEngine;

public static class BoidHelper
{
	private const int numViewDirections = 300;

	public static readonly Vector3[] directions;

	static BoidHelper()
	{
		directions = new Vector3[300];
		float num = (1f + Mathf.Sqrt(5f)) / 2f;
		float num2 = MathF.PI * 2f * num;
		for (int i = 0; i < 300; i++)
		{
			float num3 = (float)i / 300f;
			float f = Mathf.Acos(1f - 2f * num3);
			float f2 = num2 * (float)i;
			float x = Mathf.Sin(f) * Mathf.Cos(f2);
			float y = Mathf.Sin(f) * Mathf.Sin(f2);
			float z = Mathf.Cos(f);
			directions[i] = new Vector3(x, y, z);
		}
	}
}
