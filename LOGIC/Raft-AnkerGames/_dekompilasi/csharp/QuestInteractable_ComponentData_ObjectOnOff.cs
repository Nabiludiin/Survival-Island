using System;
using UnityEngine;

[Serializable]
public class QuestInteractable_ComponentData_ObjectOnOff : QuestInteractable_ComponentData
{
	public bool onInteractState = true;

	public GameObject[] gameObjects;

	public override void OnFinishedInteraction(Network_Player player, bool atStart, bool successfull)
	{
		base.OnFinishedInteraction(player, atStart, successfull);
		if (!gameObjects.ContainsItems())
		{
			return;
		}
		GameObject[] array = gameObjects;
		foreach (GameObject gameObject in array)
		{
			if (gameObject != null)
			{
				gameObject.SetActiveSafe(onInteractState);
			}
		}
	}
}
