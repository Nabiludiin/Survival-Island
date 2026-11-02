using System;
using UnityEngine;

[Serializable]
public class SplatTexture
{
	public string name;

	public int textureIndex;

	public Vector2 interval;

	public float GetAmountFromNormal(float normal, float offset)
	{
		float num = interval.x - offset;
		float num2 = interval.y + offset;
		float num3 = interval.x + offset;
		float num4 = interval.y - offset;
		if (interval.x == 0f)
		{
			num = (num3 = 0f);
		}
		else if (interval.y == 1f)
		{
			num2 = (num4 = 1f);
		}
		if (normal >= num3 && normal <= num4)
		{
			return 1f;
		}
		if (normal < num3)
		{
			float num5 = num3 - num;
			return Mathf.Clamp01(Mathf.Clamp01(normal - num) / num5);
		}
		if (normal > num4)
		{
			float num6 = num2 - num4;
			float num7 = Mathf.Clamp01(normal - num4);
			return Mathf.Clamp01(1f - num7 / num6);
		}
		return 0f;
	}
}
