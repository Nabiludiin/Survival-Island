using Knife.Tools;
using UnityEngine;

namespace Knife.DeferredDecals.Spawn;

public class SkinnedMeshDecalReceiver : SimpleDecalReceiver
{
	private SkinnedMeshRenderer skinnedMeshRenderer;

	private BoneWeight[] boneWeights;

	private Mesh mesh;

	protected override void OnHitted(Decal decalInstance, GPURaycastDecalsTargetInfo hitInfo)
	{
		skinnedMeshRenderer = hitInfo.hittedRenderer as SkinnedMeshRenderer;
		if (skinnedMeshRenderer == null)
		{
			return;
		}
		mesh = skinnedMeshRenderer.sharedMesh;
		boneWeights = mesh.boneWeights;
		if (ParentOnHit)
		{
			BoneWeight boneWeight = boneWeights[hitInfo.VertexIndex];
			_ = boneWeight.boneIndex0;
			_ = boneWeight.boneIndex1;
			_ = boneWeight.boneIndex2;
			_ = boneWeight.boneIndex3;
			float weight = boneWeight.weight0;
			float weight2 = boneWeight.weight1;
			float weight3 = boneWeight.weight2;
			float weight4 = boneWeight.weight3;
			float num = Mathf.Max(weight, weight2, weight3, weight4);
			int num2 = -1;
			if (num == weight)
			{
				num2 = boneWeight.boneIndex0;
			}
			if (num == weight2)
			{
				num2 = boneWeight.boneIndex1;
			}
			if (num == weight3)
			{
				num2 = boneWeight.boneIndex2;
			}
			if (num == weight4)
			{
				num2 = boneWeight.boneIndex3;
			}
			decalInstance.transform.SetParent(skinnedMeshRenderer.bones[num2]);
		}
	}
}
