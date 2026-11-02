using System.Collections.Generic;
using UnityEngine;

namespace UltimateWater.Internal;

public class DynamicWaterCameraData
{
	private enum TextureTypes
	{
		Displacement,
		DisplacementMask,
		Normal,
		Foam,
		FoamPrevious,
		Diffuse,
		Debug,
		TotalDisplacement
	}

	internal int _LastFrameUsed;

	private bool _TotalDisplacementMapDirty;

	private readonly int _Antialiasing;

	private readonly bool _OnlyColor;

	private readonly DynamicWater _DynamicWater;

	private readonly Dictionary<int, RenderTexture> _Textures = new Dictionary<int, RenderTexture>();

	public bool _DiffuseContentWritten = true;

	public bool _FoamContentWritten = true;

	private RenderTextureFormat _DisplacementFormat;

	private RenderTextureFormat _NormalFormat;

	private RenderTextureFormat _FoamFormat;

	private RenderTextureFormat _DiffuseFormat;

	private RenderTextureFormat _TotalDisplacementFormat;

	public RenderTexture DynamicDisplacementMap => _Textures[0];

	public RenderTexture NormalMap => _Textures[2];

	public RenderTexture FoamMap => _Textures[3];

	public RenderTexture FoamMapPrevious => _Textures[4];

	public RenderTexture DisplacementsMask => _Textures[1];

	public RenderTexture DiffuseMap => _Textures[5];

	public DynamicWater DynamicWater => _DynamicWater;

	public RenderTexture TotalDisplacementMap
	{
		get
		{
			RenderTexture renderTexture = _Textures[7];
			if (!_TotalDisplacementMapDirty)
			{
				return renderTexture;
			}
			_DynamicWater.RenderTotalDisplacementMap(Camera, renderTexture);
			_TotalDisplacementMapDirty = false;
			return renderTexture;
		}
	}

	public WaterCamera Camera { get; private set; }

	public DynamicWaterCameraData(DynamicWater dynamicWater, WaterCamera camera, int antialiasing, bool onlyColor)
	{
		ResolveFormats();
		Camera = camera;
		_DynamicWater = dynamicWater;
		_Antialiasing = antialiasing;
		_OnlyColor = onlyColor;
		camera.RenderTargetResized += Camera_RenderTargetResized;
		CreateRenderTargets();
	}

	public RenderTexture GetDebugMap(bool createIfNotExists = false)
	{
		if (!_Textures.ContainsKey(6))
		{
			if (!createIfNotExists)
			{
				return null;
			}
			_Textures.Add(6, DynamicDisplacementMap.CreateRenderTexture());
			_Textures[6].name = "[UWS] DynamicWaterCameraData - Debug";
			return _Textures[6];
		}
		return _Textures[6];
	}

	public RenderTexture GetTotalDisplacementMap()
	{
		return TotalDisplacementMap;
	}

	public void Dispose()
	{
		Camera.RenderTargetResized -= Camera_RenderTargetResized;
		DisposeTextures();
	}

	public void ClearOverlays()
	{
		ValidateRTs();
		Dictionary<int, RenderTexture>.Enumerator enumerator = _Textures.GetEnumerator();
		while (enumerator.MoveNext())
		{
			KeyValuePair<int, RenderTexture> current = enumerator.Current;
			if (current.Key != 5 && current.Key != 3 && current.Key != 4)
			{
				Graphics.SetRenderTarget(current.Value);
				GL.Clear(clearDepth: false, clearColor: true, (current.Key == 1) ? Color.white : Color.clear);
			}
		}
		enumerator.Dispose();
		Graphics.SetRenderTarget(null);
		_TotalDisplacementMapDirty = true;
	}

	public void ClearFoam()
	{
		Graphics.SetRenderTarget(_Textures[3]);
		GL.Clear(clearDepth: false, clearColor: true, Color.clear);
	}

