using System;
using Unity.Netcode;

[Serializable]
public class Message_OnEditText : Message_NetworkBehaviour_ID
{
	public string newText;

	protected bool Equals(Message_OnEditText other)
	{
		if (Equals((Message_NetworkBehaviour_ID)other))
		{
			return newText == other.newText;
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
		return Equals((Message_OnEditText)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), newText);
	}

	public static bool operator ==(Message_OnEditText left, Message_OnEditText right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_OnEditText left, Message_OnEditText right)
	{
		return !object.Equals(left, right);
	}

	public Message_OnEditText()
	{
	}

	public Message_OnEditText(Messages p_type, MonoBehaviour_Network p_behavior, TextWriterObject p_textWriter)
		: base(p_type, p_behavior, p_textWriter.ObjectIndex)
	{
		newText = p_textWriter.GetText();
	}

	public Message_OnEditText(Messages p_type, MonoBehaviour_Network p_behavior, string p_text, uint p_objectIndex)
		: base(p_type, p_behavior, p_objectIndex)
	{
		newText = p_text;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(newText);
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out newText, false);
	}
}
