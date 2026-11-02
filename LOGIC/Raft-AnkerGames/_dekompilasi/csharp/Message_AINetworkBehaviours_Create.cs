using System;
using System.Linq;
using Unity.Netcode;

[Serializable]
public class Message_AINetworkBehaviours_Create : Message_NetworkBehaviour
{
	public Message_NetworkBehaviour[] createMessages;

	protected bool Equals(Message_AINetworkBehaviours_Create other)
	{
		if (createMessages == null && other.createMessages != null)
		{
			return false;
		}
		if (createMessages != null && other.createMessages == null)
		{
			return false;
		}
		if (Equals((Message_NetworkBehaviour)other))
		{
			if (createMessages != null || other.createMessages != null)
			{
				return Enumerable.SequenceEqual(createMessages, other.createMessages);
			}
			return true;
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
		return Equals((Message_AINetworkBehaviours_Create)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), createMessages);
	}

	public static bool operator ==(Message_AINetworkBehaviours_Create left, Message_AINetworkBehaviours_Create right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_AINetworkBehaviours_Create left, Message_AINetworkBehaviours_Create right)
	{
		return !object.Equals(left, right);
	}

	public Message_AINetworkBehaviours_Create()
	{
	}

	public Message_AINetworkBehaviours_Create(Messages type, MonoBehaviour_Network behaviour)
		: base(type, behaviour)
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<int>(createMessages.Length, default(FastBufferWriter.ForPrimitives));
		Message_NetworkBehaviour[] array = createMessages;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SerializeFast(writer);
		}
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		int value = 0;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		createMessages = new Message_NetworkBehaviour[value];
		for (int i = 0; i < value; i++)
		{
			createMessages[i] = FastMessageDeserializer.DeserializeMessage<Message_NetworkBehaviour>(reader);
		}
	}
}
