namespace RG.Parsecs.Scavenge;

public class ActionOnHandUIEvent : ActionOnEvent<HandUI, EventHubArgument<HandUIEvents>, HandUIEvents>
{
	protected override bool EventsEqual(HandUIEvents a, HandUIEvents b)
	{
		return a == b;
	}
}
