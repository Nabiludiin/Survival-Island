using UnityEngine;

public class AI_State_MoveDirection : AI_State
{
	[SerializeField]
	private MovementType movementType;

	[SerializeField]
	private Vector3 direction;

	[SerializeField]
	private bool localDirection;

	[SerializeField]
	private float targetMovementSpeed = 5f;

	[SerializeField]
	private AI_State nextState;

	[SerializeField]
	[Tooltip("0 = infinity")]
	private float moveTime;

	private float timer;

	private Vector3 moveDirection;

	public Vector3 TargetPoint => stateMachine.transform.position + moveDirection * 10f;

	public override void UpdateState()
	{
		base.UpdateState();
		if (!Raft_Network.IsHost)
		{
			return;
		}
		MoveInDirection();
		if (nextState != null)
		{
			timer -= Time.deltaTime;
			if (timer <= 0f)
			{
				stateMachine.ChangeState(nextState);
			}
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (active && Application.isPlaying)
		{
			Gizmos.DrawLine(transform.position, TargetPoint);
		}
	}

	public override void EnterState()
	{
		base.EnterState();
		timer = moveTime;
		moveDirection = (localDirection ? stateMachine.transform.TransformDirection(direction) : direction);
		stateMachine.movement.SetMovementType(movementType);
	}

	private void MoveInDirection()
	{
		stateMachine.movement.ChangeMovementSpeedTowards(targetMovementSpeed, 5f);
		if (movementType == MovementType.Translate_Simple)
		{
			stateMachine.movement.MoveLook_Translate(stateMachine.transform, TargetPoint, stateMachine.movement.MovementSpeed, stateMachine.movement.RotationSpeed);
		}
	}
}
