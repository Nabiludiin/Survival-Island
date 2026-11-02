using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace UltimateWater;

[Serializable]
public class WaterProfileData
{
	public enum WaterSpectrumType
	{
		Phillips,
		Unified
	}

	[SerializeField]
	[FormerlySerializedAs("spectrumType")]
	private WaterSpectrumType _SpectrumType = WaterSpectrumType.Unified;

	[SerializeField]
	[FormerlySerializedAs("windSpeed")]
	private float _WindSpeed = 22f;

	[Tooltip("Tile size in world units of all water maps including heightmap. High values lower overall quality, but low values make the water pattern noticeable.")]
	[SerializeField]
	[FormerlySerializedAs("tileSize")]
	private float _TileSize = 180f;

	[SerializeField]
	[FormerlySerializedAs("tileScale")]
	private float _TileScale = 1f;

	[Tooltip("Setting it to something else than 1.0 will make the spectrum less physically correct, but still may be useful at times.")]
	[SerializeField]
	[FormerlySerializedAs("wavesAmplitude")]
	private float _WavesAmplitude = 1f;

	[SerializeField]
	[FormerlySerializedAs("wavesFrequencyScale")]
	private float _WavesFrequencyScale = 1f;

	[Range(0f, 4f)]
	[SerializeField]
	[FormerlySerializedAs("horizontalDisplacementScale")]
	private float _HorizontalDisplacementScale = 1f;

	[SerializeField]
	[FormerlySerializedAs("phillipsCutoffFactor")]
	private float _PhillipsCutoffFactor = 2000f;

	[SerializeField]
	[FormerlySerializedAs("gravity")]
	private float _Gravity = -9.81f;

	[Tooltip("It is the length of water in meters over which a wind has blown. Usually a distance to the closest land in the direction opposite to the wind.")]
	[SerializeField]
	[FormerlySerializedAs("fetch")]
	private float _Fetch = 100000f;

	[FormerlySerializedAs("directionality")]
	[SerializeField]
	[Range(0f, 1f)]
	[Tooltip("Eliminates waves moving against the wind.")]
	private float _Directionality;

	[ColorUsage(false, true, 0f, 10f, 0f, 10f)]
	[SerializeField]
	[FormerlySerializedAs("absorptionColor")]
	private Color _AbsorptionColor = new Color(0.35f, 0.04f, 0.001f, 1f);

	[SerializeField]
	[FormerlySerializedAs("customUnderwaterAbsorptionColor")]
	private bool _CustomUnderwaterAbsorptionColor = true;

	[Tooltip("Absorption color used by the underwater camera image-effect. Gradient describes color at each depth starting with 0 and ending on 600 units.")]
	[SerializeField]
	[FormerlySerializedAs("absorptionColorByDepth")]
	private Gradient _AbsorptionColorByDepth;

	[SerializeField]
	[FormerlySerializedAs("absorptionColorByDepthFlatGradient")]
	private Gradient _AbsorptionColorByDepthFlatGradient;

	[SerializeField]
	[FormerlySerializedAs("diffuseColor")]
	[ColorUsage(false, true, 0f, 10f, 0f, 10f)]
	private Color _DiffuseColor = new Color(0.1176f, 0.2196f, 0.2666f);

	[ColorUsage(false)]
	[SerializeField]
	[FormerlySerializedAs("specularColor")]
	private Color _SpecularColor = new Color(0.0353f, 0.0471f, 0.0549f);

	[ColorUsage(false)]
	[SerializeField]
	[FormerlySerializedAs("depthColor")]
	private Color _DepthColor = new Color(0f, 0f, 0f);

	[ColorUsage(false)]
	[SerializeField]
	[FormerlySerializedAs("emissionColor")]
	private Color _EmissionColor = new Color(0f, 0f, 0f);

	[ColorUsage(false)]
	[SerializeField]
	[FormerlySerializedAs("reflectionColor")]
	private Color _ReflectionColor = new Color(1f, 1f, 1f);

	[SerializeField]
	[FormerlySerializedAs("smoothness")]
	[Range(0f, 1f)]
	private float _Smoothness = 0.94f;

	[SerializeField]
	[FormerlySerializedAs("customAmbientSmoothness")]
	private bool _CustomAmbientSmoothness;

	[Range(0f, 1f)]
	[SerializeField]
	[FormerlySerializedAs("ambientSmoothness")]
	private float _AmbientSmoothness = 0.94f;

