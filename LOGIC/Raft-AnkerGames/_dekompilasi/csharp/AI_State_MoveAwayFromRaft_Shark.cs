using UnityEngine;

public class AI_State_MoveAwayFromRaft_Shark : AI_State_Movetowards_Destination
{
	private AI_StateMachine_Shark stateMachineShark;

	public AI_State pointReachedState;

	public float maxStateDuration = 5f;

	private float maxStateProgression;

	public float backOffRange = 5f;

	protected override void Start()
	{
		stateMachineShark = stateMachine as AI_StateMachine_Shark;
		base.Start();
	}

	public override void EnterState()
	{
		base.EnterState();
		maxStateProgression = 0f;
		destinationPoint = -stateMachineShark.transform.forward * backOffRange;
	}

	public override void ExitState()
	{
		base.ExitState();
	}

	public override void UpdateState()
	{
		base.UpdateState();
	}

	protected override void MoveTowardsDestinationPoint()
	{
		base.MoveTowardsDestinationPoint();
		maxStateProgression += Time.deltaTime;
		stateMachine.movement.GoldenRuleMovement(destinationPoint, stateMachineShark.rigid, stateMachine.movement.MovementSpeed);
		if (maxStateProgression >= maxStateDuration || IsInRangeOfDestinationPoint())
		{
			stateMachineShark.ChangeState(pointReachedState);
		}
	}
}
