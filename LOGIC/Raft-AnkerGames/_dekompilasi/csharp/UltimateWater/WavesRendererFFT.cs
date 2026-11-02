using System;
using UltimateWater.Internal;
using UnityEngine;

namespace UltimateWater;

public sealed class WavesRendererFFT
{
	[Serializable]
	public sealed class Data
	{
		[Tooltip("Determines if GPU partial derivatives or Fast Fourier Transform (high quality) should be used to compute normal map (Recommended: on). Works only if displacement map rendering is enabled.")]
		public bool HighQualityNormalMaps = true;

		[Tooltip("Check this option, if your water is flat or game crashes instantly on a DX11 GPU (in editor or build). Compute shaders are very fast, so use this as a last resort.")]
		public bool ForcePixelShader;

		[Tooltip("Fixes crest artifacts during storms, but lowers overall quality. Enabled by default when used with additive water volumes as it is actually needed and disabled in all other cases.")]
		public FlattenMode FlattenMode;

		[Tooltip("Sea state will be cached in the specified frame count for extra performance, if LoopLength on WindWaves is set to a value greater than zero.")]
		public int CachedFrameCount = 180;
	}

	public enum SpectrumType
	{
		Phillips,
		Unified
	}

	[Flags]
	public enum MapType
	{
		Displacement = 1,
		Normal = 2
	}

	public enum FlattenMode
	{
		Auto,
		ForcedOn,
		ForcedOff
	}

	private Shader _FFTShader;

	private Shader _FFTUtilitiesShader;

	private readonly Water _Water;

	private readonly WindWaves _WindWaves;

	private readonly Data _Data;

	private readonly RenderTexture[][] _NormalMapsCache;

	private readonly RenderTexture[][] _DisplacementMapsCache;

	private readonly bool[] _IsCachedFrameValid;

	private RenderTexture[] _NormalMaps;

	private RenderTexture[] _DisplacementMaps;

	private RenderTexturesCache _SingleTargetCache;

	private RenderTexturesCache _DoubleTargetCache;

	private GpuFFT _HeightFFT;

	private GpuFFT _NormalFFT;

	private GpuFFT _DisplacementFFT;

	private Material _FFTUtilitiesMaterial;

	private ComputeShader _Dx11FFT;

	private MapType _RenderedMaps;

	private bool _FinalHighQualityNormalMaps;

	private bool _CopyModeDirty;

	private int _WaveMapsFrame;

	private Water _LastCopyFrom;

	private static readonly Vector4[] _Offsets = new Vector4[4]
	{
		new Vector4(0f, 0f, 0f, 0f),
		new Vector4(0.5f, 0f, 0f, 0f),
		new Vector4(0f, 0.5f, 0f, 0f),
		new Vector4(0.5f, 0.5f, 0f, 0f)
	};

	private static readonly Vector4[] _OffsetsDual = new Vector4[2]
	{
		new Vector4(0f, 0f, 0.5f, 0f),
		new Vector4(0f, 0.5f, 0.5f, 0.5f)
	};

	public MapType RenderedMaps
	{
		get
		{
			return _RenderedMaps;
		}
		set
		{
			_RenderedMaps = value;
			if (Enabled && Application.isPlaying)
			{
				Dispose(total: false);
				ValidateResources();
			}
		}
	}

	public bool Enabled { get; private set; }

	public RenderTexture[] NormalMaps => _NormalMaps;

	public event Action ResourcesChanged;

	public void OnCopyModeChanged()
	{
		_CopyModeDirty = true;
		if (_LastCopyFrom != null)
		{
			_LastCopyFrom.WindWaves.WaterWavesFFT.ResourcesChanged -= ValidateResources;
		}
		if (_WindWaves.CopyFrom != null)
		{
			_WindWaves.CopyFrom.WindWaves.WaterWavesFFT.ResourcesChanged += ValidateResources;
		}
		_LastCopyFrom = _WindWaves.CopyFrom;
		Dispose(total: false);
	}

	public Texture GetDisplacementMap(int index)
	{
		if (_DisplacementMaps == null)
		{
			return null;
		}
		return _DisplacementMaps[index];
	}

	public Texture GetNormalMap(int index)
	{
		return _NormalMaps[index];
	}

	public void OnWaterRender(Camera camera)
	{
		if (!(_FFTUtilitiesMaterial == null))
		{
			ValidateWaveMaps();
		}
	}

