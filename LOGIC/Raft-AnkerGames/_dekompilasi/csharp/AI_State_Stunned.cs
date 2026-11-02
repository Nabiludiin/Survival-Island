using UnityEngine;

public class AI_State_Stunned : AI_State
{
	[SerializeField]
	private AI_State unStunnedState;

	[SerializeField]
	private float stunDuration = 2f;

	private float stunChannel;

	private MovementType prevMovementType;

	public override void EnterState()
	{
		base.EnterState();
		stateMachine.movement.ChangeMovementSpeedTowards(0f, 1f);
		prevMovementType = stateMachine.movement.MovementType;
		stateMachine.movement.SetMovementType(MovementType.None);
	}

	public override void ExitState()
	{
		stateMachine.movement.SetMovementType(prevMovementType);
		base.ExitState();
	}

	public override void UpdateState()
	{
		base.UpdateState();
		if (Raft_Network.IsHost)
		{
			stunChannel += Time.deltaTime;
			if (stunChannel >= stunDuration)
			{
				ResetState();
				stateMachine.ChangeState(unStunnedState);
			}
		}
	}

	private void ResetState()
	{
		stunChannel = 0f;
	}
}
