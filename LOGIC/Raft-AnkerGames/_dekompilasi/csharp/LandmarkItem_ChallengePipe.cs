public class LandmarkItem_ChallengePipe : LandmarkItem
{
	public Challenge_Pipe challengePipe;

	public override RGD_LandmarkItem GetSave()
	{
		return new RGD_LandmarkItem_ChallengePipe(RGDType.ChallengePipe, RGDSortingOrder.Pickups, this);
	}
}
