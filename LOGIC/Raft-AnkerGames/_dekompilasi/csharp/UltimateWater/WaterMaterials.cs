using System;
using System.Collections.Generic;
using UltimateWater.Internal;
using UnityEngine;

namespace UltimateWater;

[Serializable]
public class WaterMaterials
{
	public enum ColorParameter
	{
		AbsorptionColor,
		DiffuseColor,
		SpecularColor,
		DepthColor,
		EmissionColor,
		ReflectionColor
	}

	public enum FloatParameter
	{
		DisplacementScale = 6,
		Glossiness = 7,
		RefractionDistortion = 10,
		SpecularFresnelBias = 11,
		DisplacementNormalsIntensity = 13,
		EdgeBlendFactorInv = 14,
		LightSmoothnessMultiplier = 18
	}

	public enum VectorParameter
	{
		SubsurfaceScatteringPack = 8,
		WrapSubsurfaceScatteringPack = 9,
		DetailFadeFactor = 12,
		PlanarReflectionPack = 15,
		BumpScale = 16,
		FoamTiling = 17
	}

	public enum TextureParameter
	{
		BumpMap = 19,
		FoamTex,
		FoamNormalMap
	}

	public abstract class AWaterParameter<T>
	{
		public readonly int Hash;

		protected readonly MaterialPropertyBlock _PropertyBlock;

		protected T _Value;

		public abstract T Value { get; set; }

		protected AWaterParameter(MaterialPropertyBlock propertyBlock, int hash, T value)
		{
			_PropertyBlock = propertyBlock;
			Hash = hash;
			_Value = value;
		}
	}

	public class WaterParameterFloat : AWaterParameter<float>
	{
		public override float Value
		{
			get
			{
				return _Value;
			}
			set
			{
				_Value = value;
				_PropertyBlock.SetFloat(Hash, value);
			}
		}

		public WaterParameterFloat(MaterialPropertyBlock propertyBlock, int hash, float value)
			: base(propertyBlock, hash, value)
		{
		}
	}

	public class WaterParameterVector4 : AWaterParameter<Vector4>
	{
		public override Vector4 Value
		{
			get
			{
				return _Value;
			}
			set
			{
				_Value = value;
				_PropertyBlock.SetVector(Hash, value);
			}
		}

		public WaterParameterVector4(MaterialPropertyBlock propertyBlock, int hash, Vector4 value)
			: base(propertyBlock, hash, value)
		{
		}
	}

	[SerializeField]
	private float _DirectionalLightsIntensity = 1f;

	[SerializeField]
	[Tooltip("May hurt performance on some systems.")]
	[Range(0f, 1f)]
	private float _TesselationFactor = 1f;

	private Water _Water;

	private float _Smoothness;

	private float _AmbientSmoothness;

	private float _ForwardScatteringIntensity;

	private Vector4 _LastSurfaceOffset;

	private Texture2D _AbsorptionGradient;

	private bool _AbsorptionGradientDirty = true;

	private int _AlphaBlendMode;

	private WaterParameterFloat[] _FloatOverrides = new WaterParameterFloat[0];

	private WaterParameterVector4[] _VectorOverrides = new WaterParameterVector4[0];

	private const int _GradientResolution = 512;

	private static readonly Color[] _AbsorptionColorsBuffer = new Color[512];

	private static int[] _ParameterHashes;

	private static Texture2D _GlobalWaterLookupTex;

	private static bool _GlobalLookupDirty;

	private static readonly string[] _ParameterNames = new string[25]
	{
		"_AbsorptionColor", "_Color", "_SpecColor", "_DepthColor", "_EmissionColor", "_ReflectionColor", "_DisplacementsScale", "_Glossiness", "_SubsurfaceScatteringPack", "_WrapSubsurfaceScatteringPack",
		"_RefractionDistortion", "_SubsurfaceScatteringShoreColor", "_DetailFadeFactor", "_DisplacementNormalsIntensity", "_EdgeBlendFactorInv", "_PlanarReflectionPack", "_BumpScale", "_FoamTiling", "_LightSmoothnessMul", "_BumpMap",
		"_FoamTex", "_FoamNormalMap", "_FoamSpecularColor", "_RefractionMaxDepth", "_FoamDiffuseColor"
	};

