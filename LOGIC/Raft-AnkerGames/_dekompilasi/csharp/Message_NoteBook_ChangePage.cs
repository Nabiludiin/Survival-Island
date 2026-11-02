using System;
using Unity.Netcode;

[Serializable]
public class Message_NoteBook_ChangePage : Message_NetworkBehaviour_ID_SteamID
{
	public int pageIndex;

	protected bool Equals(Message_NoteBook_ChangePage other)
	{
		if (Equals((Message_NetworkBehaviour_ID_SteamID)other))
		{
			return pageIndex == other.pageIndex;
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
		return Equals((Message_NoteBook_ChangePage)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), pageIndex);
	}

	public static bool operator ==(Message_NoteBook_ChangePage left, Message_NoteBook_ChangePage right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_NoteBook_ChangePage left, Message_NoteBook_ChangePage right)
	{
		return !object.Equals(left, right);
	}

	public Message_NoteBook_ChangePage()
	{
	}

	public Message_NoteBook_ChangePage(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, int p_pageIndex, ulong p_steamID)
		: base(type, behaviour, networkID_ObjectIndex, p_steamID)
	{
		pageIndex = p_pageIndex;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in pageIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out pageIndex, default(FastBufferWriter.ForPrimitives));
	}
}
