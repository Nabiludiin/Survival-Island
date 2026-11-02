using System;
using System.Collections.Generic;
using UltimateWater.Internal;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace UltimateWater;

public sealed class StaticWaterInteraction : MonoBehaviour, IWaterShore, ILocalDisplacementMaskRenderer, IDynamicWaterEffects
{
	public enum UnderwaterAreasMode
	{
		Generate,
		UseExisting
	}

	[FormerlySerializedAs("staticWaterInteractions")]
	public static List<StaticWaterInteraction> StaticWaterInteractions = new List<StaticWaterInteraction>();

	[Tooltip("Specifies a distance from the shore over which a water gets one meter deeper (value of 50 means that water has a depth of 1m at a distance of 50m from the shore).")]
	[Range(0.001f, 80f)]
	[SerializeField]
	[FormerlySerializedAs("shoreSmoothness")]
	private float _ShoreSmoothness = 50f;

	[FormerlySerializedAs("hasBottomFaces")]
	[Tooltip("If set to true, geometry that floats above water is correctly ignored.\n\nUse for objects that are closed and have faces at the bottom like basic primitives and most custom meshes, but not terrain.")]
	[SerializeField]
	private bool _HasBottomFaces;

	[SerializeField]
	[FormerlySerializedAs("underwaterAreasMode")]
	private UnderwaterAreasMode _UnderwaterAreasMode;

	[Resolution(1024, new int[] { 128, 256, 512, 1024, 2048 })]
	[SerializeField]
	[FormerlySerializedAs("mapResolution")]
	private int _MapResolution = 1024;

	[Tooltip("All waves bigger than this (in scene units) will be dampened near the shore.")]
	[SerializeField]
	[FormerlySerializedAs("waveDampingThreshold")]
	private float _WaveDampingThreshold = 4f;

	[SerializeField]
	[FormerlySerializedAs("depthScale")]
	private float _DepthScale = 1f;

	[HideInInspector]
	[SerializeField]
	[FormerlySerializedAs("maskGenerateShader")]
	private Shader _MaskGenerateShader;

	[HideInInspector]
	[SerializeField]
	[FormerlySerializedAs("maskDisplayShader")]
	private Shader _MaskDisplayShader;

	[SerializeField]
	[FormerlySerializedAs("heightMapperShader")]
	[HideInInspector]
	private Shader _HeightMapperShader;

	[HideInInspector]
	[SerializeField]
	[FormerlySerializedAs("heightMapperShaderAlt")]
	private Shader _HeightMapperShaderAlt;

	private GameObject[] _GameObjects;

	private Terrain[] _Terrains;

	private int[] _OriginalRendererLayers;

	private float[] _OriginalTerrainPixelErrors;

	private bool[] _OriginalDrawTrees;

	private RenderTexture _IntensityMask;

	private MeshRenderer _InteractionMaskRenderer;

	private Material _InteractionMaskMaterial;

	private Bounds _Bounds;

	private Bounds _TotalBounds;

	private float[] _HeightMapData;

	private float _OffsetX;

	private float _OffsetZ;

	private float _ScaleX;

	private float _ScaleZ;

	private int _Width;

	private int _Height;

	public Bounds Bounds => _TotalBounds;

	public RenderTexture IntensityMask
	{
		get
		{
			if (_IntensityMask == null)
			{
				_IntensityMask = new RenderTexture(_MapResolution, _MapResolution, 0, RenderTextureFormat.RFloat, RenderTextureReadWrite.Linear)
				{
					name = "[UWS] StaticWaterInteraction - IntensityMask",
					hideFlags = HideFlags.DontSave,
					filterMode = FilterMode.Bilinear
				};
			}
			return _IntensityMask;
		}
	}

	public Renderer InteractionRenderer => _InteractionMaskRenderer;

	[ContextMenu("Refresh Intensity Mask (Runtime Only)")]
	public void Refresh()
	{
		if (!(_InteractionMaskRenderer == null))
		{
			RenderShorelineIntensityMask();
			Vector3 localScale = _TotalBounds.size * 0.5f;
			localScale.x /= transform.localScale.x;
			localScale.y /= transform.localScale.y;
			localScale.z /= transform.localScale.z;
			_InteractionMaskRenderer.gameObject.transform.localScale = localScale;
		}
	}

