using UnityEngine;

public class AI_State_Chase_Water : AI_State_Chase
{
	public override void UpdateState()
	{
		base.UpdateState();
		if (Raft_Network.IsHost && !Helper.IsValidWaterTarget(target))
		{
			target = FindNewTarget();
		}
	}

	protected override void Chase(Network_Player targetPlayer, Vector3 chasePosition)
	{
		chasePosition += Vector3.up * 0.75f;
		if (chasePosition.y > -1f)
		{
			chasePosition.y = -1f;
		}
		base.Chase(targetPlayer, chasePosition);
	}

	protected override Network_Player FindNewTarget()
	{
		if (network == null)
		{
			network = ComponentManager<Raft_Network>.Value;
		}
		Network_Player network_Player = null;
		float num = float.MaxValue;
		foreach (Network_Player value in network.remoteUsers.Values)
		{
			if (!(value == null) && Helper.IsValidWaterTarget(value))
			{
				float num2 = Vector3.Distance(value.transform.position, stateMachine.transform.position);
				if (network_Player == null || num2 < num)
				{
					num = num2;
					network_Player = value;
				}
			}
		}
		if (network_Player != null)
		{
			return network_Player;
		}
		stateMachine.ChangeState(fallbackState);
		return null;
	}
}
