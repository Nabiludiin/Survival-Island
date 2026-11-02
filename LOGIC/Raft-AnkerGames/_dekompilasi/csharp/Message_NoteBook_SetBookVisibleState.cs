using System;
using Unity.Netcode;

[Serializable]
public class Message_NoteBook_SetBookVisibleState : Message_NetworkBehaviour_ID_SteamID
{
	public bool setEnabled;

	protected bool Equals(Message_NoteBook_SetBookVisibleState other)
	{
		if (Equals((Message_NetworkBehaviour_ID_SteamID)other))
		{
			return setEnabled == other.setEnabled;
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
		return Equals((Message_NoteBook_SetBookVisibleState)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), setEnabled);
	}

	public static bool operator ==(Message_NoteBook_SetBookVisibleState left, Message_NoteBook_SetBookVisibleState right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_NoteBook_SetBookVisibleState left, Message_NoteBook_SetBookVisibleState right)
	{
		return !object.Equals(left, right);
	}

	public Message_NoteBook_SetBookVisibleState()
	{
	}

	public Message_NoteBook_SetBookVisibleState(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, bool p_setEnabled, ulong p_steamID)
		: base(type, behaviour, networkID_ObjectIndex, p_steamID)
	{
		setEnabled = p_setEnabled;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in setEnabled, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out setEnabled, default(FastBufferWriter.ForPrimitives));
	}
}
