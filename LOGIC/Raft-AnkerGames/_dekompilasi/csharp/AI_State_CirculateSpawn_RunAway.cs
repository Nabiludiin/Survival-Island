using UnityEngine;

public class AI_State_CirculateSpawn_RunAway : AI_State_CirculateSpawn
{
	[SerializeField]
	private Network_Entity networkEntity;

	[SerializeField]
	private AI_State damageTakenState;

	[SerializeField]
	private AI_State runAwayState;

	protected override void Start()
	{
		base.Start();
		networkEntity.OnDamageTakenEvent += OnDamageTaken;
	}

	protected override void Destroy()
	{
		base.Destroy();
		if (networkEntity != null)
		{
			networkEntity.OnDamageTakenEvent -= OnDamageTaken;
		}
	}

	private void OnDamageTaken(float damage, Vector3 hitPoint, Vector3 hitNormal, EntityType damageType, bool isLocal)
	{
		if (!active || networkEntity.IsDead)
		{
			return;
		}
		Network_Player network_Player = Helper.ClosestPlayerToPoint(stateMachine.transform.position, includeDeadPlayers: false);
		if (!(network_Player != null))
		{
			return;
		}
		switch (stateMachine.movement.MovementType)
		{
		case MovementType.NavmeshAgent:
			if (stateMachine.movement.agent.enabled && stateMachine.movement.IsValidNavmeshPath(network_Player.transform.position))
			{
				stateMachine.ChangeState(damageTakenState);
			}
			break;
		case MovementType.Translate:
		case MovementType.CharacterController:
		case MovementType.None:
			stateMachine.ChangeState(damageTakenState);
			break;
		}
	}
}
