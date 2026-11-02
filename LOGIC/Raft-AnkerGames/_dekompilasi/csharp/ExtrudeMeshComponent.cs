using UnityEngine;

public class ExtrudeMeshComponent : MonoBehaviour, ExtrudeMesh.OnGenerateMesh
{
	[SerializeField]
	[Tooltip("Auto generate/clear when extrude script generates/clears")]
	protected bool attachToExtrudeMeshScript = true;

	[SerializeField]
	protected ExtrudeMesh extrudeMeshScript;

	private void OnValidate()
	{
		if (extrudeMeshScript == null)
		{
			extrudeMeshScript = GetComponent<ExtrudeMesh>();
		}
	}

	void ExtrudeMesh.OnGenerateMesh.OnGenerateMesh()
	{
		if (attachToExtrudeMeshScript)
		{
			OnExtrudeMeshGenerate();
		}
	}

	void ExtrudeMesh.OnGenerateMesh.OnClearMesh()
	{
		if (attachToExtrudeMeshScript)
		{
			OnExtrudeMeshClear();
		}
	}

	public GameObject InstantiatePrefab(GameObject prefab)
	{
		GameObject result = null;
		if (Application.isPlaying || !Application.isEditor)
		{
			result = Object.Instantiate(prefab);
		}
		return result;
	}

	protected virtual void OnExtrudeMeshGenerate()
	{
	}

	protected virtual void OnExtrudeMeshClear()
	{
	}
}
