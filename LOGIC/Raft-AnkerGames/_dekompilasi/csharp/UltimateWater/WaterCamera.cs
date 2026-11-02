using System;
using System.Collections.Generic;
using UltimateWater.Internal;
using UnityEngine;
using UnityEngine.AzureSky;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace UltimateWater;

[AddComponentMenu("Ultimate Water/Water Camera", -1)]
[ExecuteInEditMode]
[ImageEffectAllowedInSceneView]
public class WaterCamera : MonoBehaviour
{
	public enum CameraType
	{
		Normal,
		Effect,
		RenderHelper
	}

	[Serializable]
	public class WaterCameraEvent : UnityEvent<WaterCamera>
	{
	}

	[HideInInspector]
	public Camera ReflectionCamera;

	[HideInInspector]
	[SerializeField]
	[FormerlySerializedAs("depthBlitCopyShader")]
	private Shader _DepthBlitCopyShader;

	[HideInInspector]
	[SerializeField]
	[FormerlySerializedAs("shadowEnforcerShader")]
	private Shader _ShadowEnforcerShader;

	[HideInInspector]
	[SerializeField]
	[FormerlySerializedAs("gbuffer0MixShader")]
	private Shader _Gbuffer0MixShader;

	[HideInInspector]
	[SerializeField]
	[FormerlySerializedAs("gbuffer123MixShader")]
	private Shader _Gbuffer123MixShader;

	[HideInInspector]
	[SerializeField]
	[FormerlySerializedAs("finalColorMixShader")]
	private Shader _FinalColorMixShader;

	[HideInInspector]
	[SerializeField]
	[FormerlySerializedAs("deferredReflections")]
	private Shader _DeferredReflections;

	[SerializeField]
	[FormerlySerializedAs("deferredShading")]
	[HideInInspector]
	private Shader _DeferredShading;

	[HideInInspector]
	[SerializeField]
	[FormerlySerializedAs("mergeDisplacementsShader")]
	private Shader _MergeDisplacementsShader;

	[SerializeField]
	[FormerlySerializedAs("renderMode")]
	private WaterRenderMode _RenderMode;

	[SerializeField]
	[FormerlySerializedAs("geometryType")]
	private WaterGeometryType _GeometryType;

	[SerializeField]
	[FormerlySerializedAs("renderWaterDepth")]
	private bool _RenderWaterDepth = true;

	[Tooltip("Water has a pretty smooth shape so it's often safe to render it's depth in a lower resolution than the rest of the scene. Although the default value is 1.0, you may probably safely use 0.5 and gain some minor performance boost. If you will encounter any artifacts in masking or image effects, set it back to 1.0.")]
	[FormerlySerializedAs("baseEffectsQuality")]
	[Range(0.2f, 1f)]
	[SerializeField]
	private float _BaseEffectsQuality = 1f;

	[SerializeField]
	[FormerlySerializedAs("superSampling")]
	private float _SuperSampling = 1f;

	[SerializeField]
	[FormerlySerializedAs("renderVolumes")]
	private bool _RenderVolumes = true;

	[SerializeField]
	[FormerlySerializedAs("renderFlatMasks")]
	private bool _RenderFlatMasks = true;

	[SerializeField]
	[FormerlySerializedAs("forcedVertexCount")]
	private int _ForcedVertexCount;

	[SerializeField]
	[FormerlySerializedAs("submersionStateChanged")]
	private WaterCameraEvent _SubmersionStateChanged;

	[Tooltip("Optional. Deferred rendering mode will try to match profile parameters of this water object as well as possible. It affects only some minor parameters and you may generally ignore this setting. May be removed in the future.")]
	[SerializeField]
	[FormerlySerializedAs("mainWater")]
	private Water _MainWater;

	[SerializeField]
	[FormerlySerializedAs("effectsLight")]
	private LightWaterEffects _EffectsLight;

	private RenderTexture _Gbuffer0Tex;

	private RenderTexture _DepthTex2;

	private WaterCamera _BaseCamera;

	private Camera _EffectCamera;

	private Camera _MainCamera;

	private Camera _PlaneProjectorCamera;

	protected Camera _CameraComponent;

	private Material _DepthBlitCopyMaterialCache;

	private RenderTextureFormat _BlendedDepthTexturesFormat;

	private CameraType _WaterCameraType;

	private bool _EffectsEnabled;

	private IWaterImageEffect[] _ImageEffects;

	private Rect _LocalMapsRect;

	private Rect _LocalMapsRectPrevious;

	private Rect _ShadowedWaterRect;

	private int _PixelWidth;

	private int _PixelHeight;

	private Mesh _ShadowsEnforcerMesh;

	private Material _ShadowsEnforcerMaterial;

	internal Water _ContainingWater;

	private float _WaterLevel;

	private SubmersionState _SubmersionState;

	private bool _IsInsideSubtractiveVolume;

	private bool _IsInsideAdditiveVolume;

	private Matrix4x4 _LastPlaneProjectorMatrix;

	private WaterCameraIME _WaterCameraIME;

	private List<Water> _CustomWaterRenderList;

	private static CommandBuffer _UtilityCommandBufferCache;

	private static readonly Dictionary<Camera, WaterCamera> _WaterCamerasCache = new Dictionary<Camera, WaterCamera>();

	private static readonly List<WaterCamera> _EnabledWaterCameras = new List<WaterCamera>();

	private static readonly RenderTargetIdentifier[] _DeferredTargets = new RenderTargetIdentifier[3]
	{
		BuiltinRenderTextureType.GBuffer1,
		BuiltinRenderTextureType.GBuffer2,
		BuiltinRenderTextureType.Reflections
	};

	private Camera _WaterRenderCameraCache;

	private CommandBuffer _ImageEffectCommands;

	private RenderTexture _DeferredRenderTarget;

	private Material _Gbuffer0MixMaterial;

	private Material _Gbuffer123MixMaterial;

