using FMODUnity;
using UnityEngine;

public class AI_State_ChargeUp : AI_State
{
	[SerializeField]
	private float originChargeTimeMax = 2f;

	[SerializeField]
	private float originChargeTimeMin = 1f;

	private float originChargeTime;

	private float chargeTime;

	[Space(10f)]
	[SerializeField]
	protected AI_State chargeChannelDoneState;

	protected Vector3 currentTargetPostion;

	protected bool chargeStarted;

	protected float currentSpeed;

	[SerializeField]
	[EventRef]
	private string eventRef_startCharge;

	public float NormalizedChargeTime => Mathf.Clamp01(chargeTime / originChargeTime);

	public Vector3 CurrentTargetPosition => currentTargetPostion;

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Start()
	{
		base.Start();
	}

	public override void EnterState()
	{
		base.EnterState();
		ResetState();
		StartCharge();
	}

	public override void ExitState()
	{
		base.ExitState();
	}

	public override void UpdateState()
	{
		if (Raft_Network.IsHost)
		{
			base.UpdateState();
			ChannelCharge();
		}
	}

	public virtual void ResetState()
	{
		chargeTime = 0f;
		chargeStarted = false;
		originChargeTime = Random.Range(originChargeTimeMin, originChargeTimeMax);
	}

	protected virtual void ChannelCharge()
	{
		if (chargeStarted)
		{
			currentSpeed = Mathf.Lerp(0f, stateMachine.movement.defaultMovementSpeed, originChargeTime / chargeTime);
			IncrementChannelTimer();
			if (chargeTime >= originChargeTime)
			{
				ChargeFinished();
			}
		}
	}

	protected virtual void IncrementChannelTimer()
	{
		chargeTime += Time.deltaTime;
	}

	protected virtual void ChargeFinished()
	{
		chargeStarted = false;
		ChangeToChargeChannelFinishedState();
	}

	protected virtual void ChangeToChargeChannelFinishedState()
	{
		stateMachine.ChangeState(chargeChannelDoneState);
	}

	private void StartCharge()
	{
		chargeStarted = true;
		RuntimeManager.PlayOneShotAttached(eventRef_startCharge, stateMachine.gameObject);
	}
}
