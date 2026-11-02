using System;
using UnityEngine;

public class LandmarkEntitySpawner_Repeating_QuestRequirement : LandmarkEntitySpawner_Repeating
{
	public enum SpawnWhenSetting
	{
		QuestDone,
		QuestNotDone
	}

	[SerializeField]
	private QuestType questRequirement = QuestType.None;

	[SerializeField]
	private SpawnWhenSetting spawnWhenSetting;

	[SerializeField]
	private bool killSpawnedOnQuestComplete;

	private Quest quest;

	protected override void Start()
	{
		if (Raft_Network.IsHost)
		{
			this.quest = QuestProgressTracker.GetQuestByType(questRequirement);
			if (this.quest != null)
			{
				Quest quest = this.quest;
				quest.OnQuestComplete = (Action)Delegate.Combine(quest.OnQuestComplete, new Action(HandleOnQuestIsDone));
			}
			else
			{
				Debug.Log("Unable To Find quest by type" + questRequirement);
			}
		}
		base.Start();
	}

	protected override void TryToSpawnEntity()
	{
		if (quest == null)
		{
			return;
		}
		switch (spawnWhenSetting)
		{
		case SpawnWhenSetting.QuestDone:
			if (quest.isQuestComplete)
			{
				base.TryToSpawnEntity();
			}
			break;
		case SpawnWhenSetting.QuestNotDone:
			if (!quest.isQuestComplete)
			{
				base.TryToSpawnEntity();
			}
			break;
		}
	}

	private void HandleOnQuestIsDone()
	{
		if (killSpawnedOnQuestComplete && HasSpawnedEntity())
		{
			spawnedEntityBehaviour.networkEntity.Damage(999999f, Vector3.zero, Vector3.zero, EntityType.None, isLocal: false);
		}
	}
}
