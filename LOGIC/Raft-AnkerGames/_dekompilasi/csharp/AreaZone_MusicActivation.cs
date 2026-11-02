using System;
using UnityEngine;

public class AreaZone_MusicActivation : AreaZone_PlayerCounter
{
	[SerializeField]
	private Helper_PlayMusic areaZone_music;

	[SerializeField]
	private QuestType questThatMustBeCompleteForMusic;

	[SerializeField]
	private QuestType questRequirementForMusic;

	[SerializeField]
	private bool stateToPlayAccordingToQuestIsDone;

	private Quest requirementQuest;

	private Quest trackedQuest;

	private void OnEnable()
	{
		if (questThatMustBeCompleteForMusic != QuestType.None)
		{
			requirementQuest = QuestProgressTracker.GetQuestByType(questThatMustBeCompleteForMusic);
			Quest quest = requirementQuest;
			quest.OnQuestComplete = (Action)Delegate.Combine(quest.OnQuestComplete, new Action(CheckMusicStart));
		}
		if (questRequirementForMusic != QuestType.None)
		{
			trackedQuest = QuestProgressTracker.GetQuestByType(questRequirementForMusic);
			Quest quest2 = trackedQuest;
			quest2.OnQuestComplete = (Action)Delegate.Combine(quest2.OnQuestComplete, new Action(DisableMusic));
		}
	}

	private void OnDisable()
	{
		if (requirementQuest != null)
		{
			Quest quest = requirementQuest;
			quest.OnQuestComplete = (Action)Delegate.Remove(quest.OnQuestComplete, new Action(CheckMusicStart));
		}
		if (trackedQuest != null)
		{
			Quest quest2 = trackedQuest;
			quest2.OnQuestComplete = (Action)Delegate.Remove(quest2.OnQuestComplete, new Action(DisableMusic));
		}
		DisableMusic();
	}

	protected override void OnAddNewPlayer(Network_Player player)
	{
		if (player.IsLocalPlayer && (questThatMustBeCompleteForMusic == QuestType.None || QuestProgressTracker.HasFinishedQuest(questThatMustBeCompleteForMusic)) && (questRequirementForMusic == QuestType.None || QuestProgressTracker.HasFinishedQuest(questRequirementForMusic) == stateToPlayAccordingToQuestIsDone))
		{
			areaZone_music.ApplyMusicSettings();
		}
	}

	protected override void OnRemovePlayer(Network_Player player)
	{
		if (player.IsLocalPlayer)
		{
			DisableMusic();
		}
	}

	private void DisableMusic()
	{
		areaZone_music.RevertMusicSettings();
	}

	private void CheckMusicStart()
	{
		if (playersInArea != null && playersInArea.Count >= 0 && playersInArea.Contains(localPlayer) && (questRequirementForMusic == QuestType.None || QuestProgressTracker.HasFinishedQuest(questRequirementForMusic) == stateToPlayAccordingToQuestIsDone))
		{
			areaZone_music.ApplyMusicSettings();
		}
	}
}
