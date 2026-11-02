public class QuestItemPickup : PickupItem
{
	public QuestItem[] questYield;

	private void OnValidate()
	{
		if (questYield.ContainsItems() && questYield[0] != null)
		{
			string displayTerm = questYield[0].SOQuestItem.DisplayTerm;
			if (pickupTerm != displayTerm)
			{
				pickupTerm = displayTerm;
			}
		}
	}

	public bool ContainsQuestItemInYield(QuestItemType questItemType)
	{
		QuestItem[] array = questYield;
		foreach (QuestItem questItem in array)
		{
			if (questItem != null && questItem.SOQuestItem.questItemType == questItemType)
			{
				return true;
			}
		}
		return false;
	}
}
