using System;
using UnityEngine;

[Serializable]
public class QuestInteractable_ComponentData_Achievement : QuestInteractable_ComponentData
{
	public AchievementType achievementToComplete = AchievementType.LAST;

	public override void OnFinishedInteraction(Network_Player player, bool atStart, bool successfull)
	{
		base.OnFinishedInteraction(player, atStart, successfull);
		Debug.Log("Achivement unlocked " + achievementToComplete);
		AchievementHandler.UnlockAchievement(achievementToComplete);
	}
}
