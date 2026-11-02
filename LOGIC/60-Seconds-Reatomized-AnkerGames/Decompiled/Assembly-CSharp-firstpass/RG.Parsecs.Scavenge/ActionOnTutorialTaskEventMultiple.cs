using System.Collections.Generic;

namespace RG.Parsecs.Scavenge;

public class ActionOnTutorialTaskEventMultiple : ActionOnEventMultiple<ScavengeTutorialTask, EventHubArgument<TutorialTaskEvents>, TutorialTaskEvents, TutorialTaskEventsComparer>
{
	public List<ActionsMultipleTutorialTaskEvents> Actions;

	protected override void Awake()
	{
		_dict = new Dictionary<TutorialTaskEvents, List<EventAction>>(default(TutorialTaskEventsComparer));
		_events = new List<TutorialTaskEvents>(8);
		foreach (ActionsMultipleTutorialTaskEvents action in Actions)
		{
			foreach (TutorialTaskEvents @event in action.Events)
			{
				_dict.Add(@event, action.Actions);
				_events.Add(@event);
			}
		}
		base.Awake();
	}
}
