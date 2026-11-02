using System;
using System.Linq;
using Unity.Netcode;

[Serializable]
public class Message_Reciever_FrequencyUpdate : Message_NetworkBehaviour_ID
{
	public int[] frequencyNumbers;

	public int frequencyIndex;

	protected bool Equals(Message_Reciever_FrequencyUpdate other)
	{
		if (frequencyNumbers == null && other.frequencyNumbers != null)
		{
			return false;
		}
		if (frequencyNumbers != null && other.frequencyNumbers == null)
		{
			return false;
		}
		if (Equals((Message_NetworkBehaviour_ID)other) && ((frequencyNumbers == null && other.frequencyNumbers == null) || Enumerable.SequenceEqual(frequencyNumbers, other.frequencyNumbers)))
		{
			return frequencyIndex == other.frequencyIndex;
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
		return Equals((Message_Reciever_FrequencyUpdate)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), frequencyNumbers, frequencyIndex);
	}

	public static bool operator ==(Message_Reciever_FrequencyUpdate left, Message_Reciever_FrequencyUpdate right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Reciever_FrequencyUpdate left, Message_Reciever_FrequencyUpdate right)
	{
		return !object.Equals(left, right);
	}

	public Message_Reciever_FrequencyUpdate()
	{
	}

	public Message_Reciever_FrequencyUpdate(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, int[] frequencyNumbers, int frequencyIndex)
		: base(type, behaviour, networkID_ObjectIndex)
	{
		this.frequencyNumbers = frequencyNumbers;
		this.frequencyIndex = frequencyIndex;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(frequencyNumbers, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in frequencyIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out frequencyNumbers, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out frequencyIndex, default(FastBufferWriter.ForPrimitives));
	}
}
