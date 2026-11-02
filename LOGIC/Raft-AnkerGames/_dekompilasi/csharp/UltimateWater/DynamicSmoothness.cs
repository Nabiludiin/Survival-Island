using System;
using System.Collections.Generic;
using UltimateWater.Internal;
using UnityEngine;
using UnityEngine.Rendering;

namespace UltimateWater;

[Serializable]
public class DynamicSmoothness
{
	private readonly Water _Water;

	private readonly WindWaves _WindWaves;

	private readonly bool _Supported;

	private static RenderTextureFormat _Format;

	private ComputeShader _VarianceShader;

	private RenderTexture _VarianceTexture;

	private int _LastResetIndex;

	private int _CurrentIndex;

	private bool _Finished;

	private bool _Initialized;

	private float _DynamicSmoothnessIntensity;

	public bool Enabled => _Water.ShaderSet.SmoothnessMode == DynamicSmoothnessMode.Physical;

	public Texture VarianceTexture => _VarianceTexture;

	public ComputeShader ComputeShader
	{
		get
		{
			return _VarianceShader;
		}
		set
		{
			_VarianceShader = value;
		}
	}

	public DynamicSmoothness(Water water, WindWaves windWaves)
	{
		_Water = water;
		_WindWaves = windWaves;
		_Supported = CheckSupport();
		_VarianceShader = water.ShaderSet.GetComputeShader("Spectral Variances");
		OnCopyModeChanged();
	}

	public void FreeResources()
	{
		if (_VarianceTexture != null)
		{
			_VarianceTexture.Destroy();
			_VarianceTexture = null;
		}
	}

	public void OnCopyModeChanged()
	{
		if (_WindWaves != null && !(_WindWaves.CopyFrom == null))
		{
			_WindWaves.CopyFrom.ForceStartup();
			FreeResources();
			WindWaves windWaves = _WindWaves.CopyFrom.WindWaves;
			windWaves.DynamicSmoothness.ValidateVarianceTextures();
			_Water.Renderer.PropertyBlock.SetTexture("_SlopeVariance", windWaves.DynamicSmoothness._VarianceTexture);
		}
	}

	public static bool CheckSupport()
	{
		RenderTextureFormat? format = Compatibility.GetFormat(RenderTextureFormat.RGHalf, new RenderTextureFormat[1] { RenderTextureFormat.RGFloat });
		if (!SystemInfo.supportsComputeShaders || !SystemInfo.supports3DTextures || !format.HasValue)
		{
			WaterLogger.Warning("Dynamic Smoothness", "Check Support", "Dynamic Smoothness not supported");
			if (!SystemInfo.supportsComputeShaders)
			{
				WaterLogger.Warning("Dynamic Smoothness", "Check Support", " - compute shaders not supported");
			}
			if (!SystemInfo.supports3DTextures)
			{
				WaterLogger.Warning("Dynamic Smoothness", "Check Support", " - 3D textures not supported");
			}
			if (!format.HasValue)
			{
				WaterLogger.Warning("Dynamic Smoothness", "Check Support", " - necessary RenderTexture formats not found");
			}
			return false;
		}
		_Format = format.Value;
		return true;
	}

	public void Update()
	{
		if (_Water.ShaderSet.SmoothnessMode == DynamicSmoothnessMode.Physical && _Supported)
		{
			if (!_Initialized)
			{
				InitializeVariance();
			}
			ValidateVarianceTextures();
			if (!_Finished)
			{
				RenderNextPixel();
			}
		}
	}

	private void InitializeVariance()
	{
		_Initialized = true;
		_Water.ProfilesManager.Changed.AddListener(OnProfilesChanged);
		_WindWaves.WindDirectionChanged.AddListener(OnWindDirectionChanged);
		OnProfilesChanged(_Water);
	}

	private void ValidateVarianceTextures()
	{
		if (_VarianceTexture == null)
		{
			_VarianceTexture = CreateVarianceTexture(_Format);
			ResetComputations();
		}
		if (!_VarianceTexture.IsCreated())
		{
			_VarianceTexture.Create();
			_Water.Renderer.PropertyBlock.SetTexture("_SlopeVariance", _VarianceTexture);
			_LastResetIndex = 0;
			_CurrentIndex = 0;
		}
	}

	private void RenderNextPixel()
	{
		_VarianceShader.SetInt("_FFTSize", _WindWaves.FinalResolution);
		_VarianceShader.SetInt("_FFTSizeHalf", _WindWaves.FinalResolution >> 1);
		_VarianceShader.SetFloat("_VariancesSize", _VarianceTexture.width);
		_VarianceShader.SetFloat("_IntensityScale", _DynamicSmoothnessIntensity);
		_VarianceShader.SetVector("_TileSizes", _WindWaves.TileSizes);
		_VarianceShader.SetVector("_Coordinates", new Vector4(_CurrentIndex % 4, (_CurrentIndex >> 2) % 4, _CurrentIndex >> 4, 0f));
		_VarianceShader.SetTexture(0, "_Spectrum", _WindWaves.SpectrumResolver.GetRawDirectionalSpectrum());
		_VarianceShader.SetTexture(0, "_Variance", _VarianceTexture);
		_VarianceShader.Dispatch(0, 1, 1, 1);
		_CurrentIndex++;
		if (_CurrentIndex >= 64)
		{
			_CurrentIndex = 0;
		}
		if (_CurrentIndex == _LastResetIndex)
		{
			_Finished = true;
		}
	}

	private void ResetComputations()
	{
		_LastResetIndex = _CurrentIndex;
		_Finished = false;
	}

	private void OnProfilesChanged(Water water)
	{
		ResetComputations();
		_DynamicSmoothnessIntensity = 0f;
		List<Water.WeightedProfile> profiles = water.ProfilesManager.Profiles;
		for (int num = profiles.Count - 1; num >= 0; num--)
		{
			_DynamicSmoothnessIntensity += profiles[num].Profile.DynamicSmoothnessIntensity * profiles[num].Weight;
		}
	}

	private void OnWindDirectionChanged(WindWaves windWaves)
	{
		ResetComputations();
	}

	private static RenderTexture CreateVarianceTexture(RenderTextureFormat format)
	{
		RenderTexture renderTexture = new RenderTexture(4, 4, 0, format, RenderTextureReadWrite.Linear)
		{
			name = "[UWS] DynamicSmoothness - Variance Tex",
			hideFlags = HideFlags.DontSave,
			volumeDepth = 4,
			enableRandomWrite = true,
			wrapMode = TextureWrapMode.Clamp,
			filterMode = FilterMode.Bilinear,
			autoGenerateMips = false,
			useMipMap = false,
			dimension = TextureDimension.Tex3D
		};
		renderTexture.Clear(Color.black);
		return renderTexture;
	}
}
