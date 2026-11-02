using System.Threading;
using UnityEngine;
using UnityEngine.Serialization;

namespace UltimateWater;

public class WaterProjectSettings : ScriptableObjectSingleton
{
	public enum AbsorptionEditMode
	{
		Absorption,
		Transmission
	}

	public enum SpecularEditMode
	{
		IndexOfRefraction,
		CustomColor
	}

	public static readonly float CurrentVersion = 2.1f;

	public static readonly string CurrentVersionString = "2.1.0";

	[FormerlySerializedAs("serializedVersion")]
	[SerializeField]
	private float _SerializedVersion = 1f;

	[FormerlySerializedAs("waterLayer")]
	[SerializeField]
	private int _WaterLayer = 4;

	[FormerlySerializedAs("waterTempLayer")]
	[SerializeField]
	[Tooltip("Used for some camera effects. Has to be unused. You don't need to mask it on your cameras.")]
	private int _WaterTempLayer = 22;

	[FormerlySerializedAs("waterCollidersLayer")]
	[SerializeField]
	[Tooltip("UltimateWater internally uses colliders to detect camera entering into subtractive volumes etc. You will have to ignore this layer in your scripting raycasts.")]
	private int _WaterCollidersLayer = 1;

	[Tooltip("More threads increase physics precision under stress, but also decrease overall performance a bit.")]
	[SerializeField]
	[FormerlySerializedAs("physicsThreads")]
	private int _PhysicsThreads = 1;

	[SerializeField]
	[FormerlySerializedAs("physicsThreadsPriority")]
	private System.Threading.ThreadPriority _PhysicsThreadsPriority = System.Threading.ThreadPriority.BelowNormal;

	[FormerlySerializedAs("allowCpuFFT")]
	[SerializeField]
	private bool _AllowCpuFFT = true;

	[Tooltip("Some hardware doesn't support floating point mip maps correctly and they are forcefully disabled. You may simulate how the water would look like on such hardware by disabling this option. Most notably fp mip maps don't work correctly on most AMD graphic cards (for now).")]
	[SerializeField]
	[FormerlySerializedAs("allowFloatingPointMipMaps")]
	private bool _AllowFloatingPointMipMapsOverride = true;

	[SerializeField]
	[FormerlySerializedAs("debugPhysics")]
	private bool _DebugPhysics;

	[SerializeField]
	[FormerlySerializedAs("askForWaterCameras")]
	private bool _AskForWaterCameras = true;

	[SerializeField]
	[FormerlySerializedAs("absorptionEditMode")]
	private AbsorptionEditMode _AbsorptionEditMode = AbsorptionEditMode.Transmission;

	[SerializeField]
	[FormerlySerializedAs("specularEditMode")]
	private SpecularEditMode _SpecularEditMode;

	[SerializeField]
	private bool _SinglePassStereoRendering;

	[SerializeField]
	private bool _DisableMultisampling = true;

	[SerializeField]
	private bool _ClipWaterCameraRange;

	[SerializeField]
	private float _CameraClipRange = 1000f;

	[SerializeField]
	private bool _RenderInSceneView;

	private static WaterProjectSettings _Instance;

	private static bool _AllowFloatingPointMipMaps;

	private static bool _AllowFloatingPointMipMapsChecked;

	public static WaterProjectSettings Instance
	{
		get
		{
			if (_Instance == null)
			{
				_Instance = ScriptableObjectSingleton.LoadSingleton<WaterProjectSettings>();
			}
			return _Instance;
		}
	}

	public bool ClipWaterCameraRange => _ClipWaterCameraRange;

	public float CameraClipRange => _CameraClipRange;

	public int PhysicsThreads
	{
		get
		{
			return _PhysicsThreads;
		}
		set
		{
			_PhysicsThreads = value;
		}
	}

	public int WaterLayer => _WaterLayer;

	public int WaterTempLayer => _WaterTempLayer;

	public int WaterCollidersLayer => _WaterCollidersLayer;

	public System.Threading.ThreadPriority PhysicsThreadsPriority => _PhysicsThreadsPriority;

	public bool AllowCpuFFT => _AllowCpuFFT;

	public bool AllowFloatingPointMipMaps
	{
		get
		{
			if (_AllowFloatingPointMipMapsChecked)
			{
				if (_AllowFloatingPointMipMaps)
				{
					return _AllowFloatingPointMipMapsOverride;
				}
				return false;
			}
			_AllowFloatingPointMipMapsChecked = true;
			string text = SystemInfo.graphicsDeviceVendor.ToLowerInvariant();
			_AllowFloatingPointMipMaps = !text.Contains("amd") && !text.Contains("ati") && !SystemInfo.graphicsDeviceName.ToLowerInvariant().Contains("radeon");
			if (_AllowFloatingPointMipMaps)
			{
				return _AllowFloatingPointMipMapsOverride;
			}
			return false;
		}
	}

	public AbsorptionEditMode InspectorAbsorptionEditMode => _AbsorptionEditMode;

	public SpecularEditMode InspectorSpecularEditMode => _SpecularEditMode;

	public bool DebugPhysics => _DebugPhysics;

	public bool AskForWaterCameras
	{
		get
		{
			return _AskForWaterCameras;
		}
		set
		{
			_AskForWaterCameras = value;
		}
	}

	public bool SinglePassStereoRendering => _SinglePassStereoRendering;

	public bool RenderInSceneView
	{
		get
		{
			return _RenderInSceneView;
		}
		set
		{
			_RenderInSceneView = value;
		}
	}
}
