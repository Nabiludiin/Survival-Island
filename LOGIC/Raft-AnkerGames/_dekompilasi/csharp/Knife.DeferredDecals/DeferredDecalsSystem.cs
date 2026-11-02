using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Knife.DeferredDecals;

[ExecuteInEditMode]
public class DeferredDecalsSystem : MonoBehaviour
{
	public enum TerrainDecalsType
	{
		None,
		OneTerrain,
		MultiTerrain
	}

	public struct DecalsGroup
	{
		public Material Material;

		public int SortingOrder;

		public bool Instancing;
	}

	public class FloatValue
	{
		public float Value;
	}

	private Dictionary<Camera, CommandBuffer> m_Cameras = new Dictionary<Camera, CommandBuffer>();

	private Dictionary<Camera, CommandBuffer> m_CamerasSceneview = new Dictionary<Camera, CommandBuffer>();

	private Dictionary<Camera, RenderTexture> m_ExclusionMasks = new Dictionary<Camera, RenderTexture>();

	private CommandBuffer customRenderBuffer;

	private Plane[] cameraPlanes;

	public bool LockRebuild;

	public TerrainDecalsType TerrainDecals;

	public int TerrainHeightMapSize = 1024;

	public bool UseExclusionMask;

	public LayerMask ExclusionMask;

	public bool FrustumCulling = true;

	public bool DistanceCulling = true;

	public float StartFadeDistance = 50f;

	public float FadeLength = 2f;

	public static bool DrawDecalGizmos = true;

	[SerializeField]
	private Mesh cubeMesh;

	private Mesh cubeMeshCopy;

	public Texture2D[] terrainsTextures;

	private TerrainData[] terrainsData;

	private ComputeBuffer terrainsDataBuffer;

	private HeightmapsCollector heightmapsCollector;

	private CommandBuffer copyHeightmapsBuffer;

	private Dictionary<Decal, FloatValue> lastSelectedDecals = new Dictionary<Decal, FloatValue>();

	private const float selectionDuration = 1f;

	private readonly AnimationCurve selectionFadeCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

	private List<Decal> decalsToRemove = new List<Decal>();

	private double lastRenderTime;

	[SerializeField]
	private Shader specularSmoothnessBlitterShader;

	private Material specularSmoothnessBlitterMaterial;

	private static Material defaultDecalMaterial;

	private MaterialPropertyBlock instancedBlock;

	private Dictionary<DecalsGroup, List<Decal>> decalsGrouped = new Dictionary<DecalsGroup, List<Decal>>();

	private Matrix4x4[] matrices = new Matrix4x4[1023];

	private Vector4[] tintArray = new Vector4[1023];

	private Vector4[] uvArray = new Vector4[1023];

	private readonly string cmdCustomRenderBufferName = "[Decal Master] Custom Render Buffer";

	private Shader depthShader;

	private RenderTexture exclusionMaskRenderTarget;

	private Camera exclusionCamera;

	public Mesh CubeMesh
	{
		get
		{
			if (cubeMeshCopy == null)
			{
				CreateCubeMeshCopy();
			}
			return cubeMeshCopy;
		}
	}

	public static Material DefaultDecalMaterial
	{
		get
		{
			if (defaultDecalMaterial == null)
			{
				defaultDecalMaterial = Resources.Load<Material>("Knife/Deferred Decals/DefaultDecalMaterial");
			}
			return defaultDecalMaterial;
		}
	}

	public CommandBuffer CustomRenderBuffer
	{
		get
		{
			if (customRenderBuffer == null)
			{
				customRenderBuffer = new CommandBuffer();
				customRenderBuffer.name = cmdCustomRenderBufferName;
			}
			return customRenderBuffer;
		}
		set
		{
			customRenderBuffer = value;
		}
	}

	private void Start()
	{
		if (UnityEngine.Object.FindObjectsOfType<DeferredDecalsSystem>().Length > 1)
		{
			Debug.LogError("Must be only one deferred decals system");
			Disable();
		}
	}

	private void OnEnable()
	{
		Enable();
	}

	private void OnDisable()
	{
		Disable();
	}

