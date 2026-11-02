using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableButton_Networked_TicTacToe : InteractableButton_Networked
{
	public MeshRenderer renderer;

	public MeshFilter filter;

	[SerializeField]
	private Block_TicTacToe blockTicTacToe;

	protected override void HandleOnIsRayed()
	{
		if (!(blockTicTacToe == null))
		{
			if (renderer.enabled || !blockTicTacToe.IsFullOfPlacedBricks())
			{
				base.HandleOnIsRayed();
			}
			else
			{
				InteractableButton.displayTextManager.HideDisplayTexts();
			}
		}
	}

	protected override void ShowOnHoverText(string p_localizationTerm, string p_keybindName)
	{
		if (renderer.enabled)
		{
			base.ShowOnHoverText("PickUp", p_keybindName);
			return;
		}
		LocalizationParameters.itemX = (blockTicTacToe.ShouldPlaceXNext() ? "'X'" : "'O'");
		InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
		InteractableButton.displayTextManager.ShowText(Helper.GetTerm("Game/PlaceItemX", applyParameters: true), action.GetBindingDisplayString(), MyInput.Keybinds[p_keybindName].MainKey);
	}
}
