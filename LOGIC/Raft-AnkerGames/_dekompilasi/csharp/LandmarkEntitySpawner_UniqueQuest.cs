using UnityEngine;

public class LandmarkEntitySpawner_UniqueQuest : LandmarkEntitySpawner
{
	[SerializeField]
	private QuestType questType = QuestType.None;

	protected override AI_NetworkBehaviour CreateEntity(bool forceShouldSpawn = false)
	{
		if (QuestProgressTracker.HasFinishedQuest(questType))
		{
			if (Application.isEditor)
			{
				Debug.Log("Can't spawn " + entityType.ToString() + " because " + questType.ToString() + " quest is completed");
			}
			return null;
		}
		return base.CreateEntity(forceShouldSpawn);
	}
}
