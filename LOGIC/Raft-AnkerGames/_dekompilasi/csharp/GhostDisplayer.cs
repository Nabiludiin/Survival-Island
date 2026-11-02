using UnityEngine;

public class GhostDisplayer : MonoBehaviour
{
	public Material validMaterial;

	public Material invalidMaterial;

	public MeshRenderer meshRenderer;

	public MeshFilter meshFilter;

	public void Show()
	{
		gameObject.SetActiveSafe(value: true);
	}

	public void Hide()
	{
		gameObject.SetActiveSafe(value: false);
	}

	public void Remove()
	{
		Object.Destroy(gameObject);
	}
}
