using System;
using System.Collections.Generic;
using UltimateWater.Internal;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace UltimateWater;

[RequireComponent(typeof(Water))]
[AddComponentMenu("Ultimate Water/Spray", 1)]
public sealed class Spray : MonoBehaviour, IOverlaysRenderer
{
	public struct Particle(Vector3 position, Vector3 velocity, float lifetime, float offset, float maxIntensity)
	{
		public Vector3 Position = position;

		public Vector3 Velocity = velocity;

		public Vector2 Lifetime = new Vector2(lifetime, lifetime);

		public float Offset = offset;

		public float MaxIntensity = maxIntensity;
	}

	[HideInInspector]
	[SerializeField]
	[FormerlySerializedAs("sprayTiledGeneratorShader")]
	private Shader _SprayTiledGeneratorShader;

	[HideInInspector]
	[SerializeField]
	[FormerlySerializedAs("sprayLocalGeneratorShader")]
	private Shader _SprayLocalGeneratorShader;

	[HideInInspector]
	[SerializeField]
	[FormerlySerializedAs("sprayToFoamShader")]
	private Shader _SprayToFoamShader;

	[HideInInspector]
	[SerializeField]
	[FormerlySerializedAs("sprayControllerShader")]
	private ComputeShader _SprayControllerShader;

	[SerializeField]
	[FormerlySerializedAs("sprayMaterial")]
	private Material _SprayMaterial;

	[Range(16f, 327675f)]
	[SerializeField]
	[FormerlySerializedAs("maxParticles")]
	private int _MaxParticles = 65535;

	[SerializeField]
	[FormerlySerializedAs("sprayToFoam")]
	private bool _SprayToFoam = true;

	private float _SpawnThreshold = 1f;

	private float _SpawnSkipRatio = 0.9f;

	private float _Scale = 1f;

	private Water _Water;

	private WindWaves _WindWaves;

	private DynamicWater _Overlays;

	private Material _SprayTiledGeneratorMaterial;

	private Material _SprayLocalGeneratorMaterial;

	private Material _SprayToFoamMaterial;

	private Transform _ProbeAnchor;

	private RenderTexture _BlankOutput;

	private Texture2D _BlankWhiteTex;

	private ComputeBuffer _ParticlesA;

	private ComputeBuffer _ParticlesB;

	private ComputeBuffer _ParticlesInfo;

	private ComputeBuffer _SpawnBuffer;

	private int _Resolution;

	private Mesh _Mesh;

	private bool _Supported;

	private bool _ResourcesReady;

	private Vector2 _LastSurfaceOffset;

	private readonly int[] _CountBuffer = new int[4];

	private float _SkipRatioPrecomp;

	private Particle[] _ParticlesToSpawn = new Particle[10];

	private int _NumParticlesToSpawn;

	private MaterialPropertyBlock[] _PropertyBlocks;

	public int MaxParticles => _MaxParticles;

	public int SpawnedParticles
	{
		get
		{
			if (_ParticlesA != null)
			{
				ComputeBuffer.CopyCount(_ParticlesA, _ParticlesInfo, 0);
				_ParticlesInfo.GetData(_CountBuffer);
				return _CountBuffer[0];
			}
			return 0;
		}
	}

	public ComputeBuffer ParticlesBuffer => _ParticlesA;

	public void SpawnCustomParticle(Particle particle)
	{
		if (enabled)
		{
			if (_ParticlesToSpawn.Length <= _NumParticlesToSpawn)
			{
				Array.Resize(ref _ParticlesToSpawn, _ParticlesToSpawn.Length << 1);
			}
			_ParticlesToSpawn[_NumParticlesToSpawn] = particle;
			_NumParticlesToSpawn++;
		}
	}

	public void SpawnCustomParticles(Particle[] particles, int numParticles)
	{
		if (!enabled)
		{
			return;
		}
		CheckResources();
		if (_SpawnBuffer == null || _SpawnBuffer.count < particles.Length)
		{
			if (_SpawnBuffer != null)
			{
				_SpawnBuffer.Release();
			}
			_SpawnBuffer = new ComputeBuffer(particles.Length, 40);
		}
		_SpawnBuffer.SetData(particles);
		_SprayControllerShader.SetFloat("particleCount", numParticles);
		_SprayControllerShader.SetBuffer(2, "SourceParticles", _SpawnBuffer);
		_SprayControllerShader.SetBuffer(2, "TargetParticles", _ParticlesA);
		_SprayControllerShader.Dispatch(2, 1, 1, 1);
	}

