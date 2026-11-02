using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class InvokeRandomActionAction : EventAction
{
	public List<EventAction> Actions;

	public bool RemoveAfterUse;

	public bool PickRandom;

	protected override void Act(EventActionArgs args = null)
	{
		if (Actions.Count == 0)
		{
			Debug.LogError("No actions defined.");
			return;
		}
		EventAction eventAction = null;
		eventAction = ((!PickRandom) ? Actions[0] : Actions.RandomElement());
		if (RemoveAfterUse)
		{
			Actions.Remove(eventAction);
		}
		eventAction.Invoke(args);
	}
}
