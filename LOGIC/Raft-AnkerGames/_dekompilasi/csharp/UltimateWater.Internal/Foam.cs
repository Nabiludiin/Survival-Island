using System;
using System.Collections.Generic;
using UnityEngine;

namespace UltimateWater.Internal;

public sealed class Foam : WaterModule
{
	[Serializable]
	public class Data
	{
		[Tooltip("Foam map supersampling in relation to the waves simulator resolution. Has to be a power of two (0.25, 0.5, 1, 2, etc.)")]
		public float Supersampling = 1f;
	}

	private class CameraRenderData
	{
		public readonly int[] RenderFramePerLayer = new int[32];

		public Vector2 LastSurfaceOffset;
	}

	private readonly Water _Water;

	private readonly WindWaves _WindWaves;

	private readonly Data _Data;

	private float _FoamIntensity = 1f;

	private float _FoamThreshold = 1f;

	private float _FoamFadingFactor = 0.85f;

	private float _FoamShoreExtent;

	private bool _FoamIntensityOverriden;

	private Shader _LocalFoamSimulationShader;

	private Shader _GlobalFoamSimulationShader;

	private RenderTexture _FoamMapA;

	private RenderTexture _FoamMapB;

	private RenderTexture[] _DisplacementDeltaMaps;

	private int _Resolution;

	private bool _FirstFrame;

	private readonly DynamicWater _Overlays;

	private readonly Material _GlobalFoamSimulationMaterial;

	private static readonly Dictionary<WaterCamera, CameraRenderData> _LayerUpdateFrames = new Dictionary<WaterCamera, CameraRenderData>();

	public float FoamIntensity
	{
		get
		{
			return _FoamIntensity;
		}
		set
		{
			if (float.IsNaN(value))
			{
				_FoamIntensityOverriden = false;
				OnProfilesChanged(_Water);
				return;
			}
			_FoamIntensityOverriden = true;
			_FoamIntensity = value;
			if (_GlobalFoamSimulationMaterial != null)
			{
				float y = _FoamThreshold * (float)_Resolution / 2048f * 0.5f;
				_GlobalFoamSimulationMaterial.SetVector(ShaderVariables.FoamParameters, new Vector4(_FoamIntensity * 0.6f, y, 0f, _FoamFadingFactor));
			}
			MaterialPropertyBlock propertyBlock = _Water.Renderer.PropertyBlock;
			float y2 = _FoamThreshold * (float)_Resolution / 2048f * 0.5f;
			propertyBlock.SetVector(ShaderVariables.FoamParameters, new Vector4(_FoamIntensity * 0.6f, y2, 150f / (_FoamShoreExtent * _FoamShoreExtent), _FoamFadingFactor));
		}
	}

	public Texture FoamMap => _FoamMapA;

	public Foam(Water water, Data data)
	{
		_Water = water;
		_WindWaves = water.WindWaves;
		_Overlays = water.DynamicWater;
		_Data = data;
		Validate();
		_WindWaves.ResolutionChanged.AddListener(OnResolutionChanged);
		_Resolution = Mathf.RoundToInt((float)_WindWaves.FinalResolution * data.Supersampling);
		_GlobalFoamSimulationMaterial = new Material(_GlobalFoamSimulationShader)
		{
			hideFlags = HideFlags.DontSave
		};
		_FirstFrame = true;
	}

	public void RenderOverlays(DynamicWaterCameraData overlays)
	{
		if (!Application.isPlaying || !CheckPreresquisites())
		{
			return;
		}
		WaterCamera camera = overlays.Camera;
		if (camera.Type != WaterCamera.CameraType.Normal)
		{
			return;
		}
		int layer = _Water.gameObject.layer;
		if (!_LayerUpdateFrames.TryGetValue(camera, out var value))
		{
			value = (_LayerUpdateFrames[camera] = new CameraRenderData());
			camera.Destroyed += OnCameraDestroyed;
		}
		int frameCount = Time.frameCount;
		if (value.RenderFramePerLayer[layer] < frameCount)
		{
			value.RenderFramePerLayer[layer] = frameCount;
			if (_Water.WindWaves.FinalRenderMode == WaveSpectrumRenderMode.FullFFT)
			{
				RenderTexture[] displacementDeltaMaps = GetDisplacementDeltaMaps();
				float y = _FoamThreshold * (float)_Resolution / 2048f * 0.5f;
				_GlobalFoamSimulationMaterial.SetVector(ShaderVariables.FoamParameters, new Vector4(_FoamIntensity * 0.6f, y, 0f, _FoamFadingFactor));
				for (int i = 0; i < 4; i++)
				{
					Texture displacementMap = _Water.WindWaves.WaterWavesFFT.GetDisplacementMap(i);
					RenderTexture dest = displacementDeltaMaps[i];
					_GlobalFoamSimulationMaterial.SetFloat(ShaderVariables.WaterTileSizeInvSrt, _Water.WindWaves.TileSizesInv[i]);
					Graphics.Blit(displacementMap, dest, _GlobalFoamSimulationMaterial, 1);
				}
				Shader.SetGlobalTexture("_FoamMapPrevious", overlays.FoamMapPrevious);
				Shader.SetGlobalVector("_WaterOffsetDelta", _Water.SurfaceOffset - value.LastSurfaceOffset);
				value.LastSurfaceOffset = _Water.SurfaceOffset;
				Camera planeProjectorCamera = camera.PlaneProjectorCamera;
				planeProjectorCamera.cullingMask = 1 << layer;
				planeProjectorCamera.GetComponent<WaterCamera>().RenderWaterWithShader("[PW Water] Foam", overlays.FoamMap, _LocalFoamSimulationShader, _Water);
			}
		}
		_Water.Renderer.PropertyBlock.SetTexture("_FoamMap", overlays.FoamMap);
	}

