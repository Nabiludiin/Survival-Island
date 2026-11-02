public class LandmarkItem_VendingMachine : LandmarkItem
{
	public override RGD_LandmarkItem GetSave()
	{
		return new RGD_LandmarkItem_VendingMachine(this);
	}
}