	public WavesRendererFFT(Water water, WindWaves windWaves, Data data)
	{
		_Water = water;
		_WindWaves = windWaves;
		_Data = data;
		if (windWaves.LoopDuration != 0f)
		{
			_NormalMapsCache = new RenderTexture[data.CachedFrameCount][];
			_DisplacementMapsCache = new RenderTexture[data.CachedFrameCount][];
			_IsCachedFrameValid = new bool[data.CachedFrameCount];
			water.ProfilesManager.Changed.AddListener(OnProfilesChanged);
		}
		Validate();
	}

	private void RenderSpectra(float time, out Texture heightSpectrum, out Texture normalSpectrum, out Texture displacementSpectrum)
	{
		if (_RenderedMaps == MapType.Normal)
		{
			heightSpectrum = null;
			displacementSpectrum = null;
			normalSpectrum = _WindWaves.SpectrumResolver.RenderNormalsSpectrumAt(time);
		}
		else if ((_RenderedMaps & MapType.Normal) == 0 || !_FinalHighQualityNormalMaps)
		{
			normalSpectrum = null;
			_WindWaves.SpectrumResolver.RenderDisplacementsSpectraAt(time, out heightSpectrum, out displacementSpectrum);
		}
		else
		{
			_WindWaves.SpectrumResolver.RenderCompleteSpectraAt(time, out heightSpectrum, out normalSpectrum, out displacementSpectrum);
		}
	}

	private void RenderMaps(float time, RenderTexture[] displacementMaps, RenderTexture[] normalMaps)
	{
		RenderSpectra(time, out var heightSpectrum, out var normalSpectrum, out var displacementSpectrum);
		if ((_RenderedMaps & MapType.Displacement) != 0)
		{
			TemporaryRenderTexture temporary = _SingleTargetCache.GetTemporary();
			TemporaryRenderTexture temporary2 = _DoubleTargetCache.GetTemporary();
			_HeightFFT.ComputeFFT(heightSpectrum, temporary);
			_DisplacementFFT.ComputeFFT(displacementSpectrum, temporary2);
			_FFTUtilitiesMaterial.SetTexture(ShaderVariables.HeightTex, (RenderTexture)temporary);
			_FFTUtilitiesMaterial.SetTexture(ShaderVariables.DisplacementTex, (RenderTexture)temporary2);
			_FFTUtilitiesMaterial.SetFloat(ShaderVariables.HorizontalDisplacementScale, _Water.Materials.HorizontalDisplacementScale);
			for (int i = 0; i < 4; i++)
			{
				_FFTUtilitiesMaterial.SetFloat(ShaderVariables.JacobianScale, _Water.Materials.HorizontalDisplacementScale * 0.1f * (float)displacementMaps[i].width / _WindWaves.TileSizes[i]);
				_FFTUtilitiesMaterial.SetVector(ShaderVariables.Offset, _Offsets[i]);
				Graphics.Blit(null, displacementMaps[i], _FFTUtilitiesMaterial, 1);
			}
			temporary.Dispose();
			temporary2.Dispose();
		}
		if ((_RenderedMaps & MapType.Normal) == 0)
		{
			return;
		}
		if (!_FinalHighQualityNormalMaps)
		{
			for (int j = 0; j < 2; j++)
			{
				int finalResolution = _WindWaves.FinalResolution;
				_FFTUtilitiesMaterial.SetFloat("_Intensity1", 0.58f * (float)finalResolution / _WindWaves.TileSizes[j * 2]);
				_FFTUtilitiesMaterial.SetFloat("_Intensity2", 0.58f * (float)finalResolution / _WindWaves.TileSizes[j * 2 + 1]);
				_FFTUtilitiesMaterial.SetTexture("_MainTex", displacementMaps[j << 1]);
				_FFTUtilitiesMaterial.SetTexture("_SecondTex", displacementMaps[(j << 1) + 1]);
				_FFTUtilitiesMaterial.SetFloat("_MainTex_Texel_Size", 1f / (float)displacementMaps[j << 1].width);
				Graphics.Blit(null, normalMaps[j], _FFTUtilitiesMaterial, 0);
			}
		}
		else
		{
			TemporaryRenderTexture temporary3 = _DoubleTargetCache.GetTemporary();
			_NormalFFT.ComputeFFT(normalSpectrum, temporary3);
			for (int k = 0; k < 2; k++)
			{
				_FFTUtilitiesMaterial.SetVector(ShaderVariables.Offset, _OffsetsDual[k]);
				Graphics.Blit((RenderTexture)temporary3, normalMaps[k], _FFTUtilitiesMaterial, 3);
			}
			temporary3.Dispose();
		}
	}

