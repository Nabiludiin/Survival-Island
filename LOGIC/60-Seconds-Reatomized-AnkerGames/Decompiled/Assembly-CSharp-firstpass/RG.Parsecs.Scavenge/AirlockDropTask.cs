namespace RG.Parsecs.Scavenge;

public class AirlockDropTask : ScavengeTutorialTask
{
	private PlayerController player;

	public override void TaskStart(ScavengeManager manager, HintAnnouncer hintAnnouncer)
	{
		base.TaskStart(manager, hintAnnouncer);
		manager.ShipGenerator.DisableItems();
		player = manager.ScavengePlayersManager.PlayerController;
		player.OnAirlockDrop.Add(OnAirlockDrop);
	}

	private void OnAirlockDrop()
	{
		InvokeSuccess();
		player.OnAirlockDrop.Remove(OnAirlockDrop);
		player = null;
	}
}
