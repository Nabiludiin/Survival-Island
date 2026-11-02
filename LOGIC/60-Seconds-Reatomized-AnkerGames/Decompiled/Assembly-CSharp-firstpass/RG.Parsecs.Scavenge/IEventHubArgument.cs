namespace RG.Parsecs.Scavenge;

public interface IEventHubArgument<T>
{
	T Event { get; }

	IActorBrain OtherActor { get; }

	float Value { get; }
}
