using UnityEngine;

public class DialogueMessageForwarder : MonoBehaviour_ID_Network
{
	[SerializeField]
	private Dialogue dialogue;

	private Raft_Network network;

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		if (msg.Type == Messages.Dialogue)
		{
			Message_Dialogue message_Dialogue = msg as Message_Dialogue;
			if (msg != null)
			{
				if (network == null)
				{
					network = ComponentManager<Raft_Network>.Value;
				}
				dialogue.PlayVoice(message_Dialogue, network.GetPlayerFromID(message_Dialogue.SteamID));
				return true;
			}
		}
		return base.Deserialize(msg, remoteID);
	}
}
