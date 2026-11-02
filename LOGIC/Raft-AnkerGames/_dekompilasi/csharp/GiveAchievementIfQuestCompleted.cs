using System;
using UnityEngine;

public class GiveAchievementIfQuestCompleted : MonoBehaviour
{
	[SerializeField]
	private QuestType quest;

	[SerializeField]
	private AchievementType achievementType;

	public void Start()
	{
		SaveAndLoad.LoadComplete = (Action)Delegate.Combine(SaveAndLoad.LoadComplete, new Action(TryGiveAchievement));
		Raft_Network.OnWorldReceivedLate = (Action)Delegate.Combine(Raft_Network.OnWorldReceivedLate, new Action(TryGiveAchievement));
	}

	private void OnDestroy()
	{
		SaveAndLoad.LoadComplete = (Action)Delegate.Remove(SaveAndLoad.LoadComplete, new Action(TryGiveAchievement));
		Raft_Network.OnWorldReceivedLate = (Action)Delegate.Remove(Raft_Network.OnWorldReceivedLate, new Action(TryGiveAchievement));
	}

	private void TryGiveAchievement()
	{
		if (QuestProgressTracker.HasFinishedQuest(quest))
		{
			AchievementHandler.UnlockAchievement(achievementType);
		}
	}
}
