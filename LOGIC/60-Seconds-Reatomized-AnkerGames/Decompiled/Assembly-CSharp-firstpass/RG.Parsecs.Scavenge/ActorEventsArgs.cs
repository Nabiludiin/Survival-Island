using System;

namespace RG.Parsecs.Scavenge;

public struct ActorEventsArgs : IEventHubArgument<ActorEvents>, IEquatable<ActorEventsArgs>
{
	public ActorEvents Event { get; private set; }

	public IActorBrain OtherActor { get; private set; }

	public float Value { get; private set; }

	public string ObjectPoolName
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public ActorEventsArgs(ActorEvents e)
	{
		Event = e;
		OtherActor = null;
		Value = 0f;
	}

	public void Setup(ActorEvents e)
	{
		Event = e;
		OtherActor = null;
	}

	public ActorEventsArgs(ActorEvents e, IActorBrain otherActor)
	{
		Event = e;
		OtherActor = otherActor;
		Value = 0f;
	}

	public void Setup(ActorEvents e, IActorBrain otherActor)
	{
		Event = e;
		OtherActor = otherActor;
	}

	public ActorEventsArgs(ActorEvents e, IActorBrain otherActor, float value)
	{
		Event = e;
		OtherActor = otherActor;
		Value = value;
	}

	public override bool Equals(object obj)
	{
		if (obj is ActorEventsArgs actorEventsArgs)
		{
			if (Event == actorEventsArgs.Event && OtherActor == actorEventsArgs.OtherActor)
			{
				return Value == actorEventsArgs.Value;
			}
			return false;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return new { Event, OtherActor, Value }.GetHashCode();
	}

	public bool Equals(ActorEventsArgs other)
	{
		if (Event == other.Event && OtherActor == other.OtherActor)
		{
			return Value == other.Value;
		}
		return false;
	}

	public static bool operator ==(ActorEventsArgs c1, ActorEventsArgs c2)
	{
		return c1.Equals(c2);
	}

	public static bool operator !=(ActorEventsArgs c1, ActorEventsArgs c2)
	{
		return !c1.Equals(c2);
	}
}
