using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_Compound : Message
{
	public List<Message> messages;

	protected bool Equals(Message_Compound other)
	{
		int num = 0;
		foreach (Message message in messages)
		{
			if (!message.Equals(other.messages[num]))
			{
				Debug.Log("Error missmatch message");
			}
			num++;
		}
		if (Equals((Message)other))
		{
			return messages.SequenceEqual(other.messages);
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
		return Equals((Message_Compound)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), messages);
	}

	public static bool operator ==(Message_Compound left, Message_Compound right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Compound left, Message_Compound right)
	{
		return !object.Equals(left, right);
	}

	public Message_Compound()
	{
	}

	public Message_Compound(List<Message> messages)
		: base(Messages.Compound)
	{
		this.messages = messages;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		foreach (Message message in messages)
		{
			message.SerializeFast(writer);
		}
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		messages = new List<Message>();
		while (reader.Position < reader.Length)
		{
			Message item = FastMessageDeserializer.DeserializeMessage<Message>(reader);
			messages.Add(item);
		}
	}
}
