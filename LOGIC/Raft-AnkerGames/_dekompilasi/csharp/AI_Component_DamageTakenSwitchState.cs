using UnityEngine;

public class AI_Component_DamageTakenSwitchState : AI_Component, INTERFACE_AI_Component_DamageTakenEvent
{
	[SerializeField]
	private AI_State damageTakenState;

	[SerializeField]
	private float damageThreshold = 1f;

	[SerializeField]
	private float damageTaken;

	[SerializeField]
	private AI_State playerNearbyState;

	[SerializeField]
	private float nearbyRadius = 5f;

	public override void OnExitState()
	{
		base.OnExitState();
		damageTaken = 0f;
	}

	void INTERFACE_AI_Component_DamageTakenEvent.OnDamageTakenEvent(float damage, Vector3 hitPoint, Vector3 hitNormal, EntityType damageInflictorEntityType, bool isLocal)
	{
		if (!Raft_Network.IsHost || !connectedState.IsActive())
		{
			return;
		}
		damageTaken += damage;
		if (!Raft_Network.IsHost || !(damageTaken >= damageThreshold))
		{
			return;
		}
		if (playerNearbyState != null)
		{
			if (Helper.ClosestPlayerToPoint(connectedState.stateMachine.transform.position, nearbyRadius, includeDeadPlayers: false) != null)
			{
				SwitchState(playerNearbyState);
			}
			else if (damageTakenState != null)
			{
				SwitchState(damageTakenState);
			}
		}
		else if (damageTakenState != null)
		{
			SwitchState(damageTakenState);
		}
	}
}
