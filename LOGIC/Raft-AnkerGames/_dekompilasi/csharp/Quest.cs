using System;
using UnityEngine;

public class Quest
{
	public Action OnQuestComplete;

	public Action OnQuestStarted;

	public QuestType questType;

	public QuestData questData;

	public bool isQuestComplete;

	public bool isQuestStarted;

	public int questProgress;

	public Quest(QuestType p_type)
	{
		questType = p_type;
	}

	public bool IsQuestRequirementDone()
	{
		if (questData == null || questData.questRequirements.Length == 0)
		{
			Debug.Log("Returning true for quest in requirement check, since quest data is null, or doesnt have requirement!");
			return true;
		}
		for (int i = 0; i < questData.questRequirements.Length; i++)
		{
			Quest questByType = QuestProgressTracker.GetQuestByType(questData.questRequirements[i]);
			if (questByType.isQuestComplete)
			{
				if (questByType.questType == QuestType.None)
				{
					Debug.Log("Returning true since the req is set to none");
					return true;
				}
				continue;
			}
			return false;
		}
		return true;
	}

	public void RestoreQuest(RGD_Quest rgdQuest)
	{
		isQuestComplete = rgdQuest.isQuestComplete;
		isQuestStarted = rgdQuest.isQuestStarted;
		questProgress = rgdQuest.progressAmount;
	}
}