	public Material SurfaceMaterial { get; private set; }

	public Material SurfaceBackMaterial { get; private set; }

	public Material VolumeMaterial { get; private set; }

	public Material VolumeBackMaterial { get; private set; }

	public Texture2D UnderwaterAbsorptionColorByDepth
	{
		get
		{
			if (_AbsorptionGradientDirty)
			{
				ComputeAbsorptionGradient();
			}
			return _AbsorptionGradient;
		}
	}

	public float HorizontalDisplacementScale { get; private set; }

	public string UsedKeywords { get; private set; }

	public float UnderwaterBlurSize { get; private set; }

	public float UnderwaterLightFadeScale { get; private set; }

	public float UnderwaterDistortionsIntensity { get; private set; }

	public float UnderwaterDistortionAnimationSpeed { get; private set; }

	public void SetKeyword(string keyword, bool enable)
	{
		if (enable)
		{
			SurfaceMaterial.EnableKeyword(keyword);
			SurfaceBackMaterial.EnableKeyword(keyword);
			VolumeMaterial.EnableKeyword(keyword);
			VolumeBackMaterial.EnableKeyword(keyword);
		}
		else
		{
			SurfaceMaterial.DisableKeyword(keyword);
			SurfaceBackMaterial.DisableKeyword(keyword);
			VolumeMaterial.DisableKeyword(keyword);
			VolumeBackMaterial.DisableKeyword(keyword);
		}
	}

	public void UpdateGlobalLookupTex()
	{
		int waterId = _Water.WaterId;
		if (!(_GlobalWaterLookupTex == null) && waterId != -1)
		{
			if (waterId >= 256)
			{
				Debug.LogError("There is more than 256 water objects in the scene. This is not supported in deferred water render mode. You have to switch to WaterCameraSimple.");
				return;
			}
			Color parameterValue = GetParameterValue(ColorParameter.AbsorptionColor);
			parameterValue.a = _Water.Renderer.PropertyBlock.GetFloat(_ParameterHashes[10]);
			_GlobalWaterLookupTex.SetPixel(waterId, 0, parameterValue);
			Color parameterValue2 = GetParameterValue(ColorParameter.ReflectionColor);
			parameterValue2.a = _ForwardScatteringIntensity;
			_GlobalWaterLookupTex.SetPixel(waterId, 1, parameterValue2);
			Vector2 surfaceOffset = _Water.SurfaceOffset;
			_GlobalWaterLookupTex.SetPixel(waterId, 2, new Color(surfaceOffset.x, _Water.transform.position.y, surfaceOffset.y, _DirectionalLightsIntensity));
			Color parameterValue3 = GetParameterValue(ColorParameter.DiffuseColor);
			parameterValue3.a = _Smoothness / _AmbientSmoothness;
			_GlobalWaterLookupTex.SetPixel(waterId, 3, parameterValue3);
			_GlobalLookupDirty = true;
		}
	}

	public static void ValidateGlobalWaterDataLookupTex()
	{
		if (_GlobalWaterLookupTex == null)
		{
			CreateGlobalWaterDataLookupTex();
			_GlobalLookupDirty = false;
		}
		else if (_GlobalLookupDirty)
		{
			_GlobalWaterLookupTex.Apply(updateMipmaps: false, makeNoLongerReadable: false);
			_GlobalLookupDirty = false;
			Shader.SetGlobalTexture("_GlobalWaterLookupTex", _GlobalWaterLookupTex);
		}
	}

