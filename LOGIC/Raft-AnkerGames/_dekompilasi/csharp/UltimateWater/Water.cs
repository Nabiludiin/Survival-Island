using System;
using System.Collections.Generic;
using UltimateWater.Internal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace UltimateWater;

[AddComponentMenu("Ultimate Water/Water (Base Component)", -1)]
public sealed class Water : MonoBehaviour, ISerializationCallbackReceiver
{
	[Serializable]
	public struct WeightedProfile
	{
		public WaterProfileData Profile;

		public float Weight;

		public WeightedProfile(WaterProfile profile, float weight)
		{
			if (profile.Data.Spectrum == null)
			{
				Debug.Log("spectrum not created");
			}
			Profile = new WaterProfileData();
			Profile.Copy(profile.Data);
			Profile.TemplateProfile = profile;
			Weight = weight;
		}

		public void SetNonCopy(WaterProfileData profile, float weight)
		{
			Profile = profile;
			Weight = weight;
		}
	}

	[Serializable]
	public class WaterEvent : UnityEvent<Water>
	{
	}

	[Tooltip("Synchronizes dynamic preset with default water profile")]
	public bool Synchronize = true;

	public bool AskForWaterCamera = true;

	public CommandBuffer WaterCommandBuffer;

	[SerializeField]
	[FormerlySerializedAs("shaderSet")]
	private ShaderSet _ShaderSet;

	[Tooltip("Set it to anything else than 0 if your game has multiplayer functionality or you want your water to behave the same way each time your game is played (good for intro etc.).")]
	[SerializeField]
	[FormerlySerializedAs("seed")]
	private int _Seed;

	[SerializeField]
	[FormerlySerializedAs("materials")]
	private WaterMaterials _Materials;

	[SerializeField]
	[FormerlySerializedAs("profilesManager")]
	private ProfilesManager _ProfilesManager;

	[SerializeField]
	[FormerlySerializedAs("geometry")]
	private WaterGeometry _Geometry;

	[SerializeField]
	[FormerlySerializedAs("waterRenderer")]
	private WaterRenderer _WaterRenderer;

	[SerializeField]
	[FormerlySerializedAs("uvAnimator")]
	private WaterUvAnimator _UVAnimator;

	[SerializeField]
	[FormerlySerializedAs("volume")]
	private WaterVolume _Volume;

	[SerializeField]
	[FormerlySerializedAs("subsurfaceScattering")]
	private WaterSubsurfaceScattering _SubsurfaceScattering;

	[SerializeField]
	[FormerlySerializedAs("dynamicWaterData")]
	private DynamicWater.Data _DynamicWaterData;

	[SerializeField]
	[FormerlySerializedAs("foamData")]
	private Foam.Data _FoamData;

	[SerializeField]
	[FormerlySerializedAs("planarReflectionData")]
	private PlanarReflection.Data _PlanarReflectionData;

	[SerializeField]
	[FormerlySerializedAs("windWavesData")]
	private WindWaves.Data _WindWavesData;

	[SerializeField]
	[FormerlySerializedAs("dontRotateUpwards")]
	private bool _DontRotateUpwards;

	[SerializeField]
	[FormerlySerializedAs("fastEnableDisable")]
	private bool _FastEnableDisable;

	[SerializeField]
	[FormerlySerializedAs("version")]
	private float _Version = 0.4f;

	private readonly List<WaterModule> _Modules = new List<WaterModule>();

	private bool _ComponentsCreated;

	private Vector2 _SurfaceOffset = new Vector2(float.NaN, float.NaN);

	private float _Time = -1f;

	private bool _RenderingEnabled = true;

	private int _WaterIdCache = -1;

	private static bool _IsPlaying;

	public static readonly Collider[] _CollidersBuffer = new Collider[30];

	private static readonly List<Water> _PossibleWaters = new List<Water>();

	private static readonly List<Water> _ExcludedWaters = new List<Water>();

	private static readonly float[] _CompensationStepWeights = new float[14]
	{
		0.85f, 0.75f, 0.83f, 0.77f, 0.85f, 0.75f, 0.85f, 0.75f, 0.83f, 0.77f,
		0.85f, 0.75f, 0.85f, 0.75f
	};

	public LayerMask CustomEffectsLayerMask
	{
		get
		{
			return _DynamicWaterData.CustomEffectsLayerMask;
		}
		set
		{
			_DynamicWaterData.CustomEffectsLayerMask = value;
		}
	}

	public ProfilesManager ProfilesManager => _ProfilesManager;

