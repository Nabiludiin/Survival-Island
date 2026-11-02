using UnityEngine;

public class AI_State_ChargeUp_Target : AI_State_ChargeUp
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
	private bool ignoreYPositioning = true;

	[SerializeField]
	private AI_State fallbackState;

	private Network_Player player;

	private bool isLockedOn;

	protected override void Awake()
	{
		base.Awake();
	}

	public override void EnterState()
	{
		base.EnterState();
		player = Helper.ClosestPlayerToPoint(stateMachine.transform.position, 30f, includeDeadPlayers: false);
	}

	public override void ExitState()
	{
		base.ExitState();
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
		if (player == null)
		{
			stateMachine.ChangeState(fallbackState);
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
		Vector3 position = player.transform.position;
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

	protected override void ChangeToChargeChannelFinishedState()
	{
		stateMachine.ChangeState<Network_Player>(chargeChannelDoneState, player);
	}

	private bool IsInAngle(bool ignoreY)
	{
		if (player == null)
		{
			return false;
		}
		Vector3 vector;
		if (ignoreY)
		{
			Vector3 position = player.transform.position;
			position.y = 0f;
			Vector3 position2 = movement.model.position;
			position2.y = 0f;
			vector = position - position2;
		}
		else
		{
			vector = player.transform.position - movement.model.position;
		}
		if (Vector3.Angle(vector, movement.model.forward) < chargeDirectionAngleMargin)
		{
			return true;
		}
		return false;
	}
}
