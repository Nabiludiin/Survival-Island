public class AI_State_Dive_Movetowards_Shark : AI_State_Movetowards_Destination
{
	private AI_StateMachine_Shark sharkStateMachine;

	public AI_State destinationState;

	public override void EnterState()
	{
		sharkStateMachine = stateMachine as AI_StateMachine_Shark;
		base.EnterState();
	}

	protected override void MoveTowardsDestinationPoint()
	{
		base.MoveTowardsDestinationPoint();
		sharkStateMachine.movement.GoldenRuleMovement(destinationPoint, sharkStateMachine.rigid, stateMachine.movement.MovementSpeed);
	}

	public override void UpdateState()
	{
		base.UpdateState();
		MoveTowardsDestinationPoint();
		if (IsInRangeOfDestinationPoint())
		{
			sharkStateMachine.ChangeState(destinationState);
		}
	}

	public void SetDestinationState(AI_State state)
	{
		destinationState = state;
	}
}
