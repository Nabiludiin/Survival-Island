using UnityEngine;

public class AI_State_Wait : AI_State
{
	[SerializeField]
	private Interval_Float waitInterval;

	[SerializeField]
	protected AI_State nextState;

	protected float timeToWait;

	protected float startWaitTime;

	protected float normalizedTime;

	private bool timerReached;

	public override void EnterState()
	{
		base.EnterState();
		timeToWait = waitInterval.GetRandomValue();
		startWaitTime = timeToWait;
		stateMachine.movement.SetMovementType(MovementType.None);
		timerReached = false;
	}

	public override void UpdateState()
	{
		base.UpdateState();
		if (Raft_Network.IsHost && !(nextState == null))
		{
			timeToWait -= Time.deltaTime;
			normalizedTime = Mathf.Clamp01(1f - timeToWait / startWaitTime);
			if (timeToWait <= 0f && !timerReached)
			{
				timerReached = true;
				OnWaitTimerReached();
			}
		}
	}

	protected virtual void OnWaitTimerReached()
	{
		stateMachine.ChangeState(nextState);
	}
}