	[Range(0f, 6f)]
	[SerializeField]
	[FormerlySerializedAs("isotropicScatteringIntensity")]
	private float _IsotropicScatteringIntensity = 1f;

	[Range(0f, 6f)]
	[SerializeField]
	[FormerlySerializedAs("forwardScatteringIntensity")]
	private float _ForwardScatteringIntensity = 1f;

	[Range(0f, 1f)]
	[SerializeField]
	[FormerlySerializedAs("subsurfaceScatteringContrast")]
	private float _SubsurfaceScatteringContrast;

	[ColorUsage(false, true, 1f, 8f, 1f, 8f)]
	[SerializeField]
	[FormerlySerializedAs("subsurfaceScatteringShoreColor")]
	private Color _SubsurfaceScatteringShoreColor = new Color(1.4f, 3f, 3f);

	[Range(0f, 1f)]
	[SerializeField]
	[FormerlySerializedAs("refractionDistortion")]
	private float _RefractionDistortion = 0.55f;

	[SerializeField]
	[FormerlySerializedAs("fresnelBias")]
	private float _FresnelBias = 0.02040781f;

	[SerializeField]
	[FormerlySerializedAs("detailFadeDistance")]
	[Range(0.5f, 20f)]
	private float _DetailFadeDistance = 4.5f;

	[Range(0.1f, 10f)]
	[SerializeField]
	[FormerlySerializedAs("displacementNormalsIntensity")]
	private float _DisplacementNormalsIntensity = 2f;

	[Tooltip("Planar reflections are very good solution for calm weather, but you should fade them out for profiles with big waves (storms etc.) as they get completely incorrect then.")]
	[Range(0f, 1f)]
	[SerializeField]
	[FormerlySerializedAs("planarReflectionIntensity")]
	private float _PlanarReflectionIntensity = 0.6f;

	[FormerlySerializedAs("planarReflectionFlatten")]
	[Range(1f, 10f)]
	[SerializeField]
	private float _PlanarReflectionFlatten = 6f;

	[Tooltip("Fixes some artifacts produced by planar reflections at grazing angles.")]
	[Range(0f, 0.008f)]
	[SerializeField]
	[FormerlySerializedAs("planarReflectionVerticalOffset")]
	private float _PlanarReflectionVerticalOffset = 0.0015f;

	[SerializeField]
	[FormerlySerializedAs("edgeBlendFactor")]
	private float _EdgeBlendFactor = 0.15f;

	[SerializeField]
	[FormerlySerializedAs("directionalWrapSSS")]
	private float _DirectionalWrapSss = 0.2f;

	[SerializeField]
	[FormerlySerializedAs("pointWrapSSS")]
	private float _PointWrapSss = 0.5f;

	[Tooltip("Used by the physics.")]
	[SerializeField]
	[FormerlySerializedAs("density")]
	private float _Density = 998.6f;

	[Range(0f, 0.03f)]
	[SerializeField]
	[FormerlySerializedAs("underwaterBlurSize")]
	private float _UnderwaterBlurSize = 0.003f;

	[Range(0f, 2f)]
	[SerializeField]
	[FormerlySerializedAs("underwaterLightFadeScale")]
	private float _UnderwaterLightFadeScale = 0.8f;

	[Range(0f, 0.4f)]
	[SerializeField]
	[FormerlySerializedAs("underwaterDistortionsIntensity")]
	private float _UnderwaterDistortionsIntensity = 0.05f;

	[Range(0.02f, 0.5f)]
	[SerializeField]
	[FormerlySerializedAs("underwaterDistortionAnimationSpeed")]
	private float _UnderwaterDistortionAnimationSpeed = 0.1f;

	[Range(1f, 64f)]
	[SerializeField]
	[FormerlySerializedAs("dynamicSmoothnessIntensity")]
	private float _DynamicSmoothnessIntensity = 1f;

	[SerializeField]
	[FormerlySerializedAs("normalMapAnimation1")]
	private NormalMapAnimation _NormalMapAnimation1 = new NormalMapAnimation(1f, -10f, 1f, new Vector2(1f, 1f));

	[SerializeField]
	[FormerlySerializedAs("normalMapAnimation2")]
	private NormalMapAnimation _NormalMapAnimation2 = new NormalMapAnimation(-0.55f, 20f, 0.74f, new Vector2(1.5f, 1.5f));

	[SerializeField]
	[FormerlySerializedAs("normalMap")]
	private Texture2D _NormalMap;

