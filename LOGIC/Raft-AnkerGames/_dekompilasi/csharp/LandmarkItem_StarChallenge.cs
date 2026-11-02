using UnityEngine;

public class LandmarkItem_StarChallenge : LandmarkItem
{
	public override RGD_LandmarkItem GetSave()
	{
		StarChallenge component = GetComponent<StarChallenge>();
		if (component != null)
		{
			return component.GetRGDSave();
		}
		Debug.LogError("Can't save starchallenge, could not find ComputerChallengeScript next to landmarkitem");
		return null;
	}
}
