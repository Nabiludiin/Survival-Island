using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class InvokeActionsAction : EventAction
{
	[Range(0f, 1f)]
	public float Chance = 1f;

	public List<EventAction> Actions;

	protected override void Act(EventActionArgs args = null)
	{
		if (Actions.Count == 0)
		{
			Debug.LogError("No actions defined.");
		}
		else if (Chance >= Random.Range(0f, 1f))
		{
			for (int i = 0; i < Actions.Count; i++)
			{
				Actions[i].Invoke(args);
			}
		}
	}
}