	private void RetrieveCachedFrame(int frameIndex, out RenderTexture[] displacementMaps, out RenderTexture[] normalMaps)
	{
		float time = (float)frameIndex / (float)_Data.CachedFrameCount * _WindWaves.LoopDuration;
		if (!_IsCachedFrameValid[frameIndex])
		{
			_IsCachedFrameValid[frameIndex] = true;
			if ((_RenderedMaps & MapType.Displacement) != 0 && _DisplacementMapsCache[frameIndex] == null)
			{
				CreateRenderTextures(ref _DisplacementMapsCache[frameIndex], "[UWS] WavesRendererFFT - Water Displacement Map", RenderTextureFormat.ARGBHalf, 4, mipMaps: true);
			}
			if ((_RenderedMaps & MapType.Normal) != 0 && _NormalMapsCache[frameIndex] == null)
			{
				CreateRenderTextures(ref _NormalMapsCache[frameIndex], "[UWS] WavesRendererFFT - Water Normal Map", RenderTextureFormat.ARGBHalf, 2, mipMaps: true);
			}
			RenderMaps(time, _DisplacementMapsCache[frameIndex], _NormalMapsCache[frameIndex]);
		}
		displacementMaps = _DisplacementMapsCache[frameIndex];
		normalMaps = _NormalMapsCache[frameIndex];
	}

	private void RenderMapsFromCache(float time, RenderTexture[] displacementMaps, RenderTexture[] normalMaps)
	{
		float num = (float)_Data.CachedFrameCount * FastMath.FracAdditive(time / _WindWaves.LoopDuration);
		int num2 = (int)num;
		float value = num - (float)num2;
		RetrieveCachedFrame(num2, out var displacementMaps2, out var normalMaps2);
		int num3 = num2 + 1;
		if (num3 >= _Data.CachedFrameCount)
		{
			num3 = 0;
		}
		RetrieveCachedFrame(num3, out var displacementMaps3, out var normalMaps3);
		_FFTUtilitiesMaterial.SetFloat("_BlendFactor", value);
		for (int i = 0; i < 4; i++)
		{
			_FFTUtilitiesMaterial.SetTexture("_Texture1", displacementMaps2[i]);
			_FFTUtilitiesMaterial.SetTexture("_Texture2", displacementMaps3[i]);
			Graphics.Blit(null, displacementMaps[i], _FFTUtilitiesMaterial, 6);
		}
		for (int j = 0; j < 2; j++)
		{
			_FFTUtilitiesMaterial.SetTexture("_Texture1", normalMaps2[j]);
			_FFTUtilitiesMaterial.SetTexture("_Texture2", normalMaps3[j]);
			Graphics.Blit(null, normalMaps[j], _FFTUtilitiesMaterial, 6);
		}
	}

	private void ValidateWaveMaps()
	{
		int frameCount = Time.frameCount;
		if (_WaveMapsFrame == frameCount || !Application.isPlaying)
		{
			return;
		}
		if (_LastCopyFrom != null)
		{
			if (_CopyModeDirty)
			{
				_CopyModeDirty = false;
				ValidateResources();
			}
		}
		else
		{
			_WaveMapsFrame = frameCount;
			if (_WindWaves.LoopDuration == 0f)
			{
				RenderMaps(_Water.Time, _DisplacementMaps, _NormalMaps);
			}
			else
			{
				RenderMapsFromCache(_Water.Time, _DisplacementMaps, _NormalMaps);
			}
		}
	}

	private void OnResolutionChanged(WindWaves windWaves)
	{
		Dispose(total: false);
		ValidateResources();
	}

