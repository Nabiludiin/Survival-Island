using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace UltimateWater.Internal;

public sealed class DynamicWaterInteraction : MonoBehaviour, ILocalFoamRenderer, IDynamicWaterEffects
{
	private struct VertexData
	{
		public Vector3 Position;

		public Vector3 Normal;
	}

	[FormerlySerializedAs("colliderInteractionShader")]
	[SerializeField]
	[HideInInspector]
	private ComputeShader _ColliderInteractionShader;

	[SerializeField]
	[FormerlySerializedAs("maskDisplayShader")]
	[HideInInspector]
	private Shader _MaskDisplayShader;

	[SerializeField]
	[FormerlySerializedAs("baseShader")]
	[HideInInspector]
	private Shader _BaseShader;

	[FormerlySerializedAs("foam")]
	[Header("Contact Foam")]
	[SerializeField]
	private bool _Foam = true;

	[FormerlySerializedAs("foamPatternTiling")]
	[SerializeField]
	private float _FoamPatternTiling = 1f;

	[SerializeField]
	[FormerlySerializedAs("foamRange")]
	private float _FoamRange = 1.6f;

	[SerializeField]
	[FormerlySerializedAs("uniformFoamAmount")]
	private float _UniformFoamAmount = 30.5f;

	[FormerlySerializedAs("noisyFoamAmount")]
	[SerializeField]
	private float _NoisyFoamAmount = 30.5f;

	[SerializeField]
	[Range(0f, 1f)]
	[FormerlySerializedAs("foamIntensity")]
	private float _FoamIntensity = 0.45f;

	[SerializeField]
	[FormerlySerializedAs("detectContactArea")]
	private bool _DetectContactArea;

	[SerializeField]
	[FormerlySerializedAs("foamOcclusionMapResolution")]
	private int _FoamOcclusionMapResolution = 128;

	private int _OcclusionMap2Hash;

	private int _OcclusionMapProjectionMatrixHash;

	[SerializeField]
	[FormerlySerializedAs("meshFilters")]
	private MeshFilter[] _MeshFilters;

	[SerializeField]
	[FormerlySerializedAs("waves")]
	[Header("Waves")]
	private bool _Waves = true;

	[SerializeField]
	[FormerlySerializedAs("water")]
	private WaveParticlesSystemGPU _Water;

	[FormerlySerializedAs("waveEmissionFrequency")]
	[Range(0f, 4f)]
	[SerializeField]
	private float _WaveEmissionFrequency = 1f;

	private Material _Material;

	private int _OcclusionMapHash;

	private ComputeBuffer _ColliderVerticesBuffer;

	private ComputeBuffer _ObjectToWorld;

	private static readonly Matrix4x4[] _MatrixTemp = new Matrix4x4[1];

	public void Enable()
	{
		throw new NotImplementedException();
	}

	public void Disable()
	{
		throw new NotImplementedException();
	}

