using UnityEngine;

public class AI_State_CirculateRaftBounds : AI_State
{
	[SerializeField]
	private MovementType movementType;

	[SerializeField]
	private bool targetPointFollowRaft;

	[SerializeField]
	private float targetMovementSpeed = 1f;

	[SerializeField]
	private Interval_Float posYInterval;

	[SerializeField]
	private float posYRoughWeatherOffset = -3f;

	[SerializeField]
	private Vector3 minBoundsSize = new Vector3(20f, 0f, 20f);

	[SerializeField]
	[Range(0f, 1f)]
	private float innerBoundsModifier;

	[SerializeField]
	[Range(1f, 10f)]
	private float outerBoundsModifier;

	[SerializeField]
	[Range(1f, 10f)]
	private float boundsMargin = 1f;

	[SerializeField]
	private AI_State reachPointState;

	[SerializeField]
	private float reachPointDistance = 1f;

	[SerializeField]
	private float newPointMaxAngle = 30f;

	[SerializeField]
	private uint attempstToFindPoint = 1u;

	private Vector3 targetPoint;

	private static RaftBounds bounds;

	public override void EnterState()
	{
		base.EnterState();
		stateMachine.movement.SetMovementType(movementType);
		if (Raft_Network.IsHost)
		{
			targetPoint = GetValidTargetPoint();
		}
	}

