using System.Collections.Generic;
using UnityEngine;

public class RaycastInteractable_DisplayText_QuestInteractable : RaycastInteractable_DisplayText
{
	[SerializeField]
	private QuestEventBase questEventBase;

	[SerializeField]
	private List<int> displayQuestIndexes = new List<int>();

	protected override void OnIsRayed()
	{
		if (questEventBase != null && displayQuestIndexes.Contains(questEventBase.CurrentObjectStateIndex))
		{
			base.OnIsRayed();
		}
		else if (isDisplayingText)
		{
			HideDisplayText();
		}
	}
}
