using UnityEngine;

public class AI_State_Circulate_Shark : AI_State_CirculateSpawn_Water
{
	public AI_State donePatrollingState;

	public float checkForPlayersOriginCooldown = 0.1f;

	public float checkForPlayersCurrentProgress;

	public float circulationDuration = 3f;

	public float originAttackCooldown = 5f;

	public float currentAttackCooldown;

	public bool circulatesRaft = true;

	public WaterHeightState currentWaterHeightState = WaterHeightState.WaterHeight_NoTarget;

	private AI_StateMachine_Shark stateMachineShark;

	private float currentCirculationProgress;

	protected override void Start()
	{
		base.Start();
		stateMachineShark = stateMachine as AI_StateMachine_Shark;
	}

	public override void OnWorldShift(Vector3 shift)
	{
		base.OnWorldShift(shift);
	}

	public override void EnterState()
	{
		base.EnterState();
		currentAttackCooldown = 0f;
		if (stateMachineShark == null)
		{
			stateMachineShark = stateMachine as AI_StateMachine_Shark;
		}
		targetPoint = GetValidTargetPoint();
	}

	public override void UpdateState()
	{
		if (!Raft_Network.IsHost)
		{
			return;
		}
		Circulate();
		if (!GameModeValueManager.GetCurrentGameModeValue().sharkVariables.isTame)
		{
			checkForPlayersCurrentProgress += Time.deltaTime;
			if (checkForPlayersCurrentProgress > checkForPlayersOriginCooldown)
			{
				checkForPlayersCurrentProgress = 0f;
				stateMachineShark.targetToAttack = Helper.ClosestPlayerInWaterToPoint(stateMachine.transform.position, playerDistance, includeDeadPlayers: false);
			}
			currentAttackCooldown -= Time.deltaTime;
			if (currentAttackCooldown <= 0f && stateMachineShark.targetToAttack != null && nextState != null && Helper.IsValidWaterTarget(stateMachineShark.targetToAttack))
			{
				stateMachine.ChangeState<Network_Player>(nextState, stateMachineShark.targetToAttack);
			}
		}
	}

	public override void OnStateChange<T>(params T[] parameters)
	{
		base.OnStateChange(parameters);
	}

	protected override bool PlayerIsWithinRange()
	{
		if (GameModeValueManager.GetCurrentGameModeValue().sharkVariables.isTame)
		{
			return false;
		}
		return base.PlayerIsWithinRange();
	}

	protected override Vector3 GetValidTargetPoint()
	{
		if (circulatesRaft)
		{
			float num = ((stateMachineShark.raftBounds == null) ? 0f : stateMachineShark.raftBounds.HalfWidth);
			float num2 = ((stateMachineShark.raftBounds == null) ? 0f : stateMachineShark.raftBounds.HalfLength);
			Vector3 randomWaterPointInVision = stateMachineShark.GetRandomWaterPointInVision(20f + num, 40f + num2, stateMachineShark.lockedPivot);
			float waterPoint = stateMachineShark.waterPoint.GetWaterPoint(stateMachineShark.transform.position);
			waterPoint += (float)currentWaterHeightState;
			randomWaterPointInVision.y = waterPoint;
			return randomWaterPointInVision;
		}
		return base.GetValidTargetPoint();
	}
}
