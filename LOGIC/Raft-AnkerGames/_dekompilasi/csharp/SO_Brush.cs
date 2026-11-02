using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Brush", menuName = "PrefabPainter/Brush")]
public class SO_Brush : ScriptableObject
{
	public List<Transform> prefabs = new List<Transform>();

	public string paintParentName = string.Empty;

	public bool followTerrain;

	public float terrainOffset;

	public float minDistanceCheck = 0.1f;

	public Vector3 minRotation = Vector3.zero;

	public Vector3 maxRotation = Vector3.zero;

	public float minScale = 1f;

	public float maxScale = 1f;

	[HideInInspector]
	public Transform paintParent;

	public string tmpName = string.Empty;

	public float ghostScale = 1f;

	public Vector3 ghostEuler = Vector3.zero;

	public float brushRadius = 1f;

	private int currentPrefabIndex;

	public List<Transform> paintedObjects;

	public bool IsValidToPaintWith
	{
		get
		{
			if (HasValidPrefabs)
			{
				return !string.IsNullOrEmpty(paintParentName);
			}
			return false;
		}
	}

	public bool HasValidPrefabs
	{
		get
		{
			if (prefabs != null && prefabs.Count > 0)
			{
				return prefabs[0] != null;
			}
			return false;
		}
	}

	public int CurrentPrefabIndex
	{
		get
		{
			return currentPrefabIndex;
		}
		set
		{
			if (prefabs.Count == 0)
			{
				currentPrefabIndex = 0;
				return;
			}
			currentPrefabIndex = value;
			if (currentPrefabIndex > prefabs.Count - 1)
			{
				currentPrefabIndex = prefabs.Count - 1;
			}
			else if (currentPrefabIndex < -1)
			{
				currentPrefabIndex = -1;
			}
		}
	}
}
