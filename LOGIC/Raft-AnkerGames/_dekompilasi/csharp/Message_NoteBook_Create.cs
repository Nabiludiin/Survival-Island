using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_NoteBook_Create : Message_NetworkBehaviour_ID
{
	[SerializeField]
	private RGD_NoteBook rgd_noteBook;

	protected bool Equals(Message_NoteBook_Create other)
	{
		if (Equals((Message_NetworkBehaviour_ID)other))
		{
			return object.Equals(rgd_noteBook, other.rgd_noteBook);
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
		return Equals((Message_NoteBook_Create)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgd_noteBook);
	}

	public static bool operator ==(Message_NoteBook_Create left, Message_NoteBook_Create right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_NoteBook_Create left, Message_NoteBook_Create right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		rgd_noteBook.SerializeFast(writer);
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		rgd_noteBook = new RGD_NoteBook();
		rgd_noteBook.DeserializeFast(reader);
	}

	public Message_NoteBook_Create()
	{
	}

	public Message_NoteBook_Create(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, NoteBook p_noteBook)
		: base(type, behaviour, networkID_ObjectIndex)
	{
		rgd_noteBook = new RGD_NoteBook(p_noteBook);
	}

	public void RestoreNoteBook()
	{
		NoteBook value = ComponentManager<NoteBook>.Value;
		rgd_noteBook.RestoreNoteBook(value);
	}
}
