using System;
using UnityEngine;

public class AI_State_WaitForQuest : AI_State
{
	public AI_State questDoneState;

	public QuestType questType;

	public float waitBeforeSwapTime = 3f;

	public float checkInterval = 1f;

	private TimerEventer timerEventer;

	private bool stateFired;

	protected override void Start()
	{
		base.Start();
		this.timerEventer = new TimerEventer(checkInterval);
		TimerEventer timerEventer = this.timerEventer;
		timerEventer.OnTimerReached = (Action)Delegate.Combine(timerEventer.OnTimerReached, new Action(CheckIfQuestDone));
	}

	public override void EnterState()
	{
		base.EnterState();
		ResetState();
	}

	public override void UpdateState()
	{
		base.UpdateState();
		if (!stateFired)
		{
			timerEventer.DoUpdate(Time.deltaTime, autoResetTimer: true);
		}
	}

	private void ResetState()
	{
		stateFired = false;
	}

	private void CheckIfQuestDone()
	{
		if (QuestProgressTracker.HasFinishedQuest(questType))
		{
			OnFireState();
		}
	}

	protected virtual void OnFireState()
	{
		stateFired = true;
		Invoke("SwapState", waitBeforeSwapTime);
	}

	protected virtual void SwapState()
	{
		stateMachine.ChangeState(questDoneState);
	}
}
