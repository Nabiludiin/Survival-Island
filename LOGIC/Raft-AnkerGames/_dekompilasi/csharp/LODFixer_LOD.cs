using System;
using UnityEngine;

[Serializable]
public class LODFixer_LOD
{
	[HideInInspector]
	public string name;

	public Renderer[] renderers;

	[Range(0f, 1f)]
	public float screenHeight;
}
