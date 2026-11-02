using System;
using UnityEngine;

namespace UltimateWater.Internal;

public struct TemporaryRenderTexture : IDisposable
{
	private RenderTexture _RenderTexture;

	private readonly RenderTexturesCache _Cache;

	public RenderTexture Texture => _RenderTexture;

	public void Dispose()
	{
		if (!(_RenderTexture == null))
		{
			_Cache.ReleaseTemporaryDirect(_RenderTexture);
			_RenderTexture = null;
		}
	}

	public static implicit operator RenderTexture(TemporaryRenderTexture that)
	{
		return that.Texture;
	}

	internal TemporaryRenderTexture(RenderTexturesCache renderTexturesCache)
	{
		_Cache = renderTexturesCache;
		_RenderTexture = renderTexturesCache.GetTemporaryDirect();
	}
}
