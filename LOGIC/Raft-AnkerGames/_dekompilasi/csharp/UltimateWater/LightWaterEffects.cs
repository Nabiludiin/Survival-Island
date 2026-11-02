using System;
using System.Collections.Generic;
using UltimateWater.Internal;
using UnityEngine;
using UnityEngine.Rendering;

namespace UltimateWater;

[RequireComponent(typeof(Light))]
public sealed class LightWaterEffects : MonoBehaviour
{
	public enum CausticsMode
	{
		None,
		ProjectedTexture,
		Raymarching
	}

	[Range(0f, 3f)]
	public float Intensity = 1f;

	public bool CastShadows = true;

	public Texture2D ProjectedTexture;

	[SerializeField]
	public float UvScale = 1f;

	[Range(0f, 0.25f)]
	public float ScrollSpeed = 0.01f;

	[Range(0f, 8f)]
	public float Distortions1 = 1f;

	[Range(0f, 8f)]
	public float Distortions2 = 1f;

	[Tooltip("Optional.")]
	[SerializeField]
	public Transform ScrollDirectionPointer;

	[HideInInspector]
	[SerializeField]
	private Shader _WorldPosShader;

	[HideInInspector]
	[SerializeField]
	private Shader _CausticsMapShader;

	[HideInInspector]
	[SerializeField]
	private Shader _NormalMapperShader;

	[HideInInspector]
	[SerializeField]
	private Shader _CausticUtilShader;

	[SerializeField]
	private CausticsMode _CausticsMode = CausticsMode.ProjectedTexture;

	[SerializeField]
	private LayerMask _CausticReceiversMask = int.MaxValue;

	[SerializeField]
	private Blur _Blur;

	[Tooltip("Causes minor allocation per frame (no way around it), but makes caustics rendering a lot faster. Disable it, if you don't use terrains.")]
	[SerializeField]
	private bool _SkipTerrainTrees = true;

	private Camera _RenderCamera;

	private WaterCamera _WaterCamera;

	private Material _CausticUtilMat;

	private Light _LocalLight;

	private Vector2 _Offset;

	private Vector2 _Scroll;

	private bool _RenderingPrepared;

	private bool[] _TerrainSettingTemp;

	private int _Id;

	private CommandBuffer _CopyShadowmap;

	private RenderTexture _WorldPosMap;

	private RenderTexture _CausticsMap;

	public static readonly List<LightWaterEffects> Lights = new List<LightWaterEffects>();

	private static int _ShadowmapId;

	public Light UnityLight => _LocalLight;

	public CausticsMode Mode
	{
		get
		{
			return _CausticsMode;
		}
		set
		{
			_CausticsMode = value;
			if (_CausticsMode == CausticsMode.None)
			{
				ResetCausticMaps();
				Camera.onPreCull = (Camera.CameraCallback)Delegate.Remove(Camera.onPreCull, new Camera.CameraCallback(OnSomeCameraGlobalPreCull));
				return;
			}
			if (_RenderCamera == null)
			{
				CreateCausticsCamera();
			}
			if (Camera.onPreCull != null)
			{
				Camera.onPreCull = (Camera.CameraCallback)Delegate.Remove(Camera.onPreCull, new Camera.CameraCallback(OnSomeCameraGlobalPreCull));
			}
			Camera.onPreCull = (Camera.CameraCallback)Delegate.Combine(Camera.onPreCull, new Camera.CameraCallback(OnSomeCameraGlobalPreCull));
		}
	}

	private void Awake()
	{
		_ShadowmapId = ShaderVariables.WaterShadowmap;
		_TerrainSettingTemp = new bool[32];
		_LocalLight = GetComponent<Light>();
		_CausticUtilMat = new Material(_CausticUtilShader)
		{
			hideFlags = HideFlags.DontSave
		};
	}

	private void OnEnable()
	{
		Lights.Add(this);
		OnValidate();
		if (_CausticsMode != CausticsMode.None)
		{
			CreateCausticsCamera();
		}
		else
		{
			ResetCausticMaps();
		}
		_Id = Lights.Count - 1;
		Camera.onPreCull = (Camera.CameraCallback)Delegate.Remove(Camera.onPreCull, new Camera.CameraCallback(OnSomeCameraGlobalPreCull));
		Camera.onPreCull = (Camera.CameraCallback)Delegate.Combine(Camera.onPreCull, new Camera.CameraCallback(OnSomeCameraGlobalPreCull));
	}

