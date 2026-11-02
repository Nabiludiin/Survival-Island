using UnityEngine;

public class InteractableButton_Networked_RadioCassette : InteractableButton_Networked
{
	[SerializeField]
	private RadioPlayer radio;

	[SerializeField]
	private string takeCassetteLocalizationTerm = string.Empty;

	protected override void HandleOnIsRayed()
	{
		if (radio.HasCassette || IsHoldingACassette())
		{
			base.HandleOnIsRayed();
		}
		else
		{
			InteractableButton.displayTextManager.HideDisplayTexts();
		}
	}

	protected override void ShowOnHoverText(string p_localizationTerm, string p_keybindName)
	{
		if (radio.HasCassette)
		{
			base.ShowOnHoverText(takeCassetteLocalizationTerm, p_keybindName);
		}
		else if (IsHoldingACassette())
		{
			base.ShowOnHoverText(p_localizationTerm, p_keybindName);
		}
	}

	protected override Message_InteractableButtonPress CreateInteractMessage()
	{
		int integer = 0;
		ItemInstance selectedHotbarItem = InteractableButton.localPlayer.Inventory.GetSelectedHotbarItem();
		if (selectedHotbarItem != null && selectedHotbarItem.baseItem != null)
		{
			RadioPlayer.CassetteConnection cassetConnectionIndexFromItem = radio.GetCassetConnectionIndexFromItem(selectedHotbarItem.baseItem);
			if (cassetConnectionIndexFromItem != null)
			{
				integer = cassetConnectionIndexFromItem.cassetteIndex;
			}
		}
		return new Message_InteractableButtonPress_Integer(InteractableButton.localPlayer.Network.NetworkIDManager, messageReciever.ObjectIndex, buttonIndex, InteractableButton.localPlayer, integer);
	}

	private bool IsHoldingACassette()
	{
		if (InteractableButton.localPlayer == null)
		{
			InteractableButton.localPlayer = ComponentManager<Network_Player>.Value;
		}
		ItemInstance selectedHotbarItem = InteractableButton.localPlayer.Inventory.GetSelectedHotbarItem();
		if (selectedHotbarItem != null)
		{
			return radio.IsItemACassette(selectedHotbarItem.baseItem);
		}
		return false;
	}
}
