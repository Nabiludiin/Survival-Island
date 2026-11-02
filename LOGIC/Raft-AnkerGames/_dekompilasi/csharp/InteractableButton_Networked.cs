using Steamworks;
using UnityEngine;

public class InteractableButton_Networked : InteractableButton
{
	[SerializeField]
	protected MonoBehaviour_ID_Network messageReciever;

	[SerializeField]
	public int buttonIndex;

	private void Button_SetName()
	{
		transform.name = "Button_" + buttonIndex;
	}

	protected override bool ButtonPressed(bool hideDisplayText = true)
	{
		bool flag = base.ButtonPressed(hideDisplayText);
		if (InteractableButton.localPlayer == null)
		{
			InteractableButton.localPlayer = ComponentManager<Network_Player>.Value;
		}
		Message_InteractableButtonPress message_InteractableButtonPress = CreateInteractMessage();
		message_InteractableButtonPress.success = flag;
		InteractableButton.localPlayer.SendP2P(message_InteractableButtonPress, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		return flag;
	}

	protected virtual Message_InteractableButtonPress CreateInteractMessage()
	{
		return new Message_InteractableButtonPress(InteractableButton.localPlayer.Network.NetworkIDManager, messageReciever.ObjectIndex, buttonIndex, InteractableButton.localPlayer);
	}
}