	private void Dispose(bool total)
	{
		_WaveMapsFrame = -1;
		if (_HeightFFT != null)
		{
			_HeightFFT.Dispose();
			_HeightFFT = null;
		}
		if (_NormalFFT != null)
		{
			_NormalFFT.Dispose();
			_NormalFFT = null;
		}
		if (_DisplacementFFT != null)
		{
			_DisplacementFFT.Dispose();
			_DisplacementFFT = null;
		}
		if (_NormalMaps != null)
		{
			RenderTexture[] normalMaps = _NormalMaps;
			for (int i = 0; i < normalMaps.Length; i++)
			{
				normalMaps[i].Destroy();
			}
			_NormalMaps = null;
		}
		if (_DisplacementMaps != null)
		{
			RenderTexture[] normalMaps = _DisplacementMaps;
			for (int i = 0; i < normalMaps.Length; i++)
			{
				normalMaps[i].Destroy();
			}
			_DisplacementMaps = null;
		}
		if (_NormalMapsCache != null)
		{
			for (int num = _NormalMapsCache.Length - 1; num >= 0; num--)
			{
				RenderTexture[] array = _NormalMapsCache[num];
				if (array != null)
				{
					for (int num2 = array.Length - 1; num2 >= 0; num2--)
					{
						array[num2].Destroy();
					}
					_NormalMapsCache[num] = null;
				}
			}
		}
		if (_DisplacementMapsCache != null)
		{
			for (int num3 = _DisplacementMapsCache.Length - 1; num3 >= 0; num3--)
			{
				RenderTexture[] array2 = _DisplacementMapsCache[num3];
				if (array2 != null)
				{
					for (int num4 = array2.Length - 1; num4 >= 0; num4--)
					{
						array2[num4].Destroy();
					}
					_DisplacementMapsCache[num3] = null;
				}
			}
		}
		if (_IsCachedFrameValid != null)
		{
			for (int num5 = _IsCachedFrameValid.Length - 1; num5 >= 0; num5--)
			{
				_IsCachedFrameValid[num5] = false;
			}
		}
		if (total && _FFTUtilitiesMaterial != null)
		{
			_FFTUtilitiesMaterial.Destroy();
			_FFTUtilitiesMaterial = null;
		}
	}

	private void OnProfilesChanged(Water water)
	{
		for (int num = _IsCachedFrameValid.Length - 1; num >= 0; num--)
		{
			_IsCachedFrameValid[num] = false;
		}
	}

	internal void Validate()
	{
		_Dx11FFT = _Water.ShaderSet.GetComputeShader("DX11 FFT");
		if (_FFTShader == null)
		{
			_FFTShader = Shader.Find("UltimateWater/Base/FFT");
		}
		if (_FFTUtilitiesShader == null)
		{
			_FFTUtilitiesShader = Shader.Find("UltimateWater/Utilities/FFT Utilities");
		}
		if (Application.isPlaying && Enabled)
		{
			ResolveFinalSettings(WaterQualitySettings.Instance.CurrentQualityLevel);
		}
	}

	internal void ResolveFinalSettings(WaterQualityLevel qualityLevel)
	{
		_FinalHighQualityNormalMaps = _Data.HighQualityNormalMaps;
		if (!qualityLevel.AllowHighQualityNormalMaps)
		{
			_FinalHighQualityNormalMaps = false;
		}
		if ((_RenderedMaps & MapType.Displacement) == 0)
		{
			_FinalHighQualityNormalMaps = true;
		}
	}

	private GpuFFT ChooseBestFFTAlgorithm(bool twoChannels)
	{
		int finalResolution = _WindWaves.FinalResolution;
		GpuFFT gpuFFT = ((_Data.ForcePixelShader || !(_Dx11FFT != null) || !SystemInfo.supportsComputeShaders || finalResolution > 512) ? ((GpuFFT)new PixelShaderFFT(_FFTShader, finalResolution, _WindWaves.FinalHighPrecision || finalResolution >= 2048, twoChannels)) : ((GpuFFT)new Dx11FFT(_Dx11FFT, finalResolution, _WindWaves.FinalHighPrecision || finalResolution >= 2048, twoChannels)));
		gpuFFT.SetupMaterials();
		return gpuFFT;
	}

	private void ValidateFFT(ref GpuFFT fft, bool present, bool twoChannels)
	{
		if (present)
		{
			if (fft == null)
			{
				fft = ChooseBestFFTAlgorithm(twoChannels);
			}
		}
		else if (fft != null)
		{
			fft.Dispose();
			fft = null;
		}
	}

	private RenderTexture CreateRenderTexture(string name, RenderTextureFormat format, bool mipMaps)
	{
		RenderTexture renderTexture = new RenderTexture(_WindWaves.FinalResolution, _WindWaves.FinalResolution, 0, format, RenderTextureReadWrite.Linear)
		{
			name = name,
			hideFlags = HideFlags.DontSave,
			wrapMode = TextureWrapMode.Repeat
		};
		if (mipMaps && WaterProjectSettings.Instance.AllowFloatingPointMipMaps)
		{
			renderTexture.filterMode = FilterMode.Trilinear;
			renderTexture.useMipMap = true;
			renderTexture.autoGenerateMips = true;
		}
		else
		{
			renderTexture.filterMode = FilterMode.Bilinear;
		}
		return renderTexture;
	}