	public void RenderOverlays(DynamicWaterCameraData overlays)
	{
	}

	public void RenderFoam(DynamicWaterCameraData overlays)
	{
		if (enabled)
		{
			CheckResources();
			if (_SprayToFoam)
			{
				GenerateLocalFoam(overlays);
			}
		}
	}

	private void Start()
	{
		_Water = GetComponent<Water>();
		_WindWaves = _Water.WindWaves;
		_Overlays = _Water.DynamicWater;
		_WindWaves.ResolutionChanged.AddListener(OnResolutionChanged);
		_Supported = CheckSupport();
		_LastSurfaceOffset = _Water.SurfaceOffset;
		if (!_Supported)
		{
			enabled = false;
		}
	}

	private void OnEnable()
	{
		_Water = GetComponent<Water>();
		_Water.ProfilesManager.Changed.AddListener(OnProfilesChanged);
		OnProfilesChanged(_Water);
		Camera.onPreCull = (Camera.CameraCallback)Delegate.Remove(Camera.onPreCull, new Camera.CameraCallback(OnSomeCameraPreCull));
		Camera.onPreCull = (Camera.CameraCallback)Delegate.Combine(Camera.onPreCull, new Camera.CameraCallback(OnSomeCameraPreCull));
	}

	private void OnDisable()
	{
		if (_Water != null)
		{
			_Water.ProfilesManager.Changed.RemoveListener(OnProfilesChanged);
		}
		Camera.onPreCull = (Camera.CameraCallback)Delegate.Remove(Camera.onPreCull, new Camera.CameraCallback(OnSomeCameraPreCull));
		Dispose();
	}

	private void LateUpdate()
	{
		if (Time.frameCount >= 10)
		{
			if (!_ResourcesReady)
			{
				CheckResources();
			}
			SwapParticleBuffers();
			ClearParticles();
			UpdateParticles();
			if (Camera.main != null)
			{
				SpawnWindWavesParticlesTiled(Camera.main.transform);
			}
			if (_NumParticlesToSpawn != 0)
			{
				SpawnCustomParticles(_ParticlesToSpawn, _NumParticlesToSpawn);
				_NumParticlesToSpawn = 0;
			}
		}
	}

	private void OnValidate()
	{
		_MaxParticles = Mathf.RoundToInt((float)_MaxParticles / 65535f) * 65535;
		if (_SprayTiledGeneratorShader == null)
		{
			_SprayTiledGeneratorShader = Shader.Find("UltimateWater/Spray/Generator (Tiles)");
		}
		if (_SprayLocalGeneratorShader == null)
		{
			_SprayLocalGeneratorShader = Shader.Find("UltimateWater/Spray/Generator (Local)");
		}
		if (_SprayToFoamShader == null)
		{
			_SprayToFoamShader = Shader.Find("UltimateWater/Spray/Spray To Foam");
		}
		UpdatePrecomputedParams();
	}

	private void OnSomeCameraPreCull(Camera cameraComponent)
	{
		if (!_ResourcesReady)
		{
			return;
		}
		WaterCamera waterCamera = WaterCamera.GetWaterCamera(cameraComponent);
		if (waterCamera != null && waterCamera.Type == WaterCamera.CameraType.Normal)
		{
			_SprayMaterial.SetBuffer("_Particles", _ParticlesA);
			_SprayMaterial.SetVector("_CameraUp", cameraComponent.transform.up);
			_SprayMaterial.SetVector("_WrapSubsurfaceScatteringPack", _Water.Renderer.PropertyBlock.GetVector("_WrapSubsurfaceScatteringPack"));
			_SprayMaterial.SetFloat("_UniformWaterScale", _Water.UniformWaterScale);
			if (_ProbeAnchor == null)
			{
				GameObject gameObject = new GameObject("Spray Probe Anchor")
				{
					hideFlags = HideFlags.HideAndDontSave
				};
				_ProbeAnchor = gameObject.transform;
			}
			_ProbeAnchor.position = cameraComponent.transform.position;
			int num = _PropertyBlocks.Length;
			for (int i = 0; i < num; i++)
			{
				Graphics.DrawMesh(_Mesh, Matrix4x4.identity, _SprayMaterial, 0, cameraComponent, 0, _PropertyBlocks[i], ShadowCastingMode.Off, receiveShadows: false, _ProbeAnchor);
			}
		}
	}

