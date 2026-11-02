using System;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Model : MonoBehaviour
{
	[Serializable]
	public class NPCVisualVariant
	{
		[HideInInspector]
		public SkinnedMeshRenderer skinnedRenderer;

		public Mesh mesh;

		public Material material;

		public int materialIndex;

		private void Use()
		{
			skinnedRenderer.sharedMesh = mesh;
			Material[] sharedMaterials = skinnedRenderer.sharedMaterials;
			sharedMaterials[materialIndex] = material;
			skinnedRenderer.sharedMaterials = sharedMaterials;
		}
	}

	[Serializable]
	public class NPCVisual
	{
		public SkinnedMeshRenderer skinnedRenderer;

		public NPCVisualVariant[] variants;
	}

	public Transform handParent;

	public List<NPCVisual> visuals = new List<NPCVisual>();

	private void ApplySkinnedToRenderer()
	{
		foreach (NPCVisual visual in visuals)
		{
			NPCVisualVariant[] variants = visual.variants;
			for (int i = 0; i < variants.Length; i++)
			{
				variants[i].skinnedRenderer = visual.skinnedRenderer;
			}
		}
	}
}
