using UnityEngine;

public class LandmarkItem_Pickup_QuestReset : LandmarkItem_PickupItem
{
	[SerializeField]
	private QuestType resetQuestType;

	private void OnValidate()
	{
		resetOnLandmarkReset = true;
	}

	protected override void ResetLandmarkItem()
	{
		resetOnLandmarkReset = true;
		if (QuestProgressTracker.HasFinishedQuest(resetQuestType))
		{
			base.ResetLandmarkItem();
		}
		else
		{
			gameObject.SetActiveSafe(value: false);
		}
	}
}