	public void UpdateSurfaceMaterial()
	{
		WaterQualityLevel currentQualityLevel = WaterQualitySettings.Instance.CurrentQualityLevel;
		SurfaceMaterial.SetFloat(ShaderVariables.Cull, 2f);
		float b = Mathf.Sqrt(2000000f / (float)WaterQualitySettings.Instance.MaxTesselatedVertexCount);
		_Water.Renderer.PropertyBlock.SetFloat("_TesselationFactor", Mathf.Lerp(1f, b, Mathf.Min(_TesselationFactor, currentQualityLevel.MaxTesselationFactor)));
		if (!Application.isPlaying)
		{
			bool blendMode = _Water.ShaderSet.TransparencyMode == WaterTransparencyMode.Refractive && currentQualityLevel.AllowAlphaBlending;
			if (Camera.main != null)
			{
				WaterCamera waterCamera = Camera.main.GetComponent<WaterCamera>();
				if (waterCamera == null)
				{
					waterCamera = UnityEngine.Object.FindObjectOfType<WaterCamera>();
				}
				if (waterCamera != null && waterCamera.RenderMode < WaterRenderMode.ImageEffectDeferred)
				{
					SetBlendMode(blendMode);
				}
				else
				{
					SetBlendMode(alphaBlend: false);
				}
			}
			else
			{
				SetBlendMode(alphaBlend: false);
			}
		}
		_Water.Renderer.PropertyBlock.SetFloat(_ParameterHashes[23], -1f);
		if (_AlphaBlendMode != 0)
		{
			SetBlendMode(_AlphaBlendMode == 2);
		}
		string name = SurfaceMaterial.shader.name;
		if (name.Contains("_WAVES_FFT"))
		{
			SurfaceMaterial.EnableKeyword("_WAVES_FFT");
		}
		if (name.Contains("_BOUNDED_WATER"))
		{
			SurfaceMaterial.EnableKeyword("_BOUNDED_WATER");
		}
		if (name.Contains("_WAVES_ALIGN"))
		{
			SurfaceMaterial.EnableKeyword("_WAVES_ALIGN");
		}
		if (name.Contains("_WAVES_GERSTNER"))
		{
			SurfaceMaterial.EnableKeyword("_WAVES_GERSTNER");
		}
		if (_Water.Geometry.Triangular)
		{
			SurfaceMaterial.EnableKeyword("_TRIANGLES");
		}
	}

	internal void Awake(Water water)
	{
		_Water = water;
		water.ProfilesManager.Changed.AddListener(OnProfilesChanged);
		water.WaterIdChanged += OnWaterIdChanged;
		CreateParameterHashes();
		CreateMaterials();
	}

	internal void OnWaterRender(WaterCamera waterCamera)
	{
		Vector2 surfaceOffset = _Water.SurfaceOffset;
		Vector4 vector = new Vector4(surfaceOffset.x, _Water.transform.position.y, surfaceOffset.y, _Water.UniformWaterScale);
		if (vector.x != _LastSurfaceOffset.x || vector.y != _LastSurfaceOffset.y || vector.z != _LastSurfaceOffset.z || vector.w != _LastSurfaceOffset.w)
		{
			_LastSurfaceOffset = vector;
			_Water.Renderer.PropertyBlock.SetVector(ShaderVariables.SurfaceOffset, vector);
			UpdateGlobalLookupTexOffset();
		}
		Shader.SetGlobalColor(_ParameterHashes[0], GetParameterValue(ColorParameter.AbsorptionColor));
		if (waterCamera.Type == WaterCamera.CameraType.Normal)
		{
			int num;
			if (waterCamera.RenderMode < WaterRenderMode.ImageEffectDeferred)
			{
				WaterQualityLevel currentQualityLevel = WaterQualitySettings.Instance.CurrentQualityLevel;
				num = ((_Water.ShaderSet.TransparencyMode != WaterTransparencyMode.Refractive || !currentQualityLevel.AllowAlphaBlending) ? 1 : 2);
			}
			else
			{
				num = 1;
			}
			if (_AlphaBlendMode != num)
			{
				SetBlendMode(num == 2);
			}
		}
	}

	internal void OnDestroy()
	{
		SurfaceMaterial.Destroy();
		SurfaceMaterial = null;
		SurfaceBackMaterial.Destroy();
		SurfaceBackMaterial = null;
		VolumeMaterial.Destroy();
		VolumeMaterial = null;
		VolumeBackMaterial.Destroy();
		VolumeBackMaterial = null;
		TextureUtility.Release(ref _GlobalWaterLookupTex);
		TextureUtility.Release(ref _AbsorptionGradient);
		if (Application.isPlaying && _Water != null)
		{
			_Water.ProfilesManager.Changed.RemoveListener(OnProfilesChanged);
			_Water.WaterIdChanged -= OnWaterIdChanged;
		}
	}

