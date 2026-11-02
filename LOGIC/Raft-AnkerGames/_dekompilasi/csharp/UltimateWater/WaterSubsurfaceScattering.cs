using System;
using System.Collections.Generic;
using UltimateWater.Internal;
using UnityEngine;

namespace UltimateWater;

[Serializable]
public sealed class WaterSubsurfaceScattering : WaterModule
{
	[Serializable]
	public enum SubsurfaceScatteringMode
	{
		Disabled,
		TextureSpace
	}

	[SerializeField]
	private SubsurfaceScatteringMode _Mode = SubsurfaceScatteringMode.TextureSpace;

	[SerializeField]
	private BlurSSS _SubsurfaceScatteringBlur;

	[Range(0f, 0.9f)]
	[SerializeField]
	private float _IgnoredLightFraction = 0.15f;

	[Resolution(128, new int[] { 64, 128, 256, 512 })]
	[SerializeField]
	private int _AmbientResolution = 128;

	[SerializeField]
	[Range(-1f, 6f)]
	private int _LightCount = -1;

	[SerializeField]
	private int _LightingLayer = 22;

	private RenderTexture _ScatteringTex;

	private Vector4 _ShaderParams;

	private Water _Water;

	private static readonly List<Water> _CachedRenderList;

	public float IsotropicScatteringIntensity
	{
		get
		{
			return _ShaderParams.x;
		}
		set
		{
			_ShaderParams.x = value;
		}
	}

	public float SubsurfaceScatteringContrast
	{
		get
		{
			return _ShaderParams.y;
		}
		set
		{
			_ShaderParams.y = value;
		}
	}

	static WaterSubsurfaceScattering()
	{
		_CachedRenderList = new List<Water> { null };
	}

