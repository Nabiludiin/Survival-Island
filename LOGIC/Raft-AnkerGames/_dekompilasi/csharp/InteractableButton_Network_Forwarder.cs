using System;

[Serializable]
public class InteractableButton_Network_Forwarder
{
	public InteractableButton_Networked[] buttons;

	public InteractableButton_Network_Forwarder(InteractableButton_Networked[] buttons)
	{
		this.buttons = buttons;
	}

	public void HandleMessage(Message_InteractableButtonPress msg)
	{
		if (msg == null || buttons == null || !buttons.ContainsItems())
		{
			return;
		}
		InteractableButton_Networked[] array = buttons;
		foreach (InteractableButton_Networked interactableButton_Networked in array)
		{
			if (interactableButton_Networked != null && interactableButton_Networked.buttonIndex == msg.buttonIndex)
			{
				interactableButton_Networked.PlayButtonFeedback(msg.success);
			}
		}
	}
}
