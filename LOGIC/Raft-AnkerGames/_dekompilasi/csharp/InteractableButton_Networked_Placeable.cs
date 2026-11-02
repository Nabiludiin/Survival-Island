public class InteractableButton_Networked_Placeable : InteractableButton_Networked
{
	private Placeable_Interactable placeableInteractable;

	protected override void Start()
	{
		base.Start();
		placeableInteractable = GetComponentInChildren<Placeable_Interactable>(includeInactive: true);
	}

	protected override void ShowOnHoverText(string p_localizationTerm, string p_keybindName)
	{
		if (placeableInteractable.IsOnCooldown)
		{
			InteractableButton.displayTextManager.HideDisplayTexts();
		}
		else
		{
			base.ShowOnHoverText(p_localizationTerm, p_keybindName);
		}
	}
}
