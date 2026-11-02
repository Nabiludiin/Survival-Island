using System;

[Serializable]
public class QuestInteractable_ComponentData_Quest : QuestInteractable_ComponentData
{
	public QuestType questToComplete = QuestType.None;

	public override void OnFinishedInteraction(Network_Player player, bool atStart, bool successfull)
	{
		base.OnFinishedInteraction(player, atStart, successfull);
		QuestProgressTracker.FinishQuestRPC(questToComplete);
	}
}
