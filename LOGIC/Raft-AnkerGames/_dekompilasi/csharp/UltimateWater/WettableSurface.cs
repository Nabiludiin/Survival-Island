using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace UltimateWater;

public sealed class WettableSurface : MonoBehaviour
{
	public enum Mode
	{
		TextureSpace,
		NearCamera
	}

	[HideInInspector]
	[SerializeField]
	[FormerlySerializedAs("wettableUtilShader")]
	private Shader _WettableUtilShader;

	[HideInInspector]
	[SerializeField]
	[FormerlySerializedAs("wettableUtilNearShader")]
	private Shader _WettableUtilNearShader;

	[SerializeField]
	[FormerlySerializedAs("water")]
	private Water _Water;

	[Tooltip("Surface wetting near this camera will be more precise.")]
	[SerializeField]
	[FormerlySerializedAs("mainCamera")]
	private WaterCamera _MainCamera;

	[Tooltip("Texture space is good for small objects, especially convex ones.\nNear camera mode is better for terrains and big meshes that are static and don't have geometry at the bottom.")]
	[SerializeField]
	[FormerlySerializedAs("mode")]
	private Mode _Mode;

	[SerializeField]
	[FormerlySerializedAs("resolution")]
	private int _Resolution = 512;

	[Header("Direct references (Optional)")]
	[SerializeField]
	[FormerlySerializedAs("meshRenderers")]
	private MeshRenderer[] _MeshRenderers;

	[SerializeField]
	[FormerlySerializedAs("terrains")]
	private Terrain[] _Terrains;

	private MeshFilter[] _MeshFilters;

	private Material _WettableUtilMaterial;

	private RenderTexture _WetnessMapA;

	private RenderTexture _WetnessMapB;

	private Camera _WettingCamera;

	private Material[] _Materials;

	private bool[] _TerrainDrawTrees;

	private int[] _TerrainLayers;

	public WaterCamera MainCamera
	{
		get
		{
			return _MainCamera;
		}
		set
		{
			_MainCamera = value;
		}
	}

