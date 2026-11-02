using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class MovementTask : ScavengeTutorialTask
{
	private PlayerController player;

	public float DelayChecking = 4.5f;

	private float _counter;

	public override void TaskStart(ScavengeManager manager, HintAnnouncer hintAnnouncer)
	{
		base.TaskStart(manager, hintAnnouncer);
		player = manager.ScavengePlayersManager.PlayerController;
		_counter = DelayChecking;
	}

	public override void TaskUpdate()
	{
		base.TaskUpdate();
		if (_counter > 0f)
		{
			_counter -= Time.deltaTime;
		}
		else if (!(player == null) && player.HasEverMoved)
		{
			InvokeSuccess();
			player = null;
		}
	}
}
