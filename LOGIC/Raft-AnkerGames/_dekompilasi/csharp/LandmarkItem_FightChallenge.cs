using UnityEngine;

public class LandmarkItem_FightChallenge : LandmarkItem
{
	public Fight_Challenge_Selene fightChallenge;

	public override RGD_LandmarkItem GetSave()
	{
		if (fightChallenge != null)
		{
			return fightChallenge.GetRGDSave();
		}
		Debug.LogError("Can't save temperance_FightChallenge, unassigned fight challenge");
		return null;
	}

	protected override void ResetLandmarkItem()
	{
		base.ResetLandmarkItem();
		if (fightChallenge.isCompleted)
		{
			fightChallenge.CompleteChallenge();
		}
	}
}
