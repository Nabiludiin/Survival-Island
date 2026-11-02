using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class WaitTask : ScavengeTutorialTask
{
	public float Duration;

	private float _durationCounter;

	public override void TaskStart(ScavengeManager manager, HintAnnouncer hintAnnouncer)
	{
		base.TaskStart(manager, hintAnnouncer);
		_hintAnnouncer = hintAnnouncer;
		_durationCounter = 0f;
	}

	public override void TaskUpdate()
	{
		base.TaskUpdate();
		if (_durationCounter > Duration)
		{
			InvokeSuccess();
		}
		else
		{
			_durationCounter += Time.deltaTime;
		}
	}
}