	public void RenderLocalFoam(CommandBuffer commandBuffer, DynamicWaterCameraData overlays)
	{
		if (_DetectContactArea)
		{
			Bounds bounds = _MeshFilters[0].GetComponent<MeshRenderer>().bounds;
			for (int num = _MeshFilters.Length - 1; num > 0; num--)
			{
				bounds.Encapsulate(_MeshFilters[num].GetComponent<MeshRenderer>().bounds);
			}
			Vector3 center = bounds.center;
			Vector3 extents = bounds.extents;
			extents.x += _FoamRange;
			extents.y += _FoamRange;
			extents.z += _FoamRange;
			center.y -= extents.y + 1f;
			commandBuffer.GetTemporaryRT(_OcclusionMap2Hash, _FoamOcclusionMapResolution, _FoamOcclusionMapResolution, 0, FilterMode.Point, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
			commandBuffer.SetRenderTarget(_OcclusionMap2Hash);
			commandBuffer.ClearRenderTarget(clearDepth: false, clearColor: true, new Color(0f, 0f, 0f, 0f));
			Camera effectsCamera = overlays.Camera.EffectsCamera;
			effectsCamera.transform.position = center;
			effectsCamera.transform.rotation = Quaternion.LookRotation(Vector3.up, Vector3.forward);
			effectsCamera.orthographic = true;
			effectsCamera.orthographicSize = Mathf.Max(extents.x, extents.z);
			effectsCamera.nearClipPlane = 1f;
			effectsCamera.farClipPlane = extents.y * 2f + 10f;
			Matrix4x4 matrix4x = effectsCamera.projectionMatrix * effectsCamera.worldToCameraMatrix;
			commandBuffer.SetGlobalMatrix(_OcclusionMapProjectionMatrixHash, matrix4x);
			commandBuffer.SetViewProjectionMatrices(Matrix4x4.identity, matrix4x);
			for (int num2 = _MeshFilters.Length - 1; num2 >= 0; num2--)
			{
				commandBuffer.DrawMesh(_MeshFilters[num2].sharedMesh, _MeshFilters[num2].transform.localToWorldMatrix, _Material, 0, 1);
			}
			commandBuffer.GetTemporaryRT(_OcclusionMapHash, _FoamOcclusionMapResolution, _FoamOcclusionMapResolution, 0, FilterMode.Bilinear, RenderTextureFormat.R8, RenderTextureReadWrite.Linear);
			commandBuffer.Blit(_OcclusionMap2Hash, _OcclusionMapHash, _Material, 2);
			commandBuffer.ReleaseTemporaryRT(_OcclusionMap2Hash);
			commandBuffer.SetRenderTarget(overlays.FoamMap);
			Camera planeProjectorCamera = overlays.Camera.PlaneProjectorCamera;
			commandBuffer.SetViewProjectionMatrices(planeProjectorCamera.worldToCameraMatrix, planeProjectorCamera.projectionMatrix);
		}
		for (int num3 = _MeshFilters.Length - 1; num3 >= 0; num3--)
		{
			commandBuffer.DrawMesh(_MeshFilters[num3].sharedMesh, _MeshFilters[num3].transform.localToWorldMatrix, _Material, 0, (!_DetectContactArea) ? 3 : 0, overlays.DynamicWater.Water.Renderer.PropertyBlock);
		}
		if (_DetectContactArea)
		{
			commandBuffer.ReleaseTemporaryRT(_OcclusionMapHash);
		}
		if (_Waves && _WaveEmissionFrequency != 0f)
		{
			_MatrixTemp[0] = transform.localToWorldMatrix;
			_ObjectToWorld.SetData(_MatrixTemp);
			_ColliderInteractionShader.SetVector("_LocalMapsCoords", overlays.Camera.LocalMapsShaderCoords);
			_ColliderInteractionShader.SetTexture(0, "TotalDisplacementMap", overlays.GetTotalDisplacementMap());
			_ColliderInteractionShader.SetBuffer(0, "Vertices", _ColliderVerticesBuffer);
			_ColliderInteractionShader.SetBuffer(0, "Particles", _Water.ParticlesBuffer);
			_ColliderInteractionShader.SetBuffer(0, "ObjectToWorld", _ObjectToWorld);
			_ColliderInteractionShader.Dispatch(0, Mathf.CeilToInt((float)(_ColliderVerticesBuffer.count >> 1) / 256f), 1, 1);
		}
	}

	private void Start()
	{
		OnValidate();
		if (_MeshFilters == null || _MeshFilters.Length == 0)
		{
			_MeshFilters = GetComponentsInChildren<MeshFilter>(includeInactive: true);
		}
		_Material = new Material(_BaseShader)
		{
			hideFlags = HideFlags.DontSave
		};
		_OcclusionMapHash = ShaderVariables.OcclusionMap;
		_OcclusionMap2Hash = ShaderVariables.OcclusionMap2;
		_OcclusionMapProjectionMatrixHash = ShaderVariables.OcclusionMapProjection;
		OnValidate();
		if (_Waves && _WaveEmissionFrequency != 0f)
		{
			CreateComputeBuffers();
		}
	}

	private void OnEnable()
	{
		if (_Foam)
		{
			DynamicWater.AddRenderer(this);
		}
	}

	private void OnDisable()
	{
		DynamicWater.RemoveRenderer(this);
	}

	private void OnDestroy()
	{
		if (_ColliderVerticesBuffer != null)
		{
			_ColliderVerticesBuffer.Release();
			_ColliderVerticesBuffer = null;
		}
		if (_ObjectToWorld != null)
		{
			_ObjectToWorld.Release();
			_ObjectToWorld = null;
		}
	}

	private void OnValidate()
	{
		if (_MaskDisplayShader == null)
		{
			_MaskDisplayShader = Shader.Find("UltimateWater/Utility/ShorelineMaskRender");
		}
		if (_BaseShader == null)
		{
			_BaseShader = Shader.Find("UltimateWater/Utility/DynamicWaterInteraction");
		}
		if (_Material != null)
		{
			_Material.SetVector("_FoamIntensity", new Vector4(_UniformFoamAmount, _NoisyFoamAmount, _FoamIntensity, 0f));
			_Material.SetFloat("_FoamRange", _FoamRange);
			_Material.SetFloat("_FoamIntensityMaskTiling", _FoamPatternTiling);
		}
	}

	private void CreateComputeBuffers()
	{
		Mesh sharedMesh = GetComponent<MeshCollider>().sharedMesh;
		Vector3[] vertices = sharedMesh.vertices;
		Vector3[] normals = sharedMesh.normals;
		int[] indices = sharedMesh.GetIndices(0);
		_ColliderVerticesBuffer = new ComputeBuffer(indices.Length * 2, 24, ComputeBufferType.Default);
		_ObjectToWorld = new ComputeBuffer(1, 64, ComputeBufferType.Default);
		VertexData[] array = new VertexData[indices.Length * 2];
		int num = 0;
		for (int i = 0; i < indices.Length; i++)
		{
			int num2 = indices[i];
			int num3 = indices[(i % 3 == 0) ? (i + 2) : (i - 1)];
			array[num++] = new VertexData
			{
				Position = vertices[num3],
				Normal = normals[num3]
			};
			array[num++] = new VertexData
			{
				Position = vertices[num2],
				Normal = normals[num2]
			};
		}
		_ColliderVerticesBuffer.SetData(array);
	}
}
