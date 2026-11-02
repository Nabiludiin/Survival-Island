using UnityEngine;

public class InteractableButton_Networked_ClawChallenge : InteractableButton_Networked
{
	[SerializeField]
	private ClawChallenge clawChallengeRef;

	protected override bool CanButtonBePressed()
	{
		return clawChallengeRef.hasElectricity;
	}
}
