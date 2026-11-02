using UnityEngine;

public class AI_State_Lift_StoneBird : AI_State_Lift
{
	[SerializeField]
	private AI_State_StoneBird_DropStone dropStoneState;

	private AI_StateMachine_StoneBird stateMachineStoneBird;

	protected override void Awake()
	{
		base.Awake();
		stateMachineStoneBird = stateMachine as AI_StateMachine_StoneBird;
	}

	public override void UpdateState()
	{
		base.UpdateState();
		if (Raft_Network.IsHost && stateMachineStoneBird.target != null)
		{
			movement.Look(stateMachine.transform, stateMachineStoneBird.target.transform.position + Vector3.up * dropStoneState.altitudeAbovePlayer, movement.RotationSpeed);
		}
	}
}