	public override void UpdateState()
	{
		base.UpdateState();
		if (!Raft_Network.IsHost)
		{
			return;
		}
		if (bounds == null)
		{
			bounds = ComponentManager<RaftBounds>.Value;
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
		Vector3 vector = (targetPointFollowRaft ? bounds.transform.parent.TransformPoint(targetPoint) : targetPoint);
		switch (movementType)
		{
		case MovementType.Translate:
			stateMachine.movement.MoveLookObstruction_Translate(stateMachine.transform, vector, stateMachine.movement.MovementSpeed, stateMachine.movement.RotationSpeed);
			break;
		case MovementType.Translate_Simple:
			stateMachine.movement.MoveLook_Translate(stateMachine.transform, vector, stateMachine.movement.MovementSpeed, stateMachine.movement.RotationSpeed);
			break;
		case MovementType.Rigidbody_LookObstruction:
			stateMachine.movement.MoveLookObstruction_RigidBodyMove(stateMachine.transform, vector, stateMachine.movement.MovementSpeed, stateMachine.movement.RotationSpeed, stateMachine.rigid);
			break;
		case MovementType.GoldenRule:
			stateMachine.movement.GoldenRuleMovement(vector, stateMachine.rigid, stateMachine.movement.MovementSpeed);
			break;
		}
		if (Vector3.Distance(stateMachine.transform.position, vector) < reachPointDistance)
		{
			if (reachPointState != null)
			{
				stateMachine.ChangeState(reachPointState);
			}
			else
			{
				targetPoint = GetValidTargetPoint();
			}
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (!active)
		{
			return;
		}
		Vector3 vector = (targetPointFollowRaft ? bounds.transform.parent.TransformPoint(targetPoint) : targetPoint);
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(vector, 0.5f);
		Gizmos.color = Color.white;
		Gizmos.DrawLine(stateMachine.transform.position, vector);
		if (bounds != null)
		{
			Vector3 vector2 = new Vector3(bounds.Width, 0f, bounds.Length);
			if (vector2.x < minBoundsSize.x)
			{
				vector2.x = minBoundsSize.x;
			}
			if (vector2.z < minBoundsSize.z)
			{
				vector2.z = minBoundsSize.z;
			}
			vector2 *= boundsMargin;
			Vector3 vector3 = vector2 * innerBoundsModifier;
			Vector3 vector4 = vector2 * outerBoundsModifier;
			Gizmos.color = Color.white;
			DrawBounds(bounds.transform.position, bounds.RightDir, bounds.ForwardDir, vector2);
			if (innerBoundsModifier < 1f)
			{
				Gizmos.color = Color.red;
				DrawBounds(bounds.transform.position, bounds.RightDir, bounds.ForwardDir, vector3);
			}
			if (outerBoundsModifier > 1f)
			{
				Gizmos.color = Color.red;
				DrawBounds(bounds.transform.position, bounds.RightDir, bounds.ForwardDir, vector4);
			}
		}
		static void DrawBounds(Vector3 center, Vector3 rightDir, Vector3 forwardDir, Vector3 bounds)
		{
			Gizmos.DrawLine(center + rightDir * (bounds.x / 2f) + forwardDir * (bounds.z / 2f), center + rightDir * (bounds.x / 2f) - forwardDir * (bounds.z / 2f));
			Gizmos.DrawLine(center + rightDir * (bounds.x / 2f) - forwardDir * (bounds.z / 2f), center - rightDir * (bounds.x / 2f) - forwardDir * (bounds.z / 2f));
			Gizmos.DrawLine(center - rightDir * (bounds.x / 2f) - forwardDir * (bounds.z / 2f), center - rightDir * (bounds.x / 2f) + forwardDir * (bounds.z / 2f));
			Gizmos.DrawLine(center - rightDir * (bounds.x / 2f) + forwardDir * (bounds.z / 2f), center + rightDir * (bounds.x / 2f) + forwardDir * (bounds.z / 2f));
		}
	}

	public override void OnWorldShift(Vector3 shift)
	{
		base.OnWorldShift(shift);
		if (!targetPointFollowRaft)
		{
			targetPoint -= shift;
		}
	}

	private Vector3 GetValidTargetPoint()
	{
		if (bounds == null)
		{
			bounds = ComponentManager<RaftBounds>.Value;
		}
		if (bounds == null)
		{
			return stateMachine.transform.position;
		}
		Vector3 pos = Vector3.zero;
		float allowedAngle = newPointMaxAngle;
		Vector3 currentBounds = new Vector3(bounds.Width, 0f, bounds.Length);
		if (currentBounds.x < minBoundsSize.x)
		{
			currentBounds.x = minBoundsSize.x;
		}
		if (currentBounds.z < minBoundsSize.z)
		{
			currentBounds.z = minBoundsSize.z;
		}
		currentBounds *= boundsMargin;
		Vector3 innerBounds = currentBounds * innerBoundsModifier;
		Vector3 outerBounds = currentBounds * outerBoundsModifier;
		SetNewPosInFrontOfStateMachine();
		int i;
		for (i = 0; i < attempstToFindPoint; i++)
		{
			bool flag = false;
			if (Helper.IsPointInOABB_XZOnly(pos, bounds.transform, outerBounds, Vector3.zero))
			{
				if (innerBoundsModifier > 0f)
				{
					if (!Helper.IsPointInOABB_XZOnly(pos, bounds.transform, innerBounds, Vector3.zero))
					{
						flag = true;
					}
				}
				else
				{
					flag = true;
				}
			}
			if (flag)
			{
				break;
			}
			allowedAngle *= 1.2f;
			SetNewPosInFrontOfStateMachine();
		}
		if (i >= attempstToFindPoint)
		{
			SetNewPosWithinBounds();
		}
		if (WaterRoughnessManager.RoughForAnimals)
		{
			pos.y += posYRoughWeatherOffset;
		}
		if (targetPointFollowRaft)
		{
			pos = bounds.transform.parent.InverseTransformPoint(pos);
		}
		return pos;
		void SetNewPosInFrontOfStateMachine()
		{
			Vector3 direction = Quaternion.AngleAxis(Random.Range(0f - allowedAngle, allowedAngle), Vector3.up) * Vector3.forward;
			Vector3 vector = stateMachine.transform.TransformDirection(direction);
			pos = stateMachine.transform.position + vector * (reachPointDistance * 2f);
			pos.y = posYInterval.GetRandomValue();
		}
		void SetNewPosWithinBounds()
		{
			if (innerBoundsModifier > 0f)
			{
				pos = bounds.transform.position + bounds.RightDir * Random.Range(innerBounds.x / 2f, outerBounds.x / 2f) * Helper.RandomOneMinus() + bounds.ForwardDir * Random.Range(innerBounds.z / 2f, outerBounds.z / 2f) * Helper.RandomOneMinus();
				pos.y = posYInterval.GetRandomValue();
			}
			else
			{
				pos = bounds.transform.position + bounds.RightDir * Random.Range((0f - currentBounds.x) / 2f, outerBounds.x / 2f) + bounds.ForwardDir * Random.Range((0f - currentBounds.z) / 2f, outerBounds.z / 2f);
				pos.y = posYInterval.GetRandomValue();
			}
		}
	}
}
