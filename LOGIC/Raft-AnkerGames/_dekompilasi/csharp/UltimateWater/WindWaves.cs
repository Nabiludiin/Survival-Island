using System;
using System.Collections.Generic;
using UltimateWater.Internal;
using UnityEngine;
using UnityEngine.Events;

namespace UltimateWater;

public sealed class WindWaves : WaterModule
{
	[Serializable]
	public class WindWavesEvent : UnityEvent<WindWaves>
	{
	}

	[Serializable]
	public sealed class Data
	{
		public Transform WindDirectionPointer;

		[SerializeField]
		[Tooltip("Higher values increase quality, but also decrease performance. Directly controls quality of waves, foam and spray.")]
		public int Resolution = 256;

		[SerializeField]
		[Tooltip("Determines if 32-bit precision buffers should be used for computations (Default: off). Not supported on most mobile devices. This setting has impact on performance, even on PCs.\n\nTips:\n 1024 and higher - The difference is clearly visible, use this option on higher quality settings.\n 512 or lower - Keep it disabled.")]
		public bool HighPrecision = true;

		[Tooltip("What error in world units is acceptable for elevation sampling used by physics and custom scripts? Lower values mean better precision, but higher CPU usage.")]
		public float CpuDesiredStandardError = 0.12f;

		[Tooltip("Copying wave spectrum from other fluid will make this instance a lot faster.")]
		public Water CopyFrom;

		[Tooltip("Setting this property to any value greater than zero will loop the water spectra in that time. A good value is 10 seconds. Set to 0 to resolve sea state at each frame without any looping (best quality).")]
		public float LoopDuration;

		public WindWavesEvent WindDirectionChanged;

		public WindWavesEvent ResolutionChanged;

		public float MipBias;

		public WavesRendererFFT.Data WavesRendererFFTData;

		public WavesRendererGerstner.Data WavesRendererGerstnerData;
	}

	private readonly Water _Water;

	private readonly Data _Data;

	private Vector4 _TileSizeScales = new Vector4(0.79241f, 0.163151f, 3.175131f, 13.731513f);

	private int _FinalResolution;

	private bool _FinalHighPrecision;

	private float _WindSpeedMagnitude;

	private float _SpectrumDirectionality;

	private float _TileSize;

	private float _LastTileSize = float.NaN;

	private float _LastUniformWaterScale = float.NaN;

	private Vector4 _TileSizes;

	private Vector4 _TileSizesInv;

	private Vector4 _UnscaledTileSizes;

	private Vector2 _WindDirection;

	private Vector2 _WindSpeed;

	private WaveSpectrumRenderMode _FinalRenderMode;

	private SpectrumResolver _SpectrumResolver;

	private Water _RuntimeCopyFrom;

	private bool _IsClone;

	private bool _WindSpeedChanged;

	private bool _HasWindDirectionPointer;

	private WavesRendererFFT _WaterWavesFFT;

	private WavesRendererGerstner _WaterWavesGerstner;

	private DynamicSmoothness _DynamicSmoothness;

	public Water CopyFrom
	{
		get
		{
			return _RuntimeCopyFrom;
		}
		set
		{
			if (_Data.CopyFrom != value || _RuntimeCopyFrom != value)
			{
				_Data.CopyFrom = value;
				_RuntimeCopyFrom = value;
				_IsClone = value != null;
				_DynamicSmoothness.OnCopyModeChanged();
				_WaterWavesFFT.OnCopyModeChanged();
			}
		}
	}

	public SpectrumResolver SpectrumResolver
	{
		get
		{
			if (!(_Data.CopyFrom == null))
			{
				return _Data.CopyFrom.WindWaves._SpectrumResolver;
			}
			return _SpectrumResolver;
		}
	}

	public WavesRendererFFT WaterWavesFFT => _WaterWavesFFT;

	public WavesRendererGerstner WaterWavesGerstner => _WaterWavesGerstner;

	public DynamicSmoothness DynamicSmoothness => _DynamicSmoothness;

	public WaveSpectrumRenderMode FinalRenderMode => _FinalRenderMode;

	public Vector4 TileSizes => _TileSizes;

	public Vector4 TileSizesInv => _TileSizesInv;

	public Vector4 UnscaledTileSizes => _UnscaledTileSizes;

	public Vector2 WindSpeed => _WindSpeed;

	public bool WindSpeedChanged => _WindSpeedChanged;

