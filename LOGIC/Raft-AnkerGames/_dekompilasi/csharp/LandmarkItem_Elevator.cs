public class LandmarkItem_Elevator : LandmarkItem
{
	public override RGD_LandmarkItem GetSave()
	{
		return new RGD_LandmarkItem_Elevator(GetComponent<LandmarkItem>());
	}
}
