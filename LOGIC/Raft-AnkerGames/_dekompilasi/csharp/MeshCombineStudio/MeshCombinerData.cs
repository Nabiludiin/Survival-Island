using System.Collections.Generic;
using UnityEngine;

namespace MeshCombineStudio;

[ExecuteInEditMode]
public class MeshCombinerData : MonoBehaviour
{
	public Dictionary<Collider, CachedGameObject> colliderLookup = new Dictionary<Collider, CachedGameObject>();

	public Dictionary<LODGroup, CachedGameObject> lodGroupLookup = new Dictionary<LODGroup, CachedGameObject>();

	[HideInInspector]
	public List<GameObject> combinedGameObjects = new List<GameObject>();

	[HideInInspector]
	public List<CachedGameObject> foundObjects = new List<CachedGameObject>();

	[HideInInspector]
	public List<CachedLodGameObject> foundLodObjects = new List<CachedLodGameObject>();

	[HideInInspector]
	public List<LODGroup> foundLodGroups = new List<LODGroup>();

	[HideInInspector]
	public List<Collider> foundColliders = new List<Collider>();

	private void OnValidate()
	{
		hideFlags = HideFlags.HideInInspector;
	}

	private void OnEnable()
	{
		hideFlags = HideFlags.HideInInspector;
	}

	public void ClearFound()
	{
		foundObjects.Clear();
		foundLodObjects.Clear();
		foundLodGroups.Clear();
		foundColliders.Clear();
		colliderLookup.Clear();
		lodGroupLookup.Clear();
	}
}
