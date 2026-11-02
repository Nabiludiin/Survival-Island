using UnityEngine;

namespace UltimateWater.Internal;

public static class TextureUtility
{
	public struct RenderTextureDesc
	{
		public string Name;

		public int Width;

		public int Height;

		public int Depth;

		public int VolumeDepth;

		public int Antialiasing;

		public RenderTextureFormat Format;

		public HideFlags Flags;

		public FilterMode Filter;

		public TextureWrapMode Wrap;

		public RenderTextureReadWrite ColorSpace;

		public bool EnableRandomWrite;

		public bool GenerateMipmaps;

		public bool UseMipmaps;

		public float MipmapBias;

		public RenderTextureDesc(string name)
		{
			Width = 0;
			Height = 0;
			Depth = 0;
			Format = RenderTextureFormat.Default;
			ColorSpace = RenderTextureReadWrite.Linear;
			Name = name;
			VolumeDepth = 0;
			Antialiasing = 1;
			Flags = HideFlags.DontSave;
			Filter = FilterMode.Bilinear;
			Wrap = TextureWrapMode.Clamp;
			EnableRandomWrite = false;
			GenerateMipmaps = false;
			UseMipmaps = false;
			MipmapBias = 0f;
		}

		public RenderTextureDesc(RenderTexture source)
		{
			Name = "RenderTexture";
			Width = source.width;
			Height = source.height;
			Depth = source.depth;
			VolumeDepth = source.volumeDepth;
			Antialiasing = source.antiAliasing;
			Format = source.format;
			Flags = source.hideFlags;
			Filter = source.filterMode;
			Wrap = source.wrapMode;
			ColorSpace = ((!source.sRGB) ? RenderTextureReadWrite.Linear : RenderTextureReadWrite.sRGB);
			EnableRandomWrite = source.enableRandomWrite;
			GenerateMipmaps = source.autoGenerateMips;
			UseMipmaps = source.useMipMap;
			MipmapBias = source.mipMapBias;
		}
	}

	public static void Release(ref RenderTexture texture)
	{
		if (texture != null)
		{
			texture.Release();
			texture.Destroy();
		}
		texture = null;
	}

	public static void Release(ref Texture2D texture)
	{
		if (texture != null)
		{
			texture.Destroy();
		}
		texture = null;
	}

	public static void Swap<T>(ref T left, ref T right)
	{
		T val = left;
		left = right;
		right = val;
	}

	public static void Clear(this RenderTexture texture, bool clearDepth = false, bool clearColor = true)
	{
		texture.Clear(Color.clear, clearDepth, clearColor);
	}

	public static void Clear(this RenderTexture texture, Color color, bool clearDepth = false, bool clearColor = true)
	{
		Graphics.SetRenderTarget(texture);
		GL.Clear(clearDepth, clearColor, color);
		Graphics.SetRenderTarget(null);
	}

	public static void Resize(this RenderTexture texture, int width, int height)
	{
		if (texture == null)
		{
			Debug.LogWarning("Trying to resize null RenderTexture");
		}
		else if (!texture.IsCreated())
		{
			Debug.LogWarning("Trying to resize not created RenderTexture");
		}
		else if (width <= 0 || height <= 0)
		{
			Debug.LogWarning("Trying to resize to invalid(<=0) width or height ");
		}
		else if (texture.width != width || texture.height != height)
		{
			texture.Release();
			texture.width = width;
			texture.height = height;
			texture.Create();
		}
	}

	public static bool Verify(this RenderTexture texture, bool clear = true)
	{
		if (texture == null)
		{
			Debug.LogWarning("Trying to resolve null RenderTexture");
			return false;
		}
		if (texture.IsCreated())
		{
			return false;
		}
		texture.Create();
		if (clear)
		{
			texture.Clear(Color.clear);
		}
		return true;
	}

	public static RenderTexture CreateRenderTexture(this RenderTexture template)
	{
		return template.GetDesc().CreateRenderTexture();
	}

	public static RenderTexture CreateRenderTexture(this RenderTextureDesc desc)
	{
		RenderTexture renderTexture = new RenderTexture(desc.Width, desc.Height, desc.Depth, desc.Format, desc.ColorSpace)
		{
			name = desc.Name,
			volumeDepth = desc.VolumeDepth,
			antiAliasing = desc.Antialiasing,
			hideFlags = desc.Flags,
			filterMode = desc.Filter,
			wrapMode = desc.Wrap,
			autoGenerateMips = desc.GenerateMipmaps,
			useMipMap = desc.UseMipmaps,
			mipMapBias = desc.MipmapBias
		};
		if (desc.EnableRandomWrite)
		{
			renderTexture.Release();
			renderTexture.enableRandomWrite = true;
			renderTexture.Create();
		}
		else
		{
			renderTexture.Create();
		}
		return renderTexture;
	}

	public static RenderTexture CreateTemporary(this RenderTexture template)
	{
		RenderTexture temporary = RenderTexture.GetTemporary(template.width, template.height, template.depth, template.format, (!template.sRGB) ? RenderTextureReadWrite.Linear : RenderTextureReadWrite.sRGB, template.antiAliasing);
		temporary.autoGenerateMips = template.autoGenerateMips;
		temporary.wrapMode = template.wrapMode;
		temporary.filterMode = template.filterMode;
		temporary.volumeDepth = template.volumeDepth;
		temporary.anisoLevel = template.anisoLevel;
		temporary.mipMapBias = template.mipMapBias;
		bool flag = temporary.useMipMap != (bool)template;
		bool flag2 = temporary.enableRandomWrite && !template.enableRandomWrite;
		if ((0u | (flag ? 1u : 0u) | (flag2 ? 1u : 0u)) != 0)
		{
			temporary.Release();
			if (flag)
			{
				temporary.useMipMap = template.useMipMap;
			}
			if (flag2)
			{
				temporary.enableRandomWrite = template.enableRandomWrite;
			}
			temporary.Create();
		}
		temporary.Create();
		return temporary;
	}

	public static void ReleaseTemporary(this RenderTexture texture)
	{
		if (texture != null)
		{
			RenderTexture.ReleaseTemporary(texture);
		}
	}

	public static RenderTextureDesc GetDesc(this RenderTexture source)
	{
		return new RenderTextureDesc(source);
	}
}
