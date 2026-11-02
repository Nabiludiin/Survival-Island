using System.Collections.Generic;
using UltimateWater.Internal;
using UltimateWater.Utils;
using UnityEngine;
using UnityEngine.Rendering;

namespace UltimateWater;

[RequireComponent(typeof(MeshFilter))]
[AddComponentMenu("Ultimate Water/Dynamic/Water Simulation Area")]
public class WaterSimulationArea : MonoBehaviour, ILocalDisplacementRenderer, IDynamicWaterEffects
{
	[SerializeField]
	private Water _Water;

	[SerializeField]
	private WaterRipplesProfile _Profile;

	[Header("Settings")]
	[Tooltip("How many simulation pixels per one unit are used")]
	[SerializeField]
	[Range(1f, 32f)]
	private int _PixelsPerUnit = 16;

	[Tooltip("What resolution is used for dynamic depth rendering")]
	[SerializeField]
	[Range(0.125f, 2f)]
	private float _DepthScale = 0.5f;

	[Tooltip("Does the water can be  stopped by Blocking objects")]
	[SerializeField]
	private bool _EnableStaticCalculations;

	[Tooltip("What resolution is used for static depth information")]
	[SerializeField]
	[Range(0.125f, 2f)]
	private float _StaticDepthScale = 1f;

	[Header("Edge fade")]
	[SerializeField]
	private bool _Fade;

	[SerializeField]
	private float _FadePower = 0.5f;

	private Camera _DepthCamera;

	private readonly RenderTexture[] _Buffers = new RenderTexture[3];

	private RenderTexture _PreviousDepth;

	private RenderTexture _Depth;

	private RenderTexture _StaticDepth;

	private Material _RipplesMaterialCache;

	private MeshFilter _MeshFilter;

	[SerializeField]
	[HideInInspector]
	private Material _DisplacementMaterial;

	private Vector3 _Position;

	private Vector3 _Scale;

	private Material _TranslateMaterial;

	private CommandBuffer _CommandBuffer;

	private static readonly float[] _Array = new float[2048];

	private const int _MaxArrayElements = 512;

	public WaterRipplesProfile Profile
	{
		get
		{
			return _Profile;
		}
		set
		{
			_Profile = value;
			UpdateShaderVariables();
		}
	}

	public Vector2 Resolution
	{
		get
		{
			Vector2 vector = Size * _PixelsPerUnit;
			return new Vector2((int)vector.x, (int)vector.y);
		}
	}

	public Vector2 Size
	{
		get
		{
			return new Vector2(transform.localScale.x * 10f, transform.localScale.z * 10f);
		}
		set
		{
			transform.localScale = new Vector3(value.x * 0.1f, 1f, value.y * 0.1f);
		}
	}

	public Vector2 DepthResolution => Resolution * _DepthScale;

	private Material _RipplesMaterial
	{
		get
		{
			if (_RipplesMaterialCache != null)
			{
				return _RipplesMaterialCache;
			}
			Shader shader = ShaderUtility.Instance.Get(ShaderList.Simulation);
			_RipplesMaterialCache = new Material(shader);
			return _RipplesMaterialCache;
		}
	}

	private int _Width => (int)Resolution.x;

	private int _Height => (int)Resolution.y;

	private int _DepthWidth => (int)(Resolution.x * _DepthScale);

	private int _DepthHeight => (int)(Resolution.y * _DepthScale);

	private int _StaticWidth => (int)(Resolution.x * _StaticDepthScale);

	private int _StaticHeight => (int)(Resolution.y * _StaticDepthScale);

	public Vector2 GetLocalPixelPosition(Vector3 globalPosition)
	{
		Vector2 result = new Vector2(transform.position.x, transform.position.z) - new Vector2(globalPosition.x, globalPosition.z) + Size * 0.5f;
		result.x *= _PixelsPerUnit;
		result.y *= _PixelsPerUnit;
		return result;
	}

	public void AddForce(List<WaterForce.Data> data, float radius = 1f)
	{
		Vector2 resolution = Resolution;
		int num = 0;
		for (int i = 0; i < data.Count; i++)
		{
			Vector2 localPixelPosition = GetLocalPixelPosition(data[i].Position);
			if (ContainsLocalRaw(localPixelPosition, resolution) && !(data[i].Force <= 0f))
			{
				_Array[num * 4] = localPixelPosition.x;
				_Array[num * 4 + 1] = localPixelPosition.y;
				_Array[num * 4 + 2] = data[i].Force * 500f;
				_Array[num * 4 + 3] = 0f;
				num++;
				if (num == 512)
				{
					DispatchAddForce(num);
					num = 0;
				}
			}
		}
		DispatchAddForce(num);
	}

