using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace UltimateWater;

[Serializable]
public class ShaderSet : ScriptableObject
{
	[Header("Reflection & Refraction")]
	[SerializeField]
	[FormerlySerializedAs("transparencyMode")]
	private WaterTransparencyMode _TransparencyMode = WaterTransparencyMode.Refractive;

	[SerializeField]
	[FormerlySerializedAs("reflectionProbeUsage")]
	private ReflectionProbeUsage _ReflectionProbeUsage = ReflectionProbeUsage.BlendProbesAndSkybox;

	[SerializeField]
	[FormerlySerializedAs("planarReflections")]
	private PlanarReflectionsMode _PlanarReflections = PlanarReflectionsMode.Normal;

	[SerializeField]
	[FormerlySerializedAs("receiveShadows")]
	[Tooltip("Affects direct light specular and diffuse components. Shadows currently work only for main directional light and you need to attach WaterShadowCastingLight script to it. Also it doesn't work at all on mobile platforms.")]
	private bool _ReceiveShadows;

	[Header("Waves")]
	[SerializeField]
	[FormerlySerializedAs("windWavesMode")]
	private WindWavesRenderMode _WindWavesMode;

	[SerializeField]
	[FormerlySerializedAs("dynamicSmoothnessMode")]
	private DynamicSmoothnessMode _DynamicSmoothnessMode = DynamicSmoothnessMode.Physical;

	[SerializeField]
	[FormerlySerializedAs("localEffectsSupported")]
	private bool _LocalEffectsSupported = true;

	[SerializeField]
	[FormerlySerializedAs("localEffectsDebug")]
	private bool _LocalEffectsDebug;

	[SerializeField]
	[FormerlySerializedAs("foam")]
	private bool _Foam = true;

	[Header("Render Modes")]
	[SerializeField]
	[FormerlySerializedAs("forwardRenderMode")]
	private bool _ForwardRenderMode;

	[SerializeField]
	[FormerlySerializedAs("deferredRenderMode")]
	private bool _DeferredRenderMode;

	[Header("Geometries Support")]
	[SerializeField]
	[FormerlySerializedAs("projectionGrid")]
	private bool _ProjectionGrid;

	[SerializeField]
	[FormerlySerializedAs("customTriangularGeometry")]
	private bool _CustomTriangularGeometry;

	[Header("Volumes")]
	[SerializeField]
	[FormerlySerializedAs("displayOnlyInAdditiveVolumes")]
	private bool _DisplayOnlyInAdditiveVolumes;

	[SerializeField]
	[FormerlySerializedAs("wavesAlign")]
	private bool _WavesAlign;

	[Header("Surface")]
	[SerializeField]
	[FormerlySerializedAs("normalMappingMode")]
	private NormalMappingMode _NormalMappingMode = NormalMappingMode.Auto;

	[SerializeField]
	[FormerlySerializedAs("supportEmission")]
	private bool _SupportEmission;

	[Header("Generated Shaders")]
	[SerializeField]
	[FormerlySerializedAs("surfaceShaders")]
	private Shader[] _SurfaceShaders;

	[SerializeField]
	[FormerlySerializedAs("volumeShaders")]
	private Shader[] _VolumeShaders;

	[SerializeField]
	[FormerlySerializedAs("utilityShaders")]
	private Shader[] _UtilityShaders;

	[SerializeField]
	[FormerlySerializedAs("computeShaders")]
	private ComputeShader[] _ComputeShaders;

	private bool _Rebuilding;

	private static bool _ErrorDisplayed;

	public WaterTransparencyMode TransparencyMode
	{
		get
		{
			return _TransparencyMode;
		}
		set
		{
			_TransparencyMode = value;
		}
	}

	public ReflectionProbeUsage ReflectionProbeUsage
	{
		get
		{
			return _ReflectionProbeUsage;
		}
		set
		{
			_ReflectionProbeUsage = value;
		}
	}

	public bool ReceiveShadows
	{
		get
		{
			return _ReceiveShadows;
		}
		set
		{
			_ReceiveShadows = value;
		}
	}

	public PlanarReflectionsMode PlanarReflections
	{
		get
		{
			return _PlanarReflections;
		}
		set
		{
			_PlanarReflections = value;
		}
	}

	public WindWavesRenderMode WindWavesMode
	{
		get
		{
			return _WindWavesMode;
		}
		set
		{
			_WindWavesMode = value;
		}
	}

	public Shader[] SurfaceShaders => _SurfaceShaders;

	public Shader[] VolumeShaders => _VolumeShaders;

