namespace RG.Parsecs.Scavenge;

public class ActionOnTutorialTaskEvent : ActionOnEvent<ScavengeTutorialTask, EventHubArgument<TutorialTaskEvents>, TutorialTaskEvents>
{
	protected override bool EventsEqual(TutorialTaskEvents a, TutorialTaskEvents b)
	{
		return a == b;
	}
}
