using System;
using Unity.Netcode;

[Serializable]
public class Message_Snowmobile_TakeSeat : Message_NetworkBehaviour_ID_SteamID
{
	public int seatIndex = -1;

	protected bool Equals(Message_Snowmobile_TakeSeat other)
	{
		if (Equals((Message_NetworkBehaviour_ID_SteamID)other))
		{
			return seatIndex == other.seatIndex;
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
		return Equals((Message_Snowmobile_TakeSeat)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), seatIndex);
	}

	public static bool operator ==(Message_Snowmobile_TakeSeat left, Message_Snowmobile_TakeSeat right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Snowmobile_TakeSeat left, Message_Snowmobile_TakeSeat right)
	{
		return !object.Equals(left, right);
	}

	public Message_Snowmobile_TakeSeat()
	{
	}

	public Message_Snowmobile_TakeSeat(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, ulong steamID, int seatIndex = -1)
		: base(type, behaviour, networkID_ObjectIndex, steamID)
	{
		this.seatIndex = seatIndex;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in seatIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out seatIndex, default(FastBufferWriter.ForPrimitives));
	}
}
