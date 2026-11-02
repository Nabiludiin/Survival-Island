public class AI_State_MamaBear_EatLure : AI_State
{
	private AI_StateMachine_MamaBear stateMachine_MamaBear;

	public override void EnterState()
	{
		base.EnterState();
		stateMachine_MamaBear = stateMachine as AI_StateMachine_MamaBear;
		stateMachine.movement.SetMovementType(MovementType.None);
	}

	public override void UpdateState()
	{
		base.UpdateState();
		stateMachine_MamaBear.movement.model.LookAt(stateMachine_MamaBear.lureTransform);
	}
}
