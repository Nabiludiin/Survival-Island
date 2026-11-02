public class LandmarkItem_CarryNetworked : LandmarkItem
{
	public CarryNetworked carryNetworked;

	public override void AssignIndexAndTrack(uint objectIndex)
	{
		carryNetworked.Initialize(objectIndex, carryNetworked.transform.parent);
	}

	protected virtual void OnEnable()
	{
		if (carryNetworked != null)
		{
			carryNetworked.gameObject.SetActiveSafe(value: true);
		}
	}

	protected virtual void OnDisable()
	{
		if (carryNetworked != null)
		{
			carryNetworked.gameObject.SetActiveSafe(value: false);
		}
	}

	public override RGD_LandmarkItem GetSave()
	{
		return new RGD_LandmarkItem_CarryNetworked(this, carryNetworked);
	}
}
