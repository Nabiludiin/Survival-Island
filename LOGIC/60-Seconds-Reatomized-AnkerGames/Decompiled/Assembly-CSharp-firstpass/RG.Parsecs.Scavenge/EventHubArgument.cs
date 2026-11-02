namespace RG.Parsecs.Scavenge;

public class EventHubArgument<T> : IEventHubArgument<T>
{
	public T Event { get; protected set; }

	public IActorBrain OtherActor { get; protected set; }

	public float Value { get; protected set; }

	public EventHubArgument(T e)
	{
		Event = e;
	}

	public EventHubArgument(T e, IActorBrain otherActor)
	{
		Event = e;
		OtherActor = otherActor;
	}

	public EventHubArgument(T e, IActorBrain otherActor, float value)
	{
		Event = e;
		OtherActor = otherActor;
		Value = value;
	}
}
