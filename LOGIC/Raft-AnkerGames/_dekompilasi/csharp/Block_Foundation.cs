using UnityEngine;

public class Block_Foundation : Block
{
	[SerializeField]
	private bool reinforced;

	[SerializeField]
	private MeshFilter meshFilter;

	public MeshRenderer meshRenderer;

	[SerializeField]
	private Mesh defaultMesh;

	public Mesh armoredMesh;

	[SerializeField]
	private Material defaultMaterial;

	[SerializeField]
	private Material armoredMaterial;

	public override bool Reinforced
	{
		get
		{
			return reinforced;
		}
		set
		{
			reinforced = value;
			OnReinforcedToggle(value);
		}
	}

	private void OnReinforcedToggle(bool value)
	{
		if (value)
		{
			meshFilter.sharedMesh = armoredMesh;
			meshRenderer.sharedMaterial = armoredMaterial;
		}
		else
		{
			meshFilter.sharedMesh = defaultMesh;
			meshRenderer.sharedMaterial = defaultMaterial;
		}
	}

	public override RGD Serialize_Save()
	{
		return new RGD_Block_Foundation(RGDType.Block_Foundation, this);
	}

	public override RGD_Block GetBlockCreationData()
	{
		return new RGD_Block_Foundation(RGDType.Block_Foundation, this);
	}
}