	public bool LocalEffectsSupported => _LocalEffectsSupported;

	public bool Foam => _Foam;

	public bool LocalEffectsDebug => _LocalEffectsDebug;

	public bool CustomTriangularGeometry => _CustomTriangularGeometry;

	public bool ProjectionGrid => _ProjectionGrid;

	public DynamicSmoothnessMode SmoothnessMode => _DynamicSmoothnessMode;

	public static Shader GetRuntimeShaderVariant(string keywordsString, bool volume)
	{
		Shader shader = Shader.Find("UltimateWater/Variations/Water " + (volume ? "Volume " : "") + keywordsString);
		if (shader == null && !_ErrorDisplayed && Application.isPlaying)
		{
			Debug.LogError("Could not find proper water shader variation. Select your water and click \"Rebuild shaders\" from its context menu to build proper shaders. Missing shader: \"UltimateWater/Variations/Water " + (volume ? "Volume " : "") + keywordsString + "\"");
			_ErrorDisplayed = true;
		}
		return shader;
	}

	public Shader GetShaderVariant(string[] localKeywords, string[] sharedKeywords, string additionalCode, string keywordsString, bool volume)
	{
		Array.Sort(localKeywords);
		Array.Sort(sharedKeywords);
		string text = (volume ? "Volume " : "") + keywordsString;
		return Shader.Find("UltimateWater/Variations/Water " + text);
	}

	public void FindBestShaders(out Shader surfaceShader, out Shader volumeShader)
	{
		ShaderVariant shaderVariant = new ShaderVariant();
		BuildShaderVariant(shaderVariant, WaterQualitySettings.Instance.CurrentQualityLevel);
		string[] array = shaderVariant.GetKeywordsString().Split(' ');
		surfaceShader = null;
		volumeShader = null;
		if (_SurfaceShaders != null)
		{
			for (int i = 0; i < _SurfaceShaders.Length; i++)
			{
				if (_SurfaceShaders[i] == null)
				{
					continue;
				}
				string text = _SurfaceShaders[i].name;
				for (int j = 0; j < array.Length; j++)
				{
					if (text.Contains(array[j]))
					{
						surfaceShader = _SurfaceShaders[i];
						break;
					}
				}
				if (surfaceShader != null)
				{
					break;
				}
			}
		}
		if (_VolumeShaders == null)
		{
			return;
		}
		for (int k = 0; k < _VolumeShaders.Length; k++)
		{
			if (_VolumeShaders[k] == null)
			{
				continue;
			}
			string text2 = _VolumeShaders[k].name;
			for (int l = 0; l < array.Length; l++)
			{
				if (text2.Contains(array[l]))
				{
					volumeShader = _VolumeShaders[k];
					break;
				}
			}
			if (volumeShader != null)
			{
				break;
			}
		}
	}

	[ContextMenu("Rebuild shaders")]
	public void Build()
	{
	}

	public bool ContainsShaderVariant(string keywordsString)
	{
		if (_SurfaceShaders != null)
		{
			for (int num = _SurfaceShaders.Length - 1; num >= 0; num--)
			{
				Shader shader = _SurfaceShaders[num];
				if (shader != null && shader.name.EndsWith(keywordsString))
				{
					return true;
				}
			}
		}
		if (_VolumeShaders != null)
		{
			for (int num2 = _VolumeShaders.Length - 1; num2 >= 0; num2--)
			{
				Shader shader2 = _VolumeShaders[num2];
				if (shader2 != null && shader2.name.EndsWith(keywordsString))
				{
					return true;
				}
			}
		}
		return false;
	}

	public ComputeShader GetComputeShader(string shaderName)
	{
		for (int i = 0; i < _ComputeShaders.Length; i++)
		{
			if (_ComputeShaders[i].name.Contains(shaderName))
			{
				return _ComputeShaders[i];
			}
		}
		return null;
	}

	private static void ValidateWaterObjects()
	{
		Water[] array = UnityEngine.Object.FindObjectsOfType<Water>();
		for (int num = array.Length - 1; num >= 0; num--)
		{
			array[num].ResetWater();
		}
	}

	private static void SetProgress(float progress)
	{
	}

	private void AddShader(Shader shader, bool volumeShader)
	{
		if (volumeShader)
		{
			if (_VolumeShaders != null)
			{
				Array.Resize(ref _VolumeShaders, _VolumeShaders.Length + 1);
				_VolumeShaders[_VolumeShaders.Length - 1] = shader;
			}
			else
			{
				_VolumeShaders = new Shader[1] { shader };
			}
		}
		else if (_SurfaceShaders != null)
		{
			Array.Resize(ref _SurfaceShaders, _SurfaceShaders.Length + 1);
			_SurfaceShaders[_SurfaceShaders.Length - 1] = shader;
		}
		else
		{
			_SurfaceShaders = new Shader[1] { shader };
		}
	}

