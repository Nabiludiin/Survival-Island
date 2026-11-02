using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ScreenEffect_Image : ScreenEffect
{
	public Image image;

	public void SetAlpha(float alpha)
	{
		if (image != null)
		{
			Color color = image.color;
			color.a = alpha;
			image.color = color;
		}
	}

	protected override void Tick()
	{
		base.Tick();
		SetAlpha(alpha);
	}
}
