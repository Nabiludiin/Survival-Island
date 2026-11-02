public class AI_NetworkBehaviour_StoneBird : AI_NetworkBehaviour_Animal
{
	public AI_StateMachine_StoneBird stateMachineStoneBird;

	public override RGD_AI_NetworkBehaviour Create_SaveData()
	{
		return new RGD_Stonebird(this);
	}

	public override void OnRestoreFinished(RGD_AI_NetworkBehaviour rgdNetworkBehaviour)
	{
		base.OnRestoreFinished(rgdNetworkBehaviour);
		RGD_Stonebird rGD_Stonebird = rgdNetworkBehaviour as RGD_Stonebird;
		if (!(rGD_Stonebird == null))
		{
			rGD_Stonebird.RestoreBird(this);
		}
	}

	public override void Serialize_Update()
	{
		base.Serialize_Update();
	}

	public override void Serialize_Update_Animal()
	{
		AddMessage(new Message_StoneBird_Update(Messages.AINetworkBehaviourUpdate_StoneBird, Network.NetworkIDManager, ObjectIndex, stateMachineStoneBird));
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		if (msg.Type == Messages.StoneBird_DropStone)
		{
			Message_NetworkBehaviour_ID_Vector3 message_NetworkBehaviour_ID_Vector = msg as Message_NetworkBehaviour_ID_Vector3;
			stateMachineStoneBird.dropStoneState.DropStone(message_NetworkBehaviour_ID_Vector.Vector.Value);
			return false;
		}
		return base.Deserialize(msg, remoteID);
	}
}