	[SerializeField]
	[FormerlySerializedAs("foamIntensity")]
	private float _FoamIntensity = 1f;

	[SerializeField]
	[FormerlySerializedAs("foamThreshold")]
	private float _FoamThreshold = 1f;

	[Tooltip("Determines how fast foam will fade out.")]
	[Range(0f, 1f)]
	[SerializeField]
	[FormerlySerializedAs("foamFadingFactor")]
	private float _FoamFadingFactor = 0.85f;

	[Range(0f, 5f)]
	[SerializeField]
	[FormerlySerializedAs("foamShoreIntensity")]
	private float _FoamShoreIntensity = 1f;

	[Range(0f, 5f)]
	[SerializeField]
	[FormerlySerializedAs("foamShoreExtent")]
	private float _FoamShoreExtent = 1f;

	[SerializeField]
	[FormerlySerializedAs("foamNormalScale")]
	private float _FoamNormalScale = 2.2f;

	[ColorUsage(false)]
	[SerializeField]
	[FormerlySerializedAs("foamDiffuseColor")]
	private Color _FoamDiffuseColor = new Color(0.8f, 0.8f, 0.8f);

	[Tooltip("Alpha component is PBR smoothness.")]
	[SerializeField]
	[FormerlySerializedAs("foamSpecularColor")]
	private Color _FoamSpecularColor = new Color(1f, 1f, 1f, 0f);

	[Range(0f, 4f)]
	[SerializeField]
	[FormerlySerializedAs("sprayThreshold")]
	private float _SprayThreshold = 1f;

	[Range(0f, 0.999f)]
	[SerializeField]
	[FormerlySerializedAs("spraySkipRatio")]
	private float _SpraySkipRatio = 0.9f;

	[SerializeField]
	[FormerlySerializedAs("spraySize")]
	[Range(0.25f, 4f)]
	private float _SpraySize = 1f;

	[SerializeField]
	[FormerlySerializedAs("foamDiffuseMap")]
	private Texture2D _FoamDiffuseMap;

	[SerializeField]
	[FormerlySerializedAs("foamNormalMap")]
	private Texture2D _FoamNormalMap;

	[SerializeField]
	[FormerlySerializedAs("foamTiling")]
	private Vector2 _FoamTiling = new Vector2(5.4f, 5.4f);

	[SerializeField]
	[HideInInspector]
	private WaterProfile _Template;

	private WaterWavesSpectrum _Spectrum;

	public bool Dirty;

	public WaterProfileData Template => _Template.Data;

	public WaterProfile TemplateProfile
	{
		set
		{
			_Template = value;
		}
	}

	public bool IsTemplate => _Template == null;

	public WaterSpectrumType SpectrumType => _SpectrumType;

	public WaterWavesSpectrum Spectrum
	{
		get
		{
			if (_Spectrum == null)
			{
				CreateSpectrum();
			}
			return _Spectrum;
		}
	}

	public float WindSpeed => _WindSpeed;

	public float TileSize => _TileSize;

	public float TileScale => _TileScale;

	public float HorizontalDisplacementScale
	{
		get
		{
			return _HorizontalDisplacementScale;
		}
		set
		{
			_HorizontalDisplacementScale = value;
			Dirty = true;
		}
	}

	public float Gravity
	{
		get
		{
			return _Gravity;
		}
		set
		{
			_Gravity = value;
			Dirty = true;
		}
	}

	public float Directionality
	{
		get
		{
			return _Directionality;
		}
		set
		{
			_Directionality = value;
			Dirty = true;
		}
	}

	public Color AbsorptionColor
	{
		get
		{
			return _AbsorptionColor;
		}
		set
		{
			_AbsorptionColor = value;
			Dirty = true;
		}
	}

	public Color DiffuseColor
	{
		get
		{
			return _DiffuseColor;
		}
		set
		{
			_DiffuseColor = value;
			Dirty = true;
		}
	}

	public Color SpecularColor
	{
		get
		{
			return _SpecularColor;
		}
		set
		{
			_SpecularColor = value;
			Dirty = true;
		}
	}

	public Color DepthColor
	{
		get
		{
			return _DepthColor;
		}
		set
		{
			_DepthColor = value;
			Dirty = true;
		}
	}

	public Color EmissionColor
	{
		get
		{
			return _EmissionColor;
		}
		set
		{
			_EmissionColor = value;
			Dirty = true;
		}
	}

