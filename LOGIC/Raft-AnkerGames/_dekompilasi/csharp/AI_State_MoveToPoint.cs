using UnityEngine;

public class AI_State_MoveToPoint : AI_State
{
	[HideInInspector]
	public Vector3 targetPoint;

	[SerializeField]
	protected AI_State reachPointState;

	[SerializeField]
	private MovementType movementType;

	[SerializeField]
	private float movementSpeed = 1f;

	[SerializeField]
	private float reachPointDistance = 1f;

	public override void OnStateChange<T>(params T[] parameters)
	{
		base.OnStateChange(parameters);
		if (parameters.Length != 0)
		{
			targetPoint = (Vector3)(object)parameters[0];
		}
	}

	public override void EnterState()
	{
		base.EnterState();
		stateMachine.movement.SetMovementType(movementType);
	}

	public override void UpdateState()
	{
		base.UpdateState();
		if (Raft_Network.IsHost && MoveToPoint(targetPoint) && Vector3.Distance(stateMachine.movement.model.transform.position, targetPoint) < reachPointDistance)
		{
			ReachedPoint();
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (Application.isPlaying)
		{
			Gizmos.DrawLine(stateMachine.movement.model.position, targetPoint);
			Gizmos.DrawSphere(targetPoint, 0.05f);
		}
	}

	protected virtual bool MoveToPoint(Vector3 targetPoint)
	{
		if (Vector3.Distance(stateMachine.transform.position, targetPoint) > reachPointDistance)
		{
			switch (movementType)
			{
			case MovementType.Translate:
			case MovementType.CharacterController:
			case MovementType.Rigidbody_LookObstruction:
				Debug.Log("AI_State_MoveToPoint: Movement code for " + movementType.ToString() + " is not coded yet", gameObject);
				break;
			case MovementType.NavmeshAgent:
				return stateMachine.movement.MoveLook_Navmesh(targetPoint, movementSpeed);
			}
			return false;
		}
		return true;
	}

	protected virtual void ReachedPoint()
	{
		stateMachine.ChangeState(reachPointState);
	}
}