	private void OnDisable()
	{
		Camera.onPreCull = (Camera.CameraCallback)Delegate.Remove(Camera.onPreCull, new Camera.CameraCallback(OnSomeCameraGlobalPreCull));
		ResetCausticMaps();
		Lights.Remove(this);
		TextureUtility.Release(ref _WorldPosMap);
		TextureUtility.Release(ref _CausticsMap);
		if (_RenderCamera != null)
		{
			_RenderCamera.gameObject.Destroy();
			_RenderCamera = null;
		}
		if (_WaterCamera != null)
		{
			_WaterCamera.gameObject.Destroy();
			_WaterCamera = null;
		}
	}

	private void Update()
	{
		if (_RenderCamera == null)
		{
			CreateCausticsCamera();
		}
		if (ScrollDirectionPointer != null)
		{
			Vector3 forward = ScrollDirectionPointer.forward;
			float num = ScrollSpeed * UvScale * Time.deltaTime;
			_Scroll.x += forward.x * num;
			_Scroll.y += forward.z * num;
		}
		else
		{
			float num2 = 0.7f * ScrollSpeed * UvScale * Time.deltaTime;
			_Scroll.x += num2;
			_Scroll.y += num2;
		}
	}

	private void OnValidate()
	{
		if (_WorldPosShader == null)
		{
			_WorldPosShader = Shader.Find("UltimateWater/Caustics/WorldPos");
		}
		if (_CausticsMapShader == null)
		{
			_CausticsMapShader = Shader.Find("UltimateWater/Caustics/Map");
		}
		if (_NormalMapperShader == null)
		{
			_NormalMapperShader = Shader.Find("UltimateWater/Caustics/NormalMapper");
		}
		if (_CausticUtilShader == null)
		{
			_CausticUtilShader = Shader.Find("UltimateWater/Caustics/Utility");
		}
		_Blur.Validate();
		if (_CausticsMode == CausticsMode.None)
		{
			ResetCausticMaps();
		}
	}

	public void PrepareRenderingOnCamera(WaterCamera targetCamera)
	{
		if (!_RenderingPrepared && isActiveAndEnabled)
		{
			_RenderingPrepared = true;
			if (CastShadows)
			{
				PrepareShadows(targetCamera.CameraComponent);
			}
			PrepareCaustics(targetCamera);
		}
	}

	public void CleanRenderingOnCamera()
	{
		if (_RenderingPrepared)
		{
			_RenderingPrepared = false;
			if (_CopyShadowmap != null)
			{
				_LocalLight.RemoveCommandBuffer(LightEvent.AfterScreenspaceMask, _CopyShadowmap);
			}
		}
	}

	public void AddWorldSpaceOffset(Vector3 offset)
	{
		if (isActiveAndEnabled)
		{
			_Offset.x += Vector3.Dot(offset, _RenderCamera.transform.right) * UvScale / (_RenderCamera.orthographicSize * 2f);
			_Offset.y += Vector3.Dot(offset, _RenderCamera.transform.up) * UvScale / (_RenderCamera.orthographicSize * 2f);
		}
	}

	private void OnSomeCameraGlobalPreCull(Camera cameraComponent)
	{
		if (_RenderingPrepared)
		{
			transform.position = new Vector3(_Id, 6.137f, 0f);
		}
	}