	public Vector2 WindDirection => _WindDirection;

	public Transform WindDirectionPointer => _Data.WindDirectionPointer;

	public WindWavesEvent WindDirectionChanged => _Data.WindDirectionChanged;

	public WindWavesEvent ResolutionChanged
	{
		get
		{
			if (_Data.ResolutionChanged == null)
			{
				return _Data.ResolutionChanged = new WindWavesEvent();
			}
			return _Data.ResolutionChanged;
		}
	}

	public int Resolution
	{
		get
		{
			return _Data.Resolution;
		}
		set
		{
			if (_Data.Resolution != value)
			{
				_Data.Resolution = value;
				ResolveFinalSettings(WaterQualitySettings.Instance.CurrentQualityLevel);
			}
		}
	}

	public int FinalResolution => _FinalResolution;

	public bool FinalHighPrecision => _FinalHighPrecision;

	public bool HighPrecision => _Data.HighPrecision;

	public float CpuDesiredStandardError => _Data.CpuDesiredStandardError;

	public float LoopDuration => _Data.LoopDuration;

	public Vector4 TileSizeScales => _TileSizeScales;

	public float MaxVerticalDisplacement => _SpectrumResolver.MaxVerticalDisplacement;

	public float MaxHorizontalDisplacement => _SpectrumResolver.MaxHorizontalDisplacement;

	public float SpectrumDirectionality => _SpectrumDirectionality;

	public Vector2 GetHorizontalDisplacementAt(float x, float z, float time)
	{
		return _SpectrumResolver.GetHorizontalDisplacementAt(x, z, time);
	}

	public float GetHeightAt(float x, float z, float time)
	{
		return _SpectrumResolver.GetHeightAt(x, z, time);
	}

	public Vector4 GetForceAndHeightAt(float x, float z, float time)
	{
		return _SpectrumResolver.GetForceAndHeightAt(x, z, time);
	}

	public Vector3 GetDisplacementAt(float x, float z, float time)
	{
		return _SpectrumResolver.GetDisplacementAt(x, z, time);
	}

	public WindWaves(Water water, Data data)
	{
		_Water = water;
		_Data = data;
		_RuntimeCopyFrom = data.CopyFrom;
		_IsClone = _RuntimeCopyFrom != null;
		CheckSupport();
		Validate();
		Shader spectrumShader = Shader.Find("UltimateWater/Spectrum/Water Spectrum");
		if (_SpectrumResolver == null)
		{
			_SpectrumResolver = new SpectrumResolver(water, this, spectrumShader);
		}
		if (data.WindDirectionChanged == null)
		{
			data.WindDirectionChanged = new WindWavesEvent();
		}
		CreateObjects();
		ResolveFinalSettings(WaterQualitySettings.Instance.CurrentQualityLevel);
		if (Application.isPlaying)
		{
			water.ProfilesManager.Changed.AddListener(OnProfilesChanged);
			OnProfilesChanged(water);
		}
	}

	internal override void Validate()
	{
		if (Application.isPlaying)
		{
			CopyFrom = _Data.CopyFrom;
		}
		if (_Data.CpuDesiredStandardError < 1E-05f)
		{
			_Data.CpuDesiredStandardError = 1E-05f;
		}
		_HasWindDirectionPointer = _Data.WindDirectionPointer != null;
		if (_SpectrumResolver != null)
		{
			ResolveFinalSettings(WaterQualitySettings.Instance.CurrentQualityLevel);
			_WaterWavesFFT.Validate();
			_WaterWavesGerstner.OnValidate(this);
		}
		if (_Water != null && _TileSize != 0f)
		{
			UpdateShaderParams();
		}
		if (_WaterWavesFFT != null && _WaterWavesFFT.NormalMaps != null && _WaterWavesFFT.NormalMaps.Length != 0)
		{
			_WaterWavesFFT.GetNormalMap(0).mipMapBias = _Data.MipBias;
			_WaterWavesFFT.GetNormalMap(1).mipMapBias = _Data.MipBias;
		}
	}

	internal override void Update()
	{
		UpdateWind();
		if (_IsClone)
		{
			_TileSize = _RuntimeCopyFrom.WindWaves._TileSize;
			UpdateShaderParams();
		}
		else if (Application.isPlaying)
		{
			_SpectrumResolver.Update();
			_DynamicSmoothness.Update();
			UpdateShaderParams();
		}
	}