	internal void OnValidate()
	{
		if (_Water != null)
		{
			UpdateShaders();
			UpdateSurfaceMaterial();
		}
	}

	private void SetBlendMode(bool alphaBlend)
	{
		_AlphaBlendMode = ((!alphaBlend) ? 1 : 2);
		Material surfaceMaterial = SurfaceMaterial;
		surfaceMaterial.SetOverrideTag("RenderType", alphaBlend ? "Transparent" : "Opaque");
		surfaceMaterial.SetFloat("_Mode", alphaBlend ? 2 : 0);
		surfaceMaterial.SetInt("_SrcBlend", (!alphaBlend) ? 1 : 5);
		surfaceMaterial.SetInt("_DstBlend", alphaBlend ? 10 : 0);
		surfaceMaterial.renderQueue = (alphaBlend ? 2990 : 2000);
		UpdateSurfaceBackMaterial();
		UpdateVolumeMaterials();
	}

	private void CreateMaterials()
	{
		if (!(SurfaceMaterial != null))
		{
			int waterId = _Water.WaterId;
			_Water.ShaderSet.FindBestShaders(out var surfaceShader, out var volumeShader);
			if (surfaceShader == null || volumeShader == null)
			{
				throw new InvalidOperationException("Water in a scene '" + _Water.gameObject.scene.name + "' doesn't contain necessary shaders to function properly. Please open this scene in editor and simply select the water to update its shaders.");
			}
			SurfaceMaterial = new Material(surfaceShader)
			{
				hideFlags = HideFlags.DontSave
			};
			SurfaceMaterial.SetFloat("_WaterStencilId", waterId);
			SurfaceMaterial.SetFloat("_WaterStencilIdInv", ~waterId & 0xFF);
			_Water.Renderer.PropertyBlock.SetVector("_WaterId", new Vector4(1 << waterId, 1 << waterId + 1, ((float)waterId + 0.5f) / 256f, 0f));
			UpdateSurfaceMaterial();
			SurfaceBackMaterial = new Material(surfaceShader)
			{
				hideFlags = HideFlags.DontSave
			};
			VolumeMaterial = new Material(volumeShader)
			{
				hideFlags = HideFlags.DontSave
			};
			VolumeBackMaterial = new Material(volumeShader)
			{
				hideFlags = HideFlags.DontSave
			};
			UpdateSurfaceBackMaterial();
			UpdateVolumeMaterials();
			UsedKeywords = string.Join(" ", SurfaceMaterial.shaderKeywords);
		}
	}

	private void UpdateShaders()
	{
		_Water.ShaderSet.FindBestShaders(out var surfaceShader, out var volumeShader);
		if (SurfaceMaterial.shader != surfaceShader || VolumeMaterial.shader != volumeShader)
		{
			SurfaceMaterial.shader = surfaceShader;
			SurfaceBackMaterial.shader = surfaceShader;
			VolumeMaterial.shader = volumeShader;
			VolumeBackMaterial.shader = volumeShader;
			UpdateSurfaceMaterial();
			UpdateSurfaceBackMaterial();
			UpdateVolumeMaterials();
		}
	}