	private Material _FinalColorMixMaterial;

	private readonly MaskModule _MaskModule = new MaskModule();

	private readonly DepthModule _DepthModule = new DepthModule();

	private readonly ForwardModule _ForwardModule = new ForwardModule();

	[SerializeField]
	private WaterCameraSubmersion _CameraSubmersion = new WaterCameraSubmersion();

	public bool _EditSubmersion;

	public bool RenderWaterDepth
	{
		get
		{
			return _RenderWaterDepth;
		}
		set
		{
			_RenderWaterDepth = value;
		}
	}

	public bool RenderVolumes
	{
		get
		{
			return _RenderVolumes;
		}
		set
		{
			_RenderVolumes = value;
		}
	}

	public float BaseEffectsQuality
	{
		get
		{
			return _BaseEffectsQuality;
		}
		set
		{
			_BaseEffectsQuality = value;
		}
	}

	public CameraType Type
	{
		get
		{
			return _WaterCameraType;
		}
		set
		{
			_WaterCameraType = value;
		}
	}

	public WaterGeometryType GeometryType
	{
		get
		{
			return _GeometryType;
		}
		set
		{
			_GeometryType = value;
		}
	}

	public Rect LocalMapsRect => _LocalMapsRect;

	public WaterRenderMode RenderMode
	{
		get
		{
			return _RenderMode;
		}
		set
		{
			_RenderMode = value;
			OnDisable();
			OnEnable();
		}
	}

	public Rect LocalMapsRectPrevious => _LocalMapsRectPrevious;

	public Vector4 LocalMapsShaderCoords
	{
		get
		{
			float num = 1f / _LocalMapsRect.width;
			return new Vector4((0f - _LocalMapsRect.xMin) * num, (0f - _LocalMapsRect.yMin) * num, num, _LocalMapsRect.width);
		}
	}

	public int ForcedVertexCount
	{
		get
		{
			return _ForcedVertexCount;
		}
		set
		{
			_ForcedVertexCount = value;
		}
	}

	public Water ContainingWater
	{
		get
		{
			if (!(_BaseCamera == null))
			{
				return _BaseCamera.ContainingWater;
			}
			if (_SubmersionState == SubmersionState.None)
			{
				return null;
			}
			return _ContainingWater;
		}
	}

	public float WaterLevel => _WaterLevel;

	public SubmersionState SubmersionState => _SubmersionState;

	public Camera MainCamera => _MainCamera;

	public Camera CameraComponent => _CameraComponent;

	public Water MainWater
	{
		get
		{
			if (_MainWater != null)
			{
				return _MainWater;
			}
			List<Water> boundlessWaters = ApplicationSingleton<WaterSystem>.Instance.BoundlessWaters;
			if (boundlessWaters.Count != 0)
			{
				return boundlessWaters[0];
			}
			List<Water> list = ((_CustomWaterRenderList != null) ? _CustomWaterRenderList : ApplicationSingleton<WaterSystem>.Instance.Waters);
			if (list.Count != 0)
			{
				return list[0];
			}
			return null;
		}
	}

	public static List<WaterCamera> EnabledWaterCameras => _EnabledWaterCameras;

	public int BaseEffectWidth => _PixelWidth;

	public int BaseEffectHeight => _PixelHeight;

	public bool RenderFlatMasks => _RenderFlatMasks;

	public bool IsInsideSubtractiveVolume => _IsInsideSubtractiveVolume;

	public Camera EffectsCamera
	{
		get
		{
			if (_WaterCameraType == CameraType.Normal && _EffectCamera == null)
			{
				_EffectCamera = CreateEffectsCamera(CameraType.Effect);
			}
			return _EffectCamera;
		}
	}

	public Camera PlaneProjectorCamera
	{
		get
		{
			if (_WaterCameraType == CameraType.Normal && _PlaneProjectorCamera == null)
			{
				_PlaneProjectorCamera = CreateEffectsCamera(CameraType.Effect);
			}
			return _PlaneProjectorCamera;
		}
	}

	public WaterCameraEvent SubmersionStateChanged
	{
		get
		{
			if (_SubmersionStateChanged == null)
			{
				return _SubmersionStateChanged = new WaterCameraEvent();
			}
			return _SubmersionStateChanged;
		}
	}

	public bool IsInsideAdditiveVolume => _IsInsideAdditiveVolume;

	public LightWaterEffects EffectsLight
	{
		get
		{
			return _EffectsLight;
		}
		set
		{
			_EffectsLight = value;
		}
	}

	public WaterCameraSubmersion CameraSubmersion => _CameraSubmersion;

	protected Material _DepthBlitCopyMaterial
	{
		get
		{
			if (!(_DepthBlitCopyMaterialCache != null))
			{
				Material material = new Material(_DepthBlitCopyShader)
				{
					hideFlags = HideFlags.DontSave
				};
				Material result = material;
				_DepthBlitCopyMaterialCache = material;
				return result;
			}
			return _DepthBlitCopyMaterialCache;
		}
	}

	private static CommandBuffer _UtilityCommandBuffer
	{
		get
		{
			if (_UtilityCommandBufferCache == null)
			{
				CommandBuffer commandBuffer = new CommandBuffer
				{
					name = "[PW Water] WaterCamera Utility"
				};
				_UtilityCommandBufferCache = commandBuffer;
				return commandBuffer;
			}
			return _UtilityCommandBufferCache;
		}
	}

	internal Camera _WaterRenderCamera
	{
		get
		{
			if (!(_WaterRenderCameraCache != null))
			{
				return _WaterRenderCameraCache = CreateEffectsCamera(CameraType.RenderHelper);
			}
			return _WaterRenderCameraCache;
		}
	}

	public static event Action<WaterCamera> OnGlobalPreCull;

	public static event Action<WaterCamera> OnGlobalPostRender;

	public event Action<WaterCamera> RenderTargetResized;

	public event Action<WaterCamera> Destroyed;

