using UnityEngine;

public class AI_State_Boar_Circulate_Run : AI_State_CirculateSpawn
{
	[SerializeField]
	private AI_State damageTakenState;

	[SerializeField]
	private AI_State runAwayState;

	[SerializeField]
	private float stateTurnRate = 10f;

	private AI_StateMachine_Boar boarSM;

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Start()
	{
		boarSM = stateMachine as AI_StateMachine_Boar;
		base.Start();
		if (Raft_Network.IsHost)
		{
			boarSM.networkBehaviour.networkEntity.OnDamageTakenEvent += OnDamageTaken;
		}
	}

	public override void EnterState()
	{
		base.EnterState();
		stateMachine.movement.SetRotationSpeed(stateTurnRate);
	}

	public override void ExitState()
	{
		base.ExitState();
		stateMachine.movement.SetRotationSpeed(stateMachine.movement.defaultRotationSpeed);
	}

	protected override bool PlayerIsWithinRange()
	{
		if (GameModeValueManager.GetCurrentGameModeValue().boarVariables.isTame)
		{
			return false;
		}
		if (users != null)
		{
			foreach (Network_Player value in users.Values)
			{
				if (!(value == null) && !value.PlayerScript.IsDead)
				{
					Vector3 position = boarSM.transform.position;
					Vector3 position2 = value.transform.position;
					if (Vector3.Distance(position, position2) <= playerDistance && boarSM.movement.agent.enabled && boarSM.movement.IsValidNavmeshPath(position2))
					{
						boarSM.enemyTarget = value;
						return true;
					}
				}
			}
		}
		return false;
	}

	private void OnDamageTaken(float damage, Vector3 hitPoint, Vector3 hitNormal, EntityType damageType, bool isLocal)
	{
		if (boarSM.currentState != this || !boarSM.IsStillAlive() || boarSM.IsCurrentlyFearless)
		{
			return;
		}
		boarSM.enemyTarget = Helper.ClosestPlayerToPoint(boarSM.transform.position, includeDeadPlayers: false);
		if (boarSM.enemyTarget != null)
		{
			if (boarSM.movement.agent.enabled && boarSM.movement.IsValidNavmeshPath(boarSM.enemyTarget.transform.position))
			{
				boarSM.ChangeState(damageTakenState);
			}
			else if (!boarSM.IsCurrentlyFearless)
			{
				RunAway();
			}
		}
	}

	private void RunAway()
	{
		if (Raft_Network.IsHost)
		{
			boarSM.ChangeState(runAwayState);
		}
	}
}
