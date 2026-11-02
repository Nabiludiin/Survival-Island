using UnityEngine;

public class AI_State_MoveToPointOfInterest : AI_State
{
	public float distanceToReachPoint = 2f;

	public float maxDistanceValidPoint = 1000f;

	public MovementType movementType;

	public float movementSpeed;

	public AI_State nextState;

	[SerializeField]
	private ObjectOfInterestType interestType;

	private Transform targetPoint;

	private MovementType enterMovementType;

	public override void EnterState()
	{
		base.EnterState();
		targetPoint = FindObjectOfInterest(interestType);
		enterMovementType = stateMachine.movement.MovementType;
		stateMachine.movement.SetMovementType(movementType);
	}

	public override void UpdateState()
	{
		base.UpdateState();
		if (targetPoint != null)
		{
			if (Vector3.Distance(targetPoint.position, stateMachine.transform.position) > maxDistanceValidPoint)
			{
				stateMachine.ChangeState(nextState);
			}
			else if (!targetPoint.gameObject.activeInHierarchy || Vector3.Distance(stateMachine.movement.model.position, targetPoint.position) <= distanceToReachPoint)
			{
				stateMachine.ChangeState(nextState);
			}
			else
			{
				stateMachine.movement.MoveLook_Navmesh(targetPoint.position, movementSpeed);
			}
		}
		else
		{
			stateMachine.ChangeState(nextState);
		}
	}

	public override void ExitState()
	{
		base.ExitState();
		stateMachine.movement.SetMovementType(enterMovementType);
	}

	private Transform FindObjectOfInterest(ObjectOfInterestType type)
	{
		return ObjectManager.GetObjectOfInterest(type);
	}
}
