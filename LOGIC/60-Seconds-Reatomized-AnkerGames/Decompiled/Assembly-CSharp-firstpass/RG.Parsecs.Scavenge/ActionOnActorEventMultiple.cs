using System.Collections.Generic;

namespace RG.Parsecs.Scavenge;

public class ActionOnActorEventMultiple : ActionOnEventMultiple<ActorBrain, ActorEventsArgs, ActorEvents, ActorEventsComparer>
{
	public List<ActionsMultipleOnActorEvents> Actions;

	protected override void Awake()
	{
		_dict = new Dictionary<ActorEvents, List<EventAction>>(default(ActorEventsComparer));
		_events = new List<ActorEvents>(8);
		foreach (ActionsMultipleOnActorEvents action in Actions)
		{
			foreach (ActorEvents @event in action.Events)
			{
				_dict.Add(@event, action.Actions);
				_events.Add(@event);
			}
		}
		base.Awake();
	}
}
