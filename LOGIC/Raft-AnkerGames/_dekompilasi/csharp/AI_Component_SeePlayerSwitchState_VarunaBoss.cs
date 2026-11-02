public class AI_Component_SeePlayerSwitchState_VarunaBoss : AI_Component_SeePlayerSwitchState
{
	private AI_StateMachine_Boss_Varuna stateMachine;

	protected override void OnSeePlayerSwitchState(Network_Player playerReference)
	{
		if (stateMachine == null)
		{
			stateMachine = behaviour.GetComponent<AI_StateMachine_Boss_Varuna>();
		}
		if (stateMachine.CanChargePlayer(playerReference) && stateMachine.PlayerIsInBossTargetingArea(playerReference))
		{
			base.OnSeePlayerSwitchState(playerReference);
		}
	}
}
