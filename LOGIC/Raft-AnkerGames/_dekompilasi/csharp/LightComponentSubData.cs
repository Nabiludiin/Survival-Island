using System;
using UnityEngine;

[Serializable]
public class LightComponentSubData
{
	public Light[] lights;

	public Renderer[] renderers;

	public Material normalMaterial;

	public Material emissionMaterial;
}
