using System;
using Unity.Netcode;

[Serializable]
public class Message_Reciever_Antenna : Message_NetworkBehaviour_ID
{
	public uint antennaBlockIndex;

	protected bool Equals(Message_Reciever_Antenna other)
	{
		if (Equals((Message_NetworkBehaviour_ID)other))
		{
			return antennaBlockIndex == other.antennaBlockIndex;
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
		return Equals((Message_Reciever_Antenna)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), antennaBlockIndex);
	}

	public static bool operator ==(Message_Reciever_Antenna left, Message_Reciever_Antenna right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Reciever_Antenna left, Message_Reciever_Antenna right)
	{
		return !object.Equals(left, right);
	}

	public Message_Reciever_Antenna()
	{
	}

	public Message_Reciever_Antenna(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, uint antennaBlockIndex)
		: base(type, behaviour, networkID_ObjectIndex)
	{
		this.antennaBlockIndex = antennaBlockIndex;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in antennaBlockIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out antennaBlockIndex, default(FastBufferWriter.ForPrimitives));
	}
}
