using UnityEngine;

public class AI_State_Chase : AI_State
{
	[SerializeField]
	private float distanceToChaseTarget = 2f;

	[SerializeField]
	private bool ignoreYDistance;

	[SerializeField]
	public float distanceFromOrigin = 10f;

	[SerializeField]
	private AI_State nextState;

	[SerializeField]
	protected AI_State fallbackState;

	[SerializeField]
	protected AI_State unreachableState;

	[SerializeField]
	private float targetMovementSpeed = 1f;

	[Tooltip("0 means no lerping is applied")]
	[SerializeField]
	private float targetMovementLerpSpeed;

	[SerializeField]
	private bool standStillIfBehind;

	[SerializeField]
	private bool ignoreZRotation;

	[SerializeField]
	private float targetRotationSpeed;

	[Tooltip("0 means no lerping is applied")]
	[SerializeField]
	private float targetRotationLerpSpeed;

	[SerializeField]
	private bool useVision;

	[SerializeField]
	protected MovementType movementType;

	[SerializeField]
	protected Network_Player target;

	protected Raft_Network network;

	private Vector3 startChaseOrigin;

	protected override void Start()
	{
		base.Start();
		network = ComponentManager<Raft_Network>.Value;
	}

	private void OnDrawGizmos()
	{
		if (!active || !Raft_Network.IsHost)
		{
			return;
		}
		if (target != null)
		{
			if (stateMachine.movement.MovementType == MovementType.NavmeshAgent)
			{
				Gizmos.color = Color.green;
				Gizmos.DrawSphere(stateMachine.movement.agent.pathEndPosition, 0.5f);
			}
			else
			{
				Gizmos.color = Color.green;
				Gizmos.DrawSphere(target.FeetPosition, 0.5f);
			}
			Gizmos.color = Color.white;
			Gizmos.DrawLine(stateMachine.transform.position, target.FeetPosition);
		}
		Gizmos.color = Color.Lerp(Color.green, Color.red, Helper.MapValue(0f, distanceFromOrigin, DistanceFromOrigin(stateMachine.transform.position), 0f, 1f));
		Gizmos.DrawSphere(startChaseOrigin, 0.5f);
		Gizmos.DrawLine(stateMachine.transform.position, startChaseOrigin);
		Gizmos.color = Color.red;
		Vector3 vector = startChaseOrigin + (stateMachine.transform.position - startChaseOrigin).normalized * distanceFromOrigin;
		Gizmos.DrawLine(vector, startChaseOrigin);
		Gizmos.DrawCube(vector, Vector3.one / 2f);
	}

	public override void UpdateState()
	{
		base.UpdateState();
		if (!Raft_Network.IsHost)
		{
			return;
		}
		Network_Player network_Player = FindNewTarget();
		if (!IsWithinOriginRange(stateMachine.transform.position))
		{
			stateMachine.ChangeState(fallbackState);
			return;
		}
		if (target == null || target != network_Player)
		{
			target = network_Player;
			return;
		}
		Vector3 vector = target.FeetPosition;
		if (stateMachine.movement.MovementType == MovementType.NavmeshAgent)
		{
			vector = Helper.GetClosestPointOnNavMesh(vector, 1f);
		}
		Chase(target, vector);
	}

	public override void EnterState()
	{
		base.EnterState();
		startChaseOrigin = stateMachine.transform.position;
		if (targetMovementLerpSpeed == 0f && targetMovementSpeed != 0f)
		{
			stateMachine.movement.SetMovementSpeed(targetMovementSpeed);
		}
		else
		{
			stateMachine.movement.SetMovementSpeed(0f);
		}
		if (targetRotationLerpSpeed == 0f && targetRotationSpeed != 0f)
		{
			stateMachine.movement.SetRotationSpeed(targetRotationSpeed);
		}
		else
		{
			stateMachine.movement.SetRotationSpeed(0f);
		}
		stateMachine.movement.SetMovementType(movementType);
		if (Raft_Network.IsHost)
		{
			target = FindNewTarget();
		}
	}

