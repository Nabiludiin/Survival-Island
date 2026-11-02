public class Block_Foundation_ItemNet : Block_Foundation
{
	public override RGD Serialize_Save()
	{
		return new RGD_ItemCollector(RGDType.Block_ItemCollector, this, GetComponentInChildren<ItemCollector>(includeInactive: true));
	}

	public override RGD_Block GetBlockCreationData()
	{
		return new RGD_ItemCollector(RGDType.Block_ItemCollector, this, GetComponentInChildren<ItemCollector>(includeInactive: true));
	}
}
