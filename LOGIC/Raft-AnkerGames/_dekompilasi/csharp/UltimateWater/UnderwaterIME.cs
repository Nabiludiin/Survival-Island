using System;
using UltimateWater.Internal;
using UnityEngine;
using UnityEngine.Rendering;

namespace UltimateWater;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(WaterCamera))]
[AddComponentMenu("Ultimate Water/Underwater IME")]
public sealed class UnderwaterIME : MonoBehaviour, IWaterImageEffect
{
	[SerializeField]
	private Blur _Blur;

	[SerializeField]
	private bool _UnderwaterAudio = true;

	[Tooltip("Individual camera blur scale. It's recommended to modify blur scale through water profiles. Use this one, only if some of your cameras need a clear view and some don't.")]
	[Range(0f, 4f)]
	[SerializeField]
	private float _CameraBlurScale = 1f;

	[Range(0.1f, 1f)]
	[SerializeField]
	private float _MaskResolution = 0.5f;

	private Material _MaskMaterial;

	private Material _ImeMaterial;

	private Material _NoiseMaterial;

	private Material _ComposeUnderwaterMaskMaterial;

	private Camera _LocalCamera;

	private WaterCamera _LocalWaterCamera;

	private AudioReverbFilter _ReverbFilter;

	private CommandBuffer _MaskCommandBuffer;

	private float _Intensity = float.NaN;

	private bool _RenderUnderwaterMask;

	private Water _WaterOverride;

	private bool _HasWaterOverride;

	private bool _EffectEnabled = true;

	private RenderTexture _UnderwaterMask;

	public float Intensity => _Intensity;

	public bool EffectEnabled
	{
		get
		{
			return _EffectEnabled;
		}
		set
		{
			_EffectEnabled = value;
		}
	}

	public Water WaterOverride
	{
		get
		{
			return _WaterOverride;
		}
		set
		{
			_WaterOverride = value;
			_HasWaterOverride = value != null;
			OnSubmersionStateChanged(_LocalWaterCamera);
		}
	}

	public void OnWaterCameraEnabled()
	{
		GetComponent<WaterCamera>().SubmersionStateChanged.AddListener(OnSubmersionStateChanged);
	}

	public void OnWaterCameraPreCull()
	{
		if (!_EffectEnabled)
		{
			enabled = false;
			return;
		}
		if (_HasWaterOverride)
		{
			enabled = true;
			_RenderUnderwaterMask = true;
			return;
		}
		switch (_LocalWaterCamera.SubmersionState)
		{
		case SubmersionState.None:
			enabled = false;
			break;
		case SubmersionState.Partial:
			enabled = true;
			_RenderUnderwaterMask = true;
			break;
		case SubmersionState.Full:
			enabled = true;
			_RenderUnderwaterMask = false;
			break;
		}
		float num = _LocalCamera.nearClipPlane * Mathf.Tan(_LocalCamera.fieldOfView * 0.5f * (MathF.PI / 180f));
		float effectsIntensity = (0f - (transform.position.y - _LocalWaterCamera.WaterLevel) + num) * 0.25f;
		SetEffectsIntensity(effectsIntensity);
	}

	private void Awake()
	{
		_LocalCamera = GetComponent<Camera>();
		_LocalWaterCamera = GetComponent<WaterCamera>();
		OnValidate();
		_MaskMaterial = ShaderUtility.Instance.CreateMaterial(ShaderList.ScreenSpaceMask, HideFlags.DontSave);
		_ImeMaterial = ShaderUtility.Instance.CreateMaterial(ShaderList.BaseIME, HideFlags.DontSave);
		_NoiseMaterial = ShaderUtility.Instance.CreateMaterial(ShaderList.Noise, HideFlags.DontSave);
		_ComposeUnderwaterMaskMaterial = ShaderUtility.Instance.CreateMaterial(ShaderList.ComposeUnderWaterMask, HideFlags.DontSave);
		_ReverbFilter = GetComponent<AudioReverbFilter>();
		if (_ReverbFilter == null && _UnderwaterAudio)
		{
			_ReverbFilter = gameObject.AddComponent<AudioReverbFilter>();
		}
	}

	private void OnDisable()
	{
		TextureUtility.Release(ref _UnderwaterMask);
		if (_MaskCommandBuffer != null)
		{
			_MaskCommandBuffer.Clear();
		}
	}

	private void OnDestroy()
	{
		if (_MaskCommandBuffer != null)
		{
			_MaskCommandBuffer.Dispose();
			_MaskCommandBuffer = null;
		}
		if (_Blur != null)
		{
			_Blur.Dispose();
			_Blur = null;
		}
		_MaskMaterial.Destroy();
		_ImeMaterial.Destroy();
	}

	private void OnValidate()
	{
		ShaderUtility.Instance.Use(ShaderList.ScreenSpaceMask);
		ShaderUtility.Instance.Use(ShaderList.BaseIME);
		ShaderUtility.Instance.Use(ShaderList.Noise);
		ShaderUtility.Instance.Use(ShaderList.ComposeUnderWaterMask);
		if (_Blur != null)
		{
			_Blur.Validate("UltimateWater/Utilities/Blur (Underwater)");
		}
	}

