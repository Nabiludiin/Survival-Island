using System;
using UnityEngine;

public class QuestListener : MonoBehaviour
{
	public QuestType triggeredQuest = QuestType.None;

	public void Start()
	{
		Quest questByType = QuestProgressTracker.GetQuestByType(triggeredQuest);
		if (questByType.isQuestComplete)
		{
			OnQuestRestore();
		}
		questByType.OnQuestComplete = (Action)Delegate.Combine(questByType.OnQuestComplete, new Action(OnQuestComplete));
	}

	protected virtual void OnQuestRestore()
	{
	}

	protected virtual void OnQuestComplete()
	{
	}
}