	internal override void Enable()
	{
		_Water.ProfilesManager.Changed.AddListener(OnProfilesChanged);
		OnProfilesChanged(_Water);
	}

	internal override void Disable()
	{
		_Water.ProfilesManager.Changed.RemoveListener(OnProfilesChanged);
	}

	private void SetupFoamMaterials()
	{
		if (_GlobalFoamSimulationMaterial != null)
		{
			float num = _FoamThreshold * (float)_Resolution / 2048f * 0.5f;
			float num2 = num * 220f;
			_GlobalFoamSimulationMaterial.SetVector(ShaderVariables.FoamParameters, new Vector4(_FoamIntensity * 0.6f, num, 0f, _FoamFadingFactor));
			_GlobalFoamSimulationMaterial.SetVector(ShaderVariables.FoamIntensity, new Vector4(num2 / _WindWaves.TileSizes.x, num2 / _WindWaves.TileSizes.y, num2 / _WindWaves.TileSizes.z, num2 / _WindWaves.TileSizes.w));
		}
	}

	internal override void Validate()
	{
		if (_GlobalFoamSimulationShader == null)
		{
			_GlobalFoamSimulationShader = Shader.Find("UltimateWater/Foam/Global");
		}
		if (_LocalFoamSimulationShader == null)
		{
			_LocalFoamSimulationShader = Shader.Find("UltimateWater/Foam/Local");
		}
		_Data.Supersampling = (float)Mathf.ClosestPowerOfTwo(Mathf.RoundToInt(_Data.Supersampling * 4096f)) / 4096f;
	}

	internal override void Destroy()
	{
		if (_FoamMapA != null)
		{
			_FoamMapA.Destroy();
			_FoamMapB.Destroy();
			_FoamMapA = null;
			_FoamMapB = null;
		}
		if (_DisplacementDeltaMaps != null)
		{
			for (int i = 0; i < _DisplacementDeltaMaps.Length; i++)
			{
				_DisplacementDeltaMaps[i].Destroy();
			}
			_DisplacementDeltaMaps = null;
		}
	}

	internal override void Update()
	{
		if (!_FirstFrame && _Overlays == null)
		{
			UpdateFoamTiled();
		}
		else
		{
			_FirstFrame = false;
		}
	}

	private void CheckTilesFoamResources()
	{
		if (_FoamMapA == null)
		{
			_FoamMapA = CreateRt(0, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear, FilterMode.Trilinear, TextureWrapMode.Repeat);
			_FoamMapA.name = "[UWS] Foam - Map A";
			_FoamMapB = CreateRt(0, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear, FilterMode.Trilinear, TextureWrapMode.Repeat);
			_FoamMapB.name = "[UWS] Foam - Map B";
			RenderTexture.active = null;
		}
	}

	private RenderTexture CreateRt(int depth, RenderTextureFormat format, RenderTextureReadWrite readWrite, FilterMode filterMode, TextureWrapMode wrapMode)
	{
		bool allowFloatingPointMipMaps = WaterProjectSettings.Instance.AllowFloatingPointMipMaps;
		RenderTexture renderTexture = new RenderTexture(_Resolution, _Resolution, depth, format, readWrite)
		{
			name = "[UWS] Foam",
			hideFlags = HideFlags.DontSave,
			filterMode = filterMode,
			wrapMode = wrapMode,
			useMipMap = allowFloatingPointMipMaps,
			autoGenerateMips = allowFloatingPointMipMaps
		};
		RenderTexture.active = renderTexture;
		GL.Clear(clearDepth: false, clearColor: true, new Color(0f, 0f, 0f, 0f));
		return renderTexture;
	}