	private void Awake()
	{
		_MeshFilter = GetComponent<MeshFilter>();
		_TranslateMaterial = new Material(ShaderUtility.Instance.Get(ShaderList.Translate));
		if (_Water == null)
		{
			_Water = Utilities.GetWaterReference();
		}
		if (_DisplacementMaterial == null)
		{
			_DisplacementMaterial = new Material(Resources.Load<Material>("Materials/Overlay (Displacements)"));
		}
	}

	private void OnEnable()
	{
		_Position = transform.position;
		_Scale = transform.localScale;
		_CommandBuffer = new CommandBuffer
		{
			name = "[Ultimate Water]: Water Simulation Area"
		};
		CreateDepthCamera();
		CreateTextures();
		UpdateShaderVariables();
		RenderStaticDepthTexture();
		DynamicWater.AddRenderer(this);
		WaterRipples.Register(this);
	}

	private void OnDisable()
	{
		WaterRipples.Unregister(this);
		DynamicWater.RemoveRenderer(this);
		ReleaseDepthCamera();
		ReleaseTextures();
		if (_CommandBuffer != null)
		{
			_CommandBuffer.Release();
			_CommandBuffer = null;
		}
	}

	private void OnDestroy()
	{
		OnDisable();
	}

	private void Update()
	{
		transform.position = new Vector3(transform.position.x, _Water.transform.position.y, transform.position.z);
		transform.rotation = Quaternion.identity;
		transform.localScale = new Vector3(_Scale.x, 1f, _Scale.z);
		RenderDepth();
	}

	private void OnValidate()
	{
		ShaderUtility.Instance.Use(ShaderList.Simulation);
		ShaderUtility.Instance.Use(ShaderList.Translate);
		ShaderUtility.Instance.Use(ShaderList.Velocity);
		ShaderUtility.Instance.Use(ShaderList.Depth);
	}

	private void Reset()
	{
		GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
		MeshFilter component = GetComponent<MeshFilter>();
		component.sharedMesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
		component.hideFlags = HideFlags.HideInInspector;
		Object.DestroyImmediate(gameObject);
	}

	internal void Simulate()
	{
		Shader.SetGlobalFloat("_WaterHeight", _Water.transform.position.y);
		if (transform.position.x != _Position.x || transform.position.z != _Position.z)
		{
			RenderStaticDepthTexture();
			MoveSimulation();
			_Position = transform.position;
		}
		if (_Buffers[0].Verify(clear: false) || _Buffers[1].Verify(clear: false))
		{
			_Buffers[0].Clear(Color.clear);
			_Buffers[1].Clear(Color.clear);
		}
		RipplesShader.SetPrimary(_Buffers[0], _RipplesMaterial);
		RipplesShader.SetSecondary(_Buffers[1], _RipplesMaterial);
		switch (WaterQualitySettings.Instance.Ripples.ShaderMode)
		{
		case WaterRipplesData.ShaderModes.ComputeShader:
			SimulateComputeShader();
			break;
		case WaterRipplesData.ShaderModes.PixelShader:
			SimulatePixelShader();
			break;
		}
	}

	internal void Smooth()
	{
		if (!(Profile.Sigma <= 0.1f))
		{
			TemporaryRenderTexture temporary = RenderTexturesCache.GetTemporary(_Width, _Height, 0, WaterQualitySettings.Instance.Ripples.SimulationFormat, linear: true, uav: true);
			TextureUtility.Verify(temporary);
			GaussianShader.VerticalInput = _Buffers[1];
			GaussianShader.VerticalOutput = temporary;
			GaussianShader.Dispatch(GaussianShader.KernelType.Vertical, _Width, _Height);
			GaussianShader.HorizontalInput = temporary;
			GaussianShader.HorizontalOutput = _Buffers[1];
			GaussianShader.Dispatch(GaussianShader.KernelType.Horizontal, _Width, _Height);
			temporary.Dispose();
		}
	}

	internal void Swap()
	{
		TextureUtility.Swap(ref _Buffers[0], ref _Buffers[1]);
	}

