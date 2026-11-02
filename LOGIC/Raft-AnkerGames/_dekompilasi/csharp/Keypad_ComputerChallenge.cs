using TMPro;
using UnityEngine;

public class Keypad_ComputerChallenge : Keypad
{
	[SerializeField]
	private bool codeCompleted;

	[SerializeField]
	private GameObject lamp_red;

	[SerializeField]
	private GameObject lamp_green;

	[SerializeField]
	private GameObject codeUnderline;

	[SerializeField]
	private TextMeshPro timerTextMesh;

	private static ComputerChallenge challenge;

	public bool CodeCompleted => codeCompleted;

	private void Update()
	{
		if (!challenge.IsComplete)
		{
			if (challenge.ChallengeTimer > 0f)
			{
				timerTextMesh.text = challenge.ChallengeTimer.ToString("F1");
			}
			else
			{
				timerTextMesh.text = string.Empty;
			}
		}
	}

	public void SetCodeComplete()
	{
		canInteractWithKeyPad = false;
		codeCompleted = true;
		codeUnderline.SetActiveSafe(value: false);
		lamp_green.SetActiveSafe(value: true);
		lamp_red.SetActiveSafe(value: false);
	}

	public void ResetCodeCompleted()
	{
		if (!challenge.IsComplete)
		{
			canInteractWithKeyPad = true;
			codeCompleted = false;
			codeTextMesh.text = string.Empty;
			codeUnderline.SetActiveSafe(value: true);
			lamp_green.SetActiveSafe(value: false);
			lamp_red.SetActiveSafe(value: true);
		}
	}

	public void OnCompleteChallenge()
	{
		if (displayingFeedback)
		{
			StopCoroutine(displayFeedbackRoutine);
		}
		canInteractWithKeyPad = false;
		codeCompleted = false;
		codeTextMesh.text = string.Empty;
		timerTextMesh.text = string.Empty;
		codeUnderline.SetActiveSafe(value: false);
		lamp_green.SetActiveSafe(value: true);
		lamp_red.SetActiveSafe(value: false);
	}

	public void AssignComputerChallengeReference(ComputerChallenge computerChallenge)
	{
		if (challenge == null)
		{
			challenge = computerChallenge;
		}
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		if (challenge != null && !challenge.IsComplete)
		{
			return base.Deserialize(msg, remoteID);
		}
		return false;
	}
}