	private void OnProfilesChanged(Water water)
	{
		List<Water.WeightedProfile> profiles = water.ProfilesManager.Profiles;
		WaterProfileData profile = profiles[0].Profile;
		float num = 0f;
		for (int i = 0; i < profiles.Count; i++)
		{
			Water.WeightedProfile weightedProfile = profiles[i];
			if (profile == null || num < weightedProfile.Weight)
			{
				profile = weightedProfile.Profile;
				num = weightedProfile.Weight;
			}
		}
		HorizontalDisplacementScale = 0f;
		UnderwaterBlurSize = 0f;
		UnderwaterLightFadeScale = 0f;
		UnderwaterDistortionsIntensity = 0f;
		UnderwaterDistortionAnimationSpeed = 0f;
		Color color = new Color(0f, 0f, 0f, 0f);
		Color value = new Color(0f, 0f, 0f, 0f);
		Color value2 = new Color(0f, 0f, 0f, 0f);
		Color value3 = new Color(0f, 0f, 0f, 0f);
		Color value4 = new Color(0f, 0f, 0f, 0f);
		Color value5 = new Color(0f, 0f, 0f, 0f);
		Color value6 = new Color(0f, 0f, 0f, 0f);
		Color value7 = new Color(0f, 0f, 0f, 0f);
		Color value8 = new Color(0f, 0f, 0f, 0f);
		_Smoothness = 0f;
		_AmbientSmoothness = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		float num6 = 0f;
		float num7 = 0f;
		_ForwardScatteringIntensity = 0f;
		Vector3 vector = default;
		Vector2 vector2 = default;
		NormalMapAnimation normalMapAnimation = default;
		NormalMapAnimation normalMapAnimation2 = default;
		for (int j = 0; j < profiles.Count; j++)
		{
			WaterProfileData profile2 = profiles[j].Profile;
			float weight = profiles[j].Weight;
			HorizontalDisplacementScale += profile2.HorizontalDisplacementScale * weight;
			UnderwaterBlurSize += profile2.UnderwaterBlurSize * weight;
			UnderwaterLightFadeScale += profile2.UnderwaterLightFadeScale * weight;
			UnderwaterDistortionsIntensity += profile2.UnderwaterDistortionsIntensity * weight;
			UnderwaterDistortionAnimationSpeed += profile2.UnderwaterDistortionAnimationSpeed * weight;
			color += profile2.AbsorptionColor * weight;
			value += profile2.DiffuseColor * weight;
			value2 += profile2.SpecularColor * weight;
			value3 += profile2.DepthColor * weight;
			value4 += profile2.EmissionColor * weight;
			value5 += profile2.ReflectionColor * weight;
			value6 += profile2.FoamDiffuseColor * weight;
			value7 += profile2.FoamSpecularColor * weight;
			_Smoothness += profile2.Smoothness * weight;
			_AmbientSmoothness += profile2.AmbientSmoothness * weight;
			num2 += profile2.RefractionDistortion * weight;
			value8 += profile2.SubsurfaceScatteringShoreColor * weight;
			num3 += profile2.DetailFadeDistance * weight;
			num4 += profile2.DisplacementNormalsIntensity * weight;
			num5 += profile2.EdgeBlendFactor * weight;
			num6 += profile2.DirectionalWrapSSS * weight;
			num7 += profile2.PointWrapSSS * weight;
			_ForwardScatteringIntensity += profile2.ForwardScatteringIntensity * weight;
			vector.x += profile2.PlanarReflectionIntensity * weight;
			vector.y += profile2.PlanarReflectionFlatten * weight;
			vector.z += profile2.PlanarReflectionVerticalOffset * weight;
			vector2 += profile2.FoamTiling * weight;
			normalMapAnimation += profile2.NormalMapAnimation1 * weight;
			normalMapAnimation2 += profile2.NormalMapAnimation2 * weight;
		}
		if (water.WindWaves != null && water.WindWaves.FinalRenderMode == WaveSpectrumRenderMode.GerstnerAndFFTNormals)
		{
			num4 *= 0.5f;
		}
		MaterialPropertyBlock propertyBlock = water.Renderer.PropertyBlock;
		value8.a = _ForwardScatteringIntensity;
		propertyBlock.SetVector(_ParameterHashes[0], color);
		propertyBlock.SetColor(_ParameterHashes[1], value);
		propertyBlock.SetColor(_ParameterHashes[2], value2);
		propertyBlock.SetColor(_ParameterHashes[3], value3);
		propertyBlock.SetColor(_ParameterHashes[4], value4);
		propertyBlock.SetColor(_ParameterHashes[5], value5);
		propertyBlock.SetColor(_ParameterHashes[24], value6);
		propertyBlock.SetColor(_ParameterHashes[22], value7);
		propertyBlock.SetFloat(_ParameterHashes[6], HorizontalDisplacementScale);
		propertyBlock.SetFloat(_ParameterHashes[7], _AmbientSmoothness);
		propertyBlock.SetVector(_ParameterHashes[9], new Vector4(num6, 1f / (1f + num6), num7, 1f / (1f + num7)));
		propertyBlock.SetFloat(_ParameterHashes[10], num2);
		propertyBlock.SetColor(_ParameterHashes[11], value8);
		propertyBlock.SetFloat(_ParameterHashes[12], 1f / num3);
		propertyBlock.SetFloat(_ParameterHashes[13], num4);
		propertyBlock.SetFloat(_ParameterHashes[14], 1f / num5);
		propertyBlock.SetVector(_ParameterHashes[15], vector);
		propertyBlock.SetVector(_ParameterHashes[16], new Vector4(normalMapAnimation.Intensity, normalMapAnimation2.Intensity, (0f - (normalMapAnimation.Intensity + normalMapAnimation2.Intensity)) * 0.5f, 0f));
		propertyBlock.SetVector(_ParameterHashes[17], new Vector2(vector2.x / normalMapAnimation.Tiling.x, vector2.y / normalMapAnimation.Tiling.y));
		propertyBlock.SetFloat(_ParameterHashes[18], _Smoothness / _AmbientSmoothness);
		SurfaceMaterial.SetTexture(_ParameterHashes[19], profile.NormalMap);
		SurfaceMaterial.SetTexture(_ParameterHashes[20], profile.FoamDiffuseMap);
		SurfaceMaterial.SetTexture(_ParameterHashes[21], profile.FoamNormalMap);
		water.UVAnimator.NormalMapAnimation1 = normalMapAnimation;
		water.UVAnimator.NormalMapAnimation2 = normalMapAnimation2;
		if (_VectorOverrides != null)
		{
			ApplyOverridenParameters();
		}
		_AbsorptionGradientDirty = true;
		UpdateSurfaceBackMaterial();
		UpdateVolumeMaterials();
		UpdateGlobalLookupTex();
	}

