using UnityEngine;

public class AI_State_CirculateSpawn_Air_StoneBird : AI_State_CirculateSpawn_Air
{
	[SerializeField]
	private AI_State_AirLand landState;

	[SerializeField]
	private AI_State_StoneBird_DropStone dropStoneState;

	private AI_StateMachine_StoneBird statemachineStoneBird;

	protected override void Awake()
	{
		base.Awake();
		statemachineStoneBird = stateMachine as AI_StateMachine_StoneBird;
	}

	protected override void Start()
	{
		base.Start();
		statemachineStoneBird.networkBehaviour.networkEntity.OnDamageTakenEvent += OnDamageTaken;
	}

	public override void UpdateState()
	{
		if (Raft_Network.IsHost)
		{
			Circulate();
			if (PlayerIsWithinRange())
			{
				SwitchToDroppingStone();
			}
		}
	}

	protected override bool PlayerIsWithinRange()
	{
		if (GameModeValueManager.GetCurrentGameModeValue().stonebirdVariables.isTame)
		{
			return false;
		}
		if (users != null)
		{
			foreach (Network_Player value in users.Values)
			{
				if (!(value == null) && !value.PlayerScript.IsDead)
				{
					Vector3 position = statemachineStoneBird.transform.position;
					Vector3 position2 = value.transform.position;
					position2.y = position.y;
					if (Vector3.Distance(position, position2) <= playerDistance)
					{
						statemachineStoneBird.target = value;
						return true;
					}
				}
			}
		}
		return false;
	}

	private void SwitchToDroppingStone()
	{
		if (statemachineStoneBird.dropStoneState.CanDropStone)
		{
			if (statemachineStoneBird.dropStoneState.HasStone)
			{
				stateMachine.ChangeState(dropStoneState);
			}
			else if (landState.SetRandomLandPosition())
			{
				stateMachine.ChangeState(landState);
			}
		}
	}

	private void OnDamageTaken(float damage, Vector3 hitPoint, Vector3 hitNormal, EntityType damageType, bool isLocal)
	{
		if (!statemachineStoneBird.networkBehaviour.networkEntity.IsDead)
		{
			Network_Player network_Player = Helper.ClosestPlayerToPoint(statemachineStoneBird.transform.position, includeDeadPlayers: false);
			if (statemachineStoneBird.target == null || statemachineStoneBird.target != network_Player)
			{
				statemachineStoneBird.target = network_Player;
			}
			if (statemachineStoneBird.target != null)
			{
				SwitchToDroppingStone();
			}
		}
	}
}
