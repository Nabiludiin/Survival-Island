public class QuestInteractable_CoolingChallenge_Lever : QuestInteractable
{
	public override bool CanPlayerInteract(Network_Player player, bool overwriteInventoryValidation = false)
	{
		switch (currentObjectStateIndex)
		{
		case 0:
			if (QuestProgressTracker.HasFinishedQuest(QuestType.TemperanceCoolingRodsInserted))
			{
				return base.CanPlayerInteract(player, overwriteInventoryValidation);
			}
			return false;
		case 1:
			if (QuestProgressTracker.HasFinishedQuest(QuestType.TemperanceReactorComplete))
			{
				return base.CanPlayerInteract(player, overwriteInventoryValidation);
			}
			return false;
		default:
			return false;
		}
	}

	public override void DisplayInteractText(Network_Player player)
	{
		switch (currentObjectStateIndex)
		{
		case 0:
			if (QuestProgressTracker.HasFinishedQuest(QuestType.TemperanceCoolingRodsInserted))
			{
				base.DisplayInteractText(player);
			}
			break;
		case 1:
			if (QuestProgressTracker.HasFinishedQuest(QuestType.TemperanceReactorComplete))
			{
				base.DisplayInteractText(player);
			}
			break;
		}
	}
}