	private void LateUpdate()
	{
		if (!_MainCamera.enabled)
		{
			return;
		}
		switch (_Mode)
		{
		case Mode.TextureSpace:
		{
			Graphics.SetRenderTarget(_WetnessMapA);
			_WettableUtilMaterial.SetPass(0);
			_WettableUtilMaterial.SetTexture("_TotalDisplacementMap", _Water.DynamicWater.GetCameraOverlaysData(_MainCamera.GetComponent<Camera>()).GetTotalDisplacementMap());
			_WettableUtilMaterial.SetVector("_LocalMapsCoords", _MainCamera.LocalMapsShaderCoords);
			for (int m = 0; m < _MeshFilters.Length; m++)
			{
				Graphics.DrawMeshNow(_MeshFilters[m].sharedMesh, _MeshFilters[m].transform.localToWorldMatrix);
			}
			if (_Terrains.Length != 0)
			{
				int waterTempLayer2 = WaterProjectSettings.Instance.WaterTempLayer;
				for (int n = 0; n < _Terrains.Length; n++)
				{
					GameObject gameObject2 = _Terrains[n].gameObject;
					_TerrainLayers[n] = gameObject2.layer;
					gameObject2.layer = waterTempLayer2;
				}
				Shader.SetGlobalTexture("_TotalDisplacementMap", _Water.DynamicWater.GetCameraOverlaysData(_MainCamera.GetComponent<Camera>()).GetTotalDisplacementMap());
				Shader.SetGlobalVector("_LocalMapsCoords", _MainCamera.LocalMapsShaderCoords);
				Camera wettingCamera2 = GetWettingCamera();
				wettingCamera2.transform.position = _Terrains[0].transform.position + _Terrains[0].terrainData.size * 0.5f;
				wettingCamera2.RenderWithShader(_WettableUtilShader, "CustomType");
				for (int num = 0; num < _Terrains.Length; num++)
				{
					_Terrains[num].gameObject.layer = _TerrainLayers[num];
				}
			}
			break;
		}
		case Mode.NearCamera:
		{
			int waterTempLayer = WaterProjectSettings.Instance.WaterTempLayer;
			for (int i = 0; i < _Terrains.Length; i++)
			{
				_TerrainDrawTrees[i] = _Terrains[i].drawTreesAndFoliage;
				_Terrains[i].drawTreesAndFoliage = false;
				GameObject gameObject = _Terrains[i].gameObject;
				_TerrainLayers[i] = gameObject.layer;
				gameObject.layer = waterTempLayer;
			}
			Shader.SetGlobalTexture("_WetnessMapPrevious", _WetnessMapB);
			Shader.SetGlobalTexture("_TotalDisplacementMap", _Water.DynamicWater.GetCameraOverlaysData(_MainCamera.GetComponent<Camera>()).GetTotalDisplacementMap());
			Shader.SetGlobalVector("_LocalMapsCoords", _MainCamera.LocalMapsShaderCoords);
			Rect localMapsRectPrevious = _MainCamera.LocalMapsRectPrevious;
			Shader.SetGlobalVector("_LocalMapsCoordsPrevious", new Vector4((0f - localMapsRectPrevious.xMin) / localMapsRectPrevious.width, (0f - localMapsRectPrevious.yMin) / localMapsRectPrevious.width, 1f / localMapsRectPrevious.width, localMapsRectPrevious.width));
			Rect localMapsRect = _MainCamera.LocalMapsRect;
			Camera wettingCamera = GetWettingCamera();
			wettingCamera.transform.position = new Vector3(localMapsRect.center.x, _Terrains[0].transform.position.y + _Terrains[0].terrainData.size.y + 1f, localMapsRect.center.y);
			wettingCamera.orthographicSize = localMapsRect.width * 0.5f;
			wettingCamera.nearClipPlane = 1f;
			wettingCamera.farClipPlane = _Terrains[0].terrainData.size.y * 2f;
			wettingCamera.targetTexture = _WetnessMapA;
			wettingCamera.clearFlags = CameraClearFlags.Color;
			wettingCamera.backgroundColor = new Color(0f, 0f, 0f, 0f);
			wettingCamera.RenderWithShader(_WettableUtilNearShader, "CustomType");
			for (int j = 0; j < _Materials.Length; j++)
			{
				_Materials[j].SetTexture("_WetnessMap", _WetnessMapA);
			}
			for (int k = 0; k < _Terrains.Length; k++)
			{
				_Terrains[k].drawTreesAndFoliage = _TerrainDrawTrees[k];
				_Terrains[k].gameObject.layer = _TerrainLayers[k];
			}
			RenderTexture wetnessMapB = _WetnessMapB;
			_WetnessMapB = _WetnessMapA;
			_WetnessMapA = wetnessMapB;
			for (int l = 0; l < _Materials.Length; l++)
			{
				_Materials[l].SetVector("_LocalMapsCoords", _MainCamera.LocalMapsShaderCoords);
			}
			break;
		}
		}
	}

	private void OnValidate()
	{
		if (_WettableUtilShader == null)
		{
			_WettableUtilShader = Shader.Find("UltimateWater/Utility/Wetness Update");
		}
		if (_WettableUtilNearShader == null)
		{
			_WettableUtilNearShader = Shader.Find("UltimateWater/Utility/Wetness Update (Near Camera)");
		}
		if (_MeshRenderers == null || _MeshRenderers.Length == 0)
		{
			_MeshRenderers = GetComponentsInChildren<MeshRenderer>(includeInactive: true);
		}
		if (_Terrains == null || _Terrains.Length == 0)
		{
			_Terrains = GetComponentsInChildren<Terrain>(includeInactive: true);
		}
		Material[] array = _Materials;
		if (Application.isPlaying)
		{
			return;
		}
		if (array == null)
		{
			array = _MeshRenderers.SelectMany((MeshRenderer mr) => mr.sharedMaterials).Concat(_Terrains.Select((Terrain t) => t.materialTemplate)).ToArray();
		}
		if (_Mode == Mode.NearCamera)
		{
			for (int num = 0; num < array.Length; num++)
			{
				array[num].EnableKeyword("_WET_NEAR_CAMERA");
			}
		}
		else
		{
			for (int num2 = 0; num2 < array.Length; num2++)
			{
				array[num2].DisableKeyword("_WET_NEAR_CAMERA");
			}
		}
	}