	public void ValidateRTs()
	{
		Dictionary<int, RenderTexture>.Enumerator enumerator = _Textures.GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.Value.Verify();
		}
		enumerator.Dispose();
	}

	public void SwapFoamMaps()
	{
		RenderTexture value = _Textures[4];
		_Textures[4] = _Textures[3];
		_Textures[3] = value;
	}

	private void DisposeTextures()
	{
		foreach (KeyValuePair<int, RenderTexture> texture in _Textures)
		{
			texture.Value.Destroy();
		}
		_Textures.Clear();
	}

	private void Camera_RenderTargetResized(WaterCamera camera)
	{
		CreateRenderTargets();
	}

	private void CreateRenderTargets()
	{
		DisposeTextures();
		int width = Mathf.RoundToInt(Camera.CameraComponent.pixelWidth);
		int height = Mathf.RoundToInt(Camera.CameraComponent.pixelHeight);
		TextureUtility.RenderTextureDesc renderTextureDesc = new TextureUtility.RenderTextureDesc("[UWS] DynamicWaterCameraData - Displacement");
		renderTextureDesc.Width = width;
		renderTextureDesc.Height = height;
		renderTextureDesc.Antialiasing = _Antialiasing;
		renderTextureDesc.Format = _DisplacementFormat;
		TextureUtility.RenderTextureDesc desc = renderTextureDesc;
		renderTextureDesc = new TextureUtility.RenderTextureDesc("[UWS] DynamicWaterCameraData - Normals");
		renderTextureDesc.Width = width;
		renderTextureDesc.Height = height;
		renderTextureDesc.Antialiasing = _Antialiasing;
		renderTextureDesc.Format = _NormalFormat;
		TextureUtility.RenderTextureDesc desc2 = renderTextureDesc;
		renderTextureDesc = new TextureUtility.RenderTextureDesc("[UWS] DynamicWaterCameraData - Foam");
		renderTextureDesc.Width = width;
		renderTextureDesc.Height = height;
		renderTextureDesc.Antialiasing = _Antialiasing;
		renderTextureDesc.Format = _FoamFormat;
		TextureUtility.RenderTextureDesc desc3 = renderTextureDesc;
		renderTextureDesc = new TextureUtility.RenderTextureDesc("[UWS] DynamicWaterCameraData - Diffuse");
		renderTextureDesc.Width = width;
		renderTextureDesc.Height = height;
		renderTextureDesc.Antialiasing = _Antialiasing;
		renderTextureDesc.Format = _DiffuseFormat;
		TextureUtility.RenderTextureDesc desc4 = renderTextureDesc;
		renderTextureDesc = new TextureUtility.RenderTextureDesc("[UWS] DynamicWaterCameraData - Total Displacement");
		renderTextureDesc.Width = 256;
		renderTextureDesc.Height = 256;
		renderTextureDesc.Antialiasing = 1;
		renderTextureDesc.Format = _TotalDisplacementFormat;
		TextureUtility.RenderTextureDesc desc5 = renderTextureDesc;
		_Textures.Add(5, desc4.CreateRenderTexture());
		_Textures.Add(3, desc3.CreateRenderTexture());
		_Textures.Add(4, desc3.CreateRenderTexture());
		if (!_OnlyColor)
		{
			_Textures.Add(0, desc.CreateRenderTexture());
			_Textures.Add(1, desc.CreateRenderTexture());
			_Textures.Add(2, desc2.CreateRenderTexture());
		}
		_Textures.Add(7, desc5.CreateRenderTexture());
	}

	private void ResolveFormats()
	{
		RenderTextureFormat? format = Compatibility.GetFormat(RenderTextureFormat.ARGBHalf, new RenderTextureFormat[1] { RenderTextureFormat.ARGBFloat });
		SetFormat(format, ref _DisplacementFormat);
		SetFormat(format, ref _TotalDisplacementFormat);
		format = Compatibility.GetFormat(RenderTextureFormat.RGHalf, new RenderTextureFormat[1] { RenderTextureFormat.RGFloat });
		SetFormat(format, ref _NormalFormat);
		format = Compatibility.GetFormat(RenderTextureFormat.RHalf, new RenderTextureFormat[1] { RenderTextureFormat.RFloat });
		SetFormat(format, ref _FoamFormat);
		format = Compatibility.GetFormat(RenderTextureFormat.ARGB32, new RenderTextureFormat[2]
		{
			RenderTextureFormat.ARGBHalf,
			RenderTextureFormat.ARGBFloat
		});
		SetFormat(format, ref _DiffuseFormat);
	}

	private void SetFormat(RenderTextureFormat? src, ref RenderTextureFormat dest)
	{
		if (!src.HasValue)
		{
			Debug.LogError("Target device does not support DynamicWaterEffects texture formats");
		}
		else
		{
			dest = src.Value;
		}
	}
}
