namespace RG.Parsecs.Scavenge;

public class ActionOnClockEvent : ActionOnEvent<ScavengeClock, EventHubArgument<ClockEvents>, ClockEvents>
{
	protected override bool EventsEqual(ClockEvents a, ClockEvents b)
	{
		return a == b;
	}
}