	private void CreateRenderTextures(ref RenderTexture[] renderTextures, string name, RenderTextureFormat format, int count, bool mipMaps)
	{
		renderTextures = new RenderTexture[count];
		for (int i = 0; i < count; i++)
		{
			renderTextures[i] = CreateRenderTexture(name, format, mipMaps);
		}
	}

	private void ValidateResources()
	{
		if (_WindWaves.CopyFrom == null)
		{
			ValidateFFT(ref _HeightFFT, (_RenderedMaps & MapType.Displacement) != 0, twoChannels: false);
			ValidateFFT(ref _DisplacementFFT, (_RenderedMaps & MapType.Displacement) != 0, twoChannels: true);
			ValidateFFT(ref _NormalFFT, (_RenderedMaps & MapType.Normal) != 0, twoChannels: true);
		}
		if (_DisplacementMaps != null && _NormalMaps != null)
		{
			return;
		}
		RenderTexture[] displacementMaps;
		RenderTexture[] normalMaps;
		if (_WindWaves.CopyFrom == null)
		{
			int num = _WindWaves.FinalResolution << 1;
			_SingleTargetCache = RenderTexturesCache.GetCache(num, num, 0, RenderTextureFormat.RHalf, linear: true, _HeightFFT is Dx11FFT);
			_DoubleTargetCache = RenderTexturesCache.GetCache(num, num, 0, RenderTextureFormat.RGHalf, linear: true, _DisplacementFFT is Dx11FFT);
			if (_DisplacementMaps == null && (_RenderedMaps & MapType.Displacement) != 0)
			{
				CreateRenderTextures(ref _DisplacementMaps, "[UWS] WavesRendererFFT - Water Displacement Map", RenderTextureFormat.ARGBHalf, 4, mipMaps: true);
			}
			if (_NormalMaps == null && (_RenderedMaps & MapType.Normal) != 0)
			{
				CreateRenderTextures(ref _NormalMaps, "[UWS] WavesRendererFFT - Water Normal Map", RenderTextureFormat.ARGBHalf, 2, mipMaps: true);
			}
			displacementMaps = _DisplacementMaps;
			normalMaps = _NormalMaps;
		}
		else
		{
			Water copyFrom = _WindWaves.CopyFrom;
			if (copyFrom.WindWaves.WaterWavesFFT._WindWaves == null)
			{
				copyFrom.WindWaves.ResolveFinalSettings(WaterQualitySettings.Instance.CurrentQualityLevel);
			}
			copyFrom.WindWaves.WaterWavesFFT.ValidateResources();
			displacementMaps = copyFrom.WindWaves.WaterWavesFFT._DisplacementMaps;
			normalMaps = copyFrom.WindWaves.WaterWavesFFT._NormalMaps;
		}
		for (int i = 0; i < 4; i++)
		{
			string text = ((i != 0) ? i.ToString() : "");
			if (displacementMaps != null)
			{
				string name = "_GlobalDisplacementMap" + text;
				_Water.Renderer.PropertyBlock.SetTexture(name, displacementMaps[i]);
			}
			if (i < 2 && normalMaps != null)
			{
				string name2 = "_GlobalNormalMap" + text;
				_Water.Renderer.PropertyBlock.SetTexture(name2, normalMaps[i]);
			}
		}
		if (ResourcesChanged != null)
		{
			ResourcesChanged();
		}
	}

	internal void Enable()
	{
		if (Enabled)
		{
			return;
		}
		Enabled = true;
		OnCopyModeChanged();
		if (Application.isPlaying)
		{
			if (_LastCopyFrom == null)
			{
				ValidateResources();
			}
			_WindWaves.ResolutionChanged.AddListener(OnResolutionChanged);
		}
		_FFTUtilitiesMaterial = new Material(_FFTUtilitiesShader)
		{
			hideFlags = HideFlags.DontSave
		};
	}

	internal void Disable()
	{
		if (Enabled)
		{
			Enabled = false;
			Dispose(total: false);
		}
	}

	internal void OnDestroy()
	{
		Dispose(total: true);
	}
}