	private void UpdateSurfaceBackMaterial()
	{
		if (!(SurfaceBackMaterial == null))
		{
			if (SurfaceBackMaterial.shader != SurfaceMaterial.shader)
			{
				SurfaceBackMaterial.shader = SurfaceMaterial.shader;
			}
			Color color = SurfaceBackMaterial.GetColor(_ParameterHashes[2]);
			SurfaceBackMaterial.CopyPropertiesFromMaterial(SurfaceMaterial);
			SurfaceBackMaterial.SetColor(_ParameterHashes[2], color);
			SurfaceBackMaterial.EnableKeyword("_WATER_BACK");
			SurfaceBackMaterial.SetFloat(ShaderVariables.Cull, 1f);
		}
	}

	private void UpdateVolumeMaterials()
	{
		if (!(VolumeMaterial == null))
		{
			VolumeMaterial.CopyPropertiesFromMaterial(SurfaceMaterial);
			VolumeBackMaterial.CopyPropertiesFromMaterial(SurfaceMaterial);
			if (SurfaceMaterial.renderQueue == 2990)
			{
				Material volumeBackMaterial = VolumeBackMaterial;
				int renderQueue = (VolumeMaterial.renderQueue = 2991);
				volumeBackMaterial.renderQueue = renderQueue;
			}
			VolumeBackMaterial.SetVector(ShaderVariables.WaterId, new Vector4(2f, 4f, 6f / 1024f, 0f));
		}
	}

	private void ApplyOverridenParameters()
	{
		MaterialPropertyBlock propertyBlock = _Water.Renderer.PropertyBlock;
		for (int i = 0; i < _VectorOverrides.Length; i++)
		{
			propertyBlock.SetVector(_VectorOverrides[i].Hash, _VectorOverrides[i].Value);
		}
		for (int j = 0; j < _FloatOverrides.Length; j++)
		{
			propertyBlock.SetFloat(_FloatOverrides[j].Hash, _FloatOverrides[j].Value);
		}
	}

	private void OnWaterIdChanged()
	{
		int waterId = _Water.WaterId;
		_Water.Renderer.PropertyBlock.SetVector(ShaderVariables.WaterId, new Vector4(1 << waterId, 1 << waterId + 1, ((float)waterId + 0.5f) / 256f, 0f));
	}