	private void OnPreCull()
	{
		RenderUnderwaterMask();
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Water water = (_HasWaterOverride ? _WaterOverride : _LocalWaterCamera.ContainingWater);
		if (!_LocalWaterCamera.enabled || water == null)
		{
			Graphics.Blit(source, destination);
			return;
		}
		source.filterMode = FilterMode.Bilinear;
		RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0, (destination != null) ? destination.format : source.format, RenderTextureReadWrite.Linear, 1, RenderTextureMemoryless.None, VRTextureUsage.None, useDynamicScale: false);
		temporary.filterMode = FilterMode.Bilinear;
		temporary.wrapMode = TextureWrapMode.Clamp;
		RenderDepthScatter(source, temporary);
		if (water.Materials.UnderwaterBlurSize * _CameraBlurScale > 0f)
		{
			_Blur.TotalSize = water.Materials.UnderwaterBlurSize * _CameraBlurScale;
			_Blur.Apply(temporary);
		}
		RenderDistortions(temporary, destination);
		RenderTexture.ReleaseTemporary(temporary);
	}

	private void RenderUnderwaterMask()
	{
		if (_MaskCommandBuffer == null)
		{
			return;
		}
		_MaskCommandBuffer.Clear();
		Water water = (_HasWaterOverride ? _WaterOverride : _LocalWaterCamera.ContainingWater);
		Camera current = Camera.current;
		int underwaterMask = ShaderVariables.UnderwaterMask;
		int underwaterMask2 = ShaderVariables.UnderwaterMask2;
		if (_UnderwaterMask == null)
		{
			int width = Mathf.RoundToInt((float)current.pixelWidth * _MaskResolution);
			int height = Mathf.RoundToInt((float)current.pixelHeight * _MaskResolution);
			_UnderwaterMask = new RenderTexture(width, height, 0, RenderTextureFormat.R8, RenderTextureReadWrite.Linear)
			{
				filterMode = FilterMode.Bilinear,
				name = "[UWS] UnderwaterIME - Mask"
			};
			_UnderwaterMask.Create();
		}
		if (_RenderUnderwaterMask || (water != null && water.Renderer.MaskCount > 0))
		{
			int width2 = Mathf.RoundToInt((float)current.pixelWidth * _MaskResolution);
			int height2 = Mathf.RoundToInt((float)current.pixelHeight * _MaskResolution);
			_MaskCommandBuffer.GetTemporaryRT(underwaterMask, width2, height2, 0, FilterMode.Bilinear, RenderTextureFormat.R8, RenderTextureReadWrite.Linear, 1);
			_MaskCommandBuffer.GetTemporaryRT(underwaterMask2, width2, height2, 0, FilterMode.Point, RenderTextureFormat.R8, RenderTextureReadWrite.Linear, 1);
		}
		else
		{
			_MaskCommandBuffer.GetTemporaryRT(underwaterMask, 4, 4, 0, FilterMode.Point, RenderTextureFormat.R8, RenderTextureReadWrite.Linear, 1);
		}
		if (_RenderUnderwaterMask && water != null)
		{
			_MaskMaterial.CopyPropertiesFromMaterial(water.Materials.SurfaceMaterial);
			_MaskCommandBuffer.SetRenderTarget(underwaterMask2);
			_MaskCommandBuffer.ClearRenderTarget(clearDepth: false, clearColor: true, Color.black);
			WaterGeometry geometry = water.Geometry;
			Mesh[] transformedMeshes = geometry.GetTransformedMeshes(_LocalCamera, out var matrix, (geometry.GeometryType == WaterGeometry.Type.ProjectionGrid) ? WaterGeometryType.RadialGrid : WaterGeometryType.Auto, volume: true, geometry.ComputeVertexCountForCamera(current));
			for (int num = transformedMeshes.Length - 1; num >= 0; num--)
			{
				_MaskCommandBuffer.DrawMesh(transformedMeshes[num], matrix, _MaskMaterial, 0, 0, water.Renderer.PropertyBlock);
			}
			_MaskCommandBuffer.SetRenderTarget(underwaterMask);
			_MaskCommandBuffer.DrawMesh(Quads.TriangleMeshInversedY, Matrix4x4.identity, _ImeMaterial, 0, 3, water.Renderer.PropertyBlock);
			_MaskCommandBuffer.ReleaseTemporaryRT(underwaterMask2);
		}
		else
		{
			_MaskCommandBuffer.SetRenderTarget(underwaterMask);
			_MaskCommandBuffer.ClearRenderTarget(clearDepth: false, clearColor: true, Color.white);
		}
		_MaskCommandBuffer.Blit(underwaterMask, _UnderwaterMask);
		Shader.SetGlobalTexture(underwaterMask, _UnderwaterMask);
		if (water != null && water.Renderer.MaskCount != 0 && _LocalWaterCamera.RenderVolumes)
		{
			_MaskCommandBuffer.Blit("_SubtractiveMask", underwaterMask, _ComposeUnderwaterMaskMaterial, 0);
		}
		CameraEvent evt = ((_LocalCamera.actualRenderingPath != RenderingPath.Forward) ? CameraEvent.BeforeLighting : ((!WaterProjectSettings.Instance.SinglePassStereoRendering) ? CameraEvent.AfterDepthTexture : CameraEvent.BeforeForwardOpaque));
		_LocalCamera.RemoveCommandBuffer(evt, _MaskCommandBuffer);
		_LocalCamera.AddCommandBuffer(evt, _MaskCommandBuffer);
	}

	private void RenderDepthScatter(Texture source, RenderTexture target)
	{
		Water water = (_HasWaterOverride ? _WaterOverride : _LocalWaterCamera.ContainingWater);
		_ImeMaterial.CopyPropertiesFromMaterial(water.Materials.SurfaceMaterial);
		_ImeMaterial.SetTexture("_UnderwaterAbsorptionGradient", water.Materials.UnderwaterAbsorptionColorByDepth);
		_ImeMaterial.SetFloat("_UnderwaterLightFadeScale", water.Materials.UnderwaterLightFadeScale);
		_ImeMaterial.SetMatrix("UNITY_MATRIX_VP_INVERSE", Matrix4x4.Inverse(_LocalCamera.projectionMatrix * _LocalCamera.worldToCameraMatrix));
		MaterialPropertyBlock propertyBlock = water.Renderer.PropertyBlock;
		GraphicsUtilities.Blit(source, target, _ImeMaterial, 1, propertyBlock);
	}

	private void RenderDistortions(Texture source, RenderTexture target)
	{
		Water water = (_HasWaterOverride ? _WaterOverride : _LocalWaterCamera.ContainingWater);
		float underwaterDistortionsIntensity = water.Materials.UnderwaterDistortionsIntensity;
		if (underwaterDistortionsIntensity > 0f)
		{
			int width = Camera.current.pixelWidth >> 2;
			int height = Camera.current.pixelHeight >> 2;
			TemporaryRenderTexture temporary = RenderTexturesCache.GetTemporary(width, height, 0, RenderTextureFormat.ARGB32, linear: true, uav: false);
			RenderDistortionMap(temporary);
			temporary.Texture.filterMode = FilterMode.Bilinear;
			_ImeMaterial.SetTexture("_DistortionTex", (RenderTexture)temporary);
			_ImeMaterial.SetFloat("_DistortionIntensity", underwaterDistortionsIntensity);
			GraphicsUtilities.Blit(source, target, _ImeMaterial, 2, water.Renderer.PropertyBlock);
			temporary.Dispose();
		}
		else
		{
			Graphics.Blit(source, target);
		}
	}

	private void RenderDistortionMap(RenderTexture target)
	{
		Water water = (_HasWaterOverride ? _WaterOverride : _LocalWaterCamera.ContainingWater);
		_NoiseMaterial.SetVector("_Offset", new Vector4(0f, 0f, Time.time * water.Materials.UnderwaterDistortionAnimationSpeed, 0f));
		_NoiseMaterial.SetVector("_Period", new Vector4(4f, 4f, 4f, 4f));
		Graphics.Blit(null, target, _NoiseMaterial, 3);
	}

	private void OnSubmersionStateChanged(WaterCamera waterCamera)
	{
		if (waterCamera.SubmersionState != SubmersionState.None || _HasWaterOverride)
		{
			if (_MaskCommandBuffer == null)
			{
				_MaskCommandBuffer = new CommandBuffer
				{
					name = "[UWS] UnderwaterIME - Render Underwater Mask"
				};
			}
		}
		else if (_MaskCommandBuffer != null)
		{
			Camera component = GetComponent<Camera>();
			component.RemoveCommandBuffer((!WaterProjectSettings.Instance.SinglePassStereoRendering) ? CameraEvent.AfterDepthTexture : CameraEvent.BeforeForwardOpaque, _MaskCommandBuffer);
			component.RemoveCommandBuffer(CameraEvent.AfterLighting, _MaskCommandBuffer);
		}
	}

	private void SetEffectsIntensity(float intensity)
	{
		if (_LocalCamera == null)
		{
			return;
		}
		intensity = Mathf.Clamp01(intensity);
		if (_Intensity != intensity)
		{
			_Intensity = intensity;
			if (!(_ReverbFilter == null) && _UnderwaterAudio)
			{
				float num = ((intensity > 0.05f) ? Mathf.Clamp01(intensity + 0.7f) : intensity);
				_ReverbFilter.dryLevel = -2000f * num;
				_ReverbFilter.room = -10000f * (1f - num);
				_ReverbFilter.roomHF = Mathf.Lerp(-10000f, -4000f, num);
				_ReverbFilter.decayTime = 1.6f * num;
				_ReverbFilter.decayHFRatio = 0.1f * num;
				_ReverbFilter.reflectionsLevel = -449f * num;
				_ReverbFilter.reverbLevel = 1500f * num;
				_ReverbFilter.reverbDelay = 0.0259f * num;
			}
		}
	}
}
