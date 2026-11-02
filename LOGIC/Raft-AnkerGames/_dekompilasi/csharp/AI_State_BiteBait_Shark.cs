public class AI_State_BiteBait_Shark : AI_State_BiteBait
{
	private AI_StateMachine_Shark statemachineShark;

	protected override void Start()
	{
		base.Start();
		statemachineShark = stateMachine as AI_StateMachine_Shark;
	}

	public override void ExitState()
	{
		base.ExitState();
	}
}
