using UnityEngine;

public class SO_ObjectPlacement : ScriptableObject
{
	public int sortingOrder;

	public string assetName;

	[HideInInspector]
	public string tmpAssetName;

	public string parentName;

	public GameObject[] prefabs;

	public bool alignToNormal = true;

	public float offsetFromSurface;

	public bool randomizePrefab;

	public GameObject defaultPrefab;

	public bool manipulateRotation;

	public Vector3 minRotationOffset = Vector3.zero;

	public Vector3 maxRotationOffset = Vector3.zero;

	public bool manipulateScale;

	public bool uniformScale = true;

	public Vector3 minScale = Vector3.one;

	public Vector3 maxScale = Vector3.one * 2f;

	public float minUniformScale = 1f;

	public float maxUniformScale = 1f;

	public float Custom_YRotation;

	public float Custom_ZRotation;

	public Vector3 Custom_Scale = Vector3.one;

	public bool HasValidPrefab
	{
		get
		{
			if (defaultPrefab != null)
			{
				return true;
			}
			if (prefabs == null || prefabs.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < prefabs.Length; i++)
			{
				if (prefabs[i] == null)
				{
					return false;
				}
			}
			return true;
		}
	}

	public GameObject Prefab
	{
		get
		{
			if (randomizePrefab)
			{
				if (prefabs == null || prefabs.Length == 0)
				{
					return null;
				}
				return prefabs[Random.Range(0, prefabs.Length)];
			}
			return defaultPrefab;
		}
	}

	public void CopySettings(SO_ObjectPlacement otherAsset, bool copyName)
	{
		if (copyName)
		{
			assetName = otherAsset.assetName;
			tmpAssetName = otherAsset.tmpAssetName;
		}
		parentName = otherAsset.parentName;
		prefabs = otherAsset.prefabs;
		alignToNormal = otherAsset.alignToNormal;
		offsetFromSurface = otherAsset.offsetFromSurface;
		randomizePrefab = otherAsset.randomizePrefab;
		defaultPrefab = otherAsset.defaultPrefab;
		manipulateRotation = otherAsset.manipulateRotation;
		minRotationOffset = otherAsset.minRotationOffset;
		maxRotationOffset = otherAsset.maxRotationOffset;
		manipulateScale = otherAsset.manipulateScale;
		uniformScale = otherAsset.uniformScale;
		minScale = otherAsset.minScale;
		maxScale = otherAsset.maxScale;
		minUniformScale = otherAsset.minUniformScale;
		maxUniformScale = otherAsset.maxUniformScale;
		Custom_YRotation = otherAsset.Custom_YRotation;
		Custom_ZRotation = otherAsset.Custom_ZRotation;
		Custom_Scale = otherAsset.Custom_Scale;
	}
}
