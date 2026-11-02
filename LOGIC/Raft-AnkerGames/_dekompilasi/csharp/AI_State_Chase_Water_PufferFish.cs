public class AI_State_Chase_Water_PufferFish : AI_State_Chase_Water
{
	private AI_StateMachine_PufferFish stateMachinePufferFish;

	protected override void Awake()
	{
		base.Awake();
		stateMachinePufferFish = stateMachine as AI_StateMachine_PufferFish;
	}

	public override void EnterState()
	{
		base.EnterState();
		stateMachinePufferFish.animController.SetFloat("SwimSpeed", 1f);
	}

	public override void ExitState()
	{
		base.ExitState();
		stateMachinePufferFish.animController.SetFloat("SwimSpeed", 0f);
	}
}