	public void SetUniformDepth(float depth, float boundsSize)
	{
		OnValidate();
		OnDestroy();
		_TotalBounds = new Bounds(transform.position + new Vector3(boundsSize * 0.5f, 0f, boundsSize * 0.5f), new Vector3(boundsSize, 1f, boundsSize));
		RenderTexture intensityMask = IntensityMask;
		float num = Mathf.Sqrt((float)Math.Tanh((double)depth * -0.01));
		Graphics.SetRenderTarget(intensityMask);
		GL.Clear(clearDepth: true, clearColor: true, new Color(num, num, num, num));
		Graphics.SetRenderTarget(null);
		if (_InteractionMaskRenderer == null)
		{
			CreateMaskRenderer();
		}
	}

	public float GetDepthAt(float x, float z)
	{
		x = (x + _OffsetX) * _ScaleX;
		z = (z + _OffsetZ) * _ScaleZ;
		int num = (int)x;
		if ((float)num > x)
		{
			num--;
		}
		int num2 = (int)z;
		if ((float)num2 > z)
		{
			num2--;
		}
		if (num >= _Width || num < 0 || num2 >= _Height || num2 < 0)
		{
			return 100f;
		}
		x -= (float)num;
		z -= (float)num2;
		int num3 = num2 * _Width + num;
		float num4 = _HeightMapData[num3] * (1f - x) + _HeightMapData[num3 + 1] * x;
		float num5 = _HeightMapData[num3 + _Width] * (1f - x) + _HeightMapData[num3 + _Width + 1] * x;
		return num4 * (1f - z) + num5 * z;
	}

	public static float GetTotalDepthAt(float x, float z)
	{
		float num = 100f;
		int count = StaticWaterInteractions.Count;
		for (int i = 0; i < count; i++)
		{
			float depthAt = StaticWaterInteractions[i].GetDepthAt(x, z);
			if (num > depthAt)
			{
				num = depthAt;
			}
		}
		return num;
	}

	public void RenderLocalMask(CommandBuffer commandBuffer, DynamicWaterCameraData overlays)
	{
		Vector3 position = _InteractionMaskRenderer.transform.position;
		position.y = overlays.DynamicWater.Water.transform.position.y;
		_InteractionMaskRenderer.transform.position = position;
		commandBuffer.DrawMesh(_InteractionMaskRenderer.GetComponent<MeshFilter>().sharedMesh, _InteractionMaskRenderer.transform.localToWorldMatrix, _InteractionMaskMaterial, 0, 0);
	}

	public void Enable()
	{
		throw new NotImplementedException();
	}

	public void Disable()
	{
		throw new NotImplementedException();
	}

	public static StaticWaterInteraction AttachTo(GameObject target, float shoreSmoothness, bool hasBottomFaces, UnderwaterAreasMode underwaterAreasMode, int mapResolution, float waveDampingThreshold = 4f, float depthScale = 1f)
	{
		StaticWaterInteraction staticWaterInteraction = target.AddComponent<StaticWaterInteraction>();
		staticWaterInteraction._ShoreSmoothness = shoreSmoothness;
		staticWaterInteraction._HasBottomFaces = hasBottomFaces;
		staticWaterInteraction._UnderwaterAreasMode = underwaterAreasMode;
		staticWaterInteraction._MapResolution = mapResolution;
		staticWaterInteraction._WaveDampingThreshold = waveDampingThreshold;
		staticWaterInteraction._DepthScale = depthScale;
		return staticWaterInteraction;
	}

	private void OnValidate()
	{
		if (_MaskGenerateShader == null)
		{
			_MaskGenerateShader = Shader.Find("UltimateWater/Utility/ShorelineMaskGenerate");
		}
		if (_MaskDisplayShader == null)
		{
			_MaskDisplayShader = Shader.Find("UltimateWater/Utility/ShorelineMaskRender");
		}
		if (_HeightMapperShader == null)
		{
			_HeightMapperShader = Shader.Find("UltimateWater/Utility/HeightMapper");
		}
		if (_HeightMapperShaderAlt == null)
		{
			_HeightMapperShaderAlt = Shader.Find("UltimateWater/Utility/HeightMapperAlt");
		}
		if (_InteractionMaskMaterial != null)
		{
			_InteractionMaskMaterial.SetFloat("_WaveDampingThreshold", _WaveDampingThreshold);
		}
		if (_InteractionMaskMaterial != null)
		{
			_InteractionMaskMaterial.SetFloat("_WaveDampingThreshold", _WaveDampingThreshold);
		}
	}

