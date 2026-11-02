using System;
using Unity.Netcode;

[Serializable]
public class Message_Dialogue : Message_NetworkBehaviour_ID_SteamID
{
	public int voiceDataIndex;

	public int voiceLineIndex;

	protected bool Equals(Message_Dialogue other)
	{
		if (Equals((Message_NetworkBehaviour_ID_SteamID)other) && voiceDataIndex == other.voiceDataIndex)
		{
			return voiceLineIndex == other.voiceLineIndex;
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
		return Equals((Message_Dialogue)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), voiceDataIndex, voiceLineIndex);
	}

	public static bool operator ==(Message_Dialogue left, Message_Dialogue right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Dialogue left, Message_Dialogue right)
	{
		return !object.Equals(left, right);
	}

	public Message_Dialogue()
	{
	}

	public Message_Dialogue(Messages type, MonoBehaviour_Network behaviour, Network_UserId steamID, uint networkID_ObjectIndex, int voiceDataIndex, int voiceLineIndex = -1)
		: base(type, behaviour, networkID_ObjectIndex, steamID)
	{
		this.voiceDataIndex = voiceDataIndex;
		this.voiceLineIndex = voiceLineIndex;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in voiceDataIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in voiceLineIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out voiceDataIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out voiceLineIndex, default(FastBufferWriter.ForPrimitives));
	}
}
