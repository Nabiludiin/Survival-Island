using System;
using Steamworks;

public class AI_NetworkBehaviour_Roach : AI_NetworkBehaviour_Animal
{
	public AI_State protectionState;

	public TriggerArrowEnterStateChange arrowEntered;

	protected override void Awake()
	{
		base.Awake();
		TriggerArrowEnterStateChange triggerArrowEnterStateChange = arrowEntered;
		triggerArrowEnterStateChange.OnTriggered = (Action)Delegate.Combine(triggerArrowEnterStateChange.OnTriggered, new Action(HandleProtection));
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		TriggerArrowEnterStateChange triggerArrowEnterStateChange = arrowEntered;
		triggerArrowEnterStateChange.OnTriggered = (Action)Delegate.Remove(triggerArrowEnterStateChange.OnTriggered, new Action(HandleProtection));
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		if (msg.Type == Messages.AI_Protect)
		{
			if (Raft_Network.IsHost && stateMachineAnimal != null)
			{
				ChangeToProtectionState();
				return false;
			}
			return false;
		}
		return base.Deserialize(msg, remoteID);
	}

	private void HandleProtection()
	{
		if (Raft_Network.IsHost)
		{
			ChangeToProtectionState();
			return;
		}
		Message_NetworkBehaviour_ID message = new Message_NetworkBehaviour_ID(Messages.AI_Protect, stateMachineAnimal.networkBehaviour.Network.NetworkIDManager, stateMachineAnimal.networkBehaviour.ObjectIndex);
		stateMachineAnimal.networkBehaviour.Network.SendP2P(stateMachineAnimal.networkBehaviour.Network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
	}

	private void ChangeToProtectionState()
	{
		if (stateMachineAnimal.currentState != protectionState)
		{
			stateMachineAnimal.ChangeState(protectionState);
		}
	}
}