	internal void UpdateShaderVariables()
	{
		if (!(Profile == null) && !_DisplacementMaterial.IsNullReference(this))
		{
			float num = (float)_PixelsPerUnit / 32f;
			RipplesShader.SetPropagation(Profile.Propagation * num, _RipplesMaterial);
			RipplesShader.SetStaticDepth(DefaultTextures.Get(Color.clear), _RipplesMaterial);
			RipplesShader.SetDamping(Profile.Damping / 32f, _RipplesMaterial);
			RipplesShader.SetGain(Profile.Gain, _RipplesMaterial);
			RipplesShader.SetHeightGain(Profile.HeightGain, _RipplesMaterial);
			RipplesShader.SetHeightOffset(Profile.HeightOffset, _RipplesMaterial);
			float[] array = Math.GaussianTerms(Profile.Sigma);
			GaussianShader.Term0 = array[0];
			GaussianShader.Term1 = array[1];
			GaussianShader.Term2 = array[2];
			_DisplacementMaterial.SetFloat("_Amplitude", Profile.Amplitude * 0.05f);
			_DisplacementMaterial.SetFloat("_Spread", Profile.Spread);
			_DisplacementMaterial.SetFloat("_Multiplier", Profile.Multiplier);
			_DisplacementMaterial.SetFloat("_Fadeout", _Fade ? 1f : 0f);
			_DisplacementMaterial.SetFloat("_FadePower", _FadePower);
		}
	}

	private void DispatchAddForce(int count)
	{
		if (count > 0)
		{
			ComputeShader shader = SetupShader.Shader;
			shader.SetTexture(SetupShader.Multi, "Previous", _Buffers[0]);
			shader.SetTexture(SetupShader.Multi, "Current", _Buffers[1]);
			shader.SetFloats("data", _Array);
			shader.Dispatch(SetupShader.Multi, count, 1, 1);
		}
	}

	private void SimulateComputeShader()
	{
		RipplesShader.Size = Resolution;
		RipplesShader.Dispatch(_Width, _Height);
	}

	private void SimulatePixelShader()
	{
		RenderTexture renderTexture = _Buffers[1].CreateTemporary();
		renderTexture.Clear(Color.clear);
		Graphics.Blit(null, renderTexture, _RipplesMaterial);
		Graphics.Blit(renderTexture, _Buffers[1]);
		renderTexture.ReleaseTemporary();
	}

	private void CreateDepthCamera()
	{
		if (Resolution.x <= 0f || Resolution.y <= 0f)
		{
			Debug.LogError("WaterSimulationArea: invalid resolution");
		}
		GameObject gameObject = new GameObject("Depth");
		gameObject.transform.SetParent(transform);
		_DepthCamera = gameObject.AddComponent<Camera>();
		_DepthCamera.enabled = false;
		_DepthCamera.backgroundColor = Color.clear;
		_DepthCamera.clearFlags = CameraClearFlags.Color;
		_DepthCamera.orthographic = true;
		_DepthCamera.orthographicSize = Size.y * 0.5f;
		_DepthCamera.aspect = Resolution.x / Resolution.y;
		_DepthCamera.transform.localRotation = Quaternion.Euler(new Vector3(90f, 180f, 0f));
		_DepthCamera.transform.localScale = Vector3.one;
		_DepthCamera.depthTextureMode = DepthTextureMode.Depth;
		SetDepthCameraParameters(ref _DepthCamera);
	}

	private void ReleaseDepthCamera()
	{
		if (_DepthCamera != null)
		{
			Object.Destroy(_DepthCamera.gameObject);
		}
		_DepthCamera = null;
	}

	private void RenderDepth()
	{
		if (!_DepthCamera.IsNullReference(this))
		{
			TextureUtility.Swap(ref _Depth, ref _PreviousDepth);
			GL.PushMatrix();
			GL.modelview = _DepthCamera.worldToCameraMatrix;
			GL.LoadProjectionMatrix(_DepthCamera.projectionMatrix);
			_CommandBuffer.Clear();
			_CommandBuffer.SetRenderTarget(_Depth);
			_CommandBuffer.ClearRenderTarget(clearDepth: true, clearColor: true, Color.clear);
			List<IWavesInteractive> interactions = DynamicWater.Interactions;
			for (int i = 0; i < interactions.Count; i++)
			{
				interactions[i].Render(_CommandBuffer);
			}
			Graphics.ExecuteCommandBuffer(_CommandBuffer);
			GL.PopMatrix();
			RipplesShader.SetDepth(_Depth, _RipplesMaterial);
			RipplesShader.SetPreviousDepth(_PreviousDepth, _RipplesMaterial);
		}
	}

	private void MoveSimulation()
	{
		Vector3 vector = _Position - transform.position;
		float x = vector.x * (float)_PixelsPerUnit;
		float y = vector.z * (float)_PixelsPerUnit;
		_TranslateMaterial.SetVector("_Offset", new Vector4(x, y, 0f, 0f));
		for (int i = 0; i < 2; i++)
		{
			Graphics.Blit(_Buffers[i], _Buffers[2], _TranslateMaterial);
			TextureUtility.Swap(ref _Buffers[i], ref _Buffers[2]);
		}
		SendSimulationMatrix(_Buffers[1]);
	}

