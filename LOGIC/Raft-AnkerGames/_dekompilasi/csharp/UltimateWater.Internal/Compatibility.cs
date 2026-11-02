using System.Collections.Generic;
using UltimateWater.Utils;
using UnityEngine;

namespace UltimateWater.Internal;

public class Compatibility
{
	static Compatibility()
	{
		if (PlayerPrefs.GetInt("[Ultimate Water System]: compatibility check") != Versioning.Number)
		{
			CheckFormats();
			PlayerPrefs.SetInt("[Ultimate Water System]: compatibility check", Versioning.Number);
		}
	}

	public static RenderTextureFormat? GetFormat(RenderTextureFormat preferred, IEnumerable<RenderTextureFormat> fallback = null)
	{
		if (IsFormatSupported(preferred))
		{
			return preferred;
		}
		if (fallback == null)
		{
			WaterLogger.Error("Compatibility", "GetFormat", "preferred format not supported, and no fallback formats available for :" + preferred);
			return null;
		}
		foreach (RenderTextureFormat item in fallback)
		{
			if (SystemInfo.SupportsRenderTextureFormat(item))
			{
				WaterLogger.Warning("Compatibility", "GetFormat", "preferred format not supported, chosen fallback: " + item);
				return item;
			}
		}
		return null;
	}

	private static void CheckFormats()
	{
		RenderTextureFormat[] array = new RenderTextureFormat[8]
		{
			RenderTextureFormat.ARGBFloat,
			RenderTextureFormat.ARGBHalf,
			RenderTextureFormat.ARGB32,
			RenderTextureFormat.RGHalf,
			RenderTextureFormat.RFloat,
			RenderTextureFormat.RHalf,
			RenderTextureFormat.R8,
			RenderTextureFormat.Depth
		};
		bool flag = true;
		for (int i = 0; i < array.Length; i++)
		{
			RenderTextureFormat format = array[i];
			if (!IsFormatSupported(format))
			{
				WaterLogger.Info("Compatibility", "CheckFormats", "RenderTexture format not supported: " + format);
				flag = false;
			}
		}
		if (flag)
		{
			WaterLogger.Info("Compatibility", "CheckFormats", "all necessary RenderTexture formats supported");
		}
		else
		{
			WaterLogger.Warning("Compatibility", "CheckFormats", "some of the necessary render texture formats not supported, \nsome features will not be available");
		}
	}

	private static bool IsFormatSupported(RenderTextureFormat format)
	{
		return SystemInfo.SupportsRenderTextureFormat(format);
	}
}