	private void SpawnWindWavesParticlesTiled(Transform origin)
	{
		Vector3 position = origin.position;
		float num = 400f / (float)_BlankOutput.width;
		_SprayTiledGeneratorMaterial.CopyPropertiesFromMaterial(_Water.Materials.SurfaceMaterial);
		_SprayTiledGeneratorMaterial.SetVector("_SurfaceOffset", new Vector3(_Water.SurfaceOffset.x, _Water.transform.position.y, _Water.SurfaceOffset.y));
		_SprayTiledGeneratorMaterial.SetVector("_Params", new Vector4(_SpawnThreshold * 0.25835f, _SkipRatioPrecomp, 0f, _Scale * 0.455f));
		_SprayTiledGeneratorMaterial.SetVector("_Coordinates", new Vector4(position.x - 200f + UnityEngine.Random.value * num, position.z - 200f + UnityEngine.Random.value * num, 400f, 400f));
		if (_Overlays == null)
		{
			_SprayTiledGeneratorMaterial.SetTexture("_LocalNormalMap", GetBlankWhiteTex());
		}
		Graphics.SetRandomWriteTarget(1, _ParticlesA);
		GraphicsUtilities.Blit(null, _BlankOutput, _SprayTiledGeneratorMaterial, 0, _Water.Renderer.PropertyBlock);
		Graphics.ClearRandomWriteTargets();
	}

	private void GenerateLocalFoam(DynamicWaterCameraData data)
	{
		RenderTexture temporary = RenderTexture.GetTemporary(512, 512, 0, RenderTextureFormat.RHalf, RenderTextureReadWrite.Linear);
		Graphics.SetRenderTarget(temporary);
		GL.Clear(clearDepth: false, clearColor: true, new Color(0f, 0f, 0f, 0f));
		_SprayToFoamMaterial.SetBuffer("_Particles", _ParticlesA);
		_SprayToFoamMaterial.SetVector("_LocalMapsCoords", data.Camera.LocalMapsShaderCoords);
		_SprayToFoamMaterial.SetFloat("_UniformWaterScale", 50f * _Water.UniformWaterScale / data.Camera.LocalMapsRect.width);
		Vector4 vector = _SprayMaterial.GetVector("_ParticleParams");
		vector.x *= 8f;
		vector.z = 1f;
		_SprayToFoamMaterial.SetVector("_ParticleParams", vector);
		int num = _PropertyBlocks.Length;
		for (int i = 0; i < num; i++)
		{
			_SprayToFoamMaterial.SetFloat("_ParticleOffset", i * 65535);
			if (_SprayToFoamMaterial.SetPass(0))
			{
				Graphics.DrawMeshNow(_Mesh, Matrix4x4.identity, 0);
			}
		}
		Camera planeProjectorCamera = data.Camera.PlaneProjectorCamera;
		Rect localMapsRect = data.Camera.LocalMapsRect;
		Vector2 center = localMapsRect.center;
		float num2 = localMapsRect.width * 0.5f;
		Matrix4x4 matrix = new Matrix4x4
		{
			m03 = center.x,
			m13 = _Water.transform.position.y,
			m23 = center.y,
			m00 = num2,
			m11 = num2,
			m22 = num2,
			m33 = 1f
		};
		GL.PushMatrix();
		GL.modelview = planeProjectorCamera.worldToCameraMatrix;
		GL.LoadProjectionMatrix(planeProjectorCamera.projectionMatrix);
		Graphics.SetRenderTarget(data.FoamMap);
		_SprayToFoamMaterial.mainTexture = temporary;
		if (_SprayToFoamMaterial.SetPass(1))
		{
			Graphics.DrawMeshNow(Quads.BipolarXZ, matrix, 0);
		}
		GL.PopMatrix();
		RenderTexture.ReleaseTemporary(temporary);
	}

