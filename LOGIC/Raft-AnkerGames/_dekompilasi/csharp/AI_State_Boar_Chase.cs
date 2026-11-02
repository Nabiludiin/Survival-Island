using UnityEngine;

public class AI_State_Boar_Chase : AI_State_Chase
{
	[SerializeField]
	private AI_State chargeChannelState;

	[SerializeField]
	private AI_State runAwayState;

	private AI_StateMachine_Boar boarSM;

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Start()
	{
		boarSM = stateMachine as AI_StateMachine_Boar;
	}

	public override void UpdateState()
	{
		if (!Raft_Network.IsHost)
		{
			return;
		}
		if (boarSM.enemyTarget == null)
		{
			boarSM.enemyTarget = Helper.ClosestPlayerToPoint(boarSM.transform.position, includeDeadPlayers: false);
		}
		else if (IsInRangeOfPoint(boarSM.enemyTarget.transform.position))
		{
			if (stateMachine.movement.agent.enabled)
			{
				if (stateMachine.movement.IsValidNavmeshPath(Helper.GetClosestPointOnNavMesh(boarSM.enemyTarget.transform.position, 4f)))
				{
					boarSM.ChangeState(chargeChannelState);
				}
				else
				{
					boarSM.ChangeState(runAwayState);
				}
			}
		}
		else if (stateMachine.movement.IsValidNavmeshPath(Helper.GetClosestPointOnNavMesh(boarSM.enemyTarget.transform.position, 4f)))
		{
			Chase(boarSM.enemyTarget, boarSM.enemyTarget.transform.position);
		}
		else
		{
			boarSM.ChangeState(runAwayState);
		}
	}

	protected override bool IsInRangeOfPoint(Vector3 point)
	{
		if (base.IsInRangeOfPoint(point) && Helper.HasClearVision(boarSM.transform.position, point, stateMachine.movement.obstructionLayer))
		{
			return true;
		}
		return false;
	}
}
