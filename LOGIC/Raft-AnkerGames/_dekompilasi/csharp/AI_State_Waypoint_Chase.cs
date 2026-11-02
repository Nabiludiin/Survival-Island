using UnityEngine;

public class AI_State_Waypoint_Chase : AI_State
{
	public Network_Player chaseTarget;

	public float visionRange = 25f;

	public LayerMask obstructionMask;

	public float distanceRequiredToReach = 2f;

	public AI_State StateReachWaypoint;

	public AI_State StateReachTarget;

	public AI_State StateGiveUpChase;

	protected Vector3 lastKnownPlayerPosition;

	protected bool hasVisionToTarget;

	public override void EnterState()
	{
		base.EnterState();
	}

	public override void UpdateState()
	{
		if (Raft_Network.IsHost)
		{
			base.UpdateState();
			if (Helper.IsValidWaterTarget(chaseTarget))
			{
				AttemptChase();
			}
			else
			{
				chaseTarget = Helper.ClosestPlayerInWaterToPoint(transform.position, 100f, includeDeadPlayers: false);
			}
			if (chaseTarget == null)
			{
				stateMachine.ChangeState(StateGiveUpChase);
			}
			else
			{
				CheckDistanceConditions();
			}
		}
	}

	public override void OnStateChange<T>(params T[] parameters)
	{
		base.OnStateChange(parameters);
		if (parameters[0] is Network_Player)
		{
			chaseTarget = parameters[0] as Network_Player;
		}
	}

	private void CheckDistanceConditions()
	{
		if (!(chaseTarget == null))
		{
			if (Vector3.Distance(stateMachine.transform.position, chaseTarget.transform.position) <= distanceRequiredToReach)
			{
				stateMachine.ChangeState<Network_Player>(StateReachTarget, chaseTarget);
			}
			else if (StateReachWaypoint != null && Vector3.Distance(stateMachine.transform.position, lastKnownPlayerPosition) <= distanceRequiredToReach)
			{
				stateMachine.ChangeState(StateReachWaypoint);
			}
		}
	}

	private void AttemptChase()
	{
		hasVisionToTarget = Helper.HasClearVisionAndWithinRange(transform.position, chaseTarget.transform.position, obstructionMask, visionRange);
		if (hasVisionToTarget)
		{
			lastKnownPlayerPosition = chaseTarget.transform.position;
			MoveTowardsWaypoint(chaseTarget.transform.position);
		}
		else
		{
			MoveTowardsWaypoint(lastKnownPlayerPosition);
		}
	}

	private void MoveTowardsWaypoint(Vector3 wayPointPos)
	{
		if (stateMachine.movement.MovementType == MovementType.Rigidbody_LookObstruction)
		{
			stateMachine.movement.MoveLookObstruction_RigidBodyMove(stateMachine.transform, wayPointPos, stateMachine.movement.MovementSpeed, stateMachine.movement.RotationSpeed, stateMachine.rigid);
		}
		else if (stateMachine.movement.MovementType == MovementType.GoldenRule)
		{
			stateMachine.movement.GoldenRuleMovement(wayPointPos, stateMachine.rigid, stateMachine.movement.MovementSpeed);
		}
		if (Vector3.Distance(stateMachine.transform.position, wayPointPos) <= distanceRequiredToReach)
		{
			stateMachine.ChangeState(StateReachWaypoint);
		}
	}
}