	internal override void OnWaterRender(WaterCamera waterCamera)
	{
		Camera cameraComponent = waterCamera.CameraComponent;
		Rect localMapsRect = waterCamera.LocalMapsRect;
		if (localMapsRect.width != 0f && Application.isPlaying && _Mode != SubsurfaceScatteringMode.Disabled)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(_AmbientResolution, _AmbientResolution, 0, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear);
			temporary.filterMode = FilterMode.Bilinear;
			temporary.name = "Subsurface temp 1";
			Camera effectsCamera = waterCamera.EffectsCamera;
			WaterCamera component = effectsCamera.GetComponent<WaterCamera>();
			component.enabled = true;
			component.GeometryType = WaterGeometryType.UniformGrid;
			_CachedRenderList[0] = _Water;
			component.SetCustomWaterRenderList(_CachedRenderList);
			effectsCamera.stereoTargetEye = StereoTargetEyeMask.None;
			effectsCamera.enabled = false;
			effectsCamera.depthTextureMode = DepthTextureMode.None;
			effectsCamera.renderingPath = RenderingPath.Forward;
			effectsCamera.orthographic = true;
			effectsCamera.orthographicSize = localMapsRect.width * 0.5f;
			effectsCamera.cullingMask = 1 << _LightingLayer;
			effectsCamera.farClipPlane = 2000f;
			effectsCamera.ResetProjectionMatrix();
			effectsCamera.clearFlags = CameraClearFlags.Nothing;
			effectsCamera.allowHDR = true;
			effectsCamera.transform.position = new Vector3(localMapsRect.center.x, 1000f, localMapsRect.center.y);
			effectsCamera.transform.rotation = Quaternion.LookRotation(new Vector3(0f, -1f, 0f), new Vector3(0f, 0f, 1f));
			effectsCamera.targetTexture = temporary;
			Shader.SetGlobalVector("_ScatteringParams", _ShaderParams);
			Shader.SetGlobalVector("_WorldSpaceOriginalCameraPos", cameraComponent.transform.position);
			int pixelLightCount = 3;
			if (_LightCount >= 0)
			{
				pixelLightCount = QualitySettings.pixelLightCount;
				QualitySettings.pixelLightCount = _LightCount;
			}
			Shader shader = ShaderUtility.Instance.Get(ShaderList.CollectLight);
			_Water.gameObject.layer = _LightingLayer;
			effectsCamera.RenderWithShader(shader, "CustomType");
			_Water.gameObject.layer = WaterProjectSettings.Instance.WaterLayer;
			if (_LightCount >= 0)
			{
				QualitySettings.pixelLightCount = pixelLightCount;
			}
			component.GeometryType = WaterGeometryType.Auto;
			component.SetCustomWaterRenderList(null);
			RenderTexture temporary2 = RenderTexture.GetTemporary(_AmbientResolution, _AmbientResolution, 0, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear);
			temporary2.filterMode = FilterMode.Point;
			temporary2.name = "Subsurface temp 2";
			Color parameterValue = _Water.Materials.GetParameterValue(WaterMaterials.ColorParameter.AbsorptionColor);
			_SubsurfaceScatteringBlur.BlurMaterial.SetVector("_ScatteringParams", _ShaderParams);
			_SubsurfaceScatteringBlur.Apply(temporary, temporary2, parameterValue, waterCamera.LocalMapsRect.width, _IgnoredLightFraction);
			RenderTexture.ReleaseTemporary(temporary);
			Graphics.Blit(temporary2, _ScatteringTex, _SubsurfaceScatteringBlur.BlurMaterial, 1);
			RenderTexture.ReleaseTemporary(temporary2);
			_Water.Renderer.PropertyBlock.SetTexture("_SubsurfaceScattering", _ScatteringTex);
			Graphics.SetRenderTarget(null);
		}
	}

	internal override void Start(Water water)
	{
		_Water = water;
		water.ProfilesManager.Changed.AddListener(ResolveProfileData);
	}

	internal override void Enable()
	{
		Validate();
		if (Application.isPlaying && _Mode == SubsurfaceScatteringMode.TextureSpace)
		{
			_ScatteringTex = new RenderTexture(_AmbientResolution, _AmbientResolution, 0, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear)
			{
				name = "[UWS] WaterSubsurfaceScattering - Scattering Tex",
				hideFlags = HideFlags.DontSave,
				filterMode = FilterMode.Bilinear,
				useMipMap = WaterProjectSettings.Instance.AllowFloatingPointMipMaps,
				autoGenerateMips = WaterProjectSettings.Instance.AllowFloatingPointMipMaps
			};
		}
	}

	internal override void Disable()
	{
		if (_ScatteringTex != null)
		{
			_ScatteringTex.Destroy();
			_ScatteringTex = null;
		}
		if (_Water != null)
		{
			Texture2D texture2D = DefaultTextures.Get(Color.white);
			if (texture2D != null)
			{
				_Water.Renderer.PropertyBlock.SetTexture("_SubsurfaceScattering", texture2D);
			}
		}
	}

	internal override void Destroy()
	{
		if (!(_Water == null))
		{
			_Water.ProfilesManager.Changed.RemoveListener(ResolveProfileData);
		}
	}

	private void ResolveProfileData(Water water)
	{
		List<Water.WeightedProfile> profiles = water.ProfilesManager.Profiles;
		_ShaderParams.x = 0f;
		_ShaderParams.y = 0f;
		for (int i = 0; i < profiles.Count; i++)
		{
			WaterProfileData profile = profiles[i].Profile;
			float weight = profiles[i].Weight;
			_ShaderParams.x += profile.IsotropicScatteringIntensity * weight;
			_ShaderParams.y += profile.SubsurfaceScatteringContrast * weight;
		}
		_ShaderParams.x *= 1f + _ShaderParams.y;
	}

	internal override void Validate()
	{
		if (_SubsurfaceScatteringBlur == null)
		{
			_SubsurfaceScatteringBlur = new BlurSSS();
		}
		_SubsurfaceScatteringBlur.Validate("UltimateWater/Utilities/Blur (Subsurface Scattering)", "Shaders/Blurs", 6);
	}
}