	internal void ResolveFinalSettings(WaterQualityLevel quality)
	{
		CreateObjects();
		WaterWavesMode wavesMode = quality.WavesMode;
		if (wavesMode == WaterWavesMode.DisallowAll)
		{
			_WaterWavesFFT.Disable();
			_WaterWavesGerstner.Disable();
			return;
		}
		bool flag = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBFloat) || SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf);
		int num = Mathf.Min(_Data.Resolution, quality.MaxSpectrumResolution, SystemInfo.maxTextureSize);
		bool flag2 = _Data.HighPrecision && quality.AllowHighPrecisionTextures && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBFloat);
		WindWavesRenderMode windWavesMode = _Water.ShaderSet.WindWavesMode;
		if ((windWavesMode == WindWavesRenderMode.FullFFT && wavesMode == WaterWavesMode.AllowAll) & flag)
		{
			_FinalRenderMode = WaveSpectrumRenderMode.FullFFT;
		}
		else if ((windWavesMode <= WindWavesRenderMode.GerstnerAndFFTNormals && wavesMode <= WaterWavesMode.AllowNormalFFT) & flag)
		{
			_FinalRenderMode = WaveSpectrumRenderMode.GerstnerAndFFTNormals;
		}
		else
		{
			_FinalRenderMode = WaveSpectrumRenderMode.Gerstner;
		}
		if (_FinalResolution != num)
		{
			lock (this)
			{
				_FinalResolution = num;
				_FinalHighPrecision = flag2;
				if (_SpectrumResolver != null)
				{
					_SpectrumResolver.OnMapsFormatChanged(resolution: true);
				}
				if (ResolutionChanged != null)
				{
					ResolutionChanged.Invoke(this);
				}
			}
		}
		else if (_FinalHighPrecision != flag2)
		{
			lock (this)
			{
				_FinalHighPrecision = flag2;
				if (_SpectrumResolver != null)
				{
					_SpectrumResolver.OnMapsFormatChanged(resolution: false);
				}
			}
		}
		switch (_FinalRenderMode)
		{
		case WaveSpectrumRenderMode.FullFFT:
			_WaterWavesFFT.RenderedMaps = WavesRendererFFT.MapType.Displacement | WavesRendererFFT.MapType.Normal;
			_WaterWavesFFT.Enable();
			_WaterWavesGerstner.Disable();
			break;
		case WaveSpectrumRenderMode.GerstnerAndFFTNormals:
			_WaterWavesFFT.RenderedMaps = WavesRendererFFT.MapType.Normal;
			_WaterWavesFFT.Enable();
			_WaterWavesGerstner.Enable();
			break;
		case WaveSpectrumRenderMode.Gerstner:
			_WaterWavesFFT.Disable();
			_WaterWavesGerstner.Enable();
			break;
		}
	}

	private void UpdateShaderParams()
	{
		float uniformWaterScale = _Water.UniformWaterScale;
		if (_LastTileSize != _TileSize || _LastUniformWaterScale != uniformWaterScale)
		{
			MaterialPropertyBlock propertyBlock = _Water.Renderer.PropertyBlock;
			float num = _TileSize * uniformWaterScale;
			_TileSizes.x = num * _TileSizeScales.x;
			_TileSizes.y = num * _TileSizeScales.y;
			_TileSizes.z = num * _TileSizeScales.z;
			_TileSizes.w = num * _TileSizeScales.w;
			propertyBlock.SetVector(ShaderVariables.WaterTileSize, _TileSizes);
			_TileSizesInv.x = 1f / _TileSizes.x;
			_TileSizesInv.y = 1f / _TileSizes.y;
			_TileSizesInv.z = 1f / _TileSizes.z;
			_TileSizesInv.w = 1f / _TileSizes.w;
			propertyBlock.SetVector(ShaderVariables.WaterTileSizeInv, _TileSizesInv);
			_LastUniformWaterScale = uniformWaterScale;
			_LastTileSize = _TileSize;
		}
	}

	private void OnProfilesChanged(Water water)
	{
		_TileSize = 0f;
		_WindSpeedMagnitude = 0f;
		_SpectrumDirectionality = 0f;
		List<Water.WeightedProfile> profiles = water.ProfilesManager.Profiles;
		for (int i = 0; i < profiles.Count; i++)
		{
			WaterProfileData profile = profiles[i].Profile;
			float weight = profiles[i].Weight;
			_TileSize += profile.TileSize * profile.TileScale * weight;
			_WindSpeedMagnitude += profile.WindSpeed * weight;
			_SpectrumDirectionality += profile.Directionality * weight;
		}
		WaterQualitySettings instance = WaterQualitySettings.Instance;
		_TileSize *= instance.TileSizeScale;
		_UnscaledTileSizes = _TileSize * _TileSizeScales;
		UpdateShaderParams();
		MaterialPropertyBlock propertyBlock = water.Renderer.PropertyBlock;
		propertyBlock.SetVector(ShaderVariables.WaterTileSizeScales, new Vector4(_TileSizeScales.x / _TileSizeScales.y, _TileSizeScales.x / _TileSizeScales.z, _TileSizeScales.x / _TileSizeScales.w, 0f));
		_SpectrumResolver.OnProfilesChanged();
		propertyBlock.SetFloat(ShaderVariables.MaxDisplacement, _SpectrumResolver.MaxHorizontalDisplacement);
	}

	internal override void Destroy()
	{
		if (_SpectrumResolver != null)
		{
			_SpectrumResolver.OnDestroy();
			_SpectrumResolver = null;
		}
		if (_WaterWavesFFT != null)
		{
			_WaterWavesFFT.OnDestroy();
		}
	}

	private void UpdateWind()
	{
		Vector2 windDirection;
		if (_HasWindDirectionPointer)
		{
			Vector3 forward = _Data.WindDirectionPointer.forward;
			float num = Mathf.Sqrt(forward.x * forward.x + forward.z * forward.z);
			windDirection = new Vector2(forward.x / num, forward.z / num);
		}
		else
		{
			windDirection = new Vector2(1f, 0f);
		}
		Vector2 windSpeed = new Vector2(windDirection.x * _WindSpeedMagnitude, windDirection.y * _WindSpeedMagnitude);
		if (_WindSpeed.x != windSpeed.x || _WindSpeed.y != windSpeed.y)
		{
			_WindDirection = windDirection;
			_WindSpeed = windSpeed;
			_WindSpeedChanged = true;
			_SpectrumResolver.SetWindDirection(_WindDirection);
		}
		else
		{
			_WindSpeedChanged = false;
		}
	}

	private void CreateObjects()
	{
		if (_WaterWavesFFT == null)
		{
			_WaterWavesFFT = new WavesRendererFFT(_Water, this, _Data.WavesRendererFFTData);
		}
		if (_WaterWavesGerstner == null)
		{
			_WaterWavesGerstner = new WavesRendererGerstner(_Water, this, _Data.WavesRendererGerstnerData);
		}
		if (_DynamicSmoothness == null)
		{
			_DynamicSmoothness = new DynamicSmoothness(_Water, this);
		}
	}

	private void CheckSupport()
	{
		if (_Data.HighPrecision && (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGFloat) || !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBFloat)))
		{
			_FinalHighPrecision = false;
		}
		if (!_Data.HighPrecision && (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGHalf) || !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf)))
		{
			if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGFloat))
			{
				_FinalHighPrecision = true;
			}
			else if (_Water.ShaderSet.WindWavesMode == WindWavesRenderMode.FullFFT)
			{
				_FinalRenderMode = WaveSpectrumRenderMode.Gerstner;
			}
		}
	}

	internal override void OnWaterRender(WaterCamera waterCamera)
	{
		if (Application.isPlaying)
		{
			Camera cameraComponent = waterCamera.CameraComponent;
			if (_WaterWavesFFT.Enabled)
			{
				_WaterWavesFFT.OnWaterRender(cameraComponent);
			}
			if (_WaterWavesGerstner.Enabled)
			{
				_WaterWavesGerstner.OnWaterRender(cameraComponent);
			}
		}
	}

	internal override void Enable()
	{
		UpdateWind();
		ResolveFinalSettings(WaterQualitySettings.Instance.CurrentQualityLevel);
	}

	internal override void Disable()
	{
		if (_WaterWavesFFT != null)
		{
			_WaterWavesFFT.Disable();
		}
		if (_WaterWavesGerstner != null)
		{
			_WaterWavesGerstner.Disable();
		}
		if (_DynamicSmoothness != null)
		{
			_DynamicSmoothness.FreeResources();
		}
	}
}
