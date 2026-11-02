namespace RG.Parsecs.Scavenge;

public class ActionOnRescuePodEntranceEvent : ActionOnEvent<RescurePodEntrance, EventHubArgument<RescurePodEntranceEvents>, RescurePodEntranceEvents>
{
	protected override bool EventsEqual(RescurePodEntranceEvents a, RescurePodEntranceEvents b)
	{
		return a == b;
	}
}