	private void CreateCausticsCamera()
	{
		if (_CausticsMode == CausticsMode.ProjectedTexture)
		{
			_CausticsMap = new RenderTexture(256, 256, 0, RenderTextureFormat.RGHalf, RenderTextureReadWrite.Linear)
			{
				hideFlags = HideFlags.DontSave,
				wrapMode = TextureWrapMode.Repeat,
				name = "[UWS] LightWaterEffects - Caustics Map"
			};
		}
		else
		{
			_WorldPosMap = new RenderTexture(256, 256, 32, RenderTextureFormat.Depth, RenderTextureReadWrite.Linear)
			{
				hideFlags = HideFlags.DontSave,
				wrapMode = TextureWrapMode.Clamp,
				name = "[UWS] LightWaterEffects - WorldPosMap"
			};
			_CausticsMap = new RenderTexture(512, 512, 0, RenderTextureFormat.RHalf, RenderTextureReadWrite.Linear)
			{
				hideFlags = HideFlags.DontSave,
				wrapMode = TextureWrapMode.Clamp,
				name = "[UWS] LightWaterEffects - CausticsMap"
			};
		}
		GameObject gameObject = new GameObject("Caustic Camera")
		{
			hideFlags = HideFlags.DontSave
		};
		gameObject.transform.position = transform.position;
		gameObject.transform.rotation = transform.rotation;
		_RenderCamera = gameObject.AddComponent<Camera>();
		_RenderCamera.enabled = false;
		_RenderCamera.orthographic = true;
		_RenderCamera.orthographicSize = 85f;
		_RenderCamera.farClipPlane = 5000f;
		_RenderCamera.depthTextureMode = DepthTextureMode.None;
		_RenderCamera.allowHDR = true;
		_RenderCamera.useOcclusionCulling = false;
		_RenderCamera.backgroundColor = new Color(0f, 0f, 0f, 0f);
		_RenderCamera.renderingPath = RenderingPath.VertexLit;
		_WaterCamera = gameObject.AddComponent<WaterCamera>();
		_WaterCamera.RenderWaterDepth = false;
		_WaterCamera.RenderVolumes = false;
		_WaterCamera.Type = WaterCamera.CameraType.Effect;
		_WaterCamera.GeometryType = WaterGeometryType.UniformGrid;
		gameObject.hideFlags |= HideFlags.HideInHierarchy;
	}

