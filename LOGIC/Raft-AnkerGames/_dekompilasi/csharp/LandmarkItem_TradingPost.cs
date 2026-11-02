using UnityEngine;

public class LandmarkItem_TradingPost : LandmarkItem
{
	[SerializeField]
	private TradingPost tradingPost;

	public override void OnLandmarkReset()
	{
		base.OnLandmarkReset();
		tradingPost.ResetItemsForSale();
	}

	public override RGD_LandmarkItem GetSave()
	{
		return new RGD_LandmarkItem_TradingPost(this);
	}
}