	public void Enable()
	{
		specularSmoothnessBlitterShader = Resources.Load<Shader>("Knife/Deferred Decals/SpecSmoothnessBlitter");
		specularSmoothnessBlitterMaterial = new Material(specularSmoothnessBlitterShader);
		Camera.onPreRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPreRender, new Camera.CameraCallback(Render));
		instancedBlock = new MaterialPropertyBlock();
		depthShader = Resources.Load<Shader>("Knife/Deferred Decals/DepthToTarget");
		if (exclusionCamera != null)
		{
			UnityEngine.Object.DestroyImmediate(exclusionCamera.gameObject);
		}
		exclusionCamera = new GameObject("Exclusion camera", typeof(Camera)).GetComponent<Camera>();
		exclusionCamera.gameObject.hideFlags = HideFlags.HideAndDontSave;
		exclusionCamera.enabled = false;
		exclusionCamera.AddCommandBuffer(CameraEvent.AfterEverything, CustomRenderBuffer);
		CustomRendererManager.instance.SetRenderBuffer(CustomRenderBuffer, new Material(depthShader));
	}

	private void CreateCubeMeshCopy()
	{
		cubeMeshCopy = UnityEngine.Object.Instantiate(cubeMesh);
		Vector3[] vertices = cubeMeshCopy.vertices;
		for (int i = 0; i < vertices.Length; i++)
		{
			vertices[i] += Vector3.up * -0.5f;
		}
		cubeMeshCopy.vertices = vertices;
		cubeMeshCopy.UploadMeshData(markNoLongerReadable: true);
	}

	public void Disable()
	{
		if (specularSmoothnessBlitterMaterial != null)
		{
			UnityEngine.Object.DestroyImmediate(specularSmoothnessBlitterMaterial, allowDestroyingAssets: true);
		}
		foreach (KeyValuePair<Camera, CommandBuffer> camera in m_Cameras)
		{
			if (!(camera.Key == null))
			{
				camera.Key.RemoveCommandBuffer(CameraEvent.BeforeReflections, camera.Value);
			}
		}
		foreach (KeyValuePair<Camera, CommandBuffer> item in m_CamerasSceneview)
		{
			if (!(item.Key == null))
			{
				item.Key.RemoveCommandBuffer(CameraEvent.AfterLighting, item.Value);
			}
		}
		foreach (KeyValuePair<Camera, RenderTexture> exclusionMask in m_ExclusionMasks)
		{
			UnityEngine.Object.DestroyImmediate(exclusionMask.Value, allowDestroyingAssets: true);
		}
		m_ExclusionMasks.Clear();
		m_CamerasSceneview.Clear();
		m_Cameras.Clear();
		Camera.onPreRender = (Camera.CameraCallback)Delegate.Remove(Camera.onPreRender, new Camera.CameraCallback(Render));
	}

	public void Render(Camera camera)
	{
		if (Application.isPlaying && !camera.tag.Equals("MainCamera"))
		{
			return;
		}
		if (!camera)
		{
			return;
		}
		CommandBuffer commandBuffer = null;
		if (m_Cameras.ContainsKey(camera))
		{
			commandBuffer = m_Cameras[camera];
			if (LockRebuild)
			{
				return;
			}
			commandBuffer.Clear();
		}
		else
		{
			commandBuffer = new CommandBuffer();
			commandBuffer.name = "Deferred decals";
			m_Cameras[camera] = commandBuffer;
			CommandBuffer[] commandBuffers = camera.GetCommandBuffers(CameraEvent.BeforeReflections);
			for (int i = 0; i < commandBuffers.Length; i++)
			{
				if (commandBuffers[i].name.Equals("Deferred decals"))
				{
					camera.RemoveCommandBuffer(CameraEvent.BeforeReflections, commandBuffers[i]);
				}
			}
			camera.AddCommandBuffer(CameraEvent.BeforeReflections, commandBuffer);
		}
		if (UseExclusionMask)
		{
			if (!m_ExclusionMasks.TryGetValue(camera, out exclusionMaskRenderTarget))
			{
				exclusionMaskRenderTarget = new RenderTexture(camera.pixelWidth, camera.pixelHeight, 16, RenderTextureFormat.RFloat, RenderTextureReadWrite.Linear);
				exclusionMaskRenderTarget.filterMode = FilterMode.Point;
				exclusionMaskRenderTarget.useDynamicScale = true;
				exclusionMaskRenderTarget.Create();
				exclusionMaskRenderTarget.name = "Exclusion Mask for camera " + camera.name;
				m_ExclusionMasks.Add(camera, exclusionMaskRenderTarget);
			}
			if (exclusionMaskRenderTarget != null && (camera.pixelWidth != exclusionMaskRenderTarget.width || camera.pixelHeight != exclusionMaskRenderTarget.height))
			{
				m_ExclusionMasks.Remove(camera);
				UnityEngine.Object.DestroyImmediate(exclusionMaskRenderTarget, allowDestroyingAssets: true);
				exclusionMaskRenderTarget = new RenderTexture(camera.pixelWidth, camera.pixelHeight, 16, RenderTextureFormat.RFloat, RenderTextureReadWrite.Linear);
				exclusionMaskRenderTarget.filterMode = FilterMode.Point;
				exclusionMaskRenderTarget.useDynamicScale = true;
				exclusionMaskRenderTarget.Create();
				exclusionMaskRenderTarget.name = "Exclusion Mask for camera " + camera.name;
				m_ExclusionMasks.Add(camera, exclusionMaskRenderTarget);
			}
			exclusionCamera.CopyFrom(camera);
			exclusionCamera.depth = camera.depth - 0.01f;
			exclusionCamera.transform.position = camera.transform.position;
			exclusionCamera.transform.rotation = camera.transform.rotation;
			exclusionCamera.cullingMask = ExclusionMask;
			exclusionCamera.targetTexture = exclusionMaskRenderTarget;
			exclusionCamera.renderingPath = RenderingPath.Forward;
			exclusionCamera.clearFlags = CameraClearFlags.Color;
			exclusionCamera.backgroundColor = Color.clear;
			exclusionCamera.RenderWithShader(depthShader, "RenderType");
			commandBuffer.EnableShaderKeyword("EXCLUSIONMASK");
			commandBuffer.SetGlobalTexture("_ExclusionMask", exclusionMaskRenderTarget);
		}
		else
		{
			commandBuffer.DisableShaderKeyword("EXCLUSIONMASK");
		}
		bool flag = camera.cameraType == CameraType.SceneView;
		CommandBuffer commandBuffer2 = null;
		if (m_CamerasSceneview.ContainsKey(camera))
		{
			commandBuffer2 = m_CamerasSceneview[camera];
			commandBuffer2.Clear();
		}
		else if (flag)
		{
			commandBuffer2 = new CommandBuffer();
			commandBuffer2.name = "Deferred decals selection";
			m_CamerasSceneview[camera] = commandBuffer2;
			CommandBuffer[] commandBuffers2 = camera.GetCommandBuffers(CameraEvent.AfterLighting);
			for (int j = 0; j < commandBuffers2.Length; j++)
			{
				if (commandBuffers2[j].name.Equals("Deferred decals selection"))
				{
					camera.RemoveCommandBuffer(CameraEvent.AfterLighting, commandBuffers2[j]);
				}
			}
			camera.AddCommandBuffer(CameraEvent.AfterLighting, commandBuffer2);
		}
		cameraPlanes = GeometryUtility.CalculateFrustumPlanes(camera);
		DeferredDecalsManager instance = DeferredDecalsManager.instance;
		Vector3 position = camera.transform.position;
		float num = 0f;
		instancedBlock.Clear();
		decalsGrouped.Clear();
		if (FrustumCulling)
		{
			DecalsGroup key = default;
			foreach (Decal decal in instance.m_Decals)
			{
				if (decal == null)
				{
					continue;
				}
				if (DistanceCulling)
				{
					num = Vector3.Distance(position, decal.transform.position);
					if (num >= StartFadeDistance + FadeLength)
					{
						continue;
					}
				}
				SetupBounds(decal);
				if (IsBoundsVisible(cameraPlanes, decal.Bounds))
				{
					float num2 = Mathf.InverseLerp(StartFadeDistance, StartFadeDistance + FadeLength, num);
					decal.DistanceFade = 1f - num2;
					key.Material = decal.DecalMaterial;
					key.SortingOrder = decal.SortingOrder;
					key.Instancing = decal.DecalMaterial.enableInstancing;
					if (!decalsGrouped.TryGetValue(key, out var value))
					{
						value = new List<Decal>();
						decalsGrouped.Add(key, value);
					}
					value.Add(decal);
				}
			}
		}
		if (decalsGrouped.Count == 0)
		{
			return;
		}
		int num3 = Shader.PropertyToID("_NormalsCopy");
		commandBuffer.GetTemporaryRT(num3, -1, -1, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, 1, enableRandomWrite: false, RenderTextureMemoryless.None, useDynamicScale: false);
		commandBuffer.Blit(BuiltinRenderTextureType.GBuffer2, num3);
		int num4 = Shader.PropertyToID("_SpecularCopy");
		commandBuffer.GetTemporaryRT(num4, -1, -1, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, 1, enableRandomWrite: false, RenderTextureMemoryless.None, useDynamicScale: false);
		commandBuffer.Blit(BuiltinRenderTextureType.GBuffer1, num4);
		BuiltinRenderTextureType builtinRenderTextureType = (camera.allowHDR ? BuiltinRenderTextureType.CameraTarget : BuiltinRenderTextureType.GBuffer3);
		RenderTargetIdentifier[] colors = new RenderTargetIdentifier[4]
		{
			BuiltinRenderTextureType.GBuffer0,
			BuiltinRenderTextureType.GBuffer2,
			builtinRenderTextureType,
			BuiltinRenderTextureType.GBuffer1
		};
		commandBuffer.SetRenderTarget(colors, BuiltinRenderTextureType.CameraTarget);
		Vector4 value2 = new Vector4(1f, 1f, 0f, 0f);
		foreach (KeyValuePair<DecalsGroup, List<Decal>> item in decalsGrouped)
		{
			List<Decal> value3 = item.Value;
			if (item.Key.Instancing)
			{
				commandBuffer.SetGlobalColor("_NotInstancedColor", Color.white);
				commandBuffer.SetGlobalVector("_NotInstancedUV", value2);
				int num5 = 1 + Mathf.FloorToInt(value3.Count / 1024);
				for (int k = 0; k < num5; k++)
				{
					int num6 = Mathf.Min(1023, value3.Count);
					for (int l = 0; l < num6; l++)
					{
						tintArray[l] = value3[l].InstancedColor;
						tintArray[l].w *= value3[l].Fade * value3[l].DistanceFade;
						uvArray[l] = value3[l].UV;
						matrices[l] = value3[l].transform.localToWorldMatrix;
					}
					if (num6 > 0)
					{
						instancedBlock.SetVectorArray("_Tint", tintArray);
						instancedBlock.SetVectorArray("_UV", uvArray);
						commandBuffer.DrawMeshInstanced(CubeMesh, 0, item.Key.Material, 3, matrices, num6, instancedBlock);
					}
				}
				continue;
			}
			foreach (Decal item2 in value3)
			{
				Color instancedColor = item2.InstancedColor;
				instancedColor.a *= item2.Fade * item2.DistanceFade;
				commandBuffer.SetGlobalColor("_NotInstancedColor", instancedColor);
				Vector4 uV = item2.UV;
				commandBuffer.SetGlobalVector("_NotInstancedUV", uV);
				commandBuffer.DrawMesh(CubeMesh, item2.transform.localToWorldMatrix, item2.DecalMaterial, 0, 3);
			}
		}
		if (TerrainDecals != TerrainDecalsType.None)
		{
			if (TerrainDecals == TerrainDecalsType.OneTerrain)
			{
				Terrain activeTerrain = Terrain.activeTerrain;
				activeTerrain.drawHeightmap = true;
				Shader.SetGlobalMatrix("_World2Terrain", activeTerrain.transform.worldToLocalMatrix);
				Shader.SetGlobalTexture("_TerrainHeightMap", activeTerrain.terrainData.heightmapTexture);
				Shader.SetGlobalVector("_TerrainSize", activeTerrain.terrainData.size);
			}
			else if (TerrainDecals == TerrainDecalsType.MultiTerrain)
			{
				_ = Terrain.activeTerrains;
				if (copyHeightmapsBuffer == null)
				{
					CopyHeightmaps();
				}
				terrainsDataBuffer.SetData(terrainsData);
				SetupToShaders();
			}
		}
		commandBuffer.ReleaseTemporaryRT(num3);
		commandBuffer.ReleaseTemporaryRT(num4);
		commandBuffer2?.ReleaseTemporaryRT(num3);
	}

	[ContextMenu("Copy terrains heightmaps")]
	public void CopyHeightmaps()
	{
		Terrain[] activeTerrains = Terrain.activeTerrains;
		if (terrainsTextures != null)
		{
			DestroyTextures();
		}
		if (terrainsTextures == null)
		{
			terrainsTextures = new Texture2D[activeTerrains.Length];
		}
		for (int i = 0; i < activeTerrains.Length; i++)
		{
			activeTerrains[i].drawHeightmap = true;
			if (terrainsTextures[i] == null)
			{
				terrainsTextures[i] = new Texture2D(TerrainHeightMapSize, TerrainHeightMapSize, TextureFormat.RFloat, mipChain: false);
			}
		}
		if (terrainsData == null || terrainsData.Length != activeTerrains.Length)
		{
			terrainsData = new TerrainData[activeTerrains.Length];
		}
		if (terrainsDataBuffer != null && terrainsDataBuffer.count != activeTerrains.Length)
		{
			terrainsDataBuffer.Dispose();
			terrainsDataBuffer = null;
		}
		if (terrainsDataBuffer == null)
		{
			terrainsDataBuffer = new ComputeBuffer(activeTerrains.Length, TerrainData.Size);
		}
		if (heightmapsCollector != null && heightmapsCollector.RequiredSizeX != TerrainHeightMapSize)
		{
			heightmapsCollector.Resize(TerrainHeightMapSize, TerrainHeightMapSize);
		}
		else if (heightmapsCollector == null)
		{
			heightmapsCollector = new HeightmapsCollector(TerrainHeightMapSize, TerrainHeightMapSize);
		}
		if (copyHeightmapsBuffer == null)
		{
			copyHeightmapsBuffer = new CommandBuffer();
		}
		copyHeightmapsBuffer.Clear();
		heightmapsCollector.ClearTexturesList();
		for (int j = 0; j < activeTerrains.Length; j++)
		{
			CopyTerrainHeightToTexture(activeTerrains[j], terrainsTextures[j]);
			terrainsData[j].terrainSize = activeTerrains[j].terrainData.size;
			terrainsData[j].heightmapIndex = j;
			terrainsData[j].world2Terrain = activeTerrains[j].transform.worldToLocalMatrix;
			heightmapsCollector.AddTexture(terrainsTextures[j]);
		}
		Graphics.ExecuteCommandBuffer(copyHeightmapsBuffer);
		heightmapsCollector.Generate();
		terrainsDataBuffer.SetData(terrainsData);
		SetupToShaders();
	}

	private void CopyTerrainHeightToTexture(Terrain terrain, Texture2D targetTexture)
	{
		float num = terrain.terrainData.size.x / (float)targetTexture.width;
		float num2 = terrain.terrainData.size.z / (float)targetTexture.height;
		for (int i = 0; i < targetTexture.width; i++)
		{
			for (int j = 0; j < targetTexture.height; j++)
			{
				Vector3 position = new Vector3((float)i * num, 0f, (float)j * num2);
				position = terrain.transform.TransformPoint(position);
				float r = terrain.SampleHeight(position);
				targetTexture.SetPixel(i, j, new Color(r, 0f, 0f, 0f));
			}
		}
		targetTexture.Apply();
	}

	public void SetupToShaders()
	{
		Shader.SetGlobalBuffer("TerrainsDatas", terrainsDataBuffer);
		Shader.SetGlobalTexture("_TerrainsHeightMaps", heightmapsCollector.Texture);
		Shader.SetGlobalInt("TerrainsCount", terrainsDataBuffer.count);
	}

	private void DestroyTextures()
	{
		if (terrainsTextures != null)
		{
			for (int i = 0; i < terrainsTextures.Length; i++)
			{
				UnityEngine.Object.DestroyImmediate(terrainsTextures[i], allowDestroyingAssets: true);
			}
			terrainsTextures = null;
		}
	}

	public void SetupBounds(Decal decal)
	{
		if (!(decal == null))
		{
			decal.SetupBounds();
		}
	}

	public bool IsBoundsVisible(Plane[] planes, Bounds bounds)
	{
		return GeometryUtility.TestPlanesAABB(planes, bounds);
	}
}