	private static bool ContainsLocalRaw(Vector2 localPosition, Vector2 resolution)
	{
		if (localPosition.x >= 0f && localPosition.x < resolution.x && localPosition.y >= 0f)
		{
			return localPosition.y < resolution.y;
		}
		return false;
	}

	private void RenderStaticDepthTexture()
	{
		if (!_EnableStaticCalculations)
		{
			if (_StaticDepth != null)
			{
				_StaticDepth.Release();
				_StaticDepth = null;
			}
		}
		else if (!_DepthCamera.IsNullReference(this))
		{
			if (_StaticDepth == null)
			{
				TextureUtility.RenderTextureDesc renderTextureDesc = new TextureUtility.RenderTextureDesc("[UWS] - Static Depth");
				renderTextureDesc.Width = _StaticWidth;
				renderTextureDesc.Height = _StaticHeight;
				renderTextureDesc.Depth = 24;
				renderTextureDesc.Format = RenderTextureFormat.RFloat;
				renderTextureDesc.Filter = FilterMode.Point;
				TextureUtility.RenderTextureDesc desc = renderTextureDesc;
				_StaticDepth = desc.CreateRenderTexture();
				_StaticDepth.Clear(Color.clear);
			}
			float y = _DepthCamera.transform.localPosition.y;
			float nearClipPlane = _DepthCamera.nearClipPlane;
			float farClipPlane = _DepthCamera.farClipPlane;
			SetDepthCameraParameters(ref _DepthCamera, 40f, 0f, 80f);
			_DepthCamera.cullingMask = WaterQualitySettings.Instance.Ripples.StaticDepthMask;
			_DepthCamera.targetTexture = _StaticDepth;
			_DepthCamera.SetReplacementShader(ShaderUtility.Instance.Get(ShaderList.Depth), "");
			_DepthCamera.Render();
			SetDepthCameraParameters(ref _DepthCamera, y, nearClipPlane, farClipPlane);
			RipplesShader.SetStaticDepth(_StaticDepth, _RipplesMaterial);
		}
	}

	private void CreateTextures()
	{
		ReleaseTextures();
		TextureUtility.RenderTextureDesc renderTextureDesc = new TextureUtility.RenderTextureDesc("[UWS] WaterSimulationArea - Simulation");
		renderTextureDesc.Width = _Width;
		renderTextureDesc.Height = _Height;
		renderTextureDesc.Format = WaterQualitySettings.Instance.Ripples.SimulationFormat;
		renderTextureDesc.Filter = FilterMode.Bilinear;
		renderTextureDesc.EnableRandomWrite = true;
		TextureUtility.RenderTextureDesc desc = renderTextureDesc;
		for (int i = 0; i < 3; i++)
		{
			_Buffers[i] = desc.CreateRenderTexture();
			_Buffers[i].name = "[UWS] WaterSimulationArea - Buffer[" + i + "]";
			_Buffers[i].Clear(Color.clear);
		}
		SendSimulationMatrix(_Buffers[1]);
		renderTextureDesc = new TextureUtility.RenderTextureDesc("[UWS] WaterSimulationArea - Depth");
		renderTextureDesc.Width = _DepthWidth;
		renderTextureDesc.Height = _DepthHeight;
		renderTextureDesc.Depth = 24;
		renderTextureDesc.Format = RenderTextureFormat.ARGBHalf;
		renderTextureDesc.Filter = FilterMode.Point;
		TextureUtility.RenderTextureDesc desc2 = renderTextureDesc;
		_Depth = desc2.CreateRenderTexture();
		_PreviousDepth = desc2.CreateRenderTexture();
		_Depth.Clear(Color.clear);
		_PreviousDepth.Clear(Color.clear);
	}

	private void ReleaseTextures()
	{
		TextureUtility.Release(ref _Depth);
		TextureUtility.Release(ref _PreviousDepth);
		TextureUtility.Release(ref _StaticDepth);
		TextureUtility.Release(ref _Buffers[0]);
		TextureUtility.Release(ref _Buffers[1]);
		TextureUtility.Release(ref _Buffers[2]);
	}

	private void SendSimulationMatrix(Texture texture)
	{
		_DisplacementMaterial.SetTexture("_SimulationMatrix", texture);
	}

	private static void SetDepthCameraParameters(ref Camera camera, float height = 10f, float near = 0f, float far = 20f)
	{
		camera.nearClipPlane = near;
		camera.farClipPlane = far;
		camera.transform.localPosition = new Vector3(0f, height, 0f);
	}

	public void RenderLocalDisplacement(CommandBuffer commandBuffer, DynamicWaterCameraData overlays)
	{
		commandBuffer.DrawMesh(_MeshFilter.sharedMesh, transform.localToWorldMatrix, _DisplacementMaterial);
	}

	public void Enable()
	{
	}

	public void Disable()
	{
	}
}
