using UnityEngine;

public class AI_State_Lift : AI_State
{
	[SerializeField]
	private AI_State nextState;

	[SerializeField]
	protected AI_Movement movement;

	[SerializeField]
	private float liftHeight = 3f;

	[SerializeField]
	private float liftSpeed = 5f;

	private Vector3 targetPoint = Vector3.zero;

	public override void EnterState()
	{
		base.EnterState();
		if (Raft_Network.IsHost)
		{
			targetPoint = transform.position + Vector3.up * liftHeight;
			movement.SetMovementSpeed(liftSpeed);
		}
	}

	public override void UpdateState()
	{
		base.UpdateState();
		if (Raft_Network.IsHost)
		{
			movement.Move_Translate(stateMachine.transform, targetPoint, movement.MovementSpeed);
			if (Vector3.Distance(stateMachine.transform.position, targetPoint) < 0.2f)
			{
				stateMachine.ChangeState(nextState);
			}
		}
	}
}
