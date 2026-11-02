using UnityEngine;

public class TicTacToe : MonoBehaviour_ID_Network
{
	[SerializeField]
	private Block_TicTacToe blockTicTacToe;

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		Messages type = msg.Type;
		if (type == Messages.InteractableButtonPress || type == Messages.InteractableButtonPress_Int)
		{
			Message_InteractableButtonPress message_InteractableButtonPress = msg as Message_InteractableButtonPress;
			return blockTicTacToe.InteractWithButton(message_InteractableButtonPress.buttonIndex);
		}
		return base.Deserialize(msg, remoteID);
	}
}
