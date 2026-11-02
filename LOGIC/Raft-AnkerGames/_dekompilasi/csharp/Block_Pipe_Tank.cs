using UnityEngine;

public class Block_Pipe_Tank : Block_Pipe
{
	[SerializeField]
	private Placeable_Tank placeableTank;

	public override RGD Serialize_Save()
	{
		return new RGD_Block_FuelTank(placeableTank);
	}

	public override RGD_Block GetBlockCreationData()
	{
		return new RGD_Block_FuelTank(placeableTank);
	}
}
