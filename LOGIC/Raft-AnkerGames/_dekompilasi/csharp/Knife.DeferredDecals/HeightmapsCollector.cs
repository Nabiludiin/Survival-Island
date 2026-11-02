using UnityEngine;

namespace Knife.DeferredDecals;

public class HeightmapsCollector : Texture2DArrayComposer
{
	public HeightmapsCollector(int sizeX, int sizeY)
		: base(sizeX, sizeY, TextureFormat.RFloat, bypassSrgb: true)
	{
		alwaysGenerateOnUpdate = true;
	}
}
