using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class FrontWallMaterialSetter : MonoBehaviour
{
	public Wall Wall;

	public Material TransparentMaterial;

	public Material Material;

	private void Awake()
	{
		if (Wall.AreMaterialsSetUp)
		{
			Wall.SetFrontMaterial(TransparentMaterial, Material);
		}
		else
		{
			Wall.OnMaterialsSetUp.AddOneTime(SetFrontMaterial);
		}
	}

	private void SetFrontMaterial(Wall wall)
	{
		Wall.SetFrontMaterial(TransparentMaterial, Material);
	}
}