	public Color ReflectionColor
	{
		get
		{
			return _ReflectionColor;
		}
		set
		{
			_ReflectionColor = value;
			Dirty = true;
		}
	}

	public float Smoothness
	{
		get
		{
			return _Smoothness;
		}
		set
		{
			_Smoothness = value;
			Dirty = true;
		}
	}

	public bool CustomAmbientSmoothness
	{
		get
		{
			return _CustomAmbientSmoothness;
		}
		set
		{
			_CustomAmbientSmoothness = value;
			Dirty = true;
		}
	}

	public float AmbientSmoothness
	{
		get
		{
			if (!_CustomAmbientSmoothness)
			{
				return _Smoothness;
			}
			return _AmbientSmoothness;
		}
		set
		{
			_AmbientSmoothness = value;
			Dirty = true;
		}
	}

	public float IsotropicScatteringIntensity
	{
		get
		{
			return _IsotropicScatteringIntensity;
		}
		set
		{
			_IsotropicScatteringIntensity = value;
			Dirty = true;
		}
	}

	public float ForwardScatteringIntensity
	{
		get
		{
			return _ForwardScatteringIntensity;
		}
		set
		{
			_ForwardScatteringIntensity = value;
			Dirty = true;
		}
	}

	public float SubsurfaceScatteringContrast
	{
		get
		{
			return _SubsurfaceScatteringContrast;
		}
		set
		{
			_SubsurfaceScatteringContrast = value;
			Dirty = true;
		}
	}

	public Color SubsurfaceScatteringShoreColor
	{
		get
		{
			return _SubsurfaceScatteringShoreColor;
		}
		set
		{
			_SubsurfaceScatteringShoreColor = value;
			Dirty = true;
		}
	}

	public float RefractionDistortion
	{
		get
		{
			return _RefractionDistortion;
		}
		set
		{
			_RefractionDistortion = value;
			Dirty = true;
		}
	}

	public float FresnelBias
	{
		get
		{
			return _FresnelBias;
		}
		set
		{
			_FresnelBias = value;
			Dirty = true;
		}
	}

	public float DetailFadeDistance
	{
		get
		{
			return _DetailFadeDistance * _DetailFadeDistance;
		}
		set
		{
			_DetailFadeDistance = value;
			Dirty = true;
		}
	}

	public float DisplacementNormalsIntensity
	{
		get
		{
			return _DisplacementNormalsIntensity;
		}
		set
		{
			_DisplacementNormalsIntensity = value;
			Dirty = true;
		}
	}

	public float PlanarReflectionIntensity
	{
		get
		{
			return _PlanarReflectionIntensity;
		}
		set
		{
			_PlanarReflectionIntensity = value;
			Dirty = true;
		}
	}

	public float PlanarReflectionFlatten
	{
		get
		{
			return _PlanarReflectionFlatten;
		}
		set
		{
			_PlanarReflectionFlatten = value;
			Dirty = true;
		}
	}

	public float PlanarReflectionVerticalOffset
	{
		get
		{
			return _PlanarReflectionVerticalOffset;
		}
		set
		{
			_WindSpeed = value;
			Dirty = true;
		}
	}

	public float EdgeBlendFactor
	{
		get
		{
			return _EdgeBlendFactor;
		}
		set
		{
			_EdgeBlendFactor = value;
			Dirty = true;
		}
	}

	public float DirectionalWrapSSS
	{
		get
		{
			return _DirectionalWrapSss;
		}
		set
		{
			_DirectionalWrapSss = value;
			Dirty = true;
		}
	}

	public float PointWrapSSS
	{
		get
		{
			return _PointWrapSss;
		}
		set
		{
			_PointWrapSss = value;
			Dirty = true;
		}
	}

	public float DynamicSmoothnessIntensity
	{
		get
		{
			return _DynamicSmoothnessIntensity;
		}
		set
		{
			_DynamicSmoothnessIntensity = value;
			Dirty = true;
		}
	}

	public float Density
	{
		get
		{
			return _Density;
		}
		set
		{
			_Density = value;
			Dirty = true;
		}
	}

	public float UnderwaterBlurSize
	{
		get
		{
			return _UnderwaterBlurSize;
		}
		set
		{
			_UnderwaterBlurSize = value;
			Dirty = true;
		}
	}

	public float UnderwaterLightFadeScale
	{
		get
		{
			return _UnderwaterLightFadeScale;
		}
		set
		{
			_UnderwaterLightFadeScale = value;
			Dirty = true;
		}
	}

