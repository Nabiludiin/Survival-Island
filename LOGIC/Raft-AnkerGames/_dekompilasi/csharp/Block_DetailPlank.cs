public class Block_DetailPlank : Block
{
	public DetailPlankLength lengthSetter;

	public override RGD Serialize_Save()
	{
		return new RGD_Block_DetailPlank(RGDType.Block_ShortcutPlank, this);
	}

	public override RGD_Block GetBlockCreationData()
	{
		return new RGD_Block_DetailPlank(RGDType.Block_ShortcutPlank, this);
	}
}
