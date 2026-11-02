using System;
using UnityEngine;

[Serializable]
public class QuestInteractable_ComponentData_ScriptOnOff : QuestInteractable_ComponentData
{
	public bool onInteractState = true;

	public MonoBehaviour[] scripts;

	public override void OnFinishedInteraction(Network_Player player, bool atStart, bool successfull)
	{
		base.OnFinishedInteraction(player, atStart, successfull);
		if (!scripts.ContainsItems())
		{
			return;
		}
		MonoBehaviour[] array = scripts;
		foreach (MonoBehaviour monoBehaviour in array)
		{
			if (monoBehaviour != null)
			{
				monoBehaviour.enabled = onInteractState;
			}
		}
	}
}
