using System.Collections.Generic;

namespace RG.Parsecs.Scavenge;

public class ActionOnRescuePodEntranceEventMultiple : ActionOnEventMultiple<RescurePodEntrance, EventHubArgument<RescurePodEntranceEvents>, RescurePodEntranceEvents, RescurePodEntranceEventsComparer>
{
	public List<ActionsMultipleOnRescuePodEntranceEvents> Actions;

	protected override void Awake()
	{
		_dict = new Dictionary<RescurePodEntranceEvents, List<EventAction>>(default(RescurePodEntranceEventsComparer));
		_events = new List<RescurePodEntranceEvents>(8);
		foreach (ActionsMultipleOnRescuePodEntranceEvents action in Actions)
		{
			foreach (RescurePodEntranceEvents @event in action.Events)
			{
				_dict.Add(@event, action.Actions);
				_events.Add(@event);
			}
		}
		base.Awake();
	}
}