	private static void RemoveArrayElementAt<T>(ref T[] array, int index)
	{
		T[] array2 = array;
		T[] array3 = new T[array.Length - 1];
		for (int i = 0; i < index; i++)
		{
			array3[i] = array2[i];
		}
		for (int j = index; j < array3.Length; j++)
		{
			array3[j] = array2[j + 1];
		}
		array = array3;
	}

	private static void CreateParameterHashes()
	{
		if (_ParameterHashes == null || _ParameterHashes.Length != _ParameterNames.Length)
		{
			int num = _ParameterNames.Length;
			_ParameterHashes = new int[num];
			for (int i = 0; i < num; i++)
			{
				_ParameterHashes[i] = Shader.PropertyToID(_ParameterNames[i]);
			}
		}
	}

	private void UpdateGlobalLookupTexOffset()
	{
		int waterId = _Water.WaterId;
		if (!(_GlobalWaterLookupTex == null) && waterId != -1)
		{
			if (waterId >= 256)
			{
				Debug.LogError("There is more than 256 water objects in the scene. This is not supported in deferred water render mode. You have to switch to WaterCameraSimple.");
				return;
			}
			Vector2 surfaceOffset = _Water.SurfaceOffset;
			_GlobalWaterLookupTex.SetPixel(waterId, 2, new Color(surfaceOffset.x, _Water.transform.position.y, surfaceOffset.y, _DirectionalLightsIntensity));
			_GlobalLookupDirty = true;
		}
	}

	private static void CreateGlobalWaterDataLookupTex()
	{
		_GlobalWaterLookupTex = new Texture2D(256, 4, TextureFormat.RGBAHalf, mipChain: false, linear: true)
		{
			name = "[UWS] WaterMaterials - _GlobalWaterLookupTex",
			hideFlags = HideFlags.DontSave,
			filterMode = FilterMode.Point,
			wrapMode = TextureWrapMode.Clamp
		};
		Color color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
		for (int i = 0; i < 256; i++)
		{
			_GlobalWaterLookupTex.SetPixel(i, 0, color);
			_GlobalWaterLookupTex.SetPixel(i, 1, color);
			_GlobalWaterLookupTex.SetPixel(i, 2, color);
			_GlobalWaterLookupTex.SetPixel(i, 3, color);
		}
		List<Water> waters = ApplicationSingleton<WaterSystem>.Instance.Waters;
		int count = waters.Count;
		for (int j = 0; j < count; j++)
		{
			waters[j].Materials.UpdateGlobalLookupTex();
		}
		_GlobalWaterLookupTex.Apply(updateMipmaps: false, makeNoLongerReadable: false);
		Shader.SetGlobalTexture("_GlobalWaterLookupTex", _GlobalWaterLookupTex);
	}

	private void ComputeAbsorptionGradient()
	{
		_AbsorptionGradientDirty = false;
		if (_AbsorptionGradient == null)
		{
			_AbsorptionGradient = new Texture2D(512, 1, TextureFormat.RGBAHalf, mipChain: false, linear: true)
			{
				name = "[UWS] Water Materials - Absorption Gradient",
				hideFlags = HideFlags.DontSave,
				wrapMode = TextureWrapMode.Clamp,
				filterMode = FilterMode.Bilinear
			};
		}
		List<Water.WeightedProfile> profiles = _Water.ProfilesManager.Profiles;
		for (int i = 0; i < 512; i++)
		{
			_AbsorptionColorsBuffer[i] = new Color(0f, 0f, 0f, 0f);
		}
		for (int j = 0; j < profiles.Count; j++)
		{
			Gradient absorptionColorByDepth = profiles[j].Profile.AbsorptionColorByDepth;
			float weight = profiles[j].Weight;
			for (int k = 0; k < 512; k++)
			{
				_AbsorptionColorsBuffer[k] += absorptionColorByDepth.Evaluate((float)k / 31f) * weight;
			}
		}
		_AbsorptionGradient.SetPixels(_AbsorptionColorsBuffer);
		_AbsorptionGradient.Apply();
	}

