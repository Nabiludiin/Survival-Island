using UnityEngine;

namespace MeshCombineStudio;

[ExecuteInEditMode]
public class LightmapSettings : MonoBehaviour
{
	public MeshRenderer mr;

	public int lightmapIndex;

	private void OnEnable()
	{
		if ((bool)mr)
		{
			mr.lightmapIndex = lightmapIndex;
		}
	}
}
