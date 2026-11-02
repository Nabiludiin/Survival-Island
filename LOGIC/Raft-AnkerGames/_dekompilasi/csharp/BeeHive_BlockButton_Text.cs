using UnityEngine;

public class BeeHive_BlockButton_Text : BlockButton_PickupItemText
{
	[SerializeField]
	private BeeHive beeHive;

	protected override void ShowOnHoverText(string p_localizationTerm, string p_keybindName)
	{
		if (beeHive.CurrentHoneyLevel != null)
		{
			base.ShowOnHoverText(p_localizationTerm, p_keybindName);
		}
	}
}