	private void UpdateParticles()
	{
		Vector2 vector = _WindWaves.WindSpeed * 0.0008f;
		Vector3 gravity = Physics.gravity;
		float deltaTime = Time.deltaTime;
		if (_Overlays != null)
		{
			DynamicWaterCameraData cameraOverlaysData = _Overlays.GetCameraOverlaysData(Camera.main, createIfNotExists: false);
			if (cameraOverlaysData != null)
			{
				_SprayControllerShader.SetTexture(0, "TotalDisplacementMap", cameraOverlaysData.TotalDisplacementMap);
				WaterCamera waterCamera = WaterCamera.GetWaterCamera(Camera.main);
				if (waterCamera != null)
				{
					_SprayControllerShader.SetVector("localMapsCoords", waterCamera.LocalMapsShaderCoords);
				}
			}
			else
			{
				_SprayControllerShader.SetTexture(0, "TotalDisplacementMap", GetBlankWhiteTex());
			}
		}
		else
		{
			_SprayControllerShader.SetTexture(0, "TotalDisplacementMap", GetBlankWhiteTex());
		}
		Vector2 surfaceOffset = _Water.SurfaceOffset;
		_SprayControllerShader.SetVector("deltaTime", new Vector4(deltaTime, 1f - deltaTime * 0.2f, 0f, 0f));
		_SprayControllerShader.SetVector("externalForces", new Vector3((vector.x + gravity.x) * deltaTime, gravity.y * deltaTime, (vector.y + gravity.z) * deltaTime));
		_SprayControllerShader.SetVector("surfaceOffsetDelta", new Vector3(_LastSurfaceOffset.x - surfaceOffset.x, 0f, _LastSurfaceOffset.y - surfaceOffset.y));
		_SprayControllerShader.SetFloat("surfaceOffsetY", transform.position.y);
		_SprayControllerShader.SetVector("waterTileSizesInv", _WindWaves.TileSizesInv);
		_SprayControllerShader.SetBuffer(0, "SourceParticles", _ParticlesB);
		_SprayControllerShader.SetBuffer(0, "TargetParticles", _ParticlesA);
		_SprayControllerShader.Dispatch(0, _MaxParticles / 256, 1, 1);
		_LastSurfaceOffset = surfaceOffset;
	}