	public float UnderwaterDistortionsIntensity
	{
		get
		{
			return _UnderwaterDistortionsIntensity;
		}
		set
		{
			_UnderwaterDistortionsIntensity = value;
			Dirty = true;
		}
	}

	public float UnderwaterDistortionAnimationSpeed
	{
		get
		{
			return _UnderwaterDistortionAnimationSpeed;
		}
		set
		{
			_UnderwaterDistortionAnimationSpeed = value;
			Dirty = true;
		}
	}

	public NormalMapAnimation NormalMapAnimation1
	{
		get
		{
			return _NormalMapAnimation1;
		}
		set
		{
			_NormalMapAnimation1 = value;
			Dirty = true;
		}
	}

	public NormalMapAnimation NormalMapAnimation2
	{
		get
		{
			return _NormalMapAnimation2;
		}
		set
		{
			_NormalMapAnimation2 = value;
			Dirty = true;
		}
	}

	public float FoamIntensity
	{
		get
		{
			return _FoamIntensity;
		}
		set
		{
			_FoamIntensity = value;
			Dirty = true;
		}
	}

	public float FoamThreshold
	{
		get
		{
			return _FoamThreshold;
		}
		set
		{
			_FoamThreshold = value;
			Dirty = true;
		}
	}

	public float FoamFadingFactor
	{
		get
		{
			return _FoamFadingFactor;
		}
		set
		{
			_FoamFadingFactor = value;
			Dirty = true;
		}
	}

	public float FoamShoreIntensity
	{
		get
		{
			return _FoamShoreIntensity;
		}
		set
		{
			_FoamShoreIntensity = value;
			Dirty = true;
		}
	}

	public float FoamShoreExtent
	{
		get
		{
			return _FoamShoreExtent;
		}
		set
		{
			_FoamShoreExtent = value;
			Dirty = true;
		}
	}

	public float FoamNormalScale
	{
		get
		{
			return _FoamNormalScale;
		}
		set
		{
			_FoamNormalScale = value;
			Dirty = true;
		}
	}

	public Color FoamDiffuseColor
	{
		get
		{
			return _FoamDiffuseColor;
		}
		set
		{
			_FoamDiffuseColor = value;
			Dirty = true;
		}
	}

	public Color FoamSpecularColor
	{
		get
		{
			return _FoamSpecularColor;
		}
		set
		{
			_FoamSpecularColor = value;
			Dirty = true;
		}
	}

	public float SprayThreshold
	{
		get
		{
			return _SprayThreshold;
		}
		set
		{
			_SprayThreshold = value;
			Dirty = true;
		}
	}

	public float SpraySkipRatio
	{
		get
		{
			return _SpraySkipRatio;
		}
		set
		{
			_SpraySkipRatio = value;
			Dirty = true;
		}
	}

	public float SpraySize
	{
		get
		{
			return _SpraySize;
		}
		set
		{
			_SpraySize = value;
			Dirty = true;
		}
	}

	public Texture2D NormalMap
	{
		get
		{
			return _NormalMap;
		}
		set
		{
			_NormalMap = value;
			Dirty = true;
		}
	}

	public Texture2D FoamDiffuseMap
	{
		get
		{
			return _FoamDiffuseMap;
		}
		set
		{
			_FoamDiffuseMap = value;
			Dirty = true;
		}
	}

	public Texture2D FoamNormalMap
	{
		get
		{
			return _FoamNormalMap;
		}
		set
		{
			_FoamNormalMap = value;
			Dirty = true;
		}
	}

	public Vector2 FoamTiling
	{
		get
		{
			return _FoamTiling;
		}
		set
		{
			_FoamTiling = value;
			Dirty = true;
		}
	}

	public float WavesFrequencyScale
	{
		get
		{
			return _WavesFrequencyScale;
		}
		set
		{
			_WavesFrequencyScale = value;
			Dirty = true;
		}
	}

	public Gradient AbsorptionColorByDepth
	{
		get
		{
			if (!_CustomUnderwaterAbsorptionColor)
			{
				return _AbsorptionColorByDepthFlatGradient;
			}
			return _AbsorptionColorByDepth;
		}
		set
		{
			_AbsorptionColorByDepth = value;
			Dirty = true;
		}
	}

	public void Synchronize()
	{
		Copy(Template);
	}

