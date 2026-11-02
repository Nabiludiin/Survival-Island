using System;
using System.Collections.Generic;
using UltimateWater.Internal;
using UnityEngine;

namespace UltimateWater;

public sealed class PlanarReflection : WaterModule
{
	[Serializable]
	public class Data
	{
		public LayerMask ReflectionMask = int.MaxValue;

		public bool ReflectSkybox = true;

		public bool RenderShadows = true;

		[Range(0f, 1f)]
		public float Resolution = 0.5f;

		[Range(0f, 1f)]
		[Tooltip("Allows you to use more rational resolution of planar reflections on screens with very high dpi. Planar reflections should be blurred anyway.")]
		public float RetinaResolution = 0.333f;
	}

	private readonly Data _Data;

	private readonly Water _Water;

	private readonly bool _SystemSupportsHdr;

	private readonly Dictionary<Camera, TemporaryRenderTexture> _TemporaryTargets = new Dictionary<Camera, TemporaryRenderTexture>();

	private TemporaryRenderTexture _CurrentTarget;

	private float _FinalResolutionMultiplier;

	private bool _RenderPlanarReflections;

	private Material _UtilitiesMaterial;

	private Shader _UtilitiesShader;

	private const float _ClipPlaneOffset = 0.07f;

	public float Resolution
	{
		get
		{
			return _Data.Resolution;
		}
		set
		{
			_Data.Resolution = value;
			CalculateResolutionMultiplier();
		}
	}

	public float RetinaResolution
	{
		get
		{
			return _Data.RetinaResolution;
		}
		set
		{
			_Data.RetinaResolution = value;
			CalculateResolutionMultiplier();
		}
	}

	public bool ReflectSkybox
	{
		get
		{
			return _Data.ReflectSkybox;
		}
		set
		{
			_Data.ReflectSkybox = value;
		}
	}

	public bool RenderShadows
	{
		get
		{
			return _Data.RenderShadows;
		}
		set
		{
			_Data.RenderShadows = value;
		}
	}

	public LayerMask ReflectionMask
	{
		get
		{
			return _Data.ReflectionMask;
		}
		set
		{
			_Data.ReflectionMask = value;
		}
	}