	private void BuildShaderVariant(ShaderVariant variant, WaterQualityLevel qualityLevel)
	{
		bool flag = _TransparencyMode == WaterTransparencyMode.Refractive && qualityLevel.AllowAlphaBlending;
		variant.SetWaterKeyword("_WATER_REFRACTION", flag);
		variant.SetWaterKeyword("_CUBEMAP_REFLECTIONS", _ReflectionProbeUsage != ReflectionProbeUsage.Off);
		variant.SetWaterKeyword("_WATER_RECEIVE_SHADOWS", _ReceiveShadows);
		variant.SetWaterKeyword("_ALPHABLEND_ON", flag);
		variant.SetWaterKeyword("_ALPHAPREMULTIPLY_ON", !flag);
		variant.SetUnityKeyword("_TRIANGLES", _CustomTriangularGeometry);
		if (_ProjectionGrid)
		{
			variant.SetAdditionalSurfaceCode("_PROJECTION_GRID", "\t\t\t#pragma multi_compile _PROJECTION_GRID_OFF _PROJECTION_GRID");
		}
		variant.SetUnityKeyword("_WATER_OVERLAYS", _LocalEffectsSupported);
		variant.SetUnityKeyword("_LOCAL_MAPS_DEBUG", _LocalEffectsSupported && _LocalEffectsDebug);
		WindWavesRenderMode windWavesRenderMode = BuildWindWavesVariant(variant, qualityLevel);
		variant.SetWaterKeyword("_WATER_FOAM_WS", _Foam && !_LocalEffectsSupported && windWavesRenderMode == WindWavesRenderMode.FullFFT);
		variant.SetUnityKeyword("_BOUNDED_WATER", _DisplayOnlyInAdditiveVolumes);
		variant.SetUnityKeyword("_WAVES_ALIGN", _WavesAlign);
		variant.SetWaterKeyword("_NORMALMAP", _NormalMappingMode == NormalMappingMode.Always || (_NormalMappingMode == NormalMappingMode.Auto && windWavesRenderMode > WindWavesRenderMode.GerstnerAndFFTNormals));
		variant.SetWaterKeyword("_EMISSION", _SupportEmission);
		variant.SetWaterKeyword("_PLANAR_REFLECTIONS", _PlanarReflections == PlanarReflectionsMode.Normal);
		variant.SetWaterKeyword("_PLANAR_REFLECTIONS_HQ", _PlanarReflections == PlanarReflectionsMode.HighQuality);
	}

	private WindWavesRenderMode BuildWindWavesVariant(ShaderVariant variant, WaterQualityLevel qualityLevel)
	{
		WaterWavesMode wavesMode = qualityLevel.WavesMode;
		WindWavesRenderMode windWavesRenderMode = ((_WindWavesMode == WindWavesRenderMode.Disabled || wavesMode == WaterWavesMode.DisallowAll) ? WindWavesRenderMode.Disabled : ((_WindWavesMode != WindWavesRenderMode.FullFFT || wavesMode != WaterWavesMode.AllowAll) ? ((_WindWavesMode <= WindWavesRenderMode.GerstnerAndFFTNormals && wavesMode <= WaterWavesMode.AllowNormalFFT) ? WindWavesRenderMode.GerstnerAndFFTNormals : WindWavesRenderMode.Gerstner) : WindWavesRenderMode.FullFFT));
		switch (windWavesRenderMode)
		{
		case WindWavesRenderMode.FullFFT:
			variant.SetUnityKeyword("_WAVES_FFT", value: true);
			break;
		case WindWavesRenderMode.GerstnerAndFFTNormals:
			variant.SetWaterKeyword("_WAVES_FFT_NORMAL", value: true);
			variant.SetUnityKeyword("_WAVES_GERSTNER", value: true);
			break;
		case WindWavesRenderMode.Gerstner:
			variant.SetUnityKeyword("_WAVES_GERSTNER", value: true);
			break;
		}
		if (_DynamicSmoothnessMode == DynamicSmoothnessMode.Physical)
		{
			variant.SetWaterKeyword("_INCLUDE_SLOPE_VARIANCE", value: true);
		}
		return windWavesRenderMode;
	}
}
