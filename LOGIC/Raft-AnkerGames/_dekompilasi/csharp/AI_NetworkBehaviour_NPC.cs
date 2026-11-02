using UnityEngine;

public class AI_NetworkBehaviour_NPC : AI_NetworkBehaviour_Animal
{
	[SerializeField]
	private AI_Dialogue dialogue;

	private AI_Statemachine_NPC stateMachineNPC;

	protected override void Awake()
	{
		base.Awake();
		stateMachineNPC = stateMachineAnimal as AI_Statemachine_NPC;
		LipSync componentInChildren = GetComponentInChildren<LipSync>();
		if (componentInChildren != null)
		{
			componentInChildren.dialogue = dialogue;
		}
	}

	private void OnValidate()
	{
		if (dialogue == null)
		{
			dialogue = GetComponentInChildren<AI_Dialogue>();
		}
	}

	public override void Serialize_Update_Animal()
	{
		AddMessage(new Message_NPC_Update(Messages.AINetworkBehaviourUpdate_NPC, Network.NetworkIDManager, ObjectIndex, stateMachineNPC));
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		if (msg.Type == Messages.Dialogue)
		{
			Message_Dialogue message_Dialogue = msg as Message_Dialogue;
			if (msg != null)
			{
				dialogue.PlayVoice(message_Dialogue, network.GetPlayerFromID(message_Dialogue.SteamID));
				return true;
			}
		}
		return base.Deserialize(msg, remoteID);
	}
}