	public void Copy(WaterProfileData other)
	{
		_Spectrum = other._Spectrum;
		_SpectrumType = other._SpectrumType;
		_WindSpeed = other._WindSpeed;
		_TileSize = other._TileSize;
		_TileScale = other._TileScale;
		_WavesAmplitude = other._WavesAmplitude;
		_WavesFrequencyScale = other._WavesFrequencyScale;
		_HorizontalDisplacementScale = other._HorizontalDisplacementScale;
		_PhillipsCutoffFactor = other._PhillipsCutoffFactor;
		_Gravity = other._Gravity;
		_Fetch = other._Fetch;
		_Directionality = other._Directionality;
		_AbsorptionColor = other._AbsorptionColor;
		_CustomUnderwaterAbsorptionColor = other._CustomUnderwaterAbsorptionColor;
		_AbsorptionColorByDepth = other._AbsorptionColorByDepth;
		_AbsorptionColorByDepthFlatGradient = other._AbsorptionColorByDepthFlatGradient;
		_DiffuseColor = other._DiffuseColor;
		_SpecularColor = other._SpecularColor;
		_DepthColor = other._DepthColor;
		_EmissionColor = other._EmissionColor;
		_ReflectionColor = other._ReflectionColor;
		_Smoothness = other._Smoothness;
		_CustomAmbientSmoothness = other._CustomAmbientSmoothness;
		_AmbientSmoothness = other._AmbientSmoothness;
		_IsotropicScatteringIntensity = other._IsotropicScatteringIntensity;
		_ForwardScatteringIntensity = other._ForwardScatteringIntensity;
		_SubsurfaceScatteringContrast = other._SubsurfaceScatteringContrast;
		_SubsurfaceScatteringShoreColor = other._SubsurfaceScatteringShoreColor;
		_RefractionDistortion = other._RefractionDistortion;
		_FresnelBias = other._FresnelBias;
		_DetailFadeDistance = other._DetailFadeDistance;
		_DisplacementNormalsIntensity = other._DisplacementNormalsIntensity;
		_PlanarReflectionIntensity = other._PlanarReflectionIntensity;
		_PlanarReflectionFlatten = other._PlanarReflectionFlatten;
		_PlanarReflectionVerticalOffset = other._PlanarReflectionVerticalOffset;
		_EdgeBlendFactor = other._EdgeBlendFactor;
		_DirectionalWrapSss = other._DirectionalWrapSss;
		_PointWrapSss = other._PointWrapSss;
		_Density = other._Density;
		_UnderwaterBlurSize = other._UnderwaterBlurSize;
		_UnderwaterLightFadeScale = other._UnderwaterLightFadeScale;
		_UnderwaterDistortionsIntensity = other._UnderwaterDistortionsIntensity;
		_UnderwaterDistortionAnimationSpeed = other._UnderwaterDistortionAnimationSpeed;
		_DynamicSmoothnessIntensity = other._DynamicSmoothnessIntensity;
		_NormalMapAnimation1 = other._NormalMapAnimation1;
		_NormalMapAnimation2 = other._NormalMapAnimation2;
		_NormalMap = other._NormalMap;
		_FoamIntensity = other._FoamIntensity;
		_FoamThreshold = other._FoamThreshold;
		_FoamFadingFactor = other._FoamFadingFactor;
		_FoamShoreIntensity = other._FoamShoreIntensity;
		_FoamShoreExtent = other._FoamShoreExtent;
		_FoamNormalScale = other._FoamNormalScale;
		_FoamDiffuseColor = other._FoamDiffuseColor;
		_FoamSpecularColor = other._FoamSpecularColor;
		_SprayThreshold = other._SprayThreshold;
		_SpraySkipRatio = other._SpraySkipRatio;
		_SpraySize = other._SpraySize;
		_FoamDiffuseMap = other._FoamDiffuseMap;
		_FoamNormalMap = other._FoamNormalMap;
		_FoamTiling = other._FoamTiling;
	}

	public void CreateSpectrum()
	{
		switch (_SpectrumType)
		{
		case WaterSpectrumType.Unified:
			_Spectrum = new UnifiedSpectrum(_TileSize, 0f - _Gravity, _WindSpeed, _WavesAmplitude, _WavesFrequencyScale, _Fetch);
			break;
		case WaterSpectrumType.Phillips:
			_Spectrum = new PhillipsSpectrum(_TileSize, 0f - _Gravity, _WindSpeed, _WavesAmplitude, _PhillipsCutoffFactor);
			break;
		}
	}
}
