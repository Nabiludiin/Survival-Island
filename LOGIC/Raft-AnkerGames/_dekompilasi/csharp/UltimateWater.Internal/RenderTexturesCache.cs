using System.Collections.Generic;
using UnityEngine;

namespace UltimateWater.Internal;

public class RenderTexturesCache
{
	[ExecuteInEditMode]
	public class RenderTexturesUpdater : MonoBehaviour
	{
		private static RenderTexturesUpdater _Instance;

		public static void EnsureInstance()
		{
			if (!(_Instance != null))
			{
				GameObject gameObject = new GameObject("Water.RenderTexturesCache")
				{
					hideFlags = HideFlags.HideAndDontSave
				};
				if (Application.isPlaying)
				{
					Object.DontDestroyOnLoad(gameObject);
				}
				_Instance = gameObject.AddComponent<RenderTexturesUpdater>();
			}
		}

		private void Update()
		{
			int frameCount = Time.frameCount;
			Dictionary<ulong, RenderTexturesCache>.Enumerator enumerator = _Cache.GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.Value.Update(frameCount);
			}
			enumerator.Dispose();
		}

		private void OnApplicationQuit()
		{
			Dictionary<ulong, RenderTexturesCache>.Enumerator enumerator = _Cache.GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.Value.Release();
			}
			enumerator.Dispose();
		}
	}

	private static readonly Dictionary<ulong, RenderTexturesCache> _Cache = new Dictionary<ulong, RenderTexturesCache>(UInt64EqualityComparer.Default);

	private readonly Queue<RenderTexture> _RenderTextures;

	private int _LastFrameAllUsed;

	private readonly ulong _Hash;

	private readonly int _Width;

	private readonly int _Height;

	private readonly int _DepthBuffer;

	private readonly RenderTextureFormat _Format;

	private readonly bool _Linear;

	private readonly bool _Uav;

	private readonly bool _MipMaps;

	public RenderTexturesCache(ulong hash, int width, int height, int depthBuffer, RenderTextureFormat format, bool linear, bool uav, bool mipMaps)
	{
		_Hash = hash;
		_Width = width;
		_Height = height;
		_DepthBuffer = depthBuffer;
		_Format = format;
		_Linear = linear;
		_Uav = uav;
		_MipMaps = mipMaps;
		_RenderTextures = new Queue<RenderTexture>();
	}

	public static RenderTexturesCache GetCache(int width, int height, int depthBuffer, RenderTextureFormat format, bool linear, bool uav, bool mipMaps = false)
	{
		RenderTexturesUpdater.EnsureInstance();
		ulong num = 0uL;
		num |= (uint)width;
		num |= (uint)(height << 16);
		num |= (ulong)((long)depthBuffer << 29);
		num |= (ulong)((long)(linear ? 1 : 0) << 34);
		num |= (ulong)((long)(uav ? 1 : 0) << 35);
		num |= (ulong)((long)(mipMaps ? 1 : 0) << 36);
		num |= (ulong)((long)format << 37);
		if (!_Cache.TryGetValue(num, out var value))
		{
			value = (_Cache[num] = new RenderTexturesCache(num, width, height, depthBuffer, format, linear, uav, mipMaps));
		}
		return value;
	}

	public static TemporaryRenderTexture GetTemporary(int width, int height, int depthBuffer, RenderTextureFormat format, bool linear, bool uav, bool mipMaps = false)
	{
		return GetCache(width, height, depthBuffer, format, linear, uav, mipMaps).GetTemporary();
	}

	public TemporaryRenderTexture GetTemporary()
	{
		return new TemporaryRenderTexture(this);
	}

	public RenderTexture GetTemporaryDirect()
	{
		RenderTexture renderTexture;
		if (_RenderTextures.Count == 0)
		{
			renderTexture = new RenderTexture(_Width, _Height, _DepthBuffer, _Format, _Linear ? RenderTextureReadWrite.Linear : RenderTextureReadWrite.sRGB);
			renderTexture.hideFlags = HideFlags.DontSave;
			renderTexture.name = "[UWS] RenderTexturesCache - Temporary#" + _Hash;
			renderTexture.filterMode = FilterMode.Point;
			renderTexture.anisoLevel = 1;
			renderTexture.wrapMode = TextureWrapMode.Repeat;
			renderTexture.useMipMap = _MipMaps;
			renderTexture.autoGenerateMips = _MipMaps;
			if (_Uav)
			{
				renderTexture.enableRandomWrite = true;
			}
		}
		else
		{
			renderTexture = _RenderTextures.Dequeue();
		}
		if (_Uav && !renderTexture.IsCreated())
		{
			renderTexture.Create();
		}
		if (_RenderTextures.Count == 0)
		{
			_LastFrameAllUsed = Time.frameCount;
		}
		return renderTexture;
	}

	public void ReleaseTemporaryDirect(RenderTexture renderTexture)
	{
		_RenderTextures.Enqueue(renderTexture);
	}

	internal void Update(int frame)
	{
		if (frame - _LastFrameAllUsed > 3 && _RenderTextures.Count != 0)
		{
			_RenderTextures.Dequeue().Destroy();
		}
	}

	internal void Release()
	{
		foreach (RenderTexture renderTexture in _RenderTextures)
		{
			if (renderTexture != null)
			{
				renderTexture.Release();
				renderTexture.Destroy();
			}
		}
		_RenderTextures.Clear();
	}
}
