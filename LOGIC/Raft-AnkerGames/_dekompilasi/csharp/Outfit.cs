using System;
using UnityEngine;

[Serializable]
public class Outfit
{
	public Sprite sprite;

	public Sprite fullBodySprite;

	public Material bodyMaterial;

	public Material fpArmMaterial;

	public Material hairMaterial;

	public void ApplyOutfit(SkinnedMeshRenderer bodyMesh, SkinnedMeshRenderer fpArmsMesh, SkinnedMeshRenderer[] hairMeshes)
	{
		if (bodyMesh.sharedMaterials.Length > 1)
		{
			Material[] sharedMaterials = bodyMesh.sharedMaterials;
			sharedMaterials[0] = bodyMaterial;
			bodyMesh.sharedMaterials = sharedMaterials;
		}
		else
		{
			bodyMesh.sharedMaterial = bodyMaterial;
		}
		if (fpArmsMesh != null)
		{
			fpArmsMesh.sharedMaterial = fpArmMaterial;
		}
		for (int i = 0; i < hairMeshes.Length; i++)
		{
			hairMeshes[i].sharedMaterial = hairMaterial;
		}
	}
}
