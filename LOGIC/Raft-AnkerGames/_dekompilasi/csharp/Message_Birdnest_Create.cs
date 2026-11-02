using System;
using System.Linq;
using Unity.Netcode;

[Serializable]
public class Message_Birdnest_Create : Message_NetworkBehaviour
{
	public bool[] activeEggs;

	public bool[] activeFeathers;

	protected bool Equals(Message_Birdnest_Create other)
	{
		if (activeEggs == null && other.activeEggs != null)
		{
			return false;
		}
		if (activeEggs != null && other.activeEggs == null)
		{
			return false;
		}
		if (activeFeathers == null && other.activeFeathers != null)
		{
			return false;
		}
		if (activeFeathers != null && other.activeFeathers == null)
		{
			return false;
		}
		if (Equals((Message_NetworkBehaviour)other) && ((activeEggs == null && other.activeEggs == null) || Enumerable.SequenceEqual(activeEggs, other.activeEggs)))
		{
			if (activeFeathers != null || other.activeFeathers != null)
			{
				return Enumerable.SequenceEqual(activeFeathers, other.activeFeathers);
			}
			return true;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (this == obj)
		{
			return true;
		}
		if (obj.GetType() != GetType())
		{
			return false;
		}
		return Equals((Message_Birdnest_Create)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), activeEggs, activeFeathers);
	}

	public static bool operator ==(Message_Birdnest_Create left, Message_Birdnest_Create right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Birdnest_Create left, Message_Birdnest_Create right)
	{
		return !object.Equals(left, right);
	}

	public Message_Birdnest_Create()
	{
	}

	public Message_Birdnest_Create(Messages type, BirdsNest nest)
		: base(type, nest)
	{
		activeEggs = new bool[nest.eggs.Length];
		for (int i = 0; i < activeEggs.Length; i++)
		{
			activeEggs[i] = nest.eggs[i].activeInHierarchy;
		}
		activeFeathers = new bool[nest.feathers.Length];
		for (int j = 0; j < activeFeathers.Length; j++)
		{
			activeFeathers[j] = nest.feathers[j].activeInHierarchy;
		}
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(activeEggs, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(activeFeathers, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out activeEggs, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out activeFeathers, default(FastBufferWriter.ForPrimitives));
	}
}
