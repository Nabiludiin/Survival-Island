using System.Collections.Generic;

namespace RG.Parsecs.Scavenge;

public class StopActionAction : EventAction
{
	public List<StopableEventAction> Actions = new List<StopableEventAction>();

	protected override void Act(EventActionArgs args = null)
	{
		for (int i = 0; i < Actions.Count; i++)
		{
			Actions[i].InvokeStop(args);
		}
	}
}