	public WaterMaterials Materials => _Materials;

	public WaterGeometry Geometry => _Geometry;

	public WaterRenderer Renderer => _WaterRenderer;

	public WaterVolume Volume
	{
		get
		{
			return _Volume;
		}
		set
		{
			_Volume = value;
		}
	}

	public WaterUvAnimator UVAnimator => _UVAnimator;

	public DynamicWater DynamicWater { get; private set; }

	public Foam Foam { get; private set; }

	public PlanarReflection PlanarReflection { get; private set; }

	public WindWaves WindWaves { get; private set; }

	public WaterSubsurfaceScattering SubsurfaceScattering => _SubsurfaceScattering;

	public ShaderSet ShaderSet => _ShaderSet;

	public bool RenderingEnabled
	{
		get
		{
			return _RenderingEnabled;
		}
		set
		{
			if (_RenderingEnabled == value)
			{
				return;
			}
			_RenderingEnabled = value;
			if (_RenderingEnabled)
			{
				if (enabled)
				{
					WaterSystem.Register(this);
				}
			}
			else
			{
				WaterSystem.Unregister(this);
			}
		}
	}

	public int ComputedSamplesCount { get; private set; }

	public float Density { get; private set; }

	public float Gravity { get; private set; }

	public float MaxHorizontalDisplacement { get; private set; }

	public float MaxVerticalDisplacement { get; private set; }

	public int Seed
	{
		get
		{
			return _Seed;
		}
		set
		{
			_Seed = value;
		}
	}

	public Vector2 SurfaceOffset
	{
		get
		{
			if (!float.IsNaN(_SurfaceOffset.x))
			{
				return _SurfaceOffset;
			}
			return new Vector2(0f - transform.position.x, 0f - transform.position.z);
		}
		set
		{
			_SurfaceOffset = value;
		}
	}

	public float Time
	{
		get
		{
			if (_Time != -1f)
			{
				return _Time;
			}
			return UnityEngine.Time.time;
		}
		set
		{
			_Time = value;
		}
	}

	public float UniformWaterScale => transform.localScale.y;

	public int WaterId => _WaterId;

	internal int _WaterId
	{
		get
		{
			return _WaterIdCache;
		}
		set
		{
			if (_WaterIdCache != value)
			{
				_WaterIdCache = value;
				if (WaterIdChanged != null)
				{
					WaterIdChanged();
				}
			}
		}
	}

	public event Action WaterIdChanged;

	public void ForceStartup()
	{
		CreateWaterComponents();
	}

	public void OnBeforeSerialize()
	{
	}

	public void OnAfterDeserialize()
	{
		if (!_IsPlaying)
		{
			_ComponentsCreated = false;
		}
	}

	public void ResetWater()
	{
		enabled = false;
		OnDestroy();
		_ComponentsCreated = false;
		enabled = true;
	}

	public static Water CreateWater(string name, ShaderSet shaderCollection)
	{
		Water water = new GameObject(name).AddComponent<Water>();
		water._ShaderSet = shaderCollection;
		return water;
	}

	public static Water FindWater(Vector3 position, float radius)
	{
		bool isInsideSubtractiveVolume;
		bool isInsideAdditiveVolume;
		return FindWater(position, radius, null, out isInsideSubtractiveVolume, out isInsideAdditiveVolume);
	}

	public static Water FindWater(Vector3 position, float radius, out bool isInsideSubtractiveVolume, out bool isInsideAdditiveVolume)
	{
		return FindWater(position, radius, null, out isInsideSubtractiveVolume, out isInsideAdditiveVolume);
	}

	public static Water FindWater(Vector3 position, float radius, List<Water> allowedWaters, out bool isInsideSubtractiveVolume, out bool isInsideAdditiveVolume)
	{
		isInsideSubtractiveVolume = false;
		isInsideAdditiveVolume = false;
		int num = Physics.OverlapSphereNonAlloc(position, radius, _CollidersBuffer, 1 << WaterProjectSettings.Instance.WaterCollidersLayer, QueryTriggerInteraction.Collide);
		_PossibleWaters.Clear();
		_ExcludedWaters.Clear();
		for (int i = 0; i < num; i++)
		{
			WaterVolumeBase waterVolume = WaterVolumeBase.GetWaterVolume(_CollidersBuffer[i]);
			if (!(waterVolume != null))
			{
				continue;
			}
			if (waterVolume is WaterVolumeAdd)
			{
				isInsideAdditiveVolume = true;
				if (allowedWaters == null || allowedWaters.Contains(waterVolume.Water))
				{
					_PossibleWaters.Add(waterVolume.Water);
				}
			}
			else
			{
				isInsideSubtractiveVolume = true;
				_ExcludedWaters.Add(waterVolume.Water);
			}
		}
		for (int j = 0; j < _PossibleWaters.Count; j++)
		{
			if (!_ExcludedWaters.Contains(_PossibleWaters[j]))
			{
				return _PossibleWaters[j];
			}
		}
		List<Water> boundlessWaters = ApplicationSingleton<WaterSystem>.Instance.BoundlessWaters;
		int count = boundlessWaters.Count;
		for (int k = 0; k < count; k++)
		{
			Water water = boundlessWaters[k];
			if ((allowedWaters == null || allowedWaters.Contains(water)) && water.Volume.IsPointInsideMainVolume(position, radius) && !_ExcludedWaters.Contains(water))
			{
				return water;
			}
		}
		return null;
	}

