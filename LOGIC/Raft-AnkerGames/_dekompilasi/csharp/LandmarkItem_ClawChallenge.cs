using UnityEngine;

public class LandmarkItem_ClawChallenge : LandmarkItem
{
	public override RGD_LandmarkItem GetSave()
	{
		ClawChallenge component = GetComponent<ClawChallenge>();
		if (component != null)
		{
			return component.GetRGDSave();
		}
		Debug.LogError("Can't save clawchallenge, could not find ClawChallengeScript next to landmarkitem");
		return null;
	}
}
