public class Block_ElectricPurifier : Block_Pipe
{
	public Electric_Purifier purifier;

	public override RGD_Block GetBlockCreationData()
	{
		return new RGD_Block_ElectricPurifier(purifier);
	}
}
