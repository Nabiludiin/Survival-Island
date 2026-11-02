namespace RG.Parsecs.Scavenge;

public class ActionOnScavengeItemEvent : ActionOnEvent<ScavengeItem, EventHubArgument<ScavengeItemEvents>, ScavengeItemEvents>
{
	protected override bool EventsEqual(ScavengeItemEvents a, ScavengeItemEvents b)
	{
		return a == b;
	}
}
