using UnityEngine;

public class LandmarkItem_Container : LandmarkItem
{
	[SerializeField]
	private Container container;

	public override void OnLandmarkReset()
	{
		base.OnLandmarkReset();
		if (container != null)
		{
			container.DestroyAllItems();
		}
	}

	public override RGD_LandmarkItem GetSave()
	{
		return new RGD_LandmarkItem_Container(this, container);
	}
}
