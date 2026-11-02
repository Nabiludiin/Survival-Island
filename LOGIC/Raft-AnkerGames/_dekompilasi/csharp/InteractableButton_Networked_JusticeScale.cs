public class InteractableButton_Networked_JusticeScale : InteractableButton_Networked
{
	public int weight;

	protected override Message_InteractableButtonPress CreateInteractMessage()
	{
		return new Message_InteractableButtonPress_Integer(InteractableButton.localPlayer.Network.NetworkIDManager, messageReciever.ObjectIndex, buttonIndex, InteractableButton.localPlayer, weight);
	}
}