	private void PrepareShadows(Camera cameraComponent)
	{
		if (_CopyShadowmap == null)
		{
			_CopyShadowmap = new CommandBuffer
			{
				name = "[UWS] LightWaterEffects._CopyShadowmap"
			};
		}
		Shader.SetGlobalTexture(_ShadowmapId, DefaultTextures.Get(Color.white));
		_CopyShadowmap.Clear();
		_CopyShadowmap.GetTemporaryRT(_ShadowmapId, cameraComponent.pixelWidth, cameraComponent.pixelHeight, 32, FilterMode.Point, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
		_CopyShadowmap.Blit(BuiltinRenderTextureType.CurrentActive, _ShadowmapId);
		_CopyShadowmap.ReleaseTemporaryRT(_ShadowmapId);
		_LocalLight.RemoveCommandBuffer(LightEvent.AfterScreenspaceMask, _CopyShadowmap);
		_LocalLight.AddCommandBuffer(LightEvent.AfterScreenspaceMask, _CopyShadowmap);
	}

	private void PrepareCaustics(WaterCamera waterCamera)
	{
		switch (_CausticsMode)
		{
		case CausticsMode.ProjectedTexture:
			UpdateCausticsCameraPosition(waterCamera);
			RenderProjectedTextureCaustics();
			break;
		case CausticsMode.Raymarching:
			UpdateCausticsCameraPosition(waterCamera);
			RenderRaymarchedCaustics(waterCamera);
			break;
		}
	}

	private void UpdateCausticsCameraPosition(WaterCamera waterCamera)
	{
		Vector2 center = waterCamera.LocalMapsRect.center;
		_RenderCamera.transform.position = new Vector3(center.x, 0f, center.y) - transform.forward * Mathf.Max(Mathf.Abs(waterCamera.transform.position.y * 2.2f), 300f);
		_RenderCamera.transform.rotation = transform.rotation;
	}

	private void RenderProjectedTextureCaustics()
	{
		_RenderCamera.cullingMask = 1 << WaterProjectSettings.Instance.WaterLayer;
		_WaterCamera.RenderWaterWithShader("[PW Water] Caustics Normal Map", _CausticsMap, _NormalMapperShader, surfaces: true, volumes: false, volumesTwoPass: false);
		Vector3 position = _RenderCamera.transform.position;
		float num = Vector3.Dot(position, _RenderCamera.transform.right) * UvScale / (_RenderCamera.orthographicSize * 3.5f);
		float num2 = Vector3.Dot(position, _RenderCamera.transform.up) * UvScale / (_RenderCamera.orthographicSize * 2f);
		Shader.SetGlobalTexture("_CausticsMap", ProjectedTexture);
		Shader.SetGlobalTexture("_CausticsDistortionMap", _CausticsMap);
		Shader.SetGlobalFloat("_CausticsMultiplier", Intensity * 5f);
		Shader.SetGlobalVector("_CausticsOffsetScale", new Vector4(_Offset.x + _Scroll.x + num, _Offset.y + _Scroll.y + num2, UvScale, Distortions1 * 0.02f));
		Shader.SetGlobalVector("_CausticsOffsetScale2", new Vector4(_Offset.x - _Scroll.x + num + 0.5f, _Offset.y - _Scroll.y + num2, UvScale, Distortions2 * 0.02f));
		Shader.SetGlobalMatrix("_CausticsMapProj", GL.GetGPUProjectionMatrix(_RenderCamera.projectionMatrix, renderIntoTexture: true) * _RenderCamera.worldToCameraMatrix);
	}

	private void RenderRaymarchedCaustics(WaterCamera waterCamera)
	{
		_CausticUtilMat.SetMatrix("_InvProjMatrix", Matrix4x4.Inverse(_RenderCamera.projectionMatrix * _RenderCamera.worldToCameraMatrix));
		Graphics.SetRenderTarget(_WorldPosMap);
		GL.Clear(clearDepth: true, clearColor: true, new Color(1f, 0f, 0f, 0f), 1f);
		Terrain[] array = null;
		if (_SkipTerrainTrees)
		{
			array = Terrain.activeTerrains;
			if (_TerrainSettingTemp.Length < array.Length)
			{
				Array.Resize(ref _TerrainSettingTemp, array.Length * 2);
			}
			for (int i = 0; i < array.Length; i++)
			{
				_TerrainSettingTemp[i] = array[i].drawTreesAndFoliage;
				array[i].drawTreesAndFoliage = false;
			}
		}
		_WaterCamera.enabled = false;
		_RenderCamera.orthographicSize = waterCamera.LocalMapsRect.width * 0.6f;
		_RenderCamera.clearFlags = CameraClearFlags.Depth;
		_RenderCamera.cullingMask = _CausticReceiversMask;
		_RenderCamera.targetTexture = _WorldPosMap;
		_RenderCamera.RenderWithShader(_WorldPosShader, "RenderType");
		if (_SkipTerrainTrees)
		{
			for (int j = 0; j < array.Length; j++)
			{
				array[j].drawTreesAndFoliage = _TerrainSettingTemp[j];
			}
		}
		Shader.SetGlobalTexture("_WorldPosMap", _WorldPosMap);
		Shader.SetGlobalVector("_CausticLightDir", transform.forward);
		Shader.SetGlobalFloat("_CausticLightIntensity", _LocalLight.intensity * Intensity * 1.5f);
		_WaterCamera.enabled = true;
		_RenderCamera.clearFlags = CameraClearFlags.Color;
		_RenderCamera.cullingMask = 1 << WaterProjectSettings.Instance.WaterLayer;
		_RenderCamera.targetTexture = _CausticsMap;
		_RenderCamera.RenderWithShader(_CausticsMapShader, "CustomType");
		_Blur.Apply(_CausticsMap);
		Graphics.Blit(null, _CausticsMap, _CausticUtilMat, 1);
		Shader.SetGlobalTexture("_CausticsMap", _CausticsMap);
		Shader.SetGlobalFloat("_CausticsMultiplier", 1f);
		Shader.SetGlobalMatrix("_CausticsMapProj", GL.GetGPUProjectionMatrix(_RenderCamera.projectionMatrix, renderIntoTexture: true) * _RenderCamera.worldToCameraMatrix);
	}

	private void ResetCausticMaps()
	{
		Shader.SetGlobalFloat("_CausticsMultiplier", 0f);
	}
}
