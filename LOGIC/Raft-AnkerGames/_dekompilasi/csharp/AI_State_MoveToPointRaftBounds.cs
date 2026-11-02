using UnityEngine;

public class AI_State_MoveToPointRaftBounds : AI_State
{
	[SerializeField]
	private MovementType movementType;

	[SerializeField]
	private AI_State nextState;

	[SerializeField]
	private float reachPointDistance = 1f;

	[SerializeField]
	[Range(-1f, 1f)]
	private float xOffset;

	[SerializeField]
	[Range(-1f, 1f)]
	private float zOffset;

	[SerializeField]
	private float yOffset;

	[SerializeField]
	private float targetMovementSpeed = 5f;

	private static RaftBounds bounds;

	private Vector3 targetPoint = Vector3.zero;

	public override void UpdateState()
	{
		base.UpdateState();
		if (Raft_Network.IsHost)
		{
			MoveTowardsPoint();
			if (nextState != null && Vector3.Distance(stateMachine.transform.position, targetPoint) < reachPointDistance)
			{
				stateMachine.ChangeState(nextState);
			}
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (Application.isPlaying && active)
		{
			Gizmos.DrawLine(transform.position, targetPoint);
			Gizmos.DrawSphere(targetPoint, 0.2f);
		}
	}

	public override void EnterState()
	{
		base.EnterState();
		stateMachine.movement.SetMovementType(movementType);
	}

	public override void ExitState()
	{
		base.ExitState();
		stateMachine.movement.SetMovementType(MovementType.None);
	}

	private void MoveTowardsPoint()
	{
		if (bounds == null)
		{
			bounds = ComponentManager<RaftBounds>.Value;
		}
		if (!(bounds == null))
		{
			targetPoint = bounds.Center + bounds.RightDir * bounds.HalfWidth * xOffset + bounds.ForwardDir * bounds.HalfLength * zOffset + Vector3.up * yOffset;
			stateMachine.movement.ChangeMovementSpeedTowards(targetMovementSpeed, 5f);
			if (movementType == MovementType.Translate_Simple)
			{
				stateMachine.movement.MoveLook_Translate(stateMachine.transform, targetPoint, stateMachine.movement.MovementSpeed, stateMachine.movement.RotationSpeed);
			}
		}
	}
}
