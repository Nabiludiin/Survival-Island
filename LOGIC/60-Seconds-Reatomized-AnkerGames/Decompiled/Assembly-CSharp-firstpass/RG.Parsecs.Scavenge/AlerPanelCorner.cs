using System;
using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Scavenge;

[Serializable]
public class AlerPanelCorner
{
	public Image Image;

	public Animation Animation;

	public void Stop()
	{
		Animation.Stop();
		Color color = Image.color;
		color.a = 0f;
		Image.color = color;
	}
}
