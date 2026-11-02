using UnityEngine;

public class Block_CookingTable_Juicer : Block
{
	[SerializeField]
	private CookingTable_Juicer juicer;

	public override RGD_Block GetBlockCreationData()
	{
		return new RGD_Block_CookingPot(this, new RGD_CookingTable_Juicer(juicer));
	}
}
