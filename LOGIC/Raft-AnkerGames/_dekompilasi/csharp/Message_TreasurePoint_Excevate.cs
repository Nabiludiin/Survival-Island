using System;
using Unity.Netcode;

[Serializable]
public class Message_TreasurePoint_Excevate : Message_NetworkBehaviour_ID
{
	public int exceAmount;

	protected bool Equals(Message_TreasurePoint_Excevate other)
	{
		if (Equals((Message_NetworkBehaviour_ID)other))
		{
			return exceAmount == other.exceAmount;
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
		return Equals((Message_TreasurePoint_Excevate)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), exceAmount);
	}

	public static bool operator ==(Message_TreasurePoint_Excevate left, Message_TreasurePoint_Excevate right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_TreasurePoint_Excevate left, Message_TreasurePoint_Excevate right)
	{
		return !object.Equals(left, right);
	}

	public Message_TreasurePoint_Excevate()
	{
	}

	public Message_TreasurePoint_Excevate(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, int excevate)
		: base(type, behaviour, networkID_ObjectIndex)
	{
		exceAmount = excevate;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in exceAmount, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out exceAmount, default(FastBufferWriter.ForPrimitives));
	}
}