	private Texture2D GetBlankWhiteTex()
	{
		if (_BlankWhiteTex == null)
		{
			_BlankWhiteTex = new Texture2D(2, 2, TextureFormat.ARGB32, mipChain: false, linear: true);
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					_BlankWhiteTex.SetPixel(i, j, new Color(1f, 1f, 1f, 1f));
				}
			}
			_BlankWhiteTex.Apply(updateMipmaps: false, makeNoLongerReadable: true);
		}
		return _BlankWhiteTex;
	}

	private void ClearParticles()
	{
		_SprayControllerShader.SetBuffer(1, "TargetParticlesFlat", _ParticlesA);
		_SprayControllerShader.Dispatch(1, _MaxParticles / 256, 1, 1);
	}

	private void SwapParticleBuffers()
	{
		ComputeBuffer particlesB = _ParticlesB;
		_ParticlesB = _ParticlesA;
		_ParticlesA = particlesB;
	}

	private void OnResolutionChanged(WindWaves windWaves)
	{
		if (_BlankOutput != null)
		{
			UnityEngine.Object.Destroy(_BlankOutput);
			_BlankOutput = null;
		}
		_ResourcesReady = false;
	}

	private void OnProfilesChanged(Water water)
	{
		List<Water.WeightedProfile> profiles = water.ProfilesManager.Profiles;
		_SpawnThreshold = 0f;
		_SpawnSkipRatio = 0f;
		_Scale = 0f;
		if (profiles != null)
		{
			for (int i = 0; i < profiles.Count; i++)
			{
				Water.WeightedProfile weightedProfile = profiles[i];
				WaterProfileData profile = weightedProfile.Profile;
				float weight = weightedProfile.Weight;
				_SpawnThreshold += profile.SprayThreshold * weight;
				_SpawnSkipRatio += profile.SpraySkipRatio * weight;
				_Scale += profile.SpraySize * weight;
			}
		}
	}

	private void UpdatePrecomputedParams()
	{
		if (_Water != null)
		{
			_Resolution = _WindWaves.FinalResolution;
		}
		_SkipRatioPrecomp = Mathf.Pow(_SpawnSkipRatio, 1024f / (float)_Resolution);
	}

	private bool CheckSupport()
	{
		if (SystemInfo.supportsComputeShaders && _SprayTiledGeneratorShader != null)
		{
			return _SprayTiledGeneratorShader.isSupported;
		}
		return false;
	}

	private void CheckResources()
	{
		if (_SprayTiledGeneratorMaterial == null)
		{
			_SprayTiledGeneratorMaterial = new Material(_SprayTiledGeneratorShader)
			{
				hideFlags = HideFlags.DontSave
			};
		}
		if (_SprayLocalGeneratorMaterial == null)
		{
			_SprayLocalGeneratorMaterial = new Material(_SprayLocalGeneratorShader)
			{
				hideFlags = HideFlags.DontSave
			};
		}
		if (_SprayToFoamMaterial == null)
		{
			_SprayToFoamMaterial = new Material(_SprayToFoamShader)
			{
				hideFlags = HideFlags.DontSave
			};
		}
		if (_BlankOutput == null)
		{
			UpdatePrecomputedParams();
			_BlankOutput = new RenderTexture(_Resolution, _Resolution, 0, SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.R8) ? RenderTextureFormat.R8 : RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear)
			{
				name = "[UWS] Spray - WaterSpray Blank Output Texture",
				filterMode = FilterMode.Point
			};
			_BlankOutput.Create();
		}
		if (_Mesh == null)
		{
			int num = Mathf.Min(_MaxParticles, 65535);
			_Mesh = new Mesh
			{
				name = "Spray",
				hideFlags = HideFlags.DontSave,
				vertices = new Vector3[num]
			};
			int[] array = new int[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = i;
			}
			_Mesh.SetIndices(array, MeshTopology.Points, 0);
			_Mesh.bounds = new Bounds(Vector3.zero, new Vector3(10000000f, 10000000f, 10000000f));
		}
		if (_PropertyBlocks == null)
		{
			int num2 = Mathf.CeilToInt((float)_MaxParticles / 65535f);
			_PropertyBlocks = new MaterialPropertyBlock[num2];
			for (int j = 0; j < num2; j++)
			{
				(_PropertyBlocks[j] = new MaterialPropertyBlock()).SetFloat("_ParticleOffset", j * 65535);
			}
		}
		if (_ParticlesA == null)
		{
			_ParticlesA = new ComputeBuffer(_MaxParticles, 40, ComputeBufferType.Append);
		}
		if (_ParticlesB == null)
		{
			_ParticlesB = new ComputeBuffer(_MaxParticles, 40, ComputeBufferType.Append);
		}
		if (_ParticlesInfo == null)
		{
			_ParticlesInfo = new ComputeBuffer(1, 16, ComputeBufferType.DrawIndirect);
			_ParticlesInfo.SetData(new int[4] { 0, 1, 0, 0 });
		}
		_ResourcesReady = true;
	}

	private void Dispose()
	{
		if (_BlankOutput != null)
		{
			UnityEngine.Object.Destroy(_BlankOutput);
			_BlankOutput = null;
		}
		if (_ParticlesA != null)
		{
			_ParticlesA.Dispose();
			_ParticlesA = null;
		}
		if (_ParticlesB != null)
		{
			_ParticlesB.Dispose();
			_ParticlesB = null;
		}
		if (_ParticlesInfo != null)
		{
			_ParticlesInfo.Release();
			_ParticlesInfo = null;
		}
		if (_Mesh != null)
		{
			UnityEngine.Object.Destroy(_Mesh);
			_Mesh = null;
		}
		if (_ProbeAnchor != null)
		{
			UnityEngine.Object.Destroy(_ProbeAnchor.gameObject);
			_ProbeAnchor = null;
		}
		if (_SpawnBuffer != null)
		{
			_SpawnBuffer.Release();
			_SpawnBuffer = null;
		}
		_ResourcesReady = false;
	}
}
