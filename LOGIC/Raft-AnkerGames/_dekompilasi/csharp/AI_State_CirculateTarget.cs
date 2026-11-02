using System;
using UnityEngine;

public class AI_State_CirculateTarget : AI_State
{
	public float circulationRad;

	public Interval_Float circulationRadius;

	public float angleMultiplier = 24f;

	public float speed = 6f;

	public AI_State nextState;

	private Network_Player target;

	private float angle;

	private int bootlegDirection;

	public override void EnterState()
	{
		base.EnterState();
		stateMachine.movement.SetMovementType(MovementType.NavmeshAgent);
		stateMachine.movement.SetMovementSpeed(speed);
		bootlegDirection = Helper.RandomOneMinus();
		circulationRad = circulationRadius.GetRandomValue();
	}

	public override void UpdateState()
	{
		base.UpdateState();
		if (target == null)
		{
			target = Helper.ClosestPlayerToPoint(transform.position, includeDeadPlayers: false);
		}
		if (!(target == null))
		{
			angle += Time.deltaTime * angleMultiplier * (float)bootlegDirection;
			Vector3 position = target.FeetPosition + new Vector3(Mathf.Cos(angle * (MathF.PI / 180f)), 0f, Mathf.Sin(angle * (MathF.PI / 180f))).normalized * circulationRad;
			stateMachine.movement.MoveLook_Navmesh(position, stateMachine.movement.MovementSpeed);
			if (IsInBackOfTarget())
			{
				stateMachine.ChangeState(nextState);
			}
		}
	}

	private bool IsInBackOfTarget()
	{
		if (target.transform.InverseTransformPoint(stateMachine.transform.position).z < 0f)
		{
			return true;
		}
		return false;
	}
}
