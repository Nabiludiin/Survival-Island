using UnityEngine;

public class InteractableButton_Tank : InteractableButton
{
	public Tank tank;

	[SerializeField]
	private string tankDoesntHaveEnoughWaterLocTerm = string.Empty;

	protected override void ShowOnHoverText(string p_localizationTerm, string p_keybindName)
	{
		if (!tank.PlayerHandItemIsAcceptedOutput())
		{
			return;
		}
		if (tank.HasEnoughFuelComparedToHandItem())
		{
			base.ShowOnHoverText(p_localizationTerm, p_keybindName);
			return;
		}
		string term = Helper.GetTerm(tankDoesntHaveEnoughWaterLocTerm);
		if (term.IsNullOrEmpty())
		{
			term = tankDoesntHaveEnoughWaterLocTerm;
		}
		InteractableButton.displayTextManager.ShowText(term);
	}

	protected override bool CanButtonBePressed()
	{
		if (tank.HasEnoughFuelComparedToHandItem())
		{
			return base.CanButtonBePressed();
		}
		return false;
	}
}
