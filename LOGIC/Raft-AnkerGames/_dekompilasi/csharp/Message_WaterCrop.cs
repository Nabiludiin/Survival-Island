using System;
using Unity.Netcode;

[Serializable]
public class Message_WaterCrop : Message_NetworkBehaviour
{
	public uint cropplotObjectIndex;

	public bool playSound;

	protected bool Equals(Message_WaterCrop other)
	{
		if (Equals((Message_NetworkBehaviour)other) && cropplotObjectIndex == other.cropplotObjectIndex)
		{
			return playSound == other.playSound;
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
		return Equals((Message_WaterCrop)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), cropplotObjectIndex, playSound);
	}

	public static bool operator ==(Message_WaterCrop left, Message_WaterCrop right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_WaterCrop left, Message_WaterCrop right)
	{
		return !object.Equals(left, right);
	}

	public Message_WaterCrop()
	{
	}

	public Message_WaterCrop(Messages type, MonoBehaviour_Network behaviour, Cropplot cropplot, bool playSound)
		: base(type, behaviour)
	{
		cropplotObjectIndex = cropplot.ObjectIndex;
		this.playSound = playSound;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in cropplotObjectIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in playSound, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out cropplotObjectIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out playSound, default(FastBufferWriter.ForPrimitives));
	}
}
