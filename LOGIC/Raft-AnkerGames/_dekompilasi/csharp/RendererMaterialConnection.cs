using System;
using UnityEngine;

[Serializable]
public class RendererMaterialConnection
{
	public Renderer renderer;

	public Material originalMaterial;

	public void SaveOriginalMaterial()
	{
		if (originalMaterial == null && renderer != null)
		{
			originalMaterial = renderer.sharedMaterial;
		}
	}
}
