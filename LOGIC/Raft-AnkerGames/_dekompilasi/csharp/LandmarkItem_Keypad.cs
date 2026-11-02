public class LandmarkItem_Keypad : LandmarkItem
{
	public override RGD_LandmarkItem GetSave()
	{
		return new RGD_LandmarkItem_Keypad(GetComponent<LandmarkItem>());
	}
}
