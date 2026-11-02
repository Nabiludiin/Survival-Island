using UnityEngine;

namespace MeshCombineStudio;

[ExecuteInEditMode]
public class GarbageCollectMesh : MonoBehaviour
{
	public static bool SkipGC;

	public Mesh mesh;

	private void OnDestroy()
	{
		if (mesh != null && !SkipGC)
		{
			Object.Destroy(mesh);
		}
	}
}
