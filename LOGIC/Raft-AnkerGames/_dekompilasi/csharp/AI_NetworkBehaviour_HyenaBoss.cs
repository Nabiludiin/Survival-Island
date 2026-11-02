using UnityEngine;

public class AI_NetworkBehaviour_HyenaBoss : AI_NetworkBehaviour_Animal
{
	[SerializeField]
	private AI_State_PinDown state_PinDown;

	[SerializeField]
	private AI_State_Throw state_Throw;

	[SerializeField]
	private AI_State_BunnyJump state_Jump;

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.HyenaBoss_StartPin:
		case Messages.HyenaBoss_StopPin:
			return state_PinDown.FakeDeserialize(msg, remoteID);
		case Messages.HyenaBoss_SpawnStone:
		case Messages.HyenaBoss_ThrowStone:
		case Messages.HyenaBoss_DigStoneMultiplier:
			return state_Throw.FakeDeserialize(msg, remoteID);
		case Messages.HyenaBoss_LandJump:
			return state_Jump.FakeDeserialize(msg, remoteID);
		default:
			return base.Deserialize(msg, remoteID);
		}
	}
}
