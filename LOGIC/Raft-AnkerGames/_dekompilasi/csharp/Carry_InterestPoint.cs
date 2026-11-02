using UnityEngine;
using UnityEngine.InputSystem;

public class Carry_InterestPoint : Carry
{
	[SerializeField]
	private CarryNetworked_InterestPoint carryNetworkedInterest;

	protected override void ShowStopCarryText()
	{
		if (ShouldDisplayDropText())
		{
			if (carryNetworkedInterest.aimedAtInterestPoint == null)
			{
				base.ShowStopCarryText();
			}
			else
			{
				textManager.ShowText(Helper.GetTerm("Game/InsertCogwheel"), inputInteract.action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey, 0, 0, clearAllTexts: false);
			}
		}
	}
}