	private void Awake()
	{
		WaterQualitySettings.Instance.Changed -= OnQualitySettingsChanged;
		WaterQualitySettings.Instance.Changed += OnQualitySettingsChanged;
		_Geometry.Awake(this);
		_WaterRenderer.Awake(this);
		_Materials.Awake(this);
		_ProfilesManager.Awake(this);
	}

	private void OnEnable()
	{
		if (!Application.isPlaying || (_FastEnableDisable && _ComponentsCreated))
		{
			return;
		}
		_IsPlaying = Application.isPlaying;
		CreateWaterComponents();
		if (_ComponentsCreated)
		{
			_ProfilesManager.OnEnable();
			_Geometry.OnEnable();
			_Modules.ForEach((WaterModule x) =>
			{
				x.Enable();
			});
			if (_RenderingEnabled)
			{
				WaterSystem.Register(this);
			}
		}
	}

	private void OnDisable()
	{
		if (Application.isPlaying && !_FastEnableDisable)
		{
			_Modules.ForEach((WaterModule x) =>
			{
				x.Disable();
			});
			_Geometry.OnDisable();
			_ProfilesManager.OnDisable();
			WaterSystem.Unregister(this);
		}
	}

	private void OnDestroy()
	{
		if (_FastEnableDisable)
		{
			_FastEnableDisable = false;
			OnDisable();
		}
		WaterQualitySettings.Instance.Changed -= OnQualitySettingsChanged;
		_Modules.ForEach((WaterModule x) =>
		{
			x.Destroy();
		});
		_Modules.Clear();
		_Materials.OnDestroy();
		_ProfilesManager.OnDestroy();
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			if (!_DontRotateUpwards)
			{
				transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
			}
			UpdateStatisticalData();
			_ProfilesManager.Update();
			_Geometry.Update();
			for (int i = 0; i < _Modules.Count; i++)
			{
				_Modules[i].Update();
			}
		}
	}

	public void OnValidate()
	{
		if (_ComponentsCreated)
		{
			_Modules.ForEach((WaterModule x) =>
			{
				x.Validate();
			});
			_ProfilesManager.OnValidate();
			_Materials.OnValidate();
			_Geometry.OnValidate();
			_WaterRenderer.OnValidate();
		}
	}

	public void OnDrawGizmos()
	{
		if (_Geometry != null && _Geometry.GeometryType == WaterGeometry.Type.CustomMeshes)
		{
			WaterCustomSurfaceMeshes customSurfaceMeshes = _Geometry.CustomSurfaceMeshes;
			for (int i = 0; i < customSurfaceMeshes.Meshes.Length; i++)
			{
				Gizmos.matrix = transform.localToWorldMatrix;
				Gizmos.color = Color.cyan * 0.2f;
				Gizmos.DrawMesh(customSurfaceMeshes.Meshes[i]);
				Gizmos.color = Color.cyan * 0.4f;
				Gizmos.DrawWireMesh(customSurfaceMeshes.Meshes[i]);
			}
		}
	}

	internal void OnWaterRender(WaterCamera waterCamera)
	{
		if (isActiveAndEnabled)
		{
			_Materials.OnWaterRender(waterCamera);
			for (int i = 0; i < _Modules.Count; i++)
			{
				_Modules[i].OnWaterRender(waterCamera);
			}
		}
	}

	internal void OnWaterPostRender(WaterCamera waterCamera)
	{
		for (int i = 0; i < _Modules.Count; i++)
		{
			_Modules[i].OnWaterPostRender(waterCamera);
		}
	}

	internal void OnSamplingStarted()
	{
		int computedSamplesCount = ComputedSamplesCount + 1;
		ComputedSamplesCount = computedSamplesCount;
	}

	internal void OnSamplingStopped()
	{
		int computedSamplesCount = ComputedSamplesCount - 1;
		ComputedSamplesCount = computedSamplesCount;
	}

	private void CreateWaterComponents()
	{
		if (!_ComponentsCreated)
		{
			_ComponentsCreated = true;
			_Modules.Clear();
			_Modules.AddRange(new List<WaterModule> { _Volume, _SubsurfaceScattering });
			for (int i = 0; i < _Modules.Count; i++)
			{
				_Modules[i].Start(this);
			}
			_ProfilesManager.Changed.AddListener(OnProfilesChanged);
			if (_ShaderSet.LocalEffectsSupported)
			{
				DynamicWater = new DynamicWater(this, _DynamicWaterData);
				_Modules.Add(DynamicWater);
			}
			if (_ShaderSet.PlanarReflections != PlanarReflectionsMode.Disabled)
			{
				PlanarReflection = new PlanarReflection(this, _PlanarReflectionData);
				_Modules.Add(PlanarReflection);
			}
			if (_ShaderSet.WindWavesMode != WindWavesRenderMode.Disabled)
			{
				WindWaves = new WindWaves(this, _WindWavesData);
				_Modules.Add(WindWaves);
			}
			if (_ShaderSet.Foam)
			{
				Foam = new Foam(this, _FoamData);
				_Modules.Add(Foam);
			}
		}
	}

	internal void OnProfilesChanged(Water water)
	{
		List<WeightedProfile> profiles = water.ProfilesManager.Profiles;
		Density = 0f;
		Gravity = 0f;
		for (int i = 0; i < profiles.Count; i++)
		{
			WaterProfileData profile = profiles[i].Profile;
			float weight = profiles[i].Weight;
			Density += profile.Density * weight;
			Gravity -= profile.Gravity * weight;
		}
	}

	private void OnQualitySettingsChanged()
	{
		OnValidate();
	}

	private void UpdateStatisticalData()
	{
		MaxHorizontalDisplacement = 0f;
		MaxVerticalDisplacement = 0f;
		if (WindWaves != null)
		{
			MaxHorizontalDisplacement = WindWaves.MaxHorizontalDisplacement;
			MaxVerticalDisplacement = WindWaves.MaxVerticalDisplacement;
		}
	}

	public Vector3 GetDisplacementAt(float x, float z)
	{
		Vector3 result = default;
		if (WindWaves != null)
		{
			CompensateHorizontalDisplacement(ref x, ref z);
			return WindWaves.SpectrumResolver.GetDisplacementAt(x, z, _Time);
		}
		return result;
	}

	public Vector3 GetDisplacementAt(float x, float z, float time)
	{
		Vector3 result = default;
		if (WindWaves != null)
		{
			CompensateHorizontalDisplacement(ref x, ref z);
			return WindWaves.SpectrumResolver.GetDisplacementAt(x, z, time);
		}
		return result;
	}

	public Vector3 GetUncompensatedDisplacementAt(float x, float z, float time)
	{
		if (WindWaves == null)
		{
			return default;
		}
		return WindWaves.SpectrumResolver.GetDisplacementAt(x, z, time);
	}

	public Vector2 GetHorizontalDisplacementAt(float x, float z)
	{
		Vector2 result = default;
		if (WindWaves != null)
		{
			CompensateHorizontalDisplacement(ref x, ref z);
			return WindWaves.SpectrumResolver.GetHorizontalDisplacementAt(x, z, _Time);
		}
		return result;
	}

	public Vector2 GetHorizontalDisplacementAt(float x, float z, float time)
	{
		Vector2 result = default;
		if (WindWaves != null)
		{
			CompensateHorizontalDisplacement(ref x, ref z);
			return WindWaves.SpectrumResolver.GetHorizontalDisplacementAt(x, z, time);
		}
		return result;
	}

	public Vector2 GetUncompensatedHorizontalDisplacementAt(float x, float z, float time)
	{
		if (WindWaves == null)
		{
			return default;
		}
		return WindWaves.SpectrumResolver.GetHorizontalDisplacementAt(x, z, time);
	}

	public float GetHeightAt(float x, float z)
	{
		float result = 0f;
		if (WindWaves != null)
		{
			CompensateHorizontalDisplacement(ref x, ref z);
			result = WindWaves.SpectrumResolver.GetHeightAt(x, z, _Time);
		}
		return result;
	}

	public float GetHeightAt(float x, float z, float time)
	{
		float result = 0f;
		if (WindWaves != null)
		{
			CompensateHorizontalDisplacement(ref x, ref z);
			result = WindWaves.SpectrumResolver.GetHeightAt(x, z, time);
		}
		return result;
	}

	public float GetUncompensatedHeightAt(float x, float z, float time)
	{
		if (WindWaves == null)
		{
			return 0f;
		}
		return WindWaves.SpectrumResolver.GetHeightAt(x, z, time);
	}

	public Vector4 GetHeightAndForcesAt(float x, float z)
	{
		Vector4 result = Vector4.zero;
		if (WindWaves != null)
		{
			CompensateHorizontalDisplacement(ref x, ref z);
			result = WindWaves.SpectrumResolver.GetForceAndHeightAt(x, z, _Time);
		}
		return result;
	}

	public Vector4 GetHeightAndForcesAt(float x, float z, float time)
	{
		Vector4 result = Vector4.zero;
		if (WindWaves != null)
		{
			CompensateHorizontalDisplacement(ref x, ref z);
			result = WindWaves.SpectrumResolver.GetForceAndHeightAt(x, z, time);
		}
		return result;
	}

	public Vector4 GetUncompensatedHeightAndForcesAt(float x, float z, float time)
	{
		if (WindWaves == null)
		{
			return default;
		}
		return WindWaves.SpectrumResolver.GetForceAndHeightAt(x, z, time);
	}

	[Obsolete("Use this overload instead: GetDisplacementAt(float x, float z, float time).")]
	public Vector3 GetDisplacementAt(float x, float z, float spectrumStart, float spectrumEnd, float time)
	{
		Vector3 result = default;
		if (WindWaves != null)
		{
			return WindWaves.SpectrumResolver.GetDisplacementAt(x, z, time);
		}
		return result;
	}

	[Obsolete("Use this overload instead: GetHorizontalDisplacementAt(float x, float z, float time).")]
	public Vector2 GetHorizontalDisplacementAt(float x, float z, float spectrumStart, float spectrumEnd, float time)
	{
		Vector2 result = default;
		if (WindWaves != null)
		{
			return WindWaves.SpectrumResolver.GetHorizontalDisplacementAt(x, z, time);
		}
		return result;
	}

	[Obsolete("Use this overload instead: GetHeightAt(float x, float z, float time).")]
	public float GetHeightAt(float x, float z, float spectrumStart, float spectrumEnd, float time)
	{
		float result = 0f;
		if (WindWaves != null)
		{
			result = WindWaves.SpectrumResolver.GetHeightAt(x, z, time);
		}
		return result;
	}

	[Obsolete("Use this overload instead: GetHeightAndForcesAt(float x, float z, float time).")]
	public Vector4 GetHeightAndForcesAt(float x, float z, float spectrumStart, float spectrumEnd, float time)
	{
		Vector4 result = Vector4.zero;
		if (WindWaves != null)
		{
			result = WindWaves.SpectrumResolver.GetForceAndHeightAt(x, z, time);
		}
		return result;
	}

	public void CompensateHorizontalDisplacement(ref float x, ref float z, float errorTolerance = 0.045f)
	{
		float num = x;
		float num2 = z;
		if (WindWaves == null)
		{
			return;
		}
		SpectrumResolver spectrumResolver = WindWaves.SpectrumResolver;
		Vector2 horizontalDisplacementAt = spectrumResolver.GetHorizontalDisplacementAt(x, z, _Time);
		x -= horizontalDisplacementAt.x;
		z -= horizontalDisplacementAt.y;
		if (!(horizontalDisplacementAt.x > errorTolerance) && !(horizontalDisplacementAt.y > errorTolerance) && !(horizontalDisplacementAt.x < 0f - errorTolerance) && !(horizontalDisplacementAt.y < 0f - errorTolerance))
		{
			return;
		}
		for (int i = 0; i < 14; i++)
		{
			horizontalDisplacementAt = spectrumResolver.GetHorizontalDisplacementAt(x, z, _Time);
			float num3 = num - (x + horizontalDisplacementAt.x);
			float num4 = num2 - (z + horizontalDisplacementAt.y);
			x += num3 * _CompensationStepWeights[i];
			z += num4 * _CompensationStepWeights[i];
			if (num3 < errorTolerance && num4 < errorTolerance && num3 > 0f - errorTolerance && num4 > 0f - errorTolerance)
			{
				break;
			}
		}
	}
}
