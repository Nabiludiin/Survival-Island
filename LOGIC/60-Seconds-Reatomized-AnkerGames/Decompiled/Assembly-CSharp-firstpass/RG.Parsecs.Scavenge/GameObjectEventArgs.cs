using System;

namespace RG.Parsecs.Scavenge;

public struct GameObjectEventArgs : IEventHubArgument<GameObjectEvents>, IEquatable<GameObjectEventArgs>
{
	public GameObjectEvents Event { get; private set; }

	public IActorBrain OtherActor { get; private set; }

	public float Value { get; private set; }

	public string ObjectPoolName
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public GameObjectEventArgs(GameObjectEvents e)
	{
		Event = e;
		OtherActor = null;
		Value = 0f;
	}

	public void Setup(GameObjectEvents e)
	{
		Event = e;
		OtherActor = null;
	}

	public GameObjectEventArgs(GameObjectEvents e, IActorBrain otherActor)
	{
		Event = e;
		OtherActor = otherActor;
		Value = 0f;
	}

	public void Setup(GameObjectEvents e, IActorBrain otherActor)
	{
		Event = e;
		OtherActor = otherActor;
	}

	public GameObjectEventArgs(GameObjectEvents e, IActorBrain otherActor, float value)
	{
		Event = e;
		OtherActor = otherActor;
		Value = value;
	}

	public override bool Equals(object obj)
	{
		if (obj is GameObjectEventArgs e)
		{
			if (Event == e.Event && OtherActor == e.OtherActor)
			{
				return Value == e.Value;
			}
			return false;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return new { Event, OtherActor, Value }.GetHashCode();
	}

	public bool Equals(GameObjectEventArgs other)
	{
		if (Event == other.Event && OtherActor == other.OtherActor)
		{
			return Value == other.Value;
		}
		return false;
	}

	public static bool operator ==(GameObjectEventArgs c1, GameObjectEventArgs c2)
	{
		return c1.Equals(c2);
	}

	public static bool operator !=(GameObjectEventArgs c1, GameObjectEventArgs c2)
	{
		return !c1.Equals(c2);
	}
}
