public class AI_NetworkBehaviour_PufferFish : AI_NetworkBehaviour_Animal
{
	public AI_StateMachine_PufferFish stateMachinePufferFish;

	public override void Serialize_Update()
	{
		base.Serialize_Update();
	}

	public override void Serialize_Update_Animal()
	{
		AddMessage(new Message_PufferFish_Update(Messages.AINetworkBehaviourUpdate_PufferFish, Network.NetworkIDManager, ObjectIndex, stateMachinePufferFish));
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		if (msg.Type == Messages.PufferFish_Explode)
		{
			Message_NetworkBehaviour_ID_Vector3 message_NetworkBehaviour_ID_Vector = msg as Message_NetworkBehaviour_ID_Vector3;
			stateMachinePufferFish.state_explode.Explode(message_NetworkBehaviour_ID_Vector.Vector.Value);
			return false;
		}
		return base.Deserialize(msg, remoteID);
	}
}
