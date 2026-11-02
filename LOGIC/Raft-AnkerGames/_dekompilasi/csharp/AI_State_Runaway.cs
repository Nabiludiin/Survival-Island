using UnityEngine;

public class AI_State_Runaway : AI_State
{
	[SerializeField]
	private AI_State reachTargetState;

	[SerializeField]
	private AI_State seePlayerState;

	[SerializeField]
	private float runawayMinDistance = 20f;

	[SerializeField]
	private float runawayMaxDistance = 35f;

	[SerializeField]
	private float targetPointDistance = 2f;

	[SerializeField]
	private float distanceToDetectPlayer;

	[SerializeField]
	private float movementSpeed = 6f;

	[SerializeField]
	private float navmeshSnapDistance = 3f;

	[SerializeField]
	private bool useDefaultRotationSpeed = true;

	[SerializeField]
	private float rotationSpeed = 3f;

	private Vector3 runToTargetPoint = Vector3.zero;

	private void OnDrawGizmos()
	{
		if (active)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(runToTargetPoint, 0.5f);
			Gizmos.color = Color.white;
			Gizmos.DrawLine(stateMachine.transform.position, runToTargetPoint);
		}
	}

	public override void EnterState()
	{
		base.EnterState();
		if (Raft_Network.IsHost)
		{
			Vector3 to = CalculateRunToPoint();
			runToTargetPoint = Helper.GetClosestPointOnNavMeshInDirectionTo(stateMachine.transform.position, to, 0f, 1f, navmeshSnapDistance);
		}
		if (stateMachine.movement != null)
		{
			stateMachine.movement.SetMovementType(MovementType.NavmeshAgent);
			stateMachine.movement.SetMovementSpeed(movementSpeed);
			if (useDefaultRotationSpeed)
			{
				stateMachine.movement.SetRotationSpeed(stateMachine.movement.defaultRotationSpeed);
			}
			else
			{
				stateMachine.movement.SetRotationSpeed(rotationSpeed);
			}
		}
	}

	public override void ExitState()
	{
		base.ExitState();
		if (stateMachine.movement != null)
		{
			stateMachine.movement.SetMovementType(MovementType.None);
			stateMachine.movement.SetMovementSpeed(stateMachine.movement.defaultMovementSpeed);
			stateMachine.movement.SetRotationSpeed(stateMachine.movement.defaultRotationSpeed);
		}
	}

	public override void UpdateState()
	{
		base.UpdateState();
		if (!Raft_Network.IsHost)
		{
			return;
		}
		if (runToTargetPoint == Vector3.zero)
		{
			stateMachine.ChangeState(reachTargetState);
		}
		else if (!stateMachine.movement.MoveLook_Navmesh(runToTargetPoint, stateMachine.movement.MovementSpeed))
		{
			runToTargetPoint = CalculateRunToPoint();
		}
		else if (TargetReached())
		{
			stateMachine.ChangeState(reachTargetState);
		}
		else
		{
			if (!(seePlayerState != null))
			{
				return;
			}
			Network_Player network_Player = Helper.ClosestPlayerToPoint(stateMachine.transform.position, distanceToDetectPlayer, includeDeadPlayers: false);
			if (!network_Player)
			{
				return;
			}
			switch (stateMachine.movement.MovementType)
			{
			case MovementType.NavmeshAgent:
				if (stateMachine.movement.agent.enabled && stateMachine.movement.IsValidNavmeshPath(network_Player))
				{
					stateMachine.ChangeState(seePlayerState);
				}
				break;
			case MovementType.Translate:
			case MovementType.CharacterController:
			case MovementType.None:
				stateMachine.ChangeState(seePlayerState);
				break;
			}
		}
	}

	protected virtual bool TargetReached()
	{
		return Vector3.Distance(stateMachine.transform.position, runToTargetPoint) < targetPointDistance;
	}

	private Vector3 CalculateRunToPoint()
	{
		Network_Player network_Player = Helper.ClosestPlayerToPoint(stateMachine.transform.position, includeDeadPlayers: false);
		if (network_Player != null)
		{
			Vector3 vector = stateMachine.transform.position - network_Player.transform.position;
			return GetRandomTargetPoint(stateMachine.transform.position + vector.normalized * Random.Range(runawayMinDistance, runawayMaxDistance), 5f);
		}
		return stateMachine.transform.position;
	}

	private Vector3 GetRandomTargetPoint(Vector3 center, float radius)
	{
		return Helper.GetClosestPointOnNavMesh(center + Random.insideUnitSphere * radius, radius + 5f);
	}
}
