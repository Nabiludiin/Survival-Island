public class AI_State_MamaBear_GuardCave : AI_State
{
	public override void EnterState()
	{
		base.EnterState();
		stateMachine.movement.SetMovementType(MovementType.None);
	}
}
