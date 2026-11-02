using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class BackWallMaterialSetter : MonoBehaviour
{
	public Wall Wall;

	public Material TransparentMaterial;

	public Material Material;

	private void Start()
	{
		Wall.SetBackMaterial(TransparentMaterial, Material);
	}
}
