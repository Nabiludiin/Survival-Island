using System.Collections.Generic;
using UnityEngine;

public class AI_State_CirculateSpawn : AI_State
{
	[SerializeField]
	protected AI_State nextState;

	[SerializeField]
	protected float playerDistance = 1f;

	[SerializeField]
	private bool useVisionToPlayer;

	[SerializeField]
	private AI_State reachedPointState;

	[SerializeField]
	protected float pointDistance = 0.5f;

	[SerializeField]
	private bool findNewTargetPointAtStart;

	[SerializeField]
	protected Transform spawnPoint;

	[SerializeField]
	protected float circulationRadiusMin;

	[SerializeField]
	protected float circulationRadius = 1f;

	[SerializeField]
	protected float heightDifference = 15f;

	[SerializeField]
	protected float targetMovementSpeed = 1f;

	[SerializeField]
	protected int attemptsToFindPoint = 15;

	[SerializeField]
	protected MovementType movementType;

	protected Dictionary<Network_UserId, Network_Player> users;

	protected Vector3 targetPoint;

	private Vector3 startPosition;

	public Vector3 SpawnPointPosition
	{
		get
		{
			if (spawnPoint == null)
			{
				return startPosition;
			}
			return spawnPoint.position;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		startPosition = transform.position;
	}

	protected override void Start()
	{
		base.Start();
		if (ComponentManager<Raft_Network>.Value != null)
		{
			users = ComponentManager<Raft_Network>.Value.remoteUsers;
		}
		if (stateMachine != null)
		{
			AI_NetworkBehaviour component = stateMachine.GetComponent<AI_NetworkBehaviour>();
			if (component != null && component.connectedSpawner != null)
			{
				startPosition = component.connectedSpawner.transform.position;
			}
		}
		if (spawnPoint == null)
		{
			AI_NetworkBehaviour component2 = stateMachine.GetComponent<AI_NetworkBehaviour>();
			if (component2 != null && component2.connectedSpawner != null && component2.connectedSpawner.referencePoint != null)
			{
				spawnPoint = component2.connectedSpawner.referencePoint;
			}
		}
	}

	public override void UpdateState()
	{
		if (Raft_Network.IsHost)
		{
			Circulate();
			if (nextState != null && PlayerIsWithinRange())
			{
				stateMachine.ChangeState(nextState);
			}
		}
	}

	public override void EnterState()
	{
		base.EnterState();
		stateMachine.movement.SetMovementType(movementType);
		if (Raft_Network.IsHost && (findNewTargetPointAtStart || (stateMachine.movement.agent != null && stateMachine.movement.agent.enabled)))
		{
			targetPoint = GetValidTargetPoint();
		}
	}

	public override void OnWorldShift(Vector3 shift)
	{
		base.OnWorldShift(shift);
		if (spawnPoint == null)
		{
			startPosition -= shift;
			targetPoint -= shift;
		}
	}

	public void SetNewSpawnPoint(Vector3 newSpawnPoint)
	{
		startPosition = newSpawnPoint;
	}

	protected void Circulate()
	{
		if (movementType == MovementType.NavmeshAgent && (stateMachine == null || stateMachine.movement == null || stateMachine.movement.agent == null || !stateMachine.movement.agent.enabled || !stateMachine.movement.agent.isOnNavMesh))
		{
			return;
		}
		if (targetPoint == Vector3.zero)
		{
			targetPoint = GetValidTargetPoint();
			if (targetPoint == Vector3.zero)
			{
				return;
			}
		}
		stateMachine.movement.ChangeMovementSpeedTowards(targetMovementSpeed, 5f);
		if (movementType == MovementType.NavmeshAgent)
		{
			if (!stateMachine.movement.MoveLook_Navmesh(targetPoint, stateMachine.movement.MovementSpeed))
			{
				targetPoint = GetValidTargetPoint();
				return;
			}
		}
		else if (movementType == MovementType.Translate)
		{
			stateMachine.movement.MoveLookObstruction_Translate(stateMachine.transform, targetPoint, stateMachine.movement.MovementSpeed, stateMachine.movement.RotationSpeed);
		}
		else if (movementType == MovementType.Translate_Simple)
		{
			stateMachine.movement.MoveLook_Translate(stateMachine.transform, targetPoint, stateMachine.movement.MovementSpeed, stateMachine.movement.RotationSpeed);
		}
		else if (movementType == MovementType.Rigidbody_LookObstruction)
		{
			stateMachine.movement.MoveLookObstruction_RigidBodyMove(stateMachine.transform, targetPoint, stateMachine.movement.MovementSpeed, stateMachine.movement.RotationSpeed, stateMachine.rigid);
		}
		else if (movementType == MovementType.GoldenRule)
		{
			stateMachine.movement.GoldenRuleMovement(targetPoint, stateMachine.rigid, stateMachine.movement.MovementSpeed);
		}
		if (Vector3.Distance(stateMachine.transform.position, targetPoint) < pointDistance)
		{
			if (reachedPointState != null)
			{
				stateMachine.ChangeState(reachedPointState);
			}
			else
			{
				targetPoint = GetValidTargetPoint();
			}
		}
	}

	protected virtual bool PlayerIsWithinRange()
	{
		if (users != null)
		{
			foreach (Network_Player value in users.Values)
			{
				if (value == null || value.PlayerScript.IsDead)
				{
					continue;
				}
				float num = Vector3.Distance(stateMachine.transform.position, value.transform.position);
				if (num <= playerDistance && (stateMachine.movement.MovementType != MovementType.NavmeshAgent || stateMachine.movement.IsValidNavmeshPath(value)))
				{
					if (useVisionToPlayer)
					{
						return Helper.HasClearVision(transform.position, value.transform.position, LayerMasks.MASK_Obstruction, num - 0.5f);
					}
					return true;
				}
			}
		}
		return false;
	}

	protected virtual Vector3 GetValidTargetPoint()
	{
		Vector3 randomTargetPoint = GetRandomTargetPoint();
		int num = 0;
		while (num < attemptsToFindPoint)
		{
			num++;
			if (Mathf.Abs(stateMachine.transform.position.y - randomTargetPoint.y) <= heightDifference)
			{
				switch (movementType)
				{
				case MovementType.NavmeshAgent:
					if (stateMachine.movement.IsValidNavmeshPath(randomTargetPoint))
					{
						return randomTargetPoint;
					}
					break;
				case MovementType.Translate:
				{
					Vector3 vector = randomTargetPoint - stateMachine.transform.position;
					if (Helper.HasClearVision(stateMachine.transform.position, randomTargetPoint, stateMachine.movement.obstructionLayer, vector.magnitude * 0.9f))
					{
						return randomTargetPoint;
					}
					break;
				}
				}
			}
			randomTargetPoint = GetRandomTargetPoint();
		}
		return randomTargetPoint;
	}

	protected float GetCirculationRadius()
	{
		return Random.Range(circulationRadiusMin, circulationRadius);
	}

	private Vector3 GetRandomTargetPoint()
	{
		Vector3 vector = SpawnPointPosition + Random.insideUnitSphere * GetCirculationRadius();
		vector.y = Mathf.Clamp(vector.y, stateMachine.transform.position.y - heightDifference, stateMachine.transform.position.y + heightDifference);
		if (movementType == MovementType.NavmeshAgent)
		{
			vector = Helper.GetClosestPointOnNavMesh(vector, circulationRadius + 5f);
		}
		return vector;
	}

	private bool HasDetectPlayerState()
	{
		return nextState != null;
	}
}
