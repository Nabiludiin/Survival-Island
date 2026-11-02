using UnityEngine;
using UnityEngine.InputSystem;

public class BlockButton_PickupItemText : BlockButton
{
	[SerializeField]
	private Item_Base pickupItem;

	private void OnValidate()
	{
		if (pickupItem != null)
		{
			localizationTerm = "Game/PickUpX";
		}
	}

	public Item_Base GetPickupItem()
	{
		return pickupItem;
	}

	protected override void ShowOnHoverText(string p_localizationTerm, string p_keybindName)
	{
		if (!(pickupItem == null))
		{
			LocalizationParameters.itemX = pickupItem.settings_Inventory.DisplayName;
			InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
			InteractableButton.displayTextManager.ShowText(Helper.GetTerm(localizationTerm, applyParameters: true), action.GetBindingDisplayString(), MyInput.Keybinds[p_keybindName].MainKey);
		}
	}
}
