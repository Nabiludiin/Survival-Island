using UnityEngine;

public struct MaterialRendConnection
{
	public Renderer renderer;

	public Material[] originalMaterials;

	private MaterialPropertyBlock matPropBlock;

	public void SetMaterial(Material mat)
	{
		GameManager singleton = SingletonGeneric<GameManager>.Singleton;
		Material[] sharedMaterials = renderer.sharedMaterials;
		for (int i = 0; i < sharedMaterials.Length; i++)
		{
			Material material = sharedMaterials[i];
			if (material == null)
			{
				continue;
			}
			if (singleton != null && material.shader == singleton.blockPaintShader)
			{
				if (matPropBlock == null)
				{
					matPropBlock = new MaterialPropertyBlock();
				}
				renderer.GetPropertyBlock(matPropBlock, i);
				matPropBlock.SetColor("_BuildingEmission", (mat == singleton.ghostMaterialGreen) ? singleton.shaderGreen : singleton.shaderRed);
				renderer.SetPropertyBlock(matPropBlock, i);
			}
			else
			{
				sharedMaterials[i] = mat;
			}
		}
		renderer.sharedMaterials = sharedMaterials;
	}

	public void RestoreMaterials()
	{
		Material[] sharedMaterials = renderer.sharedMaterials;
		for (int i = 0; i < sharedMaterials.Length; i++)
		{
			sharedMaterials[i] = originalMaterials[i];
		}
		renderer.sharedMaterials = sharedMaterials;
	}
}
