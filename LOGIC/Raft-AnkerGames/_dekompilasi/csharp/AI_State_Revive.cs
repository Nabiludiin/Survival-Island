using UnityEngine;

public class AI_State_Revive : AI_State
{
	[SerializeField]
	public TimerEventer reviveTimer;

	public AI_State doneRevivingState;

	public Collider pickupColliderToReset;

	private AI_StateMachine_Animal stateMachine_Animal;

	protected override void Awake()
	{
		base.Awake();
		stateMachine_Animal = stateMachine as AI_StateMachine_Animal;
	}

	public override void UpdateState()
	{
		base.UpdateState();
		reviveTimer.DoUpdate(Time.deltaTime);
		if (reviveTimer.IsTimeBeyondCooldown())
		{
			stateMachine.ChangeState(doneRevivingState);
			reviveTimer.ResetTimer();
		}
	}

	public override void EnterState()
	{
		base.EnterState();
		if (pickupColliderToReset != null)
		{
			pickupColliderToReset.enabled = true;
			pickupColliderToReset.gameObject.SetActiveSafe(value: true);
		}
	}

	public override void ExitState()
	{
		OnRevive();
		if (pickupColliderToReset != null)
		{
			pickupColliderToReset.enabled = false;
			pickupColliderToReset.gameObject.SetActiveSafe(value: false);
		}
		base.ExitState();
	}

	public void OnRevive()
	{
		Stat_Health stat_health = stateMachine_Animal.networkBehaviour.networkEntity.stat_health;
		stat_health.Value = stat_health.Max;
	}

	public override RGD_AI_State SaveState()
	{
		return new RGD_AI_Revive(this);
	}
}
