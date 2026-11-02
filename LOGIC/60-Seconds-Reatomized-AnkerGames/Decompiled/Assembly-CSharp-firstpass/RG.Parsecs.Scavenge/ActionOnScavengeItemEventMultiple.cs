using System.Collections.Generic;

namespace RG.Parsecs.Scavenge;

public class ActionOnScavengeItemEventMultiple : ActionOnEventMultiple<ScavengeItem, EventHubArgument<ScavengeItemEvents>, ScavengeItemEvents, ScavengeItemEventsComparer>
{
	public List<ActionsMultipleScavengeItemEvents> Actions;

	protected override void Awake()
	{
		_dict = new Dictionary<ScavengeItemEvents, List<EventAction>>(default(ScavengeItemEventsComparer));
		_events = new List<ScavengeItemEvents>(8);
		foreach (ActionsMultipleScavengeItemEvents action in Actions)
		{
			foreach (ScavengeItemEvents @event in action.Events)
			{
				_dict.Add(@event, action.Actions);
				_events.Add(@event);
			}
		}
		base.Awake();
	}
}