	private void OnEnable()
	{
		StaticWaterInteractions.Add(this);
		DynamicWater.AddRenderer((ILocalDisplacementMaskRenderer)this);
	}

	private void OnDisable()
	{
		DynamicWater.RemoveRenderer((ILocalDisplacementMaskRenderer)this);
		StaticWaterInteractions.Remove(this);
	}

	private void OnDestroy()
	{
		if (_IntensityMask != null)
		{
			_IntensityMask.Destroy();
			_IntensityMask = null;
		}
		if (_InteractionMaskMaterial != null)
		{
			_InteractionMaskMaterial.Destroy();
			_InteractionMaskMaterial = null;
		}
		if (_InteractionMaskRenderer != null)
		{
			_InteractionMaskRenderer.Destroy();
			_InteractionMaskRenderer = null;
		}
	}

	private void Start()
	{
		OnValidate();
		if (_IntensityMask == null)
		{
			RenderShorelineIntensityMask();
		}
		if (_InteractionMaskRenderer == null)
		{
			CreateMaskRenderer();
		}
	}

	private void RenderShorelineIntensityMask()
	{
		try
		{
			PrepareRenderers();
			float num = 1f / _ShoreSmoothness;
			_TotalBounds = _Bounds;
			if (_UnderwaterAreasMode == UnderwaterAreasMode.Generate)
			{
				float num2 = 80f / num;
				_TotalBounds.Expand(new Vector3(num2, 0f, num2));
			}
			float y = transform.position.y;
			RenderTexture renderTexture = RenderHeightMap(_MapResolution, _MapResolution);
			RenderTexture intensityMask = IntensityMask;
			_OffsetX = 0f - _TotalBounds.min.x;
			_OffsetZ = 0f - _TotalBounds.min.z;
			_ScaleX = (float)_MapResolution / _TotalBounds.size.x;
			_ScaleZ = (float)_MapResolution / _TotalBounds.size.z;
			_Width = _MapResolution;
			_Height = _MapResolution;
			RenderTexture temporary = RenderTexture.GetTemporary(_MapResolution, _MapResolution, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
			RenderTexture temporary2 = RenderTexture.GetTemporary(_MapResolution, _MapResolution, 0, RenderTextureFormat.RFloat, RenderTextureReadWrite.Linear);
			Material material = new Material(_MaskGenerateShader);
			material.SetVector("_ShorelineExtendRange", new Vector2(_TotalBounds.size.x / _Bounds.size.x - 1f, _TotalBounds.size.z / _Bounds.size.z - 1f));
			material.SetFloat("_TerrainMinPoint", y);
			material.SetFloat("_Steepness", Mathf.Max(_TotalBounds.size.x, _TotalBounds.size.z) * num);
			material.SetFloat("_Offset1", 1f / (float)_MapResolution);
			material.SetFloat("_Offset2", 1.4142135f / (float)_MapResolution);
			RenderTexture renderTexture2 = null;
			if (_UnderwaterAreasMode == UnderwaterAreasMode.Generate)
			{
				RenderTexture temporary3 = RenderTexture.GetTemporary(_MapResolution, _MapResolution, 0, RenderTextureFormat.RFloat, RenderTextureReadWrite.Linear);
				renderTexture2 = RenderTexture.GetTemporary(_MapResolution, _MapResolution, 0, RenderTextureFormat.RFloat, RenderTextureReadWrite.Linear);
				Graphics.Blit(renderTexture, temporary3, material, 2);
				ComputeDistanceMap(material, temporary3, renderTexture2);
				RenderTexture.ReleaseTemporary(temporary3);
				renderTexture2.filterMode = FilterMode.Bilinear;
				material.SetTexture("_DistanceMap", renderTexture2);
				material.SetFloat("_GenerateUnderwaterAreas", 1f);
			}
			else
			{
				material.SetFloat("_GenerateUnderwaterAreas", 0f);
			}
			material.SetFloat("_DepthScale", _DepthScale);
			Graphics.Blit(renderTexture, temporary, material, 0);
			RenderTexture.ReleaseTemporary(renderTexture);
			if (renderTexture2 != null)
			{
				RenderTexture.ReleaseTemporary(renderTexture2);
			}
			Graphics.Blit(temporary, temporary2);
			ReadBackHeightMap(temporary);
			Graphics.Blit(temporary, intensityMask, material, 1);
			RenderTexture.ReleaseTemporary(temporary2);
			RenderTexture.ReleaseTemporary(temporary);
			UnityEngine.Object.Destroy(material);
		}
		finally
		{
			RestoreRenderers();
		}
	}

	private void PrepareRenderers()
	{
		bool flag = false;
		_Bounds = default;
		List<GameObject> list = new List<GameObject>();
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>(includeInactive: false);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].name == "Shoreline Mask")
			{
				continue;
			}
			StaticWaterInteraction component = componentsInChildren[i].GetComponent<StaticWaterInteraction>();
			if (component == null || component == this)
			{
				list.Add(componentsInChildren[i].gameObject);
				if (flag)
				{
					_Bounds.Encapsulate(componentsInChildren[i].bounds);
					continue;
				}
				_Bounds = componentsInChildren[i].bounds;
				flag = true;
			}
		}
		_Terrains = GetComponentsInChildren<Terrain>(includeInactive: false);
		_OriginalTerrainPixelErrors = new float[_Terrains.Length];
		_OriginalDrawTrees = new bool[_Terrains.Length];
		for (int j = 0; j < _Terrains.Length; j++)
		{
			_OriginalTerrainPixelErrors[j] = _Terrains[j].heightmapPixelError;
			_OriginalDrawTrees[j] = _Terrains[j].drawTreesAndFoliage;
			StaticWaterInteraction component2 = _Terrains[j].GetComponent<StaticWaterInteraction>();
			if (component2 == null || component2 == this)
			{
				list.Add(_Terrains[j].gameObject);
				_Terrains[j].heightmapPixelError = 1f;
				_Terrains[j].drawTreesAndFoliage = false;
				if (flag)
				{
					_Bounds.Encapsulate(_Terrains[j].transform.position);
					_Bounds.Encapsulate(_Terrains[j].transform.position + _Terrains[j].terrainData.size);
				}
				else
				{
					_Bounds = new Bounds(_Terrains[j].transform.position + _Terrains[j].terrainData.size * 0.5f, _Terrains[j].terrainData.size);
					flag = true;
				}
			}
		}
		_GameObjects = list.ToArray();
		_OriginalRendererLayers = new int[_GameObjects.Length];
		for (int k = 0; k < _GameObjects.Length; k++)
		{
			_OriginalRendererLayers[k] = _GameObjects[k].layer;
			_GameObjects[k].layer = WaterProjectSettings.Instance.WaterTempLayer;
		}
	}

	private void RestoreRenderers()
	{
		if (_Terrains != null)
		{
			for (int i = 0; i < _Terrains.Length; i++)
			{
				_Terrains[i].heightmapPixelError = _OriginalTerrainPixelErrors[i];
				_Terrains[i].drawTreesAndFoliage = _OriginalDrawTrees[i];
			}
		}
		if (_GameObjects != null)
		{
			for (int num = _GameObjects.Length - 1; num >= 0; num--)
			{
				_GameObjects[num].layer = _OriginalRendererLayers[num];
			}
		}
	}

	private RenderTexture RenderHeightMap(int width, int height)
	{
		RenderTexture temporary = RenderTexture.GetTemporary(width, height, 32, RenderTextureFormat.RFloat, RenderTextureReadWrite.Linear);
		temporary.wrapMode = TextureWrapMode.Clamp;
		RenderTexture.active = temporary;
		GL.Clear(clearDepth: true, clearColor: true, new Color(-4000f, -4000f, -4000f, -4000f), 1000000f);
		RenderTexture.active = null;
		GameObject gameObject = new GameObject();
		Camera camera = gameObject.AddComponent<Camera>();
		camera.enabled = false;
		camera.clearFlags = CameraClearFlags.Nothing;
		camera.depthTextureMode = DepthTextureMode.None;
		camera.orthographic = true;
		camera.cullingMask = 1 << WaterProjectSettings.Instance.WaterTempLayer;
		camera.nearClipPlane = 0.95f;
		camera.farClipPlane = _Bounds.size.y + 2f;
		camera.orthographicSize = _Bounds.size.z * 0.5f;
		camera.aspect = _Bounds.size.x / _Bounds.size.z;
		Vector3 center = _Bounds.center;
		center.y = _Bounds.max.y + 1f;
		camera.transform.position = center;
		camera.transform.rotation = Quaternion.LookRotation(Vector3.down, Vector3.forward);
		camera.targetTexture = temporary;
		camera.RenderWithShader(_HasBottomFaces ? _HeightMapperShaderAlt : _HeightMapperShader, "RenderType");
		camera.targetTexture = null;
		UnityEngine.Object.Destroy(gameObject);
		return temporary;
	}

	private static void ComputeDistanceMap(Material material, RenderTexture sa, RenderTexture sb)
	{
		sa.filterMode = FilterMode.Point;
		sb.filterMode = FilterMode.Point;
		RenderTexture renderTexture = sa;
		RenderTexture renderTexture2 = sb;
		int num = (int)((float)Mathf.Max(sa.width, sa.height) * 0.7f);
		for (int i = 0; i < num; i++)
		{
			Graphics.Blit(renderTexture, renderTexture2, material, 3);
			RenderTexture renderTexture3 = renderTexture;
			renderTexture = renderTexture2;
			renderTexture2 = renderTexture3;
		}
		if (renderTexture != sb)
		{
			Graphics.Blit(renderTexture, sb, material, 3);
		}
	}

	private void ReadBackHeightMap(RenderTexture source)
	{
		int width = _IntensityMask.width;
		int height = _IntensityMask.height;
		_HeightMapData = new float[width * height + width + 1];
		RenderTexture.active = source;
		Texture2D texture2D = new Texture2D(_IntensityMask.width, _IntensityMask.height, TextureFormat.RGBAFloat, mipChain: false, linear: true);
		texture2D.ReadPixels(new Rect(0f, 0f, _IntensityMask.width, _IntensityMask.height), 0, 0);
		texture2D.Apply();
		RenderTexture.active = null;
		int num = 0;
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				float num2 = texture2D.GetPixel(j, i).r;
				if (num2 > 0f && num2 < 1f)
				{
					num2 = Mathf.Sqrt(num2);
				}
				_HeightMapData[num++] = num2;
			}
		}
		UnityEngine.Object.Destroy(texture2D);
	}

	private void CreateMaskRenderer()
	{
		GameObject gameObject = new GameObject("Shoreline Mask")
		{
			hideFlags = HideFlags.DontSave,
			layer = WaterProjectSettings.Instance.WaterTempLayer
		};
		gameObject.AddComponent<MeshFilter>().sharedMesh = Quads.BipolarXZ;
		_InteractionMaskMaterial = new Material(_MaskDisplayShader)
		{
			hideFlags = HideFlags.DontSave
		};
		_InteractionMaskMaterial.SetTexture("_MainTex", _IntensityMask);
		OnValidate();
		_InteractionMaskRenderer = gameObject.AddComponent<MeshRenderer>();
		_InteractionMaskRenderer.sharedMaterial = _InteractionMaskMaterial;
		_InteractionMaskRenderer.enabled = false;
		gameObject.transform.SetParent(transform);
		gameObject.transform.position = new Vector3(_TotalBounds.center.x, 0f, _TotalBounds.center.z);
		gameObject.transform.localRotation = Quaternion.identity;
		Vector3 localScale = _TotalBounds.size * 0.5f;
		localScale.x /= transform.localScale.x;
		localScale.y /= transform.localScale.y;
		localScale.z /= transform.localScale.z;
		gameObject.transform.localScale = localScale;
	}
}