	public Color GetParameterValue(ColorParameter parameter)
	{
		int num = _ParameterHashes[(int)parameter];
		for (int i = 0; i < _VectorOverrides.Length; i++)
		{
			if (_VectorOverrides[i].Hash == num)
			{
				return _VectorOverrides[i].Value;
			}
		}
		return _Water.Renderer.PropertyBlock.GetVector(num);
	}

	public WaterParameterVector4 GetParameterOverride(ColorParameter parameter)
	{
		int num = _ParameterHashes[(int)parameter];
		for (int i = 0; i < _VectorOverrides.Length; i++)
		{
			if (_VectorOverrides[i].Hash == num)
			{
				return _VectorOverrides[i];
			}
		}
		Vector4 vector = _Water.Renderer.PropertyBlock.GetVector(num);
		Array.Resize(ref _VectorOverrides, _VectorOverrides.Length + 1);
		return _VectorOverrides[_VectorOverrides.Length - 1] = new WaterParameterVector4(_Water.Renderer.PropertyBlock, num, vector);
	}

	public void ResetParameter(ColorParameter parameter)
	{
		int num = _ParameterHashes[(int)parameter];
		for (int i = 0; i < _VectorOverrides.Length; i++)
		{
			if (_VectorOverrides[i].Hash == num)
			{
				RemoveArrayElementAt(ref _VectorOverrides, i);
			}
		}
	}

	public Vector4 GetParameterValue(VectorParameter parameter)
	{
		int num = _ParameterHashes[(int)parameter];
		for (int i = 0; i < _VectorOverrides.Length; i++)
		{
			if (_VectorOverrides[i].Hash == num)
			{
				return _VectorOverrides[i].Value;
			}
		}
		return _Water.Renderer.PropertyBlock.GetVector(num);
	}

	public WaterParameterVector4 GetParameterOverride(VectorParameter parameter)
	{
		int num = _ParameterHashes[(int)parameter];
		for (int i = 0; i < _VectorOverrides.Length; i++)
		{
			if (_VectorOverrides[i].Hash == num)
			{
				return _VectorOverrides[i];
			}
		}
		Vector4 vector = _Water.Renderer.PropertyBlock.GetVector(num);
		Array.Resize(ref _VectorOverrides, _VectorOverrides.Length + 1);
		return _VectorOverrides[_VectorOverrides.Length - 1] = new WaterParameterVector4(_Water.Renderer.PropertyBlock, num, vector);
	}

	public void ResetParameter(VectorParameter parameter)
	{
		int num = _ParameterHashes[(int)parameter];
		for (int i = 0; i < _VectorOverrides.Length; i++)
		{
			if (_VectorOverrides[i].Hash == num)
			{
				RemoveArrayElementAt(ref _VectorOverrides, i);
			}
		}
	}

	public float GetParameterValue(FloatParameter parameter)
	{
		int num = _ParameterHashes[(int)parameter];
		for (int i = 0; i < _FloatOverrides.Length; i++)
		{
			if (_FloatOverrides[i].Hash == num)
			{
				return _FloatOverrides[i].Value;
			}
		}
		return _Water.Renderer.PropertyBlock.GetFloat(num);
	}

	public WaterParameterFloat GetParameterOverride(FloatParameter parameter)
	{
		int num = _ParameterHashes[(int)parameter];
		for (int i = 0; i < _FloatOverrides.Length; i++)
		{
			if (_FloatOverrides[i].Hash == num)
			{
				return _FloatOverrides[i];
			}
		}
		float value = _Water.Renderer.PropertyBlock.GetFloat(num);
		Array.Resize(ref _FloatOverrides, _FloatOverrides.Length + 1);
		return _FloatOverrides[_FloatOverrides.Length - 1] = new WaterParameterFloat(_Water.Renderer.PropertyBlock, num, value);
	}

	public void ResetParameter(FloatParameter parameter)
	{
		int num = _ParameterHashes[(int)parameter];
		for (int i = 0; i < _FloatOverrides.Length; i++)
		{
			if (_FloatOverrides[i].Hash == num)
			{
				RemoveArrayElementAt(ref _FloatOverrides, i);
			}
		}
	}
}
