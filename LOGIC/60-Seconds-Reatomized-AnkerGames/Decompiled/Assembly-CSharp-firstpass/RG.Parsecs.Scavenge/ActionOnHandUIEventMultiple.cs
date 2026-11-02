using System.Collections.Generic;

namespace RG.Parsecs.Scavenge;

public class ActionOnHandUIEventMultiple : ActionOnEventMultiple<HandUI, EventHubArgument<HandUIEvents>, HandUIEvents, HandUIEventsComparer>
{
	public List<ActionsMultipleHandUIEvents> Actions;

	protected override void Awake()
	{
		_dict = new Dictionary<HandUIEvents, List<EventAction>>(default(HandUIEventsComparer));
		_events = new List<HandUIEvents>(8);
		foreach (ActionsMultipleHandUIEvents action in Actions)
		{
			foreach (HandUIEvents @event in action.Events)
			{
				_dict.Add(@event, action.Actions);
				_events.Add(@event);
			}
		}
		base.Awake();
	}
}
