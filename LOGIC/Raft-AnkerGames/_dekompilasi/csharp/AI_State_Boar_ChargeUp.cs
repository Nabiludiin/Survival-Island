using FMODUnity;
using UnityEngine;

public class AI_State_Boar_ChargeUp : AI_State_ChargeUp
{
	[SerializeField]
	private float chargeDirectionAngleMargin = 10f;

	[SerializeField]
	private float rotationSpeed = 10f;

	[SerializeField]
	private AI_Movement movement;

	[Space(10f)]
	[SerializeField]
	private bool lockOnMode = true;

	[SerializeField]
	private bool isFearlessInState = true;

	[SerializeField]
	private bool ignoreYPositioning = true;

	private AI_StateMachine_Boar boarSM;

	private bool isLockedOn;

	protected override void Awake()
	{
		base.Awake();
		boarSM = stateMachine as AI_StateMachine_Boar;
	}

	public override void EnterState()
	{
		base.EnterState();
		if (isFearlessInState)
		{
			boarSM.SetIsCurrentlyFearless(value: true, 1);
		}
	}

	public override void ExitState()
	{
		base.ExitState();
		boarSM.ChargeDirection = movement.model.forward;
		RuntimeManager.PlayOneShot(boarSM.er_ChargeUpDone, stateMachine.transform.position);
		if (isFearlessInState)
		{
			boarSM.SetIsCurrentlyFearless(value: false);
		}
	}

	public override void ResetState()
	{
		base.ResetState();
		isLockedOn = false;
	}

	protected override void IncrementChannelTimer()
	{
		if (!lockOnMode || isLockedOn)
		{
			base.IncrementChannelTimer();
		}
	}

	protected override void ChannelCharge()
	{
		if (boarSM.enemyTarget == null)
		{
			boarSM.ChangeState(boarSM.defaultState);
			return;
		}
		base.ChannelCharge();
		if (lockOnMode)
		{
			if (IsInAngle(ignoreYPositioning))
			{
				isLockedOn = true;
			}
			if (isLockedOn)
			{
				return;
			}
		}
		Vector3 position = boarSM.enemyTarget.transform.position;
		if (ignoreYPositioning)
		{
			position.y = movement.model.position.y;
		}
		movement.Look(movement.model, position, rotationSpeed);
	}

	protected override void ChargeFinished()
	{
		if (lockOnMode || IsInAngle(ignoreYPositioning))
		{
			base.ChargeFinished();
		}
	}

	private bool IsInAngle(bool ignoreY)
	{
		if (boarSM.enemyTarget == null)
		{
			return false;
		}
		Vector3 vector;
		if (ignoreY)
		{
			Vector3 position = boarSM.enemyTarget.transform.position;
			position.y = 0f;
			Vector3 position2 = movement.model.position;
			position2.y = 0f;
			vector = position - position2;
		}
		else
		{
			vector = boarSM.enemyTarget.transform.position - movement.model.position;
		}
		if (Vector3.Angle(vector, movement.model.forward) < chargeDirectionAngleMargin)
		{
			return true;
		}
		return false;
	}
}
