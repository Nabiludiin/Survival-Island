using UnityEngine;

public class SkinnedMeshRendererCopyBones : MonoBehaviour
{
	public Transform destinationSkeleton;

	public SkinnedMeshRenderer destinationRenderer;

	public SkinnedMeshRenderer sourceRenderer;

	public void CopyBones()
	{
		if (sourceRenderer == null)
		{
			Debug.LogError("Source renderer is null");
			return;
		}
		if (destinationRenderer == null)
		{
			Debug.LogError("Destination renderer is null");
			return;
		}
		Transform[] array = new Transform[sourceRenderer.bones.Length];
		for (int i = 0; i < sourceRenderer.bones.Length; i++)
		{
			array[i] = destinationSkeleton.FindChildRecursively(sourceRenderer.bones[i].transform.name);
		}
		destinationRenderer.bones = array;
		Debug.Log("Copied " + array.Length + " bones");
	}
}
