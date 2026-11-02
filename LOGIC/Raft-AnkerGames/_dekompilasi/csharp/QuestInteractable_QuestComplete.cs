using System;
using UnityEngine;

public class QuestInteractable_QuestComplete : QuestInteractable
{
	[SerializeField]
	private QuestType requiredQuest;

	private void Awake()
	{
		if (!Raft_Network.IsHost)
		{
			return;
		}
		Quest questByType = QuestProgressTracker.GetQuestByType(requiredQuest);
		if (questByType != null)
		{
			questByType.OnQuestComplete = (Action)Delegate.Combine(questByType.OnQuestComplete, (Action)(() =>
			{
				SendOnInteractNetwork(null, forceSuccess: true);
			}));
		}
	}
}
