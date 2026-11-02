using UnityEngine;

public class AI_Component_TimerSwitchState : AI_Component
{
	[SerializeField]
	private TimerEventer timer;

	[SerializeField]
	private AI_State nextState;

	public override void OnEnterState()
	{
		base.OnEnterState();
		timer.ResetTimer();
	}

	protected override void OnUpdateComponent()
	{
		base.OnUpdateComponent();
		if (!Raft_Network.IsHost)
		{
			return;
		}
		if (timer.IsTimeBeyondCooldown())
		{
			if (SwitchState(nextState))
			{
				timer.ResetTimer();
			}
		}
		else
		{
			timer.DoUpdate(Time.deltaTime);
		}
	}
}