	public event Action<WaterCamera> Disabled;

	public void SetCustomWaterRenderList(List<Water> waters)
	{
		_CustomWaterRenderList = waters;
	}

	public static WaterCamera GetWaterCamera(Camera camera, bool forceAdd = false)
	{
		if (!_WaterCamerasCache.TryGetValue(camera, out var value))
		{
			value = camera.GetComponent<WaterCamera>();
			if (value != null)
			{
				_WaterCamerasCache[camera] = value;
			}
			else if (!forceAdd)
			{
				value = (_WaterCamerasCache[camera] = null);
			}
			else
			{
				_WaterCamerasCache[camera] = camera.gameObject.AddComponent<WaterCamera>();
			}
		}
		return value;
	}

	protected void Awake()
	{
		OnValidate();
		if (SystemInfo.graphicsShaderLevel >= 50 && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth) && _DepthBlitCopyMaterial.passCount > 3)
		{
			_BlendedDepthTexturesFormat = RenderTextureFormat.Depth;
		}
		else if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RFloat) && _BaseEffectsQuality > 0.2f)
		{
			_BlendedDepthTexturesFormat = RenderTextureFormat.RFloat;
		}
		else if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RHalf))
		{
			_BlendedDepthTexturesFormat = RenderTextureFormat.RHalf;
		}
		else
		{
			_BlendedDepthTexturesFormat = RenderTextureFormat.R8;
		}
		_Gbuffer0MixMaterial = new Material(_Gbuffer0MixShader)
		{
			hideFlags = HideFlags.DontSave
		};
		_Gbuffer123MixMaterial = new Material(_Gbuffer123MixShader)
		{
			hideFlags = HideFlags.DontSave
		};
		_FinalColorMixMaterial = new Material(_FinalColorMixShader)
		{
			hideFlags = HideFlags.DontSave
		};
	}

	protected void OnEnable()
	{
		_CameraComponent = GetComponent<Camera>();
		_WaterCamerasCache[_CameraComponent] = this;
		if (_WaterCameraType == CameraType.Normal)
		{
			_EnabledWaterCameras.Add(this);
			_ImageEffects = GetComponents<IWaterImageEffect>();
			IWaterImageEffect[] imageEffects = _ImageEffects;
			for (int i = 0; i < imageEffects.Length; i++)
			{
				imageEffects[i].OnWaterCameraEnabled();
			}
		}
		RemoveUtilityCommands();
		AddUtilityCommands();
		_MaskModule.OnEnable(this);
		_DepthModule.OnEnable(this);
		_ForwardModule.OnEnable(this);
		_CameraSubmersion.OnEnable(this);
	}

	protected void OnDisable()
	{
		if (_WaterCameraType == CameraType.Normal)
		{
			_EnabledWaterCameras.Remove(this);
		}
		ReleaseImageEffectTemporaryTextures();
		ReleaseTemporaryTextures();
		RemoveUtilityCommands();
		DisableEffects();
		if (_EffectCamera != null)
		{
			_EffectCamera.gameObject.Destroy();
			_EffectCamera = null;
		}
		if (_PlaneProjectorCamera != null)
		{
			_PlaneProjectorCamera.gameObject.Destroy();
			_PlaneProjectorCamera = null;
		}
		if (_DepthBlitCopyMaterialCache != null)
		{
			_DepthBlitCopyMaterialCache.Destroy();
			_DepthBlitCopyMaterialCache = null;
		}
		_ContainingWater = null;
		_MaskModule.OnDisable(this);
		_DepthModule.OnDisable(this);
		_ForwardModule.OnDisable(this);
		_CameraSubmersion.OnDisable();
		if (Disabled != null)
		{
			Disabled(this);
		}
	}

	protected void OnPreCull()
	{
		if (!ShouldRender())
		{
			return;
		}
		if (OnGlobalPreCull != null)
		{
			OnGlobalPreCull(this);
		}
		if (_WaterCameraType == CameraType.RenderHelper)
		{
			WaterCamera component = _MainCamera.GetComponent<WaterCamera>();
			if (component != null)
			{
				component.RenderWaterDirect();
			}
			return;
		}
		if (_WaterCameraType == CameraType.Normal)
		{
			bool flipY = WaterUtilities.IsSceneViewCamera(_CameraComponent) || (VersionCompatibility.Version >= 560 && _RenderMode == WaterRenderMode.DefaultQueue && _CameraComponent.allowHDR);
			SetPlaneProjectorMatrix(_RenderMode != WaterRenderMode.DefaultQueue, flipY);
			ToggleEffects();
			PrepareToRender();
			SetFallbackTextures();
		}
		if (_EffectsEnabled)
		{
			SetLocalMapCoordinates();
		}
		RenderWaterEffects();
		if (_EffectsLight != null)
		{
			_EffectsLight.PrepareRenderingOnCamera(this);
		}
		if (_RenderMode == WaterRenderMode.DefaultQueue)
		{
			RenderWaterDirect();
		}
		if (!_EffectsEnabled)
		{
			Shader.DisableKeyword("WATER_BUFFERS_ENABLED");
			return;
		}
		Shader.EnableKeyword("WATER_BUFFERS_ENABLED");
		if (_RenderVolumes)
		{
			_MaskModule.Process(this);
		}
		_DepthModule.Process(this);
		if (_ImageEffects != null && Application.isPlaying)
		{
			for (int i = 0; i < _ImageEffects.Length; i++)
			{
				_ImageEffects[i].OnWaterCameraPreCull();
			}
			_CameraComponent.GetComponent<AzureSkyFogScattering>().enabled = _SubmersionState != SubmersionState.Full;
		}
		if (_ShadowedWaterRect.xMin < _ShadowedWaterRect.xMax)
		{
			RenderShadowEnforcers();
		}
		if (_RenderMode != WaterRenderMode.DefaultQueue)
		{
			List<Water> list = ((_CustomWaterRenderList != null) ? _CustomWaterRenderList : ApplicationSingleton<WaterSystem>.Instance.Waters);
			for (int num = list.Count - 1; num >= 0; num--)
			{
				list[num].Volume.DisableRenderers();
			}
			WaterMaterials.ValidateGlobalWaterDataLookupTex();
		}
	}

	protected void OnPostRender()
	{
		if (ShouldRender())
		{
			ReleaseTemporaryTextures();
			List<Water> list = ((_CustomWaterRenderList != null) ? _CustomWaterRenderList : ApplicationSingleton<WaterSystem>.Instance.Waters);
			for (int num = list.Count - 1; num >= 0; num--)
			{
				list[num].Renderer.PostRender(this);
			}
			if ((object)_EffectsLight != null)
			{
				_EffectsLight.CleanRenderingOnCamera();
			}
			if (OnGlobalPostRender != null)
			{
				OnGlobalPostRender(this);
			}
		}
	}

	protected void Update()
	{
		if (ShouldRender() && _RenderMode == WaterRenderMode.ImageEffectDeferred)
		{
			ReleaseImageEffectTemporaryTextures();
		}
	}

	private void OnDestroy()
	{
		_WaterCamerasCache.Remove(GetComponent<Camera>());
		if (Destroyed != null)
		{
			Destroyed(this);
			Destroyed = null;
		}
	}

	private void OnValidate()
	{
		_CameraSubmersion.OnValidate();
		_MaskModule.OnValidate(this);
		_DepthModule.OnValidate(this);
		_ForwardModule.OnValidate(this);
		if (_DepthBlitCopyShader == null)
		{
			_DepthBlitCopyShader = Shader.Find("UltimateWater/Depth/Depth Copy");
		}
		if (_ShadowEnforcerShader == null)
		{
			_ShadowEnforcerShader = Shader.Find("UltimateWater/Utility/ShadowEnforcer");
		}
		if (_Gbuffer0MixShader == null)
		{
			_Gbuffer0MixShader = Shader.Find("UltimateWater/Deferred/GBuffer0Mix");
		}
		if (_Gbuffer123MixShader == null)
		{
			_Gbuffer123MixShader = Shader.Find("UltimateWater/Deferred/GBuffer123Mix");
		}
		if (_FinalColorMixShader == null)
		{
			_FinalColorMixShader = Shader.Find("UltimateWater/Deferred/FinalColorMix");
		}
		if (_DeferredReflections == null)
		{
			_DeferredReflections = Shader.Find("Hidden/UltimateWater-Internal-DeferredReflections");
		}
		if (_DeferredShading == null)
		{
			_DeferredShading = Shader.Find("Hidden/UltimateWater-Internal-DeferredShading");
		}
		if (_MergeDisplacementsShader == null)
		{
			_MergeDisplacementsShader = Shader.Find("UltimateWater/Utility/MergeDisplacements");
		}
		if (_WaterCameraIME == null)
		{
			_WaterCameraIME = GetComponent<WaterCameraIME>();
			if (_WaterCameraIME == null)
			{
				_WaterCameraIME = gameObject.AddComponent<WaterCameraIME>();
			}
		}
		if (_RenderMode == WaterRenderMode.ImageEffectDeferred)
		{
			_RenderWaterDepth = true;
		}
		_CameraComponent = GetComponent<Camera>();
		RemoveUtilityCommands();
		if (enabled)
		{
			AddUtilityCommands();
		}
		_WaterCameraIME.enabled = enabled && (_RenderMode == WaterRenderMode.ImageEffectDeferred || _RenderMode == WaterRenderMode.ImageEffectForward);
	}

	private void OnDrawGizmosSelected()
	{
		if (_EditSubmersion)
		{
			_CameraSubmersion.OnDrawGizmos();
		}
	}

	internal void ReportShadowedWaterMinMaxRect(Vector2 min, Vector2 max)
	{
		if (_ShadowedWaterRect.xMin > min.x)
		{
			_ShadowedWaterRect.xMin = min.x;
		}
		if (_ShadowedWaterRect.yMin > min.y)
		{
			_ShadowedWaterRect.yMin = min.y;
		}
		if (_ShadowedWaterRect.xMax < max.x)
		{
			_ShadowedWaterRect.xMax = max.x;
		}
		if (_ShadowedWaterRect.yMax < max.y)
		{
			_ShadowedWaterRect.yMax = max.y;
		}
	}

	internal void RenderWaterWithShader(string commandName, RenderTexture target, Shader shader, Water water)
	{
		CommandBuffer utilityCommandBuffer = _UtilityCommandBuffer;
		utilityCommandBuffer.Clear();
		utilityCommandBuffer.SetRenderTarget(target);
		water.Renderer.Render(_CameraComponent, _GeometryType, utilityCommandBuffer, shader);
		GL.PushMatrix();
		GL.modelview = _CameraComponent.worldToCameraMatrix;
		GL.LoadProjectionMatrix(_CameraComponent.projectionMatrix);
		Graphics.ExecuteCommandBuffer(utilityCommandBuffer);
		GL.PopMatrix();
	}

	internal void RenderWaterWithShader(string commandName, RenderTexture target, Shader shader, bool surfaces, bool volumes, bool volumesTwoPass)
	{
		CommandBuffer utilityCommandBuffer = _UtilityCommandBuffer;
		utilityCommandBuffer.Clear();
		utilityCommandBuffer.SetRenderTarget(target);
		AddWaterRenderCommands(utilityCommandBuffer, shader, surfaces, volumes, volumesTwoPass);
		GL.PushMatrix();
		GL.modelview = _CameraComponent.worldToCameraMatrix;
		GL.LoadProjectionMatrix(_CameraComponent.projectionMatrix);
		Graphics.ExecuteCommandBuffer(utilityCommandBuffer);
		GL.PopMatrix();
	}

	internal void AddWaterRenderCommands(CommandBuffer commandBuffer, Shader shader, bool surfaces, bool volumes, bool volumesTwoPass)
	{
		List<Water> list = ((_CustomWaterRenderList != null) ? _CustomWaterRenderList : ApplicationSingleton<WaterSystem>.Instance.Waters);
		if (volumes)
		{
			for (int num = list.Count - 1; num >= 0; num--)
			{
				list[num].Renderer.RenderVolumes(commandBuffer, shader, volumesTwoPass);
			}
		}
		if (surfaces)
		{
			for (int num2 = list.Count - 1; num2 >= 0; num2--)
			{
				list[num2].Renderer.Render(_CameraComponent, _GeometryType, commandBuffer, shader);
			}
		}
	}

	internal void AddWaterMasksRenderCommands(CommandBuffer commandBuffer)
	{
		List<Water> list = ((_CustomWaterRenderList != null) ? _CustomWaterRenderList : ApplicationSingleton<WaterSystem>.Instance.Waters);
		for (int num = list.Count - 1; num >= 0; num--)
		{
			list[num].Renderer.RenderMasks(commandBuffer);
		}
	}

	private void ReleaseTemporaryTextures()
	{
		if (_Gbuffer0Tex != null)
		{
			RenderTexture.ReleaseTemporary(_Gbuffer0Tex);
			_Gbuffer0Tex = null;
		}
		if (_DepthTex2 != null)
		{
			RenderTexture.ReleaseTemporary(_DepthTex2);
			_DepthTex2 = null;
		}
	}

	private void CopyFrom(WaterCamera waterCamera)
	{
		_LocalMapsRect = waterCamera._LocalMapsRect;
		_LocalMapsRectPrevious = waterCamera._LocalMapsRectPrevious;
		_GeometryType = waterCamera._GeometryType;
	}

	private void RenderWaterDirect()
	{
		List<Water> list = ((_CustomWaterRenderList != null) ? _CustomWaterRenderList : ApplicationSingleton<WaterSystem>.Instance.Waters);
		for (int num = list.Count - 1; num >= 0; num--)
		{
			list[num].Renderer.Render(_CameraComponent, _GeometryType);
		}
	}

	private void RenderWaterBeforeImageEffectsOpaque()
	{
		List<Water> list = ((_CustomWaterRenderList != null) ? _CustomWaterRenderList : ApplicationSingleton<WaterSystem>.Instance.Waters);
		for (int num = list.Count - 1; num >= 0; num--)
		{
			if (list[num].WaterCommandBuffer == null)
			{
				list[num].WaterCommandBuffer = new CommandBuffer
				{
					name = "[UWS] - RenderWater"
				};
			}
			list[num].WaterCommandBuffer.Clear();
			list[num].Renderer.RenderWaterBeforeImageEffectsOpaque(_CameraComponent, _GeometryType, list[num].WaterCommandBuffer);
		}
	}

	private void RenderWaterEffects()
	{
		List<Water> list = ((_CustomWaterRenderList != null) ? _CustomWaterRenderList : ApplicationSingleton<WaterSystem>.Instance.Waters);
		for (int num = list.Count - 1; num >= 0; num--)
		{
			list[num].Renderer.RenderEffects(this);
		}
	}

	internal void OnRenderImageCallback(RenderTexture source, RenderTexture destination)
	{
		if (!ShouldRender())
		{
			Graphics.Blit(source, destination);
			return;
		}
		if (_RenderMode != WaterRenderMode.DefaultQueue)
		{
			List<Water> list = ((_CustomWaterRenderList != null) ? _CustomWaterRenderList : ApplicationSingleton<WaterSystem>.Instance.Waters);
			for (int num = list.Count - 1; num >= 0; num--)
			{
				list[num].Volume.EnableRenderers();
			}
		}
		switch (_RenderMode)
		{
		case WaterRenderMode.ImageEffectForward:
			_ForwardModule.Render(this, source, destination);
			break;
		case WaterRenderMode.ImageEffectDeferred:
		{
			RenderTexture temporary = RenderTexture.GetTemporary(Mathf.RoundToInt((float)_CameraComponent.pixelWidth * _SuperSampling) + 1, Mathf.RoundToInt((float)_CameraComponent.pixelHeight * _SuperSampling), 32, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear);
			temporary.filterMode = FilterMode.Point;
			source.filterMode = FilterMode.Point;
			Shader.SetGlobalTexture(ShaderVariables.RefractTex, source);
			RenderWaterDeferred(temporary, destination);
			RenderTexture.ReleaseTemporary(temporary);
			break;
		}
		default:
			Graphics.Blit(source, destination);
			break;
		}
		if (_RenderMode != WaterRenderMode.DefaultQueue)
		{
			List<Water> list2 = ((_CustomWaterRenderList != null) ? _CustomWaterRenderList : ApplicationSingleton<WaterSystem>.Instance.Waters);
			for (int num2 = list2.Count - 1; num2 >= 0; num2--)
			{
				list2[num2].Volume.DisableRenderers();
			}
		}
	}

	private void ReleaseImageEffectTemporaryTextures()
	{
		if (_DeferredRenderTarget != null)
		{
			RenderTexture.ReleaseTemporary(_DeferredRenderTarget);
			_DeferredRenderTarget = null;
		}
	}

	private void AddUtilityCommands()
	{
		if (_ImageEffectCommands == null && _RenderMode == WaterRenderMode.ImageEffectDeferred)
		{
			_ImageEffectCommands = new CommandBuffer
			{
				name = "[PW Water] Set Buffers"
			};
			_ImageEffectCommands.SetGlobalTexture(ShaderVariables.Gbuffer0, BuiltinRenderTextureType.GBuffer0);
			_ImageEffectCommands.SetGlobalTexture(ShaderVariables.Gbuffer1, BuiltinRenderTextureType.GBuffer1);
			_ImageEffectCommands.SetGlobalTexture(ShaderVariables.Gbuffer2, BuiltinRenderTextureType.GBuffer2);
			_ImageEffectCommands.SetGlobalTexture(ShaderVariables.Gbuffer3, BuiltinRenderTextureType.Reflections);
			_ImageEffectCommands.SetGlobalTexture(ShaderVariables.WaterlessDepthTexture, BuiltinRenderTextureType.ResolvedDepth);
			_CameraComponent.RemoveCommandBuffer(CameraEvent.AfterLighting, _ImageEffectCommands);
			_CameraComponent.AddCommandBuffer(CameraEvent.AfterLighting, _ImageEffectCommands);
		}
	}

	private void RemoveUtilityCommands()
	{
		RemoveCommandBuffer(CameraEvent.AfterLighting, "[PW Water] Set Buffers");
		if (_ImageEffectCommands != null)
		{
			_ImageEffectCommands.Dispose();
			_ImageEffectCommands = null;
		}
	}

	private void RemoveCommandBuffer(CameraEvent cameraEvent, string bufferName)
	{
		CommandBuffer[] commandBuffers = _CameraComponent.GetCommandBuffers(cameraEvent);
		for (int num = commandBuffers.Length - 1; num >= 0; num--)
		{
			if (commandBuffers[num].name == bufferName)
			{
				_CameraComponent.RemoveCommandBuffer(cameraEvent, commandBuffers[num]);
				break;
			}
		}
	}

	private void RenderWaterDeferred(RenderTexture temp, RenderTexture target)
	{
		Camera waterRenderCamera = _WaterRenderCamera;
		waterRenderCamera.CopyFrom(_CameraComponent);
		Water mainWater = MainWater;
		if (_RenderMode == WaterRenderMode.ImageEffectDeferred)
		{
			_FinalColorMixMaterial.SetMatrix(ShaderVariables.UnityMatrixVPInverse, Matrix4x4.Inverse(GL.GetGPUProjectionMatrix(_CameraComponent.projectionMatrix, renderIntoTexture: true) * _CameraComponent.worldToCameraMatrix));
			_Gbuffer123MixMaterial.SetFloat(ShaderVariables.DepthClipMultiplier, -1f);
			if (_Gbuffer0Tex == null)
			{
				_Gbuffer0Tex = RenderTexture.GetTemporary(_CameraComponent.pixelWidth + 1, _CameraComponent.pixelHeight, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
				_Gbuffer0Tex.filterMode = FilterMode.Point;
			}
			if (_DepthTex2 == null)
			{
				_DepthTex2 = RenderTexture.GetTemporary(_CameraComponent.pixelWidth + 1, _CameraComponent.pixelHeight, (_BlendedDepthTexturesFormat == RenderTextureFormat.Depth) ? 32 : 0, _BlendedDepthTexturesFormat, RenderTextureReadWrite.Linear);
				_DepthTex2.filterMode = FilterMode.Point;
			}
			CommandBuffer utilityCommandBuffer = _UtilityCommandBuffer;
			utilityCommandBuffer.Clear();
			utilityCommandBuffer.name = "[PW Water] Blend Deferred Results";
			Material depthBlitCopyMaterial = _DepthBlitCopyMaterial;
			utilityCommandBuffer.SetRenderTarget(_DepthTex2);
			utilityCommandBuffer.DrawMesh(Quads.BipolarXInversedY, Matrix4x4.identity, depthBlitCopyMaterial, 0, (_BlendedDepthTexturesFormat == RenderTextureFormat.Depth) ? 5 : 2);
			utilityCommandBuffer.Blit(BuiltinRenderTextureType.GBuffer0, _Gbuffer0Tex, _Gbuffer0MixMaterial, 0);
			utilityCommandBuffer.SetRenderTarget(_DeferredTargets, BuiltinRenderTextureType.Reflections);
			utilityCommandBuffer.DrawMesh(Quads.BipolarXY, Matrix4x4.identity, _Gbuffer123MixMaterial, 0);
			utilityCommandBuffer.SetRenderTarget(target);
			utilityCommandBuffer.SetGlobalTexture("_WaterColorTex", BuiltinRenderTextureType.CameraTarget);
			utilityCommandBuffer.DrawMesh(Quads.BipolarXInversedY, Matrix4x4.identity, _FinalColorMixMaterial, 0, 0, (mainWater != null) ? mainWater.Renderer.PropertyBlock : null);
			utilityCommandBuffer.SetGlobalTexture("_CameraDepthTexture", _DepthTex2);
			utilityCommandBuffer.SetGlobalTexture("_CameraGBufferTexture0", _Gbuffer0Tex);
			utilityCommandBuffer.SetGlobalTexture("_CameraGBufferTexture1", BuiltinRenderTextureType.GBuffer1);
			utilityCommandBuffer.SetGlobalTexture("_CameraGBufferTexture2", BuiltinRenderTextureType.GBuffer2);
			utilityCommandBuffer.SetGlobalTexture("_CameraGBufferTexture3", BuiltinRenderTextureType.Reflections);
			utilityCommandBuffer.SetGlobalTexture("_CameraReflectionsTexture", BuiltinRenderTextureType.Reflections);
			waterRenderCamera.AddCommandBuffer(CameraEvent.AfterEverything, utilityCommandBuffer);
		}
		Shader customShader = GraphicsSettings.GetCustomShader(BuiltinShaderType.DeferredReflections);
		Shader customShader2 = GraphicsSettings.GetCustomShader(BuiltinShaderType.DeferredShading);
		BuiltinShaderMode shaderMode = GraphicsSettings.GetShaderMode(BuiltinShaderType.DeferredReflections);
		BuiltinShaderMode shaderMode2 = GraphicsSettings.GetShaderMode(BuiltinShaderType.DeferredShading);
		GraphicsSettings.SetCustomShader(BuiltinShaderType.DeferredReflections, _DeferredReflections);
		GraphicsSettings.SetCustomShader(BuiltinShaderType.DeferredShading, _DeferredShading);
		GraphicsSettings.SetShaderMode(BuiltinShaderType.DeferredReflections, BuiltinShaderMode.UseCustom);
		GraphicsSettings.SetShaderMode(BuiltinShaderType.DeferredShading, BuiltinShaderMode.UseCustom);
		if (mainWater != null)
		{
			Shader.SetGlobalVector("_MainWaterWrapSubsurfaceScatteringPack", mainWater.Renderer.PropertyBlock.GetVector("_WrapSubsurfaceScatteringPack"));
		}
		waterRenderCamera.GetComponent<WaterCamera>().CopyFrom(this);
		waterRenderCamera.enabled = false;
		waterRenderCamera.clearFlags = CameraClearFlags.Color;
		waterRenderCamera.backgroundColor = new Color(0f, 0f, 0f, 0f);
		waterRenderCamera.depthTextureMode = DepthTextureMode.Depth;
		waterRenderCamera.renderingPath = RenderingPath.DeferredShading;
		waterRenderCamera.allowHDR = true;
		waterRenderCamera.targetTexture = temp;
		waterRenderCamera.cullingMask = 1 << WaterProjectSettings.Instance.WaterLayer;
		waterRenderCamera.Render();
		waterRenderCamera.targetTexture = null;
		GraphicsSettings.SetCustomShader(BuiltinShaderType.DeferredReflections, customShader);
		GraphicsSettings.SetCustomShader(BuiltinShaderType.DeferredShading, customShader2);
		GraphicsSettings.SetShaderMode(BuiltinShaderType.DeferredReflections, shaderMode);
		GraphicsSettings.SetShaderMode(BuiltinShaderType.DeferredShading, shaderMode2);
		Shader.SetGlobalTexture("_CameraDepthTexture", _DepthTex2);
		if (_RenderMode == WaterRenderMode.ImageEffectDeferred)
		{
			waterRenderCamera.RemoveCommandBuffer(CameraEvent.AfterEverything, _UtilityCommandBufferCache);
		}
	}

	private void EnableEffects()
	{
		if (_WaterCameraType == CameraType.Normal)
		{
			_PixelWidth = Mathf.RoundToInt(_CameraComponent.pixelWidth);
			_PixelHeight = Mathf.RoundToInt(_CameraComponent.pixelHeight);
			if (WaterProjectSettings.Instance.SinglePassStereoRendering)
			{
				_PixelWidth = Mathf.CeilToInt((float)(_PixelWidth << 1) / 256f) * 256;
			}
			_EffectsEnabled = true;
			if (_RenderWaterDepth || _RenderVolumes)
			{
				_CameraComponent.depthTextureMode |= DepthTextureMode.Depth;
			}
		}
	}

	private void DisableEffects()
	{
		_EffectsEnabled = false;
	}

	protected Camera CreateEffectsCamera(CameraType type)
	{
		GameObject gameObject = new GameObject(name + " Water Effects Camera")
		{
			hideFlags = HideFlags.HideAndDontSave
		};
		Camera camera = gameObject.AddComponent<Camera>();
		camera.enabled = false;
		camera.useOcclusionCulling = false;
		WaterCamera waterCamera = gameObject.AddComponent<WaterCamera>();
		waterCamera._WaterCameraType = type;
		waterCamera._MainCamera = _CameraComponent;
		waterCamera._BaseCamera = this;
		_EnabledWaterCameras.Remove(waterCamera);
		return camera;
	}

	private void RenderShadowEnforcers()
	{
		if (_ShadowsEnforcerMesh == null)
		{
			_ShadowsEnforcerMesh = new Mesh
			{
				name = "Water Shadow Enforcer",
				hideFlags = HideFlags.DontSave,
				vertices = new Vector3[4]
			};
			_ShadowsEnforcerMesh.SetIndices(new int[4] { 0, 1, 2, 3 }, MeshTopology.Quads, 0);
			_ShadowsEnforcerMesh.UploadMeshData(markNoLongerReadable: true);
			_ShadowsEnforcerMaterial = new Material(_ShadowEnforcerShader)
			{
				hideFlags = HideFlags.DontSave
			};
		}
		Bounds bounds = default;
		float shadowDistance = QualitySettings.shadowDistance;
		Vector3 point = _CameraComponent.ViewportPointToRay(new Vector3(_ShadowedWaterRect.xMin, _ShadowedWaterRect.yMin, 1f)).GetPoint(shadowDistance * 1.5f);
		Vector3 point2 = _CameraComponent.ViewportPointToRay(new Vector3(_ShadowedWaterRect.xMax, _ShadowedWaterRect.yMax, 1f)).GetPoint(shadowDistance * 1.5f);
		SetBoundsMinMaxComponentWise(ref bounds, point, point2);
		bounds.Encapsulate(_CameraComponent.ViewportPointToRay(new Vector3(_ShadowedWaterRect.xMin, _ShadowedWaterRect.yMax, 1f)).GetPoint(shadowDistance * 0.3f));
		bounds.Encapsulate(_CameraComponent.ViewportPointToRay(new Vector3(_ShadowedWaterRect.xMax, _ShadowedWaterRect.yMin, 1f)).GetPoint(shadowDistance * 0.3f));
		_ShadowsEnforcerMesh.bounds = bounds;
		Graphics.DrawMesh(_ShadowsEnforcerMesh, Matrix4x4.identity, _ShadowsEnforcerMaterial, 0);
	}

	private void PrepareToRender()
	{
		_ShadowedWaterRect = new Rect(1f, 1f, -1f, -1f);
		float radius = 4f * _CameraComponent.nearClipPlane * Mathf.Tan(0.5f * _CameraComponent.fieldOfView * (MathF.PI / 180f));
		Water water = Water.FindWater(transform.position, radius, _CustomWaterRenderList, out _IsInsideSubtractiveVolume, out _IsInsideAdditiveVolume);
		if (water != _ContainingWater)
		{
			if (_ContainingWater != null && _SubmersionState != SubmersionState.None)
			{
				_SubmersionState = SubmersionState.None;
				SubmersionStateChanged.Invoke(this);
			}
			_ContainingWater = water;
			_SubmersionState = SubmersionState.None;
			if (water != null && water.Volume.Boundless)
			{
				_CameraSubmersion.Create();
			}
		}
		else
		{
			SubmersionState state = _CameraSubmersion.State;
			if (state != _SubmersionState)
			{
				_SubmersionState = state;
				SubmersionStateChanged.Invoke(this);
			}
		}
	}

	private static void SetFallbackTextures()
	{
		Shader.SetGlobalTexture(ShaderVariables.UnderwaterMask, DefaultTextures.Get(Color.clear));
		Shader.SetGlobalTexture(ShaderVariables.DisplacementsMask, DefaultTextures.Get(Color.white));
	}

	private void ToggleEffects()
	{
		bool flag = WaterSystem.IsWaterPossiblyVisible();
		if (!_EffectsEnabled)
		{
			if (flag)
			{
				EnableEffects();
			}
		}
		else if (!flag)
		{
			DisableEffects();
		}
		int num = Mathf.RoundToInt(_CameraComponent.pixelWidth);
		int num2 = Mathf.RoundToInt(_CameraComponent.pixelHeight);
		if (WaterProjectSettings.Instance.SinglePassStereoRendering)
		{
			num = Mathf.CeilToInt((float)(num << 1) / 256f) * 256;
		}
		if (_EffectsEnabled && (num != _PixelWidth || num2 != _PixelHeight))
		{
			DisableEffects();
			EnableEffects();
			if (RenderTargetResized != null)
			{
				RenderTargetResized(this);
			}
		}
	}

	private void SetPlaneProjectorMatrix(bool isRenderTarget, bool flipY)
	{
		Camera planeProjectorCamera = PlaneProjectorCamera;
		Shader.SetGlobalMatrix("_WaterProjectorPreviousVP", _LastPlaneProjectorMatrix);
		planeProjectorCamera.CopyFrom(_CameraComponent);
		planeProjectorCamera.renderingPath = RenderingPath.Forward;
		planeProjectorCamera.ResetProjectionMatrix();
		planeProjectorCamera.projectionMatrix = _CameraComponent.projectionMatrix;
		Matrix4x4 proj = (flipY ? (_CameraComponent.projectionMatrix * Matrix4x4.Scale(new Vector3(1f, -1f, 1f))) : _CameraComponent.projectionMatrix);
		_LastPlaneProjectorMatrix = GL.GetGPUProjectionMatrix(proj, isRenderTarget) * _CameraComponent.worldToCameraMatrix;
		Shader.SetGlobalMatrix("_WaterProjectorVP", _LastPlaneProjectorMatrix);
	}

	private void SetLocalMapCoordinates()
	{
		int num = Mathf.NextPowerOfTwo(_CameraComponent.pixelWidth + _CameraComponent.pixelHeight >> 1);
		float num2 = 0f;
		float num3 = 0f;
		List<Water> waters = ApplicationSingleton<WaterSystem>.Instance.Waters;
		for (int num4 = waters.Count - 1; num4 >= 0; num4--)
		{
			Water water = waters[num4];
			num2 += water.MaxVerticalDisplacement;
			float y = water.transform.position.y;
			if (num3 < y)
			{
				num3 = y;
			}
		}
		Vector3 position = _CameraComponent.transform.position;
		Vector3 forward = _CameraComponent.transform.forward;
		float f = Mathf.Min(1f, forward.y + 1f);
		float num5 = Mathf.Abs(position.y) * (1f + 7f * Mathf.Sqrt(f));
		float num6 = num2 * 2.5f;
		float num7 = ((num5 > num6) ? num5 : num6);
		if (num7 < 20f)
		{
			num7 = 20f;
		}
		Vector3 vector = new Vector3(position.x + forward.x * num7 * 0.4f, 0f, position.z + forward.z * num7 * 0.4f);
		_LocalMapsRectPrevious = _LocalMapsRect;
		float num8 = num7 / (float)num;
		_LocalMapsRect = new Rect(vector.x - num7 + num8, vector.z - num7 + num8, 2f * num7, 2f * num7);
		float num9 = 1f / _LocalMapsRectPrevious.width;
		Shader.SetGlobalVector(ShaderVariables.LocalMapsCoordsPrevious, new Vector4((0f - _LocalMapsRectPrevious.xMin) * num9, (0f - _LocalMapsRectPrevious.yMin) * num9, num9, _LocalMapsRectPrevious.width));
		float num10 = 1f / _LocalMapsRect.width;
		Shader.SetGlobalVector(ShaderVariables.LocalMapsCoords, new Vector4((0f - _LocalMapsRect.xMin) * num10, (0f - _LocalMapsRect.yMin) * num10, num10, _LocalMapsRect.width));
	}

	private bool ShouldRender()
	{
		return (byte)(1u & ((enabled && Application.isPlaying) ? 1u : 0u) & (WaterSystem.IsWaterPossiblyVisible() ? 1u : 0u) & ((!WaterUtilities.IsSceneViewCamera(_CameraComponent) || WaterProjectSettings.Instance.RenderInSceneView) ? 1u : 0u)) != 0;
	}

	private static void SetBoundsMinMaxComponentWise(ref Bounds bounds, Vector3 a, Vector3 b)
	{
		if (a.x > b.x)
		{
			float x = b.x;
			b.x = a.x;
			a.x = x;
		}
		if (a.y > b.y)
		{
			float y = b.y;
			b.y = a.y;
			a.y = y;
		}
		if (a.z > b.z)
		{
			float z = b.z;
			b.z = a.z;
			a.z = z;
		}
		bounds.SetMinMax(a, b);
	}
}
