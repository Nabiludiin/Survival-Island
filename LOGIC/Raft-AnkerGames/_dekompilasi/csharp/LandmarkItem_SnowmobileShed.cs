using UnityEngine;

public class LandmarkItem_SnowmobileShed : LandmarkItem
{
	[SerializeField]
	private SnowmobileShed shed;

	public override void OnLandmarkReset()
	{
		base.OnLandmarkReset();
		if (Raft_Network.IsHost)
		{
			shed.SpawnSnowmobileNetwork();
		}
	}

	public override RGD_LandmarkItem GetSave()
	{
		return new RGD_LandmarkItem_SnowmobileShed(this);
	}
}
