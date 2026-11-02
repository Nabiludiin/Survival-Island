using UnityEngine;

public class AI_State_Chargeup_Boss_Varuna : AI_State_ChargeUp
{
	public AI_State_Charge chargeState;

	public AI_State cancelChargeUpState;

	public bool cancelIfNoVisionToPlayer = true;

	[SerializeField]
	private float chargeDirectionAngleMargin = 10f;

	[SerializeField]
	private float rotationSpeed = 10f;

	[SerializeField]
	private float minValidDistanceToChargeTarget = 1f;

	[SerializeField]
	private AI_Movement movement;

	[Space(10f)]
	[SerializeField]
	private bool lockOnMode = true;

	[SerializeField]
	private bool isFearlessInState = true;

	[SerializeField]
	private bool ignoreYPositioning = true;

	[SerializeField]
	private bool usesMovementPrediction = true;

	private MovementDeltaTracker playerMovementPredictor;

	private AI_StateMachine_Boss_Varuna bossSM;

	private bool isLockedOn;

	protected override void Awake()
	{
		base.Awake();
		bossSM = stateMachine as AI_StateMachine_Boss_Varuna;
	}

	public override void EnterState()
	{
		base.EnterState();
		currentSpeed = movement.MovementSpeed;
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

	public override void OnStateChange<T>(params T[] parameters)
	{
		base.OnStateChange(parameters);
		Network_Player network_Player = (Network_Player)(object)parameters[0];
		if (network_Player != null)
		{
			bossSM.enemyTarget = network_Player;
			playerMovementPredictor = network_Player.GetComponent<MovementDeltaTracker>();
		}
		else
		{
			Debug.Log("Player should be assigned when entering this state, but was unable to do so :(");
		}
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
		if (bossSM.enemyTarget == null)
		{
			bossSM.ChangeState(cancelChargeUpState);
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
		if (usesMovementPrediction && playerMovementPredictor != null)
		{
			currentTargetPostion = bossSM.GetEnemyTargetDeltaTracker().GetApproximateTrackedTransformPositionWithDeltaMovement(Vector3.Distance(stateMachine.transform.position, bossSM.enemyTarget.transform.position) / 3.8f, normalizeDirection: false);
		}
		else
		{
			currentTargetPostion = bossSM.enemyTarget.transform.position;
		}
		if (ignoreYPositioning)
		{
			currentTargetPostion.y = movement.model.position.y;
		}
		if (bossSM.enemyTarget != null)
		{
			bool flag = false;
			if (cancelIfNoVisionToPlayer && NormalizedChargeTime < 0.5f && !Helper.HasVisionToPlayer(bossSM.enemyTarget, stateMachine.movement.model.position, useFeetPosition: false))
			{
				flag = true;
			}
			if (!bossSM.PlayerIsInBossTargetingArea(bossSM.enemyTarget))
			{
				flag = true;
			}
			if (flag)
			{
				chargeStarted = false;
				bossSM.ChangeState(cancelChargeUpState);
				bossSM.seePlayer_component.SetCooldownTimeStamp(Random.Range(1f, 3f));
				return;
			}
		}
		movement.MoveLookObstruction_RigidBodyMove(stateMachine.transform, currentTargetPostion, currentSpeed, rotationSpeed, bossSM.rigid);
	}

	protected override void ChargeFinished()
	{
		if (lockOnMode || !(Vector3.Distance(movement.model.position, currentTargetPostion) > minValidDistanceToChargeTarget) || IsInAngle(ignoreYPositioning))
		{
			base.ChargeFinished();
		}
	}

	protected override void ChangeToChargeChannelFinishedState()
	{
		if (bossSM.CanChargePlayer(bossSM.enemyTarget))
		{
			bossSM.ChangeState<Network_Player>(chargeChannelDoneState, bossSM.enemyTarget);
		}
		else
		{
			bossSM.ChangeState(cancelChargeUpState);
		}
	}

	private bool IsInAngle(bool ignoreY)
	{
		if (bossSM.enemyTarget == null)
		{
			return false;
		}
		Vector3 vector2;
		if (ignoreY)
		{
			Vector3 vector = currentTargetPostion;
			vector.y = 0f;
			Vector3 position = movement.model.position;
			position.y = 0f;
			vector2 = vector - position;
		}
		else
		{
			vector2 = currentTargetPostion - movement.model.position;
		}
		if (Vector3.Angle(vector2, movement.model.forward) < chargeDirectionAngleMargin)
		{
			return true;
		}
		return false;
	}
}
