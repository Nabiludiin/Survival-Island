using System;
using Unity.Netcode;

[Serializable]
public class Message_PlantSeed : Message_NetworkBehaviour
{
	public uint cropplotObjectIndex;

	public int plantUniqueIndex;

	public uint plantObjectIndex;

	public bool waterPlantedSeed;

	protected bool Equals(Message_PlantSeed other)
	{
		if (Equals((Message_NetworkBehaviour)other) && cropplotObjectIndex == other.cropplotObjectIndex && plantUniqueIndex == other.plantUniqueIndex && plantObjectIndex == other.plantObjectIndex)
		{
			return waterPlantedSeed == other.waterPlantedSeed;
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
		return Equals((Message_PlantSeed)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), cropplotObjectIndex, plantUniqueIndex, plantObjectIndex, waterPlantedSeed);
	}

	public static bool operator ==(Message_PlantSeed left, Message_PlantSeed right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_PlantSeed left, Message_PlantSeed right)
	{
		return !object.Equals(left, right);
	}

	public Message_PlantSeed()
	{
	}

	public Message_PlantSeed(Messages type, MonoBehaviour_Network behaviour, Cropplot cropplot, Plant plant, bool waterPlantedSeed)
		: base(type, behaviour)
	{
		plantUniqueIndex = plant.item.UniqueIndex;
		cropplotObjectIndex = cropplot.ObjectIndex;
		this.waterPlantedSeed = waterPlantedSeed;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(13))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in cropplotObjectIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in plantUniqueIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in plantObjectIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in waterPlantedSeed, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out cropplotObjectIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out plantUniqueIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out plantObjectIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out waterPlantedSeed, default(FastBufferWriter.ForPrimitives));
	}
}
