using System.Collections.Generic;
using UnityEngine;

public class Fixer_Component_QuestInteractable : Fixer_Component_ABS
{
	public override bool DoCheck(Transform target)
	{
		bool result = true;
		List<QuestEventBase> result2 = new List<QuestEventBase>();
		target.GetComponentsInChildrenRecursively(ref result2);
		foreach (QuestEventBase item in result2)
		{
			if (item.GetComponents<QuestEventBase>().Length > 1)
			{
				Debug.LogError(item.transform.name + " has more then 1 quest interactable component!");
				result = false;
			}
		}
		return result;
	}

	public override bool DoFix(Transform target)
	{
		return true;
	}
}