	public PlanarReflection(Water water, Data data)
	{
		_Water = water;
		_Data = data;
		_SystemSupportsHdr = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf);
		Validate();
		water.ProfilesManager.Changed.AddListener(OnProfilesChanged);
		OnProfilesChanged(water);
	}

	internal override void OnWaterPostRender(WaterCamera waterCamera)
	{
		Camera cameraComponent = waterCamera.CameraComponent;
		if (_TemporaryTargets.TryGetValue(cameraComponent, out var value))
		{
			_TemporaryTargets.Remove(cameraComponent);
			value.Dispose();
		}
	}

	internal override void Start(Water water)
	{
	}

	internal override void Enable()
	{
	}

	internal override void Disable()
	{
	}

	internal override void Validate()
	{
		if (_UtilitiesShader == null)
		{
			_UtilitiesShader = Shader.Find("UltimateWater/Utilities/PlanarReflection - Utilities");
		}
		_Data.Resolution = Mathf.Clamp01((float)Mathf.RoundToInt(_Data.Resolution * 10f) * 0.1f);
		_Data.RetinaResolution = Mathf.Clamp01((float)Mathf.RoundToInt(_Data.RetinaResolution * 10f) * 0.1f);
		CalculateResolutionMultiplier();
	}

	internal override void Destroy()
	{
		ClearRenderTextures();
	}

	internal override void Update()
	{
		ClearRenderTextures();
	}

	internal override void OnWaterRender(WaterCamera waterCamera)
	{
		Camera cameraComponent = waterCamera.CameraComponent;
		if (cameraComponent.enabled && _RenderPlanarReflections && !_TemporaryTargets.TryGetValue(cameraComponent, out _CurrentTarget))
		{
			Camera reflectionCamera = Reflection.GetReflectionCamera(cameraComponent);
			RenderReflection(cameraComponent, reflectionCamera);
			UpdateRenderProperties(reflectionCamera);
		}
	}

	private void CalculateResolutionMultiplier()
	{
		float num = ((Screen.dpi <= 220f) ? _Data.Resolution : _Data.RetinaResolution);
		if (_FinalResolutionMultiplier != num)
		{
			_FinalResolutionMultiplier = num;
			ClearRenderTextures();
		}
	}

	private void RenderReflection(Camera camera, Camera reflectionCamera)
	{
		reflectionCamera.cullingMask = _Data.ReflectionMask;
		SetCameraSettings(camera, reflectionCamera);
		_CurrentTarget = GetRenderTexture(camera.pixelWidth, camera.pixelHeight, reflectionCamera);
		_TemporaryTargets[camera] = _CurrentTarget;
		reflectionCamera.depthTextureMode = DepthTextureMode.None;
		TemporaryRenderTexture temporary = RenderTexturesCache.GetTemporary(_CurrentTarget.Texture.width, _CurrentTarget.Texture.height, 0, _CurrentTarget.Texture.format, linear: true, uav: false);
		reflectionCamera.targetTexture = temporary;
		reflectionCamera.transform.eulerAngles = CalculateReflectionAngles(camera);
		reflectionCamera.transform.position = CalculateReflectionPosition(camera);
		float w = 0f - _Water.transform.position.y - 0.07f;
		Vector4 plane = new Vector4(0f, 1f, 0f, w);
		Matrix4x4 zero = Matrix4x4.zero;
		zero = Reflection.CalculateReflectionMatrix(zero, plane);
		Vector3 position = zero.MultiplyPoint(camera.transform.position);
		reflectionCamera.worldToCameraMatrix = camera.worldToCameraMatrix * zero;
		Vector4 clipPlane = Reflection.CameraSpacePlane(reflectionCamera, _Water.transform.position, new Vector3(0f, 1f, 0f), 0.07f, 1f);
		Matrix4x4 projectionMatrix = camera.projectionMatrix;
		projectionMatrix = Reflection.CalculateObliqueMatrix(projectionMatrix, clipPlane);
		reflectionCamera.projectionMatrix = projectionMatrix;
		reflectionCamera.transform.position = position;
		Vector3 eulerAngles = camera.transform.eulerAngles;
		reflectionCamera.transform.eulerAngles = new Vector3(0f - eulerAngles.x, eulerAngles.y, eulerAngles.z);
		reflectionCamera.clearFlags = (_Data.ReflectSkybox ? CameraClearFlags.Skybox : CameraClearFlags.Color);
		if (_Data.RenderShadows)
		{
			GL.invertCulling = true;
			reflectionCamera.Render();
			GL.invertCulling = false;
		}
		else
		{
			ShadowQuality shadows = QualitySettings.shadows;
			QualitySettings.shadows = ShadowQuality.Disable;
			GL.invertCulling = true;
			reflectionCamera.Render();
			GL.invertCulling = false;
			QualitySettings.shadows = shadows;
		}
		reflectionCamera.targetTexture = null;
		if (_UtilitiesMaterial == null)
		{
			_UtilitiesMaterial = new Material(_UtilitiesShader)
			{
				hideFlags = HideFlags.DontSave
			};
		}
		Graphics.Blit((RenderTexture)temporary, _CurrentTarget, _UtilitiesMaterial, 0);
		temporary.Dispose();
	}

	private void UpdateRenderProperties(Camera reflectionCamera)
	{
		MaterialPropertyBlock propertyBlock = _Water.Renderer.PropertyBlock;
		propertyBlock.SetTexture(ShaderVariables.PlanarReflectionTex, (RenderTexture)_CurrentTarget);
		propertyBlock.SetMatrix("_PlanarReflectionProj", Matrix4x4.TRS(new Vector3(0.5f, 0.5f, 0f), Quaternion.identity, new Vector3(0.5f, 0.5f, 1f)) * reflectionCamera.projectionMatrix * reflectionCamera.worldToCameraMatrix);
		propertyBlock.SetFloat("_PlanarReflectionMipBias", 0f - Mathf.Log(1f / _FinalResolutionMultiplier, 2f));
	}

	private TemporaryRenderTexture GetRenderTexture(int width, int height, Camera reflectionCamera)
	{
		int width2 = Mathf.ClosestPowerOfTwo(Mathf.RoundToInt((float)width * _FinalResolutionMultiplier));
		int height2 = Mathf.ClosestPowerOfTwo(Mathf.RoundToInt((float)height * _FinalResolutionMultiplier));
		bool allowHDR = reflectionCamera.allowHDR;
		TemporaryRenderTexture temporary = RenderTexturesCache.GetTemporary(width2, height2, 0, (allowHDR && _SystemSupportsHdr && WaterProjectSettings.Instance.AllowFloatingPointMipMaps) ? RenderTextureFormat.ARGBHalf : RenderTextureFormat.ARGB32, linear: true, uav: false, mipMaps: true);
		temporary.Texture.filterMode = FilterMode.Trilinear;
		temporary.Texture.wrapMode = TextureWrapMode.Clamp;
		return temporary;
	}

	private void ClearRenderTextures()
	{
		Dictionary<Camera, TemporaryRenderTexture>.Enumerator enumerator = _TemporaryTargets.GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.Value.Dispose();
		}
		enumerator.Dispose();
		_TemporaryTargets.Clear();
	}

	private void OnProfilesChanged(Water water)
	{
		List<Water.WeightedProfile> profiles = water.ProfilesManager.Profiles;
		if (profiles != null)
		{
			float num = 0f;
			for (int num2 = profiles.Count - 1; num2 >= 0; num2--)
			{
				Water.WeightedProfile weightedProfile = profiles[num2];
				WaterProfileData profile = weightedProfile.Profile;
				float weight = weightedProfile.Weight;
				num += profile.PlanarReflectionIntensity * weight;
			}
			_RenderPlanarReflections = num > 0f;
		}
	}

	private void SetCameraSettings(Camera source, Camera destination)
	{
		destination.backgroundColor = new Color(0f, 0f, 0f, 0f);
		destination.fieldOfView = source.fieldOfView;
		destination.aspect = source.aspect;
		destination.allowHDR = _SystemSupportsHdr && source.allowHDR;
	}

	private Vector3 CalculateReflectionPosition(Camera camera)
	{
		Vector3 position = camera.transform.position;
		position.y = _Water.transform.position.y - position.y;
		return position;
	}

	private static Vector3 CalculateReflectionAngles(Camera camera)
	{
		Vector3 eulerAngles = camera.transform.eulerAngles;
		return new Vector3(0f - eulerAngles.x, eulerAngles.y, eulerAngles.z);
	}
}
