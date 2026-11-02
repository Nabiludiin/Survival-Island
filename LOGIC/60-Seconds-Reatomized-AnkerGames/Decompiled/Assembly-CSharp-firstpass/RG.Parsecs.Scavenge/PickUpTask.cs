using System.Collections.Generic;

namespace RG.Parsecs.Scavenge;

public class PickUpTask : ScavengeTutorialTask
{
	public List<ScavangeItemData> Items;

	private PlayerController player;

	public override void TaskStart(ScavengeManager manager, HintAnnouncer hintAnnouncer)
	{
		base.TaskStart(manager, hintAnnouncer);
		manager.ShipGenerator.EnableItems(Items);
		player = manager.ScavengePlayersManager.PlayerController;
		player.OnPickUp.Add(OnPickUp);
	}

	private void OnPickUp(ScavangeItemData data, ItemSpawnPoint spawnPoint)
	{
		foreach (ScavangeItemData item in Items)
		{
			if (data == item)
			{
				InvokeSuccess();
				player.OnPickUp.Remove(OnPickUp);
				player = null;
				break;
			}
		}
	}
}
