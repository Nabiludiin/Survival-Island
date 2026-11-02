using System;
using UnityEngine;

public class AI_State_AirLand : AI_State
{
	public Action OnStartLanding;

	public Action OnFinishedLanding;

	[SerializeField]
	private AI_State nextState;

	[SerializeField]
	private AI_Movement movement;

	[HideInInspector]
	public Vector3 landPosition = Vector3.zero;

	[Tooltip("Distance away from landpos to initiate landing")]
	[SerializeField]
	private float startLandDistance = 3f;

	[Tooltip("Slow down when reaching land distance")]
	[SerializeField]
	private bool breakWhenLanding;

	[SerializeField]
	private float breakSpeed = 3f;

	[SerializeField]
	private Interval_Float landPosDistance;

	[Tooltip("1f is ground up and 0f is straight of a wall")]
	[SerializeField]
	private float minValidAngle = 0.5f;

	[Tooltip("How fast to rotate to horizon direction when landing")]
	[SerializeField]
	private float landRotationSpeed = 3f;

	[SerializeField]
	private float landMovementSpeed = 25f;

	public bool landing;

	public bool fullyLanded;

	private void OnDrawGizmos()
	{
		if (active)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(landPosition, 0.5f);
			Gizmos.DrawLine(stateMachine.transform.position, landPosition);
		}
	}

	public override void EnterState()
	{
		base.EnterState();
		landing = false;
		fullyLanded = false;
	}

	public override void UpdateState()
	{
		base.UpdateState();
		if (!Raft_Network.IsHost)
		{
			return;
		}
		float num = Vector3.Distance(stateMachine.transform.position, landPosition);
		bool flag = num <= startLandDistance;
		if (flag && !landing && OnStartLanding != null)
		{
			OnStartLanding();
		}
		landing = flag;
		if (landing)
		{
			Vector3 direction = landPosition - stateMachine.transform.position;
			direction.y = 0f;
			movement.Move_Translate(stateMachine.transform, landPosition, movement.MovementSpeed);
			movement.LookDirection(stateMachine.transform, direction, landRotationSpeed);
			if (num <= 0.1f)
			{
				fullyLanded = true;
				movement.SetMovementSpeed(0f);
				stateMachine.transform.position = landPosition;
				stateMachine.ChangeState(nextState);
				if (OnFinishedLanding != null)
				{
					OnFinishedLanding();
				}
			}
			else if (breakWhenLanding)
			{
				movement.ChangeMovementSpeedTowards(0.2f, breakSpeed);
			}
		}
		else
		{
			movement.ChangeMovementSpeedTowards(landMovementSpeed, 10f);
			movement.MoveLookObstruction_Translate(stateMachine.transform, landPosition, movement.MovementSpeed, movement.RotationSpeed);
		}
	}

	public virtual bool SetRandomLandPosition()
	{
		int num = 0;
		while (num < 200)
		{
			num++;
			Vector3 vector = stateMachine.transform.position + UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(landPosDistance.minValue + 1f, landPosDistance.maxValue);
			Vector3 vector2 = new Vector3(vector.x, stateMachine.transform.position.y, vector.z);
			if (!Physics.Raycast(vector2, Vector3.down, out var hitInfo, 1000f, movement.obstructionLayer, QueryTriggerInteraction.Ignore))
			{
				continue;
			}
			Debug.DrawRay(vector2, Vector3.down * 1000f, Color.magenta, 5f);
			float num2 = Vector3.Distance(new Vector3(hitInfo.point.x, stateMachine.transform.position.y, hitInfo.point.z), stateMachine.transform.position);
			if (hitInfo.point.y > 1f && num2 > landPosDistance.minValue && hitInfo.normal.y >= minValidAngle)
			{
				Vector3 vector3 = hitInfo.point - stateMachine.transform.position;
				if (!Physics.Raycast(stateMachine.transform.position, vector3.normalized, vector3.magnitude * 0.8f, movement.obstructionLayer))
				{
					Debug.DrawRay(stateMachine.transform.position, vector3 * 0.8f, Color.green, 5f);
					landPosition = hitInfo.point;
					return true;
				}
				Debug.DrawRay(stateMachine.transform.position, vector3 * 0.8f, Color.red, 5f);
			}
		}
		return false;
	}
}