	private void Awake()
	{
		if (_Water == null || _Water.DynamicWater == null)
		{
			enabled = false;
			return;
		}
		OnValidate();
		_TerrainLayers = new int[_Terrains.Length];
		_TerrainDrawTrees = new bool[_Terrains.Length];
		_MeshFilters = new MeshFilter[_MeshRenderers.Length];
		for (int i = 0; i < _MeshFilters.Length; i++)
		{
			_MeshFilters[i] = _MeshRenderers[i].GetComponent<MeshFilter>();
		}
		_WetnessMapA = new RenderTexture(_Resolution, _Resolution, 0, RenderTextureFormat.RHalf, RenderTextureReadWrite.Linear)
		{
			name = "[UWS] WettableSurface - Wetness Map",
			hideFlags = HideFlags.DontSave,
			filterMode = FilterMode.Bilinear
		};
		if (_Mode == Mode.NearCamera)
		{
			_WetnessMapB = new RenderTexture(_Resolution, _Resolution, 0, RenderTextureFormat.RHalf, RenderTextureReadWrite.Linear)
			{
				name = "[UWS] WettableSurface - Wetness Map",
				hideFlags = HideFlags.DontSave,
				filterMode = FilterMode.Bilinear
			};
		}
		Material[] materials = _MeshRenderers.SelectMany((MeshRenderer mr) => mr.sharedMaterials).Concat(_Terrains.Select((Terrain t) => t.materialTemplate)).Distinct()
			.ToArray();
		_Materials = InstantiateMaterials(materials);
		OnValidate();
		_WettableUtilMaterial = new Material((_Mode == Mode.TextureSpace) ? _WettableUtilShader : _WettableUtilNearShader)
		{
			hideFlags = HideFlags.DontSave
		};
	}

	private Material[] InstantiateMaterials(Material[] materials)
	{
		Material[] array = new Material[materials.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = UnityEngine.Object.Instantiate(materials[i]);
		}
		for (int j = 0; j < _MeshRenderers.Length; j++)
		{
			MeshRenderer meshRenderer = _MeshRenderers[j];
			Material[] sharedMaterials = meshRenderer.sharedMaterials;
			for (int k = 0; k < sharedMaterials.Length; k++)
			{
				int num = Array.IndexOf(materials, sharedMaterials[k]);
				sharedMaterials[k] = array[num];
			}
			meshRenderer.sharedMaterials = sharedMaterials;
		}
		for (int l = 0; l < _Terrains.Length; l++)
		{
			Terrain terrain = _Terrains[l];
			int num2 = Array.IndexOf(materials, terrain.materialTemplate);
			terrain.materialTemplate = array[num2];
		}
		for (int m = 0; m < array.Length; m++)
		{
			array[m].SetTexture("_WetnessMap", _WetnessMapA);
		}
		return array;
	}

	private Camera GetWettingCamera()
	{
		if (_WettingCamera == null)
		{
			GameObject gameObject = new GameObject("Wetting Camera")
			{
				hideFlags = HideFlags.DontSave
			};
			gameObject.transform.position = new Vector3(0f, 100000f, 0f);
			gameObject.transform.eulerAngles = new Vector3(90f, 0f, 0f);
			_WettingCamera = gameObject.AddComponent<Camera>();
			_WettingCamera.enabled = false;
			_WettingCamera.orthographic = true;
			_WettingCamera.orthographicSize = 1000000f;
			_WettingCamera.nearClipPlane = 10f;
			_WettingCamera.farClipPlane = 1000000f;
			_WettingCamera.cullingMask = 1 << WaterProjectSettings.Instance.WaterTempLayer;
			_WettingCamera.renderingPath = RenderingPath.VertexLit;
			_WettingCamera.clearFlags = CameraClearFlags.Nothing;
			_WettingCamera.targetTexture = _WetnessMapA;
		}
		return _WettingCamera;
	}
}
