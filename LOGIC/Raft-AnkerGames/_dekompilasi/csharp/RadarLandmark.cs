using UnityEngine;

public class RadarLandmark : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.tag == "Player")
		{
			QuestProgressTracker.FinishQuestRPC(QuestType.RadioTowerReached);
			if (GameModeValueManager.GetCurrentGameModeValue().achievementVariables.trackAchievments)
			{
				AchievementHandler.UnlockAchievement(AchievementType.ach_radiotower);
			}
		}
	}
}
