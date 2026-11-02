using UnityEngine;

public class Block_CookingTable_Pot : Block
{
	[SerializeField]
	private CookingTable_Pot cookingPot;

	public override RGD_Block GetBlockCreationData()
	{
		return new RGD_Block_CookingPot(this, new RGD_CookingTable_Pot(cookingPot));
	}
}