	public override void ExitState()
	{
		base.ExitState();
		stateMachine.movement.SetMovementType();
	}

	protected virtual void Chase(Network_Player targetPlayer, Vector3 chasePosition)
	{
		if (targetMovementLerpSpeed != 0f)
		{
			stateMachine.movement.ChangeMovementSpeedTowards(targetMovementSpeed, targetMovementLerpSpeed);
		}
		if (targetRotationLerpSpeed != 0f)
		{
			stateMachine.movement.ChangeRotationSpeedTowards(targetRotationSpeed, targetRotationLerpSpeed);
		}
		bool flag = false;
		if (standStillIfBehind)
		{
			Vector3 forward = stateMachine.movement.model.forward;
			Vector3 to = chasePosition - stateMachine.transform.position;
			forward.y = (to.y = 0f);
			if (Mathf.Abs(Vector3.Angle(forward, to)) > 45f)
			{
				switch (movementType)
				{
				case MovementType.Translate:
				case MovementType.NavmeshAgent:
				case MovementType.Rigidbody:
					stateMachine.movement.LookDirection(chasePosition - stateMachine.transform.position, stateMachine.movement.RotationSpeed, ignoreZRotation);
					break;
				case MovementType.Rigidbody_LookObstruction:
					stateMachine.movement.MoveLookObstruction_RigidBodyMove(stateMachine.transform, chasePosition, stateMachine.movement.MovementSpeed, stateMachine.movement.RotationSpeed, stateMachine.rigid);
					break;
				}
			}
			else
			{
				flag = true;
			}
		}
		else
		{
			flag = true;
		}
		if (flag)
		{
			switch (movementType)
			{
			case MovementType.Translate:
				stateMachine.movement.MoveLookObstruction_Translate(stateMachine.transform, chasePosition, stateMachine.movement.MovementSpeed, stateMachine.movement.RotationSpeed);
				break;
			case MovementType.NavmeshAgent:
				if (!stateMachine.movement.MoveLook_Navmesh(chasePosition, stateMachine.movement.MovementSpeed))
				{
					stateMachine.ChangeState(unreachableState);
					return;
				}
				break;
			case MovementType.Rigidbody_LookObstruction:
				stateMachine.movement.MoveLookObstruction_RigidBodyMove(stateMachine.transform, chasePosition, stateMachine.movement.MovementSpeed, stateMachine.movement.RotationSpeed, stateMachine.rigid);
				break;
			case MovementType.GoldenRule:
				stateMachine.movement.GoldenRuleMovement(chasePosition, stateMachine.rigid, stateMachine.movement.MovementSpeed);
				break;
			}
		}
		if (IsInRangeOfPoint(chasePosition))
		{
			if (!useVision)
			{
				stateMachine.ChangeState(nextState);
			}
			else if (Helper.HasVisionToPlayer(targetPlayer, transform.position, useFeetPosition: false))
			{
				stateMachine.ChangeState(nextState);
			}
		}
	}

	protected virtual Network_Player FindNewTarget()
	{
		if (network == null)
		{
			network = ComponentManager<Raft_Network>.Value;
		}
		Network_Player network_Player = Helper.ClosestPlayerToPoint(stateMachine.transform.position, includeDeadPlayers: false);
		if (network_Player != null)
		{
			return network_Player;
		}
		stateMachine.ChangeState(fallbackState);
		return null;
	}

	protected virtual bool IsInRangeOfPoint(Vector3 point)
	{
		if (ignoreYDistance)
		{
			point.y = stateMachine.transform.position.y;
		}
		if (Helper.WithinDistance(stateMachine.transform.position, point, distanceToChaseTarget))
		{
			return true;
		}
		return false;
	}

	private float DistanceFromOrigin(Vector3 pos)
	{
		return Vector3.Distance(pos, startChaseOrigin);
	}

	private bool IsWithinOriginRange(Vector3 pos)
	{
		return DistanceFromOrigin(pos) <= distanceFromOrigin;
	}
}
