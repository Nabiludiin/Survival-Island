namespace RG.Parsecs.Scavenge;

public interface IHaveEventHub<T1, T2, T3> : IActorBrain where T2 : IEventHubArgument<T3>
{
	EventHub<T1, T2, T3> EventHub { get; }
}
