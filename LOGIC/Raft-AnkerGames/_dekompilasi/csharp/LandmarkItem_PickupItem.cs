public class LandmarkItem_PickupItem : LandmarkItem
{
	public PickupItem pickupItem;

	public void Awake()
	{
		pickupItem = connectedBehaviourID.GetComponent<PickupItem>();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		pickupItem = null;
	}

	public override RGD_LandmarkItem GetSave()
	{
		return new RGD_LandmarkItem_PickupItem(this);
	}
}
