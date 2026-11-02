using UnityEngine;

public class AI_NetworkBehavior_Shark : AI_NetworkBehaviour_Animal
{
	public AI_StateMachine_Shark stateMachineShark;

	private bool networkIsParentedToRaft;

	private bool networkIsBiting;

	public override void OnRestoreFinished(RGD_AI_NetworkBehaviour rgdNetworkBehaviour)
	{
		base.OnRestoreFinished(rgdNetworkBehaviour);
		stateMachineShark.RefreshHPShader();
	}

	protected override void UpdateRemoteclient()
	{
		LerpMovement(networkPosition, networkIsParentedToRaft);
		LerpRotation(networkEuler);
	}

	protected override void LerpRotation(Vector3 eulerAngles)
	{
		stateMachineAnimal.transform.rotation = Quaternion.Lerp(stateMachineAnimal.transform.rotation, Quaternion.Euler(networkEuler), Time.deltaTime * rotationLerpSpeed);
	}

	public override void Serialize_Update_Animal()
	{
		AddMessage(new Message_NewShark_Update(Messages.AINetworkBehaviourUpdate_Shark, Network.NetworkIDManager, ObjectIndex, stateMachineShark, stateMachineShark.biteRaftState.isBitingRaft || stateMachineShark.biteBaitState.isBitingRaft));
	}

	public override RGD Serialize_Save()
	{
		return new RGD_AI_NetworkBehaviour(this);
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		if (msg.Type == Messages.AINetworkBehaviourUpdate_Shark)
		{
			base.Deserialize(msg, remoteID);
			Message_NewShark_Update message_NewShark_Update = msg as Message_NewShark_Update;
			stateMachineShark.animController.SetBool("BitingRaft", message_NewShark_Update.Biting);
			if (message_NewShark_Update.Biting && !networkIsBiting)
			{
				stateMachineShark.biteRaftState.DoBiteSoundAndParticles();
			}
			if (message_NewShark_Update.IsParentedToRaft && !networkIsParentedToRaft)
			{
				stateMachineShark.transform.SetParent(stateMachineShark.lockedPivot);
			}
			else if (!message_NewShark_Update.IsParentedToRaft && networkIsParentedToRaft)
			{
				stateMachineShark.transform.SetParent(null);
			}
			networkIsBiting = message_NewShark_Update.Biting;
			networkIsParentedToRaft = message_NewShark_Update.IsParentedToRaft;
			return false;
		}
		return base.Deserialize(msg, remoteID);
	}
}
