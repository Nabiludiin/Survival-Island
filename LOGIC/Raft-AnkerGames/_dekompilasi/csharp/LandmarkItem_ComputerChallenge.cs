using UnityEngine;

public class LandmarkItem_ComputerChallenge : LandmarkItem
{
	public override RGD_LandmarkItem GetSave()
	{
		ComputerChallenge component = GetComponent<ComputerChallenge>();
		if (component != null)
		{
			return component.GetRGDSave();
		}
		Debug.LogError("Can't save computerchallenge, could not find ComputerChallengeScript next to landmarkitem");
		return null;
	}
}
