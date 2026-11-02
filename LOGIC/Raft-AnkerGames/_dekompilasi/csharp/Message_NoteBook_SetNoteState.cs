using System;
using Unity.Netcode;

[Serializable]
public class Message_NoteBook_SetNoteState : Message_NetworkBehaviour_ID
{
	public int noteIndex;

	public bool isUnlocked;

	public bool showNotification;

	protected bool Equals(Message_NoteBook_SetNoteState other)
	{
		if (Equals((Message_NetworkBehaviour_ID)other) && noteIndex == other.noteIndex && isUnlocked == other.isUnlocked)
		{
			return showNotification == other.showNotification;
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
		return Equals((Message_NoteBook_SetNoteState)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), noteIndex, isUnlocked, showNotification);
	}

	public static bool operator ==(Message_NoteBook_SetNoteState left, Message_NoteBook_SetNoteState right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_NoteBook_SetNoteState left, Message_NoteBook_SetNoteState right)
	{
		return !object.Equals(left, right);
	}

	public Message_NoteBook_SetNoteState()
	{
	}

	public Message_NoteBook_SetNoteState(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, int p_noteIndex, bool p_isUnlocked, bool showNotification)
		: base(type, behaviour, networkID_ObjectIndex)
	{
		noteIndex = p_noteIndex;
		isUnlocked = p_isUnlocked;
		this.showNotification = showNotification;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(6))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in noteIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in isUnlocked, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in showNotification, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out noteIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out isUnlocked, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out showNotification, default(FastBufferWriter.ForPrimitives));
	}
}
