using System;
using UnityEngine;

public class AI_State_Dive : AI_State
{
	private enum EscapeMode
	{
		Default,
		RayAngleCheck
	}

	[SerializeField]
	private MovementType movementType;

	[SerializeField]
	private EscapeMode escapeMode;

	[SerializeField]
	private LayerMask obstructionMask;

	[SerializeField]
	private Transform rayStart;

	[SerializeField]
	private float rayDistance = float.PositiveInfinity;

	[SerializeField]
	private Vector3 localDiveOffset;

	[SerializeField]
	private int numberOfChecks = 1;

	[SerializeField]
	[Range(0f, 1f)]
	private float downwardAngle = 0.3f;

	[SerializeField]
	private AI_State timerReachedState;

	[SerializeField]
	private float diveTime;

	[SerializeField]
	private AI_State reachDivePointState;

	[SerializeField]
	private float reachPointDistance = 1f;

	private Vector3 targetPoint;

	private float diveTimer;

	public override void EnterState()
	{
		base.EnterState();
		diveTimer = 0f;
		CalculateNewDivePoint();
	}

	public override void UpdateState()
	{
		base.UpdateState();
		if (!Raft_Network.IsHost)
		{
			return;
		}
		switch (movementType)
		{
		case MovementType.Translate_Simple:
			stateMachine.movement.MoveLook_Translate(stateMachine.transform, targetPoint, stateMachine.movement.MovementSpeed, stateMachine.movement.RotationSpeed);
			break;
		case MovementType.GoldenRule:
			stateMachine.movement.GoldenRuleMovement(targetPoint, stateMachine.rigid, stateMachine.movement.MovementSpeed);
			break;
		}
		if (timerReachedState != null)
		{
			diveTimer += Time.deltaTime;
			if (diveTimer >= diveTime)
			{
				stateMachine.ChangeState(timerReachedState);
				return;
			}
		}
		if (reachDivePointState != null && Vector3.Distance(stateMachine.transform.position, targetPoint) < reachPointDistance)
		{
			stateMachine.ChangeState(reachDivePointState);
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (active && Application.isPlaying)
		{
			Gizmos.DrawLine(transform.position, targetPoint);
			Gizmos.DrawSphere(targetPoint, 0.2f);
		}
	}

	public override void OnWorldShift(Vector3 shift)
	{
		base.OnWorldShift(shift);
		targetPoint -= shift;
	}

	private bool IsDefault()
	{
		return escapeMode == EscapeMode.Default;
	}

	private bool IsRayAngle()
	{
		return escapeMode == EscapeMode.RayAngleCheck;
	}

	private void CalculateNewDivePoint()
	{
		if (rayStart == null)
		{
			rayStart = stateMachine.transform;
		}
		Vector3 forward = rayStart.forward;
		Vector3 right = rayStart.right;
		forward.y = 0f;
		right.y = 0f;
		RaycastHit hitInfo;
		switch (escapeMode)
		{
		case EscapeMode.RayAngleCheck:
		{
			float num = 360f / (float)numberOfChecks;
			for (int i = 0; i < numberOfChecks; i++)
			{
				float num2 = (float)i * num;
				Vector3 vector = new Vector3(Mathf.Cos(num2 * (MathF.PI / 180f)), 0f - downwardAngle, Mathf.Sin(num2 * (MathF.PI / 180f)));
				if (!Physics.Raycast(rayStart.position, vector.normalized, out hitInfo, rayDistance, obstructionMask))
				{
					targetPoint = rayStart.position + vector.normalized * rayDistance;
					Debug.DrawRay(rayStart.position, vector.normalized * rayDistance, Color.green, 5f);
					return;
				}
				Debug.DrawLine(rayStart.position, hitInfo.point, Color.red, 5f);
			}
			targetPoint = rayStart.position + forward * rayDistance + new Vector3(0f, 0f - downwardAngle, 0f) * rayDistance;
			Debug.DrawLine(rayStart.position, targetPoint, Color.yellow, 5f);
			break;
		}
		case EscapeMode.Default:
			if (Physics.Raycast(rayStart.position, Vector3.down, out hitInfo, rayDistance, obstructionMask))
			{
				targetPoint = hitInfo.point;
				Debug.DrawLine(rayStart.position, hitInfo.point, Color.yellow, 5f);
				break;
			}
			targetPoint = rayStart.position + forward * localDiveOffset.z + right * localDiveOffset.x + Vector3.up * localDiveOffset.y;
			if (Physics.Raycast(rayStart.position, targetPoint - rayStart.position, out hitInfo, rayDistance, obstructionMask))
			{
				targetPoint = hitInfo.point;
				Debug.DrawLine(rayStart.position, targetPoint, Color.red, 5f);
			}
			else
			{
				Debug.DrawLine(rayStart.position, targetPoint, Color.green, 5f);
			}
			break;
		}
	}
}
