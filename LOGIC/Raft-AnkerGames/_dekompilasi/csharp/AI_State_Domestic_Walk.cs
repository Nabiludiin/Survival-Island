using UnityEngine;

public class AI_State_Domestic_Walk : AI_State
{
	[SerializeField]
	private MovementType movementType = MovementType.NavmeshAgent;

	[SerializeField]
	private float targetMovementSpeed = 1f;

	[SerializeField]
	private Interval_Float switchDirectionInterval;

	[Tooltip("Can be null")]
	[SerializeField]
	private AI_State_Wait waitState;

	[Range(0f, 100f)]
	[SerializeField]
	private float waitChance;

	private Vector3 currentDirection = Vector3.zero;

	private float switchDirectionTimer;

	private float switchDirectionDelay;

	private Block closestFloorBlock;

	private bool closestFloorIsHalfFloor;

	private bool WaitStateNull()
	{
		return waitState != null;
	}

	public override void EnterState()
	{
		base.EnterState();
		stateMachine.movement.SetMovementType(movementType);
		if (Raft_Network.IsHost)
		{
			closestFloorBlock = null;
			switchDirectionTimer = 0f;
			switchDirectionDelay = switchDirectionInterval.GetRandomValue();
			currentDirection = GetRandomDirection();
		}
	}

	public override void UpdateState()
	{
		base.UpdateState();
		if (!Raft_Network.IsHost)
		{
			return;
		}
		stateMachine.movement.ChangeMovementSpeedTowards(targetMovementSpeed, 5f);
		if (stateMachine.movement.hasRaftGroundUnderneath)
		{
			closestFloorBlock = null;
			stateMachine.movement.MoveDirection_CharacterController(currentDirection, stateMachine.movement.MovementSpeed);
		}
		else
		{
			if (closestFloorBlock == null)
			{
				closestFloorBlock = BlockCreator.ClosestWalkableFloorToPosition(stateMachine.transform.position, excludeFloorsAbovePosition: true, ref closestFloorIsHalfFloor);
			}
			if (closestFloorBlock != null)
			{
				Vector3 vector = closestFloorBlock.transform.position;
				if (closestFloorIsHalfFloor)
				{
					vector = closestFloorBlock.transform.position + closestFloorBlock.transform.up * 1.21f;
				}
				currentDirection = vector - stateMachine.transform.position;
				stateMachine.movement.MoveDirection_CharacterController(currentDirection, stateMachine.movement.MovementSpeed);
			}
		}
		switchDirectionTimer += Time.deltaTime;
		if (switchDirectionTimer >= switchDirectionDelay)
		{
			switchDirectionTimer = 0f;
			switchDirectionDelay = switchDirectionInterval.GetRandomValue();
			currentDirection = GetRandomDirection();
			if (waitState != null && Random.Range(0f, 100f) < waitChance)
			{
				stateMachine.ChangeState(waitState);
			}
		}
	}

	private void OnDrawGizmos()
	{
		if (closestFloorBlock != null)
		{
			Vector3 vector = closestFloorBlock.transform.position;
			if (closestFloorIsHalfFloor)
			{
				vector = closestFloorBlock.transform.position + closestFloorBlock.transform.up * 1.21f;
			}
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(vector, 0.1f);
			Gizmos.DrawLine(stateMachine.transform.position, vector);
		}
	}

	private Vector3 GetRandomDirection()
	{
		Vector3 direction = new Vector3
		{
			x = Random.Range(-1f, 1f),
			z = Random.Range(-1f, 1f)
		};
		return stateMachine.transform.TransformDirection(direction);
	}
}
