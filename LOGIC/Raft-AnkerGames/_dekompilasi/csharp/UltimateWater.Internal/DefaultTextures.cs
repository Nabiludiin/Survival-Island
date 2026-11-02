using System.Collections.Generic;
using UnityEngine;

namespace UltimateWater.Internal;

public class DefaultTextures : ApplicationSingleton<DefaultTextures>
{
	private static readonly Dictionary<Color, Texture2D> _Cache = new Dictionary<Color, Texture2D>();

	public static Texture2D Get(Color color)
	{
		if (ApplicationSingleton<DefaultTextures>.Instance == null)
		{
			return null;
		}
		if (_Cache.TryGetValue(color, out var value))
		{
			return value;
		}
		Color color2 = color;
		value = CreateColorTexure(color, "[UWS] DefaultTextures - " + color2.ToString());
		_Cache.Add(color, value);
		return value;
	}

	protected override void OnDestroy()
	{
		foreach (KeyValuePair<Color, Texture2D> item in _Cache)
		{
			item.Value.Destroy();
		}
		_Cache.Clear();
		base.OnDestroy();
	}

	private static Texture2D CreateColorTexure(Color color, string name)
	{
		Texture2D texture2D = new Texture2D(2, 2, TextureFormat.ARGB32, mipChain: false);
		texture2D.name = name;
		texture2D.hideFlags = HideFlags.DontSave;
		texture2D.SetPixel(0, 0, color);
		texture2D.SetPixel(1, 0, color);
		texture2D.SetPixel(0, 1, color);
		texture2D.SetPixel(1, 1, color);
		texture2D.Apply(updateMipmaps: false, makeNoLongerReadable: true);
		return texture2D;
	}
}
