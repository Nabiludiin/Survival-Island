namespace RG.Parsecs.Scavenge;

public class ActionOnActorEvent : ActionOnEvent<ActorBrain, ActorEventsArgs, ActorEvents>
{
	protected override bool EventsEqual(ActorEvents a, ActorEvents b)
	{
		return a == b;
	}
}
