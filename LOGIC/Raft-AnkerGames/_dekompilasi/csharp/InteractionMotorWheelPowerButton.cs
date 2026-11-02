using UnityEngine;

public class InteractionMotorWheelPowerButton : BlockButton
{
	public MotorWheel motorWheel;

	[SerializeField]
	private string localizationTermMotorWheelTurnOff = "Game/MotorWheel/TurnOff";

	protected override void ShowOnHoverText(string p_localizationTerm, string p_keybindName)
	{
		if (motorWheel.engineSwitchOn)
		{
			p_localizationTerm = localizationTermMotorWheelTurnOff;
		}
		base.ShowOnHoverText(p_localizationTerm, p_keybindName);
	}
}
