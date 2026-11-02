using UnityEngine;

public class Block_Interactable : Block
{
	[SerializeField]
	private Placeable_Interactable interactable;

	public override RGD_Block GetBlockCreationData()
	{
		return new RGD_Block_Interactable(RGDType.Block_Interactable, this, interactable.CurrentIndex);
	}
}
