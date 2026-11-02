using System;
using Unity.Netcode;

[Serializable]
public class Message_Plant_Complete : Message_NetworkBehaviour
{
	public uint cropplotObjectIndex;

	public int plantationSlotIndex;

	protected bool Equals(Message_Plant_Complete other)
	{
		if (Equals((Message_NetworkBehaviour)other) && cropplotObjectIndex == other.cropplotObjectIndex)
		{
			return plantationSlotIndex == other.plantationSlotIndex;
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
		return Equals((Message_Plant_Complete)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), cropplotObjectIndex, plantationSlotIndex);
	}

	public static bool operator ==(Message_Plant_Complete left, Message_Plant_Complete right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Plant_Complete left, Message_Plant_Complete right)
	{
		return !object.Equals(left, right);
	}

	public Message_Plant_Complete()
	{
	}

	public Message_Plant_Complete(Messages type, MonoBehaviour_Network behaviour, Cropplot cropplot, Plant plant)
		: base(type, behaviour)
	{
		cropplotObjectIndex = cropplot.ObjectIndex;
		plantationSlotIndex = plant.plantationSlotIndex;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in cropplotObjectIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in plantationSlotIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out cropplotObjectIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out plantationSlotIndex, default(FastBufferWriter.ForPrimitives));
	}
}
