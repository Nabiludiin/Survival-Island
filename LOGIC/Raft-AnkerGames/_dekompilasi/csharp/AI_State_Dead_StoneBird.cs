public class AI_State_Dead_StoneBird : AI_State_Dead_Ragdoll
{
	private AI_StateMachine_StoneBird stateMachineStoneBird;

	protected override void Awake()
	{
		base.Awake();
		stateMachineStoneBird = stateMachine as AI_StateMachine_StoneBird;
	}

	public override void EnterState()
	{
		base.EnterState();
		if (Raft_Network.IsHost && stateMachineStoneBird.dropStoneState.HasStone)
		{
			stateMachineStoneBird.dropStoneState.DropStone(transform.position);
		}
		stateMachineStoneBird.dropStoneState.SetStoneModelState(state: false);
		stateMachineStoneBird.GetComponent<AI_Movement_Lean>().enabled = false;
	}
}
