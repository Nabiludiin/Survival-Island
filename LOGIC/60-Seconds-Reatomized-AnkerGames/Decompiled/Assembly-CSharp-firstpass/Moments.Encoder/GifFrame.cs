using System;
using UnityEngine;

namespace Moments.Encoder;

public class GifFrame
{
	public int Width;

	public int Height;

	public Color32[] Data;

	public Texture2D Texture;

	internal static GifFrame GetCutFrameSameSizeAs(GifFrame gifFrame)
	{
		GifFrame gifFrame2 = new GifFrame();
		gifFrame2.Width = gifFrame.Width;
		gifFrame2.Height = gifFrame.Height;
		gifFrame2.Data = new Color32[gifFrame.Data.Length];
		byte b = Convert.ToByte(1);
		for (int i = 0; i < gifFrame2.Data.Length; i++)
		{
			gifFrame2.Data[i] = new Color32(b, b, b, Convert.ToByte(255));
		}
		return gifFrame2;
	}

	public void Flush()
	{
		Flush(Texture);
	}

	private void Flush(UnityEngine.Object obj)
	{
		UnityEngine.Object.Destroy(obj);
	}
}
