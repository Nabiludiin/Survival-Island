using System;
using Unity.Netcode;

[Serializable]
public class Message_InteractableButtonPress_Integer : Message_InteractableButtonPress
{
	public int integer;

	protected bool Equals(Message_InteractableButtonPress_Integer other)
	{
		if (Equals((Message_InteractableButtonPress)other))
		{
			return integer == other.integer;
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
		return Equals((Message_InteractableButtonPress_Integer)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), integer);
	}

	public static bool operator ==(Message_InteractableButtonPress_Integer left, Message_InteractableButtonPress_Integer right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_InteractableButtonPress_Integer left, Message_InteractableButtonPress_Integer right)
	{
		return !object.Equals(left, right);
	}

	public Message_InteractableButtonPress_Integer()
	{
	}

	public Message_InteractableButtonPress_Integer(MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, int buttonIndex, Network_Player interactingPlayer, int integer)
		: base(behaviour, networkID_ObjectIndex, buttonIndex, interactingPlayer, Messages.InteractableButtonPress_Int)
	{
		this.integer = integer;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in integer, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out integer, default(FastBufferWriter.ForPrimitives));
	}
}