	private void UpdateFoamTiled()
	{
		if (CheckPreresquisites())
		{
			CheckTilesFoamResources();
			SetupFoamMaterials();
			WavesRendererFFT waterWavesFFT = _WindWaves.WaterWavesFFT;
			_GlobalFoamSimulationMaterial.SetTexture("_DisplacementMap0", waterWavesFFT.GetDisplacementMap(0));
			_GlobalFoamSimulationMaterial.SetTexture("_DisplacementMap1", waterWavesFFT.GetDisplacementMap(1));
			_GlobalFoamSimulationMaterial.SetTexture("_DisplacementMap2", waterWavesFFT.GetDisplacementMap(2));
			_GlobalFoamSimulationMaterial.SetTexture("_DisplacementMap3", waterWavesFFT.GetDisplacementMap(3));
			Graphics.Blit(_FoamMapA, _FoamMapB, _GlobalFoamSimulationMaterial, 0);
			_Water.Renderer.PropertyBlock.SetTexture("_FoamMap", _FoamMapB);
			SwapRenderTargets();
		}
	}

	private void OnResolutionChanged(WindWaves windWaves)
	{
		_Resolution = Mathf.RoundToInt((float)windWaves.FinalResolution * _Data.Supersampling);
		Destroy();
	}

	private bool CheckPreresquisites()
	{
		if (_WindWaves != null)
		{
			return _WindWaves.FinalRenderMode == WaveSpectrumRenderMode.FullFFT;
		}
		return false;
	}

	private void OnProfilesChanged(Water water)
	{
		List<Water.WeightedProfile> profiles = water.ProfilesManager.Profiles;
		float num = 0f;
		_FoamThreshold = 0f;
		_FoamFadingFactor = 0f;
		_FoamShoreExtent = 0f;
		float num2 = 0f;
		float num3 = 0f;
		if (profiles != null)
		{
			for (int num4 = profiles.Count - 1; num4 >= 0; num4--)
			{
				Water.WeightedProfile weightedProfile = profiles[num4];
				WaterProfileData profile = weightedProfile.Profile;
				float weight = weightedProfile.Weight;
				num += profile.FoamIntensity * weight;
				_FoamThreshold += profile.FoamThreshold * weight;
				_FoamFadingFactor += profile.FoamFadingFactor * weight;
				_FoamShoreExtent += profile.FoamShoreExtent * weight;
				num2 += profile.FoamShoreIntensity * weight;
				num3 += profile.FoamNormalScale * weight;
			}
		}
		if (!_FoamIntensityOverriden)
		{
			_FoamIntensity = num;
		}
		MaterialPropertyBlock propertyBlock = water.Renderer.PropertyBlock;
		propertyBlock.SetFloat("_FoamNormalScale", num3);
		if (_FoamShoreExtent < 0.001f)
		{
			_FoamShoreExtent = 0.001f;
		}
		propertyBlock.SetVector(value: new Vector4(num * 0.6f, _FoamThreshold * (float)_Resolution / 2048f * 0.5f, 150f / (_FoamShoreExtent * _FoamShoreExtent), _FoamFadingFactor), nameID: ShaderVariables.FoamParameters);
		propertyBlock.SetFloat(ShaderVariables.FoamShoreIntensity, num2);
	}

	private void SwapRenderTargets()
	{
		RenderTexture foamMapA = _FoamMapA;
		_FoamMapA = _FoamMapB;
		_FoamMapB = foamMapA;
	}

	private RenderTexture[] GetDisplacementDeltaMaps()
	{
		if (_DisplacementDeltaMaps == null)
		{
			_DisplacementDeltaMaps = new RenderTexture[4];
			bool allowFloatingPointMipMaps = WaterProjectSettings.Instance.AllowFloatingPointMipMaps;
			for (int i = 0; i < 4; i++)
			{
				_DisplacementDeltaMaps[i] = new RenderTexture(_Resolution, _Resolution, 0, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear)
				{
					name = "[UWS] Foam - Displacement Delta Map [" + i + "]",
					useMipMap = allowFloatingPointMipMaps,
					autoGenerateMips = allowFloatingPointMipMaps,
					wrapMode = TextureWrapMode.Repeat,
					filterMode = ((!allowFloatingPointMipMaps) ? FilterMode.Bilinear : FilterMode.Trilinear)
				};
				_Water.Renderer.PropertyBlock.SetTexture(ShaderVariables.DisplacementDeltaMaps[i], _DisplacementDeltaMaps[i]);
			}
		}
		return _DisplacementDeltaMaps;
	}

	private static void OnCameraDestroyed(WaterCamera waterCamera)
	{
		_LayerUpdateFrames.Remove(waterCamera);
	}
}
